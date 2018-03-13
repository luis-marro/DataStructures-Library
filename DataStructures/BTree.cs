using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; 

namespace DataStructures
{
    public class BTree<TKey, T> where TKey : IComparable<TKey> where T : IComparable<T>
    {
        private string _keyNull = string.Empty;
        private string _dataNull = string.Empty;
        private Factory<TKey, T> _build;
        private BNode<TKey, T> _root;
        private int _degree;
        public string Name; 

        public BTree(string fileName, int degree, string path, string name)
        {
            this._build = new Factory<TKey, T>(fileName, degree, path);
            this._root = (BNode<TKey, T>)null;
            this._degree = degree;
            this._keyNull = this._build.LlaveNull;
            this._dataNull = this._build.DataNull;
            this.Name = name; 
        }

        public BTree(string fileName, string path, string name)
        {
            this._build = new Factory<TKey, T>(fileName, path);
            this._root = this._build.BringNode(this._build.ObtainRoot());
            this._degree = this._build.ObtainDegree();
            this._keyNull = this._build.LlaveNull;
            this._dataNull = this._build.DataNull;
            this.Name = name; 
        }
        
        public void Close()
        {
            this._build.CloseFile();
        }

        public bool Insert(TKey key, T dato)
        {
            if (this.Search(key) != int.MinValue)
                return false;
            if (this._build.Empty())
            {
                int num = this._build.ObtainFreePosition();
                this._build.ChangeRoot(num);
                this._build.NodeFromFactory();
                BNode<TKey, T> bnode = this._build.BringNode(num);
                bnode.Keys[0] = key.ToString();
                bnode.Data[0] = dato.ToString();
                this._build.SaveNode(bnode.Information());
                this._build.ChangeSize(this._build.ObtainSize() + 1);
            }
            else
            {
                this.Inserting(key, dato);
                this._build.ChangeSize(this._build.ObtainSize() + 1);
            }
            return true;
        }

        private void Inserting(TKey key, T dato)
        {
            BNode<TKey, T> node;
            int index;
            for (node = this._build.BringNode(this._build.ObtainRoot()); !this.IsLeaf(node.Children); node = this._build.BringNode(int.Parse(node.Children[index])))
                index = this.WhereToGo(node.Keys, key);
            if (this.SpaceAvailable(node.Keys))
                this._build.SaveNode(this.InsertInLeafNode(node, key, dato).Information());
            else
                this.InsertInFullNode(node, key, dato);
        }

        private BNode<TKey, T> InsertInLeafNode(BNode<TKey, T> node, TKey newKey, T newData)
        {
            for (int index = 0; index < node.Keys.Count; ++index)
            {
                if (node.Keys[index] == this._keyNull)
                {
                    node.Keys[index] = newKey.ToString();
                    node.Data[index] = newData.ToString();
                    return node;
                }
                if (newKey.ToString().CompareTo(node.Keys[index]) <= 0)
                {
                    node.Keys.Insert(index, newKey.ToString());
                    node.Keys.RemoveAt(node.Keys.Count - 1);
                    node.Data.Insert(index, newData.ToString());
                    node.Data.RemoveAt(node.Keys.Count - 1);
                    return node;
                }
            }
            return node;
        }

        private void InsertInFullNode(BNode<TKey, T> node, TKey newKey, T newData)
        {
            string str1 = newKey.ToString();
            string str2 = newData.ToString();
            BNode<TKey, T> bnode1 = (BNode<TKey, T>)null;
            BNode<TKey, T> bnode2 = (BNode<TKey, T>)null;
            bool flag = false;
            do
            {
                if (node == null)
                {
                    int num = this._build.ObtainFreePosition();
                    this._build.NodeFromFactory();
                    this._build.ChangeRoot(num);
                    node = this._build.BringNode(num);
                    this._root = node;
                    this._build.ChangeHeight(this._build.ObtainHeight() + 1);
                }
                BNode<TKey, T> bnode3 = this._build.BringNode(node.Father);
                int num1;
                if (!this.SpaceAvailable(node.Keys))
                {
                    List<string> stringList1 = new List<string>();
                    List<string> stringList2 = new List<string>();
                    List<string> stringList3 = new List<string>();
                    int actualNode = this._build.ObtainFreePosition();
                    this._build.NodeFromFactory();
                    BNode<TKey, T> bnode4 = this._build.BringNode(actualNode);
                    int index1 = 0;
                    while (str1.CompareTo(node.Keys[index1]) > 0 && index1 < this._degree - 1)
                    {
                        stringList1.Insert(index1, node.Keys[index1]);
                        stringList2.Insert(index1, node.Data[index1]);
                        stringList3.Insert(index1, node.Children[index1]);
                        ++index1;
                        if (index1 == node.Keys.Count<string>())
                            break;
                    }
                    stringList1.Insert(index1, str1);
                    stringList2.Insert(index1, str2);
                    if (bnode1 != null)
                    {
                        List<string> stringList4 = stringList3;
                        int index2 = index1;
                        num1 = bnode1.Position;
                        string str3 = num1.ToString("D11");
                        stringList4.Insert(index2, str3);
                    }
                    else
                    {
                        List<string> stringList4 = stringList3;
                        int index2 = index1;
                        num1 = int.MinValue;
                        string str3 = num1.ToString();
                        stringList4.Insert(index2, str3);
                    }
                    if (bnode2 != null)
                    {
                        List<string> stringList4 = stringList3;
                        int index2 = index1 + 1;
                        num1 = bnode2.Position;
                        string str3 = num1.ToString("D11");
                        stringList4.Insert(index2, str3);
                    }
                    else
                    {
                        List<string> stringList4 = stringList3;
                        int index2 = index1 + 1;
                        num1 = int.MinValue;
                        string str3 = num1.ToString();
                        stringList4.Insert(index2, str3);
                    }
                    for (; index1 < this._degree - 1; ++index1)
                    {
                        stringList1.Insert(index1 + 1, node.Keys[index1]);
                        stringList2.Insert(index1 + 1, node.Data[index1]);
                        stringList3.Insert(index1 + 2, node.Children[index1 + 1]);
                    }
                    int index3 = stringList1.Count % 2 != 0 ? stringList1.Count / 2 : stringList1.Count / 2 - 1;
                    for (int index2 = 0; index2 < node.Keys.Count; ++index2)
                    {
                        if (index2 < index3)
                        {
                            node.Keys[index2] = stringList1[index2];
                            node.Data[index2] = stringList2[index2];
                            node.Children[index2] = stringList3[index2];
                        }
                        else
                        {
                            node.Keys[index2] = this._keyNull;
                            node.Data[index2] = this._dataNull;
                            List<string> newChildren = node.Children;
                            int index4 = index2;
                            num1 = int.MinValue;
                            string str3 = num1.ToString();
                            newChildren[index4] = str3;
                        }
                    }
                    node.Children[index3] = stringList3[index3];
                    if (index3 != node.Children.Count)
                    {
                        for (int index2 = index3 + 1; index2 < node.Children.Count; ++index2)
                        {
                            List<string> newChildren = node.Children;
                            int index4 = index2;
                            num1 = int.MinValue;
                            string str3 = num1.ToString();
                            newChildren[index4] = str3;
                        }
                    }
                    for (int index2 = 0; index2 < bnode4.Keys.Count; ++index2)
                    {
                        if (this._degree % 2 == 0)
                        {
                            if (index2 <= index3)
                            {
                                bnode4.Keys[index2] = stringList1[index3 + index2 + 1];
                                bnode4.Data[index2] = stringList2[index3 + index2 + 1];
                                bnode4.Children[index2] = stringList3[index3 + index2 + 1];
                            }
                        }
                        else if (index2 < index3)
                        {
                            bnode4.Keys[index2] = stringList1[index3 + index2 + 1];
                            bnode4.Data[index2] = stringList2[index3 + index2 + 1];
                            bnode4.Children[index2] = stringList3[index3 + index2 + 1];
                        }
                    }
                    bnode4.Children[this.UsedSpaces(bnode4.Keys)] = stringList3[this._degree];
                    for (int index2 = 0; index2 <= this.UsedSpaces(node.Keys); ++index2)
                    {
                        BNode<TKey, T> bnode5 = this._build.BringNode(int.Parse(node.Children[index2]));
                        if (bnode5 != null)
                        {
                            bnode5.Father = node.Position;
                            this._build.SaveNode(bnode5.Information());
                        }
                    }
                    for (int index2 = 0; index2 <= this.UsedSpaces(bnode4.Keys); ++index2)
                    {
                        BNode<TKey, T> bnode5 = this._build.BringNode(int.Parse(bnode4.Children[index2]));
                        if (bnode5 != null)
                        {
                            bnode5.Father = bnode4.Position;
                            this._build.SaveNode(bnode5.Information());
                        }
                    }
                    str1 = stringList1[index3];
                    str2 = stringList2[index3];
                    bnode1 = node;
                    bnode2 = bnode4;
                    this._build.SaveNode(bnode1.Information());
                    this._build.SaveNode(bnode2.Information());
                    node = bnode3;
                }
                else
                {
                    int index1 = 0;
                    if (this.UsedSpaces(node.Keys) > 0)
                    {
                        int num2 = this.UsedSpaces(node.Keys);
                        while (index1 < num2 && str1.CompareTo(node.Keys[index1]) > 0)
                            ++index1;
                        for (int index2 = num2; index2 > index1; --index2)
                        {
                            node.Keys[index2] = node.Keys[index2 - 1];
                            node.Data[index2] = node.Data[index2 - 1];
                        }
                        for (int index2 = num2 + 1; index2 > index1; --index2)
                            node.Children[index2] = node.Children[index2 - 1];
                    }
                    node.Keys[index1] = str1;
                    node.Data[index1] = str2;
                    if (bnode1 != null)
                    {
                        List<string> newChildren = node.Children;
                        int index2 = index1;
                        num1 = bnode1.Position;
                        num1 = int.Parse(num1.ToString());
                        string str3 = num1.ToString("D11");
                        newChildren[index2] = str3;
                        bnode1.Father = node.Position;
                    }
                    else
                    {
                        List<string> newChildren = node.Children;
                        int index2 = index1;
                        num1 = int.MinValue;
                        string str3 = num1.ToString();
                        newChildren[index2] = str3;
                    }
                    if (bnode2 != null)
                    {
                        List<string> newChildren = node.Children;
                        int index2 = index1 + 1;
                        num1 = bnode2.Position;
                        num1 = int.Parse(num1.ToString());
                        string str3 = num1.ToString("D11");
                        newChildren[index2] = str3;
                        bnode2.Father = node.Position;
                    }
                    else
                    {
                        List<string> newChildren = node.Children;
                        int index2 = index1 + 1;
                        num1 = int.MinValue;
                        string str3 = num1.ToString();
                        newChildren[index2] = str3;
                    }
                    this._build.SaveNode(node.Information());
                    this._build.SaveNode(bnode1.Information());
                    this._build.SaveNode(bnode2.Information());
                    flag = true;
                }
            }
            while (!flag);
        }

        private bool SpaceAvailable(List<string> llaves)
        {
            return llaves.Contains(this._keyNull);
        }

        private int UsedSpaces(List<string> llaves)
        {
            int num = 0;
            for (int index = 0; index < llaves.Count; ++index)
            {
                if (llaves[index] != this._keyNull)
                    ++num;
            }
            return num;
        }

        private int FindEmptyPosition(string[] keysString)
        {
            for (int index = 0; index < keysString.Length; ++index)
            {
                if (keysString[index] == this._keyNull)
                    return index;
            }
            return 0;
        }

        private bool IsLeaf(List<string> childrenList)
        {
            for (int index = 0; index < childrenList.Count; ++index)
            {
                if (childrenList[index] != int.MinValue.ToString())
                    return false;
            }
            return true;
        }

        private int WhereToGo(List<string> keyList, TKey newKey)
        {
            int num = this._degree - 1;
            for (int index = 0; index < this._degree - 1; ++index)
            {
                if (!(keyList[index] != this._keyNull) || newKey.ToString().CompareTo(int.Parse(keyList[index]).ToString()) <= 0)
                    return index;
            }
            return num;
        }

        public int Search(TKey key)
        {
            if (this._build.Empty())
                return int.MinValue;
            for (BNode<TKey, T> bnode = this._build.BringNode(this._build.ObtainRoot()); bnode != null; bnode = this._build.BringNode(int.Parse(bnode.Children[this.WhereToGo(bnode.Keys, key)])))
            {
                for (int index = 0; index < bnode.Keys.Count; ++index)
                {
                    if (int.Parse(bnode.Keys[index]).ToString() == key.ToString())
                        return bnode.Position;
                }
            }
            return int.MinValue;
        }

        public string BringData(TKey key)
        {
            if (this._build.Empty())
                return string.Empty;
            for (BNode<TKey, T> bnode = this._build.BringNode(this._build.ObtainRoot()); bnode != null; bnode = this._build.BringNode(int.Parse(bnode.Children[this.WhereToGo(bnode.Keys, key)])))
            {
                for (int index = 0; index < bnode.Keys.Count; ++index)
                {
                    int num = int.Parse(bnode.Keys[index]);
                    if (num.ToString() == key.ToString())
                    {
                        num = int.Parse(bnode.Keys[index]);
                        return num.ToString() + "_" + bnode.Data[index];
                    }
                }
            }
            return string.Empty;
        }

        public List<string> TraverseTree()
        {
            List<string> llaves = new List<string>();
            int nodo = this._build.ObtainRoot();
            this.TraverseTreeSorted(ref llaves, nodo);
            return llaves;
        }

        private void TraverseTreeSorted(ref List<string> keysString, int node)
        {
            if (node == int.MinValue)
                return;
            BNode<TKey, T> bnode = this._build.BringNode(node);
            for (int index = 0; index < bnode.Children.Count; ++index)
                this.TraverseTreeSorted(ref keysString, int.Parse(bnode.Children[index]));
            for (int index = 0; index < bnode.Keys.Count; ++index)
                keysString.Add(int.Parse(bnode.Keys[index]).ToString());
            keysString.RemoveAll((Predicate<string>)(x => x == int.MinValue.ToString()));
        }

        public bool Erase(TKey key)
        {
            if (this._build.Empty())
                return false;
            BNode<TKey, T> node = this._build.BringNode(this._build.ObtainRoot());
            this.DeleteIntern(node, key);
            if (node.Data.Count<string>() == 0 && !this.IsLeaf(node.Children))
                this._build.ChangeHeight(this._build.ObtainHeight() - 1);
            return true;
        }

        private int WhereToGo(List<string> keysString, TKey key, int space)
        {
            if (keysString.Count<string>() == space)
            {
                if (keysString.Contains("####################################"))
                    return Array.IndexOf<string>(keysString.ToArray(), "####################################") - 1;
                return keysString.Count<string>() - 1;
            }
            if (keysString[space].CompareTo(key) < 0)
                return this.WhereToGo(keysString, key, space + 1);
            return space;
        }

        private void DeleteIntern(BNode<TKey, T> node, TKey keyToDelete)
        {
            int index = Array.IndexOf<string>(node.Keys.ToArray(), keyToDelete.ToString());
            if (index < 0)
            {
                index = this.WhereToGo(node.Keys, keyToDelete, 0);
                if (index < 0)
                    index = 0;
            }
            if (index < node.Keys.Count<string>() && node.Keys[index].CompareTo((object)keyToDelete) == 0)
            {
                this.DeleteKeyFromNode(node, keyToDelete, index);
            }
            else
            {
                if (this.IsLeaf(node.Children))
                    return;
                this.DeleteKeyFromSubtree(node, keyToDelete, index);
            }
        }

        private void DeleteKeyFromNode(BNode<TKey, T> node, TKey keyToDelete, int keyIndexInNode)
        {
            if (this.IsLeaf(node.Children))
            {
                string keyToRemove = node.Keys[keyIndexInNode];
                string itemToRemove = node.Data[keyIndexInNode];
                node.Data = ((IEnumerable<string>)node.Data.Where<string>((Func<string, bool>)(val => val != itemToRemove)).ToArray<string>()).ToList<string>();
                node.Keys = ((IEnumerable<string>)node.Keys.Where<string>((Func<string, bool>)(val => val != keyToRemove)).ToArray<string>()).ToList<string>();
                this._build.SaveNode(node.Information());
                this._build.ChangeSize(this._build.ObtainSize() - 1);
            }
            else if (this._build.BringNode(keyIndexInNode).Data.Count<string>() < this._degree)
                ;
        }

        private void DeleteKeyFromSubtree(BNode<TKey, T> parentNode, TKey keyToDelete, int subtreeIndexInNode)
        {
            BNode<TKey, T> node = this._build.BringNode(int.Parse(parentNode.Children[subtreeIndexInNode]));
            if (node.Data.Count<string>() > 0)
            {
                int index1 = subtreeIndexInNode - 1;
                int index2 = subtreeIndexInNode + 1;
                BNode<TKey, T> bnode1 = subtreeIndexInNode > 0 ? this._build.BringNode(int.Parse(parentNode.Children[index1])) : (BNode<TKey, T>)null;
                BNode<TKey, T> bnode2 = subtreeIndexInNode < parentNode.Children.Count<string>() - 1 ? this._build.BringNode(int.Parse(parentNode.Children[index2])) : (BNode<TKey, T>)null;
                if (bnode1 != null && bnode1.Data.Count<string>() > this._build.ObtainDegree() - 1)
                {
                    List<string> list1 = node.Data.OfType<string>().ToList<string>();
                    list1.Insert(0, parentNode.Data[subtreeIndexInNode]);
                    node.Data = list1;
                    List<string> list2 = node.Keys.OfType<string>().ToList<string>();
                    list2.Insert(0, parentNode.Keys[subtreeIndexInNode]);
                    node.Keys = list2;
                    parentNode.Data[subtreeIndexInNode] = bnode1.Data.Last<string>();
                    string keyToRemove = bnode1.Keys[bnode1.Data.Count<string>() - 1];
                    string itemToRemove = bnode1.Data[bnode1.Data.Count<string>() - 1];
                    bnode1.Data = ((IEnumerable<string>)bnode1.Data.Where<string>((Func<string, bool>)(val => val != itemToRemove)).ToArray<string>()).ToList<string>();
                    bnode1.Keys = ((IEnumerable<string>)bnode1.Keys.Where<string>((Func<string, bool>)(val => val != keyToRemove)).ToArray<string>()).ToList<string>();
                    if (!this.IsLeaf(bnode1.Children))
                    {
                        List<string> list3 = node.Children.OfType<string>().ToList<string>();
                        list3.Insert(0, bnode1.Children.Last<string>());
                        node.Children = list3;
                        keyToRemove = bnode1.Children[bnode1.Children.Count<string>() - 1];
                        bnode1.Children = ((IEnumerable<string>)bnode1.Children.Where<string>((Func<string, bool>)(val => val != keyToRemove)).ToArray<string>()).ToList<string>();
                    }
                    this._build.SaveNode(bnode1.Information());
                    this._build.SaveNode(parentNode.Information());
                    this._build.SaveNode(node.Information());
                }
                else if (bnode2 != null && bnode2.Data.Count<string>() > this._build.ObtainDegree() - 1)
                {
                    node.Children[node.Children.Count<string>() - 1] = parentNode.Children[subtreeIndexInNode];
                    parentNode.Data[subtreeIndexInNode] = bnode2.Data.First<string>();
                    parentNode.Keys[subtreeIndexInNode] = bnode2.Keys.First<string>();
                    string itemToRemove = bnode2.Data[0];
                    bnode2.Data = ((IEnumerable<string>)bnode2.Data.Where<string>((Func<string, bool>)(val => val != itemToRemove)).ToArray<string>()).ToList<string>();
                    itemToRemove = bnode2.Keys[0];
                    bnode2.Keys = ((IEnumerable<string>)bnode2.Keys.Where<string>((Func<string, bool>)(val => val != itemToRemove)).ToArray<string>()).ToList<string>();
                    if (!this.IsLeaf(bnode2.Children))
                    {
                        List<string> list = node.Children.OfType<string>().ToList<string>();
                        list.Insert(0, bnode2.Children.Last<string>());
                        node.Children = list;
                        itemToRemove = bnode2.Children[0];
                        bnode2.Children = ((IEnumerable<string>)bnode2.Children.Where<string>((Func<string, bool>)(val => val != itemToRemove)).ToArray<string>()).ToList<string>();
                    }
                    this._build.SaveNode(bnode2.Information());
                    this._build.SaveNode(parentNode.Information());
                    this._build.SaveNode(node.Information());
                }
                else if (bnode1 != null)
                {
                    List<string> list1 = node.Data.OfType<string>().ToList<string>();
                    list1.Insert(0, parentNode.Data[subtreeIndexInNode]);
                    node.Data = list1;
                    List<string> list2 = node.Keys.OfType<string>().ToList<string>();
                    list2.Insert(0, parentNode.Keys[subtreeIndexInNode]);
                    node.Keys = list2;
                    List<string> newData = node.Data;
                    List<string> newKeys = node.Keys;
                    node.Data = bnode1.Data;
                    node.Keys = bnode1.Keys;
                    string[] array1 = new string[newData.Count + node.Data.Count];
                    node.Data.CopyTo(array1, 0);
                    newData.CopyTo(array1, node.Data.Count);
                    string[] array2 = new string[newKeys.Count + node.Keys.Count];
                    node.Keys.CopyTo(array2, 0);
                    newKeys.CopyTo(array2, node.Keys.Count);
                    if (!this.IsLeaf(bnode1.Children))
                    {
                        List<string> newChildren = node.Children;
                        node.Children = bnode1.Children;
                        string[] array3 = new string[newData.Count + node.Data.Count];
                        node.Data.CopyTo(array3, 0);
                        newData.CopyTo(array3, node.Data.Count);
                        string[] array4 = new string[newKeys.Count + node.Keys.Count];
                        node.Keys.CopyTo(array4, 0);
                        newKeys.CopyTo(array4, node.Keys.Count);
                    }
                    string itemToRemove = parentNode.Children[index1];
                    parentNode.Children = ((IEnumerable<string>)parentNode.Children.Where<string>((Func<string, bool>)(val => val != itemToRemove)).ToArray<string>()).ToList<string>();
                    itemToRemove = parentNode.Data[subtreeIndexInNode];
                    parentNode.Data = ((IEnumerable<string>)parentNode.Data.Where<string>((Func<string, bool>)(val => val != itemToRemove)).ToArray<string>()).ToList<string>();
                    itemToRemove = parentNode.Keys[subtreeIndexInNode];
                    parentNode.Keys = ((IEnumerable<string>)parentNode.Keys.Where<string>((Func<string, bool>)(val => val != itemToRemove)).ToArray<string>()).ToList<string>();
                    this._build.SaveNode(parentNode.Information());
                    this._build.SaveNode(node.Information());
                }
                else if (bnode2 != null)
                {
                    node.Data[node.Data.Count<string>() - 1] = parentNode.Data[subtreeIndexInNode];
                    node.Keys[node.Keys.Count<string>() - 1] = parentNode.Keys[subtreeIndexInNode];
                    if (bnode2 != null)
                    {
                        string[] array1 = new string[bnode2.Data.Count + node.Data.Count];
                        node.Data.CopyTo(array1, 0);
                        bnode2.Data.CopyTo(array1, node.Data.Count);
                        string[] array2 = new string[bnode2.Keys.Count + node.Keys.Count];
                        node.Keys.CopyTo(array2, 0);
                        bnode2.Keys.CopyTo(array2, node.Keys.Count);
                        if (!this.IsLeaf(bnode2.Children))
                        {
                            string[] array3 = new string[bnode2.Children.Count + node.Children.Count];
                            node.Children.CopyTo(array3, 0);
                            bnode2.Children.CopyTo(array3, node.Children.Count);
                        }
                        string itemToRemove = parentNode.Children[index2];
                        parentNode.Children = ((IEnumerable<string>)parentNode.Children.Where<string>((Func<string, bool>)(val => val != itemToRemove)).ToArray<string>()).ToList<string>();
                        itemToRemove = parentNode.Data[subtreeIndexInNode];
                        parentNode.Data = ((IEnumerable<string>)parentNode.Data.Where<string>((Func<string, bool>)(val => val != itemToRemove)).ToArray<string>()).ToList<string>();
                        itemToRemove = parentNode.Keys[subtreeIndexInNode];
                        parentNode.Keys = ((IEnumerable<string>)parentNode.Keys.Where<string>((Func<string, bool>)(val => val != itemToRemove)).ToArray<string>()).ToList<string>();
                        this._build.SaveNode(parentNode.Information());
                        this._build.SaveNode(node.Information());
                    }
                }
            }
            this.DeleteIntern(node, keyToDelete);
        }

        public bool Delete(TKey key)
        {
            if (this._build.Empty())
                return false;
            this.DeleteInternal(this._build.BringNode(this._build.ObtainRoot()), key);
            return true;
        }

        private void DeleteInternal(BNode<TKey, T> node, TKey keyToDelete)
        {
            int index = Array.IndexOf<string>(node.Keys.ToArray(), keyToDelete.ToString());
            if (index < 0)
            {
                index = this.WhereToGo(node.Keys, keyToDelete, 0);
                if (index < 0)
                    index = 0;
            }
            if (index < node.Keys.Count<string>() && node.Keys[index].CompareTo((object)keyToDelete) == 0)
            {
                this.DeleteKeyNode(node, keyToDelete, index);
            }
            else
            {
                if (this.IsLeaf(node.Children))
                    return;
                this.DelteKeyFromSubtre(node, keyToDelete, index);
            }
        }

        private void DelteKeyFromSubtre(BNode<TKey, T> parentNode, TKey keyToDelete, int subtreeIndexInNode)
        {
            this.DeleteInternal(this._build.BringNode(int.Parse(parentNode.Children[subtreeIndexInNode])), keyToDelete);
        }

        private void DeleteKeyNode(BNode<TKey, T> node, TKey keyToDelete, int keyIndexInNode)
        {
            string llave = node.Keys[keyIndexInNode];
            string dato = node.Data[keyIndexInNode];
            this._build.NodeFromFactory();
            BNode<TKey, T> bnode = this._build.VNode();
            bnode.Keys[0] = llave.ToString();
            bnode.Data[0] = dato.ToString();
            this._build.Delete(bnode.Information());
        }
    }
}
