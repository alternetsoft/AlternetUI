Imports System.Windows.Forms

Namespace ExternalAssembly
    Public Class CustomClass
        Public Sub TestMethod(ByVal firstParam As Integer, ByVal secondParam As Boolean)
            MessageBox.Show(String.Format("first param is {0}, second param is {1}", firstParam, secondParam))
        End Sub

        Public Property TestProperty As Integer
    End Class
End Namespace
