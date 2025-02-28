namespace WordSearch.Server.Models.GameLogic
{
    using System.Text.Json;

    /// <summary>
    /// Stores all potential locations
    /// and rotations of word placement.
    /// </summary>
    public class PositionTable
    {
        public int BoardSizeX { get; }
        public int BoardSizeY { get; }
        public int WordSize { get; }
        public List<LetterTile> Table { get; }

        private static readonly Random Rand = new();

        /// <summary>
        /// Generate a table based on the 
        /// provided board size.
        /// </summary>
        /// <param name="boardSizeX"></param>
        /// <param name="boardSizeY"></param>
        public PositionTable(int boardSizeX, int boardSizeY, int wordSize)
        {
            BoardSizeX = boardSizeX;
            BoardSizeY = boardSizeY;
            WordSize = wordSize;

            List<LetterTile> positionTables = [];

            for (int y = 0; y < BoardSizeY; y++)
            {
                for (int x = 0; x < BoardSizeX; x++)
                {
                    var rotations = CreateRotationArray(x, y, boardSizeX, boardSizeY, wordSize);
                    if (rotations.Count == 0) continue;
                    var position = new Vector2D() { X = x, Y = y };
                    positionTables.Add(new LetterTile() { Position = position, Rotations = rotations });
                }
            }

            Table = positionTables;
        }

        /// <summary>
        /// This service also comes with
        /// a "bring your own" program!
        /// 
        /// You can provide your own table
        /// so we don't have to generate it.
        /// </summary>
        /// <param name="boardSizeX"></param>
        /// <param name="boardSizeY"></param>
        /// <param name="table"></param>
        public PositionTable(int boardSizeX, int boardSizeY, List<LetterTile> table)
        {
            BoardSizeX = boardSizeX;
            BoardSizeY = boardSizeY;
            Table = table;
        }

        /// <summary>
        /// Create all possible rotations for
        /// the provided position.
        /// </summary>
        /// <param name="x">X position of the cursor</param>
        /// <param name="y">Y position of the cursor</param>
        /// <param name="sizeX">The width of the board</param>
        /// <param name="sizeY">The height of the board</param>
        /// <returns></returns>
        private List<Vector2D> CreateRotationArray(int x, int y, int sizeX, int sizeY, int wordSize)
        {
            List<Vector2D> result = [];

            for (int rotY = -1; rotY < 2; rotY++)
            {
                for (int rotX = -1; rotX < 2; rotX++)
                {
                    if (rotY == 0 && rotX == 0) continue;

                    int testX = x + rotX;
                    int testY = y + rotY;

                    if (testX < 0 || testY < 0) continue;
                    if (testX >= sizeX || testY >= sizeY) continue;

                    var destinationX = (wordSize * rotX + x);
                    var destinationY = (wordSize * rotY + y);
                    // If the potential ending letter is outside
                    // of the grid, this rotation is not valid
                    if (destinationX >= sizeX || destinationX < 0) continue;
                    if (destinationY >= sizeY || destinationY < 0) continue;

                    result.Add(new Vector2D() { X = rotX, Y = rotY });
                }
            }

            return result;
        }

        /// <summary>
        /// Deep clone the table so you can
        /// attach them to more words.
        /// </summary>
        /// <returns></returns>
        public PositionTable Clone()
        {
            var newTable = JsonSerializer.Deserialize<List<LetterTile>>(JsonSerializer.Serialize(Table))!;

            return new PositionTable(BoardSizeX, BoardSizeY, newTable);
        }

        /// <summary>
        /// Efficiently create multiple
        /// deep clones.
        /// </summary>
        /// <param name="count">The amount of required clones</param>
        /// <returns></returns>
        public PositionTable[] Clone(int count)
        {
            string tableJSON = JsonSerializer.Serialize(Table);
            var result = new List<PositionTable>();
            for (int i = 0; i < count; i++)
            {
                result.Add(JsonSerializer.Deserialize<PositionTable>(tableJSON)!);
            }

            return [.. result];
        }

        /// <summary>
        /// Choose a random point & rotation
        /// from the table, and remove it
        /// from the table.
        /// </summary>
        /// <returns></returns>
        public Transform? RandomEject()
        {
            if (Table.Count == 0) return null;

            int randTileIndex = Rand.Next(0, Table.Count);
            LetterTile randTile = Table[randTileIndex];

            int randRotIndex = Rand.Next(0, randTile.Rotations.Count);
            Vector2D randRot = randTile.Rotations[randRotIndex];
            randTile.Rotations.RemoveAt(randRotIndex);
            if (randTile.Rotations.Count == 0)
            {
                Table.RemoveAt(randTileIndex);
            }

            return new Transform()
            {
                Position = randTile.Position,
                Rotation = randRot
            };
        }

        public bool IsEmpty()
        {
            return Table.Count == 0;
        }
    }

    public class LetterTile
    {
        public Vector2D Position { get; set; } = new Vector2D();
        public List<Vector2D> Rotations { get; set; } = [];
    }
}
