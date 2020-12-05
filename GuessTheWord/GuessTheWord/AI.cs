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
        public StringBuilder ChosenWord = new StringBuilder();
        public StringBuilder subjectWordFamily = new StringBuilder();

        public List<string> CurrentWordFamily = new List<string>();
        public List<string> usedWordFamilies = new List<string>();

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
                    CurrentWordFamily.Add(_WMlist[i]);
                }
            }

            //to make the AI harder to beat, make the initial word random 
            ChosenWord.Append(CurrentWordFamily[
              0 // rng.Next(0, CurrentWordFamily.Count())
                ]);
        }

        /// <summary>
        /// Returns the number of correctly guessed letters in the AI's word
        /// </summary>
        /// <param name="lines">The partially or fully guessed word</param>
        /// <returns>Returns the amount of correctly guessed letters of the AI's chosen word</returns>
        public int GetAmountOfGuessedLetters(string lines)
        {
            //number of correctly guessed letters 
            int letterCount = 0;

            //loop though the lines stringbuilder
            for (int i = 0; i < lines.Length; i++)
            {
                //if any element is not a "-"
                if (lines[i].ToString() != "-")
                {
                    //increment the letter count
                    letterCount++;
                }
            }

            //return the amount of letters in the word
            return letterCount;
        }

        /// <summary>
        /// If a member of the AI's word family has an incorrect letter in it, remove it from the word family as 
        /// it is impossible to make it the AI's new chosen word as letters cannot be re-guessed
        /// </summary>
        /// <param name="player">The player with the incorrect guesses</param>
        public void RemoveWordFromFamily(Player player)
        {
            //loop through the AI's word family
            for (int i = 0; i < CurrentWordFamily.Count; i++)
            {
                //loop through each incorrect letter
                for (int j = 0; j < player.IncorrectLetters.Count; j++)
                {

                    //if the element of the word family contains a letter from the incorrect letters list
                    if (CurrentWordFamily[i].Contains(player.IncorrectLetters[j]))
                    {
                        //remove it from the word family
                        CurrentWordFamily.RemoveAt(i);

                        //clear the chosen word
                        ChosenWord.Clear();

                        //change the chosen word to the new word in position 0
                        ChosenWord.Append(i);
                    }
                }
            }
        }

        /// <summary>
        /// As to not cause a situation where a word is changed to one that contians 
        /// a letter that has already been guessed incorrectly, this method makes sure that does not happen 
        /// </summary>
        /// <param name="player">The player that has guessed incorrect letters</param>
        /// <returns></returns>
        public bool CheckIfNextWordHasIncorrectLetters(Player player)
        {
            //stores if there is an incorrect 
            bool containsALetter = false;

            //loop through the list of incorrect letters
            for (int i = 0; i < player.IncorrectLetters.Count; i++)
            {
                //if there is more than 1 word in the AI's word family
                if (CurrentWordFamily.Count > 1)
                {
                    //if the second element of the AI's word family contains any incorrect letters, return true
                    if (CurrentWordFamily[1].Contains(player.IncorrectLetters[i]))
                    {
                        containsALetter = true;
                    }
                }
            }

            //return if the word ccontains an incorrect letter
            return containsALetter;
        }

        /// <summary>
        /// To prevent a soft-lock, this method removes words that are ALL used letters so they cannot be changed to
        /// </summary>
        /// <param name="player">The player that owns the used letters to check against</param>
        /// <param name="lines">Used to determine how many lettered have been guessed correctly</param>
        public void RemoveWordWithUsedLetter(char guess, string lines)
        {
            //stores the number fo used letters in the word
            int usedLetterCount;

            //if it is the player first guess, don't remove any words
            if (GetAmountOfGuessedLetters(lines) != 0)
            {
                //loop through the AI's current word family
                for (int i = 0; i < CurrentWordFamily.Count; i++)
                {
                    //set the used letter count to 0 for each word
                    usedLetterCount = 0;

                    //loop through the player's used letters 
                    for (int j = 0; j < CurrentWordFamily[i].Length; j++)
                    {
                        //if the current word being checked contains the guessed letter
                        if (CurrentWordFamily[i].Substring(j, 1) == guess.ToString())
                        {
                            //increment the counter
                            usedLetterCount++;
                        }
                    }

                    //if there is more then 1 used letter in that word, remove it - it will break the game 
                    if (usedLetterCount >= 2)
                    {
                        //to make it harder for the player to guess, the word will never have repeating letters
                        if (ChosenWord.ToString() == CurrentWordFamily[i])
                        {
                            ChosenWord.Clear();
                            ChosenWord.Append(CurrentWordFamily[1]);
                        }

                        //testing
                        Console.WriteLine("REMOVED " + CurrentWordFamily[i]);

                        //remove the word from the family
                        CurrentWordFamily.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// Changes the chosen word so not to lose
        /// The word is to be able to change under a number of different circum 
        /// </summary>
        /// <param name="lines">The revealed word</param>
        public void ChangeChosenWordWithOneLetterLeft(string lines)
        {
            //if there is 1 letter left to guess, change the word
            if (GetAmountOfGuessedLetters(lines) == ChosenWord.Length - 1)
            {
                //check if there is more than 1 word in the AI's word family so there is a word to change to 
                if (CurrentWordFamily.Count > 1)
                {
                    //remove the word at the first element as it was the inital chosen word and the player would have won if it was not changed
                    CurrentWordFamily.RemoveAt(0);

                    //clear the chosen word
                    ChosenWord.Clear();

                    //append the new first word of the word family onto hte newWord stringbuilder
                    ChosenWord.Append(CurrentWordFamily[0]);
                }
            }
        }

        /// <summary>
        /// Gets all the words that do NOT contians the guess, which is a valid word family
        /// </summary>
        /// <param name="guess"></param>
        List<string> GetNoMatchList(char guess)
        {
            List<string> noMatchList = new List<string>();

            for (int i = 0; i < CurrentWordFamily.Count; i++)
            {
                if (!CurrentWordFamily[i].Contains(guess))
                {
                    noMatchList.Add(CurrentWordFamily[i]);
                }
            }

            return noMatchList;
        }

        //public void AddWordsToNewFamily(char guess, string lines)
        //{
        //    //word =    blow
        //    //word =    blob
        //    //family =  **o*
        //    int amountExpected = 0;
        //    int amountFound = 0;

        //    List<string> newWordFamily = new List<string>();

        //  //  CreateNewWordFamilyPositions(guess, lines);

        //    for (int i = 0; i < CurrentWordFamily.Count; i++)
        //    {
        //        for (int j = 0; j < WordManagement.WordLength.Length; j++)
        //        {
        //            if (CurrentWordFamily[i].Substring(j, 1) == Convert.ToString(subjectWordFamily.ToString().ElementAt(j)))
        //            {
        //                amountFound++;

        //                if (amountFound == amountExpected)
        //                {
        //                    newWordFamily.Add(CurrentWordFamily[i]);
        //                }
        //            }
        //        }
        //    }

        //    subjectWordFamily.Clear();
        //}

        void SetWordFamilies(StringBuilder lastWordFamily, StringBuilder subjectWordFamily, string lines)
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
                usedWordFamilies.Add(subjectWordFamily.ToString());
                lastWordFamily = subjectWordFamily;

                for (int i = 0; i < subjectWordFamily.Length; i++)
                {
                    subjectWordFamily[i] = '-';
                }
            }
        }

        /// <summary>
        /// Checks if a particualr word family has been used before
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

        /// <summary>
        /// Creates a new word family based on the inputted guess
        /// </summary>
        /// <param name="guess">The inputted guess from the user</param>
        /// <param name="lines">The parially revealed word</param>
        public void CreateNewWordFamily(char guess, string lines)
        {
            List<string> newWordFamily = new List<string>();

            StringBuilder lastWordFamily = new StringBuilder();

            //create a visulisation of the word subject word family 
            SetWordFamilies(lastWordFamily, subjectWordFamily, lines);

            //get the highest number of the occuring guess
            for (int i = 0; i < CurrentWordFamily.Count; i++)
            {
                //if the current word family element has the guess in it, loop through it
                if (CurrentWordFamily[i].Contains(guess))
                {
                    //loop through each letter of the word
                    for (int j = 0; j < CurrentWordFamily[i].Length; j++)
                    {
                        //if the word's letter is equal to the guess, make a word family out of it
                        if (CurrentWordFamily[i].Substring(j, 1) == guess.ToString() && subjectWordFamily[j] == '-') //breakpoint here
                        {
                            //if the element in the family is not already equal to the guess, make it so
                            if (subjectWordFamily[j] != guess)
                            {
                                subjectWordFamily[j] = guess;
                                //subjectWordFamily.Remove(subjectWordFamily.Length - 1, 1);
                            }
                        }
                    }
                }

                //add words to list here 
                if (subjectWordFamily != lastWordFamily && CheckIfFamilyHasBeenUsed(usedWordFamilies, subjectWordFamily) == false)
                {
                    //add the word to the new family
                    AddWordToNewFamily(subjectWordFamily, newWordFamily, guess);

                    //reset the word families
                    SetWordFamilies(lastWordFamily, subjectWordFamily, lines);
                }
            }
        }

        /// <summary>
        /// Choose between the potential word lists
        /// </summary>
        /// <param name="currentWordFamily"></param>
        /// <param name="newWordFamily"></param>
        /// <param name="guess"></param>
        void ChooseBestWordFamily(List<string> currentWordFamily, List<string> newWordFamily, List<string> noMatchWordFamily)
        {
            //newWordFamily is bigger
            if (currentWordFamily.Count < newWordFamily.Count)
            {
                //noMatchFamily is bigger 
                if (newWordFamily.Count < noMatchWordFamily.Count)
                {
                    CurrentWordFamily.Clear();
                    CurrentWordFamily.AddRange(noMatchWordFamily);
                }
                //newWordFamily is bigger
                else
                {
                    CurrentWordFamily.Clear();
                    CurrentWordFamily.AddRange(newWordFamily);
                }
           
            }
            //family 1 is bigger
            else
            {
                //no match list is bigger
                if (currentWordFamily.Count < noMatchWordFamily.Count)
                {
                    CurrentWordFamily.Clear();
                    CurrentWordFamily.AddRange(noMatchWordFamily);
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

            //loop through the current word family
            for (int i = 0; i < CurrentWordFamily.Count; i++)
            {
                //set the matching letters for 0
                matchingLetters = 0;

                //loop through each word in the word family
                for (int j = 0; j < CurrentWordFamily[i].Length; j++)
                {
                    //loop through each letter in each word of the word family and check each one that matches the word family letter and position
                    if (CurrentWordFamily[i].Substring(j, 1) == wordFamily[j].ToString())
                    {
                        //increment the matching letters count
                        matchingLetters++;

                        //if the matching letters are equal to the amount in the word family, add it to the new family list
                        if (matchingLetters == GetLettersInWordFamily(wordFamily))
                        {
                            CurrentWordFamily.RemoveAt(i);
                            newWordFamilyList.Add(CurrentWordFamily[i]);
                        }
                    }
                }
            }

            //when all the words are added to the word family list, pick the best list 
            ChooseBestWordFamily(newWordFamilyList, CurrentWordFamily, GetNoMatchList(guess));
        }

        /// <summary>
        /// Gets the amount of letters that are expected in each word family
        /// </summary>
        /// <param name="wordFamily">The word family (f---), for example</param>
        /// <returns></returns>
        int GetLettersInWordFamily(StringBuilder wordFamily)
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
