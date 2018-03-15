# AutoFlatCopy

AutoFlatCopy is a small Windows too wich resides in the Taskbar.

It will permamently synchronize files from a Source Folder to a Destination Folder,
but insteald of just Copying the files it will flatten the File names and put them all into the root directiory of the Destination Folder.

The directories will be replaced with name prefixes separated by a dot.

I Created this Tool to edit my Code for the game Screeps.com, but this game not allows the source code to contain any subfolders,
but specially in bigger code projects i cant live without keep the stuff organized in folders.

 original file path | new file path 
--- | --- 
 notherfile.txt | notherfile.txt 
 /folder/file1.txt | /folder.file1.txt 
 /folder/file2.ping | /folder.file2.png 
 /folder/subfolderA/someFile.png | /folder.subfolderA.someFile.png 
 /folder/subfolderB/someExtraFile.png | /folder.subfolderB.someExtraFile.png 
 
 ## A known problem
 
 Deleteing a folder with contents can actually confuse the tool, so if you want to delete a folder, try to delete all files first and then the folder.
 
 ## Using with IDE´s
 Some programms, mostly IDE´s got a function to rename the old file, create a new one and then rename it
 
 ### JetBrains IDE´s:
 (Ides like Webstorm, IntelliJ, PhpStorm etc.)
 If you keep getting a file like "file.js__JB__ or something then you need to disable the option "Save Write" in the settings.
