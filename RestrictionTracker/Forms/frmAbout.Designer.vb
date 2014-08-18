<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAbout
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAbout))
    Me.LogoPictureBox = New System.Windows.Forms.PictureBox()
    Me.pnlAbout = New System.Windows.Forms.TableLayoutPanel()
    Me.lblProduct = New RestrictionTracker.LinkLabel()
    Me.lblVersion = New RestrictionTracker.LinkLabel()
    Me.lblUpdate = New RestrictionTracker.LinkLabel()
    Me.lblCompany = New RestrictionTracker.LinkLabel()
    Me.txtDescription = New System.Windows.Forms.TextBox()
    Me.cmdOK = New System.Windows.Forms.Button()
    Me.cmdDonate = New System.Windows.Forms.Button()
    Me.tmrSpeed = New System.Windows.Forms.Timer(Me.components)
    Me.cmdUpdate = New System.Windows.Forms.Button()
    Me.ttAbout = New RestrictionTracker.ToolTip(Me.components)
    CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlAbout.SuspendLayout()
    Me.SuspendLayout()
    '
    'LogoPictureBox
    '
    Me.LogoPictureBox.BackgroundImage = Global.RestrictionTracker.My.Resources.Resources.side
    Me.LogoPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
    Me.LogoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill
    Me.LogoPictureBox.Location = New System.Drawing.Point(3, 3)
    Me.LogoPictureBox.Name = "LogoPictureBox"
    Me.pnlAbout.SetRowSpan(Me.LogoPictureBox, 6)
    Me.LogoPictureBox.Size = New System.Drawing.Size(124, 244)
    Me.LogoPictureBox.TabIndex = 0
    Me.LogoPictureBox.TabStop = False
    '
    'pnlAbout
    '
    Me.pnlAbout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlAbout.ColumnCount = 3
    Me.pnlAbout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAbout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
    Me.pnlAbout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.pnlAbout.Controls.Add(Me.LogoPictureBox, 0, 0)
    Me.pnlAbout.Controls.Add(Me.lblProduct, 1, 0)
    Me.pnlAbout.Controls.Add(Me.lblVersion, 1, 1)
    Me.pnlAbout.Controls.Add(Me.lblUpdate, 1, 2)
    Me.pnlAbout.Controls.Add(Me.lblCompany, 1, 3)
    Me.pnlAbout.Controls.Add(Me.txtDescription, 1, 4)
    Me.pnlAbout.Controls.Add(Me.cmdOK, 2, 5)
    Me.pnlAbout.Controls.Add(Me.cmdDonate, 1, 5)
    Me.pnlAbout.Location = New System.Drawing.Point(9, 9)
    Me.pnlAbout.Name = "pnlAbout"
    Me.pnlAbout.RowCount = 6
    Me.pnlAbout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
    Me.pnlAbout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
    Me.pnlAbout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
    Me.pnlAbout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
    Me.pnlAbout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.pnlAbout.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.pnlAbout.Size = New System.Drawing.Size(396, 250)
    Me.pnlAbout.TabIndex = 1
    '
    'lblProduct
    '
    Me.lblProduct.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblProduct.AutoSize = True
    Me.pnlAbout.SetColumnSpan(Me.lblProduct, 2)
    Me.lblProduct.Cursor = System.Windows.Forms.Cursors.Hand
    Me.lblProduct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
    Me.lblProduct.ForeColor = System.Drawing.Color.MediumBlue
    Me.lblProduct.Location = New System.Drawing.Point(136, 7)
    Me.lblProduct.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
    Me.lblProduct.Name = "lblProduct"
    Me.lblProduct.Size = New System.Drawing.Size(75, 13)
    Me.lblProduct.TabIndex = 2
    Me.lblProduct.TabStop = True
    Me.lblProduct.Text = "Product Name"
    Me.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.ttAbout.SetTooltip(Me.lblProduct, "Visit the Satellite Restriction Tracker webpage.")
    '
    'lblVersion
    '
    Me.lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblVersion.AutoSize = True
    Me.pnlAbout.SetColumnSpan(Me.lblVersion, 2)
    Me.lblVersion.Cursor = System.Windows.Forms.Cursors.Hand
    Me.lblVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
    Me.lblVersion.ForeColor = System.Drawing.Color.MediumBlue
    Me.lblVersion.Location = New System.Drawing.Point(136, 34)
    Me.lblVersion.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
    Me.lblVersion.Name = "lblVersion"
    Me.lblVersion.Size = New System.Drawing.Size(42, 13)
    Me.lblVersion.TabIndex = 3
    Me.lblVersion.TabStop = True
    Me.lblVersion.Text = "Version"
    Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.ttAbout.SetTooltip(Me.lblVersion, "View the Satellite Restriction Tracker version history.")
    '
    'lblUpdate
    '
    Me.lblUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblUpdate.AutoEllipsis = True
    Me.pnlAbout.SetColumnSpan(Me.lblUpdate, 2)
    Me.lblUpdate.Cursor = System.Windows.Forms.Cursors.Default
    Me.lblUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
    Me.lblUpdate.ForeColor = System.Drawing.SystemColors.ControlText
    Me.lblUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.lblUpdate.Link = False
    Me.lblUpdate.Location = New System.Drawing.Point(136, 59)
    Me.lblUpdate.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
    Me.lblUpdate.Name = "lblUpdate"
    Me.lblUpdate.Size = New System.Drawing.Size(257, 17)
    Me.lblUpdate.TabIndex = 4
    Me.lblUpdate.TabStop = True
    Me.lblUpdate.Text = "No New Updates"
    Me.lblUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'lblCompany
    '
    Me.lblCompany.Anchor = System.Windows.Forms.AnchorStyles.Left
    Me.lblCompany.AutoSize = True
    Me.pnlAbout.SetColumnSpan(Me.lblCompany, 2)
    Me.lblCompany.Cursor = System.Windows.Forms.Cursors.Hand
    Me.lblCompany.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
    Me.lblCompany.ForeColor = System.Drawing.Color.MediumBlue
    Me.lblCompany.Location = New System.Drawing.Point(136, 88)
    Me.lblCompany.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
    Me.lblCompany.Name = "lblCompany"
    Me.lblCompany.Size = New System.Drawing.Size(82, 13)
    Me.lblCompany.TabIndex = 5
    Me.lblCompany.TabStop = True
    Me.lblCompany.Text = "Company Name"
    Me.lblCompany.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.ttAbout.SetTooltip(Me.lblCompany, "Visit RealityRipple Software.")
    '
    'txtDescription
    '
    Me.pnlAbout.SetColumnSpan(Me.txtDescription, 2)
    Me.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill
    Me.txtDescription.Location = New System.Drawing.Point(136, 111)
    Me.txtDescription.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
    Me.txtDescription.Multiline = True
    Me.txtDescription.Name = "txtDescription"
    Me.txtDescription.ReadOnly = True
    Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both
    Me.txtDescription.Size = New System.Drawing.Size(257, 103)
    Me.txtDescription.TabIndex = 6
    Me.txtDescription.TabStop = False
    Me.txtDescription.Text = resources.GetString("txtDescription.Text")
    '
    'cmdOK
    '
    Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cmdOK.AutoSize = True
    Me.cmdOK.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdOK.Location = New System.Drawing.Point(318, 222)
    Me.cmdOK.Name = "cmdOK"
    Me.cmdOK.Size = New System.Drawing.Size(75, 25)
    Me.cmdOK.TabIndex = 0
    Me.cmdOK.Text = "OK"
    Me.ttAbout.SetTooltip(Me.cmdOK, "Close.")
    '
    'cmdDonate
    '
    Me.cmdDonate.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.cmdDonate.AutoSize = True
    Me.cmdDonate.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdDonate.Location = New System.Drawing.Point(167, 221)
    Me.cmdDonate.Name = "cmdDonate"
    Me.cmdDonate.Size = New System.Drawing.Size(110, 25)
    Me.cmdDonate.TabIndex = 1
    Me.cmdDonate.Text = "Make a &Donation"
    Me.ttAbout.SetTooltip(Me.cmdDonate, "Donate to RealityRipple Software to keep Satellite Restriction Tracker alive.")
    Me.cmdDonate.UseVisualStyleBackColor = True
    '
    'tmrSpeed
    '
    Me.tmrSpeed.Interval = 1000
    '
    'cmdUpdate
    '
    Me.cmdUpdate.AutoSize = True
    Me.cmdUpdate.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdUpdate.Location = New System.Drawing.Point(0, 0)
    Me.cmdUpdate.Name = "cmdUpdate"
    Me.cmdUpdate.Padding = New System.Windows.Forms.Padding(6, 0, 3, 0)
    Me.cmdUpdate.Size = New System.Drawing.Size(120, 25)
    Me.cmdUpdate.TabIndex = 2
    Me.cmdUpdate.Text = "Check for Updates"
    Me.cmdUpdate.UseVisualStyleBackColor = True
    '
    'frmAbout
    '
    Me.AcceptButton = Me.cmdOK
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.AutoSize = True
    Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.ClientSize = New System.Drawing.Size(414, 272)
    Me.Controls.Add(Me.pnlAbout)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.Icon = Global.RestrictionTracker.My.Resources.Resources.sat
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmAbout"
    Me.Padding = New System.Windows.Forms.Padding(9)
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "frmAbout"
    CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlAbout.ResumeLayout(False)
    Me.pnlAbout.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents LogoPictureBox As System.Windows.Forms.PictureBox
  Friend WithEvents pnlAbout As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents lblProduct As LinkLabel
  Friend WithEvents lblVersion As LinkLabel
  Friend WithEvents lblUpdate As LinkLabel
  Friend WithEvents lblCompany As LinkLabel
  Friend WithEvents txtDescription As System.Windows.Forms.TextBox
  Friend WithEvents cmdOK As System.Windows.Forms.Button
  Friend WithEvents cmdDonate As System.Windows.Forms.Button
  Friend WithEvents tmrSpeed As System.Windows.Forms.Timer
  Friend WithEvents ttAbout As RestrictionTracker.ToolTip
  Friend WithEvents cmdUpdate As System.Windows.Forms.Button
End Class
