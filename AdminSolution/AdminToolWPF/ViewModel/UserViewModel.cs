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
            }


            RaisePropertyChanged();
        }

    }
}
