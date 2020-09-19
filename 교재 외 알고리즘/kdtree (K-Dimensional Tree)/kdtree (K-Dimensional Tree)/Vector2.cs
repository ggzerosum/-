using System;

namespace kdtree__K_Dimensional_Tree_
{
    public struct Vector2
    {
        private double _x, _y;
        public double X => _x;
        public double Y => _y;

        public Vector2(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public double Magnitude
        {
            get
            {
                double sqrtMagnitude = X * X + Y * Y;
                if (sqrtMagnitude < double.Epsilon)
                    return 0;

                return Math.Sqrt(sqrtMagnitude);
            }
        }
        public double SqrtMagnitude
        {
            get
            {
                double sqrtMagnitude = X * X + Y * Y;
                if (sqrtMagnitude < double.Epsilon)
                    return 0;

                return sqrtMagnitude;
            }
        }

        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }
        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }
        public static Vector2 operator *(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X * rhs.X, lhs.Y * rhs.Y);
        }
        public static Vector2 operator /(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X / rhs.X, lhs.Y / rhs.Y);
        }

        public static bool operator ==(Vector2 lhs, Vector2 rhs)
        {
            return lhs.X.Equals(rhs.X) && lhs.Y.Equals(rhs.Y);
        }
        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return !lhs.X.Equals(rhs.X) || !lhs.Y.Equals(rhs.Y);
        }
    }
}
