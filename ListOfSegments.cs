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
        public List<Segment> segments;
        const int BitmapSize = 50;
        const int ScrollSpeed = 5;
        public ListOfSegments()
        {
            segments = new List<Segment>();
            segments.Add(new Segment(0, new Point(10, 10), Exit.not, Exit.south, Exit.not, Exit.not, "Location"));
            segments.Add(new Segment(1, new Point(10, 10+BitmapSize), Exit.north, Exit.south, Exit.not, Exit.not, "Location_2"));
            segments.Add(new Segment(2, new Point(10, 10 + 2*BitmapSize), Exit.north, Exit.south, Exit.east, Exit.not, "Location_3"));
            segments.Add(new Segment(3, new Point(10+BitmapSize, 10 + 2*BitmapSize), Exit.not, Exit.not, Exit.east, Exit.west, "Location_4"));

            segments.Add(new Segment(4, new Point(10, 10 + 3 * BitmapSize), Exit.north, Exit.south, Exit.not, Exit.not, "Location_5"));
            segments.Add(new Segment(5, new Point(10, 10 + 4 * BitmapSize), Exit.north, Exit.south, Exit.east, Exit.not, "Location_6"));
            segments.Add(new Segment(6, new Point(10, 10 + 5 * BitmapSize), Exit.north, Exit.south, Exit.not, Exit.not, "Location_7"));
            segments.Add(new Segment(7, new Point(10, 10 + 6 * BitmapSize), Exit.north, Exit.south, Exit.east, Exit.not, "Location_8"));
            segments.Add(new Segment(8, new Point(10, 10 + 7 * BitmapSize), Exit.north, Exit.south, Exit.not, Exit.not, "Location_9"));
            segments.Add(new Segment(9, new Point(10, 10 + 8 * BitmapSize), Exit.north, Exit.south, Exit.east, Exit.not, "Location_10"));
            segments.Add(new Segment(10, new Point(10, 10 + 9 * BitmapSize), Exit.north, Exit.south, Exit.not, Exit.not, "Location_11"));
            segments.Add(new Segment(11, new Point(10, 10 + 10 * BitmapSize), Exit.north, Exit.south, Exit.east, Exit.not, "Location_12"));
            segments.Add(new Segment(12, new Point(10, 10 + 11 * BitmapSize), Exit.north, Exit.south, Exit.not, Exit.not, "Location_13"));
        }

        public void UpdateSegmentsLocation(int dx, int dy, int old_dx, int old_dy)
        {
            if(old_dx!=dx && old_dy==dy)  //Zle sa te ify w chuj
            {
                CorrectLocation((ScrollSpeed*(dx-old_dx)*-1), 0);
                old_dx = dx;
            }            
            if(old_dy!=dy && old_dx==dx)
            {
                CorrectLocation(0, ScrollSpeed*(dy-old_dy)*-1);
                old_dy = dy;
            }
        }

        void CorrectLocation(int dx, int dy)
        {
            foreach (Segment s in this.segments)
            {
                s.BMPlocation.X += dx;  //tu nwm wsm chyba bez ( -old_ ) bylo "lepiej"
                s.BMPlocation.Y += dy;
            }

        }


    }
}
