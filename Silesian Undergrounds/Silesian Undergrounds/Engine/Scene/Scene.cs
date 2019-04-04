using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Utils;

namespace Silesian_Undergrounds.Engine.Scene
{
    public class Scene
    {

        #region SCENE_VARIABLES
        private List<GameObject> gameObjects;
        public Player player;
        private List<GameObject> objectsToDelete;
        private List<GameObject> objectsToAdd;
        public Camera camera { get; private set; }


        public bool isPaused { get; private set; }

        #endregion

        public Scene()
        {
            // Inittialize variables
            gameObjects = new List<GameObject>();
            objectsToDelete = new List<GameObject>();
            objectsToAdd = new List<GameObject>();
            isPaused = false;
            player = new Player(new Vector2(100, 100), new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), 1, new Vector2(2.5f, 2.5f));


            TextureMgr.Instance.LoadIfNeeded("minerCharacter");
            player.texture = TextureMgr.Instance.GetTexture("minerCharacter");
            gameObjects.Add(player);

            camera = new Camera(player);
        }
        public List<GameObject> GameObjects { get; private set; }


        #region SCENE_OBJECTS_MANAGMENT_METHODS

        public void AddObject(GameObject obj)
        {
            objectsToAdd.Add(obj);
        }

        public void DeleteObject(GameObject obj)
        {
            objectsToDelete.Add(obj);
        }

        private void AddObjects()
        {
            foreach (var obj in objectsToAdd)
                gameObjects.Add(obj);

            objectsToAdd.Clear();
        }

        private void DeleteObjects()
        {
            foreach (var obj in objectsToDelete)
                gameObjects.Remove(obj);

            objectsToDelete.Clear();
        }

        #endregion



        public void Update(GameTime gameTime)
        {
            // Operation of add or remove from gameObjects list has to appear before updating gameObjects
            AddObjects();
            DeleteObjects();

            foreach (var obj in gameObjects)
                obj.Update(gameTime);

            camera.Update(gameTime);
            player.Collision(this.gameObjects);
        }

        public void Draw()
        {
            Drawer.Draw((spriteBatch, gameTime) =>
            {
                foreach (var obj in gameObjects)
                {
                    if (obj is Player)
                        continue;

                    if (obj.layer != 3)
                    {
                        obj.Draw(spriteBatch);
                    }
                }

                player.Draw(spriteBatch);

            }, transformMatrix: camera.Transform);

            Drawer.Shaders.DrawPickUpEffect((spriteBatch, gameTime) =>
            {
                foreach (var obj in gameObjects)
                    if (obj.layer == 3)
                        obj.Draw(spriteBatch);
            }, transformMatrix: camera.Transform);
        }

        public void OpenPauseMenu()
        {

        }

        public void PauseGame()
        {
            isPaused = true;
        }
    }
}

