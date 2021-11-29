//declaration of variables
bool stay = true;
string command;
List<string> listOfObjects = new List<string>();
DirectoryInfo[] directories;
FileInfo[] files;
int currentPage = 1;
int totalPages = 1;

//as a start getting documents folder
string currentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
if(Settings.Default.Folder != "")
{
    currentDirectory = Settings.Default.Folder;
}

//filling the list of all folders and files of the currentDirectory
FillList();

//instantiating drawing class with set parameters
ConsoleRectangle cr = new ConsoleRectangle(Settings.Default.Width, Settings.Default.Height, new System.Drawing.Point(0,0), ConsoleColor.Cyan);

//welcoming user and explaining some basic commands
WelcomeMessages();

//main body of the program. with each loop all screen is redrawn
while (stay)
{
    //drawing main frame
    CallDraw();
    //reading command from end user
    Console.Write($"Enter command ('HELP' for instructions, 'SETTINGS' for setup screen size, 'EXIT' or 'QUIT' to quit)  \n {currentDirectory}>");
    command = Console.ReadLine().ToString();

    //checking if any command was entered
    if(!String.IsNullOrEmpty(command))
    {
        if (command.ToLower() == "exit" || command.ToLower() == "quit")
        {
            Settings.Default.Folder = currentDirectory;
            Settings.Default.Save();
            stay = false;
            Console.Write("Thank you for working with FILETTO 9000! We hope you liked it. See you again!");
            Console.Read();
        }
        if (command.ToLower() == "settings")
        {
            command = "";
            Console.Clear();
            Console.WriteLine($"Please enter width of the window (Blue lines), currently: {Settings.Default.Width} : ");
            int width = Settings.Default.Width;
            if (int.TryParse(Console.ReadLine(), out width))
            {
                if(width > 0 && width < 200)
                {
                    Settings.Default.Width = width;
                    Settings.Default.Save();
                }
            }
            Console.WriteLine($"Please enter height of the window (Blue lines, it also defines number of files/directories visible per page), currently: {Settings.Default.Height} : ");
            int height = Settings.Default.Height;
            if (int.TryParse(Console.ReadLine(), out height))
            {
                if (height > 0 && height < 30)
                {
                    Settings.Default.Height = height;
                    Settings.Default.Save();
                }
            }
            command = "";
            Console.Clear();
            cr = new ConsoleRectangle(Settings.Default.Width, Settings.Default.Height, new System.Drawing.Point(0, 0), ConsoleColor.Cyan);
            RecalculatePages();
            CallDraw();
        }
        if (command.ToLower() == "n") //next page
        {
            if(totalPages > 1 && currentPage < totalPages)
            {
                currentPage++;
                CallDraw();
            }
        }
        if (command.ToLower() == "p") //previous page
        {
            if (totalPages > 1 && currentPage > 1)
            {
                currentPage--;
                CallDraw();
            }
        }
        if (command.Length > 3) 
        {
            if(command.ToLower().Substring(0, 4) == "info")
            {
                int fileNo = 0;
                if (command.Length > 5)
                {
                    if (int.TryParse(command.Trim().Substring(5, command.Length - 5), out fileNo))
                    {
                        if (fileNo <= listOfObjects.Count)
                        {
                            Console.WriteLine("=======================================");
                            if (Directory.Exists(currentDirectory + "\\" + listOfObjects[fileNo - 1]))
                            {
                                DirectoryInfo di = new DirectoryInfo(currentDirectory + "\\" + listOfObjects[fileNo - 1]);
                                Console.WriteLine($"Directory name:                {di.FullName}");
                                Console.WriteLine($"Creation date and time:   {di.CreationTime}");
                                Console.WriteLine($"Last write date and time: {di.LastWriteTime}");
                                Console.WriteLine($"Directory size:                {(Decimal.Parse(DirSize(di).ToString()) / 1024).ToString("N2")} KB");
                            }
                            else
                            {
                                FileInfo fi = new FileInfo(currentDirectory + "\\" + listOfObjects[fileNo - 1]);
                                Console.WriteLine($"File name:                {fi.FullName}");
                                Console.WriteLine($"Creation date and time:   {fi.CreationTime}");
                                Console.WriteLine($"Last write date and time: {fi.LastWriteTime}");
                                Console.WriteLine($"Is read-only:             {fi.IsReadOnly}");
                                Console.WriteLine($"File size:                {(Decimal.Parse(fi.Length.ToString()) / 1024).ToString("N2")} KB");
                            }

                            Console.WriteLine("=======================================");
                            Console.WriteLine("press ENTER to continue...");
                            Console.Read();
                        }
                        else
                        {
                            ShowError("There is no file or directory with the selected number");
                        }
                    }
                }
            }
            else if (command.Substring(0, 2).ToLower() == "cd")
            {
                string tempDirectory = currentDirectory;

                if (command.ToLower() == "cd..")
                {
                    if (Directory.GetParent(currentDirectory) != null)
                    {
                        currentDirectory = Directory.GetParent(currentDirectory).FullName;
                        currentPage = 1;
                        FillList();
                    }
                    else
                    {
                        ShowError("You are already in root folder! Can't go to upper level");
                    }
                }
                else
                {
                    if (IsFullPath(command.Substring(3, command.Length - 3)))
                    {
                        tempDirectory = command.Substring(3, command.Length - 3);
                    }
                    else
                    {
                        string tempDirPath = tempDirectory.Substring(tempDirectory.Length - 2, 2);
                        if (tempDirPath != ":\\")
                        {
                            tempDirectory += "\\";
                        }
                        tempDirectory += command.Substring(3, command.Length - 3);
                    }

                    if (Directory.Exists(tempDirectory))
                    {
                        currentDirectory = tempDirectory;
                        currentPage = 1;
                        FillList();
                    }
                    else
                    {
                        Console.WriteLine("Folder does not exist. Try again. Press ENTER.");
                        Console.Read();
                    }
                }
            }
        }
        if (command.ToLower() == "help")
        {
            Console.Clear();
            Console.WriteLine("==============================================================");
            Console.WriteLine("========================COMMANDS LIST=========================");
            Console.WriteLine("==============================================================");
            Console.WriteLine("LIST -                          To list all files and directory's (usually it is shown automatically when you change the directory");
            Console.WriteLine();
            Console.WriteLine("EXIT, QUIT -                    To save the current directory location and leave the program");
            Console.WriteLine();
            Console.WriteLine("SETTINGS -                      To change the window size of the program");
            Console.WriteLine();
            Console.WriteLine("CD 'DIRECTORY_NAME' -           Change directory to the mentioned directory. The name of the directory can be both relative, as well as absolute");
            Console.WriteLine();
            Console.WriteLine("CD.. -                          Change directory to the parent directory of current directory");
            Console.WriteLine();
            Console.WriteLine("N -                             Next page");
            Console.WriteLine();
            Console.WriteLine("P -                             Previous page");
            Console.WriteLine();
            Console.WriteLine("COPY # to 'TARGET_DIRECTORY' -  Copy the file or directory with the mentioned number (#) to the mentioned TARGET path. In the TARGET part show only target directory. For example you want to copy myFile.txt from current directory into 'C:\\Temp\\' directory. Lets say this file has line number 5. in the current view. So the command will be: \n COPY 5 to 'C:\\Temp\\'");
            Console.WriteLine();
            Console.WriteLine("DELETE # -                      Delete the file or directory with the mentioned number (#). ");
            Console.WriteLine();
            Console.WriteLine("INFO # -                        Get information about the the file or directory with the mentioned number (#). ");
            Console.WriteLine("==============================================================");
            Console.WriteLine("press ENTER to continue...");
            Console.Read();
        }
        if (command.Length > 5)
        {
            if(command.ToLower().Substring(0, 6) == "delete")
            {
                int fileNo = 0;
                if (command.Length > 7)
                {
                    if (int.TryParse(command.Trim().Substring(7, command.Length - 7), out fileNo))
                    {
                        if (fileNo <= listOfObjects.Count)
                        {
                            Console.WriteLine("Do you really want to delete the mentioned file/directory? Y/N");
                            string decision = Console.ReadLine().Substring(0, 1);
                            if (decision.ToLower() == "y")
                            {
                                if (Directory.Exists(currentDirectory + "\\" + listOfObjects[fileNo - 1]))
                                {
                                    DirectoryInfo di = new DirectoryInfo(currentDirectory + "\\" + listOfObjects[fileNo - 1]);
                                    di.Delete();
                                }
                                else
                                {
                                    FileInfo fi = new FileInfo(currentDirectory + "\\" + listOfObjects[fileNo - 1]);
                                    fi.Delete();
                                }

                                FillList();
                                CallDraw();
                                Console.WriteLine("selected file/directory was deleted");
                                Console.WriteLine("press ENTER to continue...");
                                Console.Read();
                            }
                        }
                        else
                        {
                            ShowError("There is no file or directory with the selected number");
                        }
                    }
                }
            }
            if (command.ToLower().Substring(0, 4) == "copy")
            {
                int fileNo = 0;
                if (command.Length > 5)
                {
                    if (int.TryParse(command.Trim().Substring(5, command.Length - 5), out fileNo))
                    {
                        if (fileNo <= listOfObjects.Count)
                        {
                            string copyFrom = currentDirectory + "\\" + listOfObjects[fileNo - 1];

                            if (Directory.Exists(copyFrom)) // checking if the selected object is directory
                            {
                                Console.WriteLine("Where do you want to copy mentioned directory? Please enter absolute path of the existing directory:");
                                string path = Console.ReadLine();
                                if (Directory.Exists(path))
                                {
                                    DirectoryCopy(copyFrom, path, true);
                                    Console.WriteLine("Copy process is completed!");
                                    Console.WriteLine("press ENTER to continue...");
                                    Console.Read();
                                }
                                else
                                {
                                    ShowError("Destination folder does not exist. Please create the folder first.");
                                }
                            }
                            else //if it is not directory, then it is a file
                            {
                                Console.WriteLine("Where do you want to copy mentioned file? Please enter absolute path of the existing directory:");
                                string path = Console.ReadLine();
                                if (Directory.Exists(path))
                                {
                                    FileInfo fiFrom = new FileInfo(copyFrom);
                                    fiFrom.CopyTo(path + "\\" + listOfObjects[fileNo - 1]);
                                    Console.WriteLine("Copy process is completed!");
                                    Console.WriteLine("press ENTER to continue...");
                                    Console.Read();
                                }
                                else
                                {
                                    ShowError("Destination folder does not exist. Please create the folder first.");
                                }
                            }
                        }
                        else
                        {
                            ShowError("There is no file or directory with the selected number");
                        }
                    }
                }
            }
        }
        if (command.ToLower() == "list")
        {
            FillList();
            CallDraw();
        }
        command = "";
        Console.Clear();
    }
}

void FillList()
{
    try
    {
        listOfObjects = new List<string>();
        var directory = new DirectoryInfo(currentDirectory);
        directories = directory.GetDirectories().OrderBy(p => p.Name).ToArray();
        files = directory.GetFiles().OrderBy(p => p.Name).ToArray();
        for (int i = 0; i < directories.Length; i++)
        {
            listOfObjects.Add(directories[i].Name);
        }
        for (int i = 0; i < files.Length; i++)
        {
            listOfObjects.Add(files[i].Name);
        }
        RecalculatePages();
    }
    catch (Exception ex)
    {
        ShowError(ex.Message);
    }
}

void CallDraw()
{
    Console.Clear();
    cr.Draw(listOfObjects, currentDirectory, currentPage, totalPages);
}

void RecalculatePages()
{
    Decimal linesQty = (Decimal)listOfObjects.Count;
    Decimal maxQty = (Decimal)Settings.Default.Height;
    totalPages = (int)Math.Ceiling(linesQty / maxQty);
}
