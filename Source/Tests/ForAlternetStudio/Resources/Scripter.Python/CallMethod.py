arenaBackgroundBrush = SolidBrush(Color.DarkBlue)
dotBrush = SolidBrush(Color.White)

def DegreesToRadians(degrees):
  radians = (Math.PI / 180) * degrees 
  return radians

def OnPaint(g, bounds, currentAngle):
  maxSide = Math.Max(bounds.Width, bounds.Height)
  radius = (maxSide - (maxSide / 3)) / 2
  dotRadius = maxSide / 20
  center = Point(int(bounds.Left + bounds.Width / 2), int(bounds.Top + bounds.Height / 2))
  g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality
  g.Clear(SystemColors.Control)
  arenaBounds = bounds
  arenaBounds.Inflate(-2, -2)
  g.FillEllipse(arenaBackgroundBrush, arenaBounds)
  radians = DegreesToRadians(currentAngle)
  dotCenter = Point(int(center.X + (Math.Cos(radians) * radius)), int(center.Y + (Math.Sin(radians) * radius)))
  g.FillEllipse(dotBrush, dotCenter.X - dotRadius, dotCenter.Y - dotRadius, dotRadius * 2, dotRadius * 2)

def ConstrainAngle(x):
  x %= 360
  if x < 0:
      x += 360
  return x

