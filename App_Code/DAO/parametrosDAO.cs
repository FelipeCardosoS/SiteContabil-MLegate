using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

public class ListParametros
{
    public string COD_EMPRESA { get; set; }
    public string COD_CONTAS { get; set; }
    public string IR_NA_FONTE { get; set; }
    public string CSL { get; set; }
    public string PIS { get; set; }
    public string COFINS { get; set; }
    public string ISS { get; set; }
    public string VALOR_LIQUIDO { get; set; }
}
public class parametrosDAO
{
	private Conexao _conn;

    public parametrosDAO(Conexao c)
	{
        _conn = c;
	}

    public bool existe()
    {
        string sql = "SELECT COUNT(*) FROM PARAMETROS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        int result = Convert.ToInt32(_conn.scalar(sql));
        if (result > 0)
            return true;
        else
            return false;
    }

    public bool insert(string contas, string irnafonte, string csl, string pis, string cofins, string iss, string valorliquido) 
    {
        string sql = "INSERT INTO PARAMETROS (COD_EMPRESA, COD_CONTAS, IR_NA_FONT, CSL, PIS, COFINS, ISS, VALOR_LIQUIDO) VALUES (" + HttpContext.Current.Session["empresa"] + ",'"+contas+"','"+irnafonte+"','"+csl+"','"+pis+"','"+cofins+"','"+iss+"','"+valorliquido+"')";
        int total = Convert.ToInt32(_conn.executeReturnRows(sql));
        return (total > 0);
    }

    public bool atualiza(string contas, string irnafonte, string csl, string pis, string cofins, string iss, string valorliquido)
    {
        string sql = "UPDATE PARAMETROS SET COD_CONTAS='" + contas + "', IR_NA_FONTE='" + irnafonte + "', CSL='" + csl + "', PIS='" + pis + "', COFINS='" + cofins + "', ISS='" + iss + "', VALOR_LIQUIDO='" + valorliquido + "' WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " ";
        int total = Convert.ToInt32(_conn.executeReturnRows(sql));
        return (total > 0);
    }

    public List<ListParametros> carregaValores() 
    {
        List<ListParametros> listParametros = new List<ListParametros>();
        ListParametros list = new ListParametros();
        string sql = "SELECT * FROM PARAMETROS WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"]+" ";
        List<Hashtable> listHash = _conn.reader(sql);

        list = new ListParametros();
        list.COD_CONTAS = listHash[0]["COD_CONTAS"].ToString();
        list.IR_NA_FONTE = listHash[0]["IR_NA_FONTE"].ToString();
        list.CSL = listHash[0]["CSL"].ToString();
        list.PIS = listHash[0]["PIS"].ToString();
        list.COFINS = listHash[0]["COFINS"].ToString();
        list.ISS = listHash[0]["ISS"].ToString();
        list.VALOR_LIQUIDO = listHash[0]["VALOR_LIQUIDO"].ToString();
        listParametros.Add(list);

        return listParametros;
    }
}