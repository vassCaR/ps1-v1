using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ps1_v1
{
    internal class Lien
    {
        // Noeuds connectés par ce lien
        public Noeud Noeud1 { get; set; }
        public Noeud Noeud2 { get; set; }

        // Poids du lien (optionnel, utile pour les graphes pondérés)
        public double Poids { get; set; }

        // Constructeur
        public Lien(Noeud noeud1, Noeud noeud2, double poids)
        {
            Noeud1 = noeud1;
            Noeud2 = noeud2;
            Poids = poids;
        }
    }
}
