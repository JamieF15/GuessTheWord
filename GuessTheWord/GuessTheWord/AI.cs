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

            //add the first word of the word family as the initial chosen word as it is largely irrelevant what the first chosen word is 
            ChosenWord.Append(CurrentWordFamily[0]);
        }

        /// <summary>
        /// Returns the number of correctly guessed letters in the AI's word
        /// </summary>
        /// <param name="lines">The partially or fully guessed word</param>
        /// <returns>Returns the amount of correctly guessed letters of the AI's chosen word</returns>
        public int GetAmountOfGussedLetters(string lines)
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
                    }
                }

            }
        }

        /// <summary>
        /// As to not cause a situation where a word is changed to one that contians 
        /// a letter that has already been guessed incorrectly, this method makes sure that does not happen a
        /// </summary>
        /// <param name="player">The player that has guessed incorrect letters</param>
        /// <returns></returns>
        public bool CheckIfNextWordHasUsedLettersIn(Player player)
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
        /// Changes the chosen word so not to lose
        /// The word is to be able to change under a number of different circum 
        /// </summary>
        /// <param name="lines">The revealed word</param>
        public void ChangeChosenWordWithOneLetterLeft(string lines)
        {
            //acts as a buffer between the chosen word and the current word family
            //StringBuilder newWord = new StringBuilder();

            //if there is 1 letter left to guess, change the word
            if (GetAmountOfGussedLetters(lines) == ChosenWord.Length - 1)
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




                    //make the chosen word the new word
                    //ChosenWord.Append(newWord);

                    //clear the new word
                    //newWord.Clear();
                }
            }
        }

        /// <summary>
        /// Creates a new word family based on the revealed letters in lines string builder and the expected number of revealed letters 
        /// </summary>
        /// <param name="lines">The partially revealed word</param>
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
