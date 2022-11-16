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

            GameState gameState = new GameState();

            foreach (string word in allWords)
            {
                if (word.Length == numberOfLetters)
                {
                    gameState.wordsOfLetterSize.Add(word);
                }
            }

            // Get some word to start
            gameState.chosenWord = gameState.wordsOfLetterSize[rnd.Next(0, gameState.wordsOfLetterSize.Count)];

            string clue = new string('-', numberOfLetters);

            Console.WriteLine("Welcome to hangman.");
            Console.WriteLine("Your clue is: " + clue);

            List<string> hangmanPics = new List<string>();

            hangmanPics.Add("    ┌────────┐    \r\n    │        │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n        ─────┴─────");
            hangmanPics.Add("    ┌────────┐    \r\n    │        │    \r\n    O        │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n        ─────┴─────");
            hangmanPics.Add("    ┌────────┐    \r\n    │        │    \r\n    O        │    \r\n    │        │    \r\n    │        │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n        ─────┴─────");
            hangmanPics.Add("    ┌────────┐    \r\n    │        │    \r\n    O        │    \r\n   ─┼─       │    \r\n    │        │    \r\n             │    \r\n             │    \r\n             │    \r\n             │    \r\n        ─────┴─────");
            hangmanPics.Add("    ┌────────┐    \r\n    │        │    \r\n    O        │    \r\n   ─┼─       │    \r\n    │        │    \r\n   / \\       │    \r\n             │    \r\n             │    \r\n             │    \r\n        ─────┴─────");

            if (GameLoop(rnd, gameState, hangmanPics))
            {
                Console.WriteLine("You win");
            } else
            {
                Console.WriteLine("You lose");
            }
        }

        /// <summary>
        /// Returns true if the player wins
        /// </summary>
        private static bool GameLoop(Random rnd, GameState gameState, List<string> hangmanPics)
        {
            foreach (var hangPic in hangmanPics)
            {

                Console.WriteLine(hangPic);

                char guess = GetValidGuess();
                gameState.wrongLetters.Add(guess);

                if (gameState.chosenWord.Contains(guess))
                {
                    List<string> newWords = gameState.wordsOfLetterSize.Where(word =>
                    !word.Contains(guess) // new words shouldn't have the guessed letter
                    && gameState.wrongLetters.All(letter => !word.Contains(letter)) // new words shouldn't have any wrong letters
                    && WordContainsAllCorrectGuesses(word, gameState.correctGuesses)) // new words need all previous correct guesses
                        .ToList();

                    // if we can find some other word that doesn't have the guessed letter and does have successful letters, cheat and sub it in
                    if (newWords.Count != 0)
                    {
                        gameState.wordsOfLetterSize = newWords;
                        gameState.chosenWord = gameState.wordsOfLetterSize[rnd.Next(0, gameState.wordsOfLetterSize.Count)];
                        WrongGuess(gameState.wrongLetters, guess);
                    }
                    else
                    {
                        // otherwise we will have to keep the word and add to correct guesses
                        foreach (int indx in gameState.chosenWord.AllIndexesOf(guess))
                        {
                            gameState.correctGuesses.Add(indx, guess);
                        }

                        if (gameState.chosenWord.Length == gameState.correctGuesses.Count)
                        {
                            return true;
                        }
                        else
                        {
                            Console.Write("Correct. Your clue is now: ");
                        }

                    }
                }
                else
                {
                    WrongGuess(gameState.wrongLetters, guess);
                }

                // Write out a char of the clue
                for (int i = 0; i < gameState.chosenWord.Length; i++)
                {
                    char nextCharToWrite = gameState.correctGuesses.GetValueOrDefault(i);
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

            return false;
        }

        private static char GetValidGuess()
        {
            while(true)
            {
                Console.Write("Make your guess: ");
                string guess = Console.ReadLine();
                if(guess.Length != 1)
                {
                    Console.WriteLine("Invalid guess. Guess exactly one character.");
                    continue;
                }
                else if(!char.IsLetter(guess[0]))
                {
                    Console.WriteLine("Invalid guess. Guess must be a character.");
                    continue;
                }
                return guess[0];
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
