using SiGen.Maths;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI
{
    public partial class LayoutViewer : Control
    {
        private double _Zoom;
        private Vector cameraPosition;
        private Vector dragStart;

        public LayoutViewer()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.Selectable, true);
            _Zoom = 1d;
            dragStart = Vector.Empty;
            cameraPosition = Vector.Zero;
        }

        #region Drawing

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            var zoomf = (float)_Zoom;
            var center = new PointF(Width / 2f, Height / 2f);
            pe.Graphics.TranslateTransform(center.X, center.Y);
            pe.Graphics.ScaleTransform((float)_Zoom, (float)_Zoom);
            pe.Graphics.TranslateTransform((float)cameraPosition.X * -1, (float)cameraPosition.Y);

            pe.Graphics.DrawRectangle(Pens.Black, -10, -10, 20, 20);
            pe.Graphics.DrawRectangle(Pens.Black, -20, -20, 40, 40);
            DrawLine(pe.Graphics, new Vector(-10, -10), new Vector(10, 10), Color.Red);
        }

        private void DrawLine(Graphics g, Vector p1, Vector p2, Color color)
        {
            using (var pen = new Pen(color, 1f / (float)_Zoom))
                g.DrawLine(pen, (float)p1.X, (float)p1.Y * -1, (float)p2.X, (float)p2.Y * -1);
        }

        #endregion



        public void ResetCamera()
        {
            _Zoom = 1d;
            cameraPosition = Vector.Zero;
            Invalidate();
        }

        #region Mouse Handling

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Select();
            if (e.Button == MouseButtons.Middle)
                dragStart = DisplayToLocal(e.Location);
            else if (e.Button == MouseButtons.Left)
            {
                var worldPos = DisplayToWorld(e.Location);
                Console.WriteLine(worldPos);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Middle)
                dragStart = Vector.Empty;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!dragStart.IsEmpty)
            {
                var curPos = DisplayToLocal(e.Location);
                var diffVec = curPos - dragStart;
                if (diffVec.Length > 1)
                {
                    diffVec *= -1;
                    cameraPosition += diffVec / _Zoom;
                    dragStart = curPos;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (e.Button == MouseButtons.Middle)
                ResetCamera();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (/*(ModifierKeys & Keys.Control) == Keys.Control*/true)
            {
                var oldZoom = _Zoom;
                var mousePos = e.Location;

                if (e.Delta > 0)
                    _Zoom *= 1.1;
                else
                    _Zoom *= 0.9;

                var curWorldPos = DisplayToWorld(mousePos, oldZoom);
                var finalWorldPos = DisplayToWorld(mousePos);
                cameraPosition -= (finalWorldPos - curWorldPos);
                Invalidate();
            }
        }

        #endregion

        #region UI <-> 2D coordinates

        private Vector DisplayToLocal(Point pt)
        {
            var center = new Vector(Width / 2d, Height / 2d);
            var pos = new Vector(pt.X, (Height - 1) - pt.Y);
            return pos - center;
        }

        private Vector DisplayToWorld(Point pt)
        {
            return DisplayToWorld(pt, _Zoom);
        }

        private Vector DisplayToWorld(Point pt, double zoom)
        {
            return (DisplayToLocal(pt) / zoom) + cameraPosition;
        }

        #endregion
    }
}
