using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Trivia.Scorers;
using Trivia.ScoringHelpers;

namespace Trivia.Sessions
{
    public static class GameStateSerializer
    {
        public static GameState GetGameSession(string fileName)
        {
            GameState g = new GameState();
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

        public static void SaveGameSession(GameState gs, string fileName)
        {
            try
            {
                FileStream writer = new FileStream(fileName, FileMode.Create);
                DataContractSerializer ser = new DataContractSerializer(typeof(GameState));
                ser.WriteObject(writer, gs);
                writer.Close();
            }
            catch (SerializationException serExc)
            {
                Console.WriteLine("Serialization failed");
                Console.WriteLine(serExc.Message);
            }
            
        }

        public static GameState ReadObject(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            DataContractSerializer ser = new DataContractSerializer(typeof(GameState));

            GameState gs = (GameState)ser.ReadObject(reader, true);
            reader.Close();
            fs.Close();
            return gs;
        }
    }
}
