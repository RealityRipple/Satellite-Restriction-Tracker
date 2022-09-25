''' <summary>
''' Accesses WildBlue, Exede, RuralPortal, and Dish usage pages and handles all communication internally.
''' </summary>
Public Class localRestrictionTracker
  Implements IDisposable
  ''' <summary>
  ''' Types of Satellite hosts which are supported by <see cref="localRestrictionTracker" />.
  ''' </summary>
  Public Enum SatHostTypes
    ''' <summary>
    ''' WildBlue type meter with <see cref="localRestrictionTracker.TYPEAResultEventArgs">Type A</see> usage, including Download, Download Limit, Upload, and Upload Limit values, reading through an address of myaccount.DOMAIN/wbisp/DOMAIN/.
    ''' </summary>
    WildBlue_LEGACY    ' Down, Up, DownLim, UpLim     [TYPEA]
    ''' <summary>
    ''' Exede type meter with <see cref="localRestrictionTracker.TYPEBResultEventArgs">Type B</see> usage, including only Used and Limit values, reading through my.exede.net.
    ''' </summary>
    WildBlue_EXEDE     ' Used, Limit                  [TYPEB]
    ''' <summary>
    ''' Exede type meter with <see cref="localRestrictionTracker.TYPEBResultEventArgs">Type B</see> usage, including only Used and Limit values, reading through my.satelliteinternetco.com, using AJAX.
    ''' </summary>
    WildBlue_EXEDE_RESELLER
    ''' <summary>
    ''' RuralPortal WildBlue type meter with <see cref="localRestrictionTracker.TYPEAResultEventArgs">Type A</see> usage, including Download, Download Limit, Upload, and Upload Limit values, reading through an address of DOMAIN.ruralportal.net.
    ''' </summary>
    RuralPortal_LEGACY ' Down, Up, DownLim, UpLim     [TYPEA]
    ''' <summary>
    ''' RuralPortal Exede type meter with <see cref="localRestrictionTracker.TYPEBResultEventArgs">Type B</see> usage, including only Used and Limit values, reading through an address of DOMAIN.ruralportal.net.
    ''' </summary>
    RuralPortal_EXEDE  ' Used, Limit                  [TYPEB]
    ''' <summary>
    ''' Exede through Dish Network type meter with <see cref="localRestrictionTracker.TYPEA2ResultEventArgs">Type A2</see> uage, including AnyTime, AnyTime Limit, Off-Peak, and Off-Peak Limit values, reading through mydish.com.
    ''' </summary>
    Dish_EXEDE         ' AnyTime, AnyTimeLim, OffPeak, OffPeakLim [TYPEA2]
    ''' <summary>
    ''' The provider is unknown or hasn't been determined yet.
    ''' </summary>
    Other
  End Enum
  ''' <summary>
  ''' Current status of the <see cref="localRestrictionTracker" /> connection.
  ''' </summary>
  Public Enum ConnectionStates
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
  ''' Current stage of the <see cref="localRestrictionTracker" /> connection (more detailed than <see cref="ConnectionStates" />).
  ''' </summary>
  Public Enum ConnectionSubStates
    ''' <summary>
    ''' Use the <see cref="ConnectionStates" /> value instead of this value. There's no further breakdown of status.
    ''' </summary>
    None
    ''' <summary>
    ''' The login page is being read. Exede and Exede Reseller use this step.
    ''' </summary>
    ReadLogin
    ''' <summary>
    ''' The username and password are being sent. All providers use this step.
    ''' </summary>
    Authenticate
    ''' <summary>
    ''' The login is being verified. Only Dish uses this step.
    ''' </summary>
    Verify
    ''' <summary>
    ''' The home page is being loaded. This is also known as the Dashboard. Exede and Dish use this step.
    ''' </summary>
    LoadHome
    ''' <summary>
    ''' The Code parameter from the home page URL is being loaded. Only Exede uses this step.
    ''' </summary>
    LoadAJAX
    ''' <summary>
    ''' The previous attempt to load AJAX data failed, so now the system is loading every page of AJAX data it can in an attempt to find the meter. Only Exede Reseller uses this step.
    ''' </summary>
    LoadTable
  End Enum
#Region "Events"
  ''' <summary>
  ''' Information regarding the type of <see cref="localRestrictionTracker" /> connection failure received and any details.
  ''' </summary>
  Public Class ConnectionFailureEventArgs
    Inherits EventArgs
    ''' <summary>
    ''' Types of connection failures.
    ''' </summary>
    Public Enum FailureType
      ''' <summary>
      ''' The Account Type is unknown, so the <see cref="localRestrictionTracker" /> connection can not begin. There should be no <see cref="ConnectionFailureEventArgs.Message" /> for this <see cref="ConnectionFailureEventArgs" />.
      ''' </summary>
      UnknownAccountType
      ''' <summary>
      ''' The Username or Password are missing, so the <see cref="localRestrictionTracker" /> connection can not begin. There should be no <see cref="ConnectionFailureEventArgs.Message" /> for this <see cref="ConnectionFailureEventArgs" />.
      ''' </summary>
      UnknownAccountDetails
      ''' <summary>
      ''' There was an issue while logging in, but the login process is still going through. Currently the only issue triggered is from <see cref="DetermineType.SatHostGroup.RuralPortal" /> with the <see cref="ConnectionFailureEventArgs.Message" /> "Your password needs to be changed." when the server is prompting the user to change the password for one reason or another.
      ''' </summary>
      LoginIssue
      ''' <summary>
      ''' There was an issue while logging in. The <see cref="ConnectionFailureEventArgs.Message" /> will contain information about the <see cref="ConnectionFailure" />.
      ''' </summary>
      LoginFailure
      ''' <summary>
      ''' There was an issue while logging in, and the <see cref="AppSettings.AccountType" /> is wrong. The <see cref="ConnectionFailureEventArgs.Message" /> will contain information about the <see cref="ConnectionFailure" />.
      ''' </summary>
      FatalLoginFailure
      ''' <summary>
      ''' The server timed out. There should be no <see cref="ConnectionFailureEventArgs.Message" /> for this <see cref="ConnectionFailureEventArgs" />.
      ''' </summary>
      ConnectionTimeout
      ''' <summary>
      ''' The version of TLS is too old. The <see cref="ConnectionFailureEventArgs.Message" /> for this <see cref="ConnectionFailureEventArgs" /> will either be the string "VER" if TLS 1.1 and 1.2 are both disabled, or "PROXY" if the TLS Proxy is disabled.
      ''' </summary>
      TLSTooOld
    End Enum
    Private m_FailType As FailureType
    Private m_Fail As String
    Private m_Message As String
    ''' <summary>
    ''' Constructor for a <see cref="ConnectionFailureEventArgs" /> class, used in a <see cref="ConnectionFailure" /> event to notify about a problem during connection.
    ''' </summary>
    ''' <param name="ftFailType">The <see cref="FailureType" /> describes the type of <see cref="ConnectionFailure" /> for this event. See the description for each type for details.</param>
    ''' <param name="sMessage">A message containing information about the <see cref="ConnectionFailure" />. Not all <see cref="FailureType">FailureTypes</see> use messages.</param>
    ''' <param name="sFailure">Extra data passed to the program about the <see cref="ConnectionFailure" />, which can be reported to the RealityRipple Software servers to resolve in the next version. This is usually HTML and JavaScript from a single page of the login process or the meter.</param>
    Public Sub New(ftFailType As FailureType, Optional sMessage As String = Nothing, Optional sFailure As String = Nothing)
      m_FailType = ftFailType
      m_Message = sMessage
      m_Fail = sFailure
    End Sub
    ''' <summary>
    ''' Extra data passed to the program about the <see cref="ConnectionFailure" />, which can be reported to the RealityRipple Software servers to resolve in the next version. This is usually HTML and JavaScript from a single page of the login process or the meter.
    ''' </summary>
    Public ReadOnly Property Fail As String
      Get
        Return m_Fail
      End Get
    End Property
    ''' <summary>
    ''' A message containing information about the <see cref="ConnectionFailure" />. Not all <see cref="FailureType">FailureTypes</see> use messages.
    ''' </summary>
    Public ReadOnly Property Message As String
      Get
        Return m_Message
      End Get
    End Property
    ''' <summary>
    ''' The <see cref="FailureType" /> describes the type of <see cref="ConnectionFailure" /> for this event. See the description for each type for details.
    ''' </summary>
    Public ReadOnly Property [Type] As FailureType
      Get
        Return m_FailType
      End Get
    End Property
  End Class
  ''' <summary>
  ''' Triggered when the connection to a usage page fails or has an issue.
  ''' </summary>
  ''' <param name="sender">Instance of the <see cref="localRestrictionTracker" /> class.</param>
  ''' <param name="e"><see cref="ConnectionFailureEventArgs" /> data regarding the failure.</param>
  Public Event ConnectionFailure(sender As Object, e As ConnectionFailureEventArgs)
  ''' <summary>
  ''' Result from a Type A usage meter, containing Download, Download Limit, Upload, and Upload Limit values.
  ''' </summary>
  Public Class TYPEAResultEventArgs
    Inherits EventArgs
    Private m_Down As Long
    Private m_Up As Long
    Private m_DownLim As Long
    Private m_UpLim As Long
    Private m_Update As Date
    Private m_slow As Boolean
    Private m_free As Boolean
    ''' <summary>
    ''' Constructor for a <see cref="TYPEAResultEventArgs" /> used in <see cref="ConnectionWBLResult" /> and <see cref="ConnectionRPLResult" /> events.
    ''' </summary>
    ''' <param name="lDown">Number of megabytes used in download.</param>
    ''' <param name="lDownLim">Number of megabytes allowed in download.</param>
    ''' <param name="lUp">Number of megabytes used in upload.</param>
    ''' <param name="lUpLim">Number of megabytes allowed in upload.</param>
    ''' <param name="dUpdate">The specific date and time of this usage.</param>
    ''' <param name="bSlow"><c>True</c> if the connection has been reported as restricted, <c>False</c> otherwise.</param>
    ''' <param name="bFree"><c>True</c> if usage is reported not to count at the moment, <c>False</c> under normal conditions.</param>
    Public Sub New(lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long, dUpdate As Date, bSlow As Boolean, bFree As Boolean)
      m_Down = lDown
      m_Up = lUp
      m_DownLim = lDownLim
      m_UpLim = lUpLim
      m_Update = dUpdate
      m_slow = bSlow
      m_free = bFree
    End Sub
    ''' <summary>
    ''' Number of megabytes used in download.
    ''' </summary>
    Public ReadOnly Property Download As Long
      Get
        Return m_Down
      End Get
    End Property
    ''' <summary>
    ''' Number of megabytes allowed in download.
    ''' </summary>
    Public ReadOnly Property DownloadLimit As Long
      Get
        Return m_DownLim
      End Get
    End Property
    ''' <summary>
    ''' Number of megabytes used in upload.
    ''' </summary>
    Public ReadOnly Property Upload As Long
      Get
        Return m_Up
      End Get
    End Property
    ''' <summary>
    ''' Number of megabytes allowed in upload.
    ''' </summary>
    Public ReadOnly Property UploadLimit As Long
      Get
        Return m_UpLim
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
    ''' <c>True</c> if the connection has been reported as restricted, <c>False</c> otherwise.
    ''' </summary>
    Public ReadOnly Property SlowedDetected As Boolean
      Get
        Return m_slow
      End Get
    End Property
    ''' <summary>
    ''' <c>True</c> if usage is reported not to count at the moment, <c>False</c> under normal conditions.
    ''' </summary>
    Public ReadOnly Property FreeDetected As Boolean
      Get
        Return m_free
      End Get
    End Property
  End Class
  ''' <summary>
  ''' Result from Type A2 usage meter, containing AnyTime, AnyTime Limit, Off-Peak, and Off-Peak Limit values.
  ''' </summary>
  Public Class TYPEA2ResultEventArgs
    Inherits EventArgs
    Private m_AnyTime As Long
    Private m_AnyTimeLim As Long
    Private m_OffPeak As Long
    Private m_OffPeakLim As Long
    Private m_Update As Date
    Private m_slow As Boolean
    Private m_free As Boolean
    ''' <summary>
    ''' Constructor for a <see cref="TYPEA2ResultEventArgs" /> used in <see cref="ConnectionDNXResult" /> events.
    ''' </summary>
    ''' <param name="lAnyTime">Number of megabytes used during AnyTime hours.</param>
    ''' <param name="lAnyTimeLim">Number of megabytes allowed during AnyTime hours.</param>
    ''' <param name="lOffPeak">Number of megabytes used during Off-Peak hours.</param>
    ''' <param name="lOffPeakLim">Number of megabytes allowed during Off-Peak hours.</param>
    ''' <param name="dUpdate">The specific date and time of this usage.</param>
    ''' <param name="bSlow"><c>True</c> if the connection has been reported as restricted, <c>False</c> otherwise.</param>
    ''' <param name="bFree"><c>True</c> if usage is reported not to count at the moment, <c>False</c> under normal conditions.</param>
    Public Sub New(lAnyTime As Long, lAnyTimeLim As Long, lOffPeak As Long, lOffPeakLim As Long, dUpdate As Date, bSlow As Boolean, bFree As Boolean)
      m_AnyTime = lAnyTime
      m_AnyTimeLim = lAnyTimeLim
      m_OffPeak = lOffPeak
      m_OffPeakLim = lOffPeakLim
      m_Update = dUpdate
      m_slow = bSlow
      m_free = bFree
    End Sub
    ''' <summary>
    ''' Number of megabytes used during AnyTime hours.
    ''' </summary>
    Public ReadOnly Property AnyTime As Long
      Get
        Return m_AnyTime
      End Get
    End Property
    ''' <summary>
    ''' Number of megabytes allowed during AnyTime hours.
    ''' </summary>
    Public ReadOnly Property AnyTimeLimit As Long
      Get
        Return m_AnyTimeLim
      End Get
    End Property
    ''' <summary>
    ''' Number of megabytes used during Off-Peak hours.
    ''' </summary>
    Public ReadOnly Property OffPeak As Long
      Get
        Return m_OffPeak
      End Get
    End Property
    ''' <summary>
    ''' Number of megabytes allowed during Off-Peak hours.
    ''' </summary>
    Public ReadOnly Property OffPeakLimit As Long
      Get
        Return m_OffPeakLim
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
    ''' <c>True</c> if the connection has been reported as restricted, <c>False</c> otherwise.
    ''' </summary>
    Public ReadOnly Property SlowedDetected As Boolean
      Get
        Return m_slow
      End Get
    End Property
    ''' <summary>
    ''' <c>True</c> if usage is reported not to count at the moment, <c>False</c> under normal conditions.
    ''' </summary>
    Public ReadOnly Property FreeDetected As Boolean
      Get
        Return m_free
      End Get
    End Property
  End Class
  ''' <summary>
  ''' Result from Type B usage meter, containing Used and Limit values.
  ''' </summary>
  Public Class TYPEBResultEventArgs
    Inherits EventArgs
    Private m_Used As Long
    Private m_Limit As Long
    Private m_Update As Date
    Private m_slow As Boolean
    Private m_free As Boolean
    ''' <summary>
    ''' Constructor for a <see cref="TYPEBResultEventArgs" /> used in <see cref="ConnectionWBXResult" />, <see cref="ConnectionWXRResult" />, and <see cref="ConnectionRPXResult" /> events.
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
    ''' If your connection is restricted, this value will be set to <c>True</c>. Currently only set by <see cref="SatHostTypes.WildBlue_LEGACY" />.
    ''' </summary>
    Public ReadOnly Property SlowedDetected As Boolean
      Get
        Return m_slow
      End Get
    End Property
    ''' <summary>
    ''' If the usage isn't being counted, this value will be set to <c>True</c>. Currently rarely used by <see cref="SatHostTypes.WildBlue_LEGACY" />.
    ''' </summary>
    Public ReadOnly Property FreeDetected As Boolean
      Get
        Return m_free
      End Get
    End Property
  End Class
  ''' <summary>
  ''' Triggered when the server returns data for a <see cref="SatHostTypes.WildBlue_LEGACY" /> account.
  ''' </summary>
  ''' <param name="sender">Instance of the <see cref="localRestrictionTracker" /> class.</param>
  ''' <param name="e"><see cref="TYPEAResultEventArgs" /> data regarding the result.</param>
  Public Event ConnectionWBLResult(sender As Object, e As TYPEAResultEventArgs)
  ''' <summary>
  ''' Triggered when the server returns data for a <see cref="SatHostTypes.WildBlue_EXEDE" /> account.
  ''' </summary>
  ''' <param name="sender">Instance of the <see cref="localRestrictionTracker" /> class.</param>
  ''' <param name="e"><see cref="TYPEBResultEventArgs" /> data regarding the result.</param>
  Public Event ConnectionWBXResult(sender As Object, e As TYPEBResultEventArgs)
  ''' <summary>
  ''' Triggered when the server returns data for a <see cref="SatHostTypes.WildBlue_EXEDE_RESELLER" /> account.
  ''' </summary>
  ''' <param name="sender">Instance of the <see cref="localRestrictionTracker" /> class.</param>
  ''' <param name="e"><see cref="TYPEBResultEventArgs" /> data regarding the result.</param>
  Public Event ConnectionWXRResult(sender As Object, e As TYPEBResultEventArgs)
  ''' <summary>
  ''' Triggered when the server returns data for a <see cref="SatHostTypes.Dish_EXEDE" /> account.
  ''' </summary>
  ''' <param name="sender">Instance of the <see cref="localRestrictionTracker" /> class.</param>
  ''' <param name="e"><see cref="TYPEA2ResultEventArgs" /> data regarding the result.</param>
  Public Event ConnectionDNXResult(sender As Object, e As TYPEA2ResultEventArgs)
  ''' <summary>
  ''' Triggered when the server returns data for a <see cref="SatHostTypes.RuralPortal_LEGACY" /> account.
  ''' </summary>
  ''' <param name="sender">Instance of the <see cref="localRestrictionTracker" /> class.</param>
  ''' <param name="e"><see cref="TYPEAResultEventArgs" /> data regarding the result.</param>
  Public Event ConnectionRPLResult(sender As Object, e As TYPEAResultEventArgs)
  ''' <summary>
  ''' Triggered when the server returns data for a <see cref="SatHostTypes.RuralPortal_EXEDE" /> account.
  ''' </summary>
  ''' <param name="sender">Instance of the <see cref="localRestrictionTracker" /> class.</param>
  ''' <param name="e"><see cref="TYPEBResultEventArgs" /> data regarding the result.</param>
  Public Event ConnectionRPXResult(sender As Object, e As TYPEBResultEventArgs)
  ''' <summary>
  ''' Class storing information regarding the current connection status, useful for displaying progress during connection or determining the location of an error.
  ''' </summary>
  Public Class ConnectionStatusEventArgs
    Inherits EventArgs
    Private m_state As ConnectionStates
    Private m_substate As ConnectionSubStates
    Private m_stage As Integer
    Private m_attempt As Integer
    ''' <summary>
    ''' Constructor for <see cref="ConnectionStatusEventArgs" /> class.
    ''' </summary>
    ''' <param name="status">Current status of the <see cref="localRestrictionTracker" /> connection.</param>
    ''' <param name="substate">Current stage of the <see cref="localRestrictionTracker" /> connection (more detailed than <see cref="ConnectionStates" />). This value is <see cref="ConnectionSubStates.None" /> by default.</param>
    ''' <param name="stage">The stage of the stage, so to speak. This numeric value contains detailed information on <paramref name="substate" /> values which may trigger more than one time.</param>
    ''' <param name="attempt">The number of retries the current connection is on. Retries are normal during connection, as part of the redirection process.</param>
    Public Sub New(status As ConnectionStates, Optional substate As ConnectionSubStates = ConnectionSubStates.None, Optional stage As Integer = 0, Optional attempt As Integer = 0)
      m_state = status
      m_substate = substate
      m_stage = stage
      m_attempt = attempt
    End Sub
    ''' <summary>
    ''' Current status of the <see cref="localRestrictionTracker" /> connection.
    ''' </summary>
    Public ReadOnly Property Status As ConnectionStates
      Get
        Return m_state
      End Get
    End Property
    ''' <summary>
    ''' Current stage of the <see cref="localRestrictionTracker" /> connection (more detailed than <see cref="ConnectionStates" />).
    ''' </summary>
    Public ReadOnly Property SubState As ConnectionSubStates
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
  ''' Triggered when new information regarding the current connection status is available.
  ''' </summary>
  ''' <param name="sender">Instance of the <see cref="localRestrictionTracker" /> class.</param>
  ''' <param name="e"><see cref="ConnectionStatusEventArgs" /> data regarding the current state of the connection.</param>
  Public Event ConnectionStatus(sender As Object, e As ConnectionStatusEventArgs)
  ''' <summary>
  ''' Class storing information regarding a successful connection without data, which contains the <see cref="SatHostTypes">SatHostType</see> for the connected provider.
  ''' </summary>
  Public Class LoginCompletionEventArgs
    Inherits EventArgs
    Private m_HostType As SatHostTypes
    ''' <summary>
    ''' Constructor for the <see cref="LoginCompletionEventArgs" /> class.
    ''' </summary>
    ''' <param name="myHostType">The type of host for this provider.</param>
    Public Sub New(myHostType As SatHostTypes)
      m_HostType = myHostType
    End Sub
    ''' <summary>
    ''' The type of host for this provider.
    ''' </summary>
    Public ReadOnly Property HostType As SatHostTypes
      Get
        Return m_HostType
      End Get
    End Property
  End Class
  ''' <summary>
  ''' Triggered when the server verifies that the account's login information is correct.
  ''' </summary>
  ''' <param name="sender">Instance of the <see cref="localRestrictionTracker" /> class.</param>
  ''' <param name="e"><see cref="LoginCompletionEventArgs" /> data regarding the <see cref="SatHostTypes">SatHostType</see> of the account.</param>
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
  Private AttemptedTry As Integer
  Private imSlowed As Boolean
  Private imFree As Boolean
  Private FullCheck As Boolean = True
  Private ClosingTime As Boolean
  Private tConnect As Threading.Thread
  Private c_Timeout As Integer
  Private c_Proxy As Net.IWebProxy
  Private c_Jar As Net.CookieContainer
  Private c_SendJar As Boolean
  Private c_TLSProxy As Boolean
  Private c_Protocol As Net.SecurityProtocolType
  Private c_TLSProxyAddr As String
  Private sDataPath As String
  Private wsSocket As WebClientEx
#Region "Initialization Functions"
  ''' <summary>
  ''' Constructor for the <see cref="localRestrictionTracker" /> class, which also begins the connection process.
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
    sAccount = mySettings.Account
    If Not String.IsNullOrEmpty(mySettings.PassCrypt) Then
      If String.IsNullOrEmpty(mySettings.PassKey) Or String.IsNullOrEmpty(mySettings.PassSalt) Then
        sPassword = StoredPasswordLegacy.DecryptApp(mySettings.PassCrypt)
      Else
        sPassword = StoredPassword.Decrypt(mySettings.PassCrypt, mySettings.PassKey, mySettings.PassSalt)
      End If
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
      c_TLSProxyAddr = "http://wb.realityripple.com/tls.php"
    End If
    c_Timeout = mySettings.Timeout
    c_Proxy = mySettings.Proxy
    Dim sFramework As String = srlFunctions.GetCLRCleanVersion
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
      Case DetermineType.SatHostGroup.Dish
        mySettings.AccountType = SatHostTypes.Dish_EXEDE
        GetUsage()
      Case DetermineType.SatHostGroup.RuralPortal
        mySettings.AccountType = SatHostTypes.RuralPortal_EXEDE
        GetUsage()
      Case DetermineType.SatHostGroup.Exede
        mySettings.AccountType = SatHostTypes.WildBlue_EXEDE
        GetUsage()
      Case DetermineType.SatHostGroup.ExedeReseller
        mySettings.AccountType = SatHostTypes.WildBlue_EXEDE_RESELLER
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
        If sProvider.ToLower = "exede.net" Then
          LoginExede()
        ElseIf sProvider.ToLower = "satelliteinternetco.com" Then
          LoginExedeR()
        Else
          LoginWB()
        End If
      Case SatHostTypes.WildBlue_EXEDE_RESELLER : LoginExedeR()
      Case SatHostTypes.RuralPortal_LEGACY, SatHostTypes.RuralPortal_EXEDE : LoginRP()
      Case SatHostTypes.Dish_EXEDE : LoginDN()
    End Select
  End Sub
  Private Sub LoginWB()
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Prepare))
    Dim uriString As String = String.Format(sWB, IIf(sProvider.ToLower = "exede.com", "exede.net", sProvider), "servLogin", IIf(sProvider.ToLower = "exede.net", "exede.com", sProvider))
    MakeSocket(False)
    Dim sSend As String = "uid=" & srlFunctions.PercentEncode(sUsername) & "&userPassword=" & srlFunctions.PercentEncode(sPassword)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, 0, uriString)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(uriString), sSend, responseURI, responseData)
    If ClosingTime Then Return
    WB_Login_Response(responseData, responseURI)
  End Sub
  Private Sub LoginExede()
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Prepare))
    Dim uriString As String = "https://my-viasat.ts-usage.prod.icat.viasat.io/auth"
    EX_Init(uriString)
  End Sub
  Private Sub LoginExedeR()
    mySettings.AccountType = SatHostTypes.WildBlue_EXEDE_RESELLER
    AJAXOrder = mySettings.AJAXShortOrder
    AJAXFullOrder = mySettings.AJAXFullOrder
    If AJAXOrder Is Nothing Or AJAXFullOrder Is Nothing Then
      RaiseError("Can't determine AJAX order.")
      Return
    End If
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Initialize))
    Dim uriString As String = "https://my." & sProvider & "/"
    MakeSocket(False)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.ReadLogin, 0, 0, uriString)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendGET(New Uri(uriString), responseURI, responseData)
    If ClosingTime Then Return
    ER_Login_Prepare_Response(responseData, responseURI, 0)
  End Sub
  Private Sub LoginRP()
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Prepare))
    If sProvider.Contains(".") Then sProvider = sProvider.Substring(0, sProvider.LastIndexOf("."))
    Dim uriString As String = String.Format(sRP, sProvider, "login")
    MakeSocket(False)
    Dim sSend As String = "warningTrip=false&userName=" & srlFunctions.PercentEncode(sUsername) & "&passwd=" & srlFunctions.PercentEncode(sPassword)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, 0, uriString)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(uriString), sSend, responseURI, responseData)
    If ClosingTime Then Return
    RP_Login_Response(responseData, responseURI, False)
  End Sub
  Private Sub LoginDN()
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.Prepare))
    Dim uriString As String = "https://www.mydish.com/auth/login.ashx"
    MakeSocket(False)
    Dim bpf As String = "U"
    bpf &= TimeZone.CurrentTimeZone.GetUtcOffset(New Date(2010, 12, 30)).TotalMinutes
    bpf &= My.Computer.Info.InstalledUICulture.Name.Substring(My.Computer.Info.InstalledUICulture.Name.Length - 2)
    bpf &= "qN|YMDHS|"
    Dim myID As String = WebClientCore.UserAgent & My.Computer.Screen.WorkingArea.Width & My.Computer.Screen.WorkingArea.Height & My.Computer.Info.OSPlatform & "x86"
    bpf &= DNHash(myID)
    Dim sSend As String = "action=loginuser&onlineid=" & srlFunctions.PercentEncode(sUsername) & "&pw=" & srlFunctions.PercentEncode(sPassword) & "&bfp=" & bpf & "&reCaptcha="
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.None, 0, 0, uriString)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(uriString), sSend, responseURI, responseData)
    If ClosingTime Then Return
    DN_Login_Response(responseData, responseURI)
  End Sub
  Private Function DNHash(inStr As String) As String
    If String.IsNullOrEmpty(inStr) Then Return 0
    Dim a As Integer = 0
    For I As Integer = 0 To inStr.Length - 1
      Dim lVal As Long = a << 5
      lVal = lVal - a
      lVal = lVal + AscW(inStr(I))
      If lVal > Integer.MaxValue Then
        lVal -= &HFFFFFFFFUL
      ElseIf lVal < Integer.MinValue Then
        lVal += &HFFFFFFFFUL
      End If
      a = lVal
    Next
    Dim sA As String = a.ToString
    If sA.Length > 12 Then sA = sA.Substring(0, 12)
    If sA(0) = "-" Then sA = "A" & sA.Substring(1)
    Return sA
  End Function
#End Region
#Region "Parsing Functions"
  Private Sub ReadUsage(Table As String)
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.TableRead))
    Select Case mySettings.AccountType
      Case SatHostTypes.WildBlue_LEGACY : WB_Read_Table(Table)
      Case SatHostTypes.WildBlue_EXEDE
        If sProvider.ToLower = "exede.net" Then
          EX_Read_Table(Table)
        ElseIf sProvider.ToLower = "satelliteinternetco.com" Then
          ER_Read_Table(Table)
        Else
          WB_Read_Table(Table)
        End If
      Case SatHostTypes.WildBlue_EXEDE_RESELLER : ER_Read_Table(Table)
      Case SatHostTypes.RuralPortal_LEGACY, SatHostTypes.RuralPortal_EXEDE : RP_Read_Table(Table)
      Case SatHostTypes.Dish_EXEDE : DN_Read_Table(Table)
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
    Dim uriString As String = String.Format(sWB, IIf(sProvider.ToLower = "exede.com", "exede.net", sProvider), File, IIf(sProvider.ToLower = "exede.net", "exede.com", sProvider))
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTable, 0, 0, uriString)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendGET(New Uri(uriString), responseURI, responseData)
    If ClosingTime Then Return
    WB_Usage_Response(responseData, responseURI)
  End Sub
  Private Sub WB_Usage_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "myaccount." & IIf(sProvider.ToLower = "exede.com", "exede.net", sProvider.ToLower) Then
      RaiseError("Usage Failed: Connection redirected to """ & srlFunctions.TruncateURL(ResponseURI.OriginalString) & """, check your Internet connection.")
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
        ProviderSurvey("WBL")
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
        ProviderSurvey("WBX")
        RaiseEvent ConnectionWBXResult(Me, New TYPEBResultEventArgs(StrToVal(sDown, MBPerGB), StrToVal(sDownT, MBPerGB) + StrToVal(sPlusT, MBPerGB), Now, imSlowed, imFree))
      End If
    Else
      RaiseError("Usage Read Failed: Unable to locate data table!", "WB Read Table", Table)
    End If
  End Sub
#End Region
#Region "EX"
#Region "Exede Helper Functions"
  Private Function EX_Helper_FindBetween(find As String, groupS As String, groupE As String, sepS As Char, sepE As Char) As String()
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
  Private Function EX_Helper_WithinParens(find As String) As String
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
  Private Function EX_Helper_Unescape(d As String)
    For I As Integer = 0 To 255
      Dim h As String = srlFunctions.PadHex(I, 2).ToUpper
      Dim c As Char = ChrW(I)
      d = d.Replace("\x" & h, c)
    Next
    Return d
  End Function
  Private Function EX_Helper_Parse_okta(body As String) As Dictionary(Of String, String)
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
    If Not jOkta.Serial.Count = 1 Then Return ret
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
  Private Function EX_Helper_Parse_scope(body As String) As String
    Dim ret As String = "openid profle email offline_access"
    Dim scopeData As String() = EX_Helper_FindBetween(body, "c=[", "]", """"c, """"c)
    If scopeData IsNot Nothing Then ret = Join(scopeData)
    Return ret
  End Function
  Private Function EX_Helper_Parse_urlBuilder(body As String) As Dictionary(Of String, String)
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
  Private Function EX_Helper_MakeLoginFromStruct(struct As Object, valList As Dictionary(Of String, String)) As Dictionary(Of String, Object)
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

  Private Sub EX_Init(sURI As String)
    MakeSocket(True)
    BeginAttempt(ConnectionStates.Initialize, ConnectionSubStates.None, 0, 0, sURI)
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
    BeginAttempt(ConnectionStates.Prepare, ConnectionSubStates.None, 0, 0, sURI.OriginalString)
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
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.ReadLogin, 0, 0, sURI.OriginalString)
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
    If Not jData.Serial.Count = 1 Then
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
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.None, 0, 0, sURI.OriginalString)
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
    If Not exJS.Serial.Count = 1 Then
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
      Dim sBody As String = JSONAssociator.MakeString(body)
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
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, 0, sURI.OriginalString)
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
    If Not jsAuth.Serial.Count = 1 Then
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
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadHome, 0, 0, sURI.OriginalString)
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
    Dim sSend As String = JSONAssociator.MakeString(aSend)
    Dim hdrs As New Net.WebHeaderCollection
    hdrs.Add(Net.HttpRequestHeader.ContentType, "application/json")
    MakeSocket(True)
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTable, 0, 0, tURI)
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
    If Not jTok.Serial.Count = 1 Then
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
                       "    isUnlimited\n" &
                       "    usage {\n" &
                       "      dataLeftText\n" &
                       "      dataUsedGB\n" &
                       "      dataCapGB\n" &
                       "      __typename\n" &
                       "    }\n" &
                       "    __typename\n" &
                       "  }\n" &
                       "}\n")
    Dim sSend As String = JSONAssociator.MakeString(aSend)
    Dim hdrs As New Net.WebHeaderCollection
    hdrs.Add(Net.HttpRequestHeader.ContentType, "application/json")
    hdrs.Add("x-auth-type", "Okta")
    hdrs.Add("x-auth-token", sToken)
    MakeSocket(True)
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTable, 1, 0, tURI)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(tURI), sSend, responseURI, responseData, hdrs)
    If ClosingTime Then Return
    If CheckForErrors(responseData, responseURI) Then Return
    ReadUsage(responseData)
  End Sub
  Private Sub EX_Read_Table(Table As String)
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(ConnectionStates.TableRead))
    Dim exJS As JSONReader
    Try
      Using jStream As New System.IO.MemoryStream(Text.Encoding.GetEncoding(srlFunctions.UTF_8).GetBytes(Table))
        exJS = New JSONReader(jStream, True)
      End Using
    Catch ex As Exception
      RaiseError("Usage Failed: Could not parse usage meter table.", "EX Usage Response", Table)
      Return
    End Try
    If Not exJS.Serial.Count = 1 Then
      RaiseError("Usage Failed: Could not parse usage meter table.", "EX Usage Response", Table)
      Return
    End If
    Try
      Dim assoc As Object = JSONAssociator.Associate(exJS)
      Dim sDown As String = String.Empty, sDownT As String = String.Empty
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
      If assoc("data")("getPlanData").ContainsKey("isUnlimited") AndAlso assoc("data")("getPlanData")("isUnlimited") = "true" Then imFree = True
      If Not assoc("data")("getPlanData").ContainsKey("usage") Then
        RaiseError("Usage Failed: Could not parse usage meter table.", "EX Usage Response", Table)
        Return
      End If
      Dim jUsage As Dictionary(Of String, Object) = assoc("data")("getPlanData")("usage")
      If Not assoc("data")("getPlanData")("usage").ContainsKey("dataCapGB") OrElse String.IsNullOrEmpty(assoc("data")("getPlanData")("usage")("dataCapGB")) Then
        RaiseError("Usage Failed: Could not parse usage meter table.", "EX Usage Response", Table)
        Return
      End If
      If Not assoc("data")("getPlanData")("usage").ContainsKey("dataUsedGB") OrElse String.IsNullOrEmpty(assoc("data")("getPlanData")("usage")("dataUsedGB")) Then
        RaiseError("Usage Failed: Could not parse usage meter table.", "EX Usage Response", Table)
        Return
      End If
      If assoc("data")("getPlanData")("usage").ContainsKey("dataLeftText") AndAlso assoc("data")("getPlanData")("usage")("dataLeftText") = "NONE" Then imSlowed = True
      sDown = assoc("data")("getPlanData")("usage")("dataUsedGB")
      sDownT = assoc("data")("getPlanData")("usage")("dataCapGB")
      ProviderSurvey("VIA")
      RaiseEvent ConnectionWBXResult(Me, New TYPEBResultEventArgs(StrToVal(sDown, MBPerGB), StrToVal(sDownT, MBPerGB), Now, imSlowed, imFree))
    Catch ex As Exception
      RaiseError("Usage Failed: Could not parse usage meter table.", "EX Usage Response", Table)
    End Try
  End Sub
#End Region
#Region "ER"
  Private Sub ER_Login_Prepare_Response(Response As String, ResponseURI As Uri, TryCount As Integer)
    If Response.Contains("To access this website, update your web browser Or upgrade your operating system to support TLS 1.1 Or TLS 1.2.") Or Response.Contains("Stronger security Is required") Or Response = "Error: The server requires a specific SSL/TLS version. Please check your Network Security settings in the Configuration." Then
      If (c_Protocol And SecurityProtocolTypeEx.Tls11) = SecurityProtocolTypeEx.Tls11 Or (c_Protocol And SecurityProtocolTypeEx.Tls12) = SecurityProtocolTypeEx.Tls12 Or (c_Protocol And SecurityProtocolTypeEx.Tls13) = SecurityProtocolTypeEx.Tls13 Then
        If c_TLSProxy Then
          RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "TLS Proxy failed to be of any use!"))
          Return
        End If
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.TLSTooOld, "PROXY"))
        Return
      End If
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.TLSTooOld, "VER"))
      Return
    End If
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "mysso." & sProvider And Not ResponseURI.Host.ToLower = "my." & sProvider Then
      RaiseError("Prepare Failed: Connection redirected to """ & srlFunctions.TruncateURL(ResponseURI.OriginalString) & """, check your Internet connection.")
      Return
    End If
    If Response.ToLower.Contains("unable to process request") Then
      RaiseError("Prepare Failed: The server may be down.")
      Return
    End If
    If Response.ToLower.Contains(" down for maintenance") Then
      RaiseError("Prepare Failed: Server Down for Maintenance.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower = "/federation/ssoredirect/metaalias/wsubscriber/idp" Then
      If Response.Contains("location.href") Then
        Dim sRedirURI As String = Response.Substring(Response.IndexOf("location.href"))
        sRedirURI = sRedirURI.Substring(sRedirURI.IndexOf("'") + 1)
        sRedirURI = sRedirURI.Substring(0, sRedirURI.IndexOf("'"))
        If sRedirURI = "/" Then
          sRedirURI = ResponseURI.OriginalString.Substring(0, ResponseURI.OriginalString.IndexOf("/", ResponseURI.OriginalString.IndexOf("//") + 2))
        ElseIf sRedirURI.StartsWith("/") Then
          sRedirURI = "https://" & ResponseURI.Host & sRedirURI
        End If
        Dim response2Data As String = Nothing
        Dim response2URI As Uri = Nothing
        SendGET(New Uri(sRedirURI), response2URI, response2Data)
        If ClosingTime Then Return
        ER_Login_Prepare_Response(response2Data, response2URI, TryCount)
      Else
        RaiseError("Prepare Failed: Could not understand response.", "ER Login Prepare Response", Response, ResponseURI)
      End If
      Return
    End If
    If Not Response.Contains("<form") Or Not Response.Contains("name=""Login""") Then
      RaiseError("Prepare Failed: Login form not found.", "ER Login Prepare Response", Response, ResponseURI)
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
      RaiseError("Prepare Failed: GOTO value not found.", "ER Login Prepare Response", Response, ResponseURI)
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
      RaiseError("Prepare Failed: SunQueryParamsString value not found.", "ER Login Prepare Response", Response, ResponseURI)
      Return
    End If
    ER_Login(sURI, sGOTO, sSQPS, TryCount)
  End Sub
  Private Sub ER_Login(sURI As String, sGOTO As String, sSQPS As String, TryCount As Integer)
    MakeSocket(True)
    Dim sSend As String = "realm=" & srlFunctions.PercentEncode("wsubscriber") &
                         "&IDToken1=" & srlFunctions.PercentEncode(sUsername) &
                         "&IDToken2=" & srlFunctions.PercentEncode(sPassword) &
                         "&IDButton=Sign+in" &
                         "&goto=" & srlFunctions.PercentEncode(sGOTO) &
                         "&SunQueryParamsString=" & srlFunctions.PercentEncode(sSQPS) &
                         "&encoded=true" &
                         "&gx_charset=UTF-8"
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, TryCount, sURI)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(sURI), sSend, responseURI, responseData)
    If ClosingTime Then Return
    ER_Login_Response(responseData, responseURI, TryCount)
  End Sub
  Private Sub ER_Login2(sURI As String, TryCount As Integer)
    MakeSocket(True)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 1, TryCount, sURI)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendGET(New Uri(sURI), responseURI, responseData)
    If ClosingTime Then Return
    ER_Login_Response(responseData, responseURI, TryCount)
  End Sub
  Private Sub ER_Login_Response(Response As String, ResponseURI As Uri, TryCount As Integer)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "mysso." & sProvider Then
      RaiseError("Login Failed: Connection redirected to """ & srlFunctions.TruncateURL(ResponseURI.OriginalString) & """, check your Internet connection.")
      Return
    End If
    If Response.Contains("login-error-alert") Then
      If Response.ToLower.Contains("your username and/or password are incorrect.") Then
        RaiseError("Login Failed: Incorrect Password")
      ElseIf Response.ToLower.Contains("your account has been locked due to excessive failed log in attempts.") Then
        RaiseError("Login Failed: Exede Reseller Account Locked. Check your username and password.")
      ElseIf Response.ToLower.Contains("your session has timed out.") Then
        RaiseError("Login Failed: Session timed out. Please try again.")
      ElseIf Response.ToLower.Contains("this user is not active.") Then
        RaiseError("Login Failed: Exede Reseller Account Inactive. Check your username and password.")
      Else
        RaiseError("Unknown Login Error.", "ER Login Response", Response, ResponseURI)
      End If
      Return
    ElseIf Response.Contains("<div class=""msgerror"">") Then
      If Response.ToLower.Contains("invalid user name or password") Then
        RaiseError("Login Failed: Incorrect Password")
      Else
        RaiseError("Unknown Login Error.", "ER Login Response", Response, ResponseURI)
      End If
      Return
    ElseIf Response.ToLower.Contains("sorry, we've encountered an unexpected error.") Then
      RaiseError("Login Failed: Server encountered an unexpected error.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower = "/federation/ui/login" Then
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
        BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, TryCount, sRedirURI)
        Dim response2Data As String = Nothing
        Dim response2URI As Uri = Nothing
        SendGET(New Uri(sRedirURI), response2URI, response2Data)
        If ClosingTime Then Return
        ER_Login_Response(response2Data, response2URI, TryCount)
      ElseIf Response.Contains("maintenance") Then
        RaiseError("Login Failed: Server Down for Maintenance.")
      ElseIf Response.Contains("https://DOMAIN.my.salesforce.com") Then
        RaiseError("Login Failed: Server Down for Maintenance.")
      ElseIf Response.Contains("<input type=""hidden"" name=""goto"" value="""" />") Then
        RaiseError("Login Failed: Please check your account information and try again.")
      Else
        RaiseError("Login Failed: Could not understand response.", True, "ER Login Response", Response, ResponseURI)
      End If
      Return
    End If
    If Response.Contains("Access rights validated") Then
      Dim sURI As String = Nothing
      If Response.Contains("<form method=""post"" action=""") Then
        sURI = Response.Substring(Response.IndexOf("<form method=""post"" action="""))
        sURI = sURI.Substring(sURI.IndexOf("action=""") + 8)
        If sURI.Contains(""">") Then
          sURI = sURI.Substring(0, sURI.IndexOf(""">"))
        Else
          RaiseError("Login Failed: POST URL value cut off. Please try again.", "ER Login Response")
          Return
        End If
        sURI = srlFunctions.HexDecode(sURI)
      End If
      If String.IsNullOrEmpty(sURI) Then sURI = ResponseURI.AbsoluteUri
      Dim sSAMLResponse As String = Nothing
      If Response.Contains("<input type=""hidden"" name=""SAMLResponse"" value=""") Then
        sSAMLResponse = Response.Substring(Response.IndexOf("<input type=""hidden"" name=""SAMLResponse"" value="""))
        sSAMLResponse = sSAMLResponse.Substring(sSAMLResponse.IndexOf("value=""") + 7)
        If sSAMLResponse.Contains(""" />") Then
          sSAMLResponse = sSAMLResponse.Substring(0, sSAMLResponse.IndexOf(""" />"))
        Else
          RaiseError("Login Failed: SAML Response value cut off. Please try again.", "ER Login Response")
          Return
        End If
      End If
      If String.IsNullOrEmpty(sSAMLResponse) Then
        RaiseError("Login Failed: SAML Response value not found.", "ER Login Response", Response, ResponseURI)
        Return
      End If
      Dim sRelay As String = Nothing
      If Response.Contains("<input type=""hidden"" name=""RelayState"" value=""") Then
        sRelay = Response.Substring(Response.IndexOf("<input type=""hidden"" name=""RelayState"" value="""))
        sRelay = sRelay.Substring(sRelay.IndexOf("value=""") + 7)
        If sRelay.Contains(""" />") Then
          sRelay = sRelay.Substring(0, sRelay.IndexOf(""" />"))
        Else
          RaiseError("Login Failed: Relay State value cut off. Please try again.", "ER Login Response")
          Return
        End If
      End If
      If String.IsNullOrEmpty(sRelay) Then
        TryCount += 1
        ER_Login2(sURI, TryCount)
        Return
      End If
      ER_Authenticate(sURI, sSAMLResponse, sRelay)
    ElseIf Response.Contains("<input type=""hidden"" name=""goto"" value="""" />") Then
      RaiseError("Login Failed: Please check your account information and try again.")
    ElseIf Response.Contains("<input type=""hidden"" name=""goto"" value=""") Then
      TryCount += 1
      If TryCount > 15 Then
        RaiseError("Login Failed: Server redirected too many times.")
        Return
      End If
      ER_Login_Prepare_Response(Response, ResponseURI, TryCount)
    Else
      RaiseError("Could not log in.", "ER Login Response", Response, ResponseURI)
    End If
  End Sub
  Private Sub ER_Authenticate(sURI As String, SAMLResponse As String, RelayState As String)
    MakeSocket(True)
    Dim sSend As String = "SAMLResponse=" & srlFunctions.PercentEncode(srlFunctions.HexDecode(SAMLResponse))
    If Not String.IsNullOrEmpty(RelayState) Then sSend &= "&RelayState=" & srlFunctions.PercentEncode(srlFunctions.HexDecode(RelayState))
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadHome, 0, 0, sURI)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(sURI), sSend, responseURI, responseData)
    If ClosingTime Then Return
    ER_Authenticate_Response(responseData, responseURI)
  End Sub
  Private Sub ER_Authenticate_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "my." & sProvider Then
      RaiseError("Authentication Failed: Connection redirected to """ & srlFunctions.TruncateURL(ResponseURI.OriginalString) & """, check your Internet connection.")
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
        If sProvider = "satelliteinternetco.com" Then
          sURL = "https://" & ResponseURI.Host & "/subscriber_dashboard"
        Else
          RaiseError("Authentication Failed: Unknown Provider - Can't determine Dashboard URL.")
        End If
      End If
      If justATest Then
        RaiseEvent LoginComplete(Me, New LoginCompletionEventArgs(SatHostTypes.WildBlue_EXEDE_RESELLER))
        Return
      End If
      ER_Download_Homepage(sURL)
      Return
    End If
    If Response.Contains("maintenance") Then
      RaiseError("Authentication Failed: Server Down for Maintenance.")
    Else
      RaiseError("Authentication Failed: Could not understand response.", True, "ER Authenticate Response", Response, ResponseURI)
    End If
  End Sub
  Private Structure AjaxEntry
    Public ID As String
    Public Iteration As Byte
    Public Index As Integer
    Public Sub New(iIdx As Integer, sID As String, bI As Byte)
      Index = iIdx
      ID = sID
      Iteration = bI
    End Sub
  End Structure
  Private AJAXFullOrder() As String
  Private AJAXOrder() As String
  Public ReadOnly Property ExedeResellerAJAXFirstTryRequests As Integer
    Get
      Return AJAXOrder.Length - 1
    End Get
  End Property
  Public ReadOnly Property ExedeResellerAJAXSecondTryRequests As Integer
    Get
      Return AJAXFullOrder.Length - 1
    End Get
  End Property
  Private Sub ER_Download_Homepage(sURI As String)
    MakeSocket(True)
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadAJAX, 1, 0, sURI)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendGET(New Uri(sURI), responseURI, responseData)
    If ClosingTime Then Return
    ER_Ajax_Response(responseData, responseURI, New AjaxEntry(0, AJAXOrder(0), 1))
  End Sub
  Private Sub ER_Ajax_Response(Response As String, ResponseURI As Uri, NextAjaxID As AjaxEntry)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "my." & sProvider Then
      RaiseError("AJAX Load Failed: Connection redirected to """ & srlFunctions.TruncateURL(ResponseURI.OriginalString) & """, check your Internet connection.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower = "/subscriber_dashboard" Then
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
        ER_Download_Homepage(sURL)
      ElseIf Response.Contains("maintenance") Then
        RaiseError("AJAX Load Failed: Server Down for Maintenance.")
      Else
        RaiseError("AJAX Load Failed: Could not understand response.", True, "ER Ajax Response", Response, ResponseURI)
      End If
      Return
    End If
    If Response.Contains("amount-used") Then
      If Response.Contains("green red") Then imSlowed = True
      Dim sTable As String = Response.Substring(Response.LastIndexOf("<div class=""amount-used"""))
      sTable = sTable.Substring(0, sTable.IndexOf("</p>") + 4)
      ReadUsage(sTable)
    ElseIf Response.Contains("<strong>Unable to load Usage information.<br /> Please try again later.</strong>") Then
      RaiseError("Usage Failed: Data temporarily unavailable.")
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
      If sProvider = "satelliteinternetco.com" Then
        sURL = "https://" & ResponseURI.Host & "/subscriber_dashboard?refURL=https%3A%2F%2F" & ResponseURI.Host & "%2Fsubscriber_dashboard"
      Else
        RaiseError("AJAX Load Failed: Unknown Provider - Can't determine Dashboard URL.")
      End If
      ER_Download_Ajax(sURL, NextAjaxID, sViewState, sVSVersion, sVSMAC, sVSCSRF)
    ElseIf Response.Contains("https://myexede.force.com/atlasPlanInvalid") Or Response.Contains("https://my." & sProvider & "/atlasPlanInvalid") Then
      RaiseError("AJAX Load Failed: You no longer have access to MyExede. Please check back again or contact Customer Care [(855) 463-9333] if the problem persists.")
    ElseIf Response.Contains("Concurrent requests limit exceeded.") Then
      RaiseError("AJAX Load Failed: Too many requests. Check for usage data less often.")
    ElseIf Response.Contains("maintenance") Then
      RaiseError("AJAX Load Failed: Server Down for Maintenance.")
    ElseIf Response.Contains("window.location.href") Then
      RaiseError("AJAX Load Failed: Sent back to login page.")
    ElseIf Response.Contains("Something went wrong.") Then
      RaiseError("AJAX Load Failed: Server Error - Exede may be having trouble.")
    Else
      RaiseError("AJAX Load Failed: Could not find AJAX ViewState variables.", "ER Ajax Response", Response, ResponseURI)
    End If
  End Sub
  Private Sub ER_Download_Ajax(sURI As String, AjaxID As AjaxEntry, sViewState As String, sVSVersion As String, sVSMAC As String, sVSCSRF As String)
    MakeSocket(True)
    Dim newIDX As Integer = AjaxID.Index
    Dim newID As String = AjaxID.ID
    Dim newType As Byte = AjaxID.Iteration
    If (AjaxID.Iteration = 1 And AjaxID.Index = ExedeResellerAJAXFirstTryRequests) Or (AjaxID.Iteration > 1 And AjaxID.Index = ExedeResellerAJAXSecondTryRequests) Then
      BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTable, 0, 0, sURI)
      newIDX = 0
      newID = AJAXFullOrder(0)
      newType += 1
    ElseIf AjaxID.Iteration = 1 Then
      Dim bShown As Boolean = False
      For I As Integer = 0 To AJAXOrder.Length - 1
        If AjaxID.ID = AJAXOrder(I) Then
          BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadAJAX, I + 1, 0, sURI)
          newIDX = I + 1
          newID = AJAXOrder(I + 1)
          bShown = True
          Exit For
        End If
      Next
      If Not bShown Then
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Unknown AJAX ID: " & AjaxID.ID & "."))
        Return
      End If
    ElseIf AjaxID.Iteration < 4 Then
      Dim bShown As Boolean = False
      For I As Integer = 0 To AJAXFullOrder.Length - 1
        If AjaxID.ID = AJAXFullOrder(I) Then
          BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadAJAX, I + 1, 1, sURI)
          newIDX = I + 1
          newID = AJAXFullOrder(I + 1)
          bShown = True
          Exit For
        End If
      Next
      If Not bShown Then
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Unknown AJAX ID: " & AjaxID.ID & "."))
        Return
      End If
    Else
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "AJAX failed to yield data table."))
      Return
    End If
    Dim AJTable As String = AjaxID.ID.Substring(0, AjaxID.ID.LastIndexOf(":"))
    Dim sSend As String =
      "AJAXREQUEST=_viewRoot" &
      "&" & srlFunctions.PercentEncode(AJTable) & "=" & srlFunctions.PercentEncode(AJTable) &
      "&com.salesforce.visualforce.ViewState=" & srlFunctions.PercentEncode(sViewState) &
      "&com.salesforce.visualforce.ViewStateVersion=" & srlFunctions.PercentEncode(sVSVersion) &
      "&com.salesforce.visualforce.ViewStateMAC=" & srlFunctions.PercentEncode(sVSMAC) &
      "&com.salesforce.visualforce.ViewStateCSRF=" & srlFunctions.PercentEncode(sVSCSRF) &
      "&" & srlFunctions.PercentEncode(AjaxID.ID) & "=" & srlFunctions.PercentEncode(AjaxID.ID)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(sURI), sSend, responseURI, responseData)
    If ClosingTime Then Return
    ER_Ajax_Response(responseData, responseURI, New AjaxEntry(newIDX, newID, newType))
  End Sub
  Private Sub ER_Read_Table(Table As String)
    Dim Used As String = Nothing
    Dim Total As String = Nothing
    If Table.Contains("amount-used") Then
      Used = Table.Substring(Table.IndexOf("amount-used"))
      Used = Used.Substring(Used.IndexOf(""">") + 2)
      Used = Used.Substring(0, Used.IndexOf("</"))
      Total = Nothing
      If Table.Contains("<strong>") Then
        Total = Table.Substring(Table.IndexOf("<strong>") + 8)
        If Total.Contains("</") Then
          Total = Total.Substring(0, Total.IndexOf("</"))
        Else
          RaiseError("Usage Read Failed: Unable to parse Total!", "ER Read Table", Table)
          Return
        End If
      End If
    Else
      RaiseError("Usage Read Failed: Unable to locate data table", "ER Read Table", Table)
      Return
    End If
    Dim lUsed As Long = StrToVal(Used, MBPerGB)
    Dim lTotal As Long = StrToVal(Total, MBPerGB)
    If lTotal = 0 Then
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Data temporarily unavailable."))
    Else
      ProviderSurvey("WXR")
      RaiseEvent ConnectionWXRResult(Me, New TYPEBResultEventArgs(lUsed, lTotal, Now, imSlowed, imFree))
    End If
  End Sub
#End Region
#Region "RP"
  Private Sub RP_Login_Retry(sURI As String)
    MakeSocket(False)
    Dim sUser As String = sAccount.Substring(0, sAccount.LastIndexOf("@"))
    Dim sSend As String = "warningTrip=true&userName=" & sUser & "&passwd=" & srlFunctions.PercentEncode(sPassword)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, 1, sURI)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(sURI), sSend, responseURI, responseData)
    If ClosingTime Then Return
    RP_Login_Response(responseData, responseURI, True)
  End Sub
  Private Sub RP_Login_Response(Response As String, ResponseURI As Uri, Retry As Boolean)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = sProvider & ".ruralportal.net" Then
      RaiseError("Login Failed: Connection redirected to """ & srlFunctions.TruncateURL(ResponseURI.OriginalString) & """, check your Internet connection.")
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
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginIssue, "Your password needs to be changed."))
      If sProvider.Contains(".") Then sProvider = sProvider.Substring(0, sProvider.LastIndexOf("."))
      Dim uriString As String = String.Format(sRP, sProvider, "login")
      RP_Login_Retry(uriString)
    End If
  End Sub
  Private Sub RP_Usage(File As String)
    If sProvider.Contains(".") Then sProvider = sProvider.Substring(0, sProvider.LastIndexOf("."))
    Dim uriString As String = String.Format(sRP, sProvider, File)
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTable, 0, 0, uriString)
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
      RaiseError("Usage Failed: Connection redirected to """ & srlFunctions.TruncateURL(ResponseURI.OriginalString) & """, check your Internet connection.")
      Return
    End If
    If Not Response.Contains("Current Usage") Then
      If Response.Contains("<table width=""100%"" class=""tabpane"" id=""infoTable"" style=""display:"" >") Then
        Dim sResponseTable As String = Response.Substring(Response.IndexOf("<table width=""100%"" class=""tabpane"" id=""infoTable"" style=""display:"" >") + 69).Trim
        If String.IsNullOrEmpty(sResponseTable) OrElse sResponseTable.StartsWith("</form>") Then
          RaiseError("Usage Failed: Data temporarily unavailable.")
        Else
          RaiseError("Usage Failed: Could not find usage meter", True, "RP Usage Response", Response, ResponseURI)
        End If
      Else
        RaiseError("Usage Failed: Failed to log in.", True, "RP Usage Response", Response, ResponseURI)
      End If
      Return
    End If
    If Response.Contains("Usage data is not available.") Then
      RaiseError("Usage Failed: Data temporarily unavailable.")
      Return
    End If
    If Not Response.Contains("<!-- Start Usage Bar -->") Then
      RaiseError("Usage Failed: Could not find usage meter.", "RP Usage Response", Response, ResponseURI)
      Return
    End If
    Dim sFind As String = Response.Substring(Response.IndexOf("<!-- Start Usage Bar -->"))
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
    Table = Replace(Table, vbCr, "")
    Table = Replace(Table, vbLf, "")
    Table = Replace(Table, vbTab, " ")
    Do While Table.Contains("  ")
      Table = Replace(Table, "  ", " ")
    Loop
    Dim CTag As Boolean = False
    For I As Integer = 0 To Table.Length - 1
      If Table(I) = "<" Then
        CTag = False
      ElseIf Table(I) = "/" Then
        CTag = True
      ElseIf Table(I) = ">" And CTag Then
        Table = Table.Insert(I + 1, vbLf)
        CTag = False
      End If
    Next
    Table = Replace(Table, "><", ">" & vbLf & "<")
    Table = Replace(Table, "> <", ">" & vbLf & "<")
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
        ProviderSurvey("RPL")
        RaiseEvent ConnectionRPLResult(Me, New TYPEAResultEventArgs(StrToVal(sDown), StrToVal(sDownT), StrToVal(sUp), StrToVal(sUpT), Now, imSlowed, imFree))
      End If
    ElseIf Table.Contains(" GB (") Then
      If justATest Then
        RaiseEvent LoginComplete(Me, New LoginCompletionEventArgs(SatHostTypes.RuralPortal_EXEDE))
        Return
      End If
      Dim sBuyMore As String = "0"
      Dim sBuyMoreT As String = "0"
      If Table.Contains("Within Normal Usage") Then
        imSlowed = False
      ElseIf Table.Contains("Approaching Package Threshold") Or Table.Contains("Exceeded DAP Threshold") Then
        imSlowed = True
      ElseIf Table.Contains("Using Buy More") Then
        sBuyMore = ""
        sBuyMoreT = ""
      Else
        RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginIssue, "Unknown usage state message. Requesting page source code for assistance improving the next version...", "Issue at RP Read Table: ""Unknown usage state message.""" & vbNewLine & "Data: " & vbNewLine & Table))
      End If
      Dim sRows As String() = Split(Table, vbLf)
      Dim sDown As String = String.Empty, sDownT As String = String.Empty, sOverhead As String = String.Empty
      For Each row In sRows
        If Not String.IsNullOrEmpty(row) Then
          If row.Contains(" GB of ") And row.Contains(" GB (") And row.Contains("%)") Then
            If String.IsNullOrEmpty(sDown) And String.IsNullOrEmpty(sDownT) Then
              sDown = row.Substring(0, row.IndexOf(" of ")).Trim
              If sDown.Contains(">") Then sDown = sDown.Substring(sDown.LastIndexOf(">") + 1)
              sDownT = row.Substring(row.IndexOf(" of ") + 4)
              sDownT = sDownT.Substring(0, sDownT.IndexOf(" ("))
              If Not Table.Contains("Breach:") Then Exit For
            ElseIf String.IsNullOrEmpty(sBuyMore) And String.IsNullOrEmpty(sBuyMoreT) Then
              sBuyMore = row.Substring(0, row.IndexOf(" of ")).Trim
              If sBuyMore.Contains(">") Then sBuyMore = sBuyMore.Substring(sBuyMore.LastIndexOf(">") + 1)
              sBuyMoreT = row.Substring(row.IndexOf(" of ") + 4)
              sBuyMoreT = sBuyMoreT.Substring(0, sBuyMoreT.IndexOf(" ("))
              Exit For
            End If
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
        ProviderSurvey("RPX")
        RaiseEvent ConnectionRPXResult(Me, New TYPEBResultEventArgs(StrToVal(sDown, MBPerGB) + StrToVal(sOverhead, MBPerGB) + StrToVal(sBuyMore, MBPerGB), StrToVal(sDownT, MBPerGB) + StrToVal(sBuyMoreT, MBPerGB), Now, imSlowed, imFree))
      End If
    Else
      RaiseError("Usage Read Failed: Unable to locate data table!", "RP Read Table", Table)
    End If
  End Sub
#End Region
#Region "DN"
  Private Sub DN_Login_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "www.mydish.com" Then
      RaiseError("Login Failed: Connection redirected to """ & srlFunctions.TruncateURL(ResponseURI.OriginalString) & """, check your Internet connection.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower.Contains("login.ashx") Then
      RaiseError("Login Failed: Could not understand response.", True, "DN Login Response", Response, ResponseURI)
      Return
    End If
    If Not Response.Contains("""Success"":true") Then
      Dim sFail As String = Response.Substring(Response.IndexOf("""DisplayMessage"":""") + 18)
      sFail = sFail.Substring(0, sFail.IndexOf(""","""))
      RaiseError("Login Failed: " & sFail)
      Return
    End If
    DN_Login_Continue("https://www.mydish.com/auth/saml/login.aspx?relaystate=" & srlFunctions.PercentEncode("/usermanagement/processMyDishResponse.do"))
  End Sub
  Private Sub DN_Login_Continue(sURI As String)
    MakeSocket(False)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Authenticate, 0, 0, sURI)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendGET(New Uri(sURI), responseURI, responseData)
    If ClosingTime Then Return
    DN_Login_Continue_Response(responseData, responseURI)
  End Sub
  Private Sub DN_Login_Continue_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "www.mydish.com" Then
      RaiseError("Login Failed: Connection redirected to """ & srlFunctions.TruncateURL(ResponseURI.OriginalString) & """, check your Internet connection.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower.Contains("login.aspx") Then
      RaiseError("Login Failed: Could not understand response.", True, "DN Login Continue Response", Response, ResponseURI)
      Return
    End If
    If Response.Contains("SAMLResponse"" value=""") Then
      Dim SAMLResponse As String
      SAMLResponse = Response.Substring(Response.IndexOf("SAMLResponse"" value=""") + 21)
      If SAMLResponse.Contains("""/>") Then
        SAMLResponse = SAMLResponse.Substring(0, SAMLResponse.IndexOf("""/>"))
      Else
        RaiseError("Login Failed: Incomplete SAML Response Data.", "DN Login Continue Response", Response, ResponseURI)
        Return
      End If
      Dim RelayState As String
      RelayState = Response.Substring(Response.IndexOf("RelayState"" value=""") + 19)
      If RelayState.Contains("""/>") Then
        RelayState = RelayState.Substring(0, RelayState.IndexOf("""/>"))
      Else
        RaiseError("Login Failed: Incomplete Relay State Data.", "DN Login Continue Response", Response, ResponseURI)
        Return
      End If
      DN_Login_Verify(SAMLResponse, RelayState)
    ElseIf Response.Contains("The system is currently unavailable. Please try again later.") Then
      RaiseError("System currently unavailable.")
    ElseIf Response.Contains("<div class=""custom_message_text"">") Then
      Dim sErrMsg As String = Response.Substring(Response.IndexOf("<div class=""custom_message_text"">") + 33)
      sErrMsg = sErrMsg.Substring(0, sErrMsg.IndexOf("<"))
      RaiseError(sErrMsg.Trim)
    Else
      RaiseError("Login Failed: No SAML Response", "DN Login Continue Response", Response, ResponseURI)
    End If
  End Sub
  Private Sub DN_Login_Verify(SAMLResponse As String, RelayState As String)
    MakeSocket(False)
    Dim uriString As String = "https://my.dish.com/customercare/saml/post"
    Dim sSend As String = "SAMLResponse=" & srlFunctions.PercentEncode(SAMLResponse) & "&RelayState=" & srlFunctions.PercentEncode(RelayState)
    BeginAttempt(ConnectionStates.Login, ConnectionSubStates.Verify, 0, 0, uriString)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(uriString), sSend, responseURI, responseData)
    If ClosingTime Then Return
    DN_Login_Verify_Response(responseData, responseURI, RelayState)
  End Sub
  Private Sub DN_Login_Verify_Response(Response As String, ResponseURI As Uri, ExpectedURI As String)
    If CheckForErrors(Response, ResponseURI, True) Then Return
    If Not ResponseURI.Host.ToLower = "my.dish.com" Then
      RaiseError("Login Failed: Connection redirected to """ & srlFunctions.TruncateURL(ResponseURI.OriginalString) & """, check your Internet connection.")
      Return
    End If
    If Not ResponseURI.OriginalString.ToLower.Contains(ExpectedURI.ToLower) Then
      RaiseError("Login Failed: Could not understand response.", True, "Dish Login Verify Response", Response, ResponseURI)
      Return
    End If
    If justATest Then
      RaiseEvent LoginComplete(Me, New LoginCompletionEventArgs(SatHostTypes.Dish_EXEDE))
      Return
    End If
    DN_Download_Home()
  End Sub
  Private Sub DN_Download_Home()
    MakeSocket(False)
    Dim uriString As String = "https://my.dish.com/customercare/usermanagement/getAccountNumberByUUID.do"
    Dim sSend As String = "check="
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadHome, 0, 0, uriString)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendPOST(New Uri(uriString), sSend, responseURI, responseData)
    If ClosingTime Then Return
    DN_Download_Home_Response(responseData, responseURI)
  End Sub
  Private Sub DN_Download_Home_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI, True) Then Return
    If Not ResponseURI.Host.ToLower = "my.dish.com" Then
      RaiseError("Login Failed: Connection redirected to """ & srlFunctions.TruncateURL(ResponseURI.OriginalString) & """, check your Internet connection.")
      Return
    End If
    If Response.Contains("The requested URL was rejected.") Then
      RaiseError("Login Failed: The server rejected the request.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower.Contains("accountsummary") Then
      RaiseError("Home Read Failed: Could not load home page. Redirected to """ & srlFunctions.TruncateURL(ResponseURI.OriginalString) & """.", "DN Download Home Response", Response, ResponseURI)
      Return
    End If
    DN_Download_Table()
  End Sub
  Private Sub DN_Download_Table()
    MakeSocket(False)
    Dim uriString As String = "https://my.dish.com/customercare/myaccount/myinternet"
    BeginAttempt(ConnectionStates.TableDownload, ConnectionSubStates.LoadTable, 0, 0, uriString)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendGET(New Uri(uriString), responseURI, responseData)
    If ClosingTime Then Return
    DN_Download_Table_Response(responseData, responseURI)
  End Sub
  Private Sub DN_Download_Table_Response(Response As String, ResponseURI As Uri)
    If CheckForErrors(Response, ResponseURI) Then Return
    If Not ResponseURI.Host.ToLower = "my.dish.com" Then
      RaiseError("Usage Failed: Connection redirected to """ & srlFunctions.TruncateURL(ResponseURI.OriginalString) & """, check your Internet connection.")
      Return
    End If
    If Response.Contains("The requested URL was rejected.") Then
      RaiseError("Usage Failed: The server rejected the request.")
      Return
    End If
    If Not ResponseURI.AbsolutePath.ToLower.Contains("internet") Then
      RaiseError("Usage Failed: Could not load usage meter page. Redirected to """ & srlFunctions.TruncateURL(ResponseURI.OriginalString) & """.", "DN Download Table Response", Response, ResponseURI)
      Return
    End If
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
          If wLine.Contains("'/customercare/widgets/loadMeter.do?'") Then
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
            If opV.Contains("ATTRIBUTE ERROR E11") Then opV = Nothing
            opM = jsLines("maxValueAttr")
            If opM.Contains("ATTRIBUTE ERROR E11") Then opM = Nothing
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
            If atV.Contains("ATTRIBUTE ERROR E11") Then atV = Nothing
            atM = jsLines("maxValueAttr")
            If atM.Contains("ATTRIBUTE ERROR E11") Then atM = Nothing
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
            If atxV.Contains("ATTRIBUTE ERROR E11") Then atxV = Nothing
            atxM = jsLines("maxValueAttr")
            If atxM.Contains("ATTRIBUTE ERROR E11") Then atxM = Nothing
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
        If htmlParts.Length >= 5 Then
          ReDim Preserve htmlParts(4)
          For I As Integer = 0 To htmlParts.Length - 1
            htmlParts(I) = htmlParts(I).Substring(htmlParts(I).LastIndexOf(""">") + 2)
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
            If opM.Contains("ATTRIBUTE ERROR E11") Then opM = Nothing
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
            If atM.Contains("ATTRIBUTE ERROR E11") Then atM = Nothing
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
            If opV.Contains("ATTRIBUTE ERROR E11") Then opV = Nothing
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
            If atV.Contains("ATTRIBUTE ERROR E11") Then atV = Nothing
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
            If atxV.Contains("ATTRIBUTE ERROR E11") Then atxV = Nothing
          End If
        End If
      End If
    End If
    If String.IsNullOrEmpty(atM) And String.IsNullOrEmpty(atV) And String.IsNullOrEmpty(opM) And String.IsNullOrEmpty(opV) Then
      RaiseError("Usage Read Failed: Unable to locate data table!", "DN Read Table", Table)
      Return
    End If
    If String.IsNullOrEmpty(atM) Or String.IsNullOrEmpty(atV) Or String.IsNullOrEmpty(opM) Or String.IsNullOrEmpty(opV) Then
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Data temporarily unavailable."))
      Return
    End If
    Dim lDown, lDownT, lUp, lUpT As Long
    lDownT = StrToVal(atM, MBPerGB)
    lUpT = StrToVal(opM, MBPerGB)
    If lDownT = 0 Or lUpT = 0 Then
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.LoginFailure, "Data temporarily unavailable."))
      Return
    End If
    lDown = lDownT - StrToVal(atV, MBPerGB)
    lUp = lUpT - StrToVal(opV, MBPerGB)
    If Not String.IsNullOrEmpty(atxV) And Not String.IsNullOrEmpty(atxM) Then
      If StrToFloat(atxV) > 0.0 Then
        lDown += StrToVal(atxV, MBPerGB)
        lDownT += StrToVal(atxM, MBPerGB)
      End If
    End If
    ProviderSurvey("DNX")
    RaiseEvent ConnectionDNXResult(Me, New TYPEA2ResultEventArgs(lDown, lDownT, lUp, lUpT, Now, imSlowed, imFree))
  End Sub
#End Region
#End Region
#Region "Useful Functions"
  Private Sub ProviderSurvey(s As String)
    Dim sURI As New Uri("https://wb.realityripple.com/providerSurvey.php?p=" & s)
    Dim responseData As String = Nothing
    Dim responseURI As Uri = Nothing
    SendGET(sURI, responseURI, responseData)
  End Sub
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
  Private Sub SendPOST(SendURL As Uri, SendData As String, ByRef ReturnURL As Uri, ByRef ReturnData As String, Optional Headers As Net.WebHeaderCollection = Nothing)
    If c_TLSProxy Then
      SendTLSProxy(SendURL, SendData, ReturnURL, ReturnData, Headers)
      Return
    End If
    wsSocket.SendHeaders = New Net.WebHeaderCollection
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
      If String.IsNullOrEmpty(response) OrElse response = "Error: The server sent an empty response. Please try again." Then
        RaiseError("The server sent an empty response. Please try again.")
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
    If response.ToLower.Contains("504 gateway time-out") Then
      RaiseEvent ConnectionFailure(Me, New ConnectionFailureEventArgs(ConnectionFailureEventArgs.FailureType.ConnectionTimeout))
      Return True
    End If
    If response.StartsWith("Could not resolve host: ") Then
      RaiseError("The server is unavailable. Please try again later.")
      Return True
    End If
    If response.ToLower.Contains("internal server error") Or (response.ToLower.Contains("internal error") And response.Contains("500")) Then
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
  Private Sub BeginAttempt(state As ConnectionStates, substate As ConnectionSubStates, stage As Integer, attempt As Integer, URL As String)
    sAttemptedURL = URL
    AttemptedTag = state
    AttemptedSub = substate
    AttemptedStage = stage
    AttemptedTry = attempt
    RaiseEvent ConnectionStatus(Me, New ConnectionStatusEventArgs(state, substate, stage, attempt))
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
  Private Function StripXMLTags(text As String) As String
    Dim sText As String = text
    Do While sText.Contains("<")
      Dim sPre As String = sText.Substring(0, sText.IndexOf("<"))
      Dim sPost As String = Nothing
      If sText.Substring(sText.IndexOf("<")).Contains(">") Then
        sPost = sText.Substring(sText.IndexOf("<"))
        sPost = sPost.Substring(sPost.IndexOf(">") + 1)
      End If
      sText = sPre & sPost
    Loop
    Return sText
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
    MyBase.Finalize()
  End Sub
End Class
