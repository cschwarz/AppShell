using SQLite;

namespace AppShell.Data
{
    public interface ISQLiteDatabase
    {
        SQLiteConnection GetConnection(string databaseName);
    }
}
