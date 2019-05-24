using AdminToolWPF.Helper_Classes;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        private string _userIDRecommendation;
        public string UserIDRecommendation
        {
            get { return _userIDRecommendation; }
            set
            {
                _userIDRecommendation = value;
                RaisePropertyChanged("UserIDRecommendation");
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


        private string _currentSentQuerry;
        public string CurrentSentQuerry
        {
            get { return _currentSentQuerry; }
            set
            {
                _currentSentQuerry = value;
                RaisePropertyChanged("CurrentSentQuerry");
            }
        }






        public IRelayCommand GetAllMoviesCommand => new RelayCommand(() =>
        {
            CurrentSentQuerry = $"{ConnetionSettings.AdminServiceAddress}/api/movies";

            DoWork();

        });

        public IRelayCommand GetMovieByIDCommand => new RelayCommand(() =>
        {
            CurrentSentQuerry = $"{ConnetionSettings.AdminServiceAddress}/api/movie/full/{MovieID}";

            DoWork();

        }, () => !String.IsNullOrWhiteSpace(MovieID));


   
        public IRelayCommand GetMovieByTitleCommand => new RelayCommand(() =>
        {
            //CurrentSentQuerry = $"{ConnetionSettings.AdminServiceAddress}/api/movie/{MovieTitle.Replace(" ", "%20")}";
            //ConsoleText = RequestHandler.Get(CurrentSentQuerry);

            //JToken parsedJson = JToken.Parse(ConsoleText);
            //ConsoleText = parsedJson.ToString(Formatting.Indented);
            ConsoleText = "Not implemented";

            //TO DO

        }, () => !String.IsNullOrWhiteSpace(MovieTitle));




        public IRelayCommand GetAllUsersCommand => new RelayCommand(() =>
        {
            CurrentSentQuerry = $"{ConnetionSettings.AdminServiceAddress}/api/user";

            DoWork();

        });


        public IRelayCommand GetUserByIDCommand => new RelayCommand(() =>
        {
            CurrentSentQuerry = $"{ConnetionSettings.AdminServiceAddress}/api/user/{UserID}";

            DoWork();

        }, () => !String.IsNullOrWhiteSpace(UserID));


        public IRelayCommand SendQuerry => new RelayCommand(() =>
        {
            DoWork();

        }, () => !String.IsNullOrWhiteSpace(CurrentSentQuerry));



        public IRelayCommand GetMovieRecommendationCommand => new RelayCommand(() =>
        {

            CurrentSentQuerry = $"{ConnetionSettings.AdminServiceAddress}/api/recommendation/{UserIDRecommendation}";
            
            //ConsoleText = RequestHandler.Get(CurrentSentQuerry);

            //JToken parsedJson = JToken.Parse(ConsoleText);
            //ConsoleText = parsedJson.ToString(Formatting.Indented);


            DoWork();

        }, () => !String.IsNullOrWhiteSpace(UserIDRecommendation));



        private Task<string> DoWorkAsync()
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            Task.Run(() =>
            {
                ConsoleText = RequestHandler.Get(CurrentSentQuerry);
                tcs.SetResult(ConsoleText);
            });
            //return the Task
            return tcs.Task;
        }

        private async void DoWork()
        {
            ConsoleText = "the task is running, please wait";
            //ApplicationViewModel.WorkInProgress = true;

            string result = await DoWorkAsync();


            JToken parsedJson = JToken.Parse(result);
            ConsoleText = parsedJson.ToString(Formatting.Indented);
        }


    }
}
