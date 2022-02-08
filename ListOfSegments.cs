﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OtchlanMapGenerator
{
    class ListOfSegments
    {
        public List<Segment> segments;
        public int BitmapSize = 50;
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

        public int CheckKeyBuffer(int newID, String keyBuffer, Segment chosen)
        {
            
            if (keyBuffer.Equals("n\r"))
            {
                if (chosen.BMPlocation.Y - BitmapSize < 0)
                {
                    CorrectSegmentsLocationAdd(0, BitmapSize, chosen);
                    return AddSegment(newID, 0, 0,chosen,new ExitPoints(Dir.not,Dir.south,Dir.not,Dir.not));
                }
                return AddSegment(newID, 0, -1 * BitmapSize,chosen, new ExitPoints(Dir.not, Dir.south, Dir.not, Dir.not));
            }
            if (keyBuffer.Equals("s\r"))
            {
                return AddSegment(newID, 0, BitmapSize, chosen, new ExitPoints(Dir.north, Dir.not, Dir.not, Dir.not));            
            }
            if (keyBuffer.Equals("e\r"))
            {
               return AddSegment(newID, BitmapSize, 0, chosen, new ExitPoints(Dir.not, Dir.not, Dir.not, Dir.west));
               
            }
            if (keyBuffer.Equals("w\r"))
            {
                if (chosen.BMPlocation.X - BitmapSize < 0)
                {                 
                    CorrectSegmentsLocationAdd(BitmapSize, 0, chosen);
                    return AddSegment(newID, 0, 0,chosen, new ExitPoints(Dir.not, Dir.not, Dir.east, Dir.not));
                }
                return AddSegment(newID, -1 * BitmapSize, 0,chosen, new ExitPoints(Dir.not, Dir.not, Dir.east, Dir.not));

            }
            return 0;

        } //TODO -change of neighbour's ID on segment edit / (or segment added if I can take info from otchlan's process)
        private int AddSegment(int newID, int xShift, int yShift, Segment chosen, ExitPoints e) //xShift yShift - shift of coordinates relative to the chosen segment
        {
            Segment s = new Segment(newID, new Point(chosen.BMPlocation.X + xShift, chosen.BMPlocation.Y + yShift), e.e1, e.e2, e.e3, e.e4, "Default name");
            if(findSegmentByLocation(s.BMPlocation) == true) return 0;
            segments.Add(s);
            UpdateNeighbourExits(chosen, e);
            return 1;
        }

        public void CorrectSegmentsLocationAdd(int xShift, int yShift, Segment chosen)
        {
            UpdateChosenLocation(chosen);
            foreach (Segment s in segments)
            {
                s.BMPlocation.X += xShift;
                s.BMPlocation.Y += yShift;
            }
        }

        void UpdateChosenLocation(Segment chosen)
        {
            foreach (Segment s in segments) if (s.id == chosen.id) chosen.BMPlocation = s.BMPlocation;
        }
        void UpdateNeighbourExits(Segment chosen, ExitPoints e)
        {
            foreach (Segment s in segments)
            {
                if (s.id == chosen.id)  //must remember previous existing exits!!! Now only one is updated!
                {                
                    Dir OldN= s.exits.e1;
                    Dir OldS= s.exits.e2;
                    Dir OldE= s.exits.e3;
                    Dir OldW= s.exits.e4;

                    if (e.e1==Dir.north) s.exits = new ExitPoints(OldN, Dir.south, OldE, OldW);
                    if (e.e2 == Dir.south) s.exits = new ExitPoints(Dir.north, OldS, OldE, OldW);
                    if (e.e3 == Dir.east) s.exits = new ExitPoints(OldN, OldS, OldE, Dir.west);
                    if (e.e4 == Dir.west) s.exits = new ExitPoints(OldN, OldS, Dir.east, OldW);
                    s.setBitmap();
                }
            }
        }

        public bool findSegmentByLocation(Point location)
        {
            foreach (Segment s in segments) if (s.BMPlocation == location) return true;
            return false;
        }
        public Segment findSegment(Segment searched)
        {
            foreach (Segment s in segments) if (s.id == searched.id) return s;
            return null;
        }



    }
}