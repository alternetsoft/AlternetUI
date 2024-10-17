Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Public Class Catcher
    Inherits System.ComponentModel.Component

    Public Sub CatchButton()
        ScriptGlobalClass.RunButton.Text = "Catch me if you can"
    End Sub

    Public Sub ChangeButtonLocation(ByVal obj As Object, ByVal args As EventArgs)
        Dim autoRand As Random = New Random
        Dim x As Integer = CType(((35 * autoRand.Next(0, 10)) _
                    + 1), Integer)
        Dim y As Integer = CType(((15 * autoRand.Next(0, 10)) _
                    + 1), Integer)
        ScriptGlobalClass.RunButton.Location = New Point(x, y)
    End Sub

    Public Sub RunButtonClick(ByVal obj As Object, ByVal args As EventArgs)
        t.Stop
        ScriptGlobalClass.RunButton.Text = "Test Button"
    End Sub

    Public Shared Function Main() As Catcher
        Dim f As Catcher = New Catcher
        f.CatchButton()
        Return f
    End Function

    Private t As Timer

    Public Sub New()
        MyBase.New
        Me.t = New Timer
        Me.t.Interval = 1000
        AddHandler Me.t.Tick, AddressOf Me.ChangeButtonLocation
        AddHandler ScriptGlobalClass.RunButton.Click, AddressOf Me.RunButtonClick
        Me.t.Start
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            RemoveHandler ScriptGlobalClass.RunButton.Click, AddressOf RunButtonClick
            t.Stop()
            t.Dispose()
        End If

        MyBase.Dispose(disposing)
    End Sub
End Class