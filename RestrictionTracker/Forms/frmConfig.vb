﻿Friend NotInheritable Class frmConfig
  Private bSaved, bAccount, bLoaded, bHardChange As Boolean
  Private mySettings As AppSettings
  Private keyPasting As Boolean = False
  Private dwmComp As Boolean
#Region "Form Events"
  Private Sub frmConfig_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    bLoaded = False
    mySettings = New AppSettings
    If LocalAppDataDirectory = IO.Path.Combine(Application.StartupPath, "Config") Then mySettings.HistoryDir = IO.Path.Combine(Application.StartupPath, "Config")
    RepadAllItems(Me)
    Dim Username As String = mySettings.Account
    txtAccount.Text = Username
    ttConfig.SetToolTip(txtPassword.Button, "Toggle display of the Password.")
    If Not String.IsNullOrEmpty(mySettings.PassCrypt) Then
      If String.IsNullOrEmpty(mySettings.PassKey) Or String.IsNullOrEmpty(mySettings.PassSalt) Then
        txtPassword.Text = StoredPasswordLegacy.DecryptApp(mySettings.PassCrypt)
      Else
        txtPassword.Text = StoredPassword.Decrypt(mySettings.PassCrypt, mySettings.PassKey, mySettings.PassSalt)
      End If
    End If
    chkAccountShare.Checked = mySettings.Share
    chkAccountShare.Tag = False
    lblAccountShareWarning2.Visible = mySettings.Share
    chkStartUp.Checked = My.Computer.FileSystem.FileExists(StartupPath)
    If mySettings.StartWait > txtStartWait.Maximum Then mySettings.StartWait = txtStartWait.Maximum
    If mySettings.StartWait < txtStartWait.Minimum Then mySettings.StartWait = txtStartWait.Minimum
    txtStartWait.Value = mySettings.StartWait
    txtStartWait.LargeIncrement = 1
    chkAutoHide.Checked = mySettings.AutoHide
    If mySettings.Interval > txtInterval.Maximum Then mySettings.Interval = txtInterval.Maximum
    If mySettings.Interval < txtInterval.Minimum Then mySettings.Interval = txtInterval.Minimum
    txtInterval.Value = mySettings.Interval
    txtInterval.LargeIncrement = 5
    If mySettings.Accuracy > txtAccuracy.Maximum Then mySettings.Accuracy = txtAccuracy.Maximum
    If mySettings.Accuracy < txtAccuracy.Minimum Then mySettings.Accuracy = txtAccuracy.Minimum
    txtAccuracy.Value = mySettings.Accuracy
    txtAccuracy.LargeIncrement = 1
    If mySettings.Timeout > txtTimeout.Maximum Then mySettings.Timeout = txtTimeout.Maximum
    If mySettings.Timeout < txtTimeout.Minimum Then mySettings.Timeout = txtTimeout.Minimum
    txtTimeout.Value = mySettings.Timeout
    txtTimeout.LargeIncrement = 15
    If mySettings.Retries > txtRetries.Maximum Then mySettings.Retries = txtRetries.Maximum
    If mySettings.Retries < txtRetries.Minimum Then mySettings.Retries = txtRetries.Minimum
    txtRetries.Value = mySettings.Retries
    txtRetries.LargeIncrement = 1
    If mySettings.Overuse = 0 Then
      chkOverAlert.Checked = False
      txtOverSize.Value = 100
    Else
      chkOverAlert.Checked = True
      txtOverSize.Value = mySettings.Overuse
    End If
    chkOverAlert_CheckedChanged(New Object, New EventArgs)
    txtOverTime.Value = mySettings.Overtime
    chkScaleScreen.Checked = mySettings.ScaleScreen
    CheckComposition()
    Select Case mySettings.TrayIconStyle
      Case AppSettings.TrayStyles.Always
        chkTrayIcon.Checked = True
        chkTrayMin.Checked = False
      Case AppSettings.TrayStyles.Minimized
        chkTrayIcon.Checked = True
        chkTrayMin.Checked = True
      Case AppSettings.TrayStyles.Never
        chkTrayIcon.Checked = False
        chkTrayMin.Checked = False
    End Select
    chkTrayIcon_CheckedChanged(New Object, New EventArgs)
    chkTrayAnim.Checked = mySettings.TrayIconAnimation
    chkTrayClose.Checked = mySettings.TrayIconOnClose
    ttConfig.SetToolTip(txtProxyPassword.Button, "Toggle display of the HTTP Proxy Password.")
    If mySettings.Proxy Is Nothing Then
      cmbProxyType.SelectedIndex = 0
      txtProxyAddress.Text = String.Empty
      txtProxyPort.Value = 8080
      txtProxyUser.Text = String.Empty
      txtProxyPassword.Text = String.Empty
      txtProxyDomain.Text = String.Empty
    ElseIf mySettings.Proxy.Equals(Net.WebRequest.DefaultWebProxy) Then
      cmbProxyType.SelectedIndex = 1
      txtProxyAddress.Text = String.Empty
      txtProxyPort.Value = 8080
      txtProxyUser.Text = String.Empty
      txtProxyPassword.Text = String.Empty
      txtProxyDomain.Text = String.Empty
    Else
      Dim wProxy As Net.WebProxy = mySettings.Proxy
      If IsNumeric(Replace(wProxy.Address.Host, ".", String.Empty)) Then
        cmbProxyType.SelectedIndex = 2
        txtProxyAddress.Text = wProxy.Address.Host
        txtProxyPort.Value = wProxy.Address.Port
      Else
        cmbProxyType.SelectedIndex = 3
        txtProxyAddress.Text = wProxy.Address.OriginalString
        txtProxyPort.Value = 8080
      End If
      If wProxy.Credentials IsNot Nothing Then
        Dim mCreds As Net.NetworkCredential = wProxy.Credentials.GetCredential(Nothing, String.Empty)
        txtProxyUser.Text = mCreds.UserName
        txtProxyPassword.Text = mCreds.Password
        If String.IsNullOrEmpty(mCreds.Domain) Then
          txtProxyDomain.Text = String.Empty
        Else
          txtProxyDomain.Text = mCreds.Domain
        End If
      Else
        txtProxyUser.Text = String.Empty
        txtProxyPassword.Text = String.Empty
        txtProxyDomain.Text = String.Empty
      End If
    End If
    chkNetworkProtocolSSL3.Checked = (mySettings.SecurityProtocol And SecurityProtocolTypeEx.Ssl3) = SecurityProtocolTypeEx.Ssl3
    chkNetworkProtocolTLS10.Checked = (mySettings.SecurityProtocol And SecurityProtocolTypeEx.Tls10) = SecurityProtocolTypeEx.Tls10
    chkNetworkProtocolTLS11.Checked = (mySettings.SecurityProtocol And SecurityProtocolTypeEx.Tls11) = SecurityProtocolTypeEx.Tls11
    chkNetworkProtocolTLS12.Checked = (mySettings.SecurityProtocol And SecurityProtocolTypeEx.Tls12) = SecurityProtocolTypeEx.Tls12
    chkNetworkProtocolTLS13.Checked = (mySettings.SecurityProtocol And SecurityProtocolTypeEx.Tls13) = SecurityProtocolTypeEx.Tls13
    Dim useTLSProxy As Boolean = False
    Dim myProtocol As SecurityProtocolTypeEx = Net.ServicePointManager.SecurityProtocol
    Dim cRel As UInt32 = srlFunctions.CLRRelease
    Try
      Net.ServicePointManager.SecurityProtocol = SecurityProtocolTypeEx.Ssl3
    Catch ex As Exception
      useTLSProxy = True
    End Try
    Try
      Net.ServicePointManager.SecurityProtocol = SecurityProtocolTypeEx.Tls10
    Catch ex As Exception
      useTLSProxy = True
    End Try
    If (Environment.OSVersion.Version.Major < 10) Then
      useTLSProxy = True
    ElseIf Environment.Version.Revision = 42000 And cRel < &H80000 Then
      useTLSProxy = True
    Else
      Try
        Net.ServicePointManager.SecurityProtocol = SecurityProtocolTypeEx.Tls11
      Catch ex As Exception
        useTLSProxy = True
      End Try
      Try
        Net.ServicePointManager.SecurityProtocol = SecurityProtocolTypeEx.Tls12
      Catch ex As Exception
        useTLSProxy = True
      End Try
      Try
        Net.ServicePointManager.SecurityProtocol = SecurityProtocolTypeEx.Tls13
      Catch ex As Exception
        useTLSProxy = True
      End Try
    End If
    Net.ServicePointManager.SecurityProtocol = myProtocol
    If useTLSProxy Then
      chkTLSProxy.Visible = True
      chkTLSProxy.Checked = mySettings.TLSProxy
    Else
      chkTLSProxy.Checked = False
      chkTLSProxy.Visible = False
    End If
    chkNetworkSecurityEnforce.Checked = mySettings.SecurityEnforced
    RunNetworkProtocolTest()
    If String.IsNullOrEmpty(mySettings.NetTestURL) Then
      optNetTestNone.Checked = True
    Else
      Select Case mySettings.NetTestURL
        Case "http://testmy.net" : optNetTestTestMyNet.Checked = True
        Case "http://speedtest.net" : optNetTestSpeedTest.Checked = True
        Case Else
          optNetTestCustom.Checked = True
          txtNetTestCustom.Text = mySettings.NetTestURL
      End Select
    End If
    Select Case mySettings.UpdateType
      Case AppSettings.UpdateTypes.Auto : cmbUpdateAutomation.SelectedIndex = 0
      Case AppSettings.UpdateTypes.Ask : cmbUpdateAutomation.SelectedIndex = 1
      Case AppSettings.UpdateTypes.None : cmbUpdateAutomation.SelectedIndex = 2
    End Select
    chkUpdateBETA.Checked = mySettings.UpdateBETA
    Select Case mySettings.UpdateTime
      Case 1 : cmbUpdateInterval.SelectedIndex = 0
      Case 3 : cmbUpdateInterval.SelectedIndex = 1
      Case 7 : cmbUpdateInterval.SelectedIndex = 2
      Case 15 : cmbUpdateInterval.SelectedIndex = 3
      Case 30 : cmbUpdateInterval.SelectedIndex = 4
      Case Else : cmbUpdateInterval.SelectedIndex = 3
    End Select
    DoCheck()
    Dim DisableHistory As Boolean = False
    Dim aD As String = LocalAppDataDirectory
    If aD.EndsWith(IO.Path.DirectorySeparatorChar) Then aD = aD.Substring(0, aD.Length - 1)
    Dim hD As String = mySettings.HistoryDir
    If String.IsNullOrEmpty(hD) Then hD = AppDataAllPath
    If hD.EndsWith(IO.Path.DirectorySeparatorChar) Then hD = hD.Substring(0, hD.Length - 1)
    Dim tDA As String = AppDataAllPath
    If tDA.EndsWith(IO.Path.DirectorySeparatorChar) Then tDA = tDA.Substring(0, tDA.Length - 1)
    Dim tD As String = AppDataPath
    If tD.EndsWith(IO.Path.DirectorySeparatorChar) Then tD = tD.Substring(0, tD.Length - 1)
    If chkService.Checked Then
      optHistoryProgramData.Checked = True
      DisableHistory = True
    ElseIf String.Compare(aD, IO.Path.Combine(Application.StartupPath, "Config")) = 0 Then
      optHistoryProgramData.Checked = False
      optHistoryAppData.Checked = False
      optHistoryCustom.Checked = False
      mySettings.HistoryDir = IO.Path.Combine(Application.StartupPath, "Config")
      DisableHistory = True
      lblPortableDir.Enabled = False
      txtPortableDir.Enabled = False
      cmdPortableDir.Enabled = False
      cmdMakePortable.Enabled = False
      ttConfig.SetToolTip(cmdMakePortable, "This application is already portable!")
    ElseIf String.Compare(hD, AppDataAllPath, StringComparison.OrdinalIgnoreCase) = 0 Then
      optHistoryProgramData.Checked = True
    ElseIf String.Compare(hD, AppDataPath, StringComparison.OrdinalIgnoreCase) = 0 Then
      optHistoryAppData.Checked = True
    Else
      optHistoryCustom.Checked = True
    End If
    txtHistoryDir.Text = mySettings.HistoryDir
    If String.IsNullOrEmpty(txtHistoryDir.Text) Then txtHistoryDir.Text = MySaveDir(True)
    optHistoryAppData.Enabled = Not DisableHistory
    optHistoryProgramData.Enabled = Not DisableHistory
    optHistoryCustom.Enabled = Not DisableHistory
    txtHistoryDir.Enabled = Not DisableHistory And optHistoryCustom.Checked
    cmdHistoryDir.Enabled = Not DisableHistory And optHistoryCustom.Checked
    cmdSave.Enabled = False
    bSaved = False
    bAccount = False
    fswController.Path = Application.StartupPath
    fswController.Filter = "RestrictionController.exe"
    fswController.NotifyFilter = IO.NotifyFilters.Attributes Or IO.NotifyFilters.CreationTime Or IO.NotifyFilters.DirectoryName Or IO.NotifyFilters.FileName Or IO.NotifyFilters.LastAccess Or IO.NotifyFilters.LastWrite Or IO.NotifyFilters.Security Or IO.NotifyFilters.Size
    fswController.EnableRaisingEvents = True
    bLoaded = True
  End Sub
  Private Sub RunNetworkProtocolTest()
    lblRetries1.Enabled = True
    txtRetries.Enabled = True
    lblRetries2.Enabled = True
    ttConfig.SetToolTip(txtRetries, "Number of times to retry a connection that times out or fails before giving up." & vbNewLine & "If you run into errors that say ""Please try again"", you can increase this number to improve the chances of a good connection.")
    chkTLSProxy.Enabled = True
    ttConfig.SetToolTip(chkTLSProxy, "If your Operating System does not support the Security Protocol required for your provider, you can use this Proxy to connect through the RealityRipple.com server.")
    If chkTLSProxy.Checked Then
      chkNetworkSecurityEnforce.Enabled = False
      ttConfig.SetToolTip(chkNetworkSecurityEnforce, "The server's certificate will be validated at the discretion of the TLS Proxy.")
      chkNetworkProtocolSSL3.Enabled = False
      ttConfig.SetToolTip(chkNetworkProtocolSSL3, "SSL 3.0 is disabled when using the TLS Proxy.")
      chkNetworkProtocolTLS10.Enabled = False
      ttConfig.SetToolTip(chkNetworkProtocolTLS10, "TLS 1.0 is enabled when using the TLS Proxy.")
      chkNetworkProtocolTLS11.Enabled = False
      ttConfig.SetToolTip(chkNetworkProtocolTLS11, "TLS 1.1 is enabled when using the TLS Proxy.")
      chkNetworkProtocolTLS12.Enabled = False
      ttConfig.SetToolTip(chkNetworkProtocolTLS12, "TLS 1.2 is enabled when using the TLS Proxy.")
      chkNetworkProtocolTLS13.Enabled = False
      ttConfig.SetToolTip(chkNetworkProtocolTLS13, "TLS 1.3 is enabled when using the TLS Proxy.")
      Return
    End If
    chkNetworkSecurityEnforce.Enabled = True
    ttConfig.SetToolTip(chkNetworkSecurityEnforce, "Enforce network certificate validation. If the server's certificate is not valid, your connection may fail." & vbNewLine & "Turning this feature off may potentially expose your computer or transmitted information to third parties.")
    Dim canSSL3 As Boolean = True
    Dim canTLS10 As Boolean = True
    Dim canTLS11 As Boolean = True
    Dim canTLS12 As Boolean = True
    Dim canTLS13 As Boolean = True
    Dim myProtocol As SecurityProtocolTypeEx = Net.ServicePointManager.SecurityProtocol
    Try
      Net.ServicePointManager.SecurityProtocol = SecurityProtocolTypeEx.Ssl3
    Catch ex As Exception
      canSSL3 = False
    End Try
    Try
      Net.ServicePointManager.SecurityProtocol = SecurityProtocolTypeEx.Tls10
    Catch ex As Exception
      canTLS10 = False
    End Try
    Try
      Net.ServicePointManager.SecurityProtocol = SecurityProtocolTypeEx.Tls11
    Catch ex As Exception
      canTLS11 = False
    End Try
    Try
      Net.ServicePointManager.SecurityProtocol = SecurityProtocolTypeEx.Tls12
    Catch ex As Exception
      canTLS12 = False
    End Try
    Try
      Net.ServicePointManager.SecurityProtocol = SecurityProtocolTypeEx.Tls13
    Catch ex As Exception
      canTLS13 = False
    End Try
    If (Environment.OSVersion.Version.Major < 6) OrElse
      (Environment.OSVersion.Version.Major = 6 And Environment.OSVersion.Version.Minor = 0) Then
      canTLS11 = False
      canTLS12 = False
    End If
    If (Environment.OSVersion.Version.Major < 10) Then
      canTLS13 = False
    End If
    Net.ServicePointManager.SecurityProtocol = myProtocol
    If canSSL3 Then
      chkNetworkProtocolSSL3.Enabled = True
      ttConfig.SetToolTip(chkNetworkProtocolSSL3, "Check this box to allow use of the older SSL 3.0 protocol, which is vulnerable to attacks.")
    Else
      chkNetworkProtocolSSL3.Checked = False
      chkNetworkProtocolSSL3.Enabled = False
      ttConfig.SetToolTip(chkNetworkProtocolSSL3, "Your Operating System or version of the .NET Framework does not allow SSL 3.0 connections. Probably for the best, as this protocol is vulnerable to attacks.")
    End If
    If canTLS10 Then
      chkNetworkProtocolTLS10.Enabled = True
      ttConfig.SetToolTip(chkNetworkProtocolTLS10, "Check this box to allow use of the older TLS 1.0 protocol, which may be vulnerable to attacks.")
    Else
      chkNetworkProtocolTLS10.Checked = False
      chkNetworkProtocolTLS10.Enabled = False
      ttConfig.SetToolTip(chkNetworkProtocolTLS10, "Your Operating System or version of the .NET Framework does not allow TLS 1.0 connections. Probably for the best, as this protocol is vulnerable to attacks.")
    End If
    If canTLS11 Then
      chkNetworkProtocolTLS11.Enabled = True
      ttConfig.SetToolTip(chkNetworkProtocolTLS11, "Check this box to allow use of the newer, safer TLS 1.1 protocol.")
    Else
      chkNetworkProtocolTLS11.Checked = False
      chkNetworkProtocolTLS11.Enabled = False
      If (Environment.OSVersion.Version.Major < 6) OrElse
       (Environment.OSVersion.Version.Major = 6 And Environment.OSVersion.Version.Minor = 0) Then
        ttConfig.SetToolTip(chkNetworkProtocolTLS11, "Your Operating System does not allow TLS 1.1 connections. Enable the TLS Proxy if you need TLS 1.1.")
      ElseIf Environment.Version.Revision < 17929 Then
        ttConfig.SetToolTip(chkNetworkProtocolTLS11, "Your version of the .NET Framework does not allow TLS 1.1 connections. Please update to .NET 4.5 or newer.")
      Else
        ttConfig.SetToolTip(chkNetworkProtocolTLS11, "Your Operating System or version of the .NET Framework does not allow TLS 1.1 connections. Try repairing the .NET Framework.")
      End If
    End If
    If canTLS12 Then
      chkNetworkProtocolTLS12.Enabled = True
      ttConfig.SetToolTip(chkNetworkProtocolTLS12, "Check this box to allow use of the newer, safer TLS 1.2 protocol.")
    Else
      chkNetworkProtocolTLS12.Checked = False
      chkNetworkProtocolTLS12.Enabled = False
      If (Environment.OSVersion.Version.Major < 6) OrElse
       (Environment.OSVersion.Version.Major = 6 And Environment.OSVersion.Version.Minor = 0) Then
        ttConfig.SetToolTip(chkNetworkProtocolTLS12, "Your Operating System does not allow TLS 1.2 connections. Enable the TLS Proxy if you need TLS 1.2.")
      ElseIf Environment.Version.Revision < 17929 Then
        ttConfig.SetToolTip(chkNetworkProtocolTLS12, "Your version of the .NET Framework does not allow TLS 1.2 connections. Please update to .NET 4.5 or newer.")
      Else
        ttConfig.SetToolTip(chkNetworkProtocolTLS12, "Your Operating System or version of the .NET Framework does not allow TLS 1.2 connections. Try repairing the .NET Framework.")
      End If
    End If
    If canTLS13 Then
      chkNetworkProtocolTLS13.Enabled = True
      ttConfig.SetToolTip(chkNetworkProtocolTLS13, "Check this box to allow use of the latest TLS 1.3 protocol.")
    Else
      chkNetworkProtocolTLS13.Checked = False
      chkNetworkProtocolTLS13.Enabled = False
      Dim cRel As UInt32 = srlFunctions.CLRRelease
      If (Environment.OSVersion.Version.Major < 10) Then
        ttConfig.SetToolTip(chkNetworkProtocolTLS13, "Your Operating System does not allow TLS 1.3 connections. Enable the TLS Proxy if you need TLS 1.3.")
      ElseIf Environment.Version.Revision = 42000 And cRel < &H80000 Then
        ttConfig.SetToolTip(chkNetworkProtocolTLS13, "Your version of the .NET Framework does not allow TLS 1.3 connections. Please update to .NET 4.8 or newer.")
      Else
        ttConfig.SetToolTip(chkNetworkProtocolTLS13, "Your Operating System or version of the .NET Framework does not allow TLS 1.3 connections.")
      End If
    End If
  End Sub
  Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
    Select Case m.Msg
      Case NativeMethods.WM_DWMCOMPOSITIONCHANGED
        CheckComposition()
    End Select
    MyBase.WndProc(m)
  End Sub
  Private Sub frmConfig_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If e.CloseReason = CloseReason.ApplicationExitCall Then
      Me.DialogResult = Windows.Forms.DialogResult.Abort
    Else
      If cmdSave.Enabled Then
        Dim saveRet As DialogResult = MsgDlg(Me, "Do you want to save the changes to your configuration?", "Your changes have not been saved.", "Save Changes?", MessageBoxButtons.YesNoCancel, _TaskDialogIcon.Options, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If saveRet = Windows.Forms.DialogResult.Yes Then
          cmdSave.PerformClick()
          If cmdSave.Enabled Then
            e.Cancel = True
            Me.DialogResult = Windows.Forms.DialogResult.None
            Return
          End If
          If bAccount Then
            Me.DialogResult = Windows.Forms.DialogResult.Yes
          Else
            Me.DialogResult = Windows.Forms.DialogResult.OK
          End If
        ElseIf saveRet = Windows.Forms.DialogResult.No Then
          Me.DialogResult = Windows.Forms.DialogResult.No
        ElseIf saveRet = Windows.Forms.DialogResult.Cancel Then
          e.Cancel = True
          Me.DialogResult = Windows.Forms.DialogResult.None
          Return
        End If
      ElseIf bSaved Then
        If bAccount Then
          Me.DialogResult = Windows.Forms.DialogResult.Yes
        Else
          Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
      Else
        Me.DialogResult = Windows.Forms.DialogResult.No
      End If
    End If
    e.Cancel = False
  End Sub
  Private isShown As Boolean = False
  Private Sub Panel_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pnlAccount.MouseMove,
    pnlAccountViaSat.MouseMove, pnlAccountViaSatInput.MouseMove,
    pnlPrefs.MouseMove,
    pnlPrefStart.MouseMove, pnlPrefStartInput.MouseMove,
    pnlPrefAccuracy.MouseMove, pnlPrefAccuracyInput.MouseMove,
    pnlPrefAlert.MouseMove, pnlPrefColor.MouseMove,
    pnlNetwork.MouseMove,
    pnlNetworkTimeout.MouseMove, pnlNetworkProxy.MouseMove, pnlNetworkProtocol.MouseMove, pnlNetworkUpdate.MouseMove,
    pnlAdvanced.MouseMove,
    pnlAdvancedData.MouseMove, pnlAdvancedDataInput.MouseMove, pnlHistoryDir.MouseMove,
    pnlPrefInterface.MouseMove, pnlAdvancedNetTestInput.MouseMove, pnlAdvancedNetTest.MouseMove,
    pnlAdvancedPortable.MouseMove,
    pnlButtons.MouseMove
    Dim pnlParent As TableLayoutPanel = sender
    Dim ctl As Control = pnlParent.GetChildAtPoint(e.Location)
    If Not ctl Is Nothing Then
      If Not ctl.Enabled And ctl.Visible And Not isShown Then
        Dim tipString As String = ttConfig.GetToolTip(ctl)
        ttConfig.Show(tipString, pnlParent, e.X, e.Y + 32)
        isShown = True
      End If
    Else
      ttConfig.Hide(pnlParent)
      isShown = False
    End If
  End Sub
#End Region
#Region "Inputs"
  Private Sub txtVal_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtAccount.KeyDown, txtPassword.KeyDown
    If e.Control And e.KeyCode = Keys.V Then
      CType(sender, TextBox).SelectedText = Clipboard.GetText.Trim
      e.SuppressKeyPress = True
      e.Handled = True
    End If
  End Sub
  Private Sub ValuesChanged(sender As System.Object, e As EventArgs) Handles txtPassword.KeyPress, txtPassword.TextChanged,
                                                                             txtStartWait.KeyPress, txtStartWait.KeyUp, txtStartWait.Scroll, txtStartWait.ValueChanged,
                                                                             txtInterval.KeyPress, txtInterval.KeyUp, txtInterval.Scroll, txtInterval.ValueChanged,
                                                                             txtAccuracy.KeyPress, txtAccuracy.KeyUp, txtAccuracy.Scroll, txtAccuracy.ValueChanged,
                                                                             txtTimeout.KeyPress, txtTimeout.KeyUp, txtTimeout.Scroll, txtTimeout.ValueChanged,
                                                                             txtRetries.KeyPress, txtRetries.KeyUp, txtRetries.Scroll, txtRetries.ValueChanged,
                                                                             chkStartUp.CheckedChanged, chkAutoHide.CheckedChanged,
                                                                             txtOverSize.KeyPress, txtOverSize.KeyUp, txtOverSize.Scroll, txtOverSize.ValueChanged,
                                                                             txtOverTime.KeyPress, txtOverTime.KeyUp, txtOverTime.Scroll, txtOverTime.ValueChanged,
                                                                             chkNetworkProtocolSSL3.CheckedChanged, chkNetworkProtocolTLS10.CheckedChanged, chkNetworkProtocolTLS11.CheckedChanged, chkNetworkProtocolTLS12.CheckedChanged, chkNetworkProtocolTLS13.CheckedChanged,
                                                                             txtProxyAddress.TextChanged,
                                                                             txtProxyPort.KeyPress, txtProxyPort.KeyUp, txtProxyPort.Scroll, txtProxyPort.ValueChanged,
                                                                             txtProxyUser.TextChanged,
                                                                             txtProxyPassword.TextChanged,
                                                                             txtProxyDomain.TextChanged,
                                                                             cmbUpdateInterval.KeyPress, cmbUpdateInterval.SelectedIndexChanged,
                                                                             txtHistoryDir.KeyPress, txtHistoryDir.TextChanged, chkScaleScreen.CheckedChanged,
                                                                             chkTrayMin.CheckedChanged, chkTrayAnim.CheckedChanged, chkTrayClose.CheckedChanged
    cmdSave.Enabled = SettingsChanged()
  End Sub
  Private Sub txtAccount_ValuesChanged(sender As System.Object, e As EventArgs) Handles txtAccount.KeyPress, txtAccount.TextChanged
    If Not bLoaded Then Return
    cmdSave.Enabled = SettingsChanged()
  End Sub
  Private Sub chkAccountShare_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAccountShare.CheckedChanged
    If Not bLoaded Then Return
    If chkAccountShare.Checked Then
      Dim shareRet As DialogResult = MsgDlg(Me, "Are you absolutely certain you want to share your account information?", "This will share your password.", "Share Account?", MessageBoxButtons.YesNo, _TaskDialogIcon.Options, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
      If shareRet = Windows.Forms.DialogResult.Yes Then
        chkAccountShare.Tag = True
        lblAccountShareWarning2.Visible = True
      Else
        chkAccountShare.Tag = False
        chkAccountShare.Checked = False
      End If
    Else
      chkAccountShare.Tag = False
      lblAccountShareWarning2.Visible = False
    End If
    cmdSave.Enabled = SettingsChanged()
  End Sub
  Private Sub chkService_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkService.CheckedChanged
    If chkService.Checked Then
      txtHistoryDir.Tag = txtHistoryDir.Text
      txtHistoryDir.Text = AppDataAllPath
    ElseIf Not String.IsNullOrEmpty(txtHistoryDir.Tag) Then
      txtHistoryDir.Text = txtHistoryDir.Tag
      txtHistoryDir.Tag = Nothing
      Dim hD As String = mySettings.HistoryDir
      If String.IsNullOrEmpty(hD) Then hD = AppDataPath
      If String.Compare(hD, AppDataAllPath) = 0 Then
        optHistoryProgramData.Checked = True
      ElseIf String.Compare(hD, AppDataPath, StringComparison.OrdinalIgnoreCase) = 0 Then
        optHistoryAppData.Checked = True
      Else
        optHistoryCustom.Checked = True
      End If
    End If
    optHistoryAppData.Enabled = Not chkService.Checked
    optHistoryProgramData.Enabled = Not chkService.Checked
    optHistoryCustom.Enabled = Not chkService.Checked
    If chkService.Checked Then optHistoryProgramData.Checked = True
    txtHistoryDir.Enabled = (Not chkService.Checked) And (optHistoryCustom.Checked)
    cmdHistoryDir.Enabled = (Not chkService.Checked) And (optHistoryCustom.Checked)
    cmdSave.Enabled = SettingsChanged()
  End Sub
  Private Sub chkOverAlert_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkOverAlert.CheckedChanged
    lblOverSize1.Enabled = chkOverAlert.Checked
    txtOverSize.Enabled = chkOverAlert.Checked
    lblOverSize2.Enabled = chkOverAlert.Checked
    lblOverTime1.Enabled = chkOverAlert.Checked
    txtOverTime.Enabled = chkOverAlert.Checked
    lblOverTime2.Enabled = chkOverAlert.Checked
    cmdAlertStyle.Enabled = chkOverAlert.Checked
    cmdSave.Enabled = SettingsChanged()
  End Sub
  Private Sub chkTrayIcon_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkTrayIcon.CheckedChanged
    chkTrayMin.Enabled = chkTrayIcon.Checked
    chkTrayAnim.Enabled = chkTrayIcon.Checked And CleanRendering()
    If Not chkTrayMin.Enabled Then chkTrayMin.Checked = False
    If Not chkTrayIcon.Checked Then chkTrayAnim.Checked = False
    cmdSave.Enabled = SettingsChanged()
  End Sub
  Private Sub cmbProxyType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbProxyType.SelectedIndexChanged
    Select Case cmbProxyType.SelectedIndex
      Case 2
        lblProxyAddr.Enabled = True
        lblProxyAddr.Text = "IP Address:"
        txtProxyAddress.Enabled = True
        pnlProxy.SetColumnSpan(txtProxyAddress, 1)
        lblProxyPort.Visible = True
        txtProxyPort.Visible = True
        lblProxyPort.Enabled = True
        txtProxyPort.Enabled = True
        lblProxyUser.Enabled = True
        txtProxyUser.Enabled = True
        lblProxyPassword.Enabled = True
        txtProxyPassword.Enabled = True
        lblProxyDomain.Enabled = True
        txtProxyDomain.Enabled = True
      Case 3
        lblProxyAddr.Enabled = True
        lblProxyAddr.Text = "URL:"
        txtProxyAddress.Enabled = True
        lblProxyPort.Visible = False
        txtProxyPort.Visible = False
        pnlProxy.SetColumnSpan(txtProxyAddress, 2)
        lblProxyPort.Enabled = False
        txtProxyPort.Enabled = False
        lblProxyUser.Enabled = True
        txtProxyUser.Enabled = True
        lblProxyPassword.Enabled = True
        txtProxyPassword.Enabled = True
        lblProxyDomain.Enabled = True
        txtProxyDomain.Enabled = True
      Case Else
        lblProxyAddr.Enabled = False
        lblProxyAddr.Text = "IP Address:"
        txtProxyAddress.Enabled = False
        pnlProxy.SetColumnSpan(txtProxyAddress, 1)
        lblProxyPort.Visible = True
        txtProxyPort.Visible = True
        lblProxyPort.Enabled = False
        txtProxyPort.Enabled = False
        lblProxyUser.Enabled = False
        txtProxyUser.Enabled = False
        lblProxyPassword.Enabled = False
        txtProxyPassword.Enabled = False
        lblProxyDomain.Enabled = False
        txtProxyDomain.Enabled = False
    End Select
    cmdSave.Enabled = SettingsChanged()
  End Sub
  Private Sub chkNetworkSecurityEnforce_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkNetworkSecurityEnforce.CheckedChanged
    RunNetworkProtocolTest()
    cmdSave.Enabled = SettingsChanged()
  End Sub
  Private Sub chkTLSProxy_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkTLSProxy.CheckedChanged
    RunNetworkProtocolTest()
    cmdSave.Enabled = SettingsChanged()
  End Sub
  Private Sub cmbUpdateAutomation_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbUpdateAutomation.SelectedIndexChanged
    Select Case cmbUpdateAutomation.SelectedIndex
      Case 0
        If chkUpdateBETA.Checked Then
          pctNetworkUpdateIcon.Image = My.Resources.net_update_unknown
        Else
          pctNetworkUpdateIcon.Image = My.Resources.net_update_good
        End If
        lblUpdateInterval.Enabled = True
        cmbUpdateInterval.Enabled = True
        chkUpdateBETA.Enabled = True
        cmdSave.Enabled = SettingsChanged()
      Case 1
        If chkUpdateBETA.Checked Then
          pctNetworkUpdateIcon.Image = My.Resources.net_update_unknown
        Else
          pctNetworkUpdateIcon.Image = My.Resources.net_update_warn
        End If
        lblUpdateInterval.Enabled = True
        cmbUpdateInterval.Enabled = True
        chkUpdateBETA.Enabled = True
        cmdSave.Enabled = SettingsChanged()
      Case 2
        pctNetworkUpdateIcon.Image = My.Resources.net_update_bad
        lblUpdateInterval.Enabled = False
        cmbUpdateInterval.Enabled = False
        chkUpdateBETA.Enabled = False
        cmdSave.Enabled = SettingsChanged()
      Case Else
        pctNetworkUpdateIcon.Image = My.Resources.net_update_unknown
        lblUpdateInterval.Enabled = False
        cmbUpdateInterval.Enabled = False
        chkUpdateBETA.Enabled = False
        cmdSave.Enabled = SettingsChanged()
    End Select
  End Sub
  Private Sub chkUpdateBETA_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkUpdateBETA.CheckedChanged
    If chkUpdateBETA.Checked Then
      pctNetworkUpdateIcon.Image = My.Resources.net_update_unknown
    Else
      If cmbUpdateAutomation.SelectedIndex = 0 Then
        pctNetworkUpdateIcon.Image = My.Resources.net_update_good
      ElseIf cmbUpdateAutomation.SelectedIndex = 1 Then
        pctNetworkUpdateIcon.Image = My.Resources.net_update_warn
      End If
    End If
    cmdSave.Enabled = SettingsChanged()
  End Sub
  Private Sub optHistoryProgramData_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optHistoryProgramData.CheckedChanged
    txtHistoryDir.Enabled = optHistoryCustom.Checked And Not chkService.Checked
    cmdHistoryDir.Enabled = optHistoryCustom.Checked And Not chkService.Checked
    txtHistoryDir.Text = AppDataAllPath
    cmdSave.Enabled = SettingsChanged()
  End Sub
  Private Sub optHistoryAppData_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optHistoryAppData.CheckedChanged
    txtHistoryDir.Enabled = optHistoryCustom.Checked And Not chkService.Checked
    cmdHistoryDir.Enabled = optHistoryCustom.Checked And Not chkService.Checked
    txtHistoryDir.Text = AppDataPath
    cmdSave.Enabled = SettingsChanged()
  End Sub
  Private Sub optHistoryCustom_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optHistoryCustom.CheckedChanged
    txtHistoryDir.Enabled = optHistoryCustom.Checked And Not chkService.Checked
    cmdHistoryDir.Enabled = optHistoryCustom.Checked And Not chkService.Checked
    txtHistoryDir.Text = MySaveDir
    cmdSave.Enabled = SettingsChanged()
  End Sub
  Private Sub optNetTestNone_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optNetTestNone.CheckedChanged
    If optNetTestNone.Checked Then
      txtNetTestCustom.Text = ""
      txtNetTestCustom.Enabled = False
      SetNetTestImage(My.Resources.advanced_nettest_none)
      cmdSave.Enabled = SettingsChanged()
    End If
  End Sub
  Private Sub optNetTestTestMyNet_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optNetTestTestMyNet.CheckedChanged
    If optNetTestTestMyNet.Checked Then
      txtNetTestCustom.Text = "http://testmy.net"
      txtNetTestCustom.Enabled = False
      Dim token As Integer = MakeAToken(txtNetTestCustom.Text)
      SetNetTestImage(pctAdvancedNetTestIcon.InitialImage, False, token)
      Dim wsFavicon As New clsFavicon(AddressOf wsFavicon_DownloadIconCompleted)
      wsFavicon.Start(txtNetTestCustom.Text, token)
      cmdSave.Enabled = SettingsChanged()
    End If
  End Sub
  Private Sub optNetTestSpeedTest_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optNetTestSpeedTest.CheckedChanged
    If optNetTestSpeedTest.Checked Then
      txtNetTestCustom.Text = "http://speedtest.net"
      txtNetTestCustom.Enabled = False
      Dim token As Integer = MakeAToken(txtNetTestCustom.Text)
      SetNetTestImage(pctAdvancedNetTestIcon.InitialImage, False, token)
      Dim wsFavicon As New clsFavicon(AddressOf wsFavicon_DownloadIconCompleted)
      wsFavicon.Start(txtNetTestCustom.Text, token)
      cmdSave.Enabled = SettingsChanged()
    End If
  End Sub
  Private Sub optNetTestCustom_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optNetTestCustom.CheckedChanged
    If optNetTestCustom.Checked Then
      txtNetTestCustom.Text = ""
      txtNetTestCustom.Enabled = True
      SetNetTestImage(My.Resources.advanced_nettest_edit)
      cmdSave.Enabled = SettingsChanged()
    End If
  End Sub
  Private Sub txtNetTestCustom_LostFocus(sender As Object, e As System.EventArgs) Handles txtNetTestCustom.LostFocus
    If optNetTestCustom.Checked Then
      If pctAdvancedNetTestIcon.Tag Is Nothing Then
        tmrIcoWait.Stop()
        If Not String.IsNullOrEmpty(txtNetTestCustom.Text) Then
          Dim token As Integer = MakeAToken(txtNetTestCustom.Text)
          SetNetTestImage(pctAdvancedNetTestIcon.InitialImage, False, token)
          Dim wsFavicon As New clsFavicon(AddressOf wsFavicon_DownloadIconCompleted)
          wsFavicon.Start(txtNetTestCustom.Text, token)
        End If
      End If
    End If
  End Sub
  Private Sub txtNetTestCustom_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtNetTestCustom.TextChanged
    If optNetTestCustom.Checked Then
      SetNetTestImage(My.Resources.advanced_nettest_edit)
      cmdSave.Enabled = SettingsChanged()
      tmrIcoWait.Stop()
      tmrIcoWait.Start()
    End If
  End Sub
  Private Sub txtPortableDir_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtPortableDir.TextChanged
    If String.IsNullOrEmpty(txtPortableDir.Text) Then
      cmdMakePortable.Enabled = False
      ttConfig.SetToolTip(cmdMakePortable, "No directory selected!")
      pctAdvancedPortableIcon.Image = My.Resources.advanced_portable_missing
      Return
    End If
    Try
      If Not IO.Directory.Exists(txtPortableDir.Text) Then
        cmdMakePortable.Enabled = False
        ttConfig.SetToolTip(cmdMakePortable, "Selected directory does not exist!")
        pctAdvancedPortableIcon.Image = My.Resources.advanced_portable_missing
        Return
      End If
    Catch ex As Exception
      cmdMakePortable.Enabled = False
      ttConfig.SetToolTip(cmdMakePortable, "Error accessing selected directory!")
      pctAdvancedPortableIcon.Image = My.Resources.advanced_portable_missing
      Return
    End Try
    cmdMakePortable.Enabled = True
    ttConfig.SetToolTip(cmdMakePortable, "Copy " & My.Application.Info.ProductName & " to the selected directory.")
    pctAdvancedPortableIcon.Image = My.Resources.advanced_portable
  End Sub
#Region "Net Test"
  Private Sub wsFavicon_DownloadIconCompleted(icon16 As Image, icon32 As Image, token As Object, [Error] As Exception)
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New clsFavicon.DownloadIconCompletedCallback(AddressOf wsFavicon_DownloadIconCompleted), icon16, icon32, token, [Error])
      Catch ex As Exception
      End Try
      Return
    End If
    If [Error] IsNot Nothing Then
      SetNetTestImage(pctAdvancedNetTestIcon.ErrorImage, True, token)
    Else
      SetNetTestImage(icon32, True, token, icon16)
    End If
  End Sub
  Private Sub SetNetTestImage(Image As Image)
    waitingForEndOf = 0
    pctAdvancedNetTestIcon.Image = Image
    pctAdvancedNetTestIcon.Tag = Nothing
  End Sub
  Private waitingForEndOf As Object
  Private Sub SetNetTestImage(Image As Image, [End] As Boolean, Token As Object, Optional tag As Object = Nothing)
    If [End] And Not Token = waitingForEndOf Then Return
    pctAdvancedNetTestIcon.Image = Image
    pctAdvancedNetTestIcon.Tag = tag
    If [End] Then
      waitingForEndOf = 0
    Else
      waitingForEndOf = Token
    End If
  End Sub
  Private Sub tmrIcoWait_Tick(sender As System.Object, e As System.EventArgs) Handles tmrIcoWait.Tick
    tmrIcoWait.Stop()
    If optNetTestCustom.Checked Then
      If Not String.IsNullOrEmpty(txtNetTestCustom.Text) Then
        Dim token As Integer = MakeAToken(txtNetTestCustom.Text)
        SetNetTestImage(pctAdvancedNetTestIcon.InitialImage, False, token)
        Dim wsFavicon As New clsFavicon(AddressOf wsFavicon_DownloadIconCompleted)
        wsFavicon.Start(txtNetTestCustom.Text, token)
      Else
        SetNetTestImage(My.Resources.advanced_nettest_edit)
      End If
    End If
  End Sub
  Private Shared Function MakeAToken(fromString As String) As Integer
    Dim iToken As UInteger = fromString.Length * Int(Rnd() * 32) + Int(Rnd() * &HFFFFFFUI)
    iToken = iToken Mod &HFFFFFFUI
    For I As Integer = 0 To fromString.Length - 1
      iToken *= AscW(fromString(I))
      iToken = iToken Mod &HFFFFFFUI
    Next
    Return CInt(iToken)
  End Function
#End Region
#End Region
#Region "Buttons"
  Private Sub cmdAlertStyle_Click(sender As System.Object, e As System.EventArgs) Handles cmdAlertStyle.Click
    frmAlertSelection.AlertStyle = mySettings.AlertStyle
    If frmAlertSelection.ShowDialog(Me) = Windows.Forms.DialogResult.Yes Then
      mySettings.AlertStyle = frmAlertSelection.AlertStyle
      bHardChange = True
      cmdSave.Enabled = SettingsChanged()
    End If
  End Sub
  Private Sub cmdColors_Click(sender As System.Object, e As System.EventArgs) Handles cmdColors.Click
    frmCustomColors.mySettings = mySettings
    If frmCustomColors.ShowDialog(Me) = Windows.Forms.DialogResult.Yes Then
      mySettings = frmCustomColors.mySettings
      bHardChange = True
      cmdSave.Enabled = SettingsChanged()
    End If
  End Sub
  Private Sub cmdHistoryDir_Click(sender As System.Object, e As System.EventArgs) Handles cmdHistoryDir.Click
    Using dirDlg As New SaveFileDialog With
      {
        .AddExtension = False,
        .CheckPathExists = True,
        .CheckFileExists = False,
        .CreatePrompt = False,
        .FileName = "Select a Directory",
        .Filter = "Directories|",
        .OverwritePrompt = False,
        .ShowHelp = False,
        .InitialDirectory = txtHistoryDir.Text,
        .Title = "Select a Folder to store Satellite Usage History Data...",
        .ValidateNames = False
      }
      If dirDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        Dim sDir As String = IO.Path.GetDirectoryName(dirDlg.FileName)
        If sDir.EndsWith(IO.Path.DirectorySeparatorChar) Then sDir = sDir.Substring(0, sDir.Length - 1)
        txtHistoryDir.Text = sDir
      End If
    End Using
  End Sub
  Private Sub cmdHistoryDirOpen_Click(sender As System.Object, e As System.EventArgs) Handles cmdHistoryDirOpen.Click
    If optHistoryProgramData.Checked Then
      If My.Computer.FileSystem.DirectoryExists(AppDataAllPath) Then
        Process.Start(AppDataAllPath)
      Else
        MsgDlg(Me, "The directory """ & AppDataAllPath & """ does not exist." & vbNewLine & "Please save the configuration first.", "Unable to find History directory.", "Missing Directory", MessageBoxButtons.OK, _TaskDialogIcon.Recent, MessageBoxIcon.Error)
      End If
    ElseIf optHistoryAppData.Checked Then
      If My.Computer.FileSystem.DirectoryExists(AppDataPath) Then
        Process.Start(AppDataPath)
      Else
        MsgDlg(Me, "The directory """ & AppDataPath & """ does not exist." & vbNewLine & "Please save the configuration first.", "Unable to find History directory.", "Missing Directory", MessageBoxButtons.OK, _TaskDialogIcon.Recent, MessageBoxIcon.Error)
      End If
    Else
      If My.Computer.FileSystem.DirectoryExists(txtHistoryDir.Text) Then
        Process.Start(txtHistoryDir.Text)
      Else
        MsgDlg(Me, "The directory """ & txtHistoryDir.Text & """ does not exist." & vbNewLine & "Please save the configuration first.", "Unable to find History directory.", "Missing Directory", MessageBoxButtons.OK, _TaskDialogIcon.Recent, MessageBoxIcon.Error)
      End If
    End If
  End Sub
  Private Sub cmdMakePortable_Click(sender As System.Object, e As System.EventArgs) Handles cmdMakePortable.Click
    If String.IsNullOrEmpty(txtPortableDir.Text) Then
      txtPortableDir.Focus()
      Return
    End If
    Try
      If Not IO.Directory.Exists(txtPortableDir.Text) Then
        txtPortableDir.Focus()
        Return
      End If
    Catch ex As Exception
      txtPortableDir.Focus()
      Return
    End Try
    Dim sPath As String = txtPortableDir.Text
    Try
      IO.File.Copy(Application.ExecutablePath, IO.Path.Combine(sPath, "RestrictionTracker.exe"), True)
      IO.File.Copy(IO.Path.Combine(Application.StartupPath, "RestrictionTrackerLib.dll"), IO.Path.Combine(sPath, "RestrictionTrackerLib.dll"), True)
      IO.Directory.CreateDirectory(IO.Path.Combine(sPath, "Config"))
      For Each file As IO.FileInfo In New IO.DirectoryInfo(AppDataPath).EnumerateFiles
        If file.Name = "user.config" Or file.Name = "backup.config" Then
          Dim sConfig As String = My.Computer.FileSystem.ReadAllText(file.FullName, System.Text.Encoding.GetEncoding(srlFunctions.LATIN_1))
          If sConfig.Contains("<setting name=""HistoryDir"">") Then
            Dim sHistory As String = sConfig.Substring(sConfig.IndexOf("<setting name=""HistoryDir"">"))
            sHistory = sHistory.Substring(0, sHistory.IndexOf("</setting>") + 10)
            Dim sNewHistory As String = "<setting name=""HistoryDir"">" & vbNewLine & "        <value></value>" & vbNewLine & "      </setting>"
            sConfig = sConfig.Replace(sHistory, sNewHistory)
          End If
          My.Computer.FileSystem.WriteAllText(IO.Path.Combine(sPath, "Config", file.Name), sConfig, False, System.Text.Encoding.GetEncoding(srlFunctions.LATIN_1))
        Else
          file.CopyTo(IO.Path.Combine(sPath, "Config", file.Name), True)
        End If
      Next
      If Not AppDataPath = MySaveDir Then
        For Each file As IO.FileInfo In New IO.DirectoryInfo(MySaveDir(True)).EnumerateFiles
          file.CopyTo(IO.Path.Combine(sPath, "Config", file.Name), True)
        Next
      End If
      MsgDlg(Me, My.Application.Info.ProductName & " has been ported to """ & sPath & """!", "Portable application created.", "Files Copied", MessageBoxButtons.OK, _TaskDialogIcon.RemovableDrive, MessageBoxIcon.Information)
    Catch ex As Exception
      txtPortableDir.Focus()
      MsgDlg(Me, "There was an error trying to create a portable install in """ & sPath & """!", "Portable application creation error.", "Drive Error", MessageBoxButtons.OK, _TaskDialogIcon.DriveLockedRemovable, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, ex.Message, _TaskDialogExpandedDetailsLocation.ExpandFooter, "View Error Details", "Hide Error Details")
      Return
    End Try
  End Sub
  Private Sub cmdPortableDir_Click(sender As System.Object, e As System.EventArgs) Handles cmdPortableDir.Click
    Using dirDlg As New SaveFileDialog With
      {
        .AddExtension = False,
        .CheckPathExists = True,
        .CheckFileExists = False,
        .CreatePrompt = False,
        .FileName = "Select a Directory",
        .Filter = "Directories|",
        .OverwritePrompt = False,
        .ShowHelp = False,
        .InitialDirectory = txtPortableDir.Text,
        .Title = "Select a Folder to put the Portable Application in...",
        .ValidateNames = False
      }
      If dirDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        Dim sDir As String = IO.Path.GetDirectoryName(dirDlg.FileName)
        If sDir.EndsWith(IO.Path.DirectorySeparatorChar) Then sDir = sDir.Substring(0, sDir.Length - 1)
        txtPortableDir.Text = sDir
      End If
    End Using
  End Sub
  Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click
    If String.IsNullOrEmpty(txtAccount.Text) Then
      MsgDlg(Me, "You must enter your ViaSat account Username before saving the Configuration.", "Please enter your Username.", "Unable to Save", MessageBoxButtons.OK, _TaskDialogIcon.User, MessageBoxIcon.Information)
      txtAccount.Focus()
      Return
    End If
    If String.IsNullOrEmpty(txtPassword.Text) Then
      MsgDlg(Me, "You must enter your ViaSat account Password before saving the Configuration.", "Please enter your Password.", "Unable to Save", MessageBoxButtons.OK, _TaskDialogIcon.Padlock, MessageBoxIcon.Information)
      txtPassword.Focus()
      Return
    End If
    If Not (chkNetworkProtocolSSL3.Checked Or chkNetworkProtocolTLS10.Checked Or chkNetworkProtocolTLS11.Checked Or chkNetworkProtocolTLS12.Checked Or chkNetworkProtocolTLS13.Checked) Then
      MsgDlg(Me, "Please select at least one Security Protocol type to connect with before saving the Configuration.", "Please select your Security Protocol.", "Unable to Save", MessageBoxButtons.OK, _TaskDialogIcon.Padlock, MessageBoxIcon.Information)
      If chkNetworkProtocolTLS13.CanFocus Then
        chkNetworkProtocolTLS13.Focus()
      ElseIf chkNetworkProtocolTLS12.CanFocus Then
        chkNetworkProtocolTLS12.Focus()
      ElseIf chkNetworkProtocolTLS11.CanFocus Then
        chkNetworkProtocolTLS11.Focus()
      ElseIf chkNetworkProtocolTLS10.CanFocus Then
        chkNetworkProtocolTLS10.Focus()
      ElseIf chkNetworkProtocolSSL3.CanFocus Then
        chkNetworkProtocolSSL3.Focus()
      Else
        lblNetworkProtocolDescription.Focus()
      End If
      Return
    End If
    If String.IsNullOrEmpty(txtHistoryDir.Text) Then txtHistoryDir.Text = MySaveDir(True)
    For Each c As Char In IO.Path.GetInvalidPathChars
      If txtHistoryDir.Text.Contains(c) Then
        Dim sHD As String = txtHistoryDir.Text
        Dim sC As String = c.ToString
        Select Case c
          Case vbNullChar
            sC = "Null"
            sHD = Replace(sHD, c, "[Null]")
          Case vbCr
            sC = "Carriage Return"
            sHD = Replace(sHD, c, "[CR]")
          Case vbLf
            sC = "Line Feed"
            sHD = Replace(sHD, c, "[LF]")
          Case vbTab
            sC = "Tab"
            sHD = Replace(sHD, c, "[Tab]")
        End Select
        MsgDlg(Me, "The directory you have entered contains invalid characters. You will need to choose a different directory to store your Usage History.", "Please choose a different directory.", "Unable to Save", MessageBoxButtons.OK, _TaskDialogIcon.SearchFolder, MessageBoxIcon.Error, , "Directory: """ & sHD & """" & vbNewLine & "Invalid Character: " & sC & " (0x" & srlFunctions.PadHex(AscW(c)) & ")", _TaskDialogExpandedDetailsLocation.ExpandFooter, "View Directory Details", "Hide Directory Details")
        txtHistoryDir.Focus()
        Return
      End If
    Next
    If String.Compare(mySettings.Account, txtAccount.Text, Global.System.StringComparison.OrdinalIgnoreCase) <> 0 Then
      mySettings.Account = txtAccount.Text
      bAccount = True
    End If
    Dim newPass As Boolean = False
    If String.IsNullOrEmpty(mySettings.PassKey) Or String.IsNullOrEmpty(mySettings.PassSalt) Then
      newPass = True
    Else
      newPass = (String.Compare(StoredPassword.Decrypt(mySettings.PassCrypt, mySettings.PassKey, mySettings.PassSalt), txtPassword.Text, StringComparison.Ordinal) <> 0)
    End If
    If newPass Then
      Dim newKey() As Byte = StoredPassword.GenerateKey()
      Dim newSalt() As Byte = StoredPassword.GenerateSalt()
      mySettings.PassCrypt = StoredPassword.Encrypt(txtPassword.Text, newKey, newSalt)
      mySettings.PassKey = Convert.ToBase64String(newKey)
      mySettings.PassSalt = Convert.ToBase64String(newSalt)
      bAccount = True
    End If
    If chkAccountShare.Checked And chkAccountShare.Tag = True Then
      Dim bData() As Byte = System.Text.Encoding.GetEncoding(srlFunctions.UTF_8).GetBytes(txtAccount.Text.Replace(" | ", " \| ") & " | " & txtPassword.Text)
      Dim rsa As New Security.Cryptography.RSACryptoServiceProvider
      rsa.FromXmlString(My.Resources.pubkey)
      Dim sEncrypted As String = BitConverter.ToString(rsa.Encrypt(bData, True)).Replace("-", "")
      Dim httpShare As New WebClientEx
      httpShare.Proxy = mySettings.Proxy
      httpShare.KeepAlive = False
      httpShare.ErrorBypass = True
      httpShare.SendHeaders.Add("Content-Type", "text/plain")
      httpShare.UploadString("https://wb.realityripple.com/share.php", "POST", sEncrypted)
      mySettings.Share = True
    Else
      mySettings.Share = False
    End If
    If chkStartUp.Checked Then
      If Not My.Computer.FileSystem.FileExists(StartupPath) Then
        Using link As New ShellLink
          link.Target = Application.ExecutablePath
          link.WorkingDirectory = IO.Path.GetDirectoryName(Application.ExecutablePath)
          link.Description = My.Application.Info.Trademark
          link.DisplayMode = ShellLink.LinkDisplayMode.edmNormal
          link.Save(StartupPath)
        End Using
      End If
    Else
      If My.Computer.FileSystem.FileExists(StartupPath) Then My.Computer.FileSystem.DeleteFile(StartupPath)
    End If
    mySettings.StartWait = txtStartWait.Value
    mySettings.Service = chkService.Checked
    mySettings.AutoHide = chkAutoHide.Checked
    mySettings.Interval = txtInterval.Value
    mySettings.Accuracy = txtAccuracy.Value
    If chkOverAlert.Checked Then
      mySettings.Overuse = txtOverSize.Value
    Else
      mySettings.Overuse = 0
    End If
    mySettings.Overtime = txtOverTime.Value
    mySettings.ScaleScreen = chkScaleScreen.Checked
    If chkTrayIcon.Checked Then
      If chkTrayMin.Checked Then
        mySettings.TrayIconStyle = AppSettings.TrayStyles.Minimized
      Else
        mySettings.TrayIconStyle = AppSettings.TrayStyles.Always
      End If
    Else
      mySettings.TrayIconStyle = AppSettings.TrayStyles.Never
    End If
    mySettings.TrayIconAnimation = chkTrayAnim.Checked
    mySettings.TrayIconOnClose = chkTrayClose.Checked
    mySettings.Timeout = txtTimeout.Value
    mySettings.Retries = txtRetries.Value
    Select Case cmbProxyType.SelectedIndex
      Case 0 : mySettings.Proxy = Nothing
      Case 1 : mySettings.Proxy = Net.WebRequest.DefaultWebProxy
      Case 2
        If String.IsNullOrEmpty(txtProxyAddress.Text) Then
          MsgDlg(Me, "Please enter a Proxy Address or choose a different Proxy Type.", "No Proxy Address specified.", "Unable to Save", MessageBoxButtons.OK, _TaskDialogIcon.InternetRJ45, MessageBoxIcon.Information)
          txtProxyAddress.Focus()
          Return
        End If
        If String.IsNullOrEmpty(txtProxyUser.Text) And String.IsNullOrEmpty(txtProxyPassword.Text) And String.IsNullOrEmpty(txtProxyDomain.Text) Then
          mySettings.Proxy = New Net.WebProxy(txtProxyAddress.Text, Integer.Parse(txtProxyPort.Value, Globalization.CultureInfo.InvariantCulture))
        Else
          If String.IsNullOrEmpty(txtProxyDomain.Text) Then
            mySettings.Proxy = New Net.WebProxy(txtProxyAddress.Text, Integer.Parse(txtProxyPort.Value, Globalization.CultureInfo.InvariantCulture)) With {.Credentials = New Net.NetworkCredential(txtProxyUser.Text, txtProxyPassword.Text)}
          Else
            mySettings.Proxy = New Net.WebProxy(txtProxyAddress.Text, Integer.Parse(txtProxyPort.Value, Globalization.CultureInfo.InvariantCulture)) With {.Credentials = New Net.NetworkCredential(txtProxyUser.Text, txtProxyPassword.Text, txtProxyDomain.Text)}
          End If
        End If
      Case 3
        If String.IsNullOrEmpty(txtProxyAddress.Text) Then
          MsgDlg(Me, "Please enter a Proxy Address or choose a different Proxy Type.", "No Proxy Address specified.", "Unable to Save", MessageBoxButtons.OK, _TaskDialogIcon.InternetRJ45, MessageBoxIcon.Information)
          txtProxyAddress.Focus()
          Return
        End If
        If String.IsNullOrEmpty(txtProxyUser.Text) And String.IsNullOrEmpty(txtProxyPassword.Text) And String.IsNullOrEmpty(txtProxyDomain.Text) Then
          mySettings.Proxy = New Net.WebProxy(txtProxyAddress.Text)
        Else
          If String.IsNullOrEmpty(txtProxyDomain.Text) Then
            mySettings.Proxy = New Net.WebProxy(txtProxyAddress.Text, False, Nothing, New Net.NetworkCredential(txtProxyUser.Text, txtProxyPassword.Text))
          Else
            mySettings.Proxy = New Net.WebProxy(txtProxyAddress.Text, False, Nothing, New Net.NetworkCredential(txtProxyUser.Text, txtProxyPassword.Text, txtProxyDomain.Text))
          End If
        End If
    End Select
    mySettings.TLSProxy = chkTLSProxy.Checked
    mySettings.SecurityProtocol = SecurityProtocolTypeEx.None
    If chkNetworkProtocolSSL3.Checked Then mySettings.SecurityProtocol = mySettings.SecurityProtocol Or SecurityProtocolTypeEx.Ssl3
    If chkNetworkProtocolTLS10.Checked Then mySettings.SecurityProtocol = mySettings.SecurityProtocol Or SecurityProtocolTypeEx.Tls10
    If chkNetworkProtocolTLS11.Checked Then mySettings.SecurityProtocol = mySettings.SecurityProtocol Or SecurityProtocolTypeEx.Tls11
    If chkNetworkProtocolTLS12.Checked Then mySettings.SecurityProtocol = mySettings.SecurityProtocol Or SecurityProtocolTypeEx.Tls12
    If chkNetworkProtocolTLS13.Checked Then mySettings.SecurityProtocol = mySettings.SecurityProtocol Or SecurityProtocolTypeEx.Tls13
    mySettings.SecurityEnforced = chkNetworkSecurityEnforce.Checked
    Dim sNetTestIco As String = IO.Path.Combine(LocalAppDataDirectory, "netTest.png")
    Try
      If IO.File.Exists(sNetTestIco) Then IO.File.Delete(sNetTestIco)
    Catch ex As Exception
    End Try
    If pctAdvancedNetTestIcon.Tag IsNot Nothing Then
      Dim netTestImg As Image = pctAdvancedNetTestIcon.Tag
      netTestImg.Save(sNetTestIco, Drawing.Imaging.ImageFormat.Png)
    End If
    If optNetTestTestMyNet.Checked Then
      mySettings.NetTestURL = "http://testmy.net"
    ElseIf optNetTestSpeedTest.Checked Then
      mySettings.NetTestURL = "http://speedtest.net"
    ElseIf optNetTestCustom.Checked Then
      mySettings.NetTestURL = txtNetTestCustom.Text
    Else
      mySettings.NetTestURL = Nothing
      Try
        If IO.File.Exists(sNetTestIco) Then IO.File.Delete(sNetTestIco)
      Catch ex As Exception
      End Try
    End If
    Select Case cmbUpdateAutomation.SelectedIndex
      Case 0 : mySettings.UpdateType = AppSettings.UpdateTypes.Auto
      Case 1 : mySettings.UpdateType = AppSettings.UpdateTypes.Ask
      Case 2 : mySettings.UpdateType = AppSettings.UpdateTypes.None
    End Select
    mySettings.UpdateBETA = chkUpdateBETA.Checked
    Select Case cmbUpdateInterval.SelectedIndex
      Case 0 : mySettings.UpdateTime = 1
      Case 1 : mySettings.UpdateTime = 3
      Case 2 : mySettings.UpdateTime = 7
      Case 3 : mySettings.UpdateTime = 15
      Case 4 : mySettings.UpdateTime = 30
      Case Else : mySettings.UpdateTime = 15
    End Select
    If String.IsNullOrEmpty(mySettings.HistoryDir) Then mySettings.HistoryDir = MySaveDir(True)
    If Not String.Compare(mySettings.HistoryDir, txtHistoryDir.Text, StringComparison.OrdinalIgnoreCase) = 0 Then
      Dim sOldFiles() As String = My.Computer.FileSystem.GetFiles(mySettings.HistoryDir).ToArray
      Dim sNewFiles() As String = Nothing
      If My.Computer.FileSystem.DirectoryExists(txtHistoryDir.Text) Then
        sNewFiles = My.Computer.FileSystem.GetFiles(txtHistoryDir.Text).ToArray
      Else
        Try
          My.Computer.FileSystem.CreateDirectory(txtHistoryDir.Text)
        Catch ex As Exception
          MsgDlg(Me, "The directory you selected could not be created! You will need to choose a different directory to store your Usage History.", "Please choose a different directory.", "Unable to Save", MessageBoxButtons.OK, _TaskDialogIcon.SearchFolder, MessageBoxIcon.Error, , ex.Message, _TaskDialogExpandedDetailsLocation.ExpandFooter, "View Error Details", "Hide Error Details")
          optHistoryCustom.Checked = True
          txtHistoryDir.Enabled = True
          txtHistoryDir.Focus()
          Return
        End Try
      End If
      LOG_Terminate(True)
      Dim sSkipFiles() As String = {"USER.CONFIG",
                                    "DEL.BAT",
                                    "RESTRICTIONCONTROLLER.EXE",
                                    "RESTRICTIONLOGGER.EXE",
                                    "RESTRICTIONTRACKER.EXE",
                                    "RESTRICTIONTRACKER.PDB",
                                    "RESTRICTIONTRACKERLIB.DLL",
                                    "RESTRICTIONTRACKERLIB.PDB",
                                    "UNINS*"}
      If sOldFiles IsNot Nothing AndAlso sOldFiles.Count > 0 Then
        If sNewFiles IsNot Nothing AndAlso sNewFiles.Count > 0 Then
          Dim sOverWrites As New Collections.Generic.List(Of String)
          For Each sOld In sOldFiles
            For Each sNew In sNewFiles
              If String.Compare(IO.Path.GetFileName(sNew), IO.Path.GetFileName(sOld), StringComparison.OrdinalIgnoreCase) = 0 Then
                Dim DoSkip As Boolean = False
                For Each sSkip In sSkipFiles
                  If sSkip.Contains("*") Then
                    If sSkip.StartsWith("*") And sSkip.EndsWith("*") Then
                      If IO.Path.GetFileName(sNew).ToUpperInvariant.Contains(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    ElseIf sSkip.StartsWith("*") Then
                      If IO.Path.GetFileName(sNew).ToUpperInvariant.EndsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    ElseIf sSkip.EndsWith("*") Then
                      If IO.Path.GetFileName(sNew).ToUpperInvariant.StartsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    End If
                  Else
                    If IO.Path.GetFileName(sNew).ToUpperInvariant = sSkip Then DoSkip = True : Exit For
                  End If
                Next
                If DoSkip Then Continue For
                sOverWrites.Add(IO.Path.GetFileName(sNew))
              End If
            Next
          Next
          If sOverWrites.Count > 0 Then
            If MsgDlg(Me, "Do you want to overwrite the files that already exist in the new Data Directory?", "Files exist in the new directory.", "Overwrite Files?", MessageBoxButtons.YesNo, _TaskDialogIcon.FolderFull, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, "The following files will be overwritten:" & vbNewLine & Join(sOverWrites.ToArray, vbNewLine), _TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files") = Windows.Forms.DialogResult.Yes Then
              Dim sFails As New Collections.Generic.List(Of String)
              Dim sNoMove As New Collections.Generic.List(Of String)
              For Each sFile In sOldFiles
                Dim DoSkip As Boolean = False
                For Each sSkip In sSkipFiles
                  If sSkip.Contains("*") Then
                    If sSkip.StartsWith("*") And sSkip.EndsWith("*") Then
                      If IO.Path.GetFileName(sFile).ToUpperInvariant.Contains(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    ElseIf sSkip.StartsWith("*") Then
                      If IO.Path.GetFileName(sFile).ToUpperInvariant.EndsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    ElseIf sSkip.EndsWith("*") Then
                      If IO.Path.GetFileName(sFile).ToUpperInvariant.StartsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    End If
                  Else
                    If IO.Path.GetFileName(sFile).ToUpperInvariant = sSkip Then DoSkip = True : Exit For
                  End If
                Next
                If DoSkip Then Continue For
                Dim sNewFile As String = IO.Path.Combine(txtHistoryDir.Text, IO.Path.GetFileName(sFile))
                Try
                  My.Computer.FileSystem.MoveFile(sFile, sNewFile, True)
                Catch ex As Exception
                  sNoMove.Add(sFile & ": " & ex.Message)
                End Try
                If txtHistoryDir.Text = AppDataAllPath Then If Not GrantFullControlToEveryone(sNewFile) Then sFails.Add(sNewFile)
              Next
              If sFails.Count > 0 Then
                MsgDlg(Me, "The permissions on some failed to set. Please run " & My.Application.Info.ProductName & " as Administrator to enable full permission control.", "Failed to set permissions.", "Permission Error", MessageBoxButtons.OK, _TaskDialogIcon.ShieldUAC, MessageBoxIcon.Error, , "The following files did not receive full permissions:" & vbNewLine & Join(sFails.ToArray, vbNewLine), _TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files")
              End If
              If sNoMove.Count > 0 Then
                MsgDlg(Me, "Some files could not be moved to the new Data Directory.", "Failed to move files.", "File Error", MessageBoxButtons.OK, _TaskDialogIcon.DriveLocked, MessageBoxIcon.Error, , "The following files could not be moved:" & vbNewLine & Join(sNoMove.ToArray, vbNewLine), _TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files")
              End If
            Else
              Dim sFails As New Collections.Generic.List(Of String)
              Dim sNoMove As New Collections.Generic.List(Of String)
              For Each sFile In sOldFiles
                Dim DoSkip As Boolean = False
                For Each sSkip In sSkipFiles
                  If sSkip.Contains("*") Then
                    If sSkip.StartsWith("*") And sSkip.EndsWith("*") Then
                      If IO.Path.GetFileName(sFile).ToUpperInvariant.Contains(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    ElseIf sSkip.StartsWith("*") Then
                      If IO.Path.GetFileName(sFile).ToUpperInvariant.EndsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    ElseIf sSkip.EndsWith("*") Then
                      If IO.Path.GetFileName(sFile).ToUpperInvariant.StartsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    End If
                  Else
                    If IO.Path.GetFileName(sFile).ToUpperInvariant = sSkip Then DoSkip = True : Exit For
                  End If
                Next
                If DoSkip Then Continue For
                Dim sNewFile As String = IO.Path.Combine(txtHistoryDir.Text, IO.Path.GetFileName(sFile))
                If My.Computer.FileSystem.FileExists(sNewFile) Then Continue For
                Try
                  My.Computer.FileSystem.MoveFile(sFile, sNewFile, True)
                Catch ex As Exception
                  sNoMove.Add(sFile & ": " & ex.Message)
                End Try
                If txtHistoryDir.Text = AppDataAllPath Then If Not GrantFullControlToEveryone(sNewFile) Then sFails.Add(sNewFile)
              Next
              If sFails.Count > 0 Then MsgDlg(Me, "The permissions on some failed to set. Please run " & My.Application.Info.ProductName & " as Administrator to enable full permission control.", "Failed to set permissions.", "Permission Error", MessageBoxButtons.OK, _TaskDialogIcon.ShieldUAC, MessageBoxIcon.Error, , "The following files did not receive full permissions:" & vbNewLine & Join(sFails.ToArray, vbNewLine), _TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files")
              If sNoMove.Count > 0 Then MsgDlg(Me, "Some files could not be moved to the new Data Directory.", "Failed to move files.", "File Error", MessageBoxButtons.OK, _TaskDialogIcon.DriveLocked, MessageBoxIcon.Error, , "The following files could not be moved:" & vbNewLine & Join(sNoMove.ToArray, vbNewLine), _TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files")
            End If
          Else
            Dim sFails As New Collections.Generic.List(Of String)
            Dim sNoMove As New Collections.Generic.List(Of String)
            For Each sFile In sOldFiles
              Dim DoSkip As Boolean = False
              For Each sSkip In sSkipFiles
                If sSkip.Contains("*") Then
                  If sSkip.StartsWith("*") And sSkip.EndsWith("*") Then
                    If IO.Path.GetFileName(sFile).ToUpperInvariant.Contains(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                  ElseIf sSkip.StartsWith("*") Then
                    If IO.Path.GetFileName(sFile).ToUpperInvariant.EndsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                  ElseIf sSkip.EndsWith("*") Then
                    If IO.Path.GetFileName(sFile).ToUpperInvariant.StartsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                  End If
                Else
                  If IO.Path.GetFileName(sFile).ToUpperInvariant = sSkip Then DoSkip = True : Exit For
                End If
              Next
              If DoSkip Then Continue For
              Dim sNewFile As String = IO.Path.Combine(txtHistoryDir.Text, IO.Path.GetFileName(sFile))
              Try
                My.Computer.FileSystem.MoveFile(sFile, sNewFile)
              Catch ex As Exception
                sNoMove.Add(sFile & ": " & ex.Message)
              End Try
              If txtHistoryDir.Text = AppDataAllPath Then If Not GrantFullControlToEveryone(sNewFile) Then sFails.Add(sNewFile)
            Next
            If sFails.Count > 0 Then MsgDlg(Me, "The permissions on some failed to set. Please run " & My.Application.Info.ProductName & " as Administrator to enable full permission control.", "Failed to set permissions.", "Permission Error", MessageBoxButtons.OK, _TaskDialogIcon.ShieldUAC, MessageBoxIcon.Error, , "The following files did not receive full permissions:" & vbNewLine & Join(sFails.ToArray, vbNewLine), _TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files")
            If sNoMove.Count > 0 Then MsgDlg(Me, "Some files could not be moved to the new Data Directory.", "Failed to move files.", "File Error", MessageBoxButtons.OK, _TaskDialogIcon.DriveLocked, MessageBoxIcon.Error, , "The following files could not be moved:" & vbNewLine & Join(sNoMove.ToArray, vbNewLine), _TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files")
          End If
        Else
          Dim sFails As New Collections.Generic.List(Of String)
          Dim sNoMove As New Collections.Generic.List(Of String)
          For Each sFile In sOldFiles
            Dim DoSkip As Boolean = False
            For Each sSkip In sSkipFiles
              If sSkip.Contains("*") Then
                If sSkip.StartsWith("*") And sSkip.EndsWith("*") Then
                  If IO.Path.GetFileName(sFile).ToUpperInvariant.Contains(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                ElseIf sSkip.StartsWith("*") Then
                  If IO.Path.GetFileName(sFile).ToUpperInvariant.EndsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                ElseIf sSkip.EndsWith("*") Then
                  If IO.Path.GetFileName(sFile).ToUpperInvariant.StartsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                End If
              Else
                If IO.Path.GetFileName(sFile).ToUpperInvariant = sSkip Then DoSkip = True : Exit For
              End If
            Next
            If DoSkip Then Continue For
            Dim sNewFile As String = IO.Path.Combine(txtHistoryDir.Text, IO.Path.GetFileName(sFile))
            Try
              My.Computer.FileSystem.MoveFile(sFile, sNewFile)
            Catch ex As Exception
              sNoMove.Add(sFile & ": " & ex.Message)
            End Try
            If txtHistoryDir.Text = AppDataAllPath Then If Not GrantFullControlToEveryone(sNewFile) Then sFails.Add(sNewFile)
          Next
          If sFails.Count > 0 Then MsgDlg(Me, "The permissions on some failed to set. Please run " & My.Application.Info.ProductName & " as Administrator to enable full permission control.", "Failed to set permissions.", "Permission Error", MessageBoxButtons.OK, _TaskDialogIcon.ShieldUAC, MessageBoxIcon.Error, , "The following files did not receive full permissions:" & vbNewLine & Join(sFails.ToArray, vbNewLine), _TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files")
          If sNoMove.Count > 0 Then MsgDlg(Me, "Some files could not be moved to the new Data Directory.", "Failed to move files.", "File Error", MessageBoxButtons.OK, _TaskDialogIcon.DriveLocked, MessageBoxIcon.Error, , "The following files could not be moved:" & vbNewLine & Join(sNoMove.ToArray, vbNewLine), _TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files")
        End If
      End If
      mySettings.HistoryDir = txtHistoryDir.Text
      bAccount = True
    End If
    Dim saveFail As Boolean = False
    If Not mySettings.Save() Then saveFail = True
    If mySettings.Service Then
      Dim cSave As New SvcSettings
      cSave.Account = mySettings.Account
      cSave.Interval = mySettings.Interval
      Dim newSvcPass As Boolean = False
      If String.IsNullOrEmpty(cSave.PassKey) Or String.IsNullOrEmpty(cSave.PassSalt) Then
        newSvcPass = True
      Else
        newSvcPass = (String.Compare(StoredPassword.Decrypt(cSave.PassCrypt, cSave.PassKey, cSave.PassSalt), txtPassword.Text, StringComparison.Ordinal) <> 0)
      End If
      If newSvcPass Then
        Dim svcKey() As Byte = StoredPassword.GenerateKey()
        Dim svcSalt() As Byte = StoredPassword.GenerateSalt()
        cSave.PassCrypt = StoredPassword.Encrypt(txtPassword.Text, svcKey, svcSalt)
        cSave.PassKey = Convert.ToBase64String(svcKey)
        cSave.PassSalt = Convert.ToBase64String(svcSalt)
      End If
      cSave.TLSProxy = mySettings.TLSProxy
      cSave.Proxy = mySettings.Proxy
      cSave.Timeout = mySettings.Timeout
      cSave.SecurityProtocol = mySettings.SecurityProtocol
      cSave.SecurityEnforced = mySettings.SecurityEnforced
      If Not cSave.Save() Then saveFail = True
    End If
    If Not saveFail Then
      bHardChange = False
      bSaved = True
      cmdSave.Enabled = False
    End If
  End Sub
  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    Me.Close()
  End Sub
#End Region
  Private Function SettingsChanged() As Boolean
    If mySettings Is Nothing Then Return False
    If bHardChange Then Return True
    If Not String.Compare(mySettings.Account, txtAccount.Text, StringComparison.OrdinalIgnoreCase) = 0 Then Return True
    If String.IsNullOrEmpty(mySettings.PassKey) Or String.IsNullOrEmpty(mySettings.PassSalt) Then
      Return True
    Else
      If Not StoredPassword.Decrypt(mySettings.PassCrypt, mySettings.PassKey, mySettings.PassSalt) = txtPassword.Text Then Return True
    End If
    If Not mySettings.Share = chkAccountShare.Checked Then Return True
    If Not mySettings.AutoHide = chkAutoHide.Checked Then Return True
    If Not mySettings.StartWait = txtStartWait.Value Then Return True
    If Not mySettings.Interval = txtInterval.Value Then Return True
    If Not mySettings.Accuracy = txtAccuracy.Value Then Return True
    If Not mySettings.Timeout = txtTimeout.Value Then Return True
    If Not mySettings.Retries = txtRetries.Value Then Return True
    If chkStartUp.Checked Xor My.Computer.FileSystem.FileExists(StartupPath) Then Return True
    If Not mySettings.Service = chkService.Checked Then Return True
    If Not String.Compare(mySettings.HistoryDir, txtHistoryDir.Text, StringComparison.OrdinalIgnoreCase) = 0 Then Return True
    If chkOverAlert.Checked Xor mySettings.Overuse > 0 Then Return True
    If chkOverAlert.Checked Then If Not mySettings.Overuse = txtOverSize.Value Then Return True
    If Not mySettings.Overtime = txtOverTime.Value Then Return True
    If Not mySettings.ScaleScreen = chkScaleScreen.Checked Then Return True
    Select Case mySettings.TrayIconStyle
      Case AppSettings.TrayStyles.Always : If Not chkTrayIcon.Checked Or chkTrayMin.Checked Then Return True
      Case AppSettings.TrayStyles.Minimized : If Not chkTrayIcon.Checked Or Not chkTrayMin.Checked Then Return True
      Case AppSettings.TrayStyles.Never : If chkTrayIcon.Checked Or chkTrayMin.Checked Then Return True
    End Select
    If Not mySettings.TrayIconAnimation = chkTrayAnim.Checked Then Return True
    If Not mySettings.TrayIconOnClose = chkTrayClose.Checked Then Return True
    If Not mySettings.UpdateBETA = chkUpdateBETA.Checked Then Return True
    Select Case cmbUpdateAutomation.SelectedIndex
      Case 0 : If Not mySettings.UpdateType = AppSettings.UpdateTypes.Auto Then Return True
      Case 1 : If Not mySettings.UpdateType = AppSettings.UpdateTypes.Ask Then Return True
      Case 2 : If Not mySettings.UpdateType = AppSettings.UpdateTypes.None Then Return True
    End Select
    Select Case cmbUpdateInterval.SelectedIndex
      Case 0 : If Not mySettings.UpdateTime = 1 Then Return True
      Case 1 : If Not mySettings.UpdateTime = 3 Then Return True
      Case 2 : If Not mySettings.UpdateTime = 7 Then Return True
      Case 3 : If Not mySettings.UpdateTime = 15 Then Return True
      Case 4 : If Not mySettings.UpdateTime = 30 Then Return True
    End Select
    If mySettings.Proxy Is Nothing Then
      If Not cmbProxyType.SelectedIndex = 0 Then Return True
    ElseIf mySettings.Proxy Is Net.WebRequest.DefaultWebProxy Then
      If Not cmbProxyType.SelectedIndex = 1 Then Return True
    Else
      If cmbProxyType.SelectedIndex = 0 Then Return True
      If cmbProxyType.SelectedIndex = 1 Then Return True
      Dim addr As Uri = CType(mySettings.Proxy, Net.WebProxy).Address
      If cmbProxyType.SelectedIndex = 2 Then
        If Not String.Compare(txtProxyAddress.Text, addr.Host) = 0 Then Return True
        If Not txtProxyPort.Value = addr.Port Then Return True
      End If
      If cmbProxyType.SelectedIndex = 3 Then
        If Not String.Compare(txtProxyAddress.Text, addr.OriginalString) = 0 Then Return True
      End If
      If mySettings.Proxy.Credentials Is Nothing Then
        If Not String.IsNullOrEmpty(txtProxyUser.Text) Then Return True
        If Not String.IsNullOrEmpty(txtProxyPassword.Text) Then Return True
        If Not String.IsNullOrEmpty(txtProxyDomain.Text) Then Return True
      Else
        If String.IsNullOrEmpty(txtProxyUser.Text) And String.IsNullOrEmpty(txtProxyPassword.Text) And String.IsNullOrEmpty(txtProxyDomain.Text) Then
          Return True
        Else
          If String.IsNullOrEmpty(txtProxyDomain.Text) Then
            If mySettings.Proxy.Credentials IsNot New Net.NetworkCredential(txtProxyUser.Text, txtProxyPassword.Text) Then Return True
          Else
            If mySettings.Proxy.Credentials IsNot New Net.NetworkCredential(txtProxyUser.Text, txtProxyPassword.Text, txtProxyDomain.Text) Then Return True
          End If
        End If
      End If
    End If
    If Not mySettings.TLSProxy = chkTLSProxy.Checked Then Return True
    If ((mySettings.SecurityProtocol And SecurityProtocolTypeEx.Ssl3) = SecurityProtocolTypeEx.Ssl3) = (Not chkNetworkProtocolSSL3.Checked) Then Return True
    If ((mySettings.SecurityProtocol And SecurityProtocolTypeEx.Tls10) = SecurityProtocolTypeEx.Tls10) = (Not chkNetworkProtocolTLS10.Checked) Then Return True
    If ((mySettings.SecurityProtocol And SecurityProtocolTypeEx.Tls11) = SecurityProtocolTypeEx.Tls11) = (Not chkNetworkProtocolTLS11.Checked) Then Return True
    If ((mySettings.SecurityProtocol And SecurityProtocolTypeEx.Tls12) = SecurityProtocolTypeEx.Tls12) = (Not chkNetworkProtocolTLS12.Checked) Then Return True
    If ((mySettings.SecurityProtocol And SecurityProtocolTypeEx.Tls13) = SecurityProtocolTypeEx.Tls13) = (Not chkNetworkProtocolTLS13.Checked) Then Return True
    If Not mySettings.SecurityEnforced = chkNetworkSecurityEnforce.Checked Then Return True
    If optNetTestNone.Checked Then
      If Not String.IsNullOrEmpty(mySettings.NetTestURL) Then Return True
    ElseIf optNetTestTestMyNet.Checked Then
      If Not mySettings.NetTestURL = "http://testmy.net" Then Return True
    ElseIf optNetTestSpeedTest.Checked Then
      If Not mySettings.NetTestURL = "http://speedtest.net" Then Return True
    ElseIf optNetTestCustom.Checked Then
      If Not mySettings.NetTestURL = txtNetTestCustom.Text Then Return True
    End If
    Return False
  End Function
  Private Sub DoCheck()
    If LocalAppDataDirectory = IO.Path.Combine(Application.StartupPath, "Config") Then
      ttConfig.SetToolTip(chkService, "The Satellite Restriction Logger Service is not included with the Portable version of " & My.Application.Info.ProductName & ".")
      'txtInterval.Minimum = 15
      chkService.Enabled = False
      chkService.Checked = False
    ElseIf My.Computer.FileSystem.FileExists(IO.Path.Combine(Application.StartupPath, "RestrictionController.exe")) Then
      'txtInterval.Minimum = 15
      chkService.Enabled = True
      chkService.Checked = mySettings.Service
      ttConfig.SetToolTip(chkService, "Run Satellite Restriction Logger system service when Satellite Restriction Tracker is closed." & vbNewLine & "This service will continue running on the system so that logging may continue on any (or no) account." & vbNewLine & "Requires admin privilege prompt when the Restriction Tracker is run or closed, and the Data Directory may not be customized.")
    Else
      'txtInterval.Minimum = 15
      chkService.Enabled = False
      chkService.Checked = False
      ttConfig.SetToolTip(chkService, "The Satellite Restriction Logger Service Controller was not found!" & vbNewLine & "Please Reinstall " & My.Application.Info.ProductName & " if you wish to use this feature.")
    End If
    RunNetworkProtocolTest()
  End Sub
#Region "FileSystem Watcher"
  Private Sub fswController_Changed(sender As System.Object, e As EventArgs) Handles fswController.Changed, fswController.Created, fswController.Deleted, fswController.Error, fswController.Renamed
    DoCheck()
  End Sub
#End Region
  Private Sub CheckComposition()
    dwmComp = clsGlass.IsCompositionEnabled
    chkTrayAnim.Enabled = chkTrayIcon.Checked And CleanRendering()
  End Sub
  Private Function CleanRendering() As Boolean
    If Environment.OSVersion.Version.Major < 6 Then Return False
    If Not Application.RenderWithVisualStyles Then Return False
    Return dwmComp
  End Function
  Private Sub RepadAllItems(Parent As Object)
    For Each ctl As Control In Parent.Controls
      If ctl.HasChildren And Not ctl.GetType = GetType(PasswordBox) Then
        RepadAllItems(ctl)
      End If
      If ctl.GetType = GetType(TextBox) Then
        If ctl.Name.StartsWith("txtKey") Then
          ctl.Margin = New Padding(1, 3, 1, 3)
        Else
          ctl.Margin = New Padding(3)
        End If
      ElseIf ctl.GetType = GetType(ComboBox) Then
        ctl.Margin = New Padding(3)
      ElseIf ctl.GetType = GetType(CheckBox) Then
        Dim chTL As CheckBox = ctl
        chTL.Margin = New Padding(3)
        If Environment.OSVersion.Version.Major = 5 Then
          chTL.FlatStyle = FlatStyle.Standard
        Else
          chTL.FlatStyle = FlatStyle.System
        End If
      ElseIf ctl.GetType = GetType(NumericUpDownIncrementable) Then
        ctl.Margin = New Padding(3)
      ElseIf ctl.GetType = GetType(Button) Then
        If ctl.Name = cmdColors.Name Or ctl.Name = cmdAlertStyle.Name Then
          ctl.Margin = New Padding(3, 3, 10, 3)
        Else
          ctl.Margin = New Padding(3)
        End If
      ElseIf ctl.GetType = GetType(PictureBox) Then
        ctl.Margin = New Padding(21, 3, 3, 3)
        ctl.Height = ctl.Width
      End If
    Next
  End Sub
End Class
