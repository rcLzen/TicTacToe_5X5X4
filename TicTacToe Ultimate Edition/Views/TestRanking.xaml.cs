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

namespace TicTacToe_Ultimate_Edition.Views
{
    /// <summary>
    /// Interaction logic for TestRanking.xaml
    /// </summary>
    public partial class TestRanking : UserControl
    {
        Database db;
        GameFlow gf;

        public TestRanking(GameFlow gf)
        {
            InitializeComponent();
			db = gf.Db; 

            Player[] users = db.getUserRanking();
            this.gf = gf;

            foreach (Player player in users)
            {
                PlayerPanel pp = new PlayerPanel();
                pp.lblPalyerName.Content = player.Name;
                ltvRankings.Items.Add(pp);
            }
        }
		private void btnBackMainMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	gf.changeState(STATES.Menu);
        }
                    
    }
}
