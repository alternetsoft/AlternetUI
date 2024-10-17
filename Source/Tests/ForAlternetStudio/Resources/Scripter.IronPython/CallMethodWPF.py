arenaBackgroundBrush = SolidColorBrush(Colors.DarkBlue)
dotBrush = SolidColorBrush(Colors.White)

def DegreesToRadians(degrees):
  radians = (Math.PI / 180) * degrees 
  return radians

def OnRender(dc, bounds, pen, currentAngle):
  maxSide = Math.Max(bounds.Width, bounds.Height)
  radius = (maxSide - (maxSide / 3)) / 2
  dotRadius = maxSide / 20

  arenaBounds = bounds
  arenaBounds.Inflate(-2, -2)
  center = System.Windows.Point((arenaBounds.Left + arenaBounds.Right) / 2, (arenaBounds.Top + arenaBounds.Bottom) / 2)
  dc.DrawEllipse(arenaBackgroundBrush, pen, center, arenaBounds.Width / 2, arenaBounds.Height / 2)

  radians = DegreesToRadians(currentAngle)

  dotCenter = System.Windows.Point(center.X + (Math.Cos(radians) * radius), center.Y + (Math.Sin(radians) * radius))
  dc.DrawEllipse(dotBrush, Pen(), dotCenter, dotRadius * 2, dotRadius * 2)

