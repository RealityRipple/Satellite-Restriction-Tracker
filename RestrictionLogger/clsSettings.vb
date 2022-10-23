Friend Class Settings
  Private m_Account As String
  Private m_PassCrypt As String
  Private m_PassKey As String
  Private m_PassSalt As String
  Private m_Interval As String
  Public Sub New(ConfigPath As String)
    Dim xConfig As XElement
    Try
      xConfig = XElement.Load(ConfigPath)
    Catch ex As Exception
      m_Account = String.Empty
      m_PassCrypt = String.Empty
      m_PassKey = ""
      m_PassSalt = ""
      m_Interval = "15"
      Return
    End Try
    Dim xuserSettings As XElement = xConfig.Element("userSettings")
    Dim xwbMySettings As XElement = xuserSettings.Element("RestrictionLogger.My.MySettings")
    Dim xAccount As XElement = Array.Find(xwbMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Account")
    If xAccount Is Nothing Then
      m_Account = String.Empty
    Else
      Try
        m_Account = xAccount.Element("value").Value
      Catch ex As Exception
        m_Account = String.Empty
      End Try
    End If
    Dim xPassCrypt As XElement = Array.Find(xwbMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "PassCrypt")
    If xPassCrypt Is Nothing Then
      m_PassKey = ""
      m_PassSalt = ""
      m_PassCrypt = String.Empty
    Else
      Try
        If xPassCrypt.Attribute("key") Is Nothing Then
          m_PassKey = ""
        Else
          m_PassKey = xPassCrypt.Attribute("key").Value
        End If
      Catch ex As Exception
        m_PassKey = ""
      End Try
      Try
        If xPassCrypt.Attribute("salt") Is Nothing Then
          m_PassSalt = ""
        Else
          m_PassSalt = xPassCrypt.Attribute("salt").Value
        End If
      Catch ex As Exception
        m_PassSalt = ""
      End Try
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
  Public ReadOnly Property Interval As String
    Get
      Return m_Interval
    End Get
  End Property
End Class
