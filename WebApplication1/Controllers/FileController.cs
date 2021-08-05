using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace WebApplication1
{
    public class FileController
    {
        static public void CreateTextFile(string path, string unsortedJson)
        {
            if(path != null && unsortedJson != null)
            {
                File.WriteAllLines(path, JsonToReadable(unsortedJson));
            }
        }

        static private string[] JsonToReadable(string unsortedJson)
        {
            string[] sortedJson = unsortedJson.Split(',');

            for(int i = 0; i < sortedJson.Length; i++)
            {
                if (sortedJson[i].StartsWith('{'))
                {
                    sortedJson[i] = sortedJson[i].Substring(1, sortedJson[i].Length-1);
                }
                else if (sortedJson[i].EndsWith('}'))
                {
                    sortedJson[i] = sortedJson[i].Substring(0, sortedJson[i].Length - 2);
                }
            }
            return sortedJson;
        }

        static public void CreateTextFile(string path, People[] unsortedJson)
        {
            if (path != null && unsortedJson != null)
            {
                File.WriteAllLines(path, JsonToReadable(unsortedJson));
            }
        }

        static private string[] JsonToReadable(People[] allPeople)
        {
            List<string> sortedJson = new List<string>();
            foreach (People person in allPeople)
            {
                sortedJson.Add(JsonConvert.SerializeObject(person));
            }

            for (int i = 0; i < sortedJson.Count; i++)
            {
                if (sortedJson[i].StartsWith('{'))
                {
                    sortedJson[i] = sortedJson[i].Substring(1, sortedJson[i].Length - 1);
                }
                else if (sortedJson[i].EndsWith('}'))
                {
                    sortedJson[i] = sortedJson[i].Substring(0, sortedJson[i].Length - 2);
                }
            }
            return sortedJson.ToArray();
        }

        static public void ReadAndRearrange(string path)
        {
            string input = File.ReadAllText(path);
            string[] newOutput = input.Split(',');
            File.WriteAllLines(path, newOutput);
        }
    }
}
