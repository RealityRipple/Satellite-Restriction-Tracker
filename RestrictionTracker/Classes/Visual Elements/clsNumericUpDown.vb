<ToolboxBitmap(GetType(System.Windows.Forms.NumericUpDown))>
Public Class NumericUpDownIncrementable
  Inherits NumericUpDown
  Private incr As UInteger
  Public Sub New()
    MyBase.New()
    incr = 3
  End Sub
  ''' <summary>
  ''' Indicates the amount to increment or decrement on each wheel scroll.
  ''' </summary>
  ''' <value>Increment value for wheel scrolling.</value>
  ''' <remarks>This property only effects the Mouse Wheel event.</remarks>
  <System.ComponentModel.Description("Indicates the amount to increment or decrement on each wheel scroll."), System.ComponentModel.DefaultValue(3)>
  Public Property LargeIncrement As UInteger
    Get
      Return incr
    End Get
    Set(value As UInteger)
      incr = value
    End Set
  End Property
  Protected Overrides Sub OnMouseWheel(e As System.Windows.Forms.MouseEventArgs)
    Dim newArgs As New MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, (e.Delta \ 3) * incr)
    MyBase.OnMouseWheel(newArgs)
  End Sub
End Class