﻿using Microsoft.Xna.Framework.Graphics;
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
using FarseerPhysics.Common;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Dynamics.Contacts;
using StarrockGame.Caching;
using StarrockGame.AI;
using TData.TemplateData;
using GPart;
using StarrockGame.ParticleSystems;

namespace StarrockGame.Entities
{
    public abstract class Entity
    {
        public string Type { get; private set; }

        public Body Body { get; private set; }

        private float _structure;
        public float Structure
        {
            get { return _structure; }
            set { _structure = MathHelper.Clamp(value, 0, Template.Structure); }
        }

        public bool IsAlive { get; private set; }
        public Vector2 Direction {
            get { return new Vector2((float)Math.Cos(Body.Rotation), (float)Math.Sin(Body.Rotation)); }
        }

        public IController Controller { get; private set; }

        public EntityTemplate Template { get; private set; }
        protected Texture2D Graphic { get; private set; }
        protected Vector2 Center { get; set; }
        protected float Scale { get; private set; }

        protected float DrawOrder = 1;

        private World _world;
        public World World { get; private set; }

        public Entity Target;

        public Entity(World world, string type)
        {
            World = world;
            Type = type;
            Template = LoadTemplate(type);
            Graphic = Cache.LoadGraphic(Template.TextureName);
            CreateBody(world);
        }

        protected abstract EntityTemplate LoadTemplate(string type);

        public virtual void Initialize<T>(Vector2 position, float rotation, Vector2 initialVelocity, float initialAngularVelocity = 0)
            where T : IController
        {
            Controller = Activator.CreateInstance<T>();

            Body.Enabled = true;
            Body.Position = ConvertUnits.ToSimUnits(position);
            Body.Rotation = rotation;
            Body.LinearVelocity = initialVelocity;
            Body.AngularVelocity = initialAngularVelocity;
            Body.Mass = Template.Mass;
            Body.Restitution = 0.5f;
            Body.Inertia = Template.Inertia;

            Structure = Template.Structure;
            IsAlive = true;
        }
        

        private void CreateBody(World world)
        {

            //Create an array to hold the data from the texture
            uint[] data = new uint[Graphic.Width * Graphic.Height];

            //Transfer the texture data to the array
            Graphic.GetData(data);

            //Find the vertices that makes up the outline of the shape in the texture
            Vertices textureVertices = PolygonTools.CreatePolygon(data, Graphic.Width, false);

            //The tool return vertices as they were found in the texture.
            //We need to find the real center (centroid) of the vertices for 2 reasons:

            //1. To translate the vertices so the polygon is centered around the centroid.
            Vector2 centroid = -textureVertices.GetCentroid();
            textureVertices.Translate(ref centroid);

            //2. To draw the texture the correct place.
            Center = -centroid;

            //We simplify the vertices found in the texture.
            textureVertices = SimplifyTools.ReduceByDistance(textureVertices, 4f);

            //Since it is a concave polygon, we need to partition it into several smaller convex polygons
            List<Vertices> list = Triangulate.ConvexPartition(textureVertices, TriangulationAlgorithm.Bayazit);

            //Adjust the scale of the object for WP7's lower resolution
            Scale = Template.Size;

            //scale the vertices from graphics space to sim space
            Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1)) * Scale;
            foreach (Vertices vertices in list)
            {
                vertices.Scale(ref vertScale);
            }

            //Create a single body with multiple fixtures
            Body = BodyFactory.CreateCompoundPolygon(world, list, 1f, BodyType.Dynamic);
            Body.BodyType = BodyType.Dynamic;
            Body.AngularDamping = 2;
            Body.LinearDamping = 2;
            Body.Restitution = 0.2F;
            Body.UserData = this;
            Body.CollisionCategories = Category.None;
            Body.OnCollision += Body_OnCollision;
            Body.Enabled = false;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Structure <= 0)
            {
                Destroy();
                return;
            }
            Controller?.Act(this, gameTime);
            
        }

        public virtual void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Graphic,
                ConvertUnits.ToDisplayUnits(Body.Position),
                null,
                Color.White,
                Body.Rotation,
                Center,
                Scale,
                SpriteEffects.None,
                DrawOrder);
        }

        public virtual void Accelerate(float val, float elapsed)
        {
            Body.ApplyForce(Direction * val * elapsed * 50, Body.WorldCenter);
        }

        public virtual void Decelerate(float val, float elapsed)
        {
            Body.ApplyForce(-Direction * val * elapsed * 50, Body.WorldCenter);
        }

        public virtual void Rotate(float val,float elapsed)
        {
            Body.ApplyTorque(val * elapsed * 50);
        }

        public virtual void Destroy(bool ignoreScore=false)
        {
            if (!IsAlive)
                return;

            Structure = 0;
            Body.Enabled = false;
            IsAlive = false;
            if (!(Controller is PlayerController) && !ignoreScore)
            {
                SessionManager.Score += Template.Score;
            }
            if (Template.ExplosionSize > 0)
            {
                Particles.Emit<ExplosionParticleSystem>(ConvertUnits.ToDisplayUnits(Body.Position), Direction, Template.ExplosionSize);
            }
        }

        private bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            // apply collision response in form of applying damage
            HandleCollisionResponse(fixtureB.Body);
            return true;
        }

        /// <summary>
        /// Handles the collision with another entity. Here damage will be calculated.
        /// </summary>
        /// <param name="with">other entity</param>
        protected abstract void HandleCollisionResponse(Body with);
    }
}
