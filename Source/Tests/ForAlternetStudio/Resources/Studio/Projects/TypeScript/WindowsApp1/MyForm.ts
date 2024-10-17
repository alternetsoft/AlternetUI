 
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
}

MyForm.RunForm();
  
