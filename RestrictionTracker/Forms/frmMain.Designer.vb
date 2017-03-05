<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
    Me.pnlDetails = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlSettings = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdAbout = New System.Windows.Forms.Button()
    Me.cmdHistory = New System.Windows.Forms.Button()
    Me.cmdRefresh = New System.Windows.Forms.Button()
    Me.cmdConfig = New System.Windows.Forms.Button()
    Me.gbUsage = New System.Windows.Forms.GroupBox()
    Me.pctNetTest = New RestrictionTracker.LinkPictureBox()
    Me.lblStatus = New System.Windows.Forms.Label()
    Me.pnlNothing = New System.Windows.Forms.TableLayoutPanel()
    Me.lblNothing = New System.Windows.Forms.Label()
    Me.lblRRS = New RestrictionTracker.LinkLabel()
    Me.pnlTypeA = New System.Windows.Forms.TableLayoutPanel()
    Me.gbTypeAUld = New System.Windows.Forms.GroupBox()
    Me.pnlTypeAUld = New System.Windows.Forms.TableLayoutPanel()
    Me.pctTypeAUld = New System.Windows.Forms.PictureBox()
    Me.mnuGraph = New System.Windows.Forms.ContextMenu()
    Me.mnuGraphRefresh = New System.Windows.Forms.MenuItem()
    Me.mnuGraphSpace = New System.Windows.Forms.MenuItem()
    Me.mnuGraphInvert = New System.Windows.Forms.MenuItem()
    Me.mnuGraphColors = New System.Windows.Forms.MenuItem()
    Me.pnlUldText = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlTypeAUldUsed = New System.Windows.Forms.TableLayoutPanel()
    Me.lblTypeAUldUsedVal = New System.Windows.Forms.Label()
    Me.lblTypeAUldUsed = New System.Windows.Forms.Label()
    Me.pnlTypeAUldFree = New System.Windows.Forms.TableLayoutPanel()
    Me.lblTypeAUldFreeVal = New System.Windows.Forms.Label()
    Me.lblTypeAUldFree = New System.Windows.Forms.Label()
    Me.pnlTypeAUldLimit = New System.Windows.Forms.TableLayoutPanel()
    Me.lblTypeAUldLimitVal = New System.Windows.Forms.Label()
    Me.lblTypeAUldLimit = New System.Windows.Forms.Label()
    Me.gbTypeADld = New System.Windows.Forms.GroupBox()
    Me.pnlTypeADld = New System.Windows.Forms.TableLayoutPanel()
    Me.pctTypeADld = New System.Windows.Forms.PictureBox()
    Me.pnlDldText = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlTypeADldUsed = New System.Windows.Forms.TableLayoutPanel()
    Me.lblTypeADldUsedVal = New System.Windows.Forms.Label()
    Me.lblTypeADldUsed = New System.Windows.Forms.Label()
    Me.pnlTypeADldFree = New System.Windows.Forms.TableLayoutPanel()
    Me.lblTypeADldFreeVal = New System.Windows.Forms.Label()
    Me.lblTypeADldFree = New System.Windows.Forms.Label()
    Me.pnlTypeADldLimit = New System.Windows.Forms.TableLayoutPanel()
    Me.lblTypeADldLimitVal = New System.Windows.Forms.Label()
    Me.lblTypeADldLimit = New System.Windows.Forms.Label()
    Me.pnlTypeB = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlTypeBUsed = New System.Windows.Forms.TableLayoutPanel()
    Me.lblTypeBUsed = New System.Windows.Forms.Label()
    Me.lblTypeBUsedVal = New System.Windows.Forms.Label()
    Me.pnlTypeBFree = New System.Windows.Forms.TableLayoutPanel()
    Me.lblTypeBFree = New System.Windows.Forms.Label()
    Me.lblTypeBFreeVal = New System.Windows.Forms.Label()
    Me.pnlTypeBLimit = New System.Windows.Forms.TableLayoutPanel()
    Me.lblTypeBLimit = New System.Windows.Forms.Label()
    Me.lblTypeBLimitVal = New System.Windows.Forms.Label()
    Me.pctTypeB = New System.Windows.Forms.PictureBox()
    Me.trayIcon = New System.Windows.Forms.NotifyIcon(Me.components)
    Me.mnuTray = New System.Windows.Forms.ContextMenu()
    Me.mnuRestore = New System.Windows.Forms.MenuItem()
    Me.mnuAbout = New System.Windows.Forms.MenuItem()
    Me.mnuSpacer = New System.Windows.Forms.MenuItem()
    Me.mnuExit = New System.Windows.Forms.MenuItem()
    Me.tmrUpdate = New System.Windows.Forms.Timer(Me.components)
    Me.tmrIcon = New System.Windows.Forms.Timer(Me.components)
    Me.tmrIcoDelay = New System.Windows.Forms.Timer(Me.components)
    Me.tmrSpeed = New System.Windows.Forms.Timer(Me.components)
    Me.tmrStatus = New System.Windows.Forms.Timer(Me.components)
    Me.ttUI = New RestrictionTracker.ToolTip(Me.components)
    Me.pnlDetails.SuspendLayout()
    Me.pnlSettings.SuspendLayout()
    Me.gbUsage.SuspendLayout()
    CType(Me.pctNetTest, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlNothing.SuspendLayout()
    Me.pnlTypeA.SuspendLayout()
    Me.gbTypeAUld.SuspendLayout()
    Me.pnlTypeAUld.SuspendLayout()
    CType(Me.pctTypeAUld, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlUldText.SuspendLayout()
    Me.pnlTypeAUldUsed.SuspendLayout()
    Me.pnlTypeAUldFree.SuspendLayout()
    Me.pnlTypeAUldLimit.SuspendLayout()
    Me.gbTypeADld.SuspendLayout()
    Me.pnlTypeADld.SuspendLayout()
    CType(Me.pctTypeADld, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlDldText.SuspendLayout()
    Me.pnlTypeADldUsed.SuspendLayout()
    Me.pnlTypeADldFree.SuspendLayout()
    Me.pnlTypeADldLimit.SuspendLayout()
    Me.pnlTypeB.SuspendLayout()
    Me.pnlTypeBUsed.SuspendLayout()
    Me.pnlTypeBFree.SuspendLayout()
    Me.pnlTypeBLimit.SuspendLayout()
    CType(Me.pctTypeB, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'pnlDetails
    '
    Me.pnlDetails.ColumnCount = 1
    Me.pnlDetails.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlDetails.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlDetails.Controls.Add(Me.pnlSettings, 0, 0)
    Me.pnlDetails.Controls.Add(Me.gbUsage, 0, 1)
    Me.pnlDetails.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlDetails.Location = New System.Drawing.Point(0, 0)
    Me.pnlDetails.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlDetails.Name = "pnlDetails"
    Me.pnlDetails.RowCount = 2
    Me.pnlDetails.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlDetails.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlDetails.Size = New System.Drawing.Size(434, 162)
    Me.pnlDetails.TabIndex = 0
    '
    'pnlSettings
    '
    Me.pnlSettings.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlSettings.AutoSize = True
    Me.pnlSettings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlSettings.ColumnCount = 4
    Me.pnlSettings.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
    Me.pnlSettings.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
    Me.pnlSettings.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
    Me.pnlSettings.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
    Me.pnlSettings.Controls.Add(Me.cmdAbout, 3, 0)
    Me.pnlSettings.Controls.Add(Me.cmdHistory, 1, 0)
    Me.pnlSettings.Controls.Add(Me.cmdRefresh, 0, 0)
    Me.pnlSettings.Controls.Add(Me.cmdConfig, 2, 0)
    Me.pnlSettings.Location = New System.Drawing.Point(0, 0)
    Me.pnlSettings.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlSettings.Name = "pnlSettings"
    Me.pnlSettings.RowCount = 1
    Me.pnlSettings.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlSettings.Size = New System.Drawing.Size(434, 28)
    Me.pnlSettings.TabIndex = 0
    '
    'cmdAbout
    '
    Me.cmdAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdAbout.AutoSize = True
    Me.cmdAbout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdAbout.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdAbout.Location = New System.Drawing.Point(327, 3)
    Me.cmdAbout.Name = "cmdAbout"
    Me.cmdAbout.Size = New System.Drawing.Size(104, 22)
    Me.cmdAbout.TabIndex = 3
    Me.cmdAbout.Text = "&About"
    Me.ttUI.SetToolTip(Me.cmdAbout, "View information about Satellite Restriction Tracker and check for updates.")
    Me.cmdAbout.UseVisualStyleBackColor = True
    '
    'cmdHistory
    '
    Me.cmdHistory.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdHistory.AutoSize = True
    Me.cmdHistory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdHistory.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdHistory.Location = New System.Drawing.Point(111, 3)
    Me.cmdHistory.Name = "cmdHistory"
    Me.cmdHistory.Size = New System.Drawing.Size(102, 22)
    Me.cmdHistory.TabIndex = 1
    Me.cmdHistory.Text = "&History"
    Me.ttUI.SetToolTip(Me.cmdHistory, "View your usage history.")
    Me.cmdHistory.UseVisualStyleBackColor = True
    '
    'cmdRefresh
    '
    Me.cmdRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdRefresh.AutoSize = True
    Me.cmdRefresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdRefresh.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdRefresh.Location = New System.Drawing.Point(3, 3)
    Me.cmdRefresh.Name = "cmdRefresh"
    Me.cmdRefresh.Size = New System.Drawing.Size(102, 22)
    Me.cmdRefresh.TabIndex = 0
    Me.cmdRefresh.Text = "&Refresh"
    Me.ttUI.SetToolTip(Me.cmdRefresh, "Reload usage level information immediately." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Hold CTRL to reload database.)")
    Me.cmdRefresh.UseVisualStyleBackColor = True
    '
    'cmdConfig
    '
    Me.cmdConfig.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdConfig.AutoSize = True
    Me.cmdConfig.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdConfig.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdConfig.Location = New System.Drawing.Point(219, 3)
    Me.cmdConfig.Name = "cmdConfig"
    Me.cmdConfig.Size = New System.Drawing.Size(102, 22)
    Me.cmdConfig.TabIndex = 2
    Me.cmdConfig.Text = "&Configuration"
    Me.ttUI.SetToolTip(Me.cmdConfig, "Change program settings.")
    Me.cmdConfig.UseVisualStyleBackColor = True
    '
    'gbUsage
    '
    Me.gbUsage.Controls.Add(Me.pctNetTest)
    Me.gbUsage.Controls.Add(Me.lblStatus)
    Me.gbUsage.Controls.Add(Me.pnlNothing)
    Me.gbUsage.Controls.Add(Me.pnlTypeA)
    Me.gbUsage.Controls.Add(Me.pnlTypeB)
    Me.gbUsage.Dock = System.Windows.Forms.DockStyle.Fill
    Me.gbUsage.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.gbUsage.Location = New System.Drawing.Point(3, 31)
    Me.gbUsage.Name = "gbUsage"
    Me.gbUsage.Size = New System.Drawing.Size(428, 128)
    Me.gbUsage.TabIndex = 1
    Me.gbUsage.TabStop = False
    '
    'pctNetTest
    '
    Me.pctNetTest.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pctNetTest.Location = New System.Drawing.Point(400, 0)
    Me.pctNetTest.Name = "pctNetTest"
    Me.pctNetTest.Size = New System.Drawing.Size(16, 16)
    Me.pctNetTest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pctNetTest.TabIndex = 0
    '
    'lblStatus
    '
    Me.lblStatus.AutoSize = True
    Me.lblStatus.Location = New System.Drawing.Point(6, 0)
    Me.lblStatus.Name = "lblStatus"
    Me.lblStatus.Size = New System.Drawing.Size(72, 13)
    Me.lblStatus.TabIndex = 4
    Me.lblStatus.Text = "Usage Levels"
    '
    'pnlNothing
    '
    Me.pnlNothing.ColumnCount = 2
    Me.pnlNothing.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlNothing.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlNothing.Controls.Add(Me.lblNothing, 0, 0)
    Me.pnlNothing.Controls.Add(Me.lblRRS, 1, 1)
    Me.pnlNothing.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlNothing.Location = New System.Drawing.Point(3, 16)
    Me.pnlNothing.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlNothing.Name = "pnlNothing"
    Me.pnlNothing.RowCount = 2
    Me.pnlNothing.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.0!))
    Me.pnlNothing.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
    Me.pnlNothing.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlNothing.Size = New System.Drawing.Size(422, 109)
    Me.pnlNothing.TabIndex = 5
    Me.pnlNothing.Visible = False
    '
    'lblNothing
    '
    Me.lblNothing.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblNothing.AutoSize = True
    Me.pnlNothing.SetColumnSpan(Me.lblNothing, 2)
    Me.lblNothing.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblNothing.Location = New System.Drawing.Point(8, 19)
    Me.lblNothing.Name = "lblNothing"
    Me.lblNothing.Size = New System.Drawing.Size(406, 37)
    Me.lblNothing.TabIndex = 0
    Me.lblNothing.Text = "Satellite Restriction Tracker"
    '
    'lblRRS
    '
    Me.lblRRS.AutoSize = True
    Me.lblRRS.Cursor = System.Windows.Forms.Cursors.Hand
    Me.lblRRS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
    Me.lblRRS.ForeColor = System.Drawing.Color.MediumBlue
    Me.lblRRS.Location = New System.Drawing.Point(214, 76)
    Me.lblRRS.Name = "lblRRS"
    Me.lblRRS.Size = New System.Drawing.Size(128, 13)
    Me.lblRRS.TabIndex = 1
    Me.lblRRS.Text = "by RealityRipple Software"
    '
    'pnlTypeA
    '
    Me.pnlTypeA.ColumnCount = 2
    Me.pnlTypeA.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlTypeA.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlTypeA.Controls.Add(Me.gbTypeAUld, 1, 0)
    Me.pnlTypeA.Controls.Add(Me.gbTypeADld, 0, 0)
    Me.pnlTypeA.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlTypeA.Location = New System.Drawing.Point(3, 16)
    Me.pnlTypeA.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlTypeA.Name = "pnlTypeA"
    Me.pnlTypeA.RowCount = 1
    Me.pnlTypeA.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlTypeA.Size = New System.Drawing.Size(422, 109)
    Me.pnlTypeA.TabIndex = 1
    '
    'gbTypeAUld
    '
    Me.gbTypeAUld.Controls.Add(Me.pnlTypeAUld)
    Me.gbTypeAUld.Dock = System.Windows.Forms.DockStyle.Fill
    Me.gbTypeAUld.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.gbTypeAUld.Location = New System.Drawing.Point(214, 3)
    Me.gbTypeAUld.Name = "gbTypeAUld"
    Me.gbTypeAUld.Size = New System.Drawing.Size(205, 103)
    Me.gbTypeAUld.TabIndex = 0
    Me.gbTypeAUld.TabStop = False
    Me.gbTypeAUld.Text = "Upload"
    '
    'pnlTypeAUld
    '
    Me.pnlTypeAUld.ColumnCount = 2
    Me.pnlTypeAUld.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeAUld.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlTypeAUld.Controls.Add(Me.pctTypeAUld, 1, 0)
    Me.pnlTypeAUld.Controls.Add(Me.pnlUldText, 0, 0)
    Me.pnlTypeAUld.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlTypeAUld.Location = New System.Drawing.Point(3, 16)
    Me.pnlTypeAUld.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlTypeAUld.Name = "pnlTypeAUld"
    Me.pnlTypeAUld.RowCount = 1
    Me.pnlTypeAUld.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlTypeAUld.Size = New System.Drawing.Size(199, 84)
    Me.pnlTypeAUld.TabIndex = 1
    '
    'pctTypeAUld
    '
    Me.pctTypeAUld.BackColor = System.Drawing.Color.White
    Me.pctTypeAUld.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.pctTypeAUld.ContextMenu = Me.mnuGraph
    Me.pctTypeAUld.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pctTypeAUld.Location = New System.Drawing.Point(57, 0)
    Me.pctTypeAUld.Margin = New System.Windows.Forms.Padding(0, 0, 3, 3)
    Me.pctTypeAUld.Name = "pctTypeAUld"
    Me.pctTypeAUld.Size = New System.Drawing.Size(139, 81)
    Me.pctTypeAUld.TabIndex = 0
    Me.pctTypeAUld.TabStop = False
    Me.ttUI.SetToolTip(Me.pctTypeAUld, "Graph representing your upload usage.")
    '
    'mnuGraph
    '
    Me.mnuGraph.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuGraphRefresh, Me.mnuGraphSpace, Me.mnuGraphInvert, Me.mnuGraphColors})
    '
    'mnuGraphRefresh
    '
    Me.mnuGraphRefresh.Index = 0
    Me.mnuGraphRefresh.Text = "&Refresh"
    '
    'mnuGraphSpace
    '
    Me.mnuGraphSpace.Index = 1
    Me.mnuGraphSpace.Text = "-"
    '
    'mnuGraphInvert
    '
    Me.mnuGraphInvert.Index = 2
    Me.mnuGraphInvert.Text = "Invert &Numbers"
    Me.mnuGraphInvert.Visible = False
    '
    'mnuGraphColors
    '
    Me.mnuGraphColors.Index = 3
    Me.mnuGraphColors.Text = "&Customize Colors"
    '
    'pnlUldText
    '
    Me.pnlUldText.AutoSize = True
    Me.pnlUldText.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlUldText.ColumnCount = 1
    Me.pnlUldText.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlUldText.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlUldText.Controls.Add(Me.pnlTypeAUldUsed, 0, 0)
    Me.pnlUldText.Controls.Add(Me.pnlTypeAUldFree, 0, 1)
    Me.pnlUldText.Controls.Add(Me.pnlTypeAUldLimit, 0, 2)
    Me.pnlUldText.Dock = System.Windows.Forms.DockStyle.Left
    Me.pnlUldText.Location = New System.Drawing.Point(0, 0)
    Me.pnlUldText.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlUldText.Name = "pnlUldText"
    Me.pnlUldText.RowCount = 3
    Me.pnlUldText.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlUldText.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlUldText.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlUldText.Size = New System.Drawing.Size(57, 84)
    Me.pnlUldText.TabIndex = 1
    '
    'pnlTypeAUldUsed
    '
    Me.pnlTypeAUldUsed.AutoSize = True
    Me.pnlTypeAUldUsed.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlTypeAUldUsed.ColumnCount = 2
    Me.pnlTypeAUldUsed.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeAUldUsed.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeAUldUsed.Controls.Add(Me.lblTypeAUldUsedVal, 1, 0)
    Me.pnlTypeAUldUsed.Controls.Add(Me.lblTypeAUldUsed, 0, 0)
    Me.pnlTypeAUldUsed.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlTypeAUldUsed.Location = New System.Drawing.Point(0, 0)
    Me.pnlTypeAUldUsed.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlTypeAUldUsed.Name = "pnlTypeAUldUsed"
    Me.pnlTypeAUldUsed.RowCount = 1
    Me.pnlTypeAUldUsed.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlTypeAUldUsed.Size = New System.Drawing.Size(57, 28)
    Me.pnlTypeAUldUsed.TabIndex = 6
    '
    'lblTypeAUldUsedVal
    '
    Me.lblTypeAUldUsedVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblTypeAUldUsedVal.AutoSize = True
    Me.lblTypeAUldUsedVal.Location = New System.Drawing.Point(38, 7)
    Me.lblTypeAUldUsedVal.Margin = New System.Windows.Forms.Padding(0)
    Me.lblTypeAUldUsedVal.Name = "lblTypeAUldUsedVal"
    Me.lblTypeAUldUsedVal.Size = New System.Drawing.Size(19, 13)
    Me.lblTypeAUldUsedVal.TabIndex = 1
    Me.lblTypeAUldUsedVal.Text = " -- "
    '
    'lblTypeAUldUsed
    '
    Me.lblTypeAUldUsed.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTypeAUldUsed.AutoSize = True
    Me.lblTypeAUldUsed.Location = New System.Drawing.Point(3, 7)
    Me.lblTypeAUldUsed.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblTypeAUldUsed.Name = "lblTypeAUldUsed"
    Me.lblTypeAUldUsed.Size = New System.Drawing.Size(35, 13)
    Me.lblTypeAUldUsed.TabIndex = 0
    Me.lblTypeAUldUsed.Text = "Used:"
    '
    'pnlTypeAUldFree
    '
    Me.pnlTypeAUldFree.AutoSize = True
    Me.pnlTypeAUldFree.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlTypeAUldFree.ColumnCount = 2
    Me.pnlTypeAUldFree.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeAUldFree.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeAUldFree.Controls.Add(Me.lblTypeAUldFreeVal, 1, 0)
    Me.pnlTypeAUldFree.Controls.Add(Me.lblTypeAUldFree, 0, 0)
    Me.pnlTypeAUldFree.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlTypeAUldFree.Location = New System.Drawing.Point(0, 28)
    Me.pnlTypeAUldFree.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlTypeAUldFree.Name = "pnlTypeAUldFree"
    Me.pnlTypeAUldFree.RowCount = 1
    Me.pnlTypeAUldFree.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlTypeAUldFree.Size = New System.Drawing.Size(57, 28)
    Me.pnlTypeAUldFree.TabIndex = 7
    '
    'lblTypeAUldFreeVal
    '
    Me.lblTypeAUldFreeVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblTypeAUldFreeVal.AutoSize = True
    Me.lblTypeAUldFreeVal.Location = New System.Drawing.Point(38, 7)
    Me.lblTypeAUldFreeVal.Margin = New System.Windows.Forms.Padding(0)
    Me.lblTypeAUldFreeVal.Name = "lblTypeAUldFreeVal"
    Me.lblTypeAUldFreeVal.Size = New System.Drawing.Size(19, 13)
    Me.lblTypeAUldFreeVal.TabIndex = 3
    Me.lblTypeAUldFreeVal.Text = " -- "
    '
    'lblTypeAUldFree
    '
    Me.lblTypeAUldFree.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTypeAUldFree.AutoSize = True
    Me.lblTypeAUldFree.Location = New System.Drawing.Point(3, 7)
    Me.lblTypeAUldFree.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblTypeAUldFree.Name = "lblTypeAUldFree"
    Me.lblTypeAUldFree.Size = New System.Drawing.Size(31, 13)
    Me.lblTypeAUldFree.TabIndex = 2
    Me.lblTypeAUldFree.Text = "Free:"
    '
    'pnlTypeAUldLimit
    '
    Me.pnlTypeAUldLimit.AutoSize = True
    Me.pnlTypeAUldLimit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlTypeAUldLimit.ColumnCount = 2
    Me.pnlTypeAUldLimit.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeAUldLimit.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeAUldLimit.Controls.Add(Me.lblTypeAUldLimitVal, 1, 0)
    Me.pnlTypeAUldLimit.Controls.Add(Me.lblTypeAUldLimit, 0, 0)
    Me.pnlTypeAUldLimit.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlTypeAUldLimit.Location = New System.Drawing.Point(0, 56)
    Me.pnlTypeAUldLimit.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlTypeAUldLimit.Name = "pnlTypeAUldLimit"
    Me.pnlTypeAUldLimit.RowCount = 1
    Me.pnlTypeAUldLimit.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlTypeAUldLimit.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
    Me.pnlTypeAUldLimit.Size = New System.Drawing.Size(57, 28)
    Me.pnlTypeAUldLimit.TabIndex = 8
    '
    'lblTypeAUldLimitVal
    '
    Me.lblTypeAUldLimitVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblTypeAUldLimitVal.AutoSize = True
    Me.lblTypeAUldLimitVal.Location = New System.Drawing.Point(38, 7)
    Me.lblTypeAUldLimitVal.Margin = New System.Windows.Forms.Padding(0)
    Me.lblTypeAUldLimitVal.Name = "lblTypeAUldLimitVal"
    Me.lblTypeAUldLimitVal.Size = New System.Drawing.Size(19, 13)
    Me.lblTypeAUldLimitVal.TabIndex = 5
    Me.lblTypeAUldLimitVal.Text = " -- "
    '
    'lblTypeAUldLimit
    '
    Me.lblTypeAUldLimit.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTypeAUldLimit.AutoSize = True
    Me.lblTypeAUldLimit.Location = New System.Drawing.Point(3, 7)
    Me.lblTypeAUldLimit.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblTypeAUldLimit.Name = "lblTypeAUldLimit"
    Me.lblTypeAUldLimit.Size = New System.Drawing.Size(31, 13)
    Me.lblTypeAUldLimit.TabIndex = 4
    Me.lblTypeAUldLimit.Text = "Limit:"
    '
    'gbTypeADld
    '
    Me.gbTypeADld.Controls.Add(Me.pnlTypeADld)
    Me.gbTypeADld.Dock = System.Windows.Forms.DockStyle.Fill
    Me.gbTypeADld.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.gbTypeADld.Location = New System.Drawing.Point(3, 3)
    Me.gbTypeADld.Name = "gbTypeADld"
    Me.gbTypeADld.Size = New System.Drawing.Size(205, 103)
    Me.gbTypeADld.TabIndex = 0
    Me.gbTypeADld.TabStop = False
    Me.gbTypeADld.Text = "Download"
    '
    'pnlTypeADld
    '
    Me.pnlTypeADld.ColumnCount = 2
    Me.pnlTypeADld.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeADld.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlTypeADld.Controls.Add(Me.pctTypeADld, 1, 0)
    Me.pnlTypeADld.Controls.Add(Me.pnlDldText, 0, 0)
    Me.pnlTypeADld.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlTypeADld.Location = New System.Drawing.Point(3, 16)
    Me.pnlTypeADld.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlTypeADld.Name = "pnlTypeADld"
    Me.pnlTypeADld.RowCount = 1
    Me.pnlTypeADld.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlTypeADld.Size = New System.Drawing.Size(199, 84)
    Me.pnlTypeADld.TabIndex = 0
    '
    'pctTypeADld
    '
    Me.pctTypeADld.BackColor = System.Drawing.Color.White
    Me.pctTypeADld.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.pctTypeADld.ContextMenu = Me.mnuGraph
    Me.pctTypeADld.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pctTypeADld.Location = New System.Drawing.Point(57, 0)
    Me.pctTypeADld.Margin = New System.Windows.Forms.Padding(0, 0, 3, 3)
    Me.pctTypeADld.Name = "pctTypeADld"
    Me.pctTypeADld.Size = New System.Drawing.Size(139, 81)
    Me.pctTypeADld.TabIndex = 0
    Me.pctTypeADld.TabStop = False
    Me.ttUI.SetToolTip(Me.pctTypeADld, "Graph representing your download usage.")
    '
    'pnlDldText
    '
    Me.pnlDldText.AutoSize = True
    Me.pnlDldText.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlDldText.ColumnCount = 1
    Me.pnlDldText.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlDldText.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlDldText.Controls.Add(Me.pnlTypeADldUsed, 0, 0)
    Me.pnlDldText.Controls.Add(Me.pnlTypeADldFree, 0, 1)
    Me.pnlDldText.Controls.Add(Me.pnlTypeADldLimit, 0, 2)
    Me.pnlDldText.Dock = System.Windows.Forms.DockStyle.Left
    Me.pnlDldText.Location = New System.Drawing.Point(0, 0)
    Me.pnlDldText.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlDldText.Name = "pnlDldText"
    Me.pnlDldText.RowCount = 3
    Me.pnlDldText.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlDldText.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlDldText.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlDldText.Size = New System.Drawing.Size(57, 84)
    Me.pnlDldText.TabIndex = 1
    '
    'pnlTypeADldUsed
    '
    Me.pnlTypeADldUsed.AutoSize = True
    Me.pnlTypeADldUsed.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlTypeADldUsed.ColumnCount = 2
    Me.pnlTypeADldUsed.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeADldUsed.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeADldUsed.Controls.Add(Me.lblTypeADldUsedVal, 1, 0)
    Me.pnlTypeADldUsed.Controls.Add(Me.lblTypeADldUsed, 0, 0)
    Me.pnlTypeADldUsed.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlTypeADldUsed.Location = New System.Drawing.Point(0, 0)
    Me.pnlTypeADldUsed.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlTypeADldUsed.Name = "pnlTypeADldUsed"
    Me.pnlTypeADldUsed.RowCount = 1
    Me.pnlTypeADldUsed.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlTypeADldUsed.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
    Me.pnlTypeADldUsed.Size = New System.Drawing.Size(57, 28)
    Me.pnlTypeADldUsed.TabIndex = 6
    '
    'lblTypeADldUsedVal
    '
    Me.lblTypeADldUsedVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblTypeADldUsedVal.AutoSize = True
    Me.lblTypeADldUsedVal.Location = New System.Drawing.Point(38, 7)
    Me.lblTypeADldUsedVal.Margin = New System.Windows.Forms.Padding(0)
    Me.lblTypeADldUsedVal.Name = "lblTypeADldUsedVal"
    Me.lblTypeADldUsedVal.Size = New System.Drawing.Size(19, 13)
    Me.lblTypeADldUsedVal.TabIndex = 1
    Me.lblTypeADldUsedVal.Text = " -- "
    '
    'lblTypeADldUsed
    '
    Me.lblTypeADldUsed.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTypeADldUsed.AutoSize = True
    Me.lblTypeADldUsed.Location = New System.Drawing.Point(3, 7)
    Me.lblTypeADldUsed.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblTypeADldUsed.Name = "lblTypeADldUsed"
    Me.lblTypeADldUsed.Size = New System.Drawing.Size(35, 13)
    Me.lblTypeADldUsed.TabIndex = 0
    Me.lblTypeADldUsed.Text = "Used:"
    '
    'pnlTypeADldFree
    '
    Me.pnlTypeADldFree.AutoSize = True
    Me.pnlTypeADldFree.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlTypeADldFree.ColumnCount = 2
    Me.pnlTypeADldFree.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeADldFree.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeADldFree.Controls.Add(Me.lblTypeADldFreeVal, 1, 0)
    Me.pnlTypeADldFree.Controls.Add(Me.lblTypeADldFree, 0, 0)
    Me.pnlTypeADldFree.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlTypeADldFree.Location = New System.Drawing.Point(0, 28)
    Me.pnlTypeADldFree.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlTypeADldFree.Name = "pnlTypeADldFree"
    Me.pnlTypeADldFree.RowCount = 1
    Me.pnlTypeADldFree.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlTypeADldFree.Size = New System.Drawing.Size(57, 28)
    Me.pnlTypeADldFree.TabIndex = 7
    '
    'lblTypeADldFreeVal
    '
    Me.lblTypeADldFreeVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblTypeADldFreeVal.AutoSize = True
    Me.lblTypeADldFreeVal.Location = New System.Drawing.Point(38, 7)
    Me.lblTypeADldFreeVal.Margin = New System.Windows.Forms.Padding(0)
    Me.lblTypeADldFreeVal.Name = "lblTypeADldFreeVal"
    Me.lblTypeADldFreeVal.Size = New System.Drawing.Size(19, 13)
    Me.lblTypeADldFreeVal.TabIndex = 3
    Me.lblTypeADldFreeVal.Text = " -- "
    '
    'lblTypeADldFree
    '
    Me.lblTypeADldFree.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTypeADldFree.AutoSize = True
    Me.lblTypeADldFree.Location = New System.Drawing.Point(3, 7)
    Me.lblTypeADldFree.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblTypeADldFree.Name = "lblTypeADldFree"
    Me.lblTypeADldFree.Size = New System.Drawing.Size(31, 13)
    Me.lblTypeADldFree.TabIndex = 2
    Me.lblTypeADldFree.Text = "Free:"
    '
    'pnlTypeADldLimit
    '
    Me.pnlTypeADldLimit.AutoSize = True
    Me.pnlTypeADldLimit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlTypeADldLimit.ColumnCount = 2
    Me.pnlTypeADldLimit.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeADldLimit.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeADldLimit.Controls.Add(Me.lblTypeADldLimitVal, 1, 0)
    Me.pnlTypeADldLimit.Controls.Add(Me.lblTypeADldLimit, 0, 0)
    Me.pnlTypeADldLimit.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlTypeADldLimit.Location = New System.Drawing.Point(0, 56)
    Me.pnlTypeADldLimit.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlTypeADldLimit.Name = "pnlTypeADldLimit"
    Me.pnlTypeADldLimit.RowCount = 1
    Me.pnlTypeADldLimit.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlTypeADldLimit.Size = New System.Drawing.Size(57, 28)
    Me.pnlTypeADldLimit.TabIndex = 8
    '
    'lblTypeADldLimitVal
    '
    Me.lblTypeADldLimitVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblTypeADldLimitVal.AutoSize = True
    Me.lblTypeADldLimitVal.Location = New System.Drawing.Point(38, 7)
    Me.lblTypeADldLimitVal.Margin = New System.Windows.Forms.Padding(0)
    Me.lblTypeADldLimitVal.Name = "lblTypeADldLimitVal"
    Me.lblTypeADldLimitVal.Size = New System.Drawing.Size(19, 13)
    Me.lblTypeADldLimitVal.TabIndex = 5
    Me.lblTypeADldLimitVal.Text = " -- "
    '
    'lblTypeADldLimit
    '
    Me.lblTypeADldLimit.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTypeADldLimit.AutoSize = True
    Me.lblTypeADldLimit.Location = New System.Drawing.Point(3, 7)
    Me.lblTypeADldLimit.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblTypeADldLimit.Name = "lblTypeADldLimit"
    Me.lblTypeADldLimit.Size = New System.Drawing.Size(31, 13)
    Me.lblTypeADldLimit.TabIndex = 4
    Me.lblTypeADldLimit.Text = "Limit:"
    '
    'pnlTypeB
    '
    Me.pnlTypeB.ColumnCount = 2
    Me.pnlTypeB.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeB.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlTypeB.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlTypeB.Controls.Add(Me.pnlTypeBUsed, 0, 0)
    Me.pnlTypeB.Controls.Add(Me.pnlTypeBFree, 0, 1)
    Me.pnlTypeB.Controls.Add(Me.pnlTypeBLimit, 0, 2)
    Me.pnlTypeB.Controls.Add(Me.pctTypeB, 1, 0)
    Me.pnlTypeB.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlTypeB.Location = New System.Drawing.Point(3, 16)
    Me.pnlTypeB.Name = "pnlTypeB"
    Me.pnlTypeB.RowCount = 3
    Me.pnlTypeB.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlTypeB.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334!))
    Me.pnlTypeB.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlTypeB.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlTypeB.Size = New System.Drawing.Size(422, 109)
    Me.pnlTypeB.TabIndex = 3
    '
    'pnlTypeBUsed
    '
    Me.pnlTypeBUsed.AutoSize = True
    Me.pnlTypeBUsed.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlTypeBUsed.ColumnCount = 2
    Me.pnlTypeBUsed.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeBUsed.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeBUsed.Controls.Add(Me.lblTypeBUsed, 0, 0)
    Me.pnlTypeBUsed.Controls.Add(Me.lblTypeBUsedVal, 1, 0)
    Me.pnlTypeBUsed.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlTypeBUsed.Location = New System.Drawing.Point(0, 0)
    Me.pnlTypeBUsed.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlTypeBUsed.Name = "pnlTypeBUsed"
    Me.pnlTypeBUsed.RowCount = 1
    Me.pnlTypeBUsed.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlTypeBUsed.Size = New System.Drawing.Size(66, 36)
    Me.pnlTypeBUsed.TabIndex = 18
    '
    'lblTypeBUsed
    '
    Me.lblTypeBUsed.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTypeBUsed.AutoSize = True
    Me.lblTypeBUsed.Location = New System.Drawing.Point(3, 11)
    Me.lblTypeBUsed.Name = "lblTypeBUsed"
    Me.lblTypeBUsed.Size = New System.Drawing.Size(35, 13)
    Me.lblTypeBUsed.TabIndex = 6
    Me.lblTypeBUsed.Text = "Used:"
    '
    'lblTypeBUsedVal
    '
    Me.lblTypeBUsedVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblTypeBUsedVal.AutoSize = True
    Me.lblTypeBUsedVal.Location = New System.Drawing.Point(44, 11)
    Me.lblTypeBUsedVal.Name = "lblTypeBUsedVal"
    Me.lblTypeBUsedVal.Size = New System.Drawing.Size(19, 13)
    Me.lblTypeBUsedVal.TabIndex = 12
    Me.lblTypeBUsedVal.Text = " -- "
    '
    'pnlTypeBFree
    '
    Me.pnlTypeBFree.AutoSize = True
    Me.pnlTypeBFree.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlTypeBFree.ColumnCount = 2
    Me.pnlTypeBFree.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeBFree.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeBFree.Controls.Add(Me.lblTypeBFree, 0, 0)
    Me.pnlTypeBFree.Controls.Add(Me.lblTypeBFreeVal, 1, 0)
    Me.pnlTypeBFree.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlTypeBFree.Location = New System.Drawing.Point(0, 36)
    Me.pnlTypeBFree.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlTypeBFree.Name = "pnlTypeBFree"
    Me.pnlTypeBFree.RowCount = 1
    Me.pnlTypeBFree.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlTypeBFree.Size = New System.Drawing.Size(66, 36)
    Me.pnlTypeBFree.TabIndex = 19
    '
    'lblTypeBFree
    '
    Me.lblTypeBFree.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTypeBFree.AutoSize = True
    Me.lblTypeBFree.Location = New System.Drawing.Point(3, 11)
    Me.lblTypeBFree.Name = "lblTypeBFree"
    Me.lblTypeBFree.Size = New System.Drawing.Size(31, 13)
    Me.lblTypeBFree.TabIndex = 15
    Me.lblTypeBFree.Text = "Free:"
    '
    'lblTypeBFreeVal
    '
    Me.lblTypeBFreeVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblTypeBFreeVal.AutoSize = True
    Me.lblTypeBFreeVal.Location = New System.Drawing.Point(44, 11)
    Me.lblTypeBFreeVal.Name = "lblTypeBFreeVal"
    Me.lblTypeBFreeVal.Size = New System.Drawing.Size(19, 13)
    Me.lblTypeBFreeVal.TabIndex = 16
    Me.lblTypeBFreeVal.Text = " -- "
    '
    'pnlTypeBLimit
    '
    Me.pnlTypeBLimit.AutoSize = True
    Me.pnlTypeBLimit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlTypeBLimit.ColumnCount = 2
    Me.pnlTypeBLimit.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeBLimit.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlTypeBLimit.Controls.Add(Me.lblTypeBLimit, 0, 0)
    Me.pnlTypeBLimit.Controls.Add(Me.lblTypeBLimitVal, 1, 0)
    Me.pnlTypeBLimit.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlTypeBLimit.Location = New System.Drawing.Point(0, 72)
    Me.pnlTypeBLimit.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlTypeBLimit.Name = "pnlTypeBLimit"
    Me.pnlTypeBLimit.RowCount = 1
    Me.pnlTypeBLimit.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlTypeBLimit.Size = New System.Drawing.Size(66, 37)
    Me.pnlTypeBLimit.TabIndex = 20
    '
    'lblTypeBLimit
    '
    Me.lblTypeBLimit.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTypeBLimit.AutoSize = True
    Me.lblTypeBLimit.Location = New System.Drawing.Point(3, 12)
    Me.lblTypeBLimit.Name = "lblTypeBLimit"
    Me.lblTypeBLimit.Size = New System.Drawing.Size(31, 13)
    Me.lblTypeBLimit.TabIndex = 7
    Me.lblTypeBLimit.Text = "Limit:"
    '
    'lblTypeBLimitVal
    '
    Me.lblTypeBLimitVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblTypeBLimitVal.AutoSize = True
    Me.lblTypeBLimitVal.Location = New System.Drawing.Point(44, 12)
    Me.lblTypeBLimitVal.Name = "lblTypeBLimitVal"
    Me.lblTypeBLimitVal.Size = New System.Drawing.Size(19, 13)
    Me.lblTypeBLimitVal.TabIndex = 14
    Me.lblTypeBLimitVal.Text = " -- "
    '
    'pctTypeB
    '
    Me.pctTypeB.BackColor = System.Drawing.Color.White
    Me.pctTypeB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.pctTypeB.ContextMenu = Me.mnuGraph
    Me.pctTypeB.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pctTypeB.Location = New System.Drawing.Point(66, 0)
    Me.pctTypeB.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
    Me.pctTypeB.Name = "pctTypeB"
    Me.pnlTypeB.SetRowSpan(Me.pctTypeB, 3)
    Me.pctTypeB.Size = New System.Drawing.Size(353, 109)
    Me.pctTypeB.TabIndex = 1
    Me.pctTypeB.TabStop = False
    Me.ttUI.SetToolTip(Me.pctTypeB, "Graph representing your usage.")
    '
    'trayIcon
    '
    Me.trayIcon.ContextMenu = Me.mnuTray
    Me.trayIcon.Text = "Satellite Restriction Tracker"
    '
    'mnuTray
    '
    Me.mnuTray.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuRestore, Me.mnuAbout, Me.mnuSpacer, Me.mnuExit})
    '
    'mnuRestore
    '
    Me.mnuRestore.DefaultItem = True
    Me.mnuRestore.Index = 0
    Me.mnuRestore.Text = "&Restore"
    '
    'mnuAbout
    '
    Me.mnuAbout.Index = 1
    Me.mnuAbout.Text = "&About"
    '
    'mnuSpacer
    '
    Me.mnuSpacer.Index = 2
    Me.mnuSpacer.Text = "-"
    '
    'mnuExit
    '
    Me.mnuExit.Index = 3
    Me.mnuExit.Text = "E&xit"
    '
    'tmrUpdate
    '
    Me.tmrUpdate.Enabled = True
    Me.tmrUpdate.Interval = 1000
    '
    'tmrIcon
    '
    '
    'tmrIcoDelay
    '
    Me.tmrIcoDelay.Interval = 750
    '
    'tmrSpeed
    '
    Me.tmrSpeed.Interval = 1000
    '
    'tmrStatus
    '
    Me.tmrStatus.Enabled = True
    Me.tmrStatus.Interval = 500
    '
    'ttUI
    '
    Me.ttUI.AutoPopDelay = 30000
    Me.ttUI.InitialDelay = 500
    Me.ttUI.ReshowDelay = 100
    '
    'frmMain
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
    Me.ClientSize = New System.Drawing.Size(434, 162)
    Me.Controls.Add(Me.pnlDetails)
    Me.Icon = Global.RestrictionTracker.My.Resources.Resources.sat
    Me.MinimumSize = New System.Drawing.Size(400, 200)
    Me.Name = "frmMain"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "Satellite Restriction Tracker"
    Me.pnlDetails.ResumeLayout(False)
    Me.pnlDetails.PerformLayout()
    Me.pnlSettings.ResumeLayout(False)
    Me.pnlSettings.PerformLayout()
    Me.gbUsage.ResumeLayout(False)
    Me.gbUsage.PerformLayout()
    CType(Me.pctNetTest, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlNothing.ResumeLayout(False)
    Me.pnlNothing.PerformLayout()
    Me.pnlTypeA.ResumeLayout(False)
    Me.gbTypeAUld.ResumeLayout(False)
    Me.pnlTypeAUld.ResumeLayout(False)
    Me.pnlTypeAUld.PerformLayout()
    CType(Me.pctTypeAUld, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlUldText.ResumeLayout(False)
    Me.pnlUldText.PerformLayout()
    Me.pnlTypeAUldUsed.ResumeLayout(False)
    Me.pnlTypeAUldUsed.PerformLayout()
    Me.pnlTypeAUldFree.ResumeLayout(False)
    Me.pnlTypeAUldFree.PerformLayout()
    Me.pnlTypeAUldLimit.ResumeLayout(False)
    Me.pnlTypeAUldLimit.PerformLayout()
    Me.gbTypeADld.ResumeLayout(False)
    Me.pnlTypeADld.ResumeLayout(False)
    Me.pnlTypeADld.PerformLayout()
    CType(Me.pctTypeADld, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlDldText.ResumeLayout(False)
    Me.pnlDldText.PerformLayout()
    Me.pnlTypeADldUsed.ResumeLayout(False)
    Me.pnlTypeADldUsed.PerformLayout()
    Me.pnlTypeADldFree.ResumeLayout(False)
    Me.pnlTypeADldFree.PerformLayout()
    Me.pnlTypeADldLimit.ResumeLayout(False)
    Me.pnlTypeADldLimit.PerformLayout()
    Me.pnlTypeB.ResumeLayout(False)
    Me.pnlTypeB.PerformLayout()
    Me.pnlTypeBUsed.ResumeLayout(False)
    Me.pnlTypeBUsed.PerformLayout()
    Me.pnlTypeBFree.ResumeLayout(False)
    Me.pnlTypeBFree.PerformLayout()
    Me.pnlTypeBLimit.ResumeLayout(False)
    Me.pnlTypeBLimit.PerformLayout()
    CType(Me.pctTypeB, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents pnlDetails As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlSettings As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdHistory As System.Windows.Forms.Button
  Friend WithEvents gbUsage As System.Windows.Forms.GroupBox
  Friend WithEvents gbTypeAUld As System.Windows.Forms.GroupBox
  Friend WithEvents gbTypeADld As System.Windows.Forms.GroupBox
  Friend WithEvents pnlTypeADld As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctTypeADld As System.Windows.Forms.PictureBox
  Friend WithEvents pnlDldText As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblTypeADldUsed As System.Windows.Forms.Label
  Friend WithEvents lblTypeADldUsedVal As System.Windows.Forms.Label
  Friend WithEvents lblTypeADldFree As System.Windows.Forms.Label
  Friend WithEvents lblTypeADldFreeVal As System.Windows.Forms.Label
  Friend WithEvents lblTypeADldLimit As System.Windows.Forms.Label
  Friend WithEvents lblTypeADldLimitVal As System.Windows.Forms.Label
  Friend WithEvents pnlTypeAUld As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctTypeAUld As System.Windows.Forms.PictureBox
  Friend WithEvents pnlUldText As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblTypeAUldUsed As System.Windows.Forms.Label
  Friend WithEvents lblTypeAUldUsedVal As System.Windows.Forms.Label
  Friend WithEvents lblTypeAUldFree As System.Windows.Forms.Label
  Friend WithEvents lblTypeAUldFreeVal As System.Windows.Forms.Label
  Friend WithEvents lblTypeAUldLimit As System.Windows.Forms.Label
  Friend WithEvents lblTypeAUldLimitVal As System.Windows.Forms.Label
  Friend WithEvents trayIcon As System.Windows.Forms.NotifyIcon
  Friend WithEvents ttUI As ToolTip
  Friend WithEvents pnlTypeA As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents tmrUpdate As System.Windows.Forms.Timer
  Friend WithEvents cmdAbout As System.Windows.Forms.Button
  Friend WithEvents cmdRefresh As System.Windows.Forms.Button
  Friend WithEvents cmdConfig As System.Windows.Forms.Button
  Friend WithEvents pnlTypeB As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctTypeB As System.Windows.Forms.PictureBox
  Friend WithEvents lblTypeBUsed As System.Windows.Forms.Label
  Friend WithEvents lblTypeBUsedVal As System.Windows.Forms.Label
  Friend WithEvents lblTypeBFree As System.Windows.Forms.Label
  Friend WithEvents lblTypeBFreeVal As System.Windows.Forms.Label
  Friend WithEvents lblTypeBLimit As System.Windows.Forms.Label
  Friend WithEvents lblTypeBLimitVal As System.Windows.Forms.Label
  Friend WithEvents pnlTypeADldUsed As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlTypeADldFree As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlTypeADldLimit As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlTypeAUldUsed As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlTypeAUldFree As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlTypeAUldLimit As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlTypeBUsed As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlTypeBFree As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlTypeBLimit As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents tmrIcon As System.Windows.Forms.Timer
  Friend WithEvents mnuTray As System.Windows.Forms.ContextMenu
  Friend WithEvents mnuRestore As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAbout As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSpacer As System.Windows.Forms.MenuItem
  Friend WithEvents mnuExit As System.Windows.Forms.MenuItem
  Friend WithEvents tmrIcoDelay As System.Windows.Forms.Timer
  Friend WithEvents lblStatus As System.Windows.Forms.Label
  Friend WithEvents tmrSpeed As System.Windows.Forms.Timer
  Friend WithEvents tmrStatus As System.Windows.Forms.Timer
  Friend WithEvents pnlNothing As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblNothing As System.Windows.Forms.Label
  Friend WithEvents lblRRS As RestrictionTracker.LinkLabel
  Friend WithEvents mnuGraph As System.Windows.Forms.ContextMenu
  Friend WithEvents mnuGraphRefresh As System.Windows.Forms.MenuItem
  Friend WithEvents mnuGraphSpace As System.Windows.Forms.MenuItem
  Friend WithEvents mnuGraphColors As System.Windows.Forms.MenuItem
  Friend WithEvents mnuGraphInvert As System.Windows.Forms.MenuItem
  Friend WithEvents pctNetTest As LinkPictureBox
End Class
