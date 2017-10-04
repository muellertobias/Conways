using Conway.Models;
using Conway.ViewModels;
using Conway.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Conway
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Playground playground;
        DispatcherTimer timer;
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public double CellSize { get; set; } 


        public List<CellViewModel> _viewModels = new List<CellViewModel>();

        public MainWindow()
        {
            InitializeComponent();

            SizeX = 50;
            SizeY = 50;
            CellSize = 10d;

            playground = new Playground(SizeX, SizeY);
            DefinePlaygroundGrid();

            timer = new DispatcherTimer();
            timer.Tick += (s, x) => Update(s, new RoutedEventArgs());
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
        }

        private void DefinePlaygroundGrid()
        {
            for (int i = 0; i < SizeX; i++)
            {
                var column = new ColumnDefinition()
                {
                    Width = new GridLength(CellSize, GridUnitType.Pixel)
                };
                PlaygroundGrid.ColumnDefinitions.Add(column);

            }

            for (int j = 0; j < SizeY; j++)
            {
                var row = new RowDefinition()
                {
                    Height = new GridLength(CellSize, GridUnitType.Pixel)
                };
                PlaygroundGrid.RowDefinitions.Add(row);
            }

            foreach (Cell c in playground.Cells)
            {                
                var vm = new CellViewModel(c);
                CellView view = new CellView()
                {
                    DataContext = vm
                };
                _viewModels.Add(vm);

                Grid.SetColumn(view, c.PosX);
                Grid.SetRow(view, c.PosY);
                PlaygroundGrid.Children.Add(view);
            }
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            playground.Update();
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            playground.Clear();
            timer.Stop();
        }

        private void Random(object sender, RoutedEventArgs e)
        {
            playground.Randomize();
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            if (!timer.IsEnabled)
                timer.Start();
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            //string test = playground.Save();
        }

        Point currentPoint = new Point();
        private void PlaygroundGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                currentPoint = e.GetPosition(sender as IInputElement);
                var startCell = playground.Cells.Find(c => Match(c, currentPoint, CellSize));
                if (startCell != null)
                    startCell.IsCurrentlyAlive = true;
            }
        }

        private bool Match(Cell cell, Point point, double cellSize)
        {
            return (Math.Abs(cell.PosX * CellSize - point.X) <= cellSize / 2d)
                && (Math.Abs(cell.PosY * CellSize - point.Y) <= cellSize / 2d);
        }

        private void PlaygroundGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var startCell = playground.Cells.Find(c => Match(c, currentPoint, CellSize));
                Point newPosition = e.GetPosition(sender as IInputElement);
                var endCell = playground.Cells.Find(c => Match(c, newPosition, CellSize));

                if (startCell != null)
                    startCell.IsCurrentlyAlive = true;

                if (endCell != null)
                    endCell.IsCurrentlyAlive = true;

                currentPoint = newPosition;
            }

        }

        private List<Cell> GetCellsBetweenPoints(Point start, Point end)
        {
            List<Cell> selectedCells = new List<Cell>();

            return selectedCells;
        }

        private void PlaygroundGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Cross;
        }

        private void PlaygroundGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }
    }
}
