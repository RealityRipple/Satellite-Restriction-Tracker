﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUpdate
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
    Me.pnlUpdate = New System.Windows.Forms.TableLayoutPanel()
    Me.lblTitle = New System.Windows.Forms.Label()
    Me.txtInfo = New System.Windows.Forms.TextBox()
    Me.lblBETA = New System.Windows.Forms.Label()
    Me.pnlButtons = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdDownload = New System.Windows.Forms.Button()
    Me.cmdCancel = New System.Windows.Forms.Button()
    Me.cmdChanges = New System.Windows.Forms.Button()
    Me.pctThrobber = New System.Windows.Forms.PictureBox()
    Me.chkStopBETA = New System.Windows.Forms.CheckBox()
    Me.lblNewVer = New System.Windows.Forms.Label()
    Me.ttUpdate = New RestrictionTracker.ToolTip(Me.components)
    Me.pnlUpdate.SuspendLayout()
    Me.pnlButtons.SuspendLayout()
    CType(Me.pctThrobber, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'pnlUpdate
    '
    Me.pnlUpdate.AutoSize = True
    Me.pnlUpdate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlUpdate.ColumnCount = 1
    Me.pnlUpdate.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUpdate.Controls.Add(Me.lblTitle, 0, 0)
    Me.pnlUpdate.Controls.Add(Me.txtInfo, 0, 5)
    Me.pnlUpdate.Controls.Add(Me.lblBETA, 0, 2)
    Me.pnlUpdate.Controls.Add(Me.pnlButtons, 0, 4)
    Me.pnlUpdate.Controls.Add(Me.chkStopBETA, 0, 3)
    Me.pnlUpdate.Controls.Add(Me.lblNewVer, 0, 1)
    Me.pnlUpdate.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlUpdate.Location = New System.Drawing.Point(0, 0)
    Me.pnlUpdate.Name = "pnlUpdate"
    Me.pnlUpdate.RowCount = 6
    Me.pnlUpdate.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdate.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdate.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdate.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdate.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdate.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUpdate.Size = New System.Drawing.Size(387, 223)
    Me.pnlUpdate.TabIndex = 2
    '
    'lblTitle
    '
    Me.lblTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblTitle.AutoSize = True
    Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblTitle.Location = New System.Drawing.Point(3, 6)
    Me.lblTitle.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
    Me.lblTitle.Name = "lblTitle"
    Me.lblTitle.Size = New System.Drawing.Size(381, 24)
    Me.lblTitle.TabIndex = 0
    Me.lblTitle.Text = "Satellite Restriction Tracker Update"
    '
    'txtInfo
    '
    Me.txtInfo.Dock = System.Windows.Forms.DockStyle.Top
    Me.txtInfo.Location = New System.Drawing.Point(3, 211)
    Me.txtInfo.Multiline = True
    Me.txtInfo.Name = "txtInfo"
    Me.txtInfo.ReadOnly = True
    Me.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.txtInfo.Size = New System.Drawing.Size(381, 1)
    Me.txtInfo.TabIndex = 3
    Me.txtInfo.Visible = False
    '
    'lblBETA
    '
    Me.lblBETA.AutoSize = True
    Me.lblBETA.ForeColor = System.Drawing.Color.Firebrick
    Me.lblBETA.Location = New System.Drawing.Point(3, 111)
    Me.lblBETA.Margin = New System.Windows.Forms.Padding(3)
    Me.lblBETA.Name = "lblBETA"
    Me.lblBETA.Size = New System.Drawing.Size(317, 39)
    Me.lblBETA.TabIndex = 4
    Me.lblBETA.Text = "BETA updates may have bugs and other issues that haven't been" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "worked out yet, bu" & _
    "t need testing on a wide range of accounts." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please back up your history before " & _
    "using BETAs."
    Me.lblBETA.Visible = False
    '
    'pnlButtons
    '
    Me.pnlButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlButtons.AutoSize = True
    Me.pnlButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlButtons.ColumnCount = 4
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlButtons.Controls.Add(Me.cmdDownload, 1, 0)
    Me.pnlButtons.Controls.Add(Me.cmdCancel, 2, 0)
    Me.pnlButtons.Controls.Add(Me.cmdChanges, 3, 0)
    Me.pnlButtons.Controls.Add(Me.pctThrobber, 0, 0)
    Me.pnlButtons.Location = New System.Drawing.Point(0, 177)
    Me.pnlButtons.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.RowCount = 1
    Me.pnlButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlButtons.Size = New System.Drawing.Size(387, 31)
    Me.pnlButtons.TabIndex = 2
    '
    'cmdDownload
    '
    Me.cmdDownload.AutoSize = True
    Me.cmdDownload.Dock = System.Windows.Forms.DockStyle.Fill
    Me.cmdDownload.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdDownload.Location = New System.Drawing.Point(92, 3)
    Me.cmdDownload.Name = "cmdDownload"
    Me.cmdDownload.Size = New System.Drawing.Size(120, 25)
    Me.cmdDownload.TabIndex = 0
    Me.cmdDownload.Text = "Download &Update"
    Me.ttUpdate.SetToolTip(Me.cmdDownload, "Download the new version.")
    Me.cmdDownload.UseVisualStyleBackColor = True
    '
    'cmdCancel
    '
    Me.cmdCancel.AutoSize = True
    Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdCancel.Dock = System.Windows.Forms.DockStyle.Fill
    Me.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdCancel.Location = New System.Drawing.Point(218, 3)
    Me.cmdCancel.Name = "cmdCancel"
    Me.cmdCancel.Size = New System.Drawing.Size(80, 25)
    Me.cmdCancel.TabIndex = 1
    Me.cmdCancel.Text = "&Not Now"
    Me.ttUpdate.SetToolTip(Me.cmdCancel, "Ignore the new version for now.")
    Me.cmdCancel.UseVisualStyleBackColor = True
    '
    'cmdChanges
    '
    Me.cmdChanges.AutoSize = True
    Me.cmdChanges.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdChanges.Location = New System.Drawing.Point(304, 3)
    Me.cmdChanges.Name = "cmdChanges"
    Me.cmdChanges.Size = New System.Drawing.Size(80, 25)
    Me.cmdChanges.TabIndex = 2
    Me.cmdChanges.Text = "&Changes >>"
    Me.ttUpdate.SetToolTip(Me.cmdChanges, "View the latest version's Change Log.")
    Me.cmdChanges.UseVisualStyleBackColor = True
    '
    'pctThrobber
    '
    Me.pctThrobber.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.pctThrobber.Image = Global.RestrictionTracker.My.Resources.Resources.throbber
    Me.pctThrobber.Location = New System.Drawing.Point(70, 7)
    Me.pctThrobber.Name = "pctThrobber"
    Me.pctThrobber.Size = New System.Drawing.Size(16, 16)
    Me.pctThrobber.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
    Me.pctThrobber.TabIndex = 3
    Me.pctThrobber.TabStop = False
    Me.pctThrobber.Visible = False
    '
    'chkStopBETA
    '
    Me.chkStopBETA.AutoSize = True
    Me.chkStopBETA.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkStopBETA.Location = New System.Drawing.Point(6, 156)
    Me.chkStopBETA.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
    Me.chkStopBETA.Name = "chkStopBETA"
    Me.chkStopBETA.Size = New System.Drawing.Size(189, 18)
    Me.chkStopBETA.TabIndex = 5
    Me.chkStopBETA.Text = "Don't notify me of &BETA updates."
    Me.ttUpdate.SetToolTip(Me.chkStopBETA, "Disable notifications of BETA version updates.")
    Me.chkStopBETA.UseVisualStyleBackColor = True
    Me.chkStopBETA.Visible = False
    '
    'lblNewVer
    '
    Me.lblNewVer.AutoSize = True
    Me.lblNewVer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblNewVer.Location = New System.Drawing.Point(3, 42)
    Me.lblNewVer.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
    Me.lblNewVer.Name = "lblNewVer"
    Me.lblNewVer.Size = New System.Drawing.Size(379, 60)
    Me.lblNewVer.TabIndex = 1
    Me.lblNewVer.Text = "Version %v has been released and is available for download." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "To keep up-to-date w" & _
    "ith the latest features, improvements, bug fixes, and" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "meter compliance, please " & _
    "update %p immediately." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
    '
    'frmUpdate
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.AutoSize = True
    Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.ClientSize = New System.Drawing.Size(387, 223)
    Me.Controls.Add(Me.pnlUpdate)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.Icon = Global.RestrictionTracker.My.Resources.Resources.sat
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmUpdate"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "frmUpdate"
    Me.pnlUpdate.ResumeLayout(False)
    Me.pnlUpdate.PerformLayout()
    Me.pnlButtons.ResumeLayout(False)
    Me.pnlButtons.PerformLayout()
    CType(Me.pctThrobber, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents pnlUpdate As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblTitle As System.Windows.Forms.Label
  Friend WithEvents txtInfo As System.Windows.Forms.TextBox
  Friend WithEvents lblBETA As System.Windows.Forms.Label
  Friend WithEvents pnlButtons As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdDownload As System.Windows.Forms.Button
  Friend WithEvents cmdCancel As System.Windows.Forms.Button
  Friend WithEvents cmdChanges As System.Windows.Forms.Button
  Friend WithEvents chkStopBETA As System.Windows.Forms.CheckBox
  Friend WithEvents lblNewVer As System.Windows.Forms.Label
  Friend WithEvents pctThrobber As System.Windows.Forms.PictureBox
  Friend WithEvents ttUpdate As ToolTip
End Class
