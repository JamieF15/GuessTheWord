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
            //loop throguh the list of all words 
            for (int i = 0; i < _WMlist.Count; i++)
            {
                //if the word in the word list is length of the desired word lengh, add it to hte AI's word family
                if (_WMlist[i].Length == wordLen)
                {
                    //add each word of the chosen length to the AI's first word family
                    CurrentWordFamily.Add(_WMlist[i]);
                }
            }

            //add the first word of the word family as the initial chosen word as it is largely irrelevant what the first word is 
            ChosenWord.Append(CurrentWordFamily[0]);
        }

        /// <summary>
        /// Changes the chosen word so not to lose
        /// </summary>
        /// <param name="lines">The revealed word</param>
        public void ChangeChosenWord(string lines)
        {
            if (!lines.Contains("-"))
            {
                if (CurrentWordFamily.Count > 1)
                {
                    ChosenWord.Clear();
                    CurrentWordFamily.RemoveAt(0);
                    ChosenWord.Append(CurrentWordFamily[0]);
                }
            }
        }

        /// <summary>
        /// Creates a new word family based on the revealed letters in lines string builder and the expected number of revealed letters 
        /// </summary>
        /// <param name="lines">The partially reavled word</param>
        public void CreateNewWordFamily(string lines)
        {
            //create a new list to store words in
            List<string> newWordFamily = new List<string>();

            //stores the amount of revealed letters in the lines stringbuilder
            int revealedLetters = 0;

            //stores the amount of revealed letters in the chosen word
            int revealedLettersInWord;

            //loop through the 'lines' stringbuilder and check how many letters each word needs to have revealed 
            for (int i = 0; i < lines.Length; i++)
            {
                //if the subject element of the lines stringbuilder is not a dash (a revealed letter)
                if (lines[i].ToString() != "-")
                {
                    //increment the expected number of revealedLetters for a word to be valid
                    revealedLetters++;
                }
            }

            //loop through the list of words in the family
            for (int i = 0; i < CurrentWordFamily.Count; i++)
            {
                //reset the amount of letters that are revealed in a word for each iteration 
                revealedLettersInWord = 0;

                //loop through each individual word
                for (int j = 0; j < CurrentWordFamily[i].Length; j++)
                {
                    //check if each letter of each word is equal to each revealed letter of the lines string builder
                    if (CurrentWordFamily[i].Substring(j, 1) == lines.ToString().Substring(j, 1) && lines.ToString().Substring(j, 1) != "-")
                    {
                        /*for each letter being looped through, increment the revealed letters in word counter 
                        for each letter and position is has in common with the 'lines' stringbuilder*/
                        revealedLettersInWord++;

                        //if the revealed letters in the word is the same as the expected amount 
                        if (revealedLettersInWord == revealedLetters)
                        {
                            //add the word to the new word family
                            newWordFamily.Add(CurrentWordFamily[i]);
                        }
                    }
                }
            }

            //clear the current word family
            CurrentWordFamily.Clear();

            //add the new word family to the empty current word family 
            CurrentWordFamily.AddRange(newWordFamily);
        }
        #endregion
    }
}
