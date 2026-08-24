using System;
using System.Windows.Forms;
using BankApp.Models;
using BankApp.Data;


namespace BankApp.WinForms {
    public partial class HistoryForm : Form{
        private int userId;
        private ListBox listBox;


        public HistoryForm(int userId) {
            this.userId = userId;
            SetupUI();
            LoadHistory();
        }

        private void SetupUI() {
            this.Text = "История операций";
            this.Size = new System.Drawing.Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            listBox = new ListBox();
            listBox.Location = new System.Drawing.Point(20, 20);
            listBox.Size = new System.Drawing.Size(540, 320);
            listBox.Font = new System.Drawing.Font("Consolas", 10);

            this.Controls.Add(listBox);
        }

        private void LoadHistory() {
            listBox.Items.Clear();
            bool found = false;

            foreach (Transaction trans in Database.Transactions) {
                if (trans.UserId == userId) {
                    listBox.Items.Add($"{trans.Date:dd.MM.yyyy HH:mm} | {trans.Type} | {trans.Amount} р. | {trans.Description}");
                    found = true;
                }
            }

            if (!found)
                listBox.Items.Add("Операций пока нет");
        }
    }
}