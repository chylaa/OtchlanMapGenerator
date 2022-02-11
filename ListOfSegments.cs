using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OtchlanMapGenerator
{
    class ListOfSegments
    {
        public Segment previousSegment;
        public Segment playerSeg;
        public List<Segment> segments;
        public int baseBitmapSize = 50;
        const int padding = 10;
        const int ScrollSpeed = 5;
        public ListOfSegments()
        {
            segments = new List<Segment>();
            segments.Add(new Segment(0, new Point(padding, padding), Dir.north, Dir.south, Dir.east, Dir.west, "Start Location"));
            //segments.Add(new Segment(1, new Point(padding, padding+BitmapSize), Dir.north, Dir.south, Dir.not, Dir.not, "Location_2"));
            //segments.Add(new Segment(2, new Point(padding, padding + 2*BitmapSize), Dir.north, Dir.south, Dir.east, Dir.not, "Location_3"));
            //segments.Add(new Segment(3, new Point(padding+BitmapSize, padding + 2*BitmapSize), Dir.not, Dir.not, Dir.east, Dir.west, "Location_4"));

            //segments.Add(new Segment(4, new Point(padding, padding + 3 * BitmapSize), Dir.north, Dir.south, Dir.not, Dir.not, "Location_5"));
            //segments.Add(new Segment(5, new Point(padding, padding + 4 * BitmapSize), Dir.north, Dir.south, Dir.east, Dir.not, "Location_6"));
            //segments.Add(new Segment(6, new Point(padding, padding + 5 * BitmapSize), Dir.north, Dir.south, Dir.not, Dir.not, "Location_7"));
            //segments.Add(new Segment(7, new Point(padding, padding + 6 * BitmapSize), Dir.north, Dir.south, Dir.east, Dir.not, "Location_8"));
            //segments.Add(new Segment(8, new Point(padding, padding + 7 * BitmapSize), Dir.north, Dir.south, Dir.not, Dir.not, "Location_9"));
            //segments.Add(new Segment(9, new Point(padding, padding + 8 * BitmapSize), Dir.north, Dir.south, Dir.east, Dir.not, "Location_10"));
            //segments.Add(new Segment(10, new Point(padding, padding + 9 * BitmapSize), Dir.north, Dir.south, Dir.not, Dir.not, "Location_11"));
            //segments.Add(new Segment(11, new Point(padding, padding + 10 * BitmapSize), Dir.north, Dir.south, Dir.east, Dir.not, "Location_12"));
            //segments.Add(new Segment(12, new Point(padding, padding + 11 * BitmapSize), Dir.north, Dir.south, Dir.not, Dir.not, "Location_13"));
        }

        public void UpdateSegmentsLocationScroll(int dx, int dy, int old_dx, int old_dy)
        {
            if(old_dx!=dx) //&& old_dy==dy)  
            {
                CorrectLocationScroll((ScrollSpeed*(dx-old_dx)*-1), 0);
                old_dx = dx;
            }            
            if(old_dy!=dy)// && old_dx==dx)
            {
                CorrectLocationScroll(0, ScrollSpeed*(dy-old_dy)*-1);
                old_dy = dy;
            }
        }

        void CorrectLocationScroll(int dx, int dy)
        {
            foreach (Segment s in this.segments)
            {
                s.BMPlocation.X += dx;
                s.BMPlocation.Y += dy;
            }

        }
        //Returns "1" if segment added, "0" if segment not added, "-1" if keyBuffer was never equal 
        public int CheckKeyBuffer(int newID, String keyBuffer)
        {
            
            if (keyBuffer.Equals("n\r"))
            {
                if (playerSeg.BMPlocation.Y - baseBitmapSize < 0)
                {
                    CorrectSegmentsLocationOnAdd(0, baseBitmapSize);                  
                }
                return AddSegment(newID, 0, -1 * baseBitmapSize, new ExitPoints(Dir.not, Dir.south, Dir.not, Dir.not));
            }
            if (keyBuffer.Equals("s\r"))
            {
                return AddSegment(newID, 0, baseBitmapSize, new ExitPoints(Dir.north, Dir.not, Dir.not, Dir.not));            
            }
            if (keyBuffer.Equals("e\r"))
            {
               return AddSegment(newID, baseBitmapSize, 0, new ExitPoints(Dir.not, Dir.not, Dir.not, Dir.west));
               
            }
            if (keyBuffer.Equals("w\r"))
            {
                if (playerSeg.BMPlocation.X - baseBitmapSize < 0)
                {                 
                    CorrectSegmentsLocationOnAdd(baseBitmapSize, 0);
                }
                return AddSegment(newID, -1 * baseBitmapSize, 0, new ExitPoints(Dir.not, Dir.not, Dir.east, Dir.not));

            }
            return -1; 

        }
        ///Returns "1" if segment added, "0" if segment not added.
        private int AddSegment(int newID, int xShift, int yShift, ExitPoints e) //xShift yShift - shift of coordinates relative to the chosen segment
        {
            Segment s = new Segment(newID, new Point(playerSeg.BMPlocation.X + xShift, playerSeg.BMPlocation.Y + yShift), e.e1, e.e2, e.e3, e.e4, "Default name");
            this.previousSegment = this.playerSeg;

            Dir previousSegmentDirection = e.getExistingExit();
            Dir newSegmentDirection = e.getOppositeDir();
            //if(UpdatePrevSegmentExits(findSegmentByLocation(s.BMPlocation), e)==1) return 0; 
            //if(findSegmentByLocation(s.BMPlocation) != null) return 0;
            if (UpdateEnterdSegmentExits(findSegmentByLocation(s.BMPlocation), e) == 0) //handle adding new path when entering existing segment 
            {     
                segments.Add(s);
                this.playerSeg = s;
                //--------------------------------------------------------------------------------
                UpdatePrevSegmentExits(this.previousSegment, e);
                this.playerSeg.setNeighbourID(this.previousSegment.id, previousSegmentDirection);
                this.previousSegment.setNeighbourID(this.playerSeg.id, newSegmentDirection);
                //--------------------------------------------------------------------------------
                return 1;
            }
            this.playerSeg = findSegmentByLocation(s.BMPlocation);
            //----------------------------------------------------------------------------------------
            UpdatePrevSegmentExits(this.previousSegment,e); //when up 'if' returns 1 only update exit
            this.playerSeg.setNeighbourID(this.previousSegment.id, previousSegmentDirection);
            this.previousSegment.setNeighbourID(this.playerSeg.id, newSegmentDirection);
            //-----------------------------------------------------------------------------------------
            return 0;
        }

        public void CorrectSegmentsLocationOnAdd(int xShift, int yShift)
        {
            //UpdatePlayerSegLocation(playerSeg);
            
            
            foreach (Segment s in segments)
            {
                s.BMPlocation.X += xShift;
                s.BMPlocation.Y += yShift;
            }
        }

        //void UpdatePlayerSegLocation(Segment chosen)
        //{
        //    foreach (Segment s in segments) if (s.id == chosen.id) chosen.BMPlocation = s.BMPlocation;
        //}


        int UpdatePrevSegmentExits(Segment selected, ExitPoints e) //updates previous segment exits and neighbours IDs
        {
            if (selected == null) return 0;

            int[] oldNeighID = new int[] { selected.exits.neighbourID1, selected.exits.neighbourID2, selected.exits.neighbourID3, selected.exits.neighbourID4 };

            Dir OldN= selected.exits.e1;
            Dir OldS= selected.exits.e2;
            Dir OldE= selected.exits.e3;
            Dir OldW= selected.exits.e4;

            if (e.e1==Dir.north) selected.exits = new ExitPoints(OldN, Dir.south, OldE, OldW);
            if (e.e2 == Dir.south) selected.exits = new ExitPoints(Dir.north, OldS, OldE, OldW);
            if (e.e3 == Dir.east) selected.exits = new ExitPoints(OldN, OldS, OldE, Dir.west);
            if (e.e4 == Dir.west) selected.exits = new ExitPoints(OldN, OldS, Dir.east, OldW);
            selected.setBitmap();
            selected.exits.setNeighbours(oldNeighID);
            return 1;
        }
        int UpdateEnterdSegmentExits(Segment s, ExitPoints e) //updates entered segment exits and neighbours IDs
        {
            if (s == null) return 0;

            int[] oldNeighID = new int[] { s.exits.neighbourID1, s.exits.neighbourID2, s.exits.neighbourID3, s.exits.neighbourID4 };

            Dir OldN = s.exits.e1;
            Dir OldS = s.exits.e2;
            Dir OldE = s.exits.e3;
            Dir OldW = s.exits.e4;

            if (e.e1 == Dir.north) s.exits = new ExitPoints(Dir.north, OldS, OldE, OldW); 
            if (e.e2 == Dir.south) s.exits = new ExitPoints(OldN, Dir.south, OldE, OldW);
            if (e.e3 == Dir.east) s.exits = new ExitPoints(OldN, OldS, Dir.east, OldW);
            if (e.e4 == Dir.west) s.exits = new ExitPoints(OldN, OldS, OldE, Dir.west); 
            s.setBitmap();
            s.exits.setNeighbours(oldNeighID);
            return 1;
        }


        public Segment findSegmentByLocation(Point location)
        {
            foreach (Segment s in segments)
            {
                if (s.BMPlocation == location)
                {
                    return s;
                }
            }
            return null;
        }
        public Segment findSegment(Segment searched)
        {
            foreach (Segment s in segments) if (s.id == searched.id) return s;
            return null;
        }

       public void ChangeSegmentSizes(int delta) //params: int delta from -120 to 120 
        {
            float dt = (float)(delta / 100);

            if (dt > 0)
            {
                foreach (Segment s in this.segments)
                {
                    s.bitmap = new Bitmap(s.bitmap, new Size((int)(s.bitmap.Width * dt), (int)(s.bitmap.Height * dt)));
                    s.BMPlocation.X = (int)(dt*s.BMPlocation.X);
                    s.BMPlocation.Y = (int)(dt*s.BMPlocation.Y);
                    //remember about new BMPlocation!
                }
            }                                                           //Generally somwhere there int is overfloing :ccc !!!!!!!!!!
            if(dt<0)
            {
                dt *= -1; //make it positive :)
                foreach (Segment s in this.segments)
                {
                    s.bitmap = new Bitmap(s.bitmap, new Size((int)(s.bitmap.Width / dt), (int)(s.bitmap.Height / dt)));

                    s.BMPlocation.X = (int)(dt / s.BMPlocation.X);
                    s.BMPlocation.Y = (int)(dt / s.BMPlocation.Y);
                    //remember about new BMPlocation!
                }
            }

        }
            public void ResetBitmapSizes()
            {
                foreach (Segment s in this.segments)
                {
                    s.bitmap = new Bitmap(s.bitmap, new Size(this.baseBitmapSize,this.baseBitmapSize));
                }
            }



    }
}
