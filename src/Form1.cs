﻿using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace OtchlanMapGenerator
{

    public partial class Form1 : Form
    {
        
        //Segment segment = new Segment();
        Map SegMap;
        Segment chosen = new Segment();
        ToolTip tip = new ToolTip();
        Language activeLanguage = Language.EN;
        KeyHandler keyHandler = new KeyHandler();

        ScreenRead screenRead = new ScreenRead();
        ReadResult readResult = new ReadResult();
        Boolean previousSegmentAdded = true;

        int timesKeyPressed = 0;
        String keyBuffer = "";

        Rectangle outlineSelect;
        Boolean flagDrawOutline = false;

        Color deflautMainColor = Color.Brown;
        Color deflautPanelColor = Color.RosyBrown;

        int dx = 0;
        int dy = 0;
        int newID = 1; // start from 1 becouse there is always starting element 
        int descBoxHeight = 24;
        Boolean flag_findWayBitmapsActive = false;
        Boolean flag_keyInputAcive = true;
        char playerOrientation = 'x'; //n,s,e,w or x if player_pos not shown.

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.openFileDialog.Filter = Texts.mapFileExtentionPattern;
            this.openFileDialog.InitialDirectory = @"Saves\";
            this.openFileDialog.DefaultExt = Texts.mapFileExtention;
            this.saveFileDialog.Filter = Texts.mapFileExtentionPattern;
            this.saveFileDialog.InitialDirectory = @"Saves\";
            this.saveFileDialog.DefaultExt = Texts.mapFileExtention;

            
            //confirmButton.Enabled=false;
            //textboxName.Enabled = false;
            Texts.setLanguage(activeLanguage);
            setTexts();
            SegMap = new Map();

            textboxName.Enabled = false;
            segmentPanel.Enabled = false;

            initializeMap();
        }

        private void initializeMap()
        {
            newID = 1;
            if (SegMap.segments.Count() > 0)
            {
                SegmentClicked((SegMap.segments.First()));
                SegMap.playerSeg = SegMap.segments.First();
            }

            SegMap.MainMapColor = deflautMainColor;
            SegMap.PanelMapColor = deflautPanelColor;

            SegMap.centerCameraOnPlayerSegment(this.Size, segmentPanel.Size);
        }
        //================================Language things============================================
        private void setTexts()
        {

            this.Text = Texts.text_FormName;
            detailButton.Text = Texts.text_detailButton;
            segmentPanel.Text = Texts.text_segmentPanel;
            textboxName.Text = Texts.text_textboxName;
            exitsLabel.Text = Texts.text_exitsLabel;
            deleteButton.Text = Texts.text_deleteButton;
            infoLabel.Text = Texts.text_infoLabel;
            languageGroupBox.Text = Texts.text_languageGroupBox;
            descriptionTextBox.Text = Texts.text_descriptionTextBox;


            mapFileToolStripMenuItem.Text = Texts.text_menuMap;
            newToolStripMenuItem.Text = Texts.text_menuMapNewFile;
            SaveAsToolStripMenuItem.Text = Texts.text_menuMapSaveFileAs;
            saveToolStripMenuItem.Text = Texts.text_menuMapSaveFile;
            openToolStripMenuItem.Text = Texts.text_menuMapOpenFile;
            vievToolStripMenuItem.Text = Texts.text_menuViev;
            colorsToolStripMenuItem.Text = Texts.text_menuVievColors;
            panelColorToolStripMenuItem.Text = Texts.text_menuVievColorsPanelColor;
            mainColorToolStripMenuItem.Text = Texts.text_menuVievColorsMainColor;
            helpToolStripMenuItem.Text = Texts.text_menuHelp;
            usageToolStripMenuItem.Text = Texts.text_menuHelpUsage;

        }
        private void ENradioButton_CheckedChanged(object sender, EventArgs e)
        {
            activeLanguage = Language.EN;
            Texts.setLanguage(activeLanguage);
            setTexts();
            //return activeLanguage;
        }

        private void PLradioButton1_CheckedChanged(object sender, EventArgs e)
        {
            activeLanguage = Language.PL;
            Texts.setLanguage(activeLanguage);
            setTexts();
            //return activeLanguage;
        }

        //===================================Buttons Handling=======================================================
        private void detailButton_Click(object sender, EventArgs e)
        {
            descriptionTextBox.Visible = true;
            textboxName.Enabled = true;
        }


        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (SegMap.segments.Count <= 1 || chosen.id == SegMap.playerSeg.id) return;

            SegMap.DeleteSegment(chosen);

            HideOutlineSelected();
            chosen.assignValues(SegMap.playerSeg);
            button_set_n.Focus();  //to remove focus from delete button - otherwise each click on "enter" would invoke it.
            DisplaySegments(playerOrientation, true);

        }


        //=================Handling clicking on segments to display info and select=================================================
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            descriptionTextBox.Visible = false;

            //DisplaySegments();
            foreach (Segment segment in SegMap.segments)
            {
                // if (segment.bitmap == null) return;              
                if (e.Location.X > segment.BMPlocation.X && e.Location.X < segment.BMPlocation.X + segment.bitmap.Width)
                {
                    if (e.Location.Y > segment.BMPlocation.Y && e.Location.Y < segment.BMPlocation.Y + segment.bitmap.Height)
                    {
                        if (e.Button == MouseButtons.Right) setPlayerAtSegment(segment); 
                        SegmentClicked(segment);
                        return;
                    }
                }
            }
            HideOutlineSelected();
            DisplaySegments(playerOrientation, false);
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            foreach (Segment segment in SegMap.segments)
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
            DisplaySegments(playerOrientation, true); //Double click on background return viev to player segment
        }

        private void SegmentClicked(Segment s)
        {
            segmentPanel.Enabled = true;
            button_set_n.Focus(); //to keep focus away from delete and detail buttons
            SetDefaultButtonsStyle();

            if (flag_findWayBitmapsActive)
            {
                flag_findWayBitmapsActive = false;
                routeTextBox.Text = "";
                foreach (Segment seg in SegMap.segments) seg.setBitmap('x', 'x'); //clear route showing 
            }

            DrawOutlineForSelected(s);
            
            textboxName.Text = s.name;
            descriptionTextBox.Text = s.decription;
            chosen.assignValues(s); //after "conmfirm" button clicked if chosen.id = s.id -> s=chosen??

            if (s.exits.eN == Dir.north) SetButtonStyle(s, button_set_n, "N");
            if (s.exits.eS == Dir.south) SetButtonStyle(s, button_set_s, "S");
            if (s.exits.eE == Dir.east) SetButtonStyle(s, button_set_e, "E");
            if (s.exits.eW == Dir.west) SetButtonStyle(s, button_set_w, "W");

            infoLabel.Text = "ID: " + s.id + " Dist: " + s.distance + " | " + s.exits.neighbourIDn + "N " + s.exits.neighbourIDs + "S " + s.exits.neighbourIDe + "E " + s.exits.neighbourIDw + "W";

        }
        private void SegmentDoubleClicked(Segment s)
        {
            routeTextBox.Visible = true;
            SegmentClicked(s);
            infoLabel.Text = "";

            routeTextBox.Text = SegMap.FindWay(SegMap.playerSeg, SegMap.findSegment(chosen));
            DisplaySegments(playerOrientation, false);

            flag_findWayBitmapsActive = true;
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
        //--------------Atribute changes-----------------

        private void disableKeyInput(object sender, EventArgs e) { flag_keyInputAcive = false; }
        private void enableKeyInput(object sender, EventArgs e)
        {
            textboxName.Enabled = false;
            button_set_n.Focus();

            chosen.name = textboxName.Text;
            chosen.decription = descriptionTextBox.Text;
            SegMap.findSegment(chosen).assignValues(chosen);

            flag_keyInputAcive = true; 
        }
        //correcting size of descriptionTextBox when multiple lines
        private void descriptionTextBox_TextChanged(object sender, EventArgs e)
        {
            String desc = descriptionTextBox.Text;
            int newLineCount = 1; // \n
            int newLineCount2 = 1; // \r
            foreach (char c in desc) if (c == '\n') newLineCount++;
            foreach (char c in desc) if (c == '\r') newLineCount2++;
            newLineCount = Math.Max(newLineCount, newLineCount2);
            int adjustment = (descBoxHeight/3) * (newLineCount - 1);
            int ySize = (descBoxHeight * newLineCount) - adjustment - (newLineCount - 1)*1;
            descriptionTextBox.Height = ySize + 6; // + 6 just in case :) 
        }
        private void textboxName_TextChanged(object sender, EventArgs e)
        {

        }
        private void setPlayerAtSegment(Segment clickedSegment)
        {
            Segment newPlayerSeg = SegMap.findSegment(clickedSegment);
            newPlayerSeg.bitmap = SegMap.playerSeg.bitmap;
            SegMap.playerSeg.setBitmap('x', 'x'); //sets normal bitmap
            SegMap.playerSeg = newPlayerSeg;
            DisplaySegments(playerOrientation,true);
        }

        //=================Handling painting bitmaps on form======================================
        private void DisplaySegments(char from, Boolean centerOnPlayer) //param: char from - describe direction from which player came. Can be n/e/s/w or 'x' (none) if player hasn't moved. 
        {
            descriptionTextBox.Visible = false;
            playerOrientation = from;
            SegMap.playerSeg.setPlayerBitmap(from);
            if (centerOnPlayer)
            {
                SegMap.centerCameraOnPlayerSegment(this.Size, segmentPanel.Size);
                hScrollBar1.Value = hScrollBar1.Maximum / 2;
                vScrollBar1.Value = vScrollBar1.Maximum / 2;
            }
            this.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //for(int i=0;i<SegList.segments.Count;i++) if(SegList.segments[i].bitmap!=null) e.Graphics.DrawImage(SegList.segments[i].bitmap, SegList.segments[i].BMPlocation);
            foreach (Segment segment in SegMap.segments) e.Graphics.DrawImage(segment.bitmap, segment.BMPlocation);
            if (flagDrawOutline) e.Graphics.DrawRectangle(new Pen(Color.Yellow, 2.0f), outlineSelect);
            flagDrawOutline = false;
        }
        private void Form1_SizeChanged(object sender, EventArgs e) //Camera follows player segment  
        {
            DrawOutlineForSelected(SegMap.findSegment(chosen));
            DisplaySegments(playerOrientation, true);
        }

        private void DrawOutlineForSelected(Segment selected)
        {
            if (SegMap.playerSeg == null || selected==null) return;
            if (selected.id == SegMap.playerSeg.id){
                HideOutlineSelected(); 
            }else{
                outlineSelect = new Rectangle(selected.BMPlocation, selected.bitmap.Size);

            }
            flagDrawOutline = true;
            DisplaySegments(playerOrientation, false);
        }

        private void HideOutlineSelected() { outlineSelect = new Rectangle(); }

        //==================Handling ScrollBars and MouseWheel=================================
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            dy = e.NewValue;

            segmentPanel.Text = "dx: " + dx + " dy: " + dy;
            SegMap.UpdateSegmentsLocationScroll(dx, dy, dx, e.OldValue);

            DrawOutlineForSelected(SegMap.findSegment(chosen));
            DisplaySegments(playerOrientation, false);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            dx = e.NewValue;
            segmentPanel.Text = "dx: " + dx + " dy: " + dy;
            SegMap.UpdateSegmentsLocationScroll(dx, dy, e.OldValue, dy);

            DrawOutlineForSelected(SegMap.findSegment(chosen));
            DisplaySegments(playerOrientation, false);
        }

        private void Form1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            segmentPanel.Text = e.Delta.ToString(); //e.Delta: int from -120 to 120 
            SegMap.ChangeSegmentSizes(e.Delta);

            //DrawOutlineForSelected(SegList.findSegment(chosen));
            DisplaySegments('x', false);

        }



        //===================Handling adding new Segments and keyboard read========================
        private void Form1_KeyPress(KeyPressEventArgs e)
        {
            //setTexts(); //reset description text
            int Added = -1;
            char lastDir = 'x';

            if (keyHandler.addCharToSequence(e.KeyChar) == false) return; //must be before chceckKeyState()!

            keyBuffer = keyHandler.chceckKeySequence(); //Last chars are 1:1 writed sequence 

            //charsSinceEnter += keyBuffer;   //just control
            segmentPanel.Text += keyHandler.keysSinceEnter; //just control

            Added = SegMap.CheckKeyBuffer(newID, keyBuffer); // here adding happens
            if (Added == -1) return;

            lastDir = keyBuffer[0];

            if (previousSegmentAdded)
            {
                //SegList.findSegment(SegList.previousSegment).setSegmentInfo(screenRead.getLocationInfo());

                void SetInfo() { SegMap.findSegment(SegMap.previousSegment).setSegmentInfo(screenRead.getLocationInfo()); }
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
            SegmentClicked(SegMap.playerSeg);
            DisplaySegments(lastDir, true);
            playerOrientation = lastDir;
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
            char KeyCode = keyHandler.keyboardScan(); //must be above keyInputActive flag check, to not read unprocesed keys
            if (flag_keyInputAcive == false) return;
            if (ModifierKeys.HasFlag(Keys.Control)) return; //ignore key input with ctrl for e.g ctrl+s, ctrl+o ect.
            if (KeyCode != '0') Form1_KeyPress(new KeyPressEventArgs(KeyCode));

        }

        private void keyInputCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (keyInputCheckBox.Checked == true)
            {
                flag_keyInputAcive = false;
                keyInputCheckBox.Text = Texts.text_enableKeyInput;
            }
            else
            {
                flag_keyInputAcive = true;
                keyInputCheckBox.Text = Texts.text_disableKeyInput;
            }

        }

        //============================Menu===================================
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag_keyInputAcive = false;
            openFileDialog.ShowDialog();
            flag_keyInputAcive = true;
        }

        private void openFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Map tempMap = MapSave.LoadMapFromFile(openFileDialog.FileName);
            if (tempMap == null)
            {
                MessageBox.Show(Texts.msg_FileReadError);
                flag_keyInputAcive = true;
                return;
            }
            SegMap = MapSave.LoadMapFromFile(openFileDialog.FileName);
            SegMap.MapFilePath = openFileDialog.FileName;
            SegMap.MapFileName = openFileDialog.SafeFileName;
            this.Text = Texts.text_FormName + " - " + openFileDialog.SafeFileName; //set form name to open file name

            this.BackColor = SegMap.MainMapColor;
            segmentPanel.BackColor = SegMap.PanelMapColor;
            descriptionTextBox.BackColor = SegMap.PanelMapColor;

            DisplaySegments(playerOrientation, true);
        }



        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag_keyInputAcive = false;
            saveFileDialog.ShowDialog();           
            flag_keyInputAcive = true;
        }
        private void saveFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SegMap.MapFilePath = saveFileDialog.FileName;
            SegMap.MapFileName = SegMap.MapFilePath.Substring(SegMap.MapFilePath.LastIndexOf("\\")+1);
            if (MapSave.SaveMapToFile(SegMap.MapFilePath,SegMap))
            {
                flag_keyInputAcive = true;
                this.Text = Texts.text_FormName + " - " + SegMap.MapFileName;
                return;
            }
            else
            {
                flag_keyInputAcive = true;
                MessageBox.Show(Texts.msg_FileSaveError);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag_keyInputAcive = false;
            if (SegMap.MapFilePath.Length > 0)
            {
                MapSave.SaveMapToFile(SegMap.MapFilePath, SegMap);
            }
            else
            {
                SaveAsToolStripMenuItem_Click(sender, e);
            }
            flag_keyInputAcive = true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SegMap = new Map();
            initializeMap();
            DisplaySegments(playerOrientation, true);

        }

        private void mainColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.SolidColorOnly = true;
                colorDialog.ShowDialog();

                this.BackColor = colorDialog.Color;
                SegMap.MainMapColor = colorDialog.Color; 

            }
        }

        private void panelColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.SolidColorOnly = true;
                colorDialog.ShowDialog();

                segmentPanel.BackColor = colorDialog.Color;
                descriptionTextBox.BackColor = colorDialog.Color;
                SegMap.PanelMapColor = colorDialog.Color;

            }
        }

        private void resetColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = deflautMainColor;
            segmentPanel.BackColor = deflautPanelColor;
            descriptionTextBox.BackColor = deflautPanelColor;
            SegMap.MainMapColor = deflautMainColor;
            SegMap.PanelMapColor = deflautPanelColor;
        }
    }
}