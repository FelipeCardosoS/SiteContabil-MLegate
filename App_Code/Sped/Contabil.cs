using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Data.OleDb;


/// <summary>
/// Summary description for Contabil
/// </summary>
/// 
namespace Sped
{
    public class Contabil : AbstractGeracaoSped
    {
        private string tipoEscrituracao;
        private int numeroOrdem = 0;
        private DateTime dataArquivamento;
        int contadorArquivo = 0;

        public StreamWriter writer = null;
        protected string _caminho = "";
        public Conexao _conn;

        string _nire = "";

        public Contabil(DateTime inicio, DateTime termino, string tipoEscrit, int numOrdem, 
            DateTime dataArquiv, int codEmpresa, Conexao conn, string TipoDemonstracao, bool LucroPresumido, bool DetCentroCusto)
            :base(inicio, termino, codEmpresa, conn, LucroPresumido, DetCentroCusto)
        {
            _caminho = HttpContext.Current.Request.PhysicalApplicationPath + "Speds\\SPEEDS - " + DateTime.Now.ToString("dd - MM") + " - Nome " + HttpContext.Current.Session["nome_empresa"].ToString().Replace("/","") + ".txt";

            conn = new Conexao();
            conn.open();
            _TipoDemonstracao = TipoDemonstracao;

            _conn = conn;
            _conn.open();

            tipoEscrituracao = tipoEscrit;
            numeroOrdem = numOrdem;
            dataArquivamento = dataArquiv;

            using (writer = new StreamWriter(_caminho, false, System.Text.Encoding.Default))
            {
                empresasDAO empresaDAO = new empresasDAO(conn);
                DataTable tbEmpresa = empresaDAO.loadSped(_codEmpresa);
                if (tbEmpresa.Rows.Count == 0)
                {
                    throw new ApplicationException("Nenhuma empresa encontrada.");
                }

                rowEmpresa = tbEmpresa.Rows[0];
                executaBlocos();
            }
        }

        public override string bloco0()
        {
            StringBuilder txt = new StringBuilder();
            DataTable tbDados = new DataTable("dados");
            int contador = 0;
            int contadorIndividual = 0;

            txt.Append("|0000|");                                                           //01 - reg
            txt.Append("LECD|");                                                            //02 - lecd
            txt.Append("" + formatData(_inicio) + "|");                                     //03 - data inicio
            txt.Append("" + formatData(_termino) + "|");                                    //04 - data fim
            txt.Append("" + rowEmpresa["NOME_RAZAO_SOCIAL"] + "|");                         //05 - nome
            txt.Append("" + clearTxt(rowEmpresa["CNPJ_CPF"].ToString()) + "|");             //06 - cnpj
            txt.Append("" + rowEmpresa["UF"] + "|");                                        //07 - uf
            txt.Append("" + rowEmpresa["IE_RG"] + "|");                                     //08 - insc estadual
            txt.Append("" + rowEmpresa["COD_MUNICIPIO"].ToString().PadLeft(7, '0') + "|");  //09 - cod_municipio
            txt.Append("" + rowEmpresa["IM"] + "|");                                        //10 - IM
            txt.Append("|");                                                                //11 - ind_sit_esp
            txt.Append("0|");                                                               //12 - ind_sit_ini_per
            txt.Append("1|");                                                               //13 - ind_nire
            txt.Append("0|");                                                               //14 - ind_fin_esc
            txt.Append("|");                                                                //15 - Hash
            txt.Append("0|");                                                               //16 - ind_grande_porte
            txt.Append("0|");                                                               //17 - Tip_ECD
            txt.Append("|");                                                                //18 - Cod_SCP
            txt.Append("N|");                                                               //19 - Ident. Moeda Funcional
            txt.Append("N|");                                                               //20 - Ind Escrituracao Consolidada
            if (inicio.Year >= 2019)
            {
                txt.Append("0|");                                                               //21 - Indicador da modalidade de escrituração centralizada
                txt.Append("0|");                                                               //22 - Indicador de mudança de plano de contas
                if (LucroPresumido)
                {
                    txt.Append("2|");                                                               //23 - Código do Plano de Contas Referencial que será utilizado para o mapeamento de todas as contas analíticas
                }
                else
                {
                    txt.Append("1|");                                                               //23 - Código do Plano de Contas Referencial que será utilizado para o mapeamento de todas as contas analíticas
                }
            }


            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("0000", contadorIndividual));

            contadorIndividual = 0;
            txt.Append("|0001|");
            txt.Append("0|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("0001", contadorIndividual));

            contadorIndividual = 0;
            txt.Append("|0007|");
            txt.Append("SP|");
            txt.Append("|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("0007", contadorIndividual));

            contadorIndividual = 0;
            txt.Append("|0150|");
            //codigo 
            txt.Append(rowEmpresa["COD_EMPRESA"] + "|");
            //nome 
            txt.Append(rowEmpresa["NOME_RAZAO_SOCIAL"].ToString().PadRight(100, ' ').Substring(0, 100) + "|");
            //codigo país
            txt.Append(rowEmpresa["COD_PAIS"].ToString().PadLeft(5, '0') + "|");
            //cnpj
            txt.Append(clearTxt((rowEmpresa["FISICA_JURIDICA"].ToString() == "JURIDICA" ? rowEmpresa["CNPJ_CPF"].ToString() : "")) + "|");
            //CPF
            txt.Append(clearTxt((rowEmpresa["FISICA_JURIDICA"].ToString() == "FISICA" ? rowEmpresa["CNPJ_CPF"].ToString() : "")) + "|");
            //nit
            txt.Append("|");
            //uf
            txt.Append(rowEmpresa["UF"] + "|");
            //ie do estabelecimento
            txt.Append("|");
            //txt.Append(clearTxt((rowEmpresa["IE_RG"].ToString() == "" ? "" : (rowEmpresa["IE_RG"].ToString() == "1" ? "" : rowEmpresa["IE_RG"].ToString().Replace("ISENTO", "")))).PadRight(14, ' ').Substring(0, 14).Trim() + "|");
            //ie_st
            txt.Append("|");
            //codigo do municipio
            txt.Append(rowEmpresa["COD_MUNICIPIO"].ToString().PadLeft(7, '0') + "|");
            //im
            txt.Append("" + rowEmpresa["IM"] + "|");
            //suframa 
            txt.Append(rowEmpresa["SUFRAMA"] + "|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("0150", contadorIndividual));

            contadorIndividual = 0;
            txt.Append("|0180|");
            txt.Append("04|");
            txt.Append("01012000|");
            txt.Append("|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("0180", contadorIndividual));

            //ENCERRAMENTO DO BLOCO 0
            contadorIndividual = 0;
            txt.Append("|0990|");
            txt.Append((contador + 1) + "|");
            txt.Append("\r\n");
            contadorIndividual++;
            _blocos.Add(new BlocoSped("0990", contadorIndividual));
            return txt.ToString();
        }

        public string blocoI()
        {
            int contador = 0;
            int contadorIndividual = 0;

            StringBuilder txt = new StringBuilder();

            //ABERTURA
            txt.Append("|I001|0|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("I001", contadorIndividual));


            contadorIndividual = 0;
            if (inicio.Year >= 2020)
            {
                txt.Append("|I010|" + tipoEscrituracao + "|9.00|");
            }
            else if (inicio.Year == 2019)
            {
                txt.Append("|I010|" + tipoEscrituracao + "|8.00|");
            }
            else
            {
                txt.Append("|I010|" + tipoEscrituracao + "|7.00|");
            }
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("I010", contadorIndividual));


            contadorIndividual = 0;
            txt.Append("|I030|");                                                       // campo 1 - I030
            txt.Append("TERMO DE ABERTURA|");                                           // campo 2 - Termo de Abertura
            txt.Append(numeroOrdem + "|");                                              // campo 3 - Ordem 
            txt.Append("Diario Geral|");                                                // campo 4 - Natureza Escrituracao
            txt.Append("#QTDLINHAS#|");                                                 // campo 5 - Numero de Linhas
            txt.Append(rowEmpresa["NOME_RAZAO_SOCIAL"] + "|");                          // campo 6 - Nome
            txt.Append(rowEmpresa["NIRE"].ToString() + "|");                            // campo 7 - Nire
            txt.Append(rowEmpresa["CNPJ_CPF"] + "|");                                   // campo 8 - CNPJ
            txt.Append(formatData(dataArquivamento) + "|");                             // Campo 9 - Data Arquivamento
            txt.Append("|");                                                            // campo 10 - Data arq. conv
            txt.Append("|");                                                            // campo 11 - desc municipio
            txt.Append(termino.ToString("ddMMyyyy")+"|");                               // campo 12 - Data Encerramento
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("I030", contadorIndividual));

            //PLANO DE CONTAS
            contadorIndividual = 0;
            int contadorRef = 0;
            int contadorI052 = 0;
            contasDAO contaDAO = new contasDAO(_conn);
            contasRefDAO contaRefDAO = new contasRefDAO(_conn);
            DataTable tbContas  = contaDAO.listaSpedContabil(SessionView.EmpresaSession);
            
            foreach (DataRow row in tbContas.Rows)
            {
                string analitica = row["analitica_sintetica"].ToString();
                string codigoConta = row["cod_conta"].ToString();
                int nivel = 0;
                    encontraNivel(codigoConta, ref nivel, tbContas);
                
                if (nivel == 0)
                    nivel = 1;
                txt.Append("|I050|");
                txt.Append(formatData(_inicio) + "|");
                txt.Append(getNaturezaConta(row["grupo"].ToString()) + "|");
                txt.Append(row["analitica_sintetica"].ToString() + "|");
                txt.Append(nivel.ToString() + "|");
                txt.Append(row["cod_conta"].ToString() + "|");
                txt.Append(row["cod_conta_sintetica"].ToString() + "|");
                txt.Append(row["descricao"].ToString() + "|");
                txt.Append("\r\n");
                contador++;
                contadorIndividual++;

                //CONTA REFERENCIAL
                if(analitica == "A")
                {
                    if (row["cod_conta_ref"] != DBNull.Value)
                    {
                        if (row["cod_conta_ref"] != "")
                        {
                            SContaRef contaRef = contaRefDAO.load(row["cod_conta_ref"].ToString());
                            txt.Append("|I051|");
                            if (inicio.Year < 2019)
                            {
                                txt.Append("1|");
                            }
                            txt.Append("|");
                            txt.Append(contaRef.codigoRef + "|");
                            txt.Append("\r\n");
                            contadorRef++;
                            contador++;
                        }
                    }
                }
                if (row["Cod_Balanco"].ToString().Trim() != "" && row["analitica_sintetica"].ToString() == "A")
                {
                    txt.Append("|I052|");                                       // 01 - Registro
                    txt.Append("|");                                            // 02 - Centro Custo
                    txt.Append(row["Cod_Balanco"].ToString().Trim() + "|");      // 03 - Codigo Agrutinador
                    txt.Append("\r\n");
                    contadorI052++;
                    contador++;
                }
                //for (int i = 0; i < row["Cod_Balanco"].ToString().Length; i++)
                //{
                //    DataTable tbI052 = contaDAO.listaSpedI052Balanco(SessionView.EmpresaSession, row["Cod_Balanco"].ToString().Substring(0, row["Cod_Balanco"].ToString().Length - i));
                //    foreach (DataRow rowI052 in tbI052.Rows)
                //    {
                //        if (rowI052["codigo"].ToString() != "")
                //        {
                //            txt.Append("|I052|");                                       // 01 - Registro
                //            txt.Append("|");                                            // 02 - Centro Custo
                //            txt.Append(rowI052["codigo"].ToString().Trim() + "|");      // 03 - Codigo Agrutinador
                //            txt.Append("\r\n");
                //            contadorI052++;
                //            contador++;
                //        }
                //    }
                //}
                if (row["COD_DRE"].ToString().Trim() != "" && row["analitica_sintetica"].ToString() == "A")
                {
                    txt.Append("|I052|");                                       // 01 - Registro
                    txt.Append("|");                                            // 02 - Centro Custo
                    txt.Append(row["COD_DRE"].ToString().Trim() + "|");      // 03 - Codigo Agrutinador
                    txt.Append("\r\n");
                    contadorI052++;
                    contador++;
                }
                //for (int i = 0; i < row["COD_DRE"].ToString().Length; i++)
                //{
                //    DataTable tbI052 = contaDAO.listaSpedI052DRE(SessionView.EmpresaSession, row["COD_DRE"].ToString().Substring(0, row["COD_DRE"].ToString().Length - i));
                //    foreach (DataRow rowI052 in tbI052.Rows)
                //    {
                //        if (rowI052["codigo"].ToString() != "")
                //        {
                //            txt.Append("|I052|");                                       // 01 - Registro
                //            txt.Append("|");                                            // 02 - Centro Custo
                //            txt.Append(rowI052["codigo"].ToString().Trim() + "|");      // 03 - Codigo Agrutinador
                //            txt.Append("\r\n");
                //            contadorI052++;
                //            contador++;
                //        }
                //    }
                //}
            }
            writer.Write(txt.ToString());
            txt = new StringBuilder();
            _blocos.Add(new BlocoSped("I050", contadorIndividual));
            _blocos.Add(new BlocoSped("I051", contadorRef));
            _blocos.Add(new BlocoSped("I052", contadorI052));

            //CENTRO DE CUSTOS
            contadorIndividual = 0;
            Job job = new Job(_conn);
            DataTable tbJobs = new DataTable("jobs");
            job.lista(ref tbJobs, 'A');
            foreach(DataRow row  in tbJobs.Rows)    
            {
                txt.Append("|I100|");
                txt.Append(formatData(_inicio) + "|");
                txt.Append(row["COD_JOB"] + "|");
                txt.Append(row["DESCRICAO"] + "|");
                txt.Append("\r\n");
                contadorIndividual++;
                contador++;
            }
            writer.Write(txt.ToString());
            txt = new StringBuilder();
            _blocos.Add(new BlocoSped("I100", contadorIndividual));


            //SALDOS PERIÓDICOS – IDENTIFICAÇÃO DO PERÍODO
            contadorIndividual = 0;
            int contadorBalancete = 0;
            DateTime periodo = _inicio;
            relatoriosDAO relatoriosDAO = new relatoriosDAO(_conn);

            while (periodo <= _termino)
            {
                
                txt.Append("|I150|");
                txt.Append(formatData(periodo) + "|");
                txt.Append(formatData(periodo.AddMonths(1).AddDays(-1)) + "|");
                txt.Append("\r\n");
                
				
                //DETALHE DOS SALDOS PERIÓDICOS
                DataTable tb = new DataTable("balancete");
                if (_DetCentroCusto)
                {
                    relatoriosDAO.balancete(0, periodo, null, null, null, null, null, null,
                        null, null, "JOB", null, null, null, null, ref tb, true, Convert.ToString(SessionView.EmpresaSession), "", true);
                }
                else
                {
                    relatoriosDAO.balancete(0, periodo, null, null, null, null, null, null,
                        null, null, null, null, null, null, null, ref tb, true, Convert.ToString(SessionView.EmpresaSession), "", true);
                }
                double totalDebito = 0;
                double totalCredito = 0;

                foreach (DataRow row in tb.Rows)
                {
                    int analitica = 0;
                    int.TryParse(row["ANALITICA"].ToString(), out analitica);
                    if (analitica == 1)
                    {
                        double saldoInicial = 0;
                        double saldoFinal = 0;
                        double debito = 0;
                        double credito = 0;

                        double.TryParse(row["saldo_ini"].ToString(), out saldoInicial);
                        double.TryParse(row["saldo_fim"].ToString(), out saldoFinal);
                        double.TryParse(row["debito"].ToString(), out debito);
                        double.TryParse(row["credito"].ToString(), out credito);

                        if (!(saldoInicial == 0 && saldoFinal == 0 && debito == 0 && credito == 0))
                        {
                            Boolean Vfaz = true;
                            Boolean Detalhar = true;
                            if (!_DetCentroCusto)
                            {
                                Detalhar = false;
                            }
                            
                            if (row["COD_CONTA"].ToString().Substring(0,1) == "1" || row["COD_CONTA"].ToString().Substring(0, 1) == "2")
                            {
                                Detalhar = false;
                                try
                                {
                                    if (row["codDetalhe1"].ToString().Trim() != "0")
                                    {
                                        Vfaz = false;
                                    }
                                }
                                catch
                                { }
                            }
                            else
                            {
                                try
                                {
                                    if (row["codDetalhe1"].ToString().Trim() == "0")
                                    {
                                        Vfaz = false;
                                    }
                                }
                                catch
                                { }
                            }
                            
                            if (Vfaz)
                            {
                                txt.Append("|I155|");
                                txt.Append(row["COD_CONTA"] + "|");
                                if (Detalhar)
                                {
                                    txt.Append(row["codDetalhe1"] + "|");
                                }
                                else
                                {
                                    txt.Append("|");
                                }
                                txt.Append(formatNumero(saldoInicial) + "|");
                                txt.Append((saldoInicial > 0 ? "D" : "C") + "|");
                                txt.Append(formatNumero(debito) + "|");
                                txt.Append(formatNumero(credito) + "|");
                                txt.Append(formatNumero(saldoFinal) + "|");
                                txt.Append((saldoFinal > 0 ? "D" : "C") + "|");
                                txt.Append("\r\n");
                                contadorBalancete++;
                                contador++;

                                totalCredito += (saldoFinal > 0 ? 0 : saldoFinal);
                                totalDebito += (saldoFinal > 0 ? saldoFinal : 0);
                            }
                        }
                    }
                }

                totalCredito = Math.Round(totalCredito, 2);
                totalDebito = Math.Round(totalDebito, 2);

                periodo = periodo.AddMonths(1);
                contadorIndividual++;
                contador++;
            }
            writer.Write(txt.ToString());
            txt = new StringBuilder();
            _blocos.Add(new BlocoSped("I150", contadorIndividual));
            _blocos.Add(new BlocoSped("I155", contadorBalancete));


            //Lançamentos Contábeis
            contadorIndividual = 0;
            int numeroLancto = 1;
            int contadorDetalhe = 0;
            lanctosContabDAO lanctosDAO = new lanctosContabDAO(_conn);
            DataTable totaisDia = lanctosDAO.listTotalDia(_inicio, _termino, _codEmpresa);
            foreach (DataRow rowTotal in totaisDia.Rows)
            {
                DateTime data = new DateTime();
                double total = 0;
                DateTime.TryParse(rowTotal["data"].ToString(), out data);
                double.TryParse(rowTotal["total"].ToString(), out total);

                txt.Append("|I200|");
                txt.Append(rowTotal["LOTE"].ToString() + "|");
                txt.Append(formatData( data) + "|");
                txt.Append(formatNumero(total) + "|");
                txt.Append(rowTotal["TIPO_LANCTO"].ToString() + "||");
                txt.Append("\r\n");

                DataTable detalheDia = lanctosDAO.listLanctosDia(data, rowTotal["TIPO_LANCTO"].ToString(), _codEmpresa, rowTotal["LOTE"].ToString());
                System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(@"(\r|\n|\t)", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.ExplicitCapture);
                foreach (DataRow rowDetalhe in detalheDia.Rows)
                {
                    double valor = 0;
                    double.TryParse(rowDetalhe["VALOR"].ToString(), out valor);
                    string historico = rowDetalhe["HISTORICO"].ToString();
                    string numeroDocumento = rowDetalhe["NUMERO_DOCUMENTO"].ToString();

                    if (historico.Contains("Fatura"))
                    {
                        string t = "";
                    }
                    txt.Append("|I250|");
                    txt.Append(rowDetalhe["COD_CONTA"] + "|");
                    if (_DetCentroCusto && rowDetalhe["resultado"].ToString().Trim() == "True")
                    {
                        txt.Append(rowDetalhe["COD_JOB"] + "|");
                    }
                    else
                    {
                        txt.Append("|");
                    }
                    txt.Append(formatNumero( valor) + "|");
                    txt.Append(rowDetalhe["DEB_CRED"] + "|");
                    txt.Append(rx.Replace(numeroDocumento, "").Replace("|", "") + "|");
                    txt.Append("|");
                    txt.Append(rx.Replace(historico, "").Replace("|","") + "|");
                    txt.Append(rowEmpresa["COD_EMPRESA"] + "|");
                    txt.Append("\r\n");
                    contadorDetalhe++;
                    contador++;
                }

                numeroLancto++;
                contadorIndividual++;
                contador++;
                writer.Write(txt.ToString());
                txt = new StringBuilder();
            }

            _blocos.Add(new BlocoSped("I200", contadorIndividual));
            _blocos.Add(new BlocoSped("I250", contadorDetalhe));

            //ENCERRAMENTOS
            contadorIndividual = 0;

            controleEncerramentoDAO encerramentoDAO = new controleEncerramentoDAO(_conn);
            DataTable tbEncerramentos = encerramentoDAO.listOfPeriodo(_inicio, _termino, _codEmpresa);
            int totalEncerramentos = 0;
            foreach (DataRow rowEncerramento in tbEncerramentos.Rows)
            {
                DateTime dataEncerramento = new DateTime();
                DateTime.TryParse(rowEncerramento["periodo"].ToString(), out dataEncerramento);
                txt.Append("|I350|");
                txt.Append(formatData(dataEncerramento) + "|");
                contadorIndividual++;
                contador++;
                txt.Append("\r\n");

                DataTable tb = new DataTable("dados");
                if (_DetCentroCusto)
                {
                    relatoriosDAO.balanceteResultado(dataEncerramento, null, null, null, null, null, null,
                        null, null, "JOB", null, null, null, null, ref tb, false);
                }
                else
                {
                    relatoriosDAO.balanceteResultado(dataEncerramento, null, null, null, null, null, null,
                        null, null, null, null, null, null, null, ref tb, false);
                }
                foreach (DataRow rowEnc in tb.Rows)
                {
                    int analitica = 0;
                    int.TryParse(rowEnc["ANALITICA"].ToString(), out analitica);

                    if (analitica == 1)
                    {
                        string codigoConta = rowEnc["COD_CONTA"].ToString();
                        double saldoFim = 0;
                        double.TryParse(rowEnc["SALDO_FIM"].ToString(), out saldoFim);
                        if ((_DetCentroCusto && rowEnc["codDetalhe1"].ToString().Trim() != "0") || !_DetCentroCusto)
                        {
                            txt.Append("|I355|");
                            txt.Append(codigoConta + "|");
                            if (_DetCentroCusto)
                            {
                                txt.Append(rowEnc["codDetalhe1"].ToString() + "|");
                            }
                            else
                            {
                                txt.Append("|");
                            }
                            txt.Append(formatNumero(saldoFim) + "|");
                            txt.Append((saldoFim > 0 ? "D" : "C") + "|");
                            totalEncerramentos++;
                            contador++;
                            txt.Append("\r\n");
                        }
                    }
                }
            }

            _blocos.Add(new BlocoSped("I350", contadorIndividual));
            _blocos.Add(new BlocoSped("I355", totalEncerramentos));

            //ENCERRAMENTO DO BLOCO 0
            contadorIndividual = 0;
            txt.Append("|I990|");
            txt.Append((contador + 1) + "|");
            txt.Append("\r\n");
            contadorIndividual++;
            _blocos.Add(new BlocoSped("I990", contadorIndividual));
            writer.Write(txt.ToString());
            txt = new StringBuilder();

            return txt.ToString();
        }

        public string blocoJ()
        {
            StringBuilder txt = new StringBuilder();
            DataTable tbDados = new DataTable("dados");
            int contadorIndividual = 0;
            int contador = 0;
            
            txt.Append("|J001|0|");
            txt.Append("\r\n");
            contadorIndividual++;
            contador++;
            _blocos.Add(new BlocoSped("J001", contadorIndividual));

            int periodos = 12;
            
            if (_TipoDemonstracao == "Trimestral")
            {
                periodos = 3;
            }
            if (_TipoDemonstracao == "Mensal")
            {
                periodos = 1;
            }
            DateTime DateFimDemonstracao;

            DateFimDemonstracao = _inicio.AddMonths(periodos);
            DateFimDemonstracao = DateFimDemonstracao.AddDays(-1);
            if (DateFimDemonstracao > _termino)
            {
                DateFimDemonstracao = _termino;
            }

            int contadorBalanco = 0;
            int contadorDRE = 0;
            int contadorj005 = 0;
            while (DateFimDemonstracao <= _termino)
            {



                contadorIndividual = 0;
                txt.Append("|J005|");
                txt.Append(formatData(_inicio) + "|");
                txt.Append(formatData(DateFimDemonstracao) + "|");
                txt.Append("1|");
                txt.Append("|");
                txt.Append("\r\n");
                contadorIndividual++;
                contadorj005++;
                contador++;
                

                contadorIndividual = 0;
                

                #region Balanco
                contasDAO contaDAO = new contasDAO(_conn);
                DataTable Balanco = contaDAO.MontaBalanco(SessionView.EmpresaSession, _inicio, DateFimDemonstracao);
                foreach (DataRow rowBalanco in Balanco.Rows)
                {
                    txt.Append("|J100|");                                           /// 01 - Fixo
                    txt.Append(rowBalanco["codigo"].ToString().Trim() + "|");       /// 02 - Codigo Aglutinacao
                    txt.Append(rowBalanco["IND_COD_AGL"].ToString().Trim() + "|");  /// 03 - Indicador Codigo Aglutinacao
                    txt.Append(rowBalanco["nivel"].ToString().Trim() + "|");        /// 04 - Nivel
                    txt.Append(rowBalanco["superior"].ToString().Trim() + "|");     /// 05 - Cod Aglut. Superior
                    if (rowBalanco["ativo_passivo"].ToString().Trim() == "A")
                    {
                        txt.Append("A|");                                           /// 06 - 1 - Ativo / 2 - Passivo
                    }
                    else
                    {
                        txt.Append("P|");                                           /// 06 - 1 - Ativo / 2 - Passivo
                    }
                    txt.Append(rowBalanco["descricao"].ToString().Trim() + "|");    /// 07 - Descricao
                    decimal movimento = Convert.ToDecimal(rowBalanco["Anterior"].ToString());
                    if (movimento > 0)
                    {
                        txt.Append(movimento.ToString().Replace(".", ",") + "|");        /// 08 Valor
                        txt.Append("C|");                                               /// 09 Indicador Valor
                    }
                    else
                    {
                        txt.Append((movimento * -1).ToString().Replace(".", ",") + "|");        /// 08 Valor
                        txt.Append("D|");                                                     /// 09 Indicador Valor
                    }
                    movimento = Convert.ToDecimal(rowBalanco["Movimento"].ToString());
                    if (movimento > 0)
                    {
                        txt.Append(movimento.ToString().Replace(".", ",") + "|");           /// 10 Valor
                        txt.Append("C|");                                                    /// 11 Indicador Valor
                    }
                    else
                    {
                        txt.Append((movimento * -1).ToString().Replace(".", ",") + "|");        /// 10 Valor
                        txt.Append("D|");                                                       /// 11 Indicador Valor
                    }
                    txt.Append("|");
                    txt.Append("\r\n");
                    contadorBalanco++;
                    contador++;
                }


                #region Balancoantigo

                //foreach (DataRow rowBalanco in Balanco.Rows)
                //{
                //    txt.Append("|J100|");                                           /// 01 - Fixo
                //    txt.Append(rowBalanco["codigo"].ToString().Trim() + "|");       /// 02 - Codigo Aglutinacao
                //    txt.Append(rowBalanco["nivel"].ToString().Trim() + "|");        /// 03 - Nivel
                //    if (rowBalanco["ativo_passivo"].ToString().Trim() == "A")
                //    {
                //        txt.Append("1|");                                           /// 04 - 1 - Ativo / 2 - Passivo
                //    }
                //    else
                //    {
                //        txt.Append("2|");                                           /// 04 - 1 - Ativo / 2 - Passivo
                //    }
                //    txt.Append(rowBalanco["descricao"].ToString().Trim() + "|");        /// 05 - Descricao
                //    decimal movimento = Convert.ToDecimal(rowBalanco["Movimento"].ToString());
                //    if (movimento > 0)
                //    {
                //        txt.Append(movimento.ToString().Replace(".", ",") + "|");        /// 06 Valor
                //        txt.Append("C|");                                               /// 07 Indicador Valor
                //    }
                //    else
                //    {
                //        txt.Append((movimento * -1).ToString().Replace(".", ",") + "|");        /// 06 Valor
                //        txt.Append("D|");                                                     /// 07 Indicador Valor
                //    }
                //    movimento = Convert.ToDecimal(rowBalanco["Anterior"].ToString());
                //    if (movimento > 0)
                //    {
                //        txt.Append(movimento.ToString().Replace(".", ",") + "|");           /// 08 Valor
                //        txt.Append("C|");                                                    /// 09 Indicador Valor
                //    }
                //    else
                //    {
                //        txt.Append((movimento * -1).ToString().Replace(".", ",") + "|");        /// 08 Valor
                //        txt.Append("D|");                                                       /// 09 Indicador Valor
                //    }
                //    txt.Append("|");
                //    txt.Append("\r\n");
                //    contadorBalanco++;
                //    contador++;
                //}
                #endregion Balancoantigo

                #endregion Balanco

                #region DRE
                DataTable DRE = contaDAO.MontaDRE(SessionView.EmpresaSession, _inicio, _termino, DateFimDemonstracao, periodos);

                foreach (DataRow rowDRE in DRE.Rows)
                {
                    if (inicio.Year >= 2019)
                    {
                        txt.Append("|J150|");                                           /// 01 - Fixo
                        txt.Append(rowDRE["ordem"].ToString().Trim() + "|");            /// 02 - Ordem
                        txt.Append(rowDRE["codigo"].ToString().Trim() + "|");           /// 03 - Codigo Aglutinacao
                        txt.Append(rowDRE["IND_COD_AGL"].ToString().Trim() + "|");      /// 04 - IND_COD_AGL
                        txt.Append(rowDRE["nivel"].ToString().Trim() + "|");            /// 05 - Nivel
                        txt.Append(rowDRE["superior"].ToString().Trim() + "|");         /// 06 - Cod. Agl. Superior

                        txt.Append(rowDRE["descricao"].ToString().Trim() + "|");        /// 07 - Descricao
                        /////Inicio do Saldo Anterior
                        decimal Anterior = Convert.ToDecimal(rowDRE["Anterior"].ToString());
                        if (Anterior > 0)
                        {
                            txt.Append(Anterior.ToString().Replace(".", ",") + "|");    /// 08 Valor
                            txt.Append("C|");                                            /// 09 Indicador Valor

                        }
                        else
                        {
                            txt.Append((Anterior * -1).ToString().Replace(".", ",") + "|"); /// 08 Valor
                            txt.Append("D|");                                                /// 09 Indicador Valor
                        }
                        ///// Fim do Saldo Anterior
                        /// Inicio do Periodo
                        decimal movimento = Convert.ToDecimal(rowDRE["Movimento"].ToString());
                        if (movimento > 0)
                        {
                            txt.Append(movimento.ToString().Replace(".", ",") + "|");    /// 10 Valor
                            txt.Append("C|");                                            /// 11 Indicador Valor
                            txt.Append("R|");                                            /// 12 Indicador Valor

                        }
                        else
                        {
                            txt.Append((movimento * -1).ToString().Replace(".", ",") + "|"); /// 10 Valor
                            txt.Append("D|");                                                /// 11 Indicador Valor
                            txt.Append("D|");                                                /// 12 Indicador Valor
                        }

                        txt.Append("|");
                        txt.Append("\r\n");
                        contadorDRE++;
                        contador++;
                    }
                    else
                    {

                        txt.Append("|J150|");                                       /// 01 - Fixo
                        txt.Append(rowDRE["codigo"].ToString().Trim() + "|");       /// 02 - Codigo Aglutinacao
                        txt.Append(rowDRE["IND_COD_AGL"].ToString().Trim() + "|");  /// 03 - IND_COD_AGL
                        txt.Append(rowDRE["nivel"].ToString().Trim() + "|");        /// 04 - Nivel
                        txt.Append(rowDRE["superior"].ToString().Trim() + "|");     /// 05 - Cod. Agl. Superior

                        txt.Append(rowDRE["descricao"].ToString().Trim() + "|");        /// 06 - Descricao
                        decimal movimento = Convert.ToDecimal(rowDRE["Movimento"].ToString());
                        if (movimento > 0)
                        {
                            txt.Append(movimento.ToString().Replace(".", ",") + "|");    /// 07 Valor
                            txt.Append("C|");                                            /// 08 Indicador Valor
                            txt.Append("R|");                                            /// 09 Indicador Valor

                        }
                        else
                        {
                            txt.Append((movimento * -1).ToString().Replace(".", ",") + "|"); /// 07 Valor
                            txt.Append("D|");                                                /// 08 Indicador Valor
                            txt.Append("D|");                                                /// 09 Indicador Valor
                        }

                        txt.Append("|");
                        txt.Append("\r\n");
                        contadorDRE++;
                        contador++;
                    }
                }

                #region DREAnterior

                //foreach (DataRow rowDRE in DRE.Rows)
                //{
                //    txt.Append("|J150|");                                           /// 01 - Fixo
                //    txt.Append(rowDRE["codigo"].ToString().Trim() + "|");       /// 02 - Codigo Aglutinacao
                //    txt.Append(rowDRE["nivel"].ToString().Trim() + "|");        /// 03 - Nivel

                //    txt.Append(rowDRE["descricao"].ToString().Trim() + "|");        /// 04 - Descricao
                //    decimal movimento = Convert.ToDecimal(rowDRE["Movimento"].ToString());
                //    if (movimento > 0)
                //    {
                //        txt.Append(movimento.ToString().Replace(".", ",") + "|");        /// 05 Valor
                //        if (rowDRE["analitica"].ToString().Trim() == "A")
                //        {
                //            txt.Append("R|");                                               /// 06 Indicador Valor
                //        }
                //        else
                //        {
                //            txt.Append("P|");                                               /// 06 Indicador Valor
                //        }
                //    }
                //    else
                //    {
                //        txt.Append((movimento * -1).ToString().Replace(".", ",") + "|");        /// 06 Valor
                //        if (rowDRE["analitica"].ToString().Trim() == "A")
                //        {
                //            txt.Append("D|");                                               /// 06 Indicador Valor
                //        }
                //        else
                //        {
                //            txt.Append("N|");                                               /// 06 Indicador Valor
                //        }
                //    }
                //    movimento = Convert.ToDecimal(rowDRE["Anterior"].ToString());
                //    if (movimento > 0)
                //    {
                //        txt.Append(movimento.ToString().Replace(".", ",") + "|");        /// 07 Valor
                //        if (rowDRE["analitica"].ToString().Trim() == "A")
                //        {
                //            txt.Append("R|");                                               /// 08 Indicador Valor
                //        }
                //        else
                //        {
                //            txt.Append("P|");                                               /// 08 Indicador Valor
                //        }
                //    }
                //    else
                //    {
                //        txt.Append((movimento * -1).ToString().Replace(".", ",") + "|");        /// 07 Valor
                //        if (rowDRE["analitica"].ToString().Trim() == "A")
                //        {
                //            txt.Append("D|");                                               /// 08 Indicador Valor
                //        }
                //        else
                //        {
                //            txt.Append("N|");                                               /// 08 Indicador Valor
                //        }
                //    }
                //    txt.Append("|");
                //    txt.Append("\r\n");
                //    contadorDRE++;
                //    contador++;
                //}
                #endregion DREAnterior

                #endregion DRE
                DateFimDemonstracao = DateFimDemonstracao.AddDays(-DateFimDemonstracao.Day +1);
                DateFimDemonstracao = DateFimDemonstracao.AddMonths(periodos+1);
                DateFimDemonstracao = DateFimDemonstracao.AddDays(-1);
            }
            _blocos.Add(new BlocoSped("J005", contadorj005));
            _blocos.Add(new BlocoSped("J100", contadorBalanco));
            _blocos.Add(new BlocoSped("J150", contadorDRE));
           

            ////ABRE EXCEL
            //OleDbConnection con_excel = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + HttpRuntime.AppDomainAppPath + "Dre\\" + HttpContext.Current.Session["empresa"] + ".xls;Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\";");
            //con_excel.Open();
            //int contfim = 0;
            ////BALANÇO
            //contadorIndividual = 0;
            //int contadorBalanco = 0;
            //int contadorDRE = 0;
            //OleDbDataAdapter adapter;
            //DataSet ds = new DataSet();
            //try
            //{
            //    adapter = new OleDbDataAdapter("select * from [Ativo$]", con_excel);
            //    ds = new DataSet();
            //    adapter.Fill(ds);

            //    foreach (DataRow LinhaExcel in ds.Tables[0].Rows)
            //    {
            //        string codigoConta = LinhaExcel[0].ToString();

            //        if (codigoConta == "")
            //        {
            //            contfim++;
            //            if (contfim > 10)
            //            {
            //                break;
            //            }
            //        }
            //        else
            //        {
            //            int nivel = codigoConta.Length;
            //            int indicador = 1; //(ws.Name == "Ativo" ? 1 : 2);
            //            string descricaoConta = LinhaExcel[1].ToString();

            //            string valorStr;
            //            string valorAno;
            //            double valorAnoAnterior;

            //            try
            //            {
            //                valorStr = LinhaExcel[3].ToString();
            //                valorAno = LinhaExcel[5].ToString();
            //                valorAnoAnterior = Convert.ToDouble(valorAno);
            //            }
            //            catch { break; }

            //            double valor = 0;
            //            double.TryParse(valorStr, out valor);
            //            txt.Append("|J100|");
            //            txt.Append(codigoConta.Trim() + "|");
            //            txt.Append(nivel + "|");
            //            txt.Append(indicador + "|");
            //            txt.Append(descricaoConta.Trim() + "|");
            //            txt.Append(formatNumero(valor).Replace(".", ",") + "|");
            //            txt.Append((indicador == 1 ? (valor > 0 ? "D" : "C") : (valor > 0 ? "C" : "D")) + "|");
            //            txt.Append(formatNumero(valorAnoAnterior).Replace(".", ",") + "|");
            //            txt.Append((indicador == 1 ? (valorAnoAnterior > 0 ? "D" : "C") : (valorAnoAnterior > 0 ? "C" : "D")) + "|");
            //            txt.Append("\r\n");

            //            contador++;
            //            contadorBalanco++;
            //        }
            //    }
            //}
            //catch
            //{
            //}
            //#region passivo
            //try
            //{
            //    adapter = new OleDbDataAdapter("select * from [Passivo$]", con_excel);
            //    ds = new DataSet();
            //    adapter.Fill(ds);
            //    contfim = 0;
            //    foreach (DataRow LinhaExcel in ds.Tables[0].Rows)
            //    {
            //        string codigoConta = LinhaExcel[0].ToString();

            //        if (codigoConta == "")
            //        {
            //            contfim++;
            //            if (contfim > 10)
            //            {
            //                break;
            //            }
            //        }
            //        else
            //        {
            //            int nivel = codigoConta.Length;
            //            int indicador = 2; //(ws.Name == "Ativo" ? 1 : 2);
            //            string descricaoConta = LinhaExcel[1].ToString();

            //            string valorStr;
            //            string valorAno;
            //            double valorAnoAnterior;

            //            try
            //            {
            //                valorStr = LinhaExcel[3].ToString();
            //                valorAno = LinhaExcel[5].ToString();
            //                valorAnoAnterior = Convert.ToDouble(valorAno);
            //            }
            //            catch { break; }

            //            double valor = 0;
            //            double.TryParse(valorStr, out valor);
            //            txt.Append("|J100|");
            //            txt.Append(codigoConta.Trim() + "|");
            //            txt.Append(nivel + "|");
            //            txt.Append(indicador + "|");
            //            txt.Append(descricaoConta.Trim() + "|");
            //            txt.Append(formatNumero(valor).Replace(".", ",") + "|");
            //            txt.Append((indicador == 1 ? (valor > 0 ? "D" : "C") : (valor > 0 ? "C" : "D")) + "|");
            //            txt.Append(formatNumero(valorAnoAnterior).Replace(".", ",") + "|");
            //            txt.Append((indicador == 1 ? (valorAnoAnterior > 0 ? "D" : "C") : (valorAnoAnterior > 0 ? "C" : "D")) + "|");
            //            txt.Append("\r\n");

            //            contador++;
            //            contadorBalanco++;
            //        }
            //    }
            //}
            //catch
            //{

            //}
            //#endregion


            ////DRE
            //try
            //{
            //    adapter = new OleDbDataAdapter("select * from [DRE$]", con_excel);
            //    ds = new DataSet();
            //    adapter.Fill(ds);
            //    contfim = 0;
            //    foreach (DataRow LinhaExcel in ds.Tables[0].Rows)
            //    {
            //        string codigoConta = LinhaExcel[0].ToString();
            //        if (codigoConta == "")
            //        {
            //            contfim++;
            //            if (contfim > 10)
            //            {
            //                break;
            //            }
            //        }
            //        else
            //        {
            //            int nivel = codigoConta.Length;
            //            string descricaoConta = LinhaExcel[1].ToString();
            //            string valorStr = "";
            //            if (LinhaExcel[3].ToString() != "")
            //                valorStr = LinhaExcel[3].ToString();
            //            double valor = 0;
            //            double.TryParse(valorStr, out valor);
            //            valor = Math.Abs(valor);
            //            txt.Append("|J150|");
            //            txt.Append(codigoConta.Trim() + "|");
            //            txt.Append(nivel + "|");
            //            txt.Append(descricaoConta.Trim() + "|");
            //            txt.Append(formatNumero(valor).Replace(".", ",") + "|");
            //            txt.Append(LinhaExcel[4].ToString().Trim() + "|");
            //            if (LinhaExcel[5].ToString() != "")
            //            {
            //                valorStr = LinhaExcel[5].ToString();
            //            }
            //            double valorant = 0;
            //            double.TryParse(valorStr, out valorant);
            //            valorant = Math.Abs(valorant);
            //            txt.Append(formatNumero(valorant).Replace(".", ",") + "|");
            //            txt.Append(LinhaExcel[6].ToString().Trim() + "|");
            //            //txt.Append(ws.get_Range(ws.Cells[linha, 6], ws.Cells[linha, 6]).Text.ToString().Trim() + "|");
            //            txt.Append("\r\n");

            //            contador++;
            //            contadorDRE++;
            //        }
            //    }
            //}
            //catch
            //{

            //}
            //_blocos.Add(new BlocoSped("J100", contadorBalanco));
            //_blocos.Add(new BlocoSped("J150", contadorDRE));

            //TERMO DE ENCERRAMENTO
            contadorIndividual = 0;
            txt.Append("|J900|");
            txt.Append("TERMO DE ENCERRAMENTO|");
            txt.Append(numeroOrdem + "|");
            txt.Append("Diario Geral|");
            txt.Append(rowEmpresa["NOME_RAZAO_SOCIAL"] +  "|");
            txt.Append("#QTDLINHAS#|");
            txt.Append(formatData(_inicio) + "|");
            txt.Append(formatData(_termino) + "|");
            txt.Append("\r\n");
            contadorIndividual++;
            contador++;
            _blocos.Add(new BlocoSped("J900", contadorIndividual));


            //CONTADOR
            contadorIndividual = 0;
            contadorDAO contDAO = new contadorDAO(_conn);
            SContador cont = contDAO.load(SessionView.EmpresaSession);
            if (cont != null)
            {
                txt.Append("|J930|");                                                                               // 01 - Texto Fixo
                txt.Append(cont.nome + "|");                                                                        // 02 - Nome
                txt.Append(cont.cpf.Replace("-", "").Replace(".", "") + "|");                                       // 03 - CPF
                txt.Append("CONTADOR|");                                                                            // 04 - Identificacao do Assinante
                txt.Append(cont.cod_assin + "|");                                                                   // 05 - Codigo do Assinante
                txt.Append(cont.crc + "|");                                                                         // 06 - ind do CRC
                txt.Append(cont.email + "|");                                                                       // 07 - email
                txt.Append(cont.telefone.Replace(" ", "").Replace("-", "").Replace(")", "").Replace("(", "") + "|");// 08 - Fone
                txt.Append(cont.uf_crc + "|");                                                                      // 09 - UF CRC
                txt.Append(cont.num_seq_crc + "|");                                                                 // 10 - Num seq. CRC
                txt.Append(cont.dt_crc.ToString("ddMMyyyy") + "|");                                                 // 11 - Data Validade do Contador
                txt.Append("N|");                                                                                   // 12 - Identificador do Representante Legal
                contadorIndividual++;
                contador++;
                txt.Append("\r\n");
            }
            //RESPONSÁVEL
            responsavelDAO resDAO = new responsavelDAO(_conn);
            SResponsavel res = resDAO.load(SessionView.EmpresaSession);
            if (res != null)
            {
                txt.Append("|J930|");                                                                               // 01 - Texto Fixo
                txt.Append(res.nome + "|");                                                                         // 02 - Nome
                txt.Append(res.cpf.Replace("-", "").Replace(".", "") + "|");                                        // 03 - CPF
                txt.Append(res.ident_qualif + "|");                                                                 // 04 - Identificacao do Assinante
                txt.Append(res.cod_assin + "|");                                                                    // 05 - Codigo do Assinante
                txt.Append(res.crc + "|");                                                                          // 06 - ind do CRC
                txt.Append(res.email + "|");                                                                        // 07 - email
                txt.Append(res.telefone.Replace(" ", "").Replace("-", "").Replace(")", "").Replace("(", "") + "|"); // 08 - Fone
                txt.Append(res.uf_crc + "|");                                                                       // 09 - UF CRC
                txt.Append(res.num_seq_crc + "|");                                                                  // 10 - Num seq. CRC
                txt.Append(res.dt_crc.ToString("ddMMyyyy") + "|");                                                  // 11 - Data Validade do Contador
                txt.Append("S|");                                                                                   // 12 - Identificador do Representante Legal
                txt.Append("\r\n");                                                                                 
                contadorIndividual++;
                contador++;
            }
            _blocos.Add(new BlocoSped("J930", contadorIndividual));

            //ENCERRAMENTO DO BLOCO 0
            contadorIndividual = 0;
            txt.Append("|J990|");
            txt.Append((contador + 1) + "|");
            txt.Append("\r\n");
            contadorIndividual++;
            _blocos.Add(new BlocoSped("J990", contadorIndividual));
            //con_excel.Close();
            return txt.ToString();

        }

        private string getNaturezaConta(string descricaoNatureza)
        {
            if (descricaoNatureza.Contains("Patri"))
            {
                return "03";
            }
            switch (descricaoNatureza.ToUpper())
            {
                case "ATIVO":
                    return "01";
                case "PASSIVO":
                    return "02";
                case "PATRIMONIO LIQUIDO":
                    return "03";
                case "PATRIMôNIO LIQUIDO":
                    return "03";
                case "Patrimonio Liquido":
                    return "03";
                case "RESULTADO":
                    return "04";
                case "RECEITA":
                    return "04";
                case "DESPESA":
                    return "04";
                case "DESPESAS":
                    return "04";
                case "RECEITAS":
                    return "04";
                case "COMPENSACAO":
                    return "05";
                default:
                    switch (descricaoNatureza.ToUpper().Substring(0, 3))
                    {
                        case "ATI":
                            return "01";
                        case "PAS":
                            return "02";
                        case "PAT":
                            return "03";
                        case "RES":
                            return "04";
                        case "DES":
                            return "04";
                        case "REC":
                            return "04";
                        default:
                            return "09";
                    }
            }
        }

        public override string bloco9()
        {
            int contador = 0;
            int contadorIndividual = 0;

            StringBuilder txt = new StringBuilder();

            //ABERTURA
            txt.Append("|9001|0|");
            txt.Append("\r\n");
            contador++;
            _blocos.Add(new BlocoSped("9001", 1));

            //OCORRENCIAS
            contadorIndividual = 0;
            for (int i = 0; i < _blocos.Count; i++)
            {
                txt.Append("|9900|");
                txt.Append(_blocos[i].nome + "|");
                txt.Append(_blocos[i].totalLinhas + "|");
                txt.Append("\r\n");
                contador++;
                contadorIndividual++;
            }

            txt.Append("|9900|");
            txt.Append("9990|");
            txt.Append("1|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;

            txt.Append("|9900|");
            txt.Append("9999|");
            txt.Append("1|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;

            txt.Append("|9900|");
            txt.Append("9900|");
            txt.Append((contadorIndividual + 1) + "|");
            txt.Append("\r\n");
            _blocos.Add(new BlocoSped("9900", (contadorIndividual + 1)));
            contador++;

            //ENCERRAMENTO
            txt.Append("|9990|");
            txt.Append((contador + 2) + "|");
            txt.Append("\r\n");
            contador++;
            _blocos.Add(new BlocoSped("9990", 1));

            txt.Append("|9999|");
            int total = 0;
            for (int i = 0; i < _blocos.Count; i++)
            {
                total += _blocos[i].totalLinhas;
            }
            txt.Append((total + 1) + "|");
            txt.Append("\r\n");
            contadorArquivo = (total + 1);
            return txt.ToString();
        }

        public override void executaBlocos()
        {
            try { writer.Write(bloco0()); }
            catch (Exception err) { throw new Exception("Bloco 0 - "+err.Message); }
            try { blocoI();}
            catch (Exception err) { throw new Exception("Bloco I - "+err.Message); }
            try { writer.Write(blocoJ());}
            catch (Exception err) { throw new Exception("Bloco J - "+err.Message); }
            try { writer.Write(bloco9()); }
            catch (Exception err) { throw new Exception("Bloco 9 - " + err.Message); }


            writer.Flush();
            writer.Close();
            replaceTotalLinhas();
        }

        public void encontraNivel(string codigoConta, ref int nivel, DataTable tb)
        {
            foreach(DataRow row in tb.Rows)
            {
                string codigo = row["cod_conta"].ToString();
                if (codigo == codigoConta)
                {
                    nivel++;
                }   
            }

            if (codigoConta.Length > 1)
            {
                encontraNivel(codigoConta.Substring(0, codigoConta.Length - 1), ref nivel, tb);
            }
        }

        public void replaceTotalLinhas()
        {
            string nomeArquivo = System.IO.Path.GetFileNameWithoutExtension(_caminho);
            string extensao = System.IO.Path.GetExtension(_caminho);
            string pasta = System.IO.Path.GetDirectoryName(_caminho);
            string caminhoNovo = pasta + "\\" + nomeArquivo + "_cor" + extensao;
            using (writer = new System.IO.StreamWriter(caminhoNovo, false, System.Text.Encoding.Default))
            {
                using (System.IO.StreamReader leitor = new System.IO.StreamReader(_caminho, System.Text.Encoding.Default))
                {
                    while (leitor.Peek() > -1)
                    {
                        string linha = leitor.ReadLine();
                        linha = linha.Replace("#QTDLINHAS#", contadorArquivo.ToString());
                        writer.WriteLine(linha);
                    }
                }
            }
        }
    }
}