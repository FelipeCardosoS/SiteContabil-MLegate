using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Valor
/// </summary>
/// 
namespace ImportacaoInteligente
{
    [Serializable]
    public class TipoValor : TipoColunaAbstract
    {
        public override object nulo
        {
            get { return 0; }
        }

        public override string concatenador
        {
            get { return ""; }
        }

        public override bool valida()
        {
            decimal result = 0;
            limpa();
            return (decimal.TryParse(value, out result));
        }

        public override void limpa()
        {
        }

        public override string ToString()
        {
            return Convert.ToDecimal(value.Trim().Replace("\\", "").Replace("'", "")).ToString().Replace(",", ".");
        }
    }
}
