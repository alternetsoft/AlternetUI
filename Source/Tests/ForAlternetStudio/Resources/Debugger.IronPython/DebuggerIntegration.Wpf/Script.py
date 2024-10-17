
import MyModule
from datetime import date

class MyClass:
    """A simple example class"""
    i = 12345

    def __init__(self):
        self.my_instance_var = 34
    
    def f(self):
        xx = 'hello world'
        return xx


def ChangeMenuItem():
    TestMenuItem.Text += "123"
    TestMenuItem.Enabled = not TestMenuItem.Enabled

ChangeMenuItem()

x = 123
a = [1, 2, 3, 4, 5]
e = date.today()
c = MyClass()
print(MyClass);
hw = c.f()

#while True:
#    print(x)

MessageBox.Show("From Module: " + MyModule.GetString())

def MyFunction(message):
    MessageBox.Show("MyFunction: " + message)
