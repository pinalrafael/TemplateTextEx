using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateText;
using TemplateText.Attributes;
using TemplateText.Util;

namespace TemplateTextEx
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "teste\n";
            str += "=========================================================\n";
            str += "id: {pedido.id}\n";
            str += "nome: {pedido.nome}\n";
            str += "data: {pedido.data}\n";
            str += "tipo: {pedido.tipo}\n";
            str += "=========================================================\n";
            str += "|id  |nome            |qtd   |preco   |\n";
            str += "[pedido.itens_pedido]\n";
            str += "=========================================================\n";
            str += "adicional: {pedido.totais_pedido.adicional}\n";
            str += "desconto: {pedido.totais_pedido.desconto}\n";
            str += "total: {pedido.totais_pedido.total}\n";
            str += "=========================================================\n";
            str += "total: {pedido.totais_pedido.totais_total.total}\n";

            string stritem = "{pedido.itens_pedido.id} {pedido.itens_pedido.nome} {pedido.itens_pedido.qtde} {pedido.itens_pedido.preco}\n";
            stritem += "{pedido.itens_pedido.totais_item.total} {pedido.itens_pedido.tipo} > [pedido.itens_pedido.subitens_pedido]\n \n";

            string strsubitem = "*{pedido.itens_pedido.subitens_pedido.nome}\n";


            pedido pedido = new pedido();
            pedido.id = 1;
            pedido.nome = "teste";
            pedido.data = DateTime.Now;
            pedido.tipo = pedido.Tipo.Tipo1;
            pedido.itens = new List<pedido.itens_pedido>();

            pedido.itens_pedido itens1 = new pedido.itens_pedido();
            itens1.id = 1;
            itens1.nome = "testetestetestetesteteste";
            itens1.qtde = 1;
            itens1.preco = 20.03;
            itens1.tipo = pedido.Tipo.Tipo2;
            itens1.totais = new pedido.totais_item();
            itens1.totais.total = 20;
            itens1.subitens = new List<pedido.subitens_pedido>();
            pedido.subitens_pedido subitens1 = new pedido.subitens_pedido();
            subitens1.nome = "teste";
            itens1.subitens.Add(subitens1);
            pedido.itens.Add(itens1);

            pedido.itens_pedido itens2 = new pedido.itens_pedido();
            itens2.id = 2;
            itens2.nome = "teste1";
            itens2.qtde = 1;
            itens2.preco = 20;
            itens2.subitens = new List<pedido.subitens_pedido>();
            pedido.itens.Add(itens2);

            pedido.itens_pedido itens3 = new pedido.itens_pedido();
            itens3.id = 3;
            itens3.nome = "testes prod";
            itens3.qtde = 1;
            itens3.preco = 20.03;
            itens3.tipo = pedido.Tipo.Tipo2;
            itens3.totais = new pedido.totais_item();
            itens3.totais.total = 20;
            itens3.subitens = new List<pedido.subitens_pedido>();
            pedido.subitens_pedido subitens2 = new pedido.subitens_pedido();
            subitens2.nome = "teste123";
            itens3.subitens.Add(subitens2);
            pedido.subitens_pedido subitens3 = new pedido.subitens_pedido();
            subitens3.nome = "sub item";
            itens3.subitens.Add(subitens3);
            pedido.subitens_pedido subitens4 = new pedido.subitens_pedido();
            subitens4.nome = "novo subite";
            itens3.subitens.Add(subitens4);
            //itens3.subitens = null;
            pedido.itens.Add(itens3);

            pedido.totais = new pedido.totais_pedido();
            pedido.totais.adicional = 0;
            pedido.totais.desconto = 0;
            pedido.totais.total = 40000;
            pedido.totais.totais = new pedido.totais_total();
            pedido.totais.totais.total = 40;

            Configs configs = new Configs();
            configs.LineSizeAjust = true;// Ajusta a linha no tamanho informado em LineSize
            configs.LineSize = 38;// Tamanho padrão para a linha o padrão é 38
            configs.Template = str;// Template principal
            configs.TemplateList.Add(new TemplateList() { Name = "pedido.itens_pedido", Template = stritem });// Demais templates em listas, note que o Name = pedido.itens_pedido deve ser o mesmo [pedido.itens_pedido] no template que chama a lista
            configs.TemplateList.Add(new TemplateList() { Name = "pedido.itens_pedido.subitens_pedido", Template = strsubitem });
            Template Template = new Template(configs);
            string ret = Template.CreateTemplate<pedido>(pedido);

            Console.Write(ret);

            Console.ReadKey();
        }

        public class pedido
        {
            public int id { get; set; }
            [AttrConfigs("CL:{0}")]
            public string nome { get; set; }
            [AttrConfigs("MMMM dd, yyyy HH:mm:ss")]
            public DateTime data { get; set; }
            public Tipo tipo { get; set; }
            public List<itens_pedido> itens { get; set; }
            public totais_pedido totais { get; set; }

            public enum Tipo
            {
                Tipo1, Tipo2
            }

            public class itens_pedido
            {
                [AttrConfigs(5, "{0:00000}")]
                public int id { get; set; }
                [AttrConfigs(16, true)]
                public string nome { get; set; }
                public Tipo tipo { get; set; }
                [AttrConfigs(6, "{0:0.00}")]
                public double qtde { get; set; }
                [AttrConfigs(8, "R$ {0:0.00}")]
                public double preco { get; set; }
                public totais_item totais { get; set; }
                public List<subitens_pedido> subitens { get; set; }
            }

            public class subitens_pedido
            {
                public string nome { get; set; }
            }

            public class totais_pedido
            {
                public double adicional { get; set; }
                public double desconto { get; set; }
                [AttrConfigs("R$ {0:0,0.00}")]
                public double total { get; set; }
                public totais_total totais { get; set; }
            }

            public class totais_item
            {
                [AttrConfigs("R$ {0:0,0.00}")]
                public double total { get; set; }
            }

            public class totais_total
            {
                public double total { get; set; }
            }
        }
    }
}