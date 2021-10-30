using Encrypted_Notebook.Class;
using System.IO;
using System.Windows;

namespace Encrypted_Notebook.Page
{
    public partial class pageServerOneWayLoginKeyCreate
    {
        MainWindow mw = (MainWindow)Application.Current.MainWindow;
        EncryptionManager EMgr = new EncryptionManager();

        public pageServerOneWayLoginKeyCreate()
        {
            InitializeComponent();
            msgBox_error.Visibility = Visibility.Hidden;
        }

        private void bttn_BackTo_Click(object sender, RoutedEventArgs e) => mw.pageMirror.Content = new pageServerLogin();

        private void bttn_create_Click(object sender, RoutedEventArgs e)
        {
            if (tb_serverIP.Text == "")
            {
                msgBox_error.Text = ("The server-IP cannot be empty!");
                msgBox_error.Visibility = Visibility.Visible;
                return;
            }
            else if (tb_serverDatabase.Text == "")
            {
                msgBox_error.Text = ("The database-Name cannot be empty!");
                msgBox_error.Visibility = Visibility.Visible;
                return;
            }
            else if (tb_serverUsername.Text == "")
            {
                msgBox_error.Text = ("The username cannot be empty!");
                msgBox_error.Visibility = Visibility.Visible;
                return;
            }
            else if (tb_serverPassword.Password.Contains(":"))
            {
                msgBox_error.Text = ("The password cannot contain a   ':'   !");
                msgBox_error.Visibility = Visibility.Visible;
                return;
            }
            byte[] salt = EMgr.GetNewSalt();
            string encryptedLoginData = EMgr.EncryptAES256Salt(($"{tb_serverIP.Text}:{tb_serverDatabase.Text}:{tb_serverUsername.Text}:{tb_serverPassword.Password}"), tb_loginPassoword.Password, salt);
            string[] _data = { SplitManager.SplitByteArrayIntoString(salt), encryptedLoginData };
            File.WriteAllLines("c2s_owl.gnm", _data);
            mw.pageMirror.Content = new pageServerOneWayLogin();
        }
    }
}
