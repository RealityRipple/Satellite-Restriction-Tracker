Public Class TaskbarNotifier
  Inherits System.Windows.Forms.Form
#Region "TaskbarNotifier Protected Members"
  Protected BackgroundBitmap As Bitmap = Nothing
  Protected CloseBitmap As Bitmap = Nothing
  Protected CloseBitmapLocation As Point
  Protected CloseBitmapSize As Size
  Protected RealTitleRectangle As Rectangle
  Protected RealContentRectangle As Rectangle
  Protected WorkAreaRectangle As Rectangle
  Protected timer As New Timer()
  Protected m_taskbarState As TaskbarStates = TaskbarStates.hidden
  Protected m_titleText As String
  Protected m_contentText As String
  Protected m_normalTitleColor As Color = Color.FromArgb(0, 0, 0)
  Protected m_hoverTitleColor As Color = Color.FromArgb(0, 0, &HFF)
  Protected m_normalContentColor As Color = Color.FromArgb(0, 0, 0)
  Protected m_hoverContentColor As Color = Color.FromArgb(0, 0, &HFF)
  Protected m_normalTitleFont As New Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel)
  Protected m_hoverTitleFont As New Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel)
  Protected m_normalContentFont As New Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Pixel)
  Protected m_hoverContentFont As New Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Pixel)
  Protected nShowEvents As Integer
  Protected nHideEvents As Integer
  Protected nVisibleEvents As Integer
  Protected nIncrementShow As Integer
  Protected nIncrementHide As Integer
  Protected bIsMouseOverPopup As Boolean = False
  Protected bIsMouseOverClose As Boolean = False
  Protected bIsMouseOverContent As Boolean = False
  Protected bIsMouseOverTitle As Boolean = False
  Protected bIsMouseDown As Boolean = False
  Protected bKeepVisibleOnMouseOver As Boolean = True
  Protected bReShowOnMouseOver As Boolean = True
#End Region
#Region "TaskbarNotifier Public Members"
  Public TitleRectangle As Rectangle
  Public ContentRectangle As Rectangle
  Public TitleClickable As Boolean = False
  Public ContentClickable As Boolean = True
  Public CloseClickable As Boolean = True
  Public EnableSelectionRectangle As Boolean = True
  Public Event CloseClick As EventHandler
  Public Event TitleClick As EventHandler
  Public Event ContentClick As EventHandler
#End Region
#Region "TaskbarNotifier Enums"
  Public Enum TaskbarStates
    hidden = 0
    appearing = 1
    visible = 2
    disappearing = 3
  End Enum
#End Region
#Region "TaskbarNotifier Constructor"
  Public Sub New()
    FormBorderStyle = FormBorderStyle.None
    WindowState = FormWindowState.Minimized
    MyBase.Show()
    MyBase.Hide()
    WindowState = FormWindowState.Normal
    ShowInTaskbar = False
    TopMost = True
    MaximizeBox = False
    MinimizeBox = False
    ControlBox = False
    timer.Enabled = True
    AddHandler timer.Tick, New EventHandler(AddressOf OnTimer)
  End Sub
#End Region
#Region "TaskbarNotifier Properties"
  Public ReadOnly Property TaskbarState() As TaskbarStates
    Get
      Return m_taskbarState
    End Get
  End Property
  Public Property TitleText() As String
    Get
      Return m_titleText
    End Get
    Set(value As String)
      m_titleText = value
      Refresh()
    End Set
  End Property
  Public Property ContentText() As String
    Get
      Return m_contentText
    End Get
    Set(value As String)
      m_contentText = value
      Refresh()
    End Set
  End Property
  Public Property NormalTitleColor() As Color
    Get
      Return m_normalTitleColor
    End Get
    Set(value As Color)
      m_normalTitleColor = value
      Refresh()
    End Set
  End Property
  Public Property HoverTitleColor() As Color
    Get
      Return m_hoverTitleColor
    End Get
    Set(value As Color)
      m_hoverTitleColor = value
      Refresh()
    End Set
  End Property
  Public Property NormalContentColor() As Color
    Get
      Return m_normalContentColor
    End Get
    Set(value As Color)
      m_normalContentColor = value
      Refresh()
    End Set
  End Property
  Public Property HoverContentColor() As Color
    Get
      Return m_hoverContentColor
    End Get
    Set(value As Color)
      m_hoverContentColor = value
      Refresh()
    End Set
  End Property
  Public Property NormalTitleFont() As Font
    Get
      Return m_normalTitleFont
    End Get
    Set(value As Font)
      m_normalTitleFont = value
      Refresh()
    End Set
  End Property
  Public Property HoverTitleFont() As Font
    Get
      Return m_hoverTitleFont
    End Get
    Set(value As Font)
      m_hoverTitleFont = value
      Refresh()
    End Set
  End Property
  Public Property NormalContentFont() As Font
    Get
      Return m_normalContentFont
    End Get
    Set(value As Font)
      m_normalContentFont = value
      Refresh()
    End Set
  End Property
  Public Property HoverContentFont() As Font
    Get
      Return m_hoverContentFont
    End Get
    Set(value As Font)
      m_hoverContentFont = value
      Refresh()
    End Set
  End Property
  Public Property KeepVisibleOnMousOver() As Boolean
    Get
      Return bKeepVisibleOnMouseOver
    End Get
    Set(value As Boolean)
      bKeepVisibleOnMouseOver = value
    End Set
  End Property
  Public Property ReShowOnMouseOver() As Boolean
    Get
      Return bReShowOnMouseOver
    End Get
    Set(value As Boolean)
      bReShowOnMouseOver = value
    End Set
  End Property
#End Region
#Region "TaskbarNotifier Public Methods"
  Public Overloads Sub Show(strTitle As String, strContent As String, nTimeToShow As Integer, nTimeToStay As Integer, nTimeToHide As Integer)
    Dim r As Rectangle = Nothing
    Dim XLoc As Integer = 0
    Dim YLoc As Integer = 0
    If TaskBarPosition.GetTaskBarPosition(r, Me.Handle) Then
      XLoc = r.X
      YLoc = r.Y
    End If
    WorkAreaRectangle = Screen.GetWorkingArea(New Point(XLoc, YLoc))
    XLoc = 0
    YLoc = 0
    m_titleText = strTitle
    m_contentText = strContent
    nVisibleEvents = nTimeToStay
    CalculateMouseRectangles()
    Dim nEvents As Integer
    If nTimeToShow > 10 Then
      nEvents = Math.Min((nTimeToShow \ 10), 100)
      nShowEvents = nTimeToShow \ nEvents
      nIncrementShow = 100 \ nEvents
    Else
      nShowEvents = 10
      nIncrementShow = 100
    End If
    If nTimeToHide > 10 Then
      nEvents = Math.Min((nTimeToHide \ 10), 100)
      nHideEvents = nTimeToHide \ nEvents
      nIncrementHide = 100 \ nEvents
    Else
      nHideEvents = 10
      nIncrementHide = 100
    End If
    XLoc = WorkAreaRectangle.Right - BackgroundBitmap.Width
    YLoc = WorkAreaRectangle.Bottom - BackgroundBitmap.Height - 1
    If Math.Abs(r.Height) = WorkAreaRectangle.Height Then
      If WorkAreaRectangle.Left >= r.Left Then XLoc = WorkAreaRectangle.Left
    ElseIf Math.Abs(r.Width) = WorkAreaRectangle.Width Then
      If WorkAreaRectangle.Top >= r.Top Then YLoc = WorkAreaRectangle.Top
    End If
    Select Case m_taskbarState
      Case TaskbarStates.hidden
        timer.[Stop]()
        m_taskbarState = TaskbarStates.appearing
        Opacity = 0
        SetBounds(XLoc, YLoc, BackgroundBitmap.Width, BackgroundBitmap.Height)
        timer.Interval = nShowEvents
        timer.Start()
        NativeMethods.ShowWindow(Me.Handle, 4)
      Case TaskbarStates.appearing
        Refresh()
      Case TaskbarStates.visible
        timer.[Stop]()
        If nVisibleEvents > 0 Then
          timer.Interval = nVisibleEvents
          timer.Start()
        End If
        Refresh()
      Case TaskbarStates.disappearing
        timer.[Stop]()
        m_taskbarState = TaskbarStates.visible
        Opacity = 1
        SetBounds(XLoc, YLoc, BackgroundBitmap.Width, BackgroundBitmap.Height)
        If nVisibleEvents > 0 Then
          timer.Interval = nVisibleEvents
          timer.Start()
        End If
        Refresh()
    End Select
  End Sub
  Public Shadows Sub Hide()
    If m_taskbarState <> TaskbarStates.hidden Then
      timer.[Stop]()
      m_taskbarState = TaskbarStates.hidden
      MyBase.Hide()
    End If
  End Sub
  Public Sub SlowHide()
    timer.[Stop]()
    timer.Interval = nHideEvents
    m_taskbarState = TaskbarStates.disappearing
    timer.Start()
  End Sub
  Public Sub SetBackgroundBitmap(strFilename As String, transparencyColor As Color)
    BackgroundBitmap = New Bitmap(strFilename)
    Width = BackgroundBitmap.Width
    Height = BackgroundBitmap.Height
    Region = BitmapToRegion(BackgroundBitmap, transparencyColor)
    If BackgroundBitmap Is Nothing Then SetDefaultBitmaps()
  End Sub
  Public Sub SetBackgroundBitmap(image As Image, transparencyColor As Color)
    BackgroundBitmap = New Bitmap(image)
    Width = BackgroundBitmap.Width
    Height = BackgroundBitmap.Height
    Region = BitmapToRegion(BackgroundBitmap, transparencyColor)
    If BackgroundBitmap Is Nothing Then SetDefaultBitmaps()
  End Sub
  Public Sub SetCloseBitmap(strFilename As String, transparencyColor As Color, position As Point)
    CloseBitmap = New Bitmap(strFilename)
    CloseBitmap.MakeTransparent(transparencyColor)
    CloseBitmapSize = New Size(CloseBitmap.Width \ 3, CloseBitmap.Height)
    CloseBitmapLocation = position
    If CloseBitmap Is Nothing Or CloseBitmapSize.IsEmpty Then SetDefaultBitmaps()
  End Sub
  Public Sub SetCloseBitmap(image As Image, transparencyColor As Color, position As Point)
    CloseBitmap = New Bitmap(image)
    CloseBitmap.MakeTransparent(transparencyColor)
    CloseBitmapSize = New Size(CloseBitmap.Width \ 3, CloseBitmap.Height)
    CloseBitmapLocation = position
    If CloseBitmap Is Nothing Or CloseBitmapSize.IsEmpty Then SetDefaultBitmaps()
  End Sub
  Private Sub SetDefaultBitmaps()
    SetDefaultBackgroundBitmap()
    SetDefaultCloseBitmap()
  End Sub
  Private Sub SetDefaultBackgroundBitmap()
    BackgroundBitmap = My.Resources.default_alert
    Width = BackgroundBitmap.Width
    Height = BackgroundBitmap.Height
    Region = BitmapToRegion(BackgroundBitmap, Color.Fuchsia)
    TitleRectangle = New Rectangle(7, 3, 188, 24)
    ContentRectangle = New Rectangle(9, 31, 227, 66)
  End Sub
  Private Sub SetDefaultCloseBitmap()
    CloseBitmap = My.Resources.default_close
    CloseBitmap.MakeTransparent(Color.Fuchsia)
    CloseBitmapSize = New Size(CloseBitmap.Width \ 3, CloseBitmap.Height)
    CloseBitmapLocation = New Point(190, 0)
  End Sub
#End Region
#Region "TaskbarNotifier Protected Methods"
  Protected Sub DrawCloseButton(grfx As Graphics)
    If CloseBitmap Is Nothing Or CloseBitmapSize.IsEmpty Then SetDefaultCloseBitmap()
    If CloseBitmap IsNot Nothing Then
      Dim rectDest As New Rectangle(CloseBitmapLocation, CloseBitmapSize)
      Dim rectSrc As Rectangle
      If bIsMouseOverClose Then
        If bIsMouseDown Then
          rectSrc = New Rectangle(New Point(CloseBitmapSize.Width * 2, 0), CloseBitmapSize)
        Else
          rectSrc = New Rectangle(New Point(CloseBitmapSize.Width, 0), CloseBitmapSize)
        End If
      Else
        rectSrc = New Rectangle(New Point(0, 0), CloseBitmapSize)
      End If
      grfx.DrawImage(CloseBitmap, rectDest, rectSrc, GraphicsUnit.Pixel)
    End If
  End Sub
  Protected Sub DrawText(grfx As Graphics)
    If m_titleText IsNot Nothing AndAlso m_titleText.Length <> 0 Then
      Dim sf As New StringFormat()
      sf.Alignment = StringAlignment.Near
      sf.LineAlignment = StringAlignment.Center
      sf.FormatFlags = StringFormatFlags.NoWrap
      sf.Trimming = StringTrimming.EllipsisCharacter
      If bIsMouseOverTitle Then
        grfx.DrawString(m_titleText, m_hoverTitleFont, New SolidBrush(m_hoverTitleColor), TitleRectangle, sf)
      Else
        grfx.DrawString(m_titleText, m_normalTitleFont, New SolidBrush(m_normalTitleColor), TitleRectangle, sf)
      End If
    End If
    If m_contentText IsNot Nothing AndAlso m_contentText.Length <> 0 Then
      Dim sf As New StringFormat()
      sf.Alignment = StringAlignment.Center
      sf.LineAlignment = StringAlignment.Center
      sf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces
      sf.Trimming = StringTrimming.Word
      If bIsMouseOverContent Then
        grfx.DrawString(m_contentText, m_hoverContentFont, New SolidBrush(m_hoverContentColor), ContentRectangle, sf)
        If EnableSelectionRectangle Then
          ControlPaint.DrawBorder3D(grfx, RealContentRectangle, Border3DStyle.Etched, Border3DSide.Top Or Border3DSide.Bottom Or Border3DSide.Left Or Border3DSide.Right)
        End If
      Else
        grfx.DrawString(m_contentText, m_normalContentFont, New SolidBrush(m_normalContentColor), ContentRectangle, sf)
      End If
    End If
  End Sub
  Protected Sub CalculateMouseRectangles()
    Dim grfx As Graphics = CreateGraphics()
    Dim sf As New StringFormat()
    sf.Alignment = StringAlignment.Center
    sf.LineAlignment = StringAlignment.Center
    sf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces
    Dim sizefTitle As SizeF = grfx.MeasureString(m_titleText, m_hoverTitleFont, TitleRectangle.Width, sf)
    Dim sizefContent As SizeF = grfx.MeasureString(m_contentText, m_hoverContentFont, ContentRectangle.Width, sf)
    grfx.Dispose()
    If sizefTitle.Height > TitleRectangle.Height Then
      RealTitleRectangle = New Rectangle(TitleRectangle.Left, TitleRectangle.Top, TitleRectangle.Width, TitleRectangle.Height)
    Else
      RealTitleRectangle = New Rectangle(TitleRectangle.Left, TitleRectangle.Top, CInt(Math.Truncate(sizefTitle.Width)), CInt(Math.Truncate(sizefTitle.Height)))
    End If
    RealTitleRectangle.Inflate(0, 2)
    If sizefContent.Height > ContentRectangle.Height Then
      RealContentRectangle = New Rectangle((ContentRectangle.Width - CInt(Math.Truncate(sizefContent.Width))) \ 2 + ContentRectangle.Left, ContentRectangle.Top, CInt(Math.Truncate(sizefContent.Width)), ContentRectangle.Height)
    Else
      RealContentRectangle = New Rectangle((ContentRectangle.Width - CInt(Math.Truncate(sizefContent.Width))) \ 2 + ContentRectangle.Left, (ContentRectangle.Height - CInt(Math.Truncate(sizefContent.Height))) \ 2 + ContentRectangle.Top, CInt(Math.Truncate(sizefContent.Width)), CInt(Math.Truncate(sizefContent.Height)))
    End If
    RealContentRectangle.Inflate(0, 2)
  End Sub
  Protected Function BitmapToRegion(bitmap As Bitmap, transparencyColor As Color) As Region
    If bitmap Is Nothing Then
      Throw New ArgumentNullException("Bitmap", "Bitmap cannot be null!")
    End If
    Dim height As Integer = bitmap.Height
    Dim width As Integer = bitmap.Width
    Dim path As New Drawing2D.GraphicsPath()
    For j As Integer = 0 To height - 1
      For i As Integer = 0 To width - 1
        If bitmap.GetPixel(i, j) = transparencyColor Then
          Continue For
        End If
        Dim x0 As Integer = i
        While (i < width) AndAlso (bitmap.GetPixel(i, j) <> transparencyColor)
          i += 1
        End While
        path.AddRectangle(New Rectangle(x0, j, i - x0, 1))
      Next
    Next
    Dim region As New Region(path)
    path.Dispose()
    Return region
  End Function
#End Region
#Region "TaskbarNotifier Events Overrides"
  Protected Sub OnTimer(obj As [Object], ea As EventArgs)
    Select Case m_taskbarState
      Case TaskbarStates.appearing
        If Opacity < 1 Then
          Opacity += nIncrementShow / 100
        Else
          timer.[Stop]()
          Opacity = 1
          If nVisibleEvents > 0 Then
            timer.Interval = nVisibleEvents
            m_taskbarState = TaskbarStates.visible
            timer.Start()
          End If
        End If
      Case TaskbarStates.visible
        timer.[Stop]()
        timer.Interval = nHideEvents
        If (bKeepVisibleOnMouseOver AndAlso Not bIsMouseOverPopup) OrElse (Not bKeepVisibleOnMouseOver) Then
          m_taskbarState = TaskbarStates.disappearing
        End If
        timer.Start()
      Case TaskbarStates.disappearing
        If bReShowOnMouseOver AndAlso bIsMouseOverPopup Then
          m_taskbarState = TaskbarStates.appearing
        Else
          If Opacity > 0 Then
            Opacity -= nIncrementHide / 100
          Else
            Hide()
          End If
        End If
    End Select
  End Sub
  Protected Overrides Sub OnMouseEnter(ea As EventArgs)
    MyBase.OnMouseEnter(ea)
    bIsMouseOverPopup = True
    Refresh()
  End Sub
  Protected Overrides Sub OnMouseLeave(ea As EventArgs)
    MyBase.OnMouseLeave(ea)
    bIsMouseOverPopup = False
    bIsMouseOverClose = False
    bIsMouseOverTitle = False
    bIsMouseOverContent = False
    Refresh()
  End Sub
  Protected Overrides Sub OnMouseMove(mea As MouseEventArgs)
    MyBase.OnMouseMove(mea)
    Dim bContentModified As Boolean = False
    If (mea.X > CloseBitmapLocation.X) AndAlso (mea.X < CloseBitmapLocation.X + CloseBitmapSize.Width) AndAlso (mea.Y > CloseBitmapLocation.Y) AndAlso (mea.Y < CloseBitmapLocation.Y + CloseBitmapSize.Height) AndAlso CloseClickable Then
      If Not bIsMouseOverClose Then
        bIsMouseOverClose = True
        bIsMouseOverTitle = False
        bIsMouseOverContent = False
        Cursor = Cursors.Hand
        bContentModified = True
      End If
    ElseIf RealContentRectangle.Contains(New Point(mea.X, mea.Y)) AndAlso ContentClickable Then
      If Not bIsMouseOverContent Then
        bIsMouseOverClose = False
        bIsMouseOverTitle = False
        bIsMouseOverContent = True
        Cursor = Cursors.Hand
        bContentModified = True
      End If
    ElseIf RealTitleRectangle.Contains(New Point(mea.X, mea.Y)) AndAlso TitleClickable Then
      If Not bIsMouseOverTitle Then
        bIsMouseOverClose = False
        bIsMouseOverTitle = True
        bIsMouseOverContent = False
        Cursor = Cursors.Hand
        bContentModified = True
      End If
    Else
      If bIsMouseOverClose OrElse bIsMouseOverTitle OrElse bIsMouseOverContent Then
        bContentModified = True
      End If
      bIsMouseOverClose = False
      bIsMouseOverTitle = False
      bIsMouseOverContent = False
      Cursor = Cursors.[Default]
    End If
    If bContentModified Then
      Refresh()
    End If
  End Sub
  Protected Overrides Sub WndProc(ByRef msg As System.Windows.Forms.Message)
    If msg.Msg = &H20 Then
      If MyBase.Cursor = Cursors.Hand Then
        NativeMethods.SetCursor(NativeMethods.LoadCursor(IntPtr.Zero, &H7F89))
        msg.Result = IntPtr.Zero
        Return
      End If
    End If
    MyBase.WndProc(msg)
  End Sub
  Protected Overrides Sub OnMouseDown(mea As MouseEventArgs)
    MyBase.OnMouseDown(mea)
    bIsMouseDown = True
    If bIsMouseOverClose Then
      Refresh()
    End If
  End Sub
  Protected Overrides Sub OnMouseUp(mea As MouseEventArgs)
    MyBase.OnMouseUp(mea)
    bIsMouseDown = False
    If bIsMouseOverClose Then
      bReShowOnMouseOver = False
      SlowHide()
      RaiseEvent CloseClick(Me, New EventArgs())
    ElseIf bIsMouseOverTitle Then
      RaiseEvent TitleClick(Me, New EventArgs())
    ElseIf bIsMouseOverContent Then
      RaiseEvent ContentClick(Me, New EventArgs())
    End If
  End Sub
  Protected Overrides Sub OnPaintBackground(pea As PaintEventArgs)
    Dim grfx As Graphics = pea.Graphics
    grfx.PageUnit = GraphicsUnit.Pixel
    Dim offScreenGraphics As Graphics
    Dim offscreenBitmap As Bitmap
    offscreenBitmap = New Bitmap(BackgroundBitmap.Width, BackgroundBitmap.Height)
    offScreenGraphics = Graphics.FromImage(offscreenBitmap)
    If BackgroundBitmap IsNot Nothing Then
      offScreenGraphics.DrawImage(BackgroundBitmap, 0, 0, BackgroundBitmap.Width, BackgroundBitmap.Height)
    End If
    DrawCloseButton(offScreenGraphics)
    DrawText(offScreenGraphics)
    grfx.DrawImage(offscreenBitmap, 0, 0)
  End Sub
#End Region
End Class
Public Class TaskBarPosition
  Shared Function GetTaskBarPosition(ByRef CoordinateRectangle As Rectangle, ByVal hwnd As IntPtr) As Boolean
    Try
      Dim abd As New NativeMethods.APPBARDATA
      abd.hwnd = hwnd
      NativeMethods.SHAppBarMessage(NativeMethods.ABM_GETTASKBARPOS, abd)
      CoordinateRectangle = New Rectangle(abd.rc.Left, abd.rc.Top, abd.rc.Right - abd.rc.Left, abd.rc.Top - abd.rc.Bottom)
      Return True
    Catch ex As Exception
      Return False
    End Try
  End Function
  Shared Function GetTaskBarEdge(ByVal hwnd As IntPtr) As NativeMethods.ABEdge
    Try
      Dim abd As New NativeMethods.APPBARDATA
      abd.hwnd = hwnd
      NativeMethods.SHAppBarMessage(NativeMethods.ABM_GETTASKBARPOS, abd)
      Return abd.uEdge
    Catch ex As Exception
      Return 0
    End Try
  End Function
End Class
