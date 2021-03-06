﻿using CommandLine;
using CommandLine.Text;
using Terminal.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities;

namespace Terminal
{
    class Terminal
    {
        static void Main(string[] args)
        {
            bool launchingWithArgs = args.Length != 0;

            // Printing program name, version and copyright info.
            if (!launchingWithArgs)
            {
                Console.WriteLine(HeadingInfo.Default);
                Console.WriteLine(CopyrightInfo.Default);
                Console.WriteLine("\"help\" for more info.");
                Console.WriteLine();
            }

            var parser = new Parser();

            WorkingCycle(args, launchingWithArgs, parser);
        }

        private static void WorkingCycle(string[] args, bool launchingWithArgs, Parser parser)
        {
            bool firstCommand = true;

            while (true)
            {
                if (launchingWithArgs && firstCommand)
                {
                    lastParserResult = parser.ParseArguments(args, quiteableCommands);
                    lastParserResult
                        .WithParsed<Command.ICommand>(Execute)
                        .WithNotParsed(HandleErrorsArgsLaunching);
                }
                else
                {
                    Console.Write("{0}> ", WorkingDir);
                    string readParameters = Console.ReadLine();

                    lastParserResult = parser.ParseArguments(ArgsParser.SplitCommandLine(readParameters ?? string.Empty), notQuiteableCommands);
                    lastParserResult
                        .WithParsed<Command.ICommand>(Execute)
                        .WithNotParsed(HandleErrors);
                }
                firstCommand = false;
            }
        }

        private static void Execute(Command.ICommand cmd)
        {
            cmd.Execute();

            // Adding empty line separator.
            Logger.PrintLine();

            // Exiting as -q (--quite) option is set.
            if (((Command.IQuite)cmd).Quite)
                Environment.Exit(0);
        }

        /// <summary>
        /// This method generates help text when a verb was specified,
        /// but an error occured (e. g. when parsing options)
        /// or when "--help" option was specified on a verb.
        /// It shows our options + "--help" option.
        /// </summary>
        private static HelpText GenerateOptionsHelpText() => HelpText.AutoBuild(
            lastParserResult,
            e =>
            {
                e.AutoHelp = true;
                e.AutoVersion = false;
                e.Heading = string.Empty;
                e.Copyright = string.Empty;
                e.AddNewLineBetweenHelpSections = true;
                return HelpText.DefaultParsingErrorsHandler(lastParserResult, e);
            },
            ex => ex
        );

        /// <summary>
        /// This method generates help text when "help" or "--help" command was specified.
        /// It shows our commands + "version" + "help" when no verb was specified.
        /// It show our options + "--help" option when a verb was specified.
        /// </summary>
        private static HelpText GenerateCommandsHelpText(bool autoVersion) => HelpText.AutoBuild(
            lastParserResult,
            e =>
            {
                e.AutoHelp = true;
                e.AutoVersion = autoVersion;
                e.Heading = string.Empty;
                e.Copyright = string.Empty;
                return HelpText.DefaultParsingErrorsHandler(lastParserResult, e);
            }
        );

        /// <summary>
        /// This parser is when launching w/o args or it's not the first command.
        /// </summary>
        /// <param name="errors"></param>
        private static void HandleErrors(IEnumerable<Error> errors)
        {
            var firstError = errors?.FirstOrDefault();

            // Checking whether a verb was specified.
            // We need this because "version" and "cmd --version"
            // throw the same error and we want to differentiate it.
            bool verbSpecified = lastParserResult.TypeInfo.Current != typeof(NullInstance) ||
                firstError is HelpVerbRequestedError err && err.Matched;

            switch (firstError)
            {
                case VersionRequestedError _:
                    if (verbSpecified)
                    {
                        Logger.PrintError(Localization.errTerminalVersionAsOption);
                    }
                    else
                    {
                        Console.WriteLine(HeadingInfo.Default);
                        Console.WriteLine();
                    }
                    return;
                // That's ok. Just continuing.
                case NoVerbSelectedError _:
                    return;
                case BadVerbSelectedError e:
                    Logger.PrintError(Localization.errTerminalBadCommand, e.Token);
                    Logger.PrintLine();
                    return;
                case HelpVerbRequestedError _:
                    Console.WriteLine(GenerateCommandsHelpText(!verbSpecified));
                    return;
                case Error e:
                    if (verbSpecified)
                    {
                        Console.WriteLine(GenerateOptionsHelpText());
                    }
                    else
                    {
                        Logger.PrintError(e.Tag.ToString());
                    }
                    return;
            }
        }

        /// <summary>
        /// This parser is when launching w/ args and it's the first command.
        /// </summary>
        /// <param name="errors"></param>
        private static void HandleErrorsArgsLaunching(IEnumerable<Error> errors)
        {
            var firstError = errors?.FirstOrDefault();

            // Checking whether a verb was specified.
            // We need this because "version" and "cmd --version"
            // throw the same error and we want to differentiate it.
            bool verbSpecified = lastParserResult.TypeInfo.Current != typeof(NullInstance) ||
               firstError is HelpVerbRequestedError err && err.Matched;

            switch (errors?.FirstOrDefault())
            {
                case VersionRequestedError _:
                    if (!verbSpecified)
                    {
                        Console.WriteLine(HeadingInfo.Default);
                        Environment.Exit(0);
                        return;
                    }
                    else
                    {
                        break;
                    }
                // That's ok. Just continuing.
                case NoVerbSelectedError _:
                    return;
                case BadVerbSelectedError _:
                    break;
                case HelpVerbRequestedError _:
                    Console.WriteLine(GenerateCommandsHelpText(!verbSpecified));
                    Environment.Exit(0);
                    return;
                case HelpRequestedError _:
                    Console.WriteLine(GenerateOptionsHelpText());
                    Environment.Exit(0);
                    return;
            }

            Environment.Exit(1);
        }

        /// <summary>
        /// Launch w/ args and it's the first command.
        /// </summary>
        private static readonly Type[] quiteableCommands = new Type[] {
            typeof(Command.QDisk),
            typeof(Command.QChangeDir),
            typeof(Command.QDir),
            typeof(Command.QPrint),
            typeof(Command.QCopy),
            typeof(Command.QMove),
            typeof(Command.QDeleteFile),
            typeof(Command.QWriteFile),
            typeof(Command.QColor),
            typeof(Command.QExit),
            typeof(Command.QClear),
            typeof(Command.QMakeDir),
            typeof(Command.QRemoveDir),
            typeof(Command.QHacker),
            typeof(Command.QHash),
            typeof(Command.QLanguage),
        };

        /// <summary>
        /// Launch w/o args or it's not the first command.
        /// </summary>
        private static readonly Type[] notQuiteableCommands = new Type[] {
            typeof(Command.NDisk),
            typeof(Command.NChangeDir),
            typeof(Command.NDir),
            typeof(Command.NPrint),
            typeof(Command.NCopy),
            typeof(Command.NMove),
            typeof(Command.NDeleteFile),
            typeof(Command.NWriteFile),
            typeof(Command.NColor),
            typeof(Command.NExit),
            typeof(Command.NClear),
            typeof(Command.NMakeDir),
            typeof(Command.NRemoveDir),
            typeof(Command.NHacker),
            typeof(Command.NHash),
            typeof(Command.NLanguage),
        };

        /// <summary>
        /// <see cref="ParserResult{}"/> of last parsing to work with in handlers.
        /// </summary>
        private static ParserResult<object> lastParserResult;

        /// <summary>
        /// Working directory.
        /// </summary>
        public static DirectoryInfo WorkingDir
        {
            get { return workingDir; }
            set { Directory.SetCurrentDirectory((workingDir = value).FullName); }
        }
        private static DirectoryInfo workingDir = new DirectoryInfo(Directory.GetCurrentDirectory());
    }
}
