using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ColunaCpf
/// </summary>
/// 
namespace ImportacaoInteligente
{
    [Serializable]
    public class ColunaCpf : TipoColunaAbstract
    {
        public ColunaCpf()
        {
        }

        public override bool valida()
        {

            limpa();
            return true;
        }

        public override void limpa()
        {
            value = value.Replace("\\", "").Replace("'", "").Replace(".", "").Replace("-", "");
        }

        public override string ToString()
        {
            value = value.Trim().Replace("\\", "").Replace("'", "").Replace(".", "").Replace("-", "");
            value = ("00000000000" + value.Trim()).Substring(("00000000000" + value.Trim()).Length - "00000000000".Length, "00000000000".Length);
            return value;

        }
    }
}