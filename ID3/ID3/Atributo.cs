
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ID3
{
    public class Atributo
    {
        public ArrayList Valores { get; set; }
        public string Nome { get; set; }
        public string Rotulo { get; set; }

        public Atributo(string nome, ArrayList valores)
        {
            this.Nome = nome;
            this.Valores = valores;
            this.Valores.Sort();
        }

        public Atributo(object rotulo)
        {
            var rtl = (DataRow)rotulo;
            this.Rotulo = rtl.ItemArray[0].ToString();
            this.Nome = null;
            this.Valores = null;
        }


        public int indexValue(string nome)
        {
            if (Valores.Count > 0)
                return Valores.BinarySearch(nome);
            else
                return -1;
        }

    }
}
