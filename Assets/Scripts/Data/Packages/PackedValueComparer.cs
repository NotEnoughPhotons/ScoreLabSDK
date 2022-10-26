using System.Collections.Generic;

namespace NEP.ScoreLab.Data
{
    public class PackedValueComparer : IEqualityComparer<PackedValue>
    {
        public bool Equals(PackedValue first, PackedValue second)
        {
            if(ReferenceEquals(first, second))
            {
                return true;
            }

            if(ReferenceEquals(first, null) || ReferenceEquals(null, first))
            {
                return false;
            }

            return first.eventType == second.eventType;
        }

        public int GetHashCode(PackedValue value)
        {
            if(ReferenceEquals(value, null))
            {
                return 0;
            }

            int hashProductEventType = value.eventType == null ? 0 : value.eventType.GetHashCode();

            return hashProductEventType;
        }
    }
}
