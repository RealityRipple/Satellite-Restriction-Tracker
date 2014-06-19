Public Class frmAlertSelection
  Friend AlertStyle As String
  Private Changed As Boolean
  Private taskNotifier As TaskbarNotifier = Nothing
  Private Sub frmAlertSelection_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If taskNotifier IsNot Nothing Then
      taskNotifier.Dispose()
      taskNotifier = Nothing
    End If
  End Sub
  Private Sub frmAlertSelection_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    lstStyles.Items.Clear()
    Dim iFirst As Integer = lstStyles.Items.Add("Default")
    If AlertStyle.ToLower = "default" Then lstStyles.SelectedIndex = iFirst
    For Each sFile As String In IO.Directory.GetFiles(AppData)
      Dim sTitle As String = IO.Path.GetFileNameWithoutExtension(sFile)
      Dim sExt As String = IO.Path.GetExtension(sFile).ToLower
      Do While Not String.IsNullOrEmpty(IO.Path.GetExtension(sTitle))
        sExt = IO.Path.GetExtension(sTitle).ToLower & sExt
        sTitle = IO.Path.GetFileNameWithoutExtension(sTitle)
      Loop
      If (sExt = ".tgz") Or (sExt = ".tar.gz") Then
        Dim iItem As Integer = lstStyles.Items.Add(sTitle)
        If sTitle.ToLower = AlertStyle.ToLower Then
          lstStyles.SelectedIndex = iItem
        End If
      End If
    Next
    Changed = False
    lstStyles_SelectedIndexChanged(New Object, EventArgs.Empty)
    cmdSave.Enabled = False
  End Sub
  Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click
    AlertStyle = lstStyles.SelectedItem
    Changed = True
    cmdSave.Enabled = False
  End Sub
  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    If Changed Then
      Me.DialogResult = Windows.Forms.DialogResult.Yes
    Else
      Me.DialogResult = Windows.Forms.DialogResult.No
    End If
  End Sub
  Private Sub lstStyles_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles lstStyles.DragDrop
    If e.Data.GetFormats(True).Contains("FileDrop") Then
      Dim Data = e.Data.GetData("FileDrop")
      If UBound(Data) = 0 Then
        Dim StylePath As String = Data(0)
        Dim sTitle As String = IO.Path.GetFileNameWithoutExtension(StylePath)
        Dim sExt As String = IO.Path.GetExtension(StylePath).ToLower
        Do While Not String.IsNullOrEmpty(IO.Path.GetExtension(sTitle))
          sExt = IO.Path.GetExtension(sTitle).ToLower & sExt
          sTitle = IO.Path.GetFileNameWithoutExtension(sTitle)
        Loop
        If (sExt = ".tgz") Or (sExt = ".tar.gz") Then
          IO.File.Copy(StylePath, AppData & "\" & sTitle & sExt, True)
          Dim iItem As Integer = -1
          If lstStyles.Items.Contains(sTitle) Then
            iItem = lstStyles.FindStringExact(sTitle)
          Else
            iItem = lstStyles.Items.Add(sTitle)
          End If
          If iItem > -1 Then lstStyles.SelectedIndex = iItem
        Else
          e.Effect = DragDropEffects.None
        End If
      ElseIf UBound(Data) > 0 Then
        For Each StylePath As String In Data
          Dim sTitle As String = IO.Path.GetFileNameWithoutExtension(StylePath)
          Dim sExt As String = IO.Path.GetExtension(StylePath).ToLower
          Do While Not String.IsNullOrEmpty(IO.Path.GetExtension(sTitle))
            sExt = IO.Path.GetExtension(sTitle).ToLower & sExt
            sTitle = IO.Path.GetFileNameWithoutExtension(sTitle)
          Loop
          If (sExt = ".tgz") Or (sExt = ".tar.gz") Then
            IO.File.Copy(StylePath, AppData & "\" & sTitle & sExt, True)
            If Not lstStyles.Items.Contains(sTitle) Then lstStyles.Items.Add(sTitle)
          End If
        Next
      Else
        e.Effect = DragDropEffects.None
      End If
    Else
      e.Effect = DragDropEffects.None
    End If
  End Sub
  Private Sub lstStyles_DragEnter(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles lstStyles.DragEnter
    e.Effect = DragDropEffects.Link
  End Sub
  Private Sub lstStyles_DragOver(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles lstStyles.DragOver
    If e.Data.GetFormats(True).Contains("FileDrop") Then
      Dim Data = e.Data.GetData("FileDrop")
      If UBound(Data) = 0 Then
        Dim StylePath As String = Data(0)
        Dim sTitle As String = IO.Path.GetFileNameWithoutExtension(StylePath)
        Dim sExt As String = IO.Path.GetExtension(StylePath).ToLower
        Do While Not String.IsNullOrEmpty(IO.Path.GetExtension(sTitle))
          sExt = IO.Path.GetExtension(sTitle).ToLower & sExt
          sTitle = IO.Path.GetFileNameWithoutExtension(sTitle)
        Loop
        If (sExt = ".tgz") Or (sExt = ".tar.gz") Then
          e.Effect = DragDropEffects.Link
        Else
          e.Effect = DragDropEffects.None
        End If
      ElseIf UBound(Data) > 0 Then
        Dim hasTar As Boolean = False
        For Each StylePath As String In Data
          Dim sTitle As String = IO.Path.GetFileNameWithoutExtension(StylePath)
          Dim sExt As String = IO.Path.GetExtension(StylePath).ToLower
          Do While Not String.IsNullOrEmpty(IO.Path.GetExtension(sTitle))
            sExt = IO.Path.GetExtension(sTitle).ToLower & sExt
            sTitle = IO.Path.GetFileNameWithoutExtension(sTitle)
          Loop
          If (sExt = ".tgz") Or (sExt = ".tar.gz") Then
            hasTar = True
            Exit For
          End If
        Next
        If hasTar Then
          e.Effect = DragDropEffects.Link
        Else
          e.Effect = DragDropEffects.None
        End If
      Else
        e.Effect = DragDropEffects.None
      End If
    Else
      e.Effect = DragDropEffects.None
    End If
  End Sub
  Private Sub lstStyles_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles lstStyles.KeyUp
    If e.KeyCode = Keys.Delete AndAlso lstStyles.SelectedIndices.Count = 1 Then
      Dim index As Integer = lstStyles.SelectedIndex
      Dim sTitle As String = lstStyles.SelectedItem
      If index = 0 Then
        Beep()
      ElseIf MessageBox.Show("Do you want to remove the """ & sTitle & """ Alert Window Style?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification) = Windows.Forms.DialogResult.Yes Then
        If IO.File.Exists(AppData & "\" & sTitle & ".tar.gz") Then
          lstStyles.SelectedIndex = 0
          IO.File.Delete(AppData & "\" & sTitle & ".tar.gz")
          lstStyles.Items.RemoveAt(index)
        ElseIf IO.File.Exists(AppData & "\" & sTitle & ".tgz") Then
          lstStyles.SelectedIndex = 0
          IO.File.Delete(AppData & "\" & sTitle & ".tgz")
          lstStyles.Items.RemoveAt(index)
        Else
          MessageBox.Show("No file by that name was found! Alert Window Style may already be removed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification)
          lstStyles.SelectedIndex = 0
          lstStyles.Items.RemoveAt(index)
        End If
      End If
    End If
  End Sub
  Private Sub lstStyles_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstStyles.SelectedIndexChanged
    Try
      If pctPreview.BackgroundImage IsNot Nothing Then
        pctPreview.BackgroundImage.Dispose()
        pctPreview.BackgroundImage = Nothing
      End If
      If pctPreview.Image IsNot Nothing Then
        pctPreview.Image.Dispose()
        pctPreview.Image = Nothing
      End If
      Dim notifyTest As NotifierStyle = LoadAlertStyle(lstStyles.SelectedItem)
      Using bmpTest As New Bitmap(notifyTest.Background)
        bmpTest.MakeTransparent(notifyTest.TransparencyKey)
        Using g As Graphics = Graphics.FromImage(bmpTest)
          Dim CloseBitmap As New Bitmap(notifyTest.CloseButton)
          CloseBitmap.MakeTransparent(notifyTest.TransparencyKey)
          g.DrawImageUnscaledAndClipped(CloseBitmap, New Rectangle(notifyTest.CloseLocation, New Size(notifyTest.CloseButton.Width / 3, notifyTest.CloseButton.Height)))
          Dim tsf As New StringFormat()
          tsf.Alignment = StringAlignment.Near
          tsf.LineAlignment = StringAlignment.Center
          tsf.FormatFlags = StringFormatFlags.NoWrap
          tsf.Trimming = StringTrimming.EllipsisCharacter
          g.DrawString("Important Alert Information", New Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel), New SolidBrush(notifyTest.TitleColor), notifyTest.TitleLocation, tsf)
          Dim csf As New StringFormat()
          csf.Alignment = StringAlignment.Center
          csf.LineAlignment = StringAlignment.Center
          csf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces
          csf.Trimming = StringTrimming.Word
          g.DrawString("This alert is just an example to display what an actual alert would look like using the """ & lstStyles.SelectedItem & """ style.", New Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Pixel), New SolidBrush(notifyTest.ContentColor), notifyTest.ContentLocation, csf)
        End Using
        pctPreview.Image = bmpTest.Clone
      End Using
      Using bmpBG As New Bitmap(notifyTest.Background.Width + 16, notifyTest.Background.Height + 16)
        Using g As Graphics = Graphics.FromImage(bmpBG)
          g.Clear(Color.Black)
          g.FillRectangle(New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(bmpBG.Width, bmpBG.Height), Color.SeaGreen, Color.Blue), New Rectangle(New Point(0, 0), bmpBG.Size))
        End Using
        pctPreview.BackgroundImage = bmpBG.Clone
      End Using
      pctPreview.Size = pctPreview.BackgroundImage.Size
      cmdSave.Enabled = Not (lstStyles.SelectedItem.ToLower = AlertStyle.ToLower)
    Catch ex As Exception
      pctPreview.BackgroundImage = Nothing
      pctPreview.Image = pctPreview.ErrorImage.Clone
      cmdSave.Enabled = False
    End Try
  End Sub
  Private Sub pctPreview_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctPreview.MouseUp
    If e.Button = Windows.Forms.MouseButtons.Right Then
      Clipboard.SetImage(pctPreview.Image)
    ElseIf e.Button = Windows.Forms.MouseButtons.Middle Then
      If taskNotifier IsNot Nothing Then
        taskNotifier.Dispose()
        taskNotifier = Nothing
      End If
      MakeNotifier(taskNotifier, False, LoadAlertStyle(lstStyles.SelectedItem))
      If taskNotifier IsNot Nothing Then taskNotifier.Show("Hello World", "This alert is a live example of your currently selected alert. What do you think?", 200, 5000, 100)
    End If
  End Sub
  Private Sub lblMore_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblMore.LinkClicked
    Try
      Process.Start("http://srt.realityripple.com/Alert_Styles/")
    Catch ex As Exception
      Dim taskNotifier As TaskbarNotifier = Nothing
      MakeNotifier(taskNotifier, False)
      If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", Application.ProductName & " could not navigate to srt.realityripple.com/changes.php!" & vbNewLine & ex.Message, 200, 3000, 100)
    End Try
  End Sub
End Class
