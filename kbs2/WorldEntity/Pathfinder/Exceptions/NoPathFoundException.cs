using System;
using kbs2.World;

namespace kbs2.WorldEntity.Pathfinder.Exceptions
{
    public class NoPathFoundException : Exception
    {
        public NoPathFoundException(Coords from, Coords to) : base($"Path from {from} to {to} not found")
        {
        }
    }
}