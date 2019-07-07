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

            Random rng = new Random();

            foreach (var pos in positionSource)
            {
                int chance = rng.Next(0, 100);
                if (chance <= 30)
                    list.Add(RatFactory(pos.position));
                else if (chance > 30 && chance <= 50)
                    list.Add(GhotsFactory(pos.position));
                else if (chance > 50 && chance <= 90)
                    list.Add(WormFactory(pos.position));
            }

            return list;
        }

        public static GameObject GetRandomEnemy(Vector2 position)
        {
            Random rng = TrueRng.GetInstance().GetRandom();
            int chance = rng.Next(0, 100);

            if (chance >= 70)
                return GhotsFactory(position);
            else if (chance >= 45)
                return RatFactory(position);

            return WormFactory(position);
        }

        public static GameObject GhotsFactory(Vector2 position)
        {
            TextureMgr.Instance.LoadSingleTextureFromSpritescheet(
                fileName: "Monsters/ghost",
                name: "Monsters/Ghost",
                spritesheetRows: 1,
                spritesheetColumns: 90,
                row: 0,
                column: 0,
                spacingX: 0,
                spacingY: 0
            );
            var attackPattern = new AttackPattern();
            var attackData = new AttackData(
              isRepeatable: true,
              minDamage: 15,
              maxDamage: 30,
              attackTimer: 3000,
              type: AttackType.ATTACK_TYPE_MELEE,
              minRange: 5,
              maxRange: 30
            );
            attackPattern.AddAttack(attackData);

            Texture2D texture = TextureMgr.Instance.GetTexture("Monsters/Ghost");
            var textureSize = new Vector2(48, 48);
            var obj = new GameObject(texture, position, textureSize, layer: 6);
            obj.speed = 4.0f;

            TextureMgr.Instance.LoadAnimationFromSpritesheet(
              fileName: "Monsters/ghost",
              animName: "Monsters/Ghost_MoveRight",
              spritesheetRows: 1,
              spritesheetColumns: 90,
              index: 0,
              amount: 8,
              skip: 12,
              spacingX: 0,
              spacingY: 0,
              canAddToExisting: false,
              loadByColumn: false
            );
            TextureMgr.Instance.LoadAnimationFromSpritesheet(
              fileName: "Monsters/ghost",
              animName: "Monsters/Ghost_Stand",
              spritesheetRows: 1,
              spritesheetColumns: 90,
              index: 0,
              amount: 5,
              skip: 5,
              spacingX: 0,
              spacingY: 0,
              canAddToExisting: false,
              loadByColumn: false
            );
            TextureMgr.Instance.LoadAnimationFromSpritesheet(
              fileName: "Monsters/ghost",
              animName: "Monsters/Ghost_Attack",
              spritesheetRows: 1,
              spritesheetColumns: 90,
              index: 0,
              amount: 5,
              skip: 9,
              spacingX: 0,
              spacingY: 0,
              canAddToExisting: false,
              loadByColumn: false
            );
            TextureMgr.Instance.LoadAnimationFromSpritesheet(
              fileName: "Monsters/ghost",
              animName: "Monsters/Ghost_MoveLeft",
              spritesheetRows: 1,
              spritesheetColumns: 90,
              index: 0,
              amount: 8,
              skip: 48,
              spacingX: 0,
              spacingY: 0,
              canAddToExisting: false,
              loadByColumn: false
          );
            TextureMgr.Instance.LoadAnimationFromSpritesheet(
              fileName: "Monsters/ghost",
              animName: "Monsters/Ghost_dead",
              spritesheetRows: 1,
              spritesheetColumns: 90,
              index: 0,
              amount: 7,
              skip: 24,
              spacingX: 0,
              spacingY: 0,
              canAddToExisting: false,
              loadByColumn: false
          );

            TextureMgr.Instance.LoadAnimationFromSpritesheet(
                fileName: "Monsters/ghost",
                animName: "Monsters/Ghost_hitRight",
                spritesheetRows: 1,
                spritesheetColumns: 90,
                index: 0,
                amount: 4,
                skip: 39,
                spacingX: 0,
                spacingY: 0,
                canAddToExisting: false,
                loadByColumn: false
            );
            TextureMgr.Instance.LoadAnimationFromSpritesheet(
                fileName: "Monsters/ghost",
                animName: "Monsters/Ghost_hitLeft",
                spritesheetRows: 1,
                spritesheetColumns: 90,
                index: 0,
                amount: 4,
                skip: 70,
                spacingX: 0,
                spacingY: 0,
                canAddToExisting: false,
                loadByColumn: false
            );
            TextureMgr.Instance.LoadAnimationFromSpritesheet(
                fileName: "Monsters/ghost",
                animName: "Monsters/Ghost_hitByParticle",
                spritesheetRows: 1,
                spritesheetColumns: 90,
                index: 0,
                amount: 3,
                skip: 27,
                spacingX: 0,
                spacingY: 4,
                canAddToExisting: false,
                loadByColumn: false
            );


            var behaviour = new HostileBehaviour(obj, attackPattern, 150, 10);
            behaviour.AddAnimation(AnimType.ON_STAND, TextureMgr.Instance.GetAnimation("Monsters/Ghost_Stand"), animDuration: 1000);

            behaviour.AddAnimation(AnimType.ON_MOVE_RIGHT, TextureMgr.Instance.GetAnimation("Monsters/Ghost_MoveRight"), animDuration: 1000);

            behaviour.AddAnimation(AnimType.ON_MOVE_LEFT, TextureMgr.Instance.GetAnimation("Monsters/Ghost_MoveLeft"), animDuration: 1000);

            behaviour.AddAnimation(AnimType.ON_DEATH, TextureMgr.Instance.GetAnimation("Monsters/Ghost_dead"), animDuration: 1000);

            behaviour.AddAnimation(AnimType.ON_HIT_LEFT, TextureMgr.Instance.GetAnimation("Monsters/Ghost_hitLeft"), animDuration: 500, isPermanent: true);
            behaviour.AddAnimation(AnimType.ON_HIT_RIGHT, TextureMgr.Instance.GetAnimation("Monsters/Ghost_hitRight"), animDuration: 500, isPermanent: true);

            behaviour.AddAnimation(AnimType.ON_PARTICLE_HIT, TextureMgr.Instance.GetAnimation("Monsters/Ghost_hitByParticle"), 500);

            obj.AddComponent(behaviour);
            return obj;
        }

        public static GameObject RatFactory(Vector2 position)
        {
            TextureMgr.Instance.LoadSingleTextureFromSpritescheet(
                fileName: "Monsters/rat",
                name: "Monsters/Rat",
                spritesheetRows: 1,
                spritesheetColumns: 52,
                row: 0,
                column: 0,
                spacingX: 0,
                spacingY: 0
            );
            Texture2D baseTexture = TextureMgr.Instance.GetTexture("Monsters/Rat");

            GameObject obj = new GameObject(baseTexture, position, new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), 6);
            AttackPattern attackPattern = new AttackPattern();
            AttackData attackData = new AttackData(true, 8, 13, 2000, AttackType.ATTACK_TYPE_MELEE, 5, 20);
            attackPattern.AddAttack(attackData);

            obj.speed = 3.0f;

            HostileBehaviour behaviour = new HostileBehaviour(obj, attackPattern, 70, 5);

            TextureMgr.Instance.LoadAnimationFromSpritesheet(
                fileName: "Monsters/rat",
                animName: "Monsters/Rat_MoveRight",
                spritesheetRows: 1,
                spritesheetColumns: 52,
                index: 0, amount: 4,
                skip: 8,
                spacingX: 0,
                spacingY: 0,
                canAddToExisting: false,
                loadByColumn: false
            );
            TextureMgr.Instance.LoadAnimationFromSpritesheet(
                fileName: "Monsters/rat",
                animName: "Monsters/Rat_Stand",
                spritesheetRows: 1,
                spritesheetColumns: 52,
                index: 0, amount: 8,
                skip: 0,
                spacingX: 0,
                spacingY: 0,
                canAddToExisting: false,
                loadByColumn: false
            );
            TextureMgr.Instance.LoadAnimationFromSpritesheet(
                fileName: "Monsters/rat",
                animName: "Monsters/Rat_Attack",
                spritesheetRows: 1,
                spritesheetColumns: 52,
                index: 0, amount: 10,
                skip: 20,
                spacingX: 0,
                spacingY: 0,
                canAddToExisting: false,
                loadByColumn: false
            );
            TextureMgr.Instance.LoadAnimationFromSpritesheet(
                fileName: "Monsters/rat",
                animName: "Monsters/Rat_MoveLeft",
                spritesheetRows: 1,
                spritesheetColumns: 52,
                index: 0, amount: 4,
                skip: 35,
                spacingX: 0,
                spacingY: 0,
                canAddToExisting: false,
                loadByColumn: false
            );
            TextureMgr.Instance.LoadAnimationFromSpritesheet(
                fileName: "Monsters/rat",
                animName: "Monsters/Rat_dead",
                spritesheetRows: 1,
                spritesheetColumns: 52,
                index: 0, amount: 5,
                skip: 13,
                spacingX: 0,
                spacingY: 0,
                canAddToExisting: false,
                loadByColumn: false
            );
            TextureMgr.Instance.LoadAnimationFromSpritesheet(
                fileName: "Monsters/rat",
                animName: "Monsters/Rat_OnHitRight",
                spritesheetRows: 1,
                spritesheetColumns: 52,
                index: 0, amount: 2,
                skip: 14,
                spacingX: 0,
                spacingY: 0,
                canAddToExisting: false,
                loadByColumn: false
            );
            TextureMgr.Instance.LoadAnimationFromSpritesheet(
                fileName: "Monsters/rat",
                animName: "Monsters/Rat_OnHitLeft",
                spritesheetRows: 1,
                spritesheetColumns: 52,
                index: 0, amount: 2,
                skip: 40,
                spacingX: 0,
                spacingY: 0,
                canAddToExisting: false,
                loadByColumn: false
            );

            TextureMgr.Instance.LoadAnimationFromSpritesheet(
               fileName: "Monsters/worm_obrocony",
               animName: "Monsters/Rat_HitByParticle",
               spritesheetRows: 5,
               spritesheetColumns: 8,
               index: 2,
               amount: 6,
                skip: 2,
               spacingX: 0,
               spacingY: 5,
               canAddToExisting: false
            );

            behaviour.AddAnimation(AnimType.ON_STAND, TextureMgr.Instance.GetAnimation("Monsters/Rat_Stand"), animDuration: 1000);


            behaviour.AddAnimation(AnimType.ON_MOVE_RIGHT, TextureMgr.Instance.GetAnimation("Monsters/Rat_MoveRight"), animDuration: 1000);

            behaviour.AddAnimation(AnimType.ON_MOVE_LEFT, TextureMgr.Instance.GetAnimation("Monsters/Rat_MoveLeft"), animDuration: 1000);

            behaviour.AddAnimation(AnimType.ON_ATTACK, TextureMgr.Instance.GetAnimation("Monsters/Rat_Attack"), animDuration: 1000);
            behaviour.AddAnimation(AnimType.ON_DEATH, TextureMgr.Instance.GetAnimation("Monsters/Rat_dead"), animDuration: 1000);

            behaviour.AddAnimation(AnimType.ON_HIT_LEFT, TextureMgr.Instance.GetAnimation("Monsters/Rat_OnHitLeft"), animDuration: 500, isPermanent: true);
            behaviour.AddAnimation(AnimType.ON_HIT_RIGHT, TextureMgr.Instance.GetAnimation("Monsters/Rat_OnHitRight"), animDuration: 500, isPermanent: true);

            behaviour.AddAnimation(AnimType.ON_PARTICLE_HIT, TextureMgr.Instance.GetAnimation("Monsters/Rat_HitByParticle"), 500);

            obj.AddComponent(behaviour);
            return obj;
        }

        public static GameObject WormFactory(Vector2 position)
        {
            TextureMgr.Instance.LoadSingleTextureFromSpritescheet(
                fileName: "Monsters/48x48Worm_FullSheet",
                name: "Monsters/Worm",
                spritesheetRows: 4,
                spritesheetColumns: 8,
                row: 0,
                column: 0,
                spacingX: 0,
                spacingY: 5
            );
            Texture2D texture = TextureMgr.Instance.GetTexture("Monsters/Worm");

            var obj = new GameObject(texture, position, new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), 6);
            var attackPattern = new AttackPattern();

            var attackData = new AttackData(true, 5, 10, 1000, AttackType.ATTACK_TYPE_MELEE, 5, 15);
            attackPattern.AddAttack(attackData);

            obj.speed = 1.0f;

            var behaviour = new HostileBehaviour(obj, attackPattern, 40, 1);

            TextureMgr.Instance.LoadAnimationFromSpritesheet(fileName: "Monsters/worm_obrocony",
              animName: "Monsters/Worm_Stand",
              spritesheetRows: 5,
              spritesheetColumns: 8,
              index: 0,
              amount: 6,
              spacingX: 0,
              spacingY: 5,
              canAddToExisting: false
            );

            TextureMgr.Instance.LoadAnimationFromSpritesheet(fileName: "Monsters/worm_obrocony",
               animName: "Monsters/Worm_MoveRight",
               spritesheetRows: 5,
               spritesheetColumns: 8,
               index: 1,
               amount: 6,
               spacingX: 0,
               spacingY: 5,
               canAddToExisting: false
            );
            TextureMgr.Instance.LoadAnimationFromSpritesheet(fileName: "Monsters/worm_obrocony",
               animName: "Monsters/Worm_Attack",
               spritesheetRows: 5,
               spritesheetColumns: 8,
               index: 3,
               amount: 4,
               spacingX: 0,
               spacingY: 5,
               canAddToExisting: false
            );
            TextureMgr.Instance.LoadAnimationFromSpritesheet(fileName: "Monsters/worm_obrocony",
                animName: "Monsters/Worm_MoveLeft",
                spritesheetRows: 5,
                spritesheetColumns: 8,
                index: 4,
                amount: 6,
                spacingX: 0,
                spacingY: 5,
                canAddToExisting: false
            );
            TextureMgr.Instance.LoadAnimationFromSpritesheet(fileName: "Monsters/worm_obrocony",
               animName: "Monsters/Worm_dead",
               spritesheetRows: 5,
               spritesheetColumns: 8,
               index: 2,
               amount: 8,
               spacingX: 0,
               spacingY: 5,
               canAddToExisting: false
            );

            TextureMgr.Instance.LoadAnimationFromSpritesheet(
               fileName: "Monsters/worm_obrocony",
               animName: "Monsters/worm_HitByParticle",
               spritesheetRows: 5,
               spritesheetColumns: 8,
               index: 2,
               amount: 6,
                skip: 2,
               spacingX: 0,
               spacingY: 5,
               canAddToExisting: false
            );

            behaviour.AddAnimation(AnimType.ON_STAND, TextureMgr.Instance.GetAnimation("Monsters/Worm_Stand"), animDuration: 1000);

            behaviour.AddAnimation(AnimType.ON_MOVE_RIGHT, TextureMgr.Instance.GetAnimation("Monsters/Worm_MoveRight"), 1000);

            behaviour.AddAnimation(AnimType.ON_MOVE_LEFT, TextureMgr.Instance.GetAnimation("Monsters/Worm_MoveLeft"), 1000);

            behaviour.AddAnimation(AnimType.ON_ATTACK, TextureMgr.Instance.GetAnimation("Monsters/Worm_Attack"), 1000);
            behaviour.AddAnimation(AnimType.ON_DEATH, TextureMgr.Instance.GetAnimation("Monsters/Worm_dead"), 1000);

            behaviour.AddAnimation(AnimType.ON_PARTICLE_HIT, TextureMgr.Instance.GetAnimation("Monsters/worm_HitByParticle"), 500);

            obj.AddComponent(behaviour);

            return obj;
        }
    }
}
