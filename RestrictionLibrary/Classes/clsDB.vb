Imports System.Collections.Generic
Imports System.Xml
Public Class DataBase
  Implements ICollection(Of DataRow)
  Public Structure DataRow
    Dim DATETIME As Date
    Dim DOWNLOAD As Long
    Dim DOWNLIM As Long
    Dim UPLOAD As Long
    Dim UPLIM As Long
    Public Sub New(dTime As Date, lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long)
      DATETIME = dTime
      DOWNLOAD = lDown
      DOWNLIM = lDownLim
      UPLOAD = lUp
      UPLIM = lUpLim
    End Sub
    Public Overrides Function ToString() As String
      Return DATETIME.ToString("g") & " Down: " & DOWNLOAD.ToString & "/" & DOWNLIM.ToString & ", Up: " & UPLOAD.ToString & "/" & UPLIM.ToString
    End Function
  End Structure
  Private data() As DataRow
  Public Sub New()
    Erase data
  End Sub
  Public StopNew As Boolean
  Private sPath As String
  Private bWithDisplay As Boolean
  Public Class ProgressStateEventArgs
    Inherits EventArgs
    Private m_Value As Integer
    Private m_Total As Integer
    Public Sub New(Value As Integer, Total As Integer)
      m_Value = Value
      m_Total = Total
    End Sub
    Public ReadOnly Property Value As Integer
      Get
        Return m_Value
      End Get
    End Property
    Public ReadOnly Property Total As Integer
      Get
        Return m_Total
      End Get
    End Property
  End Class
  Public Event ProgressState(sender As Object, e As ProgressStateEventArgs)
  Public Sub New(Path As String, withDisplay As Boolean)
    sPath = Path
    bWithDisplay = withDisplay
  End Sub
  Public Sub StartNew()
    StopNew = False
    If IO.File.Exists(sPath) Then
      Dim isNew As Boolean = data Is Nothing
      Try
        If LCase(IO.Path.GetExtension(sPath)).CompareTo(".xml") = 0 Then
          Dim m_xmld As New XmlDocument
          m_xmld.Load(sPath)
          Dim m_nodelist As XmlNodeList = m_xmld.ChildNodes(1).ChildNodes
          Dim I As Integer = 0
          Dim iMax As Integer = m_nodelist.Count
          If isNew Then ReDim data(m_nodelist.Count - 1)
          For Each m_node As XmlNode In m_nodelist
            I += 1
            If bWithDisplay Then RaiseEvent ProgressState(Me, New ProgressStateEventArgs(I, iMax))
            Dim sDT, sD, sDL, sU, sUL As String : sDT = "0" : sD = "0" : sDL = "0" : sU = "0" : sUL = "0"
            For Each m_child As XmlNode In m_node.ChildNodes
              Select Case m_child.Name
                Case "DATETIME" : sDT = m_child.InnerText
                Case "DOWNLOAD" : sD = m_child.InnerText
                Case "DOWNLIM" : sDL = m_child.InnerText
                Case "UPLOAD" : sU = m_child.InnerText
                Case "UPLIM" : sUL = m_child.InnerText
              End Select
            Next
            Dim DT As Date = Xml.XmlConvert.ToDateTime(sDT, Xml.XmlDateTimeSerializationMode.RoundtripKind)
            Dim Down As Long = Long.Parse(sD)
            Dim DownLim As Long = Long.Parse(sDL)
            Dim Up As Long = Long.Parse(sU)
            Dim UpLim As Long = Long.Parse(sUL)
            If isNew Then
              data(I - 1) = New DataRow(DT, Down, DownLim, Up, UpLim)
            Else
              Add(New DataRow(DT, Down, DownLim, Up, UpLim))
            End If
            If StopNew Then Exit Sub
          Next
          m_xmld = Nothing
        ElseIf LCase(IO.Path.GetExtension(sPath)).CompareTo(".wb") = 0 Then
          Using nRead As New IO.FileStream(sPath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
            Using nIn As New IO.BinaryReader(nRead)
              Dim uRows As UInt64 = LOAD_ReadULong(nIn)
              If isNew Then ReDim data(uRows - 1)
              For I As UInt64 = 1 To uRows
                If bWithDisplay Then RaiseEvent ProgressState(Me, New ProgressStateEventArgs(I, uRows))
                Dim DT As Date = LOAD_ReadDate(nIn)
                Dim Down As Long = LOAD_ReadLong(nIn)
                Dim DownLim As Long = LOAD_ReadLong(nIn)
                Dim Up As Long = LOAD_ReadLong(nIn)
                Dim UpLim As Long = LOAD_ReadLong(nIn)
                If isNew Then
                  data(I - 1) = New DataRow(DT, Down, DownLim, Up, UpLim)
                Else
                  Add(New DataRow(DT, Down, DownLim, Up, UpLim))
                End If
                If StopNew Then Exit Sub
              Next
              nIn.Close()
            End Using
          End Using
        ElseIf LCase(IO.Path.GetExtension(sPath)).CompareTo(".csv") = 0 Then
          Using nRead As New IO.FileStream(sPath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
            Using nIn As New IO.StreamReader(nRead)
              Dim firstLine As String = nIn.ReadLine
              If String.Compare(firstLine, "Time,Download,Download Limit,Upload,Upload Limit", True) = 0 Then
                'ignore
              Else
                Dim firstData() As String = Split(firstLine, ",")
                Dim DT As Date = Date.Parse(firstData(0))
                Dim Down As Long = firstData(1)
                Dim DownLim As Long = firstData(2)
                Dim Up As Long = firstData(3)
                Dim UpLim As Long = firstData(4)
                If isNew Then
                  If data Is Nothing Then
                    ReDim data(0)
                    data(0) = New DataRow(DT, Down, DownLim, Up, UpLim)
                  Else
                    ReDim Preserve data(data.Length)
                    data(data.Length - 1) = New DataRow(DT, Down, DownLim, Up, UpLim)
                  End If
                Else
                  Add(New DataRow(DT, Down, DownLim, Up, UpLim))
                End If
              End If
              Do Until nIn.EndOfStream
                Dim rowData() As String = Split(nIn.ReadLine, ",")
                Dim DT As Date = Date.Parse(rowData(0))
                Dim Down As Long = rowData(1)
                Dim DownLim As Long = rowData(2)
                Dim Up As Long = rowData(3)
                Dim UpLim As Long = rowData(4)
                If isNew Then
                  If data Is Nothing Then
                    ReDim data(0)
                    data(0) = New DataRow(DT, Down, DownLim, Up, UpLim)
                  Else
                    ReDim Preserve data(data.Length)
                    data(data.Length - 1) = New DataRow(DT, Down, DownLim, Up, UpLim)
                  End If
                Else
                  Add(New DataRow(DT, Down, DownLim, Up, UpLim))
                End If
                If StopNew Then Exit Sub
              Loop
              nIn.Close()
            End Using
          End Using
        End If
      Catch ex As Exception
        Erase data
      End Try
    End If
  End Sub
  Public Sub Sort()
    Array.Sort(Of DataRow)(data, Function(drA As DataBase.DataRow, drB As DataBase.DataRow) Date.Compare(drA.DATETIME, drB.DATETIME))
  End Sub
  Public Sub Add(item As DataRow) Implements System.Collections.Generic.ICollection(Of DataRow).Add
    If data IsNot Nothing Then
      Dim noGood As Boolean = False
      For I As Integer = 0 To data.Length - 1
        If Math.Abs(data(I).DATETIME.Subtract(item.DATETIME).TotalMinutes) < 1 Then
          noGood = True
          Exit For
        End If
      Next
      If Not noGood Then
        Dim dLen As Integer = data.Length
        ReDim Preserve data(dLen)
        'Array.Resize(data, data.Length + 1)
        data(data.Length - 1) = item
      End If
    Else
      ReDim data(0)
      data(0) = item
    End If
  End Sub
  Public Sub Merge(db As DataBase, withDisplay As Boolean)
    Dim dbCount As Integer = db.Count
    For I As Integer = 0 To db.Count - 1
      If withDisplay Then RaiseEvent ProgressState(Me, New ProgressStateEventArgs(I + 1, dbCount))
      Add(db.data(I))
    Next
  End Sub
  Public Sub Clear() Implements System.Collections.Generic.ICollection(Of DataRow).Clear
    Erase data
  End Sub
  Public Function Contains(item As DataRow) As Boolean Implements System.Collections.Generic.ICollection(Of DataRow).Contains
    If Array.IndexOf(data, item) > -1 Then
      Return True
    Else
      Return False
    End If
  End Function
  Public Sub CopyTo(array() As DataRow, arrayIndex As Integer) Implements System.Collections.Generic.ICollection(Of DataRow).CopyTo
    System.Array.Copy(data, 0, array, arrayIndex, array.Length - arrayIndex)
    Sort()
  End Sub
  Public Function ToArray() As DataRow()
    Return data
  End Function
  Public ReadOnly Property Count As Integer Implements System.Collections.Generic.ICollection(Of DataRow).Count
    Get
      If data Is Nothing Then Return 0
      Return data.Length
    End Get
  End Property
  Public ReadOnly Property IsReadOnly As Boolean Implements System.Collections.Generic.ICollection(Of DataRow).IsReadOnly
    Get
      Return data.IsReadOnly
    End Get
  End Property
  Public Function Remove(item As DataRow) As Boolean Implements System.Collections.Generic.ICollection(Of DataRow).Remove
    For I As Integer = 0 To data.Length - 1
      If Date.Compare(data(I).DATETIME, item.DATETIME) = 0 Then
        For J As Integer = I + 1 To data.Length - 1
          data(J - 1) = data(J)
        Next
        Array.Resize(data, data.Length - 1)
        Return True
      End If
    Next
    Return False
  End Function
  Public Function Remove(DateTime As Date) As Boolean
    Return Remove(New DataRow(DateTime, 0, 0, 0, 0))
  End Function
  Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of DataRow) Implements System.Collections.Generic.IEnumerable(Of DataRow).GetEnumerator
    If data Is Nothing Then Return Nothing
    Return DirectCast(data, IEnumerable(Of DataRow)).GetEnumerator
  End Function
  Public Function GetEnumerator1() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
    If data Is Nothing Then Return Nothing
    Return DirectCast(data, IEnumerable(Of DataRow)).GetEnumerator
  End Function
  Public Function GetLast() As DataRow
    Sort()
    Return data(data.Length - 1)
  End Function
  Public Overrides Function ToString() As String
    Dim sDB As String = "<?xml version=""1.0"" standalone=""yes""?>" & vbNewLine &
                        "<RestrictionTrackerUsage>" & vbNewLine
    Sort()
    For I As UInt64 = 0 To data.LongLength - 1
      Dim dRow As DataRow = data(I)
      sDB &= "  <History>" & vbNewLine &
             "    <DATETIME>" & dRow.DATETIME.ToString("o") & "</DATETIME>" & vbNewLine &
             "    <DOWNLOAD>" & dRow.DOWNLOAD.ToString & "</DOWNLOAD>" & vbNewLine &
             "    <DOWNLIM>" & dRow.DOWNLIM.ToString & "</DOWNLIM>" & vbNewLine &
             "    <UPLOAD>" & dRow.UPLOAD.ToString & "</UPLOAD>" & vbNewLine &
             "    <UPLIM>" & dRow.UPLIM.ToString & "</UPLIM>" & vbNewLine &
             "  </History>" & vbNewLine
    Next
    sDB &= "</RestrictionTrackerUsage>"
    Return sDB
  End Function
  Public Overloads Function ToString(withDisplay As Boolean) As String
    Dim sDB As String = "<?xml version=""1.0"" standalone=""yes""?>" & vbNewLine &
                        "<RestrictionTrackerUsage>" & vbNewLine
    Sort()
    Dim uLen As ULong = CULng(data.LongLength)
    For I As UInt64 = 0 To data.LongLength - 1
      Dim dRow As DataRow = data(I)
      If withDisplay Then RaiseEvent ProgressState(Me, New ProgressStateEventArgs(I + 1UL, uLen))
      sDB &= "  <History>" & vbNewLine &
             "    <DATETIME>" & dRow.DATETIME.ToString("o") & "</DATETIME>" & vbNewLine &
             "    <DOWNLOAD>" & dRow.DOWNLOAD.ToString & "</DOWNLOAD>" & vbNewLine &
             "    <DOWNLIM>" & dRow.DOWNLIM.ToString & "</DOWNLIM>" & vbNewLine &
             "    <UPLOAD>" & dRow.UPLOAD.ToString & "</UPLOAD>" & vbNewLine &
             "    <UPLIM>" & dRow.UPLIM.ToString & "</UPLIM>" & vbNewLine &
             "  </History>" & vbNewLine
    Next
    sDB &= "</RestrictionTrackerUsage>"
    Return sDB
  End Function
  Public Function Save(Path As String, withDisplay As Boolean) As Boolean
    Dim bDelBack As Boolean = False
    If My.Computer.FileSystem.FileExists(Path) Then
      My.Computer.FileSystem.RenameFile(Path, IO.Path.GetFileName(Path) & ".bak")
      bDelBack = True
    End If
    Try
      If LCase(IO.Path.GetExtension(Path)).CompareTo(".xml") = 0 Then
        Using nWrite As New IO.FileStream(Path, IO.FileMode.Create, IO.FileAccess.ReadWrite, IO.FileShare.None)
          Using nOut As New IO.StreamWriter(nWrite)
            nOut.WriteLine("<?xml version=""1.0"" standalone=""yes""?>")
            nOut.WriteLine("<RestrictionTrackerUsage>")
            Dim uData As UInt64 = CULng(data.LongLength)
            For I As UInt64 = 0 To uData - 1
              Dim dRow As DataRow = data(I)
              If withDisplay Then RaiseEvent ProgressState(Me, New ProgressStateEventArgs(I + 1UL, uData))
              nOut.WriteLine("  <History>")
              nOut.WriteLine("    <DATETIME>" & dRow.DATETIME.ToString("o") & "</DATETIME>")
              nOut.WriteLine("    <DOWNLOAD>" & dRow.DOWNLOAD.ToString & "</DOWNLOAD>")
              nOut.WriteLine("    <DOWNLIM>" & dRow.DOWNLIM.ToString & "</DOWNLIM>")
              nOut.WriteLine("    <UPLOAD>" & dRow.UPLOAD.ToString & "</UPLOAD>")
              nOut.WriteLine("    <UPLIM>" & dRow.UPLIM.ToString & "</UPLIM>")
              nOut.WriteLine("  </History>")
            Next
            nOut.Write("</RestrictionTrackerUsage>")
          End Using
        End Using
      ElseIf LCase(IO.Path.GetExtension(Path)).CompareTo(".wb") = 0 Then
        Using nWrite As New IO.FileStream(Path, IO.FileMode.Create, IO.FileAccess.ReadWrite, IO.FileShare.None)
          Using nOut As New IO.BinaryWriter(nWrite)
            Dim uData As UInt64 = CULng(data.LongLength)
            SAVE_Write(nOut, uData)
            For I As UInt64 = 0 To uData - 1
              Dim dRow As DataRow = data(I)
              If withDisplay Then RaiseEvent ProgressState(Me, New ProgressStateEventArgs(I + 1UL, uData))
              SAVE_Write(nOut, dRow.DATETIME)
              SAVE_Write(nOut, dRow.DOWNLOAD)
              SAVE_Write(nOut, dRow.DOWNLIM)
              SAVE_Write(nOut, dRow.UPLOAD)
              SAVE_Write(nOut, dRow.UPLIM)
            Next
            nOut.Close()
          End Using
        End Using
      ElseIf LCase(IO.Path.GetExtension(Path)).CompareTo(".csv") = 0 Then
        Using nWrite As New IO.FileStream(Path, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite, IO.FileShare.None)
          Using nOut As New IO.StreamWriter(nWrite)
            Dim uData As UInt64 = CULng(data.LongLength)
            nOut.WriteLine("Time,Download,Download Limit,Upload,Upload Limit")
            For I As UInt64 = 0 To uData - 1
              Dim dRow As DataRow = data(I)
              If withDisplay Then RaiseEvent ProgressState(Me, New ProgressStateEventArgs(I + 1UL, uData))
              nOut.WriteLine(dRow.DATETIME.ToString("o") & "," & dRow.DOWNLOAD & "," & dRow.DOWNLIM & "," & dRow.UPLOAD & "," & dRow.UPLIM)
            Next
            nOut.Close()
          End Using
        End Using
      Else
        Return False
      End If
      Return True
    Catch ex As Exception
      If bDelBack Then
        My.Computer.FileSystem.DeleteFile(Path)
        My.Computer.FileSystem.RenameFile(Path & ".bak", IO.Path.GetFileName(Path))
        bDelBack = False
      End If
      Return False
    Finally
      If bDelBack Then
        My.Computer.FileSystem.DeleteFile(Path & ".bak")
      End If
    End Try
  End Function
#Region "Binary"
  Private Sub SAVE_Write(ByRef nOut As IO.BinaryWriter, value As Long)
    nOut.Write(BitConverter.GetBytes(value), 0, 8)
  End Sub
  Private Sub SAVE_Write(ByRef nOut As IO.BinaryWriter, value As ULong)
    nOut.Write(BitConverter.GetBytes(value), 0, 8)
  End Sub
  Private Sub SAVE_Write(ByRef nOut As IO.BinaryWriter, value As Date)
    nOut.Write(BitConverter.GetBytes(value.ToBinary), 0, 8)
  End Sub
  Private Function LOAD_ReadDate(ByRef nIn As IO.BinaryReader) As Date
    Return Date.FromBinary(nIn.ReadInt64)
  End Function
  Private Function LOAD_ReadLong(ByRef nIn As IO.BinaryReader) As Long
    Return nIn.ReadInt64
  End Function
  Private Function LOAD_ReadULong(ByRef nIn As IO.BinaryReader) As ULong
    Return nIn.ReadInt64
  End Function
#End Region
End Class
