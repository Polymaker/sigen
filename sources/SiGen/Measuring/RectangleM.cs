using System.Collections.Generic;

namespace SiGen.Measuring
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>The rectangle is specified in 2D space (Y+ up)</remarks>
    public struct RectangleM
    {
        #region Static Const

        public static RectangleM Empty = new RectangleM(Measure.Empty, Measure.Empty, Measure.Empty, Measure.Empty);

        #endregion

        #region Fields

        private Measure x;
        private Measure y;
        private Measure width;
        private Measure height;

        #endregion

        #region Properties

        public Measure X
        {
            get { return x; }
            set { x = value; }
        }

        public Measure Y
        {
            get { return y; }
            set { y = value; }
        }

        public Measure Width
        {
            get { return width; }
            set { width = value; }
        }

        public Measure Height
        {
            get { return height; }
            set { height = value; }
        }

        public PointM Location
        {
            get { return new PointM(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public PointM Size
        {
            get { return new PointM(Width, Height); }
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }

        public PointM Center
        {
            get
            {
                return Location + new PointM(Width / 2, Height / 2 * -1);
            }
        }

        public Measure Left { get { return X; } }
        public Measure Top { get { return Y; } }
        public Measure Right { get { return X + Width; } }
        public Measure Bottom { get { return Top - Height; } }

        public bool IsEmpty
        {
            get { return X.IsEmpty; }
        }

        #endregion

        #region Ctors

        public RectangleM(Measure x, Measure y, Measure width, Measure height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        #endregion

        #region Static Ctors

        public static RectangleM FromLTRB(Measure left, Measure top, Measure right, Measure bottom)
        {
            return new RectangleM(left, top, right - left, top - bottom);
        }

        public static RectangleM BoundingRectangle(IEnumerable<PointM> points)
        {
            Measure minX = Measure.Zero;
            Measure maxX = Measure.Zero;
            Measure minY = Measure.Zero;
            Measure maxY = Measure.Zero;

            foreach (var pt in points)
            {
                if (pt.IsEmpty)
                    continue;
                if (pt.X < minX)
                    minX = pt.X;
                if (pt.Y < minY)
                    minY = pt.Y;
                if (pt.X > maxX)
                    maxX = pt.X;
                if (pt.Y > maxY)
                    maxY = pt.Y;
            }

            return RectangleM.FromLTRB(minX, maxY, maxX, minY);
        }

        #endregion

        #region Equality operators

        public override bool Equals(object obj)
        {
            if (!(obj is RectangleM))
                return false;
            return this == (RectangleM)obj;
        }

        public static bool operator ==(RectangleM left, RectangleM right)
        {
            if (left.IsEmpty || right.IsEmpty)
                return left.IsEmpty == right.IsEmpty;

            return left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;
        }

        public static bool operator !=(RectangleM left, RectangleM right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
