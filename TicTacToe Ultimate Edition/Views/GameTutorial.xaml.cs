using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToe_Ultimate_Edition
{
	/// <summary>
	/// Interaction logic for Tutorial.xaml
	/// </summary>
	public partial class GameTutorial : UserControl
	{
		GameFlow gf;
		public GameTutorial(GameFlow gf)
		{
			InitializeComponent();
			this.gf = gf;
		}
	
	private void btnBackMainMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	gf.changeState(STATES.Menu);
        }

    private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

	
	}
}