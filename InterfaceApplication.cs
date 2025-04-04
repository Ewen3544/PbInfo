using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

class Application
{
    #region Main et affichage menu principal
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            AfficherTitre();
            string bordureHaut = "╔══════════════════════════════╗";
            string bordureBas = "╚══════════════════════════════╝";
            string separateur = "╠══════════════════════════════╣";
            CentrerTexte(bordureHaut);
            CentrerTexte("║        MENU PRINCIPAL        ║");
            CentrerTexte(separateur);
            CentrerTexte("║       1 - CONNEXION          ║");
            CentrerTexte("║       2 - INSCRIPTION        ║");
            CentrerTexte("║       3 - QUITTER LE MENU    ║");
            CentrerTexte(bordureBas);
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
                    Console.WriteLine("CHOIX NON VALIDE");
                    Console.ReadKey();
                    break;
            }
        }
    }
    #endregion

    #region Grosses Inscriptions stylées (titres ascii par chatgpt)
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
    #endregion

    #region Méthodes CentrerTexte et LireMotDePasseMasque (methodes pour un menu plus clean)
    public static void CentrerTexte(string texte, bool avecMarge = true)
    {
        Console.WriteLine(new string(' ', (Console.WindowWidth - texte.Length) / 2) + texte);
        if (avecMarge) Console.WriteLine();
    }

    static string LireMotDePasseMasque()
    {
        string mdp = "";
        ConsoleKeyInfo key;
        while (true)
        {
            key = Console.ReadKey(true); ///recupere les touches sur lesquelles l'utilisiteur appuie
            if (key.Key == ConsoleKey.Enter) ///coupe la boucle si on appuie sur entrer (quand on a fini d'écrire le mdp)
            {
                break;
            }

           
            if (key.Key == ConsoleKey.Backspace) ///si appuie sur backspace alors enleve un caractere au mdp
            {
                if (mdp.Length > 0) 
                {
                    mdp = mdp.Substring(0, mdp.Length - 1);
                    Console.Write("\b \b"); 
                }
            }
            
            else
            {
                if (key.Key != ConsoleKey.Backspace) ///si touche normale alors ajoute au mdp
                {
                    mdp = mdp + key.KeyChar;
                    Console.Write("*"); ///écrit * pour plus de sécurité
                }
            }
        }

        Console.WriteLine();
        return mdp; 
    }


    #endregion

    #region Methodes d'inscription et connexion
    static void GererConnexion() ///methode de connexion utilisateur
    {
        ///affichage menu pour mot de passe et email
        Console.Clear();
        AfficherTitre();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        CentrerTexte("EMAIL", false);
        Console.ResetColor();
        string email = Console.ReadLine();


        Console.ForegroundColor = ConsoleColor.DarkYellow;
        CentrerTexte("MOT DE PASSE", false);
        Console.ResetColor();
        string mdp = LireMotDePasseMasque();

        ///fin affichage menu pour mdp email

        try
        {
            using (ConnexionBDD connexion = new ConnexionBDD(email, mdp))
            {
                Console.Clear();
                CentrerTexte($"Tu est donc un {connexion.RoleUtilisateur} !");
                Console.WriteLine();

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
            Console.WriteLine($"{ex.Message}"); ///dans le cas ou ya une erreur
            Console.ReadKey();
        }
    }

    static void GererInscription() ///metgode d'inscription utilisateur
    {
        ///partie affichage menu-----------------------
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

        ///fin partie affichage menu----------------------

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
            Console.WriteLine("tu t'es trompé la poto");
            Console.ReadKey();
        }
    }

    static void CreerCompteClient()
    {
        try
        {
            Console.Clear();
            AfficherMenuClient(); ///methode qui affiche de manière stylée le mot client

            Dictionary<string, object> infos = CollecterInfosBase(); ///crée le dico 'infos' du client

            CentrerTexte("Régime alimentaire : ", false);
            infos.Add("@regime", Console.ReadLine()); ///ajoute le régime alimentaire a 'infos'

            using (ConnexionBDD connexion = new ConnexionBDD(pourInscription: true)) ///utilise l'instance d'inscription au lieu de connexion dans ConnexionBDD
            {
                string reqUser = @"INSERT INTO Utilisateur(Nom,
                                                            Prenom,
                                                            Telephone,
                                                            Rue,
                                                            Numero_Adresse,
                                                            Code_Postal,
                                                            Ville,
                                                            Email,
                                                            Metro_Proche,
                                                            MotDePasse,
                                                            Role) 
    
                                 VALUES (@nom,
                                        @prenom,
                                        @tel,
                                        @rue,
                                        @numero,
                                        @cp,
                                        @ville,
                                        @email,
                                        @metro,
                                        @mdp,
                                        'Client')"; ///insère les informations


                int idUser = connexion.ExecuterInsertEtRetournerId(reqUser, infos);
                string reqClient = "INSERT INTO Client (Regime, id_Utilisateur) VALUES (@regime, @idUser)";


                connexion.ExecuterNonQuery(reqClient, new Dictionary<string, object> { 
                    {"@regime", infos["@regime"]},
                    {"@idUser", idUser}
                });
            }


            Console.WriteLine();
            CentrerTexte("Bienvenue !");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            Console.ReadKey();
        }
    }

    static void CreerCompteCuisinier()
    {
        try
        {
            Console.Clear();
            AfficherMenuCuisinier();

            Dictionary<string, object> infos = CollecterInfosBase(); ///appelle la fonction dictionnaire 'infos' du cuisinier

            CentrerTexte("SPECIALITE CULINAIRE", false);
            infos.Add("@specialite", Console.ReadLine()); ///ajoute la spé culinaire lue a 'info'

            using (ConnexionBDD connexion = new ConnexionBDD(pourInscription: true)) ///crée une connexion pour cuisinier avec les droits admin
            {
                
                string reqUser = @"INSERT INTO Utilisateur(Nom,
                                                            Prenom,
                                                            Telephone,
                                                            Rue,
                                                            Numero_Adresse,
                                                            Code_Postal,
                                                            Ville,
                                                            Email,
                                                            MotDePasse,
                                                            Metro_Proche,
                                                            Role) 
                                VALUES (@nom,
                                        @prenom,
                                        @tel,
                                        @rue,
                                        @numero,
                                        @cp,
                                        @ville,
                                        @email,
                                        @mdp,
                                        @metro,
                                        'Cuisinier')";   ///insertion de 'info' dans la table Utilisateur
                int idUser = connexion.ExecuterInsertEtRetournerId(reqUser, infos); ///execute la requete sql et renvoie l'id user

                
                string reqCuisinier = "INSERT INTO Cuisinier (Specialite, id_Utilisateur) VALUES (@specialite, @idUser)"; ///insère les infos dans la table cuisinier
                connexion.ExecuterNonQuery(reqCuisinier, new Dictionary<string, object> {
                    {"@specialite", infos["@specialite"]},
                    {"@idUser", idUser}
                });
            }

            CentrerTexte("Bienvenue !!");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}"); ///effiche msg d'erreur si erreur
            Console.ReadKey();
        }
    }

    static Dictionary<string, object> CollecterInfosBase()
    {
        Dictionary<string, object> infos = new Dictionary<string, object>(); 

        CentrerTexte("NOM", false);
        infos.Add("@nom", Console.ReadLine());

        CentrerTexte("PRENOM", false);
        infos.Add("@prenom", Console.ReadLine());

        CentrerTexte("EMAIL", false);
        infos.Add("@email", Console.ReadLine());

        CentrerTexte("MOT DE PASSE", false);
        infos.Add("@mdp", LireMotDePasseMasque());

        CentrerTexte("TELEPHONE", false);
        infos.Add("@tel", Console.ReadLine());

        CentrerTexte("NOM DE RUE", false);
        infos.Add("@rue", Console.ReadLine());

        CentrerTexte("NUMERO DE RUE", false);
        infos.Add("@numero", Console.ReadLine());

        CentrerTexte("CODE POSTAL", false);
        infos.Add("@cp", Console.ReadLine());

        CentrerTexte("VILLE", false);
        infos.Add("@ville", Console.ReadLine());

        CentrerTexte("STATION DE METRO LA PLUS PROCHE", false);
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

            List<Dictionary<string, object>> favoris = connexion.ExecuterRequete(requete, new Dictionary<string, object> { { "@idClient", connexion.IdClient } });

            Console.Clear();
            CentrerTexte("=== VOS PLATS FAVORIS ===");

            if (favoris.Count == 0)
            {
                CentrerTexte("Vous n'avez aucun plat mis en favori");
            }
            else
            {
                foreach (Dictionary<string, object> fav in favoris)
                {
                    Console.WriteLine("\nPlat n° " + fav["id_Plat"].ToString());
                    Console.WriteLine("- Nom: " + fav["Nom"].ToString());
                    Console.WriteLine("- Cuisinier: " + fav["Cuisinier"].ToString());
                    Console.WriteLine("- Prix par personne: " + fav["PrixParPersonne"].ToString() + "€");
                    Console.WriteLine("- Nationalité: " + fav["Nationalite"].ToString());
                    Console.WriteLine("- Régime: " + fav["RegimeAlimentaire"].ToString());
                    Console.WriteLine("- Ajouté le: " + ((DateTime)fav["Date_Ajout"]).ToString("dd/MM/yyyy"));
                    Console.WriteLine("-------------------------------");
                }
            }
            
            CentrerTexte("Appuyez nimporte ou pour continuer");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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
                                    p.DatePeremption > CURRENT_DATE()"; ///plats encore bons

            List<Dictionary<string, object>> plats = connexion.ExecuterRequete(requetePlats);

            if (plats.Count == 0) ///sil n'y a pas de plats (ce qui n'arrivera jamais biensur )
            {
                CentrerTexte("Il n'y a pas de plats dispo pour toi en ce moment....");
                Console.ReadKey();
                return;
            }

            
            CentrerTexte("=====PLATS DISPONIBLES=====");

            foreach (Dictionary<string, object> plat in plats)
            {
                int idPlat = Convert.ToInt32(plat["id_Plat"]);
                string nomPlat = plat["Nom"].ToString();
                string cuisinier = plat["Cuisinier"].ToString();
                decimal prixParPersonne = Convert.ToDecimal(plat["PrixParPersonne"]);
                string nationalite = plat["Nationalite"].ToString();
                string regimeAlimentaire = plat["RegimeAlimentaire"].ToString();

                ///affichage infos
                Console.WriteLine("\nID: " + idPlat.ToString());
                Console.WriteLine("- Nom: " + nomPlat);
                Console.WriteLine("- Cuisinier: " + cuisinier);
                Console.WriteLine("- Prix par personne: " + prixParPersonne + "€");
                Console.WriteLine("- Nationalité: " + nationalite);
                Console.WriteLine("- Régime alimentaire: " + regimeAlimentaire);
                Console.WriteLine("===================================");  ///pour faire beaugosse
            }

            
            CentrerTexte("\nQuel plat voulez vous ajouter aux favoris (ID)", false);
            int idPlatChoisi = int.Parse(Console.ReadLine());
            string requeteVerif = "SELECT COUNT(*) FROM Favoris WHERE id_Client = @idClient AND id_Plat = @idPlat"; ///requete qui verifie si le plat est deja dans favoris
            List<Dictionary<string, object>> verificationFavoris = connexion.ExecuterRequete(requeteVerif,
                new Dictionary<string, object> {
                { "@idClient", connexion.IdClient }, 
                { "@idPlat", idPlatChoisi } 
                });

           
            int existeDeja = Convert.ToInt32(verificationFavoris[0]["COUNT(*)"]);

            if (existeDeja > 0)
            {
                CentrerTexte("Ce plat est déjà dans vos favoris ...");
                Console.ReadKey(); 
                return; 
            }

            
            string requeteAjout = "INSERT INTO Favoris(id_Client, id_Plat) VALUES (@idClient, @idPlat)"; ///requete qui ajoute le plat aux favoris
            connexion.ExecuterNonQuery(requeteAjout,
                new Dictionary<string, object> {
                { "@idClient", connexion.IdClient }, 
                { "@idPlat", idPlatChoisi }
                });

            
            CentrerTexte("Ce plat a bien été ajouté a vos favoris !!");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            
            Console.WriteLine(ex.Message);
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
            CentrerTexte("1 - Voir mes plats");
            CentrerTexte("2 - Commandes à préparer");
            CentrerTexte("3 - Créer un nouveau plat");
            CentrerTexte("4 - Déconnexion");
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

    #region Affichage client commandes en cours et civraisons en cours
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
                                    Contient ct ON c.id_Commande = ct.id_Commande
                         JOIN 
                                    Plat p ON ct.id_Plat = p.id_Plat
                         WHERE c.id_Client = @idClient
                         ORDER BY c.Date_Commande DESC"; ///recup la commande client

            List<Dictionary<string, object>> commandes = connexion.ExecuterRequete(requete,new Dictionary<string, object> { { "@idClient", connexion.IdClient } });


            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            CentrerTexte("=== VOS COMMANDES ===");
            Console.ResetColor();

            ///si aucune commande
            if (commandes.Count == 0)
            {
                CentrerTexte("Vous n'avez aucune commande en cours");
            }
            else
            {
                ///si yen a
                foreach (Dictionary<string, object> cmd in commandes)
                {
                    ///afficher details
                    Console.WriteLine("\nCommande #" + cmd["id_Commande"]);
                    Console.WriteLine("- Plat: " + cmd["NomPlat"]);
                    Console.WriteLine("- Date: " + ((DateTime)cmd["Date_Commande"]).ToString("dd/MM/yyyy"));
                    Console.WriteLine("- Prix: " + cmd["Prix_Commande"] + "€");
                    Console.WriteLine("- Statut: " + cmd["Statut"]);
                    Console.WriteLine(new string('-', 40)); 
                }
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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
                         WHERE c.id_Client = @idClient AND l.Statut = 'En préparation'
                         ORDER BY l.Date_Livraison DESC"; ///requete pour recup les livraisons

            ///execute requète
            List<Dictionary<string, object>> livraisons = connexion.ExecuterRequete(requete,new Dictionary<string, object> { { "@idClient", connexion.IdClient } });

            ///affichage ecran
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            CentrerTexte("=== VOS LIVRAISONS ===");
            Console.ResetColor();

            ///si pas de livraison alors
            if (livraisons.Count == 0)
            {
                CentrerTexte("Aucune livraison en cours.");
            }
            else
            {
                ///si yen a
                foreach (Dictionary<string, object> liv in livraisons)
                {
                    ///afficher details livraison
                    Console.WriteLine("\nLivraison n°" + liv["id_Livraison"]);
                    Console.WriteLine("- Plat: " + liv["NomPlat"]);
                    Console.WriteLine("- Statut: " + liv["Statut"]);

                    
                    if (liv["Date_Livraison"] != DBNull.Value)
                        Console.WriteLine("- Date livraison: " + ((DateTime)liv["Date_Livraison"]).ToString("dd/MM/yyyy HH:mm"));

                    Console.WriteLine("- Adresse: " + liv["Adresse_Livraison"]);
                    Console.WriteLine(new string('-', 40));
                }
            }

            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.ReadKey();
        }
    }

    #endregion

    #region Fonctionnalités des cuisiniers
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
                         ORDER BY NbCommandes DESC"; ///requète pour recup les plats du cuisinier

            ///execute la requète sql
            List<Dictionary<string, object>> plats = connexion.ExecuterRequete(requete,
                new Dictionary<string, object> { { "@idCuisinier", connexion.IdUtilisateur } });

            Console.Clear();
            CentrerTexte("=== VOS PLATS ===");

            ///si aucun plat trouvé
            if (plats.Count == 0)
            {
                CentrerTexte("Vous n'avez aucun plat enregistré");
            }
            else
            {
                ///si yen a 
                foreach (Dictionary<string, object> plat in plats)
                {
                    Console.WriteLine("\nPlat #" + plat["id_Plat"]);
                    Console.WriteLine("- Nom: " + plat["Nom"]);
                    Console.WriteLine("- Type: " + plat["TypePlat"]);
                    Console.WriteLine("- Prix/pers: " + plat["PrixParPersonne"] + "€");
                    Console.WriteLine("- Nationalité: " + plat["Nationalite"]);
                    Console.WriteLine("- Régime: " + plat["RegimeAlimentaire"]);
                    Console.WriteLine("- Fabrication: " + ((DateTime)plat["DateFabrication"]).ToString("dd/MM/yyyy"));
                    Console.WriteLine("- Commandes: " + plat["NbCommandes"]);
                    Console.WriteLine(new string('-', 40)); 
                }
            }

            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
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
                                    c.Date_Commande"; ///requete pour recup les commandes a preparer

            ///éxécute requète sql
            List<Dictionary<string, object>> commandes = connexion.ExecuterRequete(requete,
                new Dictionary<string, object> { { "@idCuisinier", connexion.IdUtilisateur } });

            Console.Clear();
            CentrerTexte("=== COMMANDES A PRÉPARER ===");

            ///si pas de commandes
            if (commandes.Count == 0)
            {
                CentrerTexte("Aucune commande à préparer.");
            }
            else
            {
                ///afficher details
                foreach (Dictionary<string, object> cmd in commandes)
                {
                    Console.WriteLine("\nCommande #" + cmd["id_Commande"]);
                    Console.WriteLine("- Plat: " + cmd["NomPlat"]);
                    Console.WriteLine("- Client: " + cmd["NomClient"]);
                    Console.WriteLine("- Téléphone: " + cmd["Telephone"]);
                    Console.WriteLine("- Date: " + ((DateTime)cmd["Date_Commande"]).ToString("dd/MM/yyyy HH:mm"));
                    Console.WriteLine("- Prix: " + cmd["Prix_Commande"] + "€");
                    Console.WriteLine(new string('-', 40));
                }
            }
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            Console.ReadKey();
        }
    }

    static void CreerPlat(ConnexionBDD connexion)
    {
        try
        {
            Console.Clear();
            CentrerTexte("=== NOUVEAU PLAT ===");

            ///stocker les données dans parametres
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

            ///verif le role pour les droits
            if (connexion.RoleUtilisateur != "Cuisinier")
            {
                CentrerTexte("");
                Console.ReadKey();
                return;
            }

            ///recup id cuisinier
            string reqIdCuisinier = "SELECT id_Cuisinier FROM Cuisinier WHERE id_Utilisateur = @idUtilisateur";
            Dictionary<string, object> paramIdCuisinier = new Dictionary<string, object>
        {
            {"@idUtilisateur", connexion.IdUtilisateur}
        };

            List<Dictionary<string, object>> result = connexion.ExecuterRequete(reqIdCuisinier, paramIdCuisinier);

            ///si pas de cuisinier
            if (result.Count == 0)
            {
                CentrerTexte("Pas de cuisinier trouvé");
                Console.ReadKey();
                return;
            }

            
            int idCuisinier = Convert.ToInt32(result[0]["id_Cuisinier"]);
            parametres.Add("@idCuisinier", idCuisinier);

            string requete = @"INSERT INTO Plat(
                        Nom, 
                        TypePlat, 
                        NbPersonnes, 
                        DateFabrication, 
                        DatePeremption, 
                        PrixParPersonne, 
                        Nationalite, 
                        RegimeAlimentaire, 
                        Ingredients, 
                        id_Cuisinier)
                    VALUES (
                        @nom, 
                        @type, 
                        @nbPersonnes, 
                        @dateFab, 
                        @datePeremp, 
                        @prix, 
                        @nationalite, 
                        @regime, 
                        @ingredients, 
                        @idCuisinier)"; ///requète insérer le nouveau plat

            connexion.ExecuterNonQuery(requete, parametres);
            CentrerTexte("Votre nouveau plat a bien été créé !!");
            Console.ReadKey();
        }

        ///dans le cas d'erreurs (sql, format)
        catch (FormatException)
        {
            CentrerTexte("Vous avez saisi le mauvais format de données");
            Console.ReadKey();
        }
        catch (MySqlException sqlEx)
        {
            CentrerTexte($"Problème SQL ({sqlEx.Number}) : {sqlEx.Message}");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            CentrerTexte($"{ex.Message}");
            Console.ReadKey();
        }
    }

    #endregion

    #region Fonctionnalités Client

    #region 1 - Menu client + commander Menu du Jour
    static void AfficherMenuClient(ConnexionBDD connexion)
    {
        ///affichage menu client
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
                                    m.DateMenu = CURRENT_DATE()"; ///requete qui choppe le menu du jour

            List<Dictionary<string, object>> menus = connexion.ExecuterRequete(requeteMenu); ///execute requète

            ///si aucun menu
            if (menus.Count == 0)
            {
                CentrerTexte("Pas de menu du jour aujourd'hui");
                Console.ReadKey();
                return;
            }

            Dictionary<string, object> menu = menus[0];

            Console.WriteLine($"\nMenu du {((DateTime)menu["DateMenu"]):dd/MM/yyyy}");
            Console.WriteLine($"- Plat principal: {menu["PlatPrincipal"]} ({menu["PrixPrincipal"]}€/pers)");

            if (menu["Dessert"] != DBNull.Value)
                Console.WriteLine($"- Dessert: {menu["Dessert"]} ({menu["PrixDessert"]}€/pers)");
            Console.WriteLine($"- Description: {menu["Description"]}");
            Console.WriteLine(new string('-', 40));

            CentrerTexte("\nNombre de personnes : ", false);
            int nbPersonnes = int.Parse(Console.ReadLine()); ///lit le nombre de personnes pour le plat

            decimal prixTotal = nbPersonnes * (decimal)menu["PrixPrincipal"]; ///prix en decimal !
            if (menu["Dessert"] != DBNull.Value)
                prixTotal += nbPersonnes * (decimal)menu["PrixDessert"]; ///calcul du prix total

            
            string reqCommande = @"INSERT INTO Commande(Date_Commande, Prix_Commande, Statut, id_Client)
                        VALUES (NOW(), @prix, 'En préparation', @idClient);
                        SELECT LAST_INSERT_ID();"; ///requete qui crée la commande

            Dictionary<string, object> paramCommande = new Dictionary<string, object> {
            { "@prix", prixTotal },
            { "@idClient", connexion.IdClient }
        }; ///insère la commande

            int idCommande = connexion.ExecuterInsertEtRetournerId(reqCommande, paramCommande);

            ///ajoute le plat
            string reqPlatPrincipal = @"SELECT id_PlatPrincipal FROM MenuDuJour WHERE id_Menu = @idMenu";
            int idPlatPrincipal = Convert.ToInt32(connexion.ExecuterRequete(reqPlatPrincipal,
                new Dictionary<string, object> { { "@idMenu", menu["id_Menu"] } })[0]["id_PlatPrincipal"]);

            string reqContientPrincipal = @"INSERT INTO Contient(id_Commande,
                                                                id_Plat,
                                                                Quantite)
                                            VALUES (@idCommande,
                                                    @idPlat,
                                                    @quantite)";

            Dictionary<string, object> paramContientPrincipal = new Dictionary<string, object> {
            { "@idCommande", idCommande },
            { "@idPlat", idPlatPrincipal },
            { "@quantite", nbPersonnes }
        };

            connexion.ExecuterNonQuery(reqContientPrincipal, paramContientPrincipal);

            ///ajoute le dessert
            if (menu["Dessert"] != DBNull.Value)
            {
                string reqDessert = @"SELECT id_Dessert FROM MenuDuJour WHERE id_Menu = @idMenu";
                int idDessert = Convert.ToInt32(connexion.ExecuterRequete(reqDessert,
                    new Dictionary<string, object> { { "@idMenu", menu["id_Menu"] } })[0]["id_Dessert"]);

                string reqContientDessert = @"INSERT INTO Contient(id_Commande, id_Plat, Quantite)
                                  VALUES (@idCommande, @idPlat, @quantite)";

                Dictionary<string, object> paramContientDessert = new Dictionary<string, object> {
                { "@idCommande", idCommande },
                { "@idPlat", idDessert },
                { "@quantite", nbPersonnes }
            };

                connexion.ExecuterNonQuery(reqContientDessert, paramContientDessert);
            }

            string reqLivraison = @"INSERT INTO Livraison(Statut,
                                                        Date_Livraison,
                                                        Adresse_Livraison,
                                                        id_Commande)
                      VALUES ('En préparation', NULL, (SELECT CONCAT(Rue, ' ', Numero_Adresse, ', ', Code_Postal, ' ', Ville) 
                       FROM Utilisateur WHERE id_Utilisateur = @idUser), @idCommande)"; ///requète qui crée la livraison

            Dictionary<string, object> paramLivraison = new Dictionary<string, object> {
            { "@idUser", connexion.IdUtilisateur },
            { "@idCommande", idCommande }
        };

            connexion.ExecuterNonQuery(reqLivraison, paramLivraison);

            CentrerTexte($"\nCommande n°{idCommande} passée !");
            CentrerTexte($"Prix total: {prixTotal}€"); ///prix total en euro
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
            Console.ReadKey();
        }
    }
    #endregion

    #region 2 - Fonction CommanderPlat
    static void CommanderPlat(ConnexionBDD connexion)
    {
        try
        {
            Console.Clear();
            CentrerTexte("=== COMMANDER UN PLAT ===");

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
                          WHERE p.DatePeremption > CURRENT_DATE()"; ///requete qui recup les plats dispo et non périmé (> current date)

            List<Dictionary<string, object>> plats = connexion.ExecuterRequete(requetePlats);

            ///si aucun plat
            if (plats.Count == 0)
            {
                CentrerTexte("Il n'y a aucun plat de disponible pour le moment");
                Console.ReadKey();
                return;
            }

            ///sinon on les affiche
            CentrerTexte("=====PLATS DISPONIBLES=====");
            foreach (Dictionary<string, object> plat in plats)
            {
                Console.WriteLine($"\nID: {plat["id_Plat"]}");
                Console.WriteLine($"- Nom: {plat["Nom"]}");
                Console.WriteLine($"- Cuisinier: {plat["Cuisinier"]}");
                Console.WriteLine($"- Prix/pers: {plat["PrixParPersonne"]}€");
                Console.WriteLine($"- Nationalité: {plat["Nationalite"]}");
                Console.WriteLine($"- Régime: {plat["RegimeAlimentaire"]}");
                Console.WriteLine(new string('-', 40));
            }

            CentrerTexte("\nQuel est le super plat que vous allez commander ? (ID) ", false); ///demande quel plat
            int idPlat = int.Parse(Console.ReadLine());

            ///combien de plats
            CentrerTexte("Pour combien de personnes ?", false);
            int nbPersonnes = int.Parse(Console.ReadLine());


            Dictionary<string, object> platSelectionne = plats.Find(p => (int)p["id_Plat"] == idPlat); ///methode de trouver si le plat existe (.find)
            if (platSelectionne == null)
            {
                CentrerTexte("Le plat que vous avez sélétionné n'existe pas");
                Console.ReadKey();
                return;
            }

            ///calcul prix total (en decimal)
            decimal prixTotal = nbPersonnes * (decimal)platSelectionne["PrixParPersonne"];
            string reqCommande = @"INSERT INTO Commande(Date_Commande, Prix_Commande, Statut, id_Client)
                            VALUES (NOW(), @prix, 'En préparation', @idClient);
                            SELECT LAST_INSERT_ID()";

            int idCommande = connexion.ExecuterInsertEtRetournerId(reqCommande,
                new Dictionary<string, object> {
                {"@prix", prixTotal},
                {"@idClient", connexion.IdClient}
                });

            ///requete qui ajoute le plat a la commande
            string reqContient = @"INSERT INTO Contient(id_Commande, id_Plat, Quantite)
                               VALUES (@idCommande, @idPlat, @quantite)";

            connexion.ExecuterNonQuery(reqContient,
                new Dictionary<string, object> {
                {"@idCommande", idCommande},
                {"@idPlat", idPlat},
                {"@quantite", nbPersonnes}
                });

            ///requete qui ajoute pour la livraison
            string reqLivraison = @"INSERT INTO Livraison(Statut, Date_Livraison, Adresse_Livraison, id_Commande)
                          VALUES ('En préparation', NULL, 
                                  (SELECT CONCAT(Rue, ' ', Numero_Adresse, ', ', Code_Postal, ' ', Ville) 
                                   FROM Utilisateur WHERE id_Utilisateur = @idUser), @idCommande)";

            connexion.ExecuterNonQuery(reqLivraison,
                new Dictionary<string, object> {
                {"@idUser", connexion.IdUtilisateur},
                {"@idCommande", idCommande}
                });

            CentrerTexte($"\nCommande n°{idCommande} a été passée !");
            CentrerTexte($"Prix total: {prixTotal}€");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
            Console.ReadKey();
        }
    }
    #endregion

    #region 3 - Fonction DonnerAvis
    static void DonnerAvis(ConnexionBDD connexion)
    {
        try
        {
            Console.Clear();
            CentrerTexte("=== DONNER UN AVIS ===");

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
                               AND av.id_Avis IS NULL"; ///requete un peu chaude pour récup les commandes livrées mais qui ont pas d'avis seulement

            List<Dictionary<string, object>> commandes = connexion.ExecuterRequete(requeteCommandes,
                new Dictionary<string, object> { { "@idClient", connexion.IdUtilisateur } });

            ///si aucune commande
            if (commandes.Count == 0)
            {
                CentrerTexte("Il n'y a aucune commande livrée sans avis");
                Console.ReadKey();
                return;
            }

            ///les autres
            CentrerTexte("Voici les commandes notables :");
            foreach (Dictionary<string, object> cmd in commandes)
            {
                Console.WriteLine($"\nCommande #{cmd["id_Commande"]}");
                Console.WriteLine($"- Plat: {cmd["NomPlat"]}");
                Console.WriteLine($"- Cuisinier: {cmd["Cuisinier"]}");
                Console.WriteLine($"- Date livraison: {((DateTime)cmd["Date_Livraison"]):dd/MM/yyyy}");
                Console.WriteLine(new string('-', 40));
            }

            CentrerTexte("Quelles est le plat que vous voulez noter (ID)", false);
            int idPlat = int.Parse(Console.ReadLine());

            ///verif avec .find que cette commande existe
            Dictionary<string, object> platSelectionne = commandes.Find(c => (int)c["id_Plat"] == idPlat);
            if (platSelectionne == null)
            {
                CentrerTexte("Ce plat n'existe pas");
                Console.ReadKey();
                return;
            }
            ///demande de note
            CentrerTexte("Note (0-5) : ", false);
            decimal note = decimal.Parse(Console.ReadLine());
            if (note < 0 || note > 5)
            {
                CentrerTexte("La note doit être entre 0 et 5");
                Console.ReadKey();
                return;
            }

            ///demande de commentaire
            CentrerTexte("Commentaire (facultatif) : ", false);
            string commentaire = Console.ReadLine();

            /// Enregistrer l'avis dans la base de données
            string reqAvis = @"INSERT INTO Avis_Client(Note,
                                                        Commentaire,
                                                        Date_Avis,
                                                        id_Client,
                                                        id_Plat)
                                VALUES (@note,
                                    @commentaire,
                                    NOW(),
                                    @idClient,
                                    @idPlat)"; ///requète qui met le commentaire dans la bdd

            connexion.ExecuterNonQuery(reqAvis,new Dictionary<string, object> {
                {"@note", note},
                {"@commentaire", string.IsNullOrEmpty(commentaire) ? DBNull.Value : commentaire},
                {"@idClient", connexion.IdUtilisateur},
                {"@idPlat", idPlat}
                });

            Console.WriteLine();
            CentrerTexte("Merci pour votre avis !");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
            Console.ReadKey();
        }
    }
    #endregion

    #endregion

}