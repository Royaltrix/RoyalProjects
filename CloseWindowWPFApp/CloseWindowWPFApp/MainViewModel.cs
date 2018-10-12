using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CloseWindowWPFApp
{
    public class MainViewModel
    {
        //Properties
        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }

        //Constructor
        public MainViewModel()
        {
            this.CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
        }

        //Methods
        private void CloseWindow(IClosable window)
        {
            if (window != null)
                window.Close();
        }
    }
}
