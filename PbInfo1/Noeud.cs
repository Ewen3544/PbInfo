using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PbInfo
{
    /// <summary>
    /// Représente un nœud dans un graphe.
    /// </summary>
    public class Noeud
    {
        /// <summary>
        /// Identifiant unique du nœud.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Constructeur de la classe Noeud.
        /// </summary>
        /// <param name="id">Identifiant du nœud.</param>
        public Noeud(int id)
        {
            Id = id;
        }
        /// <summary>
        /// Retourne une représentation du nœud.
        /// </summary>
        /// <returns>Chaîne représentant le nœud.</returns>
        public override string ToString()
        {
            return $"Noeud: {Id}";
        }
    }
}
