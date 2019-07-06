using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Using Assets from 
// https://www.gameart2d.com/free-dino-sprites.html
// Creative Common Zero (CC0)

namespace SimpleSampleV3
{
    public class Player : FireCharacter
    {
        public static int score;


        public Player()
        {
            applyGravity = false;
        }

        public Player(Vector2 startingPosition)
        {
            this.position = startingPosition;
            applyGravity = false;

        }

        public override void Initialize()
        {
            score = 0;
            base.Initialize();
        }


        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"graphics/characters/PlayerSheet", content);
            LoadAnimations(@"Content\Graphics\Characters\Animations\PlayerAnimations.anm", content);
            ChangeAnimation(Animations.IdleRight);

            base.Load(content);
            boundingBoxOffset = new Vector2(0f, 0f);
            boundingBoxWidth = animationSet.frameWidth;
            boundingBoxHeight = animationSet.frameHeight;



        }

        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            CheckInput(gameObjects, map);
            base.Update(gameObjects, map);
        }

        protected override void UpdateAnimations()
        {
            if (currentAnimation == null)
                return;

            base.UpdateAnimations();

            if (velocity != Vector2.Zero && isJumping == false)
            {
                if (direction.X < 0 && AnimationIsNot(Animations.WalkingLeft))
                {
                    ChangeAnimation(Animations.WalkingLeft);
                }
                else if (direction.X > 0 && AnimationIsNot(Animations.WalkingRight))
                {
                    ChangeAnimation(Animations.WalkingRight);
                }
            }
            else if (velocity != Vector2.Zero && isJumping == true)
            {
                if (direction.X < 0 && AnimationIsNot(Animations.FallingLeft))
                {
                    ChangeAnimation(Animations.FallingLeft);
                }
                else if (direction.X > 0 && AnimationIsNot(Animations.FallingRight))
                {
                    ChangeAnimation(Animations.FallingRight);
                }
            }
            else if (velocity == Vector2.Zero && isJumping == false)
            {
                if (direction.X < 0 && AnimationIsNot(Animations.IdleLeft))
                {
                    ChangeAnimation(Animations.IdleLeft);
                }
                else if (direction.X > 0 && AnimationIsNot(Animations.IdleRight))
                {
                    ChangeAnimation(Animations.IdleRight);
                }
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private void CheckInput(List<GameObject> gameObjects, TiledMap map)
        {
            if (Input.IsKeyDown(Keys.D) == true)
                MoveRight();
            if (Input.IsKeyDown(Keys.A) == true)
                MoveLeft();
            if (Input.IsKeyDown(Keys.S) == true)
                MoveDown();
            if (Input.IsKeyDown(Keys.W) == true)
                MoveUp();

            if (Input.KeyPressed(Keys.Space))
            {
                Fire();
            }

        }



    }
}
