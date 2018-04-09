using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Maths
{
    public class BezierSpline
    {
        private Vector[] _SplinePoints;
        private Vector[] _Segments;
        private BezierCurve[] _Curves;
        private double _TotalLength;

        public Vector[] SplinePoints
        {
            get { return _SplinePoints; }
        }

        public BezierCurve[] Curves
        {
            get { return _Curves; }
        }

        public BezierSpline(params Vector[] splinePoints)
        {
            _SplinePoints = splinePoints;
            ComputeCurves();
        }

        public BezierSpline(IEnumerable<Vector> splinePoints, double weight)
        {
            _SplinePoints = splinePoints.ToArray();
            ComputeCurves(weight);
        }

        private void ComputeCurves(double weight = 1d)
        {
            var n = _SplinePoints.Length - 1;
            var p1 = new Vector[n];
            var p2 = new Vector[n];
            _Curves = new BezierCurve[n];
            _Segments = new Vector[n];

            /*rhs vector*/
            var a = new double[n];
            var b = new double[n];
            var c = new double[n];
            var r = new Vector[n];

            /*left most segment*/
            a[0] = 0;
            b[0] = 2;
            c[0] = 1;
            r[0] = SplinePoints[0] + 2 * SplinePoints[1];

            /*internal segments*/
            for (int i = 1; i < n - 1; i++)
            {
                a[i] = 1;
                b[i] = 4;
                c[i] = 1;
                r[i] = 4 * SplinePoints[i] + 2 * SplinePoints[i + 1];
            }

            /*right segment*/
            a[n - 1] = 2;
            b[n - 1] = 7;
            c[n - 1] = 0;
            r[n - 1] = 8 * SplinePoints[n - 1] + SplinePoints[n];

            /*solves Ax=b with the Thomas algorithm (from Wikipedia)*/
            for (int i = 1; i < n; i++)
            {
                var m = a[i] / b[i - 1];
                b[i] = b[i] - m * c[i - 1];
                r[i] = r[i] - m * r[i - 1];
            }

            p1[n - 1] = r[n - 1] / b[n - 1];
            for (int i = n - 2; i >= 0; --i)
                p1[i] = (r[i] - c[i] * p1[i + 1]) / b[i];

            /*we have p1, now compute p2*/
            for (int i = 0; i < n - 1; i++)
                p2[i] = 2 * SplinePoints[i + 1] - p1[i + 1];

            p2[n - 1] = 0.5 * (SplinePoints[n] + p1[n - 1]);

            for (int i = 0; i < n; i++)
            {
                _Segments[i] = (_SplinePoints[i + 1] - _SplinePoints[i]);
                _TotalLength += _Segments[i].Length;
                var cp1 = p1[i];
                var cp2 = p2[i];
                if (weight != 1)
                {
                    cp1 = _SplinePoints[i] + ((cp1 - _SplinePoints[i]) * weight);
                    cp2 = _SplinePoints[i + 1] + ((cp2 - _SplinePoints[i + 1]) * weight);
                }
                _Curves[i] = new BezierCurve(_SplinePoints[i], _SplinePoints[i + 1], cp1, cp2);
            }
        }

        public Vector GetPoint(double t)
        {
            var targetDist = _TotalLength * t;
            double curTotal = 0;
            for(int i = 0; i < _Curves.Length; i++)
            {
                var diff = targetDist - curTotal;
                curTotal += _Segments[i].Length;
                if(targetDist <= curTotal)
                {
                    var targetRatio = diff / _Segments[i].Length;
                    return _Curves[i].GetPoint(targetRatio);
                }
            }
            int curveIdx = (int)Math.Floor(_Curves.Length * t);
            if (curveIdx >= _Curves.Length)
                curveIdx = _Curves.Length - 1;
            return _Curves[curveIdx].GetPoint(t);
        }

        /// <summary>
        /// Interpolates each bezier curve
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public List<Vector> Interpolate(double resolution, double mergeTolerance = 0.01)
        {
            var points = new List<Vector>();
            for (int i = 0; i < _Curves.Length; i++)
            {
                if (points.Count > 0)//remove last point because it is the same as the first point on the next curve
                    points.RemoveAt(points.Count - 1);

                points.AddRange(_Curves[i].Interpolate(resolution));
            }

            for(int i = 0; i < points.Count - 1; i++)
            {
                if((points[i + 1] - points[i]).Length < mergeTolerance)
                    points.RemoveAt(1 + (i--));
            }

            return points;
        }

        /// <summary>
        /// Interpolates the spline
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public List<Vector> InterpolateV2(double resolution, double mergeTolerance = 0.01)
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

            for (int i = 0; i < points.Count - 1; i++)
            {
                if ((points[i + 1] - points[i]).Length < mergeTolerance)
                    points.RemoveAt(1 + (i--));
            }

            return points;
        }
    }
}
