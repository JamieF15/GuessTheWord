using System;
using System.Collections.Generic;
using System.Text;

namespace GuessTheWord
{   
    public class AI
    {
        #region Properties
        public StringBuilder ChosenWord = new StringBuilder();
        public List<string> CurentWordFamily = new List<string>();
        #endregion

        #region Methods

        /// <summary>
        /// Creates the first word family based on the desired word length 
        /// </summary>
        /// <param name="_WMlist"></param>
        /// <param name="wordLen"></param>
        public void CreateFirstWordFamily(List<string> _WMlist, int wordLen)
        {
            for (int i = 0; i < _WMlist.Count; i++)
            {
                if (_WMlist[i].Length == wordLen)
                {
                    CurentWordFamily.Add(_WMlist[i]);
                }
            }
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
