Public Class localRestrictionTracker
  Implements IDisposable

  Public Enum SatHostTypes
    WildBlue
    Exede
    RuralPortal
    DishNet
    Other
  End Enum

  Public Class ConnectionFailureEventArgs
    Inherits EventArgs
    Public Enum FailureType
      UnknownAccountType
      UnknownAccountDetails
      LoginFailure
      FatalLoginFailure
      ConnectionTimeout
      SSLFailureBypass
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
  Public Class ConnectionWBResultEventArgs
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
  Public Event ConnectionWBResult(sender As Object, e As ConnectionWBResultEventArgs)
  Public Class ConnectionEResultEventArgs
    Inherits EventArgs
    Private m_Down As Long
    Private m_Up As Long
    Private m_Over As Long
    Private m_Limit As Long
    Private m_More As Long
    Private m_Update As Date
    Public Sub New(lDown As Long, lUp As Long, lOver As Long, lLimit As Long, lMore As Long, dUpdate As Date)
      m_Down = lDown
      m_Up = lUp
      m_Over = lOver
      m_Limit = lLimit
      m_More = lMore
      m_Update = dUpdate
    End Sub
    Public ReadOnly Property Download As Long
      Get
        Return m_Down
      End Get
    End Property
    Public ReadOnly Property Upload As Long
      Get
        Return m_Up
      End Get
    End Property
    Public ReadOnly Property Over As Long
      Get
        Return m_Over
      End Get
    End Property
    Public ReadOnly Property Limit As Long
      Get
        Return m_Limit
      End Get
    End Property
    Public ReadOnly Property BuyMore As Long
      Get
        Return m_More
      End Get
    End Property
    Public ReadOnly Property Update As Date
      Get
        Return m_Update
      End Get
    End Property
  End Class
  Public Event ConnectionEResult(sender As Object, e As ConnectionEResultEventArgs)
  Public Class ConnectionDNResultEventArgs
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
  Public Event ConnectionDNResult(sender As Object, e As ConnectionDNResultEventArgs)
  Public Class ConnectionRP2ResultEventArgs
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
  Public Event ConnectionRP2Result(sender As Object, e As ConnectionRP2ResultEventArgs)
  Public Class ConnectionRP4ResultEventArgs
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
  Public Event ConnectionRP4Result(sender As Object, e As ConnectionRP4ResultEventArgs)
  Public Class ConnectionStatusEventArgs
    Inherits EventArgs
    Public Enum ConnectionStates
      Login
      Authentication
      TableDownload
      TableRead
      UsageRead

    End Enum
    Private m_state As ConnectionStates
    Public Sub New(status As ConnectionStates)
      m_state = status
    End Sub
    Public ReadOnly Property Status As ConnectionStates
      Get
        Return m_state
      End Get
    End Property
  End Class
  Public Event ConnectionStatus(sender As Object, e As ConnectionStatusEventArgs)

  Private WithEvents wsData As CookieAwareWebClient
  Private tmrReadTimeout As Threading.Timer
  Private mySettings As AppSettings
  Private Const MBPerGB As Integer = 1000
  Private Const sWB As String = "https://myaccount.{0}/wbisp/{2}/{1}.jsp"
  Private Const sRP As String = "https://{0}.ruralportal.net/us/{1}.do"
  Private sAccount, sPassword, sProvider As String
  Private sAttemptedURL As String
  Private sAttemptedTag As String
  Private bCancelled, bErrored As Boolean
  Private imSlowed As Boolean
  Private imFree As Boolean
  Private FullCheck As Boolean = True
  Private myUID, myPass As String
  Private ClosingTime As Boolean
#Region "Initialization Functions"
  Public Sub New(ConfigPath As String)
    If mySettings Is Nothing Then mySettings = New AppSettings(ConfigPath & IO.Path.DirectorySeparatorChar.ToString & "user.config")
    InitAccount()
  End Sub
  Public Sub Connect()
    ResetTimeout()
    If mySettings.AccountType = SatHostTypes.Other Then
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.UnknownAccountType))
    Else
      GetUsage()
    End If
  End Sub
  Private Sub InitAccount()
    bCancelled = False
    sAccount = mySettings.Account
    If Not String.IsNullOrEmpty(mySettings.PassCrypt) Then
      sPassword = StoredPassword.DecryptApp(mySettings.PassCrypt)
    End If
    If Not String.IsNullOrEmpty(sAccount) AndAlso (sAccount.Contains("@") And sAccount.Contains(".")) Then
      sProvider = sAccount.Substring(sAccount.LastIndexOf("@") + 1).ToLower
    Else
      sAccount = String.Empty
      sProvider = String.Empty
    End If
  End Sub
#End Region
#Region "Login Functions"
  Private ReadTimeoutCount As Integer
  Private Sub tmrReadTimeout_Tick()
    ReadTimeoutCount += 1
    Dim TimeOutTime As Integer = mySettings.Timeout
    If TimeOutTime < 60 * 3 Then TimeOutTime = 60 * 3
    If ReadTimeoutCount >= TimeOutTime Then
      If tmrReadTimeout IsNot Nothing Then
        tmrReadTimeout.Dispose()
        tmrReadTimeout = Nothing
      End If
      ReadTimeoutCount = 0
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.ConnectionTimeout))
    End If
  End Sub
  Private Sub ResetTimeout(Optional enable As Boolean = False)
    If tmrReadTimeout IsNot Nothing Then
      tmrReadTimeout.Dispose()
      tmrReadTimeout = Nothing
    End If
    ReadTimeoutCount = 0
    If enable Then tmrReadTimeout = New Threading.Timer(New Threading.TimerCallback(AddressOf tmrReadTimeout_Tick), New Object, 1000, 1000)
  End Sub
  Private Sub GetUsage()
    If String.IsNullOrEmpty(sAccount) Or String.IsNullOrEmpty(sPassword) Or Not sAccount.Contains("@") Then
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.UnknownAccountDetails))
    Else
      Dim sUser As String = sAccount.Substring(0, sAccount.LastIndexOf("@"))
      ResetTimeout(True)
      Login(sUser, sPassword)
    End If
  End Sub
  Private Function KeyCheck(TestKey As String) As Boolean
    If String.IsNullOrEmpty(TestKey) Then Return False
    If TestKey.Contains("-") Then
      Dim sKeys() As String = Split(TestKey, "-")
      If sKeys.Length = 5 Then
        If Trim(sKeys(0)).Length = 6 And Trim(sKeys(1)).Length = 4 And Trim(sKeys(2)).Length = 4 Or Trim(sKeys(3)).Length = 4 Or Trim(sKeys(4)).Length = 6 Then
          Return True
        End If
      End If
    End If
    Return False
  End Function
  Private Sub PrepareLogin()
    If wsData Is Nothing Then
      wsData = New CookieAwareWebClient
    Else
      If wsData.IsBusy Then wsData.CancelAsync()
      wsData.Dispose()
      wsData = Nothing
      wsData = New CookieAwareWebClient
    End If
    If mySettings IsNot Nothing Then
      wsData.Timeout = mySettings.Timeout
      wsData.Proxy = mySettings.Proxy
    End If
  End Sub
  Private Sub Login(sUID As String, sPass As String)
    PrepareLogin()
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStatusEventArgs.ConnectionStates.Login))
    ContinueLogin(sUID, sPass)
  End Sub
  Private Sub ContinueLogin(sUID As String, sPass As String)
    PrepareLogin()
    If mySettings.AccountType = SatHostTypes.WildBlue Or mySettings.AccountType = SatHostTypes.Exede Then
      Dim uriString As String = String.Format(sWB, sProvider, "servLogin", IIf(sProvider.ToLower = "exede.net", "exede.com", sProvider))
      wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
      wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
      sAttemptedURL = uriString
      sAttemptedTag = "LOGIN"
      Dim uriURL As Uri = Nothing
      Try
        uriURL = New Uri(uriString)
      Catch ex As Exception
        uriURL = Nothing
      End Try
      If uriURL Is Nothing Then
        ResetTimeout()
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Unable to create URI from """ & uriString & """!"))
      Else
        Try
          Dim sSend As String = "uid=" & PercentEncode(sUID) & "&userPassword=" & PercentEncode(sPass)
          wsData.UploadStringAsync(uriURL, "POST", sSend, "LOGIN")
        Catch ex As Exception
          ResetTimeout()
          RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Login Send Failure: " & LocalErrorToString(ex)))
        End Try
      End If
    ElseIf mySettings.AccountType = SatHostTypes.RuralPortal Then
      If sProvider.Contains(".") Then sProvider = sProvider.Substring(0, sProvider.LastIndexOf("."))
      Dim uriString As String = String.Format(sRP, sProvider, "login")
      wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
      wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
      Dim sSend As String = "warningTrip=false&userName=" & sUID & "&passwd=" & PercentEncode(sPass)
      sAttemptedURL = uriString
      sAttemptedTag = "LOGIN"
      Dim uriURL As Uri = Nothing
      Try
        uriURL = New Uri(uriString)
      Catch ex As Exception
        uriURL = Nothing
      End Try
      If uriURL Is Nothing Then
        ResetTimeout()
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Unable to create URI from """ & uriString & """!"))
      Else
        Try
          wsData.UploadStringAsync(uriURL, "POST", sSend, "LOGIN")
        Catch ex As Exception
          ResetTimeout()
          RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Login Send Failure: " & LocalErrorToString(ex)))
        End Try
      End If
    ElseIf mySettings.AccountType = SatHostTypes.DishNet Then
      Dim uriString As String = "https://my.dish.com/customercare/saml/login?target=%2Fcustomercare%2Fusermanagement%2FprocessSynacoreResponse.do%3Foverlayuri%3D-broadband-prepBroadBand.do&message=&forceAuthn=true"
      sAttemptedURL = uriString
      sAttemptedTag = "LOGINPAGE"
      myUID = sUID
      myPass = sPass
      Dim uriURL As Uri = Nothing
      Try
        uriURL = New Uri(uriString)
      Catch ex As Exception
        uriURL = Nothing
      End Try
      If uriURL Is Nothing Then
        ResetTimeout()
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Unable to create URI from """ & uriString & """!"))
      Else
        Try
          wsData.DownloadStringAsync(uriURL, "LOGINPAGE")
        Catch ex As Exception
          ResetTimeout()
          RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Login Send Failure: " & LocalErrorToString(ex)))
        End Try
      End If
    End If
  End Sub
  Private Sub wsData_Failure(sender As Object, e As CookieAwareWebClient.ErrorEventArgs) Handles wsData.Failure
    If e.Error.InnerException IsNot Nothing Then
      If e.Error.InnerException.Message = "The remote certificate is invalid according to the validation procedure." Then
        ResetTimeout()
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.SSLFailureBypass, "Invalid Certificate; Please check your system Clock and Certificate Store. Bypassing..."))
        RestartNoCert()
      ElseIf e.Error.Message.Contains("Could not create SSL/TLS secure channel") Or e.Error.InnerException.Message.Contains("Could not create SSL/TLS secure channel") Then
        ResetTimeout()
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.SSLFailureBypass, "SSL/TLS request invalid; Please check your system Clock and Certificate Store. Bypassing..."))
        RestartNoCert()
      ElseIf e.Error.Message.CompareTo("The authentication or decryption has failed.") = 0 Then
        ResetTimeout()
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.SSLFailureBypass, "Authentication Failed; Please check your system Clock and Certificate Store. Bypassing..."))
        RestartNoCert()
      ElseIf e.Error.InnerException.Message.CompareTo("The authentication or decryption has failed.") = 0 Then
        ResetTimeout()
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.SSLFailureBypass, "Authentication Failed; Please check your system Clock and Certificate Store. Bypassing..."))
        RestartNoCert()
      Else
        ResetTimeout()
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Error: " & LocalErrorToString(e.Error)))
      End If
    Else
      ResetTimeout()
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Error: " & LocalErrorToString(e.Error)))
    End If
    If wsData IsNot Nothing Then
      wsData.Dispose()
      wsData = Nothing
    End If
  End Sub
  Private Sub RestartNoCert()
    If wsData IsNot Nothing Then
      Do While wsData.IsBusy
        Threading.Thread.Sleep(100)
        Threading.Thread.Sleep(0)
        Threading.Thread.Sleep(100)
      Loop
    End If
    PrepareLogin()
    Net.ServicePointManager.ServerCertificateValidationCallback = New Net.Security.RemoteCertificateValidationCallback(AddressOf IgnoreCert)
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStatusEventArgs.ConnectionStates.UsageRead))
    GetUsage()
  End Sub
#End Region
#Region "Parsing Functions"
  Private Sub wsData_DownloadStringCompleted(sender As Object, e As System.Net.DownloadStringCompletedEventArgs) Handles wsData.DownloadStringCompleted
    If bCancelled Then
      bCancelled = False
      Exit Sub
    End If
    imSlowed = False
    imFree = False
    Dim sErrMsg As String = String.Empty
    Dim sFailText As String = String.Empty
    Dim bReset As Boolean = False
    Select Case e.UserState
      Case "LOGIN"
        If e.Error Is Nothing Then
          Dim sRet As String = e.Result
          If Not String.IsNullOrEmpty(sRet) Then
            If mySettings.AccountType = SatHostTypes.WildBlue Or mySettings.AccountType = SatHostTypes.Exede Then
              If Not wsData.ResponseURI.Host = "myaccount." & sProvider Then
                sErrMsg = "Login Failed: Domain redirected, check your Internet connection. [" & wsData.ResponseURI.Host & "]"
                bReset = False
              ElseIf sRet.Contains("usage.jsp") Then
                LoadUsage("usage")
              ElseIf sRet.Contains("usage_bm.jsp") Then
                LoadUsage("usage_bm")
              ElseIf sRet.Contains("<div class=""error"">") Then
                Dim sMessage As String = sRet.Substring(sRet.IndexOf("<div class=""error"">"))
                If sMessage.Contains("<b>") Then
                  sMessage = sMessage.Substring(sMessage.IndexOf("<b>") + 3)
                  sMessage = sMessage.Substring(0, sMessage.IndexOf("<"))
                  sErrMsg = "Login Failed (WB/E): " & sMessage
                  bReset = False
                Else
                  sErrMsg = "Login Failed: Could not understand error."
                  sFailText = "WB DIV Error = " & sErrMsg & vbNewLine & sRet
                  bReset = True
                End If
              ElseIf (Not sRet.Contains("ViaSat") And Not sRet.Contains("EchoStar") And Not sRet.Contains("WildBlue")) Or sRet.ToLower.Contains("error") Then
                sErrMsg = "Login Failed: Could not understand response. Check your E-Mail address and make sure the domain is owned by ViaSat."
                bReset = True
              Else
                sErrMsg = "Login Failed: Could not understand response."
                sFailText = "WB Parse Error = " & sErrMsg & vbNewLine & sRet
                bReset = True
              End If
            ElseIf mySettings.AccountType = SatHostTypes.RuralPortal Then
              If Not wsData.ResponseURI.Host = sProvider & ".ruralportal.net" Then
                sErrMsg = "Login Failed: Domain redirected, check your Internet connection. [" & wsData.ResponseURI.Host & "]"
                bReset = False
              ElseIf sRet.Contains("sitemanage.do") Then
                LoadUsage("sitemanage")
              Else
                sErrMsg = "Login Failed: Could not understand response."
                sFailText = "RuralPortal Parse Error = " & sErrMsg & vbNewLine & sRet
                bReset = True
              End If
            Else
              sErrMsg = "Login Failed: Host Type not determined (" & mySettings.AccountType.ToString & ")!"
              bReset = True
            End If
          Else
            sErrMsg = "Login Failed: No page data!"
            bReset = True
          End If
        Else
          If e.Error.InnerException IsNot Nothing Then
            If e.Error.InnerException.Message = "Object reference not set to an instance of an object." Then
              sErrMsg = Nothing
              bReset = False
            ElseIf e.Error.InnerException.Message.CompareTo("The authentication or decryption has failed.") = 0 Then
              ResetTimeout()
              RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.SSLFailureBypass, "Authentication Failed; Please check your system Clock and Certificate Store. Bypassing..."))
              RestartNoCert()
              Exit Sub
            Else
              sErrMsg = "Login Error: " & LocalErrorToString(e.Error) & " loading " & sAttemptedURL
              bReset = True
            End If
          Else
            sErrMsg = "Login Error: " & LocalErrorToString(e.Error) & " loading " & sAttemptedURL
            bReset = True
          End If
        End If
      Case "USAGE"
        If e.Error Is Nothing Then
          Dim sRet As String = e.Result
          If Not String.IsNullOrEmpty(sRet) Then
            If mySettings.AccountType = SatHostTypes.WildBlue Or mySettings.AccountType = SatHostTypes.Exede Then
              If Not wsData.ResponseURI.Host = "myaccount." & sProvider Then
                sErrMsg = "Usage Failed: Domain redirected, check your Internet connection. [" & wsData.ResponseURI.Host & "]"
                bReset = False
              ElseIf sRet.Contains("Usage Meter") Then
                Dim sFind As String = sRet.Substring(sRet.IndexOf("Usage Meter"))
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
                    sErrMsg = "Usage Failed: Could not parse usage meter."
                    sFailText = "WildBlue Parse Error = " & sErrMsg & vbNewLine & sRet
                    bReset = True
                  End If
                ElseIf sFind.Contains("At this time, your usage is not being counted toward your data allowance.") Then
                  imFree = True
                  sErrMsg = "Your usage is not being metered at this time!"
                  bReset = False
                Else
                  sErrMsg = "Usage Failed: Could not find usage meter."
                  sFailText = "WildBlue Meter Error = " & sErrMsg & vbNewLine & sRet
                  bReset = True
                End If
              ElseIf sRet.Contains("<div class=""error"">") Then
                Dim sMessage As String = sRet.Substring(sRet.IndexOf("<div class=""error"">"))
                If sMessage.Contains("<b>") Then
                  sMessage = sMessage.Substring(sMessage.IndexOf("<b>") + 3)
                  sMessage = sMessage.Substring(0, sMessage.IndexOf("<"))
                  sErrMsg = "Usage Failed: " & sMessage
                  bReset = False
                Else
                  sErrMsg = "Usage Failed: Could not find usage meter."
                  sFailText = "WildBlue Error DIV = " & sErrMsg & vbNewLine & sRet
                  bReset = True
                End If
              Else
                Dim sErr As String = "Could not find usage meter."
                If sRet.Contains("Oops") Then
                  sErr = sRet.Substring(sRet.IndexOf("Oops"))
                  If sErr.Contains("</h3>") Then
                    sErr = sErr.Substring(0, sErr.IndexOf("</h3>"))
                    If sErr = "Oops. We're having a problem displaying your usage information." Then sErr = "Usage data is not available at this time."
                  ElseIf sErr.Contains("<hr>") Then
                    sErr = sErr.Substring(sErr.IndexOf("<hr>") + 4)
                    sErr = sErr.Substring(0, sErr.IndexOf("<hr>"))
                  End If
                  If sErr.Contains("<!-") Then
                    If sErr.Contains("->") Then
                      sErr = sErr.Substring(0, sErr.IndexOf("<!-")) & sErr.Substring(sErr.IndexOf("->") + 2)
                    Else
                      sFailText = "WildBlue OOPS Response Error = " & sErr & vbNewLine & sRet
                    End If
                  End If
                  bReset = False
                Else
                  sFailText = "WildBlue Response Error = " & sErr & vbNewLine & sRet
                  bReset = True
                End If
                sErrMsg = "Usage Failed: " & sErr
              End If
            ElseIf mySettings.AccountType = SatHostTypes.RuralPortal Then
              If Not wsData.ResponseURI.Host = sProvider & ".ruralportal.net" Then
                sErrMsg = "Usage Failed: Domain redirected, check your Internet connection. [" & wsData.ResponseURI.Host & "]"
                bReset = False
              ElseIf sRet.Contains("Current Usage") Then
                If sRet.Contains("Usage data is not available.") Then
                  sErrMsg = "Usage Failed: Usage data is not available at this time."
                  bReset = False
                ElseIf sRet.Contains("<!-- usage bar -->") Then
                  Dim sFind As String = sRet.Substring(sRet.IndexOf("<!-- usage bar -->"))
                  If sFind.Contains("<table") Then
                    sFind = sFind.Substring(sFind.IndexOf("<table"))
                    If sFind.Contains("</table>") And sFind.Contains("<!-- Buy more -->") Then
                      sFind = sFind.Substring(0, sFind.IndexOf("</table>", sFind.IndexOf("<!-- Buy more -->")))
                      ReadUsage(sFind)
                    ElseIf sFind.Contains("</table>") And sFind.Contains("<!-- End up/down stream -->") Then
                      sFind = sFind.Substring(0, sFind.IndexOf("</table>", sFind.IndexOf("<!-- End up/down stream -->")))
                      ReadUsage(sFind)
                    Else
                      sErrMsg = "Usage Failed: Could not parse usage meter table."
                      sFailText = "RuralPortal Parse Error = " & sErrMsg & vbNewLine & sRet
                      bReset = True
                    End If
                  Else
                    sErrMsg = "Usage Failed: Could not find usage meter table."
                    sFailText = "RuralPortal Meter Error = " & sErrMsg & vbNewLine & sRet
                    bReset = True
                  End If
                Else
                  sErrMsg = "Usage Failed: Could not find usage meter."
                  sFailText = "RuralPortal Parse Error = " & sErrMsg & vbNewLine & sRet
                  bReset = True
                End If
              Else
                sErrMsg = "Usage Failed: Failed to log in."
                sFailText = "RuralPortal Usage Error = " & sErrMsg & vbNewLine & sRet
                bReset = True
              End If
            Else
              sErrMsg = "Usage Failed: Host Type not determined (" & mySettings.AccountType.ToString & ")!"
              bReset = True
            End If
          Else
            sErrMsg = "Usage Failed: No page data!"
            bReset = True
          End If
        Else
          If e.Error.InnerException IsNot Nothing Then
            If e.Error.InnerException.Message = "Object reference not set to an instance of an object." Then
              sErrMsg = Nothing
              bReset = False
            ElseIf e.Error.InnerException.Message.CompareTo("The authentication or decryption has failed.") = 0 Then
              ResetTimeout()
              RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.SSLFailureBypass, "Authentication Failed; Please check your system Clock and Certificate Store. Bypassing..."))
              RestartNoCert()
              Exit Sub
            Else
              sErrMsg = "Usage Error: " & LocalErrorToString(e.Error) & " loading " & sAttemptedURL
              bReset = True
            End If
          Else
            sErrMsg = "Usage Error: " & LocalErrorToString(e.Error) & " loading " & sAttemptedURL
            bReset = True
          End If
        End If
      Case "LOGINPAGE"
        If e.Error Is Nothing Then
          Dim sRet As String = e.Result
          If Not String.IsNullOrEmpty(sRet) Then
            If mySettings.AccountType = SatHostTypes.DishNet Then
              If Not wsData.ResponseURI.Host = "identity1.dishnetwork.com" Then
                sErrMsg = "Login Failed: Domain redirected, check your Internet connection. [" & wsData.ResponseURI.Host & "]"
                bReset = False
              ElseIf wsData.ResponseURI.LocalPath.Contains("login.php") Then
                If wsData.ResponseURI.Query.StartsWith("?") Then
                  Dim AuthState As String = wsData.ResponseURI.Query.Substring(wsData.ResponseURI.Query.IndexOf("=") + 1)
                  Dim uriString As String = wsData.ResponseURI.AbsoluteUri
                  wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
                  wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
                  Dim sSend As String = "AuthState=" & AuthState &
                                        "&username=" & myUID &
                                        "&password=" & PercentEncode(myPass) &
                                        "&login_type=username,password" &
                                        "&source=" &
                                        "&remember_me=" &
                                        "&source_button="
                  sAttemptedURL = uriString
                  sAttemptedTag = "LOGIN"
                  wsData.UploadStringAsync(New Uri(uriString), "POST", sSend, "LOGIN")
                  myUID = Nothing
                  myPass = Nothing
                Else
                  sErrMsg = "Login Failed: Logon info not passed!"
                  bReset = True
                End If
              Else
                sErrMsg = "Login Failed: Could not understand response."
                sFailText = "DishNet LoginPage Error = " & sErrMsg & vbNewLine & sRet
                bReset = True
              End If
            Else
              sErrMsg = "Login Failed: Host Type not determined (" & mySettings.AccountType.ToString & ")!"
              bReset = True
            End If
          Else
            sErrMsg = "Login Failed: No page data!"
            bReset = True
          End If
        Else
          If e.Error.InnerException IsNot Nothing Then
            If e.Error.InnerException.Message = "Object reference not set to an instance of an object." Then
              sErrMsg = Nothing
              bReset = False
            ElseIf e.Error.InnerException.Message.CompareTo("The authentication or decryption has failed.") = 0 Then
              ResetTimeout()
              RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.SSLFailureBypass, "Authentication Failed; Please check your system Clock and Certificate Store. Bypassing..."))
              RestartNoCert()
              Exit Sub
            Else
              sErrMsg = "Login Error: " & LocalErrorToString(e.Error) & " loading " & sAttemptedURL
              bReset = True
            End If
          Else
            sErrMsg = "Login Error: " & LocalErrorToString(e.Error) & " loading " & sAttemptedURL
            bReset = True
          End If
        End If
      Case Else
        If e.UserState IsNot Nothing Then
          sErrMsg = "Download Failed. Unknown User State: " & e.UserState.ToString & " loading " & sAttemptedURL
        Else
          sErrMsg = "Download Failed. No User State set loading " & sAttemptedURL
        End If
        bReset = True
    End Select
    If Not String.IsNullOrEmpty(sErrMsg.Trim) Then
      ResetTimeout()
      If bReset Then
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.FatalLoginFailure, sErrMsg, sFailText))
      Else
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, sErrMsg, sFailText))
      End If
    End If
  End Sub
  Private Sub wsData_UploadStringCompleted(sender As Object, e As System.Net.UploadStringCompletedEventArgs) Handles wsData.UploadStringCompleted
    If bCancelled Then
      bCancelled = False
      Exit Sub
    End If
    Dim sErrMsg As String = String.Empty
    Dim sFailText As String = String.Empty
    Dim bReset As Boolean = False
    If e.Error IsNot Nothing Then
      If e.Error.InnerException Is Nothing Then
        sErrMsg = "Login Error: " & LocalErrorToString(e.Error) & " loading " & sAttemptedURL
        bReset = True
      Else
        If e.Error.InnerException.Message = "Object reference not set to an instance of an object." Then
          sErrMsg = Nothing
          bReset = False
        ElseIf e.Error.InnerException.Message.CompareTo("The authentication or decryption has failed.") = 0 Then
          ResetTimeout()
          RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.SSLFailureBypass, "Authentication Failed; Please check your system Clock and Certificate Store. Bypassing..."))
          RestartNoCert()
          Exit Sub
        Else
          sErrMsg = "Login Error: " & LocalErrorToString(e.Error) & " loading " & sAttemptedURL
          bReset = True
        End If
      End If
    ElseIf e.Cancelled Then
      sErrMsg = "Login Error: Request Cancelled"
      bReset = False
    Else
      Dim sPath As String
      Try
        sPath = wsData.ResponseURI.LocalPath.ToLower
      Catch ex As Exception
        sPath = Nothing
      End Try
      Dim sQuery As String
      Try
        sQuery = wsData.ResponseURI.Query.ToLower
      Catch ex As Exception
        sQuery = Nothing
      End Try
      If Not String.IsNullOrEmpty(sPath) Then
        If e.UserState Is Nothing Then
          sErrMsg = "Download Failed. No User State set."
          bReset = False
        ElseIf e.UserState = "LOGIN" Then
          If mySettings.AccountType = SatHostTypes.RuralPortal Then
            If Not wsData.ResponseURI.Host = sProvider & ".ruralportal.net" Then
              sErrMsg = "Login Failed: Domain redirected, check your Internet connection. [" & wsData.ResponseURI.Host & "]"
              bReset = False
            ElseIf sPath.StartsWith("/us/home.do") Then
              LoadUsage("sitemanage")
            ElseIf sPath.StartsWith("/us/login.do") Then
              If String.IsNullOrEmpty(sQuery) Then
                Dim sRet As String = e.Result
                If String.IsNullOrEmpty(sRet) Then
                  sErrMsg = "Login Error: Sent back to login page with empty content."
                  bReset = True
                ElseIf sRet.ToLower.Contains("confirmchange(msg);") Then
                  ResetTimeout()
                  RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Login Issue: Your password needs to be changed."))
                  If Not String.IsNullOrEmpty(sProvider) Then
                    If sProvider.Contains(".") Then sProvider = sProvider.Substring(0, sProvider.LastIndexOf("."))
                    Dim uriString As String = String.Format(sRP, sProvider, "login")
                    Dim cJar As Net.CookieContainer = wsData.CookieJar
                    PrepareLogin()
                    wsData.CookieJar = cJar
                    wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
                    'wsData.Timeout = mySettings.Timeout
                    'wsData.Proxy = mySettings.Proxy
                    wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
                    Dim sSend As String = "warningTrip=true&userName=" & sAccount & "&passwd=" & PercentEncode(sPassword)
                    sAttemptedURL = uriString
                    sAttemptedTag = "LOGIN2"
                    Try
                      wsData.UploadStringAsync(New Uri(uriString), "POST", sSend, "LOGIN2")
                    Catch ex As Exception
                      sErrMsg = "Login Failed: Error Bypassing Password Change - " & ex.Message
                      sFailText = "RuralPortal Login Error = " & sErrMsg & vbNewLine & sRet
                      bReset = False
                    End Try
                  Else
                    sErrMsg = "Login Error: Provider missing. Also, your password is bad. You'll need to change it."
                    bReset = False
                  End If
                Else
                  sErrMsg = "Login Failed: Sent back to login page."
                  bReset = True
                End If
              ElseIf sQuery.Contains("pass=false") Then
                sErrMsg = "Login Failed: Incorrect password."
                bReset = False
              Else
                sErrMsg = "Login Failed (RP): " & sQuery & "."
                bReset = True
              End If
            Else
              sErrMsg = "Unknown Login Result [" & sPath & "?" & sQuery & "]"
              bReset = True
            End If
          ElseIf mySettings.AccountType = SatHostTypes.DishNet Then
            If Not wsData.ResponseURI.Host = "identity1.dishnetwork.com" Then
              sErrMsg = "Login Failed: Domain redirected, check your Internet connection. [" & wsData.ResponseURI.Host & "]"
              bReset = False
            ElseIf sPath.Contains("/login.php") Or sPath.Contains("/finish.php") Then
              Dim sRet As String = e.Result
              If sRet.Contains("SAMLResponse"" value=""") Then
                RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStatusEventArgs.ConnectionStates.Authentication))
                Dim SAMLResponse As String
                SAMLResponse = sRet.Substring(sRet.IndexOf("SAMLResponse"" value=""") + 21)
                SAMLResponse = SAMLResponse.Substring(0, SAMLResponse.IndexOf(""" />"))
                Dim uriString As String = "https://my.dish.com/customercare/saml/post"
                Dim cJar As Net.CookieContainer = wsData.CookieJar
                PrepareLogin()
                wsData.CookieJar = cJar
                wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
                wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
                Dim sSend As String = "SAMLResponse=" & PercentEncode(SAMLResponse)
                sAttemptedURL = uriString
                sAttemptedTag = "LOGIN2"
                wsData.UploadStringAsync(New Uri(uriString), "POST", sSend, "LOGIN2")
              ElseIf sRet.Contains("The system is currently unavailable. Please try again later.") Then
                sErrMsg = "System currently unavailable."
                bReset = False
              Else
                sErrMsg = "No SAML Response"
                bReset = False
                sFailText = "DishNet Login Error = " & sErrMsg & vbNewLine & sRet
              End If
            Else
              sErrMsg = "Unknown Login Result [" & sPath & "?" & sQuery & "]"
              bReset = True
            End If
          ElseIf mySettings.AccountType = SatHostTypes.WildBlue Or mySettings.AccountType = SatHostTypes.Exede Then
            Dim sRet As String = e.Result
            If sRet.Contains("usage.jsp") Then
              LoadUsage("usage")
            ElseIf sRet.Contains("usage_bm.jsp") Then
              LoadUsage("usage_bm")
            ElseIf sRet.Contains("<div class=""error"">") Then
              Dim sMessage As String = sRet.Substring(sRet.IndexOf("<div class=""error"">"))
              If sMessage.Contains("<b>") Then
                sMessage = sMessage.Substring(sMessage.IndexOf("<b>") + 3)
                sMessage = sMessage.Substring(0, sMessage.IndexOf("<"))
                sErrMsg = "Login Failed: " & sMessage
                bReset = False
              Else
                sErrMsg = "Login Failed: Could not understand error."
                sFailText = "WB DIV Error = " & sErrMsg & vbNewLine & sRet
                bReset = True
              End If
            Else
              sErrMsg = "Login Failed: Could not understand response."
              sFailText = "WB Parse Error = " & sErrMsg & vbNewLine & sRet
              bReset = True
            End If
          End If
        ElseIf e.UserState = "LOGIN2" Then
          If mySettings.AccountType = SatHostTypes.RuralPortal Then
            If Not wsData.ResponseURI.Host = sProvider & ".ruralportal.net" Then
              sErrMsg = "Login Failed: Domain redirected, check your Internet connection. [" & wsData.ResponseURI.Host & "]"
              bReset = False
            ElseIf sPath.StartsWith("/us/home.do") Then
              LoadUsage("sitemanage")
            ElseIf sPath.StartsWith("/us/login.do") Then
              If String.IsNullOrEmpty(sQuery) Then
                Dim sRet As String = e.Result
                If String.IsNullOrEmpty(sRet) Then
                  sErrMsg = "Login Error: Sent back to login page with empty content."
                  bReset = True
                ElseIf sRet.ToLower.Contains("confirmchange(msg);") Then
                  sErrMsg = "Login Issue: Your password is bad."
                  If sProvider.Contains(".") Then sProvider = sProvider.Substring(0, sProvider.LastIndexOf("."))
                  Dim uriString As String = String.Format(sRP, sProvider, "login")
                  Try
                    Process.Start(uriString)
                  Catch ex As Exception

                  End Try
                  bReset = False
                Else
                  sErrMsg = "Login Failed: Sent back to login page."
                  sFailText = "RuralPortal Login2 Error = " & sErrMsg & vbNewLine & sRet
                  bReset = True
                End If
              ElseIf sQuery.Contains("pass=false") Then
                sErrMsg = "Login Failed: Incorrect password."
                bReset = False
              Else
                sErrMsg = "Login Failed (RP2): " & sQuery & "."
                bReset = True
              End If
            Else
              sErrMsg = "Unknown Login Result [" & sPath & "?" & sQuery & "]"
              bReset = True
            End If
          ElseIf mySettings.AccountType = SatHostTypes.DishNet Then
            If Not wsData.ResponseURI.Host = "my.dish.com" Then
              sErrMsg = "Login Failed: Domain redirected, check your Internet connection. [" & wsData.ResponseURI.Host & "]"
              bReset = False
            ElseIf wsData.ResponseURI.AbsoluteUri = "https://my.dish.com/customercare/usermanagement/processSynacoreResponse.do?overlayuri=-broadband-prepBroadBand.do" Then
              RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStatusEventArgs.ConnectionStates.TableDownload))
              Dim uriString As String = "https://my.dish.com/customercare/usermanagement/getAccountNumberByUUID.do"
              Dim cJar As Net.CookieContainer = wsData.CookieJar
              PrepareLogin()
              wsData.CookieJar = cJar
              wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
              wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
              Dim sSend As String = "check="
              sAttemptedURL = uriString
              sAttemptedTag = "USAGE"
              wsData.UploadStringAsync(New Uri(uriString), "POST", sSend, "USAGE")
            Else
              sErrMsg = "Unknown Login Result [" & sPath & "?" & sQuery & "]"
              bReset = True
            End If
          Else
            sErrMsg = "Download Failed. Unknown User State: " & e.UserState.ToString & "."
            bReset = True
          End If
        ElseIf e.UserState = "USAGE" Or e.UserState = "USAGE2" Then
          Dim sRet As String = e.Result
          If Not String.IsNullOrEmpty(sRet) Then
            If mySettings.AccountType = SatHostTypes.DishNet Then
              If Not wsData.ResponseURI.Host = "my.dish.com" Then
                sErrMsg = "Login Failed: Domain redirected, check your Internet connection. [" & wsData.ResponseURI.Host & "]"
                bReset = False
              ElseIf wsData.ResponseURI.ToString = "https://my.dish.com/customercare/broadband/prepBroadBand.do" Then
                If sRet.Contains("<div id=""usagestatus""") Then
                  Dim sUsageDiv As String = sRet.Substring(sRet.IndexOf("<div id=""usagestatus"""))
                  If sUsageDiv.Contains("</h2>") Then
                    sUsageDiv = sUsageDiv.Substring(sUsageDiv.IndexOf("</h2>") + 5)
                    If sUsageDiv.Contains("<h2") Then
                      sUsageDiv = sUsageDiv.Substring(0, sUsageDiv.IndexOf("<h2"))
                      If sRet.Contains("Monthly Usage Allowance</h3>") Then
                        Dim sMaxDiv As String = sRet.Substring(sRet.IndexOf("Monthly Usage Allowance</h3>") + 28)
                        If sMaxDiv.Contains("</table>") Then
                          sMaxDiv = sMaxDiv.Substring(0, sMaxDiv.IndexOf("</table>"))
                          sUsageDiv &= vbNewLine & sMaxDiv
                        End If
                      End If
                      ReadUsage(sUsageDiv)
                    Else
                      sErrMsg = "Login Failed: Could not parse usage meter."
                      sFailText = "DishNet Usage Error = " & sErrMsg & vbNewLine & sRet
                      bReset = True
                    End If
                  Else
                    sErrMsg = "Login Failed: Could not parse usage meter."
                    sFailText = "DishNet Usage Error = " & sErrMsg & vbNewLine & sRet
                    bReset = True
                  End If
                Else
                  sErrMsg = "Usage Failed: Could not find usage meter."
                  sFailText = "DishNet Usage Error = " & sErrMsg & vbNewLine & sRet
                  bReset = True
                End If
              ElseIf e.UserState = "USAGE" Then
                Dim uriString As String = "https://my.dish.com/customercare/broadband/prepBroadBand.do"
                Dim cJar As Net.CookieContainer = wsData.CookieJar
                PrepareLogin()
                wsData.CookieJar = cJar
                wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
                wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
                Dim sSend As String = "srt=second_try"
                sAttemptedURL = uriString
                sAttemptedTag = "USAGE"
                wsData.UploadStringAsync(New Uri(uriString), "POST", sSend, "USAGE2")
              Else
                sErrMsg = "Usage Failed: Could not load usage meter page. Redirected to " & wsData.ResponseURI.ToString()
                sFailText = "DishNet Usage Error = " & sErrMsg & vbNewLine & sRet
                bReset = True
              End If
            Else
              sErrMsg = "Usage Failed: Host Type not determined (" & mySettings.AccountType.ToString & ")!"
              sFailText = "Unknown Host Usage Error = " & sErrMsg & vbNewLine & sRet
              bReset = True
            End If
          Else
            sErrMsg = "Usage Failed: No page data!"
            bReset = True
          End If
        End If
      Else
        sErrMsg = "Login Failed: No response URI!"
        bReset = True
      End If
    End If
    If Not String.IsNullOrEmpty(sErrMsg.Trim) Then
      ResetTimeout()
      If bReset Then
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.FatalLoginFailure, sErrMsg, sFailText))
      Else
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, sErrMsg, sFailText))
      End If
    End If
  End Sub
  Private Sub LoadUsage(File As String)
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStatusEventArgs.ConnectionStates.TableDownload))
    Dim cJar As Net.CookieContainer = wsData.CookieJar
    PrepareLogin()
    wsData.CookieJar = cJar
    If mySettings.AccountType = SatHostTypes.WildBlue Or mySettings.AccountType = SatHostTypes.Exede Then
      Dim uriString As String = String.Format(sWB, sProvider, File, IIf(sProvider.ToLower = "exede.net", "exede.com", sProvider))
      sAttemptedURL = uriString
      sAttemptedTag = "USAGE"
      wsData.DownloadStringAsync(New Uri(uriString), "USAGE")
    ElseIf mySettings.AccountType = SatHostTypes.RuralPortal Then
      If sProvider.Contains(".") Then sProvider = sProvider.Substring(0, sProvider.LastIndexOf("."))
      Dim uriString As String = String.Format(sRP, sProvider, File)
      sAttemptedURL = uriString
      sAttemptedTag = "USAGE"
      wsData.DownloadStringAsync(New Uri(uriString), "USAGE")
    End If
  End Sub
  Private Sub ReadUsage(Table As String)
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStatusEventArgs.ConnectionStates.TableRead))
    If mySettings.AccountType = SatHostTypes.WildBlue Or mySettings.AccountType = SatHostTypes.Exede Then
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
        ResetTimeout()
        If String.IsNullOrEmpty(sDownT) Or String.IsNullOrEmpty(sUpT) Then
          RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Usage Read Failed.", Table))
        Else
          RaiseEvent ConnectionWBResult(Me, New ConnectionWBResultEventArgs(StrToVal(sDown), StrToVal(sDownT), StrToVal(sUp), StrToVal(sUpT), Now))
        End If
      ElseIf Table.Contains("allowance") Then
        Dim sPlusT As String = String.Empty
        For I As Integer = 0 To sRows.Length - 1
          If Not String.IsNullOrEmpty(sRows(I)) Then
            If sRows(I).Contains("<strong>") Then
              If String.IsNullOrEmpty(sDownT) Then
                sDownT = sRows(I).Substring(sRows(I).IndexOf("<strong>") + 8)
                sDownT = sDownT.Substring(0, sDownT.IndexOf("</strong>"))
              ElseIf sRows(I).Contains("Total usage:") And sRows(I).Contains("</span>") And String.IsNullOrEmpty(sUpT) Then
                If sRows(I).Contains("<b>") And sRows(I).Contains("</b>") Then
                  sUpT = sRows(I).Substring(sRows(I).IndexOf("<b>") + 3)
                  sUpT = sUpT.Substring(0, sUpT.IndexOf("</b>"))
                End If
              ElseIf sRows(I - 1).ToLower.Contains("buy more purchased") And String.IsNullOrEmpty(sPlusT) Then
                If sRows(I).ToLower.Contains("<strong>") And sRows(I).ToLower.Contains("</strong>") Then
                  sPlusT = sRows(I).Substring(sRows(I).IndexOf("<strong>") + 8)
                  sPlusT = sPlusT.Substring(0, sPlusT.IndexOf("</strong>"))
                End If
              End If
            ElseIf sRows(I).Contains("<b>") And sRows(I).Contains("</b>") Then
              If sRows(I).Contains("loaded:") Then
                If String.IsNullOrEmpty(sDown) Then
                  sDown = sRows(I).Substring(sRows(I).IndexOf("<b>") + 3)
                  sDown = sDown.Substring(0, sDown.IndexOf("</b>"))
                ElseIf String.IsNullOrEmpty(sUp) Then
                  sUp = sRows(I).Substring(sRows(I).IndexOf("<b>") + 3)
                  sUp = sUp.Substring(0, sUp.IndexOf("</b>"))
                End If
              End If
            End If
          End If
        Next
        ResetTimeout()
        If String.IsNullOrEmpty(sDownT) Then
          RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Usage Read Failed.", Table))
        Else
          RaiseEvent ConnectionEResult(Me, New ConnectionEResultEventArgs(StrToVal(sDown, MBPerGB), StrToVal(sUp, MBPerGB), StrToVal(sUpT, MBPerGB), StrToVal(sDownT, MBPerGB), StrToVal(sPlusT, MBPerGB), Now))
        End If
      End If
    ElseIf mySettings.AccountType = SatHostTypes.RuralPortal Then
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
        ResetTimeout()
        If String.IsNullOrEmpty(sDownT) Or String.IsNullOrEmpty(sUpT) Then
          RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Usage Read Failed.", Table))
        Else
          RaiseEvent ConnectionRP4Result(Me, New ConnectionRP4ResultEventArgs(StrToVal(sDown), StrToVal(sDownT), StrToVal(sUp), StrToVal(sUpT), Now))
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
        ResetTimeout()
        If String.IsNullOrEmpty(sDownT) Then
          RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Usage Read Failed.", Table))
        Else
          RaiseEvent ConnectionRP2Result(Me, New ConnectionRP2ResultEventArgs(StrToVal(sDown, MBPerGB) + StrToVal(sOverhead, MBPerGB), StrToVal(sDownT, MBPerGB), Now))
        End If
      End If
    ElseIf mySettings.AccountType = SatHostTypes.DishNet Then
      If Table.Contains("alertError") Then
        ResetTimeout()
        If Table.Contains("This information is currently unavailable.") Then
          RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Usage information currently unavailable."))
        Else
          RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Usage Read Failed.", Table))
        End If
      Else
        Dim sAnytime As String = Nothing
        Dim sOffPeak As String = Nothing
        Dim sAdditional As String = Nothing
        If Table.Contains("Anytime</h3>") Then
          sAnytime = Table.Substring(Table.IndexOf("Anytime</h3>") + 12)
          If sAnytime.Contains("Remaining") Then
            sAnytime = sAnytime.Substring(0, sAnytime.IndexOf("Remaining"))
            If sAnytime.Contains("padding230") Then
              sAnytime = sAnytime.Substring(sAnytime.IndexOf("padding230"))
              If sAnytime.Contains(">") Then
                sAnytime = sAnytime.Substring(sAnytime.IndexOf(">") + 1)
                CleanupResult(sAnytime)
              Else
                sAnytime = Nothing
              End If
            Else
              sAnytime = Nothing
            End If
          Else
            sAnytime = Nothing
          End If
        End If
        If Table.Contains("Off-Peak (2am - 8am)</h3>") Then
          sOffPeak = Table.Substring(Table.IndexOf("am)</h3>") + 8)
          If sOffPeak.Contains("Remaining") Then
            sOffPeak = sOffPeak.Substring(0, sOffPeak.IndexOf("Remaining"))
            If sOffPeak.Contains("padding230") Then
              sOffPeak = sOffPeak.Substring(sOffPeak.IndexOf("padding230"))
              If sOffPeak.Contains(">") Then
                sOffPeak = sOffPeak.Substring(sOffPeak.IndexOf(">") + 1)
                CleanupResult(sOffPeak)
              Else
                sOffPeak = Nothing
              End If
            Else
              sOffPeak = Nothing
            End If
          Else
            sOffPeak = Nothing
          End If
        End If
        If Table.Contains("Additional Capacity</h3>") Then
          sAdditional = Table.Substring(Table.IndexOf("Additional Capacity</h3>") + 24)
          If sAdditional.Contains("Remaining") Then
            sAdditional = sAdditional.Substring(0, sAdditional.IndexOf("Remaining"))
            If sAdditional.Contains("padding230") Then
              sAdditional = sAdditional.Substring(sAdditional.IndexOf("padding230"))
              If sAdditional.Contains(">") Then
                sAdditional = sAdditional.Substring(sAdditional.IndexOf(">") + 1)
                CleanupResult(sAdditional)
              Else
                sAdditional = Nothing
              End If
            Else
              sAdditional = Nothing
            End If
          Else
            sAdditional = Nothing
          End If
        End If
        Dim sAnyMax As String = Nothing
        Dim sOffMax As String = Nothing
        If Table.Contains("Anytime</label>") Then
          sAnyMax = Table.Substring(Table.IndexOf("Anytime</label>"))
          If sAnyMax.Contains("<td style=""text-align: right; width: 60px;"">") Then
            sAnyMax = sAnyMax.Substring(sAnyMax.IndexOf("<td style=""text-align: right; width: 60px;"">") + 44)
            If sAnyMax.Contains("</td>") Then
              sAnyMax = sAnyMax.Substring(0, sAnyMax.IndexOf("</td>"))
              CleanupResult(sAnyMax)
            Else
              sAnytime = Nothing
              sAnyMax = Nothing
            End If
          Else
            sAnytime = Nothing
            sAnyMax = Nothing
          End If
        End If
        If Table.Contains("8am)</label>") Then
          sOffMax = Table.Substring(Table.IndexOf("8am)</label>"))
          If sOffMax.Contains("<td style=""text-align: right; width: 60px;"">") Then
            sOffMax = sOffMax.Substring(sOffMax.IndexOf("<td style=""text-align: right; width: 60px;"">") + 44)
            If sOffMax.Contains("</td>") Then
              sOffMax = sOffMax.Substring(0, sOffMax.IndexOf("</td>"))
              CleanupResult(sOffMax)
            Else
              sOffPeak = Nothing
              sOffMax = Nothing
            End If
          Else
            sOffPeak = Nothing
            sOffMax = Nothing
          End If
        End If
        If String.IsNullOrEmpty(sAnytime) Then
          ResetTimeout()
          RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Usage Read Failed.", Table))
        Else
          Dim lDown, lDownT, lUp, lUpT As Long
          If Not String.IsNullOrEmpty(sAnytime) Then
            lDown = StrToVal(sAnytime, MBPerGB)
          End If
          If Not String.IsNullOrEmpty(sAnyMax) Then
            lDownT = StrToVal(sAnyMax, MBPerGB)
            lDown = lDownT - lDown
          End If
          If Not String.IsNullOrEmpty(sOffPeak) Then
            lUp = StrToVal(sOffPeak, MBPerGB)
          End If
          If Not String.IsNullOrEmpty(sOffMax) Then
            lUpT = StrToVal(sOffMax, MBPerGB)
            lUp = lUpT - lUp
          End If
          If Not String.IsNullOrEmpty(sAdditional) Then
            lDownT += StrToVal(sAdditional, MBPerGB)
          End If
          If lDownT > 0 Then
            RaiseEvent ConnectionDNResult(Me, New ConnectionDNResultEventArgs(lDown, lDownT, lUp, lUpT, Now))
          End If
        End If
      End If
    End If
    wsData.CookieJar = New Net.CookieContainer
    SendSocketErrors()
  End Sub
#End Region
#Region "Useful Functions"
  Private Function StrToVal(str As String, Optional vMult As Integer = 1) As Long
    If String.IsNullOrEmpty(str) Then Return 0
    If Not str.Contains(" ") Then Return CLng(Val(str.Replace(",", "")) * vMult)
    Return CLng(Val(str.Substring(0, str.IndexOf(" ")).Replace(",", "")) * vMult)
  End Function
  Private Sub CleanupResult(ByRef result As String)
    result = Replace(result, "&nbsp;", " ")
    result = Replace(result, vbTab, "")
    result = Replace(result, vbCr, "")
    result = Replace(result, vbLf, "")
    result = Trim(result)
  End Sub
  Private Function LocalErrorToString(ex As System.Exception)
    Dim reportHandler As New ReportSocketErrorInvoker(AddressOf ReportSocketError)
    If ex.InnerException Is Nothing Then
      If ex.Message.StartsWith("The remote name could not be resolved:") Then
        Return "Could not connect to your DNS. Check your internet connection."
      Else
        reportHandler.BeginInvoke(ex, Nothing, Nothing)
        Return ex.Message
      End If
    Else
      If ex.Message.StartsWith("Unable to connect to the remote server") Then
        If ex.InnerException.Message.StartsWith("A connection attempt failed because the connected party did not respond properly after a period of time, or established connection failed because connected host has failed to respond") Then
          Return "The server did not respond. Check your internet connection."
        ElseIf ex.InnerException.Message.StartsWith("A socket operation was attempted to an unreachable host") Then
          Return "The host is unreachable. Check your local network."
        ElseIf ex.InnerException.Message.StartsWith("A socket operation was attempted to an unreachable network") Then
          Return "The network is unreachable. Check your internet connection."
        Else
          reportHandler.BeginInvoke(ex, Nothing, Nothing)
          Return "Can't connect to the server - " & ex.InnerException.Message
        End If
      Else
        reportHandler.BeginInvoke(ex, Nothing, Nothing)
        Return ex.Message & " - " & ex.InnerException.Message
      End If
    End If
  End Function
  Public Function IgnoreCert(sender As Object, certificate As System.Security.Cryptography.X509Certificates.X509Certificate, chain As System.Security.Cryptography.X509Certificates.X509Chain, errors As Net.Security.SslPolicyErrors) As Boolean
    Return True
  End Function
#End Region
#Region "IDisposable Support"
  Private disposedValue As Boolean
  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not Me.disposedValue Then
      If disposing Then
        ResetTimeout()
        If wsData IsNot Nothing Then
          wsData.Dispose()
          wsData = Nothing
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