namespace DataAccess.DataContext;

public static class ConnString
{
    public static string SqlServer(
        string database = "StoreDatabase",
        string server = "localhost,1583"
    )
    {
        return $"Server={server};Initial Catalog={database};User ID=sa;Password=Nam09189921;TrustServerCertificate=True;Encrypt=False";
    }
}
