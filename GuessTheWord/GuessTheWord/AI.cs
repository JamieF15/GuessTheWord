using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace GuessTheWord
{
    public class AI
    {
        #region Properties
        public StringBuilder ChosenWord = new StringBuilder();
        public List<string> CurrentWordFamily = new List<string>();
        #endregion

        #region Methods
        /// <summary>
        /// Creates the first word family based on the desired word length 
        /// </summary>
        /// <param name="_WMlist">The entire list of words</param>
        /// <param name="wordLen">The desired lenth of the words to guess</param>
        public void CreateFirstWordFamily(List<string> _WMlist, int wordLen)
        {
            for (int i = 0; i < _WMlist.Count; i++)
            {
                //if the word in the word list is length of the desired  word lengh, add it to hte AI's word family
                if (_WMlist[i].Length == wordLen)
                {
                    //add each word of the chosen length to the AI's first word family
                    CurrentWordFamily.Add(_WMlist[i]);
                }
            }

            //add the first word of the word family as the initial chosen word as it is largely irrelevant what the firs word is 
            ChosenWord.Append(CurrentWordFamily[0]);
        }

        public void CreateNewWordFamily(string lines)
        {
            //create a new list to store words in
            List<string> newWordFamily = new List<string>();

            //loop through the list of words in the family
            for (int i = 0; i < CurrentWordFamily.Count; i++)
            {
                //loop through each individual word
                for (int j = 0; j < CurrentWordFamily[i].Length; j++)
                {
                    //check if each letter of each word is equal to each revealed letter of the lines string builder
                    if (CurrentWordFamily[i].Substring(j, 1) == lines.ToString().Substring(j, 1) && lines.ToString().Substring(j, 1) != "-")
                    {
                        /*if the word contains the same letters in the same positions of revealed letters
                        in the lines stringbuilder, add it to the new word family*/
                        newWordFamily.Add(CurrentWordFamily[i]);
                    }
                }
            }
            //set the AI's word family to the new one
            CurrentWordFamily.Clear();
            CurrentWordFamily.AddRange(newWordFamily);
        }

        //public void ChangeWordFamily(char guessedLetter, string chosenWord)
        //{
        //    for (int i = 0; i < CurrentWordFamily.Count; i++)
        //    {
        //        if (CurrentWordFamily[i] )
        //        {

        //        }
        //    }
        //}
        #endregion

    }
}
