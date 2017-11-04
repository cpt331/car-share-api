using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using CarShareApi.Models;
using CarShareApi.Models.Services;
using CarShareApi.Models.ViewModels;
using CarShareApi.ViewModels;
using CarShareApi.ViewModels.Bookings;
using CarShareApi.ViewModels.Users;
using Newtonsoft.Json;
using NLog;

namespace CarShareApi.Controllers
{
    /// <summary>
    ///     Account controller
    /// </summary>
    [Authorize]
    public class AccountController : ApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IUserService UserService;

        //inject service to make testing easier
        public AccountController(IUserService userService)
        {
            UserService = userService;
        }


        /// <summary>
        ///     Return the current logged in user
        /// </summary>
        /// <returns>A view model of the user and also information about outstanding bookings</returns>
        [HttpGet]
        [Route("api/account/current")]
        public UserViewModel Current()
        {
            var userPrincipal = new UserPrincipal(ClaimsPrincipal.Current);
            if (userPrincipal.Id.HasValue)
            {
                var user = UserService.FindUser(userPrincipal.Id.Value);
                return user;
            }
            return null;
        }

        [HttpGet]
        [Route("api/account/bookings")]
        public BookingHistoryResponse Bookings(int pageNumber = 1, int pageSize = 10)
        {
            var userPrincipal = new UserPrincipal(ClaimsPrincipal.Current);
            if (userPrincipal.Id.HasValue)
            {
                var response = UserService.GetBookingHistory(userPrincipal.Id.Value, pageNumber, pageSize);
                return response;
            }
            return new BookingHistoryResponse
            {
                Success = false,
                Message = "No user is logged on."
            };
        }

        /// <summary>
        ///     Register a new user into the system
        /// </summary>
        /// <param name="request">The view model from the client</param>
        /// <returns>A response indicating success and relevant error messages</returns>
        [HttpPost]
        [Route("api/account/register")]
        [AllowAnonymous]
        public RegisterResponse Register(RegisterRequest request)
        {
            Logger.Debug("Register Request Received: {0}", JsonConvert.SerializeObject(request, Formatting.Indented));

            RegisterResponse response;
            //use in built data annotations to ensure model has binded correctly
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys.SelectMany(key => ModelState[key].Errors.Select(x => x.ErrorMessage));
                response = new RegisterResponse
                {
                    Success = false,
                    Message = "Form has validation errors",
                    Errors = errors.ToArray()
                };
            }
            else
            {
                //send request to the user service and return the response (success or fail)
                response = UserService.Register(request);
            }
            Logger.Debug("Sent Register Response: {0}",
                JsonConvert.SerializeObject(response, Formatting.Indented));
            return response;
        }

        [HttpGet]
        [Route("api/account/registerupdatereturn")]
        public RegisterViewModel Register()
        {
            var userPrincipal = new UserPrincipal(ClaimsPrincipal.Current);
            if (userPrincipal.Id.HasValue)
            {
                var response = UserService.GetRegistrationRecord(userPrincipal.Id.Value);
                return response;
            }
            return new RegisterViewModel
            {
                Success = false,
                Message = "No user is logged on."
            };
        }

        [HttpPost]
        [Route("api/account/registerupdate")]
        public InterfaceResponse UpdateRegister(RegisterUpdateRequest request)
        {
            Logger.Debug("Registration details received: {0}",
                JsonConvert.SerializeObject(request, Formatting.Indented));

            var userPrincipal = new UserPrincipal(ClaimsPrincipal.Current);
            if (userPrincipal.Id.HasValue)
            {
                InterfaceResponse response;
                //use in built data annotations to ensure model has binded correctly
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Keys.SelectMany(key => ModelState[key].Errors.Select(x => x.ErrorMessage));
                    response = new InterfaceResponse
                    {
                        Success = false,
                        Message = "Form has validation errors",
                        Errors = errors.ToArray()
                    };
                }
                else
                {
                    //send request to the user service and return the response (success or fail)
                    response = UserService.UpdateRegistration(request, userPrincipal.Id.Value);
                }
                Logger.Debug("Registration update sent Response: {0}",
                    JsonConvert.SerializeObject(response, Formatting.Indented));
                return response;
            }
            else
            {
                var response = new InterfaceResponse
                {
                    Success = false,
                    Message = "Invalid user ID",
                    Errors = new[] {"No user is logged on"}
                };
                Logger.Debug("The user ID session is invalid",
                    JsonConvert.SerializeObject(response, Formatting.Indented));
                return response;
            }
        }


        [HttpPost]
        [Route("api/account/paymentmethod")]
        public AddPaymentMethodResponse AddPaymentMethod(AddPaymentMethodRequest request)
        {
            Logger.Debug("Payment Method Request Received: {0}",
                JsonConvert.SerializeObject(request, Formatting.Indented));

            var userPrincipal = new UserPrincipal(ClaimsPrincipal.Current);
            if (userPrincipal.Id.HasValue)
            {
                AddPaymentMethodResponse response;
                //use in built data annotations to ensure model has binded correctly
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Keys.SelectMany(key => ModelState[key].Errors.Select(x => x.ErrorMessage));
                    response = new AddPaymentMethodResponse
                    {
                        Success = false,
                        Message = "Form has validation errors",
                        Errors = errors.ToArray()
                    };
                }
                else
                {
                    //send request to the user service and return the response (success or fail)
                    response = UserService.AddPaymentMethod(request, userPrincipal.Id.Value);
                }
                Logger.Debug("Sent Payment Method Response: {0}",
                    JsonConvert.SerializeObject(response, Formatting.Indented));
                return response;
            }
            else
            {
                var response = new AddPaymentMethodResponse
                {
                    Success = false,
                    Message = "Invalid user ID",
                    Errors = new[] {"No user is logged on"}
                };
                Logger.Debug("The user ID session is invalid",
                    JsonConvert.SerializeObject(response, Formatting.Indented));
                return response;
            }
        }

        [HttpPost]
        [Route("api/account/passwordreset")]
        [AllowAnonymous]
        public PasswordResetResponse PasswordReset(PasswordResetRequest request)
        {
            Logger.Debug("Password Reset Request Received: {0}",
                JsonConvert.SerializeObject(request, Formatting.Indented));

            PasswordResetResponse response;
            //use in built data annotations to ensure model has binded correctly
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys.SelectMany(key => ModelState[key].Errors.Select(x => x.ErrorMessage));
                response = new PasswordResetResponse
                {
                    Success = false,
                    Message = "Form has validation errors",
                    Errors = errors.ToArray()
                };
            }
            else
            {
                //send request to the user service and return the response (success or fail)
                response = UserService.ResetPassword(request);
            }
            Logger.Debug("Sent Password Reset Response: {0}",
                JsonConvert.SerializeObject(response, Formatting.Indented));
            return response;
        }


        [HttpPost]
        [Route("api/account/otp")]
        [AllowAnonymous]
        public OTPResponse OtpActivation(OTPRequest request)
        {
            Logger.Debug("OTP Activation request received: {0}",
                JsonConvert.SerializeObject(request, Formatting.Indented));


            OTPResponse response;
            //use in built data annotations to ensure model has binded correctly
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys.SelectMany(key => ModelState[key].Errors.Select(x => x.ErrorMessage));
                response = new OTPResponse
                {
                    Success = false,
                    Message = "Form has validation errors",
                    Errors = errors.ToArray()
                };
            }
            else
            {
                //send request to the user service and return the response (success or fail)
                response = UserService.OtpActivation(request);
            }
            Logger.Debug("Sent OTP Response: {0}",
                JsonConvert.SerializeObject(response, Formatting.Indented));
            return response;
        }
    }
}