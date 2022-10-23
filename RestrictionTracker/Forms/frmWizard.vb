Imports RestrictionLibrary.localRestrictionTracker
Imports System.Runtime.InteropServices
Public Class frmWizard
  <StructLayout(LayoutKind.Explicit)>
  Private Structure DWord
    <FieldOffset(0)> Public LongValue As Integer
    <FieldOffset(0)> Public LoWord As Short
    <FieldOffset(2)> Public HiWord As Short
    Public Sub New(lo As Integer, hi As Integer)
      LoWord = CShort(lo)
      HiWord = CShort(hi)
    End Sub
  End Structure
  Private Shared Function MakeLong(ByVal LoWord As Integer, ByVal HiWord As Integer) As Integer
    Return (New DWord(LoWord, HiWord)).LongValue
  End Function
  Private WithEvents remoteTest As remoteRestrictionTracker
  Private WithEvents localTest As localRestrictionTracker
  Private pChecker As Threading.Timer
  Private AccountType As SatHostTypes = SatHostTypes.Other
  Private NeedsTLSProxy As Boolean = False
  Private Delegate Sub ParamaterizedInvoker(parameter As Object)
  Public Shared Sub ClickDrag(hWnd As IntPtr)
    If clsGlass.IsCompositionEnabled Then
      NativeMethods.ReleaseCapture()
      NativeMethods.SendMessage(hWnd, NativeMethods.WM_NCLBUTTONDOWN, NativeMethods.HTCAPTION, IntPtr.Zero)
    End If
  End Sub
  Private Sub cmdFAQ_Click(sender As System.Object, e As System.EventArgs) Handles cmdFAQ.Click
    Try
      Process.Start("http://srt.realityripple.com/faq.php")
    Catch ex As Exception
      Dim taskNotifier As TaskbarNotifier = Nothing
      MakeNotifier(taskNotifier, False)
      If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", My.Application.Info.ProductName & " could not navigate to ""srt.realityripple.com/faq.php""!" & vbNewLine & ex.Message, 200, 3000, 100)
    End Try
  End Sub
  Private Sub cmdNext_Click(sender As System.Object, e As System.EventArgs) Handles cmdNext.Click
    Select Case tbsWizardPages.SelectedIndex
      Case 1
        If String.IsNullOrEmpty(cmbAccountHost.Text) Then
          cmbAccountHost.Focus()
          Beep()
          Return
        End If
        If String.IsNullOrEmpty(txtAccountUsername.Text) Then
          txtAccountUsername.Focus()
          Beep()
          Return
        End If
        If String.IsNullOrEmpty(txtAccountPass.Text) Then
          txtAccountPass.Focus()
          Beep()
          Return
        End If
        If AccountType = SatHostTypes.Other Then
          DrawStatus(True, "Determining your Account Type...")
          UsageTest()
          Return
        End If
      Case 2
        If Not (optRemote.Checked Or optLocal.Checked Or optNone.Checked) Then
          optNone.Focus()
          Beep()
          Return
        ElseIf optRemote.Checked Then
          If Not pnlKey.Tag = 1 Then
            If txtKey1.TextLength = txtKey1.MaxLength And txtKey2.TextLength = txtKey2.MaxLength And txtKey3.TextLength = txtKey3.MaxLength And txtKey4.TextLength = txtKey4.MaxLength And txtKey5.TextLength = txtKey5.MaxLength Then
              DrawStatus(True, "Checking your Product Key...")
              KeyCheck()
              Return
            Else
              If txtKey1.TextLength < 6 Then
                txtKey1.Focus()
              ElseIf txtKey2.TextLength < 4 Then
                txtKey2.Focus()
              ElseIf txtKey3.TextLength < 4 Then
                txtKey3.Focus()
              ElseIf txtKey4.TextLength < 4 Then
                txtKey4.Focus()
              ElseIf txtKey5.TextLength < 6 Then
                txtKey5.Focus()
              End If
              Beep()
              Return
            End If
          End If
        End If
    End Select
    If tbsWizardPages.SelectedIndex < tbsWizardPages.TabCount - 1 Then
      tbsWizardPages.SelectedIndex += 1
    End If
  End Sub
  Private Sub cmdPrevious_Click(sender As System.Object, e As System.EventArgs) Handles cmdPrevious.Click
    If tbsWizardPages.SelectedIndex > 0 Then
      tbsWizardPages.SelectedIndex -= 1
    End If
  End Sub
  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    If tbsWizardPages.SelectedIndex = tbsWizardPages.TabCount - 1 Then
      If String.IsNullOrEmpty(txtAccountUsername.Text) Then
        tbsWizardPages.SelectedIndex = 1
        Beep()
        txtAccountUsername.Focus()
        Me.DialogResult = Windows.Forms.DialogResult.None
      ElseIf String.IsNullOrEmpty(cmbAccountHost.Text) Then
        tbsWizardPages.SelectedIndex = 1
        Beep()
        cmbAccountHost.Focus()
        Me.DialogResult = Windows.Forms.DialogResult.None
      ElseIf String.IsNullOrEmpty(txtAccountPass.Text) Then
        tbsWizardPages.SelectedIndex = 1
        Beep()
        txtAccountPass.Focus()
        Me.DialogResult = Windows.Forms.DialogResult.None
      Else
        If optRemote.Checked Then
          If Not pnlKey.Tag = 1 Then
            tbsWizardPages.SelectedIndex = 2
            optRemote.Focus()
            MsgDlg(Me, "Please verify your Remote Usage Service Product Key before continuing.", "Your Product Key has not been validated.", "Verify your Product Key", MessageBoxButtons.OK, _TaskDialogIcon.Key, MessageBoxIcon.Error)
            Me.DialogResult = Windows.Forms.DialogResult.None
          End If
        ElseIf optLocal.Checked Then

        ElseIf optNone.Checked Then

        Else
          tbsWizardPages.SelectedIndex = 2
          Beep()
          optNone.Focus()
          Me.DialogResult = Windows.Forms.DialogResult.None
        End If
      End If
      Dim newSettings As New AppSettings
      newSettings.AccountType = AccountType
      newSettings.Account = txtAccountUsername.Text & "@" & cmbAccountHost.Text
      Dim newKey() As Byte = StoredPassword.GenerateKey()
      Dim newSalt() As Byte = StoredPassword.GenerateSalt()
      newSettings.PassCrypt = StoredPassword.Encrypt(txtAccountPass.Text, newKey, newSalt)
      newSettings.PassKey = Convert.ToBase64String(newKey)
      newSettings.PassSalt = Convert.ToBase64String(newSalt)
      If optRemote.Checked And pnlKey.Tag = 1 Then
        newSettings.Service = False
        newSettings.RemoteKey = txtKey1.Text & "-" & txtKey2.Text & "-" & txtKey3.Text & "-" & txtKey4.Text & "-" & txtKey5.Text
        newSettings.Interval = 30
      ElseIf optLocal.Checked Then
        newSettings.Service = True
        newSettings.Interval = 15
      Else
        newSettings.Service = False
        newSettings.Interval = 15
      End If
      If optAccuracy0.Checked Then
        newSettings.Accuracy = 0
      ElseIf optAccuracy1.Checked Then
        newSettings.Accuracy = 1
      ElseIf optAccuracy2.Checked Then
        newSettings.Accuracy = 2
      ElseIf optAccuracy3.Checked Then
        newSettings.Accuracy = 3
      Else
        newSettings.Accuracy = 0
      End If
      newSettings.ScaleScreen = chkDisplayScale.Checked
      If chkOverAlert.Checked Then
        newSettings.Overuse = txtOverSize.Value
        newSettings.Overtime = txtOverTime.Value
      Else
        newSettings.Overuse = 0
        newSettings.Overtime = 15
      End If
      newSettings.SecurityProtocol = SecurityProtocolTypeEx.Tls11 Or SecurityProtocolTypeEx.Tls12 Or SecurityProtocolTypeEx.Tls13
      If NeedsTLSProxy Then newSettings.TLSProxy = True
      newSettings.Save()
      If newSettings.Service Then
        Dim cSave As New SvcSettings
        cSave.Account = newSettings.Account
        cSave.AccountType = newSettings.AccountType
        cSave.Interval = newSettings.Interval
        If Not String.IsNullOrEmpty(newSettings.PassCrypt) Then
          Dim svcKey() As Byte = StoredPassword.GenerateKey()
          Dim svcSalt() As Byte = StoredPassword.GenerateSalt()
          cSave.PassCrypt = StoredPassword.Encrypt(txtAccountPass.Text, svcKey, svcSalt)
          cSave.PassKey = Convert.ToBase64String(svcKey)
          cSave.PassSalt = Convert.ToBase64String(svcSalt)
        End If
        cSave.Save()
      End If
      newSettings = Nothing
    End If
  End Sub
  Private Sub tbsWizardPages_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles tbsWizardPages.SelectedIndexChanged
    cmdPrevious.Enabled = Not tbsWizardPages.SelectedIndex = 0
    cmdNext.Enabled = Not tbsWizardPages.SelectedIndex = tbsWizardPages.TabCount - 1
    If tbsWizardPages.SelectedIndex = tbsWizardPages.TabCount - 1 Then
      cmdClose.Text = "Finish"
      Me.AcceptButton = cmdClose
      Me.CancelButton = cmdClose
      cmdClose.DialogResult = Windows.Forms.DialogResult.OK
    Else
      cmdClose.Text = "Cancel"
      Me.AcceptButton = cmdNext
      Me.CancelButton = cmdClose
      cmdClose.DialogResult = Windows.Forms.DialogResult.Cancel
    End If

    Select Case tbsWizardPages.SelectedIndex
      Case 0
        pctLeftBox.Image = My.Resources.wizWelcome
      Case 1
        pctLeftBox.Image = My.Resources.wizAccount
        If cmbAccountHost.Items.Count = 0 Then
          DrawStatus(True, "Loading Host List...")
          cmbAccountHost.Text = ""
          cmbAccountHost.Enabled = False
          Dim tPopulate As New Threading.Thread(AddressOf PopulateHostList)
          tPopulate.Start()
        End If
      Case 2
        pctLeftBox.Image = My.Resources.wizService
        If My.Computer.FileSystem.FileExists(IO.Path.Combine(Application.StartupPath, "RestrictionController.exe")) Then
          optLocal.Enabled = True
          lblLocal.Text = "Gather data whenever the computer is online"
          lblLocal.Enabled = optLocal.Checked
        Else
          optLocal.Enabled = False
          lblLocal.Text = "The Logger Service is not Installed"
          lblLocal.Enabled = True
        End If
      Case 3
        pctLeftBox.Image = My.Resources.wizDisplay
      Case 4
        pctLeftBox.Image = My.Resources.wizFinished
    End Select
  End Sub
  Private Sub tmrDraw_Tick(sender As System.Object, e As System.EventArgs) Handles tmrDraw.Tick
    DrawTitle()
  End Sub
  Private Sub pctHeader_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctHeader.MouseDown
    If e.Button = Windows.Forms.MouseButtons.Left Then ClickDrag(Me.Handle)
  End Sub
  Private Sub pctIcon_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctIcon.MouseDown
    If e.X >= 6 And e.X <= 6 + 16 And e.Y >= 8 And e.Y <= 8 + 16 Then
      If e.Button = Windows.Forms.MouseButtons.Left Or e.Button = Windows.Forms.MouseButtons.Right Then NativeMethods.SendMessage(Me.Handle, NativeMethods.WM_GETSYSMENU, IntPtr.Zero, (New DWord(Cursor.Position.X, Cursor.Position.Y)).LongValue)
    ElseIf e.Button = Windows.Forms.MouseButtons.Left Then
      ClickDrag(Me.Handle)
    End If
  End Sub
  Private Sub frmWizard_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If localTest IsNot Nothing Then
      localTest.Dispose()
      localTest = Nothing
    End If
    If remoteTest IsNot Nothing Then
      remoteTest.Dispose()
      remoteTest = Nothing
    End If
  End Sub
  Private Sub frmWizard_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    Me.Icon = My.Resources.t16_norm
    DrawTitle()
    tbsWizardPages.ItemSize = New Size(Math.Floor(tbsWizardPages.Width / 5) - 11, cmdFAQ.Height - 2)
    ttWizard.SetToolTip(txtAccountPass.Button, "Toggle display of the Password.")
    txtOverSize.Margin = New Padding(3)
    txtOverTime.Margin = New Padding(3)
    txtKey1.ContextMenu = mnuKey
    txtKey2.ContextMenu = mnuKey
    txtKey3.ContextMenu = mnuKey
    txtKey4.ContextMenu = mnuKey
    txtKey5.ContextMenu = mnuKey
  End Sub
  Private Sub cmbAccountHost_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbAccountHost.SelectedIndexChanged
    AccountType = SatHostTypes.Other
    pnlKey.Tag = 0
  End Sub
  Private Sub txtAccountUsername_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtAccountUsername.TextChanged
    AccountType = SatHostTypes.Other
    pnlKey.Tag = 0
  End Sub
  Private Sub txtAccountPass_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtAccountPass.TextChanged
    AccountType = SatHostTypes.Other
    pnlKey.Tag = 0
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
            txtKey1.SelectionStart = txtKey1.TextLength
            txtKey1.SelectionLength = 0
          Case "txtKey3"
            txtKey2.Focus()
            txtKey2.SelectionStart = txtKey2.TextLength
            txtKey2.SelectionLength = 0
          Case "txtKey4"
            txtKey3.Focus()
            txtKey3.SelectionStart = txtKey3.TextLength
            txtKey3.SelectionLength = 0
          Case "txtKey5"
            txtKey4.Focus()
            txtKey4.SelectionStart = txtKey4.TextLength
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
    pnlKey.Tag = 0
  End Sub
#Region "Context Menu"
  Private Sub mnuKey_Popup(sender As System.Object, e As System.EventArgs) Handles mnuKey.Popup
    Dim txtKey As TextBox = CType(CType(sender, ContextMenu).SourceControl, TextBox)
    mnuKeyCut.Enabled = Not (String.IsNullOrEmpty(txtKey1.Text) And String.IsNullOrEmpty(txtKey2.Text) And String.IsNullOrEmpty(txtKey3.Text) And String.IsNullOrEmpty(txtKey4.Text) And String.IsNullOrEmpty(txtKey5.Text))
    mnuKeyCopy.Enabled = Not (String.IsNullOrEmpty(txtKey1.Text) And String.IsNullOrEmpty(txtKey2.Text) And String.IsNullOrEmpty(txtKey3.Text) And String.IsNullOrEmpty(txtKey4.Text) And String.IsNullOrEmpty(txtKey5.Text))
    mnuKeyPaste.Enabled = Not String.IsNullOrEmpty(Clipboard.GetText)
    mnuKeyDelete.Enabled = Not String.IsNullOrEmpty(txtKey.Text)
    mnuKeyClear.Enabled = Not (String.IsNullOrEmpty(txtKey1.Text) And String.IsNullOrEmpty(txtKey2.Text) And String.IsNullOrEmpty(txtKey3.Text) And String.IsNullOrEmpty(txtKey4.Text) And String.IsNullOrEmpty(txtKey5.Text))
  End Sub
  Private Sub mnuKeyPaste_Click(sender As System.Object, e As System.EventArgs) Handles mnuKeyPaste.Click
    Dim txtKey As TextBox = CType(CType(CType(sender, MenuItem).Parent, ContextMenu).SourceControl, TextBox)
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
        Else
          If sKey.Length > txtKey.MaxLength Then sKey = sKey.Substring(0, txtKey.MaxLength)
          txtKey.Text = sKey
        End If
      Else
        If sKey.Length > txtKey.MaxLength Then sKey = sKey.Substring(0, txtKey.MaxLength)
        txtKey.Text = sKey
      End If
    End If
  End Sub
  Private Sub mnuKeyCut_Click(sender As System.Object, e As System.EventArgs) Handles mnuKeyCut.Click
    If Not (String.IsNullOrEmpty(txtKey1.Text) And String.IsNullOrEmpty(txtKey2.Text) And String.IsNullOrEmpty(txtKey3.Text) And String.IsNullOrEmpty(txtKey4.Text) And String.IsNullOrEmpty(txtKey5.Text)) Then
      If txtKey1.TextLength = txtKey1.MaxLength And txtKey2.TextLength = txtKey2.MaxLength And txtKey3.TextLength = txtKey3.MaxLength And txtKey4.TextLength = txtKey4.MaxLength And txtKey5.TextLength = txtKey5.MaxLength Then
        Dim sKey As String = txtKey1.Text & "-" & txtKey2.Text & "-" & txtKey3.Text & "-" & txtKey4.Text & "-" & txtKey5.Text
        Clipboard.SetText(sKey)
        txtKey1.Clear()
        txtKey2.Clear()
        txtKey3.Clear()
        txtKey4.Clear()
        txtKey5.Clear()
        Return
      End If
    End If
    Dim txtKey As TextBox = CType(CType(CType(sender, MenuItem).Parent, ContextMenu).SourceControl, TextBox)
    txtKey.Cut()
  End Sub
  Private Sub mnuKeyCopy_Click(sender As System.Object, e As System.EventArgs) Handles mnuKeyCopy.Click
    If Not (String.IsNullOrEmpty(txtKey1.Text) And String.IsNullOrEmpty(txtKey2.Text) And String.IsNullOrEmpty(txtKey3.Text) And String.IsNullOrEmpty(txtKey4.Text) And String.IsNullOrEmpty(txtKey5.Text)) Then
      If txtKey1.TextLength = txtKey1.MaxLength And txtKey2.TextLength = txtKey2.MaxLength And txtKey3.TextLength = txtKey3.MaxLength And txtKey4.TextLength = txtKey4.MaxLength And txtKey5.TextLength = txtKey5.MaxLength Then
        Dim sKey As String = txtKey1.Text & "-" & txtKey2.Text & "-" & txtKey3.Text & "-" & txtKey4.Text & "-" & txtKey5.Text
        Clipboard.SetText(sKey)
        Return
      End If
    End If
    Dim txtKey As TextBox = CType(CType(CType(sender, MenuItem).Parent, ContextMenu).SourceControl, TextBox)
    txtKey.Copy()
  End Sub
  Private Sub mnuKeyDelete_Click(sender As System.Object, e As System.EventArgs) Handles mnuKeyDelete.Click
    Dim txtKey As TextBox = CType(CType(CType(sender, MenuItem).Parent, ContextMenu).SourceControl, TextBox)
    txtKey.Clear()
  End Sub
  Private Sub mnuKeyClear_Click(sender As System.Object, e As System.EventArgs) Handles mnuKeyClear.Click
    txtKey1.Clear()
    txtKey2.Clear()
    txtKey3.Clear()
    txtKey4.Clear()
    txtKey5.Clear()
  End Sub
#End Region
  Private Sub txtSignUp_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles txtSignUp.LinkClicked
    Try
      Process.Start("http://srt.realityripple.com/c_signup.php")
    Catch ex As Exception
      Dim taskNotifier As TaskbarNotifier = Nothing
      MakeNotifier(taskNotifier, False)
      If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", My.Application.Info.ProductName & " could not navigate to ""srt.realityripple.com/c_signup.php""!" & vbNewLine & ex.Message, 200, 3000, 100)
    End Try
  End Sub
  Private Sub optServices_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optRemote.CheckedChanged, optLocal.CheckedChanged, optNone.CheckedChanged
    txtKey1.Enabled = optRemote.Checked
    txtKey2.Enabled = optRemote.Checked
    txtKey3.Enabled = optRemote.Checked
    txtKey4.Enabled = optRemote.Checked
    txtKey5.Enabled = optRemote.Checked
    lblLocal.Enabled = optLocal.Checked
    lblNone.Enabled = optNone.Checked
    If optRemote.Checked Then txtKey1.Focus()
  End Sub
  Private Sub chkOverAlert_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkOverAlert.CheckedChanged
    txtOverSize.Enabled = chkOverAlert.Checked
    lblOverSize.Enabled = chkOverAlert.Checked
    txtOverTime.Enabled = chkOverAlert.Checked
    lblOverTime.Enabled = chkOverAlert.Checked
  End Sub
#Region "Display Functions"
  Private Sub DrawTitle()
    Dim sTitle As String = "Satellite Restriction Tracker Configuration Wizard"
    If clsGlass.IsCompositionEnabled Then
      Me.SuspendLayout()
      If Not pctHeader.BackColor = Color.Black Then
        pctIcon.BackColor = Color.Black
        pctIcon.Height = 35
        pctHeader.BackColor = Color.Black
        pctHeader.Height = 35
        clsGlass.SetGlass(Me, 0, 35, 0, 0)
        Me.Text = Nothing
        Me.ShowIcon = False
      End If
      Dim TextFont As New Drawing.Font(Drawing.SystemFonts.CaptionFont.Name, 9)
      Dim offset As Integer = (TextFont.Height - 8) / 2
      If offset < 4 Then offset = 4
      clsGlass.DrawTextOnGlass(pctHeader.Handle, sTitle, TextFont, New Rectangle(0, -offset, 600, 36 + offset), 16 - offset, 12)
      Using g As Graphics = Graphics.FromHwnd(pctIcon.Handle)
        g.DrawImageUnscaled(My.Resources.t16_norm.ToBitmap, New Rectangle(6, 8, 16, 16))
      End Using
      Me.ResumeLayout()
    Else
      If String.IsNullOrEmpty(Me.Text) Then
        pctIcon.Height = 0
        pctIcon.BackColor = Color.White
        pctHeader.Height = 0
        pctHeader.BackColor = Color.White
        clsGlass.SetGlass(Me, 0, 0, 0, 0)
        Me.Text = sTitle
        Me.ShowIcon = True
      End If
    End If
  End Sub
  Private Delegate Sub DrawStatusInvoker(Busy As Boolean, Message As String)
  Private Sub DrawStatus(Busy As Boolean, Optional Message As String = Nothing)
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New DrawStatusInvoker(AddressOf DrawStatus), Busy, Message)
      Catch ex As Exception
      End Try
      Return
    End If
    If Busy Then
      txtAccountUsername.Enabled = False
      cmbAccountHost.Enabled = False
      txtAccountPass.Enabled = False
      optRemote.Enabled = False
      txtKey1.Enabled = False
      txtKey2.Enabled = False
      txtKey3.Enabled = False
      txtKey4.Enabled = False
      txtKey5.Enabled = False
      optLocal.Enabled = False
      optNone.Enabled = False
      cmdPrevious.Enabled = False
      cmdNext.Enabled = False
      lblActivity.Text = Message
      cmdClose.Focus()
    Else
      cmdPrevious.Enabled = Not tbsWizardPages.SelectedIndex = 0
      cmdNext.Enabled = Not tbsWizardPages.SelectedIndex = tbsWizardPages.TabCount - 1
      lblActivity.Text = Nothing
      txtAccountUsername.Enabled = True
      cmbAccountHost.Enabled = True
      txtAccountPass.Enabled = True
      optRemote.Enabled = True
      txtKey1.Enabled = optRemote.Checked
      txtKey2.Enabled = optRemote.Checked
      txtKey3.Enabled = optRemote.Checked
      txtKey4.Enabled = optRemote.Checked
      txtKey5.Enabled = optRemote.Checked
      optLocal.Enabled = True
      optNone.Enabled = True
      If cmdNext.Enabled Then
        cmdNext.Focus()
      Else
        cmdClose.Focus()
      End If
    End If
  End Sub
  Private Sub PopulateHostList()
    Try
      Dim sRet As String
      Dim wsHostList As New WebClientEx
      wsHostList.KeepAlive = False
      sRet = wsHostList.DownloadString("http://wb.realityripple.com/hosts/")
      SetHostListData(sRet)
    Catch ex As Exception
      SetHostListData(Nothing)
    End Try
  End Sub
  Private Delegate Sub SetHostListDataCallback(sHosts As String)
  Private Sub SetHostListData(sHosts As String)
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New SetHostListDataCallback(AddressOf SetHostListData), sHosts)
      Catch ex As Exception
      End Try
      Return
    End If
    DrawStatus(False)
    cmbAccountHost.Items.Clear()
    cmbAccountHost.Text = ""
    cmbAccountHost.Enabled = True
    If String.IsNullOrEmpty(sHosts) OrElse Not sHosts.Contains(vbLf) Then
      Dim hostData() As String = Split(My.Resources.HostList, vbNewLine)
      For I As Integer = 0 To hostData.Length - 1
        cmbAccountHost.Items.Add(hostData(I))
      Next
    Else
      cmbAccountHost.Items.AddRange(Split(sHosts, vbLf))
    End If
  End Sub
  Private sAccount As String
  Private Sub KeyCheck()
    Dim sKeyTest As String = txtKey1.Text & "-" & txtKey2.Text & "-" & txtKey3.Text & "-" & txtKey4.Text & "-" & txtKey5.Text
    If pChecker IsNot Nothing Then
      pChecker.Dispose()
      pChecker = Nothing
    End If
    If String.IsNullOrEmpty(txtAccountUsername.Text) Then
      tbsWizardPages.SelectedIndex = 1
      txtAccountUsername.Focus()
      DrawStatus(False)
      MsgDlg(Me, "You must enter an Account Username before validating your Product Key!", "You did not enter a Username.", "Missing Account Information", MessageBoxButtons.OK, _TaskDialogIcon.User, MessageBoxIcon.Error)
      Return
    End If
    If String.IsNullOrEmpty(cmbAccountHost.Text) Then
      tbsWizardPages.SelectedIndex = 1
      cmbAccountHost.Focus()
      DrawStatus(False)
      MsgDlg(Me, "You must choose a Provider domain name before validating your Product Key!", "You did not select your Provider.", "Missing Account Information", MessageBoxButtons.OK, _TaskDialogIcon.InternetNetwork, MessageBoxIcon.Error)
      Return
    End If
    If cmbAccountHost.Text.ToUpperInvariant.Contains("EXCEDE") Or cmbAccountHost.Text.ToUpperInvariant.Contains("FORCE") Then cmbAccountHost.Text = "exede.net"
    If cmbAccountHost.Text.ToUpperInvariant = "DISH.NET" Or cmbAccountHost.Text.ToUpperInvariant = "DISH.COM" Then cmbAccountHost.Text = "mydish.com"
    sAccount = txtAccountUsername.Text & "@" & cmbAccountHost.Text
    pChecker = New Threading.Timer(New Threading.TimerCallback(AddressOf RunAccountTest), sKeyTest, 500, 1000)
  End Sub
  Private Sub RunAccountTest(sKey As String)
    If pChecker IsNot Nothing Then
      pChecker.Dispose()
      pChecker = Nothing
    Else
      Return
    End If
    remoteTest = New remoteRestrictionTracker(sAccount, String.Empty, sKey, Nothing, 60, New Date(2000, 1, 1), LocalAppDataDirectory)
  End Sub
  Private Sub remoteTest_Failure(sender As Object, e As remoteRestrictionTracker.FailureEventArgs) Handles remoteTest.Failure
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(Of remoteRestrictionTracker.FailureEventArgs)(AddressOf remoteTest_Failure), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    Dim sErr As String = "There was an error verifying your key!"
    Select Case e.Type
      Case remoteRestrictionTracker.FailureEventArgs.FailType.BadLogin : sErr = "There was a server error. Please try again later."
      Case remoteRestrictionTracker.FailureEventArgs.FailType.BadProduct : sErr = "Your Product Key is incorrect."
      Case remoteRestrictionTracker.FailureEventArgs.FailType.BadServer : sErr = "There was a fault double-checking the server. You may have a security issue."
      Case remoteRestrictionTracker.FailureEventArgs.FailType.NoData : sErr = "The server did not receive login negotiation data!" & vbNewLine & "Please check your Internet connection and try again."
      Case remoteRestrictionTracker.FailureEventArgs.FailType.NoUsername : sErr = "Your account is not registered!"
      Case remoteRestrictionTracker.FailureEventArgs.FailType.Network : sErr = "You must be online to activate the Remote Usage Service." & vbNewLine & "Please check your Internet connection and try again."
      Case remoteRestrictionTracker.FailureEventArgs.FailType.NotBase64 : sErr = "The server responded in an unexpected format, which may indicate a problem with your connection or with the server." & vbNewLine & "Please check your Internet connection and try again."
    End Select
    If pChecker IsNot Nothing Then
      pChecker.Dispose()
      pChecker = Nothing
    End If
    If remoteTest IsNot Nothing Then
      remoteTest.Dispose()
      remoteTest = Nothing
    End If
    Select Case e.Type
      Case remoteRestrictionTracker.FailureEventArgs.FailType.BadLogin, remoteRestrictionTracker.FailureEventArgs.FailType.BadProduct, remoteRestrictionTracker.FailureEventArgs.FailType.NoData, remoteRestrictionTracker.FailureEventArgs.FailType.Network, remoteRestrictionTracker.FailureEventArgs.FailType.NotBase64
        MsgDlg(Me, sErr, "There was an error verifying your key.", "Unable to Verify", MessageBoxButtons.OK, _TaskDialogIcon.Key, MessageBoxIcon.Warning)
      Case Else
        optNone.Checked = True
        MsgDlg(Me, sErr, "Your key could not be verified.", "Failed to Verify", MessageBoxButtons.OK, _TaskDialogIcon.Key, MessageBoxIcon.Error)
    End Select
    DrawStatus(False)
    pnlKey.Tag = 0
  End Sub
  Private Sub remoteTest_OKKey(sender As Object, e As System.EventArgs) Handles remoteTest.OKKey
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(AddressOf remoteTest_OKKey), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    If pChecker IsNot Nothing Then
      pChecker.Dispose()
      pChecker = Nothing
    End If
    If remoteTest IsNot Nothing Then
      remoteTest.Dispose()
      remoteTest = Nothing
    End If
    DrawStatus(True)
    pnlKey.Tag = 1
    tbsWizardPages.SelectedIndex += 1
  End Sub
  Private Sub UsageTest()
    If localTest IsNot Nothing Then
      localTest.Dispose()
      localTest = Nothing
    End If
    Dim newSettings As New AppSettings
    newSettings.AccountType = SatHostTypes.Other
    newSettings.Account = txtAccountUsername.Text & "@" & cmbAccountHost.Text
    Dim newKey() As Byte = StoredPassword.GenerateKey()
    Dim newSalt() As Byte = StoredPassword.GenerateSalt()
    newSettings.PassCrypt = StoredPassword.Encrypt(txtAccountPass.Text, newKey, newSalt)
    newSettings.PassKey = Convert.ToBase64String(newKey)
    newSettings.PassSalt = Convert.ToBase64String(newSalt)
    newSettings.Service = False
    newSettings.RemoteKey = Nothing
    newSettings.SecurityProtocol = SecurityProtocolTypeEx.Tls11 Or SecurityProtocolTypeEx.Tls12 Or SecurityProtocolTypeEx.Tls13
    If NeedsTLSProxy Then newSettings.TLSProxy = True
    newSettings.Save()
    newSettings = Nothing
    localTest = New localRestrictionTracker(LocalAppDataDirectory, True)
  End Sub
  Private Sub LocalComplete(acct As SatHostTypes)
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New ParamaterizedInvoker(AddressOf LocalComplete), acct)
      Catch ex As Exception
      End Try
      Return
    End If
    If IO.File.Exists(IO.Path.Combine(LocalAppDataDirectory, "user.config")) Then IO.File.Delete(IO.Path.Combine(LocalAppDataDirectory, "user.config"))
    AccountType = acct
    DrawStatus(False)
    tbsWizardPages.SelectedIndex += 1
    Try
      Dim sckHostList As Net.WebRequest = Net.HttpWebRequest.Create("http://wb.realityripple.com/hosts/?add=" & cmbAccountHost.Text)
      sckHostList.BeginGetResponse(Nothing, Nothing)
    Catch ex As Exception
    End Try
    If localTest IsNot Nothing Then
      localTest.Dispose()
      localTest = Nothing
    End If
  End Sub
  Private Sub localTest_ConnectionFailure(sender As Object, e As ConnectionFailureEventArgs) Handles localTest.ConnectionFailure
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(Of ConnectionFailureEventArgs)(AddressOf localTest_ConnectionFailure), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    If IO.File.Exists(IO.Path.Combine(LocalAppDataDirectory, "user.config")) Then IO.File.Delete(IO.Path.Combine(LocalAppDataDirectory, "user.config"))
    AccountType = SatHostTypes.Other
    Dim skipIt As Boolean = False
    Dim forceRetry As Boolean = False
    Select Case e.Type
      Case ConnectionFailureEventArgs.FailureType.ConnectionTimeout : MsgDlg(Me, "The server did not respond within a reasonable amount of time.", "Connection to server timed out.", "Failed to Log In", MessageBoxButtons.OK, _TaskDialogIcon.InternetTime, MessageBoxIcon.Error)
      Case ConnectionFailureEventArgs.FailureType.TLSTooOld
        If NeedsTLSProxy = True Then
          skipIt = True
        Else
          If (Environment.OSVersion.Version.Major < 6 Or (Environment.OSVersion.Version.Major = 6 And Environment.OSVersion.Version.Minor = 0)) Then
            If MsgDlg(Me, "Your Operating System is too old to support the Security Protocol required to log in." & vbNewLine & "Using the TLS Proxy will bypass this problem, but it has limitations and is not secure.", "Would you like to enable the TLS Proxy?", "Failed to Log In", MessageBoxButtons.YesNo, _TaskDialogIcon.InternetRJ45, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then NeedsTLSProxy = True : forceRetry = True
          ElseIf (Environment.Version.Major = 4 And Environment.Version.Minor = 0 And Environment.Version.Build = 30319 And Environment.Version.Revision < 17929) Then
            skipIt = True
            If MsgDlg(Me, "Your version of the .NET Framework is too old to support the Security Protocol required to log in. Please <a href=""control update"">update</a> to <a href=""https://www.microsoft.com/net/download/framework"">.NET 4.5</a> or newer." & vbNewLine & "Using the TLS Proxy will bypass this problem, but it has limitations and is not secure.", "Would you like to enable the TLS Proxy?", "Failed to Log In", MessageBoxButtons.YesNo, _TaskDialogIcon.InternetRJ45, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then NeedsTLSProxy = True : forceRetry = True
          Else
            skipIt = True
            If e.Message = "VER" Then
              If MsgDlg(Me, "TLS 1.1 and 1.2 could not be enabled on your computer for an unknown reason. Please let me know you got this message." & vbNewLine & "Using the TLS Proxy will bypass this problem, but it has limitations and is not secure.", "Would you like to enable the TLS Proxy?", "Failed to Log In", MessageBoxButtons.YesNo, _TaskDialogIcon.InternetRJ45, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then NeedsTLSProxy = True : forceRetry = True
            ElseIf e.Message = "PROXY" Then
              If MsgDlg(Me, "Even though TLS 1.1 or 1.2 was enabled, the server still didin't like the request. Please let me know you got this message." & vbNewLine & "Using the TLS Proxy will bypass this problem, but it has limitations and is not secure.", "Would you like to enable the TLS Proxy?", "Failed to Log In", MessageBoxButtons.YesNo, _TaskDialogIcon.InternetRJ45, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then NeedsTLSProxy = True : forceRetry = True
            End If
          End If
        End If
      Case ConnectionFailureEventArgs.FailureType.LoginFailure : MsgDlg(Me, e.Message, "There was an error while logging in to the server.", "Failed to Log In", MessageBoxButtons.OK, _TaskDialogIcon.InternetRJ45, MessageBoxIcon.Error)
      Case ConnectionFailureEventArgs.FailureType.FatalLoginFailure : MsgDlg(Me, e.Message, "There was a fatal error while logging in to the server.", "Failed to Log In", MessageBoxButtons.OK, _TaskDialogIcon.InternetRJ45, MessageBoxIcon.Error)
      Case ConnectionFailureEventArgs.FailureType.UnknownAccountDetails : MsgDlg(Me, "Account information was missing. Please enter all account details before proceeding.", "Unable to log in to the server.", "Failed to Log In", MessageBoxButtons.OK, _TaskDialogIcon.User, MessageBoxIcon.Error)
      Case ConnectionFailureEventArgs.FailureType.UnknownAccountType : tbsWizardPages.SelectedIndex += 1
    End Select
    If localTest IsNot Nothing Then
      localTest.Dispose()
      localTest = Nothing
    End If
    If forceRetry Then
      DrawStatus(True, "Retrying with TLS Proxy...")
      UsageTest()
      Return
    End If
    If skipIt Then
      DrawStatus(False)
      AccountType = SatHostTypes.WildBlue_EXEDE
      tbsWizardPages.SelectedIndex += 1
      Return
    End If
    DrawStatus(False)
  End Sub
  Private Sub localTest_ConnectionStatus(sender As Object, e As ConnectionStatusEventArgs) Handles localTest.ConnectionStatus
    Dim sAppend As String = ""
    If e.Attempt > 0 Then
      If e.Stage > 0 Then
        sAppend = " (Stage " & (e.Stage + 1) & ", Redirect #" & e.Attempt & ")"
      Else
        sAppend = " (Redirect #" & e.Attempt & ")"
      End If
    ElseIf e.Stage > 0 Then
      sAppend = " (Stage " & (e.Stage + 1) & ")"
    End If
    Select Case e.Status
      Case ConnectionStates.Initialize : DrawStatus(True, "Initializing Connection" & sAppend & "...")
      Case ConnectionStates.Prepare : DrawStatus(True, "Preparing to Log In" & sAppend & "...")
      Case ConnectionStates.Login
        Select Case e.SubState
          Case ConnectionSubStates.ReadLogin : DrawStatus(True, "Reading Login Page" & sAppend & "...")
          Case ConnectionSubStates.Authenticate : DrawStatus(True, "Authenticating" & sAppend & "...")
          Case ConnectionSubStates.Verify : DrawStatus(True, "Verifying" & sAppend & "...")
          Case Else : DrawStatus(True, "Logging In" & sAppend & "...")
        End Select
      Case ConnectionStates.TableDownload
        Select Case e.SubState
          Case ConnectionSubStates.LoadHome : DrawStatus(True, "Downloading Home Page" & sAppend & "...")
          Case ConnectionSubStates.LoadAJAX
            If e.Attempt = 0 Then
              DrawStatus(True, "Downloading AJAX Data (" & e.Stage & " of " & localTest.ExedeResellerAJAXFirstTryRequests & ")...")
            Else
              DrawStatus(True, "Re-Downloading AJAX Data (" & e.Stage & " of " & localTest.ExedeResellerAJAXSecondTryRequests & ")...")
            End If
          Case ConnectionSubStates.LoadTable : DrawStatus(True, "Downloading Usage Table" & sAppend & "...")
          Case Else : DrawStatus(True, "Downloading Usage Table" & sAppend & "...")
        End Select
      Case ConnectionStates.TableRead : DrawStatus(False, "Complete!")
    End Select
  End Sub
  Private Sub localTest_LoginComplete(sender As Object, e As RestrictionLibrary.localRestrictionTracker.LoginCompletionEventArgs) Handles localTest.LoginComplete
    LocalComplete(e.HostType)
  End Sub
  Private Sub localTest_ConnectionDNXResult(sender As Object, e As TYPEA2ResultEventArgs) Handles localTest.ConnectionDNXResult
    LocalComplete(SatHostTypes.Dish_EXEDE)
  End Sub
  Private Sub localTest_ConnectionRPLResult(sender As Object, e As TYPEAResultEventArgs) Handles localTest.ConnectionRPLResult
    LocalComplete(SatHostTypes.RuralPortal_LEGACY)
  End Sub
  Private Sub localTest_ConnectionRPXResult(sender As Object, e As TYPEBResultEventArgs) Handles localTest.ConnectionRPXResult
    LocalComplete(SatHostTypes.RuralPortal_EXEDE)
  End Sub
  Private Sub localTest_ConnectionWBLResult(sender As Object, e As TYPEAResultEventArgs) Handles localTest.ConnectionWBLResult
    LocalComplete(SatHostTypes.WildBlue_LEGACY)
  End Sub
  Private Sub localTest_ConnectionWBXResult(sender As Object, e As TYPEBResultEventArgs) Handles localTest.ConnectionWBXResult
    LocalComplete(SatHostTypes.WildBlue_EXEDE)
  End Sub
  Private Sub localTest_ConnectionWXRResult(sender As Object, e As TYPEBResultEventArgs) Handles localTest.ConnectionWXRResult
    LocalComplete(SatHostTypes.WildBlue_EXEDE_RESELLER)
  End Sub
#End Region
End Class
