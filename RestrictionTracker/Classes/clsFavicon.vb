Public Class clsFavicon
  Implements IDisposable
  Private WithEvents wsFile As WebClientCore
  Public Delegate Sub DownloadIconCompletedCallback(icon16 As Image, icon32 As Image, token As Object, [Error] As Exception)
  Private c_callback As DownloadIconCompletedCallback
  Public Sub New(URL As String, callback As DownloadIconCompletedCallback, token As Object)
    If String.IsNullOrEmpty(URL) Then Return
    c_callback = callback
    Dim connectThread As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf BeginConnection))
    connectThread.Start({URL, token})
  End Sub
  Private Sub BeginConnection(obj As Object)
    Dim URL As String = obj(0)
    Dim token As Object = obj(1)
    If Not URL.Contains("://") Then URL = "http://" & URL
    Dim URI As Uri
    Try
      URI = New Uri(URL)
    Catch ex As Exception
      Return
    End Try
    ConnectToURL(URI, token)
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
      Dim sRet As String = wsString.DownloadString(URI.OriginalString)
      If sRet.StartsWith("Error: ") Then
        Try
          ConnectToFile(New Uri(URI.Scheme & "://" & URI.Host & "/favicon.ico"), IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "srt_nettest_favicon.ico"), token, False)
        Catch ex As Exception
        End Try
        Return
      End If
      Try
        Dim sHTML As String = sRet
        If sHTML.ToLower.Contains("shortcut icon") Then sHTML = Replace(sHTML, "shortcut icon", "icon", , , CompareMethod.Text)
        If sHTML.ToLower.Contains("rel=""icon""") Then
          If sHTML.Substring(0, sHTML.ToLower.IndexOf("rel=""icon""")).Contains("<") Then
            sHTML = sHTML.Substring(sHTML.Substring(0, sHTML.ToLower.IndexOf("rel=""icon""")).LastIndexOf("<"))
            If sHTML.Contains(">") Then
              sHTML = sHTML.Substring(0, sHTML.IndexOf(">") + 1)
              If sHTML.ToLower.Contains("href") Then
                sHTML = sHTML.Substring(sHTML.IndexOf("href"))
                If sHTML.Contains("""") Then
                  sHTML = sHTML.Substring(sHTML.IndexOf("""") + 1)
                  If sHTML.Contains("""") Then
                    Dim URL As String = sHTML.Substring(0, sHTML.IndexOf(""""))
                    If URL.Contains("://") Then

                    ElseIf URL.Contains("//") Then
                      Dim oldURL As String = URI.OriginalString
                      If oldURL.Contains("://") Then oldURL = oldURL.Substring(0, oldURL.IndexOf("://") + 1)
                      URL = oldURL & URL
                    Else
                      Dim oldURL As String = URI.OriginalString
                      If Not oldURL.EndsWith("/") And oldURL.IndexOf("/", oldURL.IndexOf("//") + 2) > -1 Then oldURL = oldURL.Substring(0, oldURL.LastIndexOf("/") + 1)
                      If URL.StartsWith("/") Then
                        If oldURL.IndexOf("/", oldURL.IndexOf("//") + 2) > -1 Then oldURL = oldURL.Substring(0, oldURL.IndexOf("/", oldURL.IndexOf("//") + 2))
                        URL = oldURL & URL
                      ElseIf oldURL.EndsWith("/") Then
                        URL = oldURL & URL
                      Else
                        URL = oldURL & "/" & URL
                      End If
                    End If
                    Try
                      ConnectToFile(New Uri(URL), IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "srt_nettest_favicon.ico"), token, True)
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
        ConnectToFile(New Uri(URI.Scheme & "://" & URI.Host & "/favicon.ico"), IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "srt_nettest_favicon.ico"), token, True)
      Catch ex As Exception
      End Try
    Catch ex As Exception
      c_callback.Invoke(My.Resources.ico_err, My.Resources.advanced_nettest_error, token, New Exception("Failed to initialize connection to """ & URI.OriginalString & """!"))
    End Try
  End Sub
  Private Sub ConnectToFile(URL As Uri, Filename As String, token As Object, trySimpler As Boolean)
    Try
      wsFile = New WebClientCore
      Dim tmrSocket As New Threading.Timer(New Threading.TimerCallback(AddressOf DownloadFile), New Object() {URL, Filename, token, trySimpler}, 250, System.Threading.Timeout.Infinite)
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
  Private Function GenerateCloneImage(fromImage As Image, width As Integer, height As Integer) As Image
    Using newImage As New Bitmap(width, height)
      Using g As Graphics = Graphics.FromImage(newImage)
        g.DrawImage(fromImage, 0, 0, width, height)
      End Using
      Return newImage.Clone
    End Using
  End Function
  Private Function GenerateCloneImage(fromIcon As Icon) As Image
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
          ConnectToFile(New Uri(URI.Scheme & "://" & URI.Host & "/favicon.ico"), IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "srt_nettest_favicon.ico"), Token, False)
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
          ConnectToFile(New Uri(URI.Scheme & "://" & URI.Host & "/favicon.ico"), imgFile, Token, False)
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
