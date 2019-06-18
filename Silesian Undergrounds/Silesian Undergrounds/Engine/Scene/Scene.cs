using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Config;
using Silesian_Undergrounds.Engine.Utils;
using Silesian_Undergrounds.Engine.UI;
using Silesian_Undergrounds.Views;
using Silesian_Undergrounds.Engine.Collisions;
using Silesian_Undergrounds.Engine.Enum;
using System;

namespace Silesian_Undergrounds.Engine.Scene
{
    public class Scene
    {

        #region SCENE_VARIABLES
        private List<GameObject> gameObjects;
        private List<GameObject> objectsToDelete;
        private List<GameObject> objectsToAdd;
        private List<GameObject> transitions;

        public Player player;
        private UIArea ui;
        private UIArea pauseMenu;
        public Camera camera { get; private set; }
        private Func<bool> OnPlayerWin;

        public bool isPaused { get; private set; }
        public bool isEnd { get; private set; }
        public bool lastScene { get; private set; }
        private bool isBoosterPicked;
        private const float shaderDelayInSeconds = 50;
        private float remainingShaderDelayInSeconds = shaderDelayInSeconds;
        private readonly bool canUnPause;

        #endregion

        public Scene(PlayerStatistic playerStatistic)
        {
            CollisionSystem.CleanUp();

            InitLists();
            isPaused = false;
            player = new Player(new Vector2(200, 200), new Vector2(ResolutionMgr.TileSize, ResolutionMgr.TileSize), 1, new Vector2(2.5f, 2.5f), playerStatistic);

            TextureMgr.Instance.LoadIfNeeded("minerCharacter");
            player.texture = TextureMgr.Instance.GetTexture("minerCharacter");
            player.Initialize();
            gameObjects.Add(player);
            this.lastScene = lastScene;
            camera = new Camera(player);
            ui = new InGameUI(player);
            pauseMenu = CreatePauseMenu();
            canUnPause = true;
        }

        public bool PlayerPickedBooster()
        {
            this.isBoosterPicked = true;
            return true;
        }

        private PauseView CreatePauseMenu()
        {
            PauseView pauseView = new PauseView();
            pauseView.GetResumeButton().SetOnClick(ResumeGame);
            return pauseView;
        }
        public Scene(UIArea area, bool setSceneIsEnd = false)
        {
            isEnd = setSceneIsEnd;
            pauseMenu = area;
            isPaused = true;
            canUnPause = false;
            camera = new Camera(null);
            InitLists();
        }

        void InitLists()
        {
            gameObjects = new List<GameObject>();
            objectsToDelete = new List<GameObject>();
            objectsToAdd = new List<GameObject>();
            transitions = new List<GameObject>();
        }
        #region SCENE_OBJECTS_MANAGMENT_METHODS

        public void SetOnWin(Func<bool> functionOnWin)
        {
            this.OnPlayerWin += functionOnWin;
        }

        public void DecreaseHungerDropInterval()
        {
            this.player.ChangerHungerDecreaseIntervalBy(ConfigMgr.PlayerConfig.HungerDecreaseIntervalChangedByPercent);
        }

        public void SetEndGameButtonInPauseMenu(Func<bool> functionOnExitGame)
        {
            PauseView pV = (PauseView)this.pauseMenu;
            pV.GetEndGameButton().SetOnClick(functionOnExitGame);
            this.pauseMenu = pV;
        }

        public bool ResumeGame()
        {
            this.isPaused = false;
            return true;
        }

        public void SetLastScene(bool isLastScene)
        {
            this.lastScene = isLastScene;
        }

        public void AddTransition(GameObject obj)
        {
            transitions.Add(obj);
            objectsToAdd.Add(obj);
        }

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
            {
                obj.RemoveAllComponents();
                gameObjects.Remove(obj);
            }  

            objectsToDelete.Clear();
        }

        public List<GameObject> GameObjects
        {
            get { return gameObjects; }
        }

        #endregion

        public void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Input.KeyPressed(Keys.Escape))
            {
                if (isPaused && canUnPause)
                    isPaused = false;
                else
                    isPaused = true;
            }

            if (isPaused)
            {
                pauseMenu.Update(gameTime);
                return;
            }

            var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
            remainingShaderDelayInSeconds -= timer;

            if (remainingShaderDelayInSeconds <= 0)
            {
                remainingShaderDelayInSeconds = shaderDelayInSeconds;
                isBoosterPicked = false;
            }

            // Operation of add or remove from gameObjects list has to appear before updating gameObjects
            AddObjects();
            DeleteObjects();

            foreach (var obj in gameObjects)
                obj.Update(gameTime);

            camera.Update(gameTime);

            ui.Update(gameTime);

            DetectPlayerOnTransition();
        }

        public void Draw()
        {
            Drawer.Draw((spriteBatch, gameTime) =>
            {
                foreach (var obj in gameObjects)
                {
                    if (obj is Player)
                        continue;

                    obj.Draw(spriteBatch);
                }
            }, transformMatrix: camera.Transform);

            Drawer.Draw((spriteBatch, gameTime) =>
            {
                if (player != null)
                    player.Draw(spriteBatch);

                foreach(var obj in gameObjects)
                {
                    if (obj.layer == 6)
                        obj.Draw(spriteBatch);
                }

            }, transformMatrix: camera.Transform);

            if (isBoosterPicked && player != null)
            {
                Drawer.Shaders.DrawBoosterPickupShader((spriteBatch, gameTime) =>
                {
                    player.Draw(spriteBatch);
                }, transformMatrix: camera.Transform);
            }

            Drawer.Shaders.DrawBrightShader((spritebatch, gametime) =>
            {
                foreach (var obj in gameObjects)
                {
                    if (obj.layer == (int)LayerEnum.ShopPickables)
                    {
                        obj.Draw(spritebatch);
                    }

                }

            }, transformMatrix: camera.Transform);

            if (player != null)
            {
                Drawer.Shaders.DrawShadowEffect((spriteBatch, gameTime) =>
                {
                    foreach (var obj in gameObjects)
                        if (obj.layer == 3)
                            obj.Draw(spriteBatch);
                }, transformMatrix: camera.Transform, lightSource: player.position);
            }

            Drawer.Draw((spriteBatch, gameTime) =>
            {
                if (isPaused)
                    pauseMenu.Draw(spriteBatch);
                else
                    ui.Draw(spriteBatch);
            }, null);
        }

        private void DetectPlayerOnTransition()
        {
            foreach (var transition in transitions)
                if (player.GetTileWhereStanding() == transition.GetTileWhereStanding())
                {
                    foreach (var obj in gameObjects)
                        DeleteObject(obj);

                    foreach (var obj in transitions)
                        DeleteObject(obj);

                    foreach (var obj in objectsToAdd)
                        DeleteObject(obj);

                    DeleteObjects();
                    isEnd = true;
                    if (lastScene)
                    {
                        OnPlayerWin.Invoke();
                    }
                }
        }
    }
}

