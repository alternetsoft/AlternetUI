Imports System
Imports System.Drawing
Imports System.Diagnostics

Namespace ScriptSpace
    Public Class ScriptClass
        Shared initialized As Boolean
        Shared arenaBackgroundBrush As SolidBrush
        Shared dotBrush As SolidBrush
        Shared currentAngle As Double
        Shared radius As Integer
        Shared dotRadius As Integer
        Shared center As Point

        Private Shared Function DegreesToRadians(ByVal degrees As Double) As Double
            Dim radians As Double = (Math.PI / 180) * degrees
            Return radians
        End Function

        Private Shared Sub InitializeIfNeeded(ByVal bounds As Rectangle)
            If initialized Then Return
            arenaBackgroundBrush = New SolidBrush(Color.DarkBlue)
            dotBrush = New SolidBrush(Color.White)
            Dim maxSide As Integer = Math.Max(bounds.Width, bounds.Height)
            radius = (maxSide - (maxSide / 3)) / 2
            dotRadius = maxSide / 20
            center = New Point(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2)
            initialized = True
        End Sub

        Public Shared Sub OnPaint(ByVal g As Graphics, ByVal bounds As Rectangle)
            InitializeIfNeeded(bounds)
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality
            g.Clear(SystemColors.Control)
            Dim arenaBounds = bounds
            arenaBounds.Inflate(-2, -2)
            g.FillEllipse(arenaBackgroundBrush, arenaBounds)
            Dim radians = DegreesToRadians(currentAngle)
            Dim dotCenter = New Point(center.X + CInt((Math.Cos(radians) * radius)), center.Y + CInt((Math.Sin(radians) * radius)))
            g.FillEllipse(dotBrush, dotCenter.X - dotRadius, dotCenter.Y - dotRadius, dotRadius * 2, dotRadius * 2)
        End Sub

        Private Shared Function ConstrainAngle(ByVal x As Double) As Double
            x = x Mod 360
            If x < 0 Then x += 360
            Return x
        End Function

        Public Shared Sub OnUpdate(ByVal deltaTimeMs As Integer)
            currentAngle += deltaTimeMs * 0.1
            currentAngle = ConstrainAngle(currentAngle)
            Debug.WriteLine("Current Angle: " & currentAngle)
        End Sub

        Public Shared Sub Main()
        End Sub
    End Class
End Namespace
