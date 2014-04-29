<System.ComponentModel.RunInstaller(True)> Partial Class piRL
  Inherits System.Configuration.Install.Installer

  'Installer overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Required by the Component Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Component Designer
  'It can be modified using the Component Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.spiRL = New System.ServiceProcess.ServiceProcessInstaller()
    Me.siRL = New System.ServiceProcess.ServiceInstaller()
    '
    'spiRL
    '
    Me.spiRL.Account = System.ServiceProcess.ServiceAccount.LocalSystem
    Me.spiRL.Password = Nothing
    Me.spiRL.Username = Nothing
    '
    'siRL
    '
    Me.siRL.DelayedAutoStart = True
    Me.siRL.Description = "The RestrictionLogger service logs Satellite network usage and limits."
    Me.siRL.DisplayName = "Satellite Restriction Logger"
    Me.siRL.ServiceName = "RestrictionLogger"
    Me.siRL.ServicesDependedOn = New String() {"Network Connections"}
    '
    'piRL
    '
    Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.spiRL, Me.siRL})

  End Sub
  Friend WithEvents spiRL As System.ServiceProcess.ServiceProcessInstaller
  Friend WithEvents siRL As System.ServiceProcess.ServiceInstaller

End Class
