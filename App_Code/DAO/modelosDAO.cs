using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;

public class modelosDAO
{
    private Conexao _conn;

	public modelosDAO(Conexao c)
	{
        _conn = c;
	}

    public int insert(string nome, string observacao, string tipo, bool padrao_default)
    {
        string sql = "INSERT INTO CAD_MODELOS(NOME,OBSERVACAO,TIPO,PADRAO_DEFAULT,COD_EMPRESA)";
        sql += "VALUES";
        sql += "('" + nome + "','" + observacao + "', '" + tipo + "','"+padrao_default.ToString()+"'," + HttpContext.Current.Session["empresa"] + ");";
        sql += "SELECT SCOPE_IDENTITY();";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void insert_lanctos(int cod_modelo,int seqLote, char debCred,
        string conta, string descConta, int job, string descJob, int linhaNegocio, string descLinhaNegocio, int divisao,
        string descDivisao, int cliente, string descCliente, decimal qtd, decimal valorUnit, decimal valor, string historico,
        bool titulo,int parcelas, int terceiro, string descTerceiro, int codConsultor, string descConsultor, int? valorGrupo)
    {
        string sql = "INSERT INTO MODELO_LANCTOS(COD_MODELO,SEQ_LOTE, DEB_CRED,";
        sql += "COD_CONTA,DESC_CONTA,COD_JOB,DESC_JOB,COD_LINHA_NEGOCIO,DESC_LINHA_NEGOCIO,COD_DIVISAO,DESC_DIVISAO,";
        sql += "COD_CLIENTE,DESC_CLIENTE,QTD,VALOR_UNIT,VALOR,HISTORICO,TITULO, PARCELAS,COD_TERCEIRO,DESC_TERCEIRO,COD_EMPRESA,DESC_CONSULTOR, COD_CONSULTOR, VALOR_GRUPO)";
        sql += "VALUES";
        sql += "(" + cod_modelo + "," + seqLote + ",'" + debCred + "',";
        sql += "'" + conta + "','" + descConta + "'," + job + ",'" + descJob + "'," + linhaNegocio + ",'" + descLinhaNegocio + "'," + divisao + ",";
        sql += "'" + descDivisao + "'," + cliente + ",'" + descCliente + "', " + qtd.ToString().Replace(",", ".") + "," + valorUnit.ToString().Replace(",", ".") + "," + valor.ToString().Replace(",", ".") + ",'" + historico + "',";
        sql += "'" + titulo + "'," + parcelas + "," + terceiro + ",'" + descTerceiro + "'," + HttpContext.Current.Session["empresa"] + ",'" + descConsultor + "'," + codConsultor + "," + valorGrupo + ")";

        _conn.execute(sql);
    }

    public void update(int cod_modelo, string nome, string observacao, string tipo, bool padrao_default)
    {
        string sql = "UPDATE CAD_MODELOS SET NOME='" + nome + "',OBSERVACAO = '" + observacao + "', TIPO = '" + tipo + "', PADRAO_DEFAULT='" + padrao_default.ToString() + "' ";
        sql += " WHERE COD_MODELO=" + cod_modelo + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        _conn.execute(sql);
    }

    public void delete(int cod_modelo)
    {
        string sql = "DELETE FROM CAD_MODELOS ";
        sql += " WHERE COD_MODELO=" + cod_modelo + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        _conn.execute(sql);
    }

    public void delete_lanctos(int cod_modelo)
    {
        string sql = "DELETE FROM MODELO_LANCTOS ";
        sql += " WHERE COD_MODELO=" + cod_modelo + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        _conn.execute(sql);
    }

    public int get_count_modelos_default(string tipo)
    {
        string sql = "SELECT COUNT(*) FROM CAD_MODELOS WHERE TIPO='" + tipo + "' AND PADRAO_DEFAULT='True' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public int get_count_lanctos_com_modelo(int cod_modelo)
    {
        string sql = "SELECT COUNT(LOTE) FROM LANCTOS_CONTAB WHERE COD_MODELO=" + cod_modelo + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";
        return Convert.ToInt32(_conn.scalar(sql));
    }

    public DataTable load(int cod_modelo)
    {
        string sql = "SELECT * FROM CAD_MODELOS ";
        sql += " WHERE COD_MODELO=" + cod_modelo + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        return _conn.dataTable(sql, "modelo");
    }

    public void lista(ref DataTable tb)
    {
        string sql = "select * from CAD_MODELOS WHERE 1=1 ";

        sql += " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, string tipo)
    {
        string sql = "select * from CAD_MODELOS WHERE 1=1 ";

        sql += " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        if (tipo != "" && tipo != null)
            sql += " AND TIPO = '" + tipo + "'";

        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, string tipo, bool padraoDefault)
    {
        string sql = "select * from CAD_MODELOS WHERE 1=1 ";

        sql += " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        if (tipo != "" && tipo != null)
            sql += " AND TIPO = '" + tipo + "'";

        sql += " AND PADRAO_DEFAULT = '" + padraoDefault + "'";

        _conn.fill(sql, ref tb);
    }

    public List<Hashtable> lista(string tipo)
    {
        string sql = "select * from CAD_MODELOS WHERE 1=1 ";

        sql += " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        if (tipo != "" && tipo != null)
            sql += " AND TIPO = '" + tipo + "'";

        return _conn.reader(sql);
    }

    public List<Hashtable> lista(string tipo, bool padraoDefault)
    {
        string sql = "select * from CAD_MODELOS WHERE 1=1 ";

        sql += " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        if (tipo != "" && tipo != null)
            sql += " AND TIPO = '" + tipo + "'";

        sql += " AND PADRAO_DEFAULT = '" + padraoDefault + "'";

        return _conn.reader(sql);
    }

    public void lista(ref DataTable tb, string nome, string tipo,string defaultSN, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "NOME";

        string sql = "select * from (SELECT  ROW_NUMBER() OVER (ORDER BY ";

        if (!tmpOrdenacao.Equals("NOME"))
        {
            sql += tmpOrdenacao + ", ";
        }

        sql += "NOME)  ";

        sql += "AS Row, *, case when tipo='C' then 'Contabilidade' " +
			                  " when tipo='CP' then 'Contas à Pagar' "+
			                  " when tipo='CR' then 'Contas à Receber' end as DESCRICAO_TIPO ";
        sql += "    FROM CAD_MODELOS WHERE 1=1 ";

        if (nome != null)
            sql += " AND NOME LIKE '" + nome + "%'";

        if (tipo != null)
            sql += " AND TIPO = '" + tipo + "'";

        if (defaultSN != null)
            sql += " AND PADRAO_DEFAULT = '" + defaultSN + "'";

        sql += " and COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        sql += "    ) as vw where 1=1 ";

        //PAGINACAO
        sql += " AND vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }

    public int totalRegistros(string nome, string tipo, string defaultSN)
    {
        string sql = "select COUNT(COD_MODELO) from CAD_MODELOS WHERE 1=1 ";

        sql += " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        if (nome != null)
            sql += " AND NOME LIKE '" + nome + "%'";

        if (tipo != null)
            sql += " AND TIPO = '" + tipo + "'";

        if (defaultSN != null)
            sql += " AND PADRAO_DEFAULT = '" + defaultSN + "'";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public DataTable listaLanctosModelo(int cod_modelo)
    {
        string sql = "SELECT * FROM MODELO_LANCTOS WHERE COD_MODELO=" + cod_modelo + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";
        return _conn.dataTable(sql, "lanctos");
    }
}
