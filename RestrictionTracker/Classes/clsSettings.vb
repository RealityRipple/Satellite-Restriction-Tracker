﻿Imports System.Xml.Linq
Friend Class SvcSettings
  Private m_Account As String
  Private m_PassCrypt As String
  Private m_PassKey As String
  Private m_PassSalt As String
  Private m_Interval As Integer
  Private m_Timeout As Integer
  Private m_TLSProxy As Boolean
  Private m_ProxySetting As String
  Private m_SecurProtocol As Net.SecurityProtocolType
  Private m_SecurEnforced As Boolean
  Public Function Save() As Boolean
    Dim sProtocol As String = ""
    For Each protocolTest In [Enum].GetValues(GetType(SecurityProtocolTypeEx))
      If (m_SecurProtocol And protocolTest) = protocolTest Then
        sProtocol &= [Enum].GetName(GetType(SecurityProtocolTypeEx), protocolTest).ToUpperInvariant & ", "
      End If
    Next
    If Not String.IsNullOrEmpty(sProtocol) AndAlso sProtocol.EndsWith(", ") Then sProtocol = sProtocol.Substring(0, sProtocol.Length - 2)
    Dim xConfig As New XElement("configuration",
                                New XElement("userSettings",
                                             New XElement("RestrictionLogger.My.MySettings",
                                                          New XElement("setting", New XAttribute("name", "Account"), New XElement("value", m_Account)),
                                                          New XElement("setting", New XAttribute("name", "PassCrypt"), New XAttribute("key", m_PassKey), New XAttribute("salt", m_PassSalt), New XElement("value", m_PassCrypt)),
                                                          New XElement("setting", New XAttribute("name", "Interval"), New XElement("value", m_Interval)),
                                                          New XElement("setting", New XAttribute("name", "Timeout"), New XElement("value", m_Timeout)),
                                                          New XElement("setting", New XAttribute("name", "TLSProxy"), New XElement("value", IIf(m_TLSProxy, "True", "False"))),
                                                          New XElement("setting", New XAttribute("name", "Proxy"), New XElement("value", m_ProxySetting)),
                                                          New XElement("setting", New XAttribute("name", "Protocol"), New XElement("value", sProtocol)),
                                                          New XElement("setting", New XAttribute("name", "EnforcedSecurity"), New XElement("value", IIf(m_SecurEnforced, "True", "False"))))))
    If srlFunctions.InUseChecker(IO.Path.Combine(CommonAppDataDirectory, "user.config"), IO.FileAccess.Write) Then
      Try
        xConfig.Save(IO.Path.Combine(CommonAppDataDirectory, "user.config"))
        Return True
      Catch ex As Exception
        MsgDlg(Nothing, "There was an error saving the Satellite Restriction Logger service settings file.", "The Service settings were not saved.", "Logger Service Error", MessageBoxButtons.OK, _TaskDialogIcon.Batch, MessageBoxIcon.Warning, , ex.Message, _TaskDialogExpandedDetailsLocation.ExpandFooter, "View Error Details", "Hide Error Details")
      End Try
    Else
      MsgDlg(Nothing, My.Application.Info.ProductName & " was unable to write to the Satellite Restriction Logger service settings file.", "The Service settings were not saved.", "Logger Service Error", MessageBoxButtons.OK, _TaskDialogIcon.Batch, MessageBoxIcon.Warning, , "The program could not get write permissions." & vbNewLine & "The file """ & IO.Path.Combine(CommonAppDataDirectory, "user.config") & """ may be in use.", _TaskDialogExpandedDetailsLocation.ExpandFooter, "View Error Details", "Hide Error Details")
    End If
    Return False
  End Function
  Public Sub New()
    m_Account = Nothing
    m_PassCrypt = Nothing
    m_PassKey = ""
    m_PassSalt = ""
    m_Interval = 15
    m_Timeout = 120
    m_TLSProxy = False
    m_ProxySetting = "None"
    m_SecurProtocol = SecurityProtocolTypeEx.Tls11 Or SecurityProtocolTypeEx.Tls12 Or SecurityProtocolTypeEx.Tls13
    m_SecurEnforced = False
  End Sub
  Public WriteOnly Property Account As String
    Set(value As String)
      m_Account = value
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
  Public Property PassKey As String
    Get
      Return m_PassKey
    End Get
    Set(value As String)
      m_PassKey = value
    End Set
  End Property
  Public Property PassSalt As String
    Get
      Return m_PassSalt
    End Get
    Set(value As String)
      m_PassSalt = value
    End Set
  End Property
  Public WriteOnly Property Interval As Integer
    Set(value As Integer)
      m_Interval = value
    End Set
  End Property
  Public WriteOnly Property Timeout As Integer
    Set(value As Integer)
      m_Timeout = value
    End Set
  End Property
  Public Property TLSProxy
    Get
      Return m_TLSProxy
    End Get
    Set(value)
      m_TLSProxy = value
    End Set
  End Property
  Public WriteOnly Property Proxy As Net.IWebProxy
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
      Return m_SecurProtocol
    End Get
    Set(value As Net.SecurityProtocolType)
      m_SecurProtocol = value
    End Set
  End Property
  Public Property SecurityEnforced As Boolean
    Get
      Return m_SecurEnforced
    End Get
    Set(value As Boolean)
      m_SecurEnforced = value
    End Set
  End Property
End Class
Friend Class AppSettings
  Private m_Account As String
  Private m_StartWait As Integer
  Private m_Interval As Integer
  Private m_Gr As String
  Private m_LastUpdate As Date
  Private m_LastSyncTime As Date
  Private m_Accuracy As Integer
  Private m_Ago As UInteger
  Private m_Service As Boolean
  Private m_HistoryDir As String
  Private m_UpdateBETA As Boolean
  Private m_UpdateType As UpdateTypes
  Private m_UpdateTime As Byte
  Private m_ScaleScreen As Boolean
  Private m_MainSize As Size
  Private m_PassCrypt As String
  Private m_PassKey As String
  Private m_PassSalt As String
  Private m_Share As Boolean
  Private m_TopMost As Boolean
  Private m_Timeout As Integer
  Private m_Retries As Integer
  Private m_Overuse As Integer
  Private m_Overtime As Integer
  Private m_AlertStyle As String
  Private m_TrayIcon As TrayStyles
  Private m_TrayAnim As Boolean
  Private m_TrayClose As Boolean
  Private m_AutoHide As Boolean
  Private m_TLSProxy As Boolean
  Private m_ProxySetting As String
  Private m_NetTest As String
  Private m_SecurProtocol As Net.SecurityProtocolType
  Private m_SecurEnforced As Boolean
  Public Loaded As Boolean
  Public Colors As AppColors
  Public Enum UpdateTypes
    Auto = 1
    Ask
    None
  End Enum
  Public Enum TrayStyles
    Always
    Minimized
    Never
  End Enum
  Private Shared ReadOnly Property ConfigFile As String
    Get
      Return IO.Path.Combine(LocalAppDataDirectory, "user.config")
    End Get
  End Property
  Private Shared ReadOnly Property ConfigFileBackup As String
    Get
      Return IO.Path.Combine(LocalAppDataDirectory, "backup.config")
    End Get
  End Property
  Public Sub New()
    Loaded = False
    BackupCheckup()
    If My.Computer.FileSystem.FileExists(ConfigFile) Then
      Dim xConfig As XElement
      Try
        xConfig = XElement.Load(ConfigFile)
      Catch ex As Exception
        Reset()
        Loaded = True
        Return
      End Try
      Dim xuserSettings As XElement = xConfig.Element("userSettings")
      If xuserSettings Is Nothing Then
        Reset()
        Loaded = True
        Return
      End If
      Dim xMySettings As XElement
      If xuserSettings.Element("WildBlueUsage.My.MySettings") IsNot Nothing Then
        xMySettings = xuserSettings.Element("WildBlueUsage.My.MySettings")
      ElseIf xuserSettings.Element("RestrictionTracker.My.MySettings") IsNot Nothing Then
        xMySettings = xuserSettings.Element("RestrictionTracker.My.MySettings")
      Else
        Reset()
        Loaded = True
        Return
      End If
      Dim xAccount As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Account")
      If xAccount Is Nothing Then
        m_Account = String.Empty
      Else
        Try
          m_Account = xAccount.Element("value").Value
        Catch ex As Exception
          m_Account = String.Empty
        End Try
      End If
      Dim xStartWait As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "StartWait")
      If xStartWait Is Nothing Then
        m_StartWait = 5
      Else
        Try
          m_StartWait = xStartWait.Element("value").Value
        Catch ex As Exception
          m_StartWait = 5
        End Try
      End If
      Dim xInterval As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Interval")
      If xInterval Is Nothing Then
        m_Interval = 15
      Else
        Try
          m_Interval = xInterval.Element("value").Value
        Catch ex As Exception
          m_Interval = 15
        End Try
      End If
      Dim xGr As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Gr")
      If xGr Is Nothing Then
        m_Gr = "aph"
      Else
        Try
          m_Gr = xGr.Element("value").Value
        Catch ex As Exception
          m_Gr = "aph"
        End Try
      End If
      Dim xLastUpdate As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "LastUpdate")
      If xLastUpdate Is Nothing Then
        m_LastUpdate = New Date(2000, 1, 1)
      Else
        Try
          m_LastUpdate = Date.FromBinary(xLastUpdate.Element("value").Value)
        Catch ex As Exception
          m_LastUpdate = New Date(2000, 1, 1)
        End Try
      End If
      Dim xLastSyncTime As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "LastSyncTime")
      If xLastSyncTime Is Nothing Then
        m_LastSyncTime = New Date(2000, 1, 1)
      Else
        Try
          m_LastSyncTime = Date.FromBinary(xLastSyncTime.Element("value").Value)
        Catch ex As Exception
          m_LastSyncTime = New Date(2000, 1, 1)
        End Try
      End If
      Dim xAccuracy As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Accuracy")
      If xAccuracy Is Nothing Then
        m_Accuracy = 0
      Else
        Try
          m_Accuracy = xAccuracy.Element("value").Value
        Catch ex As Exception
          m_Accuracy = 0
        End Try
      End If
      Dim xAgo As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Ago")
      If xAgo Is Nothing Then
        m_Ago = 30
      Else
        Try
          m_Ago = xAgo.Element("value").Value
        Catch ex As Exception
          m_Ago = 30
        End Try
      End If
      Dim xService As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Service")
      If xService Is Nothing Then
        m_Service = False
      Else
        Try
          m_Service = xService.Element("value").Value = "True"
        Catch ex As Exception
          m_Service = False
        End Try
      End If
      Dim xHistoryDir As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "HistoryDir")
      If xHistoryDir Is Nothing Then
        m_HistoryDir = Nothing
      Else
        Try
          m_HistoryDir = xHistoryDir.Element("value").Value
        Catch ex As Exception
          m_HistoryDir = Nothing
        End Try
      End If
      Dim xUpdateBETA As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "UpdateBETA")
      If xUpdateBETA Is Nothing Then
        Dim xBetaCheck As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "BetaCheck")
        If xBetaCheck Is Nothing Then
          m_UpdateBETA = False
        Else
          Try
            m_UpdateBETA = xBetaCheck.Element("value").Value = "True"
          Catch ex As Exception
            m_UpdateBETA = False
          End Try
        End If
      Else
        Try
          m_UpdateBETA = xUpdateBETA.Element("value").Value = "True"
        Catch ex As Exception
          m_UpdateBETA = False
        End Try
      End If
      Dim xUpdateType As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "UpdateType")
      If xUpdateType Is Nothing Then
        m_UpdateType = UpdateTypes.Ask
      Else
        Try
          Dim sUpdateType As String = xUpdateType.Element("value").Value
          If sUpdateType = "BETA" Then
            m_UpdateBETA = True
            m_UpdateType = UpdateTypes.Ask
          ElseIf sUpdateType = "Auto" Then
            m_UpdateType = UpdateTypes.Auto
          ElseIf sUpdateType = "None" Then
            m_UpdateType = UpdateTypes.None
          Else
            m_UpdateType = UpdateTypes.Ask
          End If
        Catch ex As Exception
          m_UpdateType = UpdateTypes.Ask
        End Try
      End If
      Dim xUpdateTime As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "UpdateTime")
      If xUpdateTime Is Nothing Then
        m_UpdateTime = 15
      Else
        Try
          m_UpdateTime = xUpdateTime.Element("value").Value
        Catch ex As Exception
          m_UpdateTime = 15
        End Try
      End If
      Dim xScaleScreen As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "ScaleScreen")
      If xScaleScreen Is Nothing Then
        m_ScaleScreen = False
      Else
        Try
          m_ScaleScreen = xScaleScreen.Element("value").Value = "True"
        Catch ex As Exception
          m_ScaleScreen = False
        End Try
      End If
      Dim xMainSize As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "MainSize")
      If xMainSize Is Nothing Then
        m_MainSize = New Size(450, 200)
      Else
        Try
          Dim sSizes() As String = Split(xMainSize.Element("value").Value, ",", 2)
          m_MainSize = New Size(sSizes(0), sSizes(1))
        Catch ex As Exception
          m_MainSize = New Size(450, 200)
        End Try
      End If
      Dim xPassCrypt As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "PassCrypt")
      If xPassCrypt Is Nothing Then
        m_PassKey = ""
        m_PassSalt = ""
        m_PassCrypt = Nothing
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
          m_PassCrypt = Nothing
        End Try
      End If
      Dim xShare As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Share")
      If xShare Is Nothing Then
        m_Share = False
      Else
        Try
          m_Share = xShare.Element("value").Value = "True"
        Catch ex As Exception
          m_Share = False
        End Try
      End If
      Dim xTopMost As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "TopMost")
      If xTopMost Is Nothing Then
        m_TopMost = False
      Else
        Try
          m_TopMost = xTopMost.Element("value").Value = "True"
        Catch ex As Exception
          m_TopMost = False
        End Try
      End If
      Dim xTimeout As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Timeout")
      If xTimeout Is Nothing Then
        m_Timeout = 120
      Else
        Try
          m_Timeout = xTimeout.Element("value").Value
        Catch ex As Exception
          m_Timeout = 120
        End Try
      End If
      Dim xRetries As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Retries")
      If xRetries Is Nothing Then
        m_Retries = 2
      Else
        Try
          m_Retries = xRetries.Element("value").Value
        Catch ex As Exception
          m_Retries = 2
        End Try
      End If
      Dim xOveruse As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Overuse")
      If xOveruse Is Nothing Then
        m_Overuse = 0
      Else
        Try
          m_Overuse = xOveruse.Element("value").Value
        Catch ex As Exception
          m_Overuse = 0
        End Try
      End If
      Dim xOvertime As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Overtime")
      If xOvertime Is Nothing Then
        m_Overtime = 60
      Else
        Try
          m_Overtime = xOvertime.Element("value").Value
        Catch ex As Exception
          m_Overtime = 60
        End Try
      End If
      Dim xAlertStyle As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "AlertStyle")
      If xAlertStyle Is Nothing Then
        m_AlertStyle = "Default"
      Else
        Try
          m_AlertStyle = xAlertStyle.Element("value").Value
        Catch ex As Exception
          m_AlertStyle = "Default"
        End Try
      End If
      Dim xTrayIcon As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "TrayIcon")
      If xTrayIcon Is Nothing Then
        m_TrayIcon = TrayStyles.Always
      Else
        Try
          Dim xTrayStyle As XElement = Array.Find(xTrayIcon.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Style")
          If xTrayStyle Is Nothing Then
            Try
              Select Case xTrayIcon.Element("value").Value
                Case "Never" : m_TrayIcon = TrayStyles.Never
                Case "Minimized" : m_TrayIcon = TrayStyles.Minimized
                Case Else : m_TrayIcon = TrayStyles.Always
              End Select
            Catch ex As Exception
              m_TrayIcon = TrayStyles.Always
            End Try
          Else
            Select Case xTrayStyle.Element("value").Value
              Case "Never" : m_TrayIcon = TrayStyles.Never
              Case "Minimized" : m_TrayIcon = TrayStyles.Minimized
              Case Else : m_TrayIcon = TrayStyles.Always
            End Select
          End If
          Dim xTrayAnim As XElement = Array.Find(xTrayIcon.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Animation")
          If xTrayAnim Is Nothing Then
            m_TrayAnim = True
          Else
            m_TrayAnim = xTrayAnim.Element("value").Value = "True"
          End If
          Dim xTrayClose As XElement = Array.Find(xTrayIcon.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "OnClose")
          If xTrayClose Is Nothing Then
            m_TrayClose = False
          Else
            m_TrayClose = xTrayClose.Element("value").Value = "True"
          End If
        Catch ex As Exception
          Try
            Select Case xTrayIcon.Element("value").Value
              Case "Never" : m_TrayIcon = TrayStyles.Never
              Case "Minimized" : m_TrayIcon = TrayStyles.Minimized
              Case Else : m_TrayIcon = TrayStyles.Always
            End Select
          Catch ex2 As Exception
            m_TrayIcon = TrayStyles.Always
          End Try
          m_TrayAnim = True
          m_TrayClose = False
        End Try
      End If
      Dim xAutoHide As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "AutoHide")
      If xAutoHide Is Nothing Then
        m_AutoHide = True
      Else
        Try
          m_AutoHide = xAutoHide.Element("value").Value = "True"
        Catch ex As Exception
          m_AutoHide = True
        End Try
      End If
      Dim xTLSProxy As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "TLSProxy")
      If xTLSProxy Is Nothing Then
        m_TLSProxy = False
      Else
        Try
          m_TLSProxy = xTLSProxy.Element("value").Value = "True"
        Catch ex As Exception
          m_TLSProxy = False
        End Try
      End If
      Dim xProxy As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Proxy")
      If xProxy Is Nothing Then
        m_ProxySetting = "None"
      Else
        Try
          m_ProxySetting = xProxy.Element("value").Value
        Catch ex As Exception
          m_ProxySetting = "None"
        End Try
      End If
      Dim xProtocol As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Protocol")
      If xProtocol Is Nothing Then
        m_SecurProtocol = SecurityProtocolTypeEx.Tls11 Or SecurityProtocolTypeEx.Tls12 Or SecurityProtocolTypeEx.Tls13
      Else
        Try
          m_SecurProtocol = SecurityProtocolTypeEx.None
          If xProtocol.Element("value").Value.Contains("SSL") Then m_SecurProtocol = m_SecurProtocol Or SecurityProtocolTypeEx.Ssl3
          If xProtocol.Element("value").Value.Contains("TLS10") Then m_SecurProtocol = m_SecurProtocol Or SecurityProtocolTypeEx.Tls10
          If xProtocol.Element("value").Value.Contains("TLS11") Then m_SecurProtocol = m_SecurProtocol Or SecurityProtocolTypeEx.Tls11
          If xProtocol.Element("value").Value.Contains("TLS12") Then m_SecurProtocol = m_SecurProtocol Or SecurityProtocolTypeEx.Tls12
          If xProtocol.Element("value").Value.Contains("TLS13") Then m_SecurProtocol = m_SecurProtocol Or SecurityProtocolTypeEx.Tls13
          If xProtocol.Element("value").Value.Contains("TLS") And Not xProtocol.Element("value").Value.Contains("TLS1") Then m_SecurProtocol = m_SecurProtocol Or SecurityProtocolTypeEx.Tls11 Or SecurityProtocolTypeEx.Tls12 Or SecurityProtocolTypeEx.Tls13
        Catch ex As Exception
          m_SecurProtocol = SecurityProtocolTypeEx.Tls11 Or SecurityProtocolTypeEx.Tls12
        End Try
      End If
      Dim xEnforce As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "EnforcedSecurity")
      If xEnforce Is Nothing Then
        m_SecurEnforced = False
      Else
        Try
          m_SecurEnforced = xEnforce.Element("value").Value = "True"
        Catch ex As Exception
          m_SecurEnforced = False
        End Try
      End If
      Dim xNetTest As XElement = Array.Find(xMySettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "NetTestURL")
      If xNetTest Is Nothing Then
        m_NetTest = Nothing
      Else
        Try
          m_NetTest = xNetTest.Element("value").Value
        Catch ex As Exception
          m_NetTest = Nothing
        End Try
      End If
      Colors = New AppColors
      Dim xcolorSettings As XElement = xConfig.Element("colorSettings")
      If xcolorSettings Is Nothing Then
        ResetColors()
      Else
        Dim xMain As XElement = Array.Find(xcolorSettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Main")
        If xMain Is Nothing Then
          ResetMain()
        Else
          Dim xMainDown As XElement = Array.Find(xMain.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Download")
          If xMainDown Is Nothing Then
            Colors.MainDownA = Color.Transparent
            Colors.MainDownB = Color.Transparent
            Colors.MainDownC = Color.Transparent
          Else
            Dim xMainDA As XElement = Array.Find(xMainDown.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Start")
            If xMainDA Is Nothing Then
              Colors.MainDownA = Color.Transparent
            Else
              Try
                Colors.MainDownA = StrToColor(xMainDA.Element("value").Value)
              Catch ex As Exception
                Colors.MainDownA = Color.Transparent
              End Try
            End If
            Dim xMainDB As XElement = Array.Find(xMainDown.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Mid")
            If xMainDB Is Nothing Then
              Colors.MainDownB = Color.Transparent
            Else
              Try
                If xMainDB.Element("value").Value = "No" Then
                  Colors.MainDownB = Color.Transparent
                Else
                  Colors.MainDownB = StrToColor(xMainDB.Element("value").Value)
                End If
              Catch ex As Exception
                Colors.MainDownB = Color.Transparent
              End Try
            End If
            Dim xMainDC As XElement = Array.Find(xMainDown.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "End")
            If xMainDC Is Nothing Then
              Colors.MainDownC = Color.Transparent
            Else
              Try
                Colors.MainDownC = StrToColor(xMainDC.Element("value").Value)
              Catch ex As Exception
                Colors.MainDownC = Color.Transparent
              End Try
            End If
          End If
          Dim xMainText As XElement = Array.Find(xMain.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Text")
          If xMainText Is Nothing Then
            Colors.MainText = Color.Transparent
          Else
            Try
              Colors.MainText = StrToColor(xMainText.Element("value").Value)
            Catch ex As Exception
              Colors.MainText = Color.Transparent
            End Try
          End If
          Dim xMainBG As XElement = Array.Find(xMain.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Background")
          If xMainBG Is Nothing Then
            Colors.MainBackground = Color.Transparent
          Else
            Try
              Colors.MainBackground = StrToColor(xMainBG.Element("value").Value)
            Catch ex As Exception
              Colors.MainBackground = Color.Transparent
            End Try
          End If
        End If
        Dim xTray As XElement = Array.Find(xcolorSettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Tray")
        If xTray Is Nothing Then
          ResetTray()
        Else
          Dim xTrayDown As XElement = Array.Find(xTray.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Download")
          If xTrayDown Is Nothing Then
            Colors.TrayDownA = Color.Transparent
            Colors.TrayDownB = Color.Transparent
            Colors.TrayDownC = Color.Transparent
          Else
            Dim xTrayDA As XElement = Array.Find(xTrayDown.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Start")
            If xTrayDA Is Nothing Then
              Colors.TrayDownA = Color.Transparent
            Else
              Try
                Colors.TrayDownA = StrToColor(xTrayDA.Element("value").Value)
              Catch ex As Exception
                Colors.TrayDownA = Color.Transparent
              End Try
            End If
            Dim xTrayDB As XElement = Array.Find(xTrayDown.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Mid")
            If xTrayDB Is Nothing Then
              Colors.TrayDownB = Color.Transparent
            Else
              Try
                If xTrayDB.Element("value").Value = "No" Then
                  Colors.TrayDownB = Color.Transparent
                Else
                  Colors.TrayDownB = StrToColor(xTrayDB.Element("value").Value)
                End If
              Catch ex As Exception
                Colors.TrayDownB = Color.Transparent
              End Try
            End If
            Dim xTrayDC As XElement = Array.Find(xTrayDown.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "End")
            If xTrayDC Is Nothing Then
              Colors.TrayDownC = Color.Transparent
            Else
              Try
                Colors.TrayDownC = StrToColor(xTrayDC.Element("value").Value)
              Catch ex As Exception
                Colors.TrayDownC = Color.Transparent
              End Try
            End If
          End If
        End If
        Dim xHistory As XElement = Array.Find(xcolorSettings.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "History")
        If xHistory Is Nothing Then
          ResetHistory()
        Else
          Dim xHistoryDown As XElement = Array.Find(xHistory.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Download")
          If xHistoryDown Is Nothing Then
            Colors.HistoryDownA = Color.Transparent
            Colors.HistoryDownB = Color.Transparent
            Colors.HistoryDownC = Color.Transparent
            Colors.HistoryDownMax = Color.Transparent
            Colors.HistoryDownLine = Color.Transparent
          Else
            Dim xHistoryDA As XElement = Array.Find(xHistoryDown.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Start")
            If xHistoryDA Is Nothing Then
              Colors.HistoryDownA = Color.Transparent
            Else
              Try
                Colors.HistoryDownA = StrToColor(xHistoryDA.Element("value").Value)
              Catch ex As Exception
                Colors.HistoryDownA = Color.Transparent
              End Try
            End If
            Dim xHistoryDB As XElement = Array.Find(xHistoryDown.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Mid")
            If xHistoryDB Is Nothing Then
              Colors.HistoryDownB = Color.Transparent
            Else
              Try
                If xHistoryDB.Element("value").Value = "No" Then
                  Colors.HistoryDownB = Color.Transparent
                Else
                  Colors.HistoryDownB = StrToColor(xHistoryDB.Element("value").Value)
                End If
              Catch ex As Exception
                Colors.HistoryDownB = Color.Transparent
              End Try
            End If
            Dim xHistoryDC As XElement = Array.Find(xHistoryDown.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "End")
            If xHistoryDC Is Nothing Then
              Colors.HistoryDownC = Color.Transparent
            Else
              Try
                Colors.HistoryDownC = StrToColor(xHistoryDC.Element("value").Value)
              Catch ex As Exception
                Colors.HistoryDownC = Color.Transparent
              End Try
            End If
            Dim xHistoryDM As XElement = Array.Find(xHistoryDown.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Maximum")
            If xHistoryDM Is Nothing Then
              Colors.HistoryDownMax = Color.Transparent
            Else
              Try
                Colors.HistoryDownMax = StrToColor(xHistoryDM.Element("value").Value)
              Catch ex As Exception
                Colors.HistoryDownMax = Color.Transparent
              End Try
            End If
            Dim xHistoryDLine As XElement = Array.Find(xHistoryDown.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Line")
            If xHistoryDLine Is Nothing Then
              Colors.HistoryDownLine = Color.Transparent
            Else
              Try
                Colors.HistoryDownLine = StrToColor(xHistoryDLine.Element("value").Value)
              Catch ex As Exception
                Colors.HistoryDownLine = Color.Transparent
              End Try
            End If
          End If
          Dim xHistoryText As XElement = Array.Find(xHistory.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Text")
          If xHistoryText Is Nothing Then
            Colors.HistoryText = Color.Transparent
          Else
            Try
              Colors.HistoryText = StrToColor(xHistoryText.Element("value").Value)
            Catch ex As Exception
              Colors.HistoryText = Color.Transparent
            End Try
          End If
          Dim xHistoryBG As XElement = Array.Find(xHistory.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Background")
          If xHistoryBG Is Nothing Then
            Colors.HistoryBackground = Color.Transparent
          Else
            Try
              Colors.HistoryBackground = StrToColor(xHistoryBG.Element("value").Value)
            Catch ex As Exception
              Colors.HistoryBackground = Color.Transparent
            End Try
          End If
          Dim xHistoryGrid As XElement = Array.Find(xHistory.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Grid")
          If xHistoryGrid Is Nothing Then
            Colors.HistoryLightGrid = Color.Transparent
            Colors.HistoryDarkGrid = Color.Transparent
          Else
            Dim xHistoryLight As XElement = Array.Find(xHistoryGrid.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Light")
            If xHistoryLight Is Nothing Then
              Colors.HistoryLightGrid = Color.Transparent
            Else
              Try
                Colors.HistoryLightGrid = StrToColor(xHistoryLight.Element("value").Value)
              Catch ex As Exception
                Colors.HistoryLightGrid = Color.Transparent
              End Try
            End If
            Dim xHistoryUDark As XElement = Array.Find(xHistoryGrid.Elements.ToArray, Function(xSetting As XElement) xSetting.Attribute("name").Value = "Dark")
            If xHistoryUDark Is Nothing Then
              Colors.HistoryDarkGrid = Color.Transparent
            Else
              Try
                Colors.HistoryDarkGrid = StrToColor(xHistoryUDark.Element("value").Value)
              Catch ex As Exception
                Colors.HistoryDarkGrid = Color.Transparent
              End Try
            End If
          End If
        End If
      End If
      Loaded = True
    Else
      Reset()
    End If
  End Sub
  Private Sub Reset()
    m_Account = Nothing
    m_StartWait = 5
    m_Interval = 15
    m_Gr = "aph"
    m_LastUpdate = New Date(2000, 1, 1)
    m_LastSyncTime = New Date(2000, 1, 1)
    m_Accuracy = 0
    m_Ago = 30
    m_Service = False
    m_HistoryDir = Nothing
    m_UpdateBETA = False
    m_UpdateType = UpdateTypes.Ask
    m_UpdateTime = 15
    m_ScaleScreen = False
    m_MainSize = New Size(450, 200)
    m_PassCrypt = Nothing
    m_PassKey = ""
    m_PassSalt = ""
    m_Share = False
    m_TopMost = False
    m_Timeout = 120
    m_Retries = 2
    m_Overuse = 0
    m_Overtime = 60
    m_AlertStyle = "Default"
    m_TrayIcon = TrayStyles.Always
    m_TrayAnim = True
    m_TrayClose = False
    m_AutoHide = True
    m_TLSProxy = False
    m_ProxySetting = "None"
    m_SecurProtocol = SecurityProtocolTypeEx.Tls11 Or SecurityProtocolTypeEx.Tls12 Or SecurityProtocolTypeEx.Tls13
    m_SecurEnforced = False
    m_NetTest = Nothing
    Colors = New AppColors
    ResetColors()
  End Sub
  Private Sub ResetColors()
    ResetMain()
    ResetTray()
    ResetHistory()
  End Sub
  Private Sub ResetMain()
    Colors.MainDownA = Color.Transparent
    Colors.MainDownB = Color.Transparent
    Colors.MainDownC = Color.Transparent
    Colors.MainText = Color.Transparent
    Colors.MainBackground = Color.Transparent
  End Sub
  Private Sub ResetTray()
    Colors.TrayDownA = Color.Transparent
    Colors.TrayDownB = Color.Transparent
    Colors.TrayDownC = Color.Transparent
  End Sub
  Private Sub ResetHistory()
    Colors.HistoryDownA = Color.Transparent
    Colors.HistoryDownB = Color.Transparent
    Colors.HistoryDownC = Color.Transparent
    Colors.HistoryDownMax = Color.Transparent
    Colors.HistoryDownLine = Color.Transparent
    Colors.HistoryText = Color.Transparent
    Colors.HistoryBackground = Color.Transparent
    Colors.HistoryLightGrid = Color.Transparent
    Colors.HistoryDarkGrid = Color.Transparent
  End Sub
  Public Function Save() As Boolean
    Dim sUpdateType As String = "Ask"
    Select Case m_UpdateType
      Case UpdateTypes.Auto : sUpdateType = "Auto"
      Case UpdateTypes.None : sUpdateType = "None"
    End Select
    Dim sTrayIcon As String = "Always"
    Select Case m_TrayIcon
      Case TrayStyles.Never : sTrayIcon = "Never"
      Case TrayStyles.Minimized : sTrayIcon = "Minimized"
    End Select
    Dim sProtocol As String = ""
    For Each protocolTest In [Enum].GetValues(GetType(SecurityProtocolTypeEx))
      If (m_SecurProtocol And protocolTest) = protocolTest Then
        sProtocol &= [Enum].GetName(GetType(SecurityProtocolTypeEx), protocolTest).ToUpperInvariant & ", "
      End If
    Next
    If Not String.IsNullOrEmpty(sProtocol) AndAlso sProtocol.EndsWith(", ") Then sProtocol = sProtocol.Substring(0, sProtocol.Length - 2)
    Dim xConfig As New XElement("configuration",
                                New XElement("userSettings",
                                             New XElement("RestrictionTracker.My.MySettings",
                                                          New XElement("setting", New XAttribute("name", "Account"), New XElement("value", m_Account)),
                                                          New XElement("setting", New XAttribute("name", "PassCrypt"), New XAttribute("key", m_PassKey), New XAttribute("salt", m_PassSalt), New XElement("value", m_PassCrypt)),
                                                          New XElement("setting", New XAttribute("name", "Share"), New XElement("value", IIf(m_Share, "True", "False"))),
                                                          New XElement("setting", New XAttribute("name", "StartWait"), New XElement("value", m_StartWait)),
                                                          New XElement("setting", New XAttribute("name", "Interval"), New XElement("value", m_Interval)),
                                                          New XElement("setting", New XAttribute("name", "Gr"), New XElement("value", m_Gr)),
                                                          New XElement("setting", New XAttribute("name", "LastUpdate"), New XElement("value", m_LastUpdate.ToBinary)),
                                                          New XElement("setting", New XAttribute("name", "LastSyncTime"), New XElement("value", m_LastSyncTime.ToBinary)),
                                                          New XElement("setting", New XAttribute("name", "Accuracy"), New XElement("value", m_Accuracy)),
                                                          New XElement("setting", New XAttribute("name", "Ago"), New XElement("value", m_Ago)),
                                                          New XElement("setting", New XAttribute("name", "Service"), New XElement("value", IIf(m_Service, "True", "False"))),
                                                          New XElement("setting", New XAttribute("name", "HistoryDir"), New XElement("value", m_HistoryDir)),
                                                          New XElement("setting", New XAttribute("name", "UpdateBETA"), New XElement("value", IIf(m_UpdateBETA, "True", "False"))),
                                                          New XElement("setting", New XAttribute("name", "UpdateType"), New XElement("value", sUpdateType)),
                                                          New XElement("setting", New XAttribute("name", "UpdateTime"), New XElement("value", m_UpdateTime)),
                                                          New XElement("setting", New XAttribute("name", "ScaleScreen"), New XElement("value", IIf(m_ScaleScreen, "True", "False"))),
                                                          New XElement("setting", New XAttribute("name", "MainSize"), New XElement("value", m_MainSize.Width & "," & m_MainSize.Height)),
                                                          New XElement("setting", New XAttribute("name", "TopMost"), New XElement("value", IIf(m_TopMost, "True", "False"))),
                                                          New XElement("setting", New XAttribute("name", "Timeout"), New XElement("value", m_Timeout)),
                                                          New XElement("setting", New XAttribute("name", "Retries"), New XElement("value", m_Retries)),
                                                          New XElement("setting", New XAttribute("name", "Overuse"), New XElement("value", m_Overuse)),
                                                          New XElement("setting", New XAttribute("name", "Overtime"), New XElement("value", m_Overtime)),
                                                          New XElement("setting", New XAttribute("name", "AlertStyle"), New XElement("value", m_AlertStyle)),
                                                          New XElement("section", New XAttribute("name", "TrayIcon"),
                                                                       New XElement("setting", New XAttribute("name", "Style"), New XElement("value", sTrayIcon)),
                                                                       New XElement("setting", New XAttribute("name", "Animation"), New XElement("value", IIf(m_TrayAnim, "True", "False"))),
                                                                       New XElement("setting", New XAttribute("name", "OnClose"), New XElement("value", IIf(m_TrayClose, "True", "False")))),
                                                          New XElement("setting", New XAttribute("name", "AutoHide"), New XElement("value", IIf(m_AutoHide, "True", "False"))),
                                                          New XElement("setting", New XAttribute("name", "TLSProxy"), New XElement("value", IIf(m_TLSProxy, "True", "False"))),
                                                          New XElement("setting", New XAttribute("name", "Proxy"), New XElement("value", m_ProxySetting)),
                                                          New XElement("setting", New XAttribute("name", "Protocol"), New XElement("value", sProtocol)),
                                                          New XElement("setting", New XAttribute("name", "EnforcedSecurity"), New XElement("value", IIf(m_SecurEnforced, "True", "False"))),
                                                          New XElement("setting", New XAttribute("name", "NetTestURL"), New XElement("value", m_NetTest)))),
                                New XElement("colorSettings",
                                             New XElement("graph", New XAttribute("name", "Main"),
                                                          New XElement("section", New XAttribute("name", "Download"),
                                                                       New XElement("setting", New XAttribute("name", "Start"), New XElement("value", ColorToStr(Colors.MainDownA))),
                                                                       New XElement("setting", New XAttribute("name", "Mid"), New XElement("value", ColorToStr(Colors.MainDownB))),
                                                                       New XElement("setting", New XAttribute("name", "End"), New XElement("value", ColorToStr(Colors.MainDownC)))),
                                                          New XElement("setting", New XAttribute("name", "Text"), New XElement("value", ColorToStr(Colors.MainText))),
                                                          New XElement("setting", New XAttribute("name", "Background"), New XElement("value", ColorToStr(Colors.MainBackground)))),
                                             New XElement("graph", New XAttribute("name", "Tray"),
                                                          New XElement("section", New XAttribute("name", "Download"),
                                                                       New XElement("setting", New XAttribute("name", "Start"), New XElement("value", ColorToStr(Colors.TrayDownA))),
                                                                       New XElement("setting", New XAttribute("name", "Mid"), New XElement("value", ColorToStr(Colors.TrayDownB))),
                                                                       New XElement("setting", New XAttribute("name", "End"), New XElement("value", ColorToStr(Colors.TrayDownC))))),
                                             New XElement("graph", New XAttribute("name", "History"),
                                                          New XElement("section", New XAttribute("name", "Download"),
                                                                       New XElement("setting", New XAttribute("name", "Start"), New XElement("value", ColorToStr(Colors.HistoryDownA))),
                                                                       New XElement("setting", New XAttribute("name", "Mid"), New XElement("value", ColorToStr(Colors.HistoryDownB))),
                                                                       New XElement("setting", New XAttribute("name", "End"), New XElement("value", ColorToStr(Colors.HistoryDownC))),
                                                                       New XElement("setting", New XAttribute("name", "Maximum"), New XElement("value", ColorToStr(Colors.HistoryDownMax))),
                                                                       New XElement("setting", New XAttribute("name", "Line"), New XElement("value", ColorToStr(Colors.HistoryDownLine)))),
                                                          New XElement("setting", New XAttribute("name", "Text"), New XElement("value", ColorToStr(Colors.HistoryText))),
                                                          New XElement("setting", New XAttribute("name", "Background"), New XElement("value", ColorToStr(Colors.HistoryBackground))),
                                                          New XElement("section", New XAttribute("name", "Grid"),
                                                                       New XElement("setting", New XAttribute("name", "Light"), New XElement("value", ColorToStr(Colors.HistoryLightGrid))),
                                                                       New XElement("setting", New XAttribute("name", "Dark"), New XElement("value", ColorToStr(Colors.HistoryDarkGrid)))))))
    If srlFunctions.InUseChecker(ConfigFile, IO.FileAccess.Write) Then
      MakeBackup()
      Try
        xConfig.Save(ConfigFile)
        Return True
      Catch ex As Exception
        MsgDlg(Nothing, "There was an error saving the settings file.", "Settings were not saved.", "Program Settings Error", MessageBoxButtons.OK, _TaskDialogIcon.Batch, MessageBoxIcon.Warning, , ex.Message, _TaskDialogExpandedDetailsLocation.ExpandFooter, "View Error Details", "Hide Error Details")
      End Try
    Else
      MsgDlg(Nothing, My.Application.Info.ProductName & " was unable to write to the settings file.", "Settings were not saved.", "Program Settings Error", MessageBoxButtons.OK, _TaskDialogIcon.Batch, MessageBoxIcon.Warning, , "The program could not get write permissions." & vbNewLine & "The file """ & ConfigFile & """ may be in use.", _TaskDialogExpandedDetailsLocation.ExpandFooter, "View Error Details", "Hide Error Details")
    End If
    Return False
  End Function
  Private Shared Function ColorToStr(c As Color) As String
    Dim sA As String
    If c = Color.Transparent Then
      sA = "00"
    Else
      If c.A > 0 Then
        sA = Hex(c.A)
        If sA.Length = 0 Then
          sA = "FF"
        ElseIf sA.Length = 1 Then
          sA = "0" & sA
        End If
      Else
        sA = "FF"
      End If
    End If
    Dim sR As String
    If c.R > 0 Then
      sR = Hex(c.R)
      If sR.Length = 0 Then
        sR = "00"
      ElseIf sR.Length = 1 Then
        sR = "0" & sR
      End If
    Else
      sR = "00"
    End If
    Dim sG As String
    If c.G > 0 Then
      sG = Hex(c.G)
      If sG.Length = 0 Then
        sG = "00"
      ElseIf sG.Length = 1 Then
        sG = "0" & sG
      End If
    Else
      sG = "00"
    End If
    Dim sB As String
    If c.B > 0 Then
      sB = Hex(c.B)
      If sB.Length = 0 Then
        sB = "00"
      ElseIf sB.Length = 1 Then
        sB = "0" & sB
      End If
    Else
      sB = "00"
    End If
    Return sA & sR & sG & sB
  End Function
  Private Shared Function StrToColor(s As String) As Color
    Dim iColor As Integer
    If Integer.TryParse(s, Globalization.NumberStyles.HexNumber, Globalization.CultureInfo.CurrentCulture, iColor) Then
      If Not (iColor And &HFF000000) = &HFF000000 Then Return Color.Transparent
      Return Color.FromArgb(iColor)
    Else
      Return Color.Transparent
    End If
  End Function
  Public Shared Sub MakeBackup()
    If My.Computer.FileSystem.FileExists(ConfigFile) Then
      My.Computer.FileSystem.CopyFile(ConfigFile, ConfigFileBackup, True)
    End If
  End Sub
  Public Shared Sub BackupCheckup()
    If My.Computer.FileSystem.FileExists(ConfigFile) Then
      If My.Computer.FileSystem.FileExists(ConfigFileBackup) Then
        Dim xConfig As XElement
        Try
          xConfig = XElement.Load(ConfigFile)
          Dim xuserSettings As XElement = xConfig.Element("userSettings")
          If xuserSettings Is Nothing Then My.Computer.FileSystem.CopyFile(ConfigFileBackup, ConfigFile, True)
        Catch ex As Exception
          My.Computer.FileSystem.CopyFile(ConfigFileBackup, ConfigFile, True)
        Finally
          My.Computer.FileSystem.DeleteFile(ConfigFileBackup)
        End Try
        xConfig = Nothing
      End If
    Else
      If My.Computer.FileSystem.FileExists(ConfigFileBackup) Then
        My.Computer.FileSystem.CopyFile(ConfigFileBackup, ConfigFile, True)
        My.Computer.FileSystem.DeleteFile(ConfigFileBackup)
      End If
    End If
  End Sub
  Public Property Account As String
    Get
      Return m_Account
    End Get
    Set(value As String)
      m_Account = value
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
  Public Property PassKey As String
    Get
      Return m_PassKey
    End Get
    Set(value As String)
      m_PassKey = value
    End Set
  End Property
  Public Property PassSalt As String
    Get
      Return m_PassSalt
    End Get
    Set(value As String)
      m_PassSalt = value
    End Set
  End Property
  Public Property Share As Boolean
    Get
      Return m_Share
    End Get
    Set(value As Boolean)
      m_Share = value
    End Set
  End Property
  Public Property StartWait As Integer
    Get
      Return m_StartWait
    End Get
    Set(value As Integer)
      m_StartWait = value
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
  Public Property Gr As String
    Get
      Return m_Gr
    End Get
    Set(value As String)
      m_Gr = value
    End Set
  End Property
  Public Property LastUpdate As Date
    Get
      Return m_LastUpdate
    End Get
    Set(value As Date)
      m_LastUpdate = value
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
  Public Property Accuracy As Integer
    Get
      Return m_Accuracy
    End Get
    Set(value As Integer)
      m_Accuracy = value
    End Set
  End Property
  Public Property Ago As UInteger
    Get
      Return m_Ago
    End Get
    Set(value As UInteger)
      m_Ago = value
    End Set
  End Property
  Public Property Service As Boolean
    Get
      Return m_Service
    End Get
    Set(value As Boolean)
      m_Service = value
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
  Public Property UpdateBETA As Boolean
    Get
      Return m_UpdateBETA
    End Get
    Set(value As Boolean)
      m_UpdateBETA = value
    End Set
  End Property
  Public Property UpdateType As UpdateTypes
    Get
      Return m_UpdateType
    End Get
    Set(value As UpdateTypes)
      m_UpdateType = value
    End Set
  End Property
  Public Property UpdateTime As Byte
    Get
      Return m_UpdateTime
    End Get
    Set(value As Byte)
      m_UpdateTime = value
    End Set
  End Property
  Public Property ScaleScreen As Boolean
    Get
      Return m_ScaleScreen
    End Get
    Set(value As Boolean)
      m_ScaleScreen = value
    End Set
  End Property
  Public Property MainSize As Size
    Get
      Return m_MainSize
    End Get
    Set(value As Size)
      m_MainSize = value
    End Set
  End Property
  Public Property TopMost As Boolean
    Get
      Return m_TopMost
    End Get
    Set(value As Boolean)
      m_TopMost = value
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
  Public Property Retries As Integer
    Get
      Return m_Retries
    End Get
    Set(value As Integer)
      m_Retries = value
    End Set
  End Property
  Public Property Overuse As Integer
    Get
      Return m_Overuse
    End Get
    Set(value As Integer)
      m_Overuse = value
    End Set
  End Property
  Public Property Overtime As Integer
    Get
      Return m_Overtime
    End Get
    Set(value As Integer)
      m_Overtime = value
    End Set
  End Property
  Public Property AlertStyle As String
    Get
      Return m_AlertStyle
    End Get
    Set(value As String)
      m_AlertStyle = value
    End Set
  End Property
  Public Property TrayIconStyle As TrayStyles
    Get
      Return m_TrayIcon
    End Get
    Set(value As TrayStyles)
      m_TrayIcon = value
    End Set
  End Property
  Public Property TrayIconAnimation As Boolean
    Get
      Return m_TrayAnim
    End Get
    Set(value As Boolean)
      m_TrayAnim = value
    End Set
  End Property
  Public Property TrayIconOnClose As Boolean
    Get
      Return m_TrayClose
    End Get
    Set(value As Boolean)
      m_TrayClose = value
    End Set
  End Property
  Public Property AutoHide
    Get
      Return m_AutoHide
    End Get
    Set(value)
      m_AutoHide = value
    End Set
  End Property
  Public Property TLSProxy
    Get
      Return m_TLSProxy
    End Get
    Set(value)
      m_TLSProxy = value
    End Set
  End Property
  Public Property Proxy As Net.IWebProxy
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
              If myProxySettings.Length > 3 Then
                Dim pUser As String = myProxySettings(2)
                Dim pPass As String = myProxySettings(3)
                If myProxySettings.Length > 4 Then
                  Dim pDomain As String = myProxySettings(4)
                  Return New Net.WebProxy(pURL, False, Nothing, New Net.NetworkCredential(pUser, pPass, pDomain))
                Else
                  Return New Net.WebProxy(pURL, False, Nothing, New Net.NetworkCredential(pUser, pPass))
                End If
              Else
                Dim pPort As Integer = Replace(myProxySettings(2), "/", String.Empty)
                Return New Net.WebProxy(pURL, pPort)
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
      Return m_SecurProtocol
    End Get
    Set(value As Net.SecurityProtocolType)
      m_SecurProtocol = value
    End Set
  End Property
  Public Property SecurityEnforced As Boolean
    Get
      Return m_SecurEnforced
    End Get
    Set(value As Boolean)
      m_SecurEnforced = value
    End Set
  End Property
  Public Property NetTestURL As String
    Get
      Return m_NetTest
    End Get
    Set(value As String)
      m_NetTest = value
    End Set
  End Property
  Class AppColors
    Private c_MainDA As Color
    Private c_MainDB As Color
    Private c_MainDC As Color
    Private c_MainText As Color
    Private c_MainBG As Color
    Private c_TrayDA As Color
    Private c_TrayDB As Color
    Private c_TrayDC As Color
    Private c_HistoryDA As Color
    Private c_HistoryDB As Color
    Private c_HistoryDC As Color
    Private c_HistoryDM As Color
    Private c_HistoryDLine As Color
    Private c_HistoryText As Color
    Private c_HistoryBG As Color
    Private c_HistoryLight As Color
    Private c_HistoryDark As Color
    Public Property MainDownA As Color
      Get
        Return c_MainDA
      End Get
      Set(value As Color)
        c_MainDA = value
      End Set
    End Property
    Public Property MainDownB As Color
      Get
        Return c_MainDB
      End Get
      Set(value As Color)
        c_MainDB = value
      End Set
    End Property
    Public Property MainDownC As Color
      Get
        Return c_MainDC
      End Get
      Set(value As Color)
        c_MainDC = value
      End Set
    End Property
    Public Property MainText As Color
      Get
        Return c_MainText
      End Get
      Set(value As Color)
        c_MainText = value
      End Set
    End Property
    Public Property MainBackground As Color
      Get
        Return c_MainBG
      End Get
      Set(value As Color)
        c_MainBG = value
      End Set
    End Property
    Public Property TrayDownA As Color
      Get
        Return c_TrayDA
      End Get
      Set(value As Color)
        c_TrayDA = value
      End Set
    End Property
    Public Property TrayDownB As Color
      Get
        Return c_TrayDB
      End Get
      Set(value As Color)
        c_TrayDB = value
      End Set
    End Property
    Public Property TrayDownC As Color
      Get
        Return c_TrayDC
      End Get
      Set(value As Color)
        c_TrayDC = value
      End Set
    End Property
    Public Property HistoryDownA As Color
      Get
        Return c_HistoryDA
      End Get
      Set(value As Color)
        c_HistoryDA = value
      End Set
    End Property
    Public Property HistoryDownB As Color
      Get
        Return c_HistoryDB
      End Get
      Set(value As Color)
        c_HistoryDB = value
      End Set
    End Property
    Public Property HistoryDownC As Color
      Get
        Return c_HistoryDC
      End Get
      Set(value As Color)
        c_HistoryDC = value
      End Set
    End Property
    Public Property HistoryDownMax As Color
      Get
        Return c_HistoryDM
      End Get
      Set(value As Color)
        c_HistoryDM = value
      End Set
    End Property
    Public Property HistoryDownLine As Color
      Get
        Return c_HistoryDLine
      End Get
      Set(value As Color)
        c_HistoryDLine = value
      End Set
    End Property
    Public Property HistoryText As Color
      Get
        Return c_HistoryText
      End Get
      Set(value As Color)
        c_HistoryText = value
      End Set
    End Property
    Public Property HistoryBackground As Color
      Get
        Return c_HistoryBG
      End Get
      Set(value As Color)
        c_HistoryBG = value
      End Set
    End Property
    Public Property HistoryLightGrid As Color
      Get
        Return c_HistoryLight
      End Get
      Set(value As Color)
        c_HistoryLight = value
      End Set
    End Property
    Public Property HistoryDarkGrid As Color
      Get
        Return c_HistoryDark
      End Get
      Set(value As Color)
        c_HistoryDark = value
      End Set
    End Property
  End Class
End Class
