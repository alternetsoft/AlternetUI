

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

        
  
    @staticmethod
    def ff():
        xx = 'hello world'
        return xx
        

x = 123
a = [1, 2, 3, 4, 5]

c = MyClass()
print(MyClass);
MyClass.ff();

hw = c.ff();

print(c.i);

#while True:
#    print(x)
 
MessageBox.Show("From Module: " + MyModule.GetString())
def MyFunction(message):
    MessageBox.Show("MyFunction: " + message)
    
MyFunction("s")

