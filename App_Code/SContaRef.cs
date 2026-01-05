using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SContaRef
/// </summary>
public class SContaRef
{
    private string _codigoRef;
    private string _descricao;
    private DateTime? _iniValidade;
    private DateTime? _fimValidade;
    private string _analiticaSintetica;

    public string codigoRef
    {
        get { return _codigoRef; }
        set { _codigoRef = value; }
    }
    public string descricao
    {
        get { return _descricao; }
        set { _descricao = value; }
    }
    public DateTime? iniValidade
    {
        get { return _iniValidade; }
        set { _iniValidade = value; }
    }
    public DateTime? fimValidade
    {
        get { return _fimValidade; }
        set { _fimValidade = value; }
    }
    public string analiticaSintetica
    {
        get { return _analiticaSintetica; }
        set { _analiticaSintetica = value; }
    }

	public SContaRef()
	{
	}
}