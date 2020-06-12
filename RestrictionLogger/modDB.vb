Public Module modDB
  Public Enum SatHostTypes
    WildBlue
    Exede
    ExedeReseller
    RuralPortal
    Dish
    Other
  End Enum
  Private Const HistoryAge As Long = 1
  Private sFile As String
  Friend wbDB As DataBase
  Private isLoaded As Boolean
  Private isSaving As Boolean
  ReadOnly Property HistoryPath As String
    Get
      Return sFile
    End Get
  End Property
  Public Sub LOG_Add(ByVal dTime As Date, ByVal lDown As Long, ByVal lDownLim As Long, ByVal lUp As Long, ByVal lUpLim As Long)
    If Not isLoaded Then Return
    If lDownLim <= 0 Then Return
    If wbDB Is Nothing Then wbDB = New DataBase
    wbDB.Add(New DataBase.DataRow(dTime, lDown, lDownLim, lUp, lUpLim))
    LOG_Save()
  End Sub
  Public Sub LOG_Get(ByVal lngIndex As Long, ByRef dtDate As Date, ByRef lngDown As Long, ByRef lngDownLim As Long, ByRef lngUp As Long, ByRef lngUpLim As Long)
    If Not isLoaded Then Return
    If LOG_GetCount() <= lngIndex Then Return
    Dim dArr() As DataBase.DataRow = wbDB.ToArray()
    Dim dbRow As DataBase.DataRow = dArr(lngIndex)
    dtDate = dbRow.DATETIME
    lngDown = dbRow.DOWNLOAD
    lngDownLim = dbRow.DOWNLIM
    lngUp = dbRow.UPLOAD
    lngUpLim = dbRow.UPLIM
  End Sub
  Public Function LOG_GetCount() As Integer
    If Not isLoaded Then Return 0
    If wbDB Is Nothing Then Return 0
    Return wbDB.Count
  End Function
  Public Function LOG_GetLast() As Date
    If Not isLoaded Then Return New Date(1970, 1, 1)
    If LOG_GetCount() < 1 Then Return New Date(1970, 1, 1)
    Return wbDB.GetLast.DATETIME
  End Function
  Public Sub LOG_Initialize(sPath As String)
    isLoaded = False
    sFile = sPath
    If My.Computer.FileSystem.FileExists(sFile) Then wbDB = New DataBase(sFile, False)
    isLoaded = True
  End Sub
  Public Sub LOG_Terminate(withSave As Boolean)
    If Not isLoaded Then Return
    If isSaving Then Return
    If wbDB IsNot Nothing Then
      If withSave Then LOG_Save()
      wbDB = Nothing
    End If
  End Sub
  Private Sub LOG_Save(Optional SecondTry As Boolean = False)
    If Not isLoaded Then Return
    If Not String.IsNullOrEmpty(sFile) Then
      isSaving = True
      If Not My.Computer.FileSystem.DirectoryExists(IO.Path.GetDirectoryName(sFile)) Then My.Computer.FileSystem.CreateDirectory(IO.Path.GetDirectoryName(sFile))
      If InUseChecker(sFile, IO.FileAccess.Write) Then wbDB.Save(sFile, False)
      isSaving = False
    End If
  End Sub
  Private Function FileLen(Path As String) As Long
    If My.Computer.FileSystem.FileExists(Path) Then Return My.Computer.FileSystem.GetFileInfo(Path).Length
    Return -1
  End Function
  Public Function InUseChecker(Filename As String, access As IO.FileAccess) As Boolean
    If Not My.Computer.FileSystem.FileExists(Filename) Then Return True
    Dim iStart As Long = TickCount()
    Do
      Try
        Select Case access
          Case IO.FileAccess.Read
            Using fs As IO.FileStream = IO.File.Open(Filename, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite Or IO.FileShare.Delete)
              If fs.CanRead Then
                Return True
                Exit Do
              End If
            End Using
          Case IO.FileAccess.Write, IO.FileAccess.ReadWrite
            Using fs As IO.FileStream = IO.File.Open(Filename, IO.FileMode.Open, IO.FileAccess.ReadWrite, IO.FileShare.ReadWrite Or IO.FileShare.Delete)
              If fs.CanWrite Then
                Return True
                Exit Do
              End If
            End Using
        End Select
      Catch ex As Exception
      End Try
    Loop While TickCount() - iStart < 5000
    Return False
  End Function
  Public Function TickCount() As Long
    Return (Stopwatch.GetTimestamp / Stopwatch.Frequency) * 1000
  End Function
End Module
