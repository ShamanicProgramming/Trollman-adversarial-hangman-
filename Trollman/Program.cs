using System;
using System.Collections.Generic;
using System.Linq;
using Trollman.Properties;

namespace Trollman
{
    internal class Program
    {

        static void Main(string[] args)
        {

            List<string> allWords = Resources.worddict.Split().ToList();

            Random rnd = new Random();
            int numberOfLetters = rnd.Next(3, 15);

            // Find all words of our chosen word size so we can sub them in later when we cheat
            List<string> wordsOfLetterSize = new List<string>();

            foreach(string word in allWords)
            {
                if(word.Length == numberOfLetters)
                {
                    wordsOfLetterSize.Add(word);
                }
            }

            List<char> wrongLetters = new List<char>();

            // Get some word to start
            var chosenWord = wordsOfLetterSize[rnd.Next(0, wordsOfLetterSize.Count)];

            Dictionary<int,char> correctGuesses = new Dictionary<int, char>();
            string clue = new string('-', numberOfLetters);

            Console.WriteLine("Welcome to hangman.");
            Console.WriteLine("Your clue is: " + clue);

            var hangman1 = "    ┌────────┐    \r\n    │        │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n        ─────┴─────";
            var hangman2 = "    ┌────────┐    \r\n    │        │    \r\n    O        │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n        ─────┴─────";
            var hangman3 = "    ┌────────┐    \r\n    │        │    \r\n    O        │    \r\n    │        │    \r\n    │        │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n        ─────┴─────";
            var hangman4 = "    ┌────────┐    \r\n    │        │    \r\n    O        │    \r\n   ─┼─       │    \r\n    │        │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n        ─────┴─────";
            var hangman5 = "    ┌────────┐    \r\n    │        │    \r\n    O        │    \r\n   ─┼─       │    \r\n    │        │    \r\n   / \\       │    \r\n             │    \r\n             │    \r\n             │    \r\n        ─────┴─────";

            List<string> HangmanPics = new List<string> { hangman2 , hangman3, hangman4, hangman5};

            Console.WriteLine(hangman1);
            
            while(true)
            {
                Console.Write("Make your guess: ");
                char guess = Console.ReadLine()[0];
                wrongLetters.Add(guess);

                if (chosenWord.Contains(guess))
                {
                    List<string> newWords = wordsOfLetterSize.Where(word =>
                    !word.Contains(guess) // new words shouldn't have the guessed letter
                    && wrongLetters.All(letter => !word.Contains(letter)) // new words shouldn't have any wrong letters
                    && WordContainsAllCorrectGuesses(word, correctGuesses)) // new words need all previous correct guesses
                        .ToList();

                    // if we can find some other word that doesn't have the guessed letter and does have successful letters, cheat and sub it in
                    if (newWords.Count != 0)
                    {
                        wordsOfLetterSize = newWords;
                        chosenWord = wordsOfLetterSize[rnd.Next(0, wordsOfLetterSize.Count)];
                        WrongGuess(wrongLetters, guess);
                    }
                    else
                    {
                        // otherwise we will have to keep the word and add to correct guesses
                        foreach (int indx in chosenWord.AllIndexesOf(guess))
                        {
                            correctGuesses.Add(indx, guess);
                        }

                        if (chosenWord.Length == correctGuesses.Count)
                        {
                            Console.WriteLine("You win");
                            continue;
                        }
                        else 
                        {
                            Console.Write("Correct. Your clue is now: ");
                        }
                        
                    }
                }
                else
                {
                    WrongGuess(wrongLetters, guess);
                }

                // Write out a char of the clue
                for (int i = 0; i < chosenWord.Length; i++)
                {
                    char nextCharToWrite = correctGuesses.GetValueOrDefault(i);
                    if (nextCharToWrite == '\0')
                    {
                        Console.Write('-');
                    }
                    else
                    {
                        Console.Write(nextCharToWrite);
                    }
                }
                Console.WriteLine("");

            }
        }

        private static void WrongGuess(List<char> wrongLetters, char guess)
        {
            Console.WriteLine("Guess was wrong");
            wrongLetters.Add(guess);
        }

        private static bool WordContainsAllCorrectGuesses(string word, Dictionary<int, char> correctGuesses)
        {
            foreach(int i in correctGuesses.Keys)
            {
                if (word[i] != correctGuesses[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
