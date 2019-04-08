using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.Enum;

namespace Silesian_Undergrounds.Engine.Item {
    class Chest : PickableItem {

        public Chest(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene) : base(texture, position, size, layer, scene)
        {
        }

        public override void NotifyCollision(GameObject obj)
        {
            if (obj is Player)
            {
                Player plr = obj as Player;
                
                if (plr.KeyAmount > 0)
                {
                    this.scene.DeleteObject(this);
                    plr.RemoveKey(1);
                }
                // TODO implement chest opening, player keys counter decrease, spawn pickable items != chest
            }
        }
    }
}
