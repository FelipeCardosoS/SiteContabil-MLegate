using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class Tributo
{
    private tributosDAO tributosDAO;
    private List<string> erros;

    private int _cod_tributo;
    private string _nome;
    private string _aliquota;
    private int _cod_emitente;
    private bool _Destacado;
    private int _Cod_Tributos_Sys;

    public int cod_tributo
    {
        get { return _cod_tributo; }
        set { _cod_tributo = value; }
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

    public int cod_emitente
    {
        get { return _cod_emitente; }
        set { _cod_emitente = value; }
    }

    public bool Destacado
    {
        get { return _Destacado; }
        set { _Destacado = value; }
    }

    public int Cod_Tributos_Sys
    {
        get { return _Cod_Tributos_Sys; }
        set { _Cod_Tributos_Sys = value; }
    }

    public Tributo(Conexao c)
    {
        tributosDAO = new tributosDAO(c);
    }

    public int totalRegistros(string nome, Nullable<double> aliquota, Nullable<int> emitente)
    {
        return tributosDAO.totalRegistros(nome, aliquota, emitente);
    }

    public void listaPaginada(ref DataTable tb, string nome, Nullable<double> aliquota, Nullable<int> emitente, int paginaAtual, string ordenacao)
    {
        tributosDAO.listaPaginada(ref tb, nome, aliquota, emitente, paginaAtual, ordenacao);
    }

    public void Load_Sys_Tributos(ref DataTable tb)
    {
        tributosDAO.Load_Sys_Tributos(ref tb);
    }

    public void load()
    {
        DataTable linha = tributosDAO.load(_cod_tributo);
        if (linha.Rows.Count > 0)
        {
            _nome = linha.Rows[0]["NOME"].ToString();
            _aliquota = linha.Rows[0]["ALIQUOTA"].ToString();
            _Cod_Tributos_Sys = Convert.ToInt32(linha.Rows[0]["Cod_Tributos_Sys"].ToString());
            _Destacado =  Convert.ToBoolean(linha.Rows[0]["Destacado"].ToString());
        }
    }

    public void lista_Emitentes_Selecionados(ref DataTable tb)
    {
        tributosDAO.lista_Emitentes_Selecionados(ref tb, _cod_tributo);
    }

    public List<string> novo()
    {
        erros = new List<string>();

        string cod_empresa = Convert.ToString(HttpContext.Current.Session["empresa"]); //Empresa Logada

        if (string.IsNullOrEmpty(cod_empresa) || cod_empresa == null || cod_empresa == "" || cod_empresa == "0")
            erros.Add("A sessão expirou. Faça login novamente.");

        if (_nome == "" || _nome == null)
            erros.Add("Informe o Nome do Tributo.");

        if (_aliquota == "" || _aliquota == null || _aliquota == "," || _aliquota == ".")
            erros.Add("Informe a Alíquota do Tributo.");        

        if (erros.Count == 0)
        {
            _cod_tributo = tributosDAO.novo(_nome, _aliquota, _Cod_Tributos_Sys, _Destacado);
        }
        return erros;
    }

    public List<string> Valida_Emitente(string List_Servicos)
    {
        erros = new List<string>();

        if (string.IsNullOrEmpty(List_Servicos))
            erros.Add("Informe os serviços da emtitente selecionada.");
        
        return erros;
    }

    public List<string> alterar()
    {
        erros = new List<string>();

        string cod_empresa = Convert.ToString(HttpContext.Current.Session["empresa"]); //Empresa Logada

        if (string.IsNullOrEmpty(cod_empresa) || cod_empresa == null || cod_empresa == "" || cod_empresa == "0")
            erros.Add("A sessão expirou. Faça login novamente.");

        if (_cod_tributo == 0)
            erros.Add("Código inválido.");

        if (_nome == "" || _nome == null)
            erros.Add("Informe o Nome do Tributo.");

        if (_aliquota == "" || _aliquota == null || _aliquota == "," || _aliquota == ".")
            erros.Add("Informe a Alíquota do Tributo.");

        if (erros.Count == 0)
        {
            tributosDAO.alterar(_cod_tributo, _nome, _aliquota, Cod_Tributos_Sys, Destacado);
        }
        return erros;
    }

    public virtual List<string> deletar()
    {
        erros = new List<string>();

        if (_cod_tributo == 0)
            erros.Add("Tributo " + _nome + ": Código inválido.");

        if (erros.Count == 0)
            tributosDAO.deletar(_cod_tributo);

        return erros;
    }

    public void insert_Emitentes_Selecionados(string List_Servicos)
    {
        List<int> list_servicos = List_Servicos.Split(',').Select(int.Parse).ToList();
        foreach (int item in list_servicos)
        {
            tributosDAO.insert_Emitentes_Selecionados(_cod_tributo, _cod_emitente, item);
        }        
    }

    public void delete_Emitentes_Deselecionados()
    {
        tributosDAO.delete_Emitentes_Deselecionados(_cod_tributo, _cod_emitente);
    }

    public void lista_Tributos(ref DataTable tb, int cod_emitente)
    {
        tributosDAO.lista_Tributos(ref tb, cod_emitente);
    }
}