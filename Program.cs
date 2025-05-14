// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace CybersecurityChatbot
{
    class Program
    {
        static string userName = "";
        static string favoriteTopic = "";
        static Random rnd = new Random();
        static List<string> searchHistory = new List<string>();

        static Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>()
        {
            { "password", new List<string> { "Use strong, unique passwords for each account.", "Avoid using personal details in your passwords.", "Consider using a password manager." } },
            { "scam", new List<string> { "Scammers impersonate trusted organizations.", "Don't click suspicious links.", "Check for grammar errors in scam messages." } },
            { "privacy", new List<string> { "Review privacy settings.", "Limit personal info online.", "Use encrypted messaging apps." } },
            { "phishing", new List<string> { "Be cautious of suspicious emails.", "Check sender addresses.", "Phishing creates urgency to trick you." } },
            { "malware", new List<string> { "Install antivirus software.", "Avoid untrusted downloads.", "Beware of forced downloads." } },
            { "firewall", new List<string> { "Enable firewalls.", "Monitor traffic with firewall.", "Firewalls add protection." } },
            { "updates", new List<string> { "Keep everything updated.", "Updates fix vulnerabilities.", "Enable auto-updates." } },
            { "backup", new List<string> { "Back up data regularly.", "Use cloud/external storage.", "Backups help against ransomware." } },
            { "social engineering", new List<string> { "Don't share info over phone.", "Be cautious with questions.", "It uses manipulation and trust." } },
            { "two-factor authentication", new List<string> { "Enable 2FA for safety.", "2FA adds another security step.", "Use SMS or authenticator apps." } },
            { "ransomware", new List<string> { "Back up to avoid ransomware loss.", "Avoid unknown links.", "Use antivirus software." } },
            { "vpn", new List<string> { "Use a VPN for privacy.", "Encrypts your connection.", "Avoid free VPNs with weak security." } },
            { "cyberbullying", new List<string> { "Block/report bullies.", "Document evidence.", "Talk to trusted individuals." } },
            { "cookies", new List<string> { "Manage cookies settings.", "Clear cookies regularly.", "Avoid blindly accepting cookies." } },
            { "encryption", new List<string> { "Encryption protects data.", "Use encrypted email.", "Encrypt your devices." } }
        };

        static Dictionary<string, string> emotionResponses = new Dictionary<string, string>()
        {
            { "worried", "It's okay to feel that way. Let's stay safe together." },
            { "curious", "Love your curiosity! Let's explore more." },
            { "frustrated", "It’s confusing sometimes, but you’re doing great." },
            { "happy", "Glad to hear that! Let's dive in." },
            { "sad", "I'm here for you. Let’s focus on solutions." },
            { "angry", "Let’s channel that into prevention." }
        };

        static Dictionary<string, string> emotionKeywords = new Dictionary<string, string>()
        {
            { "anxious", "worried" }, { "nervous", "worried" }, { "confused", "frustrated" },
            { "irritated", "frustrated" }, { "mad", "angry" }, { "sad", "sad" },
            { "unhappy", "sad" }, { "happy", "happy" }, { "excited", "happy" },
            { "interested", "curious" }, { "curious", "curious" }, { "worried", "worried" },
            { "frustrated", "frustrated" }
        };

        static void Main(string[] args)
        {
            Console.Title = "Cybersecurity Awareness Chatbot";
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintBanner(" CYBERSECURITY AWARENESS CHATBOT");

            Console.ResetColor();
            Console.Write("\n What's your name? ");
            userName = Console.ReadLine();

            Console.WriteLine($"\nHello, {userName}! Ask your cybersecurity questions or choose a topic, to learn more about stayng safe online.");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n Available Cybersecurity Awareness Topics:");
                foreach (var key in keywordResponses.Keys.OrderBy(k => k))
                {
                    Console.Write($"[{key}] ");
                }
                Console.ResetColor();

                Console.Write("\n\n type 'exit'(to exit the programme), \n type 'history'(to view recent searches), or 'clear history'(to clear recent searches), \n What would you like to know : ");
                string input = Console.ReadLine()?.ToLowerInvariant()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine(" Please enter something meaningful.");
                    continue;
                }

                if (input == "exit" || input == "quit" || input == "bye")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n Goodbye, {userName}! Stay cyber-safe.");
                    Console.ResetColor();
                    break;
                }

                if (input == "history")
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("\n Your Recent Searches:");
                    foreach (string historyItem in searchHistory)
                    {
                        Console.WriteLine($"- {historyItem}");
                    }
                    Console.ResetColor();
                    continue;
                }

                if (input == "clear history")
                {
                    searchHistory.Clear();
                    Console.WriteLine(" Search history cleared.");
                    continue;
                }

                Stopwatch stopwatch = Stopwatch.StartNew();
                HandleEmotions(input);
                stopwatch.Stop();

                if (stopwatch.Elapsed.TotalMilliseconds <= 5)
                {
                    Console.WriteLine(" That was fast! But feel free to take your time typing.");
                }

                if (searchHistory.Contains(input))
                {
                    Console.Write($" You've asked this before. Do you want the answer again? (yes/no): ");
                    if (Console.ReadLine()?.ToLower() != "yes")
                    {
                        Console.WriteLine(" No problem. Ask something else!");
                        continue;
                    }
                }
                searchHistory.Add(input);

                string matchedKeyword = keywordResponses.Keys.FirstOrDefault(k => Regex.IsMatch(input, $"\\b{k}\\b"));

                if (!string.IsNullOrEmpty(matchedKeyword))
                {
                    favoriteTopic = matchedKeyword;
                    string response = GetRandomResponse(keywordResponses[matchedKeyword]);

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"\n Answer about '{matchedKeyword}': {response}");
                    Console.ResetColor();

                    DisplayPixelatedImage(matchedKeyword);

                    Console.Write(" Would you like more information on this topic? (yes/no): ");
                    if (Console.ReadLine()?.ToLower() == "yes")
                    {
                        string moreInfo = GetRandomResponse(keywordResponses[matchedKeyword]);
                        Console.WriteLine($" More Info: {moreInfo}");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Sorry, I couldn't identify a keyword. Try rephrasing or picking a topic.");
                    Console.ResetColor();
                }
            }
        }

        static void PrintBanner(string message)
        {
            Console.WriteLine(new string('═', message.Length + 4));
            Console.WriteLine($"║ {message} ║");
            Console.WriteLine(new string('═', message.Length + 4));
        }

        static string GetRandomResponse(List<string> responses)
        {
            return responses[rnd.Next(responses.Count)];
        }

        static void HandleEmotions(string input)
        {
            foreach (var pair in emotionKeywords)
            {
                if (Regex.IsMatch(input, $"\\b{pair.Key}\\b"))
                {
                    string emotion = pair.Value;
                    if (emotionResponses.ContainsKey(emotion))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($" {emotionResponses[emotion]}");
                        Console.ResetColor();
                    }
                    break;
                }
            }
        }

        static void DisplayPixelatedImage(string topic)
        {
            Console.WriteLine($"\n");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Console.Write(rnd.Next(0, 2) == 0 ? "▓" : "░");
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }
    }
}


