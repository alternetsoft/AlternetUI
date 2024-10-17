Partial Public Class Form1
    Inherits System.Windows.Forms.Form

    Public Sub New()
        MyBase.New
        Me.InitializeComponent
    End Sub

    <System.STAThread()> _
    Public Shared Sub Main()
        System.Windows.Forms.Application.EnableVisualStyles
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(False)
        System.Windows.Forms.Application.Run(New Form1())
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As System.EventArgs) Handles CloseButton.Click
        Close
    End Sub
End Class
