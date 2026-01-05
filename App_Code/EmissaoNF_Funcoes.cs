using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public class EmissaoNF_Funcoes
{
    public int cod_faturamento_nf;
    public string emitente;
    public int numero_rps;

    private emissao_nf_DAO emissao_nf_DAO;
    private empresasDAO empresaDAO;
    private servicosDAO servicoDAO;
    private jobsDAO jobDAO;
    private tributosDAO tributosDAO;

    public EmissaoNF_Funcoes(Conexao _conn)
    {
        emissao_nf_DAO = new emissao_nf_DAO(_conn);
        empresaDAO = new empresasDAO(_conn);
        servicoDAO = new servicosDAO(_conn);
        jobDAO = new jobsDAO(_conn);
        tributosDAO = new tributosDAO(_conn);
    }

    public DateTime Ultima_Data_RPS(int cod_emitente)
    {
        return emissao_nf_DAO.Ultima_Data_RPS(cod_emitente);
    }

    public string Validar(int cod_emitente, int cod_tomador, DateTime data_emissao_rps, DateTime data_rps, DateTime data_competencia,
        int cod_natureza_operacao, int cod_prestacao_servico, string valor_total_liquido, string valor_total_vencimento,
        List<EmissaoNF_Servico_Job> List_Servico_Job, List<EmissaoNF_Vencimento> List_Vencimento, List<EmissaoNF_Narrativa> List_Narrativa)
    {
        List<string> Mensagem_Sucesso_Erro = new List<string>();
        string cod_empresa;
        string Mensagem_Erro;
        int Contador = 0;
        int Diferente = 0;
        int Repetido = 0;
        int List_Count = 0;
        bool Vencimento_OK = true;

        cod_empresa = Convert.ToString(HttpContext.Current.Session["empresa"]); //Empresa Logada
        if (string.IsNullOrEmpty(cod_empresa) || cod_empresa == null || cod_empresa == string.Empty || cod_empresa == "0")
            Mensagem_Sucesso_Erro.Add("A sessão expirou. Faça login novamente.");

        if (cod_tomador == 0)
            Mensagem_Sucesso_Erro.Add("Selecione um Tomador.");

        Mensagem_Erro = Verifica_Data(data_emissao_rps.ToString("dd/MM/yyyy"), "Data de Emissão");

        if (data_emissao_rps == Convert.ToDateTime("01/01/0001"))
            Mensagem_Sucesso_Erro.Add("Informe a Data de Emissão da Nota Fiscal.");
        else if (!string.IsNullOrEmpty(Mensagem_Erro)) //Se a Data de Emissão estiver no formato errado, será diferente de vazio.
            Mensagem_Sucesso_Erro.Add(Mensagem_Erro); //Mensagem de erro atribuída pelo método "Verifica_Data".
        else if (data_emissao_rps < data_rps)
            Mensagem_Sucesso_Erro.Add("Informe uma Data de Emissão igual ou posterior a Última Nota Fiscal Emitida.");

        if (data_competencia != Convert.ToDateTime("01/01/0001"))
        {
            Mensagem_Erro = Verifica_Data(data_competencia.ToString("dd/MM/yyyy"), "Data de Competência");

            if (!string.IsNullOrEmpty(Mensagem_Erro)) //Se a Data de Competência estiver no formato errado, será diferente de vazio.
                Mensagem_Sucesso_Erro.Add(Mensagem_Erro); //Mensagem de erro atribuída pelo método "Verifica_Data".
        }

        if (cod_natureza_operacao == 0)
            Mensagem_Sucesso_Erro.Add("Selecione uma Natureza da Operação.");

        if (cod_prestacao_servico == 0)
            Mensagem_Sucesso_Erro.Add("Selecione uma Prestação de Serviço.");

        //Serviço e Job
        foreach (EmissaoNF_Servico_Job item in List_Servico_Job)
        {
            if (item.cod_servico == 0)
            {
                Mensagem_Sucesso_Erro.Add("Selecione um Serviço.");
                Contador += 1;
            }

            if (item.cod_job == 0)
            {
                Mensagem_Sucesso_Erro.Add("Selecione um Job.");
                Contador += 1;
            }

            try
            {
                if (item.valor_servico_job == string.Empty || item.valor_servico_job == "," || Math.Round(Convert.ToDecimal(item.valor_servico_job), 2, MidpointRounding.AwayFromZero) == 0)
                {
                    Mensagem_Sucesso_Erro.Add("Informe o Valor do Serviço.");
                    Contador += 1;
                }
            }
            catch
            {
                Mensagem_Sucesso_Erro.Add("Valor do Serviço Inválido.");
                Contador += 1;
            }

            if (Contador > 0)
                break;

            foreach (EmissaoNF_Servico_Job item2 in List_Servico_Job)
            {
                if (item.cod_servico != item2.cod_servico)
                    Diferente += 1;

                if (item.cod_job == item2.cod_job)
                    Repetido += 1;
            }

            List_Count += 1;

            if (List_Count == List_Servico_Job.Count && Diferente != 0)
                Mensagem_Sucesso_Erro.Add("Informe apenas um tipo de Serviço.");

            if (List_Count == List_Servico_Job.Count && Repetido != List_Servico_Job.Count)
                Mensagem_Sucesso_Erro.Add("Informe Jobs diferentes.");
        }

        //Vencimento
        Contador = 0;
        Repetido = 0;
        List_Count = 0;

        foreach (EmissaoNF_Vencimento item in List_Vencimento)
        {
            if (item.data_vencimento == Convert.ToDateTime("01/01/0001") && item.valor_vencimento == string.Empty)
            {
                Mensagem_Sucesso_Erro.Add("Informe a Data e o Valor do Vencimento.");
                Contador += 1;
                Vencimento_OK = false;
            }

            if (item.data_vencimento == Convert.ToDateTime("01/01/0001") && item.valor_vencimento != "")
            {
                Mensagem_Sucesso_Erro.Add("Informe a Data de Vencimento.");
                Contador += 1;
                Vencimento_OK = false;
            }

            if (item.data_vencimento != Convert.ToDateTime("01/01/0001") && item.valor_vencimento == string.Empty)
            {
                Mensagem_Sucesso_Erro.Add("Informe o Valor do Vencimento.");
                Contador += 1;
                Vencimento_OK = false;
            }

            Mensagem_Erro = Verifica_Data(item.data_vencimento.ToString("dd/MM/yyyy"), "Data de Vencimento");

            if (!string.IsNullOrEmpty(Mensagem_Erro)) //Se a Data de Vencimento estiver no formato errado, será diferente de vazio.
            {
                Mensagem_Sucesso_Erro.Add(Mensagem_Erro); //Mensagem de erro atribuída pelo método "Verifica_Data".
                Contador += 1;
                Vencimento_OK = false;
            }

            if (item.data_vencimento != Convert.ToDateTime("01/01/0001") && item.data_vencimento < data_emissao_rps)
            {
                Mensagem_Sucesso_Erro.Add("Informe uma Data de Vencimento igual ou posterior a Data de Emissão.");
                Contador += 1;
                Vencimento_OK = false;
            }

            if (Contador > 0)
                break;

            if (item.data_vencimento != Convert.ToDateTime("01/01/0001") && item.valor_vencimento != "")
            {
                foreach (EmissaoNF_Vencimento item2 in List_Vencimento)
                {
                    if (item.data_vencimento == item2.data_vencimento)
                        Repetido += 1;
                }
            }
            else
                Repetido += 1;

            List_Count += 1;
            if (List_Count == List_Vencimento.Count && Repetido != List_Vencimento.Count)
            {
                Mensagem_Sucesso_Erro.Add("Informe Datas de Vencimento diferentes.");
                Vencimento_OK = false;
            }
        }

        if (Vencimento_OK == true)
        {
            if (valor_total_liquido != valor_total_vencimento)
                Mensagem_Sucesso_Erro.Add("O Valor Total dos Vencimentos deve ser igual ao Total Líquido.");
        }

        //Narrativa
        Repetido = 0;
        List_Count = 0;

        foreach (EmissaoNF_Narrativa item in List_Narrativa)
        {
            if (item.cod_narrativa == 0)
            {
                Mensagem_Sucesso_Erro.Add("Selecione uma Narrativa.");
                break;
            }

            foreach (EmissaoNF_Narrativa item2 in List_Narrativa)
            {
                if (item.cod_narrativa == item2.cod_narrativa)
                    Repetido += 1;
            }

            List_Count += 1;
            if (List_Count == List_Narrativa.Count && Repetido != List_Narrativa.Count)
                Mensagem_Sucesso_Erro.Add("Informe Narrativas diferentes.");
        }

        if (Mensagem_Sucesso_Erro.Count == 0)
        {
            string Data_RPS = Ultima_Data_RPS(cod_emitente).ToString("dd/MM/yyyy");

            if (data_emissao_rps < Convert.ToDateTime(Data_RPS))
                Mensagem_Sucesso_Erro.Add("Provavelmente você voltou a página, impossibilitando a atualização da Última Nota Fiscal Emitida.\n\nA página será recarregada.");
        }

        if (Mensagem_Sucesso_Erro.Count == 0)
            return string.Empty;
        else
            return string.Join("\n", Mensagem_Sucesso_Erro);
    }

    public string Verifica_Data(string ddMMyyyy, string Campo)
    {
        if (ddMMyyyy != "" && ddMMyyyy != "01/01/0001")
        {
            string[] Data = ddMMyyyy.Split('/');

            if (Convert.ToInt32(Data[0]) < 1)
                return Campo + ": Informe um Dia Válido.";

            if (Convert.ToInt32(Data[1]) < 1)
                return Campo + ": Informe um Mês Válido.";

            if (Convert.ToInt32(Data[2]) < 2000)
                return Campo + ": Informe um Ano Válido.";

            switch (Data[1])
            {
                case "01":
                    if (Convert.ToInt32(Data[0]) <= 31)
                        return string.Empty;
                    else
                        return Campo + ": Informe um Dia Válido. Janeiro possui 31 dias.";
                case "02":
                    if (!DateTime.IsLeapYear(Convert.ToInt32(Data[2]))) //Verifica se é Ano Bissexto.
                    {
                        if (Convert.ToInt32(Data[0]) <= 28)
                            return string.Empty;
                        else
                            return Campo + ": Informe um Dia Válido. Fevereiro de " + Data[2] + " possui 28 dias.";
                    }
                    else //É Ano Bissexto.
                    {
                        if (Convert.ToInt32(Data[0]) <= 29)
                            return string.Empty;
                        else
                            return Campo + ": Informe um Dia Válido. Fevereiro de " + Data[2] + " possui 29 dias.";
                    }
                case "03":
                    if (Convert.ToInt32(Data[0]) <= 31)
                        return string.Empty;
                    else
                        return Campo + ": Informe um Dia Válido. Março possui 31 dias.";
                case "04":
                    if (Convert.ToInt32(Data[0]) <= 30)
                        return string.Empty;
                    else
                        return Campo + ": Informe um Dia Válido. Abril possui 30 dias.";
                case "05":
                    if (Convert.ToInt32(Data[0]) <= 31)
                        return string.Empty;
                    else
                        return Campo + ": Informe um Dia Válido. Maio possui 31 dias.";
                case "06":
                    if (Convert.ToInt32(Data[0]) <= 30)
                        return string.Empty;
                    else
                        return Campo + ": Informe um Dia Válido. Junho possui 30 dias.";
                case "07":
                    if (Convert.ToInt32(Data[0]) <= 31)
                        return string.Empty;
                    else
                        return Campo + ": Informe um Dia Válido. Julho possui 31 dias.";
                case "08":
                    if (Convert.ToInt32(Data[0]) <= 31)
                        return string.Empty;
                    else
                        return Campo + ": Informe um Dia Válido. Agosto possui 31 dias.";
                case "09":
                    if (Convert.ToInt32(Data[0]) <= 30)
                        return string.Empty;
                    else
                        return Campo + ": Informe um Dia Válido. Setembro possui 30 dias.";
                case "10":
                    if (Convert.ToInt32(Data[0]) <= 31)
                        return string.Empty;
                    else
                        return Campo + ": Informe um Dia Válido. Outubro possui 31 dias.";
                case "11":
                    if (Convert.ToInt32(Data[0]) <= 30)
                        return string.Empty;
                    else
                        return Campo + ": Informe um Dia Válido. Novembro possui 30 dias.";
                case "12":
                    if (Convert.ToInt32(Data[0]) <= 31)
                        return string.Empty;
                    else
                        return Campo + ": Informe um Dia Válido. Dezembro possui 31 dias.";
                default:
                    return Campo + ": Informe um Mês entre 01 e 12 (Janeiro a Dezembro).";
            }
        }
        else
            return "Informe a " + Campo + ".";
    }

    // --- ALTERAÇÃO: ADICIONADOS 4 PARÂMETROS NOVOS ---
    public void Salvar(int cod_emitente, int cod_tomador, DateTime data_emissao_rps, DateTime data_competencia,
        int cod_natureza_operacao, string nome_natureza_operacao, string descricao_natureza_operacao, string natureza_operacao,
        int cod_prestacao_servico, string nome_prestacao_servico, string descricao_prestacao_servico, string valor_total_servico_job, string observacoes,
        decimal valor_ibs, decimal aliquota_ibs, decimal valor_cbs, decimal aliquota_cbs, // <--- NOVOS PARAMS
        List<EmissaoNF_Retencao> List_Retencao, List<EmissaoNF_Servico_Job> List_Servico_Job, List<EmissaoNF_Vencimento> List_Vencimento, List<EmissaoNF_Narrativa> List_Narrativa)
    {
        //Gera um número de Nota Fiscal para o emitente selecionado
        int numero_rps = emissao_nf_DAO.Nova_NF_Emitente(cod_emitente);

        //Série da Nota Fiscal
        string serie_rps = "1";

        //Emitente
        DataTable Emitente = empresaDAO.Busca_Empresa_Emitente_Tomador(cod_emitente);
        string nome_razao_social_emitente = string.Empty;
        string indicador_cpf_cnpj_emitente = string.Empty;
        string cpf_cnpj_emitente = string.Empty;
        string im_emitente = string.Empty;
        string ie_emitente = string.Empty;
        string endereco_emitente = string.Empty;
        string num_endereco_emitente = string.Empty;
        string compl_endereco_emitente = string.Empty;
        string bairro_emitente = string.Empty;
        string municipio_emitente = string.Empty;
        string uf_emitente = string.Empty;
        string cep_emitente = string.Empty;

        if (Emitente.Rows.Count > 0)
        {
            nome_razao_social_emitente = Emitente.Rows[0]["NOME_RAZAO_SOCIAL"].ToString();

            if (Emitente.Rows[0]["FISICA_JURIDICA"].ToString() == "JURIDICA")
                indicador_cpf_cnpj_emitente = "2";
            else
                indicador_cpf_cnpj_emitente = "1";

            cpf_cnpj_emitente = Emitente.Rows[0]["CNPJ_CPF"].ToString();
            im_emitente = Emitente.Rows[0]["IM"].ToString();
            ie_emitente = Emitente.Rows[0]["IE_RG"].ToString();
            endereco_emitente = Emitente.Rows[0]["ENDERECO"].ToString();
            num_endereco_emitente = Emitente.Rows[0]["NUMERO"].ToString();
            compl_endereco_emitente = Emitente.Rows[0]["COMPLEMENTO"].ToString();
            bairro_emitente = Emitente.Rows[0]["BAIRRO"].ToString();
            municipio_emitente = Emitente.Rows[0]["MUNICIPIO"].ToString();
            uf_emitente = Emitente.Rows[0]["UF"].ToString();
            cep_emitente = Emitente.Rows[0]["CEP"].ToString();
        }

        //Tomador
        DataTable Tomador = empresaDAO.Busca_Empresa_Emitente_Tomador(cod_tomador);
        string nome_razao_social_tomador = string.Empty;
        string indicador_cpf_cnpj_tomador = string.Empty;
        string cpf_cnpj_tomador = string.Empty;
        string im_tomador = string.Empty;
        string ie_tomador = string.Empty;
        string endereco_tomador = string.Empty;
        string num_endereco_tomador = string.Empty;
        string compl_endereco_tomador = string.Empty;
        string bairro_tomador = string.Empty;
        string municipio_tomador = string.Empty;
        string uf_tomador = string.Empty;
        string cep_tomador = string.Empty;

        if (Tomador.Rows.Count > 0)
        {
            nome_razao_social_tomador = Tomador.Rows[0]["NOME_RAZAO_SOCIAL"].ToString();

            if (Tomador.Rows[0]["FISICA_JURIDICA"].ToString() == "JURIDICA")
                indicador_cpf_cnpj_tomador = "2";
            else
                indicador_cpf_cnpj_tomador = "1";

            cpf_cnpj_tomador = Tomador.Rows[0]["CNPJ_CPF"].ToString();
            im_tomador = Tomador.Rows[0]["IM"].ToString();
            ie_tomador = Tomador.Rows[0]["IE_RG"].ToString();
            endereco_tomador = Tomador.Rows[0]["ENDERECO"].ToString();
            num_endereco_tomador = Tomador.Rows[0]["NUMERO"].ToString();
            compl_endereco_tomador = Tomador.Rows[0]["COMPLEMENTO"].ToString();
            bairro_tomador = Tomador.Rows[0]["BAIRRO"].ToString();
            municipio_tomador = Tomador.Rows[0]["MUNICIPIO"].ToString();
            uf_tomador = Tomador.Rows[0]["UF"].ToString();
            cep_tomador = Tomador.Rows[0]["CEP"].ToString();
        }

        //Insere uma nova Nota Fiscal
        // --- ALTERAÇÃO: Passando os 4 parâmetros novos para o DAO ---
        cod_faturamento_nf = emissao_nf_DAO.Nova_NF_Faturamento(numero_rps, data_emissao_rps, data_competencia, serie_rps,
            cod_emitente, nome_razao_social_emitente, indicador_cpf_cnpj_emitente, cpf_cnpj_emitente, im_emitente, ie_emitente,
            endereco_emitente, num_endereco_emitente, compl_endereco_emitente, bairro_emitente, municipio_emitente, uf_emitente, cep_emitente,
            cod_tomador, nome_razao_social_tomador, indicador_cpf_cnpj_tomador, cpf_cnpj_tomador, im_tomador, ie_tomador,
            endereco_tomador, num_endereco_tomador, compl_endereco_tomador, bairro_tomador, municipio_tomador, uf_tomador, cep_tomador,
            cod_natureza_operacao, nome_natureza_operacao, descricao_natureza_operacao, natureza_operacao,
            cod_prestacao_servico, nome_prestacao_servico, descricao_prestacao_servico, valor_total_servico_job, observacoes,
            valor_ibs, aliquota_ibs, valor_cbs, aliquota_cbs); // <--- AQUI

        //Insere uma nova Nota Fiscal - Retenção
        foreach (EmissaoNF_Retencao item in List_Retencao)
        {
            item.valor_retencao = Math.Round((Convert.ToDecimal(valor_total_servico_job) * Convert.ToDecimal(item.aliquota_retencao)) / 100, 2, MidpointRounding.AwayFromZero);

            emissao_nf_DAO.Nova_NF_Faturamento_Retencao(cod_faturamento_nf, item.cod_retencao, item.nome_retencao, item.aliquota_retencao, item.apresentacao_retencao, item.valor_retencao);
        }

        //Insere uma nova Nota Fiscal - Serviço e Job
        string COD_SERVICO = string.Empty; //Prepara a variável para utilizar no SELECT dos Tributos.
        string nome_servico = string.Empty;
        string cod_servico_prefeitura = string.Empty;
        decimal impostos_servico = 0;
        decimal carga_impostos_servico = 0;
        int cod_cliente = 0;
        int cod_linha_negocio = 0;
        int cod_divisao = 0;

        foreach (EmissaoNF_Servico_Job item in List_Servico_Job)
        {
            if (!string.IsNullOrEmpty(COD_SERVICO))
                COD_SERVICO += ", ";

            COD_SERVICO += item.cod_servico;

            DataTable Servico = servicoDAO.load(item.cod_servico);
            if (Servico.Rows.Count > 0)
            {
                nome_servico = Servico.Rows[0]["NOME"].ToString();
                cod_servico_prefeitura = Servico.Rows[0]["COD_SERVICO_PREFEITURA"].ToString();
                impostos_servico = Convert.ToDecimal(Servico.Rows[0]["IMPOSTOS"]);
                carga_impostos_servico = Math.Round((Convert.ToDecimal(item.valor_servico_job) * impostos_servico) / 100, 2, MidpointRounding.AwayFromZero);
            }

            DataTable Job = jobDAO.Busca_Job(item.cod_job);
            if (Job.Rows.Count > 0)
            {
                if (Job.Rows[0]["COD_CLIENTE"] != DBNull.Value)
                    cod_cliente = Convert.ToInt32(Job.Rows[0]["COD_CLIENTE"]);

                if (Job.Rows[0]["COD_LINHA_NEGOCIO"] != DBNull.Value)
                    cod_linha_negocio = Convert.ToInt32(Job.Rows[0]["COD_LINHA_NEGOCIO"]);

                if (Job.Rows[0]["COD_DIVISAO"] != DBNull.Value)
                    cod_divisao = Convert.ToInt32(Job.Rows[0]["COD_DIVISAO"]);
            }

            emissao_nf_DAO.Nova_NF_Faturamento_Servico_Job(cod_faturamento_nf, item.cod_servico, nome_servico, cod_servico_prefeitura,
                item.cod_job, cod_cliente, cod_linha_negocio, cod_divisao, item.valor_servico_job, impostos_servico, carga_impostos_servico);
        }

        //Insere uma nova Nota Fiscal - Vencimento
        foreach (EmissaoNF_Vencimento item in List_Vencimento)
            if (item.data_vencimento != Convert.ToDateTime("01/01/0001") && item.valor_vencimento != "")
                emissao_nf_DAO.Nova_NF_Faturamento_Vencimento(cod_faturamento_nf, item.data_vencimento, item.valor_vencimento);

        //Insere uma nova Nota Fiscal - Narrativa
        foreach (EmissaoNF_Narrativa item in List_Narrativa)
            emissao_nf_DAO.Nova_NF_Faturamento_Narrativa(cod_faturamento_nf, item.cod_narrativa, item.nome_narrativa, item.descricao_narrativa);

        //Insere uma nova Nota Fiscal - Tributo
        bool Nao_Existe;

        //Agrupa Serviços
        EmissaoNF_Servico_Job servico = new EmissaoNF_Servico_Job();
        List<EmissaoNF_Servico_Job> New_List_Servico = new List<EmissaoNF_Servico_Job>();

        foreach (EmissaoNF_Servico_Job item_servico_job in List_Servico_Job)
        {
            Nao_Existe = true;

            foreach (EmissaoNF_Servico_Job item_servico in New_List_Servico)
            {
                if (item_servico_job.cod_servico == item_servico.cod_servico)
                {
                    //Soma valores de serviços repetidos.
                    item_servico.valor_servico_job = Convert.ToString(Convert.ToDouble(item_servico.valor_servico_job) + Convert.ToDouble(item_servico_job.valor_servico_job));
                    Nao_Existe = false;
                    break;
                }
            }

            if (Nao_Existe)
            {
                servico = new EmissaoNF_Servico_Job();
                servico.cod_servico = item_servico_job.cod_servico;
                servico.valor_servico_job = item_servico_job.valor_servico_job;
                New_List_Servico.Add(servico);
            }
        }

        //Agrupa Tributos
        EmissaoNF_Tributo tributo = new EmissaoNF_Tributo();
        List<EmissaoNF_Tributo> List_Tributo = new List<EmissaoNF_Tributo>();
        DataTable Tributo = tributosDAO.lista_Tributos_EmissaoNF(cod_emitente, COD_SERVICO);

        foreach (EmissaoNF_Servico_Job item_servico in New_List_Servico)
        {
            foreach (DataRow row in Tributo.Rows)
            {
                if (item_servico.cod_servico == Convert.ToInt32(row["COD_SERVICO"]))
                {
                    Nao_Existe = true;

                    foreach (EmissaoNF_Tributo item in List_Tributo)
                    {
                        if (item.cod_tributo == Convert.ToInt32(row["COD_TRIBUTO"]))
                        {
                            //Soma valores de tributos repetidos.
                            item.valor_tributo += (Convert.ToDecimal(item_servico.valor_servico_job) * Convert.ToDecimal(row["ALIQUOTA"])) / 100;
                            item.valor_base += Convert.ToDecimal(item_servico.valor_servico_job);
                            Nao_Existe = false;
                            break;
                        }
                    }

                    if (Nao_Existe)
                    {
                        tributo = new EmissaoNF_Tributo();
                        tributo.cod_tributo = Convert.ToInt32(row["COD_TRIBUTO"]);
                        tributo.nome = Convert.ToString(row["NOME"]);
                        tributo.aliquota = Convert.ToString(row["ALIQUOTA"]);
                        tributo.valor_tributo = (Convert.ToDecimal(item_servico.valor_servico_job) * Convert.ToDecimal(row["ALIQUOTA"])) / 100;
                        tributo.valor_base = Convert.ToDecimal(item_servico.valor_servico_job);
                        List_Tributo.Add(tributo);
                    }
                }
            }
        }

        foreach (EmissaoNF_Tributo item in List_Tributo)
            emissao_nf_DAO.Nova_NF_Faturamento_Tributo(cod_faturamento_nf, item.cod_tributo, item.nome, item.aliquota, Math.Round(item.valor_tributo, 2, MidpointRounding.AwayFromZero), item.valor_base);
    }

    //FormGridConsultaNF     || INÍCIO ||
    public void lista_Tomadores(ref DataTable tb)
    {
        emissao_nf_DAO.lista_Tomadores(ref tb);
    }

    public int totalRegistros(Nullable<int> emitente, Nullable<int> tomador, Nullable<int> numero_nf, Nullable<int> numero_rps, Nullable<DateTime> de, Nullable<DateTime> ate, Nullable<decimal> valor_total, string situacao_nf, string status)
    {
        //return emissao_nf_DAO.totalRegistros(emitente, tomador, numero_nf, numero_rps, de, ate, valor_total, situacao_nf, status);
        return 0;
    }

    public void listaPaginada(ref DataTable tb, Nullable<int> emitente, Nullable<int> tomador, Nullable<int> numero_nf, Nullable<int> numero_rps, Nullable<DateTime> de, Nullable<DateTime> ate, Nullable<decimal> valor_total, string situacao_nf, string status, int paginaAtual, string ordenacao)
    {
        //emissao_nf_DAO.listaPaginada(ref tb, emitente, tomador, numero_nf, numero_rps, de, ate, valor_total, situacao_nf, status, paginaAtual, ordenacao);
    }

    public List<string> Atualiza_NF(List<EmissaoNF> List_EmissaoNF)
    {
        List<string> erros = new List<string>();

        foreach (EmissaoNF item in List_EmissaoNF)
            if (emissao_nf_DAO.Atualiza_NF(item.numero_nf, item.data_emissao_nf, item.numero_rps, item.data_emissao_rps, item.cpf_cnpj_emitente) == 0)
                erros.Add("Erro ao atualizar a Nota Fiscal, referente ao Número do RPS " + item.numero_rps);

        if (erros.Count > 0)
            erros.Add("\\nVerifique o Arquivo CSV da Prefeitura.");

        return erros;
    }

    public int Verifica_Baixa_Contabilizacao()
    {
        return emissao_nf_DAO.Verifica_Baixa_Contabilizacao(cod_faturamento_nf);
    }

    public List<string> Cancelar()
    {
        List<string> erros = new List<string>();

        if (cod_faturamento_nf == 0)
            erros.Add("Emitente " + emitente + " (Nº RPS " + numero_rps + "): Código inválido.");

        if (erros.Count == 0)
            emissao_nf_DAO.Cancelar(cod_faturamento_nf);

        return erros;
    }
    //FormGridConsultaNF     || FIM ||
}