using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SClassificacaoConta
/// </summary>
public class SClassificacaoConta
{
    private string _codClassificacao;
    private string _descricao;
    private int _codEmpresa;

    public string codClassificacao
    {
        get { return _codClassificacao; }
        set { _codClassificacao = value; }
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

	public SClassificacaoConta()
	{
	}
}