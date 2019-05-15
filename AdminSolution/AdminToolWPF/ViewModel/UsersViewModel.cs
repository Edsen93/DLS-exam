using AdminToolWPF.Helper_Classes;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminToolWPF;

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
        
    }
}
