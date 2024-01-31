using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace GovermentApisConsumption
{
    class Program
    {
        static void Main(string[] args)
        {
            MyClass obj = new MyClass();
            //obj.UdyogAadharApi("AASHARNO");
            //.IndianBankAccountVerificationApi("ACCOUNTNO", "IFCS CODE");
            //obj.GSTVerificationApi("GSTNo");
            //obj.PINCodeVerificationApi("PINCODE");
        }
    }

    class MyClass
    {
        public void UdyogAadharApi(string uam_number)
        {

            string apiUri = "https://udyog-aadhaar-verification.p.rapidapi.com/v3/tasks/sync/verify_with_source/udyog_aadhaar";

            var myObject = new
            {
                task_id = "74f4c926-250c-43ca-9c53-453e87ceacd1",
                group_id = "8e16424a-58fc-4ba4-ab20-5bc8e7c3c41e",
                data = new
                {
                    uam_number = uam_number,
                }
            };

            var client = new RestClient(apiUri);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            request.AddHeader("X-RapidAPI-Key", "1951f321admshd20b97f94dd21c2p131b84jsne342186f85b2");
            request.AddHeader("X-RapidAPI-Host", "udyog-aadhaar-verification.p.rapidapi.com");
            string Content = JsonConvert.SerializeObject(myObject);
            request.AddParameter("application/json", Content, ParameterType.RequestBody);
            DateTime startTime = DateTime.Now;
            Console.WriteLine($"Start Time: {startTime}");
            IRestResponse response = client.Execute(request);
            var resp = JsonConvert.DeserializeObject<IDictionary>(response.Content);
            DateTime endtime = DateTime.Now;
            Console.WriteLine($"End Time: {endtime}");
            Console.ReadKey();

        }
        public void IndianBankAccountVerificationApi(string AccountNo, string BankIFSC)
        {
            try
            {

                string apiUriGET = "https://indian-bank-account-verification.p.rapidapi.com/v3/tasks?request_id={0}";


                var myObject = new
                {
                    task_id = "123",
                    group_id = "1234",
                    data = new
                    {
                        bank_account_no = AccountNo,
                        bank_ifsc_code = BankIFSC
                    }
                };

                // POST call 
                var clientPOST = new RestClient("https://indian-bank-account-verification.p.rapidapi.com/v3/tasks/async/verify_with_source/validate_bank_account");
                clientPOST.Timeout = -1;
                var requestPOST = new RestRequest(Method.POST);
                requestPOST.AddHeader("X-RapidAPI-Key", "1951f321admshd20b97f94dd21c2p131b84jsne342186f85b2");
                requestPOST.AddHeader("X-RapidAPI-Host", "indian-bank-account-verification.p.rapidapi.com");
                string Content = JsonConvert.SerializeObject(myObject);
                requestPOST.AddParameter("application/json", Content, ParameterType.RequestBody);
                IRestResponse responsePOST = clientPOST.Execute(requestPOST);
                if (responsePOST.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    var resp = JsonConvert.DeserializeObject<IDictionary>(responsePOST.Content);
                    string RequestID = Convert.ToString(resp["request_id"]);
                    var clientGET = new RestClient(string.Format(apiUriGET, RequestID));
                    var requestGET = new RestRequest(Method.GET);
                    requestGET.AddHeader("X-RapidAPI-Key", "1951f321admshd20b97f94dd21c2p131b84jsne342186f85b2");
                    requestGET.AddHeader("X-RapidAPI-Host", "indian-bank-account-verification.p.rapidapi.com");
                    IRestResponse responseGET = clientGET.Execute(requestGET);
                    var res = responseGET.Content;
                    if (responseGET.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var respGET = JsonConvert.DeserializeObject<IDictionary>(res.Replace("[", "").Replace("]", ""));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public string GSTVerificationApi(string GSTNO)
        {
            string apiUri = "https://gst-verification.p.rapidapi.com/v3/tasks/sync/verify_with_source/ind_gst_certificate";

            var myObject = new
            {
                task_id = "74f4c926-250c-43ca-9c53-453e87ceacd1",
                group_id = "8e16424a-58fc-4ba4-ab20-5bc8e7c3c41e",
                data = new
                {
                    gstin = GSTNO,
                }
            };
            var client = new RestClient(apiUri);
            client.Timeout = -1;
            var request = new RestRequest();

            request.AddHeader("content-type", "application/json");
            request.AddHeader("X-RapidAPI-Key", "1951f321admshd20b97f94dd21c2p131b84jsne342186f85b2");
            request.AddHeader("X-RapidAPI-Host", "gst-verification.p.rapidapi.com");
            string Content = JsonConvert.SerializeObject(myObject);
            request.AddParameter("application/json", Content, ParameterType.RequestBody);
            IRestResponse response = client.ExecuteAsPost(request, "POST");
            if (Convert.ToString(response.StatusCode) == "OK")
            {
                var resp = JsonConvert.DeserializeObject<IDictionary>(response.Content);
                return Convert.ToString(resp["status"]);
            }
            return null;
        }
        public string PINCodeVerificationApi(string PinCode)
        {
            string apiUri = "https://india-pincode-with-latitude-and-longitude.p.rapidapi.com/api/v1/pincode/{0}";

            var client = new RestClient(string.Format(apiUri, PinCode));
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-RapidAPI-Key", "1951f321admshd20b97f94dd21c2p131b84jsne342186f85b2");
            request.AddHeader("X-RapidAPI-Host", "india-pincode-with-latitude-and-longitude.p.rapidapi.com");
            IRestResponse response = client.Execute(request);
            if (Convert.ToString(response.StatusCode) == "OK")
            {
                var resp = JsonConvert.DeserializeObject<IDictionary>(response.Content.Replace("[", "").Replace("]", ""));
                return Convert.ToString(resp["status"]);
            }
            return null;
        }
    }
}
