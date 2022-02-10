using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtchlanMapGenerator
{

    public static class Constants
    {
        ////windows message id for hotkey
        //public const int WM_HOTKEY_MSG_ID = 0x0312;

        //HEX
        public const int VK_ENTER = 0xD; //This is the enter key.
        public const int VK_NORTH = 0x4E; //This is the n key.
        public const int VK_SOUTH = 0x53; //This is the s key.
        public const int VK_EAST = 0x45; //This is the e key.
        public const int VK_WEST = 0x57; //This is the w key.
    }

    class KeyHandler
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private int key;
        private IntPtr hWnd;
        private int id;

        public KeyHandler(Keys key, Form form)
        {
            this.key = (int)key;
            this.hWnd = form.Handle;
            id = this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return key ^ hWnd.ToInt32();
        }

        public bool Register()
        {
            return RegisterHotKey(hWnd, id, 0, key);
        }

        public bool Unregiser()
        {
            return UnregisterHotKey(hWnd, id);
        }
    }
}
