﻿Imports System.Security.Cryptography
Imports System.IO.Compression
Imports System.Net
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
  Private sProduct As String
  Private dFrom As Date
  Private tmrReadTimeout As Threading.Timer
  Private iTimeout As Integer
  Public Class FailureEventArgs
    Inherits EventArgs
    Public Enum FailType
      NoUsername
      BadProduct
      BadServer
      BadPassword
      NoData
      NoPassword
      Network
    End Enum
    Public Type As FailType
    Public Details As String
    Public Sub New(Failure As FailType, Optional Extra As String = Nothing)
      Type = Failure
      Details = Extra
    End Sub
  End Class
  Public Event Failure(sender As Object, e As FailureEventArgs)
  Public Class SuccessEventArgs
    Inherits EventArgs
    Public Provider As localRestrictionTracker.SatHostTypes
    Public Structure Result
      Public Time As DateTime
      Public Down As Integer
      Public DownMax As Integer
      Public Up As Integer
      Public UpMax As Integer
      Public Overrides Function ToString() As String
        Return Time.ToString("G") & ":" & Down & "/" & DownMax & ", " & Up & "/" & UpMax
      End Function
      Public Sub New(AtTime As DateTime, iDown As Integer, iDownMax As Integer, iUp As Integer, iUpMax As Integer)
        Time = AtTime
        Down = iDown
        Up = iUp
        DownMax = iDownMax
        UpMax = iUpMax
      End Sub
    End Structure
    Public Results() As Result
    Public Sub New(mProvider As localRestrictionTracker.SatHostTypes, mResults() As Result)
      Provider = mProvider
      Results = mResults
    End Sub
  End Class
  Public Event Success(sender As Object, e As SuccessEventArgs)
  Public Event OKKey(sender As Object, e As EventArgs)
  Private WithEvents wsLogin As CookieAwareWebClient
  Private ReadTimeoutCount As Integer
  Private sDataPath As String
  Private Sub tmrReadTimeout_Tick()
    ReadTimeoutCount += 1
    Dim TimeOutTime As Integer = iTimeout
    If TimeOutTime < 60 * 3 Then TimeOutTime = 60 * 3
    If ReadTimeoutCount >= TimeOutTime Then
      If tmrReadTimeout IsNot Nothing Then
        tmrReadTimeout.Dispose()
        tmrReadTimeout = Nothing
      End If
      ReadTimeoutCount = 0
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.Network, "The Remote Service timed out!"))
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
  Public Sub New(Username As String, Password As String, ProductKey As String, Proxy As IWebProxy, Timeout As Integer, UpdateFrom As Date, DataPath As String)
    ResetTimeout()
    If Username.Contains("@") Then
      sUsername = Split(Username, "@", 2)(0)
      sServer = Split(Username, "@", 2)(1)
    Else
      sUsername = Username
      sServer = Nothing
    End If
    sPassword = Password
    dFrom = UpdateFrom
    sProduct = ProductKey
    sDataPath = DataPath
    Secret = System.Text.Encoding.UTF8.GetBytes(ProductKey)
    wsLogin = New CookieAwareWebClient
    wsLogin.Timeout = Timeout
    wsLogin.Proxy = Proxy
    BeginLogin()
  End Sub
  Private Sub BeginLogin()
    ResetTimeout(True)
    If wsLogin Is Nothing Then
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.Network, "Remote Service Login terminated prematurely"))
      Exit Sub
    End If
    wsLogin.Headers.Clear()
    wsLogin.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
    Dim sPost As String = "s=init&user=" & sUsername & "@" & sServer & IIf(dFrom.Year = 2000, String.Empty, "&up=" & DateDiff(DateInterval.Second, (New DateTime(1970, 1, 1, 0, 0, 0, 0)), dFrom.ToUniversalTime))
    wsLogin.UploadStringAsync(New Uri(URLPath), "POST", sPost, "LOGIN")
  End Sub
  Private Sub SendCC()
    GenCC()
    ClientResponse = HashA()
    If wsLogin Is Nothing Then
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.Network, "Remote Service Login terminated during initialization"))
      Exit Sub
    End If
    wsLogin.Headers.Clear()
    wsLogin.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
    Dim sPost As String = "s=verify&cc=" & PercentEncode(Convert.ToBase64String(ClientChallenge)) & "&cr=" & PercentEncode(Convert.ToBase64String(ClientResponse))
    wsLogin.UploadStringAsync(New Uri(URLPath), "POST", sPost, "VERIFY")
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
        Using cStream As New CryptoStream(mStream, er, CryptoStreamMode.Write)
          Dim bPas() As Byte = System.Text.Encoding.UTF8.GetBytes(sPassword)
          cStream.Write(bPas, 0, bPas.Length)
          cStream.FlushFinalBlock()
          bPass = mStream.ToArray
          cStream.Close()
        End Using
        mStream.Close()
      End Using
    End Using
    CSP = Nothing
    If wsLogin Is Nothing Then
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.Network, "Remote Service Login terminated during verification"))
      Exit Sub
    End If
    wsLogin.Headers.Clear()
    wsLogin.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
    Dim sPost As String = "s=login&pass=" & PercentEncode(Convert.ToBase64String(bPass))
    wsLogin.UploadStringAsync(New Uri(URLPath), "POST", sPost, "PASS")
  End Sub
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
  Private Sub wsLogin_Failure(sender As Object, e As CookieAwareWebClient.ErrorEventArgs) Handles wsLogin.Failure
    ResetTimeout()
    RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.Network, NetworkErrorToString(e.Error, sDataPath)))
  End Sub
  Private Sub wsLogin_UploadStringCompleted(sender As Object, e As System.Net.UploadStringCompletedEventArgs) Handles wsLogin.UploadStringCompleted
    ResetTimeout()
    If e.Error Is Nothing Then
      If Not e.Cancelled Then
        Select Case e.UserState
          Case "LOGIN"
            Dim sRet As String = e.Result
            If sRet = "NOUSER" Then
              RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NoUsername))
            Else
              Try
                ServerChallenge = Convert.FromBase64String(sRet)
              Catch ex As Exception
                RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.Network, sRet))
                Exit Sub
              End Try
              SendCC()
              ResetTimeout(True)
            End If
          Case "VERIFY"
            Dim sRet As String = e.Result
            If sRet = "BADKEY" Then
              RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.BadProduct))
            ElseIf sRet = "NOUSER" Then
              RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NoUsername))
            ElseIf sRet = "NODATA" Then
              RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NoData))
            Else
              Try
                ServerResponse = Convert.FromBase64String(sRet)
              Catch ex As Exception
                RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.Network, sRet))
                Exit Sub
              End Try
              CalculatedServerResponse = HashB()
              If IterativeEqualityCheck(ServerResponse, CalculatedServerResponse) Then
                RaiseEvent OKKey(Me, New EventArgs)
                If Not String.IsNullOrEmpty(sPassword) Then
                  SendCR()
                  ResetTimeout(True)
                End If
              Else
                RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.BadServer))
              End If
            End If
          Case "PASS"
            Dim sRet As String = e.Result
            Dim bRETS() As Byte = System.Text.Encoding.GetEncoding(LATIN_1).GetBytes(sRet)
            Using OutStream As New IO.MemoryStream
              Dim bRet() As Byte
              Try
                bRet = Convert.FromBase64String(sRet)
              Catch ex As Exception
                RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.Network, sRet))
                Exit Sub
              End Try
              Dim outData() As Byte = DecompressData(bRet)
              sRet = System.Text.Encoding.UTF8.GetString(outData)
              If sRet.Contains(vbNullChar) Then sRet = sRet.Substring(0, sRet.IndexOf(vbNullChar))
            End Using
            If sRet = "NOUSER" Then
              RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NoUsername))
            ElseIf sRet = "NOPASS" Then
              RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NoPassword))
            ElseIf sRet = "Bad Login" Or sRet = "Bad Password" Then
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
                  iProv = StringToHostType(row.Substring(9))
                ElseIf row.Contains(":") And row.Contains("|") Then
                  Dim sTime As String = Split(row, ":", 2)(0)
                  Dim dish As Boolean = False
                  If sTime.StartsWith("d") Then
                    dish = True
                    sTime = sTime.Substring(1)
                  End If
                  Dim sData() As String = Split(Split(row, ":", 2)(1), "|")
                  Dim tTime As DateTime = DateAdd(DateInterval.Second, Val(sTime), (New DateTime(1970, 1, 1, 0, 0, 0, 0))).ToLocalTime

                  If sData.Length = 5 Then
                    If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
                    rData.Add(New remoteRestrictionTracker.SuccessEventArgs.Result(tTime,
                              StrToVal(sData(0), 1000),
                              StrToVal(sData(1), 1000) + (StrToVal(sData(4), 1000)),
                              StrToVal(sData(2), 1000),
                              StrToVal(sData(3), 1000)))
                  ElseIf sData.Length = 4 Then
                    If dish Then
                      If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.DishNet_EXEDE
                      rData.Add(New remoteRestrictionTracker.SuccessEventArgs.Result(tTime, StrToVal(sData(0), 1000), StrToVal(sData(1), 1000), StrToVal(sData(2), 1000), StrToVal(sData(3), 1000)))
                    Else
                      If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.WildBlue_LEGACY
                      rData.Add(New remoteRestrictionTracker.SuccessEventArgs.Result(tTime, StrToVal(sData(0)), StrToVal(sData(1)), StrToVal(sData(2)), StrToVal(sData(3))))
                    End If
                  ElseIf sData.Length = 3 Then
                    If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.RuralPortal_EXEDE
                    rData.Add(New remoteRestrictionTracker.SuccessEventArgs.Result(tTime,
                              StrToVal(sData(0), 1000) + (StrToVal(sData(1), 1000)),
                              StrToVal(sData(2), 1000),
                              StrToVal(sData(0), 1000) + (StrToVal(sData(1), 1000)),
                              StrToVal(sData(2), 1000)))
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
              Dim sTime As String = Split(sRet, ":", 2)(0)
              Dim dish As Boolean = False
              If sTime.StartsWith("d") Then
                dish = True
                sTime = sTime.Substring(1)
              End If
              Dim sData() As String = Split(Split(sRet, ":", 2)(1), "|")
              Dim tTime As DateTime = DateAdd(DateInterval.Second, Val(sTime), (New DateTime(1970, 1, 1, 0, 0, 0, 0))).ToLocalTime
              If sData.Length = 5 Then
                If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
                rData.Add(New remoteRestrictionTracker.SuccessEventArgs.Result(tTime,
                          StrToVal(sData(0), 1000),
                          StrToVal(sData(1), 1000) + (StrToVal(sData(4), 1000)),
                          StrToVal(sData(2), 1000),
                          StrToVal(sData(3), 1000)))
              ElseIf sData.Length = 4 Then
                If dish Then
                  If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.DishNet_EXEDE
                  rData.Add(New remoteRestrictionTracker.SuccessEventArgs.Result(tTime, StrToVal(sData(0), 1000), StrToVal(sData(1), 1000), StrToVal(sData(2), 1000), StrToVal(sData(3), 1000)))
                Else
                  If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.WildBlue_LEGACY
                  rData.Add(New remoteRestrictionTracker.SuccessEventArgs.Result(tTime, StrToVal(sData(0)), StrToVal(sData(1)), StrToVal(sData(2)), StrToVal(sData(3))))
                End If
              Else
                If iProv = localRestrictionTracker.SatHostTypes.Other Then iProv = localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY
                rData.Add(New remoteRestrictionTracker.SuccessEventArgs.Result(tTime,
                          StrToVal(sData(0), 1000) + (StrToVal(sData(1), 1000)),
                          StrToVal(sData(2), 1000),
                          StrToVal(sData(0), 1000) + (StrToVal(sData(1), 1000)),
                          StrToVal(sData(2), 1000)))
              End If
              If rData.Count > 0 Then
                RaiseEvent Success(Me, New SuccessEventArgs(iProv, rData.ToArray))
              Else
                RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NoData, sRet))
              End If
            Else
              RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.NoData, sRet))
            End If
            SendSocketErrors(sDataPath)
        End Select
      End If
    Else
      RaiseEvent Failure(Me, New FailureEventArgs(FailureEventArgs.FailType.Network, NetworkErrorToString(e.Error, sDataPath)))
    End If
  End Sub
  Private Function StrToVal(str As String, Optional vMult As Integer = 1) As Long
    If String.IsNullOrEmpty(str) Then Return 0
    If Not str.Contains(" ") Then Return CLng(Val(str.Replace(",", "")) * vMult)
    Return CLng(Val(str.Substring(0, str.IndexOf(" ")).Replace(",", "")) * vMult)
  End Function
  Private Function DecompressData(inData() As Byte) As Byte()
    Using outData As New IO.MemoryStream
      Using inStream As New IO.MemoryStream(inData)
        Using Decompress As DeflateStream = New DeflateStream(inStream, CompressionMode.Decompress)
          Dim buffer As Byte() = New Byte(4096) {}
          Dim numRead As Integer
          numRead = Decompress.Read(buffer, 0, buffer.Length)
          Do While numRead <> 0
            outData.Write(buffer, 0, numRead)
            numRead = Decompress.Read(buffer, 0, buffer.Length)
          Loop
        End Using
      End Using
      Return outData.ToArray
    End Using
  End Function
#Region "IDisposable Support"
  Private disposedValue As Boolean 
  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not Me.disposedValue Then
      If disposing Then
        If wsLogin IsNot Nothing Then
          wsLogin.Dispose()
          wsLogin = Nothing
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