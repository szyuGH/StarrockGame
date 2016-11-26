using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.SceneManagement
{
    public static class SceneManager
    {
        private static Game1 game;
        private static Stack<Scene> sceneStack;
        private static Scene currentScene;
        private static Scene nextScene;

        public static void Initialize<T>(Game1 g) where T : Scene
        {
            game = g;
            sceneStack = new Stack<Scene>();
            Call<T>();
        }

        public static void Dispose()
        {
            sceneStack.Clear();
            sceneStack = null;
            currentScene = null;
            nextScene = null;
        }

        /// <summary>
        /// Update the current scene and handle transitions
        /// </summary>
        /// <param name="elapsed"></param>
        public static void Update(float elapsed)
        {
            // if there is no current scene, the game cannot run and will be exited
            if (currentScene != null)
            {
                // call the corresponding update method of the current scene based on the state
                switch (currentScene.State)
                {
                    case SceneState.FadingIn:
                        currentScene.UpdateFade(elapsed);
                        break;
                    case SceneState.FadingOut:
                        currentScene.UpdateFade(elapsed);
                        // if fadeout is complete, set the next scene
                        if (currentScene.State == SceneState.Closed)
                        {
                            currentScene = nextScene;
                            currentScene.State = SceneState.FadingIn;
                            nextScene = null;
                        }
                        break;
                    default:
                        currentScene.Update(elapsed);
                        break;
                }
            } else
            {
                Exit();
            }
        }

        /// <summary>
        /// Render the current scene
        /// </summary>
        public static void Render(GameTime gameTime)
        {
            if (currentScene.State == SceneState.FadingIn || currentScene.State == SceneState.FadingOut)
                currentScene.RenderFade(gameTime);
            else
                currentScene.Render(gameTime);
        }

        /// <summary>
        /// Call a new scene and push the current scene to the stack
        /// </summary>
        /// <typeparam name="T">Type of the new scene</typeparam>
        public static void Call<T>() where T : Scene
        {
            if (currentScene != null)
            {
                sceneStack.Push(currentScene);
                currentScene.State = SceneState.FadingOut;
                nextScene = (Scene)Activator.CreateInstance(typeof(T), game);
            }
            else
            {
                currentScene = (Scene)Activator.CreateInstance(typeof(T), game);
                currentScene.State = SceneState.FadingIn;
            }
        }

        /// <summary>
        /// Set a new scene without pushing the current scene to the stack
        /// </summary>
        /// <typeparam name="T">Type of the new scene</typeparam>
        public static void Set<T>() where T : Scene
        {
            if (currentScene != null)
            {
                currentScene.State = SceneState.FadingOut;
                nextScene = (Scene)Activator.CreateInstance(typeof(T), game);
            } else
            {
                currentScene = (Scene)Activator.CreateInstance(typeof(T), game);
                currentScene.State = SceneState.FadingIn;
            }
        }

        /// <summary>
        /// Return to the last scene on the stack
        /// </summary>
        public static void Return()
        {
            if (sceneStack.Count > 0)
            {
                nextScene = sceneStack.Pop();
                currentScene.State = SceneState.FadingOut;
            }
        }

        /// <summary>
        /// Exit the game
        /// </summary>
        public static void Exit()
        {
            game.Exit();
        }
    }
}
