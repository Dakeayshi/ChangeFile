using System;
using System.IO;

namespace ChangeFile
{
    internal sealed class ChangeFileApplication
    {
        private readonly ValidatedConsolePrompts _prompts;

        public ChangeFileApplication(ValidatedConsolePrompts prompts)
        {
            _prompts = prompts ?? throw new ArgumentNullException(nameof(prompts));
        }

        public void Run()
        {
            string lastActionMessage = "Exited without changes.";

            string folderPath = _prompts.PromptExistingFolderPath();
            SearchOption searchOption = _prompts.PromptSearchOption();

            DirectoryInfo root = new DirectoryInfo(folderPath);
            FileInfo[] fileInfos = root.GetFiles("*", searchOption);

            if (fileInfos.Length == 0)
            {
                Console.WriteLine("No files found.");
                Console.WriteLine();
                Console.WriteLine(lastActionMessage + " Press any key to exit.");
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
            int selection = _prompts.PromptFileIndex(fileInfos.Length);
            string filePath = fileInfos[selection - 1].FullName;

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found in: " + filePath);
                Console.ReadKey();
                return;
            }

            lastActionMessage = RunActionMenu(filePath, lastActionMessage);

            Console.WriteLine();
            Console.WriteLine(lastActionMessage + " Press any key to exit.");
            Console.ReadKey();
        }

        private string RunActionMenu(string filePath, string lastActionMessage)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Choose an action:");
                Console.WriteLine("1) Rename (change file name)");
                Console.WriteLine("2) Delete file");
                Console.WriteLine("3) Exit");
                Console.Write("Please enter your choice: ");

                string choice = _prompts.ReadNormalizedLine();

                if (choice == "3")
                {
                    break;
                }

                if (choice == "2")
                {
                    if (TryDelete(ref filePath, ref lastActionMessage))
                    {
                        break;
                    }

                    continue;
                }

                if (choice == "1")
                {
                    if (TryRename(ref filePath, ref lastActionMessage))
                    {
                        break;
                    }

                    continue;
                }

                Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
            }

            return lastActionMessage;
        }

        /// <returns>True if the main action menu should exit (file missing, or delete succeeded).</returns>
        private bool TryDelete(ref string filePath, ref string lastActionMessage)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File no longer exists in the path: " + filePath);
                return true;
            }

            Console.Write("Are you sure you want to delete it? ('y' for 'yes' , 'n' for 'no'): ");
            string confirm = _prompts.ReadNormalizedLine();
            if (!confirm.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Delete cancelled.");
                return false;
            }

            try
            {
                File.Delete(filePath);
                Console.WriteLine("Deleted:");
                Console.WriteLine(filePath);
                lastActionMessage = "File deleted.";
                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine("Delete failed:");
                Console.WriteLine(error.Message);
                return false;
            }
        }

        /// <returns>True if the main action menu should exit (file missing before rename).</returns>
        private bool TryRename(ref string filePath, ref string lastActionMessage)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File no longer exists:");
                Console.WriteLine(filePath);
                return true;
            }

            string directory = Path.GetDirectoryName(filePath) ?? string.Empty;
            string newName = _prompts.PromptValidNewFileName(directory, filePath);
            string destPath = Path.Combine(directory, newName);

            try
            {
                File.Move(filePath, destPath);
                filePath = destPath;
                Console.WriteLine("Renamed to:");
                Console.WriteLine(filePath);
                lastActionMessage = "File changed (renamed).";
                return false;
            }
            catch (Exception error)
            {
                Console.WriteLine("Rename failed:");
                Console.WriteLine(error.Message);
                return false;
            }
        }
    }
}
