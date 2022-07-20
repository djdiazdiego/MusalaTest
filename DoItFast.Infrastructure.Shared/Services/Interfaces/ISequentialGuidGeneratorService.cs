namespace DoItFast.Infrastructure.Shared.Services.Interfaces
{
    /// <summary>
    /// Sequential Guid for Microsoft SQL Server.
    /// </summary>
    public interface ISqlGuidGenerator
    {
        /// <summary>
        /// Returns a guid for the value of UtcNow.
        /// </summary>
        /// <returns></returns>
        Guid NewGuid();
    }

    /// <summary>
    /// Sequential Guid for MySQL and PosgreSQL.
    /// </summary>
    public interface IMySQLAndPostgreSQLGuidGenerator
    {
        /// <summary>
        /// Returns a guid for the value of UtcNow.
        /// </summary>
        /// <returns></returns>
        Guid NewGuid();
    }

    /// <summary>
    /// Sequential Guid for Oracle.
    /// </summary>
    public interface IOracleGuidGenerator
    {
        /// <summary>
        /// Returns a guid for the value of UtcNow.
        /// </summary>
        /// <returns></returns>
        Guid NewGuid();
    }

    /// <summary>
    /// Sequential Guid for SQLite.
    /// </summary>
    public interface ISQLiteGuidGenerator
    {
        /// <summary>
        /// Returns a guid for the value of UtcNow as either 16 bytes of binary data.
        /// </summary>
        /// <returns></returns>
        Guid NewBytesGuid();

        /// <summary>
        /// Returns a guid for the value of UtcNow as either 36 characters of text.
        /// </summary>
        /// <returns></returns>
        Guid NewStringGuid();
    }
}
