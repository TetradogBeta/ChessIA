using Chess.UI.Core;
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
        public event EventHandler<PosicionTableroEventArgs>? Clicked;
        public Tablero() : this(true) { }
        public Tablero(bool renderColor1,bool init=false, System.Drawing.Color? colorLeftClick=default, System.Drawing.Color? colorRightClick=default)
        {
            if (Equals(colorLeftClick, default))
            {
                colorLeftClick = System.Drawing.Color.Gold;
            }
            if (Equals(colorRightClick, default))
            {
                colorRightClick = System.Drawing.Color.Indigo;
            }

            InitializeComponent();

            ColorLeftClick = colorLeftClick.Value;
            ColorRightClick = colorRightClick.Value;
            RenderColor1 = renderColor1;
            TableroData = new TableroData();

            if (!TableroData.DicCellsSelecteds.ContainsKey(ColorLeftClick))
            {
                TableroData.DicCellsSelecteds.Add(ColorLeftClick, new List<System.Drawing.Point>());
            }
            if (!TableroData.DicCellsSelecteds.ContainsKey(ColorRightClick))
            {
                TableroData.DicCellsSelecteds.Add(ColorRightClick, new List<System.Drawing.Point>());
            }


            if (init)
            {
                TableroData.Start();
           
            }

            Refresh();
        }
        public TableroData TableroData { get; set; }
        public bool DoubleSelection { get; set; }

        public bool DisableSelection { get; set; }

        public System.Drawing.Color ColorLeftClick { get; private set; }
        public System.Drawing.Color ColorRightClick { get; private set; }
        public bool RenderColor1 { get; set; }
        public void Refresh()
        {
            img.SetImage(TableroData.Render(RenderColor1));
        }

        private void img_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Drawing.Point location = TableroData.TraslatePointImageToLocation(e.GetPosition(e.MouseDevice.Target).X, e.GetPosition(e.MouseDevice.Target).Y,RenderColor1);

            if(!DisableSelection && (DoubleSelection || !TableroData.DicCellsSelecteds[ColorLeftClick].Any(l => l.Equals(location))))
            {
                if (TableroData.DicCellsSelecteds[ColorLeftClick].Any(l => l.Equals(location)))
                {

                    TableroData.DicCellsSelecteds[ColorLeftClick].Remove(location);
                }
                else
                {
                    TableroData.DicCellsSelecteds[ColorLeftClick].Add(location);
                }
                Refresh();


            }
            if (Clicked != null)
                Clicked(this, new PosicionTableroEventArgs(location));
        }

        private void img_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Drawing.Point location = TableroData.TraslatePointImageToLocation(e.GetPosition(e.MouseDevice.Target).X, e.GetPosition(e.MouseDevice.Target).Y,RenderColor1);
            if (!DisableSelection && (DoubleSelection || !TableroData.DicCellsSelecteds[ColorRightClick].Any(l => l.Equals(location))))
            {
                if (TableroData.DicCellsSelecteds[ColorRightClick].Any(l => l.Equals(location)))
                {

                    TableroData.DicCellsSelecteds[ColorRightClick].Remove(location);
                }
                else
                {
                    TableroData.DicCellsSelecteds[ColorRightClick].Add(location);
                }
                Refresh();

            }
            if (Clicked != null)
                Clicked(this, new PosicionTableroEventArgs(location));
        }
    }
    public class PosicionTableroEventArgs : EventArgs
    {
        public PosicionTableroEventArgs(System.Drawing.Point posicion)=>Posicion = posicion;
        public System.Drawing.Point Posicion { get; set; }
    }
}
