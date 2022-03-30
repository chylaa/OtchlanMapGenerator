using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace OtchlanMapGenerator
{

    public partial class Form1 : Form
    {
        
        Map SegMap;

        Segment chosen = new Segment();
        ToolTip tip = new ToolTip();
        KeyHandler keyHandler = new KeyHandler();

        ScreenRead screenRead = new ScreenRead();
        Boolean flag_previousSegmentAdded = true;

        String keyBuffer = "";

        Rectangle outlineSelect;
        Boolean flag_DrawOutline = false;

        Color brownThemeMainColor = Color.Brown;
        Color brownThemePanelColor = Color.RosyBrown;
        Color brownThemeButtonsColor = Control.DefaultBackColor;
        Color blackThemeMainColor = Color.Black;
        Color blackThemePanelColor = Color.Green;
        Color blackThemeButtonsColor = Control.DefaultBackColor;

        int dx = 0;
        int dy = 0;
        int descBoxHeight = 24;
        Boolean flag_findWayBitmapsActive = false;
        Boolean flag_keyInputAcive = true;
        Boolean flag_makeScreen= true;
        Boolean flag_firstDeploy = true;
        char playerOrientation = 'x'; //n,s,e,w or x if player_pos not shown.
        char lastDir = 'x'; //last move direction 
        int currentFloor; //Floor to display, by default - floor where player is located

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

            Texts.setLanguage(Language.EN); //must be before new Map() - using Texts.DeflautLocationName
            setTexts();

            SegMap = new Map();
            initializeMap();

            textboxName.Enabled = false;
            segmentPanel.Enabled = true;
        }

        private void initializeMap()
        {
            currentFloor = 0;
            SegMap.newID = 1;
            if (SegMap.segments.Count() > 0)
            {
                SegmentClicked((SegMap.segments.First()));
                SegMap.playerSeg = SegMap.segments.First();
            }

            SegMap.MainMapColor = brownThemeMainColor;
            SegMap.PanelMapColor = brownThemePanelColor;

            SegMap.centerCameraOnPlayerSegment(this.Size, segmentPanel.Size);
        }

        private void resetFlags()
        {
            flag_DrawOutline = false;
            flag_findWayBitmapsActive = false;
            flag_keyInputAcive = true;
            flag_makeScreen = true;
            flag_previousSegmentAdded = true;
            flag_firstDeploy = true;
        }
        //================================Language things============================================
        private void setTexts()
        {
            this.Text = Texts.text_MainFormName;
            detailButton.Text = Texts.text_detailButton;
            segmentPanel.Text = Texts.text_segmentPanel;
            textboxName.Text = Texts.text_textboxName;
            exitsLabel.Text = Texts.text_exitsLabel;
            deleteButton.Text = Texts.text_deleteButton;
            infoLabel.Text = Texts.text_infoLabel;
            languageGroupBox.Text = Texts.text_languageGroupBox;
            descriptionTextBox.Text = Texts.text_descriptionTextBox;
            floorTextBox.Text = Texts.text_floorTextBox + currentFloor;

            mapFileToolStripMenuItem.Text = Texts.text_menuMap;
            newToolStripMenuItem.Text = Texts.text_menuMapNewFile;
            SaveAsToolStripMenuItem.Text = Texts.text_menuMapSaveFileAs;
            saveToolStripMenuItem.Text = Texts.text_menuMapSaveFile;
            openToolStripMenuItem.Text = Texts.text_menuMapOpenFile;
            vievToolStripMenuItem.Text = Texts.text_menuViev;
            searchToolStripMenuItem.Text = Texts.text_menuSearch;
            colorsToolStripMenuItem.Text = Texts.text_menuVievColors;
            panelColorToolStripMenuItem.Text = Texts.text_menuVievColorsPanelColor;
            mainColorToolStripMenuItem.Text = Texts.text_menuVievColorsMainColor;
            brownThemeToolStripMenuItem.Text = Texts.text_menuVievColorsBrownTheme;
            blackGreenThemeToolStripMenuItem.Text = Texts.text_menuVievColorsBlackTheme;
            helpToolStripMenuItem.Text = Texts.text_menuHelp;
            usageToolStripMenuItem.Text = Texts.text_menuHelpUsage;
        }
        private void ENradioButton_CheckedChanged(object sender, EventArgs e)
        {
            SegmentClicked(SegMap.playerSeg); // to not accidentally change name of selected location
            SegMap.activeLanguage = Language.EN;
            Texts.setLanguage(SegMap.activeLanguage);
            setTexts();
            keyInputCheckBox_CheckedChanged(sender, e); //to set proper text associated with checkbox
            exitsLabel.Left = 72;
            
            //return activeLanguage;
        }

        private void PLradioButton1_CheckedChanged(object sender, EventArgs e)
        {
            SegmentClicked(SegMap.playerSeg); // to not accidentally change name of selected location
            SegMap.activeLanguage = Language.PL;
            Texts.setLanguage(SegMap.activeLanguage);
            setTexts();
            keyInputCheckBox_CheckedChanged(sender, e); //to set proper text associated with checkbox
            exitsLabel.Left = 66;
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
            nButton.Focus();  //to remove focus from delete button - otherwise each click on "enter" would invoke it.
            DisplaySegments(playerOrientation, true);

        }

        private void floorButtonUP_Click(object sender, EventArgs e)
        {
            currentFloor++;
            floorTextBox.Text = Texts.text_floorTextBox + currentFloor;
            DisplaySegments('x', false);
        }

        private void floorButtonDown_Click(object sender, EventArgs e)
        {
            currentFloor--;
            floorTextBox.Text = Texts.text_floorTextBox + currentFloor;
            DisplaySegments('x',false);
        }


        //=================Handling clicking on segments to display info and select==================================
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            descriptionTextBox.Visible = false;

            //DisplaySegments();
            foreach (Segment segment in SegMap.segments)
            {
                // if (segment.bitmap == null) return;
                if (segment.floor == currentFloor)
                {
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
            }
            HideOutlineSelected();
            DisplaySegments(playerOrientation, false);
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            foreach (Segment segment in SegMap.segments)
            {
                if (segment.floor == currentFloor)
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
            }
            DisplaySegments(playerOrientation, true); //Double click on background return viev to player segment
        }

        private void SegmentClicked(Segment s)
        {
            segmentPanel.Enabled = true;
            nButton.Focus(); //to keep focus away from delete and detail buttons
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
            chosen.assignValues(s); 

            if (s.exits.eN == Dir.north) SetButtonStyle(s, nButton, "N");
            if (s.exits.eS == Dir.south) SetButtonStyle(s, sButton, "S");
            if (s.exits.eE == Dir.east) SetButtonStyle(s, eButton, "E");
            if (s.exits.eW == Dir.west) SetButtonStyle(s, wButton, "W");
            if (s.exits.eU == Dir.up) upButton.BackColor = Color.IndianRed;
            if (s.exits.eD == Dir.down) downButton.BackColor = Color.IndianRed;

            //infoLabel.Text = "ID: " + s.id + " Dist: " + s.distance + " | " + s.exits.neighbourIDn + "N " + s.exits.neighbourIDs + "S " + s.exits.neighbourIDe + "E " + s.exits.neighbourIDw + "W " + s.exits.neighbourIDu + "U " + s.exits.neighbourIDd + "D " + s.floor+"=Floor";

        }
        private void SegmentDoubleClicked(Segment s)
        {
            routeTextBox.Visible = true;
            SegmentClicked(s);
            infoLabel.Text = "";

            routeTextBox.Text = SegMap.FindWay(SegMap.playerSeg, SegMap.findSegment(chosen));
            Clipboard.SetText(routeTextBox.Text.Replace(',','\r')+'\r'); //replace all commas with new line in routeString and copy to clippboard 
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
            nButton.Text = "n";
            nButton.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            nButton.BackColor = brownThemeButtonsColor;
            sButton.Text = "s";
            sButton.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            sButton.BackColor = brownThemeButtonsColor;
            eButton.Text = "e";
            eButton.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            eButton.BackColor = brownThemeButtonsColor;
            wButton.Text = "w";
            wButton.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            wButton.BackColor = brownThemeButtonsColor;
            upButton.BackColor = brownThemeButtonsColor;
            downButton.BackColor = brownThemeButtonsColor;
        }
        //--------------Atribute changes-----------------

        private void disableKeyInput(object sender, EventArgs e) { flag_keyInputAcive = false; }
        private void enableKeyInput(object sender, EventArgs e)
        {
            textboxName.Enabled = false;
            nButton.Focus();

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

        //=================Handling painting bitmaps on form==========================================================
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
            foreach (Segment segment in SegMap.segments)
                if(segment.floor == currentFloor)
                    e.Graphics.DrawImage(segment.bitmap, segment.BMPlocation);

            if (flag_DrawOutline) e.Graphics.DrawRectangle(new Pen(Color.Yellow, 2.0f), outlineSelect);
            flag_DrawOutline = false;
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
            flag_DrawOutline = true;
            DisplaySegments(playerOrientation, false);
        }

        private void HideOutlineSelected() { outlineSelect = new Rectangle(); }

        //==================Handling ScrollBars and MouseWheel======================================================
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            dy = e.NewValue;

            //segmentPanel.Text = "dx: " + dx + " dy: " + dy;
            SegMap.UpdateSegmentsLocationScroll(dx, dy, dx, e.OldValue);

            DrawOutlineForSelected(SegMap.findSegment(chosen));
            DisplaySegments(playerOrientation, false);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            dx = e.NewValue;
            //segmentPanel.Text = "dx: " + dx + " dy: " + dy;
            SegMap.UpdateSegmentsLocationScroll(dx, dy, e.OldValue, dy);

            DrawOutlineForSelected(SegMap.findSegment(chosen));
            DisplaySegments(playerOrientation, false);
        }

        private void Form1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Temporary disabled due to problems
            //segmentPanel.Text = e.Delta.ToString(); //e.Delta: int from -120 to 120 
            //SegMap.ChangeSegmentSizes(e.Delta);

            //DrawOutlineForSelected(SegList.findSegment(chosen));
            DisplaySegments('x', false);

        }

        //===================Handling adding new Segments and keyboard read================================================
        private void Form1_KeyPress(KeyPressEventArgs e) 
        {
            int Added = -1;

            if (keyHandler.addCharToSequence(e.KeyChar) == false) return; //must be before chceckKeyState()!

            keyBuffer = keyHandler.chceckKeySequence(); //Last chars are 1:1 writed sequence 

            //here screenshot if e.keyChar == '\r' and set flag -> if flag setted and keyBuffer[0] not dir dont screen -> 
            // -> if flag setted and Dir command reset flag // Make atribute in ScreenRead to hold screenshot and use CaptureScreen() method

            //Make screenshot - each enter after direction command and first time after non-dir command to store locatioon info 
            if(keyHandler.keywordDetectedFlag==false && flag_makeScreen && !flag_firstDeploy && e.KeyChar == '\r') //rethink that onceagain?
            {
                if(screenRead.CaptureScreen()==false)
                {
                    MessageBox.Show(Texts.msg_GameProcessNotFound, Texts.msg_MessageBoxTitle);
                    //return; // comment this to test wihout game process
                }
                flag_makeScreen = false;
            }
            if(keyHandler.keywordDetectedFlag)
            {
                if (flag_makeScreen)
                {
                    if (screenRead.CaptureScreen() == false)
                    {
                        MessageBox.Show(Texts.msg_GameProcessNotFound, Texts.msg_MessageBoxTitle);
                        //return; //comment this to test wihout game process 
                    }
                }
                flag_firstDeploy = false;
                flag_makeScreen = true;
            }

            currentFloor=SegMap.playerSeg.floor; //and thats all, change floor viev by user, - same variable, just viev returns to player on move
            floorTextBox.Text = Texts.text_floorTextBox + currentFloor;

            Added = SegMap.CheckKeyBuffer(keyBuffer, currentFloor); // here adding happens
            if (Added == -1) return;

            if(keyBuffer[0]!='u' && keyBuffer[0]!='d') 
                lastDir = keyBuffer[0];

            if (flag_previousSegmentAdded)
            {
                void SetInfo() { if(SegMap.findSegment(SegMap.previousSegment)!= null )SegMap.findSegment(SegMap.previousSegment).setSegmentInfo(screenRead.getLocationInfo()); }
                Thread thread = new Thread(SetInfo);
                thread.Start();

                flag_previousSegmentAdded = false;
            }

            if (Added == 1)  //new segment added                             
            {
                SegMap.newID++;
                keyHandler.clearKeysSequence();
                flag_previousSegmentAdded = true;
            }

            routeTextBox.Visible = false; //hide routeTextBox if player moved
            currentFloor = SegMap.playerSeg.floor; //in case player changed floors
            SegmentClicked(SegMap.playerSeg);
            DisplaySegments(lastDir, true);
            playerOrientation = lastDir;
        }

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
            this.Text = Texts.text_MainFormName + " - " + openFileDialog.SafeFileName; //set form name to open file name

            this.BackColor = SegMap.MainMapColor;
            segmentPanel.BackColor = SegMap.PanelMapColor;
            descriptionTextBox.BackColor = SegMap.PanelMapColor;
            setTextColors();

            currentFloor = SegMap.playerSeg.floor;

            resetFlags();
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
                this.Text = Texts.text_MainFormName + " - " + SegMap.MapFileName;
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
            resetFlags();
            DisplaySegments(playerOrientation, true);

        }
        //------colors---------

        private void mainColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.SolidColorOnly = true;
                colorDialog.ShowDialog();

                this.BackColor = colorDialog.Color;
                SegMap.MainMapColor = colorDialog.Color;

                setTextColors();
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

                setTextColors();
            }    
        }

        private void setTextColors()
        {
            //determine if backgroud is too dark for black text
            if ((SegMap.MainMapColor.R * 0.299) + (SegMap.MainMapColor.G * 0.587) + (SegMap.MainMapColor.B * 0.114) < 64)
            {
                keyInputCheckBox.ForeColor = Color.White;
            }
            else
            {
                keyInputCheckBox.ForeColor = Color.Black;
            }
            if ((SegMap.PanelMapColor.R * 0.299) + (SegMap.PanelMapColor.G * 0.587) + (SegMap.PanelMapColor.B * 0.114) < 64)
            {
                segmentPanel.ForeColor = Color.White;
                languageGroupBox.ForeColor = Color.White;
                foreach (Control c in segmentPanel.Controls)
                {
                    if (c.Name.Contains("Button")) c.BackColor = Color.Black;
                }
            }
            else
            {
                segmentPanel.ForeColor = Color.Black;
                languageGroupBox.ForeColor = Color.Black;
                foreach (Control c in segmentPanel.Controls)
                {
                    if (c.Name.Contains("Button")) c.BackColor = Control.DefaultBackColor;
                }
            }
        }

        //----------Search----------------
        private void setBrownThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = brownThemeMainColor;
            segmentPanel.BackColor = brownThemePanelColor;
            descriptionTextBox.BackColor = brownThemePanelColor;
            SegMap.MainMapColor = brownThemeMainColor;
            SegMap.PanelMapColor = brownThemePanelColor;
            keyInputCheckBox.ForeColor = Color.Black;

            setTextColors();
        }

        private void blackGreenThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = blackThemeMainColor;
            segmentPanel.BackColor = blackThemePanelColor;
            descriptionTextBox.BackColor = blackThemePanelColor;
            SegMap.MainMapColor = blackThemeMainColor;
            SegMap.PanelMapColor = blackThemePanelColor;
            keyInputCheckBox.ForeColor = Color.White;

            setTextColors();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag_keyInputAcive = false;
            
            using (Form searchForm = new Form())
            {
                SearchLocationUserControl searchControl = new SearchLocationUserControl();
                searchControl.Show();
                searchControl.BackColor = SegMap.PanelMapColor;

                searchForm.MaximumSize = new System.Drawing.Size(325, 200);
                searchForm.MinimumSize = new System.Drawing.Size(325, 200);
                searchForm.BackColor = SegMap.MainMapColor;
                searchForm.Width = 325;
                searchForm.Height = 200;
                searchForm.Controls.Add(searchControl);
                searchForm.ActiveControl = searchControl;
                searchForm.Activate();
                searchForm.ShowDialog();
                
                String searched = "";
                if (searchControl.WasButtonClicked())
                {
                    searched = searchControl.getSearchedText();
                }

                if (searched.Length != 0) findLocationContainingString(searched); //add "Not foud" message or smth
                searchControl.Hide();
            }
            flag_keyInputAcive = true;
        }

        private void findLocationContainingString(String searched)
        {
            searched = searched.ToLower();
            foreach (Segment s in SegMap.segments )
            {
                if(s.name.ToLower().Contains(searched) || s.decription.ToLower().Contains(searched))
                {
                    SegmentClicked(s);
                    detailButton.PerformClick();
                    return;
                }
            }
            MessageBox.Show(Texts.msg_LocationNotFound,Texts.msg_MessageBoxTitle);
        }

    }
}