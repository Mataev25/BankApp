using System;
using BankApp.Models;
using BankApp.Data;
using BankApp.Services;

namespace BankApp.Tests {
    class AuthTests {
        public static void Run() {
            Console.WriteLine("Тестирование AuthService");

            Database.Seed();

            TestCardExists();
            TestGetUserByCard();
            TestActivateCard();
            TestValidatePin();
            TestFailedAttempts();
            TestCardBlocking();
        }

        private static void TestCardExists() {
            Console.WriteLine("#1 Тест: проверка существования карты\n");
            AuthService auth = new AuthService();
            
            if (auth.CardExists("1234-5678-9010-1112"))
                Console.WriteLine("Существующая карта найдена.");
            else
                Console.WriteLine("Существующая карта не найдена. Ошибка");

            if (!auth.CardExists("0000-0000-0000-0000"))
                Console.WriteLine("Несуществующая карта не найдена.");
            else
                Console.WriteLine("Несуществующая карта найдена. Ошибка");
            
            Console.WriteLine();
        }

        private static void TestGetUserByCard() {
            Console.WriteLine("#2 Тест: получение пользователя по карте\n");
            AuthService auth = new AuthService();

            User user = auth.GetUserByCard("1234-5678-9010-1112");
            if (user != null && user.FullName == "Матаев Игорь") 
                Console.WriteLine($"Пользователь найден: {user.FullName}");
            else
                Console.WriteLine("Пользователь не найден. Ошибка");
            
            Console.WriteLine();
        }

        private static void TestActivateCard() {
            Console.WriteLine("#3 Тест: активация карты");
            AuthService auth = new AuthService();
            bool result = auth.ActivateCard("1234-5678-9010-1112", "1234");

            if (result)
                Console.WriteLine("Карта активирована");
            else
                Console.WriteLine("Карта не активирована. Ошибка");

            Console.WriteLine();

        }

        private static void TestValidatePin() {
            Console.WriteLine("#4 Тест: проверка PIN-кода");
            AuthService auth = new AuthService();

            bool valid = auth.ValidatePin("1234-5678-9010-1112", "1234");
            if (valid) 
                Console.WriteLine("PIN-код верный");
            else
                Console.WriteLine("Ошибка. Верный PIN-код не проходит");

            bool invalid = auth.ValidatePin("1234-5678-9010-1112", "9090");
            if (!invalid)
                Console.WriteLine("Неверный PIN-код отклонен");
            else
                Console.WriteLine("Неверный PIN-код принят. Ошибка");

            Console.WriteLine();
        }

        private static void TestFailedAttempts() {
            Console.WriteLine("#5 Тест: неудачные попытки ввыода PIN-кода");
            AuthService auth = new AuthService();
            
            auth.ResetFailedAttempts("1234-5678-9010-1112");
            for (int i=1; i<=3; i++) {
                auth.ValidatePin("1234-5678-9010-1112", "9090");
                auth.IncrementFailedAttempts("1234-5678-9010-1112");
                Console.WriteLine($" Попытка {i}: счетчик ошибок = {auth.GetCardByNumber("1234-5678-9010-1112").FailedAttempts}");
            }
            Console.WriteLine();
        }

        private static bool TestCardBlocking() {
            Console.WriteLine("#6 Тест: блокировка карты");
            AuthService auth = new AuthService();

            if (auth.IsCardBlocked("1234-5678-9010-1112")) {
                Console.WriteLine("Карта заблокирвана\n");
                return true;
            }
            else {
                Console.WriteLine("Карта не заблокирована. Ошибка");
                return false;
            }
        }
    }
}

