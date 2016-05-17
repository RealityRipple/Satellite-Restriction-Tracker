Public Class WebClientCore
  Inherits Net.WebClient
  Public Sub New(useEvents As Boolean)
    MyBase.New()
    c_Events = useEvents
    c_CookieJar = New Net.CookieContainer
    c_Timeout = 120
    c_RWTimeout = 300
    c_HTVer = Net.HttpVersion.Version11
    c_ErrorBypass = True
    c_ManualRedirect = True
    System.Net.ServicePointManager.Expect100Continue = False
  End Sub
  Sub New()
    MyBase.New()
    c_Events = False
    c_CookieJar = New Net.CookieContainer
    c_Timeout = 120
    c_RWTimeout = 300
    c_HTVer = Net.HttpVersion.Version11
    c_ErrorBypass = True
    c_ManualRedirect = True
    System.Net.ServicePointManager.Expect100Continue = False
  End Sub
  Public Class ErrorEventArgs
    Inherits EventArgs
    Public [Error] As Exception
    Public Sub New(Err As Exception)
      [Error] = Err
    End Sub
  End Class
  Private c_CookieJar As Net.CookieContainer
  Public Property CookieJar As Net.CookieContainer
    Get
      Return c_CookieJar
    End Get
    Set(value As Net.CookieContainer)
      c_CookieJar = value
    End Set
  End Property
  Private c_ResponseURI As Uri
  Public ReadOnly Property ResponseURI As Uri
    Get
      Return c_ResponseURI
    End Get
  End Property
  Private c_Timeout As Integer
  Public Property Timeout As Integer
    Get
      Return c_Timeout
    End Get
    Set(value As Integer)
      c_Timeout = value
    End Set
  End Property
  Private c_RWTimeout As Integer
  Public Property ReadWriteTimeout As Integer
    Get
      Return c_RWTimeout
    End Get
    Set(value As Integer)
      c_RWTimeout = value
    End Set
  End Property
  Private c_HTVer As Version
  Public Property HTTPVersion As Version
    Get
      Return c_HTVer
    End Get
    Set(value As Version)
      c_HTVer = value
    End Set
  End Property
  Private c_ErrorBypass As Boolean
  Public Property ErrorBypass As Boolean
    Get
      Return c_ErrorBypass
    End Get
    Set(value As Boolean)
      c_ErrorBypass = value
    End Set
  End Property
  Private c_ManualRedirect As Boolean
  Public Property ManualRedirect As Boolean
    Get
      Return c_ManualRedirect
    End Get
    Set(value As Boolean)
      c_ManualRedirect = value
    End Set
  End Property
  Private c_Events As Boolean
  Public Event Failure(sender As Object, e As ErrorEventArgs)
  Protected Overrides Function GetWebRequest(address As System.Uri) As System.Net.WebRequest
    Try
      Dim request As Net.WebRequest = MyBase.GetWebRequest(address)
      If request.GetType Is GetType(Net.HttpWebRequest) Then
        CType(request, Net.HttpWebRequest).UserAgent = "Mozilla/5.0 (" & Environment.OSVersion.VersionString & "; CLR: " & Environment.Version.ToString & ") " & My.Application.Info.ProductName.Replace(" ", "") & "/" & My.Application.Info.Version.ToString
        CType(request, Net.HttpWebRequest).ReadWriteTimeout = c_RWTimeout * 1000
        CType(request, Net.HttpWebRequest).Timeout = c_Timeout * 1000
        CType(request, Net.HttpWebRequest).CookieContainer = c_CookieJar
        CType(request, Net.HttpWebRequest).AllowAutoRedirect = Not c_ManualRedirect
        CType(request, Net.HttpWebRequest).KeepAlive = False
        CType(request, Net.HttpWebRequest).ProtocolVersion = HTTPVersion
        CType(request, Net.HttpWebRequest).CachePolicy = New Net.Cache.HttpRequestCachePolicy(Net.Cache.HttpRequestCacheLevel.BypassCache)
      End If
      Return request
    Catch ex As Net.WebException
      MyBase.CancelAsync()
      If c_Events Then
        RaiseEvent Failure(Me, New ErrorEventArgs(ex))
      Else
        Throw ex
      End If
      Return Nothing
    End Try
  End Function
  Protected Overrides Function GetWebResponse(request As System.Net.WebRequest) As System.Net.WebResponse
    Try
      Dim response As Net.WebResponse = MyBase.GetWebResponse(request)
      c_ResponseURI = response.ResponseUri
      If response.GetType Is GetType(Net.HttpWebResponse) AndAlso Not String.IsNullOrEmpty(CType(response, Net.HttpWebResponse).CharacterSet) Then
        Dim charSet As String = CType(response, Net.HttpWebResponse).CharacterSet
        Try
          Me.Encoding = System.Text.Encoding.GetEncoding(charSet)
        Catch ex As Exception
          Me.Encoding = System.Text.Encoding.GetEncoding(LATIN_1)
        End Try
      ElseIf response.ContentType.ToLower.Contains("charset=") Then
        Dim charSet As String = response.ContentType.Substring(response.ContentType.ToLower.IndexOf("charset"))
        charSet = charSet.Substring(charSet.IndexOf("=") + 1)
        If charSet.Contains(";") Then charSet = charSet.Substring(0, charSet.IndexOf(";"))
        Try
          Me.Encoding = System.Text.Encoding.GetEncoding(charSet)
        Catch ex As Exception
          Me.Encoding = System.Text.Encoding.GetEncoding(LATIN_1)
        End Try
      End If
      Return response
    Catch ex As Net.WebException
      If c_ErrorBypass Then
        Dim response As Net.WebResponse = ex.Response
        If response IsNot Nothing Then
          c_ResponseURI = response.ResponseUri
          Return response
        End If
      End If
      If ex.Message = "The request was aborted: The request was canceled." Then Return Nothing
      MyBase.CancelAsync()
      If c_Events Then
        RaiseEvent Failure(Me, New ErrorEventArgs(ex))
      Else
        Throw ex
      End If
      Return Nothing
    End Try
  End Function
  Protected Overrides Function GetWebResponse(request As System.Net.WebRequest, result As System.IAsyncResult) As System.Net.WebResponse
    Try
      Dim response As Net.WebResponse = MyBase.GetWebResponse(request, result)
      c_ResponseURI = response.ResponseUri
      If response.GetType Is GetType(Net.HttpWebResponse) AndAlso Not String.IsNullOrEmpty(CType(response, Net.HttpWebResponse).CharacterSet) Then
        Dim charSet As String = CType(response, Net.HttpWebResponse).CharacterSet
        Try
          Me.Encoding = System.Text.Encoding.GetEncoding(charSet)
        Catch ex As Exception
          Me.Encoding = System.Text.Encoding.GetEncoding(LATIN_1)
        End Try
      ElseIf response.ContentType.ToLower.Contains("charset=") Then
        Dim charSet As String = response.ContentType.Substring(response.ContentType.ToLower.IndexOf("charset"))
        charSet = charSet.Substring(charSet.IndexOf("=") + 1)
        If charSet.Contains(";") Then charSet = charSet.Substring(0, charSet.IndexOf(";"))
        Try
          Me.Encoding = System.Text.Encoding.GetEncoding(charSet)
        Catch ex As Exception
          Me.Encoding = System.Text.Encoding.GetEncoding(LATIN_1)
        End Try
      End If
      Return response
    Catch ex As Net.WebException
      If c_ErrorBypass Then
        Dim response As Net.WebResponse = ex.Response
        If response IsNot Nothing Then
          c_ResponseURI = response.ResponseUri
          Return response
        End If
      End If
      If ex.Message = "The request was aborted: The request was canceled." Then Return Nothing
      MyBase.CancelAsync()
      If c_Events Then
        RaiseEvent Failure(Me, New ErrorEventArgs(ex))
      Else
        Throw ex
      End If
      Return Nothing
    End Try
  End Function
End Class

Public Class WebClientEx
  Private c_Timeout As Integer
  Public Property Timeout As Integer
    Get
      Return c_Timeout
    End Get
    Set(value As Integer)
      c_Timeout = value
    End Set
  End Property
  Private c_RWTimeout As Integer
  Public Property ReadWriteTimeout As Integer
    Get
      Return c_RWTimeout
    End Get
    Set(value As Integer)
      c_RWTimeout = value
    End Set
  End Property
  Private c_Proxy As Net.IWebProxy
  Public Property Proxy As Net.IWebProxy
    Get
      Return c_Proxy
    End Get
    Set(value As Net.IWebProxy)
      c_Proxy = value
    End Set
  End Property
  Private c_Jar As Net.CookieContainer
  Public Property CookieJar As Net.CookieContainer
    Get
      Return c_Jar
    End Get
    Set(value As Net.CookieContainer)
      c_Jar = value
    End Set
  End Property
  Private c_Encoding As System.Text.Encoding
  Public Property Encoding As System.Text.Encoding
    Get
      Return c_Encoding
    End Get
    Set(value As System.Text.Encoding)
      c_Encoding = value
    End Set
  End Property
  Private c_ResponseURI As Uri
  Public ReadOnly Property ResponseURI As Uri
    Get
      Return c_ResponseURI
    End Get
  End Property
  Private c_ErrorBypass As Boolean
  Public Property ErrorBypass As Boolean
    Get
      Return c_ErrorBypass
    End Get
    Set(value As Boolean)
      c_ErrorBypass = value
    End Set
  End Property
  Private c_ManualRedirect As Boolean
  Public Property ManualRedirect As Boolean
    Get
      Return c_ManualRedirect
    End Get
    Set(value As Boolean)
      c_ManualRedirect = value
    End Set
  End Property
  Private c_SendHeaders As Net.WebHeaderCollection
  Public Property SendHeaders As Net.WebHeaderCollection
    Get
      Return c_SendHeaders
    End Get
    Set(value As Net.WebHeaderCollection)
      c_SendHeaders = value
    End Set
  End Property
  Private c_Busy As Boolean
  Public ReadOnly Property IsBusy As Boolean
    Get
      Return c_Busy
    End Get
  End Property
  Private ClosingTime As Boolean
  Private sDataPath As String
  Public Sub New(DataPath As String)
    c_Timeout = 3 * 60
    c_RWTimeout = 2 * 60 * 60
    c_Proxy = New Net.WebProxy
    c_Jar = New Net.CookieContainer
    c_Encoding = System.Text.Encoding.GetEncoding(LATIN_1)
    sDataPath = DataPath
    c_Busy = False
    c_ErrorBypass = True
    c_ManualRedirect = True
    ClosingTime = False
  End Sub
  Public Sub New()
    c_Timeout = 3 * 60
    c_RWTimeout = 2 * 60 * 60
    c_Proxy = New Net.WebProxy
    c_Jar = New Net.CookieContainer
    c_Encoding = System.Text.Encoding.GetEncoding(LATIN_1)
    sDataPath = Nothing
    c_Busy = False
    c_ErrorBypass = True
    c_ManualRedirect = True
    ClosingTime = False
  End Sub
  Public Delegate Sub WebClientCallback(asyncState As Object, response As String)
  Public Sub Cancel()
    If IsBusy Then ClosingTime = True
  End Sub
#Region "Download"
  Private c_DownloadResults As New Specialized.NameValueCollection
  Private DownloadResultSync As New Object
  Private Property DownloadResults As Specialized.NameValueCollection
    Get
      SyncLock DownloadResultSync
        Return c_DownloadResults
      End SyncLock
    End Get
    Set(value As Specialized.NameValueCollection)
      SyncLock DownloadResultSync
        c_DownloadResults = value
      End SyncLock
    End Set
  End Property
#Region "Download String"
#Region "Function"
  Private Sub AsyncDownloadString(obj As Object)
    Dim RunName As String = obj(0)
    Dim address As String = obj(1)
    Dim iteration As Integer = obj(2)
    Try
      Using wsDownload As New WebClientCore
        wsDownload.Timeout = c_Timeout
        wsDownload.ReadWriteTimeout = c_RWTimeout
        wsDownload.Proxy = c_Proxy
        wsDownload.CookieJar = c_Jar
        wsDownload.Encoding = c_Encoding
        wsDownload.ErrorBypass = c_ErrorBypass
        wsDownload.ManualRedirect = c_ManualRedirect
        If Not c_SendHeaders Is Nothing Then
          For Each key As String In c_SendHeaders.Keys
            wsDownload.Headers.Add(key, c_SendHeaders(key))
          Next
        End If
        Dim uriAddr As Uri
        Try
          uriAddr = New Uri(address)
        Catch ex As Exception
          DownloadResults(RunName) = "Error: " & address & " is not a valid URI."
          Return
        End Try
        Dim sRet As String = Nothing
        Try
          sRet = wsDownload.DownloadString(uriAddr)
          c_ResponseURI = wsDownload.ResponseURI
        Catch ex As Exception
          Dim sNetErr As String = NetworkErrorToString(ex, sDataPath)
          If sNetErr.Contains("Please try again.") And iteration < 5 Then
            AsyncDownloadString({RunName, address, iteration + 1})
          Else
            c_ResponseURI = wsDownload.ResponseURI
            DownloadResults(RunName) = "Error: " & sNetErr
          End If
          Return
        End Try
        If String.IsNullOrEmpty(sRet) Then
          For Each Key In wsDownload.ResponseHeaders.AllKeys
            If Key.ToLower = "location" Then
              Dim sNewPath As String = wsDownload.ResponseHeaders.Item(Key)
              AsyncDownloadString({RunName, sNewPath, 0})
              Return
            End If
          Next
          DownloadResults(RunName) = "Error: Empty Response"
        Else
          DownloadResults(RunName) = sRet
        End If
      End Using
    Catch ex As Exception
      DownloadResults(RunName) = "Error: " & ex.Message
    End Try
  End Sub
  Public Function DownloadString(address As String) As String
    Dim RunName As String = "DOWNLOADSTRING_" & address & "_" & Int(Rnd() * &HFFFF)
    DownloadResults.Add(RunName, Nothing)
    Dim tDownload As New Threading.Thread(AddressOf AsyncDownloadString)
    c_Busy = True
    tDownload.Start({RunName, address, 0})
    Dim WaitTime As Long = TickCount() + (c_Timeout * 1000)
    Do While String.IsNullOrEmpty(DownloadResults(RunName))
      Windows.Forms.Application.DoEvents()
      Threading.Thread.Sleep(1)
      Threading.Thread.Sleep(0)
      If ClosingTime Then
        Try
          tDownload.Abort()
        Catch ex As Exception
        End Try
        If c_ResponseURI Is Nothing Then c_ResponseURI = New Uri(address)
        c_Busy = False
        Return "Connection aborted."
      ElseIf TickCount() > WaitTime Then
        Try
          tDownload.Abort()
        Catch ex As Exception
        End Try
        If c_ResponseURI Is Nothing Then c_ResponseURI = New Uri(address)
        c_Busy = False
        Return "Connection timed out."
      End If
    Loop
    Dim sRet As String = DownloadResults(RunName)
    DownloadResults.Remove(RunName)
    If c_ResponseURI Is Nothing Then c_ResponseURI = New Uri(address)
    c_Busy = False
    Return sRet
  End Function
#End Region
#Region "Callback"
  Private Sub AsyncDownloadStringWithCallback(obj As Object)
    Dim callback As WebClientCallback = obj(0)
    Dim aState As Object = obj(1)
    Dim address As String = obj(2)
    Dim iteration As Integer = obj(3)
    Try
      Using wsDownload As New WebClientCore
        wsDownload.Timeout = c_Timeout
        wsDownload.ReadWriteTimeout = c_RWTimeout
        wsDownload.Proxy = c_Proxy
        wsDownload.CookieJar = c_Jar
        wsDownload.Encoding = c_Encoding
        wsDownload.ErrorBypass = c_ErrorBypass
        wsDownload.ManualRedirect = c_ManualRedirect
        If Not c_SendHeaders Is Nothing Then
          For Each key As String In c_SendHeaders.Keys
            wsDownload.Headers.Add(key, c_SendHeaders(key))
          Next
        End If
        Dim sRet As String = Nothing
        Try
          sRet = wsDownload.DownloadString(address)
          c_ResponseURI = wsDownload.ResponseURI
        Catch ex As Exception
          Dim sNetErr As String = NetworkErrorToString(ex, sDataPath)
          If sNetErr.Contains("Please try again.") And iteration < 5 Then
            AsyncDownloadStringWithCallback({callback, aState, address, iteration + 1})
          Else
            c_ResponseURI = wsDownload.ResponseURI
            callback(aState, "Error: " & sNetErr)
          End If
          Return
        End Try
        If String.IsNullOrEmpty(sRet) Then
          For Each Key In wsDownload.ResponseHeaders.AllKeys
            If Key.ToLower = "location" Then
              Dim sNewPath As String = wsDownload.ResponseHeaders.Item(Key)
              AsyncDownloadStringWithCallback({callback, aState, sNewPath, 0})
              Return
            End If
          Next
          callback(aState, "Error: Empty Response")
        Else
          callback(aState, sRet)
        End If
      End Using
    Catch ex As Exception
      callback(aState, "Error: " & ex.Message)
    End Try
  End Sub
  Public Sub DownloadStringAsync(address As String, asyncCallback As WebClientCallback, asyncState As Object)
    Dim tDownload As New Threading.Thread(AddressOf AsyncDownloadStringWithCallback)
    tDownload.Start({asyncCallback, asyncState, address, 0})
  End Sub
#End Region
#End Region
#End Region
#Region "Upload"
  Private c_UploadResults As New Specialized.NameValueCollection
  Private UploadResultSync As New Object
  Private Property UploadResults As Specialized.NameValueCollection
    Get
      SyncLock UploadResultSync
        Return c_UploadResults
      End SyncLock
    End Get
    Set(value As Specialized.NameValueCollection)
      SyncLock UploadResultSync
        c_UploadResults = value
      End SyncLock
    End Set
  End Property
#Region "Upload String"
#Region "Function"
  Private Sub AsyncUploadString(obj As Object)
    Dim RunName As String = obj(0)
    Dim address As String = obj(1)
    Dim method As String = obj(2)
    Dim data As String = obj(3)
    Dim iteration As Integer = obj(4)
    Try
      Using wsUpload As New WebClientCore
        wsUpload.Timeout = c_Timeout
        wsUpload.ReadWriteTimeout = c_RWTimeout
        wsUpload.Proxy = c_Proxy
        wsUpload.CookieJar = c_Jar
        wsUpload.Encoding = c_Encoding
        wsUpload.ErrorBypass = c_ErrorBypass
        wsUpload.ManualRedirect = c_ManualRedirect
        If Not c_SendHeaders Is Nothing Then
          For Each key As Net.HttpRequestHeader In c_SendHeaders.Keys
            wsUpload.Headers.Add(key, c_SendHeaders(key))
          Next
        End If
        Dim sRet As String = Nothing
        If method.ToLower = "post" And Not String.IsNullOrEmpty(data) Then
          wsUpload.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
          Dim uriAddr As Uri
          Try
            uriAddr = New Uri(address)
          Catch ex As Exception
            UploadResults(RunName) = "Error: " & address & " is not a valid URI."
            Return
          End Try
          Try
            sRet = wsUpload.UploadString(uriAddr, method, data)
            c_ResponseURI = wsUpload.ResponseURI
          Catch ex As Exception
            Dim sNetErr As String = NetworkErrorToString(ex, sDataPath)
            If sNetErr.Contains("Please try again.") And iteration < 5 Then
              AsyncUploadString({RunName, address, method, data, iteration + 1})
            Else
              c_ResponseURI = wsUpload.ResponseURI
              UploadResults(RunName) = "Error: " & sNetErr
            End If
            Return
          End Try
        Else
          Dim uriAddr As Uri
          Try
            uriAddr = New Uri(address)
          Catch ex As Exception
            UploadResults(RunName) = "Error: " & address & " is not a valid URI."
            Return
          End Try
          Try
            sRet = wsUpload.DownloadString(uriAddr)
            c_ResponseURI = wsUpload.ResponseURI
          Catch ex As Exception
            Dim sNetErr As String = NetworkErrorToString(ex, sDataPath)
            If sNetErr.Contains("Please try again.") And iteration < 5 Then
              AsyncUploadString({RunName, address, method, data, iteration + 1})
            Else
              c_ResponseURI = wsUpload.ResponseURI
              UploadResults(RunName) = "Error: " & sNetErr
            End If
            Return
          End Try
        End If
        If String.IsNullOrEmpty(sRet) Then
          For Each Key In wsUpload.ResponseHeaders.AllKeys
            If Key.ToLower = "location" Then
              Dim sNewPath As String = wsUpload.ResponseHeaders.Item(Key)
              AsyncUploadString({RunName, sNewPath, "GET", Nothing, 0})
              Return
            End If
          Next
          UploadResults(RunName) = "Error: Empty Response"
        Else
          UploadResults(RunName) = sRet
        End If
      End Using
    Catch ex As Exception
      UploadResults(RunName) = "Error: " & ex.Message
    End Try
  End Sub
  Public Function UploadString(address As String, method As String, data As String) As String
    Dim RunName As String = "UPLOADSTRING_" & address & "_" & Int(Rnd() * &HFFFF)
    UploadResults.Add(RunName, Nothing)
    Dim tUpload As New Threading.Thread(AddressOf AsyncUploadString)
    c_Busy = True
    tUpload.Start({RunName, address, method, data, 0})
    Dim WaitTime As Long = TickCount() + (c_Timeout * 1000)
    Do While String.IsNullOrEmpty(UploadResults(RunName))
      Windows.Forms.Application.DoEvents()
      Threading.Thread.Sleep(1)
      Threading.Thread.Sleep(0)
      If ClosingTime Then
        Try
          tUpload.Abort()
        Catch ex As Exception
        End Try
        If c_ResponseURI Is Nothing Then c_ResponseURI = New Uri(address)
        c_Busy = False
        Return "Connection aborted."
      ElseIf TickCount() > WaitTime Then
        Try
          tUpload.Abort()
        Catch ex As Exception
        End Try
        If c_ResponseURI Is Nothing Then c_ResponseURI = New Uri(address)
        c_Busy = False
        Return "Connection timed out."
      End If
    Loop
    Dim sRet As String = UploadResults(RunName)
    UploadResults.Remove(RunName)
    If c_ResponseURI Is Nothing Then c_ResponseURI = New Uri(address)
    c_Busy = False
    Return sRet
  End Function
#End Region
#Region "Callback"
  Private Sub AsyncUploadStringWithCallback(obj As Object)
    Dim callback As WebClientCallback = obj(0)
    Dim aState As Object = obj(1)
    Dim address As String = obj(2)
    Dim method As String = obj(3)
    Dim data As String = obj(4)
    Dim iteration As Integer = obj(5)
    Try
      Using wsUpload As New WebClientCore
        wsUpload.Timeout = c_Timeout
        wsUpload.ReadWriteTimeout = c_RWTimeout
        wsUpload.Proxy = c_Proxy
        wsUpload.CookieJar = c_Jar
        wsUpload.Encoding = c_Encoding
        wsUpload.ErrorBypass = c_ErrorBypass
        wsUpload.ManualRedirect = c_ManualRedirect
        If Not c_SendHeaders Is Nothing Then
          For Each key As Net.HttpRequestHeader In c_SendHeaders.Keys
            wsUpload.Headers.Add(key, c_SendHeaders(key))
          Next
        End If
        Dim sRet As String = Nothing
        If method.ToLower = "post" And Not String.IsNullOrEmpty(data) Then
          wsUpload.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
          Try
            sRet = wsUpload.UploadString(address, method, data)
            c_ResponseURI = wsUpload.ResponseURI
          Catch ex As Exception
            Dim sNetErr As String = NetworkErrorToString(ex, sDataPath)
            If sNetErr.Contains("Please try again.") And iteration < 5 Then
              AsyncUploadStringWithCallback({callback, aState, address, method, data, iteration + 1})
            Else
              c_ResponseURI = wsUpload.ResponseURI
              callback(aState, "Error: " & sNetErr)
            End If
            Return
          End Try
        Else
          Try
            sRet = wsUpload.DownloadString(address)
            c_ResponseURI = wsUpload.ResponseURI
          Catch ex As Exception
            Dim sNetErr As String = NetworkErrorToString(ex, sDataPath)
            If sNetErr.Contains("Please try again.") And iteration < 5 Then
              AsyncUploadStringWithCallback({callback, aState, address, method, data, iteration + 1})
            Else
              c_ResponseURI = wsUpload.ResponseURI
              callback(aState, "Error: " & sNetErr)
            End If
            Return
          End Try
        End If
        If String.IsNullOrEmpty(sRet) Then
          For Each Key In wsUpload.ResponseHeaders.AllKeys
            If Key.ToLower = "location" Then
              Dim sNewPath As String = wsUpload.ResponseHeaders.Item(Key)
              AsyncUploadStringWithCallback({callback, aState, sNewPath, "GET", Nothing, 0})
              Return
            End If
          Next
          callback(aState, "Error: Empty Response")
        Else
          callback(aState, sRet)
        End If
      End Using
    Catch ex As Exception
      callback(aState, "Error: " & ex.Message)
    End Try
  End Sub
  Public Sub UploadStringAsync(address As String, method As String, data As String, asyncCallback As WebClientCallback, asyncState As Object)
    Dim tUpload As New Threading.Thread(AddressOf AsyncUploadStringWithCallback)
    tUpload.Start({asyncCallback, asyncState, address, method, data, 0})
  End Sub
#End Region
#End Region
#Region "Upload Values"
#Region "Function"
  Private Sub AsyncUploadValues(obj As Object)
    Dim RunName As String = obj(0)
    Dim address As String = obj(1)
    Dim method As String = obj(2)
    Dim data As Specialized.NameValueCollection = obj(3)
    Dim iteration As Integer = obj(4)
    Try
      Using wsUpload As New WebClientCore
        wsUpload.Timeout = c_Timeout
        wsUpload.ReadWriteTimeout = c_RWTimeout
        wsUpload.Proxy = c_Proxy
        wsUpload.CookieJar = c_Jar
        wsUpload.Encoding = c_Encoding
        wsUpload.ErrorBypass = c_ErrorBypass
        wsUpload.ManualRedirect = c_ManualRedirect
        If Not c_SendHeaders Is Nothing Then
          For Each key As String In c_SendHeaders.Keys
            wsUpload.Headers.Add(key, c_SendHeaders(key))
          Next
        End If
        Dim sRet As String = Nothing
        If method.ToLower = "post" And (data IsNot Nothing AndAlso data.Count > 0) Then
          Dim uriAddr As Uri
          Try
            uriAddr = New Uri(address)
          Catch ex As Exception
            UploadResults(RunName) = "Error: " & address & " is not a valid URI."
            Return
          End Try
          Dim bRet() As Byte = Nothing
          Try
            bRet = wsUpload.UploadValues(uriAddr, method, data)
            c_ResponseURI = wsUpload.ResponseURI
          Catch ex As Exception
            Dim sNetErr As String = NetworkErrorToString(ex, sDataPath)
            If sNetErr.Contains("Please try again.") And iteration < 5 Then
              AsyncUploadValues({RunName, address, method, data, iteration + 1})
            Else
              c_ResponseURI = wsUpload.ResponseURI
              UploadResults(RunName) = "Error: " & sNetErr
            End If
            Return
          End Try
          sRet = wsUpload.Encoding.GetString(bRet)
        Else
          Dim uriAddr As Uri
          Try
            uriAddr = New Uri(address)
          Catch ex As Exception
            UploadResults(RunName) = "Error: " & address & " is not a valid URI."
            Return
          End Try
          Try
            sRet = wsUpload.DownloadString(uriAddr)
            c_ResponseURI = wsUpload.ResponseURI
          Catch ex As Exception
            Dim sNetErr As String = NetworkErrorToString(ex, sDataPath)
            If sNetErr.Contains("Please try again.") And iteration < 5 Then
              AsyncUploadValues({RunName, address, method, data, iteration + 1})
            Else
              c_ResponseURI = wsUpload.ResponseURI
              UploadResults(RunName) = "Error: " & sNetErr
            End If
            Return
          End Try
        End If
        If String.IsNullOrEmpty(sRet) Then
          For Each Key In wsUpload.ResponseHeaders.AllKeys
            If Key.ToLower = "location" Then
              Dim sNewPath As String = wsUpload.ResponseHeaders.Item(Key)
              AsyncUploadValues({RunName, sNewPath, "GET", Nothing, 0})
              Return
            End If
          Next
          UploadResults(RunName) = "Error: Empty Response"
        Else
          UploadResults(RunName) = sRet
        End If
      End Using
    Catch ex As Exception
      UploadResults(RunName) = "Error: " & ex.Message
    End Try
  End Sub
  Public Function UploadValues(address As String, method As String, data As Specialized.NameValueCollection) As String
    Dim RunName As String = "UPLOADVAUES_" & address & "_" & Int(Rnd() * &HFFFF)
    UploadResults.Add(RunName, Nothing)
    Dim tUpload As New Threading.Thread(AddressOf AsyncUploadValues)
    c_Busy = True
    tUpload.Start({RunName, address, method, data, 0})
    Dim WaitTime As Long = TickCount() + (c_Timeout * 1000)
    Do While String.IsNullOrEmpty(UploadResults(RunName))
      Windows.Forms.Application.DoEvents()
      Threading.Thread.Sleep(1)
      Threading.Thread.Sleep(0)
      If ClosingTime Then
        Try
          tUpload.Abort()
        Catch ex As Exception
        End Try
        If c_ResponseURI Is Nothing Then c_ResponseURI = New Uri(address)
        c_Busy = False
        Return "Connection aborted."
      ElseIf TickCount() > WaitTime Then
        Try
          tUpload.Abort()
        Catch ex As Exception
        End Try
        If c_ResponseURI Is Nothing Then c_ResponseURI = New Uri(address)
        c_Busy = False
        Return "Connection timed out."
      End If
    Loop
    Dim sRet As String = UploadResults(RunName)
    UploadResults.Remove(RunName)
    If c_ResponseURI Is Nothing Then c_ResponseURI = New Uri(address)
    c_Busy = False
    Return sRet
  End Function
#End Region
#Region "Callback"
  Private Sub AsyncUploadValuesWithCallback(obj As Object)
    Dim callback As WebClientCallback = obj(0)
    Dim aState As Object = obj(1)
    Dim address As String = obj(2)
    Dim method As String = obj(3)
    Dim data As Specialized.NameValueCollection = obj(4)
    Dim iteration As Integer = obj(5)
    Try
      Using wsUpload As New WebClientCore
        wsUpload.Timeout = c_Timeout
        wsUpload.ReadWriteTimeout = c_RWTimeout
        wsUpload.Proxy = c_Proxy
        wsUpload.CookieJar = c_Jar
        wsUpload.Encoding = c_Encoding
        wsUpload.ErrorBypass = c_ErrorBypass
        wsUpload.ManualRedirect = c_ManualRedirect
        If Not c_SendHeaders Is Nothing Then
          For Each key As Net.HttpRequestHeader In c_SendHeaders.Keys
            wsUpload.Headers.Add(key, c_SendHeaders(key))
          Next
        End If
        Dim sRet As String = Nothing
        If method.ToLower = "post" And (data IsNot Nothing AndAlso data.Count > 0) Then
          Dim bRet() As Byte = Nothing
          Try
            bRet = wsUpload.UploadValues(address, method, data)
            c_ResponseURI = wsUpload.ResponseURI
          Catch ex As Exception
            Dim sNetErr As String = NetworkErrorToString(ex, sDataPath)
            If sNetErr.Contains("Please try again.") And iteration < 5 Then
              AsyncUploadValuesWithCallback({callback, aState, address, method, data, iteration + 1})
            Else
              c_ResponseURI = wsUpload.ResponseURI
              callback(aState, "Error: " & sNetErr)
            End If
            Return
          End Try
          sRet = wsUpload.Encoding.GetString(bRet)
        Else
          Try
            sRet = wsUpload.DownloadString(address)
            c_ResponseURI = wsUpload.ResponseURI
          Catch ex As Exception
            Dim sNetErr As String = NetworkErrorToString(ex, sDataPath)
            If sNetErr.Contains("Please try again.") And iteration < 5 Then
              AsyncUploadValuesWithCallback({callback, aState, address, method, data, iteration + 1})
            Else
              c_ResponseURI = wsUpload.ResponseURI
              callback(aState, "Error: " & sNetErr)
            End If
            Return
          End Try
        End If
        If String.IsNullOrEmpty(sRet) Then
          For Each Key In wsUpload.ResponseHeaders.AllKeys
            If Key.ToLower = "location" Then
              Dim sNewPath As String = wsUpload.ResponseHeaders.Item(Key)
              AsyncUploadValuesWithCallback({callback, aState, sNewPath, "GET", Nothing, 0})
              Return
            End If
          Next
          callback(aState, "Error: Empty Response")
        Else
          callback(aState, sRet)
        End If
      End Using
    Catch ex As Exception
      callback(aState, "Error: " & ex.Message)
    End Try
  End Sub
  Public Sub UploadValuesAsync(address As String, method As String, data As Specialized.NameValueCollection, asyncCallback As WebClientCallback, asyncState As Object)
    Dim tUpload As New Threading.Thread(AddressOf AsyncUploadValuesWithCallback)
    tUpload.Start({asyncCallback, asyncState, address, method, data, 0})
  End Sub
#End Region
#End Region
#End Region
End Class

Public Enum SecurityProtocolTypeEx
  Ssl3 = &H30
  Tls10 = &HC0
  Tls11 = &H300
  Tls12 = &HC00
End Enum