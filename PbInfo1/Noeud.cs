using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace PbInfo
{
    /// <summary>
    /// Représente un nœud générique dans un graphe.
    /// </summary>
    /// <typeparam name="T">Type des données stockées dans le nœud</typeparam>
    public class Noeud<T>
    {
        /// <summary>
        /// Identifiant unique du nœud.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Données associées au nœud
        /// </summary>
        public T Data { get; set; }
        
        /// <summary>
        /// Nom du nœud (pour affichage)
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Coordonnée latitude
        /// </summary>
        public double Latitude { get; set; }
        
        /// <summary>
        /// Coordonnée longitude
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Constructeur de la classe Noeud.
        /// </summary>
        /// <param name="id">Identifiant du nœud.</param>
        /// <param name="data">Données du nœud</param>
        /// <param name="nom">Nom du nœud</param>
        /// <param name="latitude">Position latitude</param>
        /// <param name="longitude">Position longitude</param>
        public Noeud(int id, T data, string nom, double latitude, double longitude)
        {
            Id = id;
            Data = data;
            Nom = nom;
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Retourne une représentation du nœud.
        /// </summary>
        /// <returns>Chaîne représentant le nœud.</returns>
        public override string ToString()
        {
            return $"Noeud {Id}: {Nom} ({Latitude}, {Longitude})";
        }
    }
}
