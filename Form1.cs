using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace OtchlanMapGenerator
{

    public partial class Form1 : Form
    {
        //Segment segment = new Segment();
        ListOfSegments SegList = new ListOfSegments();
        Segment chosen = new Segment();
        //Segment playerSeg = new Segment();
        //Segment previousSeg;
        ToolTip tip = new ToolTip();
        
        
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        int timesKeyPressed = 0;
        String keyBuffer="";
        String allKeys = "";
        int dx = 0;
        int dy = 0;
        int newID=1; // start from 1 becouse there is always starting element 

        public Form1()
        {
            InitializeComponent();
            //ghk_n = new KeyHandler(Keys.N, this);
            //ghk_s = new KeyHandler(Keys.S, this);
            //ghk_e = new KeyHandler(Keys.E, this);
            //ghk_w = new KeyHandler(Keys.W, this);
            //ghk_r = new KeyHandler(Keys.Enter, this);
            //ghk_n.Register();
            //ghk_s.Register();
            //ghk_e.Register();
            //ghk_w.Register();
            //ghk_r.Register();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //confirmButton.Enabled=false;
            //textboxName.Enabled = false;
            addPanel.Enabled = false;
            if (SegList.segments.Count() > 0)
            {
                SegmentClicked((SegList.segments.First()));
                //SegList.playerSeg.assignValues(SegList.segments.First());
                SegList.playerSeg = SegList.segments.First();
            }
        }
        //=================Handling painting bitmaps on form======================================
        private void DisplaySegments(char from) //param: char from - describe direction from which player came. Can be n/e/s/w or 'x' (none) if player hasn't moved. 
        {
            this.Invalidate();
            SegList.playerSeg.setPlayerBitmap(from);
            //dodac obsluge scrollowania - zeby odejmowalo odpowiednia ilosc od BMPLocation przy scrollu aby bylo widac tylko te co ma byc widac
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //for(int i=0;i<SegList.segments.Count;i++) if(SegList.segments[i].bitmap!=null) e.Graphics.DrawImage(SegList.segments[i].bitmap, SegList.segments[i].BMPlocation);
            foreach (Segment segment in SegList.segments) e.Graphics.DrawImage(segment.bitmap, segment.BMPlocation);
            
        }
        //==========================================================================================
        private void confirmButton_Click(object sender, EventArgs e)
        {
            chosen.description = textboxName.Text;
            chosen.assignValues(SegList.findSegment(chosen));
            //updateDescription()
            //updateBitmap()
            //update...
        }


        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (SegList.segments.Count <= 1 || chosen.id==SegList.playerSeg.id) return;
            
            SegList.segments.Remove(SegList.findSegment(chosen));
            chosen.assignValues(SegList.playerSeg);
            confirmButton.Focus();  //to remove focus from delete button - otherwise each click on "enter" would invoke it.
            DisplaySegments('x');
            
            //TODO Erase exits of neighbours

        }


            //=================Handling clicking on segments to display info and select=================================================
            private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            //DisplaySegments();
            foreach (Segment segment in SegList.segments)
            {
                // if (segment.bitmap == null) return;              
                if (e.Location.X > segment.BMPlocation.X && e.Location.X < segment.BMPlocation.X + segment.bitmap.Width)
                {
                    if (e.Location.Y > segment.BMPlocation.Y && e.Location.Y < segment.BMPlocation.Y + segment.bitmap.Height)
                    {
                        SegmentClicked(segment);
                    }
                }
            }
        }

        private void SegmentClicked(Segment s)
        {
            addPanel.Enabled = true;
            SetDefaultButtonsStyle();

            // TODO: CHANGES ONLY ON CHOSEN AND ONE METHOD "UPDATE_FORM_CHOSEN" AFTER EACH CHANGE
            textboxName.Text = s.description;
            chosen.assignValues(s); //after "conmfirm" button clicked if chosen.id = s.id -> s=chosen??

            if (s.exits.e1 == Dir.north) SetButtonStyle(s, button_set_n, "N");
            if (s.exits.e2 == Dir.south) SetButtonStyle(s, button_set_s, "S");
            if (s.exits.e3 == Dir.east) SetButtonStyle(s, button_set_e, "E");
            if (s.exits.e4 == Dir.west) SetButtonStyle(s, button_set_w, "W");



        }
        private void SetButtonStyle(Segment s, Button b, string x)
        {
                b.Text = x;
                b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                b.BackColor = Color.IndianRed;
        }

        private void SetDefaultButtonsStyle()
        {
            button_set_n.Text = "n";
            button_set_n.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            button_set_n.BackColor = Control.DefaultBackColor;
            button_set_s.Text = "s";
            button_set_s.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            button_set_s.BackColor = Control.DefaultBackColor;
            button_set_e.Text = "e";
            button_set_e.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            button_set_e.BackColor = Control.DefaultBackColor;
            button_set_w.Text = "w";
            button_set_w.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            button_set_w.BackColor = Control.DefaultBackColor;
        }

        private void textboxName_TextChanged(object sender, EventArgs e)
        {

        }


        //==================Handling ScrollBars and MouseWheel=================================
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            dy = e.NewValue;
            
            addPanel.Text = "dx: " + dx + " dy: " + dy;
            SegList.UpdateSegmentsLocationScroll(dx, dy,dx,e.OldValue);
            DisplaySegments('x');
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            dx = e.NewValue;
            addPanel.Text = "dx: " + dx + " dy: " + dy;
            SegList.UpdateSegmentsLocationScroll(dx, dy,e.OldValue,dy);
            DisplaySegments('x');
        }

        private void Form1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            addPanel.Text = e.Delta.ToString(); //e.Delta: int from -120 to 120 
            SegList.ChangeSegmentSizes(e.Delta);
            DisplaySegments('x');
            
        }

        //=====================Handling adding new Segments========================
        private void Form1_KeyPress(KeyPressEventArgs e)
        {
            int Added = -1;
            char lastDir = 'x';
            if (keyBuffer.Length > 2) keyBuffer = keyBuffer.Substring(keyBuffer.Last() - 2);
            //!Poblem with commands -> must add recognition - is it only "w\r" or maybe "ekw\r" ??
            //!Maybe second buffer "prevKeys" which monitores whats before - could be added in begining of HandleHotkey() and get m.WParam()
            if(keyBuffer.Length==2 && keyBuffer[0].Equals(keyBuffer[1])) //protection from gettin keys too fast
            {
                if (keyBuffer[0] == '\r' || keyBuffer[0] == 'n' || keyBuffer[0] == 's' || keyBuffer[0] == 'e' || keyBuffer[0] == 'w')
                {
                    keyBuffer = keyBuffer[1].ToString();
                    timesKeyPressed = 1;
                }        
            }
            if (timesKeyPressed >= 2)
            {
                keyBuffer = "";
                timesKeyPressed = 0;
            }

            keyBuffer += e.KeyChar;
            allKeys += keyBuffer;
            addPanel.Text = keyBuffer;
            timesKeyPressed++;
            Added = SegList.CheckKeyBuffer(newID, keyBuffer); 
            if (Added == 1)                               
            {
                newID++;
                lastDir=keyBuffer[0];
                keyBuffer = "";
                timesKeyPressed = 0;
                //SegList.playerSeg = SegList.segments.Last();
                SegmentClicked(SegList.playerSeg);
                //SegList.ResetBitmapSizes();
            }
            if(Added==0 || Added ==1) DisplaySegments(lastDir);

        }
        //private void HandleHotkey(Message m)  //problem - this method "takes" all keyboard action for itself
        //{
        //    //message.wParam with the GetHashCode of the KeyHandler
        //    if (m.WParam.ToInt32() == ghk_n.GetHashCode()) Form1_KeyPress(new KeyPressEventArgs('n'));
        //    if (m.WParam.ToInt32() == ghk_s.GetHashCode()) Form1_KeyPress(new KeyPressEventArgs('s'));
        //    if (m.WParam.ToInt32() == ghk_e.GetHashCode()) Form1_KeyPress(new KeyPressEventArgs('e'));
        //    if (m.WParam.ToInt32() == ghk_w.GetHashCode()) Form1_KeyPress(new KeyPressEventArgs('w'));
        //    if (m.WParam.ToInt32() == ghk_r.GetHashCode()) Form1_KeyPress(new KeyPressEventArgs('\r'));
        //}

        //protected override void WndProc(ref Message m)
        //{
            
        //    if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
        //        HandleHotkey(m);
        //    base.WndProc(ref m);
        //}

        //private string mapKeyCode(string keyCode)
        //{
        //    keyCode=keyCode.ToLower();
        //    if (keyCode == "return") keyCode = "\r";
        //    return keyCode;
        //}

        private void timer1_Tick(object sender, EventArgs e)
        {
            //zeruj wszystkie key state
            //ooo albo lepiej tworzyc je wszystkie tu
            short keyState_r = GetAsyncKeyState(Constants.VK_ENTER);
            short keyState_n = GetAsyncKeyState(Constants.VK_NORTH);
            short keyState_s = GetAsyncKeyState(Constants.VK_SOUTH);
            short keyState_e = GetAsyncKeyState(Constants.VK_EAST);
            short keyState_w = GetAsyncKeyState(Constants.VK_WEST);

            //Check if the MSB is set. If so, then the key is pressed.
            bool R_IsPressed = ((keyState_r >> 15) & 0x0001) == 0x0001;
            bool N_IsPressed = ((keyState_n >> 15) & 0x0001) == 0x0001;
            bool S_IsPressed = ((keyState_s >> 15) & 0x0001) == 0x0001;
            bool E_IsPressed = ((keyState_e >> 15) & 0x0001) == 0x0001;
            bool W_IsPressed = ((keyState_w >> 15) & 0x0001) == 0x0001;

            //Check if the LSB is set. If so, then the key was pressed since
            //the last call to GetAsyncKeyState
            bool unprocessedPress_r = ((keyState_r >> 0) & 0x0001) == 0x0001;
            bool unprocessedPress_n = ((keyState_r >> 0) & 0x0001) == 0x0001;
            bool unprocessedPress_s = ((keyState_r >> 0) & 0x0001) == 0x0001;
            bool unprocessedPress_e = ((keyState_r >> 0) & 0x0001) == 0x0001;
            bool unprocessedPress_w = ((keyState_r >> 0) & 0x0001) == 0x0001;


            //HandleKeys(unprocessedPress_r || R_IsPressed,
            //           unprocessedPress_n || N_IsPressed,
            //           unprocessedPress_s || S_IsPressed,
            //           unprocessedPress_e || E_IsPressed,
            //           unprocessedPress_w || W_IsPressed);
            HandleKeys(R_IsPressed, N_IsPressed, S_IsPressed, E_IsPressed, W_IsPressed);
            //HandleKeys(unprocessedPress_r, unprocessedPress_n, unprocessedPress_s, unprocessedPress_e, unprocessedPress_w);
            //if (R_IsPressed) //|| unprosesedPress??
            //{
            //    //TODO Execute client code...
            //}

            //if (unprocessedPress_r)
            //{
            //    //TODO Execute client code...
            //}
        }


        private int HandleKeys(bool pressedR, bool pressedN, bool pressedS, bool pressedE, bool pressedW)
        {
            if (!(pressedR || pressedN || pressedS || pressedE || pressedW)) return 0;

            if(pressedR)
            {
                Form1_KeyPress(new KeyPressEventArgs('\r'));
                return 1;
            }
            if (pressedN)
            {
                Form1_KeyPress(new KeyPressEventArgs('n'));
                return 1;
            }
            if (pressedS)
            {
                Form1_KeyPress(new KeyPressEventArgs('s'));
                return 1;
            }
            if (pressedE)
            {
                Form1_KeyPress(new KeyPressEventArgs('e'));
                return 1;
            }
            if (pressedW)
            {
                Form1_KeyPress(new KeyPressEventArgs('w'));
                return 1;
            }
            return 0;

        }






        //=========TODO: Player position (new Segment SegList.playerSeg? -> user can still select with mouse other segment)==============
    }
    
}