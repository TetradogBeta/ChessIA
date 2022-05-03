using Gabriel.Cat.S.Extension;
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

namespace Chess.UI.Wpf
{
    /// <summary>
    /// Lógica de interacción para Tablero.xaml
    /// </summary>
    public partial class Tablero : UserControl
    {
        public Tablero()
        {
            InitializeComponent();
            TableroData=new TableroData();
            TableroData.Start();
            Refresh();
        }
        public TableroData TableroData { get; set; }
        
        public void Refresh()
        {
            img.SetImage(TableroData.Render());
        }

        private void img_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TableroData.CellSelected = TableroData.TraslatePointImageToLocation(e.GetPosition(e.MouseDevice.Target).X, e.GetPosition(e.MouseDevice.Target).Y);
            Refresh();
        }
    }
}
