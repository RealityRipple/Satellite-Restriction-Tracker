<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConfig
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfig))
    Me.pnlConfig = New System.Windows.Forms.TableLayoutPanel()
    Me.chkOverAlert = New System.Windows.Forms.CheckBox()
    Me.cmdColors = New System.Windows.Forms.Button()
    Me.lblKey = New System.Windows.Forms.Label()
    Me.pnlPassword = New System.Windows.Forms.TableLayoutPanel()
    Me.txtPassword = New System.Windows.Forms.TextBox()
    Me.cmdPassDisplay = New System.Windows.Forms.Button()
    Me.lblPassword = New System.Windows.Forms.Label()
    Me.lblAccount = New System.Windows.Forms.Label()
    Me.txtAccount = New System.Windows.Forms.TextBox()
    Me.chkStartUp = New System.Windows.Forms.CheckBox()
    Me.lblTimeout = New System.Windows.Forms.Label()
    Me.pnlTimeout = New System.Windows.Forms.TableLayoutPanel()
    Me.txtTimeout = New RestrictionTracker.NumericUpDownIncrementable()
    Me.lblTimeoutScale = New System.Windows.Forms.Label()
    Me.pnlInterval = New System.Windows.Forms.TableLayoutPanel()
    Me.txtInterval = New RestrictionTracker.NumericUpDownIncrementable()
    Me.lblIntervalScale = New System.Windows.Forms.Label()
    Me.lblAccuracy = New System.Windows.Forms.Label()
    Me.pnlAccuracy = New System.Windows.Forms.TableLayoutPanel()
    Me.txtAccuracy = New RestrictionTracker.NumericUpDownIncrementable()
    Me.lblAccuracyScale = New System.Windows.Forms.Label()
    Me.chkService = New System.Windows.Forms.CheckBox()
    Me.lblHistoryDir = New System.Windows.Forms.Label()
    Me.pnlHistoryDir = New System.Windows.Forms.TableLayoutPanel()
    Me.txtHistoryDir = New System.Windows.Forms.TextBox()
    Me.cmdHistoryDir = New System.Windows.Forms.Button()
    Me.pnlButtons = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdSave = New System.Windows.Forms.Button()
    Me.cmdClose = New System.Windows.Forms.Button()
    Me.pctSRT = New System.Windows.Forms.PictureBox()
    Me.gbProxy = New System.Windows.Forms.GroupBox()
    Me.pnlProxy = New System.Windows.Forms.TableLayoutPanel()
    Me.lblProxyType = New System.Windows.Forms.Label()
    Me.txtProxyUser = New System.Windows.Forms.TextBox()
    Me.lblProxyUser = New System.Windows.Forms.Label()
    Me.txtProxyPassword = New System.Windows.Forms.TextBox()
    Me.lblProxyPassword = New System.Windows.Forms.Label()
    Me.lblProxyAddr = New System.Windows.Forms.Label()
    Me.txtProxyAddress = New System.Windows.Forms.TextBox()
    Me.lblProxyPort = New System.Windows.Forms.Label()
    Me.txtProxyPort = New RestrictionTracker.NumericUpDownIncrementable()
    Me.lblProxyDomain = New System.Windows.Forms.Label()
    Me.txtProxyDomain = New System.Windows.Forms.TextBox()
    Me.cmbProxyType = New System.Windows.Forms.ComboBox()
    Me.pnlOverAlert = New System.Windows.Forms.TableLayoutPanel()
    Me.txtOverSize = New NumericUpDownIncrementable()
    Me.lblOverSize = New System.Windows.Forms.Label()
    Me.txtOverTime = New NumericUpDownIncrementable()
    Me.lblOverTime = New System.Windows.Forms.Label()
    Me.lblProvider = New System.Windows.Forms.Label()
    Me.cmbProvider = New System.Windows.Forms.ComboBox()
    Me.lblInterval = New System.Windows.Forms.Label()
    Me.pnlKey = New System.Windows.Forms.TableLayoutPanel()
    Me.pctKeyState = New System.Windows.Forms.PictureBox()
    Me.txtKey1 = New System.Windows.Forms.TextBox()
    Me.txtKey2 = New System.Windows.Forms.TextBox()
    Me.txtKey3 = New System.Windows.Forms.TextBox()
    Me.txtKey4 = New System.Windows.Forms.TextBox()
    Me.txtKey5 = New System.Windows.Forms.TextBox()
    Me.lblPurchaseKey = New RestrictionTracker.LinkLabel()
    Me.cmdAlertStyle = New System.Windows.Forms.Button()
    Me.chkBeta = New System.Windows.Forms.CheckBox()
    Me.fswController = New System.IO.FileSystemWatcher()
    Me.tmrAnim = New System.Windows.Forms.Timer(Me.components)
    Me.ttConfig = New RestrictionTracker.ToolTip(Me.components)
    Me.pnlConfig.SuspendLayout()
    Me.pnlPassword.SuspendLayout()
    Me.pnlTimeout.SuspendLayout()
    CType(Me.txtTimeout, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlInterval.SuspendLayout()
    CType(Me.txtInterval, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlAccuracy.SuspendLayout()
    CType(Me.txtAccuracy, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlHistoryDir.SuspendLayout()
    Me.pnlButtons.SuspendLayout()
    CType(Me.pctSRT, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.gbProxy.SuspendLayout()
    Me.pnlProxy.SuspendLayout()
    CType(Me.txtProxyPort, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlOverAlert.SuspendLayout()
    CType(Me.txtOverSize, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtOverTime, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlKey.SuspendLayout()
    CType(Me.pctKeyState, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.fswController, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'pnlConfig
    '
    Me.pnlConfig.AutoSize = True
    Me.pnlConfig.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlConfig.ColumnCount = 3
    Me.pnlConfig.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlConfig.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlConfig.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlConfig.Controls.Add(Me.chkOverAlert, 0, 11)
    Me.pnlConfig.Controls.Add(Me.cmdColors, 2, 10)
    Me.pnlConfig.Controls.Add(Me.lblKey, 0, 2)
    Me.pnlConfig.Controls.Add(Me.pnlPassword, 1, 3)
    Me.pnlConfig.Controls.Add(Me.lblPassword, 0, 3)
    Me.pnlConfig.Controls.Add(Me.lblAccount, 0, 0)
    Me.pnlConfig.Controls.Add(Me.txtAccount, 1, 0)
    Me.pnlConfig.Controls.Add(Me.chkStartUp, 0, 8)
    Me.pnlConfig.Controls.Add(Me.lblTimeout, 0, 5)
    Me.pnlConfig.Controls.Add(Me.pnlTimeout, 1, 5)
    Me.pnlConfig.Controls.Add(Me.pnlInterval, 1, 4)
    Me.pnlConfig.Controls.Add(Me.lblAccuracy, 0, 6)
    Me.pnlConfig.Controls.Add(Me.pnlAccuracy, 1, 6)
    Me.pnlConfig.Controls.Add(Me.chkService, 0, 9)
    Me.pnlConfig.Controls.Add(Me.lblHistoryDir, 0, 7)
    Me.pnlConfig.Controls.Add(Me.pnlHistoryDir, 1, 7)
    Me.pnlConfig.Controls.Add(Me.pnlButtons, 2, 12)
    Me.pnlConfig.Controls.Add(Me.pctSRT, 2, 0)
    Me.pnlConfig.Controls.Add(Me.gbProxy, 2, 3)
    Me.pnlConfig.Controls.Add(Me.pnlOverAlert, 1, 11)
    Me.pnlConfig.Controls.Add(Me.lblProvider, 0, 1)
    Me.pnlConfig.Controls.Add(Me.cmbProvider, 1, 1)
    Me.pnlConfig.Controls.Add(Me.lblInterval, 0, 4)
    Me.pnlConfig.Controls.Add(Me.pnlKey, 1, 2)
    Me.pnlConfig.Controls.Add(Me.lblPurchaseKey, 2, 2)
    Me.pnlConfig.Controls.Add(Me.cmdAlertStyle, 0, 12)
    Me.pnlConfig.Controls.Add(Me.chkBeta, 0, 10)
    Me.pnlConfig.Location = New System.Drawing.Point(0, 0)
    Me.pnlConfig.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlConfig.Name = "pnlConfig"
    Me.pnlConfig.RowCount = 13
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlConfig.Size = New System.Drawing.Size(610, 342)
    Me.pnlConfig.TabIndex = 0
    '
    'chkOverAlert
    '
    Me.chkOverAlert.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkOverAlert.AutoSize = True
    Me.chkOverAlert.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkOverAlert.Location = New System.Drawing.Point(3, 290)
    Me.chkOverAlert.Name = "chkOverAlert"
    Me.chkOverAlert.Size = New System.Drawing.Size(90, 18)
    Me.chkOverAlert.TabIndex = 20
    Me.chkOverAlert.Text = "&Usage Alert:"
    Me.ttConfig.SetTooltip(Me.chkOverAlert, "Check this box to display a balloon alert when you use too much of your allocated" & _
        " usage.")
    Me.chkOverAlert.UseVisualStyleBackColor = True
    '
    'cmdColors
    '
    Me.cmdColors.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdColors.AutoSize = True
    Me.cmdColors.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdColors.Location = New System.Drawing.Point(403, 275)
    Me.cmdColors.Name = "cmdColors"
    Me.pnlConfig.SetRowSpan(Me.cmdColors, 2)
    Me.cmdColors.Size = New System.Drawing.Size(133, 24)
    Me.cmdColors.TabIndex = 24
    Me.cmdColors.Text = "Customize &Graph Colors"
    Me.ttConfig.SetTooltip(Me.cmdColors, "Change the colors of the Main Window, Tray Icon, and History Graphs.")
    Me.cmdColors.UseVisualStyleBackColor = True
    '
    'lblKey
    '
    Me.lblKey.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblKey.AutoSize = True
    Me.lblKey.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblKey.Location = New System.Drawing.Point(3, 61)
    Me.lblKey.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblKey.Name = "lblKey"
    Me.lblKey.Size = New System.Drawing.Size(68, 13)
    Me.lblKey.TabIndex = 4
    Me.lblKey.Text = "Product &Key:"
    '
    'pnlPassword
    '
    Me.pnlPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlPassword.AutoSize = True
    Me.pnlPassword.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlPassword.ColumnCount = 2
    Me.pnlPassword.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPassword.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPassword.Controls.Add(Me.txtPassword, 0, 0)
    Me.pnlPassword.Controls.Add(Me.cmdPassDisplay, 1, 0)
    Me.pnlPassword.Location = New System.Drawing.Point(96, 80)
    Me.pnlPassword.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlPassword.Name = "pnlPassword"
    Me.pnlPassword.RowCount = 1
    Me.pnlPassword.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPassword.Size = New System.Drawing.Size(233, 28)
    Me.pnlPassword.TabIndex = 8
    '
    'txtPassword
    '
    Me.txtPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtPassword.Location = New System.Drawing.Point(3, 4)
    Me.txtPassword.Margin = New System.Windows.Forms.Padding(3, 3, 1, 3)
    Me.txtPassword.Name = "txtPassword"
    Me.txtPassword.Size = New System.Drawing.Size(200, 20)
    Me.txtPassword.TabIndex = 0
    Me.ttConfig.SetTooltip(Me.txtPassword, "The password to your ViaSat account.")
    Me.txtPassword.UseSystemPasswordChar = True
    '
    'cmdPassDisplay
    '
    Me.cmdPassDisplay.AutoSize = True
    Me.cmdPassDisplay.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdPassDisplay.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdPassDisplay.Location = New System.Drawing.Point(205, 3)
    Me.cmdPassDisplay.Margin = New System.Windows.Forms.Padding(1, 3, 3, 3)
    Me.cmdPassDisplay.Name = "cmdPassDisplay"
    Me.cmdPassDisplay.Size = New System.Drawing.Size(25, 22)
    Me.cmdPassDisplay.TabIndex = 1
    Me.cmdPassDisplay.Text = "*"
    Me.ttConfig.SetTooltip(Me.cmdPassDisplay, "Toggle display of the password.")
    Me.cmdPassDisplay.UseVisualStyleBackColor = True
    '
    'lblPassword
    '
    Me.lblPassword.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblPassword.AutoSize = True
    Me.lblPassword.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblPassword.Location = New System.Drawing.Point(3, 87)
    Me.lblPassword.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblPassword.Name = "lblPassword"
    Me.lblPassword.Size = New System.Drawing.Size(56, 13)
    Me.lblPassword.TabIndex = 7
    Me.lblPassword.Text = "&Password:"
    '
    'lblAccount
    '
    Me.lblAccount.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAccount.AutoSize = True
    Me.lblAccount.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblAccount.Location = New System.Drawing.Point(3, 6)
    Me.lblAccount.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblAccount.Name = "lblAccount"
    Me.lblAccount.Size = New System.Drawing.Size(58, 13)
    Me.lblAccount.TabIndex = 0
    Me.lblAccount.Text = "User&name:"
    '
    'txtAccount
    '
    Me.txtAccount.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtAccount.Location = New System.Drawing.Point(99, 3)
    Me.txtAccount.Name = "txtAccount"
    Me.txtAccount.Size = New System.Drawing.Size(227, 20)
    Me.txtAccount.TabIndex = 1
    Me.ttConfig.SetTooltip(Me.txtAccount, "Your ViaSat Username. If you were provided with an E-Mail address, this is the fi" & _
        "rst half of that address.")
    '
    'chkStartUp
    '
    Me.chkStartUp.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkStartUp.AutoSize = True
    Me.pnlConfig.SetColumnSpan(Me.chkStartUp, 2)
    Me.chkStartUp.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkStartUp.Location = New System.Drawing.Point(3, 217)
    Me.chkStartUp.Name = "chkStartUp"
    Me.chkStartUp.Size = New System.Drawing.Size(270, 18)
    Me.chkStartUp.TabIndex = 17
    Me.chkStartUp.Text = "&Run Satellite Restriction Tracker on system startup"
    Me.ttConfig.SetTooltip(Me.chkStartUp, "Start Satellite Restriction Tracker with this Windows account.")
    Me.chkStartUp.UseVisualStyleBackColor = True
    '
    'lblTimeout
    '
    Me.lblTimeout.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTimeout.AutoSize = True
    Me.lblTimeout.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblTimeout.Location = New System.Drawing.Point(3, 140)
    Me.lblTimeout.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblTimeout.Name = "lblTimeout"
    Me.lblTimeout.Size = New System.Drawing.Size(91, 13)
    Me.lblTimeout.TabIndex = 11
    Me.lblTimeout.Text = "Network &Timeout:"
    '
    'pnlTimeout
    '
    Me.pnlTimeout.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.pnlTimeout.AutoSize = True
    Me.pnlTimeout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlTimeout.ColumnCount = 2
    Me.pnlTimeout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75.0!))
    Me.pnlTimeout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlTimeout.Controls.Add(Me.txtTimeout, 0, 0)
    Me.pnlTimeout.Controls.Add(Me.lblTimeoutScale, 1, 0)
    Me.pnlTimeout.Location = New System.Drawing.Point(96, 134)
    Me.pnlTimeout.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlTimeout.Name = "pnlTimeout"
    Me.pnlTimeout.RowCount = 1
    Me.pnlTimeout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlTimeout.Size = New System.Drawing.Size(128, 26)
    Me.pnlTimeout.TabIndex = 12
    '
    'txtTimeout
    '
    Me.txtTimeout.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtTimeout.LargeIncrement = CType(15UI, UInteger)
    Me.txtTimeout.Location = New System.Drawing.Point(3, 3)
    Me.txtTimeout.Maximum = New Decimal(New Integer() {600, 0, 0, 0})
    Me.txtTimeout.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
    Me.txtTimeout.Name = "txtTimeout"
    Me.txtTimeout.Size = New System.Drawing.Size(69, 20)
    Me.txtTimeout.TabIndex = 0
    Me.txtTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    Me.ttConfig.SetTooltip(Me.txtTimeout, "Number of seconds to wait between network communications.")
    Me.txtTimeout.Value = New Decimal(New Integer() {60, 0, 0, 0})
    '
    'lblTimeoutScale
    '
    Me.lblTimeoutScale.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTimeoutScale.AutoSize = True
    Me.lblTimeoutScale.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblTimeoutScale.Location = New System.Drawing.Point(78, 6)
    Me.lblTimeoutScale.Name = "lblTimeoutScale"
    Me.lblTimeoutScale.Size = New System.Drawing.Size(47, 13)
    Me.lblTimeoutScale.TabIndex = 1
    Me.lblTimeoutScale.Text = "seconds"
    '
    'pnlInterval
    '
    Me.pnlInterval.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.pnlInterval.AutoSize = True
    Me.pnlInterval.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlInterval.ColumnCount = 2
    Me.pnlInterval.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75.0!))
    Me.pnlInterval.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlInterval.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlInterval.Controls.Add(Me.txtInterval, 0, 0)
    Me.pnlInterval.Controls.Add(Me.lblIntervalScale, 1, 0)
    Me.pnlInterval.Location = New System.Drawing.Point(96, 108)
    Me.pnlInterval.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlInterval.Name = "pnlInterval"
    Me.pnlInterval.RowCount = 1
    Me.pnlInterval.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlInterval.Size = New System.Drawing.Size(124, 26)
    Me.pnlInterval.TabIndex = 10
    '
    'txtInterval
    '
    Me.txtInterval.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtInterval.LargeIncrement = CType(5UI, UInteger)
    Me.txtInterval.Location = New System.Drawing.Point(3, 3)
    Me.txtInterval.Maximum = New Decimal(New Integer() {1440, 0, 0, 0})
    Me.txtInterval.Minimum = New Decimal(New Integer() {15, 0, 0, 0})
    Me.txtInterval.Name = "txtInterval"
    Me.txtInterval.Size = New System.Drawing.Size(69, 20)
    Me.txtInterval.TabIndex = 0
    Me.txtInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    Me.ttConfig.SetTooltip(Me.txtInterval, "Interval between meter checks in minutes.")
    Me.txtInterval.Value = New Decimal(New Integer() {15, 0, 0, 0})
    '
    'lblIntervalScale
    '
    Me.lblIntervalScale.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblIntervalScale.AutoSize = True
    Me.lblIntervalScale.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblIntervalScale.Location = New System.Drawing.Point(78, 6)
    Me.lblIntervalScale.Name = "lblIntervalScale"
    Me.lblIntervalScale.Size = New System.Drawing.Size(43, 13)
    Me.lblIntervalScale.TabIndex = 1
    Me.lblIntervalScale.Text = "minutes"
    '
    'lblAccuracy
    '
    Me.lblAccuracy.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAccuracy.AutoSize = True
    Me.lblAccuracy.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblAccuracy.Location = New System.Drawing.Point(3, 166)
    Me.lblAccuracy.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblAccuracy.Name = "lblAccuracy"
    Me.lblAccuracy.Size = New System.Drawing.Size(89, 13)
    Me.lblAccuracy.TabIndex = 13
    Me.lblAccuracy.Text = "Usage &Accuracy:"
    '
    'pnlAccuracy
    '
    Me.pnlAccuracy.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.pnlAccuracy.AutoSize = True
    Me.pnlAccuracy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlAccuracy.ColumnCount = 2
    Me.pnlAccuracy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75.0!))
    Me.pnlAccuracy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAccuracy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlAccuracy.Controls.Add(Me.txtAccuracy, 0, 0)
    Me.pnlAccuracy.Controls.Add(Me.lblAccuracyScale, 1, 0)
    Me.pnlAccuracy.Location = New System.Drawing.Point(96, 160)
    Me.pnlAccuracy.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlAccuracy.Name = "pnlAccuracy"
    Me.pnlAccuracy.RowCount = 1
    Me.pnlAccuracy.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAccuracy.Size = New System.Drawing.Size(158, 26)
    Me.pnlAccuracy.TabIndex = 14
    '
    'txtAccuracy
    '
    Me.txtAccuracy.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtAccuracy.LargeIncrement = CType(1UI, UInteger)
    Me.txtAccuracy.Location = New System.Drawing.Point(3, 3)
    Me.txtAccuracy.Maximum = New Decimal(New Integer() {4, 0, 0, 0})
    Me.txtAccuracy.Name = "txtAccuracy"
    Me.txtAccuracy.Size = New System.Drawing.Size(69, 20)
    Me.txtAccuracy.TabIndex = 0
    Me.txtAccuracy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    Me.ttConfig.SetTooltip(Me.txtAccuracy, "Number of decimal places to display.")
    '
    'lblAccuracyScale
    '
    Me.lblAccuracyScale.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAccuracyScale.AutoSize = True
    Me.lblAccuracyScale.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblAccuracyScale.Location = New System.Drawing.Point(78, 6)
    Me.lblAccuracyScale.Name = "lblAccuracyScale"
    Me.lblAccuracyScale.Size = New System.Drawing.Size(77, 13)
    Me.lblAccuracyScale.TabIndex = 1
    Me.lblAccuracyScale.Text = "decimal places"
    '
    'chkService
    '
    Me.chkService.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkService.AutoSize = True
    Me.pnlConfig.SetColumnSpan(Me.chkService, 2)
    Me.chkService.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkService.Location = New System.Drawing.Point(3, 241)
    Me.chkService.Name = "chkService"
    Me.chkService.Size = New System.Drawing.Size(240, 18)
    Me.chkService.TabIndex = 18
    Me.chkService.Text = "Run &Logger Service when Tracker is closed"
    Me.ttConfig.SetTooltip(Me.chkService, resources.GetString("chkService.ToolTip"))
    Me.chkService.UseVisualStyleBackColor = True
    '
    'lblHistoryDir
    '
    Me.lblHistoryDir.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblHistoryDir.AutoSize = True
    Me.lblHistoryDir.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblHistoryDir.Location = New System.Drawing.Point(3, 193)
    Me.lblHistoryDir.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblHistoryDir.Name = "lblHistoryDir"
    Me.lblHistoryDir.Size = New System.Drawing.Size(78, 13)
    Me.lblHistoryDir.TabIndex = 15
    Me.lblHistoryDir.Text = "&Data Directory:"
    '
    'pnlHistoryDir
    '
    Me.pnlHistoryDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlHistoryDir.AutoSize = True
    Me.pnlHistoryDir.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlHistoryDir.ColumnCount = 2
    Me.pnlHistoryDir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlHistoryDir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlHistoryDir.Controls.Add(Me.txtHistoryDir, 0, 0)
    Me.pnlHistoryDir.Controls.Add(Me.cmdHistoryDir, 1, 0)
    Me.pnlHistoryDir.Location = New System.Drawing.Point(96, 186)
    Me.pnlHistoryDir.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlHistoryDir.Name = "pnlHistoryDir"
    Me.pnlHistoryDir.RowCount = 1
    Me.pnlHistoryDir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlHistoryDir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
    Me.pnlHistoryDir.Size = New System.Drawing.Size(233, 28)
    Me.pnlHistoryDir.TabIndex = 16
    '
    'txtHistoryDir
    '
    Me.txtHistoryDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtHistoryDir.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
    Me.txtHistoryDir.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories
    Me.txtHistoryDir.Location = New System.Drawing.Point(3, 4)
    Me.txtHistoryDir.Margin = New System.Windows.Forms.Padding(3, 3, 1, 3)
    Me.txtHistoryDir.Name = "txtHistoryDir"
    Me.txtHistoryDir.Size = New System.Drawing.Size(189, 20)
    Me.txtHistoryDir.TabIndex = 0
    Me.ttConfig.SetTooltip(Me.txtHistoryDir, "Directory used to save History Data.")
    '
    'cmdHistoryDir
    '
    Me.cmdHistoryDir.AutoSize = True
    Me.cmdHistoryDir.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdHistoryDir.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdHistoryDir.Location = New System.Drawing.Point(194, 3)
    Me.cmdHistoryDir.Margin = New System.Windows.Forms.Padding(1, 3, 3, 3)
    Me.cmdHistoryDir.Name = "cmdHistoryDir"
    Me.cmdHistoryDir.Size = New System.Drawing.Size(36, 22)
    Me.cmdHistoryDir.TabIndex = 1
    Me.cmdHistoryDir.Text = ". . ."
    Me.ttConfig.SetTooltip(Me.cmdHistoryDir, "Browse for a Directory to save History Data to.")
    Me.cmdHistoryDir.UseVisualStyleBackColor = True
    '
    'pnlButtons
    '
    Me.pnlButtons.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlButtons.AutoSize = True
    Me.pnlButtons.ColumnCount = 2
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlButtons.Controls.Add(Me.cmdSave, 0, 0)
    Me.pnlButtons.Controls.Add(Me.cmdClose, 1, 0)
    Me.pnlButtons.Location = New System.Drawing.Point(448, 314)
    Me.pnlButtons.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.RowCount = 1
    Me.pnlButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlButtons.Size = New System.Drawing.Size(162, 28)
    Me.pnlButtons.TabIndex = 25
    '
    'cmdSave
    '
    Me.cmdSave.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdSave.AutoSize = True
    Me.cmdSave.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdSave.Location = New System.Drawing.Point(3, 3)
    Me.cmdSave.Name = "cmdSave"
    Me.cmdSave.Size = New System.Drawing.Size(75, 22)
    Me.cmdSave.TabIndex = 0
    Me.cmdSave.Text = "Save"
    Me.ttConfig.SetTooltip(Me.cmdSave, "Save Changes to the Configuration.")
    Me.cmdSave.UseVisualStyleBackColor = True
    '
    'cmdClose
    '
    Me.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdClose.AutoSize = True
    Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdClose.Location = New System.Drawing.Point(84, 3)
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.Size = New System.Drawing.Size(75, 22)
    Me.cmdClose.TabIndex = 1
    Me.cmdClose.Text = "Close"
    Me.ttConfig.SetTooltip(Me.cmdClose, "Close the Configuration window.")
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'pctSRT
    '
    Me.pctSRT.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
    Me.pctSRT.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pctSRT.Location = New System.Drawing.Point(332, 3)
    Me.pctSRT.Name = "pctSRT"
    Me.pnlConfig.SetRowSpan(Me.pctSRT, 2)
    Me.pctSRT.Size = New System.Drawing.Size(275, 50)
    Me.pctSRT.TabIndex = 20
    Me.pctSRT.TabStop = False
    Me.ttConfig.SetTooltip(Me.pctSRT, "Beep . . . Beep . . . Beep . . . Beep . . .")
    '
    'gbProxy
    '
    Me.gbProxy.Controls.Add(Me.pnlProxy)
    Me.gbProxy.Dock = System.Windows.Forms.DockStyle.Fill
    Me.gbProxy.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.gbProxy.Location = New System.Drawing.Point(332, 83)
    Me.gbProxy.Name = "gbProxy"
    Me.pnlConfig.SetRowSpan(Me.gbProxy, 7)
    Me.gbProxy.Size = New System.Drawing.Size(275, 176)
    Me.gbProxy.TabIndex = 23
    Me.gbProxy.TabStop = False
    Me.gbProxy.Text = "HTTP Pro&xy"
    '
    'pnlProxy
    '
    Me.pnlProxy.AutoSize = True
    Me.pnlProxy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlProxy.ColumnCount = 2
    Me.pnlProxy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlProxy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlProxy.Controls.Add(Me.lblProxyType, 0, 0)
    Me.pnlProxy.Controls.Add(Me.txtProxyUser, 0, 4)
    Me.pnlProxy.Controls.Add(Me.lblProxyUser, 0, 3)
    Me.pnlProxy.Controls.Add(Me.txtProxyPassword, 1, 4)
    Me.pnlProxy.Controls.Add(Me.lblProxyPassword, 1, 3)
    Me.pnlProxy.Controls.Add(Me.lblProxyAddr, 0, 1)
    Me.pnlProxy.Controls.Add(Me.txtProxyAddress, 0, 2)
    Me.pnlProxy.Controls.Add(Me.lblProxyPort, 1, 1)
    Me.pnlProxy.Controls.Add(Me.txtProxyPort, 1, 2)
    Me.pnlProxy.Controls.Add(Me.lblProxyDomain, 0, 5)
    Me.pnlProxy.Controls.Add(Me.txtProxyDomain, 0, 6)
    Me.pnlProxy.Controls.Add(Me.cmbProxyType, 1, 0)
    Me.pnlProxy.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlProxy.Location = New System.Drawing.Point(3, 16)
    Me.pnlProxy.Name = "pnlProxy"
    Me.pnlProxy.RowCount = 7
    Me.pnlProxy.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99813!))
    Me.pnlProxy.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProxy.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063!))
    Me.pnlProxy.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProxy.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063!))
    Me.pnlProxy.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProxy.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063!))
    Me.pnlProxy.Size = New System.Drawing.Size(269, 157)
    Me.pnlProxy.TabIndex = 0
    '
    'lblProxyType
    '
    Me.lblProxyType.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProxyType.AutoSize = True
    Me.lblProxyType.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblProxyType.Location = New System.Drawing.Point(3, 8)
    Me.lblProxyType.Name = "lblProxyType"
    Me.lblProxyType.Size = New System.Drawing.Size(63, 13)
    Me.lblProxyType.TabIndex = 0
    Me.lblProxyType.Text = "Proxy Type:"
    '
    'txtProxyUser
    '
    Me.txtProxyUser.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtProxyUser.Location = New System.Drawing.Point(3, 88)
    Me.txtProxyUser.Name = "txtProxyUser"
    Me.txtProxyUser.Size = New System.Drawing.Size(128, 20)
    Me.txtProxyUser.TabIndex = 7
    Me.ttConfig.SetTooltip(Me.txtProxyUser, "Optional Username for HTTP Proxy authentication.")
    '
    'lblProxyUser
    '
    Me.lblProxyUser.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProxyUser.AutoSize = True
    Me.lblProxyUser.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblProxyUser.Location = New System.Drawing.Point(3, 71)
    Me.lblProxyUser.Name = "lblProxyUser"
    Me.lblProxyUser.Size = New System.Drawing.Size(58, 13)
    Me.lblProxyUser.TabIndex = 6
    Me.lblProxyUser.Text = "Username:"
    '
    'txtProxyPassword
    '
    Me.txtProxyPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtProxyPassword.Location = New System.Drawing.Point(137, 88)
    Me.txtProxyPassword.Name = "txtProxyPassword"
    Me.txtProxyPassword.Size = New System.Drawing.Size(129, 20)
    Me.txtProxyPassword.TabIndex = 9
    Me.ttConfig.SetTooltip(Me.txtProxyPassword, "Optional Password for HTTP Proxy authentication.")
    Me.txtProxyPassword.UseSystemPasswordChar = True
    '
    'lblProxyPassword
    '
    Me.lblProxyPassword.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProxyPassword.AutoSize = True
    Me.lblProxyPassword.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblProxyPassword.Location = New System.Drawing.Point(137, 71)
    Me.lblProxyPassword.Name = "lblProxyPassword"
    Me.lblProxyPassword.Size = New System.Drawing.Size(56, 13)
    Me.lblProxyPassword.TabIndex = 8
    Me.lblProxyPassword.Text = "Password:"
    '
    'lblProxyAddr
    '
    Me.lblProxyAddr.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProxyAddr.AutoSize = True
    Me.lblProxyAddr.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblProxyAddr.Location = New System.Drawing.Point(3, 29)
    Me.lblProxyAddr.Name = "lblProxyAddr"
    Me.lblProxyAddr.Size = New System.Drawing.Size(61, 13)
    Me.lblProxyAddr.TabIndex = 2
    Me.lblProxyAddr.Text = "IP Address:"
    '
    'txtProxyAddress
    '
    Me.txtProxyAddress.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtProxyAddress.Location = New System.Drawing.Point(3, 46)
    Me.txtProxyAddress.Name = "txtProxyAddress"
    Me.txtProxyAddress.Size = New System.Drawing.Size(128, 20)
    Me.txtProxyAddress.TabIndex = 3
    Me.ttConfig.SetTooltip(Me.txtProxyAddress, "Address of HTTP Proxy to connect through.")
    '
    'lblProxyPort
    '
    Me.lblProxyPort.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProxyPort.AutoSize = True
    Me.lblProxyPort.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblProxyPort.Location = New System.Drawing.Point(137, 29)
    Me.lblProxyPort.Name = "lblProxyPort"
    Me.lblProxyPort.Size = New System.Drawing.Size(29, 13)
    Me.lblProxyPort.TabIndex = 4
    Me.lblProxyPort.Text = "Port:"
    '
    'txtProxyPort
    '
    Me.txtProxyPort.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.txtProxyPort.LargeIncrement = CType(20UI, UInteger)
    Me.txtProxyPort.Location = New System.Drawing.Point(137, 46)
    Me.txtProxyPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
    Me.txtProxyPort.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
    Me.txtProxyPort.Name = "txtProxyPort"
    Me.txtProxyPort.Size = New System.Drawing.Size(50, 20)
    Me.txtProxyPort.TabIndex = 5
    Me.txtProxyPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    Me.ttConfig.SetTooltip(Me.txtProxyPort, "Port to connect to HTTP proxy over.")
    Me.txtProxyPort.Value = New Decimal(New Integer() {8080, 0, 0, 0})
    '
    'lblProxyDomain
    '
    Me.lblProxyDomain.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProxyDomain.AutoSize = True
    Me.lblProxyDomain.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblProxyDomain.Location = New System.Drawing.Point(3, 113)
    Me.lblProxyDomain.Name = "lblProxyDomain"
    Me.lblProxyDomain.Size = New System.Drawing.Size(46, 13)
    Me.lblProxyDomain.TabIndex = 10
    Me.lblProxyDomain.Text = "Domain:"
    '
    'txtProxyDomain
    '
    Me.txtProxyDomain.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlProxy.SetColumnSpan(Me.txtProxyDomain, 2)
    Me.txtProxyDomain.Location = New System.Drawing.Point(3, 131)
    Me.txtProxyDomain.Name = "txtProxyDomain"
    Me.txtProxyDomain.Size = New System.Drawing.Size(263, 20)
    Me.txtProxyDomain.TabIndex = 11
    Me.ttConfig.SetTooltip(Me.txtProxyDomain, "Optional Domain for HTTP Proxy authentication.")
    '
    'cmbProxyType
    '
    Me.cmbProxyType.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmbProxyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cmbProxyType.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmbProxyType.FormattingEnabled = True
    Me.cmbProxyType.Items.AddRange(New Object() {"None", "System", "IP", "URL"})
    Me.cmbProxyType.Location = New System.Drawing.Point(137, 4)
    Me.cmbProxyType.Name = "cmbProxyType"
    Me.cmbProxyType.Size = New System.Drawing.Size(129, 21)
    Me.cmbProxyType.TabIndex = 1
    Me.ttConfig.SetTooltip(Me.cmbProxyType, "Type of Proxy to Use" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " None: No Proxy" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " System: Default System Proxy Settings" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " I" & _
        "P: HTTP Proxy by IP Address and Port" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " URL: HTTP Proxy by Web URL")
    '
    'pnlOverAlert
    '
    Me.pnlOverAlert.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlOverAlert.AutoSize = True
    Me.pnlOverAlert.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlOverAlert.ColumnCount = 5
    Me.pnlOverAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlOverAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlOverAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlOverAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlOverAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlOverAlert.Controls.Add(Me.txtOverSize, 1, 0)
    Me.pnlOverAlert.Controls.Add(Me.lblOverSize, 2, 0)
    Me.pnlOverAlert.Controls.Add(Me.txtOverTime, 3, 0)
    Me.pnlOverAlert.Controls.Add(Me.lblOverTime, 4, 0)
    Me.pnlOverAlert.Location = New System.Drawing.Point(96, 286)
    Me.pnlOverAlert.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlOverAlert.Name = "pnlOverAlert"
    Me.pnlOverAlert.RowCount = 1
    Me.pnlOverAlert.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlOverAlert.Size = New System.Drawing.Size(233, 26)
    Me.pnlOverAlert.TabIndex = 21
    '
    'txtOverSize
    '
    Me.txtOverSize.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtOverSize.Increment = New Decimal(New Integer() {100, 0, 0, 0})
    Me.txtOverSize.Location = New System.Drawing.Point(3, 3)
    Me.txtOverSize.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
    Me.txtOverSize.Minimum = New Decimal(New Integer() {25, 0, 0, 0})
    Me.txtOverSize.Name = "txtOverSize"
    Me.txtOverSize.Size = New System.Drawing.Size(66, 20)
    Me.txtOverSize.TabIndex = 1
    Me.txtOverSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    Me.ttConfig.SetTooltip(Me.txtOverSize, "Enter the amount of usage to display an alert about (in Megabytes).")
    Me.txtOverSize.Value = New Decimal(New Integer() {100, 0, 0, 0})
    '
    'lblOverSize
    '
    Me.lblOverSize.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblOverSize.AutoSize = True
    Me.lblOverSize.Location = New System.Drawing.Point(75, 6)
    Me.lblOverSize.Name = "lblOverSize"
    Me.lblOverSize.Size = New System.Drawing.Size(34, 13)
    Me.lblOverSize.TabIndex = 2
    Me.lblOverSize.Text = "MB in"
    '
    'txtOverTime
    '
    Me.txtOverTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtOverTime.Increment = New Decimal(New Integer() {15, 0, 0, 0})
    Me.txtOverTime.Location = New System.Drawing.Point(115, 3)
    Me.txtOverTime.Maximum = New Decimal(New Integer() {360, 0, 0, 0})
    Me.txtOverTime.Minimum = New Decimal(New Integer() {15, 0, 0, 0})
    Me.txtOverTime.Name = "txtOverTime"
    Me.txtOverTime.Size = New System.Drawing.Size(66, 20)
    Me.txtOverTime.TabIndex = 3
    Me.txtOverTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    Me.ttConfig.SetTooltip(Me.txtOverTime, "Enter the duration of time to check for the defined usage (in minutes).")
    Me.txtOverTime.Value = New Decimal(New Integer() {15, 0, 0, 0})
    '
    'lblOverTime
    '
    Me.lblOverTime.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblOverTime.AutoSize = True
    Me.lblOverTime.Location = New System.Drawing.Point(187, 6)
    Me.lblOverTime.Name = "lblOverTime"
    Me.lblOverTime.Size = New System.Drawing.Size(43, 13)
    Me.lblOverTime.TabIndex = 4
    Me.lblOverTime.Text = "minutes"
    '
    'lblProvider
    '
    Me.lblProvider.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProvider.AutoSize = True
    Me.lblProvider.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblProvider.Location = New System.Drawing.Point(3, 34)
    Me.lblProvider.Name = "lblProvider"
    Me.lblProvider.Size = New System.Drawing.Size(49, 13)
    Me.lblProvider.TabIndex = 2
    Me.lblProvider.Text = "Pro&vider:"
    '
    'cmbProvider
    '
    Me.cmbProvider.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmbProvider.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmbProvider.FormattingEnabled = True
    Me.cmbProvider.Location = New System.Drawing.Point(99, 30)
    Me.cmbProvider.Name = "cmbProvider"
    Me.cmbProvider.Size = New System.Drawing.Size(227, 21)
    Me.cmbProvider.TabIndex = 3
    Me.ttConfig.SetTooltip(Me.cmbProvider, "Your ViaSat Provider domain. If you were given an E-Mail address, this is everyth" & _
        "ing after the @ symbol. You can choose a domain from the dropdown or enter your " & _
        "own to add it to the list.")
    '
    'lblInterval
    '
    Me.lblInterval.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblInterval.AutoSize = True
    Me.lblInterval.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblInterval.Location = New System.Drawing.Point(3, 114)
    Me.lblInterval.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblInterval.Name = "lblInterval"
    Me.lblInterval.Size = New System.Drawing.Size(83, 13)
    Me.lblInterval.TabIndex = 9
    Me.lblInterval.Text = "Update &Interval:"
    '
    'pnlKey
    '
    Me.pnlKey.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.pnlKey.AutoSize = True
    Me.pnlKey.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlKey.ColumnCount = 6
    Me.pnlKey.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlKey.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlKey.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlKey.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlKey.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlKey.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlKey.Controls.Add(Me.pctKeyState, 5, 0)
    Me.pnlKey.Controls.Add(Me.txtKey1, 0, 0)
    Me.pnlKey.Controls.Add(Me.txtKey2, 1, 0)
    Me.pnlKey.Controls.Add(Me.txtKey3, 2, 0)
    Me.pnlKey.Controls.Add(Me.txtKey4, 3, 0)
    Me.pnlKey.Controls.Add(Me.txtKey5, 4, 0)
    Me.pnlKey.Location = New System.Drawing.Point(96, 56)
    Me.pnlKey.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlKey.Name = "pnlKey"
    Me.pnlKey.RowCount = 1
    Me.pnlKey.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlKey.Size = New System.Drawing.Size(230, 24)
    Me.pnlKey.TabIndex = 5
    '
    'pctKeyState
    '
    Me.pctKeyState.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.pctKeyState.Location = New System.Drawing.Point(211, 4)
    Me.pctKeyState.Margin = New System.Windows.Forms.Padding(1, 3, 3, 3)
    Me.pctKeyState.Name = "pctKeyState"
    Me.pctKeyState.Size = New System.Drawing.Size(16, 16)
    Me.pctKeyState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
    Me.pctKeyState.TabIndex = 6
    Me.pctKeyState.TabStop = False
    '
    'txtKey1
    '
    Me.txtKey1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtKey1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
    Me.txtKey1.Font = New System.Drawing.Font("Lucida Console", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtKey1.Location = New System.Drawing.Point(3, 3)
    Me.txtKey1.Margin = New System.Windows.Forms.Padding(3, 3, 1, 3)
    Me.txtKey1.MaxLength = 6
    Me.txtKey1.Name = "txtKey1"
    Me.txtKey1.Size = New System.Drawing.Size(48, 18)
    Me.txtKey1.TabIndex = 0
    Me.txtKey1.Text = "@@@@@@"
    Me.txtKey1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
    Me.ttConfig.SetTooltip(Me.txtKey1, "Your Remote Usage Service Product Key.")
    '
    'txtKey2
    '
    Me.txtKey2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtKey2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
    Me.txtKey2.Font = New System.Drawing.Font("Lucida Console", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtKey2.Location = New System.Drawing.Point(53, 3)
    Me.txtKey2.Margin = New System.Windows.Forms.Padding(1, 3, 1, 3)
    Me.txtKey2.MaxLength = 4
    Me.txtKey2.Name = "txtKey2"
    Me.txtKey2.Size = New System.Drawing.Size(34, 18)
    Me.txtKey2.TabIndex = 1
    Me.txtKey2.Text = "@@@@"
    Me.txtKey2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
    Me.ttConfig.SetTooltip(Me.txtKey2, "Your Remote Usage Service Product Key.")
    '
    'txtKey3
    '
    Me.txtKey3.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtKey3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
    Me.txtKey3.Font = New System.Drawing.Font("Lucida Console", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtKey3.Location = New System.Drawing.Point(89, 3)
    Me.txtKey3.Margin = New System.Windows.Forms.Padding(1, 3, 1, 3)
    Me.txtKey3.MaxLength = 4
    Me.txtKey3.Name = "txtKey3"
    Me.txtKey3.Size = New System.Drawing.Size(34, 18)
    Me.txtKey3.TabIndex = 2
    Me.txtKey3.Text = "@@@@"
    Me.txtKey3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
    Me.ttConfig.SetTooltip(Me.txtKey3, "Your Remote Usage Service Product Key.")
    '
    'txtKey4
    '
    Me.txtKey4.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtKey4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
    Me.txtKey4.Font = New System.Drawing.Font("Lucida Console", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtKey4.Location = New System.Drawing.Point(125, 3)
    Me.txtKey4.Margin = New System.Windows.Forms.Padding(1, 3, 1, 3)
    Me.txtKey4.MaxLength = 4
    Me.txtKey4.Name = "txtKey4"
    Me.txtKey4.Size = New System.Drawing.Size(34, 18)
    Me.txtKey4.TabIndex = 3
    Me.txtKey4.Text = "@@@@"
    Me.txtKey4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
    Me.ttConfig.SetTooltip(Me.txtKey4, "Your Remote Usage Service Product Key.")
    '
    'txtKey5
    '
    Me.txtKey5.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtKey5.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
    Me.txtKey5.Font = New System.Drawing.Font("Lucida Console", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtKey5.Location = New System.Drawing.Point(161, 3)
    Me.txtKey5.Margin = New System.Windows.Forms.Padding(1, 3, 1, 3)
    Me.txtKey5.MaxLength = 6
    Me.txtKey5.Name = "txtKey5"
    Me.txtKey5.Size = New System.Drawing.Size(48, 18)
    Me.txtKey5.TabIndex = 4
    Me.txtKey5.Text = "@@@@@@"
    Me.txtKey5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
    Me.ttConfig.SetTooltip(Me.txtKey5, "Your Remote Usage Service Product Key.")
    '
    'lblPurchaseKey
    '
    Me.lblPurchaseKey.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblPurchaseKey.AutoSize = True
    Me.lblPurchaseKey.Cursor = System.Windows.Forms.Cursors.Hand
    Me.lblPurchaseKey.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
    Me.lblPurchaseKey.ForeColor = System.Drawing.Color.MediumBlue
    Me.lblPurchaseKey.Location = New System.Drawing.Point(332, 61)
    Me.lblPurchaseKey.Margin = New System.Windows.Forms.Padding(3)
    Me.lblPurchaseKey.Name = "lblPurchaseKey"
    Me.lblPurchaseKey.Size = New System.Drawing.Size(82, 13)
    Me.lblPurchaseKey.TabIndex = 6
    Me.lblPurchaseKey.TabStop = True
    Me.lblPurchaseKey.Text = "Purchase a Key"
    Me.ttConfig.SetTooltip(Me.lblPurchaseKey, "If you do not have a Product Key for Remote Usage, you can purchase one online fo" & _
        "r as little as $15.00 a year.")
    '
    'cmdAlertStyle
    '
    Me.cmdAlertStyle.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdAlertStyle.AutoSize = True
    Me.pnlConfig.SetColumnSpan(Me.cmdAlertStyle, 2)
    Me.cmdAlertStyle.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdAlertStyle.Location = New System.Drawing.Point(114, 315)
    Me.cmdAlertStyle.Name = "cmdAlertStyle"
    Me.cmdAlertStyle.Size = New System.Drawing.Size(101, 24)
    Me.cmdAlertStyle.TabIndex = 22
    Me.cmdAlertStyle.Text = "Select Alert St&yle"
    Me.ttConfig.SetTooltip(Me.cmdAlertStyle, "Choose a style for the Alert Windows used for Usage Alerts and Parse Failures.")
    Me.cmdAlertStyle.UseVisualStyleBackColor = True
    '
    'chkBeta
    '
    Me.chkBeta.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkBeta.AutoSize = True
    Me.pnlConfig.SetColumnSpan(Me.chkBeta, 2)
    Me.chkBeta.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkBeta.Location = New System.Drawing.Point(3, 265)
    Me.chkBeta.Name = "chkBeta"
    Me.chkBeta.Size = New System.Drawing.Size(150, 18)
    Me.chkBeta.TabIndex = 19
    Me.chkBeta.Text = "Check for &BETA updates"
    Me.ttConfig.SetTooltip(Me.chkBeta, "Check for updates to BETA versions when you open the About window.")
    Me.chkBeta.UseVisualStyleBackColor = True
    '
    'fswController
    '
    Me.fswController.EnableRaisingEvents = True
    Me.fswController.SynchronizingObject = Me
    '
    'tmrAnim
    '
    '
    'ttConfig
    '
    Me.ttConfig.AutoPopDelay = 30000
    Me.ttConfig.InitialDelay = 300
    Me.ttConfig.Persistant = True
    Me.ttConfig.ReshowDelay = 100
    '
    'frmConfig
    '
    Me.AcceptButton = Me.cmdSave
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.AutoSize = True
    Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.CancelButton = Me.cmdClose
    Me.ClientSize = New System.Drawing.Size(617, 350)
    Me.Controls.Add(Me.pnlConfig)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.Icon = Global.RestrictionTracker.My.Resources.Resources.sat
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmConfig"
    Me.Padding = New System.Windows.Forms.Padding(3)
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "Satellite Restriction Tracker Configuration"
    Me.pnlConfig.ResumeLayout(False)
    Me.pnlConfig.PerformLayout()
    Me.pnlPassword.ResumeLayout(False)
    Me.pnlPassword.PerformLayout()
    Me.pnlTimeout.ResumeLayout(False)
    Me.pnlTimeout.PerformLayout()
    CType(Me.txtTimeout, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlInterval.ResumeLayout(False)
    Me.pnlInterval.PerformLayout()
    CType(Me.txtInterval, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlAccuracy.ResumeLayout(False)
    Me.pnlAccuracy.PerformLayout()
    CType(Me.txtAccuracy, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlHistoryDir.ResumeLayout(False)
    Me.pnlHistoryDir.PerformLayout()
    Me.pnlButtons.ResumeLayout(False)
    Me.pnlButtons.PerformLayout()
    CType(Me.pctSRT, System.ComponentModel.ISupportInitialize).EndInit()
    Me.gbProxy.ResumeLayout(False)
    Me.gbProxy.PerformLayout()
    Me.pnlProxy.ResumeLayout(False)
    Me.pnlProxy.PerformLayout()
    CType(Me.txtProxyPort, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlOverAlert.ResumeLayout(False)
    Me.pnlOverAlert.PerformLayout()
    CType(Me.txtOverSize, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtOverTime, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlKey.ResumeLayout(False)
    Me.pnlKey.PerformLayout()
    CType(Me.pctKeyState, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.fswController, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents pnlConfig As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblAccount As System.Windows.Forms.Label
  Friend WithEvents txtAccount As System.Windows.Forms.TextBox
  Friend WithEvents lblPassword As System.Windows.Forms.Label
  Friend WithEvents txtPassword As System.Windows.Forms.TextBox
  Friend WithEvents lblInterval As System.Windows.Forms.Label
  Friend WithEvents pnlInterval As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents txtInterval As RestrictionTracker.NumericUpDownIncrementable
  Friend WithEvents lblIntervalScale As System.Windows.Forms.Label
  Friend WithEvents chkStartUp As System.Windows.Forms.CheckBox
  Friend WithEvents lblAccuracy As System.Windows.Forms.Label
  Friend WithEvents pnlAccuracy As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents txtAccuracy As RestrictionTracker.NumericUpDownIncrementable
  Friend WithEvents lblAccuracyScale As System.Windows.Forms.Label
  Friend WithEvents chkService As System.Windows.Forms.CheckBox
  Friend WithEvents ttConfig As ToolTip
  Friend WithEvents lblHistoryDir As System.Windows.Forms.Label
  Friend WithEvents pnlHistoryDir As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents txtHistoryDir As System.Windows.Forms.TextBox
  Friend WithEvents cmdHistoryDir As System.Windows.Forms.Button
  Friend WithEvents fswController As System.IO.FileSystemWatcher
  Friend WithEvents lblPurchaseKey As LinkLabel
  Friend WithEvents pnlKey As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctKeyState As System.Windows.Forms.PictureBox
  Friend WithEvents txtKey1 As System.Windows.Forms.TextBox
  Friend WithEvents txtKey2 As System.Windows.Forms.TextBox
  Friend WithEvents txtKey3 As System.Windows.Forms.TextBox
  Friend WithEvents txtKey4 As System.Windows.Forms.TextBox
  Friend WithEvents txtKey5 As System.Windows.Forms.TextBox
  Friend WithEvents pnlPassword As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdPassDisplay As System.Windows.Forms.Button
  Friend WithEvents pnlTimeout As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents txtTimeout As RestrictionTracker.NumericUpDownIncrementable
  Friend WithEvents lblTimeoutScale As System.Windows.Forms.Label
  Friend WithEvents lblTimeout As System.Windows.Forms.Label
  Friend WithEvents pnlProxy As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblProxyType As System.Windows.Forms.Label
  Friend WithEvents cmbProxyType As System.Windows.Forms.ComboBox
  Friend WithEvents lblProxyAddr As System.Windows.Forms.Label
  Friend WithEvents txtProxyAddress As System.Windows.Forms.TextBox
  Friend WithEvents lblProxyPort As System.Windows.Forms.Label
  Friend WithEvents txtProxyPort As RestrictionTracker.NumericUpDownIncrementable
  Friend WithEvents lblProxyUser As System.Windows.Forms.Label
  Friend WithEvents txtProxyUser As System.Windows.Forms.TextBox
  Friend WithEvents lblProxyPassword As System.Windows.Forms.Label
  Friend WithEvents txtProxyPassword As System.Windows.Forms.TextBox
  Friend WithEvents lblProxyDomain As System.Windows.Forms.Label
  Friend WithEvents txtProxyDomain As System.Windows.Forms.TextBox
  Friend WithEvents gbProxy As System.Windows.Forms.GroupBox
  Friend WithEvents chkBeta As System.Windows.Forms.CheckBox
  Friend WithEvents pnlButtons As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdSave As System.Windows.Forms.Button
  Friend WithEvents cmdClose As System.Windows.Forms.Button
  Friend WithEvents pctSRT As System.Windows.Forms.PictureBox
  Friend WithEvents tmrAnim As System.Windows.Forms.Timer
  Friend WithEvents pnlOverAlert As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents txtOverSize As NumericUpDownIncrementable
  Friend WithEvents lblOverSize As System.Windows.Forms.Label
  Friend WithEvents txtOverTime As NumericUpDownIncrementable
  Friend WithEvents lblOverTime As System.Windows.Forms.Label
  Friend WithEvents cmdAlertStyle As System.Windows.Forms.Button
  Friend WithEvents lblProvider As System.Windows.Forms.Label
  Friend WithEvents cmbProvider As System.Windows.Forms.ComboBox
  Friend WithEvents cmdColors As System.Windows.Forms.Button
  Friend WithEvents lblKey As System.Windows.Forms.Label
  Friend WithEvents chkOverAlert As System.Windows.Forms.CheckBox
End Class
