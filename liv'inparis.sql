CREATE DATABASE IF NOT EXISTS LivInParis;
USE LivInParis;

CREATE TABLE cuisinier(
   id_cuisinier INT PRIMARY KEY,
   listes_clients_servis VARCHAR(50)
);

CREATE TABLE type_client(
   type_ VARCHAR(50) PRIMARY KEY,
   id_nom_Entreprise VARCHAR(50),
   nom_référent VARCHAR(50)
);

CREATE TABLE type_plat(
   id_type_plat INT PRIMARY KEY,
   entree_ VARCHAR(50),
   dessert VARCHAR(50),
   plat_principal VARCHAR(50)
);

CREATE TABLE trajet(
   num_trajet INT PRIMARY KEY,
   listes_stations VARCHAR(50),
   temps_trajet TIME
);

CREATE TABLE ligne_commande(
   id_ligne_commande VARCHAR(50) PRIMARY KEY,
   id_commande VARCHAR(50),
   id_plat VARCHAR(50),
   date_livraison VARCHAR(50),
   adresse_livraison VARCHAR(50)
);

CREATE TABLE client(
   id_client INT PRIMARY KEY,
   montant_depense VARCHAR(50),
   type_ VARCHAR(50) NOT NULL,
   FOREIGN KEY(type_) REFERENCES type_client(type_)
);

CREATE TABLE plat_(
   num_plat VARCHAR(50) PRIMARY KEY,
   id_nom CHAR(50),
   date_fabrication DATE,
   nb_pers INT,
   date_peremption DATE,
   prix_par_pers INT,
   nationalite CHAR(50),
   regime VARCHAR(50),
   ingredients VARCHAR(50),
   photo TEXT,
   id_recette VARCHAR(50),
   id_cuisinier INT NOT NULL,
   id_type_plat INT NOT NULL,
   FOREIGN KEY(id_cuisinier) REFERENCES cuisinier(id_cuisinier),
   FOREIGN KEY(id_type_plat) REFERENCES type_plat(id_type_plat)
);

CREATE TABLE livraison(
   id_livraison INT PRIMARY KEY,
   date_livraison DATE,
   statut_livraison VARCHAR(50),
   id_ligne_commande VARCHAR(50) NOT NULL,
   id_cuisinier INT NOT NULL,
   FOREIGN KEY(id_ligne_commande) REFERENCES ligne_commande(id_ligne_commande),
   FOREIGN KEY(id_cuisinier) REFERENCES cuisinier(id_cuisinier)
);

CREATE TABLE commande(
   num_commande INT PRIMARY KEY,
   id_client INT NOT NULL,
   date_commande VARCHAR(50),
   chemin_livraison VARCHAR(50),
   prix_total DECIMAL(15,2),
   FOREIGN KEY(id_client) REFERENCES client(id_client)
);

CREATE TABLE utilisateur(
   num_id INT PRIMARY KEY,
   adresse VARCHAR(50),
   num_tel VARCHAR(50),
   nom_user VARCHAR(50),
   id_plateforme VARCHAR(50),
   mdp_plateforme VARCHAR(50),
   metro_prox VARCHAR(50),
   id_client INT NOT NULL,
   id_cuisinier INT NOT NULL,
   FOREIGN KEY(id_client) REFERENCES client(id_client),
   FOREIGN KEY(id_cuisinier) REFERENCES cuisinier(id_cuisinier)
);

CREATE TABLE est_composé(
   num_plat VARCHAR(50),
   num_commande INT,
   PRIMARY KEY(num_plat, num_commande),
   FOREIGN KEY(num_plat) REFERENCES plat_(num_plat),
   FOREIGN KEY(num_commande) REFERENCES commande(num_commande)
);

CREATE TABLE chemin(
   id_livraison INT,
   num_trajet INT,
   PRIMARY KEY(id_livraison, num_trajet),
   FOREIGN KEY(id_livraison) REFERENCES livraison(id_livraison),
   FOREIGN KEY(num_trajet) REFERENCES trajet(num_trajet)
);
