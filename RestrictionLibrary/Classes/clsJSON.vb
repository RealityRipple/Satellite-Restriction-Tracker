Public Enum JSONElementType
  None
  [Object]
  Array
  KeyValue
  Value
End Enum
Public Class JSONElement
  Private mType As JSONElementType
  Private mKey As String
  Public Sub New()
    mType = JSONElementType.None
    mKey = Nothing
  End Sub
  Public Sub New(eType As JSONElementType)
    mType = eType
    mKey = Nothing
  End Sub
  Public Sub New(eType As JSONElementType, eKey As String)
    mType = eType
    mKey = DecodeString(eKey)
  End Sub
  Public Shared ReadOnly Property Empty As JSONElement
    Get
      Return New JSONElement()
    End Get
  End Property
  Public ReadOnly Property Type As JSONElementType
    Get
      Return mType
    End Get
  End Property
  Public ReadOnly Property Key As String
    Get
      Return mKey
    End Get
  End Property
  Public Shared Function FromObject([in] As Object) As JSONElement
    If [in] Is Nothing Then Return New JSONNull
    If IsArray([in]) Then
      Dim aIn As Object() = [in]
      Return New JSONArray(aIn)
    End If
    If [in].GetType Is GetType(Boolean) Then
      Dim aIn As Boolean = CBool([in])
      Return New JSONBoolean(aIn)
    End If
    If [in].GetType Is GetType(SByte) Or
       [in].GetType Is GetType(Byte) Or
       [in].GetType Is GetType(Int16) Or
       [in].GetType Is GetType(UInt16) Or
       [in].GetType Is GetType(Int32) Or
       [in].GetType Is GetType(UInt32) Or
       [in].GetType Is GetType(Int64) Or
       [in].GetType Is GetType(UInt64) Or
       [in].GetType Is GetType(Single) Or
       [in].GetType Is GetType(Double) Or
       [in].GetType Is GetType(Decimal) Then Return New JSONNumber([in])
    If [in].GetType Is GetType(String) Then Return New JSONString([in])
    If [in].GetType Is GetType(Dictionary(Of String, Object)) Then
      Dim aIn As Dictionary(Of String, Object) = [in]
      Return New JSONObject(aIn)
    End If
    Return New JSONString([in].ToString)
  End Function
  Public Overrides Function GetHashCode() As Integer
    Return mType.GetHashCode Xor mKey.GetHashCode
  End Function
  Public Overrides Function Equals(obj As Object) As Boolean
    If Not obj.GetType Is GetType(JSONElement) Then Return False
    Dim jObj As JSONElement = obj
    If Not mType = jObj.Type Then Return False
    If Not mKey = jObj.Key Then Return False
    Return True
  End Function
  Public Shared Operator =(objA As JSONElement, objB As JSONElement) As Boolean
    Return objA.Equals(objB)
  End Operator
  Public Shared Operator <>(objA As JSONElement, objB As JSONElement) As Boolean
    Return Not objA.Equals(objB)
  End Operator
  Protected Shared Function EncodeString(sIn As String) As String
    If sIn Is Nothing Then Return Nothing
    If sIn = "" Then Return ""
    sIn = sIn.Replace("""", "\""")
    sIn = sIn.Replace("\", "\\")
    sIn = sIn.Replace(ChrW(&H8), "\b")
    sIn = sIn.Replace(ChrW(&HC), "\f")
    sIn = sIn.Replace(ChrW(&HA), "\n")
    sIn = sIn.Replace(ChrW(&HD), "\r")
    sIn = sIn.Replace(ChrW(&H9), "\t")
    For I As Integer = &H0 To &H1F
      sIn = sIn.Replace(ChrW(I), "\u" & srlFunctions.PadHex(I, 4))
    Next
    Return sIn
  End Function
  Protected Shared Function DecodeString(sOut As String) As String
    If sOut Is Nothing Then Return Nothing
    If sOut = "" Then Return ""
    Dim tGuid As Guid = Guid.NewGuid()
    sOut = sOut.Replace("\\", "{BACKSLASH-" & tGuid.ToString("D") & "}")
    sOut = sOut.Replace("\""", """")
    sOut = sOut.Replace("\b", ChrW(&H8))
    sOut = sOut.Replace("\f", ChrW(&HC))
    sOut = sOut.Replace("\n", ChrW(&HA))
    sOut = sOut.Replace("\r", ChrW(&HD))
    sOut = sOut.Replace("\t", ChrW(&H9))
    For I As Integer = &H0 To &H1F
      sOut = sOut.Replace("\u" & srlFunctions.PadHex(I, 4), ChrW(I))
    Next
    sOut = sOut.Replace("{BACKSLASH-" & tGuid.ToString("D") & "}", "\")
    Return sOut
  End Function
End Class
Public Class JSONClosure
  Inherits JSONElement
  Public Sub New()
    MyBase.New(JSONElementType.None)
  End Sub
  Public Sub New(eKey As String)
    MyBase.New(JSONElementType.None, eKey)
  End Sub
End Class
Public Class JSONComma
  Inherits JSONElement
  Public Sub New()
    MyBase.New(JSONElementType.None)
  End Sub
  Public Sub New(eKey As String)
    MyBase.New(JSONElementType.None, eKey)
  End Sub
End Class
Public Class JSONFailure
  Inherits JSONElement
  Public Sub New()
    MyBase.New(JSONElementType.None)
  End Sub
  Public Sub New(eKey As String)
    MyBase.New(JSONElementType.None, eKey)
  End Sub
End Class
Public Class JSONArray
  Inherits JSONElement
  Private mCollection As ObjectModel.ReadOnlyCollection(Of JSONElement)
  Public Sub New(lItems As ObjectModel.ReadOnlyCollection(Of JSONElement))
    MyBase.New(JSONElementType.Array)
    mCollection = lItems
  End Sub
  Public Sub New(sKey As String, lItems As ObjectModel.ReadOnlyCollection(Of JSONElement))
    MyBase.New(JSONElementType.Array, sKey)
    mCollection = lItems
  End Sub
  Public Sub New(eArr As Object())
    Me.New(Nothing, eArr)
  End Sub
  Public Sub New(sKey As String, eArr As Object())
    MyBase.New(JSONElementType.Array, sKey)
    Dim myItms As New List(Of JSONElement)
    For Each oEntry As Object In eArr
      If oEntry Is Nothing Then
        myItms.Add(New JSONNull())
        Continue For
      End If
      If IsArray(oEntry) Then
        Dim aVal As Object() = oEntry
        myItms.Add(New JSONArray(aVal))
        Continue For
      End If
      If oEntry.Value.GetType Is GetType(Boolean) Then
        Dim aVal As Boolean = CBool(oEntry.Value)
        myItms.Add(New JSONBoolean(aVal))
        Continue For
      End If
      If oEntry.Value.GetType Is GetType(SByte) Or
         oEntry.Value.GetType Is GetType(Byte) Or
         oEntry.Value.GetType Is GetType(Int16) Or
         oEntry.Value.GetType Is GetType(UInt16) Or
         oEntry.Value.GetType Is GetType(Int32) Or
         oEntry.Value.GetType Is GetType(UInt32) Or
         oEntry.Value.GetType Is GetType(Int64) Or
         oEntry.Value.GetType Is GetType(UInt64) Or
         oEntry.Value.GetType Is GetType(Single) Or
         oEntry.Value.GetType Is GetType(Double) Or
         oEntry.Value.GetType Is GetType(Decimal) Then
        myItms.Add(New JSONNumber(oEntry.Value))
        Continue For
      End If
      If oEntry.Value.GetType Is GetType(String) Then
        myItms.Add(New JSONString(oEntry.Value))
        Continue For
      End If
      If oEntry.Value.GetType Is GetType(Dictionary(Of String, Object)) Then
        Dim aVal As Dictionary(Of String, Object) = oEntry.Value
        myItms.Add(New JSONObject(aVal))
        Continue For
      End If
      myItms.Add(New JSONString(oEntry.ToString))
    Next
    mCollection = New ObjectModel.ReadOnlyCollection(Of JSONElement)(myItms.ToArray)
  End Sub
  Public ReadOnly Property Collection As ObjectModel.ReadOnlyCollection(Of JSONElement)
    Get
      Return New ObjectModel.ReadOnlyCollection(Of JSONElement)(mCollection)
    End Get
  End Property
  Public Overrides Function GetHashCode() As Integer
    If MyBase.Key Is Nothing Then Return MyBase.Type.GetHashCode Xor mCollection.GetHashCode
    Return MyBase.Type.GetHashCode Xor MyBase.Key.GetHashCode Xor mCollection.GetHashCode
  End Function
  Public Overrides Function Equals(obj As Object) As Boolean
    If Not obj.GetType Is GetType(JSONArray) Then Return False
    Dim jObj As JSONArray = obj
    If Not MyBase.Type = jObj.Type Then Return False
    If Not MyBase.Key = jObj.Key Then Return False
    If Not mCollection.Count = jObj.Collection.Count Then Return False
    For I As Integer = 0 To mCollection.Count - 1
      If Not mCollection(I).Equals(jObj.Collection(I)) Then Return False
    Next
    Return True
  End Function
  Public Overrides Function ToString() As String
    Dim sRet As String = Nothing
    If Not MyBase.Key Is Nothing Then sRet &= """" & EncodeString(MyBase.Key) & """:"
    sRet &= "["
    Dim isFirst As Boolean = True
    For Each mEl In mCollection
      If isFirst Then
        isFirst = False
      Else
        sRet &= ","
      End If
      sRet &= mEl.ToString
    Next
    sRet &= "]"
    Return sRet
  End Function
End Class
Public Class JSONObject
  Inherits JSONElement
  Private mSubElements As ObjectModel.ReadOnlyCollection(Of JSONElement)
  Public Sub New(lItems As ObjectModel.ReadOnlyCollection(Of JSONElement))
    MyBase.New(JSONElementType.Object)
    mSubElements = lItems
  End Sub
  Public Sub New(sKey As String, lItems As ObjectModel.ReadOnlyCollection(Of JSONElement))
    MyBase.New(JSONElementType.Object, sKey)
    mSubElements = lItems
  End Sub
  Public Sub New(eArr As Dictionary(Of String, Object))
    Me.New(Nothing, eArr)
  End Sub
  Public Sub New(sKey As String, eArr As Dictionary(Of String, Object))
    MyBase.New(JSONElementType.Object, sKey)
    Dim myEls As New List(Of JSONElement)
    For Each oEntry As KeyValuePair(Of String, Object) In eArr
      If oEntry.Value Is Nothing Then
        myEls.Add(New JSONNull(oEntry.Key))
        Continue For
      End If
      If IsArray(oEntry.Value) Then
        Dim aVal As Object() = oEntry.Value
        myEls.Add(New JSONArray(oEntry.Key, aVal))
        Continue For
      End If
      If oEntry.Value.GetType Is GetType(Boolean) Then
        Dim aVal As Boolean = CBool(oEntry.Value)
        myEls.Add(New JSONBoolean(oEntry.Key, aVal))
        Continue For
      End If
      If oEntry.Value.GetType Is GetType(SByte) Or
         oEntry.Value.GetType Is GetType(Byte) Or
         oEntry.Value.GetType Is GetType(Int16) Or
         oEntry.Value.GetType Is GetType(UInt16) Or
         oEntry.Value.GetType Is GetType(Int32) Or
         oEntry.Value.GetType Is GetType(UInt32) Or
         oEntry.Value.GetType Is GetType(Int64) Or
         oEntry.Value.GetType Is GetType(UInt64) Or
         oEntry.Value.GetType Is GetType(Single) Or
         oEntry.Value.GetType Is GetType(Double) Or
         oEntry.Value.GetType Is GetType(Decimal) Then
        myEls.Add(New JSONNumber(oEntry.Key, oEntry.Value))
        Continue For
      End If
      If oEntry.Value.GetType Is GetType(String) Then
        myEls.Add(New JSONString(oEntry.Key, oEntry.Value))
        Continue For
      End If
      If oEntry.Value.GetType Is GetType(Dictionary(Of String, Object)) Then
        Dim aVal As Dictionary(Of String, Object) = oEntry.Value
        myEls.Add(New JSONObject(oEntry.Key, aVal))
        Continue For
      End If
      myEls.Add(New JSONString(oEntry.Key, oEntry.ToString))
    Next
    mSubElements = New ObjectModel.ReadOnlyCollection(Of JSONElement)(myEls.ToArray)
  End Sub
  Public ReadOnly Property SubElements As ObjectModel.ReadOnlyCollection(Of JSONElement)
    Get
      Return New ObjectModel.ReadOnlyCollection(Of JSONElement)(mSubElements)
    End Get
  End Property
  Public Overrides Function GetHashCode() As Integer
    Return MyBase.Type.GetHashCode Xor mSubElements.GetHashCode Xor MyBase.Key.GetHashCode
  End Function
  Public Overrides Function Equals(obj As Object) As Boolean
    If Not obj.GetType Is GetType(JSONObject) Then Return False
    Dim jObj As JSONObject = obj
    If Not MyBase.Type = jObj.Type Then Return False
    If Not MyBase.Key = jObj.Key Then Return False
    If Not mSubElements.Count = jObj.SubElements.Count Then Return False
    For I As Integer = 0 To mSubElements.Count - 1
      If Not mSubElements(I).Equals(jObj.SubElements(I)) Then Return False
    Next
    Return True
  End Function
  Public Overrides Function ToString() As String
    Dim sRet As String = Nothing
    If Not MyBase.Key Is Nothing Then sRet &= """" & EncodeString(MyBase.Key) & """:"
    sRet &= "{"
    Dim isFirst As Boolean = True
    For Each mEl In mSubElements
      If isFirst Then
        isFirst = False
      Else
        sRet &= ","
      End If
      sRet &= mEl.ToString
    Next
    sRet &= "}"
    Return sRet
  End Function
End Class
Public Class JSONString
  Inherits JSONElement
  Private mValue As String
  Public Sub New(sMsg As String)
    MyBase.New(JSONElementType.Value, Nothing)
    mValue = DecodeString(sMsg)
  End Sub
  Public Sub New(sKey As String, sValue As String)
    MyBase.New(JSONElementType.KeyValue, DecodeString(sKey))
    mValue = DecodeString(sValue)
  End Sub
  Public ReadOnly Property Value As String
    Get
      Return mValue
    End Get
  End Property
  Public Overrides Function GetHashCode() As Integer
    Return MyBase.Type.GetHashCode Xor MyBase.Key.GetHashCode Xor mValue.GetHashCode
  End Function
  Public Overrides Function Equals(obj As Object) As Boolean
    If Not obj.GetType Is GetType(JSONString) Then Return False
    Dim jObj As JSONString = obj
    If Not MyBase.Type = jObj.Type Then Return False
    If Not MyBase.Key = jObj.Key Then Return False
    If Not mValue = jObj.Value Then Return False
    Return True
  End Function
  Public Overrides Function ToString() As String
    Dim sRet As String = Nothing
    If Not MyBase.Key Is Nothing Then sRet &= """" & EncodeString(MyBase.Key) & """:"
    sRet &= """" & EncodeString(mValue) & """"
    Return sRet
  End Function
End Class
Public Class JSONNumber
  Inherits JSONElement
  Private mValue As String
  Public Sub New(sMsg As String)
    MyBase.New(JSONElementType.Value, Nothing)
    mValue = sMsg
  End Sub
  Public Sub New(sKey As String, sValue As String)
    MyBase.New(JSONElementType.KeyValue, sKey)
    mValue = sValue
  End Sub
  Public ReadOnly Property Value As String
    Get
      Return mValue
    End Get
  End Property
  Public Overrides Function GetHashCode() As Integer
    Return MyBase.Type.GetHashCode Xor MyBase.Key.GetHashCode Xor mValue.GetHashCode
  End Function
  Public Overrides Function Equals(obj As Object) As Boolean
    If Not obj.GetType Is GetType(JSONNumber) Then Return False
    Dim jObj As JSONNumber = obj
    If Not MyBase.Type = jObj.Type Then Return False
    If Not MyBase.Key = jObj.Key Then Return False
    If Not mValue = jObj.Value Then Return False
    Return True
  End Function
  Public Overrides Function ToString() As String
    Dim sRet As String = Nothing
    If Not MyBase.Key Is Nothing Then sRet &= """" & EncodeString(MyBase.Key) & """:"
    sRet &= mValue
    Return sRet
  End Function
End Class
Public Class JSONBoolean
  Inherits JSONElement
  Private mValue As Boolean
  Public Sub New(sMsg As Boolean)
    MyBase.New(JSONElementType.Value, Nothing)
    mValue = sMsg
  End Sub
  Public Sub New(sKey As String, sValue As Boolean)
    MyBase.New(JSONElementType.KeyValue, sKey)
    mValue = sValue
  End Sub
  Public ReadOnly Property Value As Boolean
    Get
      Return mValue
    End Get
  End Property
  Public Overrides Function GetHashCode() As Integer
    Return MyBase.Type.GetHashCode Xor MyBase.Key.GetHashCode Xor mValue.GetHashCode
  End Function
  Public Overrides Function Equals(obj As Object) As Boolean
    If Not obj.GetType Is GetType(JSONBoolean) Then Return False
    Dim jObj As JSONBoolean = obj
    If Not MyBase.Type = jObj.Type Then Return False
    If Not MyBase.Key = jObj.Key Then Return False
    If Not mValue = jObj.Value Then Return False
    Return True
  End Function
  Public Overrides Function ToString() As String
    Dim sRet As String = Nothing
    If Not MyBase.Key Is Nothing Then sRet &= """" & EncodeString(MyBase.Key) & """:"
    If mValue Then
      sRet &= "true"
    Else
      sRet &= "false"
    End If
    Return sRet
  End Function
End Class
Public Class JSONNull
  Inherits JSONElement
  Public Sub New()
    MyBase.New(JSONElementType.Value, Nothing)
  End Sub
  Public Sub New(sKey As String)
    MyBase.New(JSONElementType.KeyValue, sKey)
  End Sub
  Public Overrides Function GetHashCode() As Integer
    Return MyBase.Type.GetHashCode Xor MyBase.Key.GetHashCode
  End Function
  Public Overrides Function Equals(obj As Object) As Boolean
    If Not obj.GetType Is GetType(JSONNull) Then Return False
    Dim jObj As JSONNull = obj
    If Not MyBase.Type = jObj.Type Then Return False
    If Not MyBase.Key = jObj.Key Then Return False
    Return True
  End Function
  Public Overrides Function ToString() As String
    Dim sRet As String = Nothing
    If Not MyBase.Key Is Nothing Then sRet &= """" & EncodeString(MyBase.Key) & """:"
    sRet &= "null"
    Return sRet
  End Function
End Class

Public NotInheritable Class JSONReader
  Private mJSON As JSONElement = New JSONFailure
  Public ReadOnly Property JSON As JSONElement
    Get
      Return mJSON
    End Get
  End Property
  Private enc As System.Text.Encoding
  Public ReadOnly Property TextEncoding As System.Text.Encoding
    Get
      Return enc
    End Get
  End Property
  Public Sub New(stream As IO.Stream, ExpectUTF8 As Boolean)
    enc = System.Text.Encoding.GetEncoding(srlFunctions.LATIN_1)
    Dim bom0 As Integer = stream.ReadByte
    Dim bom1 As Integer = stream.ReadByte
    Dim bom2 As Integer = stream.ReadByte
    Dim bom3 As Integer = stream.ReadByte
    If bom0 = &HEF And bom1 = &HBB And bom2 = &HBF Then
      enc = System.Text.Encoding.GetEncoding(srlFunctions.UTF_8)
      stream.Seek(3, IO.SeekOrigin.Begin)
    ElseIf bom0 = &H0 And bom1 = &H0 And bom2 = &HFE And bom3 = &HFF Then
      enc = System.Text.Encoding.GetEncoding(srlFunctions.UTF_32_BE)
    ElseIf bom0 = &HFF And bom1 = &HFE And bom2 = &H0 And bom3 = &H0 Then
      enc = System.Text.Encoding.GetEncoding(srlFunctions.UTF_32_LE)
    ElseIf bom0 = &HFE And bom1 = &HFF Then
      enc = System.Text.Encoding.GetEncoding(srlFunctions.UTF_16_BE)
      stream.Seek(2, IO.SeekOrigin.Begin)
    ElseIf bom0 = &HFF And bom1 = &HFE Then
      enc = System.Text.Encoding.GetEncoding(srlFunctions.UTF_16_LE)
      stream.Seek(2, IO.SeekOrigin.Begin)
    Else
      stream.Seek(0, IO.SeekOrigin.Begin)
      If ExpectUTF8 Then enc = System.Text.Encoding.GetEncoding(srlFunctions.UTF_8)
    End If
    Dim workElement As JSONElement = ReadElement(stream, TextEncoding)
    If workElement.GetType Is GetType(JSONFailure) Then
      mJSON = New JSONFailure
      Return
    End If
    Do While stream.Position < stream.Length
      Dim sRead As String = ReadCharacter(stream, enc)
      If Char.IsWhiteSpace(sRead) Then Continue Do
      If String.IsNullOrEmpty(sRead) Then Exit Do
      mJSON = New JSONFailure
      Return
    Loop
    mJSON = workElement
  End Sub
  Private Shared Function ReadCharacter(stream As IO.Stream, streamEncoding As System.Text.Encoding) As String
    If Not stream.CanRead Then Return Nothing
    If stream.Length > -1 AndAlso stream.Position > stream.Length Then Return Nothing
    Select Case streamEncoding.CodePage
      Case srlFunctions.LATIN_1
        Dim b As Integer = stream.ReadByte
        If b = -1 Then Return Nothing
        Return ChrW(b)
      Case srlFunctions.UTF_8
        Dim b0 As Integer = stream.ReadByte
        If b0 = -1 Then Return Nothing
        If (b0 And &HE0) = &HC0 Then
          Dim b1 As Integer = stream.ReadByte
          If b1 = -1 Then Return Nothing
          Return streamEncoding.GetString({b0, b1})
        ElseIf (b0 And &HF0) = &HE0 Then
          Dim b1 As Integer = stream.ReadByte
          If b1 = -1 Then Return Nothing
          Dim b2 As Integer = stream.ReadByte
          If b2 = -1 Then Return Nothing
          Return streamEncoding.GetString({b0, b1, b2})
        ElseIf (b0 And &HF8) = &HF0 Then
          Dim b1 As Integer = stream.ReadByte
          If b1 = -1 Then Return Nothing
          Dim b2 As Integer = stream.ReadByte
          If b2 = -1 Then Return Nothing
          Dim b3 As Integer = stream.ReadByte
          If b3 = -1 Then Return Nothing
          Return streamEncoding.GetString({b0, b1, b2, b3})
        Else
          Return ChrW(b0)
        End If
      Case srlFunctions.UTF_16_LE
        Dim b0 As Integer = stream.ReadByte
        If b0 = -1 Then Return Nothing
        Dim b1 As Integer = stream.ReadByte
        If b1 = -1 Then Return Nothing
        If (b1 And &HF8) = &HD8 Then
          Dim b2 As Integer = stream.ReadByte
          If b2 = -1 Then Return Nothing
          Dim b3 As Integer = stream.ReadByte
          If b3 = -1 Then Return Nothing
          Return streamEncoding.GetString({b0, b1, b2, b3})
        Else
          Return streamEncoding.GetString({b0, b1})
        End If
      Case srlFunctions.UTF_16_BE
        Dim b0 As Integer = stream.ReadByte
        If b0 = -1 Then Return Nothing
        Dim b1 As Integer = stream.ReadByte
        If b1 = -1 Then Return Nothing
        If (b0 And &HF8) = &HD8 Then
          Dim b2 As Integer = stream.ReadByte
          If b2 = -1 Then Return Nothing
          Dim b3 As Integer = stream.ReadByte
          If b3 = -1 Then Return Nothing
          Return streamEncoding.GetString({b0, b1, b2, b3})
        Else
          Return streamEncoding.GetString({b0, b1})
        End If
      Case srlFunctions.UTF_32_LE, srlFunctions.UTF_32_BE
        Dim b0 As Integer = stream.ReadByte
        If b0 = -1 Then Return Nothing
        Dim b1 As Integer = stream.ReadByte
        If b1 = -1 Then Return Nothing
        Dim b2 As Integer = stream.ReadByte
        If b2 = -1 Then Return Nothing
        Dim b3 As Integer = stream.ReadByte
        If b3 = -1 Then Return Nothing
        Return streamEncoding.GetString({b0, b1, b2, b3})
      Case Else
        Dim b As Integer = stream.ReadByte
        If b = -1 Then Return Nothing
        Return ChrW(b)
    End Select
  End Function
  Private Shared Function ReadObjectElement(stream As IO.Stream, streamEncoding As System.Text.Encoding) As JSONElement
    If Not stream.CanRead Then Return New JSONFailure
    Dim sRead As String
    Do
      sRead = ReadCharacter(stream, streamEncoding)
      If String.IsNullOrEmpty(sRead) Then Return New JSONFailure
    Loop While Char.IsWhiteSpace(sRead)
    If sRead = "}" Then Return New JSONClosure
    If sRead = "," Then Return New JSONComma
    If Not sRead = """" Then Return New JSONFailure
    Dim myKey As String = ""
    Dim bKeyEscape As Boolean = False
    Do
      Dim sKeyChar As String = ReadCharacter(stream, streamEncoding)
      If String.IsNullOrEmpty(sKeyChar) Then Return New JSONFailure
      If bKeyEscape Then
        myKey &= "\" & sKeyChar
        bKeyEscape = False
      ElseIf sKeyChar = "\" Then
        bKeyEscape = True
      ElseIf sKeyChar = """" Then
        Exit Do
      Else
        myKey &= sKeyChar
        bKeyEscape = False
      End If
    Loop
    Dim sSplit As String
    Do
      sSplit = ReadCharacter(stream, streamEncoding)
      If String.IsNullOrEmpty(sSplit) Then Return New JSONFailure
    Loop While Char.IsWhiteSpace(sSplit)
    If Not sSplit = ":" Then Return New JSONFailure
    Do
      sRead = ReadCharacter(stream, streamEncoding)
      If String.IsNullOrEmpty(sRead) Then Return New JSONFailure
    Loop While Char.IsWhiteSpace(sRead)
    If sRead = "{" Then
      Dim myList As New List(Of JSONElement)
      Do
        Dim workElement As JSONElement = ReadObjectElement(stream, streamEncoding)
        If workElement.GetType Is GetType(JSONClosure) And myList.Count = 0 Then Exit Do
        If workElement.Type = JSONElementType.None Then Return New JSONFailure
        myList.Add(workElement)
        Dim commaElement As JSONElement = ReadObjectElement(stream, streamEncoding)
        If commaElement.GetType Is GetType(JSONClosure) Then Exit Do
        If Not commaElement.GetType Is GetType(JSONComma) Then Return New JSONFailure
      Loop
      Return New JSONObject(myKey, New ObjectModel.ReadOnlyCollection(Of JSONElement)(myList))
    End If
    If sRead = "[" Then
      Dim myArr As New List(Of JSONElement)
      Do
        Dim workElement As JSONElement = ReadArrayElement(stream, streamEncoding)
        If workElement.GetType Is GetType(JSONClosure) And myArr.Count = 0 Then Exit Do
        If workElement.Type = JSONElementType.None Then Return New JSONFailure
        myArr.Add(workElement)
        Dim commaElement As JSONElement = ReadArrayElement(stream, streamEncoding)
        If commaElement.GetType Is GetType(JSONClosure) Then Exit Do
        If Not commaElement.GetType Is GetType(JSONComma) Then Return New JSONFailure
      Loop
      Return New JSONArray(myKey, New ObjectModel.ReadOnlyCollection(Of JSONElement)(myArr))
    End If
    Dim sVal As String = Nothing
    If sRead.ToUpperInvariant = "T" Then
      sVal = sRead
      For I As Integer = 1 To 3
        Dim sChr As String = ReadCharacter(stream, streamEncoding)
        If String.IsNullOrEmpty(sChr) Then Return New JSONFailure
        sVal &= sChr
      Next
      If Not sVal.ToUpperInvariant = "TRUE" Then Return New JSONFailure
      Return New JSONBoolean(myKey, True)
    End If
    If sRead.ToUpperInvariant = "F" Then
      sVal = sRead
      For I As Integer = 1 To 4
        Dim sChr As String = ReadCharacter(stream, streamEncoding)
        If String.IsNullOrEmpty(sChr) Then Return New JSONFailure
        sVal &= sChr
      Next
      If Not sVal.ToUpperInvariant = "FALSE" Then Return New JSONFailure
      Return New JSONBoolean(myKey, False)
    End If
    If sRead.ToUpperInvariant = "N" Then
      sVal = sRead
      For I As Integer = 1 To 3
        Dim sChr As String = ReadCharacter(stream, streamEncoding)
        If String.IsNullOrEmpty(sChr) Then Return New JSONFailure
        sVal &= sChr
      Next
      If Not sVal.ToUpperInvariant = "NULL" Then Return New JSONFailure
      Return New JSONNull(myKey)
    End If
    If IsNumeric(sRead) Or sRead = "-" Then
      sVal = sRead
      Dim sChar As String
      Do
        sChar = ReadCharacter(stream, streamEncoding)
        If String.IsNullOrEmpty(sChar) Then Return New JSONFailure
        If IsNumeric(sChar) Or sChar = "." Then
          sVal &= sChar
        Else
          stream.Seek(-1, IO.SeekOrigin.Current)
          Exit Do
        End If
      Loop
      Return New JSONNumber(myKey, sVal)
    End If
    If sRead = """" Then
      Dim bEscape As Boolean = False
      Do
        Dim sChar As String = ReadCharacter(stream, streamEncoding)
        If String.IsNullOrEmpty(sChar) Then Return New JSONFailure
        If bEscape Then
          sVal &= "\" & sChar
          bEscape = False
        ElseIf sChar = "\" Then
          bEscape = True
        ElseIf sChar = """" Then
          Exit Do
        Else
          sVal &= sChar
          bEscape = False
        End If
      Loop
      Return New JSONString(myKey, sVal)
    End If
    Return New JSONFailure
  End Function
  Private Shared Function ReadArrayElement(stream As IO.Stream, streamEncoding As System.Text.Encoding) As JSONElement
    If Not stream.CanRead Then Return New JSONFailure
    Dim sRead As String
    Do
      sRead = ReadCharacter(stream, streamEncoding)
      If String.IsNullOrEmpty(sRead) Then Return New JSONFailure
    Loop While Char.IsWhiteSpace(sRead)
    If sRead = "]" Then Return New JSONClosure
    If sRead = "," Then Return New JSONComma
    If sRead = "{" Then
      Dim myList As New List(Of JSONElement)
      Do
        Dim workElement As JSONElement = ReadObjectElement(stream, streamEncoding)
        If workElement.GetType Is GetType(JSONClosure) And myList.Count = 0 Then Exit Do
        If workElement.Type = JSONElementType.None Then Return New JSONFailure
        myList.Add(workElement)
        Dim commaElement As JSONElement = ReadObjectElement(stream, streamEncoding)
        If commaElement.GetType Is GetType(JSONClosure) Then Exit Do
        If Not commaElement.GetType Is GetType(JSONComma) Then Return New JSONFailure
      Loop
      Return New JSONObject(New ObjectModel.ReadOnlyCollection(Of JSONElement)(myList))
    End If
    If sRead = "[" Then
      Dim myArr As New List(Of JSONElement)
      Do
        Dim workElement As JSONElement = ReadArrayElement(stream, streamEncoding)
        If workElement.GetType Is GetType(JSONClosure) And myArr.Count = 0 Then Exit Do
        If workElement.Type = JSONElementType.None Then Return New JSONFailure
        myArr.Add(workElement)
        Dim commaElement As JSONElement = ReadArrayElement(stream, streamEncoding)
        If commaElement.GetType Is GetType(JSONClosure) Then Exit Do
        If Not commaElement.GetType Is GetType(JSONComma) Then Return New JSONFailure
      Loop
      Return New JSONArray(New ObjectModel.ReadOnlyCollection(Of JSONElement)(myArr))
    End If
    Dim sVal As String = Nothing
    If sRead.ToUpperInvariant = "T" Then
      sVal = sRead
      For I As Integer = 1 To 3
        Dim sChr As String = ReadCharacter(stream, streamEncoding)
        If String.IsNullOrEmpty(sChr) Then Return New JSONFailure
        sVal &= sChr
      Next
      If Not sVal.ToUpperInvariant = "TRUE" Then Return New JSONFailure
      Return New JSONBoolean(True)
    End If
    If sRead.ToUpperInvariant = "F" Then
      sVal = sRead
      For I As Integer = 1 To 4
        Dim sChr As String = ReadCharacter(stream, streamEncoding)
        If String.IsNullOrEmpty(sChr) Then Return New JSONFailure
        sVal &= sChr
      Next
      If Not sVal.ToUpperInvariant = "FALSE" Then Return New JSONFailure
      Return New JSONBoolean(False)
    End If
    If sRead.ToUpperInvariant = "N" Then
      sVal = sRead
      For I As Integer = 1 To 3
        Dim sChr As String = ReadCharacter(stream, streamEncoding)
        If String.IsNullOrEmpty(sChr) Then Return New JSONFailure
        sVal &= sChr
      Next
      If Not sVal.ToUpperInvariant = "NULL" Then Return New JSONFailure
      Return New JSONNull
    End If
    If IsNumeric(sRead) Or sRead = "-" Then
      sVal = sRead
      Dim sChar As String
      Do
        sChar = ReadCharacter(stream, streamEncoding)
        If String.IsNullOrEmpty(sChar) Then Return New JSONFailure
        If IsNumeric(sChar) Or sChar = "." Then
          sVal &= sChar
        Else
          stream.Seek(-1, IO.SeekOrigin.Current)
          Exit Do
        End If
      Loop
      Return New JSONNumber(sVal)
    End If
    If sRead = """" Then
      Dim bEscape As Boolean = False
      Do
        Dim sChar As String = ReadCharacter(stream, streamEncoding)
        If String.IsNullOrEmpty(sChar) Then Return New JSONFailure
        If bEscape Then
          sVal &= "\" & sChar
          bEscape = False
        ElseIf sChar = "\" Then
          bEscape = True
        ElseIf sChar = """" Then
          Exit Do
        Else
          sVal &= sChar
          bEscape = False
        End If
      Loop
      Return New JSONString(sVal)
    End If
    Return New JSONFailure
  End Function
  Public Shared Function ReadElement(stream As IO.Stream, streamEncoding As System.Text.Encoding) As JSONElement
    If Not stream.CanRead Then Return New JSONFailure
    Dim sRead As String
    Do
      sRead = ReadCharacter(stream, streamEncoding)
      If String.IsNullOrEmpty(sRead) Then Return New JSONFailure
    Loop While Char.IsWhiteSpace(sRead)
    If sRead = "{" Then
      Dim myList As New List(Of JSONElement)
      Do
        Dim workElement As JSONElement = ReadObjectElement(stream, streamEncoding)
        If workElement.GetType Is GetType(JSONClosure) And myList.Count = 0 Then Exit Do
        If workElement.Type = JSONElementType.None Then Return New JSONFailure
        myList.Add(workElement)
        Dim commaElement As JSONElement = ReadObjectElement(stream, streamEncoding)
        If commaElement.GetType Is GetType(JSONClosure) Then Exit Do
        If Not commaElement.GetType Is GetType(JSONComma) Then Return New JSONFailure
      Loop
      Return New JSONObject(New ObjectModel.ReadOnlyCollection(Of JSONElement)(myList))
    End If
    If sRead = "[" Then
      Dim myArr As New List(Of JSONElement)
      Do
        Dim workElement As JSONElement = ReadArrayElement(stream, streamEncoding)
        If workElement.GetType Is GetType(JSONClosure) And myArr.Count = 0 Then Exit Do
        If workElement.Type = JSONElementType.None Then Return New JSONFailure
        myArr.Add(workElement)
        Dim commaElement As JSONElement = ReadArrayElement(stream, streamEncoding)
        If commaElement.GetType Is GetType(JSONClosure) Then Exit Do
        If Not commaElement.GetType Is GetType(JSONComma) Then Return New JSONFailure
      Loop
      Return New JSONArray(New ObjectModel.ReadOnlyCollection(Of JSONElement)(myArr))
    End If
    If sRead = "," Then Return New JSONComma
    Return New JSONFailure
  End Function
End Class

Friend NotInheritable Class JSONAssociator
  Private Sub New()
  End Sub
  Private Shared Function MakeAssoc(jsIn As JSONNull) As Object
    Return Nothing
  End Function
  Private Shared Function MakeAssoc(jsIn As JSONBoolean) As Object
    Return jsIn.Value
  End Function
  Private Shared Function MakeAssoc(jsIn As JSONNumber) As Object
    If jsIn.Value.Contains(".") Then Return CDbl(jsIn.Value)
    If jsIn.Value.StartsWith("-") Then Return CLng(jsIn.Value)
    Return CULng(jsIn.Value)
  End Function
  Private Shared Function MakeAssoc(jsIn As JSONString) As Object
    Return jsIn.Value
  End Function
  Private Shared Function MakeAssoc(jsIn As JSONArray) As Object
    Dim dArr As New List(Of Object)
    For I As Integer = 0 To jsIn.Collection.Count - 1
      Dim assocList As Object = MakeAssoc(jsIn.Collection(I))
      dArr.Add(assocList)
    Next
    Return dArr.ToArray
  End Function
  Private Shared Function MakeAssoc(jsIn As JSONObject) As Object
    Dim dGrp As New Dictionary(Of String, Object)
    For Each gEnt As JSONElement In jsIn.SubElements
      Dim grpList As Object = MakeAssoc(gEnt)
      dGrp.Add(gEnt.Key, grpList)
    Next
    Return dGrp
  End Function
  Private Shared Function MakeAssoc(jsIn As JSONElement) As Object
    If jsIn.GetType Is GetType(JSONNull) Then Return MakeAssoc(CType(jsIn, JSONNull))
    If jsIn.GetType Is GetType(JSONBoolean) Then Return MakeAssoc(CType(jsIn, JSONBoolean))
    If jsIn.GetType Is GetType(JSONNumber) Then Return MakeAssoc(CType(jsIn, JSONNumber))
    If jsIn.GetType Is GetType(JSONString) Then Return MakeAssoc(CType(jsIn, JSONString))
    If jsIn.GetType Is GetType(JSONArray) Then Return MakeAssoc(CType(jsIn, JSONArray))
    If jsIn.GetType Is GetType(JSONObject) Then Return MakeAssoc(CType(jsIn, JSONObject))
    Stop
    Return Nothing
  End Function
  Public Shared Function Associate(jsIn As JSONReader) As Object
    If jsIn.JSON.GetType Is GetType(JSONFailure) Then Return Nothing
    Return MakeAssoc(jsIn.JSON)
  End Function
End Class
