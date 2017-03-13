Imports RestrictionLibrary.localRestrictionTracker
Public Class frmMain
  Private myPanel As SatHostTypes
  Private Enum LoadStates
    Loading
    Loaded
    Lookup
  End Enum
  Private Delegate Sub ParamaterizedInvoker(parameter As Object)
  Private Delegate Sub ParamaterizedInvoker2(parameter As Object, secondParam As Object)
  Private WithEvents updateChecker As clsUpdate
  Private WithEvents taskNotifier As TaskbarNotifier
  Private WithEvents remoteData As remoteRestrictionTracker
  Private WithEvents localData As localRestrictionTracker
#Region "Constants"
  Private Const sWB As String = "https://myaccount.{0}/wbisp/{2}/{1}.jsp"
  Private Const sRP As String = "https://{0}.ruralportal.net/us/{1}.do"
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
  Private sEXEPath As String = LocalAppDataDirectory & "SRT_Setup.exe"
  Private mySettings As AppSettings
  Private sAccount, sPassword, sProvider As String
  Private imSlowed As Boolean
  Private imFree As Boolean
  Private FullCheck As Boolean = True
  Private NextGrabTick As Long
  Private ClosingTime As Boolean
  Private sFailTray As String
  Private bAlert As TriState
  Private typeA_down, typeA_up, typeA_dlim, typeA_ulim As Long
  Private typeB_used, typeB_lim As Long
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
#Region "Server Type Determination"
  Private Class DetermineTypeOffline
    Public Delegate Sub TypeDeterminedOfflineCallback(HostType As SatHostTypes)
    Private c_callback As TypeDeterminedOfflineCallback
    Public Sub New(Provider As String, callback As TypeDeterminedOfflineCallback)
      c_callback = callback
      Dim beginInvoker As New BeginTestInvoker(AddressOf BeginTest)
      beginInvoker.BeginInvoke(Provider, Nothing, Nothing)
    End Sub
    Private Delegate Sub BeginTestInvoker(Provider As String)
    Private Sub BeginTest(Provider As String)
      If Provider.ToLower = "dish.com" Or Provider.ToLower = "dish.net" Then
        c_callback.Invoke(SatHostTypes.DishNet_EXEDE)
      ElseIf Provider.ToLower = "exede.com" Or Provider.ToLower = "exede.net" Or Provider.ToLower = "satelliteinternetco.com" Then
        c_callback.Invoke(SatHostTypes.WildBlue_EXEDE)
      Else
        OfflineCheck()
      End If
    End Sub
    Private Sub OfflineStats(ByRef rpP As Single, ByRef exP As Single, ByRef wbP As Single)
      If LOG_GetCount() > 0 Then
        Dim TotalCount As Integer
        Dim RPGuess As Integer
        Dim ExGuess As Integer
        Dim WBGuess As Integer
        Dim logStep As Integer = 1
        If LOG_GetCount() > 50 Then
          logStep = 10
        ElseIf LOG_GetCount() > 10 Then
          logStep = 5
        Else
          logStep = 1
        End If
        For I As Integer = 0 To LOG_GetCount() - 1 Step logStep
          Dim dtDate As Date
          Dim lDown As Long
          Dim lDLim As Long
          Dim lUp As Long
          Dim lULim As Long
          LOG_Get(I, dtDate, lDown, lDLim, lUp, lULim)
          If lDLim = lULim Then
            If lDown = lUp Then
              RPGuess += 1
            Else
              ExGuess += 1
            End If
          ElseIf lULim = 0 Then
            ExGuess += 1
          Else
            WBGuess += 1
          End If
          TotalCount += 1
        Next
        rpP = RPGuess / TotalCount
        exP = ExGuess / TotalCount
        wbP = WBGuess / TotalCount
      End If
    End Sub
    Private Sub OfflineCheck()
      Dim rpP, exP, wbP As Single
      OfflineStats(rpP, exP, wbP)
      If rpP = 0 And exP = 0 And wbP = 0 Then
        c_callback.Invoke(SatHostTypes.Other)
      Else
        If rpP > exP And rpP > wbP Then
          c_callback.Invoke(SatHostTypes.RuralPortal_EXEDE)
        ElseIf exP > rpP And exP > wbP Then
          c_callback.Invoke(SatHostTypes.WildBlue_EXEDE)
        ElseIf wbP > rpP And wbP > exP Then
          c_callback.Invoke(SatHostTypes.WildBlue_LEGACY)
        Else
          If rpP > wbP And exP > wbP And rpP = exP Then
            c_callback.Invoke(SatHostTypes.WildBlue_EXEDE)
          Else
            c_callback.Invoke(SatHostTypes.Other)
            'TODO: Handle unknown host type
            Debug.Print("Oh noes! I don't know what type of host this is!")
          End If
        End If
      End If
    End Sub
  End Class
  Private Sub TypeDetermination_TypeDetermined(HostGroup As DetermineType.SatHostGroup)
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New DetermineType.TypeDeterminedCallback(AddressOf TypeDetermination_TypeDetermined), HostGroup)
      Catch ex As Exception
      End Try
      Return
    End If
    NextGrabTick = srlFunctions.TickCount() + (mySettings.Interval * 60 * 1000)
    If HostGroup = DetermineType.SatHostGroup.Other Then
      iconStop = True
      Dim TypeDeterminationOffline As New DetermineTypeOffline(sProvider, AddressOf TypeDeterminationOffline_TypeDetermined)
    Else
      If HostGroup = DetermineType.SatHostGroup.DishNet Then
        mySettings.AccountType = SatHostTypes.DishNet_EXEDE
      ElseIf HostGroup = DetermineType.SatHostGroup.WildBlue Then
        mySettings.AccountType = SatHostTypes.WildBlue_LEGACY
      ElseIf HostGroup = DetermineType.SatHostGroup.RuralPortal Then
        mySettings.AccountType = SatHostTypes.RuralPortal_EXEDE
      ElseIf HostGroup = DetermineType.SatHostGroup.Exede Then
        mySettings.AccountType = SatHostTypes.WildBlue_EXEDE
      End If
      ScreenDefaultColors(mySettings.Colors, mySettings.AccountType)
      mySettings.Save()
      SetStatusText(LOG_GetLast.ToString("g"), "Preparing Connection...", False)
      If localData IsNot Nothing Then
        localData.Dispose()
        localData = Nothing
      End If
      localData = New localRestrictionTracker(LocalAppDataDirectory)
    End If
  End Sub
  Private Sub TypeDeterminationOffline_TypeDetermined(HostType As SatHostTypes)
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New DetermineTypeOffline.TypeDeterminedOfflineCallback(AddressOf TypeDeterminationOffline_TypeDetermined), HostType)
      Catch ex As Exception
      End Try
      Return
    End If
    NextGrabTick = srlFunctions.TickCount() + (mySettings.Interval * 60 * 1000)
    If HostType = SatHostTypes.Other Then
      iconStop = True
      DisplayUsage(False, True)
      SetStatusText(LOG_GetLast.ToString("g"), "Please connect to the Internet.", True)
    Else
      mySettings.AccountType = HostType
      ScreenDefaultColors(mySettings.Colors, mySettings.AccountType)
      mySettings.Save()
      SetStatusText(LOG_GetLast.ToString("g"), "Preparing Connection...", False)
      If localData IsNot Nothing Then
        localData.Dispose()
        localData = Nothing
      End If
      localData = New localRestrictionTracker(LocalAppDataDirectory)
    End If
  End Sub
#End Region
#Region "Form Events"
  Private Sub frmMain_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    AddHandler Microsoft.Win32.SystemEvents.PowerModeChanged, AddressOf PowerModeChanged
    If mySettings Is Nothing Then ReLoadSettings()
    NextGrabTick = Long.MinValue
    Me.Opacity = 0
    Me.ShowInTaskbar = False
    Me.Size = mySettings.MainSize
    ControlService(False)
    tmrIcoDelay.Interval = 100
    tmrIcoDelay.Enabled = True
    DisplayResults(0, 0, 0, 0)
    EnableProgressIcon()
  End Sub
  Private Sub tmrIcoDelay_Tick(sender As System.Object, e As System.EventArgs) Handles tmrIcoDelay.Tick
    tmrIcoDelay.Enabled = False
    Dim shellTrayWnd As IntPtr = NativeMethods.FindWindow("Shell_TrayWnd", String.Empty)
    If shellTrayWnd.Equals(IntPtr.Zero) Then
      tmrIcoDelay.Interval = 2000
      tmrIcoDelay.Enabled = True
    Else
      Dim trayNotifyWnd As IntPtr = NativeMethods.FindWindowEx(shellTrayWnd, 0, "TrayNotifyWnd", String.Empty)
      If trayNotifyWnd.Equals(IntPtr.Zero) Then
        tmrIcoDelay.Interval = 1000
        tmrIcoDelay.Enabled = True
      Else
        Dim sysPagerWnd As IntPtr = NativeMethods.FindWindowEx(trayNotifyWnd, 0, "SysPager", String.Empty)
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
              If Me.WindowState = FormWindowState.Minimized Or Not Me.Visible Then trayIcon.Visible = True
            Else
              If mySettings.AutoHide Then trayIcon.Visible = True
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
          If (Not mySettings.TrayIconAnimation) And (Not mySettings.TrayIconStyle = AppSettings.TrayStyles.Never) Then
            Me.Hide()
            Me.WindowState = FormWindowState.Minimized
          ElseIf (Not mySettings.TrayIconStyle = AppSettings.TrayStyles.Never) Then
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
        pctTypeADld.Image = DisplayProgress(pctTypeADld.DisplayRectangle.Size, 0, 0, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
        pctTypeAUld.Image = DisplayProgress(pctTypeAUld.DisplayRectangle.Size, 0, 0, mySettings.Accuracy, mySettings.Colors.MainUpA, mySettings.Colors.MainUpB, mySettings.Colors.MainUpC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
        Dim lookupInvoker As New MethodInvoker(AddressOf LookupProvider)
        lookupInvoker.BeginInvoke(Nothing, Nothing)
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
    NativeMethods.InsertMenu(hSysMenu, 2, NativeMethods.MenuFlags.MF_SEPARATOR Or NativeMethods.MenuFlags.MF_BYPOSITION, 0, String.Empty)
  End Sub
  Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
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
          Case NativeMethods.SC_MINIMIZE
            If Not mySettings.TrayIconStyle = AppSettings.TrayStyles.Never And mySettings.TrayIconAnimation Then m.Result = New IntPtr(-1)
        End Select
      Case NativeMethods.WM_WINDOWPOSCHANGING
        Dim wndPos As NativeMethods.WINDOWPOS = m.GetLParam(GetType(NativeMethods.WINDOWPOS))
        If CBool((wndPos.Flags And NativeMethods.WINDOWPOS_FLAGS.SWP_STATECHANGED) = NativeMethods.WINDOWPOS_FLAGS.SWP_STATECHANGED) And wndPos.X = -32000 And wndPos.Y = -32000 Then
          If Not mySettings.TrayIconStyle = AppSettings.TrayStyles.Never And mySettings.TrayIconAnimation Then
            Me.Opacity = 0
            If Me.Visible And mySettings.TrayIconAnimation Then AnimateWindow(Me, True)
            If mySettings.TrayIconStyle = AppSettings.TrayStyles.Minimized Then trayIcon.Visible = True
            mnuRestore.Text = "&Restore"
            Me.Hide()
            Me.Opacity = 1
            wndPos.Flags = NativeMethods.WINDOWPOS_FLAGS.SWP_NOMOVE Or NativeMethods.WINDOWPOS_FLAGS.SWP_NOSIZE Or NativeMethods.WINDOWPOS_FLAGS.SWP_NOACTIVATE Or NativeMethods.WINDOWPOS_FLAGS.SWP_NOSENDCHANGING
            System.Runtime.InteropServices.Marshal.StructureToPtr(wndPos, m.LParam, True)
          End If
        End If
    End Select
    MyBase.WndProc(m)
  End Sub
  Private Sub HideLater()
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New MethodInvoker(AddressOf HideLater))
      Catch ex As Exception
      End Try
      Return
    End If
    Threading.Thread.Sleep(100)
    Me.Hide()
    Me.Opacity = 1
  End Sub
  Private Sub frmMain_SizeChanged(sender As Object, e As System.EventArgs) Handles Me.SizeChanged
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(AddressOf frmMain_SizeChanged), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    If Me.WindowState = FormWindowState.Minimized Then
      If Not mySettings.TrayIconAnimation Then
        If mySettings.TrayIconStyle = AppSettings.TrayStyles.Always Then
          Me.Hide()
        ElseIf mySettings.TrayIconStyle = AppSettings.TrayStyles.Minimized Then
          trayIcon.Visible = True
          Me.Hide()
        End If
      End If
      mnuRestore.Text = "&Restore"
    Else
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
      If myPanel = SatHostTypes.Other Then
        lblRRS.Font = pnlDetails.Font
        lblNothing.Font = New Font(Me.Font.FontFamily, pnlDetails.Font.Size * 2.5, Me.Font.Style, Me.Font.Unit, Me.Font.GdiCharSet, Me.Font.GdiVerticalFont)
      End If
      If myState = LoadStates.Loaded Then
        If Me.WindowState = FormWindowState.Normal Then mySettings.MainSize = Me.Size
      ElseIf Not myPanel = SatHostTypes.Other Then
        lblRRS.Font = pnlDetails.Font
        lblNothing.Font = New Font(Me.Font.FontFamily, pnlDetails.Font.Size * 2.5, Me.Font.Style, Me.Font.Unit, Me.Font.GdiCharSet, Me.Font.GdiVerticalFont)
      End If
      For i As Integer = 1 To 2
        If (lblStatus.Height / 2) - (pctNetTest.Height / 2) > 0 Then
          pctNetTest.Top = (lblStatus.Height / 2) - (pctNetTest.Height / 2)
        Else
          pctNetTest.Top = 0
        End If
        If pnlTypeA.Visible Then
          If pctNetTest.Bottom > pnlTypeA.Top - 1 Then
            pctNetTest.Height = pnlTypeA.Top - 1 - pctNetTest.Top
            pctNetTest.Width = pctNetTest.Height
          Else
            pctNetTest.Height = 16
            pctNetTest.Width = pctNetTest.Height
          End If
        ElseIf pnlTypeB.Visible Then
          If pctNetTest.Bottom > pnlTypeB.Top - 1 Then
            pctNetTest.Height = pnlTypeB.Top - 1 - pctNetTest.Top
            pctNetTest.Width = pctNetTest.Height
          Else
            pctNetTest.Height = 16
            pctNetTest.Width = pctNetTest.Height
          End If
        End If
        pctNetTest.Left = gbUsage.Right - 16 - pctNetTest.Width
      Next
    End If
  End Sub
  Private Sub ResizePanels()
    Dim trayIcoVal As Icon = Nothing
    If myPanel = SatHostTypes.WildBlue_LEGACY Or myPanel = SatHostTypes.RuralPortal_LEGACY Or myPanel = SatHostTypes.DishNet_EXEDE Then
      If typeA_dlim = 0 And typeA_ulim = 0 Then
        pctTypeADld.Image = DisplayProgress(pctTypeADld.DisplayRectangle.Size, 0, 0, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
        pctTypeAUld.Image = DisplayProgress(pctTypeAUld.DisplayRectangle.Size, 0, 0, mySettings.Accuracy, mySettings.Colors.MainUpA, mySettings.Colors.MainUpB, mySettings.Colors.MainUpC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
        trayIcoVal = MakeIcon(IconName.norm)
      Else
        pctTypeADld.Image = DisplayProgress(pctTypeADld.DisplayRectangle.Size, typeA_down, typeA_dlim, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
        pctTypeAUld.Image = DisplayProgress(pctTypeAUld.DisplayRectangle.Size, typeA_up, typeA_ulim, mySettings.Accuracy, mySettings.Colors.MainUpA, mySettings.Colors.MainUpB, mySettings.Colors.MainUpC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
        trayIcoVal = CreateTypeATrayIcon(typeA_down, typeA_dlim, typeA_up, typeA_ulim)
      End If
    ElseIf myPanel = SatHostTypes.RuralPortal_EXEDE Or myPanel = SatHostTypes.WildBlue_EXEDE Then
      If typeB_lim = 0 Then
        pctTypeB.Image = DisplayRProgress(pctTypeB.DisplayRectangle.Size, 0, 1, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
        trayIcoVal = MakeIcon(IconName.norm)
      Else
        pctTypeB.Image = DisplayRProgress(pctTypeB.DisplayRectangle.Size, typeB_used, typeB_lim, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
        trayIcoVal = CreateTypeBTrayIcon(typeB_used, typeB_lim)
      End If
    ElseIf myPanel = SatHostTypes.Other Then
      lblNothing.Text = My.Application.Info.ProductName
      lblRRS.Text = "by " & Application.CompanyName
      ttUI.SetToolTip(lblRRS, "Visit realityripple.com.")
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
        Me.WindowState = FormWindowState.Minimized
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
    If remoteData IsNot Nothing Then
      remoteData.Dispose()
      remoteData = Nothing
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
        cSave.PassCrypt = StoredPassword.EncryptLogger(StoredPassword.DecryptApp(mySettings.PassCrypt))
      End If
      cSave.Proxy = mySettings.Proxy
      cSave.Timeout = mySettings.Timeout
      cSave.Save()
    End If
    If My.Computer.FileSystem.FileExists(AppDataPath & "del.bat") Then My.Computer.FileSystem.DeleteFile(AppDataPath & "del.bat")
    Select Case e.CloseReason
      Case CloseReason.WindowsShutDown
      Case CloseReason.ApplicationExitCall
      Case Else : ControlService(True)
    End Select
  End Sub
  Private Sub lblRRS_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblRRS.LinkClicked
    Try
      Process.Start("http://realityripple.com")
    Catch ex As Exception
      MakeNotifier(taskNotifier, False)
      If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", My.Application.Info.ProductName & " could not navigate to ""realityripple.com""!" & vbNewLine & ex.Message, 200, 3000, 100)
    End Try
  End Sub
  Private Sub pctNetTest_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles pctNetTest.KeyUp
    If e.KeyCode = Keys.Space Or e.KeyCode = Keys.Return Then
      Try
        If mySettings.NetTestURL.Contains("://") Then
          Process.Start(mySettings.NetTestURL)
        Else
          Process.Start("http://" & mySettings.NetTestURL)
        End If
      Catch ex As Exception
        MakeNotifier(taskNotifier, False)
        If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", My.Application.Info.ProductName & " could not navigate to """ & mySettings.NetTestURL & """!" & vbNewLine & ex.Message, 200, 3000, 100)
      End Try
    End If
  End Sub
  Private Sub pctNetTest_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pctNetTest.MouseClick
    If e.Button = Windows.Forms.MouseButtons.Left Then
      Try
        If mySettings.NetTestURL.Contains("://") Then
          Process.Start(mySettings.NetTestURL)
        Else
          Process.Start("http://" & mySettings.NetTestURL)
        End If
      Catch ex As Exception
        MakeNotifier(taskNotifier, False)
        If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", My.Application.Info.ProductName & " could not navigate to """ & mySettings.NetTestURL & """!" & vbNewLine & ex.Message, 200, 3000, 100)
      End Try
    End If
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
        If bDown And (Key = Keys.F1) Then
          Try
            Process.Start("http://srt.realityripple.com/faq.php")
          Catch ex As Exception
            MakeNotifier(taskNotifier, False)
            If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", My.Application.Info.ProductName & " could not navigate to ""srt.realityripple.com/faq.php""!" & vbNewLine & ex.Message, 200, 3000, 100)
          End Try
        End If
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
        If String.IsNullOrEmpty(mySettings.RemoteKey) Then
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
          Net.ServicePointManager.SecurityProtocol = SecurityProtocolTypeEx.None
        End If
      Else
        Try
          Net.ServicePointManager.SecurityProtocol = useProtocol
        Catch ex As Exception
        End Try
      End If
    End If
    If LocalAppDataDirectory = Application.StartupPath & "\Config\" Then mySettings.HistoryDir = Application.StartupPath & "\Config\"
    ScreenDefaultColors(mySettings.Colors, mySettings.AccountType)
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
      trayIcon.Visible = Me.WindowState = FormWindowState.Minimized Or Not Me.Visible
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
        Dim wsFavicon As New clsFavicon(mySettings.NetTestURL, AddressOf wsFavicon_DownloadIconCompleted, mySettings.NetTestURL)
      End If
      Dim sNetTestTitle As String = mySettings.NetTestURL
      If sNetTestTitle.Contains("://") Then sNetTestTitle = sNetTestTitle.Substring(sNetTestTitle.IndexOf("://") + 3)
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
    If remoteData IsNot Nothing Then
      remoteData.Dispose()
      remoteData = Nothing
    End If
    Me.Invoke(New MethodInvoker(AddressOf InitAccount))
    If Not String.IsNullOrEmpty(sAccount) Then
      Me.Invoke(New MethodInvoker(AddressOf EnableProgressIcon))
      SetStatusText("Reloading", "Reloading History...", False)
      LOG_Initialize(sAccount, False)
      If ClosingTime Then Return
      DisplayUsage(False, False)
      SetStatusText(LOG_GetLast.ToString("g"), "Preparing Connection...", False)
      Dim UsageInvoker As New MethodInvoker(AddressOf GetUsage)
      UsageInvoker.BeginInvoke(Nothing, Nothing)
    End If
    If ClosingTime Then Return
    Dim sizeChangeInvoker As New EventHandler(AddressOf frmMain_SizeChanged)
    sizeChangeInvoker.BeginInvoke(Me, New EventArgs, Nothing, Nothing)
  End Sub
  Private Sub EnableProgressIcon()
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
  Private Sub LookupProvider()
    SetTag(LoadStates.Lookup)
    SetStatusText("Loading History", "Reading usage history into memory...", False)
    LOG_Initialize(sAccount, False)
    If ClosingTime Then Return
    If mySettings.AccountType = SatHostTypes.Other Then
      If mySettings.AccountTypeForced Then
        SetStatusText(LOG_GetLast.ToString("g"), "Unknown Account Type.", True)
      Else
        SetStatusText("Analyzing Account", "Determining your account type...", False)
        Dim TypeDetermination As New DetermineType(sProvider, mySettings.Timeout, mySettings.Proxy, AddressOf TypeDetermination_TypeDetermined)
      End If
    Else
      iconStop = True
      SetStatusText("No History", String.Empty, False)
      DisplayUsage(True, False)
      Dim TimerInvoker As New MethodInvoker(AddressOf StartTimer)
      TimerInvoker.BeginInvoke(Nothing, Nothing)
    End If
  End Sub
  Private Sub InitAccount()
    sAccount = mySettings.Account
    If Not String.IsNullOrEmpty(mySettings.PassCrypt) Then
      sPassword = StoredPassword.DecryptApp(mySettings.PassCrypt)
    End If
    If Not String.IsNullOrEmpty(sAccount) AndAlso (sAccount.Contains("@") And sAccount.Contains(".")) Then
      sProvider = sAccount.Substring(sAccount.LastIndexOf("@") + 1).ToLower
    Else
      sAccount = String.Empty
      sProvider = String.Empty
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
            If String.IsNullOrEmpty(sProvider) Then
              sProvider = sAccount.Substring(sAccount.LastIndexOf("@") + 1).ToLower
              SetStatusText("Reloading", "Reloading History...", False)
              LOG_Initialize(sAccount, False)
              If ClosingTime Then Return
            End If
            If Math.Abs(DateDiff(DateInterval.Minute, LOG_GetLast, Now)) >= 10 Then
              If Not String.IsNullOrEmpty(sProvider) And Not String.IsNullOrEmpty(sPassword) Then
                NextGrabTick = Long.MaxValue
                PauseActivity = "Preparing Connection"
                EnableProgressIcon()
                SetStatusText(LOG_GetLast.ToString("g"), "Preparing Connection...", False)
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
    If String.IsNullOrEmpty(sAccount) Or String.IsNullOrEmpty(sPassword) Or Not sAccount.Contains("@") Then
      If mySettings.TrayIconStyle = AppSettings.TrayStyles.Never Then
        If Me.WindowState = FormWindowState.Minimized Then
          Me.WindowState = FormWindowState.Normal
          mnuRestore.Text = "&Focus"
        End If
      Else
        If Not Me.Visible Then
          Me.Location = New Point((Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2)
          If mySettings.TrayIconAnimation Then AnimateWindow(Me, False)
          Me.Show()
          mnuRestore.Text = "&Focus"
        End If
      End If
      cmdConfig.Focus()
      MsgDlg(Me, "Please enter your account details in the Config window by clicking Configuration.", "You haven't entered your account details.", "Account Details Required", MessageBoxButtons.OK, _TaskDialogIcon.User, MessageBoxIcon.Warning)
    Else
      If cmdRefresh.Enabled Then
        cmdRefresh.Enabled = False
        NextGrabTick = srlFunctions.TickCount() + (mySettings.Interval * 60 * 1000)
        If KeyCheck(mySettings.RemoteKey) Then
          Dim remoteCallback As New MethodInvoker(AddressOf GetRemoteUsage)
          remoteCallback.BeginInvoke(Nothing, Nothing)
        Else
          If localData IsNot Nothing Then
            localData.Dispose()
            localData = Nothing
          End If
          localData = New localRestrictionTracker(LocalAppDataDirectory)
        End If
      Else
        If remoteData IsNot Nothing Then
          remoteData.Dispose()
          remoteData = Nothing
        End If
        If localData IsNot Nothing Then
          localData.Dispose()
          localData = Nothing
        End If
        Application.DoEvents()
        SetStatusText(LOG_GetLast.ToString("g"), "Restarting Connection in 5 Seconds...", False)
        DisplayUsage(False, False)
        NextGrabTick = srlFunctions.TickCount() + 5000
      End If
    End If
  End Sub
  Private Function KeyCheck(TestKey As String) As Boolean
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
  Private Sub GetRemoteUsage()
    If remoteData IsNot Nothing Then
      remoteData.Dispose()
      remoteData = Nothing
    End If
    Dim fromDate = mySettings.LastSyncTime
    If My.Computer.Keyboard.CtrlKeyDown Then fromDate = New Date(2000, 1, 1)
    If LOG_GetCount() = 0 Then fromDate = New Date(2000, 1, 1)
    remoteData = New remoteRestrictionTracker(sAccount, sPassword, mySettings.RemoteKey, mySettings.Proxy, mySettings.Timeout, fromDate, LocalAppDataDirectory)
  End Sub
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
      Dim lDown As Long
      Dim lDLim As Long
      Dim lUp As Long
      Dim lULim As Long
      LOG_Get(LOG_GetCount() - 1, dtDate, lDown, lDLim, lUp, lULim)
      If bStatusText Then SetStatusText(dtDate.ToString("g"), String.Empty, False)
      DisplayResults(lDown, lDLim, lUp, lULim)
    End If
  End Sub
  Private Sub SetNextLoginTime(Optional MinutesAhead As Integer = -1)
    If localData IsNot Nothing Then
      localData.Dispose()
      localData = Nothing
    End If
    If remoteData IsNot Nothing Then
      remoteData.Dispose()
      remoteData = Nothing
    End If
    If MinutesAhead > -1 Then
      NextGrabTick = srlFunctions.TickCount() + (MinutesAhead * 60 * 1000)
    Else
      NextGrabTick = srlFunctions.TickCount() + (mySettings.Interval * 60 * 1000)
    End If
  End Sub
#End Region
#Region "Local Usage Events"
  Private Sub localData_ConnectionStatus(sender As Object, e As ConnectionStatusEventArgs) Handles localData.ConnectionStatus
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(AddressOf localData_ConnectionStatus), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    NextGrabTick = srlFunctions.TickCount() + ((mySettings.Timeout + 15) * 1000)
    Select Case e.Status
      Case ConnectionStates.Initialize : SetStatusText(LOG_GetLast.ToString("g"), "Initializing Connection...", False)
      Case ConnectionStates.Prepare : SetStatusText(LOG_GetLast.ToString("g"), "Preparing to Log In...", False)
      Case ConnectionStates.Login
        Select Case e.SubState
          Case ConnectionSubStates.ReadLogin : SetStatusText(LOG_GetLast.ToString("g"), "Reading Login Page...", False)
          Case ConnectionSubStates.AuthPrepare : SetStatusText(LOG_GetLast.ToString("g"), "Preparing Authentication...", False)
          Case ConnectionSubStates.Authenticate : SetStatusText(LOG_GetLast.ToString("g"), "Authenticating...", False)
          Case ConnectionSubStates.AuthenticateRetry
            If e.Stage < 1 Then
              SetStatusText(LOG_GetLast.ToString("g"), "Re-Authenticating...", False)
            Else
              SetStatusText(LOG_GetLast.ToString("g"), "Re-Authenticating (Attempt " & e.Stage & ")...", False)
            End If
          Case ConnectionSubStates.Verify : SetStatusText(LOG_GetLast.ToString("g"), "Verifying Authentication...", False)
          Case Else : SetStatusText(LOG_GetLast.ToString("g"), "Logging In...", False)
        End Select
      Case ConnectionStates.TableDownload
        Select Case e.SubState
          Case ConnectionSubStates.LoadHome : SetStatusText(LOG_GetLast.ToString("g"), "Downloading Home Page...", False)
          Case ConnectionSubStates.LoadAJAX : SetStatusText(LOG_GetLast.ToString("g"), "Downloading AJAX Data (" & e.Stage & " of " & localData.ExedeAJAXFirstTryRequests & ")...", False)
          Case ConnectionSubStates.LoadAJAXRetry : SetStatusText(LOG_GetLast.ToString("g"), "Re-Downloading AJAX Data (" & e.Stage & " of " & localData.ExedeAJAXSecondTryRequests & ")...", False)
          Case ConnectionSubStates.LoadTable : SetStatusText(LOG_GetLast.ToString("g"), "Downloading Usage Table...", False)
          Case ConnectionSubStates.LoadTableRetry : SetStatusText(LOG_GetLast.ToString("g"), "Re-Downloading Usage Table...", False)
          Case Else : SetStatusText(LOG_GetLast.ToString("g"), "Downloading Usage Table...", False)
        End Select
      Case ConnectionStates.TableRead : SetStatusText(LOG_GetLast.ToString("g"), "Reading Usage Table...", False)
    End Select
  End Sub
  Private Sub localData_ConnectionFailure(sender As Object, e As ConnectionFailureEventArgs) Handles localData.ConnectionFailure
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(AddressOf localData_ConnectionFailure), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    Select Case e.Type
      Case ConnectionFailureEventArgs.FailureType.LoginIssue
        SetStatusText(LOG_GetLast.ToString("g"), e.Message, True)
      Case ConnectionFailureEventArgs.FailureType.ConnectionTimeout
        SetStatusText(LOG_GetLast.ToString("g"), "Connection Timed Out!", True)
        DisplayUsage(False, False)
      Case ConnectionFailureEventArgs.FailureType.LoginFailure
        If e.Message = "TLS ERROR" Then
          If (Environment.OSVersion.Version.Major < 6 Or (Environment.OSVersion.Version.Major = 6 And Environment.OSVersion.Version.Minor = 0)) Then
            SetStatusText(LOG_GetLast.ToString("g"), "Security Protocol not supported on this Operating System. Please use the TLS Proxy feature for now.", True)
          ElseIf (Environment.Version.Major = 4 And Environment.Version.Minor = 0 And Environment.Version.Build = 30319 And Environment.Version.Revision < 17929) Then
            SetStatusText(LOG_GetLast.ToString("g"), "Security Protocol requires .NET Framework 4.5 or newer.", True)
          Else
            SetStatusText(LOG_GetLast.ToString("g"), "Security Protocol not supported for some reason. Please use the TLS Proxy feature for now. Also, let me know you got this message.", True)
          End If
        ElseIf e.Message.StartsWith("POSSIBLE TLS ERROR - ") Then
          Dim sMessage As String = e.Message.Substring(21)
          If (Environment.OSVersion.Version.Major < 6 Or (Environment.OSVersion.Version.Major = 6 And Environment.OSVersion.Version.Minor = 0)) Then
            SetStatusText(LOG_GetLast.ToString("g"), "Security Protocol not supported on this Operating System. Please use the TLS Proxy feature for now." & vbNewLine & sMessage, True)
          ElseIf (Environment.Version.Major = 4 And Environment.Version.Minor = 0 And Environment.Version.Build = 30319 And Environment.Version.Revision < 17929) Then
            SetStatusText(LOG_GetLast.ToString("g"), "Security Protocol requires .NET Framework 4.5 or newer." & vbNewLine & sMessage, True)
          Else
            SetStatusText(LOG_GetLast.ToString("g"), "Security Protocol not supported for some reason. Please use the TLS Proxy feature for now. Also, let me know you got this message." & vbNewLine & sMessage, True)
          End If
        Else
          SetStatusText(LOG_GetLast.ToString("g"), e.Message, True)
        End If
        If Not String.IsNullOrEmpty(e.Fail) Then FailFile(e.Fail)
        DisplayUsage(False, True)
      Case ConnectionFailureEventArgs.FailureType.FatalLoginFailure
        If Not mySettings.AccountTypeForced Then mySettings.AccountType = SatHostTypes.Other
        SetStatusText(LOG_GetLast.ToString("g"), e.Message, True)
        If Not String.IsNullOrEmpty(e.Fail) Then FailFile(e.Fail)
        DisplayUsage(False, False)
      Case ConnectionFailureEventArgs.FailureType.UnknownAccountDetails
        If mySettings.TrayIconStyle = AppSettings.TrayStyles.Never Then
          If Me.WindowState = FormWindowState.Minimized Then
            Me.WindowState = FormWindowState.Normal
            mnuRestore.Text = "&Focus"
          End If
        Else
          If Not Me.Visible Then
            Me.Location = New Point((Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2)
            If mySettings.TrayIconAnimation Then
              AnimateWindow(Me, False)
            Else
              Me.WindowState = FormWindowState.Normal
            End If
            Me.Show()
            mnuRestore.Text = "&Focus"
          End If
        End If
        cmdConfig.Focus()
        MsgDlg(Me, "Please enter your account details in the Config window by clicking Configuration.", "You haven't entered your account details.", "Account Details Required", MessageBoxButtons.OK, _TaskDialogIcon.User, MessageBoxIcon.Error)
      Case ConnectionFailureEventArgs.FailureType.UnknownAccountType
        If mySettings.AccountTypeForced Then
          SetStatusText(LOG_GetLast.ToString("g"), "Unknown Account Type.", True)
        Else
          SetStatusText("Analyzing Account", "Determining your account type...", False)
          Dim TypeDetermination As New DetermineType(sProvider, mySettings.Timeout, mySettings.Proxy, AddressOf TypeDetermination_TypeDetermined)
        End If
    End Select
    If localData IsNot Nothing Then
      localData.Dispose()
      localData = Nothing
    End If
  End Sub
  Private Sub localData_ConnectionDNXResult(sender As Object, e As TYPEA2ResultEventArgs) Handles localData.ConnectionDNXResult
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(AddressOf localData_ConnectionDNXResult), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    SetStatusText(e.Update.ToString("g"), "Saving History...", False)
    NextGrabTick = srlFunctions.TickCount() + (mySettings.Interval * 60 * 1000)
    LOG_Add(e.Update, e.AnyTime, e.AnyTimeLimit, e.OffPeak, e.OffPeakLimit, True)
    myPanel = SatHostTypes.DishNet_EXEDE
    If Not mySettings.AccountTypeForced Then mySettings.AccountType = SatHostTypes.DishNet_EXEDE
    mySettings.Save()
    ScreenDefaultColors(mySettings.Colors, mySettings.AccountType)
    If e.SlowedDetected Then imSlowed = True
    imFree = e.FreeDetected
    DisplayUsage(True, True)
    If localData IsNot Nothing Then
      localData.Dispose()
      localData = Nothing
    End If
    Dim hostInvoker As New MethodInvoker(AddressOf SaveToHostList)
    hostInvoker.BeginInvoke(Nothing, Nothing)
  End Sub
  Private Sub localData_ConnectionRPXResult(sender As Object, e As TYPEBResultEventArgs) Handles localData.ConnectionRPXResult
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(AddressOf localData_ConnectionRPXResult), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    SetStatusText(e.Update.ToString("g"), "Saving History...", False)
    NextGrabTick = srlFunctions.TickCount() + (mySettings.Interval * 60 * 1000)
    LOG_Add(e.Update, e.Used, e.Limit, e.Used, e.Limit, True)
    myPanel = SatHostTypes.RuralPortal_EXEDE
    If Not mySettings.AccountTypeForced Then mySettings.AccountType = SatHostTypes.RuralPortal_EXEDE
    mySettings.Save()
    ScreenDefaultColors(mySettings.Colors, mySettings.AccountType)
    If e.SlowedDetected Then imSlowed = True
    imFree = e.FreeDetected
    DisplayUsage(True, True)
    If localData IsNot Nothing Then
      localData.Dispose()
      localData = Nothing
    End If
    Dim hostInvoker As New MethodInvoker(AddressOf SaveToHostList)
    hostInvoker.BeginInvoke(Nothing, Nothing)
  End Sub
  Private Sub localData_ConnectionRPLResult(sender As Object, e As TYPEAResultEventArgs) Handles localData.ConnectionRPLResult
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(AddressOf localData_ConnectionRPLResult), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    SetStatusText(e.Update.ToString("g"), "Saving History...", False)
    NextGrabTick = srlFunctions.TickCount() + (mySettings.Interval * 60 * 1000)
    LOG_Add(e.Update, e.Download, e.DownloadLimit, e.Upload, e.UploadLimit, True)
    myPanel = SatHostTypes.RuralPortal_LEGACY
    If Not mySettings.AccountTypeForced Then mySettings.AccountType = SatHostTypes.RuralPortal_LEGACY
    mySettings.Save()
    ScreenDefaultColors(mySettings.Colors, mySettings.AccountType)
    If e.SlowedDetected Then imSlowed = True
    imFree = e.FreeDetected
    DisplayUsage(True, True)
    If localData IsNot Nothing Then
      localData.Dispose()
      localData = Nothing
    End If
    Dim hostInvoker As New MethodInvoker(AddressOf SaveToHostList)
    hostInvoker.BeginInvoke(Nothing, Nothing)
  End Sub
  Private Sub localData_ConnectionWBLResult(sender As Object, e As TYPEAResultEventArgs) Handles localData.ConnectionWBLResult
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(AddressOf localData_ConnectionWBLResult), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    SetStatusText(e.Update.ToString("g"), "Saving History...", False)
    NextGrabTick = srlFunctions.TickCount() + (mySettings.Interval * 60 * 1000)
    LOG_Add(e.Update, e.Download, e.DownloadLimit, e.Upload, e.UploadLimit, True)
    myPanel = SatHostTypes.WildBlue_LEGACY
    If Not mySettings.AccountTypeForced Then mySettings.AccountType = SatHostTypes.WildBlue_LEGACY
    mySettings.Save()
    ScreenDefaultColors(mySettings.Colors, mySettings.AccountType)
    If e.SlowedDetected Then imSlowed = True
    imFree = e.FreeDetected
    DisplayUsage(True, True)
    If localData IsNot Nothing Then
      localData.Dispose()
      localData = Nothing
    End If
    Dim hostInvoker As New MethodInvoker(AddressOf SaveToHostList)
    hostInvoker.BeginInvoke(Nothing, Nothing)
  End Sub
  Private Sub localData_ConnectionWBXResult(sender As Object, e As TYPEBResultEventArgs) Handles localData.ConnectionWBXResult
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(AddressOf localData_ConnectionWBXResult), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    SetStatusText(e.Update.ToString("g"), "Saving History...", False)
    NextGrabTick = srlFunctions.TickCount() + (mySettings.Interval * 60 * 1000)
    LOG_Add(e.Update, e.Used, e.Limit, e.Used, e.Limit, True)
    myPanel = SatHostTypes.WildBlue_EXEDE
    If Not mySettings.AccountTypeForced Then mySettings.AccountType = SatHostTypes.WildBlue_EXEDE
    mySettings.Save()
    ScreenDefaultColors(mySettings.Colors, mySettings.AccountType)
    If e.SlowedDetected Then imSlowed = True
    imFree = e.FreeDetected
    DisplayUsage(True, True)
    If localData IsNot Nothing Then
      localData.Dispose()
      localData = Nothing
    End If
    Dim hostInvoker As New MethodInvoker(AddressOf SaveToHostList)
    hostInvoker.BeginInvoke(Nothing, Nothing)
  End Sub
#Region "Host List"
  Private didHostListSave As Boolean = False
  Private Sub SaveToHostList()
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New MethodInvoker(AddressOf SaveToHostList))
      Catch ex As Exception
      End Try
      Return
    End If
    If didHostListSave Then Return
    Try
      Dim myProvider As String = mySettings.Account.Substring(mySettings.Account.LastIndexOf("@") + 1).ToLower
      Dim sckHostList As Net.WebRequest = Net.HttpWebRequest.Create("http://wb.realityripple.com/hosts/?add=" & myProvider)
      sckHostList.BeginGetResponse(Nothing, Nothing)
      didHostListSave = True
    Catch ex As Exception
      didHostListSave = False
    End Try
  End Sub
#End Region
#End Region
#Region "Remote Usage Events"
  Private Sub remoteData_Failure(sender As Object, e As remoteRestrictionTracker.FailureEventArgs) Handles remoteData.Failure
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(AddressOf remoteData_Failure), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    Dim sErr As String = "There was an error verifying your Product Key."
    Select Case e.Type
      Case remoteRestrictionTracker.FailureEventArgs.FailType.BadLogin
        sErr = "There was a server error. Please try again later."
      Case remoteRestrictionTracker.FailureEventArgs.FailType.BadPassword
        sErr = "Your Password is incorrect."
      Case remoteRestrictionTracker.FailureEventArgs.FailType.BadProduct
        sErr = "Your Product Key has been disabled."
        mySettings.RemoteKey = String.Empty
        Dim UsageInvoker As New MethodInvoker(AddressOf GetUsage)
        UsageInvoker.BeginInvoke(Nothing, Nothing)
      Case remoteRestrictionTracker.FailureEventArgs.FailType.BadServer
        sErr = "There was a fault double-checking the server. You may have a security issue."
      Case remoteRestrictionTracker.FailureEventArgs.FailType.NoData
        sErr = "There is no usage data." & IIf(String.IsNullOrEmpty(e.Details), "Please wait 15 minutes.", " " & e.Details)
      Case remoteRestrictionTracker.FailureEventArgs.FailType.NoPassword
        sErr = "Your Password has not been Registered on the Remote Service."
      Case remoteRestrictionTracker.FailureEventArgs.FailType.NoUsername
        sErr = "Your Account is not Registered for the Remote Service."
        mySettings.RemoteKey = String.Empty
        Dim UsageInvoker As New MethodInvoker(AddressOf GetUsage)
        UsageInvoker.BeginInvoke(Nothing, Nothing)
      Case remoteRestrictionTracker.FailureEventArgs.FailType.Network
        sErr = "Network Connection Error" & IIf(String.IsNullOrEmpty(e.Details), ".", " (" & e.Details & ")")
      Case remoteRestrictionTracker.FailureEventArgs.FailType.NotBase64
        sErr = "The server did not respond in the right manner. Please check your Internet connection." & IIf(String.IsNullOrEmpty(e.Details), "", vbNewLine & e.Details)
    End Select
    If remoteData IsNot Nothing Then
      remoteData.Dispose()
      remoteData = Nothing
    End If
    SetStatusText(LOG_GetLast.ToString("g"), "Service Failure: " & sErr, True)
    DisplayUsage(False, True)
  End Sub
  Private Sub remoteData_OKKey(sender As Object, e As System.EventArgs) Handles remoteData.OKKey
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(AddressOf remoteData_OKKey), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    NextGrabTick = srlFunctions.TickCount() + (mySettings.Interval * 60 * 1000)
    SetStatusText(LOG_GetLast.ToString("g"), "Account Accessed! Getting Usage...", False)
  End Sub
  Private Sub remoteData_Success(sender As Object, e As remoteRestrictionTracker.SuccessEventArgs) Handles remoteData.Success
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(AddressOf remoteData_Success), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    NextGrabTick = srlFunctions.TickCount() + (mySettings.Interval * 60 * 1000)
    Dim LastTime As String = LOG_GetLast.ToString("g")
    If FullCheck Then
      SetStatusText(LastTime, "Synchronizing History...", False)
    Else
      SetStatusText(LastTime, "Saving History...", False)
    End If
    If e IsNot Nothing Then
      If Not mySettings.AccountTypeForced Then mySettings.AccountType = e.Provider
      ScreenDefaultColors(mySettings.Colors, mySettings.AccountType)
      mySettings.Save()
      Dim iPercent As Integer = 0
      Dim iInterval As Integer = 1
      Dim iStart As Long = srlFunctions.TickCount()
      ttUI.UseFading = False
      For I As Integer = 0 To e.Results.Length - 1
        Dim Row As remoteRestrictionTracker.SuccessEventArgs.Result = e.Results(I)
        If FullCheck Then
          If Math.Abs(iPercent - Math.Floor((I / (e.Results.Length - 1)) * 100)) >= iInterval Then
            iPercent = Math.Floor((I / (e.Results.Length - 1)) * 100)
            SetStatusText(LastTime, "Synchronizing History [" & iPercent & "%]...", False)
            If (iPercent = 4) Then
              Dim iDur As Long = srlFunctions.TickCount() - iStart
              If iDur <= 700 Then iInterval = 2
            End If
          End If
          LOG_Add(Row.Time, Row.Down, Row.DownMax, Row.Up, Row.UpMax, (I = e.Results.Length - 1))
        Else
          If DateDiff(DateInterval.Minute, LOG_GetLast, Row.Time) > 1 Then
            LOG_Add(Row.Time, Row.Down, Row.DownMax, Row.Up, Row.UpMax, (I = e.Results.Length - 1))
          End If
        End If
      Next
      ttUI.UseFading = True
      FullCheck = False
      mySettings.LastSyncTime = LOG_GetLast()
      mySettings.Save()
      DisplayUsage(True, True)
    Else
      If LOG_GetCount() = 0 Then
        SetStatusText("No History", "No data received from the server!", True)
      End If
      DisplayUsage(True, True)
    End If
    If remoteData IsNot Nothing Then
      remoteData.Dispose()
      remoteData = Nothing
    End If
    Dim hostInvoker As New MethodInvoker(AddressOf SaveToHostList)
    hostInvoker.BeginInvoke(Nothing, Nothing)
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
    Select Case state
      Case "TYPEA"
        Dim lDown As Long = typeA_down
        Dim lDLim As Long = typeA_dlim
        Dim lUp As Long = typeA_up
        Dim lULim As Long = typeA_ulim
        Dim lDFree As Long = lDLim - lDown
        Dim lUFree As Long = lULim - lUp
        If lDown > 0 Or lDFree <> 0 Or lDLim > 0 Or lUp > 0 Or lUFree <> 0 Or lULim > 0 Then
          DoChange(lblTypeADldUsedVal, lDown)
          DoChange(lblTypeADldFreeVal, lDFree)
          DoChange(lblTypeADldLimitVal, lDLim)
          DoChange(lblTypeAUldUsedVal, lUp)
          DoChange(lblTypeAUldFreeVal, lUFree)
          DoChange(lblTypeAUldLimitVal, lULim)
        End If
        ResizePanels()
        If lDown = 0 And lDFree = 0 And lDLim = 0 And lUp = 0 And lUFree = 0 And lULim = 0 Then
          AskForDonations()
          Return
        End If
      Case "TYPEB"
        Dim lUsed As Long = typeB_used
        Dim lLim As Long = typeB_lim
        Dim lFree As Long = lLim - lUsed
        If lUsed <> 0 Or typeB_lim > 0 Or lFree <> 0 Then
          DoChange(lblTypeBUsedVal, lUsed)
          DoChange(lblTypeBFreeVal, lFree)
          DoChange(lblTypeBLimitVal, lLim)
        End If
        ResizePanels()
        If lUsed = 0 And lLim = 0 And lFree = 0 Then
          AskForDonations()
          Return
        End If
    End Select
    If tmrChanges IsNot Nothing Then
      tmrChanges.Dispose()
      tmrChanges = Nothing
    End If
    tmrChanges = New Threading.Timer(New Threading.TimerCallback(AddressOf DisplayChangeInterval), state, 25, System.Threading.Timeout.Infinite)
  End Sub
  Private Sub DoChange(ByRef lblTemp As Label, ByRef toVal As Long)
    Dim tmpVal As Long = 0
    If lblTemp.Text.Length > 3 And lblTemp.Text.Contains(" ") Then
      Dim tmpStr As String = lblTemp.Text.Substring(0, lblTemp.Text.LastIndexOf(" "))
      If tmpStr.Contains(",") Then tmpStr = tmpStr.Replace(",", "")
      If IsNumeric(tmpStr) Then
        tmpVal = Long.Parse(tmpStr)
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
        lblTemp.Text = tmpVal.ToString("N0") & " MB"
      ElseIf tmpVal < toVal Then
        tmpVal += majorDif
        lblTemp.Text = tmpVal.ToString("N0") & " MB"
      Else
        lblTemp.Text = toVal.ToString("N0") & " MB"
        toVal = 0
      End If
    Else
      lblTemp.Text = toVal.ToString("N0") & " MB"
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
  Private Sub DisplayTypeAResults(lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long, sLastUpdate As String)
    Dim sTTT As String = Me.Text
    Dim overDown, overUp As Boolean
    overDown = (lDown >= lDownLim)
    overUp = (lUp >= lUpLim)
    If overDown Or overUp Then
      imSlowed = True
    ElseIf (lDown < (lDownLim * 0.7)) And (lUp < (lUpLim * 0.7)) Then
      imSlowed = False
    End If
    pnlTypeA.Visible = True
    pnlTypeB.Visible = False
    pnlNothing.Visible = False
    typeA_down = lDown
    typeA_dlim = lDownLim
    typeA_up = lUp
    typeA_ulim = lUpLim
    If tmrChanges IsNot Nothing Then
      tmrChanges.Dispose()
      tmrChanges = Nothing
    End If
    tmrChanges = New Threading.Timer(New Threading.TimerCallback(AddressOf DisplayChangeInterval), "TYPEA", 75, System.Threading.Timeout.Infinite)
    If overDown Then
      lblTypeADldFreeVal.ForeColor = Color.Red
      ttUI.SetToolTip(lblTypeADldFreeVal, "You are over your Download limit!")
    Else
      lblTypeADldFreeVal.ForeColor = SystemColors.ControlText
      ttUI.SetToolTip(lblTypeADldFreeVal, Nothing)
    End If
    If overUp Then
      lblTypeAUldFreeVal.ForeColor = Color.Red
      ttUI.SetToolTip(lblTypeAUldFreeVal, "You are over your Upload limit!")
    Else
      lblTypeAUldFreeVal.ForeColor = SystemColors.ControlText
      ttUI.SetToolTip(lblTypeAUldFreeVal, Nothing)
    End If
    If imSlowed Then
      lblTypeADldLimitVal.ForeColor = Color.Red
      lblTypeAUldLimitVal.ForeColor = Color.Red
      ttUI.SetToolTip(lblTypeADldLimitVal, "Your connection has been restricted!")
      ttUI.SetToolTip(lblTypeAUldLimitVal, "Your connection has been restricted!")
    Else
      lblTypeADldLimitVal.ForeColor = SystemColors.ControlText
      lblTypeAUldLimitVal.ForeColor = SystemColors.ControlText
      ttUI.SetToolTip(lblTypeADldLimitVal, Nothing)
      ttUI.SetToolTip(lblTypeAUldLimitVal, Nothing)
    End If
    gbTypeADld.Text = "Download (" & AccuratePercent(lDown / lDownLim) & ")"
    gbTypeAUld.Text = "Upload (" & AccuratePercent(lUp / lUpLim) & ")"
    ttUI.SetToolTip(pctTypeADld, "Graph representing your download usage.")
    ttUI.SetToolTip(pctTypeAUld, "Graph representing your upload usage.")
    pctTypeADld.Image = DisplayProgress(pctTypeADld.DisplayRectangle.Size, lDown, lDownLim, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
    pctTypeAUld.Image = DisplayProgress(pctTypeAUld.DisplayRectangle.Size, lUp, lUpLim, mySettings.Accuracy, mySettings.Colors.MainUpA, mySettings.Colors.MainUpB, mySettings.Colors.MainUpC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
    Dim dFree, uFree As String
    If lDownLim > lDown Then
      dFree = ", " & MBorGB(lDownLim - lDown) & " Free"
    ElseIf lDownLim < lDown Then
      dFree = ", " & MBorGB(lDown - lDownLim) & " Over"
    Else
      dFree = String.Empty
    End If
    If lUpLim > lUp Then
      uFree = ", " & MBorGB(lUpLim - lUp) & " Free"
    ElseIf lUpLim < lUp Then
      uFree = ", " & MBorGB(lUp - lDownLim) & " Over"
    Else
      uFree = String.Empty
    End If
    sTTT = "Satellite Usage" & IIf(imSlowed, " (Slowed)", "") & vbCr &
           "Updated " & sLastUpdate & vbCr &
           "Down: " & MBorGB(lDown) & " (" & AccuratePercent(lDown / lDownLim) & ")" & dFree & vbCr &
           "Up: " & MBorGB(lUp) & " (" & AccuratePercent(lUp / lUpLim) & ")" & uFree
    If sTTT.Length > ttLimit Then
      If lDownLim > lDown Then
        dFree = " (" & MBorGB(lDownLim - lDown) & " Free)"
      ElseIf lDownLim < lDown Then
        dFree = " (" & MBorGB(lDown - lDownLim) & " Over)"
      Else
        dFree = String.Empty
      End If
      If lUpLim > lUp Then
        uFree = " (" & MBorGB(lUpLim - lUp) & " Free)"
      ElseIf lUpLim < lUp Then
        uFree = " (" & MBorGB(lUp - lDownLim) & " Over)"
      Else
        uFree = String.Empty
      End If
      sTTT = "Usage" & IIf(imSlowed, " (Slow)", "") & " [" & sLastUpdate & "]" & vbCr &
             "D: " & AccuratePercent(lDown / lDownLim) & dFree & vbCr &
             "U: " & AccuratePercent(lUp / lUpLim) & uFree
    End If
    iconBefore = CreateTypeATrayIcon(lDown, lDownLim, lUp, lUpLim)
    iconStop = True
    SetNotifyIconText(trayIcon, sTTT)
    DisplayResultAlert(mySettings.AccountType, lDown, lUp)
  End Sub
  Private Sub DisplayTypeA2Results(lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long, sLastUpdate As String)
    Dim sTTT As String = Me.Text
    imSlowed = (lDown >= lDownLim)
    pnlTypeA.Visible = True
    pnlTypeB.Visible = False
    pnlNothing.Visible = False
    typeA_down = lDown
    typeA_dlim = lDownLim
    typeA_up = lUp
    typeA_ulim = lUpLim
    If tmrChanges IsNot Nothing Then
      tmrChanges.Dispose()
      tmrChanges = Nothing
    End If
    tmrChanges = New Threading.Timer(New Threading.TimerCallback(AddressOf DisplayChangeInterval), "TYPEA", 75, System.Threading.Timeout.Infinite)
    If imSlowed Then
      lblTypeADldFreeVal.ForeColor = Color.Red
      lblTypeADldLimitVal.ForeColor = Color.Red
      ttUI.SetToolTip(lblTypeADldFreeVal, "You are over your Anytime limit!")
      ttUI.SetToolTip(lblTypeADldLimitVal, "Your Anytime connection has been restricted!")
    Else
      lblTypeADldFreeVal.ForeColor = SystemColors.ControlText
      lblTypeADldLimitVal.ForeColor = SystemColors.ControlText
      ttUI.SetToolTip(lblTypeADldFreeVal, Nothing)
      ttUI.SetToolTip(lblTypeADldLimitVal, Nothing)
    End If
    If lUp >= lUpLim Then
      lblTypeAUldFreeVal.ForeColor = Color.Red
      lblTypeAUldLimitVal.ForeColor = Color.Red
      ttUI.SetToolTip(lblTypeAUldFreeVal, "You are over your Off-Peak limit!")
      ttUI.SetToolTip(lblTypeAUldLimitVal, "Your Off-Peak connection has been restricted!")
    Else
      lblTypeAUldFreeVal.ForeColor = SystemColors.ControlText
      lblTypeAUldLimitVal.ForeColor = SystemColors.ControlText
      ttUI.SetToolTip(lblTypeAUldFreeVal, Nothing)
      ttUI.SetToolTip(lblTypeAUldLimitVal, Nothing)
    End If
    gbTypeADld.Text = "Anytime (" & AccuratePercent(lDown / lDownLim) & ")"
    gbTypeAUld.Text = "Off-Peak (" & AccuratePercent(lUp / lUpLim) & ")"
    ttUI.SetToolTip(pctTypeADld, "Graph representing your Anytime usage.")
    ttUI.SetToolTip(pctTypeAUld, "Graph representing your Off-Peak usage (used between 2am and 8am).")
    pctTypeADld.Image = DisplayProgress(pctTypeADld.DisplayRectangle.Size, lDown, lDownLim, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
    pctTypeAUld.Image = DisplayProgress(pctTypeAUld.DisplayRectangle.Size, lUp, lUpLim, mySettings.Accuracy, mySettings.Colors.MainUpA, mySettings.Colors.MainUpB, mySettings.Colors.MainUpC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
    Dim atFree, opFree As String
    If lDownLim > lDown Then
      atFree = ", " & MBorGB(lDownLim - lDown) & " Free"
    ElseIf lDownLim < lDown Then
      atFree = ", " & MBorGB(lDown - lDownLim) & " Over"
    Else
      atFree = String.Empty
    End If
    If lUpLim > lUp Then
      opFree = ", " & MBorGB(lUpLim - lUp) & " Free"
    ElseIf lUpLim < lUp Then
      opFree = ", " & MBorGB(lUp - lUpLim) & " Over"
    Else
      opFree = String.Empty
    End If
    sTTT = "Satellite Usage" & IIf(imSlowed, " (Slowed)", "") & vbCr &
           "Updated " & sLastUpdate & vbCr &
           "Anytime: " & MBorGB(lDown) & " (" & AccuratePercent(lDown / lDownLim) & ")" & atFree & vbCr &
           "Off-Peak: " & MBorGB(lUp) & " (" & AccuratePercent(lUp / lUpLim) & ")" & opFree
    If sTTT.Length > ttLimit Then
      If lDownLim > lDown Then
        atFree = " (" & MBorGB(lDownLim - lDown) & " Free)"
      ElseIf lDownLim < lDown Then
        atFree = " (" & MBorGB(lDown - lDownLim) & " Free)"
      Else
        atFree = String.Empty
      End If
      If lUpLim > lUp Then
        opFree = " (" & MBorGB(lUpLim - lUp) & " Free)"
      ElseIf lUpLim < lUp Then
        opFree = " (" & MBorGB(lUp - lUpLim) & " Free)"
      Else
        opFree = String.Empty
      End If
      sTTT = "Usage" & IIf(imSlowed, " (Slow)", "") & " [" & sLastUpdate & "]" & vbCr &
             "A-T: " & AccuratePercent(lDown / lDownLim) & atFree & vbCr &
             "O-P: " & AccuratePercent(lUp / lUpLim) & opFree
    End If
    iconBefore = CreateTypeATrayIcon(lDown, lDownLim, lUp, lUpLim)
    iconStop = True
    SetNotifyIconText(trayIcon, sTTT)
    DisplayResultAlert(mySettings.AccountType, lDown, lUp)
  End Sub
  Private Sub DisplayTypeBResults(lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long, sLastUpdate As String)
    If Not (lDown = lUp And lDownLim = lUpLim) Then
      DisplayTypeAResults(lDown, lDownLim, lUp, lUpLim, sLastUpdate)
      Return
    End If
    Dim sTTT As String = Me.Text
    imSlowed = (lDown >= lDownLim)
    pnlTypeA.Visible = False
    pnlTypeB.Visible = True
    pnlNothing.Visible = False
    typeB_used = lDown
    typeB_lim = lDownLim
    If tmrChanges IsNot Nothing Then
      tmrChanges.Dispose()
      tmrChanges = Nothing
    End If
    tmrChanges = New Threading.Timer(New Threading.TimerCallback(AddressOf DisplayChangeInterval), "TYPEB", 75, System.Threading.Timeout.Infinite)
    If imSlowed Then
      lblTypeBFreeVal.ForeColor = Color.Red
      lblTypeBLimitVal.ForeColor = Color.Red
      ttUI.SetToolTip(lblTypeBFreeVal, "You are over your usage limit!")
      ttUI.SetToolTip(lblTypeBLimitVal, "Your connection has been restricted!")
    Else
      lblTypeBFreeVal.ForeColor = SystemColors.ControlText
      lblTypeBLimitVal.ForeColor = SystemColors.ControlText
      ttUI.SetToolTip(lblTypeBFreeVal, Nothing)
      ttUI.SetToolTip(lblTypeBLimitVal, Nothing)
    End If
    pctTypeB.Image = DisplayRProgress(pctTypeB.DisplayRectangle.Size, lDown, lDownLim, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
    sTTT = "Satellite Usage" & IIf(imSlowed, " (Slowed)", "") & vbCr &
           "Updated " & sLastUpdate & vbCr &
            MBorGB(lDown) & " of " & MBorGB(lDownLim) & " (" & AccuratePercent(lDown / lDownLim) & ")"
    If sTTT.Length > ttLimit Then
      sTTT = "Usage" & IIf(imSlowed, " (Slow)", "") & " [" & sLastUpdate & "]" & vbCr &
            AccuratePercent(lDown / lDownLim)
    End If
    If lDownLim > lDown Then
      sTTT &= vbCr & MBorGB(lDownLim - lDown) & " Free"
    ElseIf lDownLim < lDown Then
      sTTT &= vbCr & MBorGB(lDown - lDownLim) & " Over"
    End If
    iconBefore = CreateTypeBTrayIcon(lDown, lDownLim)
    iconStop = True
    SetNotifyIconText(trayIcon, sTTT)
    DisplayResultAlert(mySettings.AccountType, lDown, lUp)
  End Sub
  Private Sub DisplayResults(lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long)
    If lDownLim > 0 Or lUpLim > 0 Then
      Dim lastUpdate As Date = LOG_GetLast()
      Dim sLastUpdate As String = lastUpdate.ToString("M/d h:mm tt")
      myPanel = mySettings.AccountType
      Select Case mySettings.AccountType
        Case SatHostTypes.RuralPortal_EXEDE, SatHostTypes.WildBlue_EXEDE : DisplayTypeBResults(lDown, lDownLim, lUp, lUpLim, sLastUpdate)
        Case SatHostTypes.DishNet_EXEDE : DisplayTypeA2Results(lDown, lDownLim, lUp, lUpLim, sLastUpdate)
        Case Else : DisplayTypeAResults(lDown, lDownLim, lUp, lUpLim, sLastUpdate)
      End Select
    Else
      pnlTypeA.Visible = False
      pnlTypeB.Visible = False
      pnlNothing.Visible = True
      myPanel = SatHostTypes.Other
      trayIcon.Text = Me.Text
      iconBefore = MakeIcon(IconName.norm)
      iconStop = True
    End If
  End Sub
  Private Sub DisplayResultAlert(Type As localRestrictionTracker.SatHostTypes, lDown As Long, lUp As Long)
    If mySettings.Overuse > 0 Then
      If lastBalloon > 0 AndAlso srlFunctions.TickCount() - lastBalloon < mySettings.Overtime * 60 * 1000 Then Return
      Dim TimeCheck As Integer = -mySettings.Overtime
      If TimeCheck <= -15 Then
        Dim lItems() As DataBase.DataRow = Array.FindAll(usageDB.ToArray, Function(satRow As DataBase.DataRow) satRow.DATETIME.CompareTo(Now.AddMinutes(TimeCheck)) >= 0 And satRow.DATETIME.CompareTo(Now) <= 0)
        For I As Integer = lItems.Count - 2 To 0 Step -1
          Select Case Type
            Case SatHostTypes.WildBlue_LEGACY, SatHostTypes.RuralPortal_LEGACY
              If lDown - lItems(I).DOWNLOAD >= mySettings.Overuse Then
                Dim ChangeSize As Long = Math.Abs(lDown - lItems(I).DOWNLOAD)
                Dim ChangeTime As Long = Math.Abs(DateDiff(DateInterval.Minute, lItems(I).DATETIME, Now) * 60 * 1000)
                MakeNotifier(taskNotifier, False)
                If taskNotifier IsNot Nothing Then taskNotifier.Show("Excessive Download Detected", My.Application.Info.ProductName & " has logged a download of " & MBorGB(ChangeSize) & " in " & ConvertTime(ChangeTime) & "!", 200, 0, 100)
                lastBalloon = srlFunctions.TickCount()
                Exit For
              ElseIf lUp - lItems(I).UPLOAD >= mySettings.Overuse Then
                Dim ChangeSize As Long = Math.Abs(lUp - lItems(I).UPLOAD)
                Dim ChangeTime As Long = Math.Abs(DateDiff(DateInterval.Minute, lItems(I).DATETIME, Now) * 60 * 1000)
                MakeNotifier(taskNotifier, False)
                If taskNotifier IsNot Nothing Then taskNotifier.Show("Excessive Upload Detected", My.Application.Info.ProductName & " has logged an upload of " & MBorGB(ChangeSize) & " in " & ConvertTime(ChangeTime) & "!", 200, 0, 100)
                lastBalloon = srlFunctions.TickCount()
                Exit For
              End If
            Case SatHostTypes.WildBlue_EXEDE, SatHostTypes.RuralPortal_EXEDE
              If lDown - lItems(I).DOWNLOAD >= mySettings.Overuse Then
                Dim ChangeSize As Long = Math.Abs(lDown - lItems(I).DOWNLOAD)
                Dim ChangeTime As Long = Math.Abs(DateDiff(DateInterval.Minute, lItems(I).DATETIME, Now) * 60 * 1000)
                MakeNotifier(taskNotifier, False)
                If taskNotifier IsNot Nothing Then taskNotifier.Show("Excessive Usage Detected", My.Application.Info.ProductName & " has logged a usage change of " & MBorGB(ChangeSize) & " in " & ConvertTime(ChangeTime) & "!", 200, 0, 100)
                lastBalloon = srlFunctions.TickCount()
                Exit For
              End If
            Case SatHostTypes.DishNet_EXEDE
              If lDown - lItems(I).DOWNLOAD >= mySettings.Overuse Then
                Dim ChangeSize As Long = Math.Abs(lDown - lItems(I).DOWNLOAD)
                Dim ChangeTime As Long = Math.Abs(DateDiff(DateInterval.Minute, lItems(I).DATETIME, Now) * 60 * 1000)
                MakeNotifier(taskNotifier, False)
                If taskNotifier IsNot Nothing Then taskNotifier.Show("Excessive Usage Detected", My.Application.Info.ProductName & " has logged a usage change of " & MBorGB(ChangeSize) & " in " & ConvertTime(ChangeTime) & "!", 200, 0, 100)
                lastBalloon = srlFunctions.TickCount()
                Exit For
              ElseIf lUp - lItems(I).UPLOAD >= mySettings.Overuse Then
                Dim ChangeSize As Long = Math.Abs(lUp - lItems(I).UPLOAD)
                Dim ChangeTime As Long = Math.Abs(DateDiff(DateInterval.Minute, lItems(I).DATETIME, Now) * 60 * 1000)
                MakeNotifier(taskNotifier, False)
                If taskNotifier IsNot Nothing Then taskNotifier.Show("Excessive Off-Peak Usage Detected", My.Application.Info.ProductName & " has logged an Off-Peak usage change of " & MBorGB(ChangeSize) & " in " & ConvertTime(ChangeTime) & "!", 200, 0, 100)
                lastBalloon = srlFunctions.TickCount()
                Exit For
              End If
          End Select
        Next
      End If
    End If
  End Sub
#End Region
#End Region
#Region "Buttons"
  Private Sub cmdRefresh_Click(sender As System.Object, e As System.EventArgs) Handles cmdRefresh.Click
    InitAccount()
    If Not String.IsNullOrEmpty(sAccount) And Not String.IsNullOrEmpty(sProvider) And Not String.IsNullOrEmpty(sPassword) Then
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
      SetStatusText(LOG_GetLast.ToString("g"), "Beginning Usage Request...", False)
      Dim UsageInvoker As New MethodInvoker(AddressOf GetUsage)
      UsageInvoker.BeginInvoke(Nothing, Nothing)
    Else
      If mySettings.TrayIconStyle = AppSettings.TrayStyles.Never Then
        If Me.WindowState = FormWindowState.Minimized Then
          Me.WindowState = FormWindowState.Normal
          mnuRestore.Text = "&Focus"
        End If
      Else
        If Not Me.Visible Then
          Me.Location = New Point((Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2)
          If mySettings.TrayIconAnimation Then AnimateWindow(Me, False)
          Me.Show()
          mnuRestore.Text = "&Focus"
        End If
      End If
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
    If remoteData IsNot Nothing Then
      bReRun = True
      remoteData.Dispose()
      remoteData = Nothing
      DisplayUsage(True, False)
    ElseIf localData IsNot Nothing Then
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
      If Not myState = LoadStates.Lookup Then
        Dim lookupInvoker As New MethodInvoker(AddressOf LookupProvider)
        lookupInvoker.BeginInvoke(Nothing, Nothing)
      End If
      Do Until myState = LoadStates.Loaded
        Application.DoEvents()
        Threading.Thread.Sleep(1)
        If srlFunctions.TickCount() > WaitTime Then Exit Do
      Loop
    End If
    Select Case dRet
      Case Windows.Forms.DialogResult.Yes
        didHostListSave = False
        ReLoadSettings()
        SetNextLoginTime()
        Dim ReInitInvoker As New MethodInvoker(AddressOf ReInit)
        ReInitInvoker.BeginInvoke(Nothing, Nothing)
      Case Windows.Forms.DialogResult.OK
        didHostListSave = False
        ReLoadSettings()
        If bReRun Then
          SetNextLoginTime()
          EnableProgressIcon()
          SetStatusText(LOG_GetLast.ToString("g"), "Preparing Connection...", False)
          Dim UsageInvoker As New MethodInvoker(AddressOf GetUsage)
          UsageInvoker.BeginInvoke(Nothing, Nothing)
        Else
          DisplayUsage(True, False)
        End If
        Dim sizeChangeInvoker As New EventHandler(AddressOf frmMain_SizeChanged)
        sizeChangeInvoker.BeginInvoke(Me, New EventArgs, Nothing, Nothing)
        If frmHistory.Visible Then
          frmHistory.mySettings = New AppSettings
          ScreenDefaultColors(frmHistory.mySettings.Colors, frmHistory.mySettings.AccountType)
          If LocalAppDataDirectory = Application.StartupPath & "\Config\" Then frmHistory.mySettings.HistoryDir = Application.StartupPath & "\Config\"
          frmHistory.DoResize(True)
        End If
      Case Windows.Forms.DialogResult.Abort

      Case Else
        If bReRun Then
          SetNextLoginTime()
          EnableProgressIcon()
          SetStatusText(LOG_GetLast.ToString("g"), "Preparing Connection...", False)
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
    If Not Me.Visible Then
      If mySettings.TrayIconAnimation Then
        Dim bMax As Boolean = False
        If Me.WindowState = FormWindowState.Maximized Then
          bMax = True
        Else
          Me.Location = New Point((Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2)
        End If
        If mySettings.TrayIconAnimation Then
          If Not ClosingTime Then AnimateWindow(Me, False)
          mnuRestore.Text = "&Focus"
          If Not Me.WindowState = FormWindowState.Maximized Then Me.WindowState = FormWindowState.Normal
          Me.Show()
        Else
          Me.Show()
          mnuRestore.Text = "&Focus"
          If Not Me.WindowState = FormWindowState.Maximized Then Me.WindowState = FormWindowState.Normal
        End If
        If bMax Then
          Me.WindowState = FormWindowState.Normal
          Me.Location = New Point((Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2)
          Me.WindowState = FormWindowState.Maximized
        End If
        If mySettings.TrayIconStyle = AppSettings.TrayStyles.Minimized Then trayIcon.Visible = False
      Else
        Me.Show()
        mnuRestore.Text = "&Focus"
        Me.WindowState = FormWindowState.Normal
        If mySettings.TrayIconStyle = AppSettings.TrayStyles.Minimized Then trayIcon.Visible = False
        Me.Location = New Point((Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2)
      End If
    End If
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
        ScreenDefaultColors(frmHistory.mySettings.Colors, frmHistory.mySettings.AccountType)
        If LocalAppDataDirectory = Application.StartupPath & "\Config\" Then frmHistory.mySettings.HistoryDir = Application.StartupPath & "\Config\"
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
  Private Function MakeIcon(name As IconName, Optional icoX As Integer = -1, Optional icoY As Integer = -1) As Icon
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
  Private Sub SetNotifyIconText(ni As NotifyIcon, text As String)
    If text.Length >= 128 Then Throw New ArgumentOutOfRangeException("Text limited to 127 characters")
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
  Private Sub taskNotifier_ContentClick(sender As Object, e As System.EventArgs) Handles taskNotifier.ContentClick
    taskNotifier.Hide()
    If Not String.IsNullOrEmpty(sFailTray) Then
      Dim tFTP As New ParamaterizedInvoker(AddressOf SaveToFTP)
      tFTP.BeginInvoke({sFailTray, New FailResponseInvoker(AddressOf FailResponse)}, Nothing, Nothing)
    End If
    sFailTray = Nothing
  End Sub
#Region "Graphs"
  Private Function CreateTypeATrayIcon(lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long) As Icon
    Dim icoX As Integer = NativeMethods.GetSystemMetrics(NativeMethods.MetricsList.SM_CXSMICON)
    Dim icoY As Integer = NativeMethods.GetSystemMetrics(NativeMethods.MetricsList.SM_CYSMICON)
    Dim imgTray As New Bitmap(icoX, icoY)
    Using g As Graphics = Graphics.FromImage(imgTray)
      g.Clear(Color.Transparent)
      If imSlowed Then
        g.DrawIconUnstretched(MakeIcon(IconName.restricted, icoX, icoY), New Rectangle(0, 0, icoX, icoY))
        CreateTrayIcon_Left(g, lDown, lDownLim, mySettings.Colors.TrayDownA, mySettings.Colors.TrayDownB, mySettings.Colors.TrayDownC, icoX, icoY)
        CreateTrayIcon_Right(g, lUp, lUpLim, mySettings.Colors.TrayUpA, mySettings.Colors.TrayUpB, mySettings.Colors.TrayUpC, icoX, icoY)
      ElseIf imFree Then
        g.DrawIconUnstretched(MakeIcon(IconName.free, icoX, icoY), New Rectangle(0, 0, icoX, icoY))
      Else
        g.DrawIconUnstretched(MakeIcon(IconName.norm, icoX, icoY), New Rectangle(0, 0, icoX, icoY))
        CreateTrayIcon_Left(g, lDown, lDownLim, mySettings.Colors.TrayDownA, mySettings.Colors.TrayDownB, mySettings.Colors.TrayDownC, icoX, icoY)
        CreateTrayIcon_Right(g, lUp, lUpLim, mySettings.Colors.TrayUpA, mySettings.Colors.TrayUpB, mySettings.Colors.TrayUpC, icoX, icoY)
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
  Private Function CreateTypeBTrayIcon(lUsed As Long, lLim As Long) As Icon
    Dim icoX As Integer = NativeMethods.GetSystemMetrics(NativeMethods.MetricsList.SM_CXSMICON)
    Dim icoY As Integer = NativeMethods.GetSystemMetrics(NativeMethods.MetricsList.SM_CYSMICON)
    Dim imgTray As New Bitmap(icoX, icoY)
    Using g As Graphics = Graphics.FromImage(imgTray)
      g.Clear(Color.Transparent)
      If imSlowed Then
        g.DrawIconUnstretched(MakeIcon(IconName.restricted, icoX, icoY), New Rectangle(0, 0, icoX, icoY))
        CreateTrayIcon_Left(g, lUsed, lLim, mySettings.Colors.TrayDownA, mySettings.Colors.TrayDownB, mySettings.Colors.TrayDownC, icoX, icoY)
        CreateTrayIcon_Right(g, lUsed, lLim, mySettings.Colors.TrayDownA, mySettings.Colors.TrayDownB, mySettings.Colors.TrayDownC, icoX, icoY)
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
      SetStatusText(LOG_GetLast.ToString("g"), "Checking for Software Update...", False)
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
        Me.Invoke(New EventHandler(AddressOf updateChecker_CheckResult), sender, e)
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
                If remoteData IsNot Nothing Then
                  remoteData.Dispose()
                  remoteData = Nothing
                ElseIf localData IsNot Nothing Then
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
                If remoteData IsNot Nothing Then
                  remoteData.Dispose()
                  remoteData = Nothing
                ElseIf localData IsNot Nothing Then
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
                  If remoteData IsNot Nothing Then
                    remoteData.Dispose()
                    remoteData = Nothing
                  ElseIf localData IsNot Nothing Then
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
                  If remoteData IsNot Nothing Then
                    remoteData.Dispose()
                    remoteData = Nothing
                  ElseIf localData IsNot Nothing Then
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
            SetStatusText(LOG_GetLast.ToString("g"), String.Empty, False)
            If updateChecker IsNot Nothing Then
              updateChecker.Dispose()
              updateChecker = Nothing
            End If
            NextGrabTick = Long.MinValue
        End Select
      Else
        Select Case e.Result
          Case clsUpdate.CheckEventArgs.ResultType.NewUpdate
            If remoteData IsNot Nothing Then
              remoteData.Dispose()
              remoteData = Nothing
            ElseIf localData IsNot Nothing Then
              localData.Dispose()
              localData = Nothing
            End If
            updateChecker.DownloadUpdate(sEXEPath)
          Case clsUpdate.CheckEventArgs.ResultType.NewBeta
            If mySettings.UpdateBETA Then
              If remoteData IsNot Nothing Then
                remoteData.Dispose()
                remoteData = Nothing
              ElseIf localData IsNot Nothing Then
                localData.Dispose()
                localData = Nothing
              End If
              updateChecker.DownloadUpdate(sEXEPath)
            End If
          Case Else
            SetStatusText(LOG_GetLast.ToString("g"), String.Empty, False)
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
    SetStatusText(LOG_GetLast.ToString("g"), "Downloading Software Update...", False)
  End Sub
  Private Sub updateChecker_DownloadResult(sender As Object, e As clsUpdate.DownloadEventArgs) Handles updateChecker.DownloadResult
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New EventHandler(AddressOf updateChecker_DownloadResult), sender, e)
      Catch ex As Exception
      End Try
      Return
    End If
    tmrSpeed.Enabled = False
    If e.Error IsNot Nothing Then
      SetStatusText(LOG_GetLast.ToString("g"), "Software Update Error: " & e.Error.Message, True)
      NextGrabTick = Long.MinValue
    ElseIf e.Cancelled Then
      If updateChecker IsNot Nothing Then
        updateChecker.Dispose()
        updateChecker = Nothing
      End If
      SetStatusText(LOG_GetLast.ToString("g"), "Software Update Cancelled!", True)
      NextGrabTick = Long.MinValue
    Else
      If updateChecker IsNot Nothing Then
        updateChecker.Dispose()
        updateChecker = Nothing
      End If
      SetStatusText(LOG_GetLast.ToString("g"), "Software Update Download Complete", False)
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
      SetStatusText(LOG_GetLast.ToString("g"), "Software Update Failure!", True)
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
        Me.Invoke(New EventHandler(AddressOf updateChecker_UpdateProgressChanged), sender, e)
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
  Private Sub AskForDonations()
    Try
      If String.IsNullOrEmpty(mySettings.RemoteKey) Then
        If Math.Abs(DateDiff(DateInterval.Minute, Process.GetCurrentProcess.StartTime, Now)) > 30 Then
          If Now.Month = 5 Or Now.Month = 9 Or Now.Month = 12 Then
            If Now.DayOfWeek = DayOfWeek.Saturday Or Now.DayOfWeek = DayOfWeek.Sunday Then
              Dim lastAsk As Long = DateDiff(DateInterval.Month, mySettings.LastNag, Now)
              If lastAsk > 6 Or lastAsk < -24 Then
                mySettings.LastNag = Today
                mySettings.Save()
                frmDonate.Show()
              End If
            End If
          End If
        End If
      End If
    Catch
    End Try
  End Sub
  Public Sub ClickedDonate()
    Try
      mySettings.LastNag = DateAdd(DateInterval.Month, 24, Today)
      mySettings.Save()
    Catch ex As Exception
    End Try
  End Sub
  Private Sub PowerModeChanged(sender As Object, e As Microsoft.Win32.PowerModeChangedEventArgs)
    If e.Mode = Microsoft.Win32.PowerModes.Suspend Then
      If localData IsNot Nothing Then
        localData.Dispose()
        localData = Nothing
      End If
      If remoteData IsNot Nothing Then
        remoteData.Dispose()
        remoteData = Nothing
      End If
      cmdRefresh.Enabled = True
    ElseIf e.Mode = Microsoft.Win32.PowerModes.Resume Then
      ReLoadSettings()
      cmdRefresh.Enabled = True
      DisplayUsage(False, False)
      SetNextLoginTime(mySettings.StartWait)
    End If
  End Sub
#Region "Failure Reports"
  Public Sub FailResponse(sRet As String)
    If Me.InvokeRequired Then
      Try
        Me.Invoke(New ParamaterizedInvoker(AddressOf FailResponse), sRet)
      Catch ex As Exception
      End Try
      Return
    End If
    MakeNotifier(taskNotifier, False)
    If taskNotifier IsNot Nothing Then
      If sRet = "added" Then
        taskNotifier.Show("Error Report Sent", "Your report has been received by " & Application.CompanyName & "." & vbNewLine & "Thank you for helping to improve " & My.Application.Info.ProductName & "!", 200, 15 * 1000, 100)
      ElseIf sRet = "exists" Then
        taskNotifier.Show("Error Already Reported", "This error has already been reported. It should be fixed in the next release." & vbNewLine & "Thank you anyway!", 200, 15 * 1000, 100)
      Else
        taskNotifier.Show("Error Reporting Error", My.Application.Info.ProductName & " was unable to contact the " & Application.CompanyName & " servers. Please check your internet connection.", 200, 30 * 1000, 100)
      End If
    End If
  End Sub
  Private Sub FailFile(sFail As String)
    If clsUpdate.QuickCheckVersion = clsUpdate.CheckEventArgs.ResultType.NoUpdate Then
      sFailTray = sFail
      MakeNotifier(taskNotifier, True)
      If taskNotifier IsNot Nothing Then taskNotifier.Show("Error Reading Page Data", My.Application.Info.ProductName & " encountered data it does not understand." & vbNewLine & "Click this alert to report the problem to " & Application.CompanyName & ".", 200, 3 * 60 * 1000, 100)
    End If
  End Sub
#End Region
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
        If My.Computer.FileSystem.FileExists(Application.StartupPath & "\RestrictionController.exe") Then
          Dim ControllerProps As New ProcessStartInfo(Application.StartupPath & "\RestrictionController.exe")
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
  Public Function CompareImages(Image1 As Bitmap, Image2 As Bitmap) As Boolean
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
#End Region
End Class
