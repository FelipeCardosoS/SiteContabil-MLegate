using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Log
/// </summary>
public class Log
{
    private double _codigo;
    private string _descricao;
    private int _usuario;
    private int _empresa;
    private string _modulo;
    private DateTime _dataHora;
    private double _lote;

    public double codigo
    {
        get { return _codigo; }
        set { _codigo = value; }
    }

    public string descricao
    {
        get { return _descricao; }
        set { _descricao = value; }
    }

    public int usuario
    {
        get { return _usuario; }
        set { _usuario = value; }
    }

    public int empresa
    {
        get { return _empresa; }
        set { _empresa = value; }
    }

    public string modulo
    {
        get { return _modulo; }
        set { _modulo = value; }
    }

    public DateTime dataHora
    {
        get { return _dataHora; }
        set { _dataHora = value; }
    }

    public double lote
    {
        get { return _lote; }
        set { _lote = value; }
    }
	public Log(double codigo, string descricao, int usuario, int empresa, string modulo, DateTime dataHora,
        double lote)
	{
        _codigo = codigo;
        _descricao = descricao;
        _usuario = usuario;
        _empresa = empresa;
        _modulo = modulo;
        _dataHora = dataHora;
        _lote = lote;
	}
}
