<ToolboxBitmap(GetType(System.Windows.Forms.ToolStripSeparator))>
Friend Class LineBreak
  Public Sub New()
    InitializeComponent()
    Me.TabStop = False
  End Sub
  Private Sub LineBreak_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
    Using g As Graphics = e.Graphics
      g.SetClip(e.ClipRectangle)
      g.DrawLine(New Pen(SystemColors.ButtonShadow, 1), Me.Padding.Left, Me.Padding.Top, Me.DisplayRectangle.Width - (Me.Padding.Right + Me.Padding.Left), Me.Padding.Top)
      g.DrawLine(New Pen(SystemColors.ButtonHighlight, 1), Me.Padding.Left, Me.Padding.Top + 1, Me.DisplayRectangle.Width - (Me.Padding.Right + Me.Padding.Left), Me.Padding.Top + 1)
    End Using
  End Sub
End Class
