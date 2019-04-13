using System;
using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Utils;
using Silesian_Undergrounds.Engine.Collisions;

namespace Silesian_Undergrounds.Engine.Item
{
    public class Chest : PickableItem
    {
        private const int NumberOfChestTexture = 4;
        // time since last frame change
        private double timeSinceLastFrameChange;
        // time it takes to update theframe
        private double timeToUpdateFrame;
        // FPS
        public int FramesPerSecond
        {
            set { timeToUpdateFrame = (1f / value); }
        }
        private int CurrentFrame = 1;
        private Boolean WasPicked = false;

        public Chest(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene) : base(texture, position, size, layer, scene)
        {
            TextureMgr.Instance.LoadIfNeeded("Items/Chests/chest_2");
            TextureMgr.Instance.LoadIfNeeded("Items/Chests/chest_3");
            TextureMgr.Instance.LoadIfNeeded("Items/Chests/chest_4");

            FramesPerSecond = 10;
            BoxCollider collider = new BoxCollider(this, 59, 46, 0, 0, false);
            AddComponent(collider);
        }

        public override void NotifyCollision(GameObject obj)
        {
            base.NotifyCollision(obj);

            if (obj is Player)
            {
                Player plr = obj as Player;
                if (!WasPicked && plr.KeyAmount > 0)
                {
                    WasPicked = true;
                    plr.RemoveKey(1);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(WasPicked)
            {
                timeSinceLastFrameChange += gameTime.ElapsedGameTime.TotalSeconds;

                if (timeSinceLastFrameChange > timeToUpdateFrame && CurrentFrame <= NumberOfChestTexture)
                {
                    timeSinceLastFrameChange -= timeToUpdateFrame;
                    CurrentFrame++;
                    this.texture = TextureMgr.Instance.GetTexture("Items/Chests/chest_" + CurrentFrame);
                } else if(CurrentFrame == NumberOfChestTexture)
                {
                    this.scene.DeleteObject(this);
                }
           }

            base.Update(gameTime);
        }
    }
}
