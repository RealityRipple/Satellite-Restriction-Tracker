Imports System.Xml
Class AppSettings
  Private m_Config As String
  Private m_Account As String
  Private m_AccountType As localRestrictionTracker.SatHostTypes
  Private m_Interval As Integer
  Private m_LastSyncTime As Date
  Private m_HistoryDir As String
  Private m_PassCrypt As String
  Private m_Timeout As Integer
  Private m_TLSProxy As Boolean
  Private m_ProxySetting As String
  Private m_Protocol As Net.SecurityProtocolType
  Public Loaded As Boolean
  Private Property ConfigFile As String
    Get
      Return m_Config
    End Get
    Set(value As String)
      m_Config = value
    End Set
  End Property
  Public Sub New(Config As String)
    m_Config = Config
    Loaded = False
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
                    If m_node.Attributes.Count > 1 Then
                      m_AccountType = StringToHostType(m_node.Attributes(1).InnerText)
                    Else
                      m_AccountType = localRestrictionTracker.SatHostTypes.Other
                    End If
                  ElseIf xName.CompareTo("Interval") = 0 Then
                    If Not Integer.TryParse(xValue, m_Interval) Then m_Interval = 15
                  ElseIf xName.CompareTo("LastSyncTime") = 0 Then
                    m_LastSyncTime = Date.FromBinary(xValue)
                  ElseIf xName.CompareTo("HistoryDir") = 0 Then
                    m_HistoryDir = xValue
                  ElseIf xName.CompareTo("PassCrypt") = 0 Then
                    m_PassCrypt = xValue
                  ElseIf xName.CompareTo("Timeout") = 0 Then
                    If Not Integer.TryParse(xValue, m_Timeout) Then m_Timeout = 120
                  ElseIf xName.CompareTo("TLSProxy") = 0 Then
                    m_TLSProxy = (xValue = "True")
                  ElseIf xName.CompareTo("Proxy") = 0 Then
                    m_ProxySetting = xValue
                  ElseIf xName.CompareTo("Protocol") = 0 Then
                    m_Protocol = SecurityProtocolTypeEx.None
                    If xValue.Contains("SSL") Then m_Protocol = m_Protocol Or SecurityProtocolTypeEx.Ssl3
                    If xValue.Contains("TLS10") Then m_Protocol = m_Protocol Or SecurityProtocolTypeEx.Tls10
                    If xValue.Contains("TLS11") Then m_Protocol = m_Protocol Or SecurityProtocolTypeEx.Tls11
                    If xValue.Contains("TLS12") Then m_Protocol = m_Protocol Or SecurityProtocolTypeEx.Tls12
                    If xValue.Contains("TLS") And Not xValue.Contains("TLS1") Then m_Protocol = m_Protocol Or SecurityProtocolTypeEx.Tls11 Or SecurityProtocolTypeEx.Tls12
                  End If
                Next
                Loaded = True
              Else
                Reset()
                Exit Sub
              End If
            Else
              Reset()
              Exit Sub
            End If
          Else
            Reset()
            Exit Sub
          End If
        Else
          Reset()
          Exit Sub
        End If
      Else
        Reset()
        Exit Sub
      End If
    Else
      Reset()
    End If
  End Sub
  Private Sub Reset()
    m_Account = Nothing
    m_AccountType = localRestrictionTracker.SatHostTypes.Other
    m_Interval = 15
    m_LastSyncTime = New Date(2000, 1, 1)
    m_HistoryDir = Nothing
    m_PassCrypt = Nothing
    m_Timeout = 120
    m_TLSProxy = False
    m_ProxySetting = "None"
    m_Protocol = SecurityProtocolTypeEx.Tls11 Or SecurityProtocolTypeEx.Tls12
  End Sub
  Public Property Account As String
    Get
      Return m_Account
    End Get
    Set(value As String)
      m_Account = value
    End Set
  End Property
  Public Property AccountType As localRestrictionTracker.SatHostTypes
    Get
      Return m_AccountType
    End Get
    Set(value As localRestrictionTracker.SatHostTypes)
      m_AccountType = value
    End Set
  End Property
  Public Property PassCrypt As String
    Get
      Return m_PassCrypt
    End Get
    Set(value As String)
      m_PassCrypt = value
    End Set
  End Property
  Public Property Interval As Integer
    Get
      Return m_Interval
    End Get
    Set(value As Integer)
      m_Interval = value
    End Set
  End Property
  Public Property LastSyncTime As Date
    Get
      Return m_LastSyncTime
    End Get
    Set(value As Date)
      m_LastSyncTime = value
    End Set
  End Property
  Public Property HistoryDir As String
    Get
      Return m_HistoryDir
    End Get
    Set(value As String)
      m_HistoryDir = value
    End Set
  End Property
  Public Property Timeout As Integer
    Get
      Return m_Timeout
    End Get
    Set(value As Integer)
      m_Timeout = value
    End Set
  End Property
  Public Property TLSProxy As Boolean
    Get
      Return m_TLSProxy
    End Get
    Set(value As Boolean)
      m_TLSProxy = value
    End Set
  End Property
  Public Property Proxy As Net.IWebProxy
    Get
      If m_ProxySetting.Contains("http://") Then m_ProxySetting = Replace(m_ProxySetting, "http://", String.Empty)
      If m_ProxySetting.Contains(":"c) Then
        Dim myProxySettings() As String = Split(m_ProxySetting, ":")
        Dim pType As String = myProxySettings(0)
        Select Case pType.ToLower
          Case "ip"
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
          Case "url"
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
        Select Case m_ProxySetting.ToLower
          Case "none" : Return Nothing
          Case "system" : Return Net.WebRequest.DefaultWebProxy
          Case Else : Return Nothing
        End Select
      End If
    End Get
    Set(value As Net.IWebProxy)
      If value Is Nothing Then
        m_ProxySetting = "None"
      ElseIf value.Equals(Net.WebRequest.DefaultWebProxy) Then
        m_ProxySetting = "System"
      Else
        Dim wValue As Net.WebProxy = value
        If IsNumeric(Replace(wValue.Address.Host, ".", String.Empty)) Then
          If value.Credentials IsNot Nothing Then
            Dim mCreds As Net.NetworkCredential = value.Credentials.GetCredential(Nothing, String.Empty)
            If String.IsNullOrEmpty(mCreds.Domain) Then
              m_ProxySetting = "IP:" & wValue.Address.ToString & ":" & ":" & mCreds.UserName & ":" & mCreds.Password
            Else
              m_ProxySetting = "IP:" & wValue.Address.ToString & ":" & ":" & mCreds.UserName & ":" & mCreds.Password & ":" & mCreds.Domain
            End If
          Else
            m_ProxySetting = "IP:" & wValue.Address.ToString
          End If
        Else
          If value.Credentials IsNot Nothing Then
            Dim mCreds As Net.NetworkCredential = value.Credentials.GetCredential(Nothing, String.Empty)
            If String.IsNullOrEmpty(mCreds.Domain) Then
              m_ProxySetting = "URL:" & wValue.Address.ToString & ":" & ":" & mCreds.UserName & ":" & mCreds.Password
            Else
              m_ProxySetting = "URL:" & wValue.Address.ToString & ":" & ":" & mCreds.UserName & ":" & mCreds.Password & ":" & mCreds.Domain
            End If
          Else
            m_ProxySetting = "URL:" & wValue.Address.ToString
          End If
        End If
      End If
    End Set
  End Property
  Public Property SecurityProtocol As Net.SecurityProtocolType
    Get
      Return m_Protocol
    End Get
    Set(value As Net.SecurityProtocolType)
      m_Protocol = value
    End Set
  End Property
End Class
