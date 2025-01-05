using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{
	public string GetData(int value)
	{
		return string.Format("You entered: {0}", value);
	}

	

	public CompositeType GetDataUsingDataContract(CompositeType composite)
	{
		if (composite == null)
		{
			throw new ArgumentNullException("composite");
		}
		if (composite.BoolValue)
		{
			composite.StringValue += "Suffix";
		}
		return composite;
	}

    public string GetWeather(string description, double degree)
    {
        using (HttpClient client = new HttpClient())
        {
            var payload = new {Description = description ,Degree = degree};
            HttpResponseMessage response = client.PostAsJsonAsync("http://localhost:5240/api/weather/receive", payload).Result;


            if (response.IsSuccessStatusCode)
            {
                return "Weather status forwarded to ASP.NET successfully.";
            }
            else
            {

                return (response.ReasonPhrase);
            }
        }
    }
}
