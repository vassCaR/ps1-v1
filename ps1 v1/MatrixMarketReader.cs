namespace ps1_v1
{
    /// <summary>
    /// Fournit des méthodes pour lire un graphe à partir d'un fichier au format Matrix Market.
    /// </summary>
    public static class MatrixMarketReader
    {
        /// <summary>
        /// Lit un graphe à partir d'un fichier Matrix Market spécifié.
        /// </summary>
        /// <param name="filePath">Le chemin du fichier à lire.</param>
        /// <returns>Un objet <see cref="Graphe"/> représentant le graphe lu.</returns>
        /// <exception cref="Exception">Lève une exception si le fichier est invalide ou s'il y a une erreur de lecture.</exception>
        public static Graphe LireGraphe(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);
                var noeuds = new List<Noeud>();
                var liens = new List<Lien>();

                // Ignore les lignes de commentaire commençant par %
                var dataLines = lines.Where(l => !l.StartsWith("%")).ToList();

                if (dataLines.Count == 0)
                {
                    throw new Exception("Le fichier ne contient pas de données valides");
                }

                // La première ligne non commentée contient les dimensions
                var dimensions = dataLines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (dimensions.Length < 2)
                {
                    throw new Exception("Format de dimensions invalide");
                }

                int nbNoeuds = int.Parse(dimensions[0]);
                Console.WriteLine($"Création de {nbNoeuds} noeuds...");

                // Créer les noeuds
                for (int i = 1; i <= nbNoeuds; i++)
                {
                    noeuds.Add(new Noeud(i));
                }

                // Lire les liens
                foreach (var line in dataLines.Skip(1))
                {
                    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2 && int.TryParse(parts[0], out int node1) && int.TryParse(parts[1], out int node2))
                    {
                        if (node1 > 0 && node1 <= nbNoeuds && node2 > 0 && node2 <= nbNoeuds)
                        {
                            var lien = new Lien(noeuds[node1 - 1], noeuds[node2 - 1], 1);
                            liens.Add(lien);
                        }
                    }
                }

                Console.WriteLine($"Création de {liens.Count} liens...");
                return new Graphe(noeuds, liens);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur de lecture du fichier: {ex.Message}");
            }
        }
    }
}
