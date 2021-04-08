using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echecs.IHM;

namespace Echecs.Domaine
{
    public class Case
    {
        // attributs
        public CouleurCamp couleurCase;
        public int row, col;

        // associations
        public Piece piece;
        public Echiquier echiquier;

        // methodes
        public Case(CouleurCamp couleur, int r, int c, Echiquier echiquier)
        {
            this.couleurCase = couleur;
            this.row = r;
            this.col = c;
            this.piece = null;
            this.echiquier = echiquier;
        }
        public void Link(Piece newPiece)
        {
            // 1. Deconnecter newPiece de l'ancienne case
            if(newPiece != null) {
                if (newPiece.position != null)
                    newPiece.position.Unlink();
                // 2. Connecter newPiece à cette case
                if (this.piece != null) // Si la case n'est pas vide, c'est une capture
                {
                    this.echiquier.partie.piecesCapturees.Add(this.piece.info);
                }
                this.piece = newPiece;
                this.piece.position = this;
                this.echiquier.partie.vue.ActualiserCase(col, row, piece.info);
            }
        }
        public void Unlink()
        {
            this.piece.position = null;
            this.piece = null;
            this.echiquier.partie.vue.ActualiserCase(col, row, null);
        }
    }
}
