using System.Collections.Generic;
using kbs2.GamePackage.Interfaces;

namespace kbs2.GamePackage.Animation
{
    public class AnimationModel
    {
        public Queue<List<IViewItem>> AnimationQueue { get; set; } = new Queue<List<IViewItem>>();
    }
}