using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    public class Rota : IComparable<Rota>
    {
        public int idCidade1, idCidade2, preco, distancia, tempo;
        

        public Rota(int idCidade1, int idCidade2, int distancia, int tempo,int preco)
        {
            this.idCidade1 = idCidade1;
            this.idCidade2 = idCidade2;
            this.preco = preco;
            this.distancia = distancia;
            this.tempo = tempo;

        }
        public int CompareTo(Rota outro)
        {
            return this.distancia - outro.distancia;

        }
    }
}
