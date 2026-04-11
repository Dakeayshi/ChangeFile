# ChangeFile

A small **C#** console app: pick a folder, list files, then **rename** or **delete** the file you choose by number.

## Requirements

- **Windows** (console paths and tooling match a typical .NET Framework setup)
- **.NET Framework 4.7.2** developer pack (or a Visual Studio workload that includes it)
- **MSBuild** (via **Visual Studio** or **Build Tools for Visual Studio**)

This repo uses a **non–SDK-style** `.csproj`, so use **MSBuild** (or Visual Studio), not `dotnet run`, unless you convert the project to SDK style.

## How to build and run (from repo root)

**PowerShell** (after MSBuild is on your `PATH`, e.g. Developer PowerShell for Visual Studio):

```powershell
msbuild .\ChangeFile\ChangeFile.csproj /p:Configuration=Debug
.\ChangeFile\bin\Debug\ChangeFile.exe
```

**Visual Studio**: open `ChangeFile\ChangeFile.csproj`, build, run (F5 or Ctrl+F5).

## How it works

1. Prompts user for a **folder path** (re-prompts until the path is non-empty and the folder exists).
2. Asks whether to list files in that folder only or **include subfolders**.
3. Lists files with index numbers `1)`, `2)`, … and asks for a **valid file number** (re-prompts on bad input).
4. Shows a menu:
   - **Rename** — asks for a **new file name** (re-prompts until the name is valid and does not collide with an existing file).
   - **Delete** — asks for confirmation.
   - **Exit**

## Notes

- If a file with the new name already exists, the app **does not** overwrite it; you are asked for another name.
- Invalid characters in the new name are rejected; you are prompted again.
