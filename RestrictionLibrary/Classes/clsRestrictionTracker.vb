Public Class localRestrictionTracker
  Implements IDisposable
  Public Enum SatHostTypes
    WildBlue_LEGACY    ' Down, Up, DownLim, UpLim     [TYPEA]
    WildBlue_EXEDE     ' Used, Limit                  [TYPEB]
    RuralPortal_LEGACY ' Down, Up, DownLim, UpLim     [TYPEA]
    RuralPortal_EXEDE  ' Used, Limit                  [TYPEB]
    DishNet_EXEDE      ' AnyTime, AnyTimeLim, OffPeak, OffPeakLim [TYPEA2]
    Other
  End Enum
  Public Enum ConnectionStates
    Initialize
    Prepare
    Login
    TableDownload
    TableRead
  End Enum
  Public Enum ConnectionSubStates
    None
    ReadLogin
    AuthPrepare
    Authenticate
    AuthenticateRetry
    Verify
    LoadHome
    LoadAJAX
    LoadAJAXRetry
    LoadTable
    LoadTableRetry
  End Enum
#Region "Events"
  Public Class ConnectionFailureEventArgs
    Inherits EventArgs
    Public Enum FailureType
      UnknownAccountType
      UnknownAccountDetails
      LoginIssue
      LoginFailure
      FatalLoginFailure
      ConnectionTimeout
    End Enum
    Private m_FailType As FailureType
    Private m_Fail As String
    Private m_Message As String
    Public Sub New(ftFailType As FailureType, Optional sMessage As String = Nothing, Optional sFailure As String = Nothing)
      m_FailType = ftFailType
      m_Message = sMessage
      m_Fail = sFailure
    End Sub
    Public ReadOnly Property Fail As String
      Get
        Return m_Fail
      End Get
    End Property
    Public ReadOnly Property Message As String
      Get
        Return m_Message
      End Get
    End Property
    Public ReadOnly Property [Type] As FailureType
      Get
        Return m_FailType
      End Get
    End Property
  End Class
  Public Event ConnectionFailure(sender As Object, e As ConnectionFailureEventArgs)
  Public Class TYPEAResultEventArgs
    Inherits EventArgs
    Private m_Down As Long
    Private m_Up As Long
    Private m_DownLim As Long
    Private m_UpLim As Long
    Private m_Update As Date
    Public Sub New(lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long, dUpdate As Date)
      m_Down = lDown
      m_Up = lUp
      m_DownLim = lDownLim
      m_UpLim = lUpLim
      m_Update = dUpdate
    End Sub
    Public ReadOnly Property Download As Long
      Get
        Return m_Down
      End Get
    End Property
    Public ReadOnly Property DownloadLimit As Long
      Get
        Return m_DownLim
      End Get
    End Property
    Public ReadOnly Property Upload As Long
      Get
        Return m_Up
      End Get
    End Property
    Public ReadOnly Property UploadLimit As Long
      Get
        Return m_UpLim
      End Get
    End Property
    Public ReadOnly Property Update As Date
      Get
        Return m_Update
      End Get
    End Property
  End Class
  Public Class TYPEA2ResultEventArgs
    Inherits EventArgs
    Private m_AnyTime As Long
    Private m_AnyTimeLim As Long
    Private m_OffPeak As Long
    Private m_OffPeakLim As Long
    Private m_Update As Date
    Public Sub New(lAnyTime As Long, lAnyTimeLim As Long, lOffPeak As Long, lOffPeakLim As Long, dUpdate As Date)
      m_AnyTime = lAnyTime
      m_AnyTimeLim = lAnyTimeLim
      m_OffPeak = lOffPeak
      m_OffPeakLim = lOffPeakLim
      m_Update = dUpdate
    End Sub
    Public ReadOnly Property AnyTime As Long
      Get
        Return m_AnyTime
      End Get
    End Property
    Public ReadOnly Property AnyTimeLimit As Long
      Get
        Return m_AnyTimeLim
      End Get
    End Property
    Public ReadOnly Property OffPeak As Long
      Get
        Return m_OffPeak
      End Get
    End Property
    Public ReadOnly Property OffPeakLimit As Long
      Get
        Return m_OffPeakLim
      End Get
    End Property
    Public ReadOnly Property Update As Date
      Get
        Return m_Update
      End Get
    End Property
  End Class
  Public Class TYPEBResultEventArgs
    Inherits EventArgs
    Private m_Used As Long
    Private m_Limit As Long
    Private m_Update As Date
    Public Sub New(lUsed As Long, lLimit As Long, dUpdate As Date)
      m_Used = lUsed
      m_Limit = lLimit
      m_Update = dUpdate
    End Sub
    Public ReadOnly Property Used As Long
      Get
        Return m_Used
      End Get
    End Property
    Public ReadOnly Property Limit As Long
      Get
        Return m_Limit
      End Get
    End Property
    Public ReadOnly Property Update As Date
      Get
        Return m_Update
      End Get
    End Property
  End Class
  Public Event ConnectionWBLResult(sender As Object, e As TYPEAResultEventArgs)
  Public Event ConnectionWBXResult(sender As Object, e As TYPEBResultEventArgs)
  Public Event ConnectionDNXResult(sender As Object, e As TYPEA2ResultEventArgs)
  Public Event ConnectionRPLResult(sender As Object, e As TYPEAResultEventArgs)
  Public Event ConnectionRPXResult(sender As Object, e As TYPEBResultEventArgs)
  Public Class ConnectionStatusEventArgs
    Inherits EventArgs
    Private m_state As ConnectionStates
    Private m_substate As ConnectionSubStates
    Private m_stage As Integer
    Public Sub New(status As ConnectionStates, Optional substate As ConnectionSubStates = ConnectionSubStates.None, Optional stage As Integer = 0)
      m_state = status
      m_substate = substate
      m_stage = stage
    End Sub
    Public ReadOnly Property Status As ConnectionStates
      Get
        Return m_state
      End Get
    End Property
    Public ReadOnly Property SubState As ConnectionSubStates
      Get
        Return m_substate
      End Get
    End Property
    Public ReadOnly Property Stage As Integer
      Get
        Return m_stage
      End Get
    End Property
  End Class
  Public Event ConnectionStatus(sender As Object, e As ConnectionStatusEventArgs)
#End Region
  Private acType As DetermineType
  Private mySettings As AppSettings
  Private Const MBPerGB As Integer = 1000
  Private Const sWB As String = "https://myaccount.{0}/wbisp/{2}/{1}.jsp"
  Private Const sRP As String = "https://{0}.ruralportal.net/us/{1}.do"
  Private sAccount, sUsername, sPassword, sProvider As String
  Private sAttemptedURL As String
  Private AttemptedTag As ConnectionStates
  Private AttemptedSub As ConnectionSubStates
  Private AttemptedStage As Integer
  Private imSlowed As Boolean
  Private imFree As Boolean
  Private FullCheck As Boolean = True
  Private ClosingTime As Boolean
  Private tConnect As Threading.Thread
  Private c_Timeout As Integer
  Private c_Proxy As Net.IWebProxy
  Private c_Jar As Net.CookieContainer
  Private sDataPath As String
  Private wsSocket As WebClientEx
#Region "Initialization Functions"
  Public Sub New(ConfigPath As String)
    ClosingTime = False
    sDataPath = ConfigPath
    If mySettings Is Nothing Then mySettings = New AppSettings(ConfigPath & IO.Path.DirectorySeparatorChar.ToString & "user.config")
    Dim useProtocol As SecurityProtocolTypeEx = SecurityProtocolTypeEx.None
    For Each protocolTest In [Enum].GetValues(GetType(SecurityProtocolTypeEx))
      If (mySettings.SecurityProtocol And protocolTest) = protocolTest Then
        Try
          Net.ServicePointManager.SecurityProtocol = protocolTest
          useProtocol = useProtocol Or protocolTest
        Catch ex As Exception
        End Try
      End If
    Next
    If useProtocol = SecurityProtocolTypeEx.None Then
      For Each protocolTest In [Enum].GetValues(GetType(SecurityProtocolTypeEx))
        Try
          Net.ServicePointManager.SecurityProtocol = protocolTest
          useProtocol = useProtocol Or protocolTest
        Catch ex As Exception
        End Try
      Next
    End If
    Try
      Net.ServicePointManager.SecurityProtocol = useProtocol
    Catch ex As Exception
    End Try
    Net.ServicePointManager.ServerCertificateValidationCallback = New Net.Security.RemoteCertificateValidationCallback(AddressOf IgnoreCert)
    sAccount = mySettings.Account
    If Not String.IsNullOrEmpty(mySettings.PassCrypt) Then
      sPassword = StoredPassword.DecryptApp(mySettings.PassCrypt)
    End If
    If Not String.IsNullOrEmpty(sAccount) AndAlso (sAccount.Contains("@") And sAccount.Contains(".")) Then
      sUsername = sAccount.Substring(0, sAccount.LastIndexOf("@"))
      sProvider = sAccount.Substring(sAccount.LastIndexOf("@") + 1).ToLower
    Else
      sUsername = String.Empty
      sAccount = String.Empty
      sProvider = String.Empty
    End If
    c_Timeout = mySettings.Timeout
    c_Proxy = mySettings.Proxy
    tConnect = New Threading.Thread(AddressOf Connect)
    tConnect.Start()
  End Sub
  Private Sub Connect()
    If mySettings.AccountType = SatHostTypes.Other Then
      If mySettings.Account.Contains("@") Then
        acType = New DetermineType(mySettings.Account.Substring(mySettings.Account.IndexOf("@") + 1), mySettings.Timeout, mySettings.Proxy, AddressOf acType_TypeDetermined)
      Else
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.UnknownAccountType))
      End If
    Else
      GetUsage()
    End If
  End Sub
  Private Sub acType_TypeDetermined(HostGroup As DetermineType.SatHostGroup)
    Select Case HostGroup
      Case DetermineType.SatHostGroup.WildBlue
        mySettings.AccountType = SatHostTypes.WildBlue_LEGACY
        GetUsage()
      Case DetermineType.SatHostGroup.DishNet
        mySettings.AccountType = SatHostTypes.DishNet_EXEDE
        GetUsage()
      Case DetermineType.SatHostGroup.RuralPortal
        mySettings.AccountType = SatHostTypes.RuralPortal_EXEDE
        GetUsage()
      Case DetermineType.SatHostGroup.Exede
        mySettings.AccountType = SatHostTypes.WildBlue_EXEDE
        GetUsage()
      Case DetermineType.SatHostGroup.Other
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.UnknownAccountType))
    End Select
  End Sub
#End Region
#Region "Login Functions"
  Private Sub GetUsage()
    If String.IsNullOrEmpty(sUsername) Or String.IsNullOrEmpty(sPassword) Then
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.UnknownAccountDetails))
    Else
      RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Initialize))
      Login()
    End If
  End Sub
  Private Sub Login()
    c_Jar = New Net.CookieContainer
    Select Case mySettings.AccountType
      Case SatHostTypes.WildBlue_LEGACY : LoginWB()
      Case SatHostTypes.WildBlue_EXEDE : LoginExede()
      Case SatHostTypes.RuralPortal_LEGACY, SatHostTypes.RuralPortal_EXEDE : LoginRP()
      Case SatHostTypes.DishNet_EXEDE : LoginDN()
    End Select
  End Sub
  Private Sub LoginWB()
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Prepare))
    Dim uriString As String = String.Format(sWB, sProvider, "servLogin", IIf(sProvider.ToLower = "exede.net", "exede.com", sProvider))
    MakeSocket()
    Dim sSend As String = "uid=" & PercentEncode(sUsername) & "&userPassword=" & PercentEncode(sPassword)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, uriString)
    Dim sRet As String = wsSocket.UploadString(uriString, "POST", sSend)
    If ClosingTime Then Return
    WB_Login_Response(sRet, wsSocket.ResponseURI)
  End Sub
  Private Sub LoginExede()
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Prepare))
    Dim uriString As String = "https://my.exede.net/login"
    MakeSocket()
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.ReadLogin, 0, uriString)
    Dim sRet As String = wsSocket.DownloadString(uriString)
    If ClosingTime Then Return
    EX_Login_Prepare_Response(sRet, wsSocket.ResponseURI, 0)
  End Sub
  Private Sub LoginRP()
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Prepare))
    If sProvider.Contains(".") Then sProvider = sProvider.Substring(0, sProvider.LastIndexOf("."))
    Dim uriString As String = String.Format(sRP, sProvider, "login")
    MakeSocket()
    Dim sSend As String = "warningTrip=false&userName=" & PercentEncode(sUsername) & "&passwd=" & PercentEncode(sPassword)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, uriString)
    Dim sRet As String = wsSocket.UploadString(uriString, "POST", sSend)
    If ClosingTime Then Return
    RP_Login_Response(sRet, wsSocket.ResponseURI, False)
  End Sub
  Private Sub LoginDN()
    iHist = 0
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Prepare))
    Dim uriString As String = "https://my.dish.com/customercare/myaccount/myinternet"
    MakeSocket()
    wsSocket.ManualRedirect = False
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.ReadLogin, 0, uriString)
    Dim sRet As String = wsSocket.DownloadString(uriString)
    If ClosingTime Then Return
    iHist += 1
    DN_Login_Prep_Response(sRet, wsSocket.ResponseURI)
  End Sub
#End Region
#Region "Parsing Functions"
  Private Sub ReadUsage(Table As String)
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.TableRead))
    Select Case mySettings.AccountType
      Case SatHostTypes.WildBlue_LEGACY : WB_Read_Table(Table)
      Case SatHostTypes.WildBlue_EXEDE : EX_Read_Table(Table)
      Case SatHostTypes.RuralPortal_LEGACY, SatHostTypes.RuralPortal_EXEDE : RP_Read_Table(Table)
      Case SatHostTypes.DishNet_EXEDE : DN_Read_Table(Table)
    End Select
    c_Jar = New Net.CookieContainer
    SendSocketErrors(sDataPath)
  End Sub
#Region "WB"
  Private Sub WB_Login_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Response.Contains("usage.jsp") Then
      WB_Usage("usage")
    ElseIf Response.Contains("usage_bm.jsp") Then
      WB_Usage("usage_bm")
    ElseIf Response.Contains("<div class=""error"">") Then
      Dim sMessage As String = Response.Substring(Response.IndexOf("<div class=""error"">"))
      If sMessage.Contains("<b>") Then
        sMessage = sMessage.Substring(sMessage.IndexOf("<b>") + 3)
        sMessage = sMessage.Substring(0, sMessage.IndexOf("<"))
        RaiseError("Login Failed: " & sMessage)
      ElseIf sMessage.Contains("my.exede.net") Then
        RaiseError("Login Failed: You must create an account at the new Exede Portal.")
      Else
        RaiseError("Login Failed: Could not understand error.", "WB Login Response Error", ResponseURI.OriginalString & vbNewLine & Response, True)
      End If
    ElseIf Response.Contains("https://my.exede.net/usage") Then
      mySettings.AccountType = SatHostTypes.WildBlue_EXEDE
      RaiseError("Login Redirect: Exede account detected.")
      GetUsage()
    Else
      RaiseError("Login Failed: Could not understand response.", "WB Login Response Error", ResponseURI.OriginalString & vbNewLine & Response, True)
    End If
  End Sub
  Private Sub WB_Usage(File As String)
    MakeSocket()
    Dim uriString As String = String.Format(sWB, sProvider, File, IIf(sProvider.ToLower = "exede.net", "exede.com", sProvider))
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTable, 0, uriString)
    Dim sRet As String = wsSocket.DownloadString(uriString)
    If ClosingTime Then Return
    WB_Usage_Response(sRet, wsSocket.ResponseURI)
  End Sub
  Private Sub WB_Usage_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "myaccount." & sProvider Then
      RaiseError("Usage Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If Response.Contains("Usage Meter") Then
      Dim sFind As String = Response.Substring(Response.IndexOf("Usage Meter"))
      If sFind.Contains("<strong>Slowed Speed</strong>") Then imSlowed = True
      If sFind.Contains("<table") Then
        sFind = sFind.Substring(sFind.IndexOf("<table"))
        Dim iSearch As Integer = 0
        If sFind.ToLower.Contains("buy more") Then
          iSearch = sFind.ToLower.IndexOf("buy more") + 13
        End If
        If sFind.Substring(iSearch).Contains("</table>") Then
          sFind = sFind.Substring(0, sFind.IndexOf("</table>", iSearch))
          ReadUsage(sFind)
        Else
          RaiseError("Usage Failed: Could not parse usage meter.", "WB Usage Response Error", ResponseURI.OriginalString & vbNewLine & Response)
        End If
      ElseIf sFind.Contains("FREEDOM") AndAlso sFind.Contains("Current Usage<strong>:") Then
        sFind = sFind.Substring(sFind.IndexOf("FREEDOM"))
        If sFind.Contains("</td>") Then
          sFind = sFind.Substring(0, sFind.IndexOf("</td>"))
          ReadUsage(sFind)
        Else
          RaiseError("Usage Failed: Could not parse usage meter.", "WB Usage Response Error (Exede Freedom)", ResponseURI.OriginalString & vbNewLine & Response)
        End If
      ElseIf sFind.Contains("At this time, your usage is not being counted toward your data allowance.") Then
        imFree = True
        RaiseError("Your usage is not being metered at this time!")
      Else
        RaiseError("Usage Failed: Could not find usage meter.", "WB Usage Response Error", ResponseURI.OriginalString & vbNewLine & Response)
      End If
    ElseIf Response.Contains("<div class=""error"">") Then
      Dim sMessage As String = Response.Substring(Response.IndexOf("<div class=""error"">"))
      If sMessage.Contains("<b>") Then
        sMessage = sMessage.Substring(sMessage.IndexOf("<b>") + 3)
        sMessage = sMessage.Substring(0, sMessage.IndexOf("<"))
        RaiseError("Usage Failed: " & sMessage)
      Else
        RaiseError("Usage Failed: Could not find usage meter.", "WB Usage Response Error (Error DIV but no BOLD text)", ResponseURI.OriginalString & vbNewLine & Response)
      End If
    Else
      Dim sErr As String = "Could not find usage meter."
      If Response.Contains("Oops") Then
        sErr = Response.Substring(Response.IndexOf("Oops"))
        If sErr.Contains("</h3>") Then
          sErr = sErr.Substring(0, sErr.IndexOf("</h3>"))
          If sErr = "Oops. We're having a problem displaying your usage information." Then sErr = "Data temporarily unavailable."
        ElseIf sErr.Contains("<hr>") Then
          sErr = sErr.Substring(sErr.IndexOf("<hr>") + 4)
          sErr = sErr.Substring(0, sErr.IndexOf("<hr>"))
        End If
        If sErr.Contains("<!-") Then
          If sErr.Contains("->") Then
            sErr = sErr.Substring(0, sErr.IndexOf("<!-")) & sErr.Substring(sErr.IndexOf("->") + 2)
          Else
          End If
        End If
        RaiseError("Usage Failed: " & sErr, "WB Usage Response Error (Oops)", ResponseURI.OriginalString & vbNewLine & Response)
      Else
        RaiseError("Usage Failed: " & sErr, "WB Usage Response Error", ResponseURI.OriginalString & vbNewLine & Response)
      End If
    End If
  End Sub
  Private Sub WB_Read_Table(Table As String)
    Dim sRows As String() = Split(Table, vbLf)
    Dim sDown As String = String.Empty, sDownT As String = String.Empty, sUp As String = String.Empty, sUpT As String = String.Empty
    If Table.Contains("threshold") Then
      For Each row In sRows
        If Not String.IsNullOrEmpty(row) Then
          If row.Contains("<b>") And row.Contains("</b>") Then
            If String.IsNullOrEmpty(sDown) Then
              sDown = row.Substring(row.IndexOf("<b>") + 3)
              sDown = sDown.Substring(0, sDown.IndexOf("</b>"))
            ElseIf String.IsNullOrEmpty(sUp) Then
              sUp = row.Substring(row.IndexOf("<b>") + 3)
              sUp = sUp.Substring(0, sUp.IndexOf("</b>"))
            ElseIf String.IsNullOrEmpty(sDownT) Then
              sDownT = row.Substring(row.IndexOf("<b>") + 3)
              sDownT = sDownT.Substring(0, sDownT.IndexOf("</b>"))
            ElseIf String.IsNullOrEmpty(sUpT) Then
              sUpT = row.Substring(row.IndexOf("<b>") + 3)
              sUpT = sUpT.Substring(0, sUpT.IndexOf("</b>"))
              Exit For
            End If
          End If
        End If
      Next
      If String.IsNullOrEmpty(sDownT) Or String.IsNullOrEmpty(sUpT) Then
        RaiseError("Usage Read Failed: Unable to parse data!", , , , Table)
      Else
        RaiseEvent ConnectionWBLResult(Me, New TYPEAResultEventArgs(StrToVal(sDown), StrToVal(sDownT), StrToVal(sUp), StrToVal(sUpT), Now))
      End If
    Else
      RaiseError("Usage Read Failed: Unable to locate data table!", , , , Table)
    End If
  End Sub
#End Region
#Region "EX"
  Private Sub EX_Login_Prepare_Response(Response As String, ResponseURI As Uri, TryCount As Integer)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "mysso.exede.net" Then
      RaiseError("Login Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower = "/federation/ssoredirect/metaalias/idp" Then
      RaiseError("Login Failed: Could not understand response.", "EX Login Prepare Response Error", ResponseURI.OriginalString & vbNewLine & Response)
      Return
    End If
    If Response.ToLower.Contains("unable to process request") Then
      RaiseError("Prepare Failed: The server may be down.")
      Return
    End If
    If Not Response.Contains("<form") Or Not Response.Contains("name=""Login""") Then
      RaiseError("Prepare Failed: Login form not found.", "EX Login Prepare Response Error", ResponseURI.OriginalString & vbNewLine & Response)
      Return
    End If
    Dim sURI As String = Response.Substring(Response.IndexOf("name=""Login"""))
    sURI = sURI.Substring(sURI.IndexOf("action=""") + 8)
    sURI = sURI.Substring(0, sURI.IndexOf(""""))
    If sURI.StartsWith("/") Then sURI = "https://" & ResponseURI.Host & sURI
    Dim sGOTO As String = Nothing
    If Response.Contains("<input type=""hidden"" name=""goto"" value=""") Then
      sGOTO = Response.Substring(Response.IndexOf("<input type=""hidden"" name=""goto"" value="""))
      sGOTO = sGOTO.Substring(sGOTO.IndexOf("value=""") + 7)
      If sGOTO.Contains(""" />") Then
        sGOTO = sGOTO.Substring(0, sGOTO.IndexOf(""" />"))
      ElseIf sGOTO.Contains("""") Then
        sGOTO = sGOTO.Substring(0, sGOTO.IndexOf(""""))
      End If
    End If
    If String.IsNullOrEmpty(sGOTO) Then
      RaiseError("Prepare Failed: GOTO value not found.", "EX Login Prepare Response Error", ResponseURI.OriginalString & vbNewLine & Response)
      Return
    End If
    Dim sSQPS As String = Nothing
    If Response.Contains("<input type=""hidden"" name=""SunQueryParamsString"" value=""") Then
      sSQPS = Response.Substring(Response.IndexOf("<input type=""hidden"" name=""SunQueryParamsString"" value="""))
      sSQPS = sSQPS.Substring(sSQPS.IndexOf("value=""") + 7)
      If sSQPS.Contains(""" />") Then
        sSQPS = sSQPS.Substring(0, sSQPS.IndexOf(""" />"))
      ElseIf sSQPS.Contains("""") Then
        sSQPS = sSQPS.Substring(0, sSQPS.IndexOf(""""))
      End If
    End If
    If String.IsNullOrEmpty(sSQPS) Then
      RaiseError("Prepare Failed: SunQueryParamsString value not found.", "Exede Login Prepare Response Error", ResponseURI.OriginalString & vbNewLine & Response)
      Return
    End If
    EX_Login(sURI, sGOTO, sSQPS, TryCount)
  End Sub
  Private Sub EX_Login(sURI As String, sGOTO As String, sSQPS As String, TryCount As Integer)
    MakeSocket()
    Dim sSend As String = "realm=" & PercentEncode("/") &
                         "&IDToken1=" & PercentEncode(sUsername) &
                         "&IDToken2=" & PercentEncode(sPassword) &
                         "&IDButton=Sign+in" &
                         "&goto=" & PercentEncode(sGOTO) &
                         "&SunQueryParamsString=" & PercentEncode(sSQPS) &
                         "&encoded=true" &
                         "&gx_charset=UTF-8"
    If TryCount = 0 Then
      BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, sURI)
    Else
      BeginAttempt(ConnectionStates.Login, ConnectionSubStates.AuthenticateRetry, TryCount, sURI)
    End If
    Dim sRet As String = wsSocket.UploadString(sURI, "POST", sSend)
    If ClosingTime Then Return
    EX_Login_Response(sRet, wsSocket.ResponseURI, TryCount)
  End Sub
  Private Sub EX_Login_Response(Response As String, ResponseURI As Uri, TryCount As Integer)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "mysso.exede.net" And Not ResponseURI.Host.ToLower = "my.exede.net" Then
      RaiseError("Login Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower = "/federation/ssoredirect/metaalias/idp" And Not ResponseURI.AbsolutePath.ToLower = "/federation/ui/login" Then
      If Response.Contains("window.location.href") Then
        TryCount += 1
        If TryCount > 15 Then
          RaiseError("Login Failed: Server redirected too many times.")
          Return
        End If
        MakeSocket()
        Dim sRedirURI As String = Nothing
        sRedirURI = Response.Substring(Response.IndexOf("window.location.href"))
        sRedirURI = sRedirURI.Substring(sRedirURI.IndexOf("'") + 1)
        sRedirURI = sRedirURI.Substring(0, sRedirURI.IndexOf("'"))
        If sRedirURI = "/" Then
          sRedirURI = wsSocket.ResponseURI.AbsoluteUri.Substring(0, wsSocket.ResponseURI.AbsoluteUri.IndexOf("/", wsSocket.ResponseURI.AbsoluteUri.IndexOf("//") + 2))
        ElseIf sRedirURI.StartsWith("/") Then
          sRedirURI = "https://" & ResponseURI.Host & sRedirURI
        End If
        BeginAttempt(ConnectionStates.Login, ConnectionSubStates.AuthenticateRetry, TryCount, sRedirURI)
        Dim sRet As String = wsSocket.DownloadString(sRedirURI)
        If ClosingTime Then Return
        EX_Login_Response(sRet, wsSocket.ResponseURI, TryCount)
      ElseIf Response.Contains("maintenance") Then
        RaiseError("Login Failed: Server Down for Maintenance.")
      ElseIf Response.Contains("https://DOMAIN.my.salesforce.com") Then
        RaiseError("Login Failed: Server Down for Maintenance.")
      Else
        RaiseError("Login Failed: Could not understand response.", "EX Login Response Error", ResponseURI.OriginalString & vbNewLine & Response, True)
      End If
      Return
    End If
    If Response.Contains("Access rights validated") Then
      Dim sURI As String = Nothing
      If Response.Contains("<form method=""post"" action=""") Then
        sURI = Response.Substring(Response.IndexOf("<form method=""post"" action="""))
        sURI = sURI.Substring(sURI.IndexOf("action=""") + 8)
        sURI = sURI.Substring(0, sURI.IndexOf(""">"))
        sURI = HexDecode(sURI)
      End If
      If String.IsNullOrEmpty(sURI) Then sURI = ResponseURI.AbsoluteUri
      Dim sSAMLResponse As String = Nothing
      If Response.Contains("<input type=""hidden"" name=""SAMLResponse"" value=""") Then
        sSAMLResponse = Response.Substring(Response.IndexOf("<input type=""hidden"" name=""SAMLResponse"" value="""))
        sSAMLResponse = sSAMLResponse.Substring(sSAMLResponse.IndexOf("value=""") + 7)
        sSAMLResponse = sSAMLResponse.Substring(0, sSAMLResponse.IndexOf(""" />"))
      End If
      If String.IsNullOrEmpty(sSAMLResponse) Then
        RaiseError("Login Failed: SAML Response value not found.", "EX Login Response Error", ResponseURI.OriginalString & vbNewLine & Response)
        Return
      End If
      Dim sRelay As String = Nothing
      If Response.Contains("<input type=""hidden"" name=""RelayState"" value=""") Then
        sRelay = Response.Substring(Response.IndexOf("<input type=""hidden"" name=""RelayState"" value="""))
        sRelay = sRelay.Substring(sRelay.IndexOf("value=""") + 7)
        sRelay = sRelay.Substring(0, sRelay.IndexOf(""" />"))
      End If
      If String.IsNullOrEmpty(sRelay) Then
        RaiseError("Login Failed: Relay State value not found.", "EX Login Response Error", ResponseURI.OriginalString & vbNewLine & Response)
        Return
      End If
      EX_Authenticate(sURI, sSAMLResponse, sRelay)
    ElseIf Response.Contains("<input type=""hidden"" name=""goto"" value="""" />") Then
      RaiseError("Login Failed: Please check your account information and try again.")
    ElseIf Response.Contains("<input type=""hidden"" name=""goto"" value=""") Then
      TryCount += 1
      If TryCount > 15 Then
        RaiseError("Login Failed: Server redirected too many times.")
        Return
      End If
      EX_Login_Prepare_Response(Response, ResponseURI, TryCount)
    ElseIf Response.Contains("login-error-alert") Then
      If Response.ToLower.Contains("your username and/or password are incorrect.") Then
        RaiseError("Login Failed: Incorrect Password")
      ElseIf Response.ToLower.Contains("your account has been locked due to excessive failed log in attempts.") Then
        RaiseError("Login Failed: Exede Account Locked. Check your username and password.")
      ElseIf Response.ToLower.Contains("your session has timed out.") Then
        RaiseError("Login Failed: Session timed out. Please try again.")
      Else
        RaiseError("Unknown Login Error.", "EX Login Response Error", ResponseURI.OriginalString & vbNewLine & Response)
      End If
    ElseIf Response.ToLower.Contains("sorry, we've encountered an unexpected error.") Then
      RaiseError("Login Failed: Server encountered an unexpected error.")
    Else
      RaiseError("Could not log in.", "EX Login Response Error", ResponseURI.OriginalString & vbNewLine & Response)
    End If
  End Sub
  Private Sub EX_Authenticate(sURI As String, SAMLResponse As String, RelayState As String)
    MakeSocket()
    Dim sSend As String = "SAMLResponse=" & PercentEncode(HexDecode(SAMLResponse)) & "&RelayState=" & PercentEncode(HexDecode(RelayState))
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadHome, 0, sURI)
    Dim sRet As String = wsSocket.UploadString(sURI, "POST", sSend)
    If ClosingTime Then Return
    EX_Authenticate_Response(sRet, wsSocket.ResponseURI)
  End Sub
  Private Sub EX_Authenticate_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "myexede.force.net" And Not ResponseURI.Host.ToLower = "my.exede.net" Then
      RaiseError("Authentication Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If ResponseURI.AbsolutePath.ToLower.Contains("/identity/saml/samlerror") Or ResponseURI.AbsolutePath.ToLower.Contains("/ssoerror") Then
      RaiseError("Authentication Failed: The server may be down.")
      Return
    End If
    If ResponseURI.AbsolutePath.ToLower = "/secur/frontdoor.jsp" Then
      Dim sURL As String = Nothing
      If Response.Contains("location.href") Then
        sURL = Response.Substring(Response.IndexOf("location.href"))
        sURL = sURL.Substring(sURL.IndexOf("""") + 1)
        sURL = sURL.Substring(0, sURL.IndexOf(""""))
        If sURL = "/" Then
          sURL = ResponseURI.OriginalString.Substring(0, ResponseURI.OriginalString.IndexOf("/", ResponseURI.OriginalString.IndexOf("//") + 2))
        ElseIf sURL.StartsWith("/") Then
          sURL = "https://" & ResponseURI.Host & sURL
        End If
      ElseIf Response.Contains("We are down for maintenance.") Then
        RaiseError("Authentication Failed. If you get this error, please let me know immediately!")
        Return
      Else
        sURL = "https://" & ResponseURI.Host & "/dashboard"
      End If
      EX_Download_Homepage(sURL)
      Return
    End If
    If Response.Contains("maintenance") Then
      RaiseError("Authentication Failed: Server Down for Maintenance.")
    Else
      RaiseError("Authentication Failed: Could not understand response.", "EX Authenticate Response Error", ResponseURI.OriginalString & vbNewLine & Response, True)
    End If
  End Sub
  Private Sub EX_Download_Homepage(sURI As String)
    MakeSocket()
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadAJAX, 1, sURI)
    Dim sRet As String = wsSocket.DownloadString(sURI)
    If ClosingTime Then Return
    EX_Ajax_Response(sRet, wsSocket.ResponseURI, "2a")
  End Sub
  Private Sub EX_Ajax_Response(Response As String, ResponseURI As Uri, AjaxID As String)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "myexede.force.net" And Not ResponseURI.Host.ToLower = "my.exede.net" Then
      RaiseError("AJAX Load Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower = "/dashboard" Then
      If Response.Contains("location.href") Then
        Dim sURL As String = Nothing
        sURL = Response.Substring(Response.IndexOf("location.href"))
        sURL = sURL.Substring(sURL.IndexOf("'") + 1)
        sURL = sURL.Substring(0, sURL.IndexOf("'"))
        If sURL = "/" Then
          sURL = ResponseURI.OriginalString.Substring(0, ResponseURI.OriginalString.IndexOf("/", ResponseURI.OriginalString.IndexOf("//") + 2))
        ElseIf sURL.StartsWith("/") Then
          sURL = "https://" & ResponseURI.Host & sURL
        End If
        EX_Download_Homepage(sURL)
      ElseIf Response.Contains("maintenance") Then
        RaiseError("AJAX Load Failed: Server Down for Maintenance.")
      Else
        RaiseError("AJAX Load Failed: Could not understand response.", "EX Ajax Response Error", ResponseURI.OriginalString & vbNewLine & Response, True)
      End If
      Return
    End If
    If Response.Contains("amount-used") Then
      Dim sTable As String = Response.Substring(Response.LastIndexOf("<div class=""amount-used"">"))
      sTable = sTable.Substring(0, sTable.IndexOf("</p>") + 4)
      ReadUsage(sTable)
    ElseIf Response.Contains("<span id=""ajax-view-state""") Then
      Dim AjaxViewState As String = Response.Substring(Response.IndexOf("<span id=""ajax-view-state"""))
      AjaxViewState = AjaxViewState.Substring(0, AjaxViewState.IndexOf("</span>"))
      Dim sViewState As String = AjaxViewState.Substring(AjaxViewState.IndexOf("""com.salesforce.visualforce.ViewState"""))
      sViewState = sViewState.Substring(sViewState.IndexOf("value=""") + 7)
      sViewState = sViewState.Substring(0, sViewState.IndexOf(""" />"))
      Dim sVSVersion As String = AjaxViewState.Substring(AjaxViewState.IndexOf("""com.salesforce.visualforce.ViewStateVersion"""))
      sVSVersion = sVSVersion.Substring(sVSVersion.IndexOf("value=""") + 7)
      sVSVersion = sVSVersion.Substring(0, sVSVersion.IndexOf(""" />"))
      Dim sVSMAC As String = AjaxViewState.Substring(AjaxViewState.IndexOf("""com.salesforce.visualforce.ViewStateMAC"""))
      sVSMAC = sVSMAC.Substring(sVSMAC.IndexOf("value=""") + 7)
      sVSMAC = sVSMAC.Substring(0, sVSMAC.IndexOf(""" />"))
      Dim sVSCSRF As String = AjaxViewState.Substring(AjaxViewState.IndexOf("""com.salesforce.visualforce.ViewStateCSRF"""))
      sVSCSRF = sVSCSRF.Substring(sVSCSRF.IndexOf("value=""") + 7)
      sVSCSRF = sVSCSRF.Substring(0, sVSCSRF.IndexOf(""" />"))
      EX_Download_Ajax("https://" & ResponseURI.Host & "/dashboard?refURL=https%3A%2F%2F" & ResponseURI.Host & "%2Fdashboard", AjaxID, sViewState, sVSVersion, sVSMAC, sVSCSRF)
    ElseIf Response.Contains("https://myexede.force.com/atlasPlanInvalid") Or Response.Contains("https://my.exede.net/atlasPlanInvalid") Then
      RaiseError("AJAX Load Failed: You no longer have access to MyExede. Please check back again or contact Customer Care [(855) 463-9333] if the problem persists.")
    ElseIf Response.Contains("Concurrent requests limit exceeded.") Then
      RaiseError("AJAX Load Failed: Too many requests. Check for usage data less often.")
    ElseIf Response.Contains("maintenance") Then
      RaiseError("AJAX Load Failed: Server Down for Maintenance.")
    ElseIf Response.Contains("window.location.href") Then
      RaiseError("AJAX Load Failed: Sent back to login page.")
    ElseIf Response.Contains("An internal server error occurred") Or Response.Contains("Something went wrong.") Then
      RaiseError("AJAX Load Failed: Server Error - Exede may be having trouble.")
    Else
      RaiseError("AJAX Load Failed: Could not find AJAX ViewState variables.", "EX Ajax Response Error", ResponseURI.OriginalString & vbNewLine & Response)
    End If
  End Sub
  Private Sub EX_Download_Ajax(sURI As String, AjaxID As String, sViewState As String, sVSVersion As String, sVSMAC As String, sVSCSRF As String)
    MakeSocket()
    If AjaxID(0) = "8" Then
      BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTable, 0, sURI)
    ElseIf AjaxID(1) = "a" Then
      If AjaxID(0) = "6" Then
        BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadAJAX, 4, sURI)
      Else
        BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadAJAX, Val(AjaxID(0)), sURI)
      End If
    Else
      BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadAJAXRetry, Val(AjaxID(0)), sURI)
    End If
    Dim sSend As String = "AJAXREQUEST=_viewRoot" &
             "&j_id0%3AidForm=j_id0%3AidForm" &
             "&com.salesforce.visualforce.ViewState=" & PercentEncode(sViewState) &
             "&com.salesforce.visualforce.ViewStateVersion=" & PercentEncode(sVSVersion) &
             "&com.salesforce.visualforce.ViewStateMAC=" & PercentEncode(sVSMAC) &
             "&com.salesforce.visualforce.ViewStateCSRF=" & PercentEncode(sVSCSRF) &
             "&j_id0%3AidForm%3Aj_id" & AjaxID(0) & "=j_id0%3AidForm%3Aj_id" & AjaxID(0)
    Dim sRet As String = wsSocket.UploadString(sURI, "POST", sSend)
    If ClosingTime Then Return
    Dim newAjaxID As Integer = Val(AjaxID(0))
    Dim newAjaxType As String = AjaxID(1)
    If AjaxID(1) = "a" Then
      Select Case Val(AjaxID)
        Case 2 : newAjaxID = 3
        Case 3 : newAjaxID = 6
        Case 6 : newAjaxID = 8
        Case Else : newAjaxID = 1 : newAjaxType = "b"
      End Select
    Else
      newAjaxID += 1
    End If
    EX_Ajax_Response(sRet, wsSocket.ResponseURI, Trim(Str(newAjaxID)) & newAjaxType)
  End Sub
  Private Sub EX_Read_Table(Table As String)
    If Not Table.Contains("amount-used") Then
      RaiseError("Usage Read Failed: Unable to locate data table", , , , Table)
    Else
      Dim Used As String = Table.Substring(Table.IndexOf("amount-used"))
      Used = Used.Substring(Used.IndexOf(""">") + 2)
      Used = Used.Substring(0, Used.IndexOf("</"))
      Dim Total As String = Nothing
      If Table.Contains("<strong>") Then
        Total = Table.Substring(Table.IndexOf("<strong>") + 8)
        Total = Total.Substring(0, Total.IndexOf("</"))
      End If
      Dim lUsed As Long = StrToVal(Used, MBPerGB)
      Dim lTotal As Long = StrToVal(Total, MBPerGB)
      If lUsed = 0 And lTotal = 0 Then
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginIssue, "Data temporarily unavailable."))
      Else
        If lTotal > 0 Then
          RaiseEvent ConnectionWBXResult(Me, New TYPEBResultEventArgs(lUsed, lTotal, Now))
        Else
          RaiseEvent ConnectionWBXResult(Me, New TYPEBResultEventArgs(lUsed, 150000, Now))
        End If
      End If
    End If
  End Sub
#End Region
#Region "RP"
  Private Sub RP_Login_Retry(sURI As String)
    MakeSocket()
    Dim sUser As String = sAccount.Substring(0, sAccount.LastIndexOf("@"))
    Dim sSend As String = "warningTrip=true&userName=" & sUser & "&passwd=" & PercentEncode(sPassword)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.AuthenticateRetry, 0, sURI)
    Dim sRet As String = wsSocket.UploadString(sURI, "POST", sSend)
    If ClosingTime Then Return
    RP_Login_Response(sRet, wsSocket.ResponseURI, True)
  End Sub
  Private Sub RP_Login_Response(Response As String, ResponseURI As Uri, Retry As Boolean)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = sProvider & ".ruralportal.net" Then
      RaiseError("Login Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If ResponseURI.AbsolutePath.ToLower.StartsWith("/us/home.do") Then
      RP_Usage("sitemanage")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower.StartsWith("/us/login.do") Then
      RaiseError("Login Failed: Could not understand response.", "RP Login Response Error", ResponseURI.OriginalString & vbNewLine & Response, True)
      Return
    End If
    If Not String.IsNullOrEmpty(ResponseURI.Query) Then
      If ResponseURI.Query.ToLower.Contains("pass=false") Then
        RaiseError("Login Failed: Incorrect password.")
        Return
      End If
    End If
    If Not Response.ToLower.Contains("confirmchange(msg);") Then
      RaiseError("Login Failed: Sent back to login page.", , , True)
      Return
    End If
    If String.IsNullOrEmpty(sProvider) Then
      RaiseError("Login Error: Provider missing. Also, your password is bad. You'll need to change it.")
      Return
    End If
    If Retry Then
      RaiseError("Login Issue: Your password is bad.")
      If sProvider.Contains(".") Then sProvider = sProvider.Substring(0, sProvider.LastIndexOf("."))
      Dim uriString As String = String.Format(sRP, sProvider, "login")
      Try
        Process.Start(uriString)
      Catch ex As Exception
      End Try
    Else
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginIssue, "Login Issue: Your password needs to be changed."))
      If sProvider.Contains(".") Then sProvider = sProvider.Substring(0, sProvider.LastIndexOf("."))
      Dim uriString As String = String.Format(sRP, sProvider, "login")
      RP_Login_Retry(uriString)
    End If
  End Sub
  Private Sub RP_Usage(File As String)
    If sProvider.Contains(".") Then sProvider = sProvider.Substring(0, sProvider.LastIndexOf("."))
    Dim uriString As String = String.Format(sRP, sProvider, File)
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTable, 0, uriString)
    MakeSocket()
    Dim sRet As String = wsSocket.DownloadString(uriString)
    If ClosingTime Then Return
    RP_Usage_Response(sRet, wsSocket.ResponseURI)
  End Sub
  Private Sub RP_Usage_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = sProvider & ".ruralportal.net" Then
      RaiseError("Usage Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If Not Response.Contains("Current Usage") Then
      RaiseError("Usage Failed: Failed to log in.", "RP Usage Response Error", ResponseURI.OriginalString & vbNewLine & Response, True)
      Return
    End If
    If Response.Contains("Usage data is not available.") Then
      RaiseError("Usage Failed: Data temporarily unavailable.")
      Return
    End If
    If Not Response.Contains("<!-- usage bar -->") Then
      RaiseError("Usage Failed: Could not find usage meter.", "RP Usage Response Error", ResponseURI.OriginalString & vbNewLine & Response)
      Return
    End If
    Dim sFind As String = Response.Substring(Response.IndexOf("<!-- usage bar -->"))
    If Not sFind.Contains("<table") Then
      RaiseError("Usage Failed: Could not find usage meter table.", "RP Usage Response Error", ResponseURI.OriginalString & vbNewLine & Response)
      Return
    End If
    sFind = sFind.Substring(sFind.IndexOf("<table"))
    If sFind.Contains("</table>") And sFind.Contains("<!-- Buy more -->") Then
      sFind = sFind.Substring(0, sFind.IndexOf("</table>", sFind.IndexOf("<!-- Buy more -->")))
      ReadUsage(sFind)
    ElseIf sFind.Contains("</table>") And sFind.Contains("<!-- End up/down stream -->") Then
      sFind = sFind.Substring(0, sFind.IndexOf("</table>", sFind.IndexOf("<!-- End up/down stream -->")))
      ReadUsage(sFind)
    Else
      RaiseError("Usage Failed: Could not parse usage meter table.", "RP Usage Response Error", ResponseURI.OriginalString & vbNewLine & Response)
    End If
  End Sub
  Private Sub RP_Read_Table(Table As String)
    If Table.Contains("MB)") Then
      Dim sRows As String() = Split(Table, vbLf)
      Dim sDown As String = String.Empty, sDownT As String = String.Empty, sUp As String = String.Empty, sUpT As String = String.Empty
      For Each row In sRows
        If Not String.IsNullOrEmpty(row) Then
          If row.Contains(" MB)") Then
            If String.IsNullOrEmpty(sUp) Then
              sUp = row.Substring(row.IndexOf("% (") + 3)
              sUp = sUp.Substring(0, sUp.IndexOf(" MB)"))
            ElseIf String.IsNullOrEmpty(sDown) Then
              sDown = row.Substring(row.IndexOf("% (") + 3)
              sDown = sDown.Substring(0, sDown.IndexOf(" MB)"))
            End If
          ElseIf row.Contains("MB Limit:") Then
            Dim sLimit As String = row.Substring(row.IndexOf("MB Limit:") + 10)
            sLimit = sLimit.Substring(0, sLimit.IndexOf("</td>"))
            sUpT = sLimit.Substring(0, sLimit.IndexOf(" Up"))
            sDownT = sLimit.Substring(sLimit.IndexOf("Up / ") + 5)
            sDownT = sDownT.Substring(0, sDownT.IndexOf(" Down"))
            Exit For
          End If
        End If
      Next
      If String.IsNullOrEmpty(sDownT) Or String.IsNullOrEmpty(sUpT) Then
        RaiseError("Usage Read Failed: Unable to parse data!", , , , Table)
      Else
        RaiseEvent ConnectionRPLResult(Me, New TYPEAResultEventArgs(StrToVal(sDown), StrToVal(sDownT), StrToVal(sUp), StrToVal(sUpT), Now))
      End If
    ElseIf Table.Contains(" GB (") Then
      Dim sRows As String() = Split(Table, vbLf)
      Dim sDown As String = String.Empty, sDownT As String = String.Empty, sOverhead As String = String.Empty
      For Each row In sRows
        If Not String.IsNullOrEmpty(row) Then
          If row.Contains(" GB of ") And row.Contains(" GB (") And row.Contains("%)") Then
            sDown = row.Substring(0, row.IndexOf(" of ")).Trim
            sDownT = row.Substring(row.IndexOf(" of ") + 4)
            sDownT = sDownT.Substring(0, sDownT.IndexOf(" ("))
            If Not Table.Contains("Breach:") Then Exit For
          ElseIf row.Contains("<td class=""red"" colspan=""2"">") Then
            sOverhead = row.Substring(row.IndexOf(">") + 1)
            If sOverhead.Contains("<") Then
              sOverhead = sOverhead.Substring(0, sOverhead.IndexOf("<"))
            Else
              sOverhead = String.Empty
            End If
            Exit For
          End If
        End If
      Next
      If String.IsNullOrEmpty(sDownT) Then
        RaiseError("Usage Read Failed: Unable to parse data!", , , , Table)
      Else
        RaiseEvent ConnectionRPXResult(Me, New TYPEBResultEventArgs(StrToVal(sDown, MBPerGB) + StrToVal(sOverhead, MBPerGB), StrToVal(sDownT, MBPerGB), Now))
      End If
    Else
      RaiseError("Usage Read Failed: Unable to locate data table!", , , , Table)
    End If
  End Sub
#End Region
#Region "DN"
  Private iHist As Integer = 0
  Private Sub DN_Login_Prep_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "my.dish.com" Then
      RaiseError("Login Prepare Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower.Contains("/preplogon.do") Then
      RaiseError("Login Prepare Failed: Could not understand response.", "DN Login Response Error", ResponseURI.OriginalString & vbNewLine & Response, True)
      Return
    End If
    DN_Login("https://my.dish.com/customercare/saml/login?target=" & PercentEncode("/usermanagement/processSynacoreResponse.do?pageurl=myinternet") & "&message=&forceAuthn=true")
  End Sub
  Private Sub DN_Login(sURI As String)
    MakeSocket()
    wsSocket.ManualRedirect = False
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.ReadLogin, 0, sURI)
    Dim sRet As String = wsSocket.DownloadString(sURI)
    If ClosingTime Then Return
    iHist += 1
    DN_Login_Response(sRet, wsSocket.ResponseURI)
  End Sub
  Private Sub DN_Login_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "identity1.dishnetwork.com" Then
      RaiseError("Login Prepare Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If Not ResponseURI.Query.StartsWith("?") Then
      RaiseError("Login Prepare Failed: AuthState is missing!")
      Return
    End If
    Dim id As String = "0"
    If Response.Contains("name=""id"" value=""") Then
      id = Response.Substring(Response.IndexOf("name=""id"" value=""") + 17)
      id = id.Substring(0, id.IndexOf(""""))
    End If
    Dim sURI As String = ResponseURI.OriginalString
    If Not sURI.Contains("&id=") Then sURI &= "&id=" & id
    If Not sURI.Contains("&coeff=") Then sURI &= "&coeff=0"
    If Not sURI.Contains("&history=") Then sURI &= "&history=" & iHist
    DN_Login_FirstBook(sURI)
  End Sub
  Private Sub DN_Login_FirstBook(sURI As String)
    MakeSocket()
    wsSocket.ManualRedirect = False
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.AuthPrepare, 0, sURI)
    Dim sRet As String = wsSocket.DownloadString(sURI)
    If ClosingTime Then Return
    iHist += 1
    DN_Login_FirstBook_Response(sRet, wsSocket.ResponseURI)
  End Sub
  Private Sub DN_Login_FirstBook_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI, True) Then Return
    If Not ResponseURI.Host.ToLower = "identity1.dishnetwork.com" Then
      RaiseError("Login Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If ResponseURI.AbsolutePath.ToLower.Contains("/firstbookend.php") Then
      DN_Login_Response(Response, ResponseURI)
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower.Contains("/login.php") Then
      RaiseError("Login Failed: Could not understand response.", "DN Login FirstBookend Response Error", ResponseURI.OriginalString & vbNewLine & Response, True)
      Return
    End If
    If Not ResponseURI.Query.StartsWith("?") Then
      RaiseError("Login Failed: AuthState is missing!")
      Return
    End If
    DN_Login_Authenticate(ResponseURI.OriginalString)
  End Sub
  Private Sub DN_Login_Authenticate(sURI As String)
    MakeSocket()
    wsSocket.ManualRedirect = False
    Dim sSend As String = "username=" & PercentEncode(sUsername) &
                          "&password=" & PercentEncode(sPassword) &
                          "&login_type=username,password" &
                          "&source=" &
                          "&source_button="
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, sURI)
    Dim sRet As String = wsSocket.UploadString(sURI, "POST", sSend)
    If ClosingTime Then Return
    iHist += 1
    DN_Login_Authenticate_Response(sRet, wsSocket.ResponseURI)
  End Sub
  Private Sub DN_Login_Authenticate_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "identity1.dishnetwork.com" Then
      RaiseError("Login Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If ResponseURI.AbsolutePath.ToLower.Contains("/lastbookend.php") Then
      If Not ResponseURI.Query.StartsWith("?") Then
        RaiseError("Login Failed: AuthState is missing!")
        Return
      End If
      Dim id As String = "0"
      If Response.Contains("name=""id"" value=""") Then
        id = Response.Substring(Response.IndexOf("name=""id"" value=""") + 17)
        id = id.Substring(0, id.IndexOf(""""))
      End If
      Dim sURI As String = ResponseURI.OriginalString
      If Not sURI.Contains("&id=") Then sURI &= "&id=" & id
      If Not sURI.Contains("&coeff=") Then sURI &= "&coeff=1"
      If Not sURI.Contains("&history=") Then sURI &= "&history=" & iHist
      DN_Login_LastBook(sURI)
    ElseIf ResponseURI.AbsolutePath.ToLower.Contains("finish.php") Then
      If Response.Contains("location.href") Then
        Dim sURL As String = Nothing
        sURL = Response.Substring(Response.IndexOf("location.href"))
        sURL = sURL.Substring(sURL.IndexOf("""") + 1)
        sURL = sURL.Substring(0, sURL.IndexOf(""""))
        If sURL = "/" Then
          sURL = ResponseURI.OriginalString.Substring(0, ResponseURI.OriginalString.IndexOf("/", ResponseURI.OriginalString.IndexOf("//") + 2))
        End If
        MakeSocket()
        wsSocket.ManualRedirect = False
        BeginAttempt(ConnectionStates.Login, ConnectionSubStates.AuthenticateRetry, 0, sURL)
        Dim sRet As String = wsSocket.DownloadString(sURL)
        If ClosingTime Then Return
        iHist += 1
        DN_Login_Authenticate_Response(sRet, wsSocket.ResponseURI)
        Return
      ElseIf Response.Contains("Exception") Then
        If Response.Contains("Unhandled Exception") Then
          RaiseError("Login Failed: Unhandled Exception!")
        ElseIf Response.Contains("<h2>") Then
          Dim sErrMsg As String = Response.Substring(Response.IndexOf("<h2>") + 4)
          sErrMsg = sErrMsg.Substring(0, sErrMsg.IndexOf("</h2>"))
          If sErrMsg.Length > 64 Then sErrMsg = sErrMsg.Substring(0, 64) & "..."
          RaiseError("Login Failed: " & sErrMsg)
        Else
          RaiseError("Login Failed: Unknown Exception!")
        End If
      Else
        RaiseError("Login Failed: Server issue.", "DN Login Authenticate Response Error", ResponseURI.OriginalString & vbNewLine & Response)
      End If
    ElseIf Response.ToLower.Contains("you've submitted your request too soon. please wait and try again.") Then
      RaiseError("Login Failed: Too many requests. Check for usage data less often.")
    ElseIf Response.ToLower.Contains("captcha") Then
      RaiseError("Login Failed: Server requires a captcha to be entered to validate your account. Please log in through your web browser, then try again.")
    Else
      RaiseError("Login Failed: Could not understand response.", "DN Login Authenticate Response Error", ResponseURI.OriginalString & vbNewLine & Response, True)
    End If
  End Sub
  Private Sub DN_Login_LastBook(sURI As String)
    MakeSocket()
    wsSocket.ManualRedirect = False
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Verify, 0, sURI)
    Dim sRet As String = wsSocket.DownloadString(sURI)
    If ClosingTime Then Return
    iHist += 1
    DN_Login_LastBook_Response(sRet, wsSocket.ResponseURI)
  End Sub
  Private Sub DN_Login_LastBook_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "identity1.dishnetwork.com" Then
      RaiseError("Login Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower.Contains("/lastbookend.php") Then
      RaiseError("Login Failed: Could not understand response.", "DN Login LastBookend Response Error", ResponseURI.OriginalString & vbNewLine & Response, True)
      Return
    End If
    If Response.Contains("SAMLResponse"" value=""") Then
      Dim SAMLResponse As String
      SAMLResponse = Response.Substring(Response.IndexOf("SAMLResponse"" value=""") + 21)
      If SAMLResponse.Contains(""" />") Then
        SAMLResponse = SAMLResponse.Substring(0, SAMLResponse.IndexOf(""" />"))
        DN_Login_Verify(SAMLResponse)
      Else
        RaiseError("Login Failed: Incomplete SAML Response Data.", "DN Login LastBookend Response Error", ResponseURI.OriginalString & vbNewLine & Response)
      End If
    ElseIf Response.Contains("The system is currently unavailable. Please try again later.") Then
      RaiseError("System currently unavailable.")
    ElseIf Response.Contains("<div class=""custom_message_text"">") Then
      Dim sErrMsg As String = Response.Substring(Response.IndexOf("<div class=""custom_message_text"">") + 33)
      sErrMsg = sErrMsg.Substring(0, sErrMsg.IndexOf("<"))
      RaiseError(sErrMsg.Trim)
    Else
      RaiseError("Login Failed: No SAML Response", "DN Login LastBookend Response Error", ResponseURI.OriginalString & vbNewLine & Response)
    End If
  End Sub
  Private Sub DN_Login_Verify(SAMLResponse As String)
    MakeSocket()
    wsSocket.ManualRedirect = False
    Dim uriString As String = "https://my.dish.com/customercare/saml/post"
    Dim sSend As String = "SAMLResponse=" & PercentEncode(SAMLResponse) & "&RelayState=" & PercentEncode("/usermanagement/processSynacoreResponse.do?pageurl=myinternet")
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadHome, 0, uriString)
    Dim sRet As String = wsSocket.UploadString(uriString, "POST", sSend)
    If ClosingTime Then Return
    iHist += 1
    DN_Login_Verify_Response(sRet, wsSocket.ResponseURI)
  End Sub
  Private Sub DN_Login_Verify_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI, True) Then Return
    If Not ResponseURI.Host.ToLower = "my.dish.com" Then
      RaiseError("Login Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower.Contains("/processsynacoreresponse.do") Then
      RaiseError("Login Failed: Could not understand response.", "DishNet Login Verify Response Error", ResponseURI.OriginalString & vbNewLine & Response, True)
      Return
    End If
    DN_Download_Home()
  End Sub
  Private Sub DN_Download_Home()
    MakeSocket()
    wsSocket.ManualRedirect = False
    Dim uriString As String = "https://my.dish.com/customercare/usermanagement/getAccountNumberByUUID.do"
    Dim sSend As String = "check="
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTable, 0, uriString)
    Dim sRet As String = wsSocket.UploadString(uriString, "POST", sSend)
    If ClosingTime Then Return
    iHist += 1
    DN_Download_Home_Response(sRet, wsSocket.ResponseURI)
  End Sub
  Private Sub DN_Download_Home_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI, True) Then Return
    If Not ResponseURI.Host.ToLower = "my.dish.com" Then
      RaiseError("Login Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower.Contains("/loadpage.do") Then
      RaiseError("Login Failed: Could not load home page. Redirected to """ & ResponseURI.OriginalString & """.", "DN Download Home Response Error", ResponseURI.OriginalString & vbNewLine & Response)
      Return
    End If
    If ResponseURI.Query = "?page=myaccountsummary_res" Then
      DN_Download_Table()
    ElseIf ResponseURI.Query = "?pageurl=myinternet" Then
      DN_Download_Table_Response(Response, ResponseURI)
    Else
      RaiseError("Home Read Failed.", , , , Response)
    End If
  End Sub
  Private Sub DN_Download_Table()
    MakeSocket()
    wsSocket.ManualRedirect = False
    Dim uriString As String = "https://my.dish.com/customercare/myaccount/myinternet"
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTableRetry, 0, uriString)
    Dim sRet As String = wsSocket.DownloadString(uriString)
    If ClosingTime Then Return
    iHist += 1
    DN_Download_Table_Response(sRet, wsSocket.ResponseURI)
  End Sub
  Private Sub DN_Download_Table_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "my.dish.com" Then
      RaiseError("Usage Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If Response.Contains("The requested URL was rejected.") Then
      RaiseError("Usage Failed: The server rejected the request.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower.Contains("/loadpage.do") Then
      RaiseError("Usage Failed: Could not load usage meter page. Redirected to """ & ResponseURI.OriginalString & """.", "DN Download Table Response Error", ResponseURI.OriginalString & vbNewLine & Response)
      Return
    End If
    If ResponseURI.Query = "?page=myaccountsummary_res" Then
      DN_Download_Table()
    ElseIf ResponseURI.Query = "?pageurl=myinternet" Then
      If Not Response.Contains("widgetLoadUrls[widgetListCount]") Then
        RaiseError("Usage Failed: Could not find usage meter.", "DN Download Table Response Error", ResponseURI.OriginalString & vbNewLine & Response)
        Return
      End If
      Dim sUsageDiv As String = Response.Substring(Response.IndexOf("widgetLoadUrls[widgetListCount]"))
      If Not sUsageDiv.Contains("</form>") Then
        RaiseError("Usage Failed: Could not parse usage data.", "DN Download Table Response Error", ResponseURI.OriginalString & vbNewLine & Response)
        Return
      End If
      sUsageDiv = sUsageDiv.Substring(0, sUsageDiv.IndexOf("</form>"))
      If Not sUsageDiv.ToLower.Contains("remaining capacity") Then
        RaiseError("Usage Failed: Could not detect usage data.", "DN Download Table Response Error", ResponseURI.OriginalString & vbNewLine & Response)
        Return
      End If
      ReadUsage(sUsageDiv)
    Else
      RaiseError("Usage Failed: Redirected to Unknown Page [" & ResponseURI.Query & "]", , , , Response)
    End If
  End Sub
  Private Sub DN_Read_Table(Table As String)
    Dim findCRLF As String = vbLf
    If Table.Contains(vbNewLine) Then
      findCRLF = vbNewLine
    ElseIf Table.Contains(vbLf) Then
      findCRLF = vbLf
    ElseIf Table.Contains(vbCr) Then
      findCRLF = vbCr
    End If
    Dim opV As String = Nothing
    Dim opM As String = Nothing
    Dim atV As String = Nothing
    Dim atM As String = Nothing
    Dim atxV As String = Nothing
    Dim atxM As String = Nothing

    Dim scriptSegment As String = Nothing
    If Table.Contains("</script>") Then
      scriptSegment = Table.Substring(0, Table.IndexOf("</script>"))
    End If
    If Not String.IsNullOrEmpty(scriptSegment) Then
      Dim widgetData() As String = Split(scriptSegment, findCRLF & findCRLF & findCRLF)
      Dim wMeterData As New Specialized.StringDictionary
      For Each widget In widgetData
        Dim widgetLines() As String = Split(widget, findCRLF)
        Dim isMeter As Boolean = False
        For Each wLine In widgetLines
          If wLine.Contains("'/customercare/widgets/loadMeter.do'") Then
            isMeter = True
          End If
        Next
        If Not isMeter Then Continue For
        Dim wID As String = Nothing
        Dim wAttrs As String = Nothing
        For Each wLine In widgetLines
          If wLine.Contains("widgetUniqueIds[widgetListCount]") Then
            wID = wLine
            If wID.Contains(" = ") Then wID = wID.Substring(wID.IndexOf(" = ") + 3)
            If wID.Contains("'") Then
              wID = wID.Substring(wID.IndexOf("'") + 1)
              If wID.Contains("'") Then wID = wID.Substring(0, wID.IndexOf("'"))
            ElseIf wID.Contains("""") Then
              wID = wID.Substring(wID.IndexOf("""") + 1)
              If wID.Contains("""") Then wID = wID.Substring(0, wID.IndexOf(""""))
            ElseIf wID.Contains(";") Then
              wID = wID.Substring(0, wID.IndexOf(";"))
            ElseIf wID.Contains(findCRLF) Then
              wID = wID.Substring(0, wID.IndexOf(findCRLF))
            End If
          End If
          If wLine.Contains("widgetAttrsJsons[widgetListCount]") Then
            wAttrs = wLine
            If wAttrs.Contains(" = ") Then wAttrs = wAttrs.Substring(wAttrs.IndexOf(" = ") + 3)
            If wAttrs.Contains("""") Then
              wAttrs = wAttrs.Substring(wAttrs.IndexOf("""") + 1)
              If wAttrs.Contains("""") Then wAttrs = wAttrs.Substring(0, wAttrs.IndexOf(""""))
            ElseIf wAttrs.Contains("'") Then
              wAttrs = wAttrs.Substring(wAttrs.IndexOf("'") + 1)
              If wAttrs.Contains("'") Then wAttrs = wAttrs.Substring(0, wAttrs.IndexOf("'"))
            ElseIf wAttrs.Contains(";") Then
              wAttrs = wAttrs.Substring(0, wAttrs.IndexOf(";"))
            ElseIf wAttrs.Contains(findCRLF) Then
              wAttrs = wAttrs.Substring(0, wAttrs.IndexOf(findCRLF))
            End If
          End If
          If Not String.IsNullOrEmpty(wID) And Not String.IsNullOrEmpty(wAttrs) Then
            wMeterData.Add(wID, wAttrs)
            Exit For
          End If
        Next
      Next
      If wMeterData.ContainsKey("w_meter_0") Then
        Dim sJSON As String = wMeterData("w_meter_0")
        If sJSON.StartsWith("{") Then sJSON = sJSON.Substring(1)
        If sJSON.EndsWith("}") Then sJSON = sJSON.Substring(0, sJSON.Length - 1)
        If sJSON.Contains(",") Then
          Dim jsLines As New Specialized.StringDictionary
          Dim sJSLines() As String = Split(sJSON, ",")
          For Each sJSLine In sJSLines
            If Not sJSLine.Contains(":") Then Continue For
            If sJSLine.Contains("'") Then sJSLine = Replace(sJSLine, "'", "")
            Dim sKeyVal() As String = Split(sJSLine, ":", 2)
            jsLines.Add(sKeyVal(0), sKeyVal(1))
          Next
          If jsLines.ContainsKey("progressValueAttr") And jsLines.ContainsKey("maxValueAttr") Then
            opV = jsLines("progressValueAttr")
            opM = jsLines("maxValueAttr")
          End If
        End If
      End If
      If wMeterData.ContainsKey("w_meter_1") Then
        Dim sJSON As String = wMeterData("w_meter_1")
        If sJSON.StartsWith("{") Then sJSON = sJSON.Substring(1)
        If sJSON.EndsWith("}") Then sJSON = sJSON.Substring(0, sJSON.Length - 1)
        If sJSON.Contains(",") Then
          Dim jsLines As New Specialized.StringDictionary
          Dim sJSLines() As String = Split(sJSON, ",")
          For Each sJSLine In sJSLines
            If Not sJSLine.Contains(":") Then Continue For
            If sJSLine.Contains("'") Then sJSLine = Replace(sJSLine, "'", "")
            Dim sKeyVal() As String = Split(sJSLine, ":", 2)
            jsLines.Add(sKeyVal(0), sKeyVal(1))
          Next
          If jsLines.ContainsKey("progressValueAttr") And jsLines.ContainsKey("maxValueAttr") Then
            atV = jsLines("progressValueAttr")
            atM = jsLines("maxValueAttr")
          End If
        End If
      End If
      If wMeterData.ContainsKey("w_meter_2") Then
        Dim sJSON As String = wMeterData("w_meter_2")
        If sJSON.StartsWith("{") Then sJSON = sJSON.Substring(1)
        If sJSON.EndsWith("}") Then sJSON = sJSON.Substring(0, sJSON.Length - 1)
        If sJSON.Contains(",") Then
          Dim jsLines As New Specialized.StringDictionary
          Dim sJSLines() As String = Split(sJSON, ",")
          For Each sJSLine In sJSLines
            If Not sJSLine.Contains(":") Then Continue For
            If sJSLine.Contains("'") Then sJSLine = Replace(sJSLine, "'", "")
            Dim sKeyVal() As String = Split(sJSLine, ":", 2)
            jsLines.Add(sKeyVal(0), sKeyVal(1))
          Next
          If jsLines.ContainsKey("progressValueAttr") And jsLines.ContainsKey("maxValueAttr") Then
            atxV = jsLines("progressValueAttr")
            atxM = jsLines("maxValueAttr")
          End If
        End If
      End If
    End If

    Dim htmlSegment As String = Nothing
    If Table.Contains("<div class=""row PrimSec bbActive"">") Then
      htmlSegment = Table.Substring(Table.IndexOf("<div class=""row PrimSec bbActive"">"))
      If htmlSegment.Contains("<div class=""row""><div class=""col-xs-12 col-lg-12 label"">&nbsp;</div></div>") Then
        htmlSegment = htmlSegment.Substring(0, htmlSegment.IndexOf("<div class=""row""><div class=""col-xs-12 col-lg-12 label"">&nbsp;</div></div>"))
      End If
    End If
    If Not String.IsNullOrEmpty(htmlSegment) Then
      If htmlSegment.Contains("<h3 class=""secondaryHeader"">Monthly Capacity</h3>") Then
        htmlSegment = htmlSegment.Substring(htmlSegment.IndexOf("<h3 class=""secondaryHeader"">Monthly Capacity</h3>"))
      End If
      If htmlSegment.Contains(" GB") Then
        Dim htmlParts() As String = Split(htmlSegment, " GB")
        If htmlParts.Length >= 6 Then
          ReDim Preserve htmlParts(htmlParts.Length - 2)
          For I As Integer = 0 To htmlParts.Length - 1
            htmlParts(I) = htmlParts(I).Substring(htmlParts(I).LastIndexOf(">") + 1)
            CleanupResult(htmlParts(I))
          Next
          If Not String.IsNullOrEmpty(htmlParts(0)) Then
            If String.IsNullOrEmpty(opM) Then
              opM = htmlParts(0)
            Else
              If Not opM = htmlParts(0) Then
                RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Not sure about this usage data (Off-Peak Max). Gonna take a closer look...", Table))
                opM = htmlParts(0)
              End If
            End If
          End If
          If Not String.IsNullOrEmpty(htmlParts(1)) Then
            If String.IsNullOrEmpty(atM) Then
              atM = htmlParts(1)
            Else
              If Not atM = htmlParts(1) Then
                RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Not sure about this usage data (Anytime Max). Gonna take a closer look...", Table))
                atM = htmlParts(1)
              End If
            End If
          End If
          If Not String.IsNullOrEmpty(htmlParts(2)) Then
            If String.IsNullOrEmpty(opV) Then
              opV = htmlParts(2)
            Else
              If Not opV = htmlParts(2) Then
                RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Not sure about this usage data (Off-Peak Value). Gonna take a closer look...", Table))
                opV = htmlParts(2)
              End If
            End If
          End If
          If Not String.IsNullOrEmpty(htmlParts(3)) Then
            If String.IsNullOrEmpty(atV) Then
              atV = htmlParts(3)
            Else
              If Not atV = htmlParts(3) Then
                RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Not sure about this usage data (Anytime Value). Gonna take a closer look...", Table))
                atV = htmlParts(3)
              End If
            End If
          End If
          If Not String.IsNullOrEmpty(htmlParts(4)) Then
            If String.IsNullOrEmpty(atxM) Then
              atxM = htmlParts(4)
            Else
              If Not atxM = htmlParts(4) Then
                If atxM = "15.0" And htmlParts(4) = "0.0" Then
                  atxM = "0.0"
                Else
                  RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Not sure about this usage data (Added Max). Gonna take a closer look...", Table))
                  atxM = htmlParts(4)
                End If
              End If
            End If
          End If
        End If
      End If
    End If
    Dim lDown, lDownT, lUp, lUpT As Long
    If Not String.IsNullOrEmpty(atM) Then lDownT = StrToVal(atM, MBPerGB)
    If Not String.IsNullOrEmpty(atV) Then
      If lDownT > 0 Then
        lDown = lDownT - StrToVal(atV, MBPerGB)
      Else
        lDown = StrToVal(atV, MBPerGB)
      End If
    End If
    If Not String.IsNullOrEmpty(opM) Then lUpT = StrToVal(opM, MBPerGB)
    If Not String.IsNullOrEmpty(opV) Then
      If lUpT > 0 Then
        lUp = lUpT - StrToVal(opV, MBPerGB)
      Else
        lUp = StrToVal(opV, MBPerGB)
      End If
    End If
    If lDownT = 0 And lUpT = 0 Then
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginIssue, "Data temporarily unavailable."))
    ElseIf lDownT > 0 Then
      If Not String.IsNullOrEmpty(atxV) And Not String.IsNullOrEmpty(atxM) Then
        If Not StrToFloat(atxV) = 0.0 Then lDown += StrToVal(atxV, MBPerGB)
        If Not StrToFloat(atxM) = 0.0 Then lDownT += StrToVal(atxV, MBPerGB)
      End If
      RaiseEvent ConnectionDNXResult(Me, New TYPEA2ResultEventArgs(lDown, lDownT, lUp, lUpT, Now))
    Else
      RaiseError("Usage Read Failed: Unable to locate data table!", , , , Table)
    End If
  End Sub
#End Region
#End Region
#Region "Useful Functions"
  Private Sub MakeSocket()
    Dim oldEncoding As System.Text.Encoding = System.Text.Encoding.GetEncoding(LATIN_1)
    If wsSocket IsNot Nothing Then
      oldEncoding = wsSocket.Encoding
      If wsSocket.IsBusy Then wsSocket.Cancel()
      wsSocket = Nothing
    End If
    wsSocket = New WebClientEx(sDataPath)
    wsSocket.Timeout = c_Timeout
    wsSocket.Proxy = c_Proxy
    wsSocket.CookieJar = c_Jar
    wsSocket.Encoding = oldEncoding
  End Sub
  Private Function CheckForErrors(response As String, responseURI As Uri, Optional IgnoreResponseData As Boolean = False) As Boolean
    If Not IgnoreResponseData Then
      If String.IsNullOrEmpty(response) OrElse response = "Error: Empty Response" Then
        RaiseError("Empty Response")
        Return True
      End If
      If response.StartsWith("Error: ") Then
        Dim sError As String = response.Substring(7)
        RaiseError(sError)
        Return True
      End If
    End If
    If response = "Connection timed out." Then
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.ConnectionTimeout))
      Return True
    End If
    If responseURI Is Nothing Then
      RaiseError("Empty Response URL")
      Return True
    End If
    Return False
  End Function
  Private Sub RaiseError(ErrorMessage As String, Optional FailureLocation As String = Nothing, Optional FailureData As String = Nothing, Optional ResetAccountType As Boolean = False, Optional FailureText As String = Nothing)
    If Not String.IsNullOrEmpty(Trim(ErrorMessage)) Then
      If String.IsNullOrEmpty(FailureText) Then
        If Not String.IsNullOrEmpty(Trim(FailureLocation)) Then
          FailureText = FailureLocation & ": " & ErrorMessage
          If Not String.IsNullOrEmpty(FailureData) Then FailureText &= vbNewLine & FailureData
        End If
      End If
      If ResetAccountType Then
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.FatalLoginFailure, ErrorMessage, FailureText))
      Else
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, ErrorMessage, FailureText))
      End If
    End If
  End Sub
  Private Sub BeginAttempt(state As ConnectionStates, substate As ConnectionSubStates, stage As Integer, URL As String)
    sAttemptedURL = URL
    AttemptedTag = state
    AttemptedSub = substate
    AttemptedStage = stage
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(state, substate, stage))
  End Sub
  Private Function StrToVal(str As String, Optional vMult As Integer = 1) As Long
    If String.IsNullOrEmpty(str) Then Return 0
    If Not str.Contains(" ") Then Return CLng(Val(str.Replace(",", "")) * vMult)
    Return CLng(Val(str.Substring(0, str.IndexOf(" ")).Replace(",", "")) * vMult)
  End Function
  Private Function StrToFloat(str As String) As Double
    If String.IsNullOrEmpty(str) Then Return 0.0#
    If Not str.Contains(" ") Then Return Val(str.Replace(",", ""))
    Return Val(str.Substring(0, str.IndexOf(" ")).Replace(",", ""))
  End Function
  Private Sub CleanupResult(ByRef result As String)
    If Not String.IsNullOrEmpty(result) Then
      result = Replace(result, "&nbsp;", " ")
      result = Replace(result, vbTab, "")
      result = Replace(result, vbCr, "")
      result = Replace(result, vbLf, "")
      Do
        result = Replace(result, "  ", " ")
      Loop While result.Contains("  ")
      result = Trim(result)
    Else
      result = ""
    End If
  End Sub
  Public Function IgnoreCert(sender As Object, certificate As System.Security.Cryptography.X509Certificates.X509Certificate, chain As System.Security.Cryptography.X509Certificates.X509Chain, errors As Net.Security.SslPolicyErrors) As Boolean
    Return True
  End Function
#End Region
#Region "IDisposable Support"
  Private disposedValue As Boolean
  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not Me.disposedValue Then
      If disposing Then
        ClosingTime = True
        If wsSocket IsNot Nothing AndAlso wsSocket.IsBusy Then
          wsSocket.Cancel()
          wsSocket = Nothing
        End If
      End If
    End If
    Me.disposedValue = True
  End Sub
  Public Sub Dispose() Implements IDisposable.Dispose
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub
#End Region
  Protected Overrides Sub Finalize()
    MyBase.Finalize()
  End Sub
End Class
