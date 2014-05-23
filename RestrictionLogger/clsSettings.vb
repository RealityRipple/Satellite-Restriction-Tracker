Class Settings
  Private m_Account As String
  Private m_AccountType As localRestrictionTracker.SatHostTypes
  Private m_PassCrypt As String
  Private m_Interval As String
  Private m_Timeout As String
  Private m_ProxySetting As String
  Public Sub New(ConfigPath As String)
    Dim xConfig As XElement
    Try
      xConfig = XElement.Load(ConfigPath)
    Catch ex As Exception
      m_Account = String.Empty
      m_AccountType = localRestrictionTracker.SatHostTypes.Other
      m_PassCrypt = String.Empty
      m_Interval = "15"
      m_Timeout = "60"
      m_ProxySetting = "None"
      Exit Sub
    End Try
    Dim xuserSettings As XElement = xConfig.Element("userSettings")
    Dim xwbMySettings As XElement = xuserSettings.Element("RestrictionLogger.My.MySettings")
    Dim xAccount As XElement = Array.Find(xwbMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Account")
    If xAccount Is Nothing Then
      m_Account = String.Empty
      m_AccountType = localRestrictionTracker.SatHostTypes.Other
    Else
      Try
        m_Account = xAccount.Element("value").Value
      Catch ex As Exception
        m_Account = String.Empty
      End Try
      Try
        Dim xAccountType As XAttribute = Array.Find(xAccount.Attributes.ToArray, Function(xSetting As XAttribute) xSetting.Name.ToString = "type")
        If xAccountType Is Nothing Then
          m_AccountType = localRestrictionTracker.SatHostTypes.Other
        Else
          m_AccountType = StringToHostType(xAccountType.Value)
        End If
      Catch ex As Exception
        m_AccountType = localRestrictionTracker.SatHostTypes.Other
      End Try
    End If
    Dim xPassCrypt As XElement = Array.Find(xwbMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "PassCrypt")
    If xPassCrypt Is Nothing Then
      m_PassCrypt = String.Empty
    Else
      Try
        m_PassCrypt = xPassCrypt.Element("value").Value
      Catch ex As Exception
        m_PassCrypt = String.Empty
      End Try
    End If
    Dim xInterval As XElement = Array.Find(xwbMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Interval")
    If xInterval Is Nothing Then
      m_Interval = "15"
    Else
      Try
        m_Interval = xInterval.Element("value").Value
      Catch ex As Exception
        m_Interval = 15
      End Try
    End If
    Dim xTimeout As XElement = Array.Find(xwbMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Timeout")
    If xTimeout Is Nothing Then
      m_Timeout = "60"
    Else
      Try
        m_Timeout = xTimeout.Element("value").Value
      Catch ex As Exception
        m_Timeout = "60"
      End Try
    End If
    Dim xProxy As XElement = Array.Find(xwbMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Proxy")
    If xProxy Is Nothing Then
      m_ProxySetting = "None"
    Else
      Try
        m_ProxySetting = xProxy.Element("value").Value
      Catch ex As Exception
        m_ProxySetting = "None"
      End Try
    End If
  End Sub
  Public ReadOnly Property Account As String
    Get
      Return m_Account
    End Get
  End Property
  Public Property AccountType As localRestrictionTracker.SatHostTypes
    Get
      Return m_AccountType
    End Get
    Set(value As localRestrictionTracker.SatHostTypes)
      m_AccountType = value
    End Set
  End Property
  Public ReadOnly Property PassCrypt As String
    Get
      Return m_PassCrypt
    End Get
  End Property
  Public ReadOnly Property Interval As String
    Get
      Return m_Interval
    End Get
  End Property
  Public Property Timeout As Integer
    Get
      Return m_Timeout
    End Get
    Set(value As Integer)
      m_Timeout = value
    End Set
  End Property
  Public Property Proxy As Net.IWebProxy
    Get
      If m_ProxySetting.Contains(":"c) Then
        Dim myProxySettings() As String = Split(m_ProxySetting, ":")
        Dim pType As String = myProxySettings(0)
        Select Case pType.ToLower
          Case "ip"
            Dim pIP As String = myProxySettings(1)
            Dim pPort As Integer = myProxySettings(2)
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

  Private Function StringToHostType(st As String) As localRestrictionTracker.SatHostTypes
    Select Case st.ToUpper
      Case "WBL" : Return localRestrictionTracker.SatHostTypes.WildBlue_LEGACY
      Case "WBX" : Return localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
      Case "WBV" : Return localRestrictionTracker.SatHostTypes.WildBlue_EVOLUTION
      Case "RPL" : Return localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY
      Case "RPX" : Return localRestrictionTracker.SatHostTypes.RuralPortal_EXEDE
      Case "DNX" : Return localRestrictionTracker.SatHostTypes.DishNet_EXEDE
      Case "WILDBLUE" : Return localRestrictionTracker.SatHostTypes.WildBlue_LEGACY
      Case "EXEDE" : Return localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
      Case "DISHNET" : Return localRestrictionTracker.SatHostTypes.DishNet_EXEDE
      Case "RURALPORTAL" : Return localRestrictionTracker.SatHostTypes.RuralPortal_EXEDE
      Case Else : Return localRestrictionTracker.SatHostTypes.Other
    End Select
  End Function
End Class