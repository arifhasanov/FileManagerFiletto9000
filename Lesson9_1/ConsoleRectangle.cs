public class ConsoleRectangle
{
    private int hWidth;
    private int hHeight;
    private Point hLocation;
    private ConsoleColor hBorderColor;
    List<string> files;

    public ConsoleRectangle(int width, int height, Point location, ConsoleColor borderColor)
    {
        hWidth = width;
        hHeight = height;
        hLocation = location;
        hBorderColor = borderColor;
    }

    public Point Location
    {
        get { return hLocation; }
        set { hLocation = value; }
    }

    public int Width
    {
        get { return hWidth; }
        set { hWidth = value; }
    }

    public int Height
    {
        get { return hHeight; }
        set { hHeight = value; }
    }

    public ConsoleColor BorderColor
    {
        get { return hBorderColor; }
        set { hBorderColor = value; }
    }

    public void Draw(List<string> iFiles, string currentFolder, int currentPage, int totalPages)
    {
        files = iFiles;
        string s = "╔";
        string space = "";
        string temp = "";
        space += spaces(18 + currentFolder.Length);
        s += $"═CURRENT FOLDER: {currentFolder} ";

        for (int i = (18 + currentFolder.Length); i < Width; i++)
        {
            space += " ";
            s += "═";
        }

        for (int j = 0; j < Location.X; j++)
            temp += " ";

        s += "╗" + "\n";

        for (int i = 0; i < Height; i++)
        {
            if(i < files.Count)
            {
                if (files[i].Length > 0 && ((currentPage - 1) * Settings.Default.Height + (i)) < files.Count)
                {
                    string lineNo = ((currentPage - 1) * Settings.Default.Height + (i + 1)).ToString() + ". ";
                    int line = (currentPage - 1) * Settings.Default.Height + (i);
                    space = lineNo + files[line] + spaces(Width - files[line].Length - lineNo.Length);
                }
                else
                {
                    space = spaces(Width);
                }
            }
            else
            {
                space = spaces(Width);
            }

            s += temp + "║" + space + "║" + "\n";
        }


        int p1factor = currentPage.ToString().Length, p2factor = totalPages.ToString().Length;

        s += temp + "╚";
        if(files.Count > Settings.Default.Height)
        {
            for (int i = 0; i < Width-(13+p1factor+p2factor); i++)
            {
                s += "═";
            }

            s += $"═ PAGE {currentPage} of {totalPages} ═";
            
            s += "╝" + "\n";
        }
        else
        {
            for (int i = 0; i < Width; i++)
            {
                s += "═";
            }
            s += "╝" + "\n";
        }

        Console.ForegroundColor = BorderColor;
        Console.CursorTop = hLocation.Y;
        Console.CursorLeft = hLocation.X;
        Console.Write(s);
        Console.ResetColor();
    }

    string spaces(int numberOfSpaces)
    {
        string space = "";
        for (int i = 0; i < numberOfSpaces; i++)
        {
            space += " ";
        }
        return space;
    }
}