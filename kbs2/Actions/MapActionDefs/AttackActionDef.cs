using System.Collections.Generic;
using kbs2.utils;
using kbs2.Unit;
using kbs2.World;
using kbs2.World.Structs;
using kbs2.WorldEntity.Structs;

namespace kbs2.Actions.MapActionDefs
{
    public class AttackActionDef : MapActionDef
    {
        public AttackActionDef(uint cooldown, ViewValues imageSource, int baseDamage, ElementType elementType) : base(cooldown, imageSource)
        {
            BaseDamage = baseDamage;
            ElementType = elementType;
        }

        public ElementType ElementType { get; }

        public int BaseDamage { get; }

        public override List<MapActionAnimationItem> GetAnimationItems(FloatCoords from, FloatCoords to) => new List<MapActionAnimationItem>()
        {
//            FloatCoords adjustedFrom =  - new Coords() {x = (int) (Icon.Width * 0.5)};
            new MapActionAnimationItem(from - new FloatCoords() {x = .5f}, 1, (float) DistanceCalculator.DiagonalDistance(from, to), Icon.Image, DistanceCalculator.DegreesFromCoords(from, to) - 90)
        };
    }
}