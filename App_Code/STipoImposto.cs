using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for STipoImposto
/// </summary>
public class STipoImposto
{
    private string _tipoImposto;
    private string _descricao;
    private int _codEmpresa;

    public string tipoImposto
    {
        get { return _tipoImposto; }
        set { _tipoImposto = value; }
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

	public STipoImposto()
	{

	}
}