using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

//==========ListOfSegments==========

//public Segment previousSegment;
//public Segment playerSeg;
//public List<Segment> segments;
//public int baseBitmapSize = 50;
//int padding;
//int ScrollSpeed = 15;

//===========Segment=================
//public int id;
//public int distance; //def distance from another, chosen segment.
//public Bitmap bitmap;
//public Bitmap standardBitmap; //bitmap of orginal Size (for ListOfSegments::changeSegmentSizes methond)
//public Point BMPlocation;
//public String name;
//public String decription;
//public ExitPoints exits;

namespace OtchlanMapGenerator
{
    static class MapSave
    {
        public static Boolean SaveMapToFile(string filename, ListOfSegments SegList)
        {
            byte[] SegmentListData = ObjectToByteArray(SegList);
            if (SegmentListData == null) return false;

            try
            {
                File.WriteAllBytes(filename, SegmentListData);

            }catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error saving the data.");
                return false;
            }

            return true;
        }

        public static ListOfSegments ReadMapFromFile(string filename)
        {
            ListOfSegments ReadedSegList = new ListOfSegments();
            try
            {
                byte[] ListOfSegmentsdataArray = File.ReadAllBytes(filename);
                ReadedSegList = (ListOfSegments)ByteArrayToObject(ListOfSegmentsdataArray);
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error reading the data.");
                return null;
            }
            return ReadedSegList;
        }


        static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        static Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }

    }
}
