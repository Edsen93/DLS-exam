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
    }
}
