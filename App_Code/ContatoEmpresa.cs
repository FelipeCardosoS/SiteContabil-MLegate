using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;

public class ContatoEmpresa
{
    private int _codigo;
    private int _funcao;
    private string _nome;
    private string _cep;
    private string _endereco;
    private string _numero;
    private string _bairro;
    private string _cidade;
    private string _estado;
    private string _telefone;
    private string _email;
    private int _enviar;
    private int _empresa;

    private contatosDAO contatoDAO;
    private List<string> erros;

    public int empresa
    {
        get { return _empresa; }
        set { _empresa = value; }
    }

    public int codigo
    {
        get { return _codigo; }
        set { _codigo = value; }
    }

    public int funcao
    {
        get { return _funcao; }
        set { _funcao = value; }
    }

    public string nome
    {
        get { return _nome; }
        set { _nome = value; }
    }

    public string cep
    {
        get { return _cep; }
        set { _cep = value; }
    }

    public string endereco
    {
        get { return _endereco; }
        set { _endereco = value; }
    }

    public string numero
    {
        get { return _numero; }
        set { _numero = value; }
    }

    public string bairro
    {
        get { return _bairro; }
        set { _bairro = value; }
    }

    public string cidade
    {
        get { return _cidade; }
        set { _cidade = value; }
    }

    public string estado
    {
        get { return _estado; }
        set { _estado = value; }
    }

    public string telefone
    {
        get { return _telefone; }
        set { _telefone = value; }
    }

    public string email
    {
        get { return _email; }
        set { _email = value; }
    }

    public int enviar
    {
        get { return _enviar; }
        set { _enviar = value; }
    }

	public ContatoEmpresa(Conexao c)
	{
        contatoDAO = new contatosDAO(c);
	}

    public ContatoEmpresa(Conexao c, int codigo)
        :this(c)
    {
        _codigo = codigo;
    }

    public ContatoEmpresa(Conexao c, int codigo, int funcao, string nome, string cep, string endereco,
        string numero, string bairro, string cidade, string estado, string telefone, string email, int enviar, int empresa)
        : this(c, codigo)
    {
        _funcao = funcao;
        _nome = nome;
        _cep = cep;
        _endereco = endereco;
        _numero = numero;
        _bairro = bairro;
        _cidade = cidade;
        _estado = estado;
        _telefone = telefone;
        _email = email;
        _enviar = enviar;
        _empresa = empresa;
    }

    public List<string> novo()
    {
        erros = new List<string>();

        if (_nome == "" || _nome == null)
            erros.Add("Informe o nome.");

        if (_cep == "" || _cep == null)
            erros.Add("Informe o CEP.");

        if (_endereco == "" || _endereco == null)
            erros.Add("Informe o Endereço.");

        if (_numero == "" || _numero == null)
            erros.Add("Informe o Número do Endereço.");

        if (_bairro == "" || _bairro == null)
            erros.Add("Informe o Bairro.");

        if (_cidade == "" || _cidade == null)
            erros.Add("Informe a Cidade.");

        if (_estado == "" || _estado == null)
            erros.Add("Informe o Estado.");

        if (_telefone == "" || _telefone == null)
            erros.Add("Informe o Telefone.");

        if (_email == "" || _email == null)
            erros.Add("Informe o Email.");

        if (_empresa <= 0)
            erros.Add("Informe a empresa do contato.");

        if (erros.Count == 0)
        {
            contatoDAO.insert(_funcao, _nome, _cep, _endereco, _numero, _bairro, _cidade,
                _estado, _telefone, _email, _enviar, _empresa);
        }

        return erros;
    }

    public List<string> alterar()
    {
        erros = new List<string>();

        if (_codigo == 0)
            erros.Add("Código inválido.");

        if (_nome == "" || _nome == null)
            erros.Add("Informe o nome.");

        if (_cep == "" || _cep == null)
            erros.Add("Informe o CEP.");

        if (_endereco == "" || _endereco == null)
            erros.Add("Informe o Endereço.");

        if (_numero == "" || _numero == null)
            erros.Add("Informe o Número do Endereço.");

        if (_bairro == "" || _bairro == null)
            erros.Add("Informe o Bairro.");

        if (_cidade == "" || _cidade == null)
            erros.Add("Informe a Cidade.");

        if (_estado == "" || _estado == null)
            erros.Add("Informe o Estado.");

        if (_telefone == "" || _telefone == null)
            erros.Add("Informe o Telefone.");

        if (_email == "" || _email == null)
            erros.Add("Informe o Email.");

        if (_empresa <= 0)
            erros.Add("Informe a empresa do contato.");

        if (erros.Count == 0)
        {
            contatoDAO.update(_codigo,_funcao, _nome, _cep, _endereco, _numero, _bairro, _cidade,
                _estado, _telefone, _email, _enviar,_empresa);
        }

        return erros;
    }

    public List<string> deletar()
    {
        erros = new List<string>();

        if (_codigo == 0)
            erros.Add("Código inválido.");


        if (erros.Count == 0)
        {
            contatoDAO.delete(_codigo);
        }

        return erros;
    }

    public List<string> load()
    {
        erros = new List<string>();

        if (_codigo == 0)
            erros.Add("Código inválido.");

        if (erros.Count == 0)
        {
            DataTable linha = contatoDAO.load(_codigo);
            if (linha.Rows.Count > 0)
            {
                _funcao = Convert.ToInt32(linha.Rows[0]["COD_FUNCAO"]);
                _nome = linha.Rows[0]["NOME_COMPLETO"].ToString();
                _cep = linha.Rows[0]["CEP"].ToString();
                _endereco = linha.Rows[0]["ENDERECO"].ToString();
                _numero = linha.Rows[0]["NUMERO"].ToString();
                _bairro = linha.Rows[0]["BAIRRO"].ToString();
                _cidade = linha.Rows[0]["CIDADE"].ToString();
                _estado = linha.Rows[0]["ESTADO"].ToString();
                _telefone = linha.Rows[0]["TELEFONE"].ToString();
                _email = linha.Rows[0]["EMAIL"].ToString();
                _enviar = Convert.ToInt32(linha.Rows[0]["ENVIAR"]);
                _empresa = Convert.ToInt32(linha.Rows[0]["COD_EMPRESA_RELACAO"]);
            }
        }

        return erros;
    }

    public void lista(ref DataTable tb)
    {
        contatoDAO.lista(ref tb);
    }

    public void listaPaginada(ref DataTable tb,string nomeCompleto, Nullable<int> empresa,
        string email, int paginaAtual, string ordenacao)
    {
        contatoDAO.lista(ref tb, nomeCompleto, empresa, email, paginaAtual, ordenacao);
    }

    public int totalRegistros(string nomeCompleto, Nullable<int> empresa, string email)
    {
        return contatoDAO.totalRegistros(nomeCompleto, empresa, email);
    }
}
