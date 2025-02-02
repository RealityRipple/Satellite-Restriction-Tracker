Friend NotInheritable Class frmUpdate
  Private Ret As Boolean
  Private Sub frmUpdate_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    Ret = False
  End Sub
  Private Sub frmUpdate_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If Not Ret Then Me.DialogResult = Windows.Forms.DialogResult.No
  End Sub
  Public Sub NewUpdate(Version As String, BETA As Boolean, UACIcon As Boolean)
    If BETA Then
      Me.Text = "New BETA Version Available - " & My.Application.Info.ProductName
      lblTitle.Text = My.Application.Info.ProductName & " BETA Update"
    Else
      Me.Text = "New Version Available - " & My.Application.Info.ProductName
      lblTitle.Text = My.Application.Info.ProductName & " Update"
    End If
    Dim newVer As String = "Version %v has been released and is available for download." & vbNewLine &
             "To keep up-to-date with the latest features, improvements, bug fixes, and" & vbNewLine &
             "meter compliance, please update %p immediately."
    newVer = newVer.Replace("%v", DisplayVersion(Version))
    newVer = newVer.Replace("%p", My.Application.Info.ProductName)
    lblNewVer.Text = newVer
    txtInfo.Text = "Loading Update Information" & vbNewLine & vbNewLine & "Please Wait..."
    lblBETA.Visible = BETA
    chkStopBETA.Visible = BETA
    If UACIcon Then NativeMethods.SendMessage(cmdDownload.Handle, NativeMethods.BCM_SETSHIELD, IntPtr.Zero, 1)
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
        Dim tInfo As New Threading.Thread(AddressOf GetVerInfo)
        tInfo.Start()
      End If
    End If
  End Sub
  Private Sub GetVerInfo()
    Dim sRet As String = Nothing
    Dim sckVerInfo As New WebClientEx
    sckVerInfo.KeepAlive = False
    If lblBETA.Visible Then
      sRet = sckVerInfo.DownloadString("http://update.realityripple.com/Satellite_Restriction_Tracker/infob.ver?sha=512")
    Else
      sRet = sckVerInfo.DownloadString("http://update.realityripple.com/Satellite_Restriction_Tracker/info.ver?sha=512")
    End If
    Dim sHash As String = Nothing
    For Each sKey As String In sckVerInfo.ResponseHeaders
      If sKey.ToLower = "x-update-signature" Then
        sHash = sckVerInfo.ResponseHeaders(sKey)
        Exit For
      End If
    Next
    If clsUpdate.VerifySignature(sRet, sHash) Then
      SetVerInfo(sRet)
    Else
      SetVerInfo("Error: Signature could not be verified!")
    End If
  End Sub
  Private Delegate Sub SetVerInfoCallback(Message As String)
  Private Sub SetVerInfo(Message As String)
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New SetVerInfoCallback(AddressOf SetVerInfo), Message)
      Catch ex As Exception
      End Try
      Return
    End If
    pctThrobber.Visible = False
    txtInfo.Text = Message
    cmdChanges.Enabled = True
    cmdChanges.Focus()
  End Sub
End Class
