using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SUnidade
/// </summary>
public  class SUnidade
{
    private string _sigla;
    private string _descricao;
    private int _codEmpresa;

    public string sigla
    {
        get { return _sigla; }
        set { _sigla = value; }
    }

    public string descricao
    {
        get { return _descricao; }
        set { _descricao = value; }
    }

    public int codEmpresa
    {
        get { return _codEmpresa; }
        set { _codEmpresa = value; }
    }

	public SUnidade()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}