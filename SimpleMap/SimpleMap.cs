using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleMap
{
    class DataBlock
    {
        public string type;
        public List<int> segment = new List<int>();
        public List<double> lon = new List<double>();
        public List<double> lat = new List<double>();
        public string name;
    }
    class SimpleMap
    {
        //this is the whole layer
        public List<DataBlock> layerData = new List<DataBlock>();

        public int screenLeft, screenTop, screenRight, screenBottom;
        public double xMin, yMin, xMax, yMax;
        public double scale;
        public int panX;
        public int panY;

        public SimpleMap()
        {
            setMapExtent(3.03002, 51.1, 3.43, 51.37);
            screenResize(0, 0, 640, 480);
            panX = -1;
            panY = -1;
        }

        public void loadPolygonData(String mifFlie, String midFile)
        {
            String sTemp;
            String[] strR;
            int segment;
            StreamReader sr = File.OpenText(mifFlie);
            //ignore the head part
            for (int i = 0; i < 10; ++i)
            {
                sr.ReadLine();
            }
            //start the data reading process.
            while (sr.Peek()!=-1)
            {
                DataBlock dbtemp = new DataBlock();

                sTemp = sr.ReadLine();
                strR = Regex.Split(sTemp.Trim(), @"\s+");
                dbtemp.type = strR[0];
                segment = Convert.ToInt32(strR[1]);

                for (int i = 0; i < segment; ++i)
                {

                    int iCount = Convert.ToInt32(sr.ReadLine());
                    dbtemp.segment.Add(iCount);
                    for (int j = 0; j < iCount; ++j)
                    {

                        sTemp = sr.ReadLine();
                        strR = Regex.Split(sTemp.Trim(), @"\s+");
                        dbtemp.lon.Add(Convert.ToDouble(strR[0]));
                        dbtemp.lat.Add(Convert.ToDouble(strR[1]));
                    }
                }
                //leave two lines away.
                for (int i = 0; i < 2; ++i)
                {
                    sr.ReadLine();
                }
                layerData.Add(dbtemp);
            }
            sr.Close();

        }
        public void loadPolylineData(String mifFlie, String midFile)
        {
            String sTemp;
            String[] strR;
            int segment;
            StreamReader sr = File.OpenText(mifFlie);
            //ignore the head part, 14 lines
            for (int i = 0; i < 14; ++i)
            {
                sr.ReadLine();
            }
            //start the data reading process.
            while (sr.Peek() != -1)
            {
                DataBlock dbtemp = new DataBlock();

                sTemp = sr.ReadLine();
                strR = Regex.Split(sTemp.Trim(), @"\s+");
                if(strR[0]=="Line")
                {
                    //read Line
                    dbtemp.type = strR[0];
                    dbtemp.segment.Add(1);
                    dbtemp.lon.Add(Convert.ToDouble(strR[1]));
                    dbtemp.lat.Add(Convert.ToDouble(strR[2]));
                    dbtemp.lon.Add(Convert.ToDouble(strR[3]));
                    dbtemp.lat.Add(Convert.ToDouble(strR[4]));
                }
                else
                {
                    //read Pline
                    dbtemp.type = strR[0];
                    dbtemp.segment.Add(1);
                    int iCount = Convert.ToInt32(strR[1]);
                    for (int i = 0; i < iCount; i++)
                    {
                        sTemp = sr.ReadLine();
                        strR = Regex.Split(sTemp.Trim(), @"\s+");
                        dbtemp.lon.Add(Convert.ToDouble(strR[0]));
                        dbtemp.lat.Add(Convert.ToDouble(strR[1]));
                    }
                }
                //leave one line away.
                for (int i = 0; i < 1; ++i)
                {
                    sr.ReadLine();
                }
                layerData.Add(dbtemp);
            }
            sr.Close();
        }
        public void loadPointData(String mifFlie, String midFile)
        {
            String sTemp;
            String[] strR;
            int segment;
            StreamReader sr = File.OpenText(mifFlie);
            //ignore the head part, 11 lines
            for (int i = 0; i < 11; ++i)
            {
                sr.ReadLine();
            }
            //start the data reading process.
            while (sr.Peek() != -1)
            {
                DataBlock dbtemp = new DataBlock();

                sTemp = sr.ReadLine();
                strR = Regex.Split(sTemp.Trim(), @"\s+");
                dbtemp.type = strR[0];
                dbtemp.segment.Add(1);
                dbtemp.lon.Add(Convert.ToDouble(strR[1]));
                dbtemp.lat.Add(Convert.ToDouble(strR[2]));
                
                //leave one line away.
                for (int i = 0; i < 1; ++i)
                {
                    sr.ReadLine();
                }
                layerData.Add(dbtemp);
            }

            sr = File.OpenText(midFile);
            char[] sep = {','};
            List<String> pointName = new List<string>();
            while (sr.Peek() != -1)
            {
                sTemp = sr.ReadLine();
                strR = sTemp.Trim().Split(sep);
                pointName.Add(strR[1].Substring(1, strR[1].Length - 2));
            }
            sr.Close();
            for(int i=0;i<layerData.Count;++i)
            {
                layerData[i].name = pointName[i];
            }
        }

        //methods relate to display map
        //setMapExtent
       
        public void setMapExtent(double xMin, double yMin, double xMax, double yMax)
        {
            this.xMin = xMin;
            this.yMin = yMin;
            this.xMax = xMax;
            this.yMax = yMax;
            adjustScale();
        }
        //setScreenArea
        public void screenResize(int screenLeft, int screenTop, int screenRight, int screenBottom)
        {
            this.screenLeft = screenLeft;
            this.screenTop = screenTop;
            this.screenRight = screenRight;
            this.screenBottom = screenBottom;
            adjustScale();
        }
        //setScale
        public void adjustScale()
        {
            double xl, yl;
            xl = xMax - xMin;
            yl = yMax - yMin;

            double scalex = (screenRight - screenLeft) / xl;
            double scaley = (screenBottom - screenTop) / yl;
            if (scalex < scaley)
                scale = scalex;
            else
                scale = scaley;

        }
        //point from world to screen X
        public int worldToScreenX(double xWorld)
        {
            return Convert.ToInt32((xWorld - xMin) * scale) + screenLeft;
        }
        //point from world to screen Y
        public int worldToScreenY(double yWorld)
        {
            return Convert.ToInt32((yMax - yWorld) * scale) + screenTop;
        }
        //points from screen to world
        public double screenToWorldX(int xScreen)
        {
            return (xScreen - screenLeft) / scale + xMin;
        }
        public double screenToWorldY(int yScreen)
        {
            return yMax - (yScreen - screenTop) / scale;
        }

        //draw polygon map layer
        public void drawPolygonLayer(Graphics g)
        {
            Point pt = new Point();
            for (int i=0;i<layerData.Count;++i)
            {
                int pos = 0;
                for (int j=0;j<layerData[i].segment.Count;++j)
                {
                    List<Point> ptdraw = new List<Point>();
                    for (int k=pos; k<layerData[i].segment[j]+pos;++k)
                    {
                        pt.X = worldToScreenX(layerData[i].lon[k]);
                        pt.Y = worldToScreenY(layerData[i].lat[k]);
                        ptdraw.Add(pt);
                    }
                    pos += layerData[i].segment[j];
                    g.FillPolygon(Brushes.Pink, ptdraw.ToArray());
                    g.DrawPolygon(Pens.Black, ptdraw.ToArray());
                }
            }
        }
        public void drawPolylineLayer(Graphics g)
        {
            Point pt = new Point();
            for (int i = 0; i < layerData.Count; ++i)
            {
                List<Point> ptdraw = new List<Point>();
                for (int k = 0; k < layerData[i].lon.Count; ++k)
                {
                    pt.X = worldToScreenX(layerData[i].lon[k]);
                    pt.Y = worldToScreenY(layerData[i].lat[k]);
                    ptdraw.Add(pt);
                }
                g.DrawLines(Pens.LightGreen, ptdraw.ToArray());

            }
        }
        public void drawPointsLayer(Graphics g)
        {
            Point pt = new Point();
            Font f = new Font("Arial", 16, FontStyle.Bold | FontStyle.Italic);
            for (int i = 0; i < layerData.Count; ++i)
            {
                pt.X = worldToScreenX(layerData[i].lon[0]);
                pt.Y = worldToScreenY(layerData[i].lat[0]);
                g.FillEllipse(Brushes.Red, pt.X - 5, pt.Y - 5, 10, 10);
                g.DrawEllipse(Pens.Black, pt.X - 5, pt.Y - 5, 10, 10);
                pt.Offset(5, -25);
                g.DrawString(layerData[i].name, f, Brushes.Black, pt);
            }
            f.Dispose();
        }

        public void panBegin(int X, int Y)
        {
            panX = X;
            panY = Y;
        }
        public void panEnd(int X, int Y)
        {
            if (panX == -1 || panY == -1) return;
            double xWorldBegin = screenToWorldX(panX);
            double yWorldBegin = screenToWorldY(panY);
            double xWorldEnd = screenToWorldX(X);
            double yWorldEnd = screenToWorldY(Y);
            double lx = xWorldEnd - xWorldBegin;
            double ly = yWorldEnd - yWorldBegin;
            setMapExtent(xMin - lx, yMin - ly, xMax - lx, yMax - ly);
            panX = -1;
            panY = -1;
        }
        public void zoomIn()
        {
            double lx = (xMax - xMin) / 4;
            double ly = (yMax - yMin) / 4;
            setMapExtent(xMin + lx, yMin + ly, xMax - lx, yMax - ly);
        }
        public void zoomOut()
        {
            double lx = (xMax - xMin) / 4;
            double ly = (yMax - yMin) / 4;
            setMapExtent(xMin - lx, yMin - ly, xMax + lx, yMax + ly);
        }
        public void zoomInAt(int X, int Y)
        {
            double oldx = screenToWorldX(X);
            double oldy = screenToWorldY(Y);

            zoomIn();

            double xWorldBegin = oldx;
            double yWorldBegin = oldy;

            double xWorldEnd = screenToWorldX(X);
            double yWorldEnd = screenToWorldY(Y);
            double lx = xWorldEnd - xWorldBegin;
            double ly = yWorldEnd - yWorldBegin;
            setMapExtent(xMin - lx, yMin - ly, xMax - lx, yMax - ly);
        }
        public void zoomOutAt(int X, int Y)
        {
            double oldx = screenToWorldX(X);
            double oldy = screenToWorldY(Y);

            zoomOut();

            double xWorldBegin = oldx;
            double yWorldBegin = oldy;

            double xWorldEnd = screenToWorldX(X);
            double yWorldEnd = screenToWorldY(Y);
            double lx = xWorldEnd - xWorldBegin;
            double ly = yWorldEnd - yWorldBegin;
            setMapExtent(xMin - lx, yMin - ly, xMax - lx, yMax - ly);
        }
    }
}
