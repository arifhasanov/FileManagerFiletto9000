# Welcome to FILETTO9000 File Manager!

Hi! This software is developed by Arif Hasanov specially for the GeekBrains classes. Software shows files and folders in a numbered list. Many commands require line number of the mentioned file or folder.


# Commands

This program allows you to browse your file system, remove files or folders and also copy file or folder from one location to another location. For these operations following command lines are used. All commands are case insensitive:

## Help

This command brings in-software help windows with the list of all commands.
`help`

## Exit / Quit

Exit or Quit, you can use any of them. These commands closes the software and saves last current directory in the user profile. Next time when the software is launched, user starts where its left.
`exit`
`quit`

## Settings

You can change the screen size of the drawn area.
`settings`

## N

N means next page. When the current directory holds too many files or folders to be visible in one window, the software starts to page it. Number of elements per page can be set up from **Settings** command. 
`n`

## P

P means previous page. When the current directory holds too many files or folders to be visible in one window, the software starts to page it. Number of elements per page can be set up from **Settings** command. 
`p`

## Info \#

Writing "info" and then line number of the desired file/folder will show all basic information about the object like size, creation date, location etc.
`info 7`

## Delete \#

Writing "delete" and then line number of the desired file/folder will ask the user if he/she really wants to delete this object. After entering "Y" and pressing ENTER this file/folder will be permanently deleted. For example:
`delete 3`

## CD 'Directory_name'

CD = Change Directory, it is command to change the current view of the software to the mentioned directory after the "cd" command. The directory name can be both absolute and relative. For example if the current view has a folder "My Files" then, user must type:
`cd my files`
But user can also enter an absolute path. For example:
`cd C:\users\john\documents`
This also means writing two dots (..) after the cd command will change current view to the parent folder of the current folder:
`cd..`
or 
`cd ..`

## Copy \#

Writing "copy" and then line number of the desired file/folder will ask the user where he/she wants to copy this object. After entering destination folder and pressing ENTER selected file/folder will be copied to the target folder. IMPORTANT: For the destination no need to show file name - only folder name. For example:
We want to copy object #8 from the view (for example 'MyFile.txt' ) to the following folder: "C:\Temp"
`copy 8`
`//Where do you want to copy mentioned file? Please enter absolute path of the existing directory:`
`C:\Temp`

