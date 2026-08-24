using System;
using System.Collections.Generic;
using System.IO;
using BankApp.Models;
using BankApp.Services;

namespace BankApp.Data {
    public static class Database {
        public static List<User> Users = new List<User>();
        public static List<Card> Cards = new List<Card>();
        public static List<Account> Accounts = new List<Account>();
        public static List<Transaction> Transactions = new List<Transaction>();

        private static int nextAccountId = 1;
        private static int nextTransactionId = 1;

        public static void Seed() {
            LoadUsers();
            LoadCards();
            LoadAccounts();
            LoadTransactions();
            MigratePins();
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
                if (parts.Length != 5) continue;

                User user = new User();
                user.Id = int.Parse(parts[0]);
                user.FullName = parts[1];
                user.Phone = parts[2];
                user.IsFirstLogin = bool.Parse(parts[3]);
                user.PinHash = parts[4];
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

        private static void LoadAccounts() {
            string path = "Data/accounts.txt";
            if (!File.Exists(path)) {
                Console.WriteLine("Файл accounts.txt не найден.");
                return;   
            }

            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines) {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                string[] parts = line.Split('|');
                if (parts.Length != 4) continue;

                Account account = new Account(int.Parse(parts[1]), parts[2]);
                account.Id = int.Parse(parts[0]);
                account.Balance = decimal.Parse(parts[3]);

                Accounts.Add(account);

                if (account.Id >= nextAccountId)
                    nextAccountId = account.Id + 1;
            }
        }

        private static void LoadTransactions() {
            string path = "Data/transactions.txt";
            if (!File.Exists(path)) {
                Console.WriteLine("Файл transactions.txt не найден.");
                return;
            }

            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines) {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;
                string[] parts = line.Split('|');
                if (parts.Length != 6) continue;

                Transaction transaction = new Transaction(
                    int.Parse(parts[1]),
                    parts[2],
                    decimal.Parse(parts[3]),
                    parts[5]
                );
                transaction.Id = int.Parse(parts[0]);
                transaction.Date = DateTime.Parse(parts[4]);

                Transactions.Add(transaction);

                if (transaction.Id >= nextTransactionId)
                    nextTransactionId = transaction.Id + 1;
            }
        }

        public static void SaveUsers() {
            string path = "Data/users.txt";
            StreamWriter writer = null;
            try {
                writer = new StreamWriter(path);
                writer.WriteLine("#UserId|FullName|Phone|IsFirstLogin|PinHash");
                foreach (User user in Users) 
                    writer.WriteLine($"{user.Id}|{user.FullName}|{user.Phone}|{user.IsFirstLogin}|{user.PinHash}");
                
            } finally {
                if (writer != null)
                    writer.Close();
            }
        }

        public static void SaveAccounts() {
            string path = "Data/accounts.txt";
            StreamWriter writer = null;
            try {
                writer = new StreamWriter(path);
                writer.WriteLine("#Id|UserId|AccountNumber|Balance");
                foreach (Account account in Accounts)
                    writer.WriteLine($"{account.Id}|{account.UserId}|{account.AccountNumber}|{account.Balance}");
            } finally {
                if (writer != null)
                    writer.Close();
            }
        }

        public static void SaveTransactions() {
            string path = "Data/transactions.txt";
            StreamWriter writer = null;
            try {
                writer = new StreamWriter(path);
                writer.WriteLine("#Id|UserId|Type|Amount|Date|Description");
                foreach (Transaction trans in Transactions)
                    writer.WriteLine($"{trans.Id}|{trans.UserId}|{trans.Type}|{trans.Amount}|{trans.Date}|{trans.Description}");
            } finally {
                if (writer != null) writer.Close();
            }
        }

        public static void AddTransaction(int userId, string type, decimal amount, string description="") {
            Transaction trans = new Transaction(userId, type, amount, description);
            trans.Id = nextTransactionId++;
            Transactions.Add(trans);
            SaveTransactions();
        }

        public static void MigratePins() {
            foreach (User user in Users) {
                if (!string.IsNullOrEmpty(user.PinHash) && user.PinHash.Length < 64)
                    user.PinHash = HashHelper.HashPassword(user.PinHash);
            }
            SaveUsers();
        }
    }
}