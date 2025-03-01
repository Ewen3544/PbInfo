using System;
using PbInfo;
using System.Collections.Generic;
using Xunit;
namespace ProjetUnitaire
{
    public class UnitTestGraphe
    {
        [Fact]
        public void TestListeAdjacence()
        {
            List<Lien> liens = new List<Lien>
            {
                new Lien(1, 2),
                new Lien(1, 3),
                new Lien(2, 3),
                new Lien(3, 4)
            };
            Graphe graphe = new Graphe(liens, 4);
            var adjacence = graphe.GetListeAdjacence();
            Assert.Equal(new List<int> { 2, 3 }, adjacence[1]);
            Assert.Equal(new List<int> { 1, 3 }, adjacence[2]);
            Assert.Equal(new List<int> { 1, 2, 4 }, adjacence[3]);
            Assert.Equal(new List<int> { 3 }, adjacence[4]);
        }

        [Fact]
        public void TestMatriceAdjacence()
        {
            List<Lien> liens = new List<Lien>
            {
                new Lien(1, 2),
                new Lien(1, 3),
                new Lien(2, 3),
                new Lien(3, 4)
            };
            Graphe graphe = new Graphe(liens, 4);
            var matrice = graphe.GetMatriceAdjacence();
            Assert.Equal(1, matrice[0, 1]);
            Assert.Equal(1, matrice[0, 2]); 
            Assert.Equal(1, matrice[1, 2]); 
            Assert.Equal(1, matrice[2, 3]); 
            Assert.Equal(0, matrice[0, 3]); 
        }

        [Fact]
        public void TestConnexite()
        {
            List<Lien> liens1 = new List<Lien>
            {
                new Lien(1, 2),
                new Lien(2, 3),
                new Lien(3, 4)
            };
            Graphe grapheConnexe = new Graphe(liens1, 4);

            Assert.True(grapheConnexe.EstConnexe());
            List<Lien> liens2 = new List<Lien>
            {
                new Lien(1, 2),
                new Lien(3, 4)
            };
            Graphe grapheNonConnexe = new Graphe(liens2, 4);

            Assert.False(grapheNonConnexe.EstConnexe());
        }

        [Fact]
        public void TestDetectionCycle()
        {
            List<Lien> liens1 = new List<Lien>
            {
                new Lien(1, 2),
                new Lien(2, 3),
                new Lien(3, 1)
            };
            Graphe grapheAvecCycle = new Graphe(liens1, 3);
            Assert.True(grapheAvecCycle.ContientUnCycle());
            List<Lien> liens2 = new List<Lien>
            {
                new Lien(1, 2),
                new Lien(2, 3)
            };
            Graphe grapheSansCycle = new Graphe(liens2, 3);
            Assert.False(grapheSansCycle.ContientUnCycle());
        }

        [Fact]
        public void TestParcoursLargeur()
        {
            List<Lien> liens = new List<Lien>
            {
                new Lien(1, 2),
                new Lien(1, 3),
                new Lien(2, 4),
                new Lien(3, 5)
            };
            Graphe graphe = new Graphe(liens, 5);
            var parcours = graphe.GetParcoursLargeur(1);
            Assert.Equal(new List<int> { 1, 2, 3, 4, 5 }, parcours);
        }

        [Fact]
        public void TestParcoursProfondeur()
        {
            List<Lien> liens = new List<Lien>
            {
                new Lien(1, 2),
                new Lien(1, 3),
                new Lien(2, 4),
                new Lien(3, 5)
            };
            Graphe graphe = new Graphe(liens, 5);
            var parcours = graphe.GetParcoursProfondeur(1);
            Assert.Equal(new List<int> { 1, 2, 4, 3, 5 }, parcours);
        }
    }
}
