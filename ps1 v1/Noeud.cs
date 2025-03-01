using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ps1_v1
{
    public class Noeud
    {
        public int Id { get; set; }
        public List<Lien> Liens { get; set; }

        public Noeud(int id)
        {
            Id = id;
            Liens = new List<Lien>();
        }

        public void AjouterLien(Lien lien)
        {
            if (!Liens.Contains(lien))
            {
                Liens.Add(lien);
                // Add reciprocal link to connected node
                if (lien.Noeud1 == this && !lien.Noeud2.Liens.Contains(lien))
                    lien.Noeud2.Liens.Add(lien);
                else if (lien.Noeud2 == this && !lien.Noeud1.Liens.Contains(lien))
                    lien.Noeud1.Liens.Add(lien);
            }
        }
    }
}
