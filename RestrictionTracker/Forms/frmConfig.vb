﻿Public Class frmConfig
  Private WithEvents remoteTest As remoteRestrictionTracker
  Private WithEvents wsHostList As WebClientEx
  Private WithEvents wsFavicon As clsFavicon
  Private bSaved, bAccount, bLoaded, bHardChange As Boolean
  Private mySettings As AppSettings
  Private pChecker As Threading.Timer
  Private Const LINK_PURCHASE As String = "Purchase a Remote Usage Service Subscription"
  Private Const LINK_PURCHASE_TT As String = "If you do not have a Product Key for the Remote Usage Service, you can purchase one online for as little as $15.00 a year."
  Private Const LINK_PANEL As String = "Visit the Remote Usage Service User Panel Page"
  Private Const LINK_PANEL_TT As String = "Manage your Remote Usage Service account online, chat with other users, and contact support directly, all from one page."
#Region "Form Events"
  Private Sub frmConfig_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    bLoaded = False
    mySettings = New AppSettings
    If AppDataPath = Application.StartupPath & "\Config\" Then mySettings.HistoryDir = Application.StartupPath & "\Config\"
    RepadAllItems(Me)
    Dim sAccount As String = mySettings.Account
    Dim sUsername, sProvider As String
    If Not String.IsNullOrEmpty(sAccount) AndAlso (sAccount.Contains("@") And sAccount.Contains(".")) Then
      sUsername = sAccount.Substring(0, sAccount.LastIndexOf("@"))
      sProvider = sAccount.Substring(sAccount.LastIndexOf("@") + 1)
    Else
      sUsername = sAccount
      sProvider = ""
    End If
    txtAccount.Text = sUsername
    UseDefaultHostList()
    cmbProvider.Text = sProvider
    ttConfig.SetToolTip(txtPassword.Button, "Toggle display of the Password.")
    If Not String.IsNullOrEmpty(mySettings.PassCrypt) Then
      txtPassword.Text = StoredPassword.DecryptApp(mySettings.PassCrypt)
    End If
    Select Case mySettings.AccountType
      Case localRestrictionTracker.SatHostTypes.WildBlue_LEGACY : optAccountTypeWBL.Checked = True
      Case localRestrictionTracker.SatHostTypes.WildBlue_EXEDE : optAccountTypeWBX.Checked = True
      Case localRestrictionTracker.SatHostTypes.DishNet_EXEDE : optAccountTypeDNX.Checked = True
      Case localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY : optAccountTypeRPL.Checked = True
      Case localRestrictionTracker.SatHostTypes.RuralPortal_EXEDE : optAccountTypeRPX.Checked = True
    End Select
    chkAccountTypeAuto.Checked = Not mySettings.AccountTypeForced
    Dim sKey As String = mySettings.RemoteKey
    If sKey.Contains("-") Then
      Dim sKeys() As String = Split(sKey, "-")
      If sKeys.Length = 5 Then
        txtKey1.Text = Trim(sKeys(0))
        txtKey2.Text = Trim(sKeys(1))
        txtKey3.Text = Trim(sKeys(2))
        txtKey4.Text = Trim(sKeys(3))
        txtKey5.Text = Trim(sKeys(4))
      Else
        txtKey1.Text = Nothing
        txtKey2.Text = Nothing
        txtKey3.Text = Nothing
        txtKey4.Text = Nothing
        txtKey5.Text = Nothing
      End If
    Else
      txtKey1.Text = Nothing
      txtKey2.Text = Nothing
      txtKey3.Text = Nothing
      txtKey4.Text = Nothing
      txtKey5.Text = Nothing
    End If
    If txtKey1.TextLength < 6 Or txtKey2.TextLength < 4 Or txtKey3.TextLength < 4 Or txtKey4.TextLength < 4 Or txtKey5.TextLength < 6 Then
      pctKeyState.Tag = 0
      pctKeyState.Image = Nothing
      ttConfig.SetToolTip(pctKeyState, String.Empty)
      lblPurchaseKey.Text = LINK_PURCHASE
      ttConfig.SetToolTip(lblPurchaseKey, LINK_PURCHASE_TT)
    Else
      pctKeyState.Tag = 1
      pctKeyState.Image = My.Resources.ico_ok
      ttConfig.SetToolTip(pctKeyState, "Thank you for purchasing the Remote Usage Service for " & My.Application.Info.ProductName & "!")
      lblPurchaseKey.Text = LINK_PANEL
      ttConfig.SetToolTip(lblPurchaseKey, LINK_PANEL_TT)
    End If
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
    chkNetworkProtocolSSL.Checked = mySettings.SecurityProtocol = Net.SecurityProtocolType.Ssl3
    If String.IsNullOrEmpty(mySettings.NetTestURL) Then
      optNetTestNone.Checked = True
    Else
      Select Case mySettings.NetTestURL
        Case "http://testmy.net" : optNetTestTestMyNet.Checked = True
        Case "http://speedtest.net" : optNetTestSpeedTest.Checked = True
        Case Else
          optNetTestCustom.Checked = True
          txtNetTestCustom.Text = mySettings.NetTestURL
          wsFavicon = New clsFavicon(txtNetTestCustom.Text)
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
    Dim aD As String = AppDataPath
    If Not aD.EndsWith(IO.Path.DirectorySeparatorChar) Then aD &= IO.Path.DirectorySeparatorChar
    Dim hD As String = mySettings.HistoryDir
    If String.IsNullOrEmpty(hD) Then hD = AppDataPath
    If Not hD.EndsWith(IO.Path.DirectorySeparatorChar) Then hD &= IO.Path.DirectorySeparatorChar
    If chkService.Checked Then
      optHistoryProgramData.Checked = False
      DisableHistory = True
    ElseIf String.Compare(aD, Application.StartupPath & "\Config\", True) = 0 Then
      optHistoryProgramData.Checked = False
      optHistoryAppData.Checked = False
      optHistoryCustom.Checked = False
      mySettings.HistoryDir = Application.StartupPath & "\Config\"
      DisableHistory = True
      lblPortableDir.Enabled = False
      txtPortableDir.Enabled = False
      cmdPortableDir.Enabled = False
      cmdMakePortable.Enabled = False
      tbsConfig.TabPages.Remove(tabAdvanced)
    ElseIf String.Compare(hD, AppDataAllPath, True) = 0 Then
      optHistoryProgramData.Checked = True
    ElseIf String.Compare(hD, AppDataPath, True) = 0 Then
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
    Dim popInvoker As New MethodInvoker(AddressOf PopulateHostList)
    popInvoker.BeginInvoke(Nothing, Nothing)
  End Sub
  Private Sub RunAccountTest(sKey As String)
    If Me.InvokeRequired Then
      Me.BeginInvoke(New Threading.ContextCallback(AddressOf RunAccountTest), sKey)
    Else
      If pChecker IsNot Nothing Then
        pChecker.Dispose()
        pChecker = Nothing
      Else
        Exit Sub
      End If
      remoteTest = New remoteRestrictionTracker(txtAccount.Text & "@" & cmbProvider.Text, String.Empty, sKey, mySettings.Proxy, mySettings.Timeout, New Date(2000, 1, 1), AppData)
    End If
  End Sub
  Private Sub frmConfig_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If pChecker IsNot Nothing Then
      pChecker.Dispose()
      pChecker = Nothing
    End If
    If remoteTest IsNot Nothing Then
      remoteTest.Dispose()
      remoteTest = Nothing
    End If
    If e.CloseReason = CloseReason.ApplicationExitCall Then
      Me.DialogResult = Windows.Forms.DialogResult.Abort
    Else
      If bSaved Then
        If bAccount Then
          Me.DialogResult = Windows.Forms.DialogResult.Yes
        Else
          Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
      ElseIf cmdSave.Enabled Then
        Dim saveRet As DialogResult = MsgDlg(Me, "Do you want to save the changes to your configuration?", "Your changes have not been saved.", "Save Changes?", MessageBoxButtons.YesNoCancel, TaskDialogIcon.Options, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        'Dim saveRet As DialogResult = MessageBox.Show("Some settings have been changed but not saved." & vbNewLine & vbNewLine & "Do you want to save the changes to your configuration?", My.Application.Info.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If saveRet = Windows.Forms.DialogResult.Yes Then
          cmdSave.PerformClick()
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
          Exit Sub
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
    pnlAccountProvider.MouseMove, pnlAccountTypes.MouseMove,
    pnlAccountKey.MouseMove, pnlKey.MouseMove,
    pnlPrefs.MouseMove,
    pnlPrefStart.MouseMove, pnlPrefStartInput.MouseMove,
    pnlPrefAccuracy.MouseMove, pnlPrefAccuracyInput.MouseMove,
    pnlPrefAlert.MouseMove, pnlPrefColor.MouseMove,
    pnlNetwork.MouseMove,
    pnlNetworkTimeout.MouseMove, pnlNetworkProxy.MouseMove, pnlNetworkUpdate.MouseMove,
    pnlAdvanced.MouseMove,
    pnlAdvancedData.MouseMove, pnlAdvancedDataInput.MouseMove, pnlHistoryDir.MouseMove,
    pnlButtons.MouseMove, pnlPrefInterface.MouseMove, pnlAdvancedNetTestInput.MouseMove, pnlAdvancedNetTest.MouseMove
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
  Private Sub txtVal_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtAccount.KeyDown, txtPassword.KeyDown, cmbProvider.KeyDown
    If e.Control And e.KeyCode = Keys.V Then
      CType(sender, TextBox).SelectedText = Clipboard.GetText.Trim
      e.SuppressKeyPress = True
      e.Handled = True
    End If
  End Sub
  Private Sub ValuesChanged(sender As System.Object, e As EventArgs) Handles txtPassword.KeyPress, txtPassword.TextChanged,
                                                                             optAccountTypeWBL.CheckedChanged, optAccountTypeWBX.CheckedChanged, optAccountTypeDNX.CheckedChanged, optAccountTypeRPL.CheckedChanged, optAccountTypeRPX.CheckedChanged,
                                                                             txtStartWait.KeyPress, txtStartWait.KeyUp, txtStartWait.Scroll, txtStartWait.ValueChanged,
                                                                             txtInterval.KeyPress, txtInterval.KeyUp, txtInterval.Scroll, txtInterval.ValueChanged,
                                                                             txtAccuracy.KeyPress, txtAccuracy.KeyUp, txtAccuracy.Scroll, txtAccuracy.ValueChanged,
                                                                             txtTimeout.KeyPress, txtTimeout.KeyUp, txtTimeout.Scroll, txtTimeout.ValueChanged,
                                                                             chkStartUp.CheckedChanged, chkAutoHide.CheckedChanged,
                                                                             txtOverSize.KeyPress, txtOverSize.KeyUp, txtOverSize.Scroll, txtOverSize.ValueChanged,
                                                                             txtOverTime.KeyPress, txtOverTime.KeyUp, txtOverTime.Scroll, txtOverTime.ValueChanged,
                                                                             chkNetworkProtocolSSL.CheckedChanged,
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
  Private Sub txtAccount_ValuesChanged(sender As System.Object, e As EventArgs) Handles txtAccount.KeyPress, txtAccount.TextChanged, cmbProvider.KeyPress, cmbProvider.TextChanged, cmbProvider.SelectedIndexChanged
    If Not bLoaded Then Exit Sub
    If pChecker IsNot Nothing Then
      pctKeyState.Tag = IIf(CheckState, 1, 0)
      pChecker.Dispose()
      pChecker = Nothing
    End If
    If remoteTest IsNot Nothing Then
      pctKeyState.Tag = IIf(CheckState, 1, 0)
      remoteTest.Dispose()
      remoteTest = Nothing
    End If
    lblPurchaseKey.Text = LINK_PURCHASE
    ttConfig.SetTooltip(lblPurchaseKey, LINK_PURCHASE_TT)
    If txtKey1.TextLength < 6 Or txtKey2.TextLength < 4 Or txtKey3.TextLength < 4 Or txtKey4.TextLength < 4 Or txtKey5.TextLength < 6 Then
      cmdSave.Enabled = SettingsChanged()
    Else
      KeyCheck()
    End If
  End Sub
  Private Sub chkAccountTypeAuto_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAccountTypeAuto.CheckedChanged
    optAccountTypeWBL.Enabled = Not chkAccountTypeAuto.Checked
    optAccountTypeDNX.Enabled = Not chkAccountTypeAuto.Checked
    optAccountTypeWBX.Enabled = Not chkAccountTypeAuto.Checked
    optAccountTypeRPL.Enabled = Not chkAccountTypeAuto.Checked
    optAccountTypeRPX.Enabled = Not chkAccountTypeAuto.Checked
    cmdSave.Enabled = SettingsChanged()
  End Sub
  Private Sub txtProductKey_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtKey1.KeyDown, txtKey2.KeyDown, txtKey3.KeyDown, txtKey4.KeyDown, txtKey5.KeyDown
    If e.KeyValue = 86 And e.Control Then
      If Not String.IsNullOrEmpty(Clipboard.GetText) Then
        Dim sKey As String = Trim(Clipboard.GetText)
        If sKey.Contains("-") Then
          Dim sKeys() As String = Split(sKey, "-")
          If sKeys.Length = 5 Then
            txtKey1.Text = sKeys(0)
            txtKey2.Text = sKeys(1)
            txtKey3.Text = sKeys(2)
            txtKey4.Text = sKeys(3)
            txtKey5.Text = sKeys(4)
            e.Handled = True
          Else
            sender.Text = sKey
          End If
        Else
          sender.Text = sKey
        End If
      End If
    ElseIf e.KeyValue = 46 Or (e.KeyValue = 88 And e.Control) Or (e.KeyValue = 67 And e.Control) Or e.KeyValue = 35 Or e.KeyValue = 36 Or e.KeyValue = 37 Or e.KeyValue = 39 Or e.KeyValue = 9 Or e.KeyValue = 16 Or e.KeyValue = 17 Or e.KeyValue = 18 Then

    ElseIf e.KeyValue = 8 Then
      If String.IsNullOrEmpty(sender.text) And sender.SelectionLength = 0 Then
        Select Case sender.Name.ToString
          Case "txtKey1"
            e.SuppressKeyPress = True
            e.Handled = True
          Case "txtKey2"
            txtKey1.Focus()
            txtKey1.SelectionStart = txtKey1.Text.Length
            txtKey1.SelectionLength = 0
          Case "txtKey3"
            txtKey2.Focus()
            txtKey2.SelectionStart = txtKey2.Text.Length
            txtKey2.SelectionLength = 0
          Case "txtKey4"
            txtKey3.Focus()
            txtKey3.SelectionStart = txtKey3.Text.Length
            txtKey3.SelectionLength = 0
          Case "txtKey5"
            txtKey4.Focus()
            txtKey4.SelectionStart = txtKey4.Text.Length
            txtKey4.SelectionLength = 0
        End Select
      End If
    Else
      If sender.TextLength = sender.MaxLength And sender.SelectionLength = 0 Then
        Select Case sender.Name
          Case "txtKey1"
            txtKey2.Focus()
            SendKeys.Send(Chr(e.KeyValue))
          Case "txtKey2"
            txtKey3.Focus()
            SendKeys.Send(Chr(e.KeyValue))
          Case "txtKey3"
            txtKey4.Focus()
            SendKeys.Send(Chr(e.KeyValue))
          Case "txtKey4"
            txtKey5.Focus()
            SendKeys.Send(Chr(e.KeyValue))
        End Select
        e.SuppressKeyPress = True
        e.Handled = True
      End If
    End If
  End Sub
  Private Sub txtProductKey_TextChanged(sender As Object, e As System.EventArgs) Handles txtKey1.TextChanged, txtKey2.TextChanged, txtKey3.TextChanged, txtKey4.TextChanged, txtKey5.TextChanged
    If Not bLoaded Then Exit Sub
    If pChecker IsNot Nothing Then
      pctKeyState.Tag = IIf(CheckState, 1, 0)
      pChecker.Dispose()
      pChecker = Nothing
    End If
    If remoteTest IsNot Nothing Then
      pctKeyState.Tag = IIf(CheckState, 1, 0)
      remoteTest.Dispose()
      remoteTest = Nothing
    End If
    lblPurchaseKey.Text = LINK_PURCHASE
    ttConfig.SetTooltip(lblPurchaseKey, LINK_PURCHASE_TT)
    If txtKey1.TextLength < 6 Or txtKey2.TextLength < 4 Or txtKey3.TextLength < 4 Or txtKey4.TextLength < 4 Or txtKey5.TextLength < 6 Then
      pctKeyState.Tag = 0
      pctKeyState.Image = Nothing
      ttConfig.SetTooltip(pctKeyState, String.Empty)
      cmdSave.Enabled = SettingsChanged()
      DoCheck()
    Else
      KeyCheck()
    End If
  End Sub
  Private Sub chkService_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkService.CheckedChanged
    If chkService.Checked Then
      txtHistoryDir.Tag = txtHistoryDir.Text
      txtHistoryDir.Text = AppDataAllPath
    ElseIf Not String.IsNullOrEmpty(txtHistoryDir.Tag) Then
      txtHistoryDir.Text = txtHistoryDir.Tag
      txtHistoryDir.Tag = Nothing
    End If
    optHistoryAppData.Enabled = Not chkService.Checked
    optHistoryProgramData.Enabled = Not chkService.Checked
    optHistoryCustom.Enabled = Not chkService.Checked
    If chkService.Checked Then optHistoryProgramData.Checked = True
    txtHistoryDir.Enabled = Not chkService.Checked
    cmdHistoryDir.Enabled = Not chkService.Checked
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
    chkTrayAnim.Enabled = chkTrayIcon.Checked
    If Not chkTrayMin.Enabled Then chkTrayMin.Checked = False
    If Not chkTrayAnim.Enabled Then chkTrayAnim.Checked = False
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
      If wsFavicon IsNot Nothing Then
        wsFavicon.Dispose()
        wsFavicon = Nothing
      End If
      txtNetTestCustom.Text = ""
      txtNetTestCustom.Enabled = False
      SetNetTestImage(My.Resources.advanced_nettest_none)
      cmdSave.Enabled = SettingsChanged()
    End If
  End Sub
  Private Sub optNetTestTestMyNet_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optNetTestTestMyNet.CheckedChanged
    If optNetTestTestMyNet.Checked Then
      If wsFavicon IsNot Nothing Then
        wsFavicon.Dispose()
        wsFavicon = Nothing
      End If
      txtNetTestCustom.Text = "http://testmy.net"
      txtNetTestCustom.Enabled = False
      SetNetTestImage(pctAdvancedNetTestIcon.InitialImage)
      wsFavicon = New clsFavicon("http://testmy.net")
      cmdSave.Enabled = SettingsChanged()
    End If
  End Sub
  Private Sub optNetTestSpeedTest_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optNetTestSpeedTest.CheckedChanged
    If optNetTestSpeedTest.Checked Then
      If wsFavicon IsNot Nothing Then
        wsFavicon.Dispose()
        wsFavicon = Nothing
      End If
      txtNetTestCustom.Text = "http://speedtest.net"
      txtNetTestCustom.Enabled = False
      SetNetTestImage(pctAdvancedNetTestIcon.InitialImage)
      wsFavicon = New clsFavicon("http://speedtest.net")
      cmdSave.Enabled = SettingsChanged()
    End If
  End Sub
  Private Sub optNetTestCustom_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optNetTestCustom.CheckedChanged
    If optNetTestCustom.Checked Then
      If wsFavicon IsNot Nothing Then
        wsFavicon.Dispose()
        wsFavicon = Nothing
      End If
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
          SetNetTestImage(pctAdvancedNetTestIcon.InitialImage)
          wsFavicon = New clsFavicon(txtNetTestCustom.Text)
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
      pctAdvancedPortableIcon.Image = My.Resources.advanced_portable_missing
      Exit Sub
    End If
    Try
      If Not IO.Directory.Exists(txtPortableDir.Text) Then
        cmdMakePortable.Enabled = False
        pctAdvancedPortableIcon.Image = My.Resources.advanced_portable_missing
        Exit Sub
      End If
    Catch ex As Exception
      cmdMakePortable.Enabled = False
      pctAdvancedPortableIcon.Image = My.Resources.advanced_portable_missing
      Exit Sub
    End Try
    cmdMakePortable.Enabled = True
    pctAdvancedPortableIcon.Image = My.Resources.advanced_portable
  End Sub
#Region "Host List"
  Private Sub PopulateHostList()
    wsHostList = New WebClientEx
    wsHostList.DownloadStringAsync(New Uri("http://wb.realityripple.com/hosts/"), "GRAB")
  End Sub
  Private Sub wsHostList_DownloadStringCompleted(sender As Object, e As System.Net.DownloadStringCompletedEventArgs) Handles wsHostList.DownloadStringCompleted
    If e.UserState = "GRAB" Then
      If Me.InvokeRequired Then
        Try
          Me.Invoke(New Net.DownloadStringCompletedEventHandler(AddressOf wsHostList_DownloadStringCompleted), sender, e)
        Catch ex As Exception

        End Try
      Else
        If e.Error Is Nothing AndAlso Not e.Cancelled AndAlso Not String.IsNullOrEmpty(e.Result) Then
          Try
            If e.Result.Contains(vbLf) Then
              Dim HostList() As String = Split(e.Result, vbLf)
              bLoaded = False
              cmbProvider.Items.Clear()
              cmbProvider.Items.AddRange(HostList)
              If mySettings.Account.Contains("@") Then
                Dim sProvider As String = mySettings.Account.Substring(mySettings.Account.LastIndexOf("@") + 1)
                cmbProvider.Text = sProvider
              End If
              bLoaded = True
            End If
          Catch ex As Exception
          End Try
        End If
      End If
    End If
  End Sub
  Private Sub UseDefaultHostList()
    cmbProvider.Items.Clear()
    cmbProvider.Items.Add("wildblue.net")
    cmbProvider.Items.Add("exede.net")
    cmbProvider.Items.Add("dishmail.net")
    cmbProvider.Items.Add("dish.net")
  End Sub
#End Region
#Region "Net Test"
  Private Sub wsFavicon_DownloadIconCompleted(sender As Object, e As clsFavicon.DownloadIconCompletedEventArgs) Handles wsFavicon.DownloadIconCompleted
    If Me.InvokeRequired Then
      Me.Invoke(New clsFavicon.DownloadIconCompletedEventHandler(AddressOf wsFavicon_DownloadIconCompleted), sender, e)
      Return
    End If
    If e.Error IsNot Nothing Then
      SetNetTestImage(pctAdvancedNetTestIcon.ErrorImage)
    Else
      SetNetTestImage(e.Icon32, e.Icon16)
    End If
  End Sub
  Private Sub SetNetTestImage(Image As Image, Optional tag As Object = Nothing)
    pctAdvancedNetTestIcon.Image = Image
    pctAdvancedNetTestIcon.Tag = tag
  End Sub
  Private Sub tmrIcoWait_Tick(sender As System.Object, e As System.EventArgs) Handles tmrIcoWait.Tick
    tmrIcoWait.Stop()
    If optNetTestCustom.Checked Then
      If Not String.IsNullOrEmpty(txtNetTestCustom.Text) Then
        SetNetTestImage(pctAdvancedNetTestIcon.InitialImage)
        wsFavicon = New clsFavicon(txtNetTestCustom.Text)
      Else
        SetNetTestImage(My.Resources.advanced_nettest_edit)
      End If
    End If
  End Sub
#End Region
#End Region
  Private Sub lblPurchaseKey_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblPurchaseKey.LinkClicked
    If lblPurchaseKey.Text = LINK_PURCHASE Then
      Try
        Process.Start("http://srt.realityripple.com/c_signup.php")
      Catch ex As Exception
        Dim taskNotifier As TaskbarNotifier = Nothing
        MakeNotifier(taskNotifier, False)
        If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", My.Application.Info.ProductName & " could not navigate to ""srt.realityripple.com/c_signup.php""!" & vbNewLine & ex.Message, 200, 3000, 100)
      End Try
    ElseIf lblPurchaseKey.Text = LINK_PANEL Then
      Try
        Process.Start("http://wb.realityripple.com?wbEMail=" & txtAccount.Text & "@" & cmbProvider.Text & "&wbKey=" & txtKey1.Text & "-" & txtKey2.Text & "-" & txtKey3.Text & "-" & txtKey4.Text & "-" & txtKey5.Text & "&wbSubmit=")
      Catch ex As Exception
        Dim taskNotifier As TaskbarNotifier = Nothing
        MakeNotifier(taskNotifier, False)
        If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", My.Application.Info.ProductName & " could not navigate to ""wb.realityripple.com""!" & vbNewLine & ex.Message, 200, 3000, 100)
      End Try
    End If
  End Sub
#Region "Remote Service Results"
  Private Sub pctKeyState_Click(sender As System.Object, e As System.EventArgs) Handles pctKeyState.Click
    If txtKey1.TextLength = 6 And txtKey2.TextLength = 4 And txtKey3.TextLength = 4 And txtKey4.TextLength = 4 And txtKey5.TextLength = 6 Then KeyCheck()
  End Sub
  Private CheckState As Boolean
  Private Sub KeyCheck()
    pctKeyState.Image = My.Resources.throbber
    CheckState = pctKeyState.Tag = 1
    pctKeyState.Tag = 0
    ttConfig.SetTooltip(pctKeyState, "Verifying your key...")
    Dim sKeyTest As String = txtKey1.Text & "-" & txtKey2.Text & "-" & txtKey3.Text & "-" & txtKey4.Text & "-" & txtKey5.Text
    cmdSave.Enabled = False
    If pChecker IsNot Nothing Then
      pChecker.Dispose()
      pChecker = Nothing
    End If
    pChecker = New Threading.Timer(New Threading.TimerCallback(AddressOf RunAccountTest), sKeyTest, 500, 1000)
  End Sub
  Private Sub remoteTest_Failure(sender As Object, e As remoteRestrictionTracker.FailureEventArgs) Handles remoteTest.Failure
    If Me.InvokeRequired Then
      Me.Invoke(New EventHandler(AddressOf remoteTest_Failure), sender, e)
    Else
      Dim bToSave As Boolean = True
      If Not CheckState Then bToSave = False
      If SettingsChanged() Then bToSave = True
      pctKeyState.Tag = 0
      pctKeyState.Image = My.Resources.ico_err
      Dim sErr As String = "There was an error verifying your key!"
      Select Case e.Type
        Case remoteRestrictionTracker.FailureEventArgs.FailType.BadLogin : sErr = "There was a server error. Please try again later."
        Case remoteRestrictionTracker.FailureEventArgs.FailType.BadProduct : sErr = "Your Product Key is incorrect."
        Case remoteRestrictionTracker.FailureEventArgs.FailType.BadServer : sErr = "There was a fault double-checking the server. You may have a security issue."
        Case remoteRestrictionTracker.FailureEventArgs.FailType.NoData : sErr = "The server did not receive login negotiation data!"
        Case remoteRestrictionTracker.FailureEventArgs.FailType.NoUsername : sErr = "Your account is not registered!"
        Case remoteRestrictionTracker.FailureEventArgs.FailType.Network : sErr = "There was a connection related error. Please check your Internet connection." & IIf(String.IsNullOrEmpty(e.Details), "", vbNewLine & e.Details)
        Case remoteRestrictionTracker.FailureEventArgs.FailType.NotBase64 : sErr = "The server did not respond in the right manner. Please check your Internet connection." & IIf(String.IsNullOrEmpty(e.Details), "", vbNewLine & e.Details)
      End Select
      If pChecker IsNot Nothing Then
        pChecker.Dispose()
        pChecker = Nothing
      End If
      If remoteTest IsNot Nothing Then
        remoteTest.Dispose()
        remoteTest = Nothing
      End If
      ttConfig.SetTooltip(pctKeyState, sErr)
      DoCheck()
      cmdSave.Enabled = bToSave
    End If
  End Sub
  Private Sub remoteTest_OKKey(sender As Object, e As System.EventArgs) Handles remoteTest.OKKey
    If Me.InvokeRequired Then
      Me.Invoke(New EventHandler(AddressOf remoteTest_OKKey), sender, e)
    Else
      Dim bToSave As Boolean = True
      If CheckState Then bToSave = False
      If SettingsChanged() Then bToSave = True
      pctKeyState.Tag = 1
      pctKeyState.Image = My.Resources.ico_ok
      ttConfig.SetTooltip(pctKeyState, "Your key has been verified!")
      lblPurchaseKey.Text = LINK_PANEL
      If pChecker IsNot Nothing Then
        pChecker.Dispose()
        pChecker = Nothing
      End If
      If remoteTest IsNot Nothing Then
        remoteTest.Dispose()
        remoteTest = Nothing
      End If
      ttConfig.SetTooltip(lblPurchaseKey, LINK_PANEL_TT)
      DoCheck()
      cmdSave.Enabled = bToSave
    End If
  End Sub
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
        If Not sDir.EndsWith(IO.Path.DirectorySeparatorChar) Then sDir &= IO.Path.DirectorySeparatorChar
        txtHistoryDir.Text = sDir
      End If
    End Using
  End Sub
  Private Sub cmdHistoryDirOpen_Click(sender As System.Object, e As System.EventArgs) Handles cmdHistoryDirOpen.Click
    If optHistoryProgramData.Checked Then
      If My.Computer.FileSystem.DirectoryExists(AppDataAllPath) Then
        Process.Start(AppDataAllPath)
      Else
        MsgDlg(Me, "The directory """ & AppDataAllPath & """ does not exist." & vbNewLine & "Please save the configuration first.", "Unable to find History directory.", "Missing Directory", MessageBoxButtons.OK, TaskDialogIcon.Recent, MessageBoxIcon.Error)
        'MessageBox.Show("The directory """ & AppDataAllPath & """ does not exist." & vbNewLine & "Please save the configuration first.", "Missing Directory", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
      End If
    ElseIf optHistoryAppData.Checked Then
      If My.Computer.FileSystem.DirectoryExists(AppDataPath) Then
        Process.Start(AppDataPath)
      Else
        MsgDlg(Me, "The directory """ & AppDataPath & """ does not exist." & vbNewLine & "Please save the configuration first.", "Unable to find History directory.", "Missing Directory", MessageBoxButtons.OK, TaskDialogIcon.Recent, MessageBoxIcon.Error)
        'MessageBox.Show("The directory """ & AppDataPath & """ does not exist." & vbNewLine & "Please save the configuration first.", "Missing Directory", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
      End If
    Else
      If My.Computer.FileSystem.DirectoryExists(txtHistoryDir.Text) Then
        Process.Start(txtHistoryDir.Text)
      Else
        MsgDlg(Me, "The directory """ & txtHistoryDir.Text & """ does not exist." & vbNewLine & "Please save the configuration first.", "Unable to find History directory.", "Missing Directory", MessageBoxButtons.OK, TaskDialogIcon.Recent, MessageBoxIcon.Error)
        'MessageBox.Show("The directory """ & txtHistoryDir.Text & """ does not exist." & vbNewLine & "Please save the configuration first.", "Missing Directory", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
      End If
    End If
  End Sub
  Private Sub cmdMakePortable_Click(sender As System.Object, e As System.EventArgs) Handles cmdMakePortable.Click
    If String.IsNullOrEmpty(txtPortableDir.Text) Then
      txtPortableDir.Focus()
      Exit Sub
    End If
    Try
      If Not IO.Directory.Exists(txtPortableDir.Text) Then
        txtPortableDir.Focus()
        Exit Sub
      End If
    Catch ex As Exception
      txtPortableDir.Focus()
      Exit Sub
    End Try
    Dim sPath As String = txtPortableDir.Text
    If Not sPath.EndsWith(IO.Path.DirectorySeparatorChar) Then sPath &= IO.Path.DirectorySeparatorChar
    Try
      IO.File.Copy(Application.ExecutablePath, sPath & "RestrictionTracker.exe", True)
      IO.File.Copy(Application.StartupPath & "\RestrictionTrackerLib.dll", sPath & "RestrictionTrackerLib.dll", True)
      IO.Directory.CreateDirectory(sPath & "Config\")
      For Each file In New IO.DirectoryInfo(AppDataPath).EnumerateFiles
        If file.Name = "user.config" Or file.Name = "backup.config" Then
          Dim sConfig As String = My.Computer.FileSystem.ReadAllText(file.FullName, System.Text.Encoding.GetEncoding(LATIN_1))
          If sConfig.Contains("<setting name=""HistoryDir"">") Then
            Dim sHistory As String = sConfig.Substring(sConfig.IndexOf("<setting name=""HistoryDir"">"))
            sHistory = sHistory.Substring(0, sHistory.IndexOf("</setting>") + 10)
            Dim sNewHistory As String = "<setting name=""HistoryDir"">" & vbNewLine & "        <value></value>" & vbNewLine & "      </setting>"
            sConfig = sConfig.Replace(sHistory, sNewHistory)
          End If
          My.Computer.FileSystem.WriteAllText(sPath & "Config\" & file.Name, sConfig, False, System.Text.Encoding.GetEncoding(LATIN_1))
        Else
          file.CopyTo(sPath & "Config\" & file.Name, True)
        End If
      Next
      If Not AppDataPath = MySaveDir Then
        For Each file In New IO.DirectoryInfo(MySaveDir(True)).EnumerateFiles
          file.CopyTo(sPath & "Config\" & file.Name, True)
        Next
      End If
      MsgDlg(Me, My.Application.Info.ProductName & " has been ported to """ & sPath & """!", "Portable application created.", "Files Copied", MessageBoxButtons.OK, TaskDialogIcon.RemovableDrive, MessageBoxIcon.Information)
      'MessageBox.Show(My.Application.Info.ProductName & " has been ported to """ & sPath & """!", "Files Copied!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
    Catch ex As Exception
      txtPortableDir.Focus()
      MsgDlg(Me, "There was an error trying to create a portable install in """ & sPath & """!", "Portable application creation error.", "Drive Error", MessageBoxButtons.OK, TaskDialogIcon.DriveLockedRemovable, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, ex.Message, Microsoft.WindowsAPICodePack.Dialogs.TaskDialogExpandedDetailsLocation.ExpandFooter, "View Error Details", "Hide Error Details")
      'MessageBox.Show("There was an error trying to create a portable install in """ & sPath & """!" & vbNewLine & ex.Message, "Drive Error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
      Exit Sub
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
        If Not sDir.EndsWith(IO.Path.DirectorySeparatorChar) Then sDir &= IO.Path.DirectorySeparatorChar
        txtPortableDir.Text = sDir
      End If
    End Using
  End Sub
  Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click
    If String.IsNullOrEmpty(txtAccount.Text) Then
      MsgDlg(Me, "You must enter your ViaSat account Username before saving the Configuration.", "Please enter your Username.", "Unable to Save", MessageBoxButtons.OK, TaskDialogIcon.User, MessageBoxIcon.Information)
      'MessageBox.Show("Please enter your ViaSat account Username.", My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
      txtAccount.Focus()
      Exit Sub
    End If
    If String.IsNullOrEmpty(txtPassword.Text) Then
      MsgDlg(Me, "You must enter your ViaSat account Password before saving the Configuration.", "Please enter your Password.", "Unable to Save", MessageBoxButtons.OK, TaskDialogIcon.Padlock, MessageBoxIcon.Information)
      'MessageBox.Show("Please enter your ViaSat account Password.", My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
      txtPassword.Focus()
      Exit Sub
    End If
    If String.IsNullOrEmpty(cmbProvider.Text) Then
      MsgDlg(Me, "Please enter your ViaSat Provider domain or select one from the list before saving the Configuration.", "Please select your Provider.", "Unable to Save", MessageBoxButtons.OK, TaskDialogIcon.Internet, MessageBoxIcon.Information)
      'MessageBox.Show("Please enter your ViaSat Provider address or select one from the list.", My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
      cmbProvider.Focus()
      Exit Sub
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
        MsgDlg(Me, "The directory you have entered contains invalid characters. You will need to choose a different directory to store your Usage History.", "Please choose a different directory.", "Unable to Save", MessageBoxButtons.OK, TaskDialogIcon.SearchFolder, MessageBoxIcon.Error, , "Directory: """ & sHD & """" & vbNewLine & "Invalid Character: " & sC & " (0x" & PadHex(AscW(c)) & ")", Microsoft.WindowsAPICodePack.Dialogs.TaskDialogExpandedDetailsLocation.ExpandFooter, "View Directory Details", "Hide Directory Details")
        'MessageBox.Show("The directory you have entered contains invalid characters. Please choose a different directory.", My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
        txtHistoryDir.Focus()
        Exit Sub
      End If
    Next
    If cmbProvider.Text.ToLower.Contains("excede") Or
       cmbProvider.Text.ToLower.Contains("force") Or
       cmbProvider.Text.ToLower.Contains("mysso") Or
       cmbProvider.Text.ToLower.Contains("myexede") Or
       cmbProvider.Text.ToLower.Contains("my.exede") Then cmbProvider.Text = "exede.net"
    If String.Compare(mySettings.Account, txtAccount.Text & "@" & cmbProvider.Text, True) <> 0 Then
      mySettings.Account = txtAccount.Text & "@" & cmbProvider.Text
      bAccount = True
    End If
    If String.Compare(StoredPassword.DecryptApp(mySettings.PassCrypt), txtPassword.Text, False) <> 0 Then
      mySettings.PassCrypt = StoredPassword.EncryptApp(txtPassword.Text)
      bAccount = True
    End If
    If Not chkAccountTypeAuto.Checked Then
      If optAccountTypeWBL.Checked Then
        mySettings.AccountType = localRestrictionTracker.SatHostTypes.WildBlue_LEGACY
      ElseIf optAccountTypeWBX.Checked Then
        mySettings.AccountType = localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
      ElseIf optAccountTypeDNX.Checked Then
        mySettings.AccountType = localRestrictionTracker.SatHostTypes.DishNet_EXEDE
      ElseIf optAccountTypeRPL.Checked Then
        mySettings.AccountType = localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY
      ElseIf optAccountTypeRPX.Checked Then
        mySettings.AccountType = localRestrictionTracker.SatHostTypes.RuralPortal_EXEDE
      End If
      mySettings.AccountTypeForced = True
    Else
      mySettings.AccountTypeForced = False
    End If
    Dim sKey As String = txtKey1.Text & "-" & txtKey2.Text & "-" & txtKey3.Text & "-" & txtKey4.Text & "-" & txtKey5.Text
    If String.Compare(mySettings.RemoteKey, sKey, True) <> 0 Then
      If pctKeyState.Tag = 1 Then
        mySettings.RemoteKey = sKey
        bAccount = True
      Else
        mySettings.RemoteKey = Nothing
        bAccount = True
      End If
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
    Select Case cmbProxyType.SelectedIndex
      Case 0 : mySettings.Proxy = Nothing
      Case 1 : mySettings.Proxy = Net.WebRequest.DefaultWebProxy
      Case 2
        If String.IsNullOrEmpty(txtProxyAddress.Text) Then
          MsgDlg(Me, "Please enter a Proxy Address or choose a different Proxy Type.", "No Proxy Address specified.", "Unable to Save", MessageBoxButtons.OK, TaskDialogIcon.InternetRJ45, MessageBoxIcon.Information)
          'MessageBox.Show("Please enter a Proxy address or choose a different option.", My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
          txtProxyAddress.Focus()
          Exit Sub
        End If
        If String.IsNullOrEmpty(txtProxyUser.Text) And String.IsNullOrEmpty(txtProxyPassword.Text) And String.IsNullOrEmpty(txtProxyDomain.Text) Then
          mySettings.Proxy = New Net.WebProxy(txtProxyAddress.Text, Integer.Parse(txtProxyPort.Value))
        Else
          If String.IsNullOrEmpty(txtProxyDomain.Text) Then
            mySettings.Proxy = New Net.WebProxy(txtProxyAddress.Text, Integer.Parse(txtProxyPort.Value)) With {.Credentials = New Net.NetworkCredential(txtProxyUser.Text, txtProxyPassword.Text)}
          Else
            mySettings.Proxy = New Net.WebProxy(txtProxyAddress.Text, Integer.Parse(txtProxyPort.Value)) With {.Credentials = New Net.NetworkCredential(txtProxyUser.Text, txtProxyPassword.Text, txtProxyDomain.Text)}
          End If
        End If
      Case 3
        If String.IsNullOrEmpty(txtProxyAddress.Text) Then
          MsgDlg(Me, "Please enter a Proxy Address or choose a different Proxy Type.", "No Proxy Address specified.", "Unable to Save", MessageBoxButtons.OK, TaskDialogIcon.InternetRJ45, MessageBoxIcon.Information)
          'MessageBox.Show("Please enter a Proxy address or choose a different option.", My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
          txtProxyAddress.Focus()
          Exit Sub
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
    If chkNetworkProtocolSSL.Checked Then
      mySettings.SecurityProtocol = Net.SecurityProtocolType.Ssl3
    Else
      mySettings.SecurityProtocol = Net.SecurityProtocolType.Tls
    End If
    Dim sNetTestIco As String = IO.Path.Combine(AppDataPath, "netTest.png")
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
    If Not String.Compare(mySettings.HistoryDir, txtHistoryDir.Text, True) = 0 Then
      Dim sOldFiles() As String = My.Computer.FileSystem.GetFiles(mySettings.HistoryDir).ToArray
      Dim sNewFiles() As String = Nothing
      If My.Computer.FileSystem.DirectoryExists(txtHistoryDir.Text) Then
        sNewFiles = My.Computer.FileSystem.GetFiles(txtHistoryDir.Text).ToArray
      Else
        Try
          My.Computer.FileSystem.CreateDirectory(txtHistoryDir.Text)
        Catch ex As Exception
          MsgDlg(Me, "The directory you selected could not be created! You will need to choose a different directory to store your Usage History.", "Please choose a different directory.", "Unable to Save", MessageBoxButtons.OK, TaskDialogIcon.SearchFolder, MessageBoxIcon.Error, , ex.Message, Microsoft.WindowsAPICodePack.Dialogs.TaskDialogExpandedDetailsLocation.ExpandFooter, "View Error Details", "Hide Error Details")
          'MessageBox.Show("The directory you selected could not be created!", My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
          optHistoryCustom.Checked = True
          txtHistoryDir.Enabled = True
          txtHistoryDir.Focus()
          Exit Sub
        End Try
      End If
      LOG_Terminate(True)
      Dim sSkipFiles() As String = {"user.config",
                                    "del.bat",
                                    "restrictioncontroller.exe",
                                    "restrictionlogger.exe",
                                    "restrictiontracker.exe",
                                    "restrictiontracker.pdb",
                                    "restrictiontrackerlib.dll",
                                    "restrictiontrackerlib.pdb",
                                    "unins*"}
      If sOldFiles IsNot Nothing AndAlso sOldFiles.Count > 0 Then
        If sNewFiles IsNot Nothing AndAlso sNewFiles.Count > 0 Then
          Dim sOverWrites As New Collections.Generic.List(Of String)
          For Each sOld In sOldFiles
            For Each sNew In sNewFiles
              If String.Compare(IO.Path.GetFileName(sNew), IO.Path.GetFileName(sOld), True) = 0 Then
                Dim DoSkip As Boolean = False
                For Each sSkip In sSkipFiles
                  If sSkip.Contains("*") Then
                    If sSkip.StartsWith("*") And sSkip.EndsWith("*") Then
                      If IO.Path.GetFileName(sNew).ToLower.Contains(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    ElseIf sSkip.StartsWith("*") Then
                      If IO.Path.GetFileName(sNew).ToLower.EndsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    ElseIf sSkip.EndsWith("*") Then
                      If IO.Path.GetFileName(sNew).ToLower.StartsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    End If
                  Else
                    If IO.Path.GetFileName(sNew).ToLower = sSkip Then DoSkip = True : Exit For
                  End If
                Next
                If DoSkip Then Continue For
                'If IO.Path.GetFileName(sNew).ToLower = "user.config" Then Continue For
                'If IO.Path.GetFileName(sNew).ToLower = "del.bat" Then Continue For
                sOverWrites.Add(IO.Path.GetFileName(sNew))
              End If
            Next
          Next
          If sOverWrites.Count > 0 Then
            If MsgDlg(Me, "Do you want to overwrite the files that already exist in the new Data Directory?", "Files exist in the new directory.", "Overwrite Files?", MessageBoxButtons.YesNo, TaskDialogIcon.FolderFull, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, "The following files will be overwritten:" & vbNewLine & Join(sOverWrites.ToArray, vbNewLine), Microsoft.WindowsAPICodePack.Dialogs.TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files") = Windows.Forms.DialogResult.Yes Then
              'If MessageBox.Show("Files exist in the new Data Directory:" & vbNewLine & Join(sOverWrites.ToArray, vbNewLine) & vbNewLine & vbNewLine & "Overwrite them?", My.Application.Info.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
              Dim sFails As New Collections.Generic.List(Of String)
              Dim sNoMove As New Collections.Generic.List(Of String)
              For Each sFile In sOldFiles
                Dim DoSkip As Boolean = False
                For Each sSkip In sSkipFiles
                  If sSkip.Contains("*") Then
                    If sSkip.StartsWith("*") And sSkip.EndsWith("*") Then
                      If IO.Path.GetFileName(sFile).ToLower.Contains(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    ElseIf sSkip.StartsWith("*") Then
                      If IO.Path.GetFileName(sFile).ToLower.EndsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    ElseIf sSkip.EndsWith("*") Then
                      If IO.Path.GetFileName(sFile).ToLower.StartsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    End If
                  Else
                    If IO.Path.GetFileName(sFile).ToLower = sSkip Then DoSkip = True : Exit For
                  End If
                Next
                If DoSkip Then Continue For
                'If IO.Path.GetFileName(sFile).ToLower = "user.config" Then Continue For
                'If IO.Path.GetFileName(sFile).ToLower = "del.bat" Then Continue For
                Dim sNewFile As String = txtHistoryDir.Text & "\" & IO.Path.GetFileName(sFile)
                Try
                  My.Computer.FileSystem.MoveFile(sFile, sNewFile, True)
                Catch ex As Exception
                  sNoMove.Add(sFile & ": " & ex.Message)
                End Try
                If txtHistoryDir.Text = AppDataAllPath Then If Not GrantFullControlToEveryone(sNewFile) Then sFails.Add(sNewFile)
              Next
              If sFails.Count > 0 Then
                MsgDlg(Me, "The permissions on some failed to set. Please run " & My.Application.Info.ProductName & " as Administrator to enable full permission control.", "Failed to set permissions.", "Permission Error", MessageBoxButtons.OK, TaskDialogIcon.ShieldUAC, MessageBoxIcon.Error, , "The following files did not receive full permissions:" & vbNewLine & Join(sFails.ToArray, vbNewLine), Microsoft.WindowsAPICodePack.Dialogs.TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files")
                'MessageBox.Show("Failed to set permissions for the following files:" & vbNewLine & Join(sFails.ToArray, vbNewLine) & "Please run " & My.Application.Info.ProductName & " as Administrator to enable full permission control.", My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
              End If
              If sNoMove.Count > 0 Then
                MsgDlg(Me, "Some files could not be moved to the new Data Directory.", "Failed to move files.", "File Error", MessageBoxButtons.OK, TaskDialogIcon.DriveLocked, MessageBoxIcon.Error, , "The following files could not be moved:" & vbNewLine & Join(sNoMove.ToArray, vbNewLine), Microsoft.WindowsAPICodePack.Dialogs.TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files")
                'MessageBox.Show("Failed to move the following files:" & vbNewLine & Join(sNoMove.ToArray, vbNewLine), My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
              End If
            Else
              Dim sFails As New Collections.Generic.List(Of String)
              Dim sNoMove As New Collections.Generic.List(Of String)
              For Each sFile In sOldFiles
                Dim DoSkip As Boolean = False
                For Each sSkip In sSkipFiles
                  If sSkip.Contains("*") Then
                    If sSkip.StartsWith("*") And sSkip.EndsWith("*") Then
                      If IO.Path.GetFileName(sFile).ToLower.Contains(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    ElseIf sSkip.StartsWith("*") Then
                      If IO.Path.GetFileName(sFile).ToLower.EndsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    ElseIf sSkip.EndsWith("*") Then
                      If IO.Path.GetFileName(sFile).ToLower.StartsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                    End If
                  Else
                    If IO.Path.GetFileName(sFile).ToLower = sSkip Then DoSkip = True : Exit For
                  End If
                Next
                If DoSkip Then Continue For
                'If IO.Path.GetFileName(sFile).ToLower = "user.config" Then Continue For
                'If IO.Path.GetFileName(sFile).ToLower = "del.bat" Then Continue For
                Dim sNewFile As String = txtHistoryDir.Text & "\" & IO.Path.GetFileName(sFile)
                If My.Computer.FileSystem.FileExists(sNewFile) Then Continue For
                Try
                  My.Computer.FileSystem.MoveFile(sFile, sNewFile, True)
                Catch ex As Exception
                  sNoMove.Add(sFile & ": " & ex.Message)
                End Try
                If txtHistoryDir.Text = AppDataAllPath Then If Not GrantFullControlToEveryone(sNewFile) Then sFails.Add(sNewFile)
              Next
              If sFails.Count > 0 Then
                MsgDlg(Me, "The permissions on some failed to set. Please run " & My.Application.Info.ProductName & " as Administrator to enable full permission control.", "Failed to set permissions.", "Permission Error", MessageBoxButtons.OK, TaskDialogIcon.ShieldUAC, MessageBoxIcon.Error, , "The following files did not receive full permissions:" & vbNewLine & Join(sFails.ToArray, vbNewLine), Microsoft.WindowsAPICodePack.Dialogs.TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files")
                'MessageBox.Show("Failed to set permissions for the following files:" & vbNewLine & Join(sFails.ToArray, vbNewLine) & "Please run " & My.Application.Info.ProductName & " as Administrator to enable full permission control.", My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
              End If
              If sNoMove.Count > 0 Then
                MsgDlg(Me, "Some files could not be moved to the new Data Directory.", "Failed to move files.", "File Error", MessageBoxButtons.OK, TaskDialogIcon.DriveLocked, MessageBoxIcon.Error, , "The following files could not be moved:" & vbNewLine & Join(sNoMove.ToArray, vbNewLine), Microsoft.WindowsAPICodePack.Dialogs.TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files")
                'MessageBox.Show("Failed to move the following files:" & vbNewLine & Join(sNoMove.ToArray, vbNewLine), My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
              End If
            End If
          Else
            Dim sFails As New Collections.Generic.List(Of String)
            Dim sNoMove As New Collections.Generic.List(Of String)
            For Each sFile In sOldFiles
              Dim DoSkip As Boolean = False
              For Each sSkip In sSkipFiles
                If sSkip.Contains("*") Then
                  If sSkip.StartsWith("*") And sSkip.EndsWith("*") Then
                    If IO.Path.GetFileName(sFile).ToLower.Contains(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                  ElseIf sSkip.StartsWith("*") Then
                    If IO.Path.GetFileName(sFile).ToLower.EndsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                  ElseIf sSkip.EndsWith("*") Then
                    If IO.Path.GetFileName(sFile).ToLower.StartsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                  End If
                Else
                  If IO.Path.GetFileName(sFile).ToLower = sSkip Then DoSkip = True : Exit For
                End If
              Next
              If DoSkip Then Continue For
              'If IO.Path.GetFileName(sFile).ToLower = "user.config" Then Continue For
              'If IO.Path.GetFileName(sFile).ToLower = "del.bat" Then Continue For
              Dim sNewFile As String = txtHistoryDir.Text & "\" & IO.Path.GetFileName(sFile)
              Try
                My.Computer.FileSystem.MoveFile(sFile, sNewFile)
              Catch ex As Exception
                sNoMove.Add(sFile & ": " & ex.Message)
              End Try
              If txtHistoryDir.Text = AppDataAllPath Then If Not GrantFullControlToEveryone(sNewFile) Then sFails.Add(sNewFile)
            Next
            If sFails.Count > 0 Then
              MsgDlg(Me, "The permissions on some failed to set. Please run " & My.Application.Info.ProductName & " as Administrator to enable full permission control.", "Failed to set permissions.", "Permission Error", MessageBoxButtons.OK, TaskDialogIcon.ShieldUAC, MessageBoxIcon.Error, , "The following files did not receive full permissions:" & vbNewLine & Join(sFails.ToArray, vbNewLine), Microsoft.WindowsAPICodePack.Dialogs.TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files")
              'MessageBox.Show("Failed to set permissions for the following files:" & vbNewLine & Join(sFails.ToArray, vbNewLine) & "Please run " & My.Application.Info.ProductName & " as Administrator to enable full permission control.", My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            End If
            If sNoMove.Count > 0 Then
              MsgDlg(Me, "Some files could not be moved to the new Data Directory.", "Failed to move files.", "File Error", MessageBoxButtons.OK, TaskDialogIcon.DriveLocked, MessageBoxIcon.Error, , "The following files could not be moved:" & vbNewLine & Join(sNoMove.ToArray, vbNewLine), Microsoft.WindowsAPICodePack.Dialogs.TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files")
              'MessageBox.Show("Failed to move the following files:" & vbNewLine & Join(sNoMove.ToArray, vbNewLine), My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            End If
          End If
        Else
          Dim sFails As New Collections.Generic.List(Of String)
          Dim sNoMove As New Collections.Generic.List(Of String)
          For Each sFile In sOldFiles
            Dim DoSkip As Boolean = False
            For Each sSkip In sSkipFiles
              If sSkip.Contains("*") Then
                If sSkip.StartsWith("*") And sSkip.EndsWith("*") Then
                  If IO.Path.GetFileName(sFile).ToLower.Contains(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                ElseIf sSkip.StartsWith("*") Then
                  If IO.Path.GetFileName(sFile).ToLower.EndsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                ElseIf sSkip.EndsWith("*") Then
                  If IO.Path.GetFileName(sFile).ToLower.StartsWith(sSkip.Replace("*", "")) Then DoSkip = True : Exit For
                End If
              Else
                If IO.Path.GetFileName(sFile).ToLower = sSkip Then DoSkip = True : Exit For
              End If
            Next
            If DoSkip Then Continue For
            'If IO.Path.GetFileName(sFile).ToLower = "user.config" Then Continue For
            'If IO.Path.GetFileName(sFile).ToLower = "del.bat" Then Continue For
            Dim sNewFile As String = txtHistoryDir.Text & "\" & IO.Path.GetFileName(sFile)
            Try
              My.Computer.FileSystem.MoveFile(sFile, sNewFile)
            Catch ex As Exception
              sNoMove.Add(sFile & ": " & ex.Message)
            End Try
            If txtHistoryDir.Text = AppDataAllPath Then If Not GrantFullControlToEveryone(sNewFile) Then sFails.Add(sNewFile)
          Next
          If sFails.Count > 0 Then
            MsgDlg(Me, "The permissions on some failed to set. Please run " & My.Application.Info.ProductName & " as Administrator to enable full permission control.", "Failed to set permissions.", "Permission Error", MessageBoxButtons.OK, TaskDialogIcon.ShieldUAC, MessageBoxIcon.Error, , "The following files did not receive full permissions:" & vbNewLine & Join(sFails.ToArray, vbNewLine), Microsoft.WindowsAPICodePack.Dialogs.TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files")
            'MessageBox.Show("Failed to set permissions for the following files:" & vbNewLine & Join(sFails.ToArray, vbNewLine) & "Please run " & My.Application.Info.ProductName & " as Administrator to enable full permission control.", My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
          End If
          If sNoMove.Count > 0 Then
            MsgDlg(Me, "Some files could not be moved to the new Data Directory.", "Failed to move files.", "File Error", MessageBoxButtons.OK, TaskDialogIcon.DriveLocked, MessageBoxIcon.Error, , "The following files could not be moved:" & vbNewLine & Join(sNoMove.ToArray, vbNewLine), Microsoft.WindowsAPICodePack.Dialogs.TaskDialogExpandedDetailsLocation.ExpandContent, "View Files", "Hide Files")
            'MessageBox.Show("Failed to move the following files:" & vbNewLine & Join(sNoMove.ToArray, vbNewLine), My.Application.Info.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
          End If
        End If
      End If
      mySettings.HistoryDir = txtHistoryDir.Text
      bAccount = True
    End If
    mySettings.Save()
    If mySettings.Service Then
      Dim cSave As New SvcSettings
      cSave.Account = mySettings.Account
      cSave.AccountType = mySettings.AccountType
      cSave.Interval = mySettings.Interval
      If Not String.IsNullOrEmpty(mySettings.PassCrypt) Then
        cSave.PassCrypt = StoredPassword.EncryptLogger(StoredPassword.DecryptApp(mySettings.PassCrypt))
      End If
      cSave.Proxy = mySettings.Proxy
      cSave.Timeout = mySettings.Timeout
      cSave.Save()
    End If
    bHardChange = False
    bSaved = True
    cmdSave.Enabled = False
  End Sub
  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    Me.Close()
  End Sub
#End Region
  Private Function SettingsChanged() As Boolean
    If mySettings Is Nothing Then Return False
    If bHardChange Then Return True
    If Not String.Compare(mySettings.Account, txtAccount.Text & "@" & cmbProvider.Text, True) = 0 Then Return True
    If Not mySettings.PassCrypt = StoredPassword.EncryptApp(txtPassword.Text) Then Return True
    If mySettings.AccountTypeForced = chkAccountTypeAuto.Checked Then Return True
    If Not chkAccountTypeAuto.Checked Then
      Select Case mySettings.AccountType
        Case localRestrictionTracker.SatHostTypes.WildBlue_LEGACY : If Not optAccountTypeWBL.Checked Then Return True
        Case localRestrictionTracker.SatHostTypes.WildBlue_EXEDE : If Not optAccountTypeWBX.Checked Then Return True
        Case localRestrictionTracker.SatHostTypes.DishNet_EXEDE : If Not optAccountTypeDNX.Checked Then Return True
        Case localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY : If Not optAccountTypeRPL.Checked Then Return True
        Case localRestrictionTracker.SatHostTypes.RuralPortal_EXEDE : If Not optAccountTypeRPX.Checked Then Return True
      End Select
    End If
    Dim sKey As String = txtKey1.Text & "-" & txtKey2.Text & "-" & txtKey3.Text & "-" & txtKey4.Text & "-" & txtKey5.Text
    If sKey.Contains("--") Then sKey = ""
    If Not String.Compare(mySettings.RemoteKey, sKey, True) = 0 Then Return True
    If Not mySettings.AutoHide = chkAutoHide.Checked Then Return True
    If Not mySettings.StartWait = txtStartWait.Value Then Return True
    If Not mySettings.Interval = txtInterval.Value Then Return True
    If Not mySettings.Accuracy = txtAccuracy.Value Then Return True
    If Not mySettings.Timeout = txtTimeout.Value Then Return True
    If chkStartUp.Checked Xor My.Computer.FileSystem.FileExists(StartupPath) Then Return True
    If Not mySettings.Service = chkService.Checked Then Return True
    If Not String.Compare(mySettings.HistoryDir, txtHistoryDir.Text, True) = 0 Then Return True
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
    If mySettings.SecurityProtocol = Net.SecurityProtocolType.Ssl3 And Not chkNetworkProtocolSSL.Checked Then Return True
    If mySettings.SecurityProtocol = Net.SecurityProtocolType.Tls And chkNetworkProtocolSSL.Checked Then Return True
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
    If pctKeyState.Tag = 0 Then
      If AppDataPath = Application.StartupPath & "\Config\" Then
        ttConfig.SetToolTip(chkService, "The Satellite Restriction Logger Service is not included with the Portable version of " & My.Application.Info.ProductName & ".")
        txtInterval.Minimum = 15
        chkService.Enabled = False
        chkService.Checked = False
      ElseIf My.Computer.FileSystem.FileExists(Application.StartupPath & "\RestrictionController.exe") Then
        txtInterval.Minimum = 15
        chkService.Enabled = True
        chkService.Checked = mySettings.Service
        ttConfig.SetTooltip(chkService, "Run Satellite Restriction Logger system service when Satellite Restriction Tracker is closed." & vbNewLine & "This service will continue running on the system so that logging may continue on any (or no) account." & vbNewLine & "Requires admin privilege prompt when the Restriction Tracker is run or closed, and the Data Directory may not be customized.")
      Else
        txtInterval.Minimum = 15
        chkService.Enabled = False
        chkService.Checked = False
        ttConfig.SetToolTip(chkService, "The Satellite Restriction Logger Service Controller was not found!" & vbNewLine & "Please Reinstall " & My.Application.Info.ProductName & " if you wish to use this feature.")
      End If
    Else
      txtInterval.Minimum = 30
      chkService.Enabled = False
      chkService.Checked = False
      ttConfig.SetTooltip(chkService, "The Satellite Restriction Logger Service is not needed when using the Remote Service!")
    End If
  End Sub
#Region "FileSystem Watcher"
  Private Sub fswController_Changed(sender As System.Object, e As EventArgs) Handles fswController.Changed, fswController.Created, fswController.Deleted, fswController.Error, fswController.Renamed
    DoCheck()
  End Sub
#End Region
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
        ctl.Margin = New Padding(3)
        If Environment.OSVersion.Version.Major = 5 Then
          CType(ctl, Checkbox).FlatStyle = FlatStyle.Standard
        Else
          CType(ctl, Checkbox).FlatStyle = FlatStyle.System
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
        If ctl.Name = pctKeyState.Name Then
          ctl.Margin = New Padding(1, 3, 3, 3)
        Else
          ctl.Margin = New Padding(21, 3, 3, 3)
          ctl.Height = ctl.Width
        End If
      End If
    Next
  End Sub

  Private Sub ValuesChanged(sender As System.Object, e As System.Windows.Forms.KeyEventArgs)

  End Sub

  Private Sub ValuesChanged(sender As System.Object, e As System.Windows.Forms.ScrollEventArgs)

  End Sub

  Private Sub ValuesChanged(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs)

  End Sub
End Class
