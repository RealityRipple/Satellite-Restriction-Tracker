<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAlertSelection
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
    Me.pnlStyle = New System.Windows.Forms.TableLayoutPanel()
    Me.lstStyles = New System.Windows.Forms.ListBox()
    Me.pctPreview = New System.Windows.Forms.PictureBox()
    Me.pnlButtons = New System.Windows.Forms.TableLayoutPanel()
    Me.cmdSave = New System.Windows.Forms.Button()
    Me.cmdClose = New System.Windows.Forms.Button()
    Me.lblMore = New RestrictionTracker.LinkLabel()
    Me.ttStyle = New System.Windows.Forms.ToolTip(Me.components)
    Me.pnlStyle.SuspendLayout()
    CType(Me.pctPreview, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlButtons.SuspendLayout()
    Me.SuspendLayout()
    '
    'pnlStyle
    '
    Me.pnlStyle.AutoSize = True
    Me.pnlStyle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlStyle.ColumnCount = 2
    Me.pnlStyle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlStyle.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlStyle.Controls.Add(Me.lstStyles, 0, 0)
    Me.pnlStyle.Controls.Add(Me.pctPreview, 1, 0)
    Me.pnlStyle.Controls.Add(Me.pnlButtons, 1, 1)
    Me.pnlStyle.Controls.Add(Me.lblMore, 0, 1)
    Me.pnlStyle.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pnlStyle.Location = New System.Drawing.Point(0, 0)
    Me.pnlStyle.Name = "pnlStyle"
    Me.pnlStyle.RowCount = 2
    Me.pnlStyle.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlStyle.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlStyle.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.pnlStyle.Size = New System.Drawing.Size(277, 95)
    Me.pnlStyle.TabIndex = 0
    '
    'lstStyles
    '
    Me.lstStyles.AllowDrop = True
    Me.lstStyles.Dock = System.Windows.Forms.DockStyle.Fill
    Me.lstStyles.FormattingEnabled = True
    Me.lstStyles.IntegralHeight = False
    Me.lstStyles.Location = New System.Drawing.Point(3, 3)
    Me.lstStyles.Name = "lstStyles"
    Me.lstStyles.Size = New System.Drawing.Size(100, 50)
    Me.lstStyles.TabIndex = 0
    Me.ttStyle.SetToolTip(Me.lstStyles, "Select the Alert Window Style you want to use." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Drag and Drop: Add an Alert Style" & _
        " from a GZipped TAR (*.tar.gz, *.tgz)." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Delete: Remove an Alert Style from the l" & _
        "ist.")
    '
    'pctPreview
    '
    Me.pctPreview.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.pctPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
    Me.pctPreview.Location = New System.Drawing.Point(166, 3)
    Me.pctPreview.Name = "pctPreview"
    Me.pctPreview.Size = New System.Drawing.Size(50, 50)
    Me.pctPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
    Me.pctPreview.TabIndex = 1
    Me.pctPreview.TabStop = False
    Me.ttStyle.SetToolTip(Me.pctPreview, "Preview the selected Alert Window Style." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Middle Click: Popup a live Alert Window" & _
        " using the selected style." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Right Click: Copy the preview to the Clipboard as an" & _
        " image.")
    '
    'pnlButtons
    '
    Me.pnlButtons.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.pnlButtons.AutoSize = True
    Me.pnlButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlButtons.ColumnCount = 2
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlButtons.Controls.Add(Me.cmdSave, 0, 0)
    Me.pnlButtons.Controls.Add(Me.cmdClose, 1, 0)
    Me.pnlButtons.Location = New System.Drawing.Point(112, 61)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.RowCount = 1
    Me.pnlButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlButtons.Size = New System.Drawing.Size(162, 29)
    Me.pnlButtons.TabIndex = 2
    '
    'cmdSave
    '
    Me.cmdSave.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdSave.Location = New System.Drawing.Point(3, 3)
    Me.cmdSave.Name = "cmdSave"
    Me.cmdSave.Size = New System.Drawing.Size(75, 23)
    Me.cmdSave.TabIndex = 0
    Me.cmdSave.Text = "Save"
    Me.ttStyle.SetToolTip(Me.cmdSave, "Save your Alert Window Style selection.")
    Me.cmdSave.UseVisualStyleBackColor = True
    '
    'cmdClose
    '
    Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdClose.Location = New System.Drawing.Point(84, 3)
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.Size = New System.Drawing.Size(75, 23)
    Me.cmdClose.TabIndex = 1
    Me.cmdClose.Text = "Close"
    Me.ttStyle.SetToolTip(Me.cmdClose, "Close the Select Alert Style window.")
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'lblMore
    '
    Me.lblMore.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblMore.AutoSize = True
    Me.lblMore.Cursor = System.Windows.Forms.Cursors.Hand
    Me.lblMore.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
    Me.lblMore.ForeColor = System.Drawing.Color.MediumBlue
    Me.lblMore.Location = New System.Drawing.Point(12, 69)
    Me.lblMore.Name = "lblMore"
    Me.lblMore.Size = New System.Drawing.Size(82, 13)
    Me.lblMore.TabIndex = 3
    Me.lblMore.Text = "Get More Styles"
    Me.ttStyle.SetToolTip(Me.lblMore, "Download new Alert Window Styles from RealityRipple.com.")
    '
    'frmAlertSelection
    '
    Me.AcceptButton = Me.cmdSave
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.AutoSize = True
    Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.CancelButton = Me.cmdClose
    Me.ClientSize = New System.Drawing.Size(277, 95)
    Me.Controls.Add(Me.pnlStyle)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.Icon = Global.RestrictionTracker.My.Resources.Resources.sat
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmAlertSelection"
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "Select Alert Window Style"
    Me.pnlStyle.ResumeLayout(False)
    Me.pnlStyle.PerformLayout()
    CType(Me.pctPreview, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlButtons.ResumeLayout(False)
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents pnlStyle As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lstStyles As System.Windows.Forms.ListBox
  Friend WithEvents pctPreview As System.Windows.Forms.PictureBox
  Friend WithEvents pnlButtons As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents cmdSave As System.Windows.Forms.Button
  Friend WithEvents cmdClose As System.Windows.Forms.Button
  Friend WithEvents lblMore As RestrictionTracker.LinkLabel
  Friend WithEvents ttStyle As System.Windows.Forms.ToolTip
End Class
