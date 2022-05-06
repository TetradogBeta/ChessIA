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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            int x = 1, y = 2,xInit=1,yInit=6;
            List<Move> moves = new List<Move>();
   
            InitializeComponent();
            tablero.TableroData.Move(new System.Drawing.Point(xInit,yInit),new System.Drawing.Point(x,y));
            tablero.TableroData.Move(new System.Drawing.Point(0, 1), new System.Drawing.Point(0, 3));
            //tablero.TableroData.CellsSelected2.AddRange(tablero.TableroData.GetLegalMoves(tablero.TableroData.Piezas[x,y]).Select(m => m.To));
            //tablero.TableroData.CellsSelected2.AddRange(tablero.TableroData.GetLegalMoves(true).Select(m => m.To));

            moves.AddRange(tablero.TableroData.GetLegalMoves(false).Distinct());
            tablero.TableroData.CellsSelected1.AddRange(moves.Select(m=>m.To));

            foreach(var move in moves)
                if(move.To.X==0 && move.To.Y==5)
                    Console.WriteLine(move.ToString());

            tablero.DoubleSelection = true;
            tablero.RenderColor1 = true;
            tablero.Refresh();

        }
    }
}
