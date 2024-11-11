using Spectre.Console.Cli;
using Spectre.Console;
using CDFX_Toolkit;
/*
 * alita <command>
 * 
 * --- commands ---
 * cdfx combine {<FILE1> <FILE2> [FILES...] | <LIST>}
 */

public static class MainProgram
{
    public static int Main(string[] args)
    {
        var app = new CommandApp<CombineFileCommand>();
        return app.Run(args);
    }
}