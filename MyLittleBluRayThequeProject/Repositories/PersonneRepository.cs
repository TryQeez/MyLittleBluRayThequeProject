using MyLittleBluRayThequeProject.DTOs;
using Npgsql;
namespace MyLittleBluRayThequeProject.Repositories
{
    public class PersonneRepository
    {
        public PersonneRepository() { }

        public List<Personne> GetListePersonne()
        {
            NpgsqlConnection conn = null;
            List<Personne> result = new List<Personne>();
            try
            {
                // Connect to a PostgreSQL database
                conn = new NpgsqlConnection("Server=127.0.0.1;User Id=postgres;Password=root;Database=postgres;");
                conn.Open();

                // Define a query returning a single row result set
                NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM \"BluRayTheque\".\"Personne\"", conn);


                // Execute the query and obtain a result set
                NpgsqlDataReader dr = command.ExecuteReader();

                // Output rows
                while (dr.Read())
                    result.Add(new Personne
                    {
                        Id = long.Parse(dr[0].ToString()),
                        Nom = dr[1].ToString(),
                        Prenom = dr[2].ToString(),
                        Nationalite = dr[4].ToString(),
                        DateNaissance = dr.GetDateTime(3)
                    });

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return result;
        }
    }
}
