using MyLittleBluRayThequeProject.DTOs;
using Npgsql;
namespace MyLittleBluRayThequeProject.Repositories
{
    public class PersonneRepository
    {
        public PersonneRepository() { }

        public static string ConnectionString = "Server=127.0.0.1;User Id=postgres;Password=root;Database=postgres;";

        public List<Personne> GetListePersonne()
        {
            NpgsqlConnection conn = null;
            List<Personne> result = new List<Personne>();
            try
            {
                // Connect to a PostgreSQL database
                conn = new NpgsqlConnection(PersonneRepository.ConnectionString);
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

        public static List<Personne> GetActeurs(long idBr)
        {
            
            List<Personne> acteurs = new List<Personne>();
            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(PersonneRepository.ConnectionString);
                conn.Open();

                string acteursRequest = "SELECT * FROM \"BluRayTheque\".\"Personne\", \"BluRayTheque\".\"BluRay\",\"BluRayTheque\".\"Acteur\"  where \"BluRayTheque\".\"BluRay\".\"Id\"=" + idBr + " and \"BluRayTheque\".\"Acteur\".\"IdBluRay\"=" + idBr + " and \"BluRayTheque\".\"Acteur\".\"IdActeur\" = \"BluRayTheque\".\"Personne\".\"Id\"";

                NpgsqlCommand command = new NpgsqlCommand(acteursRequest, conn);
                NpgsqlDataReader dr = command.ExecuteReader();

                while(dr.Read())
                {
                    Personne acteur = new Personne();
                    acteur.Id = long.Parse(dr[0].ToString());
                    acteur.Nom = dr[1].ToString();
                    acteur.Prenom = dr[2].ToString();
                    acteur.Nationalite = dr[4].ToString();
                    acteurs.Add(acteur);
                }
            } finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return acteurs;
        }

        public static Personne GetRealisateur(long idBr)
        {

            Personne realisateurReturned = new Personne();
            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(PersonneRepository.ConnectionString);
                conn.Open();

                string realisateurRequest = "SELECT * FROM \"BluRayTheque\".\"Personne\", \"BluRayTheque\".\"BluRay\",\"BluRayTheque\".\"Realisateur\"  where \"BluRayTheque\".\"BluRay\".\"Id\"=" + idBr + " and \"BluRayTheque\".\"Realisateur\".\"IdBluRay\"=" + idBr + " and \"BluRayTheque\".\"Realisateur\".\"IdRealisateur\" = \"BluRayTheque\".\"Personne\".\"Id\"";

                NpgsqlCommand command = new NpgsqlCommand(realisateurRequest, conn);
                NpgsqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    Personne realisateur = new Personne();
                    realisateur.Id = long.Parse(dr[0].ToString());
                    realisateur.Nom = dr[1].ToString();
                    realisateur.Prenom = dr[2].ToString();
                    realisateur.Nationalite = dr[4].ToString();
                    realisateurReturned = realisateur;
                    break;
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return realisateurReturned;
        }

        public static Personne GetScenariste(long idBr)
        {

            Personne scenaristeReturned = new Personne();
            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(PersonneRepository.ConnectionString);
                conn.Open();

                string scenaristeRequest = "SELECT * FROM \"BluRayTheque\".\"Personne\", \"BluRayTheque\".\"BluRay\",\"BluRayTheque\".\"Scenariste\"  where \"BluRayTheque\".\"BluRay\".\"Id\"=" + idBr + " and \"BluRayTheque\".\"Scenariste\".\"IdBluRay\"=" + idBr + " and \"BluRayTheque\".\"Scenariste\".\"IdScenariste\" = \"BluRayTheque\".\"Personne\".\"Id\"";

                NpgsqlCommand command = new NpgsqlCommand(scenaristeRequest, conn);
                NpgsqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    Personne scenariste = new Personne();
                    scenariste.Id = long.Parse(dr[0].ToString());
                    scenariste.Nom = dr[1].ToString();
                    scenariste.Prenom = dr[2].ToString();
                    scenariste.Nationalite = dr[4].ToString();
                    scenaristeReturned = scenariste;
                    break;
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return scenaristeReturned;
        }


        public void enregistrerPersonne(Personne personne)
        {
            NpgsqlConnection conn = null;
            try { 
                conn = new NpgsqlConnection("Server=127.0.0.1;User Id=postgres;Password=root;Database=postgres;");
                conn.Open();

                NpgsqlCommand sendNewUserCommand = new NpgsqlCommand("INSERT INTO \"BluRayTheque\".\"Personne\" (\"Nom\",\"Prenom\",\"Nationalite\",\"DateNaissance\") " +
                    "VALUES (@nom, @prenom, @nationalite, @dateNaissance);", 
                    conn);
                sendNewUserCommand.Parameters.AddWithValue("nom", personne.Nom);
                sendNewUserCommand.Parameters.AddWithValue("prenom", personne.Prenom);
                sendNewUserCommand.Parameters.AddWithValue("nationalite", personne.Nationalite);
                sendNewUserCommand.Parameters.AddWithValue("dateNaissance", personne.DateNaissance);
               
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
