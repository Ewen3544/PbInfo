using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class ConnexionBDD : IDisposable
{
    private MySqlConnection maConnexion;
    public string RoleUtilisateur { get; private set; }
    public int IdUtilisateur { get; private set; }

    public int IdClient { get; private set; }

    #region Constructeurs
    public ConnexionBDD(string email, string mdp) ///constructeur connexion
    {
        string chaineConnexion = "SERVER=localhost;PORT=3306;DATABASE=livinparis;UID=supercaniveau;PASSWORD=1234;";
        maConnexion = new MySqlConnection(chaineConnexion);
        maConnexion.Open();

        VerifierIdentifiants(email, mdp);
        DeterminerRole();

        if (RoleUtilisateur == "Client")
        {
            IdClient = GetIdClient(IdUtilisateur);
        }
    }

  
    public ConnexionBDD(bool pourInscription = true) ///constructeur inscription (pas de verif email mdp)
    {
        string chaineConnexion = "SERVER=localhost;PORT=3306;DATABASE=livinparis;UID=supercaniveau;PASSWORD=1234;";
        maConnexion = new MySqlConnection(chaineConnexion);
        maConnexion.Open();

        /// Pas de vérification d'identifiants ni de rôle ici !
    }
    #endregion

    #region methodes importantes (GetIdClient, VerifIdentifiant, DeterminerRole)
    public int GetIdClient(int idUtilisateur)
    {
        string requete = "SELECT id_Client FROM Client WHERE id_Utilisateur = @id"; ///requete sql qui selectionne l'id client a partir de id utilisateur
        using (MySqlCommand command = new MySqlCommand(requete, maConnexion)) ///creation commande sql connectée a la bdd
        {
            command.Parameters.AddWithValue("@id", idUtilisateur); ///ajoute le parametre a id utilisateur
            return Convert.ToInt32(command.ExecuteScalar()); ///execute la requete et convertit en entier integer
        }
    }
    private void VerifierIdentifiants(string email, string mdp)
    {
        string requete = "SELECT id_Utilisateur, MotDePasse FROM Utilisateur WHERE Email = @email"; ///requete sql qui select id utilisateur et son mdp basé sur l'email
        MySqlCommand Command = new MySqlCommand(requete, maConnexion); ///creation commande sql connexion bdd
        Command.Parameters.AddWithValue("@email", email); /// ajout parametre email

        using (MySqlDataReader lecteur = Command.ExecuteReader()) ///recupere les resultats et les insère dans le lecteru Reader
        {
            if (!lecteur.Read())///si aucun utilisatuer n'est trouvé alors
            {
                Console.WriteLine("Nous n'avons pas réussi a trouver ce compte");
                return;
            }
            ///sinon
            string mdpStocke = lecteur["MotDePasse"].ToString();
            if (mdpStocke != mdp) ///verifie si le mdp est correct
            {
                Console.WriteLine("mot de passe incorret");
                return;
            }
                

            IdUtilisateur = Convert.ToInt32(lecteur["id_Utilisateur"]);
        }
    }

    private void DeterminerRole() 
    {
        string reqClient = "SELECT COUNT(*) FROM Client WHERE id_Utilisateur = @id"; ///verif si l'utilisateur est dans la table client
        MySqlCommand CommandClient = new MySqlCommand(reqClient, maConnexion);
        CommandClient.Parameters.AddWithValue("@id", IdUtilisateur);
        int countClient = Convert.ToInt32(CommandClient.ExecuteScalar());
        bool estClient = countClient > 0;

        string reqCuisinier = "SELECT COUNT(*) FROM Cuisinier WHERE id_Utilisateur = @id"; ///pareil mais pour cuisinier
        MySqlCommand CommandCuisinier = new MySqlCommand(reqCuisinier, maConnexion);
        CommandCuisinier.Parameters.AddWithValue("@id", IdUtilisateur);
        int countCuisinier = Convert.ToInt32(CommandCuisinier.ExecuteScalar());
        bool estCuisinier = countCuisinier > 0;

        
        if (estClient)
        {
            RoleUtilisateur = "Client";
        }
        else if (estCuisinier)
        {
            RoleUtilisateur = "Cuisinier";
        }
        else
        {
            Console.WriteLine("Tu peux qu'être un cuisto ou client mon ami..");
            return;
        }
    }

    #endregion

    #region requètes
    public List<Dictionary<string, object>> ExecuterRequete(string sql, Dictionary<string, object> parametres = null) ///execute la requete sql select et retourne les resultats sous forme de liste de dictionnaire
    {
        List<Dictionary<string, object>> resultats = new List<Dictionary<string, object>>(); ///liste qui contient les resultats
        MySqlCommand Command = new MySqlCommand(sql, maConnexion);

        if (parametres != null) ///si des parametres sont fournis alors on les ajoute
        {
            List<string> keys = new List<string>(parametres.Keys); ///recupère toutes les clefs
            int i = 0;
            while (i < keys.Count) 
            {
                string key = keys[i];
                Command.Parameters.AddWithValue(key, parametres[key]);
                i++;
            }
        }
        ///execute la requète
        MySqlDataReader lecteur = Command.ExecuteReader();

        while (lecteur.Read()) ///lit ligne par ligne les résultats
        {
            Dictionary<string, object> ligne = new Dictionary<string, object>();
            int j = 0;
            while (j < lecteur.FieldCount) ///parcours toutes les colonnes et associe le nom de la colonne a sa valeur respective
            {
                ligne.Add(lecteur.GetName(j), lecteur.GetValue(j));
                j++;
            }
            resultats.Add(ligne); 
        }

        lecteur.Close(); 
        Command.Dispose(); 
        return resultats;
    }


    public int ExecuterInsertEtRetournerId(string sql, Dictionary<string, object> parametres = null) ///execute une requete insert et retourne le dernier id inséré
    {
        ///recup l'id généré après l'insert dans sql
        MySqlCommand Command = new MySqlCommand(sql + "; SELECT LAST_INSERT_ID();", maConnexion);

        if (parametres != null)
        {
            List<string> keys = new List<string>(parametres.Keys);
            int i = 0;
            while (i < keys.Count)
            {
                string key = keys[i];
                Command.Parameters.AddWithValue(key, parametres[key]);
                i++;
            }
        }

        object resultat = Command.ExecuteScalar(); ///recuperation ID
        int id = Convert.ToInt32(resultat); ///converti en entier integer
        Command.Dispose(); 
        return id;
    }


    public void ExecuterNonQuery(string sql, Dictionary<string, object> parametres = null)
    {
        MySqlCommand Command = new MySqlCommand(sql, maConnexion);

        if (parametres != null)
        {
            List<string> keys = new List<string>(parametres.Keys);
            int i = 0;
            while (i < keys.Count)
            {
                string key = keys[i];
                Command.Parameters.AddWithValue(key, parametres[key]);
                i++;
            }
        }

        Command.ExecuteNonQuery();
        Command.Dispose(); 
    }

    #endregion

    #region methodes complementaires
    public void Fermer() //methode fermer la connexion
    {
        if (maConnexion != null && maConnexion.State != System.Data.ConnectionState.Closed)
        {
            maConnexion.Close();
        }
    }
    public void Dispose()
    {
        Fermer();
    }
    #endregion
}