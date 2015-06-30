using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TicTacToe_Ultimate_Edition.Libraries;

namespace TicTacToe_Ultimate_Edition.Views
{
    /// <summary>
    /// Interaction logic for GameGridView.xaml
    /// </summary>
    public partial class GameGridView : UserControl
    {
        GameFlow gf;
        AI ai;
        Database db;
        Player player1;
        Player player2;
        bool[] p1Moves;
        bool[] p2Moves;
        int[] winningButtons;
        Button[] boardButtons;
        int gameCounter;
        bool isActive;
        
		
		

        public GameGridView(GameFlow gf, Player player1, Player player2)
        {
            InitializeComponent();

            this.gf = gf;
            this.player1 = player1;
            this.player2 = player2;
            p1Moves = new bool[25];
            p2Moves = new bool[25];
            winningButtons = new int[3];
            db = gf.Db;

            initializeGame();
            initializePlayers();

            Debug.WriteLine("We are playing: " + GameFlow.Diff);
        }

        private void initializePlayers()
        {
            if(!gf.IsMulti)
                player1.setDifficulty(GameFlow.Diff);
            else
            {
                player1.setDifficulty(DIFFICULTY.Easy);
                player2.setDifficulty(DIFFICULTY.Easy);
            }

            PlayerPanel1.imgAvatar.Source = new BitmapImage(new Uri(player1.Avatar));
            PlayerPanel1.lblPlayer.Content = player1.Name;
            PlayerPanel1.GameScoreProgress.Maximum = player1.MaxScore;
            PlayerPanel1.GameScoreProgress.Value = player1.getCurrentScore();
            PlayerPanel1.GameScoreProgress.ValueChanged += GameScoreProgress_ValueChanged;
            PlayerPanel1.lblPlayerScore.Content = player1.MaxScore;
            //PlayerPanel1.GameScoreProgress.Foreground = Brushes.Green;
            //PlayerPanel1.GameScoreProgress.Background = Brushes.White;
            PlayerPanel2.imgAvatar.Source = new BitmapImage(new Uri(player2.Avatar));
            PlayerPanel2.lblPlayer.Content = player2.Name;
            PlayerPanel2.GameScoreProgress.Maximum = player2.MaxScore;
            PlayerPanel2.GameScoreProgress.Value = player2.getCurrentScore();
            PlayerPanel2.GameScoreProgress.ValueChanged += GameScoreProgress_ValueChanged;
            PlayerPanel2.lblPlayerScore.Content = player2.MaxScore;
            PlayerPanel2.lblGameLevel.Content = GameFlow.Diff;
           //PlayerPanel2.GameScoreProgress.Foreground = Brushes.Green;
			//PlayerPanel2.GameScoreProgress.Background = Brushes.White;
			

            if(!gf.IsMulti)
            {
                PlayerPanel2.GameScoreProgress.Visibility = Visibility.Hidden;
                PlayerPanel2.lblPlayerScore.Visibility = Visibility.Hidden;
                PlayerPanel2.lblGameLevel.Visibility = Visibility.Hidden;
                PlayerPanel2.lblGameLevel.Visibility = Visibility.Visible;

                if(gameCounter % 2 == 1)
                    ai.nextMove();
            }
            else
                PlayerPanel2.lblGameLevel.Visibility = Visibility.Hidden;

            
            Debug.WriteLine(PlayerPanel1.imgAvatar.Source + "\n" + PlayerPanel2.imgAvatar.Source);
            Debug.WriteLine(PlayerPanel1.GameScoreProgress.Value + " / " + PlayerPanel1.GameScoreProgress.Maximum);
        }

        private void GameScoreProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender.Equals(PlayerPanel1.GameScoreProgress))
			{
                PlayerPanel1.lblPlayerScore.Content = e.NewValue;
			}
            else
			{
                PlayerPanel2.lblPlayerScore.Content = player2.getCurrentScore();// (player2.MaxScore - e.NewValue);
			}
        }

        private void initializeGame()
        {
            gridContainer.Children.Clear();
            isActive = true;
            gameCounter = -1;

            if (!GameFlow.P1First)
                gameCounter++;

            updatePlayerTurn();

            Button[] btn = new Button[25];

            for (int x = 0; x < 25; x++)
            {
                btn[x] = new Button();

                btn[x].Width = gridContainer.Width / 5;
                btn[x].Height = gridContainer.Height / 5;
                btn[x].Uid = x + "";
                btn[x].Click += btn_Click;
                btn[x].Style = FindResource("GridButtonStyle") as Style;

                //double top = 2.0, left = 2.0, right = 2.0, bottom = 2.0;

                //if (x < 5)
                //    top = 0;
                //else if (x > 19)
                //    bottom = 0;

                //if ((x + 1) % 5 == 0)
                //    right = 0;
                //else if (x % 5 == 0)
                //    left = 0;

                //btn[x].BorderBrush = Brushes.Black;
                //btn[x].BorderThickness = new Thickness(left, top, right, bottom);
                //btn[x].Background = Brushes.Transparent;
                //btn[x].FontSize = 20;

                gridContainer.Children.Add(btn[x]);
                p1Moves[x] = false;
                p2Moves[x] = false;
            }

            ai = new AI(btn, GameFlow.Diff);
            boardButtons = btn;
        }

        private void updatePlayerTurn()
        {
            Label curPlayer, oldPlayer;
			Storyboard sb = PlayerPanel1.FindResource("Update_Turn") as Storyboard;
			Storyboard sb2 = PlayerPanel2.FindResource("UpdatePlayer2") as Storyboard;
            Storyboard sb8 = container.FindResource("Thinking") as Storyboard;
          

            //Total number of moves taken
            gameCounter++;
            
            
            if (gameCounter % 2 == 0)
            {
                curPlayer = PlayerPanel1.lblPlayer;
                oldPlayer = PlayerPanel2.lblPlayer;
				sb.Begin();
                sb2.Stop();
                if(!gf.IsMulti)
                sb8.Stop();
               if(gameCounter != 0)
                   player2.changeCurrentScore();
            }
            else
            {
				sb.Stop();
				sb2.Begin();
                if (!gf.IsMulti)
                sb8.Begin();
                curPlayer = PlayerPanel2.lblPlayer;
                oldPlayer = PlayerPanel1.lblPlayer;

                player1.changeCurrentScore();
			
            }

            curPlayer.FontWeight = FontWeights.Bold;
            oldPlayer.FontWeight = FontWeights.Normal;
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Point p1 = new Point();
            Point p2 = new Point();
            Line myLine = null;
			
		
			Storyboard sb3 = container.FindResource("Player1_Win") as Storyboard;
            Storyboard sb4 = container.FindResource("Player2Wins") as Storyboard;
            Storyboard sb5 = container.FindResource("Player1Lose") as Storyboard;
            Storyboard sb6 = container.FindResource("Player2Lose") as Storyboard;
            Storyboard sb8 = container.FindResource("Thinking") as Storyboard;
           
            Storyboard sb7 = container.FindResource("GameDraw") as Storyboard;
			
            if (isActive)
            {
				
				
                if (gameCounter % 2 == 0)
                {
                    btn.Content = "X";
                    p1Moves[Convert.ToInt16(btn.Uid)] = true;
				
                    if (hasWon())
                    {
						sb3.Begin();
                        sb6.Begin();
						GridPlayer1Win.imgAvatar.Source = new BitmapImage(new Uri(player1.Avatar));
						GridPlayer1Win.callout.Visibility = Visibility.Visible;
						GridPLose2.imgAvatar.Source = new BitmapImage(new Uri(player2.Avatar));
						GridPLose2.callout.Visibility = Visibility.Visible;
                        string score = player1.Score + player1.changeCurrentScore() + "";
                        string newScore = player1.getCurrentScore() + "";

                        db.updateWinLoss(player1.Name, "Win", player1.getCurrentScore());
                        db.updateWinLoss(player2.Name, "Loss");

                        Debug.WriteLine(winningButtons[1] + " " + winningButtons[0] + " winning buttons!");
                        Debug.WriteLine(PlayerPanel1.lblPlayer.Content + " HAS WON!");
                        Debug.WriteLine(player1.Name + " new score is: " + score + "\nPoints this game: " + newScore);
                        
                        isActive = false;
                    }
                    else
                    {	
                        updatePlayerTurn();
						
                        if (!gf.IsMulti)
                            ai.nextMove(Convert.ToInt16(btn.Uid));
                    }

                    PlayerPanel1.GameScoreProgress.Value = player1.getCurrentScore();
                    Debug.WriteLine(PlayerPanel1.GameScoreProgress.Value + " : " + player1.getCurrentScore());
                }
                else
                {
                   	btn.Content = "O";
                    p2Moves[Convert.ToInt16(btn.Uid)] = true;
					
					
					
                    if (hasWon())
                    {
						sb4.Begin();
                        sb5.Begin();
                        sb8.Stop();
						GridPlayer2Win.imgAvatar.Source = new BitmapImage(new Uri(player2.Avatar));
						GridPlayer2Win.callout.Visibility = Visibility.Visible;
						GridPlayer1Lose.imgAvatar.Source = new BitmapImage(new Uri(player1.Avatar));
						GridPlayer1Lose.callout.Visibility = Visibility.Visible;
                        string score = player2.Score + player2.changeCurrentScore() + "";
                        string newScore = player2.getCurrentScore() + "";

                        db.updateWinLoss(player2.Name, "Win", player2.getCurrentScore());
                        db.updateWinLoss(player1.Name, "Loss");
						
						 isActive = false;

                        Debug.WriteLine(PlayerPanel2.lblPlayer.Content + " HAS WON!");
                        Debug.WriteLine(player2.Name + " new score is: " + score + "\nPoints this game: " + newScore);
                    }
                    else
					{
                        updatePlayerTurn();
						
					if(gf.IsMulti)
						PlayerPanel2.GameScoreProgress.Value = player2.getCurrentScore();
					}
				}

                if (!isActive)
                {

                    Debug.WriteLine("Win at {0} and {1}", winningButtons[0], winningButtons[1]);

                    if (winningButtons[1] - winningButtons[0] == 1) //horizontal
                    {
                        p1 = boardButtons[winningButtons[0]].TransformToAncestor(container).Transform(new Point(0, 0));
                        p2 = boardButtons[winningButtons[1]].TransformToAncestor(container).Transform(new Point(0, 0));

                        myLine = new Line();
                        myLine.Stroke = System.Windows.Media.Brushes.Red;
                        myLine.StrokeThickness = 3;
                        myLine.X1 = p1.X;
                        myLine.X2 = p1.X + 4 * boardButtons[0].Width;
                        myLine.Y1 = p1.Y + boardButtons[0].Height * .5;
                        myLine.Y2 = p1.Y + boardButtons[0].Height * .5;
                        drawLine(myLine);
                    }
                    else if (winningButtons[1] - winningButtons[0] == 5) //vertical
                    {
                        p1 = boardButtons[winningButtons[0]].TransformToAncestor(container).Transform(new Point(0, 0));
                        p2 = boardButtons[winningButtons[1]].TransformToAncestor(container).Transform(new Point(0, 0));

                        myLine = new Line();
                        myLine.Stroke = System.Windows.Media.Brushes.Red;
                        myLine.StrokeThickness = 3;
                        myLine.X1 = p1.X + boardButtons[0].Width * .5;
                        myLine.X2 = p2.X + boardButtons[0].Width * .5;
                        myLine.Y1 = p1.Y;
                        myLine.Y2 = p1.Y + 4 * boardButtons[0].Height;
                        drawLine(myLine);
                    }
                    else if (winningButtons[1] - winningButtons[0] == 4) //diagonal left
                    {
                        p1 = boardButtons[winningButtons[0]].TransformToAncestor(container).Transform(new Point(0, 0));
                        p2 = boardButtons[winningButtons[1]].TransformToAncestor(container).Transform(new Point(0, 0));

                        myLine = new Line();
                        myLine.Stroke = System.Windows.Media.Brushes.Red;
                        myLine.StrokeThickness = 3;
                        myLine.X1 = p1.X + boardButtons[0].Width;
                        myLine.X2 = p1.X - boardButtons[0].Width * 3;
                        myLine.Y1 = p1.Y;
                        myLine.Y2 = p1.Y + boardButtons[0].Height * 4;
                        drawLine(myLine);
                    }
                    else // diagonal right
                    {
                        p1 = boardButtons[winningButtons[0]].TransformToAncestor(container).Transform(new Point(0, 0));
                        p2 = boardButtons[winningButtons[1]].TransformToAncestor(container).Transform(new Point(0, 0));

                        myLine = new Line();
                        myLine.Stroke = System.Windows.Media.Brushes.Red;
                        myLine.StrokeThickness = 3;
                        myLine.X1 = p1.X;
                        myLine.X2 = p1.X + boardButtons[0].Width * 4;
                        myLine.Y1 = p1.Y;
                        myLine.Y2 = p1.Y + boardButtons[0].Height * 4;
                        drawLine(myLine);
                    }
                }
                else 
                {
                    if ((gameCounter == 24 && GameFlow.P1First) || gameCounter == 25)
                    {
                        sb7.Begin();
                        Debug.WriteLine("DRAW");
                    }
                        
                
					
                }

                //Disables the click event for THIS button
                btn.Click -= btn_Click;
            }
        }

        private void drawLine(Line winningLine)
        {
            container.Children.Add(winningLine);
        }

        private bool hasWon()
        {
            bool[] winning;

            if (gameCounter % 2 == 0)
                winning = p1Moves;
            else
                winning = p2Moves;

            //check horizontal wins
            for (int y = 0; y < 5; y++ )
            {
                bool case1 = false, case2 = false;
                
                for(int x = 0; x < 3; x++)
                {
                    int a = y * 5 + x, b = a + 1;
                    
                    if (!(x != 0 && case1 == false))
                        case1 = winning[a] && winning[b];

                    if (!(x != 0 && case2 == false))  
                       case2 = winning[b] && winning[b+1];

                    if (case1 == false && case2 == false)
                        break;

                    if (case1)
                        winningButtons[x] = a;
                    else
                        winningButtons[x] = b;
                }

                if (case1 == true || case2 == true)
                    return true;

                case1 = false;
                case2 = false;

                //check vertical wins
                for (int x = 0; x < 3; x++)
                {
                    int a = 5 * x + y, b = 5 * (x + 1) + y;

                    if (!(x != 0 && case1 == false))
                        case1 = winning[a] && winning[b];

                    if (!(x != 0 && case2 == false))
                        case2 = winning[b] && winning[b + 5];

                    if (case1 == false && case2 == false)
                        break;

                    if (case1)
                        winningButtons[x] = a;
                    else
                        winningButtons[x] = b;
                }
                
                if (case1 == true || case2 == true)
                    return true;

                if (y < 2)
                {
                    case1 = false;
                    case2 = false;
               
                    //check diagonal right wins
                    for (int x = 0; x < 3; x++)
                    {
                        int a = 6 * x + 5 * y, b = a + 1;

                        if (!(x != 0 && case1 == false))
                            case1 = winning[a] && winning[a + 6];

                        if (!(x != 0 && case2 == false))
                            case2 = winning[b] && winning[b + 6];

                        if (case1 == false && case2 == false)
                            break;

                        if (case1)
                            winningButtons[x] = a;
                        else
                            winningButtons[x] = b;
                    }

                    if (case1 == true || case2 == true)
                        return true;

                    case1 = false;
                    case2 = false;

                
                    //check diagonal left wins
                    for (int x = 0; x < 3; x++)
                    {
                        int a = 4 * (x+1) + 5 * y, b = a - 1;

                        if (!(x != 0 && case1 == false))
                            case1 = winning[a] && winning[a + 4];

                        if (!(x != 0 && case2 == false))
                            case2 = winning[b] && winning[b + 4];

                        if (case1 == false && case2 == false)
                            break;

                        if (case1)
                            winningButtons[x] = a;
                        else
                            winningButtons[x] = b;
                    }
                }

                if (case1 == true || case2 == true)
                    return true;
            }

            return false;
        }


        private void btnBackMainMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
			gf.changeState(STATES.Menu);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gf.Player1.newGame();
            gf.Player2.newGame();
            gf.changeState(STATES.Game);
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            gf.changeState(STATES.Menu);
        }
        
    }
}
