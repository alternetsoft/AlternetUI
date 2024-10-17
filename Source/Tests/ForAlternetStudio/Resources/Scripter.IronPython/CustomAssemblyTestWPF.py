class TestClass:
  def UseExternal(tc):
    customClass = CustomClass()
    customClass.TestMethod(1, True)

def Main():
  f = TestClass()
  f.UseExternal()