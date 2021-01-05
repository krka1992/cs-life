using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Collections;

namespace Life
{
    public delegate void OnStop(string message);



    class LifeStateStorage
    {
        private Hashtable ht = new Hashtable();
        private string prevHash = "";
        private string currentHash = "";
        private List<byte> currentData = new List<byte>();

        public void CalcHashBegin()
        {
            currentData.Clear();
            prevHash = currentHash;
            if (currentHash != "") ht[currentHash] = 0;
        }

        public string CalcHashEnd()
        {
            SHA256 sha = SHA256.Create();
            byte[] data = sha.ComputeHash(currentData.ToArray());
            currentHash = BitConverter.ToString(data);
            return currentHash;
        }

        public void DataAdd(int val1, int val2, bool val3)
        {
            currentData.Add((byte)(val1 & 0xFF));
            currentData.Add((byte)((val1 >> 8) & 0xFF));
            currentData.Add((byte)((val1 >> 16) & 0xFF));
            currentData.Add((byte)((val1 >> 24) & 0xFF));
            currentData.Add((byte)(val2 & 0xFF));
            currentData.Add((byte)((val2 >> 8) & 0xFF));
            currentData.Add((byte)((val2 >> 16) & 0xFF));
            currentData.Add((byte)((val2 >> 24) & 0xFF));
            if (val3) currentData.Add(1);
        }

        public bool IsPrevState(string hash)
        {
            return hash == prevHash;
        }

        public bool IsOldState(string hash)
        {
            return ht.ContainsKey(hash);
        }

        public void Clear()
        {
            ht.Clear();
            prevHash = "";
            currentHash = "";
        }
    }

    class LifeObject
    {
        public OnStop OnLifeStop;
        private LifeStateStorage StateStorage = new LifeStateStorage();
        private bool[,] data = new bool[0, 0];

        private void MoveData(bool[,] data)
        {
            int maxX = Math.Min(this.data.GetLength(0), data.GetLength(0));
            int maxY = Math.Min(this.data.GetLength(1), data.GetLength(1));

            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    data[i, j] = this.data[i, j];
                }
            }

            this.data = data;
        }

        private int GetPointValueInt_Safe(int x, int y)
        {
            if ((x < 0) || (x >= data.GetLength(0))) return 0;
            if ((y < 0) || (y >= data.GetLength(1))) return 0;
            if (data[x, y]) return 1;
            return 0;
        }
            
        private int GetPointAroundCount(int x, int y)
        {
            int aroundCount = 0;
            aroundCount += GetPointValueInt_Safe(x - 1, y);
            aroundCount += GetPointValueInt_Safe(x + 1, y);
            aroundCount += GetPointValueInt_Safe(x - 1, y - 1);
            aroundCount += GetPointValueInt_Safe(x + 1, y - 1);
            aroundCount += GetPointValueInt_Safe(x, y - 1);
            aroundCount += GetPointValueInt_Safe(x - 1, y + 1);
            aroundCount += GetPointValueInt_Safe(x + 1, y + 1);
            aroundCount += GetPointValueInt_Safe(x, y + 1);

            return aroundCount;
        }

        public LifeObject()
        {

        }

        public LifeObject(int width, int height)
        {
            this.SetSize(width, height);
        }

        public void SetSize(int width, int height)
        {
            bool[,] tmpData = new bool[width, height];
            this.MoveData(tmpData);
        }

        public void SetPointValue(int x, int y, bool value)
        {
            data[x, y] = value;
        }

        private void StopHandler(string message)
        {
            StateStorage.Clear();

            if (OnLifeStop == null)
                return;
            OnLifeStop(message);
        }

        public void Mutate()
        {
            bool[,] tmpData = new bool[data.GetLength(0), data.GetLength(1)];
            int count;
            int hasCounter = 0;

            StateStorage.CalcHashBegin();

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    count = GetPointAroundCount(i, j);
                    if (data[i, j])
                    {
                        tmpData[i, j] = (count == 2) || (count == 3);
                    } else
                    {
                        tmpData[i, j] = count == 3;
                    }
                    if (tmpData[i, j]) hasCounter++;
                    StateStorage.DataAdd(i, j, tmpData[i, j]);
                }
            }

            data = tmpData;

            if (hasCounter == 0)
            {
                StopHandler("Не осталось ни одной живой клетки");
                return;
            }

            string hash = StateStorage.CalcHashEnd();

            if (StateStorage.IsPrevState(hash))
            {
                StopHandler("Состояние эквивалентно предыдущему");
            }
            else if (StateStorage.IsOldState(hash)) {
                StopHandler("Повторилось состояние");
            }
        }

        public bool GetPointValue(int x, int y)
        {
            return data[x, y];
        }

        public void SaveToFile(string filename)
        {

        }

        public void RestoreFromFile(string filename)
        {

        }
    }
}
