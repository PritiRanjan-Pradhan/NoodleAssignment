namespace Noodle_Assignment_API.GraphQL
{
    using System.Collections.Generic;
    

    namespace Training.GraphQL
    {
        public class GraphQLResultData
        {
            public GraphCustomersResult Customers { get; set; }
        }
        public class GraphCustomersResult
        {
            public int Count { get; set; }
            public List<Customer> Results { get; set; }
        }
    }

}
