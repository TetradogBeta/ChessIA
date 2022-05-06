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
        public Color ColorPieza1 { get; set; } = Color.Sienna;
        public Color ColorPieza2 { get; set; } = Color.DarkViolet;

        public Color ColorCelda1 { get; set; } = Color.LightGray;
        public Color ColorCelda2 { get; set; } = Color.LightCoral;

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
                Piezas[x, FILAPEONES1] = new Pieza(Tipo.Peon, ColorPieza1);
                Piezas[x, FILAPEONES2] = new Pieza(Tipo.Peon, ColorPieza2);
            }

            Piezas[TORRE, FILAFIGURAS1] = new Pieza(Tipo.Torre, ColorPieza1);
            Piezas[LADO - 1, FILAFIGURAS1] = new Pieza(Tipo.Torre, ColorPieza1);
            Piezas[TORRE, FILAFIGURAS2] = new Pieza(Tipo.Torre, ColorPieza2);
            Piezas[LADO - 1, FILAFIGURAS2] = new Pieza(Tipo.Torre, ColorPieza2);


            Piezas[CABALLO, FILAFIGURAS1] = new Pieza(Tipo.Caballo, ColorPieza1);
            Piezas[LADO - 1 - CABALLO, FILAFIGURAS1] = new Pieza(Tipo.Caballo, ColorPieza1);
            Piezas[CABALLO, FILAFIGURAS2] = new Pieza(Tipo.Caballo, ColorPieza2);
            Piezas[LADO - 1 - CABALLO, FILAFIGURAS2] = new Pieza(Tipo.Caballo, ColorPieza2);


            Piezas[ALFIL, FILAFIGURAS1] = new Pieza(Tipo.Alfil, ColorPieza1);
            Piezas[LADO - 1 - ALFIL, FILAFIGURAS1] = new Pieza(Tipo.Alfil, ColorPieza1);
            Piezas[ALFIL, FILAFIGURAS2] = new Pieza(Tipo.Alfil, ColorPieza2);
            Piezas[LADO - 1 - ALFIL, FILAFIGURAS2] = new Pieza(Tipo.Alfil, ColorPieza2);


            Piezas[REINA, FILAFIGURAS1] = new Pieza(Tipo.Reina, ColorPieza1);
            Piezas[REINA, FILAFIGURAS2] = new Pieza(Tipo.Reina, ColorPieza2);

            Piezas[REY, FILAFIGURAS1] = new Pieza(Tipo.Rey, ColorPieza1);
            Piezas[REY, FILAFIGURAS2] = new Pieza(Tipo.Rey, ColorPieza2);

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
            Bitmap cell1 = GetCell(ColorCelda1);
            Bitmap cell2 = GetCell(ColorCelda2);
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
                                cell = cell1;
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
                                cell = cell2;
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
                                cell = cell2;
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
                                cell = cell1;
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
            Pieza? pieza;

            for (int y = 0; y < LADO; y++)
                for (int x = 0; x < LADO; x++)
                {
                    pieza = Piezas[x, y];
                    if (!Equals(pieza, default))
                    {
                        isColor1 = Equals(pieza.Color, ColorPieza1);

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
            const int CABALLO1 = 1, CABALLO3 = 2;

            bool reyesCaraACara;
            Color colorContrincante;
            Point? locationOtherKing = default;
            Point? location = GetLocation(pieza);

            if (!Equals(location, default))
            {
                colorContrincante = pieza.Color.Equals(ColorPieza1) ? ColorPieza2 : ColorPieza1;
                switch (pieza?.Tipo)
                {
                    case Tipo.Peon:
                        if (location.Value.Y > 0 && location.Value.Y < LADO)
                        {
                            if (pieza.Color.Equals(ColorPieza1))
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
                                if (location.Value.X > 0 && !Equals(Piezas[location.Value.X - 1, location.Value.Y + 1], default) && !Equals(Piezas[location.Value.X - 1, location.Value.Y + 1]?.Color, ColorPieza1))
                                {
                                    yield return new Move(location.Value, new Point(location.Value.X - 1, location.Value.Y + 1));
                                }
                                //peon enemigo arriba derecha
                                if (location.Value.X < LAST && !Equals(Piezas[location.Value.X + 1, location.Value.Y + 1], default) && !Equals(Piezas[location.Value.X + 1, location.Value.Y + 1]?.Color, ColorPieza1))
                                {
                                    yield return new Move(location.Value, new Point(location.Value.X + 1, location.Value.Y + 1));
                                }
                                //matar el paso
                                if (!Equals(LastMove, default) && Math.Abs(location.Value.X - LastMove.To.X) == 1 && Piezas[LastMove.To.X, LastMove.To.Y].Color.Equals(ColorPieza2) && Piezas[LastMove.To.X, LastMove.To.Y].Tipo.Equals(Tipo.Peon) && Math.Abs(LastMove.From.Y - LastMove.To.Y) == 2)
                                {
                                    yield return new Move(location.Value, new Point(LastMove.To.X, location.Value.Y -1));
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
                                if (location.Value.X > 0 && !Equals(Piezas[location.Value.X - 1, location.Value.Y - 1], default) && !Equals(Piezas[location.Value.X - 1, location.Value.Y - 1]?.Color, ColorPieza2))
                                {
                                    yield return new Move(location.Value, new Point(location.Value.X - 1, location.Value.Y - 1));
                                }
                                //peon enemigo abajo derecha
                                if (location.Value.X < LAST && !Equals(Piezas[location.Value.X + 1, location.Value.Y - 1], default) && !Equals(Piezas[location.Value.X + 1, location.Value.Y - 1]?.Color, ColorPieza2))
                                {
                                    yield return new Move(location.Value, new Point(location.Value.X + 1, location.Value.Y - 1));
                                }
                                //matar el paso
                                if (!Equals(LastMove, default) && Math.Abs(location.Value.X - LastMove.To.X) == 1 && Piezas[LastMove.To.X, LastMove.To.Y].Color.Equals(ColorPieza1) && Piezas[LastMove.To.X, LastMove.To.Y].Tipo.Equals(Tipo.Peon) && Math.Abs(LastMove.From.Y - LastMove.To.Y) == 2)
                                {
                                    yield return new Move(location.Value, new Point(LastMove.To.X, location.Value.Y - 1));
                                }
                            }
                        }
                        break;
                    case Tipo.Alfil:
                        foreach (Move move in GetDiagonalMoves(location.Value, pieza.Color.Equals(ColorPieza1)))
                            yield return move;
                        break;
                    case Tipo.Caballo:
                        if (location.Value.X - CABALLO3 >= 0 && location.Value.Y + CABALLO1 < LADO && (Equals(Piezas[location.Value.X - CABALLO3, location.Value.Y + CABALLO1], default) || Piezas[location.Value.X - CABALLO3, location.Value.Y + CABALLO1].Color.Equals(colorContrincante)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X - CABALLO3, location.Value.Y + CABALLO1));
                        }
                        if (location.Value.X + CABALLO3 < LADO && location.Value.Y + CABALLO1 < LADO && (Equals(Piezas[location.Value.X + CABALLO3, location.Value.Y + CABALLO1], default) || Piezas[location.Value.X + CABALLO3, location.Value.Y + CABALLO1].Color.Equals(colorContrincante)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X + CABALLO3, location.Value.Y + CABALLO1));
                        }
                        if (location.Value.X - CABALLO3 >= 0 && location.Value.Y - CABALLO1 >= 0 && (Equals(Piezas[location.Value.X - CABALLO3, location.Value.Y - CABALLO1], default) || Piezas[location.Value.X - CABALLO3, location.Value.Y - CABALLO1].Color.Equals(colorContrincante)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X - CABALLO3, location.Value.Y - CABALLO1));
                        }
                        if (location.Value.X + CABALLO3 < LADO && location.Value.Y - CABALLO1 >= 0 && (Equals(Piezas[location.Value.X + CABALLO3, location.Value.Y - CABALLO1], default) || Piezas[location.Value.X + CABALLO3, location.Value.Y - CABALLO1].Color.Equals(colorContrincante)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X + CABALLO3, location.Value.Y - CABALLO1));
                        }


                        if (location.Value.X - CABALLO1 >= 0 && location.Value.Y + CABALLO3 < LADO && (Equals(Piezas[location.Value.X - CABALLO1, location.Value.Y + CABALLO3], default) || Piezas[location.Value.X - CABALLO1, location.Value.Y + CABALLO3].Color.Equals(colorContrincante)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X - CABALLO1, location.Value.Y + CABALLO3));
                        }
                        if (location.Value.X + CABALLO1 < LADO && location.Value.Y + CABALLO3 < LADO && (Equals(Piezas[location.Value.X + CABALLO1, location.Value.Y + CABALLO3], default) || Piezas[location.Value.X + CABALLO1, location.Value.Y + CABALLO3].Color.Equals(colorContrincante)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X + CABALLO1, location.Value.Y + CABALLO3));
                        }
                        if (location.Value.X - CABALLO1 >= 0 && location.Value.Y - CABALLO3 >= 0 && (Equals(Piezas[location.Value.X - CABALLO1, location.Value.Y - CABALLO3], default) || Piezas[location.Value.X - CABALLO1, location.Value.Y - CABALLO3].Color.Equals(colorContrincante)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X - CABALLO1, location.Value.Y - CABALLO3));
                        }
                        if (location.Value.X + CABALLO1 < LADO && location.Value.Y - CABALLO3 >= 0 && (Equals(Piezas[location.Value.X + CABALLO1, location.Value.Y - CABALLO3], default) || Piezas[location.Value.X + CABALLO1, location.Value.Y - CABALLO3].Color.Equals(colorContrincante)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X + CABALLO1, location.Value.Y - CABALLO3));
                        }

                        break;
                    case Tipo.Torre:
                        foreach (Move move in GetCrossMoves(location.Value, pieza.Color.Equals(ColorPieza1)))
                            yield return move;
                        break;
                    case Tipo.Reina:
                        foreach (Move move in GetDiagonalMoves(location.Value, pieza.Color.Equals(ColorPieza1)))
                            yield return move;
                        foreach (Move move in GetCrossMoves(location.Value, pieza.Color.Equals(ColorPieza1)))
                            yield return move;
                        break;
                    case Tipo.Rey:
                        reyesCaraACara = EstanLosReyesCaraACara();

                        if (reyesCaraACara)
                        {
                            locationOtherKing = GetKing(colorContrincante);
                        }
                        if (location.Value.Y < LAST && (Equals(Piezas[location.Value.X, location.Value.Y + 1], default) || Piezas[location.Value.X, location.Value.Y + 1].Color.Equals(colorContrincante) && !IsProtected(new Point(location.Value.X, location.Value.Y - 1), colorContrincante) && (!reyesCaraACara || location.Value.X != locationOtherKing.Value.X || locationOtherKing.Value.Y < location.Value.Y)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X, location.Value.Y + 1));
                        }
                        if (location.Value.Y > 0 && (Equals(Piezas[location.Value.X, location.Value.Y - 1], default) || Piezas[location.Value.X, location.Value.Y - 1].Color.Equals(colorContrincante) && !IsProtected(new Point(location.Value.X, location.Value.Y - 1), colorContrincante) && (!reyesCaraACara || location.Value.X != locationOtherKing.Value.X || locationOtherKing.Value.Y > location.Value.Y)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X, location.Value.Y - 1));
                        }

                        if (location.Value.X < LAST && (Equals(Piezas[location.Value.X + 1, location.Value.Y], default) || Piezas[location.Value.X + 1, location.Value.Y].Color.Equals(colorContrincante) && !IsProtected(new Point(location.Value.X + 1, location.Value.Y), colorContrincante) && (!reyesCaraACara || location.Value.Y != locationOtherKing.Value.Y || locationOtherKing.Value.X < location.Value.X)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X + 1, location.Value.Y));
                        }
                        if (location.Value.X > 0 && (Equals(Piezas[location.Value.X - 1, location.Value.Y], default) || Piezas[location.Value.X - 1, location.Value.Y].Color.Equals(colorContrincante) && !IsProtected(new Point(location.Value.X - 1, location.Value.Y), colorContrincante) && (!reyesCaraACara || location.Value.Y != locationOtherKing.Value.Y || locationOtherKing.Value.X > location.Value.X)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X - 1, location.Value.Y));

                        }

                        if (location.Value.X < LAST && location.Value.Y < LAST && (Equals(Piezas[location.Value.X + 1, location.Value.Y + 1], default) || Piezas[location.Value.X + 1, location.Value.Y + 1].Color.Equals(colorContrincante) && !IsProtected(new Point(location.Value.X + 1, location.Value.Y + 1), colorContrincante) && (!reyesCaraACara || location.Value.X < locationOtherKing.Value.X || locationOtherKing.Value.Y < location.Value.Y)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X + 1, location.Value.Y + 1));
                        }
                        if (location.Value.X < LAST && location.Value.Y > 0 && (Equals(Piezas[location.Value.X + 1, location.Value.Y - 1], default) || Piezas[location.Value.X + 1, location.Value.Y - 1].Color.Equals(colorContrincante) && !IsProtected(new Point(location.Value.X + 1, location.Value.Y - 1), colorContrincante) && (!reyesCaraACara || location.Value.X < locationOtherKing.Value.X || locationOtherKing.Value.Y > location.Value.Y)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X + 1, location.Value.Y - 1));

                        }
                        if (location.Value.X > 0 && location.Value.Y < LAST && (Equals(Piezas[location.Value.X - 1, location.Value.Y + 1], default) || Piezas[location.Value.X - 1, location.Value.Y + 1].Color.Equals(colorContrincante) && !IsProtected(new Point(location.Value.X - 1, location.Value.Y + 1), colorContrincante) && (!reyesCaraACara || location.Value.X > locationOtherKing.Value.X || locationOtherKing.Value.Y < location.Value.Y)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X - 1, location.Value.Y + 1));
                        }
                        if (location.Value.X > 0 && location.Value.Y > 0 && (Equals(Piezas[location.Value.X - 1, location.Value.Y - 1], default) || Piezas[location.Value.X - 1, location.Value.Y - 1].Color.Equals(colorContrincante) && !IsProtected(new Point(location.Value.X - 1, location.Value.Y - 1), colorContrincante) && (!reyesCaraACara || location.Value.X > locationOtherKing.Value.X || locationOtherKing.Value.Y > location.Value.Y)))
                        {
                            yield return new Move(location.Value, new Point(location.Value.X - 1, location.Value.Y - 1));
                        }
                        break;
                }
            }

        }
        private IEnumerable<Move> GetDiagonalMoves(Point posicion, bool isColor1)
        {
            bool canMove;
            bool isEndYieldLeftUp = false;
            bool isEndYieldRightUp = false;
            bool isEndYieldLeftDown = false;
            bool isEndYieldRightDown = false;

            for (int y = posicion.Y + 1, increment = 1; y < LADO && (!isEndYieldLeftUp || !isEndYieldRightUp); y++, increment++)
            {
                if (!isEndYieldLeftUp)
                {
                    isEndYieldLeftUp = posicion.X - increment < 0;
                    if (!isEndYieldLeftUp)
                    {
                        isEndYieldLeftUp = !Equals(Piezas[posicion.X - increment, y], default);
                        canMove = !isEndYieldLeftUp || Piezas[posicion.X - increment, y].Color.Equals(ColorPieza1) && !isColor1 || Piezas[posicion.X - increment, y].Color.Equals(ColorPieza2) && isColor1;

                        if (canMove)
                        {
                            yield return new Move(posicion, new Point(posicion.X - increment, y));
                        }
                    }

                }
                if (!isEndYieldRightUp)
                {
                    isEndYieldRightUp = posicion.X + increment > LAST;
                    if (!isEndYieldRightUp)
                    {
                        isEndYieldRightUp = !Equals(Piezas[posicion.X + increment, y], default);
                        canMove = !isEndYieldRightUp || Piezas[posicion.X + increment, y].Color.Equals(ColorPieza1) && !isColor1 || Piezas[posicion.X + increment, y].Color.Equals(ColorPieza2) && isColor1;

                        if (canMove)
                        {
                            yield return new Move(posicion, new Point(posicion.X + increment, y));
                        }
                    }

                }
            }


            for (int y = posicion.Y - 1, increment = 1; y >= 0 && (!isEndYieldLeftDown || !isEndYieldRightDown); y--, increment++)
            {
                if (!isEndYieldLeftDown)
                {
                    isEndYieldLeftDown = posicion.X - increment < 0;
                    if (!isEndYieldLeftDown)
                    {
                        isEndYieldLeftDown = !Equals(Piezas[posicion.X - increment, y], default);
                        canMove = !isEndYieldLeftDown || Piezas[posicion.X - increment, y].Color.Equals(ColorPieza1) && !isColor1 || Piezas[posicion.X - increment, y].Color.Equals(ColorPieza2) && isColor1;

                        if (canMove)
                        {
                            yield return new Move(posicion, new Point(posicion.X - increment, y));
                        }
                    }

                }
                if (!isEndYieldRightDown)
                {
                    isEndYieldRightDown = posicion.X + increment > LAST;
                    if (!isEndYieldRightDown)
                    {
                        isEndYieldRightDown = !Equals(Piezas[posicion.X + increment, y], default);
                        canMove = !isEndYieldRightDown || Piezas[posicion.X + increment, y].Color.Equals(ColorPieza1) && !isColor1 || Piezas[posicion.X + increment, y].Color.Equals(ColorPieza2) && isColor1;

                        if (canMove)
                        {
                            yield return new Move(posicion, new Point(posicion.X + increment, y));
                        }
                    }

                }
            }
        }
        private IEnumerable<Move> GetCrossMoves(Point posicion, bool isColor1)
        {
            bool canMove;
            bool isEndYieldLeft = false;
            bool isEndYieldRight = false;
            bool isEndYieldDown = false;
            bool isEndYieldUp = false;

            for (int x = posicion.X + 1; x < LADO && !isEndYieldRight; x++)
            {
                isEndYieldRight = !Equals(Piezas[x, posicion.Y], default);
                canMove = !isEndYieldRight || Piezas[x, posicion.Y].Color.Equals(ColorPieza1) && !isColor1 || Piezas[x, posicion.Y].Color.Equals(ColorPieza2) && isColor1;
                if (canMove)
                {
                    yield return new Move(posicion, new Point(x, posicion.Y));
                }
            }
            for (int x = posicion.X - 1; x >= 0 && !isEndYieldLeft; x--)
            {
                isEndYieldLeft = !Equals(Piezas[x, posicion.Y], default);
                canMove = !isEndYieldLeft || Piezas[x, posicion.Y].Color.Equals(ColorPieza1) && !isColor1 || Piezas[x, posicion.Y].Color.Equals(ColorPieza2) && isColor1;
                if (canMove)
                {
                    yield return new Move(posicion, new Point(x, posicion.Y));
                }
            }
            for (int y = posicion.Y + 1; y < LADO && !isEndYieldUp; y++)
            {
                isEndYieldUp = !Equals(Piezas[posicion.X, y], default);
                canMove = !isEndYieldUp || Piezas[posicion.X, y].Color.Equals(ColorPieza1) && !isColor1 || Piezas[posicion.X, y].Color.Equals(ColorPieza2) && isColor1;
                if (canMove)
                {
                    yield return new Move(posicion, new Point(posicion.X, y));
                }
            }
            for (int y = posicion.Y - 1; y >= 0 && !isEndYieldDown; y--)
            {
                isEndYieldDown = !Equals(Piezas[posicion.X, y], default);
                canMove = !isEndYieldDown || Piezas[posicion.X, y].Color.Equals(ColorPieza1) && !isColor1 || Piezas[posicion.X, y].Color.Equals(ColorPieza2) && isColor1;
                if (canMove)
                {
                    yield return new Move(posicion, new Point(posicion.X, y));
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

    public class Move:IComparable,IComparable<Move>
    {
        public Move(Point locationInit, Point locationEnd)
        {
            From = locationInit;
            To = locationEnd;
        }

        public Point From { get; set; }
        public Point To { get; set; }

        public override bool Equals(object obj)
        {
          return CompareTo(obj as Move)==0;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"{From}:{To}";
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo(obj as Move);
        }

        int IComparable<Move>.CompareTo(Move other)
        {
            return CompareTo(other);
        }
        int CompareTo(Move? other)
        {
            int compareTo = Equals(other, default) ? -1 : 0;
            if(compareTo == 0)
            {
                compareTo=From.X.CompareTo(other.From.X);
                if (compareTo == 0)
                {
                    compareTo = From.Y.CompareTo(other.From.Y);
                    if (compareTo == 0)
                    {
                        compareTo = To.X.CompareTo(other.To.X);
                        if (compareTo == 0)
                        {
                            compareTo = To.Y.CompareTo(other.To.Y);
                        }
                    }
                }
            }
            return compareTo;
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
    public class InvalidMoveException : Exception
    {

    }
}
