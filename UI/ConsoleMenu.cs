using System;
using BankApp.Models;
using BankApp.Services;

namespace BankApp.UI {
    public class ConsoleMenu : IMenu {
        private AuthService authService;
        private User currentUser;
        private AccountService accountService;

        public ConsoleMenu() {
            authService = new AuthService();
            currentUser = null;
            accountService = new AccountService();
        }

        public void Run() {
            Console.Clear();
            Console.WriteLine("Добро пожаловать в Е-банк\n");
            ShowAuthMenu();
        }

        public void ShowAuthMenu() {
            Console.WriteLine("#1. Вход по номеру карты");
            Console.WriteLine("#2. Выход");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();
            switch (choice) {
                case "1" : ShowLogin(); break;
                case "2" : ShowExit(); break;
                default: 
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    ShowAuthMenu();
                    break;
            }
        }

        private void ShowLogin() {
            Console.Write("Ввведите номер карты: ");
            string cardNumber = Console.ReadLine();

            if (!authService.CardExists(cardNumber)) {
                Console.WriteLine("Карта не найдена.");
                ShowAuthMenu();
                return;
            }

            if (authService.IsCardBlocked(cardNumber)) {
                Console.WriteLine("Карта заблокирована");
                ShowAuthMenu();
                return;
            }

            User user = authService.GetUserByCard(cardNumber);
            if (user == null) {
                Console.WriteLine("Пользователь не найден");
                ShowAuthMenu();
                return;
            }

            if (user.IsFirstLogin) {
                Console.Write("Это ваш первый вход. Установите PIN-код: ");
                string pin = Console.ReadLine();
                if (authService.ActivateCard(cardNumber, pin)) {
                    Console.WriteLine("Карта активирована.");
                    currentUser = user;
                    ShowAccountMenu();
                }
                else {
                    Console.WriteLine("Ошибка активации.");
                    ShowAuthMenu();
                }
            }
            else {
                Console.Write("Введите PIN-код: ");
                string pin = Console.ReadLine();
                if (authService.ValidatePin(cardNumber, pin)) {
                    Console.WriteLine($"Добро поаловать, {user.FullName}!");
                    currentUser = user;
                    authService.ResetFailedAttempts(cardNumber);
                    ShowAccountMenu();
                }
                else {
                    authService.IncrementFailedAttempts(cardNumber);
                    Console.WriteLine("Неверный PIN-код.");
                    if (authService.IsCardBlocked(cardNumber))
                        Console.WriteLine("Карта заблокирована после 3 неудачных попыток.");
                    
                    ShowAuthMenu();
                }
            }
        }

        public void ShowAccountMenu() {
            Console.WriteLine("\n********* Главное меню **********");
            Console.WriteLine("#1 Просмотр баланса");
            Console.WriteLine("#2 Пополнение счета");
            Console.WriteLine("#3 Снятие наличных");
            Console.WriteLine("#4 Выход из аккаунта");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();
            switch (choice) {
                case "1" : ShowBalance(); break;
                case "2" : ShowDeposit(); break;
                case "3" : ShowWithdraw(); break;
                case "4" :
                    currentUser = null;
                    Console.WriteLine("Вы вышли из аккаунта.");
                    ShowAuthMenu();
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    ShowAccountMenu();
                    break;
            }
        }

        private void ShowBalance() {
            if (currentUser == null) {
                Console.WriteLine("Пользователь не авторизован");
                ShowAuthMenu();
                return;
            }

            decimal balance = accountService.GetBalance(currentUser.Id);
            Console.WriteLine($"Баланс: {balance} р.");
            Console.Write("Нажмите Enter для продолжения...");
            Console.ReadLine();
        }

        private void ShowDeposit() {
            if (currentUser == null) {
                Console.WriteLine("Пользователь не авторизован");
                ShowAuthMenu();
                return;
            }

            Console.Write("Введите сумму для пополнения: ");
            string input = Console.ReadLine();
            if (!decimal.TryParse(input, out decimal amount)) {
                Console.WriteLine("Неверный формат суммы.");
                Console.WriteLine("Нажминте Enter для продолжения...");
                Console.ReadLine();
                return;
            }

            accountService.Deposit(currentUser.Id, amount);
            Console.WriteLine("Нажминте Enter для продолжения...");
            Console.ReadLine();
        }

        private void ShowWithdraw() {
             if (currentUser == null) {
                Console.WriteLine("Пользователь не авторизован");
                ShowAuthMenu();
                return;
            }
            Console.WriteLine("Введите сумму для снятия наличных: ");
            string input = Console.ReadLine();
            if (!decimal.TryParse(input, out decimal amount)) {
                Console.WriteLine("Неверный формат суммы.");
                Console.WriteLine("Нажминте Enter для продолжения...");
                Console.ReadLine();
                return;
            }

            accountService.Withdraw(currentUser.Id, amount);
            Console.WriteLine("Нажминте Enter для продолжения...");
            Console.ReadLine();
            
        }

        public void ShowMainMenu() {

        }

        public void ShowExit() {
            Console.WriteLine("До свидания!");

        }
    }
}