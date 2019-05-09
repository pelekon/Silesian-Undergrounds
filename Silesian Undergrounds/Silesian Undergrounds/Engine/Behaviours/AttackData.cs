using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silesian_Undergrounds.Engine.Behaviours
{
    public enum AttackType
    {
        ATTACK_TYPE_MELEE,
        ATTACK_TYPE_RANGED,
    }

    public struct AttackData
    {
        public AttackData(bool isRepeatable, int minDamage, int maxDamage, int attackTimer, AttackType type, float minRange, float maxRange)
        {
            IsRepeatable = isRepeatable;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            AttackTimer = attackTimer;
            this.type = type;
            MinRange = minRange;
            MaxRange = maxRange;
        }

        public bool IsRepeatable { get; private set; }
        public int MinDamage { get; private set; }
        public int MaxDamage { get; private set; }
        public int AttackTimer { get; private set; }
        public AttackType type { get; private set; }
        public float MinRange { get; private set; }
        public float MaxRange { get; private set; }
        // public List<Texture2D> attackAnim { get; private set; }
    }
}
