using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Trivia.Scorers;

namespace Trivia.Sessions
{
    public static class GameSessionSerializer
    {
        public static GameSession GetGameSession(string fileName)
        {
            GameSession g = new GameSession(0, 0, 0, new List<ActiveScorer>());
            try
            {
                g = ReadObject(fileName);
            }
            catch (SerializationException serExc)
            {
                Console.WriteLine("Serialization failed");
                Console.WriteLine(serExc.Message);
            }
            return g;
        }

        public static void SaveGameSession(GameSession gs, string fileName)
        {
            try
            {
                FileStream writer = new FileStream(fileName, FileMode.Create);
                DataContractSerializer ser = new DataContractSerializer(typeof(GameSession));
                ser.WriteObject(writer, gs);
                writer.Close();
            }
            catch (SerializationException serExc)
            {
                Console.WriteLine("Serialization failed");
                Console.WriteLine(serExc.Message);
            }
            
        }

        public static GameSession ReadObject(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            DataContractSerializer ser = new DataContractSerializer(typeof(GameSession));

            GameSession gs = (GameSession)ser.ReadObject(reader, true);
            reader.Close();
            fs.Close();
            return gs;
        }
    }
}
