Imports System.IO
Module modFunctions
  Public Const LATIN_1 As Integer = 28591
  Public Function PercentEncode(inString As String) As String
    Dim sRet As String = String.Empty
    For I As Integer = inString.Length - 1 To 0 Step -1
      Dim iChar As Integer = Convert.ToInt32(inString(I))
      Select Case iChar
        Case 48 To 57, 65 To 90, 97 To 122 : sRet = inString(I).ToString & sRet
        Case 32 : sRet = "+" & sRet
        Case Else : sRet = "%" & PadHex(iChar, 2) & sRet
      End Select
    Next
    Return sRet
  End Function
  Private Function PadHex(Value As UInt32, Length As UInt16) As String
    Dim sVal As String = Convert.ToString(Value, 16)
    Do While sVal.Length < Length : sVal = "0" & sVal : Loop
    Return sVal
  End Function
  ''' <summary>
  ''' Attempts to see if a file is in use, waiting up to five seconds for it to be freed.
  ''' </summary>
  ''' <param name="Filename">The exact path to the file which needs to be checked.</param>
  ''' <param name="access">Write permissions required for checking.</param>
  ''' <returns>True on available, false on in use.</returns>
  ''' <remarks></remarks>
  Public Function InUseChecker(Filename As String, access As IO.FileAccess) As Boolean
    If Not IO.File.Exists(Filename) Then Return True
    Dim iStart As Long = TickCount()
    Do
      Try
        Select Case access
          Case FileAccess.Read
            'only check for ability to read
            Using fs As FileStream = IO.File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite Or FileShare.Delete)
              If fs.CanRead Then
                Return True
                Exit Do
              End If
            End Using
          Case FileAccess.Write, FileAccess.ReadWrite
            'check for ability to write
            Using fs As FileStream = IO.File.Open(Filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite Or FileShare.Delete)
              If fs.CanWrite Then
                Return True
                Exit Do
              End If
            End Using
        End Select
      Catch ex As Exception
      End Try
      Threading.Thread.Sleep(100)
      Threading.Thread.Sleep(0)
      Threading.Thread.Sleep(100)
    Loop While TickCount() - iStart < 5000
    Return False
  End Function
  Public Function TickCount() As Long
    Return (Stopwatch.GetTimestamp / Stopwatch.Frequency) * 1000
  End Function
  Public Function IterativeEqualityCheck(inArray1() As Byte, inArray2() As Byte) As Boolean
    If inArray1.Length = inArray2.Length Then
      Dim isEqual As Boolean = True
      For I As Integer = 0 To inArray1.Length - 1
        If Not inArray1(I) = inArray2(I) Then
          isEqual = False
          Exit For
        End If
      Next
      Return isEqual
    Else
      Return False
    End If
  End Function
End Module