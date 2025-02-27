using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        bool[,] MatriceAdjacence;

        public Graphe(List<Noeud> noeuds, List<Lien> liens)
        {
            v = noeuds;
            e = liens;
            MatriceAdjacence = GenMatAdj(noeuds, liens);
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
        public static bool[,] GenMatAdj(List<Noeud> noeuds, List<Lien> liens)
        {
            int n = noeuds.Count;
            bool[,] mat = new bool[n, n];

            foreach (var lien in liens)
            {
                int i = noeuds.IndexOf(lien.Noeud1);
                int j = noeuds.IndexOf(lien.Noeud2);
                mat[i, j] = mat[j, i] = true; // Symétrie pour graphe non orienté
            }
            return mat;
        }

        public bool Lien_Existe(Noeud n1, Noeud n2)
        {
            return e.Exists(l => (l.Noeud1 == n1 && l.Noeud2 == n2) || (l.Noeud1 == n2 && l.Noeud2 == n1));
        }


        public List<List<Noeud>> ListeAdjacence()
        {
            //listeAdj est un tableau dont chaque élément est également un tableau
            List<List<Noeud>> listeAdj = new List<List<Noeud>>();
            for (int i = 0; i < v.Count; i++)
            {
                List<Noeud> temp = new List<Noeud>();
                for (int j = 0; j < v.Count; j++)
                {
                    if (Lien_Existe(v[i], v[j]))
                    {
                        temp.Add(v[j]);
                    }
                }
                listeAdj.Add(temp);
            }
            return listeAdj;
        }


        public void AfficherMatriceAdjacence()
        {
            for (int i = 0; i < v.Count; i++)
            {
                for (int j = 0; j < v.Count; j++)
                {
                    Console.Write(MatriceAdjacence[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        // Parcours en largeur (BFS)
        public void ParcoursLargeur(int idDepart)
        {
            var depart = v.Find(n => n.Id == idDepart);
            if (depart == null) return;

            var file = new Queue<Noeud>();
            var visites = new HashSet<int>();

            file.Enqueue(depart);
            visites.Add(depart.Id);

            while (file.Count > 0)
            {
                var actuel = file.Dequeue();
                Console.Write(actuel.Id + " ");

                foreach (var voisin in ListeAdjacence()[v.IndexOf(actuel)])
                {
                    if (!visites.Contains(voisin.Id))
                    {
                        visites.Add(voisin.Id);
                        file.Enqueue(voisin);
                    }
                }
            }
            Console.WriteLine();
        }

        // Parcours en profondeur (DFS)
        public void ParcoursProfondeur(int idDepart)
        {
            var depart = v.Find(n => n.Id == idDepart);
            if (depart == null) return;

            var pile = new Stack<Noeud>();
            var visites = new HashSet<int>();

            pile.Push(depart);

            while (pile.Count > 0)
            {
                var actuel = pile.Pop();
                if (!visites.Contains(actuel.Id))
                {
                    Console.Write(actuel.Id + " ");
                    visites.Add(actuel.Id);

                    foreach (var voisin in ListeAdjacence()[v.IndexOf(actuel)])
                    {
                        if (!visites.Contains(voisin.Id))
                        {
                            pile.Push(voisin);
                        }
                    }
                }
            }
            Console.WriteLine();
        }

        // Vérifier si le graphe est connexe
        public bool EstConnexe()
        {
            if (v.Count == 0) return false;

            var visites = new HashSet<int>();
            ParcoursLargeur(v[0].Id);

            return visites.Count == v.Count;
        }

        // Détecter les cycles dans le graphe
        public bool ContientCycle()
        {
            var visites = new HashSet<int>();

            foreach (var noeud in v)
            {
                if (!visites.Contains(noeud.Id))
                {
                    if (DetecterCycle(noeud, null, visites))
                        return true;
                }
            }
            return false;
        }

        private bool DetecterCycle(Noeud actuel, Noeud parent, HashSet<int> visites)
        {
            visites.Add(actuel.Id);

            foreach (var voisin in ListeAdjacence()[v.IndexOf(actuel)])
            {
                if (!visites.Contains(voisin.Id))
                {
                    if (DetecterCycle(voisin, actuel, visites))
                        return true;
                }
                else if (voisin != parent)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
