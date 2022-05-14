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
    /// Lógica de interacción para PiezaView.xaml
    /// </summary>
    public partial class PiezaView : UserControl
    {
       
        public PiezaView()
        {
            InitializeComponent();
        }
        public PiezaView(Pieza pieza) : this()
        {
            Pieza = pieza;
            Refresh();
        }
        public Pieza Pieza { get; set; }
        public System.Drawing.Size Size { get; set; }

        public void Refresh()
        {
            if (!Equals(Pieza, default))
            {
                imgPieza.SetImage(Pieza.Render(Size));
            }
        }

    }
}
