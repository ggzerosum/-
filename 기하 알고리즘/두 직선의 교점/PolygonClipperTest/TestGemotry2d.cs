using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProvisGames.PolygonClipper;
using Provisgames_PolygonClipper;

namespace PolygonClipperTest
{
    [TestClass]
    public class TestGemotry2d
    {
        [TestMethod]
        public void TestCCW()
        {
            Console.WriteLine($"======== CCW ========");
            double positive = Geometry2D.CCW(new Vector2(0, 0.5), new Vector2(0, 0), new Vector2(1, 1));
            Console.WriteLine($"CCW Cross +: {positive}");

            double negative = Geometry2D.CCW(new Vector2(0.5, 0), new Vector2(0, 0), new Vector2(1, 1));
            Console.WriteLine($"CCW Cross -: {negative}");

            double parallel = Geometry2D.CCW(new Vector2(0.5, 0.5), new Vector2(0, 0), new Vector2(1, 1));
            Console.WriteLine($"CCW Cross 0: {parallel}");
            Console.WriteLine($"=====================");
        }

        [TestMethod]
        public void TestLineIntersection()
        {
            Console.WriteLine($"======== Line Intersection ========");
            Line a = new Line(new Vector2(0, 0), new Vector2(10, 10) - new Vector2(0, 0));
            Line b = new Line(new Vector2(10, 0), new Vector2(0, 10) - new Vector2(1, 0));

            Geometry2D.Intersection intersection = Geometry2D.GetLineIntersection(a, b);
            Console.WriteLine($"Is Parallel:{intersection.IsParallel}, Point:{intersection.IntersectPoint}");
            Console.WriteLine($"===================================");
        }

        [TestMethod]
        public void TestCyrusBeck()
        {
            Console.WriteLine($"========== Cyrus-Beck Algorithm ===========");
            Geometry2D.SegmentIntersection segmentIntersection1 = Geometry2D.ClippingLineSegmentToEdge(
                new LineSegment(new Vector2(0, 0), new Vector2(10, 10)),
                new LineSegment(new Vector2(10, 0), new Vector2(0, 10)));
            Console.WriteLine($"<시작점이 바깥에 있는 경우> State:{segmentIntersection1.State} Clip:{segmentIntersection1.Clip}");

            Geometry2D.SegmentIntersection segmentIntersection2 = Geometry2D.ClippingLineSegmentToEdge(
                new LineSegment(new Vector2(0, 0), new Vector2(10, 10)),
                new LineSegment(new Vector2(0, 10), new Vector2(10, 0)));
            Console.WriteLine($"<시작점이 안쪽에 있는 경우> State:{segmentIntersection2.State} Clip:{segmentIntersection2.Clip}");
            Console.WriteLine($"===========================================");
        }

        [TestMethod]
        public void TestComputingPolygonArea()
        {
            Console.WriteLine($"========== 도형의 넓이 구하기 ===========");
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

        [TestMethod]
        public void TestClipPolygon()
        {
            Clipper2D clipper2d = new Clipper2D();
            List<Vector2> clipWindow = new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(10, 0),
                new Vector2(10, 10),
                new Vector2(0, 10)
            };
            List<Vector2> subjectPolygon = new List<Vector2>()
            {
                //new Vector2(5, 5),
                //new Vector2(20, 20),
                //new Vector2(5, 20)

                new Vector2(5, 5),
                new Vector2(20, 5),
                new Vector2(20, 20),
                new Vector2(5, 20)
            };
            List<Vector2> clipResult = new List<Vector2>(100);
            clipper2d.Clip(clipWindow, subjectPolygon, clipResult);

            int i = 0;
            foreach (Vector2 vector2 in clipResult)
            {
                Console.WriteLine($"{i}: {vector2}");
                i++;
            }
        }
    }
}