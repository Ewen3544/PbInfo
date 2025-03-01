using System;
using System.Collections.Generic;
using System.IO;

namespace PbInfo
{
    public class Program
    {
        static void Main(string[] args)
        {
            string cheminFichier = @"C:\Users\ewenr\Downloads\karate.txt";
            string[] lignes = File.ReadAllLines(cheminFichier);
            string[] premiereLigne = lignes[0].Split(' ');
            int nbSommets = int.Parse(premiereLigne[0]);
            List<Lien> liens = new List<Lien>();
            for (int i = 1; i < lignes.Length; i++)
            {
                string[] elementsLigne = lignes[i].Split(' ');
                if (elementsLigne.Length == 2)
                {
                    int sommet1 = int.Parse(elementsLigne[0]);
                    int sommet2 = int.Parse(elementsLigne[1]);
                    liens.Add(new Lien(sommet1, sommet2));
                }
            }
            Graphe graphe = new Graphe(liens, nbSommets);
            graphe.AfficherListeAdjacence();
            graphe.AfficherMatriceAdjacence();
            graphe.Largeur(1);
            graphe.Profondeur(1);
            if (graphe.ContientUnCycle())
                Console.WriteLine("Le graphe contient un cycle");
            else
                Console.WriteLine("Le graphe ne contient pas de cycle");
            graphe.AnalyserGraphe();
            graphe.VisualiserGraphe("graphe.png");
        }
    }
}
