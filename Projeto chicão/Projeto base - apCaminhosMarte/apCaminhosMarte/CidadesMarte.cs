using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    public class CidadesMarte : IComparable<CidadesMarte>
    {
        public int id, x, y;
        public String nome;

        public CidadesMarte(int id, int x, int y, String nome)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.nome = nome;
           
        }
        public  int CompareTo(CidadesMarte outro)
        {
            return this.id - outro.id;

        }
    }
}
