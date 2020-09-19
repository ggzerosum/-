using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdtree__K_Dimensional_Tree_
{
    /// <summary>
    /// KdTree의 2차원 평면 버전
    /// <link>https://www.geeksforgeeks.org/k-dimensional-tree/</link>
    /// </summary>
    public class TwoDimensionalBinaryTree : KdTree<Vector2>
    {
        private const int k_dimension = 2;
        private Point _root = null;

        public override void Insert(Vector2 data)
        {
            if (_root == null)
            {
                _root = CreateNode(data);
                return;
            }

            Insert_Internal(_root, data, 0);
        }
        private Node<Vector2> Insert_Internal(Node<Vector2> current, Vector2 point, int depth)
        {
            if (current == null)
                return new Point(point);

            bool divideHorizontal = IsDividedHorizontally(depth);
            double nodeValue = GetDimensionValue(current.Data, divideHorizontal);
            double pointValue = GetDimensionValue(point, divideHorizontal);

            if (pointValue > nodeValue)
            {
                current.Right = Insert_Internal(current.Right, point, depth + 1);
            }
            else
            {
                current.Left = Insert_Internal(current.Left, point, depth + 1);
            }

            return current;
        }

        public override bool Search(Vector2 data)
        {
            if (_root == null)
                throw new NullReferenceException("Root Data Not Exist");

            return Search_Internal(_root, data, 0);
        }
        private bool Search_Internal(Node<Vector2> node, Vector2 data, int depth)
        {
            if (node == null)
                return false;

            if (node.Data == data)
                return true;

            bool divideHorizontal = IsDividedHorizontally(depth);
            double nodeValue = GetDimensionValue(node.Data, divideHorizontal);
            double dataValue = GetDimensionValue(data, divideHorizontal);

            if (dataValue > nodeValue)
            {
                return Search_Internal(node.Right, data, depth + 1);
            }
            else
            {
                return Search_Internal(node.Left, data, depth + 1);
            }
        }

        private double GetDimensionValue(Vector2 vector2, bool horizontal)
        {
            return horizontal ? vector2.X : vector2.Y;
        }
        private bool IsDividedHorizontally(int depth)
        {
            int currentDimension = depth % k_dimension;
            bool divideHorizontal = currentDimension == 0;
            return divideHorizontal;
        }

        internal IEnumerable<Vector2> TraverseData()
        {
            Queue<Node<Vector2>> queue = new Queue<Node<Vector2>>();
            queue.Enqueue(_root);
            while (queue.Count != 0)
            {
                Node<Vector2> currnet = queue.Dequeue();
                if (currnet.Left != null)
                    queue.Enqueue(currnet.Left);
                if (currnet.Right != null)
                    queue.Enqueue(currnet.Right);

                yield return currnet.Data;
            }
        }

        private Point CreateNode(Vector2 point)
        {
            return new Point(point);
        }
    }
    internal class Point : Node<Vector2>
    {
        public Point(Vector2 data) : base(data)
        { }
    }


    // Abstract
    public abstract class KdTree<TData>
    {
        public abstract void Insert(TData data);
        public abstract bool Search(TData data);
    }
    internal abstract class Node<TData>
    {
        private TData _data;
        public TData Data => _data;
        private Node<TData> _left;
        public Node<TData> Left
        {
            get => _left;
            set => _left = value;
        }

        private Node<TData> _right;
        public Node<TData> Right
        {
            get => _right;
            set => _right = value;
        }

        public Node(TData data)
        {
            _data = data;
        }
    }
}