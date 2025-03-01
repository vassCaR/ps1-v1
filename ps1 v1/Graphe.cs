using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ps1_v1
{
    /// <summary>
    /// Représente un graphe non orienté défini par un ensemble de sommets (Noeuds) et un ensemble d'arêtes (Liens).
    /// </summary>
    public class Graphe
    {
        /// <summary>
        /// Liste des sommets du graphe.
        /// </summary>
        private List<Noeud> v;

        /// <summary>
        /// Liste des arêtes (liens) du graphe.
        /// </summary>
        private List<Lien> e;

        /// <summary>
        /// Matrice d'adjacence représentant les connexions entre les sommets.
        /// </summary>
        private bool[,] MatriceAdjacence;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Graphe"/> avec des sommets et des arêtes spécifiés.
        /// </summary>
        /// <param name="noeuds">Liste des sommets du graphe.</param>
        /// <param name="liens">Liste des arêtes reliant les sommets.</param>
        public Graphe(List<Noeud> noeuds, List<Lien> liens)
        {
            v = noeuds;
            e = liens;
            MatriceAdjacence = GenMatAdj(noeuds, liens);
        }

        /// <summary>
        /// Obtient ou définit la liste des sommets du graphe.
        /// </summary>
        public List<Noeud> V
        {
            get { return v; }
            set { v = value; }
        }

        /// <summary>
        /// Obtient ou définit la liste des arêtes du graphe.
        /// </summary>
        public List<Lien> E
        {
            get { return e; }
            set { e = value; }
        }

        /// <summary>
        /// Génère la matrice d'adjacence du graphe à partir des sommets et des arêtes.
        /// </summary>
        /// <param name="noeuds">Liste des sommets.</param>
        /// <param name="liens">Liste des arêtes.</param>
        /// <returns>Une matrice booléenne où une valeur true indique une connexion entre deux sommets.</returns>
        public static bool[,] GenMatAdj(List<Noeud> noeuds, List<Lien> liens)
        {
            int n = noeuds.Count;
            bool[,] mat = new bool[n, n];

            foreach (var lien in liens)
            {
                int i = noeuds.IndexOf(lien.Noeud1);
                int j = noeuds.IndexOf(lien.Noeud2);
                mat[i, j] = mat[j, i] = true; // Symétrie pour un graphe non orienté
            }
            return mat;
        }

        /// <summary>
        /// Vérifie si un lien existe entre deux sommets donnés.
        /// </summary>
        /// <param name="n1">Premier sommet.</param>
        /// <param name="n2">Deuxième sommet.</param>
        /// <returns>True si un lien existe, sinon False.</returns>
        public bool Lien_Existe(Noeud n1, Noeud n2)
        {
            return e.Exists(l => (l.Noeud1 == n1 && l.Noeud2 == n2) || (l.Noeud1 == n2 && l.Noeud2 == n1));
        }

        /// <summary>
        /// Crée une liste d'adjacence où chaque sommet est associé à la liste de ses voisins.
        /// </summary>
        /// <returns>Une liste de listes représentant la structure d'adjacence.</returns>
        public List<List<Noeud>> ListeAdjacence()
        {
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

        /// <summary>
        /// Affiche la matrice d'adjacence du graphe dans la console.
        /// </summary>
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

        /// <summary>
        /// Effectue un parcours en largeur (BFS) du graphe à partir d'un sommet donné.
        /// </summary>
        /// <param name="idDepart">Identifiant du sommet de départ.</param>
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
        /// <summary>
        /// Vérifie si le graphe est connexe (tous les sommets sont accessibles depuis n'importe quel sommet).
        /// </summary>
        /// <returns>True si le graphe est connexe, sinon False.</returns>
        public bool EstConnexe()
        {
            if (v.Count == 0) return false;

            var visites = new HashSet<int>();
            ParcoursLargeur(v[0].Id);

            return visites.Count == v.Count;
        }

        /// <summary>
        /// Détecte la présence de cycles dans le graphe.
        /// </summary>
        /// <returns>True si un cycle est détecté, sinon False.</returns>
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

        /// <summary>
        /// Effectue un parcours en profondeur (DFS) du graphe à partir d'un sommet donné.
        /// </summary>
        /// <param name="idDepart">Identifiant du sommet de départ.</param>
        public void ParcoursProfondeur(int idDepart)
        {
            var depart = v.Find(n => n.Id == idDepart);
            if (depart == null)
            {
                Console.WriteLine("Sommet de départ introuvable.");
                return;
            }

            var visites = new HashSet<int>();
            ParcoursProfondeurRecursive(depart, visites);
            Console.WriteLine();
        }

        /// <summary>
        /// Méthode récursive pour explorer le graphe en profondeur.
        /// </summary>
        /// <param name="noeud">Noeud actuel à visiter.</param>
        /// <param name="visites">Ensemble des sommets déjà visités.</param>
        private void ParcoursProfondeurRecursive(Noeud noeud, HashSet<int> visites)
        {
            // Marquer le noeud comme visité
            visites.Add(noeud.Id);
            Console.Write(noeud.Id + " ");

            // Explorer les voisins non visités
            foreach (var voisin in ListeAdjacence()[v.IndexOf(noeud)])
            {
                if (!visites.Contains(voisin.Id))
                {
                    ParcoursProfondeurRecursive(voisin, visites);
                }
            }
        }

        /// <summary>
        /// Méthode auxiliaire pour détecter récursivement un cycle dans le graphe.
        /// </summary>
        /// <param name="actuel">Noeud actuel.</param>
        /// <param name="parent">Noeud parent du noeud actuel.</param>
        /// <param name="visites">Ensemble des noeuds déjà visités.</param>
        /// <returns>True si un cycle est détecté, sinon False.</returns>
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

