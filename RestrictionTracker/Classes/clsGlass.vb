Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.Diagnostics
Friend Class clsGlass
  Private Sub New()
  End Sub
  Public Shared Sub SetGlass(Form As Form, left As Integer, top As Integer, right As Integer, bottom As Integer)
    Dim mg As New NativeMethods.MARGINS()
    mg.m_Bottom = bottom
    mg.m_Left = left
    mg.m_Right = right
    mg.m_Top = top
    If IsCompositionEnabled() Then NativeMethods.DwmExtendFrameIntoClientArea(Form.Handle, mg)
  End Sub
  Public Shared Function IsCompositionEnabled() As Boolean
    If Environment.OSVersion.Version.Major < 6 Then Return False
    Dim compositionEnabled As Integer = 0
    NativeMethods.DwmIsCompositionEnabled(compositionEnabled)
    Return CBool(compositionEnabled > 0)
  End Function
  Public Shared Sub DrawTextOnGlass(ByVal hwnd As IntPtr, ByVal text As String, ByVal Font As Font, ByVal ctlRct As Rectangle, ByVal Margin As Integer, ByVal iGlowSize As Integer)
    If IsCompositionEnabled() Then
      Dim TextRect As New NativeMethods.RECT
      TextRect.Top = Margin
      TextRect.Left = Margin
      TextRect.Bottom = ctlRct.Bottom - ctlRct.Top - Margin
      TextRect.Right = ctlRct.Right - ctlRct.Left - Margin
      Dim destdc As IntPtr = NativeMethods.GetDC(hwnd)
      Dim Memdc As IntPtr = NativeMethods.CreateCompatibleDC(destdc)
      Dim bitmap As IntPtr
      Dim bitmapOld As IntPtr = IntPtr.Zero
      Dim logfnotOld As IntPtr
      Dim uFormat As Integer = NativeMethods.DT_SINGLELINE Or NativeMethods.DT_NOPREFIX Or NativeMethods.DT_VCENTER
      Dim dib As New NativeMethods.BITMAPINFO()
      dib.bmiHeader.biHeight = -ctlRct.Height
      dib.bmiHeader.biWidth = ctlRct.Width
      dib.bmiHeader.biPlanes = 1
      dib.bmiHeader.biSize = Marshal.SizeOf(GetType(NativeMethods.BITMAPINFOHEADER))
      dib.bmiHeader.biBitCount = 32
      dib.bmiHeader.biCompression = NativeMethods.BI_RGB
      If Not (NativeMethods.SaveDC(Memdc) = 0) Then
        bitmap = NativeMethods.CreateDIBSection(Memdc, dib, NativeMethods.DIB_RGB_COLORS, IntPtr.Zero, IntPtr.Zero, 0)
        If Not (bitmap = IntPtr.Zero) Then
          bitmapOld = NativeMethods.SelectObject(Memdc, bitmap)
          Dim hFont As IntPtr = Font.ToHfont
          logfnotOld = NativeMethods.SelectObject(Memdc, hFont)
          Try
            Dim renderer As New System.Windows.Forms.VisualStyles.VisualStyleRenderer(System.Windows.Forms.VisualStyles.VisualStyleElement.Window.Caption.Active)
            Dim dttOpts As New NativeMethods.DTTOPTS()
            dttOpts.dwSize = CUInt(Marshal.SizeOf(GetType(NativeMethods.DTTOPTS)))
            dttOpts.dwFlags = NativeMethods.DTT_COMPOSITED Or NativeMethods.DTT_GLOWSIZE
            dttOpts.iGlowSize = iGlowSize
            NativeMethods.DrawThemeTextEx(renderer.Handle, Memdc, 0, 0, text, -1, uFormat, TextRect, dttOpts)
            NativeMethods.BitBlt(destdc, ctlRct.Left, ctlRct.Top, ctlRct.Width, ctlRct.Height, Memdc, 0, 0, NativeMethods.SRCCOPY)
          Catch e As Exception
            Trace.WriteLine(e.Message)
          Finally
            NativeMethods.SelectObject(Memdc, bitmapOld)
            NativeMethods.SelectObject(Memdc, logfnotOld)
            NativeMethods.ReleaseDC(Memdc, -1)
            NativeMethods.DeleteDC(Memdc)
            NativeMethods.ReleaseDC(destdc, -1)
            NativeMethods.DeleteDC(destdc)
            NativeMethods.DeleteObject(logfnotOld)
            NativeMethods.DeleteObject(bitmapOld)
            NativeMethods.DeleteObject(hFont)
            NativeMethods.DeleteObject(bitmap)
          End Try
        Else
          NativeMethods.DeleteObject(bitmap)
        End If
      Else
        NativeMethods.ReleaseDC(Memdc, -1)
        NativeMethods.DeleteDC(Memdc)
        NativeMethods.ReleaseDC(destdc, -1)
        NativeMethods.DeleteDC(destdc)
      End If
    End If
  End Sub
End Class
