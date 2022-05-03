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
        public event EventHandler Clicked;
        public Tablero()
        {
            InitializeComponent();
            TableroData=new TableroData();
            TableroData.Start();
            Refresh();
        }
        public TableroData TableroData { get; set; }
        public bool DoubleSelection { get; set; }
        
        public void Refresh()
        {
            img.SetImage(TableroData.Render());
        }

        private void img_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Drawing.Point location = TableroData.TraslatePointImageToLocation(e.GetPosition(e.MouseDevice.Target).X, e.GetPosition(e.MouseDevice.Target).Y);
            if (DoubleSelection || !TableroData.CellsSelected2.Any(l => l.Equals(location)))
            {
                if (TableroData.CellsSelected1.Any(l => l.Equals(location)))
                {

                    TableroData.CellsSelected1.Remove(location);
                }
                else
                {
                    TableroData.CellsSelected1.Add(location);
                }
                Refresh();

                if (Clicked != null)
                    Clicked(this, new EventArgs());
            }
        }

        private void img_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Drawing.Point location = TableroData.TraslatePointImageToLocation(e.GetPosition(e.MouseDevice.Target).X, e.GetPosition(e.MouseDevice.Target).Y);
            if (DoubleSelection || !TableroData.CellsSelected1.Any(l => l.Equals(location)))
            {
                if (TableroData.CellsSelected2.Any(l => l.Equals(location)))
                {

                    TableroData.CellsSelected2.Remove(location);
                }
                else
                {
                    TableroData.CellsSelected2.Add(location);
                }
                Refresh();
           

            if (Clicked != null)
                Clicked(this, new EventArgs()); 
            
            }
        }
    }
}
