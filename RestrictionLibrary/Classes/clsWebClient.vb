Public Class WebClientCore
  Inherits Net.WebClient
  ''' <summary>
  ''' Create a new instance of the <see cref="WebClientCore" /> Class.
  ''' </summary>
  ''' <param name="useEvents">If set to <c>True</c>, failures will trigger events, if <c>False</c>, failures will throw errors.</param>
  Public Sub New(useEvents As Boolean)
    MyBase.New()
    c_Events = useEvents
    c_CookieJar = New Net.CookieContainer
    c_SendCookieJar = False
    c_Timeout = 120
    c_RWTimeout = 300
    c_HTVer = Net.HttpVersion.Version11
    c_ErrorBypass = True
    c_KeepAlive = True
    c_ManualRedirect = True
    c_ResponseURI = Nothing
    c_ResponseCode = Nothing
    System.Net.ServicePointManager.Expect100Continue = False
  End Sub
  ''' <summary>
  ''' Create a new instance of the <see cref="WebClientCore" /> Class.
  ''' </summary>
  Sub New()
    MyBase.New()
    c_Events = False
    c_CookieJar = New Net.CookieContainer
    c_SendCookieJar = False
    c_Timeout = 120
    c_RWTimeout = 300
    c_HTVer = Net.HttpVersion.Version11
    c_ErrorBypass = True
    c_KeepAlive = True
    c_ManualRedirect = True
    c_ResponseURI = Nothing
    c_ResponseCode = Nothing
    System.Net.ServicePointManager.Expect100Continue = False
  End Sub
  ''' <summary>
  ''' A <see cref="WebClientCore" /> Upload or Download request failure message containing an <see cref="Exception" />.
  ''' </summary>
  Public Class ErrorEventArgs
    Inherits EventArgs
    Public [Error] As Exception
    ''' <summary>
    ''' Create a new instance of the <see cref="ErrorEventArgs" /> Class for use with the <see cref="Failure" /> event.
    ''' </summary>
    ''' <param name="Err">The Exception being passed through the <see cref="Failure" /> event.</param>
    Public Sub New(Err As Exception)
      [Error] = Err
    End Sub
  End Class
  ''' <summary>
  ''' This constant replaces any commas in the values of cookies received, and will be replaced by commas on any cookies being sent.
  ''' </summary>
  Public Const REPLACE_COMMA As String = "[COMMA]"
  ''' <summary>
  ''' This constant replaces any semicolons in the values of cookies received, and will be replaced by semicolons on any cookies being sent.
  ''' </summary>
  Public Const REPLACE_SEMIC As String = "[SEMICOLON]"
  Private c_CookieJar As Net.CookieContainer
  ''' <summary>
  ''' Acts as the container for a collection of <see cref="System.Net.CookieCollection" /> objects for use with an instance of the <see cref="WebClientCore" /> class.
  ''' </summary>
  ''' <value>A new CookieJar to use in place of the default empty Jar. This is useful for sharing and storing sessions and cookies across multiple <see cref="WebClientCore" /> instances.</value>
  ''' <returns>The current CookieJar for this <see cref="WebClientCore" />.</returns>
  Public Property CookieJar As Net.CookieContainer
    Get
      Return c_CookieJar
    End Get
    Set(value As Net.CookieContainer)
      c_CookieJar = value
    End Set
  End Property
  Private c_SendCookieJar As Boolean
  ''' <summary>
  ''' The way cookies are sent can be modified using this property.
  ''' </summary>
  ''' <value>Cookies will be sent by text if this value is <c>False</c>, and by both text and cookie jar if <c>True</c>.</value>
  ''' <returns>See the value entry for details on the return values.</returns>
  Public Property SendCookieJar As Boolean
    Get
      Return c_SendCookieJar
    End Get
    Set(value As Boolean)
      c_SendCookieJar = value
    End Set
  End Property
  Private c_ResponseURI As Uri
  ''' <summary>
  ''' The <see cref="Uri" /> of the last <see cref="GetWebResponse" /> event after any redirection.
  ''' </summary>
  ''' <returns>Initially, the value will be <c>Nothing</c> until a <see cref="GetWebResponse" /> event occurs. It may be equal to the <see cref="Uri" /> sent in a Download or Upload request, or it may be modified during the request as per page redirection standards*.</returns>
  ''' <remarks>*Page Redirection can be disabled using the <see cref="ManualRedirect" /> property. The standards followed if <see cref="ManualRedirect" /> is set to <c>False</c> are the HTTP 300 Status Code set of errors. Up to 50 redirects are allowed if <see cref="Net.HttpWebRequest.MaximumAutomaticRedirections" /> has not been set manually.</remarks>
  Public ReadOnly Property ResponseURI As Uri
    Get
      Return c_ResponseURI
    End Get
  End Property
  Private c_ResponseCode As Net.HttpStatusCode
  ''' <summary>
  ''' The <see cref="Net.HttpStatusCode" /> of the last <see cref="GetWebResponse" /> event after any redirection.
  ''' </summary>
  ''' <returns>Initially, the value will be <c>Nothing</c> until a <see cref="GetWebResponse" /> event occurs.</returns>
  Public ReadOnly Property ResponseCode As Net.HttpStatusCode
    Get
      Return c_ResponseCode
    End Get
  End Property
  Private c_Timeout As Integer
  ''' <summary>
  ''' Gets or sets the time-out value in seconds for the <see cref="System.Net.HttpWebRequest.GetResponse" /> and <see cref="System.Net.HttpWebRequest.GetRequestStream" /> methods.
  ''' </summary>
  ''' <value></value>
  ''' <returns>The number of milliseconds to wait before the request times out. The default is 120 seconds.</returns>
  Public Property Timeout As Integer
    Get
      Return c_Timeout
    End Get
    Set(value As Integer)
      c_Timeout = value
    End Set
  End Property
  Private c_RWTimeout As Integer
  ''' <summary>
  ''' Gets or sets a time-out in seconds when writing to or reading from a stream.
  ''' </summary>
  ''' <value></value>
  ''' <returns>The number of seconds before the writing or reading times out. The default value is 300 seconds (5 minutes).</returns>
  Public Property ReadWriteTimeout As Integer
    Get
      Return c_RWTimeout
    End Get
    Set(value As Integer)
      c_RWTimeout = value
    End Set
  End Property
  Private c_HTVer As Version
  ''' <summary>
  ''' Gets or sets the version of HTTP to use for the request.
  ''' </summary>
  ''' <value></value>
  ''' <returns>The HTTP version to use for the request. The default is <see cref="System.Net.HttpVersion.Version11" />.</returns>
  Public Property HTTPVersion As Version
    Get
      Return c_HTVer
    End Get
    Set(value As Version)
      c_HTVer = value
    End Set
  End Property
  Private c_ErrorBypass As Boolean
  ''' <summary>
  ''' Gets or sets a <see cref="Boolean" /> value which determines how the <see cref="WebClientCore" /> reacts to the HTTP 400 Status Code set of errors.
  ''' </summary>
  ''' <value>If set to <c>True</c> and the error contains a <see cref="System.Net.WebResponse" /> value, that response is sent instead of an error.</value>
  ''' <returns>If <c>True</c>, errors may be bypassed and trigger Upload and Download completion events, if possible. If <c>False</c>, all error responses are treated as errors. The default value is <c>True</c>.</returns>
  Public Property ErrorBypass As Boolean
    Get
      Return c_ErrorBypass
    End Get
    Set(value As Boolean)
      c_ErrorBypass = value
    End Set
  End Property
  Private c_KeepAlive As Boolean
  ''' <summary>
  ''' Gets or sets a value that indicates whether to make a persistent connection to the Internet resource.
  ''' </summary>
  ''' <value>If set to <c>True</c>, the <see cref="WebClientCore" /> will send a Connection HTTP header with the value Keep-alive; if <c>False</c>, Close.</value>
  ''' <returns><c>True</c> if the request to the Internet resource should contain a Connection HTTP header with the value Keep-alive; otherwise, <c>False</c>. The default is <c>True</c>.</returns>
  Public Property KeepAlive As Boolean
    Get
      Return c_KeepAlive
    End Get
    Set(value As Boolean)
      c_KeepAlive = value
    End Set
  End Property
  Private c_ManualRedirect As Boolean
  ''' <summary>
  ''' Gets or sets a <see cref="Boolean" /> value which determines how the <see cref="WebClientCore" /> reacts to the HTTP 300 Status Code set of errors.
  ''' </summary>
  ''' <value></value>
  ''' <returns><c>False</c> if the request should automatically follow redirection responses from the Internet resource; otherwise, <c>True</c>. The default value is <c>True</c>.</returns>
  Public Property ManualRedirect As Boolean
    Get
      Return c_ManualRedirect
    End Get
    Set(value As Boolean)
      c_ManualRedirect = value
    End Set
  End Property
  Private c_Events As Boolean
  ''' <summary>
  ''' A <see cref="WebClientCore" /> Upload or Download request has failed.
  ''' </summary>
  ''' <param name="sender">The class which is triggering the event.</param>
  ''' <param name="e">The exception contained in an EventArg</param>
  Public Event Failure As EventHandler(Of ErrorEventArgs)
  ''' <summary>
  ''' The User Agent for Satellite Restriction Tracker
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  Public Shared ReadOnly Property UserAgent As String
    Get
      Return "Mozilla/5.0 (" & CurrentOS.Name & "; CLR: " & srlFunctions.GetCLRVersion & ") " & My.Application.Info.ProductName.Replace(" ", "") & "/" & My.Application.Info.Version.ToString
    End Get
  End Property
  Private m_Request As Net.WebRequest
  Private m_Result As Net.WebResponse
  Protected Overrides Function GetWebRequest(address As System.Uri) As System.Net.WebRequest
    Try
      If m_Request IsNot Nothing Then
        m_Request.Abort()
        m_Request = Nothing
      End If
      If m_Result IsNot Nothing Then
        m_Result.Close()
        m_Result = Nothing
      End If
      Dim request As Net.WebRequest = MyBase.GetWebRequest(address)
      If request.GetType Is GetType(Net.HttpWebRequest) Then
        Dim hRequest As Net.HttpWebRequest = request
        hRequest.UserAgent = WebClientCore.UserAgent
        hRequest.ReadWriteTimeout = c_RWTimeout * 1000
        hRequest.Timeout = c_Timeout * 1000
        hRequest.CookieContainer = Nothing
        Dim sCookieHeader As String = c_CookieJar.GetCookieHeader(address)
        If Not String.IsNullOrEmpty(sCookieHeader) Then
          If sCookieHeader.Contains(REPLACE_COMMA) Or sCookieHeader.Contains(REPLACE_SEMIC) Then
            sCookieHeader = sCookieHeader.Replace(REPLACE_COMMA, ",")
            sCookieHeader = sCookieHeader.Replace(REPLACE_SEMIC, ";")
          End If
          hRequest.Headers.Add(Net.HttpRequestHeader.Cookie, sCookieHeader)
          If c_SendCookieJar Then
            Dim cNewJar As New Net.CookieContainer
            AppendCookies(cNewJar, "Cookie", hRequest.Headers, address.Host)
            hRequest.CookieContainer = cNewJar
          End If
        End If
        hRequest.AllowAutoRedirect = Not c_ManualRedirect
        hRequest.KeepAlive = c_KeepAlive
        hRequest.ProtocolVersion = HTTPVersion
        hRequest.CachePolicy = New Net.Cache.HttpRequestCachePolicy(Net.Cache.HttpRequestCacheLevel.BypassCache)
      End If
      m_Request = request
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
      Return HandleWebResponse(request, MyBase.GetWebResponse(request))
    Catch ex As Net.WebException
      If c_ErrorBypass Then
        Dim errResponse As System.Net.WebResponse = HandleWebResponse(request, ex.Response)
        If errResponse IsNot Nothing Then Return errResponse
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
      Return HandleWebResponse(request, MyBase.GetWebResponse(request, result))
    Catch ex As Net.WebException
      Return HandleWebResponse(request, ex.Response)
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
  Private Sub WebClientCore_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
    If m_Request IsNot Nothing Then
      m_Request.Abort()
      m_Request = Nothing
    End If
    If m_Result IsNot Nothing Then
      m_Result.Close()
      m_Result = Nothing
    End If
  End Sub
#Region "Helper Functions"
  Private Function HandleWebResponse(request As Net.WebRequest, response As Net.WebResponse) As Net.WebResponse
    Try
      If response Is Nothing Then Return Nothing
      AppendCookies(c_CookieJar, "Set-Cookie", response.Headers, response.ResponseUri.Host)
      Dim chEncoding As System.Text.Encoding = Nothing
      If response.GetType Is GetType(Net.HttpWebResponse) Then
        Dim hResponse As Net.HttpWebResponse = response
        c_ResponseCode = hResponse.StatusCode
        If Not String.IsNullOrEmpty(hResponse.CharacterSet) Then
          Dim charSet As String = hResponse.CharacterSet
          Try
            chEncoding = System.Text.Encoding.GetEncoding(charSet)
          Catch ex As Exception
            chEncoding = Nothing
          End Try
        End If
      End If
      If chEncoding Is Nothing Then
        If response.ContentType.ToUpperInvariant.Contains("CHARSET=") Then
          Dim charSet As String = response.ContentType.Substring(response.ContentType.ToUpperInvariant.IndexOf("CHARSET"))
          charSet = charSet.Substring(charSet.IndexOf("=") + 1)
          If charSet.Contains(";") Then charSet = charSet.Substring(0, charSet.IndexOf(";"))
          Try
            chEncoding = System.Text.Encoding.GetEncoding(charSet)
          Catch ex As Exception
            chEncoding = Nothing
          End Try
        End If
      End If
      If chEncoding Is Nothing Then chEncoding = System.Text.Encoding.GetEncoding(srlFunctions.LATIN_1)
      Try
        Me.Encoding = chEncoding
      Catch ex As Exception
        Me.Encoding = System.Text.Encoding.GetEncoding(srlFunctions.LATIN_1)
      End Try
      m_Result = response
      c_ResponseURI = response.ResponseUri
      Return response
    Catch ex As Net.WebException
      Return HandleWebResponse(request, ex.Response)
    End Try
  End Function
  Private Function CleanCookie(sCookieData As String) As String
    Dim sReconstruction As String = Nothing
    Do While Not String.IsNullOrEmpty(sCookieData)
      Dim cName As String = Nothing
      Dim cVal As String = Nothing
      Dim cExtra As String = Nothing

      If sCookieData.Contains("=") Then
        cName = sCookieData.Substring(0, sCookieData.IndexOf("="))
        sCookieData = sCookieData.Substring(sCookieData.IndexOf("=") + 1).TrimStart
      End If

      If Not String.IsNullOrEmpty(sCookieData) Then
        If sCookieData.Contains(";") Then
          cVal = sCookieData.Substring(0, sCookieData.IndexOf(";"))
          sCookieData = sCookieData.Substring(sCookieData.IndexOf(";") + 1).TrimStart
        ElseIf sCookieData.Contains(",") Then
          cVal = sCookieData.Substring(0, sCookieData.IndexOf(","))
          sCookieData = sCookieData.Substring(sCookieData.IndexOf(",") + 1).TrimStart
        Else
          cVal = sCookieData
          sCookieData = Nothing
        End If
      End If

      Do While Not String.IsNullOrEmpty(sCookieData) AndAlso sCookieData.Contains(";")
        Dim sSegment As String = sCookieData.Substring(0, sCookieData.IndexOf(";") + 1)
        If (sSegment.Contains("=") And sSegment.Contains(",")) AndAlso (sSegment.IndexOf(",") < sSegment.IndexOf("=")) Then Exit Do
        cExtra &= sCookieData.Substring(0, sCookieData.IndexOf(";") + 1) & " "
        sCookieData = sCookieData.Substring(sCookieData.IndexOf(";") + 1).TrimStart
      Loop
      If Not String.IsNullOrEmpty(sCookieData) Then
        If (sCookieData.Contains("=") And sCookieData.Contains(",")) AndAlso (sCookieData.IndexOf(",") < sCookieData.IndexOf("=")) Then
          cExtra &= sCookieData.Substring(0, sCookieData.IndexOf(","))
          sCookieData = sCookieData.Substring(sCookieData.IndexOf(",") + 1).TrimStart
        Else
          cExtra &= sCookieData
          sCookieData = Nothing
        End If
      End If

      If Not String.IsNullOrEmpty(sReconstruction) Then sReconstruction &= ","
      If String.IsNullOrEmpty(cExtra) Then
        sReconstruction &= cName & "=" & cVal.Replace(",", REPLACE_COMMA).Replace(";", REPLACE_SEMIC)
      Else
        sReconstruction &= cName & "=" & cVal.Replace(",", REPLACE_COMMA).Replace(";", REPLACE_SEMIC) & "; " & cExtra
      End If
    Loop
    Return sReconstruction
  End Function
  Private Function CookieStrToCookies(sCookieData As String, DefaultDomain As String) As Net.Cookie()
    Dim cookieList As New List(Of Net.Cookie)
    Do While Not String.IsNullOrEmpty(sCookieData)
      Dim cName As String = Nothing
      Dim cVal As String = Nothing
      Dim cExtra As String = Nothing
      If sCookieData.Contains("=") Then
        cName = sCookieData.Substring(0, sCookieData.IndexOf("="))
        sCookieData = sCookieData.Substring(sCookieData.IndexOf("=") + 1).TrimStart
      End If
      If Not String.IsNullOrEmpty(sCookieData) Then
        If sCookieData.Contains(";") Then
          cVal = sCookieData.Substring(0, sCookieData.IndexOf(";"))
          sCookieData = sCookieData.Substring(sCookieData.IndexOf(";") + 1).TrimStart
        ElseIf sCookieData.Contains(",") Then
          cVal = sCookieData.Substring(0, sCookieData.IndexOf(","))
          sCookieData = sCookieData.Substring(sCookieData.IndexOf(",") + 1).TrimStart
        Else
          cVal = sCookieData
          sCookieData = Nothing
        End If
      End If
      Do While Not String.IsNullOrEmpty(sCookieData) AndAlso sCookieData.Contains(";")
        Dim sSegment As String = sCookieData.Substring(0, sCookieData.IndexOf(";") + 1)
        If sSegment.Contains("=") And sSegment.Contains(",") Then
          Dim sSegID As String = sSegment.Substring(0, sSegment.IndexOf("="))
          If sSegID.ToUpperInvariant = "EXPIRES" Then
            If sSegment.IndexOf(",") < sSegment.IndexOf("=") Then Exit Do
            If sSegment.Substring(sSegment.IndexOf(",") + 1).Contains(",") Then Exit Do
          Else
            Exit Do
          End If
        End If
        cExtra &= sCookieData.Substring(0, sCookieData.IndexOf(";") + 1) & " "
        sCookieData = sCookieData.Substring(sCookieData.IndexOf(";") + 1).TrimStart
      Loop
      If Not String.IsNullOrEmpty(sCookieData) Then
        If sCookieData.Contains("=") And sCookieData.Contains(",") Then
          Dim sSegID As String = sCookieData.Substring(0, sCookieData.IndexOf("="))
          If sSegID.ToUpperInvariant = "EXPIRES" Then
            If sCookieData.IndexOf(",") < sCookieData.IndexOf("=") Then
              cExtra &= sCookieData.Substring(0, sCookieData.IndexOf(","))
              sCookieData = sCookieData.Substring(sCookieData.IndexOf(",") + 1).TrimStart
            ElseIf sCookieData.Substring(sCookieData.IndexOf(",") + 1).Contains(",") And sCookieData.IndexOf(",", sCookieData.IndexOf(",") + 1) > sCookieData.IndexOf("=") Then
              cExtra &= sCookieData.Substring(0, sCookieData.IndexOf(",", sCookieData.IndexOf(",") + 1))
              sCookieData = sCookieData.Substring(sCookieData.IndexOf(",", sCookieData.IndexOf(",") + 1) + 1).TrimStart
            Else
              cExtra &= sCookieData
              sCookieData = Nothing
            End If
          Else
            cExtra &= sCookieData.Substring(0, sCookieData.IndexOf(","))
            sCookieData = sCookieData.Substring(sCookieData.IndexOf(",") + 1).TrimStart
          End If
        Else
          cExtra &= sCookieData
          sCookieData = Nothing
        End If
      End If
      If String.IsNullOrEmpty(cExtra) Then
        cookieList.Add(New Net.Cookie(cName, cVal, "/", DefaultDomain))
      Else
        Dim sDomain As String = Nothing
        Dim sPath As String = Nothing
        Dim bHTTP As Boolean = False
        Dim bSecure As Boolean = False
        Dim sExpires As String = Nothing
        Dim sMaxAge As String = Nothing
        Dim iVersion As Integer = Integer.MinValue
        Dim Extras() As String = Split(cExtra, ";")
        For Each sExtra In Extras
          If sExtra.Contains("=") Then
            Dim sExtraKV() As String = Split(sExtra.Trim, "=", 2)
            Select Case sExtraKV(0).ToUpperInvariant
              Case "PATH"
                sPath = sExtraKV(1)
              Case "DOMAIN"
                sDomain = sExtraKV(1)
              Case "EXPIRES"
                sExpires = sExtraKV(1)
              Case "MAX-AGE"
                sMaxAge = sExtraKV(1)
              Case "VERSION"
                iVersion = Val(sExtraKV(1))
              Case Else
                Debug.Print("Unknown Cookie Key: " & sExtraKV(0))
            End Select
          Else
            Select Case sExtra.Trim.ToUpperInvariant
              Case "HTTP"
                bHTTP = True
              Case "SECURE"
                bSecure = True
            End Select
          End If
        Next
        If String.IsNullOrEmpty(sDomain) Then sDomain = DefaultDomain
        If String.IsNullOrEmpty(sPath) Then sPath = "/"
        If iVersion = 1 Then If cVal.StartsWith("""") And cVal.EndsWith("""") Then cVal = cVal.Substring(1, cVal.Length - 2)
        Dim nC As New Net.Cookie(cName, cVal, sPath, sDomain)
        nC.HttpOnly = bHTTP
        nC.Secure = bSecure
        If Not String.IsNullOrEmpty(sExpires) Then If Not Date.TryParse(sExpires, nC.Expires) Then nC.Expires = Now.AddDays(1)
        If Not String.IsNullOrEmpty(sMaxAge) Then
          Dim dMaxAge As Double = Val(sMaxAge)
          If dMaxAge < 0 Then
            nC.Expires = Now.AddDays(1)
          ElseIf dMaxAge = 0 Then
            If nC.Expires.Year = 1 Then nC.Expires = Now.AddDays(1)
          Else
            nC.Expires = Now.AddSeconds(dMaxAge)
          End If
        End If
        'If Not iVersion = Integer.MinValue Then nC.Version = iVersion
        cookieList.Add(nC)
      End If
    Loop
    Return cookieList.ToArray
  End Function
  Private Sub AppendCookies(ByRef CookieJar As Net.CookieContainer, HeaderName As String, Headers As Net.WebHeaderCollection, DefaultDomain As String)
    For Each sHead In Headers.AllKeys
      If sHead = HeaderName Then
        Dim sCookieData As String = Headers(sHead)
        Dim sReconstruction As String = CleanCookie(sCookieData)
        Dim newCookies() As Net.Cookie = CookieStrToCookies(sReconstruction, DefaultDomain)
        For Each newCookie In newCookies
          CookieJar.Add(newCookie)
        Next
      End If
    Next
  End Sub
#End Region
End Class

Public Class WebClientEx
  Private c_Timeout As Integer
  ''' <summary>
  ''' Gets or sets the time-out value in seconds for the <see cref="System.Net.HttpWebRequest.GetResponse" /> and <see cref="System.Net.HttpWebRequest.GetRequestStream" /> methods.
  ''' </summary>
  ''' <value></value>
  ''' <returns>The number of milliseconds to wait before the request times out. The default is 180 seconds.</returns>
  Public Property Timeout As Integer
    Get
      Return c_Timeout
    End Get
    Set(value As Integer)
      c_Timeout = value
    End Set
  End Property
  Private c_RWTimeout As Integer
  ''' <summary>
  ''' Gets or sets a time-out in seconds when writing to or reading from a stream.
  ''' </summary>
  ''' <value></value>
  ''' <returns>The number of seconds before the writing or reading times out. The default value is 7200 seconds (2 hours).</returns>
  Public Property ReadWriteTimeout As Integer
    Get
      Return c_RWTimeout
    End Get
    Set(value As Integer)
      c_RWTimeout = value
    End Set
  End Property
  Private c_Proxy As Net.IWebProxy
  ''' <summary>
  ''' Gets or sets the proxy used by this <see cref="WebClientEx" /> object.
  ''' </summary>
  ''' <value></value>
  ''' <returns>An <see cref="System.Net.IWebProxy" /> instance used to send requests.</returns>
  Public Property Proxy As Net.IWebProxy
    Get
      Return c_Proxy
    End Get
    Set(value As Net.IWebProxy)
      c_Proxy = value
    End Set
  End Property
  Private c_Jar As Net.CookieContainer
  ''' <summary>
  ''' Acts as the container for a collection of <see cref="System.Net.CookieCollection" /> objects for use with an instance of the <see cref="WebClientEx" /> class.
  ''' </summary>
  ''' <value>A new CookieJar to use in place of the default empty Jar. This is useful for sharing and storing sessions and cookies across multiple <see cref="WebClientEx" /> instances.</value>
  ''' <returns>The current CookieJar for this <see cref="WebClientEx" />.</returns>
  Public Property CookieJar As Net.CookieContainer
    Get
      Return c_Jar
    End Get
    Set(value As Net.CookieContainer)
      c_Jar = value
    End Set
  End Property
  Private c_SendJar As Boolean
  Public Property SendCookieJar As Boolean
    Get
      Return c_SendJar
    End Get
    Set(value As Boolean)
      c_SendJar = value
    End Set
  End Property
  Private c_Encoding As System.Text.Encoding
  ''' <summary>
  ''' Gets and sets the System.Text.Encoding used to upload and download strings.
  ''' </summary>
  ''' <value></value>
  ''' <returns>A System.Text.Encoding that is used to encode strings. The default value of this property is <see cref="srlFunctions.LATIN_1" />.</returns>
  Public Property Encoding As System.Text.Encoding
    Get
      Return c_Encoding
    End Get
    Set(value As System.Text.Encoding)
      c_Encoding = value
    End Set
  End Property
  Private c_ResponseURI As Uri
  ''' <summary>
  ''' The <see cref="Uri" /> of the last <see cref="WebClientCore.GetWebResponse" /> event after any redirection.
  ''' </summary>
  ''' <returns>Initially, the value will be <c>Nothing</c> until a <see cref="WebClientCore.GetWebResponse" /> event occurs. It may be equal to the <see cref="Uri" /> sent in a Download or Upload request, or it may be modified during the request as per page redirection standards*.</returns>
  ''' <remarks>*Page Redirection can be disabled using the <see cref="ManualRedirect" /> property. The standards followed if <see cref="ManualRedirect" /> is set to <c>False</c> are the HTTP 300 Status Code set of errors. Up to 50 redirects are allowed if <see cref="Net.HttpWebRequest.MaximumAutomaticRedirections" /> has not been set manually.</remarks>
  Public ReadOnly Property ResponseURI As Uri
    Get
      Return c_ResponseURI
    End Get
  End Property
  Private c_ResponseCode As Net.HttpStatusCode
  ''' <summary>
  ''' The <see cref="Net.HttpStatusCode" /> of the last <see cref="WebClientCore.GetWebResponse" /> event after any redirection.
  ''' </summary>
  ''' <returns>Initially, the value will be <c>Nothing</c> until a <see cref="WebClientCore.GetWebResponse" /> event occurs.</returns>
  Public ReadOnly Property ResponseCode As Net.HttpStatusCode
    Get
      Return c_ResponseCode
    End Get
  End Property
  Private c_ErrorBypass As Boolean
  ''' <summary>
  ''' Gets or sets a <see cref="Boolean" /> value which determines how the <see cref="WebClientCore" /> reacts to the HTTP 400 Status Code set of errors.
  ''' </summary>
  ''' <value>If set to <c>True</c> and the error contains a <see cref="System.Net.WebResponse" /> value, that response is sent instead of an error.</value>
  ''' <returns>If <c>True</c>, errors may be bypassed and trigger Upload and Download completion events, if possible. If <c>False</c>, all error responses are treated as errors. The default value is <c>True</c>.</returns>
  Public Property ErrorBypass As Boolean
    Get
      Return c_ErrorBypass
    End Get
    Set(value As Boolean)
      c_ErrorBypass = value
    End Set
  End Property
  Private c_KeepAlive As Boolean
  ''' <summary>
  ''' Gets or sets a value that indicates whether to make a persistent connection to the Internet resource.
  ''' </summary>
  ''' <value>If set to <c>True</c>, the <see cref="WebClientCore" /> will send a Connection HTTP header with the value Keep-alive; if <c>False</c>, Close.</value>
  ''' <returns><c>True</c> if the request to the Internet resource should contain a Connection HTTP header with the value Keep-alive; otherwise, <c>False</c>. The default is <c>True</c>.</returns>
  Public Property KeepAlive As Boolean
    Get
      Return c_KeepAlive
    End Get
    Set(value As Boolean)
      c_KeepAlive = value
    End Set
  End Property
  Private c_ManualRedirect As Boolean
  ''' <summary>
  ''' Gets or sets a <see cref="Boolean" /> value which determines how the <see cref="WebClientCore" /> reacts to the HTTP 300 Status Code set of errors.
  ''' </summary>
  ''' <value></value>
  ''' <returns><c>False</c> if the request should automatically follow redirection responses from the Internet resource; otherwise, <c>True</c>. The default value is <c>True</c>.</returns>
  Public Property ManualRedirect As Boolean
    Get
      Return c_ManualRedirect
    End Get
    Set(value As Boolean)
      c_ManualRedirect = value
    End Set
  End Property
  Private c_SendHeaders As Net.WebHeaderCollection
  ''' <summary>
  ''' Gets or sets a collection of header name/value pairs associated with the request.
  ''' </summary>
  ''' <value></value>
  ''' <returns>A <see cref="System.Net.WebHeaderCollection" /> containing header name/value pairs associated with this request.</returns>
  Public Property SendHeaders As Net.WebHeaderCollection
    Get
      Return c_SendHeaders
    End Get
    Set(value As Net.WebHeaderCollection)
      c_SendHeaders = value
    End Set
  End Property
  Private c_Busy As Boolean
  ''' <summary>
  ''' Returns a Boolean value that provides information about the state of the request.
  ''' </summary>
  ''' <value></value>
  ''' <returns>If <c>True</c>, the request has been processed and is being sent, the client is waiting for the server, or data is being received in response. Otherwise, the request has not yet been sent or a response has already been received.</returns>
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
    c_SendJar = False
    c_Encoding = System.Text.Encoding.GetEncoding(srlFunctions.LATIN_1)
    sDataPath = DataPath
    c_Busy = False
    c_ErrorBypass = True
    c_KeepAlive = True
    c_ManualRedirect = True
    ClosingTime = False
  End Sub
  Public Sub New()
    c_Timeout = 3 * 60
    c_RWTimeout = 2 * 60 * 60
    c_Proxy = New Net.WebProxy
    c_Jar = New Net.CookieContainer
    c_SendJar = False
    c_Encoding = System.Text.Encoding.GetEncoding(srlFunctions.LATIN_1)
    sDataPath = Nothing
    c_Busy = False
    c_ErrorBypass = True
    c_KeepAlive = True
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
        wsDownload.SendCookieJar = c_SendJar
        wsDownload.Encoding = c_Encoding
        wsDownload.ErrorBypass = c_ErrorBypass
        wsDownload.KeepAlive = c_KeepAlive
        wsDownload.ManualRedirect = c_ManualRedirect
        If Not c_SendHeaders Is Nothing Then
          For I As Integer = 0 To c_SendHeaders.Count - 1
            wsDownload.Headers.Add(c_SendHeaders.GetKey(I), c_SendHeaders(I))
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
          c_Jar = wsDownload.CookieJar
          c_ResponseURI = wsDownload.ResponseURI
          c_ResponseCode = wsDownload.ResponseCode
        Catch ex As Exception
          Dim sNetErr As String = srlFunctions.NetworkErrorToString(ex, sDataPath)
          c_Jar = wsDownload.CookieJar
          If sNetErr.Contains("Please try again.") And iteration < 5 Then
            AsyncDownloadString({RunName, address, iteration + 1})
          Else
            c_ResponseURI = wsDownload.ResponseURI
            c_ResponseCode = wsDownload.ResponseCode
            DownloadResults(RunName) = "Error: " & sNetErr
          End If
          Return
        End Try
        If String.IsNullOrEmpty(sRet) Then
          Dim sNewPath As String = CheckHeaderRedirect(wsDownload.ResponseHeaders, IIf(c_ResponseURI Is Nothing, address, c_ResponseURI.OriginalString))
          If Not String.IsNullOrEmpty(sNewPath) Then
            AsyncDownloadString({RunName, sNewPath, 0})
            Return
          End If
          DownloadResults(RunName) = "Error: The server sent an empty response. Please try again."
        Else
          DownloadResults(RunName) = sRet
        End If
      End Using
    Catch ex As Exception
      DownloadResults(RunName) = "Error: " & ex.Message
    End Try
  End Sub
  Public Function DownloadString(address As String) As String
    Dim RunName As String = "DOWNLOADSTRING_" & address & "_" & Int(Rnd() * &H10000)
    DownloadResults.Add(RunName, Nothing)
    Dim tDownload As New Threading.Thread(AddressOf AsyncDownloadString)
    c_Busy = True
    tDownload.Start({RunName, address, 0})
    Dim WaitTime As Long = srlFunctions.TickCount() + (c_Timeout * 1000)
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
      ElseIf srlFunctions.TickCount() > WaitTime Then
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
        wsDownload.SendCookieJar = c_SendJar
        wsDownload.Encoding = c_Encoding
        wsDownload.ErrorBypass = c_ErrorBypass
        wsDownload.KeepAlive = c_KeepAlive
        wsDownload.ManualRedirect = c_ManualRedirect
        If Not c_SendHeaders Is Nothing Then
          For I As Integer = 0 To c_SendHeaders.Count - 1
            wsDownload.Headers.Add(c_SendHeaders.GetKey(I), c_SendHeaders(I))
          Next
        End If
        Dim sRet As String = Nothing
        Try
          sRet = wsDownload.DownloadString(address)
          c_Jar = wsDownload.CookieJar
          c_ResponseURI = wsDownload.ResponseURI
          c_ResponseCode = wsDownload.ResponseCode
        Catch ex As Exception
          Dim sNetErr As String = srlFunctions.NetworkErrorToString(ex, sDataPath)
          c_Jar = wsDownload.CookieJar
          If sNetErr.Contains("Please try again.") And iteration < 5 Then
            AsyncDownloadStringWithCallback({callback, aState, address, iteration + 1})
          Else
            c_ResponseURI = wsDownload.ResponseURI
            c_ResponseCode = wsDownload.ResponseCode
            callback(aState, "Error: " & sNetErr)
          End If
          Return
        End Try
        If String.IsNullOrEmpty(sRet) Then
          Dim sNewPath As String = CheckHeaderRedirect(wsDownload.ResponseHeaders, IIf(c_ResponseURI Is Nothing, address, c_ResponseURI.OriginalString))
          If Not String.IsNullOrEmpty(sNewPath) Then
            AsyncDownloadStringWithCallback({callback, aState, sNewPath, 0})
            Return
          End If
          callback(aState, "Error: The server sent an empty response. Please try again.")
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
        wsUpload.SendCookieJar = c_SendJar
        wsUpload.Encoding = c_Encoding
        wsUpload.ErrorBypass = c_ErrorBypass
        wsUpload.KeepAlive = c_KeepAlive
        wsUpload.ManualRedirect = c_ManualRedirect
        If Not c_SendHeaders Is Nothing Then
          For I As Integer = 0 To c_SendHeaders.Count - 1
            wsUpload.Headers.Add(c_SendHeaders.GetKey(I), c_SendHeaders(I))
          Next
        End If
        Dim sRet As String = Nothing
        If method.ToUpperInvariant = "POST" Then
          Dim hasCT As Boolean = False
          For Each hdr In wsUpload.Headers.AllKeys
            If hdr.ToUpperInvariant = "CONTENT-TYPE" Then
              hasCT = True
              Exit For
            End If
          Next
          If Not hasCT Then wsUpload.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
          Dim uriAddr As Uri
          Try
            uriAddr = New Uri(address)
          Catch ex As Exception
            UploadResults(RunName) = "Error: " & address & " is not a valid URI."
            Return
          End Try
          Try
            sRet = wsUpload.UploadString(uriAddr, method, data)
            c_Jar = wsUpload.CookieJar
            c_ResponseURI = wsUpload.ResponseURI
            c_ResponseCode = wsUpload.ResponseCode
          Catch ex As Exception
            Dim sNetErr As String = srlFunctions.NetworkErrorToString(ex, sDataPath)
            c_Jar = wsUpload.CookieJar
            If sNetErr.Contains("Please try again.") And iteration < 5 Then
              AsyncUploadString({RunName, address, method, data, iteration + 1})
            Else
              c_ResponseURI = wsUpload.ResponseURI
              c_ResponseCode = wsUpload.ResponseCode
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
            'TODO: Make this asynchronous, maybe?
            sRet = wsUpload.DownloadString(uriAddr)
            c_Jar = wsUpload.CookieJar
            c_ResponseURI = wsUpload.ResponseURI
            c_ResponseCode = wsUpload.ResponseCode
            '
          Catch ex As Exception
            Dim sNetErr As String = srlFunctions.NetworkErrorToString(ex, sDataPath)
            c_Jar = wsUpload.CookieJar
            If sNetErr.Contains("Please try again.") And iteration < 5 Then
              AsyncUploadString({RunName, address, method, data, iteration + 1})
            Else
              c_ResponseURI = wsUpload.ResponseURI
              c_ResponseCode = wsUpload.ResponseCode
              UploadResults(RunName) = "Error: " & sNetErr
            End If
            Return
          End Try
        End If
        If String.IsNullOrEmpty(sRet) Then
          Dim sNewPath As String = CheckHeaderRedirect(wsUpload.ResponseHeaders, IIf(c_ResponseURI Is Nothing, address, c_ResponseURI.OriginalString))
          If Not String.IsNullOrEmpty(sNewPath) Then
            AsyncUploadString({RunName, sNewPath, "GET", Nothing, 0})
            Return
          End If
          UploadResults(RunName) = "Error: The server sent an empty response. Please try again."
        Else
          UploadResults(RunName) = sRet
        End If
      End Using
    Catch ex As Exception
      UploadResults(RunName) = "Error: " & ex.Message
    End Try
  End Sub
  Public Function UploadString(address As String, method As String, data As String) As String
    Dim RunName As String = "UPLOADSTRING_" & address & "_" & Int(Rnd() * &H10000)
    UploadResults.Add(RunName, Nothing)
    Dim tUpload As New Threading.Thread(AddressOf AsyncUploadString)
    c_Busy = True
    tUpload.Start({RunName, address, method, data, 0})
    Dim WaitTime As Long = srlFunctions.TickCount() + (c_Timeout * 1000)
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
      ElseIf srlFunctions.TickCount() > WaitTime Then
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
        wsUpload.SendCookieJar = c_SendJar
        wsUpload.Encoding = c_Encoding
        wsUpload.ErrorBypass = c_ErrorBypass
        wsUpload.KeepAlive = c_KeepAlive
        wsUpload.ManualRedirect = c_ManualRedirect
        If Not c_SendHeaders Is Nothing Then
          For I As Integer = 0 To c_SendHeaders.Count - 1
            wsUpload.Headers.Add(c_SendHeaders.GetKey(I), c_SendHeaders(I))
          Next
        End If
        Dim sRet As String = Nothing
        If method.ToUpperInvariant = "POST" Then
          Dim hasCT As Boolean = False
          For Each hdr In wsUpload.Headers.AllKeys
            If hdr.ToUpperInvariant = "CONTENT-TYPE" Then
              hasCT = True
              Exit For
            End If
          Next
          If Not hasCT Then wsUpload.Headers.Add(Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
          Try
            sRet = wsUpload.UploadString(address, method, data)
            c_Jar = wsUpload.CookieJar
            c_ResponseURI = wsUpload.ResponseURI
            c_ResponseCode = wsUpload.ResponseCode
          Catch ex As Exception
            Dim sNetErr As String = srlFunctions.NetworkErrorToString(ex, sDataPath)
            c_Jar = wsUpload.CookieJar
            If sNetErr.Contains("Please try again.") And iteration < 5 Then
              AsyncUploadStringWithCallback({callback, aState, address, method, data, iteration + 1})
            Else
              c_ResponseURI = wsUpload.ResponseURI
              c_ResponseCode = wsUpload.ResponseCode
              callback(aState, "Error: " & sNetErr)
            End If
            Return
          End Try
        Else
          Try
            sRet = wsUpload.DownloadString(address)
            c_Jar = wsUpload.CookieJar
            c_ResponseURI = wsUpload.ResponseURI
            c_ResponseCode = wsUpload.ResponseCode
          Catch ex As Exception
            Dim sNetErr As String = srlFunctions.NetworkErrorToString(ex, sDataPath)
            c_Jar = wsUpload.CookieJar
            If sNetErr.Contains("Please try again.") And iteration < 5 Then
              AsyncUploadStringWithCallback({callback, aState, address, method, data, iteration + 1})
            Else
              c_ResponseURI = wsUpload.ResponseURI
              c_ResponseCode = wsUpload.ResponseCode
              callback(aState, "Error: " & sNetErr)
            End If
            Return
          End Try
        End If
        If String.IsNullOrEmpty(sRet) Then
          Dim sNewPath As String = CheckHeaderRedirect(wsUpload.ResponseHeaders, IIf(c_ResponseURI Is Nothing, address, c_ResponseURI.OriginalString))
          If Not String.IsNullOrEmpty(sNewPath) Then
            AsyncUploadStringWithCallback({callback, aState, sNewPath, "GET", Nothing, 0})
            Return
          End If
          callback(aState, "Error: The server sent an empty response. Please try again.")
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
        wsUpload.SendCookieJar = c_SendJar
        wsUpload.Encoding = c_Encoding
        wsUpload.ErrorBypass = c_ErrorBypass
        wsUpload.KeepAlive = c_KeepAlive
        wsUpload.ManualRedirect = c_ManualRedirect
        If Not c_SendHeaders Is Nothing Then
          For I As Integer = 0 To c_SendHeaders.Count - 1
            wsUpload.Headers.Add(c_SendHeaders.GetKey(I), c_SendHeaders(I))
          Next
        End If
        Dim sRet As String = Nothing
        If method.ToUpperInvariant = "POST" And (data IsNot Nothing AndAlso data.Count > 0) Then
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
            c_Jar = wsUpload.CookieJar
            c_ResponseURI = wsUpload.ResponseURI
            c_ResponseCode = wsUpload.ResponseCode
          Catch ex As Exception
            Dim sNetErr As String = srlFunctions.NetworkErrorToString(ex, sDataPath)
            c_Jar = wsUpload.CookieJar
            If sNetErr.Contains("Please try again.") And iteration < 5 Then
              AsyncUploadValues({RunName, address, method, data, iteration + 1})
            Else
              c_ResponseURI = wsUpload.ResponseURI
              c_ResponseCode = wsUpload.ResponseCode
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
            c_Jar = wsUpload.CookieJar
            c_ResponseURI = wsUpload.ResponseURI
            c_ResponseCode = wsUpload.ResponseCode
          Catch ex As Exception
            Dim sNetErr As String = srlFunctions.NetworkErrorToString(ex, sDataPath)
            c_Jar = wsUpload.CookieJar
            If sNetErr.Contains("Please try again.") And iteration < 5 Then
              AsyncUploadValues({RunName, address, method, data, iteration + 1})
            Else
              c_ResponseURI = wsUpload.ResponseURI
              c_ResponseCode = wsUpload.ResponseCode
              UploadResults(RunName) = "Error: " & sNetErr
            End If
            Return
          End Try
        End If
        If String.IsNullOrEmpty(sRet) Then
          Dim sNewPath As String = CheckHeaderRedirect(wsUpload.ResponseHeaders, IIf(c_ResponseURI Is Nothing, address, c_ResponseURI.OriginalString))
          If Not String.IsNullOrEmpty(sNewPath) Then
            AsyncUploadValues({RunName, sNewPath, "GET", Nothing, 0})
            Return
          End If
          UploadResults(RunName) = "Error: The server sent an empty response. Please try again."
        Else
          UploadResults(RunName) = sRet
        End If
      End Using
    Catch ex As Exception
      UploadResults(RunName) = "Error: " & ex.Message
    End Try
  End Sub
  Public Function UploadValues(address As String, method As String, data As Specialized.NameValueCollection) As String
    Dim RunName As String = "UPLOADVAUES_" & address & "_" & Int(Rnd() * &H10000)
    UploadResults.Add(RunName, Nothing)
    Dim tUpload As New Threading.Thread(AddressOf AsyncUploadValues)
    c_Busy = True
    tUpload.Start({RunName, address, method, data, 0})
    Dim WaitTime As Long = srlFunctions.TickCount() + (c_Timeout * 1000)
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
      ElseIf srlFunctions.TickCount() > WaitTime Then
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
        wsUpload.SendCookieJar = c_SendJar
        wsUpload.Encoding = c_Encoding
        wsUpload.ErrorBypass = c_ErrorBypass
        wsUpload.KeepAlive = c_KeepAlive
        wsUpload.ManualRedirect = c_ManualRedirect
        If Not c_SendHeaders Is Nothing Then
          For I As Integer = 0 To c_SendHeaders.Count - 1
            wsUpload.Headers.Add(c_SendHeaders.GetKey(I), c_SendHeaders(I))
          Next
        End If
        Dim sRet As String = Nothing
        If method.ToUpperInvariant = "POST" And (data IsNot Nothing AndAlso data.Count > 0) Then
          Dim bRet() As Byte = Nothing
          Try
            bRet = wsUpload.UploadValues(address, method, data)
            c_Jar = wsUpload.CookieJar
            c_ResponseURI = wsUpload.ResponseURI
            c_ResponseCode = wsUpload.ResponseCode
          Catch ex As Exception
            Dim sNetErr As String = srlFunctions.NetworkErrorToString(ex, sDataPath)
            c_Jar = wsUpload.CookieJar
            If sNetErr.Contains("Please try again.") And iteration < 5 Then
              AsyncUploadValuesWithCallback({callback, aState, address, method, data, iteration + 1})
            Else
              c_ResponseURI = wsUpload.ResponseURI
              c_ResponseCode = wsUpload.ResponseCode
              callback(aState, "Error: " & sNetErr)
            End If
            Return
          End Try
          sRet = wsUpload.Encoding.GetString(bRet)
        Else
          Try
            sRet = wsUpload.DownloadString(address)
            c_Jar = wsUpload.CookieJar
            c_ResponseURI = wsUpload.ResponseURI
            c_ResponseCode = wsUpload.ResponseCode
          Catch ex As Exception
            Dim sNetErr As String = srlFunctions.NetworkErrorToString(ex, sDataPath)
            c_Jar = wsUpload.CookieJar
            If sNetErr.Contains("Please try again.") And iteration < 5 Then
              AsyncUploadValuesWithCallback({callback, aState, address, method, data, iteration + 1})
            Else
              c_ResponseURI = wsUpload.ResponseURI
              c_ResponseCode = wsUpload.ResponseCode
              callback(aState, "Error: " & sNetErr)
            End If
            Return
          End Try
        End If
        If String.IsNullOrEmpty(sRet) Then
          Dim sNewPath As String = CheckHeaderRedirect(wsUpload.ResponseHeaders, IIf(c_ResponseURI Is Nothing, address, c_ResponseURI.OriginalString))
          If Not String.IsNullOrEmpty(sNewPath) Then
            AsyncUploadValuesWithCallback({callback, aState, sNewPath, "GET", Nothing, 0})
            Return
          End If
          callback(aState, "Error: The server sent an empty response. Please try again.")
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
  Private Function CheckHeaderRedirect(HeaderData As Net.WebHeaderCollection, SourceAddr As String) As String
    For Each Key In HeaderData.AllKeys
      If Key.ToUpperInvariant = "LOCATION" Then
        Dim sNewPath As String = HeaderData.Item(Key)
        If sNewPath.StartsWith("/") Then
          sNewPath = SourceAddr.Substring(0, SourceAddr.IndexOf("/", SourceAddr.IndexOf("//") + 2)) & sNewPath
        ElseIf Not sNewPath.Contains("//") Then
          sNewPath = SourceAddr.Substring(0, SourceAddr.LastIndexOf("/") + 1) & sNewPath
        End If
        Return sNewPath
      End If
    Next
    Return Nothing
  End Function
End Class

''' <summary>
''' Specifies the security protocols that are supported by the Schannel security package.
''' </summary>
''' <remarks>This contains the additional TLS 1.1, 1.2, and 1.3 standards lacking in <see cref="System.Net.SecurityProtocolType" />, which require newer .NET Framework versions.</remarks>
Public Enum SecurityProtocolTypeEx As Integer
  ''' <summary>
  ''' Specifies no security protocol is to be used.
  ''' </summary>
  ''' <remarks>This value is technically invalid, but exists for use with Settings to allow no protocol to be selected in the case of a Remote Usage Service subscription.</remarks>
  None = 0
  ''' <summary>
  ''' Specifies the Secure Socket Layer (SSL) 3.0 security protocol.
  ''' </summary>
  Ssl3 = &H30
  ''' <summary>
  ''' Specifies the Transport Layer Security (TLS) 1.0 security protocol.
  ''' </summary>
  Tls10 = &HC0
  ''' <summary>
  ''' Specifies the Transport Layer Security (TLS) 1.1 security protocol.
  ''' </summary>
  Tls11 = &H300
  ''' <summary>
  ''' Specifies the Transport Layer Security (TLS) 1.2 security protocol.
  ''' </summary>
  Tls12 = &HC00
  ''' <summary>
  ''' Specifies the Transport Layer Security (TLS) 1.3 security protocol.
  ''' </summary>
  Tls13 = &H3000
End Enum