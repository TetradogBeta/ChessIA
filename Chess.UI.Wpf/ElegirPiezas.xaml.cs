using Chess.UI.Core;
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
    /// Lógica de interacción para ElegirPiezas.xaml
    /// </summary>
    public partial class ElegirPiezas : UserControl
    {
        public event EventHandler? SelectionChanged;
        public ElegirPiezas()
        {
            InitializeComponent();

        }
        public bool IsHoritzontal
        {
            get=>stkPiezas1.Orientation == Orientation.Horizontal;
            set
            {
                if (value)
                {
                    stkPiezas1.Orientation = Orientation.Horizontal;
                    stkPiezas2.Orientation = Orientation.Horizontal;
                }
                else
                {
                    stkPiezas1.Orientation = Orientation.Vertical;
                    stkPiezas2.Orientation = Orientation.Vertical;
                }
            }
        }
        public bool IsVertical
        {
            get => !IsHoritzontal;
            set => IsHoritzontal = !value;
        }
        public Pieza? Selected { get; set; }
        private PiezaView? LastSelected { get; set; }
        public void Load(System.Drawing.Color color1,System.Drawing.Color color2)
        {
            PiezaView pieza;
            stkPiezas1.Children.Clear();
            stkPiezas2.Children.Clear(); 

            foreach (Tipo tipo in Enum.GetValues(typeof(Tipo)))
            {
                pieza = new PiezaView(new Pieza(tipo,color1));
                pieza.MouseLeftButtonDown += PonPiezaAlPreview;
                stkPiezas1.Children.Add(pieza);
                pieza = new PiezaView(new Pieza(tipo, color2));
                pieza.MouseLeftButtonDown += PonPiezaAlPreview;
                stkPiezas2.Children.Add(pieza);
            }
        }

        private void PonPiezaAlPreview(object sender, MouseButtonEventArgs e)
        {
            PiezaView? piezaSelected = sender as PiezaView;

            if (!Equals(LastSelected, default))
                LastSelected.Background = Brushes.Transparent;

            if (sender != LastSelected)
            {
                Selected = piezaSelected?.Pieza;
                LastSelected = piezaSelected;
                if (!Equals(LastSelected, default))
                    LastSelected.Background = Brushes.LightBlue;
            }
            else
            {
                Selected = default;
                LastSelected = default;
            }

            SelectionChanged?.Invoke(this, new EventArgs());
        }
    }
}
