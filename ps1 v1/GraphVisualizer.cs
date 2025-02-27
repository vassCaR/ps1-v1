using SkiaSharp.Views.Desktop;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Windows.Forms; // Pour Form, ClientSize, Controls
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace ps1_v1
{
    internal class GraphVisualizer : Form
    {
        private Graphe graphe;

        public GraphVisualizer(Graphe graphe)
        {
            this.graphe = graphe;
            this.Text = "Visualisation du Graphe";
            this.ClientSize = new System.Drawing.Size(800, 800);

            var skControl = new SKControl();
            skControl.Dock = DockStyle.Fill;
            skControl.PaintSurface += OnPaintSurface;
            this.Controls.Add(skControl);
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);

            int centerX = e.Info.Width / 2;
            int centerY = e.Info.Height / 2;
            int radius = Math.Min(centerX, centerY) - 50;
            int nodeCount = graphe.V.Count;

            var nodePositions = new Dictionary<Noeud, SKPoint>();

            // Calculer les positions circulaires
            for (int i = 0; i < nodeCount; i++)
            {
                double angle = 2 * Math.PI * i / nodeCount;
                float x = centerX + (float)(radius * Math.Cos(angle));
                float y = centerY + (float)(radius * Math.Sin(angle));
                nodePositions[graphe.V[i]] = new SKPoint(x, y);
            }

            // Dessiner les arêtes (liens)
            using (var linePaint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true,
                StrokeWidth = 2
            })
            {
                foreach (var lien in graphe.E)
                {
                    var start = nodePositions[lien.Noeud1];
                    var end = nodePositions[lien.Noeud2];
                    canvas.DrawLine(start, end, linePaint);
                }
            }

            // Dessiner les nœuds
            using (var nodePaint = new SKPaint
            {
                Color = SKColors.Blue,
                IsAntialias = true
            })
            using (var textPaint = new SKPaint
            {
                Color = SKColors.White,
                TextSize = 20,
                IsAntialias = true,
                TextAlign = SKTextAlign.Center
            })
            {
                foreach (var noeud in graphe.V)
                {
                    var position = nodePositions[noeud];
                    canvas.DrawCircle(position, 30, nodePaint);
                    canvas.DrawText(noeud.Id.ToString(), position.X, position.Y + 7, textPaint);
                }
            }
        }
    }

}
