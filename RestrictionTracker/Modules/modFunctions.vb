﻿Imports System.IO
Module modFunctions
  Public Function HostTypeToString(ht As localRestrictionTracker.SatHostTypes) As String
    Select Case ht
      Case localRestrictionTracker.SatHostTypes.WildBlue_LEGACY : Return "WBL"
      Case localRestrictionTracker.SatHostTypes.WildBlue_EXEDE : Return "WBX"
      Case localRestrictionTracker.SatHostTypes.WildBlue_EVOLUTION : Return "WBV"
      Case localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY : Return "RPL"
      Case localRestrictionTracker.SatHostTypes.RuralPortal_EXEDE : Return "RPX"
      Case localRestrictionTracker.SatHostTypes.DishNet_EXEDE : Return "DNX"
      Case Else : Return "O"
    End Select
  End Function
  Public Function StringToHostType(st As String) As localRestrictionTracker.SatHostTypes
    Select Case st.ToUpper
      Case "WBL" : Return localRestrictionTracker.SatHostTypes.WildBlue_LEGACY
      Case "WBX" : Return localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
      Case "WBV" : Return localRestrictionTracker.SatHostTypes.WildBlue_EVOLUTION
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
    If My.Computer.FileSystem.FileExists(AppData & "\" & Path & ".tgz") Then
      Path = AppData & "\" & Path & ".tgz"
    ElseIf My.Computer.FileSystem.FileExists(AppData & "\" & Path & ".tar.gz") Then
      Path = AppData & "\" & Path & ".tar.gz"
    Else
      Return New NotifierStyle
    End If
    Try
      Dim TempAlertDir As String = AppData & "\notifier\"
      Dim TempAlertTAR As String = AppData & "\notifier.tar"
      ExtractGZ(Path, TempAlertTAR)
      ExtractTar(TempAlertTAR, TempAlertDir)
      My.Computer.FileSystem.DeleteFile(TempAlertTAR)
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
      If Not My.Computer.FileSystem.DirectoryExists(IO.Path.GetDirectoryName(AppData)) Then My.Computer.FileSystem.CreateDirectory(IO.Path.GetDirectoryName(AppData))
      If Not My.Computer.FileSystem.DirectoryExists(AppData) Then My.Computer.FileSystem.CreateDirectory(AppData)
      My.Computer.FileSystem.WriteAllBytes(AppData & "\Song.mid", My.Resources.Song, False)
      Song = New MCIPlayer(AppData & "\Song.mid")
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
    If My.Computer.FileSystem.FileExists(AppData & "\Song.mid") Then
      Try
        My.Computer.FileSystem.DeleteFile(AppData & "\Song.mid")
      Catch ex As Exception
      End Try
    End If
  End Sub
  Public ReadOnly Property AppData As String
    Get
      Static sTmp As String
      If Not My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName) Then My.Computer.FileSystem.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName)
      If Not My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName & "\" & Application.ProductName) Then My.Computer.FileSystem.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName & "\" & Application.ProductName)
      If String.IsNullOrEmpty(sTmp) Then
        sTmp = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & Application.CompanyName & "\" & Application.ProductName
      End If
      Return sTmp
    End Get
  End Property
  Public ReadOnly Property AppDataAll As String
    Get
      Static sTmp As String
      Static OneAlert As Boolean
      If String.IsNullOrEmpty(sTmp) Then
        sTmp = IO.Path.GetDirectoryName(My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData)
        Try
          My.Computer.FileSystem.DeleteDirectory(My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData, FileIO.DeleteDirectoryOption.DeleteAllContents)
        Catch ex As Exception

        End Try
        Dim AppDataDir As String = IO.Path.GetDirectoryName(sTmp)
        Dim ADDOK As Boolean = GrantFullControlToEveryone(AppDataDir)
        Dim ADOK As Boolean = GrantFullControlToEveryone(sTmp)
        If ADOK And ADDOK Then
          'Both OK
        ElseIf ADOK Then
          'Bad Parent, OK child
        ElseIf ADDOK Then
          'Bad Child, OK Parent
          If Not OneAlert Then
            MsgBox("Failed to set permissions for the directory """ & sTmp & """." & vbNewLine & "Please run " & My.Application.Info.Title & " as Administrator to correct this problem.", MsgBoxStyle.Critical, "Permission Failure")
            OneAlert = True
          End If
        Else
          'Bad Both
          If Not OneAlert Then
            MsgBox("Failed to set permissions for the directory """ & sTmp & """ and its parent." & vbNewLine & "Please run " & My.Application.Info.Title & " as Administrator to correct this problem.", MsgBoxStyle.Critical, "Permission Failure")
            OneAlert = True
          End If
        End If
      End If
      Return sTmp
    End Get
  End Property
  Public ReadOnly Property MySaveDir As String
    Get
      Dim mySettings As New AppSettings
      If String.IsNullOrEmpty(mySettings.HistoryDir) Then
        If My.Computer.FileSystem.DirectoryExists(AppData) Then
          If Array.Exists(My.Computer.FileSystem.GetFiles(AppData).ToArray, Function(appFile As String) IO.Path.GetExtension(appFile).ToLower = ".xml" Or IO.Path.GetExtension(appFile).ToLower = ".wb") Then
            mySettings.HistoryDir = AppData
          Else
            mySettings.HistoryDir = AppDataAll
          End If
        Else
          mySettings.HistoryDir = AppDataAll
        End If
      End If
      Try
        If Not My.Computer.FileSystem.DirectoryExists(mySettings.HistoryDir) Then My.Computer.FileSystem.CreateDirectory(mySettings.HistoryDir)
      Catch ex As Exception
        Return AppDataAll
      End Try
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
      'Local's OK
    ElseIf Val(LocalVer(0)) = Val(RemoteVer(0)) Then
      If Val(LocalVer(1)) > Val(RemoteVer(1)) Then
        'Local's OK
      ElseIf Val(LocalVer(1)) = Val(RemoteVer(1)) Then
        If Val(LocalVer(2)) > Val(RemoteVer(2)) Then
          'Local's OK
        ElseIf Val(LocalVer(2)) = Val(RemoteVer(2)) Then
          If Val(LocalVer(3)) >= Val(RemoteVer(3)) Then
            'Local's OK
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
      Dim sFailFile As String = "WB-ReadFail-" & Now.ToString("G") & "-v" & Application.ProductVersion & ".txt"
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
    Catch ex As Exception
      'failed, whatever...
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
            'no files in SRT folder to get in the way
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
          'no files in WB folder to copy
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

  Public Function DrawLineGraph(ByVal Data() As DataBase.DataRow, ByVal Down As Boolean, ByVal ImgSize As Size, ColorA As Color, ColorB As Color, ColorC As Color, ColorText As Color, ColorBG As Color, ColorMax As Color) As Image
    If Data Is Nothing OrElse Data.Length = 0 Then Return New Bitmap(1, 1)
    Dim yDMax As Long = 0
    Dim yUMax As Long = 0
    For I As Long = 0 To Data.Length - 1
      If yDMax < Data(I).DOWNLOAD Then yDMax = Data(I).DOWNLOAD
      If yUMax < Data(I).UPLOAD Then yUMax = Data(I).UPLOAD
      If yDMax < Data(I).DOWNLIM Then yDMax = Data(I).DOWNLIM
      If yUMax < Data(I).UPLIM Then yUMax = Data(I).UPLIM
    Next
    Dim yMax As Long = IIf(yDMax > yUMax, yDMax, yUMax)
    If Not yMax Mod 1000 = 0 Then yMax = (yMax \ 1000) * 1000
    Dim lMax As Long = IIf(Down, yDMax, yUMax)
    If Not lMax Mod 1000 = 0 Then lMax = (lMax \ 1000) * 1000 + 1000
    Dim iPic As Image = New Bitmap(ImgSize.Width, ImgSize.Height)
    Dim g As Graphics = Graphics.FromImage(iPic)
    Dim tFont As New Font(FontFamily.GenericSansSerif, 7)
    Dim lYWidth As Integer = g.MeasureString(yMax.ToString.Trim & " MB", tFont).Width + 10
    'Dim lXWidth As Integer = g.MeasureString(Now.ToString("g"), tFont).Width
    Dim lXHeight As Integer = g.MeasureString(Now.ToString("g"), tFont).Height + 10
    g.Clear(ColorBG)
    Dim yTop As Integer = lXHeight / 2
    Dim yHeight As Integer = ImgSize.Height - (lXHeight * 1.5)
    If Down Then
      dGraph = New Rectangle(lYWidth, yTop, (ImgSize.Width - 4) - lYWidth, yHeight)
      dData = Data
    Else
      uGraph = New Rectangle(lYWidth, yTop, (ImgSize.Width - 4) - lYWidth, yHeight)
      uData = Data
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
    Dim lStart As Date = Data(0).DATETIME
    Dim lEnd As Date = Data(Data.Length - 1).DATETIME
    Dim dInterval As DateInterval = DateInterval.Minute
    Dim lInterval As UInteger = 1
    Dim lLabelInterval As UInteger = 5
    Select Case Math.Abs(DateDiff(DateInterval.Minute, lStart, lEnd))
      Case Is <= 61
        'Under an Hour
        lInterval = 1
        lLabelInterval = 5
        dInterval = DateInterval.Minute
      Case Is < 60 * 13
        'Under 12 hours
        lInterval = 15
        lLabelInterval = 60
        dInterval = DateInterval.Minute
      Case Is <= 60 * 25
        'Under a Day
        lInterval = 60
        lLabelInterval = 60 * 6
        dInterval = DateInterval.Minute
      Case Is <= 60 * 24 * 8
        'Under a Week
        lInterval = 12
        lLabelInterval = 24
        dInterval = DateInterval.Hour
      Case Is <= 60 * 24 * 31
        'Under 30 Days
        lInterval = 24
        lLabelInterval = 24 * 7
        dInterval = DateInterval.Hour
      Case Is <= 60 * 24 * 366
        'Under a Year
        lInterval = 7
        lLabelInterval = 30
        dInterval = DateInterval.Day
      Case Else
        'Over a year
        lInterval = 30
        lLabelInterval = 365
        dInterval = DateInterval.Day
    End Select
    Dim lMaxTime As Long = Math.Abs(DateDiff(dInterval, lStart, lEnd))
    If lMaxTime = 0 Then Return New Bitmap(1, 1)
    Dim lLineWidth As Long = (ImgSize.Width - 4) - lYWidth - 1
    Dim dCompInter As Double = lLineWidth / lMaxTime

    For I As Long = 0 To lMaxTime Step lInterval
      Dim lX As Integer = lYWidth + (I * dCompInter) + 1
      g.DrawLine(SystemPens.GrayText, lX, ImgSize.Height - (lXHeight - 3), lX, ImgSize.Height - lXHeight)
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
      If lX >= (ImgSize.Width - 4) Then
        lX = (ImgSize.Width - 4)
        g.DrawLine(New Pen(ColorText), lX, ImgSize.Height - (lXHeight - 5), lX, ImgSize.Height - lXHeight)
        Dim sDisp As String = DateAdd(dInterval, I, lStart).ToString(sDispV)
        g.DrawString(sDisp, tFont, New SolidBrush(ColorText), lX - g.MeasureString(sDisp, tFont).Width, ImgSize.Height - lXHeight + 5)
        If lX >= lastI - (iLastDispWidth * 1.6) Then lastI = -1
      Else
        g.DrawLine(New Pen(ColorText), lX, ImgSize.Height - (lXHeight - 5), lX, ImgSize.Height - lXHeight)
        Dim sDisp As String = DateAdd(dInterval, I, lStart).ToString(sDispV)
        g.DrawString(sDisp, tFont, New SolidBrush(ColorText), lX - (g.MeasureString(sDisp, tFont).Width / 2), ImgSize.Height - lXHeight + 5)
        If lX >= lastI - (iLastDispWidth * 1.6) Then lastI = -1
      End If
    Next I
    If lastI > -1 Then
      g.DrawLine(New Pen(ColorText), lastI, ImgSize.Height - (lXHeight - 5), lastI, ImgSize.Height - lXHeight)
      g.DrawString(sLastDisp, tFont, New SolidBrush(ColorText), lastI - iLastDispWidth + 3, ImgSize.Height - lXHeight + 5)
    End If

    Dim MaxY As Integer = yTop + yHeight - (IIf(Down, Data(Data.Length - 1).DOWNLIM, Data(Data.Length - 1).UPLIM) / lMax * yHeight)
    'Dim WarnY As Integer = yTop + yHeight - (IIf(Down, Data(Data.Length - 1).DOWNLIM, Data(Data.Length - 1).UPLIM) * 0.7 / lMax * yHeight)
    Dim lMaxPoints(lMaxTime) As Point
    Dim lPoints(lMaxTime + 3) As Point
    Dim lTypes(lMaxTime + 3) As Byte
    Dim lastLVal As Long = 0

    For I As Long = 0 To lMaxTime
      Dim lVal As Long = -1
      Dim lLow As Long = Long.MaxValue
      Dim lHigh As Long = 0
      For J As Integer = 0 To Data.Length - 1
        'If Data(J).DATETIME.Date = DateAdd(dInterval, I, lStart).Date Then
        If Math.Abs(DateDiff(dInterval, Data(J).DATETIME, DateAdd(dInterval, I, lStart))) < 2 Then
          Dim jLim As Long = IIf(Down, Data(J).DOWNLIM, Data(J).UPLIM)
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
        'If Data(J).DATETIME.Date = DateAdd(dInterval, I, lStart).Date Then If lVal < IIf(Down, Data(J).DOWNLOAD, Data(J).UPLOAD) Then lVal = IIf(Down, Data(J).DOWNLOAD, Data(J).UPLOAD)
        If Math.Abs(DateDiff(dInterval, Data(J).DATETIME, DateAdd(dInterval, I, lStart))) < 2 Then
          Dim jVal As Long = IIf(Down, Data(J).DOWNLOAD, Data(J).UPLOAD)
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
      lPoints(I).X = lYWidth + (I * dCompInter) + 1
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
    Dim fBrush As Drawing2D.LinearGradientBrush = TriGradientBrush(New Point(lYWidth, MaxY), New Point(lYWidth, yTop + yHeight), ColorA, ColorB, ColorC)
    fBrush.WrapMode = Drawing2D.WrapMode.TileFlipX
    g.DrawLines(New Pen(New SolidBrush(ColorMax), 5), lMaxPoints)
    g.FillPath(fBrush, New Drawing2D.GraphicsPath(lPoints, lTypes))
    g.FillRectangle(New Drawing2D.HatchBrush(Drawing2D.HatchStyle.DottedGrid, Color.FromArgb(192, ColorBG), Color.Transparent), lYWidth, yTop - 1, ImgSize.Width, MaxY - yTop + 1)
    'g.FillRectangle(New Drawing2D.HatchBrush(Drawing2D.HatchStyle.NarrowHorizontal, Color.FromArgb(128, Color.Yellow), Color.Transparent), lYWidth, MaxY + 1, ImgSize.Width, CInt(IIf(Down, Data(Data.Length - 1).DOWNLIM, Data(Data.Length - 1).UPLIM) * 0.3 / lMax * yHeight))
    g.DrawLines(New Pen(New SolidBrush(Color.FromArgb(96, ColorMax)), 5), lMaxPoints)
    g.Dispose()
    Return iPic
  End Function
  Public Function DrawEGraph(ByVal Data() As DataBase.DataRow, ByVal Invert As Boolean, ByVal ImgSize As Size, ColorDA As Color, ColorDB As Color, ColorDC As Color, ColorUA As Color, ColorUB As Color, ColorUC As Color, ColorText As Color, ColorBG As Color, ColorMax As Color) As Image
    If Data Is Nothing OrElse Data.Length = 0 Then Return New Bitmap(1, 1)
    Dim yVMax As Long = 0
    For I As Long = 0 To Data.Length - 1
      If Invert Then
        If yVMax < Data(I).DOWNLOAD + Data(I).UPLOAD + Data(I).DOWNLIM Then yVMax = Data(I).DOWNLOAD + Data(I).UPLOAD + Data(I).DOWNLIM
      Else
        If yVMax < Data(I).DOWNLOAD + Data(I).UPLOAD + Data(I).UPLIM Then yVMax = Data(I).DOWNLOAD + Data(I).UPLOAD + Data(I).UPLIM
      End If
    Next
    If Invert Then
      If yVMax < Data(Data.Length - 1).UPLIM Then yVMax = Data(Data.Length - 1).UPLIM
    Else
      If yVMax < Data(Data.Length - 1).DOWNLIM Then yVMax = Data(Data.Length - 1).DOWNLIM
    End If
    Dim yMax As Long = yVMax
    If Not yMax Mod 1000 = 0 Then yMax = (yMax \ 1000) * 1000
    Dim lMax As Long = yVMax
    If Not lMax Mod 1000 = 0 Then lMax = (lMax \ 1000) * 1000 + 1000
    Dim iPic As Image = New Bitmap(ImgSize.Width, ImgSize.Height)
    Dim g As Graphics = Graphics.FromImage(iPic)
    Dim tFont As New Font(FontFamily.GenericSansSerif, 7)
    Dim lYWidth As Integer = g.MeasureString(yMax.ToString.Trim & " MB", tFont).Width + 10
    'Dim lXWidth As Integer = g.MeasureString(Now.ToString("g"), tFont).Width
    Dim lXHeight As Integer = g.MeasureString(Now.ToString("g"), tFont).Height + 10
    g.Clear(ColorBG)
    Dim yTop As Integer = lXHeight / 2
    Dim yHeight As Integer = ImgSize.Height - (lXHeight * 1.5)
    dGraph = New Rectangle(lYWidth, yTop, ImgSize.Width - lYWidth, yHeight)
    dData = Data
    uGraph = Nothing
    uData = Nothing
    g.DrawLine(New Pen(ColorText), lYWidth, yTop, lYWidth, yTop + yHeight)
    g.DrawLine(New Pen(ColorText), lYWidth, yTop + yHeight, ImgSize.Width, yTop + yHeight)
    oldDate = Data.First.DATETIME
    newDate = Data.Last.DATETIME
    For I As Integer = 0 To lMax Step (((lMax \ (yHeight \ (tFont.Size + 12)))) \ 100) * 100
      Dim iY As Integer = yTop + yHeight - (I / lMax * yHeight)
      g.DrawString(I.ToString.Trim & " MB", tFont, New SolidBrush(ColorText), lYWidth - g.MeasureString(I.ToString.Trim & " MB", tFont).Width, iY - (g.MeasureString(I.ToString.Trim & " MB", tFont).Height / 2))
      g.DrawLine(New Pen(ColorText), lYWidth - 3, iY, lYWidth, iY)
    Next I
    Dim lStart As Date = Data(0).DATETIME
    Dim lEnd As Date = Data(Data.Length - 1).DATETIME
    Dim dInterval As DateInterval = DateInterval.Minute
    Dim lInterval As UInteger = 1
    Dim lLabelInterval As UInteger = 5
    Select Case Math.Abs(DateDiff(DateInterval.Minute, lStart, lEnd))
      Case Is <= 61
        'Under an Hour
        lInterval = 1
        lLabelInterval = 5
        dInterval = DateInterval.Minute
      Case Is < 60 * 13
        'Under 12 hours
        lInterval = 15
        lLabelInterval = 60
        dInterval = DateInterval.Minute
      Case Is <= 60 * 25
        'Under a Day
        lInterval = 60
        lLabelInterval = 60 * 6
        dInterval = DateInterval.Minute
      Case Is <= 60 * 24 * 8
        'Under a Week
        lInterval = 12
        lLabelInterval = 24
        dInterval = DateInterval.Hour
      Case Is <= 60 * 24 * 31
        'Under 30 Days
        lInterval = 24
        lLabelInterval = 24 * 7
        dInterval = DateInterval.Hour
      Case Is <= 60 * 24 * 366
        'Under a Year
        lInterval = 7
        lLabelInterval = 30
        dInterval = DateInterval.Day
      Case Else
        'Over a year
        lInterval = 30
        lLabelInterval = 365
        dInterval = DateInterval.Day
    End Select
    Dim lMaxTime As Long = Math.Abs(DateDiff(dInterval, lStart, lEnd))
    If lMaxTime = 0 Then Return New Bitmap(1, 1)
    Dim lLineWidth As Long = ImgSize.Width - lYWidth
    Dim dCompInter As Double = lLineWidth / lMaxTime
    For I As Long = 0 To lMaxTime Step lInterval
      Dim lX As Integer = lYWidth + (I * dCompInter)
      g.DrawLine(New Pen(ColorText), lX, ImgSize.Height - (lXHeight - 3), lX, ImgSize.Height - lXHeight)
    Next I
    For I As Long = 0 To lMaxTime Step lLabelInterval
      Dim lX As Integer = lYWidth + (I * dCompInter)
      g.DrawLine(New Pen(ColorText), lX, ImgSize.Height - (lXHeight - 5), lX, ImgSize.Height - lXHeight)
      Dim sDisp As String
      Select Case DateDiff(DateInterval.Day, lStart, lEnd)
        Case Is > 1
          sDisp = DateAdd(dInterval, I, lStart).ToString("d")
        Case Is < 1
          sDisp = DateAdd(dInterval, I, lStart).ToString("t")
        Case Else
          sDisp = DateAdd(dInterval, I, lStart).ToString("g")
      End Select
      g.DrawString(sDisp, tFont, New SolidBrush(ColorText), lX - (g.MeasureString(sDisp, tFont).Width / 2), ImgSize.Height - lXHeight + 5)
    Next I
    Dim MaxY As Integer
    If Invert Then
      MaxY = yTop + yHeight - (Data(Data.Length - 1).UPLIM / lMax * yHeight)
    Else
      MaxY = yTop + yHeight - (Data(Data.Length - 1).DOWNLIM / lMax * yHeight)
    End If
    'Dim WarnY As Integer = yTop + yHeight - (Data(Data.Length - 1).DOWNLIM * 0.7 / lMax * yHeight)

    Dim lMaxPoints(lMaxTime) As Point
    Dim lastLVal As Long = 0

    For I As Long = 0 To lMaxTime
      Dim lVal As Long = -1
      Dim lLow As Long = Long.MaxValue
      Dim lHigh As Long = 0
      For J As Integer = 0 To Data.Length - 1
        If Math.Abs(DateDiff(dInterval, Data(J).DATETIME, DateAdd(dInterval, I, lStart))) < 2 Then
          If Invert Then
            If lHigh < Data(J).UPLIM Then
              lHigh = Data(J).UPLIM
            End If
            If lLow > Data(J).UPLIM Then
              lLow = Data(J).UPLIM
            End If
          Else
            If lHigh < Data(J).DOWNLIM Then
              lHigh = Data(J).DOWNLIM
            End If
            If lLow > Data(J).DOWNLIM Then
              lLow = Data(J).DOWNLIM
            End If
          End If
          'If lVal < Data(J).DOWNLIM Then lVal = Data(J).DOWNLIM
        End If
      Next

      If lHigh > 0 And lLow < Long.MaxValue Then lVal = (lHigh + lLow) / 2
      If lVal = -1 And lastLVal > 0 Then lVal = lastLVal
      lMaxPoints(I).X = lYWidth + (I * dCompInter)
      lMaxPoints(I).Y = yTop + yHeight - (lVal / lMax * yHeight)
      lastLVal = lVal
    Next I

    Dim lUPoints(lMaxTime + 3) As Drawing.Point
    Dim lDPoints(lMaxTime + 3) As Drawing.Point
    Dim lOPoints(lMaxTime + 3) As Drawing.Point
    Dim lTypes(lMaxTime + 3) As Byte
    Dim lastDVal As Long = 0
    Dim lastUVal As Long = 0
    Dim lastOVal As Long = 0
    For I As Long = 0 To lMaxTime
      Dim lDVal As Long = 0
      Dim lUVal As Long = 0
      Dim lOVal As Long = 0
      Dim lDLow As Long = Long.MaxValue
      Dim lDHigh As Long = 0
      Dim lULow As Long = Long.MaxValue
      Dim lUHigh As Long = 0
      Dim lOLow As Long = Long.MaxValue
      Dim lOHigh As Long = 0

      For J As Integer = 0 To Data.Length - 1
        'If Data(J).DATETIME.Date = DateAdd(dInterval, I, lStart).Date Then
        If Math.Abs(DateDiff(dInterval, Data(J).DATETIME, DateAdd(dInterval, I, lStart))) < 2 Then

          Dim jDVal As Long = Data(J).DOWNLOAD
          If lDHigh < jDVal Then lDHigh = jDVal
          If lDLow > jDVal Then lDLow = jDVal

          Dim jUVal As Long = Data(J).UPLOAD
          If lUHigh < jUVal Then lUHigh = jUVal
          If lULow > jUVal Then lULow = jUVal

          Dim jOVal As Long
          If Data(J).DOWNLIM = Data(J).UPLIM Then
            jOVal = 0
          Else
            If Invert Then
              jOVal = Data(J).DOWNLIM
            Else
              jOVal = Data(J).UPLIM
            End If
          End If
          If lOHigh < jOVal Then lOHigh = jOVal
          If lOLow > jOVal Then lOLow = jOVal

          'If lDVal < Data(J).DOWNLOAD Then lDVal = Data(J).DOWNLOAD
          'If lUVal < Data(J).UPLOAD Then lUVal = Data(J).UPLOAD
        End If
      Next
      If lDHigh > 0 And lDLow < Long.MaxValue Then lDVal = (lDHigh + lDLow) / 2
      If lDVal = -1 And lastDVal > 0 Then lDVal = lastDVal
      If lUHigh > 0 And lULow < Long.MaxValue Then lUVal = (lUHigh + lULow) / 2
      If lUVal = -1 And lastUVal > 0 Then lUVal = lastUVal
      If lOHigh > 0 And lOLow < Long.MaxValue Then lOVal = (lOHigh + lOLow) / 2
      If lOVal = -1 And lastOVal > 0 Then lOVal = lastOVal

      lUPoints(I).X = lYWidth + (I * dCompInter)
      lUPoints(I).Y = yTop + yHeight - (lUVal / lMax * yHeight)
      lDPoints(I).X = lYWidth + (I * dCompInter) '           '''\/'''
      lDPoints(I).Y = yTop + yHeight - (lDVal / lMax * yHeight) - (yTop + yHeight - lUPoints(I).Y) ''''
      lOPoints(I).X = lYWidth + (I * dCompInter)
      lOPoints(I).Y = yTop + yHeight - (lOVal / lMax * yHeight) - (yTop + yHeight - lDPoints(I).Y)
      lastDVal = lDVal
      lastUVal = lUVal
      lastOVal = lOVal
    Next I
    lTypes(0) = Drawing2D.PathPointType.Start
    For I As Long = 1 To lMaxTime + 2
      lTypes(I) = Drawing2D.PathPointType.Line
    Next

    If lUPoints(lMaxTime).IsEmpty Then lUPoints(lMaxTime) = New Drawing.Point(ImgSize.Width, yTop + yHeight)
    lUPoints(lMaxTime + 1) = New Drawing.Point(ImgSize.Width, yTop + yHeight)
    lUPoints(lMaxTime + 2) = New Drawing.Point(lYWidth, yTop + yHeight)
    lUPoints(lMaxTime + 3) = lUPoints(0)
    lTypes(lMaxTime + 3) = Drawing2D.PathPointType.Line Or Drawing2D.PathPointType.CloseSubpath
    Dim uBrush As Drawing2D.LinearGradientBrush = TriGradientBrush(New Point(lYWidth, MaxY), New Point(lYWidth, yTop + yHeight), ColorUA, ColorUB, ColorUC)
    uBrush.WrapMode = Drawing2D.WrapMode.TileFlipX

    If lDPoints(lMaxTime).IsEmpty Then lDPoints(lMaxTime) = New Drawing.Point(ImgSize.Width, yTop + yHeight)
    lDPoints(lMaxTime + 1) = New Drawing.Point(ImgSize.Width, yTop + yHeight)
    lDPoints(lMaxTime + 2) = New Drawing.Point(lYWidth, yTop + yHeight)
    lDPoints(lMaxTime + 3) = lDPoints(0)
    Dim dBrush As Drawing2D.LinearGradientBrush = TriGradientBrush(New Point(lYWidth, MaxY), New Point(lYWidth, yTop + yHeight), ColorDA, ColorDB, ColorDC)
    dBrush.WrapMode = Drawing2D.WrapMode.TileFlipX

    If lOPoints(lMaxTime).IsEmpty Then lOPoints(lMaxTime) = New Drawing.Point(ImgSize.Width, yTop + yHeight)
    lOPoints(lMaxTime + 1) = New Drawing.Point(ImgSize.Width, yTop + yHeight)
    lOPoints(lMaxTime + 2) = New Drawing.Point(lYWidth, yTop + yHeight)
    lOPoints(lMaxTime + 3) = lOPoints(0)
    Dim oBrush As Drawing2D.LinearGradientBrush = TriGradientBrush(New Point(lYWidth, MaxY), New Point(lYWidth, yTop + yHeight), ColorDA, ColorDB, ColorDC)
    oBrush.WrapMode = Drawing2D.WrapMode.TileFlipX

    g.DrawLines(New Pen(New SolidBrush(ColorMax), 5), lMaxPoints)
    g.FillPath(oBrush, New Drawing2D.GraphicsPath(lOPoints, lTypes))
    g.FillPath(dBrush, New Drawing2D.GraphicsPath(lDPoints, lTypes))
    g.FillPath(uBrush, New Drawing2D.GraphicsPath(lUPoints, lTypes))
    g.FillRectangle(New Drawing2D.HatchBrush(Drawing2D.HatchStyle.DottedGrid, Color.FromArgb(192, ColorBG), Color.Transparent), lYWidth, yTop - 1, ImgSize.Width, MaxY - yTop + 1)
    'g.FillRectangle(New Drawing2D.HatchBrush(Drawing2D.HatchStyle.NarrowHorizontal, Color.FromArgb(128, Color.Yellow), Color.Transparent), lYWidth, MaxY + 1, ImgSize.Width, CInt(Data(Data.Length - 1).DOWNLIM * 0.3 / lMax * yHeight))
    g.DrawLines(New Pen(New SolidBrush(Color.FromArgb(96, ColorMax)), 5), lMaxPoints)
    g.Dispose()
    Return iPic
  End Function
  Public Function DrawRGraph(ByVal Data() As DataBase.DataRow, ByVal ImgSize As Size, ColorA As Color, ColorB As Color, ColorC As Color, ColorText As Color, ColorBG As Color, ColorMax As Color) As Image
    If Data Is Nothing OrElse Data.Length = 0 Then Return New Bitmap(1, 1)
    Dim yVMax As Long = 0
    For I As Long = 0 To Data.Length - 1
      If yVMax < Data(I).DOWNLOAD Then yVMax = Data(I).DOWNLOAD
    Next
    If yVMax < Data(Data.Length - 1).DOWNLIM Then yVMax = Data(Data.Length - 1).DOWNLIM
    Dim yMax As Long = yVMax
    If Not yMax Mod 1000 = 0 Then yMax = (yMax \ 1000) * 1000
    Dim lMax As Long = yVMax
    If Not lMax Mod 1000 = 0 Then lMax = (lMax \ 1000) * 1000 + 1000
    Dim iPic As Image = New Bitmap(ImgSize.Width, ImgSize.Height)
    Dim g As Graphics = Graphics.FromImage(iPic)
    Dim tFont As New Font(FontFamily.GenericSansSerif, 7)
    Dim lYWidth As Integer = g.MeasureString(yMax.ToString.Trim & " MB", tFont).Width + 10
    'Dim lXWidth As Integer = g.MeasureString(Now.ToString("g"), tFont).Width
    Dim lXHeight As Integer = g.MeasureString(Now.ToString("g"), tFont).Height + 10
    g.Clear(ColorBG)
    Dim yTop As Integer = lXHeight / 2
    Dim yHeight As Integer = ImgSize.Height - (lXHeight * 1.5)
    dGraph = New Rectangle(lYWidth, yTop, ImgSize.Width - lYWidth, yHeight)
    dData = Data
    uGraph = Nothing
    uData = Nothing
    g.DrawLine(New Pen(ColorText), lYWidth, yTop, lYWidth, yTop + yHeight)
    g.DrawLine(New Pen(ColorText), lYWidth, yTop + yHeight, ImgSize.Width, yTop + yHeight)
    oldDate = Data.First.DATETIME
    newDate = Data.Last.DATETIME
    For I As Integer = 0 To lMax Step (((lMax \ (yHeight \ (tFont.Size + 12)))) \ 100) * 100
      Dim iY As Integer = yTop + yHeight - (I / lMax * yHeight)
      g.DrawString(I.ToString.Trim & " MB", tFont, New SolidBrush(ColorText), lYWidth - g.MeasureString(I.ToString.Trim & " MB", tFont).Width, iY - (g.MeasureString(I.ToString.Trim & " MB", tFont).Height / 2))
      g.DrawLine(New Pen(ColorText), lYWidth - 3, iY, lYWidth, iY)
    Next I
    Dim lStart As Date = Data(0).DATETIME
    Dim lEnd As Date = Data(Data.Length - 1).DATETIME
    Dim dInterval As DateInterval = DateInterval.Minute
    Dim lInterval As UInteger = 1
    Dim lLabelInterval As UInteger = 5
    Select Case Math.Abs(DateDiff(DateInterval.Minute, lStart, lEnd))
      Case Is <= 61
        'Under an Hour
        lInterval = 1
        lLabelInterval = 5
        dInterval = DateInterval.Minute
      Case Is < 60 * 13
        'Under 12 hours
        lInterval = 15
        lLabelInterval = 60
        dInterval = DateInterval.Minute
      Case Is <= 60 * 25
        'Under a Day
        lInterval = 60
        lLabelInterval = 60 * 6
        dInterval = DateInterval.Minute
      Case Is <= 60 * 24 * 8
        'Under a Week
        lInterval = 12
        lLabelInterval = 24
        dInterval = DateInterval.Hour
      Case Is <= 60 * 24 * 31
        'Under 30 Days
        lInterval = 24
        lLabelInterval = 24 * 7
        dInterval = DateInterval.Hour
      Case Is <= 60 * 24 * 366
        'Under a Year
        lInterval = 7
        lLabelInterval = 30
        dInterval = DateInterval.Day
      Case Else
        'Over a year
        lInterval = 30
        lLabelInterval = 365
        dInterval = DateInterval.Day
    End Select
    Dim lMaxTime As Long = Math.Abs(DateDiff(dInterval, lStart, lEnd))
    If lMaxTime = 0 Then Return New Bitmap(1, 1)
    Dim lLineWidth As Long = ImgSize.Width - lYWidth
    Dim dCompInter As Double = lLineWidth / lMaxTime
    For I As Long = 0 To lMaxTime Step lInterval
      Dim lX As Integer = lYWidth + (I * dCompInter)
      g.DrawLine(New Pen(ColorText), lX, ImgSize.Height - (lXHeight - 3), lX, ImgSize.Height - lXHeight)
    Next I
    For I As Long = 0 To lMaxTime Step lLabelInterval
      Dim lX As Integer = lYWidth + (I * dCompInter)
      g.DrawLine(New Pen(ColorText), lX, ImgSize.Height - (lXHeight - 5), lX, ImgSize.Height - lXHeight)
      Dim sDisp As String
      Select Case DateDiff(DateInterval.Day, lStart, lEnd)
        Case Is > 1
          sDisp = DateAdd(dInterval, I, lStart).ToString("d")
        Case Is < 1
          sDisp = DateAdd(dInterval, I, lStart).ToString("t")
        Case Else
          sDisp = DateAdd(dInterval, I, lStart).ToString("g")
      End Select
      g.DrawString(sDisp, tFont, New SolidBrush(ColorText), lX - (g.MeasureString(sDisp, tFont).Width / 2), ImgSize.Height - lXHeight + 5)
    Next I
    Dim MaxY As Integer = yTop + yHeight - (Data(Data.Length - 1).DOWNLIM / lMax * yHeight)
    'Dim WarnY As Integer = yTop + yHeight - (Data(Data.Length - 1).DOWNLIM * 0.7 / lMax * yHeight)
    Dim lMaxPoints(lMaxTime) As Point
    Dim lastLVal As Long = 0
    For I As Long = 0 To lMaxTime
      Dim lVal As Long = 0
      Dim lLow As Long = Long.MaxValue
      Dim lHigh As Long = 0
      For J As Integer = 0 To Data.Length - 1
        If Math.Abs(DateDiff(dInterval, Data(J).DATETIME, DateAdd(dInterval, I, lStart))) < 2 Then
          If lHigh < Data(J).DOWNLIM Then
            lHigh = Data(J).DOWNLIM
          End If
          If lLow > Data(J).DOWNLIM Then
            lLow = Data(J).DOWNLIM
          End If
        End If
        'If Data(J).DATETIME.Date = DateAdd(dInterval, I, lStart).Date Then
        '  If lVal < Data(J).DOWNLIM Then lVal = Data(J).DOWNLIM
        'End If
      Next
      If lHigh > 0 And lLow < Long.MaxValue Then lVal = (lHigh + lLow) / 2
      If lVal = -1 And lastLVal > 0 Then lVal = lastLVal
      lMaxPoints(I).X = lYWidth + (I * dCompInter)
      lMaxPoints(I).Y = yTop + yHeight - (lVal / lMax * yHeight)
      lastLVal = lVal
    Next I

    lastLVal = 0
    Dim lDPoints(lMaxTime + 3) As Point
    Dim lTypes(lMaxTime + 3) As Byte
    For I As Long = 0 To lMaxTime
      Dim lDVal As Long = 0
      Dim lLow As Long = Long.MaxValue
      Dim lHigh As Long = 0
      For J As Integer = 0 To Data.Length - 1
        If Math.Abs(DateDiff(dInterval, Data(J).DATETIME, DateAdd(dInterval, I, lStart))) < 2 Then
          If lHigh < Data(J).DOWNLOAD Then
            lHigh = Data(J).DOWNLOAD
          End If
          If lLow > Data(J).DOWNLOAD Then
            lLow = Data(J).DOWNLOAD
          End If
        End If
        'If Data(J).DATETIME.Date = DateAdd(dInterval, I, lStart).Date Then
        '  If lDVal < Data(J).DOWNLOAD Then lDVal = Data(J).DOWNLOAD
        'End If
      Next
      If lHigh > 0 And lLow < Long.MaxValue Then lDVal = (lHigh + lLow) / 2
      If lDVal = -1 And lastLVal > 0 Then lDVal = lastLVal
      lDPoints(I).X = lYWidth + (I * dCompInter)
      lDPoints(I).Y = yTop + yHeight - (lDVal / lMax * yHeight)
      lastLVal = lDVal
    Next I
    If lDPoints(lMaxTime).IsEmpty Then lDPoints(lMaxTime) = New Drawing.Point(ImgSize.Width, yTop + yHeight)
    lDPoints(lMaxTime + 1) = New Point(ImgSize.Width, yTop + yHeight)
    lDPoints(lMaxTime + 2) = New Point(lYWidth, yTop + yHeight)
    lDPoints(lMaxTime + 3) = lDPoints(0)
    lTypes(0) = Drawing2D.PathPointType.Start
    For I As Long = 1 To lMaxTime + 2
      lTypes(I) = Drawing2D.PathPointType.Line
    Next
    lTypes(lMaxTime + 3) = Drawing2D.PathPointType.Line Or Drawing2D.PathPointType.CloseSubpath

    Dim fBrush As Drawing2D.LinearGradientBrush = TriGradientBrush(New Point(lYWidth, MaxY), New Point(lYWidth, yTop + yHeight), ColorA, ColorB, ColorC)
    fBrush.WrapMode = Drawing2D.WrapMode.TileFlipX
    g.DrawLines(New Pen(New SolidBrush(ColorMax), 5), lMaxPoints)
    g.FillPath(fBrush, New Drawing2D.GraphicsPath(lDPoints, lTypes))
    g.FillRectangle(New Drawing2D.HatchBrush(Drawing2D.HatchStyle.DottedGrid, Color.FromArgb(192, ColorBG), Color.Transparent), lYWidth, yTop - 1, ImgSize.Width, MaxY - yTop + 1)
    'g.FillRectangle(New Drawing2D.HatchBrush(Drawing2D.HatchStyle.NarrowHorizontal, Color.FromArgb(128, Color.Yellow), Color.Transparent), lYWidth, MaxY + 1, ImgSize.Width, CInt(Data(Data.Length - 1).DOWNLIM * 0.3 / lMax * yHeight))
    g.DrawLines(New Pen(New SolidBrush(Color.FromArgb(96, ColorMax)), 5), lMaxPoints)
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
        g.Clear(ColorBG)
        If Total > 0 And Down > 0 Then
          Dim downWidth As Long = ImgSize.Width - (((Total - Down) / Total) * ImgSize.Width)
          g.FillRectangle(downBrush, 0, 0, downWidth, ImgSize.Height)
          'ImgSize.ForeColor = ColorBG
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
        g.Clear(ColorBG)
        If Total > 0 And Down > 0 Then
          g.FillRectangle(downBrush, 0, 0, ImgSize.Width, ImgSize.Height)
          'ImgSize.ForeColor = ColorBG
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
    If ColorB.A = 0 Then
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
  Public Sub CreateTrayIcon_Left(ByRef g As Graphics, lUsed As Long, lLim As Long, cA As Color, cB As Color, cC As Color)
    If lLim = 0 Then Exit Sub
    Dim fillBrush As Drawing2D.LinearGradientBrush
    If cB.A = 0 Then
      fillBrush = New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(0, 16), Color.FromArgb(Alpha, cC), Color.FromArgb(Alpha, cA))
    Else
      fillBrush = New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(0, 16), Color.Black, Color.Black)
      Dim cBlend As New Drawing2D.ColorBlend
      cBlend.Positions = {0, 0.5, 1}
      cBlend.Colors = {Color.FromArgb(Alpha, cC), Color.FromArgb(Alpha, cB), Color.FromArgb(Alpha, cA)}
      fillBrush.InterpolationColors = cBlend
    End If
    Dim yUsed As Integer = 16 - (lUsed / lLim * 16)
    If yUsed < 0 Then yUsed = 0
    If yUsed > 16 Then yUsed = 16
    g.FillRectangle(fillBrush, 0, yUsed, 8, 16 - yUsed)
  End Sub
  Public Sub CreateTrayIcon_Right(ByRef g As Graphics, lUsed As Long, lLim As Long, cA As Color, cB As Color, cC As Color)
    If lLim = 0 Then Exit Sub
    Dim fillBrush As Drawing2D.LinearGradientBrush
    If cB.A = 0 Then
      fillBrush = New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(0, 16), Color.FromArgb(Alpha, cC), Color.FromArgb(Alpha, cA))
    Else
      fillBrush = New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(0, 16), Color.Black, Color.Black)
      Dim cBlend As New Drawing2D.ColorBlend
      cBlend.Positions = {0, 0.5, 1}
      cBlend.Colors = {Color.FromArgb(Alpha, cC), Color.FromArgb(Alpha, cB), Color.FromArgb(Alpha, cA)}
      fillBrush.InterpolationColors = cBlend
    End If
    Dim yUsed As Integer = 16 - (lUsed / lLim * 16)
    If yUsed < 0 Then yUsed = 0
    If yUsed > 16 Then yUsed = 16
    g.FillRectangle(fillBrush, 8, yUsed, 8, 16 - yUsed)
  End Sub
  Public Sub CreateTrayIcon_Dual(ByRef g As Graphics, lDown As Long, lUp As Long, lLim As Long, cDA As Color, cDB As Color, cDC As Color, cUA As Color, cUB As Color, cUC As Color)
    If lLim = 0 Then Exit Sub
    Dim upBrush As Drawing2D.LinearGradientBrush
    If cUB.A = 0 Then
      upBrush = New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(0, 16), Color.FromArgb(Alpha, cUC), Color.FromArgb(Alpha, cUA))
    Else
      upBrush = New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(0, 16), Color.Black, Color.Black)
      Dim cBlend As New Drawing2D.ColorBlend
      cBlend.Positions = {0, 0.5, 1}
      cBlend.Colors = {Color.FromArgb(Alpha, cUC), Color.FromArgb(Alpha, cUB), Color.FromArgb(Alpha, cUA)}
      upBrush.InterpolationColors = cBlend
    End If
    Dim downBrush As Drawing2D.LinearGradientBrush
    If cDB.A = 0 Then
      downBrush = New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(0, 16), Color.FromArgb(Alpha, cDC), Color.FromArgb(Alpha, cDA))
    Else
      downBrush = New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(0, 16), Color.Black, Color.Black)
      Dim cBlend As New Drawing2D.ColorBlend
      cBlend.Positions = {0, 0.5, 1}
      cBlend.Colors = {Color.FromArgb(Alpha, cDC), Color.FromArgb(Alpha, cDB), Color.FromArgb(Alpha, cDA)}
      downBrush.InterpolationColors = cBlend
    End If
    If lDown + lUp > lLim Then
      'Maxed
      Dim fillLim As Long = lDown + lUp
      Dim yUp As Integer = 16 - (lUp / fillLim * 16)
      Dim yDown As Integer = yUp - (lDown / fillLim * 16)
      g.FillRectangle(downBrush, 0, yDown, 16, 16 - ((16 - yUp) - 1) - yDown)
      g.FillRectangle(upBrush, 0, yUp, 16, 16 - yUp)
    Else
      Dim yUp As Integer = 16 - (lUp / lLim * 16)
      Dim yDown As Integer = yUp - (lDown / lLim * 16)
      g.FillRectangle(downBrush, 0, yDown, 16, 16 - ((16 - yUp) - 1) - yDown)
      g.FillRectangle(upBrush, 0, yUp, 16, 16 - yUp)
    End If
  End Sub
#End Region
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
      Application.DoEvents()
    Loop While TickCount() - iStart < 5000
    Return False
  End Function
  Public Function TickCount() As Long
    Return (Stopwatch.GetTimestamp / Stopwatch.Frequency) * 1000
  End Function
End Module