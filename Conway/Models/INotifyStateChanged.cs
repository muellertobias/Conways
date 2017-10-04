using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conway.Models
{
    public class StateChangedEventArgs : EventArgs { }

    public delegate void StateChangedEventHandler(object sender, StateChangedEventArgs e);

    public interface INotifyStateChanged
    {
        event StateChangedEventHandler StateChanged;
    }
}
