 
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
}

MyForm.RunForm();
  
