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

using TicTacToe_Ultimate_Edition;

namespace TicTacToe_Ultimate_Edition.Views
{
    /// <summary>
    /// Interaction logic for GameLevel.xaml
    /// </summary>
    public partial class GameLevel : UserControl
    {
        GameFlow gf;
        public GameLevel(GameFlow gf)
        {
            InitializeComponent();

            this.gf = gf;
        }

        private void btnEasyLevel_Click(object sender, RoutedEventArgs e)
        {
            gf.changeState(STATES.Game);
        }
    }
}
