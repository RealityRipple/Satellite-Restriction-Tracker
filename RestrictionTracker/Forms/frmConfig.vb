Public Class frmConfig
  Private WithEvents remoteTest As remoteRestrictionTracker
  Private bSaved, bAccount, bLoaded, bHardChange As Boolean
  Private mySettings As AppSettings
  Private pChecker As Threading.Timer
  Private Const LINK_PURCHASE As String = "Purchase a Key"
  Private Const LINK_PURCHASE_TT As String = "If you do not have a Product Key for the Remote Usage Service, you can purchase one online for as little as $15.00 a year."
  Private Const LINK_PANEL As String = "User Panel"
  Private Const LINK_PANEL_TT As String = "Manage your Remote Usage Service account online."
#Region "Form Events"
  Private Sub frmConfig_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    bLoaded = False
    mySettings = New AppSettings
    txtAccount.Text = mySettings.Account
    If Not String.IsNullOrEmpty(mySettings.PassCrypt) Then
      txtPassword.Text = StoredPassword.DecryptApp(mySettings.PassCrypt)
    End If
    txtPassword.UseSystemPasswordChar = True
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
      ttConfig.SetTooltip(pctKeyState, String.Empty)
      lblPurchaseKey.Text = LINK_PURCHASE
      ttConfig.SetTooltip(lblPurchaseKey, LINK_PURCHASE_TT)
    Else
      pctKeyState.Tag = 1
      pctKeyState.Image = My.Resources.ico_ok
      ttConfig.SetTooltip(pctKeyState, "Thank you for purchasing the Remote Usage Service for " & Application.ProductName & "!")
      lblPurchaseKey.Text = LINK_PANEL
      ttConfig.SetTooltip(lblPurchaseKey, LINK_PANEL_TT)
    End If
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
    chkStartUp.Checked = My.Computer.FileSystem.FileExists(StartupPath)
    chkScaleScreen.Checked = mySettings.ScaleScreen
    DoCheck()
    txtHistoryDir.Text = mySettings.HistoryDir
    If String.IsNullOrEmpty(txtHistoryDir.Text) Then txtHistoryDir.Text = MySaveDir
    lblHistoryDir.Enabled = Not chkService.Checked
    txtHistoryDir.Enabled = Not chkService.Checked
    cmdHistoryDir.Enabled = Not chkService.Checked
    chkInvert.Checked = mySettings.HistoryInversion

    If mySettings.Overuse = 0 Then
      chkOverAlert.Checked = False
      txtOverSize.Value = 100
    Else
      chkOverAlert.Checked = True
      txtOverSize.Value = mySettings.Overuse
    End If
    chkOverAlert_CheckedChanged(New Object, New EventArgs)
    txtOverTime.Value = mySettings.Overtime
    chkBeta.Checked = mySettings.BetaCheck
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
    cmdSave.Enabled = False
    bSaved = False
    bAccount = False
    fswController.Path = Application.StartupPath
    fswController.Filter = "RestrictionController.exe"
    fswController.NotifyFilter = IO.NotifyFilters.Attributes Or IO.NotifyFilters.CreationTime Or IO.NotifyFilters.DirectoryName Or IO.NotifyFilters.FileName Or IO.NotifyFilters.LastAccess Or IO.NotifyFilters.LastWrite Or IO.NotifyFilters.Security Or IO.NotifyFilters.Size
    fswController.EnableRaisingEvents = True
    DrawTitle()
    bLoaded = True
  End Sub
  Private Sub RunAccountTest(sKey As String)
    If pChecker IsNot Nothing Then
      pChecker.Dispose()
      pChecker = Nothing
    Else
      Exit Sub
    End If
    remoteTest = New remoteRestrictionTracker(txtAccount.Text, String.Empty, sKey, mySettings.Proxy, mySettings.Timeout, New Date(2000, 1, 1), AppData)
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
        Dim saveRet As MsgBoxResult = MsgBox("Some settings have been changed but not saved." & vbNewLine & vbNewLine & "Do you want to save the changes to your configuration?", MsgBoxStyle.Question Or MsgBoxStyle.YesNoCancel, "Save Configuration?")
        If saveRet = MsgBoxResult.Yes Then
          cmdSave.PerformClick()
          If bAccount Then
            Me.DialogResult = Windows.Forms.DialogResult.Yes
          Else
            Me.DialogResult = Windows.Forms.DialogResult.OK
          End If
        ElseIf saveRet = MsgBoxResult.No Then
          Me.DialogResult = Windows.Forms.DialogResult.No
        ElseIf saveRet = MsgBoxResult.Cancel Then
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
  Private Sub Panel_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pnlConfig.MouseMove, pnlAccuracy.MouseMove, pnlButtons.MouseMove, pnlHistoryDir.MouseMove, pnlInterval.MouseMove, pnlKey.MouseMove
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
  Private Sub ValuesChanged(sender As System.Object, e As EventArgs) Handles txtPassword.KeyPress, txtPassword.TextChanged, txtInterval.KeyPress, txtInterval.Scroll, txtInterval.ValueChanged, txtAccuracy.KeyPress, txtAccuracy.Scroll, txtAccuracy.ValueChanged, txtTimeout.ValueChanged, txtTimeout.Scroll, txtTimeout.KeyPress, chkStartUp.CheckedChanged, chkScaleScreen.CheckedChanged, txtHistoryDir.KeyPress, txtHistoryDir.TextChanged, chkInvert.CheckedChanged, txtOverSize.ValueChanged, txtOverTime.ValueChanged, chkBeta.CheckedChanged, txtProxyAddress.TextChanged, txtProxyPort.ValueChanged, txtProxyPort.Scroll, txtProxyPort.KeyPress, txtProxyUser.TextChanged, txtProxyPassword.TextChanged, txtProxyDomain.TextChanged
    If mySettings Is Nothing Then Exit Sub
    cmdSave.Enabled = SettingsChanged()
  End Sub
  Private Sub txtAccount_ValuesChanged(sender As System.Object, e As EventArgs) Handles txtAccount.KeyPress, txtAccount.TextChanged
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
      txtHistoryDir.Text = AppDataAll
    ElseIf Not String.IsNullOrEmpty(txtHistoryDir.Tag) Then
      txtHistoryDir.Text = txtHistoryDir.Tag
      txtHistoryDir.Tag = Nothing
    End If
    lblHistoryDir.Enabled = Not chkService.Checked
    txtHistoryDir.Enabled = Not chkService.Checked
    cmdHistoryDir.Enabled = Not chkService.Checked
    cmdSave.Enabled = SettingsChanged()
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
      If dirDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then txtHistoryDir.Text = IO.Path.GetDirectoryName(dirDlg.FileName) & "\"
    End Using
  End Sub
  Private Sub cmdPassDisplay_Click(sender As System.Object, e As System.EventArgs) Handles cmdPassDisplay.Click
    txtPassword.UseSystemPasswordChar = Not txtPassword.UseSystemPasswordChar
  End Sub
  Private Sub chkOverAlert_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkOverAlert.CheckedChanged
    txtOverSize.Enabled = chkOverAlert.Checked
    lblOverSize.Enabled = chkOverAlert.Checked
    txtOverTime.Enabled = chkOverAlert.Checked
    lblOverTime.Enabled = chkOverAlert.Checked
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
#End Region
  Private Sub lblPurchaseKey_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblPurchaseKey.LinkClicked
    If lblPurchaseKey.Text = LINK_PURCHASE Then
      Try
        Process.Start("http://srt.realityripple.com/c_signup.php")
      Catch ex As Exception
        Dim taskNotifier As TaskbarNotifier = Nothing
        MakeNotifier(taskNotifier, False)
        If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", Application.ProductName & " could not navigate to ""srt.realityripple.com/c_signup.php""!" & vbNewLine & ex.Message, 200, 3000, 100)
      End Try
    ElseIf lblPurchaseKey.Text = LINK_PANEL Then
      Try
        Process.Start("http://wb.realityripple.com?wbEMail=" & txtAccount.Text & "&wbKey=" & txtKey1.Text & "-" & txtKey2.Text & "-" & txtKey3.Text & "-" & txtKey4.Text & "-" & txtKey5.Text & "&wbSubmit=")
      Catch ex As Exception
        Dim taskNotifier As TaskbarNotifier = Nothing
        MakeNotifier(taskNotifier, False)
        If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", Application.ProductName & " could not navigate to ""wb.realityripple.com""!" & vbNewLine & ex.Message, 200, 3000, 100)
      End Try
    End If
  End Sub
#Region "Remote Service Results"
  Private Sub pctKeyState_Click(sender As System.Object, e As System.EventArgs) Handles pctKeyState.Click
    KeyCheck()
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
        Case remoteRestrictionTracker.FailureEventArgs.FailType.BadPassword : sErr = "Your password is incorrect!"
        Case remoteRestrictionTracker.FailureEventArgs.FailType.BadProduct : sErr = "Your Product Key is incorrect."
        Case remoteRestrictionTracker.FailureEventArgs.FailType.BadServer : sErr = "There was a fault double-checking the server. You may have a security issue."
        Case remoteRestrictionTracker.FailureEventArgs.FailType.NoData : sErr = "There is no data on your account yet!"
        Case remoteRestrictionTracker.FailureEventArgs.FailType.NoPassword : sErr = "Your account has no password registered to it!"
        Case remoteRestrictionTracker.FailureEventArgs.FailType.NoUsername : sErr = "Your account is not registered!"
        Case remoteRestrictionTracker.FailureEventArgs.FailType.Network : sErr = "There was a connection related error. Please check your Internet connection."
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
  Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click
    If Not txtAccount.Text.Contains("@") Or Not txtAccount.Text.Contains(".") Then
      MsgBox("Please enter your full ViaSat account name." & vbNewLine & "Example: Customer@WildBlue.net", MsgBoxStyle.Critical Or MsgBoxStyle.SystemModal, My.Application.Info.Title)
      txtAccount.Focus()
      Exit Sub
    End If
    If String.IsNullOrEmpty(txtPassword.Text) Then
      MsgBox("Please enter your ViaSat account password.", MsgBoxStyle.Critical Or MsgBoxStyle.SystemModal, My.Application.Info.Title)
      txtPassword.Focus()
      Exit Sub
    End If
    If String.IsNullOrEmpty(txtHistoryDir.Text) Then
      txtHistoryDir.Text = MySaveDir
    End If
    For Each c As Char In IO.Path.GetInvalidPathChars
      If txtHistoryDir.Text.Contains(c) Then
        MsgBox("The directory you have entered contains invalid characters. Please choose a different directory.", MsgBoxStyle.Critical Or MsgBoxStyle.SystemModal, My.Application.Info.Title)
        txtHistoryDir.Focus()
        Exit Sub
      End If
    Next
    If String.Compare(mySettings.Account, txtAccount.Text, True) <> 0 Then
      mySettings.Account = txtAccount.Text
      bAccount = True
    End If
    If String.Compare(StoredPassword.DecryptApp(mySettings.PassCrypt), txtPassword.Text, False) <> 0 Then
      mySettings.PassCrypt = StoredPassword.EncryptApp(txtPassword.Text)
      bAccount = True
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
    mySettings.Interval = txtInterval.Value
    mySettings.Accuracy = txtAccuracy.Value
    mySettings.Timeout = txtTimeout.Value
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
    mySettings.ScaleScreen = chkScaleScreen.Checked
    If String.IsNullOrEmpty(mySettings.HistoryDir) Then mySettings.HistoryDir = MySaveDir
    mySettings.Service = chkService.Checked
    If Not String.Compare(mySettings.HistoryDir, txtHistoryDir.Text, True) = 0 Then
      Dim sOldFiles() As String = My.Computer.FileSystem.GetFiles(mySettings.HistoryDir).ToArray
      Dim sNewFiles() As String = Nothing
      If My.Computer.FileSystem.DirectoryExists(txtHistoryDir.Text) Then
        sNewFiles = My.Computer.FileSystem.GetFiles(txtHistoryDir.Text).ToArray
      Else
        Try
          My.Computer.FileSystem.CreateDirectory(txtHistoryDir.Text)
        Catch ex As Exception
          MsgBox("The directory you selected could not be created!", MsgBoxStyle.Critical Or MsgBoxStyle.SystemModal, My.Application.Info.Title)
          txtHistoryDir.Focus()
          Exit Sub
        End Try
      End If
      LOG_Terminate(True)
      If sOldFiles IsNot Nothing AndAlso sOldFiles.Count > 0 Then
        If sNewFiles IsNot Nothing AndAlso sNewFiles.Count > 0 Then
          'Ask
          Dim sOverWrites As New Collections.Generic.List(Of String)
          For Each sOld In sOldFiles
            For Each sNew In sNewFiles
              If String.Compare(IO.Path.GetFileName(sNew), IO.Path.GetFileName(sOld), True) = 0 Then
                If IO.Path.GetFileName(sNew).ToLower = "user.config" Then Continue For
                If IO.Path.GetFileName(sNew).ToLower = "del.bat" Then Continue For
                sOverWrites.Add(IO.Path.GetFileName(sNew))
              End If
            Next
          Next
          If sOverWrites.Count > 0 Then
            If MsgBox("Files exist in the new Data Directory:" & vbNewLine & Join(sOverWrites.ToArray, vbNewLine) & vbNewLine & vbNewLine & "Overwrite them?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.SystemModal, "Overwrite Files?") = MsgBoxResult.Yes Then
              Dim sFails As New Collections.Generic.List(Of String)
              For Each sFile In sOldFiles
                If IO.Path.GetFileName(sFile).ToLower = "user.config" Then Continue For
                If IO.Path.GetFileName(sFile).ToLower = "del.bat" Then Continue For
                Dim sNewFile As String = txtHistoryDir.Text & "\" & IO.Path.GetFileName(sFile)
                My.Computer.FileSystem.MoveFile(sFile, sNewFile, True)
                If txtHistoryDir.Text = AppDataAll Then If Not GrantFullControlToEveryone(sNewFile) Then sFails.Add(sNewFile)
              Next
              If sFails.Count > 0 Then MsgBox("Failed to set permissions for the following files:" & vbNewLine & Join(sFails.ToArray, vbNewLine) & "Please run " & My.Application.Info.Title & " as Administrator to enable full permission control.", MsgBoxStyle.Exclamation, "Permission Failure")
            Else
              Dim sFails As New Collections.Generic.List(Of String)
              For Each sFile In sOldFiles
                If IO.Path.GetFileName(sFile).ToLower = "user.config" Then Continue For
                If IO.Path.GetFileName(sFile).ToLower = "del.bat" Then Continue For
                Dim sNewFile As String = txtHistoryDir.Text & "\" & IO.Path.GetFileName(sFile)
                If My.Computer.FileSystem.FileExists(sNewFile) Then Continue For
                My.Computer.FileSystem.MoveFile(sFile, sNewFile, True)
                If txtHistoryDir.Text = AppDataAll Then If Not GrantFullControlToEveryone(sNewFile) Then sFails.Add(sNewFile)
              Next
              If sFails.Count > 0 Then MsgBox("Failed to set permissions for the following files:" & vbNewLine & Join(sFails.ToArray, vbNewLine) & "Please run " & My.Application.Info.Title & " as Administrator to enable full permission control.", MsgBoxStyle.Exclamation, "Permission Failure")
            End If
          Else
            Dim sFails As New Collections.Generic.List(Of String)
            For Each sFile In sOldFiles
              If IO.Path.GetFileName(sFile).ToLower = "user.config" Then Continue For
              If IO.Path.GetFileName(sFile).ToLower = "del.bat" Then Continue For
              Dim sNewFile As String = txtHistoryDir.Text & "\" & IO.Path.GetFileName(sFile)
              My.Computer.FileSystem.MoveFile(sFile, sNewFile)
              If txtHistoryDir.Text = AppDataAll Then If Not GrantFullControlToEveryone(sNewFile) Then sFails.Add(sNewFile)
            Next
            If sFails.Count > 0 Then MsgBox("Failed to set permissions for the following files:" & vbNewLine & Join(sFails.ToArray, vbNewLine) & "Please run " & My.Application.Info.Title & " as Administrator to enable full permission control.", MsgBoxStyle.Exclamation, "Permission Failure")
          End If
        Else
          'Move old files
          Dim sFails As New Collections.Generic.List(Of String)
          For Each sFile In sOldFiles
            If IO.Path.GetFileName(sFile).ToLower = "user.config" Then Continue For
            If IO.Path.GetFileName(sFile).ToLower = "del.bat" Then Continue For
            Dim sNewFile As String = txtHistoryDir.Text & "\" & IO.Path.GetFileName(sFile)
            My.Computer.FileSystem.MoveFile(sFile, sNewFile)
            If txtHistoryDir.Text = AppDataAll Then If Not GrantFullControlToEveryone(sNewFile) Then sFails.Add(sNewFile)
          Next
          If sFails.Count > 0 Then MsgBox("Failed to set permissions for the following files:" & vbNewLine & Join(sFails.ToArray, vbNewLine) & "Please run " & My.Application.Info.Title & " as Administrator to enable full permission control.", MsgBoxStyle.Exclamation, "Permission Failure")
        End If
      Else
        'Ignore
      End If
      mySettings.HistoryDir = txtHistoryDir.Text
      bAccount = True
    End If
    mySettings.HistoryInversion = chkInvert.Checked
    If chkOverAlert.Checked Then
      mySettings.Overuse = txtOverSize.Value
    Else
      mySettings.Overuse = 0
    End If
    mySettings.Overtime = txtOverTime.Value
    mySettings.BetaCheck = chkBeta.Checked
    Select Case cmbProxyType.SelectedIndex
      Case 0 : mySettings.Proxy = Nothing
      Case 1 : mySettings.Proxy = Net.WebRequest.DefaultWebProxy
      Case 2
        If String.IsNullOrEmpty(txtProxyAddress.Text) Then
          MsgBox("Please enter a Proxy address or choose a different option.", MsgBoxStyle.Critical Or MsgBoxStyle.SystemModal, My.Application.Info.Title)
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
          MsgBox("Please enter a Proxy address or choose a different option.", MsgBoxStyle.Critical Or MsgBoxStyle.SystemModal, My.Application.Info.Title)
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
    mySettings.Save()
    If mySettings.Service Then
      Dim cSave As New SvcSettings
      cSave.Account = mySettings.Account
      If Not String.IsNullOrEmpty(mySettings.PassCrypt) Then
        cSave.PassCrypt = StoredPassword.EncryptLogger(StoredPassword.DecryptApp(mySettings.PassCrypt))
      End If
      cSave.Interval = mySettings.Interval
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
    If bHardChange Then Return True
    If Not String.Compare(mySettings.Account, txtAccount.Text, True) = 0 Then Return True
    If Not mySettings.PassCrypt = StoredPassword.EncryptApp(txtPassword.Text) Then Return True
    Dim sKey As String = txtKey1.Text & "-" & txtKey2.Text & "-" & txtKey3.Text & "-" & txtKey4.Text & "-" & txtKey5.Text
    If Not String.Compare(mySettings.RemoteKey, sKey, True) = 0 Then Return True
    If Not mySettings.Interval = txtInterval.Value Then Return True
    If Not mySettings.Accuracy = txtAccuracy.Value Then Return True
    If Not mySettings.Timeout = txtTimeout.Value Then Return True
    If chkStartUp.Checked Xor My.Computer.FileSystem.FileExists(StartupPath) Then Return True
    If Not mySettings.ScaleScreen = chkScaleScreen.Checked Then Return True
    If Not mySettings.Service = chkService.Checked Then Return True
    If Not String.Compare(mySettings.HistoryDir, txtHistoryDir.Text, True) = 0 Then Return True
    If Not mySettings.HistoryInversion = chkInvert.Checked Then Return True
    If chkOverAlert.Checked Xor mySettings.Overuse > 0 Then Return True
    If Not mySettings.Overuse = txtOverSize.Value Then Return True
    If Not mySettings.Overtime = txtOverTime.Value Then Return True
    If Not mySettings.BetaCheck = chkBeta.Checked Then Return True
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
    Return False
  End Function
  Private Sub DoCheck()
    If pctKeyState.Tag = 0 Then
      If My.Computer.FileSystem.FileExists(Application.StartupPath & "\RestrictionController.exe") Then
        txtInterval.Minimum = 15
        chkService.Enabled = True
        chkService.Checked = mySettings.Service
        ttConfig.SetTooltip(chkService, "Run Satellite Restriction Logger system service when Satellite Restriction Tracker is closed." & vbNewLine & "This service will continue running on the system so that logging may continue on any (or no) account." & vbNewLine & "Requires admin privilege prompt when the Restriction Tracker is run or closed, and the Data Directory may not be customized.")
      Else
        txtInterval.Minimum = 15
        chkService.Enabled = False
        chkService.Checked = False
        ttConfig.SetTooltip(chkService, "The Satellite Restriction Logger Service Controller was not found!" & vbNewLine & "Please Reinstall " & Application.ProductName & ".")
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
  Private Sub tmrAnim_Tick(sender As System.Object, e As System.EventArgs) Handles tmrAnim.Tick
    Dim imgSize As Size = pctSRT.DisplayRectangle.Size
    Dim iPos As Integer = tmrAnim.Tag
    If iPos >= imgSize.Width Then
      DrawTitle()
      Exit Sub
    Else
      iPos += 2
    End If
    Using bmpAnim As New Bitmap(imgSize.Width, imgSize.Height)
      Using g As Graphics = Graphics.FromImage(bmpAnim)
        g.Clear(Color.Black)
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        g.FillRectangle(New Drawing2D.LinearGradientBrush(New Rectangle(0, 0, imgSize.Width, imgSize.Height), Color.Black, Color.DeepSkyBlue, Drawing2D.LinearGradientMode.Vertical), 0, 0, imgSize.Width, imgSize.Height)
        Try
          Dim a12 As New Font("Arial", 12)
          Dim a8 As New Font("Arial", 8)
          Dim appSize As SizeF = g.MeasureString("@" & My.Application.Info.Title & "@", a12)
          Dim cmpSize As SizeF = g.MeasureString("@" & My.Application.Info.CompanyName & "#", a8)
          g.DrawString("              Restriction", a12, Brushes.Black, New RectangleF(5, 7, appSize.Width, appSize.Height))
          g.DrawString("              Restriction", a12, Brushes.White, New RectangleF(4, 6, appSize.Width, appSize.Height))
          g.DrawIconUnstretched(My.Resources.sat, New Rectangle(iPos, (imgSize.Height / 2) - 16, 32, 32))
          g.DrawString("Satellite                      Tracker", a12, Brushes.Black, New RectangleF(5, 7, appSize.Width, appSize.Height))
          g.DrawString("Satellite                      Tracker", a12, Brushes.White, New RectangleF(4, 6, appSize.Width, appSize.Height))
          g.DrawString("by " & My.Application.Info.CompanyName, a8, Brushes.RoyalBlue, New RectangleF(imgSize.Width - cmpSize.Width - 3, imgSize.Height - cmpSize.Height - 3, cmpSize.Width + 4, cmpSize.Height + 4))
          g.DrawString("by " & My.Application.Info.CompanyName, a8, Brushes.White, New RectangleF(imgSize.Width - cmpSize.Width - 4, imgSize.Height - cmpSize.Height - 4, cmpSize.Width + 4, cmpSize.Height + 4))
        Catch ex As Exception
          g.DrawIconUnstretched(My.Resources.sat, New Rectangle(iPos, (imgSize.Height / 2) - 16, 32, 32))
        End Try
      End Using
      pctSRT.Image = bmpAnim.Clone
    End Using
    tmrAnim.Tag = iPos
  End Sub
  Private Sub pctSRT_DoubleClick(sender As Object, e As System.EventArgs) Handles pctSRT.DoubleClick
    If My.Computer.Keyboard.CtrlKeyDown And My.Computer.Keyboard.AltKeyDown And Not My.Computer.Keyboard.ShiftKeyDown Then
      ToggleSong()
    End If
  End Sub
  Private Sub pctSRT_MouseEnter(sender As Object, e As System.EventArgs) Handles pctSRT.MouseEnter
    tmrAnim.Enabled = True
  End Sub
  Private Sub DrawTitle()
    tmrAnim.Tag = -32
    tmrAnim.Interval = 75
    tmrAnim.Enabled = False
    Dim imgSize As Size = pctSRT.DisplayRectangle.Size
    Using bmpAnim As New Bitmap(imgSize.Width, imgSize.Height)
      Using g As Graphics = Graphics.FromImage(bmpAnim)
        g.Clear(Color.Black)
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        g.FillRectangle(New Drawing2D.LinearGradientBrush(New Rectangle(0, 0, imgSize.Width, imgSize.Height), Color.Black, Color.DeepSkyBlue, Drawing2D.LinearGradientMode.Vertical), 0, 0, imgSize.Width, imgSize.Height)
        Try
          Dim a12 As New Font("Arial", 12)
          Dim a8 As New Font("Arial", 8)
          Dim appSize As SizeF = g.MeasureString("@" & My.Application.Info.Title & "@", a12)
          Dim cmpSize As SizeF = g.MeasureString("@" & My.Application.Info.CompanyName & "#", a8)
          g.DrawString("              Restriction", a12, Brushes.Black, New RectangleF(5, 7, appSize.Width, appSize.Height))
          g.DrawString("              Restriction", a12, Brushes.White, New RectangleF(4, 6, appSize.Width, appSize.Height))
          g.DrawString("Satellite                      Tracker", a12, Brushes.Black, New RectangleF(5, 7, appSize.Width, appSize.Height))
          g.DrawString("Satellite                      Tracker", a12, Brushes.White, New RectangleF(4, 6, appSize.Width, appSize.Height))
          g.DrawString("by " & My.Application.Info.CompanyName, a8, Brushes.RoyalBlue, New RectangleF(imgSize.Width - cmpSize.Width - 3, imgSize.Height - cmpSize.Height - 3, cmpSize.Width + 4, cmpSize.Height + 4))
          g.DrawString("by " & My.Application.Info.CompanyName, a8, Brushes.White, New RectangleF(imgSize.Width - cmpSize.Width - 4, imgSize.Height - cmpSize.Height - 4, cmpSize.Width + 4, cmpSize.Height + 4))
        Catch ex As Exception

        End Try
      End Using
      pctSRT.Image = bmpAnim.Clone
    End Using
  End Sub
End Class
