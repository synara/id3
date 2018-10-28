using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3
{
    public class ArvoreDecisao
    {
        public DataTable Amostras { get; set; }
        public int TotalAltos { get; set; }
        public int TotalBaixos { get; set; }
        public int TotalModerados { get; set; }
        public int Total { get; set; }
        public string RotuloAtributo { get; set; }
        private double Entropia { get; set; }


        private int totalRisco(DataTable amostras, string rotulo)
        {
            int resultado = 0;
            foreach (DataRow row in amostras.Rows)
            {
                if ((string)row[RotuloAtributo] == rotulo)
                    resultado++;
            }

            return resultado;
        }

        private double calcularEntropia(int altos, int baixos, int moderados)
        {
            int total = altos + baixos + moderados;
            double proporcaoAlta = (double)altos / total;
            double proporcaoBaixa = (double)baixos / total;
            double proporcaoModerada = (double)moderados / total;


            if (proporcaoAlta != 0)
                proporcaoAlta = -(proporcaoAlta) * Math.Log(proporcaoAlta, 2);

            if (proporcaoBaixa != 0)
                proporcaoBaixa = -(proporcaoBaixa) * Math.Log(proporcaoBaixa, 2);

            if (proporcaoModerada != 0)
                proporcaoModerada = -(proporcaoModerada) * Math.Log(proporcaoModerada, 2);

            return proporcaoAlta + proporcaoBaixa + proporcaoModerada;
        }


        private void getValoresDoAtributo(DataTable amostras, Atributo atributo, string rotulo, out int altos, out int baixos, out int moderados)
        {
            altos = 0;
            baixos = 0;
            moderados = 0;

            foreach (DataRow row in amostras.Rows)
            {
                if ((string)row[atributo.Nome] == rotulo)
                {
                    if ((string)row[RotuloAtributo] == "alto")
                       altos++;
                    else if ((string)row[RotuloAtributo] == "baixo")
                        baixos++;
                    else if ((string)row[RotuloAtributo] == "moderado")
                        moderados++;
                }
            }

        }

        private double ganho(DataTable amostras, Atributo atributo)
        {
            ArrayList valores = atributo.Valores;
            double soma = 0.0;

            for (int i = 0; i < valores.Count; i++)
            {
                int altos, baixos, moderados = 0;

                getValoresDoAtributo(amostras, atributo, valores[i].ToString(), out altos, out baixos, out moderados);

                double entropia = calcularEntropia(altos, baixos, moderados);
                soma += -(double)(altos + baixos + moderados) / Total * entropia;
            }

            return this.Entropia + soma;
        }



        //retorna o melhor atributo
        private Atributo getMelhorAtributo(DataTable amostras, Atributo[] atributos)
        {
            double maximoGanho = 0.0;
            Atributo resultado = null;

            foreach (Atributo atributo in atributos)
            {
                double aux = ganho(amostras, atributo);
                if (aux > maximoGanho)
                {
                    maximoGanho = aux;
                    resultado = atributo;
                }
            }

            return resultado;
        }


        private ArrayList verificaSeTodosPertecemAMesmaClasse(DataTable amostras, string rotulo)
        {
            return getValoresDistintos(amostras, rotulo);
        }

        //retorna uma lista com todos os valores distintos da tabela de amostragem
        private ArrayList getValoresDistintos(DataTable amostras, string rotulo)
        {
            ArrayList valoresDistintos = new ArrayList(amostras.Rows.Count);

            foreach (DataRow row in amostras.Rows)
            {
                if (valoresDistintos.IndexOf(row[rotulo]) == -1)
                    valoresDistintos.Add(row[rotulo]);
            }

            return valoresDistintos;
        }

        //retorna o valor mais comum dentro de uma amostragem
        private object getValorMaisComum(DataTable amostras, string rotulo)
        {
            ArrayList valoresDistintos = getValoresDistintos(amostras, rotulo);
            int[] contador = new int[valoresDistintos.Count];

            foreach (DataRow row in amostras.Rows)
            {
                int index = valoresDistintos.IndexOf(row[rotulo]);
                contador[index]++;
            }

            int maximoIndex = 0;
            int contadorMaximo = 0;

            for (int i = 0; i < contador.Length; i++)
            {
                if (contador[i] > contadorMaximo)
                {
                    contadorMaximo = contador[i];
                    maximoIndex = i;
                }
            }

            return valoresDistintos[maximoIndex];

        }

        private No montarArvoreInterna(DataTable amostras, string rotulo, Atributo[] atributos)
        {

            if (verificaSeTodosPertecemAMesmaClasse(amostras, rotulo).Count == 1)
                return new No(new Atributo(amostras.Rows[0]));

            if (atributos.Length == 0)
                return new No(new Atributo(getValorMaisComum(amostras, rotulo)));

            this.Total = amostras.Rows.Count;
            this.RotuloAtributo = rotulo;
            this.TotalAltos = totalRisco(amostras, "alto");
            this.TotalBaixos = totalRisco(amostras, "baixo");
            this.TotalModerados = totalRisco(amostras, "moderado");

            this.Entropia = calcularEntropia(this.TotalAltos, this.TotalBaixos, this.TotalModerados);
            Atributo melhorAtributo = getMelhorAtributo(amostras, atributos);

            No raiz = new No(melhorAtributo);

            DataTable amostra = amostras.Clone();

            foreach (var item in melhorAtributo.Valores)
            {

                #region Seleciona todas os elementos com o valor deste atributo
                amostra.Rows.Clear();
                DataRow[] rows = amostras.Select(melhorAtributo.Nome + " = " + "'"+ item.ToString() + "'");

                foreach (DataRow row in rows)
                {
                    amostra.Rows.Add(row.ItemArray);

                }
                #endregion

                #region Cria uma nova lista de atributos menos o atributo corrente que é o melhor atributo
                ArrayList atrbts = new ArrayList(atributos.Length - 1);
                for (int i = 0; i < atributos.Length; i++)
                {
                    if (atributos[i].Nome != melhorAtributo.Nome)
                        atrbts.Add(atributos[i]);
                }
                #endregion

                if (amostra.Rows.Count == 0)
                {
                    return new No(new Atributo(getValorMaisComum(amostra, rotulo)));
                }
                else
                {
                    ArvoreDecisao id3 = new ArvoreDecisao();
                    No noFilho = id3.montarArvore(amostra, rotulo, (Atributo[])atrbts.ToArray(typeof(Atributo)));
                    raiz.AdicionarNo(noFilho, item.ToString());
                }

            }

            return raiz;
        }

        public No montarArvore(DataTable amostras, string rotulo, Atributo[] atributos)
        {
            this.Amostras = amostras;
            return montarArvoreInterna(this.Amostras, rotulo, atributos);
        }


    }
}
