Public Class StoredPassword
  Private Shared Function AESCTR(bData() As Byte, bKey() As Byte) As Byte()
    Dim hAES As New Security.Cryptography.RijndaelManaged()
    hAES.Padding = Security.Cryptography.PaddingMode.None
    hAES.Mode = Security.Cryptography.CipherMode.ECB
    Dim kLargest As Integer = 0
    For Each hSize In hAES.LegalKeySizes
      If hSize.MaxSize > kLargest Then kLargest = hSize.MaxSize
    Next
    hAES.KeySize = kLargest
    Dim blockSize As Integer = Math.Ceiling(hAES.BlockSize / 8)
    Dim ctr(15) As Byte
    ctr(0) = 1
    Dim xorMask As New Queue(Of Byte)
    Dim zeroIV(blockSize - 1) As Byte
    Dim ctrEnc As Security.Cryptography.ICryptoTransform = hAES.CreateEncryptor(bKey, zeroIV)
    Dim bOut As New List(Of Byte)
    For p As UInt64 = 0 To bData.LongLength - 1
      Dim b As Byte = bData(p)
      If xorMask.Count = 0 Then
        Dim ctrModeBlock(blockSize - 1) As Byte
        ctrEnc.TransformBlock(ctr, 0, ctr.Length, ctrModeBlock, 0)
        For I As Integer = 0 To ctr.Length - 1
          If ctr(I) = &HFF Then
            ctr(I) = 0
          Else
            ctr(I) += 1
            Exit For
          End If
        Next
        For Each b2 As Byte In ctrModeBlock
          xorMask.Enqueue(b2)
        Next
      End If
      Dim mask As Byte = xorMask.Dequeue()
      bOut.Add((b Xor mask))
    Next
    Return bOut.ToArray()
  End Function
  ''' <summary>
  ''' Generate a key for encryption.
  ''' </summary>
  ''' <returns>The largest number of random bytes that Rijndael will accept for a key.</returns>
  Shared Function GenerateKey() As Byte()
    Dim hAES As New Security.Cryptography.RijndaelManaged()
    hAES.Padding = Security.Cryptography.PaddingMode.None
    hAES.Mode = Security.Cryptography.CipherMode.ECB
    Dim hLargest As Integer = 0
    For Each hSize In hAES.LegalKeySizes
      If hSize.MaxSize > hLargest Then hLargest = hSize.MaxSize
    Next
    Dim newKey(Math.Ceiling(hLargest / 8) - 1) As Byte
    Dim rng As Security.Cryptography.RandomNumberGenerator
    rng = Security.Cryptography.RandomNumberGenerator.Create()
    rng.GetBytes(newKey)
    Return newKey
  End Function
  ''' <summary>
  ''' Generate a salt for encryption.
  ''' </summary>
  ''' <returns>Twelve random bytes.</returns>
  Shared Function GenerateSalt() As Byte()
    Dim newSalt(11) As Byte
    Dim rng As Security.Cryptography.RandomNumberGenerator
    rng = Security.Cryptography.RandomNumberGenerator.Create()
    rng.GetBytes(newSalt)
    Return newSalt
  End Function
  ''' <summary>
  ''' Decrypt a password that was encrypted with Rijndael (AES-128).
  ''' </summary>
  ''' <param name="PassCrypt">The base64 encoded encrypted password.</param>
  ''' <param name="PassKey">The base64 encoded encryption key.</param>
  ''' <param name="PassSalt">The base64 encoded salt.</param>
  ''' <returns>The raw password which was encrypted.</returns>
  Shared Function Decrypt(PassCrypt As String, PassKey As String, PassSalt As String) As String
    Dim bKey() As Byte = Convert.FromBase64String(PassKey)
    Dim bSalt() As Byte = Convert.FromBase64String(PassSalt)
    Dim bIn() As Byte = Convert.FromBase64String(PassCrypt)
    Dim bOut() As Byte = AESCTR(bIn, bKey)
    For I As Integer = 0 To bSalt.Length - 1
      If Not bOut(I) = bSalt(I) Then Return Nothing
    Next
    Dim bPass(bOut.Length - bSalt.Length - 1) As Byte
    Array.ConstrainedCopy(bOut, bSalt.Length, bPass, 0, bOut.Length - bSalt.Length)
    Return System.Text.Encoding.GetEncoding(28591).GetString(bPass)
  End Function
  ''' <summary>
  ''' Encrypt a password using Rijndael (AES-128).
  ''' </summary>
  ''' <param name="Password">The password to encrypt.</param>
  ''' <param name="Key">Sixteen random bytes.</param>
  ''' <param name="Salt">Any number of random bytes.</param>
  ''' <returns>An base64 output of the encrypted password.</returns>
  Shared Function Encrypt(Password As String, Key As Byte(), Salt As Byte()) As String
    Dim bPass() As Byte = System.Text.Encoding.GetEncoding(28591).GetBytes(Password)
    Dim bIn(bPass.Length + Salt.Length - 1) As Byte
    Array.Copy(Salt, bIn, Salt.Length)
    Array.Copy(bPass, 0, bIn, Salt.Length, bPass.Length)
    Dim bOut() As Byte = AESCTR(bIn, Key)
    Return Convert.ToBase64String(bOut)
  End Function
End Class
Public Class StoredPasswordLegacy
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
    Dim bKey() As Byte = CType(System.Runtime.InteropServices.GuidAttribute.GetCustomAttribute(System.Reflection.Assembly.LoadFile(IO.Path.Combine(My.Application.Info.DirectoryPath, "RestrictionLogger.exe")), GetType(System.Runtime.InteropServices.GuidAttribute)).TypeId.Guid, Guid).ToByteArray()
    Dim bSeed() As Byte = {&H4E, &H4E, &HE9, &H24, &HD5, &H2D, &H45, &H2A, &HB7, &HF0, &HE5, &H21, &H22, &HC8, &H90, &H76}
    Dim sessionKeyHMAC As New Security.Cryptography.HMACSHA256(bKey)
    Dim cryptHASH = sessionKeyHMAC.ComputeHash(bSeed)
    crypter = New RC4(cryptHASH)
  End Sub
  ''' <summary>
  ''' Decrypt a password that was encrypted with the Tracker EXE GUID.
  ''' </summary>
  ''' <param name="PassCrypt">The base64 encoded encrypted password.</param>
  ''' <returns>The raw password which was encrypted.</returns>
  Shared Function DecryptApp(PassCrypt As String) As String
    GenKey()
    Dim bCrypt() As Byte = Convert.FromBase64String(PassCrypt)
    crypter.Transform(bCrypt, 0, bCrypt.Length)
    Return System.Text.Encoding.GetEncoding(28591).GetString(bCrypt)
  End Function
  ''' <summary>
  ''' Decrypt a password that was encrypted with the Logger EXE GUID.
  ''' </summary>
  ''' <param name="PassCrypt">The base64 encoded encrypted password.</param>
  ''' <returns>The raw password which was encrypted.</returns>
  Shared Function DecryptLogger(PassCrypt As String) As String
    GenLoggerKey()
    Dim bCrypt() As Byte = Convert.FromBase64String(PassCrypt)
    crypter.Transform(bCrypt, 0, bCrypt.Length)
    Return System.Text.Encoding.GetEncoding(28591).GetString(bCrypt)
  End Function
End Class
