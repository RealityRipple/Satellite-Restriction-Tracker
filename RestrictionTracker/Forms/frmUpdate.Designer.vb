<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
    Me.pnlUpdate = New System.Windows.Forms.TableLayoutPanel()
    Me.lblNewVer = New System.Windows.Forms.Label()
    Me.txtInfo = New System.Windows.Forms.TextBox()
    Me.lblBETA = New System.Windows.Forms.Label()
    Me.pnlButtons = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdDownload = New System.Windows.Forms.Button()
    Me.cmdCancel = New System.Windows.Forms.Button()
    Me.chkStopBETA = New System.Windows.Forms.CheckBox()
    Me.pnlUpdate.SuspendLayout()
    Me.pnlButtons.SuspendLayout()
    Me.SuspendLayout()
    '
    'pnlUpdate
    '
    Me.pnlUpdate.AutoSize = True
    Me.pnlUpdate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlUpdate.ColumnCount = 1
    Me.pnlUpdate.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUpdate.Controls.Add(Me.lblNewVer, 0, 0)
    Me.pnlUpdate.Controls.Add(Me.txtInfo, 0, 1)
    Me.pnlUpdate.Controls.Add(Me.lblBETA, 0, 2)
    Me.pnlUpdate.Controls.Add(Me.pnlButtons, 0, 4)
    Me.pnlUpdate.Controls.Add(Me.chkStopBETA, 0, 3)
    Me.pnlUpdate.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlUpdate.Location = New System.Drawing.Point(0, 0)
    Me.pnlUpdate.Name = "pnlUpdate"
    Me.pnlUpdate.RowCount = 5
    Me.pnlUpdate.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdate.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlUpdate.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdate.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdate.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlUpdate.Size = New System.Drawing.Size(276, 332)
    Me.pnlUpdate.TabIndex = 0
    '
    'lblNewVer
    '
    Me.lblNewVer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblNewVer.AutoSize = True
    Me.lblNewVer.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblNewVer.Location = New System.Drawing.Point(3, 0)
    Me.lblNewVer.Name = "lblNewVer"
    Me.lblNewVer.Size = New System.Drawing.Size(270, 40)
    Me.lblNewVer.TabIndex = 0
    Me.lblNewVer.Text = "Satellite Restriction Tracker Update" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Version 0.0.0.0"
    Me.lblNewVer.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'txtInfo
    '
    Me.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill
    Me.txtInfo.Location = New System.Drawing.Point(3, 43)
    Me.txtInfo.Multiline = True
    Me.txtInfo.Name = "txtInfo"
    Me.txtInfo.ReadOnly = True
    Me.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.txtInfo.Size = New System.Drawing.Size(270, 179)
    Me.txtInfo.TabIndex = 1
    '
    'lblBETA
    '
    Me.lblBETA.AutoSize = True
    Me.lblBETA.ForeColor = System.Drawing.Color.Firebrick
    Me.lblBETA.Location = New System.Drawing.Point(3, 225)
    Me.lblBETA.Name = "lblBETA"
    Me.lblBETA.Size = New System.Drawing.Size(245, 52)
    Me.lblBETA.TabIndex = 2
    Me.lblBETA.Text = "BETA updates may have bugs and other issues" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "that haven't been worked out yet, bu" & _
    "t need testing" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "on a wide range of accounts." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please back up your history before" & _
    " using BETAs."
    '
    'pnlButtons
    '
    Me.pnlButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlButtons.AutoSize = True
    Me.pnlButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlButtons.ColumnCount = 2
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
    Me.pnlButtons.Controls.Add(Me.cmdDownload, 0, 0)
    Me.pnlButtons.Controls.Add(Me.cmdCancel, 1, 0)
    Me.pnlButtons.Location = New System.Drawing.Point(0, 301)
    Me.pnlButtons.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.RowCount = 1
    Me.pnlButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlButtons.Size = New System.Drawing.Size(276, 31)
    Me.pnlButtons.TabIndex = 3
    '
    'cmdDownload
    '
    Me.cmdDownload.Dock = System.Windows.Forms.DockStyle.Fill
    Me.cmdDownload.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdDownload.Location = New System.Drawing.Point(3, 3)
    Me.cmdDownload.Name = "cmdDownload"
    Me.cmdDownload.Size = New System.Drawing.Size(132, 25)
    Me.cmdDownload.TabIndex = 0
    Me.cmdDownload.Text = "Download Update"
    Me.cmdDownload.UseVisualStyleBackColor = True
    '
    'cmdCancel
    '
    Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdCancel.Dock = System.Windows.Forms.DockStyle.Fill
    Me.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdCancel.Location = New System.Drawing.Point(141, 3)
    Me.cmdCancel.Name = "cmdCancel"
    Me.cmdCancel.Size = New System.Drawing.Size(132, 25)
    Me.cmdCancel.TabIndex = 1
    Me.cmdCancel.Text = "Not Now"
    Me.cmdCancel.UseVisualStyleBackColor = True
    '
    'chkStopBETA
    '
    Me.chkStopBETA.AutoSize = True
    Me.chkStopBETA.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkStopBETA.Location = New System.Drawing.Point(6, 280)
    Me.chkStopBETA.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
    Me.chkStopBETA.Name = "chkStopBETA"
    Me.chkStopBETA.Size = New System.Drawing.Size(189, 18)
    Me.chkStopBETA.TabIndex = 4
    Me.chkStopBETA.Text = "Don't notify me of BETA updates."
    Me.chkStopBETA.UseVisualStyleBackColor = True
    '
    'frmUpdate
    '
    Me.AcceptButton = Me.cmdDownload
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.AutoSize = True
    Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.CancelButton = Me.cmdCancel
    Me.ClientSize = New System.Drawing.Size(276, 332)
    Me.Controls.Add(Me.pnlUpdate)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.Icon = Global.RestrictionTracker.My.Resources.Resources.sat
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmUpdate"
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "New Version Available"
    Me.pnlUpdate.ResumeLayout(False)
    Me.pnlUpdate.PerformLayout()
    Me.pnlButtons.ResumeLayout(False)
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents pnlUpdate As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblNewVer As System.Windows.Forms.Label
  Friend WithEvents txtInfo As System.Windows.Forms.TextBox
  Friend WithEvents lblBETA As System.Windows.Forms.Label
  Friend WithEvents pnlButtons As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdDownload As System.Windows.Forms.Button
  Friend WithEvents cmdCancel As System.Windows.Forms.Button
  Friend WithEvents chkStopBETA As System.Windows.Forms.CheckBox
End Class
