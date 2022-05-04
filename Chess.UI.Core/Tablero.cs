using Gabriel.Cat.S.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gabriel.Cat.S.Extension;
using Gabriel.Cat.S.Utilitats;

namespace Chess.UI.Core
{
    public class TableroData
    {
        public const int LADO = 8;
        public const int LAST = LADO - 1;
        public static int DefaultLadoPieza = 100;
        public static int DefaultLadoCelda = 200;

        public event EventHandler<PiezaEventArgs>? PiezaCapturada;
        public Pieza?[,] Piezas { get; private set; } = new Pieza?[LADO, LADO];
        public Color Color1Pieza { get; set; } = Color.Sienna;
        public Color Color2Pieza { get; set; } = Color.DarkViolet;

        public Color Color1Celda { get; set; } = Color.LightGray;
        public Color Color2Celda { get; set; } = Color.LightCoral;

        public Color ColorCeldaSeleccionada1 { get; set; } = Color.Green;
        public Color ColorCeldaSeleccionada2 { get; set; } = Color.Blue;
        public Size SizePieza { get; set; } = new Size(DefaultLadoPieza, DefaultLadoPieza);
        public Size SizeCelda { get; set; } = new Size(DefaultLadoCelda, DefaultLadoCelda);

        public Point LocationPizaEnCelda { get; set; } = new Point((DefaultLadoCelda - DefaultLadoPieza) / 2, (DefaultLadoCelda - DefaultLadoPieza) / 2);

        public List<Point> CellsSelected1 { get; private set; } = new List<Point>();

        public List<Point> CellsSelected2 { get; private set; } = new List<Point>();

        public bool ReyColor1Movido { get; set; }
        public bool ReyColor2Movido { get; set; }
        public bool Torre1Color1Movida { get; set; }
        public bool Torre2Color1Movida { get; set; }
        public bool Torre1Color2Movida { get; set; }
        public bool Torre2Color2Movida { get; set; }
        public Move? LastMove { get; private set; }
        public void Start() => Reset();
        public void Reset()
        {
            const int FILAPEONES1 = 1;
            const int FILAPEONES2 = LADO - 2;
            const int FILAFIGURAS1 = 0;
            const int FILAFIGURAS2 = LADO - 1;

            const int TORRE = 0;
            const int ALFIL = 2;
            const int CABALLO = 1;
            const int REINA = 3;
            const int REY = 4;

            ReyColor1Movido = false;
            ReyColor2Movido = false;
            Torre1Color1Movida = false;
            Torre2Color1Movida = false;
            Torre1Color2Movida = false;
            Torre2Color2Movida = false;
            LastMove = default;
            for (int x = 0; x < LADO; x++)
                for (int y = 0; y < LADO; y++)
                    Piezas[x, y] = default;
            //pongo las piezas
            for (int x = 0; x < LADO; x++)
            {
                Piezas[x, FILAPEONES1] = new Pieza(Tipo.Peon, Color1Pieza);
                Piezas[x, FILAPEONES2] = new Pieza(Tipo.Peon, Color2Pieza);
            }

            Piezas[TORRE, FILAFIGURAS1] = new Pieza(Tipo.Torre, Color1Pieza);
            Piezas[LADO - 1, FILAFIGURAS1] = new Pieza(Tipo.Torre, Color1Pieza);
            Piezas[TORRE, FILAFIGURAS2] = new Pieza(Tipo.Torre, Color2Pieza);
            Piezas[LADO - 1, FILAFIGURAS2] = new Pieza(Tipo.Torre, Color2Pieza);


            Piezas[CABALLO, FILAFIGURAS1] = new Pieza(Tipo.Caballo, Color1Pieza);
            Piezas[LADO - 1 - CABALLO, FILAFIGURAS1] = new Pieza(Tipo.Caballo, Color1Pieza);
            Piezas[CABALLO, FILAFIGURAS2] = new Pieza(Tipo.Caballo, Color2Pieza);
            Piezas[LADO - 1 - CABALLO, FILAFIGURAS2] = new Pieza(Tipo.Caballo, Color2Pieza);


            Piezas[ALFIL, FILAFIGURAS1] = new Pieza(Tipo.Alfil, Color1Pieza);
            Piezas[LADO - 1 - ALFIL, FILAFIGURAS1] = new Pieza(Tipo.Alfil, Color1Pieza);
            Piezas[ALFIL, FILAFIGURAS2] = new Pieza(Tipo.Alfil, Color2Pieza);
            Piezas[LADO - 1 - ALFIL, FILAFIGURAS2] = new Pieza(Tipo.Alfil, Color2Pieza);


            Piezas[REINA, FILAFIGURAS1] = new Pieza(Tipo.Reina, Color1Pieza);
            Piezas[REINA, FILAFIGURAS2] = new Pieza(Tipo.Reina, Color2Pieza);

            Piezas[REY, FILAFIGURAS1] = new Pieza(Tipo.Rey, Color1Pieza);
            Piezas[REY, FILAFIGURAS2] = new Pieza(Tipo.Rey, Color2Pieza);

        }
        public void Move(Point locationInit, Point locationEnd, bool raisePiezaCapturadaEvent = true)
        {
            if (Piezas[locationEnd.X, locationEnd.Y] != null && Equals(Piezas[locationEnd.X, locationEnd.Y]?.Color, Piezas[locationInit.X, locationInit.Y]?.Color))
            {
                throw new InvalidMoveException();
            }
            if (PiezaCapturada != null && raisePiezaCapturadaEvent && !Equals(Piezas[locationEnd.X, locationEnd.Y], default))
                PiezaCapturada(this, new PiezaEventArgs(Piezas[locationEnd.X, locationEnd.Y], locationEnd));

            Piezas[locationEnd.X, locationEnd.Y] = Piezas[locationInit.X, locationInit.Y];
            Piezas[locationInit.X, locationInit.Y] = default;
            LastMove = new Move(locationInit, locationEnd);

            if (!ReyColor1Movido && Piezas[locationEnd.X, locationEnd.Y].Tipo.Equals(Tipo.Rey))
            {
                ReyColor1Movido = true;
            }
            if (!ReyColor2Movido && Piezas[locationEnd.X, locationEnd.Y].Tipo.Equals(Tipo.Rey))
            {
                ReyColor2Movido = true;
            }
            if (!Torre1Color1Movida && Piezas[locationEnd.X, locationEnd.Y].Tipo.Equals(Tipo.Torre))
            {
                Torre1Color1Movida = true;
            }
            if (!Torre2Color1Movida && Piezas[locationEnd.X, locationEnd.Y].Tipo.Equals(Tipo.Torre))
            {
                Torre2Color1Movida = true;
            }
            if (!Torre1Color2Movida && Piezas[locationEnd.X, locationEnd.Y].Tipo.Equals(Tipo.Torre))
            {
                Torre1Color2Movida = true;
            }
            if (!Torre2Color2Movida && Piezas[locationEnd.X, locationEnd.Y].Tipo.Equals(Tipo.Torre))
            {
                Torre2Color2Movida = true;
            }
        }
        private int Invertir(int pos)
        {
            return LAST - pos;
        }
        private Bitmap GetCell(Color color, byte piexelA = byte.MaxValue)
        {
            Bitmap cell = new Bitmap(SizeCelda.Width, SizeCelda.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            cell.TrataBytes((data) =>
            {
                for (int i = 0; i < data.Length; i += Pixel.ARGB)
                {
                    data[i + Pixel.A] = piexelA;
                    data[i + Pixel.R] = color.R;
                    data[i + Pixel.G] = color.G;
                    data[i + Pixel.B] = color.B;
                }
            });
            return cell;
        }
        public Bitmap Render(bool fromLado1 = true)
        {

            const int CAPACELDA = 2, CAPASELECTED = 1, CAPAPIEZA = 0;
            Bitmap result;
            Pieza? pieza;
            Bitmap cell;
            Bitmap cellSelected1, cellSelected2;
            Bitmap cell1 = GetCell(Color1Celda);
            Bitmap cell2 = GetCell(Color2Celda);
            Collage collage = new Collage();


            for (int y = 0, yAux = LADO - 1; y < LADO; y++, yAux--)
                for (int x = 0; x < LADO; x++)

                {
                    if (x % 2 == 0)
                    {
                        if (y % 2 == 0)
                        {
                            if (fromLado1)
                            {
                                cell = cell2;
                            }
                            else
                            {
                                cell = cell1;
                            }

                        }
                        else
                        {
                            if (fromLado1)
                            {
                                cell = cell1;
                            }
                            else
                            {
                                cell = cell2;

                            }
                        }
                    }
                    else
                    {
                        if (y % 2 == 0)
                        {
                            if (fromLado1)
                            {
                                cell = cell1;
                            }
                            else
                            {
                                cell = cell2;

                            }
                        }
                        else
                        {
                            if (fromLado1)
                            {
                                cell = cell2;
                            }
                            else
                            {
                                cell = cell1;
                            }
                        }
                    }

                    collage.Add(cell, x * SizeCelda.Width, y * SizeCelda.Height, CAPACELDA);
                    if (fromLado1)
                    {
                        pieza = Piezas[x, y];
                    }
                    else
                    {
                        pieza = Piezas[Invertir(x), Invertir(y)];
                    }
                    if (!Equals(pieza, default))
                    {
                        collage.Add(pieza.Render(SizePieza), x * SizeCelda.Width + LocationPizaEnCelda.X, yAux * SizeCelda.Height + LocationPizaEnCelda.Y, CAPAPIEZA);
                    }
                }
            if (CellsSelected1.Count > 0)
            {
                cellSelected1 = GetCell(ColorCeldaSeleccionada1, (byte)byte.MaxValue / 3);
                foreach (Point posCellSelected in CellsSelected1)
                {
                    if (fromLado1)
                    {
                        collage.Add(cellSelected1, posCellSelected.X * SizeCelda.Width, (LADO - 1 - posCellSelected.Y) * SizeCelda.Height, CAPASELECTED);
                    }
                    else
                    {
                        collage.Add(cellSelected1, Invertir(posCellSelected.X) * SizeCelda.Width, (LADO - 1 - Invertir(posCellSelected.Y)) * SizeCelda.Height, CAPASELECTED);
                    }


                }
            }
            if (CellsSelected2.Count > 0)
            {
                cellSelected2 = GetCell(ColorCeldaSeleccionada2, (byte)byte.MaxValue / 3);

                foreach (Point posCellSelected in CellsSelected2)
                {
                    if (fromLado1)
                    {

                        collage.Add(cellSelected2, posCellSelected.X * SizeCelda.Width, (LADO - 1 - posCellSelected.Y) * SizeCelda.Height, CAPASELECTED);
                    }
                    else
                    {
                        collage.Add(cellSelected2, Invertir(posCellSelected.X) * SizeCelda.Width, (LADO - 1 - Invertir(posCellSelected.Y)) * SizeCelda.Height, CAPASELECTED);
                    }

                }
            }

            result = collage.CrearCollage();

            return result;
        }
        public bool EstanLosReyesCaraACara()
        {
            List<Point> reyes = new List<Point>();
            for (int x = 0; x < LADO && reyes.Count < 2; x++)
                for (int y = 0; y < LADO && reyes.Count < 2; y++)
                {
                    if (!Equals(Piezas[x, y], default) && Piezas[x, y].Tipo.Equals(Tipo.Rey))
                        reyes.Add(new Point(x, y));
                }
            return Math.Abs(reyes[0].X - reyes[1].X) == 1 || (reyes[0].X == reyes[1].X && Math.Abs(reyes[0].Y - reyes[1].Y) == 1);
        }
        public Point TraslatePointImageToLocation(double pointImageX, double pointImageY, bool imgFromColor1 = true)
        {
            Point pos = new Point((int)pointImageX / SizeCelda.Width, LADO - 1 - ((int)pointImageY / SizeCelda.Height));
            if (!imgFromColor1)
            {
                pos = new Point(Invertir(pos.X), Invertir(pos.Y));
            }
            return pos;
        }

        public IEnumerable<Move> GetLegalMoves(bool fromColor1 = true)
        {
            bool isColor1;
            foreach (Pieza? pieza in Piezas)
            {
                if (!Equals(pieza, default))
                {
                    isColor1 = Equals(pieza.Color, Color1Pieza);

                    if (isColor1)
                    {
                        if (fromColor1)
                        {
                            foreach (Move move in GetLegalMoves(pieza))
                                yield return move;
                        }
                    }
                    else if (!fromColor1)
                    {
                        foreach (Move move in GetLegalMoves(pieza))
                            yield return move;
                    }
                }
            }
        }
        public IEnumerable<Move> GetLegalMoves(Pieza? pieza)
        {
            const int FILAPEONESINIT1 = 1, FILAPEONESINIT2 = 6;
            bool reyesCaraACara;
            Point? locationOtherKing = default;
            Point? location = GetLocation(pieza);
            Color otro;
            if (!Equals(location, default))
            {
                switch (pieza?.Tipo)
                {
                    case Tipo.Peon:
                        if (location.Value.Y > 0 || location.Value.Y < LADO)
                        {
                            if (pieza.Color.Equals(Color1Pieza))
                            {
                                //paso adelante
                                if (Equals(Piezas[location.Value.X, location.Value.Y + 1], default))
                                {
                                    yield return new Move(location.Value, new Point(location.Value.X, location.Value.Y + 1));
                                }
                                //si aun no se ha movido puede dar hasta dos pasos
                                if (location.Value.Y == FILAPEONESINIT1 && Equals(Piezas[location.Value.X, location.Value.Y + 1], default) && Equals(Piezas[location.Value.X, location.Value.Y + 2], default))
                                {
                                    yield return new Move(location.Value, new Point(location.Value.X, location.Value.Y + 2));
                                }
                                //peon enemigo arriba izquierda
                                if (location.Value.X > 0 && !Equals(Piezas[location.Value.X - 1, location.Value.Y + 1], default) && !Equals(Piezas[location.Value.X - 1, location.Value.Y + 1]?.Color, Color1Pieza))
                                {
                                    yield return new Move(location.Value, new Point(location.Value.X - 1, location.Value.Y + 1));
                                }
                                //peon enemigo arriba derecha
                                if (location.Value.X < LAST && !Equals(Piezas[location.Value.X + 1, location.Value.Y + 1], default) && !Equals(Piezas[location.Value.X + 1, location.Value.Y + 1]?.Color, Color1Pieza))
                                {
                                    yield return new Move(location.Value, new Point(location.Value.X + 1, location.Value.Y + 1));
                                }
                                //matar el paso
                                if (!Equals(LastMove, default) && Piezas[LastMove.To.X, LastMove.To.Y].Color.Equals(Color2Pieza) && Piezas[LastMove.To.X, LastMove.To.Y].Tipo.Equals(Tipo.Peon) && Math.Abs(LastMove.From.Y - LastMove.To.Y) == 2)
                                {
                                    yield return new Move(location.Value, new Point(LastMove.To.X, location.Value.Y + 1));
                                }
                            }
                            else
                            {
                                //paso adelante
                                if (Equals(Piezas[location.Value.X, location.Value.Y - 1], default))
                                {
                                    yield return new Move(location.Value, new Point(location.Value.X, location.Value.Y - 1));
                                }
                                //si aun no se ha movido puede dar hasta dos pasos
                                if (location.Value.Y == FILAPEONESINIT2 && Equals(Piezas[location.Value.X, location.Value.Y - 1], default) && Equals(Piezas[location.Value.X, location.Value.Y - 2], default))
                                {
                                    yield return new Move(location.Value, new Point(location.Value.X, location.Value.Y - 2));
                                }
                                //peon enemigo abajo izquierda
                                if (location.Value.X > 0 && !Equals(Piezas[location.Value.X - 1, location.Value.Y - 1], default) && !Equals(Piezas[location.Value.X - 1, location.Value.Y - 1]?.Color, Color2Pieza))
                                {
                                    yield return new Move(location.Value, new Point(location.Value.X - 1, location.Value.Y - 1));
                                }
                                //peon enemigo abajo derecha
                                if (location.Value.X < LAST && !Equals(Piezas[location.Value.X + 1, location.Value.Y - 1], default) && !Equals(Piezas[location.Value.X + 1, location.Value.Y - 1]?.Color, Color2Pieza))
                                {
                                    yield return new Move(location.Value, new Point(location.Value.X + 1, location.Value.Y - 1));
                                }
                                //matar el paso
                                if (!Equals(LastMove, default) && Piezas[LastMove.To.X, LastMove.To.Y].Color.Equals(Color1Pieza) && Piezas[LastMove.To.X, LastMove.To.Y].Tipo.Equals(Tipo.Peon) && Math.Abs(LastMove.From.Y - LastMove.To.Y) == 2)
                                {
                                    yield return new Move(location.Value, new Point(LastMove.To.X, location.Value.Y - 1));
                                }
                            }
                        }
                        break;
                    case Tipo.Alfil:
                        break;
                    case Tipo.Caballo:
                        break;
                    case Tipo.Torre:
                        break;
                    case Tipo.Reina:
                        break;
                    case Tipo.Rey:
                        reyesCaraACara = EstanLosReyesCaraACara();
                        otro = pieza.Color.Equals(Color1Pieza) ? Color2Pieza : Color1Pieza;
                        if (reyesCaraACara)
                        {
                            locationOtherKing = GetKing(otro);
                        }
                        if (location.Value.Y < LAST && (Equals(Piezas[location.Value.X, location.Value.Y + 1], default) || Piezas[location.Value.X, location.Value.Y + 1].Color.Equals(otro) && !IsProtected(new Point(location.Value.X, location.Value.Y - 1), otro) && (!reyesCaraACara || location.Value.X != locationOtherKing.Value.X || locationOtherKing.Value.Y < location.Value.Y)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X, location.Value.Y + 1));
                        }
                        if (location.Value.Y > 0 && (Equals(Piezas[location.Value.X, location.Value.Y - 1], default) || Piezas[location.Value.X, location.Value.Y - 1].Color.Equals(otro) && !IsProtected(new Point(location.Value.X, location.Value.Y - 1), otro) && (!reyesCaraACara || location.Value.X != locationOtherKing.Value.X || locationOtherKing.Value.Y > location.Value.Y)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X, location.Value.Y - 1));
                        }

                        if (location.Value.X < LAST && (Equals(Piezas[location.Value.X + 1, location.Value.Y], default) || Piezas[location.Value.X + 1, location.Value.Y].Color.Equals(otro) && !IsProtected(new Point(location.Value.X + 1, location.Value.Y), otro) && (!reyesCaraACara || location.Value.Y != locationOtherKing.Value.Y || locationOtherKing.Value.X < location.Value.X)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X + 1, location.Value.Y));
                        }
                        if (location.Value.X > 0 && (Equals(Piezas[location.Value.X - 1, location.Value.Y], default) || Piezas[location.Value.X - 1, location.Value.Y].Color.Equals(otro) && !IsProtected(new Point(location.Value.X - 1, location.Value.Y), otro) && (!reyesCaraACara || location.Value.Y != locationOtherKing.Value.Y || locationOtherKing.Value.X > location.Value.X)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X - 1, location.Value.Y));

                        }

                        if (location.Value.Y < LAST && (Equals(Piezas[location.Value.X + 1, location.Value.Y + 1], default) || Piezas[location.Value.X + 1, location.Value.Y + 1].Color.Equals(otro) && !IsProtected(new Point(location.Value.X + 1, location.Value.Y + 1), otro) && (!reyesCaraACara || location.Value.X < locationOtherKing.Value.X || locationOtherKing.Value.Y < location.Value.Y)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X + 1, location.Value.Y + 1));
                        }
                        if (location.Value.X < LAST && location.Value.Y > 0 && (Equals(Piezas[location.Value.X + 1, location.Value.Y - 1], default) || Piezas[location.Value.X + 1, location.Value.Y - 1].Color.Equals(otro) && !IsProtected(new Point(location.Value.X + 1, location.Value.Y - 1), otro) && (!reyesCaraACara || location.Value.X < locationOtherKing.Value.X || locationOtherKing.Value.Y > location.Value.Y)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X + 1, location.Value.Y - 1));

                        }
                        if (location.Value.Y < LAST && (Equals(Piezas[location.Value.X - 1, location.Value.Y + 1], default) || Piezas[location.Value.X - 1, location.Value.Y + 1].Color.Equals(otro) && !IsProtected(new Point(location.Value.X - 1, location.Value.Y + 1), otro) && (!reyesCaraACara || location.Value.X > locationOtherKing.Value.X || locationOtherKing.Value.Y < location.Value.Y)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X - 1, location.Value.Y + 1));
                        }
                        if (location.Value.X > 0 && location.Value.Y > 0 && (Equals(Piezas[location.Value.X - 1, location.Value.Y - 1], default) || Piezas[location.Value.X - 1, location.Value.Y - 1].Color.Equals(otro) && !IsProtected(new Point(location.Value.X - 1, location.Value.Y - 1), otro) && (!reyesCaraACara || location.Value.X > locationOtherKing.Value.X || locationOtherKing.Value.Y > location.Value.Y)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X - 1, location.Value.Y - 1));
                        }
                        break;
                }
            }

        }
        public Point? GetKing(Color colorKing)
        {
            Point? location = default;
            bool encontrado = false;
            for (int x = 0; x < LADO && !encontrado; x++)
                for (int y = 0; y < LADO && !encontrado; y++)
                {
                    encontrado = !Equals(Piezas[x, y], default) && Piezas[x, y].Tipo.Equals(Tipo.Rey) && Piezas[x, y].Color.Equals(colorKing);
                    if (encontrado)
                    {
                        location = new Point(x, y);
                    }
                }
            return location;
        }
        public bool IsProtected(Point? posicionProtegida, Color colorOther)
        {
            return false;//to do
        }
        public Point? GetLocation(Pieza? piezaTablero)
        {
            Point? location = default;
            bool encontrado = false;
            for (int x = 0; x < LADO && !encontrado; x++)
                for (int y = 0; y < LADO && !encontrado; y++)
                {
                    encontrado = Piezas[x, y] == piezaTablero;
                    if (encontrado)
                    {
                        location = new Point(x, y);
                    }
                }
            return location;
        }


    }

    public class Move
    {
        public Move(Point locationInit, Point locationEnd)
        {
            From = locationInit;
            To = locationEnd;
        }

        public Point From { get; set; }
        public Point To { get; set; }
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
    public class InvalidMoveException : Exception
    {

    }
}
