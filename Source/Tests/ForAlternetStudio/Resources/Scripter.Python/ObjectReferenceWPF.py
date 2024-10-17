def ChangeButtonLocation(o, e):
  autoRand = Random()
  x = (int)(35 * autoRand.Next(0, 10) + 1)
  y = (int)(15 * autoRand.Next(0, 10) + 1)
  RunButton.Margin = Thickness(x, y, 0, 0)

def RunButtonClick(o,e):
  timer.Stop()
  RunButton.Content = "Test Button"

def Main(buttonText):
  RunButton.Click += RunButtonClick
  RunButton.Content = buttonText
  timer.Interval = TimeSpan.FromMilliseconds(1000)
  timer.Tick += ChangeButtonLocation
  timer.Start()