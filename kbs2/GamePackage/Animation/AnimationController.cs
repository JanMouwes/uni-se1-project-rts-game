using System;
using System.Collections.Generic;
using System.Linq;
using kbs2.GamePackage.Interfaces;

namespace kbs2.GamePackage.Animation
{
    public class AnimationController
    {
        private AnimationModel model = new AnimationModel();

        public const int UPDATES_PER_SECOND = 60;

        public void AddAnimation(List<IViewItem> viewItems)
        {
            //    Add items to existing queue-items
            Queue<IViewItem> itemsToAdd = new Queue<IViewItem>(viewItems);
            foreach (List<IViewItem> viewItem in model.AnimationQueue)
            {
                if (!itemsToAdd.Any()) break;
                
                viewItem.Add(itemsToAdd.Dequeue());
            }

            //    If any items left, add new lists.
            if (!itemsToAdd.Any()) return;

            foreach (IViewItem viewItem in viewItems)
            {
                model.AnimationQueue.Enqueue(new List<IViewItem>() {viewItem});
            }
        }

        public bool HasNextFrames => model.AnimationQueue.Count > 0;

        public List<IViewItem> NextFrame => HasNextFrames ? model.AnimationQueue.Dequeue() : new List<IViewItem>();

        //    Calculate amount of frames
        public void AddAnimation_ByMillis(List<IViewItem> viewItems, int millis) => AddAnimation_ByFrames(viewItems, (int) Math.Floor((millis / (float) 1000) * UPDATES_PER_SECOND));

        //    FIXME add on to beginning and end
        //    If fewer viewitems than frames, add on to end
        public void AddAnimation_ByFrames(List<IViewItem> viewItems, int frames)
        {
            if (viewItems.Count < frames)
            {
                for (int i = 0; i < frames - viewItems.Count; i++)
                {
                    viewItems.Add(viewItems.Last());
                }
            }

            AddAnimation(viewItems);
        }
    }
}