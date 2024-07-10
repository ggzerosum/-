using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1005번___ACM_Craft
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        class Solver
        {
            private ConstructionGraph _constructionGraph;

            public Solver(ConstructionGraph constructionGraph)
            {
                _constructionGraph = constructionGraph;
            }

            public void GetMinimumPath(int from, int to)
            {

            }
        }

        private class ConstructionGraph
        {
            private Dictionary<int, Building> _buildings;
            private int BuildingCount => _buildings.Count;

            public ConstructionGraph()
            {
                _buildings = new Dictionary<int, Building>();
            }

            public void AddBuilding(int id, Building building)
            {
                _buildings.Add(id, building);
            }
            public void SetConstructionBuild(int parent, int child)
            {
                _buildings[parent].AddChild(child);
            }
        }
        private class Building
        {
            private int _id;
            public int _constructionTime;

            private HashSet<int> _children;

            public Building(int id, int constructionTime)
            {
                _id = id;
                _constructionTime = constructionTime;
            }

            public void AddChild(int id)
            {
                _children.Add(id);
            }
        }
    }
}