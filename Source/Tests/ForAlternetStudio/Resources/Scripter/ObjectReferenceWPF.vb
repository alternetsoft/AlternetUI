Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Threading

Public Class Catcher
    Inherits DependencyObject
    Implements IDisposable

    Public Sub CatchButton()
        ScriptGlobalClass.RunButton.Content = "Catch me if you can"
    End Sub

    Public Sub ChangeButtonLocation(ByVal obj As Object, ByVal args As EventArgs)
        Dim autoRand As Random = New Random()
        Dim x As Integer = CInt((30 * autoRand.Next(0, 10) + 1))
        Dim y As Integer = CInt((11 * autoRand.Next(0, 10) + 1))
        ScriptGlobalClass.RunButton.Margin = New Thickness(x, y, 0, 0)
    End Sub

    Public Sub RunButtonClick(ByVal sender As Object, ByVal es As RoutedEventArgs)
        t.Stop()
        ScriptGlobalClass.RunButton.Content = "Test Button"
    End Sub

    Public Shared Function Main() As Catcher
        Dim f As Catcher = New Catcher()
        f.CatchButton()
        Return f
    End Function

    Private t As DispatcherTimer

    Public Sub New()
        t = New DispatcherTimer()
        t.Interval = TimeSpan.FromMilliseconds(1000)

        AddHandler Me.t.Tick, AddressOf Me.ChangeButtonLocation
        AddHandler ScriptGlobalClass.RunButton.Click, AddressOf Me.RunButtonClick
        t.Start()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        t.Stop()
        RemoveHandler ScriptGlobalClass.RunButton.Click, AddressOf RunButtonClick
    End Sub
End Class
