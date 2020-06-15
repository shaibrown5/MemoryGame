using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Players
{
    public class ComputerPlayer
    {
        // Fields
        private readonly string r_Name;
        private int m_Score = 0;
        private List<string> m_EmptyBoardSpaces;
        // Key is card value value is card coordinates
        private Dictionary<string, string> m_Memory = new Dictionary<string, string>();
        private Queue m_KnownPairs = new Queue();

        // Propreties
        public string Name
        {
            get
            {
                return r_Name;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }
            set
            {
                m_Score = value;
            }
        }

        /*
         * The ComputerPlayer constructor
         * init the name for the computer to be "computer player"
         */
        public ComputerPlayer()
        {
            this.r_Name = "Computer Player";
        }

        /**
         * Remembers all coordinates of cards that are face down
         */
        public void RememberBoard(int i_Row, int i_Col)
        {
            m_EmptyBoardSpaces = new List<string>(i_Col * i_Row);

            for (int i = 0; i < i_Col; i++)
            {
                for (int j = 1; j <= i_Row; j++)
                {
                    StringBuilder coordinates = new StringBuilder();
                    // appends a letter from the column
                    coordinates.Append((char)('A' + i));
                    // appends a number of a row
                    coordinates.Append(j);
                    m_EmptyBoardSpaces.Add(coordinates.ToString().Trim());
                }
            }
        }

        /**
         * Adds a turned over cards position to the computers memory
         * If the computer remembers both cards positions, adds them to the known pairs's queue
         */
        public void AddToMemory(string i_CardCoordinates, string i_CardValue)
        {
            string cardCoordinates = "";
            bool cardInMemory = m_Memory.TryGetValue(i_CardValue, out cardCoordinates);

            // if the card value is in memory and its coordinates are different from the input
            if (cardInMemory && !cardCoordinates.Equals(i_CardCoordinates))
            {
                // add to the known pairs queue
                m_KnownPairs.Enqueue(cardCoordinates);
                m_KnownPairs.Enqueue(i_CardCoordinates);
            }
            else if (!cardInMemory)
            {
                // add card to memory
                m_Memory.Add(i_CardValue, i_CardCoordinates);
            }
        }

        /*
         * Make a turn as the computer player
         * Output a string with a pick for the computer player.
         */
        public string Turn(int i_Row, int i_Col)
        {
            string computerPick = "";
            Random random = new Random();

            // if the computer found a pair
            if (m_KnownPairs.Count != 0)
            {
                computerPick = m_KnownPairs.Dequeue().ToString();
            }
            else
            {
                if (m_EmptyBoardSpaces.Count > 2)
                {
                    //pick random card that has not been revealed
                    computerPick = m_EmptyBoardSpaces[random.Next(0, m_EmptyBoardSpaces.Count)];
                }
                //this is to improve the time of the computers turn
                else
                {
                    computerPick = m_EmptyBoardSpaces[0];
                    m_EmptyBoardSpaces.RemoveAt(0);
                }
            }

            //slows the computer reaction time
            System.Threading.Thread.Sleep(500);

            return computerPick;
        }

        /**
         * removes coordinates if they are not faced down anymore
         */
        public void RemoveFromBoardMemory(string i_Coordinates1, string i_Coordinates2)
        {
            m_EmptyBoardSpaces.Remove(i_Coordinates1);
            m_EmptyBoardSpaces.Remove(i_Coordinates2);
        }
    }
}
