using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

public struct SModulo
{
    private string _codigo;
    private string _codigoPai;
    private string _descricao;
    private string _pagina;

    public string codigo
    {
        get { return _codigo; }
        set { _codigo = value; }
    }

    public string codigoPai
    {
        get { return _codigoPai; }
        set { _codigoPai = value; }
    }

    public string descricao
    {
        get { return _descricao; }
        set { _descricao = value; }
    }

    public string pagina
    {
        get { return _pagina; }
        set { _pagina = value; }
    }

    public SModulo(string codigo, string codigoPai, string descricao, string pagina)
    {
        _codigo = codigo;
        _codigoPai = codigoPai;
        _descricao = descricao;
        _pagina = pagina;
    }
}
