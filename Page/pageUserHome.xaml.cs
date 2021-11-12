using System.Windows;
using Encrypted_Notebook.Class;

namespace Encrypted_Notebook.Page{
    public partial class pageUserHome{
        public pageUserHome(){
            if (UserInfoManager.userID == -1)
                return;
            InitializeComponent();
            bttn_logout.Content = ($"Logout ({UserInfoManager.userName})");
        }

        MainWindow mw = (MainWindow)Application.Current.MainWindow;

        private void bttn_notebooks_Click(object sender, RoutedEventArgs e) => mw.pageMirror.Content = new pageUserNotebook();
        private void bttn_importExport_Click(object sender, RoutedEventArgs e) => mw.pageMirror.Content = new pageUserNotebooksImportExport();
        private void bttn_delAccount_Click(object sender, RoutedEventArgs e) => mw.pageMirror.Content = new pageUserDelete();
        private void bttn_logout_Click(object sender, RoutedEventArgs e)
        {
            bttn_delAccount.Visibility = Visibility.Hidden;
            bttn_importExport.Visibility = Visibility.Hidden;
            bttn_logout.Visibility = Visibility.Hidden;
            bttn_notebooks.Visibility = Visibility.Hidden;
            UserInfoManager.userLogout();
            mw.pageMirror.Content = new pageUserLogin();
        }
    }
}
