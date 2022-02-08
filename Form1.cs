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

        int timesKeyPressed = 0;
        String keyBuffer="";
        int dx = 0;
        int dy = 0;
        int newID=13; // after implementing adding this must start from 0 

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //confirmButton.Enabled=false;
            //textboxName.Enabled = false;
            addPanel.Enabled = false;
        }
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

        private void confirmButton_Click(object sender, EventArgs e)
        {
            //DisplaySegments();
            // for(int i=0; i=SegList.segments.Count; i++) if (SegList.segments[i].id == chosen.id)  = chosen;
            SegList.segments[chosen.id].description = chosen.description;
            //updateDescription()
            //updateBitmap()
            //update...
        }


        //private void Form1_MouseMove(object sender, MouseEventArgs e) //zmienic z mouse move na timer co 100? zapisywac czy mysz kliknieta i jak tak to ->segmentClicked ->clicked=false
        //{
        //    bool wasShown = false;
        //    Point p;
        //        foreach (Segment segment in SegList.segments)
        //        {
        //        if (MouseButtons.Left == e.Button) return;
        //            if (segment.bitmap == null) return;
        //            p = segment.BMPlocation;
        //            if (e.Location.X > p.X && e.Location.X < p.X + segment.bitmap.Width)
        //            {
        //                if (e.Location.Y > p.Y && e.Location.Y < p.Y + segment.bitmap.Height)
        //                {
        //                    //if (MouseButtons.Left == e.Button)
        //                    //{
        //                    //    SegmentClicked(segment);
        //                    //    return;
        //                    //}
        //                    tip.ToolTipIcon = ToolTipIcon.None;
        //                    //tip.AutomaticDelay = 10;
        //                    //tip.UseFading = true;
        //                    tip.InitialDelay = 10;
        //                    p.Offset(segment.bitmap.Width, segment.bitmap.Height);
        //                    tip.Show(segment.description, this, p);
        //                    wasShown = true;
        //                }

        //            }
        //        }
            
        //    System.Threading.Thread.Sleep(200);
        //    if (wasShown)
        //    {
        //        tip.Hide(this);
        //        wasShown = false;
        //    }
        //}
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
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

            textboxName.Text = s.description;
            chosen.description = s.description; //after "conmfirm" button clicked if chosen.id = s.id -> s=chosen??
            chosen.id = s.id;
            chosen.BMPlocation = s.BMPlocation;

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

        //private void Form1_MouseClick(object sender, MouseEventArgs e)
        //{
        //    foreach (Segment s in SegList.segments)
        //    {
        //        if (e.Location.X > s.BMPlocation.X && e.Location.X < s.BMPlocation.X + s.bitmap.Width)
        //        {
        //            if (e.Location.Y > s.BMPlocation.Y && e.Location.Y < s.BMPlocation.Y + s.bitmap.Height)
        //            {
        //                textboxName.Text = s.description;
        //                chosen.description = s.description; //after "conmfirm" button clicked if chosen.id = s.id -> s=chosen??
        //                chosen.id = s.id;

        //                addPanel.Enabled = true;
        //                //confirmButton.Enabled = true;
        //                //textboxName.Enabled = true;
        //            }
        //        }
        //    }

        //}
        private void textboxName_TextChanged(object sender, EventArgs e)
        {
            chosen.description = textboxName.Text;
        }

        private void addPanel_Enter(object sender, EventArgs e)
        {
            addPanel.Enabled = true;
        }

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

        //Handling adding new Segments
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //keyBuffer += e.KeyCode.ToString();
            //if (keyBuffer.Length > 2) keyBuffer = "";
            //CheckKeyBuffer();
        }
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (timesKeyPressed >= 2)
            {
                keyBuffer = "";
                timesKeyPressed = 0;
            }
            keyBuffer += e.KeyChar;
            timesKeyPressed++;
            if(SegList.CheckKeyBuffer(newID,keyBuffer,chosen)==1)
            {
                newID++;
                keyBuffer = "";
                timesKeyPressed = 0;
                DisplaySegments();
            }
        }



        //=============================================================
    }
    
}