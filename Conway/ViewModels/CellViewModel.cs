using Conway.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conway.ViewModels
{
    public class CellViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Cell _model;

        public bool IsAlive
        {
            get
            {
                return _model.IsCurrentlyAlive;
            }
        }

        public CellViewModel(Cell model)
        {
            _model = model;
            _model.StateChanged += _model_StateChanged;
        }

        private void _model_StateChanged(object sender, StateChangedEventArgs e)
        {
            OnPropertyChanged("IsAlive");
        }

        public void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
