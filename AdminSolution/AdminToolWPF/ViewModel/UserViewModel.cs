using AdminToolWPF.Helper_Classes;
using AdminToolWPF.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminToolWPF.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        private User _user;

        private string _userName = "";
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        private string _password = "";
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
            }
        }


        private string _email = "";
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }


        private int? _userId;
        public int? UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                RaisePropertyChanged("UserId");
            }
        }

        private bool _isAdmin = false;
        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
                RaisePropertyChanged("IsAdmin");
            }
        }


        private bool _isNewUser = false;
        public bool IsNewUser
        {
            get { return _isNewUser; }
            set
            {
                _isNewUser = value;
                RaisePropertyChanged("IsNewUser");
            }
        }


        public IRelayCommand SaveUserCommand => new RelayCommand(() =>
        {
            _user.UserName = this.UserName;
            _user.IsAdmin = this.IsAdmin;
            _user.Email = this.Email;
            _user.Password = this.Password;

            if (IsNewUser)
                QuerryHandler.CreateUser(_user);
            else
            {
                var test =  QuerryHandler.UpdateUser(_user).Result;

            }

        });

        public UserViewModel(User user = null)
        {
            if (user == null || user.UserId < 1)
            {
                IsNewUser = true;
            }
            else
            {
                IsNewUser = false;
                _user = user;
                UserId = _user.UserId;
                UserName = _user.UserName;
                IsAdmin = _user.IsAdmin;
                Password = _user.Password;
                Email = _user.Email;
            }


            RaisePropertyChanged();
        }

    }
}
