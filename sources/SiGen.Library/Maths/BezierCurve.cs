using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Maths
{
    public class BezierCurve
    {
        private Vector _StartPoint;
        private Vector _EndPoint;
        private Vector[] _ControlPoints;
        private Vector[] Points;
        private Vector[] BaseSegments;

        public Vector StartPoint { get { return _StartPoint; } }
        public Vector EndPoint { get { return _EndPoint; } }
        public Vector[] ControlPoints { get { return _ControlPoints; } }

        public BezierCurve(Vector p1, Vector p2, params Vector[] controlPoints)
        {
            _StartPoint = p1;
            _EndPoint = p2;
            _ControlPoints = controlPoints;
            Initialize();
        }

        private void Initialize()
        {
            Points = new Vector[2 + ControlPoints.Length];
            Points[0] = StartPoint;
            Array.Copy(ControlPoints, 0, Points, 1, ControlPoints.Length);
            Points[Points.Length - 1] = EndPoint;

            BaseSegments = new Vector[Points.Length - 1];
            for(int i = 0; i < Points.Length - 1; i++)
                BaseSegments[i] = Points[i + 1] - Points[i];
        }

        public Vector GetPoint(double t)
        {
            var curPoints = new List<Vector>();
            for (int i = 0; i < BaseSegments.Length; i++)
                curPoints.Add(Points[i] + (BaseSegments[i] * t));

            return Loop(curPoints, t);
        }

        private Vector Loop(List<Vector> points, double t)
        {
            if (points.Count == 1)
                return points[0];
            else if (points.Count == 0)
                return Vector.Empty;

            var curPoints = new List<Vector>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                var vec = points[i + 1] - points[i];
                curPoints.Add(points[i] + (vec * t));
            }

            if (curPoints.Count == 1)
                return curPoints[0];
            else if (points.Count == 0)
                return Vector.Empty;

            return Loop(curPoints, t);
        }

        public List<Vector> Interpolate(double resolution)
        {
            var points = new List<Vector>();
            double t = -resolution;

            while (t < 1)
            {
                t += resolution;
                if (t > 1)
                    t = 1;

                points.Add(GetPoint(t));
            }

            return points;
        }

    }
}
