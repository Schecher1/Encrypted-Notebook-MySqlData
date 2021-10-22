using Encrypted_Notebook.Class;
using System.Windows;

namespace Encrypted_Notebook.Page
{
    public partial class pageLoginUser
    {
        MainWindow mw = (MainWindow)Application.Current.MainWindow;
        DatabaseManager DBMgr = new DatabaseManager();

        public pageLoginUser()
        {
            InitializeComponent();
            msgBox_error.Visibility = Visibility.Hidden;
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
                mw.pageMirror.Content = new pageNotebook();
            }
            else
            {
                msgBox_error.Text = ("The login data do not match!");
                msgBox_error.Visibility = Visibility.Visible;
            }
        }

        private void bttn_createUser_Click(object sender, RoutedEventArgs e) => mw.pageMirror.Content = new pageCreateUser();

        private void bttn_BackTo_Click(object sender, RoutedEventArgs e)
        {
            DBMgr.dbDisconnect();
            mw.pageMirror.Content = new pageLoginServer();
        }
    }
}
