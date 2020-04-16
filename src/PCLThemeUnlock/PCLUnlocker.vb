Imports System.Text
Imports System.Security.Cryptography
Imports Microsoft.VisualBasic.CompilerServices
Public Class PCLUnlocker

    '识别码
    Private Shared UniqueAddress As String

    '定义主题设置常量
    Private Const ThemeCode As String = "0|1|2|3|4|5|6|7|8|9|10|11|12|13|14|42"

    '定义主题注册表常量
    Private Const ThemeRegKey As String = "Software\PCL\UiLauncherThemeHide"

    '字符串格式化
    Private Shared Function StrFill(ByVal Str As String, ByVal Code As String, ByVal Length As Byte) As String
        Dim result As String
        If Str.Length > CInt(Length) Then
            result = Strings.Mid(Str, 1, CInt(Length))
        Else
            result = Strings.Mid(Str.PadRight(CInt(Length), Conversions.ToChar(Code)), Str.Length + 1) + Str
        End If
        Return result
    End Function

    '加密
    Private Shared Function SecureAdd(ByVal SourceString As String, Optional ByVal Key As String = "") As String
        Key = SecureKey(Key)
        Dim bytes As Byte() = Encoding.UTF8.GetBytes(Key)
        Dim bytes2 As Byte() = Encoding.UTF8.GetBytes("95168702")
        Dim dESCryptoServiceProvider As DESCryptoServiceProvider = New DESCryptoServiceProvider()
        Dim result As String
        Using memoryStream As IO.MemoryStream = New IO.MemoryStream()
            Dim bytes3 As Byte() = Encoding.UTF8.GetBytes(SourceString)
            Using cryptoStream As CryptoStream = New CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(bytes, bytes2), CryptoStreamMode.Write)
                cryptoStream.Write(bytes3, 0, bytes3.Length)
                cryptoStream.FlushFinalBlock()
                result = Convert.ToBase64String(memoryStream.ToArray())
            End Using
        End Using
        Return result
    End Function

    '密钥处理
    Private Shared Function SecureKey(ByVal Key As String) As String
        Dim result As String
        If String.IsNullOrEmpty(Key) Then
            result = "@;$ Abv2"
        Else
            result = Strings.Mid(StrFill(Conversions.ToString(GetHash(Key)), "X", 8), 1, 8)
        End If
        Return result
    End Function

    '获取Hash值
    Private Shared Function GetHash(ByVal Str As String) As ULong
        Dim num As ULong = 5381UL
        Dim num2 As Integer = Str.Length - 1
        For i As Integer = 0 To num2
            num = (num << 5 Xor num Xor CULng(AscW(Str(i))))
        Next
        Return num Xor 12218072394304324399UL
    End Function

    '获取主板UUID
    Private Shared Function GetComputerUUID() As String
        Dim RStr As String = vbNullString
        Try
            Dim cmicWmi As New System.Management.ManagementObjectSearcher("select UUID from Win32_ComputerSystemProduct")
            RStr = cmicWmi.Get(0)("UUID")
            cmicWmi.Dispose()
            cmicWmi = Nothing
        Catch
            RStr = vbNullString
        End Try
        Return RStr
    End Function

    '获取注册表ID
    Private Shared Function GetIdentify() As String
        Dim RStr As String = vbNullString
        Try
            Dim RegKey As Microsoft.Win32.RegistryKey
            RegKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\PCL", False)
            RStr = RegKey.GetValue("Identify").ToString
            RegKey.Dispose()
            RegKey = Nothing
        Catch
            RStr = vbNullString
        End Try
        Return RStr
    End Function

    '获取识别码
    Private Shared Function GetUniqueAddress() As Boolean
        Dim ID As String = GetIdentify()
        Dim UUID As String = GetComputerUUID()
        If String.IsNullOrEmpty(ID) Then
            UniqueAddress = vbNullString
            Return False
        Else
            If String.IsNullOrEmpty(UUID) Then
                UniqueAddress = "0000-0000-0000-0000"
                Return True
            Else
                Try
                    Dim RStr As String = StrFill(GetHash(UUID & ID).ToString("X"), "7", 16)
                    UniqueAddress = String.Concat(New String() {Strings.Mid(RStr, 5, 4), "-", Strings.Mid(RStr, 13, 4), "-", Strings.Mid(RStr, 1, 4), "-", Strings.Mid(RStr, 9, 4)})
                    Return True
                Catch
                    UniqueAddress = vbNullString
                    Return False
                End Try
            End If
        End If
    End Function

    '写入注册表
    Private Shared Function SetRegCuruserValue(ByVal RegName As String, ByVal Value As Object) As Boolean
        Try
            Dim RegKey As Microsoft.Win32.RegistryKey
            RegKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(Strings.Left(RegName, Strings.InStrRev(RegName, "\") - 1), True)
            RegKey.SetValue(Strings.Mid(RegName, Strings.InStrRev(RegName, "\") + 1), Value)
            RegKey.Dispose()
            RegKey = Nothing
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    '解锁主题
    Public Shared Function UnlockTheme() As Boolean
        If Not GetUniqueAddress() Then
            Return False
            Exit Function
        End If
        Dim RStr As String
        Try
            RStr = SecureAdd(ThemeCode, "PCL" & UniqueAddress)
        Catch ex As Exception
            Return False
            Exit Function
        End Try
        If Not SetRegCuruserValue(ThemeRegKey, RStr) Then
            Return False
            Exit Function
        End If
        Return True
    End Function

    '生成解锁码
    Public Shared Function GetDonateCode(ByVal Mode As Integer) As String
        If Not GetUniqueAddress() Then
            Return vbNullString
            Exit Function
        End If
        Dim RStr As String = vbNullString
        If Mode = 0 Then
            '极客蓝
            RStr = "PuzzleUnlock" & Mid(UniqueAddress, UniqueAddress.Count - 4)
        Else
            Select Case Mode
                Case 1  '活跃橙
                    RStr = "FeedbackTheme9"
                Case 2  '秋仪金
                    RStr = "DonateTheme8"
            End Select
            Try
                RStr = GetHash(RStr)
                RStr = SecureAdd(RStr, """" + StrFill(UniqueAddress, "X", 25) + "PCL")
            Catch
                RStr = vbNullString
            End Try
        End If
        Return RStr
    End Function

End Class
