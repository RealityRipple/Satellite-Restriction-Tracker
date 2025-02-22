﻿Public NotInheritable Class frmMain
  Private Enum LoadStates
    Loading
    Loaded
    DB
  End Enum
  Private Delegate Sub ParamaterizedInvoker(parameter As Object)
  Private Delegate Sub ParamaterizedInvoker2(parameter As Object, secondParam As Object)
  Private WithEvents updateChecker As clsUpdate
  Private WithEvents taskNotifier As TaskbarNotifier
  Private WithEvents localData As Local.SiteConnection
#Region "Constants"
  Private Const sDISPLAY As String = "Usage Levels (%lt)"
  Private Const sDISPLAY_LT_NONE As String = "No History"
  Private Const sDISPLAY_LT_BUSY As String = "Please Wait"
  Private Const sDISPLAY_TT_NEXT As String = "Next Update in %t."
  Private Const sDISPLAY_TT_LATE As String = "Update Should've Happened %t Ago!"
  Private Const sDISPLAY_TT_ERR As String = "%e" & vbNewLine & "%m"
  Private Const sDISPLAY_TT_T_SOON As String = "a Moment"
  Private Const MBPerGB As Integer = 1000
#End Region
  Private myState As LoadStates = LoadStates.Loading
  Private sDisp As String = sDISPLAY.Replace("%lt", sDisp_LT)
  Private sDisp_LT As String = sDISPLAY_LT_NONE
  Private sDispTT As String = sDISPLAY_TT_NEXT.Replace("%t", sDisp_TT_T)
  Private sDisp_TT_M = sDISPLAY_TT_NEXT.Replace("%t", sDisp_TT_T)
  Private sDisp_TT_T As String = sDISPLAY_TT_T_SOON
  Private sDisp_TT_E As String = ""
  Private sEXEPath As String = IO.Path.Combine(LocalAppDataDirectory, "SRT_Setup.exe")
  Private animData As WindowAnimationData
  Private dwmComp As Boolean
  Private restoreMax As Boolean
  Private mySettings As AppSettings
  Private sAccount, sPassword As String
  Private imSlowed As Boolean
  Private imFree As Boolean
  Private FullCheck As Boolean = True
  Private NextGrabTick As Long
  Private GrabAttempt As Integer
  Private ClosingTime As Boolean
  Private sFailTray As String
  Private bAlert As TriState
  Private uCache_used, uCache_lim As Long
  Private lastBalloon As Long
  Private c_PauseActivity As String
  Private iconItem As Integer
  Private iconStop As Boolean
  Private iconBefore As Icon
  Public Property PauseActivity As String
    Get
      Return c_PauseActivity
    End Get
    Set(value As String)
      c_PauseActivity = value
    End Set
  End Property
#Region "Form Events"
  Private Sub frmMain_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    AddHandler Microsoft.Win32.SystemEvents.PowerModeChanged, AddressOf PowerModeChanged
    CheckComposition()
    If mySettings Is Nothing Then ReLoadSettings()
    NextGrabTick = Long.MinValue
    Me.Opacity = 0
    Me.ShowInTaskbar = False
    Me.Size = mySettings.MainSize
    ControlService(False)
    tmrIcoDelay.Interval = 100
    tmrIcoDelay.Enabled = True
    DisplayResults(0, 0)
    EnableProgressIcon()
  End Sub
  Private Sub tmrIcoDelay_Tick(sender As System.Object, e As System.EventArgs) Handles tmrIcoDelay.Tick
    tmrIcoDelay.Enabled = False
    Dim shellTrayWnd As IntPtr = NativeMethods.FindWindow("Shell_TrayWnd", String.Empty)
    If shellTrayWnd.Equals(IntPtr.Zero) Then
      tmrIcoDelay.Interval = 2000
      tmrIcoDelay.Enabled = True
    Else
      Dim trayNotifyWnd As IntPtr = NativeMethods.FindWindowEx(shellTrayWnd, IntPtr.Zero, "TrayNotifyWnd", String.Empty)
      If trayNotifyWnd.Equals(IntPtr.Zero) Then
        tmrIcoDelay.Interval = 1000
        tmrIcoDelay.Enabled = True
      Else
        Dim sysPagerWnd As IntPtr = NativeMethods.FindWindowEx(trayNotifyWnd, IntPtr.Zero, "SysPager", String.Empty)
        If sysPagerWnd.Equals(IntPtr.Zero) Then
          tmrIcoDelay.Interval = 500
          tmrIcoDelay.Enabled = True
        Else
          trayIcon.ContextMenu = mnuTray
          trayIcon.Icon = MakeIcon(IconName.norm)
          trayIcon.Text = Me.Text
          If mySettings.TrayIconStyle = AppSettings.TrayStyles.Always Then
            trayIcon.Visible = True
          ElseIf mySettings.TrayIconStyle = AppSettings.TrayStyles.Minimized Then
            If myState = LoadStates.Loaded Then
              trayIcon.Visible = (Me.WindowState = FormWindowState.Minimized) Or (Not Me.Visible)
            Else
              trayIcon.Visible = mySettings.AutoHide
            End If
          End If
        End If
      End If
    End If
  End Sub
  Private Sub frmMain_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
    If Not myState = LoadStates.Loaded Then
      InitAccount()
      If Not String.IsNullOrEmpty(sAccount) Then
        Me.ShowInTaskbar = True
        Me.Location = New Point((Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2)
        If mySettings.AutoHide Then
          If (Not mySettings.TrayIconStyle = AppSettings.TrayStyles.Never) Then
            Me.Hide()
          Else
            Me.Hide()
            Me.WindowState = FormWindowState.Minimized
            Me.Show()
          End If
          mnuRestore.Text = "&Restore"
        End If
        If Me.Opacity = 0 Then Me.Opacity = 1
        SetStatusText("Initializing", "Beginning application initialization process...", False)
        Dim initDBInvoker As New MethodInvoker(AddressOf initDB)
        initDBInvoker.BeginInvoke(Nothing, Nothing)
      Else
        mnuRestore.Text = "&Focus"
        iconStop = True
        trayIcon.Icon = MakeIcon(IconName.norm)
        Me.ShowInTaskbar = True
        If Me.Opacity = 0 Then Me.Opacity = 1
        SetTag(LoadStates.Loaded)
        If frmWizard.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
          ReLoadSettings()
          SetNextLoginTime()
          Dim ReInitInvoker As New MethodInvoker(AddressOf ReInit)
          ReInitInvoker.BeginInvoke(Nothing, Nothing)
        Else
          cmdConfig.Focus()
        End If
      End If
    End If
  End Sub
  Private Sub MinimizeWindow()
    If (mySettings IsNot Nothing) AndAlso (Not mySettings.TrayIconStyle = AppSettings.TrayStyles.Never) Then
      mnuRestore.Text = "&Restore"
      If CleanRendering() Then
        Me.Opacity = 0
        Me.WindowState = FormWindowState.Normal
        Me.Hide()
        Me.Opacity = 1
        If mySettings.TrayIconAnimation Then AnimateWindow(animData, True)
      Else
        Me.Hide()
      End If
      If mySettings.TrayIconStyle = AppSettings.TrayStyles.Minimized Then trayIcon.Visible = True
    Else
      Me.WindowState = FormWindowState.Minimized
    End If
  End Sub
  Private Sub RestoreWindow()
    If mySettings.TrayIconStyle = AppSettings.TrayStyles.Never Then
      If Me.WindowState = FormWindowState.Minimized Then Me.WindowState = FormWindowState.Normal
    ElseIf Not Me.Visible Then
      If CleanRendering() Then
        Me.Opacity = 0
        Me.Show()
        If Me.WindowState = FormWindowState.Minimized Then Me.WindowState = FormWindowState.Normal
        If restoreMax Then
          Me.WindowState = FormWindowState.Maximized
        Else
          Me.Location = New Point((Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2)
        End If
        If mySettings.TrayIconStyle = AppSettings.TrayStyles.Minimized Then trayIcon.Visible = False
        If mySettings.TrayIconAnimation And Not ClosingTime Then AnimateWindow(Me, False)
        Me.Opacity = 1
      Else
        Me.Show()
        If Me.WindowState = FormWindowState.Minimized Then Me.WindowState = FormWindowState.Normal
        If restoreMax Then
          Me.WindowState = FormWindowState.Maximized
        Else
          Me.Location = New Point((Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2)
        End If
        If mySettings.TrayIconStyle = AppSettings.TrayStyles.Minimized Then trayIcon.Visible = False
      End If
    End If
    mnuRestore.Text = "&Focus"
  End Sub
  Protected Overrides Sub OnHandleCreated(e As System.EventArgs)
    MyBase.OnHandleCreated(e)
    If mySettings Is Nothing Then
      ReLoadSettings()
    End If
    Dim hSysMenu As IntPtr = NativeMethods.GetSystemMenu(Me.Handle, False)
    Me.TopMost = mySettings.TopMost
    If Me.TopMost Then
      NativeMethods.InsertMenu(hSysMenu, 0, NativeMethods.MenuFlags.MF_STRING Or NativeMethods.MenuFlags.MF_CHECKED Or NativeMethods.MenuFlags.MF_BYPOSITION, TOPMOST_MENU_ID, TOPMOST_MENU_TEXT)
    Else
      NativeMethods.InsertMenu(hSysMenu, 0, NativeMethods.MenuFlags.MF_STRING Or NativeMethods.MenuFlags.MF_UNCHECKED Or NativeMethods.MenuFlags.MF_BYPOSITION, TOPMOST_MENU_ID, TOPMOST_MENU_TEXT)
    End If
    If mySettings.ScaleScreen Then
      NativeMethods.InsertMenu(hSysMenu, 1, NativeMethods.MenuFlags.MF_STRING Or NativeMethods.MenuFlags.MF_CHECKED Or NativeMethods.MenuFlags.MF_BYPOSITION, SCALE_MENU_ID, SCALE_MENU_TEXT)
    Else
      NativeMethods.InsertMenu(hSysMenu, 1, NativeMethods.MenuFlags.MF_STRING Or NativeMethods.MenuFlags.MF_UNCHECKED Or NativeMethods.MenuFlags.MF_BYPOSITION, SCALE_MENU_ID, SCALE_MENU_TEXT)
    End If
    NativeMethods.InsertMenu(hSysMenu, 2, NativeMethods.MenuFlags.MF_SEPARATOR Or NativeMethods.MenuFlags.MF_BYPOSITION, IntPtr.Zero, String.Empty)
  End Sub
  Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
    If mySettings Is Nothing Then
      MyBase.WndProc(m)
      Return
    End If
    Select Case m.Msg
      Case NativeMethods.WM_SYSCOMMAND
        Select Case m.WParam.ToInt64
          Case TOPMOST_MENU_ID
            Me.TopMost = Not Me.TopMost
            mySettings.TopMost = Me.TopMost
            Dim hSysMenu As IntPtr = NativeMethods.GetSystemMenu(Me.Handle, False)
            If Me.TopMost Then
              NativeMethods.ModifyMenu(hSysMenu, TOPMOST_MENU_ID, NativeMethods.MenuFlags.MF_STRING Or NativeMethods.MenuFlags.MF_CHECKED, TOPMOST_MENU_ID, TOPMOST_MENU_TEXT)
            Else
              NativeMethods.ModifyMenu(hSysMenu, TOPMOST_MENU_ID, NativeMethods.MenuFlags.MF_STRING Or NativeMethods.MenuFlags.MF_UNCHECKED, TOPMOST_MENU_ID, TOPMOST_MENU_TEXT)
            End If
          Case SCALE_MENU_ID
            mySettings.ScaleScreen = Not mySettings.ScaleScreen
            Dim sizeChangeInvoker As New EventHandler(AddressOf frmMain_SizeChanged)
            sizeChangeInvoker.BeginInvoke(Me, New EventArgs, Nothing, Nothing)
            Dim hSysMenu As IntPtr = NativeMethods.GetSystemMenu(Me.Handle, False)
            If mySettings.ScaleScreen Then
              NativeMethods.ModifyMenu(hSysMenu, SCALE_MENU_ID, NativeMethods.MenuFlags.MF_STRING Or NativeMethods.MenuFlags.MF_CHECKED, SCALE_MENU_ID, SCALE_MENU_TEXT)
            Else
              NativeMethods.ModifyMenu(hSysMenu, SCALE_MENU_ID, NativeMethods.MenuFlags.MF_STRING Or NativeMethods.MenuFlags.MF_UNCHECKED, SCALE_MENU_ID, SCALE_MENU_TEXT)
            End If
        End Select
      Case NativeMethods.WM_WINDOWPOSCHANGING
        If Not mySettings.TrayIconStyle = AppSettings.TrayStyles.Never Then
          Dim wndPos As NativeMethods.WINDOWPOS = m.GetLParam(GetType(NativeMethods.WINDOWPOS))
          If ((wndPos.Flags And NativeMethods.WINDOWPOS_FLAGS.SWP_STATECHANGED) = NativeMethods.WINDOWPOS_FLAGS.SWP_STATECHANGED) And (wndPos.X = -32000) And (wndPos.Y = -32000) Then
            restoreMax = Me.WindowState = FormWindowState.Maximized
            If mySettings.TrayIconAnimation Then animData = GetWindowAnimationData(Me)
          End If
        End If
      Case NativeMethods.WM_DWMCOMPOSITIONCHANGED
        CheckComposition()
    End Select
    MyBase.WndProc(m)
  End Sub
  Private Sub frmMain_SizeChanged(sender As Object, e As System.EventArgs) Handles Me.SizeChanged
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(AddressOf frmMain_SizeChanged), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    If (Me.WindowState = FormWindowState.Minimized) AndAlso (mySettings IsNot Nothing) AndAlso (Not mySettings.TrayIconStyle = AppSettings.TrayStyles.Never) Then
      MinimizeWindow()
      Return
    End If
    If mySettings Is Nothing Then
      ReLoadSettings()
    End If
    Static fRatio As Single
    If fRatio = 0.0! Or Single.IsInfinity(fRatio) Or Single.IsNaN(fRatio) Then
      Dim icoSize As Integer = NativeMethods.GetSystemMetrics(NativeMethods.MetricsList.SM_CXSMSIZE)
      fRatio = Me.Font.SizeInPoints / (icoSize * 12.5)
    End If
    Static fMin As Single
    If fMin = 0.0! Or Single.IsInfinity(fMin) Or Single.IsNaN(fMin) Then fMin = Me.Font.SizeInPoints
    If mySettings.ScaleScreen Then
      Dim fontSize As Single = fMin
      If (Me.Width / 2) < Me.Height Then
        If (Me.Width / 2) * fRatio > fMin Then
          fontSize = (Me.Width / 2) * fRatio
        Else
          fontSize = fMin
        End If
      Else
        If Me.Height * fRatio > fMin Then
          fontSize = Me.Height * fRatio
        Else
          fontSize = fMin
        End If
      End If
      pnlDetails.Font = New Font(Me.Font.FontFamily, fontSize, Me.Font.Style, Me.Font.Unit, Me.Font.GdiCharSet, Me.Font.GdiVerticalFont)
    Else
      pnlDetails.Font = Me.Font
    End If
    ResizePanels()
    If myState = LoadStates.Loaded Then
      If Me.WindowState = FormWindowState.Normal Then mySettings.MainSize = Me.Size
    Else
      lblRRS.Font = pnlDetails.Font
      lblNothing.Font = New Font(Me.Font.FontFamily, pnlDetails.Font.Size * 2.5, Me.Font.Style, Me.Font.Unit, Me.Font.GdiCharSet, Me.Font.GdiVerticalFont)
    End If
    For i As Integer = 1 To 2
      If (lblStatus.Height / 2) - (pctNetTest.Height / 2) > 0 Then
        pctNetTest.Top = (lblStatus.Height / 2) - (pctNetTest.Height / 2)
      Else
        pctNetTest.Top = 0
      End If
      If pnlUsage.Visible Then
        If pctNetTest.Bottom > pnlUsage.Top - 1 Then
          pctNetTest.Height = pnlUsage.Top - 1 - pctNetTest.Top
          pctNetTest.Width = pctNetTest.Height
        Else
          pctNetTest.Height = 16
          pctNetTest.Width = pctNetTest.Height
        End If
      End If
      pctNetTest.Left = gbUsage.Right - 16 - pctNetTest.Width
    Next
  End Sub
  Private Sub ResizePanels()
    Dim trayIcoVal As Icon = Nothing
    If uCache_lim = 0 Then
      pctUsage.Image = DisplayRProgress(pctUsage.DisplayRectangle.Size, 0, 1, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
      trayIcoVal = MakeIcon(IconName.norm)
    Else
      pctUsage.Image = DisplayRProgress(pctUsage.DisplayRectangle.Size, uCache_used, uCache_lim, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
      trayIcoVal = CreateUsageTrayIcon(uCache_used, uCache_lim)
    End If
    If trayIcoVal IsNot Nothing Then
      If tmrIcon.Enabled Then
        iconBefore = trayIcoVal.Clone
      Else
        trayIcon.Icon = trayIcoVal.Clone
      End If
      trayIcoVal.Dispose()
      trayIcoVal = Nothing
    End If
  End Sub
  Private Sub frmMain_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    If e.CloseReason = CloseReason.UserClosing And mySettings.TrayIconOnClose Then
      If Me.Visible And Not Me.WindowState = FormWindowState.Minimized Then
        If mySettings.TrayIconAnimation Then animData = GetWindowAnimationData(Me)
        MinimizeWindow()
        e.Cancel = True
        Return
      End If
    End If
    ClosingTime = True
    tmrUpdate.Stop()
    If localData IsNot Nothing Then
      localData.Dispose()
      localData = Nothing
    End If
    StopSong()
    mySettings.Save()
    LOG_Terminate(False)
    trayIcon.Visible = False
    If mySettings.Service Then
      Dim cSave As New SvcSettings
      cSave.Account = mySettings.Account
      cSave.Interval = mySettings.Interval
      If Not String.IsNullOrEmpty(mySettings.PassCrypt) Then
        Dim newKey() As Byte = StoredPassword.GenerateKey()
        Dim newSalt() As Byte = StoredPassword.GenerateSalt()
        If String.IsNullOrEmpty(mySettings.PassKey) Or String.IsNullOrEmpty(mySettings.PassSalt) Then
          cSave.PassCrypt = StoredPassword.Encrypt(StoredPasswordLegacy.DecryptApp(mySettings.PassCrypt), newKey, newSalt)
        Else
          cSave.PassCrypt = StoredPassword.Encrypt(StoredPassword.Decrypt(mySettings.PassCrypt, mySettings.PassKey, mySettings.PassSalt), newKey, newSalt)
        End If
        cSave.PassKey = Convert.ToBase64String(newKey)
        cSave.PassSalt = Convert.ToBase64String(newSalt)
      End If
      cSave.Proxy = mySettings.Proxy
      cSave.Timeout = mySettings.Timeout
      cSave.Save()
    End If
    If My.Computer.FileSystem.FileExists(IO.Path.Combine(AppDataPath, "del.bat")) Then My.Computer.FileSystem.DeleteFile(IO.Path.Combine(AppDataPath, "del.bat"))
    Select Case e.CloseReason
      Case CloseReason.WindowsShutDown
      Case CloseReason.ApplicationExitCall
      Case Else : ControlService(True)
    End Select
  End Sub
  Private Sub lblRRS_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblRRS.LinkClicked
    OpenURL("realityripple.com", taskNotifier)
  End Sub
  Private Sub pctNetTest_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles pctNetTest.KeyUp
    If e.KeyCode = Keys.Space Or e.KeyCode = Keys.Return Then OpenURL(mySettings.NetTestURL, taskNotifier)
  End Sub
  Private Sub pctNetTest_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctNetTest.MouseClick
    If e.Button = Windows.Forms.MouseButtons.Left Then OpenURL(mySettings.NetTestURL, taskNotifier)
  End Sub
  Protected Overrides Function ProcessKeyPreview(ByRef m As System.Windows.Forms.Message) As Boolean
    Static bDown As Boolean
    Select Case m.Msg
      Case &H100, &H104
        Dim iKey As Integer = m.WParam.ToInt32
        Dim Key As Keys = iKey
        bDown = (Key = Keys.F1)
      Case &H101, &H105
        Dim iKey As Integer = m.WParam.ToInt32
        Dim Key As Keys = iKey
        If bDown And (Key = Keys.F1) Then OpenURL("http://srt.realityripple.com/faq.php", taskNotifier)
    End Select
    Return MyBase.ProcessKeyPreview(m)
  End Function
#End Region
#Region "Initialization Functions"
  Friend Sub ReLoadSettings()
    If mySettings IsNot Nothing Then mySettings = Nothing
    mySettings = New AppSettings
    If Not mySettings.TLSProxy Then
      Dim useProtocol As SecurityProtocolTypeEx = SecurityProtocolTypeEx.None
      For Each protocolTest In [Enum].GetValues(GetType(SecurityProtocolTypeEx))
        If (mySettings.SecurityProtocol And protocolTest) = protocolTest Then
          Try
            Net.ServicePointManager.SecurityProtocol = protocolTest
            useProtocol = useProtocol Or protocolTest
          Catch ex As Exception
          End Try
        End If
      Next
      If useProtocol = SecurityProtocolTypeEx.None Then
        For Each protocolTest In [Enum].GetValues(GetType(SecurityProtocolTypeEx))
          Try
            Net.ServicePointManager.SecurityProtocol = protocolTest
            useProtocol = useProtocol Or protocolTest
          Catch ex As Exception
          End Try
        Next
        Try
          Net.ServicePointManager.SecurityProtocol = useProtocol
          mySettings.SecurityProtocol = useProtocol
          mySettings.Save()
        Catch ex As Exception
        End Try
      Else
        Try
          Net.ServicePointManager.SecurityProtocol = useProtocol
        Catch ex As Exception
        End Try
      End If
    End If
    If LocalAppDataDirectory = IO.Path.Combine(Application.StartupPath, "Config") Then mySettings.HistoryDir = IO.Path.Combine(Application.StartupPath, "Config")
    ScreenDefaultColors(mySettings.Colors)
    NOTIFIER_STYLE = LoadAlertStyle(mySettings.AlertStyle)
    Dim hSysMenu As IntPtr = NativeMethods.GetSystemMenu(Me.Handle, False)
    If mySettings.TopMost Then
      NativeMethods.ModifyMenu(hSysMenu, TOPMOST_MENU_ID, NativeMethods.MenuFlags.MF_STRING Or NativeMethods.MenuFlags.MF_CHECKED, TOPMOST_MENU_ID, TOPMOST_MENU_TEXT)
    Else
      NativeMethods.ModifyMenu(hSysMenu, TOPMOST_MENU_ID, NativeMethods.MenuFlags.MF_STRING Or NativeMethods.MenuFlags.MF_UNCHECKED, TOPMOST_MENU_ID, TOPMOST_MENU_TEXT)
    End If
    If mySettings.ScaleScreen Then
      NativeMethods.ModifyMenu(hSysMenu, SCALE_MENU_ID, NativeMethods.MenuFlags.MF_STRING Or NativeMethods.MenuFlags.MF_CHECKED, SCALE_MENU_ID, SCALE_MENU_TEXT)
    Else
      NativeMethods.ModifyMenu(hSysMenu, SCALE_MENU_ID, NativeMethods.MenuFlags.MF_STRING Or NativeMethods.MenuFlags.MF_UNCHECKED, SCALE_MENU_ID, SCALE_MENU_TEXT)
    End If
    If mySettings.TrayIconStyle = AppSettings.TrayStyles.Always Then
      trayIcon.Visible = True
    ElseIf mySettings.TrayIconStyle = AppSettings.TrayStyles.Minimized Then
      trayIcon.Visible = (Me.WindowState = FormWindowState.Minimized) Or (Not Me.Visible)
    Else
      trayIcon.Visible = False
    End If
    If String.IsNullOrEmpty(mySettings.NetTestURL) Then
      pctNetTest.Visible = False
      pctNetTest.Cursor = Cursors.Default
    Else
      pctNetTest.Visible = True
      pctNetTest.Cursor = Cursors.Hand
      Dim sNetTestIco As String = IO.Path.Combine(LocalAppDataDirectory, "netTest.png")
      If IO.File.Exists(sNetTestIco) Then
        Using imgNetDrawn As New Bitmap(16, 16)
          Using g As Graphics = Graphics.FromImage(imgNetDrawn)
            Using imgNetTest As Image = Image.FromFile(sNetTestIco)
              g.DrawImage(imgNetTest, 0, 0)
            End Using
            pctNetTest.Image = imgNetDrawn.Clone
          End Using
        End Using
      Else
        pctNetTest.Image = My.Resources.throbber
        Dim wsFavicon As New clsFavicon(AddressOf wsFavicon_DownloadIconCompleted)
        wsFavicon.Start(mySettings.NetTestURL, mySettings.NetTestURL)
      End If
      Dim sNetTestTitle As String = mySettings.NetTestURL
      If sNetTestTitle.Contains(Uri.SchemeDelimiter) Then sNetTestTitle = sNetTestTitle.Substring(sNetTestTitle.IndexOf(Uri.SchemeDelimiter) + Uri.SchemeDelimiter.Length)
      If sNetTestTitle.StartsWith("www.") Then sNetTestTitle = sNetTestTitle.Substring(4)
      If sNetTestTitle.Contains("/") Then sNetTestTitle = sNetTestTitle.Substring(0, sNetTestTitle.IndexOf("/"))
      If sNetTestTitle = "192.168.100.1" Or CompareImages(pctNetTest.Image, My.Resources.modem16) Then sNetTestTitle = "ViaSat Modem"
      ttUI.SetToolTip(pctNetTest, "Visit " & sNetTestTitle & ".")
    End If
  End Sub
  Private Sub ReInit()
    If localData IsNot Nothing Then
      localData.Dispose()
      localData = Nothing
    End If
    InitAccount()
    If Not String.IsNullOrEmpty(sAccount) Then
      EnableProgressIcon()
      SetStatusText("Reloading", "Reloading History...", False)
      LOG_Initialize(sAccount, False)
      If ClosingTime Then Return
      DisplayUsage(False, False)
      SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Preparing Connection...", False)
      Dim UsageInvoker As New MethodInvoker(AddressOf GetUsage)
      UsageInvoker.BeginInvoke(Nothing, Nothing)
    End If
    If ClosingTime Then Return
    Dim sizeChangeInvoker As New EventHandler(AddressOf frmMain_SizeChanged)
    sizeChangeInvoker.BeginInvoke(Me, New EventArgs, Nothing, Nothing)
  End Sub
  Private Sub EnableProgressIcon()
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New MethodInvoker(AddressOf EnableProgressIcon))
      Catch ex As Exception
      End Try
      Return
    End If
    Try
      If ClosingTime Then Return
      iconBefore = trayIcon.Icon
      iconStop = False
      iconItem = 0
      tmrIcon.Enabled = True
    Catch ex As Exception

    End Try
  End Sub
  Private Sub StartTimer()
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New MethodInvoker(AddressOf StartTimer))
      Catch ex As Exception
      End Try
      Return
    End If
    NextGrabTick = Long.MinValue
    SetTag(LoadStates.Loaded)
  End Sub
  Private Sub SetTag(Tag As LoadStates)
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New ParamaterizedInvoker(AddressOf SetTag), Tag)
      Catch ex As Exception
      End Try
      Return
    End If
    myState = Tag
  End Sub
  Private Sub initDB()
    SetTag(LoadStates.DB)
    SetStatusText("Loading History", "Reading usage history into memory...", False)
    LOG_Initialize(sAccount, False)
    If ClosingTime Then Return
    iconStop = True
    SetStatusText("No History", String.Empty, False)
    DisplayUsage(True, False)
    Dim TimerInvoker As New MethodInvoker(AddressOf StartTimer)
    TimerInvoker.BeginInvoke(Nothing, Nothing)
  End Sub
  Private Sub InitAccount()
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New MethodInvoker(AddressOf InitAccount))
      Catch ex As Exception
      End Try
      Return
    End If
    sAccount = mySettings.Account
    If Not String.IsNullOrEmpty(mySettings.PassCrypt) Then
      If String.IsNullOrEmpty(mySettings.PassKey) Or String.IsNullOrEmpty(mySettings.PassSalt) Then
        sPassword = StoredPasswordLegacy.DecryptApp(mySettings.PassCrypt)
      Else
        sPassword = StoredPassword.Decrypt(mySettings.PassCrypt, mySettings.PassKey, mySettings.PassSalt)
      End If
    End If
  End Sub
#End Region
#Region "Login Functions"
  Private Sub tmrUpdate_Tick(sender As System.Object, e As System.EventArgs) Handles tmrUpdate.Tick
    If Not NextGrabTick = Long.MaxValue Then
      Dim msInterval As Long = mySettings.Interval * 60 * 1000
      If NextGrabTick = Long.MinValue Then
        Dim minutesSinceLast As Long = DateDiff(DateInterval.Minute, LOG_GetLast, Now)
        If minutesSinceLast < mySettings.Interval Then
          Dim msSinceLast As Long = minutesSinceLast * 60 * 1000
          NextGrabTick = srlFunctions.TickCount() + (msInterval - msSinceLast) + (2 * 60 * 1000)
        Else
          NextGrabTick = srlFunctions.TickCount()
        End If
        If NextGrabTick - srlFunctions.TickCount() < mySettings.StartWait * 60 * 1000 Then NextGrabTick = srlFunctions.TickCount() + (mySettings.StartWait * 60 * 1000)
      End If
      If srlFunctions.TickCount() >= NextGrabTick Then
        If Not mySettings.UpdateType = AppSettings.UpdateTypes.None AndAlso Math.Abs(DateDiff(DateInterval.Day, mySettings.LastUpdate, Now)) > mySettings.UpdateTime Then
          CheckForUpdates()
        Else
          If Not String.IsNullOrEmpty(sAccount) Then
            If Math.Abs(DateDiff(DateInterval.Minute, LOG_GetLast, Now)) >= 10 Then
              If Not String.IsNullOrEmpty(sPassword) Then
                NextGrabTick = Long.MaxValue
                PauseActivity = "Preparing Connection"
                EnableProgressIcon()
                SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Preparing Connection...", False)
                Dim UsageInvoker As New MethodInvoker(AddressOf GetUsage)
                UsageInvoker.BeginInvoke(Nothing, Nothing)
                Return
              End If
            End If
          End If
          NextGrabTick = srlFunctions.TickCount() + msInterval
        End If
      End If
    End If
  End Sub
  Private Sub GetUsage()
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New MethodInvoker(AddressOf GetUsage))
      Catch ex As Exception
      End Try
      Return
    End If
    If String.IsNullOrEmpty(sAccount) Or String.IsNullOrEmpty(sPassword) Then
      RestoreWindow()
      cmdConfig.Focus()
      MsgDlg(Me, "Please enter your account details in the Config window by clicking Configuration.", "You haven't entered your account details.", "Account Details Required", MessageBoxButtons.OK, _TaskDialogIcon.User, MessageBoxIcon.Warning)
    Else
      If cmdRefresh.Enabled Then
        cmdRefresh.Enabled = False
        NextGrabTick = srlFunctions.TickCount() + (mySettings.Interval * 60 * 1000)
        If localData IsNot Nothing Then
          localData.Dispose()
          localData = Nothing
        End If
        GrabAttempt = 0
        localData = New Local.SiteConnection(LocalAppDataDirectory)
      Else
        If localData IsNot Nothing Then
          localData.Dispose()
          localData = Nothing
        End If
        Application.DoEvents()
        SetStatusText(srlFunctions.TimeToString(LOG_GetLast), String.Empty, False)
        DisplayUsage(False, False)
        NextGrabTick = srlFunctions.TickCount() + 5000
      End If
    End If
  End Sub
  Private Shared Function KeyCheck(TestKey As String) As Boolean
    If String.IsNullOrWhiteSpace(TestKey) Then Return False
    If TestKey.Contains("-") Then
      Dim sKeys() As String = Split(TestKey, "-")
      If sKeys.Length = 5 Then
        If Trim(sKeys(0)).Length = 6 And Trim(sKeys(1)).Length = 4 And Trim(sKeys(2)).Length = 4 Or Trim(sKeys(3)).Length = 4 Or Trim(sKeys(4)).Length = 6 Then
          Return True
        End If
      End If
    End If
    Return False
  End Function
  Private Sub DisplayUsage(bStatusText As Boolean, bHardTime As Boolean)
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New ParamaterizedInvoker2(AddressOf DisplayUsage), bStatusText, bHardTime)
      Catch ex As Exception
      End Try
      Return
    End If
    If Not cmdRefresh.Enabled Then cmdRefresh.Enabled = True
    If tmrIcon.Enabled Then iconStop = True
    If bHardTime Then
      NextGrabTick = srlFunctions.TickCount() + (mySettings.Interval * 60 * 1000)
    Else
      NextGrabTick = Long.MinValue
    End If
    If LOG_GetCount() > 0 Then
      Dim dtDate As Date
      Dim lUsed As Long
      Dim lLimit As Long
      LOG_Get(LOG_GetCount() - 1, dtDate, lused, lLimit)
      If bStatusText Then SetStatusText(srlFunctions.TimeToString(dtDate), String.Empty, False)
      DisplayResults(lUsed, lLimit)
    End If
  End Sub
  Private Sub SetNextLoginTime(Optional MinutesAhead As Integer = -1)
    If localData IsNot Nothing Then
      localData.Dispose()
      localData = Nothing
    End If
    If MinutesAhead > -1 Then
      NextGrabTick = srlFunctions.TickCount() + (MinutesAhead * 60 * 1000)
    Else
      NextGrabTick = srlFunctions.TickCount() + (mySettings.Interval * 60 * 1000)
    End If
  End Sub
#End Region
#Region "Local Usage Events"
  Private Sub localData_ConnectionStatus(sender As Object, e As Local.SiteConnectionStatusEventArgs) Handles localData.ConnectionStatus
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(Of Local.SiteConnectionStatusEventArgs)(AddressOf localData_ConnectionStatus), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    NextGrabTick = srlFunctions.TickCount() + ((mySettings.Timeout + 15) * 1000)
    Dim sAppend As String = ""
    If e.Attempt > 0 Then
      If e.Stage > 0 Then
        sAppend = " (Stage " & (e.Stage + 1) & ", Redirect #" & e.Attempt & ")"
      Else
        sAppend = " (Redirect #" & e.Attempt & ")"
      End If
    ElseIf e.Stage > 0 Then
      sAppend = " (Stage " & (e.Stage + 1) & ")"
    End If
    Select Case e.Status
      Case Local.SiteConnectionStates.Initialize : SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Initializing Connection" & sAppend & "...", False)
      Case Local.SiteConnectionStates.Prepare : SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Preparing to Log In" & sAppend & "...", False)
      Case Local.SiteConnectionStates.Login
        Select Case e.SubState
          Case Local.SiteConnectionSubStates.ReadLogin : SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Reading Login Page" & sAppend & "...", False)
          Case Local.SiteConnectionSubStates.Authenticate : SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Authenticating" & sAppend & "...", False)
          Case Else : SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Logging In" & sAppend & "...", False)
        End Select
      Case Local.SiteConnectionStates.TableDownload
        Select Case e.SubState
          Case Local.SiteConnectionSubStates.LoadHome : SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Downloading Home Page" & sAppend & "...", False)
          Case Local.SiteConnectionSubStates.LoadTable : SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Downloading Usage Table" & sAppend & "...", False)
          Case Else : SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Downloading Usage Table" & sAppend & "...", False)
        End Select
      Case Local.SiteConnectionStates.TableRead : SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Reading Usage Table" & sAppend & "...", False)
    End Select
  End Sub
  Private Sub localData_ConnectionFailure(sender As Object, e As Local.SiteConnectionFailureEventArgs) Handles localData.ConnectionFailure
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(Of Local.SiteConnectionFailureEventArgs)(AddressOf localData_ConnectionFailure), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    Select Case e.Type
      Case Local.SiteConnectionFailureType.LoginIssue
        GrabAttempt = 0
        SetStatusText(srlFunctions.TimeToString(LOG_GetLast), e.Message, True)
        DisplayUsage(False, False)
        Return
      Case Local.SiteConnectionFailureType.ConnectionTimeout
        If GrabAttempt < mySettings.Retries Then
          GrabAttempt += 1
          Dim sMessage As String = "Connection Timed Out! Retry " & GrabAttempt & " of " & mySettings.Retries & "..."
          SetStatusText(srlFunctions.TimeToString(LOG_GetLast), sMessage, True)
          If localData IsNot Nothing Then
            localData.Dispose()
            localData = Nothing
          End If
          Dim waitThreeSeconds As Long = srlFunctions.TickCount + 3000
          Do Until srlFunctions.TickCount >= waitThreeSeconds
            Application.DoEvents()
            Threading.Thread.Sleep(1)
            Threading.Thread.Sleep(0)
          Loop
          localData = New Local.SiteConnection(LocalAppDataDirectory)
          Return
        End If
        SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Connection Timed Out!", True)
        DisplayUsage(False, False)
      Case Local.SiteConnectionFailureType.TLSTooOld
        GrabAttempt = 0
        If mySettings.TLSProxy Then
          SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Please enable TLS 1.1 or 1.2 under Security Protocol in the Network tab of the Config window to connect.", True)
          DisplayUsage(False, False)
        Else
          If (Environment.OSVersion.Version.Major < 6 Or (Environment.OSVersion.Version.Major = 6 And Environment.OSVersion.Version.Minor = 0)) Then
            SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Security Protocol not supported on this Operating System. Please use the TLS Proxy feature under Security Protocol in the Network tab of the Config window to connect.", True)
            DisplayUsage(False, False)
          ElseIf (Environment.Version.Major = 4 And Environment.Version.Minor = 0 And Environment.Version.Build = 30319 And Environment.Version.Revision < 17929) Then
            SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Security Protocol requires .NET Framework 4.5 or newer. Please update your .NET Framework version through Windows Updates or the Microsoft website. You can use the TLS Proxy feature under Security Protocol in the Network tab of the Config window to bypass this problem for now.", True)
            DisplayUsage(False, False)
          Else
            If e.Message = "VER" Then
              SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Please enable TLS 1.1 or 1.2 under Security Protocol in the Network tab of the Config window to connect.", True)
              DisplayUsage(False, False)
            ElseIf e.Message = "PROXY" Then
              SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Even though TLS 1.1 or 1.2 was enabled, the server still didin't like the request. Please let me know you got this message. You can use the TLS Proxy feature under Security Protocol in the Network tab of the Config window to bypass this problem for now.", True)
              DisplayUsage(False, False)
            End If
          End If
        End If
      Case Local.SiteConnectionFailureType.LoginFailure
        If e.Message.EndsWith("Please try again.") And GrabAttempt < mySettings.Retries Then
          GrabAttempt += 1
          Dim sMessage As String = e.Message.Substring(0, e.Message.IndexOf("Please try again.")) & "Retry " & GrabAttempt & " of " & mySettings.Retries & "..."
          SetStatusText(srlFunctions.TimeToString(LOG_GetLast), sMessage, True)
          If localData IsNot Nothing Then
            localData.Dispose()
            localData = Nothing
          End If
          Dim waitThreeSeconds As Long = srlFunctions.TickCount + 3000
          Do Until srlFunctions.TickCount >= waitThreeSeconds
            Application.DoEvents()
            Threading.Thread.Sleep(1)
            Threading.Thread.Sleep(0)
          Loop
          localData = New Local.SiteConnection(LocalAppDataDirectory)
          Return
        End If
        SetStatusText(srlFunctions.TimeToString(LOG_GetLast), e.Message, True)
        DisplayUsage(False, True)
      Case Local.SiteConnectionFailureType.UnknownAccountDetails
        GrabAttempt = 0
        SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Please enter your account details in the Config window.", True)
        DisplayUsage(False, False)
        RestoreWindow()
        cmdConfig.Focus()
        MsgDlg(Me, "Please enter your account details in the Config window by clicking Configuration.", "You haven't entered your account details.", "Account Details Required", MessageBoxButtons.OK, _TaskDialogIcon.User, MessageBoxIcon.Error)
    End Select
    If localData IsNot Nothing Then
      localData.Dispose()
      localData = Nothing
    End If
  End Sub
  Private Sub localData_ConnectionResult(sender As Object, e As Local.SiteResultEventArgs) Handles localData.ConnectionResult
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(Of Local.SiteResultEventArgs)(AddressOf localData_ConnectionResult), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    GrabAttempt = 0
    SetStatusText(srlFunctions.TimeToString(e.Update), "Saving History...", False)
    NextGrabTick = srlFunctions.TickCount() + (mySettings.Interval * 60 * 1000)
    LOG_Add(e.Update, e.Used, e.Limit, True)
    mySettings.Save()
    ScreenDefaultColors(mySettings.Colors)
    If e.SlowedDetected Then imSlowed = True
    imFree = e.FreeDetected
    DisplayUsage(True, True)
    If localData IsNot Nothing Then
      localData.Dispose()
      localData = Nothing
    End If
  End Sub
#End Region
#Region "Graphs"
  Const ttLimit As Integer = 127
  Private tmrChanges As Threading.Timer
  Private Sub DisplayChangeInterval(state As Object)
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New ParamaterizedInvoker(AddressOf DisplayChangeInterval), state)
      Catch ex As Exception
      End Try
      Return
    End If
    If tmrChanges IsNot Nothing Then
      tmrChanges.Dispose()
      tmrChanges = Nothing
    Else
      Return
    End If
    Dim hadChange As Boolean = True
    Dim lUsed As Long = uCache_used
    Dim lLim As Long = uCache_lim
    Dim lFree As Long = lLim - lUsed
    If lUsed <> 0 Or uCache_lim > 0 Or lFree <> 0 Then
      DoChange(lblUsageUsedVal, lUsed)
      DoChange(lblUsageFreeVal, lFree)
      DoChange(lblUsageLimitVal, lLim)
      If lUsed = 0 And lFree = 0 And lLim = 0 Then hadChange = False
    End If
    ResizePanels()
    If tmrChanges IsNot Nothing Then
      tmrChanges.Dispose()
      tmrChanges = Nothing
    End If
    If hadChange Then tmrChanges = New Threading.Timer(New Threading.TimerCallback(AddressOf DisplayChangeInterval), state, 25, System.Threading.Timeout.Infinite)
  End Sub
  Private Shared Sub DoChange(ByRef lblTemp As Label, ByRef toVal As Long)
    Dim tmpVal As Long = 0
    If lblTemp.Text.Length > 3 And lblTemp.Text.Contains(" ") Then
      Dim tmpStr As String = lblTemp.Text.Substring(0, lblTemp.Text.LastIndexOf(" "))
      If tmpStr.Contains(",") Then tmpStr = tmpStr.Replace(",", "")
      If IsNumeric(tmpStr) Then
        tmpVal = Long.Parse(tmpStr, Globalization.CultureInfo.InvariantCulture)
      Else
        tmpVal = 0
      End If
      Dim majorDif As Long = Math.Abs(tmpVal - toVal)
      Select Case majorDif
        Case Is < 10
          majorDif = 1
        Case Is < 50
          majorDif = 3
        Case Is < 100
          majorDif = 7
        Case Is < 500
          majorDif = 73
        Case Is < 1000
          majorDif = 271
        Case Is < 5000
          majorDif = 977
        Case Is < 10000
          majorDif = 3347
        Case Is < 50000
          majorDif = 8237
        Case Else
          majorDif = 38671
      End Select
      If tmpVal > toVal Then
        tmpVal -= majorDif
        lblTemp.Text = tmpVal.ToString("N0", Globalization.CultureInfo.InvariantCulture) & " MB"
      ElseIf tmpVal < toVal Then
        tmpVal += majorDif
        lblTemp.Text = tmpVal.ToString("N0", Globalization.CultureInfo.InvariantCulture) & " MB"
      Else
        lblTemp.Text = toVal.ToString("N0", Globalization.CultureInfo.InvariantCulture) & " MB"
        toVal = 0
      End If
    Else
      lblTemp.Text = toVal.ToString("N0", Globalization.CultureInfo.InvariantCulture) & " MB"
      toVal = 0
    End If
  End Sub
#Region "Results"
  Private Function MBorGB(value As Long) As String
    If value > 999 Then
      Return Math.Round(value / MBPerGB, mySettings.Accuracy) & " GB"
    Else
      Return value & " MB"
    End If
  End Function
  Private Function AccuratePercent(value As Double) As String
    Return FormatPercent(value, mySettings.Accuracy, TriState.True, TriState.False, TriState.False)
  End Function
  Private Sub DisplayUsageResults(lUsed As Long, lLimit As Long, sLastUpdate As String)
    Dim sTTT As String = Me.Text
    imSlowed = (lUsed >= lLimit)
    pnlUsage.Visible = True
    pnlNothing.Visible = False
    uCache_used = lUsed
    uCache_lim = lLimit
    If tmrChanges IsNot Nothing Then
      tmrChanges.Dispose()
      tmrChanges = Nothing
    End If
    tmrChanges = New Threading.Timer(New Threading.TimerCallback(AddressOf DisplayChangeInterval), Nothing, 75, System.Threading.Timeout.Infinite)
    If imSlowed Then
      lblUsageFreeVal.ForeColor = Color.Red
      lblUsageLimitVal.ForeColor = Color.Red
      ttUI.SetToolTip(lblUsageFreeVal, "You are over your usage limit!")
      ttUI.SetToolTip(lblUsageLimitVal, "Your connection has been restricted!")
    Else
      lblUsageFreeVal.ForeColor = SystemColors.ControlText
      lblUsageLimitVal.ForeColor = SystemColors.ControlText
      ttUI.SetToolTip(lblUsageFreeVal, Nothing)
      ttUI.SetToolTip(lblUsageLimitVal, Nothing)
    End If
    pctUsage.Image = DisplayRProgress(pctUsage.DisplayRectangle.Size, lUsed, lLimit, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
    sTTT = "Satellite Usage" & IIf(imSlowed, " (Slowed)", "") & vbCr &
           "Updated " & sLastUpdate & vbCr &
            MBorGB(lUsed) & " of " & MBorGB(lLimit) & " (" & AccuratePercent(lUsed / lLimit) & ")"
    If sTTT.Length > ttLimit Then
      sTTT = "Usage" & IIf(imSlowed, " (Slow)", "") & " [" & sLastUpdate & "]" & vbCr &
            AccuratePercent(lUsed / lLimit)
    End If
    If lLimit > lUsed Then
      sTTT &= vbCr & MBorGB(lLimit - lUsed) & " Free"
    ElseIf lLimit < lUsed Then
      sTTT &= vbCr & MBorGB(lUsed - lLimit) & " Over"
    End If
    iconBefore = CreateUsageTrayIcon(lUsed, lLimit)
    iconStop = True
    SetNotifyIconText(trayIcon, sTTT)
    DisplayResultAlert(lUsed)
  End Sub
  Private Sub DisplayResults(lUsed As Long, lLimit As Long)
    If lLimit > 0 Then
      Dim lastUpdate As Date = LOG_GetLast()
      Dim sLastUpdate As String = lastUpdate.ToString("M/d h:mm tt", Globalization.CultureInfo.InvariantCulture)
      DisplayUsageResults(lUsed, lLimit, sLastUpdate)
    Else
      pnlUsage.Visible = False
      pnlNothing.Visible = True
      trayIcon.Text = Me.Text
      iconBefore = MakeIcon(IconName.norm)
      iconStop = True
    End If
  End Sub
  Private Sub DisplayResultAlert(lUsed As Long)
    If mySettings.Overuse > 0 Then
      If lastBalloon > 0 AndAlso srlFunctions.TickCount() - lastBalloon < mySettings.Overtime * 60 * 1000 Then Return
      Dim TimeCheck As Integer = -mySettings.Overtime
      If TimeCheck <= -15 Then
        Dim lItems() As DataRow = LOG_GetRange(Now.AddMinutes(TimeCheck), Now) ' Array.FindAll(usageDB.ToArray, Function(satRow As DataBase.DataRow) satRow.DATETIME.CompareTo(Now.AddMinutes(TimeCheck)) >= 0 And satRow.DATETIME.CompareTo(Now) <= 0)
        For I As Integer = lItems.Count - 2 To 0 Step -1
          If lUsed - lItems(I).USED >= mySettings.Overuse Then
            Dim ChangeSize As Long = Math.Abs(lUsed - lItems(I).USED)
            Dim ChangeTime As Long = Math.Abs(DateDiff(DateInterval.Minute, lItems(I).DATETIME, Now) * 60 * 1000)
            MakeNotifier(taskNotifier, False)
            If taskNotifier IsNot Nothing Then taskNotifier.Show("Excessive Usage Detected", My.Application.Info.ProductName & " has logged a usage change of " & MBorGB(ChangeSize) & " in " & ConvertTime(ChangeTime) & "!", 200, 0, 100)
            lastBalloon = srlFunctions.TickCount()
            Exit For
          End If
        Next
      End If
    End If
  End Sub
#End Region
#End Region
#Region "Buttons"
  Private Sub cmdRefresh_Click(sender As System.Object, e As System.EventArgs) Handles cmdRefresh.Click
    InitAccount()
    If Not String.IsNullOrEmpty(sAccount) And Not String.IsNullOrEmpty(sPassword) Then
      EnableProgressIcon()
      SetNextLoginTime()
      If My.Computer.Keyboard.CtrlKeyDown Then
        FullCheck = True
        cmdRefresh.Enabled = False
        SetStatusText("Reloading", "Reloading History...", False)
        If Not frmDBProgress.Visible Then frmDBProgress.Show(Me)
        frmDBProgress.SetAction("Reloading History...", "Reading DataBase...")
        LOG_Initialize(sAccount, True)
        frmDBProgress.Close()
        If ClosingTime Then Return
        cmdRefresh.Enabled = True
      End If
      SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Beginning Usage Request...", False)
      Dim UsageInvoker As New MethodInvoker(AddressOf GetUsage)
      UsageInvoker.BeginInvoke(Nothing, Nothing)
    Else
      RestoreWindow()
      cmdConfig.Focus()
      MsgDlg(Me, "Please enter your account details in the Config window by clicking Configuration.", "You haven't entered your account details.", "Account Details Required", MessageBoxButtons.OK, _TaskDialogIcon.User, MessageBoxIcon.Error)
    End If
  End Sub
  Private Sub cmdHistory_Click(sender As System.Object, e As System.EventArgs) Handles cmdHistory.Click
    mySettings.Save()
    If frmHistory.Visible Then
      frmHistory.Focus()
    Else
      frmHistory.Show(Me)
    End If
  End Sub
  Private Sub cmdConfig_Click(sender As System.Object, e As System.EventArgs) Handles cmdConfig.Click
    Dim bReRun As Boolean = False
    If localData IsNot Nothing Then
      localData.Dispose()
      localData = Nothing
      bReRun = True
    End If
    mySettings.Save()
    NextGrabTick = Long.MaxValue
    PauseActivity = "Configuration Open"
    Dim dRet As DialogResult
    Using dlgConfig As New frmConfig
      dRet = dlgConfig.ShowDialog(Me)
    End Using
    Dim WaitTime As Long = srlFunctions.TickCount() + 2000
    If Not myState = LoadStates.Loaded Then
      If Not myState = LoadStates.DB Then
        Dim initDBInvoker As New MethodInvoker(AddressOf initDB)
        initDBInvoker.BeginInvoke(Nothing, Nothing)
      End If
      Do Until myState = LoadStates.Loaded
        Application.DoEvents()
        Threading.Thread.Sleep(1)
        If srlFunctions.TickCount() > WaitTime Then Exit Do
      Loop
    End If
    Select Case dRet
      Case Windows.Forms.DialogResult.Yes
        ReLoadSettings()
        SetNextLoginTime()
        Dim ReInitInvoker As New MethodInvoker(AddressOf ReInit)
        ReInitInvoker.BeginInvoke(Nothing, Nothing)
      Case Windows.Forms.DialogResult.OK
        ReLoadSettings()
        If bReRun Then
          SetNextLoginTime()
          EnableProgressIcon()
          SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Preparing Connection...", False)
          Dim UsageInvoker As New MethodInvoker(AddressOf GetUsage)
          UsageInvoker.BeginInvoke(Nothing, Nothing)
        Else
          DisplayUsage(True, False)
        End If
        Dim sizeChangeInvoker As New EventHandler(AddressOf frmMain_SizeChanged)
        sizeChangeInvoker.BeginInvoke(Me, New EventArgs, Nothing, Nothing)
        If frmHistory.Visible Then
          frmHistory.mySettings = New AppSettings
          ScreenDefaultColors(frmHistory.mySettings.Colors)
          If LocalAppDataDirectory = IO.Path.Combine(Application.StartupPath, "Config") Then frmHistory.mySettings.HistoryDir = IO.Path.Combine(Application.StartupPath, "Config")
          frmHistory.DoResize(True)
        End If
      Case Windows.Forms.DialogResult.Abort

      Case Else
        If bReRun Then
          SetNextLoginTime()
          EnableProgressIcon()
          SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Preparing Connection...", False)
          Dim UsageInvoker As New MethodInvoker(AddressOf GetUsage)
          UsageInvoker.BeginInvoke(Nothing, Nothing)
        Else
          DisplayUsage(False, False)
        End If
    End Select
  End Sub
  Private Sub cmdAbout_Click(sender As System.Object, e As System.EventArgs) Handles cmdAbout.Click
    frmAbout.TopMost = Me.TopMost
    frmAbout.Show()
  End Sub
#End Region
#Region "Menus"
#Region "Tray"
  Private Sub mnuRestore_Click(sender As System.Object, e As System.EventArgs) Handles mnuRestore.Click
    RestoreWindow()
    Dim myProc As Integer = 0
    Try
      myProc = Process.GetCurrentProcess.Id
    Catch ex As Exception
      myProc = 0
    End Try
    If Not myProc = 0 Then
      Try
        AppActivate(myProc)
        Return
      Catch ex As Exception
      End Try
    End If
    Dim myTitle As String = String.Empty
    Try
      myTitle = Me.Text
    Catch ex As Exception
      myTitle = String.Empty
    End Try
    If String.IsNullOrEmpty(myTitle) Then
      Me.Activate()
    ElseIf myTitle = "{0}" Then
      Me.Activate()
    Else
      Try
        AppActivate(myTitle)
      Catch ex As Exception
        Me.Activate()
      End Try
    End If
  End Sub
  Private Sub mnuAbout_Click(sender As System.Object, e As System.EventArgs) Handles mnuAbout.Click
    frmAbout.Show()
  End Sub
  Private Sub mnuExit_Click(sender As System.Object, e As System.EventArgs) Handles mnuExit.Click
    Application.Exit()
  End Sub
#End Region
#Region "Graph"
  Private Sub mnuGraphRefresh_Click(sender As System.Object, e As System.EventArgs) Handles mnuGraphRefresh.Click
    cmdRefresh.PerformClick()
  End Sub
  Private Sub mnuGraphColors_Click(sender As System.Object, e As System.EventArgs) Handles mnuGraphColors.Click
    frmCustomColors.mySettings = mySettings
    If frmCustomColors.ShowDialog(Me) = Windows.Forms.DialogResult.Yes Then
      mySettings = frmCustomColors.mySettings
      mySettings.Save()
      Dim sizeChangeInvoker As New EventHandler(AddressOf frmMain_SizeChanged)
      sizeChangeInvoker.BeginInvoke(Me, New EventArgs, Nothing, Nothing)
      If frmHistory.Visible Then
        frmHistory.mySettings = New AppSettings
        ScreenDefaultColors(frmHistory.mySettings.Colors)
        If LocalAppDataDirectory = IO.Path.Combine(Application.StartupPath, "Config") Then frmHistory.mySettings.HistoryDir = IO.Path.Combine(Application.StartupPath, "Config")
        frmHistory.DoResize(True)
      End If
    End If
  End Sub
#End Region
#End Region
#Region "Tray Icon"
  Private Enum IconName
    norm
    free
    restricted
    throb0
    throb1
    throb2
    throb3
    throb4
    throb5
    throb6
    throb7
    throb8
    throb9
    throb10
    throb11
  End Enum
  Private Shared Function MakeIcon(name As IconName, Optional icoX As Integer = -1, Optional icoY As Integer = -1) As Icon
    If icoX < 0 Then icoX = NativeMethods.GetSystemMetrics(NativeMethods.MetricsList.SM_CXSMICON)
    If icoY < 0 Then icoY = NativeMethods.GetSystemMetrics(NativeMethods.MetricsList.SM_CYSMICON)
    Dim large As Boolean = icoX > 16 Or icoY > 16
    Dim imgICO As New Bitmap(icoX, icoY)
    Using g As Graphics = Graphics.FromImage(imgICO)
      g.Clear(Color.Transparent)
      Select Case name
        Case IconName.norm : g.DrawIcon(IIf(large, My.Resources.t32_norm, My.Resources.t16_norm), New Rectangle(0, 0, icoX, icoY))
        Case IconName.free : g.DrawIcon(IIf(large, My.Resources.t32_free, My.Resources.t16_free), New Rectangle(0, 0, icoX, icoY))
        Case IconName.restricted : g.DrawIcon(IIf(large, My.Resources.t32_restricted, My.Resources.t16_restricted), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb0 : g.DrawIcon(IIf(large, My.Resources.t32_0, My.Resources.t16_0), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb1 : g.DrawIcon(IIf(large, My.Resources.t32_1, My.Resources.t16_1), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb2 : g.DrawIcon(IIf(large, My.Resources.t32_2, My.Resources.t16_2), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb3 : g.DrawIcon(IIf(large, My.Resources.t32_3, My.Resources.t16_3), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb4 : g.DrawIcon(IIf(large, My.Resources.t32_4, My.Resources.t16_4), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb5 : g.DrawIcon(IIf(large, My.Resources.t32_5, My.Resources.t16_5), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb6 : g.DrawIcon(IIf(large, My.Resources.t32_6, My.Resources.t16_6), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb7 : g.DrawIcon(IIf(large, My.Resources.t32_7, My.Resources.t16_7), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb8 : g.DrawIcon(IIf(large, My.Resources.t32_8, My.Resources.t16_8), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb9 : g.DrawIcon(IIf(large, My.Resources.t32_9, My.Resources.t16_9), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb10 : g.DrawIcon(IIf(large, My.Resources.t32_10, My.Resources.t16_10), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb11 : g.DrawIcon(IIf(large, My.Resources.t32_11, My.Resources.t16_11), New Rectangle(0, 0, icoX, icoY))
      End Select
    End Using
    Try
      Dim hIcon As IntPtr = imgICO.GetHicon()
      Dim iIcon As Icon = Icon.FromHandle(hIcon).Clone
      NativeMethods.DestroyIcon(hIcon)
      Return iIcon
    Catch ex As Exception
      Return IIf(large, My.Resources.t32_norm, My.Resources.t16_norm)
    End Try
  End Function
  Private Sub tmrIcon_Tick(sender As System.Object, e As System.EventArgs) Handles tmrIcon.Tick
    Try
      If iconItem = 0 And iconStop Then
        tmrIcon.Enabled = False
        trayIcon.Icon = iconBefore
        iconStop = False
        Return
      End If
      Select Case iconItem
        Case 0 : trayIcon.Icon = MakeIcon(IconName.throb0)
        Case 1 : trayIcon.Icon = MakeIcon(IconName.throb1)
        Case 2 : trayIcon.Icon = MakeIcon(IconName.throb2)
        Case 3 : trayIcon.Icon = MakeIcon(IconName.throb3)
        Case 4 : trayIcon.Icon = MakeIcon(IconName.throb4)
        Case 5 : trayIcon.Icon = MakeIcon(IconName.throb5)
        Case 6 : trayIcon.Icon = MakeIcon(IconName.throb6)
        Case 7 : trayIcon.Icon = MakeIcon(IconName.throb7)
        Case 8 : trayIcon.Icon = MakeIcon(IconName.throb8)
        Case 9 : trayIcon.Icon = MakeIcon(IconName.throb9)
        Case 10 : trayIcon.Icon = MakeIcon(IconName.throb10)
        Case 11 : trayIcon.Icon = MakeIcon(IconName.throb11)
      End Select
      iconItem += 1
      If iconItem >= 12 Then iconItem = 0
    Catch ex As Exception
    End Try
  End Sub
  Private Shared Sub SetNotifyIconText(ni As NotifyIcon, text As String)
    If text.Length >= 128 Then Throw New ArgumentOutOfRangeException("text", "Text limited to 127 characters")
    Dim t As Type = GetType(NotifyIcon)
    Dim hidden As Reflection.BindingFlags = Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance
    t.GetField("text", hidden).SetValue(ni, text)
    If CBool(t.GetField("added", hidden).GetValue(ni)) Then
      t.GetMethod("UpdateIcon", hidden).Invoke(ni, New Object() {True})
    End If
  End Sub
  Private Sub trayIcon_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles trayIcon.MouseUp
    If e.Button = Windows.Forms.MouseButtons.Left Then
      mnuRestore.PerformClick()
    ElseIf e.Button = Windows.Forms.MouseButtons.Middle Then
      cmdRefresh_Click(trayIcon, New EventArgs)
    End If
  End Sub
  Private Sub taskNotifier_CloseClick(sender As Object, e As System.EventArgs) Handles taskNotifier.CloseClick
    sFailTray = Nothing
  End Sub
#Region "Graphs"
  Private Function CreateUsageTrayIcon(lUsed As Long, lLim As Long) As Icon
    Dim icoX As Integer = NativeMethods.GetSystemMetrics(NativeMethods.MetricsList.SM_CXSMICON)
    Dim icoY As Integer = NativeMethods.GetSystemMetrics(NativeMethods.MetricsList.SM_CYSMICON)
    Dim imgTray As New Bitmap(icoX, icoY)
    Using g As Graphics = Graphics.FromImage(imgTray)
      g.Clear(Color.Transparent)
      If imSlowed Then
        g.DrawIconUnstretched(MakeIcon(IconName.restricted, icoX, icoY), New Rectangle(0, 0, icoX, icoY))
        If lUsed < lLim Then CreateTrayIcon_Left(g, lUsed, lLim, mySettings.Colors.TrayDownA, mySettings.Colors.TrayDownB, mySettings.Colors.TrayDownC, icoX, icoY)
        If lUsed < lLim Then CreateTrayIcon_Right(g, lUsed, lLim, mySettings.Colors.TrayDownA, mySettings.Colors.TrayDownB, mySettings.Colors.TrayDownC, icoX, icoY)
      ElseIf imFree Then
        g.DrawIconUnstretched(MakeIcon(IconName.free, icoX, icoY), New Rectangle(0, 0, icoX, icoY))
      Else
        g.DrawIconUnstretched(MakeIcon(IconName.norm, icoX, icoY), New Rectangle(0, 0, icoX, icoY))
        CreateTrayIcon_Left(g, lUsed, lLim, mySettings.Colors.TrayDownA, mySettings.Colors.TrayDownB, mySettings.Colors.TrayDownC, icoX, icoY)
        CreateTrayIcon_Right(g, lUsed, lLim, mySettings.Colors.TrayDownA, mySettings.Colors.TrayDownB, mySettings.Colors.TrayDownC, icoX, icoY)
      End If
    End Using
    Try
      Dim hIcon As IntPtr = imgTray.GetHicon()
      Dim iIcon As Icon = Icon.FromHandle(hIcon).Clone
      NativeMethods.DestroyIcon(hIcon)
      Return iIcon
    Catch ex As Exception
      Return MakeIcon(IconName.norm, icoX, icoY)
    End Try
  End Function
#End Region
#End Region
#Region "Update Events"
  Private Sub CheckForUpdates()
    If Not frmAbout.Visible Then
      SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Checking for Software Update...", False)
      NextGrabTick = srlFunctions.TickCount() + (mySettings.Interval * 60 * 1000)
      If updateChecker IsNot Nothing Then
        updateChecker.Dispose()
        updateChecker = Nothing
      End If
      updateChecker = New clsUpdate
      updateChecker.CheckVersion()
    End If
  End Sub
  Private Sub updateChecker_CheckResult(sender As Object, e As clsUpdate.CheckEventArgs) Handles updateChecker.CheckResult
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(Of clsUpdate.CheckEventArgs)(AddressOf updateChecker_CheckResult), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    mySettings.LastUpdate = Now
    mySettings.Save()
    If e.Error Is Nothing And Not e.Cancelled Then
      If mySettings.UpdateType = AppSettings.UpdateTypes.Ask Then
        Dim fUpdate As New frmUpdate
        Select Case e.Result
          Case clsUpdate.CheckEventArgs.ResultType.NewUpdate
            fUpdate.NewUpdate(e.Version, False, Not isAdmin())
            Select Case fUpdate.ShowDialog()
              Case Windows.Forms.DialogResult.Yes
                If localData IsNot Nothing Then
                  localData.Dispose()
                  localData = Nothing
                End If
                updateChecker.DownloadUpdate(sEXEPath)
              Case Windows.Forms.DialogResult.No
                If updateChecker IsNot Nothing Then
                  updateChecker.Dispose()
                  updateChecker = Nothing
                End If
                NextGrabTick = Long.MinValue
              Case Windows.Forms.DialogResult.OK
                If localData IsNot Nothing Then
                  localData.Dispose()
                  localData = Nothing
                End If
                updateChecker.DownloadUpdate(sEXEPath)
                mySettings.UpdateBETA = False
                mySettings.Save()
              Case Windows.Forms.DialogResult.Cancel
                mySettings.UpdateBETA = False
                mySettings.Save()
                If updateChecker IsNot Nothing Then
                  updateChecker.Dispose()
                  updateChecker = Nothing
                End If
                NextGrabTick = Long.MinValue
              Case Else
                If updateChecker IsNot Nothing Then
                  updateChecker.Dispose()
                  updateChecker = Nothing
                End If
                NextGrabTick = Long.MinValue
            End Select
          Case clsUpdate.CheckEventArgs.ResultType.NewBeta
            If mySettings.UpdateBETA Then
              fUpdate.NewUpdate(e.Version, True, Not isAdmin())
              Select Case fUpdate.ShowDialog()
                Case Windows.Forms.DialogResult.Yes
                  If localData IsNot Nothing Then
                    localData.Dispose()
                    localData = Nothing
                  End If
                  updateChecker.DownloadUpdate(sEXEPath)
                Case Windows.Forms.DialogResult.No
                  If updateChecker IsNot Nothing Then
                    updateChecker.Dispose()
                    updateChecker = Nothing
                  End If
                  NextGrabTick = Long.MinValue
                Case Windows.Forms.DialogResult.OK
                  If localData IsNot Nothing Then
                    localData.Dispose()
                    localData = Nothing
                  End If
                  updateChecker.DownloadUpdate(sEXEPath)
                  mySettings.UpdateBETA = False
                  mySettings.Save()
                Case Windows.Forms.DialogResult.Cancel
                  mySettings.UpdateBETA = False
                  mySettings.Save()
                  If updateChecker IsNot Nothing Then
                    updateChecker.Dispose()
                    updateChecker = Nothing
                  End If
                  NextGrabTick = Long.MinValue
                Case Else
                  If updateChecker IsNot Nothing Then
                    updateChecker.Dispose()
                    updateChecker = Nothing
                  End If
                  NextGrabTick = Long.MinValue
              End Select
            End If
          Case Else
            SetStatusText(srlFunctions.TimeToString(LOG_GetLast), String.Empty, False)
            If updateChecker IsNot Nothing Then
              updateChecker.Dispose()
              updateChecker = Nothing
            End If
            NextGrabTick = Long.MinValue
        End Select
      Else
        Select Case e.Result
          Case clsUpdate.CheckEventArgs.ResultType.NewUpdate
            If localData IsNot Nothing Then
              localData.Dispose()
              localData = Nothing
            End If
            updateChecker.DownloadUpdate(sEXEPath)
          Case clsUpdate.CheckEventArgs.ResultType.NewBeta
            If mySettings.UpdateBETA Then
              If localData IsNot Nothing Then
                localData.Dispose()
                localData = Nothing
              End If
              updateChecker.DownloadUpdate(sEXEPath)
            End If
          Case Else
            SetStatusText(srlFunctions.TimeToString(LOG_GetLast), String.Empty, False)
            If updateChecker IsNot Nothing Then
              updateChecker.Dispose()
              updateChecker = Nothing
            End If
            NextGrabTick = Long.MinValue
        End Select
      End If
    End If
  End Sub
  Private Sub updateChecker_DownloadingUpdate(sender As Object, e As System.EventArgs) Handles updateChecker.DownloadingUpdate
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(AddressOf updateChecker_DownloadingUpdate), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    tmrSpeed.Enabled = True
    SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Downloading Software Update...", False)
  End Sub
  Private Sub updateChecker_DownloadResult(sender As Object, e As clsUpdate.DownloadEventArgs) Handles updateChecker.DownloadResult
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(Of clsUpdate.DownloadEventArgs)(AddressOf updateChecker_DownloadResult), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    tmrSpeed.Enabled = False
    If e.Error IsNot Nothing Then
      SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Software Update Error: " & e.Error.Message, True)
      NextGrabTick = Long.MinValue
    ElseIf e.Cancelled Then
      If updateChecker IsNot Nothing Then
        updateChecker.Dispose()
        updateChecker = Nothing
      End If
      SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Software Update Cancelled!", True)
      NextGrabTick = Long.MinValue
    Else
      If updateChecker IsNot Nothing Then
        updateChecker.Dispose()
        updateChecker = Nothing
      End If
      SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Software Update Download Complete", False)
      Dim RecheckOnMissing As Boolean = False
      Do
        Application.DoEvents()
        Try
          If My.Computer.FileSystem.FileExists(sEXEPath) Then
            ShellEx(sEXEPath, UpdateParam)
            Application.Exit()
            Return
          ElseIf RecheckOnMissing Then
            If MsgDlg(Me, "The update file no longer exists in the location it was downloaded to." & vbNewLine & vbNewLine & "If you are running Anti-Virus software, please make sure it hasn't quarantined or deleted the " & My.Application.Info.ProductName & " Installer." & vbNewLine, "The update installer is missing.", "Software Update Error", MessageBoxButtons.RetryCancel, _TaskDialogIcon.ShieldWarning, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Cancel Then Exit Do
          Else
            MsgDlg(Me, "The update file no longer exists in the location it was downloaded to." & vbNewLine & vbNewLine & "If you are running Anti-Virus software, please make sure it hasn't quarantined or deleted the " & My.Application.Info.ProductName & " Installer." & vbNewLine, "The update installer is missing.", "Software Update Error", MessageBoxButtons.OK, _TaskDialogIcon.ShieldWarning, MessageBoxIcon.Warning)
            Exit Do
          End If
        Catch ex As Exception
          If MsgDlg(Me, "There was an error starting the update process." & vbNewLine & vbNewLine & "If you have User Account Control enabled," & vbNewLine & "please allow the " & My.Application.Info.ProductName & " Installer to run." & vbNewLine & "If you are running Anti-Virus software, please make sure it isn't blocking the " & My.Application.Info.ProductName & " Installer." & vbNewLine, "The update installer failed to start.", "Software Update Error", MessageBoxButtons.RetryCancel, _TaskDialogIcon.ShieldWarning, MessageBoxIcon.Warning, , ex.Message, _TaskDialogExpandedDetailsLocation.ExpandFooter, "View Error Details", "Hide Error Details") = Windows.Forms.DialogResult.Cancel Then Exit Do
          RecheckOnMissing = True
        End Try
      Loop
      SetStatusText(srlFunctions.TimeToString(LOG_GetLast), "Software Update Failure!", True)
      NextGrabTick = Long.MinValue
    End If
  End Sub
  Private CurSize As Long
  Private TotalSize As Long
  Private DownSpeed As ULong
  Private CurPercent As Integer
  Private Sub updateChecker_UpdateProgressChanged(sender As Object, e As clsUpdate.ProgressEventArgs) Handles updateChecker.UpdateProgressChanged
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(Of clsUpdate.ProgressEventArgs)(AddressOf updateChecker_UpdateProgressChanged), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    CurSize = e.BytesReceived
    TotalSize = e.TotalBytesToReceive
    CurPercent = e.ProgressPercentage
  End Sub
  Private LastSize As Long
  Private Sub tmrSpeed_Tick(sender As Object, e As System.EventArgs) Handles tmrSpeed.Tick
    If CurSize > LastSize Then
      DownSpeed = CurSize - LastSize
    Else
      DownSpeed = 0
    End If
    LastSize = CurSize
    Dim sProgress As String = "[" & CurPercent & "%]"
    Dim sStatus As String = ByteSize(CurSize) & " of " & ByteSize(TotalSize) & " at " & ByteSize(DownSpeed) & "/s..."
    If TotalSize = 0 Then
      sStatus = "Downloading Update (Waiting for Response)..."
      sProgress = "[Waiting for Response]"
    End If
    SetStatusText("Downloading Update " & sProgress, sStatus, False)
  End Sub
#End Region
#Region "Useful Functions"
  Private Sub PowerModeChanged(sender As Object, e As Microsoft.Win32.PowerModeChangedEventArgs)
    If e.Mode = Microsoft.Win32.PowerModes.Suspend Then
      If localData IsNot Nothing Then
        localData.Dispose()
        localData = Nothing
      End If
      cmdRefresh.Enabled = True
    ElseIf e.Mode = Microsoft.Win32.PowerModes.Resume Then
      ReLoadSettings()
      cmdRefresh.Enabled = True
      DisplayUsage(False, False)
      SetNextLoginTime(mySettings.StartWait)
    End If
  End Sub
  Private Delegate Sub SetStatusTextCallBack(Status As String, Details As String, Alert As Boolean)
  Private Sub SetStatusText(Status As String, Details As String, Alert As Boolean)
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New SetStatusTextCallBack(AddressOf SetStatusText), Status, Details, Alert)
      Catch ex As Exception
      End Try
      Return
    End If
    If Status = "1/1/1970 12:00 AM" Then
      If Alert Or Details.StartsWith("Next Update in ") Then
        sDisp_LT = sDISPLAY_LT_NONE
      Else
        sDisp_LT = sDISPLAY_LT_BUSY
      End If
    Else
      sDisp_LT = Status
    End If
    If String.IsNullOrEmpty(Details) Then
      bAlert = TriState.UseDefault
      sDisp_TT_E = Nothing
    ElseIf Alert Then
      bAlert = TriState.True
      sDisp_TT_E = Details
    Else
      bAlert = TriState.False
      sDisp_TT_E = Details
    End If
    Application.DoEvents()
  End Sub
  Private Sub tmrStatus_Tick(sender As System.Object, e As System.EventArgs) Handles tmrStatus.Tick
    Dim lNext As Long = NextGrabTick
    Dim lNow As Long = srlFunctions.TickCount()
    If lNext = Long.MaxValue Then
      ttUI.SetToolTip(lblStatus, "Update Temporarily Paused - " & PauseActivity)
    ElseIf lNext = Long.MinValue Then
      ttUI.SetToolTip(lblStatus, "Next Update is Being Calculated")
    Else
      sDisp = sDISPLAY.Replace("%lt", sDisp_LT)
      If lNext - lNow >= 1000 Then
        Dim uTime As ULong = Math.Abs(lNext - lNow)
        sDisp_TT_T = ConvertTime(uTime, False, False)
        sDisp_TT_M = sDISPLAY_TT_NEXT.Replace("%t", sDisp_TT_T)
      ElseIf lNow - lNext >= 1000 Then
        Dim uTime As ULong = Math.Abs(lNow - lNext)
        sDisp_TT_T = ConvertTime(uTime, False, False)
        sDisp_TT_M = sDISPLAY_TT_LATE.Replace("%t", sDisp_TT_T)
      Else
        sDisp_TT_T = sDISPLAY_TT_T_SOON
        sDisp_TT_M = sDISPLAY_TT_NEXT.Replace("%t", sDisp_TT_T)
      End If

      If bAlert = TriState.True Then
        If Not String.IsNullOrEmpty(lblStatus.Text) Then
          If lblStatus.Text.EndsWith(" !") Then
            lblStatus.Text = sDisp & "  "
          Else
            lblStatus.Text = sDisp & " !"
          End If
        End If
        sDispTT = sDISPLAY_TT_ERR.Replace("%m", sDisp_TT_M).Replace("%e", sDisp_TT_E)
      ElseIf bAlert = TriState.False Then
        If Not String.IsNullOrEmpty(ttUI.GetToolTip(lblStatus)) Then
          If ttUI.GetToolTip(lblStatus).EndsWith("...") Then
            If lblStatus.Text.EndsWith("...") Then
              lblStatus.Text = sDisp
            ElseIf lblStatus.Text.EndsWith(".") Then
              lblStatus.Text &= "."
            Else
              lblStatus.Text = sDisp & "."
            End If
          End If
        End If
        sDispTT = sDisp_TT_E
      Else
        lblStatus.Text = sDisp
        sDispTT = sDisp_TT_M
      End If
      ttUI.SetToolTip(lblStatus, sDispTT)
    End If
  End Sub
  Private Sub ControlService(Run As Boolean)
    Try
      If mySettings.Service Then
        If My.Computer.FileSystem.FileExists(IO.Path.Combine(Application.StartupPath, "RestrictionController.exe")) Then
          Dim ControllerProps As New ProcessStartInfo(IO.Path.Combine(Application.StartupPath, "RestrictionController.exe"))
          ControllerProps.Arguments = IIf(Run, "/run", "/stop")
          ControllerProps.WindowStyle = ProcessWindowStyle.Hidden
          Process.Start(ControllerProps)
        Else
          MsgDlg(Me, "The Satellite Restriction Logger Service Controller could not be found!" & vbNewLine & vbNewLine & "Please reinstall " & My.Application.Info.ProductName & " with the ""Windows Service"" component selected.", "Service Controller not found.", "Logger Service Error", MessageBoxButtons.OK, _TaskDialogIcon.Batch, MessageBoxIcon.Error)
          mySettings.Service = False
        End If
      End If
    Catch ex As Exception
      MsgDlg(Me, "The Satellite Restriction Logger Service Controller could not start!", "Service Controller failed to start.", "Logger Service Error", MessageBoxButtons.OK, _TaskDialogIcon.Batch, MessageBoxIcon.Error, , ex.Message, _TaskDialogExpandedDetailsLocation.ExpandFooter, "View Error Details", "Hide Error Details")
    End Try
  End Sub
  Private Sub wsFavicon_DownloadIconCompleted(icon16 As Image, icon32 As Image, token As Object, [Error] As Exception)
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New clsFavicon.DownloadIconCompletedCallback(AddressOf wsFavicon_DownloadIconCompleted), icon16, icon32, token, [Error])
      Catch ex As Exception
      End Try
      Return
    End If
    Try
      pctNetTest.Visible = True
      If [Error] IsNot Nothing Then
        pctNetTest.Image = My.Resources.ico_err
      Else
        icon16.Save(IO.Path.Combine(LocalAppDataDirectory, "netTest.png"))
        pctNetTest.Image = icon16
      End If
    Catch ex As Exception
      pctNetTest.Image = My.Resources.ico_err
    End Try
  End Sub
  Public Shared Function CompareImages(Image1 As Bitmap, Image2 As Bitmap) As Boolean
    If Image1 Is Nothing Then Return False
    If Image2 Is Nothing Then Return False
    Try
      If Not Image1.Size.Width = Image2.Size.Width Or Not Image1.Size.Height = Image2.Size.Height Then Return False
    Catch ex As Exception
      Return False
    End Try
    For Y As Integer = 0 To Image1.Size.Height - 1
      For X As Integer = 0 To Image1.Size.Width - 1
        If Not Image1.GetPixel(X, Y) = Image2.GetPixel(X, Y) Then Return False
      Next
    Next
    Return True
  End Function
  Private Sub CheckComposition()
    dwmComp = clsGlass.IsCompositionEnabled
  End Sub
  Private Function CleanRendering() As Boolean
    If Environment.OSVersion.Version.Major < 6 Then Return False
    If Not Application.RenderWithVisualStyles Then Return False
    Return dwmComp
  End Function
#End Region
End Class
