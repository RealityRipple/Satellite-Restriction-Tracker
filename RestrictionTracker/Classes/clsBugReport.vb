Friend Class GitHubReporter
  Public Const ProjectID As String = "Satellite-Restriction-Tracker"
  Private Const Token As String = ""
  Private Shared Function JSONEscape(sInput As String) As String
    If sInput.Contains("\") Then sInput = sInput.Replace("\", "\\")
    If sInput.Contains("""") Then sInput = sInput.Replace("""", "\""")
    If sInput.Contains("/") Then sInput = sInput.Replace("/", "\/")
    If sInput.Contains(vbLf) Then sInput = sInput.Replace(vbLf, "\n")
    If sInput.Contains(vbCr) Then sInput = sInput.Replace(vbCr, "\r")
    For I As Integer = 1 To 31
      If sInput.Contains(ChrW(I)) Then sInput = sInput.Replace(ChrW(I), "\u" & srlFunctions.PadHex(I, 4))
    Next
    For I As Integer = 127 To 65535
      If sInput.Contains(ChrW(I)) Then sInput = sInput.Replace(ChrW(I), "\u" & srlFunctions.PadHex(I, 4))
    Next
    Return sInput
  End Function
  Friend Shared Function MakeIssueTitle(e As Exception) As String
    Dim sSum As String = e.Message
    If sSum.Contains(vbNewLine) Then
      sSum = sSum.Substring(0, sSum.IndexOf(vbNewLine))
    ElseIf sSum.Contains(vbCr) Then
      sSum = sSum.Substring(0, sSum.IndexOf(vbCr))
    ElseIf sSum.Contains(vbLf) Then
      sSum = sSum.Substring(0, sSum.IndexOf(vbLf))
    End If
    If sSum.Length > 80 Then sSum = sSum.Substring(0, 77) & "..."
    Return sSum
  End Function
  Friend Shared Function MakeIssueBody(e As Exception) As String
    Dim sPlat As String = IIf(Environment.Is64BitProcess, "x64", IIf(Environment.Is64BitOperatingSystem, "x86-64", "x86"))
    Dim sVer As String = Application.ProductVersion
    Dim iParts As Integer = (sVer.Split("."c)).Length
    If iParts > 3 Then sVer = sVer.Substring(0, sVer.LastIndexOf("."c))
    Dim sRet As String = "Error in " & My.Application.Info.ProductName & " v" & sVer & ":" & vbNewLine
    sRet &= "```" & vbNewLine
    sRet &= e.ToString & vbNewLine
    sRet &= "```" & vbNewLine & vbNewLine
    sRet &= "OS: " & My.Computer.Info.OSFullName & " (" & sPlat & ") v" & My.Computer.Info.OSVersion & vbNewLine
    sRet &= "CLR: " & srlFunctions.GetCLRCleanVersion
    Return sRet
  End Function
  Private Shared Function ReportBug(Title, Body) As String
    Dim sSend As String = "{"
    sSend &= """title"": """ & JSONEscape(Title) & ""","
    sSend &= """body"": """ & JSONEscape(Body) & """"
    sSend &= "}"
    Dim httpReport As New WebClientEx
    httpReport.KeepAlive = False
    httpReport.ErrorBypass = True
    httpReport.SendHeaders = New Net.WebHeaderCollection
    httpReport.SendHeaders.Add("Accept", "application/vnd.github+json")
    httpReport.SendHeaders.Add("Authorization", "token " & Token)
    Dim sRet As String = httpReport.UploadString("https://api.github.com/repos/RealityRipple/" & ProjectID & "/issues", "POST", sSend)
    If httpReport.ResponseCode = Net.HttpStatusCode.Created Then
      Try
        Dim jRet As New JSONReader(New IO.MemoryStream(httpReport.Encoding.GetBytes(sRet), False), False)
        For Each jNode In jRet.Serial
          If Not jNode.Type = JSONReader.ElementType.Group Then Continue For
          For Each jEl In jNode.SubElements
            If Not jEl.Type = JSONReader.ElementType.KeyValue Then Continue For
            If jEl.Key = "html_url" Then Return jEl.Value
          Next
        Next
      Catch ex As Exception
      End Try
      Return "https://github.com/RealityRipple/" & ProjectID & "/issues"
    End If
    Select Case httpReport.ResponseCode
      Case Net.HttpStatusCode.Forbidden : Return "HTTP Error 403: Forbidden" & vbNewLine & "You are forbidden from reporting this error."
      Case Net.HttpStatusCode.NotFound : Return "HTTP Error 404: Not Found" & vbNewLine & "The " & ProjectID & " GitHub repository is not available."
      Case Net.HttpStatusCode.Gone : Return "HTTP Error 410: Gone" & vbNewLine & "The " & ProjectID & " GitHub repository does not have issues enabled at this time."
      Case 422 : Return "HTTP Error 422: Validation Failed" & vbNewLine & "The error you attempted to report was invalid."
      Case Net.HttpStatusCode.ServiceUnavailable : Return "HTTP Error 503: Service Unavailable" & vbNewLine & "The GitHub service is not available at this time."
    End Select
    Dim msg As String = "Unknown Error"
    Try
      Dim jRet As New JSONReader(New IO.MemoryStream(httpReport.Encoding.GetBytes(sRet), False), False)
      For Each jNode In jRet.Serial
        If Not jNode.Type = JSONReader.ElementType.Group Then Continue For
        For Each jEl In jNode.SubElements
          If Not jEl.Type = JSONReader.ElementType.KeyValue Then Continue For
          If jEl.Key = "message" Then
            msg = jEl.Value
            Exit For
          End If
        Next
        If Not msg = "Unknown Error" Then Exit For
      Next
    Catch ex As Exception
      msg = "Unknown Error (Invalid JSON)"
    End Try
    If [Enum].IsDefined(httpReport.ResponseCode.GetType, httpReport.ResponseCode) Then Return "HTTP Error " & CInt(httpReport.ResponseCode) & ": " & [Enum].GetName(httpReport.ResponseCode.GetType, httpReport.ResponseCode) & vbNewLine & msg
    Return "HTTP Error " & CInt(httpReport.ResponseCode) & vbNewLine & msg
  End Function
  Friend Shared Function ReportIssue(e As Exception) As String
    If Token = "" Then Return "Unable to Report: GitHub Account Token Not Provided"
    Dim sSum As String = MakeIssueTitle(e)
    Dim sBod As String = MakeIssueBody(e)
    Return ReportBug(sSum, sBod)
  End Function
End Class
