using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
namespace TicTacToe_Ultimate_Edition.Libraries
{
    public class AI
    {
        //easy mode computer move; random move
        Random random;
        DIFFICULTY diff;
        Button[] board;
        List<int> movesLeft;
        List<int> centerNine;
        Stopwatch sw;
        int depth;
        const int SLEEP = 1500;
        
        int[] bestMove;
        public AI(Button[] board, DIFFICULTY diff)
        {
            random = new Random();
            sw = new Stopwatch();
            this.diff = diff;
            this.board = board;
            movesLeft = new List<int>();
            
            bestMove = new int[2];
            bestMove[1] = -100000;

            for (int x = 0; x < 25; x++)
                movesLeft.Add(x);
        }

        private async void makeMove(int myMove)
        {
            Debug.WriteLine("Thread sleeps for {0} ms.", (SLEEP - sw.ElapsedMilliseconds));
            await Task.Delay((int) (SLEEP - sw.ElapsedMilliseconds));

            movesLeft.Remove(myMove);               //removes move from remaining move
            board[myMove].RaiseEvent(new RoutedEventArgs(Button.ClickEvent));     //makes move to the board
        }

        public void nextMove(int playerMove = -1)
        {
            sw.Reset();
            sw.Start();
           

            if(playerMove != -1)
                movesLeft.Remove(playerMove);
            
            switch(diff)
            {
                case DIFFICULTY.Easy: easyMove();
                    break;

                case DIFFICULTY.Medium: mediumMode();
                    break;
                case DIFFICULTY.Hard: hard();
                    break;
            }
            
        }
//*********************************   EASY    ********************************************************
        private void easyMove()
        {

            int countO = 0;                   //Count for "O"s
            int countX = 0;                     //Count for"X"s 
            int count = 0;                      //Overall Count
            int winMove = -1;                   //win move holder
            int blockMove = -1;                 // blocking move holder
            int Move2=-1;                       // 2 in a row move holder
            int Move1 = -1;                     // 1 in a row holder
            int myMove = -1;                    // move to be made
            int potentialMove=-1;               // potential moves in a row


                                                            //SEARCH FOR 3
                                                             //Horizontal search
            for (int row = 0; row <= 20; row += 5)            //start of each row
            {
                for (int shift = 0; shift <= 1; shift++)        //shift to start search at terminal combination
                {
                    countX = 0;                                     //set counts to 0
                    countO = 0;
                    count = 0;
                    
                    for (int position = row + shift; position <= row + shift + 3; position++)  //run through 4 positions
                    {
                        if (String.Equals(board[position].Content, "X"))            //add to count if position holds "X"
                        { 
                            countX++;
                            count++;
                        }
                        else if (String.Equals(board[position].Content, "O"))       //add to count if position hold "O"
                        { 
                            countO++;
                            count++;
                        }
                        else
                        { potentialMove = position; }                               // if position is null holds potential move

                    }
                    
                    if(countX==3 && count!=4)                                       //holds blocking move if 3 X's in a row
                    {
                        blockMove = potentialMove;
                    }

                    if (countO == 3 && count != 4)                                   //holds winning move if 3 O's in a row
                    {
                        winMove = potentialMove;
                    }

                    if(countO==2 && countX==0)                       //holds move if 2 in a row found
                    {
                        Move2=potentialMove;
                    }
                    if (countO == 1 && countX == 0)                   // holds move if 1 in a row found
                    {
                        Move1 = potentialMove;
                    }

                }
            }


                                                                    //Vertical Search
            for (int column = 0; column <= 4; column ++)           //start of each column
            {
                for (int shift = 0; shift <= 5; shift+=5)           //shift to start each search for terminal combinations
                {
                    countX = 0;                     //set counts to 0
                    countO = 0;
                    count = 0;

                    for (int position = column + shift; position <= column + shift + 15; position+=5)  //search is 4 column positions
                    {
                        if (String.Equals(board[position].Content, "X"))        //add to count if position holds X
                        {
                            countX++;
                            count++;
                        }
                        else if (String.Equals(board[position].Content, "O"))       //add to count if position holds O
                        {
                            countO++;
                            count++;
                        }
                        else
                        { potentialMove = position; }                           // holds potential move if position if empty
                    }

                    if (countX == 3 && count != 4)                              //hold blocking move
                    {
                        blockMove = potentialMove;
                    }

                    if (countO == 3 && count != 4)                              //hold winning move
                    {
                        winMove = potentialMove;
                    }
                    if (countO == 2 && countX == 0)                             //hold 2 in a row
                    {
                        Move2 = potentialMove;
                    }
                    if (countO == 1 && countX == 0)                             // hold 1 in a row
                    {
                        Move1 = potentialMove;
                    }

                }
            }

                                                                    //Diagonal search

            int[] LeftToRight = new int[] { 0, 1, 5, 6 };           //list starting positions
            int[] RightToLeft = new int[] { 3, 4, 8, 9 };

            foreach( int start in LeftToRight)                  //Left to right Diagonal Search
            {

                countX = 0;             //set count to 0
                countO = 0;
                count = 0;

                for(int position = start; position<=start+18; position+=6)  //searches 4 diagonally adjacent potitions
                {
                    if (String.Equals(board[position].Content, "X"))    // add to count if X
                    {
                        countX++;
                        count++;
                    }
                    else if (String.Equals(board[position].Content, "O"))       //add to count if O
                    {
                        countO++;
                        count++;
                    }
                    else
                    { potentialMove = position; }               // holds potential move
                }

                if (countX == 3 && count != 4)              //holds blocking move
                {
                    blockMove = potentialMove;
                }

                if (countO == 3 && count != 4)              // holds winning move
                {
                    winMove = potentialMove;
                }
                if (countO == 2 && countX == 0)             // holds 2 in a row
                {
                    Move2 = potentialMove;
                }
                if (countO == 1 && countX == 0)             // holds 1 in a row
                {
                    Move1 = potentialMove;
                }
            }


            foreach (int start in RightToLeft)                  //Right to Left Diagonal Search
            {

                countX = 0;                     // set count to 0
                countO = 0;
                count = 0;

                for (int position = start; position <= start+12; position += 4) //search 4 adjacent diagonal positions
                {
                    if (String.Equals(board[position].Content, "X"))    // add to count if X
                    {
                        countX++;
                        count++;
                    }
                    else if (String.Equals(board[position].Content, "O"))       //add to count if O
                    {
                        countO++;
                        count++;
                    }
                    else
                    { potentialMove = position; }                   //hold potential move
                }

                if (countX == 3 && count != 4)          //holds blocking move
                {
                    blockMove = potentialMove;
                }

                if (countO == 3 && count != 4)          //holds winning move
                {
                    winMove = potentialMove;
                }
                if (countO == 2 && countX == 0)         //holds 2 in a row
                {
                    Move2 = potentialMove;
                }
                if (countO == 1 && countX == 0)         // holds 1 in a row
                {
                    Move1 = potentialMove;
                }
            }

            if (movesLeft.Count > 0)                    //if there are move left to make
                if (winMove != -1)
                {
                    myMove = winMove;               // applys move if there is a winning move
                    Debug.WriteLine("Win at " + myMove);
                }
                else if (blockMove != -1)
                {
                    myMove = blockMove;             // applys move if there is a blocking move
                    Debug.WriteLine("Block at " + myMove);
                }
                else if(Move2!=-1)
                {
                    myMove = Move2;                 // applys move if there is 2 in a row
                    Debug.WriteLine("Move at " + myMove);
                }
                else if (Move1 != -1)
                {
                    myMove = Move1;                 // applys move if there is 1 in a row
                    Debug.WriteLine("Move at " + myMove);
                }
                else
                {                                   // makes random move
                    myMove = movesLeft.ElementAt(random.Next(0, movesLeft.Count));
                    Debug.WriteLine("Random move at " + myMove);
                }

            makeMove(myMove);
        }
        //private void easy()
        //{
        //    int myMove = -1;
        //    int winMove = -1;
        //    int blockMove = -1;

        //    //check horizontal wins
        //    for (int y = 0; y < 5; y++)
        //    {
        //        int xCount = 0,
        //            oCount = 0;
        //        Button aButton = null;
        //        Button bButton = null;
        //        bool useA = false;

        //        for (int x = 0; x < 3; x++)
        //        {
        //            int a = y * 5 + x, b = a + 2;
                    
        //            if (board[a].Content == null)
        //                aButton = board[a];
        //            if (board[b].Content == null && bButton == null)
        //                bButton = board[b];
                    
        //            if (String.Equals(board[a].Content, board[a + 1].Content))
        //                useA = true;

        //            if (String.Equals(board[a].Content, "X"))
        //                xCount++;
        //            else if (String.Equals(board[a].Content, "O"))
        //                oCount++;

        //            if(String.Equals(board[b].Content, "X") && x != 0)
        //                xCount++;
        //            else if (String.Equals(board[b].Content, "O") && x != 0)
        //                oCount++;
        //        }

        //        if (oCount > 2)
        //        {
        //            if (aButton != null && (useA || bButton == null))
        //                winMove = Convert.ToInt32(aButton.Uid);
        //            else if (bButton != null)
        //                winMove = Convert.ToInt32(bButton.Uid);
        //        }
        //        if (xCount > 2)
        //        {
        //            Debug.WriteLine("use a: " + useA);
        //            if (aButton != null && (useA || bButton == null))
        //                blockMove = Convert.ToInt32(aButton.Uid);
        //            else if(bButton != null)
        //                blockMove = Convert.ToInt32(bButton.Uid);
        //        }
        //        // end of horizontal check

        //        aButton = null;
        //        bButton = null;
        //        xCount = 0;
        //        oCount = 0;
        //        useA = false;

        //        //check vertical wins
        //        for (int x = 0; x < 3; x++)
        //        {
        //            int a = 5 * x + y, b = 5 * (x + 2) + y;

        //            if (board[a].Content == null)
        //                aButton = board[a];
        //            if (board[b].Content == null && bButton == null)
        //                bButton = board[b];

        //            if (String.Equals(board[a].Content, board[a + 5].Content))
        //                useA = true;

        //            if (String.Equals(board[a].Content, "X"))
        //                xCount++;
        //            else if (String.Equals(board[a].Content, "O"))
        //                oCount++;

        //            if (String.Equals(board[b].Content, "X") && x != 0)
        //                xCount++;
        //            else if (String.Equals(board[b].Content, "O") && x != 0)
        //                oCount++;
        //        }

        //        if (oCount > 2 && winMove == -1)
        //        {
        //            if (aButton != null)
        //                winMove = Convert.ToInt32(aButton.Uid);
        //            else if (bButton != null)
        //                winMove = Convert.ToInt32(bButton.Uid);
        //        }
        //        if (xCount > 2 && blockMove == -1)
        //        {
        //            if (aButton != null)
        //                blockMove = Convert.ToInt32(aButton.Uid);
        //            else if (bButton != null)
        //                blockMove = Convert.ToInt32(bButton.Uid);
        //        }

        //        if (y < 2)
        //        {
        //            aButton = null;
        //            bButton = null;
        //            xCount = 0;
        //            oCount = 0;

        //            //check diagonal right wins
        //            for (int x = 0; x < 3; x++)
        //            {
        //                int a = 6 * x + 5 * y;
        //                int b = 6 * (x + 1) + 5 * y;

        //                if (board[a].Content == null)
        //                    aButton = board[a];
        //                if (board[b].Content == null && bButton == null)
        //                    bButton = board[b];

        //                if (String.Equals(board[a].Content, "X"))
        //                    xCount++;
        //                else if (String.Equals(board[a].Content, "O"))
        //                    oCount++;

        //                if (String.Equals(board[b].Content, "X") && x != 0)
        //                    xCount++;
        //                else if (String.Equals(board[b].Content, "O") && x != 0)
        //                    oCount++;
        //            }

        //            if (oCount > 2 && winMove == -1)
        //            {
        //                if (aButton != null)
        //                    winMove = Convert.ToInt32(aButton.Uid);
        //                else if (bButton != null)
        //                    winMove = Convert.ToInt32(bButton.Uid);
        //            }
        //            else if (xCount > 2 && blockMove == -1)
        //            {
        //                if (aButton != null)
        //                    blockMove = Convert.ToInt32(aButton.Uid);
        //                else if (bButton != null)
        //                    blockMove = Convert.ToInt32(bButton.Uid);
        //            }

        //            aButton = null;
        //            bButton = null;
        //            xCount = 0;
        //            oCount = 0;

        //            for (int x = 0; x < 4; x++)
        //            {
        //                int a = 6 * x + 5 * y + 1;

        //                if (board[a].Content == null)
        //                    aButton = board[a];

        //                if (String.Equals(board[a].Content, "X"))
        //                    xCount++;
        //                else if (String.Equals(board[a].Content, "O"))
        //                    oCount++;
        //            }

        //            if (oCount > 2 && winMove == -1)
        //                if (aButton != null)
        //                    winMove = Convert.ToInt32(aButton.Uid);
        //            if (xCount > 2 && blockMove == -1)
        //                if (aButton != null)
        //                    blockMove = Convert.ToInt32(aButton.Uid);

        //            aButton = null;
        //            bButton = null;
        //            xCount = 0;
        //            oCount = 0;

        //            //check diagonal left wins
        //            for (int x = 0; x < 3; x++)
        //            {
        //                int a = 4 * (x + 1) + 5 * y, b = 4 * (x + 2) + 5 * y;

        //                if (board[a].Content == null)
        //                    aButton = board[a];
        //                if (board[b].Content == null && bButton == null)
        //                    bButton = board[b];

        //                if (String.Equals(board[a].Content, "X"))
        //                    xCount++;
        //                else if (String.Equals(board[a].Content, "O"))
        //                    oCount++;

        //                if (String.Equals(board[b].Content, "X") && x != 0)
        //                    xCount++;
        //                else if (String.Equals(board[b].Content, "O") && x != 0)
        //                    oCount++;
        //            }

        //            if (oCount > 2 && winMove == -1)
        //                if (aButton != null)
        //                    winMove = Convert.ToInt32(aButton.Uid);
        //                else if (bButton != null)
        //                    winMove = Convert.ToInt32(bButton.Uid);
        //            if (xCount > 2 && blockMove == -1)
        //                if (aButton != null)
        //                    blockMove = Convert.ToInt32(aButton.Uid);
        //                else if (bButton != null)
        //                    blockMove = Convert.ToInt32(bButton.Uid);

        //            aButton = null;
        //            bButton = null;
        //            xCount = 0;
        //            oCount = 0;

        //            for (int x = 0; x < 4; x++)
        //            {
        //                int a = 4 * (x + 1) + 5 * y - 1;

        //                if (board[a].Content == null)
        //                    aButton = board[a];

        //                if (String.Equals(board[a].Content, "X"))
        //                    xCount++;
        //                else if (String.Equals(board[a].Content, "O"))
        //                    oCount++;
        //            }

        //            if (oCount > 2 && winMove == -1)
        //                if (aButton != null)
        //                    winMove = Convert.ToInt32(aButton.Uid);
        //            if (xCount > 2 && blockMove == -1)
        //                if ((xCount + oCount) < 4)
        //                    if (aButton != null)
        //                        blockMove = Convert.ToInt32(aButton.Uid);
        //        }
        //    }

        //    if(movesLeft.Count > 0)
        //        if (winMove != -1)
        //        {
        //            myMove = winMove;
        //            Debug.WriteLine("Win at " + myMove);
        //        }
        //        else if (blockMove != -1)
        //        {
        //            myMove = blockMove;
        //            Debug.WriteLine("Block at " + myMove);
        //        }
        //        else
        //        {
        //            myMove = movesLeft.ElementAt(random.Next(0, movesLeft.Count));
        //            Debug.WriteLine("Random move at " + myMove);
        //        }
            
        //    movesLeft.Remove(myMove);
        //    board[myMove].RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        //}

    
        
     
//*******************************HARD MODE ***************************************


        public int EvalGameState(string[] game, string seed)            //evaluation of the game board; gives a score based on each
        {                                                               // potential winning combination's state
            int score = 0;
            score += EvaluateLine(game, seed, 0, 1, 2, 3);  // evaluates each potential row win ; adds to score
            score += EvaluateLine(game, seed, 1, 2, 3, 4);
            score += EvaluateLine(game, seed, 5, 6, 7, 8);
            score += EvaluateLine(game, seed, 6, 7, 8, 9);
            score += EvaluateLine(game, seed, 10, 11, 12, 13);
            score += EvaluateLine(game, seed, 11, 12, 13, 14);
            score += EvaluateLine(game, seed, 15, 16, 17, 18);
            score += EvaluateLine(game, seed, 16, 17, 18, 19);
            score += EvaluateLine(game, seed, 20, 21, 22, 23);
            score += EvaluateLine(game, seed, 21, 22, 23, 24);
            score += EvaluateLine(game, seed, 0, 5, 10, 15);             //evaluates each potential column win; adds to score
            score += EvaluateLine(game, seed, 5, 10, 15, 20);
            score += EvaluateLine(game, seed, 1, 6, 11, 16);
            score += EvaluateLine(game, seed, 6, 11, 16, 21);
            score += EvaluateLine(game, seed, 2, 7, 12, 17);
            score += EvaluateLine(game, seed, 7, 12, 17, 22);
            score += EvaluateLine(game, seed, 3, 8, 13, 18);
            score += EvaluateLine(game, seed, 8, 13, 18, 23);
            score += EvaluateLine(game, seed, 4, 9, 14, 19);
            score += EvaluateLine(game, seed, 9, 14, 19, 24);
            score += EvaluateLine(game, seed, 0, 6, 12, 18);             //evaluates each potential diagonal win; adds to score
            score += EvaluateLine(game, seed, 6, 12, 18, 24);
            score += EvaluateLine(game, seed, 1, 7, 13, 19);
            score += EvaluateLine(game, seed, 5, 11, 17, 23);
            score += EvaluateLine(game, seed, 4, 8, 12, 16);
            score += EvaluateLine(game, seed, 8, 12, 16, 20);
            score += EvaluateLine(game, seed, 3, 7, 11, 15);
            score += EvaluateLine(game, seed, 9, 13, 17, 21);

            return score;                   // returns overall score of the board

        }
        public int EvaluateLine(string[] game, string seed, int pos1, int pos2, int pos3, int pos4)  //evaluates a potetial winning combination
        {
            int score = 0;          //set score to 0
            int count = 0;          //set positive count to 0
            int oppCount = 0;       //set negative count to 0

            string oppSeed;

            if (seed == "X")        //if seed is X , X will add to the score and O will take away from the score
            { oppSeed = "O"; }      
            else                    //if seed is O , O will add to the score and X will take away from the score
            { oppSeed = "X"; }

            if (game[pos1] == seed)     //if a position contains "seed" adds to positive count
                count++;
            if (game[pos2] == seed)
                count++;
            if (game[pos3] == seed)
                count++;
            if (game[pos4] == seed)
                count++;

            if (game[pos1] == oppSeed)  //if a positions contains the opponent's seed adds to negative count
                oppCount++;
            if (game[pos2] == oppSeed)
                oppCount++;
            if (game[pos3] == oppSeed)
                oppCount++;
            if (game[pos4] == oppSeed)
                oppCount++;

            if (oppCount == 0)          // if there are no opponents moves in the line gives positive score depending on positive count
            {
                if (count == 4)         // if positive count is 4 score is 1000
                    return 1000;
                if (count == 3)         //if positive count is 3 score is 100
                    return 100;
                if (count == 2)         // if positive count is 2 score is 10
                    return 10;
                if (count == 1)         // if positive count is 1 score is 1
                    return 1;

            }

            if (count == 0)         // if there are no positive moves in the line, gives negative score depending on negative count
            {
                if (oppCount == 4)  // if negative count is 4 score is -1000
                    return -1000;
                if (oppCount == 3)  // if negative count is 3 score is -100
                    return -100;
                if (oppCount == 2)  // if negative count is 2 score is -10
                    return -10;
                if (oppCount == 1)  // if negative count is 1 score is -1
                    return -1;
            }

                                // if there are both opponent and seed's moves in the line the score is 0

            return score;
        }


        public void hard() //hard move
        {
            int count = 0;
            int ph = -1;
            int bestMove;
            string[] game;
            game = new string[25];   //game board copy for MINIMAX



            for (int j = 0; j < 25; j++)  //looks at the board to find any moves
            {
                if (String.Equals(board[j].Content, "X") || String.Equals(board[j].Content, "O"))
                {
                    count++;
                    ph = j;  
                }
            }

            if (count < 1)                                         // random move in center 9 squares
            {                                                   // if no moves have been made
                centerNine = new List<int>(){6,7,8,11,12,13,16,17,18};

                if (ph > 0)
                    centerNine.Remove(ph);

                bestMove = centerNine.ElementAt(random.Next(0, centerNine.Count-1));

                Debug.WriteLine("Random move at " + bestMove);
            }
            else
            {                                                   //copies the game board
                for (int i = 0; i < 25; i++)
                {
                    if (String.Equals(board[i].Content, "X"))
                    { game[i] = "X"; }
                    else if (String.Equals(board[i].Content, "O"))
                    { game[i] = "O"; }
                    else
                    { game[i] = ""; }
                }

                depth = 0;                      //sets depth to 0

                bestMove = MinMax(game);        // starts MINIMAX recursion

                Debug.WriteLine("Move at " + bestMove);
            }

            makeMove(bestMove);

        }
        public int MinMax(string[] game)             //            MinMax (GamePosition game) 
        {
            return MaxMove(game, 0, 0);             // starts minimax with a maximum move
        }
        public bool isWin(string[] game)        // funtion to determing if a winning state had been reached
        {

            //check horizontal wins
            for (int y = 0; y < 5; y++)
            {
                bool case1 = false, case2 = false;

                for (int x = 0; x < 3; x++)
                {
                    int a = y * 5 + x, b = a + 1;

                    if (!(x != 0 && case1 == false) && !game[a].Equals(""))
                        case1 = game[a].Equals(game[b]);

                    if (!(x != 0 && case2 == false) && !game[b].Equals(""))
                        case2 = game[b].Equals(game[b + 1]);

                    if (case1 == false && case2 == false)
                        break;

                }

                if (case1 == true || case2 == true)
                    return true;

                case1 = false;
                case2 = false;

                //check vertical wins
                for (int x = 0; x < 3; x++)
                {
                    int a = 5 * x + y, b = 5 * (x + 1) + y;

                    if (!(x != 0 && case1 == false) && !game[a].Equals(""))
                        case1 = game[a].Equals(game[b]);

                    if (!(x != 0 && case2 == false) && !game[b].Equals(""))
                        case2 = game[b].Equals(game[b + 5]);

                    if (case1 == false && case2 == false)
                        break;


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

                        if (!(x != 0 && case1 == false) && !game[a].Equals(""))
                            case1 = game[a].Equals(game[a + 6]);

                        if (!(x != 0 && case2 == false) && !game[b].Equals(""))
                            case2 = game[b].Equals(game[b + 6]);

                        if (case1 == false && case2 == false)
                            break;


                    }

                    if (case1 == true || case2 == true)
                        return true;

                    case1 = false;
                    case2 = false;


                    //check diagonal left wins
                    for (int x = 0; x < 3; x++)
                    {
                        int a = 4 * (x + 1) + 5 * y, b = a - 1;

                        if (!(x != 0 && case1 == false) && !game[a].Equals(""))
                            case1 = game[a].Equals(game[a + 4]);

                        if (!(x != 0 && case2 == false) && !game[b].Equals(""))
                            case2 = game[b].Equals(game[b + 4]);

                        if (case1 == false && case2 == false)
                            break;


                    }
                }


                if (case1 == true || case2 == true)
                    return true;
            }

            return false;
        }

        public bool boardFull(string[] game)    //function to determine if the board is full
        {
            for (int i = 0; i < 25; i++)
            {
                if (game[i] == "")
                    return false;
            }
            return true;
        }

        public bool squareEmpty(string[] game, int position)             // determines if a square is empty
        {
            if (game[position].Equals(""))
            { return true; }
            else
                return false;
        }

        public string[] placement(string[] game, int position, string piece = "") // places or removes a move on the game board
        {
            game[position] = piece;
            return game;
        }

        //********************MAX MOVE
        public int MaxMove(string[] game, int alpha, int beta)           //max move        
        {

            int reply;                  
            int bestMove = -1;
            int bestMoveVal = -1000000;




            if (boardFull(game) || depth > 3 || isWin(game))        // if a winning state is reached or the depth of the recursion is
            {                                                       //greater than 3, or the game board is full, it returns a score for        
                return EvalGameState(game, "O");                    //that iteration of the game board

            }
            else
            {

                for (int position = 0; position < 25; position++)     //for each potential move it receives a score
                {

                    if (squareEmpty(game, position))                    
                    {
                        game = placement(game, position, "O");               //applys a potential move
                        depth++;                                             // adds to recursion depth


                        reply = MinMove(game, alpha, beta);        // gets the score for the move

                        game = placement(game, position);                  // unapplys the move
                        depth--;                                           //subtracts from recursion depth

                        if (reply > bestMoveVal)                //picks the best score of all potential moves
                        {
                            bestMoveVal = reply;
                            bestMove = position;
                            alpha = bestMoveVal;
                        }

                        // if (beta > alpha)
                        //   return bestMoveVal;

                    }

                }
            }

            if (depth > 0)                  // if depth is greater than 0 i.e. still in recursion
                return bestMoveVal;         //returns best move value of current depth of recusion

            return bestMove;                //returns best move to make

        }

        //*************MIN MOVE
        public int MinMove(string[] game, int alpha, int beta)                 
        {

            int reply;


            int bestMove;
            int bestMoveVal = 100000000;




            if (boardFull(game) || depth > 3 || isWin(game))     //returns evalution of ending game state or limit of recursion
            {
                return EvalGameState(game, "O");

            }
            else
            {


                for (int position = 0; position < 25; position++)  //evaluates potential move of the player
                {

                    if (squareEmpty(game, position))
                    {
                        game = placement(game, position, "X");     //applys players move to game board
                        depth++;                                   // adds to depth


                        reply = MaxMove(game, alpha, beta);        // gets score of potential move

                        game = placement(game, position);           //unapplys player move 
                        depth--;                                    // subtracts depth

                        if (reply < bestMoveVal)                    // picks the lowest score i.e. the best move for the player
                        {
                            bestMoveVal = reply;
                            bestMove = position;
                            beta = bestMoveVal;
                        }

                        // if (beta < alpha)
                        //  return bestMoveVal;

                    }
                }
            }
            return bestMoveVal;         // returns best score for the player
        }

//**************************************************************MEDIUM*****************************************************    
        public void mediumMode()
        {
            int[] bestMove;         //holds move with highest score
            int[] test;            //testing potential moves
            string[] game;
            game = new string[25]; // copy of game board

            bestMove = new int[2];   //bestMove[0] is the position and bestMove[1] is the score for that position 
            test = new int[2];      // test[0] is the position and test[1] is the score for that position

            bestMove[0]=0;          //initialize best move
            bestMove[1]=-100000;
            

            if (movesLeft.Count >= 24)                                         // random move in center 9 squares
            {                                                                   // if only one or no moves have been made
                centerNine = new List<int>() {6,7,8,11,12,13,16,17,18 };

                do
                {
                    bestMove[0] = centerNine.ElementAt(random.Next(centerNine.Count));
                } while (!movesLeft.Contains(bestMove[0]));
            }
            else
            {
                for (int i = 0; i < 25; i++)            // copies game board
                {
                    if (String.Equals(board[i].Content, "X"))
                    { game[i] = "X"; }
                    else if (String.Equals(board[i].Content, "O"))
                    { game[i] = "O"; }
                    else
                    { game[i] = ""; }
                }


                for (int i=0;i<25;i++)
                {                                           //runs through potential moves
                    if(squareEmpty(game, i))
                    {
                        game = placement(game,i, "O");      //places potential move on copy of game board
                        test[0]=i;                          // sets test to that move
                        test[1] = EvalGameState(game, "O"); // receives score for that move
                        game = placement(game, i);          // removes move from copy of game board
                        
                        if(test[1]>bestMove[1])             //if test position had higher score than bestMove; assigns bestMove to test
                        {
                            Debug.WriteLine("Old best move value is at: {0}\n**********New best move value is at {1}", bestMove[1], test[1]);
                            bestMove[1] = test[1];
                            bestMove[0] = test[0];
                        }
                        
                    }
                }
            }

            makeMove(bestMove[0]);
            Debug.WriteLine("Medium move at " + bestMove[0]);
        }                     
 //******************************************************EASY REVISION**********************************************

       
        }
        
}
