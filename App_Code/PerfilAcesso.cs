using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public class PerfilAcesso
{
    private string _codigo;
    private string _descricao;
    private bool _exigeModelo;
    private List<SModulo> _arrModulos = new List<SModulo>();
    private List<STarefa> _arrTarefas = new List<STarefa>();

    private perfisDAO perfilDAO;
    private modulosDAO moduloDAO;
    private List<string> erros;

	public PerfilAcesso(Conexao c)
	{
        perfilDAO = new perfisDAO(c);
        moduloDAO = new modulosDAO(c);
	}

    public string codigo
    {
        get { return this._codigo; }
        set { _codigo = value; }
    }

    public string descricao
    {
        get { return _descricao; }
        set { _descricao = value; }
    }

    public bool exigeModelo
    {
        get { return _exigeModelo; }
        set { _exigeModelo = value; }
    }

    public List<SModulo> arrModulos
    {
        get { return _arrModulos; }
        set { _arrModulos = value; }
    }

    public List<STarefa> arrTarefas
    {
        get { return _arrTarefas; }
        set { _arrTarefas = value; }
    }

    public List<string> novo()
    {
        erros = new List<string>();

        if (_codigo == "" || _codigo == null)
            erros.Add("Código do Perfil inválido");

        if (_descricao == "" || _descricao == null)
            erros.Add("Descrição do Perfil está vazia");

        if (erros.Count == 0)
        {
            perfilDAO.insert(_codigo, _descricao);
            for (int i = 0; i < _arrModulos.Count; i++)
            {
                moduloDAO.insertModuloPerfil(_arrModulos[i].codigo, _codigo);
            }
        }

        return erros;
    }

    public List<string> novo(int empresa)
    {
        erros = new List<string>();

        if (_codigo == "" || _codigo == null)
            erros.Add("Código do Perfil inválido");

        if (_descricao == "" || _descricao == null)
            erros.Add("Descrição do Perfil está vazia");

        if (erros.Count == 0)
        {
            perfilDAO.insert(_codigo, _descricao, empresa);
            for (int i = 0; i < _arrModulos.Count; i++)
            {
                moduloDAO.insertModuloPerfil(_arrModulos[i].codigo, _codigo);
            }
        }

        return erros;
    }

    public List<string> liberaAcessoTotal()
    {
        erros = new List<string>();

        if (_codigo == "" || _codigo == null)
            erros.Add("Código do Perfil inválido");


        if (erros.Count == 0)
        {
            perfilDAO.acessoTotal(_codigo);
        }

        return erros;
    }

    public List<string> liberaAcessoTotal(int empresa)
    {
        erros = new List<string>();

        if (_codigo == "" || _codigo == null)
            erros.Add("Código do Perfil inválido");


        if (erros.Count == 0)
        {
            perfilDAO.acessoTotal(_codigo,empresa);
        }

        return erros;
    }

    public List<string> alterar()
    {
        erros = new List<string>();

        if (_codigo == "" || _codigo == null)
            erros.Add("Código do Perfil inválido");

        if (_descricao == "" || _descricao == null)
            erros.Add("Descrição do Perfil está vazia");

        if (erros.Count == 0)
        {
            perfilDAO.update(_codigo,_descricao,_exigeModelo);
            
            DataTable existentes = new DataTable("modulos");
            moduloDAO.listaModulosPerfil(ref existentes, _codigo);

            List<SModulo> deletar = new List<SModulo>();
            List<SModulo> inserir = new List<SModulo>();
            for (int i = 0; i < existentes.Rows.Count; i++)
            {
                bool existe = false;
                for (int x = 0; x < _arrModulos.Count; x++)
                {
                    if (existentes.Rows[i]["COD_MODULO"].ToString() == _arrModulos[x].codigo)
                    {
                        existe = true;
                    }
                }
                if (!existe)
                    deletar.Add(new SModulo(existentes.Rows[i]["COD_MODULO"].ToString(),
                        existentes.Rows[i]["COD_MODULO_PAI"].ToString(), existentes.Rows[i]["DESCRICAO"].ToString(),
                        existentes.Rows[i]["PAGINA"].ToString()));
            }

            for (int i = 0; i < _arrModulos.Count; i++)
            {
                bool existe = false;
                for (int x = 0; x < existentes.Rows.Count; x++)
                {
                    if (existentes.Rows[x]["COD_MODULO"].ToString() == _arrModulos[i].codigo)
                    {
                        existe = true;
                    }
                }

                if (!existe)
                    inserir.Add(_arrModulos[i]);
            }

            for (int i = 0; i < inserir.Count; i++)
            {
                moduloDAO.insertModuloPerfil(inserir[i].codigo, _codigo);
            }

            for (int i = 0; i < deletar.Count; i++)
            {
                moduloDAO.deleteModuloPerfil(deletar[i].codigo, _codigo);
            }
        }

        return erros;
    }

    public List<string> deletar()
    {
        erros = new List<string>();

        if (_codigo == "" || _codigo == null)
            erros.Add("Código do Perfil inválido");

        if (erros.Count == 0)
        {
            perfilDAO.delete(_codigo);
        }

        return erros;
    }

    public void load()
    {
        DataTable linha = perfilDAO.load(_codigo);
        if (linha.Rows.Count > 0)
        {
            _descricao = linha.Rows[0]["DESCRICAO"].ToString();
            _exigeModelo = Convert.ToBoolean(linha.Rows[0]["EXIGE_MODELO"]);

            DataTable modulos = new DataTable("modulos");
            moduloDAO.listaModulosPerfil(ref modulos, _codigo);
            _arrModulos.Clear();
            for (int i = 0; i < modulos.Rows.Count; i++)
            {
                _arrModulos.Add(new SModulo(modulos.Rows[i]["COD_MODULO"].ToString(),
                    modulos.Rows[i]["COD_MODULO_PAI"].ToString(), modulos.Rows[i]["DESCRICAO"].ToString(),
                    modulos.Rows[i]["PAGINA"].ToString()));
            }
        }
    }

    public void lista(ref DataTable tb)
    {
        perfilDAO.lista(ref tb);
    }

    public void listaPaginada(ref DataTable tb, string descricao, int paginaAtual, string ordenacao)
    {
        perfilDAO.lista(ref tb, descricao, paginaAtual, ordenacao);
    }

    public int totalRegistros(string descricao)
    {
        return perfilDAO.totalRegistros(descricao);
    }

    public void insereTarefas()
    {
        for (int i = 0; i < _arrTarefas.Count; i++)
        {
            moduloDAO.insertModuloPerfilTarefa(_arrTarefas[i].modulo, _codigo, _arrTarefas[i].codigo);
        }
    }

    public void deletaTarefas()
    {
        for (int i = 0; i < _arrTarefas.Count; i++)
        {
            moduloDAO.deleteModuloPerfilTarefa(_arrTarefas[i].modulo, _codigo, _arrTarefas[i].codigo);
        }
    }

    public void listaModulosPerfil(ref DataTable tb)
    {
        moduloDAO.listaModulosPerfil(ref tb, _codigo);
    }

    public void listaTarefasDisponiveis(ref DataTable tb, string cod_modulo)
    {
        moduloDAO.listaTarefasDisponiveis(ref tb, cod_modulo, _codigo);
    }

    public void listaTarefasSelecionadas(ref DataTable tb, string cod_modulo)
    {
        moduloDAO.listaTarefasSelecionadas(ref tb, cod_modulo, _codigo);
    }
}
