﻿Public Class frmHistory
  Friend mySettings As AppSettings
  Private lastRect As Rectangle
  Private bSizeBegin As Boolean
  Private bSizeChange As Boolean
  Private graphSpaceD, graphSpaceU As Rectangle
  Private graphMinX, graphMaxX As Date
  Private Delegate Sub ParameterizedInvoker(parameter As Object)
  Private Delegate Sub ParameterizedInvoker2(param1 As Object, param2 As Object)
  Private WithEvents usageTmp As DataBase
  Private useStyle As SatHostTypes
  Private fDB As frmDBProgress
#Region "Form Events"
  Private Sub frmHistory_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    mySettings = New AppSettings
    useStyle = frmMain.myPanel
    If useStyle = SatHostTypes.Other Then useStyle = mySettings.AccountType
    If mySettings.Colors.HistoryDownA.A = 0 Then SetDefaultColors()
    ResetDates()
    dtpFrom.Value = dtpFrom.MinDate
    dtpTo.Value = dtpTo.MaxDate
    If mySettings.Gr = "id" Then
      optGrid.Checked = True
    Else
      optGraph.Checked = True
    End If
    Select Case useStyle
      Case SatHostTypes.DishNet, SatHostTypes.Exede, SatHostTypes.RuralPortal
        cmd30Days.Text = "This Period"
        cmd60Days.Text = "Last Period"
      Case Else
        cmd30Days.Text = "30 Days"
        cmd60Days.Text = "60 Days"
    End Select
    Select Case mySettings.Ago
      Case 1 : cmdToday.PerformClick()
      Case 30 : cmd30Days.PerformClick()
      Case 60 : cmd60Days.PerformClick()
      Case UInteger.MaxValue : cmdAllTime.PerformClick()
      Case Else
        If DateAdd(DateInterval.Day, -1 * mySettings.Ago, Now) > dtpFrom.MinDate And DateAdd(DateInterval.Day, -1 * mySettings.Ago, Now) < dtpFrom.MaxDate Then
          dtpFrom.Value = DateAdd(DateInterval.Day, -1 * mySettings.Ago, Now)
        Else
          dtpFrom.Value = dtpFrom.MinDate
        End If
        cmdQuery.PerformClick()
    End Select
  End Sub
  Private Sub frmHistory_ResizeBegin(sender As Object, e As System.EventArgs) Handles Me.ResizeBegin
    bSizeBegin = True
    lastRect = Me.Bounds
  End Sub
  Private Sub frmHistory_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
    bSizeChange = True
    pctDld.Image = ResizingNote(pctDld.Size)
    pctUld.Image = ResizingNote(pctUld.Size)
  End Sub
  Public Sub frmHistory_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
    If bSizeChange And Not bSizeBegin Then
      DoResize(True)
      bSizeChange = False
    End If
  End Sub
  Private Sub frmHistory_ResizeEnd(sender As Object, e As System.EventArgs) Handles Me.ResizeEnd
    If Me.Bounds.Width <> lastRect.Width Or Me.Bounds.Height <> lastRect.Height Or bSizeChange Then
      If bSizeBegin Then
        DoResize(True)
      End If
      lastRect = Me.Bounds
    End If
    bSizeBegin = False
    bSizeChange = False
  End Sub
  Private Sub frmHistory_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If fDB IsNot Nothing Then
      If fDB.Visible Then fDB.Close()
      fDB.Dispose()
      fDB = Nothing
    End If
    Dim SetAgo As UInt32 = mySettings.Ago
    mySettings = New AppSettings
    mySettings.Ago = SetAgo
    mySettings.Gr = IIf(optGraph.Checked, "aph", "id")
    mySettings.Save()
    frmMain.ReLoadSettings()
  End Sub
#End Region
#Region "Graph"
  Private Sub DidResize(downRet As Bitmap, upRet As Bitmap)
    If Me.InvokeRequired Then
      Me.Invoke(New ParameterizedInvoker2(AddressOf DidResize), downRet, upRet)
    Else
      If downRet Is Nothing Then
        pctDld.Image = Nothing
      ElseIf pctDld.Size.Equals(downRet.Size) Then
        pctDld.Image = downRet
      End If
      If upRet Is Nothing Then
        pctUld.Image = Nothing
      ElseIf pctUld.Size.Equals(upRet.Size) Then
        pctUld.Image = upRet
      End If
      graphSpaceD = GetGraphRect(True, graphMinX, graphMaxX)
      graphSpaceU = GetGraphRect(False, Nothing, Nothing)
      If fDB.Visible Then fDB.Close()
      If fDB IsNot Nothing Then
        fDB.Dispose()
        fDB = Nothing
      End If
    End If
  End Sub
  Private Sub DoGraph(state As Object)
    Dim graphStyle As Byte = state(0)
    Dim graphData = state(1)
    Dim downSize As Size = state(2)
    Select Case graphStyle
      Case 0
        Dim upSize As Size = state(3)
        Dim bDown As Bitmap = DrawLineGraph(graphData, True, downSize, mySettings.Colors.HistoryDownA, mySettings.Colors.HistoryDownB, mySettings.Colors.HistoryDownC, mySettings.Colors.HistoryText, mySettings.Colors.HistoryBackground, mySettings.Colors.HistoryDownMax)
        Dim bUp As Bitmap = DrawLineGraph(pnlGraph.Tag, False, pctUld.Size, mySettings.Colors.HistoryUpA, mySettings.Colors.HistoryUpB, mySettings.Colors.HistoryUpC, mySettings.Colors.HistoryText, mySettings.Colors.HistoryBackground, mySettings.Colors.HistoryUpMax)
        If Me.IsDisposed OrElse Me.Disposing Then Exit Sub
        Me.Invoke(New ParameterizedInvoker2(AddressOf DidResize), bDown, bUp)
      Case 1
        Dim bGraph As Bitmap = DrawRGraph(graphData, downSize, mySettings.Colors.HistoryDownA, mySettings.Colors.HistoryDownB, mySettings.Colors.HistoryDownC, mySettings.Colors.HistoryText, mySettings.Colors.HistoryBackground, mySettings.Colors.HistoryDownMax)
        If Me.IsDisposed OrElse Me.Disposing Then Exit Sub
        Me.Invoke(New ParameterizedInvoker2(AddressOf DidResize), bGraph, Nothing)
      Case 2
        Dim bGraph As Bitmap = DrawEGraph(graphData, mySettings.HistoryInversion, downSize, mySettings.Colors.HistoryDownA, mySettings.Colors.HistoryDownB, mySettings.Colors.HistoryDownC, mySettings.Colors.HistoryUpA, mySettings.Colors.HistoryUpB, mySettings.Colors.HistoryUpC, mySettings.Colors.HistoryText, mySettings.Colors.HistoryBackground, mySettings.Colors.HistoryDownMax)
        If Me.IsDisposed OrElse Me.Disposing Then Exit Sub
        Me.Invoke(New ParameterizedInvoker2(AddressOf DidResize), bGraph, Nothing)
    End Select
  End Sub
  Friend Sub DoResize(Optional ByVal Forced As Boolean = False)
    If Me.IsHandleCreated And Not Me.IsDisposed Then
      If pnlGraph.Visible Then
        If Not (Me.Bounds.Equals(lastRect)) Or Forced Then
          If fDB Is Nothing Then fDB = New frmDBProgress
          If Not fDB.Visible Then
            fDB.Show(Me)
            fDB.SetAction("Drawing Graph...", "Collecting data, estimating, and resizing...")
          End If
          Dim lItems() As DataBase.DataRow = pnlGraph.Tag
          If (lItems Is Nothing OrElse lItems.Count = 0) Or (usageDB Is Nothing OrElse usageDB.Count = 0) Then
            pnlGraph.RowStyles(0).Height = 100
            pnlGraph.RowStyles(1).Height = 0
            lastRect = Me.Bounds
            pctDld.Image = NoDataNote(pctDld.Size)
            If fDB.Visible Then fDB.Close()
            If fDB IsNot Nothing Then
              fDB.Dispose()
              fDB = Nothing
            End If
          Else
            Dim GraphInvoker As New ParameterizedInvoker(AddressOf DoGraph)
            Dim bDisplayed As Boolean = False
            Select Case useStyle
              Case SatHostTypes.DishNet
                pnlGraph.RowStyles(0).Height = 50
                pnlGraph.RowStyles(1).Height = 50
                lastRect = Me.Bounds
                pctDld.Image = ResizingNote(pctDld.Size)
                pctUld.Image = ResizingNote(pctUld.Size)
                GraphInvoker.BeginInvoke({0, pnlGraph.Tag, pctDld.Size, pctUld.Size}, Nothing, Nothing)
                bDisplayed = True
              Case SatHostTypes.RuralPortal
                pnlGraph.RowStyles(0).Height = 100
                pnlGraph.RowStyles(1).Height = 0
                lastRect = Me.Bounds
                pctDld.Image = ResizingNote(pctDld.Size)
                GraphInvoker.BeginInvoke({1, pnlGraph.Tag, pctDld.Size}, Nothing, Nothing)
                bDisplayed = True
              Case SatHostTypes.Exede
                pnlGraph.RowStyles(0).Height = 100
                pnlGraph.RowStyles(1).Height = 0
                lastRect = Me.Bounds
                pctDld.Image = ResizingNote(pctDld.Size)
                GraphInvoker.BeginInvoke({2, pnlGraph.Tag, pctDld.Size}, Nothing, Nothing)
                bDisplayed = True
              Case SatHostTypes.WildBlue
                pnlGraph.RowStyles(0).Height = 50
                pnlGraph.RowStyles(1).Height = 50
                lastRect = Me.Bounds
                pctDld.Image = ResizingNote(pctDld.Size)
                pctUld.Image = ResizingNote(pctUld.Size)
                GraphInvoker.BeginInvoke({0, pnlGraph.Tag, pctDld.Size, pctUld.Size}, Nothing, Nothing)
                bDisplayed = True
            End Select
            If Not bDisplayed Then
              If usageDB.GetLast.DOWNLIM = usageDB.GetLast.UPLIM Then
                If usageDB.GetLast.DOWNLOAD = usageDB.GetLast.UPLOAD Then
                  'RuralPortal
                  pnlGraph.RowStyles(0).Height = 100
                  pnlGraph.RowStyles(1).Height = 0
                  lastRect = Me.Bounds
                  pctDld.Image = ResizingNote(pctDld.Size)
                  GraphInvoker.BeginInvoke({1, pnlGraph.Tag, pctDld.Size}, Nothing, Nothing)
                Else
                  'Exede
                  pnlGraph.RowStyles(0).Height = 100
                  pnlGraph.RowStyles(1).Height = 0
                  lastRect = Me.Bounds
                  pctDld.Image = ResizingNote(pctDld.Size)
                  GraphInvoker.BeginInvoke({2, pnlGraph.Tag, pctDld.Size}, Nothing, Nothing)
                End If
              Else
                pnlGraph.RowStyles(0).Height = 50
                pnlGraph.RowStyles(1).Height = 50
                lastRect = Me.Bounds
                pctDld.Image = ResizingNote(pctDld.Size)
                pctUld.Image = ResizingNote(pctUld.Size)
                GraphInvoker.BeginInvoke({0, pnlGraph.Tag, pctDld.Size, pctUld.Size}, Nothing, Nothing)
              End If
            End If
          End If
        End If
      End If
    End If
  End Sub
  Private Sub pctDld_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctDld.MouseMove
    If graphSpaceD = Nothing Then Exit Sub
    If graphSpaceD.Contains(e.Location) Then
      Dim dNow As Date = CalculateNow(graphSpaceD, graphMinX, graphMaxX, e.Location)
      Static lastShow As String
      Dim gShow = GetGraphData(dNow, True)
      Dim showTime As String = gShow.DATETIME.ToString("g")
      Dim Show As String = showTime & " : " & gShow.DOWNLOAD & " MB / " & gShow.UPLIM & " MB"
      If useStyle = SatHostTypes.Exede Then
        If mySettings.HistoryInversion Then
          If gShow.DOWNLIM > 0 And Not gShow.UPLIM = gShow.DOWNLIM Then
            Show = showTime & " : " & (gShow.DOWNLOAD + gShow.UPLOAD + gShow.DOWNLIM) & " MB / " & gShow.UPLIM & " MB" & vbNewLine &
                   Space(showTime.Length * 2 + 2) & gShow.DOWNLOAD & " MB ↓, " & gShow.UPLOAD & " MB ↑, " & gShow.DOWNLIM & " MB Over"
          Else
            Show = showTime & " : " & (gShow.DOWNLOAD + gShow.UPLOAD) & " MB / " & gShow.UPLIM & " MB" & vbNewLine &
                   Space(showTime.Length * 2 + 2) & gShow.DOWNLOAD & " MB ↓, " & gShow.UPLOAD & " MB ↑"
          End If
        Else
          If gShow.UPLIM > 0 And Not gShow.UPLIM = gShow.DOWNLIM Then
            Show = showTime & " : " & (gShow.DOWNLOAD + gShow.UPLOAD + gShow.UPLIM) & " MB / " & gShow.DOWNLIM & " MB" & vbNewLine &
                   Space(showTime.Length * 2 + 2) & gShow.DOWNLOAD & " MB ↓, " & gShow.UPLOAD & " MB ↑, " & gShow.UPLIM & " MB Over"
          Else
            Show = showTime & " : " & (gShow.DOWNLOAD + gShow.UPLOAD) & " MB / " & gShow.DOWNLIM & " MB" & vbNewLine &
                   Space(showTime.Length * 2 + 2) & gShow.DOWNLOAD & " MB ↓, " & gShow.UPLOAD & " MB ↑"
          End If
        End If
      End If
      If lastShow = Show Then Exit Sub
      lastShow = Show
      ttHistory.Show(Show, pctDld, e.X + 16, e.Y + 32)
    Else
      ttHistory.Hide(pctDld)
    End If
  End Sub
  Private Sub pctUld_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctUld.MouseMove
    If graphSpaceU = Nothing Then Exit Sub
    If graphSpaceU.Contains(e.Location) Then
      Dim dNow As Date = CalculateNow(graphSpaceU, graphMinX, graphMaxX, e.Location)
      Static lastShow As String
      Dim gShow = GetGraphData(dNow, False)
      Dim Show As String = gShow.DATETIME.ToString("g") & " : " & gShow.UPLOAD & " MB / " & gShow.UPLIM & " MB"
      If lastShow = Show Then Exit Sub
      lastShow = Show
      ttHistory.Show(Show, pctUld, e.X + 16, e.Y + 32)
    Else
      ttHistory.Hide(pctUld)
    End If
  End Sub
  Private Function CalculateNow(GraphSpace As Rectangle, StartX As Date, EndX As Date, MyLoc As Point) As Date
    Dim DateSpan As Integer = DateDiff(DateInterval.Minute, StartX, EndX)
    Return DateAdd(DateInterval.Minute, ((MyLoc.X - GraphSpace.Left) / GraphSpace.Width) * DateSpan, StartX)
  End Function
#End Region
#Region "Buttons"
  Private Sub cmdQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdQuery.Click
    grpAge.Enabled = False
    Dim dFrom As Date = Date.Parse(dtpFrom.Value.Date & " 00:00:00 AM")
    Dim dTo As Date = Date.Parse(dtpTo.Value.Date & " 11:59:59 PM")
    Dim lItems() As DataBase.DataRow
    Dim bClose As Boolean = False
    If fDB Is Nothing Then fDB = New frmDBProgress
    If Not fDB.Visible Then
      fDB.Show(Me)
      bClose = True
    End If
    fDB.SetAction("Querying DataBase...", "Reading Rows...")
    If optGraph.Checked Then
      dgvBandwidth.Visible = False
      pnlGraph.Visible = True
      If usageDB IsNot Nothing AndAlso usageDB.Count > 0 Then
        lItems = Array.FindAll(usageDB.ToArray(mySettings.HistoryInversion), Function(satRow As DataBase.DataRow) satRow.DATETIME.CompareTo(dFrom) >= 0 And satRow.DATETIME.CompareTo(dTo) <= 0)
        pnlGraph.Tag = lItems
        DoResize(True)
      Else
        pnlGraph.Tag = Nothing
        DoResize(True)
      End If
      bClose = False
    Else
      pnlGraph.Visible = False
      dgvBandwidth.Visible = True
      If usageDB IsNot Nothing AndAlso usageDB.Count > 0 Then
        lItems = Array.FindAll(usageDB.ToArray(mySettings.HistoryInversion), Function(satRow As DataBase.DataRow) satRow.DATETIME.CompareTo(dFrom) >= 0 And satRow.DATETIME.CompareTo(dTo) <= 0)
        dgvBandwidth.Rows.Clear()
        Dim SameLim As Boolean = True
        Select Case useStyle
          Case SatHostTypes.DishNet
            dgvBandwidth.Columns(1).HeaderText = "Anytime"
            dgvBandwidth.Columns(2).HeaderText = "Off-Peak"
            Dim myDLim As Long = 0
            Dim myULim As Long = 0
            For Each lItem As DataBase.DataRow In lItems
              If myDLim = 0 Then
                myDLim = lItem.DOWNLIM
              Else
                If Not myDLim = lItem.DOWNLIM Then
                  SameLim = False
                  Exit For
                End If
              End If
              If myULim = 0 Then
                myULim = lItem.UPLIM
              Else
                If Not myULim = lItem.UPLIM Then
                  SameLim = False
                  Exit For
                End If
              End If
            Next
            If SameLim Then
              For Each lItem As DataBase.DataRow In lItems
                dgvBandwidth.Rows.Add(lItem.DATETIME, lItem.DOWNLOAD, lItem.UPLOAD)
              Next lItem
            Else
              For Each lItem As DataBase.DataRow In lItems
                dgvBandwidth.Rows.Add(lItem.DATETIME, lItem.DOWNLOAD & " / " & lItem.DOWNLIM, lItem.UPLOAD & " / " & lItem.UPLIM)
              Next lItem
            End If
          Case SatHostTypes.RuralPortal
            dgvBandwidth.Columns(1).HeaderText = "Used"
            dgvBandwidth.Columns(2).HeaderText = "Total"
            For Each lItem As DataBase.DataRow In lItems
              dgvBandwidth.Rows.Add(lItem.DATETIME, lItem.DOWNLOAD, lItem.DOWNLIM)
            Next lItem
          Case SatHostTypes.WildBlue
            dgvBandwidth.Columns(1).HeaderText = "Download"
            dgvBandwidth.Columns(2).HeaderText = "Upload"
            Dim myDLim As Long = 0
            Dim myULim As Long = 0
            For Each lItem As DataBase.DataRow In lItems
              If myDLim = 0 Then
                myDLim = lItem.DOWNLIM
              Else
                If Not myDLim = lItem.DOWNLIM Then
                  SameLim = False
                  Exit For
                End If
              End If
              If myULim = 0 Then
                myULim = lItem.UPLIM
              Else
                If Not myULim = lItem.UPLIM Then
                  SameLim = False
                  Exit For
                End If
              End If
            Next
            fDB.SetAction("Querying DataBase...", "Populating Table...")
            If SameLim Then
              For Each lItem As DataBase.DataRow In lItems
                dgvBandwidth.Rows.Add(lItem.DATETIME, lItem.DOWNLOAD, lItem.UPLOAD)
              Next lItem
            Else
              For Each lItem As DataBase.DataRow In lItems
                dgvBandwidth.Rows.Add(lItem.DATETIME, lItem.DOWNLOAD.ToString & " / " & lItem.DOWNLIM.ToString, lItem.UPLOAD.ToString & " / " & lItem.UPLIM.ToString)
              Next lItem
            End If
          Case SatHostTypes.Exede
            dgvBandwidth.Columns(1).HeaderText = "Download, Upload, & Over"
            dgvBandwidth.Columns(2).HeaderText = "Limit"
            For Each lItem As DataBase.DataRow In lItems
              If lItem.DOWNLIM = lItem.UPLIM Then
                If mySettings.HistoryInversion Then
                  dgvBandwidth.Rows.Add(lItem.DATETIME, lItem.DOWNLOAD & " + " & lItem.UPLOAD, lItem.DOWNLIM)
                Else
                  dgvBandwidth.Rows.Add(lItem.DATETIME, lItem.DOWNLOAD & " + " & lItem.UPLOAD, lItem.UPLIM)
                End If
              Else
                If mySettings.HistoryInversion Then
                  If lItem.DOWNLIM = 0 Then
                    dgvBandwidth.Rows.Add(lItem.DATETIME, lItem.DOWNLOAD & " + " & lItem.UPLOAD, lItem.UPLIM)
                  Else
                    dgvBandwidth.Rows.Add(lItem.DATETIME, lItem.DOWNLOAD & " + " & lItem.UPLOAD & " + " & lItem.DOWNLIM, lItem.UPLIM)
                  End If
                Else
                  If lItem.UPLIM = 0 Then
                    dgvBandwidth.Rows.Add(lItem.DATETIME, lItem.DOWNLOAD & " + " & lItem.UPLOAD, lItem.DOWNLIM)
                  Else
                    dgvBandwidth.Rows.Add(lItem.DATETIME, lItem.DOWNLOAD & " + " & lItem.UPLOAD & " + " & lItem.UPLIM, lItem.DOWNLIM)
                  End If
                End If
              End If

            Next lItem
          Case Else
            dgvBandwidth.Columns(1).HeaderText = "Download"
            dgvBandwidth.Columns(2).HeaderText = "Upload"
            Dim myDLim As Long = 0
            Dim myULim As Long = 0
            For Each lItem As DataBase.DataRow In lItems
              If myDLim = 0 Then
                myDLim = lItem.DOWNLIM
              Else
                If Not myDLim = lItem.DOWNLIM Then
                  SameLim = False
                  Exit For
                End If
              End If
              If myULim = 0 Then
                myULim = lItem.UPLIM
              Else
                If Not myULim = lItem.UPLIM Then
                  SameLim = False
                  Exit For
                End If
              End If
            Next
            If SameLim Then
              For Each lItem As DataBase.DataRow In lItems
                dgvBandwidth.Rows.Add(lItem.DATETIME, lItem.DOWNLOAD, lItem.UPLOAD)
              Next lItem
            Else
              For Each lItem As DataBase.DataRow In lItems
                dgvBandwidth.Rows.Add(lItem.DATETIME, lItem.DOWNLOAD & " / " & lItem.DOWNLIM, lItem.UPLOAD & " / " & lItem.UPLIM)
              Next lItem
            End If

        End Select
      Else
        dgvBandwidth.Rows.Clear()
      End If
    End If
    If bClose Then
      If fDB.Visible Then fDB.Close()
      If fDB IsNot Nothing Then
        fDB.Dispose()
        fDB = Nothing
      End If
    End If
    mySettings.Ago = Math.Abs(DateDiff(DateInterval.Day, dtpFrom.Value.Date, dtpTo.Value.Date))
    If mySettings.Ago = 0 Then mySettings.Ago = 1
    grpAge.Enabled = True
  End Sub
  Private Sub cmdToday_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdToday.Click
    Dim RightNow As Date = New Date(Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, 0)
    Dim FromNow As Date
    If RightNow > dtpFrom.MaxDate Then
      FromNow = dtpFrom.MaxDate
    ElseIf RightNow < dtpFrom.MinDate Then
      FromNow = dtpFrom.MinDate
    Else
      FromNow = RightNow
    End If
    Dim ToNow As Date
    If RightNow > dtpTo.MaxDate Then
      ToNow = dtpTo.MaxDate
    ElseIf RightNow < dtpTo.MinDate Then
      ToNow = dtpTo.MinDate
    Else
      ToNow = RightNow
    End If
    If FromNow.Year < 2000 Then FromNow = dtpFrom.MinDate
    If ToNow.Year < 2000 Then ToNow = dtpTo.MinDate
    dtpFrom.Value = FromNow
    dtpTo.Value = ToNow
    cmdQuery.PerformClick()
  End Sub
  Private Sub cmd30Days_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd30Days.Click
    Dim RightNow As Date = New Date(Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, 0)
    Dim From30DaysAgo As Date
    If useStyle = SatHostTypes.WildBlue Then
      If DateAdd(DateInterval.Day, -30, RightNow) > dtpFrom.MaxDate Then
        From30DaysAgo = dtpFrom.MaxDate
      ElseIf DateAdd(DateInterval.Day, -30, RightNow) < dtpFrom.MinDate Then
        From30DaysAgo = dtpFrom.MinDate
      Else
        From30DaysAgo = DateAdd(DateInterval.Day, -30, RightNow)
      End If
    Else
      If usageDB IsNot Nothing AndAlso usageDB.Count > 1 Then
        For I As Integer = usageDB.Count - 1 To 1 Step -1
          If (usageDB(I).DOWNLOAD = 0 And usageDB(I).UPLOAD = 0) And (usageDB(I - 1).DOWNLOAD > 0 Or usageDB(I - 1).UPLOAD > 0) Then
            If usageDB(I).DATETIME > dtpFrom.MaxDate Then
              From30DaysAgo = dtpFrom.MaxDate
            ElseIf usageDB(I).DATETIME < dtpFrom.MinDate Then
              From30DaysAgo = dtpFrom.MinDate
            Else
              From30DaysAgo = usageDB(I).DATETIME
            End If
            Exit For
          End If
        Next
      Else
        If usageDB(0).DATETIME > dtpFrom.MaxDate Then
          From30DaysAgo = dtpFrom.MaxDate
        ElseIf usageDB(0).DATETIME < dtpFrom.MinDate Then
          From30DaysAgo = dtpFrom.MinDate
        Else
          From30DaysAgo = usageDB(0).DATETIME
        End If
      End If
    End If
    Dim ToNow As Date
    If RightNow > dtpTo.MaxDate Then
      ToNow = dtpTo.MaxDate
    ElseIf RightNow < dtpTo.MinDate Then
      ToNow = dtpTo.MinDate
    Else
      ToNow = RightNow
    End If
    If From30DaysAgo.Year < 2000 Then From30DaysAgo = dtpFrom.MinDate
    If ToNow.Year < 2000 Then ToNow = dtpTo.MinDate
    dtpFrom.Value = From30DaysAgo
    dtpTo.Value = ToNow
    cmdQuery.PerformClick()
  End Sub
  Private Sub cmd60Days_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd60Days.Click
    Dim RightNow As Date = New Date(Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, 0)
    Dim From60DaysAgo As Date
    If useStyle = SatHostTypes.WildBlue Then
      If DateAdd(DateInterval.Day, -60, RightNow) > dtpFrom.MaxDate Then
        From60DaysAgo = dtpFrom.MaxDate
      ElseIf DateAdd(DateInterval.Day, -60, RightNow) < dtpFrom.MinDate Then
        From60DaysAgo = dtpFrom.MinDate
      Else
        From60DaysAgo = DateAdd(DateInterval.Day, -60, RightNow)
      End If
    Else
      If usageDB IsNot Nothing AndAlso usageDB.Count > 1 Then
        Dim Finds As Integer = 0
        For I As Integer = usageDB.Count - 1 To 1 Step -1
          If (usageDB(I).DOWNLOAD = 0 And usageDB(I).UPLOAD = 0) And (usageDB(I - 1).DOWNLOAD > 0 Or usageDB(I - 1).UPLOAD > 0) Then
            Finds += 1
            If Finds = 2 Then
              If usageDB(I).DATETIME > dtpFrom.MaxDate Then
                From60DaysAgo = dtpFrom.MaxDate
              ElseIf usageDB(I).DATETIME < dtpFrom.MinDate Then
                From60DaysAgo = dtpFrom.MinDate
              Else
                From60DaysAgo = usageDB(I).DATETIME
              End If
              Exit For
            End If
          End If
        Next
        If Finds < 2 Then From60DaysAgo = dtpFrom.MinDate
      Else
        If usageDB(0).DATETIME > dtpFrom.MaxDate Then
          From60DaysAgo = dtpFrom.MaxDate
        ElseIf usageDB(0).DATETIME < dtpFrom.MinDate Then
          From60DaysAgo = dtpFrom.MinDate
        Else
          From60DaysAgo = usageDB(0).DATETIME
        End If
      End If
    End If
    Dim ToNow As Date
    If RightNow > dtpTo.MaxDate Then
      ToNow = dtpTo.MaxDate
    ElseIf RightNow < dtpTo.MinDate Then
      ToNow = dtpTo.MinDate
    Else
      ToNow = RightNow
    End If
    If From60DaysAgo.Year < 2000 Then From60DaysAgo = dtpFrom.MinDate
    If ToNow.Year < 2000 Then ToNow = dtpTo.MinDate
    dtpFrom.Value = From60DaysAgo
    dtpTo.Value = ToNow
    cmdQuery.PerformClick()
  End Sub
  Private Sub cmdAllTime_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAllTime.Click
    dtpFrom.Value = dtpFrom.MinDate
    dtpTo.Value = dtpTo.MaxDate
    cmdQuery.PerformClick()
  End Sub
  Private Sub cmdImport_Click(sender As System.Object, e As System.EventArgs) Handles cmdImport.Click
    If (usageDB Is Nothing OrElse usageDB.Count = 0) Then MsgBox("The Database has not been loaded yet, please wait.", MsgBoxStyle.Exclamation Or MsgBoxStyle.SystemModal) : Exit Sub
    Dim cdlOpen As New OpenFileDialog With {.AddExtension = True, .CheckFileExists = True, .DefaultExt = "xml", .FileName = "Backup-" & mySettings.Account & ".xml", .Filter = "XML File|*.xml|CSV File|*.csv|Satellite Restriction Tracker Database|*.wb", .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments, .ShowReadOnly = False, .Title = "Import History Database"}
    If cdlOpen.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
      If fDB Is Nothing Then fDB = New frmDBProgress
      fDB.Show(Me)
      fDB.SetAction("Importing DataBase...", "Opening File...")
      usageTmp = New DataBase(cdlOpen.FileName, True)
      usageTmp.StartNew()
      If usageTmp.Count > 0 Then
        If usageDB IsNot Nothing Then
          fDB.SetAction("Importing DataBase...", "Merging File and DataBase...")
          usageDB.Merge(usageTmp, True)
        Else
          fDB.SetAction("Importing DataBase...", "Loading File into DataBase...")
          usageDB = usageTmp
        End If
        fDB.SetAction("Importing DataBase...", "Sorting DataBase...")
        LOG_Sort()
        fDB.SetAction("Importing DataBase...", "Saving DataBase...")
        LOG_Save(True)
        If fDB.Visible Then fDB.Close()
        If fDB IsNot Nothing Then
          fDB.Dispose()
          fDB = Nothing
        End If
        MsgBox(IO.Path.GetFileName(cdlOpen.FileName) & " has been merged into your history database.", MsgBoxStyle.Information Or MsgBoxStyle.SystemModal, My.Application.Info.Title)
        Application.DoEvents()
        ResetDates()
      Else
        If fDB.Visible Then fDB.Close()
        If fDB IsNot Nothing Then
          fDB.Dispose()
          fDB = Nothing
        End If
        MsgBox("Could not import " & IO.Path.GetFileName(cdlOpen.FileName), MsgBoxStyle.Exclamation Or MsgBoxStyle.SystemModal)
      End If
      usageTmp = Nothing
    End If
  End Sub
  Private Sub cmdExport_Click(sender As System.Object, e As System.EventArgs) Handles cmdExport.Click
    If (usageDB Is Nothing OrElse usageDB.Count = 0) Then MsgBox("The Database has not been loaded yet, please wait.", MsgBoxStyle.Exclamation Or MsgBoxStyle.SystemModal) : Exit Sub
    Dim cdlSave As New SaveFileDialog With {.AddExtension = True, .CheckPathExists = True, .DefaultExt = "xml", .FileName = "Backup-" & mySettings.Account & ".xml", .Filter = "XML File|*.xml|CSV File|*.csv|Satellite Restriction Tracker Database|*.wb", .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments, .Title = "Export History Database"}
    If cdlSave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
      If fDB Is Nothing Then fDB = New frmDBProgress
      fDB.Show(Me)
      fDB.SetAction("Exporting DataBase...", "Saving File...")
      If chkExportRange.Checked Then
        Dim dFrom As Date = Date.Parse(dtpFrom.Value.Date & " 00:00:00 AM")
        Dim dTo As Date = Date.Parse(dtpTo.Value.Date & " 11:59:59 PM")
        usageTmp = New DataBase
        For Each satRow In usageDB
          If satRow.DATETIME.CompareTo(dFrom) >= 0 And satRow.DATETIME.CompareTo(dTo) <= 0 Then
            usageTmp.Add(satRow)
          End If
        Next
        usageTmp.Save(cdlSave.FileName, True)
        usageTmp = Nothing
      Else
        usageDB.Save(cdlSave.FileName, True)
      End If
      If fDB.Visible Then fDB.Close()
      If fDB IsNot Nothing Then
        fDB.Dispose()
        fDB = Nothing
      End If
      MsgBox("Your history has been exported to " & IO.Path.GetFileName(cdlSave.FileName), MsgBoxStyle.Information Or MsgBoxStyle.SystemModal, My.Application.Info.Title)
    End If
  End Sub
  Private Sub usageTmp_ProgressState(sender As Object, e As RestrictionLibrary.DataBase.ProgressStateEventArgs) Handles usageTmp.ProgressState
    If fDB IsNot Nothing AndAlso fDB.Visible Then fDB.SetProgress(e.Value, e.Total)
  End Sub
  Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
    Me.Close()
  End Sub
#End Region
#Region "Date Pickers"
  Private Sub dtpTo_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpTo.ValueChanged
    If dtpFrom.Value > dtpTo.Value Then
      If dtpTo.Value > dtpFrom.MaxDate Then
        dtpFrom.Value = dtpFrom.MaxDate
      ElseIf dtpTo.Value < dtpFrom.MinDate Then
        dtpFrom.Value = dtpFrom.MinDate
      Else
        dtpFrom.Value = dtpTo.Value
      End If
    End If
  End Sub
  Private Sub dtpFrom_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpFrom.ValueChanged
    If dtpFrom.Value > dtpTo.Value Then
      If dtpFrom.Value > dtpTo.MaxDate Then
        dtpTo.Value = dtpTo.MaxDate
      ElseIf dtpFrom.Value < dtpTo.MinDate Then
        dtpTo.Value = dtpTo.MinDate
      Else
        dtpTo.Value = dtpFrom.Value
      End If
    End If
  End Sub
  Private Sub ResetDates()
    Dim dMin, dMax As Date
    LOG_Get(0, dMin, 0, 0, 0, 0)
    Dim fDate As Date = DateSerial(2000, 1, 1)
    If DateDiff(DateInterval.Year, fDate, dMin) < 0 Then dMin = fDate
    If DateDiff(DateInterval.Minute, dMin, Now) < 0 Then
      pctErr.Image = New ErrorProvider().Icon.ToBitmap
      ttHistory.SetTooltip(pctErr, "The Log history is more recent than your system clock." & vbCr & "Please update your computer's time.")
    Else
      dMax = Now
      pctErr.Image = Nothing
      ttHistory.SetTooltip(pctErr, Nothing)
      If DateDiff(DateInterval.Minute, dMin, dMax) > 0 Then
        dtpFrom.MaxDate = dMax
        dtpTo.MaxDate = dMax
        dtpFrom.MinDate = dMin
        dtpTo.MinDate = dMin
      Else
        pctErr.Image = New ErrorProvider().Icon.ToBitmap
        ttHistory.SetTooltip(pctErr, "The Log history is more recent than your system clock." & vbCr & "Please update your computer's time.")
      End If
    End If
    If dtpFrom.MinDate.Year < 2000 Then dtpFrom.MinDate = DateSerial(2000, 1, 1)
    If dtpTo.MinDate.Year < 2000 Then dtpTo.MinDate = DateSerial(2000, 1, 1)
  End Sub
#End Region
#Region "Notices"
  Private Function NoDataNote(ImgSize As Size) As Image
    If ImgSize.Width = 0 Or ImgSize.Height = 0 Then Return Nothing
    Dim iPic As Image = New Bitmap(ImgSize.Width, ImgSize.Height)
    Dim g As Graphics = Graphics.FromImage(iPic)
    Const sNote As String = "No data has yet been accumulated."
    g.Clear(SystemColors.ButtonFace)
    g.DrawString(sNote, SystemFonts.DefaultFont, SystemBrushes.ControlText, (ImgSize.Width / 2) - (g.MeasureString(sNote, SystemFonts.DefaultFont).Width / 2), (ImgSize.Height / 2) - (g.MeasureString(sNote, SystemFonts.DefaultFont).Height / 2))
    g.Dispose()
    Return iPic
  End Function
  Private Function ResizingNote(ImgSize As Size) As Image
    If ImgSize.Width = 0 Or ImgSize.Height = 0 Then Return Nothing
    Dim iPic As Image = New Bitmap(ImgSize.Width, ImgSize.Height)
    Dim g As Graphics = Graphics.FromImage(iPic)
    'Const sNote As String = "Collecting data, estimating and resizing..." & vbNewLine & "This may take some time. Please wait."
    g.Clear(SystemColors.ButtonFace)
    g.FillRectangle(New Drawing2D.LinearGradientBrush(New Rectangle(0, 0, ImgSize.Width, ImgSize.Height), Color.White, Color.MidnightBlue, Drawing2D.LinearGradientMode.Vertical), New Rectangle(0, 0, ImgSize.Width, ImgSize.Height))
    'g.DrawString(sNote, SystemFonts.DefaultFont, SystemBrushes.ControlText, (ImgSize.Width / 2) - (g.MeasureString(sNote, SystemFonts.DefaultFont).Width / 2), (ImgSize.Height / 2) - (g.MeasureString(sNote, SystemFonts.DefaultFont).Height / 2))
    g.Dispose()
    Return iPic
  End Function
#End Region
  Private Sub SetDefaultColors()
    If Me.InvokeRequired Then
      Me.Invoke(New MethodInvoker(AddressOf SetDefaultColors))
    Else
      Select Case useStyle
        Case SatHostTypes.WildBlue
          mySettings.Colors.MainDownA = Color.DarkBlue
          mySettings.Colors.MainDownB = Color.Transparent
          mySettings.Colors.MainDownC = Color.Red
          mySettings.Colors.MainUpA = Color.DarkBlue
          mySettings.Colors.MainUpB = Color.Transparent
          mySettings.Colors.MainUpC = Color.Red
          mySettings.Colors.MainText = Color.White
          mySettings.Colors.MainBackground = Color.Black

          mySettings.Colors.TrayDownA = Color.Blue
          mySettings.Colors.TrayDownB = Color.Yellow
          mySettings.Colors.TrayDownC = Color.Red
          mySettings.Colors.TrayUpA = Color.Blue
          mySettings.Colors.TrayUpB = Color.Yellow
          mySettings.Colors.TrayUpC = Color.Red

          mySettings.Colors.HistoryDownA = Color.DarkBlue
          mySettings.Colors.HistoryDownB = Color.Transparent
          mySettings.Colors.HistoryDownC = Color.Red
          mySettings.Colors.HistoryDownMax = Color.Yellow
          mySettings.Colors.HistoryUpA = Color.DarkBlue
          mySettings.Colors.HistoryUpB = Color.Transparent
          mySettings.Colors.HistoryUpC = Color.Red
          mySettings.Colors.HistoryUpMax = Color.Yellow
          mySettings.Colors.HistoryText = Color.Black
          mySettings.Colors.HistoryBackground = Color.White

        Case SatHostTypes.Exede
          mySettings.Colors.MainDownA = Color.Orange
          mySettings.Colors.MainDownB = Color.Transparent
          mySettings.Colors.MainDownC = Color.Red
          mySettings.Colors.MainUpA = Color.Blue
          mySettings.Colors.MainUpB = Color.Transparent
          mySettings.Colors.MainUpC = Color.Violet
          mySettings.Colors.MainText = Color.White
          mySettings.Colors.MainBackground = Color.Black

          mySettings.Colors.TrayDownA = Color.Orange
          mySettings.Colors.TrayDownB = Color.Transparent
          mySettings.Colors.TrayDownC = Color.Red
          mySettings.Colors.TrayUpA = Color.Blue
          mySettings.Colors.TrayUpB = Color.Transparent
          mySettings.Colors.TrayUpC = Color.Violet

          mySettings.Colors.HistoryDownA = Color.Orange
          mySettings.Colors.HistoryDownB = Color.Transparent
          mySettings.Colors.HistoryDownC = Color.Red
          mySettings.Colors.HistoryDownMax = Color.Yellow
          mySettings.Colors.HistoryUpA = Color.Blue
          mySettings.Colors.HistoryUpB = Color.Transparent
          mySettings.Colors.HistoryUpC = Color.Violet
          mySettings.Colors.HistoryUpMax = Color.Yellow
          mySettings.Colors.HistoryText = Color.Black
          mySettings.Colors.HistoryBackground = Color.White

        Case SatHostTypes.RuralPortal
          mySettings.Colors.MainDownA = Color.Orange
          mySettings.Colors.MainDownB = Color.Transparent
          mySettings.Colors.MainDownC = Color.Red
          mySettings.Colors.MainUpA = Color.Transparent
          mySettings.Colors.MainUpB = Color.Transparent
          mySettings.Colors.MainUpC = Color.Transparent
          mySettings.Colors.MainText = Color.White
          mySettings.Colors.MainBackground = Color.Black

          mySettings.Colors.TrayDownA = Color.Orange
          mySettings.Colors.TrayDownB = Color.Transparent
          mySettings.Colors.TrayDownC = Color.Red
          mySettings.Colors.TrayUpA = Color.Transparent
          mySettings.Colors.TrayUpB = Color.Transparent
          mySettings.Colors.TrayUpC = Color.Transparent

          mySettings.Colors.HistoryDownA = Color.Orange
          mySettings.Colors.HistoryDownB = Color.Transparent
          mySettings.Colors.HistoryDownC = Color.Red
          mySettings.Colors.HistoryDownMax = Color.Yellow
          mySettings.Colors.HistoryUpA = Color.Transparent
          mySettings.Colors.HistoryUpB = Color.Transparent
          mySettings.Colors.HistoryUpC = Color.Transparent
          mySettings.Colors.HistoryUpMax = Color.Transparent
          mySettings.Colors.HistoryText = Color.Black
          mySettings.Colors.HistoryBackground = Color.White

        Case SatHostTypes.DishNet
          mySettings.Colors.MainDownA = Color.DarkBlue
          mySettings.Colors.MainDownB = Color.Transparent
          mySettings.Colors.MainDownC = Color.Red
          mySettings.Colors.MainUpA = Color.DarkBlue
          mySettings.Colors.MainUpB = Color.Transparent
          mySettings.Colors.MainUpC = Color.Red
          mySettings.Colors.MainText = Color.White
          mySettings.Colors.MainBackground = Color.Black

          mySettings.Colors.TrayDownA = Color.Blue
          mySettings.Colors.TrayDownB = Color.Yellow
          mySettings.Colors.TrayDownC = Color.Red
          mySettings.Colors.TrayUpA = Color.Blue
          mySettings.Colors.TrayUpB = Color.Yellow
          mySettings.Colors.TrayUpC = Color.Red

          mySettings.Colors.HistoryDownA = Color.DarkBlue
          mySettings.Colors.HistoryDownB = Color.Transparent
          mySettings.Colors.HistoryDownC = Color.Red
          mySettings.Colors.HistoryDownMax = Color.Yellow
          mySettings.Colors.HistoryUpA = Color.DarkBlue
          mySettings.Colors.HistoryUpB = Color.Transparent
          mySettings.Colors.HistoryUpC = Color.Red
          mySettings.Colors.HistoryUpMax = Color.Yellow
          mySettings.Colors.HistoryText = Color.Black
          mySettings.Colors.HistoryBackground = Color.White
        Case Else

      End Select
    End If
  End Sub
End Class