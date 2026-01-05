using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SProduto
/// </summary>
public class SProduto
{
    private int _codProduto;
    private string _descricao;
    private bool _geraCredito;
    private int _codEmpresa;
    private string _sigla;

    public int codProduto
    {
        get { return _codProduto; }
        set { _codProduto = value; }
    }
    public string descricao
    {
        get { return _descricao; }
        set { _descricao = value; }
    }

    public bool geraCredito
    {
        get { return _geraCredito; }
        set { _geraCredito = value; }
    }

    public int codEmpresa
    {
        get { return _codEmpresa; }
        set { _codEmpresa = value; }
    }
    public string sigla
    {
        get { return _sigla; }
        set { _sigla = value; }
    }

	public SProduto()
	{
		
	}
}