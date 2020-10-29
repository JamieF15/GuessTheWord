using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GuessTheWord
{
    public class WordManagement
    {
        #region Attributes

        //reads the text file containing all of the AI's words 
        public List<string> AllWords = File.ReadAllLines(@"C:\Users\User\Documents\Code For Github\GuessTheWord\GuessTheWord\a.txt").ToList();

       // public string[] AllWords = File.ReadAllLines(@"C:\Users\User\Documents\Code For Github\GuessTheWord\GuessTheWord\a.txt");

        #endregion

        #region Methods
        /// <summary>
        /// Prints a number of dashes (-) based on the length of the ai's chosen word
        /// </summary>
        /// <param name="wordLen">Length of the chosen word</param>
        /// <returns>Returns a number of dashes</returns>
        public string BlankOutWord(int wordLen)
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
