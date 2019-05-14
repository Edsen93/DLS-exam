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

    public class MovieViewModel : ViewModelBase
    {
        private Movie _movie;
        private MoviesViewModel _parrent;
        private ObservableCollection<GenreViewModel> _genreViewModelList = new ObservableCollection<GenreViewModel>();

        private string _title = "";
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }


        private int _releaseYear = 1800;
        public int ReleaseYear
        {
            get { return _releaseYear; }
            set
            {
                _releaseYear = value;
                RaisePropertyChanged("ReleaseYear");
            }
        }


        public ObservableCollection<GenreViewModel> GenreViewModelList
        {
            get { return _genreViewModelList; }
            set
            {
                _genreViewModelList = value;
                RaisePropertyChanged("GenreViewModelList");
            }
        }

        private bool _isNewMovie = false;
        public bool IsNewMovie
        {
            get { return _isNewMovie; }
            set
            {
                _isNewMovie = value;
                RaisePropertyChanged("IsNewMovie");
            }
        }


        public MovieViewModel(MoviesViewModel parrent = null, Movie model = null)
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

            _parrent = parrent;

            GenreViewModelList.Clear();

            List<Genre> GenreList = new List<Genre>()
            {
                new Genre(){ GenreId = 1, GenreText = "Action" },
                new Genre(){ GenreId = 2, GenreText = "Adventure" },
                new Genre(){ GenreId = 3, GenreText = "Animation" },
                new Genre(){ GenreId = 4, GenreText = "Children" },
                new Genre(){ GenreId = 5, GenreText = "Comedy" },
                new Genre(){ GenreId = 6, GenreText = "Crime" },
                new Genre(){ GenreId = 7, GenreText = "Documentary" },
                new Genre(){ GenreId = 8, GenreText = "Drama" },
                new Genre(){ GenreId = 9, GenreText = "Fantasy" },
                new Genre(){ GenreId = 10, GenreText = "Film-Noir" },
                new Genre(){ GenreId = 11, GenreText = "Horror" },
                new Genre(){ GenreId = 12, GenreText = "Musical" },
                new Genre(){ GenreId = 13, GenreText = "Mystery" },
                new Genre(){ GenreId = 14, GenreText = "Romance" },
                new Genre(){ GenreId = 15, GenreText = "Sci-Fi" },
                new Genre(){ GenreId = 16, GenreText = "Thriller" },
                new Genre(){ GenreId = 17, GenreText = "War" },
                new Genre(){ GenreId = 18, GenreText = "Western" }
            };

            foreach (Genre gen in GenreList)
                GenreViewModelList.Add(new GenreViewModel(gen));


            if (model == null || model.MovieId < 1)
            {
                IsNewMovie = true;
                _movie = new Movie();
            }
            else
            {
                IsNewMovie = false;
                _movie = model;
                ReleaseYear = _movie.ReleaseYear;
                Title = _movie.Title;

                foreach (Genre genre in _movie.GenreList)
                {
                    GenreViewModel found = GenreViewModelList.FirstOrDefault(x => x.GenreId == genre.GenreId);
                    if (found != null)
                        found.IsSelected = true;
                }

            }


            RaisePropertyChanged();
            RaisePropertyChanged("GenreViewModel");
            RaisePropertyChanged("GenreViewModelList");
            RaisePropertyChanged("Title");
        }


    }

}
