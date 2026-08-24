using System;
using System.Windows.Forms;
using BankApp.Data;
namespace BankApp.WinForms {
    static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Database.Seed();
            ApplicationConfiguration.Initialize();
            Application.Run(new LoginForm());
        }    
    }

}