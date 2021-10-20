using Encrypted_Notebook.Class;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Encrypted_Notebook.Page
{
    public partial class pageNotebook
    {
        MainWindow mw = (MainWindow)Application.Current.MainWindow;
        DatabaseManager DBMgr = new DatabaseManager();

        public pageNotebook()
        {
            InitializeComponent();
            LoadNotebooks();
        }

        private void LoadNotebooks()
        {
            lb_notebooks.Items.Clear();

            List<string> Notebooks = new List<string>();
            Notebooks = DBMgr.loadNotebooks();

            foreach (var notebook in Notebooks)
                lb_notebooks.Items.Add(notebook);
        }

        private void lb_notebooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tb_notes.Text = "";
            gp_notes.Header = ("Notes from " + lb_notebooks.SelectedItem);
            if (lb_notebooks.SelectedIndex == -1)
            {
                UserInfoManager.userActivNotebook = null;
            }
            else
            {
                UserInfoManager.userActivNotebook = lb_notebooks.SelectedItem.ToString();
                tb_notes.Text = DBMgr.readNotes();
            }
        }

        private void bttn_notebookCreate_Click(object sender, RoutedEventArgs e)
        {
            if (tb_newNotebook.Text != "")
            {
                DBMgr.createNotebook(tb_newNotebook.Text);
                LoadNotebooks();
                tb_newNotebook.Text = "";
            }
        }

        private void bttn_notebookDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lb_notebooks.SelectedIndex != -1)
            {
                DBMgr.deleteNotebook(lb_notebooks.SelectedItem.ToString());
                LoadNotebooks();
            }
            else
                MessageBox.Show("You must have selected an Notebook in the list before you can delete it!");
        }

        private void bttn_notesSave_Click(object sender, RoutedEventArgs e)
        {
            DBMgr.writeNotes(tb_notes.Text);
        }

        private void bttn_logout_Click(object sender, RoutedEventArgs e)
        {
            UserInfoManager.userName = null;
            UserInfoManager.userActivNotebook = null;
            UserInfoManager.userID = -1;
            tb_notes.Text = "";
            lb_notebooks.Items.Clear();

            mw.pageMirror.Content = new pageLoginUser();
            System.GC.Collect();
        }
    }
}
