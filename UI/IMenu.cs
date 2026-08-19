namespace BankApp.UI {
    public interface IMenu {
        void Run();
        void ShowMainMenu();
        void ShowAuthMenu();
        void ShowAccountMenu();
        void ShowExit();
    }

}