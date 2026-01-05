using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public class Retencao
{
    private retencoesDAO retencaoDAO;
    private List<string> erros;

    private int _cod_retencao;
    private string _nome;
    private string _aliquota;
    private string _apresentacao;
    private int _cod_emitente;
    private int _Cod_Retencoes_Sys;

    public int cod_retencao
    {
        get { return _cod_retencao; }
        set { _cod_retencao = value; }
    }

    public string nome
    {
        get { return _nome; }
        set { _nome = value; }
    }

    public string aliquota
    {
        get { return _aliquota; }
        set { _aliquota = value; }
    }

    public string apresentacao
    {
        get { return _apresentacao; }
        set { _apresentacao = value; }
    }

    public int cod_emitente
    {
        get { return _cod_emitente; }
        set { _cod_emitente = value; }
    }

    public int Cod_Retencoes_Sys
    {
        get { return _Cod_Retencoes_Sys; }
        set { _Cod_Retencoes_Sys = value; }
    }

    public Retencao(Conexao c)
    {
        retencaoDAO = new retencoesDAO(c);
    }

    public int totalRegistros(string nome, Nullable<double> aliquota, string apresentacao, Nullable<int> emitente)
    {
        return retencaoDAO.totalRegistros(nome, aliquota, apresentacao, emitente);
    }

    public void listaPaginada(ref DataTable tb, string nome, Nullable<double> aliquota, string apresentacao, Nullable<int> emitente, int paginaAtual, string ordenacao)
    {
        retencaoDAO.listaPaginada(ref tb, nome, aliquota, apresentacao, emitente, paginaAtual, ordenacao);
    }

    public void Load_Sys_Retencoes(ref DataTable tb)
    {
        retencaoDAO.Load_Sys_Retencoes(ref tb);
    }

    public void load()
    {
        DataTable linha = retencaoDAO.load(_cod_retencao);
        if (linha.Rows.Count > 0)
        {
            _nome = linha.Rows[0]["NOME"].ToString();
            _aliquota = linha.Rows[0]["ALIQUOTA"].ToString();
            _apresentacao = linha.Rows[0]["APRESENTACAO"].ToString();
            _Cod_Retencoes_Sys = Convert.ToInt32(linha.Rows[0]["cod_retencoes_sys"].ToString());
        }
    }

    public void lista_Emitentes_Selecionados(ref DataTable tb)
    {
        retencaoDAO.lista_Emitentes_Selecionados(ref tb, _cod_retencao);
    }

    public List<string> novo()
    {
        erros = new List<string>();

        string cod_empresa = Convert.ToString(HttpContext.Current.Session["empresa"]); //Empresa Logada

        if (string.IsNullOrEmpty(cod_empresa) || cod_empresa == null || cod_empresa == "" || cod_empresa == "0")
            erros.Add("A sessão expirou. Faça login novamente.");

        if (_nome == "" || _nome == null)
            erros.Add("Informe o Nome da Retenção.");

        if (_aliquota == "" || _aliquota == null || _aliquota == "," || _aliquota == ".")
            erros.Add("Informe a Alíquota da Retenção.");

        if (_apresentacao == "" || _apresentacao == null)
            erros.Add("Informe o Modo de Apresentação que será demonstrado na Nota Fiscal.");

        if (erros.Count == 0)
        {
            _cod_retencao = retencaoDAO.novo(_nome, _aliquota, _apresentacao, _Cod_Retencoes_Sys);
        }
        return erros;
    }

    public List<string> alterar()
    {
        erros = new List<string>();

        string cod_empresa = Convert.ToString(HttpContext.Current.Session["empresa"]); //Empresa Logada

        if (string.IsNullOrEmpty(cod_empresa) || cod_empresa == null || cod_empresa == "" || cod_empresa == "0")
            erros.Add("A sessão expirou. Faça login novamente.");

        if (_cod_retencao == 0)
            erros.Add("Código inválido.");

        if (_nome == "" || _nome == null)
            erros.Add("Informe o Nome da Retenção.");

        if (_aliquota == "" || _aliquota == null || _aliquota == "," || _aliquota == ".")
            erros.Add("Informe a Alíquota da Retenção.");

        if (_apresentacao == "" || _apresentacao == null)
            erros.Add("Informe o Modo de Apresentação que será demonstrado na Nota Fiscal.");

        if (erros.Count == 0)
        {
            retencaoDAO.alterar(_cod_retencao, _nome, _aliquota, _apresentacao, _Cod_Retencoes_Sys);
        }
        return erros;
    }

    public virtual List<string> deletar()
    {
        erros = new List<string>();

        if (_cod_retencao == 0)
            erros.Add("Retenção " + _nome + ": Código inválido.");

        if (erros.Count == 0)
            retencaoDAO.deletar(_cod_retencao);

        return erros;
    }

    public void insert_Emitentes_Selecionados()
    {
        retencaoDAO.insert_Emitentes_Selecionados(_cod_retencao, _cod_emitente);
    }

    public void delete_Emitentes_Deselecionados()
    {
        retencaoDAO.delete_Emitentes_Deselecionados(_cod_retencao, _cod_emitente);
    }

    public void lista_Retencoes(ref DataTable tb, int cod_emitente)
    {
        retencaoDAO.lista_Retencoes(ref tb, cod_emitente);
    }
}