Friend Class MantisReporter
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
  Private Shared cJar As Net.CookieContainer
  Private Shared Function GetToken(Project_ID As Integer) As String
    Dim pD1 As New Collections.Specialized.NameValueCollection
    pD1.Add("ref", "bug_report_page.php")
    pD1.Add("project_id", Project_ID.ToString.Trim)
    pD1.Add("make_default", Nothing)
    Dim httpToken As New WebClientEx
    cJar = New Net.CookieContainer
    httpToken.KeepAlive = False
    httpToken.CookieJar = cJar
    httpToken.SendHeaders = New Net.WebHeaderCollection
    httpToken.SendHeaders.Add(Net.HttpRequestHeader.Referer, "http://bugs.realityripple.com/login_select_proj_page.php?bug_report_page.php")
    Dim sTok As String = httpToken.UploadValues("http://bugs.realityripple.com/set_project.php", "POST", pD1)
    If sTok.StartsWith("Error: ") Then Return Nothing
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
    Dim sRet As String
    Dim httpReport As New WebClientEx
    httpReport.KeepAlive = False
    httpReport.CookieJar = cJar
    sRet = httpReport.UploadValues("http://bugs.realityripple.com/bug_report.php", "POST", pData)
    If sRet.StartsWith("Error: ") Then Return "Failed to Send Report - " & sRet.Substring(7)
    If sRet.Contains("Operation successful.") Then
      Return "OK"
    Else
      sRet = sRet.Substring(sRet.IndexOf("width50") - 14)
      sRet = sRet.Substring(0, sRet.IndexOf("</table>") + 8)
      Return sRet
    End If
  End Function
  Friend Shared Function ReportIssue(e As Exception) As String
    Dim sTok As String = GetToken(2)
    If String.IsNullOrEmpty(sTok) Then Return "No token was supplied by the server."
    Dim sPlat As String = IIf(Environment.Is64BitProcess, "x64", IIf(Environment.Is64BitOperatingSystem, "x86-64", "x86"))
    Dim sSum As String = "[v" & Application.ProductVersion & "] " & e.Message
    If sSum.Length > 80 Then sSum = sSum.Substring(0, 77) & "..."
    Return ReportBug(sTok, 2, Mantis_Category.General, Mantis_Reproducibility.Have_Not_Tried, Mantis_Severity.Minor, Mantis_Priority.Normal, sPlat, My.Computer.Info.OSFullName, My.Computer.Info.OSVersion, sSum, e.ToString, String.Empty, String.Empty, True)
  End Function
End Class
