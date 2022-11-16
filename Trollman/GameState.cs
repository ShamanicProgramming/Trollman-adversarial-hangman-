using System;
using System.Collections.Generic;
using System.Text;

namespace Trollman
{
    class GameState
    {
        public List<char> wrongLetters = new List<char>();

        /// <summary>
        /// These are all words of our chosen word size so we can sub them in later when we cheat
        /// </summary>
        public List<string> wordsOfLetterSize = new List<string>();
        public Dictionary<int, char> correctGuesses = new Dictionary<int, char>();
        public string chosenWord;
    }
}
