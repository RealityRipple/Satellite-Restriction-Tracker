Imports System.IO
Module modFunctions
  Public Function HostTypeToString(ht As localRestrictionTracker.SatHostTypes) As String
    Select Case ht
      Case localRestrictionTracker.SatHostTypes.WildBlue_LEGACY : Return "WBL"
      Case localRestrictionTracker.SatHostTypes.WildBlue_EXEDE : Return "WBX"
      Case localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY : Return "RPL"
      Case localRestrictionTracker.SatHostTypes.RuralPortal_EXEDE : Return "RPX"
      Case localRestrictionTracker.SatHostTypes.DishNet_EXEDE : Return "DNX"
      Case Else : Return "O"
    End Select
  End Function
  Public Function StringToHostType(st As String) As localRestrictionTracker.SatHostTypes
    Select Case st.ToUpper
      Case "WBL" : Return localRestrictionTracker.SatHostTypes.WildBlue_LEGACY
      Case "WBX", "WBV" : Return localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
      Case "RPL" : Return localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY
      Case "RPX" : Return localRestrictionTracker.SatHostTypes.RuralPortal_EXEDE
      Case "DNX" : Return localRestrictionTracker.SatHostTypes.DishNet_EXEDE
      Case "WILDBLUE" : Return localRestrictionTracker.SatHostTypes.WildBlue_LEGACY
      Case "EXEDE" : Return localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
      Case "DISHNET" : Return localRestrictionTracker.SatHostTypes.DishNet_EXEDE
      Case "RURALPORTAL" : Return localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY
      Case Else : Return localRestrictionTracker.SatHostTypes.Other
    End Select
  End Function
  Private Class MCIPlayer
    Implements IDisposable
    Private sAlias As String
    <Runtime.InteropServices.DllImport("winmm.dll")>
    Private Shared Function mciSendString(ByVal strCommand As String, ByVal strReturn As System.Text.StringBuilder, ByVal iReturnLength As Integer, ByVal hwndCallback As IntPtr) As Long
    End Function
    Public Sub New(ByVal sFileName As String)
      If Status() <> "" Then
        [Stop]()
        Close()
      End If
      sAlias = IO.Path.GetFileNameWithoutExtension(sFileName)
      Dim sCommand As String = "open """ & sFileName & """ type mpegvideo alias " & sAlias
      mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Sub Close()
      Dim sCommand As String = "close " & sAlias
      mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
      sAlias = String.Empty
    End Sub
    Public Sub CloseAll()
      Dim sCommand As String = "close all wait"
      mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Sub Pause()
      Dim sCommand As String = "pause " & sAlias
      mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Sub Play(Optional Repeat As Boolean = False)
      Dim sCommand As String = "play " & sAlias & IIf(Repeat, " repeat", String.Empty)
      mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Sub [Resume]()
      Dim sCommand As String = "play " & sAlias & " from " & CLng(Status())
      mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Sub [Stop]()
      Dim sCommand As String = "stop " & sAlias
      mciSendString(sCommand, Nothing, 0, IntPtr.Zero)
    End Sub
    Public Function Status() As String
      Dim sBuffer As New System.Text.StringBuilder(128)
      mciSendString("status " & sAlias & " mode", sBuffer, sBuffer.Capacity, IntPtr.Zero)
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
  Public Const LATIN_1 As Integer = 28591
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


      Dim locData As String = My.Computer.FileSystem.ReadAllText(LocPath, System.Text.Encoding.GetEncoding(LATIN_1))
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
        Dim iR As Integer = Integer.Parse(ID.Substring(0, 2), Globalization.NumberStyles.HexNumber)
        Dim iG As Integer = Integer.Parse(ID.Substring(2, 2), Globalization.NumberStyles.HexNumber)
        Dim iB As Integer = Integer.Parse(ID.Substring(4, 2), Globalization.NumberStyles.HexNumber)
        Return Color.FromArgb(iR, iG, iB)
      Catch ex As Exception
        Return Color.Transparent
      End Try
    End Function
    Private Function RectIDToRect(ID As String) As Rectangle
      Try
        ID = ID.Replace(" ", "")
        Dim IDs() As String = Split(ID, ",")
        Dim x As Integer = Integer.Parse(IDs(0))
        Dim y As Integer = Integer.Parse(IDs(1))
        Dim w As Integer = Integer.Parse(IDs(2))
        Dim h As Integer = Integer.Parse(IDs(3))
        Return New Rectangle(x, y, w, h)
      Catch ex As Exception
        Return Rectangle.Empty
      End Try
    End Function
    Private Function PointIDToPoint(ID As String) As Point
      Try
        ID = ID.Replace(" ", "")
        Dim IDs() As String = Split(ID, ",")
        Dim x As Integer = Integer.Parse(IDs(0))
        Dim y As Integer = Integer.Parse(IDs(1))
        Return New Point(x, y)
      Catch ex As Exception
        Return Point.Empty
      End Try
    End Function
  End Class
  Public Function LoadAlertStyle(Path As String) As NotifierStyle
    If My.Computer.FileSystem.FileExists(AppDataPath & Path & ".tgz") Then
      Path = AppDataPath & Path & ".tgz"
    ElseIf My.Computer.FileSystem.FileExists(AppDataPath & Path & ".tar.gz") Then
      Path = AppDataPath & Path & ".tar.gz"
    ElseIf My.Computer.FileSystem.FileExists(AppDataPath & Path & ".tar") Then
      Path = AppDataPath & Path & ".tar"
    Else
      Return New NotifierStyle
    End If
    Try
      Dim TempAlertDir As String = AppData & "notifier\"
      Dim TempAlertTAR As String = AppData & "notifier.tar"
      If Path.EndsWith(".tar") Then
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
      Dim ns As New NotifierStyle(TempAlertDir & "alert.png", TempAlertDir & "close.png", TempAlertDir & "loc")
      My.Computer.FileSystem.DeleteDirectory(TempAlertDir, FileIO.DeleteDirectoryOption.DeleteAllContents)
      Return ns
    Catch ex As Exception
      Return New NotifierStyle
    End Try
  End Function
  Private Sub ExtractGZ(sGZ As String, sDestFile As String)
    Using sourceTGZ As New IO.FileStream(sGZ, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
      Using sourceGZ As New System.IO.Compression.GZipStream(sourceTGZ, IO.Compression.CompressionMode.Decompress)
        Using destTAR As IO.FileStream = IO.File.Create(sDestFile)
          Dim buffer As Byte()
          ReDim buffer(4095)
          Dim numRead As Integer = sourceGZ.Read(buffer, 0, buffer.Length)
          Do While numRead <> 0
            destTAR.Write(buffer, 0, numRead)
            numRead = sourceGZ.Read(buffer, 0, buffer.Length)
          Loop
          destTAR.Flush(True)
          destTAR.Close()
        End Using
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
        Return Byte.Parse(sRet)
      Else
        Return 0
      End If
    End Function
    Private Function ReadBInt(inBytes() As Byte) As UInt32
      Dim sRet As String = ReadBString(inBytes)
      If Not String.IsNullOrEmpty(sRet) Then
        Return UInt32.Parse(sRet)
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
    If Not sDestPath.EndsWith("\") Then sDestPath &= "\"
    Using sourceTAR As New IO.FileStream(sTAR, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
      Using binTar As New IO.BinaryReader(sourceTAR)
        Do While binTar.BaseStream.Position < binTar.BaseStream.Length
          Dim tarFile As New TarFileData(binTar)
          If Not String.IsNullOrEmpty(tarFile.FileName) Then
            If tarFile.LinkIndicator = 0 Then
              My.Computer.FileSystem.WriteAllBytes(sDestPath & tarFile.FileName, tarFile.FileData, False)
              IO.File.SetLastWriteTime(sDestPath & tarFile.FileName, New Date(1970, 1, 1).AddSeconds(tarFile.LastMod))
            End If

          End If
        Loop
      End Using
    End Using
  End Sub

  Public Sub MakeNotifier(ByRef taskNotifier As TaskbarNotifier, ContentClickable As Boolean, Optional customStyle As NotifierStyle = Nothing)
    If customStyle Is Nothing Then customStyle = NOTIFIER_STYLE
    If customStyle.Background Is Nothing Or customStyle.CloseButton Is Nothing Then
      taskNotifier = Nothing
      Exit Sub
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
      My.Computer.FileSystem.WriteAllBytes(AppData & "Song.mid", My.Resources.Song, False)
      Song = New MCIPlayer(AppData & "Song.mid")
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
    If My.Computer.FileSystem.FileExists(AppDataPath & "Song.mid") Then
      Try
        My.Computer.FileSystem.DeleteFile(AppData & "Song.mid")
      Catch ex As Exception
      End Try
    End If
  End Sub
  Public ReadOnly Property AppDataPath As String
    Get
      Return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName & "\" & Application.ProductName & "\"
    End Get
  End Property
  Public ReadOnly Property AppData As String
    Get
      Static sTmp As String
      If Application.StartupPath.Contains(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)) Or Not My.Computer.FileSystem.DirectoryExists(Application.StartupPath & "\Config\") Then
        If Not My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName) Then My.Computer.FileSystem.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName)
        If Not My.Computer.FileSystem.DirectoryExists(AppDataPath) Then My.Computer.FileSystem.CreateDirectory(AppDataPath)
        If String.IsNullOrEmpty(sTmp) Then sTmp = AppDataPath
      Else
        If String.IsNullOrEmpty(sTmp) Then
          sTmp = Application.StartupPath & "\Config\"
        End If
      End If
      Return sTmp
    End Get
  End Property
  Public ReadOnly Property AppDataAllPath As String
    Get
      Return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & "\" & Application.CompanyName & "\" & Application.ProductName & "\"
    End Get
  End Property
  Public ReadOnly Property AppDataAll As String
    Get
      Static sTmp As String
      Static OneAlert As Boolean
      If Not My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & "\" & Application.CompanyName) Then My.Computer.FileSystem.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & "\" & Application.CompanyName)
      If Not My.Computer.FileSystem.DirectoryExists(AppDataAllPath) Then My.Computer.FileSystem.CreateDirectory(AppDataAllPath)
      If String.IsNullOrEmpty(sTmp) Then sTmp = AppDataAllPath

      Dim AppDataDir As String = IO.Path.GetDirectoryName(sTmp)
      Dim ADDOK As Boolean = GrantFullControlToEveryone(AppDataDir)
      Dim ADOK As Boolean = GrantFullControlToEveryone(sTmp)
      If ADOK And ADDOK Then

      ElseIf ADOK Then

      ElseIf ADDOK Then
        If Not OneAlert Then
          MessageBox.Show("Failed to set permissions for the directory """ & sTmp & """." & vbNewLine & "Please run " & My.Application.Info.Title & " as Administrator to correct this problem.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
          OneAlert = True
        End If
      Else
        If Not OneAlert Then
          MessageBox.Show("Failed to set permissions for the directory """ & sTmp & """ and its parent." & vbNewLine & "Please run " & My.Application.Info.Title & " as Administrator to correct this problem.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
          OneAlert = True
        End If
      End If
      Return sTmp
    End Get
  End Property
  Public ReadOnly Property UpdateParam As String
    Get
      If AppDataPath = Application.StartupPath & "\Config\" Then
        Return "/silent /dir=""" & Application.StartupPath & """ /type=portable"
      Else
        Return "/silent"
      End If
    End Get
  End Property
  Public ReadOnly Property MySaveDir(Optional Create As Boolean = False) As String
    Get
      Dim mySettings As New AppSettings
      If Application.StartupPath.Contains(Environment.SpecialFolder.ProgramFiles) Or Not My.Computer.FileSystem.DirectoryExists(Application.StartupPath & "\Config\") Then
        If String.IsNullOrEmpty(mySettings.HistoryDir) Then
          If My.Computer.FileSystem.DirectoryExists(AppDataPath) Then
            If Array.Exists(My.Computer.FileSystem.GetFiles(AppDataPath).ToArray, Function(appFile As String) IO.Path.GetExtension(appFile).ToLower = ".xml" Or IO.Path.GetExtension(appFile).ToLower = ".wb") Then
              mySettings.HistoryDir = IIf(Create, AppData, AppDataPath)
            Else
              mySettings.HistoryDir = IIf(Create, AppDataAll, AppDataAllPath)
            End If
          Else
            mySettings.HistoryDir = IIf(Create, AppDataAll, AppDataAllPath)
          End If
        End If
      Else
        mySettings.HistoryDir = Application.StartupPath & "\Config\"
      End If
      If Create Then
        Try
          If Not My.Computer.FileSystem.DirectoryExists(mySettings.HistoryDir) Then My.Computer.FileSystem.CreateDirectory(mySettings.HistoryDir)
        Catch ex As Exception
          Return AppDataAll
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
    If InBytes >= 1000 Then
      If InBytes / 1024 >= 1000 Then
        If InBytes / 1024 / 1024 >= 1000 Then
          If InBytes / 1024 / 1024 / 1024 >= 1000 Then
            Return Format((InBytes) / 1024 / 1024 / 1024 / 1024, "0.##") & " TB"
          Else
            Return Format((InBytes) / 1024 / 1024 / 1024, "0.##") & " GB"
          End If
        Else
          Return Format((InBytes) / 1024 / 1024, "0.##") & " MB"
        End If
      Else
        Return Format((InBytes) / 1024, "0.#") & " KB"
      End If
    Else
      Return InBytes & " B"
    End If
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
          sTmp = Environment.GetFolderPath(Environment.SpecialFolder.Startup) & " with Internet Access\Satellite Restriction Tracker.lnk"
        Else
          sTmp = Environment.GetFolderPath(Environment.SpecialFolder.Startup) & "\Satellite Restriction Tracker.lnk"
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
  Public Sub SaveToFTP(sData As String)
    Try
      Dim sFailFile As String = "SRT-ReadFail-" & Now.ToString("G") & "-v" & Application.ProductVersion & ".txt"
      sFailFile = Replace(sFailFile, "/", "-")
      sFailFile = Replace(sFailFile, ":", "-")
      Dim ftpSave As Net.FtpWebRequest = Net.FtpWebRequest.Create("ftp://realityripple.com/" & sFailFile)
      ftpSave.Proxy = Nothing
      ftpSave.Credentials = FTPCredentials 'Use [New Net.NetworkCredential("FTPUSER", "FTPPASS")] to upload failures to a FTP location.
      ftpSave.Method = Net.WebRequestMethods.Ftp.UploadFile
      Using ftpStream As IO.Stream = ftpSave.GetRequestStream
        Dim bHTTP As Byte() = System.Text.Encoding.UTF8.GetBytes(sData)
        ftpStream.Write(bHTTP, 0, bHTTP.Length)
        ftpStream.Close()
      End Using
      frmMain.FailResponse(True)
    Catch ex As Exception
      frmMain.FailResponse(False)
    End Try
  End Sub
  Public Function PercentEncode(inString As String) As String
    Dim sRet As String = String.Empty
    For I As Integer = inString.Length - 1 To 0 Step -1
      Dim iChar As Integer = Asc(inString(I))
      Select Case iChar
        Case 48 To 57, 65 To 90, 97 To 122 : sRet = inString(I) & sRet
        Case 32 : sRet = "+" & sRet
        Case Else : sRet = "%" & PadHex(iChar, 2) & sRet
      End Select
    Next
    Return sRet
  End Function
  Private Function PadHex(Value As UInt32, Length As UInt16) As String
    Dim sVal As String = Hex(Value)
    Do While sVal.Length < Length : sVal = "0" & sVal : Loop
    Return sVal
  End Function
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
              My.Computer.FileSystem.CopyFile(file, ToDir & "\" & sFName, True)
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
            My.Computer.FileSystem.CopyFile(file, ToDir & "\" & sFName, True)
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
      NativeMethods.SystemParametersInfo(NativeMethods.SPI_GETANIMATION, ai.Size, ai, 0)
      MinAnimation = ai.MinAnimate
    End Get
  End Property
  Public Sub AnimateWindow(window As Form, ToTray As Boolean)
    If MinAnimation Then
      Dim screenRect As Rectangle = Screen.GetBounds(window.Bounds.Location)
      Dim destPoint As Point
      Select Case TaskBarPosition.GetTaskBarEdge(window.Handle)
        Case TaskBarPosition.ABEdge.ABE_BOTTOM
          destPoint = New Point(screenRect.Width - 96, screenRect.Height - 16)
        Case TaskBarPosition.ABEdge.ABE_TOP
          destPoint = New Point(screenRect.Width - 96, 16)
        Case TaskBarPosition.ABEdge.ABE_LEFT
          destPoint = New Point(16, screenRect.Height - 96)
        Case TaskBarPosition.ABEdge.ABE_RIGHT
          destPoint = New Point(screenRect.Width - 16, screenRect.Height - 96)
      End Select
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
      Dim startPoint As Point = window.Bounds.Location
      Dim dWidth As Integer = destPoint.X - startPoint.X
      Dim dHeight As Integer = destPoint.Y - startPoint.Y
      Dim startWidth As Integer = window.Bounds.Width
      Dim startHeight As Integer = window.Bounds.Height
      For I As Single = a To b Step s
        curPoint = New Point(startPoint.X + I * dWidth, startPoint.Y + I * dHeight)
        curSize = New Size((1 - I) * startWidth, (1 - I) * startHeight)
        Dim drawRect As New Rectangle(curPoint, curSize)
        ControlPaint.DrawReversibleFrame(drawRect, window.BackColor, FrameStyle.Thick)
        System.Threading.Thread.Sleep(1)
        ControlPaint.DrawReversibleFrame(drawRect, window.BackColor, FrameStyle.Thick)
      Next
    End If
  End Sub
#Region "Graphs"
#Region "History"
  Private dGraph, uGraph As Rectangle
  Private oldDate, newDate As Date
  Private dData(), uData() As DataBase.DataRow
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
    Return DrawGraph(Data, TriState.UseDefault, ImgSize, ColorLine, ColorA, ColorB, ColorC, ColorText, ColorBG, ColorMax, ColorGridLight, ColorGridDark)
  End Function
  Public Function DrawLineGraph(Data() As DataBase.DataRow, Down As Boolean, ImgSize As Size, ColorLine As Color, ColorA As Color, ColorB As Color, ColorC As Color, ColorText As Color, ColorBG As Color, ColorMax As Color, ColorGridLight As Color, ColorGridDark As Color) As Image
    Return DrawGraph(Data, IIf(Down, TriState.True, TriState.False), ImgSize, ColorLine, ColorA, ColorB, ColorC, ColorText, ColorBG, ColorMax, ColorGridLight, ColorGridDark)
  End Function
  Private Function DrawGraph(ByVal Data() As DataBase.DataRow, Down As TriState, ImgSize As Size, ColorLine As Color, ColorA As Color, ColorB As Color, ColorC As Color, ColorText As Color, ColorBG As Color, ColorMax As Color, ColorGridLight As Color, ColorGridDark As Color) As Image
    If Data Is Nothing OrElse Data.Length = 0 Then Return New Bitmap(1, 1)
    Dim yMax As Long = 0
    Dim lMax As Long = 0
    If Down = TriState.UseDefault Then
      Dim yVMax As Long = 0
      For I As Long = 0 To Data.Length - 1
        If yVMax < Data(I).DOWNLOAD Then yVMax = Data(I).DOWNLOAD
        If yVMax < Data(I).DOWNLIM Then yVMax = Data(I).DOWNLIM
      Next
      yMax = yVMax
      If Not yMax Mod 1000 = 0 Then yMax = (yMax \ 1000) * 1000
      lMax = yVMax
    Else
      Dim yDMax As Long = 0
      Dim yUMax As Long = 0
      For I As Long = 0 To Data.Length - 1
        If yDMax < Data(I).DOWNLOAD Then yDMax = Data(I).DOWNLOAD
        If yUMax < Data(I).UPLOAD Then yUMax = Data(I).UPLOAD
        If yDMax < Data(I).DOWNLIM Then yDMax = Data(I).DOWNLIM
        If yUMax < Data(I).UPLIM Then yUMax = Data(I).UPLIM
      Next
      yMax = IIf(yDMax > yUMax, yDMax, yUMax)
      If Not yMax Mod 1000 = 0 Then yMax = (yMax \ 1000) * 1000
      lMax = IIf(Down = TriState.True, yDMax, yUMax)
    End If
    If Not lMax Mod 1000 = 0 Then lMax = (lMax \ 1000) * 1000 + 1000
    Dim iPic As Image = New Bitmap(ImgSize.Width, ImgSize.Height)
    Dim g As Graphics = Graphics.FromImage(iPic)
    Dim tFont As New Font(FontFamily.GenericSansSerif, 7)
    g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
    Dim lYWidth As Integer = g.MeasureString(yMax.ToString.Trim & " MB", tFont).Width + 10
    Dim lXHeight As Integer = g.MeasureString(Now.ToString("g"), tFont).Height + 10
    g.Clear(ColorBG)
    Dim yTop As Integer = lXHeight / 2
    Dim yHeight As Integer = ImgSize.Height - (lXHeight * 1.5)
    If Down = TriState.False Then
      uGraph = New Rectangle(lYWidth, yTop, (ImgSize.Width - 4) - lYWidth, yHeight)
      uData = Data
    Else
      dGraph = New Rectangle(lYWidth, yTop, (ImgSize.Width - 4) - lYWidth, yHeight)
      dData = Data
    End If
    g.DrawLine(New Pen(ColorText), lYWidth, yTop, lYWidth, yTop + yHeight)
    g.DrawLine(New Pen(ColorText), lYWidth, yTop + yHeight, ImgSize.Width, yTop + yHeight)
    oldDate = Data.First.DATETIME
    newDate = Data.Last.DATETIME
    For I As Integer = 0 To lMax Step (((lMax \ (yHeight \ (tFont.Size + 12)))) \ 100) * 100
      Dim iY As Integer = yTop + yHeight - (I / lMax * yHeight)
      g.DrawString(I.ToString.Trim & " MB", tFont, New SolidBrush(ColorText), lYWidth - g.MeasureString(I.ToString.Trim & " MB", tFont).Width, iY - (g.MeasureString(I.ToString.Trim & " MB", tFont).Height / 2))
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
    Dim dInterval As DateInterval = DateInterval.Minute
    Dim lInterval As UInteger = 1
    Dim lBitInterval As Double = 1.0
    Dim lLabelInterval As UInteger = 5
    Select Case Math.Abs(DateDiff(DateInterval.Minute, lStart, lEnd))
      Case Is <= 61
        lInterval = 5
        lBitInterval = 1
        lLabelInterval = 10
        dInterval = DateInterval.Minute
      Case Is < 60 * 13
        lInterval = 60
        lBitInterval = 30
        lLabelInterval = 60 * 2
        dInterval = DateInterval.Minute
      Case Is <= 60 * 24
        lInterval = 6
        lBitInterval = 1
        lLabelInterval = 6
        dInterval = DateInterval.Hour
      Case Is <= 60 * 24 * 2
        lInterval = 12
        lBitInterval = 6
        lLabelInterval = 24
        dInterval = DateInterval.Hour
      Case Is <= 60 * 24 * 6
        lInterval = 24
        lBitInterval = 12
        lLabelInterval = 24
        dInterval = DateInterval.Hour
      Case Is <= 60 * 24 * 12
        lInterval = 2
        lBitInterval = 1
        lLabelInterval = 2
        dInterval = DateInterval.Day
      Case Is <= 60 * 24 * 20
        lInterval = 2
        lBitInterval = 1
        lLabelInterval = 4
        dInterval = DateInterval.Day
      Case Is <= 60 * 24 * 27
        lInterval = 2
        lBitInterval = 1
        lLabelInterval = 7
        dInterval = DateInterval.Day
      Case Is <= 60 * 24 * 40
        lInterval = 4
        lBitInterval = 2
        lLabelInterval = 7
        dInterval = DateInterval.Day
      Case Is <= 60 * 24 * 90
        lInterval = 14
        lBitInterval = 7
        lLabelInterval = 14
        dInterval = DateInterval.Day
      Case Is <= 60 * 24 * 240
        lInterval = 30
        lBitInterval = 15
        lLabelInterval = 30
        dInterval = DateInterval.Day
      Case Is <= 60 * 24 * 365
        lInterval = 60
        lBitInterval = 30
        lLabelInterval = 90
        dInterval = DateInterval.Day
      Case Is <= 60 * 24 * 365 * 2
        lInterval = 90
        lBitInterval = 60
        lLabelInterval = 180
        dInterval = DateInterval.Day
      Case Else
        lInterval = 365
        lBitInterval = 180
        lLabelInterval = 365
        dInterval = DateInterval.Day
    End Select
    Dim lMaxTime As Long = Math.Abs(DateDiff(dInterval, lStart, lEnd))
    If lMaxTime = 0 Then Return New Bitmap(1, 1)
    Dim lLineWidth As Long = (ImgSize.Width - 4) - lYWidth - 1
    Dim dCompInter As Double = lLineWidth / lMaxTime
    For I As Double = 0 To lMaxTime Step lBitInterval
      Dim lX As Integer = lYWidth + (I * dCompInter) + 1
      If I > 0 Then g.DrawLine(New Pen(ColorGridLight), lX, yTop, lX, ImgSize.Height - (lXHeight + 5))
    Next
    For I As Long = 0 To lMaxTime Step lInterval
      Dim lX As Integer = lYWidth + (I * dCompInter) + 1
      g.DrawLine(New Pen(ColorText), lX, ImgSize.Height - (lXHeight - 3), lX, ImgSize.Height - lXHeight)
      If I > 0 Then g.DrawLine(New Pen(ColorGridDark), lX, yTop, lX, ImgSize.Height - (lXHeight + 5))
    Next I
    Dim lastI As Long = lYWidth + (lMaxTime * dCompInter)
    If lastI >= (ImgSize.Width - 4) Then lastI = (ImgSize.Width - 4)
    Dim sDispV As String = "g"
    Select Case DateDiff(DateInterval.Day, lStart, lEnd)
      Case Is > 1 : sDispV = "d"
      Case Is < 1 : sDispV = "t"
      Case Else : sDispV = "g"
    End Select
    Dim sLastDisp As String = lEnd.ToString(sDispV)
    Dim iLastDispWidth As Single = g.MeasureString(sLastDisp, tFont).Width
    For I As Long = 0 To lMaxTime Step lLabelInterval
      Dim lX As Integer = lYWidth + (I * dCompInter) + 1
      Dim sDisp As String = DateAdd(dInterval, I, lStart).ToString(sDispV)
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
    If Down = TriState.UseDefault Then
      MaxY = yTop + yHeight - (Data(Data.Length - 1).DOWNLIM / lMax * yHeight)
    Else
      MaxY = yTop + yHeight - (IIf(Down = TriState.True, Data(Data.Length - 1).DOWNLIM, Data(Data.Length - 1).UPLIM) / lMax * yHeight)
    End If
    Dim lMaxPoints(lMaxTime) As Point
    Dim lPoints(lMaxTime + 3) As Point
    Dim lTypes(lMaxTime + 3) As Byte
    Dim lastLVal As Long = 0
    For I As Long = 0 To lMaxTime
      Dim lVal As Long = -1
      Dim lLow As Long = Long.MaxValue
      Dim lHigh As Long = 0
      For J As Integer = 0 To Data.Length - 1
        If Math.Abs(DateDiff(dInterval, Data(J).DATETIME, DateAdd(dInterval, I, lStart))) = 0 Then
          Dim jLim As Long = 0
          If Down = TriState.False Then
            jLim = Data(J).UPLIM
          Else
            jLim = Data(J).DOWNLIM
          End If
          If lHigh < jLim Then
            lHigh = jLim
          End If
          If lLow > jLim Then
            lLow = jLim
          End If
        End If
      Next
      If lHigh > 0 And lLow < Long.MaxValue Then lVal = (lHigh + lLow) / 2
      If lVal = -1 And lastLVal > 0 Then lVal = lastLVal
      lMaxPoints(I).X = lYWidth + (I * dCompInter) + 1
      lMaxPoints(I).Y = yTop + yHeight - (lVal / lMax * yHeight)
      If I > 0 AndAlso (lMaxPoints(I - 1).X = 0 And lMaxPoints(I - 1).Y = 0) Then
        Dim J As Long = 1
        While lMaxPoints(I - J).Y = 0
          J += 1
        End While
        For K As Long = 1 To J - 1
          lMaxPoints(I - K).X = lYWidth + ((I - K) * dCompInter) + 1
          lMaxPoints(I - K).Y = (lMaxPoints(I - J).Y + lMaxPoints(I).Y) / 2
        Next
      End If
      lastLVal = lVal
    Next I
    lastLVal = 0
    For I As Long = 0 To lMaxTime
      Dim lVal As Long = -1
      Dim lLow As Long = Long.MaxValue
      Dim lHigh As Long = 0
      For J As Integer = 0 To Data.Length - 1
        If Math.Abs(DateDiff(dInterval, Data(J).DATETIME, DateAdd(dInterval, I, lStart))) = 0 Then
          Dim jVal As Long = 0
          If Down = TriState.False Then
            jVal = Data(J).UPLOAD
          Else
            jVal = Data(J).DOWNLOAD
          End If
          If lHigh < jVal Then
            lHigh = jVal
          End If
          If lLow > jVal Then
            lLow = jVal
          End If
        End If
      Next
      If lHigh > 0 And lLow < Long.MaxValue Then lVal = (lHigh + lLow) / 2
      If lVal = -1 And lastLVal > 0 Then lVal = lastLVal
      lPoints(I).X = lYWidth + (I * dCompInter) + IIf(I > 0, 1, 0)
      lPoints(I).Y = yTop + yHeight - (lVal / lMax * yHeight)
      If I > 0 AndAlso (lPoints(I - 1).X = 0 And lPoints(I - 1).Y = 0) Then
        Dim J As Long = 1
        While lPoints(I - J).Y = 0
          J += 1
        End While
        For K As Long = 1 To J - 1
          lPoints(I - K).X = lYWidth + ((I - K) * dCompInter) + 1
          lPoints(I - K).Y = (lPoints(I - J).Y + lPoints(I).Y) / 2
        Next
      End If
      lastLVal = lVal
    Next I
    If lPoints(lMaxTime).IsEmpty Then lPoints(lMaxTime) = New Point(ImgSize.Width, yTop + yHeight)
    lPoints(lMaxTime + 1) = New Point(ImgSize.Width, yTop + yHeight)
    lPoints(lMaxTime + 2) = New Point(lYWidth, yTop + yHeight)
    lPoints(lMaxTime + 3) = lPoints(0)
    lTypes(0) = Drawing2D.PathPointType.Start
    For I As Long = 1 To lMaxTime + 2
      lTypes(I) = Drawing2D.PathPointType.Line
    Next
    lTypes(lMaxTime + 3) = Drawing2D.PathPointType.Line Or Drawing2D.PathPointType.CloseSubpath
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
    If lLim = 0 Then Exit Sub
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
    If lLim = 0 Then Exit Sub
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
    If Colors.HistoryUpLine = Color.Transparent Or Colors.HistoryUpLine.A < 255 Then Colors.HistoryUpLine = defaultColors.HistoryUpLine
    If Colors.HistoryUpA = Color.Transparent Or Colors.HistoryUpA.A < 255 Then Colors.HistoryUpA = defaultColors.HistoryUpA
    If Colors.HistoryUpB = Color.Transparent Or Colors.HistoryUpB.A < 255 Then Colors.HistoryUpB = defaultColors.HistoryUpB
    If Colors.HistoryUpC = Color.Transparent Or Colors.HistoryUpC.A < 255 Then Colors.HistoryUpC = defaultColors.HistoryUpC
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

      Case localRestrictionTracker.SatHostTypes.DishNet_EXEDE
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
  ''' <summary>
  ''' Attempts to see if a file is in use, waiting up to five seconds for it to be freed.
  ''' </summary>
  ''' <param name="Filename">The exact path to the file which needs to be checked.</param>
  ''' <param name="access">Write permissions required for checking.</param>
  ''' <returns>True on available, false on in use.</returns>
  ''' <remarks></remarks>
  Public Function InUseChecker(Filename As String, access As IO.FileAccess) As Boolean
    If Not My.Computer.FileSystem.FileExists(Filename) Then Return True
    Dim iStart As Long = TickCount()
    Do
      Try
        Select Case access
          Case FileAccess.Read
            Using fs As FileStream = IO.File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite Or FileShare.Delete)
              If fs.CanRead Then
                Return True
                Exit Do
              End If
            End Using
          Case FileAccess.Write, FileAccess.ReadWrite
            Using fs As FileStream = IO.File.Open(Filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite Or FileShare.Delete)
              If fs.CanWrite Then
                Return True
                Exit Do
              End If
            End Using
        End Select
      Catch ex As Exception
      End Try
      Application.DoEvents()
    Loop While TickCount() - iStart < 5000
    Return False
  End Function
  Public Function TickCount() As Long
    Return (Stopwatch.GetTimestamp / Stopwatch.Frequency) * 1000
  End Function
End Module
