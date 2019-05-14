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


        public RelayCommand<object> NewMovieCommand
        {
            get
            {
                if (_newMovieCommand == null)
                    _newMovieCommand = new RelayCommand<object>(NewMovieCommand_Execute);
                return _newMovieCommand;
            }
        }
        private RelayCommand<object> _newMovieCommand = null;

        private void NewMovieCommand_Execute(object obj)
        {
            Window window = new Window
            {
                Title = "New Movie",
                Content = new MovieView(this, null)
                
            };
            window.Height = 300;
            window.Width = 500;
            window.ShowDialog();
        }



        public RelayCommand<object> EditMovieCommand
        {
            get
            {
                if (_editMovieCommand == null && SelectedMovie != null)
                    _editMovieCommand = new RelayCommand<object>(EditMovieCommand_Execute);
                return _editMovieCommand;
            }
        }
        private RelayCommand<object> _editMovieCommand = null;

        private void EditMovieCommand_Execute(object obj)
        {
            Window window = new Window
            {
                Title = "New Movie",
                Content = new MovieView(this, SelectedMovie)

            };
            window.Height = 300;
            window.Width = 500;
            window.ShowDialog();
        }




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

        //private Movie _movie;
        //private ObservableCollection<GenreViewModel> _genreViewModelList = new ObservableCollection<GenreViewModel>();


        //private string _title = "";
        //public string Title
        //{
        //    get { return _title; }
        //    set
        //    {
        //        _title = value;
        //        RaisePropertyChanged("Title");
        //    }
        //}


        //private int _releaseYear = 1800;
        //public int ReleaseYear
        //{
        //    get { return _releaseYear; }
        //    set
        //    {
        //        _releaseYear = value;
        //        RaisePropertyChanged("ReleaseYear");
        //    }
        //}


        //public ObservableCollection<GenreViewModel> GenreViewModelList
        //{
        //    get { return _genreViewModelList; }
        //    set
        //    {
        //        _genreViewModelList = value;
        //        RaisePropertyChanged("GenreViewModelList");
        //    }
        //}





        //private ObservableCollection<Movie> _movieViewModelList = new ObservableCollection<Movie>();
        //public ObservableCollection<Movie> MovieViewModelList
        //{
        //    get { return _movieViewModelList; }
        //    set
        //    {
        //        _movieViewModelList = value;
        //        RaisePropertyChanged("MovieViewModelList");
        //    }
        //}

        //private bool _isNewMovie = false;
        //public bool IsNewMovie
        //{
        //    get { return _isNewMovie; }
        //    set
        //    {
        //        _isNewMovie = value;
        //        RaisePropertyChanged("IsNewMovie");
        //    }
        //}

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
            
            //GenreStuff();
            //RaisePropertyChanged("GenreViewModel");
            //RaisePropertyChanged("GenreViewModelList");
            //RaisePropertyChanged("Title");
        }


        private void GenreStuff()
        {

            //GenreViewModelList.Clear();

            //List<Genre> GenreList = new List<Genre>()
            //{
            //    new Genre(){ GenreId = 1, GenreText = "Action" },
            //    new Genre(){ GenreId = 2, GenreText = "Adventure" },
            //    new Genre(){ GenreId = 3, GenreText = "Animation" },
            //    new Genre(){ GenreId = 4, GenreText = "Children" },
            //    new Genre(){ GenreId = 5, GenreText = "Comedy" },
            //    new Genre(){ GenreId = 6, GenreText = "Crime" },
            //    new Genre(){ GenreId = 7, GenreText = "Documentary" },
            //    new Genre(){ GenreId = 8, GenreText = "Drama" },
            //    new Genre(){ GenreId = 9, GenreText = "Fantasy" },
            //    new Genre(){ GenreId = 10, GenreText = "Film-Noir" },
            //    new Genre(){ GenreId = 11, GenreText = "Horror" },
            //    new Genre(){ GenreId = 12, GenreText = "Musical" },
            //    new Genre(){ GenreId = 13, GenreText = "Mystery" },
            //    new Genre(){ GenreId = 14, GenreText = "Romance" },
            //    new Genre(){ GenreId = 15, GenreText = "Sci-Fi" },
            //    new Genre(){ GenreId = 16, GenreText = "Thriller" },
            //    new Genre(){ GenreId = 17, GenreText = "War" },
            //    new Genre(){ GenreId = 18, GenreText = "Western" }
            //};

            //foreach (Genre gen in GenreList)
            //    GenreViewModelList.Add(new GenreViewModel(gen));


            //if (model == null || model.MovieId < 1)
            //{
            //    IsNewMovie = true;
            //    _movie = new Movie();
            //}
            //else
            //{
            //    IsNewMovie = false;
            //    _movie = model;
            //    ReleaseYear = _movie.ReleaseYear;
            //    Title = _movie.Title;

            //    foreach (Genre genre in _movie.GenreList)
            //    {
            //        GenreViewModel found = GenreViewModelList.FirstOrDefault(x => x.GenreId == genre.GenreId);
            //        if (found != null)
            //            found.IsSelected = true;
            //    }

            //}

        }
        

        private void GetMovies(bool loadMovies = false)
        {

            ObservableCollection<Movie> movies = new ObservableCollection<Movie>();

            if (loadMovies)
            {
                //Connect and load users
            }
            else
            {
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
