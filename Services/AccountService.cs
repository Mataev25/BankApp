using System;
using BankApp.Models;
using BankApp.Data;

namespace BankApp.Services {
    public class AccountService {
        public Account GetAccountByUserId(int userId) {
            foreach (Account account in Database.Accounts) {
                if (account.UserId == userId)
                    return account;
            }
            return null;
        }

        public decimal GetBalance(int userId) {
            Account account = GetAccountByUserId(userId);
            if (account == null) return 0;
            return account.Balance;
        }

        public bool Deposit(int userId, decimal amount) {
            if (amount <= 0) {
                Console.WriteLine("Сумма должна быть больше 0");
                return false;
            }

            Account account = GetAccountByUserId(userId);
            if (account == null) {
                Console.WriteLine("Счет не найден");
                return false;
            }

            account.Balance += amount;
            Database.SaveAccounts();

            Database.AddTransaction(userId, "Deposit", amount, $"Пополнение счета {account.AccountNumber}");

            Console.WriteLine($"Счет пополнен на {amount} р. Текущий баланс: {account.Balance} р.");
            return true;
        }

        public bool Withdraw(int userId, decimal amount) {
            if (amount <= 0) {
                Console.WriteLine("Сумма должна быть больше 0");
                return false;
            }

            Account account = GetAccountByUserId(userId);
            if (account == null) {
                Console.WriteLine("Счет не найден");
                return false;
            }

            if (account.Balance < amount) {
                Console.WriteLine($"Недостаточно средств. Доступно: {account.Balance} р.");
                return false;
            }

            account.Balance -= amount;
            Database.SaveAccounts();
            Database.AddTransaction(userId, "Deposit", amount, $"Снятие со счета {account.AccountNumber}");
            
            Console.WriteLine($"Снято: {amount} р. Текущий баланс: {account.Balance} р.");
            return true;
        }
    }


}