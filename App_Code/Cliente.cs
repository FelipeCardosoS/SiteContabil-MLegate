using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;

public class Cliente : Empresa
{
	public Cliente(Conexao c)
        :base(c)
	{
        _tipo = "CLIENTE";
	}

    public void dePara(int codigoTs)
    {
        DataTable depara = empresaDAO.dePara(codigoTs);
        if (depara.Rows.Count > 0)
        {
            DataRow row = depara.Rows[0];
            _codigo = Convert.ToInt32(row["cod_empresa"]);
            _nomeFantasia = row["nome_fantasia"].ToString();
            _nome = row["nome_razao_social"].ToString();
        }
        else
        {
            throw new Exception("Cliente " + codigoTs + " não encontrado.");
        }
    }

    public List<Hashtable> lista()
    {
        return empresaDAO.listaClientes();
    }

    public List<Hashtable> listaClientesJob(int codigoJob)
    {
        return empresaDAO.listaClientesJob(codigoJob);
    }

    public DataTable listaDataTable()
    {
        DataTable tb = new DataTable("a");
        empresaDAO.listaClientes(ref tb);
        return tb;
    }

    public void lista(ref DataTable tb)
    {
        empresaDAO.listaClientes(ref tb);
    }

    public void lista_Tomadores(ref DataTable tb)
    {
        empresaDAO.lista_Tomadores(ref tb);
    }

    public void lista(ref DataTable tb, string colunaOrdenar)
    {
        empresaDAO.listaClientes(ref tb, colunaOrdenar);
    }
}
