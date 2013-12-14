using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

 

    public partial class Screen : Form
    {
        //Screen size.
        public static int WIDTH = 1000;
        public static int HEIGHT = 1000;

        //Thread Variables.
        Boolean Running = false;
        Thread thread = null;

        Manager manager = new Manager();

        #region Function Explanation
        //Constructor, sets Screen size and then begins Thread.
        #endregion
        public Screen()
        {
            InitializeComponent();
            SetScreenSize(WIDTH, HEIGHT);
            BeginThread();
        }

        #region Function Explanation
        //Exit Event, kills Thread on Window close.
        #endregion
        private void OnExit(object sender, FormClosingEventArgs e)
        {
            killThread();
        }

        private void MouseClick(object sender, MouseEventArgs e)
        {
            manager.MouseClicked(e);
        }

        private void MouseMoved(object sender, MouseEventArgs e)
        {
            manager.MouseMoved(e);
        }

        private void Redraw(object sender, PaintEventArgs e)
        {
            manager.Redraw(e);
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            manager.MouseUp(e);
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            manager.MouseDown(e);
        }

        #region Function Explanation
        //Creates and starts a Thread.
        #endregion
        public void BeginThread()
        {
            thread = new Thread(new ThreadStart(Update));
            thread.Start();
            Running = true;
        }

        #region Function Explanation
        //Kills the thread.
        #endregion
        public void killThread()
        {
            //Simply kills off the Thread.
            Running = false;
            thread.Abort();
            thread.Join();
        }

        #region Function Explanation
        //The main Update loop. Basically just updates Manager which handles
        //all Game updates.
        #endregion
        public void Update()
        {
            while (Running)
            {
                manager.Update();
                
                //Cause screen to redraw.
                DrawScreen.Invalidate();

                //Basic Thread Slowing.
                Thread.Sleep(10);
            }
        }

        #region Function Explanation
        //Repaints Manager.
        #endregion
        private void Repaint(object sender, PaintEventArgs e)
        {
            manager.Redraw(e);
        }

        #region Function Explanation
        //Screen Size setting method with adjustments made for screen Border.
        #endregion
        private void SetScreenSize(int width, int height)
        {
            this.Width = width + 6;
            this.Height = height + 28;
        }


    }

