''' <summary>
''' Class for determining the account type for a provider domain name.
''' </summary>
Public Class DetermineType
  ''' <summary>
  ''' Enumeration of provider types, which generally describe the login system, though not necessarily the exact provider or usage type.
  ''' </summary>
  Public Enum SatHostGroup
    ''' <summary>
    ''' Provider uses Dish login system (Unique to mydish.com).
    ''' </summary>
    Dish
    ''' <summary>
    ''' Provider uses general RuralPortal login system (DOMAIN.ruralportal.net).
    ''' </summary>
    RuralPortal
    ''' <summary>
    ''' Provider uses legacy WildBlue login system (myaccount.DOMAIN/wbisp/DOMAIN).
    ''' </summary>
    WildBlue
    ''' <summary>
    ''' Provider uses modern Exede login system (exede.net).
    ''' </summary>
    Exede
    ''' <summary>
    ''' Provider uses last-gen Exede login system (satelliteinternetco.com, uses AJAX).
    ''' </summary>
    ExedeReseller
    ''' <summary>
    ''' The provider is unknown or hasn't been determined yet.
    ''' </summary>
    Other
  End Enum
  Private Class URLChecker
    Public Delegate Sub CheckCallback(asyncState As Object, success As Boolean)
    Private c_callback As CheckCallback
    Private wRequest As Net.HttpWebRequest
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
      wRequest = System.Net.HttpWebRequest.Create(Addr)
      wRequest.Timeout = Timeout * 1000
      wRequest.Proxy = Proxy
      Try
        wRequest.BeginGetResponse(New AsyncCallback(AddressOf URLCheckResponse), state)
      Catch ex As Exception
        c_callback.Invoke(state, False)
      End Try
    End Sub
    Private Sub URLCheckResponse(ar As IAsyncResult)
      Try
        Dim wResponse As Net.HttpWebResponse = wRequest.EndGetResponse(ar)
        If wResponse.ResponseUri.AbsoluteUri().ToString.IndexOf(sAddr) > -1 Then
          Dim sData As String = Nothing
          Using wData As IO.Stream = wResponse.GetResponseStream
            Using readStream As New IO.StreamReader(wData, System.Text.Encoding.GetEncoding(srlFunctions.LATIN_1))
              sData = readStream.ReadToEnd
            End Using
          End Using
          If String.IsNullOrEmpty(sData) Then
            c_callback.Invoke(ar.AsyncState, False)
          ElseIf sData.ToUpperInvariant.Contains("<META HTTP-EQUIV=""REFREQH""") Then
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
  ''' <summary>
  ''' Callback subroutine to be triggered when a type has been determined in the <see cref="DetermineType" /> class.
  ''' </summary>
  ''' <param name="HostGroup">Provider type generally describing the provider domain name's login system.</param>
  Public Delegate Sub TypeDeterminedCallback(HostGroup As SatHostGroup)
  Private sProvider As String
  Private iTimeout As Integer
  Private pProxy As Net.IWebProxy
  Private c_callback As TypeDeterminedCallback
  ''' <summary>
  ''' Constructor for <see cref="DetermineType" /> class, which also begins the determination procedure.
  ''' </summary>
  ''' <param name="Provider">URL to determine the type of.</param>
  ''' <param name="Timeout">Number of seconds to wait for a response from the server while testing the connection.</param>
  ''' <param name="Proxy">Proxy settings for testing the servers.</param>
  ''' <param name="callback">Callback subroutine to be triggered when the type has been determined.</param>
  Public Sub New(Provider As String, Timeout As Integer, Proxy As Net.IWebProxy, callback As TypeDeterminedCallback)
    iTimeout = Timeout * 1000
    pProxy = Proxy
    c_callback = callback
    Dim beginInvoker As New BeginTestInvoker(AddressOf BeginTest)
    beginInvoker.BeginInvoke(Provider, Nothing, Nothing)
  End Sub
  Private Delegate Sub BeginTestInvoker(Provider As String)
  Private Sub BeginTest(Provider As String)
    If Provider.ToUpperInvariant = "MYDISH.COM" Or Provider.ToUpperInvariant = "DISH.COM" Or Provider.ToUpperInvariant = "DISH.NET" Then
      c_callback.Invoke(SatHostGroup.Dish)
    ElseIf Provider.ToUpperInvariant = "EXEDE.COM" Or Provider.ToUpperInvariant = "EXEDE.NET" Then
      c_callback.Invoke(SatHostGroup.Exede)
    ElseIf Provider.ToUpperInvariant = "SATELLITEINTERNETCO.COM" Then
      c_callback.Invoke(SatHostGroup.ExedeReseller)
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

''' <summary>
''' Class for grabbing Exede AJAX Lists
''' </summary>
Public Class UpdateAJAXLists
  ''' <summary>
  ''' Callback subroutine to be triggered when a type has been determined in the <see cref="UpdateAJAXLists" /> class.
  ''' </summary>
  Public Delegate Sub UpdateCallback(asyncState As Object, shortList As String, fullList As String)
  Private c_callback As UpdateCallback
  Private wRequest As Net.HttpWebRequest
  Private sAddr As String
  ''' <summary>
  ''' Constructor for <see cref="UpdateAJAXLists" /> class, which also begins the update procedure.
  ''' </summary>
  ''' <param name="HostAddress">Host to determine the AJAX Lists of.</param>
  ''' <param name="iTimeout">Number of seconds to wait for a response from the server while testing the connection.</param>
  ''' <param name="pProxy">Proxy settings for testing the servers.</param>
  ''' <param name="callback">Callback subroutine to be triggered when the type has been determined.</param>
  Public Sub New(HostAddress As String, iTimeout As Integer, pProxy As Net.IWebProxy, asyncState As Object, callback As UpdateCallback)
    wRequest = Nothing
    sAddr = String.Empty
    c_callback = callback
    Dim beginInvoker As New BeginCheckInvoker(AddressOf BeginCheck)
    beginInvoker.BeginInvoke(HostAddress, iTimeout, pProxy, asyncState, Nothing, Nothing)
  End Sub
  Private Delegate Sub BeginCheckInvoker(Host As String, Timeout As Integer, Proxy As Net.IWebProxy, state As Object)
  Private Sub BeginCheck(Host As String, Timeout As Integer, Proxy As Net.IWebProxy, state As Object)
    wRequest = System.Net.HttpWebRequest.Create("http://wb.realityripple.com/hosts/erAJAX.php?h=" & Host)
    wRequest.Timeout = Timeout * 1000
    wRequest.Proxy = Proxy
    Try
      wRequest.BeginGetResponse(New AsyncCallback(AddressOf AJAXCheckResponse), state)
    Catch ex As Exception
      c_callback.Invoke(state, Nothing, Nothing)
    End Try
  End Sub
  Private Sub AJAXCheckResponse(ar As IAsyncResult)
    Try
      Dim wResponse As Net.HttpWebResponse = wRequest.EndGetResponse(ar)
      If wResponse.ResponseUri.AbsoluteUri().ToString.IndexOf(sAddr) > -1 Then
        Dim sData As String = Nothing
        Using wData As IO.Stream = wResponse.GetResponseStream
          Using readStream As New IO.StreamReader(wData, System.Text.Encoding.GetEncoding(srlFunctions.LATIN_1))
            sData = readStream.ReadToEnd
          End Using
        End Using
        If String.IsNullOrEmpty(sData) Then
          c_callback.Invoke(ar.AsyncState, "DATA_EMPTY", Nothing)
        ElseIf sData.ToUpperInvariant.Contains("<META HTTP-EQUIV=""REFRESH""") Then
          c_callback.Invoke(ar.AsyncState, "DATA_REDIR_" & sData, Nothing)
        ElseIf Not sData.Contains(vbLf) Then
          c_callback.Invoke(ar.AsyncState, "DATA_SEP_" & sData, Nothing)
        Else
          Dim minAndFull() As String = Split(sData, vbLf, 2)
          c_callback.Invoke(ar.AsyncState, minAndFull(0), minAndFull(1))
        End If
      Else
        c_callback.Invoke(ar.AsyncState, "URL_" & sAddr, Nothing)
      End If
      wResponse.Close()
      wResponse = Nothing
    Catch ex As Exception
      c_callback.Invoke(ar.AsyncState, "ERR_" & ex.ToString, Nothing)
    End Try
  End Sub
End Class
