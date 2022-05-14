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
       
   
            InitializeComponent();

            elegirPieza.Load(tablero.TableroData.ColorPieza1, tablero.TableroData.ColorPieza2);
           

            //tablero.TableroData.Move(new System.Drawing.Point(1, 7), new System.Drawing.Point(5, 5));
            //tablero.TableroData.Move(new System.Drawing.Point(2, 7), new System.Drawing.Point(4, 5));
            //tablero.TableroData.Move(new System.Drawing.Point(3, 7), new System.Drawing.Point(3, 5));

            //tablero.TableroData.CellsSelected2.AddRange(tablero.TableroData.GetLegalMoves(tablero.TableroData.Piezas[x,y]).Select(m => m.To));


            //tablero.TableroData.CellsSelected2.AddRange(tablero.TableroData.GetLegalMoves(tablero.TableroData.Piezas[x,y]).Select(m => m.To));

            tablero.DoubleSelection = true;
            tablero.RenderColor1 = true;
            //tablero.TableroData.AddOrReplace(4, 0, Tipo.Rey, true);
            //tablero.TableroData.AddOrReplace(4, 1, Tipo.Peon, false);
            //tablero.TableroData.AddOrReplace(4, 3, Tipo.Reina, true);
            //tablero.TableroData.AddOrReplace(4, 4, Tipo.Rey, false);
            //tablero.TableroData.ReyColor1Movido = true;
            //tablero.TableroData.ReyColor2Movido = true;
            //  tablero.TableroData.DicCellsSelecteds[TableroData.ColorDefaultSelected2].AddRange(tablero.TableroData.GetLegalMoves(false).Select(m => m.To));


            //tablero.TableroData.DicCellsSelecteds[TableroData.ColorDefaultSelected1].AddRange(tablero.TableroData.GetLegalMoves(2,2).Select(m => m.To));
            //tablero.TableroData.DicCellsSelecteds[TableroData.ColorDefaultSelected2].AddRange(tablero.TableroData.GetLegalMoves(2, 2,Tipo.Rey).Select(m => m.To));
            //tablero.TableroData.DicCellsSelecteds[TableroData.ColorDefaultSelected1].AddRange(tablero.TableroData.GetLegalMoves(false).Select(m => m.To));


            //tablero.Refresh();

           

        }

        private void tablero_Clicked(object sender, PosicionTableroEventArgs e)
        {
            tablero.TableroData.Piezas[e.Posicion.X, e.Posicion.Y] = elegirPieza.Selected;
            tablero.Refresh();
        }
    }
}
