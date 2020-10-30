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
        readonly StringBuilder lines = new StringBuilder();
        #endregion

        #region Methods

        /// <summary>
        /// Prompts the user if they want to see how many words they are to guess from before the game starts
        /// </summary>
        void PromptUserForTotalOfWordsInList()
        {
            string input;
            bool stop = false;

            while (!stop)
            {
                Console.WriteLine("Do you want to see the number of words in the list? y/n");
                input = Console.ReadLine();

                if (input.Length == 1 && input == "y" || input == "Y")
                {
                    if (AI.CurentWordFamily.Count == 1)
                    {
                        Console.WriteLine("There is " + AI.CurentWordFamily.Count + " word to guess from.");
                    }
                    else
                    {
                        Console.WriteLine("There are " + AI.CurentWordFamily.Count + " words to guess from.");
                    }
                    stop = true;
                }
                else if (input.Length == 1 && input == "n" || input == "N")
                {
                    stop = true;
                } 
            }
        }

        /// <summary>
        /// Prompts the user for the number of guesses they want
        /// </summary>
        /// <returns>Returns the number of guesses for the player</returns>
        int PromptUserForNumberOfGuesses()
        {
            //stops the loop
            bool stop = false;

            //number of guesses for the user
            int numberOfGuesses = -1;

            //loop until it returns a number
            while (!stop)
            {
                Console.WriteLine("Enter the number of guesses you want: ");
                numberOfGuesses = Convert.ToInt32(Console.ReadLine());
                if (numberOfGuesses > 0)
                {
                    return numberOfGuesses;
                }
                else
                {
                    Console.WriteLine("Gusses must be greater than 0.");
                } 
            }
            return numberOfGuesses;
        }

        /// <summary>
        /// Prints all of the used letters to the console
        /// </summary>
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
                //if the subject element is a dash, increment the dashCount
                if (lines[i] == '-')
                {
                    dashCount++;
                }
            }

            //Word has been guessed when there are no more dashes in the blanked out word
            if (dashCount == 0)
            {
                Console.WriteLine("You guessed the word! It was " + "'" + AI.ChosenWord + "'.");
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                p1Win = true;
            }
            //if there are any dashes, the player has not won 
            else
            {
                p1Win = false;
            }

            return p1Win;
        }
        /// <summary>
        /// Starts the game loop
        /// </summary>
        public void StartGame()
        {
            #region test
            WordManagement.WordLength = WordManagement.PromptUserForWordLength();
            player.GuessesLeft = PromptUserForNumberOfGuesses();
            PromptUserForTotalOfWordsInList();
            AI.ChosenWord.Append(WordManagement.AllWords[0]);
            Console.WriteLine("AI word fam element 0 = " + AI.ChosenWord);
            #endregion

            lines.Append(WordManagement.BlankOutWord(AI.ChosenWord.Length));
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
                        Console.WriteLine("GAME OVER! You ran out of guesses." + " The word was: " + "'" + AI.ChosenWord + "'");
                        Console.WriteLine("Press any key to exit.");
                        Console.ReadKey();
                    }
                }
            }
        }

        #endregion
    }
}
