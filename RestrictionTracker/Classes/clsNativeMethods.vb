Imports System.Runtime.InteropServices
Friend NotInheritable Class NativeMethods
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
    NTE_BAD_UID = &H80090001UI
    NTE_BAD_HASH = &H80090002UI
    NTE_BAD_KEY = &H80090003UI
    NTE_BAD_LEN = &H80090004UI
    NTE_BAD_DATA = &H80090005UI
    NTE_BAD_SIGNATURE = &H80090006UI
    NTE_BAD_VER = &H80090007UI
    NTE_BAD_ALGID = &H80090008UI
    NTE_BAD_FLAGS = &H80090009UI
    NTE_BAD_TYPE = &H8009000AUI
    NTE_BAD_KEY_STATE = &H8009000BUI
    NTE_BAD_HASH_STATE = &H8009000CUI
    NTE_NO_KEY = &H8009000DUI
    NTE_NO_MEMORY = &H8009000EUI
    NTE_EXISTS = &H8009000FUI
    NTE_PERM = &H80090010UI
    NTE_NOT_FOUND = &H80090011UI
    NTE_DOUBLE_ENCRYPT = &H80090012UI
    NTE_BAD_PROVIDER = &H80090013UI
    NTE_BAD_PROV_TYPE = &H80090014UI
    NTE_BAD_PUBLIC_KEY = &H80090015UI
    NTE_BAD_KEYSET = &H80090016UI
    NTE_PROV_TYPE_NOT_DEF = &H80090017UI
    NTE_PROV_TYPE_ENTRY_BAD = &H80090018UI
    NTE_KEYSET_NOT_DEF = &H80090019UI
    NTE_KEYSET_ENTRY_BAD = &H8009001AUI
    NTE_PROV_TYPE_NO_MATCH = &H8009001BUI
    NTE_SIGNATURE_FILE_BAD = &H8009001CUI
    NTE_PROVIDER_DLL_FAIL = &H8009001DUI
    NTE_PROV_DLL_NOT_FOUND = &H8009001EUI
    NTE_BAD_KEYSET_PARAM = &H8009001FUI
    NTE_FAIL = &H80090020UI
    NTE_SYS_ERR = &H80090021UI
    CRYPT_E_MSG_ERROR = &H80091001UI
    CRYPT_E_UNKNOWN_ALGO = &H80091002UI
    CRYPT_E_OID_FORMAT = &H80091003UI
    CRYPT_E_INVALID_MSG_TYPE = &H80091004UI
    CRYPT_E_UNEXPECTED_ENCODING = &H80091005UI
    CRYPT_E_AUTH_ATTR_MISSING = &H80091006UI
    CRYPT_E_HASH_VALUE = &H80091007UI
    CRYPT_E_INVALID_INDEX = &H80091008UI
    CRYPT_E_ALREADY_DECRYPTED = &H80091009UI
    CRYPT_E_NOT_DECRYPTED = &H8009100AUI
    CRYPT_E_RECIPIENT_NOT_FOUND = &H8009100BUI
    CRYPT_E_CONTROL_TYPE = &H8009100CUI
    CRYPT_E_ISSUER_SERIALNUMBER = &H8009100DUI
    CRYPT_E_SIGNER_NOT_FOUND = &H8009100EUI
    CRYPT_E_ATTRIBUTES_MISSING = &H8009100FUI
    CRYPT_E_STREAM_MSG_NOT_READY = &H80091010UI
    CRYPT_E_STREAM_INSUFFICIENT_DATA = &H80091011UI
    CRYPT_E_BAD_LEN = &H80092001UI
    CRYPT_E_BAD_ENCODE = &H80092002UI
    CRYPT_E_FILE_ERROR = &H80092003UI
    CRYPT_E_NOT_FOUND = &H80092004UI
    CRYPT_E_EXISTS = &H80092005UI
    CRYPT_E_NO_PROVIDER = &H80092006UI
    CRYPT_E_SELF_SIGNED = &H80092007UI
    CRYPT_E_DELETED_PREV = &H80092008UI
    CRYPT_E_NO_MATCH = &H80092009UI
    CRYPT_E_UNEXPECTED_MSG_TYPE = &H8009200AUI
    CRYPT_E_NO_KEY_PROPERTY = &H8009200BUI
    CRYPT_E_NO_DECRYPT_CERT = &H8009200CUI
    CRYPT_E_BAD_MSG = &H8009200DUI
    CRYPT_E_NO_SIGNER = &H8009200EUI
    CRYPT_E_PENDING_CLOSE = &H8009200FUI
    CRYPT_E_REVOKED = &H80092010UI
    CRYPT_E_NO_REVOCATION_DLL = &H80092011UI
    CRYPT_E_NO_REVOCATION_CHECK = &H80092012UI
    CRYPT_E_REVOCATION_OFFLINE = &H80092013UI
    CRYPT_E_NOT_IN_REVOCATION_DATABASE = &H80092014UI
    CRYPT_E_INVALID_NUMERIC_STRING = &H80092020UI
    CRYPT_E_INVALID_PRINTABLE_STRING = &H80092021UI
    CRYPT_E_INVALID_IA5_STRING = &H80092022UI
    CRYPT_E_INVALID_X500_STRING = &H80092023UI
    CRYPT_E_NOT_CHAR_STRING = &H80092024UI
    CRYPT_E_FILERESIZED = &H80092025UI
    CRYPT_E_SECURITY_SETTINGS = &H80092026UI
    CRYPT_E_NO_VERIFY_USAGE_DLL = &H80092027UI
    CRYPT_E_NO_VERIFY_USAGE_CHECK = &H80092028UI
    CRYPT_E_VERIFY_USAGE_OFFLINE = &H80092029UI
    CRYPT_E_NOT_IN_CTL = &H8009202AUI
    CRYPT_E_NO_TRUSTED_SIGNER = &H8009202BUI
    CRYPT_E_OSS_ERROR = &H80093000UI
    CERTSRV_E_BAD_REQUESTSUBJECT = &H80094001UI
    CERTSRV_E_NO_REQUEST = &H80094002UI
    CERTSRV_E_BAD_REQUESTSTATUS = &H80094003UI
    CERTSRV_E_PROPERTY_EMPTY = &H80094004UI
    TRUST_E_SYSTEM_ERROR = &H80096001UI
    TRUST_E_NO_SIGNER_CERT = &H80096002UI
    TRUST_E_COUNTER_SIGNER = &H80096003UI
    TRUST_E_CERT_SIGNATURE = &H80096004UI
    TRUST_E_TIME_STAMP = &H80096005UI
    'TRUST_E_BAD_DIGEST = &H80096010UI
    TRUST_E_BASIC_CONSTRAINTS = &H80096019UI
    TRUST_E_FINANCIAL_CRITERIA = &H8009601EUI
    TRUST_E_PROVIDER_UNKNOWN = &H800B0001UI
    TRUST_E_ACTION_UNKNOWN = &H800B0002UI
    TRUST_E_SUBJECT_FORM_UNKNOWN = &H800B0003UI
    TRUST_E_SUBJECT_NOT_TRUSTED = &H800B0004UI
    DIGSIG_E_ENCODE = &H800B0005UI
    DIGSIG_E_DECODE = &H800B0006UI
    DIGSIG_E_EXTENSIBILITY = &H800B0007UI
    DIGSIG_E_CRYPTO = &H800B0008UI
    PERSIST_E_SIZEDEFINITE = &H800B0009UI
    PERSIST_E_SIZEINDEFINITE = &H800B000AUI
    PERSIST_E_NOTSELFSIZING = &H800B000BUI
    'TRUST_E_NOSIGNATURE = &H800B0100UI
    CERT_E_EXPIRED = &H800B0101UI
    CERT_E_VALIDITYPERIODNESTING = &H800B0102UI
    CERT_E_ROLE = &H800B0103UI
    CERT_E_PATHLENCONST = &H800B0104UI
    CERT_E_CRITICAL = &H800B0105UI
    CERT_E_PURPOSE = &H800B0106UI
    CERT_E_ISSUERCHAINING = &H800B0107UI
    CERT_E_MALFORMED = &H800B0108UI
    'CERT_E_UNTRUSTEDROOT = &H800B0109UI
    CERT_E_CHAINING = &H800B010AUI
    TRUST_E_FAIL = &H800B010BUI
    CERT_E_REVOKED = &H800B010CUI
    CERT_E_UNTRUSTEDTESTROOT = &H800B010DUI
    CERT_E_REVOCATION_FAILURE = &H800B010EUI
    CERT_E_CN_NO_MATCH = &H800B010FUI
    CERT_E_WRONG_USAGE = &H800B0110UI
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

  Private Sub New()
  End Sub
End Class
