using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Echecs.Domaine
{
    public class Echiquier
    {
        public Case[,] cases { get; set; }
        public Partie partie { get; set; }

        public Echiquier(Partie partie)
        {
            this.partie = partie;
            this.cases = new Case[8, 8];
            InitEmptyCases();
        }
        private void InitEmptyCases()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    IHM.CouleurCamp couleurCase = (row + col) % 2 == 0 ?
                        IHM.CouleurCamp.Blanche : IHM.CouleurCamp.Noire ;
                    cases[row, col] = new Case(couleurCase, row, col, this);
                    partie.vue.ActualiserCase(col, row, null);
                }
            }
        }
    }
}
