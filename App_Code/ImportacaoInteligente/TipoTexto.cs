using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Texto
/// </summary>
/// 
namespace ImportacaoInteligente
{
    [Serializable]
    public class TipoTexto : TipoColunaAbstract
    {
        public override bool valida()
        {
            limpa();
            return true;
        }

        public override void limpa()
        {
            value = value.Replace("\\", "").Replace("'", "");
        }

        public override string ToString()
        {
            return value.Trim().Replace("\\", "").Replace("'", "");
        }
    }
}