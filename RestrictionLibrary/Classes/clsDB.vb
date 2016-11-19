Imports System.Collections.Generic
Imports System.Xml
''' <summary>
''' Stores information about usage activity for an account, with access to subroutines and functions for manipulating the list and storing it as one of three file types.
''' </summary>
''' <remarks>This class also contains a child <see cref="DataBase.DataRow" /> class which provides access to individual events of this larger list and is used as a Type in some instances.</remarks>
Public Class DataBase
  Implements ICollection(Of DataRow)
  ''' <summary>
  ''' Stores information about usage activity at a single date and time.
  ''' </summary>
  ''' <remarks></remarks>
  Public Structure DataRow
    ''' <summary>
    ''' The Date and Time of the data stored in this row.
    ''' </summary>
    ''' <remarks></remarks>
    Public DATETIME As Date
    ''' <summary>
    ''' The number of Megabytes used in download activity at the specified date and time.
    ''' </summary>
    ''' <remarks></remarks>
    Public DOWNLOAD As Long
    ''' <summary>
    ''' The maximum number of Megabytes allowed in download activity for the plan at the specified date and time.
    ''' </summary>
    ''' <remarks></remarks>
    Public DOWNLIM As Long
    ''' <summary>
    ''' The number of Megabytes used in upload activity at the specified date and time.
    ''' </summary>
    ''' <remarks></remarks>
    Public UPLOAD As Long
    ''' <summary>
    ''' The maximum number of Megabytes allowed in upload activity for the plan at the specified date and time.
    ''' </summary>
    ''' <remarks></remarks>
    Public UPLIM As Long
    ''' <summary>
    ''' Create a new DataRow entry.
    ''' </summary>
    ''' <param name="dTime">The Date and Time of the entry.</param>
    ''' <param name="lDown">The number of Megabytes used in download activity for this entry.</param>
    ''' <param name="lDownLim">The maximum number of Megabytes allowed in download activity for this entry.</param>
    ''' <param name="lUp">The number of Megabytes used in upload activity for this entry.</param>
    ''' <param name="lUpLim">The maximum number of Megabytes allowed in upload activity for this entry.</param>
    ''' <remarks></remarks>
    Public Sub New(dTime As Date, lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long)
      DATETIME = dTime
      DOWNLOAD = lDown
      DOWNLIM = lDownLim
      UPLOAD = lUp
      UPLIM = lUpLim
    End Sub
    ''' <summary>
    ''' Get a string representation of the Date and Time of the data stored in this row.
    ''' </summary>
    ''' <returns>The date and time are returned in the format "MM/DD/YYYY HH:MM", or the standard "g".</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property sDATETIME As String
      Get
        Return DATETIME.ToString("g")
      End Get
    End Property
    ''' <summary>
    ''' Get a string representation of the number of Megabytes used in download activity for this row.
    ''' </summary>
    ''' <returns>The download value is returned with standard thousands separators and no decimal.</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property sDOWNLOAD As String
      Get
        Return DOWNLOAD.ToString("N0")
      End Get
    End Property
    ''' <summary>
    ''' Get a string representation of the number of Megabytes allowed in download activity for this row.
    ''' </summary>
    ''' <returns>The download limit is returned with standard thousands separators and no decimal.</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property sDOWNLIM As String
      Get
        Return DOWNLIM.ToString("N0")
      End Get
    End Property
    ''' <summary>
    ''' Get a string representation of the number of Megabytes used in upload activity for this row.
    ''' </summary>
    ''' <returns>The upload value is returned with standard thousands separators and no decimal.</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property sUPLOAD As String
      Get
        Return UPLOAD.ToString("N0")
      End Get
    End Property
    ''' <summary>
    ''' Get a string representation of the number of Megabytes allowed in upload activity for this row.
    ''' </summary>
    ''' <returns>The upload limit is returned with standard thousands separators and no decimal.</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property sUPLIM As String
      Get
        Return UPLIM.ToString("N0")
      End Get
    End Property
    ''' <summary>
    ''' Get a string representation of this row in the format "MM/DD/YYYY HH:MM [Down: 0,000/0,000][, ][Up: 0,000/0,000]".
    ''' </summary>
    ''' <returns>The return value will always contain the <see cref="sDATETIME" /> value, and will be followed with Download and/or Upload values if either the value or limit is greater than 0.</returns>
    ''' <remarks></remarks>
    Public Overrides Function ToString() As String
      Dim sRet As String = sDATETIME
      If DOWNLOAD > 0 Or DOWNLIM > 0 Then
        sRet &= " Down: " & sDOWNLOAD & "/" & sDOWNLIM
      End If
      If UPLOAD > 0 Or UPLIM > 0 Then
        If sRet.Contains(" Down: ") Then sRet &= ","
        sRet &= " Up: " & sUPLOAD & "/" & sUPLIM
      End If
      Return sRet
    End Function
    ''' <summary>
    ''' This is a standard empty DataRow with all values set to zero.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property Empty() As DataRow
      Get
        Return New DataRow(Date.FromBinary(0), 0, 0, 0, 0)
      End Get
    End Property
    ''' <summary>
    ''' Check a <see cref="DataRow" /> entry to see if it is empty or unset.
    ''' </summary>
    ''' <param name="dRow">The <see cref="DataRow" /> entry you wish to check.</param>
    ''' <returns>A boolean value of <c>True</c> is returned if the <paramref name="dRow" /> entry is empty, <c>False</c> otherwise.</returns>
    ''' <remarks></remarks>
    Public Shared Function IsEmpty(dRow As DataRow) As Boolean
      Return (dRow.DATETIME.ToBinary = 0 And dRow.DOWNLOAD = 0 And dRow.UPLOAD = 0 And dRow.DOWNLIM = 0 And dRow.UPLIM = 0)
    End Function
    ''' <summary>
    ''' Check this <see cref="DataRow" /> entry to see if it is empty or unset.
    ''' </summary>
    ''' <returns>A boolean value of <c>True</c> is returned if this entry is empty, <c>False</c> otherwise.</returns>
    ''' <remarks></remarks>
    Public Function IsEmpty() As Boolean
      Return (DATETIME.ToBinary = 0 And DOWNLOAD = 0 And UPLOAD = 0 And DOWNLIM = 0 And UPLIM = 0)
    End Function
  End Structure
  Private data() As DataRow
  ''' <summary>
  ''' Create a new DataBase with no entries.
  ''' </summary>
  ''' <remarks>If this subroutine is called on an existing database, it will be erased.</remarks>
  Public Sub New()
    Erase data
  End Sub
  ''' <summary>
  ''' Set this value to true to stop loading a databased called from the initialization subroutine.
  ''' </summary>
  ''' <remarks></remarks>
  Public StopNew As Boolean
  Private sPath As String
  Private bWithDisplay As Boolean
  Public Class ProgressStateEventArgs
    Inherits EventArgs
    Private m_Value As Integer
    Private m_Total As Integer
    ''' <summary>
    ''' A progress value containing the current number of things done and the total number of things to do.
    ''' </summary>
    ''' <param name="Value">The current number of things that have been done.</param>
    ''' <param name="Total">The total number of things there are to do.</param>
    ''' <remarks></remarks>
    Public Sub New(Value As Integer, Total As Integer)
      m_Value = Value
      m_Total = Total
    End Sub
    ''' <summary>
    ''' The Current number of things that have been done.
    ''' </summary>
    ''' <remarks></remarks>
    Public ReadOnly Property Value As Integer
      Get
        Return m_Value
      End Get
    End Property
    ''' <summary>
    ''' The total number of things to do.
    ''' </summary>
    ''' <remarks></remarks>
    Public ReadOnly Property Total As Integer
      Get
        Return m_Total
      End Get
    End Property
  End Class
  ''' <summary>
  ''' The loading of the DataBase has progressed to a specific point.
  ''' </summary>
  ''' <param name="sender">A reference to this <see cref="DataBase" /> class.</param>
  ''' <param name="e">A <see cref="ProgressStateEventArgs" /> entry with the current Value and Total of the DataBase load's progress.</param>
  ''' <remarks></remarks>
  Public Event ProgressState(sender As Object, e As ProgressStateEventArgs)
  ''' <summary>
  ''' Create a new DataBase with entries loaded from the specified <paramref name="Path" />.
  ''' </summary>
  ''' <param name="Path">Path to a saved DataBase in either WB, XML, or CSV formats.</param>
  ''' <param name="withDisplay">Return the current progress of the load through the ProgressState event.</param>
  ''' <remarks>This subroutine will not begin the load process. You must call <see cref="StartNew" /> to begin loading the database. This is to allow initialization and loading at separate times or on separate threads.</remarks>
  Public Sub New(Path As String, withDisplay As Boolean)
    sPath = Path
    bWithDisplay = withDisplay
  End Sub
  ''' <summary>
  ''' Begin loading DataBase entries from the path specified in the snitialization subroutine.
  ''' </summary>
  ''' <remarks></remarks>
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
            If StopNew Then Return
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
                If StopNew Then Return
              Next
              nIn.Close()
            End Using
          End Using
        ElseIf LCase(IO.Path.GetExtension(sPath)).CompareTo(".csv") = 0 Then
          Using nRead As New IO.FileStream(sPath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
            Using nIn As New IO.StreamReader(nRead)
              Dim firstLine As String = nIn.ReadLine
              If Not String.Compare(firstLine, "Time,Download,Download Limit,Upload,Upload Limit", True) = 0 Then
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
                If StopNew Then Return
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
  ''' <summary>
  ''' Organize the entire DataBase by date and time.
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub Sort()
    Array.Sort(Of DataRow)(data, Function(drA As DataBase.DataRow, drB As DataBase.DataRow) Date.Compare(drA.DATETIME, drB.DATETIME))
  End Sub
  ''' <summary>
  ''' Add a new DataRow entry to the DataBase.
  ''' </summary>
  ''' <param name="item">The new DataRow entry to add.</param>
  ''' <remarks>The entry won't be added if it's less than one minute apart from another DataRow in the DataBase.</remarks>
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
        data(data.Length - 1) = item
      End If
    Else
      ReDim data(0)
      data(0) = item
    End If
  End Sub
  ''' <summary>
  ''' Add all the entries of another DataBase into this one.
  ''' </summary>
  ''' <param name="db">The other DataBase to include in this one.</param>
  ''' <param name="withDisplay">Return the current progress of the merge through the ProgressState event.</param>
  ''' <remarks>The original DataBase will not be modified or erased by this process.</remarks>
  Public Sub Merge(db As DataBase, withDisplay As Boolean)
    Dim dbCount As Integer = db.Count
    For I As Integer = 0 To db.Count - 1
      If withDisplay Then RaiseEvent ProgressState(Me, New ProgressStateEventArgs(I + 1, dbCount))
      Add(db.data(I))
    Next
  End Sub
  ''' <summary>
  ''' Remove all entries from this DataBase.
  ''' </summary>
  ''' <remarks>This is literally identical to the initialization subroutine without parameters.</remarks>
  Public Sub Clear() Implements System.Collections.Generic.ICollection(Of DataRow).Clear
    Erase data
  End Sub
  ''' <summary>
  ''' Check for existence of an item by searching for an exact match. 
  ''' </summary>
  ''' <param name="item">The exact match of the <see cref="DataRow" /> you wish to find.</param>
  ''' <returns><c>True</c> is returned if the DataBase contains at least one copy of the <paramref name="item" />, <c>False</c> otherwise.</returns>
  ''' <remarks></remarks>
  Public Function Contains(item As DataRow) As Boolean Implements System.Collections.Generic.ICollection(Of DataRow).Contains
    If Array.IndexOf(data, item) > -1 Then
      Return True
    Else
      Return False
    End If
  End Function
  ''' <summary>
  ''' Copy data from this DataBase into an array of DataRows.
  ''' </summary>
  ''' <param name="array">The array of <see cref="DataRow" />s in which to copy this DataBase.</param>
  ''' <param name="arrayIndex">The location at which to start copying data into in the <paramref name="array" />.</param>
  ''' <remarks>
  ''' The number of items to be copied will be equal to the space allocated for items in the <paramref name="array" /> from the <paramref name="arrayIndex" /> value to the end.
  ''' </remarks>
  Public Sub CopyTo(array() As DataRow, arrayIndex As Integer) Implements System.Collections.Generic.ICollection(Of DataRow).CopyTo
    System.Array.Copy(data, 0, array, arrayIndex, array.Length - arrayIndex)
  End Sub
  ''' <summary>
  ''' Convert this DataBase into an array of <see cref="DataRow" /> entries.
  ''' </summary>
  ''' <returns>The full list of entries in this DataBase.</returns>
  ''' <remarks></remarks>
  Public Function ToArray() As DataRow()
    Return data
  End Function
  ''' <summary>
  ''' The total number of entries in this DataBase.
  ''' </summary>
  ''' <remarks>The value will be 0 if the DataBase is not initialized or if it's been erased.</remarks>
  Public ReadOnly Property Count As Integer Implements System.Collections.Generic.ICollection(Of DataRow).Count
    Get
      If data Is Nothing Then Return 0
      Return data.Length
    End Get
  End Property
  ''' <summary>
  ''' Gets a value indicating whether the DataBase is read-only.
  ''' </summary>
  ''' <value></value>
  ''' <returns>This property is always false because it is always false for all arrays.</returns>
  ''' <remarks></remarks>
  Public ReadOnly Property IsReadOnly As Boolean Implements System.Collections.Generic.ICollection(Of DataRow).IsReadOnly
    Get
      Return data.IsReadOnly
    End Get
  End Property
  ''' <summary>
  ''' Remove a DataRow entry from the DataBase.
  ''' </summary>
  ''' <param name="item">The DataRow entry to remove.</param>
  ''' <returns><c>True</c> if the entry was successfully removed from the DataBase, <c>False</c> if the entry could not be found.</returns>
  ''' <remarks>This function only uses the <see cref="DataRow.DATETIME" /> value to compare against entries in the DataBase.</remarks>
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
  ''' <summary>
  ''' Remove a DataRow entry from the DataBase.
  ''' </summary>
  ''' <param name="DateTime">The Date and Time of the DataRow entry to remove.</param>
  ''' <returns><c>True</c> if the entry was successfully removed from the DataBase, <c>False</c> if the entry could not be found.</returns>
  ''' <remarks></remarks>
  Public Function Remove(DateTime As Date) As Boolean
    Return Remove(New DataRow(DateTime, 0, 0, 0, 0))
  End Function
  ''' <summary>
  ''' Returns an enumerator that iterates through the DataBase.
  ''' </summary>
  ''' <returns>A <see cref="System.Collections.Generic.IEnumerable(Of DataRow)" /> that can be used to iterate through the DataBase.</returns>
  ''' <remarks></remarks>
  Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of DataRow) Implements System.Collections.Generic.IEnumerable(Of DataRow).GetEnumerator
    If data Is Nothing Then Return Nothing
    Return DirectCast(data, IEnumerable(Of DataRow)).GetEnumerator
  End Function
  ''' <summary>
  ''' Returns an enumerator that iterates through the DataBase.
  ''' </summary>
  ''' <returns>A <see cref="System.Collections.Generic.IEnumerator" /> that can be used to iterate through the DataBase.</returns>
  ''' <remarks></remarks>
  Public Function GetEnumerator1() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
    If data Is Nothing Then Return Nothing
    Return DirectCast(data, IEnumerable(Of DataRow)).GetEnumerator
  End Function
  ''' <summary>
  ''' Returns the latest entry in the DataBase.
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks>This function will sort the DataBase before returning a value.</remarks>
  Public Function GetLast() As DataRow
    Sort()
    Return data(data.Length - 1)
  End Function
  ''' <summary>
  ''' A full XML representation of the DataBase.
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks>This function will sort the DataBase before returning a value.</remarks>
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
  ''' <summary>
  ''' A full XML representation of the DataBase.
  ''' </summary>
  ''' <param name="withDisplay">Return the current progress of the function through the ProgressState event.</param>
  ''' <returns></returns>
  ''' <remarks>This function will sort the DataBase before returning a value.</remarks>
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
  ''' <summary>
  ''' Save the DataBase to the specified <paramref name="Path" />.
  ''' </summary>
  ''' <param name="Path">Path to a save location for the DataBase in either WB, XML, or CSV formats.</param>
  ''' <param name="withDisplay">Return the current progress of the save through the ProgressState event.</param>
  ''' <returns><c>True</c> is returned if </returns>
  ''' <remarks></remarks>
  Public Function Save(Path As String, withDisplay As Boolean) As Boolean
    If data Is Nothing Then Return False
    Dim bDelBack As Boolean = False
    If My.Computer.FileSystem.FileExists(Path) Then
      My.Computer.FileSystem.RenameFile(Path, IO.Path.GetFileName(Path) & ".bak")
      bDelBack = True
    End If
    Dim bFreedom As Boolean = True
    Dim sample As UInt64 = CULng(data.LongLength) - 1
    If sample > 15 Then sample = 15
    For I As UInt64 = 0 To sample
      Dim dRow As DataRow = data(I)
      If Not (dRow.DOWNLIM = 150000 And dRow.UPLIM = 150000) Then
        bFreedom = False
        Exit For
      End If
    Next
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
              If dRow.DOWNLOAD = 0 And dRow.UPLOAD = 0 And dRow.DOWNLIM = 0 And dRow.UPLIM = 0 Then Continue For
              If Not bFreedom And (dRow.DOWNLOAD = 0 And dRow.UPLOAD = 0 And dRow.DOWNLIM = 150000 And dRow.UPLIM = 150000) Then Continue For
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
              If dRow.DOWNLOAD = 0 And dRow.UPLOAD = 0 And dRow.DOWNLIM = 0 And dRow.UPLIM = 0 Then Continue For
              If Not bFreedom And (dRow.DOWNLOAD = 0 And dRow.UPLOAD = 0 And dRow.DOWNLIM = 150000 And dRow.UPLIM = 150000) Then Continue For
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
              If dRow.DOWNLOAD = 0 And dRow.UPLOAD = 0 And dRow.DOWNLIM = 0 And dRow.UPLIM = 0 Then Continue For
              If Not bFreedom And (dRow.DOWNLOAD = 0 And dRow.UPLOAD = 0 And dRow.DOWNLIM = 150000 And dRow.UPLIM = 150000) Then Continue For
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
