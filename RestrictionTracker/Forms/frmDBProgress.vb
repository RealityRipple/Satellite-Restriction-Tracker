Public Class frmDBProgress
  Private Sub frmDBProgress_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    Me.CenterToParent()
  End Sub
  Public Sub SetAction(Title As String, Message As String)
    Me.CenterToParent()
    pbActivity.Style = ProgressBarStyle.Marquee
    pbActivity.MarqueeAnimationSpeed = 15
    Me.Text = Title
    lblAction.Text = Message
    lblPercentage.Text = "Please Wait..."
    Application.DoEvents()
  End Sub
  Public Sub SetProgress(Current As Integer, Total As Integer)
    If Current > Total Then Current = Total
    Dim bRefresh As Boolean = False
    Dim sPercent As String = FormatPercent(Current / Total, 0, TriState.False, TriState.False, TriState.False)
    If Not lblPercentage.Text = sPercent Then lblPercentage.Text = sPercent : bRefresh = True
    Dim weightedCurrent As Integer = (Current / Total) * 200
    pbActivity.Style = ProgressBarStyle.Continuous
    If Not pbActivity.Value = weightedCurrent Then
      pbActivity.Value = weightedCurrent
      pbActivity.Maximum = 200
      bRefresh = True
    End If
    If bRefresh Then Application.DoEvents()
  End Sub
  Public Sub SetProgress(Current As UInt64, Total As UInt64)
    If Current > Total Then Current = Total
    Dim bRefresh As Boolean = False
    Dim sPercent As String = FormatPercent(Current / Total, 0, TriState.False, TriState.False, TriState.False)
    If Not lblPercentage.Text = sPercent Then lblPercentage.Text = sPercent : bRefresh = True
    pbActivity.Style = ProgressBarStyle.Continuous
    Dim weightedCurrent As Integer = (Current / Total) * 200
    If Not pbActivity.Value = weightedCurrent Then
      pbActivity.Value = weightedCurrent
      pbActivity.Maximum = 200
      bRefresh = True
    End If
    If bRefresh Then Application.DoEvents()
  End Sub
End Class