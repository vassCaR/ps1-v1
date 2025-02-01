using System;
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
        bool[,] MatriceAdjacence;

        public Graphe (List<Noeud> noeuds, List<Lien> liens)
        {
            v = noeuds;
            e = liens ;
            MatriceAdjacence = GenMatAdj(noeuds,liens); 
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
        static bool[,] GenMatAdj(List<Noeud> noeuds, List<Lien> liens)
        {
            int n = noeuds.Count;
            bool[,] mat = new bool[n, n]; 

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    mat[i, j] = Lien_Existe(noeuds[i], noeuds[j]);//si la relation existe , on met dasn dans la case [i,j] de la matrice d'adjacence
                }
            }
            return mat;
        }

        public static bool Lien_Existe(Noeud n1, Noeud n2)
        {
            return n1.Liens.Any(l => l.Noeud1 == n2 || l.Noeud2 == n2);
        }
        public void AfficherMatriceAdjacence()
        {
            string chaine = "";
            for(int i=0;i<v.Count;i++)
            {
                for(int j=0;j<v.Count;j++)
                {
                    if (this.MatriceAdjacence[i,j]==true)
                    {
                        chaine += "1 ";
                    }
                    else
                    {
                        chaine += "0 ";
                    }
                }
                chaine += "\n";
            }
            Console.WriteLine(chaine);
        }

    }
}
