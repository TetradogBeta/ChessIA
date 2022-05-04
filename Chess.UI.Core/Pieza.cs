﻿using System;
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
        public static int DefaultHeight = 30;
        public static int DefaultWidth = 30;
        public static Color DefaultBrush = Color.Blue;

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
                default:throw new ArgumentOutOfRangeException(nameof(tipo));
            }
        
            ImageSvg = System.Text.ASCIIEncoding.UTF8.GetString(data); 

        }
        public Tipo Tipo { get; set; }
        public Color Color { get; set; }
        public string ImageSvg { get; private set; }
       
        public Bitmap Render(Size size = default)
        {
            string svgPieza;
            if (Equals(size, default))
            {
                size = new Size(DefaultWidth, DefaultHeight);
            }

            svgPieza = ImageSvg.Replace("fill=\"#000000\"", $"fill=\"#{Color.R.ToString("X").PadLeft(2, '0')}{Color.G.ToString("X").PadLeft(2,'0')}{Color.B.ToString("X").PadLeft(2, '0')}\"");

            return Svg.SvgDocument.FromSvg<Svg.SvgDocument>(svgPieza).Draw(size.Width,size.Height);
        }
        public override string ToString()
        {
            return $"{Tipo}#{Color}";
        }
    }
}