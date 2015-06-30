using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;


namespace TicTacToe_Ultimate_Edition.Views
{
    /// <summary>
    /// Interaction logic for GameMenu.xaml
    /// </summary>
    public partial class GameMenu : UserControl
    {
        GameFlow gf;
        public GameMenu(GameFlow gf)
        {
            InitializeComponent();
            this.gf = gf;
            userLogin.setGf(gf);
        }

        private void SinglePlayerClick(object sender, RoutedEventArgs e)
        {
           gf.changeState(STATES.Login);
        }

        private void btnRankings_Click(object sender, RoutedEventArgs e)
        {
            gf.changeState(STATES.Ranking);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            gf.changeState(STATES.Settings);
        }

        private void btnReloadDb_Click(object sender, RoutedEventArgs e)
        {
            gf.Db.refreshDb();
            MessageBox.Show("Database File has been emptied!");
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnSinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            Storyboard login = FindResource("UserLogin") as Storyboard;
            gf.IsMulti = false;
            userLogin.updatePlayer2();
            login.Begin();
        }

        private void btnMultiPlayer_Click(object sender, RoutedEventArgs e)
        {
            Storyboard login = FindResource("UserLogin") as Storyboard;
            gf.IsMulti = true;
            userLogin.updatePlayer2();
            login.Begin();
        }

        private void btnTutorial_Click(object sender, RoutedEventArgs e)
        {
            gf.changeState(STATES.Tutorial);
        }

        private void btnPlayerProfile_Click(object sender, RoutedEventArgs e)
        {
        	 gf.changeState(STATES.Profile);
        }
    }
}
