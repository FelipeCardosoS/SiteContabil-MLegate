using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;

public class LinhaNegocio
{
    private int _codigo;
    private string _descricao;

    private linhasNegocioDAO linhaNegocioDAO;
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

	public LinhaNegocio(Conexao c)
	{
        linhaNegocioDAO = new linhasNegocioDAO(c);
	}

    public LinhaNegocio(Conexao c, int codigo)
        : this(c)
    {
        _codigo = codigo;
    }

    public LinhaNegocio(Conexao c, int codigo, string descricao)
        : this(c, codigo)
    {
        _descricao = descricao;
    }

    public void load()
    {
        DataTable linha = linhaNegocioDAO.load(_codigo);
        if (linha.Rows.Count > 0)
        {
            _descricao = linha.Rows[0]["DESCRICAO"].ToString();
        }
    }

    public void dePara(int codigoTs)
    {
        DataTable depara = linhaNegocioDAO.dePara(codigoTs);
        if (depara.Rows.Count > 0)
        {
            DataRow row = depara.Rows[0];
            _codigo = Convert.ToInt32(row["cod_linha_negocio"]);
            _descricao = row["descricao"].ToString();
        }
        else
        {
            throw new Exception("Linha de Negócio "+codigoTs+" não encontrada.");
        }
    }

    public List<string> novo()
    {
        erros = new List<string>();
        
        if (string.IsNullOrEmpty(_descricao))
            erros.Add("Informe a descrição.");

        if (erros.Count == 0)
        {
            linhaNegocioDAO.insert(_descricao,0);
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
            linhaNegocioDAO.update(_codigo,_descricao);
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
            linhaNegocioDAO.delete(_codigo);
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
            linhaNegocioDAO.listaTimesheet(ref dsTimesheet);
            linhaNegocioDAO.lista(ref tbLocal);

            for (int i = 0; i < dsTimesheet.Tables[0].Rows.Count; i++)
            {
                bool existe = false;
                int codigoDivisaoLocal = 0;
                for (int x = 0; x < tbLocal.Rows.Count; x++)
                {
                    if (Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodProjeto"]) == Convert.ToInt32(tbLocal.Rows[x]["COD_REFERENCIA"]))
                    {
                        existe = true;
                        codigoDivisaoLocal = Convert.ToInt32(tbLocal.Rows[x]["COD_LINHA_NEGOCIO"]);
                        break;
                    }
                }


                if (!existe)
                {
                    try
                    {
                        linhaNegocioDAO.insert(dsTimesheet.Tables[0].Rows[i]["DescProjeto"].ToString(),
                            Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodProjeto"]));
                    }
                    catch (Exception ex)
                    {
                        erros.Add("Erro na inserção da linha de Negocio:" + codigoDivisaoLocal + "  " + ex.Message);
                    }
                }
                else
                {
                    bool igual = false;
                    for (int x = 0; x < tbLocal.Rows.Count; x++)
                    {
                        if (Convert.ToInt32(dsTimesheet.Tables[0].Rows[i]["CodProjeto"]) == Convert.ToInt32(tbLocal.Rows[x]["COD_REFERENCIA"]) 
                            && dsTimesheet.Tables[0].Rows[i]["DescProjeto"].ToString() == tbLocal.Rows[x]["DESCRICAO"].ToString())
                        {
                            igual = true; 
                            break;
                        }
                    }

                    if (!igual)
                    {
                        try
                        {
                            linhaNegocioDAO.update(codigoDivisaoLocal, dsTimesheet.Tables[0].Rows[i]["DescProjeto"].ToString());
                        }
                        catch (Exception ex)
                        {
                            erros.Add("Erro na alteração da linha de Negocio:" + codigoDivisaoLocal + "  " + ex.Message);
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

    public void lista(ref DataTable tb)
    {
        linhaNegocioDAO.lista(ref tb);
    }

    public void lista(ref DataTable tb, int cod_divisao)
    {
        linhaNegocioDAO.lista(ref tb, cod_divisao);
    }

    public void lista(ref DataTable tb, string colunaOrdenar)
    {
        linhaNegocioDAO.lista(ref tb, colunaOrdenar);
    }

    public List<Hashtable> lista()
    {
        return linhaNegocioDAO.lista();
    }

    public void listaPaginada(ref DataTable tb, string descricao, int paginaAtual, string ordenacao)
    {
        linhaNegocioDAO.lista(ref tb, descricao, paginaAtual, ordenacao);
    }

    public int totalRegistros(string descricao)
    {
        return linhaNegocioDAO.totalRegistros(descricao);
    }
}
