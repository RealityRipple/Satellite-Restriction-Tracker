<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWizard
  Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWizard))
    Me.pnlWizard = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlButtons = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdFAQ = New System.Windows.Forms.Button()
    Me.cmdClose = New System.Windows.Forms.Button()
    Me.cmdNext = New System.Windows.Forms.Button()
    Me.cmdPrevious = New System.Windows.Forms.Button()
    Me.lblActivity = New System.Windows.Forms.Label()
    Me.pnlContent = New System.Windows.Forms.TableLayoutPanel()
    Me.pctLeftBox = New System.Windows.Forms.PictureBox()
    Me.tbsWizardPages = New System.Windows.Forms.TabControl()
    Me.tabWelcome = New System.Windows.Forms.TabPage()
    Me.pnlWelcome = New System.Windows.Forms.TableLayoutPanel()
    Me.lblWelcomeTitle = New System.Windows.Forms.Label()
    Me.lblWelcomeText = New System.Windows.Forms.Label()
    Me.tabAccount = New System.Windows.Forms.TabPage()
    Me.pnlAccount = New System.Windows.Forms.TableLayoutPanel()
    Me.lblAccountName = New System.Windows.Forms.Label()
    Me.lblAccountProvider = New System.Windows.Forms.Label()
    Me.lblAccountHost = New System.Windows.Forms.Label()
    Me.txtAccountUsername = New System.Windows.Forms.TextBox()
    Me.lblAccountPassText = New System.Windows.Forms.Label()
    Me.txtAccountPass = New System.Windows.Forms.TextBox()
    Me.lblAccountPass = New System.Windows.Forms.Label()
    Me.lblAccountUsername = New System.Windows.Forms.Label()
    Me.cmbAccountHost = New System.Windows.Forms.ComboBox()
    Me.lblAccountTitle = New System.Windows.Forms.Label()
    Me.lnAccountSpace1 = New RestrictionTracker.LineBreak()
    Me.lnAccountSpace2 = New RestrictionTracker.LineBreak()
    Me.tabService = New System.Windows.Forms.TabPage()
    Me.pnlService = New System.Windows.Forms.TableLayoutPanel()
    Me.lblServiceTitle = New System.Windows.Forms.Label()
    Me.lblRemoteText = New System.Windows.Forms.Label()
    Me.optRemote = New System.Windows.Forms.RadioButton()
    Me.pnlKey = New System.Windows.Forms.TableLayoutPanel()
    Me.txtKey1 = New System.Windows.Forms.TextBox()
    Me.txtKey2 = New System.Windows.Forms.TextBox()
    Me.txtKey3 = New System.Windows.Forms.TextBox()
    Me.txtKey4 = New System.Windows.Forms.TextBox()
    Me.txtKey5 = New System.Windows.Forms.TextBox()
    Me.lblLocalText = New System.Windows.Forms.Label()
    Me.optLocal = New System.Windows.Forms.RadioButton()
    Me.optNone = New System.Windows.Forms.RadioButton()
    Me.lblLocal = New System.Windows.Forms.Label()
    Me.lblNone = New System.Windows.Forms.Label()
    Me.txtSignUp = New RestrictionTracker.LinkLabel()
    Me.lnServiceSpace2 = New RestrictionTracker.LineBreak()
    Me.lnServiceSpace1 = New RestrictionTracker.LineBreak()
    Me.tabDisplay = New System.Windows.Forms.TabPage()
    Me.pnlDisplay = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlOverAlert = New System.Windows.Forms.TableLayoutPanel()
    Me.chkOverAlert = New System.Windows.Forms.CheckBox()
    Me.txtOverSize = New System.Windows.Forms.NumericUpDown()
    Me.lblOverSize = New System.Windows.Forms.Label()
    Me.txtOverTime = New System.Windows.Forms.NumericUpDown()
    Me.lblOverTime = New System.Windows.Forms.Label()
    Me.lblDisplayTitle = New System.Windows.Forms.Label()
    Me.lblDisplayAccuracy = New System.Windows.Forms.Label()
    Me.pnlDisplayAccuracy = New System.Windows.Forms.TableLayoutPanel()
    Me.optAccuracy0 = New System.Windows.Forms.RadioButton()
    Me.optAccuracy1 = New System.Windows.Forms.RadioButton()
    Me.optAccuracy2 = New System.Windows.Forms.RadioButton()
    Me.optAccuracy3 = New System.Windows.Forms.RadioButton()
    Me.chkDisplayScale = New System.Windows.Forms.CheckBox()
    Me.lblDisplayOver = New System.Windows.Forms.Label()
    Me.lnDisplaySpace1 = New RestrictionTracker.LineBreak()
    Me.lnDisplaySpace2 = New RestrictionTracker.LineBreak()
    Me.tabFinished = New System.Windows.Forms.TabPage()
    Me.pnlFinished = New System.Windows.Forms.TableLayoutPanel()
    Me.lblFinishedTitle = New System.Windows.Forms.Label()
    Me.lblFinishedText = New System.Windows.Forms.Label()
    Me.pctHeader = New System.Windows.Forms.PictureBox()
    Me.tmrDraw = New System.Windows.Forms.Timer(Me.components)
    Me.ttWizard = New RestrictionTracker.ToolTip(Me.components)
    Me.pnlWizard.SuspendLayout()
    Me.pnlButtons.SuspendLayout()
    Me.pnlContent.SuspendLayout()
    CType(Me.pctLeftBox, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.tbsWizardPages.SuspendLayout()
    Me.tabWelcome.SuspendLayout()
    Me.pnlWelcome.SuspendLayout()
    Me.tabAccount.SuspendLayout()
    Me.pnlAccount.SuspendLayout()
    Me.tabService.SuspendLayout()
    Me.pnlService.SuspendLayout()
    Me.pnlKey.SuspendLayout()
    Me.tabDisplay.SuspendLayout()
    Me.pnlDisplay.SuspendLayout()
    Me.pnlOverAlert.SuspendLayout()
    CType(Me.txtOverSize, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtOverTime, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlDisplayAccuracy.SuspendLayout()
    Me.tabFinished.SuspendLayout()
    Me.pnlFinished.SuspendLayout()
    CType(Me.pctHeader, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'pnlWizard
    '
    Me.pnlWizard.AutoSize = True
    Me.pnlWizard.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlWizard.ColumnCount = 1
    Me.pnlWizard.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlWizard.Controls.Add(Me.pnlButtons, 0, 2)
    Me.pnlWizard.Controls.Add(Me.pnlContent, 0, 1)
    Me.pnlWizard.Controls.Add(Me.pctHeader, 0, 0)
    Me.pnlWizard.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlWizard.Location = New System.Drawing.Point(0, 0)
    Me.pnlWizard.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlWizard.Name = "pnlWizard"
    Me.pnlWizard.RowCount = 3
    Me.pnlWizard.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlWizard.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlWizard.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlWizard.Size = New System.Drawing.Size(614, 413)
    Me.pnlWizard.TabIndex = 0
    '
    'pnlButtons
    '
    Me.pnlButtons.AutoSize = True
    Me.pnlButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlButtons.ColumnCount = 5
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.Controls.Add(Me.cmdFAQ, 0, 0)
    Me.pnlButtons.Controls.Add(Me.cmdClose, 4, 0)
    Me.pnlButtons.Controls.Add(Me.cmdNext, 3, 0)
    Me.pnlButtons.Controls.Add(Me.cmdPrevious, 2, 0)
    Me.pnlButtons.Controls.Add(Me.lblActivity, 1, 0)
    Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlButtons.Location = New System.Drawing.Point(0, 384)
    Me.pnlButtons.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.RowCount = 1
    Me.pnlButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlButtons.Size = New System.Drawing.Size(614, 29)
    Me.pnlButtons.TabIndex = 0
    '
    'cmdFAQ
    '
    Me.cmdFAQ.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdFAQ.Location = New System.Drawing.Point(3, 3)
    Me.cmdFAQ.Name = "cmdFAQ"
    Me.cmdFAQ.Size = New System.Drawing.Size(75, 23)
    Me.cmdFAQ.TabIndex = 1
    Me.cmdFAQ.Text = "F. A. &Q."
    Me.ttWizard.SetTooltip(Me.cmdFAQ, "View the Frequently Asked Questions on the Satellite Restriction Tracker Website." & _
        "")
    Me.cmdFAQ.UseVisualStyleBackColor = True
    '
    'cmdClose
    '
    Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdClose.Location = New System.Drawing.Point(536, 3)
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.Size = New System.Drawing.Size(75, 23)
    Me.cmdClose.TabIndex = 3
    Me.cmdClose.Text = "Cancel"
    Me.ttWizard.SetTooltip(Me.cmdClose, "Close the Configuration Wizard.")
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'cmdNext
    '
    Me.cmdNext.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdNext.Location = New System.Drawing.Point(455, 3)
    Me.cmdNext.Name = "cmdNext"
    Me.cmdNext.Size = New System.Drawing.Size(75, 23)
    Me.cmdNext.TabIndex = 0
    Me.cmdNext.Text = "Next >>"
    Me.ttWizard.SetTooltip(Me.cmdNext, "Proceed to the next screen.")
    Me.cmdNext.UseVisualStyleBackColor = True
    '
    'cmdPrevious
    '
    Me.cmdPrevious.Enabled = False
    Me.cmdPrevious.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdPrevious.Location = New System.Drawing.Point(374, 3)
    Me.cmdPrevious.Name = "cmdPrevious"
    Me.cmdPrevious.Size = New System.Drawing.Size(75, 23)
    Me.cmdPrevious.TabIndex = 2
    Me.cmdPrevious.Text = "<< &Previous"
    Me.ttWizard.SetTooltip(Me.cmdPrevious, "Return to the previous screen.")
    Me.cmdPrevious.UseVisualStyleBackColor = True
    '
    'lblActivity
    '
    Me.lblActivity.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblActivity.AutoSize = True
    Me.lblActivity.Location = New System.Drawing.Point(93, 8)
    Me.lblActivity.Margin = New System.Windows.Forms.Padding(12, 0, 3, 0)
    Me.lblActivity.Name = "lblActivity"
    Me.lblActivity.Size = New System.Drawing.Size(0, 13)
    Me.lblActivity.TabIndex = 4
    '
    'pnlContent
    '
    Me.pnlContent.ColumnCount = 2
    Me.pnlContent.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150.0!))
    Me.pnlContent.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlContent.Controls.Add(Me.pctLeftBox, 0, 0)
    Me.pnlContent.Controls.Add(Me.tbsWizardPages, 1, 0)
    Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlContent.Location = New System.Drawing.Point(0, 35)
    Me.pnlContent.Margin = New System.Windows.Forms.Padding(0, 0, 3, 3)
    Me.pnlContent.Name = "pnlContent"
    Me.pnlContent.RowCount = 1
    Me.pnlContent.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlContent.Size = New System.Drawing.Size(611, 346)
    Me.pnlContent.TabIndex = 2
    '
    'pctLeftBox
    '
    Me.pctLeftBox.BackColor = System.Drawing.SystemColors.Window
    Me.pctLeftBox.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pctLeftBox.Image = Global.RestrictionTracker.My.Resources.Resources.wizWelcome
    Me.pctLeftBox.Location = New System.Drawing.Point(0, 0)
    Me.pctLeftBox.Margin = New System.Windows.Forms.Padding(0)
    Me.pctLeftBox.Name = "pctLeftBox"
    Me.pctLeftBox.Size = New System.Drawing.Size(150, 346)
    Me.pctLeftBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pctLeftBox.TabIndex = 0
    Me.pctLeftBox.TabStop = False
    '
    'tbsWizardPages
    '
    Me.tbsWizardPages.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
    Me.tbsWizardPages.Controls.Add(Me.tabWelcome)
    Me.tbsWizardPages.Controls.Add(Me.tabAccount)
    Me.tbsWizardPages.Controls.Add(Me.tabService)
    Me.tbsWizardPages.Controls.Add(Me.tabDisplay)
    Me.tbsWizardPages.Controls.Add(Me.tabFinished)
    Me.tbsWizardPages.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tbsWizardPages.ItemSize = New System.Drawing.Size(80, 21)
    Me.tbsWizardPages.Location = New System.Drawing.Point(153, 3)
    Me.tbsWizardPages.Name = "tbsWizardPages"
    Me.tbsWizardPages.SelectedIndex = 0
    Me.tbsWizardPages.Size = New System.Drawing.Size(455, 340)
    Me.tbsWizardPages.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
    Me.tbsWizardPages.TabIndex = 1
    '
    'tabWelcome
    '
    Me.tabWelcome.Controls.Add(Me.pnlWelcome)
    Me.tabWelcome.Location = New System.Drawing.Point(4, 25)
    Me.tabWelcome.Name = "tabWelcome"
    Me.tabWelcome.Size = New System.Drawing.Size(447, 311)
    Me.tabWelcome.TabIndex = 0
    Me.tabWelcome.Text = "Welcome"
    Me.tabWelcome.UseVisualStyleBackColor = True
    '
    'pnlWelcome
    '
    Me.pnlWelcome.ColumnCount = 1
    Me.pnlWelcome.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlWelcome.Controls.Add(Me.lblWelcomeTitle, 0, 0)
    Me.pnlWelcome.Controls.Add(Me.lblWelcomeText, 0, 1)
    Me.pnlWelcome.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlWelcome.Location = New System.Drawing.Point(0, 0)
    Me.pnlWelcome.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlWelcome.Name = "pnlWelcome"
    Me.pnlWelcome.RowCount = 2
    Me.pnlWelcome.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlWelcome.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlWelcome.Size = New System.Drawing.Size(447, 311)
    Me.pnlWelcome.TabIndex = 0
    '
    'lblWelcomeTitle
    '
    Me.lblWelcomeTitle.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblWelcomeTitle.AutoSize = True
    Me.lblWelcomeTitle.Font = New System.Drawing.Font("Times New Roman", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblWelcomeTitle.Location = New System.Drawing.Point(71, 3)
    Me.lblWelcomeTitle.Margin = New System.Windows.Forms.Padding(3)
    Me.lblWelcomeTitle.Name = "lblWelcomeTitle"
    Me.lblWelcomeTitle.Size = New System.Drawing.Size(304, 78)
    Me.lblWelcomeTitle.TabIndex = 0
    Me.lblWelcomeTitle.Text = "Welcome to the" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Satellite Restriction Tracker" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Configuration Wizard!"
    Me.lblWelcomeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'lblWelcomeText
    '
    Me.lblWelcomeText.AutoSize = True
    Me.lblWelcomeText.Font = New System.Drawing.Font("Times New Roman", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblWelcomeText.Location = New System.Drawing.Point(3, 87)
    Me.lblWelcomeText.Margin = New System.Windows.Forms.Padding(3)
    Me.lblWelcomeText.Name = "lblWelcomeText"
    Me.lblWelcomeText.Size = New System.Drawing.Size(441, 204)
    Me.lblWelcomeText.TabIndex = 1
    Me.lblWelcomeText.Text = resources.GetString("lblWelcomeText.Text")
    '
    'tabAccount
    '
    Me.tabAccount.Controls.Add(Me.pnlAccount)
    Me.tabAccount.Location = New System.Drawing.Point(4, 25)
    Me.tabAccount.Name = "tabAccount"
    Me.tabAccount.Size = New System.Drawing.Size(447, 311)
    Me.tabAccount.TabIndex = 1
    Me.tabAccount.Text = "Account"
    Me.tabAccount.UseVisualStyleBackColor = True
    '
    'pnlAccount
    '
    Me.pnlAccount.ColumnCount = 2
    Me.pnlAccount.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAccount.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAccount.Controls.Add(Me.lblAccountName, 0, 4)
    Me.pnlAccount.Controls.Add(Me.lblAccountProvider, 0, 1)
    Me.pnlAccount.Controls.Add(Me.lblAccountHost, 0, 2)
    Me.pnlAccount.Controls.Add(Me.txtAccountUsername, 1, 5)
    Me.pnlAccount.Controls.Add(Me.lblAccountPassText, 0, 7)
    Me.pnlAccount.Controls.Add(Me.txtAccountPass, 1, 8)
    Me.pnlAccount.Controls.Add(Me.lblAccountPass, 0, 8)
    Me.pnlAccount.Controls.Add(Me.lblAccountUsername, 0, 5)
    Me.pnlAccount.Controls.Add(Me.cmbAccountHost, 1, 2)
    Me.pnlAccount.Controls.Add(Me.lblAccountTitle, 0, 0)
    Me.pnlAccount.Controls.Add(Me.lnAccountSpace1, 0, 3)
    Me.pnlAccount.Controls.Add(Me.lnAccountSpace2, 0, 6)
    Me.pnlAccount.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlAccount.Location = New System.Drawing.Point(0, 0)
    Me.pnlAccount.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlAccount.Name = "pnlAccount"
    Me.pnlAccount.RowCount = 9
    Me.pnlAccount.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAccount.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAccount.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAccount.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlAccount.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAccount.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAccount.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlAccount.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAccount.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAccount.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlAccount.Size = New System.Drawing.Size(447, 311)
    Me.pnlAccount.TabIndex = 1
    '
    'lblAccountName
    '
    Me.lblAccountName.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAccountName.AutoSize = True
    Me.pnlAccount.SetColumnSpan(Me.lblAccountName, 2)
    Me.lblAccountName.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblAccountName.Location = New System.Drawing.Point(3, 127)
    Me.lblAccountName.Margin = New System.Windows.Forms.Padding(3)
    Me.lblAccountName.Name = "lblAccountName"
    Me.lblAccountName.Size = New System.Drawing.Size(432, 48)
    Me.lblAccountName.TabIndex = 3
    Me.lblAccountName.Text = resources.GetString("lblAccountName.Text")
    '
    'lblAccountProvider
    '
    Me.lblAccountProvider.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAccountProvider.AutoSize = True
    Me.pnlAccount.SetColumnSpan(Me.lblAccountProvider, 2)
    Me.lblAccountProvider.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblAccountProvider.Location = New System.Drawing.Point(3, 35)
    Me.lblAccountProvider.Margin = New System.Windows.Forms.Padding(3)
    Me.lblAccountProvider.Name = "lblAccountProvider"
    Me.lblAccountProvider.Size = New System.Drawing.Size(434, 48)
    Me.lblAccountProvider.TabIndex = 0
    Me.lblAccountProvider.Text = resources.GetString("lblAccountProvider.Text")
    '
    'lblAccountHost
    '
    Me.lblAccountHost.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAccountHost.AutoSize = True
    Me.lblAccountHost.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblAccountHost.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblAccountHost.Location = New System.Drawing.Point(3, 91)
    Me.lblAccountHost.Name = "lblAccountHost"
    Me.lblAccountHost.Size = New System.Drawing.Size(65, 17)
    Me.lblAccountHost.TabIndex = 1
    Me.lblAccountHost.Text = "P&rovider:"
    Me.lblAccountHost.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'txtAccountUsername
    '
    Me.txtAccountUsername.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtAccountUsername.Location = New System.Drawing.Point(84, 181)
    Me.txtAccountUsername.Name = "txtAccountUsername"
    Me.txtAccountUsername.Size = New System.Drawing.Size(360, 20)
    Me.txtAccountUsername.TabIndex = 5
    Me.ttWizard.SetTooltip(Me.txtAccountUsername, "Enter your ViaSat provider Account Username.")
    '
    'lblAccountPassText
    '
    Me.lblAccountPassText.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAccountPassText.AutoSize = True
    Me.pnlAccount.SetColumnSpan(Me.lblAccountPassText, 2)
    Me.lblAccountPassText.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblAccountPassText.Location = New System.Drawing.Point(3, 218)
    Me.lblAccountPassText.Margin = New System.Windows.Forms.Padding(3)
    Me.lblAccountPassText.Name = "lblAccountPassText"
    Me.lblAccountPassText.Size = New System.Drawing.Size(440, 64)
    Me.lblAccountPassText.TabIndex = 6
    Me.lblAccountPassText.Text = resources.GetString("lblAccountPassText.Text")
    '
    'txtAccountPass
    '
    Me.txtAccountPass.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtAccountPass.Location = New System.Drawing.Point(84, 288)
    Me.txtAccountPass.Name = "txtAccountPass"
    Me.txtAccountPass.Size = New System.Drawing.Size(360, 20)
    Me.txtAccountPass.TabIndex = 8
    Me.ttWizard.SetTooltip(Me.txtAccountPass, "Enter your ViaSat provider Account Password.")
    Me.txtAccountPass.UseSystemPasswordChar = True
    '
    'lblAccountPass
    '
    Me.lblAccountPass.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAccountPass.AutoSize = True
    Me.lblAccountPass.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblAccountPass.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblAccountPass.Location = New System.Drawing.Point(3, 289)
    Me.lblAccountPass.Name = "lblAccountPass"
    Me.lblAccountPass.Size = New System.Drawing.Size(69, 17)
    Me.lblAccountPass.TabIndex = 7
    Me.lblAccountPass.Text = "Pass&word:"
    Me.lblAccountPass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'lblAccountUsername
    '
    Me.lblAccountUsername.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAccountUsername.AutoSize = True
    Me.lblAccountUsername.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblAccountUsername.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblAccountUsername.Location = New System.Drawing.Point(3, 182)
    Me.lblAccountUsername.Name = "lblAccountUsername"
    Me.lblAccountUsername.Size = New System.Drawing.Size(75, 17)
    Me.lblAccountUsername.TabIndex = 4
    Me.lblAccountUsername.Text = "&Username:"
    Me.lblAccountUsername.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'cmbAccountHost
    '
    Me.cmbAccountHost.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmbAccountHost.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
    Me.cmbAccountHost.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
    Me.cmbAccountHost.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmbAccountHost.FormattingEnabled = True
    Me.cmbAccountHost.Location = New System.Drawing.Point(84, 89)
    Me.cmbAccountHost.Name = "cmbAccountHost"
    Me.cmbAccountHost.Size = New System.Drawing.Size(360, 21)
    Me.cmbAccountHost.TabIndex = 2
    Me.ttWizard.SetTooltip(Me.cmbAccountHost, "Choose your ViaSat provider website. If your provider is not listed, you can ente" & _
        "r the domain name of your meter page or ViaSat E-Mail Address.")
    '
    'lblAccountTitle
    '
    Me.lblAccountTitle.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblAccountTitle.AutoSize = True
    Me.pnlAccount.SetColumnSpan(Me.lblAccountTitle, 2)
    Me.lblAccountTitle.Font = New System.Drawing.Font("Times New Roman", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblAccountTitle.Location = New System.Drawing.Point(67, 3)
    Me.lblAccountTitle.Margin = New System.Windows.Forms.Padding(3)
    Me.lblAccountTitle.Name = "lblAccountTitle"
    Me.lblAccountTitle.Size = New System.Drawing.Size(313, 26)
    Me.lblAccountTitle.TabIndex = 9
    Me.lblAccountTitle.Text = "Your ViaSat Account Settings"
    Me.lblAccountTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'lnAccountSpace1
    '
    Me.lnAccountSpace1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnAccountSpace1.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnAccountSpace1.CausesValidation = False
    Me.pnlAccount.SetColumnSpan(Me.lnAccountSpace1, 2)
    Me.lnAccountSpace1.Location = New System.Drawing.Point(3, 116)
    Me.lnAccountSpace1.Name = "lnAccountSpace1"
    Me.lnAccountSpace1.Padding = New System.Windows.Forms.Padding(1)
    Me.lnAccountSpace1.Size = New System.Drawing.Size(441, 4)
    Me.lnAccountSpace1.TabIndex = 10
    Me.lnAccountSpace1.TabStop = False
    '
    'lnAccountSpace2
    '
    Me.lnAccountSpace2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnAccountSpace2.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnAccountSpace2.CausesValidation = False
    Me.pnlAccount.SetColumnSpan(Me.lnAccountSpace2, 2)
    Me.lnAccountSpace2.Location = New System.Drawing.Point(3, 207)
    Me.lnAccountSpace2.Name = "lnAccountSpace2"
    Me.lnAccountSpace2.Padding = New System.Windows.Forms.Padding(1)
    Me.lnAccountSpace2.Size = New System.Drawing.Size(441, 4)
    Me.lnAccountSpace2.TabIndex = 11
    Me.lnAccountSpace2.TabStop = False
    '
    'tabService
    '
    Me.tabService.Controls.Add(Me.pnlService)
    Me.tabService.Location = New System.Drawing.Point(4, 25)
    Me.tabService.Name = "tabService"
    Me.tabService.Size = New System.Drawing.Size(447, 311)
    Me.tabService.TabIndex = 2
    Me.tabService.Text = "Service"
    Me.tabService.UseVisualStyleBackColor = True
    '
    'pnlService
    '
    Me.pnlService.ColumnCount = 3
    Me.pnlService.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlService.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlService.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlService.Controls.Add(Me.lblServiceTitle, 0, 0)
    Me.pnlService.Controls.Add(Me.lblRemoteText, 0, 1)
    Me.pnlService.Controls.Add(Me.optRemote, 0, 2)
    Me.pnlService.Controls.Add(Me.pnlKey, 1, 2)
    Me.pnlService.Controls.Add(Me.lblLocalText, 0, 4)
    Me.pnlService.Controls.Add(Me.optLocal, 0, 5)
    Me.pnlService.Controls.Add(Me.optNone, 0, 7)
    Me.pnlService.Controls.Add(Me.lblLocal, 1, 5)
    Me.pnlService.Controls.Add(Me.lblNone, 1, 7)
    Me.pnlService.Controls.Add(Me.txtSignUp, 2, 2)
    Me.pnlService.Controls.Add(Me.lnServiceSpace2, 0, 6)
    Me.pnlService.Controls.Add(Me.lnServiceSpace1, 0, 3)
    Me.pnlService.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlService.Location = New System.Drawing.Point(0, 0)
    Me.pnlService.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlService.Name = "pnlService"
    Me.pnlService.RowCount = 8
    Me.pnlService.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlService.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlService.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlService.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlService.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlService.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlService.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlService.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlService.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlService.Size = New System.Drawing.Size(447, 311)
    Me.pnlService.TabIndex = 0
    '
    'lblServiceTitle
    '
    Me.lblServiceTitle.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblServiceTitle.AutoSize = True
    Me.pnlService.SetColumnSpan(Me.lblServiceTitle, 3)
    Me.lblServiceTitle.Font = New System.Drawing.Font("Times New Roman", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblServiceTitle.Location = New System.Drawing.Point(80, 3)
    Me.lblServiceTitle.Margin = New System.Windows.Forms.Padding(3)
    Me.lblServiceTitle.Name = "lblServiceTitle"
    Me.lblServiceTitle.Size = New System.Drawing.Size(286, 26)
    Me.lblServiceTitle.TabIndex = 13
    Me.lblServiceTitle.Text = "Keep Track of Your Usage!"
    Me.lblServiceTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'lblRemoteText
    '
    Me.lblRemoteText.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblRemoteText.AutoSize = True
    Me.pnlService.SetColumnSpan(Me.lblRemoteText, 3)
    Me.lblRemoteText.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblRemoteText.Location = New System.Drawing.Point(3, 35)
    Me.lblRemoteText.Margin = New System.Windows.Forms.Padding(3)
    Me.lblRemoteText.Name = "lblRemoteText"
    Me.lblRemoteText.Size = New System.Drawing.Size(435, 80)
    Me.lblRemoteText.TabIndex = 2
    Me.lblRemoteText.Text = resources.GetString("lblRemoteText.Text")
    '
    'optRemote
    '
    Me.optRemote.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.optRemote.AutoSize = True
    Me.optRemote.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.optRemote.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.optRemote.Location = New System.Drawing.Point(3, 121)
    Me.optRemote.Name = "optRemote"
    Me.optRemote.Size = New System.Drawing.Size(129, 22)
    Me.optRemote.TabIndex = 3
    Me.optRemote.Text = "&Remote Service"
    Me.ttWizard.SetTooltip(Me.optRemote, "Use the Remote Usage Service to keep track of your usage 24/7, for only $15 per y" & _
        "ear!")
    Me.optRemote.UseVisualStyleBackColor = True
    '
    'pnlKey
    '
    Me.pnlKey.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.pnlKey.AutoSize = True
    Me.pnlKey.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlKey.ColumnCount = 5
    Me.pnlKey.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlKey.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlKey.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlKey.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlKey.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlKey.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlKey.Controls.Add(Me.txtKey1, 0, 0)
    Me.pnlKey.Controls.Add(Me.txtKey2, 1, 0)
    Me.pnlKey.Controls.Add(Me.txtKey3, 2, 0)
    Me.pnlKey.Controls.Add(Me.txtKey4, 3, 0)
    Me.pnlKey.Controls.Add(Me.txtKey5, 4, 0)
    Me.pnlKey.Location = New System.Drawing.Point(135, 120)
    Me.pnlKey.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlKey.Name = "pnlKey"
    Me.pnlKey.RowCount = 1
    Me.pnlKey.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlKey.Size = New System.Drawing.Size(210, 24)
    Me.pnlKey.TabIndex = 4
    '
    'txtKey1
    '
    Me.txtKey1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtKey1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
    Me.txtKey1.Enabled = False
    Me.txtKey1.Font = New System.Drawing.Font("Lucida Console", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtKey1.Location = New System.Drawing.Point(3, 3)
    Me.txtKey1.Margin = New System.Windows.Forms.Padding(3, 3, 1, 3)
    Me.txtKey1.MaxLength = 6
    Me.txtKey1.Name = "txtKey1"
    Me.txtKey1.Size = New System.Drawing.Size(48, 18)
    Me.txtKey1.TabIndex = 0
    Me.txtKey1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
    Me.ttWizard.SetTooltip(Me.txtKey1, "Your Remote Usage Service Product Key.")
    '
    'txtKey2
    '
    Me.txtKey2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtKey2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
    Me.txtKey2.Enabled = False
    Me.txtKey2.Font = New System.Drawing.Font("Lucida Console", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtKey2.Location = New System.Drawing.Point(53, 3)
    Me.txtKey2.Margin = New System.Windows.Forms.Padding(1, 3, 1, 3)
    Me.txtKey2.MaxLength = 4
    Me.txtKey2.Name = "txtKey2"
    Me.txtKey2.Size = New System.Drawing.Size(34, 18)
    Me.txtKey2.TabIndex = 1
    Me.txtKey2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
    Me.ttWizard.SetTooltip(Me.txtKey2, "Your Remote Usage Service Product Key.")
    '
    'txtKey3
    '
    Me.txtKey3.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtKey3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
    Me.txtKey3.Enabled = False
    Me.txtKey3.Font = New System.Drawing.Font("Lucida Console", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtKey3.Location = New System.Drawing.Point(89, 3)
    Me.txtKey3.Margin = New System.Windows.Forms.Padding(1, 3, 1, 3)
    Me.txtKey3.MaxLength = 4
    Me.txtKey3.Name = "txtKey3"
    Me.txtKey3.Size = New System.Drawing.Size(34, 18)
    Me.txtKey3.TabIndex = 2
    Me.txtKey3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
    Me.ttWizard.SetTooltip(Me.txtKey3, "Your Remote Usage Service Product Key.")
    '
    'txtKey4
    '
    Me.txtKey4.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtKey4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
    Me.txtKey4.Enabled = False
    Me.txtKey4.Font = New System.Drawing.Font("Lucida Console", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtKey4.Location = New System.Drawing.Point(125, 3)
    Me.txtKey4.Margin = New System.Windows.Forms.Padding(1, 3, 1, 3)
    Me.txtKey4.MaxLength = 4
    Me.txtKey4.Name = "txtKey4"
    Me.txtKey4.Size = New System.Drawing.Size(34, 18)
    Me.txtKey4.TabIndex = 3
    Me.txtKey4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
    Me.ttWizard.SetTooltip(Me.txtKey4, "Your Remote Usage Service Product Key.")
    '
    'txtKey5
    '
    Me.txtKey5.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtKey5.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
    Me.txtKey5.Enabled = False
    Me.txtKey5.Font = New System.Drawing.Font("Lucida Console", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtKey5.Location = New System.Drawing.Point(161, 3)
    Me.txtKey5.Margin = New System.Windows.Forms.Padding(1, 3, 1, 3)
    Me.txtKey5.MaxLength = 6
    Me.txtKey5.Name = "txtKey5"
    Me.txtKey5.Size = New System.Drawing.Size(48, 18)
    Me.txtKey5.TabIndex = 4
    Me.txtKey5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
    Me.ttWizard.SetTooltip(Me.txtKey5, "Your Remote Usage Service Product Key.")
    '
    'lblLocalText
    '
    Me.lblLocalText.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblLocalText.AutoSize = True
    Me.pnlService.SetColumnSpan(Me.lblLocalText, 3)
    Me.lblLocalText.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblLocalText.Location = New System.Drawing.Point(3, 168)
    Me.lblLocalText.Margin = New System.Windows.Forms.Padding(3)
    Me.lblLocalText.Name = "lblLocalText"
    Me.lblLocalText.Size = New System.Drawing.Size(441, 64)
    Me.lblLocalText.TabIndex = 6
    Me.lblLocalText.Text = resources.GetString("lblLocalText.Text")
    '
    'optLocal
    '
    Me.optLocal.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.optLocal.AutoSize = True
    Me.optLocal.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.optLocal.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.optLocal.Location = New System.Drawing.Point(3, 238)
    Me.optLocal.Name = "optLocal"
    Me.optLocal.Size = New System.Drawing.Size(114, 22)
    Me.optLocal.TabIndex = 7
    Me.optLocal.Text = "&Local Service"
    Me.ttWizard.SetTooltip(Me.optLocal, "Use the Restriction Logger Service to keep track of your usage whenever you are o" & _
        "nline.")
    Me.optLocal.UseVisualStyleBackColor = True
    '
    'optNone
    '
    Me.optNone.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.optNone.AutoSize = True
    Me.optNone.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.optNone.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.optNone.Location = New System.Drawing.Point(3, 285)
    Me.optNone.Name = "optNone"
    Me.optNone.Size = New System.Drawing.Size(118, 22)
    Me.optNone.TabIndex = 0
    Me.optNone.Text = "&No Thank You"
    Me.ttWizard.SetTooltip(Me.optNone, "Satellite Restriction Tracker will only gather usage data when the program is run" & _
        "ning.")
    Me.optNone.UseVisualStyleBackColor = True
    '
    'lblLocal
    '
    Me.lblLocal.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblLocal.AutoSize = True
    Me.pnlService.SetColumnSpan(Me.lblLocal, 2)
    Me.lblLocal.Enabled = False
    Me.lblLocal.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblLocal.Location = New System.Drawing.Point(138, 241)
    Me.lblLocal.Name = "lblLocal"
    Me.lblLocal.Size = New System.Drawing.Size(261, 16)
    Me.lblLocal.TabIndex = 8
    Me.lblLocal.Text = "Gather data whenever the computer is online"
    '
    'lblNone
    '
    Me.lblNone.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblNone.AutoSize = True
    Me.pnlService.SetColumnSpan(Me.lblNone, 2)
    Me.lblNone.Enabled = False
    Me.lblNone.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblNone.Location = New System.Drawing.Point(138, 288)
    Me.lblNone.Name = "lblNone"
    Me.lblNone.Size = New System.Drawing.Size(265, 16)
    Me.lblNone.TabIndex = 1
    Me.lblNone.Text = "Just gather data when the program is running"
    '
    'txtSignUp
    '
    Me.txtSignUp.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.txtSignUp.AutoSize = True
    Me.txtSignUp.Cursor = System.Windows.Forms.Cursors.Hand
    Me.txtSignUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
    Me.txtSignUp.ForeColor = System.Drawing.Color.MediumBlue
    Me.txtSignUp.Location = New System.Drawing.Point(355, 125)
    Me.txtSignUp.Name = "txtSignUp"
    Me.txtSignUp.Size = New System.Drawing.Size(81, 13)
    Me.txtSignUp.TabIndex = 14
    Me.txtSignUp.Text = "&Sign Up Today!"
    Me.ttWizard.SetTooltip(Me.txtSignUp, "Sign up for the Remote Usage Service on the Satellite Restriction Tracker website" & _
        ".")
    '
    'lnServiceSpace2
    '
    Me.lnServiceSpace2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnServiceSpace2.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnServiceSpace2.CausesValidation = False
    Me.pnlService.SetColumnSpan(Me.lnServiceSpace2, 3)
    Me.lnServiceSpace2.Location = New System.Drawing.Point(3, 270)
    Me.lnServiceSpace2.Name = "lnServiceSpace2"
    Me.lnServiceSpace2.Padding = New System.Windows.Forms.Padding(1)
    Me.lnServiceSpace2.Size = New System.Drawing.Size(441, 4)
    Me.lnServiceSpace2.TabIndex = 15
    Me.lnServiceSpace2.TabStop = False
    '
    'lnServiceSpace1
    '
    Me.lnServiceSpace1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnServiceSpace1.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnServiceSpace1.CausesValidation = False
    Me.pnlService.SetColumnSpan(Me.lnServiceSpace1, 3)
    Me.lnServiceSpace1.Location = New System.Drawing.Point(3, 153)
    Me.lnServiceSpace1.Name = "lnServiceSpace1"
    Me.lnServiceSpace1.Padding = New System.Windows.Forms.Padding(1)
    Me.lnServiceSpace1.Size = New System.Drawing.Size(441, 4)
    Me.lnServiceSpace1.TabIndex = 16
    Me.lnServiceSpace1.TabStop = False
    '
    'tabDisplay
    '
    Me.tabDisplay.Controls.Add(Me.pnlDisplay)
    Me.tabDisplay.Location = New System.Drawing.Point(4, 25)
    Me.tabDisplay.Name = "tabDisplay"
    Me.tabDisplay.Size = New System.Drawing.Size(447, 311)
    Me.tabDisplay.TabIndex = 3
    Me.tabDisplay.Text = "Display"
    Me.tabDisplay.UseVisualStyleBackColor = True
    '
    'pnlDisplay
    '
    Me.pnlDisplay.ColumnCount = 1
    Me.pnlDisplay.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlDisplay.Controls.Add(Me.pnlOverAlert, 0, 7)
    Me.pnlDisplay.Controls.Add(Me.lblDisplayTitle, 0, 0)
    Me.pnlDisplay.Controls.Add(Me.lblDisplayAccuracy, 0, 1)
    Me.pnlDisplay.Controls.Add(Me.pnlDisplayAccuracy, 0, 2)
    Me.pnlDisplay.Controls.Add(Me.chkDisplayScale, 0, 4)
    Me.pnlDisplay.Controls.Add(Me.lblDisplayOver, 0, 6)
    Me.pnlDisplay.Controls.Add(Me.lnDisplaySpace1, 0, 3)
    Me.pnlDisplay.Controls.Add(Me.lnDisplaySpace2, 0, 5)
    Me.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlDisplay.Location = New System.Drawing.Point(0, 0)
    Me.pnlDisplay.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlDisplay.Name = "pnlDisplay"
    Me.pnlDisplay.RowCount = 8
    Me.pnlDisplay.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDisplay.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDisplay.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDisplay.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75.0!))
    Me.pnlDisplay.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDisplay.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
    Me.pnlDisplay.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDisplay.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDisplay.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlDisplay.Size = New System.Drawing.Size(447, 311)
    Me.pnlDisplay.TabIndex = 1
    '
    'pnlOverAlert
    '
    Me.pnlOverAlert.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.pnlOverAlert.AutoSize = True
    Me.pnlOverAlert.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlOverAlert.ColumnCount = 5
    Me.pnlOverAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlOverAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlOverAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlOverAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlOverAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlOverAlert.Controls.Add(Me.chkOverAlert, 0, 0)
    Me.pnlOverAlert.Controls.Add(Me.txtOverSize, 1, 0)
    Me.pnlOverAlert.Controls.Add(Me.lblOverSize, 2, 0)
    Me.pnlOverAlert.Controls.Add(Me.txtOverTime, 3, 0)
    Me.pnlOverAlert.Controls.Add(Me.lblOverTime, 4, 0)
    Me.pnlOverAlert.Location = New System.Drawing.Point(0, 282)
    Me.pnlOverAlert.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlOverAlert.Name = "pnlOverAlert"
    Me.pnlOverAlert.RowCount = 1
    Me.pnlOverAlert.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlOverAlert.Size = New System.Drawing.Size(352, 28)
    Me.pnlOverAlert.TabIndex = 20
    '
    'chkOverAlert
    '
    Me.chkOverAlert.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkOverAlert.AutoSize = True
    Me.chkOverAlert.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkOverAlert.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkOverAlert.Location = New System.Drawing.Point(3, 3)
    Me.chkOverAlert.Name = "chkOverAlert"
    Me.chkOverAlert.Size = New System.Drawing.Size(109, 22)
    Me.chkOverAlert.TabIndex = 0
    Me.chkOverAlert.Text = "Usage &Alert:"
    Me.ttWizard.SetTooltip(Me.chkOverAlert, "Display an alert when your usage exceeds a certain amount in a specified duration" & _
        ". (Off by Default)")
    Me.chkOverAlert.UseVisualStyleBackColor = True
    '
    'txtOverSize
    '
    Me.txtOverSize.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtOverSize.Enabled = False
    Me.txtOverSize.Increment = New Decimal(New Integer() {100, 0, 0, 0})
    Me.txtOverSize.Location = New System.Drawing.Point(118, 4)
    Me.txtOverSize.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
    Me.txtOverSize.Minimum = New Decimal(New Integer() {25, 0, 0, 0})
    Me.txtOverSize.Name = "txtOverSize"
    Me.txtOverSize.Size = New System.Drawing.Size(55, 20)
    Me.txtOverSize.TabIndex = 1
    Me.txtOverSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    Me.ttWizard.SetTooltip(Me.txtOverSize, "Enter the amount of usage to display an alert about (in Megabytes).")
    Me.txtOverSize.Value = New Decimal(New Integer() {100, 0, 0, 0})
    '
    'lblOverSize
    '
    Me.lblOverSize.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblOverSize.AutoSize = True
    Me.lblOverSize.Enabled = False
    Me.lblOverSize.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblOverSize.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblOverSize.Location = New System.Drawing.Point(179, 5)
    Me.lblOverSize.Name = "lblOverSize"
    Me.lblOverSize.Size = New System.Drawing.Size(45, 17)
    Me.lblOverSize.TabIndex = 2
    Me.lblOverSize.Text = "MB in"
    '
    'txtOverTime
    '
    Me.txtOverTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtOverTime.Enabled = False
    Me.txtOverTime.Increment = New Decimal(New Integer() {15, 0, 0, 0})
    Me.txtOverTime.Location = New System.Drawing.Point(230, 4)
    Me.txtOverTime.Maximum = New Decimal(New Integer() {360, 0, 0, 0})
    Me.txtOverTime.Minimum = New Decimal(New Integer() {15, 0, 0, 0})
    Me.txtOverTime.Name = "txtOverTime"
    Me.txtOverTime.Size = New System.Drawing.Size(55, 20)
    Me.txtOverTime.TabIndex = 3
    Me.txtOverTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    Me.ttWizard.SetTooltip(Me.txtOverTime, "Enter the duration of time to check for the defined usage (in minutes).")
    Me.txtOverTime.Value = New Decimal(New Integer() {15, 0, 0, 0})
    '
    'lblOverTime
    '
    Me.lblOverTime.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblOverTime.AutoSize = True
    Me.lblOverTime.Enabled = False
    Me.lblOverTime.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblOverTime.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblOverTime.Location = New System.Drawing.Point(291, 5)
    Me.lblOverTime.Name = "lblOverTime"
    Me.lblOverTime.Size = New System.Drawing.Size(58, 17)
    Me.lblOverTime.TabIndex = 4
    Me.lblOverTime.Text = "minutes"
    '
    'lblDisplayTitle
    '
    Me.lblDisplayTitle.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblDisplayTitle.AutoSize = True
    Me.lblDisplayTitle.Font = New System.Drawing.Font("Times New Roman", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblDisplayTitle.Location = New System.Drawing.Point(68, 3)
    Me.lblDisplayTitle.Margin = New System.Windows.Forms.Padding(3)
    Me.lblDisplayTitle.Name = "lblDisplayTitle"
    Me.lblDisplayTitle.Size = New System.Drawing.Size(311, 26)
    Me.lblDisplayTitle.TabIndex = 13
    Me.lblDisplayTitle.Text = "Set Optional Display Settings"
    Me.lblDisplayTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'lblDisplayAccuracy
    '
    Me.lblDisplayAccuracy.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblDisplayAccuracy.AutoSize = True
    Me.lblDisplayAccuracy.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblDisplayAccuracy.Location = New System.Drawing.Point(3, 35)
    Me.lblDisplayAccuracy.Margin = New System.Windows.Forms.Padding(3)
    Me.lblDisplayAccuracy.Name = "lblDisplayAccuracy"
    Me.lblDisplayAccuracy.Size = New System.Drawing.Size(420, 64)
    Me.lblDisplayAccuracy.TabIndex = 2
    Me.lblDisplayAccuracy.Text = "The following settings are all optional, and only effect the appearance of Satell" & _
    "ite Restriction Tracker." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Select how accurate you want usage data on the Main " & _
    "Window to be:"
    '
    'pnlDisplayAccuracy
    '
    Me.pnlDisplayAccuracy.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.pnlDisplayAccuracy.AutoSize = True
    Me.pnlDisplayAccuracy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlDisplayAccuracy.ColumnCount = 4
    Me.pnlDisplayAccuracy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
    Me.pnlDisplayAccuracy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
    Me.pnlDisplayAccuracy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
    Me.pnlDisplayAccuracy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
    Me.pnlDisplayAccuracy.Controls.Add(Me.optAccuracy0, 0, 0)
    Me.pnlDisplayAccuracy.Controls.Add(Me.optAccuracy1, 1, 0)
    Me.pnlDisplayAccuracy.Controls.Add(Me.optAccuracy2, 2, 0)
    Me.pnlDisplayAccuracy.Controls.Add(Me.optAccuracy3, 3, 0)
    Me.pnlDisplayAccuracy.Location = New System.Drawing.Point(51, 102)
    Me.pnlDisplayAccuracy.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlDisplayAccuracy.Name = "pnlDisplayAccuracy"
    Me.pnlDisplayAccuracy.RowCount = 1
    Me.pnlDisplayAccuracy.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlDisplayAccuracy.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31.0!))
    Me.pnlDisplayAccuracy.Size = New System.Drawing.Size(344, 31)
    Me.pnlDisplayAccuracy.TabIndex = 14
    '
    'optAccuracy0
    '
    Me.optAccuracy0.Appearance = System.Windows.Forms.Appearance.Button
    Me.optAccuracy0.Dock = System.Windows.Forms.DockStyle.Fill
    Me.optAccuracy0.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.optAccuracy0.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.optAccuracy0.Location = New System.Drawing.Point(3, 3)
    Me.optAccuracy0.Name = "optAccuracy0"
    Me.optAccuracy0.Size = New System.Drawing.Size(80, 25)
    Me.optAccuracy0.TabIndex = 0
    Me.optAccuracy0.TabStop = True
    Me.optAccuracy0.Text = "99%"
    Me.optAccuracy0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    Me.ttWizard.SetTooltip(Me.optAccuracy0, "Don't show any decimal places on graph data. (Default)")
    Me.optAccuracy0.UseVisualStyleBackColor = True
    '
    'optAccuracy1
    '
    Me.optAccuracy1.Appearance = System.Windows.Forms.Appearance.Button
    Me.optAccuracy1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.optAccuracy1.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.optAccuracy1.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.optAccuracy1.Location = New System.Drawing.Point(89, 3)
    Me.optAccuracy1.Name = "optAccuracy1"
    Me.optAccuracy1.Size = New System.Drawing.Size(80, 25)
    Me.optAccuracy1.TabIndex = 1
    Me.optAccuracy1.TabStop = True
    Me.optAccuracy1.Text = "99.9%"
    Me.optAccuracy1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    Me.ttWizard.SetTooltip(Me.optAccuracy1, "Show one decimal place on graph data.")
    Me.optAccuracy1.UseVisualStyleBackColor = True
    '
    'optAccuracy2
    '
    Me.optAccuracy2.Appearance = System.Windows.Forms.Appearance.Button
    Me.optAccuracy2.Dock = System.Windows.Forms.DockStyle.Fill
    Me.optAccuracy2.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.optAccuracy2.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.optAccuracy2.Location = New System.Drawing.Point(175, 3)
    Me.optAccuracy2.Name = "optAccuracy2"
    Me.optAccuracy2.Size = New System.Drawing.Size(80, 25)
    Me.optAccuracy2.TabIndex = 2
    Me.optAccuracy2.TabStop = True
    Me.optAccuracy2.Text = "99.99%"
    Me.optAccuracy2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    Me.ttWizard.SetTooltip(Me.optAccuracy2, "Show two decimal places on graph data.")
    Me.optAccuracy2.UseVisualStyleBackColor = True
    '
    'optAccuracy3
    '
    Me.optAccuracy3.Appearance = System.Windows.Forms.Appearance.Button
    Me.optAccuracy3.Dock = System.Windows.Forms.DockStyle.Fill
    Me.optAccuracy3.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.optAccuracy3.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.optAccuracy3.Location = New System.Drawing.Point(261, 3)
    Me.optAccuracy3.Name = "optAccuracy3"
    Me.optAccuracy3.Size = New System.Drawing.Size(80, 25)
    Me.optAccuracy3.TabIndex = 3
    Me.optAccuracy3.TabStop = True
    Me.optAccuracy3.Text = "99.999%"
    Me.optAccuracy3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    Me.ttWizard.SetTooltip(Me.optAccuracy3, "Show three decimal places on graph data.")
    Me.optAccuracy3.UseVisualStyleBackColor = True
    '
    'chkDisplayScale
    '
    Me.chkDisplayScale.AutoSize = True
    Me.chkDisplayScale.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkDisplayScale.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkDisplayScale.Location = New System.Drawing.Point(3, 197)
    Me.chkDisplayScale.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
    Me.chkDisplayScale.Name = "chkDisplayScale"
    Me.chkDisplayScale.Size = New System.Drawing.Size(441, 22)
    Me.chkDisplayScale.TabIndex = 16
    Me.chkDisplayScale.Text = "Set the size of the &Main Window's text to fit the size of the Main Window"
    Me.ttWizard.SetTooltip(Me.chkDisplayScale, "Scale text in the Main Window when the window is resized or maximized. (Off by De" & _
        "fault)")
    Me.chkDisplayScale.UseVisualStyleBackColor = True
    '
    'lblDisplayOver
    '
    Me.lblDisplayOver.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblDisplayOver.AutoSize = True
    Me.lblDisplayOver.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblDisplayOver.Location = New System.Drawing.Point(3, 247)
    Me.lblDisplayOver.Margin = New System.Windows.Forms.Padding(3)
    Me.lblDisplayOver.Name = "lblDisplayOver"
    Me.lblDisplayOver.Size = New System.Drawing.Size(441, 32)
    Me.lblDisplayOver.TabIndex = 19
    Me.lblDisplayOver.Text = "Satellite Restriction Tracker can notify you if your usage climbs too quickly to " & _
    "help prevent overuse. Choose a size and duration to set the alert limit:"
    '
    'lnDisplaySpace1
    '
    Me.lnDisplaySpace1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnDisplaySpace1.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnDisplaySpace1.CausesValidation = False
    Me.lnDisplaySpace1.Location = New System.Drawing.Point(3, 160)
    Me.lnDisplaySpace1.Name = "lnDisplaySpace1"
    Me.lnDisplaySpace1.Padding = New System.Windows.Forms.Padding(1)
    Me.lnDisplaySpace1.Size = New System.Drawing.Size(441, 4)
    Me.lnDisplaySpace1.TabIndex = 21
    Me.lnDisplaySpace1.TabStop = False
    '
    'lnDisplaySpace2
    '
    Me.lnDisplaySpace2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnDisplaySpace2.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnDisplaySpace2.CausesValidation = False
    Me.lnDisplaySpace2.Location = New System.Drawing.Point(3, 232)
    Me.lnDisplaySpace2.Name = "lnDisplaySpace2"
    Me.lnDisplaySpace2.Padding = New System.Windows.Forms.Padding(1)
    Me.lnDisplaySpace2.Size = New System.Drawing.Size(441, 4)
    Me.lnDisplaySpace2.TabIndex = 22
    Me.lnDisplaySpace2.TabStop = False
    '
    'tabFinished
    '
    Me.tabFinished.Controls.Add(Me.pnlFinished)
    Me.tabFinished.Location = New System.Drawing.Point(4, 25)
    Me.tabFinished.Name = "tabFinished"
    Me.tabFinished.Size = New System.Drawing.Size(447, 311)
    Me.tabFinished.TabIndex = 4
    Me.tabFinished.Text = "Finished"
    Me.tabFinished.UseVisualStyleBackColor = True
    '
    'pnlFinished
    '
    Me.pnlFinished.ColumnCount = 1
    Me.pnlFinished.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlFinished.Controls.Add(Me.lblFinishedTitle, 0, 0)
    Me.pnlFinished.Controls.Add(Me.lblFinishedText, 0, 1)
    Me.pnlFinished.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlFinished.Location = New System.Drawing.Point(0, 0)
    Me.pnlFinished.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlFinished.Name = "pnlFinished"
    Me.pnlFinished.RowCount = 2
    Me.pnlFinished.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlFinished.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlFinished.Size = New System.Drawing.Size(447, 311)
    Me.pnlFinished.TabIndex = 1
    '
    'lblFinishedTitle
    '
    Me.lblFinishedTitle.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblFinishedTitle.AutoSize = True
    Me.lblFinishedTitle.Font = New System.Drawing.Font("Times New Roman", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblFinishedTitle.Location = New System.Drawing.Point(97, 3)
    Me.lblFinishedTitle.Margin = New System.Windows.Forms.Padding(3)
    Me.lblFinishedTitle.Name = "lblFinishedTitle"
    Me.lblFinishedTitle.Size = New System.Drawing.Size(252, 26)
    Me.lblFinishedTitle.TabIndex = 0
    Me.lblFinishedTitle.Text = "Your Account is Ready!"
    Me.lblFinishedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'lblFinishedText
    '
    Me.lblFinishedText.AutoSize = True
    Me.lblFinishedText.Font = New System.Drawing.Font("Times New Roman", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblFinishedText.Location = New System.Drawing.Point(3, 35)
    Me.lblFinishedText.Margin = New System.Windows.Forms.Padding(3)
    Me.lblFinishedText.Name = "lblFinishedText"
    Me.lblFinishedText.Size = New System.Drawing.Size(435, 238)
    Me.lblFinishedText.TabIndex = 1
    Me.lblFinishedText.Text = resources.GetString("lblFinishedText.Text")
    '
    'pctHeader
    '
    Me.pctHeader.BackColor = System.Drawing.SystemColors.Window
    Me.pctHeader.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pctHeader.Location = New System.Drawing.Point(0, 0)
    Me.pctHeader.Margin = New System.Windows.Forms.Padding(0)
    Me.pctHeader.Name = "pctHeader"
    Me.pctHeader.Size = New System.Drawing.Size(614, 35)
    Me.pctHeader.TabIndex = 3
    Me.pctHeader.TabStop = False
    '
    'tmrDraw
    '
    Me.tmrDraw.Enabled = True
    Me.tmrDraw.Interval = 500
    '
    'frmWizard
    '
    Me.AcceptButton = Me.cmdNext
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.AutoSize = True
    Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.CancelButton = Me.cmdClose
    Me.ClientSize = New System.Drawing.Size(614, 413)
    Me.Controls.Add(Me.pnlWizard)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmWizard"
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.pnlWizard.ResumeLayout(False)
    Me.pnlWizard.PerformLayout()
    Me.pnlButtons.ResumeLayout(False)
    Me.pnlButtons.PerformLayout()
    Me.pnlContent.ResumeLayout(False)
    CType(Me.pctLeftBox, System.ComponentModel.ISupportInitialize).EndInit()
    Me.tbsWizardPages.ResumeLayout(False)
    Me.tabWelcome.ResumeLayout(False)
    Me.pnlWelcome.ResumeLayout(False)
    Me.pnlWelcome.PerformLayout()
    Me.tabAccount.ResumeLayout(False)
    Me.pnlAccount.ResumeLayout(False)
    Me.pnlAccount.PerformLayout()
    Me.tabService.ResumeLayout(False)
    Me.pnlService.ResumeLayout(False)
    Me.pnlService.PerformLayout()
    Me.pnlKey.ResumeLayout(False)
    Me.pnlKey.PerformLayout()
    Me.tabDisplay.ResumeLayout(False)
    Me.pnlDisplay.ResumeLayout(False)
    Me.pnlDisplay.PerformLayout()
    Me.pnlOverAlert.ResumeLayout(False)
    Me.pnlOverAlert.PerformLayout()
    CType(Me.txtOverSize, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtOverTime, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlDisplayAccuracy.ResumeLayout(False)
    Me.tabFinished.ResumeLayout(False)
    Me.pnlFinished.ResumeLayout(False)
    Me.pnlFinished.PerformLayout()
    CType(Me.pctHeader, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents pnlWizard As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlButtons As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdFAQ As System.Windows.Forms.Button
  Friend WithEvents cmdClose As System.Windows.Forms.Button
  Friend WithEvents cmdNext As System.Windows.Forms.Button
  Friend WithEvents cmdPrevious As System.Windows.Forms.Button
  Friend WithEvents pnlContent As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctLeftBox As System.Windows.Forms.PictureBox
  Friend WithEvents tbsWizardPages As System.Windows.Forms.TabControl
  Friend WithEvents tabWelcome As System.Windows.Forms.TabPage
  Friend WithEvents pnlWelcome As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblWelcomeTitle As System.Windows.Forms.Label
  Friend WithEvents lblWelcomeText As System.Windows.Forms.Label
  Friend WithEvents tabAccount As System.Windows.Forms.TabPage
  Friend WithEvents pnlAccount As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblAccountProvider As System.Windows.Forms.Label
  Friend WithEvents lblAccountHost As System.Windows.Forms.Label
  Friend WithEvents txtAccountUsername As System.Windows.Forms.TextBox
  Friend WithEvents lblAccountPassText As System.Windows.Forms.Label
  Friend WithEvents txtAccountPass As System.Windows.Forms.TextBox
  Friend WithEvents lblAccountPass As System.Windows.Forms.Label
  Friend WithEvents tabService As System.Windows.Forms.TabPage
  Friend WithEvents pnlService As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblRemoteText As System.Windows.Forms.Label
  Friend WithEvents optRemote As System.Windows.Forms.RadioButton
  Friend WithEvents pnlKey As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents txtKey1 As System.Windows.Forms.TextBox
  Friend WithEvents txtKey2 As System.Windows.Forms.TextBox
  Friend WithEvents txtKey3 As System.Windows.Forms.TextBox
  Friend WithEvents txtKey4 As System.Windows.Forms.TextBox
  Friend WithEvents txtKey5 As System.Windows.Forms.TextBox
  Friend WithEvents lblLocalText As System.Windows.Forms.Label
  Friend WithEvents optLocal As System.Windows.Forms.RadioButton
  Friend WithEvents optNone As System.Windows.Forms.RadioButton
  Friend WithEvents tabDisplay As System.Windows.Forms.TabPage
  Friend WithEvents tabFinished As System.Windows.Forms.TabPage
  Friend WithEvents pnlFinished As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblFinishedTitle As System.Windows.Forms.Label
  Friend WithEvents lblFinishedText As System.Windows.Forms.Label
  Friend WithEvents lblLocal As System.Windows.Forms.Label
  Friend WithEvents lblNone As System.Windows.Forms.Label
  Friend WithEvents pctHeader As System.Windows.Forms.PictureBox
  Friend WithEvents tmrDraw As System.Windows.Forms.Timer
  Friend WithEvents lblAccountName As System.Windows.Forms.Label
  Friend WithEvents lblAccountUsername As System.Windows.Forms.Label
  Friend WithEvents cmbAccountHost As System.Windows.Forms.ComboBox
  Friend WithEvents lblServiceTitle As System.Windows.Forms.Label
  Friend WithEvents txtSignUp As RestrictionTracker.LinkLabel
  Friend WithEvents lblActivity As System.Windows.Forms.Label
  Friend WithEvents pnlDisplay As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblDisplayTitle As System.Windows.Forms.Label
  Friend WithEvents lblDisplayAccuracy As System.Windows.Forms.Label
  Friend WithEvents pnlDisplayAccuracy As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents optAccuracy0 As System.Windows.Forms.RadioButton
  Friend WithEvents optAccuracy1 As System.Windows.Forms.RadioButton
  Friend WithEvents optAccuracy2 As System.Windows.Forms.RadioButton
  Friend WithEvents optAccuracy3 As System.Windows.Forms.RadioButton
  Friend WithEvents chkDisplayScale As System.Windows.Forms.CheckBox
  Friend WithEvents lblDisplayOver As System.Windows.Forms.Label
  Friend WithEvents pnlOverAlert As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents chkOverAlert As System.Windows.Forms.CheckBox
  Friend WithEvents txtOverSize As System.Windows.Forms.NumericUpDown
  Friend WithEvents lblOverSize As System.Windows.Forms.Label
  Friend WithEvents txtOverTime As System.Windows.Forms.NumericUpDown
  Friend WithEvents lblOverTime As System.Windows.Forms.Label
  Friend WithEvents lblAccountTitle As System.Windows.Forms.Label
  Friend WithEvents ttWizard As RestrictionTracker.ToolTip
  Friend WithEvents lnAccountSpace1 As RestrictionTracker.LineBreak
  Friend WithEvents lnAccountSpace2 As RestrictionTracker.LineBreak
  Friend WithEvents lnServiceSpace2 As RestrictionTracker.LineBreak
  Friend WithEvents lnServiceSpace1 As RestrictionTracker.LineBreak
  Friend WithEvents lnDisplaySpace1 As RestrictionTracker.LineBreak
  Friend WithEvents lnDisplaySpace2 As RestrictionTracker.LineBreak

End Class
