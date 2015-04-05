Imports RestrictionLibrary
Public Class svcRL
  Private Const LATIN_1 As Integer = 28591
  Private MySettings As Settings
  Private sAccount, sPassword, sProvider As String
  Private tmrCheck As System.Threading.Timer
  Private myLog As EventLog
  Private DataPath As String
  Private WithEvents tracker As localRestrictionTracker
  Private typeDetermination As DetermineType
  Private Class DetermineType
    Public Shared Function Determine(Provider As String, Timeout As Integer, Proxy As Net.IWebProxy) As SatHostTypes
      If Provider.ToLower = "dish.com" Or Provider.ToLower = "dish.net" Then Return SatHostTypes.DishNet
      If Provider.ToLower = "exede.com" Or Provider.ToLower = "exede.net" Then Return SatHostTypes.Exede
      If Provider.Contains(".") Then Provider = Provider.Substring(0, Provider.LastIndexOf("."))
      Provider = Provider & ".ruralportal.net"
      If CheckURL("wildblue.com", Timeout, Proxy) Then
        If CheckURL(Provider, Timeout, Proxy) Then
          Return SatHostTypes.RuralPortal
        Else
          Dim rpP, exP, wbP As Single
          OfflineStats(rpP, exP, wbP)
          If rpP = 0 And exP = 0 And wbP = 0 Then
            Return SatHostTypes.WildBlue
          Else
            If rpP > exP And rpP > wbP Then
              Return SatHostTypes.Exede
            ElseIf exP > rpP And exP > wbP Then
              Return SatHostTypes.Exede
            ElseIf wbP > rpP And wbP > exP Then
              Return SatHostTypes.WildBlue
            Else
              If rpP > wbP And exP > wbP And rpP = exP Then
                Return SatHostTypes.Exede
              Else
                Return SatHostTypes.WildBlue
              End If
            End If
          End If
        End If
      Else
        Return OfflineCheck()
      End If
    End Function
    Private Shared Sub OfflineStats(ByRef rpP As Single, ByRef exP As Single, ByRef wbP As Single)
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
    Private Shared Function OfflineCheck() As SatHostTypes
      Dim rpP, exP, wbP As Single
      OfflineStats(rpP, exP, wbP)
      If rpP = 0 And exP = 0 And wbP = 0 Then
        Return SatHostTypes.WildBlue
      Else
        If rpP > exP And rpP > wbP Then
          Return SatHostTypes.RuralPortal
        ElseIf exP > rpP And exP > wbP Then
          Return SatHostTypes.Exede
        ElseIf wbP > rpP And wbP > exP Then
          Return SatHostTypes.WildBlue
        Else
          If rpP > wbP And exP > wbP And rpP = exP Then
            Return SatHostTypes.Exede
          Else
            Return SatHostTypes.Other
            Stop
          End If
        End If
      End If
    End Function
    Private Shared Function CheckURL(HostAddress As String, iTimeout As Integer, pProxy As Net.IWebProxy) As Boolean
      Dim sAddr As String = HostAddress
      If HostAddress.IndexOf("://") < 0 Then HostAddress = "http://" & HostAddress
      Dim wRequest As Net.WebRequest
      wRequest = System.Net.WebRequest.Create(HostAddress)
      wRequest.Timeout = iTimeout
      wRequest.Proxy = pProxy
      Try
        Dim wResponse As Net.WebResponse = wRequest.GetResponse
        If wResponse.ResponseUri.AbsoluteUri.ToString.IndexOf(sAddr) > -1 Then
          Dim sData As String = Nothing
          Using wData As IO.Stream = wResponse.GetResponseStream
            Using readStream As New IO.StreamReader(wData, System.Text.Encoding.GetEncoding(LATIN_1))
              sData = readStream.ReadToEnd
            End Using
          End Using
          If String.IsNullOrEmpty(sData) Then
            Return False
          ElseIf sData.ToLower.Contains("<meta http-equiv=""refresh""") Then
            Return False
          Else
            Return True
          End If
        Else
          Return False
        End If
      Catch ex As Exception
        Return False
      End Try
    End Function
  End Class
  Protected Overrides Sub OnStart(ByVal args() As String)
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
      tmrCheck = New System.Threading.Timer(New Threading.TimerCallback(AddressOf tmrCheck_Tick), tmrCheck, 5000, 1000)
      MySettings = New Settings(DataPath & "\user.config")
      If MySettings Is Nothing Then
        If myLog IsNot Nothing Then myLog.WriteEntry("Settings failed to load.", EventLogEntryType.Warning)
      Else
        InitAccount()
        If MySettings.AccountType = localRestrictionTracker.SatHostTypes.Other Then
          Try
            MySettings.AccountType = DetermineType.Determine(sProvider, MySettings.Timeout, MySettings.Proxy)
          Catch ex As Exception
            If myLog IsNot Nothing Then myLog.WriteEntry("Failed to determine type: " & ex.Message, EventLogEntryType.Warning)
            MySettings.AccountType = localRestrictionTracker.SatHostTypes.Other
          End Try
        End If
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
    tmrCheck = New System.Threading.Timer(New Threading.TimerCallback(AddressOf tmrCheck_Tick), tmrCheck, 5000, 1000)
    MyBase.OnContinue()
  End Sub
  Private Sub tmrCheck_Tick(state As Object)
    Static iChecks As Integer
    iChecks += 1
    If iChecks = MySettings.Interval * 60 Or iChecks = 1 Then
      iChecks = 1
      InitAccount()
      If Not String.IsNullOrEmpty(sAccount) And Not String.IsNullOrEmpty(sProvider) And Not String.IsNullOrEmpty(sPassword) Then tracker.Connect()
    End If
  End Sub
  Private Sub InitAccount()
    Try
      tracker = New localRestrictionTracker(DataPath)
      MySettings = New Settings(DataPath & "\user.config")
      If Not String.IsNullOrEmpty(MySettings.PassCrypt) Then
        sPassword = StoredPassword.DecryptLogger(MySettings.PassCrypt)
      End If
      If Not sAccount = MySettings.Account Then
        sAccount = MySettings.Account
        If Not String.IsNullOrEmpty(sAccount) AndAlso (sAccount.Contains("@") And sAccount.Contains(".")) Then
          sProvider = sAccount.Substring(sAccount.LastIndexOf("@") + 1).ToLower
          If My.Computer.FileSystem.FileExists(DataPath & "\History-" & sAccount & ".wb") Then
            LOG_Initialize(DataPath & "\History-" & sAccount & ".wb")
          ElseIf My.Computer.FileSystem.FileExists(DataPath & "\History-" & sAccount & ".xml") Then
            LOG_Initialize(DataPath & "\History-" & sAccount & ".xml")
          Else
            LOG_Initialize(DataPath & "\History-" & sAccount & ".wb")
          End If
          If MySettings.AccountType = localRestrictionTracker.SatHostTypes.Other Then MySettings.AccountType = DetermineType.Determine(sProvider, MySettings.Timeout, MySettings.Proxy)
        Else
          MySettings.AccountType = localRestrictionTracker.SatHostTypes.Other
          sAccount = String.Empty
          sProvider = String.Empty
        End If
      End If
    Catch ex As Exception
      If myLog IsNot Nothing Then myLog.WriteEntry("Error on InitAccount: " & ex.Message, EventLogEntryType.Error, 2)
    End Try
  End Sub
  Private Sub tracker_ConnectionDNXResult(sender As Object, e As RestrictionLibrary.localRestrictionTracker.TYPEA2ResultEventArgs) Handles tracker.ConnectionDNXResult
    MySettings.AccountType = localRestrictionTracker.SatHostTypes.DishNet_EXEDE
    LOG_Add(e.Update, e.AnyTime, e.AnyTimeLimit, e.OffPeak, e.OffPeakLimit)
  End Sub
  Private Sub tracker_ConnectionFailure(sender As Object, e As localRestrictionTracker.ConnectionFailureEventArgs) Handles tracker.ConnectionFailure
    If myLog IsNot Nothing Then myLog.WriteEntry(e.Type.ToString & ": " & e.Message & " (" & e.Fail & ")", EventLogEntryType.Error, 3)
  End Sub
  Private Sub tracker_ConnectionRPXResult(sender As Object, e As RestrictionLibrary.localRestrictionTracker.TYPEBResultEventArgs) Handles tracker.ConnectionRPXResult
    MySettings.AccountType = localRestrictionTracker.SatHostTypes.RuralPortal_EXEDE
    LOG_Add(e.Update, e.Used, e.Limit, e.Used, e.Limit)
  End Sub
  Private Sub tracker_ConnectionRPLResult(sender As Object, e As RestrictionLibrary.localRestrictionTracker.TYPEAResultEventArgs) Handles tracker.ConnectionRPLResult
    MySettings.AccountType = localRestrictionTracker.SatHostTypes.RuralPortal_LEGACY
    LOG_Add(e.Update, e.Download, e.DownloadLimit, e.Upload, e.UploadLimit)
  End Sub
  Private Sub tracker_ConnectionStatus(sender As Object, e As localRestrictionTracker.ConnectionStatusEventArgs) Handles tracker.ConnectionStatus
    'If myLog IsNot Nothing Then myLog.WriteEntry(e.Status.ToString, EventLogEntryType.Information, 16)
  End Sub
  Private Sub tracker_ConnectionWBLResult(sender As Object, e As RestrictionLibrary.localRestrictionTracker.TYPEAResultEventArgs) Handles tracker.ConnectionWBLResult
    MySettings.AccountType = localRestrictionTracker.SatHostTypes.WildBlue_LEGACY
    LOG_Add(e.Update, e.Download, e.DownloadLimit, e.Upload, e.UploadLimit)
  End Sub
  Private Sub tracker_ConnectionWBXResult(sender As Object, e As RestrictionLibrary.localRestrictionTracker.TYPEBResultEventArgs) Handles tracker.ConnectionWBXResult
    MySettings.AccountType = localRestrictionTracker.SatHostTypes.WildBlue_EXEDE
    LOG_Add(e.Update, e.Used, e.Limit, e.Used, e.Limit)
  End Sub
End Class
