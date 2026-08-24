using System;
using System.Windows.Forms;
using BankApp.Models;
using BankApp.Services;

namespace BankApp.WinForms {
    public partial class MainForm : Form {
        private User currentUser;
        private AccountService accountService;
        private LoginForm loginForm;

        private Label lblWelcome;
        private Label lblBalance;
        private Button btnDeposit;
        private Button btnWithdraw;
        private Button btnHistory;
        private Button btnLogout;

        public MainForm(User user, LoginForm loginForm) {
            currentUser = user;
            this.loginForm = loginForm;
            accountService = new AccountService();
            //InitializeComponent();
            SetupUI();
            this.FormClosing += MainForm_FormClosing;
        }

        private void SetupUI() {
            this.Text = "Е-банк - главное меню";
            this.Size = new System.Drawing.Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblWelcome = new Label();
            lblWelcome.Text = $"Добро пожаловать, {currentUser.FullName}!";
            lblWelcome.Location = new System.Drawing.Point(20, 20);
            lblWelcome.Size = new System.Drawing.Size(400, 30);
            lblWelcome.Font = new System.Drawing.Font("Arial", 14);

            decimal balance = accountService.GetBalance(currentUser.Id);
            lblBalance = new Label();
            lblBalance.Text = $"Баланс: {balance} р.";
            lblBalance.Location = new System.Drawing.Point(20, 60);
            lblBalance.Size = new System.Drawing.Size(400, 25);
            lblBalance.Font = new System.Drawing.Font("Arial", 12);

            btnDeposit = new Button();
            btnDeposit.Text = "Пополнить счет";
            btnDeposit.Location = new System.Drawing.Point(20, 110);
            btnDeposit.Size = new System.Drawing.Size(200, 40);
            btnDeposit.Click += new EventHandler(DepoisitButton_Click);

            btnWithdraw = new Button();
            btnWithdraw.Text = "Снять наличные";
            btnWithdraw.Location = new System.Drawing.Point(240, 110);
            btnWithdraw.Size = new System.Drawing.Size(200, 40);
            btnWithdraw.Click += new EventHandler(WithdrawButton_Click);

            btnHistory = new Button();
            btnHistory.Text = "История операций";
            btnHistory.Location = new System.Drawing.Point(20, 170);
            btnHistory.Size = new System.Drawing.Size(420, 40);
            btnHistory.Click += new EventHandler(HistoryButton_Click);

            btnLogout = new Button();
            btnLogout.Text = "Выйти";
            btnLogout.Location = new System.Drawing.Point(20, 230);
            btnLogout.Size = new System.Drawing.Size(420, 40);
            btnLogout.Click += new EventHandler(LogoutButton_Click);

            this.Controls.Add(lblWelcome);
            this.Controls.Add(lblBalance);
            this.Controls.Add(btnDeposit);
            this.Controls.Add(btnWithdraw);
            this.Controls.Add(btnHistory);
            this.Controls.Add(btnLogout);
        }

        private void DepoisitButton_Click(object sender, EventArgs e) {
            TransactionForm transForm = new TransactionForm(currentUser.Id, "Deposit");
            transForm.TransactionCompleted += OnTransactionCompleted;
            transForm.ShowDialog();
            RefreshBalance(); 
        }

        private void WithdrawButton_Click(object sender, EventArgs e) {
            TransactionForm transForm = new TransactionForm(currentUser.Id, "Withdraw");
            transForm.TransactionCompleted += OnTransactionCompleted;
            transForm.ShowDialog();
            RefreshBalance(); 
        }

        private void HistoryButton_Click(object sender, EventArgs e) {
            HistoryForm histForm = new HistoryForm(currentUser.Id);
            histForm.ShowDialog();
        }

        private void LogoutButton_Click(object sender, EventArgs e) {
            this.Close();
            //LoginForm loginForm = new LoginForm();
            loginForm.Show(); 
        }

        private void OnTransactionCompleted(object sender, EventArgs e) {
            RefreshBalance();
        }

        private void RefreshBalance() {
            decimal balance = accountService.GetBalance(currentUser.Id);
            lblBalance.Text = $"Баланс: {balance} р.";
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            Application.Exit();
        }

    }
}