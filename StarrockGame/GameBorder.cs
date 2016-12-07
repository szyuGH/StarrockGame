using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;

namespace StarrockGame
{
    public class GameBorder
    {
        Body _anchor;
        public int Width, Height;
        public Vector2 Center;
        List<Line> _lines;

        public GameBorder(World world, GraphicsDevice device, int wres = 1024, int hres = 1024)
        {
            Center = new Vector2(0, 0);
            _lines = new List<Line>();

            ChangeRestriction(world, wres, hres);
        }

        public void ChangeRestriction(World world, int wres, int hres)
        {
            Width = wres;
            Height = hres;

            Vertices borders = new Vertices(8);/*
            borders.Add(ConvertUnits.ToSimUnits(new Vector2(0,0)));
            borders.Add(ConvertUnits.ToSimUnits(new Vector2(Width,0)));
            borders.Add(ConvertUnits.ToSimUnits(new Vector2(Width,Height)));
            borders.Add(ConvertUnits.ToSimUnits(new Vector2(0,Height)));
            */

            borders.Add(ConvertUnits.ToSimUnits(Center + 0.5f * new Vector2(-Width, -4 * Height / 3)));
            borders.Add(ConvertUnits.ToSimUnits(Center + 0.5f * new Vector2(Width, -4 * Height / 3)));
            borders.Add(ConvertUnits.ToSimUnits(Center + 0.5f * new Vector2(4 * Width / 3, -Height)));
            borders.Add(ConvertUnits.ToSimUnits(Center + 0.5f * new Vector2(4 * Width / 3, Height)));

            borders.Add(ConvertUnits.ToSimUnits(Center + 0.5f * new Vector2(Width, 4 * Height / 3)));
            borders.Add(ConvertUnits.ToSimUnits(Center + 0.5f * new Vector2(-Width, 4 * Height / 3)));
            borders.Add(ConvertUnits.ToSimUnits(Center + 0.5f * new Vector2(-4 * Width / 3, Height)));
            borders.Add(ConvertUnits.ToSimUnits(Center + 0.5f * new Vector2(-4 * Width / 3, -Height)));


            /*for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    borders.Add(ConvertUnits.ToSimUnits(new Vector2(x*(Width/3f),y*(Height/1f))));
                }
            }*/
            _anchor = BodyFactory.CreateLoopShape(world, borders);
            _anchor.UserData = this;



            _lines = new List<Line>();
            List<VertexPositionColor> verts = new List<VertexPositionColor>();
            if (_anchor.FixtureList[0].Shape is PolygonShape)
            {
                PolygonShape psh = (PolygonShape)_anchor.FixtureList[0].Shape;

                foreach (Vector2 pos in psh.Vertices)
                {
                    verts.Add(new VertexPositionColor(new Vector3(ConvertUnits.ToDisplayUnits(pos), 0), Color.White));
                }
            }
            else if (_anchor.FixtureList[0].Shape is ChainShape)
            {
                ChainShape psh = (ChainShape)_anchor.FixtureList[0].Shape;

                foreach (Vector2 pos in psh.Vertices)
                {
                    verts.Add(new VertexPositionColor(new Vector3(ConvertUnits.ToDisplayUnits(pos), 0), Color.White));
                }

            }
            verts.Add(verts[0]);
            _lines.Add(new Line(verts.ToArray()));
        }

        public void Render(SpriteBatch bath, GameTime gameTime, Camera2D cam)
        {
            foreach (Line line in _lines)
                line.Render(cam);
        }
    }
}
