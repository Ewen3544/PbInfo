using MySql.Data.MySqlClient;

public class Connexion : IDisposable
{
    private readonly MySqlConnection macConnexion;
    public string Role { get; private set; }
    public int UserId { get; private set; }
    public bool IsClient => Role == "Client";
    public bool IsCuisinier => Role == "Cuisinier";

    public Connexion(string email, string password)
    {
        string connexionString = "SERVER=localhost;PORT=3306;" +
                                "DATABASE=projet;" +
                                "UID=root;PASSWORD=root;";
        macConnexion = new MySqlConnection(connexionString);
        macConnexion.Open();

        AuthentifierUtilisateur(email, password);
        DeterminerRole();
    }

    private void AuthentifierUtilisateur(string email, string password)
    {
        string query = "SELECT id_Utilisateur, MotDePasse FROM Utilisateur WHERE Email = @email";
        MySqlCommand command = new MySqlCommand(query, macConnexion);
        command.Parameters.AddWithValue("@email", email);

        using (MySqlDataReader reader = command.ExecuteReader())
        {
            if (!reader.Read())
            {
                throw new Exception("Utilisateur non trouvé.");
            }

            string storedPassword = reader["MotDePasse"].ToString();
            if (storedPassword != password)
            {
                throw new Exception("Mot de passe incorrect.");
            }

            UserId = Convert.ToInt32(reader["id_Utilisateur"]);
        }
    }

    private void DeterminerRole()
    {
        // Vérifier si c'est un client
        string queryClient = "SELECT COUNT(*) FROM Client WHERE id_Utilisateur = @id";
        MySqlCommand commandClient = new MySqlCommand(queryClient, macConnexion);
        commandClient.Parameters.AddWithValue("@id", UserId);
        int isClient = Convert.ToInt32(commandClient.ExecuteScalar());

        // Vérifier si c'est un cuisinier
        string queryCuisinier = "SELECT COUNT(*) FROM Cuisinier WHERE id_Utilisateur = @id";
        MySqlCommand commandCuisinier = new MySqlCommand(queryCuisinier, macConnexion);
        commandCuisinier.Parameters.AddWithValue("@id", UserId);
        int isCuisinier = Convert.ToInt32(commandCuisinier.ExecuteScalar());

        Role = isClient > 0 ? "Client" : (isCuisinier > 0 ? "Cuisinier" : throw new Exception("Rôle non défini."));
    }

    // Méthodes pour les clients
    public List<Dictionary<string, object>> GetCommandesClient()
    {
        if (!IsClient) throw new UnauthorizedAccessException("Accès réservé aux clients");

        string query = @"SELECT c.*, p.Nom as PlatNom 
                        FROM Commande c
                        JOIN Contient ct ON c.id_Commande = ct.id_Commande
                        JOIN Plat p ON ct.id_Plat = p.id_Plat
                        WHERE c.id_Client = @idClient";

        MySqlCommand command = new MySqlCommand(query, macConnexion);
        command.Parameters.AddWithValue("@idClient", UserId);

        return ExecuteReaderToDictionaryList(command);
    }

    // Méthodes pour les cuisiniers
    public List<Dictionary<string, object>> GetPlatsCuisinier()
    {
        if (!IsCuisinier) throw new UnauthorizedAccessException("Accès réservé aux cuisiniers");

        string query = @"SELECT p.*, COUNT(c.id_Commande) as CommandesCount
                        FROM Plat p
                        LEFT JOIN Contient ct ON p.id_Plat = ct.id_Plat
                        LEFT JOIN Commande c ON ct.id_Commande = c.id_Commande
                        WHERE p.id_Cuisinier = @idCuisinier
                        GROUP BY p.id_Plat";

        MySqlCommand command = new MySqlCommand(query, macConnexion);
        command.Parameters.AddWithValue("@idCuisinier", UserId);

        return ExecuteReaderToDictionaryList(command);
    }

    public List<Dictionary<string, object>> GetClientsCuisinier()
    {
        if (!IsCuisinier) throw new UnauthorizedAccessException("Accès réservé aux cuisiniers");

        string query = @"SELECT DISTINCT u.* 
                        FROM Utilisateur u
                        JOIN Client cl ON u.id_Utilisateur = cl.id_Utilisateur
                        JOIN Commande c ON cl.id_Client = c.id_Client
                        JOIN Contient ct ON c.id_Commande = ct.id_Commande
                        JOIN Plat p ON ct.id_Plat = p.id_Plat
                        WHERE p.id_Cuisinier = @idCuisinier";

        MySqlCommand command = new MySqlCommand(query, macConnexion);
        command.Parameters.AddWithValue("@idCuisinier", UserId);

        return ExecuteReaderToDictionaryList(command);
    }

    private List<Dictionary<string, object>> ExecuteReaderToDictionaryList(MySqlCommand command)
    {
        List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

        using (MySqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }
                results.Add(row);
            }
        }

        return results;
    }

    public void Dispose()
    {
        macConnexion?.Close();
        macConnexion?.Dispose();
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("===== Liv'In Paris =====");
        Console.WriteLine("1. Se connecter");
        Console.WriteLine("2. Créer un compte");
        Console.Write("Choix : ");

        string choice = Console.ReadLine();

        if (choice == "1")
        {
            ConnecterUtilisateur();
        }
        else if (choice == "2")
        {
            CreerCompte();
        }
        else
        {
            Console.WriteLine("Option invalide");
        }
    }

    static void ConnecterUtilisateur()
    {
        Console.Write("Email : ");
        string email = Console.ReadLine();
        Console.Write("Mot de passe : ");
        string password = Console.ReadLine();

        try
        {
            using (Connexion conn = new Connexion(email, password))
            {
                Console.WriteLine($"\nConnecté en tant que {conn.Role} !");

                if (conn.IsClient)
                {
                    AfficherMenuClient(conn);
                }
                else if (conn.IsCuisinier)
                {
                    AfficherMenuCuisinier(conn);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
        }
    }

    static void AfficherMenuClient(Connexion conn)
    {
        while (true)
        {
            Console.WriteLine("\n=== MENU CLIENT ===");
            Console.WriteLine("1. Voir mes commandes");
            Console.WriteLine("2. Voir le statut de mes livraisons");
            Console.WriteLine("3. Quitter");
            Console.Write("Choix : ");

            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    AfficherCommandesClient(conn);
                    break;
                case "2":
                    AfficherStatutsLivraison(conn);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Option invalide");
                    break;
            }
        }
    }

    static void AfficherCommandesClient(Connexion conn)
    {
        try
        {
            List<Dictionary<string, object>> commandes = conn.GetCommandesClient();

            Console.WriteLine("\n=== MES COMMANDES ===");
            foreach (Dictionary<string, object> commande in commandes)
            {
                Console.WriteLine($"\nCommande #{commande["id_Commande"]}");
                Console.WriteLine($"- Plat: {commande["PlatNom"]}");
                Console.WriteLine($"- Date: {commande["Date_Commande"]}");
                Console.WriteLine($"- Prix: {commande["Prix_Commande"]}€");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur: {ex.Message}");
        }
    }

    static void AfficherStatutsLivraison(Connexion conn)
    {
        try
        {
            List<Dictionary<string, object>> commandes = conn.GetCommandesClient();

            Console.WriteLine("\n=== STATUTS DE LIVRAISON ===");
            foreach (Dictionary<string, object> commande in commandes)
            {
                Console.WriteLine($"\nCommande #{commande["id_Commande"]}");
                Console.WriteLine($"- Statut: {commande["Statut_Livraison"]}");
                Console.WriteLine($"- Date prévue: {commande["Date_Livraison"]}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur: {ex.Message}");
        }
    }

    static void AfficherMenuCuisinier(Connexion conn)
    {
        while (true)
        {
            Console.WriteLine("\n=== MENU CUISINIER ===");
            Console.WriteLine("1. Voir mes plats");
            Console.WriteLine("2. Voir mes clients");
            Console.WriteLine("3. Quitter");
            Console.Write("Choix : ");

            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    AfficherPlatsCuisinier(conn);
                    break;
                case "2":
                    AfficherClientsCuisinier(conn);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Option invalide");
                    break;
            }
        }
    }

    static void AfficherPlatsCuisinier(Connexion conn)
    {
        try
        {
            List<Dictionary<string, object>> plats = conn.GetPlatsCuisinier();

            Console.WriteLine("\n=== MES PLATS ===");
            foreach (Dictionary<string, object> plat in plats)
            {
                Console.WriteLine($"\nPlat #{plat["id_Plat"]}");
                Console.WriteLine($"- Nom: {plat["Nom"]}");
                Console.WriteLine($"- Prix: {plat["Prix"]}€");
                Console.WriteLine($"- Commandes: {plat["CommandesCount"]}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur: {ex.Message}");
        }
    }

    static void AfficherClientsCuisinier(Connexion conn)
    {
        try
        {
            List<Dictionary<string, object>> clients = conn.GetClientsCuisinier();

            Console.WriteLine("\n=== MES CLIENTS ===");
            foreach (Dictionary<string, object> client in clients)
            {
                Console.WriteLine($"\nClient #{client["id_Utilisateur"]}");
                Console.WriteLine($"- Nom: {client["Nom"]} {client["Prénom"]}");
                Console.WriteLine($"- Email: {client["Email"]}");
                Console.WriteLine($"- Téléphone: {client["Téléphone"]}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur: {ex.Message}");
        }
    }

    static void CreerCompte()
    {
        Console.WriteLine("\n=== CREATION DE COMPTE ===");
        Console.WriteLine("1. Créer un compte Client");
        Console.WriteLine("2. Créer un compte Cuisinier");
        Console.Write("Votre choix : ");

        string choix = Console.ReadLine();

        switch (choix)
        {
            case "1":
                CreerCompteClient();
                break;
            case "2":
                CreerCompteCuisinier();
                break;
            default:
                Console.WriteLine("Option invalide");
                break;
        }
    }

    static void CreerCompteClient()
    {
        Console.WriteLine("\n--- Nouveau Client ---");

        // Collecte des informations
        Console.Write("Nom : ");
        string nom = Console.ReadLine();

        Console.Write("Prénom : ");
        string prenom = Console.ReadLine();

        Console.Write("Email : ");
        string email = Console.ReadLine();

        Console.Write("Mot de passe : ");
        string motDePasse = Console.ReadLine();

        Console.Write("Téléphone : ");
        string telephone = Console.ReadLine();

        Console.Write("Rue : ");
        string rue = Console.ReadLine();

        Console.Write("Numéro de rue : ");
        int numeroAdresse;
        while (!int.TryParse(Console.ReadLine(), out numeroAdresse))
        {
            Console.WriteLine("Veuillez entrer un numéro valide : ");
        }

        Console.Write("Code postal : ");
        string codePostal = Console.ReadLine();

        Console.Write("Ville : ");
        string ville = Console.ReadLine();

        Console.Write("Métro le plus proche : ");
        string metro = Console.ReadLine();

        Console.Write("Régime alimentaire : ");
        string regime = Console.ReadLine();

        try
        {
            using (MySqlConnection macConnexion = new MySqlConnection("SERVER=localhost;PORT=3306;DATABASE=projet;UID=root;PASSWORD=root;"))
            {
                macConnexion.Open();
                using (MySqlTransaction transaction = macConnexion.BeginTransaction())
                {
                    try
                    {
                        // Trouver le prochain id_Utilisateur disponible
                        int newUserId = 1;
                        string queryMaxId = "SELECT MAX(id_Utilisateur) FROM Utilisateur";
                        MySqlCommand commandMaxId = new MySqlCommand(queryMaxId, macConnexion, transaction);
                        object result = commandMaxId.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            newUserId = Convert.ToInt32(result) + 1;
                        }

                        // Insertion dans Utilisateur
                        string queryUser = @"INSERT INTO Utilisateur 
                                        (id_Utilisateur, Nom, Prénom, Téléphone, Rue, Numéro_Adresse, 
                                         Code_Postal, Ville, Email, MotDePasse, Métro_Le_Plus_Proche, Role) 
                                        VALUES (@id, @nom, @prenom, @telephone, @rue, @numero, 
                                                @codePostal, @ville, @email, @motDePasse, @metro, 'Client')";

                        MySqlCommand commandUser = new MySqlCommand(queryUser, macConnexion, transaction);
                        commandUser.Parameters.AddWithValue("@id", newUserId);
                        commandUser.Parameters.AddWithValue("@nom", nom);
                        commandUser.Parameters.AddWithValue("@prenom", prenom);
                        commandUser.Parameters.AddWithValue("@telephone", telephone);
                        commandUser.Parameters.AddWithValue("@rue", rue);
                        commandUser.Parameters.AddWithValue("@numero", numeroAdresse);
                        commandUser.Parameters.AddWithValue("@codePostal", codePostal);
                        commandUser.Parameters.AddWithValue("@ville", ville);
                        commandUser.Parameters.AddWithValue("@email", email);
                        commandUser.Parameters.AddWithValue("@motDePasse", motDePasse);
                        commandUser.Parameters.AddWithValue("@metro", metro);

                        commandUser.ExecuteNonQuery();

                        // Trouver le prochain id_Client disponible
                        int newClientId = 1;
                        string queryMaxClientId = "SELECT MAX(id_Client) FROM Client";
                        MySqlCommand commandMaxClientId = new MySqlCommand(queryMaxClientId, macConnexion, transaction);
                        result = commandMaxClientId.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            newClientId = Convert.ToInt32(result) + 1;
                        }

                        // Insertion dans Client
                        string queryClient = @"INSERT INTO Client 
                                          (id_Client, Régime, id_Utilisateur) 
                                          VALUES (@idClient, @regime, @idUser)";

                        MySqlCommand commandClient = new MySqlCommand(queryClient, macConnexion, transaction);
                        commandClient.Parameters.AddWithValue("@idClient", newClientId);
                        commandClient.Parameters.AddWithValue("@regime", regime);
                        commandClient.Parameters.AddWithValue("@idUser", newUserId);

                        commandClient.ExecuteNonQuery();

                        transaction.Commit();
                        Console.WriteLine("Compte client créé avec succès !");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Erreur lors de la création du compte : {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur de connexion : {ex.Message}");
        }
    }

    static void CreerCompteCuisinier()
    {
        Console.WriteLine("\n--- Nouveau Cuisinier ---");

        // Collecte des informations
        Console.Write("Nom : ");
        string nom = Console.ReadLine();

        Console.Write("Prénom : ");
        string prenom = Console.ReadLine();

        Console.Write("Email : ");
        string email = Console.ReadLine();

        Console.Write("Mot de passe : ");
        string motDePasse = Console.ReadLine();

        Console.Write("Téléphone : ");
        string telephone = Console.ReadLine();

        Console.Write("Rue : ");
        string rue = Console.ReadLine();

        Console.Write("Numéro : ");
        int numeroAdresse;
        while (!int.TryParse(Console.ReadLine(), out numeroAdresse))
        {
            Console.WriteLine("Veuillez entrer un numéro valide : ");
        }

        Console.Write("Code postal : ");
        string codePostal = Console.ReadLine();

        Console.Write("Ville : ");
        string ville = Console.ReadLine();

        Console.Write("Métro le plus proche : ");
        string metro = Console.ReadLine();

        try
        {
            using (MySqlConnection macConnexion = new MySqlConnection("SERVER=localhost;PORT=3306;DATABASE=projet;UID=root;PASSWORD=root;"))
            {
                macConnexion.Open();
                using (MySqlTransaction transaction = macConnexion.BeginTransaction())
                {
                    try
                    {
                        // Trouver le prochain id_Utilisateur disponible
                        int newUserId = 1;
                        string queryMaxId = "SELECT MAX(id_Utilisateur) FROM Utilisateur";
                        MySqlCommand commandMaxId = new MySqlCommand(queryMaxId, macConnexion, transaction);
                        object result = commandMaxId.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            newUserId = Convert.ToInt32(result) + 1;
                        }

                        // Insertion dans Utilisateur
                        string queryUser = @"INSERT INTO Utilisateur 
                                        (id_Utilisateur, Nom, Prénom, Téléphone, Rue, Numéro_Adresse, 
                                         Code_Postal, Ville, Email, MotDePasse, Métro_Le_Plus_Proche, Role) 
                                        VALUES (@id, @nom, @prenom, @telephone, @rue, @numero, 
                                                @codePostal, @ville, @email, @motDePasse, @metro, 'Cuisinier')";

                        MySqlCommand commandUser = new MySqlCommand(queryUser, macConnexion, transaction);
                        commandUser.Parameters.AddWithValue("@id", newUserId);
                        commandUser.Parameters.AddWithValue("@nom", nom);
                        commandUser.Parameters.AddWithValue("@prenom", prenom);
                        commandUser.Parameters.AddWithValue("@telephone", telephone);
                        commandUser.Parameters.AddWithValue("@rue", rue);
                        commandUser.Parameters.AddWithValue("@numero", numeroAdresse);
                        commandUser.Parameters.AddWithValue("@codePostal", codePostal);
                        commandUser.Parameters.AddWithValue("@ville", ville);
                        commandUser.Parameters.AddWithValue("@email", email);
                        commandUser.Parameters.AddWithValue("@motDePasse", motDePasse);
                        commandUser.Parameters.AddWithValue("@metro", metro);

                        commandUser.ExecuteNonQuery();

                        // Trouver le prochain id_Cuisinier disponible
                        int newCuisinierId = 1;
                        string queryMaxCuisinierId = "SELECT MAX(id_Cuisinier) FROM Cuisinier";
                        MySqlCommand commandMaxCuisinierId = new MySqlCommand(queryMaxCuisinierId, macConnexion, transaction);
                        result = commandMaxCuisinierId.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            newCuisinierId = Convert.ToInt32(result) + 1;
                        }

                        // Insertion dans Cuisinier
                        string queryCuisinier = @"INSERT INTO Cuisinier 
                                            (id_Cuisinier, id_Utilisateur) 
                                            VALUES (@idCuisinier, @idUser)";

                        MySqlCommand commandCuisinier = new MySqlCommand(queryCuisinier, macConnexion, transaction);
                        commandCuisinier.Parameters.AddWithValue("@idCuisinier", newCuisinierId);
                        commandCuisinier.Parameters.AddWithValue("@idUser", newUserId);

                        commandCuisinier.ExecuteNonQuery();

                        transaction.Commit();
                        Console.WriteLine("Compte cuisinier créé avec succès !");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Erreur lors de la création du compte : {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur de connexion : {ex.Message}");
        }
    }
}