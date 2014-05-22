Public Module modDB
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
  ReadOnly Property HistoryPath As String
    Get
      Return sFile
    End Get
  End Property
  Public Sub LOG_Add(dTime As Date, lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long, Optional Save As Boolean = True)
    If Not isLoaded Then Exit Sub
    'If Math.Abs(DateDiff(DateInterval.Minute, dTime, LOG_GetLast)) >= HistoryAge Then
    If lDownLim > 0 Then
      If usageDB Is Nothing Then
        usageDB = New DataBase
        usageDB.StartNew()
      End If
      usageDB.Add(New DataBase.DataRow(dTime, lDown, lDownLim, lUp, lUpLim))
      If Save Then
        LOG_Sort()
        Dim tX As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf LOG_Save))
        tX.Start(False)
      End If
    End If
    'End If
  End Sub
  Public Sub LOG_Get(lngIndex As Long, ByRef dtDate As Date, ByRef lngDown As Long, ByRef lngDownLim As Long, ByRef lngUp As Long, ByRef lngUpLim As Long)
    If Not isLoaded Then Exit Sub
    If LOG_GetCount() > lngIndex Then
      Dim dbRow As DataBase.DataRow = usageDB(lngIndex)
      dtDate = dbRow.DATETIME
      lngDown = dbRow.DOWNLOAD
      lngDownLim = dbRow.DOWNLIM
      lngUp = dbRow.UPLOAD
      lngUpLim = dbRow.UPLIM
    End If
  End Sub
  Public Function LOG_GetCount() As Integer
    If Not isLoaded Then Return 0
    If usageDB Is Nothing Then Return 0
    Return usageDB.Count
  End Function
  Public Function LOG_GetLast() As Date
    If Not isLoaded Then Return DateSerial(1970, 1, 1)
    If LOG_GetCount() > 0 Then
      Return usageDB(LOG_GetCount() - 1).DATETIME
    Else
      Return DateSerial(1970, 1, 1)
    End If
  End Function
  Public Sub LOG_Initialize(sAccount As String, withDisplay As Boolean)
    isLoaded = False
    sFile = MySaveDir & "\History-" & sAccount & ".wb"
    If Not My.Computer.FileSystem.FileExists(sFile) Then sFile = MySaveDir & "\History-" & sAccount & ".xml"
    If My.Computer.FileSystem.FileExists(sFile) Then
      usageDB = New DataBase(sFile, withDisplay)
      usageDB.StartNew()
      If sFile = MySaveDir & "\History-" & sAccount & ".xml" Then
        sFile = MySaveDir & "\History-" & sAccount & ".wb"
        usageDB.Save(sFile, withDisplay)
        If InUseChecker(MySaveDir & "\History-" & sAccount & ".xml", IO.FileAccess.Write) Then
          My.Computer.FileSystem.DeleteFile(MySaveDir & "\History-" & sAccount & ".xml")
        End If
      End If
    Else
      If sFile = MySaveDir & "\History-" & sAccount & ".xml" Then sFile = MySaveDir & "\History-" & sAccount & ".wb"
    End If
    isLoaded = True
  End Sub
  Public Sub LOG_Terminate(withSave As Boolean)
    If Not isLoaded Then
      If usageDB IsNot Nothing Then usageDB.StopNew = True
      Exit Sub
    End If
    Do While isSaving
      Application.DoEvents()
    Loop
    If usageDB IsNot Nothing Then
      If withSave Then LOG_Save(False)
      usageDB = Nothing
    End If
  End Sub
  Public Sub LOG_Sort()
    If Not isLoaded Then Exit Sub
    Do While isSaving
      Application.DoEvents()
    Loop
    If usageDB IsNot Nothing Then
      usageDB.Sort()
    End If
  End Sub
  Friend Sub LOG_Save(withDisplay As Boolean)
    If Not isLoaded Then Exit Sub
    If Not String.IsNullOrEmpty(sFile) Then
      isSaving = True
      If Not My.Computer.FileSystem.DirectoryExists(IO.Path.GetDirectoryName(sFile)) Then My.Computer.FileSystem.CreateDirectory(IO.Path.GetDirectoryName(sFile))
      If InUseChecker(sFile, IO.FileAccess.Write) Then
        usageDB.Save(sFile, withDisplay)
      Else
        MsgBox("Your history file could not be saved because another program is using it!", MsgBoxStyle.Critical Or MsgBoxStyle.SystemModal)
      End If
      isSaving = False
    End If
  End Sub
  Private Function FileLen(Path As String) As Long
    If My.Computer.FileSystem.FileExists(Path) Then Return My.Computer.FileSystem.GetFileInfo(Path).Length
    Return -1
  End Function

  Private Sub usageDB_ProgressState(sender As Object, e As RestrictionLibrary.DataBase.ProgressStateEventArgs) Handles usageDB.ProgressState
    If frmDBProgress.Visible Then frmDBProgress.SetProgress(e.Value, e.Total)
  End Sub
End Module