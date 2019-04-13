Imports System.IO
Public Class srlFunctions
  Private Shared lastSocketErrSend As Long = 0
  ''' <summary>
  ''' ISO-8859-1 Latin-1 Encoding
  ''' </summary>
  ''' <remarks>Charset for Latin-1 when using Text Encoding functions.</remarks>
  Public Const LATIN_1 As Integer = 28591
  ''' <summary>
  ''' Encodes a string for use in HTML form submissions.
  ''' </summary>
  ''' <param name="string">The string you wish to percent-encode.</param>
  ''' <returns>A string identical to <paramref name="string" /> except that nonalphanumeric characters are encoded for use in HTML forms.</returns>
  ''' <remarks>
  '''  <para>This function percent-encodes all non-alphanumeric characters with four exceptions.</para>
  '''  <para><c>Spaces</c> will be replaced with a &quot;+&quot; rather than &quot;%20&quot;.</para>
  '''  <para><c>Hyphens</c>, <c>Underscores</c>, and <c>Asterisks</c> will be left alone.</para>
  ''' </remarks>
  Public Shared Function PercentEncode([string] As String) As String
    Dim sRet As String = ""
    If String.IsNullOrEmpty([string]) Then Return [string]
    For I As Integer = [string].Length - 1 To 0 Step -1
      Dim iChar As Integer = Convert.ToInt32([string](I))
      Select Case iChar
        Case 45, 46, 48 To 57, 65 To 90, 95, 97 To 122, 126 : sRet = [string](I).ToString & sRet
        Case 32 : sRet = "+" & sRet
        Case Else : sRet = "%" & PadHex(iChar, 2) & sRet
      End Select
    Next
    Return sRet
  End Function
  ''' <summary>
  ''' Convert an <c>Integer</c> into a hexadecimal string with an appropriate zero-prefix padding.
  ''' </summary>
  ''' <param name="value">The base-10 number you wish to convert to base-16.</param>
  ''' <returns>A string containing the hexadecimal value that was passed to the function.</returns>
  ''' <remarks>
  ''' <para>If the length of the hexadecimal value is less than 8 characters and odd, one zero is prefixed.</para>
  ''' <para>If the length of the hexadecimal value is between 9 and 15 characters, enough zeroes are prefixed to bring the length to 16.</para>
  ''' <para>If the length of the hexadecimal value is between 17 and 31 characters, the string will be padded to 32.</para>
  ''' <para>If the length of the hexadecimal value is between 33 and 63 characters, the string will be padded to 64.</para>
  ''' <para>If the length of the hexadecimal value is between 65 and 127 characters, the string will be padded to 128.</para>
  ''' <para>If the length of the hexadecimal value is greater than 128, the string will not be padded.</para>
  ''' </remarks>
  Public Shared Function PadHex(value As UInt32) As String
    Dim sHex As String = Convert.ToString(value, 16)
    Select Case sHex.Length
      Case 0 : Return "00"
      Case 1, 3, 5, 7 : Return "0" & sHex
      Case 2, 4, 6, 8 : Return sHex
      Case Is < 17 : Return StrDup(16 - sHex.Length, "0") & sHex
      Case Is < 33 : Return StrDup(32 - sHex.Length, "0") & sHex
      Case Is < 65 : Return StrDup(64 - sHex.Length, "0") & sHex
      Case Is < 129 : Return StrDup(128 - sHex.Length, "0") & sHex
      Case Else : Return sHex
    End Select
  End Function
  ''' <summary>
  ''' Convert an <c>Integer</c> into a hexadecimal string with a specific zero-prefix padding.
  ''' </summary>
  ''' <param name="value">The base-10 number you wish to convert to base-16.</param>
  ''' <param name="padTo">The minimum length of the output value.</param>
  ''' <returns>A string containing the hexadecimal value that was passed to the function.</returns>
  ''' <remarks>If the length of the hexadecimal value is greater than <paramref name="padTo" /> then no padding is performed.</remarks>
  Public Shared Function PadHex(value As UInt32, padTo As UInt16) As String
    Dim sVal As String = Convert.ToString(value, 16)
    Do While sVal.Length < padTo : sVal = "0" & sVal : Loop
    Return sVal
  End Function
  ''' <summary>
  ''' Reverts hexadecimal HTML entities in a string to their original characters.
  ''' </summary>
  ''' <param name="string">The string you wish to decode.</param>
  ''' <returns>A string identical to <paramref name="string" /> except that all encoded values have been decoded.</returns>
  ''' <remarks>A hexadecimal HTML entity is in the form of &amp;#xNN;, where NN is a hexadecimal ASCII value for the character it represents.</remarks>
  Public Shared Function HexDecode([string] As String) As String
    Dim sRet As String = String.Empty
    If String.IsNullOrEmpty([string]) Then Return [string]
    For I As Integer = 0 To [string].Length - 1
      If [string](I) = "&" Then
        If [string].Length - I > 1 AndAlso [string](I + 1) = "#" Then
          If [string].Length - I > 2 AndAlso [string](I + 2) = "x" Then
            If [string].Length - I > 4 AndAlso [string](I + 4) = ";" Then
              Dim hVal As String = [string](I + 3)
              sRet &= Chr(Convert.ToByte(hVal, 16))
              I += 4
            ElseIf [string].Length - I > 5 AndAlso [string](I + 5) = ";" Then
              Dim hVal As String = [string](I + 3) & [string](I + 4)
              sRet &= Chr(Convert.ToByte(hVal, 16))
              I += 5
            Else
              sRet &= [string](I)
            End If
          Else
            sRet &= [string](I)
          End If
        Else
          sRet &= [string](I)
        End If
      Else
        sRet &= [string](I)
      End If
    Next
    Return sRet
  End Function
  ''' <summary>
  ''' Converts an <see cref="Exception" /> to a more pleasant and human-readable network error message.
  ''' </summary>
  ''' <param name="ex">The network exception which should be converted. Any exceptions which are not recognized will be reported.</param>
  ''' <param name="dataPath">The folder where socket errors will be saved to if they are not recognized.</param>
  ''' <returns>A simpler and cleaner description of the network error passed in <paramref name="ex" />.</returns>
  ''' <remarks>See Also: <seealso cref="ReportSocketError" /></remarks>
  Public Shared Function NetworkErrorToString(ex As System.Exception, dataPath As String) As String
    Dim ERRLIST As New Dictionary(Of String, String)
    ERRLIST.Add("DNS", "Could not connect to your DNS. Check your Firewall and Interntet connection.")
    ERRLIST.Add("NETWORK", "The network is unreachable. Check your Firewall and Internet connection.")
    ERRLIST.Add("UNEXPECTED", "The server response was unexpected. Check your Firewall and Internet connection.")
    ERRLIST.Add("REFUSED", "The connection was refused. Check your Firewall and Internet connection.")
    ERRLIST.Add("DROPPED", "The connection was dropped. Please try again.")
    ERRLIST.Add("CLOSED", "The connection was closed. Please try again.")
    ERRLIST.Add("ABORTED", "The connection has been aborted.")
    ERRLIST.Add("TIMEOUT", "The connection timed out. Please try again.")
    ERRLIST.Add("NULL", "The server sent an empty response. Please try again.")
    ERRLIST.Add("MEMORY", "Too many connections open. Check your network activity or restart your computer.")
    ERRLIST.Add("BUSY", "The server is too busy to respond. Please try again later.")
    ERRLIST.Add("BADREQUEST", "The server did not like the request. Please check your provider.")
    ERRLIST.Add("DOWN", "The server is down right now. Please try again later.")
    ERRLIST.Add("NOTFOUND", "The file could not be found. The server may be down right now. Please try again later.")
    ERRLIST.Add("NOTSUPPORTED", "The server's version is not supported. It may be down right now, or you may need to change some network settings.")
    ERRLIST.Add("NEGATIVE", "Negative Length exception. Check your Firewall and Internet connection.")
    ERRLIST.Add("TLS", "The server requires a specific SSL/TLS version. Please check your Network Security settings in the Configuration.")
    ERRLIST.Add("SECURITY", "Security exception. Check your Firewall and Internet connection.")
    ERRLIST.Add("UNKNOWN", "Unable to connect to the server.")
    Dim reportHandler As New ReportSocketErrorInvoker(AddressOf ReportSocketError)
    If ex.InnerException Is Nothing Then
      If ex.Message.StartsWith("The remote name could not be resolved:") Then
        Return ERRLIST("DNS")
      ElseIf ex.Message.StartsWith("The underlying connection was closed") Then
        If ex.Message.Contains("The connection was closed unexpectedly") Then
          Return ERRLIST("DROPPED")
        ElseIf ex.Message.Contains("A connection that was expected to be kept alive was closed by the server") Then
          Return ERRLIST("CLOSED")
        Else
          If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
          Return AppendErrorDetails(ERRLIST("CLOSED"), ex)
        End If
      ElseIf ex.Message.StartsWith("The remote server returned an error:") Then
        If ex.Message.Contains("400") Then
          Return ERRLIST("BADREQUEST")
        ElseIf ex.Message.Contains("401") Then
          If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
          Return ERRLIST("BADREQUEST")
        ElseIf ex.Message.Contains("403") Then
          Return ERRLIST("BADREQUEST")
        ElseIf ex.Message.Contains("404") Then
          Return ERRLIST("NOTFOUND")
        ElseIf ex.Message.Contains("405") Then
          Return ERRLIST("BADREQUEST")
        ElseIf ex.Message.Contains("408") Then
          Return ERRLIST("TIMEOUT")
        ElseIf ex.Message.Contains("500") Then
          Return ERRLIST("BUSY")
        ElseIf ex.Message.Contains("502") Then
          Return ERRLIST("DOWN")
        ElseIf ex.Message.Contains("503") Then
          Return ERRLIST("DOWN")
        ElseIf ex.Message.Contains("504") Then
          Return ERRLIST("TIMEOUT")
        ElseIf ex.Message.Contains("505") Then
          Return ERRLIST("NOTSUPPORTED")
        Else
          If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
          If ex.Message.Contains(")") Then
            Return AppendErrorDetails("The server returned an unsupported error.", ex, ")"c)
          Else
            Return AppendErrorDetails("The server returned an unsupported error.", ex, ":"c)
          End If
        End If
      ElseIf ex.Message.StartsWith("The request timed out") Then
        Return ERRLIST("TIMEOUT")
      ElseIf ex.Message.StartsWith("The operation has timed out") Then
        Return ERRLIST("TIMEOUT")
      ElseIf ex.Message.StartsWith("The request was aborted:") Then
        If ex.Message.Contains("Could not create SSL/TLS secure channel") Then
          Return ERRLIST("TLS")
        ElseIf ex.Message.Contains("The request was canceled") Then
          Return ERRLIST("ABORTED")
        ElseIf ex.Message.Contains("The operation has timed out") Then
          Return ERRLIST("TIMEOUT")
        Else
          If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
          Return AppendErrorDetails(ERRLIST("ABORTED"), ex, ":"c)
        End If
      ElseIf ex.Message = "Aborted." Then
        Return ERRLIST("ABORTED")
      ElseIf ex.Message.Contains("Exception of type 'System.OutOfMemoryException' was thrown") Then
        Return ERRLIST("MEMORY")
      ElseIf ex.Message.Contains("The server committed a protocol violation") Then
        Return ERRLIST("UNEXPECTED")
      ElseIf ex.Message.StartsWith("Error:") Then
        If ex.Message.Contains("NameResolutionFailure") Then
          Return ERRLIST("DNS")
        ElseIf ex.Message.Contains("ConnectFailure") Then
          If ex.Message.Contains("Network is unreachable") Then
            Return ERRLIST("NETWORK")
          Else
            If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
            Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex, ":"c)
          End If
        ElseIf ex.Message.Contains("SecureChannelFailure") Then
          Return ERRLIST("TLS")
        Else
          If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
          Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex, ":"c)
        End If
      ElseIf ex.Message.StartsWith("Cannot be negative.") And ex.Message.Contains("Parameter name: length") Then
        Return ERRLIST("NEGATIVE")
      ElseIf ex.Message.StartsWith("Thread was being aborted") Then
        Return ERRLIST("ABORTED")
      Else
        If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
        Return ex.Message
      End If
    Else
      If ex.Message.StartsWith("Unable to connect to the remote server") Then
        If ex.InnerException.Message.Contains("A connection attempt failed because the connected party did not respond properly after a period of time, or established connection failed because connected host has failed to respond") Then
          Return ERRLIST("TIMEOUT")
        ElseIf ex.InnerException.Message.Contains("A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond") Then
          Return ERRLIST("TIMEOUT")
        ElseIf ex.InnerException.Message.Contains("A connection attempt failed because the connected party did not respond properly after a period of time or established connection failed because connected host has failed to respond") Then
          Return ERRLIST("TIMEOUT")
        ElseIf ex.InnerException.Message.Contains("A connection attempt failed because the connected party did not properly respond after a period of time or established connection failed because connected host has failed to respond") Then
          Return ERRLIST("TIMEOUT")
        ElseIf ex.InnerException.Message.StartsWith("A socket operation was attempted to an unreachable host") Then
          Return ERRLIST("NETWORK")
        ElseIf ex.InnerException.Message.StartsWith("A socket operation was attempted to an unreachable network") Then
          Return ERRLIST("NETWORK")
        ElseIf ex.InnerException.Message.StartsWith("No connection could be made because the target machine actively refused it") Then
          Return ERRLIST("REFUSED")
        ElseIf ex.InnerException.Message.StartsWith("An attempt was made to access a socket in a way forbidden by its access permissions") Then
          Return ERRLIST("REFUSED")
        ElseIf ex.InnerException.Message.StartsWith("An operation on a socket could not be performed because the system lacked sufficient buffer space or because a queue was full") Then
          Return ERRLIST("MEMORY")
        ElseIf ex.InnerException.Message.StartsWith("An established connection was aborted by the software in your host machine") Then
          Return ERRLIST("ABORTED")
        ElseIf ex.InnerException.Message.Contains("An operation was attempted on something that is not a socket") Then
          Return ERRLIST("NETWORK")
        ElseIf ex.InnerException.Message.Contains("An invalid argument was supplied") Then
          Return ERRLIST("NETWORK")
        Else
          If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
          Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex.InnerException)
        End If
      ElseIf ex.Message.StartsWith("An exception occurred during a WebClient request") Then
        If ex.InnerException.Message.StartsWith("Received an unexpected EOF or 0 bytes from the transport stream") Then
          Return ERRLIST("NULL")
        ElseIf ex.InnerException.Message.StartsWith("Unable to read data from the transport connection: The connection was closed") Then
          Return ERRLIST("CLOSED")
        ElseIf ex.InnerException.Message.StartsWith("Unable to read data from the transport connection: An existing connection was forcibly closed by the remote host") Then
          Return ERRLIST("CLOSED")
        ElseIf ex.InnerException.Message.StartsWith("Unable to read data from the transport connection: An established connection was aborted by the software in your host machine") Then
          Return ERRLIST("ABORTED")
        ElseIf ex.InnerException.Message.StartsWith("Unable to write data to the transport connection: An established connection was aborted by the software in your host machine") Then
          Return ERRLIST("ABORTED")
        ElseIf ex.InnerException.Message.StartsWith("Unable to read data from the transport connection: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond") Then
          Return ERRLIST("TIMEOUT")
        ElseIf ex.InnerException.Message.StartsWith("Unable to read data from the transport connection: A connection attempt failed because the connected party did not respond properly after a period of time, or established connection failed because connected host has failed to respond") Then
          Return ERRLIST("TIMEOUT")
        ElseIf ex.InnerException.Message.StartsWith("Unable to read data from the transport connection: A connection attempt failed because the connected party did not properly respond after a period of time or established connection failed because connected host has failed to respond") Then
          Return ERRLIST("TIMEOUT")
        ElseIf ex.InnerException.Message.StartsWith("Unable to read data from the transport connection: A connection attempt failed because the connected party did not respond properly after a period of time or established connection failed because connected host has failed to respond") Then
          Return ERRLIST("TIMEOUT")
        ElseIf ex.InnerException.Message.StartsWith("Unable to write data to the transport connection: An existing connection was forcibly closed by the remote host") Then
          Return ERRLIST("CLOSED")
        ElseIf ex.InnerException.Message.StartsWith("Unable to read data from the transport connection: An operation on a socket could not be performed because the system lacked sufficient buffer space or because a queue was full") Then
          Return ERRLIST("MEMORY")
        ElseIf ex.InnerException.Message.StartsWith("Cannot open log for source 'Restriction Logger'. You may not have write access") Then
          Return "Error writing to error log. Try running as an administrator?"
        ElseIf ex.InnerException.Message.StartsWith("Object reference not set to an instance of an object") Then
          Return ERRLIST("ABORTED")
        ElseIf ex.InnerException.Message.StartsWith("Thread was being aborted") Then
          Return ERRLIST("ABORTED")
        ElseIf ex.InnerException.Message.StartsWith("There were not enough free threads in the ThreadPool to complete the operation") Then
          Return ERRLIST("MEMORY")
        ElseIf ex.InnerException.Message.StartsWith("The decryption operation failed, see inner exception") Then
          If ex.InnerException.InnerException IsNot Nothing Then
            If ex.InnerException.InnerException.Message.StartsWith("The message or signature supplied for verification has been altered") Then
              Return ERRLIST("SECURITY") & " The message or signature has been altered."
            ElseIf ex.InnerException.InnerException.Message.StartsWith("The specified data could not be decrypted") Then
              Return ERRLIST("SECURITY") & " Data could not be decrypted."
            ElseIf ex.InnerException.InnerException.Message.StartsWith("The token supplied to the function is invalid") Then
              Return ERRLIST("SECURITY") & " Invalid token."
            Else
              If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
              Return AppendErrorDetails(ERRLIST("SECURITY"), ex.InnerException.InnerException)
            End If
          Else
            If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
            Return ERRLIST("SECURITY")
          End If
        ElseIf ex.InnerException.Message.StartsWith("The read operation failed, see inner exception") Then
          If ex.InnerException.InnerException IsNot Nothing Then
            If ex.InnerException.InnerException.Message.StartsWith("Thread was being aborted") Then
              Return ERRLIST("ABORTED")
            Else
              If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
              Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex.InnerException.InnerException)
            End If
          Else
            If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
            Return ERRLIST("UNKNOWN")
          End If
        ElseIf ex.InnerException.Message.StartsWith("The write operation failed, see inner exception") Then
          If ex.InnerException.InnerException IsNot Nothing Then
            If ex.InnerException.InnerException.Message.StartsWith("Thread was being aborted") Then
              Return ERRLIST("ABORTED")
            Else
              If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
              Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex.InnerException.InnerException)
            End If
          Else
            If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
            Return ERRLIST("UNKNOWN")
          End If
        ElseIf ex.InnerException.Message.StartsWith("EndRead failure") Then
          If ex.InnerException.InnerException IsNot Nothing Then
            If ex.InnerException.InnerException.Message.StartsWith("Connection reset by peer") Then
              Return ERRLIST("CLOSED")
            Else
              If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
              Return AppendErrorDetails(ERRLIST("CLOSED"), ex.InnerException)
            End If
          Else
            If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
            Return AppendErrorDetails(ERRLIST("CLOSED"), ex.InnerException)
          End If
        ElseIf ex.InnerException.Message.StartsWith("Error writing request") Then
          If ex.InnerException.InnerException IsNot Nothing Then
            If ex.InnerException.InnerException.Message.StartsWith("The socket has been shut down") Then
              Return ERRLIST("ABORTED")
            Else
              If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
              Return AppendErrorDetails(ERRLIST("CLOSED"), ex.InnerException)
            End If
          Else
            If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
            Return AppendErrorDetails(ERRLIST("CLOSED"), ex.InnerException)
          End If
        Else
          If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
          Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex.InnerException)
        End If
      ElseIf ex.Message.StartsWith("Error getting response stream") Then
        If ex.InnerException.Message.StartsWith("BeginWrite failure") Then
          Return ERRLIST("NETWORK")
        ElseIf ex.Message.Contains("ReadDone1") Then
          Return ERRLIST("CLOSED")
        ElseIf ex.Message.Contains("ReadDone2") Then
          Return ERRLIST("NULL")
        ElseIf ex.Message.Contains("SendFailure") Then
          If ex.InnerException.Message.StartsWith("The authentication or decryption has failed") Then
            If ex.InnerException.InnerException IsNot Nothing Then
              If ex.InnerException.InnerException.Message.StartsWith("The authentication or decryption has failed") Then
                Return ERRLIST("TLS")
              ElseIf ex.InnerException.InnerException.Message.StartsWith("The server stopped the handshake") Then
                Return ERRLIST("CLOSED")
              ElseIf ex.InnerException.InnerException.Message.StartsWith("Number overflow") Then
                Return ERRLIST("SECURITY") & " There was a number overflow error."
              Else
                If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
                Return AppendErrorDetails(ERRLIST("SECURITY"), ex.InnerException.InnerException)
              End If
            Else
              If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
              Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex)
            End If
          ElseIf ex.InnerException.Message.StartsWith("Unsupported security protocol type") Then
            Return ERRLIST("TLS")
          Else
            If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
            Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex)
          End If
        ElseIf ex.Message.Contains("ReceiveFailure") Then
          If ex.Message.Contains("chunked Read2") Then
            Return ERRLIST("TLS")
          Else
            If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
            Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex)
          End If
        Else
          If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
          Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex)
        End If
      ElseIf ex.Message.StartsWith("The underlying connection was closed") Then
        If ex.Message.Contains("An unexpected error occurred on a send") Then
          If ex.InnerException.Message.StartsWith("Unable to read data from the transport connection") Then
            If ex.InnerException.Message.Contains("An existing connection was forcibly closed by the remote host") Then
              Return ERRLIST("BUSY")
            ElseIf ex.InnerException.Message.Contains("An established connection was aborted by the software in your host machine") Then
              Return ERRLIST("ABORTED")
            ElseIf ex.InnerException.Message.Contains("A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond") Then
              Return ERRLIST("TIMEOUT")
            ElseIf ex.InnerException.Message.Contains("A connection attempt failed because the connected party did not respond properly after a period of time, or established connection failed because connected host has failed to respond") Then
              Return ERRLIST("TIMEOUT")
            ElseIf ex.InnerException.Message.Contains("A connection attempt failed because the connected party did not properly respond after a period of time or established connection failed because connected host has failed to respond") Then
              Return ERRLIST("TIMEOUT")
            ElseIf ex.InnerException.Message.Contains("A connection attempt failed because the connected party did not respond properly after a period of time or established connection failed because connected host has failed to respond") Then
              Return ERRLIST("TIMEOUT")
            Else
              If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
              Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex.InnerException, ":"c)
            End If
          ElseIf ex.InnerException.Message.StartsWith("Unable to write data to the transport connection") Then
            If ex.InnerException.Message.Contains("An existing connection was forcibly closed by the remote host") Then
              Return ERRLIST("BUSY")
            Else
              If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
              Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex.InnerException, ":"c)
            End If
          ElseIf ex.InnerException.Message.StartsWith("Authentication failed because the remote party has closed the transport stream") Then
            Return ERRLIST("CLOSED")
          ElseIf ex.InnerException.Message.StartsWith("The handshake failed due to an unexpected packet format") Then
            Return ERRLIST("TLS")
          ElseIf ex.InnerException.Message.Contains("Received an unexpected EOF or 0 bytes from the transport stream") Then
            Return ERRLIST("TLS")
          Else
            If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
            Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex.InnerException)
          End If
        ElseIf ex.Message.Contains("An unexpected error occurred on a receive") Then
          If ex.InnerException.Message.StartsWith("Unable to read data from the transport connection") Then
            If ex.InnerException.Message.Contains("A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond") Then
              Return ERRLIST("TIMEOUT")
            ElseIf ex.InnerException.Message.Contains("A connection attempt failed because the connected party did not respond properly after a period of time, or established connection failed because connected host has failed to respond") Then
              Return ERRLIST("TIMEOUT")
            ElseIf ex.InnerException.Message.Contains("A connection attempt failed because the connected party did not properly respond after a period of time or established connection failed because connected host has failed to respond") Then
              Return ERRLIST("TIMEOUT")
            ElseIf ex.InnerException.Message.Contains("A connection attempt failed because the connected party did not respond properly after a period of time or established connection failed because connected host has failed to respond") Then
              Return ERRLIST("TIMEOUT")
            ElseIf ex.InnerException.Message.Contains("An established connection was aborted by the software in your host machine") Then
              Return ERRLIST("ABORTED")
            ElseIf ex.InnerException.Message.Contains("The decryption operation failed, see inner exception") Then
              If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
              Return AppendErrorDetails(ERRLIST("SECURITY"), ex.InnerException.InnerException)
            ElseIf ex.InnerException.Message.Contains("Received an unexpected EOF or 0 bytes from the transport stream") Then
              Return ERRLIST("CLOSED")
            ElseIf ex.InnerException.Message.Contains("An existing connection was forcibly closed by the remote host.") Then
              Return ERRLIST("CLOSED")
            Else
              If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
              Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex.InnerException, ":"c)
            End If
          ElseIf ex.InnerException.Message.Contains("Received an unexpected EOF or 0 bytes from the transport stream") Then
            Return ERRLIST("CLOSED")
          ElseIf ex.InnerException.Message.StartsWith("The decryption operation failed, see inner exception") Then
            If ex.InnerException.InnerException IsNot Nothing Then
              If ex.InnerException.InnerException.Message.StartsWith("The specified data could not be decrypted") Then
                Return ERRLIST("SECURITY") & " Data could not be decrypted."
              Else
                If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
                Return AppendErrorDetails(ERRLIST("SECURITY"), ex.InnerException.InnerException)
              End If
            Else
              If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
              Return ERRLIST("SECURITY")
            End If
          ElseIf ex.InnerException.Message.StartsWith("The client and server cannot communicate, because they do not possess a common algorithm") Then
            Return ERRLIST("TLS")
          Else
            If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
            Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex.InnerException)
          End If
        ElseIf ex.Message.Contains("Could not establish trust relationship for the SSL/TLS secure channel.") Then
          If ex.InnerException.Message.StartsWith("The remote certificate is invalid according to the validation procedure") Then
            Return ERRLIST("SECURITY") & " Server certificate is invalid."
          Else
            If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
            Return AppendErrorDetails(ERRLIST("SECURITY"), ex.InnerException)
          End If
        ElseIf ex.Message.Contains("Unable to connect to the remote server") Then
          If ex.InnerException.Message.StartsWith("An operation on a socket could not be performed because the system lacked sufficient buffer space or because a queue was full") Then
            Return ERRLIST("MEMORY")
          Else
            If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
            Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex.InnerException)
          End If
        ElseIf ex.Message.Contains("A connection that was expected to be kept alive was closed by the server") Then
          If ex.InnerException.Message.StartsWith("Unable to read data from the transport connection") Then
            If ex.InnerException.Message.Contains("A connection attempt failed because the connected party did not properly respond after a period of time") Then
              Return ERRLIST("TIMEOUT")
            ElseIf ex.InnerException.Message.Contains("An existing connection was forcibly closed by the remote host") Then
              Return ERRLIST("CLOSED")
            ElseIf ex.InnerException.Message.Contains("An established connection was aborted by the software in your host machine") Then
              Return ERRLIST("ABORTED")
            Else
              If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
              Return AppendErrorDetails(ERRLIST("CLOSED"), ex.InnerException, ":"c)
            End If
          Else
            If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
            Return AppendErrorDetails(ERRLIST("CLOSED"), ex.InnerException)
          End If
        Else
          If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
          Return AppendErrorDetails(ERRLIST("CLOSED"), ex)
        End If
      ElseIf ex.Message.StartsWith("Error:") Or ex.Message.StartsWith("System.Net.WebException: Error:") Then
        If ex.Message.Contains("ConnectFailure") Then
          If ex.InnerException.Message.Contains("No route to host") Then
            Return ERRLIST("NETWORK")
          ElseIf ex.InnerException.Message.Contains("Host is down") Then
            Return ERRLIST("DOWN")
          ElseIf ex.InnerException.Message.Contains("Network is unreachable") Then
            Return ERRLIST("NETWORK")
          ElseIf ex.InnerException.Message.Contains("Connection refused") Then
            Return ERRLIST("REFUSED")
          ElseIf ex.InnerException.Message.Contains("Connection timed out") Then
            Return ERRLIST("TIMEOUT")
          ElseIf ex.InnerException.Message.Contains("Network subsystem is down") Then
            Return ERRLIST("NETWORK")
          ElseIf ex.InnerException.Message.Contains("The requested address is not valid in this context") Then
            Return ERRLIST("NETWORK")
          ElseIf ex.InnerException.Message.Contains("System call failed") Then
            Return "System call failed. Please check your installation."
          ElseIf ex.InnerException.Message.Contains("TLS Support not available") Then
            Return ERRLIST("TLS")
          Else
            If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
            Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex.InnerException)
          End If
        ElseIf ex.Message.Contains("SendFailure") Then
          If ex.InnerException.Message.Contains("Error writing headers") Then
            If ex.InnerException.InnerException.Message.Contains("The socket is not connected") Then
              Return ERRLIST("ABORTED")
            ElseIf ex.InnerException.InnerException.Message.Contains("The socket has been shut down") Then
              Return ERRLIST("CLOSED")
            ElseIf ex.InnerException.InnerException.Message.Contains("The authentication or decryption has failed") Then
              Return ERRLIST("TLS")
            Else
              If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
              Return AppendErrorDetails(ERRLIST("CLOSED"), ex.InnerException)
            End If
          ElseIf ex.InnerException.Message.Contains("Unsupported security protocol type") Then
            Return ERRLIST("TLS")
          ElseIf ex.InnerException.Message.Contains("The authentication or decryption has failed") Then
            Return ERRLIST("TLS")
          ElseIf ex.InnerException.Message.Contains("Cannot access a disposed object") Then
            Return ERRLIST("ABORTED")
          Else
            If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
            Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex.InnerException)
          End If
        ElseIf ex.Message.Contains("SecureChannelFailure") Then
          Return ERRLIST("TLS")
        Else
          If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
          Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex, ":"c)
        End If
      ElseIf ex.Message.StartsWith("An error occurred performing a WebClient request") Then
        If ex.InnerException.Message.StartsWith("Object reference not set to an instance of an object") Then
          Return ERRLIST("ABORTED")
        ElseIf ex.InnerException.Message.StartsWith("The object was used after being disposed") Then
          Return ERRLIST("ABORTED")
        ElseIf ex.InnerException.Message.StartsWith("Thread was being aborted") Then
          Return ERRLIST("ABORTED")
        ElseIf ex.InnerException.Message.StartsWith("Exception has been thrown by the target of an invocation") Then
          If ex.InnerException.InnerException IsNot Nothing Then
            If ex.InnerException.InnerException.Message.StartsWith("Thread was being aborted") Then
              Return ERRLIST("ABORTED")
            Else
              If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
              Return AppendErrorDetails(ERRLIST("CLOSED"), ex.InnerException.InnerException)
            End If
          Else
            Return ERRLIST("ABORTED")
          End If
        Else
          If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
          Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex.InnerException)
        End If
      ElseIf ex.Message.StartsWith("System.Net.WebException:") Then
        If ex.Message.Contains("An exception occurred during a WebClient request") Then
          If ex.Message.Contains("The object was used after being disposed") Then
            Return ERRLIST("ABORTED")
          Else
            If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
            Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex, ":"c)
          End If
        Else
          If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
          Return AppendErrorDetails(ERRLIST("UNKNOWN"), ex, ":"c)
        End If
      Else
        If Not String.IsNullOrEmpty(dataPath) Then reportHandler.BeginInvoke(ex, dataPath, Nothing, Nothing)
        Return AppendErrorDetails(ex.Message, ex)
      End If
    End If
  End Function
  Private Shared Function AppendErrorDetails(Message As String, ex As System.Exception, Optional skipTo As Char = vbNullChar) As String
    If ex Is Nothing Then Return Message
    If ex.InnerException Is Nothing Then
      If skipTo = vbNullChar Then
        Return Message & " (" & ex.Message & ")"
      Else
        Return Message & " (" & ex.Message.Substring(ex.Message.IndexOf(skipTo) + 1).Trim & ")"
      End If
    End If
    If ex.InnerException.InnerException Is Nothing Then
      If ex.Message.Contains(ex.InnerException.Message) Then
        If skipTo = vbNullChar Then
          Return Message & " (" & ex.Message & ")"
        Else
          Return Message & " (" & ex.Message.Substring(ex.Message.IndexOf(skipTo) + 1).Trim & ")"
        End If
      Else
        If skipTo = vbNullChar Then
          Return Message & vbNewLine & ex.ToString
        Else
          Return Message & vbNewLine & ex.ToString.Substring(ex.ToString.IndexOf(skipTo) + 1).Trim
        End If
      End If
    End If
    Dim sEX() As String = Split(ex.ToString, vbNewLine)
    If sEX.Length > 3 Then
      If skipTo = vbNullChar Then
        Return Message & vbNewLine & sEX(0) & vbNewLine & sEX(1) & vbNewLine & sEX(2) & "..."
      Else
        Return Message & vbNewLine & sEX(0).Substring(sEX(0).IndexOf(skipTo) + 1).Trim & vbNewLine & sEX(1) & vbNewLine & sEX(2) & "..."
      End If
    Else
      If skipTo = vbNullChar Then
        Return Message & vbNewLine & ex.ToString
      Else
        Return Message & vbNewLine & ex.ToString.Substring(ex.ToString.IndexOf(skipTo) + 1).Trim
      End If
    End If
  End Function
  Private Delegate Sub ReportSocketErrorInvoker(ex As Exception, DataPath As String)
  ''' <summary>
  ''' Saves an <see cref="Exception" /> to a network error log which will later be reported.
  ''' </summary>
  ''' <param name="ex">The exception which needs to be reported.</param>
  ''' <param name="dataPath">The folder where socket errors will be saved to.</param>
  ''' <remarks>See Also: <seealso cref="NetworkErrorToString" />, <seealso cref="SendSocketErrors" />.</remarks>
  Public Shared Sub ReportSocketError(ex As Exception, dataPath As String)
    Dim ReportList As String = dataPath & "\sckerrs.log"
    If String.IsNullOrEmpty(ex.Message) Then Return
    Dim sErrMsg As String = ex.Message
    If ex.InnerException IsNot Nothing AndAlso Not String.IsNullOrEmpty(ex.InnerException.Message) Then
      sErrMsg &= vbNewLine & ex.InnerException.Message
      If ex.InnerException.InnerException IsNot Nothing AndAlso Not String.IsNullOrEmpty(ex.InnerException.InnerException.Message) Then sErrMsg &= vbNewLine & ex.InnerException.InnerException.Message
    End If
    If IO.File.Exists(ReportList) Then
      If InUseChecker(ReportList, FileAccess.ReadWrite) Then
        My.Computer.FileSystem.WriteAllText(ReportList, sErrMsg & vbNewLine & vbNewLine, True)
      End If
    Else
      My.Computer.FileSystem.WriteAllText(ReportList, sErrMsg & vbNewLine & vbNewLine, False)
    End If
    SendSocketErrors(dataPath)
  End Sub
  ''' <summary>
  ''' Loads network errors from a log file and reports them to the RealityRipple.com server.
  ''' </summary>
  ''' <param name="dataPath">The folder where socket errors will be loaded from.</param>
  ''' <remarks>See Also: <seealso cref="ReportSocketError" /></remarks>
  Public Shared Sub SendSocketErrors(dataPath As String)
    If lastSocketErrSend > 0 AndAlso TickCount() - lastSocketErrSend < 10 * 60 * 1000 Then Return
    Dim ReportList As String = dataPath & "\sckerrs.log"
    If IO.File.Exists(ReportList) Then
      Dim reports As New Collections.Generic.List(Of String)(Split(My.Computer.FileSystem.ReadAllText(ReportList), vbNewLine & vbNewLine))
      For I As Integer = reports.Count - 1 To 0 Step -1
        Dim err As String = reports(I)
        If Not String.IsNullOrEmpty(err) Then
          Dim e As String = err.Replace(vbNewLine, " - ")
          Try
            Dim sckUpload As New WebClientEx(dataPath)
            sckUpload.KeepAlive = False
            Dim params As New Collections.Specialized.NameValueCollection
            params.Add("e", e)
            lastSocketErrSend = TickCount()
            Dim sRet As String = sckUpload.UploadValues("http://wb.realityripple.com/errmsgs.php", "POST", params)
            If sRet = "e exists" Or sRet = "e added" Then reports.RemoveAt(I)
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
  ''' <summary>
  ''' Clip down the length of a URL to arround 125 characters.
  ''' </summary>
  ''' <param name="url">The address you'd like to cut down.</param>
  ''' <returns>A smaller version of the address.</returns>
  Public Shared Function TruncateURL(url As String) As String
    Const maxLen As Integer = 125
    If url.Length < maxLen Then Return url
    If url.Contains("?") Then
      If url.Substring(url.IndexOf("?") + 1).Contains("=") Then
        url = url.Substring(0, url.IndexOf("=", url.IndexOf("?") + 1) + ((maxLen - 4) - url.IndexOf("=", url.IndexOf("?") + 1))) & "..."
      Else
        url = url.Substring(0, url.IndexOf("?") + ((maxLen - 4) - url.IndexOf("?"))) & "..."
      End If
    End If
    If url.Length < maxLen Then Return url
    If url.Contains("/") Then
      Dim endingBit As String = url.Substring(url.LastIndexOf("/"))
      url = url.Substring(0, url.Substring(0, maxLen - endingBit.Length).LastIndexOf("/") + 1) & "..." & endingBit
    End If
    Return url
  End Function
  ''' <summary>
  ''' Converts a <see cref="localRestrictionTracker.SatHostTypes" /> value to a simple string.
  ''' </summary>
  ''' <param name="hostType">A Satellite Internet host type enumeration.</param>
  ''' <returns>If the <paramref name="hostType" /> is a valid <see cref="localRestrictionTracker.SatHostTypes" />, the return will be a three-letter value. Otherwise, it will be the letter &quot;O&quot;.</returns>
  ''' <remarks>
  '''  <para>WildBlue_LEGACY: &quot;WBL&quot;</para>
  '''  <para>WildBlue_EXEDE: &quot;WBX&quot;</para>
  '''  <para>RuralPortal_LEGACY: &quot;RPL&quot;</para>
  '''  <para>RuralPortal_EXEDE: &quot;RPX&quot;</para>
  '''  <para>Dish_EXEDE: &quot;DNX&quot;</para>
  '''  <para>Other Values: &quot;O&quot;</para>
  '''  <para>See Also: <seealso cref="StringToHostType" /></para>
  ''' </remarks>
  Public Shared Function HostTypeToString(hostType As localRestrictionTracker.SatHostTypes) As String
    Select Case hostType
      Case localRestrictionTracker.SatHostTypes.WildBlue_LEGACY : Return "WBL"
      Case localRestrictionTracker.SatHostTypes.WildBlue_EXEDE : Return "WBX"
      Case localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY : Return "RPL"
      Case localRestrictionTracker.SatHostTypes.RuralPortal_EXEDE : Return "RPX"
      Case localRestrictionTracker.SatHostTypes.Dish_EXEDE : Return "DNX"
      Case Else : Return "O"
    End Select
  End Function
  ''' <summary>
  ''' Converts a simple string value to a <see cref="localRestrictionTracker.SatHostTypes" />.
  ''' </summary>
  ''' <param name="host">The string to be converted to a <see cref="localRestrictionTracker.SatHostTypes" /> enumeration value.</param>
  ''' <returns>The closest <see cref="localRestrictionTracker.SatHostTypes" /> match detected.</returns>
  ''' <remarks>
  '''  <para>These values are <i>not</i> case-sensitive.</para>
  '''  <para>&quot;WBL&quot;, &quot;WildBlue&quot;: WildBlue_LEGACY</para>
  '''  <para>&quot;WBX&quot;, &quot;WBV&quot;, &quot;Exede&quot;: WildBlue_EXEDE</para>
  '''  <para>&quot;RPL&quot;, &quot;RuralPortal&quot;: RuralPortal_LEGACY</para>
  '''  <para>&quot;RPX&quot;: RuralPortal_EXEDE</para>
  '''  <para>&quot;DNX&quot;, &quot;Dish&quot;, &quot;DishNet&quot;: Dish_EXEDE</para>
  '''  <para>Other Values: Other</para>
  '''  <para>See Also: <seealso cref="HostTypeToString" /></para>
  ''' </remarks>
  Public Shared Function StringToHostType(host As String) As localRestrictionTracker.SatHostTypes
    Select Case host.ToUpper
      Case "WBL" : Return localRestrictionTracker.SatHostTypes.WildBlue_LEGACY
      Case "WBX", "WBV" : Return localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
      Case "RPL" : Return localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY
      Case "RPX" : Return localRestrictionTracker.SatHostTypes.RuralPortal_EXEDE
      Case "DNX" : Return localRestrictionTracker.SatHostTypes.Dish_EXEDE
      Case "WILDBLUE" : Return localRestrictionTracker.SatHostTypes.WildBlue_LEGACY
      Case "EXEDE" : Return localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
      Case "DISH", "DISHNET" : Return localRestrictionTracker.SatHostTypes.Dish_EXEDE
      Case "RURALPORTAL" : Return localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY
      Case Else : Return localRestrictionTracker.SatHostTypes.Other
    End Select
  End Function
  ''' <summary>
  ''' Attempts to see if a file is in use, waiting for it to be freed.
  ''' </summary>
  ''' <param name="path">The exact path to the file which needs to be checked.</param>
  ''' <param name="access">Write permissions required for checking.</param>
  ''' <returns><c>True</c> on available, <c>False</c> on in use.</returns>
  ''' <remarks>This function can take up to five seconds while waiting for successful file access.</remarks>
  Public Shared Function InUseChecker(path As String, access As IO.FileAccess) As Boolean
    If Not IO.File.Exists(path) Then Return True
    Dim iStart As Long = TickCount()
    Do
      Try
        Select Case access
          Case FileAccess.Read
            Using fs As FileStream = IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite Or FileShare.Delete)
              If fs.CanRead Then
                Return True
                Exit Do
              End If
            End Using
          Case FileAccess.Write, FileAccess.ReadWrite
            Using fs As FileStream = IO.File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite Or FileShare.Delete)
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
  ''' <summary>
  ''' Number of ticks since the computer started up.
  ''' </summary>
  Public Shared Function TickCount() As Long
    Return (Stopwatch.GetTimestamp / Stopwatch.Frequency) * 1000
  End Function
  ''' <summary>
  ''' Iterates through two <see cref="Byte">Byte arrays</see> to ensure their precise equality.
  ''' </summary>
  ''' <param name="inArray1">One of two byte arrays to check for exact equality.</param>
  ''' <param name="inArray2">The other of two byte arrays to check for exact equality.</param>
  ''' <returns><c>True</c> if both arrays are binary equal, <c>False</c> otherwise.</returns>
  ''' <remarks>If the lengths of the arrays is different, the result will be <c>False</c>.</remarks>
  Public Shared Function IterativeEqualityCheck(inArray1() As Byte, inArray2() As Byte) As Boolean
    If Not inArray1.Length = inArray2.Length Then Return False
    Dim isEqual As Boolean = True
    For I As Integer = 0 To inArray1.Length - 1
      If Not inArray1(I) = inArray2(I) Then
        isEqual = False
        Exit For
      End If
    Next
    Return isEqual
  End Function
  ''' <summary>
  ''' Common Language Runtime exact version number.
  ''' </summary>
  Public Shared Function GetCLRVersion() As String
    Dim sCLR As String = Nothing
    Dim tMONO As Type = Nothing
    Try
      tMONO = Type.GetType("Mono.Runtime")
    Catch ex As Exception
    End Try
    If tMONO Is Nothing Then
      Dim CLRRelease As String = "0"
      Try
        If My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\NET Framework Setup\NDP\v4").OpenSubKey("Full") IsNot Nothing Then
          CLRRelease = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full").GetValue("Release")
        ElseIf My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\NET Framework Setup\NDP\v4").OpenSubKey("Client") IsNot Nothing Then
          CLRRelease = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client").GetValue("Release")
        End If
      Catch ex As Exception
        CLRRelease = "0"
      End Try
      If String.IsNullOrEmpty(CLRRelease) Then CLRRelease = "0"
      sCLR = Environment.Version.ToString
      If Not CLRRelease = "0" Then sCLR &= "_" & CLRRelease
    Else
      Dim myMethods() As Reflection.MethodInfo = tMONO.GetMethods(Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Static)
      For Each mInfo As Reflection.MethodInfo In myMethods
        If mInfo Is Nothing Then Continue For
        Dim mName As String = "Unknown"
        Try
          mName = mInfo.Name
        Catch ex As Exception
          mName = "Unknown"
        End Try
        If Not mName = "GetDisplayName" Then Continue For
        Dim mInvoke As String = Nothing
        Try
          mInvoke = mInfo.Invoke(Nothing, Nothing)
        Catch ex As Exception
          mInvoke = Nothing
        End Try
        If Not String.IsNullOrEmpty(mInvoke) Then
          If mInvoke.Contains(" ") Then
            sCLR = mInvoke.Substring(0, mInvoke.IndexOf(" "c))
            Exit For
          End If
        End If
      Next
      If Not String.IsNullOrEmpty(sCLR) Then
        sCLR = "4.0.30319.17020_" & sCLR
      Else
        If Environment.Version.Major = 4 And Environment.Version.Minor = 0 And Environment.Version.Build = 30319 Then
          Select Case Environment.Version.Revision
            Case 17020 : sCLR = "4.0.30319.17020"
            Case 42000 : sCLR = "4.0.30319.17020_4.4"
            Case Else : sCLR = "4.0.30319.17020_" & Environment.Version.Revision
          End Select
        Else
          sCLR = Environment.Version.ToString
        End If
      End If
    End If
    Return sCLR
  End Function
  ''' <summary>
  ''' Common Language Runtime Name and Release version.
  ''' </summary>
  Public Shared Function GetCLRCleanVersion() As String
    Dim sVer As String = GetCLRVersion()
    If Not sVer.Substring(0, 9) = "4.0.30319" Then Return String.Format("Unknown Runtime ({0})", sVer)
    Dim clrID, clrID2 As Integer
    Dim monoID As String = Nothing
    If Not sVer.Substring(10).Contains("_") Then
      clrID = Val(sVer.Substring(10))
      clrID2 = 0
    Else
      Dim clrRel As String = sVer.Substring(10)
      Dim clrBld As String = clrRel.Substring(0, clrRel.IndexOf("_"))
      clrRel = clrRel.Substring(clrRel.IndexOf("_") + 1)
      clrID = Val(clrBld)
      clrID2 = Val(clrRel)
      monoID = clrRel
    End If
    If clrID = 17020 Then
      If Not String.IsNullOrEmpty(monoID) Then Return String.Format("MONO {0}", monoID)
      Return "MONO"
    End If
    If clrID < 17929 Then
      If clrID = 1 Then Return ".NET 4.0 RTM"
      If clrID = 269 Then Return ".NET 4.0 + MS12-035 GDR"
      If clrID = 276 Then Return ".NET 4.0.3 Runtime Update"
      If clrID = 296 Then Return ".NET 4.0 + MS12-074 GDR"
      If clrID = 544 Then Return ".NET 4.0 + MS12-035 LDR"
      If clrID = 1008 Then Return ".NET 4.0 + MS13-052 GDR"
      If clrID = 1022 Then Return ".NET 4.0 + MS14-009 GDR"
      If clrID = 1026 Then Return ".NET 4.0 + MS14-057 GDR"
      If clrID = 2034 Then Return ".NET 4.0 + MS14-009 LDR"
      Return String.Format(".NET 4.0 ({0})", clrID)
    End If
    If clrID < 18408 Then
      If clrID = 17929 Then Return ".NET 4.5 RTM"
      If clrID = 18063 Then Return ".NET 4.5 + MS14-009"
      Return String.Format(".NET 4.5 ({0})", clrID)
    End If
    If clrID < 34209 Then
      If clrID = 18408 Then Return ".NET 4.5.1"
      If clrID = 18444 Then Return ".NET 4.5.1 + MS14-009"
      If clrID = 34011 Then Return ".NET 4.5.1 for Windows 8 + MS14-009"
      If clrID = 34014 Then Return ".NET 4.5.1 for Windows 8.1"
      Return String.Format(".NET 4.5.1 ({0})", clrID)
    End If
    If clrID < 42000 Then
      If clrID = 34209 Then Return ".NET 4.5.2"
      If clrID = 35312 Then Return ".NET 4.5.2 (35312)"
      Return String.Format(".NET 4.5.2 ({0})", clrID)
    End If
    If clrID = 42000 Then
      If clrID2 = 0 Then Return ".NET 4.6"
      If clrID2 = 393295 Or clrID2 = 393297 Then Return ".NET 4.6" '  6004F 60051
      If clrID2 = 394254 Or clrID2 = 394271 Then Return ".NET 4.6.1" '6040E 6041F
      If clrID2 = 394802 Or clrID2 = 394806 Then Return ".NET 4.6.2" '60632 60636
      If clrID2 = 460798 Or clrID2 = 460805 Then Return ".NET 4.7" '  707FE 70805
      If clrID2 = 461308 Or clrID2 = 461310 Then Return ".NET 4.7.1" '709FC 709FE
      If clrID2 = 461808 Or clrID2 = 461814 Then Return ".NET 4.7.2" '70BF0 70BF6

      If clrID2 < 393297 Then Return String.Format(".NET 4.6 ({0})", clrID2)
      If clrID2 < 394271 Then Return String.Format(".NET 4.6.1 ({0})", clrID2)
      If clrID2 < 394806 Then Return String.Format(".NET 4.6.2 ({0})", clrID2)
      If clrID2 < 460805 Then Return String.Format(".NET 4.7 ({0})", clrID2)
      If clrID2 < 461310 Then Return String.Format(".NET 4.7.1 ({0})", clrID2)
      If clrID2 < 461814 Then Return String.Format(".NET 4.7.2 ({0})", clrID2)

      Dim clrAttempt As String = Hex(clrID2)
      clrAttempt = clrAttempt.Substring(0, clrAttempt.Length - 4)
      Dim iAVer As Integer = Integer.Parse(clrAttempt, Globalization.NumberStyles.HexNumber)
      Return (String.Format(".NET 4.{1} ({0})", clrID2, iAVer))
    End If
    If clrID2 > 0 Then Return String.Format(".NET Future Version ({0}.{1})", clrID, clrID2)
    Return String.Format(".NET Future Version ({0})", clrID)
  End Function
End Class
