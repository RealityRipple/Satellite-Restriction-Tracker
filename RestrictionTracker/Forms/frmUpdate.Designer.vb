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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUpdate))
    Me.pnlUpdate = New System.Windows.Forms.TableLayoutPanel()
    Me.lblTitle = New System.Windows.Forms.Label()
    Me.txtInfo = New System.Windows.Forms.TextBox()
    Me.lblBETA = New System.Windows.Forms.Label()
    Me.pnlButtons = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdDownload = New System.Windows.Forms.Button()
    Me.cmdCancel = New System.Windows.Forms.Button()
    Me.cmdChanges = New System.Windows.Forms.Button()
    Me.chkStopBETA = New System.Windows.Forms.CheckBox()
    Me.lblNewVer = New System.Windows.Forms.Label()
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
    Me.pnlUpdate.Size = New System.Drawing.Size(297, 277)
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
    Me.lblTitle.Size = New System.Drawing.Size(291, 48)
    Me.lblTitle.TabIndex = 0
    Me.lblTitle.Text = "Satellite Restriction Tracker Update"
    '
    'txtInfo
    '
    Me.txtInfo.Dock = System.Windows.Forms.DockStyle.Top
    Me.txtInfo.Location = New System.Drawing.Point(3, 291)
    Me.txtInfo.Multiline = True
    Me.txtInfo.Name = "txtInfo"
    Me.txtInfo.ReadOnly = True
    Me.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
    Me.txtInfo.Size = New System.Drawing.Size(291, 1)
    Me.txtInfo.TabIndex = 3
    Me.txtInfo.Visible = False
    '
    'lblBETA
    '
    Me.lblBETA.AutoSize = True
    Me.lblBETA.ForeColor = System.Drawing.Color.Firebrick
    Me.lblBETA.Location = New System.Drawing.Point(3, 165)
    Me.lblBETA.Margin = New System.Windows.Forms.Padding(3)
    Me.lblBETA.Name = "lblBETA"
    Me.lblBETA.Size = New System.Drawing.Size(290, 65)
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
    Me.pnlButtons.Location = New System.Drawing.Point(0, 257)
    Me.pnlButtons.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.RowCount = 1
    Me.pnlButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlButtons.Size = New System.Drawing.Size(297, 31)
    Me.pnlButtons.TabIndex = 2
    '
    'cmdDownload
    '
    Me.cmdDownload.AutoSize = True
    Me.cmdDownload.Dock = System.Windows.Forms.DockStyle.Fill
    Me.cmdDownload.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdDownload.Location = New System.Drawing.Point(2, 3)
    Me.cmdDownload.Name = "cmdDownload"
    Me.cmdDownload.Size = New System.Drawing.Size(120, 25)
    Me.cmdDownload.TabIndex = 0
    Me.cmdDownload.Text = "Download Update"
    Me.cmdDownload.UseVisualStyleBackColor = True
    '
    'cmdCancel
    '
    Me.cmdCancel.AutoSize = True
    Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdCancel.Dock = System.Windows.Forms.DockStyle.Fill
    Me.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdCancel.Location = New System.Drawing.Point(128, 3)
    Me.cmdCancel.Name = "cmdCancel"
    Me.cmdCancel.Size = New System.Drawing.Size(80, 25)
    Me.cmdCancel.TabIndex = 1
    Me.cmdCancel.Text = "Not Now"
    Me.cmdCancel.UseVisualStyleBackColor = True
    '
    'cmdChanges
    '
    Me.cmdChanges.AutoSize = True
    Me.cmdChanges.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdChanges.Location = New System.Drawing.Point(214, 3)
    Me.cmdChanges.Name = "cmdChanges"
    Me.cmdChanges.Size = New System.Drawing.Size(80, 25)
    Me.cmdChanges.TabIndex = 2
    Me.cmdChanges.Text = "Changes >>"
    Me.cmdChanges.UseVisualStyleBackColor = True
    '
    'chkStopBETA
    '
    Me.chkStopBETA.AutoSize = True
    Me.chkStopBETA.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkStopBETA.Location = New System.Drawing.Point(6, 236)
    Me.chkStopBETA.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
    Me.chkStopBETA.Name = "chkStopBETA"
    Me.chkStopBETA.Size = New System.Drawing.Size(189, 18)
    Me.chkStopBETA.TabIndex = 5
    Me.chkStopBETA.Text = "Don't notify me of BETA updates."
    Me.chkStopBETA.UseVisualStyleBackColor = True
    Me.chkStopBETA.Visible = False
    '
    'lblNewVer
    '
    Me.lblNewVer.AutoSize = True
    Me.lblNewVer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblNewVer.Location = New System.Drawing.Point(3, 66)
    Me.lblNewVer.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
    Me.lblNewVer.Name = "lblNewVer"
    Me.lblNewVer.Size = New System.Drawing.Size(280, 90)
    Me.lblNewVer.TabIndex = 1
    Me.lblNewVer.Text = resources.GetString("lblNewVer.Text")
    '
    'frmUpdate
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.AutoSize = True
    Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.ClientSize = New System.Drawing.Size(297, 277)
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
    Me.pnlButtons.PerformLayout()
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
End Class
