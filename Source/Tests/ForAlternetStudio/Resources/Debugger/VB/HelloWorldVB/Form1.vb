Public Class Form1

    Private Overloads Function FF(i As Integer, b As String) As Integer
        Return 0
    End Function

    Private Overloads Function FF(i As Integer) As Integer
        Return 0
    End Function

    Public Overloads Sub XX()

    End Sub

    Public Overloads Sub XX(i As Integer, b As String)
    End Sub

    Public Sub Test()
    End Sub
    Public Sub Test(a As Integer, b As Integer)
    End Sub

    Private Sub Label1_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles Label1.Paint

        Label1.Text = "hi"

    End Sub
End Class
