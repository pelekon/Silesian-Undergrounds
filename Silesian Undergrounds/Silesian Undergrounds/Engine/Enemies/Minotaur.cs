using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Collisions;
using Silesian_Undergrounds.Engine.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Silesian_Undergrounds.Engine.Enemies
{
    public class Minotaur : GameObject
    {

        public Minotaur(Texture2D texture, Vector2 position, Vector2 size, int layer = 1, Vector2? scale = null) : base(texture, position, size, layer, scale) { }


        public override void NotifyCollision(GameObject obj, ICollider source)
        {
            Debug.WriteLine("Collistion");
        }

      

    }
}