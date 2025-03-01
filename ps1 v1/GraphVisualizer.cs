using SkiaSharp;
using System.Diagnostics;

namespace ps1_v1
{
    /// <summary>
    /// Classe statique pour visualiser un graphe en utilisant SkiaSharp.
    /// </summary>
    public static class GraphVisualizer
    {
        /// <summary>
        /// Génère et sauvegarde une image du graphe donné.
        /// </summary>
        /// <param name="graphe">Le graphe à visualiser.</param>
        /// <param name="filePath">Le chemin du fichier pour sauvegarder l'image.</param>
        public static void Visualize(Graphe graphe, string filePath)
        {
            const int width = 1200;
            const int height = 1200;
            const float margin = 100;

            using var surface = SKSurface.Create(new SKImageInfo(width, height));
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);

            var nodes = graphe.V;
            if (nodes.Count == 0) return;

            var positions = CalculateNodePositions(width, height, margin, nodes);
            DrawConnections(canvas, positions, graphe.E);
            DrawNodes(canvas, positions);

            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using (var stream = File.OpenWrite(filePath))
            {
                data.SaveTo(stream);
            }

            OpenImage(filePath);
        }

        /// <summary>
        /// Ouvre l'image générée en fonction du système d'exploitation.
        /// </summary>
        /// <param name="filePath">Le chemin de l'image à ouvrir.</param>
        private static void OpenImage(string filePath)
        {
            try
            {
                var fullPath = Path.GetFullPath(filePath);
                Console.WriteLine($"Tentative d'ouverture de l'image: {fullPath}");

                if (OperatingSystem.IsLinux())
                {
                    try
                    {
                        var startInfo = new ProcessStartInfo
                        {
                            FileName = "feh",
                            Arguments = $"--scale-down --auto-zoom \"{fullPath}\"",
                            UseShellExecute = false,
                            RedirectStandardError = false,
                            RedirectStandardOutput = false,
                            CreateNoWindow = false
                        };

                        var process = Process.Start(startInfo);
                        if (process != null)
                        {
                            Console.WriteLine("Image ouverte avec feh");
                            return;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Installation de feh nécessaire.");
                        Console.WriteLine("Utilisez la commande:");
                        Console.WriteLine("sudo apt-get install feh");

                        try
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = "xdg-open",
                                Arguments = $"\"{Path.GetDirectoryName(fullPath)}\"",
                                UseShellExecute = true
                            });
                            Console.WriteLine("Dossier contenant l'image ouvert");
                        }
                        catch
                        {
                            Console.WriteLine($"L'image est sauvegardée dans: {fullPath}");
                        }
                    }
                }
                else if (OperatingSystem.IsWindows())
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = fullPath,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                else if (OperatingSystem.IsMacOS())
                {
                    Process.Start("open", fullPath);
                }

                Console.WriteLine("Image générée avec succès!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not open the image: {ex.Message}");
                Console.WriteLine($"You can find the image at: {Path.GetFullPath(filePath)}");
                Console.WriteLine("Pour visualiser l'image, installez feh:");
                Console.WriteLine("sudo apt-get install feh");
            }
        }

        /// <summary>
        /// Calcule les positions des nœuds pour un agencement circulaire.
        /// </summary>
        /// <param name="width">Largeur de l'image.</param>
        /// <param name="height">Hauteur de l'image.</param>
        /// <param name="margin">Marge autour des nœuds.</param>
        /// <param name="nodes">Liste des nœuds du graphe.</param>
        /// <returns>Un dictionnaire associant chaque nœud à sa position.</returns>
        private static Dictionary<Noeud, SKPoint> CalculateNodePositions(int width, int height, float margin, List<Noeud> nodes)
        {
            var positions = new Dictionary<Noeud, SKPoint>();
            float centerX = width / 2f;
            float centerY = height / 2f;
            float radius = (Math.Min(width, height) / 2f) - margin;

            for (int i = 0; i < nodes.Count; i++)
            {
                float angle = (float)(2 * Math.PI * i / nodes.Count);
                float x = centerX + radius * (float)Math.Cos(angle);
                float y = centerY + radius * (float)Math.Sin(angle);
                positions[nodes[i]] = new SKPoint(x, y);
            }
            return positions;
        }

        /// <summary>
        /// Dessine les arêtes du graphe sur le canvas.
        /// </summary>
        /// <param name="canvas">Le canvas sur lequel dessiner.</param>
        /// <param name="positions">Les positions des nœuds.</param>
        /// <param name="liens">La liste des arêtes.</param>
        private static void DrawConnections(SKCanvas canvas, Dictionary<Noeud, SKPoint> positions, List<Lien> liens)
        {
            using var edgePaint = new SKPaint
            {
                Color = SKColors.Black,
                StrokeWidth = 2,
                IsAntialias = true
            };

            foreach (var lien in liens)
            {
                var p1 = positions[lien.Noeud1];
                var p2 = positions[lien.Noeud2];
                canvas.DrawLine(p1, p2, edgePaint);
            }
        }

        /// <summary>
        /// Dessine les nœuds du graphe sur le canvas.
        /// </summary>
        /// <param name="canvas">Le canvas sur lequel dessiner.</param>
        /// <param name="positions">Les positions des nœuds.</param>
        private static void DrawNodes(SKCanvas canvas, Dictionary<Noeud, SKPoint> positions)
        {
            using var nodePaint = new SKPaint
            {
                Color = SKColors.LightBlue,
                IsAntialias = true
            };
            using var textPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 14,
                TextAlign = SKTextAlign.Center,
                IsAntialias = true
            };

            foreach (var (node, pos) in positions)
            {
                canvas.DrawCircle(pos, 15, nodePaint);
                canvas.DrawText(node.Id.ToString(), pos.X, pos.Y + 5, textPaint);
            }
        }
    }
}
