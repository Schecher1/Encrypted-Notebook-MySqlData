using Encrypted_Notebook.Class;
using System.Net;
using System.Windows;

namespace Encrypted_Notebook.Page{
    public partial class pageUserDelete{
        MainWindow mw = (MainWindow)Application.Current.MainWindow;
        DatabaseManager DBMgr = new DatabaseManager();

        public pageUserDelete(){
            InitializeComponent();
            msgBox_error.Visibility = Visibility.Hidden;
        }

        private void bttn_BackTo_Click(object sender, RoutedEventArgs e) => mw.pageMirror.Content = new pageUserHome();

        private void bttn_delete_Click(object sender, RoutedEventArgs e){
            if (tb_Password.Password == new NetworkCredential("", UserInfoManager.userPassword).Password){
                DBMgr.deleteUser();
                UserInfoManager.userLogout();
                mw.pageMirror.Content = new pageUserLogin();
            }
            else{
                msgBox_error.Text = ("the entered password is not correct!");
                msgBox_error.Visibility = Visibility.Visible;
            }
        }
    }
}
