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
    public class Map
    {
        public List<Decoration> decorations = new List<Decoration>();
        public List<Wall> walls = new List<Wall>();
        Texture2D wallTexture;

        public int mapWidth = 15;
        public int mapHeight = 9;

        public int tileSize = 128;

        public Map()
        {

        }


        public void Load(ContentManager content)
        {
            wallTexture = TextureLoader.Load("graphics/levels/pixel_box", content);

        }

        public void LoadMap(ContentManager content)
        {
            foreach (var decoration in decorations)
            {
                decoration.Load(content, decoration.imagePath);
            }
        }

        public Rectangle CheckCollision(Rectangle input)
        {
            foreach (var wall in walls)
            {
                if (wall != null && wall.boundingBox.Intersects(input) == true)
                {
                    return wall.boundingBox;
                }
            }

            return Rectangle.Empty;
        }


        //public void Update(List<GameObject> gameObjects)
        //{
        //    foreach (var decoration in decorations)
        //    {
        //        decoration.Update(gameObjects, this);
        //    }
        //}

        public void DrawWalls(SpriteBatch spriteBatch)
        {
            foreach (var wall in walls)
            {
                if (wall != null && wall.active == true)
                {
                    spriteBatch.Draw(wallTexture, new Vector2(wall.boundingBox.X, wall.boundingBox.Y), Color.White);
                    //spriteBatch.Draw(wallTexture, new Vector2(wall.boundingBox.X, wall.boundingBox.Y), wall.boundingBox, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.7f);
                }
            }
        }

        public Point GetTileIndex(Vector2 inputPosition)
        {
            if (inputPosition == new Vector2(-1, -1))
            {
                return new Point(-1, -1);
            }

            return new Point((int)inputPosition.X / tileSize, (int)inputPosition.Y / tileSize);
        }

    }


    public class Wall
    {
        public Rectangle boundingBox;
        public bool active = true;

        public Wall()
        {

        }

        public Wall(Rectangle rect)
        {
            boundingBox = rect;
        }
    }

    public class Decoration : GameObject
    {
        public string imagePath;
        public Rectangle sourceRect;

        public string Name {
            get { return imagePath; }
        }

        
        public Decoration()
        {
            isCollidable = false;
        }

        public Decoration(Vector2 position, string imagePath, float layerDepth)
        {
            this.position = position;
            this.imagePath = imagePath;
            this.layerDepth = layerDepth;
        }

        public virtual void Load(ContentManager content, string assetPath)
        {
            texture = TextureLoader.Load(assetPath, content);
            texture.Name = assetPath;
            //imagePath = assetPath;

            boundingBoxWidth = texture.Width;
            boundingBoxHeight = texture.Height;

            if (sourceRect == Rectangle.Empty)
            {
                sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
            }

        }

        public void SetImage(Texture2D inputTexture, string assetPath)
        {
            texture = inputTexture;
            imagePath = assetPath;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null && active == true)
            {
                spriteBatch.Draw(texture, position, sourceRect, tintColor, rotation, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
            }
        }

    }
}
