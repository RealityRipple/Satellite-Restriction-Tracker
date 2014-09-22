<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHistory
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
    Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
    Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
    Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
    Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
    Me.pnlButtons = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdClose = New System.Windows.Forms.Button()
    Me.cmdImport = New System.Windows.Forms.Button()
    Me.cmdExport = New System.Windows.Forms.Button()
    Me.lblBackup = New System.Windows.Forms.Label()
    Me.chkExportRange = New System.Windows.Forms.CheckBox()
    Me.pnlGraph = New System.Windows.Forms.TableLayoutPanel()
    Me.pctUld = New System.Windows.Forms.PictureBox()
    Me.pctDld = New System.Windows.Forms.PictureBox()
    Me.dgvUsage = New System.Windows.Forms.DataGridView()
    Me.colDATETIME = New System.Windows.Forms.DataGridViewTextBoxColumn()
    Me.colDOWNLOAD = New System.Windows.Forms.DataGridViewTextBoxColumn()
    Me.colUPLOAD = New System.Windows.Forms.DataGridViewTextBoxColumn()
    Me.DATETIME = New System.Windows.Forms.DataGridViewTextBoxColumn()
    Me.pnlAge = New System.Windows.Forms.TableLayoutPanel()
    Me.lblFrom = New System.Windows.Forms.Label()
    Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
    Me.lblTo = New System.Windows.Forms.Label()
    Me.dtpTo = New System.Windows.Forms.DateTimePicker()
    Me.optGrid = New System.Windows.Forms.RadioButton()
    Me.optGraph = New System.Windows.Forms.RadioButton()
    Me.cmdAllTime = New System.Windows.Forms.Button()
    Me.cmd60Days = New System.Windows.Forms.Button()
    Me.cmd30Days = New System.Windows.Forms.Button()
    Me.cmdToday = New System.Windows.Forms.Button()
    Me.cmdQuery = New System.Windows.Forms.Button()
    Me.pctErr = New System.Windows.Forms.PictureBox()
    Me.grpAge = New System.Windows.Forms.GroupBox()
    Me.ttHistory = New RestrictionTracker.ToolTip(Me.components)
    Me.pnlButtons.SuspendLayout()
    Me.pnlGraph.SuspendLayout()
    CType(Me.pctUld, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.pctDld, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.dgvUsage, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlAge.SuspendLayout()
    CType(Me.pctErr, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.grpAge.SuspendLayout()
    Me.SuspendLayout()
    '
    'pnlButtons
    '
    Me.pnlButtons.ColumnCount = 5
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.Controls.Add(Me.cmdClose, 4, 0)
    Me.pnlButtons.Controls.Add(Me.cmdImport, 1, 0)
    Me.pnlButtons.Controls.Add(Me.cmdExport, 2, 0)
    Me.pnlButtons.Controls.Add(Me.lblBackup, 0, 0)
    Me.pnlButtons.Controls.Add(Me.chkExportRange, 3, 0)
    Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.pnlButtons.Location = New System.Drawing.Point(0, 281)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.RowCount = 1
    Me.pnlButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlButtons.Size = New System.Drawing.Size(514, 31)
    Me.pnlButtons.TabIndex = 1
    '
    'cmdClose
    '
    Me.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdClose.Location = New System.Drawing.Point(436, 4)
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.Size = New System.Drawing.Size(75, 23)
    Me.cmdClose.TabIndex = 11
    Me.cmdClose.Text = "Close"
    Me.ttHistory.SetToolTip(Me.cmdClose, "Close the History window.")
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'cmdImport
    '
    Me.cmdImport.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.cmdImport.AutoSize = True
    Me.cmdImport.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdImport.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdImport.Location = New System.Drawing.Point(56, 4)
    Me.cmdImport.Name = "cmdImport"
    Me.cmdImport.Padding = New System.Windows.Forms.Padding(0, 0, 0, 1)
    Me.cmdImport.Size = New System.Drawing.Size(99, 23)
    Me.cmdImport.TabIndex = 12
    Me.cmdImport.Text = "&Import Database"
    Me.ttHistory.SetToolTip(Me.cmdImport, "Read a DataBase, XML, or CSV file into the history.")
    Me.cmdImport.UseVisualStyleBackColor = True
    '
    'cmdExport
    '
    Me.cmdExport.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.cmdExport.AutoSize = True
    Me.cmdExport.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.cmdExport.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdExport.Location = New System.Drawing.Point(161, 4)
    Me.cmdExport.Name = "cmdExport"
    Me.cmdExport.Padding = New System.Windows.Forms.Padding(0, 0, 0, 1)
    Me.cmdExport.Size = New System.Drawing.Size(100, 23)
    Me.cmdExport.TabIndex = 13
    Me.cmdExport.Text = "&Export Database"
    Me.ttHistory.SetToolTip(Me.cmdExport, "Make a backup DataBase, XML, or CSV file.")
    Me.cmdExport.UseVisualStyleBackColor = True
    '
    'lblBackup
    '
    Me.lblBackup.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblBackup.AutoSize = True
    Me.lblBackup.Location = New System.Drawing.Point(3, 9)
    Me.lblBackup.Name = "lblBackup"
    Me.lblBackup.Size = New System.Drawing.Size(47, 13)
    Me.lblBackup.TabIndex = 14
    Me.lblBackup.Text = "Backup:"
    '
    'chkExportRange
    '
    Me.chkExportRange.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.chkExportRange.AutoSize = True
    Me.chkExportRange.Location = New System.Drawing.Point(267, 7)
    Me.chkExportRange.Name = "chkExportRange"
    Me.chkExportRange.Size = New System.Drawing.Size(136, 17)
    Me.chkExportRange.TabIndex = 15
    Me.chkExportRange.Text = "Export Selected &Range"
    Me.ttHistory.SetToolTip(Me.chkExportRange, "Export only the data within the Age Parameters range.")
    Me.chkExportRange.UseVisualStyleBackColor = True
    '
    'pnlGraph
    '
    Me.pnlGraph.ColumnCount = 1
    Me.pnlGraph.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlGraph.Controls.Add(Me.pctUld, 0, 1)
    Me.pnlGraph.Controls.Add(Me.pctDld, 0, 0)
    Me.pnlGraph.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlGraph.Location = New System.Drawing.Point(0, 76)
    Me.pnlGraph.Name = "pnlGraph"
    Me.pnlGraph.RowCount = 2
    Me.pnlGraph.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlGraph.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlGraph.Size = New System.Drawing.Size(514, 205)
    Me.pnlGraph.TabIndex = 5
    Me.pnlGraph.Visible = False
    '
    'pctUld
    '
    Me.pctUld.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
    Me.pctUld.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pctUld.Location = New System.Drawing.Point(3, 105)
    Me.pctUld.Name = "pctUld"
    Me.pctUld.Size = New System.Drawing.Size(508, 97)
    Me.pctUld.TabIndex = 5
    Me.pctUld.TabStop = False
    '
    'pctDld
    '
    Me.pctDld.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
    Me.pctDld.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pctDld.Location = New System.Drawing.Point(3, 3)
    Me.pctDld.Name = "pctDld"
    Me.pctDld.Size = New System.Drawing.Size(508, 96)
    Me.pctDld.TabIndex = 4
    Me.pctDld.TabStop = False
    '
    'dgvUsage
    '
    Me.dgvUsage.AllowUserToAddRows = False
    Me.dgvUsage.AllowUserToDeleteRows = False
    Me.dgvUsage.AllowUserToOrderColumns = True
    DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveBorder
    Me.dgvUsage.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
    Me.dgvUsage.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
    Me.dgvUsage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
    Me.dgvUsage.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colDATETIME, Me.colDOWNLOAD, Me.colUPLOAD})
    Me.dgvUsage.Dock = System.Windows.Forms.DockStyle.Fill
    Me.dgvUsage.Location = New System.Drawing.Point(0, 76)
    Me.dgvUsage.Name = "dgvUsage"
    Me.dgvUsage.ReadOnly = True
    Me.dgvUsage.RowHeadersVisible = False
    Me.dgvUsage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.dgvUsage.Size = New System.Drawing.Size(514, 205)
    Me.dgvUsage.TabIndex = 2
    Me.dgvUsage.Visible = False
    '
    'colDATETIME
    '
    DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
    Me.colDATETIME.DefaultCellStyle = DataGridViewCellStyle2
    Me.colDATETIME.HeaderText = "Date and Time"
    Me.colDATETIME.Name = "colDATETIME"
    Me.colDATETIME.ReadOnly = True
    '
    'colDOWNLOAD
    '
    DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
    Me.colDOWNLOAD.DefaultCellStyle = DataGridViewCellStyle3
    Me.colDOWNLOAD.HeaderText = "Download"
    Me.colDOWNLOAD.Name = "colDOWNLOAD"
    Me.colDOWNLOAD.ReadOnly = True
    '
    'colUPLOAD
    '
    DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
    Me.colUPLOAD.DefaultCellStyle = DataGridViewCellStyle4
    Me.colUPLOAD.HeaderText = "Upload"
    Me.colUPLOAD.Name = "colUPLOAD"
    Me.colUPLOAD.ReadOnly = True
    '
    'DATETIME
    '
    Me.DATETIME.HeaderText = "Date / Time"
    Me.DATETIME.Name = "DATETIME"
    Me.DATETIME.ReadOnly = True
    '
    'pnlAge
    '
    Me.pnlAge.ColumnCount = 6
    Me.pnlAge.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAge.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.02!))
    Me.pnlAge.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAge.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66!))
    Me.pnlAge.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66!))
    Me.pnlAge.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66!))
    Me.pnlAge.Controls.Add(Me.lblFrom, 0, 0)
    Me.pnlAge.Controls.Add(Me.dtpFrom, 1, 0)
    Me.pnlAge.Controls.Add(Me.lblTo, 0, 1)
    Me.pnlAge.Controls.Add(Me.dtpTo, 1, 1)
    Me.pnlAge.Controls.Add(Me.optGrid, 2, 1)
    Me.pnlAge.Controls.Add(Me.optGraph, 2, 0)
    Me.pnlAge.Controls.Add(Me.cmdAllTime, 5, 1)
    Me.pnlAge.Controls.Add(Me.cmd60Days, 5, 0)
    Me.pnlAge.Controls.Add(Me.cmd30Days, 4, 1)
    Me.pnlAge.Controls.Add(Me.cmdToday, 4, 0)
    Me.pnlAge.Controls.Add(Me.cmdQuery, 3, 0)
    Me.pnlAge.Controls.Add(Me.pctErr, 3, 1)
    Me.pnlAge.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlAge.Location = New System.Drawing.Point(3, 16)
    Me.pnlAge.Name = "pnlAge"
    Me.pnlAge.RowCount = 2
    Me.pnlAge.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlAge.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlAge.Size = New System.Drawing.Size(508, 57)
    Me.pnlAge.TabIndex = 0
    '
    'lblFrom
    '
    Me.lblFrom.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblFrom.AutoSize = True
    Me.lblFrom.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblFrom.Location = New System.Drawing.Point(3, 7)
    Me.lblFrom.Name = "lblFrom"
    Me.lblFrom.Size = New System.Drawing.Size(33, 13)
    Me.lblFrom.TabIndex = 0
    Me.lblFrom.Text = "&From:"
    '
    'dtpFrom
    '
    Me.dtpFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.dtpFrom.CustomFormat = ""
    Me.dtpFrom.Location = New System.Drawing.Point(42, 4)
    Me.dtpFrom.MaxDate = New Date(2010, 9, 23, 0, 0, 0, 0)
    Me.dtpFrom.MinDate = New Date(2010, 9, 23, 0, 0, 0, 0)
    Me.dtpFrom.Name = "dtpFrom"
    Me.dtpFrom.Size = New System.Drawing.Size(195, 20)
    Me.dtpFrom.TabIndex = 1
    Me.ttHistory.SetToolTip(Me.dtpFrom, "Oldest date to display records from.")
    Me.dtpFrom.Value = New Date(2010, 9, 23, 0, 0, 0, 0)
    '
    'lblTo
    '
    Me.lblTo.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblTo.AutoSize = True
    Me.lblTo.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.lblTo.Location = New System.Drawing.Point(3, 36)
    Me.lblTo.Name = "lblTo"
    Me.lblTo.Size = New System.Drawing.Size(23, 13)
    Me.lblTo.TabIndex = 2
    Me.lblTo.Text = "&To:"
    '
    'dtpTo
    '
    Me.dtpTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.dtpTo.Location = New System.Drawing.Point(42, 32)
    Me.dtpTo.MaxDate = New Date(2010, 9, 23, 0, 0, 0, 0)
    Me.dtpTo.MinDate = New Date(2010, 9, 23, 0, 0, 0, 0)
    Me.dtpTo.Name = "dtpTo"
    Me.dtpTo.Size = New System.Drawing.Size(195, 20)
    Me.dtpTo.TabIndex = 3
    Me.ttHistory.SetToolTip(Me.dtpTo, "Most recent date to display records from.")
    Me.dtpTo.Value = New Date(2010, 9, 23, 0, 0, 0, 0)
    '
    'optGrid
    '
    Me.optGrid.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.optGrid.AutoSize = True
    Me.optGrid.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.optGrid.Location = New System.Drawing.Point(243, 33)
    Me.optGrid.Name = "optGrid"
    Me.optGrid.Size = New System.Drawing.Size(50, 18)
    Me.optGrid.TabIndex = 5
    Me.optGrid.TabStop = True
    Me.optGrid.Text = "Gri&d"
    Me.ttHistory.SetToolTip(Me.optGrid, "Display history in a grid.")
    Me.optGrid.UseVisualStyleBackColor = True
    '
    'optGraph
    '
    Me.optGraph.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.optGraph.AutoSize = True
    Me.optGraph.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.optGraph.Location = New System.Drawing.Point(243, 5)
    Me.optGraph.Name = "optGraph"
    Me.optGraph.Size = New System.Drawing.Size(60, 18)
    Me.optGraph.TabIndex = 4
    Me.optGraph.TabStop = True
    Me.optGraph.Text = "&Graph"
    Me.ttHistory.SetToolTip(Me.optGraph, "Display history in a line graph.")
    Me.optGraph.UseVisualStyleBackColor = True
    '
    'cmdAllTime
    '
    Me.cmdAllTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdAllTime.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdAllTime.Location = New System.Drawing.Point(443, 31)
    Me.cmdAllTime.Name = "cmdAllTime"
    Me.cmdAllTime.Size = New System.Drawing.Size(62, 22)
    Me.cmdAllTime.TabIndex = 10
    Me.cmdAllTime.Text = "&All Time"
    Me.ttHistory.SetToolTip(Me.cmdAllTime, "Query the database to get the entire history.")
    Me.cmdAllTime.UseVisualStyleBackColor = True
    '
    'cmd60Days
    '
    Me.cmd60Days.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmd60Days.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmd60Days.Location = New System.Drawing.Point(443, 3)
    Me.cmd60Days.Name = "cmd60Days"
    Me.cmd60Days.Size = New System.Drawing.Size(62, 22)
    Me.cmd60Days.TabIndex = 9
    Me.cmd60Days.Text = "&60 Days"
    Me.ttHistory.SetToolTip(Me.cmd60Days, "Query the database to get the last 60 days' history.")
    Me.cmd60Days.UseVisualStyleBackColor = True
    '
    'cmd30Days
    '
    Me.cmd30Days.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmd30Days.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmd30Days.Location = New System.Drawing.Point(376, 31)
    Me.cmd30Days.Name = "cmd30Days"
    Me.cmd30Days.Size = New System.Drawing.Size(61, 22)
    Me.cmd30Days.TabIndex = 8
    Me.cmd30Days.Text = "&30 Days"
    Me.ttHistory.SetToolTip(Me.cmd30Days, "Query the database to get the last 30 days' history.")
    Me.cmd30Days.UseVisualStyleBackColor = True
    '
    'cmdToday
    '
    Me.cmdToday.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdToday.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdToday.Location = New System.Drawing.Point(376, 3)
    Me.cmdToday.Name = "cmdToday"
    Me.cmdToday.Size = New System.Drawing.Size(61, 22)
    Me.cmdToday.TabIndex = 7
    Me.cmdToday.Text = "T&oday"
    Me.ttHistory.SetToolTip(Me.cmdToday, "Query the database to get today's history.")
    Me.cmdToday.UseVisualStyleBackColor = True
    '
    'cmdQuery
    '
    Me.cmdQuery.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdQuery.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdQuery.Location = New System.Drawing.Point(309, 3)
    Me.cmdQuery.Name = "cmdQuery"
    Me.cmdQuery.Size = New System.Drawing.Size(61, 22)
    Me.cmdQuery.TabIndex = 6
    Me.cmdQuery.Text = "&Query"
    Me.ttHistory.SetToolTip(Me.cmdQuery, "Query the database to get your requested history.")
    Me.cmdQuery.UseVisualStyleBackColor = True
    '
    'pctErr
    '
    Me.pctErr.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.pctErr.Location = New System.Drawing.Point(331, 34)
    Me.pctErr.Name = "pctErr"
    Me.pctErr.Size = New System.Drawing.Size(16, 16)
    Me.pctErr.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
    Me.pctErr.TabIndex = 11
    Me.pctErr.TabStop = False
    '
    'grpAge
    '
    Me.grpAge.Controls.Add(Me.pnlAge)
    Me.grpAge.Dock = System.Windows.Forms.DockStyle.Top
    Me.grpAge.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.grpAge.Location = New System.Drawing.Point(0, 0)
    Me.grpAge.Name = "grpAge"
    Me.grpAge.Size = New System.Drawing.Size(514, 76)
    Me.grpAge.TabIndex = 0
    Me.grpAge.TabStop = False
    Me.grpAge.Text = "Age Parameters"
    '
    'ttHistory
    '
    Me.ttHistory.Persistant = True
    '
    'frmHistory
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
    Me.CancelButton = Me.cmdClose
    Me.ClientSize = New System.Drawing.Size(514, 312)
    Me.Controls.Add(Me.pnlGraph)
    Me.Controls.Add(Me.dgvUsage)
    Me.Controls.Add(Me.grpAge)
    Me.Controls.Add(Me.pnlButtons)
    Me.Icon = Global.RestrictionTracker.My.Resources.Resources.sat
    Me.MinimizeBox = False
    Me.MinimumSize = New System.Drawing.Size(530, 350)
    Me.Name = "frmHistory"
    Me.ShowInTaskbar = False
    Me.Text = "Satellite Restriction Tracker Usage History"
    Me.pnlButtons.ResumeLayout(False)
    Me.pnlButtons.PerformLayout()
    Me.pnlGraph.ResumeLayout(False)
    CType(Me.pctUld, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.pctDld, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.dgvUsage, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlAge.ResumeLayout(False)
    Me.pnlAge.PerformLayout()
    CType(Me.pctErr, System.ComponentModel.ISupportInitialize).EndInit()
    Me.grpAge.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents pnlButtons As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdClose As System.Windows.Forms.Button
  Friend WithEvents pnlGraph As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents dgvUsage As System.Windows.Forms.DataGridView
  Friend WithEvents DATETIME As System.Windows.Forms.DataGridViewTextBoxColumn
  Friend WithEvents pnlAge As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblFrom As System.Windows.Forms.Label
  Friend WithEvents dtpFrom As System.Windows.Forms.DateTimePicker
  Friend WithEvents lblTo As System.Windows.Forms.Label
  Friend WithEvents dtpTo As System.Windows.Forms.DateTimePicker
  Friend WithEvents optGrid As System.Windows.Forms.RadioButton
  Friend WithEvents optGraph As System.Windows.Forms.RadioButton
  Friend WithEvents cmdAllTime As System.Windows.Forms.Button
  Friend WithEvents cmd60Days As System.Windows.Forms.Button
  Friend WithEvents cmd30Days As System.Windows.Forms.Button
  Friend WithEvents cmdToday As System.Windows.Forms.Button
  Friend WithEvents cmdQuery As System.Windows.Forms.Button
  Friend WithEvents grpAge As System.Windows.Forms.GroupBox
  Friend WithEvents cmdImport As System.Windows.Forms.Button
  Friend WithEvents cmdExport As System.Windows.Forms.Button
  Friend WithEvents lblBackup As System.Windows.Forms.Label
  Friend WithEvents pctUld As System.Windows.Forms.PictureBox
  Friend WithEvents pctDld As System.Windows.Forms.PictureBox
  Friend WithEvents pctErr As System.Windows.Forms.PictureBox
  Friend WithEvents ttHistory As ToolTip
  Friend WithEvents chkExportRange As System.Windows.Forms.CheckBox
  Friend WithEvents colDATETIME As System.Windows.Forms.DataGridViewTextBoxColumn
  Friend WithEvents colDOWNLOAD As System.Windows.Forms.DataGridViewTextBoxColumn
  Friend WithEvents colUPLOAD As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
