using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe_Ultimate_Edition.Libraries 
{
    public class Player
    {
        private String avatar;          //initializes avatar field
        private String name;            //initializes name of the player(s)
        private long score;             //initializes the score of the player(s)
        private int maxScore;           //initializes the maximum score
        private int gameCount;          //initializes the number of moves made by the player
        private int modifier;           //initializes the value used to compute the max score
        private int win;                //initializes the wins
        private int loss;               //initializes the losses

        public int Win                  //the win property
        {
            get { return win; }         //returns the value of win
            set { win = value; }        //sets wins from the database
        } 
        public int Loss                 //the loss property
        {
            get { return loss; }        //returns the value of loss
            set { loss = value; }       //sets losses from the database
        }
        public int Modifier             //the modifier property
        {
            get { return modifier; }    //returns the value used to compute the max score
            set { modifier = value; }   
        }
        public int GameCount            //the gamecount property
        {
            get { return gameCount; }   
            set { gameCount = value; }      
        }
        public int MaxScore             //the maximum score property
        {
            get { return maxScore; }    //returns the maximum score
            set { maxScore = value; }
        }
        public String Avatar            //the avatar property
        {
            get { return avatar; }      //returns the avatar chosen by the player(s)
            set { avatar = value; }
        }
        public String Name              //the Name of the player(s) property
        {
            get { return name; }        //returns the player(s) name
            set { name = value; }
        }
        public long Score               //the score property        
        {
            get { return score; }       //returns score of the player(s)
            set { score = value; }
        }


        //stores the player's profile: avatar chosen, name, scores, wins & losses (default value set to 0) 
        public Player(String avatar, String name, long score, int win = 0, int loss = 0)
        {
            this.avatar = avatar;
            this.name = name;
            this.score = score;
            this.win = win;
            this.loss = loss;

            newGame();                  //once initialized the newGame() method is invoked
        }

        public void newGame()           //the new game method
        {
            maxScore = 1000;            //default maximum score is set to 1000
            modifier = 100;             //default modifier value is set to 100
            gameCount = 0;              
        }


        //the difficulty level method is invoked 
        public void setDifficulty(DIFFICULTY diff)      
        {
            switch(diff)
            {
                //sets the modifier value for Medium level
                case DIFFICULTY.Medium: maxScore *= 5;      
                    modifier *= 5;
                    break;
                //sets the modifier value for Hard level
                case DIFFICULTY.Hard: maxScore *= 15;       
                    modifier *= 15;
                    break;
            }
        }

        


        public int changeCurrentScore()     
        {
            gameCount++;                   //increments the current score

            return getCurrentScore();      //returns the current score
        }

        public int getCurrentScore()        
        {
            if (gameCount < 5)                  //if number of moves made by the player is less than 5, the max score is returned
                return maxScore;
                                           
  //if number of moves made by the player is more than 5, 
  //we compute the max score by deducting the extra moves(after 5) multiplied by the modifier from the max score   
            else    
                return maxScore - (gameCount - 4) * modifier;
        }
    }
}
