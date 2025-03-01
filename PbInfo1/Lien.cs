using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PbInfo
{
    /// <summary>
    /// Représente un lien entre deux nœuds dans un graphe.
    /// </summary>
    public class Lien
    {
        /// <summary>
        /// Identifiant du premier nœud du lien.
        /// </summary>
        public int Noeud1 { get; set; }

        /// <summary>
        /// Identifiant du second nœud du lien.
        /// </summary>
        public int Noeud2 { get; set; }

        /// <summary>
        /// Constructeur de la classe Lien.
        /// </summary>
        /// <param name="noeud1">Premier nœud.</param>
        /// <param name="noeud2">Second nœud.</param>
        public Lien(int noeud1, int noeud2)
        {
            Noeud1 = noeud1;
            Noeud2 = noeud2;
        }

        /// <summary>
        /// Retourne une représentation du lien.
        /// </summary>
        /// <returns>Chaîne représentant le lien.</returns>
        public override string ToString()
        {
            return $"{Noeud1} - {Noeud2}";
        }
    }
}
