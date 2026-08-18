using System;
using System.Collections.Generic;
using System.IO;
using BankApp.Models;

namespace BankApp.Data {
    public static class Database {
        public static List<User> Users = new List<User>();
        public static List<Card> Cards = new List<Card>();

        public static void Seed() {
            LoadUsers();
            LoadCards();
        }

        private static void LoadUsers() {
            string path = "Data/users.txt";
            if (!File.Exists(path)) {
                Console.WriteLine("Файл users.txt не найден.");
                return;
            }

            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines) {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;
                
                string[] parts = line.Split('|');
                if (parts.Length != 4) continue;

                User user = new User();
                user.Id = int.Parse(parts[0]);
                user.FullName = parts[1];
                user.Phone = parts[2];
                user.IsFirstLogin = bool.Parse(parts[3]);
                user.CreatedAt = DateTime.Now;

                Users.Add(user);
            }
        }

        private static void LoadCards() {
            string path = "Data/cards.txt";
            if (!File.Exists(path)) {
                Console.WriteLine("Файл cards.txt не найден.");
                return;
            }

            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines) {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                string[] parts = line.Split('|');
                if (parts.Length != 5) continue;

                Card card = new Card();
                card.Number = parts[0];
                card.UserId = int.Parse(parts[1]);
                card.IsActive = bool.Parse(parts[2]);
                card.FailedAttempts = int.Parse(parts[3]);
                card.IsBlocked = bool.Parse(parts[4]);

                Cards.Add(card);
            }
        }
    }
}