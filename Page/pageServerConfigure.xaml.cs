using Encrypted_Notebook.Class;
using System.Windows;

namespace Encrypted_Notebook.Page
{
    public partial class pageServerConfigure
    {

        MainWindow mw = (MainWindow)Application.Current.MainWindow;
        DatabaseManager DBMgr = new DatabaseManager();

        public pageServerConfigure() => InitializeComponent();

        private void bttn_configure_Click(object sender, RoutedEventArgs e)
        {
            if (DBMgr.ConfiguredServer() == 1)
                mw.pageMirror.Content = new pageUserLogin();
            else
                mw.pageMirror.Content = new pageDatabase404();
        }
    }
}
