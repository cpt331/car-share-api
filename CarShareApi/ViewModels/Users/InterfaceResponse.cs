namespace CarShareApi.ViewModels.Users
{
    public class InterfaceResponse
    {
        //this allows the outcome of a request by passing the success/failure
        //of the action, a message that will be displayed to the user and
        //a message that will be displayed in the browser console

        public bool Success { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
}