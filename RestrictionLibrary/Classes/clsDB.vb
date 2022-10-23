Imports System.Collections.Generic
Imports System.Xml
''' <summary>
''' Stores information about usage activity for an account, with access to subroutines and functions for manipulating the list and storing it as one of three file types.
''' </summary>
''' <remarks>This class also contains a child <see cref="DataBase.DataRow" /> class which provides access to individual events of this larger list and is used as a Type in some instances.</remarks>
Public Class DataBase
  Implements IDictionary(Of UInt64, DataRow)
  ''' <summary>
  ''' Stores information about usage activity at a single date and time.
  ''' </summary>
  Public Structure DataRow
    ''' <summary>
    ''' The Date and Time of the data stored in this row.
    ''' </summary>
    Public DATETIME As Date
    ''' <summary>
    ''' The number of Megabytes used in download activity at the specified date and time.
    ''' </summary>
    Public DOWNLOAD As Long
    ''' <summary>
    ''' The maximum number of Megabytes allowed in download activity for the plan at the specified date and time.
    ''' </summary>
    Public DOWNLIM As Long
    ''' <summary>
    ''' The number of Megabytes used in upload activity at the specified date and time.
    ''' </summary>
    Public UPLOAD As Long
    ''' <summary>
    ''' The maximum number of Megabytes allowed in upload activity for the plan at the specified date and time.
    ''' </summary>
    Public UPLIM As Long
    ''' <summary>
    ''' Create a new DataRow entry.
    ''' </summary>
    ''' <param name="dTime">The Date and Time of the entry.</param>
    ''' <param name="lDown">The number of Megabytes used in download activity for this entry.</param>
    ''' <param name="lDownLim">The maximum number of Megabytes allowed in download activity for this entry.</param>
    ''' <param name="lUp">The number of Megabytes used in upload activity for this entry.</param>
    ''' <param name="lUpLim">The maximum number of Megabytes allowed in upload activity for this entry.</param>
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
    Public ReadOnly Property sDATETIME As String
      Get
        Return DATETIME.ToString("g")
      End Get
    End Property
    ''' <summary>
    ''' Get a string representation of the number of Megabytes used in download activity for this row.
    ''' </summary>
    ''' <returns>The download value is returned with standard thousands separators and no decimal.</returns>
    Public ReadOnly Property sDOWNLOAD As String
      Get
        Return DOWNLOAD.ToString("N0")
      End Get
    End Property
    ''' <summary>
    ''' Get a string representation of the number of Megabytes allowed in download activity for this row.
    ''' </summary>
    ''' <returns>The download limit is returned with standard thousands separators and no decimal.</returns>
    Public ReadOnly Property sDOWNLIM As String
      Get
        Return DOWNLIM.ToString("N0")
      End Get
    End Property
    ''' <summary>
    ''' Get a string representation of the number of Megabytes used in upload activity for this row.
    ''' </summary>
    ''' <returns>The upload value is returned with standard thousands separators and no decimal.</returns>
    Public ReadOnly Property sUPLOAD As String
      Get
        Return UPLOAD.ToString("N0")
      End Get
    End Property
    ''' <summary>
    ''' Get a string representation of the number of Megabytes allowed in upload activity for this row.
    ''' </summary>
    ''' <returns>The upload limit is returned with standard thousands separators and no decimal.</returns>
    Public ReadOnly Property sUPLIM As String
      Get
        Return UPLIM.ToString("N0")
      End Get
    End Property
    ''' <summary>
    ''' Get a string representation of this row in the format "MM/DD/YYYY HH:MM [Down: 0,000/0,000][, ][Up: 0,000/0,000]".
    ''' </summary>
    ''' <returns>The return value will always contain the <see cref="sDATETIME" /> value, and will be followed with Download and/or Upload values if either the value or limit is greater than 0.</returns>
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
    Public Shared Function IsEmpty(dRow As DataRow) As Boolean
      Return (dRow.DATETIME.ToBinary = 0 And dRow.DOWNLOAD = 0 And dRow.UPLOAD = 0 And dRow.DOWNLIM = 0 And dRow.UPLIM = 0)
    End Function
    ''' <summary>
    ''' Check this <see cref="DataRow" /> entry to see if it is empty or unset.
    ''' </summary>
    ''' <returns>A boolean value of <c>True</c> is returned if this entry is empty, <c>False</c> otherwise.</returns>
    Public Function IsEmpty() As Boolean
      Return (DATETIME.ToBinary = 0 And DOWNLOAD = 0 And UPLOAD = 0 And DOWNLIM = 0 And UPLIM = 0)
    End Function
  End Structure
  Private data As SortedDictionary(Of UInt64, DataRow)
  ''' <summary>
  ''' Create a new DataBase with no entries.
  ''' </summary>
  ''' <remarks>If this subroutine is called on an existing database, it will be erased.</remarks>
  Public Sub New()
    If data Is Nothing Then Return
    data.Clear()
    data = Nothing
  End Sub
  ''' <summary>
  ''' Set this value to true to stop loading a databased called from the initialization subroutine.
  ''' </summary>
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
    Public Sub New(Value As Integer, Total As Integer)
      m_Value = Value
      m_Total = Total
    End Sub
    ''' <summary>
    ''' The Current number of things that have been done.
    ''' </summary>
    Public ReadOnly Property Value As Integer
      Get
        Return m_Value
      End Get
    End Property
    ''' <summary>
    ''' The total number of things to do.
    ''' </summary>
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
  Public Event ProgressState As EventHandler(Of ProgressStateEventArgs)
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
  Public Sub StartNew()
    StopNew = False
    If IO.File.Exists(sPath) Then
      Try
        If LCase(IO.Path.GetExtension(sPath)).CompareTo(".xml") = 0 Then
          Dim m_xmld As New XmlDocument
          m_xmld.Load(sPath)
          Dim m_nodelist As XmlNodeList = m_xmld.ChildNodes(1).ChildNodes
          Dim I As Integer = 0
          Dim iMax As Integer = m_nodelist.Count
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
            If data Is Nothing Then data = New SortedDictionary(Of UInt64, DataRow)
            Add(New DataRow(DT, Down, DownLim, Up, UpLim))
            If StopNew Then Return
          Next
          m_xmld = Nothing
        ElseIf LCase(IO.Path.GetExtension(sPath)).CompareTo(".wb") = 0 Then
          Using nRead As New IO.FileStream(sPath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
            Using nIn As New IO.BinaryReader(nRead)
              Dim uRows As UInt64 = LOAD_ReadULong(nIn)
              For I As UInt64 = 1 To uRows
                If bWithDisplay Then RaiseEvent ProgressState(Me, New ProgressStateEventArgs(I, uRows))
                Dim DT As Date = LOAD_ReadDate(nIn)
                Dim Down As Long = LOAD_ReadLong(nIn)
                Dim DownLim As Long = LOAD_ReadLong(nIn)
                Dim Up As Long = LOAD_ReadLong(nIn)
                Dim UpLim As Long = LOAD_ReadLong(nIn)
                If data Is Nothing Then data = New SortedDictionary(Of UInt64, DataRow)
                Add(New DataRow(DT, Down, DownLim, Up, UpLim))
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
                If data Is Nothing Then data = New SortedDictionary(Of UInt64, DataRow)
                Add(New DataRow(DT, Down, DownLim, Up, UpLim))
              End If
              Do Until nIn.EndOfStream
                Dim rowData() As String = Split(nIn.ReadLine, ",")
                Dim DT As Date = Date.Parse(rowData(0))
                Dim Down As Long = rowData(1)
                Dim DownLim As Long = rowData(2)
                Dim Up As Long = rowData(3)
                Dim UpLim As Long = rowData(4)
                If data Is Nothing Then data = New SortedDictionary(Of UInt64, DataRow)
                Add(New DataRow(DT, Down, DownLim, Up, UpLim))
                If StopNew Then Return
              Loop
              nIn.Close()
            End Using
          End Using
        End If
      Catch ex As Exception
        If data Is Nothing Then Return
        data.Clear()
        data = Nothing
      End Try
    End If
  End Sub
  ''' <summary>
  ''' Add a new DataRow entry to the DataBase.
  ''' </summary>
  ''' <param name="item">The new DataRow entry to add.</param>
  ''' <remarks>The entry won't be added if it's less than one minute apart from another DataRow in the DataBase.</remarks>
  Public Sub Add(item As DataRow)
    Dim key As UInt64 = Math.Floor(item.DATETIME.Ticks / 600000000)
    Add(key, item)
  End Sub
  Public Sub Add(item As KeyValuePair(Of UInt64, DataRow)) Implements System.Collections.Generic.ICollection(Of KeyValuePair(Of UInt64, DataRow)).Add
    Add(item.Key, item.Value)
  End Sub
  Public Sub Add(id As UInt64, item As DataRow) Implements System.Collections.Generic.IDictionary(Of UInt64, DataRow).Add
    If data Is Nothing Then data = New SortedDictionary(Of UInt64, DataRow)
    If Not id = Math.Floor(item.DATETIME.Ticks / 600000000) Then id = Math.Floor(item.DATETIME.Ticks / 600000000)
    If data.ContainsKey(id) Then Return
    data.Add(id, item)
  End Sub
  ''' <summary>
  ''' Add all the entries of another DataBase into this one.
  ''' </summary>
  ''' <param name="db">The other DataBase to include in this one.</param>
  ''' <param name="withDisplay">Return the current progress of the merge through the ProgressState event.</param>
  ''' <remarks>The original DataBase will not be modified or erased by this process.</remarks>
  Public Sub Merge(db As DataBase, withDisplay As Boolean)
    Dim dbVals() As DataRow = db.Values
    For I As Integer = 0 To dbVals.Length - 1
      If withDisplay Then RaiseEvent ProgressState(Me, New ProgressStateEventArgs(I + 1, dbVals.Length))
      Add(dbVals(I))
    Next
  End Sub
  ''' <summary>
  ''' Remove all entries from this DataBase.
  ''' </summary>
  ''' <remarks>This is literally identical to the initialization subroutine without parameters.</remarks>
  Public Sub Clear() Implements System.Collections.Generic.IDictionary(Of UInt64, DataRow).Clear
    If data Is Nothing Then Return
    data.Clear()
    data = Nothing
  End Sub
  ''' <summary>
  ''' Check for existence of an item by searching for an exact match. 
  ''' </summary>
  ''' <param name="item">The exact match of the KeyValuePair(Of UInt64, DataRow)) you wish to find.</param>
  ''' <returns><c>True</c> is returned if the DataBase contains at least one copy of the <paramref name="item" />, <c>False</c> otherwise.</returns>
  Public Function Contains(item As KeyValuePair(Of UInt64, DataRow)) As Boolean Implements System.Collections.Generic.IDictionary(Of UInt64, DataRow).Contains
    If Not data.ContainsKey(item.Key) Then Return False
    If Not data(item.Key).DATETIME = item.Value.DATETIME Then Return False
    If Not data(item.Key).DOWNLOAD = item.Value.DOWNLOAD Then Return False
    If Not data(item.Key).DOWNLIM = item.Value.DOWNLIM Then Return False
    If Not data(item.Key).UPLOAD = item.Value.UPLOAD Then Return False
    If Not data(item.Key).UPLIM = item.Value.UPLIM Then Return False
    Return True
  End Function
  Public Function ContainsKey(key As UInt64) As Boolean Implements System.Collections.Generic.IDictionary(Of UInt64, DataRow).ContainsKey
    Return data.ContainsKey(key)
  End Function
  ''' <summary>
  ''' Copy data from this DataBase into an array of DataRows.
  ''' </summary>
  ''' <param name="array">The array of <see cref="DataRow" />s in which to copy this DataBase.</param>
  ''' <param name="arrayIndex">The location at which to start copying data into in the <paramref name="array" />.</param>
  ''' <remarks>
  ''' The number of items to be copied will be equal to the space allocated for items in the <paramref name="array" /> from the <paramref name="arrayIndex" /> value to the end.
  ''' </remarks>
  Public Sub CopyTo(array() As KeyValuePair(Of UInt64, DataRow), arrayIndex As Integer) Implements System.Collections.Generic.IDictionary(Of UInt64, DataRow).CopyTo
    Dim aData(data.Count - 1) As KeyValuePair(Of UInt64, DataRow)
    Dim I As Integer = 0
    For Each aItem As UInt64 In data.Keys
      aData(I) = New KeyValuePair(Of UInt64, DataRow)(aItem, data(aItem))
      I += 1
    Next
    System.Array.Copy(aData, 0, array, arrayIndex, array.Length - arrayIndex)
  End Sub
  ''' <summary>
  ''' Convert this DataBase into an array of <see cref="DataRow" /> entries.
  ''' </summary>
  ''' <returns>The full list of entries in this DataBase.</returns>
  Public Function ToArray() As DataRow()
    Dim aData(data.Count - 1) As DataRow
    Dim I As Integer = 0
    For Each aItem As UInt64 In data.Keys
      aData(I) = data(aItem)
      I += 1
    Next
    Return aData
  End Function
  ''' <summary>
  ''' The total number of entries in this DataBase.
  ''' </summary>
  ''' <remarks>The value will be 0 if the DataBase is not initialized or if it's been erased.</remarks>
  Public ReadOnly Property Count As Integer Implements System.Collections.Generic.IDictionary(Of UInt64, DataRow).Count
    Get
      If data Is Nothing Then Return 0
      Return data.Count
    End Get
  End Property
  ''' <summary>
  ''' Gets a value indicating whether the DataBase is read-only.
  ''' </summary>
  ''' <value></value>
  ''' <returns>This property is always false because it is always false for all arrays.</returns>
  Public ReadOnly Property IsReadOnly As Boolean Implements System.Collections.Generic.IDictionary(Of UInt64, DataRow).IsReadOnly
    Get
      Return False
    End Get
  End Property
  Default Public Property Item(key As UInt64) As DataRow Implements IDictionary(Of UInt64, DataRow).Item
    Get
      Return data(key)
    End Get
    Set(value As DataRow)
      data(key) = value
    End Set
  End Property
  Public ReadOnly Property Keys As ICollection(Of UInt64) Implements IDictionary(Of UInt64, DataRow).Keys
    Get
      Return data.Keys
    End Get
  End Property
  Public ReadOnly Property Values As ICollection(Of DataRow) Implements IDictionary(Of UInt64, DataRow).Values
    Get
      Return data.Values
    End Get
  End Property
  ''' <summary>
  ''' Remove a DataRow entry from the DataBase.
  ''' </summary>
  ''' <param name="DateTime">The Date and Time of the DataRow entry to remove.</param>
  ''' <returns><c>True</c> if the entry was successfully removed from the DataBase, <c>False</c> if the entry could not be found.</returns>
  Public Function Remove(DateTime As Date) As Boolean
    Dim key As UInt64 = Math.Floor(DateTime.Ticks / 600000000)
    Return Remove(key)
  End Function
  Public Function Remove(item As KeyValuePair(Of UInt64, DataRow)) As Boolean Implements System.Collections.Generic.ICollection(Of KeyValuePair(Of UInt64, DataRow)).Remove
    Return Remove(item.Key)
  End Function
  ''' <summary>
  ''' Remove a DataRow entry from the DataBase.
  ''' </summary>
  ''' <param name="item">The DataRow entry to remove.</param>
  ''' <returns><c>True</c> if the entry was successfully removed from the DataBase, <c>False</c> if the entry could not be found.</returns>
  ''' <remarks>This function only uses the <see cref="DataRow.DATETIME" /> value to compare against entries in the DataBase.</remarks>
  Public Function Remove(item As UInt64) As Boolean Implements System.Collections.Generic.IDictionary(Of UInt64, DataRow).Remove
    If Not data.ContainsKey(item) Then Return False
    Return data.Remove(item)
  End Function
  ''' <summary>
  ''' Returns an enumerator that iterates through the DataBase.
  ''' </summary>
  ''' <returns>A <see cref="System.Collections.Generic.IEnumerable(Of KeyValuePair(Of UInt64, DataRow))" /> that can be used to iterate through the DataBase.</returns>
  Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of KeyValuePair(Of UInt64, DataRow)) Implements System.Collections.Generic.IEnumerable(Of KeyValuePair(Of UInt64, DataRow)).GetEnumerator
    If data Is Nothing Then Return Nothing
    Return DirectCast(data, IEnumerable(Of KeyValuePair(Of UInt64, DataRow))).GetEnumerator
  End Function
  ''' <summary>
  ''' Returns an enumerator that iterates through the DataBase.
  ''' </summary>
  ''' <returns>A <see cref="System.Collections.Generic.IEnumerator" /> that can be used to iterate through the DataBase.</returns>
  Public Function GetEnumerator1() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
    If data Is Nothing Then Return Nothing
    Return DirectCast(data, IEnumerable(Of KeyValuePair(Of UInt64, DataRow))).GetEnumerator
  End Function
  ''' <summary>
  ''' Returns the latest entry in the DataBase.
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks>This function will sort the DataBase before returning a value.</remarks>
  Public Function GetLast() As DataRow
    Dim dbVals() As DataRow = ToArray()
    Return dbVals(dbVals.Length - 1)
  End Function
  ''' <summary>
  ''' A full XML representation of the DataBase.
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks>This function will sort the DataBase before returning a value.</remarks>
  Public Overrides Function ToString() As String
    Dim sDB As String = "<?xml version=""1.0"" standalone=""yes""?>" & vbNewLine &
                        "<RestrictionTrackerUsage>" & vbNewLine
    For Each dRow As KeyValuePair(Of UInt64, DataRow) In data
      sDB &= "  <History>" & vbNewLine &
             "    <DATETIME>" & dRow.Value.DATETIME.ToString("o") & "</DATETIME>" & vbNewLine &
             "    <DOWNLOAD>" & dRow.Value.DOWNLOAD.ToString & "</DOWNLOAD>" & vbNewLine &
             "    <DOWNLIM>" & dRow.Value.DOWNLIM.ToString & "</DOWNLIM>" & vbNewLine &
             "    <UPLOAD>" & dRow.Value.UPLOAD.ToString & "</UPLOAD>" & vbNewLine &
             "    <UPLIM>" & dRow.Value.UPLIM.ToString & "</UPLIM>" & vbNewLine &
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
    Dim uLen As UInt64 = CULng(data.Count)
    Dim I As Integer = 0
    For Each dRow As KeyValuePair(Of UInt64, DataRow) In data
      If withDisplay Then RaiseEvent ProgressState(Me, New ProgressStateEventArgs(I + 1UL, uLen))
      sDB &= "  <History>" & vbNewLine &
             "    <DATETIME>" & dRow.Value.DATETIME.ToString("o") & "</DATETIME>" & vbNewLine &
             "    <DOWNLOAD>" & dRow.Value.DOWNLOAD.ToString & "</DOWNLOAD>" & vbNewLine &
             "    <DOWNLIM>" & dRow.Value.DOWNLIM.ToString & "</DOWNLIM>" & vbNewLine &
             "    <UPLOAD>" & dRow.Value.UPLOAD.ToString & "</UPLOAD>" & vbNewLine &
             "    <UPLIM>" & dRow.Value.UPLIM.ToString & "</UPLIM>" & vbNewLine &
             "  </History>" & vbNewLine
      I += 1
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
  Public Function Save(Path As String, withDisplay As Boolean) As Boolean
    If data Is Nothing Then Return False
    Dim bDelBack As Boolean = False
    If My.Computer.FileSystem.FileExists(Path) Then
      My.Computer.FileSystem.RenameFile(Path, IO.Path.GetFileName(Path) & ".bak")
      bDelBack = True
    End If
    Dim dVals() As DataRow = ToArray()
    Dim bFreedom As Boolean = True
    Dim sample As UInt64 = CULng(dVals.Length) - 1
    If sample > 15 Then sample = 15
    For I As UInt64 = 0 To sample
      Dim dRow As DataRow = dVals(I)
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
            Dim uData As UInt64 = CULng(dVals.Length)
            For I As UInt64 = 0 To uData - 1
              Dim dRow As DataRow = dVals(I)
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
            Dim uData As UInt64 = CULng(dVals.Length)
            Dim truData As UInt64 = 0
            For I As UInt64 = 0 To uData - 1
              Dim dRow As DataRow = dVals(I)
              If dRow.DOWNLOAD = 0 And dRow.UPLOAD = 0 And dRow.DOWNLIM = 0 And dRow.UPLIM = 0 Then Continue For
              If Not bFreedom And (dRow.DOWNLOAD = 0 And dRow.UPLOAD = 0 And dRow.DOWNLIM = 150000 And dRow.UPLIM = 150000) Then Continue For
              truData += 1
            Next
            SAVE_Write(nOut, truData)
            For I As UInt64 = 0 To uData - 1
              Dim dRow As DataRow = dVals(I)
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
            Dim uData As UInt64 = CULng(dVals.Length)
            nOut.WriteLine("Time,Download,Download Limit,Upload,Upload Limit")
            For I As UInt64 = 0 To uData - 1
              Dim dRow As DataRow = dVals(I)
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
  Private Sub SAVE_Write(ByRef nOut As IO.BinaryWriter, value As UInt64)
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
  Private Function LOAD_ReadULong(ByRef nIn As IO.BinaryReader) As UInt64
    Return nIn.ReadInt64
  End Function
  Public Function TryGetValue(key As UInt64, ByRef value As DataRow) As Boolean Implements IDictionary(Of UInt64, DataRow).TryGetValue
    Return data.TryGetValue(key, value)
  End Function
#End Region
End Class
