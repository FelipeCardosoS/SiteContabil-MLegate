using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormEmissaoNF : BaseForm
{
    private EmissaoNF_Funcoes emissao_nf_funcoes;
    private Emitente emitente;
    private Cliente cliente;
    private NaturezaOperacao natureza_operacao;
    private PrestacaoServico prestacao_servico;
    private Retencao retencao;
    private Servico servico;
    private Job job;
    private Narrativa narrativa;

    private DataTable tbEmitentes = new DataTable("tbEmitentes");
    private DataTable tbClientes = new DataTable("tbClientes");
    private DataTable tbNaturezaOperacao = new DataTable("tbNaturezaOperacao");
    private DataTable tbPrestacaoServicos = new DataTable("tbPrestacaoServicos");
    private DataTable tbRetencoes = new DataTable("tbRetencoes");
    private DataTable tbServicos = new DataTable("tbServicos");
    private DataTable tbJobs = new DataTable("tbJobs");
    private DataTable tbNarrativas = new DataTable("tbNarrativas");

    public FormEmissaoNF()
        : base("FATURAMENTO_EMISSAO_NF")
    {
        emissao_nf_funcoes = new EmissaoNF_Funcoes(_conn);
        emitente = new Emitente(_conn);
        cliente = new Cliente(_conn);
        natureza_operacao = new NaturezaOperacao(_conn);
        prestacao_servico = new PrestacaoServico(_conn);
        retencao = new Retencao(_conn);
        servico = new Servico(_conn);
        job = new Job(_conn);
        narrativa = new Narrativa(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        Title += "Emissão de Nota Fiscal";
        subTitulo.Text = "Emissão de Nota Fiscal";

        if (!Page.IsPostBack)
        {
            textObservacoes.Attributes.Add("maxlength", textObservacoes.MaxLength.ToString());

            //Emitente
            emitente.lista_Emitentes(ref tbEmitentes);
            ddlEmitente.DataSource = tbEmitentes;
            ddlEmitente.DataTextField = "NOME_RAZAO_SOCIAL";
            ddlEmitente.DataValueField = "COD_EMPRESA";
            ddlEmitente.DataBind();
            ddlEmitente.Items.Insert(0, new ListItem("Escolha", "0"));
        }
    }

    protected override void verificaTarefas()
    {
        bool aceitaGerar_NF = false;

        for (int i = 0; i < _tarefas.Count; i++)
        {
            if (_tarefas[i].tarefa == "GERAR_NF")
                aceitaGerar_NF = true;
        }

        if (!aceitaGerar_NF)
        {
            ddlEmitente.Enabled = false;
            btnGerarNF.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sistema", "alert('Você precisa de permissão para Emitir Nota Fiscal.');", true);
        }
    }

    protected virtual void repeaterDados_ItemDataBound(object sender, RepeaterItemEventArgs e) { }

    protected void ddlEmitente_Changed(object sender, EventArgs e)
    {
        int cod_emitente = Convert.ToInt32(ddlEmitente.SelectedValue);

        if (cod_emitente != 0)
        {
            //Tomador
            cliente.lista_Tomadores(ref tbClientes);
            ddlCliente.DataSource = tbClientes;
            ddlCliente.DataTextField = "NOME_RAZAO_SOCIAL";
            ddlCliente.DataValueField = "COD_EMPRESA";
            ddlCliente.DataBind();
            ddlCliente.Items.Insert(0, new ListItem("Escolha", "0"));

            //Data de Emissão
            string Data_RPS = emissao_nf_funcoes.Ultima_Data_RPS(cod_emitente).ToString("dd/MM/yyyy");
            string Data_Atual = DateTime.Now.ToString("dd/MM/yyyy");

            if (Convert.ToDateTime(Data_RPS) > Convert.ToDateTime(Data_Atual))
            {
                textDataEmissaoRPS.Text = Data_RPS;
                textData_RPS.Text = Data_RPS;
                textDataCompetencia.Text = Convert.ToDateTime(Data_RPS).ToString("MM/yyyy");
            }
            else
            {
                textDataEmissaoRPS.Text = Data_Atual;
                textDataCompetencia.Text = Convert.ToDateTime(Data_Atual).ToString("MM/yyyy");

                if (Data_RPS == "01/01/0001")
                    textData_RPS.Text = "";
                else
                    textData_RPS.Text = Data_RPS;
            }

            //Natureza da Operação
            natureza_operacao.lista_Natureza_Operacao(ref tbNaturezaOperacao, cod_emitente);
            comboNaturezaOperacao.DataSource = tbNaturezaOperacao;
            comboNaturezaOperacao.DataTextField = "NOME";
            comboNaturezaOperacao.DataValueField = "COD_NATUREZA_OPERACAO";
            comboNaturezaOperacao.DataBind();
            comboNaturezaOperacao.Items.Insert(0, new ListItem("Escolha", "0"));

            List<NaturezaOperacao> List_NaturezaOperacao = new List<NaturezaOperacao>();
            bool Padrao_Natureza_Operacao = true;
            textDescricao_NO.Text = string.Empty;
            textNaturezaOperacao.Text = string.Empty;

            foreach (DataRow row in tbNaturezaOperacao.Rows)
            {
                natureza_operacao = new NaturezaOperacao(_conn);
                natureza_operacao.cod_natureza_operacao = Convert.ToInt32(row["COD_NATUREZA_OPERACAO"]);
                natureza_operacao.descricao = Convert.ToString(row["DESCRICAO"]);
                natureza_operacao.natureza_operacao = Convert.ToString(row["NATUREZA_OPERACAO"]);

                if (Padrao_Natureza_Operacao == true && row["PADRAO"] != DBNull.Value && Convert.ToBoolean(row["PADRAO"]) == true)
                {
                    comboNaturezaOperacao.SelectedValue = Convert.ToString(row["COD_NATUREZA_OPERACAO"]);
                    textDescricao_NO.Text = Convert.ToString(row["DESCRICAO"]);
                    textNaturezaOperacao.Text = Convert.ToString(row["NATUREZA_OPERACAO"]);
                    Padrao_Natureza_Operacao = false;
                }
                List_NaturezaOperacao.Add(natureza_operacao);
            }
            HF_NaturezaOperacao.Value = new JavaScriptSerializer().Serialize(List_NaturezaOperacao);

            //Prestação de Serviço
            prestacao_servico.lista_Prestacao_Servicos(ref tbPrestacaoServicos, cod_emitente);
            comboPrestacaoServico.DataSource = tbPrestacaoServicos;
            comboPrestacaoServico.DataTextField = "NOME";
            comboPrestacaoServico.DataValueField = "COD_PRESTACAO_SERVICO";
            comboPrestacaoServico.DataBind();
            comboPrestacaoServico.Items.Insert(0, new ListItem("Escolha", "0"));

            List<PrestacaoServico> List_PrestacaoServico = new List<PrestacaoServico>();
            bool Padrao_Prestacao_Servico = true;
            textDescricao_PS.Text = string.Empty;

            foreach (DataRow row in tbPrestacaoServicos.Rows)
            {
                prestacao_servico = new PrestacaoServico(_conn);
                prestacao_servico.cod_prestacao_servico = Convert.ToInt32(row["COD_PRESTACAO_SERVICO"]);
                prestacao_servico.descricao = Convert.ToString(row["DESCRICAO"]);

                if (Padrao_Prestacao_Servico == true && row["PADRAO"] != DBNull.Value && Convert.ToBoolean(row["PADRAO"]) == true)
                {
                    comboPrestacaoServico.SelectedValue = Convert.ToString(row["COD_PRESTACAO_SERVICO"]);
                    textDescricao_PS.Text = Convert.ToString(row["DESCRICAO"]);
                    Padrao_Prestacao_Servico = false;
                }
                List_PrestacaoServico.Add(prestacao_servico);
            }
            HF_PrestacaoServico.Value = new JavaScriptSerializer().Serialize(List_PrestacaoServico);

            //Retenção
            retencao.lista_Retencoes(ref tbRetencoes, cod_emitente);
            repeaterDados.DataSource = tbRetencoes;
            repeaterDados.DataMember = tbRetencoes.TableName;
            repeaterDados.ItemDataBound += new RepeaterItemEventHandler(repeaterDados_ItemDataBound);
            repeaterDados.DataBind();

            //Serviço
            servico.lista_Servicos(ref tbServicos, cod_emitente);
            comboServico.DataSource = tbServicos;
            comboServico.DataTextField = "NOME";
            comboServico.DataValueField = "COD_SERVICO";
            comboServico.DataBind();
            comboServico.Items.Insert(0, new ListItem("Escolha", "0"));

            comboJob.Items.Insert(0, new ListItem("Escolha", "0"));

            //Narrativa
            narrativa.lista_Narrativas(ref tbNarrativas, cod_emitente);
            comboNarrativa.DataSource = tbNarrativas;
            comboNarrativa.DataTextField = "NOME";
            comboNarrativa.DataValueField = "COD_NARRATIVA";
            comboNarrativa.DataBind();
            comboNarrativa.Items.Insert(0, new ListItem("Escolha", "0"));

            List<Narrativa> List_Narrativa = new List<Narrativa>();
            bool Padrao_Narrativa = true;
            textDescricao_Narrativa.Value = string.Empty;

            foreach (DataRow row in tbNarrativas.Rows)
            {
                narrativa = new Narrativa(_conn);
                narrativa.cod_narrativa = Convert.ToInt32(row["COD_NARRATIVA"]);
                narrativa.descricao = Convert.ToString(row["DESCRICAO"]);

                if (Padrao_Narrativa == true && row["PADRAO"] != DBNull.Value && Convert.ToBoolean(row["PADRAO"]) == true)
                {
                    comboNarrativa.Value = Convert.ToString(row["COD_NARRATIVA"]);
                    textDescricao_Narrativa.Value = Convert.ToString(row["DESCRICAO"]);
                    Padrao_Narrativa = false;
                }
                List_Narrativa.Add(narrativa);
            }
            HF_Narrativa.Value = new JavaScriptSerializer().Serialize(List_Narrativa);
        }
        else
            Response.Redirect("FormEmissaoNF.aspx");
    }

    protected void ddlCliente_Changed(object sender, EventArgs e)
    {
        List<Job> List_Job = new List<Job>();
        int cod_tomador = Convert.ToInt32(ddlCliente.SelectedValue);

        if (cod_tomador != 0)
        {
            job.lista_Jobs_Tomador(ref tbJobs, cod_tomador);

            comboJob.DataSource = tbJobs;
            comboJob.DataTextField = "NOME";
            comboJob.DataValueField = "COD_JOB";
            comboJob.DataBind();
            comboJob.Items.Insert(0, new ListItem("Escolha", "0"));

            foreach (DataRow row in tbJobs.Rows)
            {
                job = new Job(_conn);
                job.codigo = Convert.ToInt32(row["COD_JOB"]);
                job.nome = Convert.ToString(row["NOME"]);
                job.status = Convert.ToChar(row["STATUS"]);
                job.obsNF = Convert.ToString(row["OBSERVACAO_NOTA_FISCAL"]);
                List_Job.Add(job);
            }
            HF_Job.Value = new JavaScriptSerializer().Serialize(List_Job);
        }
        else
            HF_Job.Value = new JavaScriptSerializer().Serialize(List_Job);
    }


    //BTN GERAR NF LEGADO 
    //[WebMethod]
    //public static string btnGerarNF_Click(EmissaoNF emissao_nf)
    //{
    //    Conexao _conn = new Conexao();
    //    EmissaoNF_Funcoes _EmissaoNF_Funcoes = new EmissaoNF_Funcoes(_conn);

    //    string Mensagem_Sucesso_Erro = _EmissaoNF_Funcoes.Validar(emissao_nf.cod_emitente, emissao_nf.cod_tomador, 
    //        emissao_nf.data_emissao_rps, emissao_nf.data_rps, emissao_nf.data_competencia, 
    //        emissao_nf.cod_natureza_operacao, emissao_nf.cod_prestacao_servico, emissao_nf.valor_total_liquido, emissao_nf.valor_total_vencimento, 
    //        emissao_nf.List_Servico_Job, emissao_nf.List_Vencimento, emissao_nf.List_Narrativa);

    //    if (string.IsNullOrEmpty(Mensagem_Sucesso_Erro)) //Se For Sucesso
    //    {
    //        _EmissaoNF_Funcoes.Salvar(emissao_nf.cod_emitente, emissao_nf.cod_tomador, emissao_nf.data_emissao_rps, emissao_nf.data_competencia, 
    //        emissao_nf.cod_natureza_operacao, emissao_nf.nome_natureza_operacao, emissao_nf.descricao_natureza_operacao, emissao_nf.natureza_operacao, 
    //        emissao_nf.cod_prestacao_servico, emissao_nf.nome_prestacao_servico, emissao_nf.descricao_prestacao_servico, emissao_nf.valor_total_servico_job, 
    //        emissao_nf.observacoes, emissao_nf.List_Retencao, emissao_nf.List_Servico_Job, emissao_nf.List_Vencimento, emissao_nf.List_Narrativa);

    //        Mensagem_Sucesso_Erro = "/FormConfirmacaoContabilizacao.aspx?id=" + _EmissaoNF_Funcoes.cod_faturamento_nf;
    //    }
    //    return Mensagem_Sucesso_Erro;
    //}


    // BOTAO GERAR NF (ATUALIZADO FASE 3 - REFORMA TRIBUTÁRIA)
    [WebMethod]
    public static string btnGerarNF_Click(EmissaoNF emissao_nf)
    {
        Conexao _conn = new Conexao();
        EmissaoNF_Funcoes _EmissaoNF_Funcoes = new EmissaoNF_Funcoes(_conn);

        // -------------------------------------------------------------
        // 1. 🛡️ CÃO DE GUARDA (VALIDAÇÃO DE CADASTRO)
        // -------------------------------------------------------------
        string ErroCadastro = ValidarCadastroTomador(emissao_nf.cod_tomador);
        if (!string.IsNullOrEmpty(ErroCadastro))
        {
            return "BLOQUEIO DE EMISSÃO: " + ErroCadastro;
        }

        // -------------------------------------------------------------
        // 2. 🧮 CÁLCULO DA REFORMA TRIBUTÁRIA (IBS / CBS)
        // -------------------------------------------------------------
        // Tenta converter o valor do serviço (que vem como string) para decimal
        decimal vServico = 0;
        try
        {
            // Remove R$ e espaços, troca ponto por nada e vírgula por ponto (ou conforme cultura)
            // Assumindo que o sistema usa formatação brasileira (1.000,00)
            string valorLimpo = emissao_nf.valor_total_servico_job.Replace("R$", "").Trim();
            vServico = Convert.ToDecimal(valorLimpo, new System.Globalization.CultureInfo("pt-BR"));
        }
        catch { vServico = 0; }

        // Alíquotas fixas para a fase de teste (2026)
        decimal aliqIBS = 0.1m; // 0,1%
        decimal aliqCBS = 0.9m; // 0,9%

        // Preenche o objeto com o cálculo
        emissao_nf.valor_ibs = Math.Round(vServico * (aliqIBS / 100m), 2);
        emissao_nf.aliquota_ibs = aliqIBS;
        emissao_nf.valor_cbs = Math.Round(vServico * (aliqCBS / 100m), 2);
        emissao_nf.aliquota_cbs = aliqCBS;
        // -------------------------------------------------------------


        // 3. VALIDAÇÃO PADRÃO (LEGADO)
        string Mensagem_Sucesso_Erro = _EmissaoNF_Funcoes.Validar(emissao_nf.cod_emitente, emissao_nf.cod_tomador,
            emissao_nf.data_emissao_rps, emissao_nf.data_rps, emissao_nf.data_competencia,
            emissao_nf.cod_natureza_operacao, emissao_nf.cod_prestacao_servico, emissao_nf.valor_total_liquido, emissao_nf.valor_total_vencimento,
            emissao_nf.List_Servico_Job, emissao_nf.List_Vencimento, emissao_nf.List_Narrativa);

        if (string.IsNullOrEmpty(Mensagem_Sucesso_Erro))
        {
            // 4. SALVAR (COM OS NOVOS PARÂMETROS)
            // Note que adicionamos os 4 campos de impostos no meio da chamada
            _EmissaoNF_Funcoes.Salvar(
                emissao_nf.cod_emitente,
                emissao_nf.cod_tomador,
                emissao_nf.data_emissao_rps,
                emissao_nf.data_competencia,
                emissao_nf.cod_natureza_operacao,
                emissao_nf.nome_natureza_operacao,
                emissao_nf.descricao_natureza_operacao,
                emissao_nf.natureza_operacao,
                emissao_nf.cod_prestacao_servico,
                emissao_nf.nome_prestacao_servico,
                emissao_nf.descricao_prestacao_servico,
                emissao_nf.valor_total_servico_job,
                emissao_nf.observacoes,

                // --- NOVOS PARÂMETROS AQUI ---
                emissao_nf.valor_ibs,
                emissao_nf.aliquota_ibs,
                emissao_nf.valor_cbs,
                emissao_nf.aliquota_cbs,
                // -----------------------------

                emissao_nf.List_Retencao,
                emissao_nf.List_Servico_Job,
                emissao_nf.List_Vencimento,
                emissao_nf.List_Narrativa
            );

            Mensagem_Sucesso_Erro = "/FormConfirmacaoContabilizacao.aspx?id=" + _EmissaoNF_Funcoes.cod_faturamento_nf;
        }
        return Mensagem_Sucesso_Erro;
    }



    //METODO NOVO DE VALIDAÇÃO DE CADASTRO DO TOMADOR
    public static string ValidarCadastroTomador(int codEmpresaTomador)
    {
        // SQL ajustado para os nomes que você confirmou: CAD_EMPRESAS, CEP, MUNICIPIO, UF
        string sql = @"
            SELECT 
                ISNULL(CEP, '') as CEP, 
                ISNULL(MUNICIPIO, '') as MUNICIPIO, 
                ISNULL(UF, '') as UF 
            FROM 
                CAD_EMPRESAS WITH(NOLOCK)
            WHERE 
                COD_EMPRESA = @CodEmpresa";

        try
        {
            // Pega a string de conexão do Web.config
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["strConexao"].ConnectionString;

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connStr))
            {
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@CodEmpresa", codEmpresaTomador);
                    connection.Open();

                    using (System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Remove espaços em branco e traços para validar
                            string cep = reader["CEP"].ToString().Replace("-", "").Replace(".", "").Trim();
                            string cidade = reader["MUNICIPIO"].ToString().Trim();
                            string uf = reader["UF"].ToString().Trim();

                            List<string> falhas = new List<string>();

                            // --- REGRAS DA REFORMA TRIBUTÁRIA ---

                            // 1. CEP Obrigatório (8 dígitos)
                            if (string.IsNullOrEmpty(cep) || cep.Length < 8)
                                falhas.Add("CEP está vazio ou inválido (Obrigatório 8 dígitos)");

                            // 2. Cidade Obrigatória
                            if (string.IsNullOrEmpty(cidade))
                                falhas.Add("Cidade não informada");

                            // 3. UF Obrigatória (2 letras)
                            if (string.IsNullOrEmpty(uf) || uf.Length != 2)
                                falhas.Add("UF inválida");

                            // Se achou erro, avisa
                            if (falhas.Count > 0)
                            {
                                return "O cadastro do cliente selecionado está incompleto para a Reforma Tributária: " +
                                       string.Join(", ", falhas) +
                                       ". Atualize o cadastro na tela de Empresas antes de emitir.";
                            }
                        }
                        else
                        {
                            return "Cliente não encontrado no cadastro (CAD_EMPRESAS).";
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return "Erro técnico ao validar cadastro: " + ex.Message;
        }

        return ""; // String vazia = Sucesso/Aprovado
    }
}