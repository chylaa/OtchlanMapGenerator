using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        ToolTip tip = new ToolTip();
        KeyHandler ghk_r, ghk_n, ghk_s, ghk_e, ghk_w;

        int timesKeyPressed = 0;
        String keyBuffer="";
        int dx = 0;
        int dy = 0;
        int newID=1; // start from 1 becouse there is always starting element 

        public Form1()
        {
            InitializeComponent();
            ghk_n = new KeyHandler(Keys.N, this);
            ghk_s = new KeyHandler(Keys.S, this);
            ghk_e = new KeyHandler(Keys.E, this);
            ghk_w = new KeyHandler(Keys.W, this);
            ghk_r = new KeyHandler(Keys.Enter, this);
            ghk_n.Register();
            ghk_s.Register();
            ghk_e.Register();
            ghk_w.Register();
            ghk_r.Register();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //confirmButton.Enabled=false;
            //textboxName.Enabled = false;
            addPanel.Enabled = false;
            if (SegList.segments.Count() > 0)
            {
                SegmentClicked((SegList.segments.First()));
            }
        }
        //=================Handling painting bitmaps on form======================================
        private void DisplaySegments()
        {
            this.Invalidate();
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
            SegList.segments[chosen.id].description = chosen.description;
            //updateDescription()
            //updateBitmap()
            //update...
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
            chosen.description = textboxName.Text;
        }


        //==================Handling ScrollBars=================================
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            dy = e.NewValue;
            
            addPanel.Text = "dx: " + dx + " dy: " + dy;
            SegList.UpdateSegmentsLocationScroll(dx, dy,dx,e.OldValue);
            DisplaySegments();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            dx = e.NewValue;
            addPanel.Text = "dx: " + dx + " dy: " + dy;
            SegList.UpdateSegmentsLocationScroll(dx, dy,e.OldValue,dy);
            DisplaySegments();
        }

        //=====================Handling adding new Segments========================
        private void Form1_KeyPress(KeyPressEventArgs e)
        {
            int Added = -1;
    
            //!Poblem with commands -> must add recognition - is it only "w\r" or maybe "ekw\r" ??
            //!Maybe second buffer "prevKeys" which monitores whats before - could be added in begining of HandleHotkey() and get m.WParam()
            if (timesKeyPressed >= 2)
            {
                if (keyBuffer[0] == '\r') //protection from deadlock
                {
                    keyBuffer = keyBuffer[1].ToString();
                    timesKeyPressed = 1;
                }
                else
                {
                    keyBuffer = "";
                    timesKeyPressed = 0;
                }
            }

            keyBuffer += e.KeyChar;
            addPanel.Text = keyBuffer;
            timesKeyPressed++;
            Added = SegList.CheckKeyBuffer(newID, keyBuffer, chosen);
            if (Added == 1)
            {
                newID++;
                keyBuffer = "";
                timesKeyPressed = 0;
                SegmentClicked(SegList.findSegment(chosen));
            }
            if(Added==0 || Added ==1) DisplaySegments();

        }
        private void HandleHotkey(Message m)
        {
            //message.wParam with the GetHashCode of the KeyHandler
            if (m.WParam.ToInt32() == ghk_n.GetHashCode()) Form1_KeyPress(new KeyPressEventArgs('n'));
            if (m.WParam.ToInt32() == ghk_s.GetHashCode()) Form1_KeyPress(new KeyPressEventArgs('s'));
            if (m.WParam.ToInt32() == ghk_e.GetHashCode()) Form1_KeyPress(new KeyPressEventArgs('e'));
            if (m.WParam.ToInt32() == ghk_w.GetHashCode()) Form1_KeyPress(new KeyPressEventArgs('w'));
            if (m.WParam.ToInt32() == ghk_r.GetHashCode()) Form1_KeyPress(new KeyPressEventArgs('\r'));
        }

        protected override void WndProc(ref Message m)
        {
            
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
                HandleHotkey(m);
            base.WndProc(ref m);
        }

        //private string mapKeyCode(string keyCode)
        //{
        //    keyCode=keyCode.ToLower();
        //    if (keyCode == "return") keyCode = "\r";
        //    return keyCode;
        //}

        private void Form1_Activated(object sender, EventArgs e)
        {

            //RaiseKeyEvent(sender, new KeyEventArgs(Keys.Enter));
            //addPanel.Enabled = false;
        }





        //=========TODO: Player position (new Segment playerSeg? -> user can still select with mouse other segment)==============
    }
    
}