using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ResetTriviaDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Options: Rewrite dabase [rw], exit [e]...");

            string input = Console.ReadLine();

            while (input != "rw" && input != "e")
            {
                Console.WriteLine("Command not recognized.");
                input = Console.ReadLine();
            }

            if (input == "rw")
            {
                Console.WriteLine("The database will be constructed at the default path; see your local friendly admin to change this.");

                try
                {
                    DatabaseRewriter rw = new DatabaseRewriter("C:\\Program Files\\Trivia Scoring");
                    rw.RewriteDatabase();
                }
                catch (DirectoryNotFoundException dnf_e)
                {
                    Console.WriteLine("File not found... " + dnf_e.Message);
                    Console.ReadLine();
                }
                catch (IOException io_e)
                {
                    Console.WriteLine("IO problem... " + io_e.Message);
                    Console.ReadLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Whoops! some other problem? Try again!" + e.Message);
                    Console.ReadLine();
                }

                Console.WriteLine("\n\nRewrite process was complete!");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Bye!");
                Console.ReadLine();
            }
        }
    }
}
