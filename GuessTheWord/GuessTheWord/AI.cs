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

        //   public List<string> allWordsOfGivenLength = new List<string>();
        public List<string> usedWordFamilies = new List<string>();
        public List<string> currentWordFamily = new List<string>();
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

        public void SetWordFamilies(StringBuilder lastWordFamily, StringBuilder subjectWordFamily, string lines)
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
            else if (lastWordFamily != subjectWordFamily)
            {
                lastWordFamily.Clear();
                lastWordFamily.Append(subjectWordFamily);
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

        void RemoveWordsThatAreNotInChosenFamily(List<string> currentWordFamily, List<string> allWordsOfGivenLength)
        {
            for (int i = 0; i < allWordsOfGivenLength.Count; i++)
            {
                for (int j = 0; j < currentWordFamily.Count; j++)
                {
                    if (allWordsOfGivenLength.Count > 0 && currentWordFamily.Count > 0)
                    {
                        if (allWordsOfGivenLength[i] != currentWordFamily[j])
                        {
                            allWordsOfGivenLength.RemoveAt(i);
                        }
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

            int WordFamilySizeToCreate = 0;

            //create a visulisation of the word subject word family 
            SetWordFamilies(lastWordFamily, subjectWordFamily, lines);

            while (WordFamilySizeToCreate != Convert.ToInt32(WordManagement.WordLength))
            {
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
                            if (currentWordFamily[i].Substring(j, 1) == guess.ToString() && subjectWordFamily[j] == '-') //breakpoint here
                            {
                                //if the element in the family is not already equal to the guess, make it so
                                if (subjectWordFamily[j] != guess)
                                {
                                    subjectWordFamily[j] = guess;

                                    //add words to list here 
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
                }
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
            //check if the list with no matches is greater than the new one, it it is, choose the no match family
            if (noMatchWordFamily.Count > newWordFamily.Count)
            {
               // chosenWordFamily = lastWordFamily;
                currentWordFamily.Clear();
                currentWordFamily.AddRange(noMatchWordFamily);

                newWordFamily.Clear();

                //do not reveal letter
            }
            else if (newWordFamily.Count > noMatchWordFamily.Count)
            {
                chosenWordFamily = subjectWordFamily;
                lastWordFamily = subjectWordFamily;
                currentWordFamily.Clear();
                currentWordFamily.AddRange(newWordFamily);

                newWordFamily.Clear();

                //reveal letter
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
