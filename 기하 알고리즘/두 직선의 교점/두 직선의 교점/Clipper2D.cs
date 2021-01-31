using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClippingAlgorithm
{
    public class Clipper2D
    {
        List<Vector2> _input = new List<Vector2>(100);
        List<Vector2> _output = new List<Vector2>(100);

        /// <summary>
        /// 시계방향으로 순회하며 다각형을 clipPolygon에 맞춰서 잘라냅니다.
        /// </summary>
        public void GetInersection(List<Vector2> clipPolygon, List<Vector2> subjectPolygon, in List<Vector2> clipResult)
        {
            CopyList(ref subjectPolygon, ref _output);
            foreach (LineSegment clipEdge in IterateSegmentCCW(clipPolygon))
            {
                CopyList(ref _output, ref _input);
                _output.Clear();

                foreach (LineSegment lineSegment in IterateSegmentCCW(_input))
                {
                    Geometry2D.SegmentIntersection intersection = Geometry2D.ClippingLineSegmentToEdge(clipEdge, lineSegment);

                    // 다각형의 순회는 시작점으로 다시 돌아오는 순회이므로,
                    // 시작점은 순회할 때 항상 만나게된다. 따라서, 다음 시작점(현재의 끝점)의 위치가 변하지 않는다면(다각형 안에 존재하는 점일 경우) 다음 순회 때
                    // 방문하므로 항상 Start가 안에 있을 때만 추가하면 된다.
                    if (intersection.CheckState(Geometry2D.SegmentIntersection.BooleanState.StartInside))
                    {
                        _output.Add(intersection.Clip.Start);

                        // 이게 교차점을 구하다보면, 시작지점과 끝점이 같아질 수가 있다.
                        // 가령, 시작지점은 선위에 걸쳐져있는데, 선이 서로 수직인 경우가 있을 수 있다.
                        if (intersection.CheckState(Geometry2D.SegmentIntersection.BooleanState.EndInside) == false)
                        {
                            if (intersection.Clip.Start != intersection.Intersection)
                                _output.Add(intersection.Intersection);
                        }
                    }
                    else if (intersection.CheckState(Geometry2D.SegmentIntersection.BooleanState.EndInside))
                    {
                        _output.Add(intersection.Intersection);
                    }
                }
            }

            // 결과값 반환
            foreach (Vector2 vertex in _output)
            {
                clipResult.Add(vertex);
            }
        }

        IEnumerable<LineSegment> IterateSegmentCCW(List<Vector2> subjectPolygon)
        {
            for (int start = 0; start < subjectPolygon.Count; start++)
            {
                int end = (start + 1) % subjectPolygon.Count;
                yield return new LineSegment(subjectPolygon[start], subjectPolygon[end]);
            }
        }

        private void CopyList(ref List<Vector2> from, ref List<Vector2> to)
        {
            if (to == null)
                to = new List<Vector2>(from.Count);

            if (to.Capacity < from.Count)
            {
                to = new List<Vector2>(from.Count);
            }

            to.Clear();
            for (int i = 0; i < from.Count; i++)
            {
                to.Add(from[i]);
            }
        }
    }
}