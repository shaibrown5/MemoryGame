using System;
using System.Collections.Generic;

namespace GameBoard
{
    public class Deck
        {
            // Field
            private List<Card> m_ListWithValues;

            /*
            * The Cards constrcutor
            * Init the size of the list
            * Adding values to m_ListWithValues and shuffling them.
            */
            public Deck(int i_BoardSize)
            {
                this.m_ListWithValues = new List<Card>(i_BoardSize);
                makeCards(i_BoardSize);
                shuffleCards();
            }

            /*
            * Build the deck of cards
            * Adding 2 one of each card
            */
            private void makeCards(int i_BoardSize)
            {
                string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R" };

                for (int i = 0; i < (i_BoardSize / 2); i++)
                {
                    Card card = new Card(letters[i]);
                    m_ListWithValues.Add(card);
                    m_ListWithValues.Add(card);
                }
            }

            /*
            * Shuffling the cards using Fisher-Yates algorithm
            */
            private void shuffleCards()
            {
                Random random = new Random();

                for (int i = m_ListWithValues.Count - 1; i > 0; i--)
                {
                    int randomIndex = random.Next(0, i + 1);
                    Card holder = m_ListWithValues[i];
                    m_ListWithValues[i] = m_ListWithValues[randomIndex];
                    m_ListWithValues[randomIndex] = holder;
                }

            }

            /*
            * Get the wanted card from the list
            */
            public Card GetCard(int i_CardIndex)
            {
                return m_ListWithValues[i_CardIndex];
            }
        }
    }
