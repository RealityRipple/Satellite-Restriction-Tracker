Public Class frmUpdate
  Private WithEvents sckVerInfo As CookieAwareWebClient
  Private Ret As Boolean
  Private Sub frmUpdate_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    Ret = False
    sckVerInfo = New CookieAwareWebClient
    If lblBETA.Visible Then
      sckVerInfo.DownloadStringAsync(New Uri("http://update.realityripple.com/Satellite_Restriction_Tracker/infob"))
    Else
      sckVerInfo.DownloadStringAsync(New Uri("http://update.realityripple.com/Satellite_Restriction_Tracker/info"))
    End If
  End Sub
  Private Sub frmUpdate_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If sckVerInfo IsNot Nothing Then
      sckVerInfo.Dispose()
      sckVerInfo = Nothing
    End If
    If Not Ret Then Me.DialogResult = Windows.Forms.DialogResult.No
  End Sub
  Public Sub NewUpdate(Version As String, BETA As Boolean)
    If BETA Then
      Me.Text = "New BETA Version Available"
    Else
      Me.Text = "New Version Available"
    End If
    lblNewVer.Text = Application.ProductName & " Update" & vbNewLine & "Version " & DisplayVersion(Version)
    txtInfo.Text = "Loading Update Information" & vbNewLine & vbNewLine & "Please Wait..."
    lblBETA.Visible = BETA
    chkStopBETA.Visible = BETA
  End Sub
  Private Sub cmdDownload_Click(sender As System.Object, e As System.EventArgs) Handles cmdDownload.Click
    If chkStopBETA.Visible And chkStopBETA.Checked Then
      Me.DialogResult = Windows.Forms.DialogResult.OK
    Else
      Me.DialogResult = Windows.Forms.DialogResult.Yes
    End If
    Ret = True
    Me.Close()
  End Sub
  Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
    If chkStopBETA.Visible And chkStopBETA.Checked Then
      Me.DialogResult = Windows.Forms.DialogResult.Cancel
    Else
      Me.DialogResult = Windows.Forms.DialogResult.No
    End If
    Ret = True
    Me.Close()
  End Sub
  Private Sub sckVerInfo_DownloadStringCompleted(sender As Object, e As System.Net.DownloadStringCompletedEventArgs) Handles sckVerInfo.DownloadStringCompleted
    If e.Cancelled Then
      txtInfo.Text = "Info Request Cancelled"
    ElseIf e.Error IsNot Nothing Then
      txtInfo.Text = "Info Request Error" & vbNewLine & e.Error.Message
    Else
      txtInfo.Text = e.Result
    End If
    If sckVerInfo IsNot Nothing Then
      sckVerInfo.Dispose()
      sckVerInfo = Nothing
    End If
  End Sub
  Private Sub sckVerInfo_Failure(sender As Object, e As CookieAwareWebClient.ErrorEventArgs) Handles sckVerInfo.Failure
    txtInfo.Text = "Info Request Error" & vbNewLine & e.Error.Message
    If sckVerInfo IsNot Nothing Then
      sckVerInfo.Dispose()
      sckVerInfo = Nothing
    End If
  End Sub
End Class