using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echecs.IHM;

namespace Echecs.Domaine
{
    public class Joueur
    {
        // attributs
        public CouleurCamp couleur;
        private Roi roi;

        // associations
        public Partie partie;
        public List<Piece> pieces = new List<Piece>();

        // methodes
        public Joueur(Partie partie, CouleurCamp couleur)
        {
            this.couleur = couleur;
            this.partie = partie;

            // TODO : creation des pieces du joueur
            pieces.Add(new Roi(this));
            pieces.Add(new Dame(this));
            for (int i = 0; i < 2; i++)
            {
                pieces.Add(new Tour(this));
                pieces.Add(new Cavalier(this));
                pieces.Add(new Fou(this));
            }
            for (int i = 0; i < 8; i++)
            {
                pieces.Add(new Pion(this));
            }
        }

        // TODO : décommentez lorsque vous auriez implementé les methode Unlink et Link de la classe Case
        public void PlacerPieces(Echiquier echiquier)
        {
            int nbTour = 0, nbCavalier = 0, nbFou = 0, nbPion = 0;
            foreach (Piece piece in pieces)
            {
                switch (piece.info.type)
                {
                    case TypePiece.Roi:
                        if (couleur == CouleurCamp.Blanche) echiquier.cases[7, 4].Link(piece);
                        else echiquier.cases[0, 4].Link(piece);
                        roi = (Roi)piece;
                        break;
                    case TypePiece.Dame:
                        if (couleur == CouleurCamp.Blanche) echiquier.cases[7, 3].Link(piece);
                        else echiquier.cases[0, 3].Link(piece);
                        break;
                    case TypePiece.Tour:
                        if (couleur == CouleurCamp.Blanche) { echiquier.cases[7, (nbTour * 7)].Link(piece); nbTour++; }
                        else { echiquier.cases[0, (nbTour * 7)].Link(piece); nbTour++; }
                        break;
                    case TypePiece.Cavalier:
                        if (couleur == CouleurCamp.Blanche) { echiquier.cases[7, ((nbCavalier * 5) + 1)].Link(piece); nbCavalier++; }
                        else { echiquier.cases[0, ((nbCavalier * 5) + 1)].Link(piece); nbCavalier++; }
                        break;
                    case TypePiece.Fou:
                        if (couleur == CouleurCamp.Blanche) { echiquier.cases[7, ((nbFou * 3) + 2)].Link(piece); nbFou++; }
                        else { echiquier.cases[0, ((nbFou * 3) + 2)].Link(piece); nbFou++; }
                        break;
                    case TypePiece.Pion:
                        if (couleur == CouleurCamp.Blanche) { echiquier.cases[6, nbPion].Link(piece); nbPion++; }
                        else { echiquier.cases[1, nbPion].Link(piece); nbPion++; }
                        break;
                    default: break;
                }
            }
        }

        public bool isRoiEnEchec(InfoPiece infos, Case depart, Case arrivee)
        {
            return roi.CanBeAttacked(infos, depart, arrivee);
        }
    }
}
