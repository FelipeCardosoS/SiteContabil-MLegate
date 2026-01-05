using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;

public class Empresa
{
    protected int _codigo;
    protected string _nome;
    protected string _nomeFantasia;
    protected string _cnpjCpf;
    protected string _endereco;
    protected string _numero;
    protected string _complemento;
    protected string _bairro;
    protected string _cep;
    protected string _municipio;
    protected string _codigoMunicipio;
    private string _codigoPais;
    protected string _telefone;
    protected string _ieRg;
    protected string _im;
    protected string _uf;
    protected int _consultor;
    protected int _empresaTimesheet;
    protected string _tipo;
    protected ETipoEmpresa _fisicaJuridica;
    protected int _sugestaoModeloCP;
    protected int _sugestaoModeloCR;
    protected bool _sincronizar;
    private string _nire;
    protected int _cod_empresa_plano_contas;
    protected empresasDAO empresaDAO;
    protected PerfilAcesso perfil;
    protected List<string> erros;
    protected bool _exigeContribuicoes;
    protected string _grupoFinanceiroEntrada;
    protected string _grupoFinanceiroSaida;
    protected int _codigoGrupoEconomico;

    public bool exigeContribuicoes
    {
        get { return _exigeContribuicoes; }
        set { _exigeContribuicoes = value; }
    }

    public int sugestaoModeloCP
    {
        get { return _sugestaoModeloCP; }
        set { _sugestaoModeloCP = value; }
    }

    public int cod_empresa_plano_contas
    {
        get { return _cod_empresa_plano_contas; }
        set { _cod_empresa_plano_contas = value; }
    }

    public int sugestaoModeloCR
    {
        get { return _sugestaoModeloCR; }
        set { _sugestaoModeloCR = value; }
    }

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

    public string nomeFantasia
    {
        get { return _nomeFantasia; }
        set { _nomeFantasia = value; }
    }

    public string cnpjCpf
    {
        get { return _cnpjCpf; }
        set { _cnpjCpf = value; }
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

    public string complemento
    {
        get { return _complemento; }
        set { _complemento = value; }
    }

    public string bairro
    {
        get { return _bairro; }
        set { _bairro = value; }
    }

    public string cep
    {
        get { return _cep; }
        set { _cep = value; }
    }

    public string municipio
    {
        get { return _municipio; }
        set { _municipio = value; }
    }

    public string codigoMunicipio
    {
        get { return _codigoMunicipio; }
        set { _codigoMunicipio = value; }
    }

    public string codigoPais
    {
        get { return _codigoPais; }
        set { _codigoPais = value; }
    }

    public string telefone
    {
        get { return _telefone; }
        set { _telefone = value; }
    }

    public string uf
    {
        get { return _uf; }
        set { _uf = value; }
    }

    public string ieRg
    {
        get { return _ieRg; }
        set { _ieRg = value; }
    }

    public string im
    {
        get { return _im; }
        set { _im = value; }
    }

    public int consultor
    {
        get { return _consultor; }
        set { _consultor = value; }
    }

    public int empresaTimesheet
    {
        get { return _empresaTimesheet; }
        set { _empresaTimesheet = value; }
    }

    public ETipoEmpresa fisicaJuridica
    {
        get { return _fisicaJuridica; }
        set { _fisicaJuridica = value; }
    }

    public string tipo
    {
        get { return _tipo; }
        set { _tipo = value; }
    }

    public bool sincronizar
    {
        get { return _sincronizar; }
        set { _sincronizar = value; }
    }

    public string nire
    {
        get { return _nire; }
        set { _nire = value; }
    }

    public string grupoFinanceiroEntrada
    {
        get
        {
            return _grupoFinanceiroEntrada;
        }

        set
        {
            _grupoFinanceiroEntrada = value;
        }
    }

    public string grupoFinanceiroSaida
    {
        get
        {
            return _grupoFinanceiroSaida;
        }

        set
        {
            _grupoFinanceiroSaida = value;
        }
    }

    public int codigoGrupoEconomico
    {
        get
        {
            return _codigoGrupoEconomico;
        }

        set
        {
            _codigoGrupoEconomico = value;
        }
    }

    public Empresa(Conexao c)
    {
        empresaDAO = new empresasDAO(c);
        perfil = new PerfilAcesso(c);
    }

    public void deParaConsultor(int codigoTs)
    {
        DataTable depara = empresaDAO.deParaConsultor(codigoTs);
        if (depara.Rows.Count > 0)
        {
            DataRow row = depara.Rows[0];
            _codigo = Convert.ToInt32(row["cod_empresa"]);
            _nome = row["nome_razao_social"].ToString();
            _nomeFantasia = row["nome_fantasia"].ToString();
        }
        else
        {
            throw new Exception("Cod. Consultor " + codigoTs + " não encontrada.");
        }
    }

    public virtual List<string> validar(string novoAlterar)
    {
        erros = new List<string>();

        if (_tipo == "" || _tipo == null)
            erros.Add("Selecione uma Classificação.");

        if (_nome == "" || _nome == null)
            erros.Add("Informe a Razão Social/Nome.");

        if (_cnpjCpf == "" || _cnpjCpf == null)
            erros.Add("Informe o CNPJ/CPF.");

        if (_ieRg == "" || _ieRg == null)
            erros.Add("Informe a Inscrição Estadudal/RG.");

        if (_cep == "" || _cep == null)
            erros.Add("Informe o CEP.");

        if (_endereco == "" || _endereco == null)
            erros.Add("Informe o Endereço.");

        if (_bairro == "" || _bairro == null)
            erros.Add("Informe o Bairro.");

        if (_municipio == "" || _municipio == null)
            erros.Add("Informe o Município.");

        if (_uf == "" || _uf == null)
            erros.Add("Informe a UF.");

        if (_codigoMunicipio == "" || _codigoMunicipio == null)
            erros.Add("Informe o Código do Município.");

        if (_codigoPais == "" || _codigoPais == null)
            erros.Add("Informe o Código do País.");

        if (_telefone == "" || _telefone == null)
            erros.Add("Informe o Telefone.");

        if (erros.Count == 0)
        {
            if (novoAlterar == "novo")
            {
                if (_fisicaJuridica == ETipoEmpresa.FISICA)
                {
                    if (_tipo == "EMPRESA" || _tipo == "FORNECEDOR" || _tipo == "CLIENTE" || _tipo == "CLIENTE_FORNECEDOR")
                    {
                        _codigo = empresaDAO.insert(Convert.ToInt32(HttpContext.Current.Session["empresa"]), _nome, "",
                        _cnpjCpf, _endereco, _numero, _complemento, _bairro, _cep, _municipio, _telefone, _ieRg, "", 0, _uf, _tipo, _consultor, _empresaTimesheet,
                        "FISICA", _sugestaoModeloCP, _sugestaoModeloCR, _sincronizar, _codigoMunicipio, _codigoPais, _nire, _cod_empresa_plano_contas, false, _grupoFinanceiroEntrada, _grupoFinanceiroSaida, codigoGrupoEconomico);
                    }
                    else
                    {
                        _codigo = empresaDAO.insert(0, _nome, "",
                        _cnpjCpf, _endereco, _numero, _complemento, _bairro, _cep, _municipio, _telefone, _ieRg, "", 0, _uf, _tipo, _consultor, _empresaTimesheet,
                        "FISICA", _sugestaoModeloCP, _sugestaoModeloCR, _sincronizar, _codigoMunicipio, _codigoPais, _nire, _cod_empresa_plano_contas, _exigeContribuicoes, _grupoFinanceiroEntrada, _grupoFinanceiroSaida, codigoGrupoEconomico);
                    }
                }
                else
                {
                    if (_tipo == "EMPRESA" || _tipo == "FORNECEDOR" || _tipo == "CLIENTE" || _tipo == "CLIENTE_FORNECEDOR")
                    {
                        _codigo = empresaDAO.insert(Convert.ToInt32(HttpContext.Current.Session["empresa"]), _nome, _nomeFantasia,
                        _cnpjCpf, _endereco, _numero, _complemento, _bairro, _cep, _municipio, _telefone, _ieRg, _im, 0, _uf, _tipo, _consultor, _empresaTimesheet,
                        "JURIDICA", _sugestaoModeloCP, _sugestaoModeloCR, _sincronizar, _codigoMunicipio, _codigoPais, _nire, _cod_empresa_plano_contas, false, _grupoFinanceiroEntrada, _grupoFinanceiroSaida, codigoGrupoEconomico);
                    }
                    else if (_tipo == "EMITENTE")
                    {
                        _codigo = empresaDAO.insert(Convert.ToInt32(HttpContext.Current.Session["empresa"]), _nome, _nomeFantasia,
                        _cnpjCpf, _endereco, _numero, _complemento, _bairro, _cep, _municipio, _telefone, _ieRg, _im, 0, _uf, _tipo, _consultor, _empresaTimesheet,
                        "JURIDICA", _sugestaoModeloCP, _sugestaoModeloCR, _sincronizar, _codigoMunicipio, _codigoPais, _nire, _cod_empresa_plano_contas, false, _grupoFinanceiroEntrada, _grupoFinanceiroSaida, codigoGrupoEconomico);

                        empresaDAO.Cria_Tabela_Faturamento_NF_Emitente(_codigo);
                    }
                    else
                    {
                        _codigo = empresaDAO.insert(0, _nome, _nomeFantasia,
                        _cnpjCpf, _endereco, _numero, _complemento, _bairro, _cep, _municipio, _telefone, _ieRg, _im, 0, _uf, _tipo, _consultor, _empresaTimesheet,
                        "JURIDICA", _sugestaoModeloCP, _sugestaoModeloCR, _sincronizar, _codigoMunicipio, _codigoPais, _nire, _cod_empresa_plano_contas, _exigeContribuicoes, _grupoFinanceiroEntrada, _grupoFinanceiroSaida, codigoGrupoEconomico);
                    }
                }
            }

            else
            {
                if (_codigo == 0)
                    erros.Add("Código inválido.");
                else
                {
                    if (_fisicaJuridica == ETipoEmpresa.FISICA)
                    {
                        if (_tipo == "EMPRESA" || _tipo == "FORNECEDOR" || _tipo == "CLIENTE" || _tipo == "CLIENTE_FORNECEDOR")
                        {
                            empresaDAO.update(_codigo, Convert.ToInt32(HttpContext.Current.Session["empresa"]), _nome, "",
                            _cnpjCpf, _endereco, _numero, _complemento, _bairro, _cep, _municipio, _telefone, _ieRg, "", 0, _uf, _tipo, _consultor, _empresaTimesheet,
                            "FISICA", _sugestaoModeloCP, _sugestaoModeloCR, _sincronizar, _codigoMunicipio, _codigoPais, _nire, _cod_empresa_plano_contas, false, _grupoFinanceiroEntrada, _grupoFinanceiroSaida, codigoGrupoEconomico);
                        }
                        else
                        {
                            empresaDAO.update(_codigo, 0, _nome, "",
                            _cnpjCpf, _endereco, _numero, _complemento, _bairro, _cep, _municipio, _telefone, _ieRg, "", 0, _uf, _tipo, _consultor, _empresaTimesheet,
                            "FISICA", _sugestaoModeloCP, _sugestaoModeloCR, _sincronizar, _codigoMunicipio, _codigoPais, _nire, _cod_empresa_plano_contas, _exigeContribuicoes, _grupoFinanceiroEntrada, _grupoFinanceiroSaida, codigoGrupoEconomico);
                        }
                    }
                    else
                    {
                        if (_tipo == "EMPRESA" || _tipo == "FORNECEDOR" || _tipo == "CLIENTE" || _tipo == "CLIENTE_FORNECEDOR")
                        {
                            empresaDAO.update(_codigo, Convert.ToInt32(HttpContext.Current.Session["empresa"]), _nome, _nomeFantasia,
                            _cnpjCpf, _endereco, _numero, _complemento, _bairro, _cep, _municipio, _telefone, _ieRg, _im, 0, _uf, _tipo, _consultor, _empresaTimesheet,
                            "JURIDICA", _sugestaoModeloCP, _sugestaoModeloCR, _sincronizar, _codigoMunicipio, _codigoPais, _nire, _cod_empresa_plano_contas, false, _grupoFinanceiroEntrada, _grupoFinanceiroSaida, codigoGrupoEconomico);
                        }
                        else if (_tipo == "EMITENTE")
                        {
                            empresaDAO.update(_codigo, Convert.ToInt32(HttpContext.Current.Session["empresa"]), _nome, _nomeFantasia,
                            _cnpjCpf, _endereco, _numero, _complemento, _bairro, _cep, _municipio, _telefone, _ieRg, _im, 0, _uf, _tipo, _consultor, _empresaTimesheet,
                            "JURIDICA", _sugestaoModeloCP, _sugestaoModeloCR, _sincronizar, _codigoMunicipio, _codigoPais, _nire, _cod_empresa_plano_contas, false, _grupoFinanceiroEntrada, _grupoFinanceiroSaida, codigoGrupoEconomico);
                        }
                        else
                        {
                            empresaDAO.update(_codigo, 0, _nome, _nomeFantasia,
                            _cnpjCpf, _endereco, _numero, _complemento, _bairro, _cep, _municipio, _telefone, _ieRg, _im, 0, _uf, _tipo, _consultor, _empresaTimesheet,
                            "JURIDICA", _sugestaoModeloCP, _sugestaoModeloCR, _sincronizar, _codigoMunicipio, _codigoPais, _nire, _cod_empresa_plano_contas, _exigeContribuicoes, _grupoFinanceiroEntrada, _grupoFinanceiroSaida, codigoGrupoEconomico);
                        }
                    }
                }
            }
        }
        return erros;
    }

    public virtual List<string> deletar()
    {
        erros = new List<string>();
        int Verifica_Empresa_Emitente = 0;

        if (_codigo == 0)
            erros.Add("Código inválido.");

        if (erros.Count == 0)
        {
            Verifica_Empresa_Emitente = empresaDAO.Verifica_Empresa_Emitente(_codigo);
            empresaDAO.delete(_codigo);
        }

        if (Verifica_Empresa_Emitente > 0)
            empresaDAO.Deleta_Tabela_Faturamento_NF_Emitente(_codigo);

        return erros;
    }

    public virtual List<string> load()
    {
        erros = new List<string>();

        if (_codigo == 0)
            erros.Add("Código inválido.");

        if (erros.Count == 0)
        {
            DataTable linha = empresaDAO.load(_codigo);
            if (linha.Rows.Count > 0)
            {
                _nome = linha.Rows[0]["NOME_RAZAO_SOCIAL"].ToString();
                _nomeFantasia = linha.Rows[0]["NOME_FANTASIA"].ToString();
                _cnpjCpf = linha.Rows[0]["CNPJ_CPF"].ToString();
                _endereco = linha.Rows[0]["ENDERECO"].ToString();
                _numero = linha.Rows[0]["NUMERO"].ToString();
                _complemento = linha.Rows[0]["COMPLEMENTO"].ToString();
                _bairro = linha.Rows[0]["BAIRRO"].ToString();
                _cep = linha.Rows[0]["CEP"].ToString();
                _municipio = linha.Rows[0]["MUNICIPIO"].ToString();
                _telefone = linha.Rows[0]["TELEFONE"].ToString();
                _ieRg = linha.Rows[0]["IE_RG"].ToString();
                _im = linha.Rows[0]["IM"].ToString();
                _uf = linha.Rows[0]["UF"].ToString();
                _tipo = linha.Rows[0]["TIPO"].ToString();
                _consultor = Convert.ToInt32(linha.Rows[0]["COD_CONSULTOR"]);
                _empresaTimesheet = Convert.ToInt32(linha.Rows[0]["COD_EMPRESA_TIMESHEET"]);
                _sugestaoModeloCP = Convert.ToInt32(linha.Rows[0]["SUGESTAO_MODELO_CP"]);
                _sugestaoModeloCR = Convert.ToInt32(linha.Rows[0]["SUGESTAO_MODELO_CR"]);
                _sincronizar = Convert.ToBoolean(linha.Rows[0]["SINCRONIZA"]);
                _codigoMunicipio = linha.Rows[0]["COD_MUNICIPIO"].ToString();
                _codigoPais = linha.Rows[0]["COD_PAIS"].ToString();
                _nire = linha.Rows[0]["NIRE"].ToString();
                _cod_empresa_plano_contas = Convert.ToInt32(linha.Rows[0]["COD_EMPRESA_PLANO_CONTAS"]);
                _exigeContribuicoes = Convert.ToBoolean(linha.Rows[0]["EXIGE_CONTRIBUICOES"]);
                _grupoFinanceiroEntrada = Convert.ToString(linha.Rows[0]["TIPO_CONTA_ENTRADA"]);
                _grupoFinanceiroSaida = Convert.ToString(linha.Rows[0]["TIPO_CONTA_SAIDA"]);
                _codigoGrupoEconomico = Convert.IsDBNull(linha.Rows[0]["COD_GRUPO_ECONOMICO"]) ? 0 : Convert.ToInt32(linha.Rows[0]["COD_GRUPO_ECONOMICO"]);

                if (linha.Rows[0]["FISICA_JURIDICA"].ToString() == "FISICA")
                    _fisicaJuridica = ETipoEmpresa.FISICA;
                else
                    _fisicaJuridica = ETipoEmpresa.JURIDICA;
            }
        }

        return erros;
    }

    public List<string> insereUsuarioEmpresa(Usuario usuario)
    {
        erros = new List<string>();

        if (_codigo == 0)
            erros.Add("Código inválido.");

        if (erros.Count == 0)
        {
            //CRIA PERFIL ADMINISTRADOR PARA EMPRESA
            perfil.codigo = "ADMIN";
            perfil.descricao = "Administrador";
            perfil.novo(_codigo);

            //LIBERA ACESSO TOTAL AO PERFIL
            perfil.liberaAcessoTotal(_codigo);

            //CRIA USUÁRIO PARA A EMPRESA
            usuario.perfil = "ADMIN";
            usuario.inserir(_codigo);
        }

        return erros;
    }

    public void listaPaginada(ref DataTable tb, string tipo, string fisicaJuridica, string nomeFantasia,
        string nomeRazaoSocial, string cnpjCpf, string grupoFinanceiro, int codigoGrupoEconomico, int paginaAtual, string ordenacao)
    {
        empresaDAO.lista(ref tb, tipo, fisicaJuridica, nomeFantasia, nomeRazaoSocial, cnpjCpf, grupoFinanceiro, codigoGrupoEconomico, paginaAtual, ordenacao);
    }

    public int totalRegistros(string tipo, string fisicaJuridica, string nomeFantasia,
        string nomeRazaoSocial, string cnpjCpf, string grupoFinanceiro, int codigoGrupoEconomico)
    {
        return empresaDAO.totalRegistros(tipo, fisicaJuridica, nomeFantasia, nomeRazaoSocial, cnpjCpf, grupoFinanceiro, codigoGrupoEconomico);
    }

    private string limpaString(string texto)
    {
        return texto.Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("/", "");
    }

    public List<string> sincroniza()
    {
        erros = new List<string>();
        DataSet dsClientes = new DataSet("dsClientes");
        DataSet dsFornecedores = new DataSet("dsFornecedores");
        DataTable tbLocal = new DataTable("tbLocal");

        try
        {
            empresaDAO.listaClientesTimesheet(ref dsClientes);
            empresaDAO.listaFornecedoresTimesheet(ref dsFornecedores);
            empresaDAO.lista_Empresas(ref tbLocal);

            //SINCRONIZA CLIENTE
            for (int i = 0; i < dsClientes.Tables[0].Rows.Count; i++)
            {
                bool existe = false;
                int codigoEmpresaLocal = 0;
                for (int x = 0; x < tbLocal.Rows.Count; x++)
                {
                    if (Convert.ToInt32(dsClientes.Tables[0].Rows[i]["CodCliente"]) == Convert.ToInt32(tbLocal.Rows[x]["COD_EMPRESA_TIMESHEET"]))
                    {
                        existe = true;
                        codigoEmpresaLocal = Convert.ToInt32(tbLocal.Rows[x]["COD_EMPRESA"]);
                        break;
                    }
                }

                if (!existe)
                {
                    try
                    {
                        empresaDAO.insert(Convert.ToInt32(HttpContext.Current.Session["empresa"]),
                        dsClientes.Tables[0].Rows[i]["NomeCliente"].ToString(),
                        dsClientes.Tables[0].Rows[i]["FantasiaCliente"].ToString(), limpaString(dsClientes.Tables[0].Rows[i]["CnpjCliente"].ToString()),
                        dsClientes.Tables[0].Rows[i]["EndCliente"].ToString(), "", limpaString(dsClientes.Tables[0].Rows[i]["CepCliente"].ToString()),
                        /*dsClientes.Tables[0].Rows[i]["nroCliente"].ToString()*/ string.Empty,
                        /*dsClientes.Tables[0].Rows[i]["compleCliente"].ToString()*/ string.Empty,
                        dsClientes.Tables[0].Rows[i]["CidCliente"].ToString(), limpaString(dsClientes.Tables[0].Rows[i]["TelCliente"].ToString()),
                        limpaString(dsClientes.Tables[0].Rows[i]["InscCliente"].ToString()), "", 0, dsClientes.Tables[0].Rows[i]["EstCliente"].ToString(),
                        "CLIENTE", 0, Convert.ToInt32(dsClientes.Tables[0].Rows[i]["CodCliente"]),
                        "JURIDICA", 0, 0, false, "0", "0", "0", _cod_empresa_plano_contas, false, _grupoFinanceiroEntrada, _grupoFinanceiroSaida, codigoGrupoEconomico);
                    }
                    catch (Exception ex)
                    {
                        erros.Add("Erro na inserção do cliente:" + codigoEmpresaLocal + "  " + ex.Message);
                    }
                }
                else
                {
                    bool igual = false;
                    for (int x = 0; x < tbLocal.Rows.Count; x++)
                    {
                        if (Convert.ToInt32(dsClientes.Tables[0].Rows[i]["CodCliente"]) == Convert.ToInt32(tbLocal.Rows[x]["COD_EMPRESA_TIMESHEET"])
                            && dsClientes.Tables[0].Rows[i]["NomeCliente"].ToString() == tbLocal.Rows[x]["NOME_RAZAO_SOCIAL"].ToString()
                            && dsClientes.Tables[0].Rows[i]["FantasiaCliente"].ToString() == tbLocal.Rows[x]["NOME_FANTASIA"].ToString()
                            && limpaString(dsClientes.Tables[0].Rows[i]["CnpjCliente"].ToString()) == tbLocal.Rows[x]["CNPJ_CPF"].ToString()
                            && dsClientes.Tables[0].Rows[i]["EndCliente"].ToString() == tbLocal.Rows[x]["ENDERECO"].ToString()
                            //&& dsClientes.Tables[0].Rows[i]["nroCliente"].ToString() == tbLocal.Rows[x]["NUMERO"].ToString()
                            //&& dsClientes.Tables[0].Rows[i]["compleCliente"].ToString() == tbLocal.Rows[x]["COMPLEMENTO"].ToString()
                            && limpaString(dsClientes.Tables[0].Rows[i]["CepCliente"].ToString()) == tbLocal.Rows[x]["CEP"].ToString()
                            && dsClientes.Tables[0].Rows[i]["CidCliente"].ToString() == tbLocal.Rows[x]["MUNICIPIO"].ToString()
                            && limpaString(dsClientes.Tables[0].Rows[i]["TelCliente"].ToString()) == tbLocal.Rows[x]["TELEFONE"].ToString()
                            && limpaString(dsClientes.Tables[0].Rows[i]["InscCliente"].ToString()) == tbLocal.Rows[x]["IE_RG"].ToString()
                            && dsClientes.Tables[0].Rows[i]["EstCliente"].ToString() == tbLocal.Rows[x]["UF"].ToString())
                        {
                            igual = true;
                            break;
                        }
                    }

                    if (!igual)
                    {
                        try
                        {
                                empresaDAO.update(codigoEmpresaLocal, Convert.ToInt32(HttpContext.Current.Session["empresa"]),
                                dsClientes.Tables[0].Rows[i]["NomeCliente"].ToString(),
                                dsClientes.Tables[0].Rows[i]["FantasiaCliente"].ToString(), limpaString(dsClientes.Tables[0].Rows[i]["CnpjCliente"].ToString()),
                                dsClientes.Tables[0].Rows[i]["EndCliente"].ToString(), "", limpaString(dsClientes.Tables[0].Rows[i]["CepCliente"].ToString()),
                                /*dsClientes.Tables[0].Rows[i]["nroCliente"].ToString()*/ string.Empty,
                                /*dsClientes.Tables[0].Rows[i]["compleCliente"].ToString()*/ string.Empty,
                                dsClientes.Tables[0].Rows[i]["CidCliente"].ToString(), limpaString(dsClientes.Tables[0].Rows[i]["TelCliente"].ToString()),
                                limpaString(dsClientes.Tables[0].Rows[i]["InscCliente"].ToString()), "", 0, dsClientes.Tables[0].Rows[i]["EstCliente"].ToString(),
                                "CLIENTE", 0, Convert.ToInt32(dsClientes.Tables[0].Rows[i]["CodCliente"]),
                                "JURIDICA", 0, 0, false, "0", "0", "0", _cod_empresa_plano_contas, false, _grupoFinanceiroEntrada, _grupoFinanceiroSaida, codigoGrupoEconomico);
                        }
                        catch (Exception ex)
                        {
                            erros.Add("Erro na alteração do cliente:" + codigoEmpresaLocal + "  " + ex.Message);
                        }
                    }
                }
            }

            //SINCRONIZA CONSULTORES
            for (int i = 0; i < dsFornecedores.Tables[0].Rows.Count; i++)
            {
                bool existe = false;
                int codigoEmpresaLocal = 0;
                for (int x = 0; x < tbLocal.Rows.Count; x++)
                {
                    if (Convert.ToInt32(dsFornecedores.Tables[0].Rows[i]["CodConsultor"]) == Convert.ToInt32(tbLocal.Rows[x]["COD_CONSULTOR"]))
                    {
                        existe = true;
                        codigoEmpresaLocal = Convert.ToInt32(tbLocal.Rows[x]["COD_EMPRESA"]);
                        break;
                    }
                }

                if (!existe)
                {
                    try
                    {
                        empresaDAO.insert(Convert.ToInt32(HttpContext.Current.Session["empresa"]),
                        dsFornecedores.Tables[0].Rows[i]["NomeConsultor"].ToString(), "", limpaString(dsFornecedores.Tables[0].Rows[i]["cpfConsultor"].ToString()),
                        dsFornecedores.Tables[0].Rows[i]["EndConsultor"].ToString(), "", "",
                        dsFornecedores.Tables[0].Rows[i]["NroConsultor"].ToString(),
                        dsFornecedores.Tables[0].Rows[i]["compleConsultor"].ToString(),
                        dsFornecedores.Tables[0].Rows[i]["CidConsultor"].ToString(), limpaString(dsFornecedores.Tables[0].Rows[i]["TelResConsultor"].ToString()), limpaString(dsFornecedores.Tables[0].Rows[i]["rgConsultor"].ToString()), "", 0,
                        dsFornecedores.Tables[0].Rows[i]["EstConsultor"].ToString(),
                        "FORNECEDOR", Convert.ToInt32(dsFornecedores.Tables[0].Rows[i]["CodConsultor"]), 0,
                        "FISICA", 0, 0, false, "0", "0", "0", _cod_empresa_plano_contas, false, _grupoFinanceiroEntrada, _grupoFinanceiroSaida, codigoGrupoEconomico);
                    }
                    catch (Exception ex)
                    {
                        erros.Add("Erro na inserção do fornecedor:" + codigoEmpresaLocal + "  " + ex.Message);
                    }
                }
                else
                {
                    bool igual = false;
                    for (int x = 0; x < tbLocal.Rows.Count; x++)
                    {
                        if (Convert.ToInt32(dsFornecedores.Tables[0].Rows[i]["CodConsultor"]) == Convert.ToInt32(tbLocal.Rows[x]["COD_CONSULTOR"])
                            && dsFornecedores.Tables[0].Rows[i]["NomeConsultor"].ToString() == tbLocal.Rows[x]["NOME_RAZAO_SOCIAL"].ToString()
                            && limpaString(dsFornecedores.Tables[0].Rows[i]["cpfConsultor"].ToString()) == tbLocal.Rows[x]["CNPJ_CPF"].ToString()
                            && dsFornecedores.Tables[0].Rows[i]["EndConsultor"].ToString() == tbLocal.Rows[x]["ENDERECO"].ToString()
                            && dsFornecedores.Tables[0].Rows[i]["CidConsultor"].ToString() == tbLocal.Rows[x]["MUNICIPIO"].ToString()
                            && limpaString(dsFornecedores.Tables[0].Rows[i]["TelResConsultor"].ToString()) == tbLocal.Rows[x]["TELEFONE"].ToString()
                            && limpaString(dsFornecedores.Tables[0].Rows[i]["rgConsultor"].ToString()) == tbLocal.Rows[x]["IE_RG"].ToString()
                            && dsFornecedores.Tables[0].Rows[i]["EstConsultor"].ToString() == tbLocal.Rows[x]["UF"].ToString())
                        {
                            igual = true;
                            break;
                        }
                    }

                    if (!igual)
                    {
                        try
                        {
                                empresaDAO.update(codigoEmpresaLocal, Convert.ToInt32(HttpContext.Current.Session["empresa"]),
                                dsFornecedores.Tables[0].Rows[i]["NomeConsultor"].ToString(), "", limpaString(dsFornecedores.Tables[0].Rows[i]["cpfConsultor"].ToString()),
                                dsFornecedores.Tables[0].Rows[i]["EndConsultor"].ToString(), "", "",
                                dsFornecedores.Tables[0].Rows[i]["nroConsultor"].ToString(),
                                dsFornecedores.Tables[0].Rows[i]["compleConsultor"].ToString(),
                                dsFornecedores.Tables[0].Rows[i]["CidConsultor"].ToString(), limpaString(dsFornecedores.Tables[0].Rows[i]["TelResConsultor"].ToString()), limpaString(dsFornecedores.Tables[0].Rows[i]["rgConsultor"].ToString()), "", 0,
                                dsFornecedores.Tables[0].Rows[i]["EstConsultor"].ToString(),
                                "FORNECEDOR", Convert.ToInt32(dsFornecedores.Tables[0].Rows[i]["CodConsultor"]), 0,
                                "FISICA", 0, 0, false, "0", "0", "0", _cod_empresa_plano_contas, false, _grupoFinanceiroEntrada, _grupoFinanceiroSaida, codigoGrupoEconomico);
                        }
                        catch (Exception ex)
                        {
                            erros.Add("Erro na alteração do fornecedor:" + codigoEmpresaLocal + "  " + ex.Message);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            erros.Add("Erro fatal: " + ex.Message);
        }

        return erros;
    }

    public void lista_Empresas(ref DataTable tb)
    {
        empresaDAO.lista_Empresas(ref tb);
    }

    public void listaConsultoresTimesheet(ref DataSet ds)
    {
        empresaDAO.listaFornecedoresTimesheet(ref ds);
    }

    public void listaEmpresasTimesheet(ref DataSet ds)
    {
        empresaDAO.listaClientesTimesheet(ref ds);
    }

    public void listaUsuarias(ref DataTable tb)
    {
        empresaDAO.lista_empresas_usuarios(ref tb);
    }

    public void listaUsuarias(ref DataTable tb, string login)
    {
        empresaDAO.lista_empresas_usuarios(ref tb, login);
    }

    public List<Hashtable> listaFornecedoresClientes()
    {
        return empresaDAO.listaFornecedoresClientes();
    }

    public void listaFornecedoresClientes(ref DataTable tb)
    {
        empresaDAO.listaFornecedoresClientes(ref tb);
    }

    public void listaFornecedoresClientesBaixar(ref DataTable tb)
    {
        empresaDAO.listaFornecedoresClientesBaixar(ref tb);
    }

    public void listaConsultores(ref DataTable tb)
    {
        empresaDAO.listaConsultores(ref tb);
    }
}
