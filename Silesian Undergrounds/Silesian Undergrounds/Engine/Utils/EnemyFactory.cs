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
using Silesian_Undergrounds.Engine.Enemies;

namespace Silesian_Undergrounds.Engine.Utils
{
    public sealed class EnemyFactory
    {
        public static List<GameObject> GenerateEnemiesForScene(List<GameObject> positionSource)
        {
            List<GameObject> list = new List<GameObject>();

            Random rng = new Random();

            foreach(var pos in positionSource)
            {
                int chance = rng.Next(0, 100);
                if (chance <= 50)
                    list.Add(MinotaurFactory(pos.position));
                else
                    list.Add(WormFactory(pos.position));
            }

            return list;
        }

        public static GameObject MinotaurFactory(Vector2 position)
        {
            TextureMgr.Instance.LoadSingleTextureFromSpritescheet("Monsters/128x80Minotaur_FullSheet", "Monsters/Minotaur", 6, 8, 0, 0, 0, 15);
            Texture2D texture = TextureMgr.Instance.GetTexture("Monsters/Minotaur");

            if (TextureMgr.Instance.LoadAnimationFromSpritesheet("Monsters/128x80Minotaur_FullSheet", "Monsters/Minotaur_Attack", 6, 8, 2, 8, 0, 15, false))
                TextureMgr.Instance.LoadAnimationFromSpritesheet("Monsters/128x80Minotaur_FullSheet", "Monsters/Minotaur_Attack", 6, 8, 3, 8, 0, 15, true);

            GameObject obj = new GameObject(texture, position, new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), 6);
            AttackPattern attackPattern = new AttackPattern();
            HostileBehaviour behaviour = new HostileBehaviour(obj, attackPattern, 100, 10);
            obj.AddComponent(behaviour);
            behaviour.Animator.AddAnimation("Attack", TextureMgr.Instance.GetAnimation("Monsters/Minotaur_Attack"), 1000);

            return obj;
        }

        public static GameObject WormFactory(Vector2 position)
        {
            TextureMgr.Instance.LoadSingleTextureFromSpritescheet("Monsters/48x48Worm_FullSheet", "Monsters/Worm", 4, 8, 0, 0, 0, 5);
            Texture2D texture = TextureMgr.Instance.GetTexture("Monsters/Worm");

            GameObject obj = new GameObject(texture, position, new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), 6);
            AttackPattern attackPattern = new AttackPattern();
            HostileBehaviour behaviour = new HostileBehaviour(obj, attackPattern, 100, 10);
            obj.AddComponent(behaviour);

            return obj;
        }
    }
}
