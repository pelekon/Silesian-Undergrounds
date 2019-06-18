using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Collisions;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Enum;
using Silesian_Undergrounds.Engine.Utils;
using Silesian_Undergrounds.Engine.Components;
using System;

namespace Silesian_Undergrounds.Engine.Traps
{
    public class Spike : PickableItem
    {
        private bool WasPicked = false;
        public Animator Animator { get; private set; }

        public Spike(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene) : base(texture, position, size, layer, scene, isBuyable: false)
        {
            BoxCollider collider = new BoxCollider(this, 20, 20, 0, 0, true);
            AddComponent(collider);

            Animator = new Animator(this);
            AddComponent(Animator);
        }

        public override void NotifyCollision(GameObject obj, ICollider source, RectCollisionSides collisionSides)
        {
            base.NotifyCollision(obj, source, collisionSides);

            if ((obj is Player) && !WasPicked)
            {
                // add damage
                WasPicked = true;
                Player player = obj as Player;
                player.DecreaseLiveValue((int)TrapsDamageEnum.Spikes);

                Animator.OnAnimationEnd += (sender, data) =>
                {
                    Scene.SceneManager.GetCurrentScene().DeleteObject(this);
                };

                if (!Animator.PlayAnimation("Activate"))
                    Scene.SceneManager.GetCurrentScene().DeleteObject(this);
            }
        }
    }
}