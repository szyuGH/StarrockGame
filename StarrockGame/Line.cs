using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame
{
    public class Line
    {
        private static BasicEffect _basicEffect;
        private static GraphicsDevice _device;
        private static Matrix _projection;

        public List<VertexPositionColor> Vertices;

        public static void Initialize(GraphicsDevice device)
        {
            _device = device;
            _projection = Matrix.CreateOrthographicOffCenter(0, device.Viewport.Width, device.Viewport.Height, 0, 0, 1);
            _basicEffect = new BasicEffect(device);
            _basicEffect.DiffuseColor = new Vector3(1f, 1f, 1f);
            _basicEffect.Projection = _projection;
        }

        public Line(params VertexPositionColor[] vertices)
        {
            Vertices = vertices.ToList();
        }

        public void Render(Camera2D cam)
        {
            _basicEffect.View = cam.Translation;
            _basicEffect.CurrentTechnique.Passes[0].Apply(); ;

            _device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, Vertices.ToArray(), 0, (int)(Vertices.Count - 1));

        }
    }
}
