using System;
using System.Collections.Generic;
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
        public float rotation;
        public float speed;
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
        public AnimatedGameObject(Texture2D texture, Vector2 position, Vector2 size) : base(texture, position, size)
        {
            speed = 50f;
        }

        // Adds animation toAnimatedGameObject
        public void AddAnimation(int frames, int yPos, int xStartFrame, string name, int width, int height, Vector2 offset)
        {
            Rectangle[] Rectangles = new Rectangle[frames];

            for (int i = 0; i < frames; i++)
            {
                Rectangles[i] = new Rectangle((i + xStartFrame) * width, yPos, width, height);
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
                currentAnimation = name;
                FramesPerSecond = 0;
            }
        }

        // causes movement
        protected void AddForce(float forceX, float forceY)
        {
            position.X += forceX * speed;
            position.Y += forceY * speed;
        }

        // determines when we have to change frames
        public virtual void Update(GameTime gameTime)
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
        // callback called every time animation is finished
        public abstract void AnimationDone(string animation);
        #endregion

    }
}
