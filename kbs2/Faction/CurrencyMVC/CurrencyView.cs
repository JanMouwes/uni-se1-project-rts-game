using System;
using kbs2.GamePackage.Interfaces;
using kbs2.World;
using kbs2.World.Structs;
using Microsoft.Xna.Framework;

namespace kbs2.Faction.CurrencyMVC
{
    public class CurrencyView : IViewText
    {
        private readonly Currency_Model model;

        public FloatCoords Coords => new FloatCoords() {x = 135, y = 3};

        public string SpriteFont => "Currency";

        public string Text => $"PokeYen: {Math.Round(model.Currency, 2)}";

        public Color Colour => Color.White;

        public int ZIndex => 1;

        public ViewMode ViewMode => ViewMode.Full;

        public CurrencyView(Currency_Model model) => this.model = model;
    }
}