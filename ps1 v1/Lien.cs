using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ps1_v1
{ 

    /// <summary>
    /// Classe représentant un lien entre deux nœuds dans un graphe.
    /// </summary>
    public class Lien
    {
        /// <summary>
        /// Nœud de départ du lien.
        /// </summary>
        public Noeud Noeud1 { get; set; }

        /// <summary>
        /// Nœud d'arrivée du lien.
        /// </summary>
        public Noeud Noeud2 { get; set; }

        /// <summary>
        /// Poids du lien (optionnel, utile pour les graphes pondérés).
        /// </summary>
        public double Poids { get; set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe Lien.
        /// </summary>
        /// <param name="noeud1">Premier nœud du lien.</param>
        /// <param name="noeud2">Deuxième nœud du lien.</param>
        /// <param name="poids">Poids du lien.</param>
        public Lien(Noeud noeud1, Noeud noeud2, double poids)
        {
            Noeud1 = noeud1;
            Noeud2 = noeud2;
            Poids = poids;
        }
    }
}