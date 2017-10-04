using Conway.Models;
using Conway.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Conway.ViewModels
{
    public delegate void PlaygroundChangedEventHandler(Playground playground, double CellSize);

    public class MainViewModel : INotifyPropertyChanged
    {
        public PlaygroundChangedEventHandler PlaygroundChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public double CellSize { get; set; }

        public ICommand UpdateCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand RandomCommand { get; private set; }

        // "Thread"
        public ICommand StartCommand { get; private set; }
        public ICommand StopCommand { get; private set; }

        // Menu
        public ICommand NewCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }

        private Playground playground;
        private DispatcherTimer timer;

        public MainViewModel(Playground playground, double cellSize)
        {
            this.playground = playground;
            CellSize = cellSize;

            timer = new DispatcherTimer();
            timer.Tick += (s, x) => UpdateCommand.Execute(null);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            UpdateCommand = new Command(o => Update(), o => !timer.IsEnabled);
            ClearCommand = new Command(o => Clear(), o => !timer.IsEnabled);
            RandomCommand = new Command(o => Random(), o => !timer.IsEnabled);

            StartCommand = new Command(o => timer.Start(), o => !timer.IsEnabled);
            StopCommand = new Command(o => timer.Stop(), o => timer.IsEnabled);

            NewCommand = new Command(o => CreateNewPlayground());
        }

        private void Update()
        {
            playground.Update();
        }

        private void Clear()
        {
            playground.Clear();
        }

        private void Random()
        {
            playground.Randomize();
        }

        private void CreateNewPlayground()
        {
            StopCommand.Execute(null);
            ClearCommand.Execute(null);

            // TEST
            playground = new Playground(10, 10);
            PlaygroundChanged?.Invoke(playground, CellSize);
        }

        private bool IsCellInRange(Cell cell, Point point, double cellSize)
        {
            return (Math.Abs(cell.PosX * CellSize - point.X) <= cellSize / 2d)
                && (Math.Abs(cell.PosY * CellSize - point.Y) <= cellSize / 2d);
        }

        public Cell GetCell(Point point)
        {
            return playground.Cells.Find(c => IsCellInRange(c, point, CellSize));
        }

        public void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
