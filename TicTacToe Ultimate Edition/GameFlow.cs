using System;
using System.Windows.Controls;
using System.Windows.Media;
using TicTacToe_Ultimate_Edition.Libraries;
using TicTacToe_Ultimate_Edition.Views;


namespace TicTacToe_Ultimate_Edition
{
    public enum STATES                     // the different windows of the game
    {
        Menu,
        Mode,
        Level,
        Login,
        Game,
        Ranking,
        Settings,
		Tutorial,
		Profile
    };

    public enum DIFFICULTY              // difficulty levels
    {
        Easy,
        Medium,
        Hard
    };

    public class GameFlow
    {
        private MainWindow mainWindow;       //main window of the game initialized
        private Player player1, player2;     //player 1 and player 2 intialized
        private Database db;                  // database intitalized
        private bool isMulti;               //is the game played by multiple players condition
        private MediaPlayer media;          //media used for the game initialized
        private static bool p1First;        //condition which player goes first
        private static DIFFICULTY diff;     //difficulty level initialized

        public static DIFFICULTY Diff           //the difficulty level property 
        {
            get { return GameFlow.diff; }       //returns the value of difficulty level chosen
            set { GameFlow.diff = value; }
        }

        public static bool P1First          //who plays first property
        {
            get { return p1First; }
            set { p1First = value; }
        }

        public Database Db                  //database for the game
        { get { return db; } }
        public MediaPlayer Media            //the media property
        {
            get { return media; }
            set { media = value; }
        }

        //shows if the game is a multiplayer game
        public bool IsMulti
        {
            get { return isMulti; }
            set { isMulti = value; }
        }

        public Player Player2
        {
            get { return player2; }
            set { player2 = value; }
        }

        public Player Player1
        {
            get { return player1; }
            set { player1 = value; }
        }

        public GameFlow(MainWindow mWindow)     //the main window class
        {
            db = new Database();                //database intialized to new database
            media = new MediaPlayer();  
            mainWindow = mWindow;

            p1First = true;                     //condition who goes first set to 1
            player1 = null;
            player2 = null;
            changeState(STATES.Menu);           //when on the menu window

            media.MediaEnded += media_MediaEnded;
            media.MediaOpened += media_Opened;
            media.Open(new Uri(@"Resources\Media\Elevator.mp3", UriKind.Relative));
        }

        public void changeMusic(string file)
        {
            Uri path = new Uri(file, UriKind.Relative);

            if (path == media.Source)
                return;

            media.Close();
            media.Open(path);
        }

        void media_Opened(object sender, EventArgs e)
        {
            media.Play();
        }

        private void media_MediaEnded(object sender, EventArgs e)       //once the songends, it starts playing again
        {
            media.Position = new TimeSpan(0);
            media.Play();
        }
        

        //the different case scenarios when we move further in the game from main menu to new game, etc.
        public void changeState(STATES state)
        {
            UIElementCollection child = mainWindow.container.Children;
            child.Clear();

            switch (state)
            {
                case STATES.Menu: child.Add(new GameMenu(this));
                    break;
                //case STATES.Mode: child.Add(new GameMode(this));
                //    break;
               // case STATES.Level: child.Add(new GameLevel(this));
                   // break;
                //case STATES.Login: child.Add(new UserLogin(this, db));
                //    break;
                case STATES.Game: child.Add(new GameGridView(this, Player1, Player2));
                    break;
                case STATES.Ranking: child.Add(new PlayerRankings(this));
                    break;
                case STATES.Settings: child.Add(new GameSettings(this));  
                    break;
				case STATES.Tutorial: child.Add(new GameTutorial(this));  
                    break;
				case STATES.Profile: child.Add(new GamePlayerProfile(this, db));  
                    break;
            }
        }
    }
}
