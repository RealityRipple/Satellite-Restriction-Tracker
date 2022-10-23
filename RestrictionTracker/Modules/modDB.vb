Friend Module modDB
  Private Const HistoryAge As Long = 1
  Private sFile As String
  Friend WithEvents usageDB As DataBase
  Private isLoaded As Boolean
  Private isSaving As Boolean
  ReadOnly Property LOG_State As Byte
    Get
      If Not isLoaded Then Return 0
      If isSaving Then Return 2
      Return 1
    End Get
  End Property
  Public Sub LOG_Add(dTime As Date, lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long, Optional Save As Boolean = True)
    If Not isLoaded Then Return
    If lDownLim <= 0 Then Return
    If usageDB Is Nothing Then
      usageDB = New DataBase
      usageDB.StartNew()
    End If
    usageDB.Add(New DataBase.DataRow(dTime, lDown, lDownLim, lUp, lUpLim))
    If Save Then
      Dim tX As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf LOG_Save))
      tX.Start(False)
    End If
  End Sub
  Public Sub LOG_Get(lngIndex As Long, ByRef dtDate As Date, ByRef lngDown As Long, ByRef lngDownLim As Long, ByRef lngUp As Long, ByRef lngUpLim As Long)
    If Not isLoaded Then Return
    If LOG_GetCount() <= lngIndex Then Return
    Dim dArr() As DataBase.DataRow = usageDB.ToArray()
    Dim dbRow As DataBase.DataRow = dArr(lngIndex)
    dtDate = dbRow.DATETIME
    lngDown = dbRow.DOWNLOAD
    lngDownLim = dbRow.DOWNLIM
    lngUp = dbRow.UPLOAD
    lngUpLim = dbRow.UPLIM
  End Sub
  Public Function LOG_GetRange(dtStart As Date, dtEnd As Date) As DataBase.DataRow()
    Dim lRet As New Collections.Generic.List(Of DataBase.DataRow)
    If Not isLoaded Then Return lRet.ToArray
    Dim kStart As UInt64 = Math.Floor(dtStart.Ticks / 600000000)
    Dim kEnd As UInt64 = Math.Floor(dtEnd.Ticks / 600000000)
    For Each dRow As Collections.Generic.KeyValuePair(Of UInt64, DataBase.DataRow) In usageDB
      If dRow.Key >= kStart And dRow.Key <= kEnd Then lRet.Add(dRow.Value)
      If dRow.Key > kEnd Then Exit For
    Next
    Return lRet.ToArray
  End Function
  Public Function LOG_GetCount() As Integer
    If Not isLoaded Then Return 0
    If usageDB Is Nothing Then Return 0
    Return usageDB.Count
  End Function
  Public Function LOG_GetLast() As Date
    If Not isLoaded Then Return New Date(1970, 1, 1)
    If LOG_GetCount() < 1 Then Return New Date(1970, 1, 1)
    Return usageDB.LastRow.DATETIME
  End Function
  Public Sub LOG_Initialize(sAccount As String, withDisplay As Boolean)
    isLoaded = False
    sFile = IO.Path.Combine(MySaveDir(True), "History-" & sAccount & ".wb")
    If Not My.Computer.FileSystem.FileExists(sFile) Then sFile = IO.Path.Combine(MySaveDir(True), "History-" & sAccount & ".xml")
    If My.Computer.FileSystem.FileExists(sFile) Then
      usageDB = New DataBase(sFile, withDisplay)
      usageDB.StartNew()
      If sFile = IO.Path.Combine(MySaveDir(True), "History-" & sAccount & ".xml") Then
        sFile = IO.Path.Combine(MySaveDir(True), "History-" & sAccount & ".wb")
        usageDB.Save(sFile, withDisplay)
        If srlFunctions.InUseChecker(IO.Path.Combine(MySaveDir(True), "History-" & sAccount & ".xml"), IO.FileAccess.Write) Then
          My.Computer.FileSystem.DeleteFile(IO.Path.Combine(MySaveDir(True), "History-" & sAccount & ".xml"))
        End If
      End If
    Else
      If sFile = IO.Path.Combine(MySaveDir(True), "History-" & sAccount & ".xml") Then sFile = IO.Path.Combine(MySaveDir(True), "History-" & sAccount & ".wb")
    End If
    isLoaded = True
  End Sub
  Public Sub LOG_Terminate(withSave As Boolean)
    If Not isLoaded Then
      If usageDB IsNot Nothing Then usageDB.StopNew = True
      Return
    End If
    Do While isSaving
      Application.DoEvents()
    Loop
    If usageDB IsNot Nothing Then
      If withSave Then LOG_Save(False)
      usageDB = Nothing
    End If
  End Sub
  Friend Sub LOG_Save(withDisplay As Boolean)
    If Not isLoaded Then Return
    If Not String.IsNullOrEmpty(sFile) Then
      isSaving = True
      If Not My.Computer.FileSystem.DirectoryExists(IO.Path.GetDirectoryName(sFile)) Then My.Computer.FileSystem.CreateDirectory(IO.Path.GetDirectoryName(sFile))
      If srlFunctions.InUseChecker(sFile, IO.FileAccess.Write) Then
        usageDB.Save(sFile, withDisplay)
      Else
        MsgDlg(Nothing, "Your history file could not be saved because another program is using it!", "History could not be saved.", "File in Use", MessageBoxButtons.OK, _TaskDialogIcon.InternetTime, MessageBoxIcon.Error)
      End If
      isSaving = False
    End If
  End Sub
  Private Sub usageDB_ProgressState(sender As Object, e As RestrictionLibrary.DataBase.ProgressStateEventArgs) Handles usageDB.ProgressState
    If frmDBProgress.Visible Then frmDBProgress.SetProgress(e.Value, e.Total)
  End Sub
End Module
