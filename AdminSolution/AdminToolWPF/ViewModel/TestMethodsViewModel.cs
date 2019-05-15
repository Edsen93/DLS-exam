using AdminToolWPF.Helper_Classes;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminToolWPF.ViewModel
{
    public class TestMethodsViewModel : ViewModelBase, IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Test methods";
            }
        }


        private string _movieID;
        public string MovieID
        {
            get { return _movieID; }
            set
            {
                _movieID = value;
                RaisePropertyChanged("MovieID");
            }
        }


        private string _movieTitle;
        public string MovieTitle
        {
            get { return _movieTitle; }
            set
            {
                _movieTitle = value;
                RaisePropertyChanged("MovieTitle");
            }
        }


        private string _userID;
        public string UserID
        {
            get { return _userID; }
            set
            {
                _userID = value;
                RaisePropertyChanged("UserID");
            }
        }


        private string _consoleText;
        public string ConsoleText
        {
            get { return _consoleText; }
            set
            {
                _consoleText = value;
                RaisePropertyChanged("ConsoleText");
            }
        }



        


        public IRelayCommand GetAllMoviesCommand => new RelayCommand(() =>
        {
            ConsoleText = RequestHandler.Get($"{ConnetionSettings.PostgresMoviesAddress}/api/movies");
        });

        public IRelayCommand GetMovieByIDCommand => new RelayCommand(() =>
        {
            ConsoleText = RequestHandler.Get($"{ConnetionSettings.PostgresMoviesAddress}/api/movies/{MovieID}");
        }, () => !String.IsNullOrWhiteSpace(MovieID));


   
        public IRelayCommand GetMovieByTitleCommand => new RelayCommand(() =>
        {
            ConsoleText = RequestHandler.Get($"{ConnetionSettings.PostgresMoviesAddress}/api/movies/{MovieTitle}");
        }, () => !String.IsNullOrWhiteSpace(MovieTitle));




        public IRelayCommand GetAllUsersCommand => new RelayCommand(() =>
        {
            ConsoleText = RequestHandler.Get($"{ConnetionSettings.PostgresUserAddress}/api/users");
        });


        public IRelayCommand GetUserByIDCommand => new RelayCommand(() =>
        {
            ConsoleText = RequestHandler.Get($"{ConnetionSettings.PostgresUserAddress}/api/users/{UserID}");
        },() => !String.IsNullOrWhiteSpace(UserID));

        
    }
}
