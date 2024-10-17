Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports ExternalAssembly

Public Class TestClass
    Public Sub UseExternal()
        Dim customClass As CustomClass = New CustomClass()
        customClass.TestMethod(1, True)
    End Sub

    Public Shared Sub Main()
        Dim F As TestClass = New TestClass()
        F.UseExternal()
    End Sub

    Public Sub New()
    End Sub
End Class
