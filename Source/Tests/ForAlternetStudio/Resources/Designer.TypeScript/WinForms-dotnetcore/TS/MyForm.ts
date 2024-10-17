 
///<reference path="clr.d.ts" />

class MyForm extends MyForm_Designer
{ 

   
    constructor()   
	{ 
		   
		super(); 
		super.InitializeComponent(); 
	}
     
	public Dispose() 
	{
		this.clrSuper.Dispose();
	}
		
	public static RunForm() {
		var form = new MyForm();
		form.ShowDialog();
		form.Dispose();
	}
	
    CloseButton_Click(sender : System.Object, args : System.EventArgs) 
    {
        this.Close(); // Close is not recognized, while this.Close works fine.
    }

    CloseButton_MouseClick(sender: System.Object, e: System.Windows.Forms.MouseEventArgs)
    {
		System.Windows.Forms.MessageBox.Show("click");
    }

    listBox1_DragOver(sender: System.Object, e: System.Windows.Forms.DragEventArgs)
    {
		
    }

    listBox1_SelectedIndexChanged(sender: System.Object, e: System.EventArgs)
    {
        
    }
}


System.Console.WriteLine("Hello from TS 1");
MyForm.RunForm();
System.Console.WriteLine("Hello from TS 2");

  
