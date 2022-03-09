using System.Windows;
using System.IO;

namespace Encrypted_Notebook{
    public partial class MainWindow : Window{
        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists("c2s_owl.gnm"))
                pageMirror.Content = new Page.pageServerOneWayLogin();
            else
                pageMirror.Content = new Page.pageServerLogin();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
