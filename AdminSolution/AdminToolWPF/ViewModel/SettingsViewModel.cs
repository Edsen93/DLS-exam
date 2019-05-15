using AdminToolWPF.Helper_Classes;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminToolWPF.ViewModel
{
    public class SettingsViewModel : ViewModelBase, IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Settings";
            }
        }



        // Recommendation
        public string Neo4jAddress
        {
            get { return ConnetionSettings.Neo4jAddress; }
            set
            {
                ConnetionSettings.Neo4jAddress = value;
                RaisePropertyChanged("Neo4jAddress");
            }
        }
        
        public string Neo4jUserName
        {
            get { return ConnetionSettings.Neo4jUserName; }
            set
            {
                ConnetionSettings.Neo4jUserName = value;
                RaisePropertyChanged("Neo4jUserName");
            }
        }

        public string Neo4jUserPassword
        {
            get { return ConnetionSettings.Neo4jUserPassword; }
            set
            {
                ConnetionSettings.Neo4jUserName = value;
                RaisePropertyChanged("Neo4jUserPassword");
            }
        }


        // MovieSearch
        public string PostgresMoviesAddress
        {
            get { return ConnetionSettings.PostgresMoviesAddress; }
            set
            {
                ConnetionSettings.PostgresMoviesAddress = value;
                RaisePropertyChanged("PostgresMoviesAddress");
            }
        }
        
        public string PostgresMoviesUserName
        {
            get { return ConnetionSettings.PostgresMoviesUserName; }
            set
            {
                ConnetionSettings.PostgresMoviesUserName = value;
                RaisePropertyChanged("PostgresMoviesUserName");
            }
        }

        public string PostgresMoviesUserPassword
        {
            get { return ConnetionSettings.PostgresMoviesUserPassword; }
            set
            {
                ConnetionSettings.PostgresMoviesUserPassword = value;
                RaisePropertyChanged("PostgresMoviesUserPassword");
            }
        }


        // UserInfo
        public string PostgresUserAddress
        {
            get { return ConnetionSettings.PostgresUserAddress; }
            set
            {
                ConnetionSettings.PostgresUserAddress = value;
                RaisePropertyChanged("PostgresUserAddress");
            }
        }
        
        public string PostgresUserUserName
        {
            get { return ConnetionSettings.PostgresUserUserName; }
            set
            {
                ConnetionSettings.PostgresUserUserName = value;
                RaisePropertyChanged("PostgresUserUserName");
            }
        }

        public string PostgresUserUserPassword
        {
            get { return ConnetionSettings.PostgresUserUserPassword; }
            set
            {
                ConnetionSettings.PostgresUserUserPassword = value;
                RaisePropertyChanged("PostgresUserUserPassword");
            }
        }

    }
}
