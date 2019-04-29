using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Collisions;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.CommonF;
using Silesian_Undergrounds.Engine.Enum;
using Silesian_Undergrounds.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Silesian_Undergrounds.Engine.Traps
{
    public class Spike : PickableItem
    {
        #region ANIMATION_VARIABLES
        private const int NUMBER_OF_SPIKE_TEXTURES = 3;
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
        #endregion

        public Spike(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene) : base(texture, position, size, layer, scene, isBuyable: false)
        {
            TextureMgr.Instance.LoadIfNeeded("Items/Traps/temporary_spike_1");
            TextureMgr.Instance.LoadIfNeeded("Items/Traps/temporary_spike_2");
            TextureMgr.Instance.LoadIfNeeded("Items/Traps/temporary_spike_3");

            FramesPerSecond = 1;
            BoxCollider collider = new BoxCollider(this, 59, 46, 0, 0, true);
            AddComponent(collider);
        }


        public override void NotifyCollision(GameObject obj, ICollider source)
        {
            base.NotifyCollision(obj, source);

            if ((obj is Player) && !WasPicked)
            {
              // add damage
              Player player = obj as Player;
              player.DecreaseLiveValue((int)TrapsDamageEnum.Spikes);
              WasPicked = true;
              
                //this.scene.DeleteObject(this);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (WasPicked)
            {
                timeSinceLastFrameChange += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeSinceLastFrameChange > timeToUpdateFrame && CurrentFrame <= NUMBER_OF_SPIKE_TEXTURES)
                {
                    timeSinceLastFrameChange -= timeToUpdateFrame;
                    CurrentFrame++;
                    this.texture = TextureMgr.Instance.GetTexture("Items/Traps/temporary_spike_" + CurrentFrame);
                }
                else if (CurrentFrame == NUMBER_OF_SPIKE_TEXTURES)
                    this.scene.DeleteObject(this);
            }
        }

    }
}