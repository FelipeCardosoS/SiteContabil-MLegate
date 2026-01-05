using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
/// <summary>
/// Summary description for AbstractGeracaoSped
/// </summary>
/// 
namespace Sped
{
    public abstract class AbstractGeracaoSped
    {
        protected DateTime _inicio;
        protected DateTime _termino;
        protected bool _LucroPresumido;
        protected bool _DetCentroCusto;
        protected int _codEmpresa;
        protected Conexao _conn;
        protected List<BlocoSped> _blocos = new List<BlocoSped>();
        protected StreamWriter writer = null;
        protected DataRow rowEmpresa = null;
        public const int PisCofins = 1;
        public const int Contabil = 2;
        protected string _caminho = "";
        protected string _TipoDemonstracao = "";

        public DateTime inicio
        {
            get { return _inicio; }
            set { _inicio = value; }
        }

        public bool LucroPresumido
        {
            get { return _LucroPresumido; }
            set { _LucroPresumido = value; }
        }
       
        public bool DetCentroCusto
        {
            get { return _DetCentroCusto; }
            set { _DetCentroCusto = value; }
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

        protected AbstractGeracaoSped(DateTime inicio, DateTime termino, int codEmpresa, Conexao conn, bool LucroPresumido, bool DetCentroCusto)
        {
            _inicio = inicio;
            _termino = termino;
            _codEmpresa = codEmpresa;
            _conn = conn;
            _LucroPresumido = LucroPresumido;
            _DetCentroCusto = DetCentroCusto;
        }

        public bool gerar(string caminho)
        {
            _caminho = caminho;
            
            using (writer = new StreamWriter(caminho, false, System.Text.Encoding.Default))
            {
                empresasDAO empresaDAO = new empresasDAO(_conn);
                DataTable tbEmpresa = empresaDAO.load(_codEmpresa);
                if (tbEmpresa.Rows.Count == 0)
                {
                    throw new ApplicationException("Nenhuma empresa encontrada.");
                }

                rowEmpresa = tbEmpresa.Rows[0];
                executaBlocos();
                return true;
            }
        }

        public abstract void executaBlocos();

        public abstract string bloco0();

        public abstract string bloco9();

        protected string formatData(DateTime data)
        {
            return data.ToString("ddMMyyyy");
        }

        protected string formatNumero(double numero)
        {
            if (numero < 0)
                numero = numero * -1;
            return String.Format("{0:0.00}", numero).Replace(".",",");
        }

        protected string clearTxt(string txt)
        {
            return txt.Replace(".", "").Replace("/", "").Replace("-", "");
        }
    }
}