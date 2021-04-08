using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echecs.IHM;

namespace Echecs.Domaine
{
    public class Cavalier : Piece
    {
        public Cavalier(Joueur joueur) : base(joueur, TypePiece.Cavalier) { }

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
            if ((diffRow == -1 || diffRow == 1) && (diffCol == 2 || diffCol == -2)) return true;
            if ((diffRow == -2 || diffRow == 2) && (diffCol == 1 || diffCol == -1)) return true;
            return false;
        }
    }
}
