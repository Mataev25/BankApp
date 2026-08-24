using System;
using BankApp.Data;
using BankApp.Models;
using BankApp.Tests;
using BankApp.UI;
using BankApp;

class Program {
    static void Main() {
       //UserTests.Run();
        Database.Seed();
        //AccountTests.Run();
        //AuthTests.Run();
        IMenu menu = new ConsoleMenu();
        menu.Run();
    }
}
