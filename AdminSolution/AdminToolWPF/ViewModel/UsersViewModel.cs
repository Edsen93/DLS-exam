using AdminToolWPF.Helper_Classes;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminToolWPF;
using System.Windows;
using AdminToolWPF.Model;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using AdminToolWPF.View;

namespace AdminToolWPF.ViewModel
{
    public class UsersViewModel : ViewModelBase, IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Users";
            }
        }





        public IRelayCommand NewUserCommand => new RelayCommand(() =>
        {
            Window window = new Window
            {
                Title = "New User",
                Content = new UserView(SelectedUser)

            };
            window.Height = 190;
            window.Width = 250;
            window.ShowDialog();
        });


        public IRelayCommand EditUserCommand => new RelayCommand(() =>
        {
            Window window = new Window
            {
                Title = "New User",
                Content = new UserView(SelectedUser)

            };
            window.Height = 190;
            window.Width = 250;
            window.ShowDialog();
        }, () => SelectedUser != null);


        public IRelayCommand DeleteUserCommand => new RelayCommand(() =>
        {
            string sMessageBoxText = $"Do you want to DELETE '{SelectedUser.UserName}' ?";
            string sCaption = "Delete User";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            if (rsltMessageBox == MessageBoxResult.Yes)
            {
                // run code
            }

        }, () => SelectedUser != null);





        public User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                RaisePropertyChanged("SelectedUser");
            }
        }


        private CollectionViewSource usersCollection = new CollectionViewSource();

        public ICollectionView SourceCollection
        {
            get
            {
                return this.usersCollection.View;
            }
        }


        private string filterText;
        public string FilterText
        {
            get
            {
                return filterText;
            }
            set
            {
                filterText = value;
                this.usersCollection.View.Refresh();
                RaisePropertyChanged("FilterText");
            }
        }


        public UsersViewModel(User model = null)
        {

            DoWork();

        }


        private Task<List<User>> DoWorkAsync()
        {
            TaskCompletionSource<List<User>> tcs = new TaskCompletionSource<List<User>>();
            Task.Run(() =>
            {
                var test = QuerryHandler.GetUsers();
                tcs.SetResult(test);
            });
            //return the Task
            return tcs.Task;
        }

        private async void DoWork()
        {
            //ApplicationViewModel.WorkInProgress = true;

            List<User> uList = await DoWorkAsync();
            ObservableCollection<User> movies = new ObservableCollection<User>(uList);

            usersCollection = new CollectionViewSource
            {
                Source = movies
            };
            usersCollection.Filter += (o, e) =>
            {
                if (string.IsNullOrEmpty(FilterText))
                {
                    e.Accepted = true;
                    return;
                }

                User usr = e.Item as User;
                if (usr.UserName.ToUpper().Contains(FilterText.ToUpper()))
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            };

            RaisePropertyChanged("SourceCollection");
            //ApplicationViewModel.GetInstance().WorkInProgress = false;
        }



        //private void GetUsers(bool loadUsers = false)
        //{

        //    ObservableCollection<User> users = new ObservableCollection<User>();

        //    if (loadUsers)
        //    {
        //        //Connect and load users
        //    }
        //    else
        //    {
        //        for (int i = 1; i < 1000; i++)
        //        {
        //            users.Add(new User()
        //            {
        //                UserId = i,
        //                UserName = "Name" + i,
        //                IsAdmin =  i % 2 == 0,
        //            });
        //        }
        //    }

        //    usersCollection = new CollectionViewSource
        //    {
        //        Source = users
        //    };
        //    usersCollection.Filter += (o, e) =>
        //    {
        //        if (string.IsNullOrEmpty(FilterText))
        //        {
        //            e.Accepted = true;
        //            return;
        //        }

        //        User usr = e.Item as User;
        //        if (usr.UserName.ToUpper().Contains(FilterText.ToUpper()))
        //        {
        //            e.Accepted = true;
        //        }
        //        else
        //        {
        //            e.Accepted = false;
        //        }
        //    };
        //}

    }
}
