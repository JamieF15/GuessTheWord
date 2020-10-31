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

            //loops until stop is true
            while (!stop)
            {
                //prompt the user if they want to see the number of words they are to guess from
                Console.WriteLine("Do you want to see the number of words in the list? y/n");
                //read the input
                input = Console.ReadLine();

                //if the length of the input is one or is 'y' or 'Y', this happens
                if (input.Length == 1 && input == "y" || input == "Y")
                {
                    //check if there is only one word in the AI word list for appropriate grammar to be printed
                    if (AI.CurentWordFamily.Count == 1)
                    {
                        Console.WriteLine("There is " + AI.CurentWordFamily.Count + " word to guess from.");
                    }
                    //check if the length is greater than 1 in order for correct grammer to be shown
                    else
                    {
                        Console.WriteLine("There are " + AI.CurentWordFamily.Count + " words to guess from.");
                    }
                    //stop the loop
                    stop = true;
                }
                //if the input is 1 and is 'n' or 'N', this happens
                else if (input.Length == 1 && input == "n" || input == "N")
                {
                    //stop the loop
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

            //for being parsed out of the int.TryParse 
            int parsed = -1;

            //number of guesses for the user
            string numberOfGuesses = "";

            //loop until it returns a number
            while (!stop)
            {
                //prompt the user for the number of guesses they want
                Console.WriteLine("Enter the number of guesses you want: ");
                numberOfGuesses = Console.ReadLine();

                //check if the input is a letter
                if (!int.TryParse(numberOfGuesses.ToString(), out parsed))
                {
                    //prompts user that the input was not a number
                    Console.WriteLine("Input was not a number.");
                }
                //if the input was a number, this happens
                else
                {
                    //check if the inputed was greater htan 0 
                    if (Convert.ToInt32(numberOfGuesses) > 0)
                    {
                        //returns the number of guesses 
                        return Convert.ToInt32(numberOfGuesses);
                    }
                    //happens when the input is less than 0
                    else
                    {
                        //shows the user that the guess must be greater than 0
                        Console.WriteLine("Guesses must be greater than 0.");
                    }
                }
            }
            //returns the number of guesses 
            return Convert.ToInt32(numberOfGuesses);
        }

        /// <summary>
        /// Prints all of the used letters to the console
        /// </summary>
        void PrintUsedLetters()
        {
            Console.Write("Used Letters: ");

            //if there has been no guessed letters this happens
            if (player.UsedLetters.Count == 0)
            {
                Console.WriteLine("no letters guessed.");
            }
            //if there are more than 0, this happens
            else
            {
                //loop through the list of used letters of the player
                for (int i = 0; i < player.UsedLetters.Count; i++)
                {
                    //write each element to the console
                    Console.Write(player.UsedLetters[i] + " \n");
                }
            }
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
                    //inrecement the dash counter
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
            WordManagement.WordLength = WordManagement.PromptUserForWordLength().ToString();
            player.GuessesLeft = PromptUserForNumberOfGuesses();
            PromptUserForTotalOfWordsInList();
            AI.ChosenWord.Append(WordManagement.AllWords[0]);
            Console.WriteLine("AI word fam element 0 = " + AI.ChosenWord);
            #endregion

            //set the appropriate number of dashes to the stringbuilder
            lines.Append(WordManagement.BlankOutWord(AI.ChosenWord.Length));
            bool gameOver = false;

            while (!gameOver)
            {
                Console.WriteLine("Guesses left: " + player.GuessesLeft);
                PrintUsedLetters();

                Console.WriteLine("Word: " + lines);
                string guess = player.MakeGuess();

                //the guess if correct if the chosen word contains the guessed letter
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
                        //for each letter of the chosen word, check if it is equal to the guess
                        if (AI.ChosenWord[i] == Convert.ToChar(guess))
                        {
                            /*set the appropriate element of the lines of the stringbuilder to the 
                            character of the corresponding chosen word*/
                            lines[i] = AI.ChosenWord[i];
                            //then check if the player has won (when there are no dashes left in the string builder)
                            if (CheckForWinner())
                            {
                                //set the game to 'over'
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
