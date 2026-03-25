namespace Chess.Logic
{
    public struct Move
    {
        public int FromRow;
        public int FromCol;
        public int ToRow;
        public int ToCol;

        public Move(int fromRow, int fromCol, int toRow, int toCol)
        {
            FromRow = fromRow;
            FromCol = fromCol;
            ToRow = toRow;
            ToCol = toCol;
        }
    }
}

