using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

using Silesian_Undergrounds.Engine.Components;
using Silesian_Undergrounds.Engine.Collisions;

namespace Silesian_Undergrounds.Engine.Common
{
    public class GameObject
    {
        public Texture2D texture { get; set; }
        public Vector2 position;
        public int layer;
        public float rotation { get; protected set; }
        public float speed { get; set; }
        public Vector2 size;
        public Vector2? scale;
        public Color color = Color.White;
        public Rectangle Rectangle { get { return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y); } }
        protected List<IComponent> components;
        private bool canDrawItself;

        public event EventHandler<CollisionNotifyData> OnCollision = delegate { };

        public GameObject(Texture2D texture, Vector2 position, Vector2 size, int layer = 1, Vector2? scale = null)
        {
            this.texture = texture;
            this.position = position;
            this.size = size;
            this.layer = layer;
            this.scale = scale;
            speed = 1.0f;

            components = new List<IComponent>();
            canDrawItself = true;
        }

        // causes movement
        public void AddForce(float forceX, float forceY)
        {
            position.X += forceX * speed;
            position.Y += forceY * speed;
        }

        public Vector2 GetTileWhereStanding()
        {
            return new Vector2((float)System.Math.Round(this.position.X / size.X) * size.X, (float)System.Math.Round(this.position.Y / size.Y) * size.Y);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var component in components)
                component.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (canDrawItself)
            {
                if (texture.Name == "Items/Food/meat_with_label")
                {
                    spriteBatch.Draw(texture: texture, destinationRectangle: new Rectangle((int)position.X, (int)position.Y, (int)size.X * 2, (int)size.Y * 2), scale: new Vector2(2f, 2f), color: color);
                }
                else
                {
                    spriteBatch.Draw(texture: texture, destinationRectangle: Rectangle, scale: scale, color: color);
                }
            }

            foreach (var component in components)
                component.Draw(spriteBatch);
        }

        public void AddComponent(IComponent c)
        {
            components.Add(c);
            c.RegisterSelf();
        }

        public void RemoveAllComponents()
        {
            foreach (var component in components)
            {
                component.CleanUp();
                component.UnRegisterSelf();
            }

            components.Clear();
        }

        public T GetComponent<T>()
        {
            foreach(var component in components)
            {
                if (component is T)
                    return (T)component;
            }

            return default(T);
        }
        
        public virtual void NotifyCollision(GameObject gameobject, ICollider source)
        {
            OnCollision.Invoke(this, new CollisionNotifyData(gameobject, source));
        }

        public void ChangeDrawAbility(bool val) { canDrawItself = val; }
    }
}
