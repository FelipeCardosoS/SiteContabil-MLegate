using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for contasRefDAO
/// </summary>
public class ContasDREDAO
{
    private Conexao _conn;

	public ContasDREDAO(Conexao conn)
	{
        _conn = conn;
	}

    public List<EstruturaDRE> CarregaEstruturaDRE()
    {
        List<EstruturaDRE> listestruturaDRE = new List<EstruturaDRE>();

        string sql = "select * from Cad_Estrutura_DRE where Cod_Empresa =" + HttpContext.Current.Session["empresa"] + " order by ordem";
        string stranalitica = string.Empty;
        DataTable dt_DRE = _conn.dataTable(sql, "EstrturaDRE");
        EstruturaDRE _EstruturaDRE = new EstruturaDRE();
        foreach (DataRow row in dt_DRE.Rows)
        {
            if (row["Analitica"].ToString().ToUpper() == "S")
            {
                stranalitica = "Sim";
            }
            else if (row["Analitica"].ToString().ToUpper() == "N")
            {
                stranalitica = "Não";
            }
            _EstruturaDRE = new EstruturaDRE();
            listestruturaDRE.Add(new EstruturaDRE
            {
                Cod_DRE = row["Codigo"].ToString(),
                Descricao = row["Descricao"].ToString(),
                Cod_Empresa = Convert.ToInt32(row["Cod_Empresa"].ToString()),
                Analitica = stranalitica,
                Nivel = Convert.ToInt32(row["Nivel"].ToString()),
                Ordem = Convert.ToInt32(row["ordem"].ToString()),
            });
        }
        return listestruturaDRE;
    }

    public void insert(EstruturaDRE estruturadre)
    {
        string sql = "SELECT isnull(max(ordem),0) FROM Cad_Estrutura_DRE where cod_empresa = "+HttpContext.Current.Session["empresa"];
        int Ordem = Convert.ToInt32(_conn.scalar(sql));        

        sql = "INSERT INTO cad_estrutura_dre (cod_empresa,Codigo, DESCRICAO,Analitica,nivel,ordem)"
        +" VALUES "
        +"(" + HttpContext.Current.Session["empresa"] + ",'" + estruturadre.Cod_DRE.Replace("'","''") + "','" + estruturadre.Descricao.Replace("'", "''") + "','" + estruturadre.Analitica.Replace("'", "''") + "'," + estruturadre.Nivel + "," + (Ordem+1)  + ") ";

        _conn.execute(sql);
    }

    public void Up(string CodigoDre)
    {
        string sql = "select isnull(max(ordem),0) from [Cad_Estrutura_DRE] where ordem < (SELECT isnull(max(ordem),0) FROM Cad_Estrutura_DRE where cod_empresa = "
            + HttpContext.Current.Session["empresa"] + " and Codigo = '" + CodigoDre + "') and cod_empresa = " + HttpContext.Current.Session["empresa"];
        int Ordem = Convert.ToInt32(_conn.scalar(sql));

        if (Ordem != 0)
        {
            sql = "update cad_estrutura_dre set ordem = (select ordem from cad_estrutura_dre as d1 where d1.codigo = '" + CodigoDre + "' and d1.cod_empresa = cad_estrutura_dre.cod_empresa ) "
                + " where ordem = '" + Ordem + "' and cod_empresa = " + HttpContext.Current.Session["empresa"];

            _conn.execute(sql);

            sql = "update cad_estrutura_dre set ordem = " + Ordem
                + " where codigo = '" + CodigoDre.Replace("'", "''") + "' and cod_empresa = " + HttpContext.Current.Session["empresa"];

            _conn.execute(sql);
        }
    }

    public void Down(string CodigoDre)
    {
        string sql = "select isnull(MIN(ordem),0) from [Cad_Estrutura_DRE] where ordem > (SELECT isnull(max(ordem),0) FROM Cad_Estrutura_DRE where cod_empresa = " 
            + HttpContext.Current.Session["empresa"] + " and Codigo = '" + CodigoDre + "') and cod_empresa = " + HttpContext.Current.Session["empresa"];
        int Ordem = Convert.ToInt32(_conn.scalar(sql));

        if (Ordem != 0)
        {
            sql = "update cad_estrutura_dre set ordem = (select ordem from cad_estrutura_dre as d1 where d1.codigo = '" + CodigoDre + "' and d1.cod_empresa = cad_estrutura_dre.cod_empresa ) "
            + " where ordem = '" + Ordem + "' and cod_empresa = " + HttpContext.Current.Session["empresa"];

            _conn.execute(sql);

            sql = "update cad_estrutura_dre set ordem = " + Ordem
                + " where codigo = '" + CodigoDre.Replace("'", "''") + "' and cod_empresa = " + HttpContext.Current.Session["empresa"];

            _conn.execute(sql);
        }
        
        
    }

    public void update(EstruturaDRE estruturadre)
    {
        string sql = "update cad_estrutura_dre set Codigo = '"+ estruturadre.Cod_DRE.Replace("'", "''") + "', DESCRICAO = '"+ 
            estruturadre.Descricao.Replace("'", "''") + "', Analitica = '"+ estruturadre.Analitica.Replace("'","''") +"', nivel = "+ estruturadre.Nivel 
            +" where codigo = '"+ estruturadre.Cod_DRE.Replace("'","''") +"' and cod_empresa = "+ HttpContext.Current.Session["empresa"];

        _conn.execute(sql);
    }

    public void delete(string CodigoDre)
    {
        string sql = "DELETE FROM cad_estrutura_dre WHERE Codigo='" + CodigoDre + "' and cod_empresa = " + HttpContext.Current.Session["empresa"];

        _conn.execute(sql);
    }

    public List<EstruturaDRE> list(int empresa)
    {
        string sql = "SELECT codigo, descricao, Analitica,nivel,ordem FROM cad_estrutura_dre where cod_empresa = " + empresa + " order by codigo ";
        DataTable tb = _conn.dataTable(sql, "contas");
        List<EstruturaDRE> list = new List<EstruturaDRE>();
        foreach (DataRow row in tb.Rows)
        {
            EstruturaDRE _SContaDRE = new EstruturaDRE
            {
                Cod_DRE = row["Codigo"].ToString(),
                Descricao = row["descricao"].ToString(),
                Analitica = row["Analitica"].ToString(),
                Nivel = Convert.ToInt16(row["nivel"].ToString()),
                Ordem = Convert.ToInt16(row["ordem"].ToString())};

            list.Add(_SContaDRE);
        }
        return list;
    }

    public EstruturaDRE load(int empresa, string codigo)
    {
        string sql = "SELECT codigo, descricao, Analitica,nivel,ordem FROM CAD_CONTAS_REF WHERE codigo='" + codigo+"' and cod_empresa = " + empresa;
        DataTable tb = _conn.dataTable(sql, "contas");
        EstruturaDRE list = null;
        if (tb.Rows.Count > 0)
        {
            DataRow row = tb.Rows[0];
            EstruturaDRE _SContaDRE = new EstruturaDRE
            {
                Cod_DRE = row["Codigo"].ToString(),
                Descricao = row["descricao"].ToString(),
                Analitica = row["Analitica"].ToString(),
                Nivel = Convert.ToInt16(row["nivel"].ToString()),
                Ordem = Convert.ToInt16(row["ordem"].ToString())
            };
            list = _SContaDRE;
        }

        return list;
    }

  
}