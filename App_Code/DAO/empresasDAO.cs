using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;

public class empresasDAO
{
    private WSTimesheet.Service ws = new WSTimesheet.Service();
    private Conexao _conn;

    public empresasDAO(Conexao c)
    {
        _conn = c;
    }

    public string retornaCNPJ()
    {
        string sql = "SELECT CNPJ_CPF FROM CAD_EMPRESAS WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        return Convert.ToString(_conn.scalar(sql));
    }

    public DataTable carregaEmpresa()
    {
        string sql = "select * from cad_empresas where COD_EMPRESA_PLANO_CONTAS=0 AND COD_EMPRESA_PAI=0 and cod_empresa in (select cod_empresa from cad_usuarios where login in (select login from cad_usuarios where cod_usuario = " + HttpContext.Current.Session["usuario"] + "))" ;
        return _conn.dataTable(sql, "empresa");
    }

    public DataTable deParaConsultor(int codigoTs)
    {
        string sql = "select * from cad_empresas where cod_consultor=" + codigoTs + " AND COD_EMPRESA_PAI=" + HttpContext.Current.Session["empresa"] + "";
        return _conn.dataTable(sql, "job");
    }

    public int insert(int cod_empresa_pai, string nome_razao_social, string nome_fantasia,
        string cnpj_cpf, string endereco, string numero, string complemento, string bairro, string cep, string municipio, string telefone,
        string ieRg, string im, double impostos, string uf, string tipo, int cod_consultor, int cod_empresa_timesheet, string fisica_juridica,
        int sugestao_modelo_cp, int sugestao_modelo_cr, bool sincroniza, string codigoMunicipio, string codigoPais, string nire, int cod_empresa_plano_contas, bool _exigeContribuicoes, string grupoFinanceiroEntrada, string grupoFinanceiroSaida, int codigoGrupoEconomico)
    {
        if (nome_fantasia.Trim() == "")
        {
            if (nome_razao_social.Length > 85)
                nome_fantasia = nome_razao_social.Substring(0, 85);
            else
                nome_fantasia = nome_razao_social;
        }

        string sql = "INSERT INTO CAD_EMPRESAS (COD_EMPRESA_PAI, NOME_RAZAO_SOCIAL, NOME_FANTASIA, CNPJ_CPF, ";
        sql += "ENDERECO, NUMERO, COMPLEMENTO, BAIRRO, CEP, MUNICIPIO, TELEFONE, IE_RG, IM, IMPOSTOS, UF, TIPO, COD_CONSULTOR, COD_EMPRESA_TIMESHEET, ";
        sql += "FISICA_JURIDICA, SUGESTAO_MODELO_CP, SUGESTAO_MODELO_CR, SINCRONIZA, COD_MUNICIPIO, COD_PAIS, NIRE, COD_EMPRESA_PLANO_CONTAS, EXIGE_CONTRIBUICOES, Tipo_Conta_Entrada, Tipo_Conta_Saida, COD_GRUPO_ECONOMICO) ";
        sql += "VALUES ";
        sql += "(" + cod_empresa_pai + ", '" + nome_razao_social.Replace("'", "''") + "', '" + nome_fantasia.Replace("'", "''") + "', '" + cnpj_cpf + "', ";
        sql += "'" + endereco.Replace("'", "''") + "', '" + numero.Replace("'", "''") + "', '" + complemento.Replace("'", "''") + "', '" + bairro.Replace("'", "''") + "', '" + cep + "', '" + municipio.Replace("'", "''") + "', '" + telefone + "', '" + ieRg + "', '" + im + "', " + impostos.ToString().Replace(",", ".") + ", ";
        sql += "'" + uf + "', '" + tipo + "', " + cod_consultor + ", " + cod_empresa_timesheet + ", '" + fisica_juridica + "', ";
        sql += sugestao_modelo_cp + ", " + sugestao_modelo_cr + ", " + Convert.ToInt32(sincroniza) + ", " + codigoMunicipio + ", " + codigoPais + ", '" + nire + "', " + cod_empresa_plano_contas + ", " + Convert.ToInt32(_exigeContribuicoes);
        sql += ", " + (string.IsNullOrEmpty(grupoFinanceiroEntrada) ? "NULL" : "'" + grupoFinanceiroEntrada + "'") + ", " + (string.IsNullOrEmpty(grupoFinanceiroSaida) ? "NULL" : "'" + grupoFinanceiroSaida + "'") + ", " + (codigoGrupoEconomico == 0 ? "NULL" : Convert.ToString(codigoGrupoEconomico)) + "); ";
        sql += "SELECT SCOPE_IDENTITY();";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void update(int cod_empresa, int cod_empresa_pai, string nome_razao_social, string nome_fantasia,
        string cnpj_cpf, string endereco, string numero, string complemento, string bairro, string cep, string municipio, string telefone,
        string ieRg, string im, double impostos, string uf, string tipo, int? cod_consultor, int cod_empresa_timesheet, string fisica_juridica,
        int sugestao_modelo_cp, int sugestao_modelo_cr, bool sincroniza, string codigoMunicipio, string codigoPais, string nire, int cod_empresa_plano_contas, bool exigeContribuicoes, string grupoFinanceiroEntrada, string grupoFinanceiroSaida, int codigoGrupoEconomico)
    {
        if (nome_fantasia.Trim() == "")
        {
            if (nome_razao_social.Length <= 85)
                nome_fantasia = nome_razao_social;
            else
                nome_fantasia = nome_razao_social.Substring(0, 85);
        }

        string sql = "UPDATE CAD_EMPRESAS SET COD_EMPRESA_PAI = " + cod_empresa_pai + ",";
        sql += " NOME_RAZAO_SOCIAL = '" + nome_razao_social.Replace("'", "''") + "', NOME_FANTASIA = '" + nome_fantasia.Replace("'", "''") + "', CNPJ_CPF = '" + cnpj_cpf + "',";
        sql += " ENDERECO = '" + endereco.Replace("'", "''") + "', NUMERO = '" + numero.Replace("'", "''") + "', COMPLEMENTO = '" + complemento.Replace("'", "''") + "', BAIRRO = '" + bairro.Replace("'", "''") + "', CEP = '" + cep + "', MUNICIPIO = '" + municipio.Replace("'", "''") + "',";
        sql += " TELEFONE = '" + telefone + "', IE_RG = '" + ieRg + "', IM = '" + im + "', IMPOSTOS = " + impostos.ToString().Replace(",", ".") + ", UF = '" + uf + "', TIPO = '" + tipo + "', " + (cod_consultor != null ? "COD_CONSULTOR = " + cod_consultor : "") + ", COD_EMPRESA_TIMESHEET = " + cod_empresa_timesheet + ", FISICA_JURIDICA = '" + fisica_juridica + "',";
        sql += " SUGESTAO_MODELO_CP = " + sugestao_modelo_cp + ", SUGESTAO_MODELO_CR = " + sugestao_modelo_cr + ", sincroniza = " + Convert.ToInt32(sincroniza) + ", COD_MUNICIPIO = " + codigoMunicipio + ", COD_PAIS = " + codigoPais + ", NIRE = '" + nire + "', COD_EMPRESA_PLANO_CONTAS = " + cod_empresa_plano_contas + ", EXIGE_CONTRIBUICOES = " + Convert.ToInt32(exigeContribuicoes);
        sql += ", Tipo_Conta_Entrada = " + (string.IsNullOrEmpty(grupoFinanceiroEntrada) ? "NULL" : "'" + grupoFinanceiroEntrada + "'") + ", Tipo_Conta_Saida = " + (string.IsNullOrEmpty(grupoFinanceiroSaida) ? "NULL" : "'" + grupoFinanceiroSaida + "'") + ", COD_GRUPO_ECONOMICO= " + (codigoGrupoEconomico == 0 ? "NULL" : Convert.ToString(codigoGrupoEconomico));
        sql += " WHERE COD_EMPRESA = " + cod_empresa;
        _conn.execute(sql);
    }

    public void delete(int cod_empresa)
    {
        string sql = "DELETE FROM CAD_EMPRESAS ";
        sql += "WHERE COD_EMPRESA = " + cod_empresa + "";

        _conn.execute(sql);
    }

    public DataTable dePara(int codigoTs)
    {
        string sql = "select * from cad_empresas where cod_empresa_timesheet=" + codigoTs + " and cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + "";
        return _conn.dataTable(sql, "empresa");
    }

    public DataTable load(int cod_empresa)
    {
        string sql = "SELECT * FROM CAD_EMPRESAS WHERE COD_EMPRESA=" + cod_empresa + "";
        return _conn.dataTable(sql, "empresa");
    }

    public DataTable loadSped(int cod_empresa)
    {
        string sql = "SELECT ce.COD_EMPRESA"
          + ", ce.COD_EMPRESA_PAI"
          + ", ce.NOME_RAZAO_SOCIAL"
          + ", ce.NOME_FANTASIA"
          + ", ce.CNPJ_CPF"
          + ", ce.ENDERECO"
          + ", ce.BAIRRO"
          + ", ce.CEP"
          + ", ce.MUNICIPIO"
          + ", ce.TELEFONE"
          + ", ce.IE_RG"
          + ", ce.UF"
          + ", ce.TIPO"
          + ", ce.COD_CONSULTOR"
          + ", ce.COD_EMPRESA_TIMESHEET"
          + ", ce.FISICA_JURIDICA"
          + ", ce.SUGESTAO_MODELO_CP"
          + ", ce.SUGESTAO_MODELO_CR"
          + ", ce.SINCRONIZA"
          + ", cm.COD_MUNICIPIO"
          + ", ce.IM"
          + ", ce.SUFRAMA"
          + ", cp.COD_PAIS"
          + ", ce.NUMERO"
          + ", ce.COMPLEMENTO"
          + ", ce.NIRE"
          + ", ce.COD_EMPRESA_PLANO_CONTAS"
          + ", ce.REGIME_CAIXA"
          + ", ce.IMPOSTOS"
          + " FROM CAD_EMPRESAS ce left join CAD_PAIS cp on ce.COD_PAIS = cp.ID_PAIS left join  CAD_MUNICIPIO cm on ce.COD_MUNICIPIO = cm.ID_MUNICIPIO"
          + " WHERE ce.COD_EMPRESA = " + cod_empresa + "";

        return _conn.dataTable(sql, "empresa");
    }

    public void lista_Empresas(ref DataTable tb)
    {
        string sql = "SELECT * FROM CAD_EMPRESAS WHERE COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"] + " ORDER BY NOME_RAZAO_SOCIAL";
        _conn.fill(sql, ref tb);
    }

    public void lista_empresas_usuarios(ref DataTable tb)
    {
        string sql = "select * from CAD_EMPRESAS WHERE 1=1 ";

        sql += " and (COD_EMPRESA_PAI=0 OR COD_EMPRESA_PAI IS NULL)";
        sql += " AND TIPO = 'GRUPO' ";

        _conn.fill(sql, ref tb);
    }

    public void lista_empresas_usuarios(ref DataTable tb, string login)
    {
        string sql = "select * from CAD_EMPRESAS, CAD_USUARIOS WHERE 1=1 ";
        sql += " and cad_usuarios.login = '" + login + "' and cad_usuarios.cod_empresa = CAD_EMPRESAS.cod_empresa ";
        sql += " and (COD_EMPRESA_PAI=0 OR COD_EMPRESA_PAI IS NULL)";
        sql += " AND TIPO = 'GRUPO' order by NOME_RAZAO_SOCIAL ";

        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, string tipo, string fisicaJuridica, string nomeFantasia,
        string nomeRazaoSocial, string cnpjCpf, string grupoFinanceiro, int grupoEconomico, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "COD_EMPRESA DESC";

        string sql = "select * from (SELECT  ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao;
        sql += ") AS Row, *  ";
        sql += "    FROM CAD_EMPRESAS WHERE 1=1 ";

        if (tipo != null)
            sql += " AND TIPO = '" + tipo + "'";

        if (fisicaJuridica != null)
            sql += " AND FISICA_JURIDICA = '" + fisicaJuridica + "'";

        if (nomeFantasia != null && nomeRazaoSocial != null)
        {
            sql += " AND (NOME_FANTASIA LIKE '%" + nomeFantasia.Replace("'", "''") + "%' OR NOME_RAZAO_SOCIAL LIKE '%" + nomeRazaoSocial.Replace("'", "''") + "%') ";
        }
        else
        {
            if (nomeFantasia != null)
                sql += " AND NOME_FANTASIA LIKE '%" + nomeFantasia.Replace("'", "''") + "%'";

            if (nomeRazaoSocial != null)
                sql += " AND NOME_RAZAO_SOCIAL LIKE '%" + nomeRazaoSocial.Replace("'", "''") + "%'";
        }

        if (cnpjCpf != null)
            sql += " AND CNPJ_CPF = '" + cnpjCpf + "'";

        if (grupoFinanceiro != null)
            sql += " AND (TIPO_CONTA_ENTRADA LIKE '%" + grupoFinanceiro + "%' OR TIPO_CONTA_SAIDA LIKE '%" + grupoFinanceiro + "%')";

        if (grupoEconomico > 0)
            sql += " AND COD_GRUPO_ECONOMICO = " + grupoEconomico;

        sql += " and (COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"] + " OR COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + ")";


        sql += "    )as vw where 1=1 ";

        //PAGINACAO
        sql += " AND vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }

    public int totalRegistros(string tipo, string fisicaJuridica, string nomeFantasia,
        string nomeRazaoSocial, string cnpjCpf, string grupoFinanceiro, int grupoEconomico)
    {
        string sql = "select count(COD_EMPRESA) from CAD_EMPRESAS WHERE 1=1 ";

        sql += " and (COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"] + " OR COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + ")";

        if (tipo != null)
            sql += " AND TIPO = '" + tipo + "'";

        if (fisicaJuridica != null)
            sql += " AND FISICA_JURIDICA = '" + fisicaJuridica + "'";

        if (nomeFantasia != null)
            sql += " AND NOME_FANTASIA LIKE '%" + nomeFantasia.Replace("'", "''") + "%'";

        if (nomeRazaoSocial != null)
            sql += " AND NOME_RAZAO_SOCIAL LIKE '%" + nomeRazaoSocial.Replace("'", "''") + "%'";

        if (cnpjCpf != null)
            sql += " AND CNPJ_CPF = '" + cnpjCpf + "'";

        if (grupoFinanceiro != null)
            sql += " AND (TIPO_CONTA_ENTRADA LIKE '%" + grupoFinanceiro + "%' OR TIPO_CONTA_SAIDA LIKE '%" + grupoFinanceiro + "%')";

        
        if (grupoEconomico > 0)
            sql += " AND COD_GRUPO_ECONOMICO = " + grupoEconomico;


        return Convert.ToInt32(_conn.scalar(sql));
    }

    public List<Hashtable> listaFornecedores()
    {
        string sql = "select * from CAD_EMPRESAS WHERE 1=1 ";
        sql += " and COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"] + " AND (TIPO='FORNECEDOR' OR TIPO='CLIENTE_FORNECEDOR') ORDER BY NOME_RAZAO_SOCIAL";

        return _conn.reader(sql);
    }

    public void listaFornecedores(ref DataTable tb)
    {
        string sql = "select * from CAD_EMPRESAS WHERE 1=1 ";
        sql += " and COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"] + " AND (TIPO='FORNECEDOR' OR TIPO='CLIENTE_FORNECEDOR') ORDER BY NOME_RAZAO_SOCIAL";

        _conn.fill(sql, ref tb);
    }

    public List<Hashtable> listaClientes()
    {
        string sql = "select * from CAD_EMPRESAS WHERE 1=1 ";
        sql += " and COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"] + " AND (TIPO='CLIENTE' OR TIPO='CLIENTE_FORNECEDOR') ORDER BY nome_razao_social";
        return _conn.reader(sql);
    }

    public void listaClientes(ref DataTable tb)
    {
        string sql = "SELECT * FROM CAD_EMPRESAS WHERE COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"];
        sql += " AND (TIPO = 'CLIENTE' OR TIPO = 'CLIENTE_FORNECEDOR' OR TIPO = 'FORNECEDOR') ORDER BY NOME_RAZAO_SOCIAL";

        _conn.fill(sql, ref tb);
    }

    public void lista_Tomadores(ref DataTable tb)
    {
        string sql = "SELECT COD_EMPRESA, ";
        sql += "(CASE WHEN LEN(CNPJ_CPF) = 14 THEN NOME_RAZAO_SOCIAL + ' - ' +  LEFT(CNPJ_CPF, 2) + '.' + SUBSTRING(CNPJ_CPF, 3, 3) + '.' + SUBSTRING(CNPJ_CPF, 6, 3) + '/' + SUBSTRING(CNPJ_CPF, 9, 4) + '-' + RIGHT(CNPJ_CPF, 2) ";
        sql += "WHEN LEN(CNPJ_CPF) = 11 THEN NOME_RAZAO_SOCIAL + ' - ' +  LEFT(CNPJ_CPF, 3) + '.' + SUBSTRING(CNPJ_CPF, 4, 3) + '.' + SUBSTRING(CNPJ_CPF, 7, 3) + '-' + RIGHT(CNPJ_CPF, 2) ";
        sql += "ELSE NOME_RAZAO_SOCIAL END) AS NOME_RAZAO_SOCIAL ";
        sql += "FROM CAD_EMPRESAS ";
        sql += "WHERE COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"] + " AND (TIPO = 'CLIENTE' OR TIPO = 'CLIENTE_FORNECEDOR') ";
        sql += "ORDER BY NOME_RAZAO_SOCIAL";

        _conn.fill(sql, ref tb);
    }

    public void listaClientes(ref DataTable tb, string colunaOrdenar)
    {
        string sql = "select * from CAD_EMPRESAS WHERE 1=1 ";
        sql += " and COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"] + " AND (TIPO='CLIENTE' OR TIPO='CLIENTE_FORNECEDOR') ORDER BY " + colunaOrdenar + "";

        _conn.fill(sql, ref tb);
    }

    public List<Hashtable> listaClientesJob(int codigoJob)
    {
        string sql = "select CAD_EMPRESAS.* from CAD_EMPRESAS, CAD_JOBS  WHERE CAD_JOBS.COD_JOB = " + codigoJob + " AND CAD_JOBS.COD_CLIENTE = CAD_EMPRESAS.COD_EMPRESA ";
        sql += " and COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"] + " AND (TIPO='CLIENTE' OR TIPO='CLIENTE_FORNECEDOR') ORDER BY nome_razao_social";
        return _conn.reader(sql);
    }

    public void lista_Emitentes(ref DataTable tb)
    {
        string sql = "SELECT COD_EMPRESA, NOME_RAZAO_SOCIAL FROM CAD_EMPRESAS WHERE COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"];
        sql += " AND TIPO = 'EMITENTE' ORDER BY NOME_RAZAO_SOCIAL";

        _conn.fill(sql, ref tb);
    }

    public List<Hashtable> listaFornecedoresClientes()
    {
        string sql = "select * from CAD_EMPRESAS WHERE 1=1 ";
        sql += " and COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"] + " AND (TIPO='CLIENTE' OR TIPO='FORNECEDOR' OR TIPO='CLIENTE_FORNECEDOR') ORDER BY NOME_RAZAO_SOCIAL";

        return _conn.reader(sql);
    }

    public void listaFornecedoresClientes(ref DataTable tb)
    {
        string sql = "select *, (CASE WHEN FISICA_JURIDICA = 'JURIDICA' THEN NOME_FANTASIA + ' - ' + NOME_RAZAO_SOCIAL ELSE NOME_RAZAO_SOCIAL END) as DESCRICAO_MISTA from CAD_EMPRESAS WHERE 1=1 ";
        sql += " and COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"] + " AND (TIPO='CLIENTE' OR TIPO='FORNECEDOR' OR TIPO='CLIENTE_FORNECEDOR') ORDER BY NOME_RAZAO_SOCIAL";

        _conn.fill(sql, ref tb);
    }

    public DataTable listaEmpresasPeriodo(DateTime inicio, DateTime termino, int codEmpresa)
    {
        string sql = "select * from CAD_EMPRESAS " +
                    " where COD_EMPRESA in(select cod_terceiro from LANCTOS_CONTAB where DATA >='" + inicio.ToString("yyyyMMdd") + "' and DATA <= '" + termino.ToString("yyyyMMdd") + "' and PENDENTE='False' and cod_empresa=" + codEmpresa + ") " +
                    " or cod_empresa in(select COD_CLIENTE from LANCTOS_CONTAB where DATA >='" + inicio.ToString("yyyyMMdd") + "' and DATA <= '" + termino.ToString("yyyyMMdd") + "' and PENDENTE='False' and cod_empresa=" + codEmpresa + ") ";
        return _conn.dataTable(sql, "dados");
    }
    //Colocado pelo Alberto

    public DataTable listaEmpresasPeriodo(bool ComMovimento, DateTime inicio, DateTime termino, int codEmpresa)
    {
        string sql = "select CAD_MUNICIPIO.COD_MUNICIPIO as IBGE_mun, CAD_PAIS.COD_PAIS as IBGE_PAIS, cad_empresas.* from CAD_EMPRESAS, CAD_MUNICIPIO, cad_pais " +
        " where  CAD_EMPRESAS.COD_MUNICIPIO = CAD_MUNICIPIO.ID_MUNICIPIO and CAD_PAIS.ID_PAIS = CAD_EMPRESAS.COD_PAIS and " +
        " (COD_EMPRESA in(select cod_terceiro from LANCTOS_CONTAB where DATA >='" + inicio.ToString("yyyyMMdd") + "' and DATA <= '" + termino.ToString("yyyyMMdd") + "' and PENDENTE='False' and cod_empresa=" + codEmpresa + " and lote in (select distinct lote from CAD_NOTAS_FISCAIS " +
        " union " +
        " select distinct lote from LANCTOS_CONTAB where COD_CONTA in " +
        " (select cod_conta from CAD_CONTAS where COD_EMPRESA = " + codEmpresa + " and RECIBO = 1) " +
        " )) " +
        " or cod_empresa in(select COD_CLIENTE from LANCTOS_CONTAB where DATA >='" + inicio.ToString("yyyyMMdd") + "' and DATA <= '" + termino.ToString("yyyyMMdd") + "' and PENDENTE='False' and cod_empresa=" + codEmpresa + " and lote in (select distinct lote from CAD_NOTAS_FISCAIS))) ";
        return _conn.dataTable(sql, "dados");
    }

    public void listaConsultores(ref DataTable tb)
    {
        string sql = "select * from CAD_EMPRESAS WHERE 1=1 ";
        sql += " and COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"] + " AND (TIPO='CLIENTE' OR TIPO='FORNECEDOR' OR TIPO='CLIENTE_FORNECEDOR' OR TIPO='EMITENTE') AND (COD_CONSULTOR > 0 OR TIPO='EMITENTE') ORDER BY NOME_RAZAO_SOCIAL";

        _conn.fill(sql, ref tb);
    }

    public void listaFornecedoresClientesBaixar(ref DataTable tb)
    {
        string sql = "select *, (CASE WHEN FISICA_JURIDICA = 'JURIDICA' THEN NOME_FANTASIA + ' \\ ' + NOME_RAZAO_SOCIAL ELSE NOME_RAZAO_SOCIAL END) as DESCRICAO_MISTA from CAD_EMPRESAS WHERE 1=1 ";
        sql += " and COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"] + " AND (TIPO='CLIENTE' OR TIPO='FORNECEDOR' OR TIPO='CLIENTE_FORNECEDOR') AND cod_empresa in (select cod_terceiro from lanctos_contab where pendente='True') ORDER BY NOME_RAZAO_SOCIAL ";

        _conn.fill(sql, ref tb);
    }

    public void listaFornecedoresTimesheet(ref DataSet ds)
    {
        ds = ws.getCadastroConsultores();
    }

    public void listaClientesTimesheet(ref DataSet ds)
    {
        ds = ws.getCadastroClientes();
    }

    public int getCodigoClienteXTimeSheet(int codClienteTimesheet)
    {
        string sql = "SELECT COD_EMPRESA FROM CAD_EMPRESAS WHERE COD_EMPRESA_TIMESHEET=" + codClienteTimesheet + " AND FISICA_JURIDICA='JURIDICA'  AND COD_EMPRESA_PAI=" + HttpContext.Current.Session["empresa"] + "";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public DataTable loadcnpjcpf(int cnpjcpf, int codigoempresa)
    {
        string sql = "SELECT * FROM CAD_EMPRESAS WHERE CNPJ_CPF=" + cnpjcpf + " and COD_EMPRESA_PAI =" + codigoempresa + "";
        return _conn.dataTable(sql, "empresa");
    }

    public string retornanomesempresas(string empresas)
    {
        string texto = "";
        string[] listcod = empresas.Split(',');
        foreach (string l in listcod)
        {
            string sql = "SELECT NOME_RAZAO_SOCIAL FROM CAD_EMPRESAS WHERE COD_EMPRESA = " + l + "";
            texto += " <br> " + _conn.scalar(sql);

        }
        return texto.Substring(6);
    }

    public bool retornaRegimeCaixa()
    {
        return Convert.ToBoolean(_conn.scalar("select REGIME_CAIXA from cad_empresas where cod_empresa = " + HttpContext.Current.Session["empresa"]));
    }

    public void Cria_Tabela_Faturamento_NF_Emitente(int cod_empresa)
    {
        string sql = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'FATURAMENTO_NF_EMITENTE_" + cod_empresa + "') AND type in (N'U')) ";
        sql += "BEGIN ";
        sql += "CREATE TABLE FATURAMENTO_NF_EMITENTE_" + cod_empresa + "(";
        sql += "[NUMERO_NF] [int] IDENTITY(1,1) NOT NULL, ";
        sql += "[TEMP] [bit] NULL, ";
        sql += "CONSTRAINT PK_FATURAMENTO_NF_EMITENTE_" + cod_empresa + " PRIMARY KEY CLUSTERED ";
        sql += "([NUMERO_NF] ASC) ";
        sql += "WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]";
        sql += ") ON [PRIMARY] ";
        sql += "END";

        _conn.execute(sql);
    }

    public int Verifica_Empresa_Emitente(int cod_empresa)
    {
        string sql = "SELECT TOP 1 * FROM CAD_EMPRESAS WHERE COD_EMPRESA = " + cod_empresa + " AND COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"] + " AND TIPO = 'EMITENTE'";
        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void Deleta_Tabela_Faturamento_NF_Emitente(int cod_empresa)
    {
        string sql = "DROP TABLE FATURAMENTO_NF_EMITENTE_" + cod_empresa;
        _conn.execute(sql);
    }

    public DataTable Busca_Empresa_Emitente_Tomador(int cod_empresa)
    {
        string sql = "SELECT NOME_RAZAO_SOCIAL, FISICA_JURIDICA, CNPJ_CPF, IM, IE_RG, ENDERECO, NUMERO, COMPLEMENTO, BAIRRO, MUNICIPIO, UF, CEP ";
        sql += "FROM CAD_EMPRESAS WHERE COD_EMPRESA = " + cod_empresa;

        return _conn.dataTable(sql, "empresa");
    }
}