using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

class MainState : BasicState
{
    //The three control points from which to draw a Bezier curve.
    Point p1 = new Point(10, 230);
    Point p2 = new Point(50, 130);
    Point p3 = new Point(100, 140);

    //The current point being drawn.
    Point currentPoint = new Point();

    //Array of calculated points.
    ArrayList points = new ArrayList();
    ArrayList controlPoints = new ArrayList();

    Rectangle screenSize = new Rectangle(0, 0, 1440, 900);

    //Incrementer.
    float i = 0;
    float j = 1;

    //The two coordinates of the guide line
    Point line1;
    Point line2;

    bool mouseDown = false;

    public MainState()
    {

        controlPoints.Add(p1);
        controlPoints.Add(p2);
        controlPoints.Add(p3);
        NewCurve(p1, p2, p3);
    }

    public Point CalculateBezierPoint(Point p1, Point p2, Point p3, float i)
    {
        // Calculating guide line coordinates. It uses i to simply move along each line 
        //connecting two of the control coordinates linearly.
        line1 = getPoint(p1, p2, i);
        line2 = getPoint(p2, p3, i);

        // Moves along the guide line by multiplying by i.
        currentPoint = getPoint(line1, line2, i);

        return currentPoint;
    }

    public override void Update()
    {
       
    }


    public void NewCurve(Point p1, Point p2, Point p3)
    {
        currentPoint = p1;
        controlPoints.Add(p1);
        controlPoints.Add(p2);
        controlPoints.Add(p3);

        //IF we are not at the end, calculate another few points.
        //i is the end because it is when the second coordinate for the
        //guide line is at the last control point, and the current point
        // rectangle is at the end of that line.
        while (i < 20 && screenSize.Contains(currentPoint))
        {
            i += 0.1f;
            CalculateBezierPoint(p1, p2, p3, i);

            if (j < 2)
            {
                if (currentPoint.Y >= 700)
                {
                    Point lastPoint = (Point) points[points.Count - 1];

                    float dX = Math.Abs((float)currentPoint.X - (float)lastPoint.X);
                    float dY = Math.Abs((float)currentPoint.Y - (float)lastPoint.Y);

                    float gradient = dY / dX;

                    int y2 = 700 - (int)(200 / j);
                    int y3 = 700;

                    double angle = Math.Atan(dX / dY) * (180 / Math.PI) ;

                    Point a = new Point(currentPoint.X, 700);
                    
                    Point b = new Point((int)((p2.Y / j) * Math.Sin(angle))/2, y2);
                    Point c = new Point((int)((p2.Y / j) * Math.Sin(angle)), y3);
                    

                    i = 0;
                    j++;

                    NewCurve(a, b, c);
                }
                else
                {
                    points.Add(currentPoint);
                }
            }
        }
    }

    public override void MouseMoved(MouseEventArgs e)
    {
        if (mouseDown)
        {
            Point mouseXY = e.Location;

            //If it's the first point, we don't want to trigger a new curve if it's below
            //The platform. Therefore, we just make x as far down as it can be.
            if (mouseXY.Y > 700)
            {
                mouseXY.Y = 699;
            }

            //Setting the curves to null.
            i = 0;
            j = 1;
            points.Clear();
            controlPoints.Clear();

            //p1 = the current mouse position.
            p1 = mouseXY;
            


            p2 = getPoint(p1, p3, 0.5f);
            p2.Y -= Math.Abs(p1.X - p3.X)/4;

            NewCurve(p1, p2, p3);
        }


        
    }

    public override void MouseUp(MouseEventArgs e)
    {
        mouseDown = false;
    }

    public override void MouseDown(MouseEventArgs e)
    {
            
            p3 = e.Location;
            mouseDown = true;
    }

    public override void Redraw(PaintEventArgs e)
    {
        //Draw the control point rectangles.
        foreach (Point p in controlPoints)
        {
            e.Graphics.DrawRectangle(Pens.Black, new Rectangle((int)p.X - 4, (int)p.Y - 4, 8, 8));
        }

        //Connect the control Points to make it look pretty.
        e.Graphics.DrawLine(Pens.Black, p1, p2);
        e.Graphics.DrawLine(Pens.Black, p2, p3);

        //Draw the guide line.
        //e.Graphics.DrawLine(Pens.Green, line1, line2);

        //Draw the current point rectangle.
        //e.Graphics.DrawRectangle(Pens.Red, new Rectangle(currentPoint.X - 4, currentPoint.Y - 4, 8, 8));

        //Draw every pixel in the array of pixels making up the line.
        foreach (Point p in points.ToArray())
        {
            e.Graphics.DrawRectangle(Pens.Red, new Rectangle((int)p.X, (int)p.Y, 1, 1));
        }

        e.Graphics.FillRectangle(Brushes.Gray, new Rectangle(0, 700, 1440, 10));
    }

    public Point getPoint(Point point1, Point point2, float perc)
    {
        //Finds the distance between two points and then multiplies
        //it by a percentage to find a position between them.
        //As the curve progresses it goes further along the guide line
        //as it multiplies it by a bigger percentage (i).
        Point length = new Point(point2.X - point1.X, point2.Y - point1.Y);

        Point point = new Point((int)(point1.X + (length.X * perc)), (int)(point1.Y + (length.Y * perc)));
        return point;
    }
}

