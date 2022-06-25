using System;
using System.Numerics;

namespace Battle.Logic.Map
{
    public class MapTile
    {
        /// <summary>
        /// Creates a new instance of MapTile.
        /// </summary>
        /// <param name="x">The node's location along the X axis</param>
        /// <param name="y">The node's location along the Y axis</param>
        /// <param name="z">The node's location along the Z axis</param>
        /// <param name="isWalkable">True if the node can be traversed, false if the node is a wall</param>
        /// <param name="endLocation">The location of the destination node</param>
        public MapTile(int x, int y, int z, string tileType, Vector3 endLocation)
        {
            this.Location = new Vector3(x, y, z);
            this.State = TileState.Untested;
            this.TileType = tileType;
            this.H = GetTraversalCost(this.Location, endLocation);
            this.G = 0;
        }

        /// <summary>
        /// The node's location in the grid
        /// </summary>
        public Vector3 Location { get; private set; }

        /// <summary>
        /// True when the node may be traversed, otherwise false
        /// </summary>
        /// //formerly IsWalkable
        public string TileType { get; set; }

        /// <summary>
        /// Cost from start to here
        /// </summary>
        public float G { get; private set; }

        /// <summary>
        /// Estimated cost from here to end
        /// </summary>
        public float H { get; private set; }

        /// <summary>
        /// Flags whether the node is open, closed or untested by the PathFinder
        /// </summary>
        public TileState State { get; set; }

        /// <summary>
        /// Estimated total cost (F = G + H)
        /// </summary>
        public float F
        {
            get { return this.G + this.H; }
        }

        public int TraversalCost
        {
            get { return Convert.ToInt32(Math.Ceiling(F)); }
        }

        private MapTile _parentTile;
        /// <summary>
        /// Gets or sets the parent node. The start node's parent is always null.
        /// </summary>
        public MapTile ParentTile
        {
            get
            {
                return this._parentTile;
            }
            set
            {
                // When setting the parent, also calculate the traversal cost from the start node to here (the 'G' value)
                this._parentTile = value;
                this.G = this._parentTile.G + GetTraversalCost(this.Location, this._parentTile.Location);
            }
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2}): {3}", this.Location.X, this.Location.Y, this.Location.Z, this.State);
        }

        /// <summary>
        /// Gets the distance between two points
        /// </summary>
        internal static float GetTraversalCost(Vector3 location, Vector3 otherLocation)
        {
            float deltaX = otherLocation.X - location.X;
            float deltaZ = otherLocation.Z - location.Z;
            float result = (float)Math.Sqrt(deltaX * deltaX + deltaZ * deltaZ);
            //System.Diagnostics.Debug.WriteLine("GetTraversalCost:" + result);
            return result;
        }
    }

    /// <summary>
    /// Represents the search state of a Node
    /// </summary>
    public enum TileState
    {
        /// <summary>
        /// The node has not yet been considered in any possible paths
        /// </summary>
        Untested,
        /// <summary>
        /// The node has been identified as a possible step in a path
        /// </summary>
        Open,
        /// <summary>
        /// The node has already been included in a path and will not be considered again
        /// </summary>
        Closed
    }
}
