Public Class clsFavicon
  Implements IDisposable
  Private WithEvents wsFile As WebClientCore
  Public Delegate Sub DownloadIconCompletedCallback(icon16 As Image, icon32 As Image, token As Object, [Error] As Exception)
  Private c_callback As DownloadIconCompletedCallback
  Public Sub New(callback As DownloadIconCompletedCallback)
    c_callback = callback
  End Sub
  Public Sub Start(sAddr As String, token As Object)
    If String.IsNullOrEmpty(sAddr) Then Return
    Dim connectThread As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf BeginConnection))
    connectThread.Start({sAddr, token})
  End Sub
  Private Sub BeginConnection(obj As Object)
    Dim sAddr As String = obj(0)
    Dim token As Object = obj(1)
    If Not sAddr.Contains(Uri.SchemeDelimiter) Then sAddr = "http://" & sAddr
    Dim uURI As Uri
    Try
      uURI = New Uri(sAddr)
    Catch ex As Exception
      Return
    End Try
    ConnectToURL(uURI, token)
  End Sub
  Private Sub ConnectToURL(URI As Uri, token As Object)
    If URI.Host = "192.168.100.1" Then
      c_callback.Invoke(My.Resources.modem16, My.Resources.modem32, token, Nothing)
      Return
    End If
    Try
      Dim urlRes() As Net.IPAddress = Net.Dns.GetHostAddresses(URI.Host)
      For Each addr In urlRes
        If addr.ToString = "192.168.100.1" Then
          c_callback.Invoke(My.Resources.modem16, My.Resources.modem32, token, Nothing)
          Return
        End If
      Next
    Catch ex As Exception
    End Try
    Try
      Dim wsString As New WebClientEx
      wsString.ManualRedirect = False
      wsString.KeepAlive = False
      Dim sRet As String = wsString.DownloadString(URI.OriginalString)
      If sRet.StartsWith("Error: ") Then
        Try
          ConnectToFile(New Uri(URI.Scheme & URI.SchemeDelimiter & URI.Host & "/favicon.ico"), IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "srt_nettest_favicon.ico"), token, False)
        Catch ex As Exception
        End Try
        Return
      End If
      Try
        Dim sHTML As String = sRet
        If sHTML.ToUpperInvariant.Contains("SHORTCUT ICON") Then sHTML = Replace(sHTML, "shortcut icon", "icon", , , CompareMethod.Text)
        If sHTML.ToUpperInvariant.Contains("REL=""ICON""") Then
          If sHTML.Substring(0, sHTML.ToUpperInvariant.IndexOf("REL=""ICON""")).Contains("<") Then
            sHTML = sHTML.Substring(sHTML.Substring(0, sHTML.ToUpperInvariant.IndexOf("REL=""ICON""")).LastIndexOf("<"))
            If sHTML.Contains(">") Then
              sHTML = sHTML.Substring(0, sHTML.IndexOf(">") + 1)
              If sHTML.ToUpperInvariant.Contains("HREF") Then
                sHTML = sHTML.Substring(sHTML.IndexOf("href", StringComparison.OrdinalIgnoreCase))
                If sHTML.Contains("""") Then
                  sHTML = sHTML.Substring(sHTML.IndexOf("""") + 1)
                  If sHTML.Contains("""") Then
                    Dim sAddr As String = sHTML.Substring(0, sHTML.IndexOf(""""))
                    If sAddr.Contains(URI.SchemeDelimiter) Then

                    ElseIf sAddr.Contains("//") Then
                      Dim oldAddr As String = URI.OriginalString
                      If oldAddr.Contains(URI.SchemeDelimiter) Then oldAddr = oldAddr.Substring(0, oldAddr.IndexOf(URI.SchemeDelimiter) + 1)
                      sAddr = oldAddr & sAddr
                    Else
                      Dim oldURL As String = URI.OriginalString
                      If Not oldURL.EndsWith("/") And oldURL.IndexOf("/", oldURL.IndexOf("//") + 2) > -1 Then oldURL = oldURL.Substring(0, oldURL.LastIndexOf("/") + 1)
                      If sAddr.StartsWith("/") Then
                        If oldURL.IndexOf("/", oldURL.IndexOf("//") + 2) > -1 Then oldURL = oldURL.Substring(0, oldURL.IndexOf("/", oldURL.IndexOf("//") + 2))
                        sAddr = oldURL & sAddr
                      ElseIf oldURL.EndsWith("/") Then
                        sAddr = oldURL & sAddr
                      Else
                        sAddr = oldURL & "/" & sAddr
                      End If
                    End If
                    Try
                      ConnectToFile(New Uri(sAddr), IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "srt_nettest_favicon.ico"), token, True)
                    Catch ex As Exception

                    End Try
                    Return
                  End If
                End If
              End If
            End If
          End If
        End If
      Catch ex As Exception
      End Try
      Try
        ConnectToFile(New Uri(URI.Scheme & URI.SchemeDelimiter & URI.Host & "/favicon.ico"), IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "srt_nettest_favicon.ico"), token, True)
      Catch ex As Exception
      End Try
    Catch ex As Exception
      c_callback.Invoke(My.Resources.ico_err, My.Resources.advanced_nettest_error, token, New Exception("Failed to initialize connection to """ & URI.OriginalString & """!"))
    End Try
  End Sub
  Private Sub ConnectToFile(URL As Uri, Filename As String, token As Object, trySimpler As Boolean)
    Try
      wsFile = New WebClientCore
      wsFile.ManualRedirect = False
      wsFile.KeepAlive = False
      Dim tSocket As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf DownloadFile))
      tSocket.Start({URL, Filename, token, trySimpler})
    Catch ex As Exception
      c_callback.Invoke(My.Resources.ico_err, My.Resources.advanced_nettest_error, token, New Exception("Failed to initialize connection to """ & URL.OriginalString & """!"))
    End Try
  End Sub
  Private Sub DownloadFile(state As Object)
    Dim URI As Uri = state(0)
    Dim Filename As String = state(1)
    Dim token As Object = state(2)
    Dim trySimpler As Boolean = state(3)
    Try
      wsFile.DownloadFileAsync(URI, Filename, {token, URI, trySimpler})
    Catch ex As Exception
      c_callback.Invoke(My.Resources.ico_err, My.Resources.advanced_nettest_error, token, New Exception("Failed to initialize connection to """ & URI.OriginalString & """!"))
    End Try
  End Sub
  Private Shared Function GenerateCloneImage(fromImage As Image, width As Integer, height As Integer) As Image
    Using newImage As New Bitmap(width, height)
      Using g As Graphics = Graphics.FromImage(newImage)
        g.DrawImage(fromImage, 0, 0, width, height)
      End Using
      Return newImage.Clone
    End Using
  End Function
  Private Shared Function GenerateCloneImage(fromIcon As Icon) As Image
    Using newImage As New Bitmap(fromIcon.Width, fromIcon.Height)
      Using g As Graphics = Graphics.FromImage(newImage)
        g.DrawIcon(fromIcon, 0, 0)
      End Using
      Return newImage.Clone
    End Using
  End Function
  Private Sub wsFile_DownloadFileCompleted(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs) Handles wsFile.DownloadFileCompleted
    Dim Token As Object = e.UserState(0)
    Dim URI As Uri = e.UserState(1)
    Dim trySimpler As Boolean = e.UserState(2)
    If e.Error IsNot Nothing Then
      If Not trySimpler Then
        c_callback.Invoke(Nothing, Nothing, Token, New Exception("Failed to get an icon."))
      Else
        Try
          ConnectToFile(New Uri(URI.Scheme & URI.SchemeDelimiter & URI.Host & "/favicon.ico"), IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "srt_nettest_favicon.ico"), Token, False)
        Catch ex As Exception
        End Try
      End If
      Return
    ElseIf e.Cancelled Then
      Return
    End If
    Dim imgFile As String = IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "srt_nettest_favicon.ico")
    Dim pctPNG16 As New Bitmap(16, 16)
    Dim pctPNG32 As New Bitmap(32, 32)
    Dim didOK As Boolean = True
    Dim imgHeader(3) As Byte
    Using iStream As IO.FileStream = IO.File.OpenRead(imgFile)
      iStream.Read(imgHeader, 0, 4)
    End Using
    Try
      Select Case BitConverter.ToUInt32(imgHeader, 0)
        Case &H10000
          Using ico As New Icon(imgFile, 16, 16)
            pctPNG16 = GenerateCloneImage(ico)
          End Using
          Using ico As New Icon(imgFile, 32, 32)
            pctPNG32 = GenerateCloneImage(ico)
          End Using
        Case &H474E5089, &H38464947
          Using ico As Image = Image.FromFile(imgFile)
            pctPNG16 = GenerateCloneImage(ico, 16, 16)
            pctPNG32 = GenerateCloneImage(ico, 32, 32)
          End Using
        Case Else
          'TODO: Handle other image header types that may be used as favicons
          Debug.Print("Unknown Header ID: " & Hex(BitConverter.ToUInt32(imgHeader, 0)))
          Using ico As Image = Image.FromFile(imgFile)
            pctPNG16 = GenerateCloneImage(ico, 16, 16)
            pctPNG32 = GenerateCloneImage(ico, 32, 32)
          End Using
      End Select
    Catch ex As Exception
      didOK = False
    End Try
    If IO.File.Exists(imgFile) Then IO.File.Delete(imgFile)
    If didOK Then
      c_callback.Invoke(pctPNG16.Clone, pctPNG32.Clone, Token, Nothing)
    Else
      If Not trySimpler Then
        c_callback.Invoke(Nothing, Nothing, Token, New Exception("Failed to read the icon."))
      Else
        Try
          ConnectToFile(New Uri(URI.Scheme & URI.SchemeDelimiter & URI.Host & "/favicon.ico"), imgFile, Token, False)
        Catch ex As Exception
          Return
        End Try
      End If
    End If
    If pctPNG16 IsNot Nothing Then
      pctPNG16.Dispose()
      pctPNG16 = Nothing
    End If
    If pctPNG32 IsNot Nothing Then
      pctPNG32.Dispose()
      pctPNG32 = Nothing
    End If
  End Sub
#Region "IDisposable Support"
  Private disposedValue As Boolean
  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not Me.disposedValue Then
      If disposing Then
        If wsFile IsNot Nothing Then
          If wsFile.IsBusy Then wsFile.CancelAsync()
          wsFile.Dispose()
          wsFile = Nothing
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
