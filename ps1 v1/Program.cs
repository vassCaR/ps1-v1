namespace ps1_v1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Création des nœuds
            Noeud n1 = new Noeud(1);
            Noeud n2 = new Noeud(2);
            Noeud n3 = new Noeud(3);
            List <Noeud> V = new List<Noeud> { n1, n2, n3 };
            

            // Création des liens
            Lien l1 = new Lien(n1, n2,4);
            Lien l2 = new Lien(n2, n3,2);
            List<Lien> E = new List<Lien> { l1, l2 };

            // Ajout des liens aux nœuds
            n1.AjouterLien(l1);
            n2.AjouterLien(l1);
            n2.AjouterLien(l2);
            n3.AjouterLien(l2);
            Graphe graphe = new Graphe(V,E);
            graphe.AfficherMatriceAdjacence();
        }
    }
}
