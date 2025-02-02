Friend NotInheritable Class frmAbout
  Private sEXEPath As String = IO.Path.Combine(LocalAppDataDirectory, "SRT_Setup.exe")
  Private WithEvents updateChecker As clsUpdate
  Private WithEvents taskNotifier As TaskbarNotifier
  Private tReset As Threading.Timer
#Region "Form Events"
  Private Sub frmAbout_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
    Dim ApplicationTitle As String
    If Not String.IsNullOrEmpty(My.Application.Info.ProductName) Then
      ApplicationTitle = My.Application.Info.ProductName
    Else
      ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
    End If
    Me.Text = "About " & ApplicationTitle
    lblProduct.Text = My.Application.Info.ProductName
    lblVersion.Text = "Version " & DisplayVersion(Application.ProductVersion)
    lblCLR.Text = "on " & srlFunctions.CLRCleanVersion
    lblCompany.Text = My.Application.Info.CompanyName
    txtDescription.Text = My.Application.Info.Description
    ResetUpdateButton()
  End Sub
  Private Sub frmAbout_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
    If tReset IsNot Nothing Then
      tReset.Dispose()
      tReset = Nothing
    End If
    If updateChecker IsNot Nothing Then
      updateChecker.Dispose()
      updateChecker = Nothing
    End If
    If Not e.CloseReason = CloseReason.ApplicationExitCall Then
      Try
        If My.Computer.FileSystem.FileExists(sEXEPath) Then My.Computer.FileSystem.DeleteFile(sEXEPath)
      Catch ex As Exception
      End Try
    End If
  End Sub
#End Region
#Region "Buttons"
  Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
    If tReset IsNot Nothing Then
      tReset.Dispose()
      tReset = Nothing
    End If
    If updateChecker IsNot Nothing Then
      updateChecker.Dispose()
      updateChecker = Nothing
    End If
    ResetUpdateButton()
    Me.Hide()
  End Sub
  Private Sub LogoPictureBox_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles LogoPictureBox.MouseDoubleClick
    If My.Computer.Keyboard.CtrlKeyDown And My.Computer.Keyboard.AltKeyDown And Not My.Computer.Keyboard.ShiftKeyDown Then
      ToggleSong()
    ElseIf My.Computer.Keyboard.CtrlKeyDown And Not My.Computer.Keyboard.AltKeyDown And My.Computer.Keyboard.ShiftKeyDown Then
      Err.Raise(vbObjectError + 72, "Error Test", "This is simply a test of the error report system.")
    ElseIf Not My.Computer.Keyboard.CtrlKeyDown And My.Computer.Keyboard.AltKeyDown And My.Computer.Keyboard.ShiftKeyDown Then
      If NOTIFIER_STYLE.Background Is Nothing Or NOTIFIER_STYLE.CloseButton Is Nothing Then
        Return
      End If
      If taskNotifier Is Nothing Then taskNotifier = New TaskbarNotifier
      If taskNotifier.Visible Then
        taskNotifier.SlowHide()
      Else
        MakeNotifier(taskNotifier, True)
        taskNotifier.Show("Alert Test", "This is simply a test of the alert notification system.", 200, 10 * 1000, 100)
      End If
    End If
  End Sub
  Private Sub taskNotifier_ContentClick(sender As Object, e As System.EventArgs) Handles taskNotifier.ContentClick
    taskNotifier.SlowHide()
  End Sub
  Private Sub cmdDonate_Click(sender As System.Object, e As System.EventArgs) Handles cmdDonate.Click
    OpenURL("http://realityripple.com/donate.php?itm=Satellite+Restriction+Tracker", taskNotifier)
  End Sub
  Private Sub lblCompany_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblCompany.LinkClicked
    If e.Button = Windows.Forms.MouseButtons.Left Then OpenURL("realityripple.com", taskNotifier)
  End Sub
  Private Sub lblProduct_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblProduct.LinkClicked
    If e.Button = Windows.Forms.MouseButtons.Left Then OpenURL("srt.realityripple.com", taskNotifier)
  End Sub
  Private Sub lblVersion_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblVersion.LinkClicked
    If e.Button = Windows.Forms.MouseButtons.Left Then OpenURL("srt.realityripple.com/changes.php", taskNotifier)
  End Sub
  Private Sub lblUpdate_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblUpdate.LinkClicked
    If updateChecker IsNot Nothing Then
      If e.Button = Windows.Forms.MouseButtons.Left Then
        updateChecker.DownloadUpdate(sEXEPath)
      End If
    Else
      lblUpdate.Link = False
    End If
  End Sub
#End Region
#Region "Updates"
  Private Sub cmdUpdate_Click(sender As Object, e As System.EventArgs) Handles cmdUpdate.Click
    If cmdUpdate.Text = "Check for &Updates" Then
      cmdUpdate.Visible = False
      pnlAbout.Controls.Remove(cmdUpdate)
      pnlAbout.Controls.Add(lblUpdate, 1, 2)
      pnlAbout.SetColumnSpan(lblUpdate, 3)
      lblUpdate.Visible = True
      Dim lState As Byte = LOG_State
      lblUpdate.Link = False
      Select Case lState
        Case 0 : SetUpdateValue("Update Skipped: Log is being read", False)
        Case 1
          SetUpdateValue("Initializing Update Check", True)
          Dim checkInvoker As New MethodInvoker(AddressOf BeginCheck)
          checkInvoker.BeginInvoke(Nothing, Nothing)
          Return
        Case 2 : SetUpdateValue("Update Skipped: Log is being saved", False)
        Case Else : SetUpdateValue("Update Skipped: Log is being edited", False)
      End Select
      If tReset IsNot Nothing Then
        tReset.Dispose()
        tReset = Nothing
      End If
      tReset = New Threading.Timer(New Threading.TimerCallback(AddressOf ResetUpdate), Nothing, 3500, 2000)
    ElseIf cmdUpdate.Text = "Apply &Update" Then
      Dim RecheckOnMissing As Boolean = False
      Do
        Application.DoEvents()
        Try
          If My.Computer.FileSystem.FileExists(sEXEPath) Then
            ShellEx(sEXEPath, UpdateParam)
            Application.Exit()
            Return
          ElseIf RecheckOnMissing Then
            If MsgDlg(Me, "The update file no longer exists in the location it was downloaded to." & vbNewLine & "If you are running Anti-Virus software, please make sure it hasn't quarantined or deleted the " & My.Application.Info.ProductName & " Installer." & vbNewLine, "The update installer is missing.", "Software Update Error", MessageBoxButtons.RetryCancel, _TaskDialogIcon.ShieldWarning, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Cancel Then Exit Do
          Else
            MsgDlg(Me, "The update file no longer exists in the location it was downloaded to." & vbNewLine & "If you are running Anti-Virus software, please make sure it hasn't quarantined or deleted the " & My.Application.Info.ProductName & " Installer." & vbNewLine, "The update installer is missing.", "Software Update Error", MessageBoxButtons.OK, _TaskDialogIcon.ShieldWarning, MessageBoxIcon.Warning)
            Exit Do
          End If
        Catch ex As Exception
          If MsgDlg(Me, "If you have User Account Control enabled," & vbNewLine & "please allow the " & My.Application.Info.ProductName & " Installer to run." & vbNewLine & "If you are running Anti-Virus software, please make sure it isn't blocking the " & My.Application.Info.ProductName & " Installer.", "There was an error starting the update.", "Software Update Error", MessageBoxButtons.RetryCancel, _TaskDialogIcon.ShieldWarning, MessageBoxIcon.Warning, , ex.Message, _TaskDialogExpandedDetailsLocation.ExpandFooter, "View Error Details", "Hide Error Details") = Windows.Forms.DialogResult.Cancel Then Exit Do
          RecheckOnMissing = True
        End Try
      Loop
      If tReset IsNot Nothing Then
        tReset.Dispose()
        tReset = Nothing
      End If
      cmdUpdate.Visible = False
      pnlAbout.Controls.Remove(cmdUpdate)
      pnlAbout.Controls.Add(lblUpdate, 1, 2)
      pnlAbout.SetColumnSpan(lblUpdate, 3)
      lblUpdate.Visible = True
      lblUpdate.Link = False
      SetUpdateValue("Update Failure")
      tReset = New Threading.Timer(New Threading.TimerCallback(AddressOf ResetUpdate), Nothing, 3500, 2000)
    End If
  End Sub
  Private Sub ResetUpdate()
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New MethodInvoker(AddressOf ResetUpdate))
      Catch ex As Exception
      End Try
      Return
    End If
    If tReset Is Nothing Then Return
    tReset.Dispose()
    tReset = Nothing
    ResetUpdateButton()
  End Sub
  Private Sub ResetUpdateButton()
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New MethodInvoker(AddressOf ResetUpdateButton))
      Catch ex As Exception
      End Try
      Return
    End If
    lblUpdate.Visible = False
    pnlAbout.Controls.Remove(lblUpdate)
    pnlAbout.Controls.Add(cmdUpdate, 1, 2)
    pnlAbout.SetColumnSpan(cmdUpdate, 3)
    cmdUpdate.Visible = True
    cmdUpdate.Text = "Check for &Updates"
    ttAbout.SetToolTip(cmdUpdate, "Check for a new version of Satellite Restriction Tracker.")
    If Not isAdmin() Then NativeMethods.SendMessage(cmdUpdate.Handle, NativeMethods.BCM_SETSHIELD, IntPtr.Zero, IntPtr.Zero)
  End Sub
  Private Sub NewUpdate()
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New MethodInvoker(AddressOf NewUpdate))
      Catch ex As Exception
      End Try
      Return
    End If
    If tReset Is Nothing Then
      Return
    Else
      tReset.Dispose()
      tReset = Nothing
    End If
    lblUpdate.Visible = False
    pnlAbout.Controls.Remove(lblUpdate)
    pnlAbout.Controls.Add(cmdUpdate, 1, 2)
    pnlAbout.SetColumnSpan(cmdUpdate, 3)
    cmdUpdate.Visible = True
    cmdUpdate.Text = "Apply &Update"
    ttAbout.SetToolTip(cmdUpdate, My.Application.Info.ProductName & " must restart before the update can be applied.")
    If Not isAdmin() Then NativeMethods.SendMessage(cmdUpdate.Handle, NativeMethods.BCM_SETSHIELD, IntPtr.Zero, 1)
  End Sub
  Private Sub BeginCheck()
    updateChecker = New clsUpdate
    updateChecker.CheckVersion()
  End Sub
  Private Sub SetUpdateValue(Message As String, Optional Throbber As Boolean = False, Optional ToolTip As String = Nothing)
    If String.IsNullOrEmpty(ToolTip) Then ToolTip = Message
    If Throbber Then
      If lblUpdate.Image Is Nothing Then lblUpdate.Image = My.Resources.throbber
      If Not lblUpdate.Text = "      " & Message Then lblUpdate.Text = "      " & Message
    Else
      If lblUpdate.Image IsNot Nothing Then
        lblUpdate.Image.Dispose()
        lblUpdate.Image = Nothing
      End If
      If Not lblUpdate.Text = Message Then lblUpdate.Text = Message
    End If
    ttAbout.SetToolTip(lblUpdate, ToolTip)
  End Sub
  Private Sub updateChecker_CheckingVersion(sender As Object, e As System.EventArgs) Handles updateChecker.CheckingVersion
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(AddressOf updateChecker_CheckingVersion), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    SetUpdateValue("Checking for Updates", True)
    lblUpdate.Link = False
  End Sub
  Private Sub updateChecker_CheckProgressChanged(sender As Object, e As clsUpdate.ProgressEventArgs) Handles updateChecker.CheckProgressChanged
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(Of clsUpdate.ProgressEventArgs)(AddressOf updateChecker_CheckProgressChanged), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    Dim sProgress As String = "(" & e.ProgressPercentage & "%)"
    SetUpdateValue("Checking for Updates " & sProgress, True)
    lblUpdate.Link = False
  End Sub
  Private Sub updateChecker_CheckResult(sender As Object, e As clsUpdate.CheckEventArgs) Handles updateChecker.CheckResult
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(Of clsUpdate.CheckEventArgs)(AddressOf updateChecker_CheckResult), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    If tReset IsNot Nothing Then
      tReset.Dispose()
      tReset = Nothing
    End If
    If e.Error IsNot Nothing Then
      If e.Error.Message.Contains("The remote name could not be resolved") Then
        SetUpdateValue("Unable to Connect: Server not Found")
        lblUpdate.Link = False
        tReset = New Threading.Timer(New Threading.TimerCallback(AddressOf ResetUpdate), Nothing, 3500, 2000)
      Else
        SetUpdateValue(e.Error.Message)
        lblUpdate.Link = False
        tReset = New Threading.Timer(New Threading.TimerCallback(AddressOf ResetUpdate), Nothing, 3500, 2000)
      End If
    ElseIf e.Cancelled Then
      SetUpdateValue("Update Check Cancelled")
      lblUpdate.Link = False
      tReset = New Threading.Timer(New Threading.TimerCallback(AddressOf ResetUpdate), Nothing, 3500, 2000)
    Else
      Dim mySettings As New AppSettings
      If mySettings.UpdateType = AppSettings.UpdateTypes.Ask Then
        Dim fUpdate As New frmUpdate
        fUpdate.TopMost = Me.TopMost
        Select Case e.Result
          Case clsUpdate.CheckEventArgs.ResultType.NewUpdate
            SetUpdateValue("New Update Available", , "Click to begin download.")
            lblUpdate.Link = True
            Application.DoEvents()
            fUpdate.NewUpdate(e.Version, False, False)
            Select Case fUpdate.ShowDialog()
              Case Windows.Forms.DialogResult.Yes
                lblUpdate.Link = False
                updateChecker.DownloadUpdate(sEXEPath)
              Case Windows.Forms.DialogResult.No

              Case Windows.Forms.DialogResult.OK
                lblUpdate.Link = False
                updateChecker.DownloadUpdate(sEXEPath)
                mySettings.UpdateBETA = False
                mySettings.Save()
              Case Windows.Forms.DialogResult.Cancel
                mySettings.UpdateBETA = False
                mySettings.Save()
              Case Else

            End Select
          Case clsUpdate.CheckEventArgs.ResultType.NewBeta
            If mySettings.UpdateBETA Then
              SetUpdateValue("New BETA Available", , "Click to begin download.")
              lblUpdate.Link = True
              Application.DoEvents()
              fUpdate.NewUpdate(e.Version, True, False)
              Select Case fUpdate.ShowDialog()
                Case Windows.Forms.DialogResult.Yes
                  lblUpdate.Link = False
                  updateChecker.DownloadUpdate(sEXEPath)
                Case Windows.Forms.DialogResult.No

                Case Windows.Forms.DialogResult.OK
                  lblUpdate.Link = False
                  updateChecker.DownloadUpdate(sEXEPath)
                  mySettings.UpdateBETA = False
                  mySettings.Save()
                Case Windows.Forms.DialogResult.Cancel
                  mySettings.UpdateBETA = False
                  mySettings.Save()
                  SetUpdateValue("No New Updates")
                  lblUpdate.Link = False
                Case Else

              End Select
            Else
              SetUpdateValue("No New Updates")
              lblUpdate.Link = False
              tReset = New Threading.Timer(New Threading.TimerCallback(AddressOf ResetUpdate), Nothing, 3500, 2000)
            End If
          Case clsUpdate.CheckEventArgs.ResultType.NoUpdate
            SetUpdateValue("No New Updates")
            lblUpdate.Link = False
            tReset = New Threading.Timer(New Threading.TimerCallback(AddressOf ResetUpdate), Nothing, 3500, 2000)
        End Select
      Else
        Select Case e.Result
          Case clsUpdate.CheckEventArgs.ResultType.NewUpdate
            SetUpdateValue("New Update Available")
            lblUpdate.Link = False
            Application.DoEvents()
            updateChecker.DownloadUpdate(sEXEPath)
          Case clsUpdate.CheckEventArgs.ResultType.NewBeta
            If mySettings.UpdateBETA Then
              SetUpdateValue("New BETA Available")
              lblUpdate.Link = False
              Application.DoEvents()
              updateChecker.DownloadUpdate(sEXEPath)
            Else
              SetUpdateValue("No New Updates")
              lblUpdate.Link = False
              tReset = New Threading.Timer(New Threading.TimerCallback(AddressOf ResetUpdate), Nothing, 3500, 2000)
            End If
          Case clsUpdate.CheckEventArgs.ResultType.NoUpdate
            SetUpdateValue("No New Updates")
            lblUpdate.Link = False
            tReset = New Threading.Timer(New Threading.TimerCallback(AddressOf ResetUpdate), Nothing, 3500, 2000)
        End Select
      End If
      mySettings = Nothing
    End If
  End Sub
  Private Sub updateChecker_DownloadingUpdate(sender As Object, e As System.EventArgs) Handles updateChecker.DownloadingUpdate
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(AddressOf updateChecker_DownloadingUpdate), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    tmrSpeed.Enabled = True
    SetUpdateValue("Downloading Update", True)
    lblUpdate.Link = False
  End Sub
  Private Sub updateChecker_DownloadResult(sender As Object, e As clsUpdate.DownloadEventArgs) Handles updateChecker.DownloadResult
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(Of clsUpdate.DownloadEventArgs)(AddressOf updateChecker_DownloadResult), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    tmrSpeed.Enabled = False
    If e.Error IsNot Nothing Then
      SetUpdateValue(e.Error.Message)
      lblUpdate.Link = False
    ElseIf e.Cancelled Then
      updateChecker.Dispose()
      SetUpdateValue("Download Cancelled")
      lblUpdate.Link = False
    Else
      updateChecker.Dispose()
      SetUpdateValue("Download Complete")
      Application.DoEvents()
      If My.Computer.FileSystem.FileExists(sEXEPath) Then
        If tReset IsNot Nothing Then
          tReset.Dispose()
          tReset = Nothing
        End If
        tReset = New Threading.Timer(New Threading.TimerCallback(AddressOf NewUpdate), Nothing, 1000, 2000)
      Else
        SetUpdateValue("Update Failure")
      End If
    End If
  End Sub
  Private CurSize As Long
  Private TotalSize As Long
  Private DownSpeed As ULong
  Private CurPercent As Integer
  Private Sub updateChecker_UpdateProgressChanged(sender As Object, e As clsUpdate.ProgressEventArgs) Handles updateChecker.UpdateProgressChanged
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(Of clsUpdate.ProgressEventArgs)(AddressOf updateChecker_UpdateProgressChanged), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    CurSize = e.BytesReceived
    TotalSize = e.TotalBytesToReceive
    CurPercent = e.ProgressPercentage
  End Sub
  Private LastSize As Long
  Private Sub tmrSpeed_Tick(sender As Object, e As System.EventArgs) Handles tmrSpeed.Tick
    If CurSize > LastSize Then
      DownSpeed = CurSize - LastSize
    Else
      DownSpeed = 0
    End If
    LastSize = CurSize
    Dim sProgress As String = "(" & CurPercent & "%)"
    Dim sStatus As String = ByteSize(CurSize) & " of " & ByteSize(TotalSize) & " at " & ByteSize(DownSpeed) & "/s"
    If TotalSize = 0 Then
      sStatus = "Downloading Update (Waiting for Response)"
      sProgress = "(Waiting for Response)"
    End If
    SetUpdateValue("Downloading Update " & sProgress, True, sStatus)
    lblUpdate.Link = False
  End Sub
#End Region
End Class
