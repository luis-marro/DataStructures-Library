using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public class AVLTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        public delegate int Compare<E>(T a, E b);

        private int count;
        private BinaryNode<T> root;

        public AVLTree()
        {
            count = 0;
            root = null;
        }

        public int Count()
        {
            return this.count;
        }

        private void SetRoot(BinaryNode<T> root)
        {
            this.root = root;
        }

        #region Add Balanced
        public void Add(T newData)
        {
            BinaryNode<T> newElement = new BinaryNode<T>();
            newElement.Value = newData;
            this.Add(newElement, this.root);
        }

        private void Add(BinaryNode<T> newElement, BinaryNode<T> root)
        {
            if (root == null)
            {
                this.SetRoot(newElement);
                count++;
                this.root.SetPadre(null);
            }
            else
            {
                BinaryNode<T> binaryNode = this.root;

                while (binaryNode != null)
                {
                    int compare = newElement.Value.CompareTo(binaryNode.Value);

                    if (compare < 0)
                    {
                        BinaryNode<T> left = binaryNode.GetLeft();

                        if (left == null)
                        {
                            binaryNode.SetLeft(newElement);
                            binaryNode.GetLeft().SetPadre(binaryNode);
                            count++;
                            InsertBalance(binaryNode, 1);
                            return;
                        }
                        else
                        {
                            binaryNode = left;
                        }
                    }
                    else
                    {
                        if (compare > 0)
                        {
                            BinaryNode<T> right = binaryNode.GetRight();

                            if (right == null)
                            {
                                binaryNode.SetRight(newElement);
                                binaryNode.GetRight().SetPadre(binaryNode);
                                count++;
                                InsertBalance(binaryNode, -1);
                                return;
                            }
                            else
                            {
                                binaryNode = right;
                            }
                        }
                        else
                        {
                            binaryNode.Value = newElement.Value;
                            return;
                        }
                    }
                }
            }
        }

        private void InsertBalance(BinaryNode<T> binaryNode, int balance)
        {
            while (binaryNode != null)
            {
                balance = (binaryNode.Balance += balance);

                if (balance == 0)
                {
                    return;
                }
                else if (balance == 2)
                {
                    if (binaryNode.GetLeft().Balance == 1)
                    {
                        RotateRight(binaryNode);
                    }
                    else
                    {
                        RotateLeftRight(binaryNode);
                    }

                    return;
                }
                else if (balance == -2)
                {
                    if (binaryNode.GetRight().Balance == -1)
                    {
                        RotateLeft(binaryNode);
                    }
                    else
                    {
                        RotateRightLeft(binaryNode);
                    }

                    return;
                }

                BinaryNode<T> parent = binaryNode.GetPadre();

                if (parent != null)
                {
                    balance = parent.GetLeft() == binaryNode ? 1 : -1;
                }

                binaryNode = parent;
            }
        }

        private BinaryNode<T> RotateRight(BinaryNode<T> binaryNode)
        {
            BinaryNode<T> left = binaryNode.GetLeft();
            BinaryNode<T> leftRight = left.GetRight();
            BinaryNode<T> parent = binaryNode.GetPadre();

            left.SetPadre(parent);
            left.SetRight(binaryNode);
            binaryNode.SetLeft(leftRight);
            binaryNode.SetPadre(left);


            if (leftRight != null)
            {
                leftRight.SetPadre(binaryNode);
            }

            if (binaryNode == this.root)
            {
                this.root = left;
            }
            else if (parent.GetLeft() == binaryNode)
            {
                parent.SetLeft(left);
            }
            else
            {
                parent.SetRight(left);
            }

            left.Balance--;
            binaryNode.Balance = -left.Balance;

            return left;
        }

        private BinaryNode<T> RotateLeft(BinaryNode<T> binaryNode)
        {
            BinaryNode<T> right = binaryNode.GetRight();
            BinaryNode<T> rightLeft = right.GetLeft();
            BinaryNode<T> parent = binaryNode.GetPadre();

            right.SetPadre(parent);
            right.SetLeft(binaryNode);
            binaryNode.SetRight(rightLeft);
            binaryNode.SetPadre(right);


            if (rightLeft != null)
            {
                rightLeft.SetPadre(binaryNode);
            }

            if (binaryNode == this.root)
            {
                this.root = right;
            }
            else if (parent.GetRight() == binaryNode)
            {
                parent.SetRight(right);
            }
            else
            {
                parent.SetLeft(right);
            }

            right.Balance++;
            binaryNode.Balance = -right.Balance;

            return right;
        }

        private BinaryNode<T> RotateRightLeft(BinaryNode<T> binaryNode)
        {
            BinaryNode<T> right = binaryNode.GetRight();
            BinaryNode<T> rightLeft = right.GetLeft();
            BinaryNode<T> parent = binaryNode.GetPadre();
            BinaryNode<T> rightLeftLeft = rightLeft.GetLeft();
            BinaryNode<T> rightLeftRight = rightLeft.GetRight();

            rightLeft.SetPadre(parent);
            binaryNode.SetRight(rightLeftLeft);
            right.SetLeft(rightLeftRight);
            rightLeft.SetRight(right);
            rightLeft.SetLeft(binaryNode);
            right.SetPadre(rightLeft);
            binaryNode.SetPadre(rightLeft);

            if (rightLeftLeft != null)
            {
                rightLeftLeft.SetPadre(binaryNode);
            }

            if (rightLeftRight != null)
            {
                rightLeftRight.SetPadre(right);
            }

            if (binaryNode == this.root)
            {
                SetRoot(rightLeft);
            }
            else if (parent.GetRight() == binaryNode)
            {
                parent.SetRight(rightLeft);
            }
            else
            {
                parent.SetLeft(rightLeft);
            }

            if (rightLeft.Balance == 1)
            {
                binaryNode.Balance = 0;
                right.Balance = -1;
            }
            else if (rightLeft.Balance == 0)
            {
                binaryNode.Balance = 0;
                right.Balance = 0;
            }
            else
            {
                binaryNode.Balance = 1;
                right.Balance = 0;
            }

            rightLeft.Balance = 0;

            return rightLeft;
        }

        private BinaryNode<T> RotateLeftRight(BinaryNode<T> binaryNode)
        {
            BinaryNode<T> left = binaryNode.GetLeft();
            BinaryNode<T> leftRight = left.GetRight();
            BinaryNode<T> parent = binaryNode.GetPadre();
            BinaryNode<T> leftRightRight = leftRight.GetRight();
            BinaryNode<T> leftRightLeft = leftRight.GetLeft();

            leftRight.SetPadre(parent);
            binaryNode.SetLeft(leftRightRight);
            left.SetRight(leftRightLeft);
            leftRight.SetLeft(left);
            leftRight.SetRight(binaryNode);
            left.SetPadre(leftRight);
            binaryNode.SetPadre(leftRight);

            if (leftRightRight != null)
            {
                leftRightRight.SetPadre(binaryNode);
            }

            if (leftRightLeft != null)
            {
                leftRightLeft.SetPadre(left);
            }

            if (binaryNode == this.root)
            {
                SetRoot(leftRight);
            }
            else if (parent.GetLeft() == binaryNode)
            {
                parent.SetLeft(leftRight);
            }
            else
            {
                parent.SetRight(leftRight);
            }

            if (leftRight.Balance == -1)
            {
                binaryNode.Balance = 0;
                left.Balance = 1;
            }
            else if (leftRight.Balance == 0)
            {
                binaryNode.Balance = 0;
                left.Balance = 0;
            }
            else
            {
                binaryNode.Balance = -1;
                left.Balance = 0;
            }

            leftRight.Balance = 0;

            return leftRight;


        }

        #endregion

        #region Edit Element

        public bool Edit<E>(Compare<E> compare, E element, T Item)
        {
            return EditElement(root, compare, element, Item);
        }

        private bool EditElement<E>(BinaryNode<T> binaryNode, Compare<E> condicion, E element, T Item)
        {
            if (condicion(binaryNode.Value, element) == 0)
            {
                binaryNode.Value = Item;
                return true;
            }
            else
            {
                if (condicion(binaryNode.Value, element) > 0)
                {
                    if (binaryNode.GetLeft() == null)
                    {
                        return false;
                    }
                    else
                    {
                        return EditElement(binaryNode.GetLeft(), condicion, element, Item);
                    }
                }
                else
                {
                    if (binaryNode.GetRight() == null)
                    {
                        return false;
                    }
                    else
                    {
                        return EditElement(binaryNode.GetRight(), condicion, element, Item);
                    }
                }
            }
        }

        #endregion


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

        private BinaryNode<T> SearchNode<E>(BinaryNode<T> binaryNode, Compare<E> condicion, E element)
        {
            if (condicion(binaryNode.Value, element) == 0)
            {
                return binaryNode;
            }
            else
            {
                if (condicion(binaryNode.Value, element) > 0)
                {
                    if (binaryNode.GetLeft() == null)
                    {
                        return null;
                    }
                    else
                    {
                        return SearchNode(binaryNode.GetLeft(), condicion, element);
                    }
                }
                else
                {
                    if (binaryNode.GetRight() == null)
                    {
                        return null;
                    }
                    else
                    {
                        return SearchNode(binaryNode.GetRight(), condicion, element);
                    }
                }
            }
        }

        private BinaryNode<T> RouteLeft(BinaryNode<T> binaryNode)
        {
            if (binaryNode.GetLeft() != null)
            {
                return RouteLeft(binaryNode.GetLeft());
            }

            return binaryNode;
        }

        public bool Eliminate(BinaryNode<T> binaryNode)
        {
            bool NodeRight = binaryNode.GetRight() != null ? true : false;
            bool NodeLeft = binaryNode.GetLeft() != null ? true : false;

            //If note is empty.
            if (binaryNode == null)
            {
                return false;
            }

            if (!NodeRight && !NodeLeft)
            {
                return Delete1(binaryNode);
            }

            if (NodeRight && !NodeLeft)
            {
                return Delete2(binaryNode);
            }


            if (!NodeRight && NodeLeft)
            {
                return Delete2(binaryNode);
            }

            if (NodeRight && NodeLeft)
            {
                return Delete3(binaryNode);
            }

            return false;
        }

        private bool Delete1(BinaryNode<T> binaryNode)
        {

            //left son
            BinaryNode<T> TempLeft = binaryNode.GetPadre().GetLeft();

            //right son
            BinaryNode<T> TempRight = binaryNode.GetPadre().GetRight();

            if (TempLeft == binaryNode)
            {
                binaryNode.GetPadre().SetLeft(null);
                count--;
                return true;
            }

            if (TempRight == binaryNode)
            {
                binaryNode.GetPadre().SetRight(null);
                count--;
                return true;
            }

            return false;
        }

        private bool Delete2(BinaryNode<T> binaryNode)
        {
            BinaryNode<T> TempLeft = binaryNode.GetPadre().GetLeft();
            BinaryNode<T> TempRight = binaryNode.GetPadre().GetRight();

            BinaryNode<T> current = binaryNode.GetLeft() != null ? binaryNode.GetLeft() : binaryNode.GetRight();

            if (TempLeft == binaryNode)
            {
                binaryNode.GetPadre().SetLeft(current);

                current.SetPadre(binaryNode.GetPadre());
                binaryNode.SetRight(null);
                binaryNode.SetLeft(null);
                count--;
                return true;
            }

            if (TempRight == binaryNode)
            {
                binaryNode.GetPadre().SetRight(current);

                current.SetPadre(binaryNode.GetPadre());
                binaryNode.SetRight(null);
                binaryNode.SetLeft(null);
                count--;
                return true;
            }

            return false;
        }

        private bool Delete3(BinaryNode<T> binaryNode)
        {
            BinaryNode<T> binaryNodeMoreLeft = RouteLeft(binaryNode.GetRight());

            if (binaryNodeMoreLeft != null)
            {
                binaryNode.Value = binaryNodeMoreLeft.Value;

                Eliminate(binaryNodeMoreLeft);
                return true;
            }

            return false;
        }


        //Recorrer AVL
        public List<T> PreOrder()
        {
            List<T> preOrden = new List<T>();
            PreOrder(root, ref preOrden);
            return preOrden;
        }

        public List<T> PostOrder()
        {
            List<T> postOrden = new List<T>();
            PostOrder(root, ref postOrden);
            return postOrden;
        }

        public List<T> InOrder()
        {
            List<T> inOrden = new List<T>();
            InOrder(root, ref inOrden);
            return inOrden;
        }

        private void PreOrder(BinaryNode<T> binaryNode, ref List<T> list)
        {
            if (binaryNode != null)
            {
                list.Add(binaryNode.Value);
                PreOrder(binaryNode.GetLeft(), ref list);
                PreOrder(binaryNode.GetRight(), ref list);
            }
        }

        private void PostOrder(BinaryNode<T> binaryNode, ref List<T> list)
        {
            if (binaryNode != null)
            {
                PostOrder(binaryNode.GetLeft(), ref list);
                PostOrder(binaryNode.GetRight(), ref list);
                list.Add(binaryNode.Value);
            }
        }

        private void InOrder(BinaryNode<T> binaryNode, ref List<T> list)
        {
            if (binaryNode != null)
            {
                InOrder(binaryNode.GetLeft(), ref list);
                list.Add(binaryNode.Value);
                InOrder(binaryNode.GetRight(), ref list);
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
            foreach (BinaryNode<T> TempNode in Traversal(root))
            {
                if (TempNode != null)
                {
                    yield return TempNode.Value;
                }
                else
                {
                    yield return default(T);
                }
            }
        }
        
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (BinaryNode<T> TempNode in Traversal(root))
            {
                yield return TempNode.Value;
            }
        }
    }
}
