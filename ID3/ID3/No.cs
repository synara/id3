
using System.Collections;
using System.Collections.Generic;

namespace ID3
{
    public class No
    {
        public ArrayList Filhos { get; set; }
        public Atributo Atributo { get; set; }

        //Atributo ao qual o nó está ligado.
        public No(Atributo atributo)
        {
            if (atributo.Valores != null)
            {
                Filhos = new ArrayList(atributo.Valores.Count);
                for (int i = 0; i < atributo.Valores.Count; i++)
                    Filhos.Add(null);
            } else
            {
                Filhos = new ArrayList(1);
                Filhos.Add(null);
            }

            this.Atributo = atributo;
        }

        public void AdicionarNo(No no, string nome)
        {
            int index = Atributo.indexValue(nome);
            Filhos[index] = no;
        }

        public No getFilho(int index)
        {
            return (No)Filhos[index];
        }


        //retorna o filho de um nó pelo galho que leva até ele
        public No getFilhoPeloGalho(string galho)
        {
            return (No)Filhos[Atributo.indexValue(galho)];
        }
    }
}
