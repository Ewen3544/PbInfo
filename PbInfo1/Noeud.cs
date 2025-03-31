using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace PbInfo
{
    public class Noeud<T>
    {
        public int Id { get; set; }
        public T Data { get; set; }
        public string Nom { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Ligne { get; set; }
        public string Commune { get; set; }

        public Noeud(int id, T data, string nom, double latitude, double longitude, string ligne, string commune)
        {
            Id = id;
            Data = data;
            Nom = nom;
            Latitude = latitude;
            Longitude = longitude;
            Ligne = ligne;
            Commune = commune;
        }

        public override string ToString()
        {
            return $"{Nom} (Ligne {Ligne})";
        }
    }
}
