﻿using Microsoft.VisualBasic.CompilerServices;
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
        static public List<string> AllWords = File.ReadAllLines(@"C:\Users\User\Documents\Code For Github\GuessTheWord\GuessTheWord\a.txt").ToList();
        #endregion

        #region Methods
        /// <summary>
        /// Prompts the user for a word length to search the AllWords list for
        /// </summary>
        /// <returns></returns>
        static public int PromptUserForWordLength()
        {
            bool stop = false;
            int parsed = -1;

            while (!stop)
            {
                Console.WriteLine("Enter a word length: ");
                WordLength = Console.ReadLine();

                if (!int.TryParse(WordLength.ToString(), out parsed))
                {
                    Console.WriteLine("Input was not a number.");
                }
                else 
                {
                    if (WordLength.Length > 0) 
                    {
                        Console.WriteLine("Enter a number.");
                    }
                    if (CheckWordList(WordLength))
                    {
                        return WordLength.Length;
                    }
                    else
                    {
                        Console.WriteLine("No words of that length exist.");
                    }

                }
            }
            return Convert.ToInt32(WordLength);
        }

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
        /// <param name="_wordLength"></param>
        /// <returns>Returns a bool stating if there is any words of the pasrsed in length</returns>
        static bool CheckWordList(string _wordLength)
        {
            //checks if there are any words of the desired length
            bool foundWords = false;
            //counts the number of valid words based on the length
            int validWords = 0;

            int wl = Convert.ToInt32(_wordLength);

            //loops through the AllWords list 
            for (int i = 0; i < AllWords.Count; i++)
            {
                /*check if each element is the length of the inputted length;
                true when the list element is the same size as the inputted length*/
                if (AllWords[i].Length ==  wl)
                {
                    //increment the valid word count
                    validWords++;
                    //if there is more than 1 word of the inputted length, this fires 
                    if (validWords > 0)
                    {
                        foundWords = true;
                    }
                    else
                    {
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
