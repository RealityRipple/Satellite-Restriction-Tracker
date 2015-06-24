Public Class frmDonate
  Private WithEvents taskNotifier As TaskbarNotifier
  Private Sub cmdDonate_Click(sender As System.Object, e As System.EventArgs) Handles cmdDonate.Click
    Try
      Process.Start("http://realityripple.com/donate.php?itm=Satellite+Restriction+Tracker")
      frmMain.ClickedDonate()
      Me.Close()
    Catch ex As Exception
      MakeNotifier(taskNotifier, False)
      If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", My.Application.Info.ProductName & " could not navigate to ""realityripple.com/donate.php""!" & vbNewLine & ex.Message, 200, 3000, 100)
    End Try
  End Sub
  Private Sub cmdSignUp_Click(sender As System.Object, e As System.EventArgs) Handles cmdSignUp.Click
    Try
      Process.Start("http://srt.realityripple.com/c_signup.php")
      Me.Close()
    Catch ex As Exception
      Dim taskNotifier As TaskbarNotifier = Nothing
      MakeNotifier(taskNotifier, False)
      If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", My.Application.Info.ProductName & " could not navigate to ""srt.realityripple.com/c_signup.php""!" & vbNewLine & ex.Message, 200, 3000, 100)
    End Try
  End Sub
  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    Me.Close()
  End Sub
  Private Sub taskNotifier_ContentClick(sender As Object, e As System.EventArgs) Handles taskNotifier.ContentClick
    taskNotifier.SlowHide()
  End Sub
End Class