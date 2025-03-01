namespace ps1_v1
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Use relative path from the executable location
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                "Association-soc-karate", "soc-karate.mtx");
            
            try
            {
                // Charger le graphe depuis le fichier
                var graphe = MatrixMarketReader.LireGraphe(filePath);
                
                Console.WriteLine($"Graphe chargé avec succès !");
                Console.WriteLine($"Nombre de noeuds: {graphe.V.Count}");
                Console.WriteLine($"Nombre de liens: {graphe.E.Count}");
                
                Console.WriteLine("\nMatrice d'adjacence :");
                graphe.AfficherMatriceAdjacence();
                
                Console.WriteLine("\nStructure détaillée du graphe:");
                AfficherGrapheConsole(graphe);

                Console.WriteLine("\nParcours en largeur à partir du noeud 1:");
                graphe.ParcoursLargeur(1);

                Console.WriteLine("\nParcours en profondeur à partir du noeud 1:");
                graphe.ParcoursProfondeur(1);

                Console.WriteLine($"\nLe graphe est{(graphe.EstConnexe() ? "" : " non")} connexe");
                Console.WriteLine($"Le graphe contient{(graphe.ContientCycle() ? "" : " non")} des cycles");

                // Visualiser le graphe
                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "karate_club.png");
                GraphVisualizer.Visualize(graphe, outputPath);
                Console.WriteLine($"\nGraph visualization saved to: {outputPath}");
                Console.WriteLine("Appuyez sur une touche pour quitter...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement du graphe: {ex.Message}");
                Console.WriteLine($"Assurez-vous que le fichier existe dans: {Path.GetFullPath(filePath)}");
            }
        }

        static void AfficherGrapheConsole(Graphe graphe)
        {
            foreach (var noeud in graphe.V)
            {
                Console.WriteLine($"Noeud {noeud.Id} est connecté à :");
                var voisins = graphe.ListeAdjacence()[graphe.V.IndexOf(noeud)];
                foreach (var voisin in voisins)
                {
                    var lien = graphe.E.Find(l => 
                        (l.Noeud1 == noeud && l.Noeud2 == voisin) || 
                        (l.Noeud1 == voisin && l.Noeud2 == noeud));
                    Console.WriteLine($"  -> Noeud {voisin.Id} (poids: {lien.Poids})");
                }
                Console.WriteLine();
            }
        }
    }
}
