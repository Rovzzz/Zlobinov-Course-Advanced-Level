using System;

namespace BinaryTree
{
    public class Tree<TItem> : IBinaryTree<TItem>
        where TItem : IComparable<TItem>
    {
        public TItem NodeData { get; set; }

        public Tree<TItem> LeftTree { get; set; }

        public Tree<TItem> RightTree { get; set; }

        public Tree(TItem nodeValue)
        {
            this.NodeData = nodeValue;
            this.LeftTree = null;
            this.RightTree = null;
        }

        public void Add(TItem newItem)
        {
            // Получите значение текущего узла
            TItem currentNodeValue = this.NodeData;

            // Проверьте, должен ли элемент быть вставлен в левое дерево.
            if (currentNodeValue.CompareTo(newItem) > 0)
            {
                // Является ли левое дерево нулевым?
                if (this.LeftTree == null)
                {
                    this.LeftTree = new Tree<TItem>(newItem);
                }
                else // Вызовите метод Add рекурсивно.
                {
                    this.LeftTree.Add(newItem);
                }
            }
            else // Вставьте в правое дерево.
            {
                // Является ли правильное дерево нулевым?
                if (this.RightTree == null)
                {
                    this.RightTree = new Tree<TItem>(newItem);
                }
                else // Вызовите метод Add рекурсивно.
                {
                    this.RightTree.Add(newItem);
                }
            }
        }

        public void WalkTree()
        {
            // Рекурсивный спуск по левому дереву.
            if (this.LeftTree != null)
            {
                this.LeftTree.WalkTree();
            }

            Console.WriteLine(this.NodeData.ToString());

            // Рекурсивный спуск по правому дереву.
            if (this.RightTree != null)
            {
                this.RightTree.WalkTree();
            }
        }

        public void Remove(TItem itemToRemove)
        {

            // Невозможно удалить значение null.
            if (itemToRemove == null)
            {
                return;
            }

            // Проверьте, может ли элемент находиться в левом дереве.
            if (this.NodeData.CompareTo(itemToRemove) > 0 && this.LeftTree != null)
            {
                // Проверьте левое дерево.
                // Проверьте 2 уровня вниз по дереву - невозможно удалить
                // 'this', только свойства левого дерева или правого дерева.
                if (this.LeftTree.NodeData.CompareTo(itemToRemove) == 0)
                {
                    // Свойство LeftTree не имеет дочерних элементов - установите для свойства LeftTree значение null.
                    if (this.LeftTree.LeftTree == null && this.LeftTree.RightTree == null)
                    {
                        this.LeftTree = null;
                    }
                    else // Удалите свойство LeftTree.
                    {
                        RemoveNodeWithChildren(this.LeftTree);
                    }
                }
                else
                {
                    // Продолжайте искать - вызовите Remove рекурсивно.
                    this.LeftTree.Remove(itemToRemove);
                }
            }

            // Проверьте, может ли элемент находиться в нужном дереве.
            if (this.NodeData.CompareTo(itemToRemove) < 0 && this.RightTree != null)
            {
                // Проверьте правильное дерево.
                // Проверьте 2 уровня вниз по дереву - невозможно удалить
                // "this", только свойства левого дерева или правого дерева.
                if (this.RightTree.NodeData.CompareTo(itemToRemove) == 0)
                {
                    // Правое дерево не имеет дочерних элементов - установите значение righttree равным null.
                    if (this.RightTree.LeftTree == null && this.RightTree.RightTree == null)
                    {
                        this.RightTree = null;
                    }
                    else // Удалите свойство RightTree.
                    {
                        RemoveNodeWithChildren(this.RightTree);
                    }
                }
                else
                {
                    // Продолжайте искать - вызовите Remove рекурсивно.
                    this.RightTree.Remove(itemToRemove);
                }
            }

            // Это будет применяться только к корневому узлу.
            if (this.NodeData.CompareTo(itemToRemove) == 0)
            {
                // Нет дочерних элементов - ничего не делайте, дерево должно иметь хотя бы один узел.              
                if (this.LeftTree == null && this.RightTree == null)
                {
                    return;
                }
                else // У корневого узла есть дочерние элементы.
                {
                    RemoveNodeWithChildren(this);
                }
            }
        }

        private void RemoveNodeWithChildren(Tree<TItem> node)
        {
            // Проверьте, есть ли у узла дочерние элементы.
            if (node.LeftTree == null && node.RightTree == null)
            {
                throw new ArgumentException("Узел не имеет дочерних элементов");
            }

            // Узел дерева имеет только один дочерний элемент - замените узел дерева его дочерним узлом.
            if (node.LeftTree == null ^ node.RightTree == null)
            {
                if (node.LeftTree == null)
                {
                    node.CopyNodeToThis(node.RightTree);
                }
                else
                {
                    node.CopyNodeToThis(node.LeftTree);
                }
            }
            else
            // Узел дерева имеет два дочерних элемента - замените значение узла дерева на его значение узла "по порядку преемника"
            // а затем удалите узел-преемник по порядку.
            {
                // Найдите преемника по порядку - крайнего левого потомка его свойства RightTree.
                Tree<TItem> successor = GetLeftMostDescendant(node.RightTree);

                // Скопируйте значение узла из преемника по порядку.
                node.NodeData = successor.NodeData;

                // Удалите узел-преемник по порядку
                if (node.RightTree.RightTree == null && node.RightTree.LeftTree == null)
                {
                    node.RightTree = null; // У узла-преемника не было дочерних элементов.
                }
                else
                {
                    node.RightTree.Remove(successor.NodeData);
                }
            }
        }

        private void CopyNodeToThis(Tree<TItem> node)
        {
            this.NodeData = node.NodeData;
            this.LeftTree = node.LeftTree;
            this.RightTree = node.RightTree;
        }

        private Tree<TItem> GetLeftMostDescendant(Tree<TItem> node)
        {
            while (node.LeftTree != null)
            {
                node = node.LeftTree;
            }
            return node;
        }

        public static Tree<TreeItem> BuildTree<TreeItem>
            (TreeItem nodeValue, params TreeItem[] values)
            where TreeItem : IComparable<TreeItem>
        {
            Tree<TreeItem> tree = new Tree<TreeItem>(nodeValue);
            foreach (TreeItem item in values)
            {
                tree.Add(item);
            }
            return tree;
        }
    }
}
