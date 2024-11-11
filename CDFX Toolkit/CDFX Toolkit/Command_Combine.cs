using System;
using System.Xml;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CDFX_Toolkit
{
    ///
    internal sealed class CombineFileCommand : Command<CombineFileCommand.Settings>
    {
        /// <summary>
        /// 
        /// </summary>
        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "<filepaths...>")]
            public string[]? Filepaths { get; set; }

            public override ValidationResult Validate()
            {
                //TODO : Validate schema
                return ValidationResult.Success();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public override int Execute(CommandContext context, Settings settings)
        {
            XmlDocument document = new XmlDocument();
            document.Load(settings.Filepaths[0]);

            /* The <SW-SYSTEM> element, which contains one or multiple systems for which the cdfx-file holds the Calibration data. 
             * The purpose of this element is to allow the Calibration data for multiple ECUs, multi-processor ECUs or multicore-CPU 
             * ECUs to be put into one cdfx-file. One set of Calibration data for one unit is contained in one <SW-SYSTEM> element.
             */

            //TODO : Check there is at least 1 SW-SYSTEM
            XmlNodeList? systemList = document.SelectNodes("//SW-SYSTEM");

            foreach (string file in context.Arguments.Skip(1))
            {
                XmlDocument iDocument = new XmlDocument();
                iDocument.Load(file);
                XmlNodeList? iSystemList = document.SelectNodes("//SW-SYSTEM");

                bool found = false;
                for (int i = 0; i < iSystemList.Count; i++)
                {
                    found = false;
                    for(int j = 0; j < systemList.Count; j++)
                    {
                        if (systemList[j].SelectSingleNode("//SHORT-NAME").Value == iSystemList[i].SelectSingleNode("//SHORT-NAME").Value)
                        {
                            found = true;

                            //copy existing SW-INSTANCE nodes over into matching SW-SYSTEM
                            XmlNode node = systemList[j].SelectSingleNode("//SW-INSTANCE-TREE");
                            XmlNodeList? iElemList = iDocument.SelectNodes("//SW-INSTANCE");
                            foreach (XmlNode iElem in iElemList) 
                            {
                                XmlNode newElem = document.ImportNode(iElem, true);
                                node.AppendChild(newElem); 
                            }

                            break;
                        }
                    }

                    if (!found)
                    {
                        //Append the new SW-SYSTEM
                        XmlNode node = document.SelectSingleNode("//SW-SYSTEMS");
                        node.AppendChild(iSystemList[i]);
                    }
                }
            }

            //output file
            document.Save("output.cdfx");

            return 0;
        }
    } 
}
