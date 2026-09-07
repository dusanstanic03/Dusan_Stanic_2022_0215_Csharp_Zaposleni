using Microsoft.Data.SqlClient;

string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=2022_0215_zaposleni;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30";



Kasir k = new Kasir
{
    Id = 1,
    ImeIPrezime = "Ana Anic",
    ProdavnicaId = 1
};

Racun r = new Racun
{
    Id = 1,
    Broj = "11AA",
    Datum = DateTime.Now,
    Iznos = 0,
    KasirId = 1
};



AzurirajRacun(r, 1);
NagradiKasira();

void AzurirajRacun(Racun r, int redniBroj)
{
    try
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        using SqlCommand command = new SqlCommand("sp_azuriraj_racun", connection);
        command.CommandType = System.Data.CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@RacunId", r.Id);
        command.Parameters.AddWithValue("@RedniBroj", redniBroj);


        command.ExecuteNonQuery();

        Console.WriteLine("Racun je uspesno azuriran.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Greska prilikom azuriranja racuna " + ex.Message);
    }
}

void NagradiKasira()
{
    try
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        string sql =
            @"SELECT TOP 1 K.Id, K.ImePrezime, K.ProdavnicaId,COUNT(R.Id) AS BrojRacuna
        FROM Kasir K JOIN Racun R ON K.Id = R.KasirId
        GROUP BY K.Id, K.ImePrezime, K.ProdavnicaId
        ORDER BY COUNT(R.Id) DESC;
        ";

        using SqlCommand kasirCommand = new SqlCommand(sql, connection);
        int kasirId;
        string imePrezime;
        int prodavnicaId;
        int brojRacuna;

        using (SqlDataReader reader = kasirCommand.ExecuteReader())
        {

            if (!reader.Read())
            {
                Console.WriteLine("Nema kasira sa kreiranim racunima.");
                return;
            }

            kasirId = (int)reader["Id"];
            imePrezime = (string)reader["ImePrezime"];
            prodavnicaId = (int)reader["ProdavnicaId"];
            brojRacuna = (int)reader["BrojRacuna"];
        }

        string racunIdSQl = "SELECT dbo.fn_vratiNajveciRacunZaKasira(@KasirId)";
        using SqlCommand racunCommand = new SqlCommand(racunIdSQl, connection);
        racunCommand.Parameters.AddWithValue("@KasirId", kasirId);

        object result = racunCommand.ExecuteScalar();

        if (result == DBNull.Value || result == null)
        {
            Console.WriteLine("Kasir nema racun sa stavkama");
            return;
        }

        int racunId = (int)result;

        string sqlRacun =
            @"SELECT Id, Broj, Datum, Iznos, KasirId
        FROM Racun
        WHERE Id = @RacunId;";
        using SqlCommand sqlRacunCommand = new SqlCommand(sqlRacun, connection);
        sqlRacunCommand.Parameters.AddWithValue("@RacunId", racunId);

        using SqlDataReader readerRacun = sqlRacunCommand.ExecuteReader();

        using StreamWriter writer = new StreamWriter("nagrada.txt");
        writer.WriteLine("Nagradjeni kasir: ");
        writer.WriteLine($"KasirId: {kasirId}");
        writer.WriteLine($"imePrezime: {imePrezime}");
        writer.WriteLine($"prodavnicaId: {prodavnicaId}");
        writer.WriteLine($"brojRacuna: {brojRacuna}");

        if (readerRacun.Read())
        {
            writer.WriteLine();
            writer.WriteLine("Podaci o racunu: ");
            writer.WriteLine($"racunId: {readerRacun["Id"]}");
            writer.WriteLine($"broj: {readerRacun["Broj"]}");
            writer.WriteLine($"Datum: {readerRacun["Datum"]}");
            writer.WriteLine($"Iznos: {readerRacun["Iznos"]}");
            writer.WriteLine($"KasirId: {readerRacun["KasirId"]}");
        }
        Console.WriteLine("Izvestaj nagrada.txt je uspesno kreiran.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("GReska pri nagradjivcanju kasira." + ex.Message);
    }
}




class Racun
{
    public int Id { get; set; }
    public string Broj { get; set; }
    public DateTime Datum { get; set; }
    public decimal Iznos { get; set; }
    public int KasirId { get; set; }
}

class Kasir
{
    public int Id { get; set; }
    public string ImeIPrezime { get; set; }
    public int ProdavnicaId { get; set; }
}
