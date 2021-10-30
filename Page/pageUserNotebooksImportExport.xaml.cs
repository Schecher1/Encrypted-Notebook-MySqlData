using Encrypted_Notebook.Class;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

namespace Encrypted_Notebook.Page
{
    public partial class pageUserNotebooksImportExport
    {
        MainWindow mw = (MainWindow)System.Windows.Application.Current.MainWindow;
        DatabaseManager DBMgr = new DatabaseManager();
        ImportExportManager IEMgr = new ImportExportManager();
        FolderBrowserDialog fbd = new FolderBrowserDialog();
        OpenFileDialog ofd = new OpenFileDialog();

        public pageUserNotebooksImportExport()
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

        private void bttn_BackTo_Click(object sender, RoutedEventArgs e) => mw.pageMirror.Content = new pageUserHome();

        private void bttn_export_Click(object sender, RoutedEventArgs e)
        {
            if (tb_password.Password != "" || tb_password.Password == null)
            {
                if (cb_all.IsChecked == true)
                {
                    IEMgr.ExportAll(tb_password.Password, tb_pathExport.Text);
                    finish();
                }  
                else if (cb_custom.IsChecked == true)
                    if (lb_notebooks.SelectedIndex != -1)
                    {
                        List<string> listOfNotebooks = new List<string>();
                        foreach (var notebook in lb_notebooks.SelectedItems)
                            listOfNotebooks.Add(notebook.ToString());
                        IEMgr.ExportCustom(tb_password.Password, tb_pathExport.Text, listOfNotebooks);
                        finish();
                    }
            }
            else
                System.Windows.MessageBox.Show("The password must not be empty or invalid!");
        }

        private void bttn_import_Click(object sender, RoutedEventArgs e)
        {
            if (tb_password.Password != "" || tb_password.Password == null)
            {
                if (tb_password.Password != "" || tb_password.Password == null)
                {
                    IEMgr.ImportAll(tb_password.Password, tb_pathImport.Text);
                    finish();
                }
                else
                    System.Windows.MessageBox.Show("The password must not be empty or invalid!");
            }
            else
                System.Windows.MessageBox.Show("The password must not be empty or invalid!");
        }

        private void bttn_selectPathExport_Click(object sender, RoutedEventArgs e)
        {
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tb_pathExport.Text = fbd.SelectedPath;
            }
        }

        private void bttn_selectPathImport_Click(object sender, RoutedEventArgs e)
        {
            ofd.Title = "Select your export file!";
            ofd.Filter = "TxT| *.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tb_pathImport.Text = ofd.FileName;
            }
        }

        private void finish()
        {
            tb_password.Password = "";
            tb_pathExport.Text = "";
            tb_pathImport.Text = "";
            lb_notebooks.SelectedIndex = -1;
            LoadNotebooks();
        }
    }
}
