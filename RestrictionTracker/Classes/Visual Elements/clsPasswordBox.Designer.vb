<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PasswordBox
  Inherits System.Windows.Forms.UserControl

  'UserControl overrides dispose to clean up the component list.
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
    Me.txtPasswordBox = New System.Windows.Forms.TextBox()
    Me.pctPasswordEye = New System.Windows.Forms.PictureBox()
    Me.pnlPasswordBox = New System.Windows.Forms.TableLayoutPanel()
    CType(Me.pctPasswordEye, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlPasswordBox.SuspendLayout()
    Me.SuspendLayout()
    '
    'txtPasswordBox
    '
    Me.txtPasswordBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtPasswordBox.BorderStyle = System.Windows.Forms.BorderStyle.None
    Me.txtPasswordBox.Location = New System.Drawing.Point(1, 3)
    Me.txtPasswordBox.Margin = New System.Windows.Forms.Padding(1)
    Me.txtPasswordBox.Name = "txtPasswordBox"
    Me.txtPasswordBox.Size = New System.Drawing.Size(105, 13)
    Me.txtPasswordBox.TabIndex = 0
    Me.txtPasswordBox.UseSystemPasswordChar = True
    '
    'pctPasswordEye
    '
    Me.pctPasswordEye.Anchor = System.Windows.Forms.AnchorStyles.Right
    Me.pctPasswordEye.Image = Global.RestrictionTracker.My.Resources.Resources.pass_norm
    Me.pctPasswordEye.Location = New System.Drawing.Point(108, 2)
    Me.pctPasswordEye.Margin = New System.Windows.Forms.Padding(1)
    Me.pctPasswordEye.Name = "pctPasswordEye"
    Me.pctPasswordEye.Size = New System.Drawing.Size(16, 16)
    Me.pctPasswordEye.TabIndex = 1
    Me.pctPasswordEye.TabStop = False
    Me.pctPasswordEye.Visible = False
    '
    'pnlPasswordBox
    '
    Me.pnlPasswordBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.pnlPasswordBox.ColumnCount = 2
    Me.pnlPasswordBox.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPasswordBox.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlPasswordBox.Controls.Add(Me.pctPasswordEye, 1, 0)
    Me.pnlPasswordBox.Controls.Add(Me.txtPasswordBox, 0, 0)
    Me.pnlPasswordBox.Location = New System.Drawing.Point(2, 2)
    Me.pnlPasswordBox.Margin = New System.Windows.Forms.Padding(2)
    Me.pnlPasswordBox.Name = "pnlPasswordBox"
    Me.pnlPasswordBox.RowCount = 1
    Me.pnlPasswordBox.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlPasswordBox.Size = New System.Drawing.Size(125, 20)
    Me.pnlPasswordBox.TabIndex = 2
    '
    'PasswordBox
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.BackColor = System.Drawing.SystemColors.Window
    Me.Controls.Add(Me.pnlPasswordBox)
    Me.Name = "PasswordBox"
    Me.Size = New System.Drawing.Size(129, 24)
    CType(Me.pctPasswordEye, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlPasswordBox.ResumeLayout(False)
    Me.pnlPasswordBox.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents txtPasswordBox As System.Windows.Forms.TextBox
  Friend WithEvents pctPasswordEye As System.Windows.Forms.PictureBox
  Friend WithEvents pnlPasswordBox As System.Windows.Forms.TableLayoutPanel

End Class
