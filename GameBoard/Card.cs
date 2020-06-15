namespace GameBoard
{
    public class Card
    {
        // Field
        private readonly string r_Data;

        // Proprties
        public string Data
        {
            get
            {
                return this.r_Data;
            }
        }

        /*
        * The Card constrcutor
        * Init the r_Data of the Card
        */
        public Card(string i_Data)
        {
            this.r_Data = i_Data;
        }
    }
}

