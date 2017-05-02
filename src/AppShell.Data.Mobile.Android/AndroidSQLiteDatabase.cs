using SQLite;
using System.IO;

namespace AppShell.Data.Mobile.Android
{
    public class AndroidSQLiteDatabase : ISQLiteDatabase
    {
        protected IPlatformProvider platformProvider;

        public AndroidSQLiteDatabase(IPlatformProvider platformProvider)
        {
            this.platformProvider = platformProvider;
        }

        public SQLiteConnection GetConnection(string databaseName)
        {
            string databasePath = Path.Combine(platformProvider.GetDocumentFolderPath(), databaseName);
            string databaseDirectory = Path.GetDirectoryName(databasePath);

            if (!Directory.Exists(databaseDirectory))
                Directory.CreateDirectory(databaseDirectory);

            SQLiteConnection sqlLiteConnection = new SQLiteConnection(databasePath);
            sqlLiteConnection.BusyTimeout  = System.TimeSpan.FromSeconds(30);
            return sqlLiteConnection;
        }
    }
}
