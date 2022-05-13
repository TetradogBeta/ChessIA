using Gabriel.Cat.S.Binaris;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.UI.Core
{
    public class Partida:IElementoBinarioComplejo
    {
        public static ElementoBinario Serializador { get; private set; } = ElementoBinario.GetSerializador<Partida>();
        public bool StartsPlayer1 { get; set; } = true;
        public List<Move> Moves { get; set; }=new List<Move>();
        public TableroData Tablero { get; set; } = new TableroData();

        ElementoBinario IElementoBinarioComplejo.Serialitzer => Serializador;
    }
}
