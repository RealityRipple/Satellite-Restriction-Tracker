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
    Me.mnuGraph = New System.Windows.Forms.ContextMenu()
    Me.mnuGraphRefresh = New System.Windows.Forms.MenuItem()
    Me.mnuGraphSpace = New System.Windows.Forms.MenuItem()
    Me.mnuGraphInvert = New System.Windows.Forms.MenuItem()
    Me.mnuGraphColors = New System.Windows.Forms.MenuItem()
    Me.pnlUsage = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlUsageUsed = New System.Windows.Forms.TableLayoutPanel()
    Me.lblUsageUsed = New System.Windows.Forms.Label()
    Me.lblUsageUsedVal = New System.Windows.Forms.Label()
    Me.pnlUsageFree = New System.Windows.Forms.TableLayoutPanel()
    Me.lblUsageFree = New System.Windows.Forms.Label()
    Me.lblUsageFreeVal = New System.Windows.Forms.Label()
    Me.pnlUsageLimit = New System.Windows.Forms.TableLayoutPanel()
    Me.lblUsageLimit = New System.Windows.Forms.Label()
    Me.lblUsageLimitVal = New System.Windows.Forms.Label()
    Me.pctUsage = New System.Windows.Forms.PictureBox()
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
    Me.pnlUsage.SuspendLayout()
    Me.pnlUsageUsed.SuspendLayout()
    Me.pnlUsageFree.SuspendLayout()
    Me.pnlUsageLimit.SuspendLayout()
    CType(Me.pctUsage, System.ComponentModel.ISupportInitialize).BeginInit()
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
    Me.gbUsage.Controls.Add(Me.pnlUsage)
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
    'pnlUsage
    '
    Me.pnlUsage.ColumnCount = 2
    Me.pnlUsage.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlUsage.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUsage.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlUsage.Controls.Add(Me.pnlUsageUsed, 0, 0)
    Me.pnlUsage.Controls.Add(Me.pnlUsageFree, 0, 1)
    Me.pnlUsage.Controls.Add(Me.pnlUsageLimit, 0, 2)
    Me.pnlUsage.Controls.Add(Me.pctUsage, 1, 0)
    Me.pnlUsage.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlUsage.Location = New System.Drawing.Point(3, 16)
    Me.pnlUsage.Name = "pnlUsage"
    Me.pnlUsage.RowCount = 3
    Me.pnlUsage.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlUsage.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334!))
    Me.pnlUsage.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlUsage.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlUsage.Size = New System.Drawing.Size(422, 109)
    Me.pnlUsage.TabIndex = 3
    '
    'pnlUsageUsed
    '
    Me.pnlUsageUsed.AutoSize = True
    Me.pnlUsageUsed.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlUsageUsed.ColumnCount = 2
    Me.pnlUsageUsed.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlUsageUsed.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlUsageUsed.Controls.Add(Me.lblUsageUsed, 0, 0)
    Me.pnlUsageUsed.Controls.Add(Me.lblUsageUsedVal, 1, 0)
    Me.pnlUsageUsed.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlUsageUsed.Location = New System.Drawing.Point(0, 0)
    Me.pnlUsageUsed.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlUsageUsed.Name = "pnlUsageUsed"
    Me.pnlUsageUsed.RowCount = 1
    Me.pnlUsageUsed.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUsageUsed.Size = New System.Drawing.Size(66, 36)
    Me.pnlUsageUsed.TabIndex = 18
    '
    'lblUsageUsed
    '
    Me.lblUsageUsed.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblUsageUsed.AutoSize = True
    Me.lblUsageUsed.Location = New System.Drawing.Point(3, 11)
    Me.lblUsageUsed.Name = "lblUsageUsed"
    Me.lblUsageUsed.Size = New System.Drawing.Size(35, 13)
    Me.lblUsageUsed.TabIndex = 6
    Me.lblUsageUsed.Text = "Used:"
    '
    'lblUsageUsedVal
    '
    Me.lblUsageUsedVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblUsageUsedVal.AutoSize = True
    Me.lblUsageUsedVal.Location = New System.Drawing.Point(44, 11)
    Me.lblUsageUsedVal.Name = "lblUsageUsedVal"
    Me.lblUsageUsedVal.Size = New System.Drawing.Size(19, 13)
    Me.lblUsageUsedVal.TabIndex = 12
    Me.lblUsageUsedVal.Text = " -- "
    '
    'pnlUsageFree
    '
    Me.pnlUsageFree.AutoSize = True
    Me.pnlUsageFree.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlUsageFree.ColumnCount = 2
    Me.pnlUsageFree.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlUsageFree.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlUsageFree.Controls.Add(Me.lblUsageFree, 0, 0)
    Me.pnlUsageFree.Controls.Add(Me.lblUsageFreeVal, 1, 0)
    Me.pnlUsageFree.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlUsageFree.Location = New System.Drawing.Point(0, 36)
    Me.pnlUsageFree.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlUsageFree.Name = "pnlUsageFree"
    Me.pnlUsageFree.RowCount = 1
    Me.pnlUsageFree.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUsageFree.Size = New System.Drawing.Size(66, 36)
    Me.pnlUsageFree.TabIndex = 19
    '
    'lblUsageFree
    '
    Me.lblUsageFree.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblUsageFree.AutoSize = True
    Me.lblUsageFree.Location = New System.Drawing.Point(3, 11)
    Me.lblUsageFree.Name = "lblUsageFree"
    Me.lblUsageFree.Size = New System.Drawing.Size(31, 13)
    Me.lblUsageFree.TabIndex = 15
    Me.lblUsageFree.Text = "Free:"
    '
    'lblUsageFreeVal
    '
    Me.lblUsageFreeVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblUsageFreeVal.AutoSize = True
    Me.lblUsageFreeVal.Location = New System.Drawing.Point(44, 11)
    Me.lblUsageFreeVal.Name = "lblUsageFreeVal"
    Me.lblUsageFreeVal.Size = New System.Drawing.Size(19, 13)
    Me.lblUsageFreeVal.TabIndex = 16
    Me.lblUsageFreeVal.Text = " -- "
    '
    'pnlUsageLimit
    '
    Me.pnlUsageLimit.AutoSize = True
    Me.pnlUsageLimit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlUsageLimit.ColumnCount = 2
    Me.pnlUsageLimit.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlUsageLimit.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlUsageLimit.Controls.Add(Me.lblUsageLimit, 0, 0)
    Me.pnlUsageLimit.Controls.Add(Me.lblUsageLimitVal, 1, 0)
    Me.pnlUsageLimit.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlUsageLimit.Location = New System.Drawing.Point(0, 72)
    Me.pnlUsageLimit.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlUsageLimit.Name = "pnlUsageLimit"
    Me.pnlUsageLimit.RowCount = 1
    Me.pnlUsageLimit.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUsageLimit.Size = New System.Drawing.Size(66, 37)
    Me.pnlUsageLimit.TabIndex = 20
    '
    'lblUsageLimit
    '
    Me.lblUsageLimit.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblUsageLimit.AutoSize = True
    Me.lblUsageLimit.Location = New System.Drawing.Point(3, 12)
    Me.lblUsageLimit.Name = "lblUsageLimit"
    Me.lblUsageLimit.Size = New System.Drawing.Size(31, 13)
    Me.lblUsageLimit.TabIndex = 7
    Me.lblUsageLimit.Text = "Limit:"
    '
    'lblUsageLimitVal
    '
    Me.lblUsageLimitVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblUsageLimitVal.AutoSize = True
    Me.lblUsageLimitVal.Location = New System.Drawing.Point(44, 12)
    Me.lblUsageLimitVal.Name = "lblUsageLimitVal"
    Me.lblUsageLimitVal.Size = New System.Drawing.Size(19, 13)
    Me.lblUsageLimitVal.TabIndex = 14
    Me.lblUsageLimitVal.Text = " -- "
    '
    'pctUsage
    '
    Me.pctUsage.BackColor = System.Drawing.Color.White
    Me.pctUsage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.pctUsage.ContextMenu = Me.mnuGraph
    Me.pctUsage.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pctUsage.Location = New System.Drawing.Point(66, 0)
    Me.pctUsage.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
    Me.pctUsage.Name = "pctUsage"
    Me.pnlUsage.SetRowSpan(Me.pctUsage, 3)
    Me.pctUsage.Size = New System.Drawing.Size(353, 109)
    Me.pctUsage.TabIndex = 1
    Me.pctUsage.TabStop = False
    Me.ttUI.SetToolTip(Me.pctUsage, "Graph representing your usage.")
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
    Me.pnlUsage.ResumeLayout(False)
    Me.pnlUsage.PerformLayout()
    Me.pnlUsageUsed.ResumeLayout(False)
    Me.pnlUsageUsed.PerformLayout()
    Me.pnlUsageFree.ResumeLayout(False)
    Me.pnlUsageFree.PerformLayout()
    Me.pnlUsageLimit.ResumeLayout(False)
    Me.pnlUsageLimit.PerformLayout()
    CType(Me.pctUsage, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents pnlDetails As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlSettings As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdHistory As System.Windows.Forms.Button
  Friend WithEvents gbUsage As System.Windows.Forms.GroupBox
  Friend WithEvents trayIcon As System.Windows.Forms.NotifyIcon
  Friend WithEvents ttUI As ToolTip
  Friend WithEvents tmrUpdate As System.Windows.Forms.Timer
  Friend WithEvents cmdAbout As System.Windows.Forms.Button
  Friend WithEvents cmdRefresh As System.Windows.Forms.Button
  Friend WithEvents cmdConfig As System.Windows.Forms.Button
  Friend WithEvents pnlUsage As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctUsage As System.Windows.Forms.PictureBox
  Friend WithEvents lblUsageUsed As System.Windows.Forms.Label
  Friend WithEvents lblUsageUsedVal As System.Windows.Forms.Label
  Friend WithEvents lblUsageFree As System.Windows.Forms.Label
  Friend WithEvents lblUsageFreeVal As System.Windows.Forms.Label
  Friend WithEvents lblUsageLimit As System.Windows.Forms.Label
  Friend WithEvents lblUsageLimitVal As System.Windows.Forms.Label
  Friend WithEvents pnlUsageUsed As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlUsageFree As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlUsageLimit As System.Windows.Forms.TableLayoutPanel
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
