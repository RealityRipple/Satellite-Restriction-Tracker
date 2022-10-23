Imports System.Collections.Generic
Imports System.Xml
''' <summary>
''' Provides data for the <see cref="RestrictionLibrary.DataBase.ProgressState" /> event.
''' </summary>
''' <remarks></remarks>
Public Class DataBaseProgressEventArgs
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
''' Stores information about usage activity at a single date and time.
''' </summary>
Public Structure DataRow
  Private mDT As Date
  Private mU As Long
  Private mL As Long
  ''' <summary>
  ''' The Date and Time of the data stored in this row.
  ''' </summary>
  Public Property DATETIME As Date
    Get
      Return mDT
    End Get
    Set(value As Date)
      mDT = value
    End Set
  End Property
  ''' <summary>
  ''' The number of Megabytes used at the specified date and time.
  ''' </summary>
  Public Property USED As Long
    Get
      Return mU
    End Get
    Set(value As Long)
      mU = value
    End Set
  End Property
  ''' <summary>
  ''' The maximum number of Megabytes allowed for the plan at the specified date and time.
  ''' </summary>
  Public Property LIMIT As Long
    Get
      Return mL
    End Get
    Set(value As Long)
      mL = value
    End Set
  End Property
  ''' <summary>
  ''' Create a new DataRow entry.
  ''' </summary>
  ''' <param name="dTime">The Date and Time of the entry.</param>
  ''' <param name="lUsed">The number of Megabytes used for this entry.</param>
  ''' <param name="lLimit">The maximum number of Megabytes allowed for this entry.</param>
  Public Sub New(dTime As Date, lUsed As Long, lLimit As Long)
    mDT = dTime
    mU = lUsed
    mL = lLimit
  End Sub
  ''' <summary>
  ''' Get a string representation of the Date and Time of the data stored in this row.
  ''' </summary>
  ''' <returns>The date and time are returned in the format "MM/DD/YYYY HH:MM", or the standard "g".</returns>
  Public ReadOnly Property sDATETIME As String
    Get
      Return srlFunctions.TimeToString(mDT)
    End Get
  End Property
  ''' <summary>
  ''' Get a string representation of the number of Megabytes used for this row.
  ''' </summary>
  ''' <returns>The value is returned with standard thousands separators and no decimal.</returns>
  Public ReadOnly Property sUSED As String
    Get
      Return mU.ToString("N0", Globalization.CultureInfo.InvariantCulture)
    End Get
  End Property
  ''' <summary>
  ''' Get a string representation of the number of Megabytes allowed for this row.
  ''' </summary>
  ''' <returns>The limit is returned with standard thousands separators and no decimal.</returns>
  Public ReadOnly Property sLIMIT As String
    Get
      Return mL.ToString("N0", Globalization.CultureInfo.InvariantCulture)
    End Get
  End Property
  ''' <summary>
  ''' Get a string representation of this row in the format "MM/DD/YYYY HH:MM [0,000/0,000]".
  ''' </summary>
  ''' <returns>The return value will always contain the <see cref="sDATETIME" /> value, and will be followed with usage if either the value or limit is greater than 0.</returns>
  Public Overrides Function ToString() As String
    Dim sRet As String = sDATETIME
    If mU > 0 Or mL > 0 Then sRet &= " " & sUSED & "/" & sLIMIT
    Return sRet
  End Function
  ''' <summary>
  ''' This is a standard empty DataRow with all values set to zero.
  ''' </summary>
  ''' <returns></returns>
  Public Shared ReadOnly Property Empty() As DataRow
    Get
      Return New DataRow(Date.FromBinary(0), 0, 0)
    End Get
  End Property
  ''' <summary>
  ''' Check a <see cref="DataRow" /> entry to see if it is empty or unset.
  ''' </summary>
  ''' <param name="dRow">The <see cref="DataRow" /> entry you wish to check.</param>
  ''' <returns>A boolean value of <c>True</c> is returned if the <paramref name="dRow" /> entry is empty, <c>False</c> otherwise.</returns>
  Public Shared Function IsEmpty(dRow As DataRow) As Boolean
    Return (dRow.DATETIME.ToBinary = 0 And dRow.USED = 0 And dRow.LIMIT = 0)
  End Function
  ''' <summary>
  ''' Check this <see cref="DataRow" /> entry to see if it is empty or unset.
  ''' </summary>
  ''' <returns>A boolean value of <c>True</c> is returned if this entry is empty, <c>False</c> otherwise.</returns>
  Public Function IsEmpty() As Boolean
    Return (mDT.ToBinary = 0 And mU = 0 And mL = 0)
  End Function
  Public Overrides Function GetHashCode() As Integer
    Return mDT.GetHashCode
  End Function
  Public Overrides Function Equals(obj As Object) As Boolean
    If Not TypeOf obj Is DataRow Then Return False
    Return mDT.ToBinary = obj.ToBinary
  End Function
  Public Shared Operator =(objA As DataRow, objB As DataRow) As Boolean
    Return objA.DATETIME.ToBinary = objB.DATETIME.ToBinary
  End Operator
  Public Shared Operator <>(objA As DataRow, objB As DataRow) As Boolean
    Return Not objA.DATETIME.ToBinary = objB.DATETIME.ToBinary
  End Operator
End Structure
''' <summary>
''' Stores information about usage activity for an account, with access to subroutines and functions for manipulating the list and storing it as one of three file types.
''' </summary>
''' <remarks>This class also contains a child <see cref="DataRow" /> class which provides access to individual events of this larger list and is used as a Type in some instances.</remarks>
Public Class DataBase
  Implements IDictionary(Of UInt64, DataRow)
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
  Private mStopNew As Boolean
  ''' <summary>
  ''' Stop loading a databased called from the initialization subroutine.
  ''' </summary>
  Public Sub StopNew()
    mStopNew = True
  End Sub
  Private sPath As String
  Private bWithDisplay As Boolean
  ''' <summary>
  ''' The loading of the DataBase has progressed to a specific point.
  ''' </summary>
  ''' <param name="sender">A reference to this <see cref="DataBase" /> class.</param>
  ''' <param name="e">A <see cref="DataBaseProgressEventArgs" /> entry with the current Value and Total of the DataBase load's progress.</param>
  Public Event ProgressState As EventHandler(Of DataBaseProgressEventArgs)
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
    mStopNew = False
    If IO.File.Exists(sPath) Then
      Try
        If IO.Path.GetExtension(sPath).ToUpperInvariant.CompareTo(".XML") = 0 Then
          Dim m_xmld As New XmlDocument
          m_xmld.Load(sPath)
          Dim m_nodelist As XmlNodeList = m_xmld.ChildNodes(1).ChildNodes
          Dim I As Integer = 0
          Dim iMax As Integer = m_nodelist.Count
          For Each m_node As XmlNode In m_nodelist
            I += 1
            If bWithDisplay Then RaiseEvent ProgressState(Me, New DataBaseProgressEventArgs(I, iMax))
            Dim sDT, sU, sL As String : sDT = "0" : sU = "0" : sL = "0"
            For Each m_child As XmlNode In m_node.ChildNodes
              Select Case m_child.Name
                Case "DATETIME" : sDT = m_child.InnerText
                Case "DOWNLOAD" : sU = m_child.InnerText
                Case "DOWNLIM" : sL = m_child.InnerText
                Case "USED" : sU = m_child.InnerText
                Case "LIMIT" : sL = m_child.InnerText
              End Select
            Next
            Dim DT As Date = Xml.XmlConvert.ToDateTime(sDT, Xml.XmlDateTimeSerializationMode.RoundtripKind)
            Dim Used As Long = Long.Parse(sU, Globalization.CultureInfo.InvariantCulture)
            Dim Lim As Long = Long.Parse(sL, Globalization.CultureInfo.InvariantCulture)
            If data Is Nothing Then data = New SortedDictionary(Of UInt64, DataRow)
            Add(New DataRow(DT, Used, Lim))
            If mStopNew Then Return
          Next
          m_xmld = Nothing
        ElseIf IO.Path.GetExtension(sPath).ToUpperInvariant.CompareTo(".WB") = 0 Then
          Using nRead As New IO.FileStream(sPath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
            Dim nIn As New IO.BinaryReader(nRead)
            Dim uRows As UInt64 = LOAD_ReadULong(nIn)
            For I As UInt64 = 1 To uRows
              If bWithDisplay Then RaiseEvent ProgressState(Me, New DataBaseProgressEventArgs(I, uRows))
              Dim DT As Date = LOAD_ReadDate(nIn)
              Dim Down As Long = LOAD_ReadLong(nIn)
              Dim DownLim As Long = LOAD_ReadLong(nIn)
              LOAD_ReadLong(nIn)
              LOAD_ReadLong(nIn)
              If data Is Nothing Then data = New SortedDictionary(Of UInt64, DataRow)
              Add(New DataRow(DT, Down, DownLim))
              If mStopNew Then Return
            Next
          End Using
        ElseIf IO.Path.GetExtension(sPath).ToUpperInvariant.CompareTo(".CSV") = 0 Then
          Using nRead As New IO.FileStream(sPath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
            Dim nIn As New IO.StreamReader(nRead)
            Dim firstLine As String = nIn.ReadLine
            If String.Compare(firstLine, "Time,Download,Download Limit,Upload,Upload Limit", StringComparison.OrdinalIgnoreCase) = 0 Then
            ElseIf String.Compare(firstLine, "Time,Usage,Limit", StringComparison.OrdinalIgnoreCase) = 0 Then
            Else
              Dim firstData() As String = Split(firstLine, ",")
              Dim DT As Date = Date.Parse(firstData(0), Globalization.CultureInfo.InvariantCulture)
              Dim Used As Long = firstData(1)
              Dim Lim As Long = firstData(2)
              If data Is Nothing Then data = New SortedDictionary(Of UInt64, DataRow)
              Add(New DataRow(DT, Used, Lim))
            End If
            Do Until nIn.EndOfStream
              Dim rowData() As String = Split(nIn.ReadLine, ",")
              Dim DT As Date = Date.Parse(rowData(0), Globalization.CultureInfo.InvariantCulture)
              Dim Used As Long = rowData(1)
              Dim Lim As Long = rowData(2)
              If data Is Nothing Then data = New SortedDictionary(Of UInt64, DataRow)
              Add(New DataRow(DT, Used, Lim))
              If mStopNew Then Return
            Loop
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
      If withDisplay Then RaiseEvent ProgressState(Me, New DataBaseProgressEventArgs(I + 1, dbVals.Length))
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
    If Not data(item.Key).USED = item.Value.USED Then Return False
    If Not data(item.Key).LIMIT = item.Value.LIMIT Then Return False
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
  Public ReadOnly Property LastRow() As DataRow
    Get
      Dim dbVals() As DataRow = ToArray()
      Return dbVals(dbVals.Length - 1)
    End Get
  End Property
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
             "    <DATETIME>" & dRow.Value.DATETIME.ToString("o", Globalization.CultureInfo.InvariantCulture) & "</DATETIME>" & vbNewLine &
             "    <USED>" & dRow.Value.USED.ToString(Globalization.CultureInfo.InvariantCulture) & "</USED>" & vbNewLine &
             "    <LIMIT>" & dRow.Value.LIMIT.ToString(Globalization.CultureInfo.InvariantCulture) & "</LIMIT>" & vbNewLine &
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
      If withDisplay Then RaiseEvent ProgressState(Me, New DataBaseProgressEventArgs(I + 1UL, uLen))
      sDB &= "  <History>" & vbNewLine &
             "    <DATETIME>" & dRow.Value.DATETIME.ToString("o", Globalization.CultureInfo.InvariantCulture) & "</DATETIME>" & vbNewLine &
             "    <USED>" & dRow.Value.USED.ToString(Globalization.CultureInfo.InvariantCulture) & "</USED>" & vbNewLine &
             "    <LIMIT>" & dRow.Value.LIMIT.ToString(Globalization.CultureInfo.InvariantCulture) & "</LIMIT>" & vbNewLine &
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
      If Not dRow.LIMIT = 150000 Then
        bFreedom = False
        Exit For
      End If
    Next
    Try
      If IO.Path.GetExtension(Path).ToUpperInvariant.CompareTo(".XML") = 0 Then
        Using nWrite As New IO.FileStream(Path, IO.FileMode.Create, IO.FileAccess.ReadWrite, IO.FileShare.None)
          Dim nOut As New IO.StreamWriter(nWrite)
          nOut.WriteLine("<?xml version=""1.0"" standalone=""yes""?>")
          nOut.WriteLine("<RestrictionTrackerUsage>")
          Dim uData As UInt64 = CULng(dVals.Length)
          For I As UInt64 = 0 To uData - 1
            Dim dRow As DataRow = dVals(I)
            If withDisplay Then RaiseEvent ProgressState(Me, New DataBaseProgressEventArgs(I + 1UL, uData))
            If dRow.USED = 0 And dRow.LIMIT = 0 Then Continue For
            If Not bFreedom And (dRow.USED = 0 And dRow.LIMIT = 150000) Then Continue For
            nOut.WriteLine("  <History>")
            nOut.WriteLine("    <DATETIME>" & dRow.DATETIME.ToString("o", Globalization.CultureInfo.InvariantCulture) & "</DATETIME>")
            nOut.WriteLine("    <USED>" & dRow.USED.ToString(Globalization.CultureInfo.InvariantCulture) & "</USED>")
            nOut.WriteLine("    <LIMIT>" & dRow.LIMIT.ToString(Globalization.CultureInfo.InvariantCulture) & "</LIMIT>")
            nOut.WriteLine("  </History>")
          Next
          nOut.Write("</RestrictionTrackerUsage>")
        End Using
      ElseIf IO.Path.GetExtension(Path).ToUpperInvariant.CompareTo(".WB") = 0 Then
        Using nWrite As New IO.FileStream(Path, IO.FileMode.Create, IO.FileAccess.ReadWrite, IO.FileShare.None)
          Dim nOut As New IO.BinaryWriter(nWrite)
          Dim uData As UInt64 = CULng(dVals.Length)
          Dim truData As UInt64 = 0
          For I As UInt64 = 0 To uData - 1
            Dim dRow As DataRow = dVals(I)
            If dRow.USED = 0 And dRow.LIMIT = 0 Then Continue For
            If Not bFreedom And (dRow.USED = 0 And dRow.LIMIT = 150000) Then Continue For
            truData += 1
          Next
          SAVE_Write(nOut, truData)
          For I As UInt64 = 0 To uData - 1
            Dim dRow As DataRow = dVals(I)
            If withDisplay Then RaiseEvent ProgressState(Me, New DataBaseProgressEventArgs(I + 1UL, uData))
            If dRow.USED = 0 And dRow.LIMIT = 0 Then Continue For
            If Not bFreedom And (dRow.USED = 0 And dRow.LIMIT = 150000) Then Continue For
            SAVE_Write(nOut, dRow.DATETIME)
            SAVE_Write(nOut, dRow.USED)
            SAVE_Write(nOut, dRow.LIMIT)
            SAVE_Write(nOut, 0L)
            SAVE_Write(nOut, 0L)
          Next
        End Using
      ElseIf IO.Path.GetExtension(Path).ToUpperInvariant.CompareTo(".CSV") = 0 Then
        Using nWrite As New IO.FileStream(Path, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite, IO.FileShare.None)
          Dim nOut As New IO.StreamWriter(nWrite)
          Dim uData As UInt64 = CULng(dVals.Length)
          nOut.WriteLine("Time,Download,Download Limit,Upload,Upload Limit")
          For I As UInt64 = 0 To uData - 1
            Dim dRow As DataRow = dVals(I)
            If withDisplay Then RaiseEvent ProgressState(Me, New DataBaseProgressEventArgs(I + 1UL, uData))
            If dRow.USED = 0 And dRow.LIMIT = 0 Then Continue For
            If Not bFreedom And (dRow.USED = 0 And dRow.LIMIT = 150000) Then Continue For
            nOut.WriteLine(dRow.DATETIME.ToString("o", Globalization.CultureInfo.InvariantCulture) & "," & dRow.USED & "," & dRow.LIMIT)
          Next
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
  Private Shared Sub SAVE_Write(ByRef nOut As IO.BinaryWriter, value As Long)
    nOut.Write(BitConverter.GetBytes(value), 0, 8)
  End Sub
  Private Shared Sub SAVE_Write(ByRef nOut As IO.BinaryWriter, value As UInt64)
    nOut.Write(BitConverter.GetBytes(value), 0, 8)
  End Sub
  Private Shared Sub SAVE_Write(ByRef nOut As IO.BinaryWriter, value As Date)
    nOut.Write(BitConverter.GetBytes(value.ToBinary), 0, 8)
  End Sub
  Private Shared Function LOAD_ReadDate(ByRef nIn As IO.BinaryReader) As Date
    Return Date.FromBinary(nIn.ReadInt64)
  End Function
  Private Shared Function LOAD_ReadLong(ByRef nIn As IO.BinaryReader) As Long
    Return nIn.ReadInt64
  End Function
  Private Shared Function LOAD_ReadULong(ByRef nIn As IO.BinaryReader) As UInt64
    Return nIn.ReadInt64
  End Function
  Public Function TryGetValue(key As UInt64, ByRef value As DataRow) As Boolean Implements IDictionary(Of UInt64, DataRow).TryGetValue
    Return data.TryGetValue(key, value)
  End Function
#End Region
End Class
