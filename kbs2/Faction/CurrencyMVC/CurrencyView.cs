using System;
using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.Faction.CurrencyMVC
{
    public class CurrencyView : IText
    {
        public Currency_Controller controller;

        Coords coords = new Coords()
        {
            x = 0,
            y = 0
        };

        public CurrencyView(Currency_Controller controller)
        {
            this.controller = controller;
        }

        public FloatCoords Coords { get => (FloatCoords)coords; set {; } }
        public string SpriteFont { get => "Currency"; set {; } }
        public string Text { get => "Currency: " + controller.model.currency.ToString(); set {; } }
        public Color Color { get => Color.White; set {; } }
        public int ZIndex { get => 1; set {; } }
    }
}
