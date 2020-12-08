using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace GuessTheWord
{
    public class AI
    {
        #region Properties
        public StringBuilder subjectWordFamily = new StringBuilder();
        public StringBuilder lastWordFamily = new StringBuilder();
        public StringBuilder chosenWordFamily = new StringBuilder();

        public List<string> usedWordFamilies = new List<string>();
        public List<string> currentWordFamily = new List<string>();
        public List<string> bestWordFamily = new List<string>();
        #endregion

        #region Methods
        /// <summary>
        /// Creates the first word family based on the desired word length 
        /// </summary>
        /// <param name="_WMlist">The entire list of words</param>
        /// <param name="wordLen">The desired lenth of the words to guess</param>
        public void GetAllWordsOfOneLength(List<string> _WMlist, int wordLen)
        {
            Random rng = new Random();

            //loop through the list of all words 
            for (int i = 0; i < _WMlist.Count; i++)
            {
                //if the word in the word list is length of the desired word lengh, add it to hte AI's word family
                if (_WMlist[i].Length == wordLen)
                {
                    //add each word of the chosen length to the AI's first word family
                    currentWordFamily.Add(_WMlist[i]);
                }
            }
        }

        /// <summary>
        /// Gets all the words that do NOT contians the guess, which is a valid word family
        /// </summary>
        /// <param name="guess"></param>
        List<string> GetNoMatchList(char guess)
        {
            //holds the words that do not contain the guessed letter
            List<string> noMatchList = new List<string>();

            //clear the list every time this method is run
            noMatchList.Clear();

            //loop through the words of a given length
            for (int i = 0; i < currentWordFamily.Count; i++)
            {
                //for each one that does not contrain the guess, add it to the list
                if (!currentWordFamily[i].Contains(guess))
                {
                    noMatchList.Add(currentWordFamily[i]);
                }
            }

            return noMatchList;
        }

        public void SetWordFamilies(StringBuilder subjectWordFamily, string lines)
        {

            //if they are both empty, set them as all dashes
            if (lastWordFamily.Length == 0 && subjectWordFamily.Length == 0)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    lastWordFamily.Append("-");
                    subjectWordFamily.Append("-");
                }
            }

            //if they are different, set the last one to the subject one
            if (lastWordFamily != subjectWordFamily)
            {
               // chosenWordFamily = lastWordFamily;
            }
        }

        /// <summary>
        /// Checks if a particular word family has been used before
        /// </summary>
        /// <param name="usedWordFamilies">A list containg used word familes</param>
        /// <param name="subjectWordFamily">The current word family</param>
        /// <returns></returns>
        public bool CheckIfFamilyHasBeenUsed(List<string> usedWordFamilies, StringBuilder subjectWordFamily)
        {
            bool familyHasBeenUsed = false;

            for (int i = 0; i < usedWordFamilies.Count; i++)
            {
                if (usedWordFamilies[i] == subjectWordFamily.ToString())
                {
                    familyHasBeenUsed = true;
                }
            }

            return familyHasBeenUsed;
        }

        void SetSubjectWordFamily(char guess, int wordFamilySize)
        {

            for (int i = 0; i < currentWordFamily.Count; i++)
            {
                int matchingLetters = 0;

                for (int j = 0; j < currentWordFamily[i].Length; j++)
                {
                    if (currentWordFamily[i].Substring(j, 1) == guess.ToString())
                    {
                        matchingLetters++;
                    }

                    if (matchingLetters == wordFamilySize)
                    {
                        subjectWordFamily[j] = guess;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new word family based on the inputted guess
        /// </summary>
        /// <param name="guess">The inputted guess from the user</param>
        /// <param name="lines">The parially revealed word</param>
        public void CreateNewWordFamily(char guess, string lines)
        {
            List<string> newWordFamily = new List<string>();

            int WordFamilySizeToCreate = 1;

            //create a visulisation of the word subject word family 
            SetWordFamilies(subjectWordFamily, lines);

            while (WordFamilySizeToCreate != Convert.ToInt32(WordManagement.WordLength))
            {
                SetSubjectWordFamily(guess, WordFamilySizeToCreate);

                WordFamilySizeToCreate++;

                //get the highest number of the occuring guess
                for (int i = 0; i < currentWordFamily.Count; i++)
                {
                    //if the current word family element has the guess in it, loop through it
                    if (currentWordFamily[i].Contains(guess))
                    {
                        //loop through each letter of the word
                        for (int j = 0; j < currentWordFamily[i].Length; j++)
                        {
                            //if the word's letter is equal to the guess, make a word family out of it
                            if (currentWordFamily[i].Substring(j, 1) == subjectWordFamily.ToString().Substring(j, 1))
                            {
                                //add words to list here THIS CHECK COULD BE BREAKING IT
                                if (subjectWordFamily != lastWordFamily && CheckIfFamilyHasBeenUsed(usedWordFamilies, subjectWordFamily) == false)
                                {
                                    //add the word to the new family
                                    AddWordToNewFamily(subjectWordFamily, newWordFamily, guess);
                                    usedWordFamilies.Add(subjectWordFamily.ToString());
                                    break;
                                }
                            }
                        }
                    }
                }
                currentWordFamily.Clear();
                currentWordFamily.AddRange(bestWordFamily);
                chosenWordFamily = subjectWordFamily;
                bestWordFamily.Clear();

            }

        }

        /// <summary>
        /// Choose between the potential word lists
        /// </summary>
        /// <param name="newWordFamily1"></param>
        /// <param name="newWordFamily2"></param>
        /// <param name="guess"></param>
        void ChooseBestWordFamily(List<string> currentWordFamily, List<string> newWordFamily, List<string> noMatchWordFamily)
        {
            //do not reveal letter
            //check if the list with no matches is greater than the new one, it it is, choose the no match family
            if (noMatchWordFamily.Count >= newWordFamily.Count)
            {
                if (newWordFamily.Count > 0)//bestWordFamily.Count)
                {
                    bestWordFamily.Clear();
                    bestWordFamily.AddRange(noMatchWordFamily);
                    newWordFamily.Clear();
                }
            }
            //reveal letter
            else
            {
                if (newWordFamily.Count >= currentWordFamily.Count)
                {
                    if (noMatchWordFamily.Count > bestWordFamily.Count)
                    {
                        bestWordFamily.Clear();
                        bestWordFamily.AddRange(noMatchWordFamily);
                        newWordFamily.Clear();
                    }
                    else
                    {
                        bestWordFamily.Clear();
                        bestWordFamily.AddRange(newWordFamily);
                        newWordFamily.Clear();
                    }
                }
                else
                {
                    lastWordFamily = chosenWordFamily;
                    newWordFamily.Clear();
                }
            }
        }

        /// <summary>
        /// Adds words to a new list based on what letters are in the subject word family
        /// </summary>
        /// <param name="wordFamily">The letters that are being checked against, e.g. f---</param>
        /// <param name="newWordFamilyList">The list the new words are being added to</param>
        void AddWordToNewFamily(StringBuilder wordFamily, List<string> newWordFamilyList, char guess)
        {
            //the matching letter in each word of the word family
            int matchingLetters;

            int lettersInWordFamily = GetAmountOfLettersInWordFamily(wordFamily);

            //loop through the current word family
            for (int i = 0; i < currentWordFamily.Count; i++)
            {
                //set the matching letters for 0
                matchingLetters = 0;

                //loop through each word in the word family
                for (int j = 0; j < currentWordFamily[i].Length; j++)
                {
                    //loop through each letter in each word of the word family and check each one that matches the word family letter and position
                    if (currentWordFamily[i].Substring(j, 1) == wordFamily[j].ToString())
                    {
                        //increment the matching letters count
                        matchingLetters++;

                        //if the matching letters are equal to the amount in the word family, add it to the new family list
                        if (matchingLetters == lettersInWordFamily)
                        {
                            newWordFamilyList.Add(currentWordFamily[i]);
                        }
                    }
                }
            }

            //when all the words are added to the word family list, pick the best list 
            ChooseBestWordFamily(currentWordFamily, newWordFamilyList, GetNoMatchList(guess));
        }

        /// <summary>
        /// Gets the amount of letters that are expected in each word family
        /// </summary>
        /// <param name="wordFamily">The word family (f---), for example</param>
        /// <returns></returns>
        int GetAmountOfLettersInWordFamily(StringBuilder wordFamily)
        {
            //the number of letter that are not '-' in the family
            int lettersInWordFamily = 0;

            //loop through the word family
            for (int i = 0; i < subjectWordFamily.Length; i++)
            {
                //for each letter thats not '-', increment the counter
                if (subjectWordFamily[i] != '-')
                {
                    lettersInWordFamily++;
                }
            }

            return lettersInWordFamily;
        }
        #endregion
    }
}
