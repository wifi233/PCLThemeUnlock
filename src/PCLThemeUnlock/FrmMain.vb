Imports System.Drawing
Imports System.Windows.Forms
Imports System.Threading

Public Class FrmMain

    '定义API
    Private Declare Auto Function ReleaseCapture Lib "user32.dll" Alias "ReleaseCapture" () As Boolean
    Private Declare Auto Function SendMessage Lib "user32.dll" Alias "SendMessage" (ByVal hWnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As IntPtr
    Private Const WM_SYSCOMMAND As Integer = &H112&
    Private Const SC_MOVE As Integer = &HF010&
    Private Const HTCAPTION As Integer = &H2&

    '定义顶部按钮
    Private TopButton_Close As Panel
    Private TopButton_Min As Panel


    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

        '加载标题
        Me.Text = 名称

        '绘制背景
        Dim FormRect As New Rectangle(0, 0, Me.Width - 1, Me.Height - 1)
        Dim TopRect As New Rectangle(0, 0, Me.Width - 1, 29)
        Dim FormBrush As New SolidBrush(Color.FromArgb(4, 175, 255))
        Dim FormPen As New Pen(FormBrush)
        Dim BackBitmap As New Bitmap(Me.Width, Me.Height)
        Dim g As Graphics = Graphics.FromImage(BackBitmap)
        With g
            .Clear(Color.White)
            .DrawRectangle(FormPen, FormRect)
            .FillRectangle(FormBrush, TopRect)
            .DrawLine(Pens.Black, 39, 229, 459, 229)
            .DrawString(名称, Me.Font, Brushes.White, 4, 4)
            .DrawString("解锁", Me.Font, Brushes.Black, 30, 40)
            .DrawString("生成解锁码", Me.Font, Brushes.Black, 290, 40)
            .Dispose()
        End With
        g = Nothing
        FormPen.Dispose()
        FormPen = Nothing
        FormBrush.Dispose()
        FormBrush = Nothing
        FormRect = Nothing
        TopRect = Nothing
        Me.BackgroundImage = BackBitmap

        '注册阴影窗体
        ShadowForm.RegisterShadowForm(Me)

        '创建顶部按钮
        TopButton_Close = New Panel
        With TopButton_Close
            .Width = 30
            .Height = 20
            .Left = Me.Width - 10 - .Width - 1
            .Top = 4
            .Tag = CreateTop(0)
            .Cursor = Cursors.Hand
            .BackgroundImage = .Tag(0)
            .BackColor = Color.Transparent
            AddHandler .MouseEnter, AddressOf TopButton_MouseEnter
            AddHandler .MouseLeave, AddressOf TopButton_MouseLeave
            AddHandler .MouseDown, AddressOf TopButton_MouseDown
            AddHandler .MouseUp, AddressOf TopButton_MouseUp
            AddHandler .Click, Sub() Me.Close()
        End With

        TopButton_Min = New Panel
        With TopButton_Min
            .Width = 30
            .Height = 20
            .Left = Me.Width - 20 - (2 * .Width) - 1
            .Top = 4
            .Tag = CreateTop(1)
            .Cursor = Cursors.Hand
            .BackgroundImage = .Tag(0)
            .BackColor = Color.Transparent
            AddHandler .MouseEnter, AddressOf TopButton_MouseEnter
            AddHandler .MouseLeave, AddressOf TopButton_MouseLeave
            AddHandler .MouseDown, AddressOf TopButton_MouseDown
            AddHandler .MouseUp, AddressOf TopButton_MouseUp
            AddHandler .Click, AddressOf TopMin_Click
        End With

        With Me.Controls
            .Add(TopButton_Close)
            .Add(TopButton_Min)
        End With

        '加载按钮
        Call LoadBlueButton(Button_UnlockAll, "解锁全部")
        Call LoadBlueButton(Button_极客蓝, "极客蓝", 0)
        Call LoadBlueButton(Button_活跃橙, "活跃橙", 1)
        Call LoadBlueButton(Button_秋仪金, "秋仪金", 2)
        Call LoadBlueButton(Button_Copy, "复制解锁码")

        '创建委托
        AddHandler Button_极客蓝.Click, AddressOf Button_GetDonateCode_Click
        AddHandler Button_活跃橙.Click, AddressOf Button_GetDonateCode_Click
        AddHandler Button_秋仪金.Click, AddressOf Button_GetDonateCode_Click

    End Sub

    Private Sub TopMin_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        With sender
            .BackgroundImage = .Tag(0)
            .Refresh()
        End With
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub FrmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        '隐藏窗体
        Me.Visible = False

        '销毁顶部按钮
        With Me.Controls
            .Remove(TopButton_Close)
            .Remove(TopButton_Min)
        End With
        TopButton_Close.Dispose()
        TopButton_Close = Nothing
        TopButton_Min.Dispose()
        TopButton_Min = Nothing

        '退出程序
        End
    End Sub

    Private Sub TopButton_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs)
        With sender
            .BackgroundImage = .Tag(1)
        End With
    End Sub

    Private Sub TopButton_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs)
        With sender
            .BackgroundImage = .Tag(0)
        End With
    End Sub

    Private Sub TopButton_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        With sender
            .BackgroundImage = .Tag(1)
        End With
    End Sub

    Private Sub TopButton_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        With sender
            .BackgroundImage = .Tag(0)
        End With
    End Sub

    Private Sub FrmMain_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        Call MoveFormByLeft(e, Me)
    End Sub

    '按钮相关
    Private Sub LoadBlueButton(ByVal MyButton As Panel, ByVal Str As String, Optional ByVal Tag As Object = Nothing)
        With MyButton
            .BackColor = Color.FromArgb(48, 150, 255)
            .Tag = New Object(3) {}
            .Tag(0) = Str
            .Tag(2) = CreatePic(.Width, .Height, .Font, .Tag(0), Color.White, 48, 150, 255)
            .Tag(3) = Tag
            .Cursor = Cursors.Hand
            AddHandler .MouseEnter, AddressOf Button_Blue_MouseEnter
            AddHandler .MouseLeave, AddressOf Button_Blue_MouseLeave
            AddHandler .Paint, AddressOf Button_Blue_Paint
        End With
    End Sub

    Private Sub Button_Blue_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs)
        With sender
            .Tag(1) = True
            Dim Cr As Byte, Cg As Byte
            Cr = .BackColor.R
            Cg = .BackColor.G
            While Cr < 102 And Cg < 204
                Cr += 3
                Cg += 3
                .BackColor = Color.FromArgb(Cr, Cg, .BackColor.B)
                .Tag(2) = CreatePic(.Width, .Height, .Font, .Tag(0), Color.White, Cr, Cg, 255)
                .Refresh()
                Thread.Sleep(10)
                Application.DoEvents()
                If .Tag(1) = False Then Exit Sub
            End While
            .BackColor = Color.FromArgb(102, 204, 255)
            .Tag(2) = CreatePic(.Width, .Height, .Font, .Tag(0), Color.White, 102, 204, 255)
            .Refresh()
        End With
    End Sub

    Private Sub Button_Blue_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs)
        With sender
            .Tag(1) = False
            Dim Cr As Byte, Cg As Byte
            Cr = .BackColor.R
            Cg = .BackColor.G
            While Cr > 48 And Cg > 150
                Cr -= 3
                Cg -= 3
                .BackColor = Color.FromArgb(Cr, Cg, .BackColor.B)
                .Tag(2) = CreatePic(.Width, .Height, .Font, .Tag(0), Color.White, Cr, Cg, 255)
                .Refresh()
                Thread.Sleep(10)
                Application.DoEvents()
                If .Tag(1) = True Then Exit Sub
            End While
            .BackColor = Color.FromArgb(48, 150, 255)
            .Tag(2) = CreatePic(.Width, .Height, .Font, .Tag(0), Color.White, 48, 150, 255)
            .Refresh()
        End With
    End Sub

    Private Sub Button_Blue_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs)
        e.Graphics.DrawImage(sender.Tag(2), 0, 0, sender.Width, sender.Height)
    End Sub

    '窗体移动
    Private Sub MoveFormByLeft(ByVal e As System.Windows.Forms.MouseEventArgs, ByRef MyForm As Form)
        If e.Button = MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(MyForm.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0)
        End If
    End Sub

    '图像生成
    Private Function CreateTop(ByVal Index As Integer) As Bitmap()
        Select Case Index
            Case 0 '关闭
                Dim MyBitmap(1) As Bitmap
                Dim MyRect As New Rectangle(0, 0, 19, 9)
                Dim g As Graphics
                MyBitmap(0) = New Bitmap(30, 20)
                g = Graphics.FromImage(MyBitmap(0))
                With g
                    .Clear(Color.Transparent)
                    .DrawLine(Pens.White, 10, 5, 18, 13)
                    .DrawLine(Pens.White, 18, 5, 10, 13)
                    .Dispose()
                End With
                g = Nothing
                MyBitmap(1) = New Bitmap(30, 20)
                g = Graphics.FromImage(MyBitmap(1))
                With g
                    .Clear(Color.Red)
                    .DrawLine(Pens.White, 10, 5, 18, 13)
                    .DrawLine(Pens.White, 18, 5, 10, 13)
                    .Dispose()
                End With
                g = Nothing
                MyRect = Nothing
                Return MyBitmap
            Case 1 '最小化
                Dim MyBitmap(1) As Bitmap
                Dim MyRect As New Rectangle(0, 0, 19, 9)
                Dim g As Graphics
                MyBitmap(0) = New Bitmap(30, 20)
                g = Graphics.FromImage(MyBitmap(0))
                With g
                    .Clear(Color.Transparent)
                    .DrawLine(Pens.White, Convert.ToSingle(9.0), Convert.ToSingle(9.5), Convert.ToSingle(19.0), Convert.ToSingle(9.5))
                    .Dispose()
                End With
                g = Nothing
                MyBitmap(1) = New Bitmap(30, 20)
                g = Graphics.FromImage(MyBitmap(1))
                With g
                    .Clear(Color.Blue)
                    .DrawLine(Pens.White, Convert.ToSingle(9.0), Convert.ToSingle(9.5), Convert.ToSingle(19.0), Convert.ToSingle(9.5))
                    .Dispose()
                End With
                g = Nothing
                MyRect = Nothing
                Return MyBitmap
            Case Else
                Return Nothing
        End Select
    End Function

    '绘制文字
    Private Sub CenterStr(ByVal g As Graphics, ByVal MyStr As String, ByVal MyFont As Font, ByVal Width As Single, ByVal LocY As Single)
        Dim MySizeF As SizeF
        Dim LocX As Single
        MySizeF = g.MeasureString(MyStr, MyFont)
        LocX = ((Width - MySizeF.Width) / 2) - 1
        MySizeF = Nothing
        g.DrawString(MyStr, MyFont, Brushes.Black, LocX, LocY)
    End Sub

    '绘制按钮图片
    Private Function CreatePic(ByVal Width As Integer, ByVal Height As Integer, ByVal MyFont As Font, ByVal MyStr As String, ByVal StrColor As Color, ByVal Cr As Byte, ByVal Cg As Byte, ByVal Cb As Byte) As Bitmap
        Dim MyBitmap As Bitmap = New Bitmap(Width, Height)
        Dim g As Graphics = Graphics.FromImage(MyBitmap)
        Dim MySizeF As SizeF = g.MeasureString(MyStr, MyFont)
        Dim LocX As Single, LocY As Single
        LocX = ((Width - MySizeF.Width) / 2) - 1
        LocY = ((Height - MySizeF.Height) / 2) - 1
        MySizeF = Nothing
        Dim MyBrush As New SolidBrush(StrColor)
        Dim MyRect As New Rectangle(2, 2, Width - 5, Height - 5)
        With g
            .Clear(Color.FromArgb(Cr, Cg, Cb))
            .DrawRectangle(Pens.White, MyRect)
            .DrawString(MyStr, MyFont, MyBrush, LocX, LocY)
            .Dispose()
        End With
        g = Nothing
        MyRect = Nothing
        MyBrush.Dispose()
        MyBrush = Nothing
        Return MyBitmap
    End Function

    '显示消息
    Private Sub Msg(ByVal Str As String, ByVal MyForm As Form, Optional ByVal Time As Integer = 1000)
        Dim MyBitmap As Bitmap = New Bitmap(10, 10)
        Dim MySizeF As SizeF
        Dim g As Graphics = Graphics.FromImage(MyBitmap)
        MySizeF = g.MeasureString(Str, MyForm.Font)
        Dim Width As Integer, Height As Integer
        Width = MySizeF.Width + 20
        Height = MySizeF.Height + 10
        MySizeF = Nothing
        g.Dispose()
        g = Nothing
        MyBitmap.Dispose()
        MyBitmap = Nothing
        MyBitmap = New Bitmap(Width, Height)
        Dim MyRect As New Rectangle(1, 1, Width - 3, Height - 3)
        g = Graphics.FromImage(MyBitmap)
        With g
            .Clear(Color.FromArgb(128, 0, 0, 0))
            .DrawString(Str, MyForm.Font, Brushes.White, 9, 4)
            .DrawRectangle(Pens.White, MyRect)
            .Dispose()
        End With
        g = Nothing
        MyRect = Nothing

        Dim MyPanel As New Panel
        Dim LocY As Single = ((MyForm.Height - Height) / 2) - 1
        Dim i As Integer
        With MyPanel
            .Width = Width
            .Height = Height
            .Top = MyForm.Height
            .Left = ((MyForm.Width - Width) / 2) - 1
            .BackColor = Color.Transparent
            .BackgroundImage = MyBitmap
            MyForm.Controls.Add(MyPanel)
            .BringToFront()
            .Focus()
            While .Top > LocY
                .Top -= 30
                Thread.Sleep(10)
                Application.DoEvents()
            End While
            .Top = LocY
            For i = 1 To Time
                Thread.Sleep(1)
                Application.DoEvents()
            Next
            While .Top > -Height - 1
                .Top -= 30
                Thread.Sleep(10)
                Application.DoEvents()
            End While
            MyForm.Controls.Remove(MyPanel)
            MyPanel.Dispose()
        End With
        MyPanel = Nothing
        MyBitmap.Dispose()
        MyBitmap = Nothing

    End Sub

    Private Sub Button_Copy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Copy.Click
        Try
            Clipboard.SetText(TextOutput.Text)
            Msg("解锁码复制成功", Me)
        Catch ex As Exception
            Msg("解锁码复制失败", Me)
        End Try
    End Sub

    Private Sub Button_UnlockAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_UnlockAll.Click
        If MsgBox("在开始之前，请先确定您已经在本设备上运行过PCL2，并且您的PCL2已经关闭。", vbYesNo, 名称) = vbYes Then
            If PCLUnlocker.UnlockTheme Then
                Msg("恭喜，解锁成功！", Me)
            Else
                Msg("解锁失败，请确保PCL2已在本设备运行过，并检查程序是否有权限访问注册表。", Me, 3000)
            End If
        End If
    End Sub

    Private Sub Button_GetDonateCode_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim RStr As String = PCLUnlocker.GetDonateCode(sender.Tag(3))
        If Not String.IsNullOrEmpty(RStr) Then
            TextOutput.Text = RStr
        Else
            TextOutput.Text = vbNullString
            Msg("解锁码生成失败", Me)
        End If
    End Sub

    Private Sub Label_HidenTheme_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label_HidenTheme.Click
        Msg("PCL目录下的Setup.ini文件内的" & vbCrLf & "UiLauncherTheme选项即为当前主题" & vbCrLf & "修改为42即可开启隐藏的隐藏主题星空蓝(请先解锁后修改)", Me, 5000)
    End Sub

End Class