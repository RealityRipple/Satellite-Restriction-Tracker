﻿Public Class clsUpdate
  Implements IDisposable
  Private Const VersionURL As String = "http://update.realityripple.com/Satellite_Restriction_Tracker/ver"
  Class ProgressEventArgs
    Inherits EventArgs
    Private mBytesReceived As Long
    Public ReadOnly Property BytesReceived As Long
      Get
        Return mBytesReceived
      End Get
    End Property
    Private mProgressPercentage As Integer
    Public ReadOnly Property ProgressPercentage As Integer
      Get
        Return mProgressPercentage
      End Get
    End Property
    Private mTotalBytesToReceive As Long
    Public ReadOnly Property TotalBytesToReceive As Long
      Get
        Return mTotalBytesToReceive
      End Get
    End Property
    Friend Sub New(bReceived As Long, bToReceive As Long, iPercentage As Integer)
      mBytesReceived = bReceived
      mTotalBytesToReceive = bToReceive
      mProgressPercentage = iPercentage
    End Sub
  End Class
  Class CheckEventArgs
    Inherits System.ComponentModel.AsyncCompletedEventArgs
    Enum ResultType
      NoUpdate
      NewUpdate
      NewBeta
    End Enum
    Private mResult As ResultType
    Public ReadOnly Property Result As ResultType
      Get
        Return mResult
      End Get
    End Property
    Private mVersion As String
    Public ReadOnly Property Version As String
      Get
        Return mVersion
      End Get
    End Property
    Friend Sub New(rtResult As ResultType, sVersion As String, ex As Exception, bCancelled As Boolean)
      MyBase.New(ex, bCancelled, Nothing)
      mVersion = sVersion
      mResult = rtResult
    End Sub
    Friend Sub New(rtResult As ResultType, sVersion As String, e As System.ComponentModel.AsyncCompletedEventArgs)
      MyBase.New(e.Error, e.Cancelled, e.UserState)
      mVersion = sVersion
      mResult = rtResult
    End Sub
  End Class
  Public Event CheckingVersion As EventHandler
  Public Event CheckProgressChanged As EventHandler(Of ProgressEventArgs)
  Public Event CheckResult As EventHandler(Of CheckEventArgs)
  Public Event DownloadingUpdate As EventHandler
  Public Event UpdateProgressChanged As EventHandler(Of ProgressEventArgs)
  Public Event DownloadResult As EventHandler(Of System.ComponentModel.AsyncCompletedEventArgs)
  Private WithEvents wsVer As WebClientCore
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
    wsVer = New WebClientCore(True)
    wsVer.KeepAlive = False
    wsVer.Proxy = myS.Proxy
    wsVer.Timeout = myS.Timeout
    myS = Nothing
    wsVer.CachePolicy = New Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore)
    RaiseEvent CheckingVersion(Me, New EventArgs)
    Dim tSocket As New Threading.Thread(New Threading.ThreadStart(AddressOf BeginCheck))
    tSocket.Start()
  End Sub
  Private Sub BeginCheck()
    wsVer.DownloadStringAsync(New Uri(VersionURL), "INFO")
  End Sub
  Public Shared Function QuickCheckVersion() As CheckEventArgs.ResultType
    Dim sVerStr As String
    Dim mySettings As New AppSettings
    Dim wsCheck As New WebClientEx
    wsCheck.KeepAlive = False
    wsCheck.Proxy = mySettings.Proxy
    wsCheck.Timeout = mySettings.Timeout
    sVerStr = wsCheck.DownloadString(VersionURL)
    If sVerStr.StartsWith("Error: ") Then
      mySettings = Nothing
      Return CheckEventArgs.ResultType.NoUpdate
    End If
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
            RaiseEvent CheckResult(sender, New CheckEventArgs(CheckEventArgs.ResultType.NoUpdate, VerNumber, New Exception("Version Reading Error", New Exception("Empty Version String")), e.Cancelled))
            Return
          End If
        End If
      Catch ex As Exception
        RaiseEvent CheckResult(sender, New CheckEventArgs(CheckEventArgs.ResultType.NoUpdate, VerNumber, New Exception("Version Parsing Error", ex), e.Cancelled))
        Return
      End Try
    End If
    RaiseEvent CheckResult(sender, New CheckEventArgs(rRet, VerNumber, e))
  End Sub
  Private Sub wsVer_DownloadFileCompleted(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs) Handles wsVer.DownloadFileCompleted
    RaiseEvent DownloadResult(sender, e)
  End Sub
  Private Sub wsVer_Failure(sender As Object, e As RestrictionLibrary.WebClientCore.ErrorEventArgs) Handles wsVer.Failure
    RaiseEvent CheckResult(sender, New CheckEventArgs(CheckEventArgs.ResultType.NoUpdate, Nothing, e.Error, False))
  End Sub
End Class
