using GPart;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.AI;
using StarrockGame.Entities;
using StarrockGame.GUI;
using StarrockGame.InputManagement;
using StarrockGame.SceneManagement.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TData.TemplateData;

namespace StarrockGame.SceneManagement.Scenes
{
    public class SceneSession : Scene
    {
        Camera2D cam;
        IngameInterface ingameInterface;
        Background bg;

        public SceneSession(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            EntityManager.Clear();

            Line.Initialize(Device);
            EntityManager.Border = new GameBorder(EntityManager.World, Device, 2000, 2000);
            EntityManager.Add<Spaceship, PlayerController>(SessionManager.UsedShipTemplate.File, new Vector2(), 0, Vector2.Zero, 0, true);

            cam = new Camera2D(Device);
            cam.TrackingBody = EntityManager.PlayerShip.Body;
            cam.Zoom = (EntityManager.PlayerShip.Template as SpaceshipTemplate).CameraZoom;
            

            ingameInterface = new IngameInterface(Game.GraphicsDevice, EntityManager.PlayerShip as Spaceship);
            bg = new Background();
        }

        public override void Update(GameTime gameTime)
        {
            if (!EntityManager.PlayerShip.IsAlive)
            {
                SceneManager.CallPopup<PopupGameover>();
            }
            if (Input.Device.OpenMenu())
            {
                SceneManager.CallPopup<PopupPause>();
            }

            SessionManager.Update(gameTime);
            EntityManager.Update(gameTime);

            cam.Update();
            ingameInterface.Update();
        }

        public override void Render(GameTime gameTime)
        {
            base.Render(gameTime);
            bg.Render(SpriteBatch, gameTime, cam);


            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, cam.Translation);

            EntityManager.Render(SpriteBatch, gameTime);
            SpriteBatch.End();

            SpriteBatch.Begin();
            ingameInterface.Render(SpriteBatch);
            SpriteBatch.End();

            Particles.SetCamera(cam.Translation, cam.Projection, cam.Zoom);
            EntityManager.Border.Render(SpriteBatch, gameTime, cam);
        }
    }
}
