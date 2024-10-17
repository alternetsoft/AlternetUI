Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Media

Namespace ScriptSpace
    Public Class ScriptClass
        Shared initialized As Boolean
        Shared arenaBackgroundColor As String
        Shared dotColor As String
        Shared currentAngle As Double
        Shared radius As Double
        Shared dotRadius As Double
        Shared center As Point

        Private Shared Function DegreesToRadians(ByVal degrees As Double) As Double
            Dim radians As Double = (Math.PI / 180) * degrees
            Return radians
        End Function

        Private Shared Sub InitializeIfNeeded(ByVal bounds As Rect)
            If initialized Then Return
            arenaBackgroundColor = Colors.DarkBlue.ToString()
            dotColor = Colors.White.ToString()
            Dim maxSide As Double = Math.Max(bounds.Width, bounds.Height)
            radius = (maxSide - (maxSide / 3)) / 2
            dotRadius = maxSide / 20
            center = New Point(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2)
            initialized = True
        End Sub

        Public Shared Sub OnRender(ByVal wrapper As IsolatedScript.DrawingContextWrapper, ByVal bounds As Rect)
            InitializeIfNeeded(bounds)
            Dim arenaBounds = bounds
            arenaBounds.Inflate(-2, -2)
            Dim center As Point = New Point((arenaBounds.Left + arenaBounds.Right) / 2, (arenaBounds.Top + arenaBounds.Bottom) / 2)
            wrapper.DrawEllipse(arenaBackgroundColor, Nothing, center, arenaBounds.Width / 2, arenaBounds.Height / 2)
            Dim radians = DegreesToRadians(currentAngle)
            Dim dotCenter = New Point(center.X + CInt((Math.Cos(radians) * radius)), center.Y + CInt((Math.Sin(radians) * radius)))
            wrapper.DrawEllipse(dotColor, Nothing, dotCenter, dotRadius * 2, dotRadius * 2)
        End Sub

        Private Shared Function ConstrainAngle(ByVal x As Double) As Double
            x = x Mod 360
            If x < 0 Then x += 360
            Return x
        End Function

        Public Shared Sub OnUpdate(ByVal deltaTimeMs As Integer)
            currentAngle += deltaTimeMs * 0.1
            currentAngle = ConstrainAngle(currentAngle)
            Debug.WriteLine("Current Angle: " + currentAngle)
            ScriptGlobalClass.MyObject.UpdateCurrentAngle(currentAngle)
        End Sub

        Public Shared Sub Main()
        End Sub
    End Class
End Namespace
