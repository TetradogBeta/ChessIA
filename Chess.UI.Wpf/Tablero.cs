using Gabriel.Cat.S.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gabriel.Cat.S.Extension;

namespace Chess.UI.Wpf
{
    public class Tablero
    {
        public const int LADO = 8;
        public static int DefaultLadoPieza = 100;
        public static int DefaultLadoCelda = 200;

        public event EventHandler<PiezaEventArgs>? PiezaCapturada;
        public Pieza?[,] Piezas { get;private set; } = new Pieza?[LADO,LADO];
        public Color Color1Pieza { get; set; }= Color.Coral;
        public Color Color2Pieza { get; set; } = Color.DarkViolet;

        public Color Color1Celda { get; set; } = Color.LightGray;
        public Color Color2Celda { get; set; } = Color.LightCoral;


        public Size SizePieza { get; set; } = new Size(DefaultLadoPieza, DefaultLadoPieza);
        public Size SizeCelda { get; set; } = new Size(DefaultLadoCelda, DefaultLadoCelda);

        public Point LocationPizaEnCelda { get; set; } = new Point((DefaultLadoCelda - DefaultLadoPieza) / 2, (DefaultLadoCelda - DefaultLadoPieza) / 2);
        public void Start() => Reset();
        public void Reset()
        {
            const int FILAPEONES1 = 1;
            const int FILAPEONES2 = LADO-2;
            const int FILAFIGURAS1 = 0;
            const int FILAFIGURAS2 = LADO-1;

            const int TORRE = 0;
            const int ALFIL = 2;
            const int CABALLO = 1;
            const int REINA = 3;
            const int REY = 4;

            for (int x = 0; x < LADO; x++)
                for (int y = 0; y < LADO; y++)
                    Piezas[x, y] = default;
            //pongo las piezas
            for(int x= 0; x < LADO; x++)
            {
                Piezas[x, FILAPEONES1] = new Pieza(Tipo.Peon, Color1Pieza);
                Piezas[x, FILAPEONES2] = new Pieza(Tipo.Peon, Color2Pieza);
            }

            Piezas[TORRE, FILAFIGURAS1] = new Pieza(Tipo.Torre, Color1Pieza);
            Piezas[LADO -1, FILAFIGURAS1] = new Pieza(Tipo.Torre, Color1Pieza);
            Piezas[TORRE, FILAFIGURAS2] = new Pieza(Tipo.Torre, Color2Pieza);
            Piezas[LADO- 1, FILAFIGURAS2] = new Pieza(Tipo.Torre, Color2Pieza);


            Piezas[CABALLO, FILAFIGURAS1] = new Pieza(Tipo.Caballo, Color1Pieza);
            Piezas[LADO-1 - CABALLO, FILAFIGURAS1] = new Pieza(Tipo.Caballo, Color1Pieza);
            Piezas[CABALLO, FILAFIGURAS2] = new Pieza(Tipo.Caballo, Color2Pieza);
            Piezas[LADO-1 - CABALLO, FILAFIGURAS2] = new Pieza(Tipo.Caballo, Color2Pieza);


            Piezas[ALFIL, FILAFIGURAS1] = new Pieza(Tipo.Alfil, Color1Pieza);
            Piezas[LADO -1 - ALFIL, FILAFIGURAS1] = new Pieza(Tipo.Alfil, Color1Pieza);
            Piezas[ALFIL, FILAFIGURAS2] = new Pieza(Tipo.Alfil, Color2Pieza);
            Piezas[LADO -1  - ALFIL, FILAFIGURAS2] = new Pieza(Tipo.Alfil, Color2Pieza);


            Piezas[REINA, FILAFIGURAS1] = new Pieza(Tipo.Reina, Color1Pieza);
            Piezas[REINA, FILAFIGURAS2] = new Pieza(Tipo.Reina, Color2Pieza);

            Piezas[REY, FILAFIGURAS1] = new Pieza(Tipo.Rey, Color1Pieza);
            Piezas[REY, FILAFIGURAS2] = new Pieza(Tipo.Rey, Color2Pieza);

        }
        public void Move(Point locationInit,Point locationEnd,bool raisePiezaCapturadaEvent = true)
        {
            if (PiezaCapturada != null && raisePiezaCapturadaEvent && !Equals(Piezas[locationEnd.X, locationEnd.Y], default))
                PiezaCapturada(this,new PiezaEventArgs(Piezas[locationEnd.X, locationEnd.Y],locationEnd));

            Piezas[locationEnd.X,locationEnd.Y]=Piezas[locationInit.X,locationInit.Y];
            Piezas[locationInit.X, locationInit.Y] = default;
        }

        public Bitmap Render()
        {
            const int A=0,R=A+1,G=R+1,B=G+1;

            Bitmap cell;
            Pieza pieza;

            Bitmap cell1 = new Bitmap(SizeCelda.Width, SizeCelda.Height,System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Bitmap cell2 = new Bitmap(SizeCelda.Width, SizeCelda.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Collage collage = new Collage();

            cell1.TrataBytes((data) =>
            {
                for(int i = 0; i < data.Length; i += 4)
                {
                    data[i + A] = byte.MaxValue;
                    data[i + R] = (byte)Color1Celda.R;
                    data[i + G] = (byte)Color1Celda.G;
                    data[i + B] = (byte)Color1Celda.B;
                }
            });
            cell2.TrataBytes((data) =>
            {
                for (int i = 0; i < data.Length; i += 4)
                {
                    data[i + A] = byte.MaxValue;
                    data[i + R] = (byte)Color2Celda.R;
                    data[i + G] = (byte)Color2Celda.G;
                    data[i + B] = (byte)Color2Celda.B;
                }
            });
            for (int y = 0,yAux=LADO-1; y < LADO; y++,yAux--)
                for (int x = 0; x < LADO; x++)

                {

                    collage.Add(x % 2 == 0 ? y % 2 == 0 ? cell2 : cell1 : y % 2 == 0 ? cell1 : cell2, x * SizeCelda.Width, y * SizeCelda.Height, 1);
                    pieza = Piezas[x, y];
                    if (!Equals(pieza, default))
                    {
                        collage.Add(pieza.Render(SizePieza), x * SizeCelda.Width + LocationPizaEnCelda.X, yAux * SizeCelda.Height + LocationPizaEnCelda.Y, 0);
                    }
                }
            return collage.CrearCollage();
        }



    }

    public class PiezaEventArgs : EventArgs
    {
        public PiezaEventArgs(Pieza? pieza, Point location)
        {
            Pieza = pieza;
            Location = location;
        }
        public Point Location { get; private set; } 
        public Pieza? Pieza { get; private set; }
    }
}
