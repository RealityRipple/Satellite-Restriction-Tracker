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
    Me.pnlWildBlue = New System.Windows.Forms.TableLayoutPanel()
    Me.gbUld = New System.Windows.Forms.GroupBox()
    Me.pnlUld = New System.Windows.Forms.TableLayoutPanel()
    Me.pctUld = New System.Windows.Forms.PictureBox()
    Me.mnuGraph = New System.Windows.Forms.ContextMenu()
    Me.mnuGraphRefresh = New System.Windows.Forms.MenuItem()
    Me.mnuGraphSpace = New System.Windows.Forms.MenuItem()
    Me.mnuGraphInvert = New System.Windows.Forms.MenuItem()
    Me.mnuGraphColors = New System.Windows.Forms.MenuItem()
    Me.pnlUldText = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlUldTextUsed = New System.Windows.Forms.TableLayoutPanel()
    Me.lblUldUsed = New System.Windows.Forms.Label()
    Me.lblUUsed = New System.Windows.Forms.Label()
    Me.pnlUldTextFree = New System.Windows.Forms.TableLayoutPanel()
    Me.lblUldFree = New System.Windows.Forms.Label()
    Me.lblUFree = New System.Windows.Forms.Label()
    Me.pnlUldTextTotal = New System.Windows.Forms.TableLayoutPanel()
    Me.lblUldTotal = New System.Windows.Forms.Label()
    Me.lblUTotal = New System.Windows.Forms.Label()
    Me.gbDld = New System.Windows.Forms.GroupBox()
    Me.pnlDld = New System.Windows.Forms.TableLayoutPanel()
    Me.pctDld = New System.Windows.Forms.PictureBox()
    Me.pnlDldText = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlDldTextUsed = New System.Windows.Forms.TableLayoutPanel()
    Me.lblDldUsed = New System.Windows.Forms.Label()
    Me.lblDUsed = New System.Windows.Forms.Label()
    Me.pnlDldTextFree = New System.Windows.Forms.TableLayoutPanel()
    Me.lblDldFree = New System.Windows.Forms.Label()
    Me.lblDFree = New System.Windows.Forms.Label()
    Me.pnlDldTextTotal = New System.Windows.Forms.TableLayoutPanel()
    Me.lblDldTotal = New System.Windows.Forms.Label()
    Me.lblDTotal = New System.Windows.Forms.Label()
    Me.pnlExede = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlExedeUp = New System.Windows.Forms.TableLayoutPanel()
    Me.lblExedeUp = New System.Windows.Forms.Label()
    Me.lblExedeUpVal = New System.Windows.Forms.Label()
    Me.pnlExedeUsed = New System.Windows.Forms.TableLayoutPanel()
    Me.lblExedeTotal = New System.Windows.Forms.Label()
    Me.lblExedeTotalVal = New System.Windows.Forms.Label()
    Me.pnlExedeFree = New System.Windows.Forms.TableLayoutPanel()
    Me.lblExedeRemain = New System.Windows.Forms.Label()
    Me.lblExedeRemainVal = New System.Windows.Forms.Label()
    Me.pnlExedeTotal = New System.Windows.Forms.TableLayoutPanel()
    Me.lblExedeAllowed = New System.Windows.Forms.Label()
    Me.lblExedeAllowedVal = New System.Windows.Forms.Label()
    Me.pnlExedeDown = New System.Windows.Forms.TableLayoutPanel()
    Me.lblExedeDown = New System.Windows.Forms.Label()
    Me.lblExedeDownVal = New System.Windows.Forms.Label()
    Me.pctExede = New System.Windows.Forms.PictureBox()
    Me.pnlRural = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlRuralUsed = New System.Windows.Forms.TableLayoutPanel()
    Me.lblRuralUsed = New System.Windows.Forms.Label()
    Me.lblRuralUsedVal = New System.Windows.Forms.Label()
    Me.pnlRuralFree = New System.Windows.Forms.TableLayoutPanel()
    Me.lblRuralRemain = New System.Windows.Forms.Label()
    Me.lblRuralRemainVal = New System.Windows.Forms.Label()
    Me.pnlRuralTotal = New System.Windows.Forms.TableLayoutPanel()
    Me.lblRuralAllowed = New System.Windows.Forms.Label()
    Me.lblRuralAllowedVal = New System.Windows.Forms.Label()
    Me.pctRural = New System.Windows.Forms.PictureBox()
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
    Me.pnlWildBlue.SuspendLayout()
    Me.gbUld.SuspendLayout()
    Me.pnlUld.SuspendLayout()
    CType(Me.pctUld, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlUldText.SuspendLayout()
    Me.pnlUldTextUsed.SuspendLayout()
    Me.pnlUldTextFree.SuspendLayout()
    Me.pnlUldTextTotal.SuspendLayout()
    Me.gbDld.SuspendLayout()
    Me.pnlDld.SuspendLayout()
    CType(Me.pctDld, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlDldText.SuspendLayout()
    Me.pnlDldTextUsed.SuspendLayout()
    Me.pnlDldTextFree.SuspendLayout()
    Me.pnlDldTextTotal.SuspendLayout()
    Me.pnlExede.SuspendLayout()
    Me.pnlExedeUp.SuspendLayout()
    Me.pnlExedeUsed.SuspendLayout()
    Me.pnlExedeFree.SuspendLayout()
    Me.pnlExedeTotal.SuspendLayout()
    Me.pnlExedeDown.SuspendLayout()
    CType(Me.pctExede, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlRural.SuspendLayout()
    Me.pnlRuralUsed.SuspendLayout()
    Me.pnlRuralFree.SuspendLayout()
    Me.pnlRuralTotal.SuspendLayout()
    CType(Me.pctRural, System.ComponentModel.ISupportInitialize).BeginInit()
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
    Me.ttUI.SetTooltip(Me.cmdAbout, "View information about Satellite Restriction Tracker and check for updates.")
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
    Me.ttUI.SetTooltip(Me.cmdHistory, "View your usage history.")
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
    Me.ttUI.SetTooltip(Me.cmdRefresh, "Reload usage level information immediately." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Hold CTRL to reload database.)")
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
    Me.ttUI.SetTooltip(Me.cmdConfig, "Change program settings.")
    Me.cmdConfig.UseVisualStyleBackColor = True
    '
    'gbUsage
    '
    Me.gbUsage.Controls.Add(Me.pctNetTest)
    Me.gbUsage.Controls.Add(Me.lblStatus)
    Me.gbUsage.Controls.Add(Me.pnlNothing)
    Me.gbUsage.Controls.Add(Me.pnlWildBlue)
    Me.gbUsage.Controls.Add(Me.pnlExede)
    Me.gbUsage.Controls.Add(Me.pnlRural)
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
    Me.pctNetTest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
    Me.pctNetTest.TabIndex = 6
    Me.pctNetTest.TabStop = False
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
    'pnlWildBlue
    '
    Me.pnlWildBlue.ColumnCount = 2
    Me.pnlWildBlue.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlWildBlue.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlWildBlue.Controls.Add(Me.gbUld, 1, 0)
    Me.pnlWildBlue.Controls.Add(Me.gbDld, 0, 0)
    Me.pnlWildBlue.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlWildBlue.Location = New System.Drawing.Point(3, 16)
    Me.pnlWildBlue.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlWildBlue.Name = "pnlWildBlue"
    Me.pnlWildBlue.RowCount = 1
    Me.pnlWildBlue.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlWildBlue.Size = New System.Drawing.Size(422, 109)
    Me.pnlWildBlue.TabIndex = 1
    '
    'gbUld
    '
    Me.gbUld.Controls.Add(Me.pnlUld)
    Me.gbUld.Dock = System.Windows.Forms.DockStyle.Fill
    Me.gbUld.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.gbUld.Location = New System.Drawing.Point(214, 3)
    Me.gbUld.Name = "gbUld"
    Me.gbUld.Size = New System.Drawing.Size(205, 103)
    Me.gbUld.TabIndex = 0
    Me.gbUld.TabStop = False
    Me.gbUld.Text = "Upload"
    '
    'pnlUld
    '
    Me.pnlUld.ColumnCount = 2
    Me.pnlUld.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlUld.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUld.Controls.Add(Me.pctUld, 1, 0)
    Me.pnlUld.Controls.Add(Me.pnlUldText, 0, 0)
    Me.pnlUld.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlUld.Location = New System.Drawing.Point(3, 16)
    Me.pnlUld.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlUld.Name = "pnlUld"
    Me.pnlUld.RowCount = 1
    Me.pnlUld.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUld.Size = New System.Drawing.Size(199, 84)
    Me.pnlUld.TabIndex = 1
    '
    'pctUld
    '
    Me.pctUld.BackColor = System.Drawing.Color.White
    Me.pctUld.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.pctUld.ContextMenu = Me.mnuGraph
    Me.pctUld.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pctUld.Location = New System.Drawing.Point(75, 0)
    Me.pctUld.Margin = New System.Windows.Forms.Padding(0, 0, 3, 3)
    Me.pctUld.Name = "pctUld"
    Me.pctUld.Size = New System.Drawing.Size(121, 81)
    Me.pctUld.TabIndex = 0
    Me.pctUld.TabStop = False
    Me.ttUI.SetTooltip(Me.pctUld, "Graph representing your upload usage.")
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
    Me.pnlUldText.Controls.Add(Me.pnlUldTextUsed, 0, 0)
    Me.pnlUldText.Controls.Add(Me.pnlUldTextFree, 0, 1)
    Me.pnlUldText.Controls.Add(Me.pnlUldTextTotal, 0, 2)
    Me.pnlUldText.Dock = System.Windows.Forms.DockStyle.Left
    Me.pnlUldText.Location = New System.Drawing.Point(0, 0)
    Me.pnlUldText.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlUldText.Name = "pnlUldText"
    Me.pnlUldText.RowCount = 3
    Me.pnlUldText.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlUldText.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlUldText.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlUldText.Size = New System.Drawing.Size(75, 84)
    Me.pnlUldText.TabIndex = 1
    '
    'pnlUldTextUsed
    '
    Me.pnlUldTextUsed.AutoSize = True
    Me.pnlUldTextUsed.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlUldTextUsed.ColumnCount = 2
    Me.pnlUldTextUsed.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlUldTextUsed.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlUldTextUsed.Controls.Add(Me.lblUldUsed, 1, 0)
    Me.pnlUldTextUsed.Controls.Add(Me.lblUUsed, 0, 0)
    Me.pnlUldTextUsed.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlUldTextUsed.Location = New System.Drawing.Point(0, 0)
    Me.pnlUldTextUsed.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlUldTextUsed.Name = "pnlUldTextUsed"
    Me.pnlUldTextUsed.RowCount = 1
    Me.pnlUldTextUsed.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUldTextUsed.Size = New System.Drawing.Size(75, 28)
    Me.pnlUldTextUsed.TabIndex = 6
    '
    'lblUldUsed
    '
    Me.lblUldUsed.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblUldUsed.AutoSize = True
    Me.lblUldUsed.Location = New System.Drawing.Point(56, 7)
    Me.lblUldUsed.Margin = New System.Windows.Forms.Padding(0)
    Me.lblUldUsed.Name = "lblUldUsed"
    Me.lblUldUsed.Size = New System.Drawing.Size(19, 13)
    Me.lblUldUsed.TabIndex = 1
    Me.lblUldUsed.Text = " -- "
    '
    'lblUUsed
    '
    Me.lblUUsed.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblUUsed.AutoSize = True
    Me.lblUUsed.Location = New System.Drawing.Point(3, 7)
    Me.lblUUsed.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblUUsed.Name = "lblUUsed"
    Me.lblUUsed.Size = New System.Drawing.Size(35, 13)
    Me.lblUUsed.TabIndex = 0
    Me.lblUUsed.Text = "Used:"
    '
    'pnlUldTextFree
    '
    Me.pnlUldTextFree.AutoSize = True
    Me.pnlUldTextFree.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlUldTextFree.ColumnCount = 2
    Me.pnlUldTextFree.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlUldTextFree.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlUldTextFree.Controls.Add(Me.lblUldFree, 1, 0)
    Me.pnlUldTextFree.Controls.Add(Me.lblUFree, 0, 0)
    Me.pnlUldTextFree.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlUldTextFree.Location = New System.Drawing.Point(0, 28)
    Me.pnlUldTextFree.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlUldTextFree.Name = "pnlUldTextFree"
    Me.pnlUldTextFree.RowCount = 1
    Me.pnlUldTextFree.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUldTextFree.Size = New System.Drawing.Size(75, 28)
    Me.pnlUldTextFree.TabIndex = 7
    '
    'lblUldFree
    '
    Me.lblUldFree.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblUldFree.AutoSize = True
    Me.lblUldFree.Location = New System.Drawing.Point(56, 7)
    Me.lblUldFree.Margin = New System.Windows.Forms.Padding(0)
    Me.lblUldFree.Name = "lblUldFree"
    Me.lblUldFree.Size = New System.Drawing.Size(19, 13)
    Me.lblUldFree.TabIndex = 3
    Me.lblUldFree.Text = " -- "
    '
    'lblUFree
    '
    Me.lblUFree.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblUFree.AutoSize = True
    Me.lblUFree.Location = New System.Drawing.Point(3, 7)
    Me.lblUFree.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblUFree.Name = "lblUFree"
    Me.lblUFree.Size = New System.Drawing.Size(53, 13)
    Me.lblUFree.TabIndex = 2
    Me.lblUFree.Text = "Available:"
    '
    'pnlUldTextTotal
    '
    Me.pnlUldTextTotal.AutoSize = True
    Me.pnlUldTextTotal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlUldTextTotal.ColumnCount = 2
    Me.pnlUldTextTotal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlUldTextTotal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlUldTextTotal.Controls.Add(Me.lblUldTotal, 1, 0)
    Me.pnlUldTextTotal.Controls.Add(Me.lblUTotal, 0, 0)
    Me.pnlUldTextTotal.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlUldTextTotal.Location = New System.Drawing.Point(0, 56)
    Me.pnlUldTextTotal.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlUldTextTotal.Name = "pnlUldTextTotal"
    Me.pnlUldTextTotal.RowCount = 1
    Me.pnlUldTextTotal.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUldTextTotal.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
    Me.pnlUldTextTotal.Size = New System.Drawing.Size(75, 28)
    Me.pnlUldTextTotal.TabIndex = 8
    '
    'lblUldTotal
    '
    Me.lblUldTotal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblUldTotal.AutoSize = True
    Me.lblUldTotal.Location = New System.Drawing.Point(56, 7)
    Me.lblUldTotal.Margin = New System.Windows.Forms.Padding(0)
    Me.lblUldTotal.Name = "lblUldTotal"
    Me.lblUldTotal.Size = New System.Drawing.Size(19, 13)
    Me.lblUldTotal.TabIndex = 5
    Me.lblUldTotal.Text = " -- "
    '
    'lblUTotal
    '
    Me.lblUTotal.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblUTotal.AutoSize = True
    Me.lblUTotal.Location = New System.Drawing.Point(3, 7)
    Me.lblUTotal.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblUTotal.Name = "lblUTotal"
    Me.lblUTotal.Size = New System.Drawing.Size(31, 13)
    Me.lblUTotal.TabIndex = 4
    Me.lblUTotal.Text = "Limit:"
    '
    'gbDld
    '
    Me.gbDld.Controls.Add(Me.pnlDld)
    Me.gbDld.Dock = System.Windows.Forms.DockStyle.Fill
    Me.gbDld.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.gbDld.Location = New System.Drawing.Point(3, 3)
    Me.gbDld.Name = "gbDld"
    Me.gbDld.Size = New System.Drawing.Size(205, 103)
    Me.gbDld.TabIndex = 0
    Me.gbDld.TabStop = False
    Me.gbDld.Text = "Download"
    '
    'pnlDld
    '
    Me.pnlDld.ColumnCount = 2
    Me.pnlDld.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlDld.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlDld.Controls.Add(Me.pctDld, 1, 0)
    Me.pnlDld.Controls.Add(Me.pnlDldText, 0, 0)
    Me.pnlDld.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlDld.Location = New System.Drawing.Point(3, 16)
    Me.pnlDld.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlDld.Name = "pnlDld"
    Me.pnlDld.RowCount = 1
    Me.pnlDld.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlDld.Size = New System.Drawing.Size(199, 84)
    Me.pnlDld.TabIndex = 0
    '
    'pctDld
    '
    Me.pctDld.BackColor = System.Drawing.Color.White
    Me.pctDld.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.pctDld.ContextMenu = Me.mnuGraph
    Me.pctDld.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pctDld.Location = New System.Drawing.Point(75, 0)
    Me.pctDld.Margin = New System.Windows.Forms.Padding(0, 0, 3, 3)
    Me.pctDld.Name = "pctDld"
    Me.pctDld.Size = New System.Drawing.Size(121, 81)
    Me.pctDld.TabIndex = 0
    Me.pctDld.TabStop = False
    Me.ttUI.SetTooltip(Me.pctDld, "Graph representing your download usage.")
    '
    'pnlDldText
    '
    Me.pnlDldText.AutoSize = True
    Me.pnlDldText.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlDldText.ColumnCount = 1
    Me.pnlDldText.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlDldText.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlDldText.Controls.Add(Me.pnlDldTextUsed, 0, 0)
    Me.pnlDldText.Controls.Add(Me.pnlDldTextFree, 0, 1)
    Me.pnlDldText.Controls.Add(Me.pnlDldTextTotal, 0, 2)
    Me.pnlDldText.Dock = System.Windows.Forms.DockStyle.Left
    Me.pnlDldText.Location = New System.Drawing.Point(0, 0)
    Me.pnlDldText.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlDldText.Name = "pnlDldText"
    Me.pnlDldText.RowCount = 3
    Me.pnlDldText.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlDldText.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlDldText.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlDldText.Size = New System.Drawing.Size(75, 84)
    Me.pnlDldText.TabIndex = 1
    '
    'pnlDldTextUsed
    '
    Me.pnlDldTextUsed.AutoSize = True
    Me.pnlDldTextUsed.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlDldTextUsed.ColumnCount = 2
    Me.pnlDldTextUsed.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlDldTextUsed.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlDldTextUsed.Controls.Add(Me.lblDldUsed, 1, 0)
    Me.pnlDldTextUsed.Controls.Add(Me.lblDUsed, 0, 0)
    Me.pnlDldTextUsed.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlDldTextUsed.Location = New System.Drawing.Point(0, 0)
    Me.pnlDldTextUsed.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlDldTextUsed.Name = "pnlDldTextUsed"
    Me.pnlDldTextUsed.RowCount = 1
    Me.pnlDldTextUsed.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlDldTextUsed.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
    Me.pnlDldTextUsed.Size = New System.Drawing.Size(75, 28)
    Me.pnlDldTextUsed.TabIndex = 6
    '
    'lblDldUsed
    '
    Me.lblDldUsed.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblDldUsed.AutoSize = True
    Me.lblDldUsed.Location = New System.Drawing.Point(56, 7)
    Me.lblDldUsed.Margin = New System.Windows.Forms.Padding(0)
    Me.lblDldUsed.Name = "lblDldUsed"
    Me.lblDldUsed.Size = New System.Drawing.Size(19, 13)
    Me.lblDldUsed.TabIndex = 1
    Me.lblDldUsed.Text = " -- "
    '
    'lblDUsed
    '
    Me.lblDUsed.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblDUsed.AutoSize = True
    Me.lblDUsed.Location = New System.Drawing.Point(3, 7)
    Me.lblDUsed.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblDUsed.Name = "lblDUsed"
    Me.lblDUsed.Size = New System.Drawing.Size(35, 13)
    Me.lblDUsed.TabIndex = 0
    Me.lblDUsed.Text = "Used:"
    '
    'pnlDldTextFree
    '
    Me.pnlDldTextFree.AutoSize = True
    Me.pnlDldTextFree.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlDldTextFree.ColumnCount = 2
    Me.pnlDldTextFree.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlDldTextFree.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlDldTextFree.Controls.Add(Me.lblDldFree, 1, 0)
    Me.pnlDldTextFree.Controls.Add(Me.lblDFree, 0, 0)
    Me.pnlDldTextFree.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlDldTextFree.Location = New System.Drawing.Point(0, 28)
    Me.pnlDldTextFree.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlDldTextFree.Name = "pnlDldTextFree"
    Me.pnlDldTextFree.RowCount = 1
    Me.pnlDldTextFree.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlDldTextFree.Size = New System.Drawing.Size(75, 28)
    Me.pnlDldTextFree.TabIndex = 7
    '
    'lblDldFree
    '
    Me.lblDldFree.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblDldFree.AutoSize = True
    Me.lblDldFree.Location = New System.Drawing.Point(56, 7)
    Me.lblDldFree.Margin = New System.Windows.Forms.Padding(0)
    Me.lblDldFree.Name = "lblDldFree"
    Me.lblDldFree.Size = New System.Drawing.Size(19, 13)
    Me.lblDldFree.TabIndex = 3
    Me.lblDldFree.Text = " -- "
    '
    'lblDFree
    '
    Me.lblDFree.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblDFree.AutoSize = True
    Me.lblDFree.Location = New System.Drawing.Point(3, 7)
    Me.lblDFree.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblDFree.Name = "lblDFree"
    Me.lblDFree.Size = New System.Drawing.Size(53, 13)
    Me.lblDFree.TabIndex = 2
    Me.lblDFree.Text = "Available:"
    '
    'pnlDldTextTotal
    '
    Me.pnlDldTextTotal.AutoSize = True
    Me.pnlDldTextTotal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlDldTextTotal.ColumnCount = 2
    Me.pnlDldTextTotal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlDldTextTotal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlDldTextTotal.Controls.Add(Me.lblDldTotal, 1, 0)
    Me.pnlDldTextTotal.Controls.Add(Me.lblDTotal, 0, 0)
    Me.pnlDldTextTotal.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlDldTextTotal.Location = New System.Drawing.Point(0, 56)
    Me.pnlDldTextTotal.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlDldTextTotal.Name = "pnlDldTextTotal"
    Me.pnlDldTextTotal.RowCount = 1
    Me.pnlDldTextTotal.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlDldTextTotal.Size = New System.Drawing.Size(75, 28)
    Me.pnlDldTextTotal.TabIndex = 8
    '
    'lblDldTotal
    '
    Me.lblDldTotal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblDldTotal.AutoSize = True
    Me.lblDldTotal.Location = New System.Drawing.Point(56, 7)
    Me.lblDldTotal.Margin = New System.Windows.Forms.Padding(0)
    Me.lblDldTotal.Name = "lblDldTotal"
    Me.lblDldTotal.Size = New System.Drawing.Size(19, 13)
    Me.lblDldTotal.TabIndex = 5
    Me.lblDldTotal.Text = " -- "
    '
    'lblDTotal
    '
    Me.lblDTotal.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblDTotal.AutoSize = True
    Me.lblDTotal.Location = New System.Drawing.Point(3, 7)
    Me.lblDTotal.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblDTotal.Name = "lblDTotal"
    Me.lblDTotal.Size = New System.Drawing.Size(31, 13)
    Me.lblDTotal.TabIndex = 4
    Me.lblDTotal.Text = "Limit:"
    '
    'pnlExede
    '
    Me.pnlExede.ColumnCount = 2
    Me.pnlExede.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlExede.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlExede.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlExede.Controls.Add(Me.pnlExedeUp, 0, 1)
    Me.pnlExede.Controls.Add(Me.pnlExedeUsed, 0, 2)
    Me.pnlExede.Controls.Add(Me.pnlExedeFree, 0, 3)
    Me.pnlExede.Controls.Add(Me.pnlExedeTotal, 0, 4)
    Me.pnlExede.Controls.Add(Me.pnlExedeDown, 0, 0)
    Me.pnlExede.Controls.Add(Me.pctExede, 1, 0)
    Me.pnlExede.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlExede.Location = New System.Drawing.Point(3, 16)
    Me.pnlExede.Name = "pnlExede"
    Me.pnlExede.RowCount = 5
    Me.pnlExede.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.pnlExede.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.pnlExede.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.pnlExede.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.pnlExede.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
    Me.pnlExede.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlExede.Size = New System.Drawing.Size(422, 109)
    Me.pnlExede.TabIndex = 2
    '
    'pnlExedeUp
    '
    Me.pnlExedeUp.AutoSize = True
    Me.pnlExedeUp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlExedeUp.ColumnCount = 2
    Me.pnlExedeUp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlExedeUp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlExedeUp.Controls.Add(Me.lblExedeUp, 0, 0)
    Me.pnlExedeUp.Controls.Add(Me.lblExedeUpVal, 1, 0)
    Me.pnlExedeUp.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlExedeUp.Location = New System.Drawing.Point(0, 21)
    Me.pnlExedeUp.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlExedeUp.Name = "pnlExedeUp"
    Me.pnlExedeUp.RowCount = 1
    Me.pnlExedeUp.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlExedeUp.Size = New System.Drawing.Size(89, 21)
    Me.pnlExedeUp.TabIndex = 19
    '
    'lblExedeUp
    '
    Me.lblExedeUp.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblExedeUp.AutoSize = True
    Me.lblExedeUp.Location = New System.Drawing.Point(3, 4)
    Me.lblExedeUp.Name = "lblExedeUp"
    Me.lblExedeUp.Size = New System.Drawing.Size(44, 13)
    Me.lblExedeUp.TabIndex = 5
    Me.lblExedeUp.Text = "Upload:"
    '
    'lblExedeUpVal
    '
    Me.lblExedeUpVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblExedeUpVal.AutoSize = True
    Me.lblExedeUpVal.Location = New System.Drawing.Point(67, 4)
    Me.lblExedeUpVal.Name = "lblExedeUpVal"
    Me.lblExedeUpVal.Size = New System.Drawing.Size(19, 13)
    Me.lblExedeUpVal.TabIndex = 10
    Me.lblExedeUpVal.Text = " -- "
    '
    'pnlExedeUsed
    '
    Me.pnlExedeUsed.AutoSize = True
    Me.pnlExedeUsed.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlExedeUsed.ColumnCount = 2
    Me.pnlExedeUsed.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlExedeUsed.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlExedeUsed.Controls.Add(Me.lblExedeTotal, 0, 0)
    Me.pnlExedeUsed.Controls.Add(Me.lblExedeTotalVal, 1, 0)
    Me.pnlExedeUsed.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlExedeUsed.Location = New System.Drawing.Point(0, 42)
    Me.pnlExedeUsed.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlExedeUsed.Name = "pnlExedeUsed"
    Me.pnlExedeUsed.RowCount = 1
    Me.pnlExedeUsed.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlExedeUsed.Size = New System.Drawing.Size(89, 21)
    Me.pnlExedeUsed.TabIndex = 20
    '
    'lblExedeTotal
    '
    Me.lblExedeTotal.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblExedeTotal.AutoSize = True
    Me.lblExedeTotal.Location = New System.Drawing.Point(3, 4)
    Me.lblExedeTotal.Name = "lblExedeTotal"
    Me.lblExedeTotal.Size = New System.Drawing.Size(35, 13)
    Me.lblExedeTotal.TabIndex = 6
    Me.lblExedeTotal.Text = "Used:"
    '
    'lblExedeTotalVal
    '
    Me.lblExedeTotalVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblExedeTotalVal.AutoSize = True
    Me.lblExedeTotalVal.Location = New System.Drawing.Point(67, 4)
    Me.lblExedeTotalVal.Name = "lblExedeTotalVal"
    Me.lblExedeTotalVal.Size = New System.Drawing.Size(19, 13)
    Me.lblExedeTotalVal.TabIndex = 12
    Me.lblExedeTotalVal.Text = " -- "
    '
    'pnlExedeFree
    '
    Me.pnlExedeFree.AutoSize = True
    Me.pnlExedeFree.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlExedeFree.ColumnCount = 2
    Me.pnlExedeFree.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlExedeFree.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlExedeFree.Controls.Add(Me.lblExedeRemain, 0, 0)
    Me.pnlExedeFree.Controls.Add(Me.lblExedeRemainVal, 1, 0)
    Me.pnlExedeFree.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlExedeFree.Location = New System.Drawing.Point(0, 63)
    Me.pnlExedeFree.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlExedeFree.Name = "pnlExedeFree"
    Me.pnlExedeFree.RowCount = 1
    Me.pnlExedeFree.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlExedeFree.Size = New System.Drawing.Size(89, 21)
    Me.pnlExedeFree.TabIndex = 21
    '
    'lblExedeRemain
    '
    Me.lblExedeRemain.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblExedeRemain.AutoSize = True
    Me.lblExedeRemain.Location = New System.Drawing.Point(3, 4)
    Me.lblExedeRemain.Name = "lblExedeRemain"
    Me.lblExedeRemain.Size = New System.Drawing.Size(53, 13)
    Me.lblExedeRemain.TabIndex = 15
    Me.lblExedeRemain.Text = "Available:"
    '
    'lblExedeRemainVal
    '
    Me.lblExedeRemainVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblExedeRemainVal.AutoSize = True
    Me.lblExedeRemainVal.Location = New System.Drawing.Point(67, 4)
    Me.lblExedeRemainVal.Name = "lblExedeRemainVal"
    Me.lblExedeRemainVal.Size = New System.Drawing.Size(19, 13)
    Me.lblExedeRemainVal.TabIndex = 16
    Me.lblExedeRemainVal.Text = " -- "
    '
    'pnlExedeTotal
    '
    Me.pnlExedeTotal.AutoSize = True
    Me.pnlExedeTotal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlExedeTotal.ColumnCount = 2
    Me.pnlExedeTotal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlExedeTotal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlExedeTotal.Controls.Add(Me.lblExedeAllowed, 0, 0)
    Me.pnlExedeTotal.Controls.Add(Me.lblExedeAllowedVal, 1, 0)
    Me.pnlExedeTotal.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlExedeTotal.Location = New System.Drawing.Point(0, 84)
    Me.pnlExedeTotal.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlExedeTotal.Name = "pnlExedeTotal"
    Me.pnlExedeTotal.RowCount = 1
    Me.pnlExedeTotal.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlExedeTotal.Size = New System.Drawing.Size(89, 25)
    Me.pnlExedeTotal.TabIndex = 22
    '
    'lblExedeAllowed
    '
    Me.lblExedeAllowed.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblExedeAllowed.AutoSize = True
    Me.lblExedeAllowed.Location = New System.Drawing.Point(3, 6)
    Me.lblExedeAllowed.Name = "lblExedeAllowed"
    Me.lblExedeAllowed.Size = New System.Drawing.Size(31, 13)
    Me.lblExedeAllowed.TabIndex = 7
    Me.lblExedeAllowed.Text = "Limit:"
    '
    'lblExedeAllowedVal
    '
    Me.lblExedeAllowedVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblExedeAllowedVal.AutoSize = True
    Me.lblExedeAllowedVal.Location = New System.Drawing.Point(67, 6)
    Me.lblExedeAllowedVal.Name = "lblExedeAllowedVal"
    Me.lblExedeAllowedVal.Size = New System.Drawing.Size(19, 13)
    Me.lblExedeAllowedVal.TabIndex = 14
    Me.lblExedeAllowedVal.Text = " -- "
    '
    'pnlExedeDown
    '
    Me.pnlExedeDown.AutoSize = True
    Me.pnlExedeDown.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlExedeDown.ColumnCount = 2
    Me.pnlExedeDown.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlExedeDown.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlExedeDown.Controls.Add(Me.lblExedeDown, 0, 0)
    Me.pnlExedeDown.Controls.Add(Me.lblExedeDownVal, 1, 0)
    Me.pnlExedeDown.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlExedeDown.Location = New System.Drawing.Point(0, 0)
    Me.pnlExedeDown.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlExedeDown.Name = "pnlExedeDown"
    Me.pnlExedeDown.RowCount = 1
    Me.pnlExedeDown.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlExedeDown.Size = New System.Drawing.Size(89, 21)
    Me.pnlExedeDown.TabIndex = 17
    '
    'lblExedeDown
    '
    Me.lblExedeDown.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblExedeDown.AutoSize = True
    Me.lblExedeDown.Location = New System.Drawing.Point(3, 4)
    Me.lblExedeDown.Name = "lblExedeDown"
    Me.lblExedeDown.Size = New System.Drawing.Size(58, 13)
    Me.lblExedeDown.TabIndex = 4
    Me.lblExedeDown.Text = "Download:"
    '
    'lblExedeDownVal
    '
    Me.lblExedeDownVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblExedeDownVal.AutoSize = True
    Me.lblExedeDownVal.Location = New System.Drawing.Point(67, 4)
    Me.lblExedeDownVal.Name = "lblExedeDownVal"
    Me.lblExedeDownVal.Size = New System.Drawing.Size(19, 13)
    Me.lblExedeDownVal.TabIndex = 8
    Me.lblExedeDownVal.Text = " -- "
    '
    'pctExede
    '
    Me.pctExede.BackColor = System.Drawing.Color.White
    Me.pctExede.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.pctExede.ContextMenu = Me.mnuGraph
    Me.pctExede.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pctExede.Location = New System.Drawing.Point(89, 0)
    Me.pctExede.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
    Me.pctExede.Name = "pctExede"
    Me.pnlExede.SetRowSpan(Me.pctExede, 5)
    Me.pctExede.Size = New System.Drawing.Size(330, 109)
    Me.pctExede.TabIndex = 1
    Me.pctExede.TabStop = False
    Me.ttUI.SetTooltip(Me.pctExede, "Graph representing your usage.")
    '
    'pnlRural
    '
    Me.pnlRural.ColumnCount = 2
    Me.pnlRural.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlRural.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlRural.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlRural.Controls.Add(Me.pnlRuralUsed, 0, 0)
    Me.pnlRural.Controls.Add(Me.pnlRuralFree, 0, 1)
    Me.pnlRural.Controls.Add(Me.pnlRuralTotal, 0, 2)
    Me.pnlRural.Controls.Add(Me.pctRural, 1, 0)
    Me.pnlRural.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlRural.Location = New System.Drawing.Point(3, 16)
    Me.pnlRural.Name = "pnlRural"
    Me.pnlRural.RowCount = 3
    Me.pnlRural.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlRural.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334!))
    Me.pnlRural.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlRural.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlRural.Size = New System.Drawing.Size(422, 109)
    Me.pnlRural.TabIndex = 3
    '
    'pnlRuralUsed
    '
    Me.pnlRuralUsed.AutoSize = True
    Me.pnlRuralUsed.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlRuralUsed.ColumnCount = 2
    Me.pnlRuralUsed.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlRuralUsed.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlRuralUsed.Controls.Add(Me.lblRuralUsed, 0, 0)
    Me.pnlRuralUsed.Controls.Add(Me.lblRuralUsedVal, 1, 0)
    Me.pnlRuralUsed.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlRuralUsed.Location = New System.Drawing.Point(0, 0)
    Me.pnlRuralUsed.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlRuralUsed.Name = "pnlRuralUsed"
    Me.pnlRuralUsed.RowCount = 1
    Me.pnlRuralUsed.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlRuralUsed.Size = New System.Drawing.Size(84, 36)
    Me.pnlRuralUsed.TabIndex = 18
    '
    'lblRuralUsed
    '
    Me.lblRuralUsed.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblRuralUsed.AutoSize = True
    Me.lblRuralUsed.Location = New System.Drawing.Point(3, 11)
    Me.lblRuralUsed.Name = "lblRuralUsed"
    Me.lblRuralUsed.Size = New System.Drawing.Size(35, 13)
    Me.lblRuralUsed.TabIndex = 6
    Me.lblRuralUsed.Text = "Used:"
    '
    'lblRuralUsedVal
    '
    Me.lblRuralUsedVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblRuralUsedVal.AutoSize = True
    Me.lblRuralUsedVal.Location = New System.Drawing.Point(62, 11)
    Me.lblRuralUsedVal.Name = "lblRuralUsedVal"
    Me.lblRuralUsedVal.Size = New System.Drawing.Size(19, 13)
    Me.lblRuralUsedVal.TabIndex = 12
    Me.lblRuralUsedVal.Text = " -- "
    '
    'pnlRuralFree
    '
    Me.pnlRuralFree.AutoSize = True
    Me.pnlRuralFree.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlRuralFree.ColumnCount = 2
    Me.pnlRuralFree.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlRuralFree.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlRuralFree.Controls.Add(Me.lblRuralRemain, 0, 0)
    Me.pnlRuralFree.Controls.Add(Me.lblRuralRemainVal, 1, 0)
    Me.pnlRuralFree.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlRuralFree.Location = New System.Drawing.Point(0, 36)
    Me.pnlRuralFree.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlRuralFree.Name = "pnlRuralFree"
    Me.pnlRuralFree.RowCount = 1
    Me.pnlRuralFree.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlRuralFree.Size = New System.Drawing.Size(84, 36)
    Me.pnlRuralFree.TabIndex = 19
    '
    'lblRuralRemain
    '
    Me.lblRuralRemain.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblRuralRemain.AutoSize = True
    Me.lblRuralRemain.Location = New System.Drawing.Point(3, 11)
    Me.lblRuralRemain.Name = "lblRuralRemain"
    Me.lblRuralRemain.Size = New System.Drawing.Size(53, 13)
    Me.lblRuralRemain.TabIndex = 15
    Me.lblRuralRemain.Text = "Available:"
    '
    'lblRuralRemainVal
    '
    Me.lblRuralRemainVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblRuralRemainVal.AutoSize = True
    Me.lblRuralRemainVal.Location = New System.Drawing.Point(62, 11)
    Me.lblRuralRemainVal.Name = "lblRuralRemainVal"
    Me.lblRuralRemainVal.Size = New System.Drawing.Size(19, 13)
    Me.lblRuralRemainVal.TabIndex = 16
    Me.lblRuralRemainVal.Text = " -- "
    '
    'pnlRuralTotal
    '
    Me.pnlRuralTotal.AutoSize = True
    Me.pnlRuralTotal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlRuralTotal.ColumnCount = 2
    Me.pnlRuralTotal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlRuralTotal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlRuralTotal.Controls.Add(Me.lblRuralAllowed, 0, 0)
    Me.pnlRuralTotal.Controls.Add(Me.lblRuralAllowedVal, 1, 0)
    Me.pnlRuralTotal.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlRuralTotal.Location = New System.Drawing.Point(0, 72)
    Me.pnlRuralTotal.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlRuralTotal.Name = "pnlRuralTotal"
    Me.pnlRuralTotal.RowCount = 1
    Me.pnlRuralTotal.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlRuralTotal.Size = New System.Drawing.Size(84, 37)
    Me.pnlRuralTotal.TabIndex = 20
    '
    'lblRuralAllowed
    '
    Me.lblRuralAllowed.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblRuralAllowed.AutoSize = True
    Me.lblRuralAllowed.Location = New System.Drawing.Point(3, 12)
    Me.lblRuralAllowed.Name = "lblRuralAllowed"
    Me.lblRuralAllowed.Size = New System.Drawing.Size(31, 13)
    Me.lblRuralAllowed.TabIndex = 7
    Me.lblRuralAllowed.Text = "Limit:"
    '
    'lblRuralAllowedVal
    '
    Me.lblRuralAllowedVal.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblRuralAllowedVal.AutoSize = True
    Me.lblRuralAllowedVal.Location = New System.Drawing.Point(62, 12)
    Me.lblRuralAllowedVal.Name = "lblRuralAllowedVal"
    Me.lblRuralAllowedVal.Size = New System.Drawing.Size(19, 13)
    Me.lblRuralAllowedVal.TabIndex = 14
    Me.lblRuralAllowedVal.Text = " -- "
    '
    'pctRural
    '
    Me.pctRural.BackColor = System.Drawing.Color.White
    Me.pctRural.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.pctRural.ContextMenu = Me.mnuGraph
    Me.pctRural.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pctRural.Location = New System.Drawing.Point(84, 0)
    Me.pctRural.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
    Me.pctRural.Name = "pctRural"
    Me.pnlRural.SetRowSpan(Me.pctRural, 3)
    Me.pctRural.Size = New System.Drawing.Size(335, 109)
    Me.pctRural.TabIndex = 1
    Me.pctRural.TabStop = False
    Me.ttUI.SetTooltip(Me.pctRural, "Graph representing your usage.")
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
    Me.pnlWildBlue.ResumeLayout(False)
    Me.gbUld.ResumeLayout(False)
    Me.pnlUld.ResumeLayout(False)
    Me.pnlUld.PerformLayout()
    CType(Me.pctUld, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlUldText.ResumeLayout(False)
    Me.pnlUldText.PerformLayout()
    Me.pnlUldTextUsed.ResumeLayout(False)
    Me.pnlUldTextUsed.PerformLayout()
    Me.pnlUldTextFree.ResumeLayout(False)
    Me.pnlUldTextFree.PerformLayout()
    Me.pnlUldTextTotal.ResumeLayout(False)
    Me.pnlUldTextTotal.PerformLayout()
    Me.gbDld.ResumeLayout(False)
    Me.pnlDld.ResumeLayout(False)
    Me.pnlDld.PerformLayout()
    CType(Me.pctDld, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlDldText.ResumeLayout(False)
    Me.pnlDldText.PerformLayout()
    Me.pnlDldTextUsed.ResumeLayout(False)
    Me.pnlDldTextUsed.PerformLayout()
    Me.pnlDldTextFree.ResumeLayout(False)
    Me.pnlDldTextFree.PerformLayout()
    Me.pnlDldTextTotal.ResumeLayout(False)
    Me.pnlDldTextTotal.PerformLayout()
    Me.pnlExede.ResumeLayout(False)
    Me.pnlExede.PerformLayout()
    Me.pnlExedeUp.ResumeLayout(False)
    Me.pnlExedeUp.PerformLayout()
    Me.pnlExedeUsed.ResumeLayout(False)
    Me.pnlExedeUsed.PerformLayout()
    Me.pnlExedeFree.ResumeLayout(False)
    Me.pnlExedeFree.PerformLayout()
    Me.pnlExedeTotal.ResumeLayout(False)
    Me.pnlExedeTotal.PerformLayout()
    Me.pnlExedeDown.ResumeLayout(False)
    Me.pnlExedeDown.PerformLayout()
    CType(Me.pctExede, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlRural.ResumeLayout(False)
    Me.pnlRural.PerformLayout()
    Me.pnlRuralUsed.ResumeLayout(False)
    Me.pnlRuralUsed.PerformLayout()
    Me.pnlRuralFree.ResumeLayout(False)
    Me.pnlRuralFree.PerformLayout()
    Me.pnlRuralTotal.ResumeLayout(False)
    Me.pnlRuralTotal.PerformLayout()
    CType(Me.pctRural, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents pnlDetails As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlSettings As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdHistory As System.Windows.Forms.Button
  Friend WithEvents gbUsage As System.Windows.Forms.GroupBox
  Friend WithEvents gbUld As System.Windows.Forms.GroupBox
  Friend WithEvents gbDld As System.Windows.Forms.GroupBox
  Friend WithEvents pnlDld As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctDld As System.Windows.Forms.PictureBox
  Friend WithEvents pnlDldText As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblDUsed As System.Windows.Forms.Label
  Friend WithEvents lblDldUsed As System.Windows.Forms.Label
  Friend WithEvents lblDFree As System.Windows.Forms.Label
  Friend WithEvents lblDldFree As System.Windows.Forms.Label
  Friend WithEvents lblDTotal As System.Windows.Forms.Label
  Friend WithEvents lblDldTotal As System.Windows.Forms.Label
  Friend WithEvents pnlUld As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctUld As System.Windows.Forms.PictureBox
  Friend WithEvents pnlUldText As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblUUsed As System.Windows.Forms.Label
  Friend WithEvents lblUldUsed As System.Windows.Forms.Label
  Friend WithEvents lblUFree As System.Windows.Forms.Label
  Friend WithEvents lblUldFree As System.Windows.Forms.Label
  Friend WithEvents lblUTotal As System.Windows.Forms.Label
  Friend WithEvents lblUldTotal As System.Windows.Forms.Label
  Friend WithEvents trayIcon As System.Windows.Forms.NotifyIcon
  Friend WithEvents ttUI As ToolTip
  Friend WithEvents pnlWildBlue As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents tmrUpdate As System.Windows.Forms.Timer
  Friend WithEvents cmdAbout As System.Windows.Forms.Button
  Friend WithEvents cmdRefresh As System.Windows.Forms.Button
  Friend WithEvents cmdConfig As System.Windows.Forms.Button
  Friend WithEvents pnlExede As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblExedeTotal As System.Windows.Forms.Label
  Friend WithEvents lblExedeUp As System.Windows.Forms.Label
  Friend WithEvents lblExedeDown As System.Windows.Forms.Label
  Friend WithEvents lblExedeAllowed As System.Windows.Forms.Label
  Friend WithEvents lblExedeDownVal As System.Windows.Forms.Label
  Friend WithEvents lblExedeUpVal As System.Windows.Forms.Label
  Friend WithEvents lblExedeTotalVal As System.Windows.Forms.Label
  Friend WithEvents lblExedeAllowedVal As System.Windows.Forms.Label
  Friend WithEvents pctExede As System.Windows.Forms.PictureBox
  Friend WithEvents lblExedeRemainVal As System.Windows.Forms.Label
  Friend WithEvents lblExedeRemain As System.Windows.Forms.Label
  Friend WithEvents pnlRural As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctRural As System.Windows.Forms.PictureBox
  Friend WithEvents lblRuralUsed As System.Windows.Forms.Label
  Friend WithEvents lblRuralUsedVal As System.Windows.Forms.Label
  Friend WithEvents lblRuralRemain As System.Windows.Forms.Label
  Friend WithEvents lblRuralRemainVal As System.Windows.Forms.Label
  Friend WithEvents lblRuralAllowed As System.Windows.Forms.Label
  Friend WithEvents lblRuralAllowedVal As System.Windows.Forms.Label
  Friend WithEvents pnlDldTextUsed As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlDldTextFree As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlDldTextTotal As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlUldTextUsed As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlUldTextFree As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlUldTextTotal As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlExedeDown As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlExedeUp As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlExedeUsed As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlExedeFree As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlExedeTotal As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlRuralUsed As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlRuralFree As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlRuralTotal As System.Windows.Forms.TableLayoutPanel
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
