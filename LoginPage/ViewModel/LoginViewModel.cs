﻿using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoginPage.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {

        private int _username;
        private int _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        public event EventHandler LoginSucceded;

        public int Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged("Username"); }
        }
        public int Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged("Password"); }
        }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }


        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }


        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }







        // Commands
        public ICommand LoginCommand { get; set; }
        public ICommand RecoverPasswordCommand { get; }

        public ICommand ShowPasswordCommand { get; }

        public ICommand RememberPasswordCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RecoverPasswordCommand = new ViewModelCommand(p => ExecuteRecoverPassword("", ""));

        }

        private void ExecuteRecoverPassword(string username, string email)
        {
            throw new NotImplementedException();
        }



        public void ExecuteLoginCommand(object obj)
        {
            var temp = IsValidCustomer(Username, Password);
            if (!temp.Result)
            {

                ErrorMessage = "InValid UserName or Password";

            }
            else
            {
                MainWindow.UserId = Username;
                MainWindow.ViewIndex = 1;
            }
        }



        async Task<bool> IsValidCustomer(int CustId, int password)
        {
            var url = $"https://localhost:7172/customer/validCustomer/{CustId}/{password}";

            using (var client = new HttpClient())
            {

                var msg = new HttpRequestMessage(HttpMethod.Get, url);
                msg.Headers.Add("User-Agent", "C# Program");
                var res = client.SendAsync(msg).Result;

                var content = await res.Content.ReadAsStringAsync();
                var stringContent = Convert.ToString(content);
                var contentResponse = JsonConvert.DeserializeObject<bool>(stringContent);
                return contentResponse;
            }

        }




        private bool CanExecuteLoginCommand(object obj)
        {
            bool canExecute;

            if (Username != 0 && Password != 0)
            {
                canExecute = true;
            }


            else
            {
                canExecute = false;
            }
            return canExecute;
        }

    }
}
