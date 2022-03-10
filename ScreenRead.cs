using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using IronOcr;

namespace OtchlanMapGenerator
{
    public struct ReadResult
    {
       public String locationName;
       public String locationDescription;
       public Dir exit_n;
       public Dir exit_s;
       public Dir exit_e;
       public Dir exit_w;
    }

    class ScreenRead
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern IntPtr ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        IronTesseract ironTesseract;
        //String ssPath;
        Bitmap bitmap;
        String readedString;

        public ScreenRead()
        {
            this.ironTesseract = new IronTesseract();
            this.ironTesseract.MultiThreaded = true;
            this.ironTesseract.Language = OcrLanguage.PolishFast;
            this.ironTesseract.AddSecondaryLanguage(OcrLanguage.EnglishFast);

            this.readedString = "";
            //this.ssPath = "Capture.jpg";
        }

        Boolean CaptureScreen()
        {
            if (Process.GetProcessesByName("otchlan_starter").Length == 0)
            {
                MessageBox.Show("Game process not detected!");
                return false;
            }

            Process proc = Process.GetProcessesByName("otchlan_starter")[0];

            try
            {
                IntPtr procHnd = proc.MainWindowHandle;
                SetForegroundWindow(procHnd);
                ShowWindow(procHnd, 9); //Activates and displays the window. 9 - If the window is minimized or maximized, the system restores it to its original size and position. 

                //Handle covering the window
                Rect rect = new Rect();
                IntPtr rectPtr = GetWindowRect(proc.MainWindowHandle, ref rect);
                while (rectPtr == (IntPtr)0)
                {
                    rectPtr = GetWindowRect(proc.MainWindowHandle, ref rect);
                }

                int width = rect.right - rect.left;
                int height = rect.bottom - rect.top;

                this.bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                Graphics.FromImage(bitmap).CopyFromScreen(rect.left, rect.top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);


                //bitmap.Save("screen.jpg", ImageFormat.Jpeg);
                //bitmap.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }
        void getTextFromScreen()
        {
            if(!this.CaptureScreen()) return;

            var result = this.ironTesseract.Read(this.bitmap);
            this.readedString = result.Text;

            //=======Searched========
            //---------last----------
            //
            // < (...)  > (...) \n
            // LOCATION_NAME \n
            // "Wyjścia:" EXITS \n
            // DESCRIPTION \n
            //
            //
            this.readedString = this.readedString.Substring(0,this.readedString.LastIndexOfAny("<".ToCharArray())-1); //
            this.readedString = this.readedString.Substring(this.readedString.LastIndexOfAny("<".ToCharArray()));

            //return result.Text;
        }

        public ReadResult getLocationInfo()
        {
            ReadResult readRersult = new ReadResult();
            readRersult.exit_n = Dir.not;
            readRersult.exit_s = Dir.not;
            readRersult.exit_e = Dir.not;
            readRersult.exit_w = Dir.not;

            this.getTextFromScreen();

            //LOCATION NAME
            String locName;
            if (this.readedString.Contains("\r\n\r\n"))
            {
                locName = this.readedString.Substring(this.readedString.IndexOf("\r\n") + 4);
            }
            else
            {
                locName = this.readedString.Substring(this.readedString.IndexOf("\r\n") + 2);
            }

            locName = locName.Substring(0, locName.IndexOf("\r"));

            //EXITS
            String locExits = this.readedString.Substring(this.readedString.IndexOf(":"));
            String locDesc = locExits;
            locExits = locExits.Substring(0, locExits.IndexOf("\n")); 

            //DECRIPTION
            locDesc = locDesc.Substring(locDesc.IndexOf("\r"));

            readRersult.locationName = locName;
            readRersult.locationDescription = locDesc;

            if (locExits.Contains("north")) readRersult.exit_n = Dir.north;
            if (locExits.Contains("south")) readRersult.exit_s = Dir.south;
            if (locExits.Contains("east")) readRersult.exit_e = Dir.east;
            if (locExits.Contains("west")) readRersult.exit_w = Dir.west;

            return readRersult;

        }
    }
}
