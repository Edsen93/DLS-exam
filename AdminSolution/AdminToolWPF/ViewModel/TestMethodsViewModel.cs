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
            SetDemo();

        });

        public IRelayCommand GetMovieByIDCommand => new RelayCommand(() =>
        {

            SetDemo();
        }, () => !String.IsNullOrWhiteSpace(MovieID));


   
        //public IRelayCommand<string> GetMovieByTitleCommand => new RelayCommand<string>((string movieTitle) =>
        //{
        //    SetDemo();
        //}, (movieTitle) => movieTitle != null);

        public IRelayCommand GetMovieByTitleCommand => new RelayCommand(() =>
        {
            SetDemo();
        }, () => !String.IsNullOrWhiteSpace(MovieTitle));




        public IRelayCommand GetAllUsersCommand => new RelayCommand(() =>
        {

            SetDemo();
        });


        public IRelayCommand GetUserByIDCommand => new RelayCommand(() =>
        {
            SetDemo();
        },() => !String.IsNullOrWhiteSpace(UserID));



        private void SetDemo()
        {
            ConsoleText = @"
Lorem ipsum dolor sit amet, consectetur adipiscing elit.Morbi rutrum magna est, ut placerat nisl posuere quis. Nam sed odio in tellus pretium molestie.Sed eleifend risus cursus dolor auctor, et sodales quam luctus.Maecenas congue varius hendrerit. Aliquam sem dui, rhoncus vitae nunc nec, ultricies iaculis nulla. Curabitur et nibh enim. Nam id pulvinar lorem, viverra lacinia eros. Pellentesque lacinia dolor dolor, ut commodo mi condimentum vitae. Pellentesque at orci dolor. Integer ut malesuada dui, efficitur posuere dui. Sed iaculis elit est, ut placerat est vulputate sed. Integer nec condimentum elit, quis blandit eros.

Vivamus posuere cursus ex, in elementum leo malesuada congue. Fusce pellentesque, purus nec dignissim faucibus, mi leo ultricies magna, ut finibus metus leo ac felis.Maecenas id quam a turpis gravida tincidunt.Lorem ipsum dolor sit amet, consectetur adipiscing elit.Nam gravida, tortor nec auctor pellentesque, orci augue semper mi, ac vehicula nunc ante eget ante.Proin sagittis lectus augue, eget convallis libero ullamcorper in. Sed imperdiet rhoncus egestas. Nulla sit amet hendrerit purus.Nam tristique nulla nec urna feugiat finibus.Suspendisse at leo eu ligula lobortis congue.Phasellus interdum leo consequat faucibus efficitur. In justo tellus, viverra eu felis eget, bibendum porta augue. Mauris blandit finibus ligula, eu volutpat libero eleifend consectetur. Integer non scelerisque diam, a ultricies lectus.

Integer quis auctor mauris. Nulla pulvinar interdum sem vel tempus. Phasellus mattis risus vel iaculis iaculis. In ac fringilla nibh. Pellentesque quam tortor, mattis eu semper at, placerat eu erat. Maecenas condimentum hendrerit quam, eget tristique orci pulvinar malesuada. Donec iaculis tellus vitae mauris malesuada efficitur.Quisque faucibus tincidunt lacus, sed convallis eros.

Nunc arcu libero, dapibus vitae rhoncus eget, molestie eu purus. Ut eget euismod ante, nec sodales mauris. Aenean sed euismod risus. Nunc magna enim, laoreet et elementum blandit, volutpat quis leo. Fusce at sagittis tellus. Praesent ac euismod lorem, quis semper diam. Vivamus eget urna ac nisi facilisis aliquam.Morbi quam dui, feugiat quis porttitor in, posuere nec ipsum.Nulla facilisis suscipit nulla nec scelerisque. Integer eros dolor, suscipit non malesuada quis, porta sit amet lorem.Nulla facilisi. Nam id arcu cursus, rutrum nisi vitae, molestie nisl.

Proin elementum semper ipsum, sed imperdiet enim mollis sed. Nulla vel mauris dignissim, auctor lectus vel, aliquam tortor.Aliquam libero tellus, accumsan sed massa pretium, ultricies laoreet nunc. Morbi non dictum dui, sit amet faucibus purus.Sed in metus nec neque pellentesque condimentum.Duis pulvinar nec nulla vitae finibus. Mauris ornare quis diam in accumsan.Phasellus sit amet mauris aliquam elit euismod tempor sed eu arcu.";
        }
    }
}
