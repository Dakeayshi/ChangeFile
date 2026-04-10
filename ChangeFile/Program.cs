using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeFile
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string lastActionMessage = "Exited without changes.";

            Console.WriteLine("Enter the folder path (example: C:\\Users\\Username\\Desktop)");
            Console.Write("Folder path: ");
            string folderPath = (Console.ReadLine() ?? string.Empty).Trim().Trim('"'); // Trim quotes and whitespace for realistic scenario

            if (string.IsNullOrWhiteSpace(folderPath))
            {
                Console.WriteLine("Please enter a valid folder path.");
                Console.ReadKey();
                return;
            }

            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("Folder not found:");
                Console.WriteLine(folderPath);
                Console.ReadKey();
                return;
            }

            Console.Write("List all the files in the folder? (y/n): ");
            string recursiveInput = (Console.ReadLine() ?? string.Empty).Trim();
            SearchOption searchOption = recursiveInput.Equals("y", StringComparison.OrdinalIgnoreCase)
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;

            DirectoryInfo root = new DirectoryInfo(folderPath);
            FileInfo[] fileInfos = root.GetFiles("*", searchOption);

            if (fileInfos.Length == 0)
            {
                Console.WriteLine("No files found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Files:");
            for (int i = 0; i < fileInfos.Length; i++)
            {
                Console.WriteLine($"{i + 1}) {fileInfos[i].FullName}");
            }

            Console.WriteLine();
            Console.Write("Please choose a file number: ");
            string selectionRaw = (Console.ReadLine() ?? string.Empty).Trim();

            if (!int.TryParse(selectionRaw, out int selection) || selection < 1 || selection > fileInfos.Length)
            {
                Console.WriteLine("It is an invalid selection.");
                Console.ReadKey();
                return;
            }

            string filePath = fileInfos[selection - 1].FullName;

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found:");
                Console.WriteLine(filePath);
                Console.ReadKey();
                return;
            }

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Choose an action:");
                Console.WriteLine("1) Rename (change file name)");
                Console.WriteLine("2) Delete file");
                Console.WriteLine("3) Exit");
                Console.Write("Enter choice: ");

                string choice = (Console.ReadLine() ?? string.Empty).Trim();

                if (choice == "3")
                {
                    break;
                }

                if (choice == "2")
                {
                    if (!File.Exists(filePath)) // Prevent recursive deletion
                    {
                        Console.WriteLine("File no longer exists:");
                        Console.WriteLine(filePath);
                        break;
                    }

                    Console.Write("Are you sure you want to delete it? (y/n): ");
                    string confirm = (Console.ReadLine() ?? string.Empty).Trim();
                    if (!confirm.Equals("y", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Cancelled.");
                        continue;
                    }

                    try
                    {
                        File.Delete(filePath);
                        Console.WriteLine("Deleted:");
                        Console.WriteLine(filePath);
                        lastActionMessage = "File deleted.";
                        break;
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine("Delete failed:");
                        Console.WriteLine(error.Message);
                        continue;
                    }
                }

                if (choice == "1")
                {
                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine("File no longer exists:");
                        Console.WriteLine(filePath);
                        break;
                    }

                    Console.WriteLine("Enter the new file name (example: newname.txt).");
                    Console.Write("New name: ");
                    string newName = (Console.ReadLine() ?? string.Empty).Trim().Trim('"');

                    if (string.IsNullOrWhiteSpace(newName))
                    {
                        Console.WriteLine("Invalid new name.");
                        continue;
                    }

                    if (newName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                    {
                        Console.WriteLine("New name contains invalid characters."); // Prevent invalid characters in the new name
                        continue;
                    }

                    string directory = Path.GetDirectoryName(filePath) ?? string.Empty;
                    string destPath = Path.Combine(directory, newName);

                    if (string.Equals(filePath, destPath, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Source and destination are the same.");
                        continue;
                    }

                    if (File.Exists(destPath))
                    {
                        Console.WriteLine("A file with that name already exists:");
                        Console.WriteLine(destPath);
                        continue;
                    }

                    try
                    {
                        File.Move(filePath, destPath);
                        filePath = destPath; // Update the file path
                        Console.WriteLine("Renamed to:");
                        Console.WriteLine(filePath);
                        lastActionMessage = "File changed (renamed).";
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine("Rename failed:");
                        Console.WriteLine(error.Message);
                    }

                    continue;
                }

                Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
            }

            Console.WriteLine();
            Console.WriteLine(lastActionMessage + " Press any key to exit.");
            Console.ReadKey();
        }
    }
}
