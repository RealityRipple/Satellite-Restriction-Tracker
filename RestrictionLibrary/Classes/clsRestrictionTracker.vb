﻿Public Class localRestrictionTracker
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
    Private m_slow As Boolean
    Private m_free As Boolean
    Public Sub New(lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long, dUpdate As Date)
      m_Down = lDown
      m_Up = lUp
      m_DownLim = lDownLim
      m_UpLim = lUpLim
      m_Update = dUpdate
      m_slow = False
      m_free = False
    End Sub
    Public Sub New(lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long, dUpdate As Date, bSlow As Boolean, bFree As Boolean)
      m_Down = lDown
      m_Up = lUp
      m_DownLim = lDownLim
      m_UpLim = lUpLim
      m_Update = dUpdate
      m_slow = bSlow
      m_free = bFree
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
    Public ReadOnly Property SlowedDetected As Boolean
      Get
        Return m_slow
      End Get
    End Property
    Public ReadOnly Property FreeDetected As Boolean
      Get
        Return m_free
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
    Private m_slow As Boolean
    Private m_free As Boolean
    Public Sub New(lAnyTime As Long, lAnyTimeLim As Long, lOffPeak As Long, lOffPeakLim As Long, dUpdate As Date)
      m_AnyTime = lAnyTime
      m_AnyTimeLim = lAnyTimeLim
      m_OffPeak = lOffPeak
      m_OffPeakLim = lOffPeakLim
      m_Update = dUpdate
      m_slow = False
      m_free = False
    End Sub
    Public Sub New(lAnyTime As Long, lAnyTimeLim As Long, lOffPeak As Long, lOffPeakLim As Long, dUpdate As Date, bSlow As Boolean, bFree As Boolean)
      m_AnyTime = lAnyTime
      m_AnyTimeLim = lAnyTimeLim
      m_OffPeak = lOffPeak
      m_OffPeakLim = lOffPeakLim
      m_Update = dUpdate
      m_slow = bSlow
      m_free = bFree
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
    Public ReadOnly Property SlowedDetected As Boolean
      Get
        Return m_slow
      End Get
    End Property
    Public ReadOnly Property FreeDetected As Boolean
      Get
        Return m_free
      End Get
    End Property
  End Class
  Public Class TYPEBResultEventArgs
    Inherits EventArgs
    Private m_Used As Long
    Private m_Limit As Long
    Private m_Update As Date
    Private m_slow As Boolean
    Private m_free As Boolean
    Public Sub New(lUsed As Long, lLimit As Long, dUpdate As Date)
      m_Used = lUsed
      m_Limit = lLimit
      m_Update = dUpdate
      m_slow = False
      m_free = False
    End Sub
    Public Sub New(lUsed As Long, lLimit As Long, dUpdate As Date, bSlow As Boolean, bFree As Boolean)
      m_Used = lUsed
      m_Limit = lLimit
      m_Update = dUpdate
      m_slow = bSlow
      m_free = bFree
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
    Public ReadOnly Property SlowedDetected As Boolean
      Get
        Return m_slow
      End Get
    End Property
    Public ReadOnly Property FreeDetected As Boolean
      Get
        Return m_free
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
  Public Class LoginCompletionEventArgs
    Inherits EventArgs
    Private m_HostType As SatHostTypes
    Public Sub New(myHostType As SatHostTypes)
      m_HostType = myHostType
    End Sub
    Public ReadOnly Property HostType As SatHostTypes
      Get
        Return m_HostType
      End Get
    End Property
  End Class
  Public Event LoginComplete(sender As Object, e As LoginCompletionEventArgs)
#End Region
  Private acType As DetermineType
  Private mySettings As AppSettings
  Private Const MBPerGB As Integer = 1000
  Private Const sWB As String = "https://myaccount.{0}/wbisp/{2}/{1}.jsp"
  Private Const sRP As String = "https://{0}.ruralportal.net/us/{1}.do"
  Private justATest As Boolean
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
  Private c_TLSProxy As Boolean
  Private c_Protocol As Net.SecurityProtocolType
  Private c_TLSProxyAddr As String
  Private sDataPath As String
  Private wsSocket As WebClientEx
#Region "Initialization Functions"
  Public Sub New(ConfigPath As String, Optional OnlyLogin As Boolean = False)
    justATest = OnlyLogin
    Randomize()
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
    c_TLSProxy = mySettings.TLSProxy
    If c_TLSProxy Then
      c_Protocol = mySettings.SecurityProtocol
      If Int(Rnd() * 2) = 0 Then
        c_TLSProxyAddr = "http://wb.realityripple.com/tls.php"
      Else
        c_TLSProxyAddr = "http://losberros.org/tls.php"
      End If
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
      Case SatHostTypes.WildBlue_EXEDE
        If sProvider.ToLower = "exede.net" Or sProvider.ToLower = "satelliteinternetco.com" Then
          LoginExede()
        Else
          LoginWB()
        End If
      Case SatHostTypes.RuralPortal_LEGACY, SatHostTypes.RuralPortal_EXEDE : LoginRP()
      Case SatHostTypes.DishNet_EXEDE : LoginDN()
    End Select
  End Sub
  Private Sub LoginWB()
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Prepare))
    Dim uriString As String = String.Format(sWB, IIf(sProvider.ToLower = "exede.com", "exede.net", sProvider), "servLogin", sProvider)
    MakeSocket(False)
    Dim sSend As String = "uid=" & srlFunctions.PercentEncode(sUsername) & "&userPassword=" & srlFunctions.PercentEncode(sPassword)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, uriString)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(uriString), sSend, responseURI, responseData)
    If ClosingTime Then Return
    WB_Login_Response(responseData, responseURI)
  End Sub
  Private Sub LoginExede()
    If sProvider = "exede.net" Then
      AJAXOrder = {4, 5, 10}
    ElseIf sProvider = "satelliteinternetco.com" Then
      AJAXOrder = {2, 4}
    Else
      RaiseError("Prepare Failed: Unknown Provider - Can't determine AJAX order.")
    End If
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Prepare))
    Dim uriString As String = "https://my." & sProvider & "/login"
    MakeSocket(True)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.ReadLogin, 0, uriString)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendGET(New Uri(uriString), responseURI, responseData)
    If ClosingTime Then Return
    EX_Login_Prepare_Response(responseData, responseURI, 0)
  End Sub
  Private Sub LoginRP()
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Prepare))
    If sProvider.Contains(".") Then sProvider = sProvider.Substring(0, sProvider.LastIndexOf("."))
    Dim uriString As String = String.Format(sRP, sProvider, "login")
    MakeSocket(False)
    Dim sSend As String = "warningTrip=false&userName=" & srlFunctions.PercentEncode(sUsername) & "&passwd=" & srlFunctions.PercentEncode(sPassword)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, uriString)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(uriString), sSend, responseURI, responseData)
    If ClosingTime Then Return
    RP_Login_Response(responseData, responseURI, False)
  End Sub
  Private Sub LoginDN()
    iHist = 0
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Prepare))
    Dim uriString As String = "https://my.dish.com/customercare/myaccount/myinternet"
    MakeSocket(False, False)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.ReadLogin, 0, uriString)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendGET(New Uri(uriString), responseURI, responseData)
    If ClosingTime Then Return
    iHist += 1
    DN_Login_Prep_Response(responseData, responseURI)
  End Sub
#End Region
#Region "Parsing Functions"
  Private Sub ReadUsage(Table As String)
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.TableRead))
    Select Case mySettings.AccountType
      Case SatHostTypes.WildBlue_LEGACY : WB_Read_Table(Table)
      Case SatHostTypes.WildBlue_EXEDE
        If sProvider.ToLower = "exede.net" Or sProvider.ToLower = "satelliteinternetco.com" Then
          EX_Read_Table(Table)
        Else
          WB_Read_Table(Table)
        End If
      Case SatHostTypes.RuralPortal_LEGACY, SatHostTypes.RuralPortal_EXEDE : RP_Read_Table(Table)
      Case SatHostTypes.DishNet_EXEDE : DN_Read_Table(Table)
    End Select
    c_Jar = New Net.CookieContainer
    srlFunctions.SendSocketErrors(sDataPath)
  End Sub
#Region "WB"
  Private Sub WB_Login_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Response.Contains("usage.jsp") Then
      If justATest Then
        RaiseEvent LoginComplete(Me, New LoginCompletionEventArgs(SatHostTypes.WildBlue_LEGACY))
        Return
      End If
      WB_Usage("usage")
    ElseIf Response.Contains("usage_bm.jsp") Then
      If justATest Then
        RaiseEvent LoginComplete(Me, New LoginCompletionEventArgs(SatHostTypes.WildBlue_EXEDE))
        Return
      End If
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
        RaiseError("Login Failed: Could not understand error.", True, "WB Login Response", Response, ResponseURI)
      End If
    ElseIf Response.Contains("https://my.exede.net/usage") Then
      mySettings.AccountType = SatHostTypes.WildBlue_EXEDE
      RaiseError("Login Redirect: Exede account detected.")
      GetUsage()
    Else
      RaiseError("Login Failed: Could not understand response.", True, "WB Login Response", Response, ResponseURI)
    End If
  End Sub
  Private Sub WB_Usage(File As String)
    MakeSocket(False)
    Dim uriString As String = String.Format(sWB, IIf(sProvider.ToLower = "exede.com", "exede.net", sProvider), File, sProvider)
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTable, 0, uriString)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendGET(New Uri(uriString), responseURI, responseData)
    If ClosingTime Then Return
    WB_Usage_Response(responseData, responseURI)
  End Sub
  Private Sub WB_Usage_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "myaccount." & IIf(sProvider.ToLower = "exede.com", "exede.net", sProvider.ToLower) Then
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
          RaiseError("Usage Failed: Could not parse usage meter.", "WB Usage Response", Response, ResponseURI)
        End If
      ElseIf sFind.Contains("FREEDOM") AndAlso sFind.Contains("Current Usage<strong>:") Then
        sFind = sFind.Substring(sFind.IndexOf("FREEDOM"))
        If sFind.Contains("</td>") Then
          sFind = sFind.Substring(0, sFind.IndexOf("</td>"))
          ReadUsage(sFind)
        Else
          RaiseError("Usage Failed: Could not parse usage meter.", "WB Usage Response (Exede Freedom)", Response, ResponseURI)
        End If
      ElseIf sFind.Contains("At this time, your usage is not being counted toward your data allowance.") Then
        imFree = True
        RaiseError("Your usage is not being metered at this time!")
      Else
        RaiseError("Usage Failed: Could not find usage meter.", "WB Usage Response", Response, ResponseURI)
      End If
    ElseIf Response.Contains("<div class=""error"">") Then
      Dim sMessage As String = Response.Substring(Response.IndexOf("<div class=""error"">"))
      If sMessage.Contains("<b>") Then
        sMessage = sMessage.Substring(sMessage.IndexOf("<b>") + 3)
        sMessage = sMessage.Substring(0, sMessage.IndexOf("<"))
        RaiseError("Usage Failed: " & sMessage)
      Else
        RaiseError("Usage Failed: Could not find usage meter.", "WB Usage Response (Error DIV but no BOLD text)", Response, ResponseURI)
      End If
    Else
      Dim sErr As String = "Could not find usage meter."
      If Response.Contains("Oops") Then
        sErr = Response.Substring(Response.IndexOf("Oops"))
        If sErr.Contains("</h3>") Then
          sErr = sErr.Substring(0, sErr.IndexOf("</h3>"))
          If sErr = "Oops. We're having a problem displaying your usage information." Or sErr = "Oops. We're having a problem displaying usage data" Then
            RaiseError("Usage Failed: Data temporarily unavailable.")
            Return
          End If
        ElseIf sErr.Contains("<hr>") Then
          sErr = sErr.Substring(sErr.IndexOf("<hr>") + 4)
          sErr = sErr.Substring(0, sErr.IndexOf("<hr>"))
        End If
        If sErr.Contains("<!-") Then
          If sErr.Contains("->") Then
            sErr = sErr.Substring(0, sErr.IndexOf("<!-")) & sErr.Substring(sErr.IndexOf("->") + 2)
          End If
        End If
        RaiseError("Usage Failed: " & sErr, "WB Usage Response (Oops)", Response, ResponseURI)
      Else
        RaiseError("Usage Failed: " & sErr, "WB Usage Response", Response, ResponseURI)
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
        RaiseError("Usage Read Failed: Unable to parse data!", "WB Read Table", Table)
      Else
        RaiseEvent ConnectionWBLResult(Me, New TYPEAResultEventArgs(StrToVal(sDown), StrToVal(sDownT), StrToVal(sUp), StrToVal(sUpT), Now, imSlowed, imFree))
      End If
    ElseIf Table.Contains("allowance") Then
      Dim sPlusT As String = String.Empty
      For I As Integer = 0 To sRows.Length - 1
        If Not String.IsNullOrEmpty(sRows(I)) Then
          If sRows(I).Contains("<strong>") Then
            If String.IsNullOrEmpty(sDownT) Then
              sDownT = sRows(I).Substring(sRows(I).IndexOf("<strong>") + 8)
              sDownT = sDownT.Substring(0, sDownT.IndexOf("</strong>"))
            ElseIf sRows(I).Contains("Total usage:") And sRows(I).Contains("</b>") And String.IsNullOrEmpty(sDown) Then
              If sRows(I).Contains("<b>") And sRows(I).Contains("</b>") Then
                sDown = sRows(I).Substring(sRows(I).IndexOf("<b>") + 3)
                sDown = sDown.Substring(0, sDown.IndexOf("</b>"))
              End If
            ElseIf sRows(I - 1).ToLower.Contains("buy more purchased") And String.IsNullOrEmpty(sPlusT) Then
              If sRows(I).ToLower.Contains("<strong>") And sRows(I).ToLower.Contains("</strong>") Then
                sPlusT = sRows(I).Substring(sRows(I).IndexOf("<strong>") + 8)
                sPlusT = sPlusT.Substring(0, sPlusT.IndexOf("</strong>"))
              End If
            End If
          End If
        End If
      Next
      If String.IsNullOrEmpty(sDownT) Then
        RaiseError("Usage Read Failed: Unable to parse data!", "WB-B Read Table", Table)
      Else
        RaiseEvent ConnectionWBXResult(Me, New TYPEBResultEventArgs(StrToVal(sDown, MBPerGB), StrToVal(sDownT, MBPerGB) + StrToVal(sPlusT, MBPerGB), Now, imSlowed, imFree))
      End If
    Else
      RaiseError("Usage Read Failed: Unable to locate data table!", "WB Read Table", Table)
    End If
  End Sub
#End Region
#Region "EX"
  Private Sub EX_Login_Prepare_Response(Response As String, ResponseURI As Uri, TryCount As Integer)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "mysso." & sProvider Then
      RaiseError("Login Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower = "/federation/ssoredirect/metaalias/idp" And Not ResponseURI.AbsolutePath.ToLower = "/federation/ssoredirect/metaalias/wsubscriber/idp" Then
      RaiseError("Login Failed: Could not understand response.", "EX Login Prepare Response", Response, ResponseURI)
      Return
    End If
    If Response.ToLower.Contains("unable to process request") Then
      RaiseError("Prepare Failed: The server may be down.")
      Return
    End If
    If Not Response.Contains("<form") Or Not Response.Contains("name=""Login""") Then
      RaiseError("Prepare Failed: Login form not found.", "EX Login Prepare Response", Response, ResponseURI)
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
      RaiseError("Prepare Failed: GOTO value not found.", "EX Login Prepare Response", Response, ResponseURI)
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
      RaiseError("Prepare Failed: SunQueryParamsString value not found.", "Exede Login Prepare Response", Response, ResponseURI)
      Return
    End If
    EX_Login(sURI, sGOTO, sSQPS, TryCount)
  End Sub
  Private Sub EX_Login(sURI As String, sGOTO As String, sSQPS As String, TryCount As Integer)
    MakeSocket(True)
    Dim sSend As String = "realm=" & srlFunctions.PercentEncode("/") &
                         "&IDToken1=" & srlFunctions.PercentEncode(sUsername) &
                         "&IDToken2=" & srlFunctions.PercentEncode(sPassword) &
                         "&IDButton=Sign+in" &
                         "&goto=" & srlFunctions.PercentEncode(sGOTO) &
                         "&SunQueryParamsString=" & srlFunctions.PercentEncode(sSQPS) &
                         "&encoded=true" &
                         "&gx_charset=UTF-8"
    If TryCount = 0 Then
      BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, sURI)
    Else
      BeginAttempt(ConnectionStates.Login, ConnectionSubStates.AuthenticateRetry, TryCount, sURI)
    End If
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(sURI), sSend, responseURI, responseData)
    If ClosingTime Then Return
    EX_Login_Response(responseData, responseURI, TryCount)
  End Sub
  Private Sub EX_Login_Response(Response As String, ResponseURI As Uri, TryCount As Integer)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "mysso." & sProvider And Not ResponseURI.Host.ToLower = "my." & sProvider Then
      RaiseError("Login Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower = "/federation/ssoredirect/metaalias/idp" And Not ResponseURI.AbsolutePath.ToLower = "/federation/ui/login" And Not ResponseURI.AbsolutePath.ToLower = "/federation/ssoredirect/metaalias/wsubscriber/idp" Then
      If Response.Contains("window.location.href") Then
        TryCount += 1
        If TryCount > 15 Then
          RaiseError("Login Failed: Server redirected too many times.")
          Return
        End If
        MakeSocket(True)
        Dim sRedirURI As String = Nothing
        If Response.Contains("window.location.href = url + escapedHash;") Then
          sRedirURI = Response.Substring(Response.IndexOf("var url"))
        Else
          sRedirURI = Response.Substring(Response.IndexOf("window.location.href"))
        End If
        sRedirURI = sRedirURI.Substring(sRedirURI.IndexOf("'") + 1)
        sRedirURI = sRedirURI.Substring(0, sRedirURI.IndexOf("'"))
        If sRedirURI = "/" Then
          sRedirURI = ResponseURI.AbsoluteUri.Substring(0, ResponseURI.AbsoluteUri.IndexOf("/", ResponseURI.AbsoluteUri.IndexOf("//") + 2))
        ElseIf sRedirURI.StartsWith("/") Then
          sRedirURI = "https://" & ResponseURI.Host & sRedirURI
        End If
        BeginAttempt(ConnectionStates.Login, ConnectionSubStates.AuthenticateRetry, TryCount, sRedirURI)
        Dim response2Data As String = Nothing
        Dim response2URI As Uri = Nothing
        SendGET(New Uri(sRedirURI), response2URI, response2Data)
        If ClosingTime Then Return
        EX_Login_Response(response2Data, response2URI, TryCount)
      ElseIf Response.Contains("maintenance") Then
        RaiseError("Login Failed: Server Down for Maintenance.")
      ElseIf Response.Contains("https://DOMAIN.my.salesforce.com") Then
        RaiseError("Login Failed: Server Down for Maintenance.")
      Else
        RaiseError("Login Failed: Could not understand response.", True, "EX Login Response", Response, ResponseURI)
      End If
      Return
    End If
    If Response.Contains("Access rights validated") Then
      Dim sURI As String = Nothing
      If Response.Contains("<form method=""post"" action=""") Then
        sURI = Response.Substring(Response.IndexOf("<form method=""post"" action="""))
        sURI = sURI.Substring(sURI.IndexOf("action=""") + 8)
        sURI = sURI.Substring(0, sURI.IndexOf(""">"))
        sURI = srlFunctions.HexDecode(sURI)
      End If
      If String.IsNullOrEmpty(sURI) Then sURI = ResponseURI.AbsoluteUri
      Dim sSAMLResponse As String = Nothing
      If Response.Contains("<input type=""hidden"" name=""SAMLResponse"" value=""") Then
        sSAMLResponse = Response.Substring(Response.IndexOf("<input type=""hidden"" name=""SAMLResponse"" value="""))
        sSAMLResponse = sSAMLResponse.Substring(sSAMLResponse.IndexOf("value=""") + 7)
        sSAMLResponse = sSAMLResponse.Substring(0, sSAMLResponse.IndexOf(""" />"))
      End If
      If String.IsNullOrEmpty(sSAMLResponse) Then
        RaiseError("Login Failed: SAML Response value not found.", "EX Login Response", Response, ResponseURI)
        Return
      End If
      Dim sRelay As String = Nothing
      If Response.Contains("<input type=""hidden"" name=""RelayState"" value=""") Then
        sRelay = Response.Substring(Response.IndexOf("<input type=""hidden"" name=""RelayState"" value="""))
        sRelay = sRelay.Substring(sRelay.IndexOf("value=""") + 7)
        sRelay = sRelay.Substring(0, sRelay.IndexOf(""" />"))
      End If
      If String.IsNullOrEmpty(sRelay) Then
        RaiseError("Login Failed: Relay State value not found.", "EX Login Response", Response, ResponseURI)
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
        RaiseError("Unknown Login Error.", "EX Login Response", Response, ResponseURI)
      End If
    ElseIf Response.ToLower.Contains("sorry, we've encountered an unexpected error.") Then
      RaiseError("Login Failed: Server encountered an unexpected error.")
    Else
      RaiseError("Could not log in.", "EX Login Response", Response, ResponseURI)
    End If
  End Sub
  Private Sub EX_Authenticate(sURI As String, SAMLResponse As String, RelayState As String)
    MakeSocket(True)
    Dim sSend As String = "SAMLResponse=" & srlFunctions.PercentEncode(srlFunctions.HexDecode(SAMLResponse)) & "&RelayState=" & srlFunctions.PercentEncode(srlFunctions.HexDecode(RelayState))
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadHome, 0, sURI)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(sURI), sSend, responseURI, responseData)
    If ClosingTime Then Return
    EX_Authenticate_Response(responseData, responseURI)
  End Sub
  Private Sub EX_Authenticate_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "myexede.force.net" And Not ResponseURI.Host.ToLower = "my." & sProvider Then
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
        If sProvider = "exede.net" Then
          sURL = "https://" & ResponseURI.Host & "/dashboard"
        ElseIf sProvider = "satelliteinternetco.com" Then
          sURL = "https://" & ResponseURI.Host & "/subscriber_dashboard"
        Else
          RaiseError("Authentication Failed: Unknown Provider - Can't determine Dashboard URL.")
        End If
      End If
      If justATest Then
        RaiseEvent LoginComplete(Me, New LoginCompletionEventArgs(SatHostTypes.WildBlue_EXEDE))
        Return
      End If
      EX_Download_Homepage(sURL)
      Return
    End If
    If Response.Contains("maintenance") Then
      RaiseError("Authentication Failed: Server Down for Maintenance.")
    Else
      RaiseError("Authentication Failed: Could not understand response.", True, "EX Authenticate Response", Response, ResponseURI)
    End If
  End Sub
  Private Structure AjaxEntry
    Public ID As Byte
    Public Iteration As Byte
    Public Sub New(bID As Byte, bI As Byte)
      ID = bID
      Iteration = bI
    End Sub
  End Structure
  Private AJAXOrder() As Byte
  Public ReadOnly Property ExedeAJAXFirstTryRequests As Integer
    Get
      Return AJAXOrder.Length - 1
    End Get
  End Property
  Public ReadOnly Property ExedeAJAXSecondTryRequests As Integer
    Get
      Return 12 - 1
    End Get
  End Property
  Private Sub EX_Download_Homepage(sURI As String)
    MakeSocket(True)
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadAJAX, 1, sURI)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendGET(New Uri(sURI), responseURI, responseData)
    If ClosingTime Then Return
    EX_Ajax_Response(responseData, responseURI, New AjaxEntry(AJAXOrder(0), 1))
  End Sub
  Private Sub EX_Ajax_Response(Response As String, ResponseURI As Uri, NextAjaxID As AjaxEntry)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "myexede.force.net" And Not ResponseURI.Host.ToLower = "my." & sProvider Then
      RaiseError("AJAX Load Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower = "/dashboard" And Not ResponseURI.AbsolutePath.ToLower = "/subscriber_dashboard" Then
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
        RaiseError("AJAX Load Failed: Could not understand response.", True, "EX Ajax Response", Response, ResponseURI)
      End If
      Return
    End If
    If Response.Contains("amount-used") Then
      Dim sTable As String = Response.Substring(Response.LastIndexOf("<div class=""amount-used"""))
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
      Dim sURL As String = Nothing
      If sProvider = "exede.net" Then
        sURL = "https://" & ResponseURI.Host & "/dashboard?refURL=https%3A%2F%2F" & ResponseURI.Host & "%2Fdashboard"
      ElseIf sProvider = "satelliteinternetco.com" Then
        sURL = "https://" & ResponseURI.Host & "/subscriber_dashboard?refURL=https%3A%2F%2F" & ResponseURI.Host & "%2Fsubscriber_dashboard"
      Else
        RaiseError("AJAX Load Failed: Unknown Provider - Can't determine Dashboard URL.")
      End If
      EX_Download_Ajax(sURL, NextAjaxID, sViewState, sVSVersion, sVSMAC, sVSCSRF)
    ElseIf Response.Contains("https://myexede.force.com/atlasPlanInvalid") Or Response.Contains("https://my." & sProvider & "/atlasPlanInvalid") Then
      RaiseError("AJAX Load Failed: You no longer have access to MyExede. Please check back again or contact Customer Care [(855) 463-9333] if the problem persists.")
    ElseIf Response.Contains("Concurrent requests limit exceeded.") Then
      RaiseError("AJAX Load Failed: Too many requests. Check for usage data less often.")
    ElseIf Response.Contains("maintenance") Then
      RaiseError("AJAX Load Failed: Server Down for Maintenance.")
    ElseIf Response.Contains("window.location.href") Then
      RaiseError("AJAX Load Failed: Sent back to login page.")
    ElseIf Response.Contains("An internal server error ") Or Response.Contains("Something went wrong.") Then
      RaiseError("AJAX Load Failed: Server Error - Exede may be having trouble.")
    Else
      RaiseError("AJAX Load Failed: Could not find AJAX ViewState variables.", "EX Ajax Response", Response, ResponseURI)
    End If
  End Sub
  Private Sub EX_Download_Ajax(sURI As String, AjaxID As AjaxEntry, sViewState As String, sVSVersion As String, sVSMAC As String, sVSCSRF As String)
    MakeSocket(True)
    Dim newID As Byte = AjaxID.ID
    Dim newType As Byte = AjaxID.Iteration
    If AjaxID.ID > ExedeAJAXSecondTryRequests Then
      BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTable, 0, sURI)
      newID = 1
      newType += 1
    ElseIf AjaxID.Iteration = 1 Then
      Dim bShown As Boolean = False
      For I As Integer = 0 To AJAXOrder.Length - 1
        If AjaxID.ID = AJAXOrder(I) Then
          If I >= AJAXOrder.Length - 1 Then
            BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTable, 0, sURI)
            newID = 1
            newType += 1
          Else
            BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadAJAX, I + 1, sURI)
            newID = AJAXOrder(I + 1)
          End If
          bShown = True
          Exit For
        End If
      Next
      If Not bShown Then
        BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadAJAX, AjaxID.ID, sURI)
        newID += 1
      End If
    Else
      BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadAJAXRetry, AjaxID.ID, sURI)
      newID += 1
    End If
    Dim sSend As String = "AJAXREQUEST=_viewRoot" &
             "&j_id0%3AidForm=j_id0%3AidForm" &
             "&com.salesforce.visualforce.ViewState=" & srlFunctions.PercentEncode(sViewState) &
             "&com.salesforce.visualforce.ViewStateVersion=" & srlFunctions.PercentEncode(sVSVersion) &
             "&com.salesforce.visualforce.ViewStateMAC=" & srlFunctions.PercentEncode(sVSMAC) &
             "&com.salesforce.visualforce.ViewStateCSRF=" & srlFunctions.PercentEncode(sVSCSRF) &
             "&j_id0%3AidForm%3Aj_id" & AjaxID.ID & "=j_id0%3AidForm%3Aj_id" & AjaxID.ID
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(sURI), sSend, responseURI, responseData)
    If ClosingTime Then Return
    EX_Ajax_Response(responseData, responseURI, New AjaxEntry(newID, newType))
  End Sub
  Private Sub EX_Read_Table(Table As String)
    If Not Table.Contains("amount-used") Then
      RaiseError("Usage Read Failed: Unable to locate data table", "EX Read Table", Table)
      Return
    End If
    Dim Used As String = Table.Substring(Table.IndexOf("amount-used"))
    Used = Used.Substring(Used.IndexOf(""">") + 2)
    Used = Used.Substring(0, Used.IndexOf("</"))
    Dim Total As String = Nothing
    If sProvider = "exede.net" Then
      If Table.Contains("<strong>") Then
        Total = Table.Substring(Table.IndexOf("<strong>") + 8)
        If Total.Contains("</") Then
          Total = Total.Substring(0, Total.IndexOf("</"))
        Else
          RaiseError("Usage Read Failed: Unable to parse Total!", "EX Read Table", Table)
        End If
      End If
    ElseIf sProvider = "mysatelliteinternetco.com" Then
      If Table.Contains("</strong> used of ") Then
        Total = Table.Substring(Table.IndexOf("</strong> used of ") + 18)
        If Total.Contains(" GB allowance") Then
          Total = Total.Substring(0, Total.IndexOf(" GB allowance"))
        ElseIf Total.Contains("</") Then
          Total = Total.Substring(0, Total.IndexOf("</"))
        Else
          RaiseError("Usage Read Failed: Unable to parse Total!", "EX Read Table", Table)
        End If
      End If
    End If
    Dim lUsed As Long = StrToVal(Used, MBPerGB)
    Dim lTotal As Long = StrToVal(Total, MBPerGB)
    If lUsed = 0 And lTotal = 0 Then
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginIssue, "Data temporarily unavailable."))
    Else
      If lTotal > 0 Then
        RaiseEvent ConnectionWBXResult(Me, New TYPEBResultEventArgs(lUsed, lTotal, Now, imSlowed, imFree))
      Else
        RaiseEvent ConnectionWBXResult(Me, New TYPEBResultEventArgs(lUsed, 150000, Now, imSlowed, imFree))
      End If
    End If
  End Sub
#End Region
#Region "RP"
  Private Sub RP_Login_Retry(sURI As String)
    MakeSocket(False)
    Dim sUser As String = sAccount.Substring(0, sAccount.LastIndexOf("@"))
    Dim sSend As String = "warningTrip=true&userName=" & sUser & "&passwd=" & srlFunctions.PercentEncode(sPassword)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.AuthenticateRetry, 0, sURI)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(sURI), sSend, responseURI, responseData)
    If ClosingTime Then Return
    RP_Login_Response(responseData, responseURI, True)
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
      RaiseError("Login Failed: Could not understand response.", True, "RP Login Response", Response, ResponseURI)
      Return
    End If
    If Not String.IsNullOrEmpty(ResponseURI.Query) Then
      If ResponseURI.Query.ToLower.Contains("pass=false") Then
        RaiseError("Login Failed: Incorrect password.")
        Return
      End If
    End If
    If Not Response.ToLower.Contains("confirmchange(msg);") Then
      RaiseError("Login Failed: Sent back to login page.", True)
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
    MakeSocket(False)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendGET(New Uri(uriString), responseURI, responseData)
    If ClosingTime Then Return
    RP_Usage_Response(responseData, responseURI)
  End Sub
  Private Sub RP_Usage_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = sProvider & ".ruralportal.net" Then
      RaiseError("Usage Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If Not Response.Contains("Current Usage") Then
      RaiseError("Usage Failed: Failed to log in.", True, "RP Usage Response", Response, ResponseURI)
      Return
    End If
    If Response.Contains("Usage data is not available.") Then
      RaiseError("Usage Failed: Data temporarily unavailable.")
      Return
    End If
    If Not Response.Contains("<!-- usage bar -->") Then
      RaiseError("Usage Failed: Could not find usage meter.", "RP Usage Response", Response, ResponseURI)
      Return
    End If
    Dim sFind As String = Response.Substring(Response.IndexOf("<!-- usage bar -->"))
    If Not sFind.Contains("<table") Then
      RaiseError("Usage Failed: Could not find usage meter table.", "RP Usage Response", Response, ResponseURI)
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
      RaiseError("Usage Failed: Could not parse usage meter table.", "RP Usage Response", Response, ResponseURI)
    End If
  End Sub
  Private Sub RP_Read_Table(Table As String)
    If Table.Contains("MB)") Then
      If justATest Then
        RaiseEvent LoginComplete(Me, New LoginCompletionEventArgs(SatHostTypes.RuralPortal_LEGACY))
        Return
      End If
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
        RaiseError("Usage Read Failed: Unable to parse data!", "RP Read Table", Table)
      Else
        RaiseEvent ConnectionRPLResult(Me, New TYPEAResultEventArgs(StrToVal(sDown), StrToVal(sDownT), StrToVal(sUp), StrToVal(sUpT), Now, imSlowed, imFree))
      End If
    ElseIf Table.Contains(" GB (") Then
      If justATest Then
        RaiseEvent LoginComplete(Me, New LoginCompletionEventArgs(SatHostTypes.RuralPortal_EXEDE))
        Return
      End If
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
        RaiseError("Usage Read Failed: Unable to parse data!", "RP Read Table", Table)
      Else
        RaiseEvent ConnectionRPXResult(Me, New TYPEBResultEventArgs(StrToVal(sDown, MBPerGB) + StrToVal(sOverhead, MBPerGB), StrToVal(sDownT, MBPerGB), Now, imSlowed, imFree))
      End If
    Else
      RaiseError("Usage Read Failed: Unable to locate data table!", "RP Read Table", Table)
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
      RaiseError("Login Prepare Failed: Could not understand response.", True, "DN Login Response", Response, ResponseURI)
      Return
    End If
    DN_Login("https://my.dish.com/customercare/saml/login?target=" & srlFunctions.PercentEncode("/usermanagement/processSynacoreResponse.do?pageurl=myinternet") & "&message=&forceAuthn=true")
  End Sub
  Private Sub DN_Login(sURI As String)
    MakeSocket(False, False)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.ReadLogin, 0, sURI)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendGET(New Uri(sURI), responseURI, responseData)
    If ClosingTime Then Return
    iHist += 1
    DN_Login_Response(responseData, responseURI)
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
    MakeSocket(False, False)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.AuthPrepare, 0, sURI)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendGET(New Uri(sURI), responseURI, responseData)
    If ClosingTime Then Return
    iHist += 1
    DN_Login_FirstBook_Response(responseData, responseURI)
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
      RaiseError("Login Failed: Could not understand response.", True, "DN Login FirstBookend Response", Response, ResponseURI)
      Return
    End If
    If Not ResponseURI.Query.StartsWith("?") Then
      RaiseError("Login Failed: AuthState is missing!")
      Return
    End If
    DN_Login_Authenticate(ResponseURI.OriginalString)
  End Sub
  Private Sub DN_Login_Authenticate(sURI As String)
    MakeSocket(False, False)
    Dim sSend As String = "username=" & srlFunctions.PercentEncode(sUsername) &
                          "&password=" & srlFunctions.PercentEncode(sPassword) &
                          "&login_type=username,password" &
                          "&source=" &
                          "&source_button="
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, sURI)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(sURI), sSend, responseURI, responseData)
    If ClosingTime Then Return
    iHist += 1
    DN_Login_Authenticate_Response(responseData, responseURI)
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
        MakeSocket(False, False)
        BeginAttempt(ConnectionStates.Login, ConnectionSubStates.AuthenticateRetry, 0, sURL)
        Dim response2Data As String = Nothing
        Dim response2URI As Uri = Nothing
        SendGET(New Uri(sURL), response2URI, response2Data)
        If ClosingTime Then Return
        iHist += 1
        DN_Login_Authenticate_Response(response2Data, response2URI)
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
        RaiseError("Login Failed: Server issue.", "DN Login Authenticate Response", Response, ResponseURI)
      End If
    ElseIf Response.ToLower.Contains("you've submitted your request too soon. please wait and try again.") Then
      RaiseError("Login Failed: Too many requests. Check for usage data less often.")
    ElseIf Response.ToLower.Contains("captcha") Then
      RaiseError("Login Failed: Server requires a captcha to be entered to validate your account. Please log in through your web browser, then try again.")
    Else
      RaiseError("Login Failed: Could not understand response.", True, "DN Login Authenticate Response", Response, ResponseURI)
    End If
  End Sub
  Private Sub DN_Login_LastBook(sURI As String)
    MakeSocket(False, False)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Verify, 0, sURI)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendGET(New Uri(sURI), responseURI, responseData)
    If ClosingTime Then Return
    iHist += 1
    DN_Login_LastBook_Response(responseData, responseURI)
  End Sub
  Private Sub DN_Login_LastBook_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "identity1.dishnetwork.com" Then
      RaiseError("Login Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower.Contains("/lastbookend.php") Then
      RaiseError("Login Failed: Could not understand response.", True, "DN Login LastBookend Response", Response, ResponseURI)
      Return
    End If
    If Response.Contains("SAMLResponse"" value=""") Then
      Dim SAMLResponse As String
      SAMLResponse = Response.Substring(Response.IndexOf("SAMLResponse"" value=""") + 21)
      If SAMLResponse.Contains(""" />") Then
        SAMLResponse = SAMLResponse.Substring(0, SAMLResponse.IndexOf(""" />"))
        DN_Login_Verify(SAMLResponse)
      Else
        RaiseError("Login Failed: Incomplete SAML Response Data.", "DN Login LastBookend Response", Response, ResponseURI)
      End If
    ElseIf Response.Contains("The system is currently unavailable. Please try again later.") Then
      RaiseError("System currently unavailable.")
    ElseIf Response.Contains("<div class=""custom_message_text"">") Then
      Dim sErrMsg As String = Response.Substring(Response.IndexOf("<div class=""custom_message_text"">") + 33)
      sErrMsg = sErrMsg.Substring(0, sErrMsg.IndexOf("<"))
      RaiseError(sErrMsg.Trim)
    Else
      RaiseError("Login Failed: No SAML Response", "DN Login LastBookend Response", Response, ResponseURI)
    End If
  End Sub
  Private Sub DN_Login_Verify(SAMLResponse As String)
    MakeSocket(False, False)
    Dim uriString As String = "https://my.dish.com/customercare/saml/post"
    Dim sSend As String = "SAMLResponse=" & srlFunctions.PercentEncode(SAMLResponse) & "&RelayState=" & srlFunctions.PercentEncode("/usermanagement/processSynacoreResponse.do?pageurl=myinternet")
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadHome, 0, uriString)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(uriString), sSend, responseURI, responseData)
    If ClosingTime Then Return
    iHist += 1
    DN_Login_Verify_Response(responseData, responseURI)
  End Sub
  Private Sub DN_Login_Verify_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI, True) Then Return
    If Not ResponseURI.Host.ToLower = "my.dish.com" Then
      RaiseError("Login Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower.Contains("/processsynacoreresponse.do") Then
      RaiseError("Login Failed: Could not understand response.", True, "DishNet Login Verify Response", Response, ResponseURI)
      Return
    End If
    If justATest Then
      RaiseEvent LoginComplete(Me, New LoginCompletionEventArgs(SatHostTypes.DishNet_EXEDE))
      Return
    End If
    DN_Download_Home()
  End Sub
  Private Sub DN_Download_Home()
    MakeSocket(False, False)
    Dim uriString As String = "https://my.dish.com/customercare/usermanagement/getAccountNumberByUUID.do"
    Dim sSend As String = "check="
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTable, 0, uriString)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(uriString), sSend, responseURI, responseData)
    If ClosingTime Then Return
    iHist += 1
    DN_Download_Home_Response(responseData, responseURI)
  End Sub
  Private Sub DN_Download_Home_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI, True) Then Return
    If Not ResponseURI.Host.ToLower = "my.dish.com" Then
      RaiseError("Login Failed: Connection redirected to """ & ResponseURI.OriginalString & """, check your Internet connection.")
      Return
    End If
    If (ResponseURI.AbsolutePath.ToLower.Contains("/loadpage.do") And ResponseURI.Query.ToLower.Contains("myinternet")) Or ResponseURI.AbsolutePath.ToLower.Contains("/myinternet") Then
      DN_Download_Table_Response(Response, ResponseURI)
      Return
    End If
    If (ResponseURI.AbsolutePath.ToLower.Contains("/loadpage.do") And ResponseURI.Query.ToLower.Contains("myaccountsummary")) Or ResponseURI.AbsolutePath.ToLower.Contains("/myaccountsummary") Then
      DN_Download_Table()
      Return
    End If
    RaiseError("Home Read Failed: Could not load home page. Redirected to """ & ResponseURI.OriginalString & """.", "DN Download Home Response", Response, ResponseURI)
  End Sub
  Private Sub DN_Download_Table()
    MakeSocket(False, False)
    Dim uriString As String = "https://my.dish.com/customercare/myaccount/myinternet"
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTableRetry, 0, uriString)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendGET(New Uri(uriString), responseURI, responseData)
    If ClosingTime Then Return
    iHist += 1
    DN_Download_Table_Response(responseData, responseURI)
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
    If Not ResponseURI.AbsolutePath.ToLower.Contains("/loadpage.do") And Not ResponseURI.AbsolutePath.ToLower.Contains("/myinternet") Then
      RaiseError("Usage Failed: Could not load usage meter page. Redirected to """ & ResponseURI.OriginalString & """.", "DN Download Table Response", Response, ResponseURI)
      Return
    End If
    If ResponseURI.Query = "?page=myaccountsummary_res" Then
      DN_Download_Table()
    ElseIf ResponseURI.Query = "?pageurl=myinternet" Or ResponseURI.AbsolutePath.ToLower.Contains("/myinternet") Then
      If Not Response.Contains("widgetLoadUrls[widgetListCount]") Then
        RaiseError("Usage Failed: Could not find usage meter.", "DN Download Table Response", Response, ResponseURI)
        Return
      End If
      Dim sUsageDiv As String = Response.Substring(Response.IndexOf("widgetLoadUrls[widgetListCount]"))
      If Not sUsageDiv.Contains("</form>") Then
        RaiseError("Usage Failed: Could not parse usage data.", "DN Download Table Response", Response, ResponseURI)
        Return
      End If
      sUsageDiv = sUsageDiv.Substring(0, sUsageDiv.IndexOf("</form>"))
      If Not sUsageDiv.ToLower.Contains("remaining capacity") Then
        RaiseError("Usage Failed: Could not detect usage data.", "DN Download Table Response", Response, ResponseURI)
        Return
      End If
      ReadUsage(sUsageDiv)
    Else
      RaiseError("Usage Failed: Redirected to Unknown Page [" & ResponseURI.Query & "]", "DN Download Table Response", Response, ResponseURI)
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
                RaiseError("Not sure about this usage data (Off-Peak Max). Gonna take a closer look...", False, "DN Read Table", Table, Nothing)
                opM = htmlParts(0)
              End If
            End If
          End If
          If Not String.IsNullOrEmpty(htmlParts(1)) Then
            If String.IsNullOrEmpty(atM) Then
              atM = htmlParts(1)
            Else
              If Not atM = htmlParts(1) Then
                RaiseError("Not sure about this usage data (Anytime Max). Gonna take a closer look...", False, "DN Read Table", Table, Nothing)
                atM = htmlParts(1)
              End If
            End If
          End If
          If Not String.IsNullOrEmpty(htmlParts(2)) Then
            If String.IsNullOrEmpty(opV) Then
              opV = htmlParts(2)
            Else
              If Not opV = htmlParts(2) Then
                RaiseError("Not sure about this usage data (Off-Peak Value). Gonna take a closer look...", False, "DN Read Table", Table, Nothing)
                opV = htmlParts(2)
              End If
            End If
          End If
          If Not String.IsNullOrEmpty(htmlParts(3)) Then
            If String.IsNullOrEmpty(atV) Then
              atV = htmlParts(3)
            Else
              If Not atV = htmlParts(3) Then
                RaiseError("Not sure about this usage data (Anytime Value). Gonna take a closer look...", False, "DN Read Table", Table, Nothing)
                atV = htmlParts(3)
              End If
            End If
          End If
          If Not String.IsNullOrEmpty(htmlParts(4)) Then
            If String.IsNullOrEmpty(atxV) Then
              atxV = htmlParts(4)
            Else
              If Not atxV = htmlParts(4) Then
                RaiseError("Not sure about this usage data (Additional Value). Gonna take a closer look...", False, "DN Read Table", Table, Nothing)
                atxV = htmlParts(4)
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
      RaiseEvent ConnectionDNXResult(Me, New TYPEA2ResultEventArgs(lDown, lDownT, lUp, lUpT, Now, imSlowed, imFree))
    Else
      RaiseError("Usage Read Failed: Unable to locate data table!", "DN Read Table", Table)
    End If
  End Sub
#End Region
#End Region
#Region "Useful Functions"
  Private Sub MakeSocket(KeepAlive As Boolean, Optional ManualRedirect As Boolean = True)
    Dim oldEncoding As System.Text.Encoding = System.Text.Encoding.GetEncoding(srlFunctions.LATIN_1)
    If wsSocket IsNot Nothing Then
      oldEncoding = wsSocket.Encoding
      If wsSocket.IsBusy Then wsSocket.Cancel()
      wsSocket = Nothing
    End If
    wsSocket = New WebClientEx(sDataPath)
    wsSocket.KeepAlive = KeepAlive
    wsSocket.Timeout = c_Timeout
    wsSocket.Proxy = c_Proxy
    wsSocket.CookieJar = c_Jar
    wsSocket.Encoding = oldEncoding
    If Not ManualRedirect Then wsSocket.ManualRedirect = False
  End Sub
  Private Sub SendTLSProxy(SendURL As Uri, SendData As String, ByRef ReturnURL As Uri, ByRef ReturnData As String)
    Dim sProtocol As String = "default"
    If (c_Protocol And SecurityProtocolTypeEx.Tls12) = SecurityProtocolTypeEx.Tls12 Then
      sProtocol = "tls12"
    ElseIf (c_Protocol And SecurityProtocolTypeEx.Tls11) = SecurityProtocolTypeEx.Tls11 Then
      sProtocol = "tls11"
    ElseIf (c_Protocol And SecurityProtocolTypeEx.Tls10) = SecurityProtocolTypeEx.Tls10 Then
      sProtocol = "tls10"
    ElseIf (c_Protocol And SecurityProtocolTypeEx.Ssl3) = SecurityProtocolTypeEx.Ssl3 Then
      sProtocol = "ssl3"
    End If
    Dim sFields As String = Nothing
    Dim cl As Net.CookieCollection = Nothing
    For Each cField As Reflection.FieldInfo In c_Jar.GetType().GetFields(Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
      If cField.Name = "m_domainTable" Then
        cl = New Net.CookieCollection
        Dim k As Hashtable = cField.GetValue(c_Jar)
        For Each dEntry As DictionaryEntry In k
          Dim l As SortedList = dEntry.Value.GetType().GetField("m_list", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic).GetValue(dEntry.Value)
          For Each e As DictionaryEntry In l
            Dim cTmp As Net.CookieCollection = e.Value
            If cTmp IsNot Nothing Then cl.Add(cTmp)
          Next
        Next
        sFields = Nothing
        Exit For
      End If
      If cField.Name = "cookies" Then
        cl = New Net.CookieCollection
        Dim cTmp As Net.CookieCollection = cField.GetValue(c_Jar)
        If cTmp IsNot Nothing Then cl.Add(cTmp)
        sFields = Nothing
        Exit For
      End If
      sFields &= cField.Name & ", "
    Next
    If cl Is Nothing Then
      If Not String.IsNullOrEmpty(sFields) Then
        If sFields.EndsWith(", ") Then sFields = sFields.Substring(0, sFields.Length - 2)
        RaiseError("Unsupported CookieContainer: Unable to use TLS Proxy on this Framework.", "Send TLS Proxy", sFields)
      End If
      ReturnURL = Nothing
      ReturnData = "Unsupported CookieContainer: Unable to use TLS Proxy on this Framework."
      Return
    End If
    Dim sCookieData As String = Nothing
    If cl IsNot Nothing Then
      For Each cookie As Net.Cookie In cl
        sCookieData &= cookie.Domain & vbTab &
                       (Not cookie.HttpOnly).ToString.ToLower & vbTab &
                       cookie.Secure.ToString.ToLower & vbTab &
                       cookie.Domain.StartsWith(".").ToString.ToLower & vbTab &
                       cookie.Path & vbTab &
                       (cookie.Expires.ToUniversalTime - New DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds & vbTab &
                       cookie.Name & vbTab &
                       cookie.Value & vbLf
      Next
    End If
    Dim sPOST As String = Nothing
    sPOST &= "protocol=" & srlFunctions.PercentEncode(ToBase64(sProtocol))
    sPOST &= "&url=" & srlFunctions.PercentEncode(ToBase64(SendURL.OriginalString))
    If Not String.IsNullOrEmpty(SendData) Then sPOST &= "&post=" & srlFunctions.PercentEncode(ToBase64(SendData))
    If Not String.IsNullOrEmpty(sCookieData) Then
      If sCookieData.EndsWith(vbLf) Then sCookieData = sCookieData.Substring(0, sCookieData.Length - 1)
      sPOST &= "&cookies=" & srlFunctions.PercentEncode(ToBase64(sCookieData))
    End If
    wsSocket.SendHeaders = New Net.WebHeaderCollection
    wsSocket.SendHeaders.Add(Net.HttpRequestHeader.UserAgent, WebClientCore.UserAgent)
    Dim sRet As String = wsSocket.UploadString(c_TLSProxyAddr, "POST", sPOST)
    If Not sRet.Contains(vbLf) Then
      ReturnURL = Nothing
      ReturnData = sRet
      Return
    End If
    If sRet.Contains("Parse error") Then
      ReturnURL = Nothing
      ReturnData = sRet
      Return
    End If
    Dim sRetParts() As String = Split(sRet, vbLf)
    If Not sRetParts.Length = 3 Then
      ReturnURL = Nothing
      ReturnData = sRet
      Return
    End If
    Dim sRetURL As String = FromBase64(sRetParts(0))
    Dim sRetCookies As String = Nothing
    If Not String.IsNullOrEmpty(sRetParts(1)) Then sRetCookies = FromBase64(sRetParts(1))
    Dim sRetData As String = FromBase64(sRetParts(2))
    ReturnURL = New Uri(sRetURL)
    ReturnData = sRetData
    If Not String.IsNullOrEmpty(sRetCookies) Then
      Dim sCookieParts() As String = Split(sRetCookies, vbLf)
      c_Jar = New Net.CookieContainer
      For Each sCookiePart In sCookieParts
        If String.IsNullOrEmpty(sCookiePart) Then Continue For
        If Not sCookiePart.Contains(vbTab) Then Continue For
        Dim sCookiePartData() As String = Split(sCookiePart, vbTab)
        Dim cAdd As New Net.Cookie(sCookiePartData(6), sCookiePartData(7), sCookiePartData(4), sCookiePartData(0))
        cAdd.HttpOnly = sCookiePartData(1) = "false"
        cAdd.Secure = sCookiePartData(2) = "true"
        If sCookiePartData(5) = "0" Then
          cAdd.Expires = Now.AddHours(1)
        Else
          cAdd.Expires = (New DateTime(1970, 1, 1, 0, 0, 0)).AddSeconds(Val(sCookiePartData(5)))
        End If
        c_Jar.Add(cAdd)
      Next
    End If
  End Sub
  Private Sub SendGET(SendURL As Uri, ByRef ReturnURL As Uri, ByRef ReturnData As String)
    If c_TLSProxy Then
      SendTLSProxy(SendURL, Nothing, ReturnURL, ReturnData)
      Return
    End If
    Dim sRet As String = wsSocket.DownloadString(SendURL.OriginalString)
    ReturnData = sRet
    If wsSocket Is Nothing Then
      ReturnURL = Nothing
    Else
      ReturnURL = wsSocket.ResponseURI
    End If
  End Sub
  Private Sub SendPOST(SendURL As Uri, SendData As String, ByRef ReturnURL As Uri, ByRef ReturnData As String)
    If c_TLSProxy Then
      SendTLSProxy(SendURL, SendData, ReturnURL, ReturnData)
      Return
    End If
    wsSocket.SendHeaders = New Net.WebHeaderCollection
    wsSocket.SendHeaders.Add(Net.HttpRequestHeader.UserAgent, WebClientCore.UserAgent)
    Dim sRet As String = wsSocket.UploadString(SendURL.OriginalString, "POST", SendData)
    ReturnData = sRet
    If wsSocket Is Nothing Then
      ReturnURL = Nothing
    Else
      ReturnURL = wsSocket.ResponseURI
    End If
  End Sub
  Private Function FromBase64(base64Str As String) As String
    Dim bRet() As Byte
    Try
      bRet = Convert.FromBase64String(base64Str)
    Catch ex As Exception
      Return Nothing
    End Try
    Dim sRet As String = System.Text.Encoding.UTF8.GetString(bRet)
    If sRet.Contains(vbNullChar) Then sRet = sRet.Substring(0, sRet.IndexOf(vbNullChar))
    Return sRet
  End Function
  Private Function ToBase64(regularStr As String) As String
    Dim bUTF() As Byte = System.Text.Encoding.UTF8.GetBytes(regularStr)
    Return Convert.ToBase64String(bUTF)
  End Function
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
    If response.StartsWith("Could not resolve host: ") Then
      RaiseError("The server is unavailable. Please try again later.")
      Return True
    End If
    If responseURI Is Nothing Then
      RaiseError(response)
      Return True
    End If
    Return False
  End Function
  Private Sub RaiseError(ErrorMessage As String)
    If String.IsNullOrEmpty(Trim(ErrorMessage)) Then Return
    Dim FailureText As String = Nothing
    RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, ErrorMessage))
  End Sub
  Private Sub RaiseError(ErrorMessage As String, AccountTypeGetsReset As Boolean)
    If String.IsNullOrEmpty(Trim(ErrorMessage)) Then Return
    If AccountTypeGetsReset Then
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.FatalLoginFailure, ErrorMessage))
    Else
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, ErrorMessage))
    End If
  End Sub
  Private Sub RaiseError(ErrorMessage As String, FailureLocation As String, FailureData As String, Optional FailureAddress As Uri = Nothing)
    If String.IsNullOrEmpty(Trim(ErrorMessage)) Then Return
    Dim FailureText As String
    If FailureAddress Is Nothing Then
      FailureText = "Error at " & FailureLocation & ": """ & ErrorMessage & """" & vbNewLine & "Data: " & vbNewLine & FailureData
    Else
      FailureText = "Error at " & FailureLocation & ": """ & ErrorMessage & """" & vbNewLine & "URL: {" & FailureAddress.OriginalString & "}" & vbNewLine & "Data: " & vbNewLine & FailureData
    End If
    RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, ErrorMessage, FailureText))
  End Sub
  Private Sub RaiseError(ErrorMessage As String, AccountTypeGetsReset As Boolean, FailureLocation As String, FailureData As String, FailureAddress As Uri)
    If String.IsNullOrEmpty(Trim(ErrorMessage)) Then Return
    Dim FailureText As String
    If FailureAddress Is Nothing Then
      FailureText = "Error at " & FailureLocation & ": """ & ErrorMessage & """" & vbNewLine & "Data: " & vbNewLine & FailureData
    Else
      FailureText = "Error at " & FailureLocation & ": """ & ErrorMessage & """" & vbNewLine & "URL: {" & FailureAddress.OriginalString & "}" & vbNewLine & "Data: " & vbNewLine & FailureData
    End If
    If AccountTypeGetsReset Then
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.FatalLoginFailure, ErrorMessage, FailureText))
    Else
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, ErrorMessage, FailureText))
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
