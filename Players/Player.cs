namespace Players
{
    public class Player
    {
        // Fields
        private readonly string r_Name;
        private int m_Score = 0;
        
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
         * The Player constructor
         * init the Player Name by the input i_Name.
         */
        public Player(string i_Name)
        {
            this.r_Name = i_Name;
        }
    }
}
