﻿using AdminToolWPF.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminToolWPF.ViewModel
{
    public class GenreViewModel : ViewModelBase
    {
        private Genre _genre;

        public string GenreText { get { return _genre.GenreText; } }
        public int GenreId { get { return _genre.GenreId; } }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public GenreViewModel(Genre genre)
        {
            _genre = genre;
            RaisePropertyChanged();
        }
    }
}
