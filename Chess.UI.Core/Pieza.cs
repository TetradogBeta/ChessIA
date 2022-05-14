using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.UI.Core
{
    public enum Tipo
    {
        Peon, Alfil, Caballo, Torre, Reina, Rey
    }
    public class Pieza
    {
        public static int DefaultHeight = 150;
        public static int DefaultWidth = 150;
        public static Color DefaultBrush = Color.Blue;
        private Tipo tipo;
        private Color color;
        private Size? lastSize = default;
        private Bitmap? lastRender = default;
        public Pieza(Tipo tipo, Color color = default)
        {
            byte[] data;


            Color = color;
            Tipo = tipo;

            switch (Tipo)
            {
                case Tipo.Peon:
                    data = Resource1.peon;
                    break;
                case Tipo.Alfil:
                    data = Resource1.alfil;
                    break;
                case Tipo.Caballo:
                    data = Resource1.caballo;
                    break;
                case Tipo.Torre:
                    data = Resource1.torre;
                    break;
                case Tipo.Reina:
                    data = Resource1.reina;
                    break;
                case Tipo.Rey:
                    data = Resource1.rey;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(tipo));
            }

            ImageSvg = Encoding.UTF8.GetString(data);

        }
        public Tipo Tipo { get => tipo; set { tipo = value; lastSize = default; } }
        public Color Color { get => color; set { color = value; lastSize = default; }}
        public string ImageSvg { get; private set; }

        public Bitmap Render(Size size = default)
        {
            string svgPieza;

            if (Equals(size, default) || size.Height == 0 || size.Width == 0)
            {
                size = new Size(DefaultWidth, DefaultHeight);
            }
            if (!lastSize.HasValue || !Equals(size, lastSize.Value))
            {
                lastSize = size;
                svgPieza = ImageSvg.Replace("fill=\"#000000\"", $"fill=\"#{Color.R.ToString("X").PadLeft(2, '0')}{Color.G.ToString("X").PadLeft(2, '0')}{Color.B.ToString("X").PadLeft(2, '0')}\"");

                lastRender= Svg.SvgDocument.FromSvg<Svg.SvgDocument>(svgPieza).Draw(size.Width, size.Height);
            }
            return lastRender;

           
        }
        public override string ToString()
        {
            return $"{Tipo}#{Color}";
        }
    }
}
