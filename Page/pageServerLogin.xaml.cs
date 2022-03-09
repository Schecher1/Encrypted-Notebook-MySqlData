using System.Windows;
using System.IO;
using Encrypted_Notebook.Class;

namespace Encrypted_Notebook.Page
{
    public partial class pageServerLogin
    {
        MainWindow mw = (MainWindow)Application.Current.MainWindow;
        DatabaseManager DBMgr = new DatabaseManager();
        EncryptionManager EMgr = new EncryptionManager();

        public pageServerLogin()
        {
            InitializeComponent();
            msgBox_error.Visibility = Visibility.Hidden;

            if (File.Exists("c2s_owl.gnm"))
                mw.pageMirror.Content = new Page.pageServerOneWayLogin();

            if (File.Exists("c2s.gnm"))
            {
                string[] data = File.ReadAllLines("c2s.gnm");
                tb_serverIP.Text = data[0];
                tb_serverDatabase.Text = data[1];
                tb_serverUsername.Text = data[2];
            }
        }

        private void bttn_login_Click(object sender, RoutedEventArgs e)
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

            DBMgr.connectionString(tb_serverIP.Text, tb_serverDatabase.Text, tb_serverUsername.Text,tb_serverPassword.Password);
           
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

        private void bttn_save_Click(object sender, RoutedEventArgs e)
        {
            string[] data =
            {
                tb_serverIP.Text,
                tb_serverDatabase.Text,
                tb_serverUsername.Text
            };
            File.WriteAllLines("c2s.gnm", data);
        }

        private void bttn_owl_Click(object sender, RoutedEventArgs e) => mw.pageMirror.Content = new pageServerOneWayLoginKeyCreate();
    }
}
