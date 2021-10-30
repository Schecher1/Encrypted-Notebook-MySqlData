using Encrypted_Notebook.Class;
using System.IO;
using System.Windows;

namespace Encrypted_Notebook.Page
{
    public partial class pageUserLogin
    {
        MainWindow mw = (MainWindow)Application.Current.MainWindow;
        DatabaseManager DBMgr = new DatabaseManager();

        public pageUserLogin()
        {
            InitializeComponent();
            msgBox_error.Visibility = Visibility.Hidden;
            mw.pageMirror.NavigationService.RemoveBackEntry();
        }

        private void bttn_login_Click(object sender, RoutedEventArgs e)
        {
            if (tb_password.Password == "" || tb_username.Text == "")
            {
                msgBox_error.Text = ("The login data can not be empty!");
                msgBox_error.Visibility = Visibility.Visible;
                return;
            }

            if (DBMgr.loginUser(tb_username.Text, tb_password.Password))
            {
                mw.pageMirror.Content = new pageUserHome();
            }
            else
            {
                msgBox_error.Text = ("The login data do not match!");
                msgBox_error.Visibility = Visibility.Visible;
            }
        }

        private void bttn_createUser_Click(object sender, RoutedEventArgs e) => mw.pageMirror.Content = new pageUserCreate();

        private void bttn_BackTo_Click(object sender, RoutedEventArgs e)
        {
            DBMgr.dbDisconnect();
            if (File.Exists("c2s_owl.gnm"))
                mw.pageMirror.Content = new pageServerOneWayLogin();
            else
                mw.pageMirror.Content = new pageServerLogin();
        }
    }
}
