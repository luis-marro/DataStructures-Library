using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public class BinaryNode<T>
    {
        private T value;
        private int balance;

        public BinaryNode<T> parent;
        public BinaryNode<T> left;
        public BinaryNode<T> right;

        public T Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        public int Balance
        {
            get
            {
                return this.balance;
            }
            set
            {
                this.balance = value;
            }
        }

        public BinaryNode<T> GetPadre()
        {
            return this.parent;
        }

        public void SetPadre(BinaryNode<T> Padre)
        {
            this.parent = Padre;
        }

        public BinaryNode<T> GetLeft()
        {
            return left;
        }

        public void SetLeft(BinaryNode<T> left)
        {
            this.left = left;
        }

        public BinaryNode<T> GetRight()
        {
            return right;
        }

        public void SetRight(BinaryNode<T> right)
        {
            this.right = right;
        }
    }
}
