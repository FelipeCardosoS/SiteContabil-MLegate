using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SContaContabil
/// </summary>
public class SContaContabil
{
    private string _codigo;
    private int _grupoContabil;
    private string _descricao;
    private char _debitoCredito;
    private string _contaSintetica;
    private int _analitica;
    private string _tipo;
    private int _jobDefault;
    private string _tipoImposto;
    private bool _despesa;
    private bool _retido;

    public string codigo
    {
        get { return _codigo; }
        set { _codigo = value; }
    }

    public int  grupoContabil
    {
        get { return _grupoContabil; }
        set { _grupoContabil = value; }
    }

    public string descricao
    {
        get { return _descricao; }
        set { _descricao = value; }
    }

    public char debitoCredito
    {
        get { return _debitoCredito; }
        set { _debitoCredito = value; }
    }

    public string contaSintetica
    {
        get { return _contaSintetica; }
        set { _contaSintetica = value; }
    }

    public int analitica
    {
        get { return _analitica; }
        set { _analitica = value; }
    }

    public string tipo
    {
        get { return _tipo; }
        set { _tipo = value; }
    }

    public int jobDefault
    {
        get { return _jobDefault; }
        set { _jobDefault = value; }
    }

    public string tipoImposto
    {
        get { return _tipoImposto; }
        set { _tipoImposto = value; }
    }

    public bool despesa
    {
        get { return _despesa; }
        set { _despesa = value; }
    }

    public bool retido
    {
        get { return _retido; }
        set { _retido = value; }
    }

	public SContaContabil()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}