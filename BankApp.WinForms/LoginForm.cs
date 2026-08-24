using System;
using System.Windows.Forms;
using BankApp.Services;
using BankApp.Models;

namespace BankApp.WinForms {
    public partial class LoginForm : Form {
        private TextBox txtCardNumber;
        private TextBox txtPin;
        private Button btnLogin;
        private Label lblCardNumber;
        private Label lblPin;

        private AuthService authService;
        
        public LoginForm() {
            authService = new AuthService();
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI() {
            this.Text = " Вход в банк";
            this.Size = new System.Drawing.Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblCardNumber = new Label();
            lblCardNumber.Text = "Номер карты:";
            lblCardNumber.Location = new System.Drawing.Point(30, 30);
            lblCardNumber.Size = new System.Drawing.Size(100, 25);

            txtCardNumber = new TextBox();
            txtCardNumber.Location = new System.Drawing.Point(150, 30);
            txtCardNumber.Size = new System.Drawing.Size(200, 25);

            lblPin = new Label();
            lblPin.Text = "PIN-код:";
            lblPin.Location = new System.Drawing.Point(30, 70);
            lblPin.Size = new System.Drawing.Size(100, 25);

            txtPin = new TextBox();
            txtPin.Location = new System.Drawing.Point(150, 70);
            txtPin.Size = new System.Drawing.Size(200, 25);
            txtPin.PasswordChar = '*';

            btnLogin = new Button();
            btnLogin.Text = "Войти";
            btnLogin.Location = new System.Drawing.Point(150, 120);
            btnLogin.Size = new System.Drawing.Size(100, 30);
            btnLogin.Click += new EventHandler(LoginButton_Click);

            this.Controls.Add(lblCardNumber);
            this.Controls.Add(txtCardNumber);
            this.Controls.Add(lblPin);
            this.Controls.Add(txtPin);
            this.Controls.Add(btnLogin);
        }

        private void LoginButton_Click(object sender, EventArgs e) {
            string cardNumber = txtCardNumber.Text.Trim();
            string pin = txtPin.Text.Trim();

            if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(pin)) {
                MessageBox.Show("Введите номер карты и PIN-код.", "Ошибка");
                return;
            }

            if (!authService.CardExists(cardNumber)) {
                MessageBox.Show("Карта не найдена.", "Ошибка");
                return;
            }

            if (authService.IsCardBlocked(cardNumber)) {
                MessageBox.Show("Карта заблокирована.", "Ошибка");
                return;
            }

            User user = authService.GetUserByCard(cardNumber);
            if (user == null) {
                MessageBox.Show("Пользователь не найден.", "Ошибка");
                return;
            }

            if (user.IsFirstLogin) {
                MessageBox.Show("Это ваш первый вход. Установите PIN-код через консольное приложение.", "Информация");
                return;
            }

            if (!authService.ValidatePin(cardNumber, pin)) {
                authService.IncrementFailedAttempts(cardNumber);

                if (authService.IsCardBlocked(cardNumber)) 
                    MessageBox.Show("Карта заблокирована после 3-х неудачных попыток.", "Ошибка");
                else
                    MessageBox.Show("Неверный PIN-код.", "Ошибка");
                return;
            }

            authService.ResetFailedAttempts(cardNumber);
            MessageBox.Show($"Добро пожаловать, {user.FullName}!", "Успех");

            MainForm mainForm = new MainForm(user);
            mainForm.Show();
            this.Hide();
        }
    }

}
