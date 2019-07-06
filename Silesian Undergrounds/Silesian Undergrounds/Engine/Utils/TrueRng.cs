using System;
using System.Collections.Generic;
using System.Threading;

namespace Silesian_Undergrounds.Engine.Utils
{
    public sealed class TrueRng
    {
        private static TrueRng instance;
        private int seed;
        private Dictionary<int /*threadId*/, Random> instancePerThread = new
            Dictionary<int, Random>();
        private object dictLock = new object();

        private TrueRng()
        {
            seed = CalculateSeed();
            // add rng for main thread
            instancePerThread.Add(1, new Random(seed));
        }

        public static TrueRng GetInstance()
        {
            if (instance == null)
                instance = new TrueRng();

            return instance;
        }

        private int CalculateSeed()
        {
            int value = Environment.TickCount;
            int temp = value * 5;
            temp /= 2;
            value += temp;

            return value;
        }

        public Random GetRandom()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            if (!instancePerThread.ContainsKey(threadId))
                AddRandForThread(threadId);

            return instancePerThread[threadId];
        }

        private void AddRandForThread(int id)
        {
            lock (dictLock)
                instancePerThread.Add(id, new Random(CalculateSeed()));
        }
    }
}
