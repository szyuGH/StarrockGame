using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame
{
    public class Camera2D
    {
        private GraphicsDevice _device;

        
        private Vector2 _position;
        private float _rotation;
        private Body _trackingBody;
        public Vector2 _translateCenter;

        public Vector2 Position
        {
            get { return ConvertUnits.ToDisplayUnits(_position); }
            set { _position = ConvertUnits.ToSimUnits(value); }
        }
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value % MathHelper.TwoPi;
            }
        }
        public Body TrackingBody
        {
            get { return _trackingBody; }
            set
            {
                _trackingBody = value;
            }
        }

        public bool EnableTrackingRotation;

        public Matrix Translation
        {
            get
            {
                return Matrix.CreateTranslation(-Position.X, -Position.Y, 0)
                    * Matrix.CreateScale(1, 1, 1)
                    * Matrix.CreateRotationZ(_rotation)
                    * Matrix.CreateTranslation(_translateCenter.X, _translateCenter.Y, 0);
            }
        }
        public Matrix Projection;
        public Matrix SimView
        {
            get
            {
                return Matrix.CreateTranslation(ConvertUnits.ToSimUnits(-Position.X), ConvertUnits.ToSimUnits(-Position.Y), 0)
                    * Matrix.CreateScale(1, 1, 1)
                    * Matrix.CreateRotationZ(_rotation)
                    * Matrix.CreateTranslation(ConvertUnits.ToSimUnits(_translateCenter.X), ConvertUnits.ToSimUnits(_translateCenter.Y), 0);
            }
        }

        public Camera2D(GraphicsDevice device)
        {
            _device = device;
            _translateCenter = new Vector2(device.Viewport.Width * 0.5f, device.Viewport.Height * 0.5f);
            Projection = Matrix.CreateOrthographicOffCenter(0, device.Viewport.Width, device.Viewport.Height, 0, -1, 1);

            ResetCamera();
        }

        public void MoveCamera(Vector2 amount)
        {
            _position += amount;
        }
        public void RotateCamera(float amount)
        {
            _rotation += amount;
        }

        public void ResetCamera()
        {
            _trackingBody = null;
            _position = Vector2.Zero;
            _rotation = 0;
            EnableTrackingRotation = false;
        }

        public void Update()
        {
            if (_trackingBody != null)
            {
                _position = _trackingBody.Position;
                if (EnableTrackingRotation)
                    Rotation = _trackingBody.Rotation;
            }
        }

        public Vector2 ConvertToWorld(Vector2 inp)
        {
            Matrix translate = Matrix.Invert(Translation);
            return Vector2.Transform(inp, translate);
        }

    }
}
