using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScalingClever
{
    public class ResolutionScaling
    {
        private static Matrix _scalingMatrix;
        public static Matrix ScalingMatrix
        {
            set
            {
                _scalingMatrix = value;
            }
            get
            {
                if (_scalingMatrix == null)
                {
                    _scalingMatrix = Matrix.CreateScale(1f, 1f, 1f);
                }
                return _scalingMatrix;
            }
        }
        private static float scalingPositionX = 1f;
        private static float scalingPositionY = 1f;

        private static Point _sourceResolution;
        private static Point _destinationResolution;
        //public static void Initialize(Point sourceResolution, Point destinationResolution)
        //{
        //    _sourceResolution = sourceResolution;
        //    _destinationResolution = destinationResolution;

        //    float sourceResolutionX = (float)_sourceResolution.X;
        //    float sourceResolutionY = (float)_sourceResolution.Y;

        //    float destinationResolutionX = destinationResolution.X;
        //    float destinationResolutionY = destinationResolution.Y;

        //    float scaleX = destinationResolutionX / sourceResolutionX;
        //    float scaleY = destinationResolutionY / sourceResolutionY;

        //    scalingPositionX = sourceResolutionX / destinationResolutionX;
        //    scalingPositionY = sourceResolutionY / destinationResolutionY;

        //    _scalingMatrix = Matrix.CreateScale(scaleX, scaleY, 1f);
        //}

        public static void Draw(Point sourceResolution,Point destinationResolution)
        {

            _sourceResolution = sourceResolution;
            _destinationResolution = destinationResolution;

            float sourceResolutionX = (float)_sourceResolution.X;
            float sourceResolutionY = (float)_sourceResolution.Y;

            float destinationResolutionX = destinationResolution.X;
            float destinationResolutionY = destinationResolution.Y;

            float scaleX = destinationResolutionX / sourceResolutionX;
            float scaleY = destinationResolutionY / sourceResolutionY;

            scalingPositionX = sourceResolutionX / destinationResolutionX;
            scalingPositionY = sourceResolutionY / destinationResolutionY;

            _scalingMatrix = Matrix.CreateScale(scaleX, scaleY, 1f);
        }

        public static Vector2 Position(Vector2 vector2)
        {
            return new Vector2(vector2.X * scalingPositionX, vector2.Y * scalingPositionY);
        }
        public static float X(float x)
        {
            return x * scalingPositionX;
        }
        public static float Y(float y)
        {
            return y * scalingPositionY;
        }
    }
}
