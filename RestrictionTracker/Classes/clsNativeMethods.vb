Imports System.Runtime.InteropServices
Public NotInheritable Class NativeMethods
  Public Const WM_SYSCOMMAND As Integer = &H112
  <Flags()> _
  Public Enum MenuFlags As Integer
    MF_BYCOMMAND = &H0
    MF_BYPOSITION = &H400
    MF_BITMAP = &H4
    MF_CHECKED = &H8
    MF_DISABLED = &H2
    MF_ENABLED = &H0
    MF_GRAYED = &H1
    MF_MENUBARBREAK = &H20
    MF_MENUBREAK = &H40
    MF_OWNERDRAW = &H100
    MF_POPUP = &H10
    MF_SEPARATOR = &H800
    MF_STRING = &H0
    MF_UNCHECKED = &H0
  End Enum
  <DllImport("user32", CharSet:=CharSet.Auto, setlasterror:=True)>
  Public Shared Function GetSystemMenu(hWnd As IntPtr, bRevert As Boolean) As IntPtr
  End Function
  <DllImport("user32", CharSet:=CharSet.Auto, setlasterror:=True)>
  Public Shared Function AppendMenu(hMenu As IntPtr, uFlags As Integer, uIDNewItem As Integer, lpNewItem As String) As IntPtr
  End Function
  <DllImport("user32", CharSet:=CharSet.Auto, setlasterror:=True)>
  Public Shared Function InsertMenu(hMenu As IntPtr, uPosition As Integer, uFlags As Integer, uIDNewItem As Integer, lpNewItem As String) As Boolean
  End Function
  <DllImport("user32", CharSet:=CharSet.Auto, setlasterror:=True)>
  Public Shared Function ModifyMenu(hMenu As IntPtr, uPosition As Integer, uFlags As Integer, uIDNewItem As Integer, lpNewItem As String) As Boolean
  End Function
  <DllImport("user32", CharSet:=CharSet.Unicode)> _
  Public Shared Function FindWindow(lpClassName As String, lpWindowName As String) As IntPtr
  End Function
  <DllImport("user32", SetLastError:=True, CharSet:=CharSet.Unicode)> _
  Public Shared Function FindWindowEx(parentHandle As IntPtr, childAfter As IntPtr, lclassName As String, windowTitle As String) As IntPtr
  End Function
  <DllImport("user32", SetLastError:=True, CharSet:=CharSet.Auto)>
  Public Shared Function SetWindowText(hWnd As IntPtr, lpString As String) As Boolean
  End Function
  <DllImport("user32", SetLastError:=True)>
  Public Shared Function LoadCursor(hInstance As IntPtr, lpCursorName As Integer) As Integer
  End Function
  <DllImport("user32", SetLastError:=True)>
  Public Shared Function SetCursor(hCursor As Integer) As Integer
  End Function
  <DllImport("user32", SetLastError:=True)>
  Public Shared Function GetAncestor(ByVal hWnd As IntPtr, ByVal gaFlags As Integer) As IntPtr
  End Function
  <DllImport("user32", SetLastError:=True, CharSet:=CharSet.Auto)>
  Public Shared Function DestroyIcon(ByVal hWnd As IntPtr) As Boolean
  End Function
End Class