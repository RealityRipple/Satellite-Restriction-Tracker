Namespace My
  Partial Friend Class MyApplication
    Private Sub MyApplication_Startup(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
      Dim v As Authenticode.Validity = Authenticode.IsSelfSigned(Reflection.Assembly.GetExecutingAssembly().Location)
      If Not (v = Authenticode.Validity.SignedAndValid Or v = Authenticode.Validity.SignedButUntrusted) Then
        MsgBox("The Executable """ & IO.Path.GetFileName(Reflection.Assembly.GetExecutingAssembly().Location) & """ is not signed and may be corrupted or modified." & vbNewLine & "Error Code: 0x" & Hex(v), MsgBoxStyle.Critical, My.Application.Info.ProductName)
        e.Cancel = True
        Return
      End If
      Dim sDLLPath As String = IO.Path.Combine(My.Application.Info.DirectoryPath, My.Application.Info.AssemblyName & "Lib.dll")
      If Not IO.File.Exists(sDLLPath) Then
        MsgBox("The Function Library """ & IO.Path.GetFileName(sDLLPath) & """ is missing. " & My.Application.Info.ProductName & " requires this library to run.", MsgBoxStyle.Critical, My.Application.Info.ProductName)
        e.Cancel = True
        Return
      End If
      v = Authenticode.IsSelfSigned(sDLLPath)
      If Not (v = Authenticode.Validity.SignedAndValid Or v = Authenticode.Validity.SignedButUntrusted) Then
        MsgBox("The Function Library """ & IO.Path.Combine(IO.Path.GetFileName(IO.Path.GetDirectoryName(sDLLPath)), IO.Path.GetFileName(sDLLPath)) & """ is not signed and may be corrupted or modified." & vbNewLine & "Error Code: 0x" & Hex(v), MsgBoxStyle.Critical, My.Application.Info.ProductName)
        e.Cancel = True
        Return
      End If
      If e.CommandLine.Contains("/stop") Then
        e.Cancel = True
        Return
      End If
      EnableVisualStyles = True
      System.Threading.Thread.CurrentThread.CurrentUICulture = Globalization.CultureInfo.GetCultureInfo(1033)
      Dim AppDataWB As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & My.Application.Info.CompanyName & "\WildBlue Bandwidth Monitor"
      Dim AppDataSRT As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & My.Application.Info.CompanyName & "\" & My.Application.Info.ProductName
      Dim appRet As TriState = CopyDirectory(AppDataWB, AppDataSRT)
      Select Case appRet
        Case TriState.True
          If MsgDlg(Nothing, My.Application.Info.ProductName & " has copied all old Application data." & vbNewLine & "Would you like to delete the old directory?", "The data was copied successfully.", "Application Data Copied", MessageBoxButtons.YesNo, _TaskDialogIcon.TrashFull, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then My.Computer.FileSystem.DeleteDirectory(AppDataWB, FileIO.DeleteDirectoryOption.DeleteAllContents)
        Case TriState.False
          If MsgDlg(Nothing, My.Application.Info.ProductName & " was unable to copy the old Application data." & vbNewLine & "You may copy the data over manually.", "There was a failure copying the data.", "Application Data Copy Failed", MessageBoxButtons.OKCancel, _TaskDialogIcon.FolderFull, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
            Try
              Process.Start("explorer", "/select,""" & AppDataWB & """")
            Catch ex As Exception
              Dim taskNotifier As TaskbarNotifier = Nothing
              MakeNotifier(taskNotifier, False)
              If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Explorer", My.Application.Info.ProductName & " could not navigate to """ & AppDataWB & """!" & vbNewLine & ex.Message, 200, 3000, 100)
            End Try
            Try
              Process.Start("explorer", "/select,""" & AppDataSRT & """")
            Catch ex As Exception
              Dim taskNotifier As TaskbarNotifier = Nothing
              MakeNotifier(taskNotifier, False)
              If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Explorer", My.Application.Info.ProductName & " could not navigate to """ & AppDataSRT & """!" & vbNewLine & ex.Message, 200, 3000, 100)
            End Try
          End If
      End Select
      Dim cSettings As New AppSettings
      If cSettings IsNot Nothing AndAlso cSettings.Loaded Then
        If Not String.IsNullOrEmpty(cSettings.HistoryDir) AndAlso cSettings.HistoryDir.Contains("WildBlue Bandwidth Monitor") Then
          Dim oldHistoryDir As String = cSettings.HistoryDir
          cSettings.HistoryDir = Replace(cSettings.HistoryDir, "WildBlue Bandwidth Monitor", My.Application.Info.ProductName)
          Dim histRet As TriState = CopyDirectory(oldHistoryDir, cSettings.HistoryDir)
          Select Case histRet
            Case TriState.True
              If MsgDlg(Nothing, My.Application.Info.ProductName & " has copied all History data." & vbNewLine & "Would you like to delete the old directory?", "The data was copied successfully.", "History Data Copied", MessageBoxButtons.YesNo, _TaskDialogIcon.TrashFull, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then My.Computer.FileSystem.DeleteDirectory(oldHistoryDir, FileIO.DeleteDirectoryOption.DeleteAllContents)
            Case TriState.False
              If MsgDlg(Nothing, My.Application.Info.ProductName & " was unable to copy the old History data." & vbNewLine & "You may copy the data over manually.", "There was a failure copying the data..", "History Data Copy Failed", MessageBoxButtons.OKCancel, _TaskDialogIcon.FolderFull, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
                Try
                  Process.Start("explorer", "/select,""" & oldHistoryDir & """")
                Catch ex As Exception
                  Dim taskNotifier As TaskbarNotifier = Nothing
                  MakeNotifier(taskNotifier, False)
                  If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Explorer", My.Application.Info.ProductName & " could not navigate to """ & oldHistoryDir & """!" & vbNewLine & ex.Message, 200, 3000, 100)
                End Try
                Try
                  Process.Start("explorer", "/select,""" & cSettings.HistoryDir & """")
                Catch ex As Exception
                  Dim taskNotifier As TaskbarNotifier = Nothing
                  MakeNotifier(taskNotifier, False)
                  If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Explorer", My.Application.Info.ProductName & " could not navigate to """ & cSettings.HistoryDir & """!" & vbNewLine & ex.Message, 200, 3000, 100)
                End Try
              End If
          End Select
          cSettings.Save()
        End If
        If cSettings.Service Then
          Dim cSave As New SvcSettings
          cSave.Account = cSettings.Account
          cSave.AccountType = cSettings.AccountType
          cSave.Interval = cSettings.Interval
          If Not String.IsNullOrEmpty(cSettings.PassCrypt) Then
            Dim newKey() As Byte = StoredPassword.GenerateKey()
            Dim newSalt() As Byte = StoredPassword.GenerateSalt()
            If String.IsNullOrEmpty(cSettings.PassKey) Or String.IsNullOrEmpty(cSettings.PassSalt) Then
              cSave.PassCrypt = StoredPassword.Encrypt(StoredPasswordLegacy.DecryptApp(cSettings.PassCrypt), newKey, newSalt)
            Else
              cSave.PassCrypt = StoredPassword.Encrypt(StoredPassword.Decrypt(cSettings.PassCrypt, cSettings.PassKey, cSettings.PassSalt), newKey, newSalt)
            End If
            cSave.PassKey = Convert.ToBase64String(newKey)
            cSave.PassSalt = Convert.ToBase64String(newSalt)
          End If
          cSave.Proxy = cSettings.Proxy
          cSave.Timeout = cSettings.Timeout
          cSave.Save()
        Else
          If My.Computer.FileSystem.FileExists(AppDataAllPath & "\user.config") Then My.Computer.FileSystem.DeleteFile(AppDataAllPath & "\user.config")
        End If
      End If
      Dim sOldStartup As String = Nothing
      If My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) & " with Internet Access") Then
        sOldStartup = Environment.GetFolderPath(Environment.SpecialFolder.Startup) & " with Internet Access\WildBlue Bandwidth Monitor.lnk"
      Else
        sOldStartup = Environment.GetFolderPath(Environment.SpecialFolder.Startup) & "\WildBlue Bandwidth Monitor.lnk"
      End If
      If My.Computer.FileSystem.FileExists(sOldStartup) Then
        My.Computer.FileSystem.DeleteFile(sOldStartup)
        If Not My.Computer.FileSystem.FileExists(StartupPath) Then
          Dim sMyEXE As String = My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe"
          Using link As New ShellLink
            link.Target = sMyEXE
            link.WorkingDirectory = My.Application.Info.DirectoryPath
            link.Description = My.Application.Info.Trademark
            link.DisplayMode = ShellLink.LinkDisplayMode.edmNormal
            link.Save(StartupPath)
          End Using
        End If
      End If
      Dim OldDir As String = My.Computer.FileSystem.SpecialDirectories.ProgramFiles & "\" & Application.Info.CompanyName & "\WildBlue Bandwidth Monitor"
      If My.Computer.FileSystem.DirectoryExists(OldDir) Then
        If MsgDlg(Nothing, "Would you like to remove the defunct WildBlue Bandwidth Monitor from your computer?" & vbNewLine & "Your settings will be saved for use in the new Satellite Restriction Tracker.", "WildBlue Bandwith Monitor has been rebranded.", "Uninstall WildBlue Bandwidth Monitor?", MessageBoxButtons.YesNo, _TaskDialogIcon.DefaultPrograms, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
          If My.Computer.FileSystem.FileExists(OldDir & "\unins000.exe") Then
            Try
              ShellEx(OldDir & "\unins000.exe", "/silent")
            Catch ex As Exception
              MsgDlg(Nothing, "Unable to run WildBlue Bandwidth Monitor uninstaller. Please uninstall the program manually through Start or the <a href=""control appwiz.cpl"">Control Panel</a>.", "The Uninstaller failed to run.", "Uninstall Failed", MessageBoxButtons.OK, _TaskDialogIcon.ControlPanel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, ex.Message, _TaskDialogExpandedDetailsLocation.ExpandFooter, "View Error Details", "Hide Error Details")
            End Try
          Else
            Try
              My.Computer.FileSystem.DeleteDirectory(OldDir, FileIO.DeleteDirectoryOption.DeleteAllContents)
            Catch ex As Exception
              If MsgDlg(Nothing, My.Application.Info.ProductName & " was unable to delete the old WildBlue Bandwidth Monitor directory." & vbNewLine & "You may delete the folder manually.", "Directory could not be deleted.", "Uninstall Failed", MessageBoxButtons.OKCancel, _TaskDialogIcon.FolderFull, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, ex.Message, _TaskDialogExpandedDetailsLocation.ExpandFooter, "View Error Details", "Hide Error Details") = DialogResult.OK Then
                Try
                  Process.Start("explorer", "/select,""" & OldDir & """")
                Catch ex2 As Exception
                  Dim taskNotifier As TaskbarNotifier = Nothing
                  MakeNotifier(taskNotifier, False)
                  If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Explorer", My.Application.Info.ProductName & " could not navigate to """ & OldDir & """!" & vbNewLine & ex2.Message, 200, 3000, 100)
                End Try
              End If
            End Try
          End If
        End If
      End If
    End Sub
    Private Sub MyApplication_StartupNextInstance(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
      If e.CommandLine.Contains("/stop") Then
        If frmMain.IsHandleCreated Then
          frmMain.Close()
        Else
          End
        End If
        Return
      End If
      If frmMain.IsHandleCreated Then
        If Not frmMain.Visible Then frmMain.mnuRestore.PerformClick()
        e.BringToForeground = True
      End If
    End Sub
    Private Sub MyApplication_UnhandledException(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
      Try
        If e.Exception.Message.Contains("Could not load file or assembly") Then
          MsgDlg(Nothing, "A critical file is missing. Please ensure " & My.Application.Info.ProductName & " has been fully extracted or installed.", "This program is missing a required file.", "Could not load File or Assembly.", MessageBoxButtons.OK, _TaskDialogIcon.DLL, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, e.Exception.Message, _TaskDialogExpandedDetailsLocation.ExpandFooter, "View Error Details", "Hide Error Details")
          e.ExitApplication = True
          Return
        End If
        Dim frmError As New Form With
          {
            .FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow,
            .Text = "Error in " & My.Application.Info.ProductName,
            .ShowIcon = False,
            .MinimizeBox = False,
            .MaximizeBox = False,
            .HelpButton = False,
            .Size = New Size(486, 250),
             .StartPosition = FormStartPosition.CenterParent,
            .MinimumSize = New Size(340, 200),
            .TopMost = True
          }
        Dim pnlError As New TableLayoutPanel With
          {
            .RowCount = 3,
            .ColumnCount = 4,
            .Dock = DockStyle.Fill
          }
        pnlError.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 48))
        pnlError.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        pnlError.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
        pnlError.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
        pnlError.RowStyles.Add(New RowStyle(SizeType.Absolute, 48))
        pnlError.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
        pnlError.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        Dim sErrorText As String = My.Application.Info.ProductName & " has Encountered an Error"
        If e.Exception.TargetSite IsNot Nothing Then sErrorText &= " in " & e.Exception.TargetSite.Name
        Dim lblError As New Label With
          {
            .Font = New Font(frmError.Font.FontFamily, 12, FontStyle.Bold),
            .Text = sErrorText,
            .Dock = DockStyle.Fill,
            .AutoEllipsis = True,
            .TextAlign = ContentAlignment.MiddleLeft
          }
        Dim pctError As New PictureBox With
          {
            .Image = SystemIcons.Error.ToBitmap,
            .SizeMode = PictureBoxSizeMode.AutoSize,
            .BackColor = Color.Transparent,
            .Anchor = AnchorStyles.None
          }
        Dim txtError As New TextBox With
          {
            .ReadOnly = True,
            .BorderStyle = BorderStyle.Fixed3D,
            .Text = "Error: " & e.Exception.Message,
            .Dock = DockStyle.Fill,
            .Multiline = True,
            .ScrollBars = ScrollBars.Vertical
          }
        txtError.Text = "Error: " & e.Exception.Message
        If Not String.IsNullOrEmpty(e.Exception.StackTrace) Then
          If e.Exception.StackTrace.Contains(vbCr) Then
            txtError.Text &= vbNewLine & e.Exception.StackTrace.Substring(0, e.Exception.StackTrace.IndexOf(vbCr))
          Else
            txtError.Text &= vbNewLine & e.Exception.StackTrace
          End If
        Else
          If Not String.IsNullOrEmpty(e.Exception.Source) Then
            txtError.Text &= vbNewLine & " @ " & e.Exception.Source
            If e.Exception.TargetSite IsNot Nothing Then txtError.Text &= "." & e.Exception.TargetSite.Name
          Else
            If e.Exception.TargetSite IsNot Nothing Then txtError.Text &= vbNewLine & " @ " & e.Exception.TargetSite.Name
          End If
        End If
        Dim cmdReport As New Button With
          {
            .Text = "Report Error",
            .AutoSize = True,
            .Padding = New Padding(4),
            .Anchor = AnchorStyles.Right,
            .FlatStyle = FlatStyle.System,
            .Enabled = False
          }
        AddHandler cmdReport.Click, Sub()
                                      frmError.DialogResult = Windows.Forms.DialogResult.OK
                                      frmError.Close()
                                    End Sub
        Dim lblReport As New Label With
          {
            .AutoSize = True,
            .Padding = New Padding(3),
            .Text = "New version available. Please update before reporting errors.",
            .Anchor = AnchorStyles.Right
          }
        Dim cmdIgnore As New Button With
          {
            .Text = "Ignore and Continue",
            .AutoSize = True,
            .Padding = New Padding(4),
            .Anchor = AnchorStyles.Right,
            .FlatStyle = FlatStyle.System
          }
        AddHandler cmdIgnore.Click, Sub()
                                      frmError.DialogResult = Windows.Forms.DialogResult.Ignore
                                      frmError.Close()
                                    End Sub
        Dim cmdExit As New Button With
          {
            .Text = "Exit Application",
            .AutoSize = True,
            .Padding = New Padding(4),
            .Anchor = AnchorStyles.Right,
            .FlatStyle = FlatStyle.System
          }
        AddHandler cmdExit.Click, Sub()
                                    frmError.DialogResult = Windows.Forms.DialogResult.Abort
                                    frmError.Close()
                                  End Sub
        frmError.Controls.Add(pnlError)
        pnlError.Controls.Add(pctError, 0, 0)
        pnlError.Controls.Add(lblError, 1, 0)
        pnlError.Controls.Add(txtError, 1, 1)
        pnlError.Controls.Add(cmdReport, 0, 2)
        pnlError.Controls.Add(cmdIgnore, 2, 2)
        pnlError.Controls.Add(cmdExit, 3, 2)
        pnlError.SetColumnSpan(lblError, 3)
        pnlError.SetColumnSpan(txtError, 3)
        pnlError.SetColumnSpan(cmdReport, 2)
        frmError.AcceptButton = cmdReport
        frmError.CancelButton = cmdIgnore
        My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Hand)
        Dim tmrCheck As New Timer With
          {
            .Interval = 400,
            .Enabled = False
          }
        AddHandler frmError.Shown, Sub()
                                     tmrCheck.Enabled = True
                                   End Sub
        AddHandler tmrCheck.Tick, Sub()
                                    tmrCheck.Enabled = False
                                    Select Case clsUpdate.QuickCheckVersion
                                      Case clsUpdate.CheckEventArgs.ResultType.NewUpdate
                                        cmdReport.Visible = False
                                        pnlError.Controls.Remove(cmdReport)
                                        pnlError.Controls.Add(lblReport, 0, 2)
                                        pnlError.SetColumnSpan(lblReport, 2)
                                        lblReport.Text = "New version available. Please update before reporting errors."
                                      Case clsUpdate.CheckEventArgs.ResultType.NewBeta
                                        cmdReport.Visible = False
                                        pnlError.Controls.Remove(cmdReport)
                                        pnlError.Controls.Add(lblReport, 0, 2)
                                        pnlError.SetColumnSpan(lblReport, 2)
                                        lblReport.Text = "New BETA version available. Errors are often fixed in BETA versions before a final release."
                                      Case clsUpdate.CheckEventArgs.ResultType.NoUpdate : cmdReport.Enabled = True
                                    End Select
                                  End Sub
        Select Case frmError.ShowDialog
          Case DialogResult.OK
            e.ExitApplication = False
            Dim sRet As String = GitHubReporter.ReportIssue(e.Exception)
            If sRet.StartsWith("https://") Or sRet.StartsWith("http://") Then
              MsgDlg(Nothing, "Thank you for reporting the error." & vbNewLine & vbNewLine & "<a href=""" & sRet & """>View Details about the Error</a>", "The error has been reported.", "Error Report Sent!", MessageBoxButtons.OK, _TaskDialogIcon.MoveToNetwork, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, , , , , True)
              Return
            End If
            Dim sErrRep As String = "https://github.com/RealityRipple/" & GitHubReporter.ProjectID & "/issues/new"
            sErrRep &= "?title=" & srlFunctions.PercentEncode(GitHubReporter.MakeIssueTitle(e.Exception))
            sErrRep &= "&body=" & srlFunctions.PercentEncode(GitHubReporter.MakeIssueBody(e.Exception))
            MsgDlg(Nothing, sRet & vbNewLine & vbNewLine & "<a href=""" & sErrRep & """>Report the Error Manually</a>", "The error could not be reported.", "Error Report Failed!", MessageBoxButtons.OK, _TaskDialogIcon.InternetRJ45, MessageBoxIcon.Error, , , , , , True)
          Case DialogResult.Ignore : e.ExitApplication = False
          Case DialogResult.Abort : e.ExitApplication = True
          Case DialogResult.Cancel : e.ExitApplication = False
        End Select
      Catch ex As Exception
        MsgDlg(Nothing, "There was an error while handling another error.", "Error in Error Report system.", "Error Report Error", MessageBoxButtons.OK, _TaskDialogIcon.Error, MessageBoxIcon.Error, , "Error: " & ex.Message & vbNewLine & vbNewLine & "Original Error: " & e.Exception.Message, _TaskDialogExpandedDetailsLocation.ExpandFooter, "View Error Details", "Hide Error Details")
      End Try
    End Sub
  End Class
End Namespace
