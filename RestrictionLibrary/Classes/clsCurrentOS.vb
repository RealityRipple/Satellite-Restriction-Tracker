Imports System.Runtime.InteropServices
Public NotInheritable Class CurrentOS
  Private NotInheritable Class NativeMethods
    <DllImport("kernel32", CallingConvention:=CallingConvention.Winapi)>
    Public Shared Function IsWow64Process(hProcess As IntPtr, <MarshalAs(UnmanagedType.Bool)> ByRef wow64Process As Boolean) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
    Private Sub New()
    End Sub
  End Class
  Private Shared bPopulated As Boolean = False
  Private Shared bWindows As Boolean
  Private Shared bUnix As Boolean
  Private Shared bMac As Boolean
  Private Shared bLinux As Boolean
  Private Shared bUnknown As Boolean
  Private Shared b32 As Boolean
  Private Shared b64 As Boolean
  Private Shared sName As String
  Private Sub New()
  End Sub
  Private Shared Sub Init()
    If bPopulated Then Return
    bWindows = System.IO.Path.DirectorySeparatorChar = "\"
    If bWindows Then
      sName = Environment.OSVersion.VersionString
      sName = sName.Replace("Microsoft ", "")
      sName = sName.Replace("  ", " ")
      sName = sName.Replace(" )", ")")
      sName = sName.Trim()
      sName = sName.Replace("NT 11.", "11 %bit 11.")
      sName = sName.Replace("NT 10.", "10 %bit 10.")
      sName = sName.Replace("NT 6.3", "8.1 %bit 6.3")
      sName = sName.Replace("NT 6.2", "8 %bit 6.2")
      sName = sName.Replace("NT 6.1", "7 %bit 6.1")
      sName = sName.Replace("NT 6.0", "Vista %bit 6.0")
      sName = sName.Replace("NT 5.", "XP %bit 5.")
      If Is64BitWindows() Then
        If Is64BitProcess Then
          sName = sName.Replace("%bit", "x64")
        Else
          sName = sName.Replace("%bit", "x86_64")
        End If
        b64 = True
      Else
        sName = sName.Replace("%bit", "x86")
        b32 = True
      End If
    Else
      Dim UnixName As String = ReadProcessOutput("uname")
      If UnixName.Contains("Darwin") Then
        bUnix = True
        bMac = True
        sName = ReadProcessOutput("sw_vers", "-productVersion")
        sName = "macOS " & sName.Trim
        Dim machine As String = ReadProcessOutput("uname", "-m")
        If machine.Contains("x86_64") Then
          b64 = True
          sName &= " x64"
        Else
          b32 = True
          sName &= " x86"
        End If
      ElseIf UnixName.Contains("Linux") Then
        bUnix = True
        bLinux = True
        sName = ReadProcessOutput("lsb_release", "-d")
        sName = sName.Substring(sName.IndexOf(":") + 1)
        sName = "Linux " & sName.Trim
        Dim machine As String = ReadProcessOutput("uname", "-m")
        If machine.Contains("x86_64") Then
          b64 = True
          sName &= " x64"
        Else
          b32 = True
          sName &= " x86"
        End If
        sName &= " " & Environment.OSVersion.Version.Major & "." & Environment.OSVersion.Version.Minor & "." & Environment.OSVersion.Version.Build & "-" & Environment.OSVersion.Version.Revision
      ElseIf Not String.IsNullOrEmpty(UnixName) Then
        bUnix = True
        sName = "Unknown Unix (" & UnixName & ")"
        sName &= " " & Environment.OSVersion.Version.ToString
      Else
        bUnknown = True
        sName = "Unknown"
        sName &= " " & Environment.OSVersion.Version.ToString
      End If
    End If
    bPopulated = True
  End Sub
  Public Shared ReadOnly Property IsWindows As Boolean
    Get
      Init()
      Return bWindows
    End Get
  End Property
  Public Shared ReadOnly Property IsUnix As Boolean
    Get
      Init()
      Return bUnix
    End Get
  End Property
  Public Shared ReadOnly Property IsMac As Boolean
    Get
      Init()
      Return bMac
    End Get
  End Property
  Public Shared ReadOnly Property IsLinux As Boolean
    Get
      Init()
      Return bLinux
    End Get
  End Property
  Public Shared ReadOnly Property IsUnknown As Boolean
    Get
      Init()
      Return bUnknown
    End Get
  End Property
  Public Shared ReadOnly Property Is32bit As Boolean
    Get
      Init()
      Return b32
    End Get
  End Property
  Public Shared ReadOnly Property Is64bit As Boolean
    Get
      Init()
      Return b64
    End Get
  End Property
  Public Shared ReadOnly Property Is32BitProcess As Boolean
    Get
      Return (IntPtr.Size = 4)
    End Get
  End Property
  Public Shared ReadOnly Property Is64BitProcess As Boolean
    Get
      Return (IntPtr.Size = 8)
    End Get
  End Property
  Public Shared ReadOnly Property Name As String
    Get
      Init()
      Return sName
    End Get
  End Property
  Private Shared Function Is64BitWindows() As Boolean
    If Is64BitProcess Then Return True
    If (Environment.OSVersion.Version.Major = 5 And Environment.OSVersion.Version.Minor >= 1) Or Environment.OSVersion.Version.Major >= 6 Then
      Using p = System.Diagnostics.Process.GetCurrentProcess()
        Dim retVal As Boolean
        If (NativeMethods.IsWow64Process(p.Handle, retVal) = False) Then Return False
        Return retVal
      End Using
    Else
      Return False
    End If
  End Function
  Private Shared Function ReadProcessOutput(name As String) As String
    Return ReadProcessOutput(name, Nothing)
  End Function
  Private Shared Function ReadprocessOutput(name As String, args As String) As String
    Try
      Dim p As New System.Diagnostics.Process
      p.StartInfo.UseShellExecute = False
      p.StartInfo.RedirectStandardOutput = True
      If Not String.IsNullOrEmpty(args) Then p.StartInfo.Arguments = " " & args
      p.StartInfo.FileName = name
      p.Start()
      Dim output As String = p.StandardOutput.ReadToEnd()
      p.WaitForExit()
      If String.IsNullOrEmpty(output) Then output = ""
      output = output.Trim()
      Return output
    Catch ex As Exception
      Return ""
    End Try
  End Function
End Class
