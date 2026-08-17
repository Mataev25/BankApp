using System;
using BankApp.Models;

namespace BankApp {
    class UserTests {
        public static void Run () {
            Console.WriteLine("Тестирование класса User\n");

            TestValidUser();
            TestInvalidFullName();
            TestInvalidPhone();
            TestEdgeCases();
        }

        private static void TestValidUser() {
            Console.WriteLine("#1 Тест: корректные данные");
            User user = new User();
            user.Id = 1;
            user.FullName = "Игорь Матаев";
            user.Phone = "+7(999) 111-22-33";

            Console.WriteLine($" ID: {user.Id} (ожидалось 1)");
            Console.WriteLine($" FullName: {user.FullName} (ожидалось Игорь Матаев)");
            Console.WriteLine($" Phone: {user.Phone} (ожидалось +79991112233");
            Console.WriteLine("Тест пройден\n");            
        } 

        private static void TestInvalidFullName() {
            Console.WriteLine("#2 Тест: неверное имя");
            User user = new User();
            try {
                user.FullName = "Игорь Матаев123";
                Console.WriteLine("Исключение не было выброшено");
            } catch (ArgumentException ex) {
                Console.WriteLine($"Исключение перехвачено: {ex.Message}");
            }
            Console.WriteLine();
        }

        private static void TestInvalidPhone() {
            Console.WriteLine("#3 Тест: неверный телефон");
            User user = new User();
            try {
                user.Phone = "4567";
                Console.WriteLine("Искючение не было выброшено");
            } catch (ArgumentException ex) {
                Console.WriteLine($"Исключение перехвачено: {ex.Message}");
            }
            Console.WriteLine();
        }

        private static void TestEdgeCases() {
            Console.WriteLine("#4 Тест: граничные случаи");
            User user = new User();
            try {
                user.FullName = "";
                Console.WriteLine("Пустое имя не должно проходить");
            } catch (ArgumentException ex) {
                Console.WriteLine($"Исключение перехвачено: {ex.Message}");
            }

            try {
                user.Phone = "899911122337";
                Console.WriteLine("Длинный номер из 12 цифр принят");
            } catch (ArgumentException ex) {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Console.WriteLine();
        }

    }


}