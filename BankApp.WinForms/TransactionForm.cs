using System;
using System.Windows.Forms;
using BankApp.Services;
using BankApp.Models;

namespace BankApp.WinForms {
    public partial class TransactionForm : Form {
        private int userId;
        private string type;
        private AccountService acc;

        private TextBox txtAmount;
        private Button btnOk;
        private Label lblMessage;

        public event EventHandler TransactionCompleted;

        public TransactionForm(int userId, string type) {
            this.userId = userId;
            this.type = type;
            acc = new AccountService();
            SetupUI();
        }

        private void SetupUI() {
            this.Text = type == "Deposit" ? "Пополнение счета" : "Снятие наличных";
            this.Size = new System.Drawing.Size(400, 200);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblMessage = new Label();
            lblMessage.Text = "Введите сумму:";
            lblMessage.Location = new System.Drawing.Point(30, 30);
            lblMessage.Size = new System.Drawing.Size(100, 25);

            txtAmount = new TextBox();
            txtAmount.Location = new System.Drawing.Point(150, 30);
            txtAmount.Size = new System.Drawing.Size(200, 25);

            btnOk = new Button();
            btnOk.Text = "OK";
            btnOk.Location = new System.Drawing.Point(150, 80);
            btnOk.Size = new System.Drawing.Size(100, 30);
            btnOk.Click += BtnOk_Click;

            this.Controls.Add(lblMessage);
            this.Controls.Add(txtAmount);
            this.Controls.Add(btnOk);
        }

        private void BtnOk_Click(object sender, EventArgs e) {
            MessageBox.Show("Кнопка OK нажата");   

            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0) {
                MessageBox.Show("Введите корректную сумму.", "Ошибка");
                return;
            }

            MessageBox.Show($"Сумма: {amount}");  

            bool success = false;
            if (type == "Deposit")
                success = acc.Deposit(userId, amount);
            else
                success = acc.Withdraw(userId, amount);

            
            MessageBox.Show($"Результат операции: {success}");   
            
            if (success) {
                MessageBox.Show("Операция выполнена успешно.", "Успех");
                TransactionCompleted?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
            else
                MessageBox.Show("Ошибка выполнения операции.", "Ошибка");
        }
    }
}