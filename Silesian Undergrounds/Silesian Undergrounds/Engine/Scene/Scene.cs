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

        #region SCENE_VARIABLES

        private List<GameObject> gameobjects;
        private Player.Player player;
        private List<GameObject> objectsToDelete;
        private List<GameObject> objectsToAdd;
  
        public bool isPaused { get; private set; }

        #endregion

        public Scene(Player.Player player)
        {
            this.player = player;
            gameobjects = new List<GameObject>();
            objectsToDelete = new List<GameObject>();
            objectsToAdd = new List<GameObject>();
            // player = new Player.Player(); TODO: Pass data by constructor to create player object
            isPaused = false;
        }

        public List<GameObject> Gameobjects
        {
            get
            {
                return gameobjects;
            }
        }


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
                gameobjects.Add(obj);

            objectsToAdd.Clear();
        }

        private void DeleteObjects()
        {
            foreach (var obj in objectsToDelete)
                gameobjects.Remove(obj);

            objectsToDelete.Clear();
        }

        #endregion



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

            player.Draw(spriteBatch);
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
