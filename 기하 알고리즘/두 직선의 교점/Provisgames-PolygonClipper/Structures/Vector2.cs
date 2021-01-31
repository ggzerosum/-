using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvisGames.PolygonClipper
{
    public struct Vector2
    {
        public double X;
        public double Y;

        public static readonly Vector2 Zero = new Vector2(0d, 0d);

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector2 Normalize()
        {
            return Normalize(this);
        }

        public double SqrtMagnitude()
        {
            return X * X + Y * Y;
        }

        public double Magnitude()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public override string ToString()
        {
            return $"Vector2 ({X}, {Y})";
        }

        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }
        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }
        public static Vector2 operator *(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X * rhs.X, lhs.Y * rhs.Y);
        }
        public static Vector2 operator *(double lhs, Vector2 rhs)
        {
            return new Vector2(lhs * rhs.X, lhs * rhs.Y);
        }
        public static Vector2 operator *(Vector2 lhs, double rhs)
        {
            return new Vector2(lhs.X * rhs, lhs.Y * rhs);
        }

        public static Vector2 operator /(Vector2 lhs, double rhs)
        {
            return new Vector2(lhs.X / rhs, lhs.Y / rhs);
        }

        public static bool operator ==(Vector2 lhs, Vector2 rhs)
        {
            return lhs.X.Equals(rhs.X) && lhs.Y.Equals(rhs.Y);
        }
        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return !lhs.X.Equals(rhs.X) || !lhs.Y.Equals(rhs.Y);
        }

        public static Vector2 Normalize(Vector2 a)
        {
            double mag = a.Magnitude();
            if (mag > double.Epsilon)
                return a / mag;
            else
                return Zero;
        }

        public static double Dot(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
        public static double Cross(Vector2 lhs, Vector2 rhs)
        {
            return lhs.X * rhs.Y - rhs.X * lhs.Y;
        }

        /// <summary>
        /// 2차원 유사 외적은 2차원 공간 상 하나의 벡터에 수직인 벡터를 구할 수 있다.
        /// 방향 판별에 유의해야한다. 유니티와 다르게 이 함수는 오른손 법칙을 사용한다.
        /// </summary>
        public static Vector2 Pseudo2dCross(Vector2 v)
        {
            return new Vector2(-v.Y, v.X);
        }
    }
}
