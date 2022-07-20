using System;

namespace DoItFast.Domain.Settings
{
    public class SqlServerSettings
    {
        /// <summary>
        /// Migrations assembly name.
        /// </summary>
        public MigrationsAssembly MigrationsAssemblyName { get; set; }

        /// <summary>
        /// The maximum number of retry attempts.
        /// </summary>
        public int MaxRetryCount { get; set; }

        /// <summary>
        /// The maximum delay between retries.
        /// </summary>
        public TimeSpan MaxRetryDelay { get; set; }

        /// <summary>
        /// Additional SQL error numbers that should be considered transient.
        /// </summary>
        public int[] ErrorNumbersToAdd { get; set; }

        public class MigrationsAssembly
        {
            public string Persistence { get; set; }
            public string Identity { get; set; }
        }
    }
}
