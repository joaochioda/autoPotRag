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

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy,int dwData, int dwExtraInfo);
        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const uint MOUSEEVENTF_RIGHTUP = 0x10;

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern void SetCursorPos(Int32 x, Int32 y);

        Mem m = new Mem();
        string healthAddress = "AsgardRO.exe+DD1A04";
        IntPtr MWhandle;
        IntPtr CWhandle;

        int middleCenterX = 20*39;
        int middleCenterY = (11 * 40) + 21;

        int offSetX = 0;
        int offSetY = 0;

        int positionPlayerX = 0;
        int positionPlayerY = 0;


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

        private void centerMouse()
        {
            SetCursorPos(middleCenterX, middleCenterY);
            Thread.Sleep(100);
            positionPlayerX = m.ReadInt("AsgardRO.exe+DBA5A0");
            positionPlayerY = m.ReadInt("AsgardRO.exe+DBA5A4");
        }
        private void moveToDirection(int x, int y)
        {
            int posX;
            int posY;
          
            posX = (x-positionPlayerX) * 39 + middleCenterX;
            posY = (positionPlayerY - y) * (40) + middleCenterY;
          
            if (posX > 1600 || posY > 900)
            {
                return;
            }
            SetCursorPos(posX, posY);
            Thread.Sleep(100);
        }

        private void findMobAndAttack()
        {
            int positionMonsterX = m.ReadInt("AsgardRO.exe+DB94D4");
            int positionMonsterY = m.ReadInt("AsgardRO.exe+DB94D8");
            if (positionMonsterX != positionPlayerX && positionMonsterY != positionPlayerY)
            {
                moveToDirection(positionMonsterX, positionMonsterY);
                Thread.Sleep(100);
                mouse_event((int)(MOUSEEVENTF_LEFTDOWN), 0, 0, 0, 0);
                Thread.Sleep(100);
                mouse_event((int)(MOUSEEVENTF_LEFTUP), 0, 0, 0, 0);
                Thread.Sleep(100);
            }
                Thread.Sleep(1500);
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
                if (checkBox2.Checked && isGameActive())
                {
                    centerMouse();
                    //moveToDirection(42, 156);
                    findMobAndAttack();
                    //mouse_event((int)(MOUSEEVENTF_LEFTDOWN), 0, 0, 0, 0);
                    //mouse_event((int)(MOUSEEVENTF_LEFTUP), 0, 0, 0, 0);

                }
                Thread.Sleep(100);
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
