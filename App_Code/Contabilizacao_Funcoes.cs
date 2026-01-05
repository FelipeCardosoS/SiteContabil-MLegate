using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class Contabilizacao_Funcoes
{
    private contabilizacaoDAO contabilizacaoDAO;
    private emissao_nf_DAO emissao_nf_DAO;
    string cod_empresa = "0";

    public Contabilizacao_Funcoes(Conexao _conn)
    {
        contabilizacaoDAO = new contabilizacaoDAO(_conn);
        emissao_nf_DAO = new emissao_nf_DAO(_conn);
    }

    public void Select_Servicos(ref DataTable tb, int cod_emitente, int cod_empresa)
    {
        contabilizacaoDAO.Select_Servicos(ref tb, cod_emitente, cod_empresa);
    }

    public void Select_Retencoes(ref DataTable tb, int cod_emitente, int cod_empresa)
    {
        contabilizacaoDAO.Select_Retencoes(ref tb, cod_emitente, cod_empresa);
    }

    public void Select_Tributos(ref DataTable tb, int cod_emitente, int cod_empresa)
    {
        contabilizacaoDAO.Select_Tributos(ref tb, cod_emitente, cod_empresa);
    }

    public void Select_Global(ref DataTable tb, int cod_emitente, int cod_empresa)
    {
        contabilizacaoDAO.Select_Global(ref tb, cod_emitente, cod_empresa);
    }

    public string Validar(string cod_conta_diferenca, string debito_credito, string historico, 
        List<Contabilizacao_Servico> List_Servico, List<Contabilizacao_Retencao> List_Retencao, List<Contabilizacao_Tributo> List_Tributo)
    {
        List<string> Mensagem_Sucesso_Erro = new List<string>();

        cod_empresa = Convert.ToString(HttpContext.Current.Session["empresa"]); //Empresa Logada

        if (string.IsNullOrEmpty(cod_empresa) || cod_empresa == null || cod_empresa == "" || cod_empresa == "0")
            Mensagem_Sucesso_Erro.Add("A sessão expirou. Faça login novamente.");

        //Serviços
        if (Mensagem_Sucesso_Erro.Count == 0)
        {
            foreach (Contabilizacao_Servico item in List_Servico)
            {
                //Débito e Crédito
                if (item.cod_conta_debito == "0" && item.cod_conta_credito == "0")
                {
                    Mensagem_Sucesso_Erro.Add("Serviço " + item.nome_servico + ": Selecione uma Conta de Débito e/ou Crédito. Respectivamente, selecione Valor Bruto ou Líquido e informe o Histórico do lançamento.");
                    break;
                }

                if (item.cod_conta_debito != "0" && item.cod_conta_credito != "0" && item.bruto_liquido_debito == "0" && item.bruto_liquido_credito == "0" && item.historico_debito.Trim() == "" && item.historico_credito.Trim() == "")
                {
                    Mensagem_Sucesso_Erro.Add("Serviço " + item.nome_servico + " (Débito e Crédito): Respectivamente, selecione Valor Bruto ou Líquido e informe o Histórico do lançamento.");
                    break;
                }

                if (item.cod_conta_debito != "0" && item.cod_conta_credito != "0" && item.bruto_liquido_debito == "0" && item.bruto_liquido_credito == "0" && item.historico_debito.Trim() != "" && item.historico_credito.Trim() != "")
                {
                    Mensagem_Sucesso_Erro.Add("Serviço " + item.nome_servico + " (Débito e Crédito): Respectivamente, selecione Valor Bruto ou Líquido.");
                    break;
                }

                if (item.cod_conta_debito != "0" && item.cod_conta_credito != "0" && item.bruto_liquido_debito != "0" && item.bruto_liquido_credito != "0" && item.historico_debito.Trim() == "" && item.historico_credito.Trim() == "")
                {
                    Mensagem_Sucesso_Erro.Add("Serviço " + item.nome_servico + " (Débito e Crédito): Respectivamente, informe o Histórico do lançamento.");
                    break;
                }

                //Débito
                if (item.cod_conta_debito != "0" && item.bruto_liquido_debito == "0" && item.historico_debito.Trim() == "")
                {
                    Mensagem_Sucesso_Erro.Add("Serviço " + item.nome_servico + " (Débito): Selecione Valor Bruto ou Líquido e informe o Histórico do lançamento.");
                    break;
                }

                if (item.cod_conta_debito != "0" && item.bruto_liquido_debito == "0" && item.historico_debito.Trim() != "")
                {
                    Mensagem_Sucesso_Erro.Add("Serviço " + item.nome_servico + " (Débito): Selecione Valor Bruto ou Líquido.");
                    break;
                }

                if (item.cod_conta_debito != "0" && item.bruto_liquido_debito != "0" && item.historico_debito.Trim() == "")
                {
                    Mensagem_Sucesso_Erro.Add("Serviço " + item.nome_servico + " (Débito): Informe o Histórico do lançamento.");
                    break;
                }

                if (item.cod_conta_debito == "0" && (item.bruto_liquido_debito != "0" || item.gera_titulo_debito == true || item.historico_debito.Trim() != ""))
                {
                    Mensagem_Sucesso_Erro.Add("Serviço " + item.nome_servico + ": Selecione uma Conta de Débito.");
                    break;
                }

                //Crédito
                if (item.cod_conta_credito != "0" && item.bruto_liquido_credito == "0" && item.historico_credito.Trim() == "")
                {
                    Mensagem_Sucesso_Erro.Add("Serviço " + item.nome_servico + " (Crédito): Selecione Valor Bruto ou Líquido e informe o Histórico do lançamento.");
                    break;
                }

                if (item.cod_conta_credito != "0" && item.bruto_liquido_credito == "0" && item.historico_credito.Trim() != "")
                {
                    Mensagem_Sucesso_Erro.Add("Serviço " + item.nome_servico + " (Crédito): Selecione Valor Bruto ou Líquido.");
                    break;
                }

                if (item.cod_conta_credito != "0" && item.bruto_liquido_credito != "0" && item.historico_credito.Trim() == "")
                {
                    Mensagem_Sucesso_Erro.Add("Serviço " + item.nome_servico + " (Crédito): Informe o Histórico do lançamento.");
                    break;
                }

                if (item.cod_conta_credito == "0" && (item.bruto_liquido_credito != "0" || item.gera_titulo_credito == true || item.historico_credito.Trim() != ""))
                {
                    Mensagem_Sucesso_Erro.Add("Serviço " + item.nome_servico + ": Selecione uma Conta de Crédito.");
                    break;
                }
            }
        }

        //Retenções
        if (Mensagem_Sucesso_Erro.Count == 0)
        {
            foreach (Contabilizacao_Retencao item in List_Retencao)
            {
                //Débito e Crédito
                if (item.cod_conta_debito == "0" && item.cod_conta_credito == "0")
                {
                    Mensagem_Sucesso_Erro.Add("Retenção " + item.nome_retencao + ": Selecione uma Conta de Débito e/ou Crédito. Respectivamente, informe o Histórico do lançamento.");
                    break;
                }

                if (item.cod_conta_debito != "0" && item.cod_conta_credito != "0" && item.historico_debito.Trim() == "" && item.historico_credito.Trim() == "")
                {
                    Mensagem_Sucesso_Erro.Add("Retenção " + item.nome_retencao + " (Débito e Crédito): Respectivamente, informe o Histórico do lançamento.");
                    break;
                }

                if (item.gera_titulo_debito == true && item.gera_titulo_credito == true && item.cod_terceiro_debito == 0 && item.cod_terceiro_credito == 0)
                {
                    Mensagem_Sucesso_Erro.Add("Retenção " + item.nome_retencao + " (Débito e Crédito): Respectivamente, selecione um Terceiro.");
                    break;
                }

                //Débito
                if (item.cod_conta_debito != "0" && item.historico_debito.Trim() == "")
                {
                    Mensagem_Sucesso_Erro.Add("Retenção " + item.nome_retencao + " (Débito): Informe o Histórico do lançamento.");
                    break;
                }

                if (item.gera_titulo_debito == true && item.cod_terceiro_debito == 0)
                {
                    Mensagem_Sucesso_Erro.Add("Retenção " + item.nome_retencao + " (Débito): Selecione um Terceiro.");
                    break;
                }

                if (item.gera_titulo_debito == false && item.cod_terceiro_debito != 0)
                {
                    Mensagem_Sucesso_Erro.Add("Retenção " + item.nome_retencao + " (Débito): Selecione Gera Título, pois um Terceiro foi selecionado.");
                    break;
                }

                if (item.cod_conta_debito == "0" && (item.gera_titulo_debito == true || item.cod_terceiro_debito != 0 || item.historico_debito.Trim() != ""))
                {
                    Mensagem_Sucesso_Erro.Add("Retenção " + item.nome_retencao + ": Selecione uma Conta de Débito.");
                    break;
                }

                //Crédito
                if (item.cod_conta_credito != "0" && item.historico_credito.Trim() == "")
                {
                    Mensagem_Sucesso_Erro.Add("Retenção " + item.nome_retencao + " (Crédito): Informe o Histórico do lançamento.");
                    break;
                }

                if (item.gera_titulo_credito == true && item.cod_terceiro_credito == 0)
                {
                    Mensagem_Sucesso_Erro.Add("Retenção " + item.nome_retencao + " (Crédito): Selecione um Terceiro.");
                    break;
                }

                if (item.gera_titulo_credito == false && item.cod_terceiro_credito != 0)
                {
                    Mensagem_Sucesso_Erro.Add("Retenção " + item.nome_retencao + " (Crédito): Selecione Gera Título, pois um Terceiro foi selecionado.");
                    break;
                }

                if (item.cod_conta_credito == "0" && (item.gera_titulo_credito == true || item.cod_terceiro_credito != 0 || item.historico_credito.Trim() != ""))
                {
                    Mensagem_Sucesso_Erro.Add("Retenção " + item.nome_retencao + ": Selecione uma Conta de Crédito.");
                    break;
                }
            }
        }

        //Tributos
        if (Mensagem_Sucesso_Erro.Count == 0)
        {
            foreach (Contabilizacao_Tributo item in List_Tributo)
            {
                //Débito e Crédito
                if (item.cod_conta_debito == "0" && item.cod_conta_credito == "0")
                {
                    Mensagem_Sucesso_Erro.Add("Tributo " + item.nome_tributo + ": Selecione uma Conta de Débito e/ou Crédito. Respectivamente, informe o Histórico do lançamento.");
                    break;
                }

                if (item.cod_conta_debito != "0" && item.cod_conta_credito != "0" && item.historico_debito.Trim() == "" && item.historico_credito.Trim() == "")
                {
                    Mensagem_Sucesso_Erro.Add("Tributo " + item.nome_tributo + " (Débito e Crédito): Respectivamente, informe o Histórico do lançamento.");
                    break;
                }

                if (item.gera_titulo_debito == true && item.gera_titulo_credito == true && item.cod_terceiro_debito == 0 && item.cod_terceiro_credito == 0)
                {
                    Mensagem_Sucesso_Erro.Add("Tributo " + item.nome_tributo + " (Débito e Crédito): Respectivamente, selecione um Terceiro.");
                    break;
                }

                //Débito
                if (item.cod_conta_debito != "0" && item.historico_debito.Trim() == "")
                {
                    Mensagem_Sucesso_Erro.Add("Tributo " + item.nome_tributo + " (Débito): Informe o Histórico do lançamento.");
                    break;
                }

                if (item.gera_titulo_debito == true && item.cod_terceiro_debito == 0)
                {
                    Mensagem_Sucesso_Erro.Add("Tributo " + item.nome_tributo + " (Débito): Selecione um Terceiro.");
                    break;
                }

                if (item.gera_titulo_debito == false && item.cod_terceiro_debito != 0)
                {
                    Mensagem_Sucesso_Erro.Add("Tributo " + item.nome_tributo + " (Débito): Selecione Gera Título, pois um Terceiro foi selecionado.");
                    break;
                }

                if (item.cod_conta_debito == "0" && (item.gera_titulo_debito == true || item.cod_terceiro_debito != 0 || item.historico_debito.Trim() != ""))
                {
                    Mensagem_Sucesso_Erro.Add("Tributo " + item.nome_tributo + ": Selecione uma Conta de Débito.");
                    break;
                }

                //Crédito
                if (item.cod_conta_credito != "0" && item.historico_credito.Trim() == "")
                {
                    Mensagem_Sucesso_Erro.Add("Tributo " + item.nome_tributo + " (Crédito): Informe o Histórico do lançamento.");
                    break;
                }

                if (item.gera_titulo_credito == true && item.cod_terceiro_credito == 0)
                {
                    Mensagem_Sucesso_Erro.Add("Tributo " + item.nome_tributo + " (Crédito): Selecione um Terceiro.");
                    break;
                }

                if (item.gera_titulo_credito == false && item.cod_terceiro_credito != 0)
                {
                    Mensagem_Sucesso_Erro.Add("Tributo " + item.nome_tributo + " (Crédito): Selecione Gera Título, pois um Terceiro foi selecionado.");
                    break;
                }

                if (item.cod_conta_credito == "0" && (item.gera_titulo_credito == true || item.cod_terceiro_credito != 0 || item.historico_credito.Trim() != ""))
                {
                    Mensagem_Sucesso_Erro.Add("Tributo " + item.nome_tributo + ": Selecione uma Conta de Crédito.");
                    break;
                }
            }
        }

        //Contas A Receber - Conta Diferença
        if (Mensagem_Sucesso_Erro.Count == 0)
        {
            if (cod_conta_diferenca == "0")
                Mensagem_Sucesso_Erro.Add("Selecione uma Conta A Receber.");

            if (debito_credito == "0")
                Mensagem_Sucesso_Erro.Add("Contas A Receber: Selecione Débito ou Crédito.");

            if (historico.Trim() == "")
                Mensagem_Sucesso_Erro.Add("Contas A Receber: Informe o Histórico do lançamento.");
        }

        if (Mensagem_Sucesso_Erro.Count == 0)
            return "";
        else
            return string.Join("\n", Mensagem_Sucesso_Erro);
    }

    public void Salvar(int cod_emitente, string cod_conta_diferenca, string debito_credito, string historico, bool gera_titulo, bool insert, bool update, 
        List<Contabilizacao_Servico> List_Servico, List<Contabilizacao_Retencao> List_Retencao, List<Contabilizacao_Tributo> List_Tributo)
    {
        //Serviços
        foreach (Contabilizacao_Servico item in List_Servico)
        {
            if (item.insert == true)
            {
                contabilizacaoDAO.Insert_Servicos(cod_emitente, item.cod_servico, Convert.ToInt32(cod_empresa), 
                    item.cod_conta_debito, item.bruto_liquido_debito, item.gera_titulo_debito, item.historico_debito, 
                    item.cod_conta_credito, item.bruto_liquido_credito, item.gera_titulo_credito, item.historico_credito);
            }
            else if (item.update == true)
            {
                contabilizacaoDAO.Update_Servicos(cod_emitente, item.cod_servico, Convert.ToInt32(cod_empresa), 
                    item.cod_conta_debito, item.bruto_liquido_debito, item.gera_titulo_debito, item.historico_debito, 
                    item.cod_conta_credito, item.bruto_liquido_credito, item.gera_titulo_credito, item.historico_credito);
            }
        }

        //Retenções
        foreach (Contabilizacao_Retencao item in List_Retencao)
        {
            if (item.insert == true)
            {
                contabilizacaoDAO.Insert_Retencoes(cod_emitente, item.cod_retencao, Convert.ToInt32(cod_empresa), 
                    item.cod_conta_debito, item.gera_titulo_debito, item.cod_terceiro_debito, item.historico_debito, 
                    item.cod_conta_credito, item.gera_titulo_credito, item.cod_terceiro_credito, item.historico_credito);
            }
            else if (item.update == true)
            {
                contabilizacaoDAO.Update_Retencoes(cod_emitente, item.cod_retencao, Convert.ToInt32(cod_empresa), 
                    item.cod_conta_debito, item.gera_titulo_debito, item.cod_terceiro_debito, item.historico_debito, 
                    item.cod_conta_credito, item.gera_titulo_credito, item.cod_terceiro_credito, item.historico_credito);
            }
        }

        //Tributos
        foreach (Contabilizacao_Tributo item in List_Tributo)
        {
            if (item.insert == true)
            {
                contabilizacaoDAO.Insert_Tributos(cod_emitente, item.cod_tributo, Convert.ToInt32(cod_empresa), 
                    item.cod_conta_debito, item.gera_titulo_debito, item.cod_terceiro_debito, item.historico_debito, 
                    item.cod_conta_credito, item.gera_titulo_credito, item.cod_terceiro_credito, item.historico_credito);
            }
            else if (item.update == true)
            {
                contabilizacaoDAO.Update_Tributos(cod_emitente, item.cod_tributo, Convert.ToInt32(cod_empresa), 
                    item.cod_conta_debito, item.gera_titulo_debito, item.cod_terceiro_debito, item.historico_debito, 
                    item.cod_conta_credito, item.gera_titulo_credito, item.cod_terceiro_credito, item.historico_credito);
            }
        }

        //Contas A Receber - Conta Diferença
        if (insert == true)
        {
            contabilizacaoDAO.Insert_Global(cod_emitente, Convert.ToInt32(cod_empresa), cod_conta_diferenca, debito_credito, historico, gera_titulo);
        }
        else if (update == true)
        {
            contabilizacaoDAO.Update_Global(cod_emitente, Convert.ToInt32(cod_empresa), cod_conta_diferenca, debito_credito, historico, gera_titulo);
        }
    }

    public string Contabilizar(int cod_faturamento_nf)
    {
        List<string> erros = new List<string>();
        double Lote = 0;

        DataTable Dados_NF = contabilizacaoDAO.Busca_Dados_NF(cod_faturamento_nf);
        DataTable Servicos_NF = contabilizacaoDAO.Busca_Servicos_NF(cod_faturamento_nf);
        DataTable Retencoes_NF = contabilizacaoDAO.Busca_Retencoes_NF(cod_faturamento_nf);
        DataTable Tributos_NF = contabilizacaoDAO.Busca_Tributos_NF(cod_faturamento_nf);
        DataTable Vencimentos_NF = contabilizacaoDAO.Busca_Vencimentos_NF(cod_faturamento_nf);

        List<SLancamento> List_SLancamento = new List<SLancamento>();
        SLancamento lancamento = new SLancamento();
        SVencimento vencimento = new SVencimento();
        lancamento.vencimentos = new List<SVencimento>();

        if (Dados_NF.Rows.Count > 0)
        {
            int cod_emitente = Convert.ToInt32(Dados_NF.Rows[0]["COD_EMITENTE"]);
            int cod_servico = 0;
            int cod_job = 0;
            int cod_retencao = 0;
            int cod_tributo = 0;
            int numero_lancamento = 2;
            decimal Valor_Lote = 0;
            string cod_conta = "";
            DataTable Parametros_Servicos = new DataTable();
            DataTable Dados_Job = new DataTable();

            #region Servicos_NF

            foreach (DataRow servicos_nf in Servicos_NF.Rows)
            {
                //DataTable Parametros_Servicos = new DataTable();

                if (cod_servico != Convert.ToInt32(servicos_nf["COD_SERVICO"]))
                {
                    cod_servico = Convert.ToInt32(servicos_nf["COD_SERVICO"]);
                    Parametros_Servicos = contabilizacaoDAO.Busca_Parametros_Servicos(cod_emitente, cod_servico);
                }

                if (cod_job != Convert.ToInt32(servicos_nf["COD_JOB"]))
                {
                    cod_job = Convert.ToInt32(servicos_nf["COD_JOB"]);
                    Dados_Job = contabilizacaoDAO.Busca_Dados_Job(cod_job);
                }

                if (Parametros_Servicos.Rows.Count > 0)
                    {
                        //Contabilização de Dédito
                        cod_conta = Parametros_Servicos.Rows[0]["COD_CONTA_DEBITO"].ToString();
                        DataTable Dados_Conta = contabilizacaoDAO.Busca_Dados_Conta(cod_conta);
                        
                        if (Dados_Conta.Rows.Count > 0 && cod_conta != "0")
                        {
                            if (Dados_Job.Rows.Count > 0)
                            {
                                lancamento.seqLote = numero_lancamento;
                                numero_lancamento++;
                                lancamento.terceiro = Convert.ToInt32(Dados_NF.Rows[0]["COD_TOMADOR"]);
                                lancamento.cliente = Convert.ToInt32(Dados_Job.Rows[0]["COD_CLIENTE"]);
                                lancamento.descCliente = Convert.ToString(Dados_Job.Rows[0]["NOME_RAZAO_SOCIAL"]);
                                lancamento.codConsultor = Convert.ToInt32(Dados_NF.Rows[0]["COD_EMITENTE"]);
                                lancamento.conta = cod_conta;
                                lancamento.dataLancamento = Convert.ToDateTime(Dados_NF.Rows[0]["DATA_EMISSAO_RPS"]);
                                lancamento.debCred = 'D';
                                lancamento.descTerceiro = Convert.ToString(Dados_NF.Rows[0]["NOME_RAZAO_SOCIAL_TOMADOR"]);
                                lancamento.descConsultor = Convert.ToString(Dados_NF.Rows[0]["NOME_RAZAO_SOCIAL_EMITENTE"]);
                                lancamento.descConta = Convert.ToString(Dados_Conta.Rows[0]["DESCRICAO"]);
                                lancamento.descDivisao = Convert.ToString(Dados_Job.Rows[0]["DESC_DIVISAO"]);
                                lancamento.divisao = Convert.ToInt32(Dados_Job.Rows[0]["COD_DIVISAO"]);
                                lancamento.descJob = Convert.ToString(Dados_Job.Rows[0]["DESCRICAO"]);
                                lancamento.descLinhaNegocio = Convert.ToString(Dados_Job.Rows[0]["DESC_LINHA"]);
                                lancamento.linhaNegocio = Convert.ToInt32(Dados_Job.Rows[0]["COD_LINHA_NEGOCIO"]);
                                lancamento.historico = Parametros_Servicos.Rows[0]["HISTORICO_DEBITO"].ToString() + " - " + Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]) + " - " + Convert.ToString(Dados_Job.Rows[0]["NOME_RAZAO_SOCIAL"]);
                                lancamento.modulo = "CAR_INCLUSAO_TITULO";
                                lancamento.numeroDocumento = string.Concat("RPS-", Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]));
                                lancamento.qtd = 1;
                                lancamento.valorBruto = Convert.ToDecimal(servicos_nf["VALOR"]);
                                lancamento.job = Convert.ToInt32(Dados_Job.Rows[0]["COD_JOB"]);
                                lancamento.modelo = 0;
                                lancamento.codMoeda = 0;

                                if (Convert.ToString(Parametros_Servicos.Rows[0]["BRUTO_LIQUIDO_DEBITO"]) == "BRUTO")
                                {
                                    lancamento.valorUnit = Convert.ToDecimal(servicos_nf["VALOR"]);
                                    lancamento.valor = Convert.ToDecimal(servicos_nf["VALOR"]);
                                }
                                else // LÍQUIDO (Valor Líquido)
                                {
                                    decimal vlr_retencoes = 0;

                                    foreach (DataRow retencoes_nf in Retencoes_NF.Rows)
                                    {
                                        vlr_retencoes = vlr_retencoes + Convert.ToDecimal(servicos_nf["VALOR"]) * Convert.ToDecimal(retencoes_nf["ALIQUOTA"]) / 100;
                                    }
                                    lancamento.valorUnit = Convert.ToDecimal(servicos_nf["VALOR"]) - Math.Round(vlr_retencoes, 2, MidpointRounding.AwayFromZero);
                                    lancamento.valor = lancamento.valorUnit;
                                }
                                Valor_Lote = Valor_Lote + Convert.ToDecimal(lancamento.valor);

                                if (Convert.ToBoolean(Parametros_Servicos.Rows[0]["GERA_TITULO_DEBITO"]))
                                {
                                    lancamento.titulo = true;

                                    if (lancamento.vencimentos == null)
                                        lancamento.vencimentos = new List<SVencimento>();
                                        
                                    vencimento.data = lancamento.dataLancamento;
                                    vencimento.valor = lancamento.valor;
                                    lancamento.vencimentos.Add(vencimento);
                                    vencimento = new SVencimento();
                                }
                                else
                                    lancamento.titulo = false;
                            }
                            List_SLancamento.Add(lancamento);
                            lancamento = new SLancamento();
                        }

                        //Contabilização de Crédito
                        cod_conta = Parametros_Servicos.Rows[0]["COD_CONTA_CREDITO"].ToString();
                        Dados_Conta = contabilizacaoDAO.Busca_Dados_Conta(cod_conta);

                        if (Dados_Conta.Rows.Count > 0 && cod_conta != "0")
                        {
                            if (Dados_Job.Rows.Count > 0)
                            {
                                lancamento.seqLote = numero_lancamento;
                                numero_lancamento++;
                                lancamento.terceiro = Convert.ToInt32(Dados_NF.Rows[0]["COD_TOMADOR"]);
                                lancamento.cliente = Convert.ToInt32(Dados_Job.Rows[0]["COD_CLIENTE"]);
                                lancamento.descCliente = Convert.ToString(Dados_Job.Rows[0]["NOME_RAZAO_SOCIAL"]);
                                lancamento.codConsultor = Convert.ToInt32(Dados_NF.Rows[0]["COD_EMITENTE"]);
                                lancamento.conta = cod_conta;
                                lancamento.dataLancamento = Convert.ToDateTime(Dados_NF.Rows[0]["DATA_EMISSAO_RPS"]);
                                lancamento.debCred = 'C';
                                lancamento.descTerceiro = Convert.ToString(Dados_NF.Rows[0]["NOME_RAZAO_SOCIAL_TOMADOR"]);
                                lancamento.descConsultor = Convert.ToString(Dados_NF.Rows[0]["NOME_RAZAO_SOCIAL_EMITENTE"]);
                                lancamento.descConta = Convert.ToString(Dados_Conta.Rows[0]["DESCRICAO"]);
                                lancamento.descDivisao = Convert.ToString(Dados_Job.Rows[0]["DESC_DIVISAO"]);
                                lancamento.divisao = Convert.ToInt32(Dados_Job.Rows[0]["COD_DIVISAO"]);
                                lancamento.descJob = Convert.ToString(Dados_Job.Rows[0]["DESCRICAO"]);
                                lancamento.descLinhaNegocio = Convert.ToString(Dados_Job.Rows[0]["DESC_LINHA"]);
                                lancamento.linhaNegocio = Convert.ToInt32(Dados_Job.Rows[0]["COD_LINHA_NEGOCIO"]);
                                lancamento.historico = Parametros_Servicos.Rows[0]["HISTORICO_CREDITO"].ToString() + " - " + Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]) + " - " + Convert.ToString(Dados_Job.Rows[0]["NOME_RAZAO_SOCIAL"]);
                                lancamento.modulo = "CAR_INCLUSAO_TITULO";
                                lancamento.numeroDocumento = string.Concat("RPS-", Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]));
                                lancamento.qtd = 1;
                                lancamento.valorBruto = Convert.ToDecimal(servicos_nf["VALOR"]);
                                lancamento.job = Convert.ToInt32(Dados_Job.Rows[0]["COD_JOB"]);
                                lancamento.modelo = 0;
                                lancamento.codMoeda = 0;

                                if (Convert.ToString(Parametros_Servicos.Rows[0]["BRUTO_LIQUIDO_CREDITO"]) == "BRUTO")
                                {
                                    lancamento.valorUnit = Convert.ToDecimal(servicos_nf["VALOR"]);
                                    lancamento.valor = Convert.ToDecimal(servicos_nf["VALOR"]);
                                }
                                else // LÍQUIDO (Valor Líquido)
                                {
                                    decimal vlr_retencoes = 0;

                                    foreach (DataRow retencoes_nf in Retencoes_NF.Rows)
                                    {
                                        vlr_retencoes = vlr_retencoes + Convert.ToDecimal(servicos_nf["VALOR"]) * Convert.ToDecimal(retencoes_nf["ALIQUOTA"]) / 100;
                                    }
                                    lancamento.valorUnit = Convert.ToDecimal(servicos_nf["VALOR"]) - Math.Round(vlr_retencoes, 2, MidpointRounding.AwayFromZero);
                                    lancamento.valor = lancamento.valorUnit;
                                }
                                Valor_Lote = Valor_Lote - Convert.ToDecimal(lancamento.valor);

                                if (Convert.ToBoolean(Parametros_Servicos.Rows[0]["GERA_TITULO_CREDITO"]))
                                {
                                    lancamento.titulo = true;

                                    if (lancamento.vencimentos == null)
                                        lancamento.vencimentos = new List<SVencimento>();

                                    vencimento.data = lancamento.dataLancamento;
                                    vencimento.valor = lancamento.valor;
                                    lancamento.vencimentos.Add(vencimento);
                                    vencimento = new SVencimento();
                                }
                                else
                                    lancamento.titulo = false;
                            }
                            List_SLancamento.Add(lancamento);
                            lancamento = new SLancamento();
                        }
                    }
            } //foreach Servicos_NF
            #endregion Servicos_NF

            #region Retencoes_NF

            foreach (DataRow retencoes_nf in Retencoes_NF.Rows)
            {
                if (cod_retencao != Convert.ToInt32(retencoes_nf["COD_RETENCAO"]))
                {
                    cod_retencao = Convert.ToInt32(retencoes_nf["COD_RETENCAO"]);
                    DataTable Parametros_Retencoes = contabilizacaoDAO.Busca_Parametros_Retencoes(cod_emitente, cod_retencao);

                    if (Parametros_Retencoes.Rows.Count > 0)
                    {
                        //Contabilização de Dédito
                        cod_conta = Parametros_Retencoes.Rows[0]["COD_CONTA_DEBITO"].ToString();
                        DataTable Dados_Conta = contabilizacaoDAO.Busca_Dados_Conta(cod_conta);

                        if (Dados_Conta.Rows.Count > 0 && cod_conta != "0")
                        {
                            if (Dados_Job.Rows.Count > 0)
                            {
                                lancamento.seqLote = numero_lancamento;
                                numero_lancamento++;
                                lancamento.terceiro = Convert.ToInt32(Parametros_Retencoes.Rows[0]["COD_TERCEIRO_DEBITO"]);
                                lancamento.cliente = Convert.ToInt32(Dados_Job.Rows[0]["COD_CLIENTE"]);
                                lancamento.descCliente = Convert.ToString(Dados_Job.Rows[0]["NOME_RAZAO_SOCIAL"]);
                                lancamento.codConsultor = Convert.ToInt32(Dados_NF.Rows[0]["COD_EMITENTE"]);
                                lancamento.conta = cod_conta;
                                lancamento.dataLancamento = Convert.ToDateTime(Dados_NF.Rows[0]["DATA_EMISSAO_RPS"]);
                                lancamento.debCred = 'D';
                                lancamento.descTerceiro = Busca_Nome_Empresa(Convert.ToInt32(Parametros_Retencoes.Rows[0]["COD_TERCEIRO_DEBITO"]));
                                lancamento.descConsultor = Convert.ToString(Dados_NF.Rows[0]["NOME_RAZAO_SOCIAL_EMITENTE"]);
                                lancamento.descConta = Convert.ToString(Dados_Conta.Rows[0]["DESCRICAO"]);
                                lancamento.descDivisao = Convert.ToString(Dados_Job.Rows[0]["DESC_DIVISAO"]);
                                lancamento.divisao = Convert.ToInt32(Dados_Job.Rows[0]["COD_DIVISAO"]);
                                lancamento.descJob = Convert.ToString(Dados_Job.Rows[0]["DESCRICAO"]);
                                lancamento.descLinhaNegocio = Convert.ToString(Dados_Job.Rows[0]["DESC_LINHA"]);
                                lancamento.linhaNegocio = Convert.ToInt32(Dados_Job.Rows[0]["COD_LINHA_NEGOCIO"]);
                                lancamento.historico = Parametros_Retencoes.Rows[0]["HISTORICO_DEBITO"].ToString() + " - " + Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]) + " - " + Convert.ToString(Dados_Job.Rows[0]["NOME_RAZAO_SOCIAL"]);
                                lancamento.modulo = "CAR_INCLUSAO_TITULO";
                                lancamento.numeroDocumento = string.Concat("RPS-", Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]));
                                lancamento.qtd = 1;
                                lancamento.valorBruto = Convert.ToDecimal(retencoes_nf["VALOR"]);
                                lancamento.valorUnit = Convert.ToDecimal(retencoes_nf["VALOR"]);
                                lancamento.valor = Convert.ToDecimal(retencoes_nf["VALOR"]);
                                lancamento.job = Convert.ToInt32(Dados_Job.Rows[0]["COD_JOB"]);
                                lancamento.modelo = 0;
                                lancamento.codMoeda = 0;

                                Valor_Lote = Valor_Lote + Convert.ToDecimal(lancamento.valor);

                                if (Convert.ToBoolean(Parametros_Retencoes.Rows[0]["GERA_TITULO_DEBITO"]))
                                {
                                    lancamento.titulo = true;

                                    if (lancamento.vencimentos == null)
                                        lancamento.vencimentos = new List<SVencimento>();

                                    vencimento.data = lancamento.dataLancamento;
                                    vencimento.valor = lancamento.valor;
                                    lancamento.vencimentos.Add(vencimento);
                                    vencimento = new SVencimento();
                                }
                                else
                                    lancamento.titulo = false;
                            }
                            List_SLancamento.Add(lancamento);
                            lancamento = new SLancamento();
                        }

                        //Contabilização de Crédito
                        cod_conta = Parametros_Retencoes.Rows[0]["COD_CONTA_CREDITO"].ToString();
                        Dados_Conta = contabilizacaoDAO.Busca_Dados_Conta(cod_conta);

                        if (Dados_Conta.Rows.Count > 0 && cod_conta != "0")
                        {
                            if (Dados_Job.Rows.Count > 0)
                            {
                                lancamento.seqLote = numero_lancamento;
                                numero_lancamento++;
                                lancamento.terceiro = Convert.ToInt32(Parametros_Retencoes.Rows[0]["COD_TERCEIRO_CREDITO"]);
                                lancamento.cliente = Convert.ToInt32(Dados_Job.Rows[0]["COD_CLIENTE"]);
                                lancamento.descCliente = Convert.ToString(Dados_Job.Rows[0]["NOME_RAZAO_SOCIAL"]);
                                lancamento.codConsultor = Convert.ToInt32(Dados_NF.Rows[0]["COD_EMITENTE"]);
                                lancamento.conta = cod_conta;
                                lancamento.dataLancamento = Convert.ToDateTime(Dados_NF.Rows[0]["DATA_EMISSAO_RPS"]);
                                lancamento.debCred = 'C';
                                lancamento.descTerceiro = Busca_Nome_Empresa(Convert.ToInt32(Parametros_Retencoes.Rows[0]["COD_TERCEIRO_CREDITO"]));
                                lancamento.descConsultor = Convert.ToString(Dados_NF.Rows[0]["NOME_RAZAO_SOCIAL_EMITENTE"]);
                                lancamento.descConta = Convert.ToString(Dados_Conta.Rows[0]["DESCRICAO"]);
                                lancamento.descDivisao = Convert.ToString(Dados_Job.Rows[0]["DESC_DIVISAO"]);
                                lancamento.divisao = Convert.ToInt32(Dados_Job.Rows[0]["COD_DIVISAO"]);
                                lancamento.descJob = Convert.ToString(Dados_Job.Rows[0]["DESCRICAO"]);
                                lancamento.descLinhaNegocio = Convert.ToString(Dados_Job.Rows[0]["DESC_LINHA"]);
                                lancamento.linhaNegocio = Convert.ToInt32(Dados_Job.Rows[0]["COD_LINHA_NEGOCIO"]);
                                lancamento.historico = Parametros_Retencoes.Rows[0]["HISTORICO_CREDITO"].ToString() + " - " + Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]) + " - " + Convert.ToString(Dados_Job.Rows[0]["NOME_RAZAO_SOCIAL"]);
                                lancamento.modulo = "CAR_INCLUSAO_TITULO";
                                lancamento.numeroDocumento = string.Concat("RPS-", Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]));
                                lancamento.qtd = 1;
                                lancamento.valorBruto = Convert.ToDecimal(retencoes_nf["VALOR"]);
                                lancamento.valorUnit = Convert.ToDecimal(retencoes_nf["VALOR"]);
                                lancamento.valor = Convert.ToDecimal(retencoes_nf["VALOR"]);
                                lancamento.job = Convert.ToInt32(Dados_Job.Rows[0]["COD_JOB"]);
                                lancamento.modelo = 0;
                                lancamento.codMoeda = 0;

                                Valor_Lote = Valor_Lote - Convert.ToDecimal(lancamento.valor);

                                if (Convert.ToBoolean(Parametros_Retencoes.Rows[0]["GERA_TITULO_CREDITO"]))
                                {
                                    lancamento.titulo = true;

                                    if (lancamento.vencimentos == null)
                                        lancamento.vencimentos = new List<SVencimento>();

                                    vencimento.data = lancamento.dataLancamento;
                                    vencimento.valor = lancamento.valor;
                                    lancamento.vencimentos.Add(vencimento);
                                    vencimento = new SVencimento();
                                }
                                else
                                    lancamento.titulo = false;
                            }
                            List_SLancamento.Add(lancamento);
                            lancamento = new SLancamento();
                        }
                    }
                }
            } //foreach Retencoes_NF
            #endregion Retenções NF

            #region Tributos_NF
            foreach (DataRow tributos_nf in Tributos_NF.Rows)
            {
                if (cod_tributo != Convert.ToInt32(tributos_nf["COD_TRIBUTO"]))
                {
                    cod_tributo = Convert.ToInt32(tributos_nf["COD_TRIBUTO"]);
                    DataTable Parametros_Tributos = contabilizacaoDAO.Busca_Parametros_Tributos(cod_emitente, cod_tributo);

                    if (Parametros_Tributos.Rows.Count > 0)
                    {
                        //Contabilização de Dédito
                        cod_conta = Parametros_Tributos.Rows[0]["COD_CONTA_DEBITO"].ToString();
                        DataTable Dados_Conta = contabilizacaoDAO.Busca_Dados_Conta(cod_conta);

                        if (Dados_Conta.Rows.Count > 0 && cod_conta != "0")
                        {
                            if (Convert.ToChar(Dados_Conta.Rows[0]["REGRA_EXIBICAO"]).Equals('B'))
                            {

                                if (Dados_Job.Rows.Count > 0)
                                {
                                    lancamento.seqLote = numero_lancamento;
                                    numero_lancamento++;
                                    lancamento.terceiro = Convert.ToInt32(Parametros_Tributos.Rows[0]["COD_TERCEIRO_DEBITO"]);
                                    lancamento.cliente = Convert.ToInt32(Dados_Job.Rows[0]["COD_CLIENTE"]);
                                    lancamento.descCliente = Convert.ToString(Dados_Job.Rows[0]["NOME_RAZAO_SOCIAL"]);
                                    lancamento.codConsultor = Convert.ToInt32(Dados_NF.Rows[0]["COD_EMITENTE"]);
                                    lancamento.conta = cod_conta;
                                    lancamento.dataLancamento = Convert.ToDateTime(Dados_NF.Rows[0]["DATA_EMISSAO_RPS"]);
                                    lancamento.debCred = 'D';
                                    lancamento.descTerceiro = Busca_Nome_Empresa(Convert.ToInt32(Parametros_Tributos.Rows[0]["COD_TERCEIRO_DEBITO"]));
                                    lancamento.descConsultor = Convert.ToString(Dados_NF.Rows[0]["NOME_RAZAO_SOCIAL_EMITENTE"]);
                                    lancamento.descConta = Convert.ToString(Dados_Conta.Rows[0]["DESCRICAO"]);
                                    lancamento.descDivisao = Convert.ToString(Dados_Job.Rows[0]["DESC_DIVISAO"]);
                                    lancamento.divisao = Convert.ToInt32(Dados_Job.Rows[0]["COD_DIVISAO"]);
                                    lancamento.descJob = Convert.ToString(Dados_Job.Rows[0]["DESCRICAO"]);
                                    lancamento.descLinhaNegocio = Convert.ToString(Dados_Job.Rows[0]["DESC_LINHA"]);
                                    lancamento.linhaNegocio = Convert.ToInt32(Dados_Job.Rows[0]["COD_LINHA_NEGOCIO"]);
                                    lancamento.historico = Parametros_Tributos.Rows[0]["HISTORICO_DEBITO"].ToString() + " - " + Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]) + " - " + Convert.ToString(Dados_Job.Rows[0]["NOME_RAZAO_SOCIAL"]);
                                    lancamento.modulo = "CAR_INCLUSAO_TITULO";
                                    lancamento.numeroDocumento = string.Concat("RPS-", Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]));
                                    lancamento.qtd = 1;
                                    lancamento.valorBruto = Convert.ToDecimal(tributos_nf["VALOR"]);
                                    lancamento.valorUnit = Convert.ToDecimal(tributos_nf["VALOR"]);
                                    lancamento.valor = Convert.ToDecimal(tributos_nf["VALOR"]);
                                    lancamento.job = Convert.ToInt32(Dados_Job.Rows[0]["COD_JOB"]);
                                    lancamento.modelo = 0;
                                    lancamento.codMoeda = 0;

                                    Valor_Lote = Valor_Lote + Convert.ToDecimal(lancamento.valor);

                                    if (Convert.ToBoolean(Parametros_Tributos.Rows[0]["GERA_TITULO_DEBITO"]))
                                    {
                                        lancamento.titulo = true;

                                        if (lancamento.vencimentos == null)
                                            lancamento.vencimentos = new List<SVencimento>();

                                        vencimento.data = lancamento.dataLancamento;
                                        vencimento.valor = lancamento.valor;
                                        lancamento.vencimentos.Add(vencimento);
                                        vencimento = new SVencimento();
                                    }
                                    else
                                        lancamento.titulo = false;
                                }
                                List_SLancamento.Add(lancamento);
                                lancamento = new SLancamento();
                            }
                            else
                            {
                                List<SLancamento> lanctosTributo = new List<SLancamento>();
                                foreach (DataRow servico in Servicos_NF.Rows)
                                {
                                    DataTable jobAux = contabilizacaoDAO.Busca_Dados_Job(Convert.ToInt32(servico["COD_JOB"]));

                                    if (Dados_Conta.Rows.Count > 0 && cod_conta != "0")
                                    {
                                        if (jobAux.Rows.Count > 0)
                                        {
                                            lancamento.seqLote = numero_lancamento;
                                            numero_lancamento++;
                                            lancamento.terceiro = Convert.ToInt32(Parametros_Tributos.Rows[0]["COD_TERCEIRO_DEBITO"]);
                                            lancamento.cliente = Convert.ToInt32(jobAux.Rows[0]["COD_CLIENTE"]);
                                            lancamento.descCliente = Convert.ToString(jobAux.Rows[0]["NOME_RAZAO_SOCIAL"]);
                                            lancamento.codConsultor = Convert.ToInt32(Dados_NF.Rows[0]["COD_EMITENTE"]);
                                            lancamento.conta = cod_conta;
                                            lancamento.dataLancamento = Convert.ToDateTime(Dados_NF.Rows[0]["DATA_EMISSAO_RPS"]);
                                            lancamento.debCred = 'D';
                                            lancamento.descTerceiro = Busca_Nome_Empresa(Convert.ToInt32(Parametros_Tributos.Rows[0]["COD_TERCEIRO_DEBITO"]));
                                            lancamento.descConsultor = Convert.ToString(Dados_NF.Rows[0]["NOME_RAZAO_SOCIAL_EMITENTE"]);
                                            lancamento.descConta = Convert.ToString(Dados_Conta.Rows[0]["DESCRICAO"]);
                                            lancamento.descDivisao = Convert.ToString(jobAux.Rows[0]["DESC_DIVISAO"]);
                                            lancamento.divisao = Convert.ToInt32(jobAux.Rows[0]["COD_DIVISAO"]);
                                            lancamento.descJob = Convert.ToString(jobAux.Rows[0]["DESCRICAO"]);
                                            lancamento.descLinhaNegocio = Convert.ToString(jobAux.Rows[0]["DESC_LINHA"]);
                                            lancamento.linhaNegocio = Convert.ToInt32(jobAux.Rows[0]["COD_LINHA_NEGOCIO"]);
                                            lancamento.historico = Parametros_Tributos.Rows[0]["HISTORICO_DEBITO"].ToString() + " - " + Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]) + " - " + Convert.ToString(jobAux.Rows[0]["NOME_RAZAO_SOCIAL"]);
                                            lancamento.modulo = "CAR_INCLUSAO_TITULO";
                                            lancamento.numeroDocumento = string.Concat("RPS-", Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]));
                                            lancamento.qtd = 1;
                                            lancamento.valorBruto = Math.Round(Convert.ToDecimal(servico["VALOR"]) * Convert.ToDecimal(tributos_nf["ALIQUOTA"]) / 100, 2);
                                            lancamento.valorUnit = Math.Round(Convert.ToDecimal(servico["VALOR"]) * Convert.ToDecimal(tributos_nf["ALIQUOTA"]) / 100, 2);
                                            lancamento.valor = Math.Round(Convert.ToDecimal(servico["VALOR"]) * Convert.ToDecimal(tributos_nf["ALIQUOTA"]) / 100, 2);
                                            lancamento.job = Convert.ToInt32(jobAux.Rows[0]["COD_JOB"]);
                                            lancamento.modelo = 0;
                                            lancamento.codMoeda = 0;

                                            Valor_Lote = Valor_Lote + Convert.ToDecimal(lancamento.valor);

                                            if (Convert.ToBoolean(Parametros_Tributos.Rows[0]["GERA_TITULO_DEBITO"]))
                                            {
                                                lancamento.titulo = true;

                                                if (lancamento.vencimentos == null)
                                                    lancamento.vencimentos = new List<SVencimento>();

                                                vencimento.data = lancamento.dataLancamento;
                                                vencimento.valor = lancamento.valor;
                                                lancamento.vencimentos.Add(vencimento);
                                                vencimento = new SVencimento();
                                            }
                                            else
                                                lancamento.titulo = false;
                                        }
                                        lanctosTributo.Add(lancamento);
                                        lancamento = new SLancamento();
                                    }

                                }

                                Valor_Lote -= (lanctosTributo.Sum(o => o.valor) - Convert.ToDecimal(tributos_nf["VALOR"])).Value;

                                lanctosTributo.Last().valor -= lanctosTributo.Sum(o => o.valor) - Convert.ToDecimal(tributos_nf["VALOR"]);
                                lanctosTributo.Last().valorBruto = lanctosTributo.Last().valor;
                                lanctosTributo.Last().valorUnit = lanctosTributo.Last().valor;


                                List_SLancamento.AddRange(lanctosTributo);
                            }
                        }


                        //Contabilização de Crédito
                        cod_conta = Parametros_Tributos.Rows[0]["COD_CONTA_CREDITO"].ToString();
                        Dados_Conta = contabilizacaoDAO.Busca_Dados_Conta(cod_conta);

                        if (Dados_Conta.Rows.Count > 0 && cod_conta != "0")
                        {
                            if (Convert.ToChar(Dados_Conta.Rows[0]["REGRA_EXIBICAO"]).Equals('B'))
                            {
                                if (Dados_Job.Rows.Count > 0)
                                {
                                    lancamento.seqLote = numero_lancamento;
                                    numero_lancamento++;
                                    lancamento.terceiro = Convert.ToInt32(Parametros_Tributos.Rows[0]["COD_TERCEIRO_CREDITO"]);
                                    lancamento.cliente = Convert.ToInt32(Dados_Job.Rows[0]["COD_CLIENTE"]);
                                    lancamento.descCliente = Convert.ToString(Dados_Job.Rows[0]["NOME_RAZAO_SOCIAL"]);
                                    lancamento.codConsultor = Convert.ToInt32(Dados_NF.Rows[0]["COD_EMITENTE"]);
                                    lancamento.conta = cod_conta;
                                    lancamento.dataLancamento = Convert.ToDateTime(Dados_NF.Rows[0]["DATA_EMISSAO_RPS"]);
                                    lancamento.debCred = 'C';
                                    lancamento.descTerceiro = Busca_Nome_Empresa(Convert.ToInt32(Parametros_Tributos.Rows[0]["COD_TERCEIRO_CREDITO"]));
                                    lancamento.descConsultor = Convert.ToString(Dados_NF.Rows[0]["NOME_RAZAO_SOCIAL_EMITENTE"]);
                                    lancamento.descConta = Convert.ToString(Dados_Conta.Rows[0]["DESCRICAO"]);
                                    lancamento.descDivisao = Convert.ToString(Dados_Job.Rows[0]["DESC_DIVISAO"]);
                                    lancamento.divisao = Convert.ToInt32(Dados_Job.Rows[0]["COD_DIVISAO"]);
                                    lancamento.descJob = Convert.ToString(Dados_Job.Rows[0]["DESCRICAO"]);
                                    lancamento.descLinhaNegocio = Convert.ToString(Dados_Job.Rows[0]["DESC_LINHA"]);
                                    lancamento.linhaNegocio = Convert.ToInt32(Dados_Job.Rows[0]["COD_LINHA_NEGOCIO"]);
                                    lancamento.historico = Parametros_Tributos.Rows[0]["HISTORICO_CREDITO"].ToString() + " - " + Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]) + " - " + Convert.ToString(Dados_Job.Rows[0]["NOME_RAZAO_SOCIAL"]);
                                    lancamento.modulo = "CAR_INCLUSAO_TITULO";
                                    lancamento.numeroDocumento = string.Concat("RPS-", Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]));
                                    lancamento.qtd = 1;
                                    lancamento.valorBruto = Convert.ToDecimal(tributos_nf["VALOR"]);
                                    lancamento.valorUnit = Convert.ToDecimal(tributos_nf["VALOR"]);
                                    lancamento.valor = Convert.ToDecimal(tributos_nf["VALOR"]);
                                    lancamento.job = Convert.ToInt32(Dados_Job.Rows[0]["COD_JOB"]);
                                    lancamento.modelo = 0;
                                    lancamento.codMoeda = 0;

                                    Valor_Lote = Valor_Lote - Convert.ToDecimal(lancamento.valor);

                                    if (Convert.ToBoolean(Parametros_Tributos.Rows[0]["GERA_TITULO_CREDITO"]))
                                    {
                                        lancamento.titulo = true;

                                        if (lancamento.vencimentos == null)
                                            lancamento.vencimentos = new List<SVencimento>();

                                        vencimento.data = lancamento.dataLancamento;
                                        vencimento.valor = lancamento.valor;
                                        lancamento.vencimentos.Add(vencimento);
                                        vencimento = new SVencimento();
                                    }
                                    else
                                        lancamento.titulo = false;
                                }
                                List_SLancamento.Add(lancamento);
                                lancamento = new SLancamento();
                            }
                            else
                            {
                                List<SLancamento> lanctosTributo = new List<SLancamento>();

                                foreach (DataRow servico in Servicos_NF.Rows)
                                {
                                    DataTable jobAux = contabilizacaoDAO.Busca_Dados_Job(Convert.ToInt32(servico["COD_JOB"]));

                                    if (Dados_Conta.Rows.Count > 0 && cod_conta != "0")
                                    {
                                        if (jobAux.Rows.Count > 0)
                                        {
                                            lancamento.seqLote = numero_lancamento;
                                            numero_lancamento++;
                                            lancamento.terceiro = Convert.ToInt32(Parametros_Tributos.Rows[0]["COD_TERCEIRO_CREDITO"]);
                                            lancamento.cliente = Convert.ToInt32(jobAux.Rows[0]["COD_CLIENTE"]);
                                            lancamento.descCliente = Convert.ToString(jobAux.Rows[0]["NOME_RAZAO_SOCIAL"]);
                                            lancamento.codConsultor = Convert.ToInt32(Dados_NF.Rows[0]["COD_EMITENTE"]);
                                            lancamento.conta = cod_conta;
                                            lancamento.dataLancamento = Convert.ToDateTime(Dados_NF.Rows[0]["DATA_EMISSAO_RPS"]);
                                            lancamento.debCred = 'C';
                                            lancamento.descTerceiro = Busca_Nome_Empresa(Convert.ToInt32(Parametros_Tributos.Rows[0]["COD_TERCEIRO_CREDITO"]));
                                            lancamento.descConsultor = Convert.ToString(Dados_NF.Rows[0]["NOME_RAZAO_SOCIAL_EMITENTE"]);
                                            lancamento.descConta = Convert.ToString(Dados_Conta.Rows[0]["DESCRICAO"]);
                                            lancamento.descDivisao = Convert.ToString(jobAux.Rows[0]["DESC_DIVISAO"]);
                                            lancamento.divisao = Convert.ToInt32(jobAux.Rows[0]["COD_DIVISAO"]);
                                            lancamento.descJob = Convert.ToString(jobAux.Rows[0]["DESCRICAO"]);
                                            lancamento.descLinhaNegocio = Convert.ToString(jobAux.Rows[0]["DESC_LINHA"]);
                                            lancamento.linhaNegocio = Convert.ToInt32(jobAux.Rows[0]["COD_LINHA_NEGOCIO"]);
                                            lancamento.historico = Parametros_Tributos.Rows[0]["HISTORICO_CREDITO"].ToString() + " - " + Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]) + " - " + Convert.ToString(jobAux.Rows[0]["NOME_RAZAO_SOCIAL"]);
                                            lancamento.modulo = "CAR_INCLUSAO_TITULO";
                                            lancamento.numeroDocumento = string.Concat("RPS-", Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]));
                                            lancamento.qtd = 1;
                                            lancamento.valorBruto = Math.Round(Convert.ToDecimal(servico["VALOR"]) * Convert.ToDecimal(tributos_nf["ALIQUOTA"]) / 100, 2);
                                            lancamento.valorUnit = Math.Round(Convert.ToDecimal(servico["VALOR"]) * Convert.ToDecimal(tributos_nf["ALIQUOTA"]) / 100, 2);
                                            lancamento.valor = Math.Round(Convert.ToDecimal(servico["VALOR"]) * Convert.ToDecimal(tributos_nf["ALIQUOTA"]) / 100, 2);
                                            lancamento.job = Convert.ToInt32(jobAux.Rows[0]["COD_JOB"]);
                                            lancamento.modelo = 0;
                                            lancamento.codMoeda = 0;

                                            Valor_Lote = Valor_Lote - Convert.ToDecimal(lancamento.valor);

                                            if (Convert.ToBoolean(Parametros_Tributos.Rows[0]["GERA_TITULO_CREDITO"]))
                                            {
                                                lancamento.titulo = true;

                                                if (lancamento.vencimentos == null)
                                                    lancamento.vencimentos = new List<SVencimento>();

                                                vencimento.data = lancamento.dataLancamento;
                                                vencimento.valor = lancamento.valor;
                                                lancamento.vencimentos.Add(vencimento);
                                                vencimento = new SVencimento();
                                            }
                                            else
                                                lancamento.titulo = false;
                                        }
                                        lanctosTributo.Add(lancamento);
                                        lancamento = new SLancamento();
                                    }
                                }

                                Valor_Lote += (lanctosTributo.Sum(o => o.valor) - Convert.ToDecimal(tributos_nf["VALOR"])).Value;

                                lanctosTributo.Last().valor -= lanctosTributo.Sum(o => o.valor) - Convert.ToDecimal(tributos_nf["VALOR"]);
                                lanctosTributo.Last().valorBruto = lanctosTributo.Last().valor;
                                lanctosTributo.Last().valorUnit = lanctosTributo.Last().valor;

                                List_SLancamento.AddRange(lanctosTributo);
                            }
                        }

                    }
                }
            } //foreach Tributos_NF
            #endregion Tributos NF

            #region Vencimentos_NF

            if (Valor_Lote != 0)
            {
                DataTable Parametros_Global = contabilizacaoDAO.Busca_Parametros_Global(cod_emitente);
                cod_conta = Convert.ToString(Parametros_Global.Rows[0]["COD_CONTA_DIFERENCA"]);
                DataTable Dados_Conta = contabilizacaoDAO.Busca_Dados_Conta(cod_conta);

                if (Dados_Conta.Rows.Count > 0 && cod_conta != "0")
                {
                    if (Dados_Job.Rows.Count > 0)
                    {
                        lancamento.seqLote = 1;
                        lancamento.terceiro = Convert.ToInt32(Dados_NF.Rows[0]["COD_TOMADOR"]);
                        lancamento.cliente = Convert.ToInt32(Dados_Job.Rows[0]["COD_CLIENTE"]);
                        lancamento.descCliente = Convert.ToString(Dados_Job.Rows[0]["NOME_RAZAO_SOCIAL"]);
                        lancamento.codConsultor = Convert.ToInt32(Dados_NF.Rows[0]["COD_EMITENTE"]);
                        lancamento.conta = Convert.ToString(Parametros_Global.Rows[0]["COD_CONTA_DIFERENCA"]);
                        lancamento.dataLancamento = Convert.ToDateTime(Dados_NF.Rows[0]["DATA_EMISSAO_RPS"]);

                        if (Valor_Lote < 0)
                            lancamento.debCred = 'D';
                        else
                            lancamento.debCred = 'C';

                        lancamento.descTerceiro = Busca_Nome_Empresa(Convert.ToInt32(Dados_NF.Rows[0]["COD_TOMADOR"]));
                        lancamento.descConsultor = Convert.ToString(Dados_NF.Rows[0]["NOME_RAZAO_SOCIAL_EMITENTE"]);
                        lancamento.descConta = Convert.ToString(Dados_Conta.Rows[0]["DESCRICAO"]);
                        lancamento.descDivisao = Convert.ToString(Dados_Job.Rows[0]["DESC_DIVISAO"]);
                        lancamento.divisao = Convert.ToInt32(Dados_Job.Rows[0]["COD_DIVISAO"]);
                        lancamento.descJob = Convert.ToString(Dados_Job.Rows[0]["DESCRICAO"]);
                        lancamento.descLinhaNegocio = Convert.ToString(Dados_Job.Rows[0]["DESC_LINHA"]);
                        lancamento.linhaNegocio = Convert.ToInt32(Dados_Job.Rows[0]["COD_LINHA_NEGOCIO"]);
                        lancamento.historico = Parametros_Global.Rows[0]["HISTORICO"].ToString() + " - " + Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]) + " - " + Convert.ToString(Dados_Job.Rows[0]["NOME_RAZAO_SOCIAL"]);
                        lancamento.modulo = "CAR_INCLUSAO_TITULO";
                        lancamento.numeroDocumento = string.Concat("RPS-", Convert.ToString(Dados_NF.Rows[0]["NUMERO_RPS"]));
                        lancamento.qtd = 1;
                        lancamento.valorBruto = Convert.ToDecimal(Dados_NF.Rows[0]["VALOR_SERVICOS"]);
                        lancamento.job = Convert.ToInt32(Dados_Job.Rows[0]["COD_JOB"]);
                        lancamento.modelo = 0;
                        lancamento.codMoeda = 0;

                        if (Valor_Lote < 0)
                        {
                            lancamento.valorUnit = -Valor_Lote;
                            lancamento.valor = -Valor_Lote;
                            lancamento.debCred = 'D';
                        }
                        else
                        {
                            lancamento.valorUnit = Valor_Lote;
                            lancamento.valor = Valor_Lote;
                            lancamento.debCred = 'C';
                        }

                        if (Convert.ToBoolean(Parametros_Global.Rows[0]["GERA_TITULO"]) == true)
                        {
                            lancamento.titulo = true;

                            foreach (DataRow vencimentos_nf in Vencimentos_NF.Rows)
                            {
                                if (lancamento.vencimentos == null)
                                    lancamento.vencimentos = new List<SVencimento>();

                                vencimento.data = Convert.ToDateTime(vencimentos_nf["DATA_VENCIMENTO"]);
                                vencimento.valor = Convert.ToDecimal(vencimentos_nf["VALOR"]);
                                lancamento.vencimentos.Add(vencimento);
                                vencimento = new SVencimento();
                            }
                        }
                        else
                            lancamento.titulo = false;

                        List_SLancamento.Add(lancamento);
                        lancamento = new SLancamento();
                    }
                }
            }
            #endregion Vencimentos NF

            #region Salva Contabilização

            Conexao _conn = new Conexao();
            FolhaLancamento _FolhaLancamento = new FolhaLancamento(_conn, List_SLancamento, "CAR_INCLUSAO_TITULO", Convert.ToDateTime(Dados_NF.Rows[0]["DATA_EMISSAO_RPS"]));
            erros = _FolhaLancamento.salvar(null);
            Lote = _FolhaLancamento.lote;

            #endregion Salva Contabilização
        }
        if (erros.Count == 0)
        {
            emissao_nf_DAO.Adiciona_Lote_NF(cod_faturamento_nf, Lote);
            return "";
        }
        else
        {
            return string.Join("\\n", erros);
        }
    }

    public string Busca_Nome_Empresa(int cod_empresa)
    {
        return contabilizacaoDAO.Busca_Nome_Empresa(cod_empresa);
    }

    public List<string> Atualiza_Lote_NF(List<EmissaoNF> List_EmissaoNF)
    {
        List<string> erros = new List<string>();

        foreach (EmissaoNF item in List_EmissaoNF)
        {
            int num_Lote = emissao_nf_DAO.Atualiza_Lote_NF(item.numero_rps, item.data_emissao_rps, item.cpf_cnpj_emitente, item.numero_nf);
            if (num_Lote == 0)
                erros.Add("Erro ao atualizar a contabilização da Nota Fiscal, referente ao Número do RPS " + item.numero_rps);

            //Atualiza_Lote_NF(num_Lote, item.numero_nf);
        }

        return erros;
    }

}