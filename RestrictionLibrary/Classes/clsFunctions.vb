Imports System.IO

Public NotInheritable Class srlFunctions
  Private Shared lastSocketErrSend As Long = 0
  ''' <summary>
  ''' ISO-8859-1 Latin-1 Encoding
  ''' </summary>
  ''' <remarks>Charset for Latin-1 when using Text Encoding functions.</remarks>
  Public Const LATIN_1 As Integer = 28591
  Public Const UTF_8 As Integer = 949
  Public Const UTF_16_LE As Integer = 1200
  Public Const UTF_16_BE As Integer = 1201
  Public Const UTF_32_LE As Integer = 12000
  Public Const UTF_32_BE As Integer = 12001

  Private Sub New()
  End Sub
  ''' <summary>
  ''' Encodes a string for use in HTML form submissions.
  ''' </summary>
  ''' <param name="string">The string you wish to percent-encode.</param>
  ''' <returns>A string identical to <paramref name="string" /> except that nonalphanumeric characters are encoded for use in HTML forms.</returns>
  ''' <remarks>
  '''  <para>This function percent-encodes all non-alphanumeric characters with four exceptions.</para>
  '''  <para><c>Spaces</c> will be replaced with a &quot;+&quot; rather than &quot;%20&quot;.</para>
  '''  <para><c>Hyphens</c>, <c>Underscores</c>, and <c>Asterisks</c> will be left alone.</para>
  ''' </remarks>
  Public Shared Function PercentEncode([string] As String) As String
    Dim sRet As String = ""
    If String.IsNullOrEmpty([string]) Then Return [string]
    For I As Integer = [string].Length - 1 To 0 Step -1
      Dim iChar As Integer = Convert.ToInt32([string](I))
      Select Case iChar
        Case 45, 46, 48 To 57, 65 To 90, 95, 97 To 122, 126 : sRet = [string](I).ToString & sRet
        Case 32 : sRet = "+" & sRet
        Case Else : sRet = "%" & PadHex(iChar, 2) & sRet
      End Select
    Next
    Return sRet
  End Function
  ''' <summary>
  ''' Decodes a string from encoded URL data.
  ''' </summary>
  ''' <param name="string">The string you wish to percent-decode.</param>
  ''' <returns>A string decoded from <paramref name="string" /> where nonalphanumeric characters are returned to their original form.</returns>
  ''' <remarks>
  '''  <para>This function percent-decodes all non-alphanumeric characters.</para>
  ''' </remarks>
  Public Shared Function PercentDecode([string] As String) As String
    Dim sRet As String = ""
    If String.IsNullOrEmpty([string]) Then Return [string]
    For I As Integer = 0 To [string].Length - 1
      If Not [string](I) = "%"c Then
        sRet &= [string](I)
        Continue For
      End If
      If I > [string].Length - 3 Then
        sRet &= [string](I)
        Continue For
      End If
      Dim iCharA As Integer = Convert.ToInt32([string](I + 1))
      If Not ((iCharA >= 48 And iCharA <= 57) Or (iCharA >= 65 And iCharA <= 70) Or (iCharA >= 97 And iCharA <= 102)) Then
        sRet &= [string](I)
        Continue For
      End If
      Dim iCharB As Integer = Convert.ToInt32([string](I + 2))
      If Not ((iCharB >= 48 And iCharB <= 57) Or (iCharB >= 65 And iCharB <= 70) Or (iCharB >= 97 And iCharB <= 102)) Then
        sRet &= [string](I)
        Continue For
      End If
      Dim sHex As String = [string](I + 1) & [string](I + 2)
      Dim iDec As Integer = Convert.ToByte(sHex, 16)
      sRet &= Convert.ToChar(iDec)
      I += 2
    Next
    Return sRet
  End Function
  ''' <summary>
  ''' Convert an <c>Integer</c> into a hexadecimal string with an appropriate zero-prefix padding.
  ''' </summary>
  ''' <param name="value">The base-10 number you wish to convert to base-16.</param>
  ''' <returns>A string containing the hexadecimal value that was passed to the function.</returns>
  ''' <remarks>
  ''' <para>If the length of the hexadecimal value is less than 8 characters and odd, one zero is prefixed.</para>
  ''' <para>If the length of the hexadecimal value is between 9 and 15 characters, enough zeroes are prefixed to bring the length to 16.</para>
  ''' <para>If the length of the hexadecimal value is between 17 and 31 characters, the string will be padded to 32.</para>
  ''' <para>If the length of the hexadecimal value is between 33 and 63 characters, the string will be padded to 64.</para>
  ''' <para>If the length of the hexadecimal value is between 65 and 127 characters, the string will be padded to 128.</para>
  ''' <para>If the length of the hexadecimal value is greater than 128, the string will not be padded.</para>
  ''' </remarks>
  Public Shared Function PadHex(value As UInt32) As String
    Dim sHex As String = Convert.ToString(value, 16)
    Select Case sHex.Length
      Case 0 : Return "00"
      Case 1, 3, 5, 7 : Return "0" & sHex
      Case 2, 4, 6, 8 : Return sHex
      Case Is < 17 : Return StrDup(16 - sHex.Length, "0") & sHex
      Case Is < 33 : Return StrDup(32 - sHex.Length, "0") & sHex
      Case Is < 65 : Return StrDup(64 - sHex.Length, "0") & sHex
      Case Is < 129 : Return StrDup(128 - sHex.Length, "0") & sHex
      Case Else : Return sHex
    End Select
  End Function
  ''' <summary>
  ''' Convert an <c>Integer</c> into a hexadecimal string with a specific zero-prefix padding.
  ''' </summary>
  ''' <param name="value">The base-10 number you wish to convert to base-16.</param>
  ''' <param name="padTo">The minimum length of the output value.</param>
  ''' <returns>A string containing the hexadecimal value that was passed to the function.</returns>
  ''' <remarks>If the length of the hexadecimal value is greater than <paramref name="padTo" /> then no padding is performed.</remarks>
  Public Shared Function PadHex(value As UInt32, padTo As UInt16) As String
    Dim sVal As String = Convert.ToString(value, 16)
    Do While sVal.Length < padTo : sVal = "0" & sVal : Loop
    Return sVal
  End Function
  ''' <summary>
  ''' Reverts hexadecimal HTML entities in a string to their original characters.
  ''' </summary>
  ''' <param name="string">The string you wish to decode.</param>
  ''' <returns>A string identical to <paramref name="string" /> except that all encoded values have been decoded.</returns>
  ''' <remarks>A hexadecimal HTML entity is in the form of &amp;#xNN;, where NN is a hexadecimal ASCII value for the character it represents.</remarks>
  Public Shared Function HexDecode([string] As String) As String
    Dim sRet As String = String.Empty
    If String.IsNullOrEmpty([string]) Then Return [string]
    For I As Integer = 0 To [string].Length - 1
      If [string](I) = "&" Then
        If [string].Length - I > 1 AndAlso [string](I + 1) = "#" Then
          If [string].Length - I > 2 AndAlso [string](I + 2) = "x" Then
            If [string].Length - I > 4 AndAlso [string](I + 4) = ";" Then
              Dim hVal As String = [string](I + 3)
              sRet &= Chr(Convert.ToByte(hVal, 16))
              I += 4
            ElseIf [string].Length - I > 5 AndAlso [string](I + 5) = ";" Then
              Dim hVal As String = [string](I + 3) & [string](I + 4)
              sRet &= Chr(Convert.ToByte(hVal, 16))
              I += 5
            Else
              sRet &= [string](I)
            End If
          Else
            sRet &= [string](I)
          End If
        Else
          sRet &= [string](I)
        End If
      Else
        sRet &= [string](I)
      End If
    Next
    Return sRet
  End Function
  Public Shared ReadOnly Property DateFormatProvider As IFormatProvider
    Get
      Dim dP As New Globalization.CultureInfo(String.Empty, True)
      dP.DateTimeFormat.DateSeparator = "/"
      dP.DateTimeFormat.TimeSeparator = ":"
      dP.DateTimeFormat.AMDesignator = "AM"
      dP.DateTimeFormat.PMDesignator = "PM"
      dP.DateTimeFormat.FullDateTimePattern = "dddd, MMMM d, yyyy HH:mm:ss"
      dP.DateTimeFormat.LongDatePattern = "dddd, MMMM d, yyyy"
      dP.DateTimeFormat.LongTimePattern = "h:mm:ss tt"
      dP.DateTimeFormat.MonthDayPattern = "MMMM d"
      dP.DateTimeFormat.ShortDatePattern = "M/d/yyyy"
      dP.DateTimeFormat.ShortTimePattern = "h:mm tt"
      dP.DateTimeFormat.YearMonthPattern = "MMMM, yyyy"
      dP.DateTimeFormat.AbbreviatedDayNames = {"Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"}
      dP.DateTimeFormat.AbbreviatedMonthNames = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", ""}
      dP.DateTimeFormat.DayNames = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"}
      dP.DateTimeFormat.MonthNames = {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", ""}
      dP.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Sunday
      Return dP
    End Get
  End Property
  Public Shared Function TimeToString(d As Date) As String
    Return d.ToString("g", DateFormatProvider)
  End Function
  ''' <summary>
  ''' Attempts to see if a file is in use, waiting for it to be freed.
  ''' </summary>
  ''' <param name="path">The exact path to the file which needs to be checked.</param>
  ''' <param name="access">Write permissions required for checking.</param>
  ''' <returns><c>True</c> on available, <c>False</c> on in use.</returns>
  ''' <remarks>This function can take up to five seconds while waiting for successful file access.</remarks>
  Public Shared Function InUseChecker(path As String, access As IO.FileAccess) As Boolean
    If Not IO.File.Exists(path) Then Return True
    Dim iStart As Long = TickCount()
    Do
      Try
        Select Case access
          Case FileAccess.Read
            Using fs As FileStream = IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite Or FileShare.Delete)
              If fs.CanRead Then
                Return True
                Exit Do
              End If
            End Using
          Case FileAccess.Write, FileAccess.ReadWrite
            Using fs As FileStream = IO.File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite Or FileShare.Delete)
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
  ''' <summary>
  ''' Number of ticks since the computer started up.
  ''' </summary>
  Public Shared Function TickCount() As Long
    Return (Stopwatch.GetTimestamp / Stopwatch.Frequency) * 1000
  End Function
  ''' <summary>
  ''' Iterates through two <see cref="Byte">Byte arrays</see> to ensure their precise equality.
  ''' </summary>
  ''' <param name="inArray1">One of two byte arrays to check for exact equality.</param>
  ''' <param name="inArray2">The other of two byte arrays to check for exact equality.</param>
  ''' <returns><c>True</c> if both arrays are binary equal, <c>False</c> otherwise.</returns>
  ''' <remarks>If the lengths of the arrays is different, the result will be <c>False</c>.</remarks>
  Public Shared Function IterativeEqualityCheck(inArray1() As Byte, inArray2() As Byte) As Boolean
    If Not inArray1.Length = inArray2.Length Then Return False
    Dim isEqual As Boolean = True
    For I As Integer = 0 To inArray1.Length - 1
      If Not inArray1(I) = inArray2(I) Then
        isEqual = False
        Exit For
      End If
    Next
    Return isEqual
  End Function
  ''' <summary>
  ''' Registry-stored Release number.
  ''' </summary>
  Public Shared ReadOnly Property CLRRelease() As UInt32
    Get
      Dim iRelease As UInt32 = 0
      Try
        If My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\NET Framework Setup\NDP\v4").OpenSubKey("Full") IsNot Nothing Then
          iRelease = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full").GetValue("Release")
        ElseIf My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\NET Framework Setup\NDP\v4").OpenSubKey("Client") IsNot Nothing Then
          iRelease = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client").GetValue("Release")
        End If
      Catch ex As Exception
        iRelease = 0
      End Try
      Return iRelease
    End Get
  End Property
  ''' <summary>
  ''' Common Language Runtime exact version number.
  ''' </summary>
  Public Shared ReadOnly Property CLRVersion() As String
    Get
      Dim sCLR As String = Nothing
      Dim tMONO As Type = Nothing
      Try
        tMONO = Type.GetType("Mono.Runtime")
      Catch ex As Exception
      End Try
      If tMONO Is Nothing Then
        Dim iRelease As UInt32 = CLRRelease
        sCLR = Environment.Version.ToString
        If Not iRelease = 0 Then sCLR &= "_" & Trim(CStr(iRelease))
      Else
        Dim myMethods() As Reflection.MethodInfo = tMONO.GetMethods(Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Static)
        For Each mInfo As Reflection.MethodInfo In myMethods
          If mInfo Is Nothing Then Continue For
          Dim mName As String = "Unknown"
          Try
            mName = mInfo.Name
          Catch ex As Exception
            mName = "Unknown"
          End Try
          If Not mName = "GetDisplayName" Then Continue For
          Dim mInvoke As String = Nothing
          Try
            mInvoke = mInfo.Invoke(Nothing, Nothing)
          Catch ex As Exception
            mInvoke = Nothing
          End Try
          If Not String.IsNullOrEmpty(mInvoke) Then
            If mInvoke.Contains(" ") Then
              sCLR = mInvoke.Substring(0, mInvoke.IndexOf(" "c))
              Exit For
            End If
          End If
        Next
        If Not String.IsNullOrEmpty(sCLR) Then
          sCLR = "4.0.30319.17020_" & sCLR
        Else
          If Environment.Version.Major = 4 And Environment.Version.Minor = 0 And Environment.Version.Build = 30319 Then
            Select Case Environment.Version.Revision
              Case 17020 : sCLR = "4.0.30319.17020"
              Case 42000 : sCLR = "4.0.30319.17020_4.4"
              Case Else : sCLR = "4.0.30319.17020_" & Environment.Version.Revision
            End Select
          Else
            sCLR = Environment.Version.ToString
          End If
        End If
      End If
      Return sCLR
    End Get
  End Property
  ''' <summary>
  ''' Common Language Runtime Name and Release version.
  ''' </summary>
  Public Shared ReadOnly Property CLRCleanVersion() As String
    Get
      Dim sVer As String = CLRVersion
      If Not sVer.Substring(0, 9) = "4.0.30319" Then Return "Unknown Runtime (" & sVer & ")"
      Dim clrID, clrID2 As Integer
      Dim monoID As String = Nothing
      If Not sVer.Substring(10).Contains("_") Then
        clrID = Val(sVer.Substring(10))
        clrID2 = 0
      Else
        Dim clrRel As String = sVer.Substring(10)
        Dim clrBld As String = clrRel.Substring(0, clrRel.IndexOf("_"))
        clrRel = clrRel.Substring(clrRel.IndexOf("_") + 1)
        clrID = Val(clrBld)
        clrID2 = Val(clrRel)
        monoID = clrRel
      End If
      If clrID = 17020 Then
        If Not String.IsNullOrEmpty(monoID) Then Return "MONO " & monoID
        Return "MONO"
      End If
      If clrID < 17929 Then
        If clrID = 1 Then Return ".NET 4.0 RTM"
        If clrID = 225 Then Return ".NET 4.0 SP1"
        If clrID = 269 Then Return ".NET 4.0 + MS12-035 GDR"
        If clrID = 276 Then Return ".NET 4.0.3 Runtime Update"
        If clrID = 296 Then Return ".NET 4.0 + MS12-074 GDR"
        If clrID = 544 Then Return ".NET 4.0 + MS12-035 LDR"
        If clrID = 1008 Then Return ".NET 4.0 + MS13-052 GDR"
        If clrID = 1022 Then Return ".NET 4.0 + MS14-009 GDR"
        If clrID = 1026 Then Return ".NET 4.0 + MS14-057 GDR"
        If clrID = 2034 Then Return ".NET 4.0 + MS14-009 LDR"
        Return ".NET 4.0 (" & clrID & ")"
      End If
      If clrID < 18408 Then
        If clrID = 17929 Then Return ".NET 4.5 RTM"
        If clrID = 18063 Then Return ".NET 4.5 + MS14-009"
        Return ".NET 4.5 (" & clrID & ")"
      End If
      If clrID < 34209 Then
        If clrID = 18408 Then Return ".NET 4.5.1"
        If clrID = 18444 Then Return ".NET 4.5.1 + MS14-009"
        If clrID = 34011 Then Return ".NET 4.5.1 for Windows 8 + MS14-009"
        If clrID = 34014 Then Return ".NET 4.5.1 for Windows 8.1"
        Return ".NET 4.5.1 (" & clrID & ")"
      End If
      If clrID < 42000 Then
        If clrID = 34209 Then Return ".NET 4.5.2"
        If clrID = 35312 Then Return ".NET 4.5.2 (35312)"
        Return ".NET 4.5.2 (" & clrID & ")"
      End If
      If clrID = 42000 Then
        If clrID2 = 0 Then Return ".NET 4.6"
        If clrID2 = 393295 Or clrID2 = 393297 Then Return ".NET 4.6" '  6004F 60051
        If clrID2 = 394254 Or clrID2 = 394271 Then Return ".NET 4.6.1" '6040E 6041F
        If clrID2 = 394802 Or clrID2 = 394806 Then Return ".NET 4.6.2" '60632 60636
        If clrID2 = 460798 Or clrID2 = 460805 Then Return ".NET 4.7" '  707FE 70805
        If clrID2 = 461308 Or clrID2 = 461310 Then Return ".NET 4.7.1" '709FC 709FE
        If clrID2 = 461808 Or clrID2 = 461814 Then Return ".NET 4.7.2" '70BF0 70BF6
        If clrID2 = 528040 Or clrID2 = 528049 Or clrID2 = 528209 Or clrID2 = 528449 Then Return ".NET 4.8" '  80EA8 80EB1

        If clrID2 < 393297 Then Return ".NET 4.6 (" & clrID2 & ")"
        If clrID2 < 394271 Then Return ".NET 4.6.1 (" & clrID2 & ")"
        If clrID2 < 394806 Then Return ".NET 4.6.2 (" & clrID2 & ")"
        If clrID2 < 460805 Then Return ".NET 4.7 (" & clrID2 & ")"
        If clrID2 < 461310 Then Return ".NET 4.7.1 (" & clrID2 & ")"
        If clrID2 < 461814 Then Return ".NET 4.7.2 (" & clrID2 & ")"
        If clrID2 < 528049 Then Return ".NET 4.8 (" & clrID2 & ")"

        Dim clrAttempt As String = Hex(clrID2)
        clrAttempt = clrAttempt.Substring(0, clrAttempt.Length - 4)
        Dim iAVer As Integer = Integer.Parse(clrAttempt, Globalization.NumberStyles.HexNumber, Globalization.CultureInfo.InvariantCulture)
        Return ".NET 4." & iAVer & " (" & clrID2 & ")"
      End If
      If clrID2 > 0 Then Return ".NET Future Version (" & clrID & "." & clrID2 & ")"
      Return ".NET Future Version (" & clrID & ")"
    End Get
  End Property
End Class
