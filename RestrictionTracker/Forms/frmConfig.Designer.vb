﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
    Me.tbsConfig = New System.Windows.Forms.TabControl()
    Me.tabAccount = New System.Windows.Forms.TabPage()
    Me.pnlAccount = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlAccountViaSatTitle = New System.Windows.Forms.TableLayoutPanel()
    Me.lblAccountViaSatTitle = New System.Windows.Forms.Label()
    Me.pnlAccountViaSat = New System.Windows.Forms.TableLayoutPanel()
    Me.pctAccountViaSatIcon = New System.Windows.Forms.PictureBox()
    Me.pnlAccountViaSatInput = New System.Windows.Forms.TableLayoutPanel()
    Me.lblAccount = New System.Windows.Forms.Label()
    Me.lblPassword = New System.Windows.Forms.Label()
    Me.txtAccount = New System.Windows.Forms.TextBox()
    Me.lblAccountViaSatDescription = New System.Windows.Forms.Label()
    Me.lblAccountShareWarning1 = New System.Windows.Forms.Label()
    Me.chkAccountShare = New System.Windows.Forms.CheckBox()
    Me.lblAccountShareWarning2 = New System.Windows.Forms.Label()
    Me.pnlAccountShareTitle = New System.Windows.Forms.TableLayoutPanel()
    Me.lblAccountShareTitle = New System.Windows.Forms.Label()
    Me.tabPrefs = New System.Windows.Forms.TabPage()
    Me.pnlPrefs = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlPrefInterfaceTitle = New System.Windows.Forms.TableLayoutPanel()
    Me.lblPrefInterfaceTitle = New System.Windows.Forms.Label()
    Me.pnlPrefColor = New System.Windows.Forms.TableLayoutPanel()
    Me.pctPrefColorIcon = New System.Windows.Forms.PictureBox()
    Me.lblPrefColorDescription = New System.Windows.Forms.Label()
    Me.cmdColors = New System.Windows.Forms.Button()
    Me.pnlPrefAlert = New System.Windows.Forms.TableLayoutPanel()
    Me.pctPrefAlertIcon = New System.Windows.Forms.PictureBox()
    Me.lblPrefAlertDescription = New System.Windows.Forms.Label()
    Me.chkOverAlert = New System.Windows.Forms.CheckBox()
    Me.lblOverTime1 = New System.Windows.Forms.Label()
    Me.lblOverSize2 = New System.Windows.Forms.Label()
    Me.lblOverTime2 = New System.Windows.Forms.Label()
    Me.lblOverSize1 = New System.Windows.Forms.Label()
    Me.cmdAlertStyle = New System.Windows.Forms.Button()
    Me.pnlPrefAccuracy = New System.Windows.Forms.TableLayoutPanel()
    Me.pctPrefAccuracyIcon = New System.Windows.Forms.PictureBox()
    Me.pnlPrefAccuracyInput = New System.Windows.Forms.TableLayoutPanel()
    Me.lblInterval1 = New System.Windows.Forms.Label()
    Me.lblInterval2 = New System.Windows.Forms.Label()
    Me.lblAccuracy1 = New System.Windows.Forms.Label()
    Me.lblAccuracy2 = New System.Windows.Forms.Label()
    Me.pnlPrefStartTitle = New System.Windows.Forms.TableLayoutPanel()
    Me.lblPrefStartTitle = New System.Windows.Forms.Label()
    Me.pnlPrefAccuracyTitle = New System.Windows.Forms.TableLayoutPanel()
    Me.lblPrefAccuracyTitle = New System.Windows.Forms.Label()
    Me.pnlPrefAlertTitle = New System.Windows.Forms.TableLayoutPanel()
    Me.lblPrefAlertTitle = New System.Windows.Forms.Label()
    Me.pnlPrefColorTitle = New System.Windows.Forms.TableLayoutPanel()
    Me.lblPrefColorTitle = New System.Windows.Forms.Label()
    Me.pnlPrefStart = New System.Windows.Forms.TableLayoutPanel()
    Me.pctPrefStartIcon = New System.Windows.Forms.PictureBox()
    Me.pnlPrefStartInput = New System.Windows.Forms.TableLayoutPanel()
    Me.chkStartUp = New System.Windows.Forms.CheckBox()
    Me.lblStartWait1 = New System.Windows.Forms.Label()
    Me.lblStartWait2 = New System.Windows.Forms.Label()
    Me.chkService = New System.Windows.Forms.CheckBox()
    Me.chkAutoHide = New System.Windows.Forms.CheckBox()
    Me.pnlPrefInterface = New System.Windows.Forms.TableLayoutPanel()
    Me.chkTrayIcon = New System.Windows.Forms.CheckBox()
    Me.chkScaleScreen = New System.Windows.Forms.CheckBox()
    Me.pctPrefInterfaceIcon = New System.Windows.Forms.PictureBox()
    Me.chkTrayMin = New System.Windows.Forms.CheckBox()
    Me.chkTrayAnim = New System.Windows.Forms.CheckBox()
    Me.chkTrayClose = New System.Windows.Forms.CheckBox()
    Me.tabNetwork = New System.Windows.Forms.TabPage()
    Me.pnlNetwork = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlNetworkProtocol = New System.Windows.Forms.TableLayoutPanel()
    Me.lblNetworkProtocolDescription = New System.Windows.Forms.Label()
    Me.pctNetworkProtocolIcon = New System.Windows.Forms.PictureBox()
    Me.chkTLSProxy = New System.Windows.Forms.CheckBox()
    Me.chkNetworkSecurityEnforce = New System.Windows.Forms.CheckBox()
    Me.chkNetworkProtocolTLS13 = New System.Windows.Forms.CheckBox()
    Me.chkNetworkProtocolSSL3 = New System.Windows.Forms.CheckBox()
    Me.chkNetworkProtocolTLS12 = New System.Windows.Forms.CheckBox()
    Me.chkNetworkProtocolTLS10 = New System.Windows.Forms.CheckBox()
    Me.chkNetworkProtocolTLS11 = New System.Windows.Forms.CheckBox()
    Me.pnlNetworkProtocolTitle = New System.Windows.Forms.TableLayoutPanel()
    Me.lblNetworkProtocolTitle = New System.Windows.Forms.Label()
    Me.pnlNetworkUpdate = New System.Windows.Forms.TableLayoutPanel()
    Me.pctNetworkUpdateIcon = New System.Windows.Forms.PictureBox()
    Me.pnlNetworkUpdateTime = New System.Windows.Forms.TableLayoutPanel()
    Me.lblUpdateInterval = New System.Windows.Forms.Label()
    Me.cmbUpdateInterval = New System.Windows.Forms.ComboBox()
    Me.chkUpdateBETA = New System.Windows.Forms.CheckBox()
    Me.cmbUpdateAutomation = New System.Windows.Forms.ComboBox()
    Me.pnlNetworkProxy = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlProxy = New System.Windows.Forms.TableLayoutPanel()
    Me.lblProxyType = New System.Windows.Forms.Label()
    Me.txtProxyUser = New System.Windows.Forms.TextBox()
    Me.lblProxyUser = New System.Windows.Forms.Label()
    Me.lblProxyPassword = New System.Windows.Forms.Label()
    Me.lblProxyAddr = New System.Windows.Forms.Label()
    Me.txtProxyAddress = New System.Windows.Forms.TextBox()
    Me.lblProxyPort = New System.Windows.Forms.Label()
    Me.lblProxyDomain = New System.Windows.Forms.Label()
    Me.txtProxyDomain = New System.Windows.Forms.TextBox()
    Me.cmbProxyType = New System.Windows.Forms.ComboBox()
    Me.pctNetworkProxyIcon = New System.Windows.Forms.PictureBox()
    Me.lblNetworkProxyDescrption = New System.Windows.Forms.Label()
    Me.pnlNetworkProxyTitle = New System.Windows.Forms.TableLayoutPanel()
    Me.lblNetworkProxyTitle = New System.Windows.Forms.Label()
    Me.pnlNetworkTimeoutTitle = New System.Windows.Forms.TableLayoutPanel()
    Me.lblNetworkTimeoutTitle = New System.Windows.Forms.Label()
    Me.pnlNetworkTimeout = New System.Windows.Forms.TableLayoutPanel()
    Me.pctNetworkTimeoutIcon = New System.Windows.Forms.PictureBox()
    Me.lblTimeout2 = New System.Windows.Forms.Label()
    Me.lblTimeout1 = New System.Windows.Forms.Label()
    Me.lblNetworkTimeoutDescription = New System.Windows.Forms.Label()
    Me.lblRetries1 = New System.Windows.Forms.Label()
    Me.lblRetries2 = New System.Windows.Forms.Label()
    Me.pnlNetworkUpdateTitle = New System.Windows.Forms.TableLayoutPanel()
    Me.lblNetworkUpdateTitle = New System.Windows.Forms.Label()
    Me.tabAdvanced = New System.Windows.Forms.TabPage()
    Me.pnlAdvanced = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlAdvancedPortable = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlPortableDir = New System.Windows.Forms.TableLayoutPanel()
    Me.txtPortableDir = New System.Windows.Forms.TextBox()
    Me.cmdPortableDir = New System.Windows.Forms.Button()
    Me.pctAdvancedPortableIcon = New System.Windows.Forms.PictureBox()
    Me.lblAdvancedPortableDescription = New System.Windows.Forms.Label()
    Me.cmdMakePortable = New System.Windows.Forms.Button()
    Me.lblPortableDir = New System.Windows.Forms.Label()
    Me.pnlAdvancedPortableTitle = New System.Windows.Forms.TableLayoutPanel()
    Me.lblAdvancedPortableTitle = New System.Windows.Forms.Label()
    Me.pnlAdvancedData = New System.Windows.Forms.TableLayoutPanel()
    Me.pctAdvancedDataIcon = New System.Windows.Forms.PictureBox()
    Me.pnlAdvancedDataInput = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlHistoryDir = New System.Windows.Forms.TableLayoutPanel()
    Me.txtHistoryDir = New System.Windows.Forms.TextBox()
    Me.cmdHistoryDir = New System.Windows.Forms.Button()
    Me.lblAdvancedDataDescription = New System.Windows.Forms.Label()
    Me.optHistoryCustom = New System.Windows.Forms.RadioButton()
    Me.optHistoryProgramData = New System.Windows.Forms.RadioButton()
    Me.optHistoryAppData = New System.Windows.Forms.RadioButton()
    Me.cmdHistoryDirOpen = New System.Windows.Forms.Button()
    Me.pnlAdvancedDataTitle = New System.Windows.Forms.TableLayoutPanel()
    Me.lblAdvancedDataTitle = New System.Windows.Forms.Label()
    Me.pnlAdvancedNetTestTitle = New System.Windows.Forms.TableLayoutPanel()
    Me.lblAdvancedNetTestTitle = New System.Windows.Forms.Label()
    Me.pnlAdvancedNetTest = New System.Windows.Forms.TableLayoutPanel()
    Me.pctAdvancedNetTestIcon = New System.Windows.Forms.PictureBox()
    Me.pnlAdvancedNetTestInput = New System.Windows.Forms.TableLayoutPanel()
    Me.lblAdvancedNetTestInput = New System.Windows.Forms.Label()
    Me.optNetTestCustom = New System.Windows.Forms.RadioButton()
    Me.optNetTestNone = New System.Windows.Forms.RadioButton()
    Me.optNetTestTestMyNet = New System.Windows.Forms.RadioButton()
    Me.txtNetTestCustom = New System.Windows.Forms.TextBox()
    Me.optNetTestSpeedTest = New System.Windows.Forms.RadioButton()
    Me.cmdSave = New System.Windows.Forms.Button()
    Me.cmdClose = New System.Windows.Forms.Button()
    Me.pnlConfig = New System.Windows.Forms.TableLayoutPanel()
    Me.pnlButtons = New System.Windows.Forms.TableLayoutPanel()
    Me.fswController = New System.IO.FileSystemWatcher()
    Me.tmrIcoWait = New System.Windows.Forms.Timer(Me.components)
    Me.mnuKey = New System.Windows.Forms.ContextMenu()
    Me.mnuKeyCut = New System.Windows.Forms.MenuItem()
    Me.mnuKeyCopy = New System.Windows.Forms.MenuItem()
    Me.mnuKeyPaste = New System.Windows.Forms.MenuItem()
    Me.mnuKeySpacer = New System.Windows.Forms.MenuItem()
    Me.mnuKeyDelete = New System.Windows.Forms.MenuItem()
    Me.mnuKeyClear = New System.Windows.Forms.MenuItem()
    Me.lnAccountViaSatTitle = New RestrictionTracker.LineBreak()
    Me.txtPassword = New RestrictionTracker.PasswordBox()
    Me.lnAccountShareTitle = New RestrictionTracker.LineBreak()
    Me.lnPrefInterfaceTitle = New RestrictionTracker.LineBreak()
    Me.txtOverTime = New RestrictionTracker.NumericUpDownIncrementable()
    Me.txtOverSize = New RestrictionTracker.NumericUpDownIncrementable()
    Me.txtInterval = New RestrictionTracker.NumericUpDownIncrementable()
    Me.txtAccuracy = New RestrictionTracker.NumericUpDownIncrementable()
    Me.lnPrefStartTitle = New RestrictionTracker.LineBreak()
    Me.lnPrefAccuracyTitle = New RestrictionTracker.LineBreak()
    Me.lnPrefAlertTitle = New RestrictionTracker.LineBreak()
    Me.lnPrefColorTitle = New RestrictionTracker.LineBreak()
    Me.txtStartWait = New RestrictionTracker.NumericUpDownIncrementable()
    Me.lnNetworkProtocolTitle = New RestrictionTracker.LineBreak()
    Me.txtProxyPassword = New RestrictionTracker.PasswordBox()
    Me.txtProxyPort = New RestrictionTracker.NumericUpDownIncrementable()
    Me.lnNetworkProxyTitle = New RestrictionTracker.LineBreak()
    Me.lnNetworkTimeoutTitle = New RestrictionTracker.LineBreak()
    Me.txtTimeout = New RestrictionTracker.NumericUpDownIncrementable()
    Me.txtRetries = New RestrictionTracker.NumericUpDownIncrementable()
    Me.lnNetworkUpdateTitle = New RestrictionTracker.LineBreak()
    Me.lnAdvancedPortableTitle = New RestrictionTracker.LineBreak()
    Me.lnAdvancedDataTitle = New RestrictionTracker.LineBreak()
    Me.lnAdvancedNetTestTitle = New RestrictionTracker.LineBreak()
    Me.ttConfig = New RestrictionTracker.ToolTip(Me.components)
    Me.tbsConfig.SuspendLayout()
    Me.tabAccount.SuspendLayout()
    Me.pnlAccount.SuspendLayout()
    Me.pnlAccountViaSatTitle.SuspendLayout()
    Me.pnlAccountViaSat.SuspendLayout()
    CType(Me.pctAccountViaSatIcon, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlAccountViaSatInput.SuspendLayout()
    Me.pnlAccountShareTitle.SuspendLayout()
    Me.tabPrefs.SuspendLayout()
    Me.pnlPrefs.SuspendLayout()
    Me.pnlPrefInterfaceTitle.SuspendLayout()
    Me.pnlPrefColor.SuspendLayout()
    CType(Me.pctPrefColorIcon, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlPrefAlert.SuspendLayout()
    CType(Me.pctPrefAlertIcon, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlPrefAccuracy.SuspendLayout()
    CType(Me.pctPrefAccuracyIcon, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlPrefAccuracyInput.SuspendLayout()
    Me.pnlPrefStartTitle.SuspendLayout()
    Me.pnlPrefAccuracyTitle.SuspendLayout()
    Me.pnlPrefAlertTitle.SuspendLayout()
    Me.pnlPrefColorTitle.SuspendLayout()
    Me.pnlPrefStart.SuspendLayout()
    CType(Me.pctPrefStartIcon, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlPrefStartInput.SuspendLayout()
    Me.pnlPrefInterface.SuspendLayout()
    CType(Me.pctPrefInterfaceIcon, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.tabNetwork.SuspendLayout()
    Me.pnlNetwork.SuspendLayout()
    Me.pnlNetworkProtocol.SuspendLayout()
    CType(Me.pctNetworkProtocolIcon, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlNetworkProtocolTitle.SuspendLayout()
    Me.pnlNetworkUpdate.SuspendLayout()
    CType(Me.pctNetworkUpdateIcon, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlNetworkUpdateTime.SuspendLayout()
    Me.pnlNetworkProxy.SuspendLayout()
    Me.pnlProxy.SuspendLayout()
    CType(Me.pctNetworkProxyIcon, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlNetworkProxyTitle.SuspendLayout()
    Me.pnlNetworkTimeoutTitle.SuspendLayout()
    Me.pnlNetworkTimeout.SuspendLayout()
    CType(Me.pctNetworkTimeoutIcon, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlNetworkUpdateTitle.SuspendLayout()
    Me.tabAdvanced.SuspendLayout()
    Me.pnlAdvanced.SuspendLayout()
    Me.pnlAdvancedPortable.SuspendLayout()
    Me.pnlPortableDir.SuspendLayout()
    CType(Me.pctAdvancedPortableIcon, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlAdvancedPortableTitle.SuspendLayout()
    Me.pnlAdvancedData.SuspendLayout()
    CType(Me.pctAdvancedDataIcon, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlAdvancedDataInput.SuspendLayout()
    Me.pnlHistoryDir.SuspendLayout()
    Me.pnlAdvancedDataTitle.SuspendLayout()
    Me.pnlAdvancedNetTestTitle.SuspendLayout()
    Me.pnlAdvancedNetTest.SuspendLayout()
    CType(Me.pctAdvancedNetTestIcon, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlAdvancedNetTestInput.SuspendLayout()
    Me.pnlConfig.SuspendLayout()
    Me.pnlButtons.SuspendLayout()
    CType(Me.fswController, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtOverTime, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtOverSize, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtInterval, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtAccuracy, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtStartWait, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtProxyPort, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtTimeout, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.txtRetries, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'tbsConfig
    '
    Me.tbsConfig.Controls.Add(Me.tabAccount)
    Me.tbsConfig.Controls.Add(Me.tabPrefs)
    Me.tbsConfig.Controls.Add(Me.tabNetwork)
    Me.tbsConfig.Controls.Add(Me.tabAdvanced)
    Me.tbsConfig.Dock = System.Windows.Forms.DockStyle.Fill
    Me.tbsConfig.Location = New System.Drawing.Point(6, 6)
    Me.tbsConfig.Margin = New System.Windows.Forms.Padding(6, 6, 6, 3)
    Me.tbsConfig.Name = "tbsConfig"
    Me.tbsConfig.SelectedIndex = 0
    Me.tbsConfig.Size = New System.Drawing.Size(393, 503)
    Me.tbsConfig.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
    Me.tbsConfig.TabIndex = 0
    '
    'tabAccount
    '
    Me.tabAccount.Controls.Add(Me.pnlAccount)
    Me.tabAccount.Location = New System.Drawing.Point(4, 22)
    Me.tabAccount.Name = "tabAccount"
    Me.tabAccount.Size = New System.Drawing.Size(385, 477)
    Me.tabAccount.TabIndex = 0
    Me.tabAccount.Text = "Account"
    Me.tabAccount.UseVisualStyleBackColor = True
    '
    'pnlAccount
    '
    Me.pnlAccount.ColumnCount = 1
    Me.pnlAccount.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAccount.Controls.Add(Me.pnlAccountViaSatTitle, 0, 0)
    Me.pnlAccount.Controls.Add(Me.pnlAccountViaSat, 0, 1)
    Me.pnlAccount.Controls.Add(Me.lblAccountShareWarning1, 0, 4)
    Me.pnlAccount.Controls.Add(Me.chkAccountShare, 0, 5)
    Me.pnlAccount.Controls.Add(Me.lblAccountShareWarning2, 0, 6)
    Me.pnlAccount.Controls.Add(Me.pnlAccountShareTitle, 0, 3)
    Me.pnlAccount.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlAccount.Location = New System.Drawing.Point(0, 0)
    Me.pnlAccount.Name = "pnlAccount"
    Me.pnlAccount.RowCount = 7
    Me.pnlAccount.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAccount.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAccount.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.06944!))
    Me.pnlAccount.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAccount.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAccount.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAccount.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.93056!))
    Me.pnlAccount.Size = New System.Drawing.Size(385, 477)
    Me.pnlAccount.TabIndex = 0
    '
    'pnlAccountViaSatTitle
    '
    Me.pnlAccountViaSatTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlAccountViaSatTitle.AutoSize = True
    Me.pnlAccountViaSatTitle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlAccountViaSatTitle.ColumnCount = 2
    Me.pnlAccountViaSatTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAccountViaSatTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAccountViaSatTitle.Controls.Add(Me.lblAccountViaSatTitle, 0, 0)
    Me.pnlAccountViaSatTitle.Controls.Add(Me.lnAccountViaSatTitle, 1, 0)
    Me.pnlAccountViaSatTitle.Location = New System.Drawing.Point(3, 10)
    Me.pnlAccountViaSatTitle.Margin = New System.Windows.Forms.Padding(3, 10, 3, 5)
    Me.pnlAccountViaSatTitle.Name = "pnlAccountViaSatTitle"
    Me.pnlAccountViaSatTitle.RowCount = 1
    Me.pnlAccountViaSatTitle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAccountViaSatTitle.Size = New System.Drawing.Size(379, 13)
    Me.pnlAccountViaSatTitle.TabIndex = 0
    '
    'lblAccountViaSatTitle
    '
    Me.lblAccountViaSatTitle.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAccountViaSatTitle.AutoSize = True
    Me.lblAccountViaSatTitle.Location = New System.Drawing.Point(3, 0)
    Me.lblAccountViaSatTitle.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblAccountViaSatTitle.Name = "lblAccountViaSatTitle"
    Me.lblAccountViaSatTitle.Size = New System.Drawing.Size(81, 13)
    Me.lblAccountViaSatTitle.TabIndex = 0
    Me.lblAccountViaSatTitle.Text = "ViaSat Account"
    '
    'pnlAccountViaSat
    '
    Me.pnlAccountViaSat.AutoSize = True
    Me.pnlAccountViaSat.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlAccountViaSat.ColumnCount = 2
    Me.pnlAccountViaSat.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAccountViaSat.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAccountViaSat.Controls.Add(Me.pctAccountViaSatIcon, 0, 0)
    Me.pnlAccountViaSat.Controls.Add(Me.pnlAccountViaSatInput, 1, 0)
    Me.pnlAccountViaSat.Location = New System.Drawing.Point(3, 31)
    Me.pnlAccountViaSat.Name = "pnlAccountViaSat"
    Me.pnlAccountViaSat.RowCount = 1
    Me.pnlAccountViaSat.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAccountViaSat.Size = New System.Drawing.Size(358, 71)
    Me.pnlAccountViaSat.TabIndex = 1
    '
    'pctAccountViaSatIcon
    '
    Me.pctAccountViaSatIcon.Anchor = System.Windows.Forms.AnchorStyles.Top
    Me.pctAccountViaSatIcon.Image = Global.RestrictionTracker.My.Resources.Resources.account_user
    Me.pctAccountViaSatIcon.Location = New System.Drawing.Point(21, 3)
    Me.pctAccountViaSatIcon.Margin = New System.Windows.Forms.Padding(21, 3, 3, 3)
    Me.pctAccountViaSatIcon.Name = "pctAccountViaSatIcon"
    Me.pctAccountViaSatIcon.Size = New System.Drawing.Size(32, 32)
    Me.pctAccountViaSatIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pctAccountViaSatIcon.TabIndex = 0
    Me.pctAccountViaSatIcon.TabStop = False
    '
    'pnlAccountViaSatInput
    '
    Me.pnlAccountViaSatInput.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlAccountViaSatInput.AutoSize = True
    Me.pnlAccountViaSatInput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlAccountViaSatInput.ColumnCount = 3
    Me.pnlAccountViaSatInput.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAccountViaSatInput.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAccountViaSatInput.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAccountViaSatInput.Controls.Add(Me.lblAccount, 0, 1)
    Me.pnlAccountViaSatInput.Controls.Add(Me.lblPassword, 0, 2)
    Me.pnlAccountViaSatInput.Controls.Add(Me.txtAccount, 1, 1)
    Me.pnlAccountViaSatInput.Controls.Add(Me.txtPassword, 1, 2)
    Me.pnlAccountViaSatInput.Controls.Add(Me.lblAccountViaSatDescription, 0, 0)
    Me.pnlAccountViaSatInput.Location = New System.Drawing.Point(56, 0)
    Me.pnlAccountViaSatInput.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlAccountViaSatInput.Name = "pnlAccountViaSatInput"
    Me.pnlAccountViaSatInput.RowCount = 3
    Me.pnlAccountViaSatInput.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAccountViaSatInput.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlAccountViaSatInput.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlAccountViaSatInput.Size = New System.Drawing.Size(302, 71)
    Me.pnlAccountViaSatInput.TabIndex = 1
    '
    'lblAccount
    '
    Me.lblAccount.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAccount.AutoSize = True
    Me.lblAccount.Location = New System.Drawing.Point(3, 25)
    Me.lblAccount.Name = "lblAccount"
    Me.lblAccount.Size = New System.Drawing.Size(58, 13)
    Me.lblAccount.TabIndex = 1
    Me.lblAccount.Text = "&Username:"
    '
    'lblPassword
    '
    Me.lblPassword.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblPassword.AutoSize = True
    Me.lblPassword.Location = New System.Drawing.Point(3, 51)
    Me.lblPassword.Name = "lblPassword"
    Me.lblPassword.Size = New System.Drawing.Size(56, 13)
    Me.lblPassword.TabIndex = 3
    Me.lblPassword.Text = "&Password:"
    '
    'txtAccount
    '
    Me.txtAccount.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.txtAccount.Location = New System.Drawing.Point(67, 22)
    Me.txtAccount.Name = "txtAccount"
    Me.txtAccount.Size = New System.Drawing.Size(150, 20)
    Me.txtAccount.TabIndex = 2
    Me.ttConfig.SetToolTip(Me.txtAccount, "Your ViaSat Username." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "If you were provided with an E-Mail address, this is the f" & _
        "irst half of that address.")
    '
    'lblAccountViaSatDescription
    '
    Me.lblAccountViaSatDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblAccountViaSatDescription.AutoSize = True
    Me.pnlAccountViaSatInput.SetColumnSpan(Me.lblAccountViaSatDescription, 3)
    Me.lblAccountViaSatDescription.Location = New System.Drawing.Point(3, 3)
    Me.lblAccountViaSatDescription.Margin = New System.Windows.Forms.Padding(3)
    Me.lblAccountViaSatDescription.Name = "lblAccountViaSatDescription"
    Me.lblAccountViaSatDescription.Size = New System.Drawing.Size(296, 13)
    Me.lblAccountViaSatDescription.TabIndex = 0
    Me.lblAccountViaSatDescription.Text = "This account information should match your meter page login."
    '
    'lblAccountShareWarning1
    '
    Me.lblAccountShareWarning1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblAccountShareWarning1.AutoSize = True
    Me.lblAccountShareWarning1.Location = New System.Drawing.Point(3, 380)
    Me.lblAccountShareWarning1.Margin = New System.Windows.Forms.Padding(3)
    Me.lblAccountShareWarning1.Name = "lblAccountShareWarning1"
    Me.lblAccountShareWarning1.Size = New System.Drawing.Size(379, 26)
    Me.lblAccountShareWarning1.TabIndex = 3
    Me.lblAccountShareWarning1.Text = "Do not enable this feature unless there is a connection problem." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Your username a" & _
    "nd password will be sent to RealityRipple Software."
    '
    'chkAccountShare
    '
    Me.chkAccountShare.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.chkAccountShare.AutoSize = True
    Me.chkAccountShare.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkAccountShare.Location = New System.Drawing.Point(56, 412)
    Me.chkAccountShare.Name = "chkAccountShare"
    Me.chkAccountShare.Size = New System.Drawing.Size(273, 18)
    Me.chkAccountShare.TabIndex = 4
    Me.chkAccountShare.Text = "Share Account Information for Diagnostic Purposes"
    Me.ttConfig.SetToolTip(Me.chkAccountShare, "Your Username and Password will be encrypted and sent to RealityRipple Software. " & _
        "Your account will not be shared with anyone else under any circumstances.")
    Me.chkAccountShare.UseVisualStyleBackColor = True
    '
    'lblAccountShareWarning2
    '
    Me.lblAccountShareWarning2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblAccountShareWarning2.AutoSize = True
    Me.lblAccountShareWarning2.Location = New System.Drawing.Point(3, 436)
    Me.lblAccountShareWarning2.Margin = New System.Windows.Forms.Padding(3)
    Me.lblAccountShareWarning2.Name = "lblAccountShareWarning2"
    Me.lblAccountShareWarning2.Size = New System.Drawing.Size(379, 38)
    Me.lblAccountShareWarning2.TabIndex = 5
    Me.lblAccountShareWarning2.Text = "If your problem is resolved, be sure to uncheck this checkbox, and then CHANGE YO" & _
    "UR PASSWORD."
    Me.lblAccountShareWarning2.Visible = False
    '
    'pnlAccountShareTitle
    '
    Me.pnlAccountShareTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlAccountShareTitle.AutoSize = True
    Me.pnlAccountShareTitle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlAccountShareTitle.ColumnCount = 2
    Me.pnlAccountShareTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAccountShareTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAccountShareTitle.Controls.Add(Me.lblAccountShareTitle, 0, 0)
    Me.pnlAccountShareTitle.Controls.Add(Me.lnAccountShareTitle, 1, 0)
    Me.pnlAccountShareTitle.Location = New System.Drawing.Point(3, 359)
    Me.pnlAccountShareTitle.Margin = New System.Windows.Forms.Padding(3, 10, 3, 5)
    Me.pnlAccountShareTitle.Name = "pnlAccountShareTitle"
    Me.pnlAccountShareTitle.RowCount = 1
    Me.pnlAccountShareTitle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAccountShareTitle.Size = New System.Drawing.Size(379, 13)
    Me.pnlAccountShareTitle.TabIndex = 6
    '
    'lblAccountShareTitle
    '
    Me.lblAccountShareTitle.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAccountShareTitle.AutoSize = True
    Me.lblAccountShareTitle.Location = New System.Drawing.Point(3, 0)
    Me.lblAccountShareTitle.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblAccountShareTitle.Name = "lblAccountShareTitle"
    Me.lblAccountShareTitle.Size = New System.Drawing.Size(188, 13)
    Me.lblAccountShareTitle.TabIndex = 0
    Me.lblAccountShareTitle.Text = "Contribute to Help with Login Changes"
    '
    'tabPrefs
    '
    Me.tabPrefs.Controls.Add(Me.pnlPrefs)
    Me.tabPrefs.Location = New System.Drawing.Point(4, 22)
    Me.tabPrefs.Name = "tabPrefs"
    Me.tabPrefs.Size = New System.Drawing.Size(385, 477)
    Me.tabPrefs.TabIndex = 1
    Me.tabPrefs.Text = "Preferences"
    Me.tabPrefs.UseVisualStyleBackColor = True
    '
    'pnlPrefs
    '
    Me.pnlPrefs.ColumnCount = 1
    Me.pnlPrefs.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPrefs.Controls.Add(Me.pnlPrefInterfaceTitle, 0, 9)
    Me.pnlPrefs.Controls.Add(Me.pnlPrefColor, 0, 13)
    Me.pnlPrefs.Controls.Add(Me.pnlPrefAlert, 0, 7)
    Me.pnlPrefs.Controls.Add(Me.pnlPrefAccuracy, 0, 4)
    Me.pnlPrefs.Controls.Add(Me.pnlPrefStartTitle, 0, 0)
    Me.pnlPrefs.Controls.Add(Me.pnlPrefAccuracyTitle, 0, 3)
    Me.pnlPrefs.Controls.Add(Me.pnlPrefAlertTitle, 0, 6)
    Me.pnlPrefs.Controls.Add(Me.pnlPrefColorTitle, 0, 12)
    Me.pnlPrefs.Controls.Add(Me.pnlPrefStart, 0, 1)
    Me.pnlPrefs.Controls.Add(Me.pnlPrefInterface, 0, 10)
    Me.pnlPrefs.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlPrefs.Location = New System.Drawing.Point(0, 0)
    Me.pnlPrefs.Name = "pnlPrefs"
    Me.pnlPrefs.RowCount = 14
    Me.pnlPrefs.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefs.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefs.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00145!))
    Me.pnlPrefs.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefs.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefs.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00145!))
    Me.pnlPrefs.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefs.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefs.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99896!))
    Me.pnlPrefs.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefs.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefs.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99813!))
    Me.pnlPrefs.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefs.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefs.Size = New System.Drawing.Size(385, 477)
    Me.pnlPrefs.TabIndex = 0
    '
    'pnlPrefInterfaceTitle
    '
    Me.pnlPrefInterfaceTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlPrefInterfaceTitle.AutoSize = True
    Me.pnlPrefInterfaceTitle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlPrefInterfaceTitle.ColumnCount = 2
    Me.pnlPrefInterfaceTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefInterfaceTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPrefInterfaceTitle.Controls.Add(Me.lblPrefInterfaceTitle, 0, 0)
    Me.pnlPrefInterfaceTitle.Controls.Add(Me.lnPrefInterfaceTitle, 1, 0)
    Me.pnlPrefInterfaceTitle.Location = New System.Drawing.Point(3, 317)
    Me.pnlPrefInterfaceTitle.Margin = New System.Windows.Forms.Padding(3, 10, 3, 5)
    Me.pnlPrefInterfaceTitle.Name = "pnlPrefInterfaceTitle"
    Me.pnlPrefInterfaceTitle.RowCount = 1
    Me.pnlPrefInterfaceTitle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPrefInterfaceTitle.Size = New System.Drawing.Size(379, 13)
    Me.pnlPrefInterfaceTitle.TabIndex = 6
    '
    'lblPrefInterfaceTitle
    '
    Me.lblPrefInterfaceTitle.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblPrefInterfaceTitle.AutoSize = True
    Me.lblPrefInterfaceTitle.Location = New System.Drawing.Point(3, 0)
    Me.lblPrefInterfaceTitle.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblPrefInterfaceTitle.Name = "lblPrefInterfaceTitle"
    Me.lblPrefInterfaceTitle.Size = New System.Drawing.Size(156, 13)
    Me.lblPrefInterfaceTitle.TabIndex = 0
    Me.lblPrefInterfaceTitle.Text = "Main Window Interface Options"
    '
    'pnlPrefColor
    '
    Me.pnlPrefColor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlPrefColor.AutoSize = True
    Me.pnlPrefColor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlPrefColor.ColumnCount = 3
    Me.pnlPrefColor.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefColor.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPrefColor.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefColor.Controls.Add(Me.pctPrefColorIcon, 0, 0)
    Me.pnlPrefColor.Controls.Add(Me.lblPrefColorDescription, 1, 0)
    Me.pnlPrefColor.Controls.Add(Me.cmdColors, 2, 0)
    Me.pnlPrefColor.Location = New System.Drawing.Point(3, 430)
    Me.pnlPrefColor.Name = "pnlPrefColor"
    Me.pnlPrefColor.RowCount = 1
    Me.pnlPrefColor.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefColor.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45.0!))
    Me.pnlPrefColor.Size = New System.Drawing.Size(379, 45)
    Me.pnlPrefColor.TabIndex = 9
    '
    'pctPrefColorIcon
    '
    Me.pctPrefColorIcon.Anchor = System.Windows.Forms.AnchorStyles.Top
    Me.pctPrefColorIcon.Image = Global.RestrictionTracker.My.Resources.Resources.prefs_colors
    Me.pctPrefColorIcon.Location = New System.Drawing.Point(21, 3)
    Me.pctPrefColorIcon.Margin = New System.Windows.Forms.Padding(21, 3, 3, 3)
    Me.pctPrefColorIcon.Name = "pctPrefColorIcon"
    Me.pctPrefColorIcon.Size = New System.Drawing.Size(32, 32)
    Me.pctPrefColorIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pctPrefColorIcon.TabIndex = 2
    Me.pctPrefColorIcon.TabStop = False
    '
    'lblPrefColorDescription
    '
    Me.lblPrefColorDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblPrefColorDescription.AutoSize = True
    Me.lblPrefColorDescription.Location = New System.Drawing.Point(59, 3)
    Me.lblPrefColorDescription.Margin = New System.Windows.Forms.Padding(3)
    Me.lblPrefColorDescription.Name = "lblPrefColorDescription"
    Me.lblPrefColorDescription.Size = New System.Drawing.Size(194, 39)
    Me.lblPrefColorDescription.TabIndex = 0
    Me.lblPrefColorDescription.Text = "Change the colors of the Graphs in the Main Window, the History Window and the Tr" & _
    "ay Icon."
    '
    'cmdColors
    '
    Me.cmdColors.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdColors.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdColors.Location = New System.Drawing.Point(259, 12)
    Me.cmdColors.Margin = New System.Windows.Forms.Padding(3, 3, 10, 3)
    Me.cmdColors.Name = "cmdColors"
    Me.cmdColors.Size = New System.Drawing.Size(110, 21)
    Me.cmdColors.TabIndex = 1
    Me.cmdColors.Text = "Customize &Colors"
    Me.ttConfig.SetToolTip(Me.cmdColors, "Change the colors of the Main Window, Tray Icon, and History Graphs.")
    Me.cmdColors.UseVisualStyleBackColor = True
    '
    'pnlPrefAlert
    '
    Me.pnlPrefAlert.AutoSize = True
    Me.pnlPrefAlert.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlPrefAlert.ColumnCount = 5
    Me.pnlPrefAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefAlert.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefAlert.Controls.Add(Me.pctPrefAlertIcon, 0, 0)
    Me.pnlPrefAlert.Controls.Add(Me.lblPrefAlertDescription, 1, 0)
    Me.pnlPrefAlert.Controls.Add(Me.chkOverAlert, 1, 1)
    Me.pnlPrefAlert.Controls.Add(Me.lblOverTime1, 1, 3)
    Me.pnlPrefAlert.Controls.Add(Me.txtOverTime, 2, 3)
    Me.pnlPrefAlert.Controls.Add(Me.lblOverSize2, 3, 2)
    Me.pnlPrefAlert.Controls.Add(Me.txtOverSize, 2, 2)
    Me.pnlPrefAlert.Controls.Add(Me.lblOverTime2, 3, 3)
    Me.pnlPrefAlert.Controls.Add(Me.lblOverSize1, 1, 2)
    Me.pnlPrefAlert.Controls.Add(Me.cmdAlertStyle, 3, 1)
    Me.pnlPrefAlert.Location = New System.Drawing.Point(3, 221)
    Me.pnlPrefAlert.Name = "pnlPrefAlert"
    Me.pnlPrefAlert.RowCount = 4
    Me.pnlPrefAlert.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefAlert.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefAlert.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefAlert.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefAlert.Size = New System.Drawing.Size(364, 97)
    Me.pnlPrefAlert.TabIndex = 5
    '
    'pctPrefAlertIcon
    '
    Me.pctPrefAlertIcon.Anchor = System.Windows.Forms.AnchorStyles.Top
    Me.pctPrefAlertIcon.Image = Global.RestrictionTracker.My.Resources.Resources.prefs_notify
    Me.pctPrefAlertIcon.Location = New System.Drawing.Point(21, 3)
    Me.pctPrefAlertIcon.Margin = New System.Windows.Forms.Padding(21, 3, 3, 3)
    Me.pctPrefAlertIcon.Name = "pctPrefAlertIcon"
    Me.pnlPrefAlert.SetRowSpan(Me.pctPrefAlertIcon, 2)
    Me.pctPrefAlertIcon.Size = New System.Drawing.Size(31, 31)
    Me.pctPrefAlertIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pctPrefAlertIcon.TabIndex = 2
    Me.pctPrefAlertIcon.TabStop = False
    '
    'lblPrefAlertDescription
    '
    Me.lblPrefAlertDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblPrefAlertDescription.AutoSize = True
    Me.pnlPrefAlert.SetColumnSpan(Me.lblPrefAlertDescription, 4)
    Me.lblPrefAlertDescription.Location = New System.Drawing.Point(58, 3)
    Me.lblPrefAlertDescription.Margin = New System.Windows.Forms.Padding(3)
    Me.lblPrefAlertDescription.Name = "lblPrefAlertDescription"
    Me.lblPrefAlertDescription.Size = New System.Drawing.Size(303, 13)
    Me.lblPrefAlertDescription.TabIndex = 0
    Me.lblPrefAlertDescription.Text = "Satellite Restriction Tracker can notify you of excessive usage."
    '
    'chkOverAlert
    '
    Me.chkOverAlert.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkOverAlert.AutoSize = True
    Me.pnlPrefAlert.SetColumnSpan(Me.chkOverAlert, 2)
    Me.chkOverAlert.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkOverAlert.Location = New System.Drawing.Point(58, 23)
    Me.chkOverAlert.Name = "chkOverAlert"
    Me.chkOverAlert.Size = New System.Drawing.Size(129, 18)
    Me.chkOverAlert.TabIndex = 1
    Me.chkOverAlert.Text = "Display Al&ert window"
    Me.ttConfig.SetToolTip(Me.chkOverAlert, "Check this box to display a balloon alert when you use too much of your allocated" & _
        " usage.")
    Me.chkOverAlert.UseVisualStyleBackColor = False
    '
    'lblOverTime1
    '
    Me.lblOverTime1.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblOverTime1.AutoSize = True
    Me.lblOverTime1.Location = New System.Drawing.Point(135, 77)
    Me.lblOverTime1.Name = "lblOverTime1"
    Me.lblOverTime1.Size = New System.Drawing.Size(34, 13)
    Me.lblOverTime1.TabIndex = 6
    Me.lblOverTime1.Text = "w&ithin"
    '
    'lblOverSize2
    '
    Me.lblOverSize2.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblOverSize2.AutoSize = True
    Me.lblOverSize2.Location = New System.Drawing.Point(231, 51)
    Me.lblOverSize2.Name = "lblOverSize2"
    Me.lblOverSize2.Size = New System.Drawing.Size(23, 13)
    Me.lblOverSize2.TabIndex = 5
    Me.lblOverSize2.Text = "MB"
    '
    'lblOverTime2
    '
    Me.lblOverTime2.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblOverTime2.AutoSize = True
    Me.lblOverTime2.Location = New System.Drawing.Point(231, 77)
    Me.lblOverTime2.Name = "lblOverTime2"
    Me.lblOverTime2.Size = New System.Drawing.Size(43, 13)
    Me.lblOverTime2.TabIndex = 8
    Me.lblOverTime2.Text = "minutes"
    '
    'lblOverSize1
    '
    Me.lblOverSize1.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.lblOverSize1.AutoSize = True
    Me.lblOverSize1.Location = New System.Drawing.Point(75, 51)
    Me.lblOverSize1.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
    Me.lblOverSize1.Name = "lblOverSize1"
    Me.lblOverSize1.Size = New System.Drawing.Size(94, 13)
    Me.lblOverSize1.TabIndex = 3
    Me.lblOverSize1.Text = "if usage goes &over"
    '
    'cmdAlertStyle
    '
    Me.cmdAlertStyle.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.pnlPrefAlert.SetColumnSpan(Me.cmdAlertStyle, 2)
    Me.cmdAlertStyle.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdAlertStyle.Location = New System.Drawing.Point(244, 22)
    Me.cmdAlertStyle.Margin = New System.Windows.Forms.Padding(3, 3, 10, 3)
    Me.cmdAlertStyle.Name = "cmdAlertStyle"
    Me.cmdAlertStyle.Size = New System.Drawing.Size(110, 20)
    Me.cmdAlertStyle.TabIndex = 2
    Me.cmdAlertStyle.Text = "Set Alert &Style"
    Me.ttConfig.SetToolTip(Me.cmdAlertStyle, "Choose a style for the Alert Windows used for Usage Alerts and Parse Failures.")
    Me.cmdAlertStyle.UseVisualStyleBackColor = True
    '
    'pnlPrefAccuracy
    '
    Me.pnlPrefAccuracy.AutoSize = True
    Me.pnlPrefAccuracy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlPrefAccuracy.ColumnCount = 2
    Me.pnlPrefAccuracy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefAccuracy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPrefAccuracy.Controls.Add(Me.pctPrefAccuracyIcon, 0, 0)
    Me.pnlPrefAccuracy.Controls.Add(Me.pnlPrefAccuracyInput, 1, 0)
    Me.pnlPrefAccuracy.Location = New System.Drawing.Point(3, 149)
    Me.pnlPrefAccuracy.Name = "pnlPrefAccuracy"
    Me.pnlPrefAccuracy.RowCount = 1
    Me.pnlPrefAccuracy.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefAccuracy.Size = New System.Drawing.Size(344, 52)
    Me.pnlPrefAccuracy.TabIndex = 3
    '
    'pctPrefAccuracyIcon
    '
    Me.pctPrefAccuracyIcon.Anchor = System.Windows.Forms.AnchorStyles.Top
    Me.pctPrefAccuracyIcon.Image = Global.RestrictionTracker.My.Resources.Resources.prefs_accuracy
    Me.pctPrefAccuracyIcon.Location = New System.Drawing.Point(21, 3)
    Me.pctPrefAccuracyIcon.Margin = New System.Windows.Forms.Padding(21, 3, 3, 3)
    Me.pctPrefAccuracyIcon.Name = "pctPrefAccuracyIcon"
    Me.pctPrefAccuracyIcon.Size = New System.Drawing.Size(32, 32)
    Me.pctPrefAccuracyIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pctPrefAccuracyIcon.TabIndex = 1
    Me.pctPrefAccuracyIcon.TabStop = False
    '
    'pnlPrefAccuracyInput
    '
    Me.pnlPrefAccuracyInput.AutoSize = True
    Me.pnlPrefAccuracyInput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlPrefAccuracyInput.ColumnCount = 3
    Me.pnlPrefAccuracyInput.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefAccuracyInput.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefAccuracyInput.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefAccuracyInput.Controls.Add(Me.lblInterval1, 0, 0)
    Me.pnlPrefAccuracyInput.Controls.Add(Me.txtInterval, 1, 0)
    Me.pnlPrefAccuracyInput.Controls.Add(Me.lblInterval2, 2, 0)
    Me.pnlPrefAccuracyInput.Controls.Add(Me.lblAccuracy1, 0, 1)
    Me.pnlPrefAccuracyInput.Controls.Add(Me.txtAccuracy, 1, 1)
    Me.pnlPrefAccuracyInput.Controls.Add(Me.lblAccuracy2, 2, 1)
    Me.pnlPrefAccuracyInput.Location = New System.Drawing.Point(56, 0)
    Me.pnlPrefAccuracyInput.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlPrefAccuracyInput.Name = "pnlPrefAccuracyInput"
    Me.pnlPrefAccuracyInput.RowCount = 2
    Me.pnlPrefAccuracyInput.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefAccuracyInput.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefAccuracyInput.Size = New System.Drawing.Size(288, 52)
    Me.pnlPrefAccuracyInput.TabIndex = 0
    '
    'lblInterval1
    '
    Me.lblInterval1.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblInterval1.AutoSize = True
    Me.lblInterval1.Location = New System.Drawing.Point(3, 6)
    Me.lblInterval1.Name = "lblInterval1"
    Me.lblInterval1.Size = New System.Drawing.Size(29, 13)
    Me.lblInterval1.TabIndex = 0
    Me.lblInterval1.Text = "W&ait"
    '
    'lblInterval2
    '
    Me.lblInterval2.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblInterval2.AutoSize = True
    Me.lblInterval2.Location = New System.Drawing.Point(106, 6)
    Me.lblInterval2.Name = "lblInterval2"
    Me.lblInterval2.Size = New System.Drawing.Size(179, 13)
    Me.lblInterval2.TabIndex = 2
    Me.lblInterval2.Text = "minutes between each usage check"
    '
    'lblAccuracy1
    '
    Me.lblAccuracy1.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAccuracy1.AutoSize = True
    Me.lblAccuracy1.Location = New System.Drawing.Point(3, 32)
    Me.lblAccuracy1.Name = "lblAccuracy1"
    Me.lblAccuracy1.Size = New System.Drawing.Size(41, 13)
    Me.lblAccuracy1.TabIndex = 3
    Me.lblAccuracy1.Text = "&Display"
    '
    'lblAccuracy2
    '
    Me.lblAccuracy2.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAccuracy2.AutoSize = True
    Me.lblAccuracy2.Location = New System.Drawing.Point(106, 32)
    Me.lblAccuracy2.Name = "lblAccuracy2"
    Me.lblAccuracy2.Size = New System.Drawing.Size(138, 13)
    Me.lblAccuracy2.TabIndex = 5
    Me.lblAccuracy2.Text = "digits after the decimal point"
    '
    'pnlPrefStartTitle
    '
    Me.pnlPrefStartTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlPrefStartTitle.AutoSize = True
    Me.pnlPrefStartTitle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlPrefStartTitle.ColumnCount = 2
    Me.pnlPrefStartTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefStartTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPrefStartTitle.Controls.Add(Me.lblPrefStartTitle, 0, 0)
    Me.pnlPrefStartTitle.Controls.Add(Me.lnPrefStartTitle, 1, 0)
    Me.pnlPrefStartTitle.Location = New System.Drawing.Point(3, 10)
    Me.pnlPrefStartTitle.Margin = New System.Windows.Forms.Padding(3, 10, 3, 5)
    Me.pnlPrefStartTitle.Name = "pnlPrefStartTitle"
    Me.pnlPrefStartTitle.RowCount = 1
    Me.pnlPrefStartTitle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPrefStartTitle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13.0!))
    Me.pnlPrefStartTitle.Size = New System.Drawing.Size(379, 13)
    Me.pnlPrefStartTitle.TabIndex = 0
    '
    'lblPrefStartTitle
    '
    Me.lblPrefStartTitle.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblPrefStartTitle.AutoSize = True
    Me.lblPrefStartTitle.Location = New System.Drawing.Point(3, 0)
    Me.lblPrefStartTitle.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblPrefStartTitle.Name = "lblPrefStartTitle"
    Me.lblPrefStartTitle.Size = New System.Drawing.Size(109, 13)
    Me.lblPrefStartTitle.TabIndex = 0
    Me.lblPrefStartTitle.Text = "Starting and Stopping"
    '
    'pnlPrefAccuracyTitle
    '
    Me.pnlPrefAccuracyTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlPrefAccuracyTitle.AutoSize = True
    Me.pnlPrefAccuracyTitle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlPrefAccuracyTitle.ColumnCount = 2
    Me.pnlPrefAccuracyTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefAccuracyTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPrefAccuracyTitle.Controls.Add(Me.lblPrefAccuracyTitle, 0, 0)
    Me.pnlPrefAccuracyTitle.Controls.Add(Me.lnPrefAccuracyTitle, 1, 0)
    Me.pnlPrefAccuracyTitle.Location = New System.Drawing.Point(3, 128)
    Me.pnlPrefAccuracyTitle.Margin = New System.Windows.Forms.Padding(3, 10, 3, 5)
    Me.pnlPrefAccuracyTitle.Name = "pnlPrefAccuracyTitle"
    Me.pnlPrefAccuracyTitle.RowCount = 1
    Me.pnlPrefAccuracyTitle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPrefAccuracyTitle.Size = New System.Drawing.Size(379, 13)
    Me.pnlPrefAccuracyTitle.TabIndex = 2
    '
    'lblPrefAccuracyTitle
    '
    Me.lblPrefAccuracyTitle.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblPrefAccuracyTitle.AutoSize = True
    Me.lblPrefAccuracyTitle.Location = New System.Drawing.Point(3, 0)
    Me.lblPrefAccuracyTitle.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblPrefAccuracyTitle.Name = "lblPrefAccuracyTitle"
    Me.lblPrefAccuracyTitle.Size = New System.Drawing.Size(86, 13)
    Me.lblPrefAccuracyTitle.TabIndex = 0
    Me.lblPrefAccuracyTitle.Text = "Usage Accuracy"
    '
    'pnlPrefAlertTitle
    '
    Me.pnlPrefAlertTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlPrefAlertTitle.AutoSize = True
    Me.pnlPrefAlertTitle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlPrefAlertTitle.ColumnCount = 2
    Me.pnlPrefAlertTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefAlertTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPrefAlertTitle.Controls.Add(Me.lblPrefAlertTitle, 0, 0)
    Me.pnlPrefAlertTitle.Controls.Add(Me.lnPrefAlertTitle, 1, 0)
    Me.pnlPrefAlertTitle.Location = New System.Drawing.Point(3, 200)
    Me.pnlPrefAlertTitle.Margin = New System.Windows.Forms.Padding(3, 10, 3, 5)
    Me.pnlPrefAlertTitle.Name = "pnlPrefAlertTitle"
    Me.pnlPrefAlertTitle.RowCount = 1
    Me.pnlPrefAlertTitle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPrefAlertTitle.Size = New System.Drawing.Size(379, 13)
    Me.pnlPrefAlertTitle.TabIndex = 4
    '
    'lblPrefAlertTitle
    '
    Me.lblPrefAlertTitle.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblPrefAlertTitle.AutoSize = True
    Me.lblPrefAlertTitle.Location = New System.Drawing.Point(3, 0)
    Me.lblPrefAlertTitle.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblPrefAlertTitle.Name = "lblPrefAlertTitle"
    Me.lblPrefAlertTitle.Size = New System.Drawing.Size(62, 13)
    Me.lblPrefAlertTitle.TabIndex = 0
    Me.lblPrefAlertTitle.Text = "Usage Alert"
    '
    'pnlPrefColorTitle
    '
    Me.pnlPrefColorTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlPrefColorTitle.AutoSize = True
    Me.pnlPrefColorTitle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlPrefColorTitle.ColumnCount = 2
    Me.pnlPrefColorTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefColorTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPrefColorTitle.Controls.Add(Me.lblPrefColorTitle, 0, 0)
    Me.pnlPrefColorTitle.Controls.Add(Me.lnPrefColorTitle, 1, 0)
    Me.pnlPrefColorTitle.Location = New System.Drawing.Point(3, 409)
    Me.pnlPrefColorTitle.Margin = New System.Windows.Forms.Padding(3, 10, 3, 5)
    Me.pnlPrefColorTitle.Name = "pnlPrefColorTitle"
    Me.pnlPrefColorTitle.RowCount = 1
    Me.pnlPrefColorTitle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPrefColorTitle.Size = New System.Drawing.Size(379, 13)
    Me.pnlPrefColorTitle.TabIndex = 8
    '
    'lblPrefColorTitle
    '
    Me.lblPrefColorTitle.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblPrefColorTitle.AutoSize = True
    Me.lblPrefColorTitle.Location = New System.Drawing.Point(3, 0)
    Me.lblPrefColorTitle.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblPrefColorTitle.Name = "lblPrefColorTitle"
    Me.lblPrefColorTitle.Size = New System.Drawing.Size(68, 13)
    Me.lblPrefColorTitle.TabIndex = 0
    Me.lblPrefColorTitle.Text = "Graph Colors"
    '
    'pnlPrefStart
    '
    Me.pnlPrefStart.AutoSize = True
    Me.pnlPrefStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlPrefStart.ColumnCount = 2
    Me.pnlPrefStart.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefStart.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPrefStart.Controls.Add(Me.pctPrefStartIcon, 0, 0)
    Me.pnlPrefStart.Controls.Add(Me.pnlPrefStartInput, 1, 0)
    Me.pnlPrefStart.Location = New System.Drawing.Point(3, 31)
    Me.pnlPrefStart.Name = "pnlPrefStart"
    Me.pnlPrefStart.RowCount = 1
    Me.pnlPrefStart.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefStart.Size = New System.Drawing.Size(379, 98)
    Me.pnlPrefStart.TabIndex = 1
    '
    'pctPrefStartIcon
    '
    Me.pctPrefStartIcon.Anchor = System.Windows.Forms.AnchorStyles.Top
    Me.pctPrefStartIcon.Image = Global.RestrictionTracker.My.Resources.Resources.prefs_power
    Me.pctPrefStartIcon.Location = New System.Drawing.Point(21, 3)
    Me.pctPrefStartIcon.Margin = New System.Windows.Forms.Padding(21, 3, 3, 3)
    Me.pctPrefStartIcon.Name = "pctPrefStartIcon"
    Me.pctPrefStartIcon.Size = New System.Drawing.Size(32, 32)
    Me.pctPrefStartIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pctPrefStartIcon.TabIndex = 1
    Me.pctPrefStartIcon.TabStop = False
    '
    'pnlPrefStartInput
    '
    Me.pnlPrefStartInput.AutoSize = True
    Me.pnlPrefStartInput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlPrefStartInput.ColumnCount = 3
    Me.pnlPrefStartInput.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefStartInput.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefStartInput.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPrefStartInput.Controls.Add(Me.chkStartUp, 0, 0)
    Me.pnlPrefStartInput.Controls.Add(Me.lblStartWait1, 0, 1)
    Me.pnlPrefStartInput.Controls.Add(Me.txtStartWait, 1, 1)
    Me.pnlPrefStartInput.Controls.Add(Me.lblStartWait2, 2, 1)
    Me.pnlPrefStartInput.Controls.Add(Me.chkService, 0, 2)
    Me.pnlPrefStartInput.Controls.Add(Me.chkAutoHide, 0, 3)
    Me.pnlPrefStartInput.Location = New System.Drawing.Point(56, 0)
    Me.pnlPrefStartInput.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlPrefStartInput.Name = "pnlPrefStartInput"
    Me.pnlPrefStartInput.RowCount = 4
    Me.pnlPrefStartInput.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefStartInput.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefStartInput.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefStartInput.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefStartInput.Size = New System.Drawing.Size(323, 98)
    Me.pnlPrefStartInput.TabIndex = 0
    '
    'chkStartUp
    '
    Me.chkStartUp.AutoSize = True
    Me.pnlPrefStartInput.SetColumnSpan(Me.chkStartUp, 3)
    Me.chkStartUp.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkStartUp.Location = New System.Drawing.Point(3, 3)
    Me.chkStartUp.Name = "chkStartUp"
    Me.chkStartUp.Size = New System.Drawing.Size(270, 18)
    Me.chkStartUp.TabIndex = 0
    Me.chkStartUp.Text = "&Run Satellite Restriction Tracker on system startup"
    Me.ttConfig.SetToolTip(Me.chkStartUp, "Start Satellite Restriction Tracker with this Windows account.")
    Me.chkStartUp.UseVisualStyleBackColor = False
    '
    'lblStartWait1
    '
    Me.lblStartWait1.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblStartWait1.AutoSize = True
    Me.lblStartWait1.Location = New System.Drawing.Point(3, 30)
    Me.lblStartWait1.Margin = New System.Windows.Forms.Padding(3)
    Me.lblStartWait1.Name = "lblStartWait1"
    Me.lblStartWait1.Size = New System.Drawing.Size(29, 13)
    Me.lblStartWait1.TabIndex = 1
    Me.lblStartWait1.Text = "&Wait"
    '
    'lblStartWait2
    '
    Me.lblStartWait2.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblStartWait2.AutoSize = True
    Me.lblStartWait2.Location = New System.Drawing.Point(94, 30)
    Me.lblStartWait2.Margin = New System.Windows.Forms.Padding(3)
    Me.lblStartWait2.Name = "lblStartWait2"
    Me.lblStartWait2.Size = New System.Drawing.Size(210, 13)
    Me.lblStartWait2.TabIndex = 3
    Me.lblStartWait2.Text = "minutes before first usage check on startup"
    '
    'chkService
    '
    Me.chkService.AutoSize = True
    Me.pnlPrefStartInput.SetColumnSpan(Me.chkService, 3)
    Me.chkService.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkService.Location = New System.Drawing.Point(3, 53)
    Me.chkService.Name = "chkService"
    Me.chkService.Size = New System.Drawing.Size(317, 18)
    Me.chkService.TabIndex = 4
    Me.chkService.Text = "Run &Logger Service when Satellite Restriction Tracker closes"
    Me.ttConfig.SetToolTip(Me.chkService, "Run Satellite Restriction Logger system service when Satellite Restriction Tracke" & _
        "r is closed.")
    Me.chkService.UseVisualStyleBackColor = False
    '
    'chkAutoHide
    '
    Me.chkAutoHide.AutoSize = True
    Me.pnlPrefStartInput.SetColumnSpan(Me.chkAutoHide, 3)
    Me.chkAutoHide.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkAutoHide.Location = New System.Drawing.Point(3, 77)
    Me.chkAutoHide.Name = "chkAutoHide"
    Me.chkAutoHide.Size = New System.Drawing.Size(255, 18)
    Me.chkAutoHide.TabIndex = 5
    Me.chkAutoHide.Text = "&Minimize Satellite Restriction Tracker on startup"
    Me.ttConfig.SetToolTip(Me.chkAutoHide, "Automatically minimize the program when it starts.")
    Me.chkAutoHide.UseVisualStyleBackColor = False
    '
    'pnlPrefInterface
    '
    Me.pnlPrefInterface.AutoSize = True
    Me.pnlPrefInterface.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlPrefInterface.ColumnCount = 5
    Me.pnlPrefInterface.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefInterface.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18.0!))
    Me.pnlPrefInterface.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefInterface.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18.0!))
    Me.pnlPrefInterface.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPrefInterface.Controls.Add(Me.chkTrayIcon, 1, 1)
    Me.pnlPrefInterface.Controls.Add(Me.chkScaleScreen, 1, 0)
    Me.pnlPrefInterface.Controls.Add(Me.pctPrefInterfaceIcon, 0, 0)
    Me.pnlPrefInterface.Controls.Add(Me.chkTrayMin, 2, 2)
    Me.pnlPrefInterface.Controls.Add(Me.chkTrayAnim, 3, 2)
    Me.pnlPrefInterface.Controls.Add(Me.chkTrayClose, 3, 0)
    Me.pnlPrefInterface.Location = New System.Drawing.Point(3, 338)
    Me.pnlPrefInterface.Name = "pnlPrefInterface"
    Me.pnlPrefInterface.RowCount = 3
    Me.pnlPrefInterface.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefInterface.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefInterface.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlPrefInterface.Size = New System.Drawing.Size(379, 72)
    Me.pnlPrefInterface.TabIndex = 7
    '
    'chkTrayIcon
    '
    Me.chkTrayIcon.AutoSize = True
    Me.pnlPrefInterface.SetColumnSpan(Me.chkTrayIcon, 2)
    Me.chkTrayIcon.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkTrayIcon.Location = New System.Drawing.Point(59, 27)
    Me.chkTrayIcon.Name = "chkTrayIcon"
    Me.chkTrayIcon.Size = New System.Drawing.Size(137, 18)
    Me.chkTrayIcon.TabIndex = 1
    Me.chkTrayIcon.Text = "Show system &tray icon"
    Me.ttConfig.SetToolTip(Me.chkTrayIcon, "Display an icon in the system Notification Area.")
    Me.chkTrayIcon.UseVisualStyleBackColor = False
    '
    'chkScaleScreen
    '
    Me.chkScaleScreen.AutoSize = True
    Me.pnlPrefInterface.SetColumnSpan(Me.chkScaleScreen, 2)
    Me.chkScaleScreen.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkScaleScreen.Location = New System.Drawing.Point(59, 3)
    Me.chkScaleScreen.Name = "chkScaleScreen"
    Me.chkScaleScreen.Size = New System.Drawing.Size(151, 18)
    Me.chkScaleScreen.TabIndex = 0
    Me.chkScaleScreen.Text = "Scale text to window si&ze"
    Me.ttConfig.SetToolTip(Me.chkScaleScreen, "Text in the main window of Satellite Restriction Tracker will scale to fit its si" & _
        "ze.")
    Me.chkScaleScreen.UseVisualStyleBackColor = False
    '
    'pctPrefInterfaceIcon
    '
    Me.pctPrefInterfaceIcon.Anchor = System.Windows.Forms.AnchorStyles.Top
    Me.pctPrefInterfaceIcon.Image = Global.RestrictionTracker.My.Resources.Resources.prefs_interface
    Me.pctPrefInterfaceIcon.Location = New System.Drawing.Point(21, 3)
    Me.pctPrefInterfaceIcon.Margin = New System.Windows.Forms.Padding(21, 3, 3, 3)
    Me.pctPrefInterfaceIcon.Name = "pctPrefInterfaceIcon"
    Me.pnlPrefInterface.SetRowSpan(Me.pctPrefInterfaceIcon, 3)
    Me.pctPrefInterfaceIcon.Size = New System.Drawing.Size(32, 32)
    Me.pctPrefInterfaceIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pctPrefInterfaceIcon.TabIndex = 1
    Me.pctPrefInterfaceIcon.TabStop = False
    '
    'chkTrayMin
    '
    Me.chkTrayMin.AutoSize = True
    Me.chkTrayMin.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkTrayMin.Location = New System.Drawing.Point(77, 51)
    Me.chkTrayMin.Name = "chkTrayMin"
    Me.chkTrayMin.Size = New System.Drawing.Size(130, 18)
    Me.chkTrayMin.TabIndex = 2
    Me.chkTrayMin.Text = "O&nly when minimized"
    Me.ttConfig.SetToolTip(Me.chkTrayMin, "Display the tray icon only when Satellite Restriction Tracker is minimized.")
    Me.chkTrayMin.UseVisualStyleBackColor = False
    '
    'chkTrayAnim
    '
    Me.chkTrayAnim.AutoSize = True
    Me.pnlPrefInterface.SetColumnSpan(Me.chkTrayAnim, 2)
    Me.chkTrayAnim.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkTrayAnim.Location = New System.Drawing.Point(216, 51)
    Me.chkTrayAnim.Name = "chkTrayAnim"
    Me.chkTrayAnim.Size = New System.Drawing.Size(169, 18)
    Me.chkTrayAnim.TabIndex = 4
    Me.chkTrayAnim.Text = "Show tra&y minimize animation"
    Me.ttConfig.SetToolTip(Me.chkTrayAnim, "See the Main Window go to the System Tray instead of the default minimize animati" & _
        "on.")
    Me.chkTrayAnim.UseVisualStyleBackColor = True
    '
    'chkTrayClose
    '
    Me.chkTrayClose.AutoSize = True
    Me.pnlPrefInterface.SetColumnSpan(Me.chkTrayClose, 2)
    Me.chkTrayClose.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkTrayClose.Location = New System.Drawing.Point(216, 3)
    Me.chkTrayClose.Name = "chkTrayClose"
    Me.chkTrayClose.Size = New System.Drawing.Size(157, 18)
    Me.chkTrayClose.TabIndex = 3
    Me.chkTrayClose.Text = "Minimize instead of closin&g"
    Me.ttConfig.SetToolTip(Me.chkTrayClose, "Instead of closing the program, pressing the X button will minimize the program.")
    Me.chkTrayClose.UseVisualStyleBackColor = True
    '
    'tabNetwork
    '
    Me.tabNetwork.Controls.Add(Me.pnlNetwork)
    Me.tabNetwork.Location = New System.Drawing.Point(4, 22)
    Me.tabNetwork.Name = "tabNetwork"
    Me.tabNetwork.Size = New System.Drawing.Size(385, 477)
    Me.tabNetwork.TabIndex = 2
    Me.tabNetwork.Text = "Network"
    Me.tabNetwork.UseVisualStyleBackColor = True
    '
    'pnlNetwork
    '
    Me.pnlNetwork.ColumnCount = 1
    Me.pnlNetwork.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlNetwork.Controls.Add(Me.pnlNetworkProtocol, 0, 7)
    Me.pnlNetwork.Controls.Add(Me.pnlNetworkProtocolTitle, 0, 6)
    Me.pnlNetwork.Controls.Add(Me.pnlNetworkUpdate, 0, 10)
    Me.pnlNetwork.Controls.Add(Me.pnlNetworkProxy, 0, 4)
    Me.pnlNetwork.Controls.Add(Me.pnlNetworkProxyTitle, 0, 3)
    Me.pnlNetwork.Controls.Add(Me.pnlNetworkTimeoutTitle, 0, 0)
    Me.pnlNetwork.Controls.Add(Me.pnlNetworkTimeout, 0, 1)
    Me.pnlNetwork.Controls.Add(Me.pnlNetworkUpdateTitle, 0, 9)
    Me.pnlNetwork.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlNetwork.Location = New System.Drawing.Point(0, 0)
    Me.pnlNetwork.Name = "pnlNetwork"
    Me.pnlNetwork.RowCount = 11
    Me.pnlNetwork.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlNetwork.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlNetwork.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlNetwork.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlNetwork.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlNetwork.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlNetwork.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlNetwork.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlNetwork.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlNetwork.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlNetwork.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlNetwork.Size = New System.Drawing.Size(385, 477)
    Me.pnlNetwork.TabIndex = 0
    '
    'pnlNetworkProtocol
    '
    Me.pnlNetworkProtocol.AutoSize = True
    Me.pnlNetworkProtocol.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlNetworkProtocol.ColumnCount = 7
    Me.pnlNetworkProtocol.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkProtocol.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkProtocol.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlNetworkProtocol.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkProtocol.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkProtocol.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkProtocol.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlNetworkProtocol.Controls.Add(Me.lblNetworkProtocolDescription, 1, 0)
    Me.pnlNetworkProtocol.Controls.Add(Me.pctNetworkProtocolIcon, 0, 0)
    Me.pnlNetworkProtocol.Controls.Add(Me.chkTLSProxy, 1, 2)
    Me.pnlNetworkProtocol.Controls.Add(Me.chkNetworkSecurityEnforce, 1, 1)
    Me.pnlNetworkProtocol.Controls.Add(Me.chkNetworkProtocolTLS13, 3, 1)
    Me.pnlNetworkProtocol.Controls.Add(Me.chkNetworkProtocolSSL3, 4, 2)
    Me.pnlNetworkProtocol.Controls.Add(Me.chkNetworkProtocolTLS12, 4, 1)
    Me.pnlNetworkProtocol.Controls.Add(Me.chkNetworkProtocolTLS10, 3, 2)
    Me.pnlNetworkProtocol.Controls.Add(Me.chkNetworkProtocolTLS11, 5, 1)
    Me.pnlNetworkProtocol.Location = New System.Drawing.Point(3, 296)
    Me.pnlNetworkProtocol.Name = "pnlNetworkProtocol"
    Me.pnlNetworkProtocol.RowCount = 3
    Me.pnlNetworkProtocol.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlNetworkProtocol.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlNetworkProtocol.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlNetworkProtocol.Size = New System.Drawing.Size(371, 80)
    Me.pnlNetworkProtocol.TabIndex = 6
    '
    'lblNetworkProtocolDescription
    '
    Me.lblNetworkProtocolDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblNetworkProtocolDescription.AutoSize = True
    Me.pnlNetworkProtocol.SetColumnSpan(Me.lblNetworkProtocolDescription, 6)
    Me.lblNetworkProtocolDescription.Location = New System.Drawing.Point(59, 3)
    Me.lblNetworkProtocolDescription.Margin = New System.Windows.Forms.Padding(3)
    Me.lblNetworkProtocolDescription.Name = "lblNetworkProtocolDescription"
    Me.lblNetworkProtocolDescription.Size = New System.Drawing.Size(309, 26)
    Me.lblNetworkProtocolDescription.TabIndex = 0
    Me.lblNetworkProtocolDescription.Text = "Some servers may prefer the older SSL protocol, others may" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "require modern TLS."
    '
    'pctNetworkProtocolIcon
    '
    Me.pctNetworkProtocolIcon.Anchor = System.Windows.Forms.AnchorStyles.Top
    Me.pctNetworkProtocolIcon.Image = Global.RestrictionTracker.My.Resources.Resources.net_security
    Me.pctNetworkProtocolIcon.Location = New System.Drawing.Point(21, 3)
    Me.pctNetworkProtocolIcon.Margin = New System.Windows.Forms.Padding(21, 3, 3, 3)
    Me.pctNetworkProtocolIcon.Name = "pctNetworkProtocolIcon"
    Me.pnlNetworkProtocol.SetRowSpan(Me.pctNetworkProtocolIcon, 2)
    Me.pctNetworkProtocolIcon.Size = New System.Drawing.Size(32, 32)
    Me.pctNetworkProtocolIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pctNetworkProtocolIcon.TabIndex = 0
    Me.pctNetworkProtocolIcon.TabStop = False
    '
    'chkTLSProxy
    '
    Me.chkTLSProxy.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkTLSProxy.AutoSize = True
    Me.chkTLSProxy.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkTLSProxy.Location = New System.Drawing.Point(59, 59)
    Me.chkTLSProxy.Name = "chkTLSProxy"
    Me.chkTLSProxy.Size = New System.Drawing.Size(81, 18)
    Me.chkTLSProxy.TabIndex = 2
    Me.chkTLSProxy.Text = "TLS Proxy"
    Me.ttConfig.SetToolTip(Me.chkTLSProxy, "If your Operating System does not support the Security Protocol required for your" & _
        " provider, you can use this Proxy to connect through the RealityRipple.com serve" & _
        "r.")
    Me.chkTLSProxy.UseVisualStyleBackColor = True
    '
    'chkNetworkSecurityEnforce
    '
    Me.chkNetworkSecurityEnforce.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkNetworkSecurityEnforce.AutoSize = True
    Me.chkNetworkSecurityEnforce.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkNetworkSecurityEnforce.Location = New System.Drawing.Point(59, 35)
    Me.chkNetworkSecurityEnforce.Name = "chkNetworkSecurityEnforce"
    Me.chkNetworkSecurityEnforce.Size = New System.Drawing.Size(69, 18)
    Me.chkNetworkSecurityEnforce.TabIndex = 1
    Me.chkNetworkSecurityEnforce.Text = "Enforce"
    Me.ttConfig.SetToolTip(Me.chkNetworkSecurityEnforce, resources.GetString("chkNetworkSecurityEnforce.ToolTip"))
    Me.chkNetworkSecurityEnforce.UseVisualStyleBackColor = True
    '
    'chkNetworkProtocolTLS13
    '
    Me.chkNetworkProtocolTLS13.AutoSize = True
    Me.chkNetworkProtocolTLS13.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkNetworkProtocolTLS13.Location = New System.Drawing.Point(146, 35)
    Me.chkNetworkProtocolTLS13.Name = "chkNetworkProtocolTLS13"
    Me.chkNetworkProtocolTLS13.Size = New System.Drawing.Size(70, 18)
    Me.chkNetworkProtocolTLS13.TabIndex = 3
    Me.chkNetworkProtocolTLS13.Text = "TLS 1.3"
    Me.ttConfig.SetToolTip(Me.chkNetworkProtocolTLS13, "Check this box to allow use of the latest TLS 1.3 protocol.")
    Me.chkNetworkProtocolTLS13.UseVisualStyleBackColor = True
    '
    'chkNetworkProtocolSSL3
    '
    Me.chkNetworkProtocolSSL3.AutoSize = True
    Me.chkNetworkProtocolSSL3.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkNetworkProtocolSSL3.Location = New System.Drawing.Point(222, 59)
    Me.chkNetworkProtocolSSL3.Name = "chkNetworkProtocolSSL3"
    Me.chkNetworkProtocolSSL3.Size = New System.Drawing.Size(70, 18)
    Me.chkNetworkProtocolSSL3.TabIndex = 7
    Me.chkNetworkProtocolSSL3.Text = "SS&L 3.0"
    Me.ttConfig.SetToolTip(Me.chkNetworkProtocolSSL3, "Check this box to allow use of the older SSL 3.0 protocol, which is vulnerable to" & _
        " attacks.")
    Me.chkNetworkProtocolSSL3.UseVisualStyleBackColor = False
    '
    'chkNetworkProtocolTLS12
    '
    Me.chkNetworkProtocolTLS12.AutoSize = True
    Me.chkNetworkProtocolTLS12.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkNetworkProtocolTLS12.Location = New System.Drawing.Point(222, 35)
    Me.chkNetworkProtocolTLS12.Name = "chkNetworkProtocolTLS12"
    Me.chkNetworkProtocolTLS12.Size = New System.Drawing.Size(70, 18)
    Me.chkNetworkProtocolTLS12.TabIndex = 4
    Me.chkNetworkProtocolTLS12.Text = "TLS 1.2"
    Me.ttConfig.SetToolTip(Me.chkNetworkProtocolTLS12, "Check this box to allow use of the newer, safer TLS 1.2 protocol.")
    Me.chkNetworkProtocolTLS12.UseVisualStyleBackColor = True
    '
    'chkNetworkProtocolTLS10
    '
    Me.chkNetworkProtocolTLS10.AutoSize = True
    Me.chkNetworkProtocolTLS10.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkNetworkProtocolTLS10.Location = New System.Drawing.Point(146, 59)
    Me.chkNetworkProtocolTLS10.Name = "chkNetworkProtocolTLS10"
    Me.chkNetworkProtocolTLS10.Size = New System.Drawing.Size(70, 18)
    Me.chkNetworkProtocolTLS10.TabIndex = 6
    Me.chkNetworkProtocolTLS10.Text = "TLS 1.0"
    Me.ttConfig.SetToolTip(Me.chkNetworkProtocolTLS10, "Check this box to allow use of the older TLS 1.0 protocol, which may be vulnerabl" & _
        "e to attacks.")
    Me.chkNetworkProtocolTLS10.UseVisualStyleBackColor = True
    '
    'chkNetworkProtocolTLS11
    '
    Me.chkNetworkProtocolTLS11.AutoSize = True
    Me.chkNetworkProtocolTLS11.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkNetworkProtocolTLS11.Location = New System.Drawing.Point(298, 35)
    Me.chkNetworkProtocolTLS11.Name = "chkNetworkProtocolTLS11"
    Me.chkNetworkProtocolTLS11.Size = New System.Drawing.Size(70, 18)
    Me.chkNetworkProtocolTLS11.TabIndex = 5
    Me.chkNetworkProtocolTLS11.Text = "TLS 1.1"
    Me.ttConfig.SetToolTip(Me.chkNetworkProtocolTLS11, "Check this box to allow use of the newer, safer TLS 1.1 protocol.")
    Me.chkNetworkProtocolTLS11.UseVisualStyleBackColor = True
    '
    'pnlNetworkProtocolTitle
    '
    Me.pnlNetworkProtocolTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlNetworkProtocolTitle.AutoSize = True
    Me.pnlNetworkProtocolTitle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlNetworkProtocolTitle.ColumnCount = 2
    Me.pnlNetworkProtocolTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkProtocolTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlNetworkProtocolTitle.Controls.Add(Me.lblNetworkProtocolTitle, 0, 0)
    Me.pnlNetworkProtocolTitle.Controls.Add(Me.lnNetworkProtocolTitle, 1, 0)
    Me.pnlNetworkProtocolTitle.Location = New System.Drawing.Point(3, 275)
    Me.pnlNetworkProtocolTitle.Margin = New System.Windows.Forms.Padding(3, 10, 3, 5)
    Me.pnlNetworkProtocolTitle.Name = "pnlNetworkProtocolTitle"
    Me.pnlNetworkProtocolTitle.RowCount = 1
    Me.pnlNetworkProtocolTitle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlNetworkProtocolTitle.Size = New System.Drawing.Size(379, 13)
    Me.pnlNetworkProtocolTitle.TabIndex = 5
    '
    'lblNetworkProtocolTitle
    '
    Me.lblNetworkProtocolTitle.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblNetworkProtocolTitle.AutoSize = True
    Me.lblNetworkProtocolTitle.Location = New System.Drawing.Point(3, 0)
    Me.lblNetworkProtocolTitle.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblNetworkProtocolTitle.Name = "lblNetworkProtocolTitle"
    Me.lblNetworkProtocolTitle.Size = New System.Drawing.Size(87, 13)
    Me.lblNetworkProtocolTitle.TabIndex = 0
    Me.lblNetworkProtocolTitle.Text = "Security Protocol"
    '
    'pnlNetworkUpdate
    '
    Me.pnlNetworkUpdate.AutoSize = True
    Me.pnlNetworkUpdate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlNetworkUpdate.ColumnCount = 3
    Me.pnlNetworkUpdate.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkUpdate.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkUpdate.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkUpdate.Controls.Add(Me.pctNetworkUpdateIcon, 0, 0)
    Me.pnlNetworkUpdate.Controls.Add(Me.pnlNetworkUpdateTime, 1, 2)
    Me.pnlNetworkUpdate.Controls.Add(Me.chkUpdateBETA, 1, 1)
    Me.pnlNetworkUpdate.Controls.Add(Me.cmbUpdateAutomation, 1, 0)
    Me.pnlNetworkUpdate.Location = New System.Drawing.Point(3, 398)
    Me.pnlNetworkUpdate.Name = "pnlNetworkUpdate"
    Me.pnlNetworkUpdate.RowCount = 3
    Me.pnlNetworkUpdate.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlNetworkUpdate.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlNetworkUpdate.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlNetworkUpdate.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlNetworkUpdate.Size = New System.Drawing.Size(310, 78)
    Me.pnlNetworkUpdate.TabIndex = 8
    '
    'pctNetworkUpdateIcon
    '
    Me.pctNetworkUpdateIcon.Anchor = System.Windows.Forms.AnchorStyles.Top
    Me.pctNetworkUpdateIcon.Image = Global.RestrictionTracker.My.Resources.Resources.net_update_unknown
    Me.pctNetworkUpdateIcon.Location = New System.Drawing.Point(21, 3)
    Me.pctNetworkUpdateIcon.Margin = New System.Windows.Forms.Padding(21, 3, 3, 3)
    Me.pctNetworkUpdateIcon.Name = "pctNetworkUpdateIcon"
    Me.pnlNetworkUpdate.SetRowSpan(Me.pctNetworkUpdateIcon, 2)
    Me.pctNetworkUpdateIcon.Size = New System.Drawing.Size(32, 32)
    Me.pctNetworkUpdateIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pctNetworkUpdateIcon.TabIndex = 2
    Me.pctNetworkUpdateIcon.TabStop = False
    '
    'pnlNetworkUpdateTime
    '
    Me.pnlNetworkUpdateTime.AutoSize = True
    Me.pnlNetworkUpdateTime.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlNetworkUpdateTime.ColumnCount = 2
    Me.pnlNetworkUpdate.SetColumnSpan(Me.pnlNetworkUpdateTime, 2)
    Me.pnlNetworkUpdateTime.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlNetworkUpdateTime.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlNetworkUpdateTime.Controls.Add(Me.lblUpdateInterval, 0, 0)
    Me.pnlNetworkUpdateTime.Controls.Add(Me.cmbUpdateInterval, 1, 0)
    Me.pnlNetworkUpdateTime.Location = New System.Drawing.Point(56, 51)
    Me.pnlNetworkUpdateTime.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlNetworkUpdateTime.Name = "pnlNetworkUpdateTime"
    Me.pnlNetworkUpdateTime.RowCount = 1
    Me.pnlNetworkUpdateTime.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlNetworkUpdateTime.Size = New System.Drawing.Size(254, 27)
    Me.pnlNetworkUpdateTime.TabIndex = 3
    '
    'lblUpdateInterval
    '
    Me.lblUpdateInterval.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblUpdateInterval.AutoSize = True
    Me.lblUpdateInterval.Location = New System.Drawing.Point(3, 7)
    Me.lblUpdateInterval.Name = "lblUpdateInterval"
    Me.lblUpdateInterval.Size = New System.Drawing.Size(117, 13)
    Me.lblUpdateInterval.TabIndex = 2
    Me.lblUpdateInterval.Text = "Perform a &check every:"
    '
    'cmbUpdateInterval
    '
    Me.cmbUpdateInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cmbUpdateInterval.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmbUpdateInterval.FormattingEnabled = True
    Me.cmbUpdateInterval.Items.AddRange(New Object() {"Day", "Three Days", "Week", "Fifteen Days", "Month"})
    Me.cmbUpdateInterval.Location = New System.Drawing.Point(130, 3)
    Me.cmbUpdateInterval.Name = "cmbUpdateInterval"
    Me.cmbUpdateInterval.Size = New System.Drawing.Size(121, 21)
    Me.cmbUpdateInterval.TabIndex = 3
    Me.ttConfig.SetToolTip(Me.cmbUpdateInterval, "Select an interval between automatic update checks.")
    '
    'chkUpdateBETA
    '
    Me.chkUpdateBETA.AutoSize = True
    Me.pnlNetworkUpdate.SetColumnSpan(Me.chkUpdateBETA, 2)
    Me.chkUpdateBETA.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkUpdateBETA.Location = New System.Drawing.Point(59, 30)
    Me.chkUpdateBETA.Name = "chkUpdateBETA"
    Me.chkUpdateBETA.Size = New System.Drawing.Size(150, 18)
    Me.chkUpdateBETA.TabIndex = 1
    Me.chkUpdateBETA.Text = "Check for BETA updates"
    Me.ttConfig.SetToolTip(Me.chkUpdateBETA, "Download potentially unstable updates to help test the next release of Satellite " & _
        "Restriction Tracker.")
    Me.chkUpdateBETA.UseVisualStyleBackColor = False
    '
    'cmbUpdateAutomation
    '
    Me.cmbUpdateAutomation.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlNetworkUpdate.SetColumnSpan(Me.cmbUpdateAutomation, 2)
    Me.cmbUpdateAutomation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cmbUpdateAutomation.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmbUpdateAutomation.FormattingEnabled = True
    Me.cmbUpdateAutomation.Items.AddRange(New Object() {"Install updates automatically (recommended)", "Let me choose whether to install updates", "Never check for updates (not recommended)"})
    Me.cmbUpdateAutomation.Location = New System.Drawing.Point(59, 3)
    Me.cmbUpdateAutomation.Name = "cmbUpdateAutomation"
    Me.cmbUpdateAutomation.Size = New System.Drawing.Size(248, 21)
    Me.cmbUpdateAutomation.TabIndex = 0
    Me.ttConfig.SetToolTip(Me.cmbUpdateAutomation, "Choose how Satellite Restriction Tracker installs updates.")
    '
    'pnlNetworkProxy
    '
    Me.pnlNetworkProxy.AutoSize = True
    Me.pnlNetworkProxy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlNetworkProxy.ColumnCount = 2
    Me.pnlNetworkProxy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkProxy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlNetworkProxy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlNetworkProxy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlNetworkProxy.Controls.Add(Me.pnlProxy, 0, 1)
    Me.pnlNetworkProxy.Controls.Add(Me.pctNetworkProxyIcon, 0, 0)
    Me.pnlNetworkProxy.Controls.Add(Me.lblNetworkProxyDescrption, 1, 0)
    Me.pnlNetworkProxy.Location = New System.Drawing.Point(3, 111)
    Me.pnlNetworkProxy.Name = "pnlNetworkProxy"
    Me.pnlNetworkProxy.RowCount = 2
    Me.pnlNetworkProxy.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlNetworkProxy.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlNetworkProxy.Size = New System.Drawing.Size(338, 163)
    Me.pnlNetworkProxy.TabIndex = 4
    '
    'pnlProxy
    '
    Me.pnlProxy.AutoSize = True
    Me.pnlProxy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlProxy.ColumnCount = 2
    Me.pnlProxy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlProxy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlProxy.Controls.Add(Me.lblProxyType, 0, 0)
    Me.pnlProxy.Controls.Add(Me.txtProxyUser, 0, 4)
    Me.pnlProxy.Controls.Add(Me.lblProxyUser, 0, 3)
    Me.pnlProxy.Controls.Add(Me.txtProxyPassword, 1, 4)
    Me.pnlProxy.Controls.Add(Me.lblProxyPassword, 1, 3)
    Me.pnlProxy.Controls.Add(Me.lblProxyAddr, 0, 1)
    Me.pnlProxy.Controls.Add(Me.txtProxyAddress, 0, 2)
    Me.pnlProxy.Controls.Add(Me.lblProxyPort, 1, 1)
    Me.pnlProxy.Controls.Add(Me.lblProxyDomain, 0, 5)
    Me.pnlProxy.Controls.Add(Me.txtProxyDomain, 0, 6)
    Me.pnlProxy.Controls.Add(Me.cmbProxyType, 1, 0)
    Me.pnlProxy.Controls.Add(Me.txtProxyPort, 1, 2)
    Me.pnlProxy.Location = New System.Drawing.Point(56, 19)
    Me.pnlProxy.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlProxy.Name = "pnlProxy"
    Me.pnlProxy.RowCount = 7
    Me.pnlProxy.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProxy.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProxy.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProxy.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProxy.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProxy.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProxy.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlProxy.Size = New System.Drawing.Size(274, 144)
    Me.pnlProxy.TabIndex = 1
    '
    'lblProxyType
    '
    Me.lblProxyType.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProxyType.AutoSize = True
    Me.lblProxyType.Location = New System.Drawing.Point(3, 7)
    Me.lblProxyType.Name = "lblProxyType"
    Me.lblProxyType.Size = New System.Drawing.Size(63, 13)
    Me.lblProxyType.TabIndex = 0
    Me.lblProxyType.Text = "&Proxy Type:"
    '
    'txtProxyUser
    '
    Me.txtProxyUser.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtProxyUser.Location = New System.Drawing.Point(3, 82)
    Me.txtProxyUser.Name = "txtProxyUser"
    Me.txtProxyUser.Size = New System.Drawing.Size(129, 20)
    Me.txtProxyUser.TabIndex = 7
    Me.ttConfig.SetToolTip(Me.txtProxyUser, "Optional Username for HTTP Proxy authentication.")
    '
    'lblProxyUser
    '
    Me.lblProxyUser.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProxyUser.AutoSize = True
    Me.lblProxyUser.Location = New System.Drawing.Point(3, 66)
    Me.lblProxyUser.Name = "lblProxyUser"
    Me.lblProxyUser.Size = New System.Drawing.Size(58, 13)
    Me.lblProxyUser.TabIndex = 6
    Me.lblProxyUser.Text = "Username:"
    '
    'lblProxyPassword
    '
    Me.lblProxyPassword.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProxyPassword.AutoSize = True
    Me.lblProxyPassword.Location = New System.Drawing.Point(138, 66)
    Me.lblProxyPassword.Name = "lblProxyPassword"
    Me.lblProxyPassword.Size = New System.Drawing.Size(56, 13)
    Me.lblProxyPassword.TabIndex = 8
    Me.lblProxyPassword.Text = "Password:"
    '
    'lblProxyAddr
    '
    Me.lblProxyAddr.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProxyAddr.AutoSize = True
    Me.lblProxyAddr.Location = New System.Drawing.Point(3, 27)
    Me.lblProxyAddr.Name = "lblProxyAddr"
    Me.lblProxyAddr.Size = New System.Drawing.Size(61, 13)
    Me.lblProxyAddr.TabIndex = 2
    Me.lblProxyAddr.Text = "IP Address:"
    '
    'txtProxyAddress
    '
    Me.txtProxyAddress.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtProxyAddress.Location = New System.Drawing.Point(3, 43)
    Me.txtProxyAddress.Name = "txtProxyAddress"
    Me.txtProxyAddress.Size = New System.Drawing.Size(129, 20)
    Me.txtProxyAddress.TabIndex = 3
    Me.ttConfig.SetToolTip(Me.txtProxyAddress, "Address of HTTP Proxy to connect through.")
    '
    'lblProxyPort
    '
    Me.lblProxyPort.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProxyPort.AutoSize = True
    Me.lblProxyPort.Location = New System.Drawing.Point(138, 27)
    Me.lblProxyPort.Name = "lblProxyPort"
    Me.lblProxyPort.Size = New System.Drawing.Size(29, 13)
    Me.lblProxyPort.TabIndex = 4
    Me.lblProxyPort.Text = "Port:"
    '
    'lblProxyDomain
    '
    Me.lblProxyDomain.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProxyDomain.AutoSize = True
    Me.lblProxyDomain.Location = New System.Drawing.Point(3, 105)
    Me.lblProxyDomain.Name = "lblProxyDomain"
    Me.lblProxyDomain.Size = New System.Drawing.Size(46, 13)
    Me.lblProxyDomain.TabIndex = 10
    Me.lblProxyDomain.Text = "Domain:"
    '
    'txtProxyDomain
    '
    Me.txtProxyDomain.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlProxy.SetColumnSpan(Me.txtProxyDomain, 2)
    Me.txtProxyDomain.Location = New System.Drawing.Point(3, 121)
    Me.txtProxyDomain.Name = "txtProxyDomain"
    Me.txtProxyDomain.Size = New System.Drawing.Size(268, 20)
    Me.txtProxyDomain.TabIndex = 11
    Me.ttConfig.SetToolTip(Me.txtProxyDomain, "Optional Domain for HTTP Proxy authentication.")
    '
    'cmbProxyType
    '
    Me.cmbProxyType.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmbProxyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cmbProxyType.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmbProxyType.FormattingEnabled = True
    Me.cmbProxyType.Items.AddRange(New Object() {"None", "System", "IP", "URL"})
    Me.cmbProxyType.Location = New System.Drawing.Point(138, 3)
    Me.cmbProxyType.Name = "cmbProxyType"
    Me.cmbProxyType.Size = New System.Drawing.Size(133, 21)
    Me.cmbProxyType.TabIndex = 1
    Me.ttConfig.SetToolTip(Me.cmbProxyType, "Type of Proxy to Use" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " None: No Proxy" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " System: Default System Proxy Settings" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " I" & _
        "P: HTTP Proxy by IP Address and Port" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " URL: HTTP Proxy by Web URL")
    '
    'pctNetworkProxyIcon
    '
    Me.pctNetworkProxyIcon.Anchor = System.Windows.Forms.AnchorStyles.Top
    Me.pctNetworkProxyIcon.Image = Global.RestrictionTracker.My.Resources.Resources.net_proxy
    Me.pctNetworkProxyIcon.Location = New System.Drawing.Point(21, 3)
    Me.pctNetworkProxyIcon.Margin = New System.Windows.Forms.Padding(21, 3, 3, 3)
    Me.pctNetworkProxyIcon.Name = "pctNetworkProxyIcon"
    Me.pnlNetworkProxy.SetRowSpan(Me.pctNetworkProxyIcon, 2)
    Me.pctNetworkProxyIcon.Size = New System.Drawing.Size(32, 32)
    Me.pctNetworkProxyIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pctNetworkProxyIcon.TabIndex = 2
    Me.pctNetworkProxyIcon.TabStop = False
    '
    'lblNetworkProxyDescrption
    '
    Me.lblNetworkProxyDescrption.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblNetworkProxyDescrption.AutoSize = True
    Me.lblNetworkProxyDescrption.Location = New System.Drawing.Point(59, 3)
    Me.lblNetworkProxyDescrption.Margin = New System.Windows.Forms.Padding(3)
    Me.lblNetworkProxyDescrption.Name = "lblNetworkProxyDescrption"
    Me.lblNetworkProxyDescrption.Size = New System.Drawing.Size(276, 13)
    Me.lblNetworkProxyDescrption.TabIndex = 0
    Me.lblNetworkProxyDescrption.Text = "If you connect through a proxy, enter the IP or URL here."
    '
    'pnlNetworkProxyTitle
    '
    Me.pnlNetworkProxyTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlNetworkProxyTitle.AutoSize = True
    Me.pnlNetworkProxyTitle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlNetworkProxyTitle.ColumnCount = 2
    Me.pnlNetworkProxyTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkProxyTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlNetworkProxyTitle.Controls.Add(Me.lblNetworkProxyTitle, 0, 0)
    Me.pnlNetworkProxyTitle.Controls.Add(Me.lnNetworkProxyTitle, 1, 0)
    Me.pnlNetworkProxyTitle.Location = New System.Drawing.Point(3, 90)
    Me.pnlNetworkProxyTitle.Margin = New System.Windows.Forms.Padding(3, 10, 3, 5)
    Me.pnlNetworkProxyTitle.Name = "pnlNetworkProxyTitle"
    Me.pnlNetworkProxyTitle.RowCount = 1
    Me.pnlNetworkProxyTitle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlNetworkProxyTitle.Size = New System.Drawing.Size(379, 13)
    Me.pnlNetworkProxyTitle.TabIndex = 3
    '
    'lblNetworkProxyTitle
    '
    Me.lblNetworkProxyTitle.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblNetworkProxyTitle.AutoSize = True
    Me.lblNetworkProxyTitle.Location = New System.Drawing.Point(3, 0)
    Me.lblNetworkProxyTitle.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblNetworkProxyTitle.Name = "lblNetworkProxyTitle"
    Me.lblNetworkProxyTitle.Size = New System.Drawing.Size(65, 13)
    Me.lblNetworkProxyTitle.TabIndex = 0
    Me.lblNetworkProxyTitle.Text = "HTTP Proxy"
    '
    'pnlNetworkTimeoutTitle
    '
    Me.pnlNetworkTimeoutTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlNetworkTimeoutTitle.AutoSize = True
    Me.pnlNetworkTimeoutTitle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlNetworkTimeoutTitle.ColumnCount = 2
    Me.pnlNetworkTimeoutTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkTimeoutTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlNetworkTimeoutTitle.Controls.Add(Me.lblNetworkTimeoutTitle, 0, 0)
    Me.pnlNetworkTimeoutTitle.Controls.Add(Me.lnNetworkTimeoutTitle, 1, 0)
    Me.pnlNetworkTimeoutTitle.Location = New System.Drawing.Point(3, 10)
    Me.pnlNetworkTimeoutTitle.Margin = New System.Windows.Forms.Padding(3, 10, 3, 5)
    Me.pnlNetworkTimeoutTitle.Name = "pnlNetworkTimeoutTitle"
    Me.pnlNetworkTimeoutTitle.RowCount = 1
    Me.pnlNetworkTimeoutTitle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlNetworkTimeoutTitle.Size = New System.Drawing.Size(379, 13)
    Me.pnlNetworkTimeoutTitle.TabIndex = 1
    '
    'lblNetworkTimeoutTitle
    '
    Me.lblNetworkTimeoutTitle.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblNetworkTimeoutTitle.AutoSize = True
    Me.lblNetworkTimeoutTitle.Location = New System.Drawing.Point(3, 0)
    Me.lblNetworkTimeoutTitle.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblNetworkTimeoutTitle.Name = "lblNetworkTimeoutTitle"
    Me.lblNetworkTimeoutTitle.Size = New System.Drawing.Size(88, 13)
    Me.lblNetworkTimeoutTitle.TabIndex = 0
    Me.lblNetworkTimeoutTitle.Text = "Network Timeout"
    '
    'pnlNetworkTimeout
    '
    Me.pnlNetworkTimeout.AutoSize = True
    Me.pnlNetworkTimeout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlNetworkTimeout.ColumnCount = 8
    Me.pnlNetworkTimeout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkTimeout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkTimeout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkTimeout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkTimeout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlNetworkTimeout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkTimeout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkTimeout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkTimeout.Controls.Add(Me.pctNetworkTimeoutIcon, 0, 0)
    Me.pnlNetworkTimeout.Controls.Add(Me.txtTimeout, 2, 1)
    Me.pnlNetworkTimeout.Controls.Add(Me.lblTimeout2, 3, 1)
    Me.pnlNetworkTimeout.Controls.Add(Me.lblTimeout1, 1, 1)
    Me.pnlNetworkTimeout.Controls.Add(Me.lblNetworkTimeoutDescription, 1, 0)
    Me.pnlNetworkTimeout.Controls.Add(Me.lblRetries1, 5, 1)
    Me.pnlNetworkTimeout.Controls.Add(Me.txtRetries, 6, 1)
    Me.pnlNetworkTimeout.Controls.Add(Me.lblRetries2, 7, 1)
    Me.pnlNetworkTimeout.Location = New System.Drawing.Point(3, 31)
    Me.pnlNetworkTimeout.Name = "pnlNetworkTimeout"
    Me.pnlNetworkTimeout.RowCount = 2
    Me.pnlNetworkTimeout.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlNetworkTimeout.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlNetworkTimeout.Size = New System.Drawing.Size(379, 58)
    Me.pnlNetworkTimeout.TabIndex = 2
    '
    'pctNetworkTimeoutIcon
    '
    Me.pctNetworkTimeoutIcon.Anchor = System.Windows.Forms.AnchorStyles.Top
    Me.pctNetworkTimeoutIcon.Image = Global.RestrictionTracker.My.Resources.Resources.net_network
    Me.pctNetworkTimeoutIcon.Location = New System.Drawing.Point(21, 3)
    Me.pctNetworkTimeoutIcon.Margin = New System.Windows.Forms.Padding(21, 3, 3, 3)
    Me.pctNetworkTimeoutIcon.Name = "pctNetworkTimeoutIcon"
    Me.pnlNetworkTimeout.SetRowSpan(Me.pctNetworkTimeoutIcon, 2)
    Me.pctNetworkTimeoutIcon.Size = New System.Drawing.Size(32, 32)
    Me.pctNetworkTimeoutIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pctNetworkTimeoutIcon.TabIndex = 2
    Me.pctNetworkTimeoutIcon.TabStop = False
    '
    'lblTimeout2
    '
    Me.lblTimeout2.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTimeout2.AutoSize = True
    Me.lblTimeout2.Location = New System.Drawing.Point(169, 38)
    Me.lblTimeout2.Name = "lblTimeout2"
    Me.lblTimeout2.Size = New System.Drawing.Size(47, 13)
    Me.lblTimeout2.TabIndex = 3
    Me.lblTimeout2.Text = "seconds"
    '
    'lblTimeout1
    '
    Me.lblTimeout1.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTimeout1.AutoSize = True
    Me.lblTimeout1.Location = New System.Drawing.Point(59, 38)
    Me.lblTimeout1.Name = "lblTimeout1"
    Me.lblTimeout1.Size = New System.Drawing.Size(48, 13)
    Me.lblTimeout1.TabIndex = 1
    Me.lblTimeout1.Text = "&Timeout:"
    '
    'lblNetworkTimeoutDescription
    '
    Me.lblNetworkTimeoutDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblNetworkTimeoutDescription.AutoSize = True
    Me.pnlNetworkTimeout.SetColumnSpan(Me.lblNetworkTimeoutDescription, 7)
    Me.lblNetworkTimeoutDescription.Location = New System.Drawing.Point(59, 3)
    Me.lblNetworkTimeoutDescription.Margin = New System.Windows.Forms.Padding(3)
    Me.lblNetworkTimeoutDescription.Name = "lblNetworkTimeoutDescription"
    Me.lblNetworkTimeoutDescription.Size = New System.Drawing.Size(317, 26)
    Me.lblNetworkTimeoutDescription.TabIndex = 0
    Me.lblNetworkTimeoutDescription.Text = "The connection to the server is closed if no response is received in a set amount" & _
    " of time."
    '
    'lblRetries1
    '
    Me.lblRetries1.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblRetries1.AutoSize = True
    Me.lblRetries1.Location = New System.Drawing.Point(248, 38)
    Me.lblRetries1.Name = "lblRetries1"
    Me.lblRetries1.Size = New System.Drawing.Size(35, 13)
    Me.lblRetries1.TabIndex = 4
    Me.lblRetries1.Text = "&Retry:"
    '
    'lblRetries2
    '
    Me.lblRetries2.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblRetries2.AutoSize = True
    Me.lblRetries2.Location = New System.Drawing.Point(345, 38)
    Me.lblRetries2.Name = "lblRetries2"
    Me.lblRetries2.Size = New System.Drawing.Size(31, 13)
    Me.lblRetries2.TabIndex = 6
    Me.lblRetries2.Text = "times"
    '
    'pnlNetworkUpdateTitle
    '
    Me.pnlNetworkUpdateTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlNetworkUpdateTitle.AutoSize = True
    Me.pnlNetworkUpdateTitle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlNetworkUpdateTitle.ColumnCount = 2
    Me.pnlNetworkUpdateTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlNetworkUpdateTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlNetworkUpdateTitle.Controls.Add(Me.lblNetworkUpdateTitle, 0, 0)
    Me.pnlNetworkUpdateTitle.Controls.Add(Me.lnNetworkUpdateTitle, 1, 0)
    Me.pnlNetworkUpdateTitle.Location = New System.Drawing.Point(3, 377)
    Me.pnlNetworkUpdateTitle.Margin = New System.Windows.Forms.Padding(3, 10, 3, 5)
    Me.pnlNetworkUpdateTitle.Name = "pnlNetworkUpdateTitle"
    Me.pnlNetworkUpdateTitle.RowCount = 1
    Me.pnlNetworkUpdateTitle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlNetworkUpdateTitle.Size = New System.Drawing.Size(379, 13)
    Me.pnlNetworkUpdateTitle.TabIndex = 7
    '
    'lblNetworkUpdateTitle
    '
    Me.lblNetworkUpdateTitle.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblNetworkUpdateTitle.AutoSize = True
    Me.lblNetworkUpdateTitle.Location = New System.Drawing.Point(3, 0)
    Me.lblNetworkUpdateTitle.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblNetworkUpdateTitle.Name = "lblNetworkUpdateTitle"
    Me.lblNetworkUpdateTitle.Size = New System.Drawing.Size(47, 13)
    Me.lblNetworkUpdateTitle.TabIndex = 0
    Me.lblNetworkUpdateTitle.Text = "Updates"
    '
    'tabAdvanced
    '
    Me.tabAdvanced.Controls.Add(Me.pnlAdvanced)
    Me.tabAdvanced.Location = New System.Drawing.Point(4, 22)
    Me.tabAdvanced.Name = "tabAdvanced"
    Me.tabAdvanced.Size = New System.Drawing.Size(385, 477)
    Me.tabAdvanced.TabIndex = 3
    Me.tabAdvanced.Text = "Advanced"
    Me.tabAdvanced.UseVisualStyleBackColor = True
    '
    'pnlAdvanced
    '
    Me.pnlAdvanced.ColumnCount = 1
    Me.pnlAdvanced.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAdvanced.Controls.Add(Me.pnlAdvancedPortable, 0, 7)
    Me.pnlAdvanced.Controls.Add(Me.pnlAdvancedPortableTitle, 0, 6)
    Me.pnlAdvanced.Controls.Add(Me.pnlAdvancedData, 0, 1)
    Me.pnlAdvanced.Controls.Add(Me.pnlAdvancedDataTitle, 0, 0)
    Me.pnlAdvanced.Controls.Add(Me.pnlAdvancedNetTestTitle, 0, 3)
    Me.pnlAdvanced.Controls.Add(Me.pnlAdvancedNetTest, 0, 4)
    Me.pnlAdvanced.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlAdvanced.Location = New System.Drawing.Point(0, 0)
    Me.pnlAdvanced.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlAdvanced.Name = "pnlAdvanced"
    Me.pnlAdvanced.RowCount = 8
    Me.pnlAdvanced.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvanced.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvanced.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlAdvanced.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvanced.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvanced.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlAdvanced.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvanced.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvanced.Size = New System.Drawing.Size(385, 477)
    Me.pnlAdvanced.TabIndex = 1
    '
    'pnlAdvancedPortable
    '
    Me.pnlAdvancedPortable.AutoSize = True
    Me.pnlAdvancedPortable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlAdvancedPortable.ColumnCount = 3
    Me.pnlAdvancedPortable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAdvancedPortable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAdvancedPortable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAdvancedPortable.Controls.Add(Me.pnlPortableDir, 2, 1)
    Me.pnlAdvancedPortable.Controls.Add(Me.pctAdvancedPortableIcon, 0, 0)
    Me.pnlAdvancedPortable.Controls.Add(Me.lblAdvancedPortableDescription, 1, 0)
    Me.pnlAdvancedPortable.Controls.Add(Me.cmdMakePortable, 1, 2)
    Me.pnlAdvancedPortable.Controls.Add(Me.lblPortableDir, 1, 1)
    Me.pnlAdvancedPortable.Location = New System.Drawing.Point(3, 386)
    Me.pnlAdvancedPortable.Name = "pnlAdvancedPortable"
    Me.pnlAdvancedPortable.RowCount = 3
    Me.pnlAdvancedPortable.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvancedPortable.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvancedPortable.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvancedPortable.Size = New System.Drawing.Size(375, 89)
    Me.pnlAdvancedPortable.TabIndex = 5
    '
    'pnlPortableDir
    '
    Me.pnlPortableDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlPortableDir.AutoSize = True
    Me.pnlPortableDir.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlPortableDir.ColumnCount = 2
    Me.pnlPortableDir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPortableDir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPortableDir.Controls.Add(Me.txtPortableDir, 0, 0)
    Me.pnlPortableDir.Controls.Add(Me.cmdPortableDir, 1, 0)
    Me.pnlPortableDir.Location = New System.Drawing.Point(114, 32)
    Me.pnlPortableDir.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlPortableDir.Name = "pnlPortableDir"
    Me.pnlPortableDir.RowCount = 1
    Me.pnlPortableDir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPortableDir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
    Me.pnlPortableDir.Size = New System.Drawing.Size(261, 28)
    Me.pnlPortableDir.TabIndex = 2
    '
    'txtPortableDir
    '
    Me.txtPortableDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtPortableDir.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
    Me.txtPortableDir.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories
    Me.txtPortableDir.Location = New System.Drawing.Point(3, 4)
    Me.txtPortableDir.Margin = New System.Windows.Forms.Padding(3, 3, 1, 3)
    Me.txtPortableDir.Name = "txtPortableDir"
    Me.txtPortableDir.Size = New System.Drawing.Size(217, 20)
    Me.txtPortableDir.TabIndex = 0
    Me.ttConfig.SetToolTip(Me.txtPortableDir, "Select the location you wish to place a portable copy of Satellite Restriction Tr" & _
        "acker, such as a desktop folder or a flash drive.")
    '
    'cmdPortableDir
    '
    Me.cmdPortableDir.AutoSize = True
    Me.cmdPortableDir.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdPortableDir.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdPortableDir.Location = New System.Drawing.Point(222, 3)
    Me.cmdPortableDir.Margin = New System.Windows.Forms.Padding(1, 3, 3, 3)
    Me.cmdPortableDir.Name = "cmdPortableDir"
    Me.cmdPortableDir.Size = New System.Drawing.Size(36, 22)
    Me.cmdPortableDir.TabIndex = 1
    Me.cmdPortableDir.Text = ". . ."
    Me.ttConfig.SetToolTip(Me.cmdPortableDir, "Browse for a Directory to place the portable application.")
    Me.cmdPortableDir.UseVisualStyleBackColor = True
    '
    'pctAdvancedPortableIcon
    '
    Me.pctAdvancedPortableIcon.Anchor = System.Windows.Forms.AnchorStyles.Top
    Me.pctAdvancedPortableIcon.Image = Global.RestrictionTracker.My.Resources.Resources.advanced_portable_missing
    Me.pctAdvancedPortableIcon.Location = New System.Drawing.Point(21, 3)
    Me.pctAdvancedPortableIcon.Margin = New System.Windows.Forms.Padding(21, 3, 3, 3)
    Me.pctAdvancedPortableIcon.Name = "pctAdvancedPortableIcon"
    Me.pnlAdvancedPortable.SetRowSpan(Me.pctAdvancedPortableIcon, 2)
    Me.pctAdvancedPortableIcon.Size = New System.Drawing.Size(32, 32)
    Me.pctAdvancedPortableIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pctAdvancedPortableIcon.TabIndex = 2
    Me.pctAdvancedPortableIcon.TabStop = False
    '
    'lblAdvancedPortableDescription
    '
    Me.lblAdvancedPortableDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblAdvancedPortableDescription.AutoSize = True
    Me.pnlAdvancedPortable.SetColumnSpan(Me.lblAdvancedPortableDescription, 2)
    Me.lblAdvancedPortableDescription.Location = New System.Drawing.Point(59, 3)
    Me.lblAdvancedPortableDescription.Margin = New System.Windows.Forms.Padding(3)
    Me.lblAdvancedPortableDescription.Name = "lblAdvancedPortableDescription"
    Me.lblAdvancedPortableDescription.Size = New System.Drawing.Size(313, 26)
    Me.lblAdvancedPortableDescription.TabIndex = 0
    Me.lblAdvancedPortableDescription.Text = "Create a portable copy of Satellite Restriction Tracker which can" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "run off a flas" & _
    "h drive or other location without being installed."
    '
    'cmdMakePortable
    '
    Me.cmdMakePortable.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.pnlAdvancedPortable.SetColumnSpan(Me.cmdMakePortable, 2)
    Me.cmdMakePortable.Enabled = False
    Me.cmdMakePortable.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdMakePortable.Location = New System.Drawing.Point(197, 63)
    Me.cmdMakePortable.Name = "cmdMakePortable"
    Me.cmdMakePortable.Size = New System.Drawing.Size(175, 23)
    Me.cmdMakePortable.TabIndex = 3
    Me.cmdMakePortable.Text = "&Create Portable Application"
    Me.ttConfig.SetToolTip(Me.cmdMakePortable, "No directory selected!")
    Me.cmdMakePortable.UseVisualStyleBackColor = True
    '
    'lblPortableDir
    '
    Me.lblPortableDir.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblPortableDir.AutoSize = True
    Me.lblPortableDir.Location = New System.Drawing.Point(59, 39)
    Me.lblPortableDir.Name = "lblPortableDir"
    Me.lblPortableDir.Size = New System.Drawing.Size(52, 13)
    Me.lblPortableDir.TabIndex = 1
    Me.lblPortableDir.Text = "&Directory:"
    '
    'pnlAdvancedPortableTitle
    '
    Me.pnlAdvancedPortableTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlAdvancedPortableTitle.AutoSize = True
    Me.pnlAdvancedPortableTitle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlAdvancedPortableTitle.ColumnCount = 2
    Me.pnlAdvancedPortableTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAdvancedPortableTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAdvancedPortableTitle.Controls.Add(Me.lblAdvancedPortableTitle, 0, 0)
    Me.pnlAdvancedPortableTitle.Controls.Add(Me.lnAdvancedPortableTitle, 1, 0)
    Me.pnlAdvancedPortableTitle.Location = New System.Drawing.Point(3, 365)
    Me.pnlAdvancedPortableTitle.Margin = New System.Windows.Forms.Padding(3, 10, 3, 5)
    Me.pnlAdvancedPortableTitle.Name = "pnlAdvancedPortableTitle"
    Me.pnlAdvancedPortableTitle.RowCount = 1
    Me.pnlAdvancedPortableTitle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAdvancedPortableTitle.Size = New System.Drawing.Size(379, 13)
    Me.pnlAdvancedPortableTitle.TabIndex = 4
    '
    'lblAdvancedPortableTitle
    '
    Me.lblAdvancedPortableTitle.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAdvancedPortableTitle.AutoSize = True
    Me.lblAdvancedPortableTitle.Location = New System.Drawing.Point(3, 0)
    Me.lblAdvancedPortableTitle.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblAdvancedPortableTitle.Name = "lblAdvancedPortableTitle"
    Me.lblAdvancedPortableTitle.Size = New System.Drawing.Size(101, 13)
    Me.lblAdvancedPortableTitle.TabIndex = 0
    Me.lblAdvancedPortableTitle.Text = "Portable Application"
    '
    'pnlAdvancedData
    '
    Me.pnlAdvancedData.AutoSize = True
    Me.pnlAdvancedData.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlAdvancedData.ColumnCount = 2
    Me.pnlAdvancedData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAdvancedData.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAdvancedData.Controls.Add(Me.pctAdvancedDataIcon, 0, 0)
    Me.pnlAdvancedData.Controls.Add(Me.pnlAdvancedDataInput, 1, 0)
    Me.pnlAdvancedData.Location = New System.Drawing.Point(3, 31)
    Me.pnlAdvancedData.Name = "pnlAdvancedData"
    Me.pnlAdvancedData.RowCount = 1
    Me.pnlAdvancedData.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAdvancedData.Size = New System.Drawing.Size(375, 150)
    Me.pnlAdvancedData.TabIndex = 1
    '
    'pctAdvancedDataIcon
    '
    Me.pctAdvancedDataIcon.Anchor = System.Windows.Forms.AnchorStyles.Top
    Me.pctAdvancedDataIcon.Image = Global.RestrictionTracker.My.Resources.Resources.advanced_data
    Me.pctAdvancedDataIcon.Location = New System.Drawing.Point(21, 3)
    Me.pctAdvancedDataIcon.Margin = New System.Windows.Forms.Padding(21, 3, 3, 3)
    Me.pctAdvancedDataIcon.Name = "pctAdvancedDataIcon"
    Me.pctAdvancedDataIcon.Size = New System.Drawing.Size(32, 32)
    Me.pctAdvancedDataIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pctAdvancedDataIcon.TabIndex = 0
    Me.pctAdvancedDataIcon.TabStop = False
    '
    'pnlAdvancedDataInput
    '
    Me.pnlAdvancedDataInput.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlAdvancedDataInput.AutoSize = True
    Me.pnlAdvancedDataInput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlAdvancedDataInput.ColumnCount = 2
    Me.pnlAdvancedDataInput.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAdvancedDataInput.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAdvancedDataInput.Controls.Add(Me.pnlHistoryDir, 1, 3)
    Me.pnlAdvancedDataInput.Controls.Add(Me.lblAdvancedDataDescription, 0, 0)
    Me.pnlAdvancedDataInput.Controls.Add(Me.optHistoryCustom, 0, 3)
    Me.pnlAdvancedDataInput.Controls.Add(Me.optHistoryProgramData, 0, 1)
    Me.pnlAdvancedDataInput.Controls.Add(Me.optHistoryAppData, 0, 2)
    Me.pnlAdvancedDataInput.Controls.Add(Me.cmdHistoryDirOpen, 0, 4)
    Me.pnlAdvancedDataInput.Location = New System.Drawing.Point(56, 0)
    Me.pnlAdvancedDataInput.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlAdvancedDataInput.Name = "pnlAdvancedDataInput"
    Me.pnlAdvancedDataInput.RowCount = 5
    Me.pnlAdvancedDataInput.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvancedDataInput.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvancedDataInput.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvancedDataInput.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvancedDataInput.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvancedDataInput.Size = New System.Drawing.Size(319, 150)
    Me.pnlAdvancedDataInput.TabIndex = 1
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
    Me.pnlHistoryDir.Location = New System.Drawing.Point(75, 93)
    Me.pnlHistoryDir.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlHistoryDir.Name = "pnlHistoryDir"
    Me.pnlHistoryDir.RowCount = 1
    Me.pnlHistoryDir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlHistoryDir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
    Me.pnlHistoryDir.Size = New System.Drawing.Size(244, 28)
    Me.pnlHistoryDir.TabIndex = 4
    '
    'txtHistoryDir
    '
    Me.txtHistoryDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtHistoryDir.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
    Me.txtHistoryDir.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories
    Me.txtHistoryDir.Location = New System.Drawing.Point(3, 4)
    Me.txtHistoryDir.Margin = New System.Windows.Forms.Padding(3, 3, 1, 3)
    Me.txtHistoryDir.Name = "txtHistoryDir"
    Me.txtHistoryDir.Size = New System.Drawing.Size(200, 20)
    Me.txtHistoryDir.TabIndex = 0
    Me.ttConfig.SetToolTip(Me.txtHistoryDir, "Directory used to save History Data.")
    '
    'cmdHistoryDir
    '
    Me.cmdHistoryDir.AutoSize = True
    Me.cmdHistoryDir.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdHistoryDir.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdHistoryDir.Location = New System.Drawing.Point(205, 3)
    Me.cmdHistoryDir.Margin = New System.Windows.Forms.Padding(1, 3, 3, 3)
    Me.cmdHistoryDir.Name = "cmdHistoryDir"
    Me.cmdHistoryDir.Size = New System.Drawing.Size(36, 22)
    Me.cmdHistoryDir.TabIndex = 1
    Me.cmdHistoryDir.Text = ". . ."
    Me.ttConfig.SetToolTip(Me.cmdHistoryDir, "Browse for a Directory in which to save History Data.")
    Me.cmdHistoryDir.UseVisualStyleBackColor = True
    '
    'lblAdvancedDataDescription
    '
    Me.lblAdvancedDataDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblAdvancedDataDescription.AutoSize = True
    Me.pnlAdvancedDataInput.SetColumnSpan(Me.lblAdvancedDataDescription, 2)
    Me.lblAdvancedDataDescription.Location = New System.Drawing.Point(3, 3)
    Me.lblAdvancedDataDescription.Margin = New System.Windows.Forms.Padding(3)
    Me.lblAdvancedDataDescription.Name = "lblAdvancedDataDescription"
    Me.lblAdvancedDataDescription.Size = New System.Drawing.Size(313, 39)
    Me.lblAdvancedDataDescription.TabIndex = 0
    Me.lblAdvancedDataDescription.Text = "Your usage data will be stored in this directory." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "If you use the Logger Service," & _
    " data must be stored in the" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "ProgramData folder. Otherwise, AppData is recommend" & _
    "ed."
    '
    'optHistoryCustom
    '
    Me.optHistoryCustom.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.optHistoryCustom.AutoSize = True
    Me.optHistoryCustom.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.optHistoryCustom.Location = New System.Drawing.Point(3, 98)
    Me.optHistoryCustom.Name = "optHistoryCustom"
    Me.optHistoryCustom.Size = New System.Drawing.Size(69, 18)
    Me.optHistoryCustom.TabIndex = 3
    Me.optHistoryCustom.TabStop = True
    Me.optHistoryCustom.Text = "&Custom:"
    Me.ttConfig.SetToolTip(Me.optHistoryCustom, "Save History Data to a custom directory.")
    Me.optHistoryCustom.UseVisualStyleBackColor = True
    '
    'optHistoryProgramData
    '
    Me.optHistoryProgramData.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.optHistoryProgramData.AutoSize = True
    Me.pnlAdvancedDataInput.SetColumnSpan(Me.optHistoryProgramData, 2)
    Me.optHistoryProgramData.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.optHistoryProgramData.Location = New System.Drawing.Point(3, 48)
    Me.optHistoryProgramData.Name = "optHistoryProgramData"
    Me.optHistoryProgramData.Size = New System.Drawing.Size(96, 18)
    Me.optHistoryProgramData.TabIndex = 1
    Me.optHistoryProgramData.TabStop = True
    Me.optHistoryProgramData.Text = "&Program Data"
    Me.ttConfig.SetToolTip(Me.optHistoryProgramData, "Save History Data to the shared Program Data directory.")
    Me.optHistoryProgramData.UseVisualStyleBackColor = True
    '
    'optHistoryAppData
    '
    Me.optHistoryAppData.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.optHistoryAppData.AutoSize = True
    Me.pnlAdvancedDataInput.SetColumnSpan(Me.optHistoryAppData, 2)
    Me.optHistoryAppData.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.optHistoryAppData.Location = New System.Drawing.Point(3, 72)
    Me.optHistoryAppData.Name = "optHistoryAppData"
    Me.optHistoryAppData.Size = New System.Drawing.Size(109, 18)
    Me.optHistoryAppData.TabIndex = 2
    Me.optHistoryAppData.TabStop = True
    Me.optHistoryAppData.Text = "&Application Data"
    Me.ttConfig.SetToolTip(Me.optHistoryAppData, "Save History Data to your local account's Application Data directory.")
    Me.optHistoryAppData.UseVisualStyleBackColor = True
    '
    'cmdHistoryDirOpen
    '
    Me.cmdHistoryDirOpen.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.pnlAdvancedDataInput.SetColumnSpan(Me.cmdHistoryDirOpen, 2)
    Me.cmdHistoryDirOpen.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdHistoryDirOpen.Location = New System.Drawing.Point(94, 124)
    Me.cmdHistoryDirOpen.Name = "cmdHistoryDirOpen"
    Me.cmdHistoryDirOpen.Size = New System.Drawing.Size(130, 23)
    Me.cmdHistoryDirOpen.TabIndex = 5
    Me.cmdHistoryDirOpen.Text = "&Open Data Directory"
    Me.ttConfig.SetToolTip(Me.cmdHistoryDirOpen, "Open the directory where your History Data is stored.")
    Me.cmdHistoryDirOpen.UseVisualStyleBackColor = True
    '
    'pnlAdvancedDataTitle
    '
    Me.pnlAdvancedDataTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlAdvancedDataTitle.AutoSize = True
    Me.pnlAdvancedDataTitle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlAdvancedDataTitle.ColumnCount = 2
    Me.pnlAdvancedDataTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAdvancedDataTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAdvancedDataTitle.Controls.Add(Me.lblAdvancedDataTitle, 0, 0)
    Me.pnlAdvancedDataTitle.Controls.Add(Me.lnAdvancedDataTitle, 1, 0)
    Me.pnlAdvancedDataTitle.Location = New System.Drawing.Point(3, 10)
    Me.pnlAdvancedDataTitle.Margin = New System.Windows.Forms.Padding(3, 10, 3, 5)
    Me.pnlAdvancedDataTitle.Name = "pnlAdvancedDataTitle"
    Me.pnlAdvancedDataTitle.RowCount = 1
    Me.pnlAdvancedDataTitle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAdvancedDataTitle.Size = New System.Drawing.Size(379, 13)
    Me.pnlAdvancedDataTitle.TabIndex = 0
    '
    'lblAdvancedDataTitle
    '
    Me.lblAdvancedDataTitle.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAdvancedDataTitle.AutoSize = True
    Me.lblAdvancedDataTitle.Location = New System.Drawing.Point(3, 0)
    Me.lblAdvancedDataTitle.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblAdvancedDataTitle.Name = "lblAdvancedDataTitle"
    Me.lblAdvancedDataTitle.Size = New System.Drawing.Size(75, 13)
    Me.lblAdvancedDataTitle.TabIndex = 0
    Me.lblAdvancedDataTitle.Text = "Data Directory"
    '
    'pnlAdvancedNetTestTitle
    '
    Me.pnlAdvancedNetTestTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlAdvancedNetTestTitle.AutoSize = True
    Me.pnlAdvancedNetTestTitle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlAdvancedNetTestTitle.ColumnCount = 2
    Me.pnlAdvancedNetTestTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAdvancedNetTestTitle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAdvancedNetTestTitle.Controls.Add(Me.lblAdvancedNetTestTitle, 0, 0)
    Me.pnlAdvancedNetTestTitle.Controls.Add(Me.lnAdvancedNetTestTitle, 1, 0)
    Me.pnlAdvancedNetTestTitle.Location = New System.Drawing.Point(3, 191)
    Me.pnlAdvancedNetTestTitle.Margin = New System.Windows.Forms.Padding(3, 10, 3, 5)
    Me.pnlAdvancedNetTestTitle.Name = "pnlAdvancedNetTestTitle"
    Me.pnlAdvancedNetTestTitle.RowCount = 1
    Me.pnlAdvancedNetTestTitle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAdvancedNetTestTitle.Size = New System.Drawing.Size(379, 13)
    Me.pnlAdvancedNetTestTitle.TabIndex = 2
    '
    'lblAdvancedNetTestTitle
    '
    Me.lblAdvancedNetTestTitle.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblAdvancedNetTestTitle.AutoSize = True
    Me.lblAdvancedNetTestTitle.Location = New System.Drawing.Point(3, 0)
    Me.lblAdvancedNetTestTitle.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
    Me.lblAdvancedNetTestTitle.Name = "lblAdvancedNetTestTitle"
    Me.lblAdvancedNetTestTitle.Size = New System.Drawing.Size(143, 13)
    Me.lblAdvancedNetTestTitle.TabIndex = 0
    Me.lblAdvancedNetTestTitle.Text = "Internet Speed Test Website"
    '
    'pnlAdvancedNetTest
    '
    Me.pnlAdvancedNetTest.AutoSize = True
    Me.pnlAdvancedNetTest.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlAdvancedNetTest.ColumnCount = 2
    Me.pnlAdvancedNetTest.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAdvancedNetTest.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAdvancedNetTest.Controls.Add(Me.pctAdvancedNetTestIcon, 0, 0)
    Me.pnlAdvancedNetTest.Controls.Add(Me.pnlAdvancedNetTestInput, 1, 0)
    Me.pnlAdvancedNetTest.Location = New System.Drawing.Point(3, 212)
    Me.pnlAdvancedNetTest.Name = "pnlAdvancedNetTest"
    Me.pnlAdvancedNetTest.RowCount = 1
    Me.pnlAdvancedNetTest.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAdvancedNetTest.Size = New System.Drawing.Size(377, 143)
    Me.pnlAdvancedNetTest.TabIndex = 3
    '
    'pctAdvancedNetTestIcon
    '
    Me.pctAdvancedNetTestIcon.Anchor = System.Windows.Forms.AnchorStyles.Top
    Me.pctAdvancedNetTestIcon.ErrorImage = Global.RestrictionTracker.My.Resources.Resources.advanced_nettest_error
    Me.pctAdvancedNetTestIcon.Image = Global.RestrictionTracker.My.Resources.Resources.advanced_nettest_edit
    Me.pctAdvancedNetTestIcon.InitialImage = Global.RestrictionTracker.My.Resources.Resources.advanced_nettest_load
    Me.pctAdvancedNetTestIcon.Location = New System.Drawing.Point(21, 3)
    Me.pctAdvancedNetTestIcon.Margin = New System.Windows.Forms.Padding(21, 3, 3, 3)
    Me.pctAdvancedNetTestIcon.Name = "pctAdvancedNetTestIcon"
    Me.pctAdvancedNetTestIcon.Size = New System.Drawing.Size(32, 32)
    Me.pctAdvancedNetTestIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.pctAdvancedNetTestIcon.TabIndex = 0
    Me.pctAdvancedNetTestIcon.TabStop = False
    '
    'pnlAdvancedNetTestInput
    '
    Me.pnlAdvancedNetTestInput.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlAdvancedNetTestInput.AutoSize = True
    Me.pnlAdvancedNetTestInput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlAdvancedNetTestInput.ColumnCount = 2
    Me.pnlAdvancedNetTestInput.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAdvancedNetTestInput.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAdvancedNetTestInput.Controls.Add(Me.lblAdvancedNetTestInput, 0, 0)
    Me.pnlAdvancedNetTestInput.Controls.Add(Me.optNetTestCustom, 0, 4)
    Me.pnlAdvancedNetTestInput.Controls.Add(Me.optNetTestNone, 0, 1)
    Me.pnlAdvancedNetTestInput.Controls.Add(Me.optNetTestTestMyNet, 0, 2)
    Me.pnlAdvancedNetTestInput.Controls.Add(Me.txtNetTestCustom, 1, 4)
    Me.pnlAdvancedNetTestInput.Controls.Add(Me.optNetTestSpeedTest, 0, 3)
    Me.pnlAdvancedNetTestInput.Location = New System.Drawing.Point(56, 0)
    Me.pnlAdvancedNetTestInput.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlAdvancedNetTestInput.Name = "pnlAdvancedNetTestInput"
    Me.pnlAdvancedNetTestInput.RowCount = 5
    Me.pnlAdvancedNetTestInput.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvancedNetTestInput.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvancedNetTestInput.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvancedNetTestInput.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvancedNetTestInput.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAdvancedNetTestInput.Size = New System.Drawing.Size(321, 143)
    Me.pnlAdvancedNetTestInput.TabIndex = 1
    '
    'lblAdvancedNetTestInput
    '
    Me.lblAdvancedNetTestInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblAdvancedNetTestInput.AutoSize = True
    Me.pnlAdvancedNetTestInput.SetColumnSpan(Me.lblAdvancedNetTestInput, 2)
    Me.lblAdvancedNetTestInput.Location = New System.Drawing.Point(3, 3)
    Me.lblAdvancedNetTestInput.Margin = New System.Windows.Forms.Padding(3)
    Me.lblAdvancedNetTestInput.Name = "lblAdvancedNetTestInput"
    Me.lblAdvancedNetTestInput.Size = New System.Drawing.Size(315, 39)
    Me.lblAdvancedNetTestInput.TabIndex = 0
    Me.lblAdvancedNetTestInput.Text = "You can add a link to an Internet Speed Test to the Main window" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "of Satellite Res" & _
    "triction Tracker if you wish to check your current" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "speeds when you look at your" & _
    " usage data."
    '
    'optNetTestCustom
    '
    Me.optNetTestCustom.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.optNetTestCustom.AutoSize = True
    Me.optNetTestCustom.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.optNetTestCustom.Location = New System.Drawing.Point(3, 121)
    Me.optNetTestCustom.Name = "optNetTestCustom"
    Me.optNetTestCustom.Size = New System.Drawing.Size(69, 18)
    Me.optNetTestCustom.TabIndex = 3
    Me.optNetTestCustom.TabStop = True
    Me.optNetTestCustom.Text = "C&ustom:"
    Me.ttConfig.SetToolTip(Me.optNetTestCustom, "Show a link to a custom website on the Main window.")
    Me.optNetTestCustom.UseVisualStyleBackColor = True
    '
    'optNetTestNone
    '
    Me.optNetTestNone.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.optNetTestNone.AutoSize = True
    Me.pnlAdvancedNetTestInput.SetColumnSpan(Me.optNetTestNone, 2)
    Me.optNetTestNone.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.optNetTestNone.Location = New System.Drawing.Point(3, 48)
    Me.optNetTestNone.Name = "optNetTestNone"
    Me.optNetTestNone.Size = New System.Drawing.Size(57, 18)
    Me.optNetTestNone.TabIndex = 1
    Me.optNetTestNone.TabStop = True
    Me.optNetTestNone.Text = "None"
    Me.ttConfig.SetToolTip(Me.optNetTestNone, "Don't show a link to any Internet Speed Test website.")
    Me.optNetTestNone.UseVisualStyleBackColor = True
    '
    'optNetTestTestMyNet
    '
    Me.optNetTestTestMyNet.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.optNetTestTestMyNet.AutoSize = True
    Me.pnlAdvancedNetTestInput.SetColumnSpan(Me.optNetTestTestMyNet, 2)
    Me.optNetTestTestMyNet.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.optNetTestTestMyNet.Location = New System.Drawing.Point(3, 72)
    Me.optNetTestTestMyNet.Name = "optNetTestTestMyNet"
    Me.optNetTestTestMyNet.Size = New System.Drawing.Size(86, 18)
    Me.optNetTestTestMyNet.TabIndex = 2
    Me.optNetTestTestMyNet.TabStop = True
    Me.optNetTestTestMyNet.Text = "TestMy.Net"
    Me.ttConfig.SetToolTip(Me.optNetTestTestMyNet, "Show a link to TestMy.Net on the Main window.")
    Me.optNetTestTestMyNet.UseVisualStyleBackColor = True
    '
    'txtNetTestCustom
    '
    Me.txtNetTestCustom.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtNetTestCustom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
    Me.txtNetTestCustom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl
    Me.txtNetTestCustom.Location = New System.Drawing.Point(78, 120)
    Me.txtNetTestCustom.Margin = New System.Windows.Forms.Padding(3, 3, 1, 3)
    Me.txtNetTestCustom.Name = "txtNetTestCustom"
    Me.txtNetTestCustom.Size = New System.Drawing.Size(242, 20)
    Me.txtNetTestCustom.TabIndex = 4
    Me.ttConfig.SetToolTip(Me.txtNetTestCustom, "Enter the URL of the site you want to link to on the Main window.")
    '
    'optNetTestSpeedTest
    '
    Me.optNetTestSpeedTest.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.optNetTestSpeedTest.AutoSize = True
    Me.pnlAdvancedNetTestInput.SetColumnSpan(Me.optNetTestSpeedTest, 2)
    Me.optNetTestSpeedTest.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.optNetTestSpeedTest.Location = New System.Drawing.Point(3, 96)
    Me.optNetTestSpeedTest.Name = "optNetTestSpeedTest"
    Me.optNetTestSpeedTest.Size = New System.Drawing.Size(101, 18)
    Me.optNetTestSpeedTest.TabIndex = 2
    Me.optNetTestSpeedTest.TabStop = True
    Me.optNetTestSpeedTest.Text = "SpeedTest.net"
    Me.ttConfig.SetToolTip(Me.optNetTestSpeedTest, "Show a link to SpeedTest.net on the Main window.")
    Me.optNetTestSpeedTest.UseVisualStyleBackColor = True
    '
    'cmdSave
    '
    Me.cmdSave.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdSave.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdSave.Location = New System.Drawing.Point(3, 3)
    Me.cmdSave.Name = "cmdSave"
    Me.cmdSave.Size = New System.Drawing.Size(74, 23)
    Me.cmdSave.TabIndex = 1
    Me.cmdSave.Text = "Save"
    Me.cmdSave.UseVisualStyleBackColor = True
    '
    'cmdClose
    '
    Me.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdClose.Location = New System.Drawing.Point(83, 3)
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.Size = New System.Drawing.Size(74, 23)
    Me.cmdClose.TabIndex = 2
    Me.cmdClose.Text = "Close"
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'pnlConfig
    '
    Me.pnlConfig.ColumnCount = 1
    Me.pnlConfig.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlConfig.Controls.Add(Me.pnlButtons, 0, 1)
    Me.pnlConfig.Controls.Add(Me.tbsConfig, 0, 0)
    Me.pnlConfig.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlConfig.Location = New System.Drawing.Point(0, 0)
    Me.pnlConfig.Name = "pnlConfig"
    Me.pnlConfig.RowCount = 2
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlConfig.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlConfig.Size = New System.Drawing.Size(405, 541)
    Me.pnlConfig.TabIndex = 4
    '
    'pnlButtons
    '
    Me.pnlButtons.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.pnlButtons.AutoSize = True
    Me.pnlButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlButtons.ColumnCount = 2
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.Controls.Add(Me.cmdClose, 1, 0)
    Me.pnlButtons.Controls.Add(Me.cmdSave, 0, 0)
    Me.pnlButtons.Location = New System.Drawing.Point(245, 512)
    Me.pnlButtons.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.RowCount = 1
    Me.pnlButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlButtons.Size = New System.Drawing.Size(160, 29)
    Me.pnlButtons.TabIndex = 5
    '
    'fswController
    '
    Me.fswController.EnableRaisingEvents = True
    Me.fswController.SynchronizingObject = Me
    '
    'tmrIcoWait
    '
    Me.tmrIcoWait.Interval = 4000
    '
    'mnuKey
    '
    Me.mnuKey.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuKeyCut, Me.mnuKeyCopy, Me.mnuKeyPaste, Me.mnuKeySpacer, Me.mnuKeyDelete, Me.mnuKeyClear})
    '
    'mnuKeyCut
    '
    Me.mnuKeyCut.Index = 0
    Me.mnuKeyCut.Text = "C&ut"
    '
    'mnuKeyCopy
    '
    Me.mnuKeyCopy.Index = 1
    Me.mnuKeyCopy.Text = "&Copy"
    '
    'mnuKeyPaste
    '
    Me.mnuKeyPaste.Index = 2
    Me.mnuKeyPaste.Text = "&Paste"
    '
    'mnuKeySpacer
    '
    Me.mnuKeySpacer.Index = 3
    Me.mnuKeySpacer.Text = "-"
    '
    'mnuKeyDelete
    '
    Me.mnuKeyDelete.Index = 4
    Me.mnuKeyDelete.Text = "&Delete"
    '
    'mnuKeyClear
    '
    Me.mnuKeyClear.Index = 5
    Me.mnuKeyClear.Text = "C&lear"
    '
    'lnAccountViaSatTitle
    '
    Me.lnAccountViaSatTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnAccountViaSatTitle.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnAccountViaSatTitle.CausesValidation = False
    Me.lnAccountViaSatTitle.Location = New System.Drawing.Point(86, 4)
    Me.lnAccountViaSatTitle.Margin = New System.Windows.Forms.Padding(2, 3, 5, 3)
    Me.lnAccountViaSatTitle.Name = "lnAccountViaSatTitle"
    Me.lnAccountViaSatTitle.Padding = New System.Windows.Forms.Padding(0, 2, 0, 2)
    Me.lnAccountViaSatTitle.Size = New System.Drawing.Size(288, 4)
    Me.lnAccountViaSatTitle.TabIndex = 1
    Me.lnAccountViaSatTitle.TabStop = False
    '
    'txtPassword
    '
    Me.txtPassword.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.txtPassword.Location = New System.Drawing.Point(67, 48)
    Me.txtPassword.Name = "txtPassword"
    Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
    Me.txtPassword.ShowContents = False
    Me.txtPassword.Size = New System.Drawing.Size(150, 20)
    Me.txtPassword.TabIndex = 4
    Me.ttConfig.SetToolTip(Me.txtPassword, "The Password to your ViaSat account.")
    '
    'lnAccountShareTitle
    '
    Me.lnAccountShareTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnAccountShareTitle.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnAccountShareTitle.CausesValidation = False
    Me.lnAccountShareTitle.Location = New System.Drawing.Point(193, 4)
    Me.lnAccountShareTitle.Margin = New System.Windows.Forms.Padding(2, 3, 5, 3)
    Me.lnAccountShareTitle.Name = "lnAccountShareTitle"
    Me.lnAccountShareTitle.Padding = New System.Windows.Forms.Padding(0, 2, 0, 2)
    Me.lnAccountShareTitle.Size = New System.Drawing.Size(181, 4)
    Me.lnAccountShareTitle.TabIndex = 1
    Me.lnAccountShareTitle.TabStop = False
    '
    'lnPrefInterfaceTitle
    '
    Me.lnPrefInterfaceTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnPrefInterfaceTitle.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnPrefInterfaceTitle.CausesValidation = False
    Me.lnPrefInterfaceTitle.Location = New System.Drawing.Point(161, 4)
    Me.lnPrefInterfaceTitle.Margin = New System.Windows.Forms.Padding(2, 3, 5, 3)
    Me.lnPrefInterfaceTitle.Name = "lnPrefInterfaceTitle"
    Me.lnPrefInterfaceTitle.Padding = New System.Windows.Forms.Padding(0, 2, 0, 2)
    Me.lnPrefInterfaceTitle.Size = New System.Drawing.Size(213, 4)
    Me.lnPrefInterfaceTitle.TabIndex = 1
    Me.lnPrefInterfaceTitle.TabStop = False
    '
    'txtOverTime
    '
    Me.txtOverTime.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.txtOverTime.LargeIncrement = CType(5UI, UInteger)
    Me.txtOverTime.Location = New System.Drawing.Point(175, 74)
    Me.txtOverTime.Maximum = New Decimal(New Integer() {360, 0, 0, 0})
    Me.txtOverTime.Minimum = New Decimal(New Integer() {15, 0, 0, 0})
    Me.txtOverTime.Name = "txtOverTime"
    Me.txtOverTime.Size = New System.Drawing.Size(50, 20)
    Me.txtOverTime.TabIndex = 7
    Me.txtOverTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    Me.ttConfig.SetToolTip(Me.txtOverTime, "Enter the duration of time to check for the defined usage (in minutes).")
    Me.txtOverTime.Value = New Decimal(New Integer() {15, 0, 0, 0})
    '
    'txtOverSize
    '
    Me.txtOverSize.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.txtOverSize.LargeIncrement = CType(100UI, UInteger)
    Me.txtOverSize.Location = New System.Drawing.Point(175, 48)
    Me.txtOverSize.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
    Me.txtOverSize.Minimum = New Decimal(New Integer() {25, 0, 0, 0})
    Me.txtOverSize.Name = "txtOverSize"
    Me.txtOverSize.Size = New System.Drawing.Size(50, 20)
    Me.txtOverSize.TabIndex = 4
    Me.txtOverSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    Me.ttConfig.SetToolTip(Me.txtOverSize, "Enter the amount of usage to display an alert about (in Megabytes).")
    Me.txtOverSize.Value = New Decimal(New Integer() {100, 0, 0, 0})
    '
    'txtInterval
    '
    Me.txtInterval.LargeIncrement = CType(5UI, UInteger)
    Me.txtInterval.Location = New System.Drawing.Point(50, 3)
    Me.txtInterval.Maximum = New Decimal(New Integer() {1440, 0, 0, 0})
    Me.txtInterval.Minimum = New Decimal(New Integer() {15, 0, 0, 0})
    Me.txtInterval.Name = "txtInterval"
    Me.txtInterval.Size = New System.Drawing.Size(50, 20)
    Me.txtInterval.TabIndex = 1
    Me.txtInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    Me.ttConfig.SetToolTip(Me.txtInterval, "Interval between meter checks in minutes.")
    Me.txtInterval.Value = New Decimal(New Integer() {15, 0, 0, 0})
    '
    'txtAccuracy
    '
    Me.txtAccuracy.LargeIncrement = CType(1UI, UInteger)
    Me.txtAccuracy.Location = New System.Drawing.Point(50, 29)
    Me.txtAccuracy.Maximum = New Decimal(New Integer() {3, 0, 0, 0})
    Me.txtAccuracy.Name = "txtAccuracy"
    Me.txtAccuracy.Size = New System.Drawing.Size(50, 20)
    Me.txtAccuracy.TabIndex = 4
    Me.txtAccuracy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    Me.ttConfig.SetToolTip(Me.txtAccuracy, "Number of decimal places to display.")
    '
    'lnPrefStartTitle
    '
    Me.lnPrefStartTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnPrefStartTitle.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnPrefStartTitle.CausesValidation = False
    Me.lnPrefStartTitle.Location = New System.Drawing.Point(114, 4)
    Me.lnPrefStartTitle.Margin = New System.Windows.Forms.Padding(2, 3, 5, 3)
    Me.lnPrefStartTitle.Name = "lnPrefStartTitle"
    Me.lnPrefStartTitle.Padding = New System.Windows.Forms.Padding(0, 2, 0, 2)
    Me.lnPrefStartTitle.Size = New System.Drawing.Size(260, 4)
    Me.lnPrefStartTitle.TabIndex = 1
    Me.lnPrefStartTitle.TabStop = False
    '
    'lnPrefAccuracyTitle
    '
    Me.lnPrefAccuracyTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnPrefAccuracyTitle.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnPrefAccuracyTitle.CausesValidation = False
    Me.lnPrefAccuracyTitle.Location = New System.Drawing.Point(91, 4)
    Me.lnPrefAccuracyTitle.Margin = New System.Windows.Forms.Padding(2, 3, 5, 3)
    Me.lnPrefAccuracyTitle.Name = "lnPrefAccuracyTitle"
    Me.lnPrefAccuracyTitle.Padding = New System.Windows.Forms.Padding(0, 2, 0, 2)
    Me.lnPrefAccuracyTitle.Size = New System.Drawing.Size(283, 4)
    Me.lnPrefAccuracyTitle.TabIndex = 1
    Me.lnPrefAccuracyTitle.TabStop = False
    '
    'lnPrefAlertTitle
    '
    Me.lnPrefAlertTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnPrefAlertTitle.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnPrefAlertTitle.CausesValidation = False
    Me.lnPrefAlertTitle.Location = New System.Drawing.Point(67, 4)
    Me.lnPrefAlertTitle.Margin = New System.Windows.Forms.Padding(2, 3, 5, 3)
    Me.lnPrefAlertTitle.Name = "lnPrefAlertTitle"
    Me.lnPrefAlertTitle.Padding = New System.Windows.Forms.Padding(0, 2, 0, 2)
    Me.lnPrefAlertTitle.Size = New System.Drawing.Size(307, 4)
    Me.lnPrefAlertTitle.TabIndex = 1
    Me.lnPrefAlertTitle.TabStop = False
    '
    'lnPrefColorTitle
    '
    Me.lnPrefColorTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnPrefColorTitle.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnPrefColorTitle.CausesValidation = False
    Me.lnPrefColorTitle.Location = New System.Drawing.Point(73, 4)
    Me.lnPrefColorTitle.Margin = New System.Windows.Forms.Padding(2, 3, 5, 3)
    Me.lnPrefColorTitle.Name = "lnPrefColorTitle"
    Me.lnPrefColorTitle.Padding = New System.Windows.Forms.Padding(0, 2, 0, 2)
    Me.lnPrefColorTitle.Size = New System.Drawing.Size(301, 4)
    Me.lnPrefColorTitle.TabIndex = 1
    Me.lnPrefColorTitle.TabStop = False
    '
    'txtStartWait
    '
    Me.txtStartWait.LargeIncrement = CType(1UI, UInteger)
    Me.txtStartWait.Location = New System.Drawing.Point(38, 27)
    Me.txtStartWait.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
    Me.txtStartWait.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
    Me.txtStartWait.Name = "txtStartWait"
    Me.txtStartWait.Size = New System.Drawing.Size(50, 20)
    Me.txtStartWait.TabIndex = 2
    Me.txtStartWait.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    Me.ttConfig.SetToolTip(Me.txtStartWait, "Interval before first meter check in minutes.")
    Me.txtStartWait.Value = New Decimal(New Integer() {5, 0, 0, 0})
    '
    'lnNetworkProtocolTitle
    '
    Me.lnNetworkProtocolTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnNetworkProtocolTitle.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnNetworkProtocolTitle.CausesValidation = False
    Me.lnNetworkProtocolTitle.Location = New System.Drawing.Point(92, 4)
    Me.lnNetworkProtocolTitle.Margin = New System.Windows.Forms.Padding(2, 3, 5, 3)
    Me.lnNetworkProtocolTitle.Name = "lnNetworkProtocolTitle"
    Me.lnNetworkProtocolTitle.Padding = New System.Windows.Forms.Padding(0, 2, 0, 2)
    Me.lnNetworkProtocolTitle.Size = New System.Drawing.Size(282, 4)
    Me.lnNetworkProtocolTitle.TabIndex = 1
    Me.lnNetworkProtocolTitle.TabStop = False
    '
    'txtProxyPassword
    '
    Me.txtProxyPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtProxyPassword.Location = New System.Drawing.Point(138, 82)
    Me.txtProxyPassword.Name = "txtProxyPassword"
    Me.txtProxyPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
    Me.txtProxyPassword.ShowContents = False
    Me.txtProxyPassword.Size = New System.Drawing.Size(133, 20)
    Me.txtProxyPassword.TabIndex = 9
    Me.ttConfig.SetToolTip(Me.txtProxyPassword, "Optional Password for HTTP Proxy authentication.")
    '
    'txtProxyPort
    '
    Me.txtProxyPort.LargeIncrement = CType(20UI, UInteger)
    Me.txtProxyPort.Location = New System.Drawing.Point(138, 43)
    Me.txtProxyPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
    Me.txtProxyPort.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
    Me.txtProxyPort.Name = "txtProxyPort"
    Me.txtProxyPort.Size = New System.Drawing.Size(50, 20)
    Me.txtProxyPort.TabIndex = 5
    Me.txtProxyPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    Me.ttConfig.SetToolTip(Me.txtProxyPort, "Port to connect to HTTP proxy over.")
    Me.txtProxyPort.Value = New Decimal(New Integer() {8080, 0, 0, 0})
    '
    'lnNetworkProxyTitle
    '
    Me.lnNetworkProxyTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnNetworkProxyTitle.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnNetworkProxyTitle.CausesValidation = False
    Me.lnNetworkProxyTitle.Location = New System.Drawing.Point(70, 4)
    Me.lnNetworkProxyTitle.Margin = New System.Windows.Forms.Padding(2, 3, 5, 3)
    Me.lnNetworkProxyTitle.Name = "lnNetworkProxyTitle"
    Me.lnNetworkProxyTitle.Padding = New System.Windows.Forms.Padding(0, 2, 0, 2)
    Me.lnNetworkProxyTitle.Size = New System.Drawing.Size(304, 4)
    Me.lnNetworkProxyTitle.TabIndex = 1
    Me.lnNetworkProxyTitle.TabStop = False
    '
    'lnNetworkTimeoutTitle
    '
    Me.lnNetworkTimeoutTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnNetworkTimeoutTitle.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnNetworkTimeoutTitle.CausesValidation = False
    Me.lnNetworkTimeoutTitle.Location = New System.Drawing.Point(93, 4)
    Me.lnNetworkTimeoutTitle.Margin = New System.Windows.Forms.Padding(2, 3, 5, 3)
    Me.lnNetworkTimeoutTitle.Name = "lnNetworkTimeoutTitle"
    Me.lnNetworkTimeoutTitle.Padding = New System.Windows.Forms.Padding(0, 2, 0, 2)
    Me.lnNetworkTimeoutTitle.Size = New System.Drawing.Size(281, 4)
    Me.lnNetworkTimeoutTitle.TabIndex = 1
    Me.lnNetworkTimeoutTitle.TabStop = False
    '
    'txtTimeout
    '
    Me.txtTimeout.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.txtTimeout.LargeIncrement = CType(15UI, UInteger)
    Me.txtTimeout.Location = New System.Drawing.Point(113, 35)
    Me.txtTimeout.Maximum = New Decimal(New Integer() {600, 0, 0, 0})
    Me.txtTimeout.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
    Me.txtTimeout.Name = "txtTimeout"
    Me.txtTimeout.Size = New System.Drawing.Size(50, 20)
    Me.txtTimeout.TabIndex = 2
    Me.txtTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    Me.ttConfig.SetToolTip(Me.txtTimeout, "Number of seconds to wait between network communications.")
    Me.txtTimeout.Value = New Decimal(New Integer() {60, 0, 0, 0})
    '
    'txtRetries
    '
    Me.txtRetries.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.txtRetries.LargeIncrement = CType(3UI, UInteger)
    Me.txtRetries.Location = New System.Drawing.Point(289, 35)
    Me.txtRetries.Maximum = New Decimal(New Integer() {9, 0, 0, 0})
    Me.txtRetries.Name = "txtRetries"
    Me.txtRetries.Size = New System.Drawing.Size(50, 20)
    Me.txtRetries.TabIndex = 5
    Me.txtRetries.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    Me.ttConfig.SetToolTip(Me.txtRetries, resources.GetString("txtRetries.ToolTip"))
    Me.txtRetries.Value = New Decimal(New Integer() {2, 0, 0, 0})
    '
    'lnNetworkUpdateTitle
    '
    Me.lnNetworkUpdateTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnNetworkUpdateTitle.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnNetworkUpdateTitle.CausesValidation = False
    Me.lnNetworkUpdateTitle.Location = New System.Drawing.Point(52, 4)
    Me.lnNetworkUpdateTitle.Margin = New System.Windows.Forms.Padding(2, 3, 5, 3)
    Me.lnNetworkUpdateTitle.Name = "lnNetworkUpdateTitle"
    Me.lnNetworkUpdateTitle.Padding = New System.Windows.Forms.Padding(0, 2, 0, 2)
    Me.lnNetworkUpdateTitle.Size = New System.Drawing.Size(322, 4)
    Me.lnNetworkUpdateTitle.TabIndex = 1
    Me.lnNetworkUpdateTitle.TabStop = False
    '
    'lnAdvancedPortableTitle
    '
    Me.lnAdvancedPortableTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnAdvancedPortableTitle.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnAdvancedPortableTitle.CausesValidation = False
    Me.lnAdvancedPortableTitle.Location = New System.Drawing.Point(106, 4)
    Me.lnAdvancedPortableTitle.Margin = New System.Windows.Forms.Padding(2, 3, 5, 3)
    Me.lnAdvancedPortableTitle.Name = "lnAdvancedPortableTitle"
    Me.lnAdvancedPortableTitle.Padding = New System.Windows.Forms.Padding(0, 2, 0, 2)
    Me.lnAdvancedPortableTitle.Size = New System.Drawing.Size(268, 4)
    Me.lnAdvancedPortableTitle.TabIndex = 1
    Me.lnAdvancedPortableTitle.TabStop = False
    '
    'lnAdvancedDataTitle
    '
    Me.lnAdvancedDataTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnAdvancedDataTitle.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnAdvancedDataTitle.CausesValidation = False
    Me.lnAdvancedDataTitle.Location = New System.Drawing.Point(80, 4)
    Me.lnAdvancedDataTitle.Margin = New System.Windows.Forms.Padding(2, 3, 5, 3)
    Me.lnAdvancedDataTitle.Name = "lnAdvancedDataTitle"
    Me.lnAdvancedDataTitle.Padding = New System.Windows.Forms.Padding(0, 2, 0, 2)
    Me.lnAdvancedDataTitle.Size = New System.Drawing.Size(294, 4)
    Me.lnAdvancedDataTitle.TabIndex = 1
    Me.lnAdvancedDataTitle.TabStop = False
    '
    'lnAdvancedNetTestTitle
    '
    Me.lnAdvancedNetTestTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lnAdvancedNetTestTitle.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    Me.lnAdvancedNetTestTitle.CausesValidation = False
    Me.lnAdvancedNetTestTitle.Location = New System.Drawing.Point(148, 4)
    Me.lnAdvancedNetTestTitle.Margin = New System.Windows.Forms.Padding(2, 3, 5, 3)
    Me.lnAdvancedNetTestTitle.Name = "lnAdvancedNetTestTitle"
    Me.lnAdvancedNetTestTitle.Padding = New System.Windows.Forms.Padding(0, 2, 0, 2)
    Me.lnAdvancedNetTestTitle.Size = New System.Drawing.Size(226, 4)
    Me.lnAdvancedNetTestTitle.TabIndex = 1
    Me.lnAdvancedNetTestTitle.TabStop = False
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
    Me.CancelButton = Me.cmdClose
    Me.ClientSize = New System.Drawing.Size(405, 541)
    Me.Controls.Add(Me.pnlConfig)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmConfig"
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    Me.Text = "Satellite Restriction Tracker Configuration"
    Me.tbsConfig.ResumeLayout(False)
    Me.tabAccount.ResumeLayout(False)
    Me.pnlAccount.ResumeLayout(False)
    Me.pnlAccount.PerformLayout()
    Me.pnlAccountViaSatTitle.ResumeLayout(False)
    Me.pnlAccountViaSatTitle.PerformLayout()
    Me.pnlAccountViaSat.ResumeLayout(False)
    Me.pnlAccountViaSat.PerformLayout()
    CType(Me.pctAccountViaSatIcon, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlAccountViaSatInput.ResumeLayout(False)
    Me.pnlAccountViaSatInput.PerformLayout()
    Me.pnlAccountShareTitle.ResumeLayout(False)
    Me.pnlAccountShareTitle.PerformLayout()
    Me.tabPrefs.ResumeLayout(False)
    Me.pnlPrefs.ResumeLayout(False)
    Me.pnlPrefs.PerformLayout()
    Me.pnlPrefInterfaceTitle.ResumeLayout(False)
    Me.pnlPrefInterfaceTitle.PerformLayout()
    Me.pnlPrefColor.ResumeLayout(False)
    Me.pnlPrefColor.PerformLayout()
    CType(Me.pctPrefColorIcon, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlPrefAlert.ResumeLayout(False)
    Me.pnlPrefAlert.PerformLayout()
    CType(Me.pctPrefAlertIcon, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlPrefAccuracy.ResumeLayout(False)
    Me.pnlPrefAccuracy.PerformLayout()
    CType(Me.pctPrefAccuracyIcon, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlPrefAccuracyInput.ResumeLayout(False)
    Me.pnlPrefAccuracyInput.PerformLayout()
    Me.pnlPrefStartTitle.ResumeLayout(False)
    Me.pnlPrefStartTitle.PerformLayout()
    Me.pnlPrefAccuracyTitle.ResumeLayout(False)
    Me.pnlPrefAccuracyTitle.PerformLayout()
    Me.pnlPrefAlertTitle.ResumeLayout(False)
    Me.pnlPrefAlertTitle.PerformLayout()
    Me.pnlPrefColorTitle.ResumeLayout(False)
    Me.pnlPrefColorTitle.PerformLayout()
    Me.pnlPrefStart.ResumeLayout(False)
    Me.pnlPrefStart.PerformLayout()
    CType(Me.pctPrefStartIcon, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlPrefStartInput.ResumeLayout(False)
    Me.pnlPrefStartInput.PerformLayout()
    Me.pnlPrefInterface.ResumeLayout(False)
    Me.pnlPrefInterface.PerformLayout()
    CType(Me.pctPrefInterfaceIcon, System.ComponentModel.ISupportInitialize).EndInit()
    Me.tabNetwork.ResumeLayout(False)
    Me.pnlNetwork.ResumeLayout(False)
    Me.pnlNetwork.PerformLayout()
    Me.pnlNetworkProtocol.ResumeLayout(False)
    Me.pnlNetworkProtocol.PerformLayout()
    CType(Me.pctNetworkProtocolIcon, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlNetworkProtocolTitle.ResumeLayout(False)
    Me.pnlNetworkProtocolTitle.PerformLayout()
    Me.pnlNetworkUpdate.ResumeLayout(False)
    Me.pnlNetworkUpdate.PerformLayout()
    CType(Me.pctNetworkUpdateIcon, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlNetworkUpdateTime.ResumeLayout(False)
    Me.pnlNetworkUpdateTime.PerformLayout()
    Me.pnlNetworkProxy.ResumeLayout(False)
    Me.pnlNetworkProxy.PerformLayout()
    Me.pnlProxy.ResumeLayout(False)
    Me.pnlProxy.PerformLayout()
    CType(Me.pctNetworkProxyIcon, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlNetworkProxyTitle.ResumeLayout(False)
    Me.pnlNetworkProxyTitle.PerformLayout()
    Me.pnlNetworkTimeoutTitle.ResumeLayout(False)
    Me.pnlNetworkTimeoutTitle.PerformLayout()
    Me.pnlNetworkTimeout.ResumeLayout(False)
    Me.pnlNetworkTimeout.PerformLayout()
    CType(Me.pctNetworkTimeoutIcon, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlNetworkUpdateTitle.ResumeLayout(False)
    Me.pnlNetworkUpdateTitle.PerformLayout()
    Me.tabAdvanced.ResumeLayout(False)
    Me.pnlAdvanced.ResumeLayout(False)
    Me.pnlAdvanced.PerformLayout()
    Me.pnlAdvancedPortable.ResumeLayout(False)
    Me.pnlAdvancedPortable.PerformLayout()
    Me.pnlPortableDir.ResumeLayout(False)
    Me.pnlPortableDir.PerformLayout()
    CType(Me.pctAdvancedPortableIcon, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlAdvancedPortableTitle.ResumeLayout(False)
    Me.pnlAdvancedPortableTitle.PerformLayout()
    Me.pnlAdvancedData.ResumeLayout(False)
    Me.pnlAdvancedData.PerformLayout()
    CType(Me.pctAdvancedDataIcon, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlAdvancedDataInput.ResumeLayout(False)
    Me.pnlAdvancedDataInput.PerformLayout()
    Me.pnlHistoryDir.ResumeLayout(False)
    Me.pnlHistoryDir.PerformLayout()
    Me.pnlAdvancedDataTitle.ResumeLayout(False)
    Me.pnlAdvancedDataTitle.PerformLayout()
    Me.pnlAdvancedNetTestTitle.ResumeLayout(False)
    Me.pnlAdvancedNetTestTitle.PerformLayout()
    Me.pnlAdvancedNetTest.ResumeLayout(False)
    Me.pnlAdvancedNetTest.PerformLayout()
    CType(Me.pctAdvancedNetTestIcon, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlAdvancedNetTestInput.ResumeLayout(False)
    Me.pnlAdvancedNetTestInput.PerformLayout()
    Me.pnlConfig.ResumeLayout(False)
    Me.pnlConfig.PerformLayout()
    Me.pnlButtons.ResumeLayout(False)
    CType(Me.fswController, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtOverTime, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtOverSize, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtInterval, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtAccuracy, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtStartWait, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtProxyPort, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtTimeout, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.txtRetries, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents tbsConfig As System.Windows.Forms.TabControl
  Friend WithEvents tabAccount As System.Windows.Forms.TabPage
  Friend WithEvents tabPrefs As System.Windows.Forms.TabPage
  Friend WithEvents cmdSave As System.Windows.Forms.Button
  Friend WithEvents cmdClose As System.Windows.Forms.Button
  Friend WithEvents pnlConfig As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlButtons As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents tabNetwork As System.Windows.Forms.TabPage
  Friend WithEvents pnlNetwork As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlNetworkTimeoutTitle As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblNetworkTimeoutTitle As System.Windows.Forms.Label
  Friend WithEvents lnNetworkTimeoutTitle As RestrictionTracker.LineBreak
  Friend WithEvents pnlNetworkTimeout As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents txtTimeout As NumericUpDownIncrementable
  Friend WithEvents lblTimeout2 As System.Windows.Forms.Label
  Friend WithEvents pctNetworkTimeoutIcon As System.Windows.Forms.PictureBox
  Friend WithEvents lblTimeout1 As System.Windows.Forms.Label
  Friend WithEvents lblNetworkTimeoutDescription As System.Windows.Forms.Label
  Friend WithEvents pnlNetworkProxy As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctNetworkProxyIcon As System.Windows.Forms.PictureBox
  Friend WithEvents lblNetworkProxyDescrption As System.Windows.Forms.Label
  Friend WithEvents pnlNetworkProxyTitle As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblNetworkProxyTitle As System.Windows.Forms.Label
  Friend WithEvents lnNetworkProxyTitle As RestrictionTracker.LineBreak
  Friend WithEvents pnlProxy As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblProxyType As System.Windows.Forms.Label
  Friend WithEvents txtProxyUser As System.Windows.Forms.TextBox
  Friend WithEvents lblProxyUser As System.Windows.Forms.Label
  Friend WithEvents txtProxyPassword As PasswordBox
  Friend WithEvents lblProxyPassword As System.Windows.Forms.Label
  Friend WithEvents lblProxyAddr As System.Windows.Forms.Label
  Friend WithEvents txtProxyAddress As System.Windows.Forms.TextBox
  Friend WithEvents lblProxyPort As System.Windows.Forms.Label
  Friend WithEvents lblProxyDomain As System.Windows.Forms.Label
  Friend WithEvents txtProxyDomain As System.Windows.Forms.TextBox
  Friend WithEvents cmbProxyType As System.Windows.Forms.ComboBox
  Friend WithEvents txtProxyPort As NumericUpDownIncrementable
  Friend WithEvents pnlNetworkUpdate As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctNetworkUpdateIcon As System.Windows.Forms.PictureBox
  Friend WithEvents pnlNetworkUpdateTitle As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblNetworkUpdateTitle As System.Windows.Forms.Label
  Friend WithEvents lnNetworkUpdateTitle As RestrictionTracker.LineBreak
  Friend WithEvents pnlPrefs As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlPrefStartTitle As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblPrefStartTitle As System.Windows.Forms.Label
  Friend WithEvents lnPrefStartTitle As RestrictionTracker.LineBreak
  Friend WithEvents pnlPrefAccuracyTitle As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblPrefAccuracyTitle As System.Windows.Forms.Label
  Friend WithEvents lnPrefAccuracyTitle As RestrictionTracker.LineBreak
  Friend WithEvents pnlPrefAlertTitle As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lnPrefAlertTitle As RestrictionTracker.LineBreak
  Friend WithEvents pnlPrefStart As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctPrefStartIcon As System.Windows.Forms.PictureBox
  Friend WithEvents pnlPrefStartInput As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents chkStartUp As System.Windows.Forms.CheckBox
  Friend WithEvents lblStartWait1 As System.Windows.Forms.Label
  Friend WithEvents txtStartWait As NumericUpDownIncrementable
  Friend WithEvents lblStartWait2 As System.Windows.Forms.Label
  Friend WithEvents chkService As System.Windows.Forms.CheckBox
  Friend WithEvents pnlPrefAccuracy As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctPrefAccuracyIcon As System.Windows.Forms.PictureBox
  Friend WithEvents pnlPrefAccuracyInput As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblInterval1 As System.Windows.Forms.Label
  Friend WithEvents txtInterval As NumericUpDownIncrementable
  Friend WithEvents lblInterval2 As System.Windows.Forms.Label
  Friend WithEvents lblAccuracy1 As System.Windows.Forms.Label
  Friend WithEvents txtAccuracy As NumericUpDownIncrementable
  Friend WithEvents lblAccuracy2 As System.Windows.Forms.Label
  Friend WithEvents pnlPrefColor As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctPrefColorIcon As System.Windows.Forms.PictureBox
  Friend WithEvents lblPrefColorDescription As System.Windows.Forms.Label
  Friend WithEvents pnlPrefAlert As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctPrefAlertIcon As System.Windows.Forms.PictureBox
  Friend WithEvents txtOverSize As NumericUpDownIncrementable
  Friend WithEvents lblOverSize2 As System.Windows.Forms.Label
  Friend WithEvents lblPrefAlertDescription As System.Windows.Forms.Label
  Friend WithEvents cmdColors As System.Windows.Forms.Button
  Friend WithEvents lblPrefAlertTitle As System.Windows.Forms.Label
  Friend WithEvents chkOverAlert As System.Windows.Forms.CheckBox
  Friend WithEvents lblOverTime1 As System.Windows.Forms.Label
  Friend WithEvents txtOverTime As NumericUpDownIncrementable
  Friend WithEvents lblOverTime2 As System.Windows.Forms.Label
  Friend WithEvents cmdAlertStyle As System.Windows.Forms.Button
  Friend WithEvents lblOverSize1 As System.Windows.Forms.Label
  Friend WithEvents pnlAccount As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlAccountViaSatTitle As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblAccountViaSatTitle As System.Windows.Forms.Label
  Friend WithEvents lnAccountViaSatTitle As RestrictionTracker.LineBreak
  Friend WithEvents pnlAccountViaSat As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctAccountViaSatIcon As System.Windows.Forms.PictureBox
  Friend WithEvents pnlAccountViaSatInput As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblAccount As System.Windows.Forms.Label
  Friend WithEvents lblPassword As System.Windows.Forms.Label
  Friend WithEvents txtAccount As System.Windows.Forms.TextBox
  Friend WithEvents txtPassword As PasswordBox
  Friend WithEvents lblAccountViaSatDescription As System.Windows.Forms.Label
  Friend WithEvents pnlNetworkUpdateTime As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblUpdateInterval As System.Windows.Forms.Label
  Friend WithEvents cmbUpdateInterval As System.Windows.Forms.ComboBox
  Friend WithEvents tabAdvanced As System.Windows.Forms.TabPage
  Friend WithEvents pnlAdvanced As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlAdvancedData As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctAdvancedDataIcon As System.Windows.Forms.PictureBox
  Friend WithEvents pnlAdvancedDataInput As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pnlHistoryDir As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents txtHistoryDir As System.Windows.Forms.TextBox
  Friend WithEvents cmdHistoryDir As System.Windows.Forms.Button
  Friend WithEvents lblAdvancedDataDescription As System.Windows.Forms.Label
  Friend WithEvents pnlAdvancedDataTitle As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblAdvancedDataTitle As System.Windows.Forms.Label
  Friend WithEvents lnAdvancedDataTitle As RestrictionTracker.LineBreak
  Friend WithEvents optHistoryCustom As System.Windows.Forms.RadioButton
  Friend WithEvents optHistoryProgramData As System.Windows.Forms.RadioButton
  Friend WithEvents optHistoryAppData As System.Windows.Forms.RadioButton
  Friend WithEvents cmdHistoryDirOpen As System.Windows.Forms.Button
  Friend WithEvents pnlAdvancedPortableTitle As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblAdvancedPortableTitle As System.Windows.Forms.Label
  Friend WithEvents lnAdvancedPortableTitle As RestrictionTracker.LineBreak
  Friend WithEvents pnlAdvancedPortable As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctAdvancedPortableIcon As System.Windows.Forms.PictureBox
  Friend WithEvents lblAdvancedPortableDescription As System.Windows.Forms.Label
  Friend WithEvents cmdMakePortable As System.Windows.Forms.Button
  Friend WithEvents lblPortableDir As System.Windows.Forms.Label
  Friend WithEvents fswController As System.IO.FileSystemWatcher
  Friend WithEvents ttConfig As RestrictionTracker.ToolTip
  Friend WithEvents pnlPortableDir As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents txtPortableDir As System.Windows.Forms.TextBox
  Friend WithEvents cmdPortableDir As System.Windows.Forms.Button
  Friend WithEvents pnlNetworkProtocol As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctNetworkProtocolIcon As System.Windows.Forms.PictureBox
  Friend WithEvents chkNetworkProtocolSSL3 As System.Windows.Forms.CheckBox
  Friend WithEvents pnlNetworkProtocolTitle As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblNetworkProtocolTitle As System.Windows.Forms.Label
  Friend WithEvents lnNetworkProtocolTitle As RestrictionTracker.LineBreak
  Friend WithEvents chkUpdateBETA As System.Windows.Forms.CheckBox
  Friend WithEvents cmbUpdateAutomation As System.Windows.Forms.ComboBox
  Friend WithEvents chkAutoHide As System.Windows.Forms.CheckBox
  Friend WithEvents pnlPrefInterfaceTitle As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblPrefInterfaceTitle As System.Windows.Forms.Label
  Friend WithEvents lnPrefInterfaceTitle As RestrictionTracker.LineBreak
  Friend WithEvents pnlPrefColorTitle As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblPrefColorTitle As System.Windows.Forms.Label
  Friend WithEvents lnPrefColorTitle As RestrictionTracker.LineBreak
  Friend WithEvents pnlPrefInterface As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctPrefInterfaceIcon As System.Windows.Forms.PictureBox
  Friend WithEvents chkScaleScreen As System.Windows.Forms.CheckBox
  Friend WithEvents chkTrayIcon As System.Windows.Forms.CheckBox
  Friend WithEvents chkTrayMin As System.Windows.Forms.CheckBox
  Friend WithEvents pnlAdvancedNetTestTitle As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblAdvancedNetTestTitle As System.Windows.Forms.Label
  Friend WithEvents lnAdvancedNetTestTitle As RestrictionTracker.LineBreak
  Friend WithEvents pnlAdvancedNetTest As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents pctAdvancedNetTestIcon As System.Windows.Forms.PictureBox
  Friend WithEvents pnlAdvancedNetTestInput As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblAdvancedNetTestInput As System.Windows.Forms.Label
  Friend WithEvents optNetTestNone As System.Windows.Forms.RadioButton
  Friend WithEvents optNetTestTestMyNet As System.Windows.Forms.RadioButton
  Friend WithEvents optNetTestCustom As System.Windows.Forms.RadioButton
  Friend WithEvents txtNetTestCustom As System.Windows.Forms.TextBox
  Friend WithEvents optNetTestSpeedTest As System.Windows.Forms.RadioButton
  Friend WithEvents tmrIcoWait As System.Windows.Forms.Timer
  Friend WithEvents lblNetworkProtocolDescription As System.Windows.Forms.Label
  Friend WithEvents chkTrayAnim As System.Windows.Forms.CheckBox
  Friend WithEvents chkTrayClose As System.Windows.Forms.CheckBox
  Friend WithEvents chkNetworkProtocolTLS10 As System.Windows.Forms.CheckBox
  Friend WithEvents chkNetworkProtocolTLS11 As System.Windows.Forms.CheckBox
  Friend WithEvents chkNetworkProtocolTLS12 As System.Windows.Forms.CheckBox
  Friend WithEvents chkTLSProxy As System.Windows.Forms.CheckBox
  Friend WithEvents mnuKey As System.Windows.Forms.ContextMenu
  Friend WithEvents mnuKeyCut As System.Windows.Forms.MenuItem
  Friend WithEvents mnuKeyCopy As System.Windows.Forms.MenuItem
  Friend WithEvents mnuKeyPaste As System.Windows.Forms.MenuItem
  Friend WithEvents mnuKeySpacer As System.Windows.Forms.MenuItem
  Friend WithEvents mnuKeyDelete As System.Windows.Forms.MenuItem
  Friend WithEvents mnuKeyClear As System.Windows.Forms.MenuItem
  Friend WithEvents chkNetworkSecurityEnforce As System.Windows.Forms.CheckBox
  Friend WithEvents lblRetries1 As System.Windows.Forms.Label
  Friend WithEvents txtRetries As RestrictionTracker.NumericUpDownIncrementable
  Friend WithEvents lblRetries2 As System.Windows.Forms.Label
  Friend WithEvents chkNetworkProtocolTLS13 As CheckBox
  Friend WithEvents lblAccountShareWarning1 As System.Windows.Forms.Label
  Friend WithEvents lblAccountShareWarning2 As System.Windows.Forms.Label
  Friend WithEvents pnlAccountShareTitle As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblAccountShareTitle As System.Windows.Forms.Label
  Friend WithEvents lnAccountShareTitle As RestrictionTracker.LineBreak
  Friend WithEvents chkAccountShare As System.Windows.Forms.CheckBox
End Class
