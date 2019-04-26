using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Silesian_Undergrounds.Engine.Utils
{
    public class TimedEventsScheduler
    {
        private List<TimedEvent> events = new List<TimedEvent>();
        private List<TimedEvent> toAdd = new List<TimedEvent>();
        private List<TimedEvent> toDelete = new List<TimedEvent>();
        private int lastUpdateMs;

        public TimedEventsScheduler()
        {
            lastUpdateMs = 0;
        }

        public void Update(GameTime gameTime)
        {
            ExecuteListChanges();

            int diff = gameTime.ElapsedGameTime.Milliseconds - lastUpdateMs;
            lastUpdateMs = gameTime.ElapsedGameTime.Milliseconds;

            foreach (var e in events)
            {
                e.UpdateEvent(diff);

                if (e.IsReadyToDelete())
                    DeScheduleEvent(e);
            }
        }

        public void ScheduleEvent(int time, bool repeat, Action func)
        {
            toAdd.Add(new TimedEvent(repeat, time, func));
        }

        private void DeScheduleEvent(TimedEvent e)
        {
            toDelete.Add(e);
        }

        public void ClearAll()
        {
            toDelete.AddRange(events);
        }

        private void ExecuteListChanges()
        {
            if(toDelete.Count > 0)
            {
                foreach (var e in toDelete)
                    events.Remove(e);
                toDelete.Clear();
            }

            if (toAdd.Count > 0)
            {
                foreach (var e in toAdd)
                    events.Add(e);
                toAdd.Clear();
            }
        }
    }
}
