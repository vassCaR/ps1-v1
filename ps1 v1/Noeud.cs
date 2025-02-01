using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ps1_v1
{
    internal class Noeud
    {
        // Identifiant unique du nœud
        public int Id { get; set; }

        // Liste des liens connectés à ce nœud
        public List<Lien> Liens { get; set; }

        // Constructeur
        public Noeud(int id)
        {
            Id = id;
            Liens = new List<Lien>();
        }

        // Méthode pour ajouter un lien
        public void AjouterLien(Lien lien)
        {
            if (!Liens.Contains(lien))
            {
                Liens.Add(lien);
            }
        }
    }
}
