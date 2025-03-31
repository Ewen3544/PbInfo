using System;
using System.Collections.Generic;
using System.IO;
using SkiaSharp;
namespace PbInfo
{
    public class Graphe<T>
    {
        private Dictionary<int, List<Arc>> listeAdjacence;
        private int[,] matriceAdjacence;
        private int nbSommets;
        private Dictionary<int, Noeud<T>> noeuds;

        public class Arc
        {
            public int Destination { get; set; }
            public double Poids { get; set; }
            public string LigneMetro { get; set; }
            
            public Arc(int destination, double poids, string ligneMetro)
            {
                Destination = destination;
                Poids = poids;
                LigneMetro = ligneMetro;
            }
        }

        public Graphe(List<Noeud<T>> noeuds)
        {
            this.nbSommets = noeuds.Count;
            this.noeuds = new Dictionary<int, Noeud<T>>();
            listeAdjacence = new Dictionary<int, List<Arc>>();
            
            foreach (var noeud in noeuds)
            {
                this.noeuds[noeud.Id] = noeud;
                listeAdjacence[noeud.Id] = new List<Arc>();
            }
            
            matriceAdjacence = new int[nbSommets, nbSommets];
        }

        public void AjouterArc(int source, int destination, double poids, string ligneMetro, bool bidirectionnel = false)
        {
            listeAdjacence[source].Add(new Arc(destination, poids, ligneMetro));
            matriceAdjacence[source - 1, destination - 1] = 1;
            
            if (bidirectionnel)
            {
                listeAdjacence[destination].Add(new Arc(source, poids, ligneMetro));
                matriceAdjacence[destination - 1, source - 1] = 1;
            }
        }

       // Dans la classe Graphe<T>

public (List<int> chemin, double distance) Dijkstra(int depart, int arrivee)
{
    var distances = new Dictionary<int, double>();
    var precedents = new Dictionary<int, int>();
    var noeudsNonVisites = new HashSet<int>();
    
    foreach (var noeud in noeuds.Keys)
    {
        distances[noeud] = double.MaxValue;
        precedents[noeud] = -1;
        noeudsNonVisites.Add(noeud);
    }
    
    distances[depart] = 0;
    
    while (noeudsNonVisites.Count > 0)
    {
        int courant = -1;
        double distanceMin = double.MaxValue;
        
        foreach (var noeud in noeudsNonVisites)
        {
            if (distances[noeud] < distanceMin)
            {
                distanceMin = distances[noeud];
                courant = noeud;
            }
        }
        
        if (courant == -1 || courant == arrivee)
            break;
            
        noeudsNonVisites.Remove(courant);
        
        foreach (var arc in listeAdjacence[courant])
        {
            double distanceAlternative = distances[courant] + arc.Poids;
            if (distanceAlternative < distances[arc.Destination])
            {
                distances[arc.Destination] = distanceAlternative;
                precedents[arc.Destination] = courant;
            }
        }
    }
    
    var chemin = new List<int>();
    int noeudCourant = arrivee;
    
    while (noeudCourant != -1 && noeudCourant != depart)
    {
        chemin.Insert(0, noeudCourant);
        noeudCourant = precedents.ContainsKey(noeudCourant) ? precedents[noeudCourant] : -1;
        
        if (noeudCourant == -1)
            return (new List<int>(), double.MaxValue);
    }
    
    chemin.Insert(0, depart);
    return (chemin, distances[arrivee]);
}

public (List<int> chemin, double distance) BellmanFord(int depart, int arrivee)
{
    var distances = new Dictionary<int, double>();
    var precedents = new Dictionary<int, int>();
    
    foreach (var noeud in noeuds.Keys)
    {
        distances[noeud] = double.MaxValue;
        precedents[noeud] = -1;
    }
    
    distances[depart] = 0;
    
    for (int i = 1; i < noeuds.Count; i++)
    {
        foreach (var u in noeuds.Keys)
        {
            foreach (var arc in listeAdjacence[u])
            {
                int v = arc.Destination;
                double poids = arc.Poids;
                
                if (distances[u] != double.MaxValue && distances[u] + poids < distances[v])
                {
                    distances[v] = distances[u] + poids;
                    precedents[v] = u;
                }
            }
        }
    }
    
    foreach (var u in noeuds.Keys)
    {
        foreach (var arc in listeAdjacence[u])
        {
            int v = arc.Destination;
            double poids = arc.Poids;
            
            if (distances[u] != double.MaxValue && distances[u] + poids < distances[v])
            {
                throw new InvalidOperationException("Le graphe contient un cycle de poids négatif");
            }
        }
    }
    
    var chemin = new List<int>();
    int noeudCourant = arrivee;
    
    while (noeudCourant != -1 && noeudCourant != depart)
    {
        chemin.Insert(0, noeudCourant);
        noeudCourant = precedents.ContainsKey(noeudCourant) ? precedents[noeudCourant] : -1;
        
        if (noeudCourant == -1)
            return (new List<int>(), double.MaxValue);
    }
    
    chemin.Insert(0, depart);
    return (chemin, distances[arrivee]);
}

public (List<int> chemin, double distance) FloydWarshall(int depart, int arrivee)
{
    int n = noeuds.Count;
    double[,] dist = new double[n, n];
    int[,] next = new int[n, n];
    
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            dist[i, j] = double.MaxValue;
            next[i, j] = -1;
        }
        dist[i, i] = 0;
    }
    
    foreach (var u in noeuds.Keys)
    {
        foreach (var arc in listeAdjacence[u])
        {
            int v = arc.Destination;
            dist[u-1, v-1] = arc.Poids;
            next[u-1, v-1] = v-1;
        }
    }
    
    for (int k = 0; k < n; k++)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (dist[i, k] != double.MaxValue && 
                    dist[k, j] != double.MaxValue && 
                    dist[i, j] > dist[i, k] + dist[k, j])
                {
                    dist[i, j] = dist[i, k] + dist[k, j];
                    next[i, j] = next[i, k];
                }
            }
        }
    }
    
    for (int i = 0; i < n; i++)
    {
        if (dist[i, i] < 0)
        {
            throw new InvalidOperationException("Le graphe contient un cycle de poids négatif");
        }
    }
    
    if (next[depart-1, arrivee-1] == -1)
    {
        return (new List<int>(), double.MaxValue);
    }
    
    var chemin = new List<int>();
    chemin.Add(depart);
    
    int u = depart-1;
    int v = arrivee-1;
    
    while (u != v)
    {
        u = next[u, v];
        chemin.Add(u+1);
    }
    
    return (chemin, dist[depart-1, arrivee-1]);
}
    }
}
