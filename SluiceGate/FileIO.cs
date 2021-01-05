using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SluiceGate
{
    internal class FileIO
    {
        public static List<Ship> ReadShipsFromFile(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Ship>));

            StreamReader reader = new StreamReader(path);
            var list = (List<Ship>)serializer.Deserialize(reader);
            reader.Close();

            return list;
        }

        public static void WriteShipsToFile(string path)
        {
            XmlSerializer serializer = new XmlSerializer(GlobalVar.ShipList.GetType());

            GlobalVar.ShipList = GlobalVar.ShipList.OrderBy(ship => ship.ArrivalTime).ToList();

            TextWriter writer = new StreamWriter(path);
            serializer.Serialize(writer, GlobalVar.ShipList);
            writer.Close();
        }

        public static void WriteToLog(string textToWriteToFile)
        {
            string path = GlobalVar.SluiceLogPath;
            using StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(textToWriteToFile);
        }

        public static void ClearShipsLogged()
        {
            if (File.Exists(GlobalVar.SluiceLogPath))
            {
                File.Delete(GlobalVar.SluiceLogPath);
            }
        }

        public static List<string> ReadShipLogFromFile()
        {
            if (File.Exists(GlobalVar.SluiceLogPath))
            {
                StreamReader reader = new StreamReader(GlobalVar.SluiceLogPath);
                string line = string.Empty;

                List<string> lines = new List<string>();

                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
                reader.Close();
                return lines;
            }
            else
            {
                List<string> list = new List<string>(new string[] { "empty" });
                return list;
            }
        }

        public static void CheckForIOFiles()
        {
            if (File.Exists(GlobalVar.PathShipList))
            {
                GlobalVar.ShipList = FileIO.ReadShipsFromFile(GlobalVar.PathShipList);
                //  todo set id to id of last ship in list
            }
        }
    }
}