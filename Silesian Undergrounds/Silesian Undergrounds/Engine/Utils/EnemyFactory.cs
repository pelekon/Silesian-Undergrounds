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
               if (chance <= 30)
                list.Add(RatFactory(pos.position));
               else if(chance > 30 && chance <= 50)
                list.Add(MinotaurFactory(pos.position));
               else if (chance > 50 && chance <= 90)
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
            AttackData attackData = new AttackData(true, 15, 30, 3000, AttackType.ATTACK_TYPE_MELEE, 5, 30);
            attackPattern.AddAttack(attackData);

            obj.speed = 2.0f;

            HostileBehaviour behaviour = new HostileBehaviour(obj, attackPattern, 150, 10);

            TextureMgr.Instance.LoadAnimationFromSpritesheet("Monsters/minotaur_obrocony", "Monsters/Minotaur_MoveRight", 7, 8, 1, 8, 0, 0, false);
            TextureMgr.Instance.LoadAnimationFromSpritesheet("Monsters/minotaur_obrocony", "Monsters/Minotaur_Attack", 7, 8, 3, 8, 0, 0, false);
            TextureMgr.Instance.LoadAnimationFromSpritesheet("Monsters/minotaur_obrocony", "Monsters/Minotaur_MoveLeft", 7, 8, 6, 8, 0, 0, false);
            TextureMgr.Instance.LoadAnimationFromSpritesheet("Monsters/minotaur_obrocony", "Monsters/Minotaur_dead", 7, 8, 5, 3, 0, 0, false);

            behaviour.Animator.AddAnimation("MoveRight", TextureMgr.Instance.GetAnimation("Monsters/Minotaur_MoveRight"), 1000);
            behaviour.Animator.AddAnimation("MoveUp", TextureMgr.Instance.GetAnimation("Monsters/Minotaur_MoveRight"), 1000);

            behaviour.Animator.AddAnimation("MoveDown", TextureMgr.Instance.GetAnimation("Monsters/Minotaur_MoveLeft"), 1000);
            behaviour.Animator.AddAnimation("MoveLeft", TextureMgr.Instance.GetAnimation("Monsters/Minotaur_MoveLeft"), 1000);

            behaviour.Animator.AddAnimation("Attack", TextureMgr.Instance.GetAnimation("Monsters/Minotaur_Attack"), 1000);
            behaviour.Animator.AddAnimation("Dead", TextureMgr.Instance.GetAnimation("Monsters/Minotaur_MoveLeft"), 1000);

            obj.AddComponent(behaviour);
            return obj;
        }

       public static GameObject RatFactory(Vector2 position)
        {
            TextureMgr.Instance.LoadSingleTextureFromSpritescheet("Monsters/48x48Rat_FullSheet", "Monsters/Rat", 4, 8, 0, 0, 0, 5);
            Texture2D texture = TextureMgr.Instance.GetTexture("Monsters/Rat");

            GameObject obj = new GameObject(texture, position, new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), 6);
            AttackPattern attackPattern = new AttackPattern();
            AttackData attackData = new AttackData(true, 8, 13, 2000, AttackType.ATTACK_TYPE_MELEE, 5, 20);
            attackPattern.AddAttack(attackData);

            obj.speed = 3.0f;

            HostileBehaviour behaviour = new HostileBehaviour(obj, attackPattern, 70, 5);


            TextureMgr.Instance.LoadAnimationFromSpritesheet("Monsters/48x48Rat_FullSheet", "Monsters/Rat_MoveRight", 5, 8, 1, 6, 0, 0, false); 
            TextureMgr.Instance.LoadAnimationFromSpritesheet("Monsters/48x48Rat_FullSheet", "Monsters/Rat_Attack", 5, 8, 3, 8, 0, 0, false);
            TextureMgr.Instance.LoadAnimationFromSpritesheet("Monsters/48x48Rat_FullSheet", "Monsters/Rat_MoveLeft", 5, 8, 4, 6, 0, 0, false);
            TextureMgr.Instance.LoadAnimationFromSpritesheet("Monsters/48x48Rat_FullSheet", "Monsters/Rat_dead", 5, 8, 2, 6, 0, 0, false);

            behaviour.Animator.AddAnimation("MoveRight", TextureMgr.Instance.GetAnimation("Monsters/Rat_MoveRight"), 1000);
            behaviour.Animator.AddAnimation("MoveUp", TextureMgr.Instance.GetAnimation("Monsters/Rat_MoveRight"), 1000);

            behaviour.Animator.AddAnimation("MoveDown", TextureMgr.Instance.GetAnimation("Monsters/Rat_MoveLeft"), 1000);
            behaviour.Animator.AddAnimation("MoveLeft", TextureMgr.Instance.GetAnimation("Monsters/Rat_MoveLeft"), 1000);

            behaviour.Animator.AddAnimation("Attack", TextureMgr.Instance.GetAnimation("Monsters/Rat_Attack"), 1000);
            behaviour.Animator.AddAnimation("Dead", TextureMgr.Instance.GetAnimation("Monsters/Rat_dead"), 1000);

            obj.AddComponent(behaviour);          
            return obj;
        }

        public static GameObject WormFactory(Vector2 position)
        {
            TextureMgr.Instance.LoadSingleTextureFromSpritescheet("Monsters/48x48Worm_FullSheet", "Monsters/Worm", 4, 8, 0, 0, 0, 5);
            Texture2D texture = TextureMgr.Instance.GetTexture("Monsters/Worm");

            GameObject obj = new GameObject(texture, position, new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), 6);
            AttackPattern attackPattern = new AttackPattern();

            AttackData attackData = new AttackData(true, 5, 10, 1000, AttackType.ATTACK_TYPE_MELEE, 5, 15);
            attackPattern.AddAttack(attackData);

            obj.speed = 1.0f;
            HostileBehaviour behaviour = new HostileBehaviour(obj, attackPattern, 40, 1);


            TextureMgr.Instance.LoadAnimationFromSpritesheet("Monsters/worm_obrocony", "Monsters/Worm_MoveRight", 5, 8, 1, 6, 0, 5, false);
            TextureMgr.Instance.LoadAnimationFromSpritesheet("Monsters/worm_obrocony", "Monsters/Worm_Attack", 5, 8, 3, 6, 0, 5, false);
            TextureMgr.Instance.LoadAnimationFromSpritesheet("Monsters/worm_obrocony", "Monsters/Worm_MoveLeft", 5, 8, 4, 6, 0, 5, false);
            TextureMgr.Instance.LoadAnimationFromSpritesheet("Monsters/worm_obrocony", "Monsters/Worm_dead", 5, 8, 2, 6, 0, 5, false);

            behaviour.Animator.AddAnimation("MoveRight", TextureMgr.Instance.GetAnimation("Monsters/Worm_MoveRight"), 1000);
            behaviour.Animator.AddAnimation("MoveUp", TextureMgr.Instance.GetAnimation("Monsters/Worm_MoveRight"), 1000);

            behaviour.Animator.AddAnimation("MoveDown", TextureMgr.Instance.GetAnimation("Monsters/Worm_MoveLeft"), 1000);
            behaviour.Animator.AddAnimation("MoveLeft", TextureMgr.Instance.GetAnimation("Monsters/Worm_MoveLeft"), 1000);

            behaviour.Animator.AddAnimation("Attack", TextureMgr.Instance.GetAnimation("Monsters/Worm_Attack"), 1000);
            behaviour.Animator.AddAnimation("Dead", TextureMgr.Instance.GetAnimation("Monsters/Worm_dead"), 1000);
            
            obj.AddComponent(behaviour);

            return obj;
        }
    }
}
