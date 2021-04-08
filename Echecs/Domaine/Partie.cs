using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Echecs.IHM;

namespace Echecs.Domaine
{
    public class Partie : IJeu
    {
        public Case ep_Case = null;
        public Piece ep_Piece = null;
        public bool canEp = false;
        public int nbMove = 0;

        public IEvenements vue
        {
            get { return _vue; }
            set { _vue = value; }
        }
        StatusPartie status
        {
            get { return _status; }
            set
            {
                _status = value;
                vue.ActualiserPartie(_status);
            }
        }
        /* attributs */
        StatusPartie _status = StatusPartie.Reset;

        /* associations */
        IEvenements _vue;
        Joueur blancs;
        Joueur noirs;
        public Echiquier echiquier;  // TODO : décommentez lorsque vous auriez implementé la classe Echiquier
        public List<InfoPiece> piecesCapturees;
        /* methodes */

        public void CommencerPartie()
        {
            // creation des joueurs
            blancs = new Joueur(this, CouleurCamp.Blanche);
            noirs = new Joueur(this, CouleurCamp.Noire);

            // creation de l'echiquier
            echiquier = new Echiquier(this);
            piecesCapturees = new List<InfoPiece>();
            this.vue.ActualiserCaptures(piecesCapturees);

            // placement des pieces
            blancs.PlacerPieces(echiquier);  // TODO : décommentez lorsque vous auriez implementé les methode Unlink et Link de la classe Case
            noirs.PlacerPieces(echiquier);  // TODO : décommentez lorsque vous auriez implementé les methode Unlink et Link de la classe Case

            // initialisation de l'état
            status = StatusPartie.TraitBlancs;

            // initialisation du tableau des cases attaquables

            //Autres
            //this.vue.ActualiserListMove(new string[] { "test", "test", "test" });
            this.vue.ActualiserListMove(null);
        }

        public void DeplacerPiece(int x_depart, int y_depart, int x_arrivee, int y_arrivee)
        {
            // case de départ
            Case depart = echiquier.cases[y_depart, x_depart];

            // case d'arrivée
            Case destination = echiquier.cases[y_arrivee, x_arrivee];

            // deplacer
            bool ok = depart.piece.Deplacer(destination);
            if (!canEp) { ep_Case = null; ep_Piece = null; }

            // changer d'état
            if (ok)
            {
                nbMove++;
                if (!canEp) { ep_Case = null; ep_Piece = null; }
                this.vue.ActualiserCaptures(piecesCapturees);
                string pos = ((char)(x_arrivee + 'A')) + ((7-y_arrivee)+1).ToString();
                this.vue.ActualiserListMove(new string[] { nbMove.ToString(), "test", pos});
                ChangerEtat();
            }
        }

        void ChangerEtat(bool echec = false, bool mat = false)
        {
            if (status == StatusPartie.TraitBlancs)
            {
                status = StatusPartie.TraitNoirs;
            }
            else
            {
                status = StatusPartie.TraitBlancs;
            }
        }
    }
}
