using System;
using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.Faction.CurrencyMVC
{
    public class CurrencyView : IViewText
    {
        public Currency_Model model;

        public FloatCoords Coords { get => new FloatCoords() { x = 135, y = 3}; set {; } }
        public string SpriteFont { get => "Currency"; set {; } }
        public string Text { get => "PokeYen: " + model.currency.ToString(); set {; } }
        public Color Colour { get => Color.White; set {; } }
        public int ZIndex { get => 1; set {; } }

        public CurrencyView(Currency_Model model) => this.model = model;

    }
}
