using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using RENDU_3;
using SkiaSharp;
namespace RENDU_3
{
    public static class MetroGraph
    {
        static List<int[]> liaisonStations = new List<int[]>();
        static List<string> nomsStations = new List<string>();
        static int[,] matrice;
        static Dictionary<int, (float x, float y)> positionsStations = new Dictionary<int, (float x, float y)>();
        static Dictionary<int, int> idVersIndice = new Dictionary<int, int>();
        /// <summary>
        /// Dans cette méthode on charge les données, calcule les chemins et on génère les graphes.
        /// </summary>
        public static void Lancer()
        {
            string fichierLiaisons = "fichierlignemetro.csv";
            string fichierCoordonnees = "longitude_et_latitude.csv";
            LireFichierCSV(fichierLiaisons);
            CreerMatrice();
            LireCoordonneesCSV(fichierCoordonnees);
            Console.Write("Station de départ : ");
            string depart = Console.ReadLine();
            Console.Write("Station de arrivée : ");
            string arrive = Console.ReadLine();
            TrouverTempsEntreStations(depart, arrive);
            int indexDepart = -1;
            int indexArrivee = -1;
            for (int i = 0; i < nomsStations.Count; i++)
            {
                if (nomsStations[i] == depart)
                    indexDepart = i;

                if (nomsStations[i] == arrive)
                    indexArrivee = i;

                if (indexDepart != -1 && indexArrivee != -1)
                    break;
            }
            List<int> chemin = DijkstraChemin(indexDepart, indexArrivee);
            DessinerGrapheGeographique("graphe_metro.png");
            DessinerCheminSurGraphe("graphe_chemin.png", chemin);
            int[] couleurs = ColorationGraphe.ColorerAvecWelshPowell(matrice, nomsStations);
            bool estBip = ColorationGraphe.EstBiparti(matrice, nomsStations);
        }
        public static void LireFichierCSV(string chemin)
        {
            StreamReader lecteur = new StreamReader(chemin);
            string ligne = lecteur.ReadLine();
            while ((ligne = lecteur.ReadLine()) != null)
            {
                string[] morceaux = ligne.Split(';');
                if (morceaux.Length < 5) continue;
                int numStation = Convert.ToInt32(morceaux[0]);
                string nom = morceaux[1];
                int precedent = Convert.ToInt32(morceaux[2]);
                int suivant = Convert.ToInt32(morceaux[3]);
                int temps = Convert.ToInt32(morceaux[4]);
                int index = nomsStations.Count;
                liaisonStations.Add(new int[] { numStation, precedent, suivant, temps });
                nomsStations.Add(nom);
                idVersIndice[numStation] = nomsStations.Count - 1; ///je prends l’ID de la station (par exemple 2038) et je l’associe à sa position dans la liste nomsStations qui correspond à son index dans la matrice
            }
            lecteur.Close();
        }
        public static void CreerMatrice()
        {
            int n = nomsStations.Count;
            matrice = new int[n, n];
            ///on initialise la matrice avec des temps par défaut : 0 sur la diagonale, 999 sinon
            ///(999 représente l'absence de liaison entre deux stations)
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        matrice[i, j] = 0;
                    }
                    else
                    {
                        matrice[i, j] = 999; ///jai pris 999 pour éviter de prendre un nombre qui peut tomber
                    }
                }
            }
            for (int i = 0; i < liaisonStations.Count; i++)
            {
                int id = liaisonStations[i][0];
                int prec = liaisonStations[i][1];
                int suiv = liaisonStations[i][2];
                int temps = liaisonStations[i][3];
                if (!idVersIndice.ContainsKey(id)) continue;
                int numIndex = idVersIndice[id];
                if (prec != -1 && idVersIndice.ContainsKey(prec) && temps != -1)
                {
                    int precIndex = idVersIndice[prec];
                    matrice[precIndex, numIndex] = temps;
                    matrice[numIndex, precIndex] = temps;
                }
                if (suiv != -1 && idVersIndice.ContainsKey(suiv) && temps != -1)
                {
                    int suivIndex = idVersIndice[suiv];
                    matrice[suivIndex, numIndex] = temps;
                    matrice[numIndex, suivIndex] = temps;
                }

            }
            for (int i = 0; i < nomsStations.Count; i++)
            {
                for (int j = i + 1; j < nomsStations.Count; j++)
                {
                    if (nomsStations[i] == nomsStations[j])
                    {
                        matrice[i, j] = 4; ///ici cest le temps de changement entre deux ligne au final je met 4 tt le temps par exemple si je suis sur la ligne 6 Bercy et que je veux utiliser la ligne 14 qui part de Bercy et bien le temps de changement entre les deux ligne de la station Bercy est de 4min
                        matrice[j, i] = 4;
                    }
                }
            }
        }
        public static void LireCoordonneesCSV(string cheminCSV)
        {
            List<float> longitude = new List<float>();
            List<float> latitude = new List<float>();
            using (StreamReader sr = new StreamReader(cheminCSV))
            {
                string ligne = sr.ReadLine();

                while ((ligne = sr.ReadLine()) != null)
                {
                    string[] morceaux = ligne.Split(';');

                    int id = int.Parse(morceaux[0]);
                    float lon = float.Parse(morceaux[1], CultureInfo.InvariantCulture);  ///IA m'a dit de faire CultureInfo.InvariantCulture pour garantir le . comme séparateur décimal car j'arrivais pas autrement
                    float lat = float.Parse(morceaux[2], CultureInfo.InvariantCulture);  ///pareil
                    longitude.Add(lon);
                    latitude.Add(lat);
                    positionsStations[id] = (lon, lat); ///Ca enregistre les coordonnées GPS de la station dans le dictionnaire
                }
            }
            float minLongitude = longitude.Min();
            float maxLongitude = longitude.Max();
            float minLatitude = latitude.Min();
            float maxLatitude = latitude.Max();
            int largeur = 6000;
            int hauteur = 6000;
            int marge = 200;
            List<int> cles = new List<int>(positionsStations.Keys);
            for (int i = 0; i < cles.Count; i++) ///parcourt toutes les stations pour convertir leurs coordonnées
            {
                int id = cles[i];
                float lon = positionsStations[id].x;
                float lat = positionsStations[id].y;
                float x = marge + (lon - minLongitude) / (maxLongitude - minLongitude) * (largeur - 2 * marge);
                float y = marge + (1 - (lat - minLatitude) / (maxLatitude - minLatitude)) * (hauteur - 2 * marge);
                positionsStations[id] = (x, y);
            }
        }
        public static int BellmanFord(int depart, int arrivee)
        {
            int n = nomsStations.Count;
            int[] distance = new int[n];
            ///on initialise toutes les distances à l'infinies sauf celle de départ
            for (int i = 0; i < n; i++)
                distance[i] = int.MaxValue;
            distance[depart] = 0;
            for (int k = 0; k < n - 1; k++)
            {
                for (int u = 0; u < n; u++)
                {
                    for (int v = 0; v < n; v++)
                    {
                        ///si une liaison existe et que le chemin via u est plus court on doit mettre à jour
                        if (matrice[u, v] != 999 && distance[u] != int.MaxValue && distance[u] + matrice[u, v] < distance[v])
                        {
                            distance[v] = distance[u] + matrice[u, v];
                        }
                    }
                }
            }
            return distance[arrivee];
        }
        public static int Dijkstra(int depart, int arrivee)
        {
            int n = nomsStations.Count;
            int[] distance = new int[n];
            bool[] visite = new bool[n];
            ///On initialise toutes les distances à l'infinies sauf celle de départ
            for (int i = 0; i < n; i++)
                distance[i] = int.MaxValue;
            distance[depart] = 0;
            for (int i = 0; i < n; i++)
            {
                ///on cherche la station non visitée avec la plus petite distance
                int u = -1;
                for (int j = 0; j < n; j++)
                {
                    if (!visite[j] && (u == -1 || distance[j] < distance[u]))
                        u = j;
                }
                ///si la distance est toujours infinie c’est qu’on ne peut plus avancer
                if (distance[u] == int.MaxValue)
                    break;
                visite[u] = true;
                ///mise à jour des voisins de u si un chemin plus court est trouvé
                for (int v = 0; v < n; v++)
                {
                    if (matrice[u, v] != 999 && distance[u] + matrice[u, v] < distance[v])
                    {
                        distance[v] = distance[u] + matrice[u, v];
                    }
                }
            }
            return distance[arrivee];
        }
        public static List<int> DijkstraChemin(int depart, int arrivee)
        {
            int n = nomsStations.Count;
            int[] distance = new int[n];
            int[] precedent = new int[n];///pour reconstruire le chemin
            bool[] visite = new bool[n];
            ///initialisation
            for (int i = 0; i < n; i++)
            {
                distance[i] = int.MaxValue;
                precedent[i] = -1;
            }
            distance[depart] = 0;
            for (int i = 0; i < n; i++)
            {
                int u = -1;
                for (int j = 0; j < n; j++)
                {
                    if (!visite[j] && (u == -1 || distance[j] < distance[u]))
                        u = j;
                }
                if (distance[u] == int.MaxValue)
                    break;
                visite[u] = true;
                for (int v = 0; v < n; v++)
                {
                    if (matrice[u, v] != 999 && distance[u] + matrice[u, v] < distance[v])
                    {
                        distance[v] = distance[u] + matrice[u, v];
                        precedent[v] = u;
                        ///on enregistre par où on est passé
                    }
                }
            }
            ///on reconstruit le chemin de l’arrivée vers le départ
            List<int> chemin = new List<int>();
            for (int v = arrivee; v != -1; v = precedent[v])
                chemin.Insert(0, v); ///on insert en début de liste pour reconstituer dans l’ordre
            return chemin;
        }
        public static int[,] FloydWarshall()
        {
            int n = nomsStations.Count;
            ///On travaille sur une copie de la matrice originale
            int[,] dist = (int[,])matrice.Clone();
            ///avec cette boucle triple on regarde si passer par un sommet k permet de raccourcir le chemin de i à j
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (dist[i, k] != 999 && dist[k, j] != 999 && dist[i, k] + dist[k, j] < dist[i, j])
                        {
                            dist[i, j] = dist[i, k] + dist[k, j];
                        }
                    }
                }
            }
            return dist;///matrice complète des plus courts chemins
        }
        public static void TrouverTempsEntreStations(string nomDepart, string nomArrivee)
        {
            int indexDepart = -1;
            int indexArrivee = -1;
            ///on recherche manuellement des indices des stations dans la liste
            for (int i = 0; i < nomsStations.Count; i++)
            {
                if (nomsStations[i] == nomDepart)
                    indexDepart = i;
                if (nomsStations[i] == nomArrivee)
                    indexArrivee = i;
                ///si les deux indices sont trouvés, on peut arrêter la boucle
                if (indexDepart != -1 && indexArrivee != -1)
                    break;
            }
            ///on regarde qu'on a bien trouvé les deux stations
            if (indexDepart == -1 || indexArrivee == -1)
            {
                Console.WriteLine("Nom de station invalide.");
                return;
            }
            int tempsDijkstra = Dijkstra(indexDepart, indexArrivee);
            int tempsBellman = BellmanFord(indexDepart, indexArrivee);
            int[,] allPairs = FloydWarshall();
            int tempsFloyd = allPairs[indexDepart, indexArrivee];
            Console.WriteLine($"Temps entre {nomDepart} et {nomArrivee} :");
            Console.WriteLine($" - Dijkstra       : {tempsDijkstra} min");
            Console.WriteLine($" - Bellman-Ford   : {tempsBellman} min");
            Console.WriteLine($" - Floyd-Warshall : {tempsFloyd} min");
            List<int> chemin = DijkstraChemin(indexDepart, indexArrivee);
            int tempsTotal = 0;
            int tempsChangement = 0;
            List<string> stationsChangement = new List<string>();
            for (int i = 0; i < chemin.Count - 1; i++)
            {
                int u = chemin[i];
                int v = chemin[i + 1];
                int t = matrice[u, v];
                tempsTotal += t;

                if (t == 4 && nomsStations[u] == nomsStations[v])
                {
                    tempsChangement += 4;
                    if (!stationsChangement.Contains(nomsStations[u]))
                        stationsChangement.Add(nomsStations[u]);
                }
            }
            Console.WriteLine($"\nChemin (selon Dijkstra) :");
            List<string> nomsChemin = new List<string>();
            for (int j = 0; j < chemin.Count; j++)
            {
                nomsChemin.Add(nomsStations[chemin[j]]);
            }
            Console.WriteLine(string.Join(" -> ", nomsChemin));
            Console.WriteLine($"\nTemps total avec correspondances : {tempsTotal} minutes.");
            if (tempsChangement > 0)
            {
                Console.WriteLine($"Dont {tempsChangement} minutes de changement (en tout) de ligne à : {string.Join(", ", stationsChangement)}.");
            }
            else
            {
                Console.WriteLine("Aucun changement de ligne.");
            }
        }
        /// <summary>
        /// Ici cest l'IA générative comme dit dans le sujet : "Les prompts issus des IA génératives concernant la visualisation du graphe (si besoin)".
        /// </summary>
        /// <param name="cheminImage"></param>
        public static void DessinerGrapheGeographique(string cheminImage)
        {
            int largeur = 6000;
            int hauteur = 6000;
            int rayon = 25;
            SKBitmap bitmap = new SKBitmap(largeur, hauteur);
            SKCanvas canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);
            SKPaint paintLigne = new SKPaint { Color = SKColors.Gray, StrokeWidth = 1, IsAntialias = true };
            SKPaint paintSommet = new SKPaint { Color = SKColors.DarkBlue, IsAntialias = true };
            SKPaint paintTexte = new SKPaint { Color = SKColors.White, TextSize = 30, TextAlign = SKTextAlign.Center, IsAntialias = true };
            SKPaint paintNom = new SKPaint { Color = SKColors.Black, TextSize = 28, TextAlign = SKTextAlign.Center };
            SKPaint paintValeurLien = new SKPaint { Color = SKColors.DarkSlateGray, TextSize = 26, TextAlign = SKTextAlign.Center, IsAntialias = true };
            int n = nomsStations.Count;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (matrice[i, j] != 999 && matrice[i, j] != 0)
                    {
                        if (positionsStations.ContainsKey(i + 1) && positionsStations.ContainsKey(j + 1))
                        {
                            SKPoint p1 = new SKPoint(positionsStations[i + 1].x, positionsStations[i + 1].y);
                            SKPoint p2 = new SKPoint(positionsStations[j + 1].x, positionsStations[j + 1].y);
                            canvas.DrawLine(p1, p2, paintLigne);
                            float mx = (p1.X + p2.X) / 2;
                            float my = (p1.Y + p2.Y) / 2;
                            canvas.DrawText(matrice[i, j].ToString(), mx, my, paintValeurLien);
                        }
                    }
                }
            }
            for (int i = 0; i < n; i++)
            {
                int id = i + 1;
                if (positionsStations.ContainsKey(id))
                {
                    float x = positionsStations[id].x;
                    float y = positionsStations[id].y;
                    SKPoint pos = new SKPoint(x, y);
                    canvas.DrawCircle(pos, rayon, paintSommet);
                    canvas.DrawText(nomsStations[i], x, y + 50, paintNom);
                }
            }
            using (SKImage image = SKImage.FromBitmap(bitmap))
            using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
            {
                File.WriteAllBytes(cheminImage, data.ToArray());
            }
            Console.WriteLine();
            Console.WriteLine("Carte du métro générée : " + cheminImage);
            Console.WriteLine();
        }
        public static void DessinerCheminSurGraphe(string cheminImage, List<int> chemin)
        {
            int largeur = 6000;
            int hauteur = 6000;
            int rayon = 25;
            SKBitmap bitmap = new SKBitmap(largeur, hauteur);
            SKCanvas canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            // Couleurs
            SKPaint paintLigne = new SKPaint { Color = SKColors.Gray, StrokeWidth = 1, IsAntialias = true };
            SKPaint paintChemin = new SKPaint { Color = SKColors.Red, StrokeWidth = 5, IsAntialias = true };
            SKPaint paintSommet = new SKPaint { Color = SKColors.DarkBlue, IsAntialias = true };
            SKPaint paintTexte = new SKPaint { Color = SKColors.White, TextSize = 30, TextAlign = SKTextAlign.Center };
            SKPaint paintNom = new SKPaint { Color = SKColors.Black, TextSize = 28, TextAlign = SKTextAlign.Center };

            // Lignes normales
            int n = nomsStations.Count;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (matrice[i, j] != 999 && matrice[i, j] != 0 && positionsStations.ContainsKey(i + 1) && positionsStations.ContainsKey(j + 1))
                    {
                        SKPoint p1 = new SKPoint(positionsStations[i + 1].x, positionsStations[i + 1].y);
                        SKPoint p2 = new SKPoint(positionsStations[j + 1].x, positionsStations[j + 1].y);
                        canvas.DrawLine(p1, p2, paintLigne);
                    }
                }
            }

            // Chemin rouge
            for (int i = 0; i < chemin.Count - 1; i++)
            {
                int id1 = chemin[i] + 1;
                int id2 = chemin[i + 1] + 1;

                if (positionsStations.ContainsKey(id1) && positionsStations.ContainsKey(id2))
                {
                    SKPoint p1 = new SKPoint(positionsStations[id1].x, positionsStations[id1].y);
                    SKPoint p2 = new SKPoint(positionsStations[id2].x, positionsStations[id2].y);
                    canvas.DrawLine(p1, p2, paintChemin);
                }
            }

            // Sommets
            for (int i = 0; i < n; i++)
            {
                int id = i + 1;
                if (positionsStations.ContainsKey(id))
                {
                    float x = positionsStations[id].x;
                    float y = positionsStations[id].y;
                    canvas.DrawCircle(new SKPoint(x, y), rayon, paintSommet);
                    canvas.DrawText(nomsStations[i], x, y + 50, paintNom);
                }
            }

            using (SKImage image = SKImage.FromBitmap(bitmap))
            using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
            {
                File.WriteAllBytes(cheminImage, data.ToArray());
            }
            Console.WriteLine();
            Console.WriteLine("Carte avec le chemin générée : " + cheminImage);
        }
    }
}
