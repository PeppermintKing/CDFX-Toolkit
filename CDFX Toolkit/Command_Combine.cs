using System;
using System.Xml;
using Spectre.Console.Cli;

public class FileSettings : CommandSettings
{
    [CommandArgument(0, "<filepaths...>")]
    public string[] Filepaths { get; set; }
}

public class CombineFileCommand : Command<FileSettings>
{
    public override int Execute(CommandContext context, FileSettings settings)
    {
        return 0;
    }
}