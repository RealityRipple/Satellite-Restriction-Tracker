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
      If Environment.OSVersion.Version.Major = 5 Then
        If Not txtPasswordBox.Enabled Then
          DrawTextbox_XP(g, Me.DisplayRectangle, ActiveState.Disable)
        ElseIf txtPasswordBox.Focused Or pctPasswordEye.Focused Then
          DrawTextbox_XP(g, Me.DisplayRectangle, ActiveState.Focus)
        ElseIf c_Hover Then
          DrawTextbox_XP(g, Me.DisplayRectangle, ActiveState.Hover)
        Else
          DrawTextbox_XP(g, Me.DisplayRectangle, ActiveState.Normal)
        End If
      ElseIf Environment.OSVersion.Version.Major = 6 Then
        If Environment.OSVersion.Version.Minor < 2 Then
          If Not txtPasswordBox.Enabled Then
            DrawTextbox_V7(g, Me.DisplayRectangle, ActiveState.Disable)
          ElseIf txtPasswordBox.Focused Or pctPasswordEye.Focused Then
            DrawTextbox_V7(g, Me.DisplayRectangle, ActiveState.Focus)
          ElseIf c_Hover Then
            DrawTextbox_V7(g, Me.DisplayRectangle, ActiveState.Hover)
          Else
            DrawTextbox_V7(g, Me.DisplayRectangle, ActiveState.Normal)
          End If
        Else
          If Not txtPasswordBox.Enabled Then
            DrawTextbox_8(g, Me.DisplayRectangle, ActiveState.Disable)
          ElseIf txtPasswordBox.Focused Or pctPasswordEye.Focused Then
            DrawTextbox_8(g, Me.DisplayRectangle, ActiveState.Focus)
          ElseIf c_Hover Then
            DrawTextbox_8(g, Me.DisplayRectangle, ActiveState.Hover)
          Else
            DrawTextbox_8(g, Me.DisplayRectangle, ActiveState.Normal)
          End If
        End If
      ElseIf Environment.OSVersion.Version.Major = 10 Then
        If Not txtPasswordBox.Enabled Then
          DrawTextbox_8(g, Me.DisplayRectangle, ActiveState.Disable)
        ElseIf txtPasswordBox.Focused Or pctPasswordEye.Focused Then
          DrawTextbox_8(g, Me.DisplayRectangle, ActiveState.Focus)
        ElseIf c_Hover Then
          DrawTextbox_8(g, Me.DisplayRectangle, ActiveState.Hover)
        Else
          DrawTextbox_8(g, Me.DisplayRectangle, ActiveState.Normal)
        End If
      Else
        If Not txtPasswordBox.Enabled Then
          DrawTextbox(g, Me.DisplayRectangle, ActiveState.Disable)
        ElseIf txtPasswordBox.Focused Or pctPasswordEye.Focused Then
          DrawTextbox(g, Me.DisplayRectangle, ActiveState.Focus)
        ElseIf c_Hover Then
          DrawTextbox(g, Me.DisplayRectangle, ActiveState.Hover)
        Else
          DrawTextbox(g, Me.DisplayRectangle, ActiveState.Normal)
        End If
      End If
    End Using
  End Sub

  Private Enum ActiveState
    Normal
    Hover
    Focus
    Disable
  End Enum

  Private Sub DrawTextbox_XP(ByRef g As Graphics, drawBox As Rectangle, state As ActiveState)
    Dim borderColor As Pen
    Dim bgColor As SolidBrush
    Select Case state
      Case ActiveState.Normal
        borderColor = New Pen(Color.FromArgb(127, 157, 185))
        bgColor = Brushes.White
      Case ActiveState.Hover
        borderColor = New Pen(Color.FromArgb(127, 157, 185))
        bgColor = Brushes.White
      Case ActiveState.Focus
        borderColor = New Pen(Color.FromArgb(127, 157, 185))
        bgColor = Brushes.White
      Case Else
        borderColor = New Pen(Color.FromArgb(126, 155, 183))
        bgColor = New SolidBrush(Color.FromArgb(236, 233, 216))
    End Select
    g.FillRectangle(bgColor, drawBox.Left + 1, drawBox.Top + 1, drawBox.Width - 2, drawBox.Height - 2)
    g.DrawRectangle(borderColor, drawBox.Left, drawBox.Top, drawBox.Width - 1, drawBox.Height - 1)
  End Sub

  Private Sub DrawTextbox_V7(ByRef g As Graphics, drawBox As Rectangle, state As ActiveState)
    Dim topBar, leftBar, rightBar, bottomBar As Pen
    Dim topBar_t1, topBar_t2, topBar_t3, bottomBar_t1 As SolidBrush
    Select Case state
      Case ActiveState.Normal
        topBar = New Pen(Color.FromArgb(171, 173, 179))
        topBar_t1 = New SolidBrush(Color.FromArgb(200, topBar.Color))
        topBar_t2 = New SolidBrush(Color.FromArgb(120, topBar.Color))
        topBar_t3 = New SolidBrush(Color.FromArgb(70, topBar.Color))
        leftBar = New Pen(Color.FromArgb(226, 227, 234))
        rightBar = New Pen(Color.FromArgb(219, 223, 230))
        bottomBar = New Pen(Color.FromArgb(227, 233, 239))
        bottomBar_t1 = New SolidBrush(Color.FromArgb(120, bottomBar.Color))
      Case ActiveState.Hover
        topBar = New Pen(Color.FromArgb(87, 148, 191))
        topBar_t1 = New SolidBrush(Color.FromArgb(200, topBar.Color))
        topBar_t2 = New SolidBrush(Color.FromArgb(120, topBar.Color))
        topBar_t3 = New SolidBrush(Color.FromArgb(70, topBar.Color))
        leftBar = New Pen(Color.FromArgb(197, 218, 237))
        rightBar = New Pen(Color.FromArgb(183, 213, 234))
        bottomBar = New Pen(Color.FromArgb(199, 226, 241))
        bottomBar_t1 = New SolidBrush(Color.FromArgb(120, bottomBar.Color))
      Case ActiveState.Focus
        topBar = New Pen(Color.FromArgb(61, 123, 173))
        topBar_t1 = New SolidBrush(Color.FromArgb(200, topBar.Color))
        topBar_t2 = New SolidBrush(Color.FromArgb(120, topBar.Color))
        topBar_t3 = New SolidBrush(Color.FromArgb(70, topBar.Color))
        leftBar = New Pen(Color.FromArgb(181, 207, 231))
        rightBar = New Pen(Color.FromArgb(164, 201, 227))
        bottomBar = New Pen(Color.FromArgb(183, 217, 237))
        bottomBar_t1 = New SolidBrush(Color.FromArgb(120, bottomBar.Color))
      Case Else
        Dim border As New Pen(Color.FromArgb(175, 175, 175))
        Dim outCorner As New SolidBrush(Color.FromArgb(120, border.Color))
        Dim inCorner As New SolidBrush(Color.FromArgb(70, border.Color))
        Dim inBorder As New Pen(Color.FromArgb(255, 255, 255))
        Dim inBG As New SolidBrush(Color.FromArgb(240, 240, 240))
        g.FillRectangle(New SolidBrush(Color.Red), drawBox.Left + 1, drawBox.Top + 1, drawBox.Width - 2, drawBox.Height - 2)
        g.DrawLine(border, drawBox.Left + 1, drawBox.Top + 0, drawBox.Right - 2, drawBox.Top + 0)
        g.DrawLine(inBorder, drawBox.Left + 2, drawBox.Top + 1, drawBox.Right - 3, drawBox.Top + 1)
        g.FillRectangle(outCorner, drawBox.Left, drawBox.Top, 1, 1)
        g.FillRectangle(outCorner, drawBox.Right - 1, drawBox.Top, 1, 1)
        g.DrawLine(border, drawBox.Left + 1, drawBox.Bottom - 1, drawBox.Right - 2, drawBox.Bottom - 1)
        g.DrawLine(inBorder, drawBox.Left + 1, drawBox.Bottom - 2, drawBox.Right - 2, drawBox.Bottom - 2)
        g.FillRectangle(outCorner, drawBox.Left, drawBox.Bottom - 1, 1, 1)
        g.FillRectangle(outCorner, drawBox.Right - 1, drawBox.Bottom - 1, 1, 1)
        g.DrawLine(border, drawBox.Left, drawBox.Top + 1, drawBox.Left, drawBox.Bottom - 2)
        g.DrawLine(inBorder, drawBox.Left + 1, drawBox.Top + 1, drawBox.Left + 1, drawBox.Bottom - 2)
        g.FillRectangle(inCorner, drawBox.Left + 1, drawBox.Top + 1, 1, 1)
        g.FillRectangle(inCorner, drawBox.Left + 1, drawBox.Bottom - 2, 1, 1)
        g.DrawLine(border, drawBox.Right - 1, drawBox.Top + 1, drawBox.Right - 1, drawBox.Bottom - 2)
        g.DrawLine(inBorder, drawBox.Right - 2, drawBox.Top + 1, drawBox.Right - 2, drawBox.Bottom - 2)
        g.FillRectangle(inCorner, drawBox.Right - 2, drawBox.Top + 1, 1, 1)
        g.FillRectangle(inCorner, drawBox.Right - 2, drawBox.Bottom - 2, 1, 1)
        g.FillRectangle(inBG, drawBox.Left + 2, drawBox.Top + 2, drawBox.Width - 4, drawBox.Height - 4)
        Return
    End Select
    g.FillRectangle(Brushes.White, drawBox.Left + 1, drawBox.Top + 1, drawBox.Width - 2, drawBox.Height - 2)
    g.FillRectangle(topBar_t2, drawBox.Left, drawBox.Top, 1, 1)
    g.FillRectangle(topBar_t1, drawBox.Left + 1, drawBox.Top, 1, 1)
    g.DrawLine(topBar, drawBox.Left + 2, drawBox.Top + 0, drawBox.Right - 2, drawBox.Top + 0)
    g.FillRectangle(topBar_t1, drawBox.Right - 2, drawBox.Top, 1, 1)
    g.FillRectangle(topBar_t2, drawBox.Right - 1, drawBox.Top, 1, 1)
    g.FillRectangle(topBar_t3, drawBox.Left + 1, drawBox.Top + 1, 1, 1)
    g.DrawLine(leftBar, drawBox.Left, drawBox.Top + 1, drawBox.Left, drawBox.Bottom - 2)
    g.FillRectangle(topBar_t3, drawBox.Left + 1, drawBox.Bottom - 2, 1, 1)
    g.FillRectangle(topBar_t3, drawBox.Right - 2, drawBox.Top + 1, 1, 1)
    g.DrawLine(rightBar, drawBox.Right - 1, drawBox.Top + 1, drawBox.Right - 1, drawBox.Bottom - 2)
    g.FillRectangle(topBar_t3, drawBox.Right - 2, drawBox.Bottom - 2, 1, 1)
    g.FillRectangle(bottomBar_t1, drawBox.Left, drawBox.Bottom - 1, 1, 1)
    g.DrawLine(bottomBar, drawBox.Left + 1, drawBox.Bottom - 1, drawBox.Right - 2, drawBox.Bottom - 1)
    g.FillRectangle(bottomBar_t1, drawBox.Right - 1, drawBox.Bottom - 1, 1, 1)
  End Sub

  Private Sub DrawTextbox_8(ByRef g As Graphics, drawBox As Rectangle, state As ActiveState)
    Dim borderColor As Pen
    Dim bgColor As SolidBrush
    Select Case state
      Case ActiveState.Normal
        borderColor = New Pen(Color.FromArgb(171, 173, 179))
        bgColor = Brushes.White
      Case ActiveState.Hover
        borderColor = New Pen(Color.FromArgb(126, 180, 234))
        bgColor = Brushes.White
      Case ActiveState.Focus
        borderColor = New Pen(Color.FromArgb(86, 157, 229))
        bgColor = Brushes.White
      Case Else
        borderColor = New Pen(Color.FromArgb(217, 217, 217))
        bgColor = New SolidBrush(Color.FromArgb(240, 240, 240))
        g.FillRectangle(Brushes.White, drawBox.Left + 1, drawBox.Top + 1, drawBox.Width - 2, drawBox.Height - 2)
        g.FillRectangle(bgColor, drawBox.Left + 2, drawBox.Top + 2, drawBox.Width - 4, drawBox.Height - 4)
        g.DrawRectangle(borderColor, drawBox.Left, drawBox.Top, drawBox.Width - 1, drawBox.Height - 1)
        Return
    End Select
    g.FillRectangle(bgColor, drawBox.Left + 1, drawBox.Top + 1, drawBox.Width - 2, drawBox.Height - 2)
    g.DrawRectangle(borderColor, drawBox.Left, drawBox.Top, drawBox.Width - 1, drawBox.Height - 1)
  End Sub

  Private Sub DrawTextbox(ByRef g As Graphics, drawBox As Rectangle, state As ActiveState)
    Dim borderColor As Pen
    Dim bgColor As SolidBrush
    Select Case state
      Case ActiveState.Normal
        borderColor = New Pen(SystemColors.InactiveBorder)
        bgColor = New SolidBrush(SystemColors.Window)
      Case ActiveState.Hover
        borderColor = New Pen(SystemColors.ActiveBorder)
        bgColor = New SolidBrush(SystemColors.Window)
      Case ActiveState.Focus
        borderColor = New Pen(SystemColors.ActiveBorder)
        bgColor = New SolidBrush(SystemColors.Window)
      Case Else
        borderColor = New Pen(SystemColors.ButtonFace)
        bgColor = New SolidBrush(SystemColors.ButtonShadow)
    End Select
    g.FillRectangle(bgColor, drawBox.Left + 1, drawBox.Top + 1, drawBox.Width - 2, drawBox.Height - 2)
    g.DrawRectangle(borderColor, drawBox.Left, drawBox.Top, drawBox.Width - 1, drawBox.Height - 1)
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

  Private Sub PasswordBox_SizeChanged(sender As Object, e As System.EventArgs) Handles Me.SizeChanged
    If pctPasswordEye.Visible Then
      txtPasswordBox.Width = Me.DisplayRectangle.Width - pctPasswordEye.Width - 4
    Else
      txtPasswordBox.Width = Me.DisplayRectangle.Width - 2
    End If
  End Sub
End Class
