using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echecs.IHM;

namespace Echecs.Domaine
{
    public class Roi : Piece
    {
        private bool canCastle = true;
        public Roi(Joueur joueur) : base(joueur, TypePiece.Roi) { }

        public override bool Deplacer(Case destination)
        {
            if (IsMoveLegal(destination))
            {
                canCastle = false;
                joueur.partie.canEp = false;
                destination.Link(this);
                return true;
            }
            return false;
        }
        private bool IsMoveLegal(Case destination)
        {
            if (destination == position) return false;
            if (this.joueur.isRoiEnEchec(this.info, this.position, destination)) return false;
            int diffRow = destination.row - this.position.row;
            int diffCol = destination.col - this.position.col;
            if (isCastle(diffRow, diffCol)) return true;
            if ((diffRow == 1 || diffRow == -1 || diffRow == 0) && 
                (diffCol == 1 || diffCol == -1 || diffCol == 0)) return true;
            return false;
        }

        private bool isCastle(int diffRow, int diffCol)
        {
            if (diffRow == 0 && diffCol == 2 && canCastle) //Vers la droite
            {
                Piece tour = this.position.echiquier.cases[this.position.row, this.position.col + 3].piece;
                if (tour != null && tour.info.type == TypePiece.Tour && ((Tour)tour).canCastle)
                {
                    Piece piece1 = this.position.echiquier.cases[this.position.row, this.position.col + 1].piece;
                    Piece piece2 = this.position.echiquier.cases[this.position.row, this.position.col + 2].piece;
                    if (piece1 == null && piece2 == null)
                    {
                        this.position.echiquier.cases[this.position.row, this.position.col + 1].Link(tour);
                        return true;
                    }
                }
            }
            else if (diffRow == 0 && diffCol == -2 && canCastle) //Vers la gauche
            {
                Piece tour = this.position.echiquier.cases[this.position.row, this.position.col - 4].piece;
                if (tour != null && tour.info.type == TypePiece.Tour && ((Tour)tour).canCastle)
                {
                    Piece piece1 = this.position.echiquier.cases[this.position.row, this.position.col - 1].piece;
                    Piece piece2 = this.position.echiquier.cases[this.position.row, this.position.col - 2].piece;
                    Piece piece3 = this.position.echiquier.cases[this.position.row, this.position.col - 3].piece;
                    if (piece1 == null && piece2 == null && piece3 == null)
                    {
                        this.position.echiquier.cases[this.position.row, this.position.col - 1].Link(tour);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CanBeAttacked(InfoPiece infos, Case depart, Case arrivee)
        {
            #region Directions
            //row, col
            Tuple<int, int> hautGauche  = new Tuple<int, int>(-1, -1);
            Tuple<int, int> haut        = new Tuple<int, int>(-1,  0);
            Tuple<int, int> hautDroite  = new Tuple<int, int>(-1,  1);
            Tuple<int, int> droite      = new Tuple<int, int>( 0,  1);
            Tuple<int, int> basDroite   = new Tuple<int, int>( 1,  1);
            Tuple<int, int> bas         = new Tuple<int, int>( 1,  0);
            Tuple<int, int> basGauche   = new Tuple<int, int>( 1, -1);
            Tuple<int, int> gauche      = new Tuple<int, int>( 0, -1);

            Tuple<int, int> cavalier1 = new Tuple<int, int>( 1,  2);
            Tuple<int, int> cavalier2 = new Tuple<int, int>( 1, -2);
            Tuple<int, int> cavalier3 = new Tuple<int, int>( 2,  1);
            Tuple<int, int> cavalier4 = new Tuple<int, int>( 2, -1);
            Tuple<int, int> cavalier5 = new Tuple<int, int>(-1,  2);
            Tuple<int, int> cavalier6 = new Tuple<int, int>(-1, -2);
            Tuple<int, int> cavalier7 = new Tuple<int, int>(-2,  1);
            Tuple<int, int> cavalier8 = new Tuple<int, int>(-2, -1);

            List<Tuple<int, int>> directions = new List<Tuple<int, int>>();
            directions.Add(hautGauche); directions.Add(cavalier1);
            directions.Add(haut);       directions.Add(cavalier2);
            directions.Add(hautDroite); directions.Add(cavalier3);
            directions.Add(droite);     directions.Add(cavalier4);
            directions.Add(basDroite);  directions.Add(cavalier5);
            directions.Add(bas);        directions.Add(cavalier6);
            directions.Add(basGauche);  directions.Add(cavalier7);
            directions.Add(gauche);     directions.Add(cavalier8);
            #endregion

            int row = this.position.row;
            int col = this.position.col;

            //Si c'est le roi qui a bouger, prendre en compte la nouvelle pos
            if (infos.type == TypePiece.Roi) 
            {
                row = arrivee.row;
                col = arrivee.col;
            }

            //Pour chaque direction
            foreach (Tuple<int,int> dir in directions)
            {
                //Slide
                int newRow = row;
                int newCol = col;
                while (true)
                {
                    newRow += dir.Item1;
                    newCol += dir.Item2;
                    //Si hors plateau, passer à la direction suivante
                    if (newRow < 0 || newRow > 7 || newCol < 0 || newCol > 7) break;
                    //On récupère la case à tester et la piece sur cette case
                    Case cSlide = this.joueur.partie.echiquier.cases[newRow, newCol];
                    Piece p = cSlide.piece;
                    //Si vide ou ancienne pos et pas nouvelle pos, continuer a glisser, 
                    //On considére l'ancienne case comme vide et la nouvelle comme remplis
                    if ((p == null && ((cSlide != arrivee)) || (cSlide == depart)))
                    {
                        if (dir.Item1 == 2  ||
                            dir.Item1 == -2 ||
                            dir.Item2 == 2  ||
                            dir.Item2 == -2)
                            break; //Si c'est une position de canasson, on ne glisse pas
                        continue;  //On passe à la suivante
                    }
                    //Si allié ou case d'arrivée, passer à la direction suivante
                    //On considere la nouvelle case comme occupé par un allié
                    if ((cSlide == arrivee) || (p != null && p.info.couleur == this.joueur.couleur)) break;
                    //Sinon ennemi, regarder le type pour voir s'il peut se déplacer dans cette direction
                    else
                    {
                        TypePiece pieceType = p.info.type;
                        //Si oui, return true
                        if (dir.Item1 == 2 || dir.Item1 == -2 || dir.Item2 == 2 || dir.Item2 == -2)
                        { // Si c'est une dir de canasson,
                            if (pieceType == TypePiece.Cavalier) return true; //et un canasson, le roi peut être attaqué
                            //continue; //Si c'en est pas un, c'est safe
                        }
                        else if (pieceType == TypePiece.Dame)
                        {
                            return true; //Peu importe la direction ou la distance 
                        }
                        else if (pieceType == TypePiece.Roi)
                        {    //Si c'est un roi
                            if (Math.Abs(newRow - row) <= 1 &&  //Distance horizontal <= 1 et
                                Math.Abs(newCol - col) <= 1)    //Distance vertical <= 1
                                return true;
                        }
                        else if (pieceType == TypePiece.Fou)
                        {
                            if (dir.Item1 != 0 && dir.Item2 != 0) return true;
                        }
                        else if (pieceType == TypePiece.Tour)//Si c'est une tour
                        {         
                            if (dir.Item1 == 0 && dir.Item2 != 1 ||   //Horizontal
                                dir.Item1 != 1 && dir.Item2 == 0)     //Vertical
                                return true;
                        }
                        //Si c'est un pion
                        else if (pieceType == TypePiece.Pion)
                        {
                            if (p.info.couleur == CouleurCamp.Blanche) //De couleur blanche
                            {
                                //Une case en dessous et une colonne à côté
                                if (newRow == row + 1 && (newCol == col + 1 || newCol == col - 1))
                                    return true;
                            }
                            else //De couleur noir
                            {
                                //Une case au dessous et une colonne à côté
                                if (newRow == row - 1 && (newCol == col + 1 || newCol == col - 1))
                                    return true;
                            }
                        }
                        //Si non, passer à la direction suivante
                        break;
                    }
                }
            }
            return false;
        }
    }
}
