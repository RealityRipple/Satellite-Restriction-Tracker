Public Class DetermineType
  Public Class TypeDeterminedEventArgs
    Inherits EventArgs
    Public Enum SatHostGroup
      DishNet
      RuralPortal
      WildBlue
      Other
    End Enum
    Public HostGroup As SatHostGroup
    Public Sub New(Type As SatHostGroup)
      HostGroup = Type
    End Sub
  End Class
  Private Class URLChecker
    Public Class CheckEventArgs
      Inherits EventArgs
      Public Result As Boolean
      Public Sub New(bRet As Boolean)
        Result = bRet
      End Sub
    End Class
    Public Event CheckResult(UserState As Object, e As CheckEventArgs)
    Private wRequest As Net.WebRequest
    Private sAddr As String
    Public Sub New()
      wRequest = Nothing
      sAddr = String.Empty
    End Sub
    Public Sub CheckURLAsync(UserState As Object, HostAddress As String, iTimeout As Integer, pProxy As Net.IWebProxy)
      sAddr = HostAddress
      If HostAddress.IndexOf("://") < 0 Then HostAddress = "http://" & HostAddress
      wRequest = System.Net.WebRequest.Create(HostAddress)
      wRequest.Timeout = iTimeout
      wRequest.Proxy = pProxy
      Try
        wRequest.BeginGetResponse(New AsyncCallback(AddressOf URLCheckResponse), UserState)
      Catch ex As Exception
        RaiseEvent CheckResult(UserState, New CheckEventArgs(False))
      End Try
    End Sub
    Private Sub URLCheckResponse(ar As IAsyncResult)
      Try
        Dim wResponse As Net.WebResponse = wRequest.EndGetResponse(ar)
        If wResponse.ResponseUri.AbsoluteUri().ToString.IndexOf(sAddr) > -1 Then
          Dim sData As String = Nothing
          Using wData As IO.Stream = wResponse.GetResponseStream
            Using readStream As New IO.StreamReader(wData, System.Text.Encoding.GetEncoding(LATIN_1))
              sData = readStream.ReadToEnd
            End Using
          End Using
          If String.IsNullOrEmpty(sData) Then
            RaiseEvent CheckResult(ar.AsyncState, New CheckEventArgs(False))
          ElseIf sData.ToLower.Contains("<meta http-equiv=""refresh""") Then
            RaiseEvent CheckResult(ar.AsyncState, New CheckEventArgs(False))
          Else
            RaiseEvent CheckResult(ar.AsyncState, New CheckEventArgs(True))
          End If
        Else
          RaiseEvent CheckResult(ar.AsyncState, New CheckEventArgs(False))
        End If
        wResponse.Close()
        wResponse = Nothing
      Catch ex As Exception
        RaiseEvent CheckResult(ar.AsyncState, New CheckEventArgs(False))
      End Try
    End Sub
  End Class
  Private WithEvents uChecker As URLChecker
  Public Event TypeDetermined(UserState As Object, e As TypeDeterminedEventArgs)
  Private Delegate Sub ParamaterizedInvoker(parameter As Object)
  Private sProvider As String
  Private iTimeout As Integer
  Private pProxy As Net.IWebProxy
  Public Sub New(Provider As String, UserState As Object, Timeout As Integer, Proxy As Net.IWebProxy)
    iTimeout = Timeout
    pProxy = Proxy
    Dim testCallback As New ParamaterizedInvoker(AddressOf BeginTest)
    testCallback.BeginInvoke({Provider, UserState}, Nothing, Nothing)
  End Sub
  Private Sub BeginTest(state As Object)
    Dim Provider As String = state(0)
    Dim UserState As Object = state(1)
    If Provider.ToLower = "dish.com" Or Provider.ToLower = "dish.net" Then
      RaiseEvent TypeDetermined(UserState, New TypeDeterminedEventArgs(TypeDeterminedEventArgs.SatHostGroup.DishNet))
    Else
      If Provider.Contains(".") Then Provider = Provider.Substring(0, Provider.LastIndexOf("."))
      sProvider = Provider
      uChecker = New URLChecker
      uChecker.CheckURLAsync({"NET", UserState}, "wildblue.com", iTimeout, pProxy)
    End If
  End Sub
  Private Sub uChecker_CheckResult(UserState As Object, e As URLChecker.CheckEventArgs) Handles uChecker.CheckResult
    Dim Source() As Object = UserState
    Dim sSource As String = Source(0).ToString
    Select Case sSource
      Case "NET"
        If e.Result Then
          uChecker.CheckURLAsync({"RP", Source(1)}, sProvider & ".ruralportal.net", iTimeout, pProxy)
        Else
          RaiseEvent TypeDetermined(Source(1), New TypeDeterminedEventArgs(TypeDeterminedEventArgs.SatHostGroup.Other))
        End If
      Case "RP"
        If e.Result Then
          RaiseEvent TypeDetermined(Source(1), New TypeDeterminedEventArgs(TypeDeterminedEventArgs.SatHostGroup.RuralPortal))
        Else
          uChecker.CheckURLAsync({"MYA", Source(1)}, "myaccount." & sProvider & ".net", iTimeout, pProxy)
        End If
      Case "MYA"
        If e.Result Then
          RaiseEvent TypeDetermined(Source(1), New TypeDeterminedEventArgs(TypeDeterminedEventArgs.SatHostGroup.WildBlue))
        Else
          RaiseEvent TypeDetermined(Source(1), New TypeDeterminedEventArgs(TypeDeterminedEventArgs.SatHostGroup.Other))
        End If
    End Select
  End Sub
End Class
