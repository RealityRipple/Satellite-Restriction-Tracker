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
    Me.pnlUpdate.Size = New System.Drawing.Size(269, 326)
    Me.pnlUpdate.TabIndex = 0
    '
    'lblNewVer
    '
    Me.lblNewVer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblNewVer.AutoSize = True
    Me.lblNewVer.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblNewVer.Location = New System.Drawing.Point(3, 0)
    Me.lblNewVer.Name = "lblNewVer"
    Me.lblNewVer.Size = New System.Drawing.Size(263, 40)
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
    Me.txtInfo.Size = New System.Drawing.Size(263, 173)
    Me.txtInfo.TabIndex = 1
    '
    'lblBETA
    '
    Me.lblBETA.AutoSize = True
    Me.lblBETA.ForeColor = System.Drawing.Color.Firebrick
    Me.lblBETA.Location = New System.Drawing.Point(3, 219)
    Me.lblBETA.Name = "lblBETA"
    Me.lblBETA.Size = New System.Drawing.Size(255, 52)
    Me.lblBETA.TabIndex = 2
    Me.lblBETA.Text = "BETA updates may have bugs and other issues that haven't been worked out yet, but" & _
    " need testing on a wide range of accounts." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please back up your history before u" & _
    "sing BETAs."
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
    Me.pnlButtons.Location = New System.Drawing.Point(0, 295)
    Me.pnlButtons.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.RowCount = 1
    Me.pnlButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlButtons.Size = New System.Drawing.Size(269, 31)
    Me.pnlButtons.TabIndex = 3
    '
    'cmdDownload
    '
    Me.cmdDownload.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdDownload.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdDownload.Location = New System.Drawing.Point(12, 3)
    Me.cmdDownload.Name = "cmdDownload"
    Me.cmdDownload.Size = New System.Drawing.Size(110, 25)
    Me.cmdDownload.TabIndex = 0
    Me.cmdDownload.Text = "Download Update"
    Me.cmdDownload.UseVisualStyleBackColor = True
    '
    'cmdCancel
    '
    Me.cmdCancel.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdCancel.Location = New System.Drawing.Point(146, 3)
    Me.cmdCancel.Name = "cmdCancel"
    Me.cmdCancel.Size = New System.Drawing.Size(110, 25)
    Me.cmdCancel.TabIndex = 1
    Me.cmdCancel.Text = "Not Now"
    Me.cmdCancel.UseVisualStyleBackColor = True
    '
    'chkStopBETA
    '
    Me.chkStopBETA.AutoSize = True
    Me.chkStopBETA.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkStopBETA.Location = New System.Drawing.Point(6, 274)
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
    Me.CancelButton = Me.cmdCancel
    Me.ClientSize = New System.Drawing.Size(269, 326)
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
