using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hex.Logic
{
    class GameBoard
    {
        private GameOptions Options;
         

        private Dictionary<HexCoordinate, GameBoardPlace> Places { get; set; }

        public GameBoard(GameOptions options)
        {
            this.Options = options;
            this.Places = new Dictionary<HexCoordinate, GameBoardPlace>(); 
            this.GenerateBoard(this.Options);
        }

        public bool PlaceOccupied(HexCoordinate coord)
        {
            if (this.Places.ContainsKey(coord))
            {
                return this.Places[coord].Occupied;
            }
            else
                throw new Exception("Imppossible Coordinate requested");
        }
        
        public GameBoardPlace GetPlace(HexCoordinate coord)
        {
            if (this.Places.ContainsKey(coord))
            {
                return this.Places[coord];
            }
            else
                throw new Exception("Imppossible Coordinate requested");
        }

        public List<GameBoardSpan> GetSpansUntilBoardEdge(HexCoordinate coord)
        {
            List<GameBoardSpan> spans = new List<GameBoardSpan>();

            foreach (var direction in HexCoordinate.Axis)
            {
                GameBoardSpan span = new GameBoardSpan();
                span.Add(GetPlace(coord));

                HexCoordinate next = coord;

                while(next.DistanceFromOrigin() <= this.Options.BoardSize)
                {
                    span.Add(GetPlace(next));
                    next = next.Add(direction);
                }

                spans.Add(span);
            }

            return spans;
        }

        private void GenerateBoard(GameOptions options)
        {

            //Add roott node
            Places.Add(new HexCoordinate(0, 0, 0), new GameBoardPlace());

            for ( int i = 1; i <= options.BoardSize; i++)
            {
                GenerateHexLayer(i);
            }
        }

        private void GenerateHexLayer(int layer)
        {
            //Lazy method
            // Iterate over all possible Hex Co-Ordinates, those that are valid
            // (sum to zero) and are the required distance from the centre (sum of absolutes X 2 = layer)
            // Add these to the dictionary
            for ( int i = -layer; i <= layer; i++)
                for (int j = -layer; j <= layer; j++)
                    for (int k = -layer; k <= layer; k++)
                    {

                        var coord = new HexCoordinate(i, j, k);

                        if (coord.Valid() && coord.DistanceFromOrigin() == layer)
                        { 
                            this.Places.Add(new HexCoordinate(i, j, k), new GameBoardPlace());
                        }
                        
                    }
        }

        
    }

    class GameBoardSpan : List<GameBoardPlace>
    {

    }

    class GameBoardPlace
    { 
        public GamePlayer Occupier { get; set; }

        public bool Occupied
        {
            get
            {
                return (Occupier != null);
            } 
        }
    }

    /// <summary> 
    /// based from: http://www-cs-students.stanford.edu/~amitp/Articles/Hexagon2.html
    /// X+Y+Z == 0 for all X,Y,Z to be a valid Hex Co-Ordinates
    /// </summary>
    class HexCoordinate : Tuple<int,int,int>
    {
        public HexCoordinate(int x, int y, int z)
            :base(x,y,z)
        {

        } 

        public int X { get { return this.Item1; } }
        public int Y { get { return this.Item2; } }
        public int Z { get { return this.Item3; } }

        public bool Valid()
        {
            if (this.X + this.Y + this.Z == 0)
                return true;
            else
                return false;
        }

        public int DistanceFromOrigin()
        {
            if (Valid())
            {

                int value = Math.Abs(this.X) + Math.Abs(this.Y) + Math.Abs(this.Z);

                if (value % 2 != 0)
                {
                    throw new Exception("Hex Co-Ordinate - none multiple of two");
                }
                else
                {
                    return value / 2;
                }
            }
            else
            {
                throw new Exception("Invalid Hex Co-Ordinate - Attempt to calculate Distance.");
            }
        }

        private static List<HexCoordinate> axis;

        /// <summary>
        /// All possible Valid Hex-Coordinate directions
        /// </summary>
        public static List<HexCoordinate> Axis
        {
            get
            {
                if (axis == null)
                {
                    axis = new List<HexCoordinate>();
                    axis.Add(new HexCoordinate(1, -1, 0));
                    axis.Add(new HexCoordinate(-1, 1, 0));
                    axis.Add(new HexCoordinate(-1, 0, 1));
                    axis.Add(new HexCoordinate(1, 0, -1));
                    axis.Add(new HexCoordinate(0, 1, -1));
                    axis.Add(new HexCoordinate(0, -1, 1));
                }

                return axis;
            }
        }
        
        /// <summary>
        /// Could be better implemented via operator overloading
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public HexCoordinate Add(HexCoordinate coord)
        {
            return new HexCoordinate(coord.X + this.X, coord.Y + this.Y, coord.Z + this.Z);
        }
    } 
}
