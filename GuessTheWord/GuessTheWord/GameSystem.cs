using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace GuessTheWord
{
    public class GameSystem
    {
        #region Objects
        public readonly AI AI = new AI();
        public readonly Player player = new Player();
        public readonly WordManagement WM = new WordManagement();
        readonly StringBuilder lines = new StringBuilder();
        #endregion

        #region Methods
        void PrintUsedLetters()
        {
            Console.Write("Used Letters: ");
            if (player.UsedLetters.Count == 0)
            {
                Console.WriteLine("No letters guessed.");
            }

            for (int i = 0; i < player.UsedLetters.Count; i++)
            {

                Console.Write(player.UsedLetters[i] + " ");
            }

            Console.WriteLine("");
        }
        /// <summary>
        /// Checks if the player has won based on the blanked out word
        /// </summary>
        /// <returns>Returns a bool that states if the player has won</returns>
        bool CheckForWinner()
        {
            //checks if the player has won
            bool p1Win;
            //checks the amount of lines in the blanked out word
            int dashCount = 0;
            //loops through the lines stringbuilder and counts the dashes (-)
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == '-')
                {
                    dashCount++;
                }
            }

            //Word has been gussed when there are no more dashes in the blanked out word
            if (dashCount == 0)
            {
                Console.WriteLine("You guessed the word! It was " + "'" + AI.ChosenWord + "'.");
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                p1Win = true;
            }
            else
            {
                p1Win = false;
            }

            return p1Win;
        }
        public void StartGame()
        {
            #region test
            Console.WriteLine(WM.AllWords[5]);
            AI.ChosenWord.Append(WM.AllWords[5]);
            #endregion

            lines.Append(WM.BlankOutWord(AI.ChosenWord.Length));
            bool gameOver = false;

            while (!gameOver)
            {
                Console.WriteLine("Guesses left: " + player.GuessesLeft);
                PrintUsedLetters();

                Console.WriteLine("Word: " + lines);
                string guess = player.MakeGuess();

                //Correct guess
                if (AI.ChosenWord.ToString().Contains(guess))
                {
                    //write to the console that the guess was correct
                    Console.WriteLine("Guess Correct! :)\n");
                    //add the guess to the correct letters 
                    player.CorrectLetters.Add(Convert.ToChar(guess));
                    //add the guess to used letters
                    player.UsedLetters.Add(Convert.ToChar(guess));

                    //loop through the lines stringbuilder
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (AI.ChosenWord[i] == Convert.ToChar(guess))
                        {
                            lines[i] = AI.ChosenWord[i];
                            if (CheckForWinner())
                            {
                                gameOver = true;
                            }
                        }
                    }
                }
                //triggers when the ai's chosen word does not contain the guess
                else
                {
                    //write to the console that the guess is incorrect
                    Console.WriteLine("Guess Incorrect :'(\n");
                    //add the guess to the used letters list
                    player.UsedLetters.Add(Convert.ToChar(guess));
                    //add the guess to the list of incorrect guesses
                    player.IncorrectLetters.Add(Convert.ToChar(guess));
                    //decrement a guess from the player
                    player.GuessesLeft--;

                    /*if the player runs out of guesses, set gameOver to true and write to the 
                    console that the game is over */
                    if (player.GuessesLeft == 0)
                    {
                        gameOver = true;
                        Console.WriteLine("GAME OVER! You ran out of guesses.");
                        Console.WriteLine("Press any key to exit.");
                        Console.ReadKey();
                    }
                }
            }
        }

        #endregion
    }
}
