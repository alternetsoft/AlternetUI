Class MainWindowVB
    <System.STAThreadAttribute()>  _
    Public Shared Sub Main()
        Dim app As System.Windows.Application = New System.Windows.Application()
        app.Run(New MainWindowVB())
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As System.Windows.RoutedEventArgs)
        Close
    End Sub
End Class
