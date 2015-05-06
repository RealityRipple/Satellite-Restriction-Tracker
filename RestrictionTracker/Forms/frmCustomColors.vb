Imports RestrictionLibrary.localRestrictionTracker
Public Class frmCustomColors
  Private lDown, lUp, lDownLim, lUpLim As Long
  Private iD, iU As Integer
  Private dDown, dUp As Boolean
  Friend mySettings As AppSettings
  Private HasSaved As Boolean = False
  Private DisplayAs As SatHostTypes
  Private useStyle As SatHostTypes = SatHostTypes.Other
  Private FakeData As Collections.Generic.List(Of DataBase.DataRow)
  Private Sub frmCustomColors_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If cmdSave.Enabled Then
      Dim saveRet As DialogResult = MessageBox.Show("Some settings have been changed but not saved." & vbNewLine & vbNewLine & "Do you want to save the changes to your color scheme?", Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
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
  Private Sub frmCustomColors_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    If mySettings IsNot Nothing Then
      Select Case mySettings.AccountType
        Case SatHostTypes.WildBlue_LEGACY, SatHostTypes.RuralPortal_LEGACY
          grpMain.Text = "Main Window Current Usage Graphs"
          grpMainDown.Text = "Download Colors"
          grpMainUp.Text = "Upload Colors"
          grpMainUp.Visible = True
          grpTray.Text = "Tray Icon Current Usage Graph Overlay"
          grpTrayDown.Text = "Download Colors"
          grpTrayUp.Text = "Upload Colors"
          grpTrayUp.Visible = True
          grpHistory.Text = "History Window Graphs"
          grpHistoryDown.Text = "Download Colors"
          grpHistoryUp.Text = "Upload Colors"
          grpHistoryUp.Visible = True
          lblHistoryUpMax.Visible = True
          pctHistoryUpMax.Visible = True
          DisplayAs = SatHostTypes.WildBlue_LEGACY
          SetTextBGAlignments(True)
        Case SatHostTypes.DishNet_EXEDE
          grpMain.Text = "Main Window Current Usage Graphs"
          grpMainDown.Text = "Anytime Colors"
          grpMainUp.Text = "Off-Peak Colors"
          grpMainUp.Visible = True
          grpTray.Text = "Tray Icon Current Usage Graph Overlay"
          grpTrayDown.Text = "Anytime Colors"
          grpTrayUp.Text = "Off-Peak Colors"
          grpTrayUp.Visible = True
          grpHistory.Text = "History Window Graphs"
          grpHistoryDown.Text = "Anytime Colors"
          grpHistoryUp.Text = "Off-Peak Colors"
          grpHistoryUp.Visible = True
          lblHistoryUpMax.Visible = True
          pctHistoryUpMax.Visible = True
          DisplayAs = SatHostTypes.DishNet_EXEDE
          SetTextBGAlignments(True)
        Case SatHostTypes.RuralPortal_EXEDE, SatHostTypes.WildBlue_EXEDE
          grpMain.Text = "Main Window Graph"
          grpMainDown.Text = "Usage Colors"
          grpMainUp.Visible = False
          grpTray.Text = "Tray Icon Graph Overlay"
          grpTrayDown.Text = "Usage Colors"
          grpTrayUp.Visible = False
          grpHistory.Text = "History Window Graph"
          grpHistoryDown.Text = "Usage Colors"
          grpHistoryUp.Visible = False
          lblHistoryUpMax.Visible = False
          pctHistoryUpMax.Visible = False
          DisplayAs = SatHostTypes.RuralPortal_EXEDE
          SetTextBGAlignments(False)
        Case Else
          grpMain.Text = "Main Window Current Usage Graphs"
          grpMainDown.Text = "Download Colors"
          grpMainUp.Text = "Upload Colors"
          grpMainUp.Visible = True
          grpTray.Text = "Tray Icon Current Usage Graph Overlay"
          grpTrayDown.Text = "Download Colors"
          grpTrayUp.Text = "Upload Colors"
          grpTrayUp.Visible = True
          grpHistory.Text = "History Window Graphs"
          grpHistoryDown.Text = "Download Colors"
          grpHistoryUp.Text = "Upload Colors"
          grpHistoryUp.Visible = True
          lblHistoryUpMax.Visible = True
          pctHistoryUpMax.Visible = True
          DisplayAs = SatHostTypes.Other
          SetTextBGAlignments(True)
      End Select
    End If
  End Sub
  Private Sub frmCustomColors_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    Randomize()
    If lDownLim = 0 And lUpLim = 0 Then
      lDown = 8
      lDownLim = 16
      dDown = True
      iD = 1
      lUp = 4
      lUpLim = 16
      dUp = True
      iU = 1
    End If
    useStyle = mySettings.AccountType
    Select Case useStyle
      Case SatHostTypes.WildBlue_LEGACY, SatHostTypes.RuralPortal_LEGACY
        grpMain.Text = "Main Window Current Usage Graphs"
        grpMainDown.Text = "Download Colors"
        grpMainUp.Text = "Upload Colors"
        grpMainUp.Visible = True
        grpTray.Text = "Tray Icon Current Usage Graph Overlay"
        grpTrayDown.Text = "Download Colors"
        grpTrayUp.Text = "Upload Colors"
        grpTrayUp.Visible = True
        grpHistory.Text = "History Window Graphs"
        grpHistoryDown.Text = "Download Colors"
        grpHistoryUp.Text = "Upload Colors"
        grpHistoryUp.Visible = True
        lblHistoryUpMax.Visible = True
        pctHistoryUpMax.Visible = True
        DisplayAs = SatHostTypes.WildBlue_LEGACY
        SetTextBGAlignments(True)
      Case SatHostTypes.DishNet_EXEDE
        grpMain.Text = "Main Window Current Usage Graphs"
        grpMainDown.Text = "Anytime Colors"
        grpMainUp.Text = "Off-Peak Colors"
        grpMainUp.Visible = True
        grpTray.Text = "Tray Icon Current Usage Graph Overlay"
        grpTrayDown.Text = "Anytime Colors"
        grpTrayUp.Text = "Off-Peak Colors"
        grpTrayUp.Visible = True
        grpHistory.Text = "History Window Graphs"
        grpHistoryDown.Text = "Anytime Colors"
        grpHistoryUp.Text = "Off-Peak Colors"
        grpHistoryUp.Visible = True
        lblHistoryUpMax.Visible = True
        pctHistoryUpMax.Visible = True
        DisplayAs = SatHostTypes.DishNet_EXEDE
        SetTextBGAlignments(True)
      Case SatHostTypes.RuralPortal_EXEDE, SatHostTypes.WildBlue_EXEDE
        grpMain.Text = "Main Window Graph"
        grpMainDown.Text = "Usage Colors"
        grpMainUp.Visible = False
        grpTray.Text = "Tray Icon Graph Overlay"
        grpTrayDown.Text = "Usage Colors"
        grpTrayUp.Visible = False
        grpHistory.Text = "History Window Graph"
        grpHistoryDown.Text = "Usage Colors"
        grpHistoryUp.Visible = False
        lblHistoryUpMax.Visible = False
        pctHistoryUpMax.Visible = False
        DisplayAs = SatHostTypes.RuralPortal_EXEDE
        SetTextBGAlignments(False)
      Case Else
        grpMain.Text = "Main Window Current Usage Graphs"
        grpMainDown.Text = "Download Colors"
        grpMainUp.Text = "Upload Colors"
        grpMainUp.Visible = True
        grpTray.Text = "Tray Icon Current Usage Graph Overlay"
        grpTrayDown.Text = "Download Colors"
        grpTrayUp.Text = "Upload Colors"
        grpTrayUp.Visible = True
        grpHistory.Text = "History Window Graphs"
        grpHistoryDown.Text = "Download Colors"
        grpHistoryUp.Text = "Upload Colors"
        grpHistoryUp.Visible = True
        lblHistoryUpMax.Visible = True
        pctHistoryUpMax.Visible = True
        DisplayAs = SatHostTypes.Other
        SetTextBGAlignments(True)
    End Select
    If mySettings.Colors.MainDownA = Color.Transparent Then
      mnuAllDefault_Click(mnuAllDefault, New EventArgs)
    Else
      SetElColor(pctMainDownA, mySettings.Colors.MainDownA)
      SetElColor(pctMainDownB, mySettings.Colors.MainDownB)
      SetElColor(pctMainDownC, mySettings.Colors.MainDownC)
      SetElColor(pctMainUpA, mySettings.Colors.MainUpA)
      SetElColor(pctMainUpB, mySettings.Colors.MainUpB)
      SetElColor(pctMainUpC, mySettings.Colors.MainUpC)
      SetElColor(pctMainText, mySettings.Colors.MainText)
      SetElColor(pctMainBG, mySettings.Colors.MainBackground)
      SetElColor(pctTrayDownA, mySettings.Colors.TrayDownA)
      SetElColor(pctTrayDownB, mySettings.Colors.TrayDownB)
      SetElColor(pctTrayDownC, mySettings.Colors.TrayDownC)
      SetElColor(pctTrayUpA, mySettings.Colors.TrayUpA)
      SetElColor(pctTrayUpB, mySettings.Colors.TrayUpB)
      SetElColor(pctTrayUpC, mySettings.Colors.TrayUpC)
      SetElColor(pctHistoryDownLine, mySettings.Colors.HistoryDownLine)
      SetElColor(pctHistoryDownA, mySettings.Colors.HistoryDownA)
      SetElColor(pctHistoryDownB, mySettings.Colors.HistoryDownB)
      SetElColor(pctHistoryDownC, mySettings.Colors.HistoryDownC)
      SetElColor(pctHistoryDownMax, mySettings.Colors.HistoryDownMax)
      SetElColor(pctHistoryUpLine, mySettings.Colors.HistoryUpLine)
      SetElColor(pctHistoryUpA, mySettings.Colors.HistoryUpA)
      SetElColor(pctHistoryUpB, mySettings.Colors.HistoryUpB)
      SetElColor(pctHistoryUpC, mySettings.Colors.HistoryUpC)
      SetElColor(pctHistoryUpMax, mySettings.Colors.HistoryUpMax)
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
    mySettings.Colors.MainDownA = pctMainDownA.BackColor
    mySettings.Colors.MainDownB = pctMainDownB.BackColor
    mySettings.Colors.MainDownC = pctMainDownC.BackColor
    mySettings.Colors.MainUpA = pctMainUpA.BackColor
    mySettings.Colors.MainUpB = pctMainUpB.BackColor
    mySettings.Colors.MainUpC = pctMainUpC.BackColor
    mySettings.Colors.MainText = pctMainText.BackColor
    mySettings.Colors.MainBackground = pctMainBG.BackColor
    mySettings.Colors.TrayDownA = pctTrayDownA.BackColor
    mySettings.Colors.TrayDownB = pctTrayDownB.BackColor
    mySettings.Colors.TrayDownC = pctTrayDownC.BackColor
    mySettings.Colors.TrayUpA = pctTrayUpA.BackColor
    mySettings.Colors.TrayUpB = pctTrayUpB.BackColor
    mySettings.Colors.TrayUpC = pctTrayUpC.BackColor
    mySettings.Colors.HistoryDownLine = pctHistoryDownLine.BackColor
    mySettings.Colors.HistoryDownA = pctHistoryDownA.BackColor
    mySettings.Colors.HistoryDownB = pctHistoryDownB.BackColor
    mySettings.Colors.HistoryDownC = pctHistoryDownC.BackColor
    mySettings.Colors.HistoryDownMax = pctHistoryDownMax.BackColor
    mySettings.Colors.HistoryUpLine = pctHistoryUpLine.BackColor
    mySettings.Colors.HistoryUpA = pctHistoryUpA.BackColor
    mySettings.Colors.HistoryUpB = pctHistoryUpB.BackColor
    mySettings.Colors.HistoryUpC = pctHistoryUpC.BackColor
    mySettings.Colors.HistoryUpMax = pctHistoryUpMax.BackColor
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
  Private Sub pctColor_MouseClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles pctMainDownA.MouseClick, pctMainDownB.MouseClick, pctMainDownC.MouseClick,
                                                                                pctMainUpA.MouseClick, pctMainUpB.MouseClick, pctMainUpC.MouseClick,
                                                                                pctMainText.MouseClick, pctMainBG.MouseClick,
                                                                                pctTrayDownA.MouseClick, pctTrayDownB.MouseClick, pctTrayDownC.MouseClick,
                                                                                pctTrayUpA.MouseClick, pctTrayUpB.MouseClick, pctTrayUpC.MouseClick,
                                                                                pctHistoryDownLine.MouseClick, pctHistoryDownA.MouseClick, pctHistoryDownB.MouseClick, pctHistoryDownC.MouseClick, pctHistoryDownMax.MouseClick,
                                                                                pctHistoryUpLine.MouseClick, pctHistoryUpA.MouseClick, pctHistoryUpB.MouseClick, pctHistoryUpC.MouseClick, pctHistoryUpMax.MouseClick,
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
    If e.Button = Windows.Forms.MouseButtons.Left Then
      If dDown Then
        lDown += iD
        If lDown >= lDownLim Then dDown = False
      Else
        lDown -= iD
        If lDown <= 0 Then dDown = True
      End If
    ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
      If dUp Then
        lUp += iU
        If lUp >= lUpLim Then dUp = False
      Else
        lUp -= iU
        If lUp <= 0 Then dUp = True
      End If
    End If
    RedrawImages()
  End Sub
  Private Sub pctTray_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctTray.MouseDown
    If e.Button = Windows.Forms.MouseButtons.Left Then
      If dDown Then
        lDown += iD
        If lDown >= lDownLim Then dDown = False
      Else
        lDown -= iD
        If lDown <= 0 Then dDown = True
      End If
    ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
      If dUp Then
        lUp += iU
        If lUp >= lUpLim Then dUp = False
      Else
        lUp -= iU
        If lUp <= 0 Then dUp = True
      End If
    End If
    RedrawImages()
  End Sub
  Private Sub chkB_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkMainDownB.CheckedChanged, chkMainUpB.CheckedChanged, chkTrayDownB.CheckedChanged, chkTrayUpB.CheckedChanged, chkHistoryDownB.CheckedChanged, chkHistoryUpB.CheckedChanged
    Dim chkThis As CheckBox = sender
    Dim pctThis As PictureBox = getControlFromName(Me, chkThis.Name.Replace("chk", "pct"))
    If pctThis Is Nothing Then Exit Sub
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
      SetElColor(pctColor, DefaultColorForElement(pctColor.Name, useStyle))
      cmdSave.Enabled = SettingsChanged()
    End If
    RedrawImages()
  End Sub
  Private Sub mnuGraphDefault_Click(sender As System.Object, e As System.EventArgs) Handles mnuGraphDefault.Click
    Dim pctColor As PictureBox = mnuColorOpts.Tag
    Dim ColorList As PictureBox()
    If pctColor.Name.StartsWith("pctMain") Then
      ColorList = {pctMainDownA, pctMainDownB, pctMainDownC,
                   pctMainUpA, pctMainUpB, pctMainUpC,
                   pctMainText, pctMainBG}
    ElseIf pctColor.Name.StartsWith("pctTray") Then
      ColorList = {pctTrayDownA, pctTrayDownB, pctTrayDownC,
                   pctTrayUpA, pctTrayUpB, pctTrayUpC}
    Else
      ColorList = {pctHistoryDownLine, pctHistoryDownA, pctHistoryDownB, pctHistoryDownC, pctHistoryDownMax,
                   pctHistoryUpLine, pctHistoryUpA, pctHistoryUpB, pctHistoryUpC, pctHistoryUpMax,
                   pctHistoryText, pctHistoryBG, pctHistoryGridL, pctHistoryGridD}
    End If
    For Each pColor As PictureBox In ColorList
      Dim bColor As Color = DefaultColorForElement(pColor.Name, useStyle)
      SetElColor(pColor, bColor)
    Next
    RedrawImages()
    cmdSave.Enabled = SettingsChanged()
  End Sub
  Private Sub mnuAllDefault_Click(sender As System.Object, e As System.EventArgs) Handles mnuAllDefault.Click
    Dim pctColor As PictureBox = mnuColorOpts.Tag
    Dim ColorList As PictureBox()
    ColorList = {pctMainDownA, pctMainDownB, pctMainDownC,
                 pctMainUpA, pctMainUpB, pctMainUpC,
                 pctMainText, pctMainBG,
                 pctTrayDownA, pctTrayDownB, pctTrayDownC,
                 pctTrayUpA, pctTrayUpB, pctTrayUpC,
                 pctHistoryDownLine, pctHistoryDownA, pctHistoryDownB, pctHistoryDownC, pctHistoryDownMax,
                 pctHistoryUpLine, pctHistoryUpA, pctHistoryUpB, pctHistoryUpC, pctHistoryUpMax,
                 pctHistoryText, pctHistoryBG, pctHistoryGridL, pctHistoryGridD}
    For Each pColor As PictureBox In ColorList
      Dim bColor As Color = DefaultColorForElement(pColor.Name, useStyle)
      SetElColor(pColor, bColor)
    Next
    RedrawImages()
    cmdSave.Enabled = SettingsChanged()
  End Sub
#End Region
#Region "Functions"
  Private Function SettingsChanged() As Boolean
    If Not mySettings.Colors.MainDownA.ToArgb = pctMainDownA.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.MainDownB.ToArgb = pctMainDownB.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.MainDownC.ToArgb = pctMainDownC.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.MainUpA.ToArgb = pctMainUpA.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.MainUpB.ToArgb = pctMainUpB.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.MainUpC.ToArgb = pctMainUpC.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.MainText.ToArgb = pctMainText.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.MainBackground.ToArgb = pctMainBG.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.TrayDownA.ToArgb = pctTrayDownA.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.TrayDownB.ToArgb = pctTrayDownB.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.TrayDownC.ToArgb = pctTrayDownC.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.TrayUpA.ToArgb = pctTrayUpA.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.TrayUpB.ToArgb = pctTrayUpB.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.TrayUpC.ToArgb = pctTrayUpC.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryDownLine.ToArgb = pctHistoryDownLine.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryDownA.ToArgb = pctHistoryDownA.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryDownB.ToArgb = pctHistoryDownB.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryDownC.ToArgb = pctHistoryDownC.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryDownMax.ToArgb = pctHistoryDownMax.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryUpLine.ToArgb = pctHistoryUpLine.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryUpA.ToArgb = pctHistoryUpA.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryUpB.ToArgb = pctHistoryUpB.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryUpC.ToArgb = pctHistoryUpC.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryUpMax.ToArgb = pctHistoryUpMax.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryText.ToArgb = pctHistoryText.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryBackground.ToArgb = pctHistoryBG.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryLightGrid.ToArgb = pctHistoryGridL.BackColor.ToArgb Then Return True
    If Not mySettings.Colors.HistoryDarkGrid.ToArgb = pctHistoryGridD.BackColor.ToArgb Then Return True
    Return False
  End Function
  Private Function getControlFromName(ByRef containerObj As Object, ByVal name As String) As Control
    Try
      For Each tempCtrl As Control In containerObj.Controls
        If tempCtrl.Name.ToUpper.Trim = name.ToUpper.Trim Then
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
  Private Function DefaultColorForElement(Element As String, Provider As SatHostTypes) As Color
    Dim myColors As AppSettings.AppColors = GetDefaultColors(Provider)
    Select Case Element
      Case pctMainDownA.Name : Return myColors.MainDownA
      Case pctMainDownB.Name : Return myColors.MainDownB
      Case pctMainDownC.Name : Return myColors.MainDownC
      Case pctMainUpA.Name : Return myColors.MainUpA
      Case pctMainUpB.Name : Return myColors.MainUpB
      Case pctMainUpC.Name : Return myColors.MainUpC
      Case pctMainText.Name : Return myColors.MainText
      Case pctMainBG.Name : Return myColors.MainBackground

      Case pctTrayDownA.Name : Return myColors.TrayDownA
      Case pctTrayDownB.Name : Return myColors.TrayDownB
      Case pctTrayDownC.Name : Return myColors.TrayDownC
      Case pctTrayUpA.Name : Return myColors.TrayUpA
      Case pctTrayUpB.Name : Return myColors.TrayUpB
      Case pctTrayUpC.Name : Return myColors.TrayUpC

      Case pctHistoryDownLine.Name : Return myColors.HistoryDownLine
      Case pctHistoryDownA.Name : Return myColors.HistoryDownA
      Case pctHistoryDownB.Name : Return myColors.HistoryDownB
      Case pctHistoryDownC.Name : Return myColors.HistoryDownC
      Case pctHistoryDownMax.Name : Return myColors.HistoryDownMax
      Case pctHistoryUpLine.Name : Return myColors.HistoryUpLine
      Case pctHistoryUpA.Name : Return myColors.HistoryUpA
      Case pctHistoryUpB.Name : Return myColors.HistoryUpB
      Case pctHistoryUpC.Name : Return myColors.HistoryUpC
      Case pctHistoryUpMax.Name : Return myColors.HistoryUpMax
      Case pctHistoryText.Name : Return myColors.HistoryText
      Case pctHistoryBG.Name : Return myColors.HistoryBackground
      Case pctHistoryGridL.Name : Return myColors.HistoryLightGrid
      Case pctHistoryGridD.Name : Return myColors.HistoryDarkGrid
      Case Else : Return Color.Transparent
    End Select

  End Function
  Private Function getTitleFromName(name As String) As String
    Select Case name
      Case pctMainDownA.Name : Return "Main Download 0% Color"
      Case pctMainDownB.Name : Return "Main Download 50% Color"
      Case pctMainDownC.Name : Return "Main Download 100% Color"
      Case pctMainUpA.Name : Return "Main Upload 0% Color"
      Case pctMainUpB.Name : Return "Main Upload 50% Color"
      Case pctMainUpC.Name : Return "Main Upload 100% Color"
      Case pctMainText.Name : Return "Main Text Color"
      Case pctMainBG.Name : Return "Main Background Color"

      Case pctTrayDownA.Name : Return "Tray Download 0% Color"
      Case pctTrayDownB.Name : Return "Tray Download 50% Color"
      Case pctTrayDownC.Name : Return "Tray Download 100% Color"
      Case pctTrayUpA.Name : Return "Tray Upload 0% Color"
      Case pctTrayUpB.Name : Return "Tray Upload 50% Color"
      Case pctTrayUpC.Name : Return "Tray Upload 100% Color"

      Case pctHistoryDownLine.Name : Return "History Download Data Line"
      Case pctHistoryDownA.Name : Return "History Download 0% Color"
      Case pctHistoryDownB.Name : Return "History Download 50% Color"
      Case pctHistoryDownC.Name : Return "History Download 100% Color"
      Case pctHistoryDownMax.Name : Return "History Download Max Limit Color"
      Case pctHistoryUpLine.Name : Return "History Upload Data Line"
      Case pctHistoryUpA.Name : Return "History Upload 0% Color"
      Case pctHistoryUpB.Name : Return "History Upload 50% Color"
      Case pctHistoryUpC.Name : Return "History Upload 100% Color"
      Case pctHistoryUpMax.Name : Return "History Upload Max Limit Color"
      Case pctHistoryText.Name : Return "History Text Color"
      Case pctHistoryBG.Name : Return "History Background Color"
      Case pctHistoryGridL.Name : Return "History Grid Lines Light Color"
      Case pctHistoryGridD.Name : Return "History Grid Lines Dark Color"

      Case Else : Return name & " color"
    End Select
  End Function
  Private Sub MakeFakeData()
    FakeData = New Collections.Generic.List(Of DataBase.DataRow)
    Dim DownList As New Collections.Generic.List(Of Integer)
    Dim UpList As New Collections.Generic.List(Of Integer)
    Dim startDate As New Date(2000, 1, 1, 0, 0, 0)
    Dim startDown As Integer = 0
    Dim startUp As Integer = 0
    If DisplayAs = SatHostTypes.WildBlue_LEGACY Then
      For I As Integer = 1 To 90
        Dim dRow As New DataBase.DataRow(startDate, startDown, 12000, startUp, 3000)
        Dim DownUsed As Integer = RandSel(50, 500)
        Dim UpUsed As Integer = RandSel(10, 120)
        DownList.Add(DownUsed)
        UpList.Add(UpUsed)
        startDown += DownUsed
        startUp += UpUsed
        startDate = startDate.AddDays(1)
        If I > 29 Then
          startDown -= DownList(I - 30)
          startUp -= UpList(I - 30)
        End If
        FakeData.Add(dRow)
      Next
    ElseIf DisplayAs = SatHostTypes.RuralPortal_EXEDE Then
      For I As Integer = 1 To 90
        Dim dRow As New DataBase.DataRow(startDate, startDown, 15000, startDown, 15000)
        Dim DownUsed As Integer = RandSel(50, 500)
        DownList.Add(DownUsed)
        startDown += DownUsed
        startDate = startDate.AddDays(1)
        If I > 29 Then
          startDown -= DownList(I - 30)
        End If
        FakeData.Add(dRow)
      Next
    ElseIf DisplayAs = SatHostTypes.DishNet_EXEDE Then
      For I As Integer = 1 To 90
        Dim dRow As New DataBase.DataRow(startDate, startDown, 10000, startUp, 10000)
        Dim DownUsed As Integer = RandSel(50, 500)
        Dim UpUsed As Integer = RandSel(50, 450)
        DownList.Add(DownUsed)
        UpList.Add(UpUsed)
        startDown += DownUsed
        startUp += UpUsed
        startDate = startDate.AddDays(1)
        If I Mod 30 = 0 Then
          startDown = 0
          startUp = 0
        End If
        FakeData.Add(dRow)
      Next
    End If
  End Sub
  Private Sub RedrawImages()
    If pctMain.Image IsNot Nothing Then
      pctMain.Image.Dispose()
      pctMain.Image = Nothing
    End If
    Dim iWidth As Integer = pctMain.Width
    Dim iHalfW As Integer = Math.Floor(iWidth / 2)
    Dim iHeight As Integer = pctMain.Height
    Dim iHalfH As Integer = Math.Floor(iHeight / 2)
    Select Case DisplayAs
      Case SatHostTypes.WildBlue_LEGACY
        Dim FakeMRect As New Rectangle(0, 0, iWidth, iHeight * 2)
        Dim FakeD As Image = DisplayProgress(FakeMRect.Size, lDown, lDownLim, mySettings.Accuracy, pctMainDownA.BackColor, pctMainDownB.BackColor, pctMainDownC.BackColor, pctMainText.BackColor, pctMainBG.BackColor)
        Dim FakeU As Image = DisplayProgress(FakeMRect.Size, lUp, lUpLim, mySettings.Accuracy, pctMainUpA.BackColor, pctMainUpB.BackColor, pctMainUpC.BackColor, pctMainText.BackColor, pctMainBG.BackColor)
        Dim fakeI As New Bitmap(pctHistory.Width, pctHistory.Height)
        Using g As Graphics = Graphics.FromImage(fakeI)
          g.Clear(Color.Black)
          g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
          g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
          g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
          Dim dRect As New Rectangle(0, 0, iHalfW, iHeight)
          Dim uRect As New Rectangle(iHalfW, 0, iHalfW, iHeight)
          g.DrawImage(FakeD, dRect, FakeMRect, GraphicsUnit.Pixel)
          g.DrawImage(FakeU, uRect, FakeMRect, GraphicsUnit.Pixel)
        End Using
        pctMain.Image = fakeI
      Case SatHostTypes.RuralPortal_EXEDE
        pctMain.Image = DisplayRProgress(pctMain.DisplayRectangle.Size, lDown, lDownLim, mySettings.Accuracy, pctMainDownA.BackColor, pctMainDownB.BackColor, pctMainDownC.BackColor, pctMainText.BackColor, pctMainBG.BackColor)
      Case SatHostTypes.DishNet_EXEDE
        Dim FakeMRect As New Rectangle(0, 0, iWidth, iHeight * 2)
        Dim FakeD As Image = DisplayProgress(FakeMRect.Size, lDown, lDownLim, mySettings.Accuracy, pctMainDownA.BackColor, pctMainDownB.BackColor, pctMainDownC.BackColor, pctMainText.BackColor, pctMainBG.BackColor)
        Dim FakeU As Image = DisplayProgress(FakeMRect.Size, lUp, lUpLim, mySettings.Accuracy, pctMainUpA.BackColor, pctMainUpB.BackColor, pctMainUpC.BackColor, pctMainText.BackColor, pctMainBG.BackColor)
        Dim fakeI As New Bitmap(pctHistory.Width, pctHistory.Height)
        Using g As Graphics = Graphics.FromImage(fakeI)
          g.Clear(Color.Black)
          g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
          g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
          g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
          Dim dRect As New Rectangle(0, 0, iHalfW, iHeight)
          Dim uRect As New Rectangle(iHalfW, 0, iHalfW, iHeight)
          g.DrawImage(FakeD, dRect, FakeMRect, GraphicsUnit.Pixel)
          g.DrawImage(FakeU, uRect, FakeMRect, GraphicsUnit.Pixel)
        End Using
        pctMain.Image = fakeI
      Case Else
        pctMain.Image = pctMain.ErrorImage.Clone
    End Select
    If pctTray.Image IsNot Nothing Then
      pctTray.Image.Dispose()
      pctTray.Image = Nothing
    End If
    Const traySquare As Integer = 16
    Dim imgTray As New Bitmap(traySquare, traySquare)
    Using g As Graphics = Graphics.FromImage(imgTray)
      g.Clear(Color.Transparent)
      Select Case DisplayAs
        Case SatHostTypes.WildBlue_LEGACY
          If lDown >= lDownLim Or lUp >= lUpLim Then
            g.DrawIconUnstretched(My.Resources.t16_restricted, New Rectangle(0, 0, traySquare, traySquare))
          Else
            g.DrawIconUnstretched(My.Resources.t16_norm, New Rectangle(0, 0, traySquare, traySquare))
          End If
          CreateTrayIcon_Left(g, lDown, lDownLim, pctTrayDownA.BackColor, pctTrayDownB.BackColor, pctTrayDownC.BackColor, traySquare, traySquare)
          CreateTrayIcon_Right(g, lUp, lUpLim, pctTrayUpA.BackColor, pctTrayUpB.BackColor, pctTrayUpC.BackColor, traySquare, traySquare)
        Case SatHostTypes.RuralPortal_EXEDE
          If lDown >= lDownLim Then
            g.DrawIconUnstretched(My.Resources.t16_restricted, New Rectangle(0, 0, traySquare, traySquare))
          Else
            g.DrawIconUnstretched(My.Resources.t16_norm, New Rectangle(0, 0, traySquare, traySquare))
          End If
          CreateTrayIcon_Left(g, lDown, lDownLim, pctTrayDownA.BackColor, pctTrayDownB.BackColor, pctTrayDownC.BackColor, traySquare, traySquare)
          CreateTrayIcon_Right(g, lDown, lDownLim, pctTrayDownA.BackColor, pctTrayDownB.BackColor, pctTrayDownC.BackColor, traySquare, traySquare)
        Case SatHostTypes.DishNet_EXEDE
          If lDown >= lDownLim Then
            g.DrawIconUnstretched(My.Resources.t16_restricted, New Rectangle(0, 0, traySquare, traySquare))
          Else
            g.DrawIconUnstretched(My.Resources.t16_norm, New Rectangle(0, 0, traySquare, traySquare))
          End If
          CreateTrayIcon_Left(g, lDown, lDownLim, pctTrayDownA.BackColor, pctTrayDownB.BackColor, pctTrayDownC.BackColor, traySquare, traySquare)
          CreateTrayIcon_Right(g, lDown, lDownLim, pctTrayDownA.BackColor, pctTrayDownB.BackColor, pctTrayDownC.BackColor, traySquare, traySquare)
        Case Else
          g.DrawImage(pctTray.ErrorImage, 0, 0)
      End Select
    End Using
    pctTray.Image = imgTray

    If pctHistory.Image IsNot Nothing Then
      pctHistory.Image.Dispose()
      pctHistory.Image = Nothing
    End If
    If FakeData Is Nothing OrElse FakeData.Count = 0 Then MakeFakeData()
    Dim FakeHRect As New Rectangle(0, 0, 500, 200)
    Select Case DisplayAs
      Case SatHostTypes.WildBlue_LEGACY
        Dim FakeD As Image = DrawLineGraph(FakeData.ToArray, True, FakeHRect.Size, pctHistoryDownA.BackColor, pctHistoryDownB.BackColor, pctHistoryDownC.BackColor, pctHistoryText.BackColor, pctHistoryBG.BackColor, pctHistoryDownMax.BackColor)
        Dim FakeU As Image = DrawLineGraph(FakeData.ToArray, False, FakeHRect.Size, pctHistoryUpA.BackColor, pctHistoryUpB.BackColor, pctHistoryUpC.BackColor, pctHistoryText.BackColor, pctHistoryBG.BackColor, pctHistoryUpMax.BackColor)
        Dim fakeI As New Bitmap(pctHistory.Width, pctHistory.Height)
        Using g As Graphics = Graphics.FromImage(fakeI)
          g.Clear(Color.Black)
          g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
          g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
          g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
          Dim dRect As New Rectangle(0, iHalfH * 0.1, iWidth, iHalfH * 0.85)
          Dim uRect As New Rectangle(0, iHalfH + (iHalfH * 0.05), iWidth, iHalfH * 0.85)
          g.DrawImage(FakeD, dRect, FakeHRect, GraphicsUnit.Pixel)
          g.DrawImage(FakeU, uRect, FakeHRect, GraphicsUnit.Pixel)
        End Using
        pctHistory.Image = fakeI
      Case SatHostTypes.RuralPortal_EXEDE
        Dim FakeR As Image = DrawRGraph(FakeData.ToArray, FakeHRect.Size, pctHistoryDownA.BackColor, pctHistoryDownB.BackColor, pctHistoryDownC.BackColor, pctHistoryText.BackColor, pctHistoryBG.BackColor, pctHistoryDownMax.BackColor, pctHistoryDownLine.BackColor, pctHistoryGridL.BackColor, pctHistoryGridD.BackColor)
        Dim fakeI As New Bitmap(pctHistory.Width, pctHistory.Height)
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
      Case SatHostTypes.DishNet_EXEDE
        Dim FakeD As Image = DrawLineGraph(FakeData.ToArray, True, FakeHRect.Size, pctHistoryDownA.BackColor, pctHistoryDownB.BackColor, pctHistoryDownC.BackColor, pctHistoryText.BackColor, pctHistoryBG.BackColor, pctHistoryDownMax.BackColor)
        Dim FakeU As Image = DrawLineGraph(FakeData.ToArray, False, FakeHRect.Size, pctHistoryUpA.BackColor, pctHistoryUpB.BackColor, pctHistoryUpC.BackColor, pctHistoryText.BackColor, pctHistoryBG.BackColor, pctHistoryUpMax.BackColor)
        Dim fakeI As New Bitmap(pctHistory.Width, pctHistory.Height)
        Using g As Graphics = Graphics.FromImage(fakeI)
          g.Clear(Color.Black)
          g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
          g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
          g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
          Dim dRect As New Rectangle(0, iHalfH * 0.1, iWidth, iHalfH * 0.85)
          Dim uRect As New Rectangle(0, iHalfH + (iHalfH * 0.05), iWidth, iHalfH * 0.85)
          g.DrawImage(FakeD, dRect, FakeHRect, GraphicsUnit.Pixel)
          g.DrawImage(FakeU, uRect, FakeHRect, GraphicsUnit.Pixel)
        End Using
        pctHistory.Image = fakeI
      Case Else
        pctHistory.Image = pctHistory.ErrorImage.Clone
    End Select
  End Sub
  Private Function RandSel(Low As Integer, High As Integer) As Integer
    Dim I As Integer = Int(Rnd() * (High - Low)) + Low
    If I = Low Then I = High
    Return I
  End Function
  Private Sub SetTextBGAlignments(Horizontal As Boolean)
    If Horizontal Then
      Dim preSize As New Size(75, 75)
      pnlCustomColors.ColumnCount = 2
      pnlCustomColors.ColumnStyles(0) = New ColumnStyle(SizeType.Percent, 100)
      pnlCustomColors.ColumnStyles(1) = New ColumnStyle(SizeType.AutoSize)
      pnlCustomColors.RowCount = 4
      pnlCustomColors.RowStyles(0) = New RowStyle(SizeType.AutoSize)
      pnlCustomColors.RowStyles(1) = New RowStyle(SizeType.AutoSize)
      If pnlCustomColors.RowStyles.Count = 2 Then
        pnlCustomColors.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        pnlCustomColors.RowStyles.Add(New RowStyle(SizeType.AutoSize))
      ElseIf pnlCustomColors.RowStyles.Count = 3 Then
        pnlCustomColors.RowStyles(2) = New RowStyle(SizeType.AutoSize)
        pnlCustomColors.RowStyles.Add(New RowStyle(SizeType.AutoSize))
      Else
        pnlCustomColors.RowStyles(2) = New RowStyle(SizeType.AutoSize)
        pnlCustomColors.RowStyles(3) = New RowStyle(SizeType.AutoSize)
      End If
      pnlCustomColors.SetRow(pnlButtons, 3)
      pnlCustomColors.SetColumn(pnlButtons, 0)
      pnlCustomColors.SetColumnSpan(pnlButtons, 2)

      pnlCustomColors.SetRow(grpHistory, 2)
      pnlCustomColors.SetColumn(grpHistory, 0)
      If pnlHistory.Controls.Contains(pctHistory) Then
        pnlHistory.Controls.Remove(pctHistory)
        pnlHistory.RowCount = 2
        pnlHistory.RowStyles(0) = New RowStyle(SizeType.AutoSize)
        pnlHistory.RowStyles(1) = New RowStyle(SizeType.AutoSize)
        pnlHistory.SetRow(grpHistoryDown, 0)
        pnlHistory.SetRow(pnlHistoryStyle, 1)
        pnlCustomColors.Controls.Add(pctHistory, 1, 2)
        pctHistory.Size = preSize
      End If
      pnlHistory.ColumnStyles(0) = New ColumnStyle(SizeType.Percent, 50)
      pnlHistory.ColumnStyles(1) = New ColumnStyle(SizeType.Percent, 50)
      pnlHistoryDown.ColumnCount = 3
      pnlHistoryDown.RowCount = 5
      pnlHistoryDown.SetRow(lblHistoryDownMax, 4)
      pnlHistoryDown.SetColumn(lblHistoryDownMax, 0)
      pnlHistoryDown.SetRow(pctHistoryDownMax, 4)
      pnlHistoryDown.SetColumn(pctHistoryDownMax, 2)
      pnlHistoryDown.SetRow(lblHistoryDownC, 3)
      pnlHistoryDown.SetColumn(lblHistoryDownC, 0)
      pnlHistoryDown.SetRow(pctHistoryDownC, 3)
      pnlHistoryDown.SetColumn(pctHistoryDownC, 2)
      pnlHistoryDown.SetRow(chkHistoryDownB, 2)
      pnlHistoryDown.SetColumn(chkHistoryDownB, 0)
      pnlHistoryDown.SetRow(pctHistoryDownB, 2)
      pnlHistoryDown.SetColumn(pctHistoryDownB, 2)
      pnlHistoryDown.SetRow(lblHistoryDownA, 1)
      pnlHistoryDown.SetColumn(lblHistoryDownA, 0)
      pnlHistoryDown.SetRow(pctHistoryDownA, 1)
      pnlHistoryDown.SetColumn(pctHistoryDownA, 2)
      pnlHistoryDown.SetRow(lblHistoryDownLine, 0)
      pnlHistoryDown.SetColumn(lblHistoryDownLine, 0)
      pnlHistoryDown.SetRow(pctHistoryDownLine, 0)
      pnlHistoryDown.SetColumn(pctHistoryDownLine, 2)
      If Not pnlHistory.Controls.Contains(grpHistoryUp) Then pnlHistory.Controls.Add(grpHistoryUp, 1, 0)
      pnlHistoryUp.ColumnCount = 3
      pnlHistoryUp.RowCount = 5
      pnlHistoryUp.SetRow(lblHistoryUpMax, 4)
      pnlHistoryUp.SetColumn(lblHistoryUpMax, 0)
      pnlHistoryUp.SetRow(pctHistoryUpMax, 4)
      pnlHistoryUp.SetColumn(pctHistoryUpMax, 2)
      pnlHistoryUp.SetRow(lblHistoryUpC, 3)
      pnlHistoryUp.SetColumn(lblHistoryUpC, 0)
      pnlHistoryUp.SetRow(pctHistoryUpC, 3)
      pnlHistoryUp.SetColumn(pctHistoryUpC, 2)
      pnlHistoryUp.SetRow(chkHistoryUpB, 2)
      pnlHistoryUp.SetColumn(chkHistoryUpB, 0)
      pnlHistoryUp.SetRow(pctHistoryUpB, 2)
      pnlHistoryUp.SetColumn(pctHistoryUpB, 2)
      pnlHistoryUp.SetRow(lblHistoryUpA, 1)
      pnlHistoryUp.SetColumn(lblHistoryUpA, 0)
      pnlHistoryUp.SetRow(pctHistoryUpA, 1)
      pnlHistoryUp.SetColumn(pctHistoryUpA, 2)
      pnlHistoryUp.SetRow(lblHistoryUpLine, 0)
      pnlHistoryUp.SetColumn(lblHistoryUpLine, 0)
      pnlHistoryUp.SetRow(pctHistoryUpLine, 0)
      pnlHistoryUp.SetColumn(pctHistoryUpLine, 2)
      pnlHistoryStyle.ColumnCount = 4
      pnlHistoryStyle.ColumnStyles(0) = New ColumnStyle(SizeType.Absolute, 75)
      pnlHistoryStyle.ColumnStyles(1) = New ColumnStyle(SizeType.Percent, 50)
      pnlHistoryStyle.ColumnStyles(2) = New ColumnStyle(SizeType.Absolute, 75)
      pnlHistoryStyle.ColumnStyles(3) = New ColumnStyle(SizeType.Percent, 50)
      pnlHistoryStyle.RowCount = 2
      pnlHistoryStyle.RowStyles(0) = New RowStyle(SizeType.AutoSize)
      pnlHistoryStyle.RowStyles(1) = New RowStyle(SizeType.AutoSize)
      pnlHistoryStyle.SetColumn(lblHistoryText, 0)
      pnlHistoryStyle.SetRow(lblHistoryText, 0)
      pnlHistoryStyle.SetColumn(pctHistoryText, 1)
      pnlHistoryStyle.SetRow(pctHistoryText, 0)
      pnlHistoryStyle.SetColumn(lblHistoryBG, 2)
      pnlHistoryStyle.SetRow(lblHistoryBG, 0)
      pnlHistoryStyle.SetColumn(pctHistoryBG, 3)
      pnlHistoryStyle.SetRow(pctHistoryBG, 0)

      pnlHistoryStyle.SetColumn(lblHistoryGridL, 0)
      pnlHistoryStyle.SetRow(lblHistoryGridL, 1)
      pnlHistoryStyle.SetColumn(pctHistoryGridL, 1)
      pnlHistoryStyle.SetRow(pctHistoryGridL, 1)
      pnlHistoryStyle.SetColumn(lblHistoryGridD, 2)
      pnlHistoryStyle.SetRow(lblHistoryGridD, 1)
      pnlHistoryStyle.SetColumn(pctHistoryGridD, 3)
      pnlHistoryStyle.SetRow(pctHistoryGridD, 1)

      pnlCustomColors.SetRow(grpTray, 1)
      pnlCustomColors.SetColumn(grpTray, 0)
      If pnlTray.Controls.Contains(pctTray) Then
        pnlTray.Controls.Remove(pctTray)
        pnlTray.RowCount = 1
        pnlTray.RowStyles(0) = New RowStyle(SizeType.AutoSize)
        pnlTray.SetRow(grpTrayDown, 0)
        pnlCustomColors.Controls.Add(pctTray, 1, 1)
        pctTray.Size = preSize
      End If
      pnlTray.ColumnStyles(0) = New ColumnStyle(SizeType.Percent, 50)
      pnlTray.ColumnStyles(1) = New ColumnStyle(SizeType.Percent, 50)
      If Not pnlTray.Controls.Contains(grpTrayUp) Then pnlTray.Controls.Add(grpTrayUp, 1, 0)

      If pnlMain.Controls.Contains(pctMain) Then
        pnlMain.Controls.Remove(pctMain)
        pnlMain.RowCount = 2
        pnlMain.RowStyles(0) = New RowStyle(SizeType.AutoSize)
        pnlMain.RowStyles(1) = New RowStyle(SizeType.AutoSize)
        pnlMain.SetRow(grpMainDown, 0)
        pnlMain.SetRow(pnlMainStyle, 1)
        pnlCustomColors.Controls.Add(pctMain, 1, 0)
        pctMain.Size = preSize
      End If
      pnlMain.ColumnStyles(0) = New ColumnStyle(SizeType.Percent, 50)
      pnlMain.ColumnStyles(1) = New ColumnStyle(SizeType.Percent, 50)
      If Not pnlMain.Controls.Contains(grpMainUp) Then pnlMain.Controls.Add(grpMainUp, 1, 0)
      pnlMainStyle.ColumnCount = 4
      pnlMainStyle.ColumnStyles(0) = New ColumnStyle(SizeType.Absolute, 75)
      pnlMainStyle.ColumnStyles(1) = New ColumnStyle(SizeType.Percent, 50)
      pnlMainStyle.ColumnStyles(2) = New ColumnStyle(SizeType.Absolute, 75)
      pnlMainStyle.ColumnStyles(3) = New ColumnStyle(SizeType.Percent, 50)
      pnlMainStyle.RowCount = 1
      pnlMainStyle.RowStyles(0) = New RowStyle(SizeType.AutoSize)
      pnlMainStyle.SetColumn(lblMainText, 0)
      pnlMainStyle.SetRow(lblMainText, 0)
      pnlMainStyle.SetColumn(pctMainText, 1)
      pnlMainStyle.SetRow(pctMainText, 0)
      pnlMainStyle.SetColumn(lblMainBG, 2)
      pnlMainStyle.SetRow(lblMainBG, 0)
      pnlMainStyle.SetColumn(pctMainBG, 3)
      pnlMainStyle.SetRow(pctMainBG, 0)
    Else
      Dim preSize As New Size(100, 50)
      pnlCustomColors.ColumnCount = 3
      pnlCustomColors.ColumnStyles(0) = New ColumnStyle(SizeType.AutoSize)
      pnlCustomColors.ColumnStyles(1) = New ColumnStyle(SizeType.AutoSize)
      If pnlCustomColors.ColumnStyles.Count = 2 Then
        pnlCustomColors.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
      Else
        pnlCustomColors.ColumnStyles(2) = New ColumnStyle(SizeType.AutoSize)
      End If

      If pnlMain.Controls.Contains(grpMainUp) Then pnlMain.Controls.Remove(grpMainUp)
      pnlMain.ColumnStyles(0) = New ColumnStyle(SizeType.Percent, 100)
      pnlMain.ColumnStyles(1) = New ColumnStyle(SizeType.AutoSize)
      If pnlCustomColors.Controls.Contains(pctMain) Then
        pnlCustomColors.Controls.Remove(pctMain)
        pnlMain.RowCount = 5
        pnlMain.RowStyles(0) = New RowStyle(SizeType.AutoSize)
        pnlMain.RowStyles(1) = New RowStyle(SizeType.AutoSize)
        If pnlMain.RowStyles.Count = 2 Then
          pnlMain.RowStyles.Add(New RowStyle(SizeType.Percent, 50))
          pnlMain.RowStyles.Add(New RowStyle(SizeType.AutoSize))
          pnlMain.RowStyles.Add(New RowStyle(SizeType.Percent, 50))
        ElseIf pnlMain.RowStyles.Count = 3 Then
          pnlMain.RowStyles(2) = New RowStyle(SizeType.Percent, 50)
          pnlMain.RowStyles.Add(New RowStyle(SizeType.AutoSize))
          pnlMain.RowStyles.Add(New RowStyle(SizeType.Percent, 50))
        ElseIf pnlMain.RowStyles.Count = 4 Then
          pnlMain.RowStyles(2) = New RowStyle(SizeType.Percent, 50)
          pnlMain.RowStyles(3) = New RowStyle(SizeType.AutoSize)
          pnlMain.RowStyles.Add(New RowStyle(SizeType.Percent, 50))
        Else
          pnlMain.RowStyles(2) = New RowStyle(SizeType.Percent, 50)
          pnlMain.RowStyles(3) = New RowStyle(SizeType.AutoSize)
          pnlMain.RowStyles(4) = New RowStyle(SizeType.Percent, 50)
        End If
        pnlMain.SetRow(pnlMainStyle, 3)
        pnlMain.SetRow(grpMainDown, 1)
        pnlMain.Controls.Add(pctMain, 0, 0)
        pctMain.Size = preSize
      End If
      pnlMainStyle.ColumnCount = 2
      pnlMainStyle.ColumnStyles(0) = New ColumnStyle(SizeType.AutoSize)
      pnlMainStyle.ColumnStyles(1) = New ColumnStyle(SizeType.Percent, 100)
      pnlMainStyle.RowCount = 2
      pnlMainStyle.RowStyles(0) = New RowStyle(SizeType.AutoSize)
      If pnlMainStyle.RowStyles.Count = 1 Then
        pnlMainStyle.RowStyles.Add(New RowStyle(SizeType.AutoSize))
      Else
        pnlMainStyle.RowStyles(1) = New RowStyle(SizeType.AutoSize)
      End If
      pnlMainStyle.SetColumn(lblMainText, 0)
      pnlMainStyle.SetRow(lblMainText, 0)
      pnlMainStyle.SetColumn(pctMainText, 1)
      pnlMainStyle.SetRow(pctMainText, 0)
      pnlMainStyle.SetColumn(lblMainBG, 0)
      pnlMainStyle.SetRow(lblMainBG, 1)
      pnlMainStyle.SetColumn(pctMainBG, 1)
      pnlMainStyle.SetRow(pctMainBG, 1)

      If pnlTray.Controls.Contains(grpTrayUp) Then pnlTray.Controls.Remove(grpTrayUp)
      pnlTray.ColumnStyles(0) = New ColumnStyle(SizeType.Percent, 100)
      pnlTray.ColumnStyles(1) = New ColumnStyle(SizeType.AutoSize)
      If pnlCustomColors.Controls.Contains(pctTray) Then
        pnlCustomColors.Controls.Remove(pctTray)
        pnlTray.RowCount = 3
        pnlTray.RowStyles(0) = New RowStyle(SizeType.AutoSize)
        If pnlTray.RowStyles.Count = 1 Then
          pnlTray.RowStyles.Add(New RowStyle(SizeType.AutoSize))
          pnlTray.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
        ElseIf pnlTray.RowStyles.Count = 2 Then
          pnlTray.RowStyles(1) = New RowStyle(SizeType.AutoSize)
          pnlTray.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
        Else
          pnlTray.RowStyles(1) = New RowStyle(SizeType.AutoSize)
          pnlTray.RowStyles(2) = New RowStyle(SizeType.Percent, 100)
        End If
        pnlTray.SetRow(grpTrayDown, 1)
        pnlTray.Controls.Add(pctTray, 0, 0)
        pctTray.Size = preSize
      End If
      If pnlHistory.Controls.Contains(grpHistoryUp) Then pnlHistory.Controls.Remove(grpHistoryUp)
      pnlHistoryDown.ColumnCount = 6
      pnlHistoryDown.RowCount = 3
      pnlHistoryDown.SetRow(lblHistoryDownMax, 2)
      pnlHistoryDown.SetColumn(lblHistoryDownMax, 0)
      pnlHistoryDown.SetRow(pctHistoryDownMax, 2)
      pnlHistoryDown.SetColumn(pctHistoryDownMax, 2)
      pnlHistoryDown.SetRow(lblHistoryDownC, 2)
      pnlHistoryDown.SetColumn(lblHistoryDownC, 4)
      pnlHistoryDown.SetRow(pctHistoryDownC, 2)
      pnlHistoryDown.SetColumn(pctHistoryDownC, 6)
      pnlHistoryDown.SetRow(chkHistoryDownB, 1)
      pnlHistoryDown.SetColumn(chkHistoryDownB, 4)
      pnlHistoryDown.SetRow(pctHistoryDownB, 1)
      pnlHistoryDown.SetColumn(pctHistoryDownB, 6)
      pnlHistoryDown.SetRow(lblHistoryDownA, 0)
      pnlHistoryDown.SetColumn(lblHistoryDownA, 4)
      pnlHistoryDown.SetRow(pctHistoryDownA, 0)
      pnlHistoryDown.SetColumn(pctHistoryDownA, 6)
      pnlHistoryDown.SetRow(lblHistoryDownLine, 0)
      pnlHistoryDown.SetColumn(lblHistoryDownLine, 0)
      pnlHistoryDown.SetRow(pctHistoryDownLine, 0)
      pnlHistoryDown.SetColumn(pctHistoryDownLine, 2)
      pnlHistory.ColumnStyles(0) = New ColumnStyle(SizeType.Percent, 100)
      pnlHistory.ColumnStyles(1) = New ColumnStyle(SizeType.AutoSize)
      If pnlCustomColors.Controls.Contains(pctHistory) Then
        pnlCustomColors.Controls.Remove(pctHistory)
        pnlHistory.RowCount = 3
        pnlHistory.RowStyles(0) = New RowStyle(SizeType.AutoSize)
        pnlHistory.RowStyles(1) = New RowStyle(SizeType.AutoSize)
        If pnlHistory.RowStyles.Count = 2 Then
          pnlHistory.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        Else
          pnlHistory.RowStyles(2) = New RowStyle(SizeType.AutoSize)
        End If
        pnlHistory.SetRow(pnlHistoryStyle, 2)
        pnlHistory.SetRow(grpHistoryDown, 1)
        pnlHistory.Controls.Add(pctHistory, 0, 0)
        pctHistory.Size = preSize
      End If
      pnlHistoryStyle.ColumnCount = 4
      pnlHistoryStyle.ColumnStyles(0) = New ColumnStyle(SizeType.AutoSize)
      pnlHistoryStyle.ColumnStyles(1) = New ColumnStyle(SizeType.Percent, 100)
      pnlHistoryStyle.ColumnStyles(2) = New ColumnStyle(SizeType.AutoSize)
      pnlHistoryStyle.ColumnStyles(3) = New ColumnStyle(SizeType.Percent, 100)
      pnlHistoryStyle.RowCount = 2
      pnlHistoryStyle.RowStyles(0) = New RowStyle(SizeType.AutoSize)
      If pnlHistoryStyle.RowStyles.Count = 1 Then
        pnlHistoryStyle.RowStyles.Add(New RowStyle(SizeType.AutoSize))
      Else
        pnlHistoryStyle.RowStyles(1) = New RowStyle(SizeType.AutoSize)
      End If

      pnlHistoryStyle.SetColumn(lblHistoryText, 0)
      pnlHistoryStyle.SetRow(lblHistoryText, 0)
      pnlHistoryStyle.SetColumn(pctHistoryText, 1)
      pnlHistoryStyle.SetRow(pctHistoryText, 0)
      pnlHistoryStyle.SetColumn(lblHistoryBG, 0)
      pnlHistoryStyle.SetRow(lblHistoryBG, 1)
      pnlHistoryStyle.SetColumn(pctHistoryBG, 1)
      pnlHistoryStyle.SetRow(pctHistoryBG, 1)

      pnlHistoryStyle.SetColumn(lblHistoryGridL, 2)
      pnlHistoryStyle.SetRow(lblHistoryGridL, 0)
      pnlHistoryStyle.SetColumn(pctHistoryGridL, 3)
      pnlHistoryStyle.SetRow(pctHistoryGridL, 0)
      pnlHistoryStyle.SetColumn(lblHistoryGridD, 2)
      pnlHistoryStyle.SetRow(lblHistoryGridD, 1)
      pnlHistoryStyle.SetColumn(pctHistoryGridD, 3)
      pnlHistoryStyle.SetRow(pctHistoryGridD, 1)

      pnlCustomColors.SetColumn(grpTray, 1)
      pnlCustomColors.SetRow(grpTray, 0)
      pnlCustomColors.SetColumn(grpHistory, 2)
      pnlCustomColors.SetRow(grpHistory, 0)

      pnlCustomColors.SetColumn(pnlButtons, 0)
      pnlCustomColors.SetRow(pnlButtons, 1)
      pnlCustomColors.SetColumnSpan(pnlButtons, 3)

      pnlCustomColors.RowCount = 2
    End If
  End Sub
#End Region
End Class
