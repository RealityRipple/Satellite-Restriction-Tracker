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
  Private WithEvents wsHostList As CookieAwareWebClient
  Private WithEvents wsFavicon As clsFavicon
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
  Private sEXEPath As String = AppDataPath & "Setup.exe"
  Private mySettings As AppSettings
  Private sAccount, sPassword, sProvider As String
  Private imSlowed As Boolean
  Private imFree As Boolean
  Private FullCheck As Boolean = True
  Private NextGrabTick As Long
  Private ClosingTime As Boolean
  Private sFailTray As String
  Private bAlert As TriState
  Private wb_down, wb_up, wb_dlim, wb_ulim As Long
  Private r_used, r_lim As Long
  Private lastBalloon As Long
#Region "Server Type Determination"
  Private Class DetermineTypeOffline
    Public Event TypeDetermined(Sender As Object, e As TypeDeterminedEventArgs)
    Public Sub New(Provider As String, Sender As Object)
      Application.DoEvents()
      Dim testCallback As New ParamaterizedInvoker(AddressOf BeginTest)
      testCallback.BeginInvoke({Provider, Sender}, Nothing, Nothing)
    End Sub
    Private Sub BeginTest(state As Object)
      Dim Provider As String = state(0)
      Dim Sender As Object = state(1)
      If Provider.ToLower = "dish.com" Or Provider.ToLower = "dish.net" Then
        RaiseEvent TypeDetermined(Sender, New TypeDeterminedEventArgs(SatHostTypes.DishNet_EXEDE))
      ElseIf Provider.ToLower = "exede.com" Or Provider.ToLower = "exede.net" Then
        RaiseEvent TypeDetermined(Sender, New TypeDeterminedEventArgs(SatHostTypes.WildBlue_EXEDE))
      Else
        OfflineCheck(state)
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
    Private Sub OfflineCheck(Sender As Object)
      Dim rpP, exP, wbP As Single
      OfflineStats(rpP, exP, wbP)
      If rpP = 0 And exP = 0 And wbP = 0 Then
        RaiseEvent TypeDetermined(Sender, New TypeDeterminedEventArgs(SatHostTypes.Other))
      Else
        If rpP > exP And rpP > wbP Then
          RaiseEvent TypeDetermined(Sender, New TypeDeterminedEventArgs(SatHostTypes.RuralPortal_EXEDE))
        ElseIf exP > rpP And exP > wbP Then
          RaiseEvent TypeDetermined(Sender, New TypeDeterminedEventArgs(SatHostTypes.WildBlue_EXEDE))
        ElseIf wbP > rpP And wbP > exP Then
          RaiseEvent TypeDetermined(Sender, New TypeDeterminedEventArgs(SatHostTypes.WildBlue_LEGACY))
        Else
          If rpP > wbP And exP > wbP And rpP = exP Then
            RaiseEvent TypeDetermined(Sender, New TypeDeterminedEventArgs(SatHostTypes.WildBlue_EXEDE))
          Else
            RaiseEvent TypeDetermined(Sender, New TypeDeterminedEventArgs(SatHostTypes.Other))
            Stop
          End If
        End If
      End If
    End Sub
  End Class
  Private Class TypeDeterminedEventArgs
    Inherits EventArgs
    Public HostType As SatHostTypes
    Public Sub New(Type As SatHostTypes)
      HostType = Type
    End Sub
  End Class
  Private WithEvents TypeDetermination As DetermineType
  Private WithEvents TypeDeterminationOffline As DetermineTypeOffline
  Private Sub TypeDetermination_TypeDetermined(Sender As Object, e As DetermineType.TypeDeterminedEventArgs) Handles TypeDetermination.TypeDetermined
    If Me.InvokeRequired Then
      Me.Invoke(New EventHandler(AddressOf TypeDetermination_TypeDetermined), Sender, e)
    Else
      NextGrabTick = TickCount() + (mySettings.Timeout * 1000)
      If e.HostGroup = DetermineType.TypeDeterminedEventArgs.SatHostGroup.Other Then
        tmrIcon.Enabled = False
        TypeDeterminationOffline = New DetermineTypeOffline(sProvider, Sender)
      Else
        If e.HostGroup = DetermineType.TypeDeterminedEventArgs.SatHostGroup.DishNet Then
          mySettings.AccountType = SatHostTypes.DishNet_EXEDE
        ElseIf e.HostGroup = DetermineType.TypeDeterminedEventArgs.SatHostGroup.WildBlue Then
          mySettings.AccountType = SatHostTypes.WildBlue_LEGACY
        ElseIf e.HostGroup = DetermineType.TypeDeterminedEventArgs.SatHostGroup.RuralPortal Then
          mySettings.AccountType = SatHostTypes.RuralPortal_EXEDE
        ElseIf e.HostGroup = DetermineType.TypeDeterminedEventArgs.SatHostGroup.Exede Then
          mySettings.AccountType = SatHostTypes.WildBlue_EXEDE
        End If
        If mySettings.Colors.MainDownA = Color.Transparent Then SetDefaultColors()
        mySettings.Save()
        SetStatusText(LOG_GetLast.ToString("g"), "Preparing Connection...", False)
        If localData IsNot Nothing Then
          localData.Dispose()
          localData = Nothing
        End If
        localData = New localRestrictionTracker(AppData)
        Dim connectInvoker As New MethodInvoker(AddressOf localData.Connect)
        connectInvoker.BeginInvoke(Nothing, Nothing)
      End If
    End If
  End Sub
  Private Sub TypeDeterminationOffline_TypeDetermined(Sender As Object, e As TypeDeterminedEventArgs) Handles TypeDeterminationOffline.TypeDetermined
    If Me.InvokeRequired Then
      Me.Invoke(New EventHandler(AddressOf TypeDeterminationOffline_TypeDetermined), Sender, e)
    Else
      NextGrabTick = TickCount() + (mySettings.Timeout * 1000)
      If e.HostType = SatHostTypes.Other Then
        tmrIcon.Enabled = False
        DisplayUsage(False, True)
        SetStatusText(LOG_GetLast.ToString("g"), "Please connect to the Internet.", True)
      Else
        mySettings.AccountType = e.HostType
        If mySettings.Colors.MainDownA = Color.Transparent Then SetDefaultColors()
        mySettings.Save()
        SetStatusText(LOG_GetLast.ToString("g"), "Preparing Connection...", False)
        If localData IsNot Nothing Then
          localData.Dispose()
          localData = Nothing
        End If
        localData = New localRestrictionTracker(AppData)
        Dim connectInvoker As New MethodInvoker(AddressOf localData.Connect)
        connectInvoker.BeginInvoke(Nothing, Nothing)
      End If
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
              If Me.WindowState = FormWindowState.Minimized Then trayIcon.Visible = True
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
          If Not mySettings.TrayIconStyle = AppSettings.TrayStyles.Never Then Me.Hide()
          mnuRestore.Text = "&Restore"
          Me.WindowState = FormWindowState.Minimized
        End If
        If Me.Opacity = 0 Then Me.Opacity = 1
        SetStatusText("Initializing", "Beginning application initialization process...", False)
        pctDld.Image = DisplayProgress(pctDld.DisplayRectangle.Size, 0, 0, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
        pctUld.Image = DisplayProgress(pctUld.DisplayRectangle.Size, 0, 0, mySettings.Accuracy, mySettings.Colors.MainUpA, mySettings.Colors.MainUpB, mySettings.Colors.MainUpC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
        Dim lookupInvoker As New MethodInvoker(AddressOf LookupProvider)
        lookupInvoker.BeginInvoke(Nothing, Nothing)
      Else
        mnuRestore.Text = "&Focus"
        tmrIcon.Enabled = False
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
    MyBase.WndProc(m)
    If m.Msg = NativeMethods.WM_SYSCOMMAND Then
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
    End If
  End Sub
  Private Sub frmMain_SizeChanged(sender As Object, e As System.EventArgs) Handles Me.SizeChanged
    If Me.InvokeRequired Then
      Me.BeginInvoke(New EventHandler(AddressOf frmMain_SizeChanged), sender, e)
    Else
      If Me.WindowState = FormWindowState.Minimized Then
        If mySettings.TrayIconStyle = AppSettings.TrayStyles.Always Then
          Me.Hide()
        ElseIf mySettings.TrayIconStyle = AppSettings.TrayStyles.Minimized Then
          trayIcon.Visible = True
          Me.Hide()
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
          If pnlWildBlue.Visible Then
            If pctNetTest.Bottom > pnlWildBlue.Top - 1 Then
              pctNetTest.Height = pnlWildBlue.Top - 1 - pctNetTest.Top
              pctNetTest.Width = pctNetTest.Height
            Else
              pctNetTest.Height = 16
              pctNetTest.Width = pctNetTest.Height
            End If
          ElseIf pnlExede.Visible Then
            If pctNetTest.Bottom > pnlExede.Top - 1 Then
              pctNetTest.Height = pnlExede.Top - 1 - pctNetTest.Top
              pctNetTest.Width = pctNetTest.Height
            Else
              pctNetTest.Height = 16
              pctNetTest.Width = pctNetTest.Height
            End If
          ElseIf pnlRural.Visible Then
            If pctNetTest.Bottom > pnlRural.Top - 1 Then
              pctNetTest.Height = pnlRural.Top - 1 - pctNetTest.Top
              pctNetTest.Width = pctNetTest.Height
            Else
              pctNetTest.Height = 16
              pctNetTest.Width = pctNetTest.Height
            End If
          End If
          pctNetTest.Left = gbUsage.Right - 16 - pctNetTest.Width
        Next
      End If
    End If
  End Sub
  Private Sub ResizePanels()
    If myPanel = SatHostTypes.WildBlue_LEGACY Or myPanel = SatHostTypes.RuralPortal_LEGACY Or myPanel = SatHostTypes.DishNet_EXEDE Then
      If wb_dlim = 0 And wb_ulim = 0 Then
        pctDld.Image = DisplayProgress(pctDld.DisplayRectangle.Size, 0, 0, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
        pctUld.Image = DisplayProgress(pctUld.DisplayRectangle.Size, 0, 0, mySettings.Accuracy, mySettings.Colors.MainUpA, mySettings.Colors.MainUpB, mySettings.Colors.MainUpC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
        trayIcon.Icon = MakeIcon(IconName.norm)
      Else
        pctDld.Image = DisplayProgress(pctDld.DisplayRectangle.Size, wb_down, wb_dlim, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
        pctUld.Image = DisplayProgress(pctUld.DisplayRectangle.Size, wb_up, wb_ulim, mySettings.Accuracy, mySettings.Colors.MainUpA, mySettings.Colors.MainUpB, mySettings.Colors.MainUpC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
        trayIcon.Icon = CreateTrayIcon(wb_down, wb_dlim, wb_up, wb_ulim)
      End If
    ElseIf myPanel = SatHostTypes.RuralPortal_EXEDE Or myPanel = SatHostTypes.WildBlue_EXEDE Then
      If r_lim = 0 Then
        pctRural.Image = DisplayRProgress(pctRural.DisplayRectangle.Size, 0, 1, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
        trayIcon.Icon = MakeIcon(IconName.norm)
      Else
        pctRural.Image = DisplayRProgress(pctRural.DisplayRectangle.Size, r_used, r_lim, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
        trayIcon.Icon = CreateRTrayIcon(r_used, r_lim)
      End If
    ElseIf myPanel = SatHostTypes.Other Then
      lblNothing.Text = Application.ProductName
      lblRRS.Text = "by " & Application.CompanyName
      ttUI.SetTooltip(lblRRS, "Visit realityripple.com.")
    End If
  End Sub
  Private Sub frmMain_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    ClosingTime = True
    tmrUpdate.Stop()
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
      If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", Application.ProductName & " could not navigate to ""realityripple.com""!" & vbNewLine & ex.Message, 200, 3000, 100)
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
        If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", Application.ProductName & " could not navigate to """ & mySettings.NetTestURL & """!" & vbNewLine & ex.Message, 200, 3000, 100)
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
        If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", Application.ProductName & " could not navigate to """ & mySettings.NetTestURL & """!" & vbNewLine & ex.Message, 200, 3000, 100)
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
            If taskNotifier IsNot Nothing Then taskNotifier.Show("Failed to run Web Browser", Application.ProductName & " could not navigate to ""srt.realityripple.com/faq.php""!" & vbNewLine & ex.Message, 200, 3000, 100)
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
    Net.ServicePointManager.SecurityProtocol = mySettings.SecurityProtocol
    If AppDataPath = Application.StartupPath & "\Config\" Then mySettings.HistoryDir = Application.StartupPath & "\Config\"
    If mySettings.Colors.MainDownA = Color.Transparent Then SetDefaultColors()
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
      trayIcon.Visible = Me.WindowState = FormWindowState.Minimized
    Else
      trayIcon.Visible = False
    End If
    If String.IsNullOrEmpty(mySettings.NetTestURL) Then
      pctNetTest.Visible = False
      pctNetTest.Cursor = Cursors.Default
    Else
      pctNetTest.Visible = True
      pctNetTest.Cursor = Cursors.Hand
      Dim sNetTestIco As String = IO.Path.Combine(AppDataPath, "netTest.png")
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
        wsFavicon = New clsFavicon(mySettings.NetTestURL)
      End If
      Dim sNetTestTitle As String = mySettings.NetTestURL
      If sNetTestTitle.Contains("://") Then sNetTestTitle = sNetTestTitle.Substring(sNetTestTitle.IndexOf("://") + 3)
      If sNetTestTitle.StartsWith("www.") Then sNetTestTitle = sNetTestTitle.Substring(4)
      If sNetTestTitle.Contains("/") Then sNetTestTitle = sNetTestTitle.Substring(0, sNetTestTitle.IndexOf("/"))
      ttUI.SetTooltip(pctNetTest, "Visit " & sNetTestTitle & ".")
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
      If ClosingTime Then Exit Sub
      DisplayUsage(False, False)
      SetStatusText(LOG_GetLast.ToString("g"), "Preparing Connection...", False)
      Dim UsageInvoker As New MethodInvoker(AddressOf GetUsage)
      UsageInvoker.BeginInvoke(Nothing, Nothing)
    End If
    If ClosingTime Then Exit Sub
    Dim sizeChangeInvoker As New EventHandler(AddressOf frmMain_SizeChanged)
    sizeChangeInvoker.BeginInvoke(Me, New EventArgs, Nothing, Nothing)
  End Sub
  Private Sub EnableProgressIcon()
    Try
      If ClosingTime Then Exit Sub
      tmrIcon.Enabled = True
    Catch ex As Exception

    End Try
  End Sub
  Private Sub StartTimer()
    If Me.InvokeRequired Then
      Me.Invoke(New MethodInvoker(AddressOf StartTimer))
    Else
      NextGrabTick = Long.MinValue
      SetTag(LoadStates.Loaded)
    End If
  End Sub
  Private Sub SetTag(Tag As LoadStates)
    If Me.InvokeRequired Then
      Me.Invoke(New ParamaterizedInvoker(AddressOf SetTag), Tag)
    Else
      myState = Tag
    End If
  End Sub
  Private Sub LookupProvider()
    SetTag(LoadStates.Lookup)
    SetStatusText("Loading History", "Reading usage history into memory...", False)
    LOG_Initialize(sAccount, False)
    If ClosingTime Then Exit Sub
    If mySettings.AccountType = SatHostTypes.Other Then
      If mySettings.AccountTypeForced Then
        SetStatusText(LOG_GetLast.ToString("g"), "Unknown Account Type.", True)
      Else
        SetStatusText("Analyzing Account", "Determining your account type...", False)
        TypeDetermination = New DetermineType(sProvider, mySettings.Timeout, mySettings.Proxy)
      End If
    Else
      tmrIcon.Enabled = False
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
          NextGrabTick = TickCount() + (msInterval - msSinceLast) + (2 * 60 * 1000)
        Else
          NextGrabTick = TickCount()
        End If
        If NextGrabTick - TickCount() < mySettings.StartWait * 60 * 1000 Then NextGrabTick = TickCount() + (mySettings.StartWait * 60 * 1000)
      End If
      If TickCount() >= NextGrabTick Then
        If Not mySettings.UpdateType = AppSettings.UpdateTypes.None AndAlso Math.Abs(DateDiff(DateInterval.Day, mySettings.LastUpdate, Now)) > mySettings.UpdateTime Then
          CheckForUpdates()
        Else
          If Not String.IsNullOrEmpty(sAccount) Then
            If String.IsNullOrEmpty(sProvider) Then
              sProvider = sAccount.Substring(sAccount.LastIndexOf("@") + 1).ToLower
              SetStatusText("Reloading", "Reloading History...", False)
              LOG_Initialize(sAccount, False)
              If ClosingTime Then Exit Sub
            End If
            If Math.Abs(DateDiff(DateInterval.Minute, LOG_GetLast, Now)) >= 10 Then
              If Not String.IsNullOrEmpty(sProvider) And Not String.IsNullOrEmpty(sPassword) Then
                NextGrabTick = Long.MaxValue
                EnableProgressIcon()
                SetStatusText(LOG_GetLast.ToString("g"), "Preparing Connection...", False)
                Dim UsageInvoker As New MethodInvoker(AddressOf GetUsage)
                UsageInvoker.BeginInvoke(Nothing, Nothing)
                Exit Sub
              End If
            End If
          End If
          NextGrabTick = TickCount() + msInterval
        End If
      End If
    End If
  End Sub
  Private Sub GetUsage()
    If Me.InvokeRequired Then
      Me.Invoke(New MethodInvoker(AddressOf GetUsage))
    Else
      If String.IsNullOrEmpty(sAccount) Or String.IsNullOrEmpty(sPassword) Or Not sAccount.Contains("@") Then
        If Not Me.Visible Then
          Me.Show()
          mnuRestore.Text = "&Focus"
        End If
        cmdConfig.Focus()
        MessageBox.Show("Please enter your account details in the configuration window.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
      Else
        If cmdRefresh.Enabled Then
          cmdRefresh.Enabled = False
          NextGrabTick = TickCount() + (mySettings.Timeout * 1000)
          If KeyCheck(mySettings.RemoteKey) Then
            Dim remoteCallback As New MethodInvoker(AddressOf GetRemoteUsage)
            remoteCallback.BeginInvoke(Nothing, Nothing)
          Else
            If localData IsNot Nothing Then
              localData.Dispose()
              localData = Nothing
            End If
            localData = New localRestrictionTracker(AppData)
            Dim connectInvoker As New MethodInvoker(AddressOf localData.Connect)
            connectInvoker.BeginInvoke(Nothing, Nothing)
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
          SetStatusText(LOG_GetLast.ToString("g"), "Preparing Connection...", False)
          DisplayUsage(False, False)
          NextGrabTick = TickCount() + 5000
        End If
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
    remoteData = New remoteRestrictionTracker(sAccount, sPassword, mySettings.RemoteKey, mySettings.Proxy, mySettings.Timeout, fromDate, AppData)
  End Sub
  Private Sub DisplayUsage(bStatusText As Boolean, bHardTime As Boolean)
    If Me.InvokeRequired Then
      Me.Invoke(New ParamaterizedInvoker2(AddressOf DisplayUsage), bStatusText, bHardTime)
    Else
      If Not cmdRefresh.Enabled Then cmdRefresh.Enabled = True
      If tmrIcon.Enabled Then tmrIcon.Enabled = False
      If bHardTime Then
        NextGrabTick = TickCount() + (mySettings.Interval * 60 * 1000)
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
      NextGrabTick = TickCount() + (MinutesAhead * 60 * 1000)
    Else
      NextGrabTick = TickCount() + (mySettings.Interval * 60 * 1000)
    End If
  End Sub
#End Region
#Region "Local Usage Events"
  Private Sub localData_ConnectionStatus(sender As Object, e As ConnectionStatusEventArgs) Handles localData.ConnectionStatus
    If Me.InvokeRequired Then
      Me.BeginInvoke(New EventHandler(AddressOf localData_ConnectionStatus), sender, e)
    Else
      NextGrabTick = TickCount() + (mySettings.Timeout * 1000)
      Select Case e.Status
        Case ConnectionStates.Initialize : SetStatusText(LOG_GetLast.ToString("g"), "Initializing Connection...", False)
        Case ConnectionStates.Prepare : SetStatusText(LOG_GetLast.ToString("g"), "Preparing to Log In...", False)
        Case ConnectionStates.Login
          Select Case e.SubState
            Case ConnectionSubStates.ReadLogin : SetStatusText(LOG_GetLast.ToString("g"), "Reading Login Page...", False)
            Case ConnectionSubStates.AuthPrepare : SetStatusText(LOG_GetLast.ToString("g"), "Preparing Authentication...", False)
            Case ConnectionSubStates.Authenticate : SetStatusText(LOG_GetLast.ToString("g"), "Authenticating...", False)
            Case ConnectionSubStates.AuthenticateRetry : SetStatusText(LOG_GetLast.ToString("g"), "Re-Authenticating...", False)
            Case ConnectionSubStates.Verify : SetStatusText(LOG_GetLast.ToString("g"), "Verifying Authentication...", False)
            Case Else : SetStatusText(LOG_GetLast.ToString("g"), "Logging In...", False)
          End Select
        Case ConnectionStates.TableDownload
          Select Case e.SubState
            Case ConnectionSubStates.LoadHome : SetStatusText(LOG_GetLast.ToString("g"), "Downloading Home Page...", False)
            Case ConnectionSubStates.LoadAJAX : SetStatusText(LOG_GetLast.ToString("g"), "Downloading AJAX Data (" & FormatPercent(e.SubPercentage, 0, TriState.False, TriState.False, TriState.False) & ")...", False)
            Case ConnectionSubStates.LoadTable : SetStatusText(LOG_GetLast.ToString("g"), "Downloading Usage Table...", False)
            Case ConnectionSubStates.LoadTableRetry : SetStatusText(LOG_GetLast.ToString("g"), "Re-Downloading Usage Table...", False)
            Case Else : SetStatusText(LOG_GetLast.ToString("g"), "Downloading Usage Table...", False)
          End Select
        Case ConnectionStates.TableRead : SetStatusText(LOG_GetLast.ToString("g"), "Reading Usage Table...", False)
      End Select
    End If
  End Sub
  Private Sub localData_ConnectionFailure(sender As Object, e As ConnectionFailureEventArgs) Handles localData.ConnectionFailure
    If Me.InvokeRequired Then
      Me.BeginInvoke(New EventHandler(AddressOf localData_ConnectionFailure), sender, e)
    Else
      Select Case e.Type
        Case ConnectionFailureEventArgs.FailureType.LoginIssue
          SetStatusText(LOG_GetLast.ToString("g"), e.Message, True)
          Exit Sub
        Case ConnectionFailureEventArgs.FailureType.ConnectionTimeout
          SetStatusText(LOG_GetLast.ToString("g"), "Connection Timed Out!", True)
          DisplayUsage(False, False)
        Case ConnectionFailureEventArgs.FailureType.LoginFailure
          SetStatusText(LOG_GetLast.ToString("g"), e.Message, True)
          If Not String.IsNullOrEmpty(e.Fail) Then FailFile(e.Fail)
          DisplayUsage(False, True)
        Case ConnectionFailureEventArgs.FailureType.FatalLoginFailure
          If Not mySettings.AccountTypeForced Then mySettings.AccountType = SatHostTypes.Other
          SetStatusText(LOG_GetLast.ToString("g"), e.Message, True)
          If Not String.IsNullOrEmpty(e.Fail) Then FailFile(e.Fail)
          DisplayUsage(False, False)
        Case ConnectionFailureEventArgs.FailureType.UnknownAccountDetails
          If Me.WindowState = FormWindowState.Minimized Then
            Me.WindowState = FormWindowState.Normal
            mnuRestore.Text = "&Focus"
          End If
          cmdConfig.Focus()
          MessageBox.Show("Please enter your account details in the configuration window.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
        Case ConnectionFailureEventArgs.FailureType.UnknownAccountType
          If mySettings.AccountTypeForced Then
            SetStatusText(LOG_GetLast.ToString("g"), "Unknown Account Type.", True)
          Else
            SetStatusText("Analyzing Account", "Determining your account type...", False)
            TypeDetermination = New DetermineType(sProvider, mySettings.Timeout, mySettings.Proxy)
          End If
      End Select
      If localData IsNot Nothing Then
        localData.Dispose()
        localData = Nothing
      End If
    End If
  End Sub
  Private Sub localData_ConnectionDNXResult(sender As Object, e As TYPEA2ResultEventArgs) Handles localData.ConnectionDNXResult
    If Me.InvokeRequired Then
      Me.BeginInvoke(New EventHandler(AddressOf localData_ConnectionDNXResult), sender, e)
    Else
      SetStatusText(e.Update.ToString("g"), "Saving History...", False)
      NextGrabTick = TickCount() + (mySettings.Interval * 60 * 1000)
      LOG_Add(e.Update, e.AnyTime, e.AnyTimeLimit, e.OffPeak, e.OffPeakLimit, True)
      myPanel = SatHostTypes.DishNet_EXEDE
      If Not mySettings.AccountTypeForced Then mySettings.AccountType = SatHostTypes.DishNet_EXEDE
      mySettings.Save()
      If mySettings.Colors.MainDownA = Color.Transparent Then SetDefaultColors()
      DisplayUsage(True, True)
      If localData IsNot Nothing Then
        localData.Dispose()
        localData = Nothing
      End If
      SaveToHostList()
    End If
  End Sub
  Private Sub localData_ConnectionRPXResult(sender As Object, e As TYPEBResultEventArgs) Handles localData.ConnectionRPXResult
    If Me.InvokeRequired Then
      Me.BeginInvoke(New EventHandler(AddressOf localData_ConnectionRPXResult), sender, e)
    Else
      SetStatusText(e.Update.ToString("g"), "Saving History...", False)
      NextGrabTick = TickCount() + (mySettings.Interval * 60 * 1000)
      LOG_Add(e.Update, e.Used, e.Limit, e.Used, e.Limit, True)
      myPanel = SatHostTypes.RuralPortal_EXEDE
      If Not mySettings.AccountTypeForced Then mySettings.AccountType = SatHostTypes.RuralPortal_EXEDE
      mySettings.Save()
      If mySettings.Colors.MainDownA = Color.Transparent Then SetDefaultColors()
      DisplayUsage(True, True)
      If localData IsNot Nothing Then
        localData.Dispose()
        localData = Nothing
      End If
      SaveToHostList()
    End If
  End Sub
  Private Sub localData_ConnectionRPLResult(sender As Object, e As TYPEAResultEventArgs) Handles localData.ConnectionRPLResult
    If Me.InvokeRequired Then
      Me.BeginInvoke(New EventHandler(AddressOf localData_ConnectionRPLResult), sender, e)
    Else
      SetStatusText(e.Update.ToString("g"), "Saving History...", False)
      NextGrabTick = TickCount() + (mySettings.Interval * 60 * 1000)
      LOG_Add(e.Update, e.Download, e.DownloadLimit, e.Upload, e.UploadLimit, True)
      myPanel = SatHostTypes.RuralPortal_LEGACY
      If Not mySettings.AccountTypeForced Then mySettings.AccountType = SatHostTypes.RuralPortal_LEGACY
      mySettings.Save()
      If mySettings.Colors.MainDownA = Color.Transparent Then SetDefaultColors()
      DisplayUsage(True, True)
      If localData IsNot Nothing Then
        localData.Dispose()
        localData = Nothing
      End If
      SaveToHostList()
    End If
  End Sub
  Private Sub localData_ConnectionWBLResult(sender As Object, e As TYPEAResultEventArgs) Handles localData.ConnectionWBLResult
    If Me.InvokeRequired Then
      Me.BeginInvoke(New EventHandler(AddressOf localData_ConnectionWBLResult), sender, e)
    Else
      SetStatusText(e.Update.ToString("g"), "Saving History...", False)
      NextGrabTick = TickCount() + (mySettings.Interval * 60 * 1000)
      LOG_Add(e.Update, e.Download, e.DownloadLimit, e.Upload, e.UploadLimit, True)
      myPanel = SatHostTypes.WildBlue_LEGACY
      If Not mySettings.AccountTypeForced Then mySettings.AccountType = SatHostTypes.WildBlue_LEGACY
      mySettings.Save()
      If mySettings.Colors.MainDownA = Color.Transparent Then SetDefaultColors()
      DisplayUsage(True, True)
      If localData IsNot Nothing Then
        localData.Dispose()
        localData = Nothing
      End If
      SaveToHostList()
    End If
  End Sub
  Private Sub localData_ConnectionWBXResult(sender As Object, e As TYPEBResultEventArgs) Handles localData.ConnectionWBXResult
    If Me.InvokeRequired Then
      Me.BeginInvoke(New EventHandler(AddressOf localData_ConnectionWBXResult), sender, e)
    Else
      SetStatusText(e.Update.ToString("g"), "Saving History...", False)
      NextGrabTick = TickCount() + (mySettings.Interval * 60 * 1000)
      LOG_Add(e.Update, e.Used, e.Limit, e.Used, e.Limit, True)
      myPanel = SatHostTypes.WildBlue_EXEDE
      If Not mySettings.AccountTypeForced Then mySettings.AccountType = SatHostTypes.WildBlue_EXEDE
      mySettings.Save()
      If mySettings.Colors.MainDownA = Color.Transparent Then SetDefaultColors()
      DisplayUsage(True, True)
      If localData IsNot Nothing Then
        localData.Dispose()
        localData = Nothing
      End If
      SaveToHostList()
    End If
  End Sub
#Region "Host List"
  Private didHostListSave As Boolean = False
  Private Sub SaveToHostList()
    If Me.InvokeRequired Then
      Me.BeginInvoke(New MethodInvoker(AddressOf SaveToHostList))
    Else
      If didHostListSave Then Exit Sub
      Try
        If wsHostList IsNot Nothing Then
          wsHostList.Dispose()
          wsHostList = Nothing
        End If
        Dim myProvider As String = mySettings.Account.Substring(mySettings.Account.LastIndexOf("@") + 1).ToLower
        wsHostList = New CookieAwareWebClient
        wsHostList.DownloadDataAsync(New Uri("http://wb.realityripple.com/hosts/?add=" & myProvider), "UPDATE")
        didHostListSave = True
      Catch ex As Exception
        didHostListSave = False
      End Try
    End If
  End Sub
#End Region
#End Region
#Region "Remote Usage Events"
  Private Sub remoteData_Failure(sender As Object, e As remoteRestrictionTracker.FailureEventArgs) Handles remoteData.Failure
    If Me.InvokeRequired Then
      Me.Invoke(New EventHandler(AddressOf remoteData_Failure), sender, e)
    Else
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
    End If
  End Sub
  Private Sub remoteData_OKKey(sender As Object, e As System.EventArgs) Handles remoteData.OKKey
    If Me.InvokeRequired Then
      Me.Invoke(New EventHandler(AddressOf remoteData_OKKey), sender, e)
    Else
      NextGrabTick = TickCount() + (mySettings.Timeout * 1000)
      SetStatusText(LOG_GetLast.ToString("g"), "Account Accessed! Getting Usage...", False)
    End If
  End Sub
  Private Sub remoteData_Success(sender As Object, e As remoteRestrictionTracker.SuccessEventArgs) Handles remoteData.Success
    If Me.InvokeRequired Then
      Me.Invoke(New EventHandler(AddressOf remoteData_Success), sender, e)
    Else
      NextGrabTick = TickCount() + (mySettings.Interval * 60 * 1000)
      Dim LastTime As String = LOG_GetLast.ToString("g")
      If FullCheck Then
        SetStatusText(LastTime, "Synchronizing History...", False)
      Else
        SetStatusText(LastTime, "Saving History...", False)
      End If
      If e IsNot Nothing Then
        If Not mySettings.AccountTypeForced Then mySettings.AccountType = e.Provider
        If mySettings.Colors.MainDownA = Color.Transparent Then SetDefaultColors()
        mySettings.Save()
        Dim iPercent As Integer = 0
        Dim iInterval As Integer = 1
        Dim iStart As Long = TickCount()
        ttUI.UseFading = False
        For I As Integer = 0 To e.Results.Length - 1
          Dim Row As remoteRestrictionTracker.SuccessEventArgs.Result = e.Results(I)
          If FullCheck Then
            If Math.Abs(iPercent - Math.Floor((I / (e.Results.Length - 1)) * 100)) >= iInterval Then
              iPercent = Math.Floor((I / (e.Results.Length - 1)) * 100)
              SetStatusText(LastTime, "Synchronizing History [" & iPercent & "%]...", False)
              If (iPercent = 4) Then
                Dim iDur As Long = TickCount() - iStart
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
        DisplayUsage(True, True)
      End If
      If remoteData IsNot Nothing Then
        remoteData.Dispose()
        remoteData = Nothing
      End If
      SaveToHostList()
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
    Else
      If tmrChanges IsNot Nothing Then
        tmrChanges.Dispose()
        tmrChanges = Nothing
      Else
        Exit Sub
      End If
      Select Case state
        Case "RURAL"
          Dim lUsed As Long = r_used
          Dim lLim As Long = r_lim
          Dim lRemain As Long = lLim - lUsed
          If lUsed <> 0 Or r_lim > 0 Or lRemain <> 0 Then
            DoChange(lblRuralUsedVal, lUsed)
            DoChange(lblRuralRemainVal, lRemain)
            DoChange(lblRuralAllowedVal, lLim)
          End If
          ResizePanels()
          If lUsed = 0 And lLim = 0 And lRemain = 0 Then
            AskForDonations()
            Exit Sub
          End If
        Case "WB"
          Dim lDown As Long = wb_down
          Dim lDLim As Long = wb_dlim
          Dim lUp As Long = wb_up
          Dim lULim As Long = wb_ulim
          Dim lDFree As Long = lDLim - lDown
          Dim lUFree As Long = lULim - lUp
          If lDown > 0 Or lDFree <> 0 Or lDLim > 0 Or lUp > 0 Or lUFree <> 0 Or lULim > 0 Then
            DoChange(lblDldUsed, lDown)
            DoChange(lblDldFree, lDFree)
            DoChange(lblDldTotal, lDLim)
            DoChange(lblUldUsed, lUp)
            DoChange(lblUldFree, lUFree)
            DoChange(lblUldTotal, lULim)
          End If
          ResizePanels()
          If lDown = 0 And lDFree = 0 And lDLim = 0 And lUp = 0 And lUFree = 0 And lULim = 0 Then
            AskForDonations()
            Exit Sub
          End If
      End Select
      If tmrChanges IsNot Nothing Then
        tmrChanges.Dispose()
        tmrChanges = Nothing
      End If
      tmrChanges = New Threading.Timer(New Threading.TimerCallback(AddressOf DisplayChangeInterval), state, 25, System.Threading.Timeout.Infinite)
    End If
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
  Private Sub DisplayRResults(lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long, sLastUpdate As String)
    If lDown <> lUp And lDownLim <> lUpLim Then
      DisplayWResults(lDown, lDownLim, lUp, lUpLim, sLastUpdate)
      Exit Sub
    End If
    Dim sTTT As String = Me.Text
    If lDown >= lDownLim Then imSlowed = True
    If mySettings.AccountType = SatHostTypes.WildBlue_EXEDE Or mySettings.AccountType = SatHostTypes.RuralPortal_EXEDE Or mySettings.AccountType = SatHostTypes.DishNet_EXEDE Then
      If lDown < lDownLim Then imSlowed = False
      If lDownLim = 150000 Then imSlowed = False
    Else
      If lDown < lDownLim * 0.7 Then imSlowed = False
    End If
    pnlWildBlue.Visible = False
    pnlExede.Visible = False
    pnlRural.Visible = True
    pnlNothing.Visible = False
    r_used = lDown
    r_lim = lDownLim
    If tmrChanges IsNot Nothing Then
      tmrChanges.Dispose()
      tmrChanges = Nothing
    End If
    tmrChanges = New Threading.Timer(New Threading.TimerCallback(AddressOf DisplayChangeInterval), "RURAL", 75, System.Threading.Timeout.Infinite)
    If imSlowed Then
      lblRuralUsedVal.ForeColor = Color.Red
    Else
      lblRuralUsedVal.ForeColor = SystemColors.ControlText
    End If
    pctRural.Image = DisplayRProgress(pctRural.DisplayRectangle.Size, lDown, lDownLim, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
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
    tmrIcon.Enabled = False
    trayIcon.Icon = CreateRTrayIcon(lDown, lDownLim)
    SetNotifyIconText(trayIcon, sTTT)
    DisplayResultAlert(mySettings.AccountType, lDown, lUp)
  End Sub
  Private Sub DisplayDResults(lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long, sLastUpdate As String)
    Dim sTTT As String = Me.Text
    If lDown >= lDownLim Then imSlowed = True
    If mySettings.AccountType = SatHostTypes.WildBlue_EXEDE Or mySettings.AccountType = SatHostTypes.RuralPortal_EXEDE Or mySettings.AccountType = SatHostTypes.DishNet_EXEDE Then
      If lDown < lDownLim Then imSlowed = False
    Else
      If lDown < lDownLim * 0.7 Then imSlowed = False
    End If
    pnlWildBlue.Visible = True
    pnlExede.Visible = False
    pnlRural.Visible = False
    pnlNothing.Visible = False
    wb_down = lDown
    wb_dlim = lDownLim
    wb_up = lUp
    wb_ulim = lUpLim
    If tmrChanges IsNot Nothing Then
      tmrChanges.Dispose()
      tmrChanges = Nothing
    End If
    tmrChanges = New Threading.Timer(New Threading.TimerCallback(AddressOf DisplayChangeInterval), "WB", 75, System.Threading.Timeout.Infinite)
    If imSlowed Then
      lblDldUsed.ForeColor = Color.Red
      lblUldUsed.ForeColor = Color.Red
    Else
      lblUldUsed.ForeColor = SystemColors.ControlText
      lblDldUsed.ForeColor = SystemColors.ControlText
    End If
    gbDld.Text = "Anytime (" & AccuratePercent(lDown / lDownLim) & ")"
    gbUld.Text = "Off-Peak (" & AccuratePercent(lUp / lUpLim) & ")"
    ttUI.SetTooltip(pctDld, "Graph representing your Anytime usage.")
    ttUI.SetTooltip(pctUld, "Graph representing your Off-Peak usage (used between 2am and 8am).")
    pctDld.Image = DisplayProgress(pctDld.DisplayRectangle.Size, lDown, lDownLim, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
    pctUld.Image = DisplayProgress(pctUld.DisplayRectangle.Size, lUp, lUpLim, mySettings.Accuracy, mySettings.Colors.MainUpA, mySettings.Colors.MainUpB, mySettings.Colors.MainUpC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
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
    tmrIcon.Enabled = False
    trayIcon.Icon = CreateTrayIcon(lDown, lDownLim, lUp, lUpLim)
    SetNotifyIconText(trayIcon, sTTT)
    DisplayResultAlert(mySettings.AccountType, lDown, lUp)
  End Sub
  Private Sub DisplayWResults(lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long, sLastUpdate As String)
    Dim sTTT As String = Me.Text
    If lDown >= lDownLim Or lUp >= lUpLim Then imSlowed = True
    If mySettings.AccountType = SatHostTypes.WildBlue_EXEDE Or mySettings.AccountType = SatHostTypes.RuralPortal_EXEDE Or mySettings.AccountType = SatHostTypes.DishNet_EXEDE Then
      If lDown < lDownLim And lUp < lUpLim Then imSlowed = False
    Else
      If lDown < lDownLim * 0.7 And lUp < lUpLim * 0.7 Then imSlowed = False
    End If
    pnlWildBlue.Visible = True
    pnlExede.Visible = False
    pnlRural.Visible = False
    pnlNothing.Visible = False
    wb_down = lDown
    wb_dlim = lDownLim
    wb_up = lUp
    wb_ulim = lUpLim
    If tmrChanges IsNot Nothing Then
      tmrChanges.Dispose()
      tmrChanges = Nothing
    End If
    tmrChanges = New Threading.Timer(New Threading.TimerCallback(AddressOf DisplayChangeInterval), "WB", 75, System.Threading.Timeout.Infinite)
    If imSlowed Then
      lblDldUsed.ForeColor = Color.Red
      lblUldUsed.ForeColor = Color.Red
    Else
      lblUldUsed.ForeColor = SystemColors.ControlText
      lblDldUsed.ForeColor = SystemColors.ControlText
    End If
    gbDld.Text = "Download (" & AccuratePercent(lDown / lDownLim) & ")"
    gbUld.Text = "Upload (" & AccuratePercent(lUp / lUpLim) & ")"
    ttUI.SetTooltip(pctDld, "Graph representing your download usage.")
    ttUI.SetTooltip(pctUld, "Graph representing your upload usage.")
    pctDld.Image = DisplayProgress(pctDld.DisplayRectangle.Size, lDown, lDownLim, mySettings.Accuracy, mySettings.Colors.MainDownA, mySettings.Colors.MainDownB, mySettings.Colors.MainDownC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
    pctUld.Image = DisplayProgress(pctUld.DisplayRectangle.Size, lUp, lUpLim, mySettings.Accuracy, mySettings.Colors.MainUpA, mySettings.Colors.MainUpB, mySettings.Colors.MainUpC, mySettings.Colors.MainText, mySettings.Colors.MainBackground)
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
    tmrIcon.Enabled = False
    trayIcon.Icon = CreateTrayIcon(lDown, lDownLim, lUp, lUpLim)
    SetNotifyIconText(trayIcon, sTTT)
    DisplayResultAlert(mySettings.AccountType, lDown, lUp)
  End Sub
  Private Sub DisplayResults(lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long)
    If lDownLim > 0 Or lUpLim > 0 Then
      Dim lastUpdate As Date = LOG_GetLast()
      Dim sLastUpdate As String = lastUpdate.ToString("M/d h:mm tt")
      myPanel = mySettings.AccountType
      Select Case mySettings.AccountType
        Case SatHostTypes.RuralPortal_EXEDE, SatHostTypes.WildBlue_EXEDE : DisplayRResults(lDown, lDownLim, lUp, lUpLim, sLastUpdate)
        Case SatHostTypes.DishNet_EXEDE : DisplayDResults(lDown, lDownLim, lUp, lUpLim, sLastUpdate)
        Case SatHostTypes.WildBlue_LEGACY, SatHostTypes.RuralPortal_LEGACY : DisplayWResults(lDown, lDownLim, lUp, lUpLim, sLastUpdate)
        Case Else : DisplayWResults(lDown, lDownLim, lUp, lUpLim, sLastUpdate)
      End Select
    Else
      pnlWildBlue.Visible = False
      pnlExede.Visible = False
      pnlRural.Visible = False
      pnlNothing.Visible = True
      myPanel = SatHostTypes.Other
      trayIcon.Text = Me.Text
      tmrIcon.Enabled = False
      trayIcon.Icon = MakeIcon(IconName.norm)
    End If
  End Sub
  Private Sub DisplayResultAlert(Type As localRestrictionTracker.SatHostTypes, lDown As Long, lUp As Long)
    If mySettings.Overuse > 0 Then
      If lastBalloon > 0 AndAlso TickCount() - lastBalloon < mySettings.Overtime * 60 * 1000 Then Exit Sub
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
                If taskNotifier IsNot Nothing Then taskNotifier.Show("Excessive Download Detected", Application.ProductName & " has logged a download of " & MBorGB(ChangeSize) & " in " & ConvertTime(ChangeTime) & "!", 200, 0, 100)
                lastBalloon = TickCount()
                Exit For
              ElseIf lUp - lItems(I).UPLOAD >= mySettings.Overuse Then
                Dim ChangeSize As Long = Math.Abs(lUp - lItems(I).UPLOAD)
                Dim ChangeTime As Long = Math.Abs(DateDiff(DateInterval.Minute, lItems(I).DATETIME, Now) * 60 * 1000)
                MakeNotifier(taskNotifier, False)
                If taskNotifier IsNot Nothing Then taskNotifier.Show("Excessive Upload Detected", Application.ProductName & " has logged an upload of " & MBorGB(ChangeSize) & " in " & ConvertTime(ChangeTime) & "!", 200, 0, 100)
                lastBalloon = TickCount()
                Exit For
              End If
            Case SatHostTypes.WildBlue_EXEDE, SatHostTypes.RuralPortal_EXEDE
              If lDown - lItems(I).DOWNLOAD >= mySettings.Overuse Then
                Dim ChangeSize As Long = Math.Abs(lDown - lItems(I).DOWNLOAD)
                Dim ChangeTime As Long = Math.Abs(DateDiff(DateInterval.Minute, lItems(I).DATETIME, Now) * 60 * 1000)
                MakeNotifier(taskNotifier, False)
                If taskNotifier IsNot Nothing Then taskNotifier.Show("Excessive Usage Detected", Application.ProductName & " has logged a usage change of " & MBorGB(ChangeSize) & " in " & ConvertTime(ChangeTime) & "!", 200, 0, 100)
                lastBalloon = TickCount()
                Exit For
              End If
            Case SatHostTypes.DishNet_EXEDE
              If lDown - lItems(I).DOWNLOAD >= mySettings.Overuse Then
                Dim ChangeSize As Long = Math.Abs(lDown - lItems(I).DOWNLOAD)
                Dim ChangeTime As Long = Math.Abs(DateDiff(DateInterval.Minute, lItems(I).DATETIME, Now) * 60 * 1000)
                MakeNotifier(taskNotifier, False)
                If taskNotifier IsNot Nothing Then taskNotifier.Show("Excessive Usage Detected", Application.ProductName & " has logged a usage change of " & MBorGB(ChangeSize) & " in " & ConvertTime(ChangeTime) & "!", 200, 0, 100)
                lastBalloon = TickCount()
                Exit For
              ElseIf lUp - lItems(I).UPLOAD >= mySettings.Overuse Then
                Dim ChangeSize As Long = Math.Abs(lUp - lItems(I).UPLOAD)
                Dim ChangeTime As Long = Math.Abs(DateDiff(DateInterval.Minute, lItems(I).DATETIME, Now) * 60 * 1000)
                MakeNotifier(taskNotifier, False)
                If taskNotifier IsNot Nothing Then taskNotifier.Show("Excessive Off-Peak Usage Detected", Application.ProductName & " has logged an Off-Peak usage change of " & MBorGB(ChangeSize) & " in " & ConvertTime(ChangeTime) & "!", 200, 0, 100)
                lastBalloon = TickCount()
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
        If ClosingTime Then Exit Sub
        cmdRefresh.Enabled = True
      End If
      SetStatusText(LOG_GetLast.ToString("g"), "Beginning Usage Request...", False)
      Dim UsageInvoker As New MethodInvoker(AddressOf GetUsage)
      UsageInvoker.BeginInvoke(Nothing, Nothing)
    Else
      If Not Me.Visible Then
        Me.Show()
        mnuRestore.Text = "&Focus"
      End If
      cmdConfig.Focus()
      MessageBox.Show("Please enter your account details in the configuration window.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
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
    Dim dRet As DialogResult
    Using dlgConfig As New frmConfig
      dRet = dlgConfig.ShowDialog(Me)
    End Using
    Dim WaitTime As Long = TickCount() + 2000
    If Not myState = LoadStates.Loaded Then
      If Not myState = LoadStates.Lookup Then
        Dim lookupInvoker As New MethodInvoker(AddressOf LookupProvider)
        lookupInvoker.BeginInvoke(Nothing, Nothing)
      End If
      Do Until myState = LoadStates.Loaded
        Application.DoEvents()
        Threading.Thread.Sleep(1)
        If TickCount() > WaitTime Then Exit Do
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
          If AppDataPath = Application.StartupPath & "\Config\" Then frmHistory.mySettings.HistoryDir = Application.StartupPath & "\Config\"
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
      Me.Show()
      mnuRestore.Text = "&Focus"
      Me.WindowState = FormWindowState.Normal
      If mySettings.TrayIconStyle = AppSettings.TrayStyles.Minimized Then trayIcon.Visible = False
      Me.Location = New Point((Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2)
    End If
    Dim myProc As Integer = 0
    Try
      myProc = Process.GetCurrentProcess.Id
    Catch ex As Exception
      myProc = 0
    End Try
    If myProc = 0 Then
      Dim myTitle As String = String.Empty
      Try
        myTitle = Me.Text
      Catch ex As Exception
        myTitle = String.Empty
      End Try
      If String.IsNullOrEmpty(myTitle) Then
        Me.Activate()
      Else
        AppActivate(myTitle)
      End If
    Else
      AppActivate(myProc)
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
        If AppDataPath = Application.StartupPath & "\Config\" Then frmHistory.mySettings.HistoryDir = Application.StartupPath & "\Config\"
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
    throb1
    throb2
    throb3
    throb4
    throb5
    throb7
    throb8
    throb9
    throb10
  End Enum
  Private Function MakeIcon(name As IconName, Optional icoX As Integer = -1, Optional icoY As Integer = -1) As Icon
    If icoX < 0 Then icoX = NativeMethods.GetSystemMetrics(NativeMethods.MetricsList.SM_CXSMICON)
    If icoY < 0 Then icoY = NativeMethods.GetSystemMetrics(NativeMethods.MetricsList.SM_CYSMICON)
    Dim large As Boolean = icoX > 16 Or icoY > 16
    Dim imgICO As New Bitmap(icoX, icoY)
    Using g As Graphics = Graphics.FromImage(imgICO)
      g.Clear(Color.Transparent)
      Select Case name
        Case IconName.norm
          g.DrawIcon(IIf(large, My.Resources.t32_norm, My.Resources.t16_norm), New Rectangle(0, 0, icoX, icoY))
        Case IconName.free
          g.DrawIcon(IIf(large, My.Resources.t32_free, My.Resources.t16_free), New Rectangle(0, 0, icoX, icoY))
        Case IconName.restricted
          g.DrawIcon(IIf(large, My.Resources.t32_restricted, My.Resources.t16_restricted), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb1
          g.DrawIcon(IIf(large, My.Resources.t32_1, My.Resources.t16_1), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb2
          g.DrawIcon(IIf(large, My.Resources.t32_2, My.Resources.t16_2), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb3
          g.DrawIcon(IIf(large, My.Resources.t32_3, My.Resources.t16_3), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb4
          g.DrawIcon(IIf(large, My.Resources.t32_4, My.Resources.t16_4), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb5
          g.DrawIcon(IIf(large, My.Resources.t32_5, My.Resources.t16_5), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb7
          g.DrawIcon(IIf(large, My.Resources.t32_7, My.Resources.t16_7), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb8
          g.DrawIcon(IIf(large, My.Resources.t32_8, My.Resources.t16_8), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb9
          g.DrawIcon(IIf(large, My.Resources.t32_9, My.Resources.t16_9), New Rectangle(0, 0, icoX, icoY))
        Case IconName.throb10
          g.DrawIcon(IIf(large, My.Resources.t32_10, My.Resources.t16_10), New Rectangle(0, 0, icoX, icoY))
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
      Static iItem As Integer
      Select Case iItem
        Case 0, 6, 11 : trayIcon.Icon = MakeIcon(IconName.norm)
        Case 1 : trayIcon.Icon = MakeIcon(IconName.throb1)
        Case 2 : trayIcon.Icon = MakeIcon(IconName.throb2)
        Case 3 : trayIcon.Icon = MakeIcon(IconName.throb3)
        Case 4 : trayIcon.Icon = MakeIcon(IconName.throb4)
        Case 5 : trayIcon.Icon = MakeIcon(IconName.throb5)
        Case 7 : trayIcon.Icon = MakeIcon(IconName.throb7)
        Case 8 : trayIcon.Icon = MakeIcon(IconName.throb8)
        Case 9 : trayIcon.Icon = MakeIcon(IconName.throb9)
        Case 10 : trayIcon.Icon = MakeIcon(IconName.throb10)
      End Select
      iItem += 1
      If iItem >= 12 Then iItem = 0
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
    If Not String.IsNullOrEmpty(sFailTray) Then
      Dim tFTP As New ParamaterizedInvoker(AddressOf SaveToFTP)
      tFTP.BeginInvoke(sFailTray, Nothing, Nothing)
    End If
    sFailTray = Nothing
  End Sub
#Region "Graphs"
  Private Function CreateTrayIcon(lDown As Long, lDownLim As Long, lUp As Long, lUpLim As Long) As Icon
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
  Private Function CreateRTrayIcon(lUsed As Long, lLim As Long) As Icon
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
      NextGrabTick = TickCount() + (mySettings.Timeout * 1000)
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
      Me.Invoke(New EventHandler(AddressOf updateChecker_CheckResult), sender, e)
    Else
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
    End If
  End Sub
  Private Sub updateChecker_DownloadingUpdate(sender As Object, e As System.EventArgs) Handles updateChecker.DownloadingUpdate
    If Me.InvokeRequired Then
      Me.Invoke(New EventHandler(AddressOf updateChecker_DownloadingUpdate), sender, e)
    Else
      tmrSpeed.Enabled = True
      SetStatusText(LOG_GetLast.ToString("g"), "Downloading Software Update...", False)
    End If
  End Sub
  Private Sub updateChecker_DownloadResult(sender As Object, e As clsUpdate.DownloadEventArgs) Handles updateChecker.DownloadResult
    If Me.InvokeRequired Then
      Me.Invoke(New EventHandler(AddressOf updateChecker_DownloadResult), sender, e)
    Else
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
        Application.DoEvents()
        If My.Computer.FileSystem.FileExists(sEXEPath) Then
          Try
            ShellEx(sEXEPath, UpdateParam)
            Application.Exit()
          Catch ex As Exception
            MessageBox.Show("There was an error starting the update. If you have User Account Control enabled, please allow the " & Application.ProductName & " Installer to run." & vbNewLine & vbNewLine & ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            SetStatusText(LOG_GetLast.ToString("g"), "Software Update Failure!", True)
            NextGrabTick = Long.MinValue
          End Try
        Else
          SetStatusText(LOG_GetLast.ToString("g"), "Software Update Failure!", True)
          NextGrabTick = Long.MinValue
        End If
      End If
    End If
  End Sub
  Private CurSize As Long
  Private TotalSize As Long
  Private DownSpeed As ULong
  Private CurPercent As Integer
  Private Sub updateChecker_UpdateProgressChanged(sender As Object, e As clsUpdate.ProgressEventArgs) Handles updateChecker.UpdateProgressChanged
    If Me.InvokeRequired Then
      Me.Invoke(New EventHandler(AddressOf updateChecker_UpdateProgressChanged), sender, e)
    Else
      CurSize = e.BytesReceived
      TotalSize = e.TotalBytesToReceive
      CurPercent = e.ProgressPercentage
    End If
  End Sub
  Private LastSize As Long
  Private Sub tmrSpeed_Tick(sender As Object, e As System.EventArgs) Handles tmrSpeed.Tick
    If CurSize > LastSize Then
      DownSpeed = CurSize - LastSize
    Else
      DownSpeed = 0
    End If
    LastSize = CurSize
    Dim sProgress As String = "(" & CurPercent & "%)"
    Dim sStatus As String = ByteSize(CurSize) & " of " & ByteSize(TotalSize) & " at " & ByteSize(DownSpeed) & "/s..."
    If TotalSize = 0 Then
      sStatus = "Downloading Update (Waiting for Response)..."
      sProgress = "(Waiting for Response)"
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
              If lastAsk > 3 Or lastAsk < -12 Then
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
      mySettings.LastNag = DateAdd(DateInterval.Month, 12, Today)
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
  Public Sub FailResponse(sRet As Boolean)
    If Me.InvokeRequired Then
      Me.Invoke(New ParamaterizedInvoker(AddressOf FailResponse), sRet)
    Else
      MakeNotifier(taskNotifier, False)
      If taskNotifier IsNot Nothing Then
        If sRet Then
          taskNotifier.Show("Error Report Sent", "Your report has been received by " & Application.CompanyName & "." & vbNewLine & "Thank you for helping to improve " & Application.ProductName & "!", 200, 15 * 1000, 100)
        Else
          taskNotifier.Show("Error Reporting Error", Application.ProductName & " was unable to contact the " & Application.CompanyName & " servers. Please check your internet connection.", 200, 30 * 1000, 100)
        End If
      End If
    End If
  End Sub
  Private Sub FailFile(sFail As String)
    If clsUpdate.QuickCheckVersion = clsUpdate.CheckEventArgs.ResultType.NoUpdate Then
      sFailTray = sFail
      MakeNotifier(taskNotifier, True)
      If taskNotifier IsNot Nothing Then taskNotifier.Show("Error Reading Page Data", Application.ProductName & " encountered data it does not understand." & vbNewLine & "Click this alert to report the problem to " & Application.CompanyName & ".", 200, 3 * 60 * 1000, 100)
    End If
  End Sub
#End Region
  Private Delegate Sub SetStatusTextCallBack(Status As String, Details As String, Alert As Boolean)
  Private Sub SetStatusText(Status As String, Details As String, Alert As Boolean)
    If Me.InvokeRequired Then
      Me.Invoke(New SetStatusTextCallBack(AddressOf SetStatusText), Status, Details, Alert)
    Else
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
    End If
  End Sub
  Private Sub tmrStatus_Tick(sender As System.Object, e As System.EventArgs) Handles tmrStatus.Tick
    Dim lNext As Long = NextGrabTick
    Dim lNow As Long = TickCount()
    If lNext = Long.MaxValue Then
      ttUI.SetTooltip(lblStatus, "Update Temporarily Paused")
    ElseIf lNext = Long.MinValue Then
      ttUI.SetTooltip(lblStatus, "Next Update is Being Calculated")
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
      ttUI.SetTooltip(lblStatus, sDispTT)
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
          MessageBox.Show("The Satellite Restriction Logger Service Controller was not found!" & vbNewLine & "Please reinstall " & Application.ProductName & ".", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
          mySettings.Service = False
        End If
      End If
    Catch ex As Exception
      MessageBox.Show("Could not start the Satellite Restriction Logger Service Controller!" & vbNewLine & ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
    End Try
  End Sub
  Private Sub SetDefaultColors()
    If Me.InvokeRequired Then
      Me.Invoke(New MethodInvoker(AddressOf SetDefaultColors))
    Else
      mySettings.Colors = GetDefaultColors(mySettings.AccountType)
    End If
  End Sub
  Private Sub wsFavicon_DownloadIconCompleted(sender As Object, e As clsFavicon.DownloadIconCompletedEventArgs) Handles wsFavicon.DownloadIconCompleted
    Try
      pctNetTest.Visible = True
      If e.Error IsNot Nothing Then
        pctNetTest.Image = My.Resources.ico_err
      Else
        e.Icon16.Save(IO.Path.Combine(AppDataPath, "netTest.png"))
        pctNetTest.Image = e.Icon16
      End If
    Catch ex As Exception
      pctNetTest.Image = My.Resources.ico_err
    End Try
  End Sub
#End Region
End Class
