using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silesian_Undergrounds.Engine.Utils
{
    public class TimedEvent
    {
        private readonly int timeToExecute;
        private readonly bool isRepeatable;
        private readonly Action eventCode;
        private int currentTime;

        public TimedEvent(bool repeat, int timeInMs, Action action)
        {
            timeToExecute = timeInMs;
            isRepeatable = repeat;
            eventCode = action;
            currentTime = timeToExecute;
        }

        public void UpdateEvent(int diff)
        {
            currentTime -= diff;

            if (currentTime <= 0)
            {
                eventCode.Invoke();
                if (isRepeatable)
                    currentTime = timeToExecute;
            }
        }

        public bool IsReadyToDelete()
        {
            return !isRepeatable && currentTime <= 0;
        }
    }
}
