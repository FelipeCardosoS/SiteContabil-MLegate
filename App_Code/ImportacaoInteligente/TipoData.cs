using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Data
/// </summary>
/// 
namespace ImportacaoInteligente
{
    [Serializable]
    public class TipoData : TipoColunaAbstract
    {
        public override bool valida()
        {
            DateTime result = new DateTime();
            limpa();
            return (DateTime.TryParse(arruma(), out result));
        }

        public override void limpa()
        {
            //nada implementado
        }

        public string arruma()
        {
            string novaData = "";

            if (value.Contains("/"))
            {
                string[] arr = value.Trim().Split('/');
                string dia = ("00" + arr[0]).Substring(("00" + arr[0]).Length - 2, 2);
                string mes = ("00" + arr[1]).Substring(("00" + arr[1]).Length - 2, 2);
                string ano = ("0000" + arr[2]).Substring(("0000" + arr[2]).Length - 4, 4);
                novaData = dia + "/" + mes + "/" + ano;
            }
            else
            {
                value = ("00000000" + value.Trim()).Substring(("00000000" + value.Trim()).Length - 8, 8);
                novaData = value.Substring(0, 2) + "/" + value.Substring(2, 2) + "/" + value.Substring(4, 4);
            }


            return novaData;
        }

        public override string ToString()
        {
            string novaData = "";
            if (value.Length > 0)
            {

                if (value.Contains("/"))
                {
                    string[] arr = value.Trim().Split('/');
                    if (arr.Length == 3)
                    {
                        string dia = ("00" + arr[0]).Substring(("00" + arr[0]).Length - 2, 2);
                        string mes = ("00" + arr[1]).Substring(("00" + arr[1]).Length - 2, 2);
                        string ano = ("0000" + arr[2]).Substring(("0000" + arr[2]).Length - 4, 4);
                        novaData = ano + mes + dia;
                    }
                }
                else
                {
                    value = ("00000000" + value.Trim()).Substring(("00000000" + value.Trim()).Length - 8, 8);
                    novaData = value.Substring(4, 4) + value.Substring(2, 2) + value.Substring(0, 2);
                }
            }
            return novaData;
        }
    }
}