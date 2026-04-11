using System;
using System.IO;

namespace ChangeFile
{
    /// <summary>
    /// Prompts the user until input passes validation. Invalid attempts re-prompt via recursive calls.
    /// </summary>
    internal sealed class ValidatedConsolePrompts
    {
        public string ReadNormalizedLine()
        {
            return (Console.ReadLine() ?? string.Empty).Trim().Trim('"');
        }

        public string PromptExistingFolderPath()
        {
            Console.WriteLine("Enter the folder path (example: C:\\Users\\Username\\Desktop)");
            Console.Write("Folder path: ");
            string folderPath = ReadNormalizedLine();

            if (string.IsNullOrWhiteSpace(folderPath))
            {
                Console.WriteLine("Please enter a non-empty folder path.");
                return PromptExistingFolderPath();
            }

            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("Folder not found:");
                Console.WriteLine(folderPath);
                return PromptExistingFolderPath();
            }

            return folderPath;
        }

        public SearchOption PromptSearchOption()
        {
            Console.Write("List all the files in the folder? ('y' for 'yes' , 'n' for 'no'): ");
            string recursiveInput = ReadNormalizedLine();
            return recursiveInput.Equals("y", StringComparison.OrdinalIgnoreCase)
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;
        }

        public int PromptFileIndex(int fileCount)
        {
            Console.Write("Please choose a file number: ");
            string selectionRaw = ReadNormalizedLine();

            if (!int.TryParse(selectionRaw, out int selection) || selection < 1 || selection > fileCount)
            {
                Console.WriteLine($"Invalid selection. Enter a number from 1 to {fileCount}.");
                return PromptFileIndex(fileCount);
            }

            return selection;
        }

        public string PromptValidNewFileName(string directory, string currentFullPath)
        {
            Console.WriteLine("Enter the new file name (example: newname.txt).");
            Console.Write("New name: ");
            string newName = ReadNormalizedLine();

            if (string.IsNullOrWhiteSpace(newName))
            {
                Console.WriteLine("Invalid new name. It cannot be empty.");
                return PromptValidNewFileName(directory, currentFullPath);
            }

            if (newName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                Console.WriteLine("New name contains invalid characters.");
                return PromptValidNewFileName(directory, currentFullPath);
            }

            string destPath = Path.Combine(directory, newName);

            if (string.Equals(currentFullPath, destPath, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("That is already the file name. Choose a different name.");
                return PromptValidNewFileName(directory, currentFullPath);
            }

            if (File.Exists(destPath))
            {
                Console.WriteLine("A file with that name already exists:");
                Console.WriteLine(destPath);
                return PromptValidNewFileName(directory, currentFullPath);
            }

            return newName;
        }
    }
}
