using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silesian_Undergrounds.Engine.Behaviours
{
    public class AttackPattern
    {
        public List<AttackData> attacks { get; private set; }

        public AttackPattern()
        {
            attacks = new List<AttackData>();
        }

        public void AddAttack(AttackData data)
        {
            attacks.Add(data);
        }
    }
}
