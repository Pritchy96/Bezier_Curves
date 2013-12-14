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
    Point p1 = new Point(10, 10);
    Point p2 = new Point(10, 300);
    Point p3 = new Point(300, 300);

    //The current point being drawn.
    Point currentPoint = new Point();

    //Array of calculated points.
    ArrayList points = new ArrayList();

    //Incrementer.
    float i = 0;

    //The two coordinates of the guide line
    Point line1;
    Point line2;

    //The rectangles of the control points.
    Rectangle p1Rect;
    Rectangle p2Rect;
    Rectangle p3Rect;

    bool mouseDown = false;

    public MainState()
    {
        //Setting up rectangles initially.
        p1Rect = new Rectangle((int)p1.X - 4, (int)p1.Y - 4, 8, 8);
        p2Rect = new Rectangle((int)p2.X - 4, (int)p2.Y - 4, 8, 8);
        p3Rect = new Rectangle((int)p3.X - 4, (int)p3.Y - 4, 8, 8);
    }

    public void CalculateBezier()
    {
        //Incrementing i.
        i += 0.01f;

        // Calculating guide line coordinates. It uses i to simply move along each line 
        //connecting two of the control coordinates linearly.
        line1 = getPoint(p1, p2, i);
        line2 = getPoint(p2, p3, i);

        // Moves along the guide line by multiplying by i.
        currentPoint = getPoint(line1, line2, i);
        points.Add(currentPoint);
    }

    public override void Update()
    {
        //IF we are not at the end, calculate another few points.
        //i is the end because it is when the second coordinate for the
        //guide line is at the last control point, and the current point
        // rectangle is at the end of that line.
        if (i < 3)
        {
                CalculateBezier();
                CalculateBezier();
                CalculateBezier();
        }
    }

    public override void MouseMoved(MouseEventArgs e)
    {
        if (mouseDown)
        {
            p1 = e.Location;
            i = 0;
            points.Clear();
            p1Rect = new Rectangle((int)p1.X - 4, (int)p1.Y - 4, 8, 8);
            p2Rect = new Rectangle((int)p2.X - 4, (int)p2.Y - 4, 8, 8);
            p3Rect = new Rectangle((int)p3.X - 4, (int)p3.Y - 4, 8, 8);

            p2 = getPoint(p1, p3, 0.5f);
            p2.Y -= Math.Abs(p1.X - p3.X)/4;
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
        e.Graphics.DrawRectangle(Pens.Black, p1Rect);
        e.Graphics.DrawRectangle(Pens.Black, p2Rect);
        e.Graphics.DrawRectangle(Pens.Black, p3Rect);

        //Connect the control Points to make it look pretty.
        e.Graphics.DrawLine(Pens.Black, p1, p2);
        e.Graphics.DrawLine(Pens.Black, p2, p3);

        //Draw the guide line.
        e.Graphics.DrawLine(Pens.Green, line1, line2);

        //Draw the current point rectangle.
        e.Graphics.DrawRectangle(Pens.Red, new Rectangle(currentPoint.X - 4, currentPoint.Y - 4, 8, 8));

        //Draw every pixel in the array of pixels making up the line.
        foreach (Point p in points.ToArray())
        {
            e.Graphics.DrawRectangle(Pens.Orange, new Rectangle((int)p.X, (int)p.Y, 1, 1));
        }
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

