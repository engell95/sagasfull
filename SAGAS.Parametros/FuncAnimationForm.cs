using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SAGAS.Parametros
{
    public class FuncAnimationForm
    {
        Form window;        
        
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        public extern static void ReleaseCapture();


        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        public extern static void Position(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.DLL", EntryPoint = "SetWindowPos")]
        public extern static bool SetWindowPos(int hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public const int SWP_DRAWFRAME = 0x20;
        public const int SWP_NOMOVE = 0x2;
        public const int SWP_NOSIZE = 0x1;
        public const int SWP_NOZORDER = 0x4;

        public void OpacityTimer(Form window, bool visible)
        {
            if(visible)
                window.Opacity = 0;
            this.window = window;
            Timer timer = new Timer();
            timer.Interval = 30;
            if(visible)
                timer.Tick += new EventHandler(OnTick_visible);
            else
                timer.Tick += new EventHandler(OnTick_invisible);
            timer.Start();
        }

        public void OnTick_visible(Object sender, EventArgs e)
        {
            if (window.Opacity >= 1)
            {
                Timer timer = sender as Timer;
                timer.Stop();                
            }
            window.Opacity += 0.1;
        }

        public void OnTick_invisible(Object sender, EventArgs e)
        {
            if (window.Opacity == 0)
            {
                Timer timer = sender as Timer;
                timer.Stop();
                window.Close();
            }
            window.Opacity -= 0.1;
        }
    }
}
