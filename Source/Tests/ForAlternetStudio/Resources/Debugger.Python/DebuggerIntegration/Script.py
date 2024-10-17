import MyModule
from datetime import date
import numpy as np

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
        

def ChangeMenuItem():
    TestMenuItem.Text += "123"
    TestMenuItem.Enabled = not TestMenuItem.Enabled

ChangeMenuItem()

x = 123
a = [1, 2, 3, 4, 5]

def MyFunc():
    mylocal = 11
    mylocal2 = 22

MyFunc()

c = MyClass()
print(MyClass);
MyClass.ff();

hw = c.ff();

print(c.i);

#while True: 
#    print(x) 

lst1 = System.Collections.ArrayList()

lst = System.Collections.Generic.List[str]()
lst.Add('Hello')
lst.Add('World')


result = [[0,0,0],
         [0,0,0],
         [0,0,0]]
 

 
det = np.linalg.det(np.array(result))
print("det = " + str(det))

lst = System.Collections.Generic.List[str]()
lst.Add('Hello')
lst.Add('World')

MessageBox.Show("From Module: " + MyModule.GetString())
def MyFunction(message):
    MessageBox.Show("MyFunction: " + message)

   
MyFunction("s")
