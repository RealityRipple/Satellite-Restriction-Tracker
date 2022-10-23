Public Class JSONReader
  Public Enum ElementType
    None
    Group
    Array
    KeyValue
    [String]
  End Enum
  Public Structure JSElement
    Public Type As ElementType
    Public SubElements As List(Of JSElement)
    Public Collection As List(Of JSElement)
    Public Key As String
    Public Value As String
  End Structure
  Public Serial As List(Of JSElement)
  Public TextEncoding As System.Text.Encoding

  Public Sub New(stream As IO.Stream, ExpectUTF8 As Boolean)
    TextEncoding = System.Text.Encoding.GetEncoding(srlFunctions.LATIN_1)
    Dim bom0 As Integer = stream.ReadByte
    Dim bom1 As Integer = stream.ReadByte
    Dim bom2 As Integer = stream.ReadByte
    Dim bom3 As Integer = stream.ReadByte
    If bom0 = &HEF And bom1 = &HBB And bom2 = &HBF Then
      TextEncoding = System.Text.Encoding.GetEncoding(srlFunctions.UTF_8)
      stream.Seek(3, IO.SeekOrigin.Begin)
    ElseIf bom0 = &H0 And bom1 = &H0 And bom2 = &HFE And bom3 = &HFF Then
      TextEncoding = System.Text.Encoding.GetEncoding(srlFunctions.UTF_32_BE)
    ElseIf bom0 = &HFF And bom1 = &HFE And bom2 = &H0 And bom3 = &H0 Then
      TextEncoding = System.Text.Encoding.GetEncoding(srlFunctions.UTF_32_LE)
    ElseIf bom0 = &HFE And bom1 = &HFF Then
      TextEncoding = System.Text.Encoding.GetEncoding(srlFunctions.UTF_16_BE)
      stream.Seek(2, IO.SeekOrigin.Begin)
    ElseIf bom0 = &HFF And bom1 = &HFE Then
      TextEncoding = System.Text.Encoding.GetEncoding(srlFunctions.UTF_16_LE)
      stream.Seek(2, IO.SeekOrigin.Begin)
    Else
      stream.Seek(0, IO.SeekOrigin.Begin)
      If ExpectUTF8 Then TextEncoding = System.Text.Encoding.GetEncoding(srlFunctions.UTF_8)
    End If
    Serial = New List(Of JSElement)
    Dim workElement As JSElement = ReadElement(stream, TextEncoding)
    Do Until workElement.Type = ElementType.None
      Serial.Add(workElement)
      workElement = Nothing
      workElement = ReadElement(stream, TextEncoding)
    Loop
  End Sub

  Private Shared Function ReadCharacter(stream As IO.Stream, streamEncoding As System.Text.Encoding) As String
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

  Public Shared Function ReadElement(stream As IO.Stream, streamEncoding As System.Text.Encoding) As JSElement
    Dim el As New JSElement
    If Not stream.CanRead Then
      el.Type = ElementType.None
      Return el
    End If
    Dim sRead As String = ReadCharacter(stream, streamEncoding)
    Do Until String.IsNullOrEmpty(sRead)
      If sRead = "{" Then
        el.Type = ElementType.Group
        el.SubElements = New List(Of JSElement)
        Dim workElement As JSElement = ReadElement(stream, streamEncoding)
        Do Until workElement.Type = ElementType.None
          el.SubElements.Add(workElement)
          workElement = Nothing
          If Not stream.CanRead Then Exit Do
          workElement = ReadElement(stream, streamEncoding)
        Loop
        Return el
      ElseIf sRead = "}" Then
        el.Type = ElementType.None
        Return el
      ElseIf sRead = "[" Then
        el.Type = ElementType.Array
        el.Collection = New List(Of JSElement)
        Dim workElement As JSElement = ReadElement(stream, streamEncoding)
        Do Until workElement.Type = ElementType.None
          el.Collection.Add(workElement)
          workElement = Nothing
          If Not stream.CanRead Then Exit Do
          workElement = ReadElement(stream, streamEncoding)
        Loop
        Return el
      ElseIf sRead = "]" Then
        el.Type = ElementType.None
        Return el
      ElseIf sRead = """" Then

        el.Type = ElementType.KeyValue
        Dim sKey As String = Nothing
        Dim sText As String = ReadCharacter(stream, streamEncoding)
        Dim escape As Boolean = False
        Do Until String.IsNullOrEmpty(sText)
          If escape Then
            sKey &= "\" & sText
            escape = False
          ElseIf sText = "\" Then
            escape = True
          ElseIf sText = """" Then
            Exit Do
          Else
            sKey &= sText
            escape = False
          End If
          If Not stream.CanRead Then
            el.Type = ElementType.None
            el.Key = Nothing
            Return el
          End If
          sText = ReadCharacter(stream, streamEncoding)
        Loop
        el.Key = sKey
        If Not stream.CanRead Then
          el.Type = ElementType.None
          el.Key = Nothing
          Return el
        End If

        Dim sSplit As String = ReadCharacter(stream, streamEncoding)
        Do Until String.IsNullOrEmpty(sSplit)
          If Not String.IsNullOrEmpty(sSplit) Then
            If sSplit = ":" Then Exit Do
            If sSplit = "," Then Exit Do
            If sSplit = "[" Then Exit Do
            If sSplit = "]" Then Exit Do
            If sSplit = "{" Then Exit Do
            If sSplit = "}" Then Exit Do
          End If
          If Not stream.CanRead Then
            Exit Do
          End If
          sSplit = ReadCharacter(stream, streamEncoding)
        Loop
        If Not sSplit = ":" Then
          el.Type = ElementType.String
          el.Value = el.Key
          stream.Seek(-1, IO.SeekOrigin.Current)
          Return el
        End If

        Dim sNext As String = ReadCharacter(stream, streamEncoding)
        Do Until String.IsNullOrEmpty(sNext)
          If Not String.IsNullOrEmpty(sNext) Then
            If sNext = """" Then Exit Do
            If sNext = "[" Then Exit Do
            If sNext = "{" Then Exit Do
            If sNext.ToUpperInvariant = "T" Or sNext.ToUpperInvariant = "F" Or sNext.ToUpperInvariant = "N" Then Exit Do
            If IsNumeric(sNext) Or sNext = "-" Then Exit Do
          End If
          If Not stream.CanRead Then
            Exit Do
          End If
          sNext = ReadCharacter(stream, streamEncoding)
        Loop
        If sNext = "[" Then
          el.Type = ElementType.Array
          el.Collection = New List(Of JSElement)
          Dim workElement As JSElement = ReadElement(stream, streamEncoding)
          Do Until workElement.Type = ElementType.None
            el.Collection.Add(workElement)
            workElement = Nothing
            If Not stream.CanRead Then Exit Do
            workElement = ReadElement(stream, streamEncoding)
          Loop
        ElseIf sNext = "{" Then
          el.Type = ElementType.Group
          el.SubElements = New List(Of JSElement)
          Dim workElement As JSElement = ReadElement(stream, streamEncoding)
          Do Until workElement.Type = ElementType.None
            el.SubElements.Add(workElement)
            workElement = Nothing
            If Not stream.CanRead Then Exit Do
            workElement = ReadElement(stream, streamEncoding)
          Loop
        ElseIf sNext.ToUpperInvariant = "T" Then
          sText = sNext
          Dim sVal As String = Nothing
          Do Until String.IsNullOrEmpty(sText)
            If (sText = "," Or sText = "]" Or sText = "}") And Not String.IsNullOrEmpty(sVal) Then
              stream.Seek(-1, IO.SeekOrigin.Current)
              If (sVal.ToUpperInvariant = "TRUE") Then Exit Do
            Else
              sVal &= sText
            End If
            If Not stream.CanRead Then
              el.Type = ElementType.None
              el.Key = Nothing
              el.Value = Nothing
              Return el
            End If
            sText = ReadCharacter(stream, streamEncoding)
          Loop
          el.Value = sVal
        ElseIf sNext.ToUpperInvariant = "F" Then
          sText = sNext
          Dim sVal As String = Nothing
          Do Until String.IsNullOrEmpty(sText)
            If (sText = "," Or sText = "]" Or sText = "}") And Not String.IsNullOrEmpty(sVal) Then
              stream.Seek(-1, IO.SeekOrigin.Current)
              If (sVal.ToUpperInvariant = "FALSE") Then Exit Do
            Else
              sVal &= sText
            End If
            If Not stream.CanRead Then
              el.Type = ElementType.None
              el.Key = Nothing
              el.Value = Nothing
              Return el
            End If
            sText = ReadCharacter(stream, streamEncoding)
          Loop
          el.Value = sVal
        ElseIf sNext.ToUpperInvariant = "N" Then
          sText = sNext
          Dim sVal As String = Nothing
          Do Until String.IsNullOrEmpty(sText)
            If (sText = "," Or sText = "]" Or sText = "}") And Not String.IsNullOrEmpty(sVal) Then
              stream.Seek(-1, IO.SeekOrigin.Current)
              If (sVal.ToUpperInvariant = "NULL") Then Exit Do
            Else
              sVal &= sText
            End If
            If Not stream.CanRead Then
              el.Type = ElementType.None
              el.Key = Nothing
              el.Value = Nothing
              Return el
            End If
            sText = ReadCharacter(stream, streamEncoding)
          Loop
          el.Value = sVal
        ElseIf IsNumeric(sNext) Or sNext = "-" Then
          sText = sNext
          Dim sVal As String = Nothing
          Do Until String.IsNullOrEmpty(sText)
            If (sText = "," Or sText = "]" Or sText = "}") And Not String.IsNullOrEmpty(sVal) Then
              stream.Seek(-1, IO.SeekOrigin.Current)
              If IsNumeric(sVal) Then Exit Do
            Else
              sVal &= sText
            End If
            If Not stream.CanRead Then
              el.Type = ElementType.None
              el.Key = Nothing
              el.Value = Nothing
              Return el
            End If
            sText = ReadCharacter(stream, streamEncoding)
          Loop
          el.Value = sVal
        Else
          sText = ReadCharacter(stream, streamEncoding)
          escape = False
          Dim sVal As String = Nothing
          Do Until String.IsNullOrEmpty(sText)
            If escape Then
              sVal &= "\" & sText
              escape = False
            ElseIf sText = "\" Then
              escape = True
            ElseIf sText = """" Then
              Exit Do
            Else
              sVal &= sText
              escape = False
            End If
            If Not stream.CanRead Then
              el.Type = ElementType.None
              el.Key = Nothing
              el.Value = Nothing
              Return el
            End If
            sText = ReadCharacter(stream, streamEncoding)
          Loop
          el.Value = sVal
        End If
        Return el
      End If
      If Not stream.CanRead Then Exit Do
      sRead = ReadCharacter(stream, streamEncoding)
    Loop
    el.Type = ElementType.None
    Return el
  End Function
End Class

Public Class JSONAssociator
  Private Shared Function MakeAssoc(jsIn As JSONReader.JSElement) As Object
    Select Case jsIn.Type
      Case JSONReader.ElementType.None
        Return Nothing
      Case JSONReader.ElementType.String
        Return jsIn.Value
      Case JSONReader.ElementType.KeyValue
        Return jsIn.Value
      Case JSONReader.ElementType.Array
        Dim dArr As New List(Of Object)
        For I As Integer = 0 To jsIn.Collection.Count - 1
          Dim assocList As Object = MakeAssoc(jsIn.Collection(I))
          dArr.Add(assocList)
        Next
        Return dArr.ToArray
      Case JSONReader.ElementType.Group
        Dim dGrp As New Dictionary(Of String, Object)
        For Each gEnt As JSONReader.JSElement In jsIn.SubElements
          Dim grpList As Object = MakeAssoc(gEnt)
          dGrp.Add(gEnt.Key, grpList)
        Next
        Return dGrp
    End Select
    Return Nothing
  End Function
  Public Shared Function Associate(jsIn As JSONReader, Optional index As Integer = 0) As Object
    If index < 0 Then Return Nothing
    If index > jsIn.Serial.Count - 1 Then Return Nothing
    Dim jEl As JSONReader.JSElement = jsIn.Serial(index)
    Return MakeAssoc(jEl)
  End Function
  Public Shared Function Associate(jEl As JSONReader.JSElement) As Object
    Return MakeAssoc(jEl)
  End Function
  Public Shared Function MakeString(dIn As Dictionary(Of String, Object)) As String
    Dim sRet As String = "{"
    Dim isFirst As Boolean = True
    For Each oEntry As KeyValuePair(Of String, Object) In dIn
      If isFirst Then
        sRet &= """" & oEntry.Key & """:"
        isFirst = False
      Else
        sRet &= ",""" & oEntry.Key & """:"
      End If
      If oEntry.Value.GetType Is GetType(Boolean) Then
        If oEntry.Value Then
          sRet &= "true"
        Else
          sRet &= "false"
        End If
      ElseIf oEntry.Value.GetType Is GetType(SByte) Or
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
        sRet &= oEntry.Value.ToString
      ElseIf oEntry.Value.GetType Is GetType(String) Then
        sRet &= """" & oEntry.Value & """"
      ElseIf IsArray(oEntry.Value) Then
        sRet &= MakeString(oEntry.Value)
      Else
        sRet &= MakeString(oEntry.Value)
      End If
    Next
    sRet &= "}"
    Return sRet
  End Function
  Public Shared Function MakeString(aIn As Object()) As String
    Dim sRet As String = "["
    Dim isFirst As Boolean = True
    For Each oValue As Object In aIn
      If isFirst Then
        isFirst = False
      Else
        sRet &= ","
      End If
      If oValue Is Nothing Then
        sRet &= "null"
      ElseIf oValue.GetType Is GetType(Boolean) Then
        If oValue.Value Then
          sRet &= "true"
        Else
          sRet &= "false"
        End If
      ElseIf oValue.GetType Is GetType(SByte) Or
             oValue.GetType Is GetType(Byte) Or
             oValue.GetType Is GetType(Int16) Or
             oValue.GetType Is GetType(UInt16) Or
             oValue.GetType Is GetType(Int32) Or
             oValue.GetType Is GetType(UInt32) Or
             oValue.GetType Is GetType(Int64) Or
             oValue.GetType Is GetType(UInt64) Or
             oValue.GetType Is GetType(Single) Or
             oValue.GetType Is GetType(Double) Or
             oValue.GetType Is GetType(Decimal) Then
        sRet &= oValue.ToString
      ElseIf oValue.GetType Is GetType(String) Then
        sRet &= """" & oValue & """"
      ElseIf IsArray(oValue) Then
        sRet &= MakeString(oValue)
      Else
        sRet &= MakeString(oValue)
      End If
    Next
    sRet &= "]"
    Return sRet
  End Function
End Class
