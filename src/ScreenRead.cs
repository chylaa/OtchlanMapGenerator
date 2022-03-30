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
        Boolean captureScreenOK = true;

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
                captureScreenOK = false;
                return false;
            }

            Process proc = Process.GetProcessesByName("otchlan_starter")[0];

            try
            {
                IntPtr procHnd = proc.MainWindowHandle;
                SetForegroundWindow(procHnd);
                //ShowWindow(procHnd, 9); //Activates and displays the window. 9 - If the window is minimized or maximized, the system restores it to its original size and position. 

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
            catch (Exception)
            {
                captureScreenOK = false;
                return false;
            }
            captureScreenOK = true;
            return true;
        }
        Boolean getTextFromScreen() // //returns false if program has problem with capturing screen of game process 
        {
            if(!this.captureScreenOK) return false;

            var result = this.ironTesseract.Read(this.capturedBitmap);
            this.readedString = result.Text;
            Boolean flag_firstDeploy = false;
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
                if (countChar62InReadedString() < 2) flag_firstDeploy = true;
                this.readedString = this.readedString.Substring(0,this.readedString.LastIndexOfAny("<".ToCharArray()) - 1); //
                if (!flag_firstDeploy) this.readedString = this.readedString.Substring(this.readedString.LastIndexOfAny(">".ToCharArray()));
            }catch(Exception e)
            {
                MessageBox.Show(Texts.msg_ReadError + ": " +e.Message);
                return false;
            }

            //this.FirstDeploy = false;
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
            readResult.invalid = true; //only false if makes to the end of method 

            if (!this.getTextFromScreen())
                return readResult;

            if (!this.readedString.Contains(":"))
                return readResult;

            //if (checkForSentence("Tam niestety nie pójdziesz!", 80))
               // return readResult;

            //LOCATION NAME
            if (this.readedString.Contains("$$")) cutStringToStartFromLastNumber();

            String locName;
            if (this.readedString.Substring(0,10).Contains("\r\n\r\n"))
            {
                locName = this.readedString.Substring(this.readedString.IndexOf("\r\n") + 4);
            }
            else
            {
                if (this.readedString.Contains("\r\n"))
                {
                    locName = this.readedString.Substring(this.readedString.IndexOf("\r\n") + 2);
                }
                else { return readResult; }
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

            //validation check 
            if (locDesc.Contains("<") || 
                locDesc.Contains(">") ||
                (readResult.exit_n == Dir.not &&
                readResult.exit_s == Dir.not &&
                readResult.exit_e == Dir.not &&
                readResult.exit_w == Dir.not &&
                readResult.exit_u == Dir.not &&
                readResult.exit_d == Dir.not))
            {
                return readResult;
            }

            readResult.invalid = false;

            return readResult;

        }

        private Boolean checkForSentence(String sentence, int accuracy) //returns true if specified amount of chars are in right place (int accuracy is min precent of right chars)
        {
            int count = 0;
            //if (accuracy > 100) accuracy = 100; if(accuracy<0) accuracy=0;
            if (readedString.Contains(sentence)) return true;
            for(int i=0; i<sentence.Length; i++)
            {
                if (i > readedString.Length) break;
                if (this.readedString[i] == sentence[i]) count++;
            }
            if ((count/sentence.Length)*100 > accuracy)
                return true;
            return false;
        }

        private int countChar62InReadedString()
        {
            int count=0;
            foreach (char c in this.readedString)
            {
                if (c == '>') count++;
            }
            return count;
        }
        private void cutStringToStartFromLastNumber()
        {
            int i = 0;
            int LastPos = 0;
            foreach (char c in this.readedString)
            {
                i++;
                if (c > '0' && c < '9') LastPos = i;
            }

            this.readedString = this.readedString.Substring(LastPos);
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


//===========Before adding new flag_firstDeploy in line 121

//after first move:


//| Otchłań — m s
//Sprawdzanie plikow. ..Ok A
//Inicjalizacja ustawien. . .Ok
//Deklaracja mobow. . .Ok
//Deklaracja przedmiotow. . .Ok

//S STH JE c
//.S$$$5. RZ © LG $'
//.SSS' "SSS, SssOssS = KIEJ
//c "5 45% .S$$$$55. $5.S$5. $SS'  .55S. "$s 5$5.
//S, oe $S' .S .S$s .S $.  $S' .S
//CEJĄ , Ś5 $5$ SS, LE S'$$s 5, Ek) LG Sy
//"SEE, „Sk $S$ SSS. .SSS $5 5) $s $5, .S. LG S
//ooh SS Sole $S$ +55 eS 'Ss .s$. 'S$$$5 $s .s$. '$5
//LP:
//Pi
//Wersja 1.3 v76
//[N] Nowa gra
//[Z] Załaduj poprzednią grę
//[W] Wyjście
//1.Micha

//after second move:

//| Otchłań — m s
//Sprawdzanie plikow. ..Ok A
//Inicjalizacja ustawien. . .Ok
//Deklaracja mobow. . .Ok
//Deklaracja przedmiotow. . .Ok

//S STH JE c
//.S$$$5. RZ © LG $'
//.SSS' "SSS, SssOssS = KIEJ
//c "5 45% .S$$$$55. $5.S$5. $SS'  .55S. "$s 5$5.
//S, oe $S' .S .S$s .S $.  $S' .S
//CEJĄ , Ś5 $5$ SS, LE S'$$s 5, Ek) LG Sy
//"SEE, „Sk $S$ SSS. .SSS $5 5) $s $5, .S. LG S
//ooh SS Sole $S$ +55 eS 'Ss .s$. 'S$$$5 $s .s$. '$5
//LP:
//Pi
//Wersja 1.3 v76
//[N] Nowa gra
//[Z] Załaduj poprzednią grę
//[W] Wyjście
//1.Micha Mag Poz:0 2.Test Mag Poz:0
//e.Menu
//Wybierz numer zapisu: 2
//Skrzyżowanie

//Ayjścia: east west north south

//Stoisz na skrzyżowaniu ulic, będącym jednocześnie dużym placem. Na zachód
//przechodzi on w niewielki targ, za którym stoi świątynia. Na południe
//niewielka uliczka biegnie wzdłuż muru. Na północ plac kończy się przy rzece,
//za którą stoi jakaś rezydencja.

//<18hp 128m 96mv 78exp>s

//Ulica Murna

//dyjścia: west north south

//a ulica biegnie wzdłuż muru wschodniego i ciągnie się na południe.

//Na zachód jest inna ulica, ale nie tak ładna jak ta. Eleganckie latarnie

//ciągną się wzdłuż niej, a kostka brukowa jest rzeźbiona w jakieś fantastyczne
//zory.

//<18hp 128m 95mv 78exp>s,