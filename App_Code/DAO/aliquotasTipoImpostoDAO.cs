using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Summary description for aliquotasTipoImpostoDAO
/// </summary>
public class aliquotasTipoImpostoDAO
{
    private Conexao _conn;

	public aliquotasTipoImpostoDAO(Conexao c)
	{
        _conn = c;
	}

    public void insert(SAliquotaImposto aliquota)
    {
        string sql = "insert into tipos_imposto_aliquota(tipo_imposto,cumulativo,aliquota,aliquota_retencao,cod_empresa)values('" + aliquota.tipoImposto + "'," + Convert.ToInt32(aliquota.cumulativo) + "," + aliquota.aliquota.ToString().Replace(",", ".") + "," + aliquota.aliquotaRetencao.ToString().Replace(",", ".") + "," + aliquota.codEmpresa + ");";

        object result = _conn.scalar(sql);
    }

    public void update(SAliquotaImposto aliquota)
    {
        string sql = "update tipos_imposto_aliquota set aliquota='" + aliquota.aliquota + "', aliquota_retencao='" + aliquota.aliquotaRetencao + "'  where tipo_imposto='" + aliquota.tipoImposto + "' and cumulativo=" + Convert.ToInt32(aliquota.cumulativo) + " and cod_empresa=" + aliquota.codEmpresa;

        _conn.execute(sql);
    }

    public void delete(string tipoImposto, int codEmpresa)
    {
        string sql = "delete from tipos_imposto_aliquota where tipo_imposto='" + tipoImposto + "' and cod_empresa=" + codEmpresa;

        _conn.execute(sql);
    }

    public void delete(string tipoImposto, bool cumulativo, int codEmpresa)
    {
        string sql = "delete from tipos_imposto_aliquota where tipo_imposto='" + tipoImposto + "' and cumulativo="+Convert.ToInt32(cumulativo)+" and cod_empresa=" + codEmpresa;

        _conn.execute(sql);
    }

    public SAliquotaImposto load(string tipoImposto, bool cumulativo, int codEmpresa)
    {
        string sql = "select * from tipos_imposto_aliquota where tipo_imposto='" + tipoImposto + "' and cod_empresa=" + codEmpresa + " and cumulativo=" + Convert.ToInt32(cumulativo);
        DataTable tb = _conn.dataTable(sql, "tiposImposto");
        SAliquotaImposto t = null;
        if (tb.Rows.Count > 0)
        {
            DataRow row = tb.Rows[0];
            t = createObject(row);
        }

        return t;
    }

    public List<SAliquotaImposto> list(string tipoImposto, int codEmpresa)
    {
        string sql = "select * from tipos_imposto_aliquota where tipo_imposto='" + tipoImposto + "' and cod_empresa=" + codEmpresa + "";
        DataTable tb = _conn.dataTable(sql, "tiposImposto");
        List<SAliquotaImposto> list = new List<SAliquotaImposto>();
        for (int i = 0; i < tb.Rows.Count; i++)
        {
            list.Add(createObject(tb.Rows[i]));
        }
        return list;
    }

    public SAliquotaImposto createObject(DataRow row)
    {
        SAliquotaImposto t = new SAliquotaImposto();
        t.tipoImposto = row["TIPO_IMPOSTO"].ToString();
        t.cumulativo = Convert.ToBoolean(Convert.ToInt32(row["CUMULATIVO"]));
        t.codEmpresa = Convert.ToInt32(row["COD_EMPRESA"]);
        t.aliquota = Convert.ToDouble(row["ALIQUOTA"]);
        t.aliquotaRetencao = Convert.ToDouble(row["ALIQUOTA_RETENCAO"]);
        return t;
    }
}