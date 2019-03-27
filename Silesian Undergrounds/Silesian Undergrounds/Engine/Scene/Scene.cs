using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Player;

namespace Silesian_Undergrounds.Engine.Scene
{
    public class Scene
    {
        public Scene()
        {
            gameobjects = new List<Gameobject>();
            objectsToDelete = new List<Gameobject>();
            objectsToAdd = new List<Gameobject>();
            // player = new Player.Player(); TODO: Pass data by constructor to create player object
        }

        // Scene variables
        private List<Gameobject> gameobjects;
        private Player.Player player;

        private List<Gameobject> objectsToDelete;
        private List<Gameobject> objectsToAdd;
        
        // Methods
        public void AddObject(Gameobject obj)
        {
            objectsToAdd.Add(obj);
        }

        public void DeleteObject(Gameobject obj)
        {
            objectsToDelete.Add(obj);
        }

        private void AddObjects()
        {
            foreach (var obj in objectsToAdd)
                gameobjects.Add(obj);

            objectsToAdd.Clear();
        }

        private void DeleteObjects()
        {
            foreach (var obj in objectsToDelete)
                gameobjects.Remove(obj);

            objectsToDelete.Clear();
        }

        public void Update(GameTime gameTime)
        {
            // Operation of add or remove from gameobjects list has to appear before updating gameobjects
            AddObjects();
            DeleteObjects();

            foreach (var obj in gameobjects)
                obj.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var obj in gameobjects)
                obj.Draw(spriteBatch);
        }
    }
}
