using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

public class ContasBalancoDAO
{
    private Conexao _conn;

	public ContasBalancoDAO(Conexao conn)
	{
        _conn = conn;
	}

    public List<EstruturaBalanco> CarregaEstruturaBalanco()
    {
        List<EstruturaBalanco> listestruturaBalanco = new List<EstruturaBalanco>();

        string sql = "select * from Cad_Estrutura_Balanco where Cod_Empresa =" + HttpContext.Current.Session["empresa"] + " order by ordem";
        string stranalitica = string.Empty;
        string strativopassivo = string.Empty;
        DataTable dt_Balanco = _conn.dataTable(sql, "EstrturaBalanco");
        EstruturaBalanco _EstruturaBalanco = new EstruturaBalanco();
        foreach (DataRow row in dt_Balanco.Rows)
        {
            if (row["Analitica"].ToString().ToUpper() == "S")
            {
                stranalitica = "Sim";
            }
            else if (row["Analitica"].ToString().ToUpper() == "N")
            {
                stranalitica = "Não";
            }
            if (row["Ativo_Passivo"].ToString().ToUpper() == "A")
            {
                strativopassivo = "Ativo";
            }
            else if (row["Ativo_Passivo"].ToString().ToUpper() == "P")
            {
                strativopassivo = "Passivo";
            }
            _EstruturaBalanco = new EstruturaBalanco();
            listestruturaBalanco.Add(new EstruturaBalanco
            {
                Cod_Balanco = row["Codigo"].ToString(),
                Descricao = row["Descricao"].ToString(),
                Cod_Empresa = Convert.ToInt32(row["Cod_Empresa"].ToString()),
                Analitica = stranalitica,
                Ativo_Passivo = strativopassivo,
                Nivel = Convert.ToInt32(row["Nivel"].ToString()),
                Ordem = Convert.ToInt32(row["ordem"].ToString()),
            });
        }
        return listestruturaBalanco;
    }

    public void insert(EstruturaBalanco EstruturaBalanco)
    {
        string sql = "SELECT isnull(max(ordem),0) FROM Cad_Estrutura_Balanco where cod_empresa = " + HttpContext.Current.Session["empresa"];
        int Ordem = Convert.ToInt32(_conn.scalar(sql));

        sql = "INSERT INTO Cad_Estrutura_Balanco (cod_empresa,Codigo, DESCRICAO,Analitica, ativo_passivo, nivel,ordem)"
        +" VALUES "
        +"(" + HttpContext.Current.Session["empresa"] + ",'" + EstruturaBalanco.Cod_Balanco.Replace("'", "''") + "','" + EstruturaBalanco.Descricao.Replace("'", "''") + "','"
            + EstruturaBalanco.Analitica.Replace("'", "''") + "','" + EstruturaBalanco.Ativo_Passivo.Replace("'", "''") + "'," + EstruturaBalanco.Nivel + "," + (Ordem + 1) + ") ";

        _conn.execute(sql);
    }

    public void Up(string CodigoBalanco)
    {
        string sql = "select isnull(max(ordem),0) from [Cad_Estrutura_Balanco] where ordem < (SELECT isnull(max(ordem),0) FROM Cad_Estrutura_Balanco where cod_empresa = "
            + HttpContext.Current.Session["empresa"] + " and Codigo = '" + CodigoBalanco + "') and cod_empresa = " + HttpContext.Current.Session["empresa"];
        int Ordem = Convert.ToInt32(_conn.scalar(sql));

        if (Ordem != 0)
        {
            sql = "update Cad_Estrutura_Balanco set ordem = (select ordem from Cad_Estrutura_Balanco as d1 where d1.codigo = '" + CodigoBalanco + "' and d1.cod_empresa = Cad_Estrutura_Balanco.cod_empresa ) "
                + " where ordem = '" + Ordem + "' and cod_empresa = " + HttpContext.Current.Session["empresa"];

            _conn.execute(sql);

            sql = "update Cad_Estrutura_Balanco set ordem = " + Ordem
                + " where codigo = '" + CodigoBalanco.Replace("'", "''") + "' and cod_empresa = " + HttpContext.Current.Session["empresa"];

            _conn.execute(sql);
        }
    }

    public void Down(string CodigoBalanco)
    {
        string sql = "select isnull(MIN(ordem),0) from [Cad_Estrutura_Balanco] where ordem > (SELECT isnull(max(ordem),0) FROM Cad_Estrutura_Balanco where cod_empresa = "
            + HttpContext.Current.Session["empresa"] + " and Codigo = '" + CodigoBalanco + "') and cod_empresa = " + HttpContext.Current.Session["empresa"];
        int Ordem = Convert.ToInt32(_conn.scalar(sql));

        if (Ordem != 0)
        {
            sql = "update Cad_Estrutura_Balanco set ordem = (select ordem from Cad_Estrutura_Balanco as d1 where d1.codigo = '" + CodigoBalanco + "' and d1.cod_empresa = Cad_Estrutura_Balanco.cod_empresa ) "
            + " where ordem = '" + Ordem + "' and cod_empresa = " + HttpContext.Current.Session["empresa"];

            _conn.execute(sql);

            sql = "update Cad_Estrutura_Balanco set ordem = " + Ordem
                + " where codigo = '" + CodigoBalanco.Replace("'", "''") + "' and cod_empresa = " + HttpContext.Current.Session["empresa"];

            _conn.execute(sql);
        }
    }

    public void update(EstruturaBalanco EstruturaBalanco)
    {
        string sql = "update Cad_Estrutura_Balanco set Codigo = '" + EstruturaBalanco.Cod_Balanco.Replace("'", "''") + "', DESCRICAO = '" +
            EstruturaBalanco.Descricao.Replace("'", "''") + "', Analitica = '" + EstruturaBalanco.Analitica.Replace("'", "''") + "', Ativo_Passivo = '" + EstruturaBalanco.Ativo_Passivo.Replace("'", "''") 
            + "', nivel = " + EstruturaBalanco.Nivel + " where codigo = '" + EstruturaBalanco.Cod_Balanco.Replace("'", "''") + "' and cod_empresa = " + HttpContext.Current.Session["empresa"];

        _conn.execute(sql);
    }

    public void delete(string CodigoBalanco)
    {
        string sql = "DELETE FROM Cad_Estrutura_Balanco WHERE Codigo='" + CodigoBalanco + "' and cod_empresa = " + HttpContext.Current.Session["empresa"];

        _conn.execute(sql);
    }

    public List<EstruturaBalanco> list(int empresa)
    {
        string sql = "SELECT codigo, descricao, Analitica, Ativo_Passivo,nivel,ordem FROM Cad_Estrutura_Balanco where cod_empresa = " + empresa + " order by codigo ";
        DataTable tb = _conn.dataTable(sql, "contas");
        List<EstruturaBalanco> list = new List<EstruturaBalanco>();
        foreach (DataRow row in tb.Rows)
        {
            EstruturaBalanco _SContaBalanco = new EstruturaBalanco
            {
                Cod_Balanco = row["Codigo"].ToString(),
                Descricao = row["descricao"].ToString(),
                Analitica = row["Analitica"].ToString(),
                Ativo_Passivo = row["Ativo_Passivo"].ToString(),
                Nivel = Convert.ToInt16(row["nivel"].ToString()),
                Ordem = Convert.ToInt16(row["ordem"].ToString())};

            list.Add(_SContaBalanco);
        }
        return list;
    }

      public EstruturaBalanco load(int empresa, string codigo)
    {
        string sql = "SELECT codigo, descricao, Analitica, Ativo_Passivo,nivel,ordem FROM Cad_Estrutura_Balanco WHERE codigo='" + codigo+"' and cod_empresa = " + empresa;
        DataTable tb = _conn.dataTable(sql, "contas");
        EstruturaBalanco list = null;
        if (tb.Rows.Count > 0)
        {
            DataRow row = tb.Rows[0];
            EstruturaBalanco _SContaBalanco = new EstruturaBalanco
            {
                Cod_Balanco = row["Codigo"].ToString(),
                Descricao = row["descricao"].ToString(),
                Analitica = row["Analitica"].ToString(),
                Ativo_Passivo = row["Ativo_Passivo"].ToString(),
                Nivel = Convert.ToInt16(row["nivel"].ToString()),
                Ordem = Convert.ToInt16(row["ordem"].ToString())
            };
            list = _SContaBalanco;
        }

        return list;
    }

  
}