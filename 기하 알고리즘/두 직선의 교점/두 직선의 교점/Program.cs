using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClippingAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"======== CCW ========");
            double positive = Geometry2D.CCW(new Vector2(0, 0.5), new Vector2(0, 0), new Vector2(1, 1));
            Console.WriteLine($"CCW Cross +: {positive}");

            double negative = Geometry2D.CCW(new Vector2(0.5, 0), new Vector2(0, 0), new Vector2(1, 1));
            Console.WriteLine($"CCW Cross -: {negative}");

            double parallel = Geometry2D.CCW(new Vector2(0.5, 0.5), new Vector2(0, 0), new Vector2(1, 1));
            Console.WriteLine($"CCW Cross 0: {parallel}");
            Console.WriteLine($"=====================");


            Console.WriteLine($"======== Line Intersection ========");
            Line a = new Line(new Vector2(0,0), new Vector2(10, 10) - new Vector2(0, 0));
            Line b = new Line(new Vector2(10,0), new Vector2(0, 10) - new Vector2(1, 0));

            Geometry2D.Intersection intersection = Geometry2D.GetLineIntersection(a, b);
            Console.WriteLine($"Is Parallel:{intersection.IsParallel}, Point:{intersection.IntersectPoint}");
            Console.WriteLine($"===================================");


            Console.WriteLine($"========== Cyrus-Beck Algorithm ===========");
            Geometry2D.SegmentIntersection segmentIntersection1 = Geometry2D.ClippingLineSegmentToEdge(
                new LineSegment(new Vector2(0,0), new Vector2(10,10)),
                new LineSegment(new Vector2(10,0), new Vector2(0, 10)));
            Console.WriteLine($"<시작점이 바깥에 있는 경우> State:{segmentIntersection1.State} Clip:{segmentIntersection1.Clip}");

            Geometry2D.SegmentIntersection segmentIntersection2 = Geometry2D.ClippingLineSegmentToEdge(
                new LineSegment(new Vector2(0, 0), new Vector2(10, 10)),
                new LineSegment(new Vector2(0, 10), new Vector2(10, 0)));
            Console.WriteLine($"<시작점이 안쪽에 있는 경우> State:{segmentIntersection2.State} Clip:{segmentIntersection2.Clip}");
            Console.WriteLine($"===========================================");

            Console.WriteLine($"========== 사각형의 넓이 구하기 ===========");
            double boxArea = Geometry2D.Area(new[]
            {
                new Vector2(0,0),
                new Vector2(10,0),
                new Vector2(10,10),
                new Vector2(0,10)
            });
            Console.WriteLine($"사각형의 넓이(가로:{10},세로:{10}) , {boxArea}");

            double triArea = Geometry2D.Area(new[]
            {
                new Vector2(0,0),
                new Vector2(10,0),
                new Vector2(5,10)
            });
            Console.WriteLine($"삼각형의 넓이(가로:{10},세로:{10}) , {triArea}");
            Console.WriteLine($"===========================================");
        }
    }

    public struct Vector2
    {
        public double X;
        public double Y;

        public static readonly Vector2 Zero = new Vector2(0d,0d);

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

    public struct Line
    {
        public readonly Vector2 InPoint;
        public readonly Vector2 Direction;

        public Line(Vector2 inPoint, Vector2 direction)
        {
            InPoint = inPoint;
            Direction = direction.Normalize();
        }
    }
    public struct LineSegment
    {
        public readonly Vector2 Start;
        public readonly Vector2 End;
        public readonly Vector2 Direction;
        /// <summary>
        /// 2차원 유사 외적을 사용하여 구하는 선분의 노멀방향. 오른손 법칙으로 구해진다.
        /// </summary>
        public readonly Vector2 Normal;
        public double Length => Direction.Magnitude();

        public LineSegment(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
            Direction = End - Start;

            Normal = Vector2.Pseudo2dCross(Direction);
        }

        public override string ToString()
        {
            return $"LineSegment: [{Start} {End}]";
        }
    }

    public class Geometry2D
    {
        public static double CCW(Vector2 point, Vector2 a, Vector2 b)
        {
            return Vector2.Cross(b - a, point);
        }

        public struct Intersection
        {
            public Intersection(bool isParallel, Vector2 intersectPoint)
            {
                IsParallel = isParallel;
                IntersectPoint = intersectPoint;
            }

            public bool IsParallel;
            public Vector2 IntersectPoint;
        }
        public static Intersection GetLineIntersection(Line line1, Line line2)
        {
            // Doc 참조할 것.
            // (line2 - line1) x (line2Direction) / line1Direction x line2Direction
            double determinant = Vector2.Cross(line1.Direction, line2.Direction);

            // 두 직선이 평행한 경우이다.
            if (UMath.IsZero(determinant))
                return new Intersection(true, Vector2.Zero);

            double p = Vector2.Cross(line2.InPoint - line1.InPoint, line2.Direction) / determinant;
            Vector2 intersection = line1.InPoint + p * line1.Direction;
            return new Intersection(false, intersection);
        }

        public struct SegmentIntersection
        {
            public enum BooleanState
            {
                Inside = 0,
                Outside = 1,
                Both = 2
            }

            public BooleanState State;
            public LineSegment Clip;

            public SegmentIntersection(BooleanState state, LineSegment clip)
            {
                State = state;
                Clip = clip;
            }
        }
        public static SegmentIntersection ClippingLineSegmentToEdge(LineSegment edge, LineSegment segment)
        {
            // Implementation of Cyrus–Beck algorithm
            // 선분 A와 선분 B가 겹친다고 할 때,
            // 선분 A의 내부 점에서 선분 B와의 교점으로 향하는 벡터와 선분 A의 노멀 벡터가 이루는 내적 값이 0이 된다는 원리를 활용하면 깔끔한 알고리즘을 작성할 수 있다.
            Vector2 s = edge.Start;
            double dot1 = Vector2.Dot(segment.Start - s, edge.Normal);
            double dot2 = Vector2.Dot(segment.End - s, edge.Normal);

            if (dot1 < 0 && dot2 < 0) // 두 벡터 모두 내적이 음수 = 모든 점이 밑쪽에 있다 = 점들이 바깥에 있다.
                return new SegmentIntersection(SegmentIntersection.BooleanState.Outside, segment);
            if (dot1 >= 0 && dot2 >= 0) // 두 벡터 모두 내적이 양수 = 모든 점이 위쪽에 있다 = 점들이 안쪽에 있다.
                return new SegmentIntersection(SegmentIntersection.BooleanState.Inside, segment);

            // 선분의 시작,끝점이 모두 안쪽에 있거나 모두 바깥에 있는 경우를 위에서 처리했으므로
            // 여기서부터는 두 점이 서로 다른 HalfSpace에 위치해있는 경우를 처리해야한다. (하나가 안쪽에 있으면 하나가 바깥쪽에 있다.)
            double t = dot1 / (dot1 - dot2);
            Vector2 intersection = segment.Start + t * segment.Direction;
            // 교차점에서 안쪽으로 위치한 선분을 선택할 것이냐, 교차점에서 바깥쪽으로 위치한 선분을 선택할 것이냐를 여기서 정할 수 있다.
            // 이 함수는 교차점 안쪽의 선분을 선택하도록 할 것이다.
            if (dot1 < 0) // 선분의 시작점이 바깥에 있을 경우
            {
                // 선분의 끝점이 안쪽에 있다는 말이므로, 시작점을 교차점으로 옮겨야한다.
                return new SegmentIntersection(SegmentIntersection.BooleanState.Both, new LineSegment(intersection, segment.End));
            }
            else
            {
                // 선분의 시작점이 안쪽에 있다는 말이므로, 끝점을 교차점으로 옮겨야한다.
                return new SegmentIntersection(SegmentIntersection.BooleanState.Both, new LineSegment(segment.Start, intersection));
            }
        }


        public static double Area(Vector2[] polygon)
        {
            double area = 0d;
            for (int i = 0; i < polygon.Length; i++)
            {
                int j = (i + 1) % polygon.Length;
                // 2차원 평면상에서 사선공식 = 외적이고, 외적을 살펴보면
                // 폴리곤 내 모든 선분의 시작점과 끝점의 외적을 더한 값과 같다.
                area += (polygon[i].X * polygon[j].Y) - (polygon[j].X * polygon[i].Y);
            }

            return Math.Abs(area * 0.5d);
        }
    }

    public class UMath
    {
        public static bool IsZero(double value)
        {
            return Math.Abs(value) < double.Epsilon;
        }
    }
}