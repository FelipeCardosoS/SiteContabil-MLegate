using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using System.Text;

/// <summary>
/// Summary description for PisCofins
/// </summary>
/// 
namespace Sped{
    public class PisCofins : AbstractGeracaoSped
    {


        public PisCofins(DateTime inicio, DateTime termino, int codEmpresa, Conexao conn)
            : base(inicio, termino, codEmpresa, conn, false, false)
        {
        }

        public override void executaBlocos()
        {
            writer.Write(bloco0());
            writer.Write(blocoA());
            writer.Write(blocoC());
            writer.Write(blocoD());
            writer.Write(blocoF());
            writer.Write(blocoM());
            writer.Write(bloco1());
            writer.Write(bloco9());
        }

        public override string bloco0()
        {
            StringBuilder txt = new StringBuilder();

            txt.Append("|0000|");
            DataTable tbDados = new DataTable("dados");
            DataRow row = null;
            int contador = 0;
            int contadorIndividual = 0;

            //INICIO BLOCO 0
            //codigo versão
            txt.Append("002|");
            //tipo de escrituracao
            txt.Append("0|");
            //indicador de situação especial
            txt.Append("|");
            //escrituracao anterior
            txt.Append("|");
            //data inicio das informações
            txt.Append("" + formatData(_inicio) + "|");
            //data termino das informações
            txt.Append("" + formatData(_termino) + "|");

            //dados da empresa

            //nome da empresa
            txt.Append("" + rowEmpresa["NOME_RAZAO_SOCIAL"] + "|");
            //cnpj da empresa
            txt.Append("" + clearTxt(rowEmpresa["CNPJ_CPF"].ToString()) + "|");
            //uf da empresa;
            txt.Append("" + rowEmpresa["UF"] + "|");
            //codigo do municipio
            txt.Append("" + rowEmpresa["COD_MUNICIPIO"].ToString().PadLeft(7, '0') + "|");
            //suframa
            txt.Append("|");
            //natureza da pessoa juridica
            txt.Append("|");
            //tipo de atividade preponderante
            txt.Append("9|");

            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("0000", contadorIndividual));

            //ABERTURA BLOCO 0
            contadorIndividual = 0;
            txt.Append("|0001|");
            //indicador de movimento
            txt.Append("0|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("0001", contadorIndividual));

            //DADOS DO CONTABILISTA
            contadorIndividual = 0;
            txt.Append("|0100|");
            //nome do contabilista
            txt.Append("Antonio Carlos Machado|");
            //cpf do contabilista
            txt.Append("06797856809|");
            //crc do contabilista
            txt.Append("1SP178287O1|");
            //CNPJ DO ESCRITÓRIO
            txt.Append("|");
            //cep
            txt.Append("|");
            //endereco
            txt.Append("|");
            //numero do imovel
            txt.Append("|");
            //complemento
            txt.Append("|");
            //bairro
            txt.Append("|");
            //telefone
            txt.Append("|");
            //celular/fax
            txt.Append("|");
            //email
            txt.Append("|");
            //codigo municipio
            txt.Append("|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("0100", contadorIndividual));

            //APURAÇÃO DA CONTRIBUIÇÃO SOCIAL E DE APROPRIAÇÃO DE CRÉDITO
            contadorIndividual = 0;
            txt.Append("|0110|");
            //incidencia tributária
            txt.Append("1|");
            //metodo de apropriação de crédito
            txt.Append("1|");
            //Tipo de Contribuição Apurada 
            txt.Append("1|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("0110", contadorIndividual));

            //TABELA DE CADASTRO DE ESTABELECIMENTO
            contadorIndividual = 0;
            txt.Append("|0140|");
            //codigo estabelecimento
            txt.Append(rowEmpresa["COD_EMPRESA"] + "|");
            //nome do estabelecimento
            txt.Append(rowEmpresa["NOME_RAZAO_SOCIAL"] + "|");
            //cnpj do estabelecimento
            txt.Append(clearTxt(rowEmpresa["CNPJ_CPF"].ToString()) + "|");
            //uf do estabelecimento
            txt.Append(rowEmpresa["UF"] + "|");
            //ie do estabelecimento
            txt.Append(clearTxt((rowEmpresa["IE_RG"].ToString() == "" ? "" : (rowEmpresa["IE_RG"].ToString() == "1" ? "" : rowEmpresa["IE_RG"].ToString().Replace("ISENTO", "")))).PadRight(14, ' ').Substring(0, 14).Trim() + "|");
            //codigo do municipio
            txt.Append(rowEmpresa["COD_MUNICIPIO"].ToString().PadLeft(7, '0') + "|");
            //im do estabelecimento
            txt.Append(rowEmpresa["IM"] + "|");
            //suframa estabelecimento
            txt.Append(rowEmpresa["SUFRAMA"] + "|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("0140", contadorIndividual));

            //TABELA DE CADASTRO DO PARTICIPANTE
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
            //ie do estabelecimento
            txt.Append(clearTxt((rowEmpresa["IE_RG"].ToString() == "" ? "" : (rowEmpresa["IE_RG"].ToString() == "1" ? "" : rowEmpresa["IE_RG"].ToString().Replace("ISENTO", "")))).PadRight(14, ' ').Substring(0, 14).Trim() + "|");
            //codigo do municipio
            txt.Append(rowEmpresa["COD_MUNICIPIO"].ToString().PadLeft(7, '0') + "|");
            //suframa 
            txt.Append(rowEmpresa["SUFRAMA"] + "|");
            //endereço
            txt.Append(rowEmpresa["ENDERECO"].ToString().PadRight(60, ' ').Substring(0, 60) + "|");
            //numero
            txt.Append(rowEmpresa["NUMERO"] + "|");
            //complemento
            txt.Append(rowEmpresa["COMPLEMENTO"] + "|");
            //bairro
            txt.Append(rowEmpresa["BAIRRO"] + "|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;

            empresasDAO empresaDAO = new empresasDAO(_conn);
            tbDados = empresaDAO.listaEmpresasPeriodo(_inicio, _termino, _codEmpresa);

            for (int i = 0; i < tbDados.Rows.Count; i++)
            {
                row = tbDados.Rows[i];
                txt.Append("|0150|");
                //codigo 
                txt.Append(row["COD_EMPRESA"] + "|");
                //nome 
                txt.Append(row["NOME_RAZAO_SOCIAL"].ToString().PadRight(100, ' ').Substring(0, 100) + "|");
                //codigo país
                txt.Append(row["COD_PAIS"].ToString().PadLeft(5, '0') + "|");
                //cnpj
                txt.Append(clearTxt((row["FISICA_JURIDICA"].ToString() == "JURIDICA" ? row["CNPJ_CPF"].ToString() : "")) + "|");
                //CPF
                txt.Append(clearTxt((row["FISICA_JURIDICA"].ToString() == "FISICA" ? row["CNPJ_CPF"].ToString() : "")) + "|");
                //ie do estabelecimento
                txt.Append(clearTxt((row["IE_RG"].ToString() == "" ? "" : (row["IE_RG"].ToString() == "1" ? "" : row["IE_RG"].ToString().Replace("ISENTO", "").Replace("isento", "")))).PadRight(14, ' ').Substring(0, 14).Trim() + "|");
                //codigo do municipio
                txt.Append(row["COD_MUNICIPIO"].ToString().PadLeft(7, '0') + "|");
                //suframa 
                txt.Append(row["SUFRAMA"] + "|");
                //endereço
                txt.Append(row["ENDERECO"].ToString().PadRight(60, ' ').Substring(0, 60) + "|");
                //numero
                txt.Append(row["NUMERO"] + "|");
                //complemento
                txt.Append(row["COMPLEMENTO"] + "|");
                //bairro
                txt.Append(row["BAIRRO"] + "|");
                txt.Append("\r\n");
                contador++;
                contadorIndividual++;
            }
            _blocos.Add(new BlocoSped("0150", contadorIndividual));

            //IDENTIFICAÇÃO DAS UNIDADES DE MEDIDA
            unidadesDAO unidadeDAO = new unidadesDAO(_conn);
            List<SUnidade> unidades = unidadeDAO.lista(_codEmpresa);
            contadorIndividual = 0;
            for (int i = 0; i < unidades.Count; i++)
            {
                txt.Append("|0190|");
                //unidade
                txt.Append(unidades[i].sigla + "|");
                //descritivo
                txt.Append(unidades[i].descricao + "|");
                txt.Append("\r\n");
                contador++;
                contadorIndividual++;
            }
            _blocos.Add(new BlocoSped("0190", contadorIndividual));

            produtosDAO produtoDAO = new produtosDAO(_conn);
            List<SProduto> produtos = produtoDAO.list(_codEmpresa);
            contadorIndividual = 0;
            for (int i = 0; i < produtos.Count; i++)
            {
                txt.Append("|0200|");
                //codigo do item
                txt.Append(produtos[i].codProduto + "|");
                //descritivo
                txt.Append(produtos[i].descricao + "|");
                //codigo de barra
                txt.Append("|");
                //codigo anterior
                txt.Append("|");
                //unidade medida
                txt.Append(produtos[i].sigla + "|");
                //tipo item
                txt.Append("09|");
                //ncm
                txt.Append("|");
                //ex_ipi
                txt.Append("|");
                //genero item
                txt.Append("00|");
                //codigo serviço
                txt.Append("|");
                //aliquota icms
                txt.Append("|");
                txt.Append("\r\n");
                contador++;
                contadorIndividual++;
            }
            _blocos.Add(new BlocoSped("0200", contadorIndividual));

            //PLANO DE CONTAS CONTÁBEIS
            contasDAO contaDAO = new contasDAO(_conn);
            tbDados = new DataTable("contas");
            contaDAO.lista(ref tbDados);
            contadorIndividual = 0;
            for (int i = 0; i < tbDados.Rows.Count; i++)
            {
                row = tbDados.Rows[i];
                string codigoConta = row["COD_CONTA"].ToString();
                txt.Append("|0500|");
                //data inclusao
                txt.Append("01012010|");
                //grupo contabil
                txt.Append(deParaGrupoContabil(row["GRUPO_CONTABIL"].ToString()) + "|");
                //analitica/sintetica
                bool analitica = Convert.ToBoolean(Convert.ToInt32(row["ANALITICA"].ToString()));
                txt.Append((analitica ? "A" : "S") + "|");
                //nivel
                txt.Append(codigoConta.Length + "|");
                //codigo conta
                txt.Append(codigoConta + "|");
                //nome conta
                txt.Append(row["DESCRICAO"] + "|");
                //conta relacionada plano de contas referenciado
                txt.Append("|");
                //cnpj do esta0belecimento
                txt.Append("|");
                txt.Append("\r\n");
                contador++;
                contadorIndividual++;
            }
            _blocos.Add(new BlocoSped("0500", contadorIndividual));

            //ENCERRAMENTO DO BLOCO 0
            contadorIndividual = 0;
            txt.Append("|0990|");
            txt.Append((contador + 1) + "|");
            txt.Append("\r\n");
            contadorIndividual++;
            _blocos.Add(new BlocoSped("0990", contadorIndividual));
            return txt.ToString();
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
            _blocos.Add(new BlocoSped("9990", 1));

            txt.Append("|9999|");
            int total = 0;
            for (int i = 0; i < _blocos.Count; i++)
            {
                total += _blocos[i].totalLinhas;
            }
            txt.Append((total + 1) + "|");
            txt.Append("\r\n");
            return txt.ToString();
        }

        private string deParaGrupoContabil(string descricao)
        {
            switch (descricao.ToUpper())
            {
                case "ATIVO":
                    return "01";
                case "PASSIVO":
                    return "02";
                case "DESPESA":
                    return "04";
                case "RECEITA":
                    return "04";
                case "PATRIMONIO LIQUIDO":
                    return "05";
                default:
                    return "09";
            }
        }

        private string blocoA()
        {
            StringBuilder txt = new StringBuilder();
            DataTable tbDados = new DataTable("dados");
            DataRow row = null;
            int contador = 0;
            int contadorIndividual = 0;

            //ABERTURA BLOCO A
            txt.Append("|A001|");
            txt.Append("0|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("A001", contadorIndividual));

            //IDENTIFICAÇÃO DO ESTABELECIMENTO
            contadorIndividual = 0;
            txt.Append("|A010|");
            txt.Append(clearTxt(rowEmpresa["CNPJ_CPF"].ToString()) + "|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("A010", contadorIndividual));

            //DOCUMENTO - NOTA FISCAL DE SERVIÇO
            notaFiscalDAO nfDAO = new notaFiscalDAO(_conn);
            List<SNotaFiscal> notas = nfDAO.list(_codEmpresa, "S", _inicio, _termino);
            lanctosContabDAO lanctosContabDAO = new lanctosContabDAO(_conn);
            contadorIndividual = 0;
            int contadorItens = 0;
            for (int i = 0; i < notas.Count; i++)
            {
                txt.Append("|A100|");
                //tipo de operacao
                txt.Append((notas[i].entradaSaida == "E" ? 0 : 1) + "|");
                //emitente do documento fiscal
                txt.Append((notas[i].entradaSaida == "E" ? 0 : 1) + "|");

                //codigo do participante
                int terceiro = lanctosContabDAO.getFornecedor(notas[i].lote, _codEmpresa);
                txt.Append(terceiro + "|");
                //situação do documento
                txt.Append("00|");
                //serie
                txt.Append("|");
                //subserie
                txt.Append("|");
                //numero do documento
                txt.Append(notas[i].numeroNota + "|");
                //codigo verificacao nota fiscal eletronica
                txt.Append("|");
                //data documento
                DateTime data = lanctosContabDAO.getData(notas[i].lote, _codEmpresa);
                txt.Append(formatData(data) + "|");
                //data término serviço
                txt.Append("|");
                //valor documento
                txt.Append(formatNumero(notas[i].valor) + "|");
                //tipo de pagamento
                txt.Append("1|");
                //valor do desconto
                txt.Append(formatNumero(notas[i].descontos) + "|");
                //valor base calculo pis
                txt.Append(formatNumero(notas[i].baseImpostos) + "|");
                //valor pis
                txt.Append(formatNumero(notas[i].baseImpostos * 0.0165) + "|");
                //valor base calculo cofins
                txt.Append(formatNumero(notas[i].baseImpostos) + "|");
                //valor cofins
                txt.Append(formatNumero(notas[i].baseImpostos * 0.076) + "|");

                SNotaFiscalServico serv = (SNotaFiscalServico)notas[i];
                //valor pis retido
                txt.Append(formatNumero(serv.valorPisRetido) + "|");
                //valor cofins retido
                txt.Append(formatNumero(serv.valorCofinsRetido) + "|");
                //valor iss
                txt.Append(formatNumero(serv.valorIss) + "|");
                txt.Append("\r\n");
                contador++;
                contadorIndividual++;

                notas[i].listItens();
                List<SItemNotaFiscal> itens = notas[i].itens;
                for (int x = 0; x < itens.Count; x++)
                {
                    txt.Append("|A170|");
                    //numero do item
                    txt.Append(itens[x].ordem.ToString("000") + "|");
                    //codigo do item
                    txt.Append(itens[x].codigoProduto + "|");
                    //descricao do item
                    txt.Append(itens[x].descProduto + "|");
                    //valor do item
                    txt.Append(formatNumero(itens[x].valorTotal) + "|");
                    //valor desconto
                    txt.Append(formatNumero(itens[x].desconto) + "|");
                    //codigo base de calculo crédito
                    txt.Append("13|");
                    //origem do crédito
                    txt.Append("0|");
                    //situacao tributaria pis
                    if (itens[x].entradaSaida == "E")
                    {
                        txt.Append("50|");
                    }
                    else
                    {
                        txt.Append("01|");
                    }
                    //valor base calculo
                    txt.Append(formatNumero(itens[x].valorBaseImp) + "|");
                    //aliquota pis
                    txt.Append(formatNumero(itens[x].aliquotaPis) + "|");
                    //valor pis
                    txt.Append(formatNumero(itens[x].valorPis) + "|");
                    //situacao tributaria cofins
                    if (itens[x].entradaSaida == "E")
                    {
                        txt.Append("50|");
                    }
                    else
                    {
                        txt.Append("01|");
                    }
                    //valor base calculo
                    txt.Append(formatNumero(itens[x].valorBaseImp) + "|");
                    //aliquota cofins
                    txt.Append(formatNumero(itens[x].aliquotaCofins) + "|");
                    //valor cofins
                    txt.Append(formatNumero(itens[x].valorCofins) + "|");
                    //codigo conta debitada
                    txt.Append("|");
                    //codigo centro custo
                    txt.Append("|");
                    txt.Append("\r\n");
                    contador++;
                    contadorItens++;
                }
            }
            _blocos.Add(new BlocoSped("A100", contadorIndividual));
            _blocos.Add(new BlocoSped("A170", contadorItens));

            //ENCERRAMENTO DO BLOCO A
            contadorIndividual = 0;
            txt.Append("|A990|");
            txt.Append((contador + 1) + "|");
            txt.Append("\r\n");
            contadorIndividual++;
            _blocos.Add(new BlocoSped("A990", contadorIndividual));

            return txt.ToString();
        }

        private string blocoF()
        {
            StringBuilder txt = new StringBuilder();
            DataTable tbDados = new DataTable("dados");
            DataRow row = null;
            int contador = 0;
            int contadorIndividual = 0;

            //: ABERTURA DO BLOCO F
            txt.Append("|F001|");
            txt.Append("0|");
            contador++;
            txt.Append("\r\n");
            contadorIndividual++;
            _blocos.Add(new BlocoSped("F001", contadorIndividual));

            //IDENTIFICAÇÃO DO ESTABELECIMENTO
            contadorIndividual = 0;
            txt.Append("|F010|");
            txt.Append(clearTxt(rowEmpresa["CNPJ_CPF"].ToString()) + "|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("F010", contadorIndividual));

            //DEMAIS DOCUMENTOS E OPERAÇÕES GERADORAS DE CONTRIBUIÇÃO E CRÉDITOS
            lanctosContabDAO lanctoDAO = new lanctosContabDAO(_conn);
            aliquotasTipoImpostoDAO aliquotasDAO = new aliquotasTipoImpostoDAO(_conn);

            tbDados = lanctoDAO.lancamentosRecibo(_inicio, _termino, _codEmpresa);
            contadorIndividual = 0;
            for (int i = 0; i < tbDados.Rows.Count; i++)
            {
                row = tbDados.Rows[i];
                double valor = Convert.ToDouble(row["VALOR"]);
                txt.Append("|F100|");
                //indicador tipo de operacao
                if (row["DEB_CRED"].ToString().Trim() == "C")
                {
                    txt.Append("1|");
                }
                else
                {
                    txt.Append("0|");

                }
                //terceiro
                int terceiro = Convert.ToInt32(row["COD_TERCEIRO_CORRETO"]);
                if (terceiro == 0)
                {
                    txt.Append(rowEmpresa["COD_EMPRESA"] + "|");
                }
                else
                {
                    txt.Append(row["COD_TERCEIRO_CORRETO"] + "|");
                }

                //codigo do item
                txt.Append(row["COD_PRODUTO"] + "|");
                //data operacao
                txt.Append(formatData(Convert.ToDateTime(row["DATA"])) + "|");
                //valor operacao
                txt.Append(formatNumero(valor) + "|");
                //CODIGO SITUACAO TRIBUTARIA PIS
                if (row["DEB_CRED"].ToString() == "C")
                {
                    txt.Append("01|");

                }
                else
                {
                    txt.Append("50|");

                }
                //balor base de calculo
                txt.Append(formatNumero(valor) + "|");
                //aliquota pis
                SAliquotaImposto aliquotaPis = aliquotasDAO.load("PIS", false, _codEmpresa);
                txt.Append(formatNumero(aliquotaPis.aliquota) + "|");
                //valor pis
                txt.Append(formatNumero(valor * (aliquotaPis.aliquota / 100)) + "|");
                //CODIGO SITUACAO TRIBUTARIA COFINS
                if (row["DEB_CRED"].ToString() == "C")
                {
                    txt.Append("01|");
                }
                else
                {
                    txt.Append("50|");
                }
                //balor base de calculo cofins
                txt.Append(formatNumero(valor) + "|");
                //aliquota cofins
                SAliquotaImposto aliquotaCofins = aliquotasDAO.load("COFINS", false, _codEmpresa);
                txt.Append(formatNumero(aliquotaCofins.aliquota) + "|");
                //valor cofins
                txt.Append(formatNumero(valor * (aliquotaCofins.aliquota / 100)) + "|");
                //codigo base calculo
                if (row["COD_CONTA"].ToString() == "311211")
                {
                    txt.Append("12|");
                }
                else
                {
                    txt.Append("13|");
                }
                //origem do crédito
                txt.Append("0|");
                //codigo conta
                txt.Append(row["COD_CONTA"] + "|");
                //codigo centro custo
                txt.Append("|");
                //historico
                txt.Append(row["HISTORICO"].ToString().Trim().Replace("\r", "").Replace("\n", "").Replace("\r\n", "") + "|");
                txt.Append("\r\n");
                contador++;
                contadorIndividual++;

            }
            _blocos.Add(new BlocoSped("F100", contadorIndividual));

            //ENCERRAMENTO DO BLOCO F
            contadorIndividual = 0;
            txt.Append("|F990|");
            txt.Append((contador + 1) + "|");
            txt.Append("\r\n");
            contadorIndividual++;
            _blocos.Add(new BlocoSped("F990", contadorIndividual));

            return txt.ToString();
        }

        private string blocoC()
        {
            StringBuilder txt = new StringBuilder();
            txt.Append("|C001|1|");
            txt.Append("\r\n");
            _blocos.Add(new BlocoSped("C001", 1));
            txt.Append("|C990|2|");
            txt.Append("\r\n");
            _blocos.Add(new BlocoSped("C990", 1));
            return txt.ToString();
        }

        private string blocoD()
        {
            StringBuilder txt = new StringBuilder();
            txt.Append("|D001|1|");
            txt.Append("\r\n");
            _blocos.Add(new BlocoSped("D001", 1));
            txt.Append("|D990|2|");
            txt.Append("\r\n");
            _blocos.Add(new BlocoSped("D990", 1));
            return txt.ToString();
        }

        private string blocoM()
        {
            StringBuilder txt = new StringBuilder();
            txt.Append("|M001|1|");
            txt.Append("\r\n");
            _blocos.Add(new BlocoSped("M001", 1));
            txt.Append("|M990|2|");
            txt.Append("\r\n");
            _blocos.Add(new BlocoSped("M990", 1));
            return txt.ToString();
        }

        private string bloco1()
        {
            StringBuilder txt = new StringBuilder();
            txt.Append("|1001|1|");
            txt.Append("\r\n");
            _blocos.Add(new BlocoSped("1001", 1));
            txt.Append("|1990|2|");
            txt.Append("\r\n");
            _blocos.Add(new BlocoSped("1990", 1));
            return txt.ToString();
        }
    }
}