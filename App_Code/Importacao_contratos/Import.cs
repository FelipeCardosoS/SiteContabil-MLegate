using System;
using System.Collections.Generic;
using ExtremeML.Packaging;
/// <summary>
/// Summary description for Import
/// </summary>
/// 

public class Vencimento
{
    public int ordem;
    public decimal valor;
    public DateTime vencto;
    public string formaPagamento;

}



public class Import
{
    private Conexao conn;
    public Import(Conexao c)
    {
        conn = c;
    }
    public string historico;
    public string nomeplanilha;
    public int cod_planilha;
    public int codigodomodelo;
    public string caminho;
    public string contachequeboleto;
    public string contacartao;
    public string contareceita;
    public string _descricaocontachequeboleto;
    public string _descricaocontacartao;
    public string _descricaocontareceita;
    public DateTime date;
    public int job;
    private List<string> errosGeral = new List<string>();
    private List<string> erros = new List<string>();

    public List<string> importar()
    {
        //Microsoft.Office.Interop.Excel.Application appExcel = null;
        //Microsoft.Office.Interop.Excel.Workbook excelWb = null;
        //object oMissing = System.Reflection.Missing.Value;
        //Microsoft.Office.Interop.Excel.Range c1 = null;
        //appExcel = new Microsoft.Office.Interop.Excel.Application();

        //Microsoft.Office.Interop.Excel.Sheets excelSS;
        //excelWb = appExcel.Workbooks.Open(caminho, 0, true, 5, "",
        //    "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, null, null);
        //excelSS = excelWb.Worksheets;

        List<SLancamento> arr = new List<SLancamento>();
        importao_planilhaDAO imp_planilha = new importao_planilhaDAO(conn);
        cod_planilha = imp_planilha.insert(nomeplanilha, date);
        int linha = 1;
        DateTime dataReferencia = new DateTime(1900, 1, 1);
        using (var appExcel = SpreadsheetDocumentWrapper.Open(caminho))
        {
            var excelWb = appExcel.WorkbookPart.WorksheetParts["CONTRATOS"];
            foreach (var row in excelWb.Worksheet.SheetData.Rows)
            {



                if (linha >= 2)
                {
                    bool ok = false;
                    string status = "";
                    bool boleto = false;
                    bool cheque = false;
                    bool cartao = false;
                    decimal totalpag = 0;
                    bool juridica = false;
                    string contrato = "";
                    string codigocliente = "";
                    string cnpjCpf = "";
                    string cliente = "";
                    string nomeFantasia = "";
                    string endereco = "";
                    string bairro = "";
                    string numero = "";
                    string complemento = "";
                    string municipio = "";
                    string codigomunicipio = "";
                    string uf = "";
                    string cep = "";
                    string pais = "";
                    string codigopais = "";
                    string ierg = "";
                    string im = "";
                    double impostos = 0;
                    string nire = "";
                    string valortotal = "";
                    string data = "";

                    //contrato = ws.get_Range(ws.Cells[linha, 1], ws.Cells[linha, 1]).Text.ToString();
                    contrato = row.GetCell(0, true).GetValue<string>();
                    if (string.IsNullOrEmpty(contrato))
                    {
                        break;
                    }
                    //codigocliente = ws.get_Range(ws.Cells[linha, 2], ws.Cells[linha, 2]).Text.ToString();
                    codigocliente = row.GetCell(1, true).GetValue<string>();

                    //cnpjCpf = ws.get_Range(ws.Cells[linha, 3], ws.Cells[linha, 3]).Text.ToString();
                    cnpjCpf = row.GetCell(2, true).GetValue<string>();
                    cliente = row.GetCell(3, true).GetValue<string>();

                    nomeFantasia = row.GetCell(4, true).GetValue<string>();

                    endereco = row.GetCell(5, true).GetValue<string>();

                    bairro = row.GetCell(6, true).GetValue<string>();

                    numero = row.GetCell(7, true).GetValue<string>();

                    complemento = row.GetCell(8, true).GetValue<string>();

                    municipio = row.GetCell(9, true).GetValue<string>();

                    if (row.GetCell(10, true).GetValue<string>() != null)
                        codigomunicipio = row.GetCell(10, true).GetValue<string>();

                    uf = row.GetCell(11, true).GetValue<string>();

                    cep = row.GetCell(12, true).GetValue<string>();

                    pais = row.GetCell(13, true).GetValue<string>();

                    if (row.GetCell(14, true).GetValue<string>() != null)
                        codigopais = row.GetCell(14, true).GetValue<string>();

                    ierg = row.GetCell(15, true).GetValue<string>();

                    nire = row.GetCell(16, true).GetValue<string>();

                    valortotal = row.GetCell(17, true).GetValue<string>();

                    data = dataReferencia.AddDays(Convert.ToDouble(row.GetCell(18, true).GetValue<string>()) - 2).ToString("dd/MM/yyyy");

                    //status = row.Cells[20].GetValue<string>();

                    if (string.IsNullOrEmpty(contrato))
                    {
                        erros.Add("Contrato não informado na linha " + row.RowIndex);
                    }

                    if (string.IsNullOrEmpty(cliente))
                    {
                        erros.Add("Cliente não informado na linha " + row.RowIndex);
                    }

                    if (string.IsNullOrEmpty(cnpjCpf))
                    {
                        erros.Add("cnpj ou Cpf não informado na linha " + row.RowIndex);
                    }
                    else
                    {
                        cnpjCpf = cnpjCpf.Replace(",", "").Replace("-", "").Replace("/", "").Replace(".", "");
                        if (cnpjCpf.Length == 14)
                        {
                            juridica = true;
                        }
                    }

                    if (string.IsNullOrEmpty(nomeFantasia) && juridica == true)
                    {
                        erros.Add("Nome Fantasia não informado na linha " + row.RowIndex);
                    }

                    if (string.IsNullOrEmpty(endereco))
                    {
                        erros.Add("Endereço não informado na linha " + row.RowIndex);
                    }

                    if (string.IsNullOrEmpty(municipio))
                    {
                        erros.Add("Municipio não informado na linha " + row.RowIndex);
                    }

                    if (string.IsNullOrEmpty(uf))
                    {
                        erros.Add("Uf não informado na linha " + row.RowIndex);
                    }

                    if (string.IsNullOrEmpty(cep))
                    {
                        erros.Add("CEP não informado na linha " + row.RowIndex);
                    }
                    else
                    {
                        cep = cep.Replace(",", "").Replace("-", "").Replace("/", "").Replace(".", "");
                    }

                    if (string.IsNullOrEmpty(pais))
                    {
                        erros.Add("Pais não informado na linha " + row.RowIndex);
                    }

                    if (string.IsNullOrEmpty(nire))
                    {
                        erros.Add("Nire não informado na linha " + row.RowIndex);
                    }

                    if (string.IsNullOrEmpty(valortotal))
                    {
                        erros.Add("Valor Total não informado na linha " + row.RowIndex);
                    }

                    if (string.IsNullOrEmpty(data))
                    {
                        erros.Add("Data não informado na linha " + row.RowIndex);
                    }

                    if (status == "Importado")
                    {
                        erros.Add("Contrato já importado na linha " + row.RowIndex);
                    }

                    if (erros.Count == 0)
                    {

                        int coluna = 19;
                        decimal totalcartao = 0;
                        decimal totaldinheiro = 0;
                        List<Vencimento> vencimentos = new List<Vencimento>();
                        int total = 4;
                        while (coluna < row.Cells.Count)
                        {
                            int ordem = 0;
                            decimal valor;
                            DateTime vencto = new DateTime();
                            string formaPagamento;

                            if (!int.TryParse(row.GetCell(coluna, true).CellValue.Value, out ordem))
                            {
                                break;
                            }

                            decimal.TryParse(row.GetCell(coluna + 1, true).GetValue<string>().Replace(".", ","), out valor);
                            valor = Math.Round(valor, 2);

                            vencto = dataReferencia.AddDays(Convert.ToDouble(row.GetCell(coluna + 2, true).GetValue<string>()) - 2);

                            formaPagamento = row.GetCell(coluna + 3, true).GetValue<String>();

                            Vencimento v = new Vencimento();

                            totalpag = totalpag + valor;
                            v.ordem = ordem;
                            v.valor = valor;
                            v.vencto = vencto;
                            if (formaPagamento.ToUpper() == "BOLETO")
                            {
                                boleto = true;
                                totaldinheiro = totaldinheiro + valor;
                            }

                            if (formaPagamento.ToUpper() == "CHEQUE")
                            {
                                cheque = true;
                                totaldinheiro = totaldinheiro + valor;
                            }
                            if (formaPagamento.ToUpper() == "CARTAO")
                            {
                                cartao = true;
                                totalcartao = totalcartao + valor;
                            }
                            v.formaPagamento = formaPagamento.ToUpper();
                            vencimentos.Add(v);

                            coluna = coluna + total;

                        }
                        decimal valortot = Convert.ToDecimal(valortotal.Replace(".", ","));
                        if (totalpag != valortot)
                        {
                            erros.Add("Valor total das parcelas diferente do valor total na linha " + row.RowIndex);
                        }

                        if (erros.Count == 0)
                        {
                            ok = true;

                            string fj = "";
                            if (juridica == true)
                            {
                                fj = "JURIDICA";
                            }
                            else
                            {
                                fj = "FISICA";
                            }
                            if (string.IsNullOrEmpty(codigocliente) || codigocliente == "0")
                            {
                                if (string.IsNullOrEmpty(codigopais))
                                {
                                    codigopais = "0";
                                }
                                empresasDAO verificarcnpjcpf = new empresasDAO(conn);
                                codigocliente = verificarcnpjcpf.insert(SessionView.EmpresaSession, cliente, nomeFantasia, cnpjCpf,
                                                endereco, numero, complemento, "", cep, municipio, "", ierg, im, impostos, uf, "CLIENTE_FORNECEDOR", 0, 0, fj, 0, 0,
                                                false, codigomunicipio, codigopais, nire, 0, false, null, null, 0).ToString();

                            }


                            //CARREGANDO DADOS
                            Job jobclass = new Job(conn);
                            jobclass.codigo = job;
                            jobclass.load();
                            LinhaNegocio linhaNegocioClass = new LinhaNegocio(conn);
                            linhaNegocioClass.codigo = jobclass.linhaNegocio;
                            linhaNegocioClass.load();
                            Divisao divisaoclass = new Divisao(conn);
                            divisaoclass.codigo = jobclass.divisao;
                            divisaoclass.load();
                            Cliente clienteclass = new Cliente(conn);
                            clienteclass.codigo = jobclass.cliente;
                            clienteclass.load();
                            ContaContabil contacontabilclass = new ContaContabil(conn);
                            contacontabilclass.codigo = contacartao;
                            string descricaocontacartao = contacontabilclass.codigo;
                            contacontabilclass.codigo = contachequeboleto;
                            string descricaocontadinheiro = contacontabilclass.codigo;
                            contacontabilclass.codigo = contareceita;
                            string descricaocontareceita = contacontabilclass.codigo;


                            //CRIANDO PRIMEIRO LANÇAMENTO
                            SLancamento prim_lancamento = new SLancamento();
                            prim_lancamento.seqLote = 1;
                            prim_lancamento.debCred = 'D';
                            prim_lancamento.descConsultor = "";
                            prim_lancamento.historico = historico;
                            prim_lancamento.modelo = 1;
                            prim_lancamento.numeroDocumento = contrato;
                            prim_lancamento.dataLancamento = Convert.ToDateTime(data);
                            prim_lancamento.job = jobclass.codigo;
                            prim_lancamento.descJob = jobclass.descricao;
                            prim_lancamento.linhaNegocio = jobclass.linhaNegocio;
                            prim_lancamento.descLinhaNegocio = linhaNegocioClass.descricao;
                            prim_lancamento.divisao = jobclass.divisao;
                            prim_lancamento.descDivisao = divisaoclass.descricao;
                            prim_lancamento.descCliente = clienteclass.nomeFantasia;
                            prim_lancamento.cliente = clienteclass.codigo;
                            prim_lancamento.qtd = 1;
                            prim_lancamento.valorBruto = 0;
                            prim_lancamento.descTerceiro = cliente;
                            prim_lancamento.terceiro = Convert.ToInt32(codigocliente);
                            prim_lancamento.titulo = true;
                            prim_lancamento.codPlanilha = cod_planilha;

                            bool criaUltimoLancto = false;
                            if (totalcartao > 0)
                            {
                                prim_lancamento.valor = totalcartao;
                                prim_lancamento.valorUnit = totalcartao;
                                prim_lancamento.conta = contacartao;
                                prim_lancamento.descConta = _descricaocontacartao;
                                prim_lancamento.vencimentos = new List<SVencimento>();
                                for (int x = 0; x < vencimentos.Count; x++)
                                {
                                    if (!(vencimentos[x].formaPagamento == "BOLETO" || vencimentos[x].formaPagamento == "CHEQUE"))
                                    {
                                        SVencimento venc = new SVencimento(vencimentos[x].vencto, vencimentos[x].valor);
                                        prim_lancamento.vencimentos.Add(venc);
                                    }
                                }

                                if (totaldinheiro > 0)
                                    criaUltimoLancto = true;
                            }
                            else
                            {
                                prim_lancamento.valor = totaldinheiro;
                                prim_lancamento.valorUnit = totaldinheiro;
                                prim_lancamento.conta = contachequeboleto;
                                prim_lancamento.vencimentos = new List<SVencimento>();
                                for (int x = 0; x < vencimentos.Count; x++)
                                {
                                    if (vencimentos[x].formaPagamento == "BOLETO" || vencimentos[x].formaPagamento == "CHEQUE")
                                    {
                                        SVencimento venc = new SVencimento(vencimentos[x].vencto, vencimentos[x].valor);
                                        prim_lancamento.vencimentos.Add(venc);
                                    }
                                }
                            }

                            arr.Add(prim_lancamento);
                            //CRIANDO SEGUNDO LANÇAMENTO
                            SLancamento segun_lancamento = new SLancamento();
                            segun_lancamento.seqLote = 2;
                            segun_lancamento.debCred = 'C';
                            segun_lancamento.descConsultor = "";
                            segun_lancamento.historico = historico;
                            segun_lancamento.modelo = 1;
                            segun_lancamento.numeroDocumento = contrato;
                            segun_lancamento.dataLancamento = Convert.ToDateTime(data);
                            segun_lancamento.job = jobclass.codigo;
                            segun_lancamento.descJob = jobclass.descricao;
                            segun_lancamento.linhaNegocio = jobclass.linhaNegocio;
                            segun_lancamento.descLinhaNegocio = linhaNegocioClass.descricao;
                            segun_lancamento.divisao = jobclass.divisao;
                            segun_lancamento.descDivisao = divisaoclass.descricao;
                            segun_lancamento.descCliente = clienteclass.nomeFantasia;
                            segun_lancamento.cliente = clienteclass.codigo;
                            segun_lancamento.qtd = 1;
                            segun_lancamento.valorBruto = 0;
                            segun_lancamento.descTerceiro = cliente;
                            segun_lancamento.terceiro = Convert.ToInt32(codigocliente);
                            segun_lancamento.titulo = false;

                            segun_lancamento.valor = totalpag;
                            segun_lancamento.valorUnit = totalpag;
                            segun_lancamento.conta = contareceita;
                            segun_lancamento.descConta = _descricaocontareceita;
                            segun_lancamento.codPlanilha = cod_planilha;
                            arr.Add(segun_lancamento);

                            //CRIANDO TERCEIRO LANÇAMENTO
                            if (criaUltimoLancto)
                            {
                                SLancamento terc_lancamento = new SLancamento();
                                terc_lancamento.seqLote = 3;
                                terc_lancamento.debCred = 'D';
                                terc_lancamento.descConsultor = "";
                                terc_lancamento.historico = historico;
                                terc_lancamento.modelo = 1;
                                terc_lancamento.numeroDocumento = contrato;
                                terc_lancamento.dataLancamento = Convert.ToDateTime(data);
                                terc_lancamento.job = jobclass.codigo;
                                terc_lancamento.descJob = jobclass.descricao;
                                terc_lancamento.linhaNegocio = jobclass.linhaNegocio;
                                terc_lancamento.descLinhaNegocio = linhaNegocioClass.descricao;
                                terc_lancamento.divisao = jobclass.divisao;
                                terc_lancamento.descDivisao = divisaoclass.descricao;
                                terc_lancamento.descCliente = clienteclass.nomeFantasia;
                                terc_lancamento.cliente = clienteclass.codigo;
                                terc_lancamento.qtd = 1;
                                terc_lancamento.descTerceiro = cliente;
                                terc_lancamento.terceiro = Convert.ToInt32(codigocliente);
                                terc_lancamento.titulo = true;

                                terc_lancamento.valor = totaldinheiro;
                                terc_lancamento.valorUnit = totaldinheiro;
                                terc_lancamento.conta = contachequeboleto;
                                terc_lancamento.descConta = _descricaocontachequeboleto;
                                terc_lancamento.codPlanilha = cod_planilha;
                                terc_lancamento.valorBruto = 0;
                                terc_lancamento.vencimentos = new List<SVencimento>();
                                for (int x = 0; x < vencimentos.Count; x++)
                                {
                                    SVencimento venc = new SVencimento(vencimentos[x].vencto, vencimentos[x].valor);
                                    terc_lancamento.vencimentos.Add(venc);
                                }
                                arr.Add(terc_lancamento);
                            }
                        }

                        FolhaLancamento folha = new FolhaLancamento(conn, arr, "CAR_INCLUSAO_TITULO", Convert.ToDateTime(data));
                        List<string> errosFolha = folha.salvar(null);

                        //ws.Cells[linha, 20] = "Importado";
                    }
                    importao_planilhaDAO import = new importao_planilhaDAO(conn);
                    import.insert_planilha(cod_planilha, contrato, codigocliente, cnpjCpf, cliente, nomeFantasia, endereco, bairro, numero, complemento, municipio, codigomunicipio, uf, cep, pais, codigopais, ierg, nire, valortotal, data, ok);
                }
                errosGeral.AddRange(erros);
                erros.Clear();
                arr.Clear();
                linha++;
            }
        }


        //foreach (Microsoft.Office.Interop.Excel.Worksheet ws in excelWb.Worksheets)
        //{
        //    int linha = 2;

        //    if (ws.Name == "CONTRATOS")
        //    {
        //        while (1 == 1)
        //        {

        //        }
        //    }
        //}
        return errosGeral;
    }
}
//}