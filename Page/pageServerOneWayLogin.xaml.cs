using System.Windows;
using System.IO;
using Encrypted_Notebook.Class;

namespace Encrypted_Notebook.Page
{
    public partial class pageServerOneWayLogin
    {
        MainWindow mw = (MainWindow)Application.Current.MainWindow;
        DatabaseManager DBMgr = new DatabaseManager();
        EncryptionManager EMgr = new EncryptionManager();

        public pageServerOneWayLogin()
        {
            InitializeComponent();
            msgBox_error.Visibility = Visibility.Hidden;
        }

        private void bttn_login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] _data = File.ReadAllLines("c2s_owl.gnm");
                byte[] salt = SplitManager.SplitStringIntoByteArray(_data[0]);
                string[] loginData = EMgr.DecryptAES256Salt(_data[1], tb_filePassword.Password, salt).Split(':');

                DBMgr.connectionString(loginData[0], loginData[1], loginData[2], loginData[3]);

                if (DBMgr.dbConnect() == "successfully connected to the database!")
                {
                    if (DBMgr.checkIfServerIsConfigured() == 1)
                        mw.pageMirror.Content = new pageUserLogin();
                    else
                        mw.pageMirror.Content = new pageServerConfigure();
                }
                else
                {
                    msgBox_error.Text = ("No connection could be established");
                    msgBox_error.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                msgBox_error.Text = ("No connection could be established");
                msgBox_error.Visibility = Visibility.Visible;
            }
        }

        private void bttn_delete_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("c2s_owl.gnm"))
            {
                File.Delete("c2s_owl.gnm");
                mw.pageMirror.Content = new pageServerLogin();
            }
        }

        private void bttn_BackTo_Click(object sender, RoutedEventArgs e) => mw.pageMirror.Content = new pageServerLogin();
    }
}
