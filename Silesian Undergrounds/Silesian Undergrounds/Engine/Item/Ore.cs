using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.Enum;

namespace Silesian_Undergrounds.Engine.Item
{
    class Ore : PickableItem {

        public OreEnum type;

        private int value;

        public Ore(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene, OreEnum oreType) : base(texture, position, size, layer, scene)
        {
            this.type = oreType;
            setVaule(this.type);
        }

        private void setVaule(OreEnum oreType)
        {
            if (oreType == OreEnum.Coal)
                this.value = 1;
            else if (oreType == OreEnum.Silver)
                this.value = 5;
            else if (oreType == OreEnum.Gold)
                this.value = 10;
            else
                this.value = 0;
        }

        public override void NotifyCollision(GameObject obj)
        {
            if (obj is Player)
            {
                Player pl = (Player)obj;
                pl.AddMoney(this.value);
                this.scene.DeleteObject(this);
            }
        }

    }


}
