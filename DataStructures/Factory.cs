using System;
using System.IO;
using System.Linq;

namespace DataStructures
{
    internal class Factory<TKey, T> where TKey : IComparable<TKey> where T : IComparable<T>
    {
        private string _strpath = "Archivo\\Prb.txt";
        private string _fileName;
        private string _path;
        private int _degree;
        private int _height;
        private int _size;
        private int _freePosition;
        private string _address;
        private string _nullData;
        private string _nullKey;
        private FileStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;

        public string DataNull
        {
            get
            {
                return _nullData;
            }
            set
            {
                _nullData = value;
            }
        }

        public string LlaveNull
        {
            get
            {
                return _nullKey;
            }
            set
            {
                _nullKey = value;
            }
        }

        public Factory(string fileName, int degree, string path)
        {
            _address = path;
            _fileName = fileName;
            CreateFolder();
            _degree = degree;
            _height = 0;
            _size = 0;
            _freePosition = 0;
            _path = _address;
            OpenFile();
            GenerateTree();
            _nullKey = int.MinValue.ToString();
            _nullData = "#########################################################################################################################################################################################################################################################################################################################################################################################";
        }

        public Factory(string fileName, string address)
        {
            _address = address;
            _fileName = fileName;
            _address = Path.Combine(_address, _fileName);
            _path = _address;
            OpenFile();
            LoadHeader();
            _nullKey = int.MinValue.ToString();
            _nullData = "#########################################################################################################################################################################################################################################################################################################################################################################################";
        }

        public void CreateFolder()
        {
            try
            {
                Directory.CreateDirectory(_address);
                _address = Path.Combine(_address, _fileName);
            }
            catch(IOException)
            {

            }
        }

        public void OpenFile()
        {
            _stream = new FileStream(_path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _reader = new StreamReader(_stream);
            _writer = new StreamWriter(_stream);
        }

        public void CloseFile()
        {
            _stream.Close();
            _reader.Close();
        }

        private void LoadNull(string keyType, string dataType)
        {
            string keyNull = _nullKey;
            int minValue;
            if (keyNull != "int")
            {
                _nullKey = keyNull == "string" ? "####################################" : (keyNull == "Guid" ? "####################################" : "####################################");
            }
            else
            {
                minValue = int.MinValue;
                _nullKey = minValue.ToString();
            }
            string str = dataType;
            if (!(str == "int"))
            {
                if (!(str == "string"))
                {
                    if (str == "Guid")
                        _nullData = "####################################";
                    else
                        _nullData = "####################################";
                }
                else
                    _nullData = "####################################";
            }
            else
            {
                minValue = int.MinValue;
                _nullData = minValue.ToString();
            }
        }

        public void LoadHeader()
        {
            _reader.BaseStream.Seek(13L, SeekOrigin.Begin);
            _freePosition = int.Parse(_reader.ReadLine());
            _reader.DiscardBufferedData();
            _reader.BaseStream.Seek(26L, SeekOrigin.Begin);
            _size = int.Parse(_reader.ReadLine());
            _reader.DiscardBufferedData();
            _reader.BaseStream.Seek(39L, SeekOrigin.Begin);
            _degree = int.Parse(_reader.ReadLine());
            _reader.DiscardBufferedData();
            _reader.BaseStream.Seek(52L, SeekOrigin.Begin);
            _height = int.Parse(_reader.ReadLine());
            _reader.DiscardBufferedData();
        }

        public void GenerateTree()
        {
            if (!File.Exists(_path))
                return;
            _writer.BaseStream.Seek(0L, SeekOrigin.Begin);
            _writer.WriteLine(int.MinValue.ToString());
            _writer.WriteLine(_freePosition.ToString("D11"));
            _writer.WriteLine(_size.ToString("D11"));
            _writer.WriteLine(_degree.ToString("D11"));
            _writer.WriteLine(_height.ToString("D11"));
            _writer.Flush();
        }

        public void NodeFromFactory()
        {
            string str1 = string.Empty + _freePosition.ToString("D11") + "|" + int.MinValue + "|||";
            for (int index = 0; index < _degree; ++index)
                str1 = str1 + int.MinValue + "|";
            string str2 = str1 + "||";
            for (int index = 0; index < _degree - 1; ++index)
                str2 = str2 + int.MinValue + "|";
            string str3 = str2 + "||";
            for (int index = 0; index < _degree - 1; ++index)
                str3 = str3 + _nullData + "|";
            _writer.BaseStream.Seek(PositionInFile(_freePosition), SeekOrigin.Begin);
            _writer.WriteLine(str3);
            _writer.Flush();
            _freePosition = _freePosition + 1;
            int length = str3.Length;
            _writer.BaseStream.Seek(13L, SeekOrigin.Begin);
            _writer.Write(_freePosition.ToString("D11"));
            _writer.Flush();
        }

        public BNode<TKey, T> BringNode(int actualNode)
        {
            if (actualNode == int.MinValue)
                return null;
            _reader.BaseStream.Seek(PositionInFile(actualNode), SeekOrigin.Begin);
            string str = _reader.ReadLine();
            string[] information = str.Remove(str.Length - 1, 1).Split('|', '|', '|');
            _reader.DiscardBufferedData();
            return new BNode<TKey, T>(_degree, information);
        }

        public void SaveNode(string[] node)
        {
            string str = string.Empty;
            for (int index = 0; index < node.Length; ++index)
                str = !(node[index] == "") ? str + node[index] + "|" : str + "|";
            _writer.BaseStream.Seek(PositionInFile(int.Parse(node[0])), SeekOrigin.Begin);
            _writer.Write(str);
            _writer.Flush();
        }

        private int PositionInFile(int searchedNode)
        {
            int num1 = 65;
            for (int index = 0; index < searchedNode; ++index)
            {
                int num2 = num1;
                int minValue = int.MinValue;
                int num3 = minValue.ToString().Length * 2 + 1;
                int num4 = num2 + num3 + 9;
                minValue = int.MinValue;
                int num5 = minValue.ToString().Length * _degree;
                int num6 = num4 + num5 + (_degree - 1);
                minValue = int.MinValue;
                int num7 = minValue.ToString().Length * (_degree - 1);
                num1 = num6 + num7 + _nullData.Length * (_degree - 1) + (_degree - 2) * 2 + 3;
            }
            return num1;
        }

        public bool Empty()
        {
            _reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            string str = _reader.ReadLine();
            _reader.DiscardBufferedData();
            return str == int.MinValue.ToString();
        }

        public int ObtainRoot()
        {
            _reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            string s = _reader.ReadLine();
            _reader.DiscardBufferedData();
            return int.Parse(s);
        }

        public int ObtainFreePosition()
        {
            _reader.BaseStream.Seek(13L, SeekOrigin.Begin);
            string s = _reader.ReadLine();
            _reader.DiscardBufferedData();
            return int.Parse(s);
        }

        public void ChangeRoot(int newRoot)
        {
            _writer.BaseStream.Seek(0L, SeekOrigin.Begin);
            _writer.Write(newRoot.ToString("D11"));
            _writer.Flush();
        }

        public void ChangeHeight(int add)
        {
            _writer.BaseStream.Seek(52L, SeekOrigin.Begin);
            _writer.Write(add.ToString("D11"));
            _writer.Flush();
        }

        public int ObtainHeight()
        {
            _reader.BaseStream.Seek(52L, SeekOrigin.Begin);
            int num = int.Parse(_reader.ReadLine());
            _reader.DiscardBufferedData();
            return num;
        }

        public void ChangeSize(int add)
        {
            _writer.BaseStream.Seek(26L, SeekOrigin.Begin);
            _writer.Write(add.ToString("D11"));
            _writer.Flush();
        }

        public int ObtainSize()
        {
            _reader.BaseStream.Seek(26L, SeekOrigin.Begin);
            int num = int.Parse(_reader.ReadLine());
            _reader.DiscardBufferedData();
            return num;
        }

        public int ObtainDegree()
        {
            _reader.BaseStream.Seek(39L, SeekOrigin.Begin);
            int num = int.Parse(_reader.ReadLine());
            _reader.DiscardBufferedData();
            return num;
        }

        public bool Delete(string[] nodo)
        {
            try
            {
                if (!File.Exists(_strpath))
                    File.Create(_strpath).Close();
                string[] strArray = File.ReadAllLines(_strpath);
                File.WriteAllText(_strpath, "");
                string str = string.Empty;
                for (int index = 0; index < nodo.Length; ++index)
                    str = !(nodo[index] == "") ? str + nodo[index] + "|" : str + " | ";
                using (StreamWriter streamWriter = new StreamWriter(_strpath, true))
                {
                    for (int index = 0; index < strArray.Length; ++index)
                        streamWriter.WriteLine(strArray[index]);
                    streamWriter.WriteLine(str);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public BNode<TKey, T> VNode()
        {
            string str1 = string.Empty + _freePosition.ToString("D11") + "|" + int.MinValue + "|||";
            for (int index = 0; index < _degree; ++index)
                str1 = str1 + int.MinValue + "|";
            string str2 = str1 + "||";
            _nullData = "####################################";
            for (int index = 0; index < _degree - 1; ++index)
                str2 = str2 + _nullData + "|";
            string str3 = str2 + "||";
            for (int index = 0; index < _degree - 1; ++index)
                str3 = str3 + _nullData + "|";
            return new BNode<TKey, T>(_degree, str3.Remove(str3.Length - 1, 1).Split('|', '|', '|'));
        }

        public bool Search(TKey key, T data)
        {
            try
            {
                string[] strArray = File.ReadAllLines(_strpath);
                for (int index = 0; index < strArray.Count(); ++index)
                {
                    if (strArray[index].Contains(key.ToString()))
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
