using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.Enum;
using System.Diagnostics;

namespace Silesian_Undergrounds.Engine.Item {
    class Chest : PickableItem {
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
        }

        public override void NotifyCollision(GameObject obj)
        {
            if (obj is Player)
            {
                if (Player.keyAmount > 0)
                {
                    WasPicked = true;
                    ((Player)obj).RemoveKey(1);
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
