using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Artichoke.xamarin.Models;

namespace Artichoke.xamarin.Models
{
	public class ObservableFamily : Family, INotifyPropertyChanged
	{
		public ObservableFamily()
		{
            
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string IName
        {
            get
            {
                return this.Name;
            }

            set
            {
                if (value != this.Name)
                {
                    this.Name = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}

