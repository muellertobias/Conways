using Conway.Models;
using Conway.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;

namespace Conway.ViewModels
{
    public delegate void PlaygroundChangedEventHandler(Playground Playground, double CellSize);

    public class MainViewModel : INotifyPropertyChanged
    {
        public PlaygroundChangedEventHandler PlaygroundChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool? Applicator { get; set; }

        public double CellSize { get; set; }

        private int _generation;
        public int Generation
        {
            get { return _generation; }
            private set
            {
                if (_generation != value)
                {
                    _generation = value;
                    OnPropertyChanged("Generation");
                }
            }
        }

        private string _softwareName;
        public string SoftwareName
        {
            get { return _softwareName; }
            private set
            {
                if (_softwareName != value)
                {
                    _softwareName = value;
                    OnPropertyChanged("SoftwareName");
                }
            }
        }

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
        public ICommand CloseCommand { get; private set; }

        private Playground playground;
        private DispatcherTimer timer;
        private string filename;

        public MainViewModel(Playground playground, double cellSize)
        {
            this.playground = playground;
            CellSize = cellSize;

            timer = new DispatcherTimer(DispatcherPriority.Send);
            timer.Tick += (s, x) => UpdateCommand.Execute(null);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            UpdateCommand = new Command(o => Update(), o => !timer.IsEnabled);
            ClearCommand = new Command(o => Clear(), o => !timer.IsEnabled);
            RandomCommand = new Command(o => Random(), o => !timer.IsEnabled);

            StartCommand = new Command(o => timer.Start(), o => !timer.IsEnabled);
            StopCommand = new Command(o => timer.Stop(), o => timer.IsEnabled);

            NewCommand = new Command(o => CreateNewPlayground());
            OpenCommand = new Command(o => Open());
            SaveCommand = new Command(o => Save());
            SaveAsCommand = new Command(o => SaveAs());

            CloseCommand = new Command(o => System.Windows.Application.Current.Shutdown());

            SoftwareName = string.Format("Conway's Game of Life: Neu {0}x{1}", playground.SizeX, playground.SizeY);
        }

        private void Update()
        {
            playground.Update();
            Generation++;
        }

        private void Clear()
        {
            playground.Clear();
            Generation = 0;
        }

        private void Random()
        {
            playground.Randomize();
            Generation = 0;
        }

        private void Open()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }

            using (StreamReader reader = new StreamReader(dialog.OpenFile(), Encoding.ASCII))
            {
                playground.Load(reader.ReadToEnd());
            }
            filename = dialog.FileName;
            PlaygroundChanged?.Invoke(playground, CellSize);
            dialog.Dispose();

            Generation = 0;
            SoftwareName = string.Format("Conway's Game of Life: {2} {0}x{1}", playground.SizeX, playground.SizeY, filename);
        }

        private void Save()
        {
            string content = playground.Save();

            if (string.IsNullOrEmpty(filename))
            {
                filename = SaveAs();
                SoftwareName = string.Format("Conway's Game of Life: {2} {0}x{1}", playground.SizeX, playground.SizeY, filename);
            }
            else
            {
                FileStream stream = new FileStream(filename, FileMode.Open);
                using (StreamWriter writer = new StreamWriter(stream, Encoding.ASCII))
                {
                    writer.Write(content);
                }
                stream.Close();
            }
        }

        private string SaveAs()
        {
            string content = playground.Save();
            SaveFileDialog dialog = new SaveFileDialog();
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                return string.Empty;
            }

            using (StreamWriter writer = new StreamWriter(dialog.OpenFile(), Encoding.ASCII))
            {
                writer.Write(content);
            }
            string filename = dialog.FileName;
            dialog.Dispose();
            return filename;
        }

        private void CreateNewPlayground()
        {
            Generation = 0;
            StopCommand.Execute(null);
            ClearCommand.Execute(null);

            NewPlaygroundDialog dialog = new NewPlaygroundDialog();
            if (dialog.ShowDialog() == true)
            {
                int SizeX = dialog.SizeX;
                int SizeY = dialog.SizeY;
                CellSize = dialog.CellSize;

                playground = new Playground(SizeX, SizeY);
                PlaygroundChanged?.Invoke(playground, CellSize);

                SoftwareName = string.Format("Conway's Game of Life: Neu {0}x{1}", playground.SizeX, playground.SizeY, filename);
            }
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
