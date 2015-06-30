using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace TicTacToe_Ultimate_Edition.Views
{
    /// <summary>
    /// Interaction logic for UserLogin.xaml
    /// </summary>
    public partial class UserLogin : UserControl
    {
        private GameFlow gf;
        private Database db;

        public UserLogin()
        {
            InitializeComponent();
            rdaPlayer1.Checked += rdPlayer_Checked;
            rdaPlayer2.Checked += rdPlayer_Checked;
        }

        private void updateCmbBox()
        {
            cmbPlayer1.Items.Clear();
            cmbPlayer2.Items.Clear();

            cmbPlayer1.Items.Add("Guest");
            cmbPlayer1.SelectedIndex = 0;
            cmbPlayer2.SelectedIndex = 0;

            string[] names = db.getUsers();

            for (int x = 0; x < names.Length; x++)
                cmbPlayer1.Items.Add(names[x]); 
        }

        public void updatePlayer2()
        {
            Debug.WriteLine("This is a multiplayer game: " + gf.IsMulti);

            if (gf.IsMulti)
            {
                cmbPlayer2.ItemsSource = cmbPlayer1.Items;
                cmbPlayer2.IsEnabled = true;
            }
            else
            {
                lblPlayer2.Content = "T3P0";
                cmbPlayer2.Items.Clear();
                cmbPlayer2.Items.Add("Easy");
                cmbPlayer2.Items.Add("Medium");
                cmbPlayer2.Items.Add("Hard");
                cmbPlayer2.IsEditable = false;
                cmbPlayer2.IsEnabled = true;
            }
        }

        public void setGf(GameFlow gf)
        {
            this.gf = gf;
            db = gf.Db;
            updateCmbBox();

            if (GameFlow.P1First)
                rdaPlayer1.IsChecked = GameFlow.P1First;
            else
                rdaPlayer2.IsChecked = !GameFlow.P1First;
        }

        private void btnOKLogin_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPlayer1.SelectedIndex != cmbPlayer2.SelectedIndex || 
                cmbPlayer1.SelectedIndex == 0 && cmbPlayer2.SelectedIndex == 0 ||
                cmbPlayer1.SelectedIndex == -1 && cmbPlayer2.SelectedIndex == -1)
            {
                Player p1, p2;

                if (cmbPlayer1.SelectedItem != null)
                    p1 = db.getUser((string)cmbPlayer1.SelectedValue);
                else
                    p1 = db.newUser(cmbPlayer1.Text);

                if (gf.IsMulti)
                {
                    if (cmbPlayer2.SelectedItem != null)
                        p2 = db.getUser((string)cmbPlayer2.SelectedValue);
                    else
                        p2 = db.newUser(cmbPlayer2.Text);
                }
                else
                {
                    string value = (string)cmbPlayer2.SelectedValue;
                    
                    p2 = new Player("pack://application:,,,/TicTacToe Ultimate Edition;component/Resources/Images/Avatars/Avatar06.png", "T3PO", 0);
                    GameFlow.Diff = value == "Easy" ? DIFFICULTY.Easy : value == "Medium" ? DIFFICULTY.Medium : DIFFICULTY.Hard; 
                }

                gf.Player1 = p1;
                gf.Player2 = p2;

                if(p1 != null && p2 != null)
                    gf.changeState(STATES.Game);
            }
            else
                lblError.Content = "Please select 2 distinct names!";
        }

        private void btnCancelLogin_Click(object sender, RoutedEventArgs e)
        {
            gf.changeState(STATES.Menu);
        }

        private void rdPlayer_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == rdaPlayer1)
                GameFlow.P1First = true;
            else
                GameFlow.P1First = false;
        }
    }
}
