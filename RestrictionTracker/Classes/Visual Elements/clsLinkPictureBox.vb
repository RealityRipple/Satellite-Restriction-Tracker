Imports System.ComponentModel
<ToolboxBitmap(GetType(System.Windows.Forms.PictureBox))>
Public Class LinkPictureBox
  Inherits System.Windows.Forms.PictureBox
  Private oldPos As Point = Point.Empty
  Public Sub New()
    Me.SetStyle(ControlStyles.Selectable, True)
    Me.SetStyle(ControlStyles.Opaque, False)
    Me.TabStop = True
    Me.TabIndex = 0
  End Sub
  Protected Overrides Sub OnMouseDown(e As System.Windows.Forms.MouseEventArgs)
    If e.Button = Windows.Forms.MouseButtons.Left Then
      Me.Focus()
      If oldPos.IsEmpty Then
        oldPos = Me.Location
        Me.Location = Point.Add(oldPos, New Size(1, 1))
      End If
    End If
    MyBase.OnMouseDown(e)
  End Sub
  Protected Overrides Sub OnEnter(e As System.EventArgs)
    Me.Invalidate()
    MyBase.OnEnter(e)
  End Sub
  Protected Overrides Sub OnLeave(e As System.EventArgs)
    Me.Invalidate()
    MyBase.OnLeave(e)
  End Sub
  <Browsable(True), EditorBrowsable(EditorBrowsableState.Always)>
  Public Shadows Property TabStop As Boolean
    Get
      Return MyBase.TabStop
    End Get
    Set(value As Boolean)
      MyBase.TabStop = value
    End Set
  End Property
  <Browsable(True), EditorBrowsable(EditorBrowsableState.Always)>
  Public Shadows Property TabIndex As Integer
    Get
      Return MyBase.TabIndex
    End Get
    Set(value As Integer)
      MyBase.TabIndex = value
    End Set
  End Property
  Protected Overrides Sub OnMouseMove(e As System.Windows.Forms.MouseEventArgs)
    If e.Button = Windows.Forms.MouseButtons.Left Then
      If Not oldPos.IsEmpty Then
        Dim testRect As Rectangle = Me.ClientRectangle
        If Not Me.Location = oldPos Then
          testRect = New Rectangle(Me.ClientRectangle.X - 1, Me.ClientRectangle.Y - 1, Me.ClientRectangle.Width + 1, Me.ClientRectangle.Height + 1)
        End If
        If Not testRect.Contains(e.Location) Then
          Me.Location = oldPos
        Else
          Me.Location = Point.Add(oldPos, New Size(1, 1))
        End If
      End If
    End If
    MyBase.OnMouseMove(e)
  End Sub
  Protected Overrides Sub OnMouseUp(e As System.Windows.Forms.MouseEventArgs)
    If e.Button = Windows.Forms.MouseButtons.Left Then
      If Not oldPos.IsEmpty Then
        Me.Location = oldPos
        oldPos = Point.Empty
      End If
    End If
    MyBase.OnMouseUp(e)
  End Sub
  <EditorBrowsable(EditorBrowsableState.Always)>
  Shadows Event KeyDown(sender As Object, e As KeyEventArgs)
  Protected Overrides Sub OnKeyDown(e As System.Windows.Forms.KeyEventArgs)
    If e.KeyCode = Keys.Space Or e.KeyCode = Keys.Return Then
      If oldPos.IsEmpty Then
        oldPos = Me.Location
        Me.Location = Point.Add(oldPos, New Size(1, 1))
      End If
    End If
    RaiseEvent KeyDown(Me, e)
    MyBase.OnKeyDown(e)
  End Sub
  <EditorBrowsable(EditorBrowsableState.Always)>
  Shadows Event KeyUp(sender As Object, e As KeyEventArgs)
  Protected Overrides Sub OnKeyUp(e As System.Windows.Forms.KeyEventArgs)
    If e.KeyCode = Keys.Space Or e.KeyCode = Keys.Return Then
      If Not oldPos.IsEmpty Then
        Me.Location = oldPos
        oldPos = Point.Empty
      End If
    End If
    RaiseEvent KeyUp(Me, e)
    MyBase.OnKeyUp(e)
  End Sub
  Protected Overrides Sub OnPaint(pe As System.Windows.Forms.PaintEventArgs)
    MyBase.OnPaint(pe)
    If Me.Focused And Form.ActiveForm Is Me.FindForm Then
      Dim rc = Me.ClientRectangle
      ControlPaint.DrawFocusRectangle(pe.Graphics, rc)
    End If
  End Sub
  Protected Overrides Sub OnGotFocus(e As System.EventArgs)
    Me.Invalidate()
    MyBase.OnGotFocus(e)
  End Sub
  Protected Overrides Sub OnLostFocus(e As System.EventArgs)
    If Not oldPos.IsEmpty Then
      Me.Location = oldPos
      oldPos = Point.Empty
    End If
    Me.Invalidate()
    MyBase.OnLostFocus(e)
  End Sub
  Protected Overrides Sub WndProc(ByRef msg As System.Windows.Forms.Message)
    If msg.Msg = &H20 Then
      If MyBase.Cursor = Cursors.Hand Then
        NativeMethods.SetCursor(NativeMethods.LoadCursor(IntPtr.Zero, &H7F89))
        msg.Result = IntPtr.Zero
        Return
      End If
    End If
    MyBase.WndProc(msg)
  End Sub
End Class
