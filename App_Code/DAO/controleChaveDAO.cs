using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for controleChave
/// </summary>
public class controleChaveDAO
{
    private Conexao _conn;

    public controleChaveDAO(Conexao c)
    {
        _conn = c;
    }

    public void delete(int empresa, int mes, int ano)
    {
        _conn.execute("delete from controle_chave where cod_empresa="+empresa+" and mes="+mes+" and ano="+ano+"");
    }

    public void insert(int empresa, int mes, int ano, string chave)
    {
        _conn.execute("insert into controle_chave(cod_empresa,mes,ano,chave)values(" + empresa + ", " + mes + ", " + ano + ", '" + chave + "')");
    }

    public DataTable buscaPeriodo(int mes, int ano)
    {
        return _conn.dataTable("select * from controle_chave where mes=" + mes + " and ano=" + ano + " AND COD_EMPRESA='" + HttpContext.Current.Session["empresa"] + "'", "periodoChaves");
    }
}
