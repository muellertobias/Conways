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
using System.Windows.Shapes;

namespace Conway.Utilities
{
    /// <summary>
    /// Interaktionslogik für NewPlaygroundDialog.xaml
    /// </summary>
    public partial class NewPlaygroundDialog : Window
    {
        public int SizeX
        {
            get { return int.Parse(txtWidth.Text); }
        }

        public int SizeY
        {
            get { return int.Parse(txtHeight.Text); }
        }

        public int CellSize
        {
            get { return int.Parse(txtCellSize.Text); }
        }

        public NewPlaygroundDialog()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtWidth.SelectAll();
            txtWidth.Focus();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
