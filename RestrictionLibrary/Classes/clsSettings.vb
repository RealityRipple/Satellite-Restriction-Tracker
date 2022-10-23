Imports System.Xml
Class AppSettings
  Private m_Config As String
  Private m_Account As String
  Private m_Interval As Integer
  Private m_PassCrypt As String
  Private m_PassKey As String
  Private m_PassSalt As String
  Private m_Timeout As Integer
  Private m_TLSProxy As Boolean
  Private m_ProxySetting As String
  Private m_SecurProtocol As Net.SecurityProtocolType
  Private m_SecurEnforced As Boolean
  Private ReadOnly Property ConfigFile As String
    Get
      Return m_Config
    End Get
  End Property
  Public Sub New(Config As String)
    m_Config = Config
    If My.Computer.FileSystem.FileExists(m_Config) Then
      Dim m_xmld As New XmlDocument
      m_xmld.Load(ConfigFile)
      If m_xmld.HasChildNodes Then
        If m_xmld.ChildNodes.Count > 1 Then
          Dim xConfig As XmlNode = m_xmld.ChildNodes(1)
          If xConfig.HasChildNodes Then
            Dim xUserSettings As XmlNode = xConfig.ChildNodes(0)
            If xUserSettings.HasChildNodes Then
              Dim xMySettings As XmlNode = xUserSettings.ChildNodes(0)
              If xMySettings.HasChildNodes Then
                Dim xNodeList As XmlNodeList = xMySettings.ChildNodes
                Reset()
                For Each m_node As XmlNode In xNodeList
                  Dim xName = m_node.Attributes(0).InnerText
                  Dim xValue = m_node.FirstChild.InnerText
                  If xName.CompareTo("Account") = 0 Then
                    m_Account = xValue
                  ElseIf xName.CompareTo("Interval") = 0 Then
                    If Not Integer.TryParse(xValue, m_Interval) Then m_Interval = 15
                  ElseIf xName.CompareTo("PassCrypt") = 0 Then
                    If m_node.Attributes.Count > 1 Then
                      For I As Integer = 0 To m_node.Attributes.Count - 1
                        If m_node.Attributes(I).Name = "key" Then m_PassKey = m_node.Attributes(I).Value
                        If m_node.Attributes(I).Name = "salt" Then m_PassSalt = m_node.Attributes(I).Value
                      Next
                    End If
                    m_PassCrypt = xValue
                  ElseIf xName.CompareTo("Timeout") = 0 Then
                    If Not Integer.TryParse(xValue, m_Timeout) Then m_Timeout = 120
                  ElseIf xName.CompareTo("TLSProxy") = 0 Then
                    m_TLSProxy = (xValue = "True")
                  ElseIf xName.CompareTo("Proxy") = 0 Then
                    m_ProxySetting = xValue
                  ElseIf xName.CompareTo("Protocol") = 0 Then
                    m_SecurProtocol = SecurityProtocolTypeEx.None
                    If xValue.Contains("SSL") Then m_SecurProtocol = m_SecurProtocol Or SecurityProtocolTypeEx.Ssl3
                    If xValue.Contains("TLS10") Then m_SecurProtocol = m_SecurProtocol Or SecurityProtocolTypeEx.Tls10
                    If xValue.Contains("TLS11") Then m_SecurProtocol = m_SecurProtocol Or SecurityProtocolTypeEx.Tls11
                    If xValue.Contains("TLS12") Then m_SecurProtocol = m_SecurProtocol Or SecurityProtocolTypeEx.Tls12
                    If xValue.Contains("TLS13") Then m_SecurProtocol = m_SecurProtocol Or SecurityProtocolTypeEx.Tls13
                    If xValue.Contains("TLS") And Not xValue.Contains("TLS1") Then m_SecurProtocol = m_SecurProtocol Or SecurityProtocolTypeEx.Tls11 Or SecurityProtocolTypeEx.Tls12 Or SecurityProtocolTypeEx.Tls13
                  ElseIf xName.CompareTo("EnforcedSecurity") = 0 Then
                    m_SecurEnforced = (xValue = "True")
                  End If
                Next
              Else
                Reset()
                Return
              End If
            Else
              Reset()
              Return
            End If
          Else
            Reset()
            Return
          End If
        Else
          Reset()
          Return
        End If
      Else
        Reset()
        Return
      End If
    Else
      Reset()
    End If
  End Sub
  Private Sub Reset()
    m_Account = Nothing
    m_Interval = 15
    m_PassCrypt = Nothing
    m_PassKey = ""
    m_PassSalt = ""
    m_Timeout = 120
    m_TLSProxy = False
    m_ProxySetting = "None"
    m_SecurProtocol = SecurityProtocolTypeEx.Tls11 Or SecurityProtocolTypeEx.Tls12 Or SecurityProtocolTypeEx.Tls13
    m_SecurEnforced = False
  End Sub
  Public ReadOnly Property Account As String
    Get
      Return m_Account
    End Get
  End Property
  Public ReadOnly Property PassCrypt As String
    Get
      Return m_PassCrypt
    End Get
  End Property
  Public ReadOnly Property PassKey As String
    Get
      Return m_PassKey
    End Get
  End Property
  Public ReadOnly Property PassSalt As String
    Get
      Return m_PassSalt
    End Get
  End Property
  Public ReadOnly Property Timeout As Integer
    Get
      Return m_Timeout
    End Get
  End Property
  Public ReadOnly Property TLSProxy As Boolean
    Get
      Return m_TLSProxy
    End Get
  End Property
  Public ReadOnly Property Proxy As Net.IWebProxy
    Get
      If m_ProxySetting.Contains("http://") Then m_ProxySetting = Replace(m_ProxySetting, "http://", String.Empty)
      If m_ProxySetting.Contains(":"c) Then
        Dim myProxySettings() As String = Split(m_ProxySetting, ":")
        Dim pType As String = myProxySettings(0)
        Select Case pType.ToUpperInvariant
          Case "IP"
            Dim pIP As String = myProxySettings(1)
            Dim pPort As Integer = 80
            If myProxySettings.Length > 2 Then pPort = Replace(myProxySettings(2), "/", String.Empty)
            If myProxySettings.Length > 3 Then
              Dim pUser As String = myProxySettings(3)
              Dim pPass As String = myProxySettings(4)
              If myProxySettings.Length > 5 Then
                Dim pDomain As String = myProxySettings(5)
                Return New Net.WebProxy(pIP, pPort) With {.Credentials = New Net.NetworkCredential(pUser, pPass, pDomain)}
              Else
                Return New Net.WebProxy(pIP, pPort) With {.Credentials = New Net.NetworkCredential(pUser, pPass)}
              End If
            Else
              Return New Net.WebProxy(pIP, pPort)
            End If
          Case "URL"
            Dim pURL As String = myProxySettings(1)
            If myProxySettings.Length > 2 Then
              Dim pUser As String = myProxySettings(2)
              Dim pPass As String = myProxySettings(3)
              If myProxySettings.Length > 4 Then
                Dim pDomain As String = myProxySettings(4)
                Return New Net.WebProxy(pURL, False, Nothing, New Net.NetworkCredential(pUser, pPass, pDomain))
              Else
                Return New Net.WebProxy(pURL, False, Nothing, New Net.NetworkCredential(pUser, pPass))
              End If
            Else
              Return New Net.WebProxy(pURL)
            End If
          Case Else
            Return Nothing
        End Select
      Else
        Select Case m_ProxySetting.ToUpperInvariant
          Case "NONE" : Return Nothing
          Case "SYSTEM" : Return Net.WebRequest.DefaultWebProxy
          Case Else : Return Nothing
        End Select
      End If
    End Get
  End Property
  Public ReadOnly Property SecurityProtocol As Net.SecurityProtocolType
    Get
      Return m_SecurProtocol
    End Get
  End Property
  Public ReadOnly Property SecurityEnforced As Boolean
    Get
      Return m_SecurEnforced
    End Get
  End Property
End Class
