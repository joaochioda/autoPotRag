using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Memory;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern int FindWindow(string sClass, string sWindow);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        Mem m = new Mem();
        string healthAddress = "AsgardRO.exe+DD1A04";
        IntPtr MWhandle;
        IntPtr CWhandle;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int PID = m.GetProcIdFromName("AsgardRO");
            if (PID > 0)
            {
                m.OpenProcess(PID);
                Thread h1 = new Thread(H1) { IsBackground = true };
                h1.Start();
            }
        }
        bool isGameActive()
        {
            MWhandle = (IntPtr)FindWindow(null, "Asgard RO | Gepard Shield 3.0 (^-_-^)");
            CWhandle = GetForegroundWindow();

            return MWhandle == CWhandle ? true : false;
        }
        private void H1()
        {
            while (true)
            {
                if (checkbox1.Checked && isGameActive())
                {
                    int health = m.ReadInt(healthAddress);
                    if (health < 3000)
                    {
                        SendKeys.SendWait("w");
                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(100);
                }
                Thread.Sleep(100);
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
