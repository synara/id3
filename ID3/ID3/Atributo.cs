
using System.Collections;
using System.Collections.Generic;


namespace ID3
{
    public class Atributo
    {
        public ArrayList Valores { get; set; }
        public string Nome { get; set; }
        public object Rotulo { get; set; }
        public bool Iguais { get; set; }

        public Atributo(string nome, ArrayList valores)
        {
            this.Nome = nome;
            this.Valores = valores;
            this.Valores.Sort();
        }

        public Atributo(object rotulo)
        {
            this.Rotulo = rotulo;
            this.Nome = null;
            this.Valores = null;
        }

        public Atributo(string rotulo, bool valor)
        {
            this.Rotulo = rotulo;
            this.Iguais = valor;
        }

        public Atributo()
        {

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
