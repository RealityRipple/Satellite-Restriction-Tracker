Imports System.Runtime.InteropServices
Public Class CurrentOS
  Private Shared m_Windows As Boolean
  Private Shared m_Unix As Boolean
  Private Shared m_Mac As Boolean
  Private Shared m_Linux As Boolean
  Private Shared m_Unknown As Boolean
  Private Shared m_32 As Boolean
  Private Shared m_64 As Boolean
  Private Shared m_Name As String
  <DllImport("kernel32.dll", SetLastError:=True, CallingConvention:=CallingConvention.Winapi)>
  Private Shared Function IsWow64Process(hProcess As IntPtr, ByRef wow64Process As Boolean) As <MarshalAs(UnmanagedType.Bool)> Boolean
  End Function
  Public Shared ReadOnly Property IsWindows As Boolean
    Get
      Return m_Windows
    End Get
  End Property
  Public Shared ReadOnly Property IsUnix As Boolean
    Get
      Return m_Unix
    End Get
  End Property
  Public Shared ReadOnly Property IsMac As Boolean
    Get
      Return m_Mac
    End Get
  End Property
  Public Shared ReadOnly Property IsLinux As Boolean
    Get
      Return m_Linux
    End Get
  End Property
  Public Shared ReadOnly Property IsUnknown As Boolean
    Get
      Return m_Unknown
    End Get
  End Property
  Public Shared ReadOnly Property Is32bit As Boolean
    Get
      Return m_32
    End Get
  End Property
  Public Shared ReadOnly Property Is64bit As Boolean
    Get
      Return m_64
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
      Return m_Name
    End Get
  End Property
  Private Shared Function Is64BitWindows() As Boolean
    If Is64BitProcess Then Return True
    If (Environment.OSVersion.Version.Major = 5 And Environment.OSVersion.Version.Minor >= 1) Or Environment.OSVersion.Version.Major >= 6 Then
      Using p = System.Diagnostics.Process.GetCurrentProcess()
        Dim retVal As Boolean
        If (IsWow64Process(p.Handle, retVal) = False) Then Return False
        Return retVal
      End Using
    Else
      Return False
    End If
  End Function
  Shared Sub New()
    m_Windows = System.IO.Path.DirectorySeparatorChar = "\"
    If m_Windows Then
      m_Name = Environment.OSVersion.VersionString
      m_Name = m_Name.Replace("Microsoft ", "")
      m_Name = m_Name.Replace("  ", " ")
      m_Name = m_Name.Replace(" )", ")")
      m_Name = m_Name.Trim()
      m_Name = m_Name.Replace("NT 10.", "10 %bit 10.")
      m_Name = m_Name.Replace("NT 6.3", "8.1 %bit 6.3")
      m_Name = m_Name.Replace("NT 6.2", "8 %bit 6.2")
      m_Name = m_Name.Replace("NT 6.1", "7 %bit 6.1")
      m_Name = m_Name.Replace("NT 6.0", "Vista %bit 6.0")
      m_Name = m_Name.Replace("NT 5.", "XP %bit 5.")
      If Is64BitWindows() Then
        If Is64BitProcess Then
          m_Name = m_Name.Replace("%bit", "x64")
        Else
          m_Name = m_Name.Replace("%bit", "x86_64")
        End If
        m_64 = True
      Else
        m_Name = m_Name.Replace("%bit", "x86")
        m_32 = True
      End If
    Else
      Dim UnixName As String = ReadProcessOutput("uname")
      If UnixName.Contains("Darwin") Then
        m_Unix = True
        m_Mac = True
        m_Name = ReadProcessOutput("sw_vers", "-productVersion")
        m_Name = "macOS " & m_Name.Trim
        Dim machine As String = ReadProcessOutput("uname", "-m")
        If machine.Contains("x86_64") Then
          m_64 = True
          m_Name &= " x64"
        Else
          m_32 = True
          m_Name &= " x86"
        End If
      ElseIf UnixName.Contains("Linux") Then
        m_Unix = True
        m_Linux = True
        m_Name = ReadProcessOutput("lsb_release", "-d")
        m_Name = m_Name.Substring(m_Name.IndexOf(":") + 1)
        m_Name = "Linux " & m_Name.Trim
        Dim machine As String = ReadProcessOutput("uname", "-m")
        If machine.Contains("x86_64") Then
          m_64 = True
          m_Name &= " x64"
        Else
          m_32 = True
          m_Name &= " x86"
        End If
        m_Name &= " " & Environment.OSVersion.Version.Major & "." & Environment.OSVersion.Version.Minor & "." & Environment.OSVersion.Version.Build & "-" & Environment.OSVersion.Version.Revision
      ElseIf Not String.IsNullOrEmpty(UnixName) Then
        m_Unix = True
        m_Name = "Unknown Unix (" & UnixName & ")"
        m_Name &= " " & Environment.OSVersion.Version.ToString
      Else
        m_Unknown = True
        m_Name = "Unknown"
        m_Name &= " " & Environment.OSVersion.Version.ToString
      End If
    End If
  End Sub
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
