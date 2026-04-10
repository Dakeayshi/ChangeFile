# ChangeFile
It is a simple **C#** console app that allows you view the existing file under a specific file path, change the file name, and delete the selected file

## Requirements
.NET SDK

## How to run
From the repo root:
```bash
dotnet run --project .\ChangeFile\ChangeFile.csproj
```

## How it works
1. Asks user the to enter a folder path
2. Asks if user want to view **all** the files under that path
    if so, prints all the files with index number: 1)... 2)...
3. Asks user to choose a file (choose by number)
4. Shows an action menu and carries out the action user chooses:
    1) Rename (change file name)
    2) Delete file
    3) Exit

## Notes
If the destination file name already exists, the app will not overwrite it.