﻿Imports System.Runtime.InteropServices
Imports System.Text
Friend Class ShellLink
  Implements IDisposable
#Region "ComInterop for IShellLink"
#Region "IPersistFile Interface"
  <ComImportAttribute()> _
  <GuidAttribute("0000010B-0000-0000-C000-000000000046")> _
  <InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)> _
  Private Interface IPersistFile
    <PreserveSig()> Sub GetClassID(pClassID As Guid)
    Sub IsDirty()
    Sub Load(<MarshalAs(UnmanagedType.LPWStr)> pszFileName As String, dwMode As UInteger)
    Sub Save(<MarshalAs(UnmanagedType.LPWStr)> pszFileName As String, <MarshalAs(UnmanagedType.Bool)> fRemember As Boolean)
    Sub SaveCompleted(<MarshalAs(UnmanagedType.LPWStr)> pszFileName As String)
    Sub GetCurFile(<MarshalAs(UnmanagedType.LPWStr)> ppszFileName As String)
  End Interface
#End Region
#Region "IShellLink Interface"
  <ComImportAttribute()> _
  <GuidAttribute("000214EE-0000-0000-C000-000000000046")> _
  <InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)> _
  Private Interface IShellLinkA
    Sub GetPath(<Out(), MarshalAs(UnmanagedType.LPStr)> pszFile As StringBuilder, cchMaxPath As Integer, ByRef pfd As _WIN32_FIND_DATAA, fFlags As UInteger)
    Sub GetIDList(ppidl As IntPtr)
    Sub SetIDList(pidl As IntPtr)
    Sub GetDescription(<Out(), MarshalAs(UnmanagedType.LPStr)> pszFile As StringBuilder, cchMaxName As Integer)
    Sub SetDescription(<MarshalAs(UnmanagedType.LPStr)> pszName As String)
    Sub GetWorkingDirectory(<Out(), MarshalAs(UnmanagedType.LPStr)> pszDir As StringBuilder, cchMaxPath As Integer)
    Sub SetWorkingDirectory(<MarshalAs(UnmanagedType.LPStr)> pszDir As String)
    Sub GetArguments(<Out(), MarshalAs(UnmanagedType.LPStr)> pszArgs As StringBuilder, cchMaxPath As Integer)
    Sub SetArguments(<MarshalAs(UnmanagedType.LPStr)> pszArgs As String)
    Sub GetHotkey(pwHotkey As Short)
    Sub SetHotkey(pwHotkey As Short)
    Sub GetShowCmd(piShowCmd As UInteger)
    Sub SetShowCmd(piShowCmd As UInteger)
    Sub GetIconLocation(<Out(), MarshalAs(UnmanagedType.LPStr)> pszIconPath As StringBuilder, cchIconPath As Integer, piIcon As Integer)
    Sub SetIconLocation(<MarshalAs(UnmanagedType.LPStr)> pszIconPath As String, iIcon As Integer)
    Sub SetRelativePath(<MarshalAs(UnmanagedType.LPStr)> pszPathRel As String, dwReserved As UInteger)
    Sub Resolve(hWnd As IntPtr, fFlags As UInteger)
    Sub SetPath(<MarshalAs(UnmanagedType.LPStr)> pszFile As String)
  End Interface
  <ComImportAttribute()> _
  <GuidAttribute("000214F9-0000-0000-C000-000000000046")> _
  <InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)> _
  Private Interface IShellLinkW
    Sub GetPath(<Out(), MarshalAs(UnmanagedType.LPWStr)> pszFile As StringBuilder, cchMaxPath As Integer, ByRef pfd As _WIN32_FIND_DATAW, fFlags As UInteger)
    Sub GetIDList(ppidl As IntPtr)
    Sub SetIDList(pidl As IntPtr)
    Sub GetDescription(<Out(), MarshalAs(UnmanagedType.LPWStr)> pszFile As StringBuilder, cchMaxName As Integer)
    Sub SetDescription(<MarshalAs(UnmanagedType.LPWStr)> pszName As String)
    Sub GetWorkingDirectory(<Out(), MarshalAs(UnmanagedType.LPWStr)> pszDir As StringBuilder, cchMaxPath As Integer)
    Sub SetWorkingDirectory(<MarshalAs(UnmanagedType.LPWStr)> pszDir As String)
    Sub GetArguments(<Out(), MarshalAs(UnmanagedType.LPWStr)> pszArgs As StringBuilder, cchMaxPath As Integer)
    Sub SetArguments(<MarshalAs(UnmanagedType.LPWStr)> pszArgs As String)
    Sub GetHotkey(pwHotkey As Short)
    Sub SetHotkey(pwHotkey As Short)
    Sub GetShowCmd(piShowCmd As UInteger)
    Sub SetShowCmd(piShowCmd As UInteger)
    Sub GetIconLocation(<Out(), MarshalAs(UnmanagedType.LPWStr)> pszIconPath As StringBuilder, cchIconPath As Integer, piIcon As Integer)
    Sub SetIconLocation(<MarshalAs(UnmanagedType.LPWStr)> pszIconPath As String, iIcon As Integer)
    Sub SetRelativePath(<MarshalAs(UnmanagedType.LPWStr)> pszPathRel As String, dwReserved As UInteger)
    Sub Resolve(hWnd As IntPtr, fFlags As UInteger)
    Sub SetPath(<MarshalAs(UnmanagedType.LPWStr)> pszFile As String)
  End Interface
#End Region
#Region "ShellLinkCoClass"
  <GuidAttribute("00021401-0000-0000-C000-000000000046")> _
  <ClassInterfaceAttribute(ClassInterfaceType.None)> _
  <ComImportAttribute()> _
  Private Class CShellLink
  End Class
#End Region
#Region "Private IShellLink enumerations"
  Private Enum EShellLinkGP As UInteger
    SLGP_SHORTPATH = 1
    SLGP_UNCPRIORITY = 2
  End Enum
  <Flags()> _
  Private Enum EShowWindowFlags As UInteger
    SW_HIDE = 0
    SW_SHOWNORMAL = 1
    SW_NORMAL = 1
    SW_SHOWMINIMIZED = 2
    SW_SHOWMAXIMIZED = 3
    SW_MAXIMIZE = 3
    SW_SHOWNOACTIVATE = 4
    SW_SHOW = 5
    SW_MINIMIZE = 6
    SW_SHOWMINNOACTIVE = 7
    SW_SHOWNA = 8
    SW_RESTORE = 9
    SW_SHOWDEFAULT = 10
    SW_MAX = 10
  End Enum
#End Region
#Region "IShellLink Private structs"
  <StructLayoutAttribute(LayoutKind.Sequential, Pack:=4, Size:=0, CharSet:=CharSet.Unicode)> _
  Private Structure _WIN32_FIND_DATAW
    Public dwFileAttributes As UInteger
    Public ftCreationTime As _FILETIME
    Public ftLastAccessTime As _FILETIME
    Public ftLastWriteTime As _FILETIME
    Public nFileSizeHigh As UInteger
    Public nFileSizeLow As UInteger
    Public dwReserved0 As UInteger
    Public dwReserved1 As UInteger
    <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)> Public cFileName As String
    <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=14)> Public cAlternateFileName As String
  End Structure
  <StructLayoutAttribute(LayoutKind.Sequential, Pack:=4, Size:=0, CharSet:=CharSet.Ansi)> _
  Private Structure _WIN32_FIND_DATAA
    Public dwFileAttributes As UInteger
    Public ftCreationTime As _FILETIME
    Public ftLastAccessTime As _FILETIME
    Public ftLastWriteTime As _FILETIME
    Public nFileSizeHigh As UInteger
    Public nFileSizeLow As UInteger
    Public dwReserved0 As UInteger
    Public dwReserved1 As UInteger
    <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)> Public cFileName As String
    <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=14)> Public cAlternateFileName As String
  End Structure
  <StructLayoutAttribute(LayoutKind.Sequential, Pack:=4, Size:=0)> _
  Private Structure _FILETIME
    Public dwLowDateTime As UInteger
    Public dwHighDateTime As UInteger
  End Structure
#End Region
#End Region
#Region "Enumerations"
  Public Enum LinkDisplayMode As UInteger
    edmNormal = EShowWindowFlags.SW_NORMAL
    edmMinimized = EShowWindowFlags.SW_SHOWMINNOACTIVE
    edmMaximized = EShowWindowFlags.SW_MAXIMIZE
  End Enum
#End Region
#Region "Member Variables"
  Private linkW As IShellLinkW
  Private linkA As IShellLinkA
#End Region
#Region "Constructor"
  Public Sub New()
    If System.Environment.OSVersion.Platform = PlatformID.Win32NT Then
      linkW = DirectCast(New CShellLink(), IShellLinkW)
    Else
      linkA = DirectCast(New CShellLink(), IShellLinkA)
    End If
  End Sub
#End Region
#Region "Destructor and Dispose"
  Protected Overrides Sub Finalize()
    Try
      Dispose()
    Finally
      MyBase.Finalize()
    End Try
  End Sub
  Public Sub Dispose() Implements System.IDisposable.Dispose
    If linkW IsNot Nothing Then
      Marshal.ReleaseComObject(linkW)
      linkW = Nothing
    End If
    If linkA IsNot Nothing Then
      Marshal.ReleaseComObject(linkA)
      linkA = Nothing
    End If
  End Sub
#End Region
#Region "Implementation"
  Public WriteOnly Property Target() As String
    Set(value As String)
      If linkA Is Nothing Then
        linkW.SetPath(value)
      Else
        linkA.SetPath(value)
      End If
    End Set
  End Property
  Public WriteOnly Property WorkingDirectory() As String
    Set(value As String)
      If linkA Is Nothing Then
        linkW.SetWorkingDirectory(value)
      Else
        linkA.SetWorkingDirectory(value)
      End If
    End Set
  End Property
  Public WriteOnly Property Description() As String
    Set(value As String)
      If linkA Is Nothing Then
        linkW.SetDescription(value)
      Else
        linkA.SetDescription(value)
      End If
    End Set
  End Property
  Public WriteOnly Property DisplayMode() As LinkDisplayMode
    Set(value As LinkDisplayMode)
      If linkA Is Nothing Then
        linkW.SetShowCmd(CUInt(value))
      Else
        linkA.SetShowCmd(CUInt(value))
      End If
    End Set
  End Property
  Public Sub Save(linkFile As String)
    If linkA Is Nothing Then
      DirectCast(linkW, IPersistFile).Save(linkFile, True)
    Else
      DirectCast(linkA, IPersistFile).Save(linkFile, True)
    End If
  End Sub
#End Region
End Class
