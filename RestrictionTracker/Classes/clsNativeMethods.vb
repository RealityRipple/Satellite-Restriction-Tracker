Imports System.Runtime.InteropServices
Public NotInheritable Class NativeMethods
  Public Const WM_WINDOWPOSCHANGING As Integer = &H46
  Public Const WM_SYSCOMMAND As Integer = &H112
  Public Const SC_MINIMIZE As Integer = &HF020
  Public Const SPI_GETANIMATION As Integer = &H48
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
  Public Enum MetricsList As Integer
    SM_CXSCREEN = 0
    SM_CYSCREEN = 1
    SM_CXVSCROLL = 2
    SM_CYHSCROLL = 3
    SM_CYCAPTION = 4
    SM_CXBORDER = 5
    SM_CYBORDER = 6
    SM_CXDLGFRAME = 7
    SM_CYDLGFRAME = 8
    SM_CYVTHUMB = 9
    SM_CXHTHUMB = 10
    SM_CXICON = 11
    SM_CYICON = 12
    SM_CXCURSOR = 13
    SM_CYCURSOR = 14
    SM_CYMENU = 15
    SM_CXFULLSCREEN = 16
    SM_CYFULLSCREEN = 17
    SM_CYKANJIWINDOW = 18
    SM_MOUSEPRESENT = 19
    SM_CYVSCROLL = 20
    SM_CXHSCROLL = 21
    SM_DEBUG = 22
    SM_SWAPBUTTON = 23

    SM_CXMIN = 28
    SM_CYMIN = 29
    SM_CXSIZE = 30
    SM_CYSIZE = 31
    SM_CXFRAME = 32
    SM_CYFRAME = 33
    SM_CXMINTRACK = 34
    SM_CYMINTRACK = 35
    SM_CXDOUBLECLICK = 36
    SM_CYDOUBLECLICK = 37
    SM_CXICONSPACING = 38
    SM_CYICONSPACING = 39
    SM_MENUDROPALIGNMENT = 40
    SM_PENWINDOWS = 41
    SM_DBCSENABLED = 42
    SM_CMOUSEBUTTONS = 43
    SM_SECURE = 44
    SM_CXEDGE = 45
    SM_CYEDGE = 46
    SM_CXMINSPACING = 47
    SM_CYMINSPACING = 48
    SM_CXSMICON = 49
    SM_CYSMICON = 50
    SM_CYSMCAPTION = 51
    SM_CXSMSIZE = 52
    SM_CYSMSIZE = 53
    SM_CXMENUSIZE = 54
    SM_CYMENUSIZE = 55
    SM_ARRANGE = 56
    SM_CXMINIMZED = 57
    SM_CYMINIMIZED = 58
    SM_CXMAXTRACK = 59
    SM_CYMAXTRACK = 60
    SM_CXMAXIMIZED = 61
    SM_CYMAXIMIZED = 62
    SM_NETWORK = 63

    SM_CLEANBOOT = 67
    SM_CXDRAG = 68
    SM_CYDRAG = 69
    SM_SHOWSOUNDS = 70
    SM_CXMENUCHECK = 71
    SM_CYMENUCHECK = 72
    SM_SLOWMACHINE = 73
    SM_MIDEASTENABLED = 74
    SM_MOUSEWHEELPRESENT = 75
    SM_XVIRTUALSCREEN = 76
    SM_YVIRTUALSCREEN = 77
    SM_CXVIRTUALSCREEN = 78
    SM_CYVIRTUALSCREEN = 79
    SM_CMONITORS = 80
    SM_DISPLAYFORMAT = 81
    SM_IMMENABLED = 82
    SM_CXFOCUSBORDER = 83
    SM_CYFOCUSBORDER = 84

    SM_TABLETPC = 86
    SM_MEDIACENTER = 87
    SM_STARTER = 88
    SM_SERVER2 = 89

    SM_MOUSEHORIZONTALWHEELPRESENT = 91
    SM_CXPADDEDBORDER = 92

    SM_DIGITIZER = 94
    SM_MAXIMUMTOUCHES = 95

    SM_REMOTESESSION = &H1000

    SM_SHUTTINGDOWN = &H2000
    SM_REMOTECONTROL = &H2001
    SM_CONVERTIBLESLATEMODE = &H2003
    SM_SYSTEMDOCKED = &H2004
  End Enum
  Public Structure ANIMATIONINFO
    Public Size As Integer
    Public MinAnimate As Integer
  End Structure
  Public Const BCM_SETSHIELD As Integer = &H160C
  <Flags()>
  Public Enum WINDOWPOS_FLAGS As UInteger
    ''' <summary>
    '''  Retains the current size (ignores the cx and cy members).
    ''' </summary>
    SWP_NOSIZE = &H1
    ''' <summary>
    '''  Retains the current position (ignores the x and y members).
    ''' </summary>
    SWP_NOMOVE = &H2
    ''' <summary>
    '''  Retains the current Z order (ignores the hwndInsertAfter member).
    ''' </summary>
    SWP_NOZORDER = &H4
    ''' <summary>
    '''  Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
    ''' </summary>
    SWP_NOREDRAW = &H8
    ''' <summary>
    '''  Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hwndInsertAfter member).
    ''' </summary>
    SWP_NOACTIVATE = &H10
    ''' <summary>
    '''  Draws a frame (defined in the window's class description) around the window.
    ''' </summary>
    ''' <remarks>Also known as SWP_FRAMECHANGED, which sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.</remarks>
    SWP_DRAWFRAME = &H20
    ''' <summary>
    '''  Displays the window.
    ''' </summary>
    SWP_SHOWWINDOW = &H40
    ''' <summary>
    '''  Hides the window.
    ''' </summary>
    SWP_HIDEWINDOW = &H80
    ''' <summary>
    '''  Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
    ''' </summary>
    SWP_NOCOPYBITS = &H100
    ''' <summary>
    '''  Does not change the owner window's position in the Z order.
    ''' </summary>
    ''' <remarks>Also known as NOREPOSITION.
    SWP_NOOWNERZORDER = &H200
    ''' <summary>
    '''  Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
    ''' </summary>
    SWP_NOSENDCHANGING = &H400
    ''' <summary>
    '''  Used by Windows to determine if WM_SIZE needs to be sent.
    ''' </summary>
    ''' <remarks>Undocumented.</remarks>
    SWP_NOCLIENTSIZE = &H800
    ''' <summary>
    '''  Used by Windows to determine if WM_MOVE needs to be sent.
    ''' </summary>
    ''' <remarks>Undocumented.</remarks>
    SWP_NOCLIENTMOVE = &H1000
    ''' <summary>
    '''  Indicates that the window state is changing or has changed.
    ''' </summary>
    ''' <remarks>Undocumented.</remarks>
    SWP_STATECHANGED = &H8000
  End Enum
  Public Structure WINDOWPOS
    Public hWnd As IntPtr
    Public hWndInsertAfter As IntPtr
    Public X As Integer
    Public Y As Integer
    Public Width As Integer
    Public Height As Integer
    Public Flags As WINDOWPOS_FLAGS
  End Structure
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
  Public Shared Function GetSystemMetrics(ByVal nIndex As MetricsList) As Integer
  End Function
  <DllImport("user32", SetLastError:=True, CharSet:=CharSet.Auto)>
  Public Shared Function DestroyIcon(ByVal hWnd As IntPtr) As Boolean
  End Function
  <DllImport("user32", CharSet:=CharSet.Auto, setlasterror:=True)>
  Public Shared Function SendMessage(hWnd As IntPtr, msg As UInt32, wParam As UInt32, lParam As UInt32) As UInt32
  End Function
  <DllImport("user32", CharSet:=CharSet.Auto, SetLastError:=True)>
  Public Shared Function SystemParametersInfo(uAction As UInteger, uParam As UInteger, ByRef lpvParam As ANIMATIONINFO, fuWinIni As UInteger) As UInteger
  End Function
End Class
