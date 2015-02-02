Public Class clsUpdate
  Implements IDisposable
  Private Const VersionURL As String = "http://update.realityripple.com/Satellite_Restriction_Tracker/ver"
  Class ProgressEventArgs
    Inherits EventArgs
    Public BytesReceived As Long
    Public ProgressPercentage As Integer
    Public TotalBytesToReceive As Long
    Friend Sub New(bReceived As Long, bToReceive As Long, iPercentage As Integer)
      BytesReceived = bReceived
      TotalBytesToReceive = bToReceive
      ProgressPercentage = iPercentage
    End Sub
  End Class
  Class CheckEventArgs
    Inherits System.ComponentModel.AsyncCompletedEventArgs
    Enum ResultType
      NoUpdate
      NewUpdate
      NewBeta
    End Enum
    Public Result As ResultType
    Public Version As String
    Friend Sub New(rtResult As ResultType, sVersion As String, ex As Exception, bCancelled As Boolean, state As Object)
      MyBase.New(ex, bCancelled, state)
      Version = sVersion
      Result = rtResult
    End Sub
    Friend Sub New(rtResult As ResultType, sVersion As String, e As System.ComponentModel.AsyncCompletedEventArgs)
      MyBase.New(e.Error, e.Cancelled, e.UserState)
      Version = sVersion
      Result = rtResult
    End Sub
  End Class
  Class DownloadEventArgs
    Inherits System.ComponentModel.AsyncCompletedEventArgs
    Public Version As String
    Friend Sub New(sVersion As String, [error] As Exception, [cancelled] As Boolean, [userState] As Object)
      MyBase.New([error], [cancelled], [userState])
      Version = sVersion
    End Sub
    Friend Sub New(sVersion As String, e As System.ComponentModel.AsyncCompletedEventArgs)
      MyBase.New(e.Error, e.Cancelled, e.UserState)
      Version = sVersion
    End Sub
  End Class
  Public Event CheckingVersion(sender As Object, e As EventArgs)
  Public Event CheckProgressChanged(sender As Object, e As ProgressEventArgs)
  Public Event CheckResult(sender As Object, e As CheckEventArgs)
  Public Event DownloadingUpdate(sender As Object, e As EventArgs)
  Public Event UpdateProgressChanged(sender As Object, e As ProgressEventArgs)
  Public Event DownloadResult(sender As Object, e As DownloadEventArgs)
  Private WithEvents wsVer As New CookieAwareWebClient()
  Private DownloadURL As String
  Private VerNumber As String
#Region "IDisposable Support"
  Private disposedValue As Boolean
  Protected Overridable Sub Dispose(disposing As Boolean)
    If Not Me.disposedValue Then
      If disposing Then
        If wsVer IsNot Nothing Then
          If wsVer.IsBusy Then
            wsVer.CancelAsync()
          End If
          wsVer.Dispose()
          wsVer = Nothing
        End If
      End If
    End If
    Me.disposedValue = True
  End Sub
  Public Sub Dispose() Implements IDisposable.Dispose
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub
#End Region
  Public Sub CheckVersion()
    Dim myS As New AppSettings
    wsVer.Proxy = myS.Proxy
    wsVer.Timeout = myS.Timeout
    myS = Nothing
    wsVer.CachePolicy = New Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore)
    wsVer.DownloadStringAsync(New Uri(VersionURL), "INFO")
    RaiseEvent CheckingVersion(Me, New EventArgs)
  End Sub
  Public Shared Function QuickCheckVersion() As CheckEventArgs.ResultType
    Dim sVerStr As String
    Dim mySettings As New AppSettings
    Using wsCheck As New CookieAwareWebClient()
      wsCheck.Proxy = mySettings.Proxy
      wsCheck.Timeout = mySettings.Timeout
      wsCheck.CachePolicy = New Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore)
      Try
        sVerStr = wsCheck.DownloadString(New Uri(VersionURL))
      Catch ex As Exception
        mySettings = Nothing
        Return CheckEventArgs.ResultType.NoUpdate
      End Try
    End Using
    Dim sSplit As String = Nothing
    If sVerStr.Contains(vbNewLine) Then
      sSplit = vbNewLine
    ElseIf sVerStr.Contains(vbLf) Then
      sSplit = vbLf
    ElseIf sVerStr.Contains(vbCr) Then
      sSplit = vbCr
    End If
    If String.IsNullOrEmpty(sSplit) Then
      Dim sVU() As String = sVerStr.Split("|"c)
      If CompareVersions(sVU(0)) Then
        mySettings = Nothing
        Return CheckEventArgs.ResultType.NewUpdate
      End If
    Else
      Dim sVL() As String = Split(sVerStr, sSplit, 2)
      If Not String.IsNullOrEmpty(sVL(0)) Then
        Dim sVMU() As String = sVL(0).Split("|"c)
        If CompareVersions(sVMU(0)) Then
          mySettings = Nothing
          Return CheckEventArgs.ResultType.NewUpdate
        ElseIf mySettings.UpdateBETA And Not String.IsNullOrEmpty(sVL(1)) Then
          Dim sVBU() As String = sVL(1).Split("|"c)
          If CompareVersions(sVBU(0)) Then
            mySettings = Nothing
            Return CheckEventArgs.ResultType.NewBeta
          End If
        End If
      End If
    End If
    mySettings = Nothing
    Return CheckEventArgs.ResultType.NoUpdate
  End Function
  Public Sub DownloadUpdate(toLocation As String)
    If Not String.IsNullOrEmpty(DownloadURL) Then
      wsVer.CachePolicy = New Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore)
      wsVer.DownloadFileAsync(New Uri(DownloadURL), toLocation, "FILE")
      RaiseEvent DownloadingUpdate(Me, New EventArgs)
    Else
      RaiseEvent DownloadResult(Me, New System.ComponentModel.AsyncCompletedEventArgs(New Exception("Version Check was not run."), True, Nothing))
    End If
  End Sub
  Private Sub wsVer_DownloadProgressChanged(sender As Object, e As System.Net.DownloadProgressChangedEventArgs) Handles wsVer.DownloadProgressChanged
    If e.UserState = "INFO" Then
      RaiseEvent CheckProgressChanged(sender, New ProgressEventArgs(e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage))
    ElseIf e.UserState = "FILE" Then
      RaiseEvent UpdateProgressChanged(sender, New ProgressEventArgs(e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage))
    End If
  End Sub
  Private Sub wsVer_DownloadStringCompleted(sender As Object, e As System.Net.DownloadStringCompletedEventArgs) Handles wsVer.DownloadStringCompleted
    Dim rRet As CheckEventArgs.ResultType = CheckEventArgs.ResultType.NoUpdate
    DownloadURL = Nothing
    VerNumber = Nothing
    If e.Error Is Nothing Then
      Try
        Dim sVerStr As String = e.Result
        Dim sSplit As String = Nothing
        If sVerStr.Contains(vbNewLine) Then
          sSplit = vbNewLine
        ElseIf sVerStr.Contains(vbLf) Then
          sSplit = vbLf
        ElseIf sVerStr.Contains(vbCr) Then
          sSplit = vbCr
        End If
        If String.IsNullOrEmpty(sSplit) Then
          Dim sVU() As String = sVerStr.Split("|"c)
          If CompareVersions(sVU(0)) Then
            rRet = CheckEventArgs.ResultType.NewUpdate
            DownloadURL = sVU(1)
            VerNumber = sVU(0)
          End If
        Else
          Dim sVL() As String = Split(sVerStr, sSplit, 2)
          If Not String.IsNullOrEmpty(sVL(0)) Then
            Dim sVMU() As String = sVL(0).Split("|"c)
            Dim mySettings As New AppSettings
            If CompareVersions(sVMU(0)) Then
              rRet = CheckEventArgs.ResultType.NewUpdate
              DownloadURL = sVMU(1)
              VerNumber = sVMU(0)
            ElseIf mySettings.UpdateBETA And Not String.IsNullOrEmpty(sVL(1)) Then
              Dim sVBU() As String = sVL(1).Split("|"c)
              If CompareVersions(sVBU(0)) Then
                rRet = CheckEventArgs.ResultType.NewBeta
                DownloadURL = sVBU(1)
                VerNumber = sVBU(0)
              End If
            End If
            mySettings = Nothing
          Else
            RaiseEvent CheckResult(sender, New CheckEventArgs(CheckEventArgs.ResultType.NoUpdate, VerNumber, New Exception("Version Reading Error", New Exception("Empty Version String")), e.Cancelled, e.UserState))
            Exit Sub
          End If
        End If
      Catch ex As Exception
        RaiseEvent CheckResult(sender, New CheckEventArgs(CheckEventArgs.ResultType.NoUpdate, VerNumber, New Exception("Version Parsing Error", ex), e.Cancelled, e.UserState))
        Exit Sub
      End Try
    End If
    RaiseEvent CheckResult(sender, New CheckEventArgs(rRet, VerNumber, e))
  End Sub
  Private Sub wsVer_DownloadFileCompleted(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs) Handles wsVer.DownloadFileCompleted
    RaiseEvent DownloadResult(sender, New DownloadEventArgs(VerNumber, e))
  End Sub
End Class
