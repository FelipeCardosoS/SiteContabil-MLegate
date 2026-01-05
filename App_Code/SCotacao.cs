using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SCotacao
/// </summary>
public class SCotacao
{
    private int _codCotacao;
    private int _codMoeda;
    private string _descrMoeda;
    private decimal _valor;
    private DateTime _data;

    public int codCotacao
    {
        get { return _codCotacao; }
        set { _codCotacao = value; }
    }

    public int codMoeda
    {
        get { return _codMoeda; }
        set { _codMoeda = value; }
    }

    public string descrMoeda 
    {
        get { return _descrMoeda; }
        set { _descrMoeda = value; }
    }

    public decimal valor 
    {
        get { return _valor; }
        set { _valor = value; }
    }

    public DateTime data 
    {
        get { return _data; }
        set { _data = value; }
    }
}