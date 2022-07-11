using RestSharp;
using JsonServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonServerMSTest
{
    [TestClass]
    public class ResdtSharpTestCases
    {

        //declaring restClient variable
        RestClient client;

        /// <summary>
        /// Setups this instance for the client by giving url along with port.
        /// </summary>

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:4000");
        }

        /// <summary>
        /// Gets the employee list in the form of restresponse
        /// </summary>
        /// <returns>RestResponse response</returns>
        private IRestResponse getEmployeeList()
        {
            //arrange
            //makes restrequest for getting all the data from json server by giving table name and method.get
            RestRequest request = new RestRequest("/employees", Method.GET);

            //act
            //executing the request using client and saving the result in IrestResponse.
            IRestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// Ons the calling get API return employee list.
        /// </summary>
        [TestMethod]
        public void onCallingGetApi_ReturnEmployeeList()
        {
            //gets the irest response from getemployee list method
            IRestResponse response = getEmployeeList();
            //assert
            //assert for checking status code of get
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            //adding the data into list from irestresponse by using deserializing.
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            //printing out the content for list of employee
            foreach (Employee employee in dataResponse)
            {
                Console.WriteLine("Id: " + employee.id + " Name: " + employee.name + " Salary: " + employee.salary);
            }
            //assert for checking count of no of element in list to be equal to data in jsonserver table.
            Assert.AreEqual(7, dataResponse.Count);
        }



        /// <summary>
        /// Givens the employee on post should return added employee. UC2
        /// </summary>
        [TestMethod]
        public void givenEmployee_OnPost_ShouldReturnAddedEmployee()
        {
            //arrange
            //adding request to post(add) data
            RestRequest request = new RestRequest("/employees", Method.POST);
            //instatiating jObject for adding data for name and salary, id auto increments
            JObject jObject = new JObject();
            jObject.Add("name", "Clark");
            jObject.Add("salary", "15000");
            //as parameters are passed as body hence "request body" call is made, in parameter type
            request.AddParameter("application/json", jObject, ParameterType.RequestBody);
            //Act
            //request contains method of post and along with added parameter which contains data to be added
            //hence response will contain the data which is added and not all the data from jsonserver.
            //data is added to json server json file in this step.
            IRestResponse response = client.Execute(request);
            //assert
            //code will be 201 for posting data
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            //derserializing object for assert and checking test case
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Clark", dataResponse.name);
            Assert.AreEqual("15000", dataResponse.salary);
            Console.WriteLine(response.Content);
        }



        /// <summary>
        /// Givens the multiple employee on post should return added employee. UC3
        /// </summary>
        [TestMethod]
        public void givenMultipleEmployee_OnPost_ShouldReturnAddedEmployee()
        {
            //adding multiple employees to table
            List<Employee> MultipleEmployeeList = new List<Employee>();
            MultipleEmployeeList.Add(new Employee { name = "Akshat", salary = "4000" });
            MultipleEmployeeList.Add(new Employee { name = "Vivek", salary = "5000" });
            MultipleEmployeeList.ForEach(employeeData =>
            {
                //arrange
                //adding request to post(add) data
                RestRequest request = new RestRequest("/employees", Method.POST);

                //instatiating jObject for adding data for name and salary, id auto increments
                JObject jObject = new JObject();
                jObject.Add("name", employeeData.name);
                jObject.Add("salary", employeeData.salary);
                //as parameters are passed as body hence "request body" call is made, in parameter type
                request.AddParameter("application/json", jObject, ParameterType.RequestBody);
                //Act
                //request contains method of post and along with added parameter which contains data to be added
                //hence response will contain the data which is added and not all the data from jsonserver.
                //data is added to json server json file in this step.
                IRestResponse response = client.Execute(request);
                //assert
                //code will be 201 for posting data
                Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
                //derserializing object for assert and checking test case
                Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(employeeData.name, dataResponse.name);
                Console.WriteLine(response.Content);
            });

        }



        /// <summary>
        /// Givens the employee on update should return updated employee. UC4
        /// </summary>
        [TestMethod]
        public void GivenEmployee_OnUpdate_ShouldReturnUpdatedEmployee()
        {
            //making a request for a particular employee to be updated
            RestRequest request = new RestRequest("employees/6", Method.PUT);
            //creating a jobject for new data to be added in place of old
            //json represents a new json object
            JObject jobject = new JObject();
            jobject.Add("name", "Akshat");
            jobject.Add("salary", 6000);
            //adding parameters in request
            //request body parameter type signifies values added using add.
            request.AddParameter("application/json", jobject, ParameterType.RequestBody);
            //executing request using client
            //IRest response act as a container for the data sent back from api.
            IRestResponse response = client.Execute(request);
            //checking status code of response
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            //deserializing content added in json file
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            //asserting for salary
            Assert.AreEqual(dataResponse.salary, "6000");
            //writing content without deserializing from resopnse. 
            Console.WriteLine(response.Content);
        }

    }
}