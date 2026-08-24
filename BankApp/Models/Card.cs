using System;

namespace BankApp.Models {
    public class Card {
        private string number = "";
        private int userId;
        private bool isActive;
        private int failedAttempts;
        private bool isBlocked;

        /*public Card(string number, int userId) {
            this.number = number;
            this.userId = userId;
            this.isActive = true;
            this.failedAttempts = 0;
            this.isBlocked = false;
        }*/

        public string Number {
            get {return number;}
            set {number = value;}
        }

        public int UserId {
            get {return userId;}
            set {userId = value;}
        }

        public bool IsActive {
            get {return isActive;}
            set {isActive = value;}
        }

        public int FailedAttempts {
            get {return failedAttempts;}
            set {failedAttempts = value;}
        }

        public bool IsBlocked {
            get {return isBlocked;}
            set {isBlocked = value;}
        }
    }
}