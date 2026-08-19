using System;
using BankApp.Models;
using BankApp.Services;

namespace BankApp.UI {
    public class ConsoleMenu : IMenu {
        private AuthService authService;
        private User currentUser;

        public ConsoleMenu() {
            authService = new AuthService();
            currentUser = null;
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
            Console.WriteLine("Баланс: 10000 р");
            Console.WriteLine("Нажминте Enter для продолжения...");
            Console.ReadLine();
            ShowAccountMenu();
        }

        private void ShowDeposit() {
            Console.WriteLine("Пополнение счета");
            Console.WriteLine("Нажминте Enter для продолжения...");
            Console.ReadLine();
            ShowAccountMenu();
        }

        private void ShowWithdraw() {
            Console.WriteLine("Снятие наличных");
            Console.WriteLine("Нажминте Enter для продолжения...");
            Console.ReadLine();
            ShowAccountMenu();
        }

        public void ShowMainMenu() {

        }

        public void ShowExit() {
            Console.WriteLine("До свидания!");

        }
    }
}