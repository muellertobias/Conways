using Conway.Models;
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
        int sizeX;
        int sizeY;
        int interval; 

        public MainWindow()
        {
            InitializeComponent();

            sizeX = 90;
            sizeY = 90;

            playground = new Playground(sizeX, sizeY);
            definePlaygroundGrid();

            timer = new DispatcherTimer();
            timer.Tick += (s, x) => Update(s, new RoutedEventArgs());
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
        }

        private void definePlaygroundGrid()
        {
            for (int i = 0; i < sizeX; i++)
            {
                var column = new ColumnDefinition();
                column.Width = new GridLength(10, GridUnitType.Pixel);
                PlaygroundGrid.ColumnDefinitions.Add(column);

            }

            for (int j = 0; j < sizeY; j++)
            {
                var row = new RowDefinition();
                row.Height = new GridLength(10, GridUnitType.Pixel);
                PlaygroundGrid.RowDefinitions.Add(row);
            }

            foreach (Cell c in playground.Cells)
            {
                Button button = new Button();
                Grid.SetColumn(button, c.PosX);
                Grid.SetRow(button, c.PosY);

                button.Click += (s, e) =>
                {
                    c.Toogle();
                    Button sender = s as Button;
                    if (c.Alive)
                    {
                        sender.Background = Brushes.Green;
                    }
                    else
                    {
                        sender.Background = Brushes.LightGray;
                    }
                };

                PlaygroundGrid.Children.Add(button);
            }
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            playground.Update();
            UpdatePlaygroundGrid();
        }

        private void UpdatePlaygroundGrid()
        {
            foreach (Cell c in playground.Cells)
            {
                int index = sizeX * c.PosY + c.PosX;
                var button = PlaygroundGrid.Children[index] as Button;
                if (c.Alive)
                {
                    button.Background = Brushes.Green;
                }
                else
                {
                    button.Background = Brushes.LightGray;
                }
            }
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            playground.Clear();
            UpdatePlaygroundGrid();
            timer.Stop();
        }

        private void Random(object sender, RoutedEventArgs e)
        {
            playground.Randomize();
            UpdatePlaygroundGrid();
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
