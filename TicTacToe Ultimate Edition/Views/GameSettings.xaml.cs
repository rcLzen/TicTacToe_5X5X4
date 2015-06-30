using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace TicTacToe_Ultimate_Edition
{
    /// </summary>
    /// This section supports the Setting tab of the game. 
    /// The functions supported are music and sound
    /// the back button is shared like any other tab.
    /// </summary>
    public partial class GameSettings : UserControl
    {
        GameFlow gf;
        string[] music;

        public GameSettings(GameFlow gf)
        {
            this.InitializeComponent();
            this.gf = gf;

            initializeMusic();
            SdrMusic.Value = gf.Media.Volume;

            Debug.WriteLine("***GameFlow p1 fisrt: " + GameFlow.P1First);

            if (GameFlow.P1First)
                rdPlayerSettings.IsChecked = GameFlow.P1First;
            else
                rdPlayer2Settings.IsChecked = !GameFlow.P1First;

            rdPlayerSettings.Checked += rdPlayerSettings_Checked;
            rdPlayer2Settings.Checked += rdPlayerSettings_Checked;
            cmbMusic.SelectionChanged += cmbMusic_SelectionChanged;
        }

        private void initializeMusic()
        {
            music = Directory.GetFiles(@"Resources\Media\");

            Debug.WriteLine(gf.Media.Source.ToString());

            foreach (string file in music)
            {
                cmbMusic.Items.Add(Path.GetFileNameWithoutExtension(file));

                if (file.Equals(gf.Media.Source.ToString()))
                    cmbMusic.SelectedIndex = cmbMusic.Items.Count - 1;
            }
        }


        private void btnMuteMusic_Click(object sender, RoutedEventArgs e)
        {
            SdrMusic.IsEnabled = !SdrMusic.IsEnabled;

            if (!SdrMusic.IsEnabled)
                gf.Media.Volume = 0;
            else
                gf.Media.Volume = SdrMusic.Value;
        }

        private void SdrMusic_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            gf.Media.Volume = SdrMusic.Value;
        }

        private void btnBackMainMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            gf.changeState(STATES.Menu);
        }

        private void rdPlayerSettings_Checked(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Sender is  player1 " + (sender == rdPlayerSettings));

            if (sender == rdPlayerSettings)
                GameFlow.P1First = true;
            else
                GameFlow.P1First = false;

            Debug.WriteLine("GameFlow p1 first = " + GameFlow.P1First);
        }

        private void cmbMusic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int pos = ((ComboBox)sender).SelectedIndex;

            gf.changeMusic(music[pos]);

            Debug.WriteLine(pos);
        }
    }
}