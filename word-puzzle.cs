using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Load user marks from file
        Dictionary<string, int> userMarks = LoadUserMarks();

        // Welcome message
        Console.WriteLine("Welcome to the Word Puzzle Game!");

        // Display user's previous marks if available
        Console.Write("Enter your name: ");
        string userName = Console.ReadLine();
        if (userMarks.ContainsKey(userName))
        {
            Console.WriteLine("Your previous score: " + userMarks[userName]);
        }

        // List of words for the game
        string[] words = { "apple", "banana", "orange", "grape", "kiwi" };

        // Randomly choose a word
        Random random = new Random();
        string wordToGuess = words[random.Next(0, words.Length)];

        // Maximum number of attempts
        int maxAttempts = 5;
        int attemptsLeft = maxAttempts;

        // Initialize guessed word with underscores
        char[] guessedWord = new char[wordToGuess.Length];
        for (int i = 0; i < wordToGuess.Length; i++)
        {
            guessedWord[i] = '_';
        }

        // Game loop
        while (attemptsLeft > 0)
        {
            // Display guessed word
            Console.WriteLine("Word: " + string.Join(" ", guessedWord));

            // Prompt user for input
            Console.Write("Enter a letter: ");
            char guess = Console.ReadLine().ToLower()[0];

            // Check if the guessed letter is in the word
            bool correctGuess = false;
            for (int i = 0; i < wordToGuess.Length; i++)
            {
                if (wordToGuess[i] == guess)
                {
                    guessedWord[i] = guess;
                    correctGuess = true;
                }
            }

            // If the guess was incorrect, decrement attempts left
            if (!correctGuess)
            {
                attemptsLeft--;
                Console.WriteLine($"Incorrect guess! Attempts left: {attemptsLeft}");
            }

            // Check if the player has guessed the word
            if (string.Join("", guessedWord) == wordToGuess)
            {
                Console.WriteLine("Congratulations! You guessed the word: " + wordToGuess);

                // Update user marks
                if (!userMarks.ContainsKey(userName) || userMarks[userName] < attemptsLeft)
                {
                    userMarks[userName] = attemptsLeft;
                    SaveUserMarks(userMarks);
                }
                break;
            }
        }

        // If the player runs out of attempts
        if (attemptsLeft == 0)
        {
            Console.WriteLine("You ran out of attempts! The word was: " + wordToGuess);
        }
    }

    // Function to load user marks from file
    static Dictionary<string, int> LoadUserMarks()
    {
        Dictionary<string, int> userMarks = new Dictionary<string, int>();
        if (File.Exists("user_marks.txt"))
        {
            string[] lines = File.ReadAllLines("user_marks.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2)
                {
                    string userName = parts[0];
                    int marks = int.Parse(parts[1]);
                    userMarks[userName] = marks;
                }
            }
        }
        return userMarks;
    }

    // Function to save user marks to file
    static void SaveUserMarks(Dictionary<string, int> userMarks)
    {
        using (StreamWriter writer = new StreamWriter("user_marks.txt"))
        {
            foreach (var pair in userMarks)
            {
                writer.WriteLine($"{pair.Key},{pair.Value}");
            }
        }
    }
}
