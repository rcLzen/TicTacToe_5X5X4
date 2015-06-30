using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TicTacToe_Ultimate_Edition.Libraries;

namespace LoginTesting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Database db;
        public MainWindow()
        {
            InitializeComponent();
            

            db = new Database();

            updateCmbBox();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            db.refreshDb();
            updateCmbBox();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (cmbLogin.SelectedItem != null)
                lblName.Content = cmbLogin.SelectedValue;
            else
                lblName.Content = cmbLogin.Text;

            db.newUser((string)lblName.Content);

            cmbLogin.Text = "";
            updateCmbBox();
        }

        private void updateCmbBox()
        {
            cmbLogin.Items.Clear();

            string[] names = db.getUsers();

            for (int x = 0; x < names.Length; x++)
                cmbLogin.Items.Add(names[x]);
        }
    }
}
