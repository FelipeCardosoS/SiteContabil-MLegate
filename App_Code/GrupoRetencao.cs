//Projeto interrompido em 2018. Sem previsão de retomada.
//Menu (Banco de Dados): "CAD_MODULOS" e "CAD_TAREFAS" já possuem estrutura (FATURAMENTO_CADASTROS_GRUPO_RETENCOES).
//Engloba 5 Arquivos: "FormGrupoRetencoes.js", "FormGrupoRetencoes.aspx", "FormGrupoRetencoes.aspx.cs", "GrupoRetencao.cs" e "GrupoRetencoesDAO.cs".

using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public class GrupoRetencao
{
    private GrupoRetencoesDAO GrupoRetencoesDAO;
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

    public GrupoRetencao(Conexao Conexao)
    {
        GrupoRetencoesDAO = new GrupoRetencoesDAO(Conexao);
    }

    public void lista_Grupo_Retencoes(ref DataTable tb, int cod_emitente)
    {
        GrupoRetencoesDAO.lista_Grupo_Retencoes(ref tb, cod_emitente);
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
            _cod_retencao = GrupoRetencoesDAO.novo(_nome, _aliquota, _apresentacao, _Cod_Retencoes_Sys);
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
            GrupoRetencoesDAO.alterar(_cod_retencao, _nome, _aliquota, _apresentacao, _Cod_Retencoes_Sys);
        }
        return erros;
    }

    public virtual List<string> deletar()
    {
        erros = new List<string>();

        if (_cod_retencao == 0)
            erros.Add("Retenção " + _nome + ": Código inválido.");

        if (erros.Count == 0)
            GrupoRetencoesDAO.deletar(_cod_retencao);

        return erros;
    }

    public void insert_Emitentes_Selecionados()
    {
        GrupoRetencoesDAO.insert_Emitentes_Selecionados(_cod_retencao, _cod_emitente);
    }

    public void delete_Emitentes_Deselecionados()
    {
        GrupoRetencoesDAO.delete_Emitentes_Deselecionados(_cod_retencao, _cod_emitente);
    }
}