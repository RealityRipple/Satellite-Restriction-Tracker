Public Class clsFavicon
  Implements IDisposable
  Private WithEvents wsNetTest As WebClientEx
  Public Class DownloadIconCompletedEventArgs
    Inherits EventArgs
    Public [Error] As Exception
    Public Icon32 As Image
    Public Icon16 As Image
    Public Sub New(icon16 As Image, icon32 As Image, Optional err As Exception = Nothing)
      Me.Icon16 = icon16
      Me.Icon32 = icon32
      Me.Error = err
    End Sub
  End Class
  Public Event DownloadIconCompleted(sender As Object, e As DownloadIconCompletedEventArgs)
  Public Sub New(URL As String)
    If String.IsNullOrEmpty(URL) Then Return
    Dim connectThread As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf BeginConnection))
    connectThread.Start(URL)
  End Sub
  Private Sub BeginConnection(o As Object)
    Dim URL As String = o
    If Not URL.Contains("://") Then URL = "http://" & URL
    Dim URI As Uri
    Try
      URI = New Uri(URL)
    Catch ex As Exception
      Return
    End Try
    ConnectToURL(URI, URL)
  End Sub
  Private Sub ConnectToURL(URL As Uri, Optional token As Object = Nothing)
    If URL.Host = "192.168.100.1" Then
      RaiseEvent DownloadIconCompleted(Me, New DownloadIconCompletedEventArgs(My.Resources.modem16, My.Resources.modem32))
      Return
    End If
    Try
      Dim urlRes = Net.Dns.GetHostAddresses(URL.Host)
      For Each addr In urlRes
        If addr.ToString = "192.168.100.1" Then
          RaiseEvent DownloadIconCompleted(Me, New DownloadIconCompletedEventArgs(My.Resources.modem16, My.Resources.modem32))
          Return
        End If
      Next
    Catch ex As Exception
    End Try
    Try
      wsNetTest = New WebClientEx
      wsNetTest.ErrorBypass = True
      wsNetTest.DownloadStringAsync(URL, token)
    Catch ex As Exception
      RaiseEvent DownloadIconCompleted(Me, New DownloadIconCompletedEventArgs(My.Resources.ico_err, My.Resources.advanced_nettest_error, New Exception("Failed to initialize connection to """ & URL.OriginalString & """!")))
    End Try
  End Sub
  Private Sub ConnectToFile(URL As Uri, Filename As String, Optional token As Object = Nothing)
    Try
      wsNetTest = New WebClientEx
      wsNetTest.ErrorBypass = True
      wsNetTest.DownloadFileAsync(URL, Filename, token)
    Catch ex As Exception
      RaiseEvent DownloadIconCompleted(Me, New DownloadIconCompletedEventArgs(My.Resources.ico_err, My.Resources.advanced_nettest_error, New Exception("Failed to initialize connection to """ & URL.OriginalString & """!")))
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
  Private Sub wsNetTest_DownloadFileCompleted(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs) Handles wsNetTest.DownloadFileCompleted
    If e.Error IsNot Nothing Then
      If e.UserState Is Nothing Then
        RaiseEvent DownloadIconCompleted(Me, New DownloadIconCompletedEventArgs(Nothing, Nothing, New Exception("Failed to get an icon.")))
      Else
        Dim pathURL As New Uri(e.UserState)
        ConnectToFile(New Uri(pathURL.Scheme & "://" & pathURL.Host & "/favicon.ico"), IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "srt_nettest_favicon.ico"))
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
      RaiseEvent DownloadIconCompleted(Me, New DownloadIconCompletedEventArgs(pctPNG16.Clone, pctPNG32.Clone))
    Else
      If e.UserState Is Nothing Then
        RaiseEvent DownloadIconCompleted(Me, New DownloadIconCompletedEventArgs(Nothing, Nothing, New Exception("Failed to read the icon.")))
      Else
        Dim pathURL As New Uri(e.UserState)
        ConnectToFile(New Uri(pathURL.Scheme & "://" & pathURL.Host & "/favicon.ico"), imgFile)
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
  Private Sub wsNetTest_DownloadStringCompleted(sender As Object, e As System.Net.DownloadStringCompletedEventArgs) Handles wsNetTest.DownloadStringCompleted
    If e.Error IsNot Nothing Then
      Dim defPathURL As New Uri(e.UserState)
      ConnectToFile(New Uri(defPathURL.Scheme & "://" & defPathURL.Host & "/favicon.ico"), IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "srt_nettest_favicon.ico"))
      Return
    ElseIf e.Cancelled Then
      Return
    End If
    Try
      Dim sHTML As String = e.Result
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
                    Dim oldURL As String = e.UserState
                    If oldURL.Contains("://") Then oldURL = oldURL.Substring(0, oldURL.IndexOf("://") + 1)
                    URL = oldURL & URL
                  Else
                    Dim oldURL As String = e.UserState
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
                  ConnectToFile(New Uri(URL), IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "srt_nettest_favicon.ico"), URL)
                  Return
                End If
              End If
            End If
          End If
        End If
      End If
    Catch ex As Exception
    End Try
    Dim pathURL As New Uri(e.UserState)
    ConnectToFile(New Uri(pathURL.Scheme & "://" & pathURL.Host & "/favicon.ico"), IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "srt_nettest_favicon.ico"))
  End Sub
#Region "IDisposable Support"
  Private disposedValue As Boolean
  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not Me.disposedValue Then
      If disposing Then
        If wsNetTest IsNot Nothing Then
          If wsNetTest.IsBusy Then wsNetTest.CancelAsync()
          wsNetTest.Dispose()
          wsNetTest = Nothing
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
