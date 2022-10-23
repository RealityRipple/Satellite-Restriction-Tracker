Friend Module modDB
  Private Const HistoryAge As Long = 1
  Private sFile As String
  Friend wbDB As DataBase
  Private isLoaded As Boolean
  Private isSaving As Boolean
  Public Sub LOG_Add(ByVal dTime As Date, ByVal lUsed As Long, ByVal lLimit As Long)
    If Not isLoaded Then Return
    If lLimit <= 0 Then Return
    If wbDB Is Nothing Then wbDB = New DataBase
    wbDB.Add(New DataRow(dTime, lUsed, lLimit))
    LOG_Save()
  End Sub
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
  Private Sub LOG_Save()
    If Not isLoaded Then Return
    If Not String.IsNullOrEmpty(sFile) Then
      isSaving = True
      If Not My.Computer.FileSystem.DirectoryExists(IO.Path.GetDirectoryName(sFile)) Then My.Computer.FileSystem.CreateDirectory(IO.Path.GetDirectoryName(sFile))
      If InUseChecker(sFile, IO.FileAccess.Write) Then wbDB.Save(sFile, False)
      isSaving = False
    End If
  End Sub
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
