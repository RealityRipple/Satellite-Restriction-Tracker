Imports System.Runtime.InteropServices
Public NotInheritable Class NativeMethods
  Public Const WM_WINDOWPOSCHANGING As Int32 = &H46
  Public Const WM_SYSCOMMAND As Int32 = &H112
  Public Const WM_DWMCOMPOSITIONCHANGED As Int32 = &H31E
  Public Const WM_NCLBUTTONDOWN As Integer = &HA1
  Public Const WM_GETSYSMENU As Integer = &H313
  Public Const BCM_SETSHIELD As Int32 = &H160C
  Public Const SC_MINIMIZE As Int32 = &HF020
  Public Const SPI_GETANIMATION As UInt32 = &H48
  Public Const ABM_GETTASKBARPOS = &H5
  Public Const ABM_GETSTATE = &H4
  Public Const DTT_COMPOSITED As Int32 = CInt((1 << 13))
  Public Const DTT_GLOWSIZE As Int32 = CInt((1 << 11))
  Public Const DT_SINGLELINE As Int32 = &H20
  Public Const DT_CENTER As Int32 = &H1
  Public Const DT_VCENTER As Int32 = &H4
  Public Const DT_NOPREFIX As Int32 = &H800
  Public Const SRCCOPY As Int32 = &HCC0020
  Public Const BI_RGB As Int32 = 0
  Public Const DIB_RGB_COLORS As Int32 = 0
  Public Const HTCAPTION As Integer = 2

  <Flags()>
  Public Enum MenuFlags As Int32
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
  Public Enum MetricsList As Int32
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
  <Flags()>
  Public Enum WINDOWPOS_FLAGS As UInt32
    SWP_NOSIZE = &H1
    SWP_NOMOVE = &H2
    SWP_NOZORDER = &H4
    SWP_NOREDRAW = &H8
    SWP_NOACTIVATE = &H10
    SWP_DRAWFRAME = &H20
    SWP_SHOWWINDOW = &H40
    SWP_HIDEWINDOW = &H80
    SWP_NOCOPYBITS = &H100
    SWP_NOOWNERZORDER = &H200
    SWP_NOSENDCHANGING = &H400
    SWP_NOCLIENTSIZE = &H800
    SWP_NOCLIENTMOVE = &H1000
    SWP_STATECHANGED = &H8000
  End Enum
  Public Enum ABEdge
    ABE_LEFT = 0
    ABE_TOP
    ABE_RIGHT
    ABE_BOTTOM
  End Enum
  Public Enum Validity As UInt32
    Unsigned = &H800B0100UI
    SignedButBad = &H80096010UI
    SignedButInvalid = &H800B0000UI
    SignedButUntrusted = &H800B0109UI
    SignedAndValid = 0
    BadThumb = &HA0090001UI
    BadSerial = &HA0090002UI
    BadSubject = &HA0090003UI
    BadRootThumb = &HA0090101UI
    BadRootSerial = &HA0090102UI
    BadRootSubject = &HA0090103UI
  End Enum

  Public Structure ANIMATIONINFO
    Public Size As Int32
    Public MinAnimate As Int32
  End Structure
  Public Structure WINDOWPOS
    Public hWnd As IntPtr
    Public hWndInsertAfter As IntPtr
    Public X As Int32
    Public Y As Int32
    Public Width As Int32
    Public Height As Int32
    Public Flags As WINDOWPOS_FLAGS
  End Structure
  Public Structure APPBARDATA
    Public cbSize As Int32
    Public hwnd As IntPtr
    Public uCallbackMessage As Int32
    Public uEdge As ABEdge
    Public rc As RECT
    Public lParam As Int32
  End Structure
  Public Structure RECT
    Public Left As Int32
    Public Top As Int32
    Public Right As Int32
    Public Bottom As Int32
  End Structure
  Public Structure MARGINS
    Public m_Left As Int32
    Public m_Right As Int32
    Public m_Top As Int32
    Public m_Bottom As Int32
  End Structure
  Public Structure POINTAPI
    Public x As Int32
    Public y As Int32
  End Structure
  Public Structure DTTOPTS
    Public dwSize As UInt32
    Public dwFlags As UInt32
    Public crText As UInt32
    Public crBorder As UInt32
    Public crShadow As UInt32
    Public iTextShadowType As Int32
    Public ptShadowOffset As POINTAPI
    Public iBorderSize As Int32
    Public iFontPropId As Int32
    Public iColorPropId As Int32
    Public iStateId As Int32
    Public fApplyOverlay As Int32
    Public iGlowSize As Int32
    Public pfnDrawTextCallback As IntPtr
    Public lParam As Int32
  End Structure
  Public Structure BITMAPINFOHEADER
    Public biSize As Int32
    Public biWidth As Int32
    Public biHeight As Int32
    Public biPlanes As Int16
    Public biBitCount As Int16
    Public biCompression As Int32
    Public biSizeImage As Int32
    Public biXPelsPerMeter As Int32
    Public biYPelsPerMeter As Int32
    Public biClrUsed As Int32
    Public biClrImportant As Int32
  End Structure
  Public Structure RGBQUAD
    Public rgbBlue As Byte
    Public rgbGreen As Byte
    Public rgbRed As Byte
    Public rgbReserved As Byte
  End Structure
  Public Structure BITMAPINFO
    Public bmiHeader As BITMAPINFOHEADER
    Public bmiColors As RGBQUAD
  End Structure


  <DllImport("user32", CharSet:=CharSet.Unicode)>
  Public Shared Function ShowWindow(hWnd As IntPtr, nCmdShow As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean
  End Function
  <DllImport("user32", CharSet:=CharSet.Unicode)>
  Public Shared Function GetSystemMenu(hWnd As IntPtr, <MarshalAs(UnmanagedType.Bool)> bRevert As Boolean) As IntPtr
  End Function
  <DllImport("user32", CharSet:=CharSet.Unicode)>
  Public Shared Function InsertMenu(hMenu As IntPtr, uPosition As Int32, uFlags As Int32, uIDNewItem As IntPtr, lpNewItem As String) As <MarshalAs(UnmanagedType.Bool)> Boolean
  End Function
  <DllImport("user32", CharSet:=CharSet.Unicode)>
  Public Shared Function ModifyMenu(hMenu As IntPtr, uPosition As Int32, uFlags As Int32, uIDNewItem As IntPtr, lpNewItem As String) As <MarshalAs(UnmanagedType.Bool)> Boolean
  End Function
  <DllImport("user32", CharSet:=CharSet.Unicode)>
  Public Shared Function FindWindow(lpClassName As String, lpWindowName As String) As IntPtr
  End Function
  <DllImport("user32", CharSet:=CharSet.Unicode)>
  Public Shared Function FindWindowEx(parentHandle As IntPtr, childAfter As IntPtr, lclassName As String, windowTitle As String) As IntPtr
  End Function
  <DllImport("user32", CharSet:=CharSet.Unicode)>
  Public Shared Function SetWindowText(hWnd As IntPtr, lpString As String) As <MarshalAs(UnmanagedType.Bool)> Boolean
  End Function
  <DllImport("user32", CharSet:=CharSet.Unicode)>
  Public Shared Function LoadCursor(hInstance As IntPtr, lpCursorName As IntPtr) As IntPtr
  End Function
  <DllImport("user32", CharSet:=CharSet.Unicode)>
  Public Shared Function SetCursor(hCursor As IntPtr) As IntPtr
  End Function
  <DllImport("user32", CharSet:=CharSet.Unicode)>
  Public Shared Function GetAncestor(ByVal hWnd As IntPtr, ByVal gaFlags As Int32) As IntPtr
  End Function
  <DllImport("user32", CharSet:=CharSet.Unicode)>
  Public Shared Function GetSystemMetrics(ByVal nIndex As MetricsList) As Int32
  End Function
  <DllImport("user32", CharSet:=CharSet.Unicode)>
  Public Shared Function DestroyIcon(ByVal hWnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
  End Function
  <DllImport("user32", CharSet:=CharSet.Unicode)>
  Public Shared Function SystemParametersInfo(uAction As UInt32, uParam As UInt32, ByRef lpvParam As ANIMATIONINFO, fuWinIni As UInt32) As UInt32
  End Function
  <DllImport("user32", CharSet:=CharSet.Unicode)>
  Friend Shared Function SendMessage(hWnd As IntPtr, msg As Int32, wp As IntPtr, lp As IntPtr) As IntPtr
  End Function
  <DllImport("user32", CharSet:=CharSet.Unicode)>
  Public Shared Function GetDC(ByVal hdc As IntPtr) As IntPtr
  End Function
  <DllImport("user32", CharSet:=CharSet.Unicode)>
  Public Shared Function ReleaseCapture() As <MarshalAs(UnmanagedType.Bool)> Boolean
  End Function
  <DllImport("user32", CharSet:=CharSet.Unicode)>
  Public Shared Function ReleaseDC(ByVal hdc As IntPtr, ByVal state As IntPtr) As Int32
  End Function

  <DllImport("gdi32", CharSet:=CharSet.Unicode)>
  Public Shared Function SaveDC(ByVal hdc As IntPtr) As Int32
  End Function
  <DllImport("gdi32", CharSet:=CharSet.Unicode)>
  Public Shared Function CreateCompatibleDC(ByVal hDC As IntPtr) As IntPtr
  End Function
  <DllImport("gdi32", CharSet:=CharSet.Unicode)>
  Public Shared Function SelectObject(ByVal hDC As IntPtr, ByVal hObject As IntPtr) As IntPtr
  End Function
  <DllImport("gdi32", CharSet:=CharSet.Unicode)>
  Public Shared Function DeleteObject(ByVal hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
  End Function
  <DllImport("gdi32", CharSet:=CharSet.Unicode)>
  Public Shared Function DeleteDC(ByVal hdc As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
  End Function
  <DllImport("gdi32", CharSet:=CharSet.Unicode)>
  Public Shared Function BitBlt(ByVal hdc As IntPtr, ByVal nXDest As Int32, ByVal nYDest As Int32, ByVal nWidth As Int32, ByVal nHeight As Int32, ByVal hdcSrc As IntPtr, ByVal nXSrc As Int32, ByVal nYSrc As Int32, ByVal dwRop As UInt32) As <MarshalAs(UnmanagedType.Bool)> Boolean
  End Function
  <DllImport("gdi32", CharSet:=CharSet.Unicode)>
  Public Shared Function CreateDIBSection(ByVal hdc As IntPtr, ByRef pbmi As BITMAPINFO, ByVal iUsage As UInt32, ByVal ppvBits As IntPtr, ByVal hSection As IntPtr, ByVal dwOffset As UInt32) As IntPtr
  End Function

  <DllImport("shell32", CharSet:=CharSet.Unicode)>
  Public Shared Function SHAppBarMessage(ByVal dwMessage As Int32, ByRef pData As APPBARDATA) As IntPtr
  End Function

  <DllImport("wintrust", CharSet:=CharSet.Unicode)>
  Public Shared Function WinVerifyTrust(hWnd As IntPtr, pgActionID As IntPtr, pWinTrustData As IntPtr) As UInt32
  End Function

  <DllImport("dwmapi", CharSet:=CharSet.Unicode)>
  Public Shared Sub DwmIsCompositionEnabled(ByRef enabledptr As Int32)
  End Sub
  <DllImport("dwmapi", CharSet:=CharSet.Unicode)>
  Public Shared Sub DwmExtendFrameIntoClientArea(ByVal hWnd As IntPtr, ByRef margin As MARGINS)
  End Sub

  <DllImport("uxtheme", CharSet:=CharSet.Unicode)>
  Public Shared Function DrawThemeTextEx(ByVal hTheme As IntPtr, ByVal hdc As IntPtr, ByVal iPartId As Int32, ByVal iStateId As Int32, ByVal text As String, ByVal iCharCount As Int32, ByVal dwFlags As Int32, ByRef pRect As RECT, ByRef pOptions As DTTOPTS) As Int32
  End Function

  <DllImport("winmm", CharSet:=CharSet.Unicode)>
  Public Shared Function mciSendString(ByVal strCommand As String, ByVal strReturn As System.Text.StringBuilder, ByVal iReturnLength As Int32, ByVal hwndCallback As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
  End Function
End Class
