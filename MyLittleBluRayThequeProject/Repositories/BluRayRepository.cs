using MyLittleBluRayThequeProject.DTOs;
using Npgsql;

namespace MyLittleBluRayThequeProject.Repositories
{
    public class BluRayRepository
    {
        public string ConnectionString { get; set; }

        /// <summary>
        /// Consctructeur par défaut
        /// </summary>
        public BluRayRepository()
        {
            this.ConnectionString = "";
        }

        public List<BluRay> GetListeBluRay()
        {
            List<BluRay> allBluRays = new List<BluRay>();
            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(PersonneRepository.ConnectionString);
                conn.Open();

                string allBluRaysRequest = "SELECT * FROM \"BluRayTheque\".\"BluRay\";";
                NpgsqlCommand command = new NpgsqlCommand(allBluRaysRequest, conn);
                NpgsqlDataReader dr = command.ExecuteReader();

                while(dr.Read())
                {

                    BluRay bluRay = new BluRay();
                    bluRay.Id = long.Parse(dr[0].ToString());
                    bluRay.Acteurs = PersonneRepository.GetActeurs(bluRay.Id);
                    bluRay.DateSortie = DateTime.Parse(dr[3].ToString());
                    bluRay.Scenariste = PersonneRepository.GetScenariste(bluRay.Id);
                    bluRay.Realisateur = PersonneRepository.GetRealisateur(bluRay.Id);
                    bluRay.Titre = dr[1].ToString();
                    bluRay.Duree = TimeSpan.FromMinutes(long.Parse(dr[2].ToString()));
                    bluRay.Version = dr[4].ToString();
                    allBluRays.Add(bluRay);

                }
            }
            finally
            {
                if(conn != null)
                {
                    conn.Close();
                }
            }

            return allBluRays;
        }
        

        /// <summary>
        /// Récupération d'un BR par son Id
        /// </summary>
        /// <param name="Id">l'Id du bluRay</param>
        /// <returns></returns>
        public BluRay GetBluRay(long Id)
        {
            NpgsqlConnection conn = null;
            BluRay result = new BluRay();
            try
            {
                List<BluRay> qryResult = new List<BluRay>();
                // Connect to a PostgreSQL database
                conn = new NpgsqlConnection("Server=127.0.0.1;User Id=postgres;Password=root;Database=postgres;");
                conn.Open();

                // Define a query returning a single row result set
                NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM \"BluRayTheque\".\"BluRay\" where \"Id\" = @p", conn);
                command.Parameters.AddWithValue("p", Id);

                // Execute the query and obtain a result set
                NpgsqlDataReader dr = command.ExecuteReader();

                // Output rows
                while (dr.Read())
                {
                    BluRay bluRay = new BluRay();
                    bluRay.Id = long.Parse(dr[0].ToString());
                    bluRay.DateSortie = DateTime.Parse(dr[3].ToString());
                    bluRay.Titre = dr[1].ToString();
                    bluRay.Duree = TimeSpan.FromMinutes(long.Parse(dr[2].ToString()));
                    bluRay.Version = dr[4].ToString();
                    qryResult.Add(bluRay);
                }

                result = qryResult.SingleOrDefault();
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
        public void enregistrerBluRay(BluRay bluRay)
        {
            NpgsqlConnection conn = null;
            try
            {
                conn = new NpgsqlConnection("Server=127.0.0.1;User Id=postgres;Password=root;Database=postgres;");
                conn.Open();

                NpgsqlCommand sendNewUserCommand = new NpgsqlCommand("INSERT INTO \"BluRayTheque\".\"BluRay\" (\"Id\",\"Titre\",\"DateSortie\",\"Version\",\"Duree\", \"Disponible\") " +
                    "VALUES (@id, @titre, @dateSortie, @version, @duree, True);", conn);
                sendNewUserCommand.Parameters.AddWithValue("id", bluRay.Id);
                sendNewUserCommand.Parameters.AddWithValue("titre", bluRay.Titre);
                sendNewUserCommand.Parameters.AddWithValue("dateSortie", bluRay.DateSortie);
                sendNewUserCommand.Parameters.AddWithValue("version", bluRay.Version);
                sendNewUserCommand.Parameters.AddWithValue("duree", (int) bluRay.Duree.TotalMinutes);
                sendNewUserCommand.ExecuteNonQuery();
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}