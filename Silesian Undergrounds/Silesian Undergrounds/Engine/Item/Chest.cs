using System;
using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Utils;
using Silesian_Undergrounds.Engine.Collisions;
using System.Diagnostics;
using Silesian_Undergrounds.Engine.CommonF;
using System.Collections.Generic;
using System.Linq;


namespace Silesian_Undergrounds.Engine.Item
{
    public class Chest : PickableItem
    {
        private const int NUMBER_OF_CHEST_TEXTURE = 4;
        private const int NUMBER_OF_POSSIBLE_SPAWNED_ITEM = 6;
        private const int MINIMUM_NUMBER_OF_SPAWNED_ITEM = 1;
        private const int RANGE_OF_SPAWN = 1;
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
        private Random random = new Random();

        public Chest(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene, bool isBuyable = false) : base(texture, position, size, layer, scene, isBuyable)
        {
            TextureMgr.Instance.LoadIfNeeded("Items/Chests/chest_2");
            TextureMgr.Instance.LoadIfNeeded("Items/Chests/chest_3");
            TextureMgr.Instance.LoadIfNeeded("Items/Chests/chest_4");

            FramesPerSecond = 10;
            BoxCollider collider = new BoxCollider(this, 59, 46, 0, 0, false);
            AddComponent(collider);
        }

        public override void NotifyCollision(GameObject obj, ICollider source)
        {
            base.NotifyCollision(obj, source);

            if (obj is Player && !isBuyable)
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

                if (timeSinceLastFrameChange > timeToUpdateFrame && CurrentFrame <= NUMBER_OF_CHEST_TEXTURE)
                {
                    timeSinceLastFrameChange -= timeToUpdateFrame;
                    CurrentFrame++;
                    this.texture = TextureMgr.Instance.GetTexture("Items/Chests/chest_" + CurrentFrame);
                } else if(CurrentFrame == NUMBER_OF_CHEST_TEXTURE)
                {
                    List<GameObject> list = new List<GameObject>();

                    int itemAmount = random.Next(MINIMUM_NUMBER_OF_SPAWNED_ITEM, NUMBER_OF_POSSIBLE_SPAWNED_ITEM);

                    foreach (var obj in this.scene.GameObjects.Where(obj => obj.layer == 2).ToList())
                    {
                        
                        if (Math.Abs(obj.position.X - this.position.X) <= RANGE_OF_SPAWN * this.size.X && Math.Abs(obj.position.Y - this.position.Y) <= RANGE_OF_SPAWN * this.size.X)
                        {
                            if (obj.position != (this.scene.player.GetTileWhereStanding()) && itemAmount > 0)
                            {
                                list.Add(obj);
                                itemAmount--;
                            }
                        }
                    }

                    foreach (var obj in GameObjectFactory.ScenePickableItemsFactory(list, this.scene))
                    {
                        this.scene.AddObject(obj);
                    }

                    this.scene.DeleteObject(this);
                }
           }

            base.Update(gameTime);
        }

    }
}
