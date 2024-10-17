using System;

public partial class MyForm : System.Windows.Forms.Form
{

    public MyForm()
    {
        this.InitializeComponent();
    }

    [STAThread]
    public static void Main()
    {
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
        System.Windows.Forms.Application.Run(new MyForm());
    }

    private void CloseButton_Click(object sender, System.EventArgs e)
    {
        Close();
    }
}
