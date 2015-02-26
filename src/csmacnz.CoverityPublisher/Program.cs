﻿using System;
using System.IO;
using System.Reflection;

namespace csmacnz.CoverityPublisher
{
    public class Program
    {
        public static void Main(string[] argv)
        {
            var args = new MainArgs(argv, exit: true, version: Assembly.GetEntryAssembly().GetName().Version);
            if (args.CmdPublish)
            {
                var payload = ParseInput(args);

                var results  =CoveritySubmitter.Submit(payload);
                if (results.Successful)
                {
                    Console.Error.WriteLine(results.Message);
                }
                else
                {
                    Console.Error.WriteLine(results.Message);
                    Environment.Exit(1);
                }
            }
            else if (args.CmdCompress)
            {

            }
        }

        private static Payload ParseInput(MainArgs args)
        {
            string coverityFileName = args.OptZip;
            if (!File.Exists(coverityFileName))
            {
                Console.Error.WriteLine("Input file '{0}' cannot be found.", coverityFileName);
                Environment.Exit(1);
            }

            string repoName = args.OptReponame;
            if (!repoName.Contains("/"))
            {
                Console.Error.WriteLine("Invalid repository name '{0}' provided.", repoName);
                Environment.Exit(1);
            }

            string coverityToken = args.OptToken;
            string description = args.OptDescription;
            string email = args.OptEmail;
            string version = args.OptCodeversion;
            bool dryrun = args.OptDryrun;
            var payload = new Payload
            {
                FileName = coverityFileName,
                RepositoryName = repoName,
                Token = coverityToken,
                Email = email,
                Version = version,
                Description = description,
                SubmitToCoverity = !dryrun
            };
            return payload;
        }

        public static string UnQuoted(string theString)
        {
            const char singleQuote = '\'';
            const char doubleQuote = '"';
            if ((theString[0] == doubleQuote && theString[theString.Length - 1] == doubleQuote)
                || (theString[0] == singleQuote && theString[theString.Length - 1] == singleQuote))
            {
                return theString.Substring(1, theString.Length - 2);
            }
            return theString;
        }
    }
}
