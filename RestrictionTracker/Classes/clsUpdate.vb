﻿Friend Class clsUpdate
  Implements IDisposable
  Private Const VersionURL As String = "http://update.realityripple.com/Satellite_Restriction_Tracker/update.ver?sha=512"
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
  Class DownloadEventArgs
    Inherits System.ComponentModel.AsyncCompletedEventArgs
    Public Version As String
    Friend Sub New(ByVal sVersion As String, ByVal [error] As Exception, ByVal [cancelled] As Boolean, ByVal [userState] As Object)
      MyBase.New([error], [cancelled], [userState])
      Version = sVersion
    End Sub
    Friend Sub New(ByVal sVersion As String, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
      MyBase.New(e.Error, e.Cancelled, e.UserState)
      Version = sVersion
    End Sub
  End Class
  Public Event CheckingVersion As EventHandler
  Public Event CheckProgressChanged As EventHandler(Of ProgressEventArgs)
  Public Event CheckResult As EventHandler(Of CheckEventArgs)
  Public Event DownloadingUpdate As EventHandler
  Public Event UpdateProgressChanged As EventHandler(Of ProgressEventArgs)
  Public Event DownloadResult As EventHandler(Of DownloadEventArgs)
  Private WithEvents wsVer As WebClientCore
  Private DownloadURL As String
  Private DownloadHash As String
  Private DownloadLoc As String
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
    wsVer.Headers.Add("X-Thumb", Authenticode.RRSignThumb)
    wsVer.Headers.Add("X-Serial", Authenticode.RRSignSerial)
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
    wsCheck.SendHeaders.Add("X-Thumb", Authenticode.RRSignThumb)
    wsCheck.SendHeaders.Add("X-Serial", Authenticode.RRSignSerial)
    sVerStr = wsCheck.DownloadString(VersionURL)
    Dim sHash As String = Nothing
    For Each sKey As String In wsCheck.ResponseHeaders
      If sKey.ToLower = "x-update-signature" Then
        sHash = wsCheck.ResponseHeaders(sKey)
        Exit For
      End If
    Next
    If Not VerifySignature(sVerStr, sHash) Then
      mySettings = Nothing
      Return CheckEventArgs.ResultType.NoUpdate
    End If
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
      DownloadLoc = toLocation
      wsVer.CachePolicy = New Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore)
      wsVer.DownloadFileAsync(New Uri(DownloadURL), toLocation, "FILE")
      RaiseEvent DownloadingUpdate(Me, New EventArgs)
    Else
      DownloadLoc = Nothing
      RaiseEvent DownloadResult(Me, New DownloadEventArgs(Nothing, New Exception("Version Check was not run."), True, Nothing))
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
    downloadhash = Nothing
    VerNumber = Nothing
    If e.Error Is Nothing Then
      Try
        Dim sVerStr As String = e.Result
        Dim sHash As String = Nothing
        For Each sKey As String In wsVer.ResponseHeaders
          If sKey.ToLower = "x-update-signature" Then
            sHash = wsVer.ResponseHeaders(sKey)
            Exit For
          End If
        Next
        If Not VerifySignature(sVerStr, sHash) Then
          RaiseEvent CheckResult(sender, New CheckEventArgs(CheckEventArgs.ResultType.NoUpdate, VerNumber, New Exception("Invalid Server Response"), e.Cancelled))
          Return
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
              DownloadHash = sVMU(2).ToUpper
            ElseIf mySettings.UpdateBETA And Not String.IsNullOrEmpty(sVL(1)) Then
              Dim sVBU() As String = sVL(1).Split("|"c)
              If CompareVersions(sVBU(0)) Then
                rRet = CheckEventArgs.ResultType.NewBeta
                DownloadURL = sVBU(1)
                VerNumber = sVBU(0)
                DownloadHash = sVBU(2).ToUpper
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
  Private Sub wsVer_DownloadFileCompleted(sender As Object, e As clsUpdate.DownloadEventArgs) Handles wsVer.DownloadFileCompleted
    If Not IO.File.Exists(DownloadLoc) Then
      RaiseEvent DownloadResult(sender, New DownloadEventArgs(VerNumber, New Exception("Download Failure"), e.Cancelled, e.UserState))
      Return
    End If
    Dim bData As Byte() = IO.File.ReadAllBytes(DownloadLoc)
    Dim sha512 As New Security.Cryptography.SHA512CryptoServiceProvider
    Dim bHash As Byte() = sha512.ComputeHash(bData)
    Dim sHash As String = BitConverter.ToString(bHash).Replace("-", "").ToUpper
    If sHash = DownloadHash Then
      RaiseEvent DownloadResult(sender, New DownloadEventArgs(VerNumber, e))
    Else
      RaiseEvent DownloadResult(sender, New DownloadEventArgs(VerNumber, New Exception("Download Failure"), e.Cancelled, e.UserState))
    End If
  End Sub
  Private Sub wsVer_Failure(sender As Object, e As RestrictionLibrary.WebClientErrorEventArgs) Handles wsVer.Failure
    RaiseEvent CheckResult(sender, New CheckEventArgs(CheckEventArgs.ResultType.NoUpdate, Nothing, e.Error, False))
  End Sub
  Friend Shared Function VerifySignature(ByVal Message As String, ByVal Signature As String) As Boolean
    If String.IsNullOrEmpty(Signature) Then Return False
    Dim bMsg() As Byte = System.Text.Encoding.GetEncoding(srlFunctions.LATIN_1).GetBytes(Message)
    Dim bSig() As Byte = Nothing
    Try
      bSig = System.Convert.FromBase64String(Signature)
    Catch ex As Exception
      Return False
    End Try
    Dim rsa As New Security.Cryptography.RSACryptoServiceProvider
    rsa.FromXmlString(My.Resources.pubkey)
    Return rsa.VerifyData(bMsg, Security.Cryptography.CryptoConfig.MapNameToOID("SHA512"), bSig)
  End Function
End Class
