using System;
using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Utils;
using Silesian_Undergrounds.Engine.Collisions;
using System.Collections.Generic;
using System.Linq;
using Silesian_Undergrounds.Engine.Config;
using Silesian_Undergrounds.Engine.Components;

namespace Silesian_Undergrounds.Engine.Item
{
    public class Chest : PickableItem
    {
        private int CurrentFrame = 1;
        private bool WasPicked = false;
        private Player player = null;
        private Random random = new Random();

        public Animator Animator { get; private set; }

        public Chest(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene, bool isBuyable = false) : base(texture, position, size, layer, scene, isBuyable)
        {
            BoxCollider collider = new BoxCollider(this, 59, 46, 0, 0, false);
            AddComponent(collider);

            Animator = new Animator(this);
            AddComponent(Animator);
        }

        public override void NotifyCollision(GameObject obj, ICollider source)
        {
            base.NotifyCollision(obj, source);

            if (obj is Player && !isBuyable)
            {
                Player plr = obj as Player;
                this.player = plr;
                if (!WasPicked && plr.KeyAmount > 0)
                {
                    WasPicked = true;
                    plr.RemoveKey(1);

                    Animator.OnAnimationEnd += (sender, name) =>
                    {
                        HandleAnimationEnd();
                    };

                    if (!Animator.PlayAnimation("Open"))
                        HandleAnimationEnd();
                }
            }
        }

        private void HandleAnimationEnd()
        {
            List<GameObject> list = new List<GameObject>();

            int itemAmount = random.Next(ConfigMgr.ChestConfig.MinimumNumberOfSpawnedItem, ConfigMgr.ChestConfig.NumberOfPossibleSpawnedItem);

            foreach (var obj in this.scene.GameObjects.Where(obj => obj.layer == 2).ToList())
            {

                if (Math.Abs(obj.position.X - this.position.X) <= ConfigMgr.ChestConfig.RangeOfSpawn * this.size.X && Math.Abs(obj.position.Y - this.position.Y) <= ConfigMgr.ChestConfig.RangeOfSpawn * this.size.X)
                {
                    if (obj.position != (this.scene.player.GetTileWhereStanding()) && itemAmount > 0)
                    {
                        list.Add(obj);
                        itemAmount--;
                    }
                }
            }

            foreach (var obj in GameObjectFactory.ScenePickableItemsFactory(list, this.scene, player.PlayerStatistic, generateChest: false))
            {
                this.scene.AddObject(obj);
            }

            this.scene.DeleteObject(this);
        }
    }
}
