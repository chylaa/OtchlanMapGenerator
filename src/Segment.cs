using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace OtchlanMapGenerator
{
    [Serializable]
    class ExitPoints
    {
        public Dir eN,eS,eE,eW,eU,eD; //north, south, east, west, up, down
        public int neighbourIDn, neighbourIDs, neighbourIDe, neighbourIDw, neighbourIDu, neighbourIDd; // -1 if dont exist
        public ExitPoints(Dir en, Dir es, Dir ee, Dir ew, [Optional]Dir eu, [Optional]Dir ed)
        {
            this.eN = en;
            this.eS = es;
            this.eE = ee;
            this.eW = ew;
            this.eD = ed;
            this.eU = eu;
            this.neighbourIDn = -1;
            this.neighbourIDs = -1;
            this.neighbourIDe = -1;
            this.neighbourIDw = -1;
            this.neighbourIDu = -1;
            this.neighbourIDd = -1;
        }

        public virtual bool Equals(ExitPoints other)
        {
            if (this.eN == other.eN && this.eS == other.eS && this.eE == other.eE && this.eW == other.eW ) return true; //wihout && other.eU == this.eU && other.eD == this.eD bc I dont have more bitmaps :) 
            return false;
        }
        public Dir getExistingExit() //returns first !not direction from exits (nessesary for settin neighbourID) 
        {
            if (this.eN == Dir.north) return this.eN;
            if (this.eS == Dir.south) return this.eS;
            if (this.eE == Dir.east) return this.eE;
            if (this.eW == Dir.west) return this.eW;
            if (this.eU == Dir.up) return this.eU;
            if (this.eD == Dir.down) return this.eD;
            return Dir.not;
        }
        public Dir getOppositeDir()
        {
            if (this.eN == Dir.north) return Dir.south;
            if (this.eS == Dir.south) return Dir.north;
            if (this.eE == Dir.east) return Dir.west;
            if (this.eW == Dir.west) return Dir.east;
            if (this.eU == Dir.up) return Dir.down;
            if (this.eD == Dir.down) return Dir.up;
            return Dir.not;
        }
        public void setNeighbours(int[] tabOf6)
        {
            //if (tabOf4.Length != 4) return;
            this.neighbourIDn = tabOf6[0];
            this.neighbourIDs = tabOf6[1];
            this.neighbourIDe = tabOf6[2];
            this.neighbourIDw = tabOf6[3];
            this.neighbourIDu = tabOf6[4];
            this.neighbourIDd = tabOf6[5];
        }

    }

    public enum Dir {not,north,south,west,east, up, down};
    [Serializable]
    class Segment
    {
        public int id;
        public int distance; //def distance from another, chosen segment.
        public Bitmap bitmap;
        public Bitmap standardBitmap; //bitmap of orginal Size (for ListOfSegments::changeSegmentSizes methond)
        //public Size BitmapSize= new Size(50,50);
        public Point BMPlocation;
        public int floor = 0; // 0 - base level, -1 basement, 1 - first floot  etc.
        public String name;
        public String decription;
        //public List<Dir> exits;
        public ExitPoints exits;
        public Boolean flag_infoProperAssigned;
        public Rectangle outlineRect;

        public Segment()
        {
        }
        public Segment(int id, Point location, int floor, Dir exit1, Dir exit2, Dir exit3, Dir exit4, [Optional] Dir exit5, [Optional] Dir exit6, String name) 
        {
            this.id = id;
            this.exits = new ExitPoints(exit1, exit2, exit3, exit4,exit5,exit6);
            setBitmap('x','x');
            this.name = name;
            this.BMPlocation = location;
            this.decription = "";
            this.floor = floor;
            this.distance = -1;
            this.flag_infoProperAssigned = true;
        }
        //params char previusDir/Direction - if == 'x' then sets normal bitmap, otherwise set adequate "route" bitmap. 
        public void setBitmap(char previousDir, char Direction)
        {
            if (previousDir == 'x' && Direction == 'x' )
            {
                if (this.exits.Equals(new ExitPoints(Dir.not, Dir.not, Dir.not, Dir.not))) this.bitmap = new Bitmap(@"Images\MiastoBruk.bmp");
                if (this.exits.Equals(new ExitPoints(Dir.not, Dir.not, Dir.east, Dir.not))) this.bitmap = new Bitmap(@"Images\MiastoBrukEast.bmp");
                if (this.exits.Equals(new ExitPoints(Dir.not, Dir.not, Dir.east, Dir.west))) this.bitmap = new Bitmap(@"Images\MiastoBrukEastWest.bmp");
                if (this.exits.Equals(new ExitPoints(Dir.north, Dir.not, Dir.not, Dir.not))) this.bitmap = new Bitmap(@"Images\MiastoBrukNorth.bmp");
                if (this.exits.Equals(new ExitPoints(Dir.north, Dir.not, Dir.east, Dir.not))) this.bitmap = new Bitmap(@"Images\MiastoBrukNorthEast.bmp");
                if (this.exits.Equals(new ExitPoints(Dir.north, Dir.not, Dir.east, Dir.west))) this.bitmap = new Bitmap(@"Images\MiastoBrukNorthEastWest.bmp");
                if (this.exits.Equals(new ExitPoints(Dir.north, Dir.south, Dir.not, Dir.not))) this.bitmap = new Bitmap(@"Images\MiastoBrukNorthSouth.bmp");
                if (this.exits.Equals(new ExitPoints(Dir.north, Dir.south, Dir.east, Dir.not))) this.bitmap = new Bitmap(@"Images\MiastoBrukNorthSouthEast.bmp");
                if (this.exits.Equals(new ExitPoints(Dir.north, Dir.south, Dir.east, Dir.west))) this.bitmap = new Bitmap(@"Images\MiastoBrukNorthSouthEastWest.bmp"); //Path.Combine(Environment.CurrentDirectory, @"Images\", "MiastoBrukNorthSouthEastWest.bmp"));
                if (this.exits.Equals(new ExitPoints(Dir.north, Dir.south, Dir.not, Dir.west))) this.bitmap = new Bitmap(@"Images\MiastoBrukNorthSouthWest.bmp");
                if (this.exits.Equals(new ExitPoints(Dir.north, Dir.not, Dir.not, Dir.west))) this.bitmap = new Bitmap(@"Images\MiastoBrukNorthWest.bmp");
                if (this.exits.Equals(new ExitPoints(Dir.not, Dir.south, Dir.not, Dir.not))) this.bitmap = new Bitmap(@"Images\MiastoBrukSouth.bmp");
                if (this.exits.Equals(new ExitPoints(Dir.not, Dir.south, Dir.east, Dir.not))) this.bitmap = new Bitmap(@"Images\MiastoBrukSouthEast.bmp");
                if (this.exits.Equals(new ExitPoints(Dir.not, Dir.south, Dir.east, Dir.west))) this.bitmap = new Bitmap(@"Images\MiastoBrukSouthEastWest.bmp");
                if (this.exits.Equals(new ExitPoints(Dir.not, Dir.south, Dir.not, Dir.west))) this.bitmap = new Bitmap(@"Images\MiastoBrukSouthWest.bmp");
                if (this.exits.Equals(new ExitPoints(Dir.not, Dir.not, Dir.not, Dir.west))) this.bitmap = new Bitmap(@"Images\MiastoBrukWest.bmp");
            }
            else //consider (instead of all if's) autocompletion of the file path by pasting variables previousDir and Direction e.g "..\Route"+previousDir+""+Direction
            {
                for (int i = 0; i < 2; i++)
                {
                    if (previousDir == 'x' && Direction == 'n') this.bitmap = new Bitmap(@"Images\RouteXNorth.bmp");
                    if (previousDir == 'x' && Direction == 's') this.bitmap = new Bitmap(@"Images\RouteXSouth.bmp");
                    if (previousDir == 'x' && Direction == 'e') this.bitmap = new Bitmap(@"Images\RouteXEast.bmp");
                    if (previousDir == 'x' && Direction == 'w') this.bitmap = new Bitmap(@"Images\RouteXWest.bmp");
                    //swap and check again
                    char buffer = previousDir;
                    previousDir = Direction;
                    Direction = buffer;
                }

               // if ((previousDir == 'n' && Direction == 's') || (previousDir == 'n' && Direction == 'n') || (previousDir == 's' && Direction == 's')) this.bitmap = new Bitmap(@"Images\RouteNorthSouth.bmp");
               // if ((previousDir == 'e' && Direction == 'w') || (previousDir == 'w' && Direction == 'w') || (previousDir == 'e' && Direction == 'e')) this.bitmap = new Bitmap(@"Images\RouteEastWest.bmp");

                for (int i = 0; i < 2; i++)
                {
                    if (previousDir == 'n' && Direction == 's') this.bitmap = new Bitmap(@"Images\RouteNorthSouth.bmp");
                    if (previousDir == 'e' && Direction == 'w') this.bitmap = new Bitmap(@"Images\RouteEastWest.bmp");
                    if (previousDir == 'n' && Direction == 'e') this.bitmap = new Bitmap(@"Images\RouteNorthEast.bmp");
                    if (previousDir == 'n' && Direction == 'w') this.bitmap = new Bitmap(@"Images\RouteNorthWest.bmp");
                    if (previousDir == 's' && Direction == 'e') this.bitmap = new Bitmap(@"Images\RouteSouthEast.bmp");
                    if (previousDir == 's' && Direction == 'w') this.bitmap = new Bitmap(@"Images\RouteSouthWest.bmp");
                    //swap and check again
                    char buffer = previousDir; 
                    previousDir = Direction;
                    Direction = buffer;
                }
            }
            setStandardBitmap();
        }

        private void setStandardBitmap()
        {
            this.standardBitmap = new Bitmap(this.bitmap);
        }
        public void setPlayerBitmap(char from)
        {
            if (from == 'x') return;
            if (from == 'n') this.bitmap = new Bitmap(@"Images\MiastoBrukPlayerNorth.bmp");
            if (from == 's') this.bitmap = new Bitmap(@"Images\MiastoBrukPlayerSouth.bmp");
            if (from == 'e') this.bitmap = new Bitmap(@"Images\MiastoBrukPlayerEast.bmp");
            if (from == 'w') this.bitmap = new Bitmap(@"Images\MiastoBrukPlayerWest.bmp");


            //this.bitmap = new Bitmap(this.bitmap, BitmapSize);
        }

        public void setExits(ExitPoints exitPoints)
        {
            this.exits = exitPoints;
        }

        public void setNeighbourID(int segmentID,Dir neighbourLocation) //int segmentID = ID of neighbour's of this segment | Dir neighbourLocation - witch way from this segment is his neighbour
        {
            //if (neighbourLocation == Dir.not) return; //Error situation
            if (neighbourLocation == Dir.north) this.exits.neighbourIDn = segmentID;
            if (neighbourLocation == Dir.south) this.exits.neighbourIDs = segmentID;
            if (neighbourLocation == Dir.east) this.exits.neighbourIDe = segmentID;
            if (neighbourLocation == Dir.west) this.exits.neighbourIDw = segmentID;
            if (neighbourLocation == Dir.up) this.exits.neighbourIDu = segmentID;
            if (neighbourLocation == Dir.down) this.exits.neighbourIDd = segmentID;

        }


        public void assignValues(Segment s)
        {
            this.name = s.name; 
            this.id = s.id;
            //this.BMPlocation = s.BMPlocation; //breaks map due to input old values, ignoring constant changes in BMPlocations
            this.distance = s.distance;
            this.decription = s.decription;

            if (this.exits!=null && s.exits!=null)
            {
                this.exits.eN = s.exits.eN;
                this.exits.eS = s.exits.eS;
                this.exits.eE = s.exits.eE;
                this.exits.eW = s.exits.eW;
                this.exits.eU = s.exits.eU;
                this.exits.eD = s.exits.eD;
            }
        }

        public void setSegmentInfo(ReadResult readResult)
        {
            if (readResult.invalid)
            {
                flag_infoProperAssigned = false;
                this.setRectangle();
                return;
            }
            if (!(this.name.Equals(Texts.text_DefaultName))) return;

            this.name = readResult.locationName;
            this.decription = readResult.locationDescription;
            this.exits.eN = readResult.exit_n;
            this.exits.eS = readResult.exit_s;
            this.exits.eE = readResult.exit_e;
            this.exits.eW = readResult.exit_w;
            this.exits.eU = readResult.exit_u;
            this.exits.eD = readResult.exit_d;

            this.setBitmap('x', 'x');

        }

        public void setRectangle() {if(!flag_infoProperAssigned) outlineRect = new Rectangle(this.BMPlocation, this.bitmap.Size); }
        public void resetRectangle() { outlineRect = new Rectangle(); }

    }
}
