using System.IO;
namespace CurlyBracelessCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            string mode = args[0];
            string directory = args[1];

            string[] filePaths = Directory.GetFiles(directory, "*.cs", SearchOption.TopDirectoryOnly);
            foreach (string filePath in filePaths)
            {
                Core core = new Core(filePath, filePath);
                core.Run(mode);
            }
        }
    }
}
