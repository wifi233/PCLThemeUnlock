<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMain
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMain))
        Me.Button_UnlockAll = New System.Windows.Forms.Panel()
        Me.TextOutput = New System.Windows.Forms.TextBox()
        Me.Button_极客蓝 = New System.Windows.Forms.Panel()
        Me.Button_活跃橙 = New System.Windows.Forms.Panel()
        Me.Button_秋仪金 = New System.Windows.Forms.Panel()
        Me.Button_Copy = New System.Windows.Forms.Panel()
        Me.Label_HidenTheme = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Button_UnlockAll
        '
        Me.Button_UnlockAll.Location = New System.Drawing.Point(12, 66)
        Me.Button_UnlockAll.Name = "Button_UnlockAll"
        Me.Button_UnlockAll.Size = New System.Drawing.Size(80, 40)
        Me.Button_UnlockAll.TabIndex = 4
        '
        'TextOutput
        '
        Me.TextOutput.BackColor = System.Drawing.Color.White
        Me.TextOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextOutput.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.TextOutput.Location = New System.Drawing.Point(12, 112)
        Me.TextOutput.Name = "TextOutput"
        Me.TextOutput.ReadOnly = True
        Me.TextOutput.Size = New System.Drawing.Size(476, 29)
        Me.TextOutput.TabIndex = 5
        Me.TextOutput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Button_极客蓝
        '
        Me.Button_极客蓝.Location = New System.Drawing.Point(176, 66)
        Me.Button_极客蓝.Name = "Button_极客蓝"
        Me.Button_极客蓝.Size = New System.Drawing.Size(100, 40)
        Me.Button_极客蓝.TabIndex = 5
        Me.Button_极客蓝.Tag = "0"
        '
        'Button_活跃橙
        '
        Me.Button_活跃橙.Location = New System.Drawing.Point(282, 66)
        Me.Button_活跃橙.Name = "Button_活跃橙"
        Me.Button_活跃橙.Size = New System.Drawing.Size(100, 40)
        Me.Button_活跃橙.TabIndex = 6
        Me.Button_活跃橙.Tag = "1"
        '
        'Button_秋仪金
        '
        Me.Button_秋仪金.Location = New System.Drawing.Point(388, 66)
        Me.Button_秋仪金.Name = "Button_秋仪金"
        Me.Button_秋仪金.Size = New System.Drawing.Size(100, 40)
        Me.Button_秋仪金.TabIndex = 6
        Me.Button_秋仪金.Tag = "2"
        '
        'Button_Copy
        '
        Me.Button_Copy.Location = New System.Drawing.Point(388, 147)
        Me.Button_Copy.Name = "Button_Copy"
        Me.Button_Copy.Size = New System.Drawing.Size(100, 40)
        Me.Button_Copy.TabIndex = 5
        '
        'Label_HidenTheme
        '
        Me.Label_HidenTheme.AutoSize = True
        Me.Label_HidenTheme.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Label_HidenTheme.Font = New System.Drawing.Font("微软雅黑", 12.0!, System.Drawing.FontStyle.Underline)
        Me.Label_HidenTheme.ForeColor = System.Drawing.SystemColors.Highlight
        Me.Label_HidenTheme.Location = New System.Drawing.Point(12, 166)
        Me.Label_HidenTheme.Name = "Label_HidenTheme"
        Me.Label_HidenTheme.Size = New System.Drawing.Size(154, 21)
        Me.Label_HidenTheme.TabIndex = 7
        Me.Label_HidenTheme.Text = "关于隐藏的隐藏主题"
        '
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(500, 200)
        Me.Controls.Add(Me.Label_HidenTheme)
        Me.Controls.Add(Me.Button_Copy)
        Me.Controls.Add(Me.Button_秋仪金)
        Me.Controls.Add(Me.Button_活跃橙)
        Me.Controls.Add(Me.Button_极客蓝)
        Me.Controls.Add(Me.TextOutput)
        Me.Controls.Add(Me.Button_UnlockAll)
        Me.Font = New System.Drawing.Font("微软雅黑", 12.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(5)
        Me.Name = "FrmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button_UnlockAll As System.Windows.Forms.Panel
    Friend WithEvents TextOutput As System.Windows.Forms.TextBox
    Friend WithEvents Button_极客蓝 As System.Windows.Forms.Panel
    Friend WithEvents Button_活跃橙 As System.Windows.Forms.Panel
    Friend WithEvents Button_秋仪金 As System.Windows.Forms.Panel
    Friend WithEvents Button_Copy As System.Windows.Forms.Panel
    Friend WithEvents Label_HidenTheme As System.Windows.Forms.Label
End Class
