using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using Microsoft.Xna.Framework;

namespace StarrockGame.Entities
{
    public class Entity
    {
        Texture2D image;
        public Body body;
        int health;

        public Entity(Texture2D texture, World world, float x, float y, float mass)
        {
            image = texture;
            body = BodyFactory.CreateRectangle(world,
            (float)ConvertUnits.ToSimUnits(image.Width),
            (float)ConvertUnits.ToSimUnits(image.Height), 1.5f);
    
            body.Mass = mass;
            body.BodyType = BodyType.Dynamic;
            body.Restitution = 1f;
            body.Friction = 0;
            body.Position = new Vector2(ConvertUnits.ToSimUnits(x), ConvertUnits.ToDisplayUnits(y));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, new Rectangle((int)ConvertUnits.ToDisplayUnits(body.Position.X),
            (int)ConvertUnits.ToDisplayUnits(body.Position.Y), image.Width, image.Height), Color.White);
        }


        //TODO: 550 && 450 durch wirkliche Fenstergröße ersetzen
        public bool isOnScreen()
        {
            if (ConvertUnits.ToDisplayUnits(body.Position.X) < 550
            && ConvertUnits.ToDisplayUnits(body.Position.X) + image.Width > 0)
                if (ConvertUnits.ToDisplayUnits(body.Position.Y) < 450
                && ConvertUnits.ToDisplayUnits(body.Position.Y) + image.Height > 0)
                {
                    return true;
                }
            return false;
        }
        public float XPosition
        {
            get {
                return body.Position.X;
            }
            set
                {
                if (isOnScreen())
                {
                    XPosition = value;
                }
            }
        }
        public float YPosition
        {
            get
            {
                return body.Position.Y;
            }
            set
            {
                if (isOnScreen())
                {
                    XPosition = value;
                }
            }
        }
    }
}
