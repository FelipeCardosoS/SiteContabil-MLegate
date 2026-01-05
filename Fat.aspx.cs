using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Data;

public partial class Fat : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string server = "";
        string bd = ""; 
        string login = "";
        string pass="";
        int reg = 0;
        Session["empresa"] = 1;
        Session["usuario"] = 1;

        server = Request.QueryString["server"].ToString();
        bd = Request.QueryString["bd"].ToString();
        login = Request.QueryString["login"].ToString();
        pass = Request.QueryString["pass"].ToString();
        int.TryParse(Request.QueryString["reg"].ToString(), out reg);

        //server = "192.168.0.10";
        //bd = "fat_web";
        //login = "sa";
        //pass = "Managers2b";
        //int.TryParse("4634", out reg);

        Response.Write(server);
        Response.Write(bd);
        Response.Write(login);
        Response.Write(pass);
        Response.Write(reg);

        string strConexao = "Data Source=" + server + ";Initial Catalog=" + bd + ";User ID=" + login + ";Password=" + pass + ";";
        Conexao conn = null;
        Conexao connContabil = null;
        List<string> erros = new List<string>();
        List<SLancamento> lanctos = new List<SLancamento>();
        DateTime data_emissao = DateTime.Now;
        try
        {
            conn = new Conexao(strConexao);
            connContabil = new Conexao(); ;

            DataTable tblanctos = conn.dataTable("select * from contabil where reg=" + reg, " order by seqlote");


            foreach (DataRow row in tblanctos.Rows)
            {
                SLancamento lancto = new SLancamento();
                lancto.seqLote = Convert.ToInt32(row["seqlote"]);
                lancto.debCred = Convert.ToChar(row["debcred"]);
                lancto.conta = row["conta"].ToString();
                lancto.valor = Convert.ToDecimal(row["valor"]);
                lancto.dataLancamento = Convert.ToDateTime(row["dataLancamento"]);
                data_emissao = Convert.ToDateTime(row["dataLancamento"]);
                lancto.historico = row["historico"].ToString();

                //lancto.job = Convert.ToInt32(row["job"]);
                Job job = new Job(connContabil);
                try
                {
                    job.dePara(Convert.ToInt32(row["job"]));
                    lancto.job = job.codigo;
                    Cliente cliente1 = new Cliente(connContabil);
                    cliente1.codigo = job.cliente;
                    cliente1.load();
                    lancto.cliente = job.cliente;
                    lancto.descCliente = cliente1.nomeFantasia;
                    lancto.descJob = cliente1.nomeFantasia + " - " + job.descricao;
                    lancto.linhaNegocio = job.linhaNegocio;
                    lancto.divisao = job.divisao;
                }
                catch (Exception ex)
                {
                    erros.Add(ex.StackTrace);
                }

                //lancto.divisao = Convert.ToInt32(row["divisao"]);
                Divisao divisao = new Divisao(connContabil);
                try
                {
                    divisao.dePara((int)lancto.divisao);
                    lancto.divisao = divisao.codigo;
                    lancto.descDivisao = divisao.descricao;
                }
                catch (Exception ex)
                {
                    erros.Add(ex.StackTrace);
                }

                //lancto.linhaNegocio = Convert.ToInt32(row["linhaNegocio"]);
                LinhaNegocio linhaNegocio = new LinhaNegocio(connContabil);
                try
                {
                    linhaNegocio.dePara((int)lancto.linhaNegocio);
                    lancto.linhaNegocio = linhaNegocio.codigo;
                    lancto.descLinhaNegocio = linhaNegocio.descricao;
                }
                catch (Exception ex)
                {
                    erros.Add(ex.StackTrace);
                }

                //lancto.cliente = Convert.ToInt32(row["cliente"]);
                //Cliente cliente = new Cliente(connContabil);
                //try
                //{
                //    cliente.dePara(Convert.ToInt32(row["cliente"]));
                //    lancto.cliente = cliente.codigo;
                //    lancto.descCliente = cliente.nomeFantasia;
                //}
                //catch (Exception ex)
                //{
                //    erros.Add(ex.StackTrace);
                //}

                int titulo = -1;
                int.TryParse(row["titulo"].ToString(), out titulo);

                if (titulo < 0)
                {
                    lancto.titulo = true;
                    SVencimento vencimento = new SVencimento(Convert.ToDateTime(row["dataVencimento"]), Convert.ToDecimal(row["valor"]));
                    lancto.vencimentos = new List<SVencimento>();
                    lancto.vencimentos.Add(vencimento);

                    Empresa emp = new Empresa(connContabil);
                    emp.codigo = Convert.ToInt32(row["terceiro"]);
                    emp.load();
                    lancto.terceiro = emp.codigo;
                    lancto.descTerceiro = emp.nome;
                }
                else
                {
                    lancto.titulo = false;
                }


                lancto.qtd = Convert.ToDecimal(row["qtd"]);
                lancto.valorUnit = Convert.ToDecimal(row["valorunit"]);
                lancto.terceiro = Convert.ToInt32(row["terceiro"]);
                lancto.numeroDocumento = row["numeroDocumento"].ToString();
                lancto.modelo = Convert.ToInt32(row["modelo"]);
                lancto.modulo = "CAR_INCLUSAO_TITULO";
                try
                {
                    lancto.valorBruto = Convert.ToDecimal(row["VALORBRUTO"]);
                }
                catch
                {
                    lancto.valorBruto = 0;
                }
                lanctos.Add(lancto);
            }

            FolhaLancamento f = new FolhaLancamento(connContabil, lanctos, "CAR_INCLUSAO_TITULO", data_emissao);
            erros.AddRange(f.salvar(null));
            if (erros.Count == 0)
            {
                conn.execute("insert into logerro(reg, erro, data)values(" + reg + ",'Sucesso!','" + DateTime.Now.ToString("yyyyMMdd H:mm:ss") + "');");
            }
        }
        catch (Exception ex)
        {
            erros.Add(ex.StackTrace);
        }
        finally
        {
            
            foreach (string erro in erros)
            {
                Response.Write(erro);
                conn.execute("insert into logerro(reg, erro, data)values(" + reg + ",'" + erro + "','" + DateTime.Now.ToString("yyyyMMdd H:mm:ss") + "');");
            }
        }
    }

    [WebMethod]
    public void BuscaFaturamento()
    {
        
    }
}