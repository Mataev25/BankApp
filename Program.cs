using System;
using BankApp.Data;
using BankApp.Tests;

class Program {
    static void Main() {
        //UserTests.Run();
        Database.Seed();
        AuthTests.Run();
    }
}
