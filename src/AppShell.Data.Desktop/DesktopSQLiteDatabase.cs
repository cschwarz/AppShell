using SQLite;
using System.IO;

namespace AppShell.Data.Desktop
{
    public class DesktopSQLiteDatabase : ISQLiteDatabase
    {
        protected IPlatformProvider platformProvider;

        public DesktopSQLiteDatabase(IPlatformProvider platformProvider)
        {
            this.platformProvider = platformProvider;
        }

        public SQLiteConnection GetConnection(string databaseName)
        {
            string databasePath = Path.Combine(platformProvider.GetDocumentFolderPath(), databaseName);
            string databaseDirectory = Path.GetDirectoryName(databasePath);

            if (!Directory.Exists(databaseDirectory))
                Directory.CreateDirectory(databaseDirectory);

            return new SQLiteConnection(databasePath);
        }
    }
}
