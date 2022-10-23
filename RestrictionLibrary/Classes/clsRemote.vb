Imports System.Security.Cryptography
Imports System.IO.Compression
''' <summary>
''' Accesses the Remote Usage Service and handles all communication internally.
''' </summary>
Public Class remoteRestrictionTracker
  Implements IDisposable
  Private ServerChallenge() As Byte
  Private ClientChallenge() As Byte
  Private ServerResponse() As Byte
  Private ClientResponse() As Byte
  Private CalculatedServerResponse() As Byte
  Private Key() As Byte
  Private IV() As Byte
  Private Secret() As Byte
  Private Const URLPath As String = "http://wb.realityripple.com/login.php"
  Private sUsername As String
  Private sServer As String
  Private sPassword As String
  Private dFrom As Date
  ''' <summary>
  ''' Information regarding the type of failure received and any details.
  ''' </summary>
  Public Class FailureEventArgs
    Inherits EventArgs
    ''' <summary>
    ''' Types of failures used by the <see cref="FailureEventArgs" /> class.
    ''' </summary>
    Public Enum FailType
      ''' <summary>
      ''' The Username does not exist on the server.
      ''' </summary>
      NoUsername
      ''' <summary>
      ''' The Product Key has been disabled.
      ''' </summary>
      BadProduct
      ''' <summary>
      ''' The server's authenticity could not be verified.
      ''' </summary>
      BadServer
      ''' <summary>
      ''' Incorrect Password.
      ''' </summary>
      BadPassword
      ''' <summary>
      ''' The server could not log in to the provider.
      ''' </summary>
      BadLogin
      ''' <summary>
      ''' No usage data accumulated yet.
      ''' </summary>
      NoData
      ''' <summary>
      ''' The server does not know what your password is.
      ''' </summary>
      NoPassword
      ''' <summary>
      ''' Network connection error.
      ''' </summary>
      Network
      ''' <summary>
      ''' Server response was not Base64 encoded. All server responses should be, so this either means the server crapped out and gave a PHP error, or the connection was redirected elsewhere.
      ''' </summary>
      NotBase64
    End Enum
    ''' <summary>
    ''' The type of failure received from the server.
    ''' </summary>
    Public Type As FailType
    ''' <summary>
    ''' Details regarding the failure received from the server.
    ''' </summary>
    Public Details As String
    ''' <summary>
    ''' Generate a failure message as received from the server.
    ''' </summary>
    ''' <param name="Failure">The type of failure received from the server.</param>
    ''' <param name="Extra">Details regarding the failure from the server (optional). This data will be parsed down to the HTML title if one exists.</param>
    Public Sub New(Failure As FailType, Optional Extra As String = Nothing)
      Type = Failure
      If Type = FailType.NotBase64 Then
        If Not String.IsNullOrEmpty(Extra) AndAlso Extra.ToUpperInvariant.Contains("<HTML") Then
          If Extra.ToUpperInvariant.Contains("<TITLE") Then
            Dim htmlTitle As String = Extra.Substring(Extra.IndexOf("<TITLE", StringComparison.OrdinalIgnoreCase))
            htmlTitle = htmlTitle.Substring(htmlTitle.IndexOf(">") + 1)
            htmlTitle = htmlTitle.Substring(0, htmlTitle.IndexOf("</"))
            Details = """" & htmlTitle & """ received"
          Else
            Details = Extra
          End If
        Else
          Details = Extra
        End If
      Else
        Details = Extra
      End If
    End Sub
  End Class
  ''' <summary>
  ''' Triggered when the connection to the Remote Usage Service fails.
  ''' </summary>
  ''' <param name="sender">Instance of the <see cref="remoteRestrictionTracker" /> class.</param>
  ''' <param name="e"><see cref="FailureEventArgs" /> data regarding the failure.</param>
  Public Event Failure As EventHandler(Of FailureEventArgs)
  ''' <summary>
  ''' Response information from the server on a successful connection to the Remote Usage Service.
  ''' </summary>
  Public Class SuccessEventArgs
    Inherits EventArgs
    ''' <summary>
    ''' The type of provider which this account uses.
    ''' </summary>
    Public Provider As localRestrictionTracker.SatHostTypes
    ''' <summary>
    ''' A structure containing a single moment in usage history.
    ''' </summary>
    Public Structure Result
      ''' <summary>
      ''' The exact date and time this usage data belongs to.
      ''' </summary>
      Public Time As DateTime
      ''' <summary>
      ''' Number of megabytes used in download.
      ''' </summary>
      Public Down As Integer
      ''' <summary>
      ''' Number of megabytes allowed in download.
      ''' </summary>
      Public DownMax As Integer
      ''' <summary>
      ''' Number of megabytes used in upload.
      ''' </summary>
      Public Up As Integer
      ''' <summary>
      ''' Number of megabytes allowed in upload.
      ''' </summary>
      Public UpMax As Integer
      Private Ditto As Boolean
      Public Overrides Function ToString() As String
        If Ditto Then
          Return Time.ToString("G", srlFunctions.DateFormatProvider) & ":" & Down & "/" & DownMax
        Else
          Return Time.ToString("G", srlFunctions.DateFormatProvider) & ":" & Down & "/" & DownMax & ", " & Up & "/" & UpMax
        End If
      End Function
      ''' <summary>
      ''' Generate a new <see cref="Result" /> using a <see cref="localRestrictionTracker.TYPEBResultEventArgs">Type B</see> history.
      ''' </summary>
      ''' <param name="AtTime">The specific date and time of this usage.</param>
      ''' <param name="iUsed">Total number of megabytes used.</param>
      ''' <param name="iMax">Total number of megabytes allowed.</param>
      ''' <remarks><see cref="localRestrictionTracker.TYPEBResultEventArgs">Type B</see> history data does not separate Download and Upload data, and stores the Used and Max values in both Down and Up variables.</remarks>
      Public Sub New(AtTime As DateTime, iUsed As Integer, iMax As Integer)
        Ditto = True
        Time = AtTime
        Down = iUsed
        Up = iUsed
        DownMax = iMax
        UpMax = iMax
      End Sub
      ''' <summary>
      ''' Generate a new <see cref="Result" /> using a <see cref="localRestrictionTracker.TYPEAResultEventArgs">Type A</see> history.
      ''' </summary>
      ''' <param name="AtTime">The specific date and time of this usage.</param>
      ''' <param name="iDown">Number of megabytes used in download.</param>
      ''' <param name="iDownMax">Number of megabytes allowed in download.</param>
      ''' <param name="iUp">Number of megabytes used in upload.</param>
      ''' <param name="iUpMax">Number of megabytes allowed in upload.</param>
      ''' <remarks><see cref="localRestrictionTracker.TYPEAResultEventArgs">Type A</see> history data separates Download and Upload data, providing a more accurate breakdown of usage.</remarks>
      Public Sub New(AtTime As DateTime, iDown As Integer, iDownMax As Integer, iUp As Integer, iUpMax As Integer)
        Ditto = False
        Time = AtTime
        Down = iDown
        Up = iUp
        DownMax = iDownMax
        UpMax = iUpMax
      End Sub
    End Structure
    ''' <summary>
    ''' List of <see cref="Result" /> values in an array. This contains all the results which fit the range requested in the UpdateFrom param in the <see cref="remoteRestrictionTracker" /> constructor.
    ''' </summary>
    Public Results() As Result
    ''' <summary>
    ''' Generate a success message containing the data received from the server.
    ''' </summary>
    ''' <param name="mProvider">The type of provider which this account uses.</param>
    ''' <param name="mResults">An array of <see cref="Result" /> values containing all the responses from the server.</param>
    Public Sub New(mProvider As localRestrictionTracker.SatHostTypes, mResults() As Result)
      Provider = mProvider
      Results = mResults
    End Sub
  End Class
  ''' <summary>
  ''' Triggered when the Remote Usage Service returns data.
  ''' </summary>
  ''' <param name="sender">Instance of the <see cref="remoteRestrictionTracker" /> class.</param>
  ''' <param name="e"><see cref="SuccessEventArgs" /> data regarding the result.</param>
  Public Event Success As EventHandler(Of SuccessEventArgs)
  ''' <summary>
  ''' Triggered when the connection to the Remote Usage Service succeeds and the Product Key is correct.
  ''' </summary>
  ''' <param name="sender">Instance of the <see cref="remoteRestrictionTracker" /> class.</param>
  ''' <param name="e">Empty <see cref="EventArgs" /> object.</param>
  Public Event OKKey As EventHandler
  Private ClosingTime As Boolean
  Private tLogin As Threading.Thread
  Private c_Timeout As Integer
  Private c_Proxy As Net.IWebProxy
  Private c_Jar As Net.CookieContainer
  Private c_SendJar As Boolean
  Private sDataPath As String
  Private wsSocket As WebClientEx
  ''' <summary>
  ''' Constructor of the class, which also starts the connection process.
  ''' </summary>
  ''' <param name="Username">The Remote Usage Service account to connect to, which consists of the meter page Username and Provider Domain separated by an "@".</param>
  ''' <param name="Password">The password for the account. If this is set to blank, the connection will terminate after the <see cref="OKKey" /> event, which is useful for verifying the legitimacy of a <paramref name="ProductKey" /> without initiating a full connection.</param>
  ''' <param name="ProductKey">The account's Product Key, including hyphens, in the format XXXXXX-XXXX-XXXX-XXXX-XXXXXX</param>
  ''' <param name="Proxy">Proxy settings for connecting to the Remote Usage Service.</param>
  ''' <param name="Timeout">How long to wait between messages before timing out during connection.</param>
  ''' <param name="UpdateFrom">The earliest date to grab information from. The Remote Usage Service will return data from this date until the present time, rather than an entire usage history. Set the year to 2000 to grab the whole history.</param>
  ''' <param name="ConfigPath">The directory where configuration data is stored. This is used for reporting socket errors. If this value is null, then socket errors are not reported.</param>
  Public Sub New(Username As String, Password As String, ProductKey As String, Proxy As Net.IWebProxy, Timeout As Integer, UpdateFrom As Date, ConfigPath As String)
    ClosingTime = False
    If Username.Contains("@") Then
      sUsername = Split(Username, "@", 2)(0)
      sServer = Split(Username, "@", 2)(1)
    Else
      sUsername = Username
      sServer = Nothing
    End If
    sPassword = Password
    dFrom = UpdateFrom
    sDataPath = ConfigPath
    Secret = System.Text.Encoding.UTF8.GetBytes(ProductKey)
    c_Timeout = Timeout
    c_Proxy = Proxy
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
      ElseIf fwMajor = 4 Then
        If fwMinor >= 8 Then
          c_SendJar = False
        Else
          c_SendJar = True
        End If
      Else
        c_SendJar = True
      End If
    Else
      c_SendJar = False
    End If
    tLogin = New Threading.Thread(AddressOf Login)
    tLogin.Start()
  End Sub
  Private Sub Login()
    c_Jar = New Net.CookieContainer
    MakeSocket()
    Dim sPost As String = "s=init&user=" & sUsername & "@" & sServer & IIf(dFrom.Year = 2000, String.Empty, "&up=" & DateDiff(DateInterval.Second, (New DateTime(1970, 1, 1, 0, 0, 0, 0)), dFrom.ToUniversalTime))
    Dim sRet As String = wsSocket.UploadString(URLPath, "POST", sPost)
    If ClosingTime Then Return
    If CheckForErrors(sRet, wsSocket.ResponseURI) Then Return
    LoginResponse(sRet)
  End Sub
  Private Sub LoginResponse(response As String)
    If response = "NOUSER" Then
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NoUsername))
    Else
      Try
        ServerChallenge = Convert.FromBase64String(response)
      Catch ex As Exception
        RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NotBase64, response))
        Return
      End Try
      SendCC()
    End If
  End Sub

  Private Sub SendCC()
    GenCC()
    ClientResponse = HashA()
    MakeSocket()
    Dim sPost As String = "s=verify&cc=" & srlFunctions.PercentEncode(Convert.ToBase64String(ClientChallenge)) & "&cr=" & srlFunctions.PercentEncode(Convert.ToBase64String(ClientResponse))
    Dim sRet As String = wsSocket.UploadString(URLPath, "POST", sPost)
    If ClosingTime Then Return
    If CheckForErrors(sRet, wsSocket.ResponseURI) Then Return
    VerifyResponse(sRet)
  End Sub
  Private Sub VerifyResponse(Response As String)
    If Response = "BADKEY" Then
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.BadProduct))
    ElseIf Response = "NOUSER" Then
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NoUsername))
    ElseIf Response = "NODATA" Then
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NoData))
    Else
      Try
        ServerResponse = Convert.FromBase64String(Response)
      Catch ex As Exception
        RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NotBase64, Response))
        Return
      End Try
      CalculatedServerResponse = HashB()
      If srlFunctions.IterativeEqualityCheck(ServerResponse, CalculatedServerResponse) Then
        RaiseEvent OKKey(Me, New EventArgs)
        If Not String.IsNullOrEmpty(sPassword) Then SendCR()
      Else
        RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.BadServer))
      End If
    End If
  End Sub

  Private Sub SendCR()
    Dim bPass(sPassword.Length - 1) As Byte
    Dim CSP As New RijndaelManaged
    Key = HashC()
    CSP.Mode = CipherMode.CBC
    CSP.KeySize = 256
    CSP.BlockSize = 256
    Array.Resize(Key, CSP.KeySize / 8)
    IV = (SHA256Managed.Create).ComputeHash(Key)
    Array.Resize(IV, CSP.BlockSize / 8)
    Using er = CSP.CreateEncryptor(Key, IV)
      Using mStream As New IO.MemoryStream
        Dim cStream As New CryptoStream(mStream, er, CryptoStreamMode.Write)
        Dim bPas() As Byte = System.Text.Encoding.UTF8.GetBytes(sPassword)
        cStream.Write(bPas, 0, bPas.Length)
        cStream.FlushFinalBlock()
        bPass = mStream.ToArray
      End Using
    End Using
    CSP = Nothing
    MakeSocket()
    Dim sPost As String = "s=login&pass=" & srlFunctions.PercentEncode(Convert.ToBase64String(bPass))
    Dim sRet As String = wsSocket.UploadString(URLPath, "POST", sPost)
    If ClosingTime Then Return
    If CheckForErrors(sRet, wsSocket.ResponseURI) Then Return
    PassResponse(sRet)
  End Sub
  Private Sub PassResponse(Response As String)
    Dim sRet As String = Nothing
    Using OutStream As New IO.MemoryStream
      Dim bRet() As Byte
      Try
        bRet = Convert.FromBase64String(Response)
      Catch ex As Exception
        RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NotBase64, Response))
        Return
      End Try
      Dim outData() As Byte = DecompressData(bRet)
      sRet = System.Text.Encoding.UTF8.GetString(outData)
      If sRet.Contains(vbNullChar) Then sRet = sRet.Substring(0, sRet.IndexOf(vbNullChar))
    End Using
    If sRet = "NOUSER" Then
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NoUsername))
    ElseIf sRet = "NOPASS" Then
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NoPassword))
    ElseIf sRet = "Bad Login" Then
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.BadLogin))
    ElseIf sRet = "Bad Password" Then
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.BadPassword))
    ElseIf sRet = "BADKEY" Then
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.BadProduct))
    ElseIf sRet = "" Then
      RaiseEvent Success(Me, Nothing)
    ElseIf sRet.Contains(vbLf) Then
      Dim iProv As localRestrictionTracker.SatHostTypes
      Dim sRows() As String = sRet.Split(vbLf)
      Dim rData As New Collections.Generic.List(Of remoteRestrictionTracker.SuccessEventArgs.Result)
      For Each row In sRows
        If row.Contains("PROVIDER ") Then
          iProv = srlFunctions.StringToHostType(row.Substring(9))
        ElseIf row.Contains(":") And row.Contains("|") Then
          Dim sTime As String = Split(row, ":", 2)(0)
          Dim dish As Boolean = False
          If sTime.StartsWith("d", StringComparison.Ordinal) Then
            dish = True
            sTime = sTime.Substring(1)
          End If
          Dim sData() As String = Split(Split(row, ":", 2)(1), "|")
          Dim tTime As DateTime = DateAdd(DateInterval.Second, Val(sTime), (New DateTime(1970, 1, 1, 0, 0, 0, 0))).ToLocalTime
          If sData.Length = 5 Then
            If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
            Dim iUsed As Integer = StrToVal(sData(0), 1000) + StrToVal(sData(2), 1000)
            Dim iTotal As Integer = StrToVal(sData(1), 1000) + StrToVal(sData(3), 1000) + (StrToVal(sData(4), 1000))
            rData.Add(New remoteRestrictionTracker.SuccessEventArgs.Result(tTime, iUsed, iTotal))
          ElseIf sData.Length = 4 Then
            If dish Then
              If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.Dish_EXEDE
              rData.Add(New remoteRestrictionTracker.SuccessEventArgs.Result(tTime, StrToVal(sData(0), 1000), StrToVal(sData(1), 1000), StrToVal(sData(2), 1000), StrToVal(sData(3), 1000)))
            Else
              If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.WildBlue_LEGACY
              rData.Add(New remoteRestrictionTracker.SuccessEventArgs.Result(tTime, StrToVal(sData(0)), StrToVal(sData(1)), StrToVal(sData(2)), StrToVal(sData(3))))
            End If
          ElseIf sData.Length = 3 Then
            If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.RuralPortal_EXEDE
            Dim iUsed As Integer = StrToVal(sData(0), 1000) + (StrToVal(sData(1), 1000))
            Dim iTotal As Integer = StrToVal(sData(2), 1000)
            rData.Add(New remoteRestrictionTracker.SuccessEventArgs.Result(tTime, iUsed, iTotal))
          ElseIf sData.Length = 2 Then
            If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
            rData.Add(New SuccessEventArgs.Result(tTime, StrToVal(sData(0), 1000), StrToVal(sData(1), 1000)))
          End If
        End If
      Next
      If rData.Count > 0 Then
        RaiseEvent Success(Me, New SuccessEventArgs(iProv, rData.ToArray))
      Else
        RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NoData, sRet))
      End If
    ElseIf sRet.Contains(":") And sRet.Contains("|") Then
      Dim iProv As localRestrictionTracker.SatHostTypes
      Dim sRows() As String = sRet.Split(vbLf)
      Dim rData As New Collections.Generic.List(Of remoteRestrictionTracker.SuccessEventArgs.Result)
      For Each sRow As String In sRows
        Dim sTime As String = Split(sRow, ":", 2)(0)
        Dim dish As Boolean = False
        If sTime.StartsWith("d", StringComparison.Ordinal) Then
          dish = True
          sTime = sTime.Substring(1)
        End If
        Dim sData() As String = Split(Split(sRow, ":", 2)(1), "|")
        Dim tTime As DateTime = DateAdd(DateInterval.Second, Val(sTime), (New DateTime(1970, 1, 1, 0, 0, 0, 0))).ToLocalTime
        If sData.Length = 5 Then
          If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
          Dim iUsed As Integer = StrToVal(sData(0), 1000) + StrToVal(sData(2), 1000)
          Dim iTotal As Integer = StrToVal(sData(1), 1000) + StrToVal(sData(3), 1000) + (StrToVal(sData(4), 1000))
          rData.Add(New remoteRestrictionTracker.SuccessEventArgs.Result(tTime, iUsed, iTotal))
        ElseIf sData.Length = 4 Then
          If dish Then
            If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.Dish_EXEDE
            rData.Add(New remoteRestrictionTracker.SuccessEventArgs.Result(tTime, StrToVal(sData(0), 1000), StrToVal(sData(1), 1000), StrToVal(sData(2), 1000), StrToVal(sData(3), 1000)))
          Else
            If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.WildBlue_LEGACY
            rData.Add(New remoteRestrictionTracker.SuccessEventArgs.Result(tTime, StrToVal(sData(0)), StrToVal(sData(1)), StrToVal(sData(2)), StrToVal(sData(3))))
          End If
        ElseIf sData.Length = 3 Then
          If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY
          Dim iUsed As Integer = StrToVal(sData(0), 1000) + (StrToVal(sData(1), 1000))
          Dim iTotal As Integer = StrToVal(sData(2), 1000)
          rData.Add(New remoteRestrictionTracker.SuccessEventArgs.Result(tTime, iUsed, iTotal))
        ElseIf sData.Length = 2 Then
          If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
          rData.Add(New SuccessEventArgs.Result(tTime, StrToVal(sData(0), 1000), StrToVal(sData(1), 1000)))
        End If
      Next
      If rData.Count > 0 Then
        RaiseEvent Success(Me, New SuccessEventArgs(iProv, rData.ToArray))
      Else
        RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NoData, sRet))
      End If
    Else
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NoData, sRet))
    End If
    srlFunctions.SendSocketErrors(sDataPath)
  End Sub

  Private Sub MakeSocket()
    Dim oldEncoding As System.Text.Encoding = System.Text.Encoding.GetEncoding(srlFunctions.LATIN_1)
    If wsSocket IsNot Nothing Then
      oldEncoding = wsSocket.Encoding
      If wsSocket.IsBusy Then wsSocket.Cancel()
      wsSocket = Nothing
    End If
    wsSocket = New WebClientEx(sDataPath)
    wsSocket.KeepAlive = False
    wsSocket.Timeout = c_Timeout
    wsSocket.Proxy = c_Proxy
    wsSocket.CookieJar = c_Jar
    wsSocket.SendCookieJar = c_SendJar
    wsSocket.Encoding = oldEncoding
  End Sub
  Private Function CheckForErrors(response As String, responseURI As Uri) As Boolean
    If String.IsNullOrEmpty(response) Then
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.Network, "The server sent an empty response. Please try again."))
      Return True
    End If
    If response.StartsWith("Error: ") Then
      Dim sError As String = response.Substring(7)
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.Network, sError))
      Return True
    End If
    If response = "Connection timed out." Then
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.Network, "Connection Timed Out!"))
      Return True
    End If
    If responseURI IsNot Nothing Then
      If Not responseURI.Host = "wb.realityripple.com" Then
        RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.Network, "Connection redirected to """ & srlFunctions.TruncateAddress(responseURI) & """, check your Internet connection."))
        Return True
      End If
    End If
    Return False
  End Function
  Private Sub GenCC()
    ReDim ClientChallenge(63)
    Dim rng = RandomNumberGenerator.Create
    rng.GetNonZeroBytes(ClientChallenge)
    Do While ClientChallenge(ClientChallenge.Length - 1) = 0
      Array.Resize(ClientChallenge, ClientChallenge.Length - 1)
    Loop
  End Sub
  Private Function HashA() As Byte()
    Dim bHash(ClientChallenge.Length + ServerChallenge.Length + Secret.Length - 1) As Byte
    Array.ConstrainedCopy(ClientChallenge, 0, bHash, 0, ClientChallenge.Length)
    Array.ConstrainedCopy(ServerChallenge, 0, bHash, ClientChallenge.Length, ServerChallenge.Length)
    Array.ConstrainedCopy(Secret, 0, bHash, ClientChallenge.Length + ServerChallenge.Length, Secret.Length)
    Using mSHA As New RR_HMACSHA512
      mSHA.Initialize()
      mSHA.Key = System.Text.Encoding.UTF8.GetBytes("A45F314C8FCD60F63F7E2CC2FFADA1202D123F74D523BB6D5DE0788B49683FD4227A6CEDF33D1C0E6CFEFF34490C8D7F461ACC840739307590B1E1E393577B5D")
      Return mSHA.ComputeHash(bHash)
    End Using
  End Function
  Private Function HashB() As Byte()
    Dim bHash(ClientChallenge.Length + ServerChallenge.Length + Secret.Length - 1) As Byte
    Array.ConstrainedCopy(ClientChallenge, 0, bHash, ServerChallenge.Length, ClientChallenge.Length)
    Array.ConstrainedCopy(ServerChallenge, 0, bHash, 0, ServerChallenge.Length)
    Array.ConstrainedCopy(Secret, 0, bHash, ClientChallenge.Length + ServerChallenge.Length, Secret.Length)
    Using mSHA As New RR_HMACSHA512
      mSHA.Initialize()
      mSHA.Key = System.Text.Encoding.UTF8.GetBytes("5995917484CCCE1D3B174E89E9DF20A564D95527DFCFE6B3BB9D279762C3CDCB3596215496B88304839248F04353051C9B0D7A321EAAF0095DAAA2EB42095FC5")
      Return mSHA.ComputeHash(bHash)
    End Using
  End Function
  Private Function HashC() As Byte()
    Dim bHash(ClientChallenge.Length + ServerChallenge.Length + Secret.Length - 1) As Byte
    Array.ConstrainedCopy(ClientChallenge, 0, bHash, Secret.Length, ClientChallenge.Length)
    Array.ConstrainedCopy(ServerChallenge, 0, bHash, Secret.Length + ClientChallenge.Length, ServerChallenge.Length)
    Array.ConstrainedCopy(Secret, 0, bHash, 0, Secret.Length)
    Using mSHA As New RR_HMACSHA512
      mSHA.Initialize()
      mSHA.Key = System.Text.Encoding.UTF8.GetBytes("C474CAA2A61EF632CD6A20C659F7ED4E04C995045B68AE239445F5181E25B8E667A81547E54FAFC40D8274DDA19EB4B8DBEA5C545B2D922C975E15A866CEAE6B")
      Return mSHA.ComputeHash(bHash)
    End Using
  End Function
  Private Shared Function StrToVal(str As String, Optional vMult As Integer = 1) As Long
    If String.IsNullOrEmpty(str) Then Return 0
    If Not str.Contains(" ") Then Return CLng(Val(str.Replace(",", "")) * vMult)
    Return CLng(Val(str.Substring(0, str.IndexOf(" ")).Replace(",", "")) * vMult)
  End Function
  Private Shared Function DecompressData(inData() As Byte) As Byte()
    Using outData As New IO.MemoryStream
      Using inStream As New IO.MemoryStream(inData)
        Dim Decompress As DeflateStream = New DeflateStream(inStream, CompressionMode.Decompress)
        Dim buffer As Byte() = New Byte(4096) {}
        Dim numRead As Integer
        numRead = Decompress.Read(buffer, 0, buffer.Length)
        Do While numRead <> 0
          outData.Write(buffer, 0, numRead)
          numRead = Decompress.Read(buffer, 0, buffer.Length)
        Loop
      End Using
      Return outData.ToArray
    End Using
  End Function
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
End Class
