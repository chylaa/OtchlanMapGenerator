﻿using System;
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
            this.neighbourID1 = -1;
            this.neighbourID2 = -1;
            this.neighbourID3 = -1;
            this.neighbourID4 = -1;
        }

        public virtual bool Equals(ExitPoints other)
        {
            if (this.e1 == other.e1 && this.e2 == other.e2 && this.e3 == other.e3 && this.e4 == other.e4) return true;
            return false;
        }
        public Dir getExistingExit() //returns first !not direction from exits (nessesary for settin neighbourID) 
        {
            if (this.e1 == Dir.north) return this.e1;
            if (this.e2 == Dir.south) return this.e2;
            if (this.e3 == Dir.east) return this.e3;
            if (this.e4 == Dir.west) return this.e4;
            return Dir.not;
        }
        public Dir getOppositeDir()
        {
            if (this.e1 == Dir.north) return Dir.south;
            if (this.e2 == Dir.south) return Dir.north;
            if (this.e3 == Dir.east) return Dir.west;
            if (this.e4 == Dir.west) return Dir.east;
            return Dir.not;
        }
        public void setNeighbours(int[] tabOf4)
        {
            //if (tabOf4.Length != 4) return;
            this.neighbourID1 = tabOf4[0];
            this.neighbourID2 = tabOf4[1];
            this.neighbourID3 = tabOf4[2];
            this.neighbourID4 = tabOf4[3];
        }


    }

    enum Dir {not,north,south,west,east};
    class Segment
    {
        public int id;
        public int distance; //def distance from another, chosen segment.
        //int neighbour_id //todo - show route
        public Bitmap bitmap;
        //public Size BitmapSize= new Size(50,50);
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

            this.distance = -1;
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
        public void setPlayerBitmap(char from)
        {
            if (from == 'x') return;
            if (from == 'n') this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukPlayerNorth.bmp");
            if (from == 's') this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukPlayerSouth.bmp");
            if (from == 'e') this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukPlayerEast.bmp");
            if (from == 'w') this.bitmap = new Bitmap(@"C:\Users\Dell\Desktop\CDI\OtchlanMapGenerator\MiastoBrukPlayerWest.bmp");


            //this.bitmap = new Bitmap(this.bitmap, BitmapSize);
        }

        public void setExits(ExitPoints exitPoints)
        {
            this.exits = exitPoints;
        }

        public void setNeighbourID(int segmentID,Dir neighbourLocation) //int segmentID = ID of neighbour's of this segment | Dir neighbourLocation - witch way from this segment is his neighbour
        {
            //if (neighbourLocation == Dir.not) return; //Error situation
            if (neighbourLocation == Dir.north) this.exits.neighbourID1 = segmentID;
            if (neighbourLocation == Dir.south) this.exits.neighbourID2 = segmentID;
            if (neighbourLocation == Dir.east) this.exits.neighbourID3 = segmentID;
            if (neighbourLocation == Dir.west) this.exits.neighbourID4 = segmentID;

        }


        public void assignValues(Segment s)
        {
            this.description = s.description; 
            this.id = s.id;
            this.BMPlocation = s.BMPlocation;
            this.distance = s.distance;

            if (this.exits!=null)
            {
                this.exits.e1 = s.exits.e1;
                this.exits.e2 = s.exits.e2;
                this.exits.e3 = s.exits.e3;
                this.exits.e4 = s.exits.e4;
            }
        }
    }
}
