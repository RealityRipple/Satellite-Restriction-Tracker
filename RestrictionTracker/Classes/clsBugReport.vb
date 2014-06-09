Friend Class MantisReporter
  Private Shared httpSend As CookieAwareWebClient
  Friend Enum Mantis_Category
    [Select] = 0
    General = 1
    [Interface] = 3
    Setup = 4
  End Enum
  Private Enum Mantis_Reproducibility
    Always = 10
    Sometimes = 30
    Random = 50
    Have_Not_Tried = 70
    Unable_to_Reproduce = 90
    NotApplicable = 100
  End Enum
  Private Enum Mantis_Severity
    Feature = 10
    Trivial = 20
    Text = 30
    Tweak = 40
    Minor = 50
    Major = 60
    Crash = 70
    Block = 80
  End Enum
  Private Enum Mantis_Priority
    None = 10
    Low = 20
    Normal = 30
    High = 40
    Urgent = 50
    Immediate = 60
  End Enum
  Private Shared Function GetToken(Project_ID As Integer) As String
    Dim pD1 As New Collections.Specialized.NameValueCollection
    pD1.Add("ref", "bug_report_page.php")
    pD1.Add("project_id", Project_ID.ToString.Trim)
    pD1.Add("make_default", Nothing)
    httpSend.Headers.Add(Net.HttpRequestHeader.Referer, "http://bugs.realityripple.com/login_select_proj_page.php?bug_report_page.php")
    Dim bTok() As Byte = httpSend.UploadValues("http://bugs.realityripple.com/set_project.php", "POST", pD1)
    Dim sTok As String = System.Text.Encoding.GetEncoding(28591).GetString(bTok)
    If sTok.Contains("bug_report_token") Then
      sTok = sTok.Substring(sTok.IndexOf("bug_report_token") + 25)
      sTok = sTok.Substring(0, sTok.IndexOf("""/"))
      Return sTok
    Else
      Return Nothing
    End If
  End Function
  Private Shared Function ReportBug(Token As String, Project_ID As Integer, Category As Mantis_Category, Reproducable As Mantis_Reproducibility, Severity As Mantis_Severity, Priority As Mantis_Priority, Platform As String, OS As String, OS_Build As String, Summary As String, Description As String, Steps As String, Info As String, [Public] As Boolean) As String
    Dim pData As New Collections.Specialized.NameValueCollection
    pData.Add("bug_report_token", Token)
    pData.Add("m_id", "0")
    pData.Add("project_id", Project_ID)
    pData.Add("category_id", Category)
    pData.Add("reproducibility", Reproducable)
    pData.Add("severity", Severity)
    pData.Add("priority", Priority)
    pData.Add("platform", Platform)
    pData.Add("os", OS)
    pData.Add("os_build", OS_Build)
    pData.Add("summary", Summary)
    pData.Add("description", Description)
    pData.Add("steps_to_reproduce", Steps)
    pData.Add("additional_info", Info)
    pData.Add("view_state", IIf([Public], "10", "50"))
    pData.Add("report_stay", Nothing)
    Dim bRet() As Byte
    Try
      bRet = httpSend.UploadValues("http://bugs.realityripple.com/bug_report.php", "POST", pData)
    Catch ex As Exception
      Return "Failed to Send Report - " & ex.Message
    Finally
      httpSend.Dispose()
      httpSend = Nothing
    End Try
    Dim sRet As String = System.Text.Encoding.GetEncoding(28591).GetString(bRet)
    If sRet.Contains("Operation successful.") Then
      Return "OK"
    Else
      sRet = sRet.Substring(sRet.IndexOf("width50") - 14)
      sRet = sRet.Substring(0, sRet.IndexOf("</table>") + 8)
      Return sRet
    End If
  End Function
  Friend Shared Function ReportIssue(e As Exception) As String
    If httpSend IsNot Nothing Then
      httpSend.Dispose()
      httpSend = Nothing
    End If
    httpSend = New CookieAwareWebClient
    Dim sTok As String = GetToken(1)
    If String.IsNullOrEmpty(sTok) Then
      httpSend.Dispose()
      httpSend = Nothing
      Return "No token was supplied by the server."
    End If
    Dim sPlat As String = IIf(Environment.Is64BitProcess, "x64", IIf(Environment.Is64BitOperatingSystem, "x86-64", "x86"))
    Dim sSum As String = e.Message
    If sSum.Length > 80 Then sSum = sSum.Substring(0, 77) & "..."
    Dim sDesc As String = e.Message
    If Not String.IsNullOrEmpty(e.StackTrace) Then
      sDesc &= vbNewLine & e.StackTrace
    Else
      If Not String.IsNullOrEmpty(e.Source) Then
        sDesc &= vbNewLine & " @ " & e.Source
        If e.TargetSite IsNot Nothing Then sDesc &= "." & e.TargetSite.Name
      Else
        If e.TargetSite IsNot Nothing Then sDesc &= vbNewLine & " @ " & e.TargetSite.Name
      End If
    End If
    sDesc &= vbNewLine & "Version " & Application.ProductVersion
    Dim sSteps As String = Nothing
    Dim sInfo As String = Nothing
    If e.InnerException IsNot Nothing Then
      sInfo = e.InnerException.Message
      If e.InnerException.TargetSite IsNot Nothing Then sInfo &= vbNewLine & "Trace - " & e.InnerException.TargetSite.Name
      If Not String.IsNullOrEmpty(e.InnerException.Source) Then sInfo &= " > " & e.InnerException.Source
      If Not String.IsNullOrEmpty(e.InnerException.StackTrace) Then sInfo &= vbNewLine & e.InnerException.StackTrace
    End If
    Return ReportBug(sTok, 2, Mantis_Category.General, Mantis_Reproducibility.Have_Not_Tried, Mantis_Severity.Minor, Mantis_Priority.Normal, sPlat, My.Computer.Info.OSFullName, My.Computer.Info.OSVersion, sSum, sDesc, sSteps, sInfo, True)
  End Function
End Class
