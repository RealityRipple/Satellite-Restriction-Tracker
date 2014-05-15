﻿Imports System.IO
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
  Public Delegate Sub ReportSocketErrorInvoker(ex As Exception)
  Public Sub ReportSocketError(ex As Exception)
    Dim ReportList As String = AppData & "\sckerrs.log"
    If IO.File.Exists(ReportList) Then
      If InUseChecker(ReportList, FileAccess.ReadWrite) Then
        My.Computer.FileSystem.WriteAllText(ReportList, ex.Message & vbNewLine, True)
        If ex.InnerException IsNot Nothing Then My.Computer.FileSystem.WriteAllText(ReportList, ex.InnerException.Message & vbNewLine, True)
        My.Computer.FileSystem.WriteAllText(ReportList, vbNewLine, True)
      End If
    Else
      My.Computer.FileSystem.WriteAllText(ReportList, ex.Message & vbNewLine, False)
      If ex.InnerException IsNot Nothing Then My.Computer.FileSystem.WriteAllText(ReportList, ex.InnerException.Message & vbNewLine, True)
      My.Computer.FileSystem.WriteAllText(ReportList, vbNewLine, True)
    End If
    SendSocketErrors()
  End Sub
  Public Sub SendSocketErrors()
    Dim ReportList As String = AppData & "\sckerrs.log"
    If IO.File.Exists(ReportList) Then
      Dim reports As New Collections.Generic.List(Of String)(Split(My.Computer.FileSystem.ReadAllText(ReportList), vbNewLine & vbNewLine))
      For I As Integer = reports.Count - 1 To 0 Step -1
        Dim ReportBunch As String = reports(I)
        If Not String.IsNullOrEmpty(ReportBunch) Then
          Dim e, ie As String
          If ReportBunch.Contains(vbNewLine) Then
            e = Split(ReportBunch, vbNewLine)(0)
            ie = Split(ReportBunch, vbNewLine)(1)
          Else
            e = ReportBunch
            ie = Nothing
          End If
          Try
            Using sckUpload As New CookieAwareWebClient
              Dim params As New Collections.Specialized.NameValueCollection
              params.Add("e", e)
              If Not String.IsNullOrEmpty(ie) Then params.Add("ie", ie)
              Dim bRet() As Byte = sckUpload.UploadValues("http://wb.realityripple.com/errmsgs.php", "POST", params)
              Dim sRet As String = System.Text.Encoding.GetEncoding(LATIN_1).GetString(bRet)
              If sRet = "e exists" Or sRet = "e added" Then
                reports.RemoveAt(I)
              Else
                Debug.Print(sRet)
              End If
            End Using
          Catch
            Exit For
          End Try
        Else
          reports.RemoveAt(I)
        End If
      Next
      If InUseChecker(ReportList, FileAccess.ReadWrite) Then
        If reports.Count > 0 Then
          If reports.Count = 1 AndAlso String.IsNullOrEmpty(reports(0)) Then
            IO.File.Delete(ReportList)
          Else
            My.Computer.FileSystem.WriteAllText(ReportList, Join(reports.ToArray, vbNewLine & vbNewLine) & vbNewLine & vbNewLine, False)
          End If
        Else
          IO.File.Delete(ReportList)
        End If
      End If
    End If
  End Sub
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
  Private ReadOnly Property AppData As String
    Get
      Static sTmp As String
      If Not My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & My.Application.Info.CompanyName) Then My.Computer.FileSystem.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & My.Application.Info.CompanyName)
      If Not My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & My.Application.Info.CompanyName & "\" & My.Application.Info.ProductName) Then My.Computer.FileSystem.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & My.Application.Info.CompanyName & "\" & My.Application.Info.ProductName)
      If String.IsNullOrEmpty(sTmp) Then
        sTmp = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & My.Application.Info.CompanyName & "\" & My.Application.Info.ProductName
      End If
      Return sTmp
    End Get
  End Property
End Module