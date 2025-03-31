using System;
using System.Collections.Generic;
using System.IO;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PbInfo
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Charger les données du métro
            string cheminFichier = @"C:\chemin\vers\MetroParis.xlsx";
            var graphe = MetroParisLoader.ChargerDonnees(cheminFichier);

            // Exemple : Trouver le chemin le plus court entre Porte Maillot (1) et Nation (141)
            int depart = 1;   // Porte Maillot
            int arrivee = 141; // Nation

            Console.WriteLine("Calcul du chemin le plus court...");
            Console.WriteLine($"De: {graphe.GetNoeud(depart)}");
            Console.WriteLine($"À: {graphe.GetNoeud(arrivee)}");
            Console.WriteLine();

            // Dijkstra
            var (cheminDijkstra, tempsDijkstra) = graphe.Dijkstra(depart, arrivee);
            AfficherChemin(cheminDijkstra, graphe, tempsDijkstra, "Dijkstra");

            // Bellman-Ford
            var (cheminBellman, tempsBellman) = graphe.BellmanFord(depart, arrivee);
            AfficherChemin(cheminBellman, graphe, tempsBellman, "Bellman-Ford");

            // Floyd-Warshall
            var (cheminFloyd, tempsFloyd) = graphe.FloydWarshall(depart, arrivee);
            AfficherChemin(cheminFloyd, graphe, tempsFloyd, "Floyd-Warshall");

            // Visualiser le graphe
            graphe.VisualiserGraphe("metro_paris.png");
        }

        static void AfficherChemin(List<int> chemin, Graphe<string> graphe, double temps, string algorithme)
        {
            Console.WriteLine($"=== Résultat avec {algorithme} ===");
            
            if (chemin.Count == 0)
            {
                Console.WriteLine("Aucun chemin trouvé");
                return;
            }
            
            Console.WriteLine($"Temps total: {temps} minutes");
            Console.WriteLine("Itinéraire:");
            
            string ligneActuelle = null;
            
            for (int i = 0; i < chemin.Count; i++)
            {
                var station = graphe.GetNoeud(chemin[i]);
                
                if (i == 0)
                {
                    ligneActuelle = station.Ligne;
                    Console.WriteLine($"Prendre la ligne {ligneActuelle} à {station.Nom}");
                }
                else
                {
                    var stationPrecedente = graphe.GetNoeud(chemin[i-1]);
                    var arcs = graphe.GetListeAdjacence()[chemin[i-1]];
                    var arc = arcs.FirstOrDefault(a => a.Destination == chemin[i]);
                    
                    if (arc.Ligne != ligneActuelle && !arc.EstCorrespondance)
                    {
                        Console.WriteLine($"Changer à {stationPrecedente.Nom} pour la ligne {arc.Ligne}");
                        ligneActuelle = arc.Ligne;
                    }
                    
                    if (arc.EstCorrespondance)
                    {
                        Console.WriteLine($"Correspondance à {stationPrecedente.Nom} (temps: {arc.Poids} minutes)");
                    }
                    
                    Console.WriteLine($"- {station.Nom} (Ligne {station.Ligne})");
                }
            }
            
            Console.WriteLine();
        }
    }
}
