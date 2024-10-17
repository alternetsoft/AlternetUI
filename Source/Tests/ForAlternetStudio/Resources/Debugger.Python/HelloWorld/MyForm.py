import MyForm_Designer
from MyForm_Designer import *

class MyForm (MyForm_Designer): 
    """A form class"""
   
    def __init__(self):
        super().__init__()
        self.InitializeComponent();
  
        
    @staticmethod
    def  RunForm():
        form = MyForm()
        form.ShowDialog()
        form.Dispose()
	
    def CloseButton_Click(self, sender, args):
        self.Close()

    def CloseButton_MouseClick(self, sender, e):
      System.Windows.Forms.MessageBox.Show("click")
    
    def listBox1_DragOver(self, sender, e):
      System.Windows.Forms.MessageBox.Show("Drag over")

System.Console.WriteLine("Hello from Python")
MyForm.RunForm()

  
