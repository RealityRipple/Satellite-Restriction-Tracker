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
    Private m_subval As Decimal
    Public Sub New(status As ConnectionStates, Optional substate As ConnectionSubStates = ConnectionSubStates.None, Optional percent As Decimal = 0)
      m_state = status
      m_substate = substate
      m_subval = percent
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
    Public ReadOnly Property SubPercentage As Decimal
      Get
        Return m_subval
      End Get
    End Property
  End Class
  Public Event ConnectionStatus(sender As Object, e As ConnectionStatusEventArgs)
#End Region
  Private WithEvents wsData As CookieAwareWebClient
  Private WithEvents acType As DetermineType
  Private tmrReadTimeout As Threading.Timer
  Private mySettings As AppSettings
  Private Const MBPerGB As Integer = 1000
  Private Const sWB As String = "https://myaccount.{0}/wbisp/{2}/{1}.jsp"
  Private Const sRP As String = "https://{0}.ruralportal.net/us/{1}.do"
  Private sAccount, sPassword, sProvider As String
  Private sAttemptedURL As String
  Private AttemptedTag As ConnectionStates
  Private AttemptedSub As ConnectionSubStates
  Private AttemptedVal As Decimal
  Private bCancelled, bErrored As Boolean
  Private imSlowed As Boolean
  Private imFree As Boolean
  Private FullCheck As Boolean = True
  Private myUID, myPass As String
  Private ClosingTime As Boolean
  Private sDataPath As String
#Region "Initialization Functions"
  Public Sub New(ConfigPath As String)
    sDataPath = ConfigPath
    If mySettings Is Nothing Then mySettings = New AppSettings(ConfigPath & IO.Path.DirectorySeparatorChar.ToString & "user.config")
    Net.ServicePointManager.SecurityProtocol = mySettings.SecurityProtocol
    InitAccount()
  End Sub
  Public Sub Connect()
    ResetTimeout()
    If mySettings.AccountType = SatHostTypes.Other Then
      If mySettings.Account.Contains("@") Then
        acType = New DetermineType(mySettings.Account.Substring(mySettings.Account.IndexOf("@") + 1), mySettings.Timeout, mySettings.Proxy)
      Else
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.UnknownAccountType))
      End If
    Else
      GetUsage()
    End If
  End Sub
  Private Sub acType_TypeDetermined(UserState As Object, e As DetermineType.TypeDeterminedEventArgs) Handles acType.TypeDetermined
    Select Case e.HostGroup
      Case DetermineType.TypeDeterminedEventArgs.SatHostGroup.WildBlue
        mySettings.AccountType = SatHostTypes.WildBlue_LEGACY
        GetUsage()
      Case DetermineType.TypeDeterminedEventArgs.SatHostGroup.DishNet
        mySettings.AccountType = SatHostTypes.DishNet_EXEDE
        GetUsage()
      Case DetermineType.TypeDeterminedEventArgs.SatHostGroup.RuralPortal
        mySettings.AccountType = SatHostTypes.RuralPortal_EXEDE
        GetUsage()
      Case DetermineType.TypeDeterminedEventArgs.SatHostGroup.Exede
        mySettings.AccountType = SatHostTypes.WildBlue_EXEDE
        GetUsage()
      Case DetermineType.TypeDeterminedEventArgs.SatHostGroup.Other
        ResetTimeout()
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.UnknownAccountType))
    End Select
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
      RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Initialize))
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
    iHist = 0
    If wsData IsNot Nothing Then
      If wsData.IsBusy Then wsData.CancelAsync()
      wsData.Dispose()
      wsData = Nothing
    End If
    wsData = New CookieAwareWebClient
    If mySettings IsNot Nothing Then
      wsData.Timeout = mySettings.Timeout
      wsData.Proxy = mySettings.Proxy
    End If
  End Sub
  Private Sub Login(sUID As String, sPass As String)
    PrepareLogin()
    ContinueLogin(sUID, sPass)
  End Sub
  Private Sub ContinueLogin(sUID As String, sPass As String)
    PrepareLogin()
    Select Case mySettings.AccountType
      Case SatHostTypes.WildBlue_LEGACY : ContinueLoginWB(sUID, sPass)
      Case SatHostTypes.WildBlue_EXEDE : ContinueLoginExede(sUID, sPass)
      Case SatHostTypes.RuralPortal_LEGACY, SatHostTypes.RuralPortal_EXEDE : ContinueLoginRP(sUID, sPass)
      Case SatHostTypes.DishNet_EXEDE : ContinueLoginDN(sUID, sPass)
    End Select
  End Sub
  Private Sub ContinueLoginWB(sUID As String, sPass As String)
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Prepare))
    Dim uriString As String = String.Format(sWB, sProvider, "servLogin", IIf(sProvider.ToLower = "exede.net", "exede.com", sProvider))
    wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
    wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
    Dim aState As Object = BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, uriString)
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
        wsData.UploadStringAsync(uriURL, "POST", sSend, aState)
      Catch ex As Exception
        ResetTimeout()
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Login Send Failure: " & NetworkErrorToString(ex, sDataPath)))
      End Try
    End If
  End Sub
  Private Sub ContinueLoginExede(sUID As String, sPass As String)
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Prepare))
    Dim uriString As String = "https://myexede.force.com/login?startURL=%2Fdashboard"
    Dim aState As Object = BeginAttempt(ConnectionStates.Login, ConnectionSubStates.ReadLogin, 0, uriString)
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
      wsData.ManualRedirect = True
      Try
        wsData.DownloadStringAsync(uriURL, aState)
      Catch ex As Exception
        ResetTimeout()
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Login Send Failure: " & NetworkErrorToString(ex, sDataPath)))
      End Try
    End If
  End Sub
  Private Sub ContinueLoginRP(sUID As String, sPass As String)
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Prepare))
    If sProvider.Contains(".") Then sProvider = sProvider.Substring(0, sProvider.LastIndexOf("."))
    Dim uriString As String = String.Format(sRP, sProvider, "login")
    wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
    wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
    Dim sSend As String = "warningTrip=false&userName=" & PercentEncode(sUID) & "&passwd=" & PercentEncode(sPass)
    Dim aState As Object = BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, uriString)
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
        wsData.UploadStringAsync(uriURL, "POST", sSend, aState)
      Catch ex As Exception
        ResetTimeout()
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Login Send Failure: " & NetworkErrorToString(ex, sDataPath)))
      End Try
    End If
  End Sub
  Private Sub ContinueLoginDN(sUID As String, sPass As String)
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Prepare))
    Dim uriString As String = "https://my.dish.com/customercare/saml/login?target=%2Fcustomercare%2Fusermanagement%2FprocessSynacoreResponse.do%3Foverlayuri%3D-broadband-prepBroadBand.do&message=&forceAuthn=true"
    Dim aState As Object = BeginAttempt(ConnectionStates.Login, ConnectionSubStates.ReadLogin, 0, uriString)
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
        wsData.DownloadStringAsync(uriURL, aState)
      Catch ex As Exception
        ResetTimeout()
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Login Send Failure: " & NetworkErrorToString(ex, sDataPath)))
      End Try
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
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Error: " & NetworkErrorToString(e.Error, sDataPath)))
      End If
    Else
      ResetTimeout()
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Error: " & NetworkErrorToString(e.Error, sDataPath)))
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
    ResetTimeout()
    GetUsage()
  End Sub
#End Region
#Region "Parsing Functions"
  Private iHist As Integer = 0
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
    If e.Error IsNot Nothing Then
      If e.Error.InnerException Is Nothing Then
        sErrMsg = "Login Error: " & NetworkErrorToString(e.Error, sDataPath) & " loading " & sAttemptedURL
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
          sErrMsg = "Login Error: " & NetworkErrorToString(e.Error, sDataPath) & " loading " & sAttemptedURL
          bReset = True
        End If
      End If
    ElseIf e.Cancelled Then
      sErrMsg = "Login Error: Request Cancelled"
      bReset = False
    Else
      Dim sHost As String
      Try
        sHost = wsData.ResponseURI.Host.ToLower
      Catch ex As Exception
        sHost = Nothing
      End Try
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
      Dim sRet As String = e.Result
      If String.IsNullOrEmpty(sPath) Then
        sErrMsg = "Login Failed: No Response URI!"
        bReset = True
      ElseIf String.IsNullOrEmpty(sRet) Then
        For Each Key In wsData.ResponseHeaders.AllKeys
          If Key.ToLower = "location" Then
            Dim sNewPath As String = wsData.ResponseHeaders.Item(Key)
            wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
            ResetTimeout(True)
            wsData.DownloadStringAsync(New Uri(sNewPath), e.UserState)
            Exit Sub
          End If
        Next
        sErrMsg = "Login Error: Empty Content!"
        bReset = True
      Else
        HandleResponse(e.UserState(0), e.UserState(1), e.UserState(2), mySettings.AccountType, wsData.ResponseURI.AbsoluteUri, sHost, sPath, sQuery, sRet, sErrMsg, sFailText, bReset)
      End If
    End If
    If Not String.IsNullOrEmpty(Trim(sErrMsg)) Then
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
        sErrMsg = "Login Error: " & NetworkErrorToString(e.Error, sDataPath) & " loading " & sAttemptedURL
        bReset = True
      Else
        If e.Error.InnerException.Message = "Object reference not set to an instance of an object." Then
          sErrMsg = Nothing
          bReset = False
        ElseIf e.Error.InnerException.Message.CompareTo("The authentication or decryption has failed.") = 0 Then
          ResetTimeout(True)
          RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.SSLFailureBypass, "Authentication Failed; Please check your system Clock and Certificate Store. Bypassing..."))
          RestartNoCert()
          Exit Sub
        Else
          sErrMsg = "Login Error: " & NetworkErrorToString(e.Error, sDataPath) & " loading " & sAttemptedURL
          bReset = True
        End If
      End If
    ElseIf e.Cancelled Then
      sErrMsg = "Login Error: Request Cancelled"
      bReset = False
    Else
      Dim sHost As String
      Try
        sHost = wsData.ResponseURI.Host.ToLower
      Catch ex As Exception
        sHost = Nothing
      End Try
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
      Dim sRet As String = e.Result
      If String.IsNullOrEmpty(sPath) Then
        sErrMsg = "Login Failed: No Response URI!"
        bReset = True
      ElseIf String.IsNullOrEmpty(sRet) Then
        For Each Key In wsData.ResponseHeaders.AllKeys
          If Key.ToLower = "location" Then
            Dim sNewPath As String = wsData.ResponseHeaders.Item(Key)
            wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
            ResetTimeout(True)
            wsData.DownloadStringAsync(New Uri(sNewPath), e.UserState)
            Exit Sub
          End If
        Next
        sErrMsg = "Login Error: Empty Content!"
        bReset = True
      Else
        HandleResponse(e.UserState(0), e.UserState(1), e.UserState(2), mySettings.AccountType, wsData.ResponseURI.AbsoluteUri, sHost, sPath, sQuery, sRet, sErrMsg, sFailText, bReset)
      End If
    End If
    If Not String.IsNullOrEmpty(Trim(sErrMsg)) Then
      ResetTimeout()
      If bReset Then
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.FatalLoginFailure, sErrMsg, sFailText))
      Else
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, sErrMsg, sFailText))
      End If
    End If
  End Sub
  Private Sub HandleResponse(LoginState As ConnectionStates, LoginSubState As ConnectionSubStates, LoginSubPercent As Decimal, AccountType As SatHostTypes, sURI As String, sHost As String, sPath As String, sQuery As String, sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean)
    iHist += 1
    ResetTimeout(True)
    Dim CloseSocket As Boolean = False
    Select Case AccountType
      Case SatHostTypes.WildBlue_LEGACY
        Select Case LoginState
          Case ConnectionStates.Login
            WB_Login_Authenticate(sRet, sErrMsg, sFailText, bReset)
          Case ConnectionStates.TableDownload
            WB_Download_Table(sHost, sRet, sErrMsg, sFailText, bReset)
            CloseSocket = True
          Case Else
            sErrMsg = "Login Failed. Unknown Login State: " & LoginState.ToString & " (" & AttemptedTag.ToString & " [" & AttemptedSub & "(" & AttemptedVal & ")]) loading " & sAttemptedURL
            bReset = True
        End Select
      Case SatHostTypes.WildBlue_EXEDE
        Select Case LoginState
          Case ConnectionStates.Login
            Select Case LoginSubState
              Case ConnectionSubStates.ReadLogin
                EX_Login_ReadLogin(sHost, sPath, sRet, sErrMsg, sFailText, bReset)
              Case ConnectionSubStates.Authenticate, ConnectionSubStates.AuthenticateRetry
                EX_Login_Authenticate(sURI, sHost, sPath, sRet, sErrMsg, sFailText, bReset)
            End Select
          Case ConnectionStates.TableDownload
            Select Case LoginSubState
              Case ConnectionSubStates.LoadHome
                EX_Download_Homepage(sHost, sPath, sRet, sErrMsg, sFailText, bReset)
              Case ConnectionSubStates.LoadAJAX
                Select Case LoginSubPercent
                  Case (1 / 5) : EX_Download_Ajax(sHost, sPath, sRet, sErrMsg, sFailText, bReset, "2")
                  Case (2 / 5) : EX_Download_Ajax(sHost, sPath, sRet, sErrMsg, sFailText, bReset, "5")
                  Case (3 / 5) : EX_Download_Ajax(sHost, sPath, sRet, sErrMsg, sFailText, bReset, "6")
                  Case (4 / 5) : EX_Download_Ajax(sHost, sPath, sRet, sErrMsg, sFailText, bReset, "4")
                  Case (5 / 5) : EX_Download_Ajax(sHost, sPath, sRet, sErrMsg, sFailText, bReset, "3")
                End Select
              Case ConnectionSubStates.LoadTable
                EX_Download_Table(sHost, sPath, sRet, sErrMsg, sFailText, bReset)
                CloseSocket = True
              Case Else
                sErrMsg = "Login Failed. Unknown Login SubState: " & LoginSubState.ToString & " (" & AttemptedTag.ToString & " [" & AttemptedSub & "(" & AttemptedVal & ")]) loading " & sAttemptedURL
                bReset = True
            End Select
          Case Else
            sErrMsg = "Login Failed. Unknown Login State: " & LoginState.ToString & ">" & LoginSubState.ToString & " (" & AttemptedTag.ToString & " [" & AttemptedSub & "(" & AttemptedVal & ")]) loading " & sAttemptedURL
            bReset = True
        End Select
      Case SatHostTypes.RuralPortal_LEGACY, SatHostTypes.RuralPortal_EXEDE
        Select Case LoginState
          Case ConnectionStates.Login
            Select Case LoginSubState
              Case ConnectionSubStates.Authenticate
                RP_Login_Authenticate(sHost, sPath, sQuery, sRet, sErrMsg, sFailText, bReset)
              Case ConnectionSubStates.AuthenticateRetry
                RP_Login_AuthenticateRetry(sHost, sPath, sQuery, sRet, sErrMsg, sFailText, bReset)
              Case Else
                sErrMsg = "Login Failed. Unknown Login SubState: " & LoginSubState.ToString & " (" & AttemptedTag.ToString & " [" & AttemptedSub & "(" & AttemptedVal & ")]) loading " & sAttemptedURL
                bReset = True
            End Select
          Case ConnectionStates.TableDownload
            RP_Download_Table(sHost, sPath, sQuery, sRet, sErrMsg, sFailText, bReset)
            CloseSocket = True
          Case Else
            sErrMsg = "Login Failed. Unknown Login State: " & LoginState.ToString & ">" & LoginSubState.ToString & " (" & AttemptedTag.ToString & " [" & AttemptedSub & "(" & AttemptedVal & ")]) loading " & sAttemptedURL
            bReset = True
        End Select
      Case SatHostTypes.DishNet_EXEDE
        Select Case LoginState
          Case ConnectionStates.Login
            Select Case LoginSubState
              Case ConnectionSubStates.ReadLogin
                DN_Login_ReadLogin(sURI, sHost, sPath, sQuery, sRet, sErrMsg, sFailText, bReset)
              Case ConnectionSubStates.AuthPrepare
                DN_Login_Prepare(sURI, sHost, sPath, sQuery, sRet, sErrMsg, sFailText, bReset)
              Case ConnectionSubStates.Authenticate, ConnectionSubStates.AuthenticateRetry
                DN_Login_Authenticate(sURI, sHost, sPath, sQuery, sRet, sErrMsg, sFailText, bReset)
              Case ConnectionSubStates.Verify
                DN_Login_Verify(sURI, sHost, sPath, sQuery, sRet, sErrMsg, sFailText, bReset)
              Case Else
                sErrMsg = "Login Failed. Unknown Login SubState: " & LoginSubState.ToString & " (" & AttemptedTag.ToString & " [" & AttemptedSub & "(" & AttemptedVal & ")]) loading " & sAttemptedURL
                bReset = True
            End Select
          Case ConnectionStates.TableDownload
            Select Case LoginSubState
              Case ConnectionSubStates.LoadHome
                DN_Download_Home(sHost, sPath, sQuery, sRet, sErrMsg, sFailText, bReset)
              Case ConnectionSubStates.LoadTable
                DN_Download_Table(sHost, sPath, sQuery, sRet, sErrMsg, sFailText, bReset)
              Case ConnectionSubStates.LoadTableRetry
                DN_Download_TableRetry(sHost, sPath, sQuery, sRet, sErrMsg, sFailText, bReset)
                CloseSocket = True
              Case Else
                sErrMsg = "Login Failed. Unknown Login SubState: " & LoginSubState.ToString & " (" & AttemptedTag.ToString & " [" & AttemptedSub & "(" & AttemptedVal & ")]) loading " & sAttemptedURL
                bReset = True
            End Select
          Case Else
            sErrMsg = "Login Failed. Unknown Login State: " & LoginState.ToString & ">" & LoginSubState.ToString & " (" & AttemptedTag.ToString & " [" & AttemptedSub & "(" & AttemptedVal & ")]) loading " & sAttemptedURL
            bReset = True
        End Select
      Case Else
        sErrMsg = "Login Failed: Host Type not determined (" & AccountType.ToString & ")!"
        bReset = True
    End Select
    If CloseSocket Then
      If wsData IsNot Nothing Then
        wsData.CancelAsync()
        wsData.Dispose()
        wsData = Nothing
      End If
    End If
  End Sub
  Private Sub LoadUsage(File As String)
    Dim cJar As Net.CookieContainer = wsData.CookieJar
    PrepareLogin()
    wsData.CookieJar = cJar
    Select Case mySettings.AccountType
      Case SatHostTypes.WildBlue_LEGACY
        Dim uriString As String = String.Format(sWB, sProvider, File, IIf(sProvider.ToLower = "exede.net", "exede.com", sProvider))
        Dim aState As Object = BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTable, 0, uriString)
        wsData.DownloadStringAsync(New Uri(uriString), aState)
      Case SatHostTypes.RuralPortal_LEGACY, SatHostTypes.RuralPortal_EXEDE
        If sProvider.Contains(".") Then sProvider = sProvider.Substring(0, sProvider.LastIndexOf("."))
        Dim uriString As String = String.Format(sRP, sProvider, File)
        Dim aState As Object = BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTable, 0, uriString)
        wsData.DownloadStringAsync(New Uri(uriString), aState)
      Case Else
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.FatalLoginFailure, "Usage Failed: Host Type not determined (" & mySettings.AccountType.ToString & ")!"))
    End Select
  End Sub
  Private Sub ReadUsage(Table As String)
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.TableRead))
    Select Case mySettings.AccountType
      Case SatHostTypes.WildBlue_LEGACY : WB_Read_Table(Table)
      Case SatHostTypes.WildBlue_EXEDE : EX_Read_Table(Table)
      Case SatHostTypes.RuralPortal_LEGACY, SatHostTypes.RuralPortal_EXEDE : RP_Read_Table(Table)
      Case SatHostTypes.DishNet_EXEDE : DN_Read_Table(Table)
    End Select
    wsData.CookieJar = New Net.CookieContainer
    SendSocketErrors(sDataPath)
  End Sub
#Region "WB"
  Private Sub WB_Login_Authenticate(sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean)
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
      ElseIf sMessage.Contains("my.exede.net") Then
        sErrMsg = "Login Failed: You must create an account at the new Exede Portal."
        bReset = True
      Else
        sErrMsg = "Login Failed: Could not understand error."
        sFailText = "WB DIV Error = " & sErrMsg & vbNewLine & sRet
        bReset = True
      End If
    ElseIf sRet.Contains("https://my.exede.net/usage") Then
      sErrMsg = "Login Redirect: Exede account detected."
      mySettings.AccountType = SatHostTypes.WildBlue_EXEDE
      ResetTimeout()
      GetUsage()
    Else
      sErrMsg = "Login Failed: Could not understand response."
      sFailText = "WB Parse Error = " & sErrMsg & vbNewLine & sRet
      bReset = True
    End If
  End Sub
  Private Sub WB_Download_Table(sHost As String, sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean)
    If Not sHost = "myaccount." & sProvider Then
      sErrMsg = "Usage Failed: Domain redirected, check your Internet connection. [" & sHost & "]"
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
      ElseIf sFind.Contains("FREEDOM") AndAlso sFind.Contains("Current Usage<strong>:") Then
        sFind = sFind.Substring(sFind.IndexOf("FREEDOM"))
        If sFind.Contains("</td>") Then
          sFind = sFind.Substring(0, sFind.IndexOf("</td>"))
          ReadUsage(sFind)
        Else
          sErrMsg = "Usage Failed: Could not parse usage meter."
          sFailText = "Exede Freedom Parse Error = " & sErrMsg & vbNewLine & sRet
          bReset = False
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
      ResetTimeout()
      If String.IsNullOrEmpty(sDownT) Or String.IsNullOrEmpty(sUpT) Then
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Usage Read Failed.", Table))
      Else
        RaiseEvent ConnectionWBLResult(Me, New TYPEAResultEventArgs(StrToVal(sDown), StrToVal(sDownT), StrToVal(sUp), StrToVal(sUpT), Now))
      End If
    End If
  End Sub
#End Region
#Region "EX"
  Private Sub EX_Login_ReadLogin(sHost As String, sPath As String, sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean)
    If Not sHost = "mysso.exede.net" Then
      sErrMsg = "Prepare Failed: Domain redirected, check your Internet connection. [" & sHost & "]"
      bReset = False
    ElseIf sRet.ToLower.Contains("unable to process request") Then
      sErrMsg = "Login Failed: The server may be down."
      bReset = False
    ElseIf sPath = "/federation/ssoredirect/metaalias/idp" Then
      If sRet.Contains("<form") And sRet.Contains("name=""Login""") Then
        wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
        wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
        Dim sURI As String = sRet.Substring(sRet.IndexOf("name=""Login"""))
        sURI = sURI.Substring(sURI.IndexOf("action=""") + 8)
        sURI = sURI.Substring(0, sURI.IndexOf(""""))
        If sURI.StartsWith("/") Then sURI = "https://" & sHost & sURI
        Dim sGOTO As String = Nothing
        If sRet.Contains("<input type=""hidden"" name=""goto"" value=""") Then
          sGOTO = sRet.Substring(sRet.IndexOf("<input type=""hidden"" name=""goto"" value="""))
          sGOTO = sGOTO.Substring(sGOTO.IndexOf("value=""") + 7)
          If sGOTO.Contains(""" />") Then
            sGOTO = sGOTO.Substring(0, sGOTO.IndexOf(""" />"))
          ElseIf sGOTO.Contains("""") Then
            sGOTO = sGOTO.Substring(0, sGOTO.IndexOf(""""))
          End If
        End If
        If String.IsNullOrEmpty(sGOTO) Then
          sErrMsg = "Prepare Failed: GOTO value not found."
          sFailText = sRet
          bReset = False
          Exit Sub
        End If
        Dim sSQPS As String = Nothing
        If sRet.Contains("<input type=""hidden"" name=""SunQueryParamsString"" value=""") Then
          sSQPS = sRet.Substring(sRet.IndexOf("<input type=""hidden"" name=""SunQueryParamsString"" value="""))
          sSQPS = sSQPS.Substring(sSQPS.IndexOf("value=""") + 7)
          If sSQPS.Contains(""" />") Then
            sSQPS = sSQPS.Substring(0, sSQPS.IndexOf(""" />"))
          ElseIf sSQPS.Contains("""") Then
            sSQPS = sSQPS.Substring(0, sSQPS.IndexOf(""""))
          End If
        End If
        If String.IsNullOrEmpty(sSQPS) Then
          sErrMsg = "Prepare Failed: SunQueryParamsString value not found."
          sFailText = sRet
          bReset = False
          Exit Sub
        End If
        Dim sSend As String = "realm=" & PercentEncode("/") &
                             "&IDToken1=" & PercentEncode(myUID) &
                             "&IDToken2=" & PercentEncode(myPass) &
                             "&IDButton=Sign+in" &
                             "&goto=" & PercentEncode(sGOTO) &
                             "&SunQueryParamsString=" & PercentEncode(sSQPS) &
                             "&encoded=true" &
                             "&gx_charset=UTF-8"
        Dim aState As Object = BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, sURI)
        wsData.ErrorBypass = True
        wsData.UploadStringAsync(New Uri(sURI), "POST", sSend, aState)
        myUID = Nothing
        myPass = Nothing
      Else
        sErrMsg = "Prepare Failed: Login form not found."
        sFailText = "Exede Prepare Page Error = " & sErrMsg & vbNewLine & sRet
        bReset = False
      End If
    Else
      sErrMsg = "Prepare Failed: Could not understand response."
      sFailText = "Exede Prepare Error = " & sErrMsg & vbNewLine & sPath & vbNewLine & sRet
      bReset = True
    End If
  End Sub
  Private Sub EX_Login_Authenticate(sURL As String, sHost As String, sPath As String, sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean)
    If Not sHost = "mysso.exede.net" And Not sHost = "my.exede.net" Then
      sErrMsg = "Login Failed: Domain redirected, check your Internet connection. [" & sHost & "]"
      bReset = False
    ElseIf sPath = "/federation/ui/login" Or sPath = "/federation/ssoredirect/metaalias/idp" Then
      If sRet.Contains("Access rights validated") Then
        wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
        wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
        Dim sURI As String = Nothing
        If sRet.Contains("<form method=""post"" action=""") Then
          sURI = sRet.Substring(sRet.IndexOf("<form method=""post"" action="""))
          sURI = sURI.Substring(sURI.IndexOf("action=""") + 8)
          sURI = sURI.Substring(0, sURI.IndexOf(""">"))
          sURI = HexDecode(sURI)
        End If
        If String.IsNullOrEmpty(sURI) Then sURI = sURL
        Dim sSAMLResponse As String = Nothing
        If sRet.Contains("<input type=""hidden"" name=""SAMLResponse"" value=""") Then
          sSAMLResponse = sRet.Substring(sRet.IndexOf("<input type=""hidden"" name=""SAMLResponse"" value="""))
          sSAMLResponse = sSAMLResponse.Substring(sSAMLResponse.IndexOf("value=""") + 7)
          sSAMLResponse = sSAMLResponse.Substring(0, sSAMLResponse.IndexOf(""" />"))
        End If
        If String.IsNullOrEmpty(sSAMLResponse) Then
          sErrMsg = "Login Failed: SAML Response value not found."
          sFailText = sRet
          bReset = False
          Exit Sub
        End If
        Dim sRelay As String = Nothing
        If sRet.Contains("<input type=""hidden"" name=""RelayState"" value=""") Then
          sRelay = sRet.Substring(sRet.IndexOf("<input type=""hidden"" name=""RelayState"" value="""))
          sRelay = sRelay.Substring(sRelay.IndexOf("value=""") + 7)
          sRelay = sRelay.Substring(0, sRelay.IndexOf(""" />"))
        End If
        If String.IsNullOrEmpty(sRelay) Then
          sErrMsg = "Login Failed: Relay State value not found."
          sFailText = sRet
          bReset = False
          Exit Sub
        End If
        Dim sSend As String = "SAMLResponse=" & PercentEncode(HexDecode(sSAMLResponse)) & "&RelayState=" & PercentEncode(HexDecode(sRelay))
        Dim aState As Object = BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadHome, 0, sURI)
        wsData.UploadStringAsync(New Uri(sURI), "POST", sSend, aState)
      ElseIf sRet.Contains("login-error-alert") Then
        If sRet.ToLower.Contains("your username and/or password are incorrect.") Then
          sErrMsg = "Login Failed: Incorrect Password"
          bReset = False
        Else
          sErrMsg = "Unknown Login Error."
          sFailText = "Exede Login Page Error = " & sErrMsg & vbNewLine & sRet
          bReset = False
        End If
      ElseIf sRet.Contains("<input type=""hidden"" name=""goto"" value="""" />") Then
        sErrMsg = "Login Failed: Please check your account information and try again."
        bReset = False
      Else
        sErrMsg = "Could not log in."
        sFailText = "Exede Login Page Error = " & sErrMsg & vbNewLine & sRet
        bReset = False
      End If
    Else
      If sRet.Contains("window.location.href") Then
        wsData.ErrorBypass = False
        Dim sURI As String = Nothing
        sURI = sRet.Substring(sRet.IndexOf("window.location.href"))
        sURI = sURI.Substring(sURI.IndexOf("'") + 1)
        sURI = sURI.Substring(0, sURI.IndexOf("'"))
        If sURI = "/" Then
          sURI = sURL.Substring(0, sURL.IndexOf("/", sURL.IndexOf("//") + 2))
        End If
        Dim aState As Object = BeginAttempt(ConnectionStates.Login, ConnectionSubStates.AuthenticateRetry, 0, sURI)
        wsData.DownloadStringAsync(New Uri(sURI), aState)
      ElseIf sRet.Contains("maintenance") Then
        sErrMsg = "Login Failed: Server Down for Maintenance."
      Else
        sErrMsg = "Login Failed: Could not understand response."
        sFailText = "Exede Login Error = " & sErrMsg & vbNewLine & sPath & vbNewLine & sRet
        bReset = True
      End If
    End If
  End Sub
  Private Sub EX_Download_Homepage(sHost As String, sPath As String, sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean)
    If Not sHost = "myexede.force.com" Then
      sErrMsg = "Authentication Failed: Domain redirected, check your Internet connection. [" & sHost & "]"
      bReset = False
    ElseIf sPath = "/secur/frontdoor.jsp" Then
      Dim sURI As String = "https://myexede.force.com/dashboard"
      Dim aState As Object = BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadAJAX, (1 / 5), sURI)
      wsData.DownloadStringAsync(New Uri(sURI), aState)
    ElseIf sPath.ToLower.Contains("/identity/saml/samlerror") Then
      sErrMsg = "Authentication Failed: The server may be down."
      bReset = True
    Else
      sErrMsg = "Authentication Failed: Could not understand response."
      sFailText = "Exede Authentication Error = " & sErrMsg & vbNewLine & sPath & vbNewLine & sRet
      bReset = True
    End If
  End Sub
  Private Sub EX_Download_Ajax(sHost As String, sPath As String, sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean, AjaxID As String)
    If Not sHost = "myexede.force.com" Then
      sErrMsg = "Dashboard Load Failed: Domain redirected, check your Internet connection. [" & sHost & "]"
      bReset = False
    ElseIf sPath = "/dashboard" Then
      If sRet.Contains("<span id=""ajax-view-state""") Then
        wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
        wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
        Dim AjaxViewState As String = sRet.Substring(sRet.IndexOf("<span id=""ajax-view-state"""))
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
        Dim sSend As String = "AJAXREQUEST=_viewRoot" &
               "&j_id0%3AidForm=j_id0%3AidForm" &
               "&com.salesforce.visualforce.ViewState=" & PercentEncode(sViewState) &
               "&com.salesforce.visualforce.ViewStateVersion=" & PercentEncode(sVSVersion) &
               "&com.salesforce.visualforce.ViewStateMAC=" & PercentEncode(sVSMAC) &
               "&com.salesforce.visualforce.ViewStateCSRF=" & PercentEncode(sVSCSRF) &
               "&j_id0%3AidForm%3Aj_id" & AjaxID & "=j_id0%3AidForm%3Aj_id" & AjaxID
        Dim sURI As String = "https://myexede.force.com/dashboard?refURL=https%3A%2F%2Fmyexede.force.com%2Fdashboard"
        Dim subState As ConnectionSubStates = ConnectionSubStates.None
        Dim subVal As Decimal = 0
        Select Case AjaxID
          Case "2"
            subState = ConnectionSubStates.LoadAJAX
            subVal = (2 / 5)
          Case "5"
            subState = ConnectionSubStates.LoadAJAX
            subVal = (3 / 5)
          Case "6"
            subState = ConnectionSubStates.LoadAJAX
            subVal = (4 / 5)
          Case "4"
            subState = ConnectionSubStates.LoadAJAX
            subVal = (5 / 5)
          Case "3"
            subState = ConnectionSubStates.LoadTable
            subVal = 0
        End Select
        Dim aState As Object = BeginAttempt(ConnectionStates.TableDownload, subState, subVal, sURI)
        wsData.UploadStringAsync(New Uri(sURI), "POST", sSend, aState)
      ElseIf sRet.Contains("https://myexede.force.com/atlasPlanInvalid") Then
        sErrMsg = "You no longer have access to MyExede. Please check back again or contact Customer Care [(855) 463-9333] if the problem persists."
        bReset = False
      Else
        sErrMsg = "Dashboard Load Failed: Could not find AJAX ViewState variables."
        sFailText = "Exede Dashboard Page Error = " & sErrMsg & vbNewLine & sRet
        bReset = False
      End If
    Else
      sErrMsg = "Dashboard Load Failed: Could not understand response."
      sFailText = "Exede Dashboard Error = " & sErrMsg & vbNewLine & sPath & vbNewLine & sRet
      bReset = True
    End If
  End Sub
  Private Sub EX_Download_Table(sHost As String, sPath As String, sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean)
    If Not sHost = "myexede.force.com" Then
      sErrMsg = "Usage Failed: Domain redirected, check your Internet connection. [" & sHost & "]"
      bReset = False
    ElseIf sPath = "/dashboard" Then
      If sRet.Contains("amount-used") Then
        Dim sTable As String = sRet.Substring(sRet.LastIndexOf("<div class=""amount-used"">"))
        sTable = sTable.Substring(0, sTable.IndexOf("</p>") + 4)
        ReadUsage(sTable)
      ElseIf sRet.Contains("<span id=""ajax-view-state""") Then
        EX_Download_Ajax(sHost, sPath, sRet, sErrMsg, sFailText, bReset, "3")
      Else
        sErrMsg = "ViewState Load Failed: Could not find usage data."
        sFailText = "Exede AJAX Page Error = " & sErrMsg & vbNewLine & sRet
        bReset = False
      End If
    Else
      sErrMsg = "ViewState Load Failed: Could not understand response."
      sFailText = "Exede AJAX Error = " & sErrMsg & vbNewLine & sPath & vbNewLine & sRet
      bReset = True
    End If
  End Sub
  Private Sub EX_Read_Table(Table As String)
    If Not Table.Contains("amount-used") Then
      ResetTimeout()
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Usage Read Failed", Table))
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
      ResetTimeout()
      If lTotal > 0 Then
        RaiseEvent ConnectionWBXResult(Me, New TYPEBResultEventArgs(lUsed, lTotal, Now))
      Else
        RaiseEvent ConnectionWBXResult(Me, New TYPEBResultEventArgs(lUsed, 150000, Now))
      End If
    End If
  End Sub
#End Region
#Region "RP"
  Private Sub RP_Login_Authenticate(sHost As String, sPath As String, sQuery As String, sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean)
    If Not sHost = sProvider & ".ruralportal.net" Then
      sErrMsg = "Login Failed: Domain redirected, check your Internet connection. [" & sHost & "]"
      bReset = False
    ElseIf sPath.StartsWith("/us/home.do") Then
      LoadUsage("sitemanage")
    ElseIf sPath.StartsWith("/us/login.do") Then
      If String.IsNullOrEmpty(sQuery) Then
        If String.IsNullOrEmpty(sRet) Then
          sErrMsg = "Login Error: Sent back to login page with empty content."
          bReset = True
        ElseIf sRet.ToLower.Contains("confirmchange(msg);") Then
          ResetTimeout()
          RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginIssue, "Login Issue: Your password needs to be changed."))
          If Not String.IsNullOrEmpty(sProvider) Then
            If sProvider.Contains(".") Then sProvider = sProvider.Substring(0, sProvider.LastIndexOf("."))
            Dim uriString As String = String.Format(sRP, sProvider, "login")
            Dim cJar As Net.CookieContainer = wsData.CookieJar
            PrepareLogin()
            wsData.CookieJar = cJar
            wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
            wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
            Dim sSend As String = "warningTrip=true&userName=" & sAccount & "&passwd=" & PercentEncode(sPassword)
            Dim aState As Object = BeginAttempt(ConnectionStates.Login, ConnectionSubStates.AuthenticateRetry, 0, uriString)
            ResetTimeout(True)
            Try
              wsData.UploadStringAsync(New Uri(uriString), "POST", sSend, aState)
            Catch ex As Exception
              ResetTimeout()
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
  End Sub
  Private Sub RP_Login_AuthenticateRetry(sHost As String, sPath As String, sQuery As String, sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean)
    If Not sHost = sProvider & ".ruralportal.net" Then
      sErrMsg = "Login Retry Failed: Domain redirected, check your Internet connection. [" & sHost & "]"
      bReset = False
    ElseIf sPath.StartsWith("/us/home.do") Then
      LoadUsage("sitemanage")
    ElseIf sPath.StartsWith("/us/login.do") Then
      If String.IsNullOrEmpty(sQuery) Then
        If String.IsNullOrEmpty(sRet) Then
          sErrMsg = "Login Retry Error: Sent back to login page with empty content."
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
          sErrMsg = "Login Retry Failed: Sent back to login page."
          sFailText = "Login Retry Error = " & sErrMsg & vbNewLine & sRet
          bReset = True
        End If
      ElseIf sQuery.Contains("pass=false") Then
        sErrMsg = "Login Retry Failed: Incorrect password."
        bReset = False
      Else
        sErrMsg = "Login Retry Failed: " & sQuery & "."
        bReset = True
      End If
    Else
      sErrMsg = "Unknown Login Retry Result [" & sPath & "?" & sQuery & "]"
      bReset = True
    End If
  End Sub
  Private Sub RP_Download_Table(sHost As String, sPath As String, sQuery As String, sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean)
    If Not sHost = sProvider & ".ruralportal.net" Then
      sErrMsg = "Usage Failed: Domain redirected, check your Internet connection. [" & sHost & "]"
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
      ResetTimeout()
      If String.IsNullOrEmpty(sDownT) Or String.IsNullOrEmpty(sUpT) Then
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Usage Read Failed.", Table))
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
      ResetTimeout()
      If String.IsNullOrEmpty(sDownT) Then
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Usage Read Failed.", Table))
      Else
        RaiseEvent ConnectionRPXResult(Me, New TYPEBResultEventArgs(StrToVal(sDown, MBPerGB) + StrToVal(sOverhead, MBPerGB), StrToVal(sDownT, MBPerGB), Now))
      End If
    End If
  End Sub
#End Region
#Region "DN"
  Private Sub DN_Login_ReadLogin(sURI As String, sHost As String, sPath As String, sQuery As String, sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean)
    If Not sHost = "identity1.dishnetwork.com" Then
      sErrMsg = "Prepare Failed: Domain redirected, check your Internet connection. [" & sHost & "]"
      bReset = False
    ElseIf sPath.Contains("/firstbookend.php") Then
      If sQuery.StartsWith("?") Then
        wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
        wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
        Dim id As String = "0"
        If sRet.Contains("name=""id"" value=""") Then
          id = sRet.Substring(sRet.IndexOf("name=""id"" value=""") + 17)
          id = id.Substring(0, id.IndexOf(""""))
        End If
        sURI &= "&id=" & id
        sURI &= "&coeff=0"
        sURI &= "&history=" & iHist
        Dim aState As Object = BeginAttempt(ConnectionStates.Login, ConnectionSubStates.AuthPrepare, 0, sURI)
        wsData.DownloadStringAsync(New Uri(sURI), aState)
      Else
        sErrMsg = "Prepare Failed: AuthState is missing!"
        bReset = True
      End If
    Else
      sErrMsg = "Prepare Failed: Could not understand response."
      sFailText = "DishNet Prerpare Error = " & sErrMsg & vbNewLine & sPath & vbNewLine & sRet
      bReset = True
    End If
  End Sub
  Private Sub DN_Login_Prepare(sURI As String, sHost As String, sPath As String, sQuery As String, sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean)
    If Not sHost = "identity1.dishnetwork.com" Then
      sErrMsg = "FirstBookend Failed: Domain redirected, check your Internet connection. [" & sHost & "]"
      bReset = False
    ElseIf sPath.Contains("/login.php") Then
      If sQuery.StartsWith("?") Then
        Dim AuthState As String = sQuery.Substring(sQuery.IndexOf("=") + 1)
        wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
        wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
        Dim sSend As String = "username=" & PercentEncode(myUID) &
                              "&password=" & PercentEncode(myPass) &
                              "&login_type=username,password" &
                              "&source=" &
                              "&source_button="
        Dim aState As Object = BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, sURI)
        wsData.UploadStringAsync(New Uri(sURI), "POST", sSend, aState)
        myUID = Nothing
        myPass = Nothing
      Else
        sErrMsg = "FirstBookend Failed: AuthState is missing!"
        bReset = True
      End If
    ElseIf sPath.Contains("/firstbookend.php") Then
      DN_Login_ReadLogin(sURI, sHost, sPath, sQuery, sRet, sErrMsg, sFailText, bReset)
    Else
      sErrMsg = "FirstBookend Failed: Could not understand response."
      sFailText = "DishNet FirstBookend Error = " & sErrMsg & vbNewLine & sRet
      bReset = True
    End If
  End Sub
  Private Sub DN_Login_Authenticate(sURI As String, sHost As String, sPath As String, sQuery As String, sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean)
    If Not sHost = "identity1.dishnetwork.com" Then
      sErrMsg = "Login Failed: Domain redirected, check your Internet connection. [" & sHost & "]"
      bReset = False
    ElseIf sPath.Contains("/lastbookend.php") Then
      If sQuery.StartsWith("?") Then
        wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
        wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
        Dim id As String = "0"
        If sRet.Contains("name=""id"" value=""") Then
          id = sRet.Substring(sRet.IndexOf("name=""id"" value=""") + 17)
          id = id.Substring(0, id.IndexOf(""""))
        End If
        sURI &= "&id=" & id
        sURI &= "&coeff=1"
        sURI &= "&history=" & iHist
        Dim aState As Object = BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Verify, 0, sURI)
        wsData.DownloadStringAsync(New Uri(sURI), aState)
      Else
        sErrMsg = "Login Failed: AuthState is missing!"
        bReset = True
      End If
    ElseIf sPath.Contains("finish.php") Then
      If sRet.Contains("location.href") Then
        Dim sURL As String = Nothing
        sURL = sRet.Substring(sRet.IndexOf("location.href"))
        sURL = sURL.Substring(sURL.IndexOf("""") + 1)
        sURL = sURL.Substring(0, sURL.IndexOf(""""))
        If sURL = "/" Then
          sURL = sURI.Substring(0, sURI.IndexOf("/", sURI.IndexOf("//") + 2))
        End If
        Dim aState As Object = BeginAttempt(ConnectionStates.Login, ConnectionSubStates.AuthenticateRetry, 0, sURL)
        wsData.DownloadStringAsync(New Uri(sURL), aState)
      ElseIf sRet.Contains("Exception") Then
        If sRet.Contains("Unhandled Exception") Then
          sErrMsg = "Login Failed: Unhandled Exception!"
        ElseIf sRet.Contains("<h2>") Then
          sErrMsg = sRet.Substring(sRet.IndexOf("<h2>") + 4)
          sErrMsg = sErrMsg.Substring(0, sErrMsg.IndexOf("</h2>"))
          If sErrMsg.Length > 64 Then sErrMsg = sErrMsg.Substring(0, 64) & "..."
          sErrMsg = "Login Failed: " & sErrMsg
        Else
          sErrMsg = "Login Failed: Unknown Exception!"
        End If
        bReset = False
      Else
        sErrMsg = "Login Failed: Server issue."
        sFailText = "DishNet Login Error = " & sErrMsg & vbNewLine & sRet
        bReset = False
      End If
    Else
      sErrMsg = "Login Failed: Could not understand response."
      sFailText = "DishNet Login Error = " & sErrMsg & vbNewLine & sRet
      bReset = True
    End If
  End Sub
  Private Sub DN_Login_Verify(sURI As String, sHost As String, sPath As String, sQuery As String, sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean)
    If Not sHost = "identity1.dishnetwork.com" Then
      sErrMsg = "LastBookend Failed: Domain redirected, check your Internet connection. [" & sHost & "]"
      bReset = False
    ElseIf sPath.Contains("/lastbookend.php") Then
      If sRet.Contains("SAMLResponse"" value=""") Then
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
        Dim aState As Object = BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadHome, 0, uriString)
        wsData.UploadStringAsync(New Uri(uriString), "POST", sSend, aState)
      ElseIf sRet.Contains("The system is currently unavailable. Please try again later.") Then
        sErrMsg = "System currently unavailable."
        bReset = False
      ElseIf sRet.Contains("<div class=""custom_message_text"">") Then
        sErrMsg = sRet.Substring(sRet.IndexOf("<div class=""custom_message_text"">") + 33)
        sErrMsg = sErrMsg.Substring(0, sErrMsg.IndexOf("<"))
        sErrMsg = sErrMsg.Trim
        bReset = False
      Else
        sErrMsg = "No SAML Response"
        bReset = False
        sFailText = "DishNet LastBookend Error = " & sErrMsg & vbNewLine & sRet
      End If
    Else
      sErrMsg = "LastBookend Failed: Could not understand response."
      sFailText = "DishNet LastBookend Error = " & sErrMsg & vbNewLine & sRet
      bReset = True
    End If
  End Sub
  Private Sub DN_Download_Home(sHost As String, sPath As String, sQuery As String, sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean)
    If Not sHost = "my.dish.com" Then
      sErrMsg = "Login Failed: Domain redirected, check your Internet connection. [" & sHost & "]"
      bReset = False
    ElseIf sPath.Contains("/processsynacoreresponse.do") Then
      Dim uriString As String = "https://my.dish.com/customercare/usermanagement/getAccountNumberByUUID.do"
      Dim cJar As Net.CookieContainer = wsData.CookieJar
      PrepareLogin()
      wsData.CookieJar = cJar
      wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
      wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
      Dim sSend As String = "check="
      Dim aState As Object = BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTable, 0, uriString)
      wsData.UploadStringAsync(New Uri(uriString), "POST", sSend, aState)
    Else
      sErrMsg = "Unknown Authentication Result [" & sPath & "?" & sQuery & "]"
      bReset = True
    End If
  End Sub
  Private Sub DN_Download_Table(sHost As String, sPath As String, sQuery As String, sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean)
    If Not sHost = "my.dish.com" Then
      sErrMsg = "Login Failed: Domain redirected, check your Internet connection. [" & sHost & "]"
      bReset = False
    ElseIf sPath.Contains("/prepbroadband.do") Then
      If sRet.Contains("Data Plan start") Then
        Dim sUsageDiv As String = sRet.Substring(sRet.IndexOf("Data Plan start"))
        sUsageDiv = sUsageDiv.Substring(sUsageDiv.IndexOf(">") + 1)
        If sUsageDiv.Contains("Data Usage Graph End") Then
          sUsageDiv = sUsageDiv.Substring(0, sUsageDiv.IndexOf("Data Usage Graph End"))
          sUsageDiv = sUsageDiv.Substring(0, sUsageDiv.LastIndexOf("<"))
        End If
        ReadUsage(sUsageDiv)
      ElseIf sRet.Contains("<div id=""usagestatus""") Then
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
    Else
      Dim uriString As String = "https://my.dish.com/customercare/broadband/prepBroadBand.do"
      Dim cJar As Net.CookieContainer = wsData.CookieJar
      PrepareLogin()
      wsData.CookieJar = cJar
      wsData.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
      wsData.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
      Dim sSend As String = "srt=second_try"
      Dim aState As Object = BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTableRetry, 0, uriString)
      wsData.UploadStringAsync(New Uri(uriString), "POST", sSend, aState)
    End If
  End Sub
  Private Sub DN_Download_TableRetry(sHost As String, sPath As String, sQuery As String, sRet As String, ByRef sErrMsg As String, ByRef sFailText As String, ByRef bReset As Boolean)
    If Not sHost = "my.dish.com" Then
      sErrMsg = "Login Failed: Domain redirected, check your Internet connection. [" & sHost & "]"
      bReset = False
    ElseIf sPath.Contains("/prepbroadband.do") Then
      If sRet.Contains("Data Plan start") Then
        Dim sUsageDiv As String = sRet.Substring(sRet.IndexOf("Data Plan start"))
        sUsageDiv = sUsageDiv.Substring(sUsageDiv.IndexOf(">") + 1)
        If sUsageDiv.Contains("Data Usage Graph End") Then
          sUsageDiv = sUsageDiv.Substring(0, sUsageDiv.IndexOf("Data Usage Graph End"))
          sUsageDiv = sUsageDiv.Substring(0, sUsageDiv.LastIndexOf("<"))
        End If
        ReadUsage(sUsageDiv)
      ElseIf sRet.Contains("<div id=""usagestatus""") Then
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
    Else
      sErrMsg = "Usage Failed: Could not load usage meter page. Redirected to " & wsData.ResponseURI.ToString()
      sFailText = "DishNet Usage Error = " & sErrMsg & vbNewLine & sRet
      bReset = True
    End If
  End Sub
  Private Sub DN_Read_Table(Table As String)
    If Table.Contains("alertError") Then
      ResetTimeout()
      If Table.Contains("This information is currently unavailable.") Then
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Usage information currently unavailable."))
      Else
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Usage Read Failed.", Table))
      End If
    ElseIf Table.Contains("Data Usage Graph Start") Then
      Dim sMaxAnytime As String = Nothing
      If Table.Contains("Anytime") Then
        sMaxAnytime = Table.Substring(Table.IndexOf("Anytime"))
        sMaxAnytime = sMaxAnytime.Substring(0, sMaxAnytime.IndexOf("<div class=""row"""))
        sMaxAnytime = sMaxAnytime.Substring(sMaxAnytime.IndexOf("label"">") + 7)
        sMaxAnytime = sMaxAnytime.Substring(0, sMaxAnytime.IndexOf("</"))
        CleanupResult(sMaxAnytime)
      End If
      Dim sMaxOffPeak As String = Nothing
      If Table.Contains("Off-Peak") Then
        sMaxOffPeak = Table.Substring(Table.IndexOf("Off-Peak"))
        sMaxOffPeak = sMaxOffPeak.Substring(0, sMaxOffPeak.IndexOf("<div class=""row"""))
        sMaxOffPeak = sMaxOffPeak.Substring(sMaxOffPeak.IndexOf("label"">") + 7)
        sMaxOffPeak = sMaxOffPeak.Substring(0, sMaxOffPeak.IndexOf("</"))
        CleanupResult(sMaxOffPeak)
      End If
      Table = Table.Substring(Table.IndexOf("Data Usage Graph Start"))
      Table = Table.Substring(Table.IndexOf(">") + 1)
      Dim sAnytime As String = Nothing
      If Table.Contains("Anytime") Then
        sAnytime = Table.Substring(0, Table.IndexOf("Anytime"))
        sAnytime = sAnytime.Substring(sAnytime.LastIndexOf(">") + 1)
        CleanupResult(sAnytime)
      End If
      Dim sOffPeak As String = Nothing
      If Table.Contains("Off-Peak") Then
        sOffPeak = Table.Substring(0, Table.IndexOf("Off-Peak"))
        sOffPeak = sOffPeak.Substring(sOffPeak.LastIndexOf(">") + 1)
        CleanupResult(sOffPeak)
      End If
      Dim sAdditional As String = Nothing
      If Table.Contains("Additional") Then
        sAdditional = Table.Substring(0, Table.IndexOf("Additional"))
        sAdditional = sAdditional.Substring(sAdditional.LastIndexOf(">") + 1)
        CleanupResult(sAdditional)
        If sAdditional.Contains("&mdash;") Then sAdditional = Nothing
      End If
      If String.IsNullOrEmpty(sAnytime) Then
        ResetTimeout()
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Usage Read Failed.", Table))
      Else
        Dim lDown, lDownT, lUp, lUpT As Long
        If Not String.IsNullOrEmpty(sAnytime) Then
          lDown = StrToVal(sAnytime, MBPerGB)
        End If
        If Not String.IsNullOrEmpty(sMaxAnytime) Then
          lDownT = StrToVal(sMaxAnytime, MBPerGB)
          lDown = lDownT - lDown
        End If
        If Not String.IsNullOrEmpty(sOffPeak) Then
          lUp = StrToVal(sOffPeak, MBPerGB)
        End If
        If Not String.IsNullOrEmpty(sMaxOffPeak) Then
          lUpT = StrToVal(sMaxOffPeak, MBPerGB)
          lUp = lUpT - lUp
        End If
        If Not String.IsNullOrEmpty(sAdditional) Then
          lDownT += StrToVal(sAdditional, MBPerGB)
        End If
        If lDownT > 0 Then
          RaiseEvent ConnectionDNXResult(Me, New TYPEA2ResultEventArgs(lDown, lDownT, lUp, lUpT, Now))
        Else
          RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Usage Read Failed.", Table))
        End If
      End If
    Else
      DN_Read_OldTable(Table)
    End If
  End Sub
  Private Sub DN_Read_OldTable(Table As String)
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
          RaiseEvent ConnectionDNXResult(Me, New TYPEA2ResultEventArgs(lDown, lDownT, lUp, lUpT, Now))
        Else
          RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Usage Read Failed.", Table))
        End If
      End If
    End If
  End Sub
#End Region
#End Region
#Region "Useful Functions"
  Private Function BeginAttempt(state As ConnectionStates, substate As ConnectionSubStates, subval As Decimal, URL As String) As Object
    sAttemptedURL = URL
    AttemptedTag = state
    AttemptedSub = substate
    AttemptedVal = subval
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(state, substate, subval))
    Return {state, substate, subval}
  End Function
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
    Do
      result = Replace(result, "  ", " ")
    Loop While result.Contains("  ")
    result = Trim(result)
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
