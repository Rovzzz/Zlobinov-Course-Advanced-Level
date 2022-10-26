using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryTree;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            IBinaryTree<int> tree =
                Tree<int>.BuildTree<int>(1, new int[] { 4, 7, 3, 4, 5 });
            Console.WriteLine("Текущее дерево: ");
            tree.WalkTree();
            Console.WriteLine("Добавить 15\n");
            tree.Add(15);
            Console.WriteLine("Текущее дерево: ");
            tree.WalkTree();
            Console.WriteLine("Удалить 5\n");
            tree.Remove(5);
            Console.WriteLine("Текущее дерево: ");
            tree.WalkTree();
            Console.ReadLine();
        }
    }
}
