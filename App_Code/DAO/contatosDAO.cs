using System;
using System.Data;
using System.Configuration;
using System.Web;

public class contatosDAO
{
    private Conexao _conn;

    public contatosDAO(Conexao c)
    {
        _conn = c;
    }

    public int insert(int cod_funcao, string nome_completo, string cep, string endereco,
        string numero, string bairro, string cidade, string estado, string telefone, string email, int enviar, int cod_empresa_relacao)
    {
        string sql = "INSERT INTO CAD_CONTATOS(COD_FUNCAO,NOME_COMPLETO,CEP,ENDERECO,NUMERO,BAIRRO,CIDADE,ESTADO,TELEFONE,EMAIL,ENVIAR,COD_EMPRESA,COD_EMPRESA_RELACAO)";
        sql += "VALUES";
        sql += "(" + cod_funcao + ",'" + nome_completo.Replace("'", "''") + "','" + cep + "','" + endereco.Replace("'", "''") + "','" + numero + "',";
        sql += "'" + bairro.Replace("'", "''") + "','" + cidade.Replace("'", "''") + "','" + estado + "','" + telefone + "','" + email.Replace("'", "''") + "'," + enviar + "," + HttpContext.Current.Session["empresa"] + "," + cod_empresa_relacao + ");";
        sql += "SELECT SCOPE_IDENTITY();";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void update(int cod_contato, int cod_funcao, string nome_completo, string cep, string endereco,
        string numero, string bairro, string cidade, string estado, string telefone, string email, int enviar, int cod_empresa_relacao)
    {
        string sql = "UPDATE CAD_CONTATOS SET COD_FUNCAO=" + cod_funcao + ",NOME_COMPLETO='" + nome_completo.Replace("'", "''") + "',";
        sql += " CEP='" + cep + "',ENDERECO='" + endereco.Replace("'", "''") + "',NUMERO='" + numero + "',BAIRRO='" + bairro.Replace("'", "''") + "',";
        sql += " CIDADE='" + cidade.Replace("'", "''") + "',ESTADO='" + estado + "',TELEFONE='" + telefone + "',EMAIL='" + email.Replace("'", "''") + "',ENVIAR=" + enviar + ", ";
        sql += " COD_EMPRESA_RELACAO=" + cod_empresa_relacao + " ";
        sql += " WHERE COD_CONTATO = " + cod_contato + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        _conn.execute(sql);
    }

    public void delete(int cod_contato)
    {
        string sql = "DELETE FROM CAD_CONTATOS ";
        sql += " WHERE COD_CONTATO = " + cod_contato + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        _conn.execute(sql);
    }

    public DataTable load(int cod_contato)
    {
        string sql = "SELECT *  FROM CAD_CONTATOS ";
        sql += " WHERE COD_CONTATO = " + cod_contato + " AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        return _conn.dataTable(sql, "contato");
    }

    public void lista(ref DataTable tb)
    {
        string sql = "SELECT * FROM CAD_CONTATOS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY NOME_COMPLETO";
        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, string nomeCompleto, Nullable<int> empresa, string email, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "COD_CONTATO DESC";

        string sql = "select vw.* from (SELECT  ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao;
        sql += ") AS Row, *  ";
        sql += "    FROM CAD_CONTATOS WHERE 1=1 ";

        if (nomeCompleto != null)
            sql += " AND NOME_COMPLETO LIKE '%" + nomeCompleto.Replace("'", "''") + "%'";

        if (empresa != null)
            sql += " AND COD_EMPRESA_RELACAO=" + empresa.Value + "";

        if (email != null)
            sql += " AND EMAIL = '" + email.Replace("'", "''") + "'";

        sql += " and COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        sql += "    ) as vw where 1=1 ";

        //PAGINACAO
        sql += " AND vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }

    public int totalRegistros(string nomeCompleto, Nullable<int> empresa, string email)
    {
        string sql = "select COUNT(COD_CONTATO) from CAD_CONTATOS WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        if (nomeCompleto != null)
            sql += " AND NOME_COMPLETO LIKE '%" + nomeCompleto.Replace("'", "''") + "%'";

        if (empresa != null)
            sql += " AND COD_EMPRESA_RELACAO=" + empresa.Value + "";

        if (email != null)
            sql += " AND EMAIL = '" + email.Replace("'", "''") + "'";

        return Convert.ToInt32(_conn.scalar(sql));
    }
}
