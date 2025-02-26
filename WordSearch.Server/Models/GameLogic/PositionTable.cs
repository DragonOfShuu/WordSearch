namespace WordSearch.Server.Models.GameLogic
{
    using System.Text.Json;
    using TableList = List<List<List<Vector2D>>>;

    /// <summary>
    /// Stores all potential locations
    /// and rotations of word placement.
    /// </summary>
    public class PositionTable
    {
        public int BoardSizeX { get; }
        public int BoardSizeY { get; }
        public TableList Table { get; }

        private static readonly Random Rand = new();

        /// <summary>
        /// Generate a table based on the 
        /// provided board size.
        /// </summary>
        /// <param name="boardSizeX"></param>
        /// <param name="boardSizeY"></param>
        public PositionTable(int boardSizeX, int boardSizeY)
        {
            BoardSizeX = boardSizeX;
            BoardSizeY = boardSizeY;

            TableList positionTables = [];

            for (int y = 0; y < BoardSizeY; y++)
            {
                List<List<Vector2D>> yList = [];
                for (int x = 0; x < BoardSizeX; x++)
                {
                    yList.Add(CreateRotationArray(x, y, boardSizeX, boardSizeY));
                }
                positionTables.Add(yList);
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
        public PositionTable(int boardSizeX, int boardSizeY, TableList table)
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
        private List<Vector2D> CreateRotationArray(int x, int y, int sizeX, int sizeY)
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
            var newTable = JsonSerializer.Deserialize<TableList>(JsonSerializer.Serialize(Table))!;

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

            int randY = Rand.Next(0, Table.Count);
            var listOfX = Table[randY];
            int randX = Rand.Next(0, listOfX.Count);
            var listOfRot = listOfX[randX];
            int randRotIndex = Rand.Next(0, listOfRot.Count);

            Vector2D randomRot = listOfRot[randRotIndex];
            listOfRot.RemoveAt(randRotIndex);
            if (listOfRot.Count == 0)
            {
                listOfX.RemoveAt(randX);
            }

            if (listOfX.Count == 0)
            {
                Table.RemoveAt(randY);
            }

            return new Transform()
            {
                Position = new Vector2D() { X = randX, Y = randY },
                Rotation = randomRot
            };
        }

        public bool IsEmpty()
        {
            return Table.Count == 0;
        }
    }
}
