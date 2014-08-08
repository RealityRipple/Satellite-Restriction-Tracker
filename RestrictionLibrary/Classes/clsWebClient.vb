Public Class CookieAwareWebClient
  Inherits Net.WebClient
  Sub New()
    Me.New(New Net.CookieContainer)
    System.Net.ServicePointManager.Expect100Continue = False
  End Sub
  Sub New(c As Net.CookieContainer)
    Me.CookieJar = c
    c_Timeout = 60
    c_HTVer = Net.HttpVersion.Version10
    c_ErrorBypass = False
  End Sub
  Sub New(v As Version)
    Me.New(New Net.CookieContainer)
    c_HTVer = v
  End Sub
  Public CookieJar As Net.CookieContainer
  Public ResponseURI As Uri
  Public Class ErrorEventArgs
    Public [Error] As Exception
    Public Sub New(Err As Exception)
      [Error] = Err
    End Sub
  End Class
  Private c_Timeout As Integer
  Public Property Timeout As Integer
    Get
      Return c_Timeout
    End Get
    Set(value As Integer)
      c_Timeout = value
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
  Public Event Failure(sender As Object, e As ErrorEventArgs)
  Protected Overrides Function GetWebRequest(address As System.Uri) As System.Net.WebRequest
    Try
      Dim request As Net.WebRequest = MyBase.GetWebRequest(address)
      If request.GetType Is GetType(Net.HttpWebRequest) Then
        CType(request, Net.HttpWebRequest).UserAgent = "Mozilla/5.0 (" & Environment.OSVersion.VersionString & ") " & My.Application.Info.ProductName & "/" & My.Application.Info.Version.ToString
        CType(request, Net.HttpWebRequest).ReadWriteTimeout = c_Timeout * 1000
        CType(request, Net.HttpWebRequest).Timeout = c_Timeout * 1000
        CType(request, Net.HttpWebRequest).CookieContainer = Me.CookieJar
        CType(request, Net.HttpWebRequest).AllowAutoRedirect = True
        CType(request, Net.HttpWebRequest).KeepAlive = False
        CType(request, Net.HttpWebRequest).ProtocolVersion = HTTPVersion
      End If
      Return request
    Catch ex As Net.WebException
      MyBase.CancelAsync()
      RaiseEvent Failure(Me, New ErrorEventArgs(ex))
      Return Nothing
    End Try
  End Function
  Protected Overrides Function GetWebResponse(request As System.Net.WebRequest) As System.Net.WebResponse
    Try
      Dim response As Net.WebResponse = MyBase.GetWebResponse(request)
      ResponseURI = response.ResponseUri
      Return response
    Catch ex As Net.WebException
      If c_ErrorBypass Then
        Dim response As Net.WebResponse = ex.Response
        ResponseURI = response.ResponseUri
        Return response
      End If
      MyBase.CancelAsync()
      RaiseEvent Failure(Me, New ErrorEventArgs(ex))
      Return Nothing
    End Try
  End Function
  Protected Overrides Function GetWebResponse(request As System.Net.WebRequest, result As System.IAsyncResult) As System.Net.WebResponse
    Try
      Dim response As Net.WebResponse = MyBase.GetWebResponse(request, result)
      ResponseURI = response.ResponseUri
      Return response
    Catch ex As Net.WebException
      If c_ErrorBypass Then
        Dim response As Net.WebResponse = ex.Response
        ResponseURI = response.ResponseUri
        Return response
      End If
      If ex.Message = "The request was aborted: The request was canceled." Then Return Nothing
      MyBase.CancelAsync()
      RaiseEvent Failure(Me, New ErrorEventArgs(ex))
      Return Nothing
    End Try
  End Function
End Class
