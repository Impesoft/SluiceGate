using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SluiceGate
{
    class FileIO
    {
        public static List<Ship> ReadShipsFromFile(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Ship>));

            StreamReader reader = new StreamReader(path);
            var list = (List<Ship>)serializer.Deserialize(reader);
            reader.Close();

            return list;
        }

        public static void WriteOrdersToFile(string path)
        {
            XmlSerializer serializer = new XmlSerializer(GlobalVar.ShipList.GetType());

            GlobalVar.ShipList = GlobalVar.ShipList.OrderBy(ship => ship.ArrivalTime).ToList(); 

            TextWriter writer = new StreamWriter(path);
            serializer.Serialize(writer, GlobalVar.ShipList);
            writer.Close();
        }

    }
}
