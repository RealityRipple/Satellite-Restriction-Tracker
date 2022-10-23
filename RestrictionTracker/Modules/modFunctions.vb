Imports System.IO
Imports Microsoft.WindowsAPICodePack.Dialogs

Module modFunctions
  Friend Structure WindowAnimationData
    Public destPoint As Point
    Public startRect As Rectangle
    Public backColor As Color
  End Structure
  Private Class MCIPlayer
    Implements IDisposable
    Private sAlias As String
    Public Sub New(ByVal sFileName As String)
      If Status() <> "" Then
        [Stop]()
        Close()
      End If
      sAlias = IO.Path.GetFileNameWithoutExtension(sFileName)
      Dim sCommand As String = "open """ & sFileName & """ type mpegvideo alias " & sAlias
      NativeMethods.mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Sub Close()
      Dim sCommand As String = "close " & sAlias
      NativeMethods.mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
      sAlias = String.Empty
    End Sub
    Public Sub CloseAll()
      Dim sCommand As String = "close all wait"
      NativeMethods.mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Sub Pause()
      Dim sCommand As String = "pause " & sAlias
      NativeMethods.mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Sub Play(Optional Repeat As Boolean = False)
      Dim sCommand As String = "play " & sAlias & IIf(Repeat, " repeat", String.Empty)
      NativeMethods.mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Sub [Resume]()
      Dim sCommand As String = "play " & sAlias & " from " & CLng(Status())
      NativeMethods.mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Sub [Stop]()
      Dim sCommand As String = "stop " & sAlias
      NativeMethods.mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Function Status() As String
      Dim sBuffer As New System.Text.StringBuilder(128)
      NativeMethods.mciSendString("status " & sAlias & " mode", sBuffer, sBuffer.Capacity, IntPtr.Zero)
      Return sBuffer.ToString()
    End Function
#Region "IDisposable Support"
    Private disposedValue As Boolean
    Protected Overridable Sub Dispose(disposing As Boolean)
      If Not Me.disposedValue Then
        If disposing Then
          [Stop]()
          Close()
          CloseAll()
        End If
      End If
      Me.disposedValue = True
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
      Dispose(True)
      GC.SuppressFinalize(Me)
    End Sub
#End Region
  End Class
  Private Song As MCIPlayer
  Public Const TOPMOST_MENU_ID As Int64 = &H4815
  Public Const TOPMOST_MENU_TEXT As String = "&Topmost"
  Public Const SCALE_MENU_ID As Int64 = &H4816
  Public Const SCALE_MENU_TEXT As String = "Sc&ale Text"
  Public NOTIFIER_STYLE As NotifierStyle
#Region "Alert Notifier"
  Public Class NotifierStyle
    Public Background As Image
    Public CloseButton As Image
    Public TransparencyKey As Color
    Public TitleLocation As Rectangle
    Public ContentLocation As Rectangle
    Public CloseLocation As Point
    Public TitleColor As Color
    Public ContentColor As Color
    Public ContentHoverColor As Color
    Public Sub New()
      Background = My.Resources.default_alert
      CloseButton = My.Resources.default_close
      TransparencyKey = Color.Fuchsia
      TitleLocation = New Rectangle(7, 3, 188, 24)
      ContentLocation = New Rectangle(9, 31, 227, 66)
      CloseLocation = New Point(190, 0)
      TitleColor = Color.White
      ContentColor = Color.Black
      ContentHoverColor = Color.Blue
    End Sub
    Public Sub New(BGPath As String, ClosePath As String, LocPath As String)
      Using iBG As Image = Image.FromFile(BGPath)
        Using bg As New Bitmap(iBG.Width, iBG.Height)
          Using g As Graphics = Graphics.FromImage(bg)
            g.DrawImage(iBG, 0, 0)
          End Using
          Background = bg.Clone
        End Using
      End Using

      Using iClose As Image = Image.FromFile(ClosePath)
        Using close As New Bitmap(iClose.Width, iClose.Height)
          Using g As Graphics = Graphics.FromImage(close)
            g.DrawImage(iClose, 0, 0)
          End Using
          CloseButton = close.Clone
        End Using
      End Using


      Dim locData As String = My.Computer.FileSystem.ReadAllText(LocPath, System.Text.Encoding.GetEncoding(srlFunctions.LATIN_1))
      locData = locData.Replace(vbNewLine, vbCr).Replace(vbLf, vbCr)
      Do While locData.Contains(vbCr & vbCr)
        locData = locData.Replace(vbCr & vbCr, vbCr)
      Loop
      Dim locLines() As String = Split(locData, vbCr)
      TransparencyKey = ColorIDToColor(locLines(0))
      TitleLocation = RectIDToRect(locLines(1))
      TitleColor = ColorIDToColor(locLines(2))
      ContentLocation = RectIDToRect(locLines(3))
      ContentColor = ColorIDToColor(locLines(4))
      ContentHoverColor = ColorIDToColor(locLines(5))
      CloseLocation = PointIDToPoint(locLines(6))
    End Sub
    Private Function ColorIDToColor(ID As String) As Color
      Try
        If ID.StartsWith("#") Then ID = ID.Substring(1)
        Dim iR As Integer = Integer.Parse(ID.Substring(0, 2), Globalization.NumberStyles.HexNumber, Globalization.CultureInfo.InvariantCulture)
        Dim iG As Integer = Integer.Parse(ID.Substring(2, 2), Globalization.NumberStyles.HexNumber, Globalization.CultureInfo.InvariantCulture)
        Dim iB As Integer = Integer.Parse(ID.Substring(4, 2), Globalization.NumberStyles.HexNumber, Globalization.CultureInfo.InvariantCulture)
        Return Color.FromArgb(iR, iG, iB)
      Catch ex As Exception
        Return Color.Transparent
      End Try
    End Function
    Private Function RectIDToRect(ID As String) As Rectangle
      Try
        ID = ID.Replace(" ", "")
        Dim IDs() As String = Split(ID, ",")
        Dim x As Integer = Integer.Parse(IDs(0), Globalization.CultureInfo.InvariantCulture)
        Dim y As Integer = Integer.Parse(IDs(1), Globalization.CultureInfo.InvariantCulture)
        Dim w As Integer = Integer.Parse(IDs(2), Globalization.CultureInfo.InvariantCulture)
        Dim h As Integer = Integer.Parse(IDs(3), Globalization.CultureInfo.InvariantCulture)
        Return New Rectangle(x, y, w, h)
      Catch ex As Exception
        Return Rectangle.Empty
      End Try
    End Function
    Private Function PointIDToPoint(ID As String) As Point
      Try
        ID = ID.Replace(" ", "")
        Dim IDs() As String = Split(ID, ",")
        Dim x As Integer = Integer.Parse(IDs(0), Globalization.CultureInfo.InvariantCulture)
        Dim y As Integer = Integer.Parse(IDs(1), Globalization.CultureInfo.InvariantCulture)
        Return New Point(x, y)
      Catch ex As Exception
        Return Point.Empty
      End Try
    End Function
  End Class
  Public Function LoadAlertStyle(Path As String) As NotifierStyle
    If My.Computer.FileSystem.FileExists(IO.Path.Combine(LocalAppDataDirectory, Path & ".tgz")) Then
      Path = IO.Path.Combine(LocalAppDataDirectory, Path & ".tgz")
    ElseIf My.Computer.FileSystem.FileExists(IO.Path.Combine(LocalAppDataDirectory, Path & ".tar.gz")) Then
      Path = IO.Path.Combine(LocalAppDataDirectory, Path & ".tar.gz")
    ElseIf My.Computer.FileSystem.FileExists(IO.Path.Combine(LocalAppDataDirectory, Path & ".tar")) Then
      Path = IO.Path.Combine(LocalAppDataDirectory, Path & ".tar")
    Else
      Return New NotifierStyle
    End If
    Try
      Dim TempAlertDir As String = IO.Path.Combine(LocalAppDataDirectory, "notifier")
      Dim TempAlertTAR As String = IO.Path.Combine(LocalAppDataDirectory, "notifier.tar")
      If Path.EndsWith(".tar", StringComparison.OrdinalIgnoreCase) Then
        ExtractTar(Path, TempAlertDir)
      Else
        Try
          ExtractGZ(Path, TempAlertTAR)
          ExtractTar(TempAlertTAR, TempAlertDir)
          My.Computer.FileSystem.DeleteFile(TempAlertTAR)
        Catch ex As Exception
          ExtractTar(Path, TempAlertDir)
        End Try
      End If
      Dim ns As New NotifierStyle(IO.Path.Combine(TempAlertDir, "alert.png"), IO.Path.Combine(TempAlertDir, "close.png"), IO.Path.Combine(TempAlertDir, "loc"))
      My.Computer.FileSystem.DeleteDirectory(TempAlertDir, FileIO.DeleteDirectoryOption.DeleteAllContents)
      Return ns
    Catch ex As Exception
      Return New NotifierStyle
    End Try
  End Function
  Private Sub ExtractGZ(sGZ As String, sDestFile As String)
    Using sourceTGZ As New IO.FileStream(sGZ, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
      Dim sourceGZ As New System.IO.Compression.GZipStream(sourceTGZ, IO.Compression.CompressionMode.Decompress)
      Using destTAR As IO.FileStream = IO.File.Create(sDestFile)
        Dim buffer As Byte()
        ReDim buffer(4095)
        Dim numRead As Integer = sourceGZ.Read(buffer, 0, buffer.Length)
        Do While numRead <> 0
          destTAR.Write(buffer, 0, numRead)
          numRead = sourceGZ.Read(buffer, 0, buffer.Length)
        Loop
        destTAR.Flush(True)
      End Using
    End Using
  End Sub
  Private Class TarFileData
    Public FileName As String
    Public FileMode As UInt32
    Public OwnerID As UInt32
    Public GroupID As UInt32
    Public FileSize As UInt64
    Public LastMod As UInt64
    Public Checksum As UInt32
    Public LinkIndicator As Byte
    Public LinkedFile As String
    Public FileData() As Byte
    Public Sub New(bIn As IO.BinaryReader)
      Dim startAt As Long = bIn.BaseStream.Position
      FileName = ReadBString(bIn.ReadBytes(100))
      FileMode = ReadBInt(bIn.ReadBytes(8))
      OwnerID = ReadBInt(bIn.ReadBytes(8))
      GroupID = ReadBInt(bIn.ReadBytes(8))
      FileSize = ReadBOct(bIn.ReadBytes(12))
      LastMod = ReadBOct(bIn.ReadBytes(12))
      Checksum = ReadBInt(bIn.ReadBytes(8))
      LinkIndicator = ReadBByte(bIn.ReadBytes(1))
      LinkedFile = ReadBString(bIn.ReadBytes(100))
      bIn.BaseStream.Seek(startAt, IO.SeekOrigin.Begin)
      bIn.BaseStream.Seek(512, IO.SeekOrigin.Current)
      If FileSize > 0 Then
        FileData = bIn.ReadBytes(FileSize)
        Dim Leftovers As Integer = FileSize Mod 512
        If Leftovers > 0 Then bIn.BaseStream.Seek(512 - Leftovers, IO.SeekOrigin.Current)
      End If
    End Sub
    Private Function ReadBString(inBytes() As Byte) As String
      Dim sRet As String = System.Text.Encoding.ASCII.GetString(inBytes)
      If sRet.Contains(vbNullChar) Then sRet = sRet.Replace(vbNullChar, " ")
      If sRet.Contains(" ") Then sRet = sRet.Trim
      sRet = sRet.Trim
      Return sRet
    End Function
    Private Function ReadBByte(inBytes() As Byte) As Byte
      Dim sRet As String = ReadBString(inBytes)
      If Not String.IsNullOrEmpty(sRet) Then
        Return Byte.Parse(sRet, Globalization.CultureInfo.InvariantCulture)
      Else
        Return 0
      End If
    End Function
    Private Function ReadBInt(inBytes() As Byte) As UInt32
      Dim sRet As String = ReadBString(inBytes)
      If Not String.IsNullOrEmpty(sRet) Then
        Return UInt32.Parse(sRet, Globalization.CultureInfo.InvariantCulture)
      Else
        Return 0
      End If
    End Function
    Private Function ReadBOct(inBytes() As Byte) As UInt64
      Dim sRet As String = ReadBString(inBytes)
      If Not String.IsNullOrEmpty(sRet) Then
        Return Convert.ToUInt64(sRet, 8)
      Else
        Return 0
      End If
    End Function
  End Class
  Private Sub ExtractTar(sTAR As String, sDestPath As String)
    If Not My.Computer.FileSystem.DirectoryExists(sDestPath) Then My.Computer.FileSystem.CreateDirectory(sDestPath)
    Using sourceTAR As New IO.FileStream(sTAR, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
      Dim binTar As New IO.BinaryReader(sourceTAR)
      Do While binTar.BaseStream.Position < binTar.BaseStream.Length
        Dim tarFile As New TarFileData(binTar)
        If Not String.IsNullOrEmpty(tarFile.FileName) Then
          If tarFile.LinkIndicator = 0 Then
            My.Computer.FileSystem.WriteAllBytes(IO.Path.Combine(sDestPath, tarFile.FileName), tarFile.FileData, False)
            IO.File.SetLastWriteTime(IO.Path.Combine(sDestPath, tarFile.FileName), New Date(1970, 1, 1).AddSeconds(tarFile.LastMod))
          End If
        End If
      Loop
    End Using
  End Sub

  Public Sub MakeNotifier(ByRef taskNotifier As TaskbarNotifier, ContentClickable As Boolean, Optional customStyle As NotifierStyle = Nothing)
    If customStyle Is Nothing Then customStyle = NOTIFIER_STYLE
    If customStyle.Background Is Nothing Or customStyle.CloseButton Is Nothing Then
      taskNotifier = Nothing
      Return
    End If
    If taskNotifier Is Nothing Then taskNotifier = New TaskbarNotifier
    taskNotifier.TitleClickable = False
    taskNotifier.ContentClickable = ContentClickable
    taskNotifier.TitleRectangle = customStyle.TitleLocation
    taskNotifier.ContentRectangle = customStyle.ContentLocation
    taskNotifier.EnableSelectionRectangle = False
    taskNotifier.AllowTransparency = True
    taskNotifier.TransparencyKey = customStyle.TransparencyKey
    taskNotifier.SetBackgroundBitmap(customStyle.Background, customStyle.TransparencyKey)
    taskNotifier.SetCloseBitmap(customStyle.CloseButton, customStyle.TransparencyKey, customStyle.CloseLocation)
    taskNotifier.NormalTitleColor = customStyle.TitleColor
    taskNotifier.NormalContentColor = customStyle.ContentColor
    taskNotifier.HoverContentColor = customStyle.ContentHoverColor
  End Sub
#End Region
  Public ReadOnly Property IsSongPlaying As Boolean
    Get
      Return Song IsNot Nothing
    End Get
  End Property
  Public Sub ToggleSong()
    If Song Is Nothing Then
      PlaySong()
    Else
      StopSong()
    End If
  End Sub
  Public Sub PlaySong()
    If Song Is Nothing Then
      My.Computer.FileSystem.WriteAllBytes(IO.Path.Combine(LocalAppDataDirectory, "Song.mid"), My.Resources.Song, False)
      Song = New MCIPlayer(IO.Path.Combine(LocalAppDataDirectory, "Song.mid"))
      Song.Play()
    End If
  End Sub
  Public Sub StopSong()
    If Song IsNot Nothing Then
      Song.Stop()
      Song.Close()
      Song.CloseAll()
      Song.Dispose()
      Song = Nothing
    End If
    If My.Computer.FileSystem.FileExists(IO.Path.Combine(LocalAppDataDirectory, "Song.mid")) Then
      Try
        My.Computer.FileSystem.DeleteFile(IO.Path.Combine(LocalAppDataDirectory, "Song.mid"))
      Catch ex As Exception
      End Try
    End If
  End Sub
  Public ReadOnly Property AppDataPath As String
    Get
      Return IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.CompanyName, My.Application.Info.ProductName)
    End Get
  End Property
  Public ReadOnly Property AppDataAllPath As String
    Get
      Return IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Application.CompanyName, My.Application.Info.ProductName)
    End Get
  End Property
  Public ReadOnly Property CommonAppDataDirectory As String
    Get
      Static sTmp As String
      Static OneAlert As Boolean
      If Not My.Computer.FileSystem.DirectoryExists(IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Application.CompanyName)) Then My.Computer.FileSystem.CreateDirectory(IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Application.CompanyName))
      If Not My.Computer.FileSystem.DirectoryExists(AppDataAllPath) Then My.Computer.FileSystem.CreateDirectory(AppDataAllPath)
      If String.IsNullOrEmpty(sTmp) Then sTmp = AppDataAllPath

      Dim AppDataDir As String = IO.Path.GetDirectoryName(sTmp)
      Dim ADDOK As Boolean = GrantFullControlToEveryone(AppDataDir)
      Dim ADOK As Boolean = GrantFullControlToEveryone(sTmp)
      If ADOK And ADDOK Then

      ElseIf ADOK Then

      ElseIf ADDOK Then
        If Not OneAlert Then
          MsgDlg(Nothing, "Failed to set permissions for the directory """ & sTmp & """." & vbNewLine & vbNewLine & "Please run " & My.Application.Info.ProductName & " as Administrator to correct this problem.", "Unable to set folder permissions", "Permissions Error", MessageBoxButtons.OK, _TaskDialogIcon.UserFolder, MessageBoxIcon.Error)
          OneAlert = True
        End If
      Else
        If Not OneAlert Then
          MsgDlg(Nothing, "Failed to set permissions for the directory """ & sTmp & """ and its parent." & vbNewLine & vbNewLine & "Please run " & My.Application.Info.ProductName & " as Administrator to correct this problem.", "Unable to set folder permissions", "Permissions Error", MessageBoxButtons.OK, _TaskDialogIcon.UserFolder, MessageBoxIcon.Error)
          OneAlert = True
        End If
      End If
      Return sTmp
    End Get
  End Property
  Public ReadOnly Property LocalAppDataDirectory As String
    Get
      Static sTmp As String
      If Application.StartupPath.Contains(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)) Or Not My.Computer.FileSystem.DirectoryExists(IO.Path.Combine(Application.StartupPath, "Config")) Then
        If Not My.Computer.FileSystem.DirectoryExists(IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.CompanyName)) Then My.Computer.FileSystem.CreateDirectory(IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.CompanyName))
        If Not My.Computer.FileSystem.DirectoryExists(AppDataPath) Then My.Computer.FileSystem.CreateDirectory(AppDataPath)
        If String.IsNullOrEmpty(sTmp) Then sTmp = AppDataPath
      Else
        If String.IsNullOrEmpty(sTmp) Then
          sTmp = IO.Path.Combine(Application.StartupPath, "Config")
        End If
      End If
      Return sTmp
    End Get
  End Property
  Public ReadOnly Property UpdateParam As String
    Get
      If LocalAppDataDirectory = IO.Path.Combine(Application.StartupPath, "Config") Then
        Return "/silent /noicons /dir=""" & Application.StartupPath & """ /type=portable"
      Else
        Return "/silent /noicons"
      End If
    End Get
  End Property
  Public ReadOnly Property MySaveDir(Optional Create As Boolean = False) As String
    Get
      Dim mySettings As New AppSettings
      If Application.StartupPath.Contains(Environment.SpecialFolder.ProgramFiles) Or Not My.Computer.FileSystem.DirectoryExists(IO.Path.Combine(Application.StartupPath, "Config")) Then
        If String.IsNullOrEmpty(mySettings.HistoryDir) Then
          If My.Computer.FileSystem.DirectoryExists(AppDataPath) Then
            If Array.Exists(My.Computer.FileSystem.GetFiles(AppDataPath).ToArray, Function(appFile As String) IO.Path.GetExtension(appFile).ToUpperInvariant = ".XML" Or IO.Path.GetExtension(appFile).ToUpperInvariant = ".WB") Then
              mySettings.HistoryDir = IIf(Create, LocalAppDataDirectory, AppDataPath)
            Else
              mySettings.HistoryDir = IIf(Create, CommonAppDataDirectory, AppDataAllPath)
            End If
          Else
            mySettings.HistoryDir = IIf(Create, CommonAppDataDirectory, AppDataAllPath)
          End If
        End If
      Else
        mySettings.HistoryDir = IO.Path.Combine(Application.StartupPath, "Config")
      End If
      If Create Then
        Try
          If Not My.Computer.FileSystem.DirectoryExists(mySettings.HistoryDir) Then My.Computer.FileSystem.CreateDirectory(mySettings.HistoryDir)
        Catch ex As Exception
          Return CommonAppDataDirectory
        End Try
      End If
      Return mySettings.HistoryDir
    End Get
  End Property
  Public ReadOnly Property MonospaceFont(Size As Single) As Font
    Get
      Try
        If Drawing.FontFamily.GenericMonospace.IsStyleAvailable(FontStyle.Regular) Then
          Return New Font(Drawing.FontFamily.GenericMonospace, Size)
        Else
          Dim FontList As New Collections.Generic.List(Of String)
          For Each fam In Drawing.FontFamily.Families
            If fam.IsStyleAvailable(FontStyle.Regular) Then FontList.Add(fam.Name)
          Next
          If FontList.Contains("Courier New") Then
            Return New Font("Courier New", Size)
          ElseIf FontList.Contains("Consolas") Then
            Return New Font("Consolas", Size)
          ElseIf FontList.Contains("Lucida Console") Then
            Return New Font("Lucida Console", Size)
          Else
            Return New Font(SystemFonts.DefaultFont.Name, Size)
          End If
        End If
      Catch ex As Exception
        Return New Font(SystemFonts.DefaultFont.Name, Size)
      End Try
    End Get
  End Property
  Friend Function GrantFullControlToEveryone(ByVal Folder As String) As Boolean
    Try
      Dim Security As System.Security.AccessControl.DirectorySecurity = Directory.GetAccessControl(Folder)
      Dim Sid As New System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, Nothing)
      Dim Account As System.Security.Principal.NTAccount = TryCast(Sid.Translate(GetType(System.Security.Principal.NTAccount)), System.Security.Principal.NTAccount)
      Dim Grant As New System.Security.AccessControl.FileSystemAccessRule(Account, System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.InheritanceFlags.ContainerInherit Or System.Security.AccessControl.InheritanceFlags.ObjectInherit, System.Security.AccessControl.PropagationFlags.None, System.Security.AccessControl.AccessControlType.Allow)
      Security.AddAccessRule(Grant)
      Directory.SetAccessControl(Folder, Security)
      Return True
    Catch ex As Exception
      Return False
    End Try
  End Function
  Public Function ByteSize(ByVal InBytes As UInt64) As String
    If InBytes < 1000 Then Return InBytes & " B"
    If InBytes / 1024 < 1000 Then Return Format((InBytes) / 1024, "0.#") & " KB"
    If InBytes / 1024 / 1024 < 1000 Then Return Format((InBytes) / 1024 / 1024, "0.##") & " MB"
    If InBytes / 1024 / 1024 / 1024 < 1000 Then Return Format((InBytes) / 1024 / 1024 / 1024, "0.##") & " GB"
    Return Format((InBytes) / 1024 / 1024 / 1024 / 1024, "0.##") & " TB"
  End Function
  Public Function ConvertTime(ByVal lngMS As UInt64, Optional ByVal Abbreviated As Boolean = False, Optional ByVal Trimmed As Boolean = True) As String
    Dim lngSeconds As UInt64 = lngMS \ 1000
    Dim lngWeeks As UInt64 = lngSeconds \ (60 * 60 * 24 * 7)
    lngSeconds = lngSeconds Mod (60 * 60 * 24 * 7)
    Dim lngDays As UInt64 = lngSeconds \ (60 * 60 * 24)
    lngSeconds = lngSeconds Mod (60 * 60 * 24)
    Dim lngHours As UInt64 = lngSeconds \ (60 * 60)
    lngSeconds = lngSeconds Mod (60 * 60)
    Dim lngMins As UInt64 = lngSeconds \ 60
    lngSeconds = lngSeconds Mod 60
    If Abbreviated Then
      If Trimmed Then
        If lngWeeks > 0 Then
          Return lngWeeks & "w " & lngDays & "d"
        ElseIf lngDays > 0 Then
          If lngHours > 20 Then
            If lngDays >= 6 Then
              Return "1 w"
            Else
              Return lngDays + 1 & " d"
            End If
          Else
            Return lngDays & IIf(lngHours > 14, "¾ d", IIf(lngHours > 8, "½ d", IIf(lngHours > 2, "¼ d", " d")))
          End If
        ElseIf lngHours > 0 Then
          If lngHours >= 22 Or (lngHours = 21 And lngMins > 50) Then
            Return "1 d"
          ElseIf lngMins > 50 Then
            Return lngHours + 1 & " h"
          Else
            Return lngHours & IIf(lngMins > 35, "¾ h", IIf(lngMins > 20, "½ h", IIf(lngMins > 5, "¼ h", " h")))
          End If
        ElseIf lngMins > 0 Then
          If lngMins >= 55 Or (lngMins = 54 And lngSeconds > 50) Then
            Return "1 h"
          ElseIf lngSeconds > 50 Then
            Return lngMins + 1 & " m"
          Else
            Return lngMins & IIf(lngSeconds > 35, "¾ m", IIf(lngSeconds > 20, "½ m", IIf(lngSeconds > 5, "¼ m", " m")))
          End If
        Else
          If lngSeconds > 55 Then
            Return "1 m"
          Else
            Return lngSeconds & "s"
          End If
        End If
      Else
        If lngWeeks > 0 Then
          Return lngWeeks & "w " & lngDays & "d " & lngHours & "h " & lngMins & "m " & lngSeconds & "s"
        ElseIf lngDays > 0 Then
          Return lngDays & "d " & lngHours & "h " & lngMins & "m " & lngSeconds & "s"
        ElseIf lngHours > 0 Then
          Return lngHours & "h " & lngMins & "m " & lngSeconds & "s"
        ElseIf lngMins > 0 Then
          Return lngMins & "m " & lngSeconds & "s"
        Else
          Return lngSeconds & "s"
        End If
      End If
    Else
      Dim strWeeks As String = IIf(lngWeeks = 1, vbNullString, "s")
      Dim strDays As String = IIf(lngDays = 1, vbNullString, "s")
      Dim strHours As String = IIf(lngHours = 1, vbNullString, "s")
      Dim strMins As String = IIf(lngMins = 1, vbNullString, "s")
      Dim strSeconds As String = IIf(lngSeconds = 1, vbNullString, "s")
      If Trimmed Then
        If lngWeeks > 0 Then
          Return lngWeeks & " Week" & strWeeks & " and " & lngDays & " Day" & strDays
        ElseIf lngDays > 0 Then
          If lngHours > 20 Then
            If lngDays >= 6 Then
              Return "1 Week"
            Else
              Return lngDays + 1 & " Days"
            End If
          Else
            Return lngDays & IIf(lngHours > 14, " and Three Quarter Days", IIf(lngHours > 8, " and a Half Days", IIf(lngHours > 2, " and a Quarter Days", " Day" & strDays)))
          End If
        ElseIf lngHours > 0 Then
          If lngHours >= 22 Or (lngHours = 21 And lngMins > 50) Then
            Return "1 Day"
          ElseIf lngMins > 50 Then
            Return lngHours + 1 & " Hours"
          Else
            Return lngHours & IIf(lngMins > 35, " and Three Quarter Hours", IIf(lngMins > 20, " and a Half Hours", IIf(lngMins > 5, " and a Quarter Hours", " Hour" & strHours)))
          End If
        ElseIf lngMins > 0 Then
          If lngMins >= 55 Or (lngMins = 54 And lngSeconds > 55) Then
            Return "1 Hour"
          ElseIf lngSeconds > 50 Then
            Return lngMins + 1 & " Minutes"
          Else
            Return lngMins & IIf(lngSeconds > 35, " and Three Quarter Minutes", IIf(lngSeconds > 20, " and a Half Minutes", IIf(lngSeconds > 5, " and a Quarter Minutes", " Minute" & strMins)))
          End If
        Else
          If lngSeconds > 55 Then
            Return "1 Minute"
          Else
            Return lngSeconds & "Second" & strSeconds
          End If
        End If
      Else
        If lngWeeks > 0 Then
          Return lngWeeks & " Week" & strWeeks & ", " & lngDays & " Day" & strDays & ", " & lngHours & " Hour" & strHours & ", " & lngMins & " Minute" & strMins & ", and " & lngSeconds & " Second" & strSeconds
        ElseIf lngDays > 0 Then
          Return lngDays & " Day" & strDays & ", " & lngHours & " Hour" & strHours & ", " & lngMins & " Minute" & strMins & ", and " & lngSeconds & " Second" & strSeconds
        ElseIf lngHours > 0 Then
          Return lngHours & " Hour" & strHours & ", " & lngMins & " Minute" & strMins & ", and " & lngSeconds & " Second" & strSeconds
        ElseIf lngMins > 0 Then
          Return lngMins & " Minute" & strMins & " and " & lngSeconds & " Second" & strSeconds
        Else
          Return lngSeconds & " Second" & strSeconds
        End If
      End If
    End If
  End Function
  Public ReadOnly Property StartupPath As String
    Get
      Static sTmp As String
      If String.IsNullOrEmpty(sTmp) Then
        If My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) & " with Internet Access") Then
          sTmp = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup) & " with Internet Access", "Satellite Restriction Tracker.lnk")
        Else
          sTmp = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "Satellite Restriction Tracker.lnk")
        End If
      End If
      Return sTmp
    End Get
  End Property
  Public Function DisplayVersion(sVersion As String) As String
    Dim ApplicationVersion As String = sVersion
    While Not String.IsNullOrEmpty(ApplicationVersion) AndAlso ApplicationVersion.Length > 3 AndAlso ApplicationVersion.EndsWith(".0")
      ApplicationVersion = ApplicationVersion.Substring(0, ApplicationVersion.Length - 2)
    End While
    If String.IsNullOrEmpty(ApplicationVersion) Then
      Return sVersion
    Else
      Return ApplicationVersion
    End If
  End Function
  Public Function CompareVersions(sRemote As String) As Boolean
    Dim sLocal As String = Application.ProductVersion
    Dim LocalVer(3) As String
    If sLocal.Contains(".") Then
      For I As Integer = 0 To 3
        If sLocal.Split(".").Count > I Then
          Dim sTmp As String = sLocal.Split(".")(I).Trim
          If IsNumeric(sTmp) And sTmp.Length < 4 Then sTmp &= StrDup(4 - sTmp.Length, "0"c)
          LocalVer(I) = sTmp
        Else
          LocalVer(I) = "0000"
        End If
      Next
    ElseIf sLocal.Contains(",") Then
      For I As Integer = 0 To 3
        If sLocal.Split(",").Count > I Then
          Dim sTmp As String = sLocal.Split(",")(I).Trim
          If IsNumeric(sTmp) And sTmp.Length < 4 Then sTmp &= StrDup(4 - sTmp.Length, "0"c)
          LocalVer(I) = sTmp
        Else
          LocalVer(I) = "0000"
        End If
      Next
    End If
    Dim RemoteVer(3) As String
    If sRemote.Contains(".") Then
      For I As Integer = 0 To 3
        If sRemote.Split(".").Count > I Then
          Dim sTmp As String = sRemote.Split(".")(I).Trim
          If IsNumeric(sTmp) And sTmp.Length < 4 Then sTmp &= StrDup(4 - sTmp.Length, "0"c)
          RemoteVer(I) = sTmp
        Else
          RemoteVer(I) = "0000"
        End If
      Next
    ElseIf sRemote.Contains(",") Then
      For I As Integer = 0 To 3
        If sRemote.Split(",").Count > I Then
          Dim sTmp As String = sRemote.Split(",")(I).Trim
          If IsNumeric(sTmp) And sTmp.Length < 4 Then sTmp &= StrDup(4 - sTmp.Length, "0"c)
          RemoteVer(I) = sTmp
        Else
          RemoteVer(I) = "0000"
        End If
      Next
    End If
    Dim bUpdate As Boolean = False
    If Val(LocalVer(0)) > Val(RemoteVer(0)) Then

    ElseIf Val(LocalVer(0)) = Val(RemoteVer(0)) Then
      If Val(LocalVer(1)) > Val(RemoteVer(1)) Then

      ElseIf Val(LocalVer(1)) = Val(RemoteVer(1)) Then
        If Val(LocalVer(2)) > Val(RemoteVer(2)) Then

        ElseIf Val(LocalVer(2)) = Val(RemoteVer(2)) Then
          If Val(LocalVer(3)) >= Val(RemoteVer(3)) Then

          Else
            bUpdate = True
          End If
        Else
          bUpdate = True
        End If
      Else
        bUpdate = True
      End If
    Else
      bUpdate = True
    End If
    Return bUpdate
  End Function
  Public Sub ShellEx(FilePath As String, Arguments As String)
    Dim oProc As New Process
    oProc.StartInfo.FileName = FilePath
    oProc.StartInfo.Arguments = Arguments
    oProc.StartInfo.WindowStyle = ProcessWindowStyle.Normal
    oProc.StartInfo.UseShellExecute = False
    oProc.Start()
  End Sub
  Public Delegate Sub FailResponseInvoker(sRet As String)
  Public Sub SaveToFTP(oData As Object)
    Dim sData As String = oData(0)
    Dim callback As FailResponseInvoker = oData(1)
    Try
      Dim bData As Byte() = System.Text.Encoding.UTF8.GetBytes(sData)
      Dim sBase64Data As String = Convert.ToBase64String(bData, Base64FormattingOptions.None)
      Dim sckUpload As New WebClientEx()
      sckUpload.KeepAlive = False
      Dim params As New Collections.Specialized.NameValueCollection
      params.Add("eFile", sBase64Data)
      Dim sRet As String = sckUpload.UploadValues("http://wb.realityripple.com/errmsgs.php", "POST", params)
      If sRet = "e exists" Then
        callback.Invoke("exists")
      ElseIf sRet = "e added" Then
        callback.Invoke("added")
      Else
        callback.Invoke("error")
      End If
    Catch ex As Exception
      callback.Invoke("error")
    End Try
  End Sub
  Public Function CopyDirectory(FromDir As String, ToDir As String) As TriState
    If My.Computer.FileSystem.DirectoryExists(FromDir) Then
      Dim bDidSomething As Boolean = False
      If My.Computer.FileSystem.DirectoryExists(ToDir) Then
        Dim wbFiles As Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(FromDir)
        If wbFiles.Count > 0 Then
          Dim srtFiles As Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(ToDir)
          Dim spareFiles As Collections.ObjectModel.Collection(Of String)
          If srtFiles.Count > 0 Then
            spareFiles = New Collections.ObjectModel.Collection(Of String)
            For I As Integer = 0 To wbFiles.Count - 1
              Dim isUnique As Boolean = True
              For J As Integer = 0 To srtFiles.Count - 1
                If IO.Path.GetFileName(srtFiles(J)) = IO.Path.GetFileName(wbFiles(I)) Then
                  isUnique = False
                  Exit For
                End If
              Next
              If isUnique Then spareFiles.Add(wbFiles(I))
            Next
          Else
            spareFiles = New Collections.ObjectModel.Collection(Of String)(srtFiles.ToArray)
          End If
          If spareFiles.Count > 0 Then
            My.Computer.FileSystem.CreateDirectory(ToDir)
            For I As Integer = 0 To spareFiles.Count - 1
              Dim file As String = spareFiles(I)
              Dim sFName As String = IO.Path.GetFileName(file)
              My.Computer.FileSystem.CopyFile(file, IO.Path.Combine(ToDir, sFName), True)
              bDidSomething = True
            Next
          End If
        Else

        End If
      Else
        Dim wbFiles As Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(FromDir)
        If wbFiles.Count > 0 Then
          My.Computer.FileSystem.CreateDirectory(ToDir)
          For I As Integer = 0 To wbFiles.Count - 1
            Dim file As String = wbFiles(I)
            Dim sFName As String = IO.Path.GetFileName(file)
            My.Computer.FileSystem.CopyFile(file, IO.Path.Combine(ToDir, sFName), True)
            bDidSomething = True
          Next
        End If
      End If
      If Not bDidSomething Then Return TriState.UseDefault
      Dim wFileTmp As Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(FromDir)
      Dim sFileTmp As Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(ToDir)
      Dim Equal As Boolean = True
      If wFileTmp.Count = sFileTmp.Count Then
        For I As Integer = 0 To wFileTmp.Count - 1
          If (IO.Path.GetFileName(wFileTmp(I)) = IO.Path.GetFileName(sFileTmp(I))) And (My.Computer.FileSystem.GetFileInfo(wFileTmp(I)).Length = My.Computer.FileSystem.GetFileInfo(sFileTmp(I)).Length) Then
            Continue For
          Else
            Equal = False
            Exit For
          End If
        Next
      Else
        Equal = False
      End If
      Return IIf(Equal, TriState.True, TriState.False)
    Else
      Return TriState.UseDefault
    End If
  End Function
  Public Function isAdmin() As Boolean
    Dim id As Security.Principal.WindowsIdentity = Security.Principal.WindowsIdentity.GetCurrent
    Dim p As New Security.Principal.WindowsPrincipal(id)
    Return p.IsInRole(Security.Principal.WindowsBuiltInRole.Administrator)
  End Function
  Public ReadOnly Property MinAnimation As Boolean
    Get
      Dim ai As NativeMethods.ANIMATIONINFO
      ai.Size = Len(ai)
      NativeMethods.SystemParametersInfo(NativeMethods.SPI_GETANIMATION, ai.Size, ai, 0UI)
      MinAnimation = ai.MinAnimate
    End Get
  End Property
  Public Function GetWindowAnimationData(window As Form) As WindowAnimationData
    If Not MinAnimation Then Return Nothing
    Dim wad As New WindowAnimationData
    wad.startRect = window.Bounds
    Dim screenRect = Screen.GetBounds(wad.startRect.Location)
    wad.destPoint = New Point(screenRect.Width - 96, screenRect.Height - 16)
    Select Case TaskBarPosition.GetTaskBarEdge(window.Handle)
      Case NativeMethods.ABEdge.ABE_TOP
        wad.destPoint = New Point(screenRect.Width - 96, 16)
      Case NativeMethods.ABEdge.ABE_LEFT
        wad.destPoint = New Point(16, screenRect.Height - 96)
      Case NativeMethods.ABEdge.ABE_RIGHT
        wad.destPoint = New Point(screenRect.Width - 16, screenRect.Height - 96)
    End Select
    wad.backColor = window.BackColor
    Return wad
  End Function
  Public Sub AnimateWindow(wnd As Form, ToTray As Boolean)
    AnimateWindow(GetWindowAnimationData(wnd), ToTray)
  End Sub
  Public Sub AnimateWindow(wad As WindowAnimationData, ToTray As Boolean)
    If Not MinAnimation Then Return
    Dim a, b, s As Single
    If ToTray Then
      a = 0
      b = 1
      s = 0.4
    Else
      a = 1
      b = 0
      s = -0.4
    End If
    Dim curPoint As Point, curSize As Size
    Dim startPoint As Point = wad.startRect.Location
    Dim dWidth As Integer = wad.destPoint.X - startPoint.X
    Dim dHeight As Integer = wad.destPoint.Y - startPoint.Y
    Dim startWidth As Integer = wad.startRect.Width
    Dim startHeight As Integer = wad.startRect.Height
    For I As Single = a To b Step s
      curPoint = New Point(startPoint.X + I * dWidth, startPoint.Y + I * dHeight)
      curSize = New Size((1 - I) * startWidth, (1 - I) * startHeight)
      Dim drawRect As New Rectangle(curPoint, curSize)
      ControlPaint.DrawReversibleFrame(drawRect, wad.backColor, FrameStyle.Thick)
      System.Threading.Thread.Sleep(1)
      ControlPaint.DrawReversibleFrame(drawRect, wad.backColor, FrameStyle.Thick)
    Next
  End Sub
#Region "Graphs"
#Region "History"
  Private dGraph, uGraph As Rectangle
  Private oldDate, newDate As Date
  Private dData(), uData() As DataBase.DataRow
  Private Enum Direction
    No
    Down
    Up
  End Enum
  Public Function GetGraphRect(DownGraph As Boolean, ByRef firstX As Date, ByRef lastX As Date) As Rectangle
    firstX = oldDate
    lastX = newDate
    If DownGraph Then
      Return dGraph
    Else
      Return uGraph
    End If
  End Function
  Friend Function GetGraphData(fromDate As Date, DownGraph As Boolean) As DataBase.DataRow
    If DownGraph Then
      If dData Is Nothing Then Return DataBase.DataRow.Empty
      Dim closestRow As DataBase.DataRow
      Dim closestVal As Integer = Integer.MaxValue
      For Each item In dData
        If Math.Abs(DateDiff(DateInterval.Minute, item.DATETIME, fromDate)) < closestVal Then
          closestRow = item
          closestVal = Math.Abs(DateDiff(DateInterval.Minute, item.DATETIME, fromDate))
        End If
      Next
      Return closestRow
    Else
      If uData Is Nothing Then Return DataBase.DataRow.Empty
      Dim closestRow As DataBase.DataRow
      Dim closestVal As Integer = Integer.MaxValue
      For Each item In uData
        If Math.Abs(DateDiff(DateInterval.Minute, item.DATETIME, fromDate)) < closestVal Then
          closestRow = item
          closestVal = Math.Abs(DateDiff(DateInterval.Minute, item.DATETIME, fromDate))
        End If
      Next
      Return closestRow
    End If
  End Function
  Public Function DrawRGraph(Data() As DataBase.DataRow, ImgSize As Size, ColorLine As Color, ColorA As Color, ColorB As Color, ColorC As Color, ColorText As Color, ColorBG As Color, ColorMax As Color, ColorGridLight As Color, ColorGridDark As Color) As Image
    Return DrawGraph(Data, Direction.No, ImgSize, ColorLine, ColorA, ColorB, ColorC, ColorText, ColorBG, ColorMax, ColorGridLight, ColorGridDark)
  End Function
  Public Function DrawLineGraph(Data() As DataBase.DataRow, Down As Boolean, ImgSize As Size, ColorLine As Color, ColorA As Color, ColorB As Color, ColorC As Color, ColorText As Color, ColorBG As Color, ColorMax As Color, ColorGridLight As Color, ColorGridDark As Color) As Image
    Return DrawGraph(Data, IIf(Down, Direction.Down, Direction.Up), ImgSize, ColorLine, ColorA, ColorB, ColorC, ColorText, ColorBG, ColorMax, ColorGridLight, ColorGridDark)
  End Function
  Private Function DrawGraph(ByVal Data() As DataBase.DataRow, GraphDir As Direction, ImgSize As Size, ColorLine As Color, ColorA As Color, ColorB As Color, ColorC As Color, ColorText As Color, ColorBG As Color, ColorMax As Color, ColorGridLight As Color, ColorGridDark As Color) As Image
    If Data Is Nothing OrElse Data.Length = 0 Then
      dData = Nothing
      uData = Nothing
      Return New Bitmap(1, 1)
    End If
    Dim yMax As Long = 0
    Dim lMax As Long = 0
    If GraphDir = Direction.No Then
      Dim yVMax As Long = -1
      For I As Integer = 0 To Data.Length - 1
        If yVMax < Data(I).DOWNLOAD Then yVMax = Data(I).DOWNLOAD
        If yVMax < Data(I).DOWNLIM Then yVMax = Data(I).DOWNLIM
      Next
      If yVMax = -1 Then yVMax = 0
      yMax = yVMax
      If Not yMax Mod 1000 = 0 Then yMax = (yMax \ 1000) * 1000
      lMax = yVMax
    Else
      Dim yDMax As Long = -1
      Dim yUMax As Long = -1
      For I As Integer = 0 To Data.Length - 1
        If yDMax < Data(I).DOWNLOAD Then yDMax = Data(I).DOWNLOAD
        If yUMax < Data(I).UPLOAD Then yUMax = Data(I).UPLOAD
        If yDMax < Data(I).DOWNLIM Then yDMax = Data(I).DOWNLIM
        If yUMax < Data(I).UPLIM Then yUMax = Data(I).UPLIM
      Next
      If yDMax = -1 Then yDMax = 0
      If yUMax = -1 Then yUMax = 0
      yMax = IIf(yDMax > yUMax, yDMax, yUMax)
      If Not yMax Mod 1000 = 0 Then yMax = (yMax \ 1000) * 1000
      lMax = IIf(GraphDir = Direction.Down, yDMax, yUMax)
    End If
    If Not lMax Mod 1000 = 0 Then lMax = (lMax \ 1000) * 1000 + 1000
    Dim iPic As Image = New Bitmap(ImgSize.Width, ImgSize.Height)
    Dim g As Graphics = Graphics.FromImage(iPic)
    Dim tFont As New Font(FontFamily.GenericSansSerif, 7)
    g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
    Dim lYWidth As Integer = g.MeasureString(yMax.ToString(Globalization.CultureInfo.InvariantCulture) & " MB", tFont).Width + 10
    Dim lXHeight As Integer = g.MeasureString(srlFunctions.TimeToString(Now), tFont).Height + 10
    Dim lLineWidth As Long = (ImgSize.Width - 4) - lYWidth - 1
    g.Clear(ColorBG)
    Dim yTop As Integer = lXHeight / 2
    Dim yHeight As Integer = ImgSize.Height - (lXHeight * 1.5)
    If GraphDir = Direction.Up Then
      uGraph = New Rectangle(lYWidth, yTop, lLineWidth + 1, yHeight)
      uData = Data
    Else
      dGraph = New Rectangle(lYWidth, yTop, lLineWidth + 1, yHeight)
      dData = Data
    End If
    g.DrawLine(New Pen(ColorText), lYWidth, yTop, lYWidth, yTop + yHeight)
    g.DrawLine(New Pen(ColorText), lYWidth, yTop + yHeight, ImgSize.Width, yTop + yHeight)
    oldDate = Data.First.DATETIME
    newDate = Data.Last.DATETIME
    For I As Integer = 0 To lMax Step (((lMax \ (yHeight \ (tFont.Size + 12)))) \ 100) * 100
      Dim iY As Integer = yTop + yHeight - (I / lMax * yHeight)
      g.DrawString(I.ToString(Globalization.CultureInfo.InvariantCulture) & " MB", tFont, New SolidBrush(ColorText), lYWidth - g.MeasureString(I.ToString(Globalization.CultureInfo.InvariantCulture) & " MB", tFont).Width, iY - (g.MeasureString(I.ToString(Globalization.CultureInfo.InvariantCulture) & " MB", tFont).Height / 2))
      g.DrawLine(New Pen(ColorText), lYWidth - 3, iY, lYWidth, iY)
    Next I
    For I As Integer = 0 To lMax Step (lMax \ 10)
      Dim iY As Integer = yTop + yHeight - (I / lMax * yHeight)
      If I > 0 Then
        g.DrawLine(New Pen(ColorGridLight), lYWidth + 1, iY, ImgSize.Width - 4, iY)
      End If
    Next
    For I As Integer = 0 To lMax Step (lMax \ 5)
      Dim iY As Integer = yTop + yHeight - (I / lMax * yHeight)
      If I > 0 Then
        g.DrawLine(New Pen(ColorGridDark), lYWidth + 1, iY, ImgSize.Width - 4, iY)
      End If
    Next
    Dim lStart As Date = Data(0).DATETIME
    Dim lEnd As Date = Data(Data.Length - 1).DATETIME
    Dim dAxisInterval As DateInterval = DateInterval.Minute
    Dim lAxisInterval As UInteger = 1
    Dim lAxisSubInterval As Double = 1.0
    Dim lAxisLabelInterval As UInteger = 5
    Dim lMaxTime As Long = Math.Abs(DateDiff(DateInterval.Second, lStart, lEnd))
    Dim dPPS As Double = lLineWidth / lMaxTime
    Dim dSPP As Double = 1 / dPPS
    Dim dGraphInterval As DateInterval = DateInterval.Second
    Dim lGraphInterval As UInteger = Math.Ceiling(dSPP)
    If lGraphInterval >= 60 Then
      dGraphInterval = DateInterval.Minute
      lGraphInterval = Math.Floor(lGraphInterval / 60)
      If lGraphInterval >= 60 Then
        dGraphInterval = DateInterval.Hour
        lGraphInterval = Math.Floor(lGraphInterval / 60)
        If lGraphInterval >= 24 Then
          dGraphInterval = DateInterval.Day
          lGraphInterval = Math.Floor(lGraphInterval / 24)
        End If
      End If
    End If
    Dim sDispV As String = "g"
    Select Case Math.Abs(DateDiff(DateInterval.Minute, lStart, lEnd))
      Case Is <= 60
        lAxisInterval = 5
        lAxisSubInterval = 2.5
        lAxisLabelInterval = 10
        dAxisInterval = DateInterval.Minute
        sDispV = "t"
      Case Is <= 60 * 12
        lAxisInterval = 60
        lAxisSubInterval = 30
        lAxisLabelInterval = 120
        dAxisInterval = DateInterval.Minute
        sDispV = "t"
      Case Is <= 60 * 24
        lAxisInterval = 96
        lAxisSubInterval = 48
        lAxisLabelInterval = 192
        dAxisInterval = DateInterval.Minute
        sDispV = "t"
      Case Is <= 60 * 24 * 2
        lAxisInterval = 4
        lAxisSubInterval = 2
        lAxisLabelInterval = 12
        dAxisInterval = DateInterval.Hour
        sDispV = "g"
      Case Is <= 60 * 24 * 3
        lAxisInterval = 6
        lAxisSubInterval = 3
        lAxisLabelInterval = 18
        dAxisInterval = DateInterval.Hour
        sDispV = "g"
      Case Is <= 60 * 24 * 5
        lAxisInterval = 12
        lAxisSubInterval = 6
        lAxisLabelInterval = 24
        dAxisInterval = DateInterval.Hour
        sDispV = "d"
      Case Is <= 60 * 24 * 7
        lAxisInterval = 12
        lAxisSubInterval = 6
        lAxisLabelInterval = 24
        dAxisInterval = DateInterval.Hour
        sDispV = "d"
      Case Is <= 60 * 24 * 13
        lAxisInterval = 24
        lAxisSubInterval = 12
        lAxisLabelInterval = 48
        dAxisInterval = DateInterval.Hour
        sDispV = "d"
      Case Is <= 60 * 24 * 18
        lAxisInterval = 36
        lAxisSubInterval = 12
        lAxisLabelInterval = 72
        dAxisInterval = DateInterval.Hour
        sDispV = "d"
      Case Is <= 60 * 24 * 20
        lAxisInterval = 2
        lAxisSubInterval = 1
        lAxisLabelInterval = 4
        dAxisInterval = DateInterval.Day
        sDispV = "d"
      Case Is <= 60 * 24 * 27
        lAxisInterval = 3
        lAxisSubInterval = 1
        lAxisLabelInterval = 6
        dAxisInterval = DateInterval.Day
        sDispV = "d"
      Case Is <= 60 * 24 * 29
        lAxisInterval = 3
        lAxisSubInterval = 1.5
        lAxisLabelInterval = 6
        dAxisInterval = DateInterval.Day
        sDispV = "d"
      Case Is <= 60 * 24 * 55
        lAxisInterval = 4
        lAxisSubInterval = 2
        lAxisLabelInterval = 8
        dAxisInterval = DateInterval.Day
        sDispV = "d"
      Case Is <= 60 * 24 * 90
        lAxisInterval = 6
        lAxisSubInterval = 3
        lAxisLabelInterval = 12
        dAxisInterval = DateInterval.Day
        sDispV = "d"
      Case Is <= 60 * 24 * 120
        lAxisInterval = 8
        lAxisSubInterval = 4
        lAxisLabelInterval = 16
        dAxisInterval = DateInterval.Day
        sDispV = "d"
      Case Is <= 60 * 24 * 180
        lAxisInterval = 16
        lAxisSubInterval = 8
        lAxisLabelInterval = 32
        dAxisInterval = DateInterval.Day
        sDispV = "d"
      Case Is <= 60 * 24 * 210
        lAxisInterval = 24
        lAxisSubInterval = 12
        lAxisLabelInterval = 48
        dAxisInterval = DateInterval.Day
        sDispV = "d"
      Case Is <= 60 * 24 * 240
        lAxisInterval = 30
        lAxisSubInterval = 15
        lAxisLabelInterval = 60
        dAxisInterval = DateInterval.Day
        sDispV = "d"
      Case Is <= 60 * 24 * 365 * 2
        lAxisInterval = 60
        lAxisSubInterval = 30
        lAxisLabelInterval = 90
        dAxisInterval = DateInterval.Day
        sDispV = "d"
      Case Else
        lAxisInterval = 90
        lAxisSubInterval = 45
        lAxisLabelInterval = 180
        dAxisInterval = DateInterval.Day
        sDispV = "d"
    End Select
    Dim lMaxAxisTime As Long = Math.Abs(DateDiff(dAxisInterval, lStart, lEnd))
    Dim lMaxGraphTime As Long = Math.Abs(DateDiff(dGraphInterval, lStart, lEnd)) / lGraphInterval
    If lMaxAxisTime = 0 Or lMaxGraphTime = 0 Then Return New Bitmap(1, 1)
    Dim dAxisCompInter As Double = lLineWidth / lMaxAxisTime
    Dim dGraphCompInter As Double = lLineWidth / lMaxGraphTime
    For I As Double = 0 To lMaxAxisTime Step lAxisSubInterval
      Dim lX As Integer = lYWidth + (I * dAxisCompInter) + 1
      If I > 0 Then g.DrawLine(New Pen(ColorGridLight), lX, yTop, lX, ImgSize.Height - lXHeight)
    Next
    For I As Long = 0 To lMaxAxisTime Step lAxisInterval
      Dim lX As Integer = lYWidth + (I * dAxisCompInter) + 1
      If I > 0 Then g.DrawLine(New Pen(ColorText), lX, ImgSize.Height - (lXHeight - 3), lX, ImgSize.Height - lXHeight)
      If I > 0 Then g.DrawLine(New Pen(ColorGridDark), lX, yTop, lX, ImgSize.Height - lXHeight)
    Next I
    Dim lastI As Long = lYWidth + (lMaxAxisTime * dAxisCompInter)
    If lastI >= (ImgSize.Width - 4) Then lastI = (ImgSize.Width - 4)
    Dim sLastDisp As String = lEnd.ToString(sDispV, srlFunctions.DateFormatProvider)
    Dim iLastDispWidth As Single = g.MeasureString(sLastDisp, tFont).Width
    For I As Long = 0 To lMaxAxisTime Step lAxisLabelInterval
      Dim lX As Integer = lYWidth + (I * dAxisCompInter) + 1
      If I = 0 Then lX -= 1
      Dim dDisp As Date = DateAdd(dAxisInterval, I, lStart)
      If Not sDispV = "d" Then
        If dDisp.Minute < 15 Then
          dDisp = New Date(dDisp.Year, dDisp.Month, dDisp.Day, dDisp.Hour, 0, 0)
        ElseIf dDisp.Minute < 30 Then
          dDisp = New Date(dDisp.Year, dDisp.Month, dDisp.Day, dDisp.Hour, 15, 0)
        ElseIf dDisp.Minute < 45 Then
          dDisp = New Date(dDisp.Year, dDisp.Month, dDisp.Day, dDisp.Hour, 30, 0)
        Else
          dDisp = New Date(dDisp.Year, dDisp.Month, dDisp.Day, dDisp.Hour, 45, 0)
        End If
      End If
      Dim sDisp As String = dDisp.ToString(sDispV, srlFunctions.DateFormatProvider)
      If sDisp.Contains(":00") Then sDisp = sDisp.Replace(":00", "")
      g.DrawLine(New Pen(ColorText), lX, ImgSize.Height - (lXHeight - 5), lX, ImgSize.Height - lXHeight)
      If lX >= (ImgSize.Width - (g.MeasureString(sDisp, tFont).Width / 2)) Then
        g.DrawString(sDisp, tFont, New SolidBrush(ColorText), (ImgSize.Width - g.MeasureString(sDisp, tFont).Width), ImgSize.Height - lXHeight + 5)
      ElseIf lX - (g.MeasureString(sDisp, tFont).Width / 2) < lYWidth Then
        g.DrawString(sDisp, tFont, New SolidBrush(ColorText), lYWidth - (g.MeasureString(sDisp, tFont).Width / sDisp.Length), ImgSize.Height - lXHeight + 5)
      Else
        g.DrawString(sDisp, tFont, New SolidBrush(ColorText), lX - (g.MeasureString(sDisp, tFont).Width / 2), ImgSize.Height - lXHeight + 5)
      End If
      If lX >= lastI - (iLastDispWidth * 1.6) Then lastI = -1
    Next I
    If lastI > -1 Then
      g.DrawLine(New Pen(ColorText), lastI, ImgSize.Height - (lXHeight - 5), lastI, ImgSize.Height - lXHeight)
      g.DrawString(sLastDisp, tFont, New SolidBrush(ColorText), lastI - iLastDispWidth + 3, ImgSize.Height - lXHeight + 5)
    End If
    Dim MaxY As Integer = 0
    If GraphDir = Direction.No Then
      MaxY = yTop + yHeight - (Data(Data.Length - 1).DOWNLIM / lMax * yHeight)
    Else
      MaxY = yTop + yHeight - (IIf(GraphDir = Direction.Down, Data(Data.Length - 1).DOWNLIM, Data(Data.Length - 1).UPLIM) / lMax * yHeight)
    End If
    Dim lMaxPoints(lMaxGraphTime) As Point
    Dim lPoints(lMaxGraphTime + 3) As Point
    Dim lTypes(lMaxGraphTime + 3) As Byte
    Dim lastVal As Long = -1
    For I As Long = 0 To lMaxGraphTime
      Dim lVal As Long = -1
      Dim lHigh As Long = -1
      Dim dFind As Date = DateAdd(dGraphInterval, I * lGraphInterval, lStart)
      For J As Integer = 0 To Data.Length - 1
        If Math.Abs(DateDiff(dGraphInterval, Data(J).DATETIME, dFind)) <= lGraphInterval Then
          Dim jLim As Long = -1
          If GraphDir = Direction.Up Then
            jLim = Data(J).UPLIM
          Else
            jLim = Data(J).DOWNLIM
          End If
          If lHigh < jLim Then lHigh = jLim
        End If
      Next
      If lHigh > -1 Then
        lVal = lHigh
      Else
        If I = lMaxGraphTime Then
          If lastVal > -1 Then
            lVal = lastVal
          Else
            lVal = 0
          End If
        Else
          Dim nextHVal As Long = -1
          Dim K As Long = I
          Do Until nextHVal > -1
            K += 1
            If K > lMaxGraphTime Then Exit Do
            dFind = DateAdd(dGraphInterval, K * lGraphInterval, lStart)
            For J As Integer = 0 To Data.Length - 1
              If Math.Abs(DateDiff(dGraphInterval, Data(J).DATETIME, dFind)) <= lGraphInterval Then
                Dim jLim As Long = -1
                If GraphDir = Direction.Up Then
                  jLim = Data(J).UPLIM
                Else
                  jLim = Data(J).DOWNLIM
                End If
                If nextHVal < jLim Then nextHVal = jLim
              End If
            Next
          Loop
          If nextHVal > -1 Then
            If lastVal < nextHVal Then
              lVal = nextHVal
            Else
              lVal = lastVal
            End If
          Else
            lVal = lastVal
          End If
        End If
      End If
      lMaxPoints(I).X = lYWidth + (I * dGraphCompInter) + 1
      lMaxPoints(I).Y = yTop + yHeight - (lVal / lMax * yHeight)
      If I > 0 AndAlso (lMaxPoints(I - 1).X = 0 And lMaxPoints(I - 1).Y = 0) Then
        Dim J As Long = 1
        While lMaxPoints(I - J).Y = 0
          J += 1
        End While
        For K As Long = 1 To J - 1
          lMaxPoints(I - K).X = lYWidth + ((I - K) * dGraphCompInter) + 1
          lMaxPoints(I - K).Y = (lMaxPoints(I - J).Y + lMaxPoints(I).Y) / 2
        Next
      End If
      If lVal > -1 Then lastVal = lVal
    Next I
    lastVal = -1
    For I As Long = 0 To lMaxGraphTime
      Dim lVal As Long = -1
      Dim lLow As Long = Long.MaxValue
      Dim lHigh As Long = -1
      Dim mLow As Long = Long.MaxValue
      Dim mHigh As Long = -1
      Dim dFind As Date = DateAdd(dGraphInterval, I * lGraphInterval, lStart)
      For J As Integer = 0 To Data.Length - 1
        If Math.Abs(DateDiff(dGraphInterval, Data(J).DATETIME, dFind)) <= lGraphInterval Then
          Dim jVal As Long = -1
          Dim jMax As Long = -1
          If GraphDir = Direction.Up Then
            jVal = Data(J).UPLOAD
            jMax = Data(J).UPLIM
          Else
            jVal = Data(J).DOWNLOAD
            jMax = Data(J).DOWNLIM
          End If
          If lLow > jVal Then lLow = jVal
          If mLow > jMax Then mLow = jMax
          If lHigh < jVal Then lHigh = jVal
          If mHigh < jMax Then mHigh = jMax
        End If
      Next
      If mHigh = -1 Then mHigh = 0
      Dim aMax As Long = Math.Floor((mLow + mHigh) / 2)
      If lHigh > (Math.Floor(aMax / 10) * 9) Then
        If lHigh > -1 Then
          lVal = lHigh
        Else
          If I = lMaxGraphTime Then
            If lastVal > -1 Then
              lVal = lastVal
            Else
              lVal = 0
            End If
          Else
            Dim nextHVal As Long = -1
            Dim K As Long = I
            Do Until nextHVal > -1
              K += 1
              If K > lMaxGraphTime Then Exit Do
              dFind = DateAdd(dGraphInterval, K * lGraphInterval, lStart)
              For J As Integer = 0 To Data.Length - 1
                If Math.Abs(DateDiff(dGraphInterval, Data(J).DATETIME, dFind)) <= lGraphInterval Then
                  Dim jVal As Long = -1
                  If GraphDir = Direction.Up Then
                    jVal = Data(J).UPLOAD
                  Else
                    jVal = Data(J).DOWNLOAD
                  End If
                  If nextHVal < jVal Then nextHVal = jVal
                End If
              Next
            Loop
            If nextHVal > -1 Then
              If lastVal < nextHVal Then
                lVal = nextHVal
              Else
                lVal = lastVal
              End If
            Else
              lVal = lastVal
            End If
          End If
        End If
      ElseIf lLow < Math.Ceiling(aMax / 10) Then
        If lLow < Long.MaxValue Then
          lVal = lLow
        Else
          If I = lMaxGraphTime Then
            If lastVal > -1 Then
              lVal = lastVal
            Else
              lVal = 0
            End If
          Else
            Dim nextLVal As Long = Long.MaxValue
            Dim K As Long = I
            Do Until nextLVal < Long.MaxValue
              K += 1
              If K > lMaxGraphTime Then Exit Do
              dFind = DateAdd(dGraphInterval, K * lGraphInterval, lStart)
              For J As Integer = 0 To Data.Length - 1
                If Math.Abs(DateDiff(dGraphInterval, Data(J).DATETIME, dFind)) <= lGraphInterval Then
                  Dim jVal As Long = -1
                  If GraphDir = Direction.Up Then
                    jVal = Data(J).UPLOAD
                  Else
                    jVal = Data(J).DOWNLOAD
                  End If
                  If nextLVal > jVal Then nextLVal = jVal
                End If
              Next
            Loop
            If nextLVal < Long.MaxValue Then
              If lastVal > nextLVal Then
                lVal = nextLVal
              Else
                lVal = lastVal
              End If
            Else
              lVal = lastVal
            End If
          End If
        End If
      Else
        If lHigh > -1 And lLow < Long.MaxValue Then
          lVal = Math.Round((lLow + lHigh) / 2)
        ElseIf lHigh > -1 Then
          lVal = lHigh
        ElseIf lLow < Long.MaxValue Then
          lVal = lLow
        Else
          If I = lMaxGraphTime Then
            If lastVal > -1 Then
              lVal = lastVal
            Else
              lVal = 0
            End If
          Else
            Dim nextLVal As Long = Long.MaxValue
            Dim K As Long = I
            Do Until nextLVal < Long.MaxValue
              K += 1
              If K > lMaxGraphTime Then Exit Do
              dFind = DateAdd(dGraphInterval, K * lGraphInterval, lStart)
              For J As Integer = 0 To Data.Length - 1
                If Math.Abs(DateDiff(dGraphInterval, Data(J).DATETIME, dFind)) <= lGraphInterval Then
                  Dim jVal As Long = -1
                  If GraphDir = Direction.Up Then
                    jVal = Data(J).UPLOAD
                  Else
                    jVal = Data(J).DOWNLOAD
                  End If
                  If nextLVal > jVal Then nextLVal = jVal
                End If
              Next
            Loop
            Dim nextHVal As Long = -1
            K = I
            Do Until nextHVal > -1
              K += 1
              If K > lMaxGraphTime Then Exit Do
              dFind = DateAdd(dGraphInterval, K * lGraphInterval, lStart)
              For J As Integer = 0 To Data.Length - 1
                If Math.Abs(DateDiff(dGraphInterval, Data(J).DATETIME, dFind)) <= lGraphInterval Then
                  Dim jVal As Long = 0
                  If GraphDir = Direction.Up Then
                    jVal = Data(J).UPLOAD
                  Else
                    jVal = Data(J).DOWNLOAD
                  End If
                  If nextHVal < jVal Then nextHVal = jVal
                End If
              Next
            Loop
            If nextLVal < Long.MaxValue And nextHVal > -1 Then
              lVal = Math.Round((nextLVal + nextHVal) / 2)
            Else
              lVal = lastVal
            End If
          End If
        End If
      End If
      If lVal > -1 Then lastVal = lVal
      lPoints(I).X = lYWidth + (I * dGraphCompInter) + IIf(I > 0, 1, 0)
      lPoints(I).Y = yTop + yHeight - (lVal / lMax * yHeight)
      If I > 0 AndAlso (lPoints(I - 1).X = 0 And lPoints(I - 1).Y = 0) Then
        Dim J As Long = 1
        While lPoints(I - J).Y = 0
          J += 1
        End While
        For K As Long = 1 To J - 1
          lPoints(I - K).X = lYWidth + ((I - K) * dGraphCompInter) + 1
          lPoints(I - K).Y = (lPoints(I - J).Y + lPoints(I).Y) / 2
        Next
      End If
    Next I
    If lPoints(lMaxGraphTime).IsEmpty Then lPoints(lMaxGraphTime) = New Point(ImgSize.Width, yTop + yHeight)
    lPoints(lMaxGraphTime + 1) = New Point(ImgSize.Width, yTop + yHeight)
    lPoints(lMaxGraphTime + 2) = New Point(lYWidth, yTop + yHeight)
    lPoints(lMaxGraphTime + 3) = lPoints(0)
    lTypes(0) = Drawing2D.PathPointType.Start
    For I As Long = 1 To lMaxGraphTime + 2
      lTypes(I) = Drawing2D.PathPointType.Line
    Next
    lTypes(lMaxGraphTime + 3) = Drawing2D.PathPointType.Line Or Drawing2D.PathPointType.CloseSubpath
    g.DrawLines(New Pen(New SolidBrush(ColorMax), 5), lMaxPoints)
    Dim gPath As New Drawing2D.GraphicsPath(lPoints, lTypes)
    Dim fBrush As Drawing2D.LinearGradientBrush = TriGradientBrush(New Point(lYWidth, MaxY), New Point(lYWidth, yTop + yHeight), Color.FromArgb(192, ColorA), Color.FromArgb(192, ColorB), Color.FromArgb(192, ColorC))
    fBrush.WrapMode = Drawing2D.WrapMode.TileFlipX
    g.FillPath(fBrush, gPath)
    g.SetClip(gPath)
    g.FillRectangle(New Drawing2D.HatchBrush(Drawing2D.HatchStyle.LightUpwardDiagonal, Color.FromArgb(192, ColorMax), Color.Transparent), lYWidth + 1, yTop - 1, ImgSize.Width, MaxY - yTop + 1)
    g.ResetClip()
    g.DrawPath(New Pen(ColorLine, 1.5), gPath)
    g.DrawLines(New Pen(New SolidBrush(Color.FromArgb(96, ColorMax)), 5), lMaxPoints)
    g.DrawLine(New Pen(ColorText), lYWidth, yTop, lYWidth, yTop + yHeight)
    g.DrawLine(New Pen(ColorText), lYWidth, yTop + yHeight, ImgSize.Width, yTop + yHeight)
    g.Dispose()
    Return iPic
  End Function
  Public Sub ClearGraphData()
    dData = Nothing
    uData = Nothing
  End Sub
#End Region
#Region "Progress"
  Public Function DisplayProgress(ImgSize As Size, Current As Long, Total As Long, Accuracy As Integer, ColorA As Color, ColorB As Color, ColorC As Color, ColorText As Color, ColorBG As Color) As Image
    If ImgSize.IsEmpty Then Return Nothing
    Dim bmpTmp As Image = New Bitmap(ImgSize.Width, ImgSize.Height)
    Dim fontSize As Single = 8
    If ImgSize.Width < ImgSize.Height Then
      If ImgSize.Width / 12 > 8 Then
        fontSize = ImgSize.Width / 12
      Else
        fontSize = 8
      End If
    Else
      If ImgSize.Height / 12 > 8 Then
        fontSize = ImgSize.Height / 12
      Else
        fontSize = 8
      End If
    End If
    Dim fFont As Font = MonospaceFont(fontSize)
    Dim linGrBrush As Drawing2D.LinearGradientBrush = TriGradientBrush(New Point(0, 0), New Point(0, ImgSize.Height), ColorA, ColorB, ColorC)
    If Total = 0 Then
      Using g As Graphics = Graphics.FromImage(bmpTmp)
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
        g.Clear(ColorBG)
        g.FillRectangle(linGrBrush, 0, 0, ImgSize.Width, ImgSize.Height)
        Dim Msg As String = "Loading..."
        Dim pF As New PointF(ImgSize.Width / 2 - g.MeasureString(Msg, fFont).Width / 2, ImgSize.Height / 2 - g.MeasureString(Msg, fFont).Height / 2)
        g.DrawString(Msg, fFont, New SolidBrush(Color.FromArgb(128, ColorBG)), pF.X + 2, pF.Y + 2)
        g.DrawString(Msg, fFont, New SolidBrush(ColorBG), pF.X + 1, pF.Y + 1)
        g.DrawString(Msg, fFont, New SolidBrush(ColorText), pF)
      End Using
      Return bmpTmp
    Else
      Dim Val As Long
      Dim Msg As String
      If Current < Total Then
        Using g As Graphics = Graphics.FromImage(bmpTmp)
          g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
          g.Clear(ColorBG)
          If Total > 0 And Current > 0 Then
            Val = (Total - Current) / Total * ImgSize.Height
            g.FillRectangle(linGrBrush, 0, Val, ImgSize.Width, ImgSize.Height - Val)
            For I As Double = -1 To ImgSize.Height + 1 Step ((ImgSize.Height + 2.0) / 10.0)
              g.DrawLine(New Pen(ColorBG), 0, CInt(I), ImgSize.Width, CInt(I))
            Next I
            Msg = FormatPercent(Current / Total, Accuracy, TriState.True, TriState.False, TriState.False)
          Else
            Msg = "0%"
          End If
          Dim pF As New PointF(ImgSize.Width / 2 - g.MeasureString(Msg, fFont).Width / 2, ImgSize.Height / 2 - g.MeasureString(Msg, fFont).Height / 2)
          g.DrawString(Msg, fFont, New SolidBrush(Color.FromArgb(128, ColorBG)), pF.X + 2, pF.Y + 2)
          g.DrawString(Msg, fFont, New SolidBrush(ColorBG), pF.X + 1, pF.Y + 1)
          g.DrawString(Msg, fFont, New SolidBrush(ColorText), pF)
        End Using
      Else
        Using g As Graphics = Graphics.FromImage(bmpTmp)
          g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
          g.Clear(ColorBG)
          If Total > 0 And Current > 0 Then
            g.FillRectangle(linGrBrush, 0, 0, ImgSize.Width, ImgSize.Height)
            For I As Double = -1 To ImgSize.Height + 1 Step ((ImgSize.Height + 2.0) / 10.0)
              g.DrawLine(New Pen(ColorBG), 0, CInt(I), ImgSize.Width, CInt(I))
            Next I
            Msg = FormatPercent(Current / Total, Accuracy, TriState.True, TriState.False, TriState.False)
          Else
            Msg = "0%"
          End If
          Dim pF As New PointF(ImgSize.Width / 2 - g.MeasureString(Msg, fFont).Width / 2, ImgSize.Height / 2 - g.MeasureString(Msg, fFont).Height / 2)
          g.DrawString(Msg, fFont, New SolidBrush(Color.FromArgb(128, ColorBG)), pF.X + 2, pF.Y + 2)
          g.DrawString(Msg, fFont, New SolidBrush(ColorBG), pF.X + 1, pF.Y + 1)
          g.DrawString(Msg, fFont, New SolidBrush(ColorText), pF)
        End Using
      End If
      Return bmpTmp
    End If
  End Function
  Public Function DisplayEProgress(ImgSize As Size, Down As Long, Up As Long, Over As Long, Total As Long, Accuracy As Integer, ColorDA As Color, ColorDB As Color, ColorDC As Color, ColorUA As Color, ColorUB As Color, ColorUC As Color, ColorText As Color, ColorBG As Color) As Image
    If ImgSize.IsEmpty Then Return Nothing
    Dim Msg As String
    Dim bmpTmp As Image = New Bitmap(ImgSize.Width, ImgSize.Height)
    Dim fontSize As Single = 8
    If ImgSize.Width < ImgSize.Height Then
      If ImgSize.Width / 12 > 8 Then
        fontSize = ImgSize.Width / 12
      Else
        fontSize = 8
      End If
    Else
      If ImgSize.Height / 12 > 8 Then
        fontSize = ImgSize.Height / 12
      Else
        fontSize = 8
      End If
    End If
    Dim fFont As Font = MonospaceFont(fontSize)
    Dim downBrush As Drawing2D.LinearGradientBrush = TriGradientBrush(New Point(0, 0), New Point(ImgSize.Width, 0), ColorDC, ColorDB, ColorDA)
    Dim upBrush As Drawing2D.LinearGradientBrush = TriGradientBrush(New Point(0, 0), New Point(ImgSize.Width, 0), ColorUC, ColorUB, ColorUA)
    If Down + Up + Over < Total Then
      Using g As Graphics = Graphics.FromImage(bmpTmp)
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
        g.Clear(ColorBG)
        If Total > 0 And (Down > 0 Or Up > 0) Then
          Dim upWidth As Long = ImgSize.Width - ((Total - Up) / Total * ImgSize.Width)
          g.FillRectangle(upBrush, 0, 0, upWidth, ImgSize.Height)
          Dim downWidth As Long = ImgSize.Width - ((Total - Down) / Total * ImgSize.Width)
          g.FillRectangle(downBrush, upWidth, 0, downWidth, ImgSize.Height)
          For I As Double = -1 To ImgSize.Width + 1 Step ((ImgSize.Width + 2.0) / 10.0)
            g.DrawLine(New Pen(ColorBG), CInt(I), 0, CInt(I), ImgSize.Height)
          Next I
          If Over > 0 Then
            Msg = "Down:  " & FormatPercent(Down / Total, Accuracy, TriState.True, TriState.False, TriState.False) & vbLf &
                  "Up:    " & FormatPercent(Up / Total, Accuracy, TriState.True, TriState.False, TriState.False) & vbLf &
                  "Over:  " & FormatPercent(Over / Total, Accuracy, TriState.True, TriState.False, TriState.False) & vbLf &
                  "Total: " & FormatPercent((Down + Up + Over) / Total, Accuracy, TriState.True, TriState.False, TriState.False)
          Else
            Msg = "Down:  " & FormatPercent(Down / Total, Accuracy, TriState.True, TriState.False, TriState.False) & vbLf &
                  "Up:    " & FormatPercent(Up / Total, Accuracy, TriState.True, TriState.False, TriState.False) & vbLf &
                  "Total: " & FormatPercent((Down + Up) / Total, Accuracy, TriState.True, TriState.False, TriState.False)
          End If
        Else
          Msg = "0%"
        End If
        Dim pF As New PointF(ImgSize.Width / 2 - g.MeasureString(Msg, fFont).Width / 2, ImgSize.Height / 2 - g.MeasureString(Msg, fFont).Height / 2)
        g.DrawString(Msg, fFont, New SolidBrush(Color.FromArgb(128, ColorBG)), pF.X + 2, pF.Y + 2)
        g.DrawString(Msg, fFont, New SolidBrush(ColorBG), pF.X + 1, pF.Y + 1)
        g.DrawString(Msg, fFont, New SolidBrush(ColorText), pF)
      End Using
    Else
      Using g As Graphics = Graphics.FromImage(bmpTmp)
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
        g.Clear(ColorBG)
        If Total > 0 And (Down > 0 Or Up > 0) Then
          Dim fillT As Long = Down + Up
          Dim upWidth As Long = ImgSize.Width - ((fillT - Up) / fillT * ImgSize.Width)
          g.FillRectangle(upBrush, 0, 0, upWidth, ImgSize.Height)
          Dim downWidth As Long = ImgSize.Width - ((fillT - Down) / fillT * ImgSize.Width)
          g.FillRectangle(downBrush, upWidth, 0, downWidth, ImgSize.Height)
          For I As Double = -1 To ImgSize.Width + 1 Step ((ImgSize.Width + 2.0) / 10.0)
            g.DrawLine(New Pen(ColorBG), CInt(I), 0, CInt(I), ImgSize.Height)
          Next I
          If Over > 0 Then
            Msg = "Down:  " & FormatPercent(Down / Total, Accuracy, TriState.True, TriState.False, TriState.False) & vbLf &
                  "Up:    " & FormatPercent(Up / Total, Accuracy, TriState.True, TriState.False, TriState.False) & vbLf &
                  "Over:  " & FormatPercent(Over / Total, Accuracy, TriState.True, TriState.False, TriState.False) & vbLf &
                  "Total: " & FormatPercent((Down + Up + Over) / Total, Accuracy, TriState.True, TriState.False, TriState.False)
          Else
            Msg = "Down:  " & FormatPercent(Down / Total, Accuracy, TriState.True, TriState.False, TriState.False) & vbLf &
                  "Up:    " & FormatPercent(Up / Total, Accuracy, TriState.True, TriState.False, TriState.False) & vbLf &
                  "Total: " & FormatPercent((Down + Up) / Total, Accuracy, TriState.True, TriState.False, TriState.False)
          End If

        Else
          Msg = "0%"
        End If
        Dim pF As New PointF(ImgSize.Width / 2 - g.MeasureString(Msg, fFont).Width / 2, ImgSize.Height / 2 - g.MeasureString(Msg, fFont).Height / 2)
        g.DrawString(Msg, fFont, New SolidBrush(Color.FromArgb(128, ColorBG)), pF.X + 2, pF.Y + 2)
        g.DrawString(Msg, fFont, New SolidBrush(ColorBG), pF.X + 1, pF.Y + 1)
        g.DrawString(Msg, fFont, New SolidBrush(ColorText), pF)
      End Using
    End If
    Return bmpTmp
  End Function
  Public Function DisplayRProgress(ImgSize As Size, Down As Long, Total As Long, Accuracy As Integer, ColorA As Color, ColorB As Color, ColorC As Color, ColorText As Color, ColorBG As Color) As Image
    If ImgSize.IsEmpty Then Return Nothing
    Dim Msg As String
    Dim bmpTmp As Image = New Bitmap(ImgSize.Width, ImgSize.Height)
    Dim fontSize As Single = 8
    If ImgSize.Width < ImgSize.Height Then
      If ImgSize.Width / 12 > 8 Then
        fontSize = ImgSize.Width / 12
      Else
        fontSize = 8
      End If
    Else
      If ImgSize.Height / 12 > 8 Then
        fontSize = ImgSize.Height / 12
      Else
        fontSize = 8
      End If
    End If
    Dim fFont As Font = MonospaceFont(fontSize)
    Dim downBrush As Drawing2D.LinearGradientBrush = TriGradientBrush(New Point(0, 0), New Point(ImgSize.Width, 0), ColorC, ColorB, ColorA)
    If Down < Total Then
      Using g As Graphics = Graphics.FromImage(bmpTmp)
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
        g.Clear(ColorBG)
        If Total > 0 And Down > 0 Then
          Dim downWidth As Long = ImgSize.Width - (((Total - Down) / Total) * ImgSize.Width)
          g.FillRectangle(downBrush, 0, 0, downWidth, ImgSize.Height)
          For I As Double = -1 To ImgSize.Width + 1 Step ((ImgSize.Width + 2.0) / 10.0)
            g.DrawLine(New Pen(ColorBG), CInt(I), 0, CInt(I), ImgSize.Height)
          Next I
          Msg = FormatPercent(Down / Total, Accuracy, TriState.True, TriState.False, TriState.False)
        Else
          Msg = "0%"
        End If
        Dim pF As New PointF(ImgSize.Width / 2 - g.MeasureString(Msg, fFont).Width / 2, ImgSize.Height / 2 - g.MeasureString(Msg, fFont).Height / 2)
        g.DrawString(Msg, fFont, New SolidBrush(Color.FromArgb(128, ColorBG)), pF.X + 2, pF.Y + 2)
        g.DrawString(Msg, fFont, New SolidBrush(ColorBG), pF.X + 1, pF.Y + 1)
        g.DrawString(Msg, fFont, New SolidBrush(ColorText), pF)
      End Using
    Else
      Using g As Graphics = Graphics.FromImage(bmpTmp)
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
        g.Clear(ColorBG)
        If Total > 0 And Down > 0 Then
          g.FillRectangle(downBrush, 0, 0, ImgSize.Width, ImgSize.Height)
          For I As Double = -1 To ImgSize.Width + 1 Step ((ImgSize.Width + 2.0) / 10.0)
            g.DrawLine(New Pen(ColorBG), CInt(I), 0, CInt(I), ImgSize.Height)
          Next I
          Msg = FormatPercent(Down / Total, Accuracy, TriState.True, TriState.False, TriState.False)
        Else
          Msg = "0%"
        End If
        Dim pF As New PointF(ImgSize.Width / 2 - g.MeasureString(Msg, fFont).Width / 2, ImgSize.Height / 2 - g.MeasureString(Msg, fFont).Height / 2)
        g.DrawString(Msg, fFont, New SolidBrush(Color.FromArgb(128, ColorBG)), pF.X + 2, pF.Y + 2)
        g.DrawString(Msg, fFont, New SolidBrush(ColorBG), pF.X + 1, pF.Y + 1)
        g.DrawString(Msg, fFont, New SolidBrush(ColorText), pF)
      End Using
    End If
    Return bmpTmp
  End Function
  Private Function TriGradientBrush(point1 As Point, point2 As Point, ColorA As Color, ColorB As Color, ColorC As Color) As Drawing2D.LinearGradientBrush
    If point1.Equals(point2) Then
      Return New Drawing2D.LinearGradientBrush(point1, New Point(point2.X, point2.Y + 1), ColorC, ColorA)
    End If
    Dim tBrush As Drawing2D.LinearGradientBrush
    If ColorB = Color.Transparent Then
      tBrush = New Drawing2D.LinearGradientBrush(point1, point2, ColorC, ColorA)
    Else
      tBrush = New Drawing2D.LinearGradientBrush(point1, point2, Color.Black, Color.Black)
      Dim cb As New Drawing2D.ColorBlend
      cb.Positions = {0, 0.5, 1}
      cb.Colors = {ColorC, ColorB, ColorA}
      tBrush.InterpolationColors = cb
    End If
    Return tBrush
  End Function
#End Region
#Region "Tray"
  Private Const Alpha As Integer = 192
  Public Sub CreateTrayIcon_Left(ByRef g As Graphics, lUsed As Long, lLim As Long, cA As Color, cB As Color, cC As Color, icoX As Integer, icoY As Integer)
    If lLim = 0 Then Return
    Dim fillBrush As Drawing2D.LinearGradientBrush
    If cB = Color.Transparent Then
      fillBrush = New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(0, icoY), Color.FromArgb(Alpha, cC), Color.FromArgb(Alpha, cA))
    Else
      fillBrush = New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(0, icoY), Color.Black, Color.Black)
      Dim cBlend As New Drawing2D.ColorBlend
      cBlend.Positions = {0, 0.5, 1}
      cBlend.Colors = {Color.FromArgb(Alpha, cC), Color.FromArgb(Alpha, cB), Color.FromArgb(Alpha, cA)}
      fillBrush.InterpolationColors = cBlend
    End If
    Dim yUsed As Integer = icoY - (lUsed / lLim * icoY)
    If yUsed < 0 Then yUsed = 0
    If yUsed > icoY Then yUsed = icoY
    g.FillRectangle(fillBrush, 0, yUsed, CInt(Math.Floor(icoX / 2)), icoY - yUsed)
  End Sub
  Public Sub CreateTrayIcon_Right(ByRef g As Graphics, lUsed As Long, lLim As Long, cA As Color, cB As Color, cC As Color, icoX As Integer, icoY As Integer)
    If lLim = 0 Then Return
    Dim fillBrush As Drawing2D.LinearGradientBrush
    If cB = Color.Transparent Then
      fillBrush = New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(0, icoY), Color.FromArgb(Alpha, cC), Color.FromArgb(Alpha, cA))
    Else
      fillBrush = New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(0, icoY), Color.Black, Color.Black)
      Dim cBlend As New Drawing2D.ColorBlend
      cBlend.Positions = {0, 0.5, 1}
      cBlend.Colors = {Color.FromArgb(Alpha, cC), Color.FromArgb(Alpha, cB), Color.FromArgb(Alpha, cA)}
      fillBrush.InterpolationColors = cBlend
    End If
    Dim yUsed As Integer = icoY - (lUsed / lLim * icoY)
    If yUsed < 0 Then yUsed = 0
    If yUsed > icoY Then yUsed = icoY
    g.FillRectangle(fillBrush, CInt(Math.Floor(icoX / 2)), yUsed, CInt(Math.Ceiling(icoX / 2)), icoY - yUsed)
  End Sub
#End Region
  Public Sub ScreenDefaultColors(ByRef Colors As AppSettings.AppColors, useStyle As localRestrictionTracker.SatHostTypes)
    Dim defaultColors As AppSettings.AppColors = GetDefaultColors(useStyle)
    If Colors.MainDownA = Color.Transparent Or Colors.MainDownA.A < 255 Then Colors.MainDownA = defaultColors.MainDownA
    If Colors.MainDownB = Color.Transparent Or Colors.MainDownB.A < 255 Then Colors.MainDownB = defaultColors.MainDownB
    If Colors.MainDownC = Color.Transparent Or Colors.MainDownC.A < 255 Then Colors.MainDownC = defaultColors.MainDownC
    If Colors.MainUpA = Color.Transparent Or Colors.MainUpA.A < 255 Then Colors.MainUpA = defaultColors.MainUpA
    If Colors.MainUpB = Color.Transparent Or Colors.MainUpB.A < 255 Then Colors.MainUpB = defaultColors.MainUpB
    If Colors.MainUpC = Color.Transparent Or Colors.MainUpC.A < 255 Then Colors.MainUpC = defaultColors.MainUpC
    If Colors.MainText = Color.Transparent Or Colors.MainText.A < 255 Then Colors.MainText = defaultColors.MainText
    If Colors.MainBackground = Color.Transparent Or Colors.MainBackground.A < 255 Then Colors.MainBackground = defaultColors.MainBackground

    If Colors.TrayDownA = Color.Transparent Or Colors.TrayDownA.A < 255 Then Colors.TrayDownA = defaultColors.TrayDownA
    If Colors.TrayDownB = Color.Transparent Or Colors.TrayDownB.A < 255 Then Colors.TrayDownB = defaultColors.TrayDownB
    If Colors.TrayDownC = Color.Transparent Or Colors.TrayDownC.A < 255 Then Colors.TrayDownC = defaultColors.TrayDownC
    If Colors.TrayUpA = Color.Transparent Or Colors.TrayUpA.A < 255 Then Colors.TrayUpA = defaultColors.TrayUpA
    If Colors.TrayUpB = Color.Transparent Or Colors.TrayUpB.A < 255 Then Colors.TrayUpB = defaultColors.TrayUpB
    If Colors.TrayUpC = Color.Transparent Or Colors.TrayUpC.A < 255 Then Colors.TrayUpC = defaultColors.TrayUpC

    If Colors.HistoryDownLine = Color.Transparent Or Colors.HistoryDownLine.A < 255 Then Colors.HistoryDownLine = defaultColors.HistoryDownLine
    If Colors.HistoryDownA = Color.Transparent Or Colors.HistoryDownA.A < 255 Then Colors.HistoryDownA = defaultColors.HistoryDownA
    If Colors.HistoryDownB = Color.Transparent Or Colors.HistoryDownB.A < 255 Then Colors.HistoryDownB = defaultColors.HistoryDownB
    If Colors.HistoryDownC = Color.Transparent Or Colors.HistoryDownC.A < 255 Then Colors.HistoryDownC = defaultColors.HistoryDownC
    If Colors.HistoryDownMax = Color.Transparent Or Colors.HistoryDownMax.A < 255 Then Colors.HistoryDownMax = defaultColors.HistoryDownMax
    If Colors.HistoryUpLine = Color.Transparent Or Colors.HistoryUpLine.A < 255 Then Colors.HistoryUpLine = defaultColors.HistoryUpLine
    If Colors.HistoryUpA = Color.Transparent Or Colors.HistoryUpA.A < 255 Then Colors.HistoryUpA = defaultColors.HistoryUpA
    If Colors.HistoryUpB = Color.Transparent Or Colors.HistoryUpB.A < 255 Then Colors.HistoryUpB = defaultColors.HistoryUpB
    If Colors.HistoryUpC = Color.Transparent Or Colors.HistoryUpC.A < 255 Then Colors.HistoryUpC = defaultColors.HistoryUpC
    If Colors.HistoryUpMax = Color.Transparent Or Colors.HistoryUpMax.A < 255 Then Colors.HistoryUpMax = defaultColors.HistoryUpMax
    If Colors.HistoryText = Color.Transparent Or Colors.HistoryText.A < 255 Then Colors.HistoryText = defaultColors.HistoryText
    If Colors.HistoryBackground = Color.Transparent Or Colors.HistoryBackground.A < 255 Then Colors.HistoryBackground = defaultColors.HistoryBackground
    If Colors.HistoryLightGrid = Color.Transparent Or Colors.HistoryLightGrid.A < 255 Then Colors.HistoryLightGrid = defaultColors.HistoryLightGrid
    If Colors.HistoryDarkGrid = Color.Transparent Or Colors.HistoryDarkGrid.A < 255 Then Colors.HistoryDarkGrid = defaultColors.HistoryDarkGrid
  End Sub
  Public Function GetDefaultColors(useStyle As localRestrictionTracker.SatHostTypes) As AppSettings.AppColors
    Dim outColors As New AppSettings.AppColors
    Select Case useStyle
      Case localRestrictionTracker.SatHostTypes.WildBlue_LEGACY, localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY
        outColors.MainDownA = Color.DarkBlue
        outColors.MainDownB = Color.Blue
        outColors.MainDownC = Color.Aqua
        outColors.MainUpA = Color.DarkBlue
        outColors.MainUpB = Color.Blue
        outColors.MainUpC = Color.Aqua
        outColors.MainText = Color.White
        outColors.MainBackground = Color.Black

        outColors.TrayDownA = Color.DarkBlue
        outColors.TrayDownB = Color.Blue
        outColors.TrayDownC = Color.Aqua
        outColors.TrayUpA = Color.DarkBlue
        outColors.TrayUpB = Color.Blue
        outColors.TrayUpC = Color.Aqua

        outColors.HistoryDownLine = Color.DarkBlue
        outColors.HistoryDownA = Color.DarkBlue
        outColors.HistoryDownB = Color.Blue
        outColors.HistoryDownC = Color.Aqua
        outColors.HistoryDownMax = Color.Yellow
        outColors.HistoryUpLine = Color.DarkBlue
        outColors.HistoryUpA = Color.DarkBlue
        outColors.HistoryUpB = Color.Blue
        outColors.HistoryUpC = Color.Aqua
        outColors.HistoryUpMax = Color.Yellow
        outColors.HistoryText = Color.Black
        outColors.HistoryBackground = Color.White
        outColors.HistoryLightGrid = Color.LightGray
        outColors.HistoryDarkGrid = Color.DarkGray

      Case localRestrictionTracker.SatHostTypes.RuralPortal_EXEDE, localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
        outColors.MainDownA = Color.DarkBlue
        outColors.MainDownB = Color.Blue
        outColors.MainDownC = Color.Aqua
        outColors.MainUpA = Color.Transparent
        outColors.MainUpB = Color.Transparent
        outColors.MainUpC = Color.Transparent
        outColors.MainText = Color.White
        outColors.MainBackground = Color.Black

        outColors.TrayDownA = Color.DarkBlue
        outColors.TrayDownB = Color.Blue
        outColors.TrayDownC = Color.Aqua
        outColors.TrayUpA = Color.Transparent
        outColors.TrayUpB = Color.Transparent
        outColors.TrayUpC = Color.Transparent

        outColors.HistoryDownLine = Color.DarkBlue
        outColors.HistoryDownA = Color.DarkBlue
        outColors.HistoryDownB = Color.Blue
        outColors.HistoryDownC = Color.Aqua
        outColors.HistoryDownMax = Color.Yellow
        outColors.HistoryUpLine = Color.DarkBlue
        outColors.HistoryUpA = Color.Transparent
        outColors.HistoryUpB = Color.Transparent
        outColors.HistoryUpC = Color.Transparent
        outColors.HistoryUpMax = Color.Transparent
        outColors.HistoryText = Color.Black
        outColors.HistoryBackground = Color.White
        outColors.HistoryLightGrid = Color.LightGray
        outColors.HistoryDarkGrid = Color.DarkGray

      Case localRestrictionTracker.SatHostTypes.Dish_EXEDE
        outColors.MainDownA = Color.DarkBlue
        outColors.MainDownB = Color.Blue
        outColors.MainDownC = Color.Aqua
        outColors.MainUpA = Color.DarkBlue
        outColors.MainUpB = Color.Blue
        outColors.MainUpC = Color.Aqua
        outColors.MainText = Color.White
        outColors.MainBackground = Color.Black

        outColors.TrayDownA = Color.DarkBlue
        outColors.TrayDownB = Color.Blue
        outColors.TrayDownC = Color.Aqua
        outColors.TrayUpA = Color.DarkBlue
        outColors.TrayUpB = Color.Blue
        outColors.TrayUpC = Color.Aqua

        outColors.HistoryDownLine = Color.DarkBlue
        outColors.HistoryDownA = Color.DarkBlue
        outColors.HistoryDownB = Color.Blue
        outColors.HistoryDownC = Color.Aqua
        outColors.HistoryDownMax = Color.Yellow
        outColors.HistoryUpLine = Color.DarkBlue
        outColors.HistoryUpA = Color.DarkBlue
        outColors.HistoryUpB = Color.Blue
        outColors.HistoryUpC = Color.Aqua
        outColors.HistoryUpMax = Color.Yellow
        outColors.HistoryText = Color.Black
        outColors.HistoryBackground = Color.White
        outColors.HistoryLightGrid = Color.LightGray
        outColors.HistoryDarkGrid = Color.DarkGray

      Case Else
        outColors.MainDownA = Color.DarkBlue
        outColors.MainDownB = Color.Blue
        outColors.MainDownC = Color.Aqua
        outColors.MainUpA = Color.DarkBlue
        outColors.MainUpB = Color.Blue
        outColors.MainUpC = Color.Aqua
        outColors.MainText = Color.White
        outColors.MainBackground = Color.Black

        outColors.TrayDownA = Color.DarkBlue
        outColors.TrayDownB = Color.Blue
        outColors.TrayDownC = Color.Aqua
        outColors.TrayUpA = Color.DarkBlue
        outColors.TrayUpB = Color.Blue
        outColors.TrayUpC = Color.Aqua

        outColors.HistoryDownLine = Color.DarkBlue
        outColors.HistoryDownA = Color.DarkBlue
        outColors.HistoryDownB = Color.Blue
        outColors.HistoryDownC = Color.Aqua
        outColors.HistoryDownMax = Color.Yellow
        outColors.HistoryUpLine = Color.DarkBlue
        outColors.HistoryUpA = Color.DarkBlue
        outColors.HistoryUpB = Color.Blue
        outColors.HistoryUpC = Color.Aqua
        outColors.HistoryUpMax = Color.Yellow
        outColors.HistoryText = Color.Black
        outColors.HistoryBackground = Color.White
        outColors.HistoryLightGrid = Color.LightGray
        outColors.HistoryDarkGrid = Color.DarkGray

    End Select
    Return outColors
  End Function
#End Region
#Region "Task Dialogs"
  Public Enum _TaskDialogIcon
    None = 0
    Space = &H1
    File = &H2
    OpenFolder = &H3
    OpenFolder2 = &H4
    ClosedFolder = &H5
    FolderOpening = &H6
    SearchCatalog = &H8
    CatalogFolder = &H9
    CatalogFolderOpening = &HA
    Games = &HE
    EXE = &HF
    SearchFolder = &H12
    AlignedTextFile = &H13
    Envelope = &H14
    PictureViewer = &H15
    MusicSheet = &H16
    VideoClip = &H17
    DefaultPrograms = &H18
    Internet = &H19
    PrinterFolder = &H1A
    ControlPanel = &H1B
    FloppyDrive35 = &H1C
    FloppyDrive525 = &H1D
    CDDrive = &H1E
    NetworkDriveDisconnected = &H1F
    HardDrive = &H20
    NetworkDrive = &H21
    MemoryChip = &H22
    RemovableDrive = &H23
    WindowsDrive = &H24
    DVDDrive = &H25
    DVDR = &H26
    DVDRAM = &H27
    DVDROM = &H28
    DVDRW = &H29
    ZipDrive = &H2A
    TapeDrive = &H2B
    BluDrive = &H2C
    PrinterInternet = &H2D
    Camcorder = &H2E
    Phone = &H2F
    PrinterDefaultFloppy = &H30
    PrinterDefault = &H31
    PrinterDefaultNetwork = &H32
    Printer = &H33
    PrinterFloppy = &H34
    PrinterNetwork = &H35
    TrashFull = &H36
    TrashEmpty = &H37
    DVD = &H38
    Camera = &H39
    CardReader = &H3A
    Padlock = &H3B
    SDCard = &H3C
    CD = &H3D
    CDR = &H3E
    CDROM = &H3F
    CDRW = &H40
    JewelCase = &H41
    MP3Player = &H42
    DLL = &H43
    Batch = &H44
    INI = &H45
    GIF = &H46
    BMP = &H47
    JPEG = &H48
    InternetFolder = &H49
    InternetFolderOpen = &H4A
    UnknownDrive = &H4B
    Fax = &H4C
    Fonts = &H4D
    ShieldUAC = &H4E
    UserAccount = &H4F
    StartMenu = &H50
    Information = &H51
    Key = &H52
    PNG = &H53
    Warning = &H54
    CDMusic = &H55
    Accessibility = &H56
    WindowsUpdate = &H57
    UserAccount2 = &H58
    Delete = &H59
    RichText = &H5A
    WhiteFloppyDrive = &H5B
    SilverScreen = &H5C
    PDA = &H5D
    TextSelection = &H5E
    Scanner = &H5F
    ExternalChip = &H60
    Disabled = &H61
    [Error] = &H62
    Question = &H63
    Run = &H64
    Sleep = &H65
    BlankTextFile = &H66
    Projector = &H67
    ShieldQuestion = &H68
    ShieldError = &H69
    ShieldOK = &H6A
    ShieldWarning = &H6B
    MusicFolder = &H6C
    Computer = &H6D
    Desktop = &H6E
    Defrag = &H6F
    DocumentsFolder = &H70
    PicturesFolder = &H71
    Options = &H72
    FavoritesFolder = &H73
    TaskDialog = &H74
    Recent = &H75
    InternetNetwork = &H78
    UserFolder = &H7B
    FontBigA = &H7C
    FontTTCert = &H7D
    FontTCCert = &H7E
    FontOCert = &H7F
    FontLittleA = &H80
    Fonts2 = &H81
    GuestUser = &H82
    MusicFile = &H83
    PictureFile = &H84
    VideoFile = &H85
    MediaFile = &H86
    DVDMusic = &H87
    DVDVideo = &H88
    CDMusic2 = &H89
    DVDVideoClip = &H8A
    HDDVDVideoClip = &H8B
    BluRayVideoClip = &H8C
    VCDVideoClip = &H8D
    SVCDVideoClip = &H8E
    NetworkFolder = &H8F
    InternetTime = &H90
    SearchComputer = &H91
    CDUnknown = &H92
    ComputerTransfer = &H93
    LibraryStack = &H94
    SystemProperties = &H95
    ResourceMonitor = &H96
    Personalize = &H97
    Network = &H98
    CDBurn = &H9B
    [Default] = &H9D
    FontTT = &H9E
    FontTC = &H9F
    FontO = &HA0
    WindowsUpdate2 = &HA1
    FolderFull = &HA2
    Shortcut = &HA3
    [Shared] = &HA4
    Preferences = &HA5
    FolderPreferences = &HA6
    ZipDriveDisconnected = &HA7
    WindowsPhotoFile = &HA8
    Download = &HA9
    Bad = &HAA
    MoveToNetwork = &HAB
    DVDPlusR = &HAC
    DVDPlusRW = &HAD
    ZipFile = &HAE
    CompressedFile = &HAF
    Sound = &HB0
    Search = &HB1
    UserAccountsFolder = &HB2
    InternetRJ45 = &HB3
    CDMusicPlus = &HB4
    ContactsFolder = &HB5
    Keyboard = &HB6
    DesktopFolder = &HB7
    DownloadsFolder = &HB8
    LinksFolder = &HB9
    GamesFolder = &HBA
    VideoFolder = &HBD
    GreenFolder = &HBE
    EmptyBox = &HBF
    BorderedBox = &HC0
    VideoStandard = &HC1
    VideoWide = &HC2
    ShieldUpdateTime = &HC3
    PrinterSound = &HC4
    PersonalizeComputer = &HC5
    Library = &H3E9
    LibraryDocuments = &H3EA
    LibraryPictures = &H3EB
    LibraryMusic = &H3EC
    LibraryVideos = &H3ED
    TV = &H3F0
    Users = &H3F2
    Homegroup = &H3F5
    Library2 = &H3F6
    FavoritesFolder2 = &H3FC
    ComputerDialog = &H3FD
    ComputerTasks = &H3FE
    Libraries = &H3FF
    Favorites = &H400
    SearchesFolder = &H401
    Album = &H402
    No = &H403
    User = &H405
    DriveUnlocked = &H406
    DriveLocked = &H407
    DriveUnlockedError = &H408
    DriveUnlockedWindows = &H409
    DriveUnlcokedErrorWindows = &H40A
    DriveUnlockedRemovable = &H40B
    DriveLockedRemovable = &H40C
    DriveUnlockedErrorRemovable = &H40D
    TaskDialog2 = &HFF9C
    ShieldUAC2 = &HFFF7
    ShieldOK2 = &HFFF8
    ShieldError2 = &HFFF9
    ShieldWarning2 = &HFFFA
    ShieldUAC3 = &HFFFB
    ShieldUAC4 = &HFFFC
    Information2 = &HFFFD
    Error2 = &HFFFE
    Warning2 = &HFFFF
  End Enum
  Public Enum _TaskDialogExpandedDetailsLocation
    Hide
    ExpandContent
    ExpandFooter
  End Enum
  Public Function MsgDlg(owner As Form, Text As String, Optional Header As String = Nothing, Optional Caption As String = Nothing, Optional Buttons As MessageBoxButtons = MessageBoxButtons.OK, Optional Icon As _TaskDialogIcon = _TaskDialogIcon.None, Optional OldIcon As MessageBoxIcon = MessageBoxIcon.None, Optional DefaultButton As MessageBoxDefaultButton = MessageBoxDefaultButton.Button1, Optional Details As String = Nothing, Optional DetailsMode As _TaskDialogExpandedDetailsLocation = _TaskDialogExpandedDetailsLocation.Hide, Optional ShowDetails As String = "View Details", Optional HideDetails As String = "Hide Details", Optional OldHelpLink As Boolean = False) As DialogResult
    Try
      Return MsgDlgInternal(owner, Text, Header, Caption, Buttons, Icon, OldIcon, DefaultButton, Details, DetailsMode, ShowDetails, HideDetails, OldHelpLink)
    Catch ex As Exception
      Return MsgDlgLegacy(owner, Text, Header, My.Application.Info.ProductName & " - " & Caption, Buttons, OldIcon, DefaultButton, Details, OldHelpLink)
    End Try
  End Function
  Private Function MsgDlgInternal(owner As Form, Text As String, Optional Header As String = Nothing, Optional Caption As String = Nothing, Optional Buttons As MessageBoxButtons = MessageBoxButtons.OK, Optional Icon As _TaskDialogIcon = _TaskDialogIcon.None, Optional OldIcon As MessageBoxIcon = MessageBoxIcon.None, Optional DefaultButton As MessageBoxDefaultButton = MessageBoxDefaultButton.Button1, Optional Details As String = Nothing, Optional DetailsMode As _TaskDialogExpandedDetailsLocation = _TaskDialogExpandedDetailsLocation.Hide, Optional ShowDetails As String = "View Details", Optional HideDetails As String = "Hide Details", Optional OldHelpLink As Boolean = False) As DialogResult
    If TaskDialog.IsPlatformSupported Then
      Using dlgMessage As New TaskDialog
        dlgMessage.Cancelable = True
        dlgMessage.Caption = My.Application.Info.ProductName & " - " & Caption
        dlgMessage.InstructionText = Header
        dlgMessage.Text = Text
        dlgMessage.Icon = Icon
        dlgMessage.HyperlinksEnabled = True
        AddHandler dlgMessage.HyperlinkClick, AddressOf SelectionDialogHyperlink_Click
        Select Case Buttons
          Case MessageBoxButtons.OK
            Dim cmdOK As New TaskDialogButton("cmdOK", "&OK")
            If DefaultButton = MessageBoxDefaultButton.Button1 Then
              cmdOK.Default = True
            Else
              cmdOK.Default = False
            End If
            AddHandler cmdOK.Click, AddressOf SelectionDialogButton_Click
            dlgMessage.Controls.Add(cmdOK)
          Case MessageBoxButtons.YesNo
            Dim cmdYes As New TaskDialogButton("cmdYes", "&Yes")
            Dim cmdNo As New TaskDialogButton("cmdNo", "&No")
            If DefaultButton = MessageBoxDefaultButton.Button1 Then
              cmdYes.Default = True
              cmdNo.Default = False
            ElseIf DefaultButton = MessageBoxDefaultButton.Button2 Then
              cmdYes.Default = False
              cmdNo.Default = True
            Else
              cmdYes.Default = False
              cmdNo.Default = False
            End If
            AddHandler cmdYes.Click, AddressOf SelectionDialogButton_Click
            AddHandler cmdNo.Click, AddressOf SelectionDialogButton_Click
            dlgMessage.Controls.Add(cmdYes)
            dlgMessage.Controls.Add(cmdNo)
          Case MessageBoxButtons.YesNoCancel
            Dim cmdYes As New TaskDialogButton("cmdYes", "&Yes")
            Dim cmdNo As New TaskDialogButton("cmdNo", "&No")
            Dim cmdCancel As New TaskDialogButton("cmdCancel", "&Cancel")
            If DefaultButton = MessageBoxDefaultButton.Button1 Then
              cmdYes.Default = True
              cmdNo.Default = False
              cmdCancel.Default = False
            ElseIf DefaultButton = MessageBoxDefaultButton.Button2 Then
              cmdYes.Default = False
              cmdNo.Default = True
              cmdCancel.Default = False
            Else
              cmdYes.Default = False
              cmdNo.Default = False
              cmdCancel.Default = True
            End If
            AddHandler cmdYes.Click, AddressOf SelectionDialogButton_Click
            AddHandler cmdNo.Click, AddressOf SelectionDialogButton_Click
            AddHandler cmdCancel.Click, AddressOf SelectionDialogButton_Click
            dlgMessage.Controls.Add(cmdYes)
            dlgMessage.Controls.Add(cmdNo)
            dlgMessage.Controls.Add(cmdCancel)
          Case MessageBoxButtons.OKCancel
            Dim cmdOK As New TaskDialogButton("cmdOK", "&OK")
            Dim cmdCancel As New TaskDialogButton("cmdCancel", "&Cancel")
            If DefaultButton = MessageBoxDefaultButton.Button1 Then
              cmdOK.Default = True
              cmdCancel.Default = False
            ElseIf DefaultButton = MessageBoxDefaultButton.Button2 Then
              cmdOK.Default = False
              cmdCancel.Default = True
            Else
              cmdOK.Default = False
              cmdCancel.Default = False
            End If
            AddHandler cmdOK.Click, AddressOf SelectionDialogButton_Click
            AddHandler cmdCancel.Click, AddressOf SelectionDialogButton_Click
            dlgMessage.Controls.Add(cmdOK)
            dlgMessage.Controls.Add(cmdCancel)
          Case MessageBoxButtons.RetryCancel
            Dim cmdRetry As New TaskDialogButton("cmdRetry", "&Retry")
            Dim cmdCancel As New TaskDialogButton("cmdCancel", "&Cancel")
            If DefaultButton = MessageBoxDefaultButton.Button1 Then
              cmdRetry.Default = True
              cmdCancel.Default = False
            ElseIf DefaultButton = MessageBoxDefaultButton.Button2 Then
              cmdRetry.Default = False
              cmdCancel.Default = True
            Else
              cmdRetry.Default = False
              cmdCancel.Default = False
            End If
            AddHandler cmdRetry.Click, AddressOf SelectionDialogButton_Click
            AddHandler cmdCancel.Click, AddressOf SelectionDialogButton_Click
            dlgMessage.Controls.Add(cmdRetry)
            dlgMessage.Controls.Add(cmdCancel)
          Case MessageBoxButtons.AbortRetryIgnore
            Dim cmdAbort As New TaskDialogButton("cmdNo", "&Abort")
            Dim cmdRetry As New TaskDialogButton("cmdRetry", "&Retry")
            Dim cmdIgnore As New TaskDialogButton("cmdClose", "&Ignore")
            If DefaultButton = MessageBoxDefaultButton.Button1 Then
              cmdAbort.Default = True
              cmdRetry.Default = False
              cmdIgnore.Default = False
            ElseIf DefaultButton = MessageBoxDefaultButton.Button2 Then
              cmdAbort.Default = False
              cmdRetry.Default = True
              cmdIgnore.Default = False
            Else
              cmdAbort.Default = False
              cmdRetry.Default = False
              cmdIgnore.Default = True
            End If
            AddHandler cmdAbort.Click, AddressOf SelectionDialogButton_Click
            AddHandler cmdRetry.Click, AddressOf SelectionDialogButton_Click
            AddHandler cmdIgnore.Click, AddressOf SelectionDialogButton_Click
            dlgMessage.Controls.Add(cmdAbort)
            dlgMessage.Controls.Add(cmdRetry)
            dlgMessage.Controls.Add(cmdIgnore)
        End Select
        If Not String.IsNullOrEmpty(Details) Then
          dlgMessage.DetailsExpanded = False
          dlgMessage.DetailsCollapsedLabel = ShowDetails
          dlgMessage.DetailsExpandedLabel = HideDetails
          dlgMessage.DetailsExpandedText = Details
          dlgMessage.ExpansionMode = DetailsMode
        End If
        If owner IsNot Nothing Then dlgMessage.OwnerWindowHandle = owner.Handle
        AddHandler dlgMessage.Opened, AddressOf RefreshDlg
        Dim ret As TaskDialogResult
        Try
          ret = dlgMessage.Show()
        Catch ex As Exception
          Return MsgDlgLegacy(owner, Text, Header, My.Application.Info.ProductName & " - " & Caption, Buttons, OldIcon, DefaultButton, Details, OldHelpLink)
        End Try
        Select Case ret
          Case TaskDialogResult.Yes : Return DialogResult.Yes
          Case TaskDialogResult.No : Return IIf(Buttons = MessageBoxButtons.AbortRetryIgnore, DialogResult.Abort, DialogResult.No)
          Case TaskDialogResult.Ok : Return DialogResult.OK
          Case TaskDialogResult.Cancel : Return DialogResult.Cancel
          Case TaskDialogResult.Close : Return DialogResult.Ignore
          Case TaskDialogResult.Retry : Return DialogResult.Retry
        End Select
        Return DialogResult.None
      End Using
    Else
      Return MsgDlgLegacy(owner, Text, Header, My.Application.Info.ProductName & " - " & Caption, Buttons, OldIcon, DefaultButton, Details, OldHelpLink)
    End If
  End Function
  Private Sub SelectionDialogButton_Click(sender As TaskDialogButton, e As EventArgs)
    Select Case sender.Name
      Case "cmdYes" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Yes)
      Case "cmdNo" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.No)
      Case "cmdCancel" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Cancel)
      Case "cmdClose" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Close)
      Case "cmdAbort" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.CustomButtonClicked)
      Case "cmdOK" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Ok)
      Case "cmdRetry" : CType(sender.HostingDialog, TaskDialog).Close(TaskDialogResult.Retry)
    End Select
  End Sub
  Private Sub SelectionDialogHyperlink_Click(sender As Object, e As TaskDialogHyperlinkClickedEventArgs)
    Try
      If String.IsNullOrEmpty(e.LinkText) Then Return
      If e.LinkText.Contains(" ") Then
        ShellEx(e.LinkText.Substring(0, e.LinkText.IndexOf(" ")), e.LinkText.Substring(e.LinkText.IndexOf(" ") + 1))
      Else
        Process.Start(e.LinkText)
      End If
    Catch ex As Exception
      Dim taskNotifier As TaskbarNotifier = Nothing
      MakeNotifier(taskNotifier, False)
      If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", My.Application.Info.ProductName & " could not navigate to """ & e.LinkText & """!" & vbNewLine & ex.Message, 200, 3000, 100)
    End Try
  End Sub
  Private Sub RefreshDlg(sender As Object, e As EventArgs)
    Dim dlg As TaskDialog = sender
    dlg.Icon = dlg.Icon
    dlg.InstructionText = dlg.InstructionText
  End Sub
  Private Function MsgDlgLegacy(owner As Form, Text As String, Optional Title As String = Nothing, Optional Caption As String = Nothing, Optional Buttons As MessageBoxButtons = MessageBoxButtons.OK, Optional Icon As MessageBoxIcon = MessageBoxIcon.None, Optional DefaultButton As MessageBoxDefaultButton = MessageBoxDefaultButton.Button1, Optional Details As String = Nothing, Optional HelpLink As Boolean = False) As DialogResult
    Dim Content As String
    Dim Link As String = Nothing
    Dim LinkText As String = Nothing
    If String.IsNullOrEmpty(Title) And String.IsNullOrEmpty(Text) Then
      Content = String.Empty
    ElseIf String.IsNullOrEmpty(Title) Then
      Content = Text
    ElseIf String.IsNullOrEmpty(Text) Then
      Content = Title
    Else
      If HelpLink Then
        If Text.Contains("<a") Then
          Link = Text.Substring(Text.IndexOf("<a"))
          Link = Link.Substring(Link.IndexOf("""") + 1)
          Link = Link.Substring(0, Link.IndexOf(""""))
          LinkText = Text.Substring(Text.IndexOf("<a"))
          LinkText = LinkText.Substring(LinkText.IndexOf(">") + 1)
          LinkText = LinkText.Substring(0, LinkText.IndexOf("<"))
          Text = Text.Substring(0, Text.IndexOf("<a")) & Text.Substring(Text.IndexOf("</a>") + 4)
          If Not Text.EndsWith(vbNewLine & vbNewLine) Then Text &= vbNewLine & vbNewLine
          Text &= "Click ""Help"" to " & LinkText & "."
        End If
      Else
        Do While Text.Contains("<a")
          Link = Text.Substring(Text.IndexOf("<a"))
          Link = Link.Substring(Link.IndexOf("""") + 1)
          Link = Link.Substring(0, Link.IndexOf(""""))
          LinkText = Text.Substring(Text.IndexOf("<a"))
          LinkText = LinkText.Substring(LinkText.IndexOf(">") + 1)
          LinkText = LinkText.Substring(0, LinkText.IndexOf("<"))
          Text = Text.Substring(0, Text.IndexOf("<a")) & LinkText & Text.Substring(Text.IndexOf("</a>") + 4)
          Link = Nothing
          LinkText = Nothing
        Loop
        If Text.EndsWith(vbNewLine & vbNewLine) Then Text = Text.Substring(0, Text.Length - 4)
      End If
      Content = Title & vbNewLine & vbNewLine & Text
    End If
    If Not String.IsNullOrEmpty(Details) Then
      If String.IsNullOrEmpty(Content) Then
        Content = Details
      Else
        Content &= vbNewLine & vbNewLine & Details
      End If
    End If
    If String.IsNullOrEmpty(Link) And String.IsNullOrEmpty(LinkText) Then
      Return MessageBox.Show(owner, Content, Caption, Buttons, Icon, DefaultButton)
    Else
      Return MessageBox.Show(owner, Content, Caption, Buttons, Icon, DefaultButton, Nothing, Link, HelpNavigator.Index)
    End If
  End Function
  Private Function LinkSplitPath(Path As String, Optional Separator As String = "\") As String
    If Path.EndsWith(Separator) Then Path = Path.Substring(0, Path.Length - Separator.Length)
    Dim PathStr As String = Nothing
    Dim sParts() As String = Split(Path, Separator)
    Dim Chunk As String = Nothing
    For Each part In sParts
      Chunk &= part & Separator
      If Not String.IsNullOrEmpty(part) Then
        If part = "http:" Or
          part = "ftp:" Or
          part = "file:" Then
          PathStr &= part & Separator
        Else
          PathStr &= "<a href=""explorer " & Chunk & """>" & part & Separator & "</a>"
        End If
      Else
        PathStr &= Separator
      End If
    Next
    If PathStr.EndsWith(Separator & "</a>") Then PathStr = PathStr.Substring(0, PathStr.Length - (4 + Separator.Length)) & "</a>"
    Return PathStr
  End Function
#End Region
End Module
