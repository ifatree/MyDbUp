using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDbUp
{
    public class Program
    {
        // properties
        public static string connectionName = "MyDbUp";
        public static string connectionString = "Server=(local)\\SqlExpress; Database=MyApp; Trusted_connection=true";

        public static string scriptFolder = ".";
        public static string action = "update";

        public static bool usage = false;
        public static bool journalOnly = false;

        private static DbUp.Engine.DatabaseUpgradeResult result;

        protected static int Main(string[] args)
        {
            try
            {
                // parse command line args
                var opts = new Mono.Options.OptionSet()
                    {
                        { "scriptFolder=", v => scriptFolder = v },
                        { "connectionString=", v => connectionString = v },
                        { "connectionName=", v => connectionName = v },

                        { "j|journalOnly", v => journalOnly = true },
                        { "?|help", v => usage = true }
                    };

                List<string> extra = opts.Parse(args);

                // default to showing usage
                if (args.Count() == 0 || extra.Count == 0)
                    usage = true;

                if (usage)
                {
                    var processName = Process.GetCurrentProcess().ProcessName;
                    Console.WriteLine("usage: {0} ACTION [OPTIONS]", processName);
                    Console.WriteLine("       {0} ACTION [SCRIPT_FOLDER] [OPTIONS]", processName);

                    // print options and bail
                    Console.WriteLine(String.Empty);
                    opts.WriteOptionDescriptions(Console.Out);
                    return 0;
                } 
                else
                {
                    // get un-named options (from back to front)
                    if (extra.Count > 1) scriptFolder = extra[1];
                    if (extra.Count > 0) action = extra[0];

                    // validate target folder exists
                    var folder = Path.GetFullPath(Path.Combine(scriptFolder.ToLower(), action.ToLower()));
                    if (!Directory.Exists(folder))
                        throw new FileNotFoundException("Cannot access script folder", folder);

                    // create connection
                    var connectionStringObject =
                        ConfigurationManager.ConnectionStrings[connectionName] ??
                            new ConnectionStringSettings(connectionName, connectionString);

                    // run blank scripts if only journaling
                    DbUp.Engine.IScriptPreprocessor journalingPreprocessor = new PassThruPreprocessor();
                    if (journalOnly)
                        journalingPreprocessor = new BlankScriptPreprocessor();

                    // perform migration actions
                    var migrator =
                        DbUp.DeployChanges.To
                            .SqlDatabase(connectionStringObject.ConnectionString)
                            .WithTransaction()
                            .WithScriptsFromFileSystem(folder)
                            .WithPreprocessor(journalingPreprocessor)
                            .LogToConsole()
                            .Build();

                    result = migrator.PerformUpgrade();
                }

                // write status to console and exit
                if (result.Successful)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Success!");
                    Console.ResetColor();
                    return 0;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Failed!");
                    Console.ResetColor();
                    return -1;
                }
            }
            catch(Exception ex)
            {
                result = new DbUp.Engine.DatabaseUpgradeResult(new List<DbUp.Engine.SqlScript>(), false, ex);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Errored!");
                Console.ResetColor();
                return -1;
            }
        }

        protected class BlankScriptPreprocessor : DbUp.Engine.IScriptPreprocessor
        {
            string DbUp.Engine.IScriptPreprocessor.Process(string contents)
            {
                return String.Empty;
            }
        }

        protected class PassThruPreprocessor : DbUp.Engine.IScriptPreprocessor
        {
            string DbUp.Engine.IScriptPreprocessor.Process(string contents)
            {
                return contents;
            }
        }
    }
}
