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
    public class AnimatedObject : GameObject
    {
        protected int currentAnimationFrame;
        protected int animationTimer;
        protected int currentAnimationX = -1, currentAnimationY = -1;
        protected AnimationSet animationSet = new AnimationSet();
        protected Animation currentAnimation;

        protected bool flipForLeftFrames = true;
        protected bool flipForRightFrames = false;

        protected SpriteEffects spriteEffect = SpriteEffects.None;

        protected enum Animations
        {
            WalkingRight, WalkingLeft,
            IdleRight, IdleLeft,
            RunFiringRight, RunFiringLeft,
            FallingRight, FallingLeft
        }

        public bool IsAnimationComplete
        {
            get
            {
                return currentAnimationFrame >= currentAnimation.framesOrder.Count - 1;
            }

        }

        protected void LoadAnimations(string path, ContentManager content)
        {
            AnimationData animationData = AnimationLoader.Load(path);
            animationSet = animationData.AnimationSet;

            center.X = animationSet.frameWidth / 2;
            center.Y = animationSet.frameHeight / 2;

            //Default animation is first on the list
            if (animationSet.animationList.Count > 0)
            {
                currentAnimation = animationSet.animationList[0];
                currentAnimationFrame = 0;
                animationTimer = 0;
            }

        }


        protected void CalculateFramePosition()
        {
            int index1D = currentAnimation.framesOrder[currentAnimationFrame];
            currentAnimationX = (index1D % animationSet.gridX) * animationSet.frameWidth;
            currentAnimationY = (index1D / animationSet.gridX) * animationSet.frameHeight;
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            base.Update(gameObjects, map);
            if (currentAnimation != null)
            {
                UpdateAnimations();
            }
        }

        protected virtual void UpdateAnimations()
        {
            if (currentAnimation?.framesOrder.Count == 0)
                return;
            animationTimer -= 1;

            if (animationTimer < 0)
            {
                animationTimer = currentAnimation.animationSpeed;

                if (IsAnimationComplete) {
                    currentAnimationFrame = 0;
                }
                else {
                    currentAnimationFrame++;
                }

                CalculateFramePosition();
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (active == false)
                return;

            if (currentAnimationX == -1 || currentAnimationY == -1)
                base.Draw(spriteBatch);
            else
            {
                spriteBatch.Draw(texture, position, new Rectangle(currentAnimationX, currentAnimationY, animationSet.frameWidth, animationSet.frameHeight), tintColor, rotation, Vector2.Zero, scale, spriteEffect, layerDepth);
            }
        }

        protected virtual void ChangeAnimation(Animations newAnimation)
        {
            currentAnimation = GetAnimation(newAnimation);
            if (currentAnimation == null)
                return;

            currentAnimationFrame = 0;
            animationTimer = currentAnimation.animationSpeed;

            CalculateFramePosition();

            if (flipForLeftFrames == true && currentAnimation.name.Contains("Left") ||
                flipForRightFrames == true && currentAnimation.name.Contains("Right")) {
                spriteEffect = SpriteEffects.FlipHorizontally;
            }
            else {
                spriteEffect = SpriteEffects.None;
            }

        }

        private Animation GetAnimation(Animations animation)
        {
            string name = GetAnimationName(animation);
            foreach (Animation anim in animationSet.animationList)
            {
                if (anim.name == name)
                    return anim;
            }

            return null;
        }


        protected string GetAnimationName(Animations animation)
        {
            //Make an accurately spaced string. Example: "RunLeft" will now be "Run Left":
            return AddSpacesToSentence(animation.ToString(), false);
        }

        protected bool AnimationIsNot(Animations input)
        {
            //Used to check if our currentAnimation isn't set to the one passed in:
            return currentAnimation != null && GetAnimationName(input) != currentAnimation.name;
        }

        public string AddSpacesToSentence(string text, bool preserveAcronyms) //IfThisWasPassedIn... "If This Was Passed In" would be returned
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }



    }
}
