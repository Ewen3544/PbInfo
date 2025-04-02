using System;
using System.Collections.Generic;

class Application
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            AfficherTitre();

            CentrerTexte("1 - CONNEXION");
            CentrerTexte("2 - INSCRIPTION");
            CentrerTexte("3 - QUITTER LE MENU");
            Console.WriteLine("\n");
            CentrerTexte("Votre choix : ", false);

            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    GererConnexion();
                    break;
                case "2":
                    GererInscription();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Choix non valide");
                    Console.ReadKey();
                    break;
            }
        }
    }

    #region Affichage de la console
    static void AfficherTitre()
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        string titre = @"
        ██╗     ██╗██╗   ██╗██╗███╗   ██╗██████╗  █████╗ ██████╗ ██╗███████╗
        ██║     ██║██║   ██║██║████╗  ██║██╔══██╗██╔══██╗██╔══██╗██║██╔════╝
        ██║     ██║██║   ██║██║██╔██╗ ██║██████╔╝███████║██████╔╝██║███████╗
        ██║     ██║╚██╗ ██╔╝██║██║╚██╗██║██╔═══╝ ██╔══██║██╔══██╗██║╚════██║
        ███████╗██║ ╚████╔╝ ██║██║ ╚████║██║     ██║  ██║██║  ██║██║███████║
        ╚══════╝╚═╝  ╚═══╝  ╚═╝╚═╝  ╚═══╝╚═╝     ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝╚══════╝
        ";
        Console.WriteLine(titre);
        Console.ResetColor();
    }
    static void AfficherMenuCuisinier()
    {
        Console.ForegroundColor = ConsoleColor.White;
        string titre = @"
    ███╗   ███╗███████╗███╗   ██╗██╗   ██╗     ██████╗██╗   ██╗██╗███████╗██╗███╗   ██╗███████╗██████╗ 
    ████╗ ████║██╔════╝████╗  ██║██║   ██║    ██╔════╝██║   ██║██║██╔════╝██║████╗  ██║██╔════╝██╔══██╗
    ██╔████╔██║█████╗  ██╔██╗ ██║██║   ██║    ██║     ██║   ██║██║███████╗██║██╔██╗ ██║█████╗  ██████╔╝
    ██║╚██╔╝██║██╔══╝  ██║╚██╗██║██║   ██║    ██║     ██║   ██║██║╚════██║██║██║╚██╗██║██╔══╝  ██╔══██╗
    ██║ ╚═╝ ██║███████╗██║ ╚████║╚██████╔╝    ╚██████╗╚██████╔╝██║███████║██║██║ ╚████║███████╗██║  ██║
    ╚═╝     ╚═╝╚══════╝╚═╝  ╚═══╝ ╚═════╝      ╚═════╝ ╚═════╝ ╚═╝╚══════╝╚═╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝
    ";
        Console.WriteLine(titre);
        Console.ResetColor();
    }

    static void AfficherMenuClient()
    {
        Console.ForegroundColor = ConsoleColor.White;
        string titre = @"
    ███╗   ███╗███████╗███╗   ██╗██╗   ██╗     ██████╗██╗     ██╗███████╗███╗   ██╗████████╗
    ████╗ ████║██╔════╝████╗  ██║██║   ██║    ██╔════╝██║     ██║██╔════╝████╗  ██║╚══██╔══╝
    ██╔████╔██║█████╗  ██╔██╗ ██║██║   ██║    ██║     ██║     ██║█████╗  ██╔██╗ ██║   ██║   
    ██║╚██╔╝██║██╔══╝  ██║╚██╗██║██║   ██║    ██║     ██║     ██║██╔══╝  ██║╚██╗██║   ██║   
    ██║ ╚═╝ ██║███████╗██║ ╚████║╚██████╔╝    ╚██████╗███████╗██║███████╗██║ ╚████║   ██║   
    ╚═╝     ╚═╝╚══════╝╚═╝  ╚═══╝ ╚═════╝      ╚═════╝╚══════╝╚═╝╚══════╝╚═╝  ╚═══╝   ╚═╝   
    ";
        Console.WriteLine(titre);
        Console.ResetColor();
    }
    static void CentrerTexte(string texte, bool avecMarge = true)
    {
        int largeurConsole = Console.WindowWidth;
        int espaceAvant = (largeurConsole - texte.Length) / 2;
        Console.WriteLine("{0}" + texte, new string(' ', espaceAvant));
        if (avecMarge) Console.WriteLine();
    }

    static string LireMotDePasseMasque() //ce qui va faire que le mot de passe est en *****
    {
        string mdp = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                mdp += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && mdp.Length > 0)
            {
                mdp = mdp.Substring(0, (mdp.Length - 1));
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);
        Console.WriteLine();
        return mdp;
    }
    #endregion

    #region Gestion connexion/inscription
    static void GererConnexion() //methode de connexion utilisateur
    {
        Console.Clear();
        AfficherTitre();

        CentrerTexte("Email : ", false);
        string email = Console.ReadLine();
        CentrerTexte("Mot de passe : ", false);
        string mdp = LireMotDePasseMasque();

        try
        {
            using (ConnexionBDD connexion = new ConnexionBDD(email, mdp))
            {
                Console.Clear();
                CentrerTexte($"Connecté en tant que {connexion.RoleUtilisateur}\n");

                if (connexion.RoleUtilisateur == "Client")
                {
                    AfficherMenuClient(connexion);
                }
                else
                {
                    AfficherMenuCuisinier(connexion);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
            Console.ReadKey();
        }
    }

    static void GererInscription()
    {
        Console.Clear();
        AfficherTitre();
        Console.ForegroundColor = ConsoleColor.Blue;
        CentrerTexte("=== INSCRIPTION ===");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkBlue;

       CentrerTexte("╔════════════════════╗");
        CentrerTexte("║       CLIENT       ║");
        CentrerTexte("╚════════════════════╝");
        CentrerTexte("╔════════════════════╗");
        CentrerTexte("║      CUISINIER     ║");
        CentrerTexte("╚════════════════════╝");
        CentrerTexte("CHOIX : ", false);
        Console.ResetColor();

        string choixType = Console.ReadLine();

        if (choixType == "1")
        {
            CreerCompteClient();
        }
        else if (choixType == "2")
        {
            CreerCompteCuisinier(); 
        }
        else
        {
            Console.WriteLine("Type invalide");
            Console.ReadKey();
        }
    }

    static void CreerCompteClient()
    {
        try
        {
            Console.Clear();
            AfficherMenuClient(); //methode qui affiche de manière stylée le mot client

            Dictionary<string, object> infos = CollecterInfosBase(); //crée le dico 'infos' du client

            CentrerTexte("Régime alimentaire : ", false);
            infos.Add("@regime", Console.ReadLine()); //ajoute le régime alimentaire a 'infos'

            using (ConnexionBDD connexion = new ConnexionBDD("root", "root")) // Connexion admin
            {
                string reqUser = @"INSERT INTO Utilisateur(Nom, Prenom, Telephone, Rue, Numero_Adresse, Code_Postal, Ville, Email, MotDePasse, Metro_Proche, Role) 
                                VALUES (@nom, @prenom, @tel, @rue, @numero, @cp, @ville, @email, @mdp, @metro, 'Client')"; 

                int idUser = connexion.ExecuterInsertEtRetournerId(reqUser, infos);
                string reqClient = "INSERT INTO Client (Régime, id_Utilisateur) VALUES (@regime, @idUser)";


                connexion.ExecuterNonQuery(reqClient, new Dictionary<string, object> { 
                    {"@regime", infos["@regime"]},
                    {"@idUser", idUser}
                });
            }

            CentrerTexte("\nCompte client créé avec succès !");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
            Console.ReadKey();
        }
    }

    static void CreerCompteCuisinier()
    {
        try
        {
            Console.Clear();
            AfficherMenuCuisinier();

            Dictionary<string, object> infos = CollecterInfosBase(); //crée le dictionnaire 'infos' du cuisinier

            CentrerTexte("Spécialité culinaire : ", false);
            infos.Add("@specialite", Console.ReadLine()); //ajoute la spé culinaire lue a 'info'

            using (ConnexionBDD connexion = new ConnexionBDD("root", "root")) //crée une connexion pour cuisinier avec les droits admin
            {
                
                string reqUser = @"INSERT INTO Utilisateur(Nom, Prenom, Telephone, Rue, Numero_Adresse, Code_Postal, Ville, Email, MotDePasse, Metro_Proche, Role) 
                                VALUES (@nom, @prenom, @tel, @rue, @numero, @cp, @ville, @email, @mdp, @metro, 'Cuisinier')";   //insertion de 'info' dans la table Utilisateur
                int idUser = connexion.ExecuterInsertEtRetournerId(reqUser, infos); //execute la requete sql et renvoie l'id user

                
                string reqCuisinier = "INSERT INTO Cuisinier (Specialite, id_Utilisateur) VALUES (@specialite, @idUser)"; //insère les infos dans la table cuisinier
                connexion.ExecuterNonQuery(reqCuisinier, new Dictionary<string, object> {
                    {"@specialite", infos["@specialite"]},
                    {"@idUser", idUser}
                });
            }

            CentrerTexte("\nCompte cuisinier créé avec succès !");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}"); //effiche msg d'erreur si erreur
            Console.ReadKey();
        }
    }

    static Dictionary<string, object> CollecterInfosBase()
    {
        var infos = new Dictionary<string, object>();

        CentrerTexte("Nom : ", false);
        infos.Add("@nom", Console.ReadLine());

        CentrerTexte("Prénom : ", false);
        infos.Add("@prenom", Console.ReadLine());

        CentrerTexte("Email : ", false);
        infos.Add("@email", Console.ReadLine());

        CentrerTexte("Mot de passe : ", false);
        infos.Add("@mdp", LireMotDePasseMasque());

        CentrerTexte("Téléphone : ", false);
        infos.Add("@tel", Console.ReadLine());

        CentrerTexte("Rue : ", false);
        infos.Add("@rue", Console.ReadLine());

        CentrerTexte("Numéro : ", false);
        infos.Add("@numero", Console.ReadLine());

        CentrerTexte("Code postal : ", false);
        infos.Add("@cp", Console.ReadLine());

        CentrerTexte("Ville : ", false);
        infos.Add("@ville", Console.ReadLine());

        CentrerTexte("Métro proche : ", false);
        infos.Add("@metro", Console.ReadLine());

        return infos;
    }
    #endregion

    #region Fonctionnalités Favoris
    static void AfficherFavoris(ConnexionBDD connexion)
    {
        try
        {
            string requete = @"SELECT 
                                    f.id_Plat,
                                    p.Nom,
                                    p.PrixParPersonne,
                                    p.Nationalite,
                                    p.RegimeAlimentaire,
                                    u.Nom AS Cuisinier,
                                    f.Date_Ajout
                         FROM 
                                    Favoris f
                         JOIN 
                                    Plat p ON f.id_Plat = p.id_Plat
                         JOIN 
                                    Cuisinier c ON p.id_Cuisinier = c.id_Cuisinier
                         JOIN 
                                    Utilisateur u ON c.id_Utilisateur = u.id_Utilisateur
                         WHERE 
                                    f.id_Client = @idClient
                         ORDER BY 
                                    f.Date_Ajout DESC";

            var favoris = connexion.ExecuterRequete(requete,
                new Dictionary<string, object> { { "@idClient", connexion.IdClient } });

            Console.Clear();
            CentrerTexte("=== VOS PLATS FAVORIS ===");

            if (favoris.Count == 0)
            {
                CentrerTexte("Aucun plat favori enregistré.");
            }
            else
            {
                foreach (var fav in favoris)
                {
                    Console.WriteLine($"\nPlat #{fav["id_Plat"]}");
                    Console.WriteLine($"- Nom: {fav["Nom"]}");
                    Console.WriteLine($"- Cuisinier: {fav["Cuisinier"]}");
                    Console.WriteLine($"- Prix/pers: {fav["PrixParPersonne"]}€");
                    Console.WriteLine($"- Nationalité: {fav["Nationalite"]}");
                    Console.WriteLine($"- Régime: {fav["RegimeAlimentaire"]}");
                    Console.WriteLine($"- Ajouté le: {((DateTime)fav["Date_Ajout"]):dd/MM/yyyy}");
                    Console.WriteLine(new string('-', 40));
                }
            }
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
            Console.ReadKey();
        }
    }

    static void AjouterAuxFavoris(ConnexionBDD connexion)
    {
        try
        {
            Console.Clear();
            CentrerTexte("=== AJOUTER UN FAVORI ===");

            string requetePlats = @"SELECT 
                                        p.id_Plat,
                                        p.Nom,
                                        p.PrixParPersonne,
                                        p.Nationalite, 
                                        p.RegimeAlimentaire,
                                        u.Nom AS Cuisinier
                              FROM 
                                        Plat p
                              JOIN 
                                        Cuisinier c ON p.id_Cuisinier = c.id_Cuisinier
                              JOIN 
                                        Utilisateur u ON c.id_Utilisateur = u.id_Utilisateur
                              WHERE 
                                        p.DatePeremption > CURRENT_DATE()";

            var plats = connexion.ExecuterRequete(requetePlats);

            if (plats.Count == 0)
            {
                CentrerTexte("Aucun plat disponible pour le moment.");
                Console.ReadKey();
                return;
            }

            CentrerTexte("Plats disponibles :");
            foreach (var plat in plats)
            {
                Console.WriteLine($"\nID: {plat["id_Plat"]}");
                Console.WriteLine($"- Nom: {plat["Nom"]}");
                Console.WriteLine($"- Cuisinier: {plat["Cuisinier"]}");
                Console.WriteLine($"- Prix/pers: {plat["PrixParPersonne"]}€");
                Console.WriteLine($"- Nationalité: {plat["Nationalite"]}");
                Console.WriteLine($"- Régime: {plat["RegimeAlimentaire"]}");
                Console.WriteLine(new string('-', 40));
            }

            CentrerTexte("\nID du plat à ajouter aux favoris : ", false);
            int idPlat = int.Parse(Console.ReadLine());

            // Vérifier si le plat est déjà en favori
            string reqVerif = "SELECT COUNT(*) FROM Favoris WHERE id_Client = @idClient AND id_Plat = @idPlat";
            int existeDeja = Convert.ToInt32(connexion.ExecuterRequete(reqVerif,
                new Dictionary<string, object> {
                {"@idClient", connexion.IdClient},
                {"@idPlat", idPlat}
                })[0]["COUNT(*)"]);

            if (existeDeja > 0)
            {
                CentrerTexte("Ce plat est déjà dans vos favoris.");
                Console.ReadKey();
                return;
            }

            // Ajouter aux favoris
            string reqAjout = "INSERT INTO Favoris(id_Client, id_Plat) VALUES (@idClient, @idPlat)";
            connexion.ExecuterNonQuery(reqAjout,
                new Dictionary<string, object> {
                {"@idClient", connexion.IdClient},
                {"@idPlat", idPlat}
                });

            CentrerTexte("\nPlat ajouté aux favoris avec succès !");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
            Console.ReadKey();
        }
    }
    #endregion

    #region Menu cuisinier


    static void AfficherMenuCuisinier(ConnexionBDD connexion)
    {
        while (true)
        {
            Console.Clear();
            CentrerTexte("=== MENU CUISINIER ===");

            CentrerTexte("1. Voir mes plats");
            CentrerTexte("2. Commandes à préparer");
            CentrerTexte("3. Créer un nouveau plat");
            CentrerTexte("4. Déconnexion");
            CentrerTexte("Choix : ", false);

            switch (Console.ReadLine())
            {
                case "1":
                    AfficherPlatsCuisinier(connexion);
                    break;
                case "2":
                    AfficherCommandesAPreparer(connexion);
                    break;
                case "3":
                    CreerPlat(connexion);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Option invalide");
                    break;
            }
        }
    }
    #endregion

    #region Fonctionnalités Client
    static void AfficherCommandesClient(ConnexionBDD connexion)
    {
        try
        {
            string requete = @"SELECT 
                                        c.id_Commande,
                                        p.Nom AS NomPlat,
                                        c.Date_Commande, 
                                        c.Prix_Commande,
                                        c.Statut
                             FROM 
                                        Commande c
                             JOIN 
                                        Contient ct 
                                                ON c.id_Commande = ct.id_Commande
                             JOIN 
                                        Plat p 
                                                ON ct.id_Plat = p.id_Plat
                             WHERE c.id_Client = @idClient
                             ORDER BY c.Date_Commande DESC";

            var commandes = connexion.ExecuterRequete(requete,
                new Dictionary<string, object> { { "@idClient", connexion.IdUtilisateur } });

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            CentrerTexte("=== VOS COMMANDES ===");
            Console.ResetColor();

            if (commandes.Count == 0)
            {
                CentrerTexte("Aucune commande trouvée.");
            }
            else
            {
                foreach (var cmd in commandes)
                {
                    Console.WriteLine($"\nCommande #{cmd["id_Commande"]}");
                    Console.WriteLine($"- Plat: {cmd["NomPlat"]}");
                    Console.WriteLine($"- Date: {((DateTime)cmd["Date_Commande"]):dd/MM/yyyy}");
                    Console.WriteLine($"- Prix: {cmd["Prix_Commande"]}€");
                    Console.WriteLine($"- Statut: {cmd["Statut"]}");
                    Console.WriteLine(new string('-', 40));
                }
            }
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
            Console.ReadKey();
        }
    }

    static void AfficherLivraisonsClient(ConnexionBDD connexion)
    {
        try
        {
            string requete = @"SELECT 
                                        l.id_Livraison,
                                        l.Statut,
                                        l.Date_Livraison,
                                        l.Adresse_Livraison,
                                        p.Nom AS NomPlat
                             FROM 
                                        Livraison l
                             JOIN 
                                        Commande c 
                                        ON l.id_Commande = c.id_Commande
                             JOIN 
                                        Contient ct 
                                        ON c.id_Commande = ct.id_Commande
                             JOIN 
                                        Plat p 
                                        ON ct.id_Plat = p.id_Plat
                             WHERE c.id_Client = @idClient
                             ORDER BY l.Date_Livraison DESC";

            var livraisons = connexion.ExecuterRequete(requete,
                new Dictionary<string, object> { { "@idClient", connexion.IdUtilisateur } });

            Console.Clear();
            CentrerTexte("=== VOS LIVRAISONS ===");

            if (livraisons.Count == 0)
            {
                CentrerTexte("Aucune livraison en cours.");
            }
            else
            {
                foreach (var liv in livraisons)
                {
                    Console.WriteLine($"\nLivraison #{liv["id_Livraison"]}");
                    Console.WriteLine($"- Plat: {liv["NomPlat"]}");
                    Console.WriteLine($"- Statut: {liv["Statut"]}");
                    if (liv["Date_Livraison"] != DBNull.Value)
                        Console.WriteLine($"- Date livraison: {((DateTime)liv["Date_Livraison"]):dd/MM/yyyy HH:mm}");
                    Console.WriteLine($"- Adresse: {liv["Adresse_Livraison"]}");
                    Console.WriteLine(new string('-', 40));
                }
            }
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
            Console.ReadKey();
        }
    }
    #endregion

    #region Fonctionnalités Cuisinier
    static void AfficherPlatsCuisinier(ConnexionBDD connexion)
    {
        try
        {
            string requete = @"SELECT 
                                        p.id_Plat,
                                        p.Nom,
                                        p.TypePlat,
                                        p.PrixParPersonne, 
                                        p.Nationalite,
                                        p.RegimeAlimentaire,
                                        p.DateFabrication,
                                        COUNT(c.id_Commande) AS NbCommandes
                             FROM 
                                        Plat p
                             LEFT JOIN 
                                        Contient ct 
                                                 ON p.id_Plat = ct.id_Plat
                             LEFT JOIN 
                                        Commande c 
                                                 ON ct.id_Commande = c.id_Commande
                             WHERE p.id_Cuisinier = @idCuisinier
                             GROUP BY p.id_Plat
                             ORDER BY NbCommandes DESC";

            var plats = connexion.ExecuterRequete(requete,
                new Dictionary<string, object> { { "@idCuisinier", connexion.IdUtilisateur } });

            Console.Clear();
            CentrerTexte("=== VOS PLATS ===");

            if (plats.Count == 0)
            {
                CentrerTexte("Aucun plat enregistré.");
            }
            else
            {
                foreach (var plat in plats)
                {
                    Console.WriteLine($"\nPlat #{plat["id_Plat"]}");
                    Console.WriteLine($"- Nom: {plat["Nom"]}");
                    Console.WriteLine($"- Type: {plat["TypePlat"]}");
                    Console.WriteLine($"- Prix/pers: {plat["PrixParPersonne"]}€");
                    Console.WriteLine($"- Nationalité: {plat["Nationalite"]}");
                    Console.WriteLine($"- Régime: {plat["RegimeAlimentaire"]}");
                    Console.WriteLine($"- Fabrication: {((DateTime)plat["DateFabrication"]):dd/MM/yyyy}");
                    Console.WriteLine($"- Commandes: {plat["NbCommandes"]}");
                    Console.WriteLine(new string('-', 40));
                }
            }
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
            Console.ReadKey();
        }
    }

    static void AfficherCommandesAPreparer(ConnexionBDD connexion)
    {
        try
        {
            string requete = @"SELECT   
                                        c.id_Commande,
                                        p.Nom AS NomPlat,
                                        c.Date_Commande,
                                        c.Prix_Commande,
                                        u.Nom AS NomClient,
                                        u.Telephone
                             FROM 
                                        Commande c
                             JOIN 
                                        Contient ct
                                        ON c.id_Commande = ct.id_Commande
                             JOIN 
                                        Plat p
                                        ON ct.id_Plat = p.id_Plat
                             JOIN 
                                        Client cl 
                                        ON c.id_Client = cl.id_Client
                             JOIN 
                                        Utilisateur u 
                                        ON cl.id_Utilisateur = u.id_Utilisateur
                             WHERE 
                                        p.id_Cuisinier = @idCuisinier
                             AND 
                                        c.Statut = 'En préparation'
                             ORDER BY 
                                        c.Date_Commande";
                


            var commandes = connexion.ExecuterRequete(requete,
                new Dictionary<string, object> { { "@idCuisinier", connexion.IdUtilisateur } });

            Console.Clear();
            CentrerTexte("=== COMMANDES À PRÉPARER ===");

            if (commandes.Count == 0)
            {
                CentrerTexte("Aucune commande à préparer.");
            }
            else
            {
                foreach (var cmd in commandes)
                {
                    Console.WriteLine($"\nCommande #{cmd["id_Commande"]}");
                    Console.WriteLine($"- Plat: {cmd["NomPlat"]}");
                    Console.WriteLine($"- Client: {cmd["NomClient"]}");
                    Console.WriteLine($"- Téléphone: {cmd["Telephone"]}");
                    Console.WriteLine($"- Date: {((DateTime)cmd["Date_Commande"]):dd/MM/yyyy HH:mm}");
                    Console.WriteLine($"- Prix: {cmd["Prix_Commande"]}€");
                    Console.WriteLine(new string('-', 40));
                }
            }
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
            Console.ReadKey();
        }
    }

    static void CreerPlat(ConnexionBDD connexion)
    {
        try
        {
            Console.Clear();
            CentrerTexte("=== NOUVEAU PLAT ===");

            Dictionary<string, object> parametres = new Dictionary<string, object>();

            CentrerTexte("Nom du plat : ", false);
            parametres.Add("@nom", Console.ReadLine());

            CentrerTexte("Type (Entrée/Principal/Dessert) : ", false);
            parametres.Add("@type", Console.ReadLine());

            CentrerTexte("Nombre de personnes : ", false);
            parametres.Add("@nbPersonnes", int.Parse(Console.ReadLine()));

            CentrerTexte("Date de fabrication (yyyy-mm-dd) : ", false);
            parametres.Add("@dateFab", DateTime.Parse(Console.ReadLine()));

            CentrerTexte("Date de péremption (yyyy-mm-dd) : ", false);
            parametres.Add("@datePeremp", DateTime.Parse(Console.ReadLine()));

            CentrerTexte("Prix par personne : ", false);
            parametres.Add("@prix", decimal.Parse(Console.ReadLine()));

            CentrerTexte("Nationalité : ", false);
            parametres.Add("@nationalite", Console.ReadLine());

            CentrerTexte("Régime alimentaire : ", false);
            parametres.Add("@regime", Console.ReadLine());

            CentrerTexte("Ingrédients (séparés par des virgules) : ", false);
            parametres.Add("@ingredients", Console.ReadLine());

            parametres.Add("@idCuisinier", connexion.IdUtilisateur);

            string requete = @"INSERT INTO Plat(Nom, TypePlat, NbPersonnes, DateFabrication, DatePeremption, PrixParPersonne, Nationalite, RegimeAlimentaire, Ingredients, id_Cuisinier)
                            VALUES (@nom, @type, @nbPersonnes, @dateFab, @datePeremp, @prix, @nationalite, @regime, @ingredients, @idCuisinier)";

            connexion.ExecuterNonQuery(requete, parametres);
            CentrerTexte("\nVotre nouveau plat est bien créé !");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErreur : {ex.Message}");
            Console.ReadKey();
        }
    }
    #endregion

    #region Fonctionnalités Client
    static void AfficherMenuClient(ConnexionBDD connexion)
    {
        while (true)
        {
            Console.Clear();

            CentrerTexte("1. Commander un plat");
            CentrerTexte("2. Commander le menu du jour");
            CentrerTexte("3. Mes commandes");
            CentrerTexte("4. Suivi livraisons");
            CentrerTexte("5. Donner un avis");
            CentrerTexte("6. Mes plats favoris");
            CentrerTexte("7. Ajouter un favori");
            CentrerTexte("8. Déconnexion");
            CentrerTexte("Choix : ", false);

            switch (Console.ReadLine())
            {
                case "1":
                    CommanderPlat(connexion);
                    break;
                case "2":
                    CommanderMenuDuJour(connexion);
                    break;
                case "3":
                    AfficherCommandesClient(connexion);
                    break;
                case "4":
                    AfficherLivraisonsClient(connexion);
                    break;
                case "5":
                    DonnerAvis(connexion);
                    break;
                case "6":
                    AfficherFavoris(connexion);
                    break;
                case "7":
                    AjouterAuxFavoris(connexion);
                    break;
                case "8":
                    return;
                default:
                    Console.WriteLine("Option invalide");
                    break;
            }
        }
    }

    static void CommanderMenuDuJour(ConnexionBDD connexion)
    {
        try
        {
            Console.Clear();
            CentrerTexte("=== COMMANDER LE MENU DU JOUR ===");

            // Récupérer le menu du jour
            string requeteMenu = @"SELECT 
                                        m.id_Menu,
                                        m.DateMenu,
                                        pp.Nom AS PlatPrincipal,
                                        pp.PrixParPersonne AS PrixPrincipal,
                                        d.Nom AS Dessert,
                                        d.PrixParPersonne AS PrixDessert,
                                        m.Description
                              FROM 
                                        MenuDuJour m
                              JOIN 
                                        Plat pp ON m.id_PlatPrincipal = pp.id_Plat
                              LEFT JOIN 
                                        Plat d ON m.id_Dessert = d.id_Plat
                              WHERE 
                                        m.DateMenu = CURRENT_DATE()";

            var menus = connexion.ExecuterRequete(requeteMenu);

            if (menus.Count == 0)
            {
                CentrerTexte("Aucun menu disponible pour aujourd'hui.");
                Console.ReadKey();
                return;
            }

            var menu = menus[0];

            Console.WriteLine($"\nMenu du {((DateTime)menu["DateMenu"]):dd/MM/yyyy}");
            Console.WriteLine($"- Plat principal: {menu["PlatPrincipal"]} ({menu["PrixPrincipal"]}€/pers)");
            if (menu["Dessert"] != DBNull.Value)
                Console.WriteLine($"- Dessert: {menu["Dessert"]} ({menu["PrixDessert"]}€/pers)");
            Console.WriteLine($"- Description: {menu["Description"]}");
            Console.WriteLine(new string('-', 40));

            CentrerTexte("\nNombre de personnes : ", false);
            int nbPersonnes = int.Parse(Console.ReadLine());

            decimal prixTotal = nbPersonnes * (decimal)menu["PrixPrincipal"];
            if (menu["Dessert"] != DBNull.Value)
                prixTotal += nbPersonnes * (decimal)menu["PrixDessert"];

            // Créer la commande
            string reqCommande = @"INSERT INTO Commande(Date_Commande, Prix_Commande, Statut, id_Client)
                            VALUES (NOW(), @prix, 'En préparation', @idClient);
                            SELECT LAST_INSERT_ID();";

            int idCommande = connexion.ExecuterInsertEtRetournerId(reqCommande,
                new Dictionary<string, object> {
                {"@prix", prixTotal},
                {"@idClient", connexion.IdClient}
                });

            // Ajouter le plat principal à la commande
            string reqPlatPrincipal = @"SELECT id_PlatPrincipal FROM MenuDuJour WHERE id_Menu = @idMenu";
            int idPlatPrincipal = Convert.ToInt32(connexion.ExecuterRequete(reqPlatPrincipal,
                new Dictionary<string, object> { { "@idMenu", menu["id_Menu"] } })[0]["id_PlatPrincipal"]);

            string reqContientPrincipal = @"INSERT INTO Contient(id_Commande, id_Plat, Quantite)
                                    VALUES (@idCommande, @idPlat, @quantite)";
            connexion.ExecuterNonQuery(reqContientPrincipal,
                new Dictionary<string, object> {
                {"@idCommande", idCommande},
                {"@idPlat", idPlatPrincipal},
                {"@quantite", nbPersonnes}
                });

            // Ajouter le dessert si présent
            if (menu["Dessert"] != DBNull.Value)
            {
                string reqDessert = @"SELECT id_Dessert FROM MenuDuJour WHERE id_Menu = @idMenu";
                int idDessert = Convert.ToInt32(connexion.ExecuterRequete(reqDessert,
                    new Dictionary<string, object> { { "@idMenu", menu["id_Menu"] } })[0]["id_Dessert"]);

                string reqContientDessert = @"INSERT INTO Contient(id_Commande, id_Plat, Quantite)
                                      VALUES (@idCommande, @idPlat, @quantite)";
                connexion.ExecuterNonQuery(reqContientDessert,
                    new Dictionary<string, object> {
                    {"@idCommande", idCommande},
                    {"@idPlat", idDessert},
                    {"@quantite", nbPersonnes}
                    });
            }

            // Créer la livraison
            string reqLivraison = @"INSERT INTO Livraison(Statut, Date_Livraison, Adresse_Livraison, id_Commande)
                          VALUES ('En préparation', NULL, (SELECT CONCAT(Rue, ' ', Numero_Adresse, ', ', Code_Postal, ' ', Ville) 
                           FROM Utilisateur WHERE id_Utilisateur = @idUser), @idCommande)";

            connexion.ExecuterNonQuery(reqLivraison,
                new Dictionary<string, object> {
                {"@idUser", connexion.IdUtilisateur},
                {"@idCommande", idCommande}
                });

            CentrerTexte($"\nCommande #{idCommande} passée avec succès !");
            CentrerTexte($"Prix total: {prixTotal}€");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
            Console.ReadKey();
        }
    }
    static void CommanderPlat(ConnexionBDD connexion)
    {
        try
        {
            Console.Clear();
            CentrerTexte("=== COMMANDER UN PLAT ===");

            // Afficher tous les plats disponibles
            string requetePlats = @"SELECT 
                                            p.id_Plat,
                                            p.Nom,
                                            p.PrixParPersonne,
                                            p.Nationalite, 
                                            p.RegimeAlimentaire,
                                            u.Nom AS Cuisinier
                              FROM 
                                            Plat p
                              JOIN 
                                            Cuisinier c 
                                            ON p.id_Cuisinier = c.id_Cuisinier
                              JOIN 
                                            Utilisateur u 
                                            ON c.id_Utilisateur = u.id_Utilisateur
                              WHERE         p.DatePeremption > CURRENT_DATE()";

            var plats = connexion.ExecuterRequete(requetePlats);

            if (plats.Count == 0)
            {
                CentrerTexte("Aucun plat disponible pour le moment.");
                Console.ReadKey();
                return;
            }

            CentrerTexte("Plats disponibles :");
            foreach (var plat in plats)
            {
                Console.WriteLine($"\nID: {plat["id_Plat"]}");
                Console.WriteLine($"- Nom: {plat["Nom"]}");
                Console.WriteLine($"- Cuisinier: {plat["Cuisinier"]}");
                Console.WriteLine($"- Prix/pers: {plat["PrixParPersonne"]}€");
                Console.WriteLine($"- Nationalité: {plat["Nationalite"]}");
                Console.WriteLine($"- Régime: {plat["RegimeAlimentaire"]}");
                Console.WriteLine(new string('-', 40));
            }

            CentrerTexte("\nID du plat à commander : ", false);
            int idPlat = int.Parse(Console.ReadLine());

            CentrerTexte("Nombre de personnes : ", false);
            int nbPersonnes = int.Parse(Console.ReadLine());

            // Vérifier que le plat existe
            var platSelectionne = plats.Find(p => (int)p["id_Plat"] == idPlat);
            if (platSelectionne == null)
            {
                CentrerTexte("ID de plat invalide.");
                Console.ReadKey();
                return;
            }

            decimal prixTotal = nbPersonnes * (decimal)platSelectionne["PrixParPersonne"];

            // Créer la commande
            string reqCommande = @"INSERT INTO Commande(Date_Commande, Prix_Commande, Statut, id_Client)
                                VALUES (NOW(), @prix, 'En préparation', @idClient);
                                SELECT LAST_INSERT_ID();"; //chatté

            int idCommande = connexion.ExecuterInsertEtRetournerId(reqCommande,
                new Dictionary<string, object> {
                {"@prix", prixTotal},
                {"@idClient", connexion.IdUtilisateur}
                });

            // Ajouter le plat à la commande
            string reqContient = @"INSERT INTO Contient(id_Commande, id_Plat, Quantite)
                                   VALUES (@idCommande, @idPlat, @quantite)";

            connexion.ExecuterNonQuery(reqContient,
                new Dictionary<string, object> {
                {"@idCommande", idCommande},
                {"@idPlat", idPlat},
                {"@quantite", nbPersonnes}
                });

            // Créer la livraison
            string reqLivraison = @"INSERT INTO Livraison(Statut, Date_Livraison, Adresse_Livraison, id_Commande)
                              VALUES ('En préparation', NULL, (SELECT CONCAT(Rue, ' ', Numero_Adresse, ', ', Code_Postal, ' ', Ville) 
                               FROM Utilisateur WHERE id_Utilisateur = @idUser), @idCommande)";

            connexion.ExecuterNonQuery(reqLivraison,
                new Dictionary<string, object> {
                {"@idUser", connexion.IdUtilisateur},
                {"@idCommande", idCommande}
                });

            CentrerTexte($"\nCommande #{idCommande} passée avec succès !");
            CentrerTexte($"Prix total: {prixTotal}€");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
            Console.ReadKey();
        }
    }

    static void DonnerAvis(ConnexionBDD connexion)
    {
        try
        {
            Console.Clear();
            CentrerTexte("=== DONNER UN AVIS ===");

            // Afficher les commandes livrées sans avis
            string requeteCommandes = @"SELECT 
                                                    c.id_Commande,
                                                    p.id_Plat,
                                                    p.Nom AS NomPlat, 
                                                    u.Nom AS Cuisinier,
                                                    l.Date_Livraison
                                   FROM 
                                                    Commande c
                                   JOIN 
                                                    Contient ct 
                                                    ON c.id_Commande = ct.id_Commande
                                   JOIN 
                                                    Plat p 
                                                    ON ct.id_Plat = p.id_Plat
                                   JOIN 
                                                    Cuisinier cu 
                                                    ON p.id_Cuisinier = cu.id_Cuisinier
                                   JOIN 
                                                    Utilisateur u 
                                                    ON cu.id_Utilisateur = u.id_Utilisateur
                                   JOIN 
                                                    Livraison l 
                                                    ON c.id_Commande = l.id_Commande
                                   LEFT JOIN 
                                                    Avis_Client av 
                                                    ON c.id_Client = av.id_Client 
                                                    AND p.id_Plat = av.id_Plat
                                   WHERE c.id_Client = @idClient
                                   AND l.Statut = 'Livrée'
                                   AND av.id_Avis IS NULL";

            var commandes = connexion.ExecuterRequete(requeteCommandes,
                new Dictionary<string, object> { { "@idClient", connexion.IdUtilisateur } });

            if (commandes.Count == 0)
            {
                CentrerTexte("Aucune commande livrée sans avis.");
                Console.ReadKey();
                return;
            }

            CentrerTexte("Commandes pouvant être notées :");
            foreach (var cmd in commandes)
            {
                Console.WriteLine($"\nCommande #{cmd["id_Commande"]}");
                Console.WriteLine($"- Plat: {cmd["NomPlat"]}");
                Console.WriteLine($"- Cuisinier: {cmd["Cuisinier"]}");
                Console.WriteLine($"- Date livraison: {((DateTime)cmd["Date_Livraison"]):dd/MM/yyyy}");
                Console.WriteLine(new string('-', 40));
            }

            CentrerTexte("\nID du plat à noter : ", false);
            int idPlat = int.Parse(Console.ReadLine());

            // Vérifier que le plat est dans la liste
            var platSelectionne = commandes.Find(c => (int)c["id_Plat"] == idPlat);
            if (platSelectionne == null)
            {
                CentrerTexte("ID de plat invalide.");
                Console.ReadKey();
                return;
            }

            CentrerTexte("Note (0-5) : ", false);
            decimal note = decimal.Parse(Console.ReadLine());
            if (note < 0 || note > 5)
            {
                CentrerTexte("La note doit être entre 0 et 5.");
                Console.ReadKey();
                return;
            }

            CentrerTexte("Commentaire (facultatif) : ", false);
            string commentaire = Console.ReadLine();

            // Enregistrer l'avis
            string reqAvis = @"INSERT INTO Avis_Client(Note, Commentaire, Date_Avis, id_Client, id_Plat)
                          VALUES (@note, @commentaire, NOW(), @idClient, @idPlat)";

            connexion.ExecuterNonQuery(reqAvis,
                new Dictionary<string, object> {
                {"@note", note},
                {"@commentaire", string.IsNullOrEmpty(commentaire) ? DBNull.Value : commentaire},
                {"@idClient", connexion.IdUtilisateur},
                {"@idPlat", idPlat}
                });

            CentrerTexte("\nMerci pour votre avis !");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
            Console.ReadKey();
        }
    }
    #endregion
}