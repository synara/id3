using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3
{
    public class ID3Amostra
    {
     
        public static void print(No raiz, string tabs)
        {
            Console.WriteLine(tabs + '|' + raiz.Atributo.Nome + '|');

            if(raiz.Atributo.Valores != null)
            {
                for (int i = 0; i < raiz.Atributo.Valores.Count; i++)
                {
                    Console.WriteLine(tabs + "\t" + "<" + raiz.Atributo.Valores[i] + ">");
                    No noFilho = raiz.getFilhoPeloGalho(raiz.Atributo.Valores[i].ToString());
                    print(noFilho, "\t" + tabs);

                }
            }
            else
            {
                Console.WriteLine(tabs + "\t" + "[" + raiz.Atributo.Rotulo + "]");
            }
        }


       
        [STAThread]
        static void Main(string[] args)
        {
            Atributo risco = new Atributo("risco", new ArrayList { "alto", "moderado", "baixo" });
            Atributo historico_credito = new Atributo("historico_credito", new ArrayList { "ruim", "desconhecida", "boa" });
            Atributo divida = new Atributo("divida", new ArrayList { "alta", "baixa" });
            Atributo garantia = new Atributo("garantia", new ArrayList { "nenhuma", "adequada" });
            Atributo renda = new Atributo("renda", new ArrayList{ "$0 a $15 mil", "$15 a $35 mil", "acima de $35 mil" });


            Atributo[] atributos = new Atributo[] { historico_credito, divida, garantia, renda };

            DataTable amostras = GetDataTable();

            ArvoreDecisao id3 = new ArvoreDecisao();
            No raiz = id3.montarArvore(amostras, "risco", atributos);

            print(raiz, "");
            Console.ReadKey();

        }

        static DataTable GetDataTable()
        {
            DataTable result = new DataTable("AmostraID3");
            DataColumn column = result.Columns.Add("risco");
            column.DataType = typeof(string);

            column = result.Columns.Add("historico_credito");
            column.DataType = typeof(string);

            column = result.Columns.Add("divida");
            column.DataType = typeof(string);

            column = result.Columns.Add("garantia");
            column.DataType = typeof(string);

            column = result.Columns.Add("renda");
            column.DataType = typeof(string);

            result.Rows.Add(new object[] { "alto", "ruim", "alta", "nenhuma", "$0 a $15 mil" });
            result.Rows.Add(new object[] { "alto", "desconhecida", "alta", "nenhuma", "$15 a $35 mil" });
            result.Rows.Add(new object[] { "moderado", "desconhecida", "baixa", "nenhuma", "$15 a $35 mil" });
            result.Rows.Add(new object[] { "alto", "desconhecida", "baixa", "nenhuma", "$0 a $15 mil" });
            result.Rows.Add(new object[] { "baixo", "desconhecida", "baixa", "nenhuma", "acima de $35 mil" });
            result.Rows.Add(new object[] { "baixo", "desconhecida", "baixa", "adequada", "acima de $35 mil" });
            result.Rows.Add(new object[] { "alto", "ruim", "baixa", "nenhuma", "$0 a $15 mil" });
            result.Rows.Add(new object[] { "moderado", "ruim", "baixa", "adequada", "acima de $35 mil" });
            result.Rows.Add(new object[] { "baixo", "boa", "baixa", "nenhuma", "acima de $35 mil" });
            result.Rows.Add(new object[] { "baixo", "boa", "alta", "adequada", "acima de $35 mil" });
            result.Rows.Add(new object[] { "alto", "boa", "alta", "nenhuma", "$0 a $15 mil" });
            result.Rows.Add(new object[] { "moderado", "boa", "alta", "nenhuma", "$15 a $35 mil" });
            result.Rows.Add(new object[] { "baixo", "boa", "alta", "nenhuma", "acima de $35 mil" });
            result.Rows.Add(new object[] { "alto", "ruim", "alta", "nenhuma", "$15 a $35 mil" });
            return result;

        }
    }
}
