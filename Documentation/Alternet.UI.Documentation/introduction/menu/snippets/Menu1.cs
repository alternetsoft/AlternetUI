public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = this;
        SaveCommand = new Command(o => MessageBox.Show("Save"));
    }

    public Command SaveCommand { get; }

    private void OpenMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("Open");

    private void GridMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("Grid item is checked: " + gridMenuItem.Checked);

    private void ExitMenuItem_Click(object sender, EventArgs e) => Close();
}