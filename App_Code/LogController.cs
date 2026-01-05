using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for LogController
/// </summary>
public class LogController
{
    private logDAO logDAO;
	
    public LogController(Conexao c)
	{
        logDAO = new logDAO(c);
	}

    public bool add(Log log)
    {
        try
        {
            logDAO.insert(log.descricao, log.usuario, log.empresa, log.modulo, log.lote);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public List<Log> list(string modulo)
    {
        DataTable tb = logDAO.list(modulo);
        List<Log> l = new List<Log>();
        for (int i = 0; i < tb.Rows.Count; i++)
        {
            l.Add(new Log(Convert.ToDouble(tb.Rows[i]["cod_log"]), tb.Rows[i]["descricao"].ToString(),
                Convert.ToInt32(tb.Rows[i]["cod_usuario"]), Convert.ToInt32(tb.Rows[i]["cod_empresa"]),
                tb.Rows[i]["cod_modulo"].ToString(), Convert.ToDateTime(tb.Rows[i]["datahora"]),
                Convert.ToDouble(tb.Rows[i]["lote"])));
        }

        return l;
    }

    public List<Log> list(string modulo, double lote)
    {
        DataTable tb = logDAO.list(modulo, lote);
        List<Log> l = new List<Log>();
        for (int i = 0; i < tb.Rows.Count; i++)
        {
            l.Add(new Log(Convert.ToDouble(tb.Rows[i]["cod_log"]), tb.Rows[i]["descricao"].ToString(),
                Convert.ToInt32(tb.Rows[i]["cod_usuario"]), Convert.ToInt32(tb.Rows[i]["cod_empresa"]),
                tb.Rows[i]["cod_modulo"].ToString(), Convert.ToDateTime(tb.Rows[i]["datahora"]),
                Convert.ToDouble(tb.Rows[i]["lote"])));
        }

        return l;
    }
}
