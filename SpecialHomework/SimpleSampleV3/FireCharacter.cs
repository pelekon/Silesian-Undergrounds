using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimpleSampleV3
{
    public class FireCharacter : Character
    {
        List<Bullet> bullets = new List<Bullet>();
        const int numberOfBullets = 10;

        public FireCharacter()
        {

        }

        public override void Initialize()
        {
            if (bullets.Count == 0)
            {
                for (int i = 0; i < numberOfBullets; i++)
                {
                    bullets.Add(new Bullet());
                }
            }
            base.Initialize();
        }

        public override void Load(ContentManager content)
        {
            foreach (var bullet in bullets)
            {
                bullet.Load(content);
            }
            base.Load(content);
        }

        public void Fire()
        {
            foreach (var bullet in bullets)
            {
                if (bullet.active == false)
                {
                    bullet.Fire(this, position, direction);
                    break;
                }
            }
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            foreach (var bullet in bullets)
            {
                 bullet.Update(gameObjects, map);
            }
            base.Update(gameObjects, map);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
    }
}
