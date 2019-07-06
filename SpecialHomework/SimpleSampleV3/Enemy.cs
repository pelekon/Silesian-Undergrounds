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
    public class Enemy : Character
    {
        int respawnTimer;
        int maxRespawnTimer = 60;

        Random random = new Random();

        SoundEffect explosion;

        public Enemy()
        {

        }

        public Enemy(Vector2 position)
        {
            this.position = position;
        }


        public override void Initialize()
        {
            active = true;
            isCollidable = false;
            position.X = random.Next(0, 500);
            position.Y = 0;

            base.Initialize();
        }

        public override void Load(ContentManager content)
        {
            texture = TextureLoader.Load(@"graphics/characters/enemy", content);
            explosion = content.Load<SoundEffect>(@"Audio/explosion");
            base.Load(content);
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            if (respawnTimer > 0)
            {
                respawnTimer--;
                if (respawnTimer <= 0)
                    Initialize();
            }
            base.Update(gameObjects, map);
        }

        public override void BulletResponse()
        {
            active = false;
            respawnTimer = maxRespawnTimer;
            Player.score += 1;
            explosion.Play(1, 0, 0.5f);

            base.BulletResponse();
        }
    }
}
