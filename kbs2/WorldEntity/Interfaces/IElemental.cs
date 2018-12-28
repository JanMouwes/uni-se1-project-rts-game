using System;
using System.Collections.Generic;

namespace kbs2.Unit.Interfaces
{
    public interface IElemental
    {
        List<ElementType> ElementTypes
        {
            get;
            set;
        }
    }
}
