 
///<reference path="clr.d.ts" />

class MyForm extends MyForm_Designer
{ 

   
    constructor()   
	{ 
		   
		super(); 
		super.InitializeComponent(); 
	}
     
    Dispose() 
	{
		System.Console.WriteLine("Dispose");
		this.clrSuper.Dispose();
	}

    static RunForm()
    {
        var form = new MyForm();
        form.ShowDialog();
        form.Dispose();
    }
	
    CloseButton_Click(sender, args) 
    {
        this.Close(); // Close is not recognized, while this.Close works fine.
    }

    CloseButton_MouseClick(sender, e)
    {
		System.Windows.Forms.MessageBox.Show("click");
    }

    listBox1_DragOver(sender, e)
    {
		
    }

    listBox1_SelectedIndexChanged(sender, e)
    {
        
    }
}


System.Console.WriteLine("Hello from JS 1");

MyForm.RunForm();
System.Console.WriteLine("Hello from JS 2");

  
