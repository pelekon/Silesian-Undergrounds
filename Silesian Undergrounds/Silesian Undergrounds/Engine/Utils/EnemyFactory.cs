using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Behaviours;

namespace Silesian_Undergrounds.Engine.Utils
{
    public sealed class EnemyFactory
    {
        public static GameObject TestEnemyFactory()
        {
            Texture2D texture = TextureMgr.Instance.GetTexture("test");
            GameObject obj = new GameObject(texture, new Vector2(300, 300), new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), 4, new Vector2(1.5f, 1.5f));
            AttackPattern attackPattern = new AttackPattern();
            // set up attacks
            AttackData attOne = new AttackData(true, 10, 15, 4000, AttackType.ATTACK_TYPE_MELEE, 0.0f, 5.0f);
            attackPattern.AddAttack(attOne);
            HostileBehaviour behaviour = new HostileBehaviour(obj, attackPattern);
            obj.AddComponent(behaviour);
            return obj;
        }
    }
}
