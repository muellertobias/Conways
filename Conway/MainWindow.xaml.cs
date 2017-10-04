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

        public List<CellViewModel> _viewModels = new List<CellViewModel>();

        public MainWindow()
        {
            InitializeComponent();

            SizeX = 100;
            SizeY = 100;

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
                    Width = new GridLength(7, GridUnitType.Pixel)
                };
                PlaygroundGrid.ColumnDefinitions.Add(column);

            }

            for (int j = 0; j < SizeY; j++)
            {
                var row = new RowDefinition()
                {
                    Height = new GridLength(7, GridUnitType.Pixel)
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
    }
}
