using System;
using System.Text;
using Players;
using GameBoard;
using Ex02.ConsoleUtils;

namespace MemoryGame
{
    public class Game
    {
        private static Player s_Player1;
        private static Player s_Player2H;
        private static ComputerPlayer s_Player2C;
        private static Board s_Board;
        private static bool s_IsHuman = true;
        private static int s_PlayerTurn = 1;

        /**
        * This method starting the game.
        */
        public static void StartGame()
        {
            //initiating stages of the game
            s_Player1 = new Player(initiatePlayer());
            gameMode();
            if (s_IsHuman)
            {
                s_Player2H = new Player(initiatePlayer());
            }
            else
            {
                s_Player2C = new ComputerPlayer();
            }
            //starts the game
            run();
        }

        /**
        * This method initiate a playing player.
        */
        private static string initiatePlayer()
        {
            Console.WriteLine("Hello please enter a name (must not be empty):");
            string playerName = Console.ReadLine();
            while (String.IsNullOrWhiteSpace(playerName))
            {
                Console.WriteLine("Name must not be empty!");
                Console.WriteLine("Hello please enter your name:");
                playerName = Console.ReadLine();
            }

            Console.WriteLine();

            return playerName;
        }

        /**
         * This method checks if the player want to play against the computer of a friend 
         */
        private static void gameMode()
        {
            string playerTwoType;
            do
            {
                Console.WriteLine("Do you want to play against a friend or the computer?");
                Console.WriteLine("Type f for friend or c for computer");
                playerTwoType = Console.ReadLine();
                Console.WriteLine();
            } while (!checkPlayerType(playerTwoType));
        }

        /**
        * This method running the game flow.
        */
        private static void run()
        {
            bool gameFinished = true;

            Screen.Clear();
            // builds the board
            buildBoard();
            // show first empty board
            boardClearAndReBuild("");
            while (!isGameOver())
            {
                //pick first card
                string card1 = playerPick(s_PlayerTurn);
                if (card1.Equals("Q"))
                {
                    gameFinished = false;
                    break;
                }

                boardClearAndReBuild(card1);
                //pick second card
                string card2 = playerPick(s_PlayerTurn);
                if (card2.Equals("Q"))
                {
                    gameFinished = false;
                    break;
                }

                //checks if the 2 cards match
                //switches players if there was no match
                if (!isMatch(card1, card2))
                {
                    if (s_PlayerTurn == 1)
                    {
                        s_PlayerTurn++;
                        if (!s_IsHuman)
                        {
                            s_PlayerTurn++;
                        }
                    }
                    else
                    {
                        s_PlayerTurn = 1;
                    }
                }
            }

            if (gameFinished)
            {
                boardClearAndReBuild("");
                isWinner();
                replayGame();
            }
        }

        /**
        * This method builds the current games board
        */
        private static void buildBoard()
        {
            //height
            string rows = "0";
            //width
            string columns = "0";

            do
            {
                string sizeMessage = "4,5 or 6";
                Console.WriteLine("Enter the width of the board ({0}) followed by pressing enter", sizeMessage);
                columns = Console.ReadLine();
                Console.WriteLine();
                sizeMessage = columns.Equals("5") ? sizeMessage.Replace(",5", "") : sizeMessage;
                Console.WriteLine("Enter the height of the board ({0}) followed by pressing enter", sizeMessage);
                rows = Console.ReadLine();
            } while (!checkValidBoardSize(rows, columns));

            s_Board = new Board(int.Parse(rows), int.Parse(columns));
            if (!s_IsHuman)
            {
                s_Player2C.RememberBoard(s_Board.Row, s_Board.Col);
            }
        }

        /**
        * This method collects the players picks
        */
        private static string playerPick(int i_Player)
        {
            string playerPick = "";

            // computer player
            if (i_Player == 3)
            {
                do
                {
                    playerPick = s_Player2C.Turn(s_Board.Row, s_Board.Col);
                } while (!checkForValidInput(playerPick));
            }
            // Human Player
            else
            {
                string PlayerName = (i_Player == 1) ? s_Player1.Name : s_Player2H.Name;
                do
                {
                    Console.WriteLine("{0} pick a square (or press Q to quit):", PlayerName);
                    Console.WriteLine("Examples for valid inputs A1, D2, C4");
                    playerPick = Console.ReadLine();
                    if (playerPick.Equals("Q"))
                    {
                        break;
                    }
                } while (!checkForValidInput(playerPick));
            }

            return playerPick;
        }

        /**
        * This method help for quick rebuilding the board after the pick.
        */
        private static void boardClearAndReBuild(string i_PlayerInput)
        {
            Screen.Clear();
            if (!i_PlayerInput.Equals(""))
            {
                int row = int.Parse(i_PlayerInput[1].ToString());
                eColumnIndex col = (eColumnIndex)Enum.Parse(typeof(eColumnIndex), i_PlayerInput[0].ToString());
                string cardValue = s_Board.Reveal(row, (int)col);
                if (!s_IsHuman)
                {
                    s_Player2C.AddToMemory(i_PlayerInput, cardValue);
                }
            }

            s_Board.PrintBoard();
        }

        /**
        * This method help for quick rebuilding the board after the pick.
        */
        private static bool isMatch(string i_Card1, string i_Card2)
        {
            bool hasMatched = true;
            int row1 = int.Parse(i_Card1[1].ToString());
            eColumnIndex col1 = (eColumnIndex)Enum.Parse(typeof(eColumnIndex), i_Card1[0].ToString());
            int row2 = int.Parse(i_Card2[1].ToString());
            eColumnIndex col2 = (eColumnIndex)Enum.Parse(typeof(eColumnIndex), i_Card2[0].ToString());

            boardClearAndReBuild(i_Card2);
            if (s_Board.GetShownCard(row1, (int)col1).Equals(s_Board.GetShownCard(row2, (int)col2)))
            {
                string pointMessage = "";
                if (s_PlayerTurn == 1)
                {
                    s_Player1.Score++;
                    pointMessage = s_Player1.Name;
                }
                else if (s_PlayerTurn == 2)
                {
                    s_Player2H.Score++;
                    pointMessage = s_Player2H.Name;
                }
                else
                {
                    s_Player2C.Score++;
                    pointMessage = s_Player2C.Name;
                }

                if (!s_IsHuman)
                {
                    s_Player2C.RemoveFromBoardMemory(i_Card1, i_Card2);
                }

                Console.WriteLine("{0} got a point!\n", pointMessage);
            }
            else
            {
                hasMatched = false;
                System.Threading.Thread.Sleep(2000);
                //hides first card
                s_Board.Hide(row1, (int)col1);
                //hides second card
                s_Board.Hide(row2, (int)col2);

                boardClearAndReBuild("");
            }

            return hasMatched;
        }

        /**
        * This method determing the winner and end the game.
        */
        private static void isWinner()
        {
            string winnerName = "";
            string loserName = "";
            int scorePlayer2 = 0;
            int winningScore = 0;
            int loosingScore = (s_Board.Row * s_Board.Col) / 2;

            if (s_IsHuman)
            {
                winnerName = s_Player1.Score > s_Player2H.Score ? s_Player1.Name : s_Player2H.Name;
                loserName = s_Player1.Score > s_Player2H.Score ? s_Player2H.Name : s_Player1.Name;
                winningScore = s_Player1.Score > s_Player2H.Score ? s_Player1.Score : s_Player2H.Score;
                scorePlayer2 = s_Player2H.Score;
            }
            else
            {
                winnerName = s_Player1.Score > s_Player2C.Score ? s_Player1.Name : s_Player2C.Name;
                loserName = s_Player1.Score > s_Player2C.Score ? s_Player2C.Name : s_Player1.Name;
                winningScore = s_Player1.Score > s_Player2C.Score ? s_Player1.Score : s_Player2C.Score;
                scorePlayer2 = s_Player2C.Score;
            }

            if (scorePlayer2 == s_Player1.Score)
            {
                Console.WriteLine("{0} score: {1}, {2} score: {3}", loserName, s_Player1.Score, winnerName, scorePlayer2);
                Console.WriteLine("It's a tie!\n");
            }
            else
            {
                loosingScore -= winningScore;
                Console.WriteLine("{0} score: {1}, {2} score: {3}", winnerName, winningScore, loserName, loosingScore);
                Console.WriteLine("{0} is the winner!\n", winnerName);
            }
        }

        /**
        * This method asks the user if he want to play again.
        */
        private static void replayGame()
        {
            string replay = "";

            do
            {
                Console.WriteLine("Would you like to play another game? y or n");
                replay = Console.ReadLine();
            } while (!checkReplay(replay));

            if (replay.Equals("y"))
            {
                s_Player1.Score = 0;
                if (s_IsHuman)
                {
                    s_Player2H.Score = 0;
                }
                else
                {
                    // creates new player with a clean memory
                    s_Player2C = new ComputerPlayer();
                }

                s_PlayerTurn = 1;
                run();
            }
        }

        /**
        * This method determines if the game is over.
        */
        private static bool isGameOver()
        {
            int pointSum = s_IsHuman ? s_Player1.Score + s_Player2H.Score : s_Player1.Score + s_Player2C.Score;

            return (s_Board.Col * s_Board.Row) / 2 == pointSum;
        }

        /**
         * enum representing the column letters and index
         */
        private enum eColumnIndex
        {
            A = 1,
            B = 2,
            C = 3,
            D = 4,
            E = 5,
            F = 6
        }

        /**
        * This method checks if the player input was valid or not
        * if we update s_IsHuman accordingly
        */
        private static bool checkPlayerType(string i_playerType)
        {
            //checks if it is a human
            s_IsHuman = i_playerType.Equals("f");
            bool isValid = i_playerType.Equals("c") || i_playerType.Equals("f");
            if (!isValid)
            {
                Console.WriteLine("Illegal player type entered\n");
            }
            return isValid;
        }

        /**
        * This method checks if the given board dimensions are legal
        */
        private static bool checkValidBoardSize(string i_row, string i_col)
        {
            int rows = 0;
            int col = 0;
            //check that they are numbers
            bool isValid = int.TryParse(i_row, out rows) && int.TryParse(i_col, out col);

            if (isValid)
            {
                //check that they are in the right range and that there are an even number of squares
                isValid = (rows >= 4 && rows <= 6)
                          && (col >= 4 && col <= 6)
                          && !(col == 5 && rows == 5);
            }

            if (!isValid)
            {
                Screen.Clear();
                Console.WriteLine("Illegal dimensions entered!\n");
            }

            return isValid;
        }

        /**
        * This method check for valid input from the player.
        */
        private static bool checkForValidInput(string i_PlayerInput)
        {
            bool isValid = (i_PlayerInput.Length == 2);
            int row = 0;
            StringBuilder errorMessage = new StringBuilder("");

            if (!isValid)
            {
                errorMessage.Append("Not a co-ordinate! please refer to given example");

            }
            else if (int.TryParse(i_PlayerInput[0].ToString(), out row))
            {
                errorMessage.Append("Illegal column input");
                isValid = false;
            }
            else
            {
                eColumnIndex col = 0;
                bool validCol = Enum.TryParse(i_PlayerInput[0].ToString(), false, out col);
                bool validRow = int.TryParse(i_PlayerInput[1].ToString(), out row);
                bool isHidden = true;

                //if the inputed letter is an option check if it is bigger than the max option
                if (!validCol || (validCol && (int) col > s_Board.Col))
                {
                    errorMessage.Append("Illegal column input");
                    validCol = false;
                }

                //checks row validity
                if (row < 1 || row > s_Board.Row)
                {
                    if (errorMessage.Length != 0)
                    {
                        errorMessage.Append(" and ");
                    }

                    errorMessage.Append("Illegal row input");
                    validRow = false;
                }

                //checks if card is shown already
                if (validRow && validCol)
                {
                    isHidden = !s_Board.IsShown(row, (int)col);
                }

                if (!isHidden && s_PlayerTurn != 3)
                {
                    errorMessage.Append("Card already chosen, pick a different card");
                }
                isValid = isHidden && validRow && validCol;
            }

            boardClearAndReBuild("");
            errorMessage.AppendLine();
            Console.Write(errorMessage.AppendLine().ToString());
            
            return isValid;
        }

        /**
         * Checks the input of the replay
        */
        private static bool checkReplay(string i_Input)
        {
            bool isValid = i_Input.Equals("y") || i_Input.Equals("n");
            if (!isValid)
            {
                Console.WriteLine("Illegal input entered");
            }

            return isValid;
        }
    }
}
