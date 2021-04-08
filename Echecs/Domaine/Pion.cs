using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echecs.IHM;

namespace Echecs.Domaine
{
    public class Pion : Piece
    {
        public Pion(Joueur joueur) : base(joueur, TypePiece.Pion) { }
        private bool isFirstMove = true;

        public override bool Deplacer(Case destination)
        {
            if (IsMoveLegal(destination))
            {
                destination.Link(this);
                isFirstMove = false;
                if(destination.row == 0 || destination.row == 7) Promote();                
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
            if (this.info.couleur == CouleurCamp.Blanche)
            {
                //On peut avancer de 2 au 1er déplacement si pas d'obstacle && actu ep
                if (isFirstMove && diffRow == -2 && diffCol == 0
                    && destination.piece == null
                    && this.position.echiquier.cases[this.position.row - 1, this.position.col].piece == null)
                {
                    this.position.echiquier.partie.ep_Case = this.position.echiquier.cases[this.position.row - 1, this.position.col];
                    this.position.echiquier.partie.ep_Piece = this;
                    this.joueur.partie.canEp = true;
                    return true;
                }
                //Ou de 1 vers l'avant si la case est vide
                if (diffRow == -1 && diffCol == 0 && destination.piece == null)
                {
                    this.joueur.partie.canEp = false;
                    return true;
                }
                //Ou si on avance de 1 en diagonal vers l'avant et qu'il y a une piece sur la case
                if (diffRow == -1 && (diffCol == 1 || diffCol == -1))
                {
                    if (destination == this.position.echiquier.partie.ep_Case)
                    {
                        this.position.echiquier.partie.piecesCapturees.Add(this.position.echiquier.partie.ep_Piece.info);                        
                        this.position.echiquier.partie.ep_Piece.position.Unlink();
                    }
                    else if(destination.piece == null) return false;
                    this.joueur.partie.canEp = false;
                    return true;
                }
            }
            else
            {
                //On peut avancer de 2 au 1er déplacement si pas d'obstacle && actu ep
                if (isFirstMove && diffRow == 2 && diffCol == 0
                    && destination.piece == null
                    && this.position.echiquier.cases[this.position.row + 1, this.position.col].piece == null)
                {
                    this.position.echiquier.partie.ep_Case = this.position.echiquier.cases[this.position.row + 1, this.position.col];
                    this.position.echiquier.partie.ep_Piece = this;
                    this.joueur.partie.canEp = true;
                    return true;
                }
                //Ou de 1 vers l'avant si la case est vide
                if (diffRow == 1 && diffCol == 0 && destination.piece == null)
                {
                    this.joueur.partie.canEp = false;
                    return true;
                }
                //Ou si on avance de 1 en diagonal vers l'avant et qu'il y a une piece sur la case
                if (diffRow == 1 && (diffCol == 1 || diffCol == -1))
                {
                    if(destination == this.position.echiquier.partie.ep_Case)
                    {
                        this.position.echiquier.partie.piecesCapturees.Add(this.position.echiquier.partie.ep_Piece.info);
                        this.position.echiquier.partie.ep_Piece.position.Unlink();
                    }
                    else if (destination.piece == null) return false;
                    this.joueur.partie.canEp = false;
                    return true;
                }
            }
            return false;
        }

        private void Promote()
        {
            Dame newDame = new Dame(this.joueur);
            this.position.Link(newDame);
            joueur.pieces.Remove(this);
            joueur.pieces.Add(newDame);
        }
    }
}
