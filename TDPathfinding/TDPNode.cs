namespace TimeDRODPOF.TDPathfinding
{
    public class TDPNode
    {
        public TDPNode(int mX, int mY)
        {
            X = mX;
            Y = mY;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public TDPNode Parent { get; set; }
        public double G { get; set; }
        public double H { get; set; }
        public double F { get { return G + H; } }
        public bool IsClosed { get; set; }
        public bool IsObstacle { get; set; }
        public bool IsVisited { get; set; }

        public void Refresh()
        {
            G = H = 0;
            Parent = null;
            IsClosed = false;
            IsObstacle = false;
            IsVisited = false;
        }

        public override string ToString() { return string.Format("X:{0} Y:{1}", X, Y); }
    }
}