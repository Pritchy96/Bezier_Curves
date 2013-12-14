using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


    class Manager
    {
        private BasicState currentState;

        public BasicState CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        public Manager()
        {
            currentState = new MainState();
        }

        public void MouseMoved(MouseEventArgs e)
        {
            currentState.MouseMoved(e);
        }

        public void MouseClicked(MouseEventArgs e)
        {
            currentState.MouseClicked(e);
        }

        public void MouseUp(MouseEventArgs e)
        {
            currentState.MouseUp(e);
        }

        public void MouseDown(MouseEventArgs e)
        {
            currentState.MouseDown(e);
        }

        public void Update()
        {
            currentState.Update();
        }

        public void Redraw(PaintEventArgs e)
        {
            currentState.Redraw(e);
        }

    }

        
