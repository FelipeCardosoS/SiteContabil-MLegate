using System;
using System.Data;
using System.Configuration;
using System.Web;

/// <summary>
/// Summary description for importao_planilhaDAO
/// </summary>
public class importao_planilhaDAO
{

	private Conexao _conn;

    public importao_planilhaDAO(Conexao c)
	{
        _conn = c;
	}


    public DataTable list_detalhe(string cod_planilha)
    {
        string sql = "select * from CONTROLE_planilha where cod_planilha = " + cod_planilha + "";
        DataTable tabelaimport = _conn.dataTable(sql, "tabelaimport");
        return tabelaimport;
    }

    public void deletecontrole(int cod_planilha)
    {
        string sql = "DELETE FROM controle_planilha WHERE cod_planilha =" + cod_planilha + "";
        _conn.execute(sql);
    }

    public int insert_planilha(int cod_planilha, string numero_contrato, string cod_empresa, string cpf_cnpj, string cliente, string nome_fantasia, string endereco, string bairro, string numero, string complemento, string municipio, string cod_municipio, string uf, string cep, string pais, string cod_pais, string ie_rg, string nire, string valor_total, string data, bool ok)
    {
        string sql = "INSERT INTO controle_planilha ";
        sql += "VALUES";
        sql += "('" + cod_planilha + "','" + numero_contrato + "','" + cod_empresa + "','" + cpf_cnpj + "','" + cliente + "','" + nome_fantasia + "','" + endereco + "','" + bairro + "','" + numero + "','" + complemento + "','" + municipio + "','" + cod_municipio + "','" + uf + "','" + cep + "','" + pais + "','" + cod_pais + "','" + ie_rg + "','" + nire + "','" + valor_total + "','" + data + "','" + Convert.ToInt32(ok) + "')";
        
        return Convert.ToInt32(_conn.scalar(sql));
    }


    public int insert(string nome_planilha, DateTime data)
    {
        string sql = "INSERT INTO IMPORT_PLANILHAS(nome_planilha, data) ";
        sql += "VALUES";
        sql += "('" + nome_planilha + "','" + data.ToString("yyyyMMdd H:mm:ss") + "')";
        sql += "SELECT SCOPE_IDENTITY()";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public DataTable list_importacao(string ordem, Nullable<DateTime> data)
    {
        if (string.IsNullOrEmpty(ordem) || ordem == "0")
            ordem = "cod_planilha";
        string sql = " select *, isnull(( select SUM(LANCTOS_CONTAB.VALOR) from LANCTOS_CONTAB where LANCTOS_CONTAB.DEB_CRED = 'D' and LANCTOS_CONTAB.cod_planilha = import_planilhas.cod_planilha),0) as Valor_Total from import_planilhas ";
        if (data != null)
            sql += " Where convert (datetime, convert (varchar,DATA,112)) = '" + data.Value.ToString("yyyyMMdd") + "' ";
        sql += " order by "+ ordem +" desc ";
        DataTable tabelaimport = _conn.dataTable(sql, "tabelaimport");
        return tabelaimport;
    }

    public int list_totallinha(Nullable<DateTime> data)
    {
        string sql = "select count (*) from import_planilhas";
        if (data != null)
            sql += " Where convert (datetime, convert (varchar,DATA,112)) = '" + data.Value.ToString("yyyyMMdd") + "' ";
        int totallinha = (int)_conn.scalar(sql);
        return totallinha;
    }

    public DataTable listLotes(int codigo)
    {
        string sql = "select distinct lote,modulo from lanctos_contab where pendente='False' and det_baixa=0 and cod_planilha=0" + codigo;
        return _conn.dataTable(sql, "chupeta");
    }

    public void delete(int cod_planilha)
    {
        string sql = "DELETE FROM import_planilhas ";
        sql += " WHERE COD_PLANILHA=" + cod_planilha + "";
        _conn.execute(sql);
    }
}





	
