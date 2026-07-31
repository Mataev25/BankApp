namespace BankApp.Models {
    public class User {
        private string fName;
        private string phone;
        private string pinHash;
        private int id;
        private bool firstLogin;
        private DateTime createAt;

        public User () {
            fName = string.Empty;
            phone = string.Empty;
            pinHash = string.Empty;
            id = 0;
            firstLogin = true;
            createAt = DateTime.Now;
        }

        public int Id {
            get {return id;}
            set {id = value;}
        }

        public string FullName {
            get {return fName;}
            set {
                if (value == "")
                    throw new ArgumentException("Имя не может быть пустым");
                
                foreach (char c in value) {
                    if (!char.IsLetter(c) && c != ' ' && c != '-')
                        throw new ArgumentException("Недопустимый символ в имени");
                }
                
                fName = value.Trim();
            }
        }

        public string Phone {
            get {return phone;}
            set {
                string cleaned = "";
                foreach (int c in value) {
                    if (char.IsDigit(c) || c == '+' || c == ' '
                        || c == '(' || c == ')' || c == '-')
                        cleaned += c;
                }

                if (cleaned.Length < 11 || cleaned.Length > 12)
                    throw new ArgumentException("Номер телефона должен содержать 11-12 цифр");

                phone = cleaned; 
            }
        }
    }
}