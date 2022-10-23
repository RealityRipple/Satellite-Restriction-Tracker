Public NotInheritable Class frmHistory
  Friend mySettings As AppSettings
  Private lastRect As Rectangle
  Private bSizeBegin As Boolean
  Private bSizeChange As Boolean
  Private graphSpace As Rectangle
  Private graphMinX, graphMaxX As Date
  Private Delegate Sub ParameterizedInvoker(parameter As Object)
  Private Delegate Sub ParameterizedInvoker2(param1 As Object, param2 As Object)
  Private WithEvents usageTmp As DataBase
  Private fDB As frmDBProgress
#Region "Form Events"
  Private Sub frmHistory_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    mySettings = New AppSettings
    If LocalAppDataDirectory = IO.Path.Combine(Application.StartupPath, "Config") Then mySettings.HistoryDir = IO.Path.Combine(Application.StartupPath, "Config")
    ScreenDefaultColors(mySettings.Colors)
    ResetDates()
    dtpFrom.Value = dtpFrom.MinDate
    dtpTo.Value = dtpTo.MaxDate
    If mySettings.Gr = "id" Then
      optGrid.Checked = True
    Else
      optGraph.Checked = True
    End If
    ChangeStyle()
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
    pctGraph.Image = ResizingNote(pctGraph.Size)
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
    Dim SetAgo As UInt32 = 30
    If mySettings IsNot Nothing Then SetAgo = mySettings.Ago
    mySettings = New AppSettings
    If LocalAppDataDirectory = IO.Path.Combine(Application.StartupPath, "Config") Then mySettings.HistoryDir = IO.Path.Combine(Application.StartupPath, "Config")
    mySettings.Ago = SetAgo
    mySettings.Gr = IIf(optGraph.Checked, "aph", "id")
    mySettings.Save()
    frmMain.ReLoadSettings()
  End Sub
  Private Sub ChangeStyle()
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New MethodInvoker(AddressOf ChangeStyle))
      Catch ex As Exception
      End Try
      Return
    End If
    cmd30Days.Text = "T&his Period"
    ttHistory.SetToolTip(cmd30Days, "Query the database to get the history of this usage period.")
    cmd60Days.Text = "&Last Period"
    ttHistory.SetToolTip(cmd60Days, "Query the database to get the history of this usage period and the previous usage period.")
    dgvUsage.Columns.Clear()
    colUSED.HeaderText = "Used"
    colLIMIT.HeaderText = "Total"
    dgvUsage.Columns.Add(colDATETIME)
    dgvUsage.Columns.Add(colUSED)
    dgvUsage.Columns.Add(colLIMIT)
  End Sub
#End Region
#Region "Graph"
  Private Sub DidResize(downRet As Bitmap)
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New ParameterizedInvoker(AddressOf DidResize), downRet)
      Catch ex As Exception
      End Try
      Return
    End If
    If downRet Is Nothing Then
      pctGraph.Image = Nothing
    ElseIf pctGraph.Size.Equals(downRet.Size) Then
      pctGraph.Image = downRet
    End If
    graphSpace = GetGraphRect(graphMinX, graphMaxX)
    If fDB IsNot Nothing Then
      If fDB.Visible Then fDB.Close()
      fDB.Dispose()
      fDB = Nothing
    End If
  End Sub
  Private Sub DoGraph(state As Object)
    Dim graphData As DataRow() = state(0)
    Dim downSize As Size = state(1)
    Dim bGraph As Bitmap = DrawRGraph(graphData, downSize, mySettings.Colors.HistoryDownLine, mySettings.Colors.HistoryDownA, mySettings.Colors.HistoryDownB, mySettings.Colors.HistoryDownC, mySettings.Colors.HistoryText, mySettings.Colors.HistoryBackground, mySettings.Colors.HistoryDownMax, mySettings.Colors.HistoryLightGrid, mySettings.Colors.HistoryDarkGrid)
    If Me.IsDisposed OrElse Me.Disposing Then Return
    Dim tResize As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf DidResize))
    tResize.Start(bGraph)
  End Sub
  Friend Sub DoResize(Optional ByVal Forced As Boolean = False)
    If Me.IsHandleCreated And Not Me.IsDisposed Then
      If pctGraph.Visible Then
        If Not (Me.Bounds.Equals(lastRect)) Or Forced Then
          If fDB Is Nothing Then fDB = New frmDBProgress
          If Not fDB.Visible Then
            fDB.Show(Me)
            fDB.SetAction("Drawing Graph...", "Collecting data, estimating, and resizing...")
          End If
          Dim lItems() As DataRow = pctGraph.Tag
          If usageDB Is Nothing OrElse usageDB.Count = 0 Then
            lastRect = Me.Bounds
            pctGraph.Image = BadDataNote(BadDataNotes.Null, pctGraph.Size)
            ClearGraphData()
            graphSpace = Nothing
            If fDB.Visible Then fDB.Close()
            If fDB IsNot Nothing Then
              fDB.Dispose()
              fDB = Nothing
            End If
          ElseIf lItems Is Nothing OrElse lItems.Count = 0 Then
            lastRect = Me.Bounds
            pctGraph.Image = BadDataNote(BadDataNotes.Null, pctGraph.Size)
            ClearGraphData()
            graphSpace = Nothing
            If fDB.Visible Then fDB.Close()
            If fDB IsNot Nothing Then
              fDB.Dispose()
              fDB = Nothing
            End If
          ElseIf lItems.Count = 1 Then
            lastRect = Me.Bounds
            pctGraph.Image = BadDataNote(BadDataNotes.One, pctGraph.Size)
            ClearGraphData()
            graphSpace = Nothing
            If fDB.Visible Then fDB.Close()
            If fDB IsNot Nothing Then
              fDB.Dispose()
              fDB = Nothing
            End If
          Else
            Dim GraphInvoker As New ParameterizedInvoker(AddressOf DoGraph)
            lastRect = Me.Bounds
            pctGraph.Image = ResizingNote(pctGraph.Size)
            GraphInvoker.BeginInvoke({pctGraph.Tag, pctGraph.Size}, Nothing, Nothing)
          End If
        End If
      End If
    End If
  End Sub
  Private Sub pctGraph_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctGraph.MouseMove
    If graphSpace = Nothing Then Return
    If graphSpace.Contains(e.Location) Then
      Dim dNow As Date = CalculateNow(graphSpace, graphMinX, graphMaxX, e.Location)
      Static lastShow As String
      Dim gShow As DataRow = GetGraphData(dNow)
      If gShow.IsEmpty Then Return
      Dim showTime As String = gShow.sDATETIME
      Dim Show As String = showTime & " : " & gShow.sUSED & " MB / " & gShow.sLIMIT & " MB"
      If lastShow = Show Then Return
      lastShow = Show
      ttHistory.Show(Show, pctGraph, e.X + 16, e.Y + 32)
    Else
      ttHistory.Hide(pctGraph)
    End If
  End Sub
  Private Shared Function CalculateNow(GraphSpace As Rectangle, StartX As Date, EndX As Date, MyLoc As Point) As Date
    Dim DateSpan As Integer = DateDiff(DateInterval.Minute, StartX, EndX)
    Return DateAdd(DateInterval.Minute, ((MyLoc.X - GraphSpace.Left) / GraphSpace.Width) * DateSpan, StartX)
  End Function
#End Region
#Region "Buttons"
  Private Sub cmdQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdQuery.Click
    ToggleInterface(False)
    Dim dFrom As New Date(dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day, 0, 0, 0)
    Dim dTo As New Date(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day, 23, 59, 59)
    Dim bClose As Boolean = False
    If fDB Is Nothing Then fDB = New frmDBProgress
    If Not fDB.Visible Then
      fDB.Show(Me)
      bClose = True
    End If
    fDB.SetAction("Querying DataBase...", "Reading Rows...")
    If optGraph.Checked Then
      dgvUsage.Visible = False
      pctGraph.Visible = True
      If usageDB IsNot Nothing AndAlso usageDB.Count > 0 Then
        Dim lItems() As DataRow = LOG_GetRange(dFrom, dTo)
        pctGraph.Tag = lItems
        DoResize(True)
      Else
        pctGraph.Tag = Nothing
        DoResize(True)
      End If
      bClose = False
    Else
      pctGraph.Visible = False
      dgvUsage.Visible = True
      If usageDB IsNot Nothing AndAlso usageDB.Count > 0 Then
        Dim lItems() As DataRow = LOG_GetRange(dFrom, dTo)
        dgvUsage.Rows.Clear()
        ChangeStyle()
        If fDB IsNot Nothing Then fDB.SetAction("Querying DataBase...", "Populating Table...")
        For Each lItem As DataRow In lItems
          dgvUsage.Rows.Add(lItem.DATETIME, lItem.sUSED, lItem.sLIMIT)
        Next lItem
      Else
        dgvUsage.Rows.Clear()
      End If
    End If
    If bClose Then
      If fDB IsNot Nothing Then
        If fDB.Visible Then fDB.Close()
        fDB.Dispose()
        fDB = Nothing
      End If
    End If
    mySettings.Ago = Math.Abs(DateDiff(DateInterval.Day, dtpFrom.Value.Date, dtpTo.Value.Date))
    If mySettings.Ago = 0 Then mySettings.Ago = 1
    ToggleInterface(True)
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
    Dim bClose As Boolean = False
    If usageDB IsNot Nothing Then
      Dim dbValues() As DataRow = usageDB.ToArray()
      If dbValues.Length > 1 Then
        If fDB Is Nothing Then fDB = New frmDBProgress
        If Not fDB.Visible Then
          fDB.Show(Me)
          bClose = True
        End If
        fDB.SetAction("Querying DataBase...", "Scanning for Resets...")
        For I As Integer = dbValues.Length - 1 To 1 Step -1
          Try
            fDB.SetProgress(dbValues.Length - I, dbValues.Length)
          Catch
          End Try
          Dim thisDB As DataRow = dbValues(I)
          Dim lastDB As DataRow = thisDB
          If I > 0 Then lastDB = dbValues(I - 1)
          Dim nextDB As DataRow = thisDB
          If I < dbValues.Length - 1 Then nextDB = dbValues(I + 1)
          If ((thisDB.USED < lastDB.USED) Or (thisDB.USED = 0 And lastDB.USED = 0)) And
            Not (lastDB.USED = 0) And
            Not (nextDB.USED = lastDB.USED) Then
            If DateDiff(DateInterval.Day, thisDB.DATETIME, Today) > 0 Then
              If thisDB.DATETIME > dtpFrom.MaxDate Then
                From30DaysAgo = dtpFrom.MaxDate
              ElseIf thisDB.DATETIME < dtpFrom.MinDate Then
                From30DaysAgo = dtpFrom.MinDate
              Else
                From30DaysAgo = thisDB.DATETIME
              End If
              Exit For
            End If
          End If
        Next
      ElseIf dbValues.Length > 0 Then
        If dbValues(0).DATETIME > dtpFrom.MaxDate Then
          From30DaysAgo = dtpFrom.MaxDate
        ElseIf dbValues(0).DATETIME < dtpFrom.MinDate Then
          From30DaysAgo = dtpFrom.MinDate
        Else
          From30DaysAgo = dbValues(0).DATETIME
        End If
      Else
        From30DaysAgo = dtpFrom.MinDate
      End If
    Else
      From30DaysAgo = dtpFrom.MinDate
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
    If bClose Then
      If fDB IsNot Nothing Then
        If fDB.Visible Then fDB.Close()
        fDB.Dispose()
        fDB = Nothing
      End If
    End If
  End Sub
  Private Sub cmd60Days_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd60Days.Click
    Dim RightNow As Date = New Date(Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, 0)
    Dim From60DaysAgo As Date
    Dim bClose As Boolean = False
    If usageDB IsNot Nothing Then
      Dim dbValues() As DataRow = usageDB.ToArray()
      If dbValues.Length > 1 Then
        If fDB Is Nothing Then fDB = New frmDBProgress
        If Not fDB.Visible Then
          fDB.Show(Me)
          bClose = True
        End If
        fDB.SetAction("Querying DataBase...", "Scanning for Resets...")
        Dim Finds As Integer = 0
        For I As Integer = dbValues.Length - 1 To 1 Step -1
          Try
            fDB.SetProgress(dbValues.Length - I, dbValues.Length)
          Catch
          End Try
          Dim thisDB As DataRow = dbValues(I)
          Dim lastDB As DataRow = thisDB
          If I > 0 Then lastDB = dbValues(I - 1)
          Dim nextDB As DataRow = thisDB
          If I < dbValues.Length - 1 Then nextDB = dbValues(I + 1)
          If ((thisDB.USED < lastDB.USED) Or (thisDB.USED = 0 And lastDB.USED = 0)) And
            Not (lastDB.USED = 0) And
            Not (nextDB.USED = lastDB.USED) Then
            If DateDiff(DateInterval.Day, usageDB(usageDB.Keys(I)).DATETIME, Today) > 0 Then
              Finds += 1
              If Finds = 2 Then
                If thisDB.DATETIME > dtpFrom.MaxDate Then
                  From60DaysAgo = dtpFrom.MaxDate
                ElseIf thisDB.DATETIME < dtpFrom.MinDate Then
                  From60DaysAgo = dtpFrom.MinDate
                Else
                  From60DaysAgo = thisDB.DATETIME
                End If
                Exit For
              End If
            End If
          End If
        Next
        If Finds < 2 Then From60DaysAgo = dtpFrom.MinDate
      ElseIf dbValues.Length > 0 Then
        If usageDB.First.Value.DATETIME > dtpFrom.MaxDate Then
          From60DaysAgo = dtpFrom.MaxDate
        ElseIf usageDB.First.Value.DATETIME < dtpFrom.MinDate Then
          From60DaysAgo = dtpFrom.MinDate
        Else
          From60DaysAgo = usageDB.First.Value.DATETIME
        End If
      Else
        From60DaysAgo = dtpFrom.MinDate
      End If
    Else
      From60DaysAgo = dtpFrom.MinDate
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
    If bClose Then
      If fDB IsNot Nothing Then
        If fDB.Visible Then fDB.Close()
        fDB.Dispose()
        fDB = Nothing
      End If
    End If
  End Sub
  Private Sub cmdAllTime_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAllTime.Click
    dtpFrom.Value = dtpFrom.MinDate
    dtpTo.Value = dtpTo.MaxDate
    cmdQuery.PerformClick()
  End Sub
  Private Sub cmdImport_Click(sender As System.Object, e As System.EventArgs) Handles cmdImport.Click
    If (usageDB Is Nothing OrElse usageDB.Count = 0) And Not modDB.LOG_State = 1 Then
      MsgDlg(Me, "The Database has not been loaded yet, please wait.", "Unable to import data.", "Database not Loaded", MessageBoxButtons.OK, _TaskDialogIcon.ResourceMonitor, MessageBoxIcon.Warning)
      Return
    End If
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
        fDB.SetAction("Importing DataBase...", "Saving DataBase...")
        LOG_Save(True)
        If fDB.Visible Then fDB.Close()
        If fDB IsNot Nothing Then
          fDB.Dispose()
          fDB = Nothing
        End If
        MsgDlg(Me, IO.Path.GetFileName(cdlOpen.FileName) & " has been merged into your history database.", "The data was successfully imported.", "Database Merged", MessageBoxButtons.OK, _TaskDialogIcon.ResourceMonitor, MessageBoxIcon.Information)
        Application.DoEvents()
        ResetDates()
      Else
        If fDB.Visible Then fDB.Close()
        If fDB IsNot Nothing Then
          fDB.Dispose()
          fDB = Nothing
        End If
        MsgDlg(Me, "Could not import " & IO.Path.GetFileName(cdlOpen.FileName) & ".", "Unable to import data.", "Database not Loaded", MessageBoxButtons.OK, _TaskDialogIcon.ResourceMonitor, MessageBoxIcon.Error)
      End If
      usageTmp = Nothing
    End If
  End Sub
  Private Sub cmdExport_Click(sender As System.Object, e As System.EventArgs) Handles cmdExport.Click
    If (usageDB Is Nothing OrElse usageDB.Count = 0) Then
      MsgDlg(Me, "The Database has not been loaded yet, please wait.", "Unable to export data.", "Database not Loaded", MessageBoxButtons.OK, _TaskDialogIcon.ResourceMonitor, MessageBoxIcon.Warning)
      Return
    End If
    Dim cdlSave As New SaveFileDialog With {.AddExtension = True, .CheckPathExists = True, .DefaultExt = "xml", .FileName = "Backup-" & mySettings.Account & ".xml", .Filter = "XML File|*.xml|CSV File|*.csv|Satellite Restriction Tracker Database|*.wb", .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments, .Title = "Export History Database"}
    If cdlSave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
      If fDB Is Nothing Then fDB = New frmDBProgress
      fDB.Show(Me)
      fDB.SetAction("Exporting DataBase...", "Saving File...")
      If chkExportRange.Checked Then
        Dim dFrom As New Date(dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day, 0, 0, 0)
        Dim dTo As New Date(dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day, 23, 59, 59)
        usageTmp = New DataBase
        For Each satRow In usageDB
          If satRow.Value.DATETIME.CompareTo(dFrom) >= 0 And satRow.Value.DATETIME.CompareTo(dTo) <= 0 Then
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
      MsgDlg(Me, "Your history has been exported to " & IO.Path.GetFileName(cdlSave.FileName) & ".", "The data was successfully exported.", "Database Exported", MessageBoxButtons.OK, _TaskDialogIcon.ResourceMonitor, MessageBoxIcon.Information)
    End If
  End Sub
  Private Sub usageTmp_ProgressState(sender As Object, e As RestrictionLibrary.DataBaseProgressEventArgs) Handles usageTmp.ProgressState
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
  Private Sub ToggleInterface(Enable As Boolean, Optional IncludeImport As Boolean = True)
    lblFrom.Enabled = Enable
    dtpFrom.Enabled = Enable
    lblTo.Enabled = Enable
    dtpTo.Enabled = Enable
    cmdToday.Enabled = Enable
    cmd30Days.Enabled = Enable
    cmd60Days.Enabled = Enable
    cmdAllTime.Enabled = Enable
    optGraph.Enabled = Enable
    optGrid.Enabled = Enable
    cmdQuery.Enabled = Enable
    If IncludeImport Then cmdImport.Enabled = Enable
    cmdExport.Enabled = Enable
    chkExportRange.Enabled = Enable
  End Sub
  Private Sub ResetDates()
    Dim dMin, dMax As Date
    LOG_Get(0, dMin, 0, 0)
    Dim fDate As Date = New Date(2000, 1, 1)
    If DateDiff(DateInterval.Year, fDate, dMin) < 0 Then dMin = fDate
    If DateDiff(DateInterval.Second, dMin, Now) < 0 Then
      pctErr.Visible = True
      pctErr.Image = New ErrorProvider().Icon.ToBitmap
      ttHistory.SetToolTip(pctErr, "The Log history is more recent than your system clock." & vbCr & "Please update your computer's time.")
    Else
      dMax = Now
      If DateDiff(DateInterval.Second, dMin, dMax) > 0 Then
        pctErr.Visible = False
        pctErr.Image = Nothing
        ttHistory.SetToolTip(pctErr, Nothing)
        dtpFrom.MinDate = fDate
        dtpTo.MinDate = fDate
        dtpFrom.MaxDate = dMax
        dtpTo.MaxDate = dMax
        dtpFrom.MinDate = dMin
        dtpTo.MinDate = dMin
      Else
        pctErr.Visible = True
        pctErr.Image = New ErrorProvider().Icon.ToBitmap
        ttHistory.SetToolTip(pctErr, "The Log history is more recent than your system clock." & vbCr & "Please update your computer's time.")
      End If
    End If
    If dtpFrom.MinDate.Year < 2000 Then dtpFrom.MinDate = fDate
    If dtpTo.MinDate.Year < 2000 Then dtpTo.MinDate = fDate
    If dtpFrom.MinDate = fDate And dtpTo.MinDate = fDate Then
      ToggleInterface(False, False)
    Else
      ToggleInterface(True, False)
    End If
  End Sub
#End Region
#Region "Notices"
  Private Enum BadDataNotes
    Null
    None
    One
  End Enum
  Private Shared Function BadDataNote(Note As BadDataNotes, ImgSize As Size) As Image
    If ImgSize.Width = 0 Or ImgSize.Height = 0 Then Return Nothing
    Dim iPic As Image = New Bitmap(ImgSize.Width, ImgSize.Height)
    Using g As Graphics = Graphics.FromImage(iPic)
      g.Clear(SystemColors.ButtonFace)
      Dim sNote As String = Nothing
      Select Case Note
        Case BadDataNotes.Null : sNote = "No data has been accumulated yet." & vbNewLine & "Please wait until you have a little more data accumulated."
        Case BadDataNotes.None : sNote = "No data was found for the specified range." & vbNewLine & "Please try a different range."
        Case BadDataNotes.One : sNote = "Not enough data has been accumulated yet." & vbNewLine & "Please try a different range."
      End Select
      g.DrawString(sNote, SystemFonts.DefaultFont, SystemBrushes.ControlText, (ImgSize.Width / 2) - (g.MeasureString(sNote, SystemFonts.DefaultFont).Width / 2), (ImgSize.Height / 2) - (g.MeasureString(sNote, SystemFonts.DefaultFont).Height / 2))
    End Using
    Return iPic
  End Function
  Private Shared Function ResizingNote(ImgSize As Size) As Image
    If ImgSize.Width = 0 Or ImgSize.Height = 0 Then Return Nothing
    Dim iPic As Image = New Bitmap(ImgSize.Width, ImgSize.Height)
    Dim g As Graphics = Graphics.FromImage(iPic)
    g.Clear(SystemColors.ButtonFace)
    g.FillRectangle(New Drawing2D.LinearGradientBrush(New Rectangle(0, 0, ImgSize.Width, ImgSize.Height), Color.White, Color.MidnightBlue, Drawing2D.LinearGradientMode.Vertical), New Rectangle(0, 0, ImgSize.Width, ImgSize.Height))
    g.Dispose()
    Return iPic
  End Function
#End Region
End Class
