using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
/// <summary>
/// Summary description for SessionView
/// </summary>
public class SessionView
{
    public static int UsuarioSession
    {
        get
        {
            if (HttpContext.Current.Session["usuario"] == null)
            {
                return 0;
            }

            int empresa = 0;
            int.TryParse(HttpContext.Current.Session["usuario"].ToString(), out empresa);
            return empresa;
        }

        set
        {
            HttpContext.Current.Session["usuario"] = value;
        }
    }

    public static int EmpresaSession
    {
        get 
        {
            if (HttpContext.Current.Session["empresa"] == null)
            {
                return 0;
            }

            int empresa = 0;
            int.TryParse(HttpContext.Current.Session["empresa"].ToString(), out empresa);
            return empresa;
        }

        set
        {
            HttpContext.Current.Session["empresa"] = value;
        }
    }

    public static SNotaFiscal NotaFiscalSession
    {
        get
        {
            if (HttpContext.Current.Session["ss_nota_fiscal"] == null)
            {
                return null;
            }

            return (SNotaFiscal)HttpContext.Current.Session["ss_nota_fiscal"];
        }

        set
        {
            HttpContext.Current.Session["ss_nota_fiscal"] = value;
        }
    }
}