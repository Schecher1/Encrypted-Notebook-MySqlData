using Encrypted_Notebook.Class;
using System.Windows;

namespace Encrypted_Notebook.Page{
    public partial class pageUserCreate{
        MainWindow mw = (MainWindow)Application.Current.MainWindow;
        DatabaseManager DBMgr = new DatabaseManager();

        public pageUserCreate(){
            InitializeComponent();
            msgBox_error.Visibility = Visibility.Hidden;
        }

        private void bttn_create_Click(object sender, RoutedEventArgs e){
            if (tb_username.Text == ""){
                msgBox_error.Text = ("The username cannot be empty");
                msgBox_error.Visibility = Visibility.Visible;
                return;
            }
            else if (tb_password.Password == ""){
                msgBox_error.Text = ("The password cannot be empty");
                msgBox_error.Visibility = Visibility.Visible;
                return;
            }
            else if (tb_password.Password != tb_passwordRepeat.Password){
                msgBox_error.Text = ("The passwords are not identical");
                msgBox_error.Visibility = Visibility.Visible;
                return;
            }
            else if (tb_password.Password.Length <= 8){
                msgBox_error.Text = ("Your password must be at least 8 characters long");
                msgBox_error.Visibility = Visibility.Visible;
                return;
            }

            if (DBMgr.checkIfUserExist(tb_username.Text.ToLower()) == 0){
                DBMgr.createUser(tb_username.Text, tb_password.Password);
                mw.pageMirror.Content = new pageUserLogin();
            }
            else{
                msgBox_error.Text = ("The username is already taken!");
                msgBox_error.Visibility = Visibility.Visible;
            }
        }

        private void bttn_BackTo_Click(object sender, RoutedEventArgs e) => mw.pageMirror.Content = new pageUserLogin();
    }
}