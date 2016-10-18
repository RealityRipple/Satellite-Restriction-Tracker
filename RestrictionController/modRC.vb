Module modRC
  Sub Main()
    If Array.Exists(ServiceProcess.ServiceController.GetServices, Function(scService As ServiceProcess.ServiceController) scService.ServiceName = "RestrictionLogger") Then
      Select Case Command()
        Case "/run"
          Run()
        Case "/stop"
          [Stop]()
        Case Else
          Console.Clear()
          Console.WriteLine("==Restriction Service Controller==")
          Console.WriteLine()
          Dim sC As New ServiceProcess.ServiceController("RestrictionLogger")
          sC.Refresh()
          Console.Write("The service is: ")
          Select Case sC.Status
            Case ServiceProcess.ServiceControllerStatus.Running : Console.WriteLine("Running")
            Case ServiceProcess.ServiceControllerStatus.StartPending : Console.WriteLine("Starting")
            Case ServiceProcess.ServiceControllerStatus.ContinuePending : Console.WriteLine("Continuing")
            Case ServiceProcess.ServiceControllerStatus.Paused : Console.WriteLine("Paused")
            Case ServiceProcess.ServiceControllerStatus.PausePending : Console.WriteLine("Pausing")
            Case ServiceProcess.ServiceControllerStatus.Stopped : Console.WriteLine("Stopped")
            Case ServiceProcess.ServiceControllerStatus.StopPending : Console.WriteLine("Stopping")
          End Select
          Console.WriteLine()
          Console.WriteLine("Would you like to run or stop the Satellite Restriction Logger Service?")
          Console.Write("[R]un, [S]top, [C]ancel ")
          Select Case Console.ReadKey.Key
            Case ConsoleKey.R : Run()
            Case ConsoleKey.S : [Stop]()
            Case ConsoleKey.C : Return
            Case Else : Main()
          End Select
      End Select
    Else
      Console.Clear()
      Console.WriteLine("==Restriction Service Controller==")
      Console.WriteLine()
      Console.WriteLine("The Satellite Restriction Logger Service is not installed.")
      Console.WriteLine("Please re-install Satellite Restriction Tracker.")
      System.Threading.Thread.Sleep(5000)
    End If
  End Sub
  Private Sub Run()
    Console.Clear()
    Console.WriteLine("==Restriction Service Controller==")
    Console.WriteLine()
    Dim sC As New ServiceProcess.ServiceController("RestrictionLogger")
    sC.Refresh()
    If sC.Status = ServiceProcess.ServiceControllerStatus.Running Or sC.Status = ServiceProcess.ServiceControllerStatus.StartPending Then
      Console.WriteLine("Service already running!")
    Else
      Console.Write("Running Service.")
      sC.Start()
      sC.Refresh()
      Do
        Threading.Thread.Sleep(250)
        sC.Refresh()
        Console.Write(".")
      Loop While sC.Status = ServiceProcess.ServiceControllerStatus.StartPending
      Console.WriteLine()
      Select Case sC.Status
        Case ServiceProcess.ServiceControllerStatus.Running : Console.WriteLine("Service Started!")
        Case ServiceProcess.ServiceControllerStatus.Stopped : Console.WriteLine("Service Stopped!")
        Case Else : Console.WriteLine("Service could not be started!")
      End Select
    End If
  End Sub
  Private Sub [Stop]()
    Console.Clear()
    Console.WriteLine("==Restriction Service Controller==")
    Console.WriteLine()
    Dim sC As New ServiceProcess.ServiceController("RestrictionLogger")
    sC.Refresh()
    If sC.Status = ServiceProcess.ServiceControllerStatus.Stopped Or sC.Status = ServiceProcess.ServiceControllerStatus.StopPending Then
      Console.WriteLine("Service already stopped!")
    Else
      If sC.CanStop Then
        Console.Write("Stopping Service.")
        sC.Stop()
        sC.Refresh()
        Do
          Threading.Thread.Sleep(250)
          sC.Refresh()
          Console.Write(".")
        Loop While sC.Status = ServiceProcess.ServiceControllerStatus.StopPending
        Console.WriteLine()
        Select Case sC.Status
          Case ServiceProcess.ServiceControllerStatus.Stopped : Console.WriteLine("Service Stopped!")
          Case Else : Console.WriteLine("Service could not be stopped!")
        End Select
      Else
        Console.WriteLine("Can't stop service!")
      End If
    End If
  End Sub
End Module
