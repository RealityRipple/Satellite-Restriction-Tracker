<ToolboxBitmap(GetType(System.Windows.Forms.ColorDialog))>
Public Class ColorDialog
  Inherits System.Windows.Forms.ColorDialog
  Private m_title As String = String.Empty
  Private titleSet As Boolean = False
  Public Property Title() As String
    Get
      Return m_title
    End Get
    Set(value As String)
      If Not String.IsNullOrEmpty(value) AndAlso Not value = m_title Then
        m_title = value
        titleSet = False
      End If
    End Set
  End Property
  <System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.Demand, Name:="FullTrust")>
  Protected Overrides Function HookProc(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    If msg = &HF Then
      If Not titleSet Then
        NativeMethods.SetWindowText(NativeMethods.GetAncestor(hWnd, 2), m_title)
        titleSet = True
      End If
    End If
    Return MyBase.HookProc(hWnd, msg, wParam, lParam)
  End Function
End Class
