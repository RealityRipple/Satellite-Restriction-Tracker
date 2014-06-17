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
      Dim saveRet As MsgBoxResult = MsgBox("Some settings have been changed but not saved." & vbNewLine & vbNewLine & "Do you want to save the changes to your color scheme?", MsgBoxStyle.Question Or MsgBoxStyle.YesNoCancel, "Save Colors?")
      If saveRet = MsgBoxResult.Yes Then
        cmdSave.PerformClick()
      ElseIf saveRet = MsgBoxResult.No Then
        Me.DialogResult = Windows.Forms.DialogResult.No
      ElseIf saveRet = MsgBoxResult.Cancel Then
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
        grpMainDown.Text = "Download Colors"
        grpMainUp.Text = "Upload Colors"
        grpMainUp.Visible = True
        grpTrayDown.Text = "Download Colors"
        grpTrayUp.Text = "Upload Colors"
        grpTrayUp.Visible = True
        grpHistoryDown.Text = "Download Colors"
        grpHistoryUp.Text = "Upload Colors"
        grpHistoryUp.Visible = True
        lblHistoryUpMax.Visible = True
        pctHistoryUpMax.Visible = True
        DisplayAs = SatHostTypes.WildBlue_LEGACY
      Case SatHostTypes.WildBlue_EXEDE
        grpMainDown.Text = "Download Colors"
        grpMainUp.Text = "Upload Colors"
        grpMainUp.Visible = True
        grpTrayDown.Text = "Download Colors"
        grpTrayUp.Text = "Upload Colors"
        grpTrayUp.Visible = True
        grpHistoryDown.Text = "Download Colors"
        grpHistoryUp.Text = "Upload Colors"
        grpHistoryUp.Visible = True
        lblHistoryUpMax.Visible = False
        pctHistoryUpMax.Visible = False
        DisplayAs = SatHostTypes.WildBlue_EXEDE
      Case SatHostTypes.DishNet_EXEDE
        grpMainDown.Text = "Anytime Colors"
        grpMainUp.Text = "Off-Peak Colors"
        grpMainUp.Visible = True
        grpTrayDown.Text = "Anytime Colors"
        grpTrayUp.Text = "Off-Peak Colors"
        grpTrayUp.Visible = True
        grpHistoryDown.Text = "Anytime Colors"
        grpHistoryUp.Text = "Off-Peak Colors"
        grpHistoryUp.Visible = True
        lblHistoryUpMax.Visible = True
        pctHistoryUpMax.Visible = True
        DisplayAs = SatHostTypes.DishNet_EXEDE
      Case SatHostTypes.RuralPortal_EXEDE, SatHostTypes.WildBlue_EVOLUTION
        grpMainDown.Text = "Usage Colors"
        grpMainUp.Visible = False
        grpTrayDown.Text = "Usage Colors"
        grpTrayUp.Visible = False
        grpHistoryDown.Text = "Usage Colors"
        grpHistoryUp.Visible = False
        lblHistoryUpMax.Visible = False
        pctHistoryUpMax.Visible = False
        DisplayAs = SatHostTypes.RuralPortal_EXEDE
      Case Else
        grpMainDown.Text = "Download Colors"
        grpMainUp.Text = "Upload Colors"
        grpMainUp.Visible = True
        grpTrayDown.Text = "Download Colors"
        grpTrayUp.Text = "Upload Colors"
        grpTrayUp.Visible = True
        grpHistoryDown.Text = "Download Colors"
        grpHistoryUp.Text = "Upload Colors"
        grpHistoryUp.Visible = True
        lblHistoryUpMax.Visible = True
        pctHistoryUpMax.Visible = True
        DisplayAs = SatHostTypes.Other
    End Select
    If mySettings.Colors.MainDownA.A = 0 Then
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
      SetElColor(pctHistoryDownA, mySettings.Colors.HistoryDownA)
      SetElColor(pctHistoryDownB, mySettings.Colors.HistoryDownB)
      SetElColor(pctHistoryDownC, mySettings.Colors.HistoryDownC)
      SetElColor(pctHistoryDownMax, mySettings.Colors.HistoryDownMax)
      SetElColor(pctHistoryUpA, mySettings.Colors.HistoryUpA)
      SetElColor(pctHistoryUpB, mySettings.Colors.HistoryUpB)
      SetElColor(pctHistoryUpC, mySettings.Colors.HistoryUpC)
      SetElColor(pctHistoryUpMax, mySettings.Colors.HistoryUpMax)
      SetElColor(pctHistoryText, mySettings.Colors.HistoryText)
      SetElColor(pctHistoryBG, mySettings.Colors.HistoryBackground)
    End If
    RedrawImages()
    cmdSave.Enabled = False
    HasSaved = False
  End Sub
  Private Sub RedrawImages()
    If pctMain.Image IsNot Nothing Then
      pctMain.Image.Dispose()
      pctMain.Image = Nothing
    End If
    Dim iSize As Integer = pctMain.Width
    Dim iHalf As Integer = Math.Floor(iSize / 2)
    Select Case DisplayAs
      Case SatHostTypes.WildBlue_LEGACY
        Dim FakeMRect As New Rectangle(0, 0, iSize, iSize * 2)
        Dim FakeD As Image = DisplayProgress(FakeMRect.Size, lDown, lDownLim, mySettings.Accuracy, pctMainDownA.BackColor, pctMainDownB.BackColor, pctMainDownC.BackColor, pctMainText.BackColor, pctMainBG.BackColor)
        Dim FakeU As Image = DisplayProgress(FakeMRect.Size, lUp, lUpLim, mySettings.Accuracy, pctMainUpA.BackColor, pctMainUpB.BackColor, pctMainUpC.BackColor, pctMainText.BackColor, pctMainBG.BackColor)
        Dim fakeI As New Bitmap(pctHistory.Width, pctHistory.Height)
        Using g As Graphics = Graphics.FromImage(fakeI)
          g.Clear(Color.Black)
          g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
          g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
          g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
          Dim dRect As New Rectangle(0, 0, iHalf, iSize)
          Dim uRect As New Rectangle(iHalf, 0, iHalf, iSize)
          g.DrawImage(FakeD, dRect, FakeMRect, GraphicsUnit.Pixel)
          g.DrawImage(FakeU, uRect, FakeMRect, GraphicsUnit.Pixel)
        End Using
        pctMain.Image = fakeI
      Case SatHostTypes.WildBlue_EXEDE
        Dim FakeMRect As New Rectangle(0, 0, iSize * 2, iSize * 1.5)
        Dim FakeE As Image = DisplayEProgress(FakeMRect.Size, lDown, lUp, 0, lDownLim, mySettings.Accuracy, pctMainDownA.BackColor, pctMainDownB.BackColor, pctMainDownC.BackColor, pctMainUpA.BackColor, pctMainUpB.BackColor, pctMainUpC.BackColor, pctMainText.BackColor, pctMainBG.BackColor)
        Dim fakeI As New Bitmap(pctHistory.Width, pctHistory.Height)
        Using g As Graphics = Graphics.FromImage(fakeI)
          g.Clear(Color.Black)
          g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
          g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
          g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
          Dim MRect As New Rectangle(0, iHalf / 2, iSize, iHalf)
          g.DrawImage(FakeE, MRect, FakeMRect, GraphicsUnit.Pixel)
        End Using
        pctMain.Image = fakeI
      Case SatHostTypes.RuralPortal_EXEDE
        pctMain.Image = DisplayRProgress(pctMain.DisplayRectangle.Size, lDown, lDownLim, mySettings.Accuracy, pctMainDownA.BackColor, pctMainDownB.BackColor, pctMainDownC.BackColor, pctMainText.BackColor, pctMainBG.BackColor)
      Case SatHostTypes.DishNet_EXEDE
        Dim FakeMRect As New Rectangle(0, 0, iSize, iSize * 2)
        Dim FakeD As Image = DisplayProgress(FakeMRect.Size, lDown, lDownLim, mySettings.Accuracy, pctMainDownA.BackColor, pctMainDownB.BackColor, pctMainDownC.BackColor, pctMainText.BackColor, pctMainBG.BackColor)
        Dim FakeU As Image = DisplayProgress(FakeMRect.Size, lUp, lUpLim, mySettings.Accuracy, pctMainUpA.BackColor, pctMainUpB.BackColor, pctMainUpC.BackColor, pctMainText.BackColor, pctMainBG.BackColor)
        Dim fakeI As New Bitmap(pctHistory.Width, pctHistory.Height)
        Using g As Graphics = Graphics.FromImage(fakeI)
          g.Clear(Color.Black)
          g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
          g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
          g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
          Dim dRect As New Rectangle(0, 0, iHalf, iSize)
          Dim uRect As New Rectangle(iHalf, 0, iHalf, iSize)
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
        Case SatHostTypes.WildBlue_EXEDE
          If lDown + lUp >= lUpLim Then
            g.DrawIconUnstretched(My.Resources.t16_restricted, New Rectangle(0, 0, traySquare, traySquare))
          Else
            g.DrawIconUnstretched(My.Resources.t16_norm, New Rectangle(0, 0, traySquare, traySquare))
          End If
          CreateTrayIcon_Dual(g, lDown, lUp, lDownLim, pctTrayDownA.BackColor, pctTrayDownB.BackColor, pctTrayDownC.BackColor, pctTrayUpA.BackColor, pctTrayUpB.BackColor, pctTrayUpC.BackColor, traySquare, traySquare)
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
          Dim dRect As New Rectangle(0, iHalf * 0.1, iSize, iHalf * 0.85)
          Dim uRect As New Rectangle(0, iHalf + (iHalf * 0.05), iSize, iHalf * 0.85)
          g.DrawImage(FakeD, dRect, FakeHRect, GraphicsUnit.Pixel)
          g.DrawImage(FakeU, uRect, FakeHRect, GraphicsUnit.Pixel)
        End Using
        pctHistory.Image = fakeI
      Case SatHostTypes.WildBlue_EXEDE
        Dim FakeE As Image = DrawEGraph(FakeData.ToArray, False, FakeHRect.Size, pctHistoryDownA.BackColor, pctHistoryDownB.BackColor, pctHistoryDownC.BackColor, pctHistoryUpA.BackColor, pctHistoryUpB.BackColor, pctHistoryUpC.BackColor, pctHistoryText.BackColor, pctHistoryBG.BackColor, pctHistoryDownMax.BackColor)
        Dim fakeI As New Bitmap(pctHistory.Width, pctHistory.Height)
        Using g As Graphics = Graphics.FromImage(fakeI)
          g.Clear(Color.Black)
          g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
          g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
          g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
          Dim dRect As New Rectangle(0, iHalf / 2, iSize, iHalf)
          g.DrawImage(FakeE, dRect, FakeHRect, GraphicsUnit.Pixel)
        End Using
        pctHistory.Image = fakeI
      Case SatHostTypes.RuralPortal_EXEDE
        Dim FakeR As Image = DrawRGraph(FakeData.ToArray, FakeHRect.Size, pctHistoryDownA.BackColor, pctHistoryDownB.BackColor, pctHistoryDownC.BackColor, pctHistoryText.BackColor, pctHistoryBG.BackColor, pctHistoryDownMax.BackColor)
        Dim fakeI As New Bitmap(pctHistory.Width, pctHistory.Height)
        Using g As Graphics = Graphics.FromImage(fakeI)
          g.Clear(Color.Black)
          g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
          g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
          g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
          Dim dRect As New Rectangle(0, iHalf / 2, iSize, iHalf)
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
          Dim dRect As New Rectangle(0, iHalf * 0.1, iSize, iHalf * 0.85)
          Dim uRect As New Rectangle(0, iHalf + (iHalf * 0.05), iSize, iHalf * 0.85)
          g.DrawImage(FakeD, dRect, FakeHRect, GraphicsUnit.Pixel)
          g.DrawImage(FakeU, uRect, FakeHRect, GraphicsUnit.Pixel)
        End Using
        pctHistory.Image = fakeI
      Case Else
        pctHistory.Image = pctHistory.ErrorImage.Clone
    End Select
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
    mySettings.Colors.HistoryDownA = pctHistoryDownA.BackColor
    mySettings.Colors.HistoryDownB = pctHistoryDownB.BackColor
    mySettings.Colors.HistoryDownC = pctHistoryDownC.BackColor
    mySettings.Colors.HistoryDownMax = pctHistoryDownMax.BackColor
    mySettings.Colors.HistoryUpA = pctHistoryUpA.BackColor
    mySettings.Colors.HistoryUpB = pctHistoryUpB.BackColor
    mySettings.Colors.HistoryUpC = pctHistoryUpC.BackColor
    mySettings.Colors.HistoryUpMax = pctHistoryUpMax.BackColor
    mySettings.Colors.HistoryText = pctHistoryText.BackColor
    mySettings.Colors.HistoryBackground = pctHistoryBG.BackColor
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
                                                                                pctHistoryDownA.MouseClick, pctHistoryDownB.MouseClick, pctHistoryDownC.MouseClick, pctHistoryDownMax.MouseClick,
                                                                                pctHistoryUpA.MouseClick, pctHistoryUpB.MouseClick, pctHistoryUpC.MouseClick, pctHistoryUpMax.MouseClick,
                                                                                pctHistoryText.MouseClick, pctHistoryBG.MouseClick
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
    ElseIf e.Button = Windows.Forms.MouseButtons.Middle Then
      If DisplayAs = SatHostTypes.WildBlue_LEGACY Then
        DisplayAs = SatHostTypes.WildBlue_EXEDE
      ElseIf DisplayAs = SatHostTypes.WildBlue_EXEDE Then
        DisplayAs = SatHostTypes.RuralPortal_EXEDE
      ElseIf DisplayAs = SatHostTypes.RuralPortal_EXEDE Then
        DisplayAs = SatHostTypes.DishNet_EXEDE
      ElseIf DisplayAs = SatHostTypes.DishNet_EXEDE Then
        DisplayAs = SatHostTypes.WildBlue_LEGACY
      End If
      MakeFakeData()
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
    ElseIf e.Button = Windows.Forms.MouseButtons.Middle Then
      If DisplayAs = SatHostTypes.WildBlue_LEGACY Then
        DisplayAs = SatHostTypes.WildBlue_EXEDE
      ElseIf DisplayAs = SatHostTypes.WildBlue_EXEDE Then
        DisplayAs = SatHostTypes.RuralPortal_EXEDE
      ElseIf DisplayAs = SatHostTypes.RuralPortal_EXEDE Then
        DisplayAs = SatHostTypes.DishNet_EXEDE
      ElseIf DisplayAs = SatHostTypes.DishNet_EXEDE Then
        DisplayAs = SatHostTypes.WildBlue_LEGACY
      End If
      MakeFakeData()
    End If
    RedrawImages()
  End Sub
  Private Sub pctHistory_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctHistory.MouseDown
    If e.Button = Windows.Forms.MouseButtons.Middle Then
      If DisplayAs = SatHostTypes.WildBlue_LEGACY Then
        DisplayAs = SatHostTypes.WildBlue_EXEDE
      ElseIf DisplayAs = SatHostTypes.WildBlue_EXEDE Then
        DisplayAs = SatHostTypes.RuralPortal_EXEDE
      ElseIf DisplayAs = SatHostTypes.RuralPortal_EXEDE Then
        DisplayAs = SatHostTypes.DishNet_EXEDE
      ElseIf DisplayAs = SatHostTypes.DishNet_EXEDE Then
        DisplayAs = SatHostTypes.WildBlue_LEGACY
      End If
      MakeFakeData()
      RedrawImages()
    End If
  End Sub
  Private Sub chkB_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkMainDownB.CheckedChanged, chkMainUpB.CheckedChanged, chkTrayDownB.CheckedChanged, chkTrayUpB.CheckedChanged, chkHistoryDownB.CheckedChanged, chkHistoryUpB.CheckedChanged
    Dim chkThis As CheckBox = sender
    Dim pctThis As PictureBox = getControlFromName(Me, chkThis.Name.Replace("chk", "pct"))
    If pctThis Is Nothing Then Exit Sub
    If chkThis.Checked Then
      pctThis.Enabled = True
      If pctThis.Tag Is Nothing Then
        If pctThis.BackColor.A = 0 Then
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
    cmdSave.Enabled = True
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
          cmdSave.Enabled = True
        End If
        customColors = dlgColor.CustomColors
      End Using
    End If
  End Sub
  Private Sub mnuThisDefault_Click(sender As System.Object, e As System.EventArgs) Handles mnuThisDefault.Click
    Dim pctColor As PictureBox = mnuColorOpts.Tag
    If pctColor IsNot Nothing Then
      SetElColor(pctColor, DefaultColorForElement(pctColor.Name, useStyle))
      cmdSave.Enabled = True
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
      ColorList = {pctHistoryDownA, pctHistoryDownB, pctHistoryDownC, pctHistoryDownMax,
                   pctHistoryUpA, pctHistoryUpB, pctHistoryUpC, pctHistoryUpMax,
                   pctHistoryText, pctHistoryBG}
    End If
    For Each pColor As PictureBox In ColorList
      Dim bColor As Color = DefaultColorForElement(pColor.Name, useStyle)
      SetElColor(pColor, bColor)
    Next
    RedrawImages()
    cmdSave.Enabled = True
  End Sub
  Private Sub mnuAllDefault_Click(sender As System.Object, e As System.EventArgs) Handles mnuAllDefault.Click
    Dim pctColor As PictureBox = mnuColorOpts.Tag
    Dim ColorList As PictureBox()
    ColorList = {pctMainDownA, pctMainDownB, pctMainDownC,
                 pctMainUpA, pctMainUpB, pctMainUpC,
                 pctMainText, pctMainBG,
                 pctTrayDownA, pctTrayDownB, pctTrayDownC,
                 pctTrayUpA, pctTrayUpB, pctTrayUpC,
                 pctHistoryDownA, pctHistoryDownB, pctHistoryDownC, pctHistoryDownMax,
                 pctHistoryUpA, pctHistoryUpB, pctHistoryUpC, pctHistoryUpMax,
                 pctHistoryText, pctHistoryBG}
    For Each pColor As PictureBox In ColorList
      Dim bColor As Color = DefaultColorForElement(pColor.Name, useStyle)
      SetElColor(pColor, bColor)
    Next
    RedrawImages()
    cmdSave.Enabled = True
  End Sub
#End Region
#Region "Functions"
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
      chkThis.Checked = Not pctColor.BackColor.A = 0
    End If
    pctColor.Tag = Nothing
  End Sub
  Private Function DefaultColorForElement(Element As String, Provider As SatHostTypes) As Color
    Select Case Provider
      Case SatHostTypes.WildBlue_LEGACY, SatHostTypes.RuralPortal_LEGACY
        Select Case Element
          Case pctMainDownA.Name : Return Color.DarkBlue
          Case pctMainDownB.Name : Return Color.Transparent
          Case pctMainDownC.Name : Return Color.Red
          Case pctMainUpA.Name : Return Color.DarkBlue
          Case pctMainUpB.Name : Return Color.Transparent
          Case pctMainUpC.Name : Return Color.Red
          Case pctMainText.Name : Return Color.White
          Case pctMainBG.Name : Return Color.Black

          Case pctTrayDownA.Name : Return Color.Blue
          Case pctTrayDownB.Name : Return Color.Yellow
          Case pctTrayDownC.Name : Return Color.Red
          Case pctTrayUpA.Name : Return Color.Blue
          Case pctTrayUpB.Name : Return Color.Yellow
          Case pctTrayUpC.Name : Return Color.Red

          Case pctHistoryDownA.Name : Return Color.DarkBlue
          Case pctHistoryDownB.Name : Return Color.Transparent
          Case pctHistoryDownC.Name : Return Color.Red
          Case pctHistoryDownMax.Name : Return Color.Yellow
          Case pctHistoryUpA.Name : Return Color.DarkBlue
          Case pctHistoryUpB.Name : Return Color.Transparent
          Case pctHistoryUpC.Name : Return Color.Red
          Case pctHistoryUpMax.Name : Return Color.Yellow
          Case pctHistoryText.Name : Return Color.Black
          Case pctHistoryBG.Name : Return Color.White
        End Select
      Case SatHostTypes.WildBlue_EXEDE
        Select Case Element
          Case pctMainDownA.Name : Return Color.Orange
          Case pctMainDownB.Name : Return Color.Transparent
          Case pctMainDownC.Name : Return Color.Red
          Case pctMainUpA.Name : Return Color.Blue
          Case pctMainUpB.Name : Return Color.Transparent
          Case pctMainUpC.Name : Return Color.Violet
          Case pctMainText.Name : Return Color.White
          Case pctMainBG.Name : Return Color.Black

          Case pctTrayDownA.Name : Return Color.Orange
          Case pctTrayDownB.Name : Return Color.Transparent
          Case pctTrayDownC.Name : Return Color.Red
          Case pctTrayUpA.Name : Return Color.Blue
          Case pctTrayUpB.Name : Return Color.Transparent
          Case pctTrayUpC.Name : Return Color.Violet

          Case pctHistoryDownA.Name : Return Color.Orange
          Case pctHistoryDownB.Name : Return Color.Transparent
          Case pctHistoryDownC.Name : Return Color.Red
          Case pctHistoryDownMax.Name : Return Color.Yellow
          Case pctHistoryUpA.Name : Return Color.Blue
          Case pctHistoryUpB.Name : Return Color.Transparent
          Case pctHistoryUpC.Name : Return Color.Violet
          Case pctHistoryUpMax.Name : Return Color.Yellow
          Case pctHistoryText.Name : Return Color.Black
          Case pctHistoryBG.Name : Return Color.White
        End Select
      Case SatHostTypes.RuralPortal_EXEDE, SatHostTypes.WildBlue_EVOLUTION
        Select Case Element
          Case pctMainDownA.Name : Return Color.Orange
          Case pctMainDownB.Name : Return Color.Transparent
          Case pctMainDownC.Name : Return Color.Red
          Case pctMainUpA.Name : Return Color.Transparent
          Case pctMainUpB.Name : Return Color.Transparent
          Case pctMainUpC.Name : Return Color.Transparent
          Case pctMainText.Name : Return Color.White
          Case pctMainBG.Name : Return Color.Black

          Case pctTrayDownA.Name : Return Color.Orange
          Case pctTrayDownB.Name : Return Color.Transparent
          Case pctTrayDownC.Name : Return Color.Red
          Case pctTrayUpA.Name : Return Color.Transparent
          Case pctTrayUpB.Name : Return Color.Transparent
          Case pctTrayUpC.Name : Return Color.Transparent

          Case pctHistoryDownA.Name : Return Color.Orange
          Case pctHistoryDownB.Name : Return Color.Transparent
          Case pctHistoryDownC.Name : Return Color.Red
          Case pctHistoryDownMax.Name : Return Color.Yellow
          Case pctHistoryUpA.Name : Return Color.Transparent
          Case pctHistoryUpB.Name : Return Color.Transparent
          Case pctHistoryUpC.Name : Return Color.Transparent
          Case pctHistoryUpMax.Name : Return Color.Transparent
          Case pctHistoryText.Name : Return Color.Black
          Case pctHistoryBG.Name : Return Color.White
        End Select
      Case SatHostTypes.DishNet_EXEDE
        Select Case Element
          Case pctMainDownA.Name : Return Color.DarkBlue
          Case pctMainDownB.Name : Return Color.Transparent
          Case pctMainDownC.Name : Return Color.Red
          Case pctMainUpA.Name : Return Color.DarkBlue
          Case pctMainUpB.Name : Return Color.Transparent
          Case pctMainUpC.Name : Return Color.Red
          Case pctMainText.Name : Return Color.White
          Case pctMainBG.Name : Return Color.Black

          Case pctTrayDownA.Name : Return Color.Blue
          Case pctTrayDownB.Name : Return Color.Yellow
          Case pctTrayDownC.Name : Return Color.Red
          Case pctTrayUpA.Name : Return Color.Blue
          Case pctTrayUpB.Name : Return Color.Yellow
          Case pctTrayUpC.Name : Return Color.Red

          Case pctHistoryDownA.Name : Return Color.DarkBlue
          Case pctHistoryDownB.Name : Return Color.Transparent
          Case pctHistoryDownC.Name : Return Color.Red
          Case pctHistoryDownMax.Name : Return Color.Yellow
          Case pctHistoryUpA.Name : Return Color.DarkBlue
          Case pctHistoryUpB.Name : Return Color.Transparent
          Case pctHistoryUpC.Name : Return Color.Red
          Case pctHistoryUpMax.Name : Return Color.Yellow
          Case pctHistoryText.Name : Return Color.Black
          Case pctHistoryBG.Name : Return Color.White
        End Select
      Case Else
        Return Color.Transparent
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

      Case pctHistoryDownA.Name : Return "History Download 0% Color"
      Case pctHistoryDownB.Name : Return "History Download 50% Color"
      Case pctHistoryDownC.Name : Return "History Download 100% Color"
      Case pctHistoryDownMax.Name : Return "History Download Max Limit Color"
      Case pctHistoryUpA.Name : Return "History Upload 0% Color"
      Case pctHistoryUpB.Name : Return "History Upload 50% Color"
      Case pctHistoryUpC.Name : Return "History Upload 100% Color"
      Case pctHistoryUpMax.Name : Return "History Upload Max Limit Color"
      Case pctHistoryText.Name : Return "History Text Color"
      Case pctHistoryBG.Name : Return "History Background Color"

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
    ElseIf DisplayAs = SatHostTypes.WildBlue_EXEDE Then
      For I As Integer = 1 To 90
        Dim dRow As New DataBase.DataRow(startDate, startDown, 10000, startUp, 10000)
        Dim DownUsed As Integer = RandSel(50, 450)
        Dim UpUsed As Integer = RandSel(10, 120)
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
  Private Function RandSel(Low As Integer, High As Integer) As Integer
    Dim I As Integer = Int(Rnd() * (High - Low)) + Low
    If I = Low Then I = High
    Return I
  End Function
#End Region
End Class
