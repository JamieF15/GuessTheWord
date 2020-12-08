using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace GuessTheWord
{
    public class Player
    {
        #region Properties
        public int GuessesLeft { get; set; }

        public List<char> CorrectLetters = new List<char>();
        public List<char> UsedLetters = new List<char>();
        public List<char> IncorrectLetters = new List<char>();
        #endregion

        #region Methods
        /// <summary>
        /// Prompts the user for a guess and has logic to determine if the guess was valid
        /// </summary>
        /// <returns>Returns the user's guess in the form of a char </returns>
        public string MakeGuess()
        {
            //variable for the users guess
            string guess = "";

            //check to end the loop
            bool end = false;

            //checks if the letter has already been guessed
            bool usedLetter;

            //Loop while end is false
            while (!end)
            {
                //reset the usedLetter bool each iteration of the loop
                usedLetter = false;

                //write to the conole a prompt
                Console.Write("Enter a letter: ");

                //read the input from the user
                guess = Console.ReadLine();

                //code refrenced from 'https://stackoverflow.com/questions/34616050/how-to-check-if-a-char-is-in-the-alphabet-in-c-sharp'
                //check if the inputted letter is a part of the alphabet
                bool alphabetCheck = Regex.IsMatch(guess.ToString(), "[a-z]", RegexOptions.IgnoreCase);

                //if the input is exactly 1 and is in the alphabet, this is true
                if (guess.Length == 1 && alphabetCheck)
                {
                    //loop through the list of used letters 
                    for (int i = 0; i < UsedLetters.Count; i++)
                    {
                        //if the guess is equal to any elements in that list, it has been used
                        if (Convert.ToChar(guess) == UsedLetters[i])
                        {
                            //set the letter to used
                            usedLetter = true;

                            //stop the loop
                            break;
                        }
                    }
                }

                //true if the guess is 1 character in length, is in the alphabet, and has not been used
                if (guess.Length == 1 && alphabetCheck && !usedLetter)
                {
                    //return the guess as lower case to allow for both uppercase and lower case to be valid
                    return guess.ToLower();
                }

                //Outputs for when a letter is incorrect
                else
                {
                    //if the letter is used, prompt the user that is the case
                    if (usedLetter)
                    {
                        Console.WriteLine("'" + guess + "'" + " has already been guessed.\n");
                    }

                    //if the guess is empty, prompt the user that is the case
                    else if (guess.Length == 0)
                    {
                        Console.WriteLine("Guess is empty.\n");
                    }

                    //if the letter is not in the alphabet, prompt the user that is the case
                    else if (!alphabetCheck)
                    {
                        Console.WriteLine("Guess not a valid character.\n");
                    }

                    //if the letter longer than one character, prompt the user that is the case
                    else if (guess.Length > 1)
                    {
                        Console.WriteLine("Only enter one character.\n");
                    }
                }
            }
            return guess;
        }
        #endregion
    }
}
