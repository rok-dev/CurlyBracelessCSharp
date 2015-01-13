using System.IO;
using System;

namespace CurlyBracelessCSharp
{
    public class Core
    {
        private readonly string TabString = "    ";

        private string inputText;
        private string outputFile;
        private StreamWriter writer;

        public Core(string inputFile, string outputFile)
        {
            StreamReader reader = new StreamReader(inputFile);
            inputText = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();

            this.outputFile = outputFile;
        }

        public void Run(string mode)
        {
            writer = new StreamWriter(outputFile);

            switch (mode)
            {
                case "MODE_ADD_BRACES":
                    {
                        AddBracesToFile();
                        break;
                    }
                case "MODE_REMOVE_BRACES":
                    {
                        RemoveBracesFromFile();
                        break;
                    }
            }

            writer.Close();
            writer.Dispose();
        }

        private void RemoveBracesFromFile()
        {
            string[] readLines = inputText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string readLine in readLines)
            {
                string trimmedLine = readLine.Trim();
                switch (trimmedLine)
                {
                    case "{":
                        continue;
                    case "}":
                        continue;
                }

                writer.WriteLine(readLine);
            }
        }

        private void AddBracesToFile()
        {
            int prevLineTabs = 0;

            string[] readLines = inputText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            int numOfEmptyLines = 0;
            foreach (string readLine in readLines)
            {
                if (readLine.Trim() == "")
                {
                    numOfEmptyLines++;
                    continue;
                }
                int lineTabs = CountTabsInLine(readLine);

                if (prevLineTabs == lineTabs - 1)
                {
                    writer.WriteLine(GenerateTabsString(prevLineTabs) + "{");
                }
                CheckClosingCurlyBraces(prevLineTabs, lineTabs, numOfEmptyLines);

                AddEmptyTabbedLines(prevLineTabs, numOfEmptyLines);
                numOfEmptyLines = 0;

                prevLineTabs = lineTabs;

                writer.WriteLine(readLine);
            }

            AddClosingCurlyBracesAtDocumentEnd(prevLineTabs);
        }

        private void AddClosingCurlyBracesAtDocumentEnd(int prevLineTabs)
        {
            CheckClosingCurlyBraces(prevLineTabs, 0, 0);
        }

        private void CheckClosingCurlyBraces(int prevLineTabs, int lineTabs, int numOfEmptyLines)
        {
            int diff = prevLineTabs - lineTabs;
            if (diff <= 0)
            {
                return;
            }

            int numOfCurlyBraceTabs = prevLineTabs - 1;
            for (int i = 0; i < diff; i++)
            {
                writer.WriteLine(GenerateTabsString(numOfCurlyBraceTabs) + "}");
                numOfCurlyBraceTabs--;
            }
        }

        private int AddEmptyTabbedLines(int numOfTabsInPrevLine, int numOfEmptyLines)
        {
            for (int i = 0; i < numOfEmptyLines; i++)
            {
                writer.WriteLine(GenerateTabsString(numOfTabsInPrevLine));
            }
            return numOfEmptyLines;
        }

        private int CountTabsInLine(string line)
        {
            return (line.Length - line.Replace(TabString, "").Length) / TabString.Length;
        }

        private string GenerateTabsString(int count)
        {
            string str = "";
            for (int i = 0; i < count; i++)
            {
                str += TabString;
            }
            return str;
        }
    }
}
