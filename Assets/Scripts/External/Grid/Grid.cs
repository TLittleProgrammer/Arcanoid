using UnityEngine;

namespace External.Grid
{
    public class Grid<TValue>
    {
        private  Vector2Int _size;
        private TValue[][] _matrix;

        public Grid(Vector2Int size)
        {
            UpdateMatrix(size);
        }
        
        public int Width => _size.x;
        public int Height => _size.y;
        public Vector2Int Size => _size;
        
        public void UpdateMatrix(Vector2Int size)
        {
            if (_size == size)
            {
                return;
            }
            
            _size = size;
            _matrix = new TValue[size.y][];

            for (int i = 0; i < _size.y; i++)
            {
               _matrix[i] = new TValue[_size.x];
            }
        }
        
        public TValue this[int x, int y]
        {
            get => _matrix[y][x];

            set => _matrix[y][x] = value;
        }

        public TValue this[Vector2Int index]
        {
            get => this[index.x, index.y];
            set => this[index.x, index.y] = value;
        }
    }
}