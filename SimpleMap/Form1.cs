using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleMap
{
    public partial class Form1 : Form
    {
        SimpleMap mapLayer1;  //polygon layer
        SimpleMap mapLayer2;  //pline layer
        SimpleMap mapLayer3;  //point layer
        string strStatusScale = "scale = ";
        string strStatusLonLat = "[ lon , lat ] = ";
        public Form1()
        {
            mapLayer1 = new SimpleMap();
            mapLayer1.loadPolygonData(@"e:\CSharpLocal\SimpleMap\SampleData\layerNatural.mif", "#");
            mapLayer2 = new SimpleMap();
            mapLayer2.loadPolylineData(@"e:\CSharpLocal\SimpleMap\SampleData\layerRoads.mif", "#");
            mapLayer3 = new SimpleMap();
            mapLayer3.loadPointData(@"e:\CSharpLocal\SimpleMap\SampleData\layerPlaces.mif", @"e:\CSharpLocal\SimpleMap\SampleData\layerPlaces.mid");
            
            InitializeComponent();

            statusScale.Text = strStatusScale;
            statusLonLat.Text = strStatusLonLat;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("A simple vector map application.\r\n\r\nAuthor: Wade\r\nVersion: 1.0", "SimpleMap");
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            mapLayer2.drawPolylineLayer(g);
            mapLayer1.drawPolygonLayer(g);
            mapLayer3.drawPointsLayer(g);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            mapLayer1.screenResize(pictureBox1.Left, pictureBox1.Top, pictureBox1.Right, pictureBox1.Bottom);
            mapLayer2.screenResize(pictureBox1.Left, pictureBox1.Top, pictureBox1.Right, pictureBox1.Bottom);
            mapLayer3.screenResize(pictureBox1.Left, pictureBox1.Top, pictureBox1.Right, pictureBox1.Bottom);
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mapLayer1.panBegin(e.X, e.Y);
                mapLayer2.panBegin(e.X, e.Y);
                mapLayer3.panBegin(e.X, e.Y);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button== MouseButtons.Left)
            {
                mapLayer1.panEnd(e.X, e.Y);
                mapLayer2.panEnd(e.X, e.Y);
                mapLayer3.panEnd(e.X, e.Y);
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            statusScale.Text = strStatusScale + string.Format("{0:f5}", mapLayer1.scale);
            double xWorld = mapLayer1.screenToWorldX(e.X);
            double yWorld = mapLayer1.screenToWorldY(e.Y);
            statusLonLat.Text = strStatusLonLat + string.Format("{0:f5},{1:f5}", xWorld, yWorld);
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Left)
            {
                mapLayer1.zoomInAt(e.X,e.Y);
                mapLayer2.zoomInAt(e.X, e.Y);
                mapLayer3.zoomInAt(e.X, e.Y);
                pictureBox1.Invalidate();
            }
            if (e.Button == MouseButtons.Right)
            {
                mapLayer1.zoomOutAt(e.X,e.Y);
                mapLayer2.zoomOutAt(e.X, e.Y);
                mapLayer3.zoomOutAt(e.X, e.Y);
                pictureBox1.Invalidate();
            }
        }

        private void defaultMapExtentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapLayer1.setMapExtent(3.03002, 51.1, 3.43, 51.37);
            mapLayer2.setMapExtent(3.03002, 51.1, 3.43, 51.37);
            mapLayer3.setMapExtent(3.03002, 51.1, 3.43, 51.37);
            pictureBox1.Invalidate();
        }
    }
}
