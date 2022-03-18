using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace OtchlanMapGenerator
{

    public static class Constants
    {
        ////windows message id for hotkey
        //public const int WM_HOTKEY_MSG_ID = 0x0312;

        //HEX
        public const int VK_ENTER = 0xD; //This is the enter key.
        public const int VK_BACKSPACE = 0x8; //This is the backspace key.
        public const int VK_NORTH = 0x4E; //This is the n key.
        public const int VK_SOUTH = 0x53; //This is the s key.
        public const int VK_EAST = 0x45; //This is the e key.
        public const int VK_WEST = 0x57; //This is the w key.
    }

    class KeyHandler
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        public String keysSinceEnter = "";
        char directionKeyword;

        public Boolean addCharToSequence(char keyChar) //returns false if Char not added (was Backspace)
        {
            if(keyChar == Constants.VK_BACKSPACE && this.keysSinceEnter.Length>0)
            {
                this.keysSinceEnter = this.keysSinceEnter.Substring(0, this.keysSinceEnter.Length - 1);
                return false;
            }

            this.keysSinceEnter += keyChar;
            this.keysSinceEnter=this.keysSinceEnter.ToLower();
            return true;
        }
        public void clearKeysSequence() //clears string (using after adding new map segment)
        {
            this.keysSinceEnter = "";
        }

        public string chceckKeySequence()
        {
            //this.deleteDuplicats();
            if(this.keysSinceEnter.Contains('\r'))//if (this.keysSinceEnter.Length >= 2)
            {
                //below return is enough for this "if" statment if user use only n/s/e/w letters for moving - rest will handle entering "south", "sou", "nort" etc.
                //return this.keysSinceEnter.Substring(this.keysSinceEnter.Length - 2); //return last 2 characters
                if (keywordDetected())
                {
                    return (directionKeyword + "\r");
                }
                else
                {
                    this.clearKeysSequence(); //clear if oher command was entered
                }
            }
            return this.keysSinceEnter;
        }

        private Boolean keywordDetected()
        {

            int strLength = this.keysSinceEnter.Length-1;// -1 to not include potential ENTER
            int compatibility = 0;


            char first = this.keysSinceEnter[0];
            String keyword = choseKeyword(first);
            if (keyword.Length == 0 || strLength > keyword.Length) return false;

            for(int i=0; i<strLength;i++) // 
            {
                if (this.keysSinceEnter[i].Equals(keyword[i])) compatibility++;
            }
            if (compatibility == strLength && this.keysSinceEnter[strLength] == '\r')
            {
                this.clearKeysSequence();
                directionKeyword = first;
                return true;
            }
            return false;
        }
        //whole method not nessesary even wrong because it causes for example "sssssss" as "s"
        private void deleteDuplicats() //eesseweseesw
        {
            //string chars = "";
            //int howManyDiferentChars = 0;

            //for(int i=0; i<this.keysSinceEnter.Length;i++)
            //{
            //    if (!chars.Contains(this.keysSinceEnter[i]))
            //    {
            //        chars += this.keysSinceEnter[i];
            //        howManyDiferentChars++;
            //    }

            //}

            //this.keysSinceEnter = this.keysSinceEnter.Substring(this.keysSinceEnter.Length-howManyDiferentChars);
            //==============================================================================
            char last;
            string newKeySequence = "";

            for (int i = 1; i < this.keysSinceEnter.Length; i++)
            {
                last = this.keysSinceEnter[i - 1];
                if (last == this.keysSinceEnter[i]) continue;
                newKeySequence += last;
            }
            newKeySequence += this.keysSinceEnter[this.keysSinceEnter.Length - 1];
            this.keysSinceEnter = newKeySequence;
        }

        private string choseKeyword(char first)
        {
            if (first == 'n') return "north";
            if (first == 's') return "south";
            if (first == 'e') return "east";
            if (first == 'w') return "west";
            return "";
        }

        public char keyboardScan()
        {
            short keyState;
            bool unprocessedPress;

            for (int i = 64; i < 91; i++) //loop from a to z in ASCII
            {
                keyState = GetAsyncKeyState(i); 
                unprocessedPress = ((keyState >> 0) & 0x0001) == 0x0001;  //Check if the LSB is set. If so, then the key was pressed since the last call to GetAsyncKeyState

                if (unprocessedPress)
                {
                    return (char)(i);
                }
            }
            keyState = GetAsyncKeyState(Constants.VK_ENTER);
            unprocessedPress = ((keyState >> 0) & 0x0001) == 0x0001;
            if(unprocessedPress) return ('\r');

            keyState = GetAsyncKeyState(Constants.VK_BACKSPACE);
            unprocessedPress = ((keyState >> 0) & 0x0001) == 0x0001;
            if (unprocessedPress) return (char)(Constants.VK_BACKSPACE);

            return '0'; // no essential keys were pressed
        }
            
    }
}
