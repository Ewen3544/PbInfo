using System;
using System.Collections.Generic;
using System.IO;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkiaSharp;

namespace PbInfo
{
    public class Graphe<T>
    {
        private Dictionary<int, List<Arc>> listeAdjacence;
        private Dictionary<int, Noeud<T>> noeuds;
        private int nbSommets;

        public Graphe()
        {
            listeAdjacence = new Dictionary<int, List<Arc>>();
            noeuds = new Dictionary<int, Noeud<T>>();
            nbSommets = 0;
        }

        public void AjouterNoeud(Noeud<T> noeud)
        {
            noeuds[noeud.Id] = noeud;
            listeAdjacence[noeud.Id] = new List<Arc>();
            nbSommets++;
        }

        public void AjouterArc(int source, int destination, double poids, string ligne, bool estCorrespondance = false)
        {
            if (!noeuds.ContainsKey(source) throw new ArgumentException($"Le nœud source {source} n'existe pas");
            if (!noeuds.ContainsKey(destination)) throw new ArgumentException($"Le nœud destination {destination} n'existe pas");

            listeAdjacence[source].Add(new Arc(destination, poids, ligne, estCorrespondance));
        }

        public List<Noeud<T>> GetNoeuds()
        {
            return noeuds.Values.ToList();
        }

        public Noeud<T> GetNoeud(int id)
        {
            return noeuds.ContainsKey(id) ? noeuds[id] : null;
        }

        public Dictionary<int, List<Arc>> GetListeAdjacence()
        {
            return listeAdjacence;
        }

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
    double[,] dist = new double[n + 1, n + 1]; // +1 car les IDs commencent à 1
    int[,] next = new int[n + 1, n + 1];
    
    for (int i = 1; i <= n; i++)
    {
        for (int j = 1; j <= n; j++)
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
            dist[u, v] = arc.Poids;
            next[u, v] = v;
        }
    }
    
    for (int k = 1; k <= n; k++)
    {
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= n; j++)
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
    
    for (int i = 1; i <= n; i++)
    {
        if (dist[i, i] < 0)
        {
            throw new InvalidOperationException("Le graphe contient un cycle de poids négatif");
        }
    }
    
    if (next[depart, arrivee] == -1)
    {
        return (new List<int>(), double.MaxValue);
    }
    
    var chemin = new List<int>();
    chemin.Add(depart);
    
    int u = depart;
    int v = arrivee;
    
    while (u != v)
    {
        u = next[u, v];
        chemin.Add(u);
    }
    
    return (chemin, dist[depart, arrivee]);
}

       public void VisualiserGraphe(string cheminImage)
{
    int largeur = 5000;
    int hauteur = 5000;
    int rayonSommet = 10;
    int marge = 100;
    
    using (var bitmap = new SKBitmap(largeur, hauteur))
    using (var canvas = new SKCanvas(bitmap))
    {
        canvas.Clear(SKColors.White);
        Dictionary<int, SKPoint> positionsSommets = new Dictionary<int, SKPoint>();
        
        // Convertir les coordonnées GPS en positions sur l'image
        double minLon = noeuds.Values.Min(n => n.Longitude);
        double maxLon = noeuds.Values.Max(n => n.Longitude);
        double minLat = noeuds.Values.Min(n => n.Latitude);
        double maxLat = noeuds.Values.Max(n => n.Latitude);
        
        foreach (var noeud in noeuds.Values)
        {
            float x = marge + (float)((noeud.Longitude - minLon) / (maxLon - minLon) * (largeur - 2 * marge);
            float y = hauteur - marge - (float)((noeud.Latitude - minLat) / (maxLat - minLat) * (hauteur - 2 * marge);
            positionsSommets[noeud.Id] = new SKPoint(x, y);
        }
        
        // Dessiner les arcs avec couleurs par ligne
        var couleursLignes = new Dictionary<string, SKColor>
        {
            {"1", SKColors.Yellow}, {"2", SKColors.Blue}, {"3", SKColors.Red},
            {"4", SKColors.DarkMagenta}, {"5", SKColors.Orange}, {"6", SKColors.DarkGreen},
            {"7", SKColors.Pink}, {"7bis", SKColors.LightGreen}, {"8", SKColors.Purple},
            {"9", SKColors.LightBlue}, {"10", SKColors.Beige}, {"11", SKColors.Brown},
            {"12", SKColors.DarkCyan}, {"13", SKColors.LightSkyBlue}, {"14", SKColors.DarkRed},
            {"Correspondance", SKColors.Gray}
        };
        
        foreach (var sommet in listeAdjacence)
        {
            foreach (var arc in sommet.Value)
            {
                SKPoint p1 = positionsSommets[sommet.Key];
                SKPoint p2 = positionsSommets[arc.Destination];
                
                var couleur = couleursLignes.ContainsKey(arc.Ligne) ? 
                    couleursLignes[arc.Ligne] : SKColors.Black;
                
                using (var paintArrete = new SKPaint { 
                    Color = couleur, 
                    StrokeWidth = arc.EstCorrespondance ? 1 : 3, 
                    IsAntialias = true,
                    PathEffect = arc.EstCorrespondance ? SKPathEffect.CreateDash(new float[] {5, 5}, 0) : null
                })
                {
                    canvas.DrawLine(p1, p2, paintArrete);
                }
            }
        }
        
        // Dessiner les nœuds
        using (var paintSommet = new SKPaint { Color = SKColors.Black, IsAntialias = true })
        using (var paintTexte = new SKPaint { 
            Color = SKColors.Black, 
            TextSize = 14, 
            TextAlign = SKTextAlign.Center, 
            IsAntialias = true 
        })
        {
            foreach (var sommet in positionsSommets)
            {
                SKPoint position = sommet.Value;
                canvas.DrawCircle(position, rayonSommet, paintSommet);
                
                // Afficher seulement les noms des stations principales
                var noeud = noeuds[sommet.Key];
                if (noeud.Nom.Contains("Charles") || noeud.Nom.Contains("Châtelet") || 
                    noeud.Nom.Contains("Nation") || noeud.Nom.Contains("Gare"))
                {
                    canvas.DrawText(noeud.Nom, position.X, position.Y - 15, paintTexte);
                }
            }
        }
        
        // Légende
        float yLegende = 50;
        foreach (var ligne in couleursLignes)
        {
            if (ligne.Key == "Correspondance") continue;
            
            using (var paintLigne = new SKPaint { Color = ligne.Value, StrokeWidth = 10, IsAntialias = true })
            using (var paintTexte = new SKPaint { Color = SKColors.Black, TextSize = 20, IsAntialias = true })
            {
                canvas.DrawLine(50, yLegende, 150, yLegende, paintLigne);
                canvas.DrawText($"Ligne {ligne.Key}", 160, yLegende + 7, paintTexte);
                yLegende += 30;
            }
        }
        
        // Enregistrement
        using (var image = SKImage.FromBitmap(bitmap))
        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
        {
            File.WriteAllBytes(cheminImage, data.ToArray());
        }
    }
    
    Console.WriteLine($"Carte du métro enregistrée sous : {cheminImage}");
}
    }
}
