Namespace Local
  ''' <summary>
  ''' Current status of the <see cref="SiteConnection" /> connection.
  ''' </summary>
  Public Enum SiteConnectionStates
    ''' <summary>
    ''' The class is being initialized.
    ''' </summary>
    Initialize
    ''' <summary>
    ''' Praparing to log in.
    ''' </summary>
    Prepare
    ''' <summary>
    ''' Logging in.
    ''' </summary>
    Login
    ''' <summary>
    ''' Downloading the Usage Table.
    ''' </summary>
    TableDownload
    ''' <summary>
    ''' Reading the Usage Table.
    ''' </summary>
    TableRead
  End Enum
  ''' <summary>
  ''' Current stage of the <see cref="SiteConnection" /> connection (more detailed than <see cref="SiteConnectionStates" />).
  ''' </summary>
  Public Enum SiteConnectionSubStates
    ''' <summary>
    ''' Use the <see cref="SiteConnectionStates" /> value instead of this value. There's no further breakdown of status.
    ''' </summary>
    None
    ''' <summary>
    ''' The login page is being read.
    ''' </summary>
    ReadLogin
    ''' <summary>
    ''' The username and password are being sent.
    ''' </summary>
    Authenticate
    ''' <summary>
    ''' The Code parameter from the home page URL is being loaded.
    ''' </summary>
    LoadHome
    ''' <summary>
    ''' Usage data is being requested and accessed.
    ''' </summary>
    LoadTable
  End Enum
  ''' <summary>
  ''' Types of connection failures.
  ''' </summary>
  Public Enum SiteConnectionFailureType
    ''' <summary>
    ''' The Username or Password are missing, so the <see cref="SiteConnection" /> connection can not begin. There should be no <see cref="SiteConnectionFailureEventArgs.Message" /> for this <see cref="SiteConnectionFailureEventArgs" />.
    ''' </summary>
    UnknownAccountDetails
    ''' <summary>
    ''' There was an issue while logging in, but the login process is still going through.
    ''' </summary>
    LoginIssue
    ''' <summary>
    ''' There was an issue while logging in. The <see cref="SiteConnectionFailureEventArgs.Message" /> will contain information about the <see cref="SiteConnection.ConnectionFailure" />.
    ''' </summary>
    LoginFailure
    ''' <summary>
    ''' The server timed out. There should be no <see cref="SiteConnectionFailureEventArgs.Message" /> for this <see cref="SiteConnectionFailureEventArgs" />.
    ''' </summary>
    ConnectionTimeout
    ''' <summary>
    ''' The version of TLS is too old. The <see cref="SiteConnectionFailureEventArgs.Message" /> for this <see cref="SiteConnectionFailureEventArgs" /> will either be the string "VER" if TLS 1.1 and 1.2 are both disabled, or "PROXY" if the TLS Proxy is disabled.
    ''' </summary>
    TLSTooOld
  End Enum

  ''' <summary>
  ''' Information regarding the type of <see cref="SiteConnection" /> connection failure received and any details.
  ''' </summary>
  Public Class SiteConnectionFailureEventArgs
    Inherits EventArgs
    Private m_FailType As SiteConnectionFailureType
    Private m_Fail As String
    Private m_Message As String
    ''' <summary>
    ''' Constructor for a <see cref="SiteConnectionFailureEventArgs" /> class, used in a <see cref="SiteConnection.ConnectionFailure" /> event to notify about a problem during connection.
    ''' </summary>
    ''' <param name="ftFailType">The <see cref="SiteConnectionFailureType" /> describes the type of <see cref="SiteConnection.ConnectionFailure" /> for this event. See the description for each type for details.</param>
    ''' <param name="sMessage">A message containing information about the <see cref="SiteConnection.ConnectionFailure" />. Not all <see cref="SiteConnectionFailureType">FailureTypes</see> use messages.</param>
    ''' <param name="sFailure">Extra data passed to the program about the <see cref="SiteConnection.ConnectionFailure" />, which can be reported to the RealityRipple Software servers to resolve in the next version. This is usually HTML and JavaScript from a single page of the login process or the meter.</param>
    Public Sub New(ftFailType As SiteConnectionFailureType, Optional sMessage As String = Nothing, Optional sFailure As String = Nothing)
      m_FailType = ftFailType
      m_Message = sMessage
      m_Fail = sFailure
    End Sub
    ''' <summary>
    ''' Extra data passed to the program about the <see cref="SiteConnection.ConnectionFailure" />, which can be reported to the RealityRipple Software servers to resolve in the next version. This is usually HTML and JavaScript from a single page of the login process or the meter.
    ''' </summary>
    Public ReadOnly Property Fail As String
      Get
        Return m_Fail
      End Get
    End Property
    ''' <summary>
    ''' A message containing information about the <see cref="SiteConnection.ConnectionFailure" />. Not all <see cref="SiteConnectionFailureType">FailureTypes</see> use messages.
    ''' </summary>
    Public ReadOnly Property Message As String
      Get
        Return m_Message
      End Get
    End Property
    ''' <summary>
    ''' The <see cref="SiteConnectionFailureType" /> describes the type of <see cref="SiteConnection.ConnectionFailure" /> for this event. See the description for each type for details.
    ''' </summary>
    Public ReadOnly Property [Type] As SiteConnectionFailureType
      Get
        Return m_FailType
      End Get
    End Property
  End Class
  ''' <summary>
  ''' Result from usage meter, containing Used and Limit values.
  ''' </summary>
  Public Class SiteResultEventArgs
    Inherits EventArgs
    Private m_Used As Long
    Private m_Limit As Long
    Private m_Update As Date
    Private m_slow As Boolean
    Private m_free As Boolean
    ''' <summary>
    ''' Constructor for a <see cref="SiteResultEventArgs" /> used in <see cref="SiteConnection.ConnectionResult" /> events.
    ''' </summary>
    ''' <param name="lUsed">Total number of megabytes used.</param>
    ''' <param name="lLimit">Total number of megabytes allowed.</param>
    ''' <param name="dUpdate">The specific date and time of this usage.</param>
    ''' <param name="bSlow"><c>True</c> if the connection has been reported as restricted, <c>False</c> otherwise.</param>
    ''' <param name="bFree"><c>True</c> if usage is reported not to count at the moment, <c>False</c> under normal conditions.</param>
    Public Sub New(lUsed As Long, lLimit As Long, dUpdate As Date, bSlow As Boolean, bFree As Boolean)
      m_Used = lUsed
      m_Limit = lLimit
      m_Update = dUpdate
      m_slow = bSlow
      m_free = bFree
    End Sub
    ''' <summary>
    ''' Total number of megabytes used.
    ''' </summary>
    Public ReadOnly Property Used As Long
      Get
        Return m_Used
      End Get
    End Property
    ''' <summary>
    ''' Total number of megabytes allowed.
    ''' </summary>
    Public ReadOnly Property Limit As Long
      Get
        Return m_Limit
      End Get
    End Property
    ''' <summary>
    ''' The specific date and time of this usage.
    ''' </summary>
    Public ReadOnly Property Update As Date
      Get
        Return m_Update
      End Get
    End Property
    ''' <summary>
    ''' If your connection is restricted, this value will be set to <c>True</c>.
    ''' </summary>
    Public ReadOnly Property SlowedDetected As Boolean
      Get
        Return m_slow
      End Get
    End Property
    ''' <summary>
    ''' If the usage isn't being counted, this value will be set to <c>True</c>.
    ''' </summary>
    Public ReadOnly Property FreeDetected As Boolean
      Get
        Return m_free
      End Get
    End Property
  End Class
  ''' <summary>
  ''' Class storing information regarding the current connection status, useful for displaying progress during connection or determining the location of an error.
  ''' </summary>
  Public Class SiteConnectionStatusEventArgs
    Inherits EventArgs
    Private m_state As SiteConnectionStates
    Private m_substate As SiteConnectionSubStates
    Private m_stage As Integer
    Private m_attempt As Integer
    ''' <summary>
    ''' Constructor for <see cref="SiteConnectionStatusEventArgs" /> class.
    ''' </summary>
    ''' <param name="status">Current status of the <see cref="SiteConnection" /> connection.</param>
    ''' <param name="substate">Current stage of the <see cref="SiteConnection" /> connection (more detailed than <see cref="SiteConnectionStates" />). This value is <see cref="SiteConnectionSubStates.None" /> by default.</param>
    ''' <param name="stage">The stage of the stage, so to speak. This numeric value contains detailed information on <paramref name="substate" /> values which may trigger more than one time.</param>
    ''' <param name="attempt">The number of retries the current connection is on. Retries are normal during connection, as part of the redirection process.</param>
    Public Sub New(status As SiteConnectionStates, Optional substate As SiteConnectionSubStates = SiteConnectionSubStates.None, Optional stage As Integer = 0, Optional attempt As Integer = 0)
      m_state = status
      m_substate = substate
      m_stage = stage
      m_attempt = attempt
    End Sub
    ''' <summary>
    ''' Current status of the <see cref="SiteConnection" /> connection.
    ''' </summary>
    Public ReadOnly Property Status As SiteConnectionStates
      Get
        Return m_state
      End Get
    End Property
    ''' <summary>
    ''' Current stage of the <see cref="SiteConnection" /> connection (more detailed than <see cref="SiteConnectionStates" />).
    ''' </summary>
    Public ReadOnly Property SubState As SiteConnectionSubStates
      Get
        Return m_substate
      End Get
    End Property
    ''' <summary>
    ''' The stage of the stage, so to speak. This numeric value contains detailed information on <see cref="SubState" /> values which may trigger more than one time.
    ''' </summary>
    Public ReadOnly Property Stage As Integer
      Get
        Return m_stage
      End Get
    End Property
    ''' <summary>
    ''' The number of retries the current connection is on. Retries are normal during connection, as part of the redirection process.
    ''' </summary>
    Public ReadOnly Property Attempt As Integer
      Get
        Return m_attempt
      End Get
    End Property
  End Class

  ''' <summary>
  ''' Accesses usage pages and handles all communication internally.
  ''' </summary>
  Public Class SiteConnection
    Implements IDisposable
#Region "Events"
    ''' <summary>
    ''' Triggered when the connection to a usage page fails or has an issue.
    ''' </summary>
    ''' <param name="sender">Instance of the <see cref="SiteConnection" /> class.</param>
    ''' <param name="e"><see cref="SiteConnectionFailureEventArgs" /> data regarding the failure.</param>
    Public Event ConnectionFailure As EventHandler(Of SiteConnectionFailureEventArgs)
    ''' <summary>
    ''' Triggered when the server returns data for an account.
    ''' </summary>
    ''' <param name="sender">Instance of the <see cref="SiteConnection" /> class.</param>
    ''' <param name="e"><see cref="SiteResultEventArgs" /> data regarding the result.</param>
    Public Event ConnectionResult As EventHandler(Of SiteResultEventArgs)
    ''' <summary>
    ''' Triggered when new information regarding the current connection status is available.
    ''' </summary>
    ''' <param name="sender">Instance of the <see cref="SiteConnection" /> class.</param>
    ''' <param name="e"><see cref="SiteConnectionStatusEventArgs" /> data regarding the current state of the connection.</param>
    Public Event ConnectionStatus As EventHandler(Of SiteConnectionStatusEventArgs)
    ''' <summary>
    ''' Triggered when the server verifies that the account's login information is correct.
    ''' </summary>
    ''' <param name="sender">Instance of the <see cref="SiteConnection" /> class.</param>
    ''' <param name="e">Unset</param>
    Public Event LoginComplete As EventHandler
#End Region
    Private mySettings As AppSettings
    Private Const MBPerGB As Integer = 1000
    Private justATest As Boolean
    Private sUsername, sPassword As String
    Private imSlowed As Boolean
    Private imFree As Boolean
    Private ClosingTime As Boolean
    Private tConnect As Threading.Thread
    Private c_Timeout As Integer
    Private c_Proxy As Net.IWebProxy
    Private c_Jar As Net.CookieContainer
    Private c_SendJar As Boolean
    Private c_TLSProxy As Boolean
    Private c_TLSProxyAddr As String
    Private sDataPath As String
    Private wsSocket As WebClientEx
#Region "Initialization Functions"
    ''' <summary>
    ''' Constructor for the <see cref="SiteConnection" /> class, which also begins the connection process.
    ''' </summary>
    ''' <param name="ConfigPath">Directory where the user.config file is stored.</param>
    ''' <param name="OnlyLogin">If set to <c>True</c>, this connection will abort after verifying that the Username and Password are valid, without returning data. This value is <c>False</c> by default.</param>
    Public Sub New(ConfigPath As String, Optional OnlyLogin As Boolean = False)
      justATest = OnlyLogin
      Randomize()
      ClosingTime = False
      sDataPath = ConfigPath
      If mySettings Is Nothing Then mySettings = New AppSettings(IO.Path.Combine(ConfigPath, "user.config"))
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
      Dim iWait As Integer = Net.ServicePointManager.MaxServicePointIdleTime
      Net.ServicePointManager.MaxServicePointIdleTime = 1
      If mySettings.SecurityEnforced Then
        Net.ServicePointManager.ServerCertificateValidationCallback = Nothing
      Else
        Net.ServicePointManager.ServerCertificateValidationCallback = New Net.Security.RemoteCertificateValidationCallback(AddressOf IgnoreCert)
      End If
      Net.ServicePointManager.MaxServicePointIdleTime = iWait
      If Not String.IsNullOrEmpty(mySettings.PassCrypt) Then
        If String.IsNullOrEmpty(mySettings.PassKey) Or String.IsNullOrEmpty(mySettings.PassSalt) Then
          sPassword = StoredPasswordLegacy.DecryptApp(mySettings.PassCrypt)
        Else
          sPassword = StoredPassword.Decrypt(mySettings.PassCrypt, mySettings.PassKey, mySettings.PassSalt)
        End If
      End If
      sUsername = mySettings.Account
      c_TLSProxy = mySettings.TLSProxy
      If c_TLSProxy Then c_TLSProxyAddr = "http://wb.realityripple.com/tls.php"
      c_Timeout = mySettings.Timeout
      c_Proxy = mySettings.Proxy
      Dim sFramework As String = srlFunctions.CLRCleanVersion
      If sFramework.Contains("MONO") Then
        Dim sFWVer As String = sFramework.Substring(5)
        Dim fwMajor As Integer = sFWVer.Substring(0, sFWVer.IndexOf("."))
        sFWVer = sFWVer.Substring(sFWVer.IndexOf(".") + 1)
        Dim fwMinor As Integer = 0
        If sFWVer.Contains(".") Then
          fwMinor = sFWVer.Substring(0, sFWVer.IndexOf("."))
        Else
          fwMinor = sFWVer
        End If
        If fwMajor > 4 Then
          c_SendJar = False
        Else
          If fwMinor >= 8 Then
            c_SendJar = False
          Else
            c_SendJar = True
          End If
        End If
      Else
        c_SendJar = False
      End If
      tConnect = New Threading.Thread(AddressOf GetUsage)
      tConnect.Start()
    End Sub
#End Region
#Region "Login Functions"
    Private Sub GetUsage()
      If String.IsNullOrEmpty(sUsername) Or String.IsNullOrEmpty(sPassword) Then
        RaiseEvent ConnectionFailure(Me, New SiteConnectionFailureEventArgs(SiteConnectionFailureType.UnknownAccountDetails))
      Else
        RaiseEvent ConnectionStatus(Me, New SiteConnectionStatusEventArgs(SiteConnectionStates.Initialize))
        Login()
      End If
    End Sub
    Private Sub Login()
      c_Jar = New Net.CookieContainer
      RaiseEvent ConnectionStatus(Me, New SiteConnectionStatusEventArgs(SiteConnectionStates.Prepare))
      Dim uriString As String = "https://my-viasat.ts-usage.prod.icat.viasat.io/auth"
      EX_Init(uriString)
    End Sub
#End Region
#Region "Parsing Functions"
    Private Sub ReadUsage(Table As String)
      RaiseEvent ConnectionStatus(Me, New SiteConnectionStatusEventArgs(SiteConnectionStates.TableRead))
      EX_Read_Table(Table)
      c_Jar = New Net.CookieContainer
      srlFunctions.SendSocketErrors(sDataPath)
    End Sub
#Region "EX Helper Functions"
    Private Shared Function EX_Helper_FindBetween(find As String, groupS As String, groupE As String, sepS As Char, sepE As Char) As String()
      Dim gS As Integer = find.IndexOf(groupS)
      If gS = -1 Then Return Nothing
      find = find.Substring(gS + groupS.Length)
      Dim gE As Integer = find.IndexOf(groupE)
      If gE = -1 Then Return Nothing
      find = find.Substring(0, gE)
      Dim ret As New List(Of String)
      Dim d As String = vbNullChar
      For I As Integer = 0 To find.Length - 1
        If d = vbNullChar Then
          If find(I) = sepS Then d = ""
        Else
          If find(I) = sepE Then
            ret.Add(d)
            d = vbNullChar
          Else
            d = d & find(I)
          End If
        End If
      Next
      If ret.Count = 0 Then Return Nothing
      Return ret.ToArray
    End Function
    Private Shared Function EX_Helper_WithinParens(find As String) As String
      Dim quote1 As Boolean = False
      Dim quote2 As Boolean = False
      Dim parens As Integer = 0
      For I As Integer = 0 To find.Length - 1
        If find(I) = "'"c Then
          If quote1 Then
            quote1 = False
          ElseIf Not quote2 Then
            quote1 = True
          End If
        End If
        If find(I) = """"c Then
          If quote2 Then
            quote2 = False
          ElseIf Not quote1 Then
            quote2 = True
          End If
        End If
        If quote1 Or quote2 Then Continue For
        If find(I) = "{"c Then parens += 1
        If find(I) = "}"c Then parens -= 1
        If parens = 0 Then
          find = find.Substring(0, I + 1)
          Exit For
        End If
      Next
      Return find
    End Function
    Private Shared Function EX_Helper_Unescape(d As String)
      For I As Integer = 0 To 255
        Dim h As String = srlFunctions.PadHex(I, 2).ToUpperInvariant
        Dim c As Char = ChrW(I)
        d = d.Replace("\x" & h, c)
      Next
      Return d
    End Function
    Private Shared Function EX_Helper_Parse_okta(body As String) As Dictionary(Of String, String)
      Dim ret As New Dictionary(Of String, String)
      ret.Add("base", "https://login.viasat.com/oauth2/auss5o0l1DDF9mIke696")
      ret.Add("client_id", "0oa1z83muzscFURhw697")
      ret.Add("redirect_uri", "https://my.viasat.com/callback")
      Dim oktaLoc As Integer = body.IndexOf("okta:{")
      If oktaLoc = -1 Then Return ret
      Dim okta As String = body.Substring(oktaLoc + 5)
      okta = EX_Helper_WithinParens(okta)
      okta = System.Text.RegularExpressions.Regex.Replace(okta, "([{,])([A-Za-z_$][A-Za-z0-9_$]+):", "$1""$2"":")
      Dim jOkta As JSONReader = Nothing
      Try
        Using jStream As New System.IO.MemoryStream(Text.Encoding.GetEncoding(srlFunctions.UTF_8).GetBytes(okta))
          jOkta = New JSONReader(jStream, True)
        End Using
      Catch ex As Exception
        Return ret
      End Try
      If jOkta.JSON.GetType Is GetType(JSONFailure) Then Return ret
      Try
        Dim assoc As Object = JSONAssociator.Associate(jOkta)
        If Not assoc.ContainsKey("prod") Then Return ret
        If assoc("prod").ContainsKey("baseUrl") Then ret("base") = assoc("prod")("baseUrl")
        If assoc("prod").ContainsKey("webClient") Then
          If assoc("prod")("webClient").ContainsKey("clientId") Then ret("client_id") = assoc("prod")("webClient")("clientId")
          If assoc("prod")("webClient").ContainsKey("redirectUrl") Then ret("redirect_uri") = assoc("prod")("webClient")("redirectUrl")
        End If
      Catch ex As Exception

      End Try
      Return ret
    End Function
    Private Shared Function EX_Helper_Parse_scope(body As String) As String
      Dim ret As String = "openid profile email offline_access"
      Dim scopeData As String() = EX_Helper_FindBetween(body, "l=[", "]", """"c, """"c)
      If scopeData IsNot Nothing Then ret = Join(scopeData)
      Return ret
    End Function
    Private Shared Function EX_Helper_Parse_urlBuilder(body As String) As Dictionary(Of String, String)
      Dim ret As New Dictionary(Of String, String)
      ret.Add("path", "/v1/authorize?")
      ret.Add("response_type", "code")
      ret.Add("state", "asdfasdf")
      Dim fLoc As Integer = body.IndexOf("var i,s="""".concat(t).concat(")
      If fLoc = -1 Then Return ret
      Dim f As String = body.Substring(fLoc)
      Dim fEnd As Integer = f.IndexOf("}))")
      If fEnd = -1 Then Return ret
      f = f.Substring(0, fEnd + 3)
      Dim pathData As String() = EX_Helper_FindBetween(f, "s="""".concat(t).concat(", ")", """"c, """"c)
      If pathData IsNot Nothing Then ret("path") = Join(pathData, "")
      Dim respData As String() = EX_Helper_FindBetween(f, "response_type:", ",", """"c, """"c)
      If respData IsNot Nothing Then ret("response_type") = respData(0)
      Dim stateData As String() = EX_Helper_FindBetween(f, "state:", "}", """"c, """"c)
      If stateData IsNot Nothing Then ret("state") = stateData(0)
      Return ret
    End Function
    Private Shared Function EX_Helper_MakeLoginFromStruct(struct As Object, valList As Dictionary(Of String, String)) As Dictionary(Of String, Object)
      Dim oRet As New Dictionary(Of String, Object)
      If Not struct.ContainsKey("value") Then Return New Dictionary(Of String, Object)
      If Not IsArray(struct("value")) Then Return New Dictionary(Of String, Object)
      For I As Integer = 0 To struct("value").Length - 1
        If Not struct("value")(I).ContainsKey("name") Then Continue For
        Dim prefName As String = struct("value")(I)("name")
        If valList.ContainsKey(prefName) Then
          oRet.Add(prefName, valList(prefName))
          Continue For
        End If
        If struct("value")(I).ContainsKey("value") Then
          oRet.Add(prefName, struct("value")(I)("value"))
          Continue For
        End If
        If struct("value")(I).ContainsKey("type") Then
          Dim r As Object = Nothing
          Select Case struct("value")(I)("type")
            Case "object"
              If struct("value")(I).ContainsKey("form") Then r = EX_Helper_MakeLoginFromStruct(struct("value")(I)("form"), valList)
            Case "boolean"
              r = Nothing
          End Select
          If Not r Is Nothing Then oRet.Add(prefName, r)
        End If
      Next
      Return oRet
    End Function
#End Region
#Region "EX"
    Private Sub EX_Init(sURI As String)
      MakeSocket(True)
      RaiseEvent ConnectionStatus(Me, New SiteConnectionStatusEventArgs(SiteConnectionStates.Initialize))
      Dim responseData As String = Nothing
      Dim responseURI As Uri = Nothing
      SendGET(New Uri(sURI), responseURI, responseData)
      If ClosingTime Then Return
      EX_Init_Response(responseData, responseURI)
    End Sub
    Private Sub EX_Init_Response(Response As String, ResponseURI As Uri)
      If CheckForErrors(Response, ResponseURI) Then Return
      Dim appLoc As Integer = Response.IndexOf("_app")
      If appLoc = -1 Then
        RaiseError("Initialize Failed: Could not find OAuth Script.", "EX Init", Response, ResponseURI)
        Return
      End If
      Dim scriptLoc As Integer = Response.Substring(0, appLoc).LastIndexOf("<script")
      If scriptLoc = -1 Then
        RaiseError("Initialize Failed: Could not find OAuth Script Tag.", "EX Init", Response, ResponseURI)
        Return
      End If
      Dim scriptSrc As String = Response.Substring(scriptLoc)
      Dim srcStart As Integer = scriptSrc.IndexOf("""")
      If srcStart = -1 Then
        RaiseError("Initialize Failed: Could not find OAuth Script URL.", "EX Init", Response, ResponseURI)
        Return
      End If
      scriptSrc = scriptSrc.Substring(srcStart + 1)
      Dim srcEnd As Integer = scriptSrc.IndexOf("""")
      If srcEnd = -1 Then
        RaiseError("Initialize Failed: Could not find OAuth Script URL.", "EX Init", Response, ResponseURI)
        Return
      End If
      scriptSrc = scriptSrc.Substring(0, srcEnd)
      Dim host As String = ResponseURI.Scheme & Uri.SchemeDelimiter & ResponseURI.Host
      Dim url As New Uri(host & scriptSrc)
      EX_OAuth(url)
    End Sub
    Private Sub EX_OAuth(sURI As Uri)
      MakeSocket(True)
      RaiseEvent ConnectionStatus(Me, New SiteConnectionStatusEventArgs(SiteConnectionStates.Prepare))
      Dim responseData As String = Nothing
      Dim responseURI As Uri = Nothing
      SendGET(sURI, responseURI, responseData)
      If ClosingTime Then Return
      EX_OAuth_Response(responseData, responseURI)
    End Sub
    Private Sub EX_OAuth_Response(Response As String, ResponseURI As Uri)
      If CheckForErrors(Response, ResponseURI) Then Return
      Dim okData As Dictionary(Of String, String) = EX_Helper_Parse_okta(Response)
      Dim scope As String = EX_Helper_Parse_scope(Response)
      Dim uData As Dictionary(Of String, String) = EX_Helper_Parse_urlBuilder(Response)
      Dim url As New Uri(okData("base") & uData("path") &
                         "client_id=" & System.Uri.EscapeDataString(okData("client_id")) &
                         "&response_type=" & System.Uri.EscapeDataString(uData("response_type")) &
                         "&redirect_uri=" & System.Uri.EscapeDataString(okData("redirect_uri")) &
                         "&scope=" & System.Uri.EscapeDataString(scope) &
                         "&state=" & System.Uri.EscapeDataString(uData("state")))
      EX_ReadLogin(url)
    End Sub
    Private Sub EX_ReadLogin(sURI As Uri)
      MakeSocket(True)
      RaiseEvent ConnectionStatus(Me, New SiteConnectionStatusEventArgs(SiteConnectionStates.Login, SiteConnectionSubStates.ReadLogin))
      Dim responseData As String = Nothing
      Dim responseURI As Uri = Nothing
      SendGET(sURI, responseURI, responseData)
      If ClosingTime Then Return
      EX_ReadLogin_Response(responseData, responseURI)
    End Sub
    Private Sub EX_ReadLogin_Response(Response As String, ResponseURI As Uri)
      If CheckForErrors(Response, ResponseURI) Then Return
      Dim oktaDataLoc As Integer = Response.IndexOf("var oktaData = {")
      If oktaDataLoc = -1 Then
        RaiseError("Login Failed: Could not parse Okta response.", "EX Read Login Response", Response, ResponseURI)
        Return
      End If
      Dim oktaData As String = Response.Substring(oktaDataLoc + 15)
      Dim oktaDataEnd As Integer = oktaData.IndexOf("};")
      If oktaDataEnd = -1 Then
        RaiseError("Login Failed: Could not parse Okta response.", "EX Read Login Response", Response, ResponseURI)
        Return
      End If
      oktaData = oktaData.Substring(0, oktaDataEnd + 1)
      While oktaData.Contains("function(){")
        Dim fn As String = oktaData.Substring(oktaData.IndexOf("function(){") + 10)
        fn = EX_Helper_WithinParens(fn)
        oktaData = oktaData.Replace("function()" & fn, """[JS Function]""")
      End While
      oktaData = EX_Helper_Unescape(oktaData)
      Dim jData As JSONReader = Nothing
      Try
        Using jStream As New System.IO.MemoryStream(Text.Encoding.GetEncoding(srlFunctions.UTF_8).GetBytes(oktaData))
          jData = New JSONReader(jStream, True)
        End Using
      Catch ex As Exception
        RaiseError("Reading Login Failed: Could not parse Okta JSON.", "EX Read Login Response", Response, ResponseURI)
        Return
      End Try
      If jData.JSON.GetType Is GetType(JSONFailure) Then
        RaiseError("Reading Login Failed: Could not parse Okta JSON.", "EX Read Login Response", Response, ResponseURI)
        Return
      End If
      Try
        Dim assoc As Object = JSONAssociator.Associate(jData)
        If Not assoc.ContainsKey("signIn") Then
          RaiseError("Reading Login Failed: Could not parse Okta JSON.", "EX Read Login Response", Response, ResponseURI)
          Return
        End If
        If Not assoc("signIn").ContainsKey("stateToken") Then
          RaiseError("Reading Login Failed: Could not parse Okta JSON.", "EX Read Login Response", Response, ResponseURI)
          Return
        End If
        Dim sBody As String = "{""stateToken"":""" & assoc("signIn")("stateToken") & """}"
        Dim url As New Uri("https://login.viasat.com/idp/idx/introspect")
        EX_Login(url, sBody)
      Catch ex As Exception
        RaiseError("Reading Login Failed: Could not parse Okta JSON.", "EX Read Login Response", Response, ResponseURI)
      End Try
    End Sub
    Private Sub EX_Login(sURI As Uri, POSTData As String)
      MakeSocket(True)
      RaiseEvent ConnectionStatus(Me, New SiteConnectionStatusEventArgs(SiteConnectionStates.Login))
      Dim responseData As String = Nothing
      Dim responseURI As Uri = Nothing
      Dim aHeader As New Net.WebHeaderCollection()
      aHeader.Add(Net.HttpRequestHeader.Accept, "application/ion+json; okta-version=1.0.0")
      aHeader.Add(Net.HttpRequestHeader.ContentType, "application/ion+json; okta-version=1.0.0")
      SendPOST(sURI, POSTData, responseURI, responseData, aHeader)
      If ClosingTime Then Return
      EX_Login_Response(responseData, responseURI)
    End Sub
    Private Sub EX_Login_Response(Response As String, ResponseURI As Uri)
      If CheckForErrors(Response, ResponseURI) Then Return
      Dim exJS As JSONReader
      Try
        Using jStream As New System.IO.MemoryStream(Text.Encoding.GetEncoding(srlFunctions.UTF_8).GetBytes(Response))
          exJS = New JSONReader(jStream, True)
        End Using
      Catch ex As Exception
        RaiseError("Login Failed: Could not parse login response.", "EX Login Response", Response, ResponseURI)
        Return
      End Try
      If exJS.JSON.GetType Is GetType(JSONFailure) Then
        RaiseError("Login Failed: Could not parse login response.", "EX Login Response", Response, ResponseURI)
        Return
      End If
      Try
        Dim assoc As Object = JSONAssociator.Associate(exJS)
        Dim formData As Object = Nothing
        If Not assoc.ContainsKey("remediation") Then
          RaiseError("Login Failed: Could not parse login response JSON.", "EX Login Response", Response, ResponseURI)
          Return
        End If
        If Not assoc("remediation").ContainsKey("value") Then
          RaiseError("Login Failed: Could not parse login response JSON.", "EX Login Response", Response, ResponseURI)
          Return
        End If
        If Not IsArray(assoc("remediation")("value")) Then
          RaiseError("Login Failed: Could not parse login response JSON.", "EX Login Response", Response, ResponseURI)
          Return
        End If
        For I As Integer = 0 To assoc("remediation")("value").Length - 1
          If Not assoc("remediation")("value")(I).ContainsKey("name") Then Continue For
          If Not assoc("remediation")("value")(I)("name") = "identify" Then Continue For
          If Not assoc("remediation")("value")(I).ContainsKey("method") Then Continue For
          If Not assoc("remediation")("value")(I)("method") = "POST" Then Continue For
          If Not assoc("remediation")("value")(I).ContainsKey("accepts") Then Continue For
          If Not assoc("remediation")("value")(I).ContainsKey("produces") Then Continue For
          If Not assoc("remediation")("value")(I).ContainsKey("href") Then Continue For
          If Not assoc("remediation")("value")(I).ContainsKey("value") Then Continue For
          If Not IsArray(assoc("remediation")("value")(I)("value")) Then Continue For
          formData = assoc("remediation")("value")(I)
          Exit For
        Next
        Dim url As New Uri(formData("href"))
        Dim valList As New Dictionary(Of String, String)
        valList.Add("identifier", sUsername)
        valList.Add("passcode", sPassword)
        Dim body As Dictionary(Of String, Object) = EX_Helper_MakeLoginFromStruct(formData, valList)
        Dim sBody As String = New JSONObject(body).ToString
        Dim aHeaders As New Net.WebHeaderCollection
        aHeaders.Add(Net.HttpRequestHeader.Accept, formData("accepts"))
        aHeaders.Add(Net.HttpRequestHeader.ContentType, formData("produces"))
        EX_Auth(url, aHeaders, sBody)
      Catch ex As Exception
        RaiseError("Login Failed: Could not parse login response JSON.", "EX Login Response", Response, ResponseURI)
      End Try
    End Sub
    Private Sub EX_Auth(sURI As Uri, aHeaders As Net.WebHeaderCollection, POSTData As String)
      MakeSocket(True)
      RaiseEvent ConnectionStatus(Me, New SiteConnectionStatusEventArgs(SiteConnectionStates.Login, SiteConnectionSubStates.Authenticate))
      Dim responseData As String = Nothing
      Dim responseURI As Uri = Nothing
      SendPOST(sURI, POSTData, responseURI, responseData, aHeaders)
      If ClosingTime Then Return
      EX_Auth_Response(responseData, responseURI)
    End Sub
    Private Sub EX_Auth_Response(Response As String, ResponseURI As Uri)
      If CheckForErrors(Response, ResponseURI) Then Return
      Dim jsAuth As JSONReader
      Try
        Using jStream As New System.IO.MemoryStream(Text.Encoding.GetEncoding(srlFunctions.UTF_8).GetBytes(Response))
          jsAuth = New JSONReader(jStream, True)
        End Using
      Catch ex As Exception
        RaiseError("Authentication Failed: Could not parse auth response.", "EX Auth Response", Response, ResponseURI)
        Return
      End Try
      If jsAuth.JSON.GetType Is GetType(JSONFailure) Then
        RaiseError("Authentication Failed: Could not parse auth response.", "EX Auth Response", Response, ResponseURI)
        Return
      End If
      Try
        Dim assoc As Object = JSONAssociator.Associate(jsAuth)
        If assoc.ContainsKey("messages") Then
          If assoc("messages").ContainsKey("value") AndAlso IsArray(assoc("messages")("value")) Then
            For I As Integer = 0 To assoc("messages")("value").Length - 1
              If Not assoc("messages")("value")(I).ContainsKey("message") Then Continue For
              If assoc("messages")("value")(I)("message") = "Authentication failed" Then
                RaiseError("Login Failed: Incorrect Password")
                Return
              End If
            Next
          End If
        End If
        If Not assoc.ContainsKey("success") Then
          RaiseError("Authentication Failed: Could not parse auth response.", "EX Auth Response", Response, ResponseURI)
          Return
        End If
        If Not assoc("success").ContainsKey("href") Then
          RaiseError("Authentication Failed: Could not parse auth response.", "EX Auth Response", Response, ResponseURI)
          Return
        End If
        Dim url As New Uri(assoc("success")("href"))
        EX_Home(url)
      Catch ex As Exception
        RaiseError("Authentication Failed: Could not parse auth response.", "EX Auth Response", Response, ResponseURI)
      End Try
    End Sub
    Private Sub EX_Home(sURI As Uri)
      MakeSocket(True)
      RaiseEvent ConnectionStatus(Me, New SiteConnectionStatusEventArgs(SiteConnectionStates.TableDownload, SiteConnectionSubStates.LoadHome))
      Dim responseData As String = Nothing
      Dim responseURI As Uri = Nothing
      SendGET(sURI, responseURI, responseData)
      If ClosingTime Then Return
      EX_Home_Response(responseData, responseURI)
    End Sub
    Private Sub EX_Home_Response(Response As String, ResponseURI As Uri)
      If CheckForErrors(Response, ResponseURI) Then Return
      Dim sToken As String = ResponseURI.Query
      If String.IsNullOrEmpty(sToken) Then
        RaiseError("Could not log in.", "EX Home Response", Response, ResponseURI)
        Return
      End If
      If Not sToken.Contains("code=") Then
        If Response.Contains("Access Forbidden") Then
          RaiseEvent ConnectionFailure(Me, New SiteConnectionFailureEventArgs(SiteConnectionFailureType.TLSTooOld, "PROXY"))
          Return
        End If
        RaiseError("Could not log in.", "EX Home Response", Response, ResponseURI)
        Return
      End If
      sToken = sToken.Substring(sToken.IndexOf("code=") + 5)
      If sToken.Contains("&") Then sToken = sToken.Substring(0, sToken.IndexOf("&"))
      EX_Token(sToken)
    End Sub
    Private Sub EX_Token(sCode As String)
      Dim tURI As String = "https://my-viasat.ts-usage.prod.icat.viasat.io/api/graphql"
      Dim aSend As New Dictionary(Of String, Object)
      aSend.Add("operationName", "getTokenUsingCode")
      Dim aInput As New Dictionary(Of String, Object)
      aInput.Add("code", sCode)
      aInput.Add("platform", "Web")
      aInput.Add("env", "prod")
      Dim aVars As New Dictionary(Of String, Object)
      aVars.Add("input", aInput)
      aSend.Add("variables", aVars)
      aSend.Add("query", "query getTokenUsingCode($input: GetTokenUsingCodeInput!) {\n" &
                         "  getTokenUsingCode(input: $input) {\n" &
                         "    accessToken\n" &
                         "    __typename\n" &
                         "  }\n" &
                         "}\n")
      Dim sSend As String = New JSONObject(aSend).ToString
      Dim hdrs As New Net.WebHeaderCollection
      hdrs.Add(Net.HttpRequestHeader.ContentType, "application/json")
      MakeSocket(True)
      RaiseEvent ConnectionStatus(Me, New SiteConnectionStatusEventArgs(SiteConnectionStates.TableDownload, SiteConnectionSubStates.LoadTable))
      Dim responseData As String = Nothing
      Dim responseURI As Uri = Nothing
      SendPOST(New Uri(tURI), sSend, responseURI, responseData, hdrs)
      If ClosingTime Then Return
      EX_Token_Response(responseData, responseURI)
    End Sub
    Private Sub EX_Token_Response(Response As String, ResponseURI As Uri)
      If CheckForErrors(Response, ResponseURI) Then Return
      Dim jTok As JSONReader
      Try
        Using jStream As New System.IO.MemoryStream(Text.Encoding.GetEncoding(srlFunctions.UTF_8).GetBytes(Response))
          jTok = New JSONReader(jStream, True)
        End Using
      Catch ex As Exception
        RaiseError("Could not log in.", "EX Token Response", Response, ResponseURI)
        Return
      End Try
      If jTok.JSON.GetType Is GetType(JSONFailure) Then
        RaiseError("Could not log in.", "EX Token Response", Response, ResponseURI)
        Return
      End If
      Try
        Dim assoc As Object = JSONAssociator.Associate(jTok)
        If Not assoc.ContainsKey("data") Then
          RaiseError("Could not log in.", "EX Token Response", Response, ResponseURI)
          Return
        End If
        If Not assoc("data").ContainsKey("getTokenUsingCode") Then
          RaiseError("Could not log in.", "EX Token Response", Response, ResponseURI)
          Return
        End If
        If Not assoc("data")("getTokenUsingCode").ContainsKey("accessToken") Then
          RaiseError("Could not log in.", "EX Token Response", Response, ResponseURI)
          Return
        End If
        Dim sToken As String = assoc("data")("getTokenUsingCode")("accessToken")
        If String.IsNullOrEmpty(sToken) Then
          RaiseError("Could not log in.", "EX Token Response", Response, ResponseURI)
          Return
        End If
        If justATest Then
          RaiseEvent LoginComplete(Me, New EventArgs)
          Return
        End If
        EX_Downlad_Table(sToken)
      Catch ex As Exception
        RaiseError("Could not log in.", "EX Token Response", Response, ResponseURI)
      End Try
    End Sub
    Private Sub EX_Downlad_Table(sToken As String)
      Dim tURI As String = "https://my-viasat.ts-usage.prod.icat.viasat.io/api/graphql"
      Dim aSend As New Dictionary(Of String, Object)
      aSend.Add("operationName", "getPlanData")
      aSend.Add("variables", New Dictionary(Of String, Object))
      aSend.Add("query", "query getPlanData($refetchData: Boolean) {\n" &
                         "  getPlanData(refetchData: $refetchData) {\n" &
                         "    accountStatus\n" &
                         "    usage {\n" &
                         "      monthly {\n" &
                         "        dataUsedGB\n" &
                         "        dataAllotmentGB\n" &
                         "        __typename\n" &
                         "      },\n" &
                         "      buymore {\n" &
                         "        dataUsedGB\n" &
                         "        dataAllotmentGB\n" &
                         "        __typename\n" &
                         "      }\n" &
                         "      __typename\n" &
                         "    }\n" &
                         "    __typename\n" &
                         "  }\n" &
                         "}\n")
      Dim sSend As String = New JSONObject(aSend).ToString
      Dim hdrs As New Net.WebHeaderCollection
      hdrs.Add(Net.HttpRequestHeader.ContentType, "application/json")
      hdrs.Add("x-auth-type", "Okta")
      hdrs.Add("x-auth-token", sToken)
      MakeSocket(True)
      RaiseEvent ConnectionStatus(Me, New SiteConnectionStatusEventArgs(SiteConnectionStates.TableDownload, SiteConnectionSubStates.LoadTable, 1))
      Dim responseData As String = Nothing
      Dim responseURI As Uri = Nothing
      SendPOST(New Uri(tURI), sSend, responseURI, responseData, hdrs)
      If ClosingTime Then Return
      If CheckForErrors(responseData, responseURI) Then Return
      ReadUsage(responseData)
    End Sub
    Private Sub EX_Read_Table(Table As String)
      RaiseEvent ConnectionStatus(Me, New SiteConnectionStatusEventArgs(SiteConnectionStates.TableRead))
      Dim exJS As JSONReader
      Try
        Using jStream As New System.IO.MemoryStream(Text.Encoding.GetEncoding(srlFunctions.UTF_8).GetBytes(Table))
          exJS = New JSONReader(jStream, True)
        End Using
      Catch ex As Exception
        RaiseError("Usage Failed: Could not parse usage meter table.", "EX Usage Response", Table)
        Return
      End Try
      If exJS.JSON.GetType Is GetType(JSONFailure) Then
        RaiseError("Usage Failed: Could not parse usage meter table.", "EX Usage Response", Table)
        Return
      End If
      Try
        Dim assoc As Object = JSONAssociator.Associate(exJS)
        Dim dDown As Double = 0.0, dDownT As Double = 0.0
        If assoc.ContainsKey("errors") AndAlso IsArray(assoc("errors")) Then
          For I As Integer = 0 To assoc("errors").Length - 1
            If assoc("errors")(I).ContainsKey("message") Then
              If assoc("errors")(I)("message") = "Internal server error" Then
                RaiseError("The server ran into an internal error. Please try again later.")
                Return
              End If
              If assoc("errors")(I)("message").Contains("received invalid response to SSL negotiation") Then
                RaiseError("The server ran into an internal error. Please try again later.")
                Return
              End If
              If assoc("errors")(I)("message").Contains("canceling statement due to statement timeout") Then
                RaiseError("The server ran into an internal error. Please try again later.")
                Return
              End If
              RaiseError("Usage Failed: " & assoc("errors")(I)("message"), "EX Usage Response", Table)
              Return
            End If
          Next
        End If
        If Not assoc.ContainsKey("data") Then
          RaiseError("Usage Failed: Could not parse usage meter table.", "EX Usage Response", Table)
          Return
        End If
        If Not assoc("data").ContainsKey("getPlanData") Then
          RaiseError("Usage Failed: Could not parse usage meter table.", "EX Usage Response", Table)
          Return
        End If
        If assoc("data")("getPlanData").ContainsKey("accountStatus") AndAlso Not String.IsNullOrEmpty(assoc("data")("getPlanData")("accountStatus")) Then
          Select Case assoc("data")("getPlanData")("accountStatus")
            Case "DISCONNECTED"
              RaiseError("Login Failed: Exede Account Disconnected. Check your username and password.")
              Return
            Case "NON-PAY"
              RaiseError("Login Failed: Exede Account Unpaid. Check your username and password.")
              Return
            Case "DEACTIVATED"
              RaiseError("Login Failed: Exede Account Inactive. Check your username and password.")
              Return
            Case "ACTIVE"

            Case Else
              'RaiseError("Login Failed: Exede Account in Unknown State: " & assoc("data")("getPlanData")("accountStatus") & ". Check your username and password.")
              'Return
          End Select
        End If
        If Not assoc("data")("getPlanData").ContainsKey("usage") Then
          RaiseError("Usage Failed: Could not parse usage meter table.", "EX Usage Response", Table)
          Return
        End If
        If Not assoc("data")("getPlanData")("usage").ContainsKey("monthly") Then
          RaiseError("Usage Failed: Could not parse usage meter table.", "EX Usage Response", Table)
          Return
        End If
        Dim jMonthly As Dictionary(Of String, Object) = assoc("data")("getPlanData")("usage")("monthly")
        If Not jMonthly.ContainsKey("dataAllotmentGB") OrElse String.IsNullOrEmpty(jMonthly("dataAllotmentGB").ToString) Then
          RaiseError("Usage Failed: Could not parse usage meter table.", "EX Usage Response", Table)
          Return
        End If
        If Not jMonthly.ContainsKey("dataUsedGB") OrElse String.IsNullOrEmpty(jMonthly("dataUsedGB").ToString) Then
          RaiseError("Usage Failed: Could not parse usage meter table.", "EX Usage Response", Table)
          Return
        End If
        dDown = CDbl(jMonthly("dataUsedGB"))
        dDownT = CDbl(jMonthly("dataAllotmentGB"))
        If assoc("data")("getPlanData")("usage").ContainsKey("buymore") Then
          Dim jBuyMore As Dictionary(Of String, Object) = assoc("data")("getPlanData")("usage")("buymore")
          If Not jBuyMore.ContainsKey("dataAllotmentGB") OrElse String.IsNullOrEmpty(jBuyMore("dataAllotmentGB").ToString) Then
            RaiseError("Usage Failed: Could not parse usage meter table.", "EX Usage Response", Table)
            Return
          End If
          If Not jBuyMore.ContainsKey("dataUsedGB") OrElse String.IsNullOrEmpty(jBuyMore("dataUsedGB").ToString) Then
            RaiseError("Usage Failed: Could not parse usage meter table.", "EX Usage Response", Table)
            Return
          End If
          dDown += CDbl(jBuyMore("dataUsedGB"))
          dDownT += CDbl(jBuyMore("dataAllotmentGB"))
        End If
        Dim sDown As String = dDown.ToString
        Dim sDownT As String = dDownT.ToString
        RaiseEvent ConnectionResult(Me, New SiteResultEventArgs(StrToVal(sDown, MBPerGB), StrToVal(sDownT, MBPerGB), Now, imSlowed, imFree))
      Catch ex As Exception
        RaiseError("Usage Failed: Could not parse usage meter table.", "EX Usage Response", Table)
      End Try
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
      wsSocket.SendCookieJar = c_SendJar
      wsSocket.Encoding = oldEncoding
      If Not ManualRedirect Then wsSocket.ManualRedirect = False
    End Sub
    Private Sub SendTLSProxy(SendURL As Uri, SendData As String, ByRef ReturnURL As Uri, ByRef ReturnData As String, Optional Headers As Net.WebHeaderCollection = Nothing)
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
                         (Not cookie.HttpOnly).ToString.ToLowerInvariant & vbTab &
                         cookie.Secure.ToString.ToLowerInvariant & vbTab &
                         cookie.Domain.StartsWith(".").ToString.ToLowerInvariant & vbTab &
                         cookie.Path & vbTab &
                         (cookie.Expires.ToUniversalTime - New DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds & vbTab &
                         cookie.Name & vbTab &
                         cookie.Value & vbLf
        Next
      End If
      Dim sPOST As String = Nothing
      sPOST &= "&url=" & srlFunctions.PercentEncode(ToBase64(SendURL.OriginalString))
      If SendData IsNot Nothing Then sPOST &= "&post=" & srlFunctions.PercentEncode(ToBase64(SendData))
      If Not String.IsNullOrEmpty(sCookieData) Then
        If sCookieData.EndsWith(vbLf) Then sCookieData = sCookieData.Substring(0, sCookieData.Length - 1)
        sPOST &= "&cookies=" & srlFunctions.PercentEncode(ToBase64(sCookieData))
      End If
      If Headers IsNot Nothing AndAlso Headers.Count > 0 Then
        Dim sHeaderData As String = ""
        For I As Integer = 0 To Headers.Count - 1
          For Each hVal In Headers.GetValues(I)
            sHeaderData &= Headers.GetKey(I) & ": " & hVal & vbLf
          Next
        Next
        If sHeaderData.EndsWith(vbLf) Then sHeaderData = sHeaderData.Substring(0, sHeaderData.Length - 1)
        sPOST &= "&headers=" & srlFunctions.PercentEncode(ToBase64(sHeaderData))
      End If
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
    Private Sub SendPOST(SendURL As Uri, SendData As String, ByRef ReturnURL As Uri, ByRef ReturnData As String, Optional Headers As Net.WebHeaderCollection = Nothing)
      If c_TLSProxy Then
        SendTLSProxy(SendURL, SendData, ReturnURL, ReturnData, Headers)
        Return
      End If
      wsSocket.SendHeaders.Add(Net.HttpRequestHeader.UserAgent, WebClientCore.UserAgent)
      If Headers IsNot Nothing AndAlso Headers.Count > 0 Then wsSocket.SendHeaders.Add(Headers)
      Dim sRet As String = wsSocket.UploadString(SendURL.OriginalString, "POST", SendData)
      ReturnData = sRet
      If wsSocket Is Nothing Then
        ReturnURL = Nothing
      Else
        ReturnURL = wsSocket.ResponseURI
      End If
    End Sub
    Private Shared Function FromBase64(base64Str As String) As String
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
    Private Shared Function ToBase64(regularStr As String) As String
      Dim bUTF() As Byte = System.Text.Encoding.UTF8.GetBytes(regularStr)
      Return Convert.ToBase64String(bUTF)
    End Function
    Private Function CheckForErrors(response As String, responseURI As Uri) As Boolean
      If String.IsNullOrEmpty(response) OrElse response = "Error: The server sent an empty response. Please try again." Then
        RaiseError("The server sent an empty response. Please try again.")
        Return True
      End If
      If response.StartsWith("Error: ") Then
        Dim sError As String = response.Substring(7)
        RaiseError(sError)
        Return True
      End If
      If response = "Connection timed out." Then
        RaiseEvent ConnectionFailure(Me, New SiteConnectionFailureEventArgs(SiteConnectionFailureType.ConnectionTimeout))
        Return True
      End If
      If response.ToUpperInvariant.Contains("504 GATEWAY TIME-OUT") Then
        RaiseEvent ConnectionFailure(Me, New SiteConnectionFailureEventArgs(SiteConnectionFailureType.ConnectionTimeout))
        Return True
      End If
      If response.StartsWith("Could not resolve host: ") Then
        RaiseError("The server is unavailable. Please try again later.")
        Return True
      End If
      If response.ToUpperInvariant.Contains("INTERNAL SERVER ERROR") Or (response.ToUpperInvariant.Contains("INTERNAL ERROR") And response.Contains("500")) Then
        RaiseError("The server ran into an internal error. Please try again later.")
        Return True
      End If
      If response.Contains("Apache Tomcat") And response.Contains("Error report") Then
        Dim sError As String = Nothing
        If response.Contains("<b>description</b>") Then
          sError = response.Substring(response.IndexOf("<b>description</b>"))
          If sError.Contains("<u>") Then
            sError = sError.Substring(sError.IndexOf("<u>") + 3)
            sError = sError.Substring(0, sError.IndexOf("</u>"))
          Else
            sError = Nothing
          End If
        End If
        If String.IsNullOrEmpty(sError) Then
          RaiseError("The server ran into an unknown error (Tomcat). Please try again later.", "Check For Errors", "Unknown Tomcat Error: " & response, responseURI)
        Else
          If Not sError.EndsWith(".") Then sError &= "."
          RaiseError("The server ran into an error (Tomcat): " & sError & " Please try again later.")
        End If
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
      RaiseEvent ConnectionFailure(Me, New SiteConnectionFailureEventArgs(SiteConnectionFailureType.LoginFailure, ErrorMessage))
    End Sub
    Private Sub RaiseError(ErrorMessage As String, FailureLocation As String, FailureData As String, Optional FailureAddress As Uri = Nothing)
      If String.IsNullOrEmpty(Trim(ErrorMessage)) Then Return
      Dim FailureText As String
      If FailureAddress Is Nothing Then
        FailureText = "Error at " & FailureLocation & ": """ & ErrorMessage & """" & vbNewLine & "Data: " & vbNewLine & FailureData
      Else
        FailureText = "Error at " & FailureLocation & ": """ & ErrorMessage & """" & vbNewLine & "URL: {" & FailureAddress.OriginalString & "}" & vbNewLine & "Data: " & vbNewLine & FailureData
      End If
      RaiseEvent ConnectionFailure(Me, New SiteConnectionFailureEventArgs(SiteConnectionFailureType.LoginFailure, ErrorMessage, FailureText))
    End Sub
    Private Shared Function StrToVal(str As String, Optional vMult As Integer = 1) As Long
      If String.IsNullOrEmpty(str) Then Return 0
      If Not str.Contains(" ") Then Return CLng(Val(str.Replace(",", "")) * vMult)
      Return CLng(Val(str.Substring(0, str.IndexOf(" ")).Replace(",", "")) * vMult)
    End Function
    Private Function IgnoreCert(sender As Object, certificate As System.Security.Cryptography.X509Certificates.X509Certificate, chain As System.Security.Cryptography.X509Certificates.X509Chain, errors As Net.Security.SslPolicyErrors) As Boolean
      Return True
    End Function
#End Region
#Region "IDisposable Support"
    Private disposedValue As Boolean
    Protected Overridable Sub Dispose(disposing As Boolean)
      If Not Me.disposedValue Then
        If disposing Then
          ClosingTime = True
          If wsSocket IsNot Nothing Then
            If wsSocket.IsBusy Then wsSocket.Cancel()
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
      Dispose(False)
      MyBase.Finalize()
    End Sub
  End Class
End Namespace
