﻿using CommandLine;
using CommandLine.Text;
using FileManager;
using FileManager.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Command
{
    /// <summary>
    /// This command prints content of one (or more files) to console with default (or specified) encoding.
    /// </summary>
    [Verb("print", HelpText = "cmdPrint", ResourceType = typeof(Localization))]
    class BasePrint : BaseCommand
    {
        public enum EncodingTypes
        {
            Default = 0,
            UTF8 = 1,
            ASCII = 2,
            Unicode = 3
        }

        [Option('e', "encoding", Default = EncodingTypes.Default, HelpText = "commonEncoding", ResourceType = typeof(Localization))]
        public EncodingTypes Encoding_ { get; set; }

        [Value(0, MetaName = "files", HelpText = "printFiles", Required = true, ResourceType = typeof(Localization))]
        public IEnumerable<string> Files { get; set; }

        [Usage(ApplicationAlias = "\b")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example(Localization.exampleCommon, 
                    new UnParserSettings { PreferShortName = true }, 
                    new BasePrint
                    {
                        Files = new string[] { "file.ext" },
                    });
                yield return new Example(Localization.exampleAdvanced,
                    new UnParserSettings { PreferShortName = true },
                    new BasePrint
                    {
                        Files = new string[] { "file1.ext", "file2.ext", "file3.ext" },
                        Encoding_ = EncodingTypes.ASCII,
                    });
            }
        }

        public override void Execute()
        {
            base.Execute();

            try
            {
                Encoding encoding = Encoding_ switch
                {
                    EncodingTypes.Default => Encoding.Default,
                    EncodingTypes.UTF8 => Encoding.UTF8,
                    EncodingTypes.ASCII => Encoding.ASCII,
                    EncodingTypes.Unicode => Encoding.Unicode,
                    _ => Encoding.Default,
                };

                foreach (var filePath in Files)
                {
                    if (!File.Exists(filePath))
                    {
                        throw new FileNotFoundException(string.Format(Localization.errCommonFileNotExists, filePath));
                    }
                }

                foreach (var filePath in Files)
                {
                    using (StreamReader reader = new StreamReader(filePath, encoding))
                    {
                        for (string s = reader.ReadLine(); s != null; s = reader.ReadLine())
                        {
                            Logger.PrintLine(s);
                        }
                    }
                    Logger.PrintLine();
                }
            }
            catch (Exception e)
            {
                Logger.PrintError(e.Message);
                OnError();
            }
        }
    }

    class QPrint : BasePrint, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NPrint : BasePrint, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
