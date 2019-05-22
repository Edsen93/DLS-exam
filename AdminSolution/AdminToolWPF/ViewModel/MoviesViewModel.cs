using AdminToolWPF.Helper_Classes;
using AdminToolWPF.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using AdminToolWPF.View;

namespace AdminToolWPF.ViewModel
{
    public class MoviesViewModel : ViewModelBase, IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Movies";
            }
        }


        //public RelayCommand<object> NewMovieCommand
        //{
        //    get
        //    {
        //        if (_newMovieCommand == null)
        //            _newMovieCommand = new RelayCommand<object>(NewMovieCommand_Execute);
        //        return _newMovieCommand;
        //    }
        //}
        //private RelayCommand<object> _newMovieCommand = null;

        //private void NewMovieCommand_Execute(object obj)
        //{
        //    Window window = new Window
        //    {
        //        Title = "New Movie",
        //        Content = new MovieView(this, null)
                
        //    };
        //    window.Height = 300;
        //    window.Width = 500;
        //    window.ShowDialog();
        //}



        //public RelayCommand<object> EditMovieCommand
        //{
        //    get
        //    {
        //        if (_editMovieCommand == null && SelectedMovie != null)
        //            _editMovieCommand = new RelayCommand<object>(EditMovieCommand_Execute);
        //        return _editMovieCommand;
        //    }
        //}
        //private RelayCommand<object> _editMovieCommand = null;

        //private void EditMovieCommand_Execute(object obj)
        //{
        //    Window window = new Window
        //    {
        //        Title = "New Movie",
        //        Content = new MovieView(this, SelectedMovie)

        //    };
        //    window.Height = 300;
        //    window.Width = 500;
        //    window.ShowDialog();
        //}



        public IRelayCommand NewMovieCommand => new RelayCommand(() =>
        {
            Window window = new Window
            {
                Title = "New Movie",
                Content = new MovieView(this, null)

            };
            window.Height = 300;
            window.Width = 500;
            window.ShowDialog();
        });


        public IRelayCommand EditMovieCommand => new RelayCommand(() =>
        {
            Window window = new Window
            {
                Title = "New Movie",
                Content = new MovieView(this, SelectedMovie)

            };
            window.Height = 300;
            window.Width = 500;
            window.ShowDialog();
        }, () => SelectedMovie != null);


        public IRelayCommand DeleteMovieCommand => new RelayCommand(() =>
        {
            string sMessageBoxText = $"Do you want to DELETE '{SelectedMovie.Title}' ?";
            string sCaption = "Delete movie";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            if(rsltMessageBox == MessageBoxResult.Yes)
            {
                // run code
                //moviesCollection.remo
            }

        }, () => SelectedMovie != null);





        public Movie _selectedMovie;
        public Movie SelectedMovie
        {
            get { return _selectedMovie; }
            set
            {
                _selectedMovie = value;
                RaisePropertyChanged("SelectedMovie");
            }
        }
        

        private CollectionViewSource moviesCollection;

        public ICollectionView SourceCollection
        {
            get
            {
                return this.moviesCollection.View;
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
                this.moviesCollection.View.Refresh();
                RaisePropertyChanged("FilterText");
            }
        }

        
        public MoviesViewModel(Movie model = null)
        {
            if (IsInDesignMode || ViewModelBase.IsInDesignModeStatic)
            {
                model = new Movie()
                {
                    Title = "Test Movie",
                    ReleaseYear = 1999,
                    MovieId = 9000,
                    GenreList = new List<Genre>() {
                        new Genre(){ GenreId = 1, GenreText = "Action" }
                    }
                };

            }
            

            GetMovies();
            
        }

        

        private void GetMovies(bool loadMovies = true)
        {

            ObservableCollection<Movie> movies = null;

            if (loadMovies)
            {
                //Connect and load users

                var test = QuerryHandler.GetMovies();

                movies = new ObservableCollection<Movie>(test);
                
            }
            else
            {
                movies = new ObservableCollection<Movie>();
                for (int i = 1; i < 1000; i++)
                {
                    movies.Add(new Movie() {
                        MovieId = i,
                        Title = "Title"+i,
                        ReleaseYear = 1999
                    });
                }
            }
            
            moviesCollection = new CollectionViewSource
            {
                Source = movies
            };
            moviesCollection.Filter += (o, e) =>
            {
                if (string.IsNullOrEmpty(FilterText))
                {
                    e.Accepted = true;
                    return;
                }

                Movie usr = e.Item as Movie;
                if (usr.Title.ToUpper().Contains(FilterText.ToUpper()))
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            };
        }
    }

}
