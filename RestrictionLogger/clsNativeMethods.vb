Imports System.Runtime.InteropServices
Friend NotInheritable Class NativeMethods
  Public Enum Validity As UInteger
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

  <DllImport("wintrust", CharSet:=CharSet.Unicode)>
  Public Shared Function WinVerifyTrust(hWnd As IntPtr, pgActionID As IntPtr, pWinTrustData As IntPtr) As UInt32
  End Function

  Private Sub New()
  End Sub
End Class
