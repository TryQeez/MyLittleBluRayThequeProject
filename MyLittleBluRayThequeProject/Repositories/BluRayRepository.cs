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
            NpgsqlConnection conn = null;
            List<BluRay> result = new List<BluRay>();
            try
            {
                // Connect to a PostgreSQL database
                conn = new NpgsqlConnection("Server=127.0.0.1;User Id=postgres;Password=root;Database=postgres;");
                conn.Open();

                // Define a query returning a single row result set
                NpgsqlCommand command = new NpgsqlCommand("SELECT \"Id\", \"Titre\", \"Duree\", \"Version\" FROM \"BluRayTheque\".\"BluRay\"", conn);
                
                // Execute the query and obtain a result set
                NpgsqlDataReader dr = command.ExecuteReader();
                BluRay bluRay = new BluRay();
                // Output rows
                while (dr.Read())
                {

                    bluRay.Id = long.Parse(dr[0].ToString());
                    bluRay.Titre = dr[1].ToString();
                    bluRay.Duree = TimeSpan.FromSeconds(long.Parse(dr[2].ToString()));
                    bluRay.Version = dr[3].ToString();

                    command.Cancel();
                    break;
                }
                    Personne scenariste = new Personne();
                    Personne realisateur = new Personne();
                    List<Personne> acteurs = new List<Personne>();

                    string scenaristeRequest = "SELECT * FROM \"BluRayTheque\".\"Personne\", \"BluRayTheque\".\"BluRay\",\"BluRayTheque\".\"Scenariste\"  where \"BluRayTheque\".\"BluRay\".\"Id\"=" + bluRay.Id + " and \"BluRayTheque\".\"Scenariste\".\"IdBluRay\"=" + bluRay.Id + " and \"BluRayTheque\".\"Scenariste\".\"IdScenariste\" = \"BluRayTheque\".\"Personne\".\"Id\"";
                    string acteursRequest = "SELECT * FROM \"BluRayTheque\".\"Personne\", \"BluRayTheque\".\"BluRay\",\"BluRayTheque\".\"Acteur\"  where \"BluRayTheque\".\"BluRay\".\"Id\"=" + bluRay.Id + " and \"BluRayTheque\".\"Acteur\".\"IdBluRay\"=" + bluRay.Id + " and \"BluRayTheque\".\"Acteur\".\"IdActeur\" = \"BluRayTheque\".\"Personne\".\"Id\"";
                    string realisateurRequest = "SELECT * FROM \"BluRayTheque\".\"Personne\", \"BluRayTheque\".\"BluRay\",\"BluRayTheque\".\"Realisateyr\"  where \"BluRayTheque\".\"BluRay\".\"Id\"=" + bluRay.Id + " and \"BluRayTheque\".\"Realisateur\".\"IdBluRay\"=" + bluRay.Id + " and \"BluRayTheque\".\"Realisateur\".\"IdRealisateur\" = \"BluRayTheque\".\"Personne\".\"Id\"";

                    NpgsqlCommand getScenaristeCommand = new NpgsqlCommand(scenaristeRequest, conn);
                    NpgsqlCommand getActeursCommand = new NpgsqlCommand(acteursRequest, conn);
                    NpgsqlCommand getRealisateurCommand = new NpgsqlCommand(realisateurRequest, conn);


                    NpgsqlDataReader scanaristeDataReader = getScenaristeCommand.ExecuteReader();
                    NpgsqlDataReader acteursDataReader = getActeursCommand.ExecuteReader();
                    NpgsqlDataReader realisateurDataReader = getRealisateurCommand.ExecuteReader();

                    while (scanaristeDataReader.Read())
                    {
                        scenariste.Id = long.Parse(scanaristeDataReader[0].ToString());
                        scenariste.Nom = scanaristeDataReader[1].ToString();
                        scenariste.Prenom = scanaristeDataReader[2].ToString();
                        scenariste.Nationalite = scanaristeDataReader[4].ToString();
                        getScenaristeCommand.Cancel();
                        break;
                    }

                    bluRay.Scenariste = scenariste;

                    while(acteursDataReader.Read())
                    {
                        Personne acteur = new Personne();
                        acteur.Id = long.Parse(acteursDataReader[0].ToString());
                        acteur.Nom = acteursDataReader[1].ToString();
                        acteur.Prenom = acteursDataReader[2].ToString();
                        acteur.Nationalite = scanaristeDataReader[4].ToString();
                        getActeursCommand.Cancel();
                        acteurs.Add(acteur);
                    }

                    bluRay.Acteurs = acteurs;

                    while (realisateurDataReader.Read())
                    {
                        Personne realisateurParsed = new Personne();
                        realisateurParsed.Id = long.Parse(realisateurDataReader[0].ToString());
                        realisateurParsed.Nom = realisateurDataReader[1].ToString();
                        realisateurParsed.Prenom = realisateurDataReader[2].ToString();
                        realisateurParsed.Nationalite = realisateurDataReader[4].ToString();

                        realisateur = realisateurParsed;
                        getRealisateurCommand.Cancel(); 
                        break;
                    }

                    bluRay.Realisateur = realisateur;


                    result.Add(bluRay);

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
                NpgsqlCommand command = new NpgsqlCommand("SELECT \"Id\", \"Titre\", \"Duree\", \"Version\" FROM \"BluRayTheque\".\"BluRay\" where \"Id\" = @p", conn);
                command.Parameters.AddWithValue("p", Id);

                // Execute the query and obtain a result set
                NpgsqlDataReader dr = command.ExecuteReader();

                // Output rows
                while (dr.Read())
                    qryResult.Add(new BluRay
                    {
                        Id = long.Parse(dr[0].ToString()),
                        Titre = dr[1].ToString(),
                        Duree = TimeSpan.FromSeconds(long.Parse(dr[2].ToString())),
                        Version = dr[3].ToString()
                    });

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
    }
}
