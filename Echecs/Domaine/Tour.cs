using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echecs.IHM;

namespace Echecs.Domaine
{
    public class Tour : Piece
    {
        public Tour(Joueur joueur) : base(joueur, TypePiece.Tour) { }
        public bool canCastle = true;

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
            if (    diffRow == 0  // Horizontal
                ||  diffCol == 0) // Vertical
            {
                int nbCase = 0;
                int colDir = 0;
                int rowDir = 0;
                if (diffRow == 0)
                {
                    nbCase = diffCol;
                    colDir = diffCol / Math.Abs(diffCol);
                }
                else
                {
                    nbCase = diffRow;
                    rowDir = diffRow / Math.Abs(diffRow);
                }

                for (int i = 1; i < Math.Abs(nbCase) - 1; i++)
                {
                    int row = this.position.row + (i * rowDir);
                    int col = this.position.col + (i * colDir);
                    Case c = this.position.echiquier.cases[row, col];//Row:Col
                    if (c.piece != null) return false;
                }
                return true;
            }
            return false;
        }
    }
}
