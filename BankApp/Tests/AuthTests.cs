using System;
using BankApp.Models;
using BankApp.Data;
using BankApp.Services;

namespace BankApp.Tests {
    class AuthTests {
        public static void Run() {
            Console.WriteLine("Тестирование AuthService");

            AppDbContext context = null;
            AuthService auth = null;

            try {
                context = new AppDbContext();
                DbInitializer.Seed(context);
                auth = new AuthService(context);

                TestCardExists(auth);
                TestGetUserByCard(auth);
                TestActivateCard(auth);
                TestValidatePin(auth);
                TestFailedAttempts(auth);
                TestCardBlocking(auth);
            } finally {
                if (context != null)
                    context.Dispose();
            }

        }

        private static void TestCardExists(AuthService auth) {
            Console.WriteLine("#1 Тест: проверка существования карты\n");
            
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

        private static void TestGetUserByCard(AuthService auth) {
            Console.WriteLine("#2 Тест: получение пользователя по карте\n");

            User user = auth.GetUserByCard("1234-5678-9010-1112");
            if (user != null && user.FullName == "Матаев Игорь") 
                Console.WriteLine($"Пользователь найден: {user.FullName}");
            else
                Console.WriteLine("Пользователь не найден. Ошибка");
            
            Console.WriteLine();
        }

        private static void TestActivateCard(AuthService auth) {
            Console.WriteLine("#3 Тест: активация карты");
            bool result = auth.ActivateCard("1234-5678-9010-1112", "1234");

            if (result)
                Console.WriteLine("Карта активирована");
            else
                Console.WriteLine("Карта не активирована. Ошибка");

            Console.WriteLine();

        }

        private static void TestValidatePin(AuthService auth) {
            Console.WriteLine("#4 Тест: проверка PIN-кода");

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

        private static void TestFailedAttempts(AuthService auth) {
            Console.WriteLine("#5 Тест: неудачные попытки ввыода PIN-кода");
            
            auth.ResetFailedAttempts("1234-5678-9010-1112");
            for (int i=1; i<=3; i++) {
                auth.ValidatePin("1234-5678-9010-1112", "9090");
                auth.IncrementFailedAttempts("1234-5678-9010-1112");
                Console.WriteLine($" Попытка {i}: счетчик ошибок = {auth.GetCardByNumber("1234-5678-9010-1112").FailedAttempts}");
            }
            Console.WriteLine();
        }

        private static bool TestCardBlocking(AuthService auth) {
            Console.WriteLine("#6 Тест: блокировка карты");

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

