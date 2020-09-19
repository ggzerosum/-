using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdtree__K_Dimensional_Tree_
{
    class Program
    {
        static void Main(string[] args)
        {
            TextWriterTraceListener debugTracer = new TextWriterTraceListener(System.Console.Out);
            Debug.Listeners.Add(debugTracer);

            TwoDimensionalBinaryTree kdTree = new TwoDimensionalBinaryTree();
            Vector2[] vectors = new Vector2[]
            {
                new Vector2(3, 16),
                new Vector2(17, 15),
                new Vector2(13, 15),
                new Vector2(6, 12),
                new Vector2(9, 1),
                new Vector2(2, 7),
                new Vector2(10, 19)
            };

            foreach (Vector2 vector2 in vectors)
            {
                kdTree.Insert(vector2);
            }

            Debug.WriteLine("=== Traverse ===");
            foreach (Vector2 data in kdTree.TraverseData())
            {
                Debug.WriteLine($"({data.X},{data.Y})");
            }
            Debug.WriteLine("=== === ===");

            bool found = kdTree.Search(new Vector2(2, 12));
            if (found)
                Debug.WriteLine($"Found Point in kdTree");
            else
                Debug.WriteLine($"Couldn't find Point in kdTree");

            Trace.Flush();
        }
    }
}
