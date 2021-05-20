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
        public List<string> newWordFamily = new List<string>();

        #endregion

        #region Methods

        /// <summary>
        /// Creates the first word family based on the desired word length 
        /// </summary>
        /// <param name="allWords">The entire list of words</param>
        /// <param name="wordLen">The desired lenth of the words to guess</param>
        public void GetAllWordsOfOneLength(List<string> allWords, int wordLen)
        {
            //loop through the list of all words 
            for (int i = 0; i < allWords.Count; i++)
            {
                //if the word in the word list is length of the desired word lengh, add it to hte AI's word family
                if (allWords[i].Length == wordLen)
                {
                    //add each word of the chosen length to the AI's first word family
                    currentWordFamily.Add(allWords[i]);
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
                    //add the word to the list
                    noMatchList.Add(currentWordFamily[i]);
                }
            }

            return noMatchList;
        }

        /// <summary>
        /// Sets the word families to all dashes
        /// </summary>
        /// <param name="subjectWordFamily">The word family that has been chosen based on the guess</param>
        /// <param name="lines">The partially revealed (or not) word</param>
        public void SetWordFamilies(StringBuilder subjectWordFamily, string lines)
        {
            //if they are both empty, set them as all dashes
            if (lastWordFamily.Length == 0 && subjectWordFamily.Length == 0 && chosenWordFamily.Length == 0)
            {
                //loop through the lengths of the lines stringbuilder and set all the word families to a certain amount of dashes
                for (int i = 0; i < lines.Length; i++)
                {
                    lastWordFamily.Append("-");
                    subjectWordFamily.Append("-");
                    chosenWordFamily.Append("-");
                }
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
            //states if the word family has been used
            bool familyHasBeenUsed = false;

            //loop throuh the list of used word families
            for (int i = 0; i < usedWordFamilies.Count; i++)
            {
                //if the subject word family is equal to any of them, set the boolean to true
                if (usedWordFamilies[i] == subjectWordFamily.ToString())
                {
                    familyHasBeenUsed = true;
                }
            }

            return familyHasBeenUsed;
        }

        /// <summary>
        /// Sets the subject word family based on the guess and the intended size of the word family
        /// </summary>
        /// <param name="guess">The guess from the player</param>
        /// <param name="wordFamilySize">The size of the word family</param>
        void SetSubjectWordFamily(char guess, int wordFamilySize)
        {
            //loop through the current word family list
            for (int i = 0; i < currentWordFamily.Count; i++)
            {
                // set the matching letters of each word to 0
                int matchingLetters = 0;

                //loop through each word
                for (int j = 0; j < currentWordFamily[i].Length; j++)
                {
                    //each letter that is the guess, increment the matchingLetters counter
                    if (currentWordFamily[i].Substring(j, 1) == guess.ToString())
                    {
                        matchingLetters++;
                    }

                    //if the matching letters count is greater or equal to the wordFamilySize, make the subject word family at position j equal to the word at position j
                    if (matchingLetters >= wordFamilySize)
                    {
                        subjectWordFamily[j] = guess;

                        break;
                    }
                }

                //if a word family has been found, break from this loop
                if (GetAmountOfLettersInWordFamily() == wordFamilySize)
                {
                    break;
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
            //states the amount of words are to be in the word family
            int WordFamilySizeToCreate;

            //states whether the word families need to be updated 
            bool updateWordFamilies;

            //create a visulisation of the word subject word family 
            SetWordFamilies(subjectWordFamily, lines);

            WordFamilySizeToCreate = 1;

            //loop through the list of words until the word family to create is greater than the length of the words
            while (WordFamilySizeToCreate != Convert.ToInt32(WordManagement.WordLength))
            {
                //create the subject word family to base the chosen words off of
                SetSubjectWordFamily(guess, WordFamilySizeToCreate);

                //increment the amount of letters the word family is to be made of
                WordFamilySizeToCreate++;

                //set the update families to false
                updateWordFamilies = false;

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
                                    AddWordToNewFamily(newWordFamily, guess);

                                    //add the word family to the used word families list
                                    usedWordFamilies.Add(subjectWordFamily.ToString());

                                    //set the word families to be updated
                                    updateWordFamilies = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                //happens when the word families need to be updated
                if (updateWordFamilies)
                {
                    //clear the current word family
                    currentWordFamily.Clear();

                    //set the current word family to the best word family
                    currentWordFamily.AddRange(bestWordFamily);

                    //set the chosen word family to the subject word family
                    chosenWordFamily = subjectWordFamily;

                    //clear the best word family for the next loop interation
                    bestWordFamily.Clear();

                    //update the word family
                    UpdateWordFamily(chosenWordFamily);
                }
            }
        }

        /// <summary>
        /// Updates the subject word family based on what was secretly chosen
        /// </summary>
        /// <param name="wordFamily">The word family to update</param>
        void UpdateWordFamily(StringBuilder wordFamily)
        {
            //loopthrough the word family
            for (int i = 0; i < wordFamily.Length; i++)
            {
                //each letter in the word family triggers this
                if (wordFamily[i] != '-')
                {
                    //set the element in the word family that are not dashes to the letter 
                    //of the current word family to compensate for picking a 'no match list'
                    wordFamily[i] = Convert.ToChar(currentWordFamily[0].Substring(i, 1));
                }
            }
        }

        /// <summary>
        /// Chooses out of the possible word families and picks the best one
        /// </summary>
        /// <param name="currentWordFamily">The family used by the AI</param>
        /// <param name="newWordFamily">The last new word family</param>
        /// <param name="noMatchWordFamily">The family that does not contain the guess</param>
        void ChooseBestWordFamily(List<string> currentWordFamily, List<string> newWordFamily, List<string> noMatchWordFamily)
        {
            //do not reveal letter
            //check if the list with no matches is greater than the new one, if it is, choose the no match family
            if (noMatchWordFamily.Count >= newWordFamily.Count && currentWordFamily.Count > 0)
            {
                //if the no match family is bigger than the best word family, and the current word family is greater than 0
                if (noMatchWordFamily.Count >= bestWordFamily.Count && currentWordFamily.Count > 0)
                {
                    //clear the best word family
                    bestWordFamily.Clear();

                    //set the best word family to the no match word family
                    bestWordFamily.AddRange(noMatchWordFamily);

                    //clear the new word family for the next iteration
                    newWordFamily.Clear();

                    //testing
                    Console.WriteLine(bestWordFamily[0]);
                }
            }

            //reveal letter
            else
            {
                //if the new word family is greater than the best word family and the current word family is greater than 0
                if (newWordFamily.Count > bestWordFamily.Count && currentWordFamily.Count > 0)
                {
                    //clear the best word family
                    bestWordFamily.Clear();

                    //set the best word family to the new word family
                    bestWordFamily.AddRange(newWordFamily);

                    //clear the new word family for the next iteration
                    newWordFamily.Clear();
                }
            }
        }

        /// <summary>
        /// Adds words to a new list based on what letters are in the subject word family
        /// </summary>
        /// <param name="wordFamily">The letters that are being checked against, e.g. f---</param>
        /// <param name="newWordFamilyList">The list the new words are being added to</param>
        void AddWordToNewFamily(List<string> newWordFamilyList, char guess)
        {
            //the matching letter in each word of the word family
            int matchingLetters;

            int lettersInWordFamily = GetAmountOfLettersInWordFamily();

            //loop through the current word family
            for (int i = 0; i < currentWordFamily.Count; i++)
            {
                //set the matching letters for 0
                matchingLetters = 0;

                //loop through each word in the word family
                for (int j = 0; j < currentWordFamily[i].Length; j++)
                {
                    //loop through each letter in each word of the word family and check each one that matches the word family letter and position
                    if (currentWordFamily[i].Substring(j, 1) == subjectWordFamily[j].ToString())
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
        /// <returns></returns>
        int GetAmountOfLettersInWordFamily()
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
