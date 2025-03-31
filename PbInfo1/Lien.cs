using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PbInfo
{
    public class Arc
    {
        public int Destination { get; set; }
        public double Poids { get; set; }
        public string Ligne { get; set; }
        public bool EstCorrespondance { get; set; }

        public Arc(int destination, double poids, string ligne, bool estCorrespondance = false)
        {
            Destination = destination;
            Poids = poids;
            Ligne = ligne;
            EstCorrespondance = estCorrespondance;
        }
    }
}
