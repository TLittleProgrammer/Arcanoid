namespace App.Scripts.External.Extensions.MathExtensions
{
    public static class IntDoubleArrayExtensions
    {
        public static int[,] CopyWithNewSize(this int[,] sourceArray, int x, int y)
        {
            int[,] resultArray = new int[x, y];

            if (sourceArray is not null)
            {
                for (int i = 0; i < sourceArray.GetLength(1) && i < x; i++)
                {
                    for (int j = 0; j < sourceArray.GetLength(0) && j < y; j++)
                    {
                        resultArray[j, i] = sourceArray[j, i];
                    }
                }
            }
            
            return resultArray;
        }
    }
}