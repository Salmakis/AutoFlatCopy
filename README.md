# AutoFlatCopy

AutoFlatCopy is a small Windows too wich resides in the Taskbar.

It will permamently synchronize files from a Source Folder to a Target Folder,
but insteald of just Copying the files it will flatten the File names and put them all into the root directiory of the Destination Folder.

The directories will be replaced with name prefixes separated by a dot.

I Created this Tool to edit my Code for the game Screeps.com, but this game not allows the source code to contain any subfolders,
but specially in bigger code projects i cant live without keep the stuff organized in folders.

original file path | new file path 
--- | --- 
sourceDir/notherfile.txt | targetDir/notherfile.txt 
sourceDir/folder/file1.txt | targetDir/folder.file1.txt 
sourceDir/folder/file2.ping | targetDir/folder.file2.png 
sourceDir/folder/subfolderA/someFile.png | targetDir/folder.subfolderA.someFile.png 
sourceDir/folder/subfolderB/someExtraFile.png | targetDir/folder.subfolderB.someExtraFile.png 

## Example video 
![sync_opt.gif](https://s13.postimg.org/oaeu7w97r/sync_opt.gif)](https://postimg.org/image/55bky4ujn/)
 
## Usage
Just start AutoFlatCopy.exe it will create a new Icon in your Toolbar.
Rightclick it, chose Configuration and make your setup (Source folder, destination folder, file filter etc)
 
If you want you can create a clean "Initial status" with the button "FlatCopy All Now", but care, this will delete all file from the    target directory, and then copy all files from the source dir and its subdirectiories over with the given rules.

Rightclick the icon again and chose Start Sync to start the sync
The icon turns somewhat green to indicate that its active.

Now can you edit your files in the source directory and they will be (flat)copied into the destination directory.

Rightclick the icon once again and chose Stop Sync to stop this behavior.
The icon turns somewhat red to indicate that its inactive.

 ## A known problem
 
 Deleteing a folder with contents can actually confuse the tool, so if you want to delete a folder, try to delete all files first and then the folder.
 
 ## Using with IDE´s
 Some programms, mostly IDE´s got a function to rename the old file, create a new one and then rename it
 
 ### JetBrains IDE´s:
 (Ides like Webstorm, IntelliJ, PhpStorm etc.)
 If you keep getting a file like "file.js__JB__ or something then you need to disable the option "Save Write" in the settings.

## Codestuff
Its written quick & dirty in 1 day with C# by using WinForms.
I tested it so far and use it alot and never had any problems besindes of the ones above.
