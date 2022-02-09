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
        public Dir e1,e2,e3,e4; //north, south, east, west
        public int neighbourID1, neighbourID2, neighbourID3, neighbourID4; // -1 if dont exist
        public ExitPoints(Dir a, Dir b, Dir c, Dir d)
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

    enum Dir {not,north,south,west,east};
    class Segment
    {
        public int id;
        //int neighbour_id //todo - show route
        public Bitmap bitmap;
        public Point BMPlocation;
        public String description;
        //public List<Dir> exits;
        public ExitPoints exits;

        public Segment()
        {
        }
        public Segment(int id, Point location, Dir exit1, Dir exit2, Dir exit3, Dir exit4, String name) //n_id 4 element array
        {
            this.id = id;
            this.exits = new ExitPoints(exit1, exit2, exit3, exit4);
            setBitmap();
            this.description = name;
            this.BMPlocation = location;
        }

        public void setBitmap()//use this in constructor, if i decide to List<Segment> - then List.add(new Segment(location, description,exits)) and inside setBitmap checks whith bitmap set 
        {
            if (this.exits.Equals(new ExitPoints(Dir.not, Dir.not, Dir.not, Dir.not))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBruk.bmp");
            if (this.exits.Equals(new ExitPoints(Dir.not, Dir.not, Dir.east, Dir.not))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukEast.bmp");
            if (this.exits.Equals(new ExitPoints(Dir.not,Dir.not, Dir.east, Dir.west))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukEastWest.bmp");
            if (this.exits.Equals(new ExitPoints(Dir.north, Dir.not, Dir.not, Dir.not))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukNorth.bmp");
            if (this.exits.Equals(new ExitPoints(Dir.north, Dir.not, Dir.east, Dir.not))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukNorthEast.bmp");
            if (this.exits.Equals(new ExitPoints(Dir.north, Dir.not, Dir.east, Dir.west))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukNorthEastWest.bmp");
            if (this.exits.Equals(new ExitPoints(Dir.north, Dir.south, Dir.not, Dir.not))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukNorthSouth.bmp");
            if (this.exits.Equals(new ExitPoints(Dir.north, Dir.south, Dir.east, Dir.not))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukNorthSouthEast.bmp");
            if (this.exits.Equals(new ExitPoints(Dir.north, Dir.south, Dir.east, Dir.west))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukNorthSouthEastWest.bmp");
            if (this.exits.Equals(new ExitPoints(Dir.north, Dir.south, Dir.not, Dir.west))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukNorthSouthWest.bmp");
            if (this.exits.Equals(new ExitPoints(Dir.north, Dir.not, Dir.not, Dir.west))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukNorthWest.bmp");
            if (this.exits.Equals(new ExitPoints(Dir.not, Dir.south, Dir.not, Dir.not))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukSouth.bmp");
            if (this.exits.Equals(new ExitPoints(Dir.not, Dir.south, Dir.east, Dir.not))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukSouthEast.bmp");
            if (this.exits.Equals(new ExitPoints(Dir.not, Dir.south, Dir.east, Dir.west))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukSouthEastWest.bmp");
            if (this.exits.Equals(new ExitPoints(Dir.not, Dir.south, Dir.not, Dir.west))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukSouthWest.bmp");
            if (this.exits.Equals(new ExitPoints(Dir.not, Dir.not, Dir.not, Dir.west))) this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukWest.bmp");
            
        }

        public void setExits(ExitPoints exitPoints)
        {
            this.exits = exitPoints;
        }
        public void assignValues(Segment s)
        {
            this.description = s.description; 
            this.id = s.id;
            this.BMPlocation = s.BMPlocation;
        }
    }
}
