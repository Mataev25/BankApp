using System;
using BankApp.Models;
using BankApp.Data;

namespace BankApp.Services {
    public class AuthService {
        public bool CardExists(string cardNumber) {
            foreach (Card card in Database.Cards) {
                if (card.Number == cardNumber)
                    return true;
            }
            return false;
        }

        public User GetUserByCard(string cardNumber) {
            Card card = null;
            foreach (Card c in Database.Cards) {
                if (c.Number == cardNumber) {
                    card = c;
                    break;
                }
            }

            if (card == null) return null;

            foreach (User user in Database.Users) {
                if (user.Id == card.UserId)
                    return user;
            }
            return null;
        }

        public bool ValidatePin(string cardNumber, string pin) {
            User user = GetUserByCard(cardNumber);
            if (user == null) return false;
            return user.PinHash == pin;
        }

        public bool ActivateCard(string cardNumber, string pin) {
            User user = GetUserByCard(cardNumber);
            if (user == null) return false;
            
            if (user.IsFirstLogin) {
                user.PinHash = pin;
                user.IsFirstLogin = false;
                return true;
            }
            return false;
        }

        public void IncrementFailedAttempts(string cardNumber) {
            foreach (Card card in Database.Cards) {
                if (card.Number == cardNumber) {
                    card.FailedAttempts++;
                    if (card.FailedAttempts >= 3) 
                        card.IsBlocked = true;
                    break;
                }
            }
        }

        public bool IsCardBlocked(string cardNumber) {
            foreach (Card card in Database.Cards) {
                if (card.Number == cardNumber)
                    return card.IsBlocked;
            }
            return true;
        }

        public void ResetFailedAttempts(string cardNumber) {
            foreach (Card card in Database.Cards) {
                if (card.Number == cardNumber) {
                    card.FailedAttempts = 0;
                    break;
                }
            }
        }

        //Вспомогательный метод для тестов.
         public Card GetCardByNumber(string cardNumber) {
            foreach (Card card in Database.Cards) {
                if (card.Number == cardNumber) 
                    return card;
            }
            return null;
        }
    }
}