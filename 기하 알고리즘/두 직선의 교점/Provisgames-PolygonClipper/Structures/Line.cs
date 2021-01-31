namespace ProvisGames.PolygonClipper
{
    internal struct Line
    {
        public readonly Vector2 InPoint;
        public readonly Vector2 Direction;

        public Line(Vector2 inPoint, Vector2 direction)
        {
            InPoint = inPoint;
            Direction = direction.Normalize();
        }
    }

    internal struct LineSegment
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
}