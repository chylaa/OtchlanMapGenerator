using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtchlanMapGenerator
{
    class ExitPoints
    {
        public Exit e1,e2,e3,e4;
        public int neighbourID1, neighbourID2, neighbourID3, neighbourID4; // -1 if dont exist
        public ExitPoints(Exit a, Exit b, Exit c, Exit d)
        {
            this.e1 = a;
            this.e2 = b;
            this.e3 = c;
            this.e4 = d;
        }

        public virtual bool Equals(ExitPoints other)
        {
            if (this.e1 == other.e1 && this.e2 == other.e2 && this.e3 == other.e3 && this.e4 == other.e4) return true;
            return false;
        }
            
    }

    enum Exit {not,north,south,west,east};
    class Segment
    {
        public int id;
        //int neighbour_id //todo - show route
        public Bitmap bitmap;
        public Point BMPlocation;
        public String description;
        //public List<Exit> exits;
        public ExitPoints exits;

        public Segment()
        {
        }
        public Segment(int id, Point location, Exit exit1, Exit exit2, Exit exit3, Exit exit4, String name) //n_id 4 element array
        {
            this.id = id;
            this.exits = new ExitPoints(exit1, exit2, exit3, exit4);
            setBitmap();
            this.description = name;
            this.BMPlocation = location;
        }

        public void setBitmap()//use this in constructor, if i decide to List<Segment> - then List.add(new Segment(location, description,exits)) and inside setBitmap checks whith bitmap set 
        {
            
            //exits.Add(exit1);
            //exits.Add(exit2);
            //exits.Add(exit3);
            //exits.Add(exit4);

            if(this.exits.Equals(new ExitPoints(Exit.north, Exit.not, Exit.not, Exit.not))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukNorth.bmp");
            if (this.exits.Equals(new ExitPoints(Exit.not, Exit.south, Exit.not, Exit.not))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukSouth.bmp");
            if (this.exits.Equals(new ExitPoints(Exit.not, Exit.not, Exit.east, Exit.not))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukEast.bmp");
            if (this.exits.Equals(new ExitPoints(Exit.not, Exit.not, Exit.not, Exit.west))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukWest.bmp");
            if (this.exits.Equals(new ExitPoints(Exit.north, Exit.south, Exit.not, Exit.not))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukNorthSouth.bmp");
            if (this.exits.Equals(new ExitPoints(Exit.not,Exit.not, Exit.east, Exit.west))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukEastWest.bmp");
            if (this.exits.Equals(new ExitPoints(Exit.north, Exit.south, Exit.east, Exit.not))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukNorthSouthEast.bmp");
            if (this.exits.Equals(new ExitPoints(Exit.north, Exit.south, Exit.east, Exit.west))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukNorthSouthEastWest.bmp");

            
        }
    }
}
