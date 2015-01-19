Imports System.IO
Module modFunctions
  Public Const LATIN_1 As Integer = 28591
  Public Function PercentEncode(inString As String) As String
    Dim sRet As String = String.Empty
    If String.IsNullOrEmpty(inString) Then Return inString
    For I As Integer = inString.Length - 1 To 0 Step -1
      Dim iChar As Integer = Convert.ToInt32(inString(I))
      Select Case iChar
        Case 45, 46, 48 To 57, 65 To 90, 95, 97 To 122 : sRet = inString(I).ToString & sRet
        Case 32 : sRet = "+" & sRet
        Case Else : sRet = "%" & PadHex(iChar, 2) & sRet
      End Select
    Next
    Return sRet
  End Function
  Private Function PadHex(Value As UInt32, Length As UInt16) As String
    Dim sVal As String = Convert.ToString(Value, 16)
    Do While sVal.Length < Length : sVal = "0" & sVal : Loop
    Return sVal
  End Function
  Public Function HexDecode(inString As String) As String
    Dim sRet As String = String.Empty
    If String.IsNullOrEmpty(inString) Then Return inString
    For I As Integer = 0 To inString.Length - 1
      If inString(I) = "&" Then
        If inString.Length - I > 1 AndAlso inString(I + 1) = "#" Then
          If inString.Length - I > 2 AndAlso inString(I + 2) = "x" Then
            If inString.Length - I > 4 AndAlso inString(I + 4) = ";" Then
              Dim hVal As String = inString(I + 3)
              sRet &= Chr(Convert.ToByte(hVal, 16))
              I += 4
            ElseIf inString.Length - I > 5 AndAlso inString(I + 5) = ";" Then
              Dim hVal As String = inString(I + 3) & inString(I + 4)
              sRet &= Chr(Convert.ToByte(hVal, 16))
              I += 5
            Else
              sRet &= inString(I)
            End If
          Else
            sRet &= inString(I)
          End If
        Else
          sRet &= inString(I)
        End If
      Else
        sRet &= inString(I)
      End If
    Next
    Return sRet
  End Function
  Public Function NetworkErrorToString(ex As System.Exception, sDataPath As String)
    Dim reportHandler As New ReportSocketErrorInvoker(AddressOf ReportSocketError)
    If ex.InnerException Is Nothing Then
      If ex.Message.StartsWith("The remote name could not be resolved:") Then
        Return "Could not connect to your DNS. Check your Internet connection."
      ElseIf ex.Message.StartsWith("The underlying connection was closed") Then
        If ex.Message.Contains("The connection was closed unexpectedly") Then
          Return "Connection to server dropped. Please try again."
        ElseIf ex.Message.Contains("A connection that was expected to be kept alive was closed by the server") Then
          Return "Connection to server closed. Please try again."
        Else
          reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
          Return "Connection to server closed - " & ex.InnerException.Message
        End If
      ElseIf ex.Message.StartsWith("The remote server returned an error:") Then
        If ex.Message.Contains("400") Then
          Return "The server did not like the request. Please try again."
        ElseIf ex.Message.Contains("401") Then
          reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
          Return "The server did not like the login. Please check your provider."
        ElseIf ex.Message.Contains("404") Then
          Return "Server 404 (Not Found). The server may not be supported or may be down. Check your account settings and try again."
        ElseIf ex.Message.Contains("405") Then
          Return "The server did not like the method. Please try again."
        ElseIf ex.Message.Contains("500") Then
          Return "The server is too busy. Please try again."
        ElseIf ex.Message.Contains("502") Then
          Return "The gateway is unavailable. Please try again later."
        ElseIf ex.Message.Contains("503") Then
          Return "The server is unavailable. Please try again later."
        ElseIf ex.Message.Contains("504") Then
          Return "The server timed out. Please try again."
        ElseIf ex.Message.Contains("505") Then
          Return "Server 505 (Version Not Supported). The server may not be supported or may be down. Check your account settings and try again."
        Else
          reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
          If ex.Message.Contains(")") Then
            Return "The server returned " & ex.Message.Substring(ex.Message.IndexOf(")") + 1).Trim
          Else
            Return "The server returned " & ex.Message.Substring(ex.Message.IndexOf(":") + 1).Trim
          End If
        End If
      ElseIf ex.Message.StartsWith("The request timed out") Then
        Return "Connection to the server timed out. Please try again."
      ElseIf ex.Message.StartsWith("The operation has timed out") Then
        Return "Connection to the server timed out. Please try again."
      ElseIf ex.Message.StartsWith("The request was aborted:") Then
        If ex.Message.Contains("Could not create SSL/TLS secure channel") Then
          Return "Unable to create secure connection. Please change your Network Security Protocol settings and try again."
        ElseIf ex.Message.Contains("The request was canceled") Then
          Return "Connection aborted."
        Else
          reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
          Return "Connection aborted - " & ex.Message.Substring(ex.Message.IndexOf(": ") + 2)
        End If
      ElseIf ex.Message.StartsWith("Error:") Then
        If ex.Message.Contains("NameResolutionFailure") Then
          Return "Could not connect to your DNS. Check your Internet connection."
        ElseIf ex.Message.Contains("ConnectFailure") Then
          If ex.Message.Contains("Network is unreachable") Then
            Return "The network is unreachable. Check your Internet connection."
          Else
            reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
            Return "Connection Error - " & ex.Message
          End If
        Else
          reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
          Return "Error - " & ex.Message
        End If
      Else
        reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
        Return ex.Message
      End If
    Else
      If ex.Message.StartsWith("Unable to connect to the remote server") Then
        If ex.InnerException.Message.StartsWith("A connection attempt failed because the connected party did not respond properly after a period of time, or established connection failed because connected host has failed to respond") Then
          Return "The server did not respond. Check your Internet connection."
        ElseIf ex.InnerException.Message.StartsWith("A socket operation was attempted to an unreachable host") Then
          Return "The host is unreachable. Check your local network."
        ElseIf ex.InnerException.Message.StartsWith("A socket operation was attempted to an unreachable network") Then
          Return "The network is unreachable. Check your Internet connection."
        ElseIf ex.InnerException.Message.StartsWith("No connection could be made because the target machine actively refused it") Then
          Return "The server refused the connection. Please try again."
        ElseIf ex.InnerException.Message.StartsWith("An attempt was made to access a socket in a way forbidden by its access permissions") Then
          Return "The connection was forbidden. Please check your local network and firewall settings."
        ElseIf ex.InnerException.Message.StartsWith("An operation on a socket could not be performed because the system lacked sufficient buffer space or because a queue was full") Then
          Return "Too many connections open. Check your network activity or restart your computer."
        ElseIf ex.InnerException.Message.StartsWith("An established connection was aborted by the software in your host machine") Then
          Return "Connection aborted."
        ElseIf ex.InnerException.Message.Contains("A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond") Then
          Return "The server did not respond. Please try again."
        ElseIf ex.InnerException.Message.Contains("An operation was attempted on something that is not a socket") Then
          Return "Unable to connect. Check your local network and firewall settings."
        Else
          reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
          Return "Can't connect to the server - " & ex.InnerException.Message
        End If
      ElseIf ex.Message.StartsWith("An exception occurred during a WebClient request") Then
        If ex.InnerException.Message.StartsWith("Received an unexpected EOF or 0 bytes from the transport stream") Then
          Return "Received empty response from server. Please try again."
        ElseIf ex.InnerException.Message.StartsWith("Unable to read data from the transport connection: The connection was closed") Then
          Return "The server closed the connection. Please try again."
        ElseIf ex.InnerException.Message.StartsWith("Unable to read data from the transport connection: An existing connection was forcibly closed by the remote host") Then
          Return "The server closed the connection. Please try again."
        ElseIf ex.InnerException.Message.StartsWith("Unable to read data from the transport connection: An established connection was aborted by the software in your host machine") Then
          Return "Connection aborted."
        Else
          reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
          Return "Error during request - " & ex.InnerException.Message
        End If
      ElseIf ex.Message.StartsWith("Error getting response stream") Then
        If ex.InnerException.Message.StartsWith("BeginWrite failure") Then
          Return "Could not write response data. Check your local network."
        ElseIf ex.Message.Contains("ReadDone1") Then
          Return "The server closed the connection. Please try again."
        ElseIf ex.Message.Contains("ReadDone2") Then
          Return "Received empty response from server. Please try again."
        Else
          reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
          Return "Error during response - " & ex.InnerException.Message
        End If
      ElseIf ex.Message.StartsWith("The underlying connection was closed") Then
        If ex.Message.Contains("An unexpected error occurred on a send") Then
          If ex.InnerException.Message.StartsWith("Unable to read data from the transport connection") Then
            If ex.InnerException.Message.Contains("An existing connection was forcibly closed by the remote host") Then
              Return "The server is too busy to respond. Please try again later."
            Else
              reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
              Return "Connection to server failed to read - " & ex.InnerException.Message
            End If
          ElseIf ex.InnerException.Message.StartsWith("Authentication failed because the remote party has closed the transport stream") Then
            Return "The server closed the connection. Please try again."
          ElseIf ex.InnerException.Message.StartsWith("The handshake failed due to an unexpected packet format") Then
            Return "Connection server failed to negotiate. Please change your Network Security Protocol settings and try again."
          ElseIf ex.InnerException.Message.Contains("Received an unexpected EOF or 0 bytes from the transport stream") Then
            Return "The server closed the connection. Please try again."
          Else
            reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
            Return "Connection to server closed with an unexpected error - " & ex.InnerException.Message
          End If
        ElseIf ex.Message.Contains("An unexpected error occurred on a receive") Then
          If ex.InnerException.Message.StartsWith("Unable to read data from the transport connection") Then
            If ex.InnerException.Message.Contains("A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond") Then
              Return "The server did not respond. Please try again."
            ElseIf ex.InnerException.Message.Contains("An established connection was aborted by the software in your host machine") Then
              Return "Connection aborted."
            ElseIf ex.InnerException.Message.Contains("The decryption operation failed, see inner exception") Then
              reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
              Return "Decryption failure - " & ex.InnerException.InnerException.Message
            ElseIf ex.InnerException.Message.Contains("Received an unexpected EOF or 0 bytes from the transport stream") Then
              Return "The server closed the connection. Please try again."
            ElseIf ex.InnerException.Message.Contains("An existing connection was forcibly closed by the remote host.") Then
              Return "The server closed the connection. Please try again."
            Else
              reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
              Return "Read failure - " & ex.InnerException.Message
            End If
          ElseIf ex.InnerException.Message.Contains("Received an unexpected EOF or 0 bytes from the transport stream") Then
            Return "The server closed the connection. Please change your Network Security Protocol settings and try again."
          Else
            reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
            Return "Receive failure - " & ex.InnerException.Message
          End If
        ElseIf ex.Message.Contains("Could not establish trust relationship for the SSL/TLS secure channel.") Then
          If ex.InnerException.Message.StartsWith("The remote certificate is invalid according to the validation procedure") Then
            Return "Server certificate is invalid. Please change your Network Security Protocol settings and try again."
          Else
            reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
            Return "Server security could not be established - " & ex.InnerException.Message
          End If
        ElseIf ex.Message.Contains("Unable to connect to the remote server") Then
          If ex.InnerException.Message.StartsWith("An operation on a socket could not be performed because the system lacked sufficient buffer space or because a queue was full") Then
            Return "The server is busy with other requests. Please try again later."
          Else
            reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
            Return "Can't connect to the server - " & ex.InnerException.Message
          End If
        Else
          reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
          Return "Connection to server closed - " & ex.InnerException.Message
        End If
      ElseIf ex.Message.StartsWith("Error:") Then
        If ex.Message.Contains("ConnectFailure") Then
          If ex.InnerException.Message.Contains("No route to host") Then
            Return "Could not connect to the server. Check your Internet connection."
          ElseIf ex.InnerException.Message.Contains("Network is unreachable") Then
            Return "The network is unreachable. Check your Internet connection."
          Else
            reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
            Return "Connection Error - " & ex.InnerException.Message
          End If
        Else
          reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
          Return "Error - " & ex.Message
        End If
      Else
        reportHandler.BeginInvoke(ex, sDataPath, Nothing, Nothing)
        Return ex.Message & " - " & ex.InnerException.Message
      End If
    End If
  End Function
  Public Delegate Sub ReportSocketErrorInvoker(ex As Exception, DataPath As String)
  Public Sub ReportSocketError(ex As Exception, DataPath As String)
    Dim ReportList As String = DataPath & "\sckerrs.log"
    If IO.File.Exists(ReportList) Then
      If InUseChecker(ReportList, FileAccess.ReadWrite) Then
        My.Computer.FileSystem.WriteAllText(ReportList, ex.Message & vbNewLine, True)
        If ex.InnerException IsNot Nothing Then
          My.Computer.FileSystem.WriteAllText(ReportList, ex.InnerException.Message & vbNewLine, True)
          If ex.InnerException.InnerException IsNot Nothing Then My.Computer.FileSystem.WriteAllText(ReportList, ex.InnerException.InnerException.Message & vbNewLine, True)
        End If
        My.Computer.FileSystem.WriteAllText(ReportList, vbNewLine, True)
      End If
    Else
      My.Computer.FileSystem.WriteAllText(ReportList, ex.Message & vbNewLine, False)
      If ex.InnerException IsNot Nothing Then
        My.Computer.FileSystem.WriteAllText(ReportList, ex.InnerException.Message & vbNewLine, True)
        If ex.InnerException.InnerException IsNot Nothing Then My.Computer.FileSystem.WriteAllText(ReportList, ex.InnerException.InnerException.Message & vbNewLine, True)
      End If
      My.Computer.FileSystem.WriteAllText(ReportList, vbNewLine, True)
    End If
    SendSocketErrors(DataPath)
  End Sub
  Public Sub SendSocketErrors(DataPath As String)
    Dim ReportList As String = DataPath & "\sckerrs.log"
    If IO.File.Exists(ReportList) Then
      Dim reports As New Collections.Generic.List(Of String)(Split(My.Computer.FileSystem.ReadAllText(ReportList), vbNewLine & vbNewLine))
      For I As Integer = reports.Count - 1 To 0 Step -1
        Dim ReportBunch As String = reports(I)
        If Not String.IsNullOrEmpty(ReportBunch) Then
          Dim e, ie As String
          If ReportBunch.Contains(vbNewLine) Then
            e = Split(ReportBunch, vbNewLine, 2)(0)
            ie = Split(ReportBunch, vbNewLine, 2)(1)
            If ie.Contains(vbNewLine) Then ie = ie.Replace(vbNewLine, " - ")
          Else
            e = ReportBunch
            ie = Nothing
          End If
          Try
            Using sckUpload As New CookieAwareWebClient()
              Dim params As New Collections.Specialized.NameValueCollection
              params.Add("e", e)
              If Not String.IsNullOrEmpty(ie) Then params.Add("ie", ie)
              Dim bRet() As Byte = sckUpload.UploadValues("http://wb.realityripple.com/errmsgs.php", "POST", params)
              Dim sRet As String = System.Text.Encoding.GetEncoding(LATIN_1).GetString(bRet)
              If sRet = "e exists" Or sRet = "e added" Then reports.RemoveAt(I)
            End Using
          Catch
            Exit For
          End Try
        Else
          reports.RemoveAt(I)
        End If
      Next
      If InUseChecker(ReportList, FileAccess.ReadWrite) Then
        If reports.Count > 0 Then
          If reports.Count = 1 AndAlso String.IsNullOrEmpty(reports(0)) Then
            IO.File.Delete(ReportList)
          Else
            My.Computer.FileSystem.WriteAllText(ReportList, Join(reports.ToArray, vbNewLine & vbNewLine) & vbNewLine & vbNewLine, False)
          End If
        Else
          IO.File.Delete(ReportList)
        End If
      End If
    End If
  End Sub
  Public Function HostTypeToString(ht As localRestrictionTracker.SatHostTypes) As String
    Select Case ht
      Case localRestrictionTracker.SatHostTypes.WildBlue_LEGACY : Return "WBL"
      Case localRestrictionTracker.SatHostTypes.WildBlue_EXEDE : Return "WBX"
      Case localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY : Return "RPL"
      Case localRestrictionTracker.SatHostTypes.RuralPortal_EXEDE : Return "RPX"
      Case localRestrictionTracker.SatHostTypes.DishNet_EXEDE : Return "DNX"
      Case Else : Return "O"
    End Select
  End Function
  Public Function StringToHostType(st As String) As localRestrictionTracker.SatHostTypes
    Select Case st.ToUpper
      Case "WBL" : Return localRestrictionTracker.SatHostTypes.WildBlue_LEGACY
      Case "WBX", "WBV" : Return localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
      Case "RPL" : Return localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY
      Case "RPX" : Return localRestrictionTracker.SatHostTypes.RuralPortal_EXEDE
      Case "DNX" : Return localRestrictionTracker.SatHostTypes.DishNet_EXEDE
      Case "WILDBLUE" : Return localRestrictionTracker.SatHostTypes.WildBlue_LEGACY
      Case "EXEDE" : Return localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
      Case "DISHNET" : Return localRestrictionTracker.SatHostTypes.DishNet_EXEDE
      Case "RURALPORTAL" : Return localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY
      Case Else : Return localRestrictionTracker.SatHostTypes.Other
    End Select
  End Function
  ''' <summary>
  ''' Attempts to see if a file is in use, waiting up to five seconds for it to be freed.
  ''' </summary>
  ''' <param name="Filename">The exact path to the file which needs to be checked.</param>
  ''' <param name="access">Write permissions required for checking.</param>
  ''' <returns>True on available, false on in use.</returns>
  ''' <remarks></remarks>
  Public Function InUseChecker(Filename As String, access As IO.FileAccess) As Boolean
    If Not IO.File.Exists(Filename) Then Return True
    Dim iStart As Long = TickCount()
    Do
      Try
        Select Case access
          Case FileAccess.Read
            Using fs As FileStream = IO.File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite Or FileShare.Delete)
              If fs.CanRead Then
                Return True
                Exit Do
              End If
            End Using
          Case FileAccess.Write, FileAccess.ReadWrite
            Using fs As FileStream = IO.File.Open(Filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite Or FileShare.Delete)
              If fs.CanWrite Then
                Return True
                Exit Do
              End If
            End Using
        End Select
      Catch ex As Exception
      End Try
      Threading.Thread.Sleep(100)
      Threading.Thread.Sleep(0)
      Threading.Thread.Sleep(100)
    Loop While TickCount() - iStart < 5000
    Return False
  End Function
  Public Function TickCount() As Long
    Return (Stopwatch.GetTimestamp / Stopwatch.Frequency) * 1000
  End Function
  Public Function IterativeEqualityCheck(inArray1() As Byte, inArray2() As Byte) As Boolean
    If inArray1.Length = inArray2.Length Then
      Dim isEqual As Boolean = True
      For I As Integer = 0 To inArray1.Length - 1
        If Not inArray1(I) = inArray2(I) Then
          isEqual = False
          Exit For
        End If
      Next
      Return isEqual
    Else
      Return False
    End If
  End Function
End Module
