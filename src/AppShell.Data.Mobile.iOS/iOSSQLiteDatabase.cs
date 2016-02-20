using SQLite;
using System.IO;

namespace AppShell.Data.Mobile.iOS
{
    public class iOSSQLiteDatabase : ISQLiteDatabase
    {
        protected IPlatformProvider platformProvider;

        public iOSSQLiteDatabase(IPlatformProvider platformProvider)
        {
            this.platformProvider = platformProvider;
        }

        public SQLiteConnection GetConnection(string databaseName)
        {
            string databasePath = Path.Combine(platformProvider.GetDocumentFolderPath(), "..", "Library", databaseName);
            string databaseDirectory = Path.GetDirectoryName(databasePath);

            if (!Directory.Exists(databaseDirectory))
                Directory.CreateDirectory(databaseDirectory);

            return new SQLiteConnection(databasePath);
        }
    }
}