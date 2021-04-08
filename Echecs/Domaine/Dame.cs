using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echecs.IHM;

namespace Echecs.Domaine
{
    public class Dame : Piece
    {
        public Dame(Joueur joueur) : base(joueur, TypePiece.Dame) { }

        public override bool Deplacer(Case destination)
        {
            if (IsMoveLegal(destination))
            {
                joueur.partie.canEp = false;
                destination.Link(this);
                return true;
            }
            return false;
        }
        private bool IsMoveLegal(Case destination)
        {
            if (destination == position) return false;
            //this.joueur.pieces.Find(x=>x.info.type == TypePiece.Roi)
            if (this.joueur.isRoiEnEchec(this.info, this.position, destination)) return false;

            int diffRow = destination.row - this.position.row;
            int diffCol = destination.col - this.position.col;
            if(     diffRow == 0         // Horizontal
                ||  diffCol == 0         // Vertical
                ||  diffRow ==  diffCol  // Bas-droite
                ||  diffRow == -diffCol  // Bas-Gauche
                || -diffRow ==  diffCol  // Haut-droite
                || -diffRow == -diffCol) // Haut-gauche
            {
                int rowDir = 0;
                int colDir = 0;
                int nbCase = 0;

                if (diffRow == 0)
                {
                    nbCase = diffCol;
                    colDir = diffCol / Math.Abs(diffCol);
                }
                else if (diffCol == 0)
                {
                    nbCase = diffRow;
                    rowDir = diffRow / Math.Abs(diffRow);
                }
                else
                {
                    rowDir = diffRow / Math.Abs(diffRow);
                    colDir = diffCol / Math.Abs(diffCol);
                    nbCase = (diffRow == 0) ? diffCol : diffRow;
                }

                for (int i = 1; i < Math.Abs(nbCase)-1; i++)
                {
                    int row = this.position.row + (i * rowDir);
                    int col = this.position.col + (i * colDir);
                    Case c = this.position.echiquier.cases[row,col];//Row:Col
                    if (c.piece != null) return false;
                }
                return true;
            }
            return false;
        }
    }
}
