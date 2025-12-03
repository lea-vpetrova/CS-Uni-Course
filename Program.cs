using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace upr2_LP
{
    // ----------------------
    // Task 1 Structures
    // ----------------------
    public struct GeoCoordinate
    {
   
        [System.Text.Json.Serialization.JsonPropertyName("lat")]
        public float Latitude { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("lng")]
        public float Longitude { get; set; }
    }

    // ----------------------
    // Task 2 Structures
    // ----------------------
    public class PhoneEntry
    {
        public string FullName { get; set; }
        public string UniqueId { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class ContactDirectory
    {
        public List<PhoneEntry> Entries { get; set; } = new List<PhoneEntry>();

        public string FindNumber(string query)
        {
            var match = Entries.FirstOrDefault(e => e.FullName == query || e.UniqueId == query);
            return match != null ? match.PhoneNumber : "Not Found";
        }
    }

    // ----------------------
    // Task 3 Structures
    // ----------------------
    public class SceneObject
    {
        public string NodeName { get; set; }
        public string NodeId { get; set; }
        public string ReferenceTarget { get; set; } 
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("\n=== C# Exercise 2 Menu ===");
                Console.WriteLine("1. Process Geographic Data");
                Console.WriteLine("2. Process Phone Data");
                Console.WriteLine("3. Process 3D Scene Data");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ExecuteCoordinateTask();
                        break;
                    case "2":
                        ExecutePhoneTask();
                        break;
                    case "3":
                        ExecuteSceneTask();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        static void ExecuteCoordinateTask()
        {
            string sourceFile = "input-01.txt";
            if (!File.Exists(sourceFile))
            {
                Console.WriteLine($"File {sourceFile} not found.");
                return;
            }

            string fileData = File.ReadAllText(sourceFile);

            var coordinateSegments = fileData.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var pointList = new List<GeoCoordinate>();

            foreach (var segment in coordinateSegments)
            {
                var values = segment.Split(',');
                if (values.Length == 2)
                {
                    if (float.TryParse(values[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float parsedLat) &&
                        float.TryParse(values[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float parsedLng))
                    {
                        pointList.Add(new GeoCoordinate { Latitude = parsedLat, Longitude = parsedLng });
                    }
                }
            }

            string jsonOutput = JsonSerializer.Serialize(pointList, new JsonSerializerOptions { WriteIndented = true });
            string destinationFile = "locations.json";
            File.WriteAllText(destinationFile, "var locations = " + jsonOutput + ";");

            Console.WriteLine($"Processed {pointList.Count} coordinates.");
            Console.WriteLine($"Saved to {destinationFile}.");
        }

        static void ExecutePhoneTask()
        {
            string sourceFile = "input-02.txt";
            if (!File.Exists(sourceFile))
            {
                Console.WriteLine($"File {sourceFile} not found.");
                return;
            }

            string textData = File.ReadAllText(sourceFile);

            Regex rxNames = new Regex(@"\b[А-Я][а-я]+\b");
            Regex rxIds = new Regex(@"\b\d{6}\b");
            Regex rxPhones = new Regex(@"\+395\s+\d{3}\s+\d{2}\s+\d{2}");

            var nameMatches = rxNames.Matches(textData);
            var idMatches = rxIds.Matches(textData);
            var phoneMatches = rxPhones.Matches(textData);

            if (nameMatches.Count != idMatches.Count || idMatches.Count != phoneMatches.Count)
            {
                Console.WriteLine("Error: Data count mismatch.");
                return;
            }

            ContactDirectory directory = new ContactDirectory();

            for (int i = 0; i < nameMatches.Count; i++)
            {
                directory.Entries.Add(new PhoneEntry
                {
                    FullName = nameMatches[i].Value,
                    UniqueId = idMatches[i].Value,
                    PhoneNumber = Regex.Replace(phoneMatches[i].Value, @"\s+", " ")
                });
            }

            XmlSerializer xmlExporter = new XmlSerializer(typeof(List<PhoneEntry>));
            using (FileStream stream = new FileStream("contacts.xml", FileMode.Create))
            {
                xmlExporter.Serialize(stream, directory.Entries);
            }

            Console.WriteLine($"Processed {directory.Entries.Count} contacts.");
            Console.WriteLine("Saved to contacts.xml.");

            Console.WriteLine("Test Search -> Enter Name or ID:");
            string query = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(query))
            {
                Console.WriteLine($"Result: {directory.FindNumber(query)}");
            }
        }

        static void ExecuteSceneTask()
        {
            string sourceFile = "input-03.dae";
            if (!File.Exists(sourceFile))
            {
                Console.WriteLine($"File {sourceFile} not found.");
                return;
            }

            try
            {
                XDocument xmlDoc = XDocument.Load(sourceFile);
                List<SceneObject> objectLinks = new List<SceneObject>();

                foreach (var node in xmlDoc.Descendants())
                {
                    foreach (var attribute in node.Attributes())
                    {
                        if (attribute.Value.StartsWith("#"))
                        {
                            string refId = attribute.Value.Substring(1);

                            objectLinks.Add(new SceneObject
                            {
                                NodeName = node.Name.LocalName,
                                NodeId = node.Attribute("id")?.Value ?? "no-id",
                                ReferenceTarget = refId
                            });
                        }
                    }
                }

                string jsonOutput = JsonSerializer.Serialize(objectLinks, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("scene_links.json", jsonOutput);

                Console.WriteLine($"Found {objectLinks.Count} tags with connections.");
                Console.WriteLine("Saved to scene_links.json.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}