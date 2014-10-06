Public Class frmUpdate
  Private WithEvents sckVerInfo As CookieAwareWebClient
  Private Ret As Boolean
  Private Sub frmUpdate_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    Ret = False
  End Sub
  Private Sub frmUpdate_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If sckVerInfo IsNot Nothing Then
      sckVerInfo.Dispose()
      sckVerInfo = Nothing
    End If
    If Not Ret Then Me.DialogResult = Windows.Forms.DialogResult.No
  End Sub
  Public Sub NewUpdate(Version As String, BETA As Boolean, UACIcon As Boolean)
    If BETA Then
      Me.Text = "New BETA Version Available - " & Application.ProductName
      lblTitle.Text = Application.ProductName & " BETA Update"
    Else
      Me.Text = "New Version Available - " & Application.ProductName
      lblTitle.Text = Application.ProductName & " Update"
    End If
    Dim newVer As String = "Version %v has been released and is available for download." & vbNewLine &
             "To keep up-to-date with the latest features, improvements, bug fixes, and" & vbNewLine &
             "meter compliance, please update %p immediately."
    newVer = newVer.Replace("%v", DisplayVersion(Version))
    newVer = newVer.Replace("%p", Application.ProductName)
    lblNewVer.Text = newVer
    txtInfo.Text = "Loading Update Information" & vbNewLine & vbNewLine & "Please Wait..."
    lblBETA.Visible = BETA
    chkStopBETA.Visible = BETA
    If UACIcon Then NativeMethods.SendMessage(cmdDownload.Handle, NativeMethods.BCM_SETSHIELD, 0, &HFFFFFFFFUI)
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
  Private Sub cmdChanges_Click(sender As System.Object, e As System.EventArgs) Handles cmdChanges.Click
    If txtInfo.Visible Then
      cmdChanges.Text = "Changes >>"
      txtInfo.Visible = False
    Else
      txtInfo.Height = 100
      cmdChanges.Text = "Changes <<"
      txtInfo.Visible = True
      If Not txtInfo.Text.StartsWith("Released:") Then
        pctThrobber.Visible = True
        cmdDownload.Focus()
        cmdChanges.Enabled = False
        txtInfo.Text = "Loading Update Information" & vbNewLine & vbNewLine & "Please Wait..."
        Dim getInfo As New MethodInvoker(AddressOf GetVerInfo)
        getInfo.BeginInvoke(Nothing, Nothing)
      End If
    End If
  End Sub
  Private Sub GetVerInfo()
    sckVerInfo = New CookieAwareWebClient()
    If lblBETA.Visible Then
      sckVerInfo.DownloadStringAsync(New Uri("http://update.realityripple.com/Satellite_Restriction_Tracker/infob"))
    Else
      sckVerInfo.DownloadStringAsync(New Uri("http://update.realityripple.com/Satellite_Restriction_Tracker/info"))
    End If
  End Sub
  Private Sub sckVerInfo_DownloadStringCompleted(sender As Object, e As System.Net.DownloadStringCompletedEventArgs) Handles sckVerInfo.DownloadStringCompleted
    If Me.InvokeRequired Then
      Me.Invoke(New Net.DownloadStringCompletedEventHandler(AddressOf sckVerInfo_DownloadStringCompleted), sender, e)
    Else
      pctThrobber.Visible = False
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
      cmdChanges.Enabled = True
      cmdChanges.Focus()
    End If
  End Sub
  Private Delegate Sub FailureHandler(sender As Object, e As CookieAwareWebClient.ErrorEventArgs)
  Private Sub sckVerInfo_Failure(sender As Object, e As CookieAwareWebClient.ErrorEventArgs) Handles sckVerInfo.Failure
    If Me.InvokeRequired Then
      Me.Invoke(New FailureHandler(AddressOf sckVerInfo_Failure), sender, e)
    Else
      pctThrobber.Visible = False
      txtInfo.Text = "Info Request Error" & vbNewLine & e.Error.Message
      If sckVerInfo IsNot Nothing Then
        sckVerInfo.Dispose()
        sckVerInfo = Nothing
      End If
      cmdChanges.Enabled = True
      cmdChanges.Focus()
    End If
  End Sub
End Class
