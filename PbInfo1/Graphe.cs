using System;
using System.Collections.Generic;
using System.IO;
using SkiaSharp;

namespace PbInfo
{
    public class Graphe
    {
        private Dictionary<int, List<int>> listeAdjacence;
        private int[,] matriceAdjacence;
        private int nbSommets;
        public Graphe(List<Lien> liens, int nbSommets)
        {
            this.nbSommets = nbSommets;
            listeAdjacence = new Dictionary<int, List<int>>();
            for (int i = 1; i <= nbSommets; i++)
            {
                listeAdjacence[i] = new List<int>();
            }
            foreach (var lien in liens)
            {
                listeAdjacence[lien.Noeud1].Add(lien.Noeud2);
                listeAdjacence[lien.Noeud2].Add(lien.Noeud1);
            }
            matriceAdjacence = new int[nbSommets, nbSommets];
            foreach (var lien in liens)
            {
                matriceAdjacence[lien.Noeud1 - 1, lien.Noeud2 - 1] = 1;
                matriceAdjacence[lien.Noeud2 - 1, lien.Noeud1 - 1] = 1;
            }
        }

        public void AfficherListeAdjacence()
        {
            Console.WriteLine("Liste d'adjacence :");
            foreach (var sommet in listeAdjacence)
            {
                Console.Write(sommet.Key + " : ");
                Console.WriteLine(string.Join(" ", sommet.Value));
            }
        }
        /// <summary>
        /// Affiche la matrice d'adjacence du graphe.
        /// </summary>
        public void AfficherMatriceAdjacence()
        {
            Console.WriteLine("Matrice d'adjacence :");
            for (int i = 0; i < nbSommets; i++)
            {
                for (int j = 0; j < nbSommets; j++)
                {
                    Console.Write(matriceAdjacence[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public void Largeur(int sommetDepart)
        {
            Console.WriteLine("Parcours en Largeur :");
            Queue<int> fileAttente = new Queue<int>();
            List<int> sommetsVisites = new List<int>();

            fileAttente.Enqueue(sommetDepart);
            sommetsVisites.Add(sommetDepart);

            while (fileAttente.Count > 0)
            {
                int sommet = fileAttente.Dequeue();
                Console.Write(sommet + " ");

                foreach (var voisin in listeAdjacence[sommet])
                {
                    if (!sommetsVisites.Contains(voisin))
                    {
                        fileAttente.Enqueue(voisin);
                        sommetsVisites.Add(voisin);
                    }
                }
            }

            Console.WriteLine("\nNombre de sommets visités : " + sommetsVisites.Count);
            if (sommetsVisites.Count == nbSommets)
                Console.WriteLine("Le graphe est connexe");
            else
                Console.WriteLine("Le graphe n'est pas connexe");
        }

        public void Profondeur(int sommetDepart)
        {
            Console.WriteLine("\nParcours en Profondeur :");
            List<int> sommetsVisites = new List<int>();
            ExplorerProfondeur(sommetDepart, sommetsVisites);
            Console.WriteLine();
        }

        private void ExplorerProfondeur(int sommet, List<int> sommetsVisites)
        {
            Console.Write(sommet + " ");
            sommetsVisites.Add(sommet);

            foreach (var voisin in listeAdjacence[sommet])
            {
                if (!sommetsVisites.Contains(voisin))
                {
                    ExplorerProfondeur(voisin, sommetsVisites);
                }
            }
        }

        public bool ContientUnCycle()
        {
            List<int> visites = new List<int>();
            return VerifierCycle(1, -1, visites);
        }
        /// <summary>
        /// Vérifie si le graphe contient un cycle.
        /// </summary>
        /// <returns>True si un cycle est détecté, False sinon.</returns>
        private bool VerifierCycle(int sommet, int parent, List<int> visites)
        {
            visites.Add(sommet);
            foreach (var voisin in listeAdjacence[sommet])
            {
                if (!visites.Contains(voisin))
                {
                    if (VerifierCycle(voisin, sommet, visites))
                        return true;
                }
                else if (voisin != parent)
                {
                    return true;
                }
            }
            return false;
        }
        public void AnalyserGraphe()
        {
            int tailleGraphe = 0;
            foreach (var listeVoisins in listeAdjacence.Values)
            {
                tailleGraphe += listeVoisins.Count;
            }
            tailleGraphe /= 2;

            Console.WriteLine("Analyse du Graphe :");
            Console.WriteLine($"Ordre du graphe (nombre de sommets) : {nbSommets}");
            Console.WriteLine($"Taille du graphe (nombre d’arêtes) : {tailleGraphe}");
            Console.WriteLine("Type du graphe :");
            Console.WriteLine("Graphes non orienté ");
            Console.WriteLine("Graphe simple (pas d’arêtes multiples) ");
        }
        /// <summary>
        /// Il faut installer SkiaSharp
        /// </summary>
        /// <param name="cheminImage"></param>
        public void VisualiserGraphe(string cheminImage)
        {
            int largeur = 3000;
            int hauteur = 3000;
            int rayonSommet = 70;
            int marge = 50;
            using (var bitmap = new SKBitmap(largeur, hauteur))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.White);
                Dictionary<int, SKPoint> positionsSommets = new Dictionary<int, SKPoint>();
                double angleStep = 2 * Math.PI / nbSommets;
                int centreX = largeur / 2;
                int centreY = hauteur / 2;
                int rayonGraph = Math.Min(largeur, hauteur) / 3;

                for (int i = 1; i <= nbSommets; i++)
                {
                    double angle = i * angleStep;
                    float x = centreX + (float)(rayonGraph * Math.Cos(angle));
                    float y = centreY + (float)(rayonGraph * Math.Sin(angle));
                    positionsSommets[i] = new SKPoint(x, y);
                }
                using (var paintArrete = new SKPaint { Color = SKColors.Black, StrokeWidth = 2, IsAntialias = true })
                {
                    foreach (var sommet in listeAdjacence)
                    {
                        foreach (var voisin in sommet.Value)
                        {
                            SKPoint p1 = positionsSommets[sommet.Key];
                            SKPoint p2 = positionsSommets[voisin];
                            canvas.DrawLine(p1, p2, paintArrete);
                        }
                    }
                }
                using (var paintSommet = new SKPaint { Color = SKColors.Blue, IsAntialias = true })
                using (var paintTexte = new SKPaint { Color = SKColors.White, TextSize = 35, TextAlign = SKTextAlign.Center, IsAntialias = true })
                {
                    foreach (var sommet in positionsSommets)
                    {
                        SKPoint position = sommet.Value;
                        canvas.DrawCircle(position, rayonSommet, paintSommet);
                        canvas.DrawText(sommet.Key.ToString(), position.X, position.Y + 5, paintTexte);
                    }
                }
                using (var image = SKImage.FromBitmap(bitmap))
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    File.WriteAllBytes(cheminImage, data.ToArray());
                }
            }

            Console.WriteLine($"Graphe visualisé et enregistré sous : {cheminImage}");
        }
        public Dictionary<int, List<int>> GetListeAdjacence()
        {
            return listeAdjacence;
        }

        public int[,] GetMatriceAdjacence()
        {
            return matriceAdjacence;
        }

        public bool EstConnexe()
        {
            List<int> sommetsVisites = new List<int>();
            ExplorerProfondeur(1, sommetsVisites);
            return sommetsVisites.Count == nbSommets;
        }

        public List<int> GetParcoursLargeur(int sommetDepart)
        {
            Queue<int> file = new Queue<int>();
            List<int> parcours = new List<int>();

            file.Enqueue(sommetDepart);
            parcours.Add(sommetDepart);

            while (file.Count > 0)
            {
                int sommet = file.Dequeue();
                foreach (var voisin in listeAdjacence[sommet])
                {
                    if (!parcours.Contains(voisin))
                    {
                        file.Enqueue(voisin);
                        parcours.Add(voisin);
                    }
                }
            }
            return parcours;
        }

        public List<int> GetParcoursProfondeur(int sommetDepart)
        {
            List<int> parcours = new List<int>();
            ExplorerProfondeur(sommetDepart, parcours);
            return parcours;
        }

    }
}

