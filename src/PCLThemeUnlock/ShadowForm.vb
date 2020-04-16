Imports System.Runtime.InteropServices
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.Drawing

Public Class ShadowForm
    Inherits Form

    Public isRoundShadow As Boolean = True
    Public isShowShadow As Boolean = True
    Private WithEvents _MainForm As Form
    Private _ShadowWidth As Integer = 9
    Private _ShadowImage As Bitmap

    Private Shared Sub UnloadObject(ByVal MyObj As Object)
        MyObj.Dispose()
        MyObj = Nothing
    End Sub

    Public Property ShadowWidth As Integer
        Get
            Return _ShadowWidth
        End Get
        Set(ByVal value As Integer)
            Me._ShadowWidth = value
            ReSet()
        End Set
    End Property
    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim x As CreateParams = MyBase.CreateParams
            x.ExStyle = x.ExStyle Or &H80000
            Return x
            UnloadObject(x)
        End Get
    End Property

    Public Shared Function RegisterShadowForm(ByVal form As Form) As ShadowForm
        Return New ShadowForm(form)
    End Function
    Private Sub New(ByVal form As Form)

        ' 此调用是设计器所必需的。
        InitializeComponent()
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        _MainForm = form
        InitMe()
    End Sub

    Private Sub InitMe()
        _MainForm.Owner = Me
        Me.ShowInTaskbar = False
        Me.ShowIcon = True
        Me.Icon = _MainForm.Icon
    End Sub

    Public Sub SizeChange() Handles _MainForm.SizeChanged
        ReSet()
    End Sub
    Public Sub LocationChange() Handles _MainForm.LocationChanged
        Me.Location = New Point(_MainForm.Location.X - Me._ShadowWidth, _MainForm.Location.Y - Me._ShadowWidth)
    End Sub
    Private Sub ShowMe(ByVal sender As Object, ByVal e As EventArgs) Handles _MainForm.Shown
        Me.Show()
        ReSet()
    End Sub

    Private Sub ReSet()
        If Me.isShowShadow Then
            SetSizeLocation()
            SetShadowImage()
            setPaint()
        End If
    End Sub

    Private Sub SetSizeLocation()
        Me.Size = New Size(_MainForm.Size.Width + 2 * Me._ShadowWidth, _MainForm.Size.Height + 2 * Me._ShadowWidth)
        Me.Location = New Point(_MainForm.Location.X - Me._ShadowWidth, _MainForm.Location.Y - Me._ShadowWidth)
    End Sub
    Private Function SetShadowImage() As Bitmap
        If IsNothing(_ShadowImage) Then
            _ShadowImage = New Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height)
        End If
        Graphics.FromImage(_ShadowImage).Clear(Color.Transparent)
        If isRoundShadow Then
            _ShadowImage = SetRoundShadowStyle()
        Else
            _ShadowImage = SetShadowStyle()
        End If
        Return _ShadowImage
    End Function
    Private Function SetRoundShadowStyle()
        '_ShadowImage = New Bitmap(Me.Width, Me.Height)
        Dim g As Graphics = Graphics.FromImage(_ShadowImage)
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim pen As Pen = New Pen(Color.FromArgb(0), 2)
        For i As Integer = 0 To _ShadowWidth Step 1
            pen.Color = Color.FromArgb((255 / 10 / _ShadowWidth) * i, 0, 0, 0)
            g.DrawPath(pen, CreateRoundPath(New Rectangle(i, i, Me.Width - 2 * i - 1, Me.Height - 2 * i - 1)))
        Next
        Return _ShadowImage
    End Function
    Private Function CreateRoundPath(ByVal rect As Rectangle)
        Dim cornerRadius As Integer = ShadowWidth * 0.6
        Dim roundedRect As GraphicsPath = New GraphicsPath()
        roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90)
        roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y)
        roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90)
        roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2)
        roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90)
        roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom)
        roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90)
        roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2)
        roundedRect.CloseFigure()
        Return roundedRect
    End Function
    Protected Overridable Function SetShadowStyle()
        '_ShadowImage = New Bitmap(Me.Width, Me.Height)
        Dim g As Graphics = Graphics.FromImage(_ShadowImage)

        Dim pen As Pen = New Pen(Color.FromArgb(0), 2)
        Dim MyRect As Rectangle
        For i As Integer = 0 To _ShadowWidth Step 1
            pen.Color = Color.FromArgb((255 / 10 / _ShadowWidth) * i, 0, 0, 0)
            UnloadObject(MyRect)
            MyRect = New Rectangle(i, i, Me.Width - 2 * i - 1, Me.Height - 2 * i - 1)
            g.DrawRectangle(pen, MyRect)
        Next
        Return _ShadowImage
        UnloadObject(MyRect)
        UnloadObject(g)
        UnloadObject(pen)
    End Function

    Private Sub setPaint()
        Dim zero As IntPtr = IntPtr.Zero
        Dim dc As IntPtr = GetDC(IntPtr.Zero)
        Dim hgdiobj As IntPtr = IntPtr.Zero
        Dim hdc As IntPtr = CreateCompatibleDC(dc)
        Try
            Dim pptdst As WinPoint = New WinPoint
            pptdst.x = Me.Left
            pptdst.y = Me.Top
            Dim psize As WinSize = New WinSize With {.cx = Me.Width, .cy = Me.Height}
            Dim pblend As BLENDFUNCTION = New BLENDFUNCTION()
            Dim pprsrc As WinPoint = New WinPoint With {.x = 0, .y = 0}
            hgdiobj = _ShadowImage.GetHbitmap(Color.FromArgb(0))
            zero = SelectObject(hdc, hgdiobj)
            pblend.BlendOp = 0
            pblend.SourceConstantAlpha = Byte.Parse("255")
            pblend.AlphaFormat = 1
            pblend.BlendFlags = 0
            If Not UpdateLayeredWindow(MyBase.Handle, dc, pptdst, psize, hdc, pprsrc, 0, pblend, 2) Then
                Dim x = GetLastError()
            End If
            Return
        Finally
            If hgdiobj <> IntPtr.Zero Then
                SelectObject(hdc, zero)
                DeleteObject(hgdiobj)
            End If
            ReleaseDC(IntPtr.Zero, dc)
            DeleteDC(hdc)
        End Try
    End Sub

#Region "import dll"
    <DllImport("gdi32.dll")>
    Private Shared Function DeleteDC(ByVal hdc As IntPtr) As Boolean

    End Function
    <DllImport("user32.dll")>
    Private Shared Function ReleaseDC(ByVal hwnd As IntPtr, ByVal hdc As IntPtr) As Integer

    End Function
    <DllImport("kernel32.dll")>
    Private Shared Function GetLastError() As Integer

    End Function
    <DllImport("user32.dll")>
    Private Shared Function UpdateLayeredWindow(ByVal hwnd As IntPtr, ByVal sdc As IntPtr, ByRef loc As WinPoint, ByRef size As WinSize, ByVal srcdc As IntPtr, ByRef sloc As WinPoint, ByVal c As Integer, ByRef bd As BLENDFUNCTION, ByVal x As Integer) As Integer

    End Function
    <DllImport("gdi32.dll")>
    Private Shared Function CreateCompatibleDC(ByVal intptr As IntPtr) As IntPtr

    End Function
    <DllImport("user32.dll")>
    Private Shared Function GetDC(ByVal hwnd As IntPtr) As IntPtr

    End Function
    <DllImport("gdi32.dll")>
    Private Shared Function DeleteObject(ByVal hwnd As IntPtr) As Boolean

    End Function
    <DllImport("gdi32.dll")>
    Private Shared Function SelectObject(ByVal hwnd As IntPtr, ByVal obj As IntPtr) As Integer

    End Function
#End Region
#Region "WinStructure"

    Structure WinPoint
        Dim x As Integer
        Dim y As Integer
    End Structure
    Structure WinSize
        Dim cx As Integer
        Dim cy As Integer
    End Structure

    Structure BLENDFUNCTION
        Dim BlendOp As Byte
        Dim BlendFlags As Byte
        Dim SourceConstantAlpha As Byte
        Dim AlphaFormat As Byte
    End Structure
#End Region

End Class