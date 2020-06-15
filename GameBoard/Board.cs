using System;
using System.Text;

namespace GameBoard
{
    public class Board
    {
        private readonly int r_Rows;
        private readonly int r_Columns;
        private readonly Deck r_Deck;
        private string[] m_ShownValues;
        private string m_Board;

        public int Row
        {
            get
            {
                return r_Rows;
            }
        }

        public int Col
        {
            get
            {
                return r_Columns;
            }
        }

        /**
         * Constructor that receives number of columns and rows
         * changed int to string param
         */
        public Board(int i_Rows, int i_Columns)
        {
            //4,5 or 6
            r_Rows = i_Rows;
            //4 5 or 6
            r_Columns = i_Columns;
            m_ShownValues = new string[i_Columns * i_Rows];
            r_Deck = new Deck(i_Rows * i_Columns);
            setEmptyValue();
            makeBoard();
        }

        /**
         * Sets all values in the array m_ShownValues to " "
         */
        private void setEmptyValue()
        {
            for (int i = 0; i < m_ShownValues.Length; i++)
            {
                m_ShownValues[i] = " ";
            }
        }

        /**
         * Creates and stores the board according to the given dimensions
         */
        private void makeBoard()
        {
           // creates the appropriate orw of = signs
           string equalRow = makeEqualSignRow();
           // keeps track of the index number for string formating
           int formatIndex = 0;
           //creates board with generic column index row
           StringBuilder board = new StringBuilder("    A   B   C   D");

           //sets the first Column index row according to the number of columns
           if (r_Columns > 4)
           { 
               board.Append("   E"); 
               if (r_Columns == 6) 
               { 
                   board.Append("   F");
               }
           }

           board.AppendLine();
           board.Append(equalRow);

           //adds the rows to the board with their proper string format index numbers
            for (int i = 1; i <= r_Rows; i++)
            {
                board.Append(makeRow(i, formatIndex));
                board.Append(equalRow);
                formatIndex += r_Columns;
            }

            //saves the final board
            m_Board = board.ToString();
        }

        /**
         * This method creates a row of = sign with regards to the number of columns.
         */
        private string makeEqualSignRow()
        {
            StringBuilder row = new StringBuilder("  =");

            for (int i = 0; i < 4 * r_Columns; i++)
            {
                row.Append("=");
            }

            return row.AppendLine().ToString();
        }

        /**
         * This method builds the rows for the board
         */
        private string makeRow(int i_RowNum, int i_FirstFormatNumber)
        {
            StringBuilder row = new StringBuilder();

            row.Append(i_RowNum);
            for (int i = i_FirstFormatNumber; i < (i_FirstFormatNumber + r_Columns); i++)
            {
                row.Append(" | {");
                row.Append(i);
                row.Append("}");
            }

            row.AppendLine(" |");

            return row.ToString();
        }

        /**
         * reveals the box in row i_Row and column i_Col and returns it value
         */
        public string Reveal(int i_Row, int i_Col)
        {
            int position = positionValue(i_Row, i_Col);
            m_ShownValues[position] = r_Deck.GetCard(position).Data;

            return m_ShownValues[position];
        }

        /**
         * returns the revealed called
         */
        public string GetShownCard(int i_Row, int i_Col)
        {
            return m_ShownValues[positionValue(i_Row, i_Col)];
        }

        /**
         * This method hides a given square 
         */
        public void Hide(int i_Row, int i_Col)
        {
            m_ShownValues[positionValue(i_Row, i_Col)] = " ";
        }

        /**
         * This method returns true if a given coordinate is already shown
         */
        public bool IsShown(int i_Row, int i_Col)
        {
            return !m_ShownValues[positionValue(i_Row, i_Col)].Equals(" ");
        }

        /**
         * this method converts coordinates to a certain position in a 1D array
         */
        private int positionValue(int i_Row, int i_Col)
        {
            return ((i_Row - 1) * r_Columns) + (i_Col - 1);
        }

        /**
         * This method prints the board with the respective shown values
         */
        public void PrintBoard()
        {
            Console.WriteLine(this.m_Board, this.m_ShownValues);
        }
    }
}

