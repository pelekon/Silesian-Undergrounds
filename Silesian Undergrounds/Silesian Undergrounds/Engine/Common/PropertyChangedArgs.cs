using System;

namespace Silesian_Undergrounds.Engine.Common
{
    public class PropertyChangedArgs<T> : EventArgs
    {
        public readonly T NewValue;
        public readonly T OldValue;

        public PropertyChangedArgs(T Old, T New)
        {
            OldValue = Old;
            NewValue = New;
        }
    }
}
