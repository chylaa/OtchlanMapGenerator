using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



namespace OtchlanMapGenerator
{
    static class MapSave
    {
        public static Boolean SaveMapToFile(string filename, Map SegMap)
        {
            byte[] MapData = ObjectToByteArray(SegMap);
            if (MapData == null) return false;

            try
            {
                File.WriteAllBytes(filename, MapData);

            }catch(Exception)
            {
                return false;
            }

            return true;
        }

        public static Map LoadMapFromFile(string filename)
        {
            Map ReadedSegList = new Map();
            try
            {
                byte[] MapDataArray = File.ReadAllBytes(filename);
                ReadedSegList = (Map)ByteArrayToObject(MapDataArray);
            }
            catch(Exception)
            {
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
