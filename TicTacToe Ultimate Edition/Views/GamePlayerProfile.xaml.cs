using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Diagnostics;
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
    /// Interaction logic for GamePlayerProfile.xaml
    /// </summary>
    public partial class GamePlayerProfile : UserControl
    {
		GameFlow gf;
        Database db;
        Player currentPlayer;

        public GamePlayerProfile(GameFlow gf, Database db)
        {
            InitializeComponent();
			this.gf = gf;
            this.db = db;

            initialize();
        }

        private void initialize()
        {
            //cmbPlayerProfile.Items.Clear();
            cmbPlayerProfile.ItemsSource = db.getUsers();

            imgAvatar01.MouseEnter += Image_MouseEnter;
            imgAvatar02.MouseEnter += Image_MouseEnter;
            imgAvatar03.MouseEnter += Image_MouseEnter;
            imgAvatar04.MouseEnter += Image_MouseEnter;
            imgAvatar05.MouseEnter += Image_MouseEnter;
        }

        private void btnBackFromProfile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			gf.changeState(STATES.Menu);
        }

        private void cmbPlayerProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentPlayer = db.getUser((string)cmbPlayerProfile.SelectedValue);

            imgPlayer.Source = new BitmapImage(new Uri(currentPlayer.Avatar)); 
            //lblCurrentPlayerName.Content = currentPlayer.Name;
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            
            imgPlayer.Source = ((Image)sender).Source;
            
            if(currentPlayer != null)
                currentPlayer.Avatar = ((Image)sender).Source.ToString();
        }

        private void btnSaveChange_Click(object sender, RoutedEventArgs e)
        {
            db.updateAvatar(currentPlayer);
            initialize();
        }

        private void ShowImages(object sender, System.Windows.Input.MouseEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			
        }
    }
}
