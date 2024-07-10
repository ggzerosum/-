using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProvisGames.PolygonClipper;

namespace ProvisGames.PolygonClipper
{
    internal class Geometry2D
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
            [Flags]
            public enum ClipState
            {
                BothOutside = 0b0000,
                StartInside = 0b0001,
                EndInside = 0b0010,
                BothInside = 0b0011
            }

            public ClipState State;
            public LineSegment Clip;
            public Vector2 Intersection;

            public SegmentIntersection(ClipState state, LineSegment clip, Vector2 intersection)
            {
                State = state;
                Clip = clip;
                Intersection = intersection;
            }

            public bool CheckState(ClipState state)
            {
                return (State & state) == state;
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
                return new SegmentIntersection(SegmentIntersection.ClipState.BothOutside, segment, Vector2.Zero);
            if (dot1 >= 0 && dot2 >= 0) // 두 벡터 모두 내적이 양수 = 모든 점이 위쪽에 있다 = 점들이 안쪽에 있다.
                return new SegmentIntersection(SegmentIntersection.ClipState.BothInside, segment, Vector2.Zero);

            // 선분의 시작,끝점이 모두 안쪽에 있거나 모두 바깥에 있는 경우를 위에서 처리했으므로
            // 여기서부터는 두 점이 서로 다른 HalfSpace에 위치해있는 경우를 처리해야한다. (하나가 안쪽에 있으면 하나가 바깥쪽에 있다.)
            double t = dot1 / (dot1 - dot2);
            Vector2 intersection = segment.Start + t * segment.Direction;
            // 교차점에서 안쪽으로 위치한 선분을 선택할 것이냐, 교차점에서 바깥쪽으로 위치한 선분을 선택할 것이냐를 여기서 정할 수 있다.
            // 이 함수는 교차점 안쪽의 선분을 선택하도록 할 것이다.
            if (dot1 < 0) // 선분의 시작점이 바깥에 있을 경우
            {
                // 선분의 끝점이 안쪽에 있다는 말이므로, 시작점을 교차점으로 옮겨야한다.
                return new SegmentIntersection(SegmentIntersection.ClipState.EndInside, new LineSegment(intersection, segment.End), intersection);
            }
            else
            {
                // 선분의 시작점이 안쪽에 있다는 말이므로, 끝점을 교차점으로 옮겨야한다.
                return new SegmentIntersection(SegmentIntersection.ClipState.StartInside, new LineSegment(segment.Start, intersection), intersection);
            }
        }

        public static double Area(IList<Vector2> polygon)
        {
            double area = 0d;
            for (int i = 0; i < polygon.Count; i++)
            {
                int j = (i + 1) % polygon.Count;
                // 2차원 평면상에서 사선공식 = 외적이고, 외적을 살펴보면
                // 폴리곤 내 모든 선분의 시작점과 끝점의 외적을 더한 값과 같다.
                area += (polygon[i].X * polygon[j].Y) - (polygon[j].X * polygon[i].Y);
            }

            return Math.Abs(area * 0.5d);
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
}