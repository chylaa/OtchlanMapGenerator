using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace OtchlanMapGenerator
{

    public partial class Form1 : Form
    {
        //Segment segment = new Segment();
        ListOfSegments SegList;
        Segment chosen = new Segment();
        //Segment playerSeg = new Segment();
        //Segment previousSeg;
        ToolTip tip = new ToolTip();
        Text texts;
        KeyHandler keyHandler = new KeyHandler();

        ScreenRead screenRead = new ScreenRead();
        ReadResult readResult = new ReadResult();
        Boolean previousSegmentAdded = true;

        int timesKeyPressed = 0;
        String keyBuffer="";
        
        int dx = 0;
        int dy = 0;
        int newID=1; // start from 1 becouse there is always starting element 
        Boolean findWayBitmapsActive = false;
        Boolean keyInputAcive = true;
        char playerOrientation = 'x'; //n,s,e,w or x if player_pos not shown.

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //confirmButton.Enabled=false;
            //textboxName.Enabled = false;
            setTexts(Language.EN);
            SegList = new ListOfSegments(texts);
            
            segmentPanel.Enabled = false;

            if (SegList.segments.Count() > 0)
            {
                SegmentClicked((SegList.segments.First()));
                SegList.playerSeg = SegList.segments.First();
            }

            SegList.centerCameraOnPlayerSegment(this.Size, segmentPanel.Size);
        }
        //================================Language things============================================
        private void setTexts(Language ln)
        {
            texts = new Text(ln);

            this.Text = texts.text_FormName;
            correctButton.Text = texts.text_correctButton;
            segmentPanel.Text = texts.text_segmentPanel;
            textboxName.Text = texts.text_textboxName;
            exitsLabel.Text = texts.text_exitsLabel;
            deleteButton.Text = texts.text_deleteButton;
            infoLabel.Text = texts.text_infoLabel;
            languageGroupBox.Text = texts.text_languageGroupBox;
        }
        private void ENradioButton_CheckedChanged(object sender, EventArgs e)
        {
            setTexts(Language.EN);
        }

        private void PLradioButton1_CheckedChanged(object sender, EventArgs e)
        {
            setTexts(Language.PL);
        }

        //===================================Buttons Handling=======================================================
        private void correctButton_Click(object sender, EventArgs e)
        {

            chosen.name = textboxName.Text;
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
            correctButton.Focus();  //to remove focus from delete button - otherwise each click on "enter" would invoke it.
            DisplaySegments('x',true);
           
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

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            foreach (Segment segment in SegList.segments)
            {
                if (e.Location.X > segment.BMPlocation.X && e.Location.X < segment.BMPlocation.X + segment.bitmap.Width)
                {
                    if (e.Location.Y > segment.BMPlocation.Y && e.Location.Y < segment.BMPlocation.Y + segment.bitmap.Height)
                    {
                        SegmentDoubleClicked(segment);
                        return;
                    }
                }
            }
            DisplaySegments('x', true); //Double click on background return viev to player segment
        }

        private void SegmentClicked(Segment s)
        {
            segmentPanel.Enabled = true;
            SetDefaultButtonsStyle();

            if (findWayBitmapsActive)
            {
                findWayBitmapsActive = false;
                routeTextBox.Text = "";
                foreach (Segment seg in SegList.segments) seg.setBitmap('x', 'x'); //clear route showing 
            }

            textboxName.Text = s.name;
            chosen.assignValues(s); //after "conmfirm" button clicked if chosen.id = s.id -> s=chosen??

            if (s.exits.e1 == Dir.north) SetButtonStyle(s, button_set_n, "N");
            if (s.exits.e2 == Dir.south) SetButtonStyle(s, button_set_s, "S");
            if (s.exits.e3 == Dir.east) SetButtonStyle(s, button_set_e, "E");
            if (s.exits.e4 == Dir.west) SetButtonStyle(s, button_set_w, "W");

            infoLabel.Text = "ID: " + s.id + " Dist: " +s.distance +" | "+ s.exits.neighbourID1 + "N " + s.exits.neighbourID2 + "S " + s.exits.neighbourID3 + "E " + s.exits.neighbourID4 + "W";
            
        }
        private void SegmentDoubleClicked(Segment s)
        {
            routeTextBox.Visible = true;
            SegmentClicked(s);
            infoLabel.Text = "";

            routeTextBox.Text = SegList.FindWay(SegList.playerSeg, SegList.findSegment(chosen));
            DisplaySegments(playerOrientation,false);
            
            findWayBitmapsActive = true;
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

        //=================Handling painting bitmaps on form======================================
        private void DisplaySegments(char from, Boolean centerOnPlayer) //param: char from - describe direction from which player came. Can be n/e/s/w or 'x' (none) if player hasn't moved. 
        {
            playerOrientation = from;
            SegList.playerSeg.setPlayerBitmap(from);
            if (centerOnPlayer)
            {
                SegList.centerCameraOnPlayerSegment(this.Size, segmentPanel.Size);
                hScrollBar1.Value = hScrollBar1.Maximum / 2;
                vScrollBar1.Value = vScrollBar1.Maximum / 2;
            }
            this.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //for(int i=0;i<SegList.segments.Count;i++) if(SegList.segments[i].bitmap!=null) e.Graphics.DrawImage(SegList.segments[i].bitmap, SegList.segments[i].BMPlocation);
            foreach (Segment segment in SegList.segments) e.Graphics.DrawImage(segment.bitmap, segment.BMPlocation);

        }
        private void Form1_SizeChanged(object sender, EventArgs e) //Camera follows player segment  
        {
            DisplaySegments('x',true);
        }

        //==================Handling ScrollBars and MouseWheel=================================
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            dy = e.NewValue;
            
            segmentPanel.Text = "dx: " + dx + " dy: " + dy;
            SegList.UpdateSegmentsLocationScroll(dx, dy,dx,e.OldValue);
            DisplaySegments('x',false);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            dx = e.NewValue;
            segmentPanel.Text = "dx: " + dx + " dy: " + dy;
            SegList.UpdateSegmentsLocationScroll(dx, dy,e.OldValue,dy);
            DisplaySegments('x',false);
        }

        private void Form1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            segmentPanel.Text = e.Delta.ToString(); //e.Delta: int from -120 to 120 
            SegList.ChangeSegmentSizes(e.Delta);
            DisplaySegments('x',false);
            
        }

        //===================Handling adding new Segments and keyboard read========================
        private void Form1_KeyPress(KeyPressEventArgs e)  
        {

            int Added = -1;
            char lastDir = 'x';

            if (keyHandler.addCharToSequence(e.KeyChar) == false) return; //must be before chceckKeyState()!
                   
            keyBuffer=keyHandler.chceckKeySequence(); //Last chars are 1:1 writed sequence 

            //charsSinceEnter += keyBuffer;   //just control
            segmentPanel.Text += keyHandler.keysSinceEnter; //just control

            Added = SegList.CheckKeyBuffer(newID, keyBuffer); // here adding happens
            if (Added == -1) return;

            lastDir = keyBuffer[0];

            if (previousSegmentAdded)
            {
                //SegList.findSegment(SegList.previousSegment).setSegmentInfo(screenRead.getLocationInfo());

                void SetInfo() { SegList.findSegment(SegList.previousSegment).setSegmentInfo(screenRead.getLocationInfo()); }
                Thread thread = new Thread(SetInfo);
                thread.Start();

                //label1.Text = SegList.findSegment(SegList.previousSegment).decription; // just control
                previousSegmentAdded = false;
            }

            if (Added == 1)  //new segment added                             
            {
                newID++;
                keyHandler.clearKeysSequence();
                previousSegmentAdded = true;
                    
                //timesKeyPressed = 0;
                //SegList.playerSeg = SegList.segments.Last();
                //SegList.ResetBitmapSizes();
            }

            routeTextBox.Visible = false; //hide routeTextBox if player moved
            SegmentClicked(SegList.playerSeg);
            DisplaySegments(lastDir,true);
            segmentPanel.Text = "";

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
            if (keyInputAcive == false) return;
            char KeyCode = keyHandler.keyboardScan();
            if(KeyCode != '0') Form1_KeyPress(new KeyPressEventArgs(KeyCode));

        }

        private void keyInputCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(keyInputCheckBox.Checked == true)
            {
                keyInputAcive = true;
                keyInputCheckBox.Text = "Disable keys input";
            }
            else
            {
                keyInputAcive = false;
                keyInputCheckBox.Text = "Enable keys input";
            }
            
        }

    }
    
}