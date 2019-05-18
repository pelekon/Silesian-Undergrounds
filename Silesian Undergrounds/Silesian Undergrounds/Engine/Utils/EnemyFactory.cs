using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Behaviours;
using Silesian_Undergrounds.Engine.Scene;

namespace Silesian_Undergrounds.Engine.Utils
{
    public sealed class EnemyFactory
    {
        public static List<GameObject> GenerateEnemiesForScene(List<GameObject> positionSource)
        {
            List<GameObject> list = new List<GameObject>();

            return list;
        }

        public static GameObject MinotaurFactory(Vector2 position)
        {
            TextureMgr.Instance.LoadSingleTextureFromSpritescheet("Monsters/128x80Minotaur_FullSheet", "Monsters/Minotaur", 6, 8, 0, 0);
            Texture2D texture = TextureMgr.Instance.GetTexture("Monsters/Minotaur");

            if (TextureMgr.Instance.LoadAnimationFromSpritesheet("Monsters/128x80Minotaur_FullSheet", "Monsters/Minotaur_Attack", 6, 8, 2, 8, 0, 0, false))
                TextureMgr.Instance.LoadAnimationFromSpritesheet("Monsters/128x80Minotaur_FullSheet", "Monsters/Minotaur_Attack", 6, 8, 3, 8, 0, 0, true);

            GameObject obj = new GameObject(texture, position, new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), 6);
            AttackPattern attackPattern = new AttackPattern();
            HostileBehaviour behaviour = new HostileBehaviour(obj, attackPattern, 100, 10);
            obj.AddComponent(behaviour);
            behaviour.Animator.AddAnimation("Attack", TextureMgr.Instance.GetAnimation("Monsters/Minotaur_Attack"), 1000);

            return obj;
        }
    }
}
