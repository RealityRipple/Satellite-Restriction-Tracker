Public NotInheritable Class frmCustomColors
  Private lUsed, lLimit As Long
  Private iD As Integer
  Private dDown As Boolean
  Friend mySettings As AppSettings
  Private HasSaved As Boolean = False
  Private FakeData As Collections.Generic.List(Of DataRow)
  Private Sub frmCustomColors_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If cmdSave.Enabled Then
      Dim saveRet As DialogResult = MsgDlg(Me, "Do you want to save the changes to your color scheme?", "Your changes have not been saved.", "Save Changes?", MessageBoxButtons.YesNoCancel, _TaskDialogIcon.Options, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
      If saveRet = Windows.Forms.DialogResult.Yes Then
        cmdSave.PerformClick()
      ElseIf saveRet = Windows.Forms.DialogResult.No Then
        Me.DialogResult = Windows.Forms.DialogResult.No
      ElseIf saveRet = Windows.Forms.DialogResult.Cancel Then
        Me.DialogResult = Windows.Forms.DialogResult.None
        e.Cancel = True
      End If
    End If
    If HasSaved Then
      Me.DialogResult = Windows.Forms.DialogResult.Yes
    Else
      Me.DialogResult = Windows.Forms.DialogResult.No
    End If
  End Sub
  Private Sub frmCustomColors_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    Randomize()
    If lLimit = 0 Then
      lUsed = 8
      lLimit = 16
      dDown = True
      iD = 1
    End If
    If mySettings.Colors.MainDownA = Color.Transparent Then
      mnuAllDefault_Click(mnuAllDefault, New EventArgs)
    Else
      SetElColor(pctMainUsedA, mySettings.Colors.MainDownA)
      SetElColor(pctMainUsedB, mySettings.Colors.MainDownB)
      SetElColor(pctMainUsedC, mySettings.Colors.MainDownC)
      SetElColor(pctMainText, mySettings.Colors.MainText)
      SetElColor(pctMainBG, mySettings.Colors.MainBackground)
      SetElColor(pctTrayUsedA, mySettings.Colors.TrayDownA)
      SetElColor(pctTrayUsedB, mySettings.Colors.TrayDownB)
      SetElColor(pctTrayUsedC, mySettings.Colors.TrayDownC)
      SetElColor(pctHistoryUsedLine, mySettings.Colors.HistoryDownLine)
      SetElColor(pctHistoryUsedA, mySettings.Colors.HistoryDownA)
      SetElColor(pctHistoryUsedB, mySettings.Colors.HistoryDownB)
      SetElColor(pctHistoryUsedC, mySettings.Colors.HistoryDownC)
      SetElColor(pctHistoryUsedMax, mySettings.Colors.HistoryDownMax)
      SetElColor(pctHistoryText, mySettings.Colors.HistoryText)
      SetElColor(pctHistoryBG, mySettings.Colors.HistoryBackground)
      SetElColor(pctHistoryGridL, mySettings.Colors.HistoryLightGrid)
      SetElColor(pctHistoryGridD, mySettings.Colors.HistoryDarkGrid)
    End If
    RedrawImages()
    cmdSave.Enabled = False
    HasSaved = False
  End Sub
#Region "Buttons"
  Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click
    mySettings.Colors.MainDownA = pctMainUsedA.BackColor
    mySettings.Colors.MainDownB = pctMainUsedB.BackColor
    mySettings.Colors.MainDownC = pctMainUsedC.BackColor
    mySettings.Colors.MainText = pctMainText.BackColor
    mySettings.Colors.MainBackground = pctMainBG.BackColor
    mySettings.Colors.TrayDownA = pctTrayUsedA.BackColor
    mySettings.Colors.TrayDownB = pctTrayUsedB.BackColor
    mySettings.Colors.TrayDownC = pctTrayUsedC.BackColor
    mySettings.Colors.HistoryDownLine = pctHistoryUsedLine.BackColor
    mySettings.Colors.HistoryDownA = pctHistoryUsedA.BackColor
    mySettings.Colors.HistoryDownB = pctHistoryUsedB.BackColor
    mySettings.Colors.HistoryDownC = pctHistoryUsedC.BackColor
    mySettings.Colors.HistoryDownMax = pctHistoryUsedMax.BackColor
    mySettings.Colors.HistoryText = pctHistoryText.BackColor
    mySettings.Colors.HistoryBackground = pctHistoryBG.BackColor
    mySettings.Colors.HistoryLightGrid = pctHistoryGridL.BackColor
    mySettings.Colors.HistoryDarkGrid = pctHistoryGridD.BackColor
    mySettings.Save()
    cmdSave.Enabled = False
    HasSaved = True
  End Sub
  Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
    Me.Close()
  End Sub
#End Region
#Region "Clickables"
  Private Sub pctColor_MouseClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles pctMainUsedA.MouseClick, pctMainUsedB.MouseClick, pctMainUsedC.MouseClick,
                                                                                pctMainText.MouseClick, pctMainBG.MouseClick,
                                                                                pctTrayUsedA.MouseClick, pctTrayUsedB.MouseClick, pctTrayUsedC.MouseClick,
                                                                                pctHistoryUsedLine.MouseClick, pctHistoryUsedA.MouseClick, pctHistoryUsedB.MouseClick, pctHistoryUsedC.MouseClick, pctHistoryUsedMax.MouseClick,
                                                                                pctHistoryText.MouseClick, pctHistoryBG.MouseClick, pctHistoryGridL.MouseClick, pctHistoryGridD.MouseClick
    mnuColorOpts.Tag = sender
    Dim pctColor As PictureBox = sender
    If e.Button = Windows.Forms.MouseButtons.Left Then
      mnuChoose_Click(sender, New EventArgs)
    ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
      mnuColorOpts.Show(pctColor, New Point(0, 20))
    End If
    RedrawImages()
  End Sub
  Private Sub pctMain_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctMain.MouseDown
    If dDown Then
      lUsed += iD
      If lUsed >= lLimit Then dDown = False
    Else
      lUsed -= iD
      If lUsed <= 0 Then dDown = True
    End If
    RedrawImages()
  End Sub
  Private Sub pctTray_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctTray.MouseDown
    If dDown Then
      lUsed += iD
      If lUsed >= lLimit Then dDown = False
    Else
      lUsed -= iD
      If lUsed <= 0 Then dDown = True
    End If
    RedrawImages()
  End Sub
  Private Sub chkB_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkMainUsedB.CheckedChanged, chkTrayUsedB.CheckedChanged, chkHistoryUsedB.CheckedChanged
    Dim chkThis As CheckBox = sender
    Dim pctThis As PictureBox = getControlFromName(Me, chkThis.Name.Replace("chk", "pct"))
    If pctThis Is Nothing Then Return
    If chkThis.Checked Then
      pctThis.Enabled = True
      If pctThis.Tag Is Nothing Then
        If pctThis.BackColor = Color.Transparent Then
          pctThis.BackColor = Color.White
        End If
      Else
        pctThis.BackColor = pctThis.Tag
        pctThis.Tag = Nothing
      End If
    Else
      pctThis.Enabled = False
      pctThis.Tag = pctThis.BackColor
      pctThis.BackColor = Color.Transparent
    End If
    cmdSave.Enabled = SettingsChanged()
    RedrawImages()
  End Sub
#End Region
#Region "Menus"
  Private customColors As Integer() = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                     0, 0, 0, 0, 0, 0, 0, 0, 0}
  Private Sub mnuChoose_Click(sender As System.Object, e As System.EventArgs) Handles mnuChoose.Click
    Dim pctColor As PictureBox = mnuColorOpts.Tag
    If pctColor IsNot Nothing Then
      Using dlgColor As New ColorDialog
        dlgColor.Title = getTitleFromName(pctColor.Name)
        dlgColor.Color = pctColor.BackColor
        dlgColor.SolidColorOnly = True
        dlgColor.AnyColor = True
        Dim pColorI As Integer = (CInt(pctColor.BackColor.B) << 16) + (CInt(pctColor.BackColor.G) << 8) + CInt(pctColor.BackColor.R)
        For I As Integer = 0 To customColors.Count - 1
          If customColors(I) = pColorI Then Exit For
          If customColors(I) = 0 Then
            customColors(I) = pColorI
            Exit For
          End If
        Next
        dlgColor.CustomColors = customColors
        If dlgColor.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
          pctColor.BackColor = dlgColor.Color
          RedrawImages()
          cmdSave.Enabled = SettingsChanged()
        End If
        customColors = dlgColor.CustomColors
      End Using
    End If
  End Sub
  Private Sub mnuThisDefault_Click(sender As System.Object, e As System.EventArgs) Handles mnuThisDefault.Click
    Dim pctColor As PictureBox = mnuColorOpts.Tag
    If pctColor IsNot Nothing Then
      SetElColor(pctColor, DefaultColorForElement(pctColor.Name))
      cmdSave.Enabled = SettingsChanged()
    End If
    RedrawImages()
  End Sub
  Private Sub mnuGraphDefault_Click(sender As System.Object, e As System.EventArgs) Handles mnuGraphDefault.Click
    Dim pctColor As PictureBox = mnuColorOpts.Tag
    Dim ColorList As PictureBox()
    If pctColor.Name.StartsWith("pctMain") Then
      ColorList = {pctMainUsedA, pctMainUsedB, pctMainUsedC,
                   pctMainText, pctMainBG}
    ElseIf pctColor.Name.StartsWith("pctTray") Then
      ColorList = {pctTrayUsedA, pctTrayUsedB, pctTrayUsedC}
    Else
      ColorList = {pctHistoryUsedLine, pctHistoryUsedA, pctHistoryUsedB, pctHistoryUsedC, pctHistoryUsedMax,
                   pctHistoryText, pctHistoryBG, pctHistoryGridL, pctHistoryGridD}
    End If
    For Each pColor As PictureBox In ColorList
      Dim bColor As Color = DefaultColorForElement(pColor.Name)
      SetElColor(pColor, bColor)
    Next
    RedrawImages()
    cmdSave.Enabled = SettingsChanged()
  End Sub
  Private Sub mnuAllDefault_Click(sender As System.Object, e As System.EventArgs) Handles mnuAllDefault.Click
    Dim ColorList As PictureBox()
    ColorList = {pctMainUsedA, pctMainUsedB, pctMainUsedC,
                 pctMainText, pctMainBG,
                 pctTrayUsedA, pctTrayUsedB, pctTrayUsedC,
                 pctHistoryUsedLine, pctHistoryUsedA, pctHistoryUsedB, pctHistoryUsedC, pctHistoryUsedMax,
                 pctHistoryText, pctHistoryBG, pctHistoryGridL, pctHistoryGridD}
    For Each pColor As PictureBox In ColorList
      Dim bColor As Color = DefaultColorForElement(pColor.Name)
      SetElColor(pColor, bColor)
    Next
    RedrawImages()
    cmdSave.Enabled = SettingsChanged()
  End Sub
#End Region
#Region "Functions"
  Private Function SettingsChanged() As Boolean
    If Not mySettings.Colors.MainDownA.ToArgb = pctMainUsedA.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.MainDownB.ToArgb = pctMainUsedB.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.MainDownC.ToArgb = pctMainUsedC.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.MainText.ToArgb = pctMainText.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.MainBackground.ToArgb = pctMainBG.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.TrayDownA.ToArgb = pctTrayUsedA.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.TrayDownB.ToArgb = pctTrayUsedB.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.TrayDownC.ToArgb = pctTrayUsedC.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryDownLine.ToArgb = pctHistoryUsedLine.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryDownA.ToArgb = pctHistoryUsedA.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryDownB.ToArgb = pctHistoryUsedB.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryDownC.ToArgb = pctHistoryUsedC.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryDownMax.ToArgb = pctHistoryUsedMax.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryText.ToArgb = pctHistoryText.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryBackground.ToArgb = pctHistoryBG.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryLightGrid.ToArgb = pctHistoryGridL.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryDarkGrid.ToArgb = pctHistoryGridD.BackColor.ToArgb Then Return True
    Return False
  End Function
  Private Function getControlFromName(ByRef containerObj As Object, ByVal name As String) As Control
    Try
      For Each tempCtrl As Control In containerObj.Controls
        If String.Compare(tempCtrl.Name.Trim, name.Trim, StringComparison.OrdinalIgnoreCase) = 0 Then
          Return tempCtrl
        Else
          Dim tC As Control = getControlFromName(tempCtrl, name)
          If tC IsNot Nothing Then Return tC
        End If
      Next tempCtrl
      Return Nothing
    Catch ex As Exception
      Return Nothing
    End Try
  End Function
  Private Sub SetElColor(ByRef pctColor As PictureBox, color As Color)
    pctColor.BackColor = color
    Dim chkThis As CheckBox = getControlFromName(Me, pctColor.Name.Replace("pct", "chk"))
    If chkThis IsNot Nothing Then
      chkThis.Checked = Not pctColor.BackColor = color.Transparent
    End If
    pctColor.Tag = Nothing
  End Sub
  Private Function DefaultColorForElement(Element As String) As Color
    Dim myColors As AppSettings.AppColors = GetDefaultColors()
    Select Case Element
      Case pctMainUsedA.Name : Return myColors.MainDownA
      Case pctMainUsedB.Name : Return myColors.MainDownB
      Case pctMainUsedC.Name : Return myColors.MainDownC
      Case pctMainText.Name : Return myColors.MainText
      Case pctMainBG.Name : Return myColors.MainBackground

      Case pctTrayUsedA.Name : Return myColors.TrayDownA
      Case pctTrayUsedB.Name : Return myColors.TrayDownB
      Case pctTrayUsedC.Name : Return myColors.TrayDownC

      Case pctHistoryUsedLine.Name : Return myColors.HistoryDownLine
      Case pctHistoryUsedA.Name : Return myColors.HistoryDownA
      Case pctHistoryUsedB.Name : Return myColors.HistoryDownB
      Case pctHistoryUsedC.Name : Return myColors.HistoryDownC
      Case pctHistoryUsedMax.Name : Return myColors.HistoryDownMax
      Case pctHistoryText.Name : Return myColors.HistoryText
      Case pctHistoryBG.Name : Return myColors.HistoryBackground
      Case pctHistoryGridL.Name : Return myColors.HistoryLightGrid
      Case pctHistoryGridD.Name : Return myColors.HistoryDarkGrid
      Case Else : Return Color.Transparent
    End Select

  End Function
  Private Function getTitleFromName(name As String) As String
    Select Case name
      Case pctMainUsedA.Name : Return "Main Usage 0% Color"
      Case pctMainUsedB.Name : Return "Main Usage 50% Color"
      Case pctMainUsedC.Name : Return "Main Usage 100% Color"
      Case pctMainText.Name : Return "Main Text Color"
      Case pctMainBG.Name : Return "Main Background Color"

      Case pctTrayUsedA.Name : Return "Tray Usage 0% Color"
      Case pctTrayUsedB.Name : Return "Tray Usage 50% Color"
      Case pctTrayUsedC.Name : Return "Tray Usage 100% Color"

      Case pctHistoryUsedLine.Name : Return "History Usage Data Line"
      Case pctHistoryUsedA.Name : Return "History Usage 0% Color"
      Case pctHistoryUsedB.Name : Return "History Usage 50% Color"
      Case pctHistoryUsedC.Name : Return "History Usage 100% Color"
      Case pctHistoryUsedMax.Name : Return "History Usage Max Limit Color"
      Case pctHistoryText.Name : Return "History Text Color"
      Case pctHistoryBG.Name : Return "History Background Color"
      Case pctHistoryGridL.Name : Return "History Grid Lines Light Color"
      Case pctHistoryGridD.Name : Return "History Grid Lines Dark Color"

      Case Else : Return name & " color"
    End Select
  End Function
  Private Sub MakeFakeData()
    FakeData = New Collections.Generic.List(Of DataRow)
    Dim UsedList As New Collections.Generic.List(Of Integer)
    Dim startDate As New Date(2000, 1, 1, 0, 0, 0)
    Dim startUsed As Integer = 0
    For I As Integer = 1 To 90
      Dim dRow As New DataRow(startDate, startUsed, 15000)
      Dim iUsed As Integer = RandSel(50, 500)
      UsedList.Add(iUsed)
      startUsed += iUsed
      startDate = startDate.AddDays(1)
      If I > 29 Then
        startUsed -= UsedList(I - 30)
      End If
      FakeData.Add(dRow)
    Next
  End Sub
  Private Sub RedrawImages()
    If pctMain.Image IsNot Nothing Then
      pctMain.Image.Dispose()
      pctMain.Image = Nothing
    End If
    Dim iWidth As Integer = pctMain.Width
    Dim iHeight As Integer = pctMain.Height
    Dim iHalfH As Integer = Math.Floor(iHeight / 2)
    pctMain.Image = DisplayRProgress(pctMain.DisplayRectangle.Size, lUsed, lLimit, mySettings.Accuracy, pctMainUsedA.BackColor, pctMainUsedB.BackColor, pctMainUsedC.BackColor, pctMainText.BackColor, pctMainBG.BackColor)
    If pctTray.Image IsNot Nothing Then
      pctTray.Image.Dispose()
      pctTray.Image = Nothing
    End If
    Const traySquare As Integer = 16
    Dim imgTray As New Bitmap(traySquare, traySquare)
    Using g As Graphics = Graphics.FromImage(imgTray)
      g.Clear(Color.Transparent)
      If lUsed >= lLimit Then
        g.DrawIconUnstretched(My.Resources.t16_restricted, New Rectangle(0, 0, traySquare, traySquare))
      Else
        g.DrawIconUnstretched(My.Resources.t16_norm, New Rectangle(0, 0, traySquare, traySquare))
        CreateTrayIcon_Left(g, lUsed, lLimit, pctTrayUsedA.BackColor, pctTrayUsedB.BackColor, pctTrayUsedC.BackColor, traySquare, traySquare)
        CreateTrayIcon_Right(g, lUsed, lLimit, pctTrayUsedA.BackColor, pctTrayUsedB.BackColor, pctTrayUsedC.BackColor, traySquare, traySquare)
      End If
    End Using
    pctTray.Image = imgTray

    iWidth = pctHistory.Width
    iHeight = pctHistory.Height
    iHalfH = Math.Floor(iHeight / 2)
    If pctHistory.Image IsNot Nothing Then
      pctHistory.Image.Dispose()
      pctHistory.Image = Nothing
    End If
    If FakeData Is Nothing OrElse FakeData.Count = 0 Then MakeFakeData()
    Dim FakeHRect As New Rectangle(0, 0, 500, 200)
    Dim FakeR As Image = DrawRGraph(FakeData.ToArray, FakeHRect.Size, pctHistoryUsedLine.BackColor, pctHistoryUsedA.BackColor, pctHistoryUsedB.BackColor, pctHistoryUsedC.BackColor, pctHistoryText.BackColor, pctHistoryBG.BackColor, pctHistoryUsedMax.BackColor, pctHistoryGridL.BackColor, pctHistoryGridD.BackColor)
    Dim fakeI As New Bitmap(iWidth, iHeight)
    Using g As Graphics = Graphics.FromImage(fakeI)
      g.Clear(Color.Black)
      g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
      g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
      g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
      Dim dRect As Rectangle
      If iHeight = 75 Then
        dRect = New Rectangle(0, iHalfH / 2, iWidth, iHalfH)
      Else
        dRect = New Rectangle(0, 0, iWidth, iHeight)
      End If
      g.DrawImage(FakeR, dRect, FakeHRect, GraphicsUnit.Pixel)
    End Using
    pctHistory.Image = fakeI
  End Sub
  Private Shared Function RandSel(Low As Integer, High As Integer) As Integer
    Dim I As Integer = Int(Rnd() * (High - Low)) + Low
    If I = Low Then I = High
    Return I
  End Function
#End Region
End Class
