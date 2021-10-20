using System.Windows;

namespace Encrypted_Notebook
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            pageMirror.Content = new Page.pageLoginServer();
        }
    }
}
