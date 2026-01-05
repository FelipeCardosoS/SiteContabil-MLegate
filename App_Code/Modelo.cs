using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;

public class Modelo
{
    private int _codigo;
    private string _nome;
    private string _observacao;
    private string _tipo;
    private bool _padraoDefault;

    private List<SLancamento> _arrLancamentos=  new List<SLancamento>();

    private List<string> erros;
    private modelosDAO modeloDAO;

    public int codigo
    {
        get { return _codigo; }
        set { _codigo = value; }
    }

    public string nome
    {
        get { return _nome; }
        set { _nome = value; }
    }

    public string observacao
    {
        get { return _observacao; }
        set { _observacao = value; }
    }

    public string tipo
    {
        get { return _tipo; }
        set { _tipo = value; }
    }

    public bool padraoDefault 
    {
        get { return _padraoDefault; }
        set { _padraoDefault = value; }
    }

    public List<SLancamento> arrLancamentos
    {
        get { return _arrLancamentos; }
        set { _arrLancamentos = value; }
    }

	public Modelo(Conexao c)
	{
        modeloDAO = new modelosDAO(c);
	}

    public Modelo(Conexao c, int codigo)
        :this(c)
    {
        _codigo = codigo;
    }

    public Modelo(Conexao c, int codigo, string nome, string observacao, string tipo, bool padraoDefault)
        : this(c, codigo)
    {
        _codigo = codigo;
        _nome = nome;
        _observacao = observacao;
        _tipo = tipo;
        _padraoDefault = padraoDefault;
    }

    public List<string> novo()
    {
        erros = new List<string>();

        if (_nome == "" || _nome == null)
            erros.Add("Informe o nome.");


        if (_tipo == "" || _tipo == null)
            erros.Add("Informe o tipo.");

        if (_padraoDefault)
        {
            if (modeloDAO.get_count_modelos_default(_tipo) > 0)
            {
                switch (_tipo)
                {
                    case "CP":
                        erros.Add("Modelo não pode ser default pois já existe um default para Contas à Pagar.");
                        break;

                    case "CR":
                        erros.Add("Modelo não pode ser default pois já existe um default para Contas à Receber.");
                        break;

                    case "C":
                        erros.Add("Modelo não pode ser default pois já existe um default para Contabilidade.");
                        break;
                }
            }
        }

        if (erros.Count == 0)
        {
            modeloDAO.insert(_nome, _observacao, _tipo,_padraoDefault);
        }

        return erros;
    }

    public List<string> alterar()
    {
        erros = new List<string>();

        if (_codigo == 0)
            erros.Add("Código inválido");

        if (_nome == "" || _nome == null)
            erros.Add("Informe o nome.");


        if (_tipo == "" || _tipo == null)
            erros.Add("Informe o tipo.");

        if (_padraoDefault)
        {
            if (modeloDAO.get_count_modelos_default(_tipo) > 0)
            {
                switch (_tipo)
                {
                    case "CP":
                        erros.Add("Modelo não pode ser default pois já existe um default para Contas à Pagar.");
                        break;

                    case "CR":
                        erros.Add("Modelo não pode ser default pois já existe um default para Contas à Receber.");
                        break;

                    case "C":
                        erros.Add("Modelo não pode ser default pois já existe um default para Contabilidade.");
                        break;
                }
            }
        }

        if (erros.Count == 0)
        {
            modeloDAO.update(_codigo, _nome, _observacao, _tipo, _padraoDefault);
        }

        return erros;
    }

    public List<string> deletar()
    {
        erros = new List<string>();

        if (_codigo == 0)
            erros.Add("Código inválido");


        if (erros.Count == 0)
        {
            if (modeloDAO.get_count_lanctos_com_modelo(_codigo) == 0)
            {
                modeloDAO.delete(_codigo);
            }
            else
            {
                erros.Add("Modelo não pode ser deletado porque está sendo usado em folhas já lançadas.");
            }
        }

        return erros;
    }

    public List<string> salvaLancamentos()
    {
        this.erros = new List<string>();

        if (_codigo <= 0)
            erros.Add("Código do módulo inválido");

        if (_arrLancamentos.Count == 0)
            erros.Add("Você não fez nenhum lançamento.");

        if (erros.Count == 0)
        {
            modeloDAO.delete_lanctos(_codigo);

            for (int i = 0; i < _arrLancamentos.Count; i++)
            {
                SLancamento l = _arrLancamentos[i];
                modeloDAO.insert_lanctos(_codigo, l.seqLote, l.debCred.Value, l.conta, l.descConta, l.job.Value, l.descJob, l.linhaNegocio.Value, l.descLinhaNegocio,
                    l.divisao.Value, l.descDivisao, l.cliente.Value, l.descCliente,l.qtd.Value, l.valorUnit.Value, l.valor.Value, l.historico, l.titulo.Value, l.vencimentos.Count, l.terceiro.Value, l.descTerceiro,
                    l.codConsultor, l.descConsultor,l.valorGrupo);
            }
        }

        return erros;
    }

    public void load()
    {
        DataTable linha = modeloDAO.load(_codigo);
        if (linha.Rows.Count > 0)
        {
            _nome = linha.Rows[0]["NOME"].ToString();
            _observacao = linha.Rows[0]["OBSERVACAO"].ToString();
            _tipo = linha.Rows[0]["TIPO"].ToString();
            _padraoDefault = Convert.ToBoolean(linha.Rows[0]["PADRAO_DEFAULT"]);

            DataTable lanctos = modeloDAO.listaLanctosModelo(_codigo);
            if (lanctos.Rows.Count > 0)
            {
                _arrLancamentos = new List<SLancamento>();
                for (int i = 0; i < lanctos.Rows.Count; i++)
                {
                    DataRow row = lanctos.Rows[i];
                    List<SVencimento> vencimentos = new List<SVencimento>();
                    
                    for (int p = 0; p < Convert.ToInt32(row["PARCELAS"]); p++)
                    {
                        vencimentos.Add(new SVencimento(null, null));
                    }

                    SLancamento lancto = new SLancamento(null, Convert.ToInt32(row["SEQ_LOTE"]),null, null,null,null,null,null, 
                        Convert.ToChar(row["DEB_CRED"]), null, row["COD_CONTA"].ToString(),
                        row["DESC_CONTA"].ToString(), Convert.ToInt32(row["COD_JOB"]), row["DESC_JOB"].ToString(), Convert.ToInt32(row["COD_LINHA_NEGOCIO"]),
                        row["DESC_LINHA_NEGOCIO"].ToString(), Convert.ToInt32(row["COD_DIVISAO"]), row["DESC_DIVISAO"].ToString(),
                        Convert.ToInt32(row["COD_CLIENTE"]), row["DESC_CLIENTE"].ToString(), Convert.ToInt32(row["QTD"]),Convert.ToDecimal(row["VALOR_UNIT"]),Convert.ToDecimal(row["VALOR"]), row["HISTORICO"].ToString(),
                        Convert.ToBoolean(row["TITULO"]), vencimentos, Convert.ToInt32(row["COD_TERCEIRO"]), row["DESC_TERCEIRO"].ToString(), null, _codigo, "0", row["DESC_CONSULTOR"].ToString(), Convert.ToInt32(row["COD_CONSULTOR"]), null, Convert.ToInt32(row["VALOR_GRUPO"]), 0);

                    _arrLancamentos.Add(lancto);
                }
            }
            else
            {
                _arrLancamentos = new List<SLancamento>();
            }
        }
    }

    public void listaTipoCP(ref DataTable tb)
    {
        modeloDAO.lista(ref tb, "CP");
    }

    public void listaTipoCR(ref DataTable tb)
    {
        modeloDAO.lista(ref tb, "CR");
    }


    public void listaTipoC(ref DataTable tb)
    {
        modeloDAO.lista(ref tb, "C");
    }

    public List<Hashtable> listaTipoCP()
    {
        return modeloDAO.lista("CP");

    }

    public List<Hashtable> listaTipoCR()
    {
        return modeloDAO.lista("CR");
    }

    public List<Hashtable> listaTipoC()
    {
        return modeloDAO.lista("C");
    }

    public void listaDefaultCP(ref DataTable tb)
    {
        modeloDAO.lista(ref tb, "CP", true);
    }

    public void listaDefaultCR(ref DataTable tb)
    {
        modeloDAO.lista(ref tb, "CR", true);
    }

    public List<Hashtable> listaDefaultCP()
    {
        return modeloDAO.lista("CP", true);

    }

    public List<Hashtable> listaDefaultCR()
    {
        return modeloDAO.lista("CR", true);
    }

    public void listaPaginada(ref DataTable tb, string nome, string tipo, string defaultSN, int paginaAtual, string ordenacao)
    {
        modeloDAO.lista(ref tb, nome, tipo, defaultSN, paginaAtual, ordenacao);
    }

    public int totalRegistros(string nome, string tipo, string defaultSN)
    {
        return modeloDAO.totalRegistros(nome, tipo, defaultSN);
    }
}
