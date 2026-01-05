using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web;
using System.Text;

namespace MansoftValidators
{
	/// <summary>
	/// Summary description for SelectiveValidator.
	/// </summary>
	public class SelectiveValidator : BaseValidator
	{
		
		/// <summary>
		/// Validator that is going to be target of enabling/disabling
		/// </summary>
		public string ValidatorToBeDisabled
		{
			get
			{
				object o=ViewState[this.UniqueID + "_released"];
				return (o==null)? String.Empty : (string)o;
			}
			set
			{
				ViewState[this.UniqueID + "_released"]=value;
			}	
		}

		/// <summary>
		/// Value that is used when determined when validation is disabled
		/// </summary>
		public string DisableValue
		{
			get
			{
				object o=ViewState[this.UniqueID + "_InitValue"];
				return (o==null)? String.Empty : (string)o;
			}
			set
			{
				ViewState[this.UniqueID + "_InitValue"]=value;
			}	
		}

		/// <summary>
		/// Value thet is used to determine if validation for this validator will fail or not
		/// </summary>
		public string InitialValue
		{
			get
			{
				object o=ViewState[this.UniqueID +  "_initial"];
				return (o==null)? String.Empty : (string)o;
			}
			set
			{
				ViewState[this.UniqueID +  "_initial"]=value;
			}
		}

		//Enable client-side validation by naming the function
		protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
			if(RenderUplevel)
			{
	
				writer.AddAttribute("evaluationfunction","SelectiveValidatorEvaluateIsValid");
			}
		}


		//Override to check properties
		protected override bool ControlPropertiesValid()
		{

			//Target validator not specified
			if(ValidatorToBeDisabled.Equals(String.Empty))
			{
				throw new
					ArgumentException("ValidatorToBeDisabled cannot be empty","ControlToBeReleased")				; 
			}

			Control c=Page.FindControl(ValidatorToBeDisabled);
			//Target validator does not exist
			if(c==null)
			{
				throw new ArgumentException("ValidatorToBeDisabled must be specified","ValidatorToBeDisabled"); 
			}

			//Target control is not BaseValidator or derived from it
			if(!(c is BaseValidator)) 
			{
				throw new ArgumentException("ValidatorToBeDisabled must be valid validator","ValidatorToBeDisabled"); 
			}

			//Otherwise let base class decide
			return base.ControlPropertiesValid();
		}


		//Server-side validation logic
		protected override bool EvaluateIsValid()
		{
			//Get the validator
			BaseValidator validator=(BaseValidator)Page.FindControl(ValidatorToBeDisabled);

			//Get the value of control being validated and check if it equals to value when
			//validation should be disabled
			string valuetobevalidated=GetControlValidationValue(ControlToValidate).Trim();

			if(valuetobevalidated.Equals(DisableValue))
			{
				//If value equals "disabling" value
				//disable control and return true;
				validator.Enabled =false;
				return true;
			}
			else
			{
				
				//Check that mandatory selections have been made at Control to be validated
				if(valuetobevalidated.Equals(InitialValue))
				{
					validator.Enabled =false;
					//Return false from our validator
					return false;
				}
				else
				{
					//enable validator
					validator.Enabled =true;
					//Do validation
					validator.Validate(); 

					//Return true from our validator
					return true;
				}
				

				
			}
			
		}

		

		//Overridden OnPreRender
		protected override void OnPreRender(System.EventArgs e)
		{
			base.OnPreRender(e);

			//Client-side validation if browser is uplevel
			if(RenderUplevel)
			{
				
				//Get the clientid of "ValidatorToBeDisabled"
				string clientidofvalidator=Page.FindControl(ValidatorToBeDisabled).ClientID;

				StringBuilder sb=new StringBuilder();
				sb.Append("<script language='javascript'>\r\n");

				//Client-side validation function that follows same logic as server-side
				//validation
				sb.Append("function SelectiveValidatorEvaluateIsValid(val){\r\n");

				//Compare validated control's value into value when
				//validation should be disabled
				sb.Append("if(trim(ValidatorGetValue(val.controltovalidate))==");
				sb.Append("'");
				sb.Append(DisableValue);
				sb.Append("'){\r\n");
	
				//Disable the target validator
				sb.Append("ValidatorEnable(");
				sb.Append(clientidofvalidator);
				sb.Append(",false);\r\n");
			
				//return true for our validator
				sb.Append("return true;\r\n");
				sb.Append("}\r\n");
				
				//Compare validated control's value into initial value
				sb.Append("else{\r\n");
				sb.Append("if(trim(ValidatorGetValue(val.controltovalidate))==");
				sb.Append("'");
				sb.Append(InitialValue);
				sb.Append("'){\r\n");

				//If they equal disable the target validator
				sb.Append("ValidatorEnable(");
				sb.Append(clientidofvalidator);
				sb.Append(",false);\r\n");
			
				//return false (because manadatory selections/input at target control has not been done)
				sb.Append("return false;\r\n");
				sb.Append("}\r\n");



				//Enable target validator
				sb.Append("ValidatorEnable(");
				sb.Append(clientidofvalidator);
				sb.Append(");\r\n");
				sb.Append("ValidatorValidate(");
				sb.Append(clientidofvalidator);
				sb.Append(");\r\n");
				sb.Append("}\r\n");

				//return true from our validator
				sb.Append("return true;\r\n");

				
				sb.Append("}\r\n");

				//Create js function to trim input to be checked
				sb.Append("function trim(strText) {\r\n");
				sb.Append("while (strText.substring(0,1) == ' ')\r\n"); 
				sb.Append("strText = strText.substring(1, strText.length);\r\n");
				sb.Append("while (strText.substring(strText.length-1,strText.length)== ' ')\r\n");
				sb.Append("strText = strText.substring(0, strText.length-1);\r\n");
				sb.Append("return strText;\r\n");
				sb.Append("}\r\n");
				sb.Append("</script>\r\n");
				
				Page.RegisterClientScriptBlock(typeof(SelectiveValidator).FullName,sb.
					ToString());  

			}
		}

		
	}
}