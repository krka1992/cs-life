using System;
using System.Collections.Generic;
using System.Text;

namespace Life
{
    class LifeObject
    {
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

        public void Mutate()
        {
            bool[,] tmpData = new bool[data.GetLength(0), data.GetLength(1)];
            int count;

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
                }
            }

            data = tmpData;
        }

        public bool GetPointValue(int x, int y)
        {
            return data[x, y];
        }
    }
}
