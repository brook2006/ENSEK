using RestSharp;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace API_Tests
{
    [TestClass]
    public class ApiTests
    {
        private const string BaseUrl = "https://qacandidatetest.ensek.io";
        private RestClient _client;

        [TestInitialize]
        public void Setup()
        {
            _client = new RestClient(BaseUrl);
        }

        [TestMethod]
        public void Test_BuyElectricity_Success()
        {
            RestRequest request = new RestRequest("/ENSEK/buy/3/10", Method.Put);
            RestResponse response = _client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// This is an example test that creates an order and then verifies the responce message.
        /// </summary>
        [TestMethod]
        public void Test_BuyGas_Success()
        {
            RestRequest request = new RestRequest("/ENSEK/buy/1/10", Method.Put);
            RestResponse response = _client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Content);
            JObject jsonContent = JObject.Parse(response.Content);
            string message = jsonContent["message"].ToString();
            string messagePattern = @"You have purchased \d+ m³ at a cost of \d+(\.\d+)? there are \d+ units remaining\. Your order id is \w+-\w+-\w+-\w+-\w+\.";
            Assert.IsTrue(Regex.IsMatch(message, messagePattern));
        }

        [TestMethod]
        public void Test_BuyNuclear_Success()
        {
            RestRequest request = new RestRequest("/ENSEK/buy/2/10", Method.Put);
            RestResponse response = _client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void Test_BuyOil_Success()
        {
            RestRequest request = new RestRequest("/ENSEK/buy/4/10", Method.Put);
            RestResponse response = _client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// This is a negative test that verifies the API returns NotFound if the order can't be found.
        /// 
        /// BUG: The API incorrectly returns InternalServerError rather than NotFound
        /// </summary>
        [TestMethod]
        public void Test_DeleteOrder_Failure()
        {
            string OrderToDelete = "aaaaaaaaa";
            RestRequest request = new RestRequest($"/ENSEK/orders/{OrderToDelete}", Method.Delete);
            RestResponse response = _client.Execute(request);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        /// <summary>
        /// BUG: The delete order does not work and instead returns Internal Server Error
        /// </summary>
        [TestMethod]
        public void Test_DeleteOrder_Success()
        {
            string OrderToDelete = GetOrderID();
            RestRequest request = new RestRequest($"/ENSEK/orders/{OrderToDelete}", Method.Delete);
            RestResponse response = _client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void Test_GetEnergyTypes_Success()
        {
            RestRequest request = new RestRequest("/ENSEK/energy", Method.Get);
            RestResponse response = _client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// BUG: This method isn't working
        /// </summary>
        [TestMethod]
        public void Test_GetOrderById_Success()
        {
            string orderToRetrieve = GetOrderID();
            RestRequest request = new RestRequest($"/ENSEK/orders/{orderToRetrieve}", Method.Get);
            RestResponse response = _client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void Test_GetOrders_Success()
        {
            RestRequest request = new RestRequest("/ENSEK/orders", Method.Get);
            RestResponse response = _client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Posative login test
        /// </summary>
        [TestMethod]
        public void Test_Login_Success()
        {
            RestRequest request = new RestRequest("/ENSEK/login", Method.Post);
            request.AddJsonBody(new { username = "test", password = "testing" });
            RestResponse response = _client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Negative login test
        /// </summary>
        [TestMethod]
        public void Test_Login_Unautharised()
        {
            RestRequest request = new RestRequest("/ENSEK/login", Method.Post);
            request.AddJsonBody(new { username = "Unautharised", password = "Unautharised" });
            RestResponse response = _client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        /// <summary>
        /// BUG: This returns unauthorised, it might require a username and password
        /// </summary>
        [TestMethod]
        public void Test_ResetData_Success()
        {
            RestRequest request = new RestRequest("/ENSEK/reset", Method.Post);
            RestResponse response = _client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        private string GetOrderID(string energyType = "1", string quantity = "10")
        {
            RestRequest request = new RestRequest($"/ENSEK/buy/{energyType}/{quantity}", Method.Put);
            RestResponse response = _client.Execute(request);
            JObject jsonContent = JObject.Parse(response.Content);

            string message = jsonContent["message"].ToString();

            Regex orderIdRegex = new Regex(@"Your order id is (\w+-\w+-\w+-\w+-\w+)");//Matches on the five words of the order
            return orderIdRegex.Match(message).Groups[1].Value;
        }
    }
}
