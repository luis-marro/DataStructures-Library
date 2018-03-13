using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        public delegate int Compare<E>(T a, E b);

        //How many elements are in the binary tree.
        private int count;

        //Nodo root
        private BinaryNode<T> root;

        /// <summary>
        /// Constructor.
        /// </summary>
        public BinaryTree()
        {
            count = 0;
            root = null;
        }

        public int GetCount()
        {
            return this.count;
        }

        //Verify if empty.
        public bool Empty()
        {
            if (root == null)
            {
                return true;
            }

            return false;
        }

        private void myRoot(BinaryNode<T> root)
        {
            this.root = root;
        }

        public void Add(T x)
        {
            BinaryNode<T> newElement = new BinaryNode<T>();
            newElement.Value = x;
            this.Add(newElement, root);
        }

        private void Add(BinaryNode<T> value, BinaryNode<T> root)
        {
            if (root == null)
            {
                //Insert first element.

                this.myRoot(value);
                this.root.SetPadre(null);
                count++;
            }
            else
            {
                if (value.Value.CompareTo(root.Value) == 1)
                {
                    if (root.GetRight() == null)
                    {
                        root.SetRight(value);
                        root.GetRight().SetPadre(root);
                        count++;
                    }
                    else
                    {
                        Add(value, root.GetRight());
                    }
                }
                else
                {
                    if (root.GetLeft() == null)
                    {
                        root.SetLeft(value);
                        root.GetLeft().SetPadre(root);
                        count++;
                    }
                    else
                    {
                        Add(value, root.GetLeft());
                    }
                }
            }
        }

        //Search.
        public BinaryNode<T> Search<E>(Compare<E> compare, E element)
        {
            if (root == null)
            {
                return null;
            }
            else
            {
                return SearchNode(root, compare, element);
            }
        }

        private BinaryNode<T> SearchNode<E>(BinaryNode<T> node, Compare<E> compare, E element)
        {
            if (compare(node.Value, element) == 0)
            {
                return node;
            }
            else
            {
                if (compare(node.Value, element) > 0)
                {
                    if (node.GetLeft() == null)
                    {
                        return null;
                    }
                    else
                    {
                        return SearchNode(node.GetLeft(), compare, element);
                    }
                }
                else
                {
                    if (node.GetRight() == null)
                    {
                        return null;
                    }
                    else
                    {
                        return SearchNode(node.GetRight(), compare, element);
                    }
                }
            }
        }

        private BinaryNode<T> SearchLeft(BinaryNode<T> node)
        {
            if (node.GetLeft() != null)
            {
                return SearchLeft(node.GetLeft());
            }

            return node;
        }

        public bool Eliminate(BinaryNode<T> node)
        {
            bool NodeRight = node.GetRight() != null ? true : false;
            bool NodeLeft = node.GetLeft() != null ? true : false;

            //If node does not exist.
            if (node == null)
            {
                return false;
            }

            if (!NodeRight && !NodeLeft)
            {
                return Eliminate1(node);
            }

            if (NodeRight && !NodeLeft)
            {
                return Eliminate2(node);
            }

            if (!NodeRight && NodeLeft)
            {
                return Eliminate2(node);
            }

            if (NodeRight && NodeLeft)
            {
                return Eliminate3(node);
            }

            return false;
        }
        private bool Eliminate1(BinaryNode<T> node)
        {
            //left
            BinaryNode<T> TempLeft = node.GetPadre().GetLeft();

            //right
            BinaryNode<T> TempRight = node.GetPadre().GetRight();

            if (TempLeft == node)
            {
                node.GetPadre().SetLeft(null);
                count--;
                return true;
            }

            if (TempRight == node)
            {
                node.GetPadre().SetRight(null);
                count--;
                return true;
            }

            return false;
        }

        private bool Eliminate2(BinaryNode<T> node)
        {
            // left
            BinaryNode<T> TempLeft = node.GetPadre().GetLeft();
            // right
            BinaryNode<T> TempRight = node.GetPadre().GetRight();

            BinaryNode<T> actual = node.GetLeft() != null ? node.GetLeft() : node.GetRight();

            if (TempLeft == node)
            {
                node.GetPadre().SetLeft(actual);

                actual.SetPadre(node.GetPadre());
                node.SetRight(null);
                node.SetLeft(null);
                count--;
                return true;
            }

            if (TempRight == node)
            {
                node.GetPadre().SetRight(actual);

                /* Eliminando todas las referencias hacia el nodo */
                actual.SetPadre(node.GetPadre());
                node.SetRight(null);
                node.SetLeft(null);
                count--;
                return true;
            }

            return false;
        }

        private bool Eliminate3(BinaryNode<T> node)
        {
            BinaryNode<T> LeftNode = SearchLeft(node.GetRight());

            if (LeftNode != null)
            {
                node.Value = LeftNode.Value;

                Eliminate(LeftNode);
                return true;
            }

            return false;
        }

        //Edit Element

        public bool Edit<E>(Compare<E> compare, E element, T Item)
        {
            return EditElement(root, compare, element, Item);
        }
        private bool EditElement<E>(BinaryNode<T> node, Compare<E> compare, E element, T Item)
        {
            if (compare(node.Value, element) == 0)
            {
                node.Value = Item;
                return true;
            }
            else
            {
                if (compare(node.Value, element) > 0)
                {
                    if (node.GetLeft() == null)
                    {
                        return false;
                    }
                    else
                    {
                        return EditElement(node.GetLeft(), compare, element, Item);
                    }
                }
                else
                {
                    if (node.GetRight() == null)
                    {
                        return false;
                    }
                    else
                    {
                        return EditElement(node.GetRight(), compare, element, Item);
                    }
                }
            }
        }


        //Recorridos.
        public List<T> PreOrder()
        {
            List<T> preOrder = new List<T>();
            PreOrder(root, ref preOrder);
            return preOrder;
        }

        public List<T> PostOrder()
        {
            List<T> postOrder = new List<T>();
            PostOrder(root, ref postOrder);
            return postOrder;
        }
        public List<T> InOrden()
        {
            List<T> inOrden = new List<T>();
            InOrden(root, ref inOrden);
            return inOrden;
        }

        private void PreOrder(BinaryNode<T> node, ref List<T> list)
        {
            if (node != null)
            {
                list.Add(node.Value);
                PreOrder(node.GetLeft(), ref list);
                PreOrder(node.GetRight(), ref list);
            }
        }

        private void PostOrder(BinaryNode<T> node, ref List<T> list)
        {
            if (node != null)
            {
                PostOrder(node.GetLeft(), ref list);
                PostOrder(node.GetRight(), ref list);
                list.Add(node.Value);
            }
        }

        private void InOrden(BinaryNode<T> node, ref List<T> list)
        {
            if (node != null)
            {
                InOrden(node.GetLeft(), ref list);
                list.Add(node.Value);
                InOrden(node.GetRight(), ref list);
            }
        }

        private IEnumerable<BinaryNode<T>> Traversal(BinaryNode<T> Node)
        {
            if (Node != null)
            {
                if (Node.GetLeft() != null)
                {
                    foreach (BinaryNode<T> LeftNode in Traversal(Node.GetLeft()))
                        yield return LeftNode;
                }

                yield return Node;

                if (Node.GetRight() != null)
                {
                    foreach (BinaryNode<T> RightNode in Traversal(Node.GetRight()))
                        yield return RightNode;
                }
            }
            else
            {
                yield return null;
            }

        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (BinaryNode<T> tempNode in Traversal(root))
            {
                if (tempNode != null)
                {
                    yield return tempNode.Value;
                }
                else
                {
                    yield return default(T);
                }

            }
        }
        
        //In order that we can print the binary tree.
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (BinaryNode<T> TempNode in Traversal(root))
            {
                yield return TempNode.Value;
            }
        }
    }
}
