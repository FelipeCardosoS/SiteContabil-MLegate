using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

public struct STarefa
{
    private int _codigo;
    private string _modulo;
    private string _tarefa;
    private string _descricao;

    public int codigo
    {
        get { return _codigo; }
        set { _codigo = value; }
    }

    public string modulo
    {
        get { return _modulo; }
        set { _modulo = value; }
    }

    public string tarefa
    {
        get { return _tarefa; }
        set { _tarefa = value; }
    }

    public string descricao
    {
        get { return _descricao; }
        set { _descricao = value; }
    }

    public STarefa(int codigo, string modulo, string tarefa, string descricao)
    {
        _codigo = codigo;
        _modulo = modulo;
        _tarefa = tarefa;
        _descricao = descricao;
    }
}
