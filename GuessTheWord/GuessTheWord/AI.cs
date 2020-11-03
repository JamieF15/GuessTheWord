using System;
using System.Collections.Generic;
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
                    CurrentWordFamily.Add(_WMlist[i]);
                }
            }

            //add the first word of the word family as the initial chosen word
            ChosenWord.Append(CurrentWordFamily[0]);
        }

        //public void ChangeWordFamily(char guessedLetter)
        //{
        //    for (int i = 0; i < CurentWordFamily.Count; i++)
        //    {
        //        if (CurentWordFamily[i].ToString())
        //        {

        //        }
        //    }

        //}
        #endregion

    }
}
