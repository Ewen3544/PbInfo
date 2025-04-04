CREATE DATABASE IF NOT EXISTS livinparis;
USE livinparis;


SELECT User, Host FROM mysql.user WHERE User = 'supercaniveau';
DROP USER IF EXISTS 'supercaniveau'@'localhost';
CREATE USER IF NOT EXISTS 'supercaniveau'@'localhost' IDENTIFIED BY '1234';
SELECT User, Host, authentication_string FROM mysql.user WHERE User = 'supercaniveau';
GRANT ALL PRIVILEGES ON LivinParis.* TO 'supercaniveau'@'localhost';
FLUSH PRIVILEGES;

SELECT nom, ville from Utilisateur WHERE email = 'ewen';



CREATE TABLE Utilisateur (
    id_Utilisateur INT PRIMARY KEY AUTO_INCREMENT,
    Nom VARCHAR(50) NOT NULL,
    Prenom VARCHAR(50) NOT NULL,
    Telephone VARCHAR(15) NOT NULL,
    Rue VARCHAR(50) NOT NULL,
    Numero_Adresse INT NOT NULL,
    Code_Postal VARCHAR(10) NOT NULL,
    Ville VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Metro_Proche VARCHAR(50),
    MotDePasse VARCHAR(100) NOT NULL,
    Role VARCHAR(20) NOT NULL
);

CREATE TABLE Client (
    id_Client INT PRIMARY KEY AUTO_INCREMENT,
    Regime VARCHAR(50),
    id_Utilisateur INT NOT NULL,
    FOREIGN KEY (id_Utilisateur) REFERENCES Utilisateur(id_Utilisateur)
);

CREATE TABLE Cuisinier (
    id_Cuisinier INT PRIMARY KEY AUTO_INCREMENT,
    Specialite VARCHAR(50) NOT NULL,
    id_Utilisateur INT NOT NULL,
    FOREIGN KEY (id_Utilisateur) REFERENCES Utilisateur(id_Utilisateur)
);

CREATE TABLE Plat (
    id_Plat INT PRIMARY KEY AUTO_INCREMENT,
    Nom VARCHAR(50) NOT NULL,
    TypePlat VARCHAR(20) NOT NULL,
    NbPersonnes INT NOT NULL,
    DateFabrication DATE NOT NULL,
    DatePeremption DATE NOT NULL,
    PrixParPersonne DECIMAL(15,2) NOT NULL,
    Nationalite VARCHAR(50) NOT NULL,
    RegimeAlimentaire VARCHAR(50) NOT NULL,
    Ingredients TEXT NOT NULL,
    id_Cuisinier INT NOT NULL,
    FOREIGN KEY (id_Cuisinier) REFERENCES Cuisinier(id_Cuisinier)
);

CREATE TABLE Commande (
    id_Commande INT PRIMARY KEY AUTO_INCREMENT,
    Date_Commande DATETIME NOT NULL,
    Prix_Commande DECIMAL(15,2) NOT NULL,
    Statut VARCHAR(20) NOT NULL,
    id_Client INT NOT NULL,
    FOREIGN KEY (id_Client) REFERENCES Client(id_Client)
);

CREATE TABLE Livraison (
    id_Livraison INT PRIMARY KEY AUTO_INCREMENT,
    Statut VARCHAR(20) NOT NULL,
    Date_Livraison DATETIME,
    Adresse_Livraison VARCHAR(100) NOT NULL,
    id_Commande INT NOT NULL,
    FOREIGN KEY (id_Commande) REFERENCES Commande(id_Commande)
);

CREATE TABLE Contient (
    id_Commande INT,
    id_Plat INT,
    Quantite INT NOT NULL,
    PRIMARY KEY (id_Commande, id_Plat),
    FOREIGN KEY (id_Commande) REFERENCES Commande(id_Commande),
    FOREIGN KEY (id_Plat) REFERENCES Plat(id_Plat)
);

CREATE TABLE Avis_Client (
    id_Avis INT PRIMARY KEY AUTO_INCREMENT,
    Note DECIMAL(3,1) NOT NULL,
    Commentaire TEXT,
    Date_Avis DATETIME NOT NULL,
    id_Client INT NOT NULL,
    id_Plat INT NOT NULL,
    FOREIGN KEY (id_Client) REFERENCES Client(id_Client),
    FOREIGN KEY (id_Plat) REFERENCES Plat(id_Plat)
);

CREATE TABLE Favoris (
    id_Client INT NOT NULL,
    id_Plat INT NOT NULL,
    Date_Ajout DATETIME DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id_Client, id_Plat),
    FOREIGN KEY (id_Client) REFERENCES Client(id_Client) ON DELETE CASCADE,
    FOREIGN KEY (id_Plat) REFERENCES Plat(id_Plat) ON DELETE CASCADE
);

CREATE TABLE MenuDuJour (
    id_Menu INT PRIMARY KEY AUTO_INCREMENT,
    DateMenu DATE NOT NULL UNIQUE,
    id_PlatPrincipal INT NOT NULL,
    id_Dessert INT,
    Description TEXT,
    FOREIGN KEY (id_PlatPrincipal) REFERENCES Plat(id_Plat),
    FOREIGN KEY (id_Dessert) REFERENCES Plat(id_Plat)
);


-- Insertion des utilisateurs
INSERT INTO Utilisateur (Nom, Prenom, Telephone, Rue, Numero_Adresse, Code_Postal, Ville, Email, Metro_Proche, MotDePasse, Role) VALUES
('Dupont', 'Jean', '0612345678', 'Rue de Paris', 10, '75001', 'Paris', 'jean.dupont@email.com', 'Châtelet', 'mdp123', 'Client'),
('Martin', 'Sophie', '0623456789', 'Avenue de Lyon', 25, '69002', 'Lyon', 'sophie.martin@email.com', 'Bellecour', 'mdp456', 'Client'),
('Bernard', 'Luc', '0634567890', 'Boulevard Saint-Michel', 18, '75005', 'Paris', 'luc.bernard@email.com', 'Luxembourg', 'mdp789', 'Cuisinier');

INSERT INTO Client (Regime, id_Utilisateur) VALUES
('Végétarien', 1),
('Sans gluten', 2);

INSERT INTO Cuisinier (Specialite, id_Utilisateur) VALUES
('Cuisine française', 3);

INSERT INTO Plat (Nom, TypePlat, NbPersonnes, DateFabrication, DatePeremption, PrixParPersonne, Nationalite, RegimeAlimentaire, Ingredients, id_Cuisinier) VALUES
('Ratatouille', 'Plat principal', 2, '2025-04-01', '2025-04-05', 12.50, 'Française', 'Végétarien', 'Tomates, courgettes, aubergines', 1),
('Tarte aux pommes', 'Dessert', 4, '2025-04-02', '2025-04-06', 8.00, 'Française', 'Sans gluten', 'Pommes, pâte sans gluten, sucre', 1);

INSERT INTO Commande (Date_Commande, Prix_Commande, Statut, id_Client) VALUES
('2025-04-02 12:00:00', 25.00, 'En cours', 1),
('2025-04-03 18:30:00', 16.00, 'Livrée', 2);

INSERT INTO Livraison (Statut, Date_Livraison, Adresse_Livraison, id_Commande) VALUES
('En attente', NULL, '10 Rue de Paris, 75001 Paris', 1),
('Livrée', '2025-04-03 20:00:00', '25 Avenue de Lyon, 69002 Lyon', 2);

INSERT INTO Contient (id_Commande, id_Plat, Quantite) VALUES
(1, 1, 2),
(2, 2, 1);

INSERT INTO Avis_Client (Note, Commentaire, Date_Avis, id_Client, id_Plat) VALUES
(4.5, 'délicieux!!!!!', '2025-04-03 14:00:00', 1, 1),
(3.8, 'Plutot pas mal cette tarte', '2025-04-04 10:30:00', 2, 2);

INSERT INTO Favoris (id_Client, id_Plat) VALUES
(1, 1),
(2, 2);

INSERT INTO MenuDuJour (DateMenu, id_PlatPrincipal, id_Dessert, Description) VALUES
('2025-04-05', 1, 2, 'Le magnifique menu du jour');
