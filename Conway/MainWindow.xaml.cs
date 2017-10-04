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
        private MainViewModel _viewModel;
        private Point currentPoint;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            currentPoint = new Point();
            int SizeX = 50;
            int SizeY = 50;
            double cellSize = 10d;

            Playground playground = new Playground(SizeX, SizeY);

            _viewModel = new MainViewModel(playground, cellSize);
            _viewModel.PlaygroundChanged += DefinePlaygroundGrid;

            DefinePlaygroundGrid(playground, _viewModel.CellSize);
            DataContext = _viewModel;
        }

        private void DefinePlaygroundGrid(Playground playground, double cellSize)
        {
            PlaygroundGrid.Children.Clear();

            for (int i = 0; i < playground.SizeX; i++)
            {
                var column = new ColumnDefinition()
                {
                    Width = new GridLength(cellSize, GridUnitType.Pixel)
                };
                PlaygroundGrid.ColumnDefinitions.Add(column);
            }

            for (int j = 0; j < playground.SizeY; j++)
            {
                var row = new RowDefinition()
                {
                    Height = new GridLength(cellSize, GridUnitType.Pixel)
                };
                PlaygroundGrid.RowDefinitions.Add(row);
            }

            foreach (Cell c in playground.Cells)
            {                
                CellView view = new CellView()
                {
                    DataContext = new CellViewModel(c)
                };

                Grid.SetColumn(view, c.PosX);
                Grid.SetRow(view, c.PosY);
                PlaygroundGrid.Children.Add(view);
            }
        }

        private void PlaygroundGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                currentPoint = e.GetPosition(sender as IInputElement);
                var startCell = _viewModel.GetCell(currentPoint);
                if (startCell != null)
                    startCell.IsCurrentlyAlive = true;
            }
        }

        private void PlaygroundGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var startCell = _viewModel.GetCell(currentPoint);
                Point newPosition = e.GetPosition(sender as IInputElement);
                var endCell = _viewModel.GetCell(newPosition);

                if (startCell != null)
                    startCell.IsCurrentlyAlive = true;

                if (endCell != null)
                    endCell.IsCurrentlyAlive = true;

                currentPoint = newPosition;
            }
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
