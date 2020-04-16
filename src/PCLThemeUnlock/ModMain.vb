Imports System.Windows.Forms

Module ModMain

    '名称
    Public Const 名称 As String = "PCL2一键主题解锁工具"

    '定义主窗体
    Public Form_Main As FrmMain

    '主类
    Sub Main()

        '初始化窗体
        Form_Main = New FrmMain
        Form_Main.Show()

        '运行
        Application.Run()

    End Sub

End Module
