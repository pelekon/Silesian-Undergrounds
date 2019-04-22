using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Collisions;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.CommonF;
using Silesian_Undergrounds.Engine.Enum;
using Silesian_Undergrounds.Engine.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Silesian_Undergrounds.Engine.Traps
{
    public class Spike : PickableItem
    {
        public Spike(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene) : base(texture, position, size, layer, scene, isBuyable: false)
        {
            TextureMgr.Instance.LoadIfNeeded("Items/Traps/temporary_spike");

            BoxCollider collider = new BoxCollider(this, 59, 46, 0, 0, false);
            AddComponent(collider);
        }


        public override void NotifyCollision(GameObject obj)
        {
            base.NotifyCollision(obj);

            if (obj is Player)
                // add damage
                ((Player)obj).DecreaseLiveValue((int)TrapsDamageEnum.Spikes);
        }

    }
}