Imports System.Runtime.InteropServices
Friend NotInheritable Class frmWizard
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
  Private WithEvents localTest As Local.SiteConnection
  Private pChecker As Threading.Timer
  Private NeedsTLSProxy As Boolean = False
  Private Delegate Sub ParamaterizedInvoker(parameter As Object)
  Public Shared Sub ClickDrag(hWnd As IntPtr)
    If clsGlass.IsCompositionEnabled Then
      NativeMethods.ReleaseCapture()
      NativeMethods.SendMessage(hWnd, NativeMethods.WM_NCLBUTTONDOWN, NativeMethods.HTCAPTION, IntPtr.Zero)
    End If
  End Sub
  Private Sub cmdFAQ_Click(sender As System.Object, e As System.EventArgs) Handles cmdFAQ.Click
    Dim taskNotifier As TaskbarNotifier = Nothing
    OpenURL("srt.realityripple.com/faq.php", taskNotifier)
  End Sub
  Private Sub cmdNext_Click(sender As System.Object, e As System.EventArgs) Handles cmdNext.Click
    Select Case tbsWizardPages.SelectedIndex
      Case 1
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
        If txtAccountPass.ShowContents Then txtAccountPass.ShowContents = False
        UsageTest()
        Return
      Case 2
        If Not (optLocal.Checked Or optNone.Checked) Then
          optNone.Focus()
          Beep()
          Return
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
      ElseIf String.IsNullOrEmpty(txtAccountPass.Text) Then
        tbsWizardPages.SelectedIndex = 1
        Beep()
        txtAccountPass.Focus()
        Me.DialogResult = Windows.Forms.DialogResult.None
      Else
        If optLocal.Checked Then

        ElseIf optNone.Checked Then

        Else
          tbsWizardPages.SelectedIndex = 2
          Beep()
          optNone.Focus()
          Me.DialogResult = Windows.Forms.DialogResult.None
        End If
      End If
      Dim newSettings As New AppSettings
      newSettings.Account = txtAccountUsername.Text
      Dim newKey() As Byte = StoredPassword.GenerateKey()
      Dim newSalt() As Byte = StoredPassword.GenerateSalt()
      newSettings.PassCrypt = StoredPassword.Encrypt(txtAccountPass.Text, newKey, newSalt)
      newSettings.PassKey = Convert.ToBase64String(newKey)
      newSettings.PassSalt = Convert.ToBase64String(newSalt)
      If optLocal.Checked Then
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
  End Sub
  Private Sub frmWizard_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    Me.Icon = My.Resources.t16_norm
    DrawTitle()
    tbsWizardPages.ItemSize = New Size(Math.Floor(tbsWizardPages.Width / 5) - 11, cmdFAQ.Height - 2)
    ttWizard.SetToolTip(txtAccountPass.Button, "Toggle display of the Password.")
    txtOverSize.Margin = New Padding(3)
    txtOverTime.Margin = New Padding(3)
  End Sub
  Private Sub optServices_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optLocal.CheckedChanged, optNone.CheckedChanged
    lblLocal.Enabled = optLocal.Checked
    lblNone.Enabled = optNone.Checked
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
      txtAccountPass.Enabled = False
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
      txtAccountPass.Enabled = True
      optLocal.Enabled = True
      optNone.Enabled = True
      If cmdNext.Enabled Then
        cmdNext.Focus()
      Else
        cmdClose.Focus()
      End If
    End If
  End Sub
  Private Sub UsageTest()
    If localTest IsNot Nothing Then
      localTest.Dispose()
      localTest = Nothing
    End If
    Dim newSettings As New AppSettings
    newSettings.Account = txtAccountUsername.Text
    Dim newKey() As Byte = StoredPassword.GenerateKey()
    Dim newSalt() As Byte = StoredPassword.GenerateSalt()
    newSettings.PassCrypt = StoredPassword.Encrypt(txtAccountPass.Text, newKey, newSalt)
    newSettings.PassKey = Convert.ToBase64String(newKey)
    newSettings.PassSalt = Convert.ToBase64String(newSalt)
    newSettings.Service = False
    newSettings.SecurityProtocol = SecurityProtocolTypeEx.Tls11 Or SecurityProtocolTypeEx.Tls12 Or SecurityProtocolTypeEx.Tls13
    If NeedsTLSProxy Then newSettings.TLSProxy = True
    newSettings.Save()
    newSettings = Nothing
    localTest = New Local.SiteConnection(LocalAppDataDirectory, True)
  End Sub
  Private Sub LocalComplete()
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New MethodInvoker(AddressOf LocalComplete))
      Catch ex As Exception
      End Try
      Return
    End If
    If IO.File.Exists(IO.Path.Combine(LocalAppDataDirectory, "user.config")) Then IO.File.Delete(IO.Path.Combine(LocalAppDataDirectory, "user.config"))
    DrawStatus(False)
    tbsWizardPages.SelectedIndex += 1
    If localTest IsNot Nothing Then
      localTest.Dispose()
      localTest = Nothing
    End If
  End Sub
  Private Sub localTest_ConnectionFailure(sender As Object, e As Local.SiteConnectionFailureEventArgs) Handles localTest.ConnectionFailure
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(Of Local.SiteConnectionFailureEventArgs)(AddressOf localTest_ConnectionFailure), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    If IO.File.Exists(IO.Path.Combine(LocalAppDataDirectory, "user.config")) Then IO.File.Delete(IO.Path.Combine(LocalAppDataDirectory, "user.config"))
    Dim skipIt As Boolean = False
    Dim forceRetry As Boolean = False
    Select Case e.Type
      Case Local.SiteConnectionFailureType.ConnectionTimeout : MsgDlg(Me, "The server did not respond within a reasonable amount of time.", "Connection to server timed out.", "Failed to Log In", MessageBoxButtons.OK, _TaskDialogIcon.InternetTime, MessageBoxIcon.Error)
      Case Local.SiteConnectionFailureType.TLSTooOld
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
      Case Local.SiteConnectionFailureType.LoginFailure : MsgDlg(Me, e.Message, "There was an error while logging in to the server.", "Failed to Log In", MessageBoxButtons.OK, _TaskDialogIcon.InternetRJ45, MessageBoxIcon.Error)
      Case Local.SiteConnectionFailureType.UnknownAccountDetails : MsgDlg(Me, "Account information was missing. Please enter all account details before proceeding.", "Unable to log in to the server.", "Failed to Log In", MessageBoxButtons.OK, _TaskDialogIcon.User, MessageBoxIcon.Error)
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
      tbsWizardPages.SelectedIndex += 1
      Return
    End If
    DrawStatus(False)
  End Sub
  Private Sub localTest_ConnectionStatus(sender As Object, e As Local.SiteConnectionStatusEventArgs) Handles localTest.ConnectionStatus
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
      Case Local.SiteConnectionStates.Initialize : DrawStatus(True, "Initializing Connection" & sAppend & "...")
      Case Local.SiteConnectionStates.Prepare : DrawStatus(True, "Preparing to Log In" & sAppend & "...")
      Case Local.SiteConnectionStates.Login
        Select Case e.SubState
          Case Local.SiteConnectionSubStates.ReadLogin : DrawStatus(True, "Reading Login Page" & sAppend & "...")
          Case Local.SiteConnectionSubStates.Authenticate : DrawStatus(True, "Authenticating" & sAppend & "...")
          Case Else : DrawStatus(True, "Logging In" & sAppend & "...")
        End Select
      Case Local.SiteConnectionStates.TableDownload
        Select Case e.SubState
          Case Local.SiteConnectionSubStates.LoadHome : DrawStatus(True, "Downloading Home Page" & sAppend & "...")
          Case Local.SiteConnectionSubStates.LoadTable : DrawStatus(True, "Downloading Usage Table" & sAppend & "...")
          Case Else : DrawStatus(True, "Downloading Usage Table" & sAppend & "...")
        End Select
      Case Local.SiteConnectionStates.TableRead : DrawStatus(False, "Complete!")
    End Select
  End Sub
  Private Sub localTest_LoginComplete(sender As Object, e As EventArgs) Handles localTest.LoginComplete
    LocalComplete()
  End Sub
  Private Sub localTest_ConnectionResult(sender As Object, e As Local.SiteResultEventArgs) Handles localTest.ConnectionResult
    LocalComplete()
  End Sub
#End Region
End Class
