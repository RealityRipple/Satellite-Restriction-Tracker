Namespace My
  Partial Friend Class MyApplication
    Private Sub MyApplication_Startup(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
      Dim AppDataWB As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & My.Application.Info.CompanyName & "\WildBlue Bandwidth Monitor"
      Dim AppDataSRT As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & My.Application.Info.CompanyName & "\" & My.Application.Info.ProductName
      Dim appRet As TriState = CopyDirectory(AppDataWB, AppDataSRT)
      Select Case appRet
        Case TriState.True
          If MessageBox.Show(Application.Info.ProductName & " has copied all old Application data." & vbNewLine & "Would you like to delete the old directory?", "Application Data Copied", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification) = DialogResult.Yes Then
            My.Computer.FileSystem.DeleteDirectory(AppDataWB, FileIO.DeleteDirectoryOption.DeleteAllContents)
          End If
        Case TriState.False
          If MessageBox.Show(Application.Info.ProductName & " was unable to copy the old Application data." & vbNewLine & "You may copy the data over manually.", "Application Data Copy Failed", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification) = DialogResult.OK Then
            Try
              Process.Start("explorer", "/select,""" & AppDataWB & """")
            Catch ex As Exception
              Dim taskNotifier As TaskbarNotifier = Nothing
              MakeNotifier(taskNotifier, False)
              If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Explorer", Application.Info.ProductName & " could not navigate to """ & AppDataWB & """!" & vbNewLine & ex.Message, 200, 3000, 100)
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
              If MessageBox.Show(Application.Info.ProductName & " has copied all History data." & vbNewLine & "Would you like to delete the old directory?", "History Data Copied", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification) = DialogResult.Yes Then
                My.Computer.FileSystem.DeleteDirectory(oldHistoryDir, FileIO.DeleteDirectoryOption.DeleteAllContents)
              End If
            Case TriState.False
              If MessageBox.Show(Application.Info.ProductName & " was unable to copy the old History data." & vbNewLine & "You may copy the data over manually.", "History Data Copy Failed", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification) = DialogResult.OK Then
                Try
                  Process.Start("explorer", "/select,""" & oldHistoryDir & """")
                Catch ex As Exception
                  Dim taskNotifier As TaskbarNotifier = Nothing
                  MakeNotifier(taskNotifier, False)
                  If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Explorer", Application.Info.ProductName & " could not navigate to """ & oldHistoryDir & """!" & vbNewLine & ex.Message, 200, 3000, 100)
                End Try
              End If
          End Select
          cSettings.Save()
        End If
        If cSettings.Service Then
          Dim cSave As New SvcSettings
          cSave.Account = cSettings.Account
          If Not String.IsNullOrEmpty(cSettings.PassCrypt) Then
            cSave.PassCrypt = StoredPassword.EncryptLogger(StoredPassword.DecryptApp(cSettings.PassCrypt))
          End If
          cSave.Interval = cSettings.Interval
          cSave.Save()
        Else
          If My.Computer.FileSystem.FileExists(AppDataAll & "\user.config") Then My.Computer.FileSystem.DeleteFile(AppDataAll & "\user.config")
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
        If MessageBox.Show("Would you like to remove the defunct WildBlue Bandwidth Monitor from your computer?" & vbNewLine & "Your settings will be saved for use in the new Satellite Restriction Tracker.", "Uninstall WildBlue Bandwidth Monitor?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification) = DialogResult.Yes Then
          If My.Computer.FileSystem.FileExists(OldDir & "\unins000.exe") Then
            Try
              ShellEx(OldDir & "\unins000.exe", "/silent")
            Catch ex As Exception
              MessageBox.Show("Unable to run WildBlue Bandwidth Monitor uninstaller. Please uninstall manually.", "Uninstall Failed", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification)
            End Try
          Else
            Try
              My.Computer.FileSystem.DeleteDirectory(OldDir, FileIO.DeleteDirectoryOption.DeleteAllContents)
            Catch ex As Exception
              If MessageBox.Show(Application.Info.ProductName & " was unable to delete the old WildBlue Bandwidth Monitor directory." & vbNewLine & "You may delete the folder manually.", "Uninstall Failed", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification) = DialogResult.OK Then
                Try
                  Process.Start("explorer", "/select,""" & OldDir & """")
                Catch ex2 As Exception
                  Dim taskNotifier As TaskbarNotifier = Nothing
                  MakeNotifier(taskNotifier, False)
                  If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Explorer", Application.Info.ProductName & " could not navigate to """ & OldDir & """!" & vbNewLine & ex2.Message, 200, 3000, 100)
                End Try
              End If
            End Try
          End If
        End If
      End If
    End Sub
    Private Sub MyApplication_StartupNextInstance(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
      If frmMain.IsHandleCreated Then
        If Not frmMain.Visible Then frmMain.mnuRestore.PerformClick()
        e.BringToForeground = True
      End If
    End Sub
    Private Sub MyApplication_UnhandledException(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
      Try
        If e.Exception.Message.Contains("Could not load file or assembly") Then
          MessageBox.Show("A critical file is missing. Please ensure " & Application.Info.ProductName & " has been fully extracted or installed." & vbNewLine & e.Exception.Message, "Could not load File or Assembly.", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification)
          e.ExitApplication = True
        Else
          Dim frmError As New Form With
            {
              .FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow,
              .Text = "Error in " & Application.Info.ProductName,
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
          Dim sErrorText As String = Application.Info.ProductName & " has Encountered an Error"
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
              Dim sRet As String = MantisReporter.ReportIssue(e.Exception)
              If sRet = "OK" Then
                MessageBox.Show("Thank you for reporting the error." & vbNewLine & "For details, click Help to visit the Bug Report page.", My.Application.Info.Title & " Error Report Sent!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, 0, "http://bugs.realityripple.com/set_project.php?project_id=2", HelpNavigator.Index)
              Else
                Dim sErrRep As String = "http://bugs.realityripple.com/set_project.php?project_id=2&make_default=no&ref=bug_report_page.php"
                sErrRep &= "?platform=" & IIf(Environment.Is64BitProcess, "x64", IIf(Environment.Is64BitOperatingSystem, "x86-64", "x86"))
                sErrRep &= "%2526os=" & DoubleEncode(My.Computer.Info.OSFullName.Trim)
                sErrRep &= "%2526os_build=" & DoubleEncode(My.Computer.Info.OSVersion.Trim)
                Dim sSum As String = e.Exception.Message
                If sSum.Length > 80 Then sSum = sSum.Substring(0, 77) & "..."
                sErrRep &= "%2526summary=" & DoubleEncode(sSum)
                Dim sDesc As String = e.Exception.Message
                If Not String.IsNullOrEmpty(e.Exception.StackTrace) Then
                  sDesc &= vbNewLine & e.Exception.StackTrace.Substring(0, e.Exception.StackTrace.IndexOf(vbCr))
                Else
                  If Not String.IsNullOrEmpty(e.Exception.Source) Then
                    sDesc &= vbNewLine & " @ " & e.Exception.Source
                    If e.Exception.TargetSite IsNot Nothing Then sDesc &= "." & e.Exception.TargetSite.Name
                  Else
                    If e.Exception.TargetSite IsNot Nothing Then sDesc &= vbNewLine & " @ " & e.Exception.TargetSite.Name
                  End If
                End If
                sDesc &= vbNewLine & "Version " & Windows.Forms.Application.ProductVersion.Trim
                sErrRep &= "%2526description=" & DoubleEncode(sDesc)
                MessageBox.Show(sRet & vbNewLine & "If you would like to report the error manually, please click the Help button below.", My.Application.Info.Title & " Error Report Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2, Nothing, sErrRep, HelpNavigator.Index)
              End If
            Case DialogResult.Ignore : e.ExitApplication = False
            Case DialogResult.Abort : e.ExitApplication = True
            Case DialogResult.Cancel : e.ExitApplication = False
          End Select
        End If
      Catch ex As Exception
        MessageBox.Show("There was an error while handling an error..." & vbNewLine & ex.Message & vbNewLine & vbNewLine & "Original Error: " & e.Exception.Message, Application.Info.Title & " Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification)
      End Try
    End Sub
    Private Function DoubleEncode(inString As String) As String
      Return PercentEncode(PercentEncode(inString))
    End Function
  End Class
End Namespace
