using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GuessTheWord
{ 
    static public class WordManagement
    {
        #region Attributes
        public static string WordLength { get; set; }

        //reads the text file containing all of the AI's words 
        static public List<string> AllWords = File.ReadAllLines("a.txt").ToList();
        #endregion

        #region Methods
        /// <summary>
        /// Prompts the user for a word length to search the AllWords list for
        /// </summary>
        /// <returns></returns>
        static public int PromptUserForWordLength()
        {
            bool stop = false;

            //loop until stop is true
            while (!stop)
            {
                //prompts the user for the length of hte word to guess
                Console.WriteLine("Enter a word length: ");

                //read the input
                WordLength = Console.ReadLine();

                //check if the inputted value was a letter
                if (!int.TryParse(WordLength.ToString(), out int parsed))
                {
                    //display that it was a number
                    Console.WriteLine("Input was not a number.");
                }
                //if the input was a number this happens
                else
                {   //checks if the length of the word is greater than 0 and words of the inputted length are in the word list
                    if (WordLength.Length > 0 && CheckWordList(WordLength))
                    {
                        //returns the length of the word 
                        return Convert.ToInt32(WordLength);
                    }
                    //if there are no words of the inputted length in the list or the input is less than 0, this happesn 
                    else
                    {
                        //prompt the user tha there are no words of that length in the list
                        Console.WriteLine("No words of that length exist.");
                    }
                }
            }

            //return the wordlength as a integer
            return Convert.ToInt32(WordLength);
        }

        //NOT USED CURRENTLY
        public static List<string> GiveAIWordFamily()
        {
            List<string> WordFamily = new List<string>();
            
            for (int i = 0; i < AllWords.Count; i++)
            {
                if (AllWords[i].Length == Convert.ToInt32(WordLength))
                {
                    WordFamily.Add(AllWords[i]);
                }
            }
            return WordFamily;
        }

        /// <summary>
        /// Checks to see if AllWords list has any words of the inputted length
        /// </summary>
        /// <param name="_wordLength">The length of the chosen word</param>
        /// <returns>Returns a bool stating if there is any words of the pasrsed in length</returns>
        static bool CheckWordList(string _wordLength)
        {
            //checks if there are any words of the desired length
            bool foundWords = false;

            //counts the number of valid words based on the length
            int validWords = 0;

            //loops through the AllWords list 
            for (int i = 0; i < AllWords.Count; i++)
            {
                /*check if each element is the length of the inputted length;
                true when the list element is the same size as the inputted length*/
                if (AllWords[i].Length ==  Convert.ToInt32(_wordLength))
                {
                    //check if there are any empty lines in the word file before inrementing the valid work count
                    if (AllWords[i].Length > 0)
                    {
                        //increment the valid word count
                        validWords++;
                    }

                    //if there is more than 1 word of the inputted length, found words get set to true, if not, it is set to false
                    if (validWords > 0)
                    {
                        //set found words to true
                        foundWords = true;
                    }
                    //if there are no words of the desired length
                    else
                    {
                        //set found words to false
                        foundWords = false;
                    }
                }
            }
            return foundWords;
        }

        /// <summary>
        /// Prints a number of dashes (-) based on the length of the ai's chosen word
        /// </summary>
        /// <param name="wordLen">Length of the chosen word</param>
        /// <returns>Returns a number of dashes</returns>
        static public string BlankOutWord(int wordLen)
        {
            //string builder for blanked word
            StringBuilder blankedWord = new StringBuilder();

            //loop through the length of the chosen word
            for (int i = 0; i < wordLen; i++)
            {
                //adds a dash each interation
                blankedWord.Append("-");
            }
            //retuns the stringbuilder as a string
            return blankedWord.ToString();
        }
        #endregion
    }
}
