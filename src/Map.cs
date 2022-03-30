using System;
using System.Collections.Generic;
using System.Drawing;


namespace OtchlanMapGenerator
{
    [Serializable]
    class Map
    {
        public String MapFileName = "";
        public String MapFilePath="";
        public Color MainMapColor;
        public Color PanelMapColor;
        public Language activeLanguage = Language.EN;

        //==============================
        public int newID=1;
        public Segment previousSegment;
        public Segment playerSeg;
        public List<Segment> segments;
        public int baseBitmapSize = 50;
        int ScrollSpeed = 15;
        public Map()
        {
            segments = new List<Segment>();
            segments.Add(new Segment(0, new Point(0, 0),0, Dir.not, Dir.not, Dir.not, Dir.not, Dir.not, Dir.not, Texts.text_DefaultName)); //add base segment
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
        public int CheckKeyBuffer(String keyBuffer, int currentFloor)
        {
            
            if (keyBuffer.Equals("n\r"))
            {
                //if (playerSeg.BMPlocation.Y - baseBitmapSize < 0) //not nessesary when camera follows player segment
                //{
                //    CorrectSegmentsLocationOnMap(0, baseBitmapSize);                  
                //}
                return AddSegment(0, -1 * baseBitmapSize, new ExitPoints(Dir.not, Dir.south, Dir.not, Dir.not, Dir.not, Dir.not), currentFloor);
            }
            if (keyBuffer.Equals("s\r"))
            {
                return AddSegment(0, baseBitmapSize, new ExitPoints(Dir.north, Dir.not, Dir.not, Dir.not, Dir.not, Dir.not),currentFloor);            
            }
            if (keyBuffer.Equals("e\r"))
            {
               return AddSegment(baseBitmapSize, 0, new ExitPoints(Dir.not, Dir.not, Dir.not, Dir.west, Dir.not, Dir.not),currentFloor);
               
            }
            if (keyBuffer.Equals("w\r"))
            {
                //if (playerSeg.BMPlocation.X - baseBitmapSize < 0) //not nessesary when camera follows player segment
                //{                 
                //    CorrectSegmentsLocationOnMap(baseBitmapSize, 0);
                //}
                return AddSegment(-1 * baseBitmapSize, 0, new ExitPoints(Dir.not, Dir.not, Dir.east, Dir.not, Dir.not, Dir.not),currentFloor);

            }
            if(keyBuffer.Equals("u\r"))
            {
                return AddSegment(0, 0, new ExitPoints(Dir.not, Dir.not, Dir.not, Dir.not, Dir.not, Dir.down), currentFloor+1);
            }
            if (keyBuffer.Equals("d\r"))
            {
                return AddSegment(0, 0, new ExitPoints(Dir.not, Dir.not, Dir.not, Dir.not, Dir.up, Dir.not), currentFloor-1);
            }
            return -1; 

        }
        ///Returns "1" if segment added, "0" if segment not added.
        private int AddSegment(int xShift, int yShift, ExitPoints e, int floor) //xShift yShift - shift of coordinates relative to the chosen segment
        {
            Segment s = new Segment(this.newID, new Point(playerSeg.BMPlocation.X + xShift, playerSeg.BMPlocation.Y + yShift),floor, e.eN, e.eS, e.eE, e.eW, e.eU,e.eD, Texts.text_DefaultName);
            this.previousSegment = this.playerSeg;

            Dir previousSegmentDirection = e.getExistingExit();
            Dir newSegmentDirection = e.getOppositeDir(); 
            //if(findSegmentByLocation(s.BMPlocation) != null) return 0;
            if (UpdateEnterdSegmentExits(findSegmentByLocation(s.BMPlocation, floor), e) == 0) //handle adding new path when entering existing segment 
            {     
                segments.Add(s); 
                this.playerSeg = s;
                //--------------------------------------------------------------------------------
                UpdatePrevSegmentExits(this.previousSegment, e);
                this.playerSeg.setNeighbourID(this.previousSegment.id, previousSegmentDirection);
                this.previousSegment.setNeighbourID(this.playerSeg.id, newSegmentDirection);
                //--------------------------------------------------------------------------------
                this.newID++;
                return 1;
            }
            this.playerSeg = findSegmentByLocation(s.BMPlocation, floor);
            //----------------------------------------------------------------------------------------
            UpdatePrevSegmentExits(this.previousSegment,e); //when top 'if' returns 1 only update exit
            this.playerSeg.setNeighbourID(this.previousSegment.id, previousSegmentDirection);
            this.previousSegment.setNeighbourID(this.playerSeg.id, newSegmentDirection);
            //-----------------------------------------------------------------------------------------
            return 0;
        }

        public void CorrectSegmentsLocationOnMap(int xShift, int yShift)
        {
            //UpdatePlayerSegLocation(playerSeg);     
            foreach (Segment s in segments)
            {
                s.BMPlocation.X += xShift;
                s.BMPlocation.Y += yShift;
            }
        }

        public void centerCameraOnPlayerSegment(Size formSize, Size sidePanelSize)
        {
            int xShift = ((formSize.Width / 2) - sidePanelSize.Width/2 - playerSeg.BMPlocation.X - this.baseBitmapSize/2);
            int yShift = ((formSize.Height / 2) - playerSeg.BMPlocation.Y - this.baseBitmapSize);

            CorrectSegmentsLocationOnMap(xShift, yShift);
        }

        int UpdatePrevSegmentExits(Segment selected, ExitPoints e) //updates previous segment exits and neighbours IDs
        {
            if (selected == null) return 0;

            int[] oldNeighID = new int[] { selected.exits.neighbourIDn, selected.exits.neighbourIDs,
                                           selected.exits.neighbourIDe, selected.exits.neighbourIDw, 
                                           selected.exits.neighbourIDu, selected.exits.neighbourIDd };

            Dir OldN= selected.exits.eN;
            Dir OldS= selected.exits.eS;
            Dir OldE= selected.exits.eE;
            Dir OldW= selected.exits.eW;
            Dir OldU = selected.exits.eU;
            Dir OldD = selected.exits.eD;

            if (e.eN==Dir.north) selected.exits = new ExitPoints(OldN, Dir.south, OldE, OldW, OldU,OldD);
            if (e.eS == Dir.south) selected.exits = new ExitPoints(Dir.north, OldS, OldE, OldW, OldU, OldD);
            if (e.eE == Dir.east) selected.exits = new ExitPoints(OldN, OldS, OldE, Dir.west, OldU, OldD);
            if (e.eW == Dir.west) selected.exits = new ExitPoints(OldN, OldS, Dir.east, OldW, OldU, OldD);
            if (e.eU == Dir.up) selected.exits = new ExitPoints(OldN, OldS, OldE, OldW, OldU, Dir.down);
            if (e.eD == Dir.down) selected.exits = new ExitPoints(OldN, OldS, OldE, OldW, Dir.up, OldD);
            selected.setBitmap('x','x');
            selected.exits.setNeighbours(oldNeighID);
            return 1;
        }
        int UpdateEnterdSegmentExits(Segment s, ExitPoints e) //updates entered segment exits and neighbours IDs
        {
            if (s == null) return 0;

            int[] oldNeighID = new int[] { s.exits.neighbourIDn, s.exits.neighbourIDs,
                                           s.exits.neighbourIDe, s.exits.neighbourIDw,
                                           s.exits.neighbourIDu, s.exits.neighbourIDd };
            Dir OldN = s.exits.eN;
            Dir OldS = s.exits.eS;
            Dir OldE = s.exits.eE;
            Dir OldW = s.exits.eW;
            Dir OldU = s.exits.eU;
            Dir OldD = s.exits.eD;

            if (e.eN == Dir.north) s.exits = new ExitPoints(Dir.north, OldS, OldE, OldW); 
            if (e.eS == Dir.south) s.exits = new ExitPoints(OldN, Dir.south, OldE, OldW);
            if (e.eE == Dir.east) s.exits = new ExitPoints(OldN, OldS, Dir.east, OldW);
            if (e.eW == Dir.west) s.exits = new ExitPoints(OldN, OldS, OldE, Dir.west);
            if (e.eU == Dir.up) s.exits = new ExitPoints(OldN, OldS, OldE, OldW, Dir.up, OldD);
            if (e.eD == Dir.down) s.exits = new ExitPoints(OldN, OldS, OldE, OldW, OldU, Dir.down);
            s.setBitmap('x','x');
            s.exits.setNeighbours(oldNeighID);
            return 1;
        }
        public void DeleteSegment(Segment toDelete)
        {
            int delSegID = toDelete.id;
            foreach (Segment s in segments)
            {
                if (s.exits.neighbourIDn == delSegID)
                {
                    s.exits.neighbourIDn = -1;
                    s.exits.eN = Dir.not;
                }
                if (s.exits.neighbourIDs == delSegID)
                {
                    s.exits.neighbourIDs = -1;
                    s.exits.eS = Dir.not;
                }
                if (s.exits.neighbourIDe == delSegID)
                {
                    s.exits.neighbourIDe = -1;
                    s.exits.eE = Dir.not;
                }
                if (s.exits.neighbourIDw == delSegID)
                {
                    s.exits.neighbourIDw = -1;
                    s.exits.eW = Dir.not;
                }
                if (s.exits.neighbourIDu == delSegID)
                {
                    s.exits.neighbourIDu = -1;
                    s.exits.eU = Dir.not;
                }
                if (s.exits.neighbourIDd == delSegID)
                {
                    s.exits.neighbourIDd = -1;
                    s.exits.eD = Dir.not;
                }
                s.setBitmap('x','x');
            }
            this.segments.Remove(this.findSegment(toDelete));
        }

        public Segment findSegmentByLocation(Point location, int floor)
        {
            foreach (Segment s in segments)
            {
                if (s.BMPlocation == location && s.floor==floor)
                {
                    return s;
                }
            }
            return null;
        }
        public Segment findSegmentByID(int id)
        {
            if (id < 0) return null;
            foreach (Segment s in segments) if (s.id == id) return s;

            return null;
        }
        public Segment findSegment(Segment searched)
        {
            foreach (Segment s in segments) if (s.id == searched.id) return s;
            return null;
        }

       public void ChangeSegmentSizes(double delta) //params: int delta from -120 to 120 
        {
            double dt = (delta / 100);

            if (dt > 0)
            {
                foreach (Segment s in this.segments)
                {
                    s.bitmap = new Bitmap(s.standardBitmap, new Size((int)(s.bitmap.Width * dt), (int)(s.bitmap.Height * dt)));
                    s.BMPlocation.X = (int)((dt*s.BMPlocation.X)); //
                    s.BMPlocation.Y = (int)((dt*s.BMPlocation.Y)); //use s.correctSegmentLocationOnMap ?                   
                }
            }                                                           
            if(dt<0)
            {
                dt *= -1; //make it positive :)
                foreach (Segment s in this.segments)
                {
                    s.bitmap = new Bitmap(s.standardBitmap, new Size((int)(s.bitmap.Width / dt), (int)(s.bitmap.Height / dt)));
                    s.BMPlocation.X = (int)((s.BMPlocation.X / dt));
                    s.BMPlocation.Y = (int)((s.BMPlocation.Y / dt));
                }
            }

        }
        public void ResetBitmapSizes()
        {
            foreach (Segment s in this.segments)
            {
                s.bitmap = new Bitmap(s.standardBitmap, new Size(this.baseBitmapSize,this.baseBitmapSize));
            }
        }

        void ClearDistances() //method sets distance to -1 in all segments before new distance map is created 
        {
            foreach (Segment s in this.segments) s.distance = -1;
        }

        //Method assigns each segment distance from player's segment (recursive)
        void UpdateDistanceMap(Segment thisSeg,int ID_neighbour, int prevSegID, int distance, int destSegID) 
        {
            if ( thisSeg==null  || ID_neighbour == -1 || ID_neighbour == prevSegID) return;
            if ( thisSeg.distance >= 0)
            {
                if (distance < thisSeg.distance)
                {
                    thisSeg.distance = distance;
                }
                else
                {
                    return;
                }
            }

            thisSeg.distance = distance;
            distance++;

            UpdateDistanceMap(findSegmentByID(thisSeg.exits.neighbourIDn), thisSeg.exits.neighbourIDn, thisSeg.id, distance, destSegID);
            UpdateDistanceMap(findSegmentByID(thisSeg.exits.neighbourIDs), thisSeg.exits.neighbourIDs, thisSeg.id, distance, destSegID);
            UpdateDistanceMap(findSegmentByID(thisSeg.exits.neighbourIDe), thisSeg.exits.neighbourIDe, thisSeg.id, distance, destSegID);
            UpdateDistanceMap(findSegmentByID(thisSeg.exits.neighbourIDw), thisSeg.exits.neighbourIDw, thisSeg.id, distance,destSegID);
            UpdateDistanceMap(findSegmentByID(thisSeg.exits.neighbourIDu), thisSeg.exits.neighbourIDu, thisSeg.id, distance, destSegID);
            UpdateDistanceMap(findSegmentByID(thisSeg.exits.neighbourIDd), thisSeg.exits.neighbourIDd, thisSeg.id, distance, destSegID);
        }


        public string FindWay(Segment segFrom, Segment segDest)
        {
            String route="";
            char previousDir='x'; // 'x' to establish this is start position (end position really but loop of findig route starts from end) 

            ClearDistances(); 
            UpdateDistanceMap(segFrom, 0, -1, 0, segDest.id);

            Segment temp = segDest;
            int distance= segDest.distance;

            while(distance>0)   //letters in String route are swapped to get proper sequence ( loop executes from the destination segment - not to it).
            {               
                temp = findSegmentByID(segDest.exits.neighbourIDn); //n
                if (temp!=null && temp.distance == (distance - 1))
                {                  
                    segDest.setBitmap(previousDir, 'n');
                    distance--;
                    route += "s,";
                    previousDir = 's';
                    segDest = temp;
                    continue;
                }
                temp = findSegmentByID(segDest.exits.neighbourIDs); //s
                if (temp != null && temp.distance == (distance - 1))
                {
                    segDest.setBitmap(previousDir, 's');
                    distance--;
                    route += "n,";
                    previousDir = 'n';
                    segDest = temp;
                    continue;
                }
                temp = findSegmentByID(segDest.exits.neighbourIDe); //e
                if (temp != null && temp.distance == (distance - 1))
                { 
                    segDest.setBitmap(previousDir, 'e');
                    distance--;
                    route += "w,";
                    previousDir = 'w';
                    segDest = temp;
                    continue;
                }
                temp = findSegmentByID(segDest.exits.neighbourIDw); //w
                if (temp != null && temp.distance == (distance - 1))
                {
                    segDest.setBitmap(previousDir, 'w');
                    distance--;
                    route += "e,";
                    previousDir = 'e';
                    segDest = temp;
                    continue;
                }
                temp = findSegmentByID(segDest.exits.neighbourIDu); //u
                if (temp != null && temp.distance == (distance - 1))
                {
                    segDest.setBitmap(previousDir, 'x');
                    distance--;
                    route += "d,";
                    previousDir = 'x';
                    segDest = temp;
                    continue;
                }
                temp = findSegmentByID(segDest.exits.neighbourIDd); //d
                if (temp != null && temp.distance == (distance - 1))
                {
                    segDest.setBitmap(previousDir, 'x');
                    distance--;
                    route += "u,";
                    previousDir = 'x';
                    segDest = temp;
                    continue;
                }

            }
            //reverse letters
            char[] charArray = route.ToCharArray();
            Array.Reverse(charArray);
            if(route.Length==0) return "";
            route = new string(charArray).Substring(1); //Substring to get rid of comma
            return route;
            
        }



    }
}
