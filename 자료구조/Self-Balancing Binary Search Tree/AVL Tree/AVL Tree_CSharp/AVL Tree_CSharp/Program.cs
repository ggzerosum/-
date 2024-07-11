// See https://aka.ms/new-console-template for more information
using ggzerosum.Datastructure;

Console.WriteLine("=== start ===");

AVLTree_Sketch tree = new AVLTree_Sketch();
tree.Add(100);
tree.Add(50);
tree.Add(200);
tree.Add(20);
tree.Add(10);
tree.Add(8);
tree.Add(5);

string treeDescription = tree.PrintTree();
Console.WriteLine(treeDescription);