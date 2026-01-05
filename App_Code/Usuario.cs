using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;


public class Usuario
{
    private int _id;
    private string _nome;
    private string _email;
    private string _senha;
    private string _perfil;
	private int _idSessao;


	private List<string> erros;
    private usuariosDAO usuarioDAO;

    public Usuario(Conexao conn)
    {
        usuarioDAO = new usuariosDAO(conn);
    }

    public Usuario(Conexao conn, int id)
        : this(conn)
    {
        _id = id;
    }

    public Usuario(Conexao conn, int id, string nome, string email, string senha, string perfil)
        : this(conn, id)
    {
        _nome = nome;
        _email = email;
        _senha = senha;
        _perfil = perfil;
    }

    public int id
    {
        get { return _id; }
        set { _id = value; }
    }

    public string nome
    {
        get { return _nome; }
        set { _nome = value; }
    }

    public string perfil
    {
        get { return _perfil; }
        set { _perfil = value; }
    }

    public string email
    {
        get { return _email; }
        set { _email = value; }
    }

    public string senha
    {
        get { return _senha; }
        set { _senha = value; }
    }

	public int idSessao
	{
		get { return _idSessao; }
		set { _idSessao = value; }
	}


	public List<string> inserir()
    {
        erros = new List<string>();

        if ((this._nome == "" || this._nome == null))
            erros.Add("Informe seu nome.");
        if ((this._email == "" || this._email == null))
            erros.Add("Informe seu email.");
        if ((this._senha == "" || this._senha == null))
            erros.Add("Informe sua senha");
        if ((this._perfil == "" || this._perfil == null))
            erros.Add("Informe o perfil do usuário");

        if (erros.Count == 0)
        {
            if (!usuarioDAO.existe(this._email))
            {
                usuarioDAO.insert(this._nome, this._email, encripta(this._senha), this._perfil);
            }
            else
            {
                erros.Add("Login já está sendo usado.");
            }
        }
        return erros;
    }

    private string encripta(string texto)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(texto, "SHA1");
    }

    public List<string> inserir(int empresa)
    {
        erros = new List<string>();

        if ((this._nome == "" || this._nome == null))
            erros.Add("Informe seu nome.");
        if ((this._email == "" || this._email == null))
            erros.Add("Informe seu email.");
        if ((this._senha == "" || this._senha == null))
            erros.Add("Informe sua senha");
        if ((this._perfil == "" || this._perfil == null))
            erros.Add("Informe o perfil do usuário");

        if (erros.Count == 0)
        {
            if (!usuarioDAO.existe(this._email))
            {
                usuarioDAO.insert(this._nome, this._email, encripta(this._senha), this._perfil, empresa);
            }
            else
            {
                erros.Add("Login já está sendo usado.");
            }
        }
        return erros;
    }

    public List<string> alterar()
    {
        erros = new List<string>();

        if (this._id <= 0)
            erros.Add("ID inválido.");
        if ((this._nome == "" || this._nome == null))
            erros.Add("Informe seu nome.");
        if ((this._email == "" || this._email == null))
            erros.Add("Informe seu email.");
        if ((this._perfil == "" || this._perfil == null))
            erros.Add("Informe o perfil do usuário");

        if (erros.Count == 0)
        {
            if (!usuarioDAO.igual(this._id, this._email))
            {
                if (!usuarioDAO.existe(this._email))
                {
                    usuarioDAO.update(this._nome, this._email, this._perfil, this._id);
                }
                else
                {
                    erros.Add("Login já está sendo usado.");
                }
            }
            else
            {
                usuarioDAO.update(this._nome, this._email, this._perfil, this._id);
            }
        }
        return erros;
    }

    public List<string> alterarSenha(string novaSenha)
    {
        erros = new List<string>();

        if ((novaSenha == "" || novaSenha == null))
            erros.Add("Informe a nova senha");

        if ((this.idSessao == this.id) && !this.usuarioDAO.verifica_senha(encripta(this._senha), this._id))
            erros.Add("Senha atual está incorreta.");

        if (erros.Count == 0)
        {
            this.usuarioDAO.update_senha(encripta(novaSenha), this._id);
        }

        return erros;
    }

    public List<string> deletar()
    {
        erros = new List<string>();

        if (this._id <= 0)
            erros.Add("ID inválido.");

        if (erros.Count == 0)
        {
            usuarioDAO.delete(this._id);
        }
        return erros;
    }

    public void load()
    {
        if (this._id > 0)
        {
            DataTable usuario = usuarioDAO.load(this._id);
            if (usuario.Rows.Count > 0)
            {
                this._nome = usuario.Rows[0]["nome_completo"].ToString();
                this._email = usuario.Rows[0]["login"].ToString();
                this._senha = usuario.Rows[0]["senha"].ToString();
                this._perfil = usuario.Rows[0]["cod_perfil"].ToString();
            }
        }
    }

    public bool auth(int empresa)
    {
        erros = new List<string>();

        if ((this._email == "" || this._email == null))
            erros.Add("Informe seu email.");
        if ((this._senha == "" || this._senha == null))
            erros.Add("Informe sua senha");

        if (erros.Count == 0)
        {
            DataTable usuarios = usuarioDAO.auth(this._email, empresa);
            if (usuarios.Rows.Count > 0)
            {
                if (encripta(this._senha).Equals(usuarios.Rows[0]["senha"].ToString()))
                {
                    HttpContext.Current.Session.Add("usuario", usuarios.Rows[0]["COD_USUARIO"]);
                    HttpContext.Current.Session.Add("empresa", usuarios.Rows[0]["COD_EMPRESA"]);
                    HttpContext.Current.Session.Add("nome_empresa", usuarios.Rows[0]["NOME_RAZAO_SOCIAL"].ToString());
                    HttpContext.Current.Session.Add("exige_modelo", Convert.ToBoolean(usuarios.Rows[0]["EXIGE_MODELO"]));
                    HttpContext.Current.Session.Add("exige_contribuicoes", Convert.ToBoolean(usuarios.Rows[0]["EXIGE_CONTRIBUICOES"]));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    public List<SModulo> modulosPermitidos()
    {
        List<SModulo> arr = new List<SModulo>();

        if ((this._perfil != "" || this._perfil != null))
        {
            DataTable linha = usuarioDAO.modulosDisponiveis(this._perfil);
            for (int i = 0; i < linha.Rows.Count; i++)
            {
                SModulo mod = new SModulo(linha.Rows[i]["COD_MODULO"].ToString(),
                    linha.Rows[i]["COD_MODULO_PAI"].ToString(), linha.Rows[i]["DESCRICAO"].ToString(),
                    linha.Rows[i]["PAGINA"].ToString());
                arr.Add(mod);
            }

        }

        return arr;
    }

    public List<STarefa> tarefasPermitidas(string codigo_modulo)
    {
        List<STarefa> arr = new List<STarefa>();

        if ((this._perfil != "" || this._perfil != null) && (codigo_modulo != "" || codigo_modulo != null))
        {
            DataTable linha = usuarioDAO.tarefasDisponiveis(codigo_modulo, this._perfil);
            for (int i = 0; i < linha.Rows.Count; i++)
            {
                STarefa tarefa = new STarefa(Convert.ToInt32(linha.Rows[i]["COD_TAREFA"]),
                    linha.Rows[i]["COD_MODULO"].ToString(), linha.Rows[i]["TAREFA"].ToString(), linha.Rows[i]["DESCRICAO"].ToString());
                arr.Add(tarefa);
            }
        }

        return arr;
    }

    public void listaPaginada(ref DataTable tb, string nome, string perfil, int paginaAtual, string ordenacao)
    {
        usuarioDAO.lista(ref tb, nome, perfil, paginaAtual, ordenacao);
    }

    public int totalRegistros(string nome, string perfil)
    {
        return usuarioDAO.totalRegistros(nome, perfil);
    }
}
