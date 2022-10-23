Imports RestrictionLibrary
Friend Class svcRL
  Private Const LATIN_1 As Integer = 28591
  Private MySettings As Settings
  Private sAccount, sPassword As String
  Private tmrCheck As System.Threading.Timer
  Private myLog As EventLog
  Private DataPath As String
  Private WithEvents tracker As Local.SiteConnection
  Protected Overrides Sub OnStart(ByVal args() As String)
    Dim v As NativeMethods.Validity = Authenticode.IsSelfSigned(Reflection.Assembly.GetExecutingAssembly().Location)
    If Not (v = NativeMethods.Validity.SignedAndValid Or v = NativeMethods.Validity.SignedButUntrusted) Then
      Me.Stop()
      Return
    End If
    Try
      If Not EventLog.SourceExists("Restriction Logger") Then
        EventLog.CreateEventSource("Restriction Logger", "Application")
        Dim startWait As Long = TickCount()
        Do Until EventLog.SourceExists("Restriction Logger")
          Threading.Thread.Sleep(1)
          If TickCount() - startWait >= 5000 Then Exit Do
        Loop
      End If
      If EventLog.SourceExists("Restriction Logger") Then
        myLog = New EventLog("Application")
        myLog.Source = "Restriction Logger"
      End If
    Catch ex As Exception
      myLog = Nothing
    End Try
    Try
      DataPath = IO.Path.GetDirectoryName(My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData)
      tmrCheck = New System.Threading.Timer(New Threading.TimerCallback(AddressOf tmrCheck_Tick), DataPath, 5000, 1000)
      MySettings = New Settings(IO.Path.Combine(DataPath, "user.config"))
      If MySettings Is Nothing Then
        If myLog IsNot Nothing Then myLog.WriteEntry("Settings failed to load.", EventLogEntryType.Warning)
      Else
        InitAccount()
      End If
      My.Computer.FileSystem.DeleteDirectory(My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData, FileIO.DeleteDirectoryOption.DeleteAllContents)
      MyBase.OnStart(args)
    Catch ex As Exception
      If myLog IsNot Nothing Then myLog.WriteEntry("Error on Start: " & ex.Message, EventLogEntryType.Error, 1)
    End Try
  End Sub
  Protected Overrides Sub OnStop()
    LOG_Terminate(False)
    tmrCheck.Dispose()
    MyBase.OnStop()
  End Sub
  Protected Overrides Sub OnShutdown()
    tmrCheck.Dispose()
    LOG_Terminate(False)
    MyBase.OnShutdown()
  End Sub
  Protected Overrides Sub OnPause()
    tmrCheck.Dispose()
    MyBase.OnPause()
  End Sub
  Protected Overrides Sub OnContinue()
    tmrCheck = New System.Threading.Timer(New Threading.TimerCallback(AddressOf tmrCheck_Tick), DataPath, 5000, 1000)
    MyBase.OnContinue()
  End Sub
  Private Sub tmrCheck_Tick(state As Object)
    Static iChecks As Integer
    iChecks += 1
    If iChecks = MySettings.Interval * 60 Or iChecks = 1 Then
      Dim dataPath As String = state
      iChecks = 1
      InitAccount()
      If Not String.IsNullOrEmpty(sAccount) And Not String.IsNullOrEmpty(sPassword) Then tracker = New Local.SiteConnection(dataPath)
    End If
  End Sub
  Private Sub InitAccount()
    Try
      MySettings = New Settings(IO.Path.Combine(DataPath, "user.config"))
      If Not String.IsNullOrEmpty(MySettings.PassCrypt) Then
        If String.IsNullOrEmpty(MySettings.PassKey) Or String.IsNullOrEmpty(MySettings.PassSalt) Then
          sPassword = StoredPasswordLegacy.DecryptLogger(MySettings.PassCrypt)
        Else
          sPassword = StoredPassword.Decrypt(MySettings.PassCrypt, MySettings.PassKey, MySettings.PassSalt)
        End If
      End If
      If Not sAccount = MySettings.Account Then
        sAccount = MySettings.Account
        If Not String.IsNullOrEmpty(sAccount) Then
          If (sAccount.Contains("@") And sAccount.Contains(".")) Then sAccount = sAccount.Substring(0, sAccount.LastIndexOf("@"))
          If My.Computer.FileSystem.FileExists(IO.Path.Combine(DataPath, "History-" & sAccount & ".wb")) Then
            LOG_Initialize(IO.Path.Combine(DataPath, "History-" & sAccount & ".wb"))
          ElseIf My.Computer.FileSystem.FileExists(IO.Path.Combine(DataPath, "History-" & sAccount & ".xml")) Then
            LOG_Initialize(IO.Path.Combine(DataPath, "History-" & sAccount & ".xml"))
          Else
            LOG_Initialize(IO.Path.Combine(DataPath, "History-" & sAccount & ".wb"))
          End If
        End If
      End If
    Catch ex As Exception
      If myLog IsNot Nothing Then myLog.WriteEntry("Error on InitAccount: " & ex.Message, EventLogEntryType.Error, 2)
    End Try
  End Sub
  Private Sub tracker_ConnectionFailure(sender As Object, e As Local.SiteConnectionFailureEventArgs) Handles tracker.ConnectionFailure
    If myLog IsNot Nothing Then myLog.WriteEntry(e.Type.ToString & ": " & e.Message & " (" & e.Fail & ")", EventLogEntryType.Error, 3)
  End Sub
  Private Sub tracker_ConnectionStatus(sender As Object, e As Local.SiteConnectionStatusEventArgs) Handles tracker.ConnectionStatus
  End Sub
  Private Sub tracker_ConnectionResult(sender As Object, e As Local.SiteResultEventArgs) Handles tracker.ConnectionResult
    LOG_Add(e.Update, e.Used, e.Limit)
  End Sub
End Class
