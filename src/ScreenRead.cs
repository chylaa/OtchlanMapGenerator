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
        public Dir exit_u;
        public Dir exit_d;
        public Boolean invalid; // to inform if Reasult was readed properly
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
        //int TempIteration = 0;
        //String ssPath;
        IronTesseract ironTesseract;
        public Bitmap capturedBitmap;
        String readedString;
        Boolean FirstDeploy = true;

        public ScreenRead()
        {
            this.ironTesseract = new IronTesseract();
            this.ironTesseract.MultiThreaded = true;
            this.ironTesseract.Language = OcrLanguage.PolishFast;
            this.ironTesseract.AddSecondaryLanguage(OcrLanguage.EnglishFast);

            this.readedString = "";
        }

        public Boolean CaptureScreen() //returns false if program has problem with capturing screen of game process
        {
            if (Process.GetProcessesByName("otchlan_starter").Length == 0)
            {
                MessageBox.Show(Texts.msg_GameProcessNotFound);
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

                this.capturedBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                Graphics.FromImage(capturedBitmap).CopyFromScreen(rect.left, rect.top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);

                //this.TempIteration++;
                //bitmap.Save("screen" + TempIteration +".jpg", ImageFormat.Jpeg);
                //bitmap.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }
        Boolean getTextFromScreen() // //returns false if program has problem with capturing screen of game process 
        {
           // if(!this.CaptureScreen()) return false;

            var result = this.ironTesseract.Read(this.capturedBitmap);
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
            try
            {
                if (this.readedString.Contains("$$") && !(this.readedString.Contains(">"))) return false; //if this is starting screen
                this.readedString = this.readedString.Substring(0,this.readedString.LastIndexOfAny("<".ToCharArray()) - 1); //
                if (!FirstDeploy) this.readedString = this.readedString.Substring(this.readedString.LastIndexOfAny(">".ToCharArray()));
            }catch(Exception e)
            {
                MessageBox.Show(Texts.msg_ReadError);
                return false;
            }

            this.FirstDeploy = false;
            return true;
        }

        public ReadResult getLocationInfo()
        {
            ReadResult readResult = new ReadResult();
            readResult.exit_n = Dir.not;
            readResult.exit_s = Dir.not;
            readResult.exit_e = Dir.not;
            readResult.exit_w = Dir.not;
            readResult.exit_u = Dir.not;
            readResult.exit_d = Dir.not;

            if (!this.getTextFromScreen())
            {
                readResult.invalid = true;
                return readResult;
            }
            if (!this.readedString.Contains(":"))
            {
                readResult.invalid = true;
                return readResult;
            }

            //LOCATION NAME
            String locName;
            if (this.readedString.Substring(0,10).Contains("\r\n\r\n"))
            {
                locName = this.readedString.Substring(this.readedString.IndexOf("\r\n") + 4);
            }
            else
            {
                locName = this.readedString.Substring(this.readedString.IndexOf("\r\n") + 2);
            }

            locName = locName.Substring(0, locName.IndexOf("\r"));

            //EXITS
            
            String locExits = this.readedString.Substring(this.readedString.LastIndexOf(":")); //(":")
            String locDesc = locExits;
            locExits = locExits.Substring(0, locExits.IndexOf("\n")); 

            //DECRIPTION
            locDesc = locDesc.Substring(locDesc.IndexOf("\r"));
            locDesc=locDesc.Replace("\r\n\r\n", "");
            locDesc=locDesc.Replace("\n\r\n\r", "");
            locDesc=locDesc.Replace("\r\n\r", "");
            locDesc=locDesc.Replace("\n\r\n", "");
            //for (int i = 0; i < 4; i++) if (locDesc[i] == '\r' || locDesc[i] == '\n') locDesc=locDesc.Substring(i+1);
            //for (int i = locDesc.Length; i > locDesc.Length - 4; i--) if (locDesc[i] == '\r' || locDesc[i] == '\n') locDesc = locDesc.Substring(0,i-1);

            readResult.locationName = locName;
            readResult.locationDescription = locDesc;

            if (locExits.Contains("north")) readResult.exit_n = Dir.north; //: east west north south up \r
            if (locExits.Contains("south")) readResult.exit_s = Dir.south;
            if (locExits.Contains("east")) readResult.exit_e = Dir.east;
            if (locExits.Contains("west")) readResult.exit_w = Dir.west;
            if (locExits.Contains("up")) readResult.exit_u = Dir.up;
            if (locExits.Contains("down")) readResult.exit_d = Dir.down;

            if (locDesc.Contains("<") || 
                locDesc.Contains(">") ||
                (readResult.exit_n == Dir.not &&
                readResult.exit_s == Dir.not &&
                readResult.exit_e == Dir.not &&
                readResult.exit_w == Dir.not &&
                readResult.exit_u == Dir.not &&
                readResult.exit_d == Dir.not))
            {
                readResult.invalid = true;
            }
            else
            {
                readResult.invalid = false;
            }

            return readResult;

        }
    }
}

//[N] Nowa gra
//[Z] Załaduj poprzednią grę
//[W] Wyjście

//1.Micha Mag Poz:0 2.Test Mag Poz:0

//e.Menu
//bierz numer zapisu: 2

//Skrzyżowanie

//Wyjścia: east west north south

//Stoisz na skrzyżowaniu ulic, będącym jednocześnie dużym placem. Na zachód
//przechodzi on w niewielki targ, za którym stoi świątynia. Na południe
//niewielka uliczka biegnie wzdłuż muru. Na północ plac kończy się przy rzece,
//Jza którą stoi jakaś rezydencja.
//  ^^^
//throws {"Długość nie może być mniejsza od zera.\r\nNazwa parametru: length"} exception //KINDA DONE??? Check again
//make converting more "step by step" - not in one line
//also when first deploy may be neccesary to change rules of detecting and cutting proper string
//((make change to not include in cutted string < ... > - players stats ? it will be easier to have the same code for first and next deploys))
// (maybe also second set of protection? for eg. detecting "OTHLAN"-capition elements ? )
//
