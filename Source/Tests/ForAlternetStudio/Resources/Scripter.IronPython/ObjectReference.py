def ChangeButtonLocation(o, e):
  autoRand = Random()
  x = (int)(35 * autoRand.Next(0, 10) + 1)
  y = (int)(15 * autoRand.Next(0, 10) + 1)
  RunButton.Location = Point(x, y)

def RunButtonClick(o,e):
  timer.Stop()
  RunButton.Text = "Test Button"

def Main(buttonText):
  RunButton.Click += RunButtonClick
  RunButton.Text = buttonText
  timer.Interval = 1000
  timer.Tick += ChangeButtonLocation
  timer.Start()
