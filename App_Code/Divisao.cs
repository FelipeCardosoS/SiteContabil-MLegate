using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;

public class Divisao
{
    private int _codigo;
    private string _descricao;
    private bool _sincronizaBool;

    private divisoesDAO divisaoDAO;
    private List<string> erros;

    public int codigo
    {
        get { return _codigo; }
        set { _codigo = value; }
    }

    public string descricao
    {
        get { return _descricao; }
        set { _descricao = value; }
    }

    public bool sincronizaBool
    {
        get { return _sincronizaBool; }
        set { _sincronizaBool = value; }
    }

    public int total
    {
        get { return divisaoDAO.totalRegistros(_descricao); }
    }

	public Divisao(Conexao c)
	{
        divisaoDAO = new divisoesDAO(c);
	}

    public Divisao(Conexao c, int codigo)
        : this(c)
    {
        _codigo = codigo;
    }

    public Divisao(Conexao c, int codigo, string descricao)
        : this(c, codigo)
    {
        _descricao = descricao;
    }

    public void dePara(int codigoTs)
    {
        DataTable depara = divisaoDAO.dePara(codigoTs);
        if (depara.Rows.Count > 0)
        {
            DataRow row = depara.Rows[0];
            _codigo = Convert.ToInt32(row["cod_divisao"]);
            _descricao = row["descricao"].ToString();
        }
        else
        {
            throw new Exception("Divisão " + codigoTs + " não encontrada.");
        }
    }

    public void load()
    {
        DataTable linha = divisaoDAO.load(_codigo);
        if (linha.Rows.Count > 0)
        {
            _descricao = linha.Rows[0]["DESCRICAO"].ToString();
            try
            {
                _sincronizaBool = Convert.ToBoolean(linha.Rows[0]["SINCRONIZA"].ToString());
            }
            catch 
            {
                _sincronizaBool = true;
            }
        }
    }

    public List<string> novo()
    {
        erros = new List<string>();

        if (string.IsNullOrEmpty(_descricao))
            erros.Add("Informe a descrição.");

        if (erros.Count == 0)
        {
            divisaoDAO.insert(_descricao, 0, _sincronizaBool);
        }

        return erros;
    }

    public List<string> alterar()
    {
        erros = new List<string>();

        if (_codigo == 0)
            erros.Add("Código inválido.");

        if (string.IsNullOrEmpty(_descricao))
            erros.Add("Informe a descrição.");

        if (erros.Count == 0)
        {
            divisaoDAO.update(_codigo, _descricao, _sincronizaBool);
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
            divisaoDAO.delete(_codigo);            
        }

        return erros;
    }

    public List<string> sincroniza()
    {
        erros = new List<string>();
        DataSet dsTimesheet = new DataSet("dsTimesheet");
        DataTable tbLocal = new DataTable("tbLocal");

        try
        {
            divisaoDAO.listaTimesheet(ref dsTimesheet);
            divisaoDAO.listaSincronizacao(ref tbLocal);

            List<Divisao> cadastrar = new List<Divisao>();

            for (int i = 0; i < dsTimesheet.Tables[0].Rows.Count; i++)
            {
                bool existe = false;
                int codigoDivisaoLocal = 0;
                for (int x = 0; x < tbLocal.Rows.Count; x++)
                {
                    if (Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodDivisao"]) == Convert.ToInt32(tbLocal.Rows[x]["COD_REFERENCIA"]))
                    {
                        existe = true;
                        codigoDivisaoLocal = Convert.ToInt32(tbLocal.Rows[x]["COD_DIVISAO"]);
                        break;
                    }
                }


                if (!existe)
                {
                    try
                    {
                        divisaoDAO.insert(dsTimesheet.Tables[0].Rows[i]["NomeDivisao"].ToString(),
                            Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodDivisao"]),true);
                    }
                    catch (Exception ex)
                    {
                        erros.Add("Erro na inserção da divisão:" + codigoDivisaoLocal + "  " + ex.Message);
                    }
                }
                else
                {
                    bool igual = false;
                    for (int x = 0; x < tbLocal.Rows.Count; x++)
                    {
                        if (Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodDivisao"]) == Convert.ToInt32(tbLocal.Rows[x]["COD_REFERENCIA"]) && dsTimesheet.Tables[0].Rows[i]["NomeDivisao"].ToString() == tbLocal.Rows[x]["DESCRICAO"].ToString())
                        {
                            igual = true; 
                            break;
                        }
                    }

                    if (!igual)
                    {
                        try
                        {
                            divisaoDAO.update(codigoDivisaoLocal, dsTimesheet.Tables[0].Rows[i]["NomeDivisao"].ToString(), true);
                        }
                        catch (Exception ex)
                        {
                            erros.Add("Erro na alteração da divisão:" + codigoDivisaoLocal + "  " + ex.Message);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            erros.Add("Erro fatal:" + "  " + ex.Message);
        }

        return erros;
    }

    public List<Hashtable> lista()
    {
        return divisaoDAO.lista();
    }

    public void lista(ref DataTable tb)
    {
        divisaoDAO.lista(ref tb);
    }

    public void lista(ref DataTable tb, string colunaOrdenar)
    {
        divisaoDAO.lista(ref tb, colunaOrdenar);
    }

    public void listaPaginada(ref DataTable tb, string descricao, int paginaAtual, string ordenacao)
    {
        divisaoDAO.lista(ref tb, descricao, paginaAtual, ordenacao);
    }

}
