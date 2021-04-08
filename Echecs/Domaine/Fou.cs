using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echecs.IHM;

namespace Echecs.Domaine
{
    public class Fou : Piece
    {
        public Fou(Joueur joueur) : base(joueur, TypePiece.Fou) { }

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
            if (this.joueur.isRoiEnEchec(this.info, this.position, destination)) return false;
            int diffRow = destination.row - this.position.row;
            int diffCol = destination.col - this.position.col;
            if (    diffRow ==  diffCol  // Bas-droite
                ||  diffRow == -diffCol  // Bas-Gauche
                || -diffRow ==  diffCol  // Haut-droite
                || -diffRow == -diffCol) // Haut-gauche
            {
                int rowDir = diffRow / Math.Abs(diffRow);
                int colDir = diffCol / Math.Abs(diffCol);
                for (int i = 1; i < Math.Abs(diffRow) - 1; i++)
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
