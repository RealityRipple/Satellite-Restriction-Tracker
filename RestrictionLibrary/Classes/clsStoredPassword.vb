Public Class StoredPassword
  Private Class RC4
    Private State As Byte()
    Private X, Y As Byte
    Public Sub New(key As Byte())
      State = New Byte(255) {}
      X = 0
      Y = 0
      InitKey(key)
    End Sub
    Public Function Transform(ByRef buffer As Byte(), ByRef start As Integer, count As Integer) As Integer
      Return RC4Transform(buffer, start, count, buffer, start)
    End Function
    Private Sub InitKey(key As Byte())
      Dim A As Byte = 0
      Dim B As Byte = 0
      For I As Integer = 0 To 255
        State(I) = CByte(I)
      Next
      X = 0
      Y = 0
      For counter As Integer = 0 To 255
        B = CByte((CLng(key(A)) + State(counter) + B) And &HFF)
        Dim tmp As Byte = State(counter)
        State(counter) = State(B)
        State(B) = tmp
        A = CByte((A + 1) Mod key.Length)
      Next
    End Sub
    Private Function RC4Transform(inBuffer As Byte(), inIndex As Integer, inCount As Integer, ByRef outBuffer As Byte(), ByRef outIndex As Integer) As Integer
      For I As Integer = 0 To inCount - 1
        X = CByte(X + 1)
        Y = CByte((CLng(State(X)) + Y) And &HFF)
        Dim tmp As Byte = State(X)
        State(X) = State(Y)
        State(Y) = tmp
        Dim xorer As Byte = CByte((CLng(State(X)) + State(Y)) And &HFF)
        outBuffer(outIndex + I) = CByte(inBuffer(inIndex + I) Xor State(xorer))
      Next
      Return inCount
    End Function
  End Class
  Private Shared crypter As RC4
  Private Shared Sub GenKey()
    crypter = Nothing
    Dim bKey() As Byte = {&H43, &HD6, &HE6, &HFD, &H8A, &H76, &H91, &H3C, &HA1, &H69, &H2C, &H8F, &HB7, &HC1, &HCD, &H1F}
    Dim bSeed() As Byte = {&H4E, &H4E, &HE9, &H24, &HD5, &H2D, &H45, &H2A, &HB7, &HF0, &HE5, &H21, &H22, &HC8, &H90, &H76} '{4E4EE924-D52D-452A-B7F0-E52122C89076}
    Dim sessionKeyHMAC As New Security.Cryptography.HMACSHA256(bKey)
    Dim cryptHASH = sessionKeyHMAC.ComputeHash(bSeed)
    crypter = New RC4(cryptHASH)
  End Sub
  Private Shared Sub GenLoggerKey()
    crypter = Nothing
    Dim bKey() As Byte = CType(System.Runtime.InteropServices.GuidAttribute.GetCustomAttribute(System.Reflection.Assembly.LoadFile(My.Application.Info.DirectoryPath & "\RestrictionLogger.exe"), GetType(System.Runtime.InteropServices.GuidAttribute)).TypeId.Guid, Guid).ToByteArray()
    Dim bSeed() As Byte = {&H4E, &H4E, &HE9, &H24, &HD5, &H2D, &H45, &H2A, &HB7, &HF0, &HE5, &H21, &H22, &HC8, &H90, &H76}
    Dim sessionKeyHMAC As New Security.Cryptography.HMACSHA256(bKey)
    Dim cryptHASH = sessionKeyHMAC.ComputeHash(bSeed)
    crypter = New RC4(cryptHASH)
  End Sub
  Shared Function DecryptApp(PassCrypt As String) As String
    GenKey()
    Dim bCrypt() As Byte = Convert.FromBase64String(PassCrypt)
    crypter.Transform(bCrypt, 0, bCrypt.Length)
    Return System.Text.Encoding.GetEncoding(28591).GetString(bCrypt)
  End Function
  Shared Function EncryptApp(Password As String) As String
    GenKey()
    Dim bPass() As Byte = System.Text.Encoding.GetEncoding(28591).GetBytes(Password)
    crypter.Transform(bPass, 0, bPass.Length)
    Return Convert.ToBase64String(bPass)
  End Function
  Shared Function DecryptLogger(PassCrypt As String) As String
    genLoggerKey()
    Dim bCrypt() As Byte = Convert.FromBase64String(PassCrypt)
    crypter.Transform(bCrypt, 0, bCrypt.Length)
    Return System.Text.Encoding.GetEncoding(28591).GetString(bCrypt)
  End Function
  Shared Function EncryptLogger(Password As String) As String
    GenLoggerKey()
    Dim bPass() As Byte = System.Text.Encoding.GetEncoding(28591).GetBytes(Password)
    crypter.Transform(bPass, 0, bPass.Length)
    Return Convert.ToBase64String(bPass)
  End Function
End Class