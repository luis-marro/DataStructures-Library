using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    internal class BNode<TKey, T> where TKey : IComparable<TKey> where T : IComparable<T>
    {
        private int _position;
        private int _father;
        private int _degree;
        private List<string> _children;
        private List<string> _keys;
        private List<string> _data;

        public int Degree
        {
            get
            {
                return this._degree;
            }
            set
            {
                this._degree = value;
            }
        }

        public int Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }

        public int Father
        {
            get
            {
                return this._father;
            }
            set
            {
                this._father = value;
            }
        }

        public List<string> Children
        {
            get
            {
                return this._children;
            }
            set
            {
                this._children = value;
            }
        }

        public List<string> Keys
        {
            get
            {
                return this._keys;
            }
            set
            {
                this._keys = value;
            }
        }

        public List<string> Data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            }
        }

        public BNode(int Degree, string[] information)
        {
            this._degree = Degree;
            this.Position = int.Parse(information[0]);
            this.Father = int.Parse(information[1]);
            this._children = new List<string>();
            this._keys = new List<string>();
            this._data = new List<string>();
            int index1 = 4;
            for (int index2 = 0; index2 < this._degree; ++index2)
            {
                this._children.Add(information[index1]);
                ++index1;
            }
            int index3 = this._degree + 6;
            for (int index2 = 0; index2 < this._degree - 1; ++index2)
            {
                this._keys.Add(information[index3]);
                this._data.Add(information[index3 + this._degree + 1]);
                ++index3;
            }
        }

        public string[] Information()
        {
            List<string> stringList = new List<string>();
            stringList.Add(this._position.ToString("D11"));
            if (this._father == int.MinValue)
                stringList.Add(this._father.ToString());
            else
                stringList.Add(this._father.ToString("D11"));
            stringList.Add("");
            stringList.Add("");
            for (int index = 0; index < this.Children.Count; ++index)
                stringList.Add(this.Children[index]);
            stringList.Add("");
            stringList.Add("");
            for (int index = 0; index < this.Keys.Count; ++index)
            {
                if (this._keys[index] == int.MinValue.ToString())
                    stringList.Add(this.Keys[index]);
                else
                    stringList.Add(int.Parse(this.Keys[index]).ToString("D11"));
            }
            stringList.Add("");
            stringList.Add("");
            for (int index = 0; index < this.Data.Count; ++index)
            {
                if (this.Data[index].Length == 377)
                {
                    stringList.Add(this.Data[index]);
                }
                else
                {
                    string str = this.Data[index] + "_";
                    while (str.Length < 377)
                        str += "#";
                    stringList.Add(str);
                }
            }
            return stringList.ToArray();
        }
    }
}
