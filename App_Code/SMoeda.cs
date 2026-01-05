using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SMoeda
/// </summary>
public class SMoeda
{
    private string _descricao;
    private int _codMoeda;

    public string descricao
    {
        get { return _descricao; }
        set { _descricao = value; }
    }

    public int codMoeda
    {
        get { return _codMoeda; }
        set { _codMoeda = value; }
    }
}