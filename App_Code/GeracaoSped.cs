using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;

/// <summary>
/// Summary description for GeracaoSped
/// </summary>
/// 
public class BlocoSped
{
    private string _nome;
    private int _totalLinhas;

    public string nome
    {
        get { return _nome; }
        set { _nome = value; }
    }

    public int totalLinhas
    {
        get { return _totalLinhas; }
        set { _totalLinhas = value; }
    }

    public BlocoSped(string nome, int totalLinhas)
    {
        _nome = nome;
        _totalLinhas = totalLinhas;
    }
}

public class GeracaoSped
{
    private DateTime _inicio;
    private DateTime _termino;
    private int _codEmpresa;
    private Conexao _conn;
    private bool _DocumentoFiscal; //Indicador da Composição da Receita obs: Agrupamento por Documento ou por Cliente
    private bool _RegimeCaixa;
    DataRow rowEmpresa = null;
    List<BlocoSped> _blocos = new List<BlocoSped>();

    public DateTime inicio
    {
        get { return _inicio; }
        set { _inicio = value; }
    }

    public DateTime termino
    {
        get { return _termino; }
        set { _termino = value; }
    }

    public int codEmpresa
    {
        get { return _codEmpresa; }
        set { _codEmpresa = value; }
    }

    public bool DocumentoFiscal
    {
        get { return _DocumentoFiscal; }
        set { _DocumentoFiscal = value; }
    }

    public bool RegimeCaixa 
    {
        get { return _RegimeCaixa; }
        set { _RegimeCaixa = value; }
    }

	public GeracaoSped(DateTime inicio, DateTime termino, int codEmpresa, Conexao conn)
	{
        _inicio = inicio;
        _termino = termino;
        _codEmpresa = codEmpresa;
        _conn = conn;
	}


    public bool gerar(string caminho, bool lucroReal)
    {
        //bool lucroReal;
        using (StreamWriter writer = new StreamWriter(caminho,false, Encoding.Default))
        {
            empresasDAO empresaDAO = new empresasDAO(_conn);
            _RegimeCaixa = empresaDAO.retornaRegimeCaixa();

            DataTable tbEmpresa = empresaDAO.loadSped(_codEmpresa);
            if (tbEmpresa.Rows.Count == 0)
            {
                throw new ApplicationException("Nenhuma empresa encontrada.");
            }

            
            rowEmpresa = tbEmpresa.Rows[0];
            
            string retornoBloco0 = bloco0(lucroReal);

            if (retornoBloco0 == "erro") 
                return false;

            writer.Write(retornoBloco0);
            writer.Write(blocoA(lucroReal));
            writer.Write(blocoC());
            writer.Write(blocoD());
            writer.Write(blocoF(lucroReal));
            writer.Write(blocoM());
            writer.Write(bloco1());
            writer.Write(bloco9());
        }

        return true;
    }

    private string bloco0(bool lucroReal)
    {
        StringBuilder txt = new StringBuilder();
            
        txt.Append("|0000|");
        DataTable tbDados = new DataTable("dados");
        DataRow row = null;
        int contador = 0;
        int contadorIndividual = 0;

        //INICIO BLOCO 0
        //codigo versão
        txt.Append("006|");
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
        txt.Append("" + rowEmpresa["NOME_RAZAO_SOCIAL"].ToString().Trim().Replace(".","").Replace("@", "").Replace("/", "").Replace("|", "").Replace("-", "").Replace(":", "").Replace("&", "").Replace("+", "").Replace("_", "").Replace("<", "").Replace(">", "").Replace("(", "").Replace(")", "").Replace("!", "").Replace("?", "").Replace("$", "").Replace("'", "").Replace("%", "") + "|");
        //cnpj da empresa
        txt.Append("" + clearTxt(rowEmpresa["CNPJ_CPF"].ToString()) + "|");
        //uf da empresa;
        txt.Append("" + rowEmpresa["UF"] + "|");
        //codigo do municipio
        txt.Append("" + rowEmpresa["COD_MUNICIPIO"].ToString().PadLeft(7,'0') + "|");
        //suframa
        txt.Append("|");
        //natureza da pessoa juridica
        txt.Append("|");
        //tipo de atividade preponderante
        txt.Append( "9|");
       
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

        contadorDAO contadorDAO = new contadorDAO(_conn);
        SContador cont = contadorDAO.load(SessionView.EmpresaSession);
        if (cont != null)
        {
            //DADOS DO CONTABILISTA
            contadorIndividual = 0;
            txt.Append("|0100|");
            //nome do contabilista
            txt.Append("" + cont.nome + "|");
            //cpf do contabilista
            txt.Append("" + cont.cpf.Replace(".", "").Replace("-", "") + "|");
            //crc do contabilista
            txt.Append("" + cont.crc + "|");
            //CNPJ DO ESCRITÓRIO
            txt.Append("" + cont.cnpjEscritorio.Replace("/", "").Replace(".", "").Replace(" ", "").Replace("-", "").PadLeft(14, '0') + "|");
            //cep
            txt.Append(""+cont.cep.ToString().Replace("-","").PadLeft(8,'0')+"|");
            //endereco
            txt.Append(""+cont.endereco+"|");
            //numero do imovel
            txt.Append("" + cont.numero + "|");
            //complemento
            txt.Append("" + cont.complemento + "|");
            //bairro
            txt.Append("" + cont.bairro + "|");
            //telefone
            txt.Append("" + cont.telefone.ToString().Replace(")", "").Replace("(", "").Replace(" ", "").Replace("-", "") + "|");
            //celular/fax
            txt.Append("" + cont.celular.ToString().Replace(")", "").Replace("(", "").Replace(" ", "").Replace("-", "") + "|");
            //email
            txt.Append("" + cont.email + "|");
            //codigo municipio
            txt.Append(""+cont.codigoMunicipio.ToString().PadRight(7,'0')+"|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("0100", contadorIndividual));
        }
        else 
        {
            return"erro";
        }


        //APURAÇÃO DA CONTRIBUIÇÃO SOCIAL E DE APROPRIAÇÃO DE CRÉDITO
        contadorIndividual = 0;
        txt.Append("|0110|");
        //incidencia tributária
        if (lucroReal)
        {
            txt.Append((_RegimeCaixa ? "2" : "1" )+"|");
            //metodo de apropriação de crédito
            txt.Append("1|");
            //Tipo de Contribuição Apurada 
            txt.Append("1|"+(_RegimeCaixa ? "1" : "" )+"|");
            txt.Append("\r\n");
        }
        else
        {
            txt.Append("2|");
            //metodo de apropriação de crédito
            txt.Append("1|");
            //Tipo de Contribuição Apurada 
            txt.Append("1|");
            txt.Append((_RegimeCaixa ? "1" : "2")+"|");
            txt.Append("\r\n");
        }
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
        txt.Append(clearTxt((rowEmpresa["IE_RG"].ToString() == "" ? "" : (rowEmpresa["IE_RG"].ToString() == "1" ? "" : rowEmpresa["IE_RG"].ToString().Trim().ToUpper().Replace("ISENTO", "").Replace("ISENTA", "")))).PadRight(14, ' ').Substring(0, 14).Trim() + "|");
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
        txt.Append(rowEmpresa["cod_pais"].ToString().PadLeft(5, '0') + "|");
        //cnpj
        txt.Append(clearTxt((rowEmpresa["FISICA_JURIDICA"].ToString() == "JURIDICA" ? rowEmpresa["CNPJ_CPF"].ToString() : "")) + "|");
        //CPF
        txt.Append(clearTxt((rowEmpresa["FISICA_JURIDICA"].ToString() == "FISICA" ? rowEmpresa["CNPJ_CPF"].ToString() : "")) + "|");
        //ie do estabelecimento
        txt.Append(clearTxt((rowEmpresa["IE_RG"].ToString() == "" || rowEmpresa["FISICA_JURIDICA"].ToString() == "FISICA" ? "" : (rowEmpresa["IE_RG"].ToString() == "1" ? "" : rowEmpresa["IE_RG"].ToString().Trim().ToUpper().Replace("ISENTO", "").Replace("ISENTA", "")))).PadRight(14, ' ').Substring(0, 14).Trim() + "|");
        //codigo do municipio
        txt.Append(rowEmpresa["cod_municipio"].ToString().PadLeft(7, '0') + "|");
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
        tbDados = empresaDAO.listaEmpresasPeriodo(true, _inicio, _termino, _codEmpresa);
        
        for (int i = 0; i < tbDados.Rows.Count; i++)
        {
            row = tbDados.Rows[i];
            txt.Append("|0150|");
            //codigo 
            txt.Append(row["COD_EMPRESA"] + "|");
            //nome 
            txt.Append(row["NOME_RAZAO_SOCIAL"].ToString().PadRight(100,' ').Substring(0,100) + "|");
            //codigo país
            txt.Append(row["IBGE_PAIS"].ToString().PadLeft(5,'0') + "|");
            //cnpj
            txt.Append(clearTxt((row["FISICA_JURIDICA"].ToString() == "JURIDICA" ? row["CNPJ_CPF"].ToString().Replace("00000000000000","") : "")) + "|");
            //CPF
            txt.Append(clearTxt((row["FISICA_JURIDICA"].ToString() == "FISICA" ? row["CNPJ_CPF"].ToString().Replace("00000000000000", "") : "")) + "|");
            //ie do estabelecimento
            txt.Append(clearTxt((row["IE_RG"].ToString() == "" || row["FISICA_JURIDICA"].ToString() == "FISICA" ? "" : (row["IE_RG"].ToString() == "1" ? "" : row["IE_RG"].ToString().ToUpper().Replace("ISENTO", "").Replace("ISENTA", "").Replace("isento", "").Replace("Isenta", "").Replace("isenta", "").Replace("ISENTA", "").Replace("00000000000000", "")))).PadRight(14, ' ').Substring(0, 14).Trim() + "|");
            //codigo do municipio
            txt.Append(row["IBGE_mun"].ToString().PadLeft(7, '0') + "|");
            //suframa 
            txt.Append(row["SUFRAMA"] + "|");
            //endereço
            txt.Append(row["ENDERECO"].ToString().PadRight(60, ' ').Substring(0, 60).Replace("º", "").Replace(",", "").Replace(".", "").Replace("/", "") + "|");
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

        txt.Append("|0200|");
        //codigo do item
        txt.Append("1|");
        //descritivo
        txt.Append("Outros Produtos|");
        //codigo de barra
        txt.Append("|");
        //codigo anterior
        txt.Append("|");
        //unidade medida
        txt.Append("|");
        //tipo item
        txt.Append("99|");
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

            string DescConta = row["DESCRICAO"].ToString();
            if (DescConta.Length > 60)
            {
                DescConta = DescConta.Substring(0, 60);
            }

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
            txt.Append(row["nivel"].ToString() + "|");
            //codigo conta
            txt.Append(codigoConta + "|");
            //nome conta
            txt.Append(DescConta + "|");
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

    //private string blocoA(bool LucroReal)
    //{
    //    StringBuilder txt = new StringBuilder();
    //    DataTable tbDados = new DataTable("dados");
    //    DataRow row = null;
    //    int contador = 0;
    //    int contadorIndividual = 0;

    //    //ABERTURA BLOCO A
    //    txt.Append("|A001|");

    //    if (LucroReal && !_RegimeCaixa)
    //    {
    //        txt.Append("0|");
    //    }
    //    else
    //    {
    //        txt.Append("1|");
    //    }
    //    txt.Append("\r\n");
    //    contador++;
    //    contadorIndividual++;
    //    _blocos.Add(new BlocoSped("A001", contadorIndividual));

    //    //IDENTIFICAÇÃO DO ESTABELECIMENTO
    //    if (LucroReal && !_RegimeCaixa)
    //    {
    //        contadorIndividual = 0;
    //        txt.Append("|A010|");
    //        txt.Append(clearTxt(rowEmpresa["CNPJ_CPF"].ToString()) + "|");
    //        txt.Append("\r\n");
    //        contador++;
    //        contadorIndividual++;
    //        _blocos.Add(new BlocoSped("A010", contadorIndividual));

    //        //DOCUMENTO - NOTA FISCAL DE SERVIÇO
    //        notaFiscalDAO nfDAO = new notaFiscalDAO(_conn);
    //        List<SNotaFiscal> notas = nfDAO.list(_codEmpresa, "S", _inicio, _termino);
    //        lanctosContabDAO lanctosContabDAO = new lanctosContabDAO(_conn);
    //        contadorIndividual = 0;
    //        int contadorItens = 0;
    //        for (int i = 0; i < notas.Count; i++)
    //        {
    //            txt.Append("|A100|");
    //            //tipo de operacao
    //            txt.Append((notas[i].entradaSaida == "E" ? 0 : 1) + "|");
    //            //emitente do documento fiscal
    //            txt.Append((notas[i].entradaSaida == "E" ? 1 : 0) + "|");

    //            //codigo do participante
    //            int terceiro = lanctosContabDAO.getFornecedor(notas[i].lote, _codEmpresa);
    //            txt.Append(terceiro + "|");
    //            //situação do documento
    //            txt.Append("00|");
    //            //serie
    //            txt.Append("|");
    //            //subserie
    //            txt.Append("|");
    //            //numero do documento
    //            txt.Append(notas[i].numeroNota + "|");
    //            //codigo verificacao nota fiscal eletronica
    //            txt.Append("|");
    //            //data documento
    //            DateTime data = lanctosContabDAO.getData(notas[i].lote, _codEmpresa);
    //            txt.Append(formatData(data) + "|");
    //            //data término serviço
    //            txt.Append("|");
    //            //valor documento
    //            txt.Append(formatNumero(notas[i].valor) + "|");
    //            //tipo de pagamento
    //            txt.Append("1|");
    //            //valor do desconto
    //            txt.Append(formatNumero(notas[i].descontos) + "|");
    //            //valor base calculo pis
    //            txt.Append(formatNumero(notas[i].baseImpostos) + "|");
    //            //valor pis
    //            txt.Append(formatNumero(notas[i].baseImpostos * 0.0165) + "|");
    //            //valor base calculo cofins
    //            txt.Append(formatNumero(notas[i].baseImpostos) + "|");
    //            //valor cofins
    //            txt.Append(formatNumero(notas[i].baseImpostos * 0.076) + "|");

    //            SNotaFiscalServico serv = (SNotaFiscalServico)notas[i];
    //            //valor pis retido
    //            txt.Append(formatNumero(serv.valorPisRetido) + "|");
    //            //valor cofins retido
    //            txt.Append(formatNumero(serv.valorCofinsRetido) + "|");
    //            //valor iss
    //            txt.Append(formatNumero(serv.valorIss) + "|");
    //            txt.Append("\r\n");
    //            contador++;
    //            contadorIndividual++;

    //            notas[i].listItens();
    //            List<SItemNotaFiscal> itens = notas[i].itens;
    //            for (int x = 0; x < itens.Count; x++)
    //            {
    //                txt.Append("|A170|");
    //                //numero do item
    //                txt.Append(itens[x].ordem.ToString("000") + "|");
    //                //codigo do item
    //                txt.Append(itens[x].codigoProduto + "|");
    //                //descricao do item
    //                txt.Append(itens[x].descProduto + "|");
    //                //valor do item
    //                txt.Append(formatNumero(itens[x].valorTotal) + "|");
    //                //valor desconto
    //                txt.Append(formatNumero(itens[x].desconto) + "|");
    //                //codigo base de calculo crédito
    //                txt.Append("13|");
    //                //origem do crédito
    //                txt.Append("0|");
    //                //situacao tributaria pis
    //                if (itens[x].entradaSaida == "E")
    //                {
    //                    txt.Append("50|");
    //                }
    //                else
    //                {
    //                    txt.Append("01|");
    //                }
    //                //valor base calculo
    //                txt.Append(formatNumero(itens[x].valorBaseImp) + "|");
    //                //aliquota pis
    //                txt.Append(formatNumero(itens[x].aliquotaPis) + "|");
    //                //valor pis
    //                txt.Append(formatNumero(itens[x].valorPis) + "|");
    //                //situacao tributaria cofins
    //                if (itens[x].entradaSaida == "E")
    //                {
    //                    txt.Append("50|");
    //                }
    //                else
    //                {
    //                    txt.Append("01|");
    //                }
    //                //valor base calculo
    //                txt.Append(formatNumero(itens[x].valorBaseImp) + "|");
    //                //aliquota cofins
    //                txt.Append(formatNumero(itens[x].aliquotaCofins) + "|");
    //                //valor cofins
    //                txt.Append(formatNumero(itens[x].valorCofins) + "|");
    //                //codigo conta debitada
    //                txt.Append("|");
    //                //codigo centro custo
    //                txt.Append("|");
    //                txt.Append("\r\n");
    //                contador++;
    //                contadorItens++;
    //            }
    //        }
    //        _blocos.Add(new BlocoSped("A100", contadorIndividual));
    //        _blocos.Add(new BlocoSped("A170", contadorItens));
    //    }
    //    //ENCERRAMENTO DO BLOCO A
    //    contadorIndividual = 0;
    //    txt.Append("|A990|");
    //    txt.Append((contador + 1) +"|");
    //    txt.Append("\r\n");
    //    contadorIndividual++;
    //    _blocos.Add(new BlocoSped("A990", contadorIndividual));

    //    return txt.ToString();
    //}

    private string blocoA(bool LucroReal)
    {
        StringBuilder txt = new StringBuilder();
        DataTable tbDados = new DataTable("dados");
        DataRow row = null;
        int contador = 0;
        int contadorIndividual = 0;

        //ABERTURA BLOCO A
        txt.Append("|A001|");

        if (LucroReal && !_RegimeCaixa)
        {
            txt.Append("0|");
        }
        else
        {
            txt.Append("1|");
        }
        txt.Append("\r\n");
        contador++;
        contadorIndividual++;
        _blocos.Add(new BlocoSped("A001", contadorIndividual));

        //IDENTIFICAÇÃO DO ESTABELECIMENTO
        if (LucroReal && !_RegimeCaixa)
        {
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
                txt.Append((notas[i].entradaSaida == "E" ? 1 : 0) + "|");

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

                //Busca Conta Contabil

                string SqlConta = @"select * from cad_contas where cod_conta in
                                        (select cod_conta from lanctos_contab where lote = " + serv.lote.ToString() + @")
                                        and cod_empresa = " + _codEmpresa.ToString() + @"
                                        and gera_credito = 1";
                //Alberto aqui
                DataTable tb = _conn.dataTable(SqlConta, "Contas");
                String strConta = "";
                if (tb.Rows.Count > 0)
                {
                    DataRow rowConta = tb.Rows[0];
                    strConta = rowConta["cod_conta"].ToString();
                }

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
                    txt.Append(strConta + "|");
                    //codigo centro custo
                    txt.Append("|");
                    txt.Append("\r\n");
                    contador++;
                    contadorItens++;
                }
            }
            _blocos.Add(new BlocoSped("A100", contadorIndividual));
            _blocos.Add(new BlocoSped("A170", contadorItens));
        }

        //ENCERRAMENTO DO BLOCO A
        contadorIndividual = 0;
        txt.Append("|A990|");
        txt.Append((contador + 1) + "|");
        txt.Append("\r\n");
        contadorIndividual++;
        _blocos.Add(new BlocoSped("A990", contadorIndividual));

        return txt.ToString();
    }

    private string blocoF(bool LucroReal)
    {
        StringBuilder txt = new StringBuilder();
        DataTable tbDados = new DataTable("dados");
        DataRow row = null;
        int contador = 0;
        int contadorIndividual = 0;

        //: ABERTURA DO BLOCO F
        txt.Append("|F001|");
        if (LucroReal)
        {
            txt.Append("0|");
        }
        else
        {
            txt.Append("0|");
        }
        contador++;
        txt.Append("\r\n");
        contadorIndividual++;
        _blocos.Add(new BlocoSped("F001", contadorIndividual));

        //IDENTIFICAÇÃO DO ESTABELECIMENTO
        //if (LucroReal)
        {
            contadorIndividual = 0;
            txt.Append("|F010|");
            txt.Append(clearTxt(rowEmpresa["CNPJ_CPF"].ToString()) + "|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("F010", contadorIndividual));

            //DEMAIS DOCUMENTOS E OPERAÇÕES GERADORAS DE CONTRIBUIÇÃO E CRÉDITOS
            empresasDAO empresasDAO = new empresasDAO(_conn);            
            lanctosContabDAO lanctoDAO = new lanctosContabDAO(_conn);
            aliquotasTipoImpostoDAO aliquotasDAO = new aliquotasTipoImpostoDAO(_conn);

            if (!_RegimeCaixa)
            {
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
                    txt.Append("1|");
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
            }
            else 
            {
                contadorIndividual = 0;
                DataTable Resumo = lanctoDAO.lancamentosReciboSumValor(_inicio, _termino, _codEmpresa);
                DataTable dt = lanctoDAO.lancamentosReciboBlocoF525(_inicio, _termino, _codEmpresa, _DocumentoFiscal);

                DataTable AliqImposto = lanctoDAO.BuscaAliqImpostos(_codEmpresa);

                double AliqPis = Convert.ToDouble("1.65");
                double AliqCofins = Convert.ToDouble("7");
                foreach (DataRow LinhaImposto in AliqImposto.Rows)
                {
                    if (LinhaImposto["TIPO_IMPOSTO"].ToString().ToUpper().Contains("PIS"))
                    {
                        AliqPis = Convert.ToDouble(LinhaImposto["ALIQUOTA"]);
                    }
                    if (LinhaImposto["TIPO_IMPOSTO"].ToString().ToUpper().Contains("COFIN"))
                    {
                        AliqCofins = Convert.ToDouble(LinhaImposto["ALIQUOTA"]);
                    }
                }

                foreach (DataRow RowResumo in Resumo.Rows)
                {

                    decimal valorSum = Convert.ToDecimal( RowResumo["valor"]);

                    txt.Append("|F500|");
                    txt.Append(valorSum + "|");
                    txt.Append(RowResumo["tipo"].ToString().Trim() + "|");
                    txt.Append("0|");
                    txt.Append(valorSum + "|");
                    txt.Append(AliqPis.ToString().Replace(".", ",") + "|");
                    txt.Append(Math.Round(((Convert.ToDouble(valorSum) * AliqPis) / 100), 2) + "|");
                    txt.Append(RowResumo["tipo"].ToString().Trim() + "|"); //Código da Situação Tributária referente à Cofins
                    txt.Append("0|"); //Valor do desconto/exclusão da base de cálculo
                    txt.Append(valorSum + "|"); //Valor Cofins
                    txt.Append(AliqCofins.ToString().Replace(".", ",") + "|");
                    txt.Append(Math.Round(((Convert.ToDouble(valorSum) * AliqCofins) / 100), 2) + "|");
                    txt.Append("|");
                    txt.Append("|");
                    txt.Append("|");
                    txt.Append("|");
                    txt.Append("\r\n");
                    contador++;
                    contadorIndividual++;
                    _blocos.Add(new BlocoSped("F500", contadorIndividual));

                    contadorIndividual = 0;


                    foreach (DataRow datarow in dt.Rows)
                    {
                        if (RowResumo["tipo"].ToString().Trim() == datarow["tipo"].ToString().Trim())
                        {
                            txt.Append("|F525|");
                            txt.Append(valorSum + "|");
                            txt.Append((_DocumentoFiscal ? "04" : "01") + "|");
                            txt.Append((_DocumentoFiscal ? "" : datarow["CNPJ"].ToString()) + "|"); //CNPJ
                            txt.Append((_DocumentoFiscal ? datarow["NUMERO_DOCUMENTO"].ToString() : "") + "|"); //NUMERO DOCUMENTO
                            txt.Append("|");
                            txt.Append(datarow["VALOR"] + "|"); //Valor da Receita
                            txt.Append(datarow["tipo"].ToString().Trim() + "|");
                            txt.Append(datarow["tipo"].ToString().Trim() + "|");
                            txt.Append("|");
                            txt.Append("|");
                            txt.Append("\r\n");
                            contador++;
                            contadorIndividual++;
                        }
                    }
                    _blocos.Add(new BlocoSped("F525", contadorIndividual));
                }
            }
        }
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
        // Bloco C100
        notaFiscalDAO notaFiscalDAO = new notaFiscalDAO(_conn);
        List<SNotaFiscal> listNota = notaFiscalDAO.list(Convert.ToInt32(HttpContext.Current.Session["empresa"]), "P", _inicio, _termino);


        // Bloco C001
        StringBuilder txt = new StringBuilder();
        txt.Append("|C001|" + (listNota.Count > 0 ? 0 : 1) + "|");
        txt.Append("\r\n");
        _blocos.Add(new BlocoSped("C001", 1));

        int contadorIndividualC010 = 0;

        if (listNota.Count != 0)
        {
            // Bloco C010
            empresasDAO empresaDAO = new empresasDAO(_conn);
            txt.Append("|C010|" + empresaDAO.retornaCNPJ().PadLeft(14,'0') + "|2|");
            txt.Append("\r\n");
            contadorIndividualC010++;
            _blocos.Add(new BlocoSped("C010", contadorIndividualC010));
        }

        int contadorIndividual = 0;
        int contadorIndividualC170 = 0;
        int numeroSeqItemNF = 0;

        foreach(SNotaFiscal notaFiscal in listNota)
        {
            contadorIndividual++;

            txt.Append("|C100|");
            txt.Append((notaFiscal.entradaSaida == "E" ? 0 : 1) + "|");
            txt.Append((notaFiscal.entradaSaida == "E" ? 1 : 0) + "|");
            txt.Append(notaFiscal.cod_part + "|");
            txt.Append("01|");
            txt.Append("00|");
            txt.Append("|"); //Série do Documento Fiscal
            txt.Append(notaFiscal.numeroNota + "|");
            txt.Append("|"); //Chave da Nota Fiscal
            txt.Append(notaFiscal.data_emissao.ToString("ddMMyyyy") + "|"); //Data Emissão
            txt.Append(notaFiscal.data_emissao.ToString("ddMMyyyy") + "|"); //Data da Entrada ou Saída
            txt.Append(formatNumero(notaFiscal.valor) + "|");
            txt.Append("0|"); //Á vista!
            txt.Append(formatNumero(notaFiscal.descontos) + "|"); //Valor do desconto
            txt.Append(formatNumero(0) + "|"); //Abatimento
            txt.Append(formatNumero(notaFiscal.valor) + "|"); //Valor Total das Mercadorias
            txt.Append("9|"); //Tipo de Frete: Sem Cobrança
            txt.Append(formatNumero(notaFiscal.frete) + "|"); //Valor do Frete Indicado - 18
            txt.Append(formatNumero(0) + "|"); //Valor do Seguro Indicado - 19
            txt.Append(formatNumero(0) + "|"); //Valor de Outras Despesas - 20
            txt.Append(formatNumero(notaFiscal.baseImpostos) + "|"); //Valor da Base de Cálculo - 21
            txt.Append(formatNumero(notaFiscal.icms) + "|"); //Valor do ICMS - 22
            txt.Append(formatNumero(notaFiscal.baseImpostos) + "|"); //Valor da Base de Cálculo do ICMS - 23
            txt.Append(formatNumero(0) + "|"); //Valor do ICMS Retido - 24
            txt.Append(formatNumero(notaFiscal.ipi) + "|"); //Valor Total do IPI - 25
            txt.Append(formatNumero(notaFiscal.valorPis) + "|"); //Valor Total do PIS - 26
            txt.Append(formatNumero(notaFiscal.valorCofins) + "|"); //Valor Total do COFINS - 27
            txt.Append(formatNumero(0) + "|"); //Valor Total do PIS Retido - 28
            txt.Append(formatNumero(0) + "|"); //Valor Total do COFINS Retido - 29
            txt.Append("\r\n");

            foreach(SItemNotaFiscal item in notaFiscal.itens)
            {
                contadorIndividualC170++;
                numeroSeqItemNF++;

                txt.Append("|C170|");
                txt.Append(numeroSeqItemNF+"|");
                txt.Append(item.codigoProduto + "|"); //Código do Produto
                txt.Append("|"); //Descrição do Produto
                txt.Append(item.qtde + "|"); //Quantidade
                txt.Append("|"); //Unidade
                txt.Append(formatNumero(item.valorTotal) + "|"); //Valor Total
                txt.Append("|"); //Valor do Desconto
                txt.Append("|"); //Movimentação Física
                txt.Append("000|"); //Código da Situação Tributária 
                txt.Append((item.cfop.ToString().Length > 4 ? item.cfop.ToString().Substring(0, 4) : item.cfop.ToString().PadLeft(4, '0')) + "|"); //CFOP
                txt.Append("|"); //Código da Natureza da Operação
                txt.Append(formatNumero(item.valorBaseImp)+"|"); //Valor da base de cálculo do ICMS
                txt.Append(formatNumero(item.aliquotaIcms) + "|"); //AliqIcms
                txt.Append(formatNumero(item.valorIcms) + "|"); //Valor Icms
                txt.Append(formatNumero(0) + "|"); //Valor da base de cálculo referente à substituição tributária 
                txt.Append("|"); //Alíquota do ICMS 
                txt.Append("|"); //Valor do ICMS referente 
                txt.Append("0|"); //Indicador de período de apuração do IPI
                txt.Append("00|"); //Código da Situação Tributária referente ao IPI, conforme a Tabela indicada no item 4.3.2
                txt.Append("|"); // Código de enquadramento legal do IPI
                txt.Append(formatNumero(item.valorBaseImp) + "|"); //Valor da base de cálculo do IPI
                txt.Append(formatNumero(item.aliquotaIpi) + "|"); //Alíquota do IPI
                txt.Append(formatNumero(item.valorIpi) + "|"); //Valor do IPI
                txt.Append((notaFiscal.entradaSaida == "E" ? "50" : "01") + "|"); //Código da Situação Tributária referente ao PIS. Entrada 50 / Saída 01
                txt.Append(formatNumero(item.valorBaseImp) + "|"); //Valor da base de cálculo do PIS 
                txt.Append(formatNumero(item.aliquotaPis) + "|"); //Alíquota do PIS
                txt.Append("|"); //Quantidade – Base de cálculo PIS/PASEP 
                txt.Append("|"); //Alíquota do PIS (em reais) 
                txt.Append(formatNumero(item.valorPis) + "|"); //Valor do PIS
                txt.Append((notaFiscal.entradaSaida == "E" ? "50" : "01") + "|"); //Código da Situação Tributária referente ao COFINS. Entrada 50 / Saída 01
                txt.Append(formatNumero(item.valorBaseImp) + "|"); //Valor da base de cálculo da COFINS
                txt.Append(formatNumero(item.aliquotaCofins) + "|"); //Alíquota da COFINS
                txt.Append("|"); //Quantidade
                txt.Append("|"); //Alíquota da COFINS (em reais)
                txt.Append(formatNumero(item.valorCofins) + "|"); //Valor da COFINS
                txt.Append("|"); //Código da Conta Analítica
                txt.Append("\r\n");
            }
            numeroSeqItemNF = 0;
        }
        _blocos.Add(new BlocoSped("C100", contadorIndividual));
        _blocos.Add(new BlocoSped("C170", contadorIndividualC170));

        txt.Append("|C990|" + (1 + 1 + contadorIndividual + contadorIndividualC170 + contadorIndividualC010) + "|");
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
        lanctosContabDAO lanctoDAO = new lanctosContabDAO(_conn);
        DataTable dt = lanctoDAO.lancamentosReciboBlocoF525(_inicio, _termino, _codEmpresa, _DocumentoFiscal);

        StringBuilder txt = new StringBuilder();
        if (dt.Rows.Count > 0)
        {
            txt.Append("|1001|0|");
        }
        else
        {
            txt.Append("|1001|1|");
        }
        txt.Append("\r\n");
        _blocos.Add(new BlocoSped("1001", 1));
        long contador = 1;
        int contadorIndividual = 0;
        //Alberto
        bool Primeiro = true;
        bool Vfaz = false;
        string CNPJAnt = "";
        decimal ValorTotal = 0;
        string StrTipo = "";
        foreach (DataRow datarow in dt.Rows)
        {
            if (Primeiro)
            {
                Primeiro = false;
                CNPJAnt = datarow["cnpj_cpf"].ToString().Trim();
            }
            if (CNPJAnt == datarow["cnpj_cpf"].ToString().Trim())
            {
                StrTipo = datarow["tipo"].ToString();
                ValorTotal = ValorTotal + Convert.ToDecimal(datarow["valor"]);
                Vfaz = true;
            }
            else
            {
                txt.Append("|1900|");
                txt.Append(CNPJAnt + "|");
                txt.Append(datarow["tipo"].ToString().Trim() + "|");
                txt.Append("|");
                txt.Append("|");
                txt.Append("00|");
                txt.Append(ValorTotal.ToString().Trim().Replace(".", ",") + "|");
                txt.Append("|");
                txt.Append(datarow["tipo"].ToString().Trim() + "|");
                txt.Append(datarow["tipo"].ToString().Trim() + "|");
                txt.Append("|");
                if (datarow["tipo"].ToString().Trim() == "99")
                {
                    txt.Append("Aluguel|");
                }
                else
                {
                    txt.Append("Outras Receitas|");
                }
                txt.Append("|");
                txt.Append("\r\n");
                contador++;
                contadorIndividual++;
                //_blocos.Add(new BlocoSped("1900", contadorIndividual));
                CNPJAnt = datarow["cnpj_cpf"].ToString().Trim();
                ValorTotal = Convert.ToDecimal(datarow["valor"]);
                StrTipo = datarow["tipo"].ToString();
                Vfaz = true;
            }
        }
        if (Vfaz)
        {
            txt.Append("|1900|");
            txt.Append(CNPJAnt + "|");
            txt.Append(StrTipo.ToString().Trim() + "|");
            txt.Append("|");
            txt.Append("|");
            txt.Append("00|");
            txt.Append(ValorTotal.ToString().Trim().Replace(".", ",") + "|");
            txt.Append("|");
            txt.Append(StrTipo.ToString().Trim() + "|");
            txt.Append(StrTipo.ToString().Trim() + "|");
            txt.Append("|");
            if (StrTipo.ToString().Trim() == "99")
            {
                txt.Append("Aluguel|");
            }
            else
            {
                txt.Append("Outras Receitas|");
            }
            txt.Append("|");
            txt.Append("\r\n");
            contador++;
            contadorIndividual++;
            _blocos.Add(new BlocoSped("1900", contadorIndividual));
        }
        contador++;
        txt.Append("|1990|"+contador.ToString().Trim()+"|");
        txt.Append("\r\n");
        _blocos.Add(new BlocoSped("1990", 1));
        return txt.ToString();
    }

    private string bloco9()
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
            txt.Append(_blocos[i].nome+"|");
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

    public string deParaGrupoContabil(string descricao)
    {
        switch (descricao.ToUpper())
        {
            case "ATIVO":
                return "01";
            case "PASSIVO":
                return "02";
            case "DESPESA":
                return "04";
            case "DESPESAS":
                return "04";
            case "RECEITA":
                return "04";
            case "RECEITAS":
                return "04";
            case "RESULTADOS":
                return "04";
            case "RESULTADO":
                return "04";
            case "PATRIMONIO LIQUIDO":
                return "03";
            case "PATRIMÔNIO LIQUIDO":
                return "03";
            case "PATRIMÔNIO LÍQUIDO":
                return "03";
            default:
                switch (descricao.Substring(0, 3).ToUpper())
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

    private string formatData(DateTime data)
    {
        return data.ToString("ddMMyyyy");
    }

    private string formatNumero(double numero)
    {
        return String.Format("{0:0.00}", numero);
    }

    private string clearTxt(string txt)
    {
        return txt.Replace(".", "").Replace("/", "").Replace("-","");
    }
}