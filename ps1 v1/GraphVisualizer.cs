using SkiaSharp;
using System.Diagnostics;

namespace ps1_v1
{
    public static class GraphVisualizer
    {
        public static void Visualize(Graphe graphe, string filePath)
        {
            // Augmenter la taille pour un plus grand graphe
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
            // Ensure file is completely written before opening
            OpenImage(filePath);
        }

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
                            FileName = "feh",  // Using feh as it's more lightweight
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
                        
                        // Try to open file manager as fallback
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
                TextSize = 14, // Réduire la taille du texte
                TextAlign = SKTextAlign.Center, 
                IsAntialias = true 
            };

            foreach (var (node, pos) in positions)
            {
                canvas.DrawCircle(pos, 15, nodePaint); // Réduire la taille des cercles
                canvas.DrawText(node.Id.ToString(), pos.X, pos.Y + 5, textPaint);
            }
        }
    }
}
