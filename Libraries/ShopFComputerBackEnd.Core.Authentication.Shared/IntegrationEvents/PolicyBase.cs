namespace ShopFComputerBackEnd.Core.Authentication.Shared.IntegrationEvents
{
    public class PolicyBase
    {
        public PolicyBase()
        {
        }
        public PolicyBase(string serviceName, string functionName)
        {
            ServiceName = serviceName;
            FunctionName = functionName;
        }

        public string ServiceName { get; set; }
        public string FunctionName { get; set; }
    }

}
