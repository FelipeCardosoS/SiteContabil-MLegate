using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

public class Fornecedor : Empresa
{
	public Fornecedor(Conexao c)
        :base(c)
	{
        _tipo = "FORNECEDOR";
	}

    public Fornecedor(Conexao c,int codigo)
        : this(c)
    {
        _codigo = codigo;
    }

    public void lista(ref DataTable tb)
    {
        empresaDAO.listaFornecedores(ref tb);
    }

    public List<Hashtable> lista()
    {
        return empresaDAO.listaFornecedores();
    }
}
