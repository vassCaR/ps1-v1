using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ps1_v1
{
    /// <summary>
    /// Représente un nœud dans un graphe avec un identifiant unique et une liste de liens.
    /// </summary>
    public class Noeud
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique du nœud.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des liens connectés au nœud.
        /// </summary>
        public List<Lien> Liens { get; set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Noeud"/> avec un identifiant donné.
        /// </summary>
        /// <param name="id">L'identifiant unique du nœud.</param>
        public Noeud(int id)
        {
            Id = id;
            Liens = new List<Lien>();
        }

        /// <summary>
        /// Ajoute un lien au nœud s'il n'existe pas déjà et assure la liaison réciproque.
        /// </summary>
        /// <param name="lien">Le lien à ajouter au nœud.</param>
        public void AjouterLien(Lien lien)
        {
            if (!Liens.Contains(lien))
            {
                Liens.Add(lien);

                if (lien.Noeud1 == this && !lien.Noeud2.Liens.Contains(lien))
                    lien.Noeud2.Liens.Add(lien);
                else if (lien.Noeud2 == this && !lien.Noeud1.Liens.Contains(lien))
                    lien.Noeud1.Liens.Add(lien);
            }
        }
    }
}
