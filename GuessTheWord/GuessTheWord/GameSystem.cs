using Microsoft.VisualBasic;
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
        //instantiate the relevant objects
        public readonly AI AI = new AI();
        public readonly Player player = new Player();
        readonly StringBuilder lines = new StringBuilder();
        #endregion

        #region Methods

        /// <summary>
        /// Resets the data structures of the player and AI
        /// </summary>
        void ResetGameData()
        {
            //clear all the lists of correct, incorrect, and used letters
            player.CorrectLetters.Clear();
            player.IncorrectLetters.Clear();
            player.UsedLetters.Clear();

            //clear the lines stringbuilder
            lines.Clear();

            //clear the AI's data structures
            AI.ChosenWord.Clear();
            AI.CurrentWordFamily.Clear();
        }

        /// <summary>
        /// Prompts the user if they want to see how many words they are to guess from before the game starts
        /// </summary>
        void PromptUserForTotalOfWordsInList()
        {
            //holds user input
            string input;

            //stops the prompt loop
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
                    if (AI.CurrentWordFamily.Count == 1)
                    {
                        Console.WriteLine("There is " + AI.CurrentWordFamily.Count + " word to guess from.");
                    }
                    //check if the length is greater than 1 in order for correct grammer to be shown
                    else
                    {
                        Console.WriteLine("There are " + AI.CurrentWordFamily.Count + " words to guess from.");
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

            Console.WriteLine("Press any key to start!");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        void PromptUserToPlayAgain()
        {
            //holds user input
            string input;

            //stops the prompt loop
            bool stop = false;

            //loops until stop is true
            while (!stop)
            {
                //prompt the user if they want to see the number of words they are to guess from
                Console.WriteLine("Do you want to play again? y/n");

                //read the input
                input = Console.ReadLine();

                //if the length of the input is one or is 'y' or 'Y', this happens
                if (input.Length == 1 && input == "y" || input == "Y")
                {
                    //reset all the data structures in the game
                    ResetGameData();

                    //clear the console
                    Console.Clear();

                    //start the game again
                    StartGame();

                    //stop the loop
                    stop = true;
                }
                //if the input is 1 and is 'n' or 'N', this happens
                else if (input.Length == 1 && input == "n" || input == "N")
                {

                    //prompt the user to exit
                    Console.WriteLine("Press any key to exit.");

                    //puase the console
                    Console.ReadKey();

                    //stop the program from running
                    Environment.Exit(0);

                    //stop the loop
                    stop = true;
                }
            }

            Console.WriteLine("Press any key to start!");
            Console.ReadKey();
            Console.Clear();
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
            string numberOfGuesses = "";

            //loop until it returns a number
            while (!stop)
            {
                //prompt the user for the number of guesses they want
                Console.WriteLine("Enter the number of guesses you want: ");

                //reads and assigns the number of guesses for the player
                numberOfGuesses = Console.ReadLine();

                //check if the input is a letter
                if (!int.TryParse(numberOfGuesses, out _))
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
                        Console.WriteLine("Number of guesses must be greater than 0.");
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
            //this is written before the letters 
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
                    Console.Write(player.UsedLetters[i] + " ");
                }

                //Add a blank line to the console
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Checks if the player has won based on the blanked out word
        /// </summary>
        /// <returns>Returns a bool that states if the player has won</returns>
        bool CheckForWinner()
        {
            //checks if the player has won
            bool playerWin;

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

            //word has been guessed when there are no more dashes in the blanked out word
            if (dashCount == 0)
            {
                //print that the user has guessed the word
                Console.WriteLine("You guessed the word! It was " + "'" + AI.ChosenWord + "'.");
                PromptUserToPlayAgain();
                Console.ReadKey();
                playerWin = true;
            }
            //if there are any dashes, the player has not won 
            else
            {
                playerWin = false;
            }

            return playerWin;
        }

        /// <summary>
        /// Starts the game loop
        /// </summary>
        public void StartGame()
        {
            //shows if the game loop should stop or not
            bool gameOver = false;

            //holds the guessed letter
            string guess;

            //prompt the user for a number and set it to the word length
            WordManagement.WordLength = WordManagement.PromptUserForWordLength().ToString();

            //prompt the user for a number and set it to the number of guesses left
            player.GuessesLeft = PromptUserForNumberOfGuesses();

            //create the first word for the AI
            AI.GetAllWordsOfOneLength(WordManagement.AllWords, Convert.ToInt32(WordManagement.WordLength));

            //prompt the user if they want to see how many words are in the AI's initial word family
            PromptUserForTotalOfWordsInList();

            //set the appropriate number of dashes to the stringbuilder
            lines.Append(WordManagement.BlankOutWord(AI.ChosenWord.Length));

            //loop until gameOver is true
            while (!gameOver)
            {
                //write the number of guesses the player has left
                Console.WriteLine("Guesses left: " + player.GuessesLeft);

                //print the used letters to the console
                PrintUsedLetters();

                //print the lines string bulder
                Console.WriteLine("Word: " + lines);

                //read the guess from the user
                guess = player.MakeGuess();

                //change word family (cheat) here and then the chosen word based on the family chosen 

                //the guess if correct if the chosen word contains the guessed letter
                if (AI.ChosenWord.ToString().Contains(guess))
                {
                    //write to the console that the guess was correct
                    Console.WriteLine("Guess Correct! :)\n");

                    //add the guess to the correct letters 
                    player.CorrectLetters.Add(Convert.ToChar(guess));

                    //add the guess to used letters
                    player.UsedLetters.Add(Convert.ToChar(guess));

                    AI.RemoveWordWithUsedLetter(Convert.ToChar(guess), lines.ToString());

                    //loop through the lines stringbuilder
                    for (int i = 0; i < lines.Length; i++)
                    {
                        //for each letter of the chosen word, check if it is equal to the guess
                        if (AI.ChosenWord[i] == Convert.ToChar(guess))
                        {
                            /*set the appropriate element of the lines of the stringbuilder to the 
                            character of the corresponding chosen word*/

                            //check if the amount of guessed letters is equal to the lenght of the word -1 (one letter left to guess)
                            if (AI.GetAmountOfGuessedLetters(lines.ToString()) != AI.ChosenWord.Length - 1 || AI.CurrentWordFamily.Count == 1)
                            {
                                //if there is only one word left in the word family, do not change it.
                                lines[i] = AI.ChosenWord[i];
                            }
                            else
                            {
                                //if there is more than 1 word in the AI's current word family and there is only 1 letter left to guess
                                if (!AI.CheckIfNextWordHasIncorrectLetters(player))
                                {
                                    //change the word
                                    AI.ChangeChosenWordWithOneLetterLeft(lines.ToString());
                                }
                            }

                            //then check if the player has won (when there are no dashes left in the string builder)
                            if (CheckForWinner())
                            {
                                //set the game to 'over'
                                gameOver = true;
                            }
                        }
                    }

                    //after each correct guess, the AI creates a new word family
                    AI.CreateNewWordFamily(lines.ToString());

                    //removes all invald word from the family
                    AI.RemoveWordFromFamily(player);

                    //Testing 
                    Console.WriteLine("AI CHOSEN WORD: " + AI.ChosenWord);

                    //test purposes
                    Console.WriteLine("AI new list size: " + AI.CurrentWordFamily.Count);
                }
                //triggers when the AI's chosen word does not contain the guess
                else
                {
                    //write to the console that the guess is incorrect
                    Console.WriteLine("Guess Incorrect :'(\n");

                    //add the guess to the used letters list
                    player.UsedLetters.Add(Convert.ToChar(guess));

                    //add the guess to the list of incorrect guesses
                    player.IncorrectLetters.Add(Convert.ToChar(guess));

                    //remove the word with an incorrect letter in them from the family so they will not be changed to and lock the game
                    AI.RemoveWordFromFamily(player);

                    //decrement a guess from the player
                    player.GuessesLeft--;

                    /*if the player runs out of guesses, set gameOver to true and write to the 
                    console that the game is over */
                    if (player.GuessesLeft == 0)
                    {
                        //set gameover to true to stop the game
                        gameOver = true;

                        //print that the player has lost, along with the chosen word
                        Console.WriteLine("GAME OVER! You ran out of guesses." + " The word was: " + "'" + AI.ChosenWord + "'");

                        //prompt the user to replay the game
                        PromptUserToPlayAgain();

                        //pause the program
                        Console.ReadKey();
                    }
                }
            }
        }
        #endregion
    }
}
