﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ps1_v1
{
    internal class Graphe
    {
        //graphe simple G(V,E)
        // V est l'ensemble des sommets du graphe G
        // E est l'ensemble des liaisons/arêtes du graphe G
        private List<Noeud> v;
        private List<Lien> e;

        public Graphe (List<Noeud> noeuds, List<Lien> liens)
        {
            v = noeuds;
            e = liens ;
        }   
        public List<Noeud> V
        {
            get { return v; }
            set { v = value; }
        }
        public List<Lien> E
        {
            get { return e; }
            set { e = value; }
        }
    }
}
