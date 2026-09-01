using System;
using BankApp.Models;
using BankApp.Services;
using BankApp.Data;

namespace BankApp.Tests {
    class AccountTests {
        public static void Run() {
            Console.WriteLine("Тестирование класса AccountService\n");
            
            AppDbContext context = null;
            AccountService acc = null;

            try {
                context = new AppDbContext();
                DbInitializer.Seed(context);
                acc = new AccountService(context);

                TestGetBalance(acc);
                TestDeposit(acc);
                TestWithdraw(acc);
                TestWithdrawInsufficientFunds(acc);
                TestNegativeAmount(acc);
            } finally {
                if (context != null)
                    context.Dispose();
            }
        }

        private static void TestGetBalance(AccountService acc) {
            Console.WriteLine("#1 Тест: получение баланса");
            decimal balance = acc.GetBalance(1);
            
            if (balance == 70000)
                Console.WriteLine($"Баланс получен:  {balance} р.");
            else
                Console.WriteLine($"Ошибка. Ожидалось 70000 р. Получено {balance} р.");
            Console.WriteLine();
        }

        private static void TestDeposit(AccountService acc) {
            Console.WriteLine("#2 Тест: пополнение счета");
            decimal before = acc.GetBalance(1);
            bool result = acc.Deposit(1, 5000);
            decimal after = acc.GetBalance(1);

            if (result && after == before + 5000)
                Console.WriteLine($"Пополнение успешно: {before} + 5000 = {after}");
            else
                Console.WriteLine($"Ошибка пополнения.");
            Console.WriteLine();

        }

        private static void TestWithdraw(AccountService acc) {
            Console.WriteLine("#3 Тест: снятие наличных");
            decimal before = acc.GetBalance(1);
            bool result = acc.Withdraw(1, 30000);
            decimal after = acc.GetBalance(1);

            if (result && after == before - 30000)
                Console.WriteLine($"Снятие успешно: {before} - 30000 = {after}");
            else
                Console.WriteLine($"Ошибка снятия");
            Console.WriteLine();
        }

        private static void TestWithdrawInsufficientFunds(AccountService acc) {
            Console.WriteLine("#4 Тест: снятие при недостатке средств");
            decimal before = acc.GetBalance(1);
            bool result = acc.Withdraw(1, 98989898);
            decimal after = acc.GetBalance(1);

            if (!result && after == before)
                Console.WriteLine($"Недостаточно средств. Баланс не изменился: {after}");
            else
                Console.WriteLine($"Ошибка. Система позволила снять больше, чем есть");
            Console.WriteLine();
        }

        private static void TestNegativeAmount(AccountService acc) {
            Console.WriteLine("#5 Тест: отрицательная сумма");
            decimal before = acc.GetBalance(1);
            bool result = acc.Deposit(1, -1000);
            decimal after = acc.GetBalance(1);

            if (!result && after == before)
                Console.WriteLine($"Отрицательная сумма не принята. Баланс: {after}");
            else
                Console.WriteLine($"Ошибка. Отрицательная сумма прошла.");
            Console.WriteLine();
        }
    }
}