Imports System.ComponentModel
<ToolboxBitmap(GetType(System.Windows.Forms.PictureBox))>
Public Class LinkPictureBox
  Inherits System.Windows.Forms.PictureBox
  Protected Overrides Sub OnMouseDown(e As System.Windows.Forms.MouseEventArgs)
    Me.Top += 1
    Me.Left += 1
    MyBase.OnMouseDown(e)
  End Sub
  Protected Overrides Sub OnMouseUp(e As System.Windows.Forms.MouseEventArgs)
    Me.Top -= 1
    Me.Left -= 1
    MyBase.OnMouseUp(e)
  End Sub
  Protected Overrides Sub WndProc(ByRef msg As System.Windows.Forms.Message)
    If msg.Msg = 32 Then
      If MyBase.Cursor = Cursors.Hand Then
        NativeMethods.SetCursor(NativeMethods.LoadCursor(0, 32649))
        msg.Result = IntPtr.Zero
        Exit Sub
      End If
    End If
    MyBase.WndProc(msg)
  End Sub
End Class
