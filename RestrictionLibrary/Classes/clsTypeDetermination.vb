Public Class DetermineType
  Public Enum SatHostGroup
    DishNet
    RuralPortal
    WildBlue
    Exede
    Other
  End Enum
  Private Class URLChecker
    Public Delegate Sub CheckCallback(asyncState As Object, success As Boolean)
    Private c_callback As CheckCallback
    Private wRequest As Net.WebRequest
    Private sAddr As String
    Public Sub New(HostAddress As String, iTimeout As Integer, pProxy As Net.IWebProxy, asyncState As Object, callback As CheckCallback)
      wRequest = Nothing
      sAddr = String.Empty
      c_callback = callback
      Dim beginInvoker As New BeginCheckInvoker(AddressOf BeginCheck)
      beginInvoker.BeginInvoke(HostAddress, iTimeout, pProxy, asyncState, Nothing, Nothing)
    End Sub
    Private Delegate Sub BeginCheckInvoker(Addr As String, Timeout As Integer, Proxy As Net.IWebProxy, state As Object)
    Private Sub BeginCheck(Addr As String, Timeout As Integer, Proxy As Net.IWebProxy, state As Object)
      sAddr = Addr
      If Addr.IndexOf("://") < 0 Then Addr = "http://" & Addr
      wRequest = System.Net.WebRequest.Create(Addr)
      wRequest.Timeout = Timeout
      wRequest.Proxy = Proxy
      Try
        wRequest.BeginGetResponse(New AsyncCallback(AddressOf URLCheckResponse), state)
      Catch ex As Exception
        c_callback.Invoke(state, False)
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
            c_callback.Invoke(ar.AsyncState, False)
          ElseIf sData.ToLower.Contains("<meta http-equiv=""refresh""") Then
            c_callback.Invoke(ar.AsyncState, False)
          Else
            c_callback.Invoke(ar.AsyncState, True)
          End If
        Else
          c_callback.Invoke(ar.AsyncState, False)
        End If
        wResponse.Close()
        wResponse = Nothing
      Catch ex As Exception
        c_callback.Invoke(ar.AsyncState, False)
      End Try
    End Sub
  End Class
  Public Delegate Sub TypeDeterminedCallback(HostGroup As SatHostGroup)
  Private sProvider As String
  Private iTimeout As Integer
  Private pProxy As Net.IWebProxy
  Private c_callback As TypeDeterminedCallback
  Public Sub New(Provider As String, Timeout As Integer, Proxy As Net.IWebProxy, callback As TypeDeterminedCallback)
    iTimeout = Timeout
    pProxy = Proxy
    c_callback = callback
    Dim beginInvoker As New BeginTestInvoker(AddressOf BeginTest)
    beginInvoker.BeginInvoke(Provider, Nothing, Nothing)
  End Sub
  Private Delegate Sub BeginTestInvoker(Provider As String)
  Private Sub BeginTest(Provider As String)
    If Provider.ToLower = "dish.com" Or Provider.ToLower = "dish.net" Then
      c_callback.Invoke(SatHostGroup.DishNet)
    ElseIf Provider.ToLower = "exede.com" Or Provider.ToLower = "exede.net" Then
      c_callback.Invoke(SatHostGroup.Exede)
    Else
      If Provider.Contains(".") Then Provider = Provider.Substring(0, Provider.LastIndexOf("."))
      sProvider = Provider
      Dim check As New URLChecker("wildblue.com", iTimeout, pProxy, "NET", AddressOf uChecker_CheckResult)
    End If
  End Sub
  Private Sub uChecker_CheckResult(asyncState As Object, success As Boolean)
    Select Case asyncState
      Case "NET"
        If success Then
          Dim check As New URLChecker(sProvider & ".ruralportal.net", iTimeout, pProxy, "RP", AddressOf uChecker_CheckResult)
        Else
          c_callback.Invoke(SatHostGroup.Other)
        End If
      Case "RP"
        If success Then
          c_callback.Invoke(SatHostGroup.RuralPortal)
        Else
          Dim check As New URLChecker("myaccount." & sProvider & ".net", iTimeout, pProxy, "MYA", AddressOf uChecker_CheckResult)
        End If
      Case "MYA"
        If success Then
          c_callback.Invoke(SatHostGroup.WildBlue)
        Else
          c_callback.Invoke(SatHostGroup.Other)
        End If
    End Select
  End Sub
End Class
