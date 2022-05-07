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



            //tablero.TableroData.Move(new System.Drawing.Point(1, 7), new System.Drawing.Point(5, 5));
            //tablero.TableroData.Move(new System.Drawing.Point(2, 7), new System.Drawing.Point(4, 5));
            //tablero.TableroData.Move(new System.Drawing.Point(3, 7), new System.Drawing.Point(3, 5));

            //tablero.TableroData.CellsSelected2.AddRange(tablero.TableroData.GetLegalMoves(tablero.TableroData.Piezas[x,y]).Select(m => m.To));


            //tablero.TableroData.CellsSelected2.AddRange(tablero.TableroData.GetLegalMoves(tablero.TableroData.Piezas[x,y]).Select(m => m.To));

            tablero.DoubleSelection = true;
            tablero.RenderColor1 = true;
            tablero.TableroData.Start();


            //tablero.TableroData.DicCellsSelecteds[TableroData.ColorDefaultSelected2].AddRange(tablero.TableroData.GetLegalMoves(false).Select(m => m.To));
            //tablero.TableroData.DicCellsSelecteds[TableroData.ColorDefaultSelected1].AddRange(tablero.TableroData.GetLegalMoves(true).Select(m => m.To));


            tablero.Refresh();

        }
    }
}
