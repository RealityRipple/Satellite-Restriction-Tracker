Imports System.Security.Cryptography
Public MustInherit Class RR_HMAC
  Inherits KeyedHashAlgorithm
  Private _disposed As Boolean
  Private _hashName As String
  Private _algo As HashAlgorithm
  Private _block As BlockProcessor
  Private _blockSizeValue As Integer
  Protected Sub New()
    _disposed = False
    _blockSizeValue = 64
  End Sub
  Protected Property BlockSizeValue() As Integer
    Get
      Return _blockSizeValue
    End Get
    Set(value As Integer)
      _blockSizeValue = value
    End Set
  End Property
  Public Property HashName() As String
    Get
      Return _hashName
    End Get
    Set(value As String)
      _hashName = value
      _algo = HashAlgorithm.Create(_hashName)
    End Set
  End Property
  Public Overrides Property Key() As Byte()
    Get
      Return DirectCast(MyBase.Key.Clone(), Byte())
    End Get
    Set(value As Byte())
      If (value IsNot Nothing) AndAlso (value.Length > BlockSizeValue) Then
        MyBase.Key = _algo.ComputeHash(value)
      Else
        MyBase.Key = DirectCast(value.Clone(), Byte())
      End If
    End Set
  End Property
  Friend ReadOnly Property Block() As BlockProcessor
    Get
      If _block Is Nothing Then
        _block = New BlockProcessor(_algo, (BlockSizeValue >> 3))
      End If
      Return _block
    End Get
  End Property
  Private Function KeySetup(key As Byte(), padding As Byte) As Byte()
    Dim buf As Byte() = New Byte(BlockSizeValue - 1) {}
    For i As Integer = 0 To key.Length - 1
      buf(i) = CByte(CByte(key(i)) Xor padding)
    Next
    For i As Integer = key.Length To BlockSizeValue - 1
      buf(i) = padding
    Next
    Return buf
  End Function
  Protected Overrides Sub Dispose(disposing As Boolean)
    If Not _disposed Then
      MyBase.Dispose(disposing)
    End If
  End Sub
  Protected Overrides Sub HashCore(rgb As Byte(), ib As Integer, cb As Integer)
    If _disposed Then
      Throw New ObjectDisposedException("HMACSHA1")
    End If

    If State = 0 Then
      Initialize()
      State = 1
    End If
    Block.Core(rgb, ib, cb)
  End Sub
  Protected Overrides Function HashFinal() As Byte()
    If _disposed Then
      Throw New ObjectDisposedException("HMAC")
    End If
    State = 0
    Block.Final()
    Dim intermediate As Byte() = _algo.Hash
    Dim buf As Byte() = KeySetup(Key, &H5C)
    _algo.Initialize()
    _algo.TransformBlock(buf, 0, buf.Length, buf, 0)
    _algo.TransformFinalBlock(intermediate, 0, intermediate.Length)
    Dim hash As Byte() = _algo.Hash
    _algo.Initialize()
    Array.Clear(buf, 0, buf.Length)
    Array.Clear(intermediate, 0, intermediate.Length)
    Return hash
  End Function
  Public Overrides Sub Initialize()
    If _disposed Then
      Throw New ObjectDisposedException("HMAC")
    End If
    State = 0
    Block.Initialize()
    Dim buf As Byte() = KeySetup(Key, &H36)
    _algo.Initialize()
    Block.Core(buf)
    Array.Clear(buf, 0, buf.Length)
  End Sub
  Public Shared Shadows Function Create() As HMAC
#If FULL_AOT_RUNTIME Then
			Return New System.Security.Cryptography.HMACSHA1()
#Else
    Return Create("System.Security.Cryptography.HMAC")
#End If
  End Function
  Public Shared Shadows Function Create(algorithmName As String) As HMAC
    Return DirectCast(CryptoConfig.CreateFromName(algorithmName), HMAC)
  End Function
End Class
Public Class RR_HMACSHA512
  Inherits RR_HMAC
  Shared legacy_mode As Boolean
  Private legacy As Boolean
  Shared Sub New()
    legacy_mode = False
  End Sub
  Public Sub New()
    Me.New(KeyBuilder.Key(8))
    ProduceLegacyHmacValues = legacy_mode
  End Sub
  Public Sub New(key__1 As Byte())
    ProduceLegacyHmacValues = legacy_mode
    HashName = "SHA512"
    HashSizeValue = 512
    Key = key__1
  End Sub
  Public Property ProduceLegacyHmacValues() As Boolean
    Get
      Return legacy
    End Get
    Set(value As Boolean)
      legacy = value
      BlockSizeValue = If(legacy, 64, 128)
    End Set
  End Property
End Class
Public Class KeyBuilder
  Private Shared rng As RandomNumberGenerator
  Shared Sub New()
    rng = RandomNumberGenerator.Create()
  End Sub
  Public Shared Function Key(size As Integer) As Byte()
    Dim key__1 As Byte() = New Byte(size - 1) {}
    rng.GetBytes(key__1)
    Return key__1
  End Function
  Public Shared Function IV(size As Integer) As Byte()
    Dim iv__1 As Byte() = New Byte(size - 1) {}
    rng.GetBytes(iv__1)
    Return iv__1
  End Function
End Class
Public Class BlockProcessor
  Private transform As ICryptoTransform
  Private block As Byte()
  Private blockSize As Integer
  Private blockCount As Integer
  Public Sub New(transform As ICryptoTransform)
    Me.New(transform, transform.InputBlockSize)
  End Sub
  Public Sub New(transform As ICryptoTransform, blockSize As Integer)
    Me.transform = transform
    Me.blockSize = blockSize
    block = New Byte(blockSize - 1) {}
  End Sub
  Protected Overrides Sub Finalize()
    Try
      Array.Clear(block, 0, blockSize)
    Finally
      MyBase.Finalize()
    End Try
  End Sub
  Public Sub Initialize()
    Array.Clear(block, 0, blockSize)
    blockCount = 0
  End Sub
  Public Sub Core(rgb As Byte())
    Core(rgb, 0, rgb.Length)
  End Sub
  Public Sub Core(rgb As Byte(), ib As Integer, cb As Integer)
    Dim n As Integer = System.Math.Min(blockSize - blockCount, cb)
    Array.Copy(rgb, ib, block, blockCount, n)
    blockCount += n
    If blockCount = blockSize Then
      transform.TransformBlock(block, 0, blockSize, block, 0)
      Dim b As Integer = CInt((cb - n) \ blockSize)
      For i As Integer = 0 To b - 1
        transform.TransformBlock(rgb, n, blockSize, block, 0)
        n += blockSize
      Next
      blockCount = cb - n
      If blockCount > 0 Then
        Array.Copy(rgb, n, block, 0, blockCount)
      End If
    End If
  End Sub
  Public Function Final() As Byte()
    Return transform.TransformFinalBlock(block, 0, blockCount)
  End Function
End Class
