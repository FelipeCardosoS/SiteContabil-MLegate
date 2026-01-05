using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;

public partial class FormBaseImpostoParametros : BaseForm
{
    public FormBaseImpostoParametros()
        : base("RELATORIO_IMPOSTO_PARAMETROS")
    {
        
    }

    protected override void montaTela()
    {
        base.montaTela();
        Title += "Imposto - Parâmetros";
        subTitulo.Text = "Parâmetros";
    }

    [WebMethod]
    public static string atualizaParametros(string contas, string irnafonte, string csl, string pis, string cofins, string iss, string valorliquido)
    {
        Conexao con = new Conexao();
        parametrosDAO parametrosDAO = new parametrosDAO(con);
        string valida = "true";

        try
        {
            if (parametrosDAO.existe())
            {
                parametrosDAO.atualiza(contas, irnafonte, csl, pis, cofins, iss, valorliquido);
            }
            else
            {
                parametrosDAO.insert(contas, irnafonte, csl, pis, cofins, iss, valorliquido);
            }
        }
        catch 
        {
            valida = "false";
        }

        return valida;
    }

    [WebMethod]
    public static List<ListContaAnalitica> retornaContas()
    {
        Conexao con = new Conexao();
        ContaContabil conta = new ContaContabil(con);
        DataTable data = new DataTable();
        List<ListContaAnalitica> lista = new List<ListContaAnalitica>();

        conta.listaAnaliticas(ref data);

        foreach(DataRow row in data.Rows)
        {
            ListContaAnalitica listaRow = new ListContaAnalitica();
            listaRow.codigo = row["COD_CONTA"].ToString();
            listaRow.valor = row["DESCRICAO"].ToString();
            lista.Add(listaRow);
        }

        return lista;
    }

    [WebMethod]
    public static List<ListParametros> carregaValores()
    {
        Conexao con = new Conexao();
        parametrosDAO paramsDAO = new parametrosDAO(con);
        return paramsDAO.carregaValores();

    }

    public class ListContaAnalitica 
    {
        public string codigo{get; set;}
        public string valor{get; set;}
    }
}