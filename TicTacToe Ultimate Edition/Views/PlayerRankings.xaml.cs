using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using TicTacToe_Ultimate_Edition.Libraries;

namespace TicTacToe_Ultimate_Edition.Views
{
    /// <summary>
    /// Interaction logic for PlayerRankings.xaml
    /// </summary>
    public partial class PlayerRankings : UserControl
    {
        Database db;
        GameFlow gf;

        public PlayerRankings(GameFlow gf)
        {
            InitializeComponent();

            db = gf.Db;
            Player[] users = db.getUserRanking();
            this.gf = gf;
            int rank = 1;

            foreach (Player player in users)
            {
                PlayerPanel pp = new PlayerPanel();

                pp.lblPalyerName.Content = player.Name;
                pp.lblPlayerLoss.Content = player.Loss;
                pp.lblPlayerRank.Content = rank++;
                pp.lblPlayerScore.Content = player.Score;
                pp.lblPlayerWins.Content = player.Win;
                pp.imgAvatar.Source = new BitmapImage(new Uri(player.Avatar));
                //imgPlayer.Source = new BitmapImage(new Uri(currentPlayer.Avatar)); 
                //ltvRankings.Items.Add(pp);

                Debug.WriteLine(player.Avatar);

                lstRanking.Items.Add(pp);
            }
        }
		 private void btnBackMainMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	gf.changeState(STATES.Menu);
        }
    }
}
