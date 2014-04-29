<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDBProgress
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
    Me.pnlStatus = New System.Windows.Forms.TableLayoutPanel()
    Me.lblAction = New System.Windows.Forms.Label()
    Me.pbActivity = New System.Windows.Forms.ProgressBar()
    Me.lblPercentage = New System.Windows.Forms.Label()
    Me.pnlStatus.SuspendLayout()
    Me.SuspendLayout()
    '
    'pnlStatus
    '
    Me.pnlStatus.AutoSize = True
    Me.pnlStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlStatus.ColumnCount = 1
    Me.pnlStatus.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlStatus.Controls.Add(Me.lblAction, 0, 0)
    Me.pnlStatus.Controls.Add(Me.pbActivity, 0, 1)
    Me.pnlStatus.Controls.Add(Me.lblPercentage, 0, 2)
    Me.pnlStatus.Location = New System.Drawing.Point(0, 0)
    Me.pnlStatus.Name = "pnlStatus"
    Me.pnlStatus.RowCount = 3
    Me.pnlStatus.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlStatus.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlStatus.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlStatus.Size = New System.Drawing.Size(226, 65)
    Me.pnlStatus.TabIndex = 0
    '
    'lblAction
    '
    Me.lblAction.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblAction.AutoSize = True
    Me.lblAction.Location = New System.Drawing.Point(61, 4)
    Me.lblAction.Margin = New System.Windows.Forms.Padding(4)
    Me.lblAction.Name = "lblAction"
    Me.lblAction.Size = New System.Drawing.Size(104, 13)
    Me.lblAction.TabIndex = 0
    Me.lblAction.Text = "Loading DataBase..."
    Me.lblAction.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'pbActivity
    '
    Me.pbActivity.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pbActivity.Location = New System.Drawing.Point(13, 25)
    Me.pbActivity.Margin = New System.Windows.Forms.Padding(13, 4, 13, 4)
    Me.pbActivity.MarqueeAnimationSpeed = 15
    Me.pbActivity.Name = "pbActivity"
    Me.pbActivity.Size = New System.Drawing.Size(200, 15)
    Me.pbActivity.Style = System.Windows.Forms.ProgressBarStyle.Marquee
    Me.pbActivity.TabIndex = 1
    '
    'lblPercentage
    '
    Me.lblPercentage.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.lblPercentage.AutoSize = True
    Me.lblPercentage.Location = New System.Drawing.Point(102, 48)
    Me.lblPercentage.Margin = New System.Windows.Forms.Padding(4)
    Me.lblPercentage.Name = "lblPercentage"
    Me.lblPercentage.Size = New System.Drawing.Size(21, 13)
    Me.lblPercentage.TabIndex = 2
    Me.lblPercentage.Text = "0%"
    Me.lblPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'frmDBProgress
    '
    Me.Icon = Global.RestrictionTracker.My.Resources.Resources.sat
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.AutoSize = True
    Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.ClientSize = New System.Drawing.Size(227, 78)
    Me.ControlBox = False
    Me.Controls.Add(Me.pnlStatus)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmDBProgress"
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "Loading..."
    Me.pnlStatus.ResumeLayout(False)
    Me.pnlStatus.PerformLayout()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents pnlStatus As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblAction As System.Windows.Forms.Label
  Friend WithEvents pbActivity As System.Windows.Forms.ProgressBar
  Friend WithEvents lblPercentage As System.Windows.Forms.Label
End Class
