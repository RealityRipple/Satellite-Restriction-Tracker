Public Class PasswordBox
  Private c_Hover As Boolean

  Private Sub pctPasswordEye_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctPasswordEye.MouseDown
    pctPasswordEye.Image = My.Resources.pass_active
    txtPasswordBox.UseSystemPasswordChar = False
  End Sub

  Private Sub pctPasswordEye_MouseEnter(sender As Object, e As System.EventArgs) Handles pctPasswordEye.MouseEnter
    pctPasswordEye.Image = My.Resources.pass_hover
    c_Hover = True
    Me.Invalidate()
  End Sub

  Private Sub pctPasswordEye_MouseLeave(sender As Object, e As System.EventArgs) Handles pctPasswordEye.MouseLeave
    pctPasswordEye.Image = My.Resources.pass_norm
    c_Hover = False
    Me.Invalidate()
  End Sub

  Private Sub pctPasswordEye_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctPasswordEye.MouseUp
    txtPasswordBox.UseSystemPasswordChar = True
    If pctPasswordEye.DisplayRectangle.Contains(e.Location) Then
      pctPasswordEye.Image = My.Resources.pass_hover
    Else
      pctPasswordEye.Image = My.Resources.pass_norm
    End If
  End Sub

  Public Overrides Property Text As String
    Get
      Return txtPasswordBox.Text
    End Get
    Set(value As String)
      txtPasswordBox.Text = value
    End Set
  End Property

  Public Overloads Property Enabled As Boolean
    Get
      Return MyBase.Enabled
    End Get
    Set(value As Boolean)
      MyBase.Enabled = value
      txtPasswordBox.Enabled = value
      pctPasswordEye.Enabled = value
      If value Then
        Me.BackColor = SystemColors.Window
        pnlPasswordBox.BackColor = SystemColors.Window
        pctPasswordEye.Image = My.Resources.pass_norm
      Else
        Me.BackColor = SystemColors.ButtonFace
        pnlPasswordBox.BackColor = SystemColors.ButtonFace
        pctPasswordEye.Image = My.Resources.pass_hover
      End If
    End Set
  End Property

  Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
    MyBase.OnPaint(e)


    Using g As Graphics = e.Graphics
      If Environment.OSVersion.Version.Major > 5 Then
        Dim rBox As Rectangle = Me.DisplayRectangle
        Dim topBar, leftBar, rightBar, bottomBar As Pen
        Dim topBar_t1, topBar_t2, topBar_t3, bottomBar_t1 As SolidBrush

        If Not txtPasswordBox.Enabled Then
          Dim border As New Pen(Color.FromArgb(175, 175, 175))
          Dim outCorner As New SolidBrush(Color.FromArgb(120, border.Color))
          Dim inCorner As New SolidBrush(Color.FromArgb(70, border.Color))
          Dim inBorder As New Pen(Color.FromArgb(255, 255, 255))
          Dim inBG As New SolidBrush(Color.FromArgb(240, 240, 240))
          g.FillRectangle(New SolidBrush(Color.Red), rBox.Left + 1, rBox.Top + 1, rBox.Width - 2, rBox.Height - 2)
          g.DrawLine(border, rBox.Left + 1, rBox.Top + 0, rBox.Right - 2, rBox.Top + 0)
          g.DrawLine(inBorder, rBox.Left + 2, rBox.Top + 1, rBox.Right - 3, rBox.Top + 1)
          g.FillRectangle(outCorner, rBox.Left, rBox.Top, 1, 1)
          g.FillRectangle(outCorner, rBox.Right - 1, rBox.Top, 1, 1)

          g.DrawLine(border, rBox.Left + 1, rBox.Bottom - 1, rBox.Right - 2, rBox.Bottom - 1)
          g.DrawLine(inBorder, rBox.Left + 1, rBox.Bottom - 2, rBox.Right - 2, rBox.Bottom - 2)
          g.FillRectangle(outCorner, rBox.Left, rBox.Bottom - 1, 1, 1)
          g.FillRectangle(outCorner, rBox.Right - 1, rBox.Bottom - 1, 1, 1)

          g.DrawLine(border, rBox.Left, rBox.Top + 1, rBox.Left, rBox.Bottom - 2)
          g.DrawLine(inBorder, rBox.Left + 1, rBox.Top + 1, rBox.Left + 1, rBox.Bottom - 2)
          g.FillRectangle(inCorner, rBox.Left + 1, rBox.Top + 1, 1, 1)
          g.FillRectangle(inCorner, rBox.Left + 1, rBox.Bottom - 2, 1, 1)


          g.DrawLine(border, rBox.Right - 1, rBox.Top + 1, rBox.Right - 1, rBox.Bottom - 2)
          g.DrawLine(inBorder, rBox.Right - 2, rBox.Top + 1, rBox.Right - 2, rBox.Bottom - 2)
          g.FillRectangle(inCorner, rBox.Right - 2, rBox.Top + 1, 1, 1)
          g.FillRectangle(inCorner, rBox.Right - 2, rBox.Bottom - 2, 1, 1)

          g.FillRectangle(inBG, rBox.Left + 2, rBox.Top + 2, rBox.Width - 4, rBox.Height - 4)
          Return
        ElseIf txtPasswordBox.Focused Or pctPasswordEye.Focused Then
          topBar = New Pen(Color.FromArgb(61, 123, 173))
          topBar_t1 = New SolidBrush(Color.FromArgb(200, topBar.Color))
          topBar_t2 = New SolidBrush(Color.FromArgb(120, topBar.Color))
          topBar_t3 = New SolidBrush(Color.FromArgb(70, topBar.Color))

          leftBar = New Pen(Color.FromArgb(181, 207, 231))
          rightBar = New Pen(Color.FromArgb(164, 201, 227))

          bottomBar = New Pen(Color.FromArgb(183, 217, 237))
          bottomBar_t1 = New SolidBrush(Color.FromArgb(120, bottomBar.Color))

        ElseIf c_Hover Then
          topBar = New Pen(Color.FromArgb(87, 148, 191))
          topBar_t1 = New SolidBrush(Color.FromArgb(200, topBar.Color))
          topBar_t2 = New SolidBrush(Color.FromArgb(120, topBar.Color))
          topBar_t3 = New SolidBrush(Color.FromArgb(70, topBar.Color))

          leftBar = New Pen(Color.FromArgb(197, 218, 237))
          rightBar = New Pen(Color.FromArgb(183, 213, 234))

          bottomBar = New Pen(Color.FromArgb(199, 226, 241))
          bottomBar_t1 = New SolidBrush(Color.FromArgb(120, bottomBar.Color))
        Else
          topBar = New Pen(Color.FromArgb(171, 173, 179))
          topBar_t1 = New SolidBrush(Color.FromArgb(200, topBar.Color))
          topBar_t2 = New SolidBrush(Color.FromArgb(120, topBar.Color))
          topBar_t3 = New SolidBrush(Color.FromArgb(70, topBar.Color))

          leftBar = New Pen(Color.FromArgb(226, 227, 234))
          rightBar = New Pen(Color.FromArgb(219, 223, 230))

          bottomBar = New Pen(Color.FromArgb(227, 233, 239))
          bottomBar_t1 = New SolidBrush(Color.FromArgb(120, bottomBar.Color))

        End If
        g.FillRectangle(New SolidBrush(SystemColors.Window), rBox.Left + 1, rBox.Top + 1, rBox.Width - 2, rBox.Height - 2)
        g.FillRectangle(topBar_t2, rBox.Left, rBox.Top, 1, 1)
        g.FillRectangle(topBar_t1, rBox.Left + 1, rBox.Top, 1, 1)
        g.DrawLine(topBar, rBox.Left + 2, rBox.Top + 0, rBox.Right - 2, rBox.Top + 0)
        g.FillRectangle(topBar_t1, rBox.Right - 2, rBox.Top, 1, 1)
        g.FillRectangle(topBar_t2, rBox.Right - 1, rBox.Top, 1, 1)

        g.FillRectangle(topBar_t3, rBox.Left + 1, rBox.Top + 1, 1, 1)
        g.DrawLine(leftBar, rBox.Left, rBox.Top + 1, rBox.Left, rBox.Bottom - 2)
        g.FillRectangle(topBar_t3, rBox.Left + 1, rBox.Bottom - 2, 1, 1)

        g.FillRectangle(topBar_t3, rBox.Right - 2, rBox.Top + 1, 1, 1)
        g.DrawLine(rightBar, rBox.Right - 1, rBox.Top + 1, rBox.Right - 1, rBox.Bottom - 2)
        g.FillRectangle(topBar_t3, rBox.Right - 2, rBox.Bottom - 2, 1, 1)

        g.FillRectangle(bottomBar_t1, rBox.Left, rBox.Bottom - 1, 1, 1)
        g.DrawLine(bottomBar, rBox.Left + 1, rBox.Bottom - 1, rBox.Right - 2, rBox.Bottom - 1)
        g.FillRectangle(bottomBar_t1, rBox.Right - 1, rBox.Bottom - 1, 1, 1)
      Else
        If VisualStyles.VisualStyleRenderer.IsSupported Then
          If Not txtPasswordBox.Enabled Then
            Dim r As New VisualStyles.VisualStyleRenderer(VisualStyles.VisualStyleElement.TextBox.TextEdit.Disabled)
            r.DrawBackground(g, Me.ClientRectangle, e.ClipRectangle)
            r.DrawEdge(g, Me.ClientRectangle, VisualStyles.Edges.Top Or VisualStyles.Edges.Left Or VisualStyles.Edges.Bottom Or VisualStyles.Edges.Right, VisualStyles.EdgeStyle.Etched, VisualStyles.EdgeEffects.None)
          ElseIf txtPasswordBox.Focused Or pctPasswordEye.Focused Then
            Dim r As New VisualStyles.VisualStyleRenderer(VisualStyles.VisualStyleElement.TextBox.TextEdit.Focused)
            r.DrawBackground(g, Me.ClientRectangle, e.ClipRectangle)
            r.DrawEdge(g, Me.ClientRectangle, VisualStyles.Edges.Top Or VisualStyles.Edges.Left Or VisualStyles.Edges.Bottom Or VisualStyles.Edges.Right, VisualStyles.EdgeStyle.Etched, VisualStyles.EdgeEffects.Flat)
          ElseIf c_Hover Then
            Dim r As New VisualStyles.VisualStyleRenderer(VisualStyles.VisualStyleElement.TextBox.TextEdit.Hot)
            r.DrawBackground(g, Me.ClientRectangle, e.ClipRectangle)
            r.DrawEdge(g, Me.ClientRectangle, VisualStyles.Edges.Top Or VisualStyles.Edges.Left Or VisualStyles.Edges.Bottom Or VisualStyles.Edges.Right, VisualStyles.EdgeStyle.Etched, VisualStyles.EdgeEffects.Flat)
          Else
            Dim r As New VisualStyles.VisualStyleRenderer(VisualStyles.VisualStyleElement.TextBox.TextEdit.Normal)
            r.DrawBackground(g, Me.ClientRectangle, e.ClipRectangle)
            r.DrawEdge(g, Me.ClientRectangle, VisualStyles.Edges.Top Or VisualStyles.Edges.Left Or VisualStyles.Edges.Bottom Or VisualStyles.Edges.Right, VisualStyles.EdgeStyle.Etched, VisualStyles.EdgeEffects.Flat)
          End If
        Else

        End If
      End If
    End Using
  End Sub

  Private Sub txtPasswordBox_GotFocus(sender As Object, e As System.EventArgs) Handles txtPasswordBox.GotFocus
    Me.Invalidate()
  End Sub

  Private Sub txtPasswordBox_LostFocus(sender As Object, e As System.EventArgs) Handles txtPasswordBox.LostFocus
    Me.Invalidate()
  End Sub

  Private Sub txtPasswordBox_MouseEnter(sender As Object, e As System.EventArgs) Handles txtPasswordBox.MouseEnter
    c_Hover = True
    Me.Invalidate()
  End Sub

  Private Sub txtPasswordBox_MouseLeave(sender As Object, e As System.EventArgs) Handles txtPasswordBox.MouseLeave
    c_Hover = False
    Me.Invalidate()
  End Sub

  Private Sub txtPasswordBox_TextChanged(sender As Object, e As System.EventArgs) Handles txtPasswordBox.TextChanged
    If String.IsNullOrEmpty(txtPasswordBox.Text) Then
      pctPasswordEye.Visible = False
    Else
      pctPasswordEye.Visible = True
    End If
  End Sub

  Private Sub PasswordBox_MouseEnter(sender As Object, e As System.EventArgs) Handles Me.MouseEnter
    c_Hover = True
    Me.Invalidate()
  End Sub

  Private Sub PasswordBox_MouseLeave(sender As Object, e As System.EventArgs) Handles Me.MouseLeave
    c_Hover = False
    Me.Invalidate()
  End Sub
End Class
