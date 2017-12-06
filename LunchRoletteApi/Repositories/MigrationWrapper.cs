using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using Dapper;
using Oracle.DataAccess.Client;

namespace LunchRoletteApi.Repositories
{
    public class MigrationsWrapper
    {
        private static Assembly migrationsAssembly = Assembly.Load("Migrations");

        private string connectionString;
        private Action<string> logMethod = Console.WriteLine;

        public MigrationsWrapper(string connectionString, Action<string> logMethod = null)
        {
            this.connectionString = connectionString;
            if (logMethod != null)
            {
                this.logMethod = logMethod;
            }
        }

        private MigrationRunner GetMigrator()
        {
            var announcer = new NullAnnouncer();

            var migrationContext = new RunnerContext(announcer);
            var options = new MigrationOptions { PreviewOnly = false, Timeout = 60 };
            var factory = new FluentMigrator.Runner.Processors.Oracle.OracleProcessorFactory();
            var processor = factory.Create(this.connectionString, announcer, options);
            var runner = new MigrationRunner(migrationsAssembly, migrationContext, processor);

            return runner;
        }

        public void MigrateToLatestVersion()
        {
            var runner = GetMigrator();
            runner.MigrateUp(LatestVersionNumber);
        }

        public void MigrateToVersion(int version)
        {
            var runner = GetMigrator();

            long currentVersion = CurrentVersionNumber;

            if (currentVersion < version)
            {
                runner.MigrateUp(version);
            }
            else if (currentVersion > version)
            {
                runner.MigrateDown(version);
            }
        }

        public long LatestVersionNumber
        {
            get
            {
                long toReturn = 0;
                // Look through all types
                foreach (Type t in migrationsAssembly.GetTypes())
                {
                    // Get all the types with MigrationAttribute (object[] because it can have multiple Migration attributes)
                    object[] attributes = t.GetCustomAttributes(typeof(MigrationAttribute), true);
                    if (attributes.Length > 0)
                    {
                        // Get the max of (current max, max version specified in this Type's Migration attributes)
                        toReturn = Math.Max(toReturn, attributes.Max(o => (o as MigrationAttribute).Version));
                    }
                }

                return toReturn;
            }
        }

        public long CurrentVersionNumber
        {
            get
            {
                long toReturn = 0;

                using (var conn = new OracleConnection(this.connectionString))
                {
                    conn.Open();

                    try
                    {
                        toReturn = conn.Query<long>("SELECT MAX(Version) FROM VersionInfo").First();
                    }
                    catch (OracleException)
                    {
                        toReturn = 0;
                    }
                }
                return toReturn;
            }
        }

        private class MigrationOptions : IMigrationProcessorOptions
        {
            public bool PreviewOnly { get; set; }
            public string ProviderSwitches { get; set; }
            public int Timeout { get; set; }
        }
    }
}
