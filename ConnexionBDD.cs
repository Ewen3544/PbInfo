using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class ConnexionBDD : IDisposable
{
    private MySqlConnection maConnexion;
    public string RoleUtilisateur { get; private set; }
    public int IdUtilisateur { get; private set; }

    public int IdClient { get; private set; }

    public ConnexionBDD(string email, string mdp)
    {
        string chaineConnexion = "SERVER=localhost;PORT=3306;DATABASE=projet;UID=root;PASSWORD=root;";
        maConnexion = new MySqlConnection(chaineConnexion);
        maConnexion.Open();

        VerifierIdentifiants(email, mdp);
        DeterminerRole();

        if (RoleUtilisateur == "Client")
        {
            IdClient = GetIdClient(IdUtilisateur);
        }
    }

    public int GetIdClient(int idUtilisateur)
    {
        string requete = "SELECT id_Client FROM Client WHERE id_Utilisateur = @id";
        using (MySqlCommand command = new MySqlCommand(requete, maConnexion))
        {
            command.Parameters.AddWithValue("@id", idUtilisateur);
            return Convert.ToInt32(command.ExecuteScalar());
        }
    }
    private void VerifierIdentifiants(string email, string mdp)
    {
        string requete = "SELECT id_Utilisateur, MotDePasse FROM Utilisateur WHERE Email = @email";
        MySqlCommand Command = new MySqlCommand(requete, maConnexion);
        Command.Parameters.AddWithValue("@email", email);

        using (MySqlDataReader lecteur = Command.ExecuteReader())
        {
            if (!lecteur.Read())
                throw new Exception("Email incorrect ou compte inexistant");

            string mdpStocke = lecteur["MotDePasse"].ToString();
            if (mdpStocke != mdp)
                throw new Exception("Mot de passe incorrect");

            IdUtilisateur = Convert.ToInt32(lecteur["id_Utilisateur"]);
        }
    }

    private void DeterminerRole() //DETERMINE SI CUISINIER OU CLIENT PAR REQUETE SQL
    {
        string reqClient = "SELECT COUNT(*) FROM Client WHERE id_Utilisateur = @id"; //compte le nombre d'utilisateur qui a le meme id que la personne connectée (si >0 alors client)
        MySqlCommand CommandClient = new MySqlCommand(reqClient, maConnexion);
        CommandClient.Parameters.AddWithValue("@id", IdUtilisateur);
        bool estClient = Convert.ToInt32(CommandClient.ExecuteScalar()) > 0;

        string reqCuisinier = "SELECT COUNT(*) FROM Cuisinier WHERE id_Utilisateur = @id"; //meme logique mais pour le cuisinier dans la table cuisinier dcp
        MySqlCommand CommandCuisinier = new MySqlCommand(reqCuisinier, maConnexion);
        CommandCuisinier.Parameters.AddWithValue("@id", IdUtilisateur);
        bool estCuisinier = Convert.ToInt32(CommandCuisinier.ExecuteScalar()) > 0;

        RoleUtilisateur = estClient ? "Client" : estCuisinier ? "Cuisinier" : throw new Exception("Problème : vous êtes ni un client ni un cuisinier"); //si aucun des deux alors renvoyer un bug
    }

    public List<Dictionary<string, object>> ExecuterRequete(string sql, Dictionary<string, object> parametres = null)
    {
        List<Dictionary<string, object>> resultats = new List<Dictionary<string, object>>();

        using (MySqlCommand Command = new MySqlCommand(sql, maConnexion))
        {
            if (parametres != null)
            {
                foreach (KeyValuePair<string, object> param in parametres)
                {
                    Command.Parameters.AddWithValue(param.Key, param.Value);
                }
            }

            using (MySqlDataReader lecteur = Command.ExecuteReader())
            {
                while (lecteur.Read())
                {
                    Dictionary<string, object> ligne = new Dictionary<string, object>();
                    for (int i = 0; i < lecteur.FieldCount; i++)
                    {
                        ligne.Add(lecteur.GetName(i), lecteur.GetValue(i));
                    }
                    resultats.Add(ligne);
                }
            }
        }
        return resultats;
    }

    public int ExecuterInsertEtRetournerId(string sql, Dictionary<string, object> parametres = null)
    {
        using (MySqlCommand Command = new MySqlCommand(sql + "; SELECT LAST_INSERT_ID();", maConnexion))
        {
            if (parametres != null)
            {
                foreach (KeyValuePair<string, object> param in parametres)
                {
                    Command.Parameters.AddWithValue(param.Key, param.Value);
                }
            }
            return Convert.ToInt32(Command.ExecuteScalar());
        }
    }

    public void ExecuterNonQuery(string sql, Dictionary<string, object> parametres = null)
    {
        using (MySqlCommand Command = new MySqlCommand(sql, maConnexion))
        {
            if (parametres != null)
            {
                foreach (KeyValuePair<string, object> param in parametres)
                {
                    Command.Parameters.AddWithValue(param.Key, param.Value);
                }
            }
            Command.ExecuteNonQuery();
        }
    }

    public void Fermer()
    {
        if (maConnexion?.State != System.Data.ConnectionState.Closed)
        {
            maConnexion?.Close();
        }
    }

    public void Dispose()
    {
        Fermer();
    }
}