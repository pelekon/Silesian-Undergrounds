using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Silesian_Undergrounds.Engine.Common
{
    public abstract class AnimatedGameObject : Gameobject
    {

        #region AnimationVariables
        // eum telling in which direction the animatedGO is moving
        public enum movementDirection { standstill, left, right, up, down }
        protected movementDirection currentDirection = movementDirection.standstill;
        // texture and position in GameObject
        //number of the frames in the animation 
        private int numberFrames;
        // time since last frame change
        private double timeSinceLastFrameChange;
        // time it takes to update theframe
        private double timeToUpdateFrame;
        // name of the current animation 
        protected string currentAnimation;
        // the velocity of the current object
        protected Vector2 sDirection = Vector2.Zero;
        // FPS's
        public int FramesPerSecond
        {
            set { timeToUpdateFrame = (1f / value); }
        }
        #endregion

        #region AnimationDictionaries
        // all animations dictionary
        private Dictionary<string, Rectangle[]> animationDic = new Dictionary<string, Rectangle[]>();
        // dictionary that contains animations offsets
        private Dictionary<string, Vector2> animOffsets = new Dictionary<string, Vector2>();
        #endregion

        #region Methods
        public AnimatedGameObject(Vector2 position, Vector2 size, int layer) : base(null, position, size, layer)
        {
            speed = 50f;
        }

        // Adds animation toAnimatedGameObject
        public void AddAnimation(int frames, int yPos, int xStartFrame, string name, int width, int height, Vector2 offset)
        {
            Rectangle[] Rectangles = new Rectangle[frames];

            for (int i = 0; i < frames; i++)
            {
                //new Rectangle(x, y, width, height);
                //new Rectangle(xStartFrame, yPos, width, height)
                // 
                Rectangles[i] = new Rectangle(xStartFrame, yPos + (i* (height + (2 * yPos))), width, height);
            }
            animationDic.Add(name, Rectangles);
            animOffsets.Add(name, offset);
        }

        // plays the animation
        public void PlayAnimation(string name)
        {
            //Makes sure we won't start a new annimation unless it differs from our current animation
            if (currentAnimation != name && currentDirection == movementDirection.standstill)
            {
                Debug.WriteLine("Current animation is: " + name);
                currentAnimation = name;
                numberFrames = 0;
            }
        }

        // determines when we have to change frames
        public override void Update(GameTime gameTime)
        {
            //Adds time that has elapsed since our last draw
            timeSinceLastFrameChange += gameTime.ElapsedGameTime.TotalSeconds;

            //We need to change our image if our timeElapsed is greater than our timeToUpdate(calculated by our framerate)
            if (timeSinceLastFrameChange > timeToUpdateFrame)
            {
                //Resets the timer in a way, so that we keep our desired FPS
                timeSinceLastFrameChange -= timeToUpdateFrame;

                //Adds one to our frameIndex
                if (numberFrames < animationDic[currentAnimation].Length - 1)
                {
                    numberFrames++;
                }
                else //Restarts the animation
                {
                    AnimationDone(currentAnimation);
                    numberFrames = 0;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position + animOffsets[currentAnimation], animationDic[currentAnimation][numberFrames], Color.White);
        }

        // callback called every time animation is finished
        public abstract void AnimationDone(string animation);
        #endregion

    }
}
