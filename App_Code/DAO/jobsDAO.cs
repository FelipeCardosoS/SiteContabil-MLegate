using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;

public class jobsDAO
{
    private WSTimesheet.Service ws = new WSTimesheet.Service();
    private Conexao _conn;

	public jobsDAO(Conexao c)
	{
        _conn = c;
	}

    public void AtualizaTabelaJobs()
    {
        //Foi criado em Set/2019, para adicionar novos campos na tabela "CAD_JOBS".
        //Como possui Banco de Dados externos (clientes) é para assegurar que não dê erro de SQL.

        string sql = @"ALTER TABLE CAD_JOBS ADD Cod_Medico INT DEFAULT 0 NOT NULL, Cod_Equipe INT DEFAULT 0 NOT NULL, Cod_Vendedor INT DEFAULT 0 NOT NULL,
                       Paciente VARCHAR(2000) DEFAULT '' NOT NULL, Convenio VARCHAR(2000) DEFAULT '' NOT NULL, OBSERVACAO_NOTA_FISCAL VARCHAR(MAX) NULL";

        try
        {
            _conn.execute(sql);
        }
        catch { }

        //Criado em Jun/2021.
        sql = @"ALTER TABLE CAD_JOBS ADD OBSERVACAO_NOTA_FISCAL VARCHAR(MAX) NULL";

        try
        {
            _conn.execute(sql);
        }
        catch { }
    }

    public DataTable dePara(int codigoTs)
    {
        string sql = "SELECT * FROM CAD_JOBS WHERE COD_REFERENCIA = " + codigoTs + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        return _conn.dataTable(sql, "job");
    }

    //public void insert(int cod_cliente, int cod_linha_negocio, int cod_divisao, string nome, string descricao, string obs_nf, char status, decimal valor_faturamento,
    //    string data_proposta, string data_resposta, string data_inicio, string data_fim, char status_resposta,
    //    int cod_referencia, int medico, int equipe, int vendedor, string paciente, string convenio)
    //{
    //    AtualizaTabelaJobs();

    //    string sql = "INSERT INTO CAD_JOBS (COD_EMPRESA, COD_CLIENTE, COD_LINHA_NEGOCIO, COD_DIVISAO, NOME, DESCRICAO, OBSERVACAO_NOTA_FISCAL, STATUS, VALOR_FATURAMENTO, DATA_PROPOSTA, ";
    //    sql += "DATA_RESPOSTA, DATA_INICIO, DATA_FIM, STATUS_RESPOSTA, COD_REFERENCIA, Cod_Medico, Cod_Equipe, Cod_Vendedor, Paciente, Convenio) VALUES (";
    //    sql += HttpContext.Current.Session["empresa"] + ", " + cod_cliente + ", " + cod_linha_negocio + ", " + cod_divisao + ", '" + nome.Replace("'", "''") + "', '";
    //    sql += descricao.Replace("'", "''") + "', " + (string.IsNullOrEmpty(obs_nf) ? "NULL" : "'" + obs_nf.Replace("'", "''") + "'") + ", '" + status + "', " + valor_faturamento.ToString().Replace(",", ".") + ", '" + data_proposta.Replace("'", "''") + "', '";
    //    sql += data_resposta.Replace("'", "''") + "', '" + data_inicio.Replace("'", "''") + "', '" + data_fim.Replace("'", "''") + "', '" + status_resposta + "', ";
    //    sql += cod_referencia + ", " + medico + ", " + equipe + ", " + vendedor + ", '";
    //    sql += (string.IsNullOrEmpty(paciente) ? "" : paciente.Replace("'", "''")) + "', '";
    //    sql += (string.IsNullOrEmpty(convenio) ? "" : convenio.Replace("'", "''")) + "')";

    //    _conn.execute(sql);
    //}

    public int insert(int cod_cliente, int cod_linha_negocio, int cod_divisao, string nome, string descricao, string obs_nf, char status, decimal valor_faturamento,
        string data_proposta, string data_resposta, string data_inicio, string data_fim, char status_resposta,
        int cod_referencia, int medico, int equipe, int vendedor, string paciente, string convenio)
    {
        AtualizaTabelaJobs();

        string sql = "INSERT INTO CAD_JOBS (COD_EMPRESA, COD_CLIENTE, COD_LINHA_NEGOCIO, COD_DIVISAO, NOME, DESCRICAO, OBSERVACAO_NOTA_FISCAL, STATUS, VALOR_FATURAMENTO, DATA_PROPOSTA, ";
        sql += "DATA_RESPOSTA, DATA_INICIO, DATA_FIM, STATUS_RESPOSTA, COD_REFERENCIA, Cod_Medico, Cod_Equipe, Cod_Vendedor, Paciente, Convenio) OUTPUT INSERTED.COD_JOB VALUES (";
        sql += HttpContext.Current.Session["empresa"] + ", " + cod_cliente + ", " + cod_linha_negocio + ", " + cod_divisao + ", '" + nome.Replace("'", "''") + "', '";
        sql += descricao.Replace("'", "''") + "', " + (string.IsNullOrEmpty(obs_nf) ? "NULL" : "'" + obs_nf.Replace("'", "''") + "'") + ", '" + status + "', " + valor_faturamento.ToString().Replace(",", ".") + ", '" + data_proposta.Replace("'", "''") + "', '";
        sql += data_resposta.Replace("'", "''") + "', '" + data_inicio.Replace("'", "''") + "', '" + data_fim.Replace("'", "''") + "', '" + status_resposta + "', ";
        sql += cod_referencia + ", " + medico + ", " + equipe + ", " + vendedor + ", '";
        sql += (string.IsNullOrEmpty(paciente) ? "" : paciente.Replace("'", "''")) + "', '";
        sql += (string.IsNullOrEmpty(convenio) ? "" : convenio.Replace("'", "''")) + "')";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void update(int cod_job, int cod_cliente, int cod_linha_negocio, int cod_divisao, string nome, string descricao, string obs_nf, char status, decimal valor_faturamento,
        string data_proposta, string data_resposta, string data_inicio, string data_fim, char status_resposta, int cod_referencia, bool VemPorSincronizacao, int medico,
        int equipe, int vendedor, string paciente, string convenio)
    {
        AtualizaTabelaJobs();

        string sql = "UPDATE CAD_JOBS SET COD_CLIENTE = " + cod_cliente + ", ";

        if (VemPorSincronizacao == false)
              sql += "COD_LINHA_NEGOCIO = " + cod_linha_negocio + ", COD_DIVISAO = " + cod_divisao + ", ";

        sql += "NOME = '" + nome.Replace("'", "''") + "', DESCRICAO = '" + descricao.Replace("'", "''") + "', OBSERVACAO_NOTA_FISCAL = " + (string.IsNullOrEmpty(obs_nf) ? "NULL" : "'" + obs_nf.Replace("'", "''") + "'") + ", STATUS = '" + status + "', ";
        sql += "VALOR_FATURAMENTO = " + valor_faturamento.ToString().Replace(",", ".") + ", DATA_PROPOSTA = '" + data_proposta.Replace("'", "''") + "', ";
        sql += "DATA_RESPOSTA = '" + data_resposta.Replace("'", "''") + "', DATA_INICIO = '" + data_inicio.Replace("'", "''") + "', ";
        sql += "DATA_FIM = '" + data_fim.Replace("'", "''") + "', STATUS_RESPOSTA = '" + status_resposta + "', ";
        
        if (cod_referencia != 0)
            sql += "COD_REFERENCIA = " + cod_referencia + ", ";

        sql += "Cod_Medico = " + medico + ", Cod_Equipe = " + equipe + ", Cod_Vendedor = " + vendedor + ", ";
        sql += "Paciente = '" + paciente.Replace("'", "''") + "', Convenio = '" + convenio.Replace("'", "''") + "' ";
        sql += "WHERE COD_JOB = " + cod_job + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        _conn.execute(sql);
    }

    public void delete(int cod_job)
    {
        string sql = "DELETE FROM CAD_JOBS WHERE COD_JOB = " + cod_job + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        _conn.execute(sql);
    }

    public DataTable load(int cod_job)
    {
        AtualizaTabelaJobs();

        string sql = "SELECT CAD_JOBS.*, ";
        sql += "ISNULL(CAD_EMPRESAS.NOME_RAZAO_SOCIAL, '') AS DESCRICAO_CLIENTE, ";
        sql += "ISNULL(CAD_DIVISOES.DESCRICAO, '') AS DESCRICAO_DIVISAO, ";
        sql += "ISNULL(CAD_LINHA_NEGOCIOS.DESCRICAO, '') AS DESCRICAO_LINHA_NEGOCIOS, ";
        sql += "ISNULL(CE1.NOME_RAZAO_SOCIAL, '') AS DescMedico, ";
        sql += "ISNULL(CE2.NOME_RAZAO_SOCIAL, '') AS DescEquipe, ";
        sql += "ISNULL(CE3.NOME_RAZAO_SOCIAL, '') AS DescVendedor ";
        sql += "FROM CAD_JOBS ";
        sql += "LEFT JOIN CAD_DIVISOES ON CAD_JOBS.COD_DIVISAO = CAD_DIVISOES.COD_DIVISAO ";
        sql += "LEFT JOIN CAD_LINHA_NEGOCIOS ON CAD_JOBS.COD_LINHA_NEGOCIO = CAD_LINHA_NEGOCIOS.COD_LINHA_NEGOCIO ";
        sql += "LEFT JOIN CAD_EMPRESAS ON CAD_JOBS.COD_CLIENTE = CAD_EMPRESAS.COD_EMPRESA ";
        sql += "LEFT JOIN CAD_EMPRESAS CE1 ON CAD_JOBS.Cod_Medico = CE1.COD_EMPRESA  ";
        sql += "LEFT JOIN CAD_EMPRESAS CE2 ON CAD_JOBS.Cod_Equipe = CE2.COD_EMPRESA ";
        sql += "LEFT JOIN CAD_EMPRESAS CE3 ON CAD_JOBS.Cod_Vendedor = CE3.COD_EMPRESA ";
        sql += "WHERE CAD_JOBS.COD_JOB = " + cod_job + " AND CAD_JOBS.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        return _conn.dataTable(sql, "job");
    }

    public void lista(ref DataTable tb)
    {
        string sql = "SELECT *, ";
        sql += "(SELECT NOME_FANTASIA FROM CAD_EMPRESAS ";
        sql += "WHERE FISICA_JURIDICA = 'JURIDICA' AND CAD_EMPRESAS.COD_EMPRESA = CAD_JOBS.COD_CLIENTE AND CAD_EMPRESAS.COD_EMPRESA_PAI = CAD_JOBS.COD_EMPRESA)";
        sql += " + ' - ' + DESCRICAO AS DESCRICAO_COMPLETO ";
        sql += "FROM CAD_JOBS WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " ";
        sql += "ORDER BY (SELECT NOME_FANTASIA FROM CAD_EMPRESAS WHERE TIPO = 'CLIENTE' AND FISICA_JURIDICA = 'JURIDICA' ";
        sql += "AND CAD_EMPRESAS.COD_EMPRESA = CAD_JOBS.COD_CLIENTE AND CAD_EMPRESAS.COD_EMPRESA_PAI = CAD_JOBS.COD_EMPRESA) + ' - ' + DESCRICAO";

        _conn.fill(sql, ref tb);
    }

    public void listaSincronizacao(ref DataTable tb)
    {
        string sql = "SELECT *, ";
        sql += "(SELECT NOME_FANTASIA FROM CAD_EMPRESAS ";
        sql += "WHERE FISICA_JURIDICA = 'JURIDICA' AND CAD_EMPRESAS.COD_EMPRESA = CAD_JOBS.COD_CLIENTE AND CAD_EMPRESAS.COD_EMPRESA_PAI = CAD_JOBS.COD_EMPRESA)";
        sql += " + ' - ' + DESCRICAO AS DESCRICAO_COMPLETO ";
        sql += "FROM CAD_JOBS WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " ";
        sql += "ORDER BY COD_REFERENCIA";

        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, string colunaOrdenar)
    {
        string sql = "SELECT *, ";
        sql += "(SELECT NOME_FANTASIA FROM CAD_EMPRESAS ";
        sql += "WHERE FISICA_JURIDICA = 'JURIDICA' AND CAD_EMPRESAS.COD_EMPRESA = CAD_JOBS.COD_CLIENTE AND CAD_EMPRESAS.COD_EMPRESA_PAI = CAD_JOBS.COD_EMPRESA)";
        sql += " + ' - ' + DESCRICAO AS DESCRICAO_COMPLETO ";
        sql += "FROM CAD_JOBS WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " ";
        sql += "ORDER BY " + colunaOrdenar;

        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, char status)
    {
        string sql = "SELECT *, ";
        sql += "(SELECT NOME_FANTASIA FROM CAD_EMPRESAS ";
        sql += "WHERE (TIPO = 'CLIENTE' OR TIPO = 'CLIENTE_FORNECEDOR') AND CAD_EMPRESAS.COD_EMPRESA = CAD_JOBS.COD_CLIENTE AND CAD_EMPRESAS.COD_EMPRESA_PAI = CAD_JOBS.COD_EMPRESA)";
        sql += " + ' - ' + DESCRICAO AS DESCRICAO_COMPLETO ";
        sql += "FROM CAD_JOBS WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " ";

        if (status != null)
            sql += "AND STATUS = '" + status + "' ";

        sql += "ORDER BY (SELECT NOME_FANTASIA FROM CAD_EMPRESAS WHERE (TIPO = 'CLIENTE' OR TIPO = 'CLIENTE_FORNECEDOR') ";
        sql += "AND CAD_EMPRESAS.COD_EMPRESA = CAD_JOBS.COD_CLIENTE AND CAD_EMPRESAS.COD_EMPRESA_PAI = CAD_JOBS.COD_EMPRESA) + ' - ' + DESCRICAO";

        _conn.fill(sql, ref tb);
    }

    public void listaFiltroRelatorio(ref DataTable tb)
    {
        string sql = "SELECT *, ";
        sql += "(SELECT NOME_FANTASIA FROM CAD_EMPRESAS ";
        sql += "WHERE (TIPO = 'CLIENTE' OR TIPO = 'CLIENTE_FORNECEDOR') AND CAD_EMPRESAS.COD_EMPRESA = CAD_JOBS.COD_CLIENTE AND CAD_EMPRESAS.COD_EMPRESA_PAI = CAD_JOBS.COD_EMPRESA)";
        sql += " + ' - ' + DESCRICAO AS DESCRICAO_COMPLETO ";
        sql += "FROM CAD_JOBS WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " ";

        sql += "ORDER BY (SELECT NOME_FANTASIA FROM CAD_EMPRESAS WHERE (TIPO = 'CLIENTE' OR TIPO = 'CLIENTE_FORNECEDOR') ";
        sql += "AND CAD_EMPRESAS.COD_EMPRESA = CAD_JOBS.COD_CLIENTE AND CAD_EMPRESAS.COD_EMPRESA_PAI = CAD_JOBS.COD_EMPRESA) + ' - ' + DESCRICAO";

        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, char status, string colunaOrdenar)
    {
        string sql = "SELECT *, ";
        sql += "(SELECT NOME_FANTASIA FROM CAD_EMPRESAS ";
        sql += "WHERE (TIPO = 'CLIENTE' OR TIPO = 'CLIENTE_FORNECEDOR') AND FISICA_JURIDICA = 'JURIDICA' AND CAD_EMPRESAS.COD_EMPRESA = CAD_JOBS.COD_CLIENTE AND CAD_EMPRESAS.COD_EMPRESA_PAI = CAD_JOBS.COD_EMPRESA)";
        sql += " + ' - ' + DESCRICAO AS DESCRICAO_COMPLETO ";
        sql += "FROM CAD_JOBS WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " ";

        if (status != null)
            sql += "AND STATUS = '" + status + "' ";

        sql += "ORDER BY " + colunaOrdenar;

        _conn.fill(sql, ref tb);
    }

    public List<Hashtable> lista()
    {
        string sql = "SELECT CAD_JOBS.*, UPPER(NOME_FANTASIA) + ' - ' + DESCRICAO AS DESCRICAO_COMPLETO ";
        sql += "FROM CAD_JOBS, CAD_EMPRESAS ";
        sql += "WHERE CAD_JOBS.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " ";
        sql += "AND CAD_JOBS.COD_CLIENTE = CAD_EMPRESAS.COD_EMPRESA ";
        sql += "ORDER BY UPPER(NOME_FANTASIA) + ' - ' + DESCRICAO";

        return _conn.reader(sql);
    }

    public DataTable listaDataTable()
    {
        string sql = "SELECT TOP 10 *, ";
        sql += "(SELECT NOME_FANTASIA FROM CAD_EMPRESAS ";
        sql += "WHERE TIPO = 'CLIENTE' AND FISICA_JURIDICA = 'JURIDICA' AND CAD_EMPRESAS.COD_EMPRESA = CAD_JOBS.COD_CLIENTE AND CAD_EMPRESAS.COD_EMPRESA_PAI = CAD_JOBS.COD_EMPRESA)";
        sql += " + ' - ' + DESCRICAO AS DESCRICAO_COMPLETO ";
        sql += "FROM CAD_JOBS WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " ";
        sql += "AND COD_DIVISAO IN (SELECT COD_DIVISAO FROM CAD_DIVISOES WHERE COD_REFERENCIA IN (1, 2)) ";
        sql += "ORDER BY (SELECT NOME_FANTASIA FROM CAD_EMPRESAS WHERE TIPO = 'CLIENTE' AND FISICA_JURIDICA = 'JURIDICA' ";
        sql += "AND CAD_EMPRESAS.COD_EMPRESA = CAD_JOBS.COD_CLIENTE AND CAD_EMPRESAS.COD_EMPRESA_PAI = CAD_JOBS.COD_EMPRESA) + ' - ' + DESCRICAO";

        return _conn.dataTable(sql, "a");
    }

    public List<Hashtable> lista(char status)
    {
        string sql = "SELECT *, ";
        sql += "(SELECT NOME_FANTASIA FROM CAD_EMPRESAS ";
        sql += "WHERE FISICA_JURIDICA = 'JURIDICA' AND CAD_EMPRESAS.COD_EMPRESA = CAD_JOBS.COD_CLIENTE AND CAD_EMPRESAS.COD_EMPRESA_PAI = CAD_JOBS.COD_EMPRESA)";
        sql += " + ' - ' + DESCRICAO AS DESCRICAO_COMPLETO ";
        sql += "FROM CAD_JOBS WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " ";

        if (status != null)
            sql += "AND STATUS = '" + status + "' ";

        sql += "ORDER BY (SELECT NOME_FANTASIA FROM CAD_EMPRESAS WHERE FISICA_JURIDICA = 'JURIDICA' ";
        sql += "AND CAD_EMPRESAS.COD_EMPRESA = CAD_JOBS.COD_CLIENTE AND CAD_EMPRESAS.COD_EMPRESA_PAI = CAD_JOBS.COD_EMPRESA) + ' - ' + DESCRICAO";

        return _conn.reader(sql);
    }

    public int totalRegistros(int? cliente, string nome, string descricao)
    {
        string sql = "SELECT COUNT(COD_JOB) FROM CAD_JOBS WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        if (cliente != null)
            sql += " AND COD_CLIENTE = " + cliente.Value;

        if (nome != null)
            sql += " AND NOME LIKE '%" + nome.Replace("'", "''") + "%'";

        if (descricao != null)
            sql += " AND DESCRICAO LIKE '%" + descricao.Replace("'", "''") + "%'";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void lista(ref DataTable tb, int? cliente, string nome, string descricao, int paginaAtual, string ordenacao)
    {
        if (string.IsNullOrEmpty(ordenacao))
            ordenacao = "CJ.COD_JOB DESC";

        string sql = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY " + ordenacao + ") AS ROW, ";
        sql += "CJ.COD_JOB, CE.NOME_RAZAO_SOCIAL, CJ.NOME, CJ.DESCRICAO ";
        sql += "FROM CAD_JOBS CJ, CAD_EMPRESAS CE ";
        sql += "WHERE CJ.COD_CLIENTE = CE.COD_EMPRESA ";
        sql += "AND CJ.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        if (cliente != null)
            sql += " AND CJ.COD_CLIENTE = " + cliente.Value;

        if (nome != null)
            sql += " AND CJ.NOME LIKE '%" + nome.Replace("'", "''") + "%'";

        if (descricao != null)
            sql += " AND CJ.DESCRICAO LIKE '%" + descricao.Replace("'", "''") + "%'";

        sql += ") AS VW WHERE VW.ROW <= " + (((paginaAtual - 1) * 50) + 50) + " AND VW.ROW >= " + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }

    public void listaTimesheet(ref DataSet ds)
    {
        ds = ws.getCadastroJobs();
    }

    //Verifica se existe Jobs com a classificação diferente da Sextan ou Gerhir.
    public int sincronizaJobLinhaNegocio() 
    {
        string sql = "select count(*) from (select 1 as cod_empresa, " +
                     "(select max(cod_empresa) from GerenciamentoContabil.dbo.cad_empresas i where i.cod_empresa_timesheet = x.codcliente) as cod_cliente, " +
                     "(select max(COD_LINHA_NEGOCIO) from GerenciamentoContabil.dbo.cad_linha_negocios j where j.cod_referencia = x.codprojeto) as cod_linha_negocio, " +
                     "(select max(cod_divisao) from GerenciamentoContabil.dbo.cad_divisoes k where k.cod_referencia = x.coddivisao) as divisao, " +
                     "nomejob as nome, descjob as descricao, statusjob, valorfaturamento, dataproposta, dataresposta, datainicio, datafim, StatusResposta, codjob " +
                     "from mlegate.dbo.jobs x where x.codjob not in (select a.cod_referencia " +
                     "from GerenciamentoContabil.dbo.cad_jobs a) " +
                     "and x.codjob not in " +
                     "(select codjob from mlegate.dbo.jobs where coddivisao in " +
                     "(select a.cod_referencia from GerenciamentoContabil.dbo.cad_divisoes a where a.sincroniza <> 0)) " +
                     "and x.codjob in " +
                     "(select distinct codjob from mlegate.dbo.consultorjobs " +
                     "where codconsultor in " +
                     "(select codconsultor from mlegate.dbo.colaboradores where coddivisao in " +
                     "(select a.cod_referencia from GerenciamentoContabil.dbo.cad_divisoes a where a.sincroniza <> 0)))) x";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public DataTable sincronizaJobLinhaNegocioData()
    {
        string sql = "select 1 as cod_empresa, " +
                     "(SELECT COD_EMPRESA FROM GerenciamentoContabil.dbo.CAD_EMPRESAS WHERE COD_EMPRESA_TIMESHEET = x.CodCliente AND COD_EMPRESA_PAI = 1) AS COD_CLIENTE, " +
                     "(select max(COD_LINHA_NEGOCIO) from GerenciamentoContabil.dbo.cad_linha_negocios j where j.cod_referencia = x.codprojeto) as cod_linha_negocio, " +
                     "(select max(cod_divisao) from GerenciamentoContabil.dbo.cad_divisoes k where k.cod_referencia = x.coddivisao) as cod_divisao, " +
                     "nomejob as nome, descjob as descricao, statusjob, valorfaturamento, dataproposta, dataresposta, datainicio, datafim, StatusResposta, codjob " +
                     "from mlegate.dbo.jobs x where x.codjob not in (select a.cod_referencia " +
                     "from GerenciamentoContabil.dbo.cad_jobs a) " +
                     "and x.codjob not in " +
                     "(select codjob from mlegate.dbo.jobs where coddivisao in " +
                     "(select a.cod_referencia from GerenciamentoContabil.dbo.cad_divisoes a where a.sincroniza <> 0)) " +
                     "and x.codjob in " +
                     "(select distinct codjob from mlegate.dbo.consultorjobs " +
                     "where codconsultor in " +
                     "(select codconsultor from mlegate.dbo.colaboradores where coddivisao in " +
                     "(select a.cod_referencia from GerenciamentoContabil.dbo.cad_divisoes a where a.sincroniza <> 0)))";

        return _conn.dataTable(sql, "s");
    }

    public bool verificaDivisao(int cod_referencia) 
    {
        string sql = "SELECT SINCRONIZA FROM CAD_DIVISOES WHERE COD_REFERENCIA = " + cod_referencia + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        bool condicao1;
        try { condicao1 = Convert.ToBoolean(_conn.scalar(sql)); }
        catch { condicao1 = false; }

        sql = @"SELECT COUNT(MV.CodJob)
                FROM mlegate.dbo.Movimentacao MV, mlegate.dbo.Colaboradores CB, GerenciamentoContabil.dbo.CAD_DIVISOES CD
                WHERE MV.CodConsultor = CB.CodConsultor
                AND CB.CodDivisao = CD.COD_REFERENCIA and CD.SINCRONIZA = 1
                AND MV.CodJob = " + cod_referencia + @"
                AND CD.COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        bool condicao2;
        try { condicao2 = Convert.ToBoolean(_conn.scalar(sql)); }
        catch { condicao2 = false; }

        if (!condicao1 && !condicao2)
            return false;
        else
            return true;
    }

    public void lista_Jobs_Tomador(ref DataTable tb, int cod_tomador)
    {
        string sql = "SELECT CJ.COD_JOB, CJ.STATUS, CJ.OBSERVACAO_NOTA_FISCAL, ";
        sql += "(CASE WHEN CE.NOME_FANTASIA <> '' AND CJ.DESCRICAO <> '' THEN CE.NOME_FANTASIA + ' - ' + CJ.DESCRICAO ";
        sql += "WHEN CE.NOME_FANTASIA = '' THEN CJ.DESCRICAO ";
        sql += "WHEN CJ.DESCRICAO = '' THEN CE.NOME_FANTASIA END) AS NOME ";
        sql += "FROM CAD_JOBS CJ, CAD_EMPRESAS CE, mlegate.dbo.ClienteJobs JB ";
        sql += "WHERE CJ.COD_REFERENCIA = JB.CodJob ";
        sql += "AND CE.COD_EMPRESA_TIMESHEET = JB.CodCliente ";
        sql += "AND CJ.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " ";
        sql += "AND CE.COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"] + " ";
        sql += "AND CE.COD_EMPRESA = " + cod_tomador + " ";
        sql += "UNION ";
        sql += "SELECT CJ.COD_JOB, CJ.STATUS, CJ.OBSERVACAO_NOTA_FISCAL, ";
        sql += "(CASE WHEN CE.NOME_FANTASIA <> '' AND CJ.DESCRICAO <> '' THEN CE.NOME_FANTASIA + ' - ' + CJ.DESCRICAO ";
        sql += "WHEN CE.NOME_FANTASIA = '' THEN CJ.DESCRICAO ";
        sql += "WHEN CJ.DESCRICAO = '' THEN CE.NOME_FANTASIA END) AS NOME ";
        sql += "FROM CAD_JOBS CJ, CAD_EMPRESAS CE ";
        sql += "WHERE CJ.COD_CLIENTE = CE.COD_EMPRESA ";
        sql += "AND CJ.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " ";
        sql += "AND CE.COD_EMPRESA_PAI = " + HttpContext.Current.Session["empresa"] + " ";
        sql += "AND CJ.COD_CLIENTE = " + cod_tomador + " ";
        sql += "ORDER BY NOME";

        _conn.fill(sql, ref tb);
    }

    public DataTable Busca_Job(int cod_job)
    {
        string sql = "SELECT COD_CLIENTE, COD_LINHA_NEGOCIO, COD_DIVISAO, NOME, DESCRICAO FROM CAD_JOBS ";
        sql += "WHERE COD_JOB = " + cod_job + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        return _conn.dataTable(sql, "job");
    }
}