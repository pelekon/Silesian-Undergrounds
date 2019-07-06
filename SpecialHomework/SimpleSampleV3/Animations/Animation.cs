using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace SimpleSampleV3
{
    public class Animation
    {
        public string name;
        public List<int> framesOrder = new List<int>();
        public int animationSpeed;

        public Animation()
        {

        }

        public Animation(string name, List<int> framesOrder, int animationSpeed)
        {
            this.name = name;
            this.framesOrder = framesOrder;
            this.animationSpeed = animationSpeed;
        }
    }

    public class AnimationSet
    {
        public int frameWidth;
        public int frameHeight;
        public int gridX;
        public int gridY;
        public List<Animation> animationList = new List<Animation>();

        public AnimationSet()
        {

        }

        public AnimationSet(int frameWidth, int frameHeight, int gridX, int gridY)
        {
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.gridX = gridX;
            this.gridY = gridY;

        }



    }

    public class AnimationData
    {
        public AnimationSet AnimationSet { get; set; }
        public string SpriteSheetPath { get; set; }
    }


}
